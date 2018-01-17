<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="list.aspx.cs" Inherits="DTcms.Web.admin.company.list" %>

<%@ Import Namespace="DTcms.Common" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>申请群组管理</title>
    <link type="text/css" rel="stylesheet" href="../../scripts/ui/skins/Aqua/css/ligerui-all.css" />
    <link type="text/css" rel="stylesheet" href="../images/style.css" />
    <link type="text/css" rel="stylesheet" href="../../css/pagination.css" />
    <link rel="stylesheet" type="text/css" href="../css/common.css" />
    <script type="text/javascript" src="../../scripts/jquery/jquery-1.11.0.min.js"></script>
    <script type="text/javascript" src="../../scripts/ui/js/ligerBuild.min.js"></script>
    <script type="text/javascript" src="../js/function.js"></script>
    <script type="text/javascript">
        $(function () {

        });
    </script>
</head>
<body class="mainbody">
    <form id="form1" runat="server">
        <div class="navigation">首页 &gt; 申请群组管理 &gt; 管理列表</div>
        <div class="tools_box">
            <div class="tools_bar">
                <div class="search_box">
			        <asp:TextBox ID="txtKeywords" runat="server" CssClass="txtInput"></asp:TextBox>
                    <asp:Button ID="btnSearch" runat="server" Text="搜 索" CssClass="btnSearch" onclick="btnSearch_Click" />
		        </div>
                <a href="edit.aspx?action=<%=DTEnums.ActionEnum.Add %>" class="tools_btn"><span><b class="add">添加</b></span></a>
                <a href="javascript:void(0);" onclick="checkAll(this);" class="tools_btn"><span><b class="all">全选</b></span></a>
                <asp:LinkButton ID="btnDelete" runat="server" CssClass="tools_btn"
                    OnClientClick="return ExePostBack('btnDelete');" OnClick="btnDelete_Click"><span><b class="delete">批量删除</b></span></asp:LinkButton>
                <asp:LinkButton ID="btnContact" runat="server" CssClass="tools_btn"
                    OnClientClick="return ExePostBack('btnContact');" OnClick="btnContact_Click"><span><b class="combine">向下合并</b></span></asp:LinkButton>
            </div>
        </div>

        <!--列表展示.开始-->
        <asp:Repeater ID="rptList1" runat="server">
            <HeaderTemplate>
                <table class="main_table">
                    <thead>
                        <tr>
                            <th width="6%">选择</th>
                            <th width="15%">群组名称</th>
                            <th >群组地址</th>
                            <th width="6%">人数</th>
                            <th width="8%">联系方式</th>
                            <th width="8%">姓名</th>
                            <th width="8%">提交时间</th>
                            <th width="8%">状态</th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td align="center">
                        <asp:CheckBox ID="chkId" CssClass="checkall" runat="server" /><asp:HiddenField ID="hidId" Value='<%#Eval("Id")%>' runat="server" />
                    </td>
                    <td align="left"><a href="edit.aspx?action=<%#DTEnums.ActionEnum.Edit %>&id=<%#Eval("id")%>"><%#Eval("CompanyName")%></a></td>
                    <td  align="left"><%#Eval("Address") %></td>
                    <td  align="left"><%#Eval("PersonCount") %></td>
                    <td  align="left"><%#Eval("Telphone") %></td>
                    <td  align="left"><%#Eval("AcceptName") %></td>
                    <td  align="left"><%#Eval("AddTime") %></td>
                    <td  align="left"><%#GetStatus(Eval("Status").ToString()) %></td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>

                <%#rptList1.Items.Count == 0 ? "<tr><td align=\"center\" colspan=\"8\">暂无记录</td></tr>" : ""%>
      </tbody>
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
    </form>
</body>
</html>
