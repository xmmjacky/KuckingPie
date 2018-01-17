using System;
using System.Xml.Serialization;

namespace Aop.Api.Domain
{
    /// <summary>
    /// MarketingCdpAdvertiseOperateModel Data Structure.
    /// </summary>
    [Serializable]
    public class MarketingCdpAdvertiseOperateModel : AopObject
    {
        /// <summary>
        /// 广告ID
        /// </summary>
        [XmlElement("ad_id")]
        public string AdId { get; set; }

        /// <summary>
        /// 操作类型，上线: online  和 下线: offline
        /// </summary>
        [XmlElement("operate_type")]
        public string OperateType { get; set; }
    }
}
