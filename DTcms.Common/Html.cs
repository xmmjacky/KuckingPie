using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using log4net;
using System.Net;
using System.IO.Compression;
using System.IO;

namespace DTcms.Common
{
    public class Html
    {
        public const string PC_USER_AGENT = "Mozilla/5.0 (Windows NT 6.2; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/32.0.1667.0 Safari/537.36";

        public static string GetHtmlWithSimpleCookie(string url, string referer, string cookies)
        {
            HttpWebRequest wReq = (HttpWebRequest)WebRequest.Create(url);
            wReq.Accept = "application/json, text/javascript, */*; q=0.01";
            wReq.Method = "GET";

            wReq.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, sdch");
            wReq.Headers.Add(HttpRequestHeader.AcceptLanguage, "zh-CN,zh;q=0.8");
            wReq.UserAgent = PC_USER_AGENT;
            if (!string.IsNullOrEmpty(cookies))
            {
                wReq.Headers.Add(HttpRequestHeader.Cookie, cookies);
            }
            if (!string.IsNullOrEmpty(referer))
            {
                wReq.Referer = referer;
            }
            // Get the response instance.
            string html = string.Empty;
            html = GetHtml(wReq);
            return html;
        }

        private static string GetHtml(HttpWebRequest wReq)
        {
            return GetHtml(wReq, Encoding.GetEncoding("UTF-8"));
        }

        private static string GetHtml(HttpWebRequest wReq, Encoding encoding)
        {

            string html = string.Empty;
            HttpWebResponse wResp = null;
            Stream respStream = null;
            try
            {
                wResp = (HttpWebResponse)wReq.GetResponse();
                respStream = wResp.GetResponseStream();
                if (wResp.ContentEncoding.ToLower().Contains("gzip"))
                    respStream = new GZipStream(respStream, CompressionMode.Decompress);
                else if (wResp.ContentEncoding.ToLower().Contains("deflate"))
                    respStream = new DeflateStream(respStream, CompressionMode.Decompress);

                using (System.IO.StreamReader reader = new System.IO.StreamReader(respStream, encoding))
                {
                    html = reader.ReadToEnd();
                }
                wResp.Close();
                respStream.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (wResp != null) wResp.Close();
                if (respStream != null) respStream.Close();
            }
            return html;
        }


    }
}
