using System.Collections.Generic;
using TeeGonSdk.Response;
using TeeGonSdk.Util;

namespace TeeGonSdk.Request
{
    /// <summary>
    /// 获取账户详情
    /// </summary>
    public class AccountGetRequest : BaseTopRequest<AccountGetResponse>
    {
        /// <summary>
        /// 子账户id
        /// </summary>
        public string account_id { get; set; }

        #region ITopRequest Members

        public override string GetApiName()
        {
            return "account/get";
        }

        public override bool IsPost()
        {
            return false;
        }

        public override IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("account_id", this.account_id);
            if (this.otherParams != null)
            {
                parameters.AddAll(this.otherParams);
            }
            return parameters;
        }

        public override void Validate()
        {
            RequestValidator.ValidateRequired("account_id", this.account_id);
        }

        #endregion
    }
}
