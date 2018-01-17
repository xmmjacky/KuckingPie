using DTcms.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.IO;
using static DTcms.API.SyncOrder.SyncOrder;

namespace DTcms.Web.api.push_order.eleme
{
    /// <summary>
    /// index 的摘要说明
    /// </summary>
    public class index : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string json = string.Empty;
            if (HttpContext.Current.Request.HttpMethod.ToUpper() == "POST")
            {
                using (Stream stream = HttpContext.Current.Request.InputStream)
                {
                    Byte[] postBytes = new Byte[stream.Length];
                    stream.Read(postBytes, 0, (Int32)stream.Length);
                    json = Encoding.UTF8.GetString(postBytes);
                    JObject jsonObj = null;
                    jsonObj = JObject.Parse(json);
                    switch(jsonObj["type"].ToString())
                    {
                        case "10":
                            BookingFood.BLL.bf_area bllArea = new BookingFood.BLL.bf_area();
                            List<BookingFood.Model.bf_area> list = bllArea.GetModelList("ElemeCookie!=''");
                            ElemeToken elemeToken = new API.SyncOrder.SyncOrder.ElemeToken();
                            foreach (var itemArea in list)
                            {
                                elemeToken = GetElemeToken(itemArea);
                                if (elemeToken == null) continue;
                                if (elemeToken.ShopId == jsonObj["shopId"].ToString())
                                {
                                    string unixstamp = GetUnixStamp();
                                    JObject jsonOrder = JObject.Parse(jsonObj["message"].ToString());
                                    json = PostData("https://open-api.shop.ele.me/api/v1/"
                                        , "{\"token\":\"" + elemeToken.Token + "\",\"nop\":\"1.0.0\",\"metas\":{\"app_key\":\"" + elemeToken.Key + "\",\"timestamp\":" + unixstamp
                                        + "},\"params\":{\"orderId\":\"" + jsonOrder["orderId"].ToString() + "\"},\"action\":\"eleme.order.confirmOrderLite\",\"id\":\"" + Guid.NewGuid().ToString() + "\",\"signature\":\"" +
                                        GetMd5("eleme.order.confirmOrderLite" + elemeToken.Token + "app_key=\"" + elemeToken.Key + "\"orderId=\"" + jsonOrder["orderId"].ToString() + "\"timestamp=" + unixstamp + elemeToken.Secret).ToUpper()
                                        + "\"}");
                                }
                            }
                            break;
                    }
                    context.Response.Write("{\"message\":\"ok\"}");
                }
            }
            else
            {
                context.Response.Write("{\"message\":\"ok\"}");
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