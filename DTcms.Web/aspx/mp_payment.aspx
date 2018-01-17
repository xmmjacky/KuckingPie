<%@ Page Language="C#" AutoEventWireup="true" Inherits="DTcms.Web.UI.Page.mp_payment" ValidateRequest="false" %>
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

	templateBuilder.Append("<!DOCTYPE html>\r\n<html>\r\n<head>\r\n    <meta charset=\"utf-8\" />\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0;\">\r\n    <title>支付中心－");
	templateBuilder.Append(Utils.ObjectToStr(config.webname));
	templateBuilder.Append("</title>\r\n    <script src=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.templateskin));
	templateBuilder.Append("/js/jquery-1.10.2.min.js\" type=\"text/javascript\"></");
	templateBuilder.Append("script>\r\n    <link media=\"screen\" type=\"text/css\" href=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.templateskin));
	templateBuilder.Append("/css/mp.css?ver=1\" rel=\"stylesheet\">\r\n    \r\n</head>\r\n\r\n<body style=\"background-image: url(");
	templateBuilder.Append(Utils.ObjectToStr(config.mp_backgroundimage));
	templateBuilder.Append("); background-repeat:no-repeat;\">\r\n    ");
	if (action=="succeed")
	{

	if (string.IsNullOrEmpty(orderModel.MpForHere))
	{

	templateBuilder.Append("\r\n    <script type=\"text/javascript\">\r\n        $(function () {\r\n            setTimeout(function () {\r\n                location.href = 'https://open.weixin.qq.com/connect/oauth2/authorize?appid=");
	templateBuilder.Append(Utils.ObjectToStr(config.mp_slave_appid));
	templateBuilder.Append("&redirect_uri=http%3A%2F%2Fwww.4008317417.cn%2Fmp_index.aspx&response_type=code&scope=snsapi_base&state=slave#wechat_redirect';\r\n            }, 8000);\r\n        });\r\n    </");
	templateBuilder.Append("script>\r\n    <div class=\"order_complete_window success\" style=\"height: 1443px;\">\r\n        <div class=\"order_status_window\">\r\n            <div class=\"success_f1\">\r\n                <div class=\"thank_you\">\r\n                </div>\r\n                <b class=\"status_txt\">订餐成功</b> <span class=\"auto_close\">5秒后自动关闭本页</span> <span class=\"estimate_ok\">\r\n                    配送时间：11:00-20:00,预计30-50分钟到达\r\n                </span>\r\n                <div class=\"txt\" style=\"margin-top: 10px; margin-bottom: 5px;\">\r\n                    您可在\r\n                </div>\r\n                <div class=\"btn_myorder\">\r\n                    <div class=\"cn\">\r\n                        订单处理\r\n                    </div>\r\n                </div>\r\n                <div class=\"txt\" style=\"margin-top:5px;\">\r\n                    中查询订单详情\r\n                </div>\r\n            </div>\r\n        </div>\r\n    </div>\r\n    ");
	}
	else
	{

	templateBuilder.Append("\r\n    <div class=\"order_complete_window success\" style=\"height: 1443px;\">\r\n        <div class=\"order_status_window\">\r\n            <div class=\"success_f1\">\r\n                <div class=\"thank_you\">\r\n                </div>\r\n                <b class=\"status_txt\">订餐成功</b> <span class=\"estimate_ok\">\r\n                    请记住您的取餐号，并及时至收银台取餐，感谢光临！\r\n                </span>\r\n                <div class=\"txt\" style=\"margin-top:5px;\">\r\n                    <span style=\"display: block; color: red; font-size: 24px;\">取餐号</span>\r\n                    <span style=\"display: block; color: red; font-size: 30px; font-weight: bold; \">");
	templateBuilder.Append(Utils.ObjectToStr(orderModel.MpForHere));
	templateBuilder.Append("</span>\r\n                </div>\r\n                <input type=\"button\" value=\"确认\" onclick=\"location.href = 'https://open.weixin.qq.com/connect/oauth2/authorize?appid=");
	templateBuilder.Append(Utils.ObjectToStr(config.mp_slave_appid));
	templateBuilder.Append("&redirect_uri=http%3A%2F%2Fwww.4008317417.cn%2Fmp_index.aspx&response_type=code&scope=snsapi_base&state=slave#wechat_redirect'\" style=\"background: red; border: 0px; border-radius: 10px; width: 120px; height: 30px; font-size: 22px; color: white;\"/>\r\n            </div>\r\n        </div>\r\n    </div>\r\n    ");
	}	//end if


	}	//end if


	if (action=="error")
	{

	templateBuilder.Append("\r\n    <div class=\"order_complete_window fail\" style=\"height: 1443px;\">\r\n        <div class=\"order_status_window\">\r\n            <div class=\"success_f1\">\r\n                <div class=\"fail_you\">\r\n                </div>\r\n                <b class=\"status_txt\">订餐失败，请再次确认</b>\r\n            </div>\r\n        </div>\r\n    </div>\r\n    ");
	}	//end if



	templateBuilder.Append("<div style=\"display:none;\">\r\n    <script type=\"text/javascript\">var cnzz_protocol = ((\"https:\" == document.location.protocol) ? \" https://\" : \" http://\");document.write(unescape(\"%3Cspan id='cnzz_stat_icon_1256906522'%3E%3C/span%3E%3Cscript src='\" + cnzz_protocol + \"s11.cnzz.com/z_stat.php%3Fid%3D1256906522%26show%3Dpic' type='text/javascript'%3E%3C/script%3E\"));</");
	templateBuilder.Append("script>\r\n</div>");


	templateBuilder.Append("\r\n</body>\r\n</html>\r\n");
	Response.Write(templateBuilder.ToString());
}
</script>
