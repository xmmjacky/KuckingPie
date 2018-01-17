<%@ Page Language="C#" AutoEventWireup="true" Inherits="DTcms.Web.UI.Page.mp_full" ValidateRequest="false" %>
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
	templateBuilder.Append("\" name=\"description\">    \r\n    <link media=\"screen\" type=\"text/css\" href=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.templateskin));
	templateBuilder.Append("/css/mp.css?ver=8\" rel=\"stylesheet\">    \r\n    ");

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


	templateBuilder.Append("\r\n    <script type=\"text/javascript\">\r\n\r\n    </");
	templateBuilder.Append("script>\r\n</head>\r\n<body>    \r\n    ");
	if (carnivalDetail!=null)
	{

	templateBuilder.Append("\r\n    <!--满送活动 Begin-->\r\n    <div class=\"carnival\" id=\"divCarnivalOnline\">\r\n        <div class=\"main\">\r\n            <span class=\"num\">");
	templateBuilder.Append((carnivalUserModel!=null?carnivalUserModel.Num:0).ToString());
	

	templateBuilder.Append("</span>\r\n            <span class=\"white\">您当前累计订单次数</span>\r\n            <span class=\"date white\">本次活动截止：");
	templateBuilder.Append(carnivalModel.EndTime.ToString("yyyy年MM月dd日").ToString());
	

	templateBuilder.Append("</span>\r\n            <ul style=\"overflow: hidden; margin: 10px auto 40px auto; width: 237px; \">\r\n                ");
	int dr__loop__id=0;
	foreach(DataRow dr in carnivalDetail.Rows)
	{
		dr__loop__id++;


	templateBuilder.Append("\r\n                <li>                    \r\n                    <div class=\"item one selected\" data-id=\"" + Utils.ObjectToStr(dr["id"]) + "\" data-title=\"满" + Utils.ObjectToStr(dr["change_nums"]) + "次送" + Utils.ObjectToStr(dr["title"]) + "\" data-change=\"" + Utils.ObjectToStr(dr["change_nums"]) + "\" style=\"background: url(" + Utils.ObjectToStr(dr["mp_img_url"]) + ");\">\r\n                        <div class=\"unselect cover\">\r\n                            <span style=\"color: rgb(255,241,0); margin-top: 25px; border: 1px solid rgb(255,241,0); width: 60px; margin-left: 7px; transform: rotate(-19deg); -ms-transform: rotate(-19deg); -moz-transform: rotate(-19deg); -webkit-transform: rotate(-19deg); -o-transform: rotate(-19deg);\">需" + Utils.ObjectToStr(dr["change_nums"]) + "次</span>\r\n                        </div>\r\n                        <div class=\"name\">\r\n                            " + Utils.ObjectToStr(dr["title"]) + "                            \r\n                        </div>\r\n                    </div>\r\n                </li>\r\n                ");
	}	//end loop


	templateBuilder.Append("                \r\n            </ul>\r\n            <span class=\"btn active\" onclick=\"wx.closeWindow();\">关闭</span>\r\n            <span class=\"notice\">*当天多次订餐累计为1次 <br />*请在满足条件后自主兑换下单,逾期则自动清零<br />");
	templateBuilder.Append(Utils.ObjectToStr(config.webname));
	templateBuilder.Append("拥有活动最终解释权</span>\r\n        </div>\r\n    </div>\r\n    <!--满送活动 End-->    \r\n    ");
	}	//end if



	templateBuilder.Append("<div style=\"display:none;\">\r\n    <script type=\"text/javascript\">var cnzz_protocol = ((\"https:\" == document.location.protocol) ? \" https://\" : \" http://\");document.write(unescape(\"%3Cspan id='cnzz_stat_icon_1256906522'%3E%3C/span%3E%3Cscript src='\" + cnzz_protocol + \"s11.cnzz.com/z_stat.php%3Fid%3D1256906522%26show%3Dpic' type='text/javascript'%3E%3C/script%3E\"));</");
	templateBuilder.Append("script>\r\n</div>");


	templateBuilder.Append("\r\n</body>\r\n</html>\r\n");
	Response.Write(templateBuilder.ToString());
}
</script>
