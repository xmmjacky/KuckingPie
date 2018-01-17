<%@ Page Language="C#" AutoEventWireup="true" Inherits="DTcms.Web.UI.Page.mp_show_company" ValidateRequest="false" %>
<%@ Import namespace="System.Collections.Generic" %>
<%@ Import namespace="System.Text" %>
<%@ Import namespace="System.Data" %>
<%@ Import namespace="DTcms.Common" %>

<script runat="server">
override protected void OnInit(EventArgs e)
{

	/* 
		This page was created by DTcms Template Engine at 2017/8/27 2:49:51.
		本页面代码由DTcms模板引擎生成于 2017/8/27 2:49:51. 
	*/

	base.OnInit(e);
	StringBuilder templateBuilder = new StringBuilder(220000);

	templateBuilder.Append("<!DOCTYPE html>\r\n<html>\r\n<head>\r\n    <meta charset=\"utf-8\" />\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0;\">\r\n    <title>");
	templateBuilder.Append(Utils.ObjectToStr(config.webname));
	templateBuilder.Append("</title>\r\n    <meta content=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.webkeyword));
	templateBuilder.Append("\" name=\"keywords\">\r\n    <meta content=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.webdescription));
	templateBuilder.Append("\" name=\"description\">\r\n    <script src=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.templateskin));
	templateBuilder.Append("/js/jquery-1.10.2.min.js\" type=\"text/javascript\"></");
	templateBuilder.Append("script>\r\n    <script src=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.templateskin));
	templateBuilder.Append("/js/jquery-ui.min.js\" type=\"text/javascript\"></");
	templateBuilder.Append("script>\r\n    <link media=\"screen\" type=\"text/css\" href=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.templateskin));
	templateBuilder.Append("/css/jquery-ui.min.css\" rel=\"stylesheet\">\r\n    <script charset=\"utf-8\" src=\"http://map.qq.com/api/js?v=2.exp&key=BOEBZ-2AB2R-IKTWG-W2JQG-HEUOV-2RF7Z\"></");
	templateBuilder.Append("script>\r\n    <script type=\"text/javascript\" src=\"http://res.wx.qq.com/open/js/jweixin-1.0.0.js\"></");
	templateBuilder.Append("script>\r\n    <script type=\"text/javascript\">\r\n        wx.config({\r\n            debug: false, // 开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印。\r\n            appId: '");
	templateBuilder.Append(Utils.ObjectToStr(config.mp_slave_appid));
	templateBuilder.Append("', // 必填，公众号的唯一标识\r\n            timestamp: ");
	templateBuilder.Append(Utils.ObjectToStr(jstimestamp));
	templateBuilder.Append(", // 必填，生成签名的时间戳\r\n                    nonceStr: '");
	templateBuilder.Append(Utils.ObjectToStr(jsnoncestr));
	templateBuilder.Append("', // 必填，生成签名的随机串\r\n                    signature: '");
	templateBuilder.Append(Utils.ObjectToStr(mp_signature));
	templateBuilder.Append("',// 必填，签名，见附录1\r\n            jsApiList: ['onMenuShareTimeline','onMenuShareAppMessage','onMenuShareQQ','onMenuShareWeibo','onMenuShareQZone'] // 必填，需要使用的JS接口列表，所有JS接口列表见附录2\r\n        });\r\n        wx.ready(function(){\r\n            ");
	if (modelCompay!=null)
	{

	templateBuilder.Append("\r\n            var wxShareTitle = '1分钱吃馍,企业专享';\r\n            var wxShareDesc = '招齐50位同事,1分钱吃馍乐翻天';\r\n            var wxShareImage = 'http://4008317417.cn/company_qr/");
	templateBuilder.Append(Utils.ObjectToStr(modelCompay.Id));
	templateBuilder.Append(".jpg';\r\n            var wxShareUrl = 'http://4008317417.cn/mp_show_company.aspx?companyid=");
	templateBuilder.Append(Utils.ObjectToStr(modelCompay.Id));
	templateBuilder.Append("';\r\n            wx.onMenuShareTimeline({\r\n                title: wxShareTitle, // 分享标题\r\n                link: wxShareUrl, // 分享链接\r\n                imgUrl: wxShareImage, // 分享图标\r\n                success: function () {\r\n                    // 用户确认分享后执行的回调函数\r\n                },\r\n                cancel: function () {\r\n                    // 用户取消分享后执行的回调函数\r\n                }\r\n            });\r\n            wx.onMenuShareAppMessage({\r\n                title: wxShareTitle, // 分享标题\r\n                desc: wxShareDesc, // 分享描述\r\n                link: wxShareUrl, // 分享链接\r\n                imgUrl: wxShareImage, // 分享图标\r\n                type: 'link', // 分享类型,music、video或link，不填默认为link\r\n                dataUrl: '', // 如果type是music或video，则要提供数据链接，默认为空\r\n                success: function () {\r\n                    // 用户确认分享后执行的回调函数\r\n                },\r\n                cancel: function () {\r\n                    // 用户取消分享后执行的回调函数\r\n                }\r\n            });\r\n            wx.onMenuShareQQ({\r\n                title: wxShareTitle, // 分享标题\r\n                desc: wxShareDesc, // 分享描述\r\n                link: wxShareUrl, // 分享链接\r\n                imgUrl: wxShareImage, // 分享图标\r\n                success: function () {\r\n                    // 用户确认分享后执行的回调函数\r\n                },\r\n                cancel: function () {\r\n                    // 用户取消分享后执行的回调函数\r\n                }\r\n            });\r\n            wx.onMenuShareWeibo({\r\n                title: wxShareTitle, // 分享标题\r\n                desc: wxShareDesc, // 分享描述\r\n                link: wxShareUrl, // 分享链接\r\n                imgUrl: wxShareImage, // 分享图标\r\n                success: function () {\r\n                    // 用户确认分享后执行的回调函数\r\n                },\r\n                cancel: function () {\r\n                    // 用户取消分享后执行的回调函数\r\n                }\r\n            });\r\n            wx.onMenuShareQZone({\r\n                title: wxShareTitle, // 分享标题\r\n                desc: wxShareDesc, // 分享描述\r\n                link: wxShareUrl, // 分享链接\r\n                imgUrl: wxShareImage, // 分享图标\r\n                success: function () {\r\n                    // 用户确认分享后执行的回调函数\r\n                },\r\n                cancel: function () {\r\n                    // 用户取消分享后执行的回调函数\r\n                }\r\n            });\r\n            ");
	}	//end if


	templateBuilder.Append("\r\n        });\r\n        wx.error(function(res){\r\n            // config信息验证失败会执行error函数，如签名过期导致验证失败，具体错误信息可以打开config的debug模式查看，\r\n            //也可以在返回的res参数中查看，对于SPA可以在这里更新签名。\r\n\r\n        });\r\n\r\n        \r\n    </");
	templateBuilder.Append("script>\r\n</head>\r\n<body>\r\n    ");
	if (modelCompay!=null)
	{

	templateBuilder.Append("\r\n        <img src=\"/company_qr/");
	templateBuilder.Append(Utils.ObjectToStr(modelCompay.Id));
	templateBuilder.Append(".jpg\" style=\"width:100%;\" />\r\n    ");
	}	//end if


	templateBuilder.Append("\r\n</body>\r\n</html>\r\n");
	Response.Write(templateBuilder.ToString());
}
</script>
