<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sys_config.aspx.cs" Inherits="DTcms.Web.admin.settings.sys_config" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>系统参数设置</title>
    <link href="../../scripts/ui/skins/Aqua/css/ligerui-all.css" rel="stylesheet" type="text/css" />
    <link href="../images/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../scripts/jquery/jquery-1.3.2.min.js"></script>
    <script type="text/javascript" src="../../scripts/jquery/jquery.form.js"></script>
    <script type="text/javascript" src="../../scripts/jquery/jquery.validate.min.js"></script>
    <script type="text/javascript" src="../../scripts/jquery/messages_cn.js"></script>
    <script type="text/javascript" src="../../scripts/ui/js/ligerBuild.min.js"></script>
    <script type="text/javascript" src="../js/function.js"></script>
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
        <div class="navigation">首页 &gt; 控制面板 &gt; 系统参数设置</div>
        <div id="contentTab">
            <ul class="tab_nav">
                <li class="selected"><a onclick="tabs('#contentTab',0);" href="javascript:;">网站基本信息</a></li>
                <li><a onclick="tabs('#contentTab',1);" href="javascript:void(0);">功能权限配置</a></li>
                <li><a onclick="tabs('#contentTab',2);" href="javascript:void(0);">邮件发送配置</a></li>
                <li><a onclick="tabs('#contentTab',3);" href="javascript:void(0);">附件配置</a></li>
                <li><a onclick="tabs('#contentTab',4);" href="javascript:void(0);">订餐配置</a></li>
                <li><a onclick="tabs('#contentTab',5);" href="javascript:void(0);">广告配置</a></li>
                <li><a onclick="tabs('#contentTab',6);" href="javascript:void(0);">微信配置</a></li>
                <li><a onclick="tabs('#contentTab',7);" href="javascript:void(0);">模板消息配置</a></li>
                <li><a onclick="tabs('#contentTab',8);" href="javascript:void(0);">提示消息配置</a></li>
                <li><a onclick="tabs('#contentTab',9);" href="javascript:void(0);">VIP卡消息配置</a></li>
            </ul>

            <div class="tab_con" style="display: block;">
                <table class="form_table">
                    <col width="180px">
                    <col>
                    <tbody>
                        <tr>
                            <th>站点名称：</th>
                            <td>
                                <asp:TextBox ID="webname" runat="server" CssClass="txtInput normal required" MaxLength="100"></asp:TextBox><label>*</label></td>
                        </tr>
                        <tr>
                            <th>公司名称：</th>
                            <td>
                                <asp:TextBox ID="webcompany" runat="server" CssClass="txtInput normal required" MaxLength="100"></asp:TextBox><label>*</label></td>
                        </tr>
                        <tr>
                            <th>网站域名：</th>
                            <td>
                                <asp:TextBox ID="weburl" runat="server" CssClass="txtInput normal required url" MaxLength="250"></asp:TextBox><label>*以“http://”开头</label></td>
                        </tr>
                        <tr>
                            <th>联系电话：</th>
                            <td>
                                <asp:TextBox ID="webtel" runat="server" CssClass="txtInput normal" MaxLength="50"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <th>传真号码：</th>
                            <td>
                                <asp:TextBox ID="webfax" runat="server" CssClass="txtInput normal" MaxLength="50"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <th>管理员邮箱：</th>
                            <td>
                                <asp:TextBox ID="webmail" runat="server" CssClass="txtInput normal email" MaxLength="100"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <th>网站备案号：</th>
                            <td>
                                <asp:TextBox ID="webcrod" runat="server" CssClass="txtInput normal" MaxLength="50"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <th>首页标题(SEO)：</th>
                            <td>
                                <asp:TextBox ID="webtitle" runat="server" CssClass="txtInput normal required" MaxLength="250" Style="width: 350px;"></asp:TextBox><label>*自定义的首页标题</label></td>
                        </tr>
                        <tr>
                            <th>页面关健词(SEO)：</th>
                            <td>
                                <asp:TextBox ID="webkeyword" runat="server" CssClass="txtInput" MaxLength="250" Style="width: 350px;"></asp:TextBox>
                                <label>页面关键词(keyword)</label></td>
                        </tr>
                        <tr>
                            <th>页面描述(SEO)：</th>
                            <td>
                                <asp:TextBox ID="webdescription" runat="server" MaxLength="250" TextMode="MultiLine" CssClass="small"></asp:TextBox>
                                <label>页面描述(description)</label></td>
                        </tr>
                        <tr>
                            <th>网站版权信息：</th>
                            <td>
                                <asp:TextBox ID="webcopyright" runat="server" MaxLength="500" TextMode="MultiLine" CssClass="small"></asp:TextBox><label>支持HTML格式</label></td>
                        </tr>
                    </tbody>
                </table>
            </div>

            <div class="tab_con">
                <table class="form_table">
                    <col width="180px">
                    <col>
                    <tbody>
                        <tr>
                            <th>网站安装目录：</th>
                            <td>
                                <asp:TextBox ID="webpath" runat="server" CssClass="txtInput normal required" MaxLength="100">/</asp:TextBox><label>*根目录下，输入“/”；如：http://abc.com/web，输入“web/”</label></td>
                        </tr>
                        <tr>
                            <th>网站管理目录：</th>
                            <td>
                                <asp:TextBox ID="webmanagepath" runat="server" CssClass="txtInput normal required" minlength="2" MaxLength="100">admin</asp:TextBox><label>*默认是admin，如已经更改，请输入目录名</label></td>
                        </tr>
                        <tr>
                            <th>URL重写开关：</th>
                            <td>
                                <asp:RadioButtonList ID="staticstatus" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                    <asp:ListItem Selected="True" Value="0">关闭</asp:ListItem>
                                    <asp:ListItem Value="1">伪URL重写</asp:ListItem>
                                </asp:RadioButtonList>
                                <label>(<a href="url_rewrite_list.aspx">编辑伪静态url替换规则</a>)</label>
                            </td>
                        </tr>
                        <tr>
                            <th>静态URL后缀：</th>
                            <td>
                                <asp:TextBox ID="staticextension" runat="server" CssClass="txtInput small required" minlength="2" MaxLength="100"></asp:TextBox><label>*扩展名，不包括“.”，如：aspx、html</label></td>
                        </tr>
                        <tr>
                            <th>开启会员功能：</th>
                            <td>
                                <asp:RadioButtonList ID="memberstatus" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                    <asp:ListItem Value="0">关闭</asp:ListItem>
                                    <asp:ListItem Selected="True" Value="1">开启</asp:ListItem>
                                </asp:RadioButtonList>
                                <label></label>
                            </td>
                        </tr>
                        <tr>
                            <th>开启评论审核：</th>
                            <td>
                                <asp:RadioButtonList ID="commentstatus" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                    <asp:ListItem Selected="True" Value="0">关闭</asp:ListItem>
                                    <asp:ListItem Value="1">开启</asp:ListItem>
                                </asp:RadioButtonList>
                                <label></label>
                            </td>
                        </tr>
                        <tr>
                            <th>后台管理日志：</th>
                            <td>
                                <asp:RadioButtonList ID="logstatus" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                    <asp:ListItem Selected="True" Value="0">关闭</asp:ListItem>
                                    <asp:ListItem Value="1">开启</asp:ListItem>
                                </asp:RadioButtonList>
                                <label></label>
                            </td>
                        </tr>
                        <tr>
                            <th>是否关闭网站：</th>
                            <td>
                                <asp:RadioButtonList ID="webstatus" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                    <asp:ListItem Value="0">关闭</asp:ListItem>
                                    <asp:ListItem Selected="True" Value="1">开启</asp:ListItem>
                                </asp:RadioButtonList>
                                <label></label>
                            </td>
                        </tr>
                        <tr>
                            <th>关闭原因描述：</th>
                            <td>
                                <asp:TextBox ID="webclosereason" runat="server" MaxLength="500" TextMode="MultiLine" CssClass="small"></asp:TextBox><label>支持HTML格式</label></td>
                        </tr>
                        <tr>
                            <th>网站统计代码：</th>
                            <td>
                                <asp:TextBox ID="webcountcode" runat="server" MaxLength="500" TextMode="MultiLine" CssClass="small"></asp:TextBox><label>支持HTML格式</label></td>
                        </tr>
                    </tbody>
                </table>
            </div>

            <div class="tab_con">
                <table class="form_table">
                    <col width="180px">
                    <col>
                    <tbody>
                        <tr>
                            <th>STMP服务器：</th>
                            <td>
                                <asp:TextBox ID="emailstmp" runat="server" CssClass="txtInput normal required" MaxLength="100"></asp:TextBox><label>*发送邮件的SMTP服务器地址</label></td>
                        </tr>
                        <tr>
                            <th>SMTP端口：</th>
                            <td>
                                <asp:TextBox ID="emailport" runat="server" CssClass="txtInput small required digits" MaxLength="10">25</asp:TextBox><label>*SMTP服务器的端口</label></td>
                        </tr>
                        <tr>
                            <th>发件人地址：</th>
                            <td>
                                <asp:TextBox ID="emailfrom" runat="server" CssClass="txtInput normal required" MaxLength="100"></asp:TextBox><label>*</label></td>
                        </tr>
                        <tr>
                            <th>邮箱账号：</th>
                            <td>
                                <asp:TextBox ID="emailusername" runat="server" CssClass="txtInput normal required" MaxLength="100"></asp:TextBox><label>*</label></td>
                        </tr>
                        <tr>
                            <th>邮箱密码：</th>
                            <td>
                                <asp:TextBox ID="emailpassword" runat="server" CssClass="txtInput normal required" MaxLength="100" TextMode="Password"></asp:TextBox><label>*</label></td>
                        </tr>
                        <tr>
                            <th>发件人昵称：</th>
                            <td>
                                <asp:TextBox ID="emailnickname" runat="server" CssClass="txtInput normal required" MaxLength="100"></asp:TextBox><label>*显示发件人的昵称</label></td>
                        </tr>
                    </tbody>
                </table>
            </div>

            <div class="tab_con">
                <table class="form_table">
                    <col width="180px">
                    <col>
                    <tbody>
                        <tr>
                            <th>附件上传目录：</th>
                            <td>
                                <asp:TextBox ID="attachpath" runat="server" CssClass="txtInput normal required" minlength="2" MaxLength="100">upload</asp:TextBox><label>*上传图片或附件的目录，自动创建在网站根目录下</label></td>
                        </tr>
                        <tr>
                            <th>附件上传类型：</th>
                            <td>
                                <asp:TextBox ID="attachextension" runat="server" CssClass="txtInput normal required" MaxLength="250"></asp:TextBox><label>*以英文的逗号分隔开，如：“jpg,gif,rar”</label></td>
                        </tr>
                        <tr>
                            <th>附件保存方式：</th>
                            <td>
                                <asp:DropDownList ID="attachsave" runat="server" CssClass="select2">
                                    <asp:ListItem Value="1">按年月日每天一个目录</asp:ListItem>
                                    <asp:ListItem Value="2">按年月/日/存入不同目录</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <th>文件上传大小：</th>
                            <td>
                                <asp:TextBox ID="attachfilesize" runat="server" CssClass="txtInput small required number" MaxLength="10"></asp:TextBox>KB<label>*超过设置的文件大小不予上传，0不限制</label></td>
                        </tr>
                        <tr>
                            <th>图片上传大小：</th>
                            <td>
                                <asp:TextBox ID="attachimgsize" runat="server" CssClass="txtInput small required number" MaxLength="10"></asp:TextBox>KB<label>*超过设置的图片大小不予上传，0不限制</label></td>
                        </tr>
                        <tr>
                            <th>图片最大尺寸：</th>
                            <td>
                                <asp:TextBox ID="attachimgmaxheight" runat="server" CssClass="txtInput small2 required digits" MaxLength="10">0</asp:TextBox>×
                    <asp:TextBox ID="attachimgmaxwidth" runat="server" CssClass="txtInput small2 required digits" MaxLength="10">0</asp:TextBox>px
                    <label>*设置图片高和宽，超出自动裁剪，0为不受限制</label>
                            </td>
                        </tr>
                        <tr>
                            <th>生成缩略图大小：</th>
                            <td>
                                <asp:TextBox ID="thumbnailheight" runat="server" CssClass="txtInput small2 required digits" MaxLength="10">0</asp:TextBox>×
                    <asp:TextBox ID="thumbnailwidth" runat="server" CssClass="txtInput small2 required digits" MaxLength="10">0</asp:TextBox>px
                    <label>*图片生成缩略图高和宽，0为不生成</label>
                            </td>
                        </tr>
                        <tr>
                            <th>图片水印类型：</th>
                            <td>
                                <asp:RadioButtonList ID="watermarktype" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                    <asp:ListItem Value="0" Selected="True">关闭水印 </asp:ListItem>
                                    <asp:ListItem Value="1">文字水印 </asp:ListItem>
                                    <asp:ListItem Value="2">图片水印 </asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <th>图片水印位置：</th>
                            <td>
                                <asp:RadioButtonList ID="watermarkposition" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                    <asp:ListItem Value="1">左上 </asp:ListItem>
                                    <asp:ListItem Value="2">中上 </asp:ListItem>
                                    <asp:ListItem Value="3">右上 </asp:ListItem>
                                    <asp:ListItem Value="4">左中 </asp:ListItem>
                                    <asp:ListItem Value="5">居中 </asp:ListItem>
                                    <asp:ListItem Value="6">右中 </asp:ListItem>
                                    <asp:ListItem Value="7">左下 </asp:ListItem>
                                    <asp:ListItem Value="8">中下 </asp:ListItem>
                                    <asp:ListItem Value="9" Selected="True">右下 </asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <th>图片生成质量：</th>
                            <td>
                                <asp:TextBox ID="watermarkimgquality" runat="server" CssClass="txtInput small required digits" MaxLength="3">80</asp:TextBox><label>*只适用于加水印的jpeg格式图片.取值范围 0-100, 0质量最低, 100质量最高, 默认80</label></td>
                        </tr>
                        <tr>
                            <th>图片水印文件：</th>
                            <td>
                                <asp:TextBox ID="watermarkpic" runat="server" CssClass="txtInput normal required" MaxLength="100">watermark.png</asp:TextBox><label>*需存放在站点目录下，如图片不存在将使用文字水印</label></td>
                        </tr>
                        <tr>
                            <th>水印透明度：</th>
                            <td>
                                <asp:TextBox ID="watermarktransparency" runat="server" CssClass="txtInput small required digits" MaxLength="2" max="10">5</asp:TextBox><label>*取值范围1--10 (10为不透明)</label></td>
                        </tr>
                        <tr>
                            <th>水印文字：</th>
                            <td>
                                <asp:TextBox ID="watermarktext" runat="server" CssClass="txtInput normal required" MaxLength="100"></asp:TextBox><label>*文字水印的内容</label></td>
                        </tr>
                        <tr>
                            <th>文字字体：</th>
                            <td>
                                <asp:DropDownList ID="watermarkfont" runat="server" CssClass="select2">
                                    <asp:ListItem Value="Arial">Arial</asp:ListItem>
                                    <asp:ListItem Value="Arial Black">Arial Black</asp:ListItem>
                                    <asp:ListItem Value="Batang">Batang</asp:ListItem>
                                    <asp:ListItem Value="BatangChe">BatangChe</asp:ListItem>
                                    <asp:ListItem Value="Comic Sans MS">Comic Sans MS</asp:ListItem>
                                    <asp:ListItem Value="Courier New">Courier New</asp:ListItem>
                                    <asp:ListItem Value="Dotum">Dotum</asp:ListItem>
                                    <asp:ListItem Value="DotumChe">DotumChe</asp:ListItem>
                                    <asp:ListItem Value="Estrangelo Edessa">Estrangelo Edessa</asp:ListItem>
                                    <asp:ListItem Value="Franklin Gothic Medium">Franklin Gothic Medium</asp:ListItem>
                                    <asp:ListItem Value="Gautami">Gautami</asp:ListItem>
                                    <asp:ListItem Value="Georgia">Georgia</asp:ListItem>
                                    <asp:ListItem Value="Gulim">Gulim</asp:ListItem>
                                    <asp:ListItem Value="GulimChe">GulimChe</asp:ListItem>
                                    <asp:ListItem Value="Gungsuh">Gungsuh</asp:ListItem>
                                    <asp:ListItem Value="GungsuhChe">GungsuhChe</asp:ListItem>
                                    <asp:ListItem Value="Impact">Impact</asp:ListItem>
                                    <asp:ListItem Value="Latha">Latha</asp:ListItem>
                                    <asp:ListItem Value="Lucida Console">Lucida Console</asp:ListItem>
                                    <asp:ListItem Value="Lucida Sans Unicode">Lucida Sans Unicode</asp:ListItem>
                                    <asp:ListItem Value="Mangal">Mangal</asp:ListItem>
                                    <asp:ListItem Value="Marlett">Marlett</asp:ListItem>
                                    <asp:ListItem Value="Microsoft Sans Serif">Microsoft Sans Serif</asp:ListItem>
                                    <asp:ListItem Value="MingLiU">MingLiU</asp:ListItem>
                                    <asp:ListItem Value="MS Gothic">MS Gothic</asp:ListItem>
                                    <asp:ListItem Value="MS Mincho">MS Mincho</asp:ListItem>
                                    <asp:ListItem Value="MS PGothic">MS PGothic</asp:ListItem>
                                    <asp:ListItem Value="MS PMincho">MS PMincho</asp:ListItem>
                                    <asp:ListItem Value="MS UI Gothic">MS UI Gothic</asp:ListItem>
                                    <asp:ListItem Value="MV Boli">MV Boli</asp:ListItem>
                                    <asp:ListItem Value="Palatino Linotype">Palatino Linotype</asp:ListItem>
                                    <asp:ListItem Value="PMingLiU">PMingLiU</asp:ListItem>
                                    <asp:ListItem Value="Raavi">Raavi</asp:ListItem>
                                    <asp:ListItem Value="Shruti">Shruti</asp:ListItem>
                                    <asp:ListItem Value="Sylfaen">Sylfaen</asp:ListItem>
                                    <asp:ListItem Value="Symbol">Symbol</asp:ListItem>
                                    <asp:ListItem Value="Tahoma" Selected="selected">Tahoma</asp:ListItem>
                                    <asp:ListItem Value="Times New Roman">Times New Roman</asp:ListItem>
                                    <asp:ListItem Value="Trebuchet MS">Trebuchet MS</asp:ListItem>
                                    <asp:ListItem Value="Tunga">Tunga</asp:ListItem>
                                    <asp:ListItem Value="Verdana">Verdana</asp:ListItem>
                                    <asp:ListItem Value="Webdings">Webdings</asp:ListItem>
                                    <asp:ListItem Value="Wingdings">Wingdings</asp:ListItem>
                                    <asp:ListItem Value="仿宋_GB2312">仿宋_GB2312</asp:ListItem>
                                    <asp:ListItem Value="宋体">宋体</asp:ListItem>
                                    <asp:ListItem Value="新宋体">新宋体</asp:ListItem>
                                    <asp:ListItem Value="楷体_GB2312">楷体_GB2312</asp:ListItem>
                                    <asp:ListItem Value="黑体">黑体</asp:ListItem>
                                </asp:DropDownList>
                                <asp:TextBox ID="watermarkfontsize" runat="server" CssClass="txtInput small2 required digits" MaxLength="10">12</asp:TextBox>px
                    <label>*文字水印的字体和大小</label>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>

            <div class="tab_con">
                <table class="form_table">
                    <col width="180px">
                    <col>
                    <tbody>
                        <tr>
                            <th>外卖订餐时间：</th>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlStartTime">
                                    <asp:ListItem Text="00:00" Value="00:00"></asp:ListItem>
                                    <asp:ListItem Text="07:00" Value="07:00"></asp:ListItem>
                                    <asp:ListItem Text="07:30" Value="07:30"></asp:ListItem>
                                    <asp:ListItem Text="08:00" Value="08:00"></asp:ListItem>
                                    <asp:ListItem Text="08:30" Value="08:30"></asp:ListItem>
                                    <asp:ListItem Text="09:00" Value="09:00"></asp:ListItem>
                                    <asp:ListItem Text="09:30" Value="09:30"></asp:ListItem>
                                    <asp:ListItem Text="10:00" Value="10:00"></asp:ListItem>
                                    <asp:ListItem Text="10:30" Value="10:30"></asp:ListItem>
                                    <asp:ListItem Text="11:00" Value="11:00"></asp:ListItem>
                                    <asp:ListItem Text="11:30" Value="11:30"></asp:ListItem>
                                    <asp:ListItem Text="12:00" Value="12:00"></asp:ListItem>
                                    <asp:ListItem Text="12:30" Value="12:30"></asp:ListItem>
                                    <asp:ListItem Text="13:00" Value="13:00"></asp:ListItem>
                                    <asp:ListItem Text="13:30" Value="13:30"></asp:ListItem>
                                    <asp:ListItem Text="14:00" Value="14:00"></asp:ListItem>
                                    <asp:ListItem Text="14:30" Value="14:30"></asp:ListItem>
                                    <asp:ListItem Text="15:00" Value="15:00"></asp:ListItem>
                                    <asp:ListItem Text="15:30" Value="15:30"></asp:ListItem>
                                    <asp:ListItem Text="16:00" Value="16:00"></asp:ListItem>
                                    <asp:ListItem Text="16:30" Value="16:30"></asp:ListItem>
                                    <asp:ListItem Text="17:00" Value="17:00"></asp:ListItem>
                                    <asp:ListItem Text="17:30" Value="17:30"></asp:ListItem>
                                    <asp:ListItem Text="18:00" Value="18:00"></asp:ListItem>
                                    <asp:ListItem Text="18:30" Value="18:30"></asp:ListItem>
                                    <asp:ListItem Text="19:00" Value="19:00"></asp:ListItem>
                                    <asp:ListItem Text="19:30" Value="19:30"></asp:ListItem>
                                    <asp:ListItem Text="20:05" Value="20:05"></asp:ListItem>
                                    <asp:ListItem Text="20:30" Value="20:30"></asp:ListItem>
                                    <asp:ListItem Text="21:00" Value="21:00"></asp:ListItem>
                                    <asp:ListItem Text="21:30" Value="21:30"></asp:ListItem>
                                </asp:DropDownList>
                                -
                    <asp:DropDownList runat="server" ID="ddlEndTime">
                        <asp:ListItem Text="07:30" Value="07:30"></asp:ListItem>						
                        <asp:ListItem Text="08:00" Value="08:00"></asp:ListItem>
                        <asp:ListItem Text="08:30" Value="08:30"></asp:ListItem>
                        <asp:ListItem Text="09:00" Value="09:00"></asp:ListItem>
                        <asp:ListItem Text="09:30" Value="09:30"></asp:ListItem>
                        <asp:ListItem Text="10:00" Value="10:00"></asp:ListItem>
                        <asp:ListItem Text="10:30" Value="10:30"></asp:ListItem>
                        <asp:ListItem Text="11:00" Value="11:00"></asp:ListItem>
                        <asp:ListItem Text="11:30" Value="11:30"></asp:ListItem>
                        <asp:ListItem Text="12:00" Value="12:00"></asp:ListItem>
                        <asp:ListItem Text="12:30" Value="12:30"></asp:ListItem>
                        <asp:ListItem Text="13:00" Value="13:00"></asp:ListItem>
                        <asp:ListItem Text="13:30" Value="13:30"></asp:ListItem>
                        <asp:ListItem Text="14:00" Value="14:00"></asp:ListItem>
                        <asp:ListItem Text="14:30" Value="14:30"></asp:ListItem>
                        <asp:ListItem Text="15:00" Value="15:00"></asp:ListItem>
                        <asp:ListItem Text="15:30" Value="15:30"></asp:ListItem>
                        <asp:ListItem Text="16:00" Value="16:00"></asp:ListItem>
                        <asp:ListItem Text="16:30" Value="16:30"></asp:ListItem>
                        <asp:ListItem Text="17:00" Value="17:00"></asp:ListItem>
                        <asp:ListItem Text="17:30" Value="17:30"></asp:ListItem>
                        <asp:ListItem Text="18:00" Value="18:00"></asp:ListItem>
                        <asp:ListItem Text="18:30" Value="18:30"></asp:ListItem>
                        <asp:ListItem Text="19:00" Value="19:00"></asp:ListItem>
                        <asp:ListItem Text="19:30" Value="19:30"></asp:ListItem>
                        <asp:ListItem Text="20:05" Value="20:05"></asp:ListItem>
                        <asp:ListItem Text="20:30" Value="20:30"></asp:ListItem>
                        <asp:ListItem Text="21:00" Value="21:00"></asp:ListItem>
                        <asp:ListItem Text="21:30" Value="21:30"></asp:ListItem>
                        <asp:ListItem Text="23:59" Value="23:59"></asp:ListItem>
                    </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <th>微信堂吃时间：</th>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlStartTimeHere">
                                    <asp:ListItem Text="00:00" Value="00:00"></asp:ListItem>
                                    <asp:ListItem Text="07:00" Value="07:00"></asp:ListItem>
                                    <asp:ListItem Text="07:30" Value="07:30"></asp:ListItem>
									<asp:ListItem Text="07:45" Value="07:45"></asp:ListItem>
                                    <asp:ListItem Text="08:00" Value="08:00"></asp:ListItem>
                                    <asp:ListItem Text="08:30" Value="08:30"></asp:ListItem>
                                    <asp:ListItem Text="09:00" Value="09:00"></asp:ListItem>
                                    <asp:ListItem Text="09:30" Value="09:30"></asp:ListItem>
                                    <asp:ListItem Text="10:00" Value="10:00"></asp:ListItem>
                                    <asp:ListItem Text="10:30" Value="10:30"></asp:ListItem>
                                    <asp:ListItem Text="11:00" Value="11:00"></asp:ListItem>
                                    <asp:ListItem Text="11:30" Value="11:30"></asp:ListItem>
                                    <asp:ListItem Text="12:00" Value="12:00"></asp:ListItem>
                                    <asp:ListItem Text="12:30" Value="12:30"></asp:ListItem>
                                    <asp:ListItem Text="13:00" Value="13:00"></asp:ListItem>
                                    <asp:ListItem Text="13:30" Value="13:30"></asp:ListItem>
                                    <asp:ListItem Text="14:00" Value="14:00"></asp:ListItem>
                                    <asp:ListItem Text="14:30" Value="14:30"></asp:ListItem>
                                    <asp:ListItem Text="15:00" Value="15:00"></asp:ListItem>
                                    <asp:ListItem Text="15:30" Value="15:30"></asp:ListItem>
                                    <asp:ListItem Text="16:00" Value="16:00"></asp:ListItem>
                                    <asp:ListItem Text="16:30" Value="16:30"></asp:ListItem>
                                    <asp:ListItem Text="17:00" Value="17:00"></asp:ListItem>
                                    <asp:ListItem Text="17:30" Value="17:30"></asp:ListItem>
                                    <asp:ListItem Text="18:00" Value="18:00"></asp:ListItem>
                                    <asp:ListItem Text="18:30" Value="18:30"></asp:ListItem>
                                    <asp:ListItem Text="19:00" Value="19:00"></asp:ListItem>
                                    <asp:ListItem Text="19:30" Value="19:30"></asp:ListItem>
                                    <asp:ListItem Text="20:00" Value="20:00"></asp:ListItem>
                                    <asp:ListItem Text="20:30" Value="20:30"></asp:ListItem>
                                    <asp:ListItem Text="21:00" Value="21:00"></asp:ListItem>
                                    <asp:ListItem Text="21:30" Value="21:30"></asp:ListItem>
                                </asp:DropDownList>
                                -
                    <asp:DropDownList runat="server" ID="ddlEndTimeHere">
                        <asp:ListItem Text="07:30" Value="07:30"></asp:ListItem>
						<asp:ListItem Text="07:45" Value="07:45"></asp:ListItem>
                        <asp:ListItem Text="08:00" Value="08:00"></asp:ListItem>
                        <asp:ListItem Text="08:30" Value="08:30"></asp:ListItem>
                        <asp:ListItem Text="09:00" Value="09:00"></asp:ListItem>
                        <asp:ListItem Text="09:30" Value="09:30"></asp:ListItem>
                        <asp:ListItem Text="10:00" Value="10:00"></asp:ListItem>
                        <asp:ListItem Text="10:30" Value="10:30"></asp:ListItem>
                        <asp:ListItem Text="11:00" Value="11:00"></asp:ListItem>
                        <asp:ListItem Text="11:30" Value="11:30"></asp:ListItem>
                        <asp:ListItem Text="12:00" Value="12:00"></asp:ListItem>
                        <asp:ListItem Text="12:30" Value="12:30"></asp:ListItem>
                        <asp:ListItem Text="13:00" Value="13:00"></asp:ListItem>
                        <asp:ListItem Text="13:30" Value="13:30"></asp:ListItem>
                        <asp:ListItem Text="14:00" Value="14:00"></asp:ListItem>
                        <asp:ListItem Text="14:30" Value="14:30"></asp:ListItem>
                        <asp:ListItem Text="15:00" Value="15:00"></asp:ListItem>
                        <asp:ListItem Text="15:30" Value="15:30"></asp:ListItem>
                        <asp:ListItem Text="16:00" Value="16:00"></asp:ListItem>
                        <asp:ListItem Text="16:30" Value="16:30"></asp:ListItem>
                        <asp:ListItem Text="17:00" Value="17:00"></asp:ListItem>
                        <asp:ListItem Text="17:30" Value="17:30"></asp:ListItem>
                        <asp:ListItem Text="18:00" Value="18:00"></asp:ListItem>
                        <asp:ListItem Text="18:30" Value="18:30"></asp:ListItem>
                        <asp:ListItem Text="19:00" Value="19:00"></asp:ListItem>
                        <asp:ListItem Text="19:30" Value="19:30"></asp:ListItem>
                        <asp:ListItem Text="20:00" Value="20:00"></asp:ListItem>
                        <asp:ListItem Text="20:30" Value="20:30"></asp:ListItem>
                        <asp:ListItem Text="21:00" Value="21:00"></asp:ListItem>
                        <asp:ListItem Text="21:30" Value="21:30"></asp:ListItem>
                        <asp:ListItem Text="22:00" Value="22:00"></asp:ListItem>
                        <asp:ListItem Text="22:30" Value="22:30"></asp:ListItem>
                        <asp:ListItem Text="23:59" Value="23:59"></asp:ListItem>
                    </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <th>起送金额：</th>
                            <td>
                                <asp:TextBox ID="txtLowAmount" runat="server" CssClass="txtInput small required number" MaxLength="5">0</asp:TextBox>
                                <label>*起送金额</label></td>
                        </tr>
                        <tr>
                            <th>外圈起送金额：</th>
                            <td>
                                <asp:TextBox ID="txtLowAmount_2" runat="server" CssClass="txtInput small required number" MaxLength="5">0</asp:TextBox>
                                <label>*外圈起送金额</label></td>
                        </tr>
                        <tr>
                            <th>包邮金额：</th>
                            <td>
                                <asp:TextBox ID="txtFreeDisAmount" runat="server" CssClass="txtInput small required number" MaxLength="5">0</asp:TextBox>
                                <label>*包邮金额</label></td>
                        </tr>
                        <tr>
                            <th>外送费：</th>
                            <td>
                                <asp:TextBox ID="txtDisAmount" runat="server" CssClass="txtInput small required number" MaxLength="5">0</asp:TextBox>
                                <label>*外送费</label></td>
                        </tr>
                        <tr>
                            <th>已支付订餐账号：</th>
                            <td>
                                <asp:TextBox ID="txtTaoDianDianAccount" runat="server" CssClass="txtInput normal" MaxLength="100"></asp:TextBox>
                                <label>*已支付订餐账号。</label></td>
                        </tr>
                        <tr>
                            <th>权限订餐账号：</th>
                            <td>
                                <asp:TextBox ID="txtIsNullCartAccount" runat="server" CssClass="txtInput normal" MaxLength="100"></asp:TextBox>
                                <label>*通过该账号订餐不受起送费和配送费控制，不受购物车非空限制。</label></td>
                        </tr>
                        <tr>
                            <th>补单预判时间：</th>
                            <td>
                                <asp:TextBox ID="txtAdditional" runat="server" CssClass="txtInput small required digits" MaxLength="3">0</asp:TextBox>
                                <label>*该分钟数内可以进入补单流程。</label>
                            </td>
                        </tr>
                        <tr>
                            <th>补单强制断开时间：</th>
                            <td>
                                <asp:TextBox ID="txtAdditionalForce" runat="server" CssClass="txtInput small required digits" MaxLength="100">0</asp:TextBox>
                                <label>*该分钟数内，处于补单流程内的订单可以提交。</label>
                            </td>
                        </tr>
                        <tr>
                            <th>微信货到付款金额：</th>
                            <td>
                                <asp:TextBox ID="txtDeliverPayMaxAmountForMp" runat="server" CssClass="txtInput small required number" MaxLength="5">0</asp:TextBox>
                                <label>*订单总金额高于该值时可以使用货到付款，0为不限制。</label></td>
                        </tr>
                        <tr>
                            <th>网页货到付款金额：</th>
                            <td>
                                <asp:TextBox ID="txtDeliverPayMaxAmountForWeb" runat="server" CssClass="txtInput small required number" MaxLength="5">0</asp:TextBox>
                                <label>*订单总金额低于该值时可以使用货到付款，0为不限制。</label></td>
                        </tr>
                        <tr>
                            <th>饿了么自动下单：</th>
                            <td>
                                <asp:RadioButtonList ID="rblThirdOrder" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                    <asp:ListItem Value="0">关闭</asp:ListItem>
                                    <asp:ListItem Selected="True" Value="1">开启</asp:ListItem>
                                </asp:RadioButtonList>
                                <label></label>
                            </td>
                        </tr>
                        <tr>
                            <th>天工收银：</th>
                            <td>
                                <asp:RadioButtonList ID="rblTigoon" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                    <asp:ListItem Value="0">关闭</asp:ListItem>
                                    <asp:ListItem Selected="True" Value="1">开启</asp:ListItem>
                                </asp:RadioButtonList>
                                <label></label>
                            </td>
                        </tr>
                        <tr>
                            <th>饿了么自动接单：</th>
                            <td>
                                <asp:RadioButtonList ID="rblSyncOrderDownload" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                    <asp:ListItem Value="0">关闭</asp:ListItem>
                                    <asp:ListItem Selected="True" Value="1">开启</asp:ListItem>
                                </asp:RadioButtonList>
                                <label>直接下载订单</label>
                            </td>
                        </tr>
                        <tr>
                            <th>群组注册时赠送15元：</th>
                            <td>
                                <asp:RadioButtonList ID="rblSendVoucher" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                    <asp:ListItem Value="0">关闭</asp:ListItem>
                                    <asp:ListItem Selected="True" Value="1">开启</asp:ListItem>
                                </asp:RadioButtonList>
                                <label></label>
                            </td>
                        </tr>
                        <tr>
                            <th>美团Cookie：</th>
                            <td>
                                <asp:TextBox ID="txtMeituanCookie" runat="server" TextMode="MultiLine" CssClass="small"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <th>百度Cookie：</th>
                            <td>
                                <asp:TextBox ID="txtBaiduCookie" runat="server" TextMode="MultiLine" CssClass="small"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <th>饿了么Cookie：</th>
                            <td>
                                <asp:TextBox ID="txtElemeCookie" runat="server" TextMode="MultiLine" CssClass="small"></asp:TextBox></td>
                        </tr>
                    </tbody>
                </table>
            </div>

            <div class="tab_con">
                <table class="form_table">
                    <col width="180px">
                    <col>
                    <tbody>
                        <tr>
                            <th>图片1：</th>
                            <td>
                                <asp:TextBox ID="txtImgUrl1" runat="server" CssClass="txtInput normal left" MaxLength="255"></asp:TextBox>
                                <a href="javascript:;" class="files">
                                    <input type="file" id="FileUpload1" name="FileUpload1" onchange="Upload('SingleFile', 'txtImgUrl1', 'FileUpload1');" /></a>
                                <span class="uploading">正在上传，请稍候...</span>
                            </td>
                        </tr>
                        <tr>
                            <th>图片2：</th>
                            <td>
                                <asp:TextBox ID="txtImgUrl2" runat="server" CssClass="txtInput normal left" MaxLength="255"></asp:TextBox>
                                <a href="javascript:;" class="files">
                                    <input type="file" id="FileUpload2" name="FileUpload2" onchange="Upload('SingleFile', 'txtImgUrl2', 'FileUpload2');" /></a>
                                <span class="uploading">正在上传，请稍候...</span>
                            </td>
                        </tr>
                        <tr>
                            <th>图片3：</th>
                            <td>
                                <asp:TextBox ID="txtImgUrl3" runat="server" CssClass="txtInput normal left" MaxLength="255"></asp:TextBox>
                                <a href="javascript:;" class="files">
                                    <input type="file" id="FileUpload3" name="FileUpload3" onchange="Upload('SingleFile', 'txtImgUrl3', 'FileUpload3');" /></a>
                                <span class="uploading">正在上传，请稍候...</span>
                            </td>
                        </tr>
                        <tr>
                            <th>图片4：</th>
                            <td>
                                <asp:TextBox ID="txtImgUrl4" runat="server" CssClass="txtInput normal left" MaxLength="255"></asp:TextBox>
                                <a href="javascript:;" class="files">
                                    <input type="file" id="FileUpload4" name="FileUpload4" onchange="Upload('SingleFile', 'txtImgUrl4', 'FileUpload4');" /></a>
                                <span class="uploading">正在上传，请稍候...</span>
                            </td>
                        </tr>
                        <tr>
                            <th>图片5：</th>
                            <td>
                                <asp:TextBox ID="txtImgUrl5" runat="server" CssClass="txtInput normal left" MaxLength="255"></asp:TextBox>
                                <a href="javascript:;" class="files">
                                    <input type="file" id="FileUpload5" name="FileUpload5" onchange="Upload('SingleFile', 'txtImgUrl5', 'FileUpload5');" /></a>
                                <span class="uploading">正在上传，请稍候...</span>
                            </td>
                        </tr>
                        <tr>
                            <th>图片6：</th>
                            <td>
                                <asp:TextBox ID="txtImgUrl6" runat="server" CssClass="txtInput normal left" MaxLength="255"></asp:TextBox>
                                <a href="javascript:;" class="files">
                                    <input type="file" id="FileUpload6" name="FileUpload6" onchange="Upload('SingleFile', 'txtImgUrl6', 'FileUpload6');" /></a>
                                <span class="uploading">正在上传，请稍候...</span>
                            </td>
                        </tr>
                        <tr>
                            <th>图片7：</th>
                            <td>
                                <asp:TextBox ID="txtImgUrl7" runat="server" CssClass="txtInput normal left" MaxLength="255"></asp:TextBox>
                                <a href="javascript:;" class="files">
                                    <input type="file" id="FileUpload7" name="FileUpload7" onchange="Upload('SingleFile', 'txtImgUrl7', 'FileUpload7');" /></a>
                                <span class="uploading">正在上传，请稍候...</span>
                            </td>
                        </tr>
                        <tr>
                            <th>图片8：</th>
                            <td>
                                <asp:TextBox ID="txtImgUrl8" runat="server" CssClass="txtInput normal left" MaxLength="255"></asp:TextBox>
                                <a href="javascript:;" class="files">
                                    <input type="file" id="FileUpload8" name="FileUpload8" onchange="Upload('SingleFile', 'txtImgUrl8', 'FileUpload8');" /></a>
                                <span class="uploading">正在上传，请稍候...</span>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div class="tab_con">
                <table class="form_table">
                    <col width="180px">
                    <col>
                    <tbody>
                        <tr>
                            <th>关注欢迎标题：</th>
                            <td>
                                <asp:TextBox ID="txtMpWelcomeTitle" runat="server" CssClass="txtInput normal" MaxLength="50"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <th>关注欢迎内容：</th>
                            <td>
                                <asp:TextBox ID="txtMpWelcomeContent" runat="server" MaxLength="250" TextMode="MultiLine" CssClass="small"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>关注欢迎图片：</th>
                            <td>
                                <asp:TextBox ID="txtMpWelcomeImage" runat="server" CssClass="txtInput normal left" MaxLength="255"></asp:TextBox>
                                <a href="javascript:;" class="files">
                                    <input type="file" id="FileUpload9" name="FileUpload9" onchange="Upload('SingleFile', 'txtMpWelcomeImage', 'FileUpload9');" /></a>
                                <span class="uploading">正在上传，请稍候...</span>
                            </td>
                        </tr>
                        <tr>
                            <th>微信区域选择背景图片：</th>
                            <td>
                                <asp:TextBox ID="txtMpBackgroundImage" runat="server" CssClass="txtInput normal left" MaxLength="255"></asp:TextBox>
                                <a href="javascript:;" class="files">
                                    <input type="file" id="FileUpload10" name="FileUpload10" onchange="Upload('SingleFile', 'txtMpBackgroundImage', 'FileUpload10');" /></a>
                                <span class="uploading">正在上传，请稍候...</span>
                            </td>
                        </tr>
                        <tr>
                            <th>堂吃区域选择背景图片：</th>
                            <td>
                                <asp:TextBox ID="txtMpBackgroundImage2" runat="server" CssClass="txtInput normal left" MaxLength="255"></asp:TextBox>
                                <a href="javascript:;" class="files">
                                    <input type="file" id="FileUpload11" name="FileUpload11" onchange="Upload('SingleFile', 'txtMpBackgroundImage2', 'FileUpload11');" /></a>
                                <span class="uploading">正在上传，请稍候...</span>
                            </td>
                        </tr>
                        <tr>
                            <th>AppId：</th>
                            <td>
                                <asp:TextBox ID="txtAppId" runat="server" CssClass="txtInput normal" MaxLength="50"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <th>AppSecret：</th>
                            <td>
                                <asp:TextBox ID="txtAppSecret" runat="server" CssClass="txtInput normal" MaxLength="100"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <th>AESKey：</th>
                            <td>
                                <asp:TextBox ID="txtAESKey" runat="server" CssClass="txtInput normal" MaxLength="100"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <th>菜单：</th>
                            <td>
                                <asp:TextBox ID="txtMenu" runat="server" MaxLength="500" TextMode="MultiLine" CssClass="small"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>副AppId：</th>
                            <td>
                                <asp:TextBox ID="txtSlaveAppId" runat="server" CssClass="txtInput normal" MaxLength="50"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <th>副AppSecret：</th>
                            <td>
                                <asp:TextBox ID="txtSlaveAppSecret" runat="server" CssClass="txtInput normal" MaxLength="100"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <th>副AESKey：</th>
                            <td>
                                <asp:TextBox ID="txtSlaveAESKey" runat="server" CssClass="txtInput normal" MaxLength="100"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <th>副菜单：</th>
                            <td>
                                <asp:TextBox ID="txtSlaveMenu" runat="server" MaxLength="500" TextMode="MultiLine" CssClass="small"></asp:TextBox>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div class="tab_con">
                <table class="form_table">
                    <col width="180px" />
                    <tbody>
                        <tr>
                            <th>模板消息下单通知标题：</th>
                            <td>
                                <asp:TextBox ID="txt_mp_temp_submitorder_first" runat="server" CssClass="txtInput normal" MaxLength="30"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>模板消息下单通知备注：</th>
                            <td>
                                <asp:TextBox ID="txt_mp_temp_submitorder_remark" runat="server" CssClass="txtInput normal" MaxLength="30"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>模板消息外卖派送标题：</th>
                            <td>
                                <asp:TextBox ID="txt_mp_temp_distribution_takeout_first" runat="server" CssClass="txtInput normal" MaxLength="30"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>模板消息外卖派送备注：</th>
                            <td>
                                <asp:TextBox ID="txt_mp_temp_distribution_takeout_remark" runat="server" CssClass="txtInput normal" MaxLength="30"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>模板消息外卖派送送达时间：</th>
                            <td>
                                <asp:TextBox ID="txt_mp_temp_distribution_takeout_keyword5" runat="server" CssClass="txtInput normal" MaxLength="30"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>模板消息超范围标题：</th>
                            <td>
                                <asp:TextBox ID="txt_mp_temp_range_out_first" runat="server" CssClass="txtInput normal" MaxLength="30"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>模板消息超范围备注：</th>
                            <td>
                                <asp:TextBox ID="txt_mp_temp_range_out_remark" runat="server" CssClass="txtInput normal" MaxLength="30"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>模板消息区域错误标题：</th>
                            <td>
                                <asp:TextBox ID="txt_mp_temp_error_area_first" runat="server" CssClass="txtInput normal" MaxLength="30"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>模板消息区域错误备注：</th>
                            <td>
                                <asp:TextBox ID="txt_mp_temp_error_area_remark" runat="server" CssClass="txtInput normal" MaxLength="30"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>模板消息条件无法满足标题：</th>
                            <td>
                                <asp:TextBox ID="txt_mp_temp_much_condition_first" runat="server" CssClass="txtInput normal" MaxLength="30"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>模板消息条件无法满足备注：</th>
                            <td>
                                <asp:TextBox ID="txt_mp_temp_much_condition_remark" runat="server" CssClass="txtInput normal" MaxLength="30"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>模板消息呼叫取餐标题：</th>
                            <td>
                                <asp:TextBox ID="txt_mp_temp_call_takeout_first" runat="server" CssClass="txtInput normal" MaxLength="30"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>模板消息呼叫取餐备注：</th>
                            <td>
                                <asp:TextBox ID="txt_mp_temp_call_takeout_remark" runat="server" CssClass="txtInput normal" MaxLength="30"></asp:TextBox>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div class="tab_con">
                <table class="form_table">
                    <col width="180px" />
                    <tbody>
                        <tr>
                            <th>不满足起送费：</th>
                            <td>
                                <asp:TextBox ID="txtAlarmMessageLowAmount" runat="server" CssClass="txtInput normal" MaxLength="100"></asp:TextBox>
                                <label>*金额处使用{0}通配符代替</label>
                            </td>
                        </tr>

                    </tbody>
                </table>
            </div>
            <div class="tab_con">
                <table class="form_table">
                    <col width="180px" />
                    <tbody>
                        <tr>
                            <th>启用微信端外卖的VIP功能：</th>
                            <td><asp:CheckBox runat="server" ID="cboenable_waimai_vip" />是否启用微信外卖群组的VIP满59减5元功能</td>
                        </tr>
                        <tr>
                            <th>扫码关注VIP二维码成功时的模板消息-标题：</th>
                            <td>
                                <asp:TextBox ID="txtVipJoinWelcomeTitle" runat="server" CssClass="txtInput normal" MaxLength="100"></asp:TextBox>
                                <label>*扫码关注VIP二维码成功时的模板消息标题</label>
                            </td>
                        </tr>
                        <tr>
                            <th>扫码关注VIP二维码成功时的模板消息-商家类型：</th>
                            <td>
                                <asp:TextBox ID="txtvip_join_welcome_name" runat="server" CssClass="txtInput normal" MaxLength="100"></asp:TextBox>
                                <label>*也使用在申请通过时的商家类型</label>
                            </td>
                        </tr>
                        <tr>
                            <th>扫码关注VIP二维码成功时的模板消息-商家地址：</th>
                            <td>
                                <asp:TextBox ID="txtvip_join_welcome_companyname" runat="server" CssClass="txtInput normal" MaxLength="100"></asp:TextBox>
                                <label>*也使用在申请通过时的商家地址</label>
                            </td>
                        </tr>
                        <tr>
                            <th>扫码关注VIP二维码成功时的模板消息-页脚信息：</th>
                            <td>
                                <asp:TextBox ID="txtvip_join_welcome_footer" runat="server" CssClass="txtInput normal" MaxLength="100"></asp:TextBox>
                                <label>*也使用在申请通过时的页脚信息</label>
                            </td>
                        </tr>
                        <tr>
                            <th>每增加一人时的模板消息-标题：</th>
                            <td>
                                <asp:TextBox ID="txtvip_enough_title" runat="server" CssClass="txtInput normal" MaxLength="100"></asp:TextBox>
                                <label>*满30人时的模板消息-标题</label>
                            </td>
                        </tr>
                        <tr>
                            <th>每增加一人时的模板消息-页脚：</th>
                            <td>
                                <asp:TextBox ID="txtvip_enough_footer" runat="server" CssClass="txtInput normal" MaxLength="100"></asp:TextBox>
                                <label>*满30人时的模板消息-页脚</label>
                            </td>
                        </tr>
                        <tr>
                            <th>通过申请时的模板消息-标题：</th>
                            <td>
                                <asp:TextBox ID="txtvip_join_approve_title" runat="server" CssClass="txtInput normal" MaxLength="100"></asp:TextBox>
                                <label>*通过申请时的模板消息-标题</label>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div class="foot_btn_box">
                <asp:Button ID="btnSubmit" runat="server" Text="提交保存" CssClass="btnSubmit" OnClick="btnSubmit_Click" />
                &nbsp;
    <asp:Button ID="btnPublish" runat="server" Text="发布主菜单" CssClass="btnSubmit" OnClick="btnPublish_Click" />
                &nbsp;
    <input name="重置" type="reset" class="btnSubmit" value="重 置" />
            </div>
        </div>
    </form>
</body>
</html>
