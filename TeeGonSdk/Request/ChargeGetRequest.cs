using System.Collections.Generic;
using TeeGonSdk.Response;

namespace TeeGonSdk.Request
{
    /// <summary>
    /// 获取订单
    /// </summary>
    public class ChargeGetRequest : BaseTopRequest<ChargeGetResponse>
    {
        /// <summary>
        /// 支付单id
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// 子账户id
        /// </summary>
        public string account_id { get; set; }

        #region ITopRequest Members

        public override string GetApiName()
        {
            return "v1/charge/get";
        }

        public override bool IsPost()
        {
            return false;
        }

        public override IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            if (!string.IsNullOrWhiteSpace(id))
                parameters.Add("id", this.id);
            if (!string.IsNullOrWhiteSpace(account_id))
                parameters.Add("account_id", this.account_id);
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
