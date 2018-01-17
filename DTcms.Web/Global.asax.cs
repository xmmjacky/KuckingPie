using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Senparc.Weixin.MP.TenPayLibV3;
using DTcms.Common;
using System.Data;
using System.Net;

namespace DTcms.Web
{
    public class Global : System.Web.HttpApplication
    {
        public static string GetValueByTag(string html, string starttag, string endtag)
        {
            if (html.IndexOf(starttag) == -1) return string.Empty;
            int startpox = html.IndexOf(starttag) + starttag.Length;
            int endpox = html.IndexOf(endtag, startpox);
            if (startpox > 0 && endpox > 0)
            {
                return html.Substring(startpox, html.IndexOf(endtag, startpox) - startpox);
            }
            return string.Empty;
        }
        

        protected void Application_Start(object sender, EventArgs e)
        {
            //BookingFood.BLL.bf_area bllArea = new BookingFood.BLL.bf_area();
            //DataTable dtArea = bllArea.
            //        GetList(" IsShow=1 AND ParentId=1 Order By SortId Asc").Tables[0];
            //string url = string.Empty;
            //DTcms.BLL.users bllusers = new BLL.users();
            //DataTable dt = bllusers.GetList(1000,"[address] IS NOT NULL AND [address]<>''", " id desc").Tables[0];
            //string areaId = string.Empty, areaTitle = string.Empty, areaType = string.Empty;
            //BookingFood.BLL.bf_user_address bllUserAddress = new BookingFood.BLL.bf_user_address();
            //foreach (DataRow item in dt.Rows)
            //{
            //    areaId = string.Empty;
            //    areaTitle = string.Empty;
            //    areaType = string.Empty;
            //    int existAddress = bllUserAddress.GetRecordCount(string.Format("UserId={0} And Address='{1}' And Telphone='{2}'"
            //        , item["id"].ToString(), item["address"].ToString(), item["telphone"].ToString()));
            //    if (existAddress == 1)
            //    {
            //        continue;
            //    }
            //    url = "http://apis.map.qq.com/ws/geocoder/v1/?key=BOEBZ-2AB2R-IKTWG-W2JQG-HEUOV-2RF7Z&address=" + item["address"].ToString();
            //    WebClient wc = new WebClient();
            //    string json = wc.DownloadString(url);
            //    System.Threading.Thread.Sleep(300);
            //    string lng = GetValueByTag(json, "lng\": ", ",");
            //    string lat = GetValueByTag(json, "lat\": ", "\n");
            //    string position = lng + "," + lat;
            //    foreach (DataRow itemArea in dtArea.Rows)
            //    {
            //        if (string.IsNullOrEmpty(itemArea["DistributionArea"].ToString())) continue;
            //        bool isInArea = Polygon.GetResult(position, itemArea["DistributionArea"].ToString());
            //        if (isInArea)
            //        {
            //            areaId = item["Id"].ToString();
            //            areaTitle = item["Title"].ToString();
            //            areaType = "1";
            //            break;
            //        }
            //        else
            //        {
            //            isInArea = Polygon.GetResult(position, itemArea["DistributionArea_2"].ToString());
            //            if (isInArea)
            //            {
            //                areaId = item["Id"].ToString();
            //                areaTitle = item["Title"].ToString();
            //                areaType = "2";
            //                break;
            //            }
            //        }
            //    }
            //    if (string.IsNullOrEmpty(areaId)) continue;
            //    bllUserAddress.Add(new BookingFood.Model.bf_user_address()
            //    {
            //        Address = item["address"].ToString(),
            //        AreaId = int.Parse(areaId),
            //        AreaTitle = areaTitle,
            //        AreaType = int.Parse(areaType),
            //        NickName = item["nick_name"].ToString(),
            //        Telphone = item["telphone"].ToString(),
            //        UserId = int.Parse(item["id"].ToString())
            //    });
            //}
            








            var tenPayV3_MchId = System.Configuration.ConfigurationManager.AppSettings["TenPayV3_MchId"];
            var tenPayV3_Key = System.Configuration.ConfigurationManager.AppSettings["TenPayV3_Key"];
            var tenPayV3_AppId = System.Configuration.ConfigurationManager.AppSettings["TenPayV3_AppId"];
            var tenPayV3_AppSecret = System.Configuration.ConfigurationManager.AppSettings["TenPayV3_AppSecret"];
            var tenPayV3_TenpayNotify = System.Configuration.ConfigurationManager.AppSettings["TenPayV3_TenpayNotify"];
            var tenPayV3Info = new TenPayV3Info(tenPayV3_AppId, tenPayV3_AppSecret, tenPayV3_MchId, tenPayV3_Key,
                                                tenPayV3_TenpayNotify);
            TenPayV3InfoCollection.Register(tenPayV3Info);
            
            
#if DEBUG
            BLL.siteconfig bll = new BLL.siteconfig();
            Model.siteconfig model = bll.loadConfig(Utils.GetXmlMapPath(DTKeys.FILE_SITE_XML_CONFING));
            if (model.RunLoopThirdOrder == 1)
            {
                //DTcms.API.SyncOrder.SyncOrder.SyncOrderFromMeituan();
                //DTcms.API.SyncOrder.SyncOrder.SyncOrderFromBaidu();
                //DTcms.API.SyncOrder.SyncOrder.SyncOrderFromEleme();
            }
#else
            //订单同步轮询机制
            System.Timers.Timer myTimer = new System.Timers.Timer();
            myTimer.Interval = 1000 * 60;
            myTimer.Enabled = true;
            myTimer.Elapsed += MyTimer_Elapsed;
#endif

        }

        private void MyTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            BLL.siteconfig bll = new BLL.siteconfig();
            Model.siteconfig model = bll.loadConfig(Utils.GetXmlMapPath(DTKeys.FILE_SITE_XML_CONFING));
            if(model.RunLoopThirdOrder==1)
            {
                DTcms.API.SyncOrder.SyncOrder.SyncOrderFromMeituan();
                DTcms.API.SyncOrder.SyncOrder.SyncOrderFromEleme();
                DTcms.API.SyncOrder.SyncOrder.SyncOrderFromBaidu();
            }
            
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

#if DEBUG
            
#else
            Exception ex = Server.GetLastError();
            StringBuilder sb = new StringBuilder();
            string dt = DateTime.Now.ToString();
            string strCookie = string.Empty;
            for (int i = 0; i < HttpContext.Current.Request.Cookies.Count; i++)
			{
                strCookie += HttpContext.Current.Request.Cookies.Get(i).Name + ":" + Common.Utils.UrlDecode(HttpContext.Current.Request.Cookies.Get(i).Value) +":"
                    + HttpContext.Current.Request.Cookies.Get(i).Expires.ToString();
			}
            sb.Append("------<br/>").Append(dt).Append("<br/>内部错误:").Append(ex.InnerException != null ? ex.InnerException.ToString() : "")
            .Append("<br/>堆栈:").Append(ex.StackTrace).Append("<br/>内容:").Append(ex.Message)
            .Append("<br/>来源:").Append(ex.Source).Append("<br/>")
            .Append("<br/>FormParam:").Append(Common.Utils.UrlDecode(HttpContext.Current.Request.Form.ToString())).Append("<br/>")
            .Append("<br/>GuestIp:").Append(Common.DTRequest.GetIP()).Append("<br/>")
            .Append("<br/>CookieParam:").Append(strCookie).Append("<br/>");
            Model.siteconfig siteConfig = new BLL.siteconfig().loadConfig(DTcms.Common.Utils.GetXmlMapPath(DTcms.Common.DTKeys.FILE_SITE_XML_CONFING));
            DTcms.Common.DTMail.sendMail(siteConfig.emailstmp,
                        siteConfig.emailusername,
                        DTcms.Common.DESEncrypt.Decrypt(siteConfig.emailpassword),
                        siteConfig.emailnickname,
                        siteConfig.emailfrom,
                        "frank3660@msn.com",
                        "馍王报错_"+ex.Message, sb.ToString());
            Log.Info(sb.ToString());
#endif

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}