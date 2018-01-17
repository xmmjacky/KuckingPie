<%@ Page Language="C#" AutoEventWireup="true" Inherits="DTcms.Web.UI.Page.mp_additional" ValidateRequest="false" %>
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

	templateBuilder.Append("<!DOCTYPE html>\r\n<html>\r\n<head>\r\n    <meta charset=\"utf-8\" />\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0;\">\r\n    <title></title>\r\n    <script src=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.templateskin));
	templateBuilder.Append("/js/zepto.min.1.1.6.js\" type=\"text/javascript\"></");
	templateBuilder.Append("script>    \r\n    <style>\r\n        html, body {\r\n            height: 100%;\r\n            width: 100%;\r\n            margin: 0;\r\n            padding: 0;\r\n            font-family: \"微软雅黑\",STXiHei;\r\n        }\r\n        .typechoice {\r\n            width: 100%;\r\n            height: 100%;\r\n            overflow: hidden;            \r\n        }\r\n\r\n        .txt {\r\n            width: 96%;\r\n            border: 1px solid #ea5414;\r\n            border-radius: 10px;\r\n            font-size: 16px;\r\n            line-height: 20px;\r\n            padding: 2%;\r\n            display: block;\r\n            background: white;\r\n        }\r\n            .txt .big {\r\n                font-size: 30px;\r\n                line-height: 45px;\r\n                display: block;\r\n            }\r\n        .main {\r\n            width: 80%;\r\n            text-align: center;\r\n            margin: 0px auto;\r\n            margin-top: 20px;\r\n        }\r\n        .btn {\r\n            color: white;\r\n            background: #ea5414;\r\n            border: 0px;\r\n            height: 30px;\r\n            width: 100px;\r\n            font-size: 20px;\r\n            border-radius: 10px;\r\n            line-height: 30px;\r\n        }\r\n    </style> \r\n    ");

	templateBuilder.Append("<script type=\"text/javascript\" src=\"http://res.wx.qq.com/open/js/jweixin-1.0.0.js\"></");
	templateBuilder.Append("script>\r\n<script type=\"text/javascript\">\r\n    wx.config({\r\n        debug: false, // 开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印。\r\n        appId: '");
	templateBuilder.Append(Utils.ObjectToStr(config.mp_slave_appid));
	templateBuilder.Append("', // 必填，公众号的唯一标识\r\n        timestamp: ");
	templateBuilder.Append(Utils.ObjectToStr(timestamp));
	templateBuilder.Append(", // 必填，生成签名的时间戳\r\n        nonceStr: '");
	templateBuilder.Append(Utils.ObjectToStr(noncestr));
	templateBuilder.Append("', // 必填，生成签名的随机串\r\n        signature: '");
	templateBuilder.Append(get_mp_signature().ToString());
	

	templateBuilder.Append("',// 必填，签名，见附录1\r\n        jsApiList: ['onMenuShareTimeline','onMenuShareAppMessage','onMenuShareQQ','onMenuShareWeibo','closeWindow'] // 必填，需要使用的JS接口列表，所有JS接口列表见附录2\r\n    });    \r\n    wx.error(function(res){\r\n        // config信息验证失败会执行error函数，如签名过期导致验证失败，具体错误信息可以打开config的debug模式查看，\r\n        //也可以在返回的res参数中查看，对于SPA可以在这里更新签名。\r\n\r\n    });\r\n</");
	templateBuilder.Append("script>");


	templateBuilder.Append("       \r\n    <script type=\"text/javascript\">\r\n        $(function () {            \r\n            $('div').css(\"height\", $(window).height());\r\n        });\r\n    </");
	templateBuilder.Append("script>\r\n</head>\r\n<body class=\"typechoice\">\r\n    ");
	if (msg==1)
	{

	templateBuilder.Append("\r\n    <div class=\"main\">\r\n        <span class=\"txt\">");
	templateBuilder.Append(Utils.ObjectToStr(msgbox));
	templateBuilder.Append("</span>\r\n    </div>\r\n    ");
	}
	else
	{

	templateBuilder.Append("\r\n    <div class=\"main\">\r\n        <span class=\"txt\">抱歉，已超出");
	templateBuilder.Append(Utils.ObjectToStr(config.additional));
	templateBuilder.Append("分钟补单时间\r\n            <span class=\"big\">需再满");
	templateBuilder.Append(Utils.ObjectToStr(config.lowamount));
	templateBuilder.Append("元起送</span>\r\n        </span>        \r\n    </div>\r\n    ");
	}	//end if



	templateBuilder.Append("<div style=\"display:none;\">\r\n    <script type=\"text/javascript\">var cnzz_protocol = ((\"https:\" == document.location.protocol) ? \" https://\" : \" http://\");document.write(unescape(\"%3Cspan id='cnzz_stat_icon_1256906522'%3E%3C/span%3E%3Cscript src='\" + cnzz_protocol + \"s11.cnzz.com/z_stat.php%3Fid%3D1256906522%26show%3Dpic' type='text/javascript'%3E%3C/script%3E\"));</");
	templateBuilder.Append("script>\r\n</div>");


	templateBuilder.Append("\r\n</body>\r\n</html>\r\n");
	Response.Write(templateBuilder.ToString());
}
</script>
