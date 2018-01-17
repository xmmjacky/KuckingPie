using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DTcms.Common;
using DTcms.API.Payment.Alipay_Pc;

namespace DTcms.Web.api.payment.alipay_pc
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ////////////////////////////////////////////请求参数////////////////////////////////////////////

            //支付类型
            string payment_type = "1";
            //必填，不能修改
            //服务器异步通知页面路径
            string notify_url = Config.Notify_url;
            //需http://格式的完整路径，不能加?id=123这类自定义参数

            //页面跳转同步通知页面路径
            string return_url = Config.Return_url;
            //需http://格式的完整路径，不能加?id=123这类自定义参数，不能写成http://localhost/

            //读取站点配置信息
            Model.siteconfig siteConfig = new BLL.siteconfig().loadConfig(DTKeys.FILE_SITE_XML_CONFING);

            //获得订单信息
            //商户订单号
            string out_trade_no = DTRequest.GetQueryString("pay_order_no");//商户网站订单系统中唯一订单号，必填
            decimal order_amount = DTRequest.GetQueryDecimal("pay_order_amount", 0);
            string user_name = DTRequest.GetQueryString("pay_user_name");
            string subject = DTRequest.GetQueryString("pay_subject");
            if (out_trade_no == "" || order_amount == 0 || user_name == "")
            {
                Response.Redirect(siteConfig.webpath + "error.aspx?msg=" + Utils.UrlEncode("对不起，您提交的参数有误！"));
                return;
            }
            
            //付款金额
            string total_fee = order_amount.ToString();
            //必填

            //订单描述

            string body = "支付会员_" + user_name;
            //商品展示地址
            string show_url = siteConfig.weburl.TrimEnd('/')+"/index.aspx";
            //需以http://开头的完整路径，例如：http://www.商户网址.com/myorder.html

            //防钓鱼时间戳
            string anti_phishing_key = "";
            //若要使用请调用类文件submit中的query_timestamp函数

            //客户端的IP地址
            string exter_invoke_ip = "";
            //非局域网的外网IP地址，如：221.0.0.1
            

            ////////////////////////////////////////////////////////////////////////////////////////////////

            //把请求参数打包成数组
            SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            sParaTemp.Add("service", "create_direct_pay_by_user");
            sParaTemp.Add("partner", Config.Partner);
            sParaTemp.Add("_input_charset", Config.Input_charset.ToLower());
            sParaTemp.Add("notify_url", notify_url);
            sParaTemp.Add("return_url", return_url);
            sParaTemp.Add("out_trade_no", out_trade_no);
            sParaTemp.Add("subject", subject);
            sParaTemp.Add("payment_type", payment_type);
            sParaTemp.Add("total_fee", total_fee);
            //sParaTemp.Add("seller_id", Config.Seller_email);
            sParaTemp.Add("seller_email", Config.Seller_email);

            sParaTemp.Add("body", body);
            sParaTemp.Add("show_url", show_url);
            //sParaTemp.Add("anti_phishing_key", anti_phishing_key);
            //sParaTemp.Add("exter_invoke_ip", exter_invoke_ip);

            //建立请求
            string sHtmlText = Submit.BuildRequest(sParaTemp, "get", "确认");
            Response.Write(sHtmlText);

        }
    }
}