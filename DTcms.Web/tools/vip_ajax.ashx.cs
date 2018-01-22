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
                case "add_user_charge":
                    AddCharge(context);
                    break;
                default:
                    break;

            }
        }

        private void ChargeList()
        {
            var pageindex = 10;
            var page = 1;
            var wheresql = " 1=1";
            var begin = DTRequest.GetFormString("begin");
            var end = DTRequest.GetFormString("end");
            if (!string.IsNullOrEmpty(begin))
            {
                wheresql += " AND w2.CreateTime>=" + begin;
            }
            if (!string.IsNullOrEmpty(end))
            {
                wheresql += " AND w2.CreateTime<=" + end;
            }
            var sqlstr = string.Format(@"SELECT w2.n, w1.* FROM dt_user_Top_up w1,
(SELECT TOP {0} row_number() OVER (ORDER BY CreateTime DESC, Id DESC) n, Id FROM dt_user_Top_up) w2 
WHERE w1.Id = w2.Id AND w2.n > {1} AND {2} ORDER BY w2.n ASC ", pageindex * page, (page - 1) * pageindex);
            var res = db.ExecuteSql(sqlstr);

        }

        private void AddCharge(HttpContext context)
        {
            var OpenId = DTRequest.GetFormString("OpenId");
            var Amount = DTRequest.GetFormString("Amount");
            var AreaId = DTRequest.GetFormString("AreaId");
            var AreaName = DTRequest.GetFormString("AreaName");
            var NickName = DTRequest.GetFormString("NickName");
            var userId = DTRequest.GetFormString("UserId");
            if (string.IsNullOrEmpty(userId)) userId = "0";
            if (string.IsNullOrEmpty(AreaId)) AreaId = "0";
            if (string.IsNullOrEmpty(Amount)) Amount = "0";
            var req = new dt_user_top_up()
            {
                OpenId = OpenId,
                Amount = Convert.ToDecimal(Amount),
                AreaId = Convert.ToInt32(AreaId),
                AreaName = AreaName,
                NickName = NickName,
                CreateTime = DateTime.Now,
                IsDeleted=0,
                Stage=0,
                UserId= Convert.ToInt32(userId)
            };
            var res = db.Insert(req);
            if (res > 0)
            {
                context.Response.Write("{\"msg\":1, \"msgbox\": 新增成功}");
            }
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