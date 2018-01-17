using System.Collections.Generic;
using TeeGonSdk.Response;

namespace TeeGonSdk.Request
{
    /// <summary>
    /// 查找订单
    /// </summary>
    public class ChargeListRequest : BaseTopRequest<ChargeListResponse>
    {
        /// <summary>
        /// 字段 默认(order_no, channel, amount, subject, paid, time_paid, created, updated)
        /// </summary>
        public string cols { get; set; }

        /// <summary>
        /// 状态(已支付:paid,未支付:ready,过期支付单:dead)
        /// </summary>
        public string status { get; set; }

        /// <summary>
        /// 支付渠道(支付宝:alipay,微信:wxpay)
        /// </summary>
        public string pay_channel { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public long from_time { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public long to_time { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string out_order_no { get; set; }

        /// <summary>
        /// 子账户id
        /// </summary>
        public string account_id { get; set; }

        /// <summary>
        /// offset
        /// </summary>
        public int offset { get; set; }

        /// <summary>
        /// limit
        /// </summary>
        public int limit { get; set; }

        #region ITopRequest Members

        public override string GetApiName()
        {
            return "v1/charge/list";
        }

        public override bool IsPost()
        {
            return false;
        }

        public override IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            if (!string.IsNullOrWhiteSpace(cols))
                parameters.Add("cols", this.cols);
            if (!string.IsNullOrWhiteSpace(status))
                parameters.Add("status", this.status);
            if (!string.IsNullOrWhiteSpace(pay_channel))
                parameters.Add("pay_channel", this.pay_channel);
            if (from_time > 0)
                parameters.Add("from_time", this.from_time);
            if (to_time > 0)
                parameters.Add("to_time", this.to_time);
            if (!string.IsNullOrWhiteSpace(out_order_no))
                parameters.Add("out_order_no", this.out_order_no);
            if (!string.IsNullOrWhiteSpace(account_id))
                parameters.Add("account_id", this.account_id);
            if (offset > 0)
                parameters.Add("offset", this.offset);
            if (limit > 0)
                parameters.Add("limit", this.limit);
            if (this.otherParams != null)
            {
                parameters.AddAll(this.otherParams);
            }
            return parameters;
        }

        public override void Validate()
        {
        }

        #endregion
    }
}
