<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="bi_category_order_list.aspx.cs" Inherits="DTcms.Web.admin.orders.bi_category_order_list" %>
<%@ Import namespace="DTcms.Common" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<title>统计列表</title>
<link type="text/css" rel="stylesheet" href="../../scripts/ui/skins/Aqua/css/ligerui-all.css" />
<link type="text/css" rel="stylesheet" href="../images/style.css" />
<link type="text/css" rel="stylesheet" href="../../css/pagination.css" />
<link rel="stylesheet" type="text/css" href="../css/common.css" />
<script type="text/javascript" src="../../scripts/jquery/jquery-1.11.0.min.js"></script>
<script type="text/javascript" src="../../scripts/ui/js/ligerBuild.min.js"></script>
<script type="text/javascript" src="../../scripts/datepicker/js/bootstrap-datepicker.js"></script>
<link type="text/css" rel="stylesheet" href="../../scripts/datepicker/css/datepicker.css" />
<script type="text/javascript" src="../js/function.js"></script>
<style>
    .pop_container .pop_table{width:380px;}
        .pop_container .pop_table td{
            padding:2px 5px;
        }
        .pop_table .menu_item b {
            color: #22abef;
            font-weight: normal;
            display: inline-block;
            margin: 0 8px;
        }

</style>
<script type="text/javascript">
    $(function () {
        $('#txtBeginTime').datepicker({
            format: 'yyyy-mm-dd'
        });
        $('#txtEndTime').datepicker({
            format: 'yyyy-mm-dd'
        });
    });
</script>

</head>
<body class="mainbody">
<form id="form1" runat="server">
    <div class="tools_box">
	    <div class="tools_bar">
            <div class="search_box">
                总销售量<%=totalamount%>
                <asp:TextBox ID="txtBeginTime" runat="server" CssClass="txtInput"></asp:TextBox>
                <asp:TextBox ID="txtEndTime" runat="server" CssClass="txtInput"></asp:TextBox>
                <asp:Button ID="btnSearch" runat="server" Text="搜 索" CssClass="btnSearch" onclick="btnSearch_Click" />
		    </div>      
            <div style="margin-top: 7px;">    
                <span style="display:inline-block;color:Black;background-color:rgb(230,230,230);height: 30px;line-height: 30px;width: 150px;text-align: center;font-size: 18px;cursor:pointer;" onclick="location.href='bi_area_order_list.aspx'">区域</span>
                <span style="display:inline-block;color:White;background-color:rgb(0,161,233);height: 30px;line-height: 30px;width: 150px;text-align: center;font-size: 18px;cursor:pointer;" onclick="location.href='bi_category_order_list.aspx'">菜品</span>
            </div>                        
        </div>        
        <div class="tools_bar">
        <div style="margin-top: 7px;">                
                <asp:Literal runat="server" ID="ltlCategory"></asp:Literal>
            </div>  
        </div>   
    </div>
    <div style="overflow:scroll;">
        <asp:Literal runat="server" ID="ltlTable"></asp:Literal>
    </div>

   
    <div class="line15"></div>
    <div class="page_box">
      <div id="PageContent" runat="server" class="flickr right"></div>
      <div class="left">
         显示<asp:TextBox ID="txtPageNum" runat="server" CssClass="txtInput2 small2" onkeypress="return (/[\d]/.test(String.fromCharCode(event.keyCode)));" 
             ontextchanged="txtPageNum_TextChanged" AutoPostBack="True"></asp:TextBox>条/页
      </div>
    </div>
    <div class="line10"></div>
     
    <input type="hidden" id="hfRepeatId" />
</form>
</body>
</html>
