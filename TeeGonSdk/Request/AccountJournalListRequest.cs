using System.Collections.Generic;
using TeeGonSdk.Response;

namespace TeeGonSdk.Request
{
    /// <summary>
    /// 流水记录
    /// </summary>
    public class AccountJournalListRequest : BaseTopRequest<AccountJournalListResponse>
    {

        /// <summary>
        /// 流水类型(收入:income,支出:outcome,)
        /// </summary>
        public string journal_type { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string out_order_no { get; set; }

        /// <summary>
        /// 子账户id
        /// </summary>
        public string account_id { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public long from_time { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public long to_time { get; set; }

        /// <summary>
        /// "offset"
        /// </summary>
        public int offset { get; set; }

        /// <summary>
        /// "limit"
        /// </summary>
        public int limit { get; set; }

        #region ITopRequest Members

        public override string GetApiName()
        {
            return "account/journal/list";
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

        }

        #endregion
    }
}
