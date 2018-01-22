using DTcms.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Orm.Son.Core;
using BookingFood.Model.Pay;

namespace DTcms.Web.tools
{
    /// <summary>
    /// vip_ajax 的摘要说明
    /// </summary>
    public class vip_ajax : IHttpHandler, IRequiresSessionState
    {
        private readonly SonConnection db;

        public vip_ajax()
        {
            db = new SonConnection();
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");
            string action = DTRequest.GetQueryString("action");
            switch (action)
            {
                case "vip_user_charge_list":
                    ChargeList();
                    break;
                default:
                    break;

            }
        }

        private void ChargeList()
        {

            var res1 = db.FindMany<dt_user_top_up>(t => t.IsDeleted == 0);
            var sqlstr = string.Format(@"SELECT w2.n, w1.* FROM dt_user_Top_up w1,
(SELECT TOP 100 row_number() OVER (ORDER BY CreateTime DESC, ID DESC) n, Id FROM dt_user_Top_up) w2 
WHERE w1.Id = w2.ID AND w2.n > 1000 ORDER BY w2.n ASC; ");
            var res = db.ExecuteSql(sqlstr);

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