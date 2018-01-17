using System;
using Newtonsoft.Json;

namespace TeeGonSdk
{
    [Serializable]
    public abstract class TopResponse
    {
        /// <summary>
        /// 错误码
        /// </summary>
        [JsonProperty(PropertyName = "error")]
        public string ErrorMsg { get; set; }

        /// <summary>
        /// 响应原始内容
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// 响应结果是否错误
        /// </summary>
        public bool IsError
        {
            get
            {
                return !string.IsNullOrEmpty(this.ErrorMsg);
            }
        }
    }
}
