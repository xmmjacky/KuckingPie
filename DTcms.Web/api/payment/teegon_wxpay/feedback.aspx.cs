using DTcms.Common;
using Senparc.Weixin.MP.TenPayLibV3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DTcms.Web.api.payment.teegon_wxpay
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
            Log.Info("is_success:" + is_success);
            if (string.Equals(is_success, "true"))
            {
                BLL.orders bll = new BLL.orders();
                Log.Info("order_no:" + order_no);
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
                    //Response.Write("success");
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
                bool result = bll.UpdateField(order_no, "payment_status=2,payment_time='" + DateTime.Now + "'");
                #region 更新后厨推送数据
                BLL.article bllArticle = new BLL.article();
                Model.article_goods goodsModel = null;
                BookingFood.Model.bf_back_door back = null;
                BookingFood.BLL.bf_good_nickname bllNick = new BookingFood.BLL.bf_good_nickname();
                BookingFood.Model.bf_good_nickname nickModel = null;
                List<BookingFood.Model.bf_back_door> listBack = new List<BookingFood.Model.bf_back_door>();
                foreach (var item in model.order_goods)
                {
                    if (item.type == "one")
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
                            Freight = "外卖"
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
                                Taste = sub.Split('‡').Length == 4 ? sub.Split('‡')[3] : "",
                                Freight = "外卖"
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
                #endregion

                #region 发送邮件
                Model.mail_template mailModel = new BLL.mail_template().GetModel("ordermail");
                if (mailModel != null)
                {
                    //替换模板内容
                    string titletxt = mailModel.maill_title + model.order_no;
                    string bodytxt = mailModel.content;
                    bodytxt = bodytxt.Replace("{useremail}", model.email);
                    bodytxt = bodytxt.Replace("{useraddress}", model.address);
                    bodytxt = bodytxt.Replace("{usertelphone}", model.telphone);
                    bodytxt = bodytxt.Replace("{orderaddtime}", model.add_time.ToString("yyyy-MM-dd HH:mm:ss"));
                    bodytxt = bodytxt.Replace("{orderno}", model.order_no);
                    bodytxt = bodytxt.Replace("{orderamount}", (model.real_freight != 0 ? "外送费：" + model.real_freight.ToString() : "") + "总计：" + model.order_amount.ToString());
                    bodytxt = bodytxt.Replace("{ordermessage}", model.message);
                    string rtn = string.Empty;
                    foreach (var item in model.order_goods)
                    {
                        if (item.type == "one")
                        {
                            rtn += string.Format("<tr style=\"line-height: 16px;\">" +
                                    "<td style=\"width:60px;text-align:center;\">" +
                                        "{0}" +
                                    "</td>" +
                                    "<td  style=\"width:160px;\">" +
                                        "{1} {4}" +
                                    "</td>" +
                                    "<td >" +
                                        "{3}" +
                                    "</td>" +
                                "</tr>"
                                , item.quantity
                                , item.goods_name
                                , item.goods_price.ToString().Replace(".00", "")
                                , (item.quantity * item.goods_price).ToString().Replace(".00", "")
                                , !string.IsNullOrEmpty(item.subgoodsid) ? "(" + item.subgoodsid.Split('‡')[2] + ")" : "");
                        }
                        else if (item.type == "combo")
                        {
                            rtn += string.Format("<tr style=\"line-height: 16px;\">" +
                                    "<td  style=\"width:60px;text-align:center;\">" +
                                        "{0}" +
                                    "</td>" +
                                    "<td  style=\"width:160px;\">" +
                                        "{1}" +
                                    "</td>" +
                                    "<td  >" +
                                        "{3}" +
                                    "</td>" +
                                "</tr>", item.quantity, item.goods_name, item.goods_price.ToString().Replace(".00", "")
                                , (item.quantity * item.goods_price).ToString().Replace(".00", ""));
                            string[] subgoods = item.subgoodsid.Split('†');
                            foreach (var sub in subgoods)
                            {
                                rtn += string.Format("<tr style=\"line-height: 16px;\">" +
                                    "<td  style=\"width:60px;text-align:center;\"></td>" +
                                    "<td  style=\"width:160px;\">" +
                                        "{0} {1}" +
                                    "</td>" +
                                    "<td ></td>" +
                                    "</tr>"
                                    , sub.Split('‡')[2]
                                    , sub.Split('‡').Length == 4 ? "(" + sub.Split('‡')[3] + ")" : ""
                                    );
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(rtn))
                    {
                        rtn = "<table style=\"font-sze:18px;\">" + rtn + "</table>";
                    }
                    bodytxt = bodytxt.Replace("{orderdetail}", rtn);

                    //发送邮件
                    Model.siteconfig siteConfig = new BLL.siteconfig().loadConfig(Utils.GetXmlMapPath(DTKeys.FILE_SITE_XML_CONFING));
                    BookingFood.Model.bf_area areaModel = new BookingFood.BLL.bf_area().GetModel(model.area_id);
                    try
                    {
                        DTMail.sendMail(siteConfig.emailstmp,
                            siteConfig.emailusername,
                            DESEncrypt.Decrypt(siteConfig.emailpassword),
                            siteConfig.emailnickname,
                            siteConfig.emailfrom,
                            model.email,
                            titletxt, bodytxt);
                        //区域所属管理员邮件地址
                        if (areaModel.ManagerId != null)
                        {
                            DTMail.sendMail(siteConfig.emailstmp,
                            siteConfig.emailusername,
                            DESEncrypt.Decrypt(siteConfig.emailpassword),
                            siteConfig.emailnickname,
                            siteConfig.emailfrom,
                            new BLL.manager().GetModel((int)areaModel.ManagerId).user_name,
                            titletxt, bodytxt);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Info("{\"msg\":0, \"msgbox\":\"邮件发送失败，请联系本站管理员！" + ex.Message + "_" + ex.InnerException.Message + "\"}");
                    }

                }
                #endregion
            }

        }
    }
}