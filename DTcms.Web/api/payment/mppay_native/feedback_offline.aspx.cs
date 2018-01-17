using DTcms.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Senparc.Weixin.MP.TenPayLibV3;

namespace DTcms.Web.api.payment.mppay_native
{
    public partial class feedback_offline : System.Web.UI.Page
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

            if (string.Equals(result_code, "SUCCESS"))
            {
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
                                Taste = (sub.Split('‡').Length == 4 || sub.Split('‡').Length == 5) ? sub.Split('‡')[3] : "",
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

                Response.Write("<xml><return_code><![CDATA[SUCCESS]]></return_code><return_msg><![CDATA[OK]]></return_msg></xml>");
            }
        }
    }
}