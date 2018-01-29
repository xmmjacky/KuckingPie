var isInArea = false;
var isShowDeliver = true;
var isShowConfirmByOnline = false;
$(function () {
    $('.online .area_select li').on('click', function () {
        $('.online .area_select li').removeClass('active');
        $(this).addClass('active');
        $('.online .area_confirm').show();
        $('.online .area_detail').html($(this).data('content'));
    });
    $('.online .confirm_div').on('click', function () {
        if ($('.online .area_select .active').length == 0) return;
        $('#hfAreaId').val($('.area_select .active').data('id'));
        $('.button_order').text($('.area_select .active').text());
        $('#divArea').hide();
        $('.menu li').eq(0).trigger('click');
        $('body').attr('style', 'background-color: white;');
        var isrun = $('#hfIsRun').val();
        if (isrun == 'false') {
            $('#divGoods').show();
            $('.bottom_nav').show();
            $('#divShopClose').show();
            $('#divBottomBtn').hide();
            return;
        }
        ShowLocker();
        //判断下架和繁忙
        $.ajax({
            type: "post",
            url: "/tools/submit_ajax.ashx?action=get_area_type",
            data: {
                id: $('.area_select .active').data('id')
            },
            dataType: "json",
            beforeSend: function (XMLHttpRequest) {
                //发送前动作
            },
            success: function (data, textStatus) {
                ShowLocker()
                if (!data) return;
                if (data.busy == 1) {
                    $('.order_complete_window.late').show();
                    $('#divGoods').show();
                    $('.bottom_nav').show();
                }
                if (data.lock == 1) {
                    $('.order_complete_window.arealock').show();
                    $('#divGoods').show();
                    $('#divAreaClose').show();
                    $('#divBottomBtn').remove();
                    $('.bottom_nav').show();
                } else {
                    $('.bottom_nav').show();
                    $('#divGoods').show();
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                ShowLocker()
                alert("状态：" + textStatus + "；出错提示：" + errorThrown);
            },
            timeout: 50000
        });
    });
    $('.menu li').on('click', function () {
        $('.menu li').removeClass('active');
        $(this).addClass('active');
        var categoryid = $(this).data('id');
        $.ajax({
            type: "post", //提交的类型
            url: "/tools/submit_ajax.ashx?action=mp_getgoodslist", //提交地址
            data: {
                categoryid: categoryid,
                areaid: $('#hfAreaId').val()
            }, //参数
            dataType: "html",
            beforeSend: function (XMLHttpRequest) {
                //发送前动作

            },
            success: function (data) { //回调方法
                $('.suits').empty();
                if (!data) return false;
                $(data).appendTo($('.suits'));
                setTimeout(function () {
                    setDivGoodsHeight(1);
                }, 500);

                $('.suit li').each(function (index) {
                    if ($(this).find('.item.taste').length == 0 && $(this).find('.item .sold').length == 0) {
                        $(this).find('.item.one').on('click', function () {
                            oneOrder($(this));
                        });
                    }
                    $(this).find('.combo').on('click', function () {
                        if ($(this).parent().find('.hide').length) {
                            //console.log($(this).parent().parent().index());
                            $('.suits .title').removeClass('edit');
                            $(this).parent().parent().find('.title').addClass('edit');
                            $(this).parent().addClass('clicked');
                            $('.page_mask').show();
                            $('.suit li').find('.item:not(.selected)').addClass('hide');
                            $(this).parent().find('.hide').removeClass('hide');
                            if ($(this).parent().find('.combotastearea[data-id="' + $(this).parent().find('.selected').data('id') + '"]').length) {
                                $(this).parent().find('.combotastearea[data-id="' + $(this).parent().find('.selected').data('id') + '"]').show();
                                $(this).parent().find('.comboconfirm').show();
                            }


                        } else {
                            if ($(this).parent().find('.combotastearea[data-id="' + $(this).data('id') + '"]').length) {
                                $(this).parent().find('.combotastearea').hide();
                                $(this).parent().find('.comboconfirm').hide();
                                $(this).parent().find('.combotastearea[data-id="' + $(this).data('id') + '"]').show();
                                $(this).parent().find('.comboconfirm').show();
                                $(this).parent().find('.combo').removeClass('selected');
                                $(this).addClass('selected');
                            } else {
                                $(this).parent().find('.combotastearea').hide();
                                $(this).parent().find('.comboconfirm').hide();
                                $(this).parent().removeClass('clicked');
                                $('.page_mask').hide();
                                $(this).parent().find('.selected').removeClass('selected');
                                $(this).addClass('selected');
                                $(this).parent().find('.item:not(.selected)').addClass('hide');
                                var combofee = 0;
                                $(this).parent().parent().find('.selected').each(function () {
                                    combofee += parseFloat($(this).data('price'));
                                });
                                $(this).parent().parent().find('.title .price').html(combofee.toString().replace('.00', ''));
                                //$(this).insertBefore($(this).parent().find('.item:first'));
                            }
                            //$(this).parent().removeClass('clicked');
                            //$('.page_mask').hide();
                            //$(this).parent().find('.selected').removeClass('selected');
                            //$(this).addClass('selected');
                            //$(this).parent().find('.item:not(.selected)').addClass('hide');
                            //var combofee = 0;
                            //$(this).parent().parent().find('.selected').each(function () {
                            //    combofee += parseFloat($(this).data('price'));
                            //});
                            //$(this).parent().parent().find('.title .price').html(combofee.toString().replace('.00', ''));
                            //$(this).insertBefore($(this).parent().find('.item:first'));
                        }
                        setDivGoodsHeight(1);
                    });
                    $(this).find('.comboconfirm').on('click', function () {
                        var tastearea = $(this).parent().find('.combotastearea:visible');
                        var combo = $(this).parent().find('.combo[data-id="' + tastearea.data('id') + '"]');
                        $(this).parent().find('.combotastearea').hide();
                        $(this).parent().find('.comboconfirm').hide();
                        $(this).parent().removeClass('clicked');
                        $('.page_mask').hide();
                        $(this).parent().find('.selected').removeClass('selected');
                        combo.addClass('selected');
                        $(this).parent().find('.item:not(.selected)').addClass('hide');
                        var combofee = 0;
                        $(this).parent().parent().find('.selected').each(function () {
                            combofee += parseFloat($(this).data('price'));
                        });
                        $(this).parent().parent().find('.title .price').html(combofee.toString().replace('.00', ''));
                        //$(this).insertBefore($(this).parent().find('.item:first'));

                        var _taste = '';
                        tastearea.find('.taste.active').each(function () {
                            _taste += $(this).data('id') + ',';
                        });
                        combo.find('.taste').html(_taste);
                        setDivGoodsHeight(1);
                    });
                    $(this).find('.one').on('click', function () {
                        if ($(this).parent().find('.hide').length) {
                            $(this).parent().addClass('clicked');
                            $('.page_mask').show();
                            $('.suit li').find('.item:not(.selected)').addClass('hide');
                            $(this).parent().find('.hide').removeClass('hide');
                        } else {
                            $(this).parent().removeClass('clicked');
                            $('.page_mask').hide();
                            $(this).parent().find('.item:not(.selected)').addClass('hide');
                        }
                        setDivGoodsHeight(1);
                    });
                    if ($(this).find('.item .sold').length == 0) {
                        $(this).find('.confirm').on('click', function () {
                            $(this).parent().removeClass('clicked');
                            $(this).parent().find('.item:not(.selected)').addClass('hide');
                            $('.page_mask').hide();
                            var _taste = '';
                            $(this).parent().find('.taste.active').each(function () {
                                _taste += $(this).data('id') + ',';
                            });
                            $(this).parent().find('.one .taste').html(_taste);
                            setDivGoodsHeight(1);
                            oneOrder($(this).parent().find('.one'));
                            $(this).parent().find('.taste.active').removeClass('active');
                        });
                    }
                    $(this).find('.tastearea>.taste,.combotastearea>.taste ').on('click', function () {
                        if ($(this).hasClass('active')) {
                            $(this).removeClass('active');
                        } else {
                            if ($(this).parent().find('.active').length < 3) {
                                $(this).addClass('active');
                            }

                        }

                        //$(this).parent().removeClass('clicked');
                        //$('.page_mask').hide();
                        //$(this).parent().find('.one .taste').html($(this).data('id'));
                        //$(this).parent().find('.item:not(.selected)').addClass('hide');
                        //$(this).parent().find('.active').removeClass('active');
                        //$(this).addClass('active');
                        //setDivGoodsHeight(1);
                        //oneOrder($(this).parent().find('.one'));
                    });
                });
                return false;
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("状态：" + textStatus + "；出错提示：" + errorThrown);
            },
            timeout: 50000
        });
    });
    $('#divCartOnline .b_submit').on('click', function () {
        if (isShowDeliver) {
            if ($('#divCartOnline .b_pay_mppay:hidden').length) {
                $(this).parent().find('div:hidden').show();
            } else {
                $(this).parent().find('div:not(:last)').hide();
            }
        } else {
            if ($('#divCartOnline .b_pay_mppay:hidden').length) {
                $('#divCartOnline .b_pay_mppay').show();
            } else {
                $('#divCartOnline .b_pay_mppay').hide();
            }
        }

    });
    $('#btnOfflineSubmit ').on('click', function () {
        if ($(this).parent().find('div:hidden').length) {
            $(this).parent().find('div:hidden').show();
            $(this).parent().prev().find('div:hidden').show();
            $('#btnOfflineSubmit font').text('立即支付');
        } else {
            if ($('#divCarnivalOffline .item .unselect.cover').length) {
                var cart = new Object();
                if ($('#hfCookie').val() != "") {
                    cart = $.parseJSON($('#hfCookie').val());
                    var goodsid = $('#divCarnivalOffline .item .unselect.cover').parent().data('id');
                    var cartgoodsid = goodsid + "†discount†";
                    var isInCart = false;
                    $.each(cart, function (key, val) {
                        if (key.indexOf(cartgoodsid) > -1) {
                            isInCart = true;
                            return false;
                        }
                    });
                }
                if (!isInCart && $('#hfCookie').val() != "") {
                    $('#btnCarnivalOffline').trigger('click');
                    //alert('优惠产品请加入购物车');
                    //return;
                }
            }
            //判断选择状态并发起微信支付
            //if ($('#btnOfflineInCompany .b_yes:visible').length == 0 && $('#btnOfflineOutCompany .b_yes:visible').length == 0) {
            //    alert('请选择已到店或未到店');
            //    return;
            //}
            var remark = "";
            if ($('#btnOfflineOutCompany .b_yes:visible').length) {
                remark = "一会到";
            }
            if ($('#btnOfflineTakeOut .b_yes:visible').length) {
                submitOrder(5, 2, remark);
            } else {
                submitOrder(5, 1, remark);
            }
            $(this).parent().find('div:not(:last)').hide();
            $(this).parent().prev().find('div:not(:last)').hide();
        }
    });
    $('.b_continue,.b_history,#btnOfflineContinue').on('click', function () {
        setDivGoodsHeight(1);
        $('#divGoods').css('transform', 'translate(' + (0 - $(window).width()) + 'px, 0px)');
        $('#divGoods').css('-ms-transform', 'translate(' + (0 - $(window).width()) + 'px, 0px)');
        $('#divGoods').css('-moz-transform', 'translate(' + (0 - $(window).width()) + 'px, 0px)');
        $('#divGoods').css('-webkit-transform', 'translate(' + (0 - $(window).width()) + 'px, 0px)');
        $('#divGoods').css('-o-transform', 'translate(' + (0 - $(window).width()) + 'px, 0px)');

        $('.bottom_nav').css('overflow', 'hidden');
        $('#divBottomBtn').css('transform', 'translate(0px,-43px)');
        $('#divBottomBtn').css('-ms-transform', 'translate(0px,-43px)');
        $('#divBottomBtn').css('-moz-transform', 'translate(0px,-43px)');
        $('#divBottomBtn').css('-webkit-transform', 'translate(0px,-43px)');
        $('#divBottomBtn').css('-o-transform', 'translate(0px,-43px)');
    });
    $('.button_cart,.b_cart').on('click', function () {
        showCart();
        setDivGoodsHeight(0);
        $('#divGoods').css('transform', 'translate(0px, 0px)');
        $('#divGoods').css('-ms-transform', 'translate(0px, 0px)');
        $('#divGoods').css('-moz-transform', 'translate(0px, 0px)');
        $('#divGoods').css('-webkit-transform', 'translate(0px, 0px)');
        $('#divGoods').css('-o-transform', 'translate(0px, 0px)');
        $('#divBottomBtn').css('transform', 'translate(0px,0px)');
        $('#divBottomBtn').css('-ms-transform', 'translate(0px,0px)');
        $('#divBottomBtn').css('-moz-transform', 'translate(0px,0px)');
        $('#divBottomBtn').css('-webkit-transform', 'translate(0px,0px)');
        $('#divBottomBtn').css('-o-transform', 'translate(0px,0px)');
        setTimeout(function () {
            $('.bottom_nav').css('overflow', 'visible');
        }, 500);
    })
    $('.button_order').on('click', function () {
        if (confirm('返回首页？')) {
            location.href = '/mp_index.aspx?openid=' + $('#hfOpenId').val() + '&uninitarea=1';
        }
    });
    $('#divCartOnline .b_pay_offline').on('click', function () {
        $(this).parent().find('div:not(:last)').hide();
        submitOrder(1, 0);
    });
    $('#divCartOnline .b_pay_online').on('click', function () {
        $(this).parent().find('div:not(:last)').hide();
        submitOrder(3, 0);
    });
    $('#divCartOnline .b_pay_mppay').on('click', function () {
        $(this).parent().find('div:not(:last)').hide();
        submitOrder(5, 0);
    });
    $('#divGoods').css('transform', 'translate(' + (0 - $(window).width() * parseInt($('#hfTab').val())) + 'px, 0px)');
    $('#divGoods').css('-ms-transform', 'translate(' + (0 - $(window).width() * parseInt($('#hfTab').val())) + 'px, 0px)');
    $('#divGoods').css('-moz-transform', 'translate(' + (0 - $(window).width() * parseInt($('#hfTab').val())) + 'px, 0px)');
    $('#divGoods').css('-webkit-transform', 'translate(' + (0 - $(window).width() * parseInt($('#hfTab').val())) + 'px, 0px)');
    $('#divGoods').css('-o-transform', 'translate(' + (0 - $(window).width() * parseInt($('#hfTab').val())) + 'px, 0px)');
    $('#divGoods').css('height', $('body').css('height'));
    $('.order_complete_window').height($(window).height() + 'px');
    $('#btnCancelUserAddress').on('click', function () {
        $('#divUserAddress').hide();
        $('#divInputAddress').show();
    });
    $('.typechoice .item:first').on('click', function () {
        $('.typechoice').hide();
        $('body').css('background-image', 'url(' + $('#hfBackground1').val() + ')');
        $('#divOfflineAreaError').hide();
        $('#divOfflineArea').hide();
        $('#divVipOnline').show();
        //if (navigator.geolocation) {
        //    navigator.geolocation.getCurrentPosition(function (position) {
        //        var coords = position.coords;
        //        $.ajax({
        //            success: function (data_position) {
        //                if (data_position.status == 0) {
        //                    $.ajax({
        //                        success: function (data) {
        //                            if (!data) return;
        //                            if (data.Status == 1) {
        //                                isInArea = true;
        //                                if ($('#hfLastOnlineAreaId').val().length == 0) {
        //                                    //在区域内,没点过单直接显示配送站的提示
        //                                    var areaid = parseInt(data.Id);
        //                                    $('#hfAreaId').val(areaid);
        //                                    GetCarnivalOffline(areaid);//获取线下加价购商品信息
        //                                    //$('.button_order').text(data.Title);
        //                                    $('#hfDisamountId').val(data.Type);
        //                                    if (data.Type == 1) {
        //                                        $('.button_order').text('当前' + $('#hflowamount').val() + '元起送');
        //                                    } else {
        //                                        $('.button_order').text('当前' + $('#hflowamount_2').val() + '元起送');
        //                                    }
        //                                    $('#TipAreaAddressTitle').text($(this).data('areatitle'));
        //                                    $('#TipAreaAddressAddress').text($(this).data('areaaddress'));
        //                                    ShowDivTipAreaAddress()
        //                                    setTimeout(function () {
        //                                        $('#divUserAddress').hide();
        //                                        $('#DivTipAreaAddress').hide();
        //                                        $('#btnStartOrder').trigger('click');
        //                                    }, 3000);
        //                                } else {
        //                                    $('#divUserAddress').show();
        //                                    if ($('#hfUserAddress').val().length > 0 && $('#divUserAddress ul li').length == 0) {
        //                                        var userAddress = eval($('#hfUserAddress').val());
        //                                        for (var i = 0; i < userAddress.length; i++) {
        //                                            $('<li class="address" data-id="' + userAddress[i].Id + '" data-AreaId="' + userAddress[i].AreaId + '" data-AreaType="' + userAddress[i].AreaType
        //                                                + '" data-AreaTitle="' + userAddress[i].AreaTitle + '" data-AreaAddress="' + userAddress[i].AreaAddress
        //                                                + '" data-NickName="' + userAddress[i].NickName + '" data-Telphone="' + userAddress[i].Telphone + '" data-Address="' + userAddress[i].Address + '">'
        //                                                + userAddress[i].Address + ' ' + userAddress[i].Telphone + '</li>').appendTo($('#divUserAddress ul'));
        //                                        }
        //                                        $('#divUserAddress li').on('click', function () {
        //                                            $('#hfUserAddressId').val($(this).data('id'));
        //                                            $('#address').val($(this).data('address'));
        //                                            $('#address').attr('disabled', 'disabled');
        //                                            $('#phone').val($(this).data('telphone'));
        //                                            $('#nickname').val($(this).data('nickname'));
        //                                            var areaid = parseInt($(this).data('areaid'));
        //                                            $('#hfAreaId').val(areaid);
        //                                            GetCarnivalOffline(areaid);//获取线下加价购商品信息
        //                                            //$('.button_order').text(data.Title);
        //                                            $('#hfDisamountId').val(parseInt($(this).data('areatype')));
        //                                            if (parseInt($(this).data('areatype')) == 1) {
        //                                                $('.button_order').text('当前' + $('#hflowamount').val() + '元起送');
        //                                            } else {
        //                                                $('.button_order').text('当前' + $('#hflowamount_2').val() + '元起送');
        //                                            }
        //                                            $('#TipAreaAddressTitle').text($(this).data('areatitle'));
        //                                            $('#TipAreaAddressAddress').text($(this).data('areaaddress'));
        //                                            ShowDivTipAreaAddress()
        //                                            setTimeout(function () {
        //                                                $('#divUserAddress').hide();
        //                                                $('#DivTipAreaAddress').hide();
        //                                                $('#btnStartOrder').trigger('click');
        //                                            }, 1500);//时间
        //                                        });
        //                                    }
        //                                }
        //                            } else {
        //                                $('#txtInputAddres').val('');
        //                                $('#divInputAddress').show();
        //                            }
        //                        },
        //                        error: function (XMLHttpRequest, textStatus, errorThrown) {
        //                            $('#txtInputAddres').val('');
        //                            $('#divInputAddress').show();
        //                        },
        //                        url: '/tools/submit_ajax.ashx?action=get_polygon_contain',
        //                        data: {
        //                            position: data_position.locations[0].lat + ',' + data_position.locations[0].lng,
        //                            openid: $('#hfOpenId').val()
        //                        },
        //                        type: "post",
        //                        dataType: "json",
        //                        timeout: 60000
        //                    });
        //                } else {
        //                    $('#txtInputAddres').val('');
        //                    $('#divInputAddress').show();
        //                }

        //            },
        //            error: function (XMLHttpRequest, textStatus, errorThrown) {
        //                $('#txtInputAddres').val('');
        //                $('#divInputAddress').show();
        //            },
        //            url: 'http://apis.map.qq.com/ws/coord/v1/translate?locations=' + coords.latitude + ',' + coords.longitude + '&type=1&key=BOEBZ-2AB2R-IKTWG-W2JQG-HEUOV-2RF7Z&output=jsonp',
        //            type: "get",
        //            dataType: "jsonp",
        //            timeout: 60000
        //        });

        //    }, function (error) {
        //        if ($('#hfLastOnlineAreaId').val().length == 0) {
        //            $('#txtInputAddres').val('');
        //            $('#divInputAddress').show();
        //        } else {
        //            isInArea = true;
        //            $('#divUserAddress').show();
        //            if ($('#hfUserAddress').val().length > 0 && $('#divUserAddress ul li').length == 0) {
        //                var userAddress = eval($('#hfUserAddress').val());
        //                for (var i = 0; i < userAddress.length; i++) {
        //                    $('<li class="address" data-id="' + userAddress[i].Id + '" data-AreaId="' + userAddress[i].AreaId + '" data-AreaType="' + userAddress[i].AreaType
        //                        + '" data-AreaTitle="' + userAddress[i].AreaTitle + '" data-AreaAddress="' + userAddress[i].AreaAddress
        //                        + '" data-NickName="' + userAddress[i].NickName + '" data-Telphone="' + userAddress[i].Telphone + '" data-Address="' + userAddress[i].Address + '">'
        //                        + userAddress[i].Address + ' ' + userAddress[i].Telphone + '</li>').appendTo($('#divUserAddress ul'));
        //                }
        //                $('#divUserAddress li').on('click', function () {
        //                    $('#hfUserAddressId').val($(this).data('id'));
        //                    $('#address').val($(this).data('address'));
        //                    $('#address').attr('disabled', 'disabled');
        //                    $('#phone').val($(this).data('telphone'));
        //                    $('#nickname').val($(this).data('nickname'));
        //                    $('#hfUserAddressId').val($(this).data('id'));
        //                    var areaid = parseInt($(this).data('areaid'));
        //                    $('#hfAreaId').val(areaid);
        //                    GetCarnivalOffline(areaid);//获取线下加价购商品信息
        //                    //$('.button_order').text(data.Title);
        //                    $('#hfDisamountId').val(parseInt($(this).data('areatype')));
        //                    if (parseInt($(this).data('areatype')) == 1) {
        //                        $('.button_order').text('当前' + $('#hflowamount').val() + '元起送');
        //                    } else {
        //                        $('.button_order').text('当前' + $('#hflowamount_2').val() + '元起送');
        //                    }
        //                    $('#TipAreaAddressTitle').text($(this).data('areatitle'));
        //                    $('#TipAreaAddressAddress').text($(this).data('areaaddress'));
        //                    ShowDivTipAreaAddress()
        //                    setTimeout(function () {
        //                        $('#divUserAddress').hide();
        //                        $('#DivTipAreaAddress').hide();
        //                        $('#btnStartOrder').trigger('click');
        //                    }, 3000);
        //                });
        //            }
        //        }
        //    }, {
        //        // 指示浏览器获取高精度的位置，默认为false
        //        enableHighAcuracy: true,
        //        // 指定获取地理位置的超时时间，默认不限时，单位为毫秒
        //        timeout: 3000,
        //        // 最长有效期，在重复获取地理位置时，此参数指定多久再次获取位置。
        //        maximumAge: 3000
        //    });
        //}
        ShowlineAddress();
        $('#hfClass').val('0');

    });
    $('.typechoice .item:last').on('click', function () {
        $('.typechoice').hide();
        //$('body').attr('style', 'background-color: #693905;');
        $('body').css('background-image', 'url(' + $('#hfBackground2').val() + ')');
        $('#divInputAddress').hide();
        $('#divVipOffline').show();

        if ($.cookie('phone')) $("#phone").val($.cookie('phone'));
        searchCompanyList();
        $('#hfClass').val('1');
    });
    //$('.typechoice .item1:first').on('click', function () {
    //    window.location.href = "/mp_join_company.html";
    //});
    //$('.typechoice .item1:last').on('click', function () {
    //    window.location.href = "/mp_join_company.html";
    //});
    $('.offline .area_select li').on('click', function () {
        $('.offline .area_select li').removeClass('active');
        $(this).addClass('active');
        //$('.offline .area_confirm').show();
        //$('.offline .area_detail span').html($(this).data('content'));
    });
    $('.offline .btnCircle').on('click', function () {
        if ($(this).text() == '手动选店') {
            //$('.typechoice').show();
            $('#divOfflineArea').hide();
            $('#divOfflineAreaError').show();

            //$('#divOfflineArea .area_select').empty();
            return;
        }
        else if ($(this).text() == '返回') {
            $('#divOfflineAreaError').hide()
            $('.typechoice').show()
            return;
        }

        if ($('#txtInputAddresTake').val().length == 0) {
            alert('请输入店铺地址');
            return;
        }
        var areaid = 0;
        var areatitle = '';
        for (var i = 0; i < auto_areas.length; i++) {
            if ($('#txtInputAddresTake').val() == auto_areas[i].title) {
                areaid = auto_areas[i].id;
                areatitle = auto_areas[i].title;
            }
        }
        if (areaid == 0) {
            alert('没有匹配到地址');
            return;
        }
        ShowOffline(areaid, areatitle);
    });
    $('#divCarnivalOnline .item').on('click', function () {
        $('#divCarnivalOnline .item.select').find('div:first').hide();
        $('#divCarnivalOnline .item.select').next().show();
        $('#divCarnivalOnline .item.select').removeClass('select');
        $('#btnCarnivalOnline').removeClass('active');
        if (parseInt($('#divCarnivalOnline .num').html() + 1) >= parseInt($(this).data('change'))) {
            $(this).addClass('select');
            $(this).find('div:first').show();
            $(this).next().hide();
            $('#btnCarnivalOnline').addClass('active');
        }
    });

    $('#btnCarnivalOnline').on('click', function () {
        if ($('#divCarnivalOnline .item.select').length && $('#btnCarnivalOnline.active').length) {
            fullOrder($('#divCarnivalOnline .item.select'));
        }
    })
    //$('.typechoice .quan').html($('#divCarnivalOffline .item').length + '券');
    $('#btnCarnivalOffline').on('click', function () {
        if ($('#divCarnivalOffline .item:has(.cover)').length && $('#btnCarnivalOffline.active').length) {
            discountOrder($('#divCarnivalOffline .item:has(.cover)'));
        } else {
            //$('#divCarnivalOffline').hide();
            var type = 'discount';
            var cart = new Object();
            if ($('#hfCookie').val() != "") {
                cart = $.parseJSON($('#hfCookie').val());
                $.each(cart, function (key, val) {
                    if (key.indexOf("†" + type + "†") > -1) {
                        delete cart[key];
                        return false;
                    }
                });
            }
            $('#hfCookie').val(JSON.stringify(cart));
            calcTotal();
            showCart();
        }

    })

    $('#divCartOffline .b_pay_offline').on('click', function () {
        if ($(this).find('.b_yes').css('display') == 'inline-block') {
            $(this).find('.b_yes').css('display', 'none');
        } else {
            $(this).find('.b_yes').css('display', 'inline-block');
        }
        //$(this).parent().find('.b_yes').hide();

    });
    $('#btnCarnivalClose').on('click', function () {
        $('.bottom_nav').show();
        $('#divGoods').show();
        $('#divCarnivalOnline').hide();
    });
    $('#btnCarnivalOfflineClose').on('click', function () {
        //$('.bottom_nav').show();
        //$('#divGoods').show();
        $('#divCarnivalOffline').hide();
        $('#divCarnivalOffline .item').find('div:first').removeClass('cover');
        //var type = 'discount';
        //var cart = new Object();
        //if ($('#hfCookie').val() != "") {
        //    cart = $.parseJSON($('#hfCookie').val());
        //    $.each(cart, function (key, val) {
        //        if (key.indexOf("†" + type + "†") > -1) {
        //            delete cart[key];
        //            return false;
        //        }
        //    });
        //}
        //$('#hfCookie').val(JSON.stringify(cart));
        //calcTotal();
        //showCart();
        //setDivGoodsHeight(0);
        //$('#divCarnivalOffline .item .unselect').removeClass('cover');
        //$('#btnCarnivalOffline').removeClass('active');
    });

    if ($('#hfTab').val() == '2') {
        $('body').attr('style', 'background-color: white;');
        $('.b_history').unbind();
        $('.typechoice').hide();
        $('#divGoods').show();
        $('.bottom_nav').show();
        $.ajax({
            type: "post",
            url: "/tools/submit_ajax.ashx?action=mp_getlastorder",
            data: {
                openid: $('#hfOpenId').val()
            },
            dataType: "json",
            beforeSend: function (XMLHttpRequest) {
                //发送前动作

            },
            success: function (data, textStatus) {
                if (!data) return;
                if (data.msg == 1) {
                    $('.transdiv:last').show();
                    $('#orderaddress').html(data.address);
                    $('#ordertelphone').html(data.telphone);
                    $('#ordermessage').html(data.message);
                    $('#orderno').html(data.orderno);
                    $('.o_list').empty();
                    $(data.msgbox).appendTo($('.o_list'));
                    $('#order_time_line').html(data.ordertime);
                    $('#total').html('总计：￥' + data.total);
                    if (data.status == 0) {
                        $('.progress').hide();
                    } else if (data.status == 1) {
                        $('.progress .success').removeClass('gray');
                    } else if (data.status == 2) {
                        $('.progress .success').removeClass('gray');
                        $('.progress .next_step:first').removeClass('gray');
                        $('.progress .make').removeClass('gray');
                    } else if (data.status == 3) {
                        $('.progress .success').removeClass('gray');
                        $('.progress .next_step').removeClass('gray');
                        $('.progress .make').removeClass('gray');
                        $('.progress .sent').removeClass('gray');
                    }
                    setDivGoodsHeight(2);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("状态：" + textStatus + "；出错提示：" + errorThrown);

            },
            timeout: 50000
        });
    }
    var arrayPath = new Array();
    var strDistributionArea = $('#hfDistributionArea').val();
    var areas = strDistributionArea.split('_');
    for (var i = 0; i < areas.length; i++) {
        var path = new Array();
        for (var y = 0; y < areas[i].split('-')[2].split('|').length; y++) {
            path.push(new qq.maps.LatLng(parseFloat(areas[i].split('-')[2].split('|')[y].split(',')[0]), parseFloat(areas[i].split('-')[2].split('|')[y].split(',')[1])));
        }
        arrayPath.push({ Id: areas[i].split('-')[0], Title: areas[i].split('-')[1], Path: path });
    }
    var isAlertInputAddress = false;
    $("#txtInputAddres").autocomplete({
        source: function (request, response) {
            isInArea = false;
            $.ajax({
                url: "http://apis.map.qq.com/ws/place/v1/suggestion/?region=%E4%B8%8A%E6%B5%B7&output=jsonp&key=BOEBZ-2AB2R-IKTWG-W2JQG-HEUOV-2RF7Z&region_fix=1",
                dataType: "jsonp",
                data: {
                    keyword: request.term
                },
                success: function (data) {
                    response(data.data);
                }
            });
        },
        minLength: 0,
        select: function (event, ui) {
            var areaid = 0;
            $.ajax({
                success: function (data) {
                    if (!data) return;
                    if (data.Status == 1) {
                        isInArea = true;
                        areaid = parseInt(data.Id);
                        //$('.button_order').text(data.Title);
                        GetCarnivalOffline(areaid);//获取线下加价购商品信息
                        $('#hfDisamountId').val(data.Type);
                        if (data.Type == 1) {
                            $('.button_order').text('当前' + $('#hflowamount').val() + '元起送');
                        } else {
                            $('.button_order').text('当前' + $('#hflowamount_2').val() + '元起送');
                        }
                        if (data.ShowConfirm == 1 && isShowConfirmByOnline == false) {
                            $('#confirmTitle').text(data.Address);
                            $('#confirmAddress').prev().hide();
                            $('#confirmAddress').next().hide();
                            $('#confirmAddress').hide();
                            $('#btnConfirmReturn').unbind('click');
                            $('#btnConfirmReturn').on('click', function () {
                                $('body').attr('style', 'background-color: #007c54;');
                                $('#divGoods').hide();
                                $('.bottom_nav').hide();
                                $('#divInputAddress').show();
                                ShowDivConfirm();
                            });
                            $('#confirmAccept').text('3');
                            $('#btnConfirmReturn').text('不对');

                            $('#confirmTip2').show();
                            $('#confirmTip1').text('您的外卖将由');
                            ShowDivConfirm();
                            var confirmAcceptInterval = setInterval(function () {
                                $('#confirmAccept').text(parseInt($('#confirmAccept').text() - 1));
                                if (parseInt($('#confirmAccept').text()) == 0) {
                                    clearInterval(confirmAcceptInterval);
                                    $('#DivConfirm').hide();
                                }
                            }, 1000);
                        }
                        $("#txtInputAddres").val(ui.item.title + " " + ui.item.address.replace(ui.item.city, '').replace(ui.item.district, ''));
                        $('#hfAreaId').val(areaid);
                        $('#hfLatLng').val(ui.item.location.lat + ',' + ui.item.location.lng);
                        $('#btnStartOrder').trigger('click');

                    } else {
                        alert('抱歉！我们无法配送到您这里。');
                        isAlertInputAddress = true;
                    }
                },
                url: '/tools/submit_ajax.ashx?action=get_polygon_contain',
                data: {
                    position: ui.item.location.lat + ',' + ui.item.location.lng,
                    openid: $('#hfOpenId').val()
                },
                type: "post",
                dataType: "json",
                timeout: 60000
            });
            return false;
        },
        focus: function (event, ui) {
            $("#txtInputAddres").val(ui.item.title + " " + ui.item.address.replace(ui.item.city, '').replace(ui.item.district, ''));
            return false;
        }
    })
    .autocomplete("instance")._renderItem = function (ul, item) {
        return $('<li style="padding:4px;font-size:12px;">')
            .append('<i class="map_position"></i>' + item.title + ' <font style="font-size:12px;">' + item.address.replace(item.city, '').replace(item.district, '') + '</font>')
            .appendTo(ul);
    };

    $('#btnStartOrder').on('click', function () {
        if (!isInArea && !$('#txtInputAddres').val().length) {
            alert('请输入您的订餐地标（写字楼、小区或学校等）');
            return false;
        }
        if (!isInArea && !isAlertInputAddress) {
            if (!confirm('抱歉！我们无法配送到您这里。是否查看菜单')) {
                return false;
            }
        }
        //if (!$('#txtInputDetail').val().length) {
        //    alert('请输入楼层、房间号、详细地址');
        //    return false;
        //}
        //alert('位置Flag：' + isInArea);
        $('#divInputAddress').hide();
        if (!$('#hfAreaId').val().length) {
            $('#hfAreaId').val(arrayPath[0].Id);
            $('.button_order').text(arrayPath[0].Title);
        }
        $.cookie('AreaId', $('#hfAreaId').val(), { expires: 14 });
        $.cookie('AreaTitle_Input', $("#txtInputAddres").val(), { expires: 14 });
        $.cookie('AreaTitle_Input_Detail', $("#txtInputDetail").val(), { expires: 14 });
        //$('#address').val($("#txtInputAddres").val());
        //$('#address_detail').val($("#txtInputDetail").val());
        if ($('.button_order').text().length == 0) $('.button_order').text('返回首页');
        $('.menu li').eq(0).trigger('click');
        $('body').attr('style', 'background-color: white;');
        var isrun = $('#hfIsRun').val();
        if (isrun == 'false') {
            $('#divGoods').show();
            $('.bottom_nav').show();
            $('#divShopClose').show();
            $('#divBottomBtn').hide();
            return;
        }
        ShowLocker();
        //判断下架和繁忙
        $.ajax({
            type: "post",
            url: "/tools/submit_ajax.ashx?action=get_area_type",
            data: {
                id: $('#hfAreaId').val()
            },
            dataType: "json",
            success: function (data, textStatus) {
                ShowLocker()
                if (!data) return;
                if (data.busy == 1) {
                    $('.order_complete_window.late').show();
                    $('#divGoods').show();
                    $('.bottom_nav').show();
                } else {
                    $('.bottom_nav').show();
                    $('#divGoods').show();
                }
                if (data.lock == 1) {
                    $('.order_complete_window.arealock').show();
                    $('#divGoods').show();
                    $('#divAreaClose').show();
                    $('#divBottomBtn').remove();
                    $('.bottom_nav').show();
                } else {
                    $('.bottom_nav').show();
                    $('#divGoods').show();
                }
            },
            timeout: 50000
        });
    });
    //补单自动选择区域
    if ($('#hfAdditional').val() == '1') {
        //$('.online .area_select li[data-id="' + $('#hfAreaId').val() + '"]').addClass('active');
        //$('.online .confirm_div').trigger('click');
        isInArea = true;
        $('#txtInputAddres').val('1');
        $('#btnStartOrder').trigger('click');
    }
    $('#btnOldAddress').on('click', function () {
        if ($.cookie('OldAddress') != null && $.cookie('OldAddress').length > 0) {
            var arrAddress = $.parseJSON($.cookie('OldAddress'));
            var _curAddress = $('#address').val();
            for (var i = 0; i < arrAddress.length; i++) {
                if (arrAddress[i] == _curAddress) {
                    $('#address').val(arrAddress[(i + 1 >= arrAddress.length ? 0 : i + 1)]);
                    break;
                }
            }
        }
    });
    if ($.cookie('OldAddress') != null && $.cookie('OldAddress').length > 0) {
        var arrAddress = $.parseJSON($.cookie('OldAddress'));
        if (arrAddress.length <= 1) {
            $('#btnOldAddress').css('background-color', 'grey');
        }
    } else {
        $('#btnOldAddress').css('background-color', 'grey');
    }
    $('#DivGuide').on('click', function () {

    });

});

function comboOrder(obj) {
    $(obj).removeClass('edit');
    cartEffect();
    var goodsid = $(obj).data('id');
    var type = $(obj).data('type');
    var subgoodsid = "";
    $(obj).parent().find('.selected').each(function () {
        if (subgoodsid != "") subgoodsid += "†";
        subgoodsid += $(this).data('type') + "‡" + $(this).data('id') + "‡" + $(this).find('.name').html()
            + "‡" + ($(this).find('.taste').length > 0 ? $(this).find('.taste').html() : '');
    });
    var cart = new Object();
    var cartgoodsid = goodsid + "†" + type + "†" + subgoodsid;
    var price = $(obj).find('.price').html();
    var title = $(obj).find('.kind').html() + $(obj).find('.name').html();
    if ($('#hfCookie').val() != "") {
        cart = $.parseJSON($('#hfCookie').val());
        if (cart[cartgoodsid] != null) {
            cart[cartgoodsid].quantity += 1;
        } else {
            cart[cartgoodsid] = { quantity: 1, subgoodsid: subgoodsid, price: parseFloat(price), title: title };
        }
    } else {
        cart[cartgoodsid] = { quantity: 1, subgoodsid: subgoodsid, price: parseFloat(price), title: title };
    }
    $('#hfCookie').val(JSON.stringify(cart));
    calcTotal();
}

function oneOrder(obj) {
    cartEffect();
    var goodsid = $(obj).data('id');
    var type = $(obj).data('type');
    var lownum = $(obj).data('low') ? parseInt($(obj).data('low')) : 0;
    var taste = $(obj).find('.taste').html();
    var subgoodsid = '';
    if (taste) {
        subgoodsid = 'taste‡' + taste + '‡' + taste;
    }
    var cart = new Object();
    var cartgoodsid = goodsid + "†" + type + "†" + subgoodsid;
    var price = $(obj).find('.price').html();
    var title = $(obj).find('.name:not(.taste)').html();
    if ($('#hfCookie').val() != "") {
        cart = $.parseJSON($('#hfCookie').val());
        if (cart[cartgoodsid] != null) {
            cart[cartgoodsid].quantity += 1;
        } else {
            cart[cartgoodsid] = { quantity: lownum ? lownum : 1, subgoodsid: subgoodsid, price: parseFloat(price), title: title, low: lownum ? lownum : 1 };
        }
    } else {
        cart[cartgoodsid] = { quantity: lownum ? lownum : 1, subgoodsid: subgoodsid, price: parseFloat(price), title: title, low: lownum ? lownum : 1 };
    }
    $('#hfCookie').val(JSON.stringify(cart));
    calcTotal();
}

function fullOrder(obj) {
    var goodsid = $(obj).data('id');
    var type = 'full';
    var cart = new Object();
    var cartgoodsid = goodsid + "†" + type + "†";
    var price = '0';
    var title = $(obj).data('title');
    if ($('#hfCookie').val() != "") {
        cart = $.parseJSON($('#hfCookie').val());
        $.each(cart, function (key, val) {
            if (key.indexOf("†" + type + "†") > -1) {
                delete cart[key];
                return false;
            }
        });
        if (cart[cartgoodsid] != null) {
            cart[cartgoodsid].quantity = 1;
        } else {
            cart[cartgoodsid] = { quantity: 1, subgoodsid: '', price: parseFloat(price), title: title, low: 1 };
        }
    } else {
        cart[cartgoodsid] = { quantity: 1, subgoodsid: '', price: parseFloat(price), title: title, low: 1 };
    }
    $('#hfCookie').val(JSON.stringify(cart));
    calcTotal();
    showCart();
    $('.bottom_nav').show();
    $('#divGoods').show();
    $('#divCarnivalOnline').hide();
}

function discountOrder(obj) {
    var goodsid = $(obj).data('id');
    var type = 'discount';
    var lownum = 1;
    var subgoodsid = '';
    var cart = new Object();
    var cartgoodsid = goodsid + "†" + type + "†" + subgoodsid;
    var price = $(obj).data('price');
    var title = $(obj).data('title');
    if ($('#hfCookie').val() != "") {
        cart = $.parseJSON($('#hfCookie').val());
        $.each(cart, function (key, val) {
            if (key.indexOf("†" + type + "†") > -1) {
                delete cart[key];
                return false;
            }
        });
        if (cart[cartgoodsid] != null) {
            cart[cartgoodsid].quantity += 1;
        } else {
            cart[cartgoodsid] = { quantity: 1, subgoodsid: subgoodsid, price: parseFloat(price), title: title, low: 1 };
        }
    } else {
        cart[cartgoodsid] = { quantity: 1, subgoodsid: subgoodsid, price: parseFloat(price), title: title, low: 1 };
    }
    $('#hfCookie').val(JSON.stringify(cart));
    calcTotal();
    showCart();
    setDivGoodsHeight(0);
    //$('.bottom_nav').show();
    //$('#divGoods').show();
    //$('#divCarnivalOffline').hide();
}

function calcTotal() {
    if (!$('#hfCookie').val()) {
        $('#spanCount').html('0');
        $('#spanCount2').html('0');
        $('#spanCount3').html('0');
        return;
    }
    var cart = new Object();
    cart = $.parseJSON($('#hfCookie').val());
    var totalmoney = 0;
    for (var item in cart) {
        totalmoney += parseFloat((cart[item].quantity * cart[item].price).toFixed(2));
    }
    if (totalmoney < parseFloat($('#hffreedisamount').val()) && totalmoney > 0 && $('#hfClass').val() == '0' && $('#hfAdditional').val() == '0') totalmoney += parseFloat($('#hfdisamount').val());
    //判断外卖满99减5元 堂吃满39减2元
    if ($('#divVipOnline').length && $('#hfClass').val() == '0' && totalmoney >= 99 && parseInt($('#hfAvaliableCompanyAmount').val()) >= 5) {
        totalmoney = totalmoney - 5;
    } else if ($('#divVipOffline').length && $('#hfClass').val() == '1' && totalmoney >= 39 && parseInt($('#hfAvaliableCompanyAmount').val()) >= 2) {
        totalmoney = totalmoney - 2;
    }
    $('#spanCount').html(totalmoney.toString().replace('.00', ''));
    $('#spanCount2').html(totalmoney.toString().replace('.00', ''));
    $('#spanCount3').html(totalmoney.toString().replace('.00', ''));
}

var divheight = 0;
function setDivGoodsHeight(obj) {
    var tempheight = 0;
    if (obj == 1 && $('.clicked').length > 0) {
        tempheight = $('.clicked>div').length * 82 + $('.clicked').offset().top + 50;
    } else {
        tempheight = parseFloat($('.transdiv').eq(obj).css('height').replace('px', ''));
    }
    if (tempheight > divheight) {
        divheight = tempheight;
        $('#divGoods').css('height', divheight + 'px');
    }

}

function showCart() {
    if (!$('#hfCookie').val()) {
        $('.order').empty();
        $('<div class="detail" style="text-align: center;"><span>没有选购商品</span></div>').appendTo($('.order'));
        return;
    }
    var cart = new Object();
    cart = $.parseJSON($('#hfCookie').val());
    var rtn = '';
    var totalmoney = 0;
    for (var item in cart) {
        totalmoney += parseFloat((cart[item].quantity * cart[item].price).toFixed(2));
        if (item.split('†')[1] == 'combo') {
            rtn += '<div class="line ' + item.split('†')[1] + '"><span class="b_minus" data-id="' + item + '" onclick=\"minusGoods(this);\"></span><span class="count">' +
                cart[item].quantity
                + '</span><span class="name">' +
                cart[item].title
                + '</span><span class="price">￥' +
                (cart[item].price * cart[item].quantity).toString().replace('.00', '')
                + '元</span><span class="b_add" data-id="' + item + '" onclick=\"addGoods(this);\"></span></div>';
            rtn += '<div class="detail">';
            for (var i = 0; i < cart[item].subgoodsid.split('†').length ; i++) {
                rtn += '<span>*' + cart[item].subgoodsid.split('†')[i].split('‡')[2]
                if (cart[item].subgoodsid.split('†')[i].split('‡')[3].length > 0) {
                    rtn += '/' + cart[item].subgoodsid.split('†')[i].split('‡')[3];
                }
                rtn += '</span>';
            }
            rtn += '</div>';
        } else if (item.split('†')[1] == 'one') {
            rtn += '<div class="line ' + item.split('†')[1] + '"><span class="b_minus" data-id="' + item + '" data-low="' + cart[item].low + '" onclick=\"minusGoods(this);\"></span><span class="count">' +
                cart[item].quantity
                + '</span><span class="name">' +
                cart[item].title + (item.split('†')[2] ? '/' + item.split('†')[2].split('‡')[1] : '')
                + '</span><span class="price">￥' +
                (cart[item].price * cart[item].quantity).toString().replace('.00', '')
                + '元</span><span class="b_add" data-id="' + item + '" onclick=\"addGoods(this);\"></span></div>';
        } else if (item.split('†')[1] == 'full') {
            rtn += '<div class="line ' + item.split('†')[1] + '">'
                + '<span class="name" style="margin-left: 40px;color: rgb(233,84,18);">' +
                cart[item].title
                + '</span><span class="price" style="color: rgb(233,84,18);">￥' +
                (cart[item].price * cart[item].quantity).toString().replace('.00', '')
                + '元</span></div>';
        } else if (item.split('†')[1] == 'discount') {
            rtn += '<div class="line ' + item.split('†')[1] + '">'
                + '<span class="b_minus" data-id="' + item + '" data-low="' + cart[item].low + '" onclick=\"minusGoods(this);\"></span>'
                + '<span class="name" style="margin-left: 26px;color: rgb(233,84,18);">' +
                cart[item].title
                + '</span><span class="price" style="color: rgb(233,84,18);">￥' +
                (cart[item].price * cart[item].quantity).toString().replace('.00', '')
                + '元</span></div>';
        }
    }
    if (totalmoney < parseFloat($('#hffreedisamount').val()) && totalmoney > 0 && $('#hfClass').val() == '0' && $('#hfAdditional').val() != '1') {
        rtn += '<div class="line"><span class="name" style="width: 190px;">订单不满' + $('#hffreedisamount').val() + '元加' + $('#hfdisamount').val() + '元配送费</span><span class="price">￥' + $('#hfdisamount').val() + '元</span></div>';
    }
    //判断外卖满99减5元 堂吃满39减2元
    if ($('#divVipOnline').length && $('#hfClass').val() == '0' && totalmoney >= 99 && parseInt($('#hfAvaliableCompanyAmount').val()) >= 5) {
        $('#divCartOnline .b_pay_offline').text('餐到付款');
        $('#divCartOnline .b_pay_mppay').text('VIP微信支付');
        totalmoney = totalmoney - 5;
    } else {
        $('#divCartOnline .b_pay_offline').text('餐到付款');
        $('#divCartOnline .b_pay_mppay').text('微信支付');
    }
    if ($('#divVipOffline').length && $('#hfClass').val() == '1' && totalmoney >= 39 && parseInt($('#hfAvaliableCompanyAmount').val()) >= 2) {
        totalmoney = totalmoney - 2;
        $('#btnOfflineSubmit').text('VIP微信支付');
    } else {
        $('#btnOfflineSubmit').text('立即支付');
    }
    if ($('#divCarnivalOnline').length && $('#hfAdditional').val() != '0') {
        rtn += '<div onclick="showCarnival()" class="line" style="background: rgb(233,84,18);overflow: hidden;"><div class="carnivalgift"></div> <span class="carnivalgifttitle">尊贵满送服务</span></div>';
    }
    if ($('#divCarnivalOffline li').length) {
        rtn += '<div onclick="showCarnivalOffline()" class="line" style="background: rgb(233,84,18);overflow: hidden;"><div class="carnivalgift"></div> <span class="carnivalgifttitle">请选择优惠产品</span></div>';
        if (!$('#divCarnivalOffline').data('defaultShow')) {
            $('#divCarnivalOffline').show();
            $('#divCarnivalOffline').data('defaultShow', 'true');
        }
    }
    //高于限制金额时不显示货到付款
    if (totalmoney <= parseFloat($('#hfDeliverPayMaxAmountForMp').val()) && parseFloat($('#hfDeliverPayMaxAmountForMp').val()) > 0 && $('#hfAdditional').val() != '1') {
        isShowDeliver = false;
        $('#divCartOnline .b_pay_offline').hide();
    } else if ($('#hfAdditional').val() == '1') {
        isShowDeliver = false;
        $('#divCartOnline .b_pay_offline').hide();
    } else {
        isShowDeliver = true;
        if ($('#divCartOnline .b_pay_mppay:visible').length) {
            $('#divCartOnline .b_pay_offline').show();
        }
    }
    $('.order').empty();
    if (rtn) {
        $(rtn).appendTo($('.order'));
    } else {
        $('<div class="detail" style="text-align: center;"><span>没有选购商品</span></div>').appendTo($('.order'));
    }
}

function ShowLocker() {
    $('#DivLocker').css({
        "height": function () { return $(document).height(); },
        "width": function () { return $(document).width(); }
    });
    if ($('#DivLocker').css('display') == 'none') {
        $('#DivLocker').show();
    } else {
        $('#DivLocker').hide();
    }
}

function ShowDivConfirm() {
    $('#DivConfirm').css({
        "height": function () { return '900px'; },
        "width": function () { return $(window).width(); }
    });
    if ($('#DivConfirm').css('display') == 'none') {
        $('#DivConfirm').show();
    } else {
        $('#DivConfirm').hide();
    }
}
function ShowDivTipAreaAddress() {
    $('#DivTipAreaAddress').css({
        "height": function () { return '900px'; },
        "width": function () { return $(window).width(); }
    });
    if ($('#DivTipAreaAddress').css('display') == 'none') {
        $('#DivTipAreaAddress').show();
    } else {
        $('#DivTipAreaAddress').hide();
    }
}

function ShowChangeFocus() {
    $('#DivChangeFocus').css({
        "height": function () { return $(document).height(); },
        "width": function () { return $(document).width(); }
    });
    if ($('#DivChangeFocus').css('display') == 'none') {
        $('#DivChangeFocus').show();
    } else {
        $('#DivChangeFocus').hide();
    }
}

function minusGoods(obj) {
    var cartgoodsid = $(obj).data('id');
    var low = $(obj).data('low') ? parseInt($(obj).data('low')) : 1;
    var cart = new Object();
    cart = $.parseJSON($('#hfCookie').val());
    if (cart[cartgoodsid].quantity <= low) {
        delete cart[cartgoodsid];
    } else {
        cart[cartgoodsid].quantity -= 1;
    }
    //当减少商品只剩下线下活动的加价购时，减掉加价购商品，清空购物车Cookie
    var _discount = 0, _line = 0;
    var _key = '';
    $.each(cart, function (key, val) {
        if (key.indexOf("†discount†") > -1) {
            _discount++;
            _key = key;
        }
        _line++;
    });
    if (_discount == _line) {
        $('#hfCookie').val('');
    } else {
        $('#hfCookie').val(JSON.stringify(cart));
    }
    showCart();
    calcTotal();
}

function addGoods(obj) {
    var cartgoodsid = $(obj).data('id');
    var cart = new Object();
    cart = $.parseJSON($('#hfCookie').val());
    cart[cartgoodsid].quantity += 1;
    $('#hfCookie').val(JSON.stringify(cart));
    showCart();
    calcTotal();
}

function submitOrder(paymentid, takeout, remark) {
    if (!$('#hfCookie').val()) return;
    var patrn = /^1[3-9]\d{9}$/;
    var phone = $('#phone').val();
    if (takeout == 0) {
        //在非堂吃的情况下检测输入
        if (!$.trim($('#phone').val())) {
            $('#phone').css('border', '2px solid red');
            $('.b_submit').parent().find('div:not(:last)').hide();
            $('#phone').focus();
            return;
        } else if (isNaN($.trim($('#phone').val()))) {
            $('#phone').css('border', '2px solid red');
            $('.b_submit').parent().find('div:not(:last)').hide();
            alert("电话格式错误，请不要携带符号。");
            $('#phone').focus();
            return;
        } else if ($.trim($('#phone').val()).length != 11 || !patrn.test($.trim($('#phone').val()))) {
            $('#phone').css('border', '2px solid red');
            $('.b_submit').parent().find('div:not(:last)').hide();
            alert("手机号码必须为11位。");
            $('#phone').focus();
            return;
        } else {
            $('#phone').css('border', '1px solid #ea5414');
        }
        if (!$.trim($('#nickname').val()) && $('#hfClass').val() == '0') {
            $('#nickname').css('border', '2px solid red');
            $('.b_submit').parent().find('div:not(:last)').hide();
            $('#nickname').focus();
            return;
        } else {
            $('#nickname').css('border', '1px solid #ea5414');
        }
        if (!$.trim($('#address').val()) && $('#hfClass').val() == '0') {
            $('#address').css('border', '2px solid red');
            $('.b_submit').parent().find('div:not(:last)').hide();
            $('#address').focus();
            return;
        } else {
            $('#address').css('border', '1px solid #ea5414');
        }
        if (!isNaN($.trim($('#address').val())) && ($.trim($('#address').val()).length == 11 || $.trim($('#address').val()).length == 8)) {
            $('#address').css('border', '2px solid red');
            $('.b_submit').parent().find('div:not(:last)').hide();
            alert("地址有误请重新输入。");
            $('#address').focus();
            return;
        } else {
            $('#address').css('border', '1px solid #ea5414');
        }
        //if (!isInArea && $('#hfClass').val() == '0') {
        //    $('.b_submit').parent().find('div:not(:last)').hide();
        //    if (confirm("抱歉！我们无法配送到<" + $('#txtInputAddres').val() + ">。返回首页？")) {
        //        location.href = '/mp_index.aspx?openid=' + $('#hfOpenId').val() + '&uninitarea=1';
        //        return;
        //    } else {
        //        return;
        //    }

        //} else {
        //    $('#address').css('border', '1px solid #ea5414');
        //}
        //if (!$.trim($('#address_detail').val()) && $('#hfClass').val() == '0') {
        //    $('#address_detail').css('border', '2px solid red');
        //    $('.b_submit').parent().find('div:not(:last)').hide();
        //    $('#address_detail').focus();
        //    return;
        //} else {
        //    $('#address_detail').css('border', '0px');
        //}
    }
    if ($('#hfState').val() == 'master' && paymentid == 5) {
        ShowChangeFocus();
        return;
    }
    ShowLocker();
    $.ajax({
        type: "post",
        url: "/tools/submit_ajax.ashx?action=mp_ordersave",
        data: {
            phone: phone,
            nickname: $('#nickname').val(),
            address: $('#address').val(),
            message: $('#message').val(),
            areaid: $('#hfAreaId').val(),
            order: $('#hfCookie').val(),
            paymentid: paymentid,
            openid: $('#hfOpenId').val(),
            takeout: takeout,
            remark: remark,
            additional: $('#hfAdditional').val(),
            state: $('#hfState').val(),
            dismountid: $('#hfDisamountId').val(),
            inorout: $('#divCartOffline .btn_list').eq(1).find('.b_yes:visible').parent().text()
        },
        dataType: "json",
        beforeSend: function (XMLHttpRequest) {
            //发送前动作                            
        },
        success: function (data, textStatus) {
            ShowLocker();
            if (!data) return;
            if (data.msg == 1) {
                $.cookie('AreaTitle_Input', $("#address").val(), { expires: 14 });
                $.cookie('AreaTitle_Input_Detail', $("#address_detail").val(), { expires: 14 });
                $.cookie('phone', $("#phone").val(), { expires: 14 });
                InsertOldAddress();
                if (data.url) {
                    location.href = data.url;
                } else if (data.mppay) {
                    if (data.mppay == 1) {
                        wx.chooseWXPay({
                            timestamp: data.timestamp, // 支付签名时间戳，注意微信jssdk中的所有使用timestamp字段均为小写。但最新版的支付后台生成签名使用的timeStamp字段名需大写其中的S字符
                            nonceStr: data.noncestr, // 支付签名随机串，不长于 32 位
                            package: data.package, // 统一支付接口返回的prepay_id参数值，提交格式如：prepay_id=***）
                            signType: 'MD5', // 签名方式，默认为'SHA1'，使用新版支付需传入'MD5'
                            paySign: data.paysign, // 支付签名
                            success: function (res) {
                                // 支付成功后的回调函数
                                if (res.errMsg == "chooseWXPay:ok") {
                                    if (takeout == 0) {
                                        $('.order_complete_window.success .status_txt').html('订单已成功提交！');
                                        $('.order_complete_window.success').show();
                                    } else {
                                        $('#lblForHere').text(data.forhere);
                                        $('.order_complete_window.success_forhere').show();
                                    }
                                    setTimeout(function () {
                                        location.href = '/mp_index.aspx?state=slave&openid=' + $('#hfOpenId').val();
                                    }, 5000);
                                }
                            }
                        });
                    } else {
                        location.href = data.pay_location;
                    }
                } else {
                    if (data.msgbox) {
                        $('.status_txt').html(data.msgbox);
                    }
                    if (data.carnivalnums) {
                        $('#divCarnivalOnline .num').html(data.carnivalnums);
                    }
                    $('.order_complete_window.success').show();
                    setTimeout(function () {
                        if ($('#hfAdditional').val().length) {
                            location.href = '/mp_index.aspx?openid=' + $('#hfOpenId').val();
                        }
                        $('.order_complete_window.success').hide();
                        if ($('#hfClass').val() == '0') {
                            $('.b_continue').trigger('click');
                        } else if ($('#hfClass').val() == '1') {
                            $('#btnOfflineContinue').trigger('click');
                        }

                    }, 5000);
                    $('#hfCookie').val('');
                    //$('#phone').val('');
                    //$('#address').val('');
                    //$('#message').val('');
                    showCart();
                    calcTotal();
                }

            } else {
                if (data.msgbox) {
                    $('.status_txt').html(data.msgbox);
                }
                $('.order_complete_window.fail').show();
                setTimeout(function () {
                    $('.order_complete_window.fail').hide();
                    if ($('#hfClass').val() == '0') {
                        $('.b_continue').trigger('click');
                    } else if ($('#hfClass').val() == '1') {
                        $('#btnOfflineContinue').trigger('click');
                    }
                }, 5000);
            }



        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            ShowLocker();
            $('.status_txt').html('由于同时订餐人数较多，请稍后重新提交。');
            $('.order_complete_window.fail').show();
            setTimeout(function () {
                $('.order_complete_window.fail').hide();
                if ($('#hfClass').val() == '0') {
                    $('.b_continue').trigger('click');
                } else if ($('#hfClass').val() == '1') {
                    $('#btnOfflineContinue').trigger('click');
                }
            }, 5000);

        },
        timeout: 50000
    });

}

function cartEffect() {
    var identity = new Date().getTime();
    $('<div style="width:30px;height:30px;background-image:url(/templates/green/img/ball.png);background-size:30px 30px;position:fixed;top:' + window.event.y + 'px;left:' + window.event.x
        + 'px;transition: all 1s ease-in-out;-moz-transition: all 1s ease-in-out; -webkit-transition: all 1s ease-in-out; -o-transition: all 1s ease-in-out;border-radius:50%;" id="' + identity + '"></div>').appendTo($('body'));
    setTimeout(function () {
        $('#' + identity).css('top', $('body').height());
        $('#' + identity).css('left', '70%');
        setTimeout(function () {
            $('#' + identity).remove();
        }, 1000);
    }, 50);
}

function showCarnival() {
    $('.bottom_nav').hide();
    $('#divGoods').hide();
    $('#divCarnivalOnline').show();
}

function showCarnivalOffline() {
    //$('.bottom_nav').hide();
    //$('#divGoods').hide();
    $('#divCarnivalOffline').show();
}

function searchCompanyList() {
    var map, geolocation;
    //加载地图，调用浏览器定位服务
    map = new AMap.Map('container', {
        resizeEnable: true
    });
    map.plugin('AMap.Geolocation', function () {
        geolocation = new AMap.Geolocation({
            enableHighAccuracy: true,//是否使用高精度定位，默认:true
            timeout: 10000,          //超过10秒后停止定位，默认：无穷大
            buttonOffset: new AMap.Pixel(10, 20),//定位按钮与设置的停靠位置的偏移量，默认：Pixel(10, 20)
            zoomToAccuracy: true,      //定位成功后调整地图视野范围使定位位置及精度范围视野内可见，默认：false
            buttonPosition: 'RB'
        });
        map.addControl(geolocation);
        geolocation.getCurrentPosition();
        AMap.event.addListener(geolocation, 'complete', onComplete);//返回定位信息
        AMap.event.addListener(geolocation, 'error', onError);      //返回定位出错信息
    });
    //解析定位结果
    function onComplete(data) {
        //str.push('经度：' + data.position.getLng());
        //str.push('纬度：' + data.position.getLat());
        //if (data.accuracy) {
        //    str.push('精度：' + data.accuracy + ' 米');

        //}//如为IP精确定位结果则没有精度信息
        //str.push('是否经过偏移：' + (data.isConverted ? '是' : '否'));
        $.ajax({
            data: {
                position: data.position.getLat() + ',' + data.position.getLng(),
                openid: $('#hfOpenId').val()
            },
            success: function (data) {
                if (!data) return;
                if (data.Status == 1) {
                    if (data.ShowConfirm == 1) {
                        $('#confirmTitle').text(data.Title);
                        $('#confirmAddress').text(data.Address);
                        $('#btnConfirmReturn').unbind('click');
                        $('#btnConfirmReturn').on('click', function () {
                            $('body').attr('style', 'background-color: #007c54;');
                            $('#divGoods').hide();
                            $('.bottom_nav').hide();
                            $('#divOfflineAreaError').show();
                            ShowDivConfirm();
                            $('#confirmAccept').insertBefore($('#btnConfirmReturn'));
                        });
                        ShowDivConfirm();
                        $('#btnConfirmReturn').insertBefore($('#confirmAccept'));
                    }
                    ShowOffline(data.Id, data.Title);
                } else {

                    $('#divOfflineArea').hide();
                    if ($('#hfLastOfflineAreaId').val()) {
                        ShowOffline($('#hfLastOfflineAreaId').val(), $('#hfLastOfflineArea').val());
                        $('#confirmTitle').text($('#hfLastOfflineArea').val());
                        $('#confirmAddress').text($('#hflastOfflineAreaAddress').val());
                        $('#btnConfirmReturn').unbind('click');
                        $('#btnConfirmReturn').on('click', function () {
                            $('body').attr('style', 'background-color: #007c54;');
                            $('#divGoods').hide();
                            $('.bottom_nav').hide();
                            $('#divOfflineAreaError').show();
                            ShowDivConfirm();
                        });
                        ShowDivConfirm();
                    } else {
                        $('#divOfflineAreaError').show();
                    }
                }
            },
            url: '/tools/submit_ajax.ashx?action=get_polygon_contain_for_take',
            type: "post",
            dataType: "json",
            timeout: 60000
        });

    }
    //解析定位错误信息
    function onError(data) {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(function (position) {
                var coords = position.coords;
                $.ajax({
                    success: function (data_position) {
                        $.ajax({
                            data: {
                                position: data_position.locations[0].lat + ',' + data_position.locations[0].lng,
                                openid: $('#hfOpenId').val()
                            },
                            success: function (data) {
                                if (!data) return;
                                if (data.Status == 1) {
                                    if (data.ShowConfirm == 1) {
                                        $('#confirmTitle').text(data.Title);
                                        $('#confirmAddress').text(data.Address);
                                        $('#btnConfirmReturn').unbind('click');
                                        $('#btnConfirmReturn').on('click', function () {
                                            $('body').attr('style', 'background-color: #007c54;');
                                            $('#divGoods').hide();
                                            $('.bottom_nav').hide();
                                            $('#divOfflineAreaError').show();
                                            ShowDivConfirm();
                                            $('#confirmAccept').insertBefore($('#btnConfirmReturn'));
                                        });
                                        ShowDivConfirm();
                                        $('#btnConfirmReturn').insertBefore($('#confirmAccept'));
                                    }
                                    ShowOffline(data.Id, data.Title);
                                } else {
                                    //$('#divOfflineArea ul').empty();
                                    //$('<li class="item">为保证口感，堂吃仅支持600米内提前下单，您可以至店附近下单，谢谢！</li>').appendTo($('#divOfflineArea ul'));
                                    //$('#btnStartOfflineOrder').text('返回').css('line-height','40px');
                                    //$('#btnStartOfflineOrder').addClass('other');
                                    //$('#divOfflineArea').show();

                                    //$('#divOfflineArea').hide();
                                    //$('#divOfflineAreaError').show();
                                    $('#divOfflineArea').hide();
                                    if ($('#hfLastOfflineAreaId').val()) {
                                        ShowOffline($('#hfLastOfflineAreaId').val(), $('#hfLastOfflineArea').val());
                                        $('#confirmTitle').text($('#hfLastOfflineArea').val());
                                        $('#confirmAddress').text($('#hflastOfflineAreaAddress').val());
                                        $('#btnConfirmReturn').unbind('click');
                                        $('#btnConfirmReturn').on('click', function () {
                                            $('body').attr('style', 'background-color: #007c54;');
                                            $('#divGoods').hide();
                                            $('.bottom_nav').hide();
                                            $('#divOfflineAreaError').show();
                                            ShowDivConfirm();
                                        });
                                        ShowDivConfirm();
                                    } else {
                                        $('#divOfflineAreaError').show();
                                    }
                                }
                            },
                            url: '/tools/submit_ajax.ashx?action=get_polygon_contain_for_take',
                            type: "post",
                            dataType: "json",
                            timeout: 60000
                        });
                    },
                    url: 'http://apis.map.qq.com/ws/coord/v1/translate?locations=' + coords.latitude + ',' + coords.longitude + '&type=1&key=BOEBZ-2AB2R-IKTWG-W2JQG-HEUOV-2RF7Z&output=jsonp',
                    type: "get",
                    dataType: "jsonp",
                    timeout: 60000
                });

            }, function (error) {
                $('#divOfflineArea').hide();
                if ($('#hfLastOfflineAreaId').val()) {
                    ShowOffline($('#hfLastOfflineAreaId').val(), $('#hfLastOfflineArea').val());
                    $('#confirmTitle').text($('#hfLastOfflineArea').val());
                    $('#confirmAddress').text($('#hflastOfflineAreaAddress').val());
                    $('#btnConfirmReturn').unbind('click');
                    $('#btnConfirmReturn').on('click', function () {
                        $('body').attr('style', 'background-color: #007c54;');
                        $('#divGoods').hide();
                        $('.bottom_nav').hide();
                        $('#divOfflineAreaError').show();
                        ShowDivConfirm();
                    });
                    ShowDivConfirm();
                } else {
                    $('#divOfflineAreaError').show();
                }

            }, {
                // 指示浏览器获取高精度的位置，默认为false
                enableHighAcuracy: true,
                // 指定获取地理位置的超时时间，默认不限时，单位为毫秒
                timeout: 3000,
                // 最长有效期，在重复获取地理位置时，此参数指定多久再次获取位置。
                maximumAge: 3000
            });
        }
    }

}

function InsertOldAddress() {
    var _curAddress = $('#address').val();
    if (_curAddress.length == 0) return;
    if ($.cookie('OldAddress') != null && $.cookie('OldAddress').length > 0) {
        var arrAddress = $.parseJSON($.cookie('OldAddress'));
        for (var i = 0; i < arrAddress.length; i++) {
            if (arrAddress[i] == _curAddress) {
                return;
            }
        }
        arrAddress.push(_curAddress);
        $.cookie('OldAddress', JSON.stringify(arrAddress), { expires: 365 });
        if (arrAddress.length <= 1) {
            $('#btnOldAddress').css('background-color', 'grey');
        } else {
            $('#btnOldAddress').css('background-color', '#ea5414');
        }
    } else {
        var arrAddress = new Array();
        arrAddress.push(_curAddress);
        $.cookie('OldAddress', JSON.stringify(arrAddress), { expires: 365 });
    }
}

function GetCarnivalOffline(areaid) {
    $.ajax({
        type: "post",
        url: "/tools/submit_ajax.ashx?action=get_carnival_offline",
        data: {
            areaid: areaid,
            openid: $('#hfOpenId').val()
        },
        dataType: "html",
        success: function (data, textStatus) {
            if (!data) return;
            $('#carnivalOfflineMenu').remove();
            $('#btnCarnivalOffline').before(data);
            //$('#btnCarnivalOffline,#btnCarnivalOfflineClose').css('display', 'inline-block');
            $('#divCarnivalOffline .item').on('click', function () {
                //$('#divCarnivalOffline .item .unselect').removeClass('cover');
                if ($(this).find('div:first').hasClass('cover')) {
                    $(this).find('div:first').removeClass('cover');
                    $(this).grumble({
                        text: '已取消',
                        angle: 340,
                        distance: 50,
                        hideAfter: 2000
                    });
                    $('#btnCarnivalOffline').trigger('click');
                } else {
                    $('#divCarnivalOffline .item .unselect').removeClass('cover');
                    $(this).find('div:first').addClass('cover');
                    $('#btnCarnivalOffline').trigger('click');
                    //$('#btnCarnivalOffline,#btnCarnivalOfflineClose').css('display', 'inline-block');
                }
            });
            ShowHeadLine();
            if ($('#divCarnivalOffline div[data-headline="1"]').length) {
                $('#divCarnivalOffline div[data-headline="1"]').find('div:first').addClass('cover');
            } else {
                $('#divCarnivalOffline .item:first div:first').addClass('cover');
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("状态：" + textStatus + "；出错提示：" + errorThrown);
        },
        timeout: 50000
    });
}

function ShowOffline(areaid, areatitle) {

    GetCarnivalOffline(areaid);//获取线下加价购商品信息
    $('#hfAreaId').val(areaid);
    $('.button_order').text(areatitle);
    $('#divOfflineArea').hide();
    $('#divOfflineAreaError').hide();
    $('.bottom_nav').show();
    $('.menu li').eq(0).trigger('click');
    $('body').attr('style', 'background-color: white;');
    $('.attach_info .line').hide();
    //$('.attach_info .line').eq(1).hide();
    //$('.attach_info .line').eq(2).hide();
    $('#divCartOnline').hide();
    $('#divCartOffline').show();
    $('#divGoods').show();
    var isrun = $('#hfHereIsRun').val();
    if (isrun == 'false') {
        $('#divHereClose').show();
        $('#divBottomBtn').hide();
        return;
    }
    ShowLocker();
    $.ajax({
        type: "post",
        url: "/tools/submit_ajax.ashx?action=get_area_type",
        data: {
            id: areaid
        },
        dataType: "json",
        beforeSend: function (XMLHttpRequest) {
            //发送前动作
        },
        success: function (data, textStatus) {
            ShowLocker();
            if (!data) return;
            if (data.busy == 1) {
                $('.order_complete_window.late').show();
                $('#divGoods').show();
                $('.bottom_nav').show();
            }
            if (data.lock == 1) {
                $('.order_complete_window.arealock').show();
                $('#divGoods').show();
                $('#divAreaClose').show();
                $('#divBottomBtn').remove();
                $('.bottom_nav').show();
            } else {
                $('.bottom_nav').show();
                $('#divGoods').show();
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            ShowLocker();
            alert("状态：" + textStatus + "；出错提示：" + errorThrown);
        },
        timeout: 50000
    });
}

function ShowHeadLine() {
    if ($('#DivConfirm:visible').length) return;
    if ($('#divCarnivalOffline div[data-headline="1"]').length) {
        $('#DivHeadLine').css({
            "height": function () { return '900px'; },
            "width": function () { return $(window).width(); }
        });
        $('#DivHeadLineImg').attr('src', $('#divCarnivalOffline div[data-headline="1"]').data('headlineimg'));
        $('#headlineTitle').text($('#divCarnivalOffline div[data-headline="1"]').data('headlinetitle'));
        $('#DivHeadLine').show();
    }
}

function ShowlineAddress() {
    var map, geolocation;
    //加载地图，调用浏览器定位服务
    map = new AMap.Map('container', {
        resizeEnable: true
    });
    map.plugin('AMap.Geolocation', function () {
        geolocation = new AMap.Geolocation({
            enableHighAccuracy: true,//是否使用高精度定位，默认:true
            timeout: 10000,          //超过10秒后停止定位，默认：无穷大
            buttonOffset: new AMap.Pixel(10, 20),//定位按钮与设置的停靠位置的偏移量，默认：Pixel(10, 20)
            zoomToAccuracy: true,      //定位成功后调整地图视野范围使定位位置及精度范围视野内可见，默认：false
            buttonPosition: 'RB'
        });
        map.addControl(geolocation);
        geolocation.getCurrentPosition();
        AMap.event.addListener(geolocation, 'complete', onComplete);//返回定位信息
        AMap.event.addListener(geolocation, 'error', onError);      //返回定位出错信息
    });
    //解析定位结果
    function onComplete(data) {
        $.ajax({
            success: function (data) {
                if (!data) return;
                if (data.Status == 1) {
                    isInArea = true;
                    //if ($('#hfLastOnlineAreaId').val().length == 0) {
                        //在区域内,没点过单直接显示配送站的提示
                        var areaid = parseInt(data.Id);
                        $('#hfAreaId').val(areaid);
                        var userAddress = eval($('#hfUserAddress').val());
                        $('#TipAreaAddressTitle').text($(this).data('areatitle'));
                        $('#TipAreaAddressAddress').text($(this).data('areaaddress'));
                        if ($('#hfUserAddress').val().length > 0) {
                            for (var i = 0; i < userAddress.length; i++) {
                                if (areaid == userAddress[i].AreaId) {
                                    $('#hfUserAddressId').val(userAddress[i].Id);
                                    $('#address').val(userAddress[i].Address);
                                    $('#address').attr('disabled', 'disabled');
                                    $('#phone').val(userAddress[i].Telphone);
                                    $('#nickname').val(userAddress[i].NickName);
                                    $('#TipAreaAddressTitle').text(userAddress[i].AreaTitle);
                                    $('#TipAreaAddressAddress').text(userAddress[i].AreaAddress);
                                }
                            }
                        }
                        
                        GetCarnivalOffline(areaid);//获取线下加价购商品信息
                        //$('.button_order').text(data.Title);
                        $('#hfDisamountId').val(data.Type);
                        if (data.Type == 1) {
                            $('.button_order').text('当前' + $('#hflowamount').val() + '元起送');
                        } else {
                            $('.button_order').text('当前' + $('#hflowamount_2').val() + '元起送');
                        }
                       
                        ShowDivTipAreaAddress()
                        setTimeout(function () {
                            $('#divUserAddress').hide();
                            $('#DivTipAreaAddress').hide();
                            $('#btnStartOrder').trigger('click');
                        }, 3000);
                    //} else {
                    //    $('#divUserAddress').show();
                    //    if ($('#hfUserAddress').val().length > 0 && $('#divUserAddress ul li').length == 0) {
                    //        var userAddress = eval($('#hfUserAddress').val());
                    //        for (var i = 0; i < userAddress.length; i++) {
                    //            $('<li class="address" data-id="' + userAddress[i].Id + '" data-AreaId="' + userAddress[i].AreaId + '" data-AreaType="' + userAddress[i].AreaType
                    //                + '" data-AreaTitle="' + userAddress[i].AreaTitle + '" data-AreaAddress="' + userAddress[i].AreaAddress
                    //                + '" data-NickName="' + userAddress[i].NickName + '" data-Telphone="' + userAddress[i].Telphone + '" data-Address="' + userAddress[i].Address + '">'
                    //                + userAddress[i].Address + ' ' + userAddress[i].Telphone + '</li>').appendTo($('#divUserAddress ul'));
                    //        }
                    //        $('#divUserAddress li').on('click', function () {
                    //            $('#hfUserAddressId').val($(this).data('id'));
                    //            $('#address').val($(this).data('address'));
                    //            $('#address').attr('disabled', 'disabled');
                    //            $('#phone').val($(this).data('telphone'));
                    //            $('#nickname').val($(this).data('nickname'));
                    //            var areaid = parseInt($(this).data('areaid'));
                    //            $('#hfAreaId').val(areaid);
                    //            GetCarnivalOffline(areaid);//获取线下加价购商品信息
                    //            //$('.button_order').text(data.Title);
                    //            $('#hfDisamountId').val(parseInt($(this).data('areatype')));
                    //            if (parseInt($(this).data('areatype')) == 1) {
                    //                $('.button_order').text('当前' + $('#hflowamount').val() + '元起送');
                    //            } else {
                    //                $('.button_order').text('当前' + $('#hflowamount_2').val() + '元起送');
                    //            }
                    //            $('#TipAreaAddressTitle').text($(this).data('areatitle'));
                    //            $('#TipAreaAddressAddress').text($(this).data('areaaddress'));
                    //            ShowDivTipAreaAddress()
                    //            setTimeout(function () {
                    //                $('#divUserAddress').hide();
                    //                $('#DivTipAreaAddress').hide();
                    //                $('#btnStartOrder').trigger('click');
                    //            }, 1500);//时间
                    //        });
                    //    }
                    //}
                } else {
                    $('#txtInputAddres').val('');
                    $('#divInputAddress').show();
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                $('#txtInputAddres').val('');
                $('#divInputAddress').show();
            },
            url: '/tools/submit_ajax.ashx?action=get_polygon_contain',
            data: {
                position: data.position.getLat() + ',' + data.position.getLng(),
                openid: $('#hfOpenId').val()
            },
            type: "post",
            dataType: "json",
            timeout: 60000
        });

    }
    //解析定位错误信息
    function onError(data) {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(function (position) {
                var coords = position.coords;
                $.ajax({
                    success: function (data_position) {
                        if (data_position.status == 0) {
                            $.ajax({
                                success: function (data) {
                                    if (!data) return;
                                    if (data.Status == 1) {
                                        isInArea = true;
                                        if ($('#hfLastOnlineAreaId').val().length == 0) {
                                            //在区域内,没点过单直接显示配送站的提示
                                            var areaid = parseInt(data.Id);
                                            $('#hfAreaId').val(areaid);
                                            GetCarnivalOffline(areaid);//获取线下加价购商品信息
                                            //$('.button_order').text(data.Title);
                                            $('#hfDisamountId').val(data.Type);
                                            if (data.Type == 1) {
                                                $('.button_order').text('当前' + $('#hflowamount').val() + '元起送');
                                            } else {
                                                $('.button_order').text('当前' + $('#hflowamount_2').val() + '元起送');
                                            }
                                            $('#TipAreaAddressTitle').text($(this).data('areatitle'));
                                            $('#TipAreaAddressAddress').text($(this).data('areaaddress'));
                                            ShowDivTipAreaAddress()
                                            setTimeout(function () {
                                                $('#divUserAddress').hide();
                                                $('#DivTipAreaAddress').hide();
                                                $('#btnStartOrder').trigger('click');
                                            }, 3000);
                                        } else {
                                            $('#divUserAddress').show();
                                            if ($('#hfUserAddress').val().length > 0 && $('#divUserAddress ul li').length == 0) {
                                                var userAddress = eval($('#hfUserAddress').val());
                                                for (var i = 0; i < userAddress.length; i++) {
                                                    $('<li class="address" data-id="' + userAddress[i].Id + '" data-AreaId="' + userAddress[i].AreaId + '" data-AreaType="' + userAddress[i].AreaType
                                                        + '" data-AreaTitle="' + userAddress[i].AreaTitle + '" data-AreaAddress="' + userAddress[i].AreaAddress
                                                        + '" data-NickName="' + userAddress[i].NickName + '" data-Telphone="' + userAddress[i].Telphone + '" data-Address="' + userAddress[i].Address + '">'
                                                        + userAddress[i].Address + ' ' + userAddress[i].Telphone + '</li>').appendTo($('#divUserAddress ul'));
                                                }
                                                $('#divUserAddress li').on('click', function () {
                                                    $('#hfUserAddressId').val($(this).data('id'));
                                                    $('#address').val($(this).data('address'));
                                                    $('#address').attr('disabled', 'disabled');
                                                    $('#phone').val($(this).data('telphone'));
                                                    $('#nickname').val($(this).data('nickname'));
                                                    var areaid = parseInt($(this).data('areaid'));
                                                    $('#hfAreaId').val(areaid);
                                                    GetCarnivalOffline(areaid);//获取线下加价购商品信息
                                                    //$('.button_order').text(data.Title);
                                                    $('#hfDisamountId').val(parseInt($(this).data('areatype')));
                                                    if (parseInt($(this).data('areatype')) == 1) {
                                                        $('.button_order').text('当前' + $('#hflowamount').val() + '元起送');
                                                    } else {
                                                        $('.button_order').text('当前' + $('#hflowamount_2').val() + '元起送');
                                                    }
                                                    $('#TipAreaAddressTitle').text($(this).data('areatitle'));
                                                    $('#TipAreaAddressAddress').text($(this).data('areaaddress'));
                                                    ShowDivTipAreaAddress()
                                                    setTimeout(function () {
                                                        $('#divUserAddress').hide();
                                                        $('#DivTipAreaAddress').hide();
                                                        $('#btnStartOrder').trigger('click');
                                                    }, 1500);//时间
                                                });
                                            }
                                        }
                                    } else {
                                        $('#txtInputAddres').val('');
                                        $('#divInputAddress').show();
                                    }
                                },
                                error: function (XMLHttpRequest, textStatus, errorThrown) {
                                    $('#txtInputAddres').val('');
                                    $('#divInputAddress').show();
                                },
                                url: '/tools/submit_ajax.ashx?action=get_polygon_contain',
                                data: {
                                    position: data_position.locations[0].lat + ',' + data_position.locations[0].lng,
                                    openid: $('#hfOpenId').val()
                                },
                                type: "post",
                                dataType: "json",
                                timeout: 60000
                            });
                        } else {
                            $('#txtInputAddres').val('');
                            $('#divInputAddress').show();
                        }

                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        $('#txtInputAddres').val('');
                        $('#divInputAddress').show();
                    },
                    url: 'http://apis.map.qq.com/ws/coord/v1/translate?locations=' + coords.latitude + ',' + coords.longitude + '&type=1&key=BOEBZ-2AB2R-IKTWG-W2JQG-HEUOV-2RF7Z&output=jsonp',
                    type: "get",
                    dataType: "jsonp",
                    timeout: 60000
                });

            }, function (error) {
                if ($('#hfLastOnlineAreaId').val().length == 0) {
                    $('#txtInputAddres').val('');
                    $('#divInputAddress').show();
                } else {
                    isInArea = true;
                    $('#divUserAddress').show();
                    if ($('#hfUserAddress').val().length > 0 && $('#divUserAddress ul li').length == 0) {
                        var userAddress = eval($('#hfUserAddress').val());
                        for (var i = 0; i < userAddress.length; i++) {
                            $('<li class="address" data-id="' + userAddress[i].Id + '" data-AreaId="' + userAddress[i].AreaId + '" data-AreaType="' + userAddress[i].AreaType
                                + '" data-AreaTitle="' + userAddress[i].AreaTitle + '" data-AreaAddress="' + userAddress[i].AreaAddress
                                + '" data-NickName="' + userAddress[i].NickName + '" data-Telphone="' + userAddress[i].Telphone + '" data-Address="' + userAddress[i].Address + '">'
                                + userAddress[i].Address + ' ' + userAddress[i].Telphone + '</li>').appendTo($('#divUserAddress ul'));
                        }
                        $('#divUserAddress li').on('click', function () {
                            $('#hfUserAddressId').val($(this).data('id'));
                            $('#address').val($(this).data('address'));
                            $('#address').attr('disabled', 'disabled');
                            $('#phone').val($(this).data('telphone'));
                            $('#nickname').val($(this).data('nickname'));
                            $('#hfUserAddressId').val($(this).data('id'));
                            var areaid = parseInt($(this).data('areaid'));
                            $('#hfAreaId').val(areaid);
                            GetCarnivalOffline(areaid);//获取线下加价购商品信息
                            //$('.button_order').text(data.Title);
                            $('#hfDisamountId').val(parseInt($(this).data('areatype')));
                            if (parseInt($(this).data('areatype')) == 1) {
                                $('.button_order').text('当前' + $('#hflowamount').val() + '元起送');
                            } else {
                                $('.button_order').text('当前' + $('#hflowamount_2').val() + '元起送');
                            }
                            $('#TipAreaAddressTitle').text($(this).data('areatitle'));
                            $('#TipAreaAddressAddress').text($(this).data('areaaddress'));
                            ShowDivTipAreaAddress()
                            setTimeout(function () {
                                $('#divUserAddress').hide();
                                $('#DivTipAreaAddress').hide();
                                $('#btnStartOrder').trigger('click');
                            }, 3000);
                        });
                    }
                }
            }, {
                // 指示浏览器获取高精度的位置，默认为false
                enableHighAcuracy: true,
                // 指定获取地理位置的超时时间，默认不限时，单位为毫秒
                timeout: 3000,
                // 最长有效期，在重复获取地理位置时，此参数指定多久再次获取位置。
                maximumAge: 3000
            });
        }

    }
}