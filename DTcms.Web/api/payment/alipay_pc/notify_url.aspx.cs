using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DTcms.API.Payment.Alipay_Pc;
using DTcms.Common;

namespace DTcms.Web.api.payment.alipay_pc
{
    public partial class notify_url : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SortedDictionary<string, string> sPara = GetRequestPost();

            if (sPara.Count > 0)//判断是否有带返回参数
            {
                Notify aliNotify = new Notify();
                bool verifyResult = aliNotify.Verify(sPara, Request.Form["notify_id"], Request.Form["sign"]);

                if (verifyResult)//验证成功
                {
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //请在这里加上商户的业务逻辑程序代码


                    //——请根据您的业务逻辑来编写程序（以下代码仅作参考）——
                    //获取支付宝的通知返回参数，可参考技术文档中服务器异步通知参数列表

                    //商户订单号

                    string out_trade_no = Request.Form["out_trade_no"];

                    //支付宝交易号

                    string trade_no = Request.Form["trade_no"];

                    //交易状态
                    string trade_status = Request.Form["trade_status"];


                    if (Request.Form["trade_status"] == "TRADE_FINISHED" || Request.Form["trade_status"] == "TRADE_SUCCESS")
                    {
                        //判断该笔订单是否在商户网站中已经做过处理
                        //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                        //如果有做过处理，不执行商户的业务程序

                        //注意：
                        //退款日期超过可退款期限后（如三个月可退款），支付宝系统发送该交易状态通知
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
                    else
                    {
                    }

                    //——请根据您的业务逻辑来编写程序（以上代码仅作参考）——

                    Response.Write("success");  //请不要修改或删除

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
        public SortedDictionary<string, string> GetRequestPost()
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
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