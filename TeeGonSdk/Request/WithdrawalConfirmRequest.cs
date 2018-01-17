using System.Collections.Generic;
using TeeGonSdk.Response;
using TeeGonSdk.Util;

namespace TeeGonSdk.Request
{
    /// <summary>
    /// 验证提现
    /// </summary>
    public class WithdrawalConfirmRequest : BaseTopRequest<WithdrawalConfirmResponse>
    {
        /// <summary>
        /// 提现 transaction_no
        /// </summary>
        public string transaction_no { get; set; }

        /// <summary>
        /// 手机验证码
        /// </summary>
        public int verfiy_code { get; set; }

        #region ITopRequest Members

        public override string GetApiName()
        {
            return "withdrawal/confirm";
        }

        public override bool IsPost()
        {
            return true;
        }

        public override IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("transaction_no", this.transaction_no);
            parameters.Add("verfiy_code", this.verfiy_code);
            if (this.otherParams != null)
            {
                parameters.AddAll(this.otherParams);
            }
            return parameters;
        }

        public override void Validate()
        {
            RequestValidator.ValidateRequired("transaction_no", this.transaction_no);
            RequestValidator.ValidateMinValue("amount", verfiy_code, 1000000);
        }

        #endregion
    }
}
