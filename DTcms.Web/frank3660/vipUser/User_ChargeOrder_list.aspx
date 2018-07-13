<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="User_ChargeOrder_list.aspx.cs" Inherits="DTcms.Web.frank3660.vipUser.User_ChargeOrder_list" %>

<%@ Import Namespace="DTcms.Common" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>充值记录列表</title>
    <link type="text/css" rel="stylesheet" href="../../scripts/ui/skins/Aqua/css/ligerui-all.css" />
    <link type="text/css" rel="stylesheet" href="../images/style.css" />
    <link type="text/css" rel="stylesheet" href="../../css/pagination.css" />
    <link rel="stylesheet" type="text/css" href="../css/common.css" />
    <script type="text/javascript" src="../../scripts/jquery/jquery-1.11.0.min.js"></script>
    <script type="text/javascript" src="../../scripts/ui/js/ligerBuild.min.js"></script>
    <script type="text/javascript" src="../js/function.js"></script>
    <style type="text/css">
        .pop_container .pop_table {
            width: 380px;
        }

            .pop_container .pop_table td {
                padding: 2px 5px;
            }

        .pop_table .menu_item b {
            color: #22abef;
            font-weight: normal;
            display: inline-block;
            margin: 0 8px;
        }

        .orderdetail {
            margin-left: 20px;
            max-height: 500px;
            margin-top: -50px;
            overflow: scroll;
            position: absolute;
            background-color: rgb(240, 240, 240);
            padding: 10px;
            border-top-left-radius: 5px;
            border-top-right-radius: 5px;
            border-bottom-right-radius: 5px;
            border-bottom-left-radius: 5px;
            border: 1px solid black;
        }
    </style>
    <style type="text/css">
        .din_type {
        }

            .din_type li {
                cursor: pointer;
                float: left;
                border-right: 1px solid #cdcdcd;
                color: #000;
                font-size: 1.3em;
                padding: 6px 15px;
                background: #e6e6e6;
            }

            .din_type .green {
                color: #04943d;
            }

            .din_type .reverse {
                color: #fff;
                background: #00a1e9;
            }

            .din_type .add {
                background: linear-gradient(to bottom, #ea4808, #e50913);
                background: -moz-linear-gradient(top, #ea4808, #e50913);
                background: -ms-linear-gradient(top, #ea4808, #e50913);
                filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#ea4808', endColorstr='#e50913'); /* IE6,IE7 */
                -ms-filter: "progid:DXImageTransform.Microsoft.gradient(startColorstr='#ea4808', endColorstr='#e50913')"; /* IE8 */
                color: #fff;
            }

        .din_type {
            clear: both;
            display: block;
            overflow: hidden;
        }




        .places {
            text-indent: 1em;
            font-size: 1.2em;
            color: #00a1e9;
        }

            .places span {
                display: inline-block;
                padding: 4px 10px 4px 10px;
            }
    </style>
    <script type="text/javascript">
        $(function () {

            $('#divChangeArea .btn_confirm').click(function () {
                if ($('#cboChangeArea input:checked').length == 0) {
                    alert('请选择转单区域');
                    return false;
                }
                $.ajax({
                    type: "post",
                    url: "/tools/vip_ajax.ashx?action=change_area",
                    data: {
                        id: $('#hfRepeatId').val(),
                        changeareaid: $('#cboChangeArea input:checked').val(),
                        changeareaname: $('#cboChangeArea input:checked').next().text()
                    },
                    //dataType: "json",
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
                    error: function (err, status) {
                        alert("状态：" + err + "；出错提示：" + status);
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

    </script>
</head>
<body class="mainbody">
    <form id="form1" runat="server">

        <!--列表展示.开始-->
        <asp:Repeater ID="rptList" runat="server" OnItemCommand="rptList_ItemCommand">
            <HeaderTemplate>
                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="msgtable">
                    <tr>
                        <th width="15%" align="left">单号<a href="#" class="icon_expand"></a></th>
                        <th width="20%" align="left">昵称<a href="#" class="icon_expand"></a></th>
                        <th width="10%" align="left">金额<a href="#" class="icon_expand"></a></th>
                        <th width="20%" id="area"><span style="color: red;">区域</span><a href="#" class="icon_expand"></a>
                            <div style="position: absolute; background: white; border: 1px solid black; padding: 3px; display: none;">
                             
                            </div>
                        </th>
                        <th width="20%" align="left">创建时间<a href="#" class="icon_expand"></a></th>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td><%#Eval("Id")%></td>
                    <td><%#Eval("NickName")%></td>
                    <td><%#Eval("Amount")%></td>
                    <td align="center">
                        <a class="changearea" ><%#Eval("AreaName")%></a>
                    </td>
                    <td><%#string.Format("{0:g}",Eval("CreateTime"))%></td>
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
                    OnTextChanged="txtPageNum_TextChanged" AutoPostBack="True"></asp:TextBox>条/页
            </div>
        </div>
        <div class="line10"></div>

        <div class="pop_container" style="display: none; z-index: 200; background-color: white;" id="divChangeArea">
            <table class="pop_table">
                <caption>转单(区域)</caption>
                <tbody>
                    <tr>
                        <td style="width: 30px;">区域</td>
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

