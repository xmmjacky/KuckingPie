using BookingFood.BLL;
using DTcms.BLL;
using DTcms.Common;
using DTcms.Web.tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using TeeGonSdk.Util;

namespace LuckingPieTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //Test();
            //xmlf();

           // var body= @"{"result":{"charge_id":"1017815794471411712","domain_id":"5678f022","order_no":"JH153149843102","amount":1,"channel":"alipay_ws","subject":"Luckingpie馍王微信订单","device_id":"iZ236ejklutZ","paid":false,"created":1531501170,"refund":false,"transaction_no":"","action":{"type":"js","url":"https://api.teegon.com/app/checkout/alipaysign?id=1017815794471411712\u0026channel=alipay_ws\u0026t=1531501170","params":"var img = window.document.createElement(\"img\");\n\t\t\timg.src = \"https://qr.teegon.com/qr/get_qrcode?url=https%3A%2F%2Fapi.teegon.com%2Fapp%2Fcheckout%2Falipaysign%3Fid%3D1017815794471411712%26channel%3Dalipay_ws%26t%3D1531501170\";\n\t\t\tdocument.getElementById('native').appendChild(img);"}}}";
            var a = new A();
            a.TianGong();
        }


            static void Test()
            {
                string position = "31.229018,121.470183";
                string openid = "omc8At4ULCBtNZxm9aogl7aBLCEs";
               
               bf_area bllArea = new BookingFood.BLL.bf_area();
                string prevAreaId = string.Empty;
                if (!string.IsNullOrEmpty(openid))
                {
                    orders bll = new orders();
                    DataTable dt = bll.GetList(1, " user_name='" + openid + "' And takeout in (1,2)", " id desc").Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["takeout"].ToString() == "2")
                        {
                            BookingFood.Model.bf_area areaModel = bllArea.GetModelList("OppositeId=" + dt.Rows[0]["area_id"].ToString())[0];
                            prevAreaId = areaModel.Id.ToString();
                        }
                        else
                        {
                            prevAreaId = dt.Rows[0]["area_id"].ToString();
                        }

                    }
                }
                //Log.Info("ip:" + Common.DTRequest.GetIP() + " " + position + " " + openid);

                DataTable dtArea = bllArea.
                        GetList(" IsShow=0 AND IsLock=0 AND ParentId=1 Order By SortId Asc").Tables[0];
                string rtn = "{\"Status\":0}";
                foreach (DataRow item in dtArea.Rows)
                {
                    if (string.IsNullOrEmpty(item["DistributionArea"].ToString().Trim())) continue;
                    bool isInArea = Polygon.GetResult(position, item["DistributionArea"].ToString());
                    if (isInArea)
                    {
                        if (string.IsNullOrEmpty(prevAreaId) || prevAreaId != item["Id"].ToString())
                        {
                            rtn = "{\"Status\":1,\"Id\":" + item["Id"].ToString() + ",\"Title\":\"" + item["Title"].ToString()
                                + "\",\"ShowConfirm\":1,\"Address\":\"" + item["Address"].ToString() + "\"}";
                        }
                        else
                        {
                            rtn = "{\"Status\":1,\"Id\":" + item["Id"].ToString() + ",\"Title\":\"" + item["Title"].ToString() + "\",\"ShowConfirm\":0}";
                        }

                        break;
                    }
                }

            }
        
        static void xmlf()
        {
            var data = "{\"alipay_trade_precreate_response\":{\"code\":\"10000\",\"msg\":\"Success\",\"out_trade_no\":\"HA152812825639\",\"qr_code\":\"https:\\/\\/ qr.alipay.com\\/ bax03771cve7qfxo4wsv6098\"},\"sign\":\"l0PbHeWI0chlThfU7QgkrS1uzNT7mqte / GJ / acBhAsIkqlfslQTnhEzWD7HOWKFXgvS0y1avTBG8lDq5lOzVAofRzLszLutwc4t5jnLrXnYu1fWWkTuC4Iz5IZEPx / +aTmK77gOqSvZdDPRYa2YHv5jFgomTk9t3yTsTq9qzmn0 = \"}";
            //XmlDocument doc = new XmlDocument();
            //doc.LoadXml(data.Trim());
            //var res = XDocument.Parse(data);
            AliPayDto AliPay = new AliPayDto();
            var res = JsonConvert.DeserializeObject<AliPayDto>(data.Trim());
            
        }


       

    }
}
