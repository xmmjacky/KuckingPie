using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TeeGonSdk.Util;

namespace TeeGonSdk
{
    /// <summary>
    /// 基于REST的TOP客户端。
    /// </summary>
    public class DefaultTopClient : ITopClient
    {
        internal string serverUrl;
        internal string clientId;
        internal string clientSecret;

        internal WebUtils webUtils;
        internal bool disableParser = false; // 禁用响应结果解释
        internal bool useGzipEncoding = true;  // 是否启用响应GZIP压缩
        internal IDictionary<string, string> systemParameters; // 设置所有请求共享的系统级参数

        #region DefaultTopClient Constructors

        public DefaultTopClient(string serverUrl, string clientId, string clientSecret)
        {
            this.clientId = clientId;
            this.clientSecret = clientSecret;
            this.serverUrl = serverUrl;
            this.webUtils = new WebUtils();
        }

        #endregion

        public void SetTimeout(int timeout)
        {
            this.webUtils.Timeout = timeout;
        }

        public void SetReadWriteTimeout(int readWriteTimeout)
        {
            this.webUtils.ReadWriteTimeout = readWriteTimeout;
        }

        public void SetDisableParser(bool disableParser)
        {
            this.disableParser = disableParser;
        }

        public void SetSystemParameters(IDictionary<string, string> systemParameters)
        {
            this.systemParameters = systemParameters;
        }

        #region ITopClient Members

        public virtual T Execute<T>(ITopRequest<T> request) where T : TopResponse
        {
            return Execute<T>(request, null);
        }

        public virtual T Execute<T>(ITopRequest<T> request, string session) where T : TopResponse
        {
            return DoExecute<T>(request, session, DateTime.Now);
        }

        public virtual T Execute<T>(ITopRequest<T> request, string session, DateTime timestamp) where T : TopResponse
        {
            return DoExecute<T>(request, session, timestamp);
        }

        #endregion

        private T DoExecute<T>(ITopRequest<T> request, string session, DateTime timestamp) where T : TopResponse
        {
            // 提前检查业务参数
            //DTcms.Common.Log.Info("downloadlist:areaids_" +);
            try
            {
                request.Validate();
            }
            catch (TopException e)
            {
                DTcms.Common.Log.Info("天工支付异常"+e.ToString());
                return CreateErrorResponse<T>(e.Error);
            }

            // 添加协议级请求参数
            TopDictionary txtParams = new TopDictionary(request.GetParameters());
            txtParams.Add(Constants.CLIENT_ID, clientId);
            txtParams.Add(Constants.CLIENT_SECRET, clientSecret);
            txtParams.AddAll(this.systemParameters);

            // 添加头部参数
            if (this.useGzipEncoding)
            {
                request.GetHeaderParameters()[Constants.ACCEPT_ENCODING] = Constants.CONTENT_ENCODING_GZIP;
            }

            string realServerUrl = GetServerUrl(this.serverUrl, request.GetApiName(), session);
            //string reqUrl = WebUtils.BuildRequestUrl(realServerUrl, txtParams);
            DTcms.Common.Log.Info("获取天工url" + serverUrl);
            DTcms.Common.Log.Info("获取天工接口名称" + request.GetApiName());
            try
            {
                string body = "";
                if (request.IsPost())
                {
                    txtParams.Add("sign", TopUtils.SignTopRequest(txtParams, txtParams["client_secret"], "md5"));
                    foreach (var item in txtParams) {
                        DTcms.Common.Log.Info(string.Format("获取天工接口参数Key---->{0},Value---->{1}",item.Key,item.Value));
                    }
                    body = webUtils.DoPost(realServerUrl, txtParams, request.GetHeaderParameters());
                    DTcms.Common.Log.Info("获取天工接口返回参数"+body);
                }
                else
                {
                    body = webUtils.DoGet(realServerUrl, txtParams, request.GetHeaderParameters());
                }
                body = body.Replace("\"result\": \"error\"", "\"result\": null").Replace("\"result\":\"error\"", "\"result\":null");
                // 解释响应结果
                T rsp;
                if (disableParser)
                {
                    rsp = Activator.CreateInstance<T>();
                }
                else
                {
                    rsp = JsonConvert.DeserializeObject<T>(body);
                }
                rsp.Body = body;
                return rsp;
            }
            catch (Exception e)
            {
                DTcms.Common.Log.Info("天工返回结果异常"+e.ToString());
                throw e;
            }
        }

        internal virtual string GetServerUrl(string serverUrl, string apiName, string session)
        {
            return serverUrl + apiName;
        }

        internal virtual string GetSdkVersion()
        {
            return Constants.SDK_VERSION;
        }

        internal T CreateErrorResponse<T>(string errMsg) where T : TopResponse
        {
            T rsp = Activator.CreateInstance<T>();
            rsp.ErrorMsg = errMsg;
            IDictionary<string, object> errObj = new Dictionary<string, object>();
            errObj.Add(Constants.ERRORMSG, errMsg);
            string body = JsonConvert.SerializeObject(errObj);
            rsp.Body = body;
            return rsp;
        }
    }
}
