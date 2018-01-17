<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="old_order_list.aspx.cs" Inherits="DTcms.Web.admin.orders.old_order_list" %>
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


</head>
<body class="mainbody">
<form id="form1" runat="server">
    <div class="tools_box">
	    <div class="tools_bar">
            <div class="search_box">
                <asp:DropDownList runat="server" ID="ddlType">
                    <asp:ListItem Text="线上" Value="1"></asp:ListItem>
                    <asp:ListItem Text="线下" Value="2"></asp:ListItem>
                </asp:DropDownList>
			    区域&地址&邮箱&电话：<asp:TextBox ID="txtKeywords" runat="server" CssClass="txtInput"></asp:TextBox>
                <asp:Button ID="btnSearch" runat="server" Text="搜 索" CssClass="btnSearch" onclick="btnSearch_Click" />
		    </div>            
        </div>        
    </div>

    <!--列表展示.开始-->
    <asp:Repeater ID="rptList" runat="server">
    <HeaderTemplate>
    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="msgtable">
      <tr>
        <th width="6%">区域<a href="#" class="icon_expand"></a></th>
        <th width="10%" align="center">总价<a href="#" class="icon_expand"></a></th>
        <th align="left">地址<a href="#" class="icon_expand"></a></th>
        <th width="10%" align="center">邮箱<a href="#" class="icon_expand"></a></th>
        <th width="10%" align="center">电话<a href="#" class="icon_expand"></a></th>
      </tr>
    </HeaderTemplate>
    <ItemTemplate>
      <tr>
        <td align="center"><%#Eval("ctry")%></td>
        <td align="center"><%#Eval("totalPrice").ToString().Replace("\\r","")%></td>
        <td ><%#string.Equals(ddlType.SelectedValue,"1") ? Eval("adress"): ""%></td>
        <td align="center"><%#string.Equals(ddlType.SelectedValue,"1") ?Eval("email") : ""%></td>
        <td align="center"><%#string.Equals(ddlType.SelectedValue, "1") ? Eval("tel") : ""%></td>
      </tr>
    </ItemTemplate>
    <FooterTemplate>
      <%#rptList.Items.Count == 0 ? "<tr><td align=\"center\" colspan=\"9\">暂无记录</td></tr>" : ""%>
      </table>
    </FooterTemplate>
    </asp:Repeater>
    <!--列表展示.结束-->

   
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
