using DTcms.Common;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.Helpers;
using Senparc.Weixin.MP.TenPayLibV3;
using System;
using System.Xml.Linq;

namespace DTcms.Web.api.payment.mppay
{
    public partial class index : System.Web.UI.Page
    {
        protected string appId = "";
        protected string timeStamp = "";
        protected string nonceStr = "";
        protected string package = "";
        protected string paySign = "";
        
        protected string jsnoncestr = string.Empty;
        protected string jstimestamp = string.Empty;
        protected decimal order_amount = 0.1M;
        protected string openid = string.Empty;
        protected string order_no = string.Empty;
        //读取站点配置信息
        Model.siteconfig siteConfig = new BLL.siteconfig().loadConfig(DTKeys.FILE_SITE_XML_CONFING);
        protected void Page_Load(object sender, EventArgs e)
        {
            jsnoncestr = JSSDKHelper.GetNoncestr();
            jstimestamp = JSSDKHelper.GetTimestamp();

            openid = DTRequest.GetQueryString("pay_user_name");
            
            //获得订单信息
            order_no = DTRequest.GetQueryString("pay_order_no"); //订单号
            order_amount = DTRequest.GetQueryDecimal("pay_order_amount", 0.1M); //订单金额
            
            //创建支付应答对象
            RequestHandler packageReqHandler = new RequestHandler(null);
            //初始化
            packageReqHandler.Init();
            //packageReqHandler.SetKey(""/*TenPayV3Info.Key*/);

            timeStamp = TenPayUtil.GetTimestamp();
            nonceStr = TenPayUtil.GetNoncestr();
            

            //设置package订单参数
            packageReqHandler.SetParameter("appid", siteConfig.mp_appid);		  //公众账号ID
            packageReqHandler.SetParameter("mch_id", TenPayV3Info.MchId);		  //商户号
            packageReqHandler.SetParameter("nonce_str", nonceStr);                    //随机字符串
            packageReqHandler.SetParameter("body", siteConfig.webname+"微信订单");
            packageReqHandler.SetParameter("out_trade_no", order_no);		//商家订单号
            packageReqHandler.SetParameter("total_fee", (order_amount * 100).ToString().Replace(".00", ""));			        //商品金额,以分为单位(money * 100).ToString()
            packageReqHandler.SetParameter("spbill_create_ip", Request.UserHostAddress);   //用户的公网ip，不是商户服务器IP
            packageReqHandler.SetParameter("notify_url", TenPayV3Info.TenPayV3Notify);		    //接收财付通通知的URL
            packageReqHandler.SetParameter("trade_type", TenPayV3Type.JSAPI.ToString());	                    //交易类型
            packageReqHandler.SetParameter("openid", openid);	                    //用户的openId
            //packageReqHandler.SetParameter("openid", "11");	                    //用户的openId

            string sign = packageReqHandler.CreateMd5Sign("key", TenPayV3Info.Key);
            packageReqHandler.SetParameter("sign", sign);	                    //签名
            
            string data = packageReqHandler.ParseXML();

            var result = TenPayV3.Unifiedorder(data);
            var res = XDocument.Parse(result);
            string prepayId = string.Empty;
            try
            {
                prepayId = res.Element("xml").Element("prepay_id").Value;
            }
            catch (Exception)
            {
                Log.Info(res.ToString());
            }
            

            //设置支付参数
            RequestHandler paySignReqHandler = new RequestHandler(null);
            paySignReqHandler.SetParameter("appId", siteConfig.mp_appid);
            paySignReqHandler.SetParameter("timeStamp", timeStamp);
            paySignReqHandler.SetParameter("nonceStr", nonceStr);
            paySignReqHandler.SetParameter("package", string.Format("prepay_id={0}", prepayId));
            paySignReqHandler.SetParameter("signType", "MD5");
            paySign = paySignReqHandler.CreateMd5Sign("key", TenPayV3Info.Key);

            appId = siteConfig.mp_appid;
            package = string.Format("prepay_id={0}", prepayId);
        }

        private TenPayV3Info _tenPayV3Info;
        public TenPayV3Info TenPayV3Info
        {
            get
            {
                if (_tenPayV3Info == null)
                {
                    _tenPayV3Info =
                        TenPayV3InfoCollection.Data[System.Configuration.ConfigurationManager.AppSettings["TenPayV3_MchId"]];
                }
                return _tenPayV3Info;
            }
        }

        protected string get_mp_signature()
        {
            string jsapi_token = JsApiTicketContainer.TryGetTicket(siteConfig.mp_appid, siteConfig.mp_appsecret);
            JSSDKHelper helper = new JSSDKHelper();
            return helper.GetSignature(jsapi_token, jsnoncestr, jstimestamp, Context.Request.Url.ToString().Split('#')[0]);
        }

        ////订单查询
        //public ActionResult OrderQuery()
        //{
        //    string nonceStr = TenPayV3Util.GetNoncestr();
        //    RequestHandler packageReqHandler = new RequestHandler(null);

        //    //设置package订单参数
        //    packageReqHandler.SetParameter("appid", TenPayV3Info.AppId);		  //公众账号ID
        //    packageReqHandler.SetParameter("mch_id", TenPayV3Info.MchId);		  //商户号
        //    packageReqHandler.SetParameter("transaction_id", "");       //填入微信订单号 
        //    packageReqHandler.SetParameter("out_trade_no", "");         //填入商家订单号
        //    packageReqHandler.SetParameter("nonce_str", nonceStr);             //随机字符串
        //    string sign = packageReqHandler.CreateMd5Sign("key", TenPayV3Info.Key);
        //    packageReqHandler.SetParameter("sign", sign);	                    //签名

        //    string data = packageReqHandler.ParseXML();

        //    var result = TenPayV3.OrderQuery(data);
        //    var res = XDocument.Parse(result);
        //    string openid = res.Element("xml").Element("sign").Value;

        //    return Content(openid);
        //}

        ////关闭订单接口
        //public ActionResult CloseOrder()
        //{
        //    string nonceStr = TenPayV3Util.GetNoncestr();
        //    RequestHandler packageReqHandler = new RequestHandler(null);

        //    //设置package订单参数
        //    packageReqHandler.SetParameter("appid", TenPayV3Info.AppId);		  //公众账号ID
        //    packageReqHandler.SetParameter("mch_id", TenPayV3Info.MchId);		  //商户号
        //    packageReqHandler.SetParameter("out_trade_no", "");                 //填入商家订单号
        //    packageReqHandler.SetParameter("nonce_str", nonceStr);              //随机字符串
        //    string sign = packageReqHandler.CreateMd5Sign("key", TenPayV3Info.Key);
        //    packageReqHandler.SetParameter("sign", sign);	                    //签名

        //    string data = packageReqHandler.ParseXML();

        //    var result = TenPayV3.CloseOrder(data);
        //    var res = XDocument.Parse(result);
        //    string openid = res.Element("xml").Element("openid").Value;

        //    return Content(openid);
        //}

        ////退款申请接口
        //public ActionResult Refund()
        //{
        //    string nonceStr = TenPayV3Util.GetNoncestr();
        //    RequestHandler packageReqHandler = new RequestHandler(null);

        //    //设置package订单参数
        //    packageReqHandler.SetParameter("appid", TenPayV3Info.AppId);		  //公众账号ID
        //    packageReqHandler.SetParameter("mch_id", TenPayV3Info.MchId);		  //商户号
        //    packageReqHandler.SetParameter("out_trade_no", "");                 //填入商家订单号
        //    packageReqHandler.SetParameter("out_refund_no", "");                //填入退款订单号
        //    packageReqHandler.SetParameter("total_fee", "");               //填入总金额
        //    packageReqHandler.SetParameter("refund_fee", "");               //填入退款金额
        //    packageReqHandler.SetParameter("op_user_id", TenPayV3Info.MchId);   //操作员Id，默认就是商户号
        //    packageReqHandler.SetParameter("nonce_str", nonceStr);              //随机字符串
        //    string sign = packageReqHandler.CreateMd5Sign("key", TenPayV3Info.Key);
        //    packageReqHandler.SetParameter("sign", sign);	                    //签名
        //    //退款需要post的数据
        //    string data = packageReqHandler.ParseXML();

        //    //退款接口地址
        //    string url = "https://api.mch.weixin.qq.com/secapi/pay/refund";
        //    //本地或者服务器的证书位置（证书在微信支付申请成功发来的通知邮件中）
        //    string cert = @"F:\apiclient_cert.p12";
        //    //私钥（在安装证书时设置）
        //    string password = "";
        //    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
        //    //调用证书
        //    X509Certificate2 cer = new X509Certificate2(cert, password, X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet);

        //    #region 发起post请求
        //    HttpWebRequest webrequest = (HttpWebRequest)HttpWebRequest.Create(url);
        //    webrequest.ClientCertificates.Add(cer);
        //    webrequest.Method = "post";

        //    byte[] postdatabyte = Encoding.UTF8.GetBytes(data);
        //    webrequest.ContentLength = postdatabyte.Length;
        //    Stream stream;
        //    stream = webrequest.GetRequestStream();
        //    stream.Write(postdatabyte, 0, postdatabyte.Length);
        //    stream.Close();

        //    HttpWebResponse httpWebResponse = (HttpWebResponse)webrequest.GetResponse();
        //    StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
        //    string responseContent = streamReader.ReadToEnd();
        //    #endregion

        //    var res = XDocument.Parse(responseContent);
        //    string openid = res.Element("xml").Element("out_refund_no").Value;

        //    return Content(openid);
        //}

        //private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        //{
        //    if (errors == SslPolicyErrors.None)
        //        return true;
        //    return false;
        //}

        ////红包
        ///// <summary>
        ///// 目前支持向指定微信用户的openid发放指定金额红包
        ///// 注意total_amount、min_value、max_value值相同
        ///// total_num=1固定
        ///// 单个红包金额介于[1.00元，200.00元]之间
        ///// </summary>
        ///// <returns></returns>
        //public ActionResult SendRedPack()
        //{
        //    string mchbillno = DateTime.Now.ToString("HHmmss") + TenPayV3Util.BuildRandomStr(28);

        //    string nonceStr = TenPayV3Util.GetNoncestr();
        //    RequestHandler packageReqHandler = new RequestHandler(null);

        //    //设置package订单参数
        //    packageReqHandler.SetParameter("nonce_str", nonceStr);              //随机字符串
        //    packageReqHandler.SetParameter("wxappid", TenPayV3Info.AppId);		  //公众账号ID
        //    packageReqHandler.SetParameter("mch_id", TenPayV3Info.MchId);		  //商户号
        //    packageReqHandler.SetParameter("mch_billno", mchbillno);                 //填入商家订单号
        //    packageReqHandler.SetParameter("nick_name", "提供方名称");                 //提供方名称
        //    packageReqHandler.SetParameter("send_name", "红包发送者名称");                 //红包发送者名称
        //    packageReqHandler.SetParameter("re_openid", "接受收红包的用户的openId");                 //接受收红包的用户的openId
        //    packageReqHandler.SetParameter("total_amount", "100");                //付款金额，单位分
        //    packageReqHandler.SetParameter("min_value", "100");                //最小红包金额，单位分
        //    packageReqHandler.SetParameter("max_value", "100");                //最大红包金额，单位分
        //    packageReqHandler.SetParameter("total_num", "1");               //红包发放总人数
        //    packageReqHandler.SetParameter("wishing", "红包祝福语");               //红包祝福语
        //    packageReqHandler.SetParameter("client_ip", Request.UserHostAddress);               //调用接口的机器Ip地址
        //    packageReqHandler.SetParameter("act_name", "活动名称");   //活动名称
        //    packageReqHandler.SetParameter("remark", "备注信息");   //备注信息
        //    string sign = packageReqHandler.CreateMd5Sign("key", TenPayV3Info.Key);
        //    packageReqHandler.SetParameter("sign", sign);	                    //签名
        //    //退款需要post的数据
        //    string data = packageReqHandler.ParseXML();

        //    //发红包接口地址
        //    string url = "https://api.mch.weixin.qq.com/mmpaymkttransfers/sendredpack";
        //    //本地或者服务器的证书位置（证书在微信支付申请成功发来的通知邮件中）
        //    string cert = @"F:\apiclient_cert.p12";
        //    //私钥（在安装证书时设置）
        //    string password = "";
        //    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
        //    //调用证书
        //    X509Certificate2 cer = new X509Certificate2(cert, password, X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet);

        //    #region 发起post请求
        //    HttpWebRequest webrequest = (HttpWebRequest)HttpWebRequest.Create(url);
        //    webrequest.ClientCertificates.Add(cer);
        //    webrequest.Method = "post";

        //    byte[] postdatabyte = Encoding.UTF8.GetBytes(data);
        //    webrequest.ContentLength = postdatabyte.Length;
        //    Stream stream;
        //    stream = webrequest.GetRequestStream();
        //    stream.Write(postdatabyte, 0, postdatabyte.Length);
        //    stream.Close();

        //    HttpWebResponse httpWebResponse = (HttpWebResponse)webrequest.GetResponse();
        //    StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
        //    string responseContent = streamReader.ReadToEnd();
        //    #endregion

        //    return Content(responseContent);
        //}
    }
}