<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="DTcms.Web.admin.index" %>

<%@ Import Namespace="DTcms.Common" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>后台管理</title>
    <link href="../scripts/ui/skins/Aqua/css/ligerui-all.css" rel="stylesheet" type="text/css" />
    <link href="images/style.css" rel="stylesheet" type="text/css" />
    <script src="../scripts/jquery/jquery-1.11.0.min.js" type="text/javascript"></script>
    <script src="../scripts/ui/js/ligerBuild.min.js" type="text/javascript"></script>
    <script src="../scripts/artdialog/dialog-min.js" type="text/javascript"></script>
    <script src="../scripts/artdialog/dialog-plus-min.js" type="text/javascript"></script>
    <link href="../scripts/artdialog/ui-dialog.css" rel="stylesheet" type="text/css" />
    <script src="js/function.js" type="text/javascript"></script>
    <script>
        document.createElement('header');
        document.createElement('nav');
    </script>
    <script type="text/javascript">
        var d;
        $(function () {
            $('nav').on('click', 'li', function () {
                $(this).parent().find('li').removeClass('hover');
                $(this).addClass('hover');
            });
            $('.city_selector .tab_con').hide().eq(0).show();
            $('.city_selector .tab_hd').on('click', 'li', function () {
                var tabs = $('.city_selector .tab_hd li');
                var $this = $(this);
                var _idx = tabs.index(this);
                tabs.removeClass('hover');
                $this.addClass('hover');
                $('.city_selector .tab_bd div').hide().eq(_idx).show();
                var currentLi = $(this);
                $.ajax({
                    type: "post", //提交的类型
                    url: "/tools/admin_ajax.ashx?action=set_area", //提交地址
                    data: { areaid: $(this).data('id') }, //参数
                    dataType: "json",
                    success: function (data) { //回调方法

                    }
                });
            });
            $('.btn_search').on('click', function () {
                if (tab.isTabItemExist('wangshangdingdanfilter')) {
                    $('iframe[name="wangshangdingdanfilter"]').attr('src', 'orders/order_list.aspx?type=0&keyword=' + $('#txtOrderKeyword').val());
                } else {
                    f_addTab('wangshangdingdanfilter', '网上订单查询', 'orders/order_list.aspx?type=0&keyword=' + $('#txtOrderKeyword').val());
                }
                return false;
            });
            
            setInterval(function () {
                $.ajax({
                    type: "post",
                    url: "/tools/admin_ajax.ashx?action=query_unconfirm_sync_order",
                    dataType: "json",
                    success: function (data, textStatus) {
                        if (!data) return;
                        if (data.count > 0) {
                            var follow = $('.side li:first')[0];
                            if (d) {
                                d.close();
                            }
                            d = dialog({
                                align: 'right',
                                content: '有未确认的同步订单' + data.count + '个,请到订单列表确认',
                                quickClose: true// 点击空白处快速关闭
                            });
                            d.show(follow);
                        }

                    },
                    timeout: 20000
                });
            }, 1000 * 30);
        })
    </script>
    <script type="text/javascript">
        var tab = null;
        var accordion = null;
        var tree = null;
        $(function () {
            //页面布局
            $("#global_layout").ligerLayout({ leftWidth: 180, height: '100%', topHeight: 65, bottomHeight: 24, allowTopResize: false, allowBottomResize: false, allowLeftCollapse: true, onHeightChanged: f_heightChanged });

            var height = $(document).height() - 160;

            //Tab
            $("#framecenter").ligerTab({ height: height });

            $(".l-link").hover(function () {
                $(this).addClass("l-link-over");
            }, function () {
                $(this).removeClass("l-link-over");
            });

            //快捷菜单
            var menu = $.ligerMenu({ width: 120, items:
		    [
			    { text: '管理首页', click: itemclick },
			    { text: '修改密码', click: itemclick },
			    { line: true },
			    { text: '关闭菜单', click: itemclick }
		    ]
            });
            $("#tab-tools-nav").bind("click", function () {
                var offset = $(this).offset(); //取得事件对象的位置
                menu.show({ top: offset.top + 27, left: offset.left - 120 });
                return false;
            });

            tab = $("#framecenter").ligerGetTabManager();
            $('.switch').click(function () {
                var status = $('.switch').hasClass('on') ? 'off' : 'on';
                $.ajax({
                    type: "post", //提交的类型
                    url: "/tools/admin_ajax.ashx?action=switch_busy", //提交地址
                    data: { status: status }, //参数
                    dataType: "json",
                    beforeSend: function (XMLHttpRequest) {
                        //发送前动作                        
                    },
                    success: function (data) { //回调方法
                        if (!data) return;
                        if (data.msg == 1) {
                            $('.switch').attr('class', 'switch ' + status);
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert("状态：" + textStatus + "；出错提示：" + errorThrown);
                    },
                    timeout: 20000
                });
            });
            f_addTab('wangshangdingdan', '网上订单', 'orders/order_list.aspx?type=1');
        });

        //快捷菜单回调函数
        function itemclick(item) {
            switch (item.text) {
                case "管理首页":
                    f_addTab('home', '管理中心', 'center.aspx');
                    break;
                case "快捷导航":
                    //调用函数
                    break;
                case "修改密码":
                    f_addTab('manager_pwd', '修改密码', 'manager/manager_pwd.aspx');
                    break;
                default:
                    //关闭窗口
                    break;
            }
        }
        function f_heightChanged(options) {
            if (tab)
                tab.addHeight(options.diff);
            if (accordion && options.middleHeight - 24 > 0)
                accordion.setHeight(options.middleHeight - 24);
        }
        //添加Tab，可传3个参数
        function f_addTab(tabid, text, url, iconcss) {
            if (arguments.length == 4) {
                tab.addTabItem({ tabid: tabid, text: text, url: url, iconcss: iconcss });
            } else {
                tab.addTabItem({ tabid: tabid, text: text, url: url });
            }
        }
        //提示Dialog并关闭Tab
        function f_errorTab(tit, msg) {
            $.ligerDialog.open({
                isDrag: false,
                allowClose: false,
                type: 'error',
                title: tit,
                content: msg,
                buttons: [{
                    text: '确定',
                    onclick: function (item, dialog, index) {
                        //查找当前iframe名称
                        var itemiframe = "#framecenter .l-tab-content .l-tab-content-item";
                        var curriframe = "";
                        $(itemiframe).each(function () {
                            if ($(this).css("display") != "none") {
                                curriframe = $(this).attr("tabid");
                                return false;
                            }
                        });
                        if (curriframe != "") {
                            tab.removeTabItem(curriframe);
                            dialog.close();
                        }
                    }
                }]
            });
        }
    </script>
    <link href="css/common.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        html
        {
            padding: 102px 0 0 0;
        }
        nav ul li a
        {
            display: block;
        }
        header
        {
            position: relative;
            width: 100%;
            height: 65px;
            margin-top: -102px;
            margin-bottom: 10px;
            background: linear-gradient(to bottom, #017fcc, #00a2e6);
            background: -moz-linear-gradient(top, #017fcc, #00a2e6);
            background: -ms-linear-gradient(top, #017fcc, #00a2e6);
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#017fcc', endColorstr='#00a2e6'); /* IE6,IE7 */
            -ms-filter: "progid:DXImageTransform.Microsoft.gradient(startColorstr='#017fcc', endColorstr='#00a2e6')"; /* IE8 */
            position: relative; /*box-sizing:border-box;*/
        }
        header .tab_bd
        {
            color: #fff;
            padding-top: 6px;
            height: 20px;
            padding-left: 12px;
            overflow: hidden;
        }
        
        header .city_selector
        {
            float: left;
            display: inline-block;
            text-align: left;
            padding-top: 10px;
            height: 65px;
            padding-right: 22px;
            padding-left: 100px;
            background: url(img/logo_small.gif) no-repeat;
            background-size: 50px;
        }
        header .city_selector li
        {
            border-radius: 20px;
            background: #bababa;
            color: #fff;
            width: 40px;
            height: 40px;
            margin-right: 6px;
            text-align: center;
            line-height: 40px;
            display: inline-block;
            cursor: pointer;
        }
        header .city_selector li.hover, header .city_selector li:hover
        {
            background: #01b4bb;
        }
        header .city_selector a
        {
            color: white;
            cursor: pointer;
        }
        header .city_selector a.hover
        {
            color: rgb(255, 106, 0);
        }
        header .city_selector a:hover
        {
            color: rgb(255,106,0);
        }
        header .profile
        {
            padding-top: 18px;
            float: right;
            padding-right: 30px;
        }
        header .profile a
        {
            color: #fff;
            text-decoration: none;
        }
        header .profile a:hover
        {
            color: #f5e700;
        }
        .side
        {
            position: relative;
            height: 100%;
            background: #e6e6e6;
            overflow: auto;
            width: 157px;
            float: left;
            margin-right: 0 !important;
            margin-right: -3px;
            overflow: auto;
        }
        .side a
        {
            text-decoration: none;
            color: #000;
        }
        .side li
        {
            text-indent: 1em;
            font-size: 1.2em;
            line-height: 2em;
            background: #e6e6e6;
            border-bottom: 1px solid #bbb;
            cursor: pointer;
        }
        .side li.hover, .side li:hover
        {
            color: #fff;
            background: #00a1e9;
            border-bottom: 1px solid #00a1e9;
        }
        .side li.hover a, .side li:hover a
        {
            color: #fff;
        }
        .main
        {
            position: relative;
            overflow: hidden;
            height: 100%;
            background: #f30;
            top: 92px;
            margin-top: -92px;
            margin-left: 167px;
            _margin-left: 0;
            z-index: 2;
        }
        .main iframe
        {
            height: 100%;
            width: 100%;
            background: #fff;
            position: absolute;
            left: 0;
            top: 0;
        }
    </style>
    <style type="text/css">
        header .toolbar
        {
            float: right;
            position: relative;
            padding-top: 18px;
            color: #fff;
        }
        header .keyword
        {
            border-radius: 8px;
            border: 0;
        }
        header .btn_search
        {
            color: #fff;
            background: #f1ab00;
            border-radius: 8px;
            border: 0;
        }
        header .dropdown
        {
            vertical-align: top;
            display: inline-block;
            height: 20px;
            background: #efefef url(img/arr_down.png) 410px center no-repeat;
            border-radius: 8px;
            padding: 0 8px;
            position: relative;
            z-index: 3;
            width: 416px;
            text-align: left;
        }
        header .dropdown_list li
        {
        }
        header .dropdown_list li:hover
        {
            background: #fff;
        }
        header .dropdown_list
        {
            padding: 10px 8px 10px 8px;
            border-bottom-right-radius: 8px;
            border-bottom-left-radius: 8px;
            width: 100%;
            margin-left: -8px;
            background: #efefef;
            margin-top: -6px;
            text-align: left;
            margin-bottom: -120px;
            height: 120px;
        }
        header .time_from, header .time_to
        {
            display: inline-block;
            background: #efefef;
            padding: 0 6px;
            border-radius: 4px;
        }
        header .switch
        {
            background: url(img/switch_off.png) no-repeat;
            height: 26px;
            width: 51px;
            display: inline-block;
            vertical-align: top;
            cursor: pointer;
        }
        header .switch.on
        {
            background: url(img/switch_on.png) no-repeat;
        }
    </style>
</head>
<body style="padding: 0px;">
    <form id="form1" runat="server">
    <!--头部-->
    <header>
        <div class="city_selector">
            <ul class="tab_hd">
                <asp:Literal runat="server" ID="ltlArea"></asp:Literal>     
            </ul>
            
        </div>        
        <div class="profile">
            <asp:LinkButton ID="lbtnExit" runat="server" onclick="lbtnExit_Click">安全退出</asp:LinkButton>
        </div>
        <div class="toolbar">
            <input type="text" class="keyword" id="txtOrderKeyword"/><button class="btn_search">搜索</button> &nbsp;            
        </div>
    </header>
    <nav class="side">
        <ul>
            <%
                if (IsAdminLevel("orders", DTEnums.ActionEnum.View.ToString()))
            { %>
            <li><a href="javascript:f_addTab('wangshangdingdan','网上订单','orders/order_list.aspx?type=1')">网上订单</a></li>
            <%}
                if (IsAdminLevel("orders", DTEnums.ActionEnum.View.ToString()))
            { %>
            <li><a href="javascript:f_addTab('xianxiadingdan','线下订单','orders/order_list.aspx?type=2')">线下订单</a></li>
             <li><a href="javascript:f_addTab('chongzhiqueren','充值确认','vipUser/confirm_Charge_list.aspx')">充值确认</a></li>
             <li><a href="javascript:f_addTab('xianxiadingdan','充值记录','vipUser/User_ChargeOrder_list.aspx')">充值记录</a></li>
            <%}
                if (IsAdminLevel("caipinguanli", DTEnums.ActionEnum.View.ToString()))
            {%>
            <li><a href="javascript:f_addTab('caipinguanli','菜品管理','goods/list.aspx?channel_id=2')">菜品管理</a></li>
            <%}
                if (IsAdminLevel("caipinguanli", DTEnums.ActionEnum.View.ToString()))
            { %>
            <li><a href="javascript:f_addTab('xiajiacaipinguanli','下架菜品管理','goods/list.aspx?channel_id=2&is_lock=1')">下架菜品管理</a></li>
            <%}

            if (IsAdminLevel("tongjiguanli", DTEnums.ActionEnum.View.ToString()))
            { %>
            <li><a href="javascript:f_addTab('tongjiguanli','统计管理','orders/bi_area_order_list.aspx')">统计管理</a></li>
            <%}
              if (IsAdminLevel("tongjiguanli", DTEnums.ActionEnum.View.ToString()))
            { %>
            <li><a href="javascript:f_addTab('tongjiguanli_map','统计管理折线图','orders/bi_map_area_order_list.aspx')">统计管理折线图</a></li>
            <%}
                if (IsAdminLevel("area", DTEnums.ActionEnum.View.ToString()))
            { %>
            <li><a href="javascript:f_addTab('quyuguanli','区域管理','area/list.aspx')">区域管理</a></li>
            <%}
                if (IsAdminLevel("worker", DTEnums.ActionEnum.View.ToString()))
            { %>
            <li><a href="javascript:f_addTab('quyujirenyuanguanli','区域及人员管理','worker/list.aspx')">区域及人员管理</a></li>
            <%}
            if (IsAdminLevel("dingdanhuishouzhan", DTEnums.ActionEnum.View.ToString()))
            { %>
            <li><a href="javascript:f_addTab('dingdanhuishouzhan','订单回收站','orders/fake_order_list.aspx?type=0')">订单回收站</a></li>
            <%}
            if (IsAdminLevel("houtaizhanghaoguanli", DTEnums.ActionEnum.View.ToString()))
            { %>
            <li><a href="javascript:f_addTab('houtaizhanghaoguanli','后台账号管理','manager/manager_list.aspx')">后台账号管理</a></li>
            <%}
                if (IsAdminLevel("templet_list", DTEnums.ActionEnum.View.ToString()))
            { %>
            <li><a class="l-link" href="javascript:f_addTab('templet_list','系统模板管理','settings/templet_list.aspx')">系统模板管理</a></li>
            <%}
            if (IsAdminLevel("sys_config", DTEnums.ActionEnum.View.ToString()))
            { %>
            <li><a class="l-link" href="javascript:f_addTab('sys_config','系统参数设置','settings/sys_config.aspx')">系统参数设置</a></li>
            <%}
            if (IsAdminLevel("category", DTEnums.ActionEnum.View.ToString()))
            { %>
            <li><a class="l-link" href="javascript:f_addTab('category','类别设置','category/list.aspx?channel_id=2')">类别设置</a></li>
            <%}
                if (IsAdminLevel("category", DTEnums.ActionEnum.View.ToString()))
            { %>
            <li><a class="l-link" href="javascript:f_addTab('virtualcategory','虚拟类别设置','category/list.aspx?channel_id=3')">虚拟类别设置</a></li>
            <%}
                if (IsAdminLevel("mail_template", DTEnums.ActionEnum.View.ToString()))
            { %>
            <li><a class="l-link" href="javascript:f_addTab('mail_template','邮件模板管理','users/mail_template_list.aspx')">邮件模板管理</a></li>
            <%}
                if (IsAdminLevel("user_config", DTEnums.ActionEnum.View.ToString()))
            { %>
            <li><a href="javascript:f_addTab('order_payment','支付方式设置','orders/payment_list.aspx')">支付方式设置</a></li>
            <%}
                if (IsAdminLevel("bf_carnival", DTEnums.ActionEnum.View.ToString()))
            { %>
            <li><a href="javascript:f_addTab('bf_carnival','活动设置','carnival/list.aspx')">活动设置</a></li>
            <%}
              if (IsAdminLevel("bf_condition", DTEnums.ActionEnum.View.ToString()))
            { %>
            <li><a href="javascript:f_addTab('bf_condition','附加条件设置','condition/list.aspx')">附加条件设置</a></li>
            <%}
              if (IsAdminLevel("bf_condition_price", DTEnums.ActionEnum.View.ToString()))
            { %>
            <li><a href="javascript:f_addTab('bf_condition_price','条件(价格)设置','condition_price/list.aspx')">条件(价格)设置</a></li>
            <%}
              if (IsAdminLevel("bf_company", DTEnums.ActionEnum.View.ToString()))
            { %>
            <li><a href="javascript:f_addTab('bf_company','群组管理','company/list.aspx')">群组管理</a></li>
            <%}
            %>
        </ul>
    </nav>
    <div>
        <div position="center" id="framecenter" toolsid="tab-tools-nav" style="overflow: hidden;">
            
        </div>
    </div>
    </form>
</body>
</html>
