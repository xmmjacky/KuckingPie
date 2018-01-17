using System;
using System.Xml.Serialization;

namespace Aop.Api.Domain
{
    /// <summary>
    /// MarketingCdpAdvertiseQueryModel Data Structure.
    /// </summary>
    [Serializable]
    public class MarketingCdpAdvertiseQueryModel : AopObject
    {
        /// <summary>
        /// 广告Id
        /// </summary>
        [XmlElement("ad_id")]
        public string AdId { get; set; }
    }
}
