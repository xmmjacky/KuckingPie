using System.Collections.Generic;
using TeeGonSdk.Response;
using TeeGonSdk.Util;

namespace TeeGonSdk.Request
{
    /// <summary>
    /// 创建订单
    /// </summary>
    public class ChargePayStatusRequest<T> : BaseTopRequest<ChargePayStatusResponse<T>>
    {

        public string charge_id { get; set; }

        #region ITopRequest Members

        public override string GetApiName()
        {
            return "charge/barcode_pay_status";
        }

        public override bool IsPost()
        {
            return true;
        }

        public override IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("charge_id", this.charge_id);
            if (this.otherParams != null)
            {
                parameters.AddAll(this.otherParams);
            }
            return parameters;
        }

        public override void Validate()
        {
            RequestValidator.ValidateRequired("charge_id", this.charge_id);
        }

        #endregion
    }
}
