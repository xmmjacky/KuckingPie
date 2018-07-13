<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="edit.aspx.cs" Inherits="DTcms.Web.admin.area.edit" ValidateRequest="false" %>

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
<script type="text/javascript" src="https://map.qq.com/api/js?v=2.exp&key=BOEBZ-2AB2R-IKTWG-W2JQG-HEUOV-2RF7Z&libraries=drawing"></script>
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
        
        //绘制配送范围
        var map = new qq.maps.Map(document.getElementById("containerArea"), {
            center: new qq.maps.LatLng(parseFloat($('#hfLag').val()), parseFloat($('#hfLng').val())),
            zoom: 13
        });
        var drawingManager = new qq.maps.drawing.DrawingManager({
            drawingControl: true,
            drawingControlOptions: {
                position: qq.maps.ControlPosition.TOP_CENTER,
                drawingModes: [
                    qq.maps.drawing.OverlayType.POLYGON
                ]
            }
        });
        drawingManager.setMap(map);
        //设置overlaycomplete事件
        qq.maps.event.addListener(drawingManager, 'overlaycomplete', function (res) {
            //获取到绘制的多边形或者矩形的经纬度坐标数组
            $('#hfDistributionArea').val('');
            var path = res.overlay.getPath().forEach(function (e) {
                $('#hfDistributionArea').val($('#hfDistributionArea').val()+ e + "|");
            });
        });
        
        if ($('#hfDistributionArea').val().length) {
            var path1 = new Array();
            var oldarea = $('#hfDistributionArea').val().split('|');
            for (var i = 0; i < oldarea.length; i++) {
                if (!oldarea[i].length) continue;
                path1.push(new qq.maps.LatLng(parseFloat(oldarea[i].split(',')[0]), parseFloat(oldarea[i].split(',')[1])));
            }
            //添加覆盖物
            var polygon = new qq.maps.Polygon({
                path: path1,
                strokeColor: new qq.maps.Color(38, 145, 234, 1),
                strokeWeight: 3,
                fillColor: new qq.maps.Color(38, 145, 234, 0.3),
                map: map
            });
            $('#btnClearArea').show();
            var mapM = document.getElementById("btnClearArea");
            qq.maps.event.addDomListener(mapM, "click", function () {
                if (polygon.getMap()) {
                    polygon.setMap(null);
                    $('#btnClearArea').hide();
                    $('#hfDistributionArea').val('');
                }
            });
        }
        //绘制配送区域2
        var map2 = new qq.maps.Map(document.getElementById("containerArea_2"), {
            center: new qq.maps.LatLng(parseFloat($('#hfLag').val()), parseFloat($('#hfLng').val())),
            zoom: 13
        });
        var drawingManager2 = new qq.maps.drawing.DrawingManager({
            drawingControl: true,
            drawingControlOptions: {
                position: qq.maps.ControlPosition.TOP_CENTER,
                drawingModes: [
                    qq.maps.drawing.OverlayType.POLYGON,
                    qq.maps.drawing.OverlayType.POLYGON
                ]
            }
        });
        drawingManager2.setMap(map2);
        //设置overlaycomplete事件
        qq.maps.event.addListener(drawingManager2, 'overlaycomplete', function (res) {
            //获取到绘制的多边形或者矩形的经纬度坐标数组
            $('#hfDistributionArea_2').val('');
            var path = res.overlay.getPath().forEach(function (e) {
                $('#hfDistributionArea_2').val($('#hfDistributionArea_2').val() + e + "|");
            });
        });

        if ($('#hfDistributionArea_2').val().length) {
            var path1 = new Array();
            var oldarea = $('#hfDistributionArea_2').val().split('|');
            for (var i = 0; i < oldarea.length; i++) {
                if (!oldarea[i].length) continue;
                path1.push(new qq.maps.LatLng(parseFloat(oldarea[i].split(',')[0]), parseFloat(oldarea[i].split(',')[1])));
            }
            //添加覆盖物
            var polygon2 = new qq.maps.Polygon({
                path: path1,
                strokeColor: new qq.maps.Color(38, 145, 234, 1),
                strokeWeight: 3,
                fillColor: new qq.maps.Color(38, 145, 234, 0.3),
                map: map2
            });
            $('#btnClearArea2').show();
            var mapM2 = document.getElementById("btnClearArea2");
            qq.maps.event.addDomListener(mapM2, "click", function () {
                if (polygon2.getMap()) {
                    polygon2.setMap(null);
                    $('#btnClearArea2').hide();
                    $('#hfDistributionArea_2').val('');
                }
            });
        }
        //商家定位
        var centerPosition = new qq.maps.LatLng(parseFloat($('#hfLag').val()), parseFloat($('#hfLng').val()));
        var markerPosition;
        var mapPosition = new qq.maps.Map(document.getElementById("containerPosition"), {
            center: centerPosition,
            zoom: 14
        });
        if (parseFloat($('#hfLng').val()) != 0) {
            centerPosition = new qq.maps.LatLng(parseFloat($('#hfLag').val()), parseFloat($('#hfLng').val()));
            markerPosition = new qq.maps.Marker({
                position: centerPosition,
                animation: qq.maps.MarkerAnimation.DROP,
                map: mapPosition
            });
        }
        qq.maps.event.addListener(mapPosition, 'click', function (event) {
            if (markerPosition) markerPosition.setMap(null);
            $('#hfLag').val(event.latLng.getLat());
            $('#hfLng').val(event.latLng.getLng());
            markerPosition = new qq.maps.Marker({
                position: event.latLng,
                animation: qq.maps.MarkerAnimation.DROP,
                map: mapPosition
            });
        });
        
    });    
</script>
</head>
<body class="mainbody">
<form id="form1" runat="server">
<div class="navigation"><a href="javascript:history.go(-1);" class="back">后退</a>首页 &gt; 区域管理 &gt; 编辑信息</div>
<div id="contentTab">
    <ul class="tab_nav">
        <li class="selected"><a onclick="tabs('#contentTab',0);" href="javascript:;">基本信息</a></li>
        <li class=""><a onclick="tabs('#contentTab',1);" href="javascript:;">店铺地址</a></li>
        <li class=""><a onclick="tabs('#contentTab',2);" href="javascript:;">配送范围</a></li>
        <li class=""><a onclick="tabs('#contentTab',3);" href="javascript:;">外卖Cookie</a></li>
    </ul>

    <div class="tab_con" style="display:block;">
        <table class="form_table">
            <col width="150px"><col>
            <tbody>            
            <tr>
                <th>所属区域：</th>
                <td>
                    <asp:DropDownList runat="server" ID="ddlParentId">
                        <asp:ListItem Text="请选择" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>对应区域(线上/线下)：</th>
                <td>
                    <asp:DropDownList runat="server" ID="ddlOppositeId">
                        <asp:ListItem Text="请选择" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                    微信堂吃订单选择外带时,自动转单的区域
                </td>
            </tr>
                <tr>
                <th>收银平板转单区域：</th>
                <td>
                    <asp:CheckBoxList ID="cblChangeOrderArea" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                    </asp:CheckBoxList>
                    <font style="color:red;">收银平板上的转单功能可使用的区域,没有选择区域在平板上无法转单过去</font>
                </td>
            </tr>
            <tr>
                <th>管理员：</th>
                <td>
                    <asp:DropDownList runat="server" ID="ddlManager" CssClass="required">
                        <asp:ListItem Text="请选择" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>区域：</th>
                <td><asp:TextBox ID="txtArea" runat="server" CssClass="txtInput normal required" maxlength="100" /></td>
            </tr>
                <tr>
                <th>地址：</th>
                <td><asp:TextBox ID="txtAddress" runat="server" CssClass="txtInput normal" maxlength="250" /></td>
            </tr>
            <tr style="display:none;">
                <th>排序：</th>
                <td><asp:TextBox ID="txtSort" runat="server" CssClass="txtInput normal required digits" maxlength="3" >99</asp:TextBox>                
                </td>
            </tr>
            <tr>
                <th>显示：</th>
                <td><asp:CheckBox runat="server" ID="cboIsShow" />是外卖店铺时选择显示</td>
            </tr>
            <tr>
                <th>高峰：</th>
                <td><asp:CheckBox runat="server" ID="cboIsBusy" /></td>
            </tr>
            <tr>
                <th>下架：</th>
                <td><asp:CheckBox runat="server" ID="cboIsLock" /></td>
            </tr>
            <tr>
                <th>延迟一天：</th>
                <td><asp:CheckBox runat="server" ID="cboIsUnWelcome" />当天关注的用户延迟一天使用线下活动商品</td>
            </tr>
            <tr>
                <th>介绍：</th>
                <td><asp:TextBox ID="txtDescription" runat="server" CssClass="txtInput normal" maxlength="249" /></td>
            </tr>

            </tbody>
        </table>
    </div>
    <div class="tab_con" style="display:none;">
        <table class="form_table">
            <col width="150px"><col>
            <tbody>            
            <tr>
                <th>店铺地址：</th>
                <td>
                    <div style="width:50%;height:500px" id="containerPosition"></div>
                    <asp:HiddenField runat="server" ID="hfLag" Value="" />
                    <asp:HiddenField runat="server" ID="hfLng" Value="" />
                </td>
            </tr>

            </tbody>
        </table>
    </div>
    <div class="tab_con" style="display:none;">
        <table class="form_table">
            <tbody>            
            <tr>
                <td>
                    <div style="width:49%;height:700px;display:inline-block;" id="containerArea"></div>
                    <div style="width:49%;height:700px;display:inline-block;" id="containerArea_2"></div>
                    <input type="button" id="btnClearArea" class="btnSubmit" value="清除左侧" style="display:none;" />
                    <input type="button" id="btnClearArea2" class="btnSubmit" value="清除右侧" style="display:none;" />
                    <asp:HiddenField runat="server" ID="hfDistributionArea" Value="" />
                    <asp:HiddenField runat="server" ID="hfDistributionArea_2" Value="" />
                </td>
            </tr>

            </tbody>
        </table>
    </div>
    <div class="tab_con" style="display:none;">
        <table class="form_table">
            <tbody>
                <tr>
                    <th style="width:120px;">美团Cookie：</th>
                    <td>
                        <asp:TextBox ID="txtMeituanCookie" runat="server" TextMode="MultiLine" CssClass="small"></asp:TextBox></td>
                </tr>
                <tr>
                    <th style="width:120px;">百度Cookie：</th>
                    <td>
                        <asp:TextBox ID="txtBaiduCookie" runat="server" TextMode="MultiLine" CssClass="small"></asp:TextBox></td>
                </tr>
                <tr>
                    <th style="width:120px;">饿了么Cookie：</th>
                    <td>
                        <asp:TextBox ID="txtElemeCookie" runat="server" TextMode="MultiLine" CssClass="small"></asp:TextBox></td>
                </tr>
            </tbody>
        </table>
    </div>
    
    <div class="foot_btn_box">
    <asp:Button ID="btnSubmit" runat="server" Text="提交保存" CssClass="btnSubmit" onclick="btnSubmit_Click" />
    &nbsp;<input name="重置" type="reset" class="btnSubmit" value="重 置" />
    </div>
</div>
</form>
</body>
</html>
