<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="DTcms.Web.api.payment.mppay.index" %>

<!DOCTYPE html>

<html >
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width,target-densitydpi=medium-dpi,initial-scale=1.0, maximum-scale=1.0,minimum-scale=1.0,user-scalable=no" />
    <title>馍王订单支付</title>
    <script src="jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="https://res.wx.qq.com/open/js/jweixin-1.0.0.js"></script>
    <meta content="yes" name="apple-mobile-web-app-capable" />
    <meta content="black" name="apple-mobile-web-app-status-bar-style" />
    <meta content="telephone=no" name="format-detection" />
    <script type="text/javascript">
        wx.config({
            debug: true, // 开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印。
            appId: '<%=appId%>', // 必填，公众号的唯一标识
            timestamp: <%=jstimestamp%>, // 必填，生成签名的时间戳
            nonceStr: '<%=jsnoncestr%>', // 必填，生成签名的随机串
            signature: '<%=get_mp_signature()%>',// 必填，签名，见附录1
            jsApiList: ['chooseWXPay'] // 必填，需要使用的JS接口列表，所有JS接口列表见附录2
        });    
        wx.error(function(res){
            // config信息验证失败会执行error函数，如签名过期导致验证失败，具体错误信息可以打开config的debug模式查看，
            //也可以在返回的res参数中查看，对于SPA可以在这里更新签名。

        });
        $(function(){
            $('#btn').click(function(){
                wx.chooseWXPay({
                    timestamp: <%=timeStamp%>, // 支付签名时间戳，注意微信jssdk中的所有使用timestamp字段均为小写。但最新版的支付后台生成签名使用的timeStamp字段名需大写其中的S字符
                    nonceStr: '<%=nonceStr%>', // 支付签名随机串，不长于 32 位
                    package: '<%=package%>', // 统一支付接口返回的prepay_id参数值，提交格式如：prepay_id=***）
                    signType: 'MD5', // 签名方式，默认为'SHA1'，使用新版支付需传入'MD5'
                    paySign: '<%=paySign%>', // 支付签名
                    success: function (res) {
                        // 支付成功后的回调函数
                        if (res.err_msg == "get_brand_wcpay_request:ok") {
                            location.href='/mp_payment.aspx?action=succeed&order_type=BuyGoods&order_no=<%=order_no%>&openid=<%=openid%>';
                        }
                    }
                });
            });
            
        });
    </script>   
</head>
<body>
    <form id="form1" runat="server">
        <div class="themeblock listsub">
        <ul>
            <li class="list_title">
                <div class="typelist login_area">
                    <h3 class="h_title">
                        支付金额</h3>
                </div>
            </li>                        
            <li class="list_title">
                <div class="typelist login_area">
                    <span class="txt_gray">支付方式：</span><span >微信支付</span>
                </div>
            </li>
            <li class="list_title">
                <div class="typelist login_area">
                    <span class="txt_gray">金额：</span><span class="price_n" ><%=order_amount %></span>元
                </div>
            </li>
        </ul>
    </div>    

    <div class="login_area_b">
        <input class="add-cart" type="button" value="微信支付" id="btn"/>
    </div>
    </form>
</body>
</html>
