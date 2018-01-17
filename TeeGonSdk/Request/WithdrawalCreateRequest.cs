using System.Collections.Generic;
using TeeGonSdk.Response;
using TeeGonSdk.Util;

namespace TeeGonSdk.Request
{
    /// <summary>
    /// 创建提现申请
    /// </summary>
    public class WithdrawalCreateRequest : BaseTopRequest<WithdrawalCreateResponse>
    {
        /// <summary>
        /// 子账户
        /// </summary>
        public string account_id { get; set; }

        /// <summary>
        /// 提现金额
        /// </summary>
        public decimal amount { get; set; }

        #region ITopRequest Members

        public override string GetApiName()
        {
            return "withdrawal/create";
        }

        public override bool IsPost()
        {
            return true;
        }

        public override IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("account_id", this.account_id);
            parameters.Add("amount", this.amount);
            if (this.otherParams != null)
            {
                parameters.AddAll(this.otherParams);
            }
            return parameters;
        }

        public override void Validate()
        {
            RequestValidator.ValidateRequired("account_id", this.account_id);
            RequestValidator.ValidateDecimalMinValue("amount", amount, 0.01M);
        }

        #endregion
    }
}
