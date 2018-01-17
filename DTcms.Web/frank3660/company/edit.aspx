<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="edit.aspx.cs" Inherits="DTcms.Web.admin.company.edit" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>编辑申请群组信息</title>
    <link href="../../scripts/ui/skins/Aqua/css/ligerui-all.css" rel="stylesheet" type="text/css" />
    <link href="../images/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../scripts/jquery/jquery-1.3.2.min.js"></script>
    <script type="text/javascript" src="../../scripts/jquery/jquery.form.js"></script>
    <script type="text/javascript" src="../../scripts/jquery/jquery.validate.min.js"></script>
    <script type="text/javascript" src="../../scripts/jquery/messages_cn.js"></script>
    <script type="text/javascript" src="../../scripts/ui/js/ligerBuild.min.js"></script>
    <script type='text/javascript' src="../../scripts/swfupload/swfupload.js"></script>
    <script type='text/javascript' src="../../scripts/swfupload/swfupload.queue.js"></script>
    <script type="text/javascript" src="../../scripts/swfupload/swfupload.handlers.js"></script>
    <script type="text/javascript" src="../js/function.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../editor/kindeditor-min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../editor/lang/zh_CN.js"></script>
    <script type="text/javascript" src="../../scripts/datepicker_new/WdatePicker.js"></script>
    <script type="text/javascript">
        //表单验证
        $(function () {
            $("#form1").validate({
                invalidHandler: function (e, validator) {
                    parent.jsprint("有 " + validator.numberOfInvalids() + " 项填写有误，请检查！", "", "Warning");
                },
                errorPlacement: function (lable, element) {
                    //可见元素显示错误提示
                    if (element.parents(".tab_con").css('display') != 'none') {
                        element.ligerTip({ content: lable.html(), appendIdTo: lable });
                    }
                },
                success: function (lable) {
                    lable.ligerHideTip();
                }
            });
            //初始化上传控件
            $(function () {
                
        });
    });
    </script>
</head>
<body class="mainbody">
    <form id="form1" runat="server">
        <div class="navigation"><a href="javascript:history.go(-1);" class="back">后退</a>首页 &gt; 申请群组管理 &gt; 编辑信息</div>
        <div id="contentTab">
            <ul class="tab_nav">
                <li class="selected"><a onclick="tabs('#contentTab',0);" href="javascript:;">基本信息</a></li>
            </ul>

            <div class="tab_con" style="display: block;">
                <table class="form_table">
                    <col width="150px">
                    <col>
                    <tbody>
                        <tr>
                            <th>群组名称：</th>
                            <td>
                                <asp:TextBox ID="txtCompanyName" runat="server" CssClass="txtInput normal required" MaxLength="25" /></td>
                        </tr>
                        <tr>
                            <th>群组地址：</th>
                            <td>
                                <asp:TextBox ID="txtAddress" runat="server" CssClass="txtInput normal required" MaxLength="250" /></td>
                        </tr>
                        <tr>
                            <th>人数：</th>
                            <td>
                                <asp:TextBox ID="txtPersonCount" runat="server" CssClass="txtInput normal required digits" MaxLength="3">0</asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>联系人：</th>
                            <td>
                                <asp:TextBox ID="txtAcceptName" runat="server" CssClass="txtInput normal" MaxLength="250" /></td>
                        </tr>
                        <tr>
                            <th>联系电话：</th>
                            <td>
                                <asp:TextBox ID="txtTelphone" runat="server" CssClass="txtInput normal" MaxLength="250" /></td>
                        </tr>
                        <tr>
                            <th>状态：</th>
                            <td>
                                <asp:DropDownList id="ddlStatus" CssClass="select2 required" runat="server">
                                    <asp:ListItem Text="合并" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="申请" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="通过" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="驳回" Value="3"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <th>合并群组：</th>
                            <td>
                                <asp:DropDownList id="ddlContactCompany" CssClass="select2 " runat="server">
                                    <asp:ListItem Text="请选择合并的群组" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <th>开始时间：</th>
                            <td>
                                <asp:TextBox ID="txtBeginTime" runat="server" CssClass="txtInput normal" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'})" MaxLength="16" /></td>
                        </tr>
                        <tr>
                            <th>结束时间：</th>
                            <td>
                                <asp:TextBox ID="txtEndTime" runat="server" CssClass="txtInput normal" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'})" MaxLength="16" /></td>
                        </tr>
                        <tr>
                            <th>二维码：</th>
                            <td>
                                <asp:Image runat="server" ID="imgQr" Width="200" />
                            </td>
                        </tr>

                    </tbody>
                </table>
            </div>

            <div class="foot_btn_box">
                <asp:Button ID="btnSubmit" runat="server" Text="提交保存" CssClass="btnSubmit" OnClick="btnSubmit_Click" />
                &nbsp;<input name="重置" type="reset" class="btnSubmit" value="重 置" />
            </div>
        </div>
    </form>
</body>
</html>
