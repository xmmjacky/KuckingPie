<%@ Page Language="C#" AutoEventWireup="true" Inherits="DTcms.Web.UI.Page.payment" ValidateRequest="false" %>
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

	templateBuilder.Append("<!doctype html>\r\n<html>\r\n<head>\r\n    <meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0;\">\r\n    <title>支付中心</title>\r\n    <script src=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.templateskin));
	templateBuilder.Append("/js/jquery-1.10.2.min.js\" type=\"text/javascript\"></");
	templateBuilder.Append("script>\r\n    <script src=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.templateskin));
	templateBuilder.Append("/js/jquery.cookie.js\" type=\"text/javascript\"></");
	templateBuilder.Append("script>\r\n\r\n    <link media=\"screen\" type=\"text/css\" href=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.templateskin));
	templateBuilder.Append("/css/mp.css?ver=48\" rel=\"stylesheet\">    \r\n    <script src=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.templateskin));
	templateBuilder.Append("/js/jquery-ui.min.js\" type=\"text/javascript\"></");
	templateBuilder.Append("script>\r\n    <link media=\"screen\" type=\"text/css\" href=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.templateskin));
	templateBuilder.Append("/css/jquery-ui.min.css\" rel=\"stylesheet\">\r\n    <script type=\"text/javascript\">\r\n        var takeout = ");
	templateBuilder.Append(Utils.ObjectToStr(takeout));
	templateBuilder.Append(";\r\n        var mpforhere = '");
	templateBuilder.Append(Utils.ObjectToStr(mpforhere));
	templateBuilder.Append("';\r\n        $(function () {\r\n            if (takeout == 0) {\r\n                $('.order_complete_window.success .status_txt').html('订单已成功提交！');\r\n                $('.order_complete_window.success').show();\r\n            } else {\r\n                $('#lblForHere').text(mpforhere);\r\n                $('.order_complete_window.success_forhere').show();\r\n            }\r\n            setTimeout(function () {\r\n                location.href = '");
	templateBuilder.Append(Utils.ObjectToStr(url));
	templateBuilder.Append("';\r\n            }, 5000);\r\n        });\r\n    </");
	templateBuilder.Append("script>\r\n</head>\r\n\r\n<body style=\"background-color: white;\">\r\n    <div class=\"order_complete_window success\" style=\"height: 773px; display: none;\">\r\n        <div class=\"order_status_window\">\r\n            <div class=\"success_f1\">\r\n                <div class=\"thank_you\">\r\n                </div>\r\n                <b class=\"status_txt\">订单已成功提交！</b> <span class=\"auto_close\">5秒后自动关闭本页</span> <span class=\"estimate_ok\">\r\n                    配送时间：11:00-20:00,预计30-50分钟到达\r\n                </span>\r\n                <div class=\"txt\" style=\"margin-top: 10px; margin-bottom: 5px;\">\r\n                    您可在\r\n                </div>\r\n                <div class=\"btn_myorder\">\r\n                    <div class=\"cn\">\r\n                        订单处理\r\n                    </div>\r\n                </div>\r\n                <div class=\"txt\" style=\"margin-top:5px;\">\r\n                    中查询订单详情\r\n                </div>\r\n            </div>\r\n        </div>\r\n    </div>\r\n\r\n    <div class=\"order_complete_window success_forhere\" style=\"height: 773px; display: none;\">\r\n        <div class=\"order_status_window\">\r\n            <div class=\"success_f1\">\r\n                <div class=\"thank_you\">\r\n                </div>\r\n                <b class=\"status_txt\">订餐成功</b> <span class=\"estimate_ok\">\r\n                    请记住您的取餐号，并及时至收银台取餐，感谢光临！\r\n                </span>\r\n                <div class=\"txt\" style=\"margin-top:5px;\">\r\n                    <span style=\"display: block; color: red; font-size: 24px;\">取餐号</span>\r\n                    <span style=\"display: block; color: red; font-size: 30px; font-weight: bold; \" id=\"lblForHere\"></span>\r\n                </div>\r\n            </div>\r\n        </div>\r\n    </div>\r\n</body>\r\n</html>\r\n");
	Response.Write(templateBuilder.ToString());
}
</script>
