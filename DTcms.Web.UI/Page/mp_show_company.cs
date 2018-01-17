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
    public partial class mp_show_company : Web.UI.BasePage
    {
        public BookingFood.Model.bf_company modelCompay = null;

        protected string jsnoncestr = string.Empty;
        protected string jstimestamp = string.Empty;
        protected string mp_signature = string.Empty, distributionArea = string.Empty;

        protected override void ShowPage()
        {
            this.Init += mp_discount_Init;       
            
        }

        void mp_discount_Init(object sender, EventArgs e)
        {
            InitByMp();
            BookingFood.BLL.bf_company bllCompany = new BookingFood.BLL.bf_company();
            int companyId = DTRequest.GetQueryInt("companyid");
            modelCompay = bllCompany.GetModel(companyId);

            jsnoncestr = JSSDKHelper.GetNoncestr();
            jstimestamp = JSSDKHelper.GetTimestamp();
            string jsapi_token = JsApiTicketContainer.TryGetTicket(config.mp_slave_appid, config.mp_slave_appsecret);
            JSSDKHelper helper = new JSSDKHelper();
            mp_signature = helper.GetSignature(jsapi_token, jsnoncestr, jstimestamp, Context.Request.Url.ToString().Split('#')[0].Replace("/aspx", ""));
        }        
    }

}