using DTcms.Common;
using Senparc.Weixin.MP.TenPayLibV3;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DTcms.Web.api.payment.teegon_jsapi
{
    public partial class feedback : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string is_success = DTRequest.GetFormString("is_success");
            string order_no = DTRequest.GetFormString("order_no");

            string amount = DTRequest.GetQueryString("amount");
            string bank = DTRequest.GetQueryString("bank");
            string buyer = DTRequest.GetQueryString("buyer");
            string channel = DTRequest.GetQueryString("channel");
            string charge_id = DTRequest.GetQueryString("charge_id");
            string device_info = DTRequest.GetQueryString("device_info");
            string metadata = DTRequest.GetQueryString("metadata");
            string pay_time = DTRequest.GetQueryString("pay_time");
            string real_amount = DTRequest.GetQueryString("real_amount");
            string sign = DTRequest.GetQueryString("sign");
            string status = DTRequest.GetQueryString("status");
            string timestamp = DTRequest.GetQueryString("timestamp");

#if DEBUG
            is_success = "true";
#endif

            if (string.Equals(is_success, "true"))
            {
                //写日志
                //System.IO.File.AppendAllText(Utils.GetMapPath("alipaylog.txt"), "商品订单\n", System.Text.Encoding.UTF8);
                BLL.orders bll = new BLL.orders();
                Model.orders model = bll.GetModel(order_no);
                if (model == null)
                {
                    //写日志
                    //System.IO.File.AppendAllText(Utils.GetMapPath("alipaylog.txt"), "订单号：" + out_trade_no + "不存在\n", System.Text.Encoding.UTF8);
                    Response.Write("该订单号不存在");
                    return;
                }
                if (model.payment_status == 2) //已付款
                {
                    //写日志
                    //System.IO.File.AppendAllText(Utils.GetMapPath("alipaylog.txt"), "订单号：" + out_trade_no + "已付款\n", System.Text.Encoding.UTF8);
                    //Response.Write("<xml><return_code><![CDATA[SUCCESS]]></return_code><return_msg><![CDATA[OK]]></return_msg></xml>");
                    return;
                }
                //if (model.order_amount != decimal.Parse(total_fee))
                //{
                //    //写日志
                //    //System.IO.File.AppendAllText(Utils.GetMapPath("alipaylog.txt"), "订单号：" + out_trade_no + "订单金额" + model.order_amount + "和支付金额" + total_fee + "不相符\n", System.Text.Encoding.UTF8);
                //    Response.Write("订单金额和支付金额不相符");
                //    return;
                //}
                bool result = bll.UpdateField(order_no, "payment_status=2,payment_time='" + DateTime.Now + "'");
                Common.Log.Info("微信支付成功:" + order_no);
                //更新地址记录
                if(model.address.IndexOf("取餐号")==-1)
                {
                    BookingFood.BLL.bf_user_address bllUserAddress = new BookingFood.BLL.bf_user_address();
                    int existAddress = bllUserAddress.GetRecordCount(string.Format("UserId={0} And Address='{1}' And Telphone='{2}'", model.user_id, model.address, model.telphone));
                    int user_address_id = 0;
                    if (existAddress == 0)
                    {
                        user_address_id = bllUserAddress.Add(new BookingFood.Model.bf_user_address()
                        {
                            Address = model.address,
                            AreaId = model.area_id,
                            AreaTitle = model.area_title,
                            AreaType = model.area_type,
                            NickName = model.accept_name,
                            Telphone = model.telphone,
                            UserId = model.user_id
                        });
                    }
                    else
                    {
                        user_address_id = bllUserAddress.GetModelList(string.Format("UserId={0} And Address='{1}' And Telphone='{2}'", model.user_id, model.address, model.telphone))[0].Id;
                    }
                    bll.UpdateField(model.id, "user_address_id=" + user_address_id);
                }
                
                //更新活动日志中对应的使用记录的支付状态
                BookingFood.BLL.bf_carnival_user_log bllCarUserLog = new BookingFood.BLL.bf_carnival_user_log();
                BookingFood.Model.bf_carnival_user_log modelCarUserLog = bllCarUserLog.GetModelList(" OrderId=" + model.id).FirstOrDefault();
                if(modelCarUserLog!=null)
                {
                    modelCarUserLog.IsPayForTakeOut = 1;
                    bllCarUserLog.Update(modelCarUserLog);
                }
                Model.article_goods goodsModel = null;
                BookingFood.Model.bf_back_door back = null;
                BookingFood.BLL.bf_good_nickname bllNick = new BookingFood.BLL.bf_good_nickname();
                BookingFood.Model.bf_good_nickname nickModel = null;
                List<BookingFood.Model.bf_back_door> listBack = new List<BookingFood.Model.bf_back_door>();
                BLL.article bllArticle = new BLL.article();
                foreach (var item in model.order_goods)
                {
                    if (item.type == "one" || item.type == "full" || item.type == "discount")
                    {
                        goodsModel = bllArticle.GetGoodsModel(item.goods_id);
                        back = new BookingFood.Model.bf_back_door()
                        {
                            OrderId = model.id,
                            GoodsCount = item.quantity,
                            CategoryId = goodsModel.opposition_id != 0 ? goodsModel.opposition_id : goodsModel.category_id,
                            AreaId = model.area_id,
                            IsDown = 0,
                            Taste = !string.IsNullOrEmpty(item.subgoodsid) ? item.subgoodsid.Split('‡')[2] : "",
                            Freight = model.takeout == 2 ? "打包" : model.takeout == 1 ? "堂吃" : "外卖"
                        };
                        if (goodsModel.nick_id != 0)
                        {
                            nickModel = bllNick.GetModel(goodsModel.nick_id);
                            back.GoodsName = nickModel.Title;
                        }
                        else
                        {
                            back.GoodsName = item.goods_name;
                        }
                        listBack.Add(back);
                    }
                    else if (item.type == "combo")
                    {
                        string[] subgoods = item.subgoodsid.Split('†');
                        foreach (var sub in subgoods)
                        {
                            if (sub.Split('‡')[0] == "taste") continue;
                            goodsModel = bllArticle.GetGoodsModel(int.Parse(sub.Split('‡')[1]));
                            back = new BookingFood.Model.bf_back_door()
                            {
                                OrderId = model.id,
                                GoodsCount = item.quantity,
                                CategoryId = goodsModel.opposition_id != 0 ? goodsModel.opposition_id : goodsModel.category_id,
                                AreaId = model.area_id,
                                IsDown = 0,
                                Taste = sub.Split('‡')[3],
                                Freight = model.takeout == 2 ? "打包" : model.takeout == 1 ? "堂吃" : "外卖"
                            };
                            if (goodsModel.nick_id != 0)
                            {
                                nickModel = bllNick.GetModel(goodsModel.nick_id);
                                back.GoodsName = nickModel.Title;
                            }
                            else
                            {
                                back.GoodsName = goodsModel.title;
                            }
                            listBack.Add(back);
                        }
                    }
                }
                BookingFood.BLL.bf_back_door bllBack = new BookingFood.BLL.bf_back_door();
                foreach (var item in listBack)
                {
                    bllBack.Add(item);
                }
                if (!result)
                {
                    Response.Write("修改订单状态失败");
                    return;
                }

                //写日志
                //System.IO.File.AppendAllText(Utils.GetMapPath("alipaylog.txt"), "修改订单状态：" + result.ToString() + "\n", System.Text.Encoding.UTF8);

                //扣除积分
                if (model.point < 0)
                {
                    new BLL.point_log().Add(model.user_id, model.user_name, model.point, "换购扣除积分，订单号：" + model.order_no);
                }

                #region 处理群组优惠金额的扣减
                if (model.voucher_total > 0)
                {
                    BookingFood.BLL.bf_user_voucher bllUserVoucher = new BookingFood.BLL.bf_user_voucher();
                    List<BookingFood.Model.bf_user_voucher> listVoucher =
                        bllUserVoucher.GetModelList("UserId=" + model.user_id + " and GetDate()<ExpireTime and Status=0");
                    decimal _unless = 0;
                    foreach (var item in listVoucher)
                    {
                        if (_unless > 0)
                        {
                            if (item.Amount >= _unless)
                            {
                                item.Amount -= _unless;
                                bllUserVoucher.Update(item);
                                break;
                            }
                            else
                            {
                                _unless = _unless - item.Amount;
                                item.Amount = 0;
                                bllUserVoucher.Update(item);
                            }
                        }
                        else
                        {
                            if (item.Amount >= model.voucher_total)
                            {
                                item.Amount -= model.voucher_total;
                                bllUserVoucher.Update(item);
                                break;
                            }
                            else
                            {
                                _unless = model.voucher_total - item.Amount;
                                item.Amount = 0;
                                bllUserVoucher.Update(item);
                            }
                        }

                    }
                }
                #endregion

                #region 在满送活动期间内数量+1
                BookingFood.BLL.bf_carnival bllCarnival = new BookingFood.BLL.bf_carnival();
                Model.users userModel = new BLL.users().GetModel(model.user_id);
                BookingFood.BLL.bf_carnival_user_log bllCarnivalUserLog = new BookingFood.BLL.bf_carnival_user_log();
                string accessToken = string.Empty;
                Model.siteconfig siteConfig = new BLL.siteconfig().loadConfig(Utils.GetXmlMapPath(DTKeys.FILE_SITE_XML_CONFING));
                if (model.takeout == 0)
                {
                    
                    BookingFood.Model.bf_carnival carnivalModel =
                        bllCarnival.GetModelList(" Type=1 And GetDate() Between BeginTime And EndTime Order By BeginTime Asc").FirstOrDefault();
                    if (carnivalModel != null)
                    {
                        Model.article_goods carnivalGoodsModel = null;
                        BookingFood.Model.bf_carnival_user carnivalUserModel = null;
                        BookingFood.BLL.bf_carnival_user bllCarnivalUser = new BookingFood.BLL.bf_carnival_user();
                        if(model.order_goods.FirstOrDefault(s => s.type == "full")!=null)
                        {
                            carnivalGoodsModel = bllArticle.GetGoodsModel(model.order_goods.FirstOrDefault(s => s.type == "full").goods_id);
                        }
                        
                        
                        int joinNums = bllCarnivalUserLog.GetRecordCount(" CarnivalId=" + carnivalModel.Id
                            + " And AddTime Between '" + DateTime.Now.ToString("yyyy-MM-dd 00:00:00") + "' And '"
                            + DateTime.Now.ToString("yyyy-MM-dd 23:59:59") + "' And UserId=" + userModel.id);
                        if (joinNums == 0)
                        {
                            bllCarnivalUserLog.Add(new BookingFood.Model.bf_carnival_user_log()
                            {
                                AddTime = DateTime.Now,
                                CarnivalId = carnivalModel.Id,
                                OpenId = userModel.user_name,
                                OrderId = model.id,
                                UserId = userModel.id
                            });

                            carnivalUserModel =
                                bllCarnivalUser.GetModelList(" UserId=" + userModel.id + " and CarnivalId=" + carnivalModel.Id).FirstOrDefault();
                            if (carnivalUserModel == null)
                            {
                                carnivalUserModel = new BookingFood.Model.bf_carnival_user()
                                {
                                    CarnivalId = carnivalModel.Id,
                                    Num = 1,
                                    Openid = userModel.user_name,
                                    UserId = userModel.id,
                                    AreaId = model.area_id
                                };
                                bllCarnivalUser.Add(carnivalUserModel);
                            }
                            else
                            {
                                carnivalUserModel.Num += 1;
                                bllCarnivalUser.Update(carnivalUserModel);
                            }
                        }
                        if (carnivalGoodsModel != null)
                        {
                            //更新用户的兑换次数
                            carnivalUserModel.Num -= carnivalGoodsModel.change_nums;
                            bllCarnivalUser.Update(carnivalUserModel);
                        }
                    }
                }
                else if (model.takeout != 0)
                {
                    BookingFood.Model.bf_carnival carnivalOffline = null;
                    int areaid = 0;
                    if (model.takeout==2)
                    {
                        BookingFood.BLL.bf_area bllArea = new BookingFood.BLL.bf_area();
                        BookingFood.Model.bf_area areaModel = bllArea.GetModelList("OppositeId=" + model.area_id)[0];
                        areaid = areaModel.Id;
                        
                    }
                    else
                    {
                        areaid = model.area_id;
                    }
                    carnivalOffline =
                        bllCarnival.GetModelList(" Type=2 And GetDate() Between BeginTime And EndTime And Id In (Select CarnivalId From bf_carnival_area Where AreaId="
                        + areaid + ") Order By BeginTime Asc").FirstOrDefault();
                    bool isHaveDiscountGood = false;
                    if (carnivalOffline != null && model.order_goods.Where(s => s.type == "discount").Sum(s => s.quantity) > 0)
                    {
                        isHaveDiscountGood = true;
                    }
                    if (bllCarUserLog.GetRecordCount(" UserId=" + userModel.id + " And CarnivalId=" + carnivalOffline.Id 
                        + " And AddTime Between '" + DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00" + "' And '" + DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59"
                        + "' And IsPayForTakeOut=1") == 0)
                    {
                        if (carnivalOffline != null && isHaveDiscountGood)
                        {
                            bllCarnivalUserLog.Add(new BookingFood.Model.bf_carnival_user_log()
                            {
                                AddTime = DateTime.Now,
                                CarnivalId = carnivalOffline.Id,
                                OpenId = userModel.user_name,
                                OrderId = model.id,
                                UserId = userModel.id,
                                AreaId = areaid,
                                IsPayForTakeOut = 1
                            });
                        }
                    }
                    //2017-04-03 一周内消费超过3次 送20元余额
                    if(userModel.company_id!=0)
                    {
                        DataTable listUserLog = bll.GetList(0, " user_id=" + userModel.id
                        + " And add_time Between '" + DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd") + " 00:00:00" + "' And '" + DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59"
                        + "' And takeout<>0 And IsCountFreeAvaliable=0 And [status]=2", " id desc").Tables[0];
                        string _orderTime = ",";
                        foreach (DataRow item in listUserLog.Rows)
                        {
                            if (_orderTime.IndexOf("," + DateTime.Parse(item["add_time"].ToString()).ToString("yyyy-MM-dd") + ",") == -1)
                            {
                                _orderTime += DateTime.Parse(item["add_time"].ToString()).ToString("yyyy-MM-dd") + ",";
                            }
                        }
                        int orderNums = _orderTime.TrimStart(',').TrimEnd(',').Split(',').Length;
                        if (orderNums >= 3)
                        {
                            foreach (DataRow item in listUserLog.Rows)
                            {
                                bll.UpdateField(int.Parse(item["id"].ToString()), "IsCountFreeAvaliable=1");
                            }
                            BookingFood.BLL.bf_user_voucher bllUserVoucher = new BookingFood.BLL.bf_user_voucher();
                            bllUserVoucher.Add(new BookingFood.Model.bf_user_voucher()
                            {
                                AddTime = DateTime.Now,
                                Amount = 5,
                                CompanyId = userModel.company_id,
                                ExpireTime = DateTime.Now.AddMonths(1),
                                UserId = userModel.id
                            });

                            decimal totalAmount = bllUserVoucher.GetModelList("UserId=" + userModel.id + " and GetDate()<ExpireTime and Status=0").Sum(s => s.Amount);
                            Dictionary<string, Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem> tempData = new Dictionary<string, Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem>();
                            tempData = new Dictionary<string, Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem>();
                            tempData.Add("first", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem("感谢您7天内多次光临馍王,送您5元"));//"您有新同事加入馍王贵司VIP，所有成员余额均增加3元！"
                            tempData.Add("keyword1", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(model.area_title));//"中山西路1919号馍王"
                            tempData.Add("keyword2", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(totalAmount + "元"));
                            tempData.Add("remark", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(siteConfig.vip_enough_footer));//"赶紧介绍给更多的同事吧！"

                            accessToken = Senparc.Weixin.MP.CommonAPIs
                                .AccessTokenContainer.TryGetToken(siteConfig.mp_slave_appid, siteConfig.mp_slave_appsecret);
                            try
                            {
                                Senparc.Weixin.MP.AdvancedAPIs.Template.SendTemplateMessage(accessToken, userModel.user_name, "KPeoGA1cOZTDiqAf1IECWauABgqtz-P-y-zHME9hhp8"
                                , "#173177", "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wxc5776b5774a3d010&redirect_uri=https%3A%2F%2Fwww.4008317417.cn%2Fmp_join_company.aspx%3Fshowwxpaytitle%3D1&response_type=code&scope=snsapi_userinfo&state=slave#wechat_redirect", tempData);
                            }
                            catch (Exception ex) { }
                        }
                    }
                    
                }
                #endregion

                #region 发送微信模板消息
                if (model.MpForHere != "")
                {
                    Dictionary<string, Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem> tempData = new Dictionary<string, Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem>();
                    tempData.Add("first", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(siteConfig.mp_temp_submitorder_first));
                    tempData.Add("keyword1", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(model.add_time.ToString("yyyy-MM-dd HH:mm")));
                    tempData.Add("keyword2", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(model.order_amount.ToString("0.00")));
                    string tempMessage = string.Empty;
                    foreach (var item in model.order_goods)
                    {
                        if (item.type == "one")
                        {
                            tempMessage += string.Format("{0}￥{1}{2};"
                                                , item.goods_name
                                                , (item.quantity * item.goods_price).ToString().Replace(".00", "")
                                                , !string.IsNullOrEmpty(item.subgoodsid) ? "*" + item.subgoodsid.Split('‡')[2] : "");
                        }
                        else if (item.type == "combo")
                        {
                            string[] subgoods = item.subgoodsid.Split('†');
                            string subrtn = string.Empty;
                            foreach (var sub in subgoods)
                            {
                                subrtn += string.Format("*{0}", sub.Split('‡')[2] + (!string.IsNullOrEmpty(sub.Split('‡')[3]) ? "/" + sub.Split('‡')[3] : ""));
                            }
                            tempMessage += string.Format("{0}￥{1}{2};"
                                                , item.goods_name
                                                , (item.quantity * item.goods_price).ToString().Replace(".00", "")
                                                , subrtn);

                        }
                        else if (item.type == "full" || item.type == "discount")
                        {
                            tempMessage += string.Format("{0}￥{1}{2};"
                                                , item.goods_name
                                                , (item.quantity * item.goods_price).ToString().Replace(".00", "")
                                                , !string.IsNullOrEmpty(item.subgoodsid) ? "*" + item.subgoodsid.Split('‡')[2] : "");
                        }
                    }
                    tempData.Add("keyword3", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(tempMessage));
                    tempData.Add("keyword4", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(model.address));
                    tempData.Add("remark", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(
                            model.takeout == 2 ? "打包取餐号：" + model.MpForHere :
                                model.takeout == 1 ? "堂吃取餐号：" + model.MpForHere : ""));

                    switch (model.which_mp)
                    {
                        case "master":
                            accessToken = Senparc.Weixin.MP.CommonAPIs
                                .AccessTokenContainer.TryGetToken(siteConfig.mp_appid, siteConfig.mp_appsecret);
                            try
                            {
                                Senparc.Weixin.MP.AdvancedAPIs.Template.SendTemplateMessage(accessToken, model.user_name, "REmtsGZMK7-3NXNJf3NZMOfmH9dKwkvwvCBww5F9VYQ"
                                , "#173177", "", tempData);
                            }
                            catch (Exception) { }
                            break;
                        case "slave":
                            accessToken = Senparc.Weixin.MP.CommonAPIs
                                .AccessTokenContainer.TryGetToken(siteConfig.mp_slave_appid, siteConfig.mp_slave_appsecret);
                            try
                            {
                                Senparc.Weixin.MP.AdvancedAPIs.Template.SendTemplateMessage(accessToken, model.user_name, "gYklU4AeAT7KCehbfRP5emhBsSNkhMVDVtdIBFlhn8Y"
                                , "#173177", "", tempData);
                            }
                            catch (Exception) { }
                            break;
                    }
                }
                else if (model.payment_id == 3 || model.payment_id == 5)
                {
                    Dictionary<string, Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem> tempData = new Dictionary<string, Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem>();
                    tempData.Add("first", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(siteConfig.mp_temp_submitorder_first));
                    tempData.Add("keyword1", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(model.add_time.ToString("yyyy-MM-dd HH:mm")));
                    tempData.Add("keyword2", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(model.order_amount.ToString("0.00")));
                    string tempMessage = string.Empty;
                    foreach (var item in model.order_goods)
                    {
                        if (item.type == "one")
                        {
                            tempMessage += string.Format("{0}￥{1}{2};"
                                                , item.goods_name
                                                , (item.quantity * item.goods_price).ToString().Replace(".00", "")
                                                , !string.IsNullOrEmpty(item.subgoodsid) ? "*" + item.subgoodsid.Split('‡')[2] : "");
                        }
                        else if (item.type == "combo")
                        {
                            string[] subgoods = item.subgoodsid.Split('†');
                            string subrtn = string.Empty;
                            foreach (var sub in subgoods)
                            {
                                subrtn += string.Format("*{0}", sub.Split('‡')[2] + (!string.IsNullOrEmpty(sub.Split('‡')[3]) ? "/" + sub.Split('‡')[3] : ""));
                            }
                            tempMessage += string.Format("{0}￥{1}{2};"
                                                , item.goods_name
                                                , (item.quantity * item.goods_price).ToString().Replace(".00", "")
                                                , subrtn);

                        }
                        else if (item.type == "full" || item.type == "discount")
                        {
                            tempMessage += string.Format("{0}￥{1}{2};"
                                                , item.goods_name
                                                , (item.quantity * item.goods_price).ToString().Replace(".00", "")
                                                , !string.IsNullOrEmpty(item.subgoodsid) ? "*" + item.subgoodsid.Split('‡')[2] : "");
                        }
                    }
                    tempData.Add("keyword3", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(tempMessage));
                    tempData.Add("keyword4", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(model.address));
                    tempData.Add("remark", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(siteConfig.mp_temp_submitorder_remark));

                    switch (model.which_mp)
                    {
                        case "master":
                            accessToken = Senparc.Weixin.MP.CommonAPIs
                                .AccessTokenContainer.TryGetToken(siteConfig.mp_appid, siteConfig.mp_appsecret);
                            try
                            {
                                Senparc.Weixin.MP.AdvancedAPIs.Template.SendTemplateMessage(accessToken, model.user_name, "REmtsGZMK7-3NXNJf3NZMOfmH9dKwkvwvCBww5F9VYQ"
                                , "#173177", "", tempData);
                            }
                            catch (Exception) { }
                            break;
                        case "slave":
                            accessToken = Senparc.Weixin.MP.CommonAPIs
                                .AccessTokenContainer.TryGetToken(siteConfig.mp_slave_appid, siteConfig.mp_slave_appsecret);
                            try
                            {
                                Senparc.Weixin.MP.AdvancedAPIs.Template.SendTemplateMessage(accessToken, model.user_name, "gYklU4AeAT7KCehbfRP5emhBsSNkhMVDVtdIBFlhn8Y"
                                , "#173177", "", tempData);
                            }
                            catch (Exception) { }
                            break;
                    }

                }
                #endregion
                Common.Log.Info("微信发送模板消息:" + order_no);
                Response.Write("<xml><return_code><![CDATA[SUCCESS]]></return_code><return_msg><![CDATA[OK]]></return_msg></xml>");
            }
            
        }
    }
}