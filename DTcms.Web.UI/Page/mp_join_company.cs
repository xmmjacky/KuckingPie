using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DTcms.Common;
using System.Linq;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.Helpers;
using Senparc.Weixin.MP.CommonAPIs;

namespace DTcms.Web.UI.Page
{
    public partial class mp_join_company : Web.UI.BasePage
    {
        public BookingFood.Model.bf_company modelCompay = null;
        public BookingFood.Model.bf_company modelRegisterCompany = null;
        public int totalAmount = 0;

        protected string jsnoncestr = string.Empty;
        protected string jstimestamp = string.Empty;
        protected string mp_signature = string.Empty, distributionArea = string.Empty;
        public int useraccount = 0;
        protected override void ShowPage()
        {
            this.Init += mp_discount_Init;       
            
        }

        void mp_discount_Init(object sender, EventArgs e)
        {
            InitByMp();
            BookingFood.BLL.bf_company bllCompany = new BookingFood.BLL.bf_company();
            modelCompay = bllCompany.GetModel(userModel.company_id);
            List<BookingFood.Model.bf_company> list = bllCompany.GetModelList("RequestUserId=" + userModel.id);
            modelRegisterCompany = list.Count > 0 ? list[0] : null;
            BookingFood.BLL.bf_user_voucher bllVoucher = new BookingFood.BLL.bf_user_voucher();
            totalAmount = (int)(bllVoucher.GetModelList("UserId=" + userModel.id + " and GetDate()<ExpireTime  and Status=0 ").Sum(s=>s.Amount));
             useraccount = Convert.ToInt32(userModel.account);
            jsnoncestr = JSSDKHelper.GetNoncestr();
            jstimestamp = JSSDKHelper.GetTimestamp();
            string jsapi_token = JsApiTicketContainer.TryGetTicket(config.mp_slave_appid, config.mp_slave_appsecret);
            JSSDKHelper helper = new JSSDKHelper();
            mp_signature = helper.GetSignature(jsapi_token, jsnoncestr, jstimestamp, Context.Request.Url.ToString().Split('#')[0].Replace("/aspx", ""));
        }        
    }

}