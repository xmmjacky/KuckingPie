using Senparc.Weixin.MP.TenPayLibV3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DTcms.Common;
using Orm.Son.Core;
using BookingFood.Model.Pay;

namespace DTcms.Web.api.payment.mppay_native
{
    public partial class chargefeedback : System.Web.UI.Page
    {
        private readonly string strcon = "ConnectionString";

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
                using (var db = new SonConnection(strcon))
                {
                    var userinfo = db.Find<dt_user_top_up>(Convert.ToInt32(out_trade_no));
                    if (userinfo == null)
                    {
                        Response.Write("该订单号不存在");
                        return;
                    }
                    if (userinfo.Paystate == 1)
                    {
                        Response.Write("<xml><return_code><![CDATA[SUCCESS]]></return_code><return_msg><![CDATA[OK]]></return_msg></xml>");
                        return;
                    }
                    userinfo.Paystate = 1;
                    db.Update(userinfo);
                    if (userinfo.Type == 1)
                    {
                        var userconfirm = new dt_user_confirm_top()
                        {
                            IsDeleted = 0,
                            AddAmount = userinfo.Amount.ToString(),
                            Amount = userinfo.Amount+100,
                            OpneId = userinfo.OpenId,
                            AreaId = userinfo.AreaId,
                            AreaName = userinfo.AreaName,
                            NickName = userinfo.NickName,
                            UserId = userinfo.UserId,
                            type = userinfo.Type
                        };
                        db.Insert(userconfirm);
                        var useraccountstr = string.Format(@" update dt_users set account=account+{0} where id={1}", userconfirm.Amount, userconfirm.UserId);
                        db.ExecuteSql(useraccountstr);
                    }
                }
                Response.Write("<xml><return_code><![CDATA[SUCCESS]]></return_code><return_msg><![CDATA[OK]]></return_msg></xml>");
            }
        }
    }
}