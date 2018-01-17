<%@ Page Language="C#" AutoEventWireup="true" Inherits="DTcms.Web.UI.Page.mp_index" ValidateRequest="false" %>
<%@ Import namespace="System.Collections.Generic" %>
<%@ Import namespace="System.Text" %>
<%@ Import namespace="System.Data" %>
<%@ Import namespace="DTcms.Common" %>

<script runat="server">
override protected void OnInit(EventArgs e)
{

	/* 
		This page was created by DTcms Template Engine at 2017/8/27 2:49:51.
		本页面代码由DTcms模板引擎生成于 2017/8/27 2:49:51. 
	*/

	base.OnInit(e);
	StringBuilder templateBuilder = new StringBuilder(220000);

	templateBuilder.Append("<!DOCTYPE html>\r\n<html>\r\n<head>\r\n    <meta charset=\"utf-8\" />\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0;\">\r\n    <title>");
	templateBuilder.Append(Utils.ObjectToStr(config.webname));
	templateBuilder.Append("</title>\r\n    <meta content=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.webkeyword));
	templateBuilder.Append("\" name=\"keywords\">\r\n    <meta content=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.webdescription));
	templateBuilder.Append("\" name=\"description\">\r\n    <script src=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.templateskin));
	templateBuilder.Append("/js/jquery-1.10.2.min.js\" type=\"text/javascript\"></");
	templateBuilder.Append("script>\r\n    <script src=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.templateskin));
	templateBuilder.Append("/js/jquery.cookie.js\" type=\"text/javascript\"></");
	templateBuilder.Append("script>\r\n    <script src=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.templateskin));
	templateBuilder.Append("/js/mp.js?ver=147\" type=\"text/javascript\"></");
	templateBuilder.Append("script>\r\n    <link media=\"screen\" type=\"text/css\" href=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.templateskin));
	templateBuilder.Append("/css/mp.css?ver=54\" rel=\"stylesheet\">    \r\n    <script type=\"text/javascript\" src=\"http://res.wx.qq.com/open/js/jweixin-1.0.0.js\"></");
	templateBuilder.Append("script>\r\n    <script charset=\"utf-8\" src=\"http://map.qq.com/api/js?v=2.exp&key=BOEBZ-2AB2R-IKTWG-W2JQG-HEUOV-2RF7Z\"></");
	templateBuilder.Append("script>\r\n    <script src=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.templateskin));
	templateBuilder.Append("/js/jquery-ui.min.js\" type=\"text/javascript\"></");
	templateBuilder.Append("script>\r\n    <link media=\"screen\" type=\"text/css\" href=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.templateskin));
	templateBuilder.Append("/css/jquery-ui.min.css\" rel=\"stylesheet\">\r\n    <script src=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.templateskin));
	templateBuilder.Append("/grumble/js/jquery.grumble.min.js\" type=\"text/javascript\"></");
	templateBuilder.Append("script>\r\n    <link media=\"screen\" type=\"text/css\" href=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.templateskin));
	templateBuilder.Append("/grumble/css/grumble.min.css\" rel=\"stylesheet\">\r\n    <script type=\"text/javascript\">\r\n        wx.config({\r\n            debug: false, // 开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印。\r\n            appId: '");
	templateBuilder.Append(Utils.ObjectToStr(config.mp_slave_appid));
	templateBuilder.Append("', // 必填，公众号的唯一标识\r\n            timestamp: ");
	templateBuilder.Append(Utils.ObjectToStr(jstimestamp));
	templateBuilder.Append(", // 必填，生成签名的时间戳\r\n            nonceStr: '");
	templateBuilder.Append(Utils.ObjectToStr(jsnoncestr));
	templateBuilder.Append("', // 必填，生成签名的随机串\r\n            signature: '");
	templateBuilder.Append(Utils.ObjectToStr(mp_signature));
	templateBuilder.Append("',// 必填，签名，见附录1\r\n            jsApiList: ['chooseWXPay'] // 必填，需要使用的JS接口列表，所有JS接口列表见附录2\r\n        });    \r\n        wx.error(function(res){\r\n            // config信息验证失败会执行error函数，如签名过期导致验证失败，具体错误信息可以打开config的debug模式查看，\r\n            //也可以在返回的res参数中查看，对于SPA可以在这里更新签名。\r\n\r\n        });\r\n        var auto_areas = new Array();\r\n        ");
	int drAreaOffline__loop__id=0;
	foreach(DataRow drAreaOffline in dtOfflineArea.Rows)
	{
		drAreaOffline__loop__id++;


	templateBuilder.Append("\r\n        auto_areas.push({ 'title': '" + Utils.ObjectToStr(drAreaOffline["Title"]) + "', 'location': { 'lat': " + Utils.ObjectToStr(drAreaOffline["lat"]) + ", 'lng': " + Utils.ObjectToStr(drAreaOffline["lng"]) + " },'id':" + Utils.ObjectToStr(drAreaOffline["id"]) + "});\r\n        ");
	}	//end loop


	templateBuilder.Append("\r\n    </");
	templateBuilder.Append("script>\r\n</head>\r\n<body style=\"background-repeat:no-repeat;background-size:100% 100%;\">\r\n    <div id=\"DivLocker\" style=\"position:absolute;background-color:rgba(0, 0, 0,0.5);filter:alpha(opacity=30);overflow:hidden;z-index:5;display:none;\">\r\n        <img src=\"/templates/green/img/mp_loading.gif\" style=\"margin:100px auto;display:block;\" />\r\n    </div>\r\n    <div id=\"DivChangeFocus\" style=\"position:absolute;background-color:rgba(0, 0, 0,0.5);filter:alpha(opacity=30);overflow:hidden;z-index:5;display:none;\">\r\n        <img src=\"/templates/green/img/changeweixin.jpg\" style=\"margin:10px auto;display:block;\" />\r\n    </div>\r\n    \r\n    <div id=\"DivConfirm\" style=\"position:absolute;background-color:rgba(0, 0, 0,0.8);filter:alpha(opacity=30);overflow:hidden;z-index:5;display:none;\">\r\n        <div style=\"width: 100%;margin: 40% 0%;text-align: center;\">\r\n            <span style=\"color: red;font-size: 28px;font-weight: bold;\" id=\"confirmTip1\">【特别提醒】<br />请核对当前下单店铺地址<br />下单即做 无法改退单</span>\r\n            <div style=\"background-color: rgb(233,84,18);width: 280px;margin: 15px auto;padding: 10px 0px;border-radius: 15px;\">\r\n                <span style=\"font-weight: bold;font-size: 28px;color: white;display: block;\">馍王<font id=\"confirmTitle\"></font></span>\r\n                <i style=\"border-top: 1px dotted white;width: 40px;display: inline-block;vertical-align: middle;\"></i>\r\n                <span style=\"color: white;\" id=\"confirmAddress\"></span>\r\n                <i style=\"border-top: 1px dotted white;width: 40px;display: inline-block;vertical-align: middle;\"></i>\r\n            </div>\r\n            <div style=\"color: white;font-size: 28px;display:none;margin: -10px 0px 10px;\" id=\"confirmTip2\">完成配送及售后</div>\r\n            <a id=\"confirmAccept\" href=\"javascript:$('#DivConfirm').hide();ShowHeadLine();\" style=\"display: block;color: white;background-color: red;border: 1px solid white;height: 60px;width: 60px;line-height: 60px;font-size: 24px;border-radius: 50%;margin: 0px auto;text-decoration: none;margin-bottom: 10px;\">是</a>\r\n            <a href=\"javascript:void(0)\" id=\"btnConfirmReturn\" style=\"display: block;color: white;background-color: rgb(128,78,43);border: 1px solid white;height: 60px;width: 60px;line-height: 60px;font-size: 24px;border-radius: 50%;margin: 0px auto;text-decoration: none;\">不是</a>\r\n        </div>\r\n    </div>\r\n    <div id=\"DivTipAreaAddress\" style=\"position:absolute;background-color:rgba(0, 0, 0,0.8);filter:alpha(opacity=30);overflow:hidden;z-index:5;display:none;\">\r\n        <div style=\"width: 100%;margin: 40% 0%;text-align: center;\">\r\n            <span style=\"color:white; font-size: 28px;font-weight: bold;\" id=\"confirmTip1\">您的外卖将由</span>\r\n            <div style=\"background-color: rgb(233,84,18);width: 280px;margin: 15px auto;padding: 10px 0px;border-radius: 15px;\">\r\n                <span style=\"font-weight: bold;font-size: 28px;color: white;display: block;\">馍王<font id=\"TipAreaAddressTitle\"></font></span>\r\n                <i style=\"border-top: 1px dotted white;width: 40px;display: inline-block;vertical-align: middle;\"></i>\r\n                <span style=\"color: white;\" id=\"TipAreaAddressAddress\"></span>\r\n                <i style=\"border-top: 1px dotted white;width: 40px;display: inline-block;vertical-align: middle;\"></i>\r\n            </div>\r\n            <div style=\"color: white;font-size: 28px;margin: -10px 0px 10px;\" >完成配送及售后</div>\r\n        </div>\r\n    </div>\r\n    <div id=\"DivHeadLine\" style=\"position:absolute;background-color:rgba(0, 0, 0,0.8);filter:alpha(opacity=30);overflow:hidden;z-index:5;display:none;\">\r\n        <div style=\"width: 100%;margin: 40% 0%;text-align: center;\">\r\n            <span style=\"color: white;font-size: 28px;color:rgb(249,232,90);\" id=\"headlineTitle\">半价5元</span>\r\n            <img src=\"\" id=\"DivHeadLineImg\" style=\"width: 200px;margin: 10px auto;display:block;\" />\r\n            <a href=\"javascript:void(0);\" class=\"btn\" onclick=\"$('#btnCarnivalOffline').click();$('#DivHeadLine').hide();\">先加入购物车</a>\r\n            <a href=\"javascript:void(0);\" class=\"btn\" style=\"background-color: rgb(147,97,62);\" onclick=\"$('#divCarnivalOffline .item .unselect').removeClass('cover');$('#DivHeadLine').hide();\">不需要 直接点餐</a>\r\n        </div>\r\n    </div>\r\n        <!--在线/堂吃选择Begin-->\r\n        <div class=\"typechoice\" style=\"" + Utils.ObjectToStr(divShowAndHide[0]) + "\">\r\n            <div class=\"logo_top\"></div>\r\n            <div class=\"main\">\r\n                <ul>\r\n                    <li class=\"item\" style=\"background-color: white;\">\r\n                        <img src=\"/templates/green/holiday/logo_waimai.png\" style=\"width: 75px;margin-top: 18px;\" />\r\n                        <span style=\"display: block;color: rgb(97,57,28);font-size: 21px;font-weight: bold;\">极速外卖</span>\r\n                    </li>\r\n                    <li class=\"item\" style=\"background-color: white; \">\r\n                        <img src=\"/templates/green/holiday/logo_tangchi.png\" style=\"width: 115px;margin-top: 22px;margin-bottom: 8px;\" />\r\n                        <span style=\"display: block;color: rgb(97,57,28);font-size: 21px;font-weight: bold;\">堂吃/打包</span>\r\n                        <span class=\"quan\">券</span>\r\n                    </li>\r\n                </ul>\r\n\r\n            </div>\r\n            <div style=\"position: absolute; bottom: 20px; width: 100%; text-align: center;\">\r\n                <img src=\"");
	templateBuilder.Append(Utils.ObjectToStr(userModel.avatar));
	templateBuilder.Append("\" style=\"width: 50px;border-radius: 50%;\">\r\n                <span style=\"display: block;color: white;font-size: 12px;\">");
	templateBuilder.Append(Utils.ObjectToStr(userModel.nick_name));
	templateBuilder.Append("</span>\r\n            </div>\r\n        </div>\r\n        <!--在线/堂吃选择End-->\r\n        <!--区域选择Begin-->\r\n        <div id=\"divArea\" style=\"" + Utils.ObjectToStr(divShowAndHide[1]) + "\" class=\"online\">\r\n            <ul class=\"area_select\">\r\n                ");
	int drArea__loop__id=0;
	foreach(DataRow drArea in dtArea.Rows)
	{
		drArea__loop__id++;


	templateBuilder.Append("\r\n                <li class=\"item \" data-id=\"" + Utils.ObjectToStr(drArea["Id"]) + "\" data-content=\"" + Utils.ObjectToStr(drArea["Description"]) + "\">" + Utils.ObjectToStr(drArea["Title"]) + "</li>\r\n                ");
	}	//end loop


	templateBuilder.Append("\r\n            </ul>\r\n            <div class=\"area_confirm\">\r\n                <div class=\"area_detail\">\r\n                    ");
	if (dtArea.Rows.Count>0)
	{

	templateBuilder.Append(dtArea.Rows[0]["Description"].ToString().ToString());
	

	}	//end if


	templateBuilder.Append("\r\n                </div>\r\n                <div class=\"confirm_div\">\r\n                    请确认所在区域\r\n                </div>\r\n            </div>\r\n        </div>\r\n        <!--区域选择End-->\r\n        <!--地址定位Begin-->\r\n        <div class=\"input_address_main\" style=\"display:none;\" id=\"divInputAddress\">\r\n            <input type=\"text\" placeholder=\"输入您所在地标（如小区/写字楼/医院等），等下拉框出来后选中\" class=\"txt\" id=\"txtInputAddres\" style=\"width:98%;\" />\r\n            <!--<input type=\"text\" placeholder=\"房间号等\" class=\"txt\" id=\"txtInputDetail\" style=\"width:30%;\"/>-->\r\n            <div style=\"text-align: center;\">\r\n                <a class=\"btnCircle\" id=\"btnStartOrder\" style=\"display:none;\">确定</a>\r\n            </div>\r\n        </div>\r\n        <!--地址定位End-->\r\n        <!--地址定位Begin-->\r\n        <div class=\"user_address\" style=\"display:none;\" id=\"divUserAddress\">\r\n            <ul>\r\n            </ul>\r\n            <div style=\"text-align: center;\">\r\n                <a class=\"btnCircle cancel\" id=\"btnCancelUserAddress\">新增地址</a>\r\n            </div>\r\n        </div>\r\n        <!--地址定位End-->\r\n        <!--区域选择Begin-->\r\n        <div id=\"divOfflineArea\" style=\"" + Utils.ObjectToStr(divShowAndHide[2]) + "\" class=\"offline\">\r\n            <ul class=\"area_select\"></ul>\r\n            <div style=\"text-align: center;\">\r\n                <a class=\"btnCircle\" id=\"btnStartOfflineOrder\">确定</a>\r\n            </div>\r\n        </div>\r\n        <div id=\"divOfflineAreaError\" style=\"display:none;\" class=\"offline input_address_main\">\r\n            <input type=\"text\" placeholder=\"定位失败，请咨询前台操作\" class=\"txt\" id=\"txtInputAddresTake\" style=\"width:98%;\" />\r\n            <div style=\"text-align: center;\">\r\n                <a class=\"btnCircle\" style=\"line-height:50px;\">确认</a>\r\n            </div>\r\n        </div>\r\n        <!--区域选择End-->\r\n        <!--Main Begin-->\r\n        <div style=\"overflow:hidden;\">\r\n            <div id=\"divGoods\" style=\"display: none; transform:translate3d(0px, 0px, 0px); -ms-transform: translate3d(0px, 0px, 0px); -moz-transform: translate3d(0px, 0px, 0px); -webkit-transform: translate3d(0px, 0px, 0px); -o-transform: translate3d(0px, 0px, 0px); transition: all 0.5s ease-in-out;\">\r\n                <div class=\"transdiv\" style=\"background-color:white;\">\r\n                    <div class=\"order\">\r\n                        <div class=\"detail\" style=\"text-align: center;\">\r\n                            <span>没有选购餐品</span>\r\n                        </div>\r\n                    </div>\r\n                    ");
	if (modelCompany!=null)
	{

	if (config.enable_waimai_vip)
	{

	templateBuilder.Append("\r\n                    <div class=\"content\" id=\"divVipOnline\" style=\"display:none;\">\r\n                        <img src=\"/templates/green/joincompany/join_company_1.png\" style=\"width:100%;height:100%;position:relative;\" />\r\n                        <div class=\"cont show\">\r\n                            <table>\r\n                                <tr>\r\n                                    <td colspan=\"3\" style=\"color: rgb(72,24,42);font-size: 16px;padding-left: 55px;font-weight: bold;\">\r\n                                        ");
	templateBuilder.Append(Utils.ObjectToStr(modelCompany.CompanyName));
	templateBuilder.Append("VIP卡\r\n                                    </td>\r\n                                </tr>\r\n                                <tr>\r\n                                    <td class=\"tdTip\" style=\"vertical-align: bottom;\"><div class=\"purple\"><span>满99减5元/</span><span>外卖</span></div></td>\r\n                                    <td rowspan=\"2\">\r\n                                        <div class=\"person_info\">\r\n                                            <table>\r\n                                                <tr>\r\n                                                    <td rowspan=\"2\" style=\"width: 38px;color: rgba(0,161,233,1);font-size: 27px;text-align: center;\">");
	templateBuilder.Append(Utils.ObjectToStr(modelCompany.PersonCount));
	templateBuilder.Append("</td>\r\n                                                    <td style=\"font-size: 12px;color: rgba(0,161,233,1);\">位</td>\r\n                                                </tr>\r\n                                                <tr>\r\n                                                    <td style=\"font-size: 12px;color: rgba(0,161,233,1);\">同事</td>\r\n                                                </tr>\r\n                                                <tr>\r\n                                                    <td rowspan=\"2\" style=\"width: 38px;color: rgba(234,85,22,1);font-size: 27px;text-align: center;\">");
	templateBuilder.Append(Utils.ObjectToStr(avaliableCompanyAmount));
	templateBuilder.Append("</td>\r\n                                                    <td style=\"font-size: 12px;color: rgba(234,85,22,1);\">余额</td>\r\n                                                </tr>\r\n                                                <tr>\r\n                                                    <td style=\"font-size: 12px;color: rgba(234,85,22,1);\">元</td>\r\n                                                </tr>\r\n                                            </table>\r\n                                        </div>\r\n                                    </td>\r\n                                </tr>\r\n                                <tr>\r\n                                    <td style=\"vertical-align: top;\"><div class=\"purple\"><span>免外送费</span></div></td>\r\n                                </tr>\r\n                            </table>\r\n                        </div>\r\n                    </div>\r\n                    ");
	}	//end if


	templateBuilder.Append("\r\n                    <div class=\"content\" id=\"divVipOffline\" style=\"display:none;\">\r\n                        <img src=\"/templates/green/joincompany/join_company_1.png\" style=\"width:100%;height:100%;position:relative;\" />\r\n                        <div class=\"cont show\">\r\n                            <table>\r\n                                <tr>\r\n                                    <td colspan=\"3\" style=\"color: rgb(72,24,42);font-size: 16px;padding-left: 55px;font-weight: bold;\">\r\n                                        ");
	templateBuilder.Append(Utils.ObjectToStr(modelCompany.CompanyName));
	templateBuilder.Append("VIP卡\r\n                                    </td>\r\n                                </tr>\r\n                                <tr>\r\n                                    <td  class=\"tdTip\" style=\"vertical-align: bottom;\"><div class=\"red\"><span>满29减2元/</span><span>堂吃</span></div></td>\r\n                                    <td rowspan=\"2\">\r\n                                        <div class=\"person_info\">\r\n                                            <table>\r\n                                                <tr>\r\n                                                    <td rowspan=\"2\" style=\"width: 38px;color: rgba(0,161,233,1);font-size: 27px;text-align: center;\">");
	templateBuilder.Append(Utils.ObjectToStr(modelCompany.PersonCount));
	templateBuilder.Append("</td>\r\n                                                    <td style=\"font-size: 12px;color: rgba(0,161,233,1);\">位</td>\r\n                                                </tr>\r\n                                                <tr>\r\n                                                    <td style=\"font-size: 12px;color: rgba(0,161,233,1);\">同事</td>\r\n                                                </tr>\r\n                                                <tr>\r\n                                                    <td rowspan=\"2\" style=\"width: 38px;color: rgba(234,85,22,1);font-size: 27px;text-align: center;\">");
	templateBuilder.Append(Utils.ObjectToStr(avaliableCompanyAmount));
	templateBuilder.Append("</td>\r\n                                                    <td style=\"font-size: 12px;color: rgba(234,85,22,1);\">余额</td>\r\n                                                </tr>\r\n                                                <tr>\r\n                                                    <td style=\"font-size: 12px;color: rgba(234,85,22,1);\">元</td>\r\n                                                </tr>\r\n                                            </table>\r\n                                        </div>\r\n                                    </td>\r\n                                </tr>\r\n                                <tr>\r\n                                    <td style=\"vertical-align: top;\"><div class=\"red\"><span>抵扣现磨饮料</span></div></td>\r\n\r\n                                </tr>\r\n                            </table>\r\n                        </div>\r\n                    </div>\r\n                    ");
	}	//end if


	templateBuilder.Append("\r\n                    <!--堂吃优惠活动 Begin-->\r\n                    <div class=\"carnival\" style=\"display:none;\" id=\"divCarnivalOffline\">\r\n                        <div class=\"main\">\r\n\r\n                            <span class=\"btn active\" id=\"btnCarnivalOffline\" style=\"display: none;\">加入购物车</span>\r\n                            <span class=\"btn active\" id=\"btnCarnivalOfflineClose\" style=\"display: none;\">不需要</span>\r\n                        </div>\r\n                    </div>\r\n                    <!--堂吃优惠活动 End-->\r\n                    <div class=\"attach_info\">\r\n                        <div class=\"line\">\r\n                            <input type=\"text\" id=\"phone\" name=\"phone\" placeholder=\"手机/请核对\" value=\"");
	templateBuilder.Append(Utils.ObjectToStr(userModel.telphone));
	templateBuilder.Append("\" style=\"width: 60%;display: inline-block;\" />\r\n                            <input type=\"text\" id=\"nickname\" name=\"nickname\" placeholder=\"姓名\" value=\"");
	templateBuilder.Append(Utils.ObjectToStr(userModel.qq));
	templateBuilder.Append("\" style=\"width: 29%;display: inline-block;\" />\r\n                        </div>\r\n                        <div class=\"line\">\r\n                            <input type=\"text\" id=\"address\" name=\"address\" placeholder=\"地址/请核对\" value=\"\" style=\"width:95%\" maxlength=\"40\" />\r\n                        </div>\r\n                        <div class=\"line\">\r\n                            <textarea name=\"msg\" id=\"message\" rows=\"5\" placeholder=\"配送时间：11:00-20:00. 外送无法满足个性化条件，请理解！\" maxlength=\"18\"></textarea>\r\n                        </div>\r\n                    </div>\r\n                    <div class=\"padding\"></div>\r\n\r\n\r\n                </div>\r\n                <div class=\"transdiv\" style=\"left:100%;\">\r\n                    <div style=\"overflow: hidden;\">\r\n                        <ul class=\"menu\">\r\n                            ");
	int drcategory__loop__id=0;
	foreach(DataRow drcategory in dtcategory.Rows)
	{
		drcategory__loop__id++;


	templateBuilder.Append("\r\n                            <li data-id=\"" + Utils.ObjectToStr(drcategory["id"]) + "\" class=\"");
	if (drcategory__loop__id==1)
	{

	templateBuilder.Append("active");
	}	//end if


	if (drcategory__loop__id>4)
	{

	templateBuilder.Append("large");
	}	//end if


	templateBuilder.Append("\">\r\n                                " + Utils.ObjectToStr(drcategory["title"]) + "\r\n                            </li>\r\n                            ");
	}	//end loop


	templateBuilder.Append("\r\n                        </ul>\r\n                    </div>\r\n                    <div class=\"suits\">\r\n\r\n                    </div>\r\n                    <div class=\"page_mask\"></div>\r\n                </div>\r\n                <div class=\"transdiv\" style=\"left:200%;display:none;\">\r\n                    <div class=\"address\" id=\"orderaddress\" style=\"padding-left: 15px;margin-top:10px;\">\r\n\r\n                    </div>\r\n                    <div class=\"phone\" id=\"ordertelphone\" style=\"padding-left: 15px;\">\r\n\r\n                    </div>\r\n                    <div class=\"od_status\" style=\"padding-left: 15px;\">\r\n                        <span class=\"order_date\" id=\"order_time_line\"></span>\r\n                        <span class=\"order_id\" id=\"orderno\"></span>\r\n                    </div>\r\n                    <ul class=\"o_list\"></ul>\r\n                    <div class=\"total_price\">\r\n                        <span class=\"total\" id=\"total\">总计：</span>\r\n                    </div>\r\n                    <div class=\"notice\" id=\"ordermessage\">\r\n\r\n                    </div>\r\n                    <div class=\"progress\">\r\n                        <div class=\"success p-box gray\">\r\n                            <div>下单成功</div>\r\n                            <span>ORDER SUCCESS</span>\r\n                        </div>\r\n                        <div class=\"next_step gray\" style=\"display:none;\">\r\n\r\n                        </div>\r\n                        <div class=\"make p-box gray\">\r\n                            <div>正在配单</div>\r\n                            <span>MAKE THE ORDER</span>\r\n                        </div>\r\n                        <div class=\"next_step gray\" style=\"display:none;\"></div>\r\n                        <div class=\"sent p-box gray\">\r\n                            <div>已送出</div>\r\n                            <span>SEND OUT</span>\r\n                        </div>\r\n                    </div>\r\n\r\n                </div>\r\n            </div>\r\n        </div>\r\n        <!--Main End-->\r\n        <!--按钮 Begin-->\r\n        <div class=\"bottom_nav\" style=\"display:none;overflow:hidden;\">\r\n            ");
	if (tab==2)
	{

	}
	else
	{

	templateBuilder.Append("\r\n            <div id=\"divShopClose\" style=\"transform:translate3d(0px, 0px, 0px);-ms-transform:translate3d(0px, 0px, 0px);-moz-transform:translate3d(0px, 0px, 0px);-webkit-transform:translate3d(0px, 0px, 0px);-o-transform:translate3d(0px, 0px, 0px);  transition: all 0.5s ease-in-out; height: 43px; width: 100%; display: none;\">\r\n                <img src=\"/templates/green/img/mp_close.png\" />\r\n            </div>\r\n            <div id=\"divHereClose\" style=\"transform:translate3d(0px, 0px, 0px);-ms-transform:translate3d(0px, 0px, 0px);-moz-transform:translate3d(0px, 0px, 0px);-webkit-transform:translate3d(0px, 0px, 0px);-o-transform:translate3d(0px, 0px, 0px);  transition: all 0.5s ease-in-out; height: 43px; width: 100%; display: none;\">\r\n                <img src=\"/templates/green/img/mp_here_close.jpg\" />\r\n            </div>\r\n            <div id=\"divAreaClose\" style=\"transform:translate3d(0px, 0px, 0px);-ms-transform:translate3d(0px, 0px, 0px);-moz-transform:translate3d(0px, 0px, 0px);-webkit-transform:translate3d(0px, 0px, 0px);-o-transform:translate3d(0px, 0px, 0px);  transition: all 0.5s ease-in-out; height: 43px; width: 100%;display:none;\">\r\n                <img src=\"/templates/green/img/mp_maxorder.png\" />\r\n            </div>\r\n            <div id=\"divBottomBtn\" style=\"transform: translate3d(0px, -43px, 0px); -ms-transform: translate3d(0px, -43px, 0px); -moz-transform: translate3d(0px, -43px, 0px); -webkit-transform: translate3d(0px, -43px, 0px); -o-transform: translate3d(0px, -43px, 0px); transition: all 0.5s ease-in-out; height: 43px; width: 100%;\">\r\n                <div class=\"btns\" id=\"divCartOnline\">\r\n                    <div class=\"b_continue\">继续订餐</div>\r\n                    <div class=\"btn_list\">\r\n                        <div class=\"b_pay_offline b_full_width\" style=\"display:none;\" data-isshow=\"true\">餐到付款</div>\r\n                        <!--<div class=\"b_pay_online b_full_width\" style=\"display:none;\">支付宝支付</div>-->\r\n                        <div class=\"b_pay_mppay b_full_width\" style=\"display:none;\" data-isshow=\"true\">微信支付</div>\r\n                        <div class=\"b_submit b_full_width\">提交订单<span class=\"count\" id=\"spanCount2\">0</span>元</div>\r\n                    </div>\r\n                </div>\r\n                <div class=\"btns\" id=\"divCartOffline\" style=\"display:none;\">\r\n                    <div class=\"btn_list\">\r\n                        <div class=\"b_pay_offline b_half_width\" style=\"display: block; top: -46px;right:0px;width:100%;\" id=\"btnOfflineOutCompany\">一会到<i class=\"b_yes\" style=\"margin-left: 10px;\"></i>\r\n                        </div>\r\n                        <div class=\"b_continue b_full_width\" id=\"btnOfflineContinue\">继续订餐</div>\r\n                    </div>\r\n                    <div class=\"btn_list\">\r\n                        <div class=\"b_pay_offline b_half_width\" style=\"display: block; top: -46px;right:0px;width:100%;\" id=\"btnOfflineTakeOut\">外带<i class=\"b_yes\" style=\"margin-left: 10px;\"></i></div>\r\n                        <div class=\"b_submit b_full_width\" id=\"btnOfflineSubmit\"><font>立即支付</font><span class=\"count\" id=\"spanCount3\">0</span>元</div>\r\n                    </div>\r\n                </div>\r\n                <div class=\"bottom_button\">\r\n                    <div class=\"button_order\" style=\"margin-left: 0px;\">\r\n\r\n                    </div>\r\n                    <div class=\"button_cart\">查看购物车<span class=\"count\" id=\"spanCount\">0</span>元\r\n                    </div>\r\n\r\n                </div>\r\n                <div class=\"btns\">\r\n                    <div class=\"b_cart\">查看购物车</div>\r\n                    <div class=\"b_history\">继续订餐</div>\r\n                </div>\r\n            </div>\r\n            ");
	}	//end if


	templateBuilder.Append("\r\n        </div>\r\n        <!--按钮 End-->\r\n        <!--订餐成功 Begin-->\r\n        <div class=\"order_complete_window success\" style=\"display: none; height: 1443px;\">\r\n            <div class=\"order_status_window\">\r\n                <div class=\"success_f1\">\r\n                    <div class=\"thank_you\">\r\n                    </div>\r\n                    <b class=\"status_txt\">订餐成功</b> <span class=\"auto_close\">5秒自动关闭</span> <span class=\"estimate_ok\">\r\n                        配送时间：11:00-20:00,预计30-50分钟到达\r\n                    </span>\r\n                    <div class=\"txt\" style=\"margin-top: 10px; margin-bottom: 5px;\">\r\n                        您可在\r\n                    </div>\r\n                    <div class=\"btn_myorder\">\r\n                        <div class=\"cn\">\r\n                            订单处理\r\n                        </div>\r\n                    </div>\r\n                    <div class=\"txt\" style=\"margin-top:5px;\">\r\n                        中查询订单详情\r\n                    </div>\r\n                </div>\r\n            </div>\r\n        </div>\r\n        <!--订餐成功 End-->\r\n        <!--堂吃订餐成功 Begin-->\r\n        <div class=\"order_complete_window success_forhere\" style=\"display: none;height: 1443px;\">\r\n            <div class=\"order_status_window\">\r\n                <div class=\"success_f1\">\r\n                    <div class=\"thank_you\">\r\n                    </div>\r\n                    <b class=\"status_txt\">订餐成功</b> <span class=\"estimate_ok\">\r\n                        请记住您的取餐号，并及时至收银台取餐，感谢光临！\r\n                    </span>\r\n                    <div class=\"txt\" style=\"margin-top:5px;\">\r\n                        <span style=\"display: block; color: red; font-size: 24px;\">取餐号</span>\r\n                        <span style=\"display: block; color: red; font-size: 40px; font-weight: bold; \" id=\"lblForHere\"></span>\r\n                    </div>\r\n                </div>\r\n            </div>\r\n        </div>\r\n        <!--堂吃订餐成功 End-->\r\n        <!--订餐失败 Begin-->\r\n        <div class=\"order_complete_window fail\" style=\"display: none; height: 1443px;\">\r\n            <div class=\"order_status_window\">\r\n                <div class=\"success_f1\">\r\n                    <div class=\"fail_you\">\r\n                    </div>\r\n                    <b class=\"status_txt\">订餐失败,请再次确认</b>\r\n                </div>\r\n            </div>\r\n        </div>\r\n        <!--订餐失败 End-->\r\n        <!--延时配送 Begin-->\r\n        <div class=\"order_complete_window late\" style=\"display:none;height: 100%;\">\r\n            <div class=\"order_status_window\" style=\"text-align:center;\">\r\n                <div class=\"success_f1\">\r\n                    <div class=\"late\" style=\"background: url(/templates/green/img/weipopup.jpg) center top no-repeat; \">\r\n                    </div>\r\n                </div>\r\n                <span style=\"display: inline-block; width: 170px; color: white; background: red; margin-top: 10px; height: 26px; line-height: 25px; border-radius: 10px;\" onclick=\"$('.order_complete_window.late').hide()\">我已知晓</span>\r\n            </div>\r\n        </div>\r\n        <!--延时配送 End-->\r\n        <!--区域关闭 Begin-->\r\n        <div class=\"order_complete_window arealock\" style=\"display:none;height: 100%;\">\r\n            <div class=\"order_status_window\" style=\"text-align:center;\">\r\n                <div class=\"success_f1\">\r\n                    <div class=\"arealock\" style=\"background: url(/templates/green/img/weipopuplock.jpg) center top no-repeat; \">\r\n                    </div>\r\n                </div>\r\n                <span style=\"display: inline-block; width: 170px; color: white; background: red; margin-top: 10px; height: 26px; line-height: 25px; border-radius: 10px;\" onclick=\"$('.order_complete_window.arealock').hide()\">我已知晓</span>\r\n            </div>\r\n        </div>\r\n        <!--区域关闭 End-->\r\n        ");
	if (carnivalDetail!=null)
	{

	templateBuilder.Append("\r\n        <!--满送活动 Begin-->\r\n        <div class=\"carnival\" style=\"display:none;background-color: rgba(0,0,0,0.9);\" id=\"divCarnivalOnline\">\r\n            <div class=\"main\">\r\n                <span class=\"num\">");
	templateBuilder.Append((carnivalUserModel!=null?carnivalUserModel.Num:0).ToString());
	

	templateBuilder.Append("</span>\r\n                <span class=\"white\">您当前累计订单次数</span>\r\n                <span class=\"date white\">本次活动截止：");
	templateBuilder.Append(carnivalModel.EndTime.ToString("yyyy年MM月dd日").ToString());
	

	templateBuilder.Append("</span>\r\n                <ul style=\"overflow: hidden; margin: 10px auto 20px auto; \">\r\n                    ");
	int dr__loop__id=0;
	foreach(DataRow dr in carnivalDetail.Rows)
	{
		dr__loop__id++;


	templateBuilder.Append("\r\n                    <li style=\"height:108px;margin-right: -4px;\">\r\n                        <div class=\"item one\" data-id=\"" + Utils.ObjectToStr(dr["id"]) + "\" data-title=\"满" + Utils.ObjectToStr(dr["change_nums"]) + "次送" + Utils.ObjectToStr(dr["title"]) + "\" data-change=\"" + Utils.ObjectToStr(dr["change_nums"]) + "\" style=\"background: url(" + Utils.ObjectToStr(dr["mp_img_url"]) + ");\">\r\n                            <div class=\"cover active\" style=\"display:none;\"></div>\r\n                            <div class=\"name\">\r\n                                " + Utils.ObjectToStr(dr["title"]) + "\r\n                            </div>\r\n                        </div>\r\n                        <div style=\"border: 1px solid rgb(149,135,132); border-radius: 10px; margin-top: 3px;\">\r\n                            <span style=\"color: white;\">需" + Utils.ObjectToStr(dr["change_nums"]) + "次</span>\r\n                        </div>\r\n                    </li>\r\n                    ");
	}	//end loop


	templateBuilder.Append("\r\n                </ul>\r\n                <span class=\"btn\" id=\"btnCarnivalOnline\">确定兑换</span>\r\n                <span class=\"btn active\" id=\"btnCarnivalClose\">关闭</span>\r\n                <span class=\"notice\">*当天多次订餐累计为1次 <br />*请在满足条件后自主兑换下单,逾期则自动清零<br />LUCKINGPIE拥有最终解释权</span>\r\n            </div>\r\n        </div>\r\n        <!--满送活动 End-->\r\n        ");
	}	//end if


	templateBuilder.Append("\r\n\r\n        <input type=\"hidden\" id=\"hfAreaId\" value=\"");
	templateBuilder.Append(Utils.ObjectToStr(defaultAreaId));
	templateBuilder.Append("\" />\r\n        <input type=\"hidden\" id=\"hfDisamountId\" value=\"\" />\r\n        <input type=\"hidden\" id=\"hfCookie\" />\r\n        <input type=\"hidden\" id=\"hfOpenId\" value=\"");
	templateBuilder.Append(Utils.ObjectToStr(openid));
	templateBuilder.Append("\" />\r\n        <input type=\"hidden\" id=\"hflowamount\" value=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.lowamount));
	templateBuilder.Append("\" />\r\n        <input type=\"hidden\" id=\"hflowamount_2\" value=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.lowamount_2));
	templateBuilder.Append("\" />\r\n        <input type=\"hidden\" id=\"hffreedisamount\" value=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.freedisamount));
	templateBuilder.Append("\" />\r\n        <input type=\"hidden\" id=\"hfdisamount\" value=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.disamount));
	templateBuilder.Append("\" />\r\n        <input type=\"hidden\" id=\"hfClass\" value=\"\" />\r\n        <input type=\"hidden\" id=\"hfBackground1\" value=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.mp_backgroundimage));
	templateBuilder.Append("\" />\r\n        <input type=\"hidden\" id=\"hfBackground2\" value=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.mp_backgroundimage2));
	templateBuilder.Append("\" />\r\n        <input type=\"hidden\" id=\"hfTab\" value=\"");
	templateBuilder.Append(Utils.ObjectToStr(tab));
	templateBuilder.Append("\" />\r\n        <input type=\"hidden\" id=\"hfIsRun\" value=\"");
	templateBuilder.Append(Utils.ObjectToStr(GetIsRun()));
	templateBuilder.Append("\" />\r\n        <input type=\"hidden\" id=\"hfHereIsRun\" value=\"");
	templateBuilder.Append(Utils.ObjectToStr(GetForHereIsRun()));
	templateBuilder.Append("\" />\r\n        <input type=\"hidden\" id=\"hfAdditional\" value=\"");
	templateBuilder.Append(Utils.ObjectToStr(additional));
	templateBuilder.Append("\" />\r\n        <input type=\"hidden\" id=\"hfLastOnlineAreaId\" value=\"");
	templateBuilder.Append(Utils.ObjectToStr(lastOnlineAreaId));
	templateBuilder.Append("\" />\r\n        <input type=\"hidden\" id=\"hfLastOnlineArea\" value=\"");
	templateBuilder.Append(Utils.ObjectToStr(lastOnlineArea));
	templateBuilder.Append("\" />\r\n        <input type=\"hidden\" id=\"hfLastOfflineAreaId\" value=\"");
	templateBuilder.Append(Utils.ObjectToStr(lastOfflineAreaId));
	templateBuilder.Append("\" />\r\n        <input type=\"hidden\" id=\"hfLastOfflineArea\" value=\"");
	templateBuilder.Append(Utils.ObjectToStr(lastOfflineArea));
	templateBuilder.Append("\" />\r\n        <input type=\"hidden\" id=\"hflastOfflineAreaAddress\" value=\"");
	templateBuilder.Append(Utils.ObjectToStr(lastOfflineAreaAddress));
	templateBuilder.Append("\" />\r\n    \r\n        <input type=\"hidden\" id=\"hfState\" value=\"");
	templateBuilder.Append(Utils.ObjectToStr(state));
	templateBuilder.Append("\" />\r\n        <input type=\"hidden\" id=\"hfDistributionArea\" value=\"");
	templateBuilder.Append(Utils.ObjectToStr(distributionArea));
	templateBuilder.Append("\" />\r\n        <input type=\"hidden\" id=\"hfLatLng\" />\r\n        <input type=\"hidden\" id=\"hfIsNoDiscountGood\" value=\"");
	templateBuilder.Append(Utils.ObjectToStr(isNoDiscountGood));
	templateBuilder.Append("\" />\r\n        <input type=\"hidden\" id=\"hfDeliverPayMaxAmountForMp\" value=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.DeliverPayMaxAmountForMp));
	templateBuilder.Append("\" />\r\n        <input type=\"hidden\" id=\"hfAvaliableCompanyAmount\" value=\"");
	templateBuilder.Append(Utils.ObjectToStr(avaliableCompanyAmount));
	templateBuilder.Append("\" />\r\n        <input type=\"hidden\" id=\"hfUserAddress\" value=\"");
	templateBuilder.Append(Utils.ObjectToStr(userAddress));
	templateBuilder.Append("\" />\r\n        <input type=\"hidden\" id=\"hfUserAddressId\" value=\"\" />\r\n        ");

	templateBuilder.Append("<div style=\"display:none;\">\r\n    <script type=\"text/javascript\">var cnzz_protocol = ((\"https:\" == document.location.protocol) ? \" https://\" : \" http://\");document.write(unescape(\"%3Cspan id='cnzz_stat_icon_1256906522'%3E%3C/span%3E%3Cscript src='\" + cnzz_protocol + \"s11.cnzz.com/z_stat.php%3Fid%3D1256906522%26show%3Dpic' type='text/javascript'%3E%3C/script%3E\"));</");
	templateBuilder.Append("script>\r\n</div>");


	templateBuilder.Append("\r\n</body>\r\n</html>\r\n");
	Response.Write(templateBuilder.ToString());
}
</script>
