using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DTcms.Common;

namespace DTcms.Web.admin.ashx
{
    /// <summary>
    /// business 的摘要说明
    /// </summary>
    public class business : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //取得处事类型
            string action = DTRequest.GetQueryString("action");
            switch (action)
            {
                //case "digg_add": //顶踩
                //    digg_add(context);
                //    break;                

            }
        }

        private void a(HttpContext context)
        {
            
            context.Response.Write("{\"msg\":1, \"msgbox\":\"申请邀请码已成功！\"}");
            return;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}