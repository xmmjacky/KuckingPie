<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="list.aspx.cs" Inherits="DTcms.Web.admin.condition_price.list" %>

<%@ Import Namespace="DTcms.Common" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>附加条件管理</title>
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
        <div class="navigation">首页 &gt; 附加条件管理 &gt; 管理列表</div>
        <div class="tools_box">
            <div class="tools_bar">
                <a href="edit.aspx?action=<%=DTEnums.ActionEnum.Add %>" class="tools_btn"><span><b class="add">添加</b></span></a>
                <a href="javascript:void(0);" onclick="checkAll(this);" class="tools_btn"><span><b class="all">全选</b></span></a>
                <asp:LinkButton ID="btnDelete" runat="server" CssClass="tools_btn"
                    OnClientClick="return ExePostBack('btnDelete');" OnClick="btnDelete_Click"><span><b class="delete">批量删除</b></span></asp:LinkButton>
            </div>
        </div>

        <!--列表展示.开始-->
        <asp:Repeater ID="rptList1" runat="server">
            <HeaderTemplate>
                <table class="main_table">
                    <thead>
                        <tr>
                            <th width="6%">选择</th>
                            <th>附加条件</th>
                            <th width="10%">价格</th>
                            <th width="10%">排序</th>
                            <th width="8%">操作</th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td align="center">
                        <asp:CheckBox ID="chkId" CssClass="checkall" runat="server" /><asp:HiddenField ID="hidId" Value='<%#Eval("Id")%>' runat="server" />
                    </td>
                    <td align="left"><a href="edit.aspx?action=<%#DTEnums.ActionEnum.Edit %>&id=<%#Eval("id")%>"><%#Eval("Title")%></a></td>
                    <td  align="left"><%#Eval("Price") %></td>
                    <td  align="left"><%#Eval("SortId") %></td>
                    <td align="center"><a href="edit.aspx?action=<%#DTEnums.ActionEnum.Edit %>&id=<%#Eval("id")%>">修改</a></td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>

                <%#rptList1.Items.Count == 0 ? "<tr><td align=\"center\" colspan=\"5\">暂无记录</td></tr>" : ""%>
      </tbody>
      </table>
            </FooterTemplate>
        </asp:Repeater>
        <!--列表展示.结束-->
        <div class="line15"></div>        

    </form>
</body>
</html>
