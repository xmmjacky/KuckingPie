using System;
using System.Xml.Serialization;

namespace Aop.Api.Domain
{
    /// <summary>
    /// AlipayOfflineMarketProductBatchqueryModel Data Structure.
    /// </summary>
    [Serializable]
    public class AlipayOfflineMarketProductBatchqueryModel : AopObject
    {
        /// <summary>
        /// 页码，留空标示第一页，默认300个结果为一页
        /// </summary>
        [XmlElement("page_no")]
        public string PageNo { get; set; }
    }
}
