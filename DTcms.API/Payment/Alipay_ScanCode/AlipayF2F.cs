using Aop.Api;
using Aop.Api.Request;
using Aop.Api.Response;
using DTcms.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace DTcms.API.Payment.Alipay_ScanCode
{
    public class AlipayF2F
    {
        private static IAopClient client = new DefaultAopClient(Config.serverUrl, Config.appId, Config.merchant_private_key, "json", Config.version,
            Config.sign_type, Config.alipay_public_key, Config.charset);

        public static string PayOrder(string out_trade_no,string auth_code,string total_amount,string area_title,string area_id)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"out_trade_no\":\"" + out_trade_no + "\",");
            sb.Append("\"scene\":\"bar_code\",");
            sb.Append("\"auth_code\":\"" + auth_code + "\",");
            sb.Append("\"total_amount\":\"" + total_amount + "\",\"discountable_amount\":\"0.00\",");
            sb.Append("\"subject\":\"LUCKING'PIE 馍王-" + area_title + "-条码支付\",");
            sb.Append("\"store_id\":\""+ area_id + "\",\"terminal_id\":\"t_001\",");

            string expire_time = System.DateTime.Now.AddHours(1).ToString("yyyy-MM-dd HH:mm:ss");
            sb.Append("\"time_expire\":\"" + expire_time + "\"}");

            AlipayTradePayResponse payResponse = Pay(sb.ToString());

            string result = payResponse.Body;

            if (payResponse != null)
            {

                switch (payResponse.Code)
                {
                    case ResultCode.SUCCESS:
                        //System.Console.Write("支付成功");
                        //result = payResponse.Body;
                        result = "支付成功";
                        break;


                    case ResultCode.INRROCESS:
                        StringBuilder sb1 = new StringBuilder();
                        sb1.Append("{\"out_trade_no\":\"" + out_trade_no + "\"}");

                        //根据业务需要，选择是否新起线程进行轮询
                        //ParameterizedThreadStart ParStart = new ParameterizedThreadStart(LoopQuery);
                        //Thread myThread = new Thread(ParStart);
                        //object o = payResponse;
                        //myThread.Start(o);

                        //返回支付处理中，需要进行轮询
                        Common.Log.Info(out_trade_no + "扫码支付进入轮询");
                        AlipayTradeQueryResponse queryResponse = LoopQuery(sb1.ToString());   //用订单号trade_no进行轮询也是可以的。
                        if (queryResponse != null)
                        {
                            if(queryResponse.Code==ResultCode.SUCCESS)
                            {
                                if (queryResponse.TradeStatus == "TRADE_FINISHED"
                                || queryResponse.TradeStatus == "TRADE_SUCCESS"
                                || queryResponse.TradeStatus == "TRADE_CLOSED")
                                {
                                    result = "支付成功";
                                }
                                else
                                {
                                    result = "支付失败";
                                }
                            }
                            else
                            {
                                result = queryResponse.SubMsg;
                            }
                        }
                        else
                        {
                            result = "支付失败";
                        }
                        break;

                    case ResultCode.FAIL:
                        StringBuilder sb2 = new StringBuilder();
                        sb2.Append("{\"out_trade_no\":\"" + out_trade_no + "\"}");
                        Cancel(sb2.ToString());
                        result = payResponse.SubMsg;
                        break;

                }
            }

            return result;
        }

        private static AlipayTradePayResponse Pay(string biz_content)
        {
            AlipayTradePayRequest payRequst = new AlipayTradePayRequest();
            payRequst.BizContent = biz_content;

            Dictionary<string, string> paramsDict = (Dictionary<string, string>)payRequst.GetParameters();
            //new DefaultAopClient(Config.serverUrl, Config.appId, Config.merchant_private_key);

            AlipayTradePayResponse payResponse = client.Execute(payRequst);
            return payResponse;
        }

        private static AlipayTradeCancelResponse Cancel(string biz_content)
        {
            AlipayTradeCancelRequest cancelRequest = new AlipayTradeCancelRequest();
            cancelRequest.BizContent = biz_content;
            AlipayTradeCancelResponse cancelResponse = client.Execute(cancelRequest);


            if (null != cancelResponse)
            {
                if (cancelResponse.Code == ResultCode.FAIL && cancelResponse.RetryFlag == "Y")
                {
                    //if (cancelResponse.Body.Contains("\"retry_flag\":\"Y\""))		
                    //cancelOrderRetry(biz_content);
                    // 新开一个线程重试撤销
                    ParameterizedThreadStart ParStart = new ParameterizedThreadStart(CancelOrderRetry);
                    Thread myThread = new Thread(ParStart);
                    object o = biz_content;
                    myThread.Start(o);
                }
            }

            return cancelResponse;

        }

        public static void CancelOrderRetry(object o)
        {
            int retryCount = 10;

            for (int i = 0; i < retryCount; ++i)
            {

                Thread.Sleep(5000);
                AlipayTradeCancelRequest cancelRequest = new AlipayTradeCancelRequest();
                cancelRequest.BizContent = o.ToString();
                // Dictionary<string, string> paramsDict = (Dictionary<string, string>)cancelRequest.GetParameters();
                AlipayTradeCancelResponse cancelResponse = client.Execute(cancelRequest);

                if (null != cancelResponse)
                {
                    if (cancelResponse.Code == ResultCode.FAIL)
                    {
                        //if (cancelResponse.Body.Contains("\"retry_flag\":\"N\""))		
                        if (cancelResponse.RetryFlag == "N")
                        {
                            break;
                        }
                    }
                    if ((cancelResponse.Code == ResultCode.SUCCESS))
                    {
                        break;
                    }
                }

                if (i == retryCount - 1)
                {
                    // 处理到最后一次，还是未撤销成功，需要在商户数据库中对此单最标记，人工介入处理

                }

            }
        }

        private static AlipayTradeQueryResponse LoopQuery(string biz_content)
        {

            AlipayTradeQueryRequest payRequst = new AlipayTradeQueryRequest();
            payRequst.BizContent = biz_content;

            Dictionary<string, string> paramsDict = (Dictionary<string, string>)payRequst.GetParameters();
            AlipayTradeQueryResponse payResponse = null;

            for (int i = 1; i <= 6; i++)
            {
                Thread.Sleep(5000);

                payResponse = client.Execute(payRequst);
                if (string.Compare(payResponse.Code, ResultCode.SUCCESS, false) == 0)
                {
                    if (payResponse.TradeStatus == "TRADE_FINISHED"
                        || payResponse.TradeStatus == "TRADE_SUCCESS"
                        || payResponse.TradeStatus == "TRADE_CLOSED")
                        return payResponse;
                }

            }

            StringBuilder sb1 = new StringBuilder();
            sb1.Append("{\"out_trade_no\":\"" + payResponse.OutTradeNo + "\"}");
            biz_content = sb1.ToString();
            Cancel(biz_content);

            return payResponse;

        }

        public static string Query(string out_trade_no)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"out_trade_no\":\"" + out_trade_no + "\"}");
            AlipayTradeQueryResponse queryResponse = LoopQuery(sb.ToString());   //用订单号trade_no进行轮询也是可以的。
            string result = string.Empty;
            if (queryResponse != null)
            {
                if (queryResponse.Code == ResultCode.SUCCESS)
                {
                    result = "支付成功";
                }
                else
                {
                    result = queryResponse.SubMsg;
                }
            }
            else
            {
                result = "支付失败";
            }

            return result;
        }

        public static string PreCreatePay(string biz_content)
        {
            AlipayTradePrecreateRequest payRequst = new AlipayTradePrecreateRequest();
            payRequst.BizContent = biz_content;
            Log.Info("支付宝请求参数：" + JsonConvert.SerializeObject(payRequst));
            AlipayTradePrecreateResponse payResponse = client.Execute(payRequst);
            return payResponse.Body;
        }
    }
}
