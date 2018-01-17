<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="list.aspx.cs" Inherits="DTcms.Web.admin.worker.list" %>

<%@ Import Namespace="DTcms.Common" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>区域管理</title>
    <link type="text/css" rel="stylesheet" href="../../scripts/ui/skins/Aqua/css/ligerui-all.css" />
    <link type="text/css" rel="stylesheet" href="../images/style.css" />
    <link type="text/css" rel="stylesheet" href="../../css/pagination.css" />
    <link rel="stylesheet" type="text/css" href="../css/common.css" />
    <script type="text/javascript" src="../../scripts/jquery/jquery-1.11.0.min.js"></script>
    <script type="text/javascript" src="../../scripts/ui/js/ligerBuild.min.js"></script>
    <script type="text/javascript" src="../js/function.js"></script>
    <style>
        .din_type
        {
        }
        .din_type li
        {
            cursor: pointer;
            float: left;
            border-right: 1px solid #cdcdcd;
            color: #000;
            font-size: 1.3em;
            padding: 6px 15px;
            background: #e6e6e6;
        }
        .din_type .green
        {
            color: #04943d;
        }
        .din_type .reverse
        {
            color: #fff;
            background: #00a1e9;
        }
        .din_type .add
        {
            background: linear-gradient(to bottom, #ea4808, #e50913);
            background: -moz-linear-gradient(top, #ea4808, #e50913);
            background: -ms-linear-gradient(top, #ea4808, #e50913);
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#ea4808', endColorstr='#e50913'); /* IE6,IE7 */
            -ms-filter: "progid:DXImageTransform.Microsoft.gradient(startColorstr='#ea4808', endColorstr='#e50913')"; /* IE8 */
            color: #fff;
        }
        .din_type
        {
            clear: both;
            display: block;
            overflow: hidden;
        }
        
        
        
        
        .places
        {
            text-indent: 1em;
            font-size: 1.2em;
            color: #00a1e9;
        }
        .places span
        {
            display: inline-block;
            padding: 4px 10px 4px 10px;
        }
    </style>
</head>
<body class="mainbody">
    <form id="form1" runat="server">
    <ul class="din_type">
        <asp:Literal runat="server" ID="ltlArea"></asp:Literal>
        <li class="add" onclick="location.href='../area/edit.aspx?action=add'">+添加</li>
    </ul>
    <!--列表展示.开始-->
    <asp:Repeater ID="rptList1" runat="server" OnItemCommand="rptList_ItemCommand">
        <HeaderTemplate>
            <table class="main_table">
                <thead>
                    <tr>
                        <th style="width: 4%">
                            <span class="check_head" id="btnAddWorker">添加</span>
                        </th>
                        <th style="width: 15%;">
                            排序
                        </th>
                        <th style="width: 15%;">
                            姓名
                        </th>
                        <th>
                            电话 <a href="#" class="icon_expand"></a>
                        </th>
                        <th>
                            上班 <a href="#" class="icon_expand"></a>
                        </th>
                        <th>
                            下班 <a href="#" class="icon_expand"></a>
                        </th>
                        <th>
                            本月加班 <a href="#" class="icon_expand"></a>
                        </th>
                        <th>
                            性质 <a href="#" class="icon_expand"></a>
                        </th>
                        <th>
                            时间 <a href="#" class="icon_expand"></a>
                        </th>
                        <th style="width: 7%;">
                            管理
                        </th>
                    </tr>
                </thead>
                <tbody>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td align="center">
                    <asp:CheckBox ID="chkId" CssClass="checkall" runat="server" /><asp:HiddenField ID="hidId"
                        Value='<%#Eval("id")%>' runat="server" />
                </td>
                <td class="aln_left">
                    <%#Eval("SortId")%>
                </td>
                <td class="aln_left">
                    <%#Eval("Title")%>
                </td>
                <td>
                    <%#Eval("Telphone")%>
                </td>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
                <td>
                    <%#Eval("WorkerType")%>
                </td>
                <td>
                </td>
                <td align="center">
                    <a onclick="editWorker(<%#Eval("id")%>)" style="cursor:pointer;">修改</a>
                    <asp:LinkButton runat="server" ID="btnDelete" CommandName="delete">删除</asp:LinkButton>
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            <%#rptList1.Items.Count == 0 ? "<tr><td align=\"center\" colspan=\"7\">暂无记录</td></tr>" : ""%>
            </tbody> </table>
        </FooterTemplate>
    </asp:Repeater>
    <!--列表展示.结束-->
    <div class="line15">
    </div>
    <div class="page_box">
        <div id="PageContent" runat="server" class="flickr right">
        </div>
        <div class="left">
            显示<asp:TextBox ID="txtPageNum" runat="server" CssClass="txtInput2 small2" onkeypress="return (/[\d]/.test(String.fromCharCode(event.keyCode)));"
                OnTextChanged="txtPageNum_TextChanged" AutoPostBack="True"></asp:TextBox>条/页
        </div>
    </div>
    <div class="line10">
    </div>
    <style>
        .pop_container .pop_table
        {
            width: 380px;
        }
        .pop_container .pop_table td
        {
            padding: 2px 5px;
        }
        .pop_table .menu_item
        {
            font-weight: normal;
            margin: 0 8px;
        }
        .pop_table .menu_item .blue
        {
            color: #22abef;
        }
    </style>
    <script type="text/javascript">
        function show_pop(obj) {
            var _max_height = $('body').height();
            var _max_width = $('body').width();
            var _this_width = $(obj).width();
            var _this_height = $(obj).height();
            $(obj).css({
                top: _max_height / 2 - _this_height / 2, left: _max_width / 2 - _this_width / 2
            }).show();
            $('#pageloading_bg').show();
        }
        function hide_pop(obj) {
            $(obj).fadeOut();
            $('#pageloading_bg').hide();
        }
        function editWorker(obj) {
            $('#hfWorkerId').val(obj);
            $.ajax({
                type: "post", //提交的类型
                url: "/tools/admin_ajax.ashx?action=get_worker_edit", //提交地址
                data: { workerid: obj }, //参数
                dataType: "json",
                success: function (data) { //回调方法
                    $('.menu_item').empty();
                    if (!data) return false;
                    $(data.msgbox).appendTo($('.menu_item'));
                    $('#txtTitle').val(data.title);
                    $('#txtTelphone').val(data.telphone);
                    $('#txtSortId').val(data.sortid);
                    $('#ddlWorkerType').val(data.workertype);
                    $('#ddlOperate').val(data.operatetype);
                    show_pop('#PopupWorker');
                }
            });
            
        }
        $(function () {
            $('#PopupWorker .btn_cancle').on('click', function () {
                hide_pop('#PopupWorker');
                return false;
            });
            $('#PopupWorker .btn_confirm').on('click', function () {
                var strArea = "";
                $('.menu_item input[type="checkbox"]:checked').each(function () {
                    if (strArea != "") {
                        strArea += ",";
                    }
                    strArea += $(this).data('id');
                });
                $.ajax({
                    type: "post", //提交的类型
                    url: "/tools/admin_ajax.ashx?action=add_worker", //提交地址
                    dataType: "json",
                    data: {
                        workerid: $('#hfWorkerId').val(),
                        title: $('#txtTitle').val(),
                        telphone: $('#txtTelphone').val(),
                        sortid: $('#txtSortId').val(),
                        workertype: $('#ddlWorkerType').val(),
                        operate: $('#ddlOperate').val(),                        
                        areaid: strArea
                    }, //参数
                    success: function (data) { //回调方法
                        alert(data.msgbox);
                        location.href = location.href;
                    }
                });                
                return false;
            });
            $('#btnAddWorker').click(function () {
                $('#hfWorkerId').val('');
                $.ajax({
                    type: "post", //提交的类型
                    url: "/tools/admin_ajax.ashx?action=get_init_worker_area", //提交地址
                    data: {}, //参数
                    dataType: "html",
                    success: function (data) { //回调方法
                        if (!data) return false;
                        $(data).appendTo($('.menu_item'));
                    }
                });
                show_pop('#PopupWorker');
            });

        });
    </script>
    <div class="pop_container" style="display: none; z-index: 200; background-color: white;"
        id="PopupWorker">
        <table class="pop_table">
            <caption>
                工作人员</caption>
            <tbody>
                <tr>
                    <td style="width: 100px;">
                        排序
                    </td>
                    <td>
                        <input type="text" id="txtSortId" />*必填,纯数字
                    </td>
                </tr>
                <tr>
                    <td>
                        名称
                    </td>
                    <td>
                        <input type="text" id="txtTitle" />*必填
                    </td>
                </tr>
                <tr>
                    <td>
                        电话
                    </td>
                    <td>
                        <input type="text" id="txtTelphone" />*必填,纯数字
                    </td>
                </tr>
                <tr>
                    <td>
                        性质
                    </td>
                    <td>
                        <select id="ddlWorkerType">
                            <option value="长工">长工</option>
                            <option value="钟点工">钟点工</option>
                            <option value="收银员">收银员</option>
                        </select>
                    </td>
                </tr>
                <tr>
                    <td>
                        操作类型
                    </td>
                    <td>
                        <select id="ddlOperate">
                            <option value="0">正常</option>
                            <option value="1">不发送模板消息</option>
                            <option value="2">超范围</option>
                            <option value="3">当前餐厅忙</option>
                            <option value="4">收到通知/错送</option>
                            <option value="5">条件无法满足取消</option>
                            <option value="6">外带取餐通知</option>
                            
                        </select>
                    </td>
                </tr>
                <tr>
                    <td>
                        对应区域
                    </td>
                    <td class="menu_item">                    
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <button class="btn_confirm">
                            确认</button>&nbsp;
                        <button class="btn_cancle">
                            取消</button>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <input type="hidden" id="hfWorkerId" />
    </form>
</body>
</html>
