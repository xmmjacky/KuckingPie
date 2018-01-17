<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="DTcms.Web.admin.login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<title>管理员登录</title>
<link href="../scripts/ui/skins/Aqua/css/ligerui-all.css" rel="stylesheet" type="text/css" />
<link href="css/common.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="../scripts/jquery/jquery-1.3.2.min.js"></script>
<script type="text/javascript" src="../scripts/jquery/jquery.validate.min.js"></script> 
<script type="text/javascript" src="../scripts/jquery/messages_cn.js"></script>
<script type="text/javascript" src="../scripts/ui/js/ligerBuild.min.js"></script>
<script type="text/javascript" src="js/function.js"></script>
<style>
        body {
            background: linear-gradient(to bottom right, #fff, #eee);
            background: -moz-linear-gradient(left top, #fff, #eee);
            background: -ms-linear-gradient(top left, #fff, #eee);
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#ffffff', endColorstr='#eeeeee'); /* IE6,IE7 */
            -ms-filter: "progid:DXImageTransform.Microsoft.gradient(startColorstr='#ffffff', endColorstr='#eeeeee')"; /* IE8 */
        }
        .lg_box{
            width:240px;
            height:278px;
            
            position:absolute;
            top:50%;
            left:50%;
            margin-top:-178px;
            margin-left:-120px;
        }
        .title {
            line-height:1.2em;
            font-family:"微软雅黑","黑体",Arial;
            background: linear-gradient(to bottom, #00a6db, #0181cc);
            background: -moz-linear-gradient(top, #00a6db, #0181cc);
            background: -ms-linear-gradient(top, #00a6db, #0181cc);
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#00a6db', endColorstr='#0181cc'); /* IE6,IE7 */
            -ms-filter: "progid:DXImageTransform.Microsoft.gradient(startColorstr='#00a6db', endColorstr='#0181cc')"; /* IE8 */
            height: 73px;
            text-align: center;
            color: #fff;
            font-size: 2em;
        }
        form {
            border: 1px solid #ccc;
            background: linear-gradient(to bottom, #d8f0fa, #bde7f7);
            background: -moz-linear-gradient(top, #d8f0fa, #bde7f7);
            background: -ms-linear-gradient(top, #d8f0fa, #bde7f7);
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#d8f0fa', endColorstr='#bde7f7'); /* IE6,IE7 */
            -ms-filter: "progid:DXImageTransform.Microsoft.gradient(startColorstr='#d8f0fa', endColorstr='#bde7f7')"; /* IE8 */
            height: 175px;
            padding-top:30px;
        }
            form .txtInput {
                border: 1px solid #bdc1c2;
                height: 40px;
                width: 170px;
                margin:0 auto;
                display:block;
                background: linear-gradient(to bottom, #e8e8e8, #ffffff);
                background: -moz-linear-gradient(top, #e8e8e8, #ffffff);
                background: -ms-linear-gradient(top, #e8e8e8, #ffffff);
                filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#e8e8e8', endColorstr='#ffffff'); /* IE6,IE7 */
                -ms-filter: "progid:DXImageTransform.Microsoft.gradient(startColorstr='#e8e8e8', endColorstr='#ffffff')"; /* IE8 */
            }
            form .chkLine{width:170px;margin:0 auto;padding-bottom:10px}
            form .btnLine{
                padding-top:16px;
                text-align:center;
                margin:0 auto;
                width:170px;
            }
        #btnSubmit {
            /*ea4b09*/
            background: linear-gradient(to bottom, #ea4b09, #e50110);
            background: -moz-linear-gradient(top, #ea4b09, #e50110);
            background: -ms-linear-gradient(top, #ea4b09, #e50110);
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#ea4b09', endColorstr='#e50110'); /* IE6,IE7 */
            -ms-filter: "progid:DXImageTransform.Microsoft.gradient(startColorstr='#ea4b09', endColorstr='#e50110')"; /* IE8 */
            border: 0;
            color: #fff;
            cursor: pointer;
            font-size: 16px;
            font-weight: bold;
            font-family: "黑体";
            height: 29px;
            width: 80px;
        }
        #resetPwd {
            background: linear-gradient(to bottom, #adb1b2, #78797d);
            background: -moz-linear-gradient(top, #adb1b2, #78797d);
            background: -ms-linear-gradient(top, #adb1b2, #78797d);
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#adb1b2', endColorstr='#78797d'); /* IE6,IE7 */
            -ms-filter: "progid:DXImageTransform.Microsoft.gradient(startColorstr='#adb1b2', endColorstr='#78797d')"; /* IE8 */
            border: 0;
            color: #fff;
            height: 17px;
            width: 68px;
            font-family: "黑体";
            font-weight: bold;
            font-size: 11px;
        }
    </style>
<script type="text/javascript">
    //表单验证
    $(function () {
        //检测IE
        if ($.browser.msie && $.browser.version == "6.0") {
            window.location.href = 'ie6update.html';
        }
        $('#txtUserName').focus();
        $("#form1").validate({
            errorPlacement: function (lable, element) {
                element.ligerTip({ content: lable.html(), appendIdTo: lable });
            },
            success: function(lable){
                lable.ligerHideTip();
            }
        });
    });
</script>
</head>
<body >
<div class="lg_box">
        <div class="title">
            <div>LUCKING'PIE</div>
            <span>后台管理系统</span>
        </div>
        <form id="form1" runat="server">
            <asp:TextBox ID="txtUserName" runat="server" CssClass="txtInput required" placeholder="用户名" />
            <div class="chkLine">
                <input type="checkbox" id="chkPwd" /><label for="chkPwd">保护账号</label> &nbsp; <asp:CheckBox ID="cbRememberId" runat="server" Text="记住账号" Checked="True" />
            </div>
            <asp:TextBox ID="txtUserPwd" runat="server" CssClass="txtInput required" TextMode="Password" placeholder="密码"/>
            <div class="btnLine">
                <asp:Button ID="btnSubmit" runat="server" Text="登 录" CssClass="login_btn" onclick="btnSubmit_Click" />
                <button id="resetPwd" type="button">忘记密码</button>
            <div class="login_tip">
                <asp:Label ID="lblTip" runat="server" Text="请输入用户名及密码" Visible="False" />
            </div>
            </div>
        </form>
    </div>
</body>
</html>
