using System.Collections.Generic;
using TeeGonSdk.Response;
using TeeGonSdk.Util;

namespace TeeGonSdk.Request
{
    /// <summary>
    /// 获取银行卡绑定的url
    /// </summary>
    public class BankUrlBindRequest : BaseTopRequest<BankUrlBindResponse>
    {
        /// <summary>
        /// 子账户id
        /// </summary>
        public string account_id { get; set; }

        /// <summary>
        /// 回调地址 绑定成功后把绑定状态发给平台
        /// </summary>
        public string notify_url { get; set; }

        /// <summary>
        /// 绑卡类型 小额鉴权:b,银联鉴权:u 默认u
        /// </summary>
        public string bind_type { get; set; }

        #region ITopRequest Members

        public override string GetApiName()
        {
            return "bank/url/bind";
        }

        public override bool IsPost()
        {
            return true;
        }

        public override IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("account_id", this.account_id);
            parameters.Add("notify_url", this.notify_url);
            if (!string.IsNullOrWhiteSpace(this.bind_type))
                parameters.Add("bind_type", this.bind_type);
            if (this.otherParams != null)
            {
                parameters.AddAll(this.otherParams);
            }
            return parameters;
        }

        public override void Validate()
        {
            RequestValidator.ValidateRequired("account_id", this.account_id);
            RequestValidator.ValidateRequired("notify_url", this.notify_url);
        }

        #endregion
    }
}
