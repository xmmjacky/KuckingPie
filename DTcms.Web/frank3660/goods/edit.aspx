<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="edit.aspx.cs" Inherits="DTcms.Web.admin.goods.edit" ValidateRequest="false" %>

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
    <script type="text/javascript" src="../../scripts/datepicker/js/bootstrap-datepicker.js"></script>
<link type="text/css" rel="stylesheet" href="../../scripts/datepicker/css/datepicker.css" />
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
    //初始化上传控件
    $(function () {
        InitSWFUpload("../../tools/upload_ajax.ashx", "Filedata", "<%=siteConfig.attachimgsize%> KB", "../../scripts/swfupload/swfupload.swf", 1, 1);
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
<div class="navigation"><a href="javascript:history.go(-1);" class="back">后退</a>单品信息 <a href="combo.aspx?action=add&channel_id=2">套餐信息</a></div>
<div id="contentTab">
    <ul class="tab_nav">
        <li class="selected"><a onclick="tabs('#contentTab',0);" href="javascript:;">基本信息</a></li>
        <li><a onclick="tabs('#contentTab',1);" href="javascript:;">详细描述</a></li>
        <li><a onclick="tabs('#contentTab',2);" href="javascript:;">起送份数</a></li>
    </ul>

    <div class="tab_con" style="display:block;">
        <table class="form_table">
            <col width="150px"><col>
            <tbody>
            <tr>
                <th></th>
                <td>
                    <span style="font-size:18px;color:Red;">单品信息</span>
                </td>
            </tr>
            <tr>
                <th>所属类别：</th>
                <td><asp:DropDownList id="ddlCategoryId" CssClass="select2 required" runat="server"></asp:DropDownList></td>
            </tr>
            <tr>
                <th>对应类别：</th>
                <td><asp:DropDownList id="ddlOppositionCategoryId" CssClass="select2 required" runat="server"></asp:DropDownList><label>*套餐内单品必选,对应的后厨分类。(否则后厨不显示套餐产品)</label></td>
            </tr>
            <tr>
                <th>别名分类：</th>
                <td><asp:DropDownList id="ddlNickName" CssClass="select2 required" runat="server">
                    <asp:ListItem Text="请选择" Value="0"></asp:ListItem>
                </asp:DropDownList></td>
            </tr>
            <tr>
                <th>商品名称：</th>
                <td><asp:TextBox ID="txtTitle" runat="server" CssClass="txtInput normal required" maxlength="100" /></td>
            </tr>
                <tr>
                <th>标签：</th>
                <td><asp:TextBox ID="txtMark" runat="server" CssClass="txtInput normal" maxlength="25" /></td>
            </tr>
            <tr>
                <th>上下架：</th>
                <td><asp:DropDownList id="ddlIsLock" CssClass="select2 required" runat="server">
                    <asp:ListItem Text="上架" Value="0"></asp:ListItem>
                    <asp:ListItem Text="下架" Value="1"></asp:ListItem>
                </asp:DropDownList></td>
            </tr>
            <tr>
                <th>群组专享：</th>
                <td><asp:CheckBox runat="server" ID="cboOnlyCompany" />只有群组成员可以购买,显示条件由群组开启的购买时间窗口和区域上下架控制</td>
            </tr>
                <tr>
                <th>头条显示：</th>
                <td><asp:CheckBox runat="server" ID="cboHeadLine" />选择区域后立即浮层展示</td>
            </tr>
            <tr >
                <th>附加条件：</th>
                <td>
                    <asp:CheckBoxList ID="cblTaste" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                        
                    </asp:CheckBoxList>
                </td>
            </tr>
                <tr >
                <th>附加条件(带价格)：</th>
                <td>
                    <asp:CheckBoxList ID="cblConditionPrice" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                        
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
            <tr style="display:none;">
                <th>推荐类型：</th>
                <td>
                    <asp:CheckBoxList ID="cblItem" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                        <asp:ListItem Value="1">允许评论</asp:ListItem>
                        <asp:ListItem Value="1">置顶</asp:ListItem>
                        <asp:ListItem Value="1">推荐</asp:ListItem>
                        <asp:ListItem Value="1">热点</asp:ListItem>
                        <asp:ListItem Value="1">幻灯</asp:ListItem>
                        <asp:ListItem Value="1">隐藏</asp:ListItem>
                    </asp:CheckBoxList>
                </td>
            </tr>
            <tr>
                <th>商品货号：</th>
                <td><asp:TextBox ID="txtGoodsNo" runat="server" CssClass="txtInput normal" maxlength="100" />头条商品的黄字标题</td>
            </tr>
            <tr>
                <th>库存数量：</th>
                <td><asp:TextBox ID="txtStockQuantity" runat="server" CssClass="txtInput small required digits" maxlength="100" >0</asp:TextBox>
                </td>
            </tr>
            <tr>
                <th>市场价格：</th>
                <td><asp:TextBox ID="txtMarketPrice" runat="server" CssClass="txtInput small required number" maxlength="10" >0</asp:TextBox>
                    <label>*只供参考的市场价格</label></td>
            </tr>
            <tr>
                <th>销售价格：</th>
                <td><asp:TextBox ID="txtSellPrice" runat="server" CssClass="txtInput small required number" maxlength="10" >0</asp:TextBox>
                    <label>*用户交易的实际价格</label></td>
            </tr>
            <tr style="display:none;">
                <th>振华价格：</th>
                <td><asp:TextBox ID="txtZhenghuaPrice" runat="server" CssClass="txtInput small required number" maxlength="10" >0</asp:TextBox>
                    </td>
            </tr>
            <tr style="display:none;">
                <th>淘宝价格：</th>
                <td><asp:TextBox ID="txtTaobaoPrice" runat="server" CssClass="txtInput small required number" maxlength="10" >0</asp:TextBox>
                    </td>
            </tr>
            <asp:Repeater ID="rptPrice" runat="server">
                    <HeaderTemplate>
            <tr style="display:none;">
                <th valign="top" style="padding-top:10px;">会员价格：</th>
                <td>
                    <table border="0" cellspacing="0" cellpadding="0" class="border_table">
                        <tbody>
                        <col width="80px"><col>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <th><%#Eval("title")%></th>
                            <td>
                                <asp:HiddenField ID="hidePriceId" runat="server" />
                                <asp:HiddenField ID="hideGroupId" Value='<%#Eval("id") %>' runat="server" />
                                <asp:TextBox ID="txtGroupPrice" runat="server" size="10" discount='<%#Eval("discount") %>' CssClass="txtInput groupprice small required number" maxlength="10">0</asp:TextBox>
                                <label>享受<%#Eval("discount") %>折优惠</label>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </tbody>
                    </table>
                </td>
            </tr>
                    </FooterTemplate>
            </asp:Repeater>
            <tr style="display:none;">
                <th>购物积分：</th>
                <td><asp:TextBox ID="txtPoint" runat="server" CssClass="txtInput small required number" maxlength="10" >0</asp:TextBox>
                    <label>*如果正数则返还用户积分，负数则扣取积分</label></td>
            </tr>
            <tr>
                <th>排序数字：</th>
                <td><asp:TextBox ID="txtSortId" runat="server" CssClass="txtInput small required digits" maxlength="10">99</asp:TextBox></td>
            </tr>
            <tr style="display:none;">
                <th>浏览次数：</th>
                <td><asp:TextBox ID="txtClick" runat="server" CssClass="txtInput small required digits" maxlength="10">0</asp:TextBox></td>
            </tr>
            <tr style="display:none;">
                <th>起送份数：</th>
                <td><asp:TextBox ID="txtLowNum" runat="server" CssClass="txtInput small required digits" maxlength="10">0</asp:TextBox>
                    <label>*0为不做起送限制，填写数字后在添加购物车时自动添加该份数，并不能减少低于该份数</label></td>
            </tr>
            <tr>
                <th>兑换所需次数：</th>
                <td><asp:TextBox ID="txtChangeNums" runat="server" CssClass="txtInput small required digits" maxlength="3">0</asp:TextBox>
                    <label>*当商品作为满送商品时，需满足的兑换次数</label></td>
            </tr>
            <tr>
                <th>微信图片：</th>
                <td>
                    <asp:TextBox ID="txtMpImgUrl" runat="server" CssClass="txtInput normal left" maxlength="255"></asp:TextBox>
                    <a href="javascript:;" class="files"><input type="file" id="FileUpload1" name="FileUpload1" onchange="Upload('SingleFile', 'txtMpImgUrl', 'FileUpload1');" /></a>
                    <span class="uploading">正在上传，请稍候...</span>
                </td>
            </tr>
            <tr>
                <th>头条图片：</th>
                <td>
                    <asp:TextBox ID="txtHeadLineImgUrl" runat="server" CssClass="txtInput normal left" maxlength="255"></asp:TextBox>
                    <a href="javascript:;" class="files"><input type="file" id="FileUpload2" name="FileUpload2" onchange="Upload('SingleFile', 'txtHeadLineImgUrl', 'FileUpload2');" /></a>
                    <span class="uploading">正在上传，请稍候...</span>
                </td>
            </tr>
            <tr>
                <th valign="top" style="padding-top:10px;">上传图片：</th>
                <td>
                    <input type="text" class="txtInput normal left" />
                    <div class="upload_btn"><span id="upload"></span></div><label>可以上传多张图片。</label>
                    <div class="clear"></div>
                    <!--封面隐藏值.开始-->
                    <!--
                    <input type="hidden" name="focus_photo" id="focus_photo" value=""/>
                    -->
                    <asp:HiddenField ID="focus_photo" runat="server" />
                    <!--封面隐藏值.结束-->
                    <!--上传提示.开始-->
                    <div id="show"></div>
                    <!--上传提示.结束-->
                    <!--图片列表.开始-->
                    <div id="show_list">
                        <ul>
                          <asp:Literal ID="LitAlbumList" runat="server"></asp:Literal>
                        </ul>
                    </div>
                    <!--图片列表.结束-->
                </td>
            </tr>
            
            <!--
            <tr>
                <th>扩展属性：</th>
                <td>
                    <table border="0" cellspacing="0" cellpadding="0" class="border_table">
                        <tbody>
                        <col width="80px"><col>
                        <tr>
                            <th>属性一</th>
                            <td><input name="nav_url" type="text" value="" class="txtInput middle" /></td>
                        </tr>
                        <tr>
                            <th>属性二</th>
                            <td><input name="nav_url" type="text" value="" class="txtInput middle" /></td>
                        </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
            -->
            <!--扩展属性.开始-->
            <asp:Literal ID="LitAttributeList" runat="server"></asp:Literal>
            <!--扩展属性.结束-->

            </tbody>
        </table>
    </div>

    <div class="tab_con">
        <table class="form_table">
            <col width="150px"><col>
            <tbody>
            <tr style="display:none;">
                <th>赞成人数：</th>
                <td><asp:TextBox ID="txtDiggGood" runat="server" CssClass="txtInput small required digits" maxlength="10">0</asp:TextBox></td>
            </tr>
            <tr style="display:none;">
                <th>反对人数：</th>
                <td><asp:TextBox ID="txtDiggBad" runat="server" CssClass="txtInput small required digits" maxlength="10">0</asp:TextBox></td>
            </tr>
            <tr style="display:none;">
                <th>URL链接：</th>
                <td><asp:TextBox ID="txtLinkUrl" runat="server" CssClass="txtInput normal" maxlength="255"></asp:TextBox><label>URL跳转地址</label></td>
            </tr>
            <tr>
                <th valign="top">详细描述：</th>
                <td>
                    <textarea id="txtContent" cols="100" rows="8" style="width:99%;height:350px;visibility:hidden;" runat="server"></textarea>
                </td>
            </tr>
            </tbody>
        </table>
    </div>

    <div class="tab_con">
        <table class="form_table">
            <col width="150px"><col>
            <tbody>
            <tr style="display:none;">
                <th>SEO标题：</th>
                <td><asp:TextBox ID="txtSeoTitle" runat="server" maxlength="255" CssClass="txtInput normal" /></td>
            </tr>
            <tr style="display:none;">
                <th>SEO关健字：</th>
                <td><asp:TextBox ID="txtSeoKeywords" runat="server" maxlength="255" TextMode="MultiLine" CssClass="small" /></td>
            </tr>
            <tr style="display:none;">
                <th>SEO描述：</th>
                <td><asp:TextBox ID="txtSeoDescription" runat="server" maxlength="255" TextMode="MultiLine" CssClass="small" /></td>
            </tr>
            <tr>
                <th>起送份数：</th>
                <td>
                    <asp:TextBox ID="txtBeginTime1" runat="server" CssClass="txtInput"></asp:TextBox>
                    <asp:TextBox ID="txtEndTime1" runat="server" CssClass="txtInput"></asp:TextBox>
                    <asp:TextBox ID="txtLowNum1" runat="server" CssClass="txtInput small required digits" maxlength="100" >0</asp:TextBox>
                    输入份数才有效.设置为0则取消此设置
                </td>
            </tr>
                <tr>
                <th>起送份数：</th>
                <td>
                    <asp:TextBox ID="txtBeginTime2" runat="server" CssClass="txtInput" ></asp:TextBox>
                    <asp:TextBox ID="txtEndTime2" runat="server" CssClass="txtInput" ></asp:TextBox>
                    <asp:TextBox ID="txtLowNum2" runat="server" CssClass="txtInput small required digits" maxlength="100" >0</asp:TextBox>
                    输入份数才有效.设置为0则取消此设置
                </td>
            </tr>
                <tr>
                <th>起送份数：</th>
                <td>
                    <asp:TextBox ID="txtBeginTime3" runat="server" CssClass="txtInput" ></asp:TextBox>
                    <asp:TextBox ID="txtEndTime3" runat="server" CssClass="txtInput" ></asp:TextBox>
                    <asp:TextBox ID="txtLowNum3" runat="server" CssClass="txtInput small required digits" maxlength="100" >0</asp:TextBox>
                    输入份数才有效.设置为0则取消此设置
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
