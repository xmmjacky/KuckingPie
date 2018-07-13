using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using DTcms.Common;
using Newtonsoft.Json.Linq;

namespace DTcms.Web.UI.Page
{
    public partial class payment : Web.UI.BasePage
    {
        protected string action = string.Empty, url = string.Empty, takeout=string.Empty, mpforhere=string.Empty;
        

        /// <summary>
        /// 重写父类的虚方法,此方法将在Init事件前执行
        /// </summary>
        protected override void ShowPage()
        {
            this.Init += new EventHandler(payment_Init); //加入Init事件
        }

        /// <summary>
        /// 将在Init事件执行
        /// </summary>
        protected void payment_Init(object sender, EventArgs e)
        {

            url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + config.mp_slave_appid + "&redirect_uri=https%3A%2F%2Fwww.4008317417.cn%2fmp_index.aspx&response_type=code&scope=snsapi_userinfo&state=" + state + "#wechat_redirect";
            action = DTRequest.GetQueryString("metadata");
            action = Common.Utils.UrlDecode(action);
            JObject jsonObj = JObject.Parse(action);
            takeout = ((JValue)jsonObj["takeout"]).Value.ToString();
            mpforhere = ((JValue)jsonObj["mpforhere"]).Value.ToString();
        }

        protected void IsValidParams()
        {

        }
    }
}
    