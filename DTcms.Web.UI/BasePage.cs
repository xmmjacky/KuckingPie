using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Configuration;
using DTcms.Common;
using Senparc.Weixin.MP.Helpers;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.AdvancedAPIs;

namespace DTcms.Web.UI
{
    public partial class BasePage : System.Web.UI.Page
    {
        protected internal Model.siteconfig config = new BLL.siteconfig().loadConfig(Utils.GetXmlMapPath(DTKeys.FILE_SITE_XML_CONFING), true);
        protected internal Model.userconfig uconfig = new BLL.userconfig().loadConfig(Utils.GetXmlMapPath(DTKeys.FILE_USER_XML_CONFING));
        protected string noncestr = JSSDKHelper.GetNoncestr();
        protected string timestamp = JSSDKHelper.GetTimestamp();
        protected string openid = string.Empty;
        protected string state = string.Empty;
        protected Model.users userModel = null;        

        /// <summary>
        /// 父类的构造函数
        /// </summary>
        public BasePage()
        {
            //是否关闭网站
            if (config.webstatus == 0)
            {
                HttpContext.Current.Response.Redirect(config.webpath + "error.aspx?msg=" + Utils.UrlEncode(config.webclosereason));
                return;
            }            
            ShowPage();
        }

        protected void InitByMp()
        {
            string code = string.Empty;
            try
            {
                //openid = DTRequest.GetQueryString("openid");
                code = DTRequest.GetQueryString("code");
                Common.Log.Info("InitByMp code:" + code);
                state = DTRequest.GetQueryString("state");
                Common.Log.Info("InitByMp state:" + state);
                OAuthAccessTokenResult token = null;
                if (string.IsNullOrEmpty(state) || string.Equals(state, "123"))
                {
                    state = "slave";
                }
                if (string.IsNullOrEmpty(code))
                {
                    openid = DTRequest.GetQueryString("openid");
                }
                else
                {
                    switch(state)
                    {
                        case "master":
                            token = OAuth.GetAccessToken(config.mp_appid, config.mp_appsecret, code);
                            openid = token.openid;
                            break;
                        case "slave":
                            token = OAuth.GetAccessToken(config.mp_slave_appid, config.mp_slave_appsecret, code);
                            openid = token.openid;
                            break;
                    }
                    
                }
                userModel = new BLL.users().GetModel(openid);
                BLL.users bllUsers = new BLL.users();
#if DEBUG

#else
     
                OAuthUserInfo userinfo = OAuth.GetUserInfo(token.access_token, token.openid);
                if (userModel == null)
                {
                    //检查默认组别是否存在
                    
                    Model.user_groups modelGroup = new BLL.user_groups().GetDefault();
                    userModel = new Model.users();
                    userModel.group_id = modelGroup.id;
                    userModel.user_name = userinfo.openid;
                    userModel.nick_name = userinfo.nickname;
                    userModel.sex = userinfo.sex == 1 ? "男" : userinfo.sex == 2 ? "女" : "未知";
                    //userModel.address = userinfo.country + userinfo.province + userinfo.city;
                    //userModel.province = userinfo.province;
                    //userModel.city = userinfo.city;
                    userModel.avatar = userinfo.headimgurl.Replace("/0", "/96");
                    userModel.reg_time = DateTime.Now;
                    userModel.password = DESEncrypt.Encrypt("111111");
                    userModel.id = bllUsers.Add(userModel);
                }
                else
                {
                    bllUsers.UpdateField(userModel.id, "avatar='"+ userinfo.headimgurl.Replace("/0", "/96") + "',nick_name='"+ userinfo.nickname + "'");
                }
#endif
                HttpContext.Current.Response.Cookies.Add(new HttpCookie("openid", openid));
                HttpContext.Current.Session.Add("openid", openid);
            }
            catch (Exception ex)
            {
                string url = Request.Path.Substring(Request.Path.LastIndexOf("/"));
                string[] _params = Request.Url.Query.TrimStart('?').Split('&');
                if (_params.Length > 0)
                {
                    url += "?";
                    for (int i = 0; i < _params.Length; i++)
                    {
                        if (_params[i].IndexOf("code=") > -1 || _params[i].IndexOf("state=") > -1) continue;
                        url += _params[i] + "&";
                    }
                    url = url.TrimEnd('&');
                    url = url.Replace("?", "%3F").Replace("=", "%3D").Replace("&", "%26");
                }
                //url = DTcms.Common.Utils.UrlEncode(url);
                Log.Info("state:"+ state + " ExceptionMessage:"+ex.Message + " code:"+ code + " state:"+ state);
                switch (state)
                {
                    case "master":
                        Log.Info("https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + config.mp_appid + "&redirect_uri=http%3A%2F%2Fwww.4008317417.cn" + url + "&response_type=code&scope=snsapi_base&state=" + state + "#wechat_redirect");
                        Response.Redirect("https://open.weixin.qq.com/connect/oauth2/authorize?appid="+config.mp_appid+"&redirect_uri=http%3A%2F%2Fwww.4008317417.cn" + url + "&response_type=code&scope=snsapi_userinfo&state=" + state + "#wechat_redirect");
                        break;
                    case "slave":
                        Log.Info("https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + config.mp_slave_appid + "&redirect_uri=http%3A%2F%2Fwww.4008317417.cn" + url + "&response_type=code&scope=snsapi_base&state=" + state + "#wechat_redirect");
                        Response.Redirect("https://open.weixin.qq.com/connect/oauth2/authorize?appid="+config.mp_slave_appid+"&redirect_uri=http%3A%2F%2Fwww.4008317417.cn" + url + "&response_type=code&scope=snsapi_userinfo&state=" + state + "#wechat_redirect");
                        break;
                }
                return;
                
                //if (HttpContext.Current.Request.Cookies.Get("openid") != null)
                //{
                //    openid = HttpContext.Current.Request.Cookies.Get("openid").Value;
                //    Log.Info("使用Cookie中Openid:" + openid);
                //    userModel = new BLL.users().GetModel(openid);
                //    if (userModel == null) userModel = new Model.users();
                //}
                //else
                //{
                //    if(HttpContext.Current.Session["openid"]!=null)
                //    {
                //        openid = HttpContext.Current.Session["openid"].ToString();
                //        Log.Info("使用Session中Openid:" + openid);
                //        userModel = new BLL.users().GetModel(openid);
                //        if (userModel == null) userModel = new Model.users();
                //    }
                //    else
                //    {
                //        Log.Info("没有使用OpenId");
                //    }
                    
                //}
            } 
        }

        /// <summary>
        /// 页面处理虚方法
        /// </summary>
        protected virtual void ShowPage()
        {
            //虚方法代码
        }

#region 通用处理方法========================================================
        /// <summary>
        /// 判断用户是否已经登录(解决Session超时问题)
        /// </summary>
        public bool IsUserLogin()
        {
            //如果Session为Null
            if (HttpContext.Current.Session[DTKeys.SESSION_USER_INFO] != null)
            {
                return true;
            }
            else
            {
                //检查Cookies
                string username = Utils.GetCookie(DTKeys.COOKIE_USER_NAME_REMEMBER, "DTcms"); //解密用户名
                string password = Utils.GetCookie(DTKeys.COOKIE_USER_PWD_REMEMBER, "DTcms");
                if (username != "" && password != "")
                {
                    BLL.users bll = new BLL.users();
                    Model.users model = bll.GetModel(username, password, 0);
                    if (model != null)
                    {
                        HttpContext.Current.Session[DTKeys.SESSION_USER_INFO] = model;
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 取得用户信息
        /// </summary>
        public Model.users GetUserInfo()
        {
            if (IsUserLogin())
            {
                Model.users model = HttpContext.Current.Session[DTKeys.SESSION_USER_INFO] as Model.users;
                if (model != null)
                {
                    //为了能查询到最新的用户信息，必须查询最新的用户资料
                    model = new BLL.users().GetModel(model.id);
                    return model;
                }
            }
            return null;
        }
#endregion

#region 页面基本处理方法=====================================================
        /// <summary>
        /// 返回URL重写统一链接地址
        /// </summary>
        public string linkurl(string _key)
        {
            return linkurl(_key, "");
        }

        /// <summary>
        /// 返回URL重写统一链接地址
        /// </summary>
        public string linkurl(string _key, params object[] _params)
        {
            Hashtable ht = new BLL.url_rewrite().GetList();
            Model.url_rewrite model = ht[_key] as Model.url_rewrite;
            if (model == null)
            {
                return "";
            }
            try
            {
                string _result = string.Empty;
                string _rewriteurl = string.Format(model.path, _params);
                switch (config.staticstatus)
                {
                    case 1: //URL重写
                        _result = config.webpath + _rewriteurl;
                        break;
                    case 2: //全静态
                        _rewriteurl = _rewriteurl.Substring(0, _rewriteurl.LastIndexOf(".") + 1);
                        _result = config.webpath + DTKeys.DIRECTORY_REWRITE_HTML + "/" + _rewriteurl + config.staticextension;
                        break;
                    default: //不开启
                        string _originalurl = model.page;
                        if (!string.IsNullOrEmpty(model.querystring))
                        {
                            _originalurl = model.page + "?" + Regex.Replace(_rewriteurl, model.pattern, model.querystring, RegexOptions.None | RegexOptions.IgnoreCase);
                        }
                        _result = config.webpath + _originalurl;
                        break;
                }
                return _result;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 返回分页字符串
        /// </summary>
        /// <param name="pagesize">页面大小</param>
        /// <param name="pageindex">当前页</param>
        /// <param name="totalcount">记录总数</param>
        /// <param name="_key">URL映射Name名称</param>
        /// <param name="_params">传输参数</param>
        protected string get_page_link(int pagesize, int pageindex, int totalcount, string _key, params object[] _params)
        {
            return Utils.OutPageList(pagesize, pageindex, totalcount, linkurl(_key, _params), 8);
        }

        /// <summary>
        /// 返回分页字符串
        /// </summary>
        /// <param name="pagesize">页面大小</param>
        /// <param name="pageindex">当前页</param>
        /// <param name="totalcount">记录总数</param>
        /// <param name="linkurl">链接地址</param>
        protected string get_page_link(int pagesize, int pageindex, int totalcount, string linkurl)
        {
            return Utils.OutPageList(pagesize, pageindex, totalcount, linkurl, 8);
        }

        protected string get_mp_signature()
        {
            string jsapi_token = JsApiTicketContainer.TryGetTicket(config.mp_slave_appid, config.mp_slave_appsecret);
            JSSDKHelper helper = new JSSDKHelper();
            return helper.GetSignature(jsapi_token, noncestr, timestamp, Context.Request.Url.ToString().Split('#')[0].Replace("/aspx", ""));
        }
#endregion

    }
}
