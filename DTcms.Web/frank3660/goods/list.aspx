<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="list.aspx.cs" Inherits="DTcms.Web.admin.goods.list" %>
<%@ Import namespace="DTcms.Common" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<title>商品管理</title>
<link type="text/css" rel="stylesheet" href="../../scripts/ui/skins/Aqua/css/ligerui-all.css" />
<link type="text/css" rel="stylesheet" href="../images/style.css" />
<link type="text/css" rel="stylesheet" href="../../css/pagination.css" />
<link rel="stylesheet" type="text/css" href="../css/common.css" />
<script type="text/javascript" src="../../scripts/jquery/jquery-1.11.0.min.js"></script>
<script type="text/javascript" src="../../scripts/ui/js/ligerBuild.min.js"></script>
<script type="text/javascript" src="../js/function.js"></script>
<style>
        .din_type{

        }
            .din_type li {
                cursor:pointer;
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
            .din_type{
                clear:both;
                display:block;
                overflow:hidden;
            }
        

        

        .places {
            text-indent: 1em;
            font-size: 1.2em;
            color: #00a1e9;
        }
            .places span{
                display:inline-block;
                padding:4px 10px 4px 10px;
            }
            

    </style><script type="text/javascript">
    $(function () {
        $('span[name="articlearea"]').on('click', function () {
            var obj = $(this);
            $.ajax({
                type: "post",
                url: "/tools/admin_ajax.ashx?action=switch_area",
                data: {
                    changeareaid: obj.data('areaid'),
                    articleid: obj.data('articleid'),
                    type: obj.data('type')
                },
                dataType: "json",
                beforeSend: function (XMLHttpRequest) {
                    //发送前动作
                },
                success: function (data, textStatus) {
                    if (!data) return;
                    if (data.msg == 1) {
                        if (obj.css('color') == 'rgb(0, 0, 0)') {
                            obj.removeAttr("style");
                        } else {
                            obj.css('color','black');
                        }
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("状态：" + textStatus + "；出错提示：" + errorThrown);
                },
                timeout: 20000
            });
        });
    });
</script>
</head>
<body class="mainbody">
<form id="form1" runat="server">
    <ul class="din_type">
        <asp:Literal runat="server" ID="ltlCategory"></asp:Literal>
        <li class="add" onclick="location.href='../category/edit.aspx?action=add'">+添加</li>
    </ul>
    <div class="places" style="display:none;">
        <asp:Literal runat="server" ID="ltlArea"></asp:Literal>        
    </div>

    <!--列表展示.开始-->
    <asp:Repeater ID="rptList1" runat="server" onitemcommand="rptList_ItemCommand">
    <HeaderTemplate>
    <table class="main_table">
    <thead>
      <tr>
            <th style="width:4%">
                <span class="check_head" onclick="location.href='edit.aspx?action=<%=DTEnums.ActionEnum.Add %>&channel_id=<%=this.channel_id %>'">添加</span>
            </th>
            <th style="width:4%;">
                排序
            </th>
            <th style="width:10%;">
                品名
                <a href="#" class="icon_expand"></a>
            </th>
            <th style="width:4%;">
                单价
                <a href="#" class="icon_expand"></a>
            </th>
            <%--<th style="width:8%;">
                类别
                <a href="#" class="icon_expand"></a>
            </th>--%>
            <th >
                区域
                <a href="#" class="icon_expand"></a>
            </th>
            <th style="width:5%;">
                图片
            </th>
            <th style="width:14%;">
                套餐组合
            </th>
            <th  style="width:15%;">
                附加条件
            </th>            
            <th style="width:7%;">
                管理
            </th>
        </tr>
    </thead>
    <tbody>
    </HeaderTemplate>
    <ItemTemplate>
      <tr>
        <td align="center"><asp:CheckBox ID="chkId" CssClass="checkall" runat="server" /><asp:HiddenField ID="hidId" Value='<%#Eval("id")%>' runat="server" /></td>
        <td class="aln_left"><%#Eval("sort_id")%></td>
        <td><a href="<%# Eval("type").ToString() == "one" ? "edit" : "combo"%>.aspx?channel_id=<%#this.channel_id %>&action=<%#DTEnums.ActionEnum.Edit %>&id=<%#Eval("id")%>"><%#Eval("title")%></a></td>
        <td class="aln_right"><%#Eval("sell_price")%></td>
        <%--<td><%#new DTcms.BLL.category().GetTitle(Convert.ToInt32(Eval("category_id")))%></td>--%>
        <td class="condition"><%#GetAreaByArtecleId(Eval("id").ToString(), Eval("type").ToString())%></td>
        <td><img src="<%#Eval("img_url") %>" style="width:50px;" /></td>
        <td class="condition" style="color:Black;"><%#GetComboDetail(Eval("id").ToString())%></td>
        <td class="condition"><%#GetTaste(Eval("taste").ToString())%></td>        
        <td align="center"><a href="<%# Eval("type").ToString() == "one" ? "edit" : "combo"%>.aspx?channel_id=<%#this.channel_id %>&action=<%#DTEnums.ActionEnum.Edit %>&id=<%#Eval("id")%>">修改</a>
                            <asp:LinkButton runat="server" ID="btnDelete" CommandName="delete" CommandArgument='<%#Eval("type") %>'>删除</asp:LinkButton>
        </td>
      </tr>
    </ItemTemplate>
    <FooterTemplate>
        
      <%#rptList1.Items.Count == 0 ? "<tr><td align=\"center\" colspan=\"7\">暂无记录</td></tr>" : ""%>
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
    <div class="line10"></div>

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
        function show_pop() {
            var _max_height = $('body').height();
            var _max_width = $('body').width();
            var _this_width = $('.pop_container').width();
            var _this_height = $('.pop_container').height();
            $('.pop_container').css({
                top: _max_height / 2 - _this_height / 2, left: _max_width / 2 - _this_width / 2
            }).show();
            $('#pageloading_bg').show();
        }
        function hide_pop(animate) {
            if (!animate) {
                $('.pop_container').hide();
            } else {
                $('.pop_container').fadeOut();
            }
            $('#pageloading_bg').hide();
        }
        $(function () {
            $('.pop_container .btn_cancle').on('click', function () {
                hide_pop(true);
            })
            $('.pop_container .btn_confirm').on('click', function () {
                $.ajax({
                    type: "post", //提交的类型
                    url: "/ashx/BusinessControl.ashx?class=CorrectSellerPurchase&action=correct", //提交地址
                    dataType: "json",
                    data: { ddlChannel: $('#ddlChannel').val(),
                            txtTitle: $('#txtTitle').val(),
                            txtSortId: $('#txtSortId').val()
                    }, //参数
                    success: function (data) { //回调方法
                        alert(data.msgbox);
                    }
                });
                hide_pop(true);
            })
        })
    </script>
    <div class="pop_container" style="display:none;z-index:200;background-color: white;">
        <table class="pop_table">
            <caption>编辑类别</caption>
            <tbody>
                <tr>
                    <td>类型</td>
                    <td style="width:80%">
                        <select id="ddlChannel">
                            <option value="2">正常</option>
                            <option value="3">隐藏</option>
                        </select>
                    </td>
                </tr>
                <tr>
                    <td>名称</td>
                    <td><input type="text" id="txtTitle" /></td>
                </tr>
                <tr>
                    <td>排序</td>
                    <td><input type="text" id="txtSortId" /></td>
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
</form>
</body>
</html>
