using DTcms.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace DTcms.Web.tools
{
    /// <summary>
    /// vip_ajax 的摘要说明
    /// </summary>
    public class vip_ajax : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");
            string action = DTRequest.GetQueryString("action");
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