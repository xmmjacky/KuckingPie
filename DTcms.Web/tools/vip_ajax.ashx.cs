using DTcms.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Orm.Son.Core;
using BookingFood.Model.Pay;
using System.Web.Script.Serialization;

namespace DTcms.Web.tools
{
    /// <summary>
    /// vip_ajax 的摘要说明
    /// </summary>
    public class vip_ajax : IHttpHandler, IRequiresSessionState
    {
        private readonly string strcon = "ConnectionString";


        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string action = DTRequest.GetQueryString("action");
            switch (action)
            {
                case "add_user_charge":
                    AddCharge(context);
                    break;
                case "change_area":
                    ChangeArea(context);
                    break;
                case "ChangeState":
                    ChangeState(context);
                    break;
                case "Order_account":
                default:
                    break;

            }
        }

        private void AddCharge(HttpContext context)
        {
            var OpenId = DTRequest.GetFormString("OpenId");
            var Amount = DTRequest.GetFormString("Amount");
            var AreaId = DTRequest.GetFormString("AreaId");
            var AreaName = DTRequest.GetFormString("AreaName");
            var NickName = DTRequest.GetFormString("NickName");
            var userId = DTRequest.GetFormString("serId");
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
                IsDeleted = 0,
                State = 0,
                UserId = Convert.ToInt32(userId)
            };
            using (var db = new SonConnection(strcon))
            {
                var res = db.Insert(req);
                if (res > 0)
                {
                    context.Response.Write("{\"msg\":1, \"msgbox\": 新增成功}");
                }
                else
                {
                    context.Response.Write("{\"msg\":-1, \"msgbox\": 新增失败}");

                }
            }

        }

        private void ChangeArea(HttpContext context)
        {
            var retres = new Reslt();
            string orderid = context.Request.Params["id"];
            string areaid = context.Request.Params["changeareaid"];
            string areaname = context.Request.Params["changeareaname"];
            var req = new dt_user_top_up()
            {
                Id = Convert.ToInt32(orderid),
                AreaId = Convert.ToInt32(areaid),
                AreaName = areaname
            };
            var strsql = string.Format(@"update dt_user_Top_up set AreaId={0},AreaName='{1}' where id={2}", req.AreaId, req.AreaName, req.Id);
            using (var db = new SonConnection(strcon))
            {
                //var res = db.ExecuteSql(strsql);
                var userinfo = db.Find<dt_user_top_up>(req.Id);
                userinfo.AreaId = req.AreaId;
                userinfo.AreaName = req.AreaName;
                var res = db.Update(userinfo);
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                if (Convert.ToInt32(res) > 0)
                {
                    retres.msg = 1;
                    retres.msgbox = "修改成功!";
                    context.Response.Write(serializer.Serialize(retres));

                }
                else
                {
                    retres.msg = 0;
                    retres.msgbox = "修改失败!";
                    context.Response.Write(serializer.Serialize(retres));
                }
            }
        }

        private void ChangeState(HttpContext context)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var retres = new Reslt();
            string reqid = context.Request.Params["id"];
            string reqstate = context.Request.Params["state"];
            var id = Convert.ToInt32(reqid);
            var state = Convert.ToInt32(reqstate);
            using (var db = new SonConnection(strcon))
            {
                var userinfo = db.Find<dt_user_top_up>(id);
                if (userinfo.State != 0)
                {
                    retres.msg = 0;
                    retres.msgbox = "已确认!";
                    context.Response.Write(serializer.Serialize(retres));
                    return;
                }
                userinfo.State = state;
                var res = db.Update(userinfo);

                if (Convert.ToInt32(res) > 0)
                {
                    #region 更新金额
                    var userconfirm = new dt_user_confirm_top()
                    {
                        IsDeleted = 0,
                        AddAmount= (userinfo.Amount * 100).ToString(),
                        Amount = userinfo.Amount * 100 + userinfo.Amount,
                        OpneId = userinfo.OpenId,
                        AreaId = userinfo.AreaId,
                        AreaName = userinfo.AreaName,
                        NickName = userinfo.NickName,
                        UserId = userinfo.UserId
                    };
                    db.Insert(userconfirm);

                    var useraccountstr = string.Format(@" update dt_users set account=account+{0} where id={1}", userconfirm.Amount,userconfirm.UserId);
                    db.ExecuteSql(useraccountstr);
                    #endregion
                    retres.msg = 1;
                    retres.msgbox = "更新成功!";
                    context.Response.Write(serializer.Serialize(retres));

                }
                else
                {
                    retres.msg = 0;
                    retres.msgbox = "更新失败!";
                    context.Response.Write(serializer.Serialize(retres));
                }
            }
        }

        private void AccountPay(HttpContext context)
        {

        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
    public class Reslt
    {
        public int msg { get; set; }

        public string msgbox { get; set; }
    }

}