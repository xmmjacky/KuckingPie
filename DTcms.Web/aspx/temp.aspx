<%@ Page Language="C#" AutoEventWireup="true" Inherits="DTcms.Web.UI.Page.temp" ValidateRequest="false" %>
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

	templateBuilder.Append("<!doctype html>\r\n<html>\r\n<head>\r\n    <meta http-equiv=\"content-type\" content=\"text/html;charset=utf-8\" />\r\n    <link rel=\"stylesheet\" href=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.templateskin));
	templateBuilder.Append("/css/common.css?ver=7\" type=\"text/css\" />    \r\n    <title>");
	templateBuilder.Append(Utils.ObjectToStr(config.webname));
	templateBuilder.Append("</title>    \r\n    <meta content=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.webkeyword));
	templateBuilder.Append("\" name=\"keywords\">\r\n    <meta content=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.webdescription));
	templateBuilder.Append("\" name=\"description\">\r\n    <script type=\"text/javascript\">\r\n        var browser = navigator.appName\r\n        var b_version = navigator.appVersion\r\n        var version = b_version.split(\";\");\r\n        var trim_Version = version[1].replace(/[ ]/g, \"\");\r\n        if (browser == \"Microsoft Internet Explorer\" && trim_Version == \"MSIE6.0\") {\r\n            window.location.href = '/ie6update.html';\r\n        }\r\n    </");
	templateBuilder.Append("script>\r\n    <script src=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.templateskin));
	templateBuilder.Append("/js/jquery-1.10.2.min.js\" type=\"text/javascript\"></");
	templateBuilder.Append("script>\r\n    <script type=\"text/javascript\">                \r\n        function ShowLocker() {            \r\n            $('#DivLocker').css({\r\n                \"height\": function () { return $(document).height(); },\r\n                \"width\": function () { return $(document).width(); }\r\n            });\r\n            if ($('#DivLocker').css('display') == 'none') {\r\n                $('#DivLocker').show();\r\n            } else {\r\n                $('#DivLocker').hide();\r\n            }\r\n        }        \r\n    </");
	templateBuilder.Append("script>\r\n    <script src=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.templateskin));
	templateBuilder.Append("/js/jquery.mailAutoComplete-3.1.js\" type=\"text/javascript\"></");
	templateBuilder.Append("script>    \r\n    <script src=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.templateskin));
	templateBuilder.Append("/js/jquery.cookie.js\" type=\"text/javascript\"></");
	templateBuilder.Append("script>\r\n    <script src=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.templateskin));
	templateBuilder.Append("/js/jquery.kinMaxShow-1.1.min.js\" type=\"text/javascript\"></");
	templateBuilder.Append("script>\r\n    <script src=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.templateskin));
	templateBuilder.Append("/js/jQuery.md5.js\" type=\"text/javascript\"></");
	templateBuilder.Append("script>\r\n    <script src=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.webpath));
	templateBuilder.Append("scripts/placeholder/placeholder.js\" type=\"text/javascript\"></");
	templateBuilder.Append("script>\r\n    <link media=\"screen\" type=\"text/css\" href=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.webpath));
	templateBuilder.Append("scripts/placeholder/placeholder.css\" rel=\"stylesheet\">\r\n    <link media=\"screen\" type=\"text/css\" href=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.templateskin));
	templateBuilder.Append("/css/luckingpie.css?ver=7\" rel=\"stylesheet\">\r\n    <script type=\"text/javascript\">\r\n        \r\n        $(function () {\r\n            $(\"#kinMaxShow\").kinMaxShow({\r\n                height: 322,\r\n                intervalTime: 5\r\n            });\r\n            if ('");
	templateBuilder.Append(Utils.ObjectToStr(areabusy));
	templateBuilder.Append("' == '1') {\r\n                $('#divTimePopup').fadeIn();\r\n            }\r\n            if ('");
	templateBuilder.Append(Utils.ObjectToStr(arealock));
	templateBuilder.Append("' == '1') {\r\n                $('#divlock').fadeIn();\r\n            }\r\n            $('#email').val($.cookie('user_email'));\r\n            $('#phone').val($.cookie('user_phone'));\r\n            $('#address').val($.cookie('user_address'));\r\n        });\r\n        \r\n    </");
	templateBuilder.Append("script>\r\n</head>\r\n<body>    \r\n    <header>\r\n        <div class=\"mainWrap\" style=\"max-width:1024px;\">\r\n            <div class=\"logo\"></div>\r\n            <div class=\"tele\"></div>\r\n        </div>\r\n        <div class=\"mainWrap\" style=\"max-width:1024px;\">\r\n            <div id=\"kinMaxShow\" class=\"slide\" style=\"display:none;\">\r\n                ");
	if (config.HeadPhoto1!="")
	{

	templateBuilder.Append("\r\n                <div>\r\n                    <a><img src=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.HeadPhoto1));
	templateBuilder.Append("\" /></a>\r\n                </div>\r\n                ");
	}	//end if


	if (config.HeadPhoto2!="")
	{

	templateBuilder.Append("\r\n                <div>\r\n                    <a><img src=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.HeadPhoto2));
	templateBuilder.Append("\" /></a>\r\n                </div>\r\n                ");
	}	//end if


	if (config.HeadPhoto3!="")
	{

	templateBuilder.Append("\r\n                <div>\r\n                    <a><img src=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.HeadPhoto3));
	templateBuilder.Append("\" /></a>\r\n                </div>\r\n                ");
	}	//end if


	if (config.HeadPhoto4!="")
	{

	templateBuilder.Append("\r\n                <div>\r\n                    <a><img src=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.HeadPhoto4));
	templateBuilder.Append("\" /></a>\r\n                </div>\r\n                ");
	}	//end if


	if (config.HeadPhoto5!="")
	{

	templateBuilder.Append("\r\n                <div>\r\n                    <a><img src=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.HeadPhoto5));
	templateBuilder.Append("\" /></a>\r\n                </div>\r\n                ");
	}	//end if


	if (config.HeadPhoto6!="")
	{

	templateBuilder.Append("\r\n                <div>\r\n                    <a><img src=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.HeadPhoto6));
	templateBuilder.Append("\" /></a>\r\n                </div>\r\n                ");
	}	//end if


	if (config.HeadPhoto7!="")
	{

	templateBuilder.Append("\r\n                <div>\r\n                    <a><img src=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.HeadPhoto7));
	templateBuilder.Append("\" /></a>\r\n                </div>\r\n                ");
	}	//end if


	if (config.HeadPhoto8!="")
	{

	templateBuilder.Append("\r\n                <div>\r\n                    <a><img src=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.HeadPhoto8));
	templateBuilder.Append("\" /></a>\r\n                </div>\r\n                ");
	}	//end if


	templateBuilder.Append("\r\n\r\n            </div>\r\n        </div>\r\n    </header>\r\n\r\n    <input type=\"hidden\" id=\"hfAreaId\" />\r\n    <input type=\"hidden\" id=\"hfCookie\" />\r\n    <input type=\"hidden\" id=\"hfAdditional\" />\r\n</body>\r\n</html>");
	Response.Write(templateBuilder.ToString());
}
</script>
