using System;
using System.Xml.Serialization;

namespace Aop.Api.Response
{
    /// <summary>
    /// AlipayMarketingCdpAdvertiseQueryResponse.
    /// </summary>
    public class AlipayMarketingCdpAdvertiseQueryResponse : AopResponse
    {
        /// <summary>
        /// 行为地址。用户点击广告后，跳转URL地址
        /// </summary>
        [XmlElement("action_url")]
        public string ActionUrl { get; set; }

        /// <summary>
        /// 广告标识码,钱包上开放给商家的广告位标识码
        /// </summary>
        [XmlElement("ad_code")]
        public string AdCode { get; set; }

        /// <summary>
        /// 广告展示规则。该规则用于商家设置对用户是否展示广告的校验条件，目前支持设置城市规则、商家店铺规则。按业务要求应用对应规则即可。
        /// </summary>
        [XmlElement("ad_rules")]
        public string AdRules { get; set; }

        /// <summary>
        /// 广告内容
        /// </summary>
        [XmlElement("content")]
        public string Content { get; set; }

        /// <summary>
        /// 广告内容类型，目前包括HTML5和图片，分别传入：H5和PIC
        /// </summary>
        [XmlElement("content_type")]
        public string ContentType { get; set; }

        /// <summary>
        /// 高度
        /// </summary>
        [XmlElement("height")]
        public string Height { get; set; }

        /// <summary>
        /// 在线：ONLINE , 下线：OFFLINE
        /// </summary>
        [XmlElement("status")]
        public string Status { get; set; }
    }
}
