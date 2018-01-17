<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="fake_order_list.aspx.cs" Inherits="DTcms.Web.admin.orders.fake_order_list" %>
<%@ Import namespace="DTcms.Common" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<title>订单列表</title>
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
<script type="text/javascript">
    $(function () {
        $('.msgtable td[data-type="address"]').mousemove(function () {
            $('.msgtable td[data-type="address"]').each(function () {
                $(this).find('span').eq(0).css('color', 'black');
                $(this).find('div').hide();
            });
            $(this).find('span').eq(0).css('color', 'RGB(98,183,98)');
            $(this).find('div').show();
        });
        $('.msgtable td[data-type="address"]').mouseleave(function () {
            $('.msgtable td[data-type="address"]').each(function () {
                $(this).find('span').eq(0).css('color', 'black');
                $(this).find('div').hide();
            });
        });
        $('.attention').click(function () {
            if ($(this).position().left + 120 > $(document).width()) {
                $('#divAttention').css('left', '1016.5px');
            } else {
                $('#divAttention').css('left', $(this).position().left + 'px');
            }
            $('#divAttention').css('top', ($(this).position().top + 20) + 'px');
            $('#txtMessage').val('');
            $('#divAttention').fadeIn();
            $('#hfRepeatId').val($(this).data('id'));
        });
        $('#btnMessage').click(function () {
            if (!$('#txtMessage').val()) return;
            $.ajax({
                type: "post",
                url: "/tools/admin_ajax.ashx?action=repeat_order",
                data: {
                    id: $('#hfRepeatId').val(),
                    message: $('#txtMessage').val()
                },
                dataType: "json",
                beforeSend: function (XMLHttpRequest) {
                    //发送前动作
                },
                success: function (data, textStatus) {
                    if (!data) return;
                    if (data.msg == 1) {
                        alert(data.msgbox);
                        location.href = location.href;
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("状态：" + textStatus + "；出错提示：" + errorThrown);
                },
                timeout: 20000
            });
            $('#divAttention').fadeOut();
        });
        $('.quickedit').click(function () {
            var orderid = $(this).data('id');
            $('#hfRepeatId').val($(this).data('id'));
            $('#txtTelphone').val('');
            $('#txtAddress').val('');
            $.ajax({
                type: "post",
                url: "/tools/admin_ajax.ashx?action=get_order",
                data: {
                    id: orderid
                },
                dataType: "json",
                beforeSend: function (XMLHttpRequest) {
                    //发送前动作
                },
                success: function (data, textStatus) {
                    if (!data) return;
                    $('#txtTelphone').val(data.telphone);
                    $('#txtAddress').val(data.address);

                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("状态：" + textStatus + "；出错提示：" + errorThrown);
                },
                timeout: 20000
            });
            ShowDialog('divQuickEdit');
            //$('#divQuickEdit').fadeIn();
        });
        $('#divQuickEdit .btn_confirm').click(function () {
            $.ajax({
                type: "post",
                url: "/tools/admin_ajax.ashx?action=update_order",
                data: {
                    id: $('#hfRepeatId').val(),
                    telphone: $('#txtTelphone').val(),
                    address: $('#txtAddress').val()
                },
                dataType: "json",
                beforeSend: function (XMLHttpRequest) {
                    //发送前动作
                },
                success: function (data, textStatus) {
                    if (!data) return;
                    if (data.msg == 0) {
                        alert(data.msgbox);
                    } else {
                        $('.msgtable a[data-id="' + $('#hfRepeatId').val() + '"]').parent().parent().find('td').eq(4).html($('#txtTelphone').val());
                        $('#divQuickEdit').fadeOut();
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("状态：" + textStatus + "；出错提示：" + errorThrown);
                },
                timeout: 20000
            });
            return false;
        });
        $('#divQuickEdit .btn_cancle').click(function () {
            $('#divQuickEdit').fadeOut();
            return false;
        });
        $('.changearea').click(function () {
            var orderid = $(this).data('id');
            $('#hfRepeatId').val($(this).data('id'));
            ShowDialog('divChangeArea');
            //$('#divQuickEdit').fadeIn();
        });
        $('#divChangeArea .btn_confirm').click(function () {
            if ($('#cboChangeArea input:checked').length == 0) {
                alert('请选择转单区域');
                return false;
            }
            $.ajax({
                type: "post",
                url: "/tools/admin_ajax.ashx?action=change_area",
                data: {
                    id: $('#hfRepeatId').val(),
                    changeareaid: $('#cboChangeArea input:checked').val()
                },
                dataType: "json",
                beforeSend: function (XMLHttpRequest) {
                    //发送前动作
                },
                success: function (data, textStatus) {
                    if (!data) return;
                    if (data.msg == 0) {
                        alert(data.msgbox);
                    } else {
                        var areatitle = $('#cboChangeArea label[for="' + $('#cboChangeArea input:checked').attr('id') + '"]').html();
                        $('.msgtable a[data-id="' + $('#hfRepeatId').val() + '"]').parent().parent().find('td').eq(5).find('a').html(areatitle);
                        $('#divChangeArea').fadeOut();
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("状态：" + textStatus + "；出错提示：" + errorThrown);
                },
                timeout: 20000
            });
            return false;
        });
        $('#divChangeArea .btn_cancle').click(function () {
            $('#divChangeArea').fadeOut();
            return false;
        });
        $('#btnClose').click(function () {
            $('#divAttention').fadeOut();
        });
        $('.din_type li').click(function () {
            if (confirm('确认清空？')) {
                $('#btnClear').click();
            }
        });
    });

    function ShowDialog(obj) {
        var _max_height = $('body').height();
        var _max_width = $('body').width();
        var _this_width = $('#' + obj).width();
        var _this_height = $('#' + obj).height();
        $('#' + obj).css({
            top: _max_height / 2 - _this_height / 2, left: _max_width / 2 - _this_width / 2
        }).show();
    }
</script>
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
        <li class="add">清空</li>
    </ul>
    <asp:Button runat="server" ID="btnClear" style="display:none;" />
    <!--列表展示.开始-->
    <asp:Repeater ID="rptList" runat="server" onitemcommand="rptList_ItemCommand">
    <HeaderTemplate>
    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="msgtable">
      <tr>
        <th width="6%">类别<a href="#" class="icon_expand"></a></th>
        <th align="left">订单号<a href="#" class="icon_expand"></a></th>
        <th width="23%" align="left">订单信息<a href="#" class="icon_expand"></a></th>
        <th width="10%" align="left">总价<a href="#" class="icon_expand"></a></th>
        <th width="10%" align="left">联系方式<a href="#" class="icon_expand"></a></th>
        <th width="6%">区域<a href="#" class="icon_expand"></a></th>
        <th width="10%">支付状态<a href="#" class="icon_expand"></a></th>
        <th width="8%">订单状态<a href="#" class="icon_expand"></a></th>
        <th width="12%" align="left">下单时间<a href="#" class="icon_expand"></a></th>
        <th width="6%">管理</th>
      </tr>
    </HeaderTemplate>
    <ItemTemplate>
      <tr>
        <td align="center">
            <%#Eval("OrderType")%>
            <asp:HiddenField ID="hidId" Value='<%#Eval("id")%>' runat="server" /></td>
        <td><%#Eval("order_no")%></td>
        <td data-type="address">
            <span><%#Eval("address")%></span>
            <div style="display:none;">
                <%#Eval("telphone")%>
                <br />
                <span style="color:Red;" ><%#Eval("email")%></span>                
                <br />
                -----------------------------------------------<br />
                <%#GetOrderDetail(Eval("id").ToString())%>
                
                <span style="color:RGB(98,183,98)"><%#Eval("message")%></span>
                <%#Eval("payable_freight").ToString()!="0.00" ? "外送费："+Eval("payable_freight").ToString()+"元": ""%>
            </div>
        </td>
        <td><%#Eval("order_amount")%></td>
        <td><%#Eval("telphone")%></td>
        <td align="center">
            <a class="changearea" data-id="<%#Eval("id")%>"><%#Eval("area_title")%></a>
        </td>
        <td align="center"><%#Eval("payment_status").ToString()=="1" ? "未支付":"<span style=\"color:red;\">已支付<span>"%></td>
        <td align="center"><%#GetOrderStatus(int.Parse(Eval("id").ToString())
                               , Eval("morningorderamount").ToString(), Eval("morningtotal").ToString(), Eval("afternoonorderamount").ToString(), Eval("afternoontotal").ToString())%></td>
        <td><%#string.Format("{0:g}",Eval("add_time"))%></td>
        <td align="center" >
            <a style="display:none;" class="quickedit" data-id="<%#Eval("id")%>">编辑</a>
            <a style="display:none;" class="attention" data-id="<%#Eval("id")%>">通知</a>
            <asp:LinkButton runat="server" ID="btnDelete" CommandName="delete" CommandArgument='<%#Eval("id")%>'>恢复</asp:LinkButton>
        </td>
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
    <div style="z-index: 10;position: fixed;width: 120px;background-color: rgb(239,239,239);text-align: center;display:none;" id="divAttention">
        <textarea id="txtMessage" style="width: 116px;height: 60px;border: 0px;background-color: rgb(239,239,239);"></textarea>
        <input type="button" id="btnMessage" value="添加" style="background-color: rgb(255, 0, 0);border: 0px;border-radius: 5px;color: white;margin: 2px;" />
        <input type="button" id="btnClose" value="关闭" style="background-color: rgb(255, 0, 0);border: 0px;border-radius: 5px;color: white;margin: 2px;" />
    </div>
    <div class="pop_container" style="display:none;z-index:200;background-color: white;" id="divQuickEdit">
        <table class="pop_table">
            <caption>快速编辑</caption>
            <tbody>                
                <tr>
                    <td>电话</td>
                    <td><input type="text" id="txtTelphone" /></td>
                </tr>
                <tr>
                    <td>地址</td>
                    <td><input type="text" id="txtAddress" /></td>
                </tr> 
                <tr>
                    <td colspan="2">
                        <button class="btn_confirm">确认</button>&nbsp;
                        <button class="btn_cancle">取消</button>
                    </td>
                </tr>                               
            </tbody>
        </table>
    </div>

    <div class="pop_container" style="display:none;z-index:200;background-color: white;" id="divChangeArea">
        <table class="pop_table">
            <caption>转单(区域)</caption>
            <tbody>                
                <tr>
                    <td style="width:30px;">区域</td>
                    <td>
                        <asp:RadioButtonList runat="server" RepeatDirection="Horizontal" ID="cboChangeArea" RepeatLayout="Flow"></asp:RadioButtonList>
                    </td>
                </tr>    
                <tr>
                    <td colspan="2">
                        <button class="btn_confirm">确认</button>&nbsp;
                        <button class="btn_cancle">取消</button>
                    </td>
                </tr>            
            </tbody>
        </table>
    </div>

    <input type="hidden" id="hfRepeatId" />
</form>
</body>
</html>
