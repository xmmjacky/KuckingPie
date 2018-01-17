<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="order_list.aspx.cs" Inherits="DTcms.Web.admin.orders.order_list" %>
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
<style type="text/css">
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
    .orderdetail{margin-left: 20px;max-height: 500px;margin-top: -50px;overflow: scroll;position:absolute;background-color: rgb(240, 240, 240);padding: 10px;border-top-left-radius: 5px;border-top-right-radius: 5px;border-bottom-right-radius: 5px;border-bottom-left-radius: 5px;border: 1px solid black;}
</style>
<style type="text/css">
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
<script type="text/javascript">
    $(function () {
        $('.msgtable td[data-type="address"]').mousemove(function () {
            var _this = $(this);
            $('.msgtable td[data-type="address"]').each(function () {
                if ($(this).data('index') == _this.data('index')) return true;
                $(this).find('span').eq(0).css('color', 'black');
                $(this).find('div').hide();
            });
            $(this).find('span').eq(0).css('color', 'RGB(98,183,98)');
            var _height = $(window).height() - $(this).offset().top - $(this).find('.orderdetail').height() - 50;
            if (_height < 0) {
                $(this).find('.orderdetail').css('margin-top', _height + 'px');
            }
            $(this).find('div').show();
            $('.msgtable td[data-type="status"]').each(function () {
                $(this).find('div').hide();
            });
            $(this).parent().find('td[data-type="status"] div').show();
        });
        $('.msgtable td[data-type="address"]').mouseleave(function () {
            $('.msgtable td[data-type="address"]').each(function () {
                $(this).find('span').eq(0).css('color', 'black');
                $(this).find('div').hide();
            });
            $('.msgtable td[data-type="status"]').each(function () {
                $(this).find('div').hide();
            });
        });
        $('.attention').click(function () {
            $('#divAttention').css('left', ($(document).width() / 2) + 'px');
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
                    $('#txtMessageEdit').val(data.message);
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
                    address: $('#txtAddress').val(),
                    message: $('#txtMessageEdit').val()
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
                    location.reload();
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

        $('#area').on('mouseover', function () {
            $(this).find('div').show();
        })
        .on('mouseout', function () {
            $(this).find('div').hide();
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

    function QueryPayStatus(orderno) {
        $.ajax({
            type: "post",
            url: "/tools/admin_ajax.ashx?action=query_mp_pay_status",
            data: {
                orderno: orderno
            },
            dataType: "json",
            beforeSend: function (XMLHttpRequest) {
                //发送前动作
            },
            success: function (data, textStatus) {
                if (!data) return;
                alert(data.msgbox);
                if (data.paystatus == 1) {
                    location.reload();
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("状态：" + textStatus + "；出错提示：" + errorThrown);
            },
            timeout: 20000
        });
    }

    function ConfirmSyncStatus(orderno) {
        $.ajax({
            type: "post",
            url: "/tools/admin_ajax.ashx?action=confirm_sync_status",
            data: {
                orderno: orderno
            },
            dataType: "json",
            beforeSend: function (XMLHttpRequest) {
                //发送前动作
            },
            success: function (data, textStatus) {
                if (!data) return;
                if (data.msg == 0) {
                    location.reload();
                } else {
                    alert(data.msgbox);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("状态：" + textStatus + "；出错提示：" + errorThrown);
            },
            timeout: 20000
        });
    }
</script>
</head>
<body class="mainbody">
<form id="form1" runat="server">
    
    <!--列表展示.开始-->
    <asp:Repeater ID="rptList" runat="server" onitemcommand="rptList_ItemCommand">
    <HeaderTemplate>
    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="msgtable">
      <tr>
        <th width="6%">类别<a href="#" class="icon_expand"></a></th>
        <th width="8%" align="left">订单号<a href="#" class="icon_expand"></a></th>
        <th align="left">订单信息<a href="#" class="icon_expand"></a></th>
        <th width="6%" align="left">总价<a href="#" class="icon_expand"></a></th>
        <th width="4%" align="left">电话<a href="#" class="icon_expand"></a></th>
        <th width="9%" id="area"><span style="color:red;">区域</span><a href="#" class="icon_expand"></a>
            <div style="position: absolute;background: white;border: 1px solid black;padding: 3px;display:none;">
                <%=areafilter%>
            </div>
        </th>
        <th width="2%">支付<a href="#" class="icon_expand"></a></th>
        <th width="6%">订单状态<a href="#" class="icon_expand"></a></th>
        <th width="12%" align="left">下单时间<a href="#" class="icon_expand"></a></th>
        <th width="8%">管理</th>
      </tr>
    </HeaderTemplate>
    <ItemTemplate>
      <tr>
        <td align="center" <%#Eval("OrderType").ToString()=="网" ? "style='color:green;'" : Eval("OrderType").ToString()=="转单(区域)" ? "style='color:black;'" : Eval("OrderType").ToString()=="通知" ? "style='color:red;'" : ""%>>
            <%#Eval("OrderType").ToString().Replace("(区域)","")%>
            <asp:HiddenField ID="hidId" Value='<%#Eval("id")%>' runat="server" /></td>
        <td><%#Eval("order_no")%></td>
        <td data-type="address" data-index="<%#Container.ItemIndex%>">
            <span><%#GetAddress(Eval("email").ToString(),Eval("address").ToString())%></span>
            <div style="display:none;" class="orderdetail">
                <%#Eval("address")%><%#Eval("takeout").ToString()=="0" ? Eval("mobile").ToString():""%><br />
                <%#Eval("telphone")%>
                <%#Eval("telphone").ToString() != "" ? "<br />" : ""%>
                
                <span style="color:Red;cursor:pointer;display:inline;" onclick="location.href='order_list.aspx?type=<%#this.type%>&telphone=<%#Eval("telphone")%>'" ><%#Eval("email")%></span>
                <span style="color:deepskyblue;cursor:pointer;display:inline;" onclick="location.href='order_list.aspx?type=0&userid=<%#Eval("user_id")%>'" ><%#Eval("accept_name")%></span>
                <%#Eval("email").ToString() != "" ? "<br />" : ""%>    
                <%#GetOrderDetail(Eval("id").ToString())%>
                
                <span style="color:RGB(98,183,98)"><%#Eval("message")%></span>
                <%#Eval("payable_freight").ToString()!="0.00" ? "外送费："+Eval("payable_freight").ToString()+"元": ""%>
                <br />
                -----------------------------------------------<br />
                <%#GetShortOrderStatus(int.Parse(Eval("id").ToString())
                               , Eval("morningorderamount").ToString(), Eval("morningtotal").ToString(), Eval("afternoonorderamount").ToString(), Eval("afternoontotal").ToString())%>
            </div>
        </td>
        <td><%#Eval("order_amount")%></td>
        <td><%#type == "2" ? "" : Eval("telphone")%></td>
        <td align="center">
            <a class="changearea" data-id="<%#Eval("id")%>"><%#Eval("area_title")%></a>
        </td>
        <td align="center">
            <%#GetPayment(Eval("payment_status").ToString(),Eval("payment_id").ToString(),Eval("payment_time").ToString(),Eval("order_no").ToString()) %>
        </td>
        <td align="center" data-type="status"><%#GetOrderStatus(int.Parse(Eval("id").ToString())
                               , Eval("morningorderamount").ToString(), Eval("morningtotal").ToString(), Eval("afternoonorderamount").ToString(), Eval("afternoontotal").ToString(), type)%></td>
        <td><%#string.Format("{0:g}",Eval("add_time"))%></td>
        <td align="center">
            <a class="quickedit" data-id="<%#Eval("id")%>" style="cursor:pointer;">编辑</a>
            <a class="attention" data-id="<%#Eval("id")%>" style="cursor:pointer;">通知</a>
            <asp:LinkButton runat="server" ID="btnDelete" CommandName="delete" CommandArgument='<%#Eval("id")%>' OnClientClick="return confirm('确认删除?微信支付方式请先查询支付状态');">删除</asp:LinkButton>
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
                    <td>留言</td>
                    <td><input type="text" id="txtMessageEdit" /></td>
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
