using System;
using System.Xml.Serialization;

namespace Aop.Api.Domain
{
    /// <summary>
    /// AlipayOfflineMarketShopQuerydetailModel Data Structure.
    /// </summary>
    [Serializable]
    public class AlipayOfflineMarketShopQuerydetailModel : AopObject
    {
        /// <summary>
        /// 店铺id
        /// </summary>
        [XmlElement("shop_id")]
        public string ShopId { get; set; }
    }
}
