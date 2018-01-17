using DTcms.Common;
using Senparc.Weixin.MP.TenPayLibV3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DTcms.Web.api.payment.mppay
{
    public partial class feedback : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ResponseHandler resHandler = new ResponseHandler(null);
            string result_code = resHandler.GetParameter("result_code");
            string appid = resHandler.GetParameter("appid");
            string mch_id = resHandler.GetParameter("mch_id");
            string device_info = resHandler.GetParameter("device_info");
            string nonce_str = resHandler.GetParameter("nonce_str");
            string sign = resHandler.GetParameter("sign");
            string err_code = resHandler.GetParameter("err_code");
            string err_code_des = resHandler.GetParameter("err_code_des");
            string openid = resHandler.GetParameter("openid");
            string is_subscribe = resHandler.GetParameter("is_subscribe");
            string trade_type = resHandler.GetParameter("trade_type");
            string bank_type = resHandler.GetParameter("bank_type");
            string total_fee = resHandler.GetParameter("total_fee");
            string coupon_fee = resHandler.GetParameter("coupon_fee");
            string fee_type = resHandler.GetParameter("fee_type");
            string transaction_id = resHandler.GetParameter("transaction_id");
            string out_trade_no = resHandler.GetParameter("out_trade_no");
            string attach = resHandler.GetParameter("attach");
            string time_end = resHandler.GetParameter("time_end");

#if DEBUG
            result_code = "SUCCESS";
#endif

            if (string.Equals(result_code, "SUCCESS"))
            {
                //写日志
                //System.IO.File.AppendAllText(Utils.GetMapPath("alipaylog.txt"), "商品订单\n", System.Text.Encoding.UTF8);
                BLL.orders bll = new BLL.orders();
                Model.orders model = bll.GetModel(out_trade_no);
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
                    Response.Write("<xml><return_code><![CDATA[SUCCESS]]></return_code><return_msg><![CDATA[OK]]></return_msg></xml>");
                    return;
                }
                //if (model.order_amount != decimal.Parse(total_fee))
                //{
                //    //写日志
                //    //System.IO.File.AppendAllText(Utils.GetMapPath("alipaylog.txt"), "订单号：" + out_trade_no + "订单金额" + model.order_amount + "和支付金额" + total_fee + "不相符\n", System.Text.Encoding.UTF8);
                //    Response.Write("订单金额和支付金额不相符");
                //    return;
                //}
                bool result = bll.UpdateField(out_trade_no, "payment_status=2,payment_time='" + DateTime.Now + "'");
                Common.Log.Info("微信支付成功:" + out_trade_no);
                if (model.takeout == 0)
                {
                    //更新地址记录
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
                if (modelCarUserLog != null)
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
                //在满送活动期间内数量+1
                BookingFood.BLL.bf_carnival bllCarnival = new BookingFood.BLL.bf_carnival();
                Model.users userModel = new BLL.users().GetModel(model.user_id);
                BookingFood.BLL.bf_carnival_user_log bllCarnivalUserLog = new BookingFood.BLL.bf_carnival_user_log();
                if (model.takeout == 0)
                {

                    BookingFood.Model.bf_carnival carnivalModel =
                        bllCarnival.GetModelList(" Type=1 And GetDate() Between BeginTime And EndTime Order By BeginTime Asc").FirstOrDefault();
                    if (carnivalModel != null)
                    {
                        Model.article_goods carnivalGoodsModel = null;
                        BookingFood.Model.bf_carnival_user carnivalUserModel = null;
                        BookingFood.BLL.bf_carnival_user bllCarnivalUser = new BookingFood.BLL.bf_carnival_user();
                        if (model.order_goods.FirstOrDefault(s => s.type == "full") != null)
                        {
                            carnivalGoodsModel = bllArticle.GetGoodsModel(model.order_goods.FirstOrDefault(s => s.type == "full").id);
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
                    if (model.takeout == 2)
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


                Model.siteconfig siteConfig = new BLL.siteconfig().loadConfig(Utils.GetXmlMapPath(DTKeys.FILE_SITE_XML_CONFING));
                string accessToken = string.Empty;
                //发送微信模板消息
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
                Common.Log.Info("微信发送模板消息:" + out_trade_no);
                Response.Write("<xml><return_code><![CDATA[SUCCESS]]></return_code><return_msg><![CDATA[OK]]></return_msg></xml>");
            }

        }
    }
}