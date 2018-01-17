<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="edit.aspx.cs" Inherits="DTcms.Web.admin.carnival.edit" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>编辑商品信息</title>
    <link href="../../scripts/ui/skins/Aqua/css/ligerui-all.css" rel="stylesheet" type="text/css" />
    <link href="../images/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../scripts/jquery/jquery-1.8.3.min.js"></script>
    <script type="text/javascript" src="../../scripts/jquery/jquery.form.js"></script>
    <script type="text/javascript" src="../../scripts/jquery/jquery.validate.min.js"></script>
    <script type="text/javascript" src="../../scripts/jquery/messages_cn.js"></script>
    <script type="text/javascript" src="../../scripts/ui/js/ligerBuild.min.js"></script>    
    <script type="text/javascript" src="../js/function.js"></script>    
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
        });        
    </script>
</head>
<body class="mainbody">
    <form id="form1" runat="server">
        <div class="navigation"><a href="javascript:history.go(-1);" class="back">后退</a>首页 &gt; 活动管理 &gt; 编辑信息</div>
        <div id="contentTab">
            <ul class="tab_nav">
                <li class="selected"><a onclick="tabs('#contentTab',0);" href="javascript:;">基本信息</a></li>
            </ul>

            <div class="tab_con" style="display: block;">
                <table class="form_table">
                    <col width="150px" />
                    <tbody>
                        <tr>
                            <th>活动名称：</th>
                            <td>
                                <asp:TextBox ID="txtTitle" runat="server" CssClass="txtInput normal required" MaxLength="100" /></td>
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
                            <th>活动类型：</th>
                            <td>
                                <asp:RadioButtonList ID="rblType" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                    <asp:ListItem Text="满送" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="单品汇" Value="2"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr >
                            <th>区域：</th>
                            <td>
                                <asp:CheckBoxList ID="cblArea" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                        
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                        <tr>
                            <th>参与活动的分类：</th>
                            <td>
                                <asp:RadioButtonList ID="cblComboCategory" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                </asp:RadioButtonList>
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
        <asp:HiddenField runat="server" ID="hfUrlReferrer" />
    </form>
</body>
</html>
