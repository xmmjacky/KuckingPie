using System;
using System.Xml.Serialization;

namespace Aop.Api.Response
{
    /// <summary>
    /// AlipayMobilePublicTemplateMessageGetResponse.
    /// </summary>
    public class AlipayMobilePublicTemplateMessageGetResponse : AopResponse
    {
        /// <summary>
        /// 模板内容
        /// </summary>
        [XmlElement("template")]
        public string Template { get; set; }
    }
}
