using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeeGonSdk.Util;

namespace LuckingPieTest
{
   public class A
    {
        public void TianGong()
        {
            var url = "https://api.teegon.com/v1/charge";
            IDictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("amount", "1.0");
            dic.Add("out_order_no", "JH153149843102");
            dic.Add("pay_channel", "alipay");
            dic.Add("ip", "114.94.109.63");
            dic.Add("subject", "Luckingpie馍王微信订单");
            dic.Add("return_url", "https://www.4008317417.cn/api/payment/teegon_wxpay/feedback_offline.aspx");
            dic.Add("notify_url", "https://www.4008317417.cn/api/payment/teegon_jsapi/feedback2.aspx");
            dic.Add("metadata", "2018-07-14 00:13:50.449");
            //dic.Add("auth_code", "285905526662001921");
            dic.Add("device_id", "iZ236ejklutZ");
            dic.Add("charge_type", "pay");
            dic.Add("account_id", "main");
            dic.Add("client_id", "bxkgovptblsbxe4zyi7ixbdh");
            dic.Add("client_secret", "ot5rhjgescrhcewcex65uamkcypaaxfu");
            dic.Add("sign", "094BC31768124C334B310C00625D1931");

            IDictionary<string, string> dic1 = new Dictionary<string, string>();
            dic1.Add("Accept-Encoding", "gzip");
            var str = new WebUtils().DoPost(url, dic, dic1);
            str = str.Replace("\"result\": \"error\"", "\"result\": null").Replace("\"result\":\"error\"", "\"result\":null");
        }
    }
}
