<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="combo.aspx.cs" Inherits="DTcms.Web.admin.goods.combo" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<title>编辑商品信息</title>
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
<script type="text/javascript">
    //加载编辑器
    $(function () {
        var editor = KindEditor.create('textarea[name="txtContent"]', {
            resizeType: 1,
            uploadJson: '../../tools/upload_ajax.ashx?action=EditorFile&IsWater=1',
            fileManagerJson: '../../tools/upload_ajax.ashx?action=ManagerFile',
            allowFileManager: true
        });

    });
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
    
    //计算用户组价格
    $(function () {
        $("#txtSellPrice").change(function () {
            var sprice = $(this).val();
            if (sprice > 0) {
                $(".groupprice").each(function () {
                    //$(this).val($(this).attr("discount") * sprice / 100);
                    var num = $(this).attr("discount") * sprice / 100;
                    $(this).val(ForDight(num, 2));
                    //$(this).val(num);
                });
            }
        });
        $('#cblAllCheck').click(function () {
            if ($(this).attr('checked') == true) {
                $('#cblArea input').attr('checked', 'checked');
            } else {
                $('#cblArea input').removeAttr('checked');
            }

        });
    });
    //四舍五入函数
    function ForDight(Dight, How) {
        Dight = Math.round(Dight * Math.pow(10, How)) / Math.pow(10, How);
        return Dight;
    }
</script>
</head>
<body class="mainbody">
<form id="form1" runat="server">
<div class="navigation"><a href="javascript:history.go(-1);" class="back">后退</a>套餐信息<a href="edit.aspx?action=add&channel_id=2">单品信息</a></div>
<div id="contentTab">
    <ul class="tab_nav">
        <li class="selected"><a onclick="tabs('#contentTab',0);" href="javascript:;">基本信息</a></li>
        <li><a onclick="tabs('#contentTab',1);" href="javascript:;">详细描述</a></li>
    </ul>

    <div class="tab_con" style="display:block;">
        <table class="form_table">
            <col width="150px"><col>
            <tbody>
            <tr>
                <th></th>
                <td>
                    <span style="font-size:18px;color:Red;">套餐信息</span>
                </td>
            </tr>
            <tr>
                <th>所属类别：</th>
                <td><asp:DropDownList id="ddlCategoryId" CssClass="select2 required" runat="server"></asp:DropDownList></td>
            </tr>
            <tr>
                <th>套餐名称：</th>
                <td><asp:TextBox ID="txtTitle" runat="server" CssClass="txtInput normal required" maxlength="100" /></td>
            </tr>
            <tr>
                <th>上下架：</th>
                <td><asp:DropDownList id="ddlIsLock" CssClass="select2 required" runat="server">
                    <asp:ListItem Text="上架" Value="0"></asp:ListItem>
                    <asp:ListItem Text="下架" Value="1"></asp:ListItem>
                </asp:DropDownList></td>
            </tr>
            <tr >
                <th>附加条件：</th>
                <td>
                    <asp:CheckBoxList ID="cblTaste" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                        
                    </asp:CheckBoxList>
                </td>
            </tr>
            <tr >
                <th>区域：</th>
                <td>
                    
                    <asp:CheckBoxList ID="cblArea" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                        
                    </asp:CheckBoxList>
                    <label><input type="checkbox" id="cblAllCheck" />全选</label>
                </td>
            </tr>
            <tr >
                <th>搭配食品分类：</th>
                <td>
                    <asp:CheckBoxList ID="cblComboCategory" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                        
                    </asp:CheckBoxList>
                </td>
            </tr>                       
            
            <tr>
                <th>排序数字：</th>
                <td><asp:TextBox ID="txtSortId" runat="server" CssClass="txtInput small required digits" maxlength="10">99</asp:TextBox></td>
            </tr>            
            <tr>
                <th valign="top" style="padding-top:10px;">上传图片：</th>
                <td>
                    <asp:TextBox ID="txtImgUrl" runat="server" CssClass="txtInput normal left" maxlength="255"></asp:TextBox>
                    <a href="javascript:;" class="files"><input type="file" id="FileUpload" name="FileUpload" onchange="Upload('SingleFile', 'txtImgUrl', 'FileUpload');" /></a>
                    <span class="uploading">正在上传，请稍候...</span>
                </td>
            </tr>
            
            </tbody>
        </table>
    </div>

    <div class="tab_con">
        <table class="form_table">
            <col width="150px"><col>
            <tbody>            
            <tr>
                <th valign="top">详细描述：</th>
                <td>
                    <textarea id="txtContent" cols="100" rows="8" style="width:99%;height:350px;visibility:hidden;" runat="server"></textarea>
                </td>
            </tr>
            </tbody>
        </table>
    </div>

    <div class="foot_btn_box">
    <asp:Button ID="btnSubmit" runat="server" Text="提交保存" CssClass="btnSubmit" onclick="btnSubmit_Click" />
    &nbsp;<input name="重置" type="reset" class="btnSubmit" value="重 置" />
    </div>
</div>
<asp:HiddenField runat="server" ID="hfUrlReferrer" />
</form>
</body>
</html>
