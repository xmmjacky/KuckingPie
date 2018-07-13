using System.Collections.Generic;
using TeeGonSdk.Response;
using TeeGonSdk.Util;

namespace TeeGonSdk.Request
{
    /// <summary>
    /// 创建订单
    /// </summary>
    public class ChargeRequest<T> : BaseTopRequest<ChargeResponse<T>>
    {

        /// <summary>
        /// 支付金额
        /// </summary>
        public decimal amount { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string out_order_no { get; set; }

        /// <summary>
        /// 支付渠道 (支付宝手机:alipay_wap,支付宝pc:alipay,微信二维码:wxpay,微信jsapi支付:wxpay_jsapi)
        /// </summary>
        public string pay_channel { get; set; }
        public string channel { get; set; }

        public string barcodepay { get; set; }
        /// <summary>
        /// 支付ip
        /// </summary>
        public string ip { get; set; }

        /// <summary>
        /// 订单名字
        /// </summary>
        public string subject { get; set; }

        /// <summary>
        /// 页面跳转同步通知页面路径
        /// </summary>
        public string return_url { get; set; }

        /// <summary>
        /// 服务器异步通知页面路径
        /// </summary>
        public string notify_url { get; set; }

        /// <summary>
        /// 币种(默认RMB)
        /// </summary>
        public string currency { get; set; }

        /// <summary>
        /// 额外数据(存json格式 会在支付成功后回调接口传回)
        /// </summary>
        public string metadata { get; set; }

        /// <summary>
        /// 过期时间 单位秒 默认600
        /// </summary>
        public int time_expire { get; set; }

        /// <summary>
        /// 分账模式(自动分账:auto(默认),手动分账:manual)
        /// </summary>
        public string profit_sharing_mode { get; set; }

        /// <summary>
        /// 微信openid 在微信支付的时候,如果平台要用自己的openid请传下，如果不传默认是和天工对呀的appid
        /// </summary>
        public string wx_openid { get; set; }

        /// <summary>
        /// 设备id
        /// </summary>
        public string device_id { get; set; }

        /// <summary>
        /// 支付类型(支付单:pay,充值:recharge)
        /// </summary>
        public string charge_type { get; set; }

        /// <summary>
        /// 子账户id
        /// </summary>
        public string account_id { get; set; }

        public string auth_code { get; set; }

        #region ITopRequest Members

        public override string GetApiName()
        {
            return "v1/charge";
        }

        public override bool IsPost()
        {
            return true;
        }

        public override IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("amount", this.amount);
            parameters.Add("out_order_no", this.out_order_no);
            parameters.Add("pay_channel", this.pay_channel);
            //parameters.Add("channel", this.channel);
            parameters.Add("barcodepay", this.barcodepay);
            parameters.Add("ip", this.ip);
            parameters.Add("subject", this.subject);
            if (!string.IsNullOrWhiteSpace(return_url))
                parameters.Add("return_url", this.return_url);
            if (!string.IsNullOrWhiteSpace(notify_url))
                parameters.Add("notify_url", this.notify_url);
            if (!string.IsNullOrWhiteSpace(currency))
                parameters.Add("currency", this.currency);
            if (!string.IsNullOrWhiteSpace(metadata))
                parameters.Add("metadata", this.metadata);
            if (time_expire != 0)
                parameters.Add("time_expire", this.time_expire);
            if (!string.IsNullOrWhiteSpace(profit_sharing_mode))
                parameters.Add("profit-sharing-mode", this.profit_sharing_mode);
            if (!string.IsNullOrWhiteSpace(wx_openid))
                parameters.Add("wx_openid", this.wx_openid);
            if (!string.IsNullOrWhiteSpace(this.auth_code))
                parameters.Add("auth_code", this.auth_code);
            parameters.Add("device_id", this.device_id);
            if (!string.IsNullOrWhiteSpace(this.charge_type))
                parameters.Add("charge_type", this.charge_type);
            if (!string.IsNullOrWhiteSpace(this.account_id))
                parameters.Add("account_id", this.account_id);
            if (this.otherParams != null)
            {
                parameters.AddAll(this.otherParams);
            }
            return parameters;
        }

        public override void Validate()
        {
            RequestValidator.ValidateDecimalMinValue("amount", this.amount, 0.01M);
            RequestValidator.ValidateRequired("out_order_no", this.out_order_no);
            RequestValidator.ValidateMaxLength("out_order_no", this.out_order_no, 16);
            //RequestValidator.ValidateRequired("pay_channel", this.pay_channel);
            RequestValidator.ValidateRequired("ip", this.ip);
            RequestValidator.ValidateRequired("subject", this.subject);
            //RequestValidator.ValidateRequired("return_url", this.return_url);
            //RequestValidator.ValidateRequired("notify_url", this.notify_url);
            RequestValidator.ValidateRequired("device_id", this.device_id);
            //RequestValidator.ValidateRequired("charge_type", this.charge_type);
            //RequestValidator.ValidateRequired("account_id", this.account_id);
        }

        #endregion
    }
}
