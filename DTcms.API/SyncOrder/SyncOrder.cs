using DTcms.Common;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;

namespace DTcms.API.SyncOrder
{
    public class SyncOrder
    {
        public static void SyncOrderFromMeituan()
        {
            //Model.siteconfig modelConfig = new BLL.siteconfig().loadConfig(Utils.GetXmlMapPath(DTKeys.FILE_SITE_XML_CONFING));
            List<BookingFood.Model.bf_area> list = new BookingFood.BLL.bf_area().GetModelList("");
            foreach (var itemArea in list)
            {
                if (string.IsNullOrEmpty(itemArea.MeituanCookie)) continue;
                string url = string.Format("http://e.waimai.meituan.com/v2/order/history/r/query?getNewVo=1&wmOrderPayType=-2&wmOrderStatus=-2&sortField=1&startDate={0}&endDate={0}&pageNum=1"
                , DateTime.Now.ToString("yyyy-MM-dd"));
                string cookie = itemArea.MeituanCookie;
                string json = Html.GetHtmlWithSimpleCookie(url, "http://e.waimai.meituan.com/v2/order/history", cookie);
                JObject jsonObj = JObject.Parse(json);
                Model.users modelUser = null;
                BLL.users bllUsers = new BLL.users();
                BLL.orders bllOrders = new BLL.orders();
                string telphone = string.Empty, address = string.Empty, nickname = string.Empty, message = string.Empty, position = string.Empty, areaTitle = string.Empty;
                string orderno = string.Empty, dispatch = string.Empty;
                decimal total_after = 0, total_before = 0, shipping_fee = 0;
                int areaId = 0, inArea = 0, outArea = 0;
                BookingFood.BLL.bf_area bllArea = new BookingFood.BLL.bf_area();
                DataTable dtArea = bllArea.GetList(" IsShow=1 AND ParentId=1 Order By SortId Asc").Tables[0];
                try
                {
                    foreach (var item in jsonObj["wmOrderList"])
                    {
                        orderno = "MT" + item["wm_order_id_view_str"].ToString();
                        if (bllOrders.GetCount(" order_no='" + orderno + "'") > 0) continue;
                        telphone = item["recipient_phone"].ToString();
                        address = item["recipient_address"].ToString();
                        nickname = item["recipient_name"].ToString();
                        message = item["remark"].ToString();
                        dispatch = item["wm_order_pay_type"].ToString();
                        position = (decimal.Parse(item["address_latitude"].ToString()) / 1000000).ToString()
                                    + "," + (decimal.Parse(item["address_longitude"].ToString()) / 1000000).ToString();
                        total_before = decimal.Parse(item["total_before"].ToString());
                        total_after = decimal.Parse(item["total_after"].ToString());
                        shipping_fee = decimal.Parse(item["shipping_fee"].ToString());

                        #region 用户信息处理
                        modelUser = bllUsers.GetModel("mei" + item["user_id"].ToString());
                        if (modelUser == null)
                        {
                            modelUser = new Model.users();
                            Model.user_groups modelGroup = new BLL.user_groups().GetDefault();
                            modelUser.group_id = modelGroup.id;
                            modelUser.user_name = "mei" + item["user_id"].ToString();
                            modelUser.nick_name = nickname;
                            modelUser.password = DESEncrypt.Encrypt("111111");
                            modelUser.email = "register@meituan.com";
                            modelUser.telphone = telphone;
                            modelUser.address = address;
                            modelUser.reg_time = DateTime.Now;
                            modelUser.is_lock = 0; //设置为对应状态
                            int newId = bllUsers.Add(modelUser);
                            modelUser.id = newId;
                        }
                        else
                        {
                            modelUser.email = "register@meituan.com";
                            modelUser.address = address;
                            bllUsers.Update(modelUser);
                        }
                        #endregion

                        #region 区域判断
                        //foreach (DataRow itemArea in dtArea.Rows)
                        //{
                        //    if (string.IsNullOrEmpty(itemArea["DistributionArea"].ToString())) continue;
                        //    bool isInArea = Polygon.GetResult(position, itemArea["DistributionArea"].ToString());
                        //    if (isInArea)
                        //    {
                        //        areaTitle = itemArea["Title"].ToString();
                        //        areaId = int.Parse(itemArea["Id"].ToString());
                        //        break;
                        //    }
                        //    else
                        //    {
                        //        if (!string.IsNullOrEmpty(itemArea["DistributionArea_2"].ToString()))
                        //        {
                        //            isInArea = Polygon.GetResult(position, itemArea["DistributionArea_2"].ToString());
                        //            if (isInArea)
                        //            {
                        //                areaTitle = itemArea["Title"].ToString();
                        //                areaId = int.Parse(itemArea["Id"].ToString());
                        //                break;
                        //            }
                        //        }

                        //    }
                        //}
                        //if (areaId == 0)
                        //{
                        //    areaTitle = "公共区域";
                        //    areaId = 18;
                        //    outArea++;
                        //}
                        //else
                        //{
                        //    inArea++;
                        //}
                        inArea++;
                        areaId = itemArea.Id;
                        areaTitle = itemArea.Title;
                        #endregion

                        #region 保存订单
                        Model.orders model = new Model.orders();
                        model.order_no = orderno; //订单号
                        model.user_id = modelUser.id;
                        model.user_name = modelUser.user_name;
                        model.distribution_id = 1;
                        model.accept_name = modelUser.nick_name;
                        model.post_code = "";
                        model.telphone = modelUser.telphone;
                        model.mobile = "";
                        model.email = modelUser.email;
                        model.address = address + " " + model.accept_name;
                        model.message = message;

                        model.payable_amount = total_before;
                        model.real_amount = total_after;

                        model.area_id = areaId;
                        model.area_title = areaTitle;
                        model.OrderType = "电话";
                        model.is_additional = 0;

                        model.payment_fee = 0;
                        if (dispatch == "1")
                        {
                            model.payment_status = 11;
                            model.payment_id = 1;
                        }
                        else if (dispatch == "2")
                        {
                            model.payment_status = 12;
                            model.payment_id = 2;
                            model.payment_time = DateTime.Now;
                        }

                        model.payable_freight = shipping_fee; //应付运费
                        model.real_freight = shipping_fee; //实付运费

                        //订单总金额=实付商品金额+运费+支付手续费
                        model.order_amount = model.real_amount;
                        //购物积分,可为负数
                        model.point = 0;
                        model.add_time = DateTime.Parse(item["order_time_fmt"].ToString());
                        //商品详细列表
                        List<Model.order_goods> gls = new List<Model.order_goods>();
                        foreach (var item1 in item["cartDetailVos"][0]["details"])
                        {
                            gls.Add(new Model.order_goods
                            {
                                goods_id = 0,
                                goods_name = item1["food_name"].ToString(),
                                goods_price = decimal.Parse(item1["food_price"].ToString()),
                                real_price = decimal.Parse(item1["food_price"].ToString()),
                                quantity = int.Parse(item1["count"].ToString()),
                                point = 0
                                ,
                                type = "one",
                                subgoodsid = string.Empty,
                                category_title = string.Empty
                            });
                        }
                        model.order_goods = gls;
                        model.status = 1;
                        model.restore_status = 1;
                        int result = bllOrders.Add(model);
                        #endregion

                        #region 更新后厨推送数据
                        BLL.article bllArticle = new BLL.article();
                        BookingFood.Model.bf_back_door back = null;
                        BookingFood.BLL.bf_good_nickname bllNick = new BookingFood.BLL.bf_good_nickname();
                        BookingFood.Model.bf_good_nickname nickModel = null;
                        List<BookingFood.Model.bf_back_door> listBack = new List<BookingFood.Model.bf_back_door>();
                        foreach (var item2 in gls)
                        {
                            back = new BookingFood.Model.bf_back_door()
                            {
                                OrderId = result,
                                GoodsCount = item2.quantity,
                                CategoryId = 0,
                                AreaId = model.area_id,
                                IsDown = 0,
                                Taste = !string.IsNullOrEmpty(item2.subgoodsid) ? item2.subgoodsid.Split('‡')[2] : "",
                                Freight = "外卖"
                            };
                            back.GoodsName = item2.goods_name;
                            listBack.Add(back);
                        }
                        BookingFood.BLL.bf_back_door bllBack = new BookingFood.BLL.bf_back_door();
                        foreach (var item2 in listBack)
                        {
                            bllBack.Add(item2);
                        }
                        #endregion

                    }
                }
                catch (Exception ex)
                {
                    //Log.Info("美团轮训区域:" + itemArea.Title + " 报错:" + ex.Message + " " + ex.Source.ToString());
                    //Model.siteconfig siteConfig = new BLL.siteconfig().loadConfig(DTcms.Common.Utils.GetXmlMapPath(DTcms.Common.DTKeys.FILE_SITE_XML_CONFING));
                    //DTcms.Common.DTMail.sendMail(siteConfig.emailstmp,
                    //    siteConfig.emailusername,
                    //    DTcms.Common.DESEncrypt.Decrypt(siteConfig.emailpassword),
                    //    siteConfig.emailnickname,
                    //    siteConfig.emailfrom,
                    //    "frank3660@msn.com",
                    //    "馍王报错_" + ex.Message, ex.Source.ToString());
                }
                
            }
            //http://e.waimai.meituan.com/v2/order/history/r/query?getNewVo=1&wmOrderPayType=-2&wmOrderStatus=-2&sortField=1&startDate=2016-07-14&endDate=2016-07-14&pageNum=1
            
        }

        public static void SyncOrderFromBaidu()
        {
            //BLL.siteconfig bllConfig = new BLL.siteconfig();
            //Model.siteconfig modelConfig = bllConfig.loadConfig(Utils.GetXmlMapPath(DTKeys.FILE_SITE_XML_CONFING));
            List<BookingFood.Model.bf_area> list = new BookingFood.BLL.bf_area().GetModelList("");
            foreach (var itemArea in list)
            {
                if (string.IsNullOrEmpty(itemArea.BaiduCookie)) continue;
                //http://wmcrm.baidu.com/crm?qt=orderlist&order_status=0&start_time=2016-07-14&end_time=2016-07-14&pay_type=2&is_asap=0
                string url = string.Format("http://wmcrm.baidu.com/crm?qt=orderlist&order_status=0&start_time={0}&end_time={0}&pay_type=2&is_asap=0"
                    , DateTime.Now.ToString("yyyy-MM-dd"));
                string cookie = itemArea.BaiduCookie;
                string html = Common.Html.GetHtmlWithSimpleCookie(url, string.Empty, cookie);
                if (html.IndexOf("require(\"wand:widget/order/filter/filter.js\").createWidget({content:") == -1)
                {
                    //cookie = RefreshCookie("baidu", cookie);
                    //if (string.IsNullOrEmpty(cookie)) return;
                    //modelConfig.BaiduCookie = cookie;
                    //bllConfig.saveConifg(modelConfig, Utils.GetXmlMapPath(DTKeys.FILE_SITE_XML_CONFING));
                    //html = Common.Html.GetHtmlWithSimpleCookie(url, string.Empty, cookie);
                }
                int startPos = html.IndexOf("require(\"wand:widget/order/filter/filter.js\").createWidget({content:") + "require(\"wand:widget/order/filter/filter.js\").createWidget({content:".Length;
                int endPos = html.IndexOf("});", startPos);
                html = html.Substring(startPos, endPos - startPos);
                JObject jsonObj = JObject.Parse(html);
                Model.users modelUser = null;
                BLL.users bllUsers = new BLL.users();
                BLL.orders bllOrders = new BLL.orders();
                string telphone = string.Empty, address = string.Empty, nickname = string.Empty, message = string.Empty, position = string.Empty, areaTitle = string.Empty;
                string orderno = string.Empty, dispatch = string.Empty;
                decimal total_after = 0, total_before = 0, shipping_fee = 0;
                int areaId = 0, inArea = 0, outArea = 0;
                BookingFood.BLL.bf_area bllArea = new BookingFood.BLL.bf_area();
                DataTable dtArea = bllArea.GetList(" IsShow=1 AND ParentId=1 Order By SortId Asc").Tables[0];
                try
                {
                    foreach (var item in jsonObj["order_list"])
                    {
                        orderno = "BD" + item["order_basic"]["order_id"].ToString();
                        if (bllOrders.GetCount(" order_no='" + orderno + "'") > 0) continue;
                        telphone = item["order_basic"]["user_phone"].ToString();
                        address = item["order_basic"]["user_address"].ToString();
                        nickname = item["order_basic"]["user_real_name"].ToString();
                        message = item["order_basic"]["user_note"].ToString();
                        //dispatch = item["wm_order_pay_type"].ToString();
                        dispatch = "2";
                        total_before = decimal.Parse(item["order_total"]["customer_price"].ToString());
                        total_after = decimal.Parse(item["order_total"]["customer_price"].ToString());
                        shipping_fee = 0;

                        #region 用户信息处理
                        modelUser = bllUsers.GetModel("bai" + item["order_basic"]["user_id"].ToString());
                        if (modelUser == null)
                        {
                            modelUser = new Model.users();
                            Model.user_groups modelGroup = new BLL.user_groups().GetDefault();
                            modelUser.group_id = modelGroup.id;
                            modelUser.user_name = "bai" + item["order_basic"]["user_id"].ToString();
                            modelUser.nick_name = nickname;
                            modelUser.password = DESEncrypt.Encrypt("111111");
                            modelUser.email = "register@baidu.com";
                            modelUser.telphone = telphone;
                            modelUser.address = address;
                            modelUser.reg_time = DateTime.Now;
                            modelUser.is_lock = 0; //设置为对应状态
                            int newId = bllUsers.Add(modelUser);
                            modelUser.id = newId;
                        }
                        else
                        {
                            modelUser.email = "register@baidu.com";
                            modelUser.address = address;
                            bllUsers.Update(modelUser);
                        }
                        #endregion

                        areaTitle = itemArea.Title;
                        areaId = itemArea.Id;
                        inArea++;

                        #region 保存订单
                        Model.orders model = new Model.orders();
                        model.order_no = orderno; //订单号
                        model.user_id = modelUser.id;
                        model.user_name = modelUser.user_name;
                        model.distribution_id = 1;
                        model.accept_name = modelUser.nick_name;
                        model.post_code = "";
                        model.telphone = modelUser.telphone;
                        model.mobile = "";
                        model.email = modelUser.email;
                        model.address = address + " " + model.accept_name;
                        model.message = message;

                        model.payable_amount = total_before;
                        model.real_amount = total_after;

                        model.area_id = areaId;
                        model.area_title = areaTitle;
                        model.OrderType = "电话";
                        model.is_additional = 0;

                        model.payment_fee = 0;
                        if (dispatch == "1")
                        {
                            model.payment_status = 11;
                            model.payment_id = 1;
                        }
                        else if (dispatch == "2")
                        {
                            model.payment_status = 12;
                            model.payment_id = 2;
                            model.payment_time = DateTime.Now;
                        }

                        model.payable_freight = shipping_fee; //应付运费
                        model.real_freight = shipping_fee; //实付运费

                        //订单总金额=实付商品金额+运费+支付手续费
                        model.order_amount = model.real_amount + model.real_freight + model.payment_fee;
                        //购物积分,可为负数
                        model.point = 0;
                        model.add_time = DateTime.Now;
                        //商品详细列表
                        List<Model.order_goods> gls = new List<Model.order_goods>();
                        foreach (var item1 in item["order_goods"]["goods_list"])
                        {
                            gls.Add(new Model.order_goods
                            {
                                goods_id = 0,
                                goods_name = item1["name"].ToString(),
                                goods_price = decimal.Parse(item1["customer_price"].ToString()),
                                real_price = decimal.Parse(item1["customer_price"].ToString()),
                                quantity = int.Parse(item1["number"].ToString()),
                                point = 0,
                                type = "one",
                                subgoodsid = string.Empty,
                                category_title = string.Empty
                            });
                        }
                        model.order_goods = gls;
                        model.status = 1;
                        model.restore_status = 1;
                        int result = bllOrders.Add(model);
                        #endregion

                        #region 更新后厨推送数据
                        BLL.article bllArticle = new BLL.article();
                        BookingFood.Model.bf_back_door back = null;
                        BookingFood.BLL.bf_good_nickname bllNick = new BookingFood.BLL.bf_good_nickname();
                        BookingFood.Model.bf_good_nickname nickModel = null;
                        List<BookingFood.Model.bf_back_door> listBack = new List<BookingFood.Model.bf_back_door>();
                        foreach (var item2 in gls)
                        {
                            back = new BookingFood.Model.bf_back_door()
                            {
                                OrderId = result,
                                GoodsCount = item2.quantity,
                                CategoryId = 0,
                                AreaId = model.area_id,
                                IsDown = 0,
                                Taste = !string.IsNullOrEmpty(item2.subgoodsid) ? item2.subgoodsid.Split('‡')[2] : "",
                                Freight = "外卖"
                            };
                            back.GoodsName = item2.goods_name;
                            listBack.Add(back);
                        }
                        BookingFood.BLL.bf_back_door bllBack = new BookingFood.BLL.bf_back_door();
                        foreach (var item2 in listBack)
                        {
                            bllBack.Add(item2);
                        }
                        #endregion

                    }
                }
                catch (Exception ex)
                {
                    //Log.Info("百度轮训区域:" + itemArea.Title + " 报错:" + ex.Message + " " + ex.Source.ToString());
                    //Model.siteconfig siteConfig = new BLL.siteconfig().loadConfig(DTcms.Common.Utils.GetXmlMapPath(DTcms.Common.DTKeys.FILE_SITE_XML_CONFING));
                    //DTcms.Common.DTMail.sendMail(siteConfig.emailstmp,
                    //    siteConfig.emailusername,
                    //    DTcms.Common.DESEncrypt.Decrypt(siteConfig.emailpassword),
                    //    siteConfig.emailnickname,
                    //    siteConfig.emailfrom,
                    //    "frank3660@msn.com",
                    //    "馍王报错_" + ex.Message, ex.Source.ToString());
                }
                
            }
            
        }


        public class ElemeToken
        {
            public string Token { get; set; }
            public DateTime ExpireTime { get; set; }
            public string ShopId { get; set; }
            public string Key { get; set; }
            public string Secret { get; set; }

        }

        public static void SyncOrderFromEleme()
        {
            BLL.siteconfig bllConfig = new BLL.siteconfig();
            Model.siteconfig modelConfig = bllConfig.loadConfig(Utils.GetXmlMapPath(DTKeys.FILE_SITE_XML_CONFING));
            BookingFood.BLL.bf_area bllArea = new BookingFood.BLL.bf_area();
            List<BookingFood.Model.bf_area> list = bllArea.GetModelList("ElemeCookie!=''");
            string html = string.Empty;
            ElemeToken elemeToken = new API.SyncOrder.SyncOrder.ElemeToken();
            foreach (var itemArea in list)
            {
                elemeToken = GetElemeToken(itemArea);
                if (elemeToken == null) continue;
                //获取数据
                string unixstamp = GetUnixStamp();
                string json = PostData("https://open-api.shop.ele.me/api/v1/"
                    , "{\"token\":\"" + elemeToken.Token + "\",\"nop\":\"1.0.0\",\"metas\":{\"app_key\":\"" + elemeToken.Key + "\",\"timestamp\":" + unixstamp
                    + "},\"params\":{\"shopId\":" + elemeToken.ShopId + ",\"pageNo\":1,\"pageSize\":50,\"date\":\"" + DateTime.Now.ToString("yyyy-MM-dd")
                    + "\"},\"action\":\"eleme.order.getAllOrders\",\"id\":\"" + Guid.NewGuid().ToString() + "\",\"signature\":\"" +
                    GetMd5("eleme.order.getAllOrders" + elemeToken.Token + "app_key=\"" + elemeToken.Key + "\"date=\""+ DateTime.Now.ToString("yyyy-MM-dd") + "\"pageNo=1pageSize=50shopId="+elemeToken.ShopId+"timestamp=" + unixstamp + elemeToken.Secret).ToUpper()  
                    + "\"}");
                JObject jsonObj = JObject.Parse(json);
                Model.users modelUser = null;
                BLL.users bllUsers = new BLL.users();
                BLL.orders bllOrders = new BLL.orders();
                string telphone = string.Empty, address = string.Empty, nickname = string.Empty, message = string.Empty, position = string.Empty, areaTitle = string.Empty;
                string orderno = string.Empty, dispatch = string.Empty, invoice=string.Empty, book=string.Empty;
                DateTime deliverTime=new DateTime ();
                decimal total_after = 0, total_before = 0, shipping_fee = 0;
                int areaId = 0, inArea = 0, outArea = 0;
                DataTable dtArea = bllArea.GetList(" IsShow=1 AND ParentId=1 Order By SortId Asc").Tables[0];
                try
                {
                    foreach (var item in jsonObj["result"]["list"])
                    {
                        if (item["status"].ToString() == "invalid") continue;
                        orderno = "EL" + item["id"].ToString();
                        if (bllOrders.GetCount(" order_no='" + orderno + "'") > 0) continue;
                        if(item["phoneList"].Count()>0)
                        {
                            telphone = item["phoneList"][0].ToString();
                        }
                        
                        address = item["address"].ToString();
                        nickname = item["consignee"].ToString();
                        message = item["description"].ToString();
                        book = item["book"].ToString().ToLower();
                        if(string.Equals(book,"true"))
                        {
                            deliverTime = DateTime.Parse(item["deliverTime"].ToString());
                            message += " [预]" + (deliverTime.Date == DateTime.Now.Date ? "今日" : deliverTime.ToString("MM月dd日")) + deliverTime.ToString("HH时mm分") + "送达";
                        }
                        invoice = item["invoice"].ToString();
                        if(!string.IsNullOrEmpty(invoice))
                        {
                            message += " [发票]" + invoice;
                        }
                        dispatch = "2";
                        //position = (decimal.Parse(item["consigneeGeoLocation"]["latitude"].ToString())).ToString()
                        //            + "," + (decimal.Parse(item["consigneeGeoLocation"]["longitude"].ToString())).ToString();
                        total_before = decimal.Parse(item["totalPrice"].ToString());
                        total_after = decimal.Parse(item["income"].ToString());
                        

                        shipping_fee = 0;

                        #region 用户信息处理
                        modelUser = bllUsers.GetModel("ele" + telphone);
                        if (modelUser == null)
                        {
                            modelUser = new Model.users();
                            Model.user_groups modelGroup = new BLL.user_groups().GetDefault();
                            modelUser.group_id = modelGroup.id;
                            modelUser.user_name = "ele" + telphone;
                            modelUser.nick_name = nickname;
                            modelUser.password = DESEncrypt.Encrypt("111111");
                            modelUser.email = "register@eleme.com";
                            modelUser.telphone = telphone;
                            modelUser.address = address;
                            modelUser.reg_time = DateTime.Now;
                            modelUser.is_lock = 0; //设置为对应状态
                            int newId = bllUsers.Add(modelUser);
                            modelUser.id = newId;
                        }
                        else
                        {
                            modelUser.email = "register@eleme.com";
                            modelUser.address = address;
                            bllUsers.Update(modelUser);
                        }
                        #endregion

                        #region 区域判断
                        //foreach (DataRow itemArea in dtArea.Rows)
                        //{
                        //    if (string.IsNullOrEmpty(itemArea["DistributionArea"].ToString())) continue;
                        //    bool isInArea = Polygon.GetResult(position, itemArea["DistributionArea"].ToString());
                        //    if (isInArea)
                        //    {
                        //        areaTitle = itemArea["Title"].ToString();
                        //        areaId = int.Parse(itemArea["Id"].ToString());
                        //        break;
                        //    }
                        //    else
                        //    {
                        //        if (!string.IsNullOrEmpty(itemArea["DistributionArea_2"].ToString()))
                        //        {
                        //            isInArea = Polygon.GetResult(position, itemArea["DistributionArea_2"].ToString());
                        //            if (isInArea)
                        //            {
                        //                areaTitle = itemArea["Title"].ToString();
                        //                areaId = int.Parse(itemArea["Id"].ToString());
                        //                break;
                        //            }
                        //        }

                        //    }
                        //}
                        //if (areaId == 0)
                        //{
                        //    areaTitle = "公共区域";
                        //    areaId = 18;
                        //    outArea++;
                        //}
                        //else
                        //{
                        //    inArea++;
                        //}
                        areaTitle = itemArea.Title;
                        areaId = itemArea.Id;
                        inArea++;
                        #endregion

                        #region 保存订单
                        Model.orders model = new Model.orders();
                        model.order_no = orderno; //订单号
                        model.user_id = modelUser.id;
                        model.user_name = modelUser.user_name;
                        model.distribution_id = 1;
                        model.accept_name = modelUser.nick_name;
                        model.post_code = "";
                        model.telphone = modelUser.telphone;
                        model.mobile = "";
                        model.email = modelUser.email;
                        model.address = address + " " + model.accept_name;
                        model.message = message;

                        model.payable_amount = total_before;
                        model.real_amount = total_after;

                        model.area_id = areaId;
                        model.area_title = areaTitle;
                        model.OrderType = "电话";
                        model.is_additional = 0;

                        model.payment_fee = 0;
                        if (dispatch == "1")
                        {
                            if(modelConfig.SyncOrderToDownload==1)
                            {
                                model.payment_status = 1;
                            }
                            else
                            {
                                model.payment_status = 11;
                            }
                            
                            model.payment_id = 1;
                        }
                        else if (dispatch == "2")
                        {
                            if (modelConfig.SyncOrderToDownload == 1)
                            {
                                model.payment_status = 2;
                            }
                            else
                            {
                                model.payment_status = 12;
                            }
                                
                            model.payment_id = 2;
                            model.payment_time = DateTime.Now;
                        }

                        model.payable_freight = shipping_fee; //应付运费
                        model.real_freight = shipping_fee; //实付运费

                        //订单总金额=实付商品金额+运费+支付手续费
                        model.order_amount = total_after;
                        //购物积分,可为负数
                        model.point = 0;

                        model.add_time = DateTime.Parse(item["createdAt"].ToString());
                        //商品详细列表
                        List<Model.order_goods> gls = new List<Model.order_goods>();
                        foreach (var item1 in item["groups"])
                        {
                            if (item1["name"].ToString().IndexOf("篮子") >= 0)
                            {
                                foreach (var item2 in item1["items"])
                                {
                                    gls.Add(new Model.order_goods
                                    {
                                        goods_id = 0,
                                        goods_name = item2["name"].ToString(),
                                        goods_price = decimal.Parse(item2["price"].ToString()),
                                        real_price = decimal.Parse(item2["price"].ToString()),
                                        quantity = int.Parse(item2["quantity"].ToString()),
                                        point = 0,
                                        type = "one",
                                        subgoodsid = string.Empty,
                                        category_title = string.Empty
                                    });
                                }

                            }

                        }
                        model.order_goods = gls;
                        model.status = 1;
                        model.restore_status = 1;
                        int result = bllOrders.Add(model);
                        #endregion

                        #region 更新后厨推送数据
                        BLL.article bllArticle = new BLL.article();
                        BookingFood.Model.bf_back_door back = null;
                        BookingFood.BLL.bf_good_nickname bllNick = new BookingFood.BLL.bf_good_nickname();
                        BookingFood.Model.bf_good_nickname nickModel = null;
                        List<BookingFood.Model.bf_back_door> listBack = new List<BookingFood.Model.bf_back_door>();
                        foreach (var item2 in gls)
                        {
                            back = new BookingFood.Model.bf_back_door()
                            {
                                OrderId = result,
                                GoodsCount = item2.quantity,
                                CategoryId = 0,
                                AreaId = model.area_id,
                                IsDown = 0,
                                Taste = !string.IsNullOrEmpty(item2.subgoodsid) ? item2.subgoodsid.Split('‡')[2] : "",
                                Freight = "外卖"
                            };
                            back.GoodsName = item2.goods_name;
                            listBack.Add(back);
                        }
                        BookingFood.BLL.bf_back_door bllBack = new BookingFood.BLL.bf_back_door();
                        foreach (var item2 in listBack)
                        {
                            bllBack.Add(item2);
                        }
                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    //Log.Info("饿了么轮训区域:" + itemArea.Title + " 报错:" + ex.Message + " " + ex.Source.ToString());
                    //Model.siteconfig siteConfig = new BLL.siteconfig().loadConfig(DTcms.Common.Utils.GetXmlMapPath(DTcms.Common.DTKeys.FILE_SITE_XML_CONFING));
                    //DTcms.Common.DTMail.sendMail(siteConfig.emailstmp,
                    //    siteConfig.emailusername,
                    //    DTcms.Common.DESEncrypt.Decrypt(siteConfig.emailpassword),
                    //    siteConfig.emailnickname,
                    //    siteConfig.emailfrom,
                    //    "frank3660@msn.com",
                    //    "馍王报错_" + ex.Message, ex.Source.ToString());
                }
                
            }

            
        }

        private static string RefreshCookie(string site,string cookie)
        {
            CookieContainer cookieContainer = new CookieContainer();
            CookieCollection cookies = null;
            switch (site)
            {
                case "baidu":
                    foreach (var item in cookie.Split(';'))
                    {
                        if (string.IsNullOrEmpty(item)) continue;
                        cookieContainer.Add(new Cookie(item.Split('=')[0].Trim(), item.Split('=')[1].Trim(), "/", ".baidu.com"));
                    }
                    cookies = GetRtnCookie("http://wmcrm.baidu.com/crm?qt=orderlist", string.Empty, cookieContainer);
                    break;
            }
            string new_cookie = string.Empty;
            foreach (Cookie item in cookies)
            {
                new_cookie += item.Name + "=" + item.Value + ";";
            }
            return new_cookie;
            //foreach (var item in cookie.Split(';'))
            //{
            //    if (cookie.IndexOf(item.Split('=')[0].Trim() + "=") >= 0) continue;
            //    cookie += item + ";";
            //}
        }

        private static CookieCollection GetRtnCookie(string url, string referer, CookieContainer cookie)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, sdch");
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "zh-CN,zh;q=0.8");
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.2; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/32.0.1667.0 Safari/537.36";
            if (cookie != null)
            {
                request.CookieContainer = cookie;
            }
            if (!string.IsNullOrEmpty(referer))
            {
                request.Referer = referer;
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            return response.Cookies;
        }

        private static string PostData(string url, string postdata, string referer, string cookie, string shopid)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json;charset=UTF-8";
            request.ContentLength = Encoding.UTF8.GetByteCount(postdata);
            if (cookie != null)
            {
                request.Headers.Add(HttpRequestHeader.Cookie, cookie);
            }
            request.Headers.Add("X-Shard", "shopid="+ shopid);
            if (!string.IsNullOrEmpty(referer))
            {
                request.Referer = referer;
            }
            Stream myRequestStream = request.GetRequestStream();
            StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("gb2312"));
            myStreamWriter.Write(postdata);
            myStreamWriter.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            if (response.ContentEncoding.ToLower().Contains("gzip"))
                myResponseStream = new GZipStream(myResponseStream, CompressionMode.Decompress);
            else if (response.ContentEncoding.ToLower().Contains("deflate"))
                myResponseStream = new DeflateStream(myResponseStream, CompressionMode.Decompress);
            string html = string.Empty;
            using (System.IO.StreamReader reader = new System.IO.StreamReader(myResponseStream, Encoding.GetEncoding("UTF-8")))
            {
                html = reader.ReadToEnd();
            }
            myResponseStream.Close();
            return html;
        }

        private static DateTime ConvertUnixStampToDateTime(double d)
        {
            System.DateTime time = System.DateTime.MinValue;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            time = startTime.AddSeconds(d);
            return time;
        }

        private static string GetValueByTag(string html, string starttag, string endtag)
        {
            if (html.IndexOf(starttag) == -1) return string.Empty;
            int startpox = html.IndexOf(starttag) + starttag.Length;
            int endpox = html.IndexOf(endtag, startpox);
            if (startpox > 0 && endpox > 0)
            {
                return html.Substring(startpox, html.IndexOf(endtag, startpox) - startpox);
            }
            return string.Empty;
        }

        public static string PostData(string url, string postdata)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json;charset=UTF-8";
            request.ContentLength = Encoding.UTF8.GetByteCount(postdata);
            Stream myRequestStream = request.GetRequestStream();
            StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("gb2312"));
            myStreamWriter.Write(postdata);
            myStreamWriter.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            if (response.ContentEncoding.ToLower().Contains("gzip"))
                myResponseStream = new GZipStream(myResponseStream, CompressionMode.Decompress);
            else if (response.ContentEncoding.ToLower().Contains("deflate"))
                myResponseStream = new DeflateStream(myResponseStream, CompressionMode.Decompress);
            string html = string.Empty;
            using (System.IO.StreamReader reader = new System.IO.StreamReader(myResponseStream, Encoding.GetEncoding("UTF-8")))
            {
                html = reader.ReadToEnd();
            }
            myResponseStream.Close();
            return html;
        }

        private static string PostDataByToken(string url, string postdata, string auth)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json;charset=UTF-8";
            request.ContentLength = Encoding.UTF8.GetByteCount(postdata);
            request.Headers.Add("Authorization:" + auth);
            Stream myRequestStream = request.GetRequestStream();
            StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("gb2312"));
            myStreamWriter.Write(postdata);
            myStreamWriter.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            if (response.ContentEncoding.ToLower().Contains("gzip"))
                myResponseStream = new GZipStream(myResponseStream, CompressionMode.Decompress);
            else if (response.ContentEncoding.ToLower().Contains("deflate"))
                myResponseStream = new DeflateStream(myResponseStream, CompressionMode.Decompress);
            string html = string.Empty;
            using (System.IO.StreamReader reader = new System.IO.StreamReader(myResponseStream, Encoding.GetEncoding("UTF-8")))
            {
                html = reader.ReadToEnd();
            }
            myResponseStream.Close();
            return html;
        }

        private static string Base64Encode(string str)
        {
            System.Text.Encoding encode = System.Text.Encoding.ASCII;
            byte[] bytedata = encode.GetBytes(str);
            return Convert.ToBase64String(bytedata, 0, bytedata.Length);
        }

        public static string GetUnixStamp()
        {
            TimeSpan ts = DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return Convert.ToUInt32(ts.TotalSeconds).ToString();
        }

        public static string GetMd5(string str)
        {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "md5").ToLower();
        }

        public static ElemeToken GetElemeToken(BookingFood.Model.bf_area itemArea)
        {
            if (string.IsNullOrEmpty(itemArea.ElemeCookie)) return null;
            BookingFood.BLL.bf_area bllArea = new BookingFood.BLL.bf_area();
            string[] tokens = itemArea.ElemeCookie.Split('_');
            ElemeToken elemeToken = new API.SyncOrder.SyncOrder.ElemeToken();
            string html = string.Empty;
            if (tokens.Length == 3)
            {
                elemeToken.ShopId = tokens[0];
                elemeToken.Key = tokens[1];
                elemeToken.Secret = tokens[2];
                html = PostDataByToken("https://open-api.shop.ele.me/token", "grant_type=client_credentials", "Basic " + Base64Encode(elemeToken.Key + ":" + elemeToken.Secret));
                elemeToken.Token = GetValueByTag(html, "access_token\":\"", "\"");
                elemeToken.ExpireTime = DateTime.Now.AddDays(1);
                itemArea.ElemeCookie = itemArea.ElemeCookie + "_" + elemeToken.Token + "_" + elemeToken.ExpireTime.ToString("yyyyMMddHHmmss");
                bllArea.Update(itemArea);
            }
            else if (tokens.Length == 5)
            {
                elemeToken.ShopId = tokens[0];
                elemeToken.Key = tokens[1];
                elemeToken.Secret = tokens[2];
                elemeToken.Token = tokens[3];
                elemeToken.ExpireTime = DateTime.ParseExact(tokens[4], "yyyyMMddHHmmss", null);
                if (elemeToken.ExpireTime <= DateTime.Now)
                {
                    html = PostDataByToken("https://open-api.shop.ele.me/token", "grant_type=client_credentials", "Basic " + Base64Encode(elemeToken.Key + ":" + elemeToken.Secret));
                    elemeToken.Token = GetValueByTag(html, "access_token\":\"", "\"");
                    tokens[3] = elemeToken.Token;
                    tokens[4] = DateTime.Now.AddDays(1).ToString("yyyyMMddHHmmss");
                    itemArea.ElemeCookie = string.Join("_", tokens);
                    bllArea.Update(itemArea);
                }
            }
            return elemeToken;
        }

    }
}
