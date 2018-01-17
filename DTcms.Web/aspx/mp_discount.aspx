<%@ Page Language="C#" AutoEventWireup="true" Inherits="DTcms.Web.UI.Page.mp_discount" ValidateRequest="false" %>
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
	templateBuilder.Append("/css/mp.css?ver=8\" rel=\"stylesheet\">    \r\n</head>\r\n<body>        \r\n    ");
	if (carnivalOffline!=null&&carnivalOffline.Count>0)
	{

	templateBuilder.Append("\r\n    <!--堂吃优惠活动 Begin-->\r\n    <div class=\"carnival\"  id=\"divCarnivalOffline\">\r\n        <div class=\"main\">\r\n            ");
	int modelt__loop__id=0;
	foreach(BookingFood.Model.bf_carnival modelt in carnivalOffline)
	{
		modelt__loop__id++;


	templateBuilder.Append("            \r\n            <span class=\"date white\" style=\"margin-top: 10px;\">本次活动截止：");
	templateBuilder.Append(modelt.EndTime.ToString("yyyy年MM月dd日").ToString());
	

	templateBuilder.Append("</span>\r\n            <ul style=\"overflow: hidden; margin: 10px auto 40px auto; width: 237px;\">\r\n                ");
	DataTable carnivalOfflineDetail = get_goods_list(3,modelt.BusinessId,0,"");
	

	int dr__loop__id=0;
	foreach(DataRow dr in carnivalOfflineDetail.Rows)
	{
		dr__loop__id++;


	templateBuilder.Append("\r\n                <li>                    \r\n                    <div class=\"item one selected\" data-id=\"" + Utils.ObjectToStr(dr["id"]) + "\" data-price=\"" + Utils.ObjectToStr(dr["sell_price"]) + "\" data-title=\"" + Utils.ObjectToStr(dr["title"]) + "\" data- style=\"background: url(" + Utils.ObjectToStr(dr["mp_img_url"]) + ");\">\r\n                        <div class=\"unselect cover\"></div>\r\n                        <span style=\"background: rgb(243,152,0); position: absolute; right: 0px; width: 20px; height: 20px; border-radius: 50%; color: white; font-size: 14px;\">");
	templateBuilder.Append(dr["sell_price"].ToString().Replace(".00","").ToString());
	

	templateBuilder.Append("</span>\r\n                        <div class=\"name\">\r\n                            " + Utils.ObjectToStr(dr["title"]) + "                            \r\n                        </div>\r\n                    </div>\r\n                </li>\r\n                ");
	}	//end loop


	templateBuilder.Append("                \r\n            </ul>\r\n            ");
	}	//end loop


	templateBuilder.Append("\r\n            <span class=\"btn active\" id=\"btnCarnivalOfflineClose\">关闭</span>\r\n        </div>\r\n    </div>\r\n    <!--堂吃优惠活动 End-->\r\n    ");
	}	//end if



	templateBuilder.Append("<div style=\"display:none;\">\r\n    <script type=\"text/javascript\">var cnzz_protocol = ((\"https:\" == document.location.protocol) ? \" https://\" : \" http://\");document.write(unescape(\"%3Cspan id='cnzz_stat_icon_1256906522'%3E%3C/span%3E%3Cscript src='\" + cnzz_protocol + \"s11.cnzz.com/z_stat.php%3Fid%3D1256906522%26show%3Dpic' type='text/javascript'%3E%3C/script%3E\"));</");
	templateBuilder.Append("script>\r\n</div>");


	templateBuilder.Append("\r\n</body>\r\n</html>\r\n");
	Response.Write(templateBuilder.ToString());
}
</script>
