using DTcms.Common;
using Senparc.Weixin.MP.TenPayLibV3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DTcms.Web.api.payment.teegon_jsapi
{
    public partial class feedback2 : System.Web.UI.Page
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
            if (string.Equals(is_success, "true"))
            {
                //写日志
                //System.IO.File.AppendAllText(Utils.GetMapPath("alipaylog.txt"), "商品订单\n", System.Text.Encoding.UTF8);
                BLL.orders bll = new BLL.orders();
                Model.orders model = bll.GetModel(order_no);
                if (model == null)
                {
                    Response.Write("该订单号不存在");
                    return;
                }
                if (model.payment_status == 2) //已付款
                {
                    return;
                }
                bool result = bll.UpdateField(order_no, "payment_status=2,payment_time='" + DateTime.Now + "'");
                Common.Log.Info("P2P扫码支付成功:" + order_no);
            }
        }
    }
}