using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DTcms.API.Payment.Alipay;
using DTcms.Common;
using System.Xml;
using System.Linq;

namespace DTcms.Web.api.payment.alipay
{
    public partial class notify_url : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Dictionary<string, string> sPara = GetRequestPost();
            if (sPara.Count > 0)//判断是否有带返回参数
            {
                Notify aliNotify = new Notify();
                bool verifyResult = aliNotify.VerifyNotify(sPara, Request.Form["sign"]);

                if (verifyResult)//验证成功
                {
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //请在这里加上商户的业务逻辑程序代码


                    //——请根据您的业务逻辑来编写程序（以下代码仅作参考）——
                    //获取支付宝的通知返回参数，可参考技术文档中服务器异步通知参数列表

                    //解密（如果是RSA签名需要解密，如果是MD5签名则下面一行清注释掉）
                    sPara = aliNotify.Decrypt(sPara);

                    //XML解析notify_data数据
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(sPara["notify_data"]);
                    //商户订单号
                    string out_trade_no = xmlDoc.SelectSingleNode("/notify/out_trade_no").InnerText;
                    //支付宝交易号
                    string trade_no = xmlDoc.SelectSingleNode("/notify/trade_no").InnerText;
                    //交易状态
                    string trade_status = xmlDoc.SelectSingleNode("/notify/trade_status").InnerText;
                    if (trade_status == "TRADE_FINISHED" || trade_status == "TRADE_SUCCESS")
                    {
                        //判断该笔订单是否在商户网站中已经做过处理
                        //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                        //如果有做过处理，不执行商户的业务程序
                        if (out_trade_no.StartsWith("R")) //充值订单
                        {
                            
                        }
                        else //商品订单
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
                                Response.Write("success");
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
                                        Freight = model.takeout == 2 ? "外带" : model.takeout == 1 ? "堂吃" : "外卖"
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
                                            Taste = "",
                                            Freight = model.takeout == 2 ? "外带" : model.takeout == 1 ? "堂吃" : "外卖"
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
                            BookingFood.Model.bf_carnival carnivalModel =
                                bllCarnival.GetModelList(" Type=1 And GetDate() Between BeginTime And EndTime Order By BeginTime Asc").FirstOrDefault();
                            if (carnivalModel != null)
                            {
                                Model.users userModel = new BLL.users().GetModel(model.user_id);
                                BookingFood.BLL.bf_carnival_user_log bllCarnivalUserLog = new BookingFood.BLL.bf_carnival_user_log();
                                int joinNums = bllCarnivalUserLog.GetRecordCount(" CarnivalId=" + carnivalModel.Id
                                    + " And AddTime Between '" + DateTime.Now.ToString("yyyy-MM-dd 00:00:00") + "' And '"
                                    + DateTime.Now.ToString("yyyy-MM-dd 23:59:59") + "' And UserId=userModel.id");
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
                                    BookingFood.BLL.bf_carnival_user bllCarnivalUser = new BookingFood.BLL.bf_carnival_user();
                                    BookingFood.Model.bf_carnival_user carnivalUserModel =
                                        bllCarnivalUser.GetModelList(" UserId=" + userModel.id + " and CarnivalId=" + carnivalModel.Id).FirstOrDefault();
                                    carnivalUserModel.Num += 1;
                                    bllCarnivalUser.Update(carnivalUserModel);
                                }

                            }

                            Model.siteconfig siteConfig = new BLL.siteconfig().loadConfig(Utils.GetXmlMapPath(DTKeys.FILE_SITE_XML_CONFING));
                            string accessToken = string.Empty;
                            //发送微信模板消息
                            if (model.MpForHere != "")
                            {
                                Dictionary<string, Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem> tempData = new Dictionary<string, Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem>();
                                tempData.Add("first", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(siteConfig.mp_temp_submitorder_first));
                                tempData.Add("keyword1", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(model.order_no));
                                tempData.Add("keyword2", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(model.order_amount.ToString("0.00")));
                                tempData.Add("keyword3", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(model.add_time.ToString("yyyy-MM-dd HH:mm")));
                                tempData.Add("remark", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(
                                        model.takeout == 2 ? "外带取餐号：" + model.MpForHere :
                                            model.takeout == 1 ? "堂吃取餐号：" + model.MpForHere : ""));
                                
                                switch(model.which_mp)
                                {
                                    case "master":
                                        accessToken = Senparc.Weixin.MP.CommonAPIs
                                            .AccessTokenContainer.TryGetToken(siteConfig.mp_appid, siteConfig.mp_appsecret);
                                        try
                                        {
                                            Senparc.Weixin.MP.AdvancedAPIs.Template.SendTemplateMessage(accessToken, model.user_name, "jX6j5239NXpRrNe2Adw0ickwbrZz09RaRG9Yuh-pb3o"
                                            , "#173177", "", tempData);
                                        }
                                        catch (Exception) { }
                                        break;
                                    case "slave":
                                        accessToken = Senparc.Weixin.MP.CommonAPIs
                                            .AccessTokenContainer.TryGetToken(siteConfig.mp_slave_appid, siteConfig.mp_slave_appsecret);
                                        try
                                        {
                                            Senparc.Weixin.MP.AdvancedAPIs.Template.SendTemplateMessage(accessToken, model.user_name, "wRXoKBtpkrAMXA-5X5S1HDZWaTozHxVHX4jBPmdK2pc"
                                            , "#173177", "", tempData);
                                        }
                                        catch (Exception) { }
                                        break;
                                }
                                
                                
                                
                            }
                            else if (model.payment_id == 3)
                            {
                                Dictionary<string, Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem> tempData = new Dictionary<string, Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem>();
                                tempData.Add("first", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(siteConfig.mp_temp_submitorder_first));
                                tempData.Add("keyword1", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(model.order_no));
                                tempData.Add("keyword2", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(model.order_amount.ToString("0.00")));
                                tempData.Add("keyword3", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(model.add_time.ToString("yyyy-MM-dd HH:mm")));
                                tempData.Add("remark", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(siteConfig.mp_temp_submitorder_remark));
                                
                                switch (model.which_mp)
                                {
                                    case "master":
                                        accessToken = Senparc.Weixin.MP.CommonAPIs
                                            .AccessTokenContainer.TryGetToken(siteConfig.mp_appid, siteConfig.mp_appsecret);
                                        try
                                        {
                                            Senparc.Weixin.MP.AdvancedAPIs.Template.SendTemplateMessage(accessToken, model.user_name, "jX6j5239NXpRrNe2Adw0ickwbrZz09RaRG9Yuh-pb3o"
                                            , "#173177", "", tempData);
                                        }
                                        catch (Exception) { }
                                        break;
                                    case "slave":
                                        accessToken = Senparc.Weixin.MP.CommonAPIs
                                            .AccessTokenContainer.TryGetToken(siteConfig.mp_slave_appid, siteConfig.mp_slave_appsecret);
                                        try
                                        {
                                            Senparc.Weixin.MP.AdvancedAPIs.Template.SendTemplateMessage(accessToken, model.user_name, "wRXoKBtpkrAMXA-5X5S1HDZWaTozHxVHX4jBPmdK2pc"
                                            , "#173177", "", tempData);
                                        }
                                        catch (Exception) { }
                                        break;
                                }
                                
                            }
                        }
                        
                        //注意：
                        //该种交易状态只在两种情况下出现
                        //1、开通了普通即时到账，买家付款成功后。
                        //2、开通了高级即时到账，从该笔交易成功时间算起，过了签约时的可退款时限（如：三个月以内可退款、一年以内可退款等）后。
                        Response.Write("success");  //请不要修改或删除
                    }
                    else
                    {
                        Response.Write(trade_status);
                    }

                    //——请根据您的业务逻辑来编写程序（以上代码仅作参考）——

                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                }
                else//验证失败
                {
                    Response.Write("fail");
                }
            }
            else
            {
                Response.Write("无通知参数");
            }

        }

        /// <summary>
        /// 获取支付宝POST过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        public Dictionary<string, string> GetRequestPost()
        {
            int i = 0;
            Dictionary<string, string> sArray = new Dictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = Request.Form;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.Form[requestItem[i]]);
            }

            return sArray;
        }

    }
}