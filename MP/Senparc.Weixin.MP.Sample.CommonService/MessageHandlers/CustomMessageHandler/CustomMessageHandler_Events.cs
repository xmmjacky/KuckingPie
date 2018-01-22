using System;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Senparc.Weixin.MP.Agent;
using Senparc.Weixin.Context;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Helpers;
using Senparc.Weixin.MP.MessageHandlers;
using DTcms.Common;
using System.Collections.Generic;
using System.Data;

namespace Senparc.Weixin.MP.Sample.CommonService.CustomMessageHandler
{
    /// <summary>
    /// 自定义MessageHandler
    /// </summary>
    public partial class CustomMessageHandler
    {

        DTcms.Model.siteconfig config = new DTcms.BLL.siteconfig().loadConfig(Utils.GetXmlMapPath(DTKeys.FILE_SITE_XML_CONFING));
        private static string strAppDomain = "http://www.4008317417.cn";
        public override IResponseMessageBase OnTextOrEventRequest(RequestMessageText requestMessage)
        {
            // 预处理文字或事件类型请求。
            // 这个请求是一个比较特殊的请求，通常用于统一处理来自文字或菜单按钮的同一个执行逻辑，
            // 会在执行OnTextRequest或OnEventRequest之前触发，具有以下一些特征：
            // 1、如果返回null，则继续执行OnTextRequest或OnEventRequest
            // 2、如果返回不为null，则终止执行OnTextRequest或OnEventRequest，返回最终ResponseMessage
            // 3、如果是事件，则会将RequestMessageEvent自动转为RequestMessageText类型，其中RequestMessageText.Content就是RequestMessageEvent.EventKey
            
            return null;//返回null，则继续执行OnTextRequest或OnEventRequest
        }

        public override IResponseMessageBase OnEvent_ScanRequest(RequestMessageEvent_Scan requestMessage)
        {
            //通过扫描关注
            Log.Info("进入 Scan事件");
            ResponseMessageNews strongResponseMessage = Subscribe(requestMessage.FromUserName, requestMessage.EventKey.Replace("qrscene_", ""));
            return strongResponseMessage;
        }

        /// <summary>
        /// 订阅（关注）事件
        /// </summary>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_SubscribeRequest(RequestMessageEvent_Subscribe requestMessage)
        {
            Log.Info("进入 Subscribe事件");
            ResponseMessageNews strongResponseMessage = Subscribe(requestMessage.FromUserName, requestMessage.EventKey.Replace("qrscene_",""));
            return strongResponseMessage;
        }

        private ResponseMessageNews Subscribe(string fromUserName,string EventKey)
        {
            var strongResponseMessage = CreateResponseMessage<ResponseMessageNews>();
            strongResponseMessage.Articles.Add(new Article()
            {
                Description = config.mp_welcomecontent,
                PicUrl = strAppDomain + config.mp_welcomeimage,
                Title = config.mp_welcometitle,
                Url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wxc5776b5774a3d010&amp;redirect_uri=http%3A%2F%2Fwww.4008317417.cn%2Fmp_index.aspx%3Fshowwxpaytitle%3D1&amp;response_type=code&amp;scope=snsapi_userinfo&amp;state=slave#wechat_redirect"
            });
            if (!string.IsNullOrEmpty(EventKey))
            {
                BookingFood.BLL.bf_company bllCompany = new BookingFood.BLL.bf_company();
                Log.Info("EventKey" + EventKey);
                BookingFood.Model.bf_company modelCompany = bllCompany.GetModel(int.Parse(EventKey));
                BookingFood.BLL.bf_user_voucher bllUserVoucher = new BookingFood.BLL.bf_user_voucher();
                #region 用户不存在需要新增
                DTcms.BLL.users bllUsers = new DTcms.BLL.users();
                DTcms.Model.users modelUser = bllUsers.GetModel(fromUserName);
                string accessToken = string.Empty;
                accessToken = Senparc.Weixin.MP.CommonAPIs
                            .AccessTokenContainer.TryGetToken(config.mp_slave_appid, config.mp_slave_appsecret);
                bool isNewCompany = false;
                if (modelUser == null)
                {
                    AdvancedAPIs.UserInfoJson userJson = AdvancedAPIs.User.Info(accessToken, fromUserName);
                    DTcms.Model.user_groups modelGroup = new DTcms.BLL.user_groups().GetDefault();
                    modelUser = new DTcms.Model.users();
                    modelUser.group_id = modelGroup.id;
                    modelUser.user_name = userJson.openid;
                    modelUser.nick_name = userJson.nickname;
                    modelUser.sex = userJson.sex == 1 ? "男" : userJson.sex == 2 ? "女" : "未知";
                    modelUser.avatar = userJson.headimgurl.Replace("/0", "/96");
                    modelUser.reg_time = DateTime.Now;
                    modelUser.password = DESEncrypt.Encrypt("111111");
                    modelUser.company_id = modelCompany.Id;
                    modelUser.id = bllUsers.Add(modelUser);
                    isNewCompany = true;
                }
                else
                {
                    //如果为加入过群组则赠送15元
                    if (modelUser.company_id == 0)
                    {
                        isNewCompany = true;
                    }
                    //用户存在需要修改群组ID
                    bllUsers.UpdateField(modelUser.id, "company_id=" + modelCompany.Id);
                }
                #endregion
                if(isNewCompany)
                {
                    bllUserVoucher.Add(new BookingFood.Model.bf_user_voucher()
                    {
                        AddTime = DateTime.Now,
                        Amount = 10,
                        CompanyId = modelCompany.Id,
                        ExpireTime = DateTime.Now.AddMonths(1),
                        UserId = modelUser.id
                    });
                }
                //判断是否加入过这个群组
                BookingFood.BLL.bf_company_user_log bllCompanyUser = new BookingFood.BLL.bf_company_user_log();
                Dictionary<string, Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem> tempData = new Dictionary<string, Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem>();
                if (bllCompanyUser.GetRecordCount("UserId=" + modelUser.id + " and CompanyId=" + modelCompany.Id) == 0)
                {
                    //群组人数+1
                    modelCompany.PersonCount += 1;
                    if (modelCompany.PersonCount == 30)
                    {
                        modelCompany.CompleteTime = DateTime.Now;
                    }
                    bllCompany.Update(modelCompany);
                    bllCompanyUser.Add(new BookingFood.Model.bf_company_user_log()
                    {
                        AddTime = DateTime.Now,
                        CompanyId = modelCompany.Id,
                        UserId = modelUser.id
                    });
                    //新通过的人取消默认的88元 17-01-15 改为本群组内所有人增加2元
                    //bllUserVoucher.Add(new BookingFood.Model.bf_user_voucher()
                    //{
                    //    AddTime = DateTime.Now,
                    //    Amount = 88,
                    //    CompanyId = modelCompany.Id,
                    //    ExpireTime = DateTime.Now.AddMonths(2),
                    //    UserId = modelUser.id
                    //});

                    //每新增加一位,则群组下所有人增加2元
                    DataTable dtUser = null;
                    if (isNewCompany)
                    {
                        dtUser = bllUsers.GetList(0, "company_id=" + modelCompany.Id + " and id!=" + modelUser.id, "id asc").Tables[0];
                    }
                    else
                    {
                        dtUser = bllUsers.GetList(0, "company_id=" + modelCompany.Id, "id asc").Tables[0];
                    }

                    if (modelCompany.Id == 347) dtUser = new DataTable();

                    foreach (DataRow item in dtUser.Rows)
                    {
                        bllUserVoucher.Add(new BookingFood.Model.bf_user_voucher()
                        {
                            AddTime = DateTime.Now,
                            Amount = 2,
                            CompanyId = modelCompany.Id,
                            ExpireTime = DateTime.Now.AddMonths(1),
                            UserId = int.Parse(item["id"].ToString())
                        });
                        //判断当日是否已经提醒
                        BookingFood.BLL.bf_company_user_amount_notice bllNotice = new BookingFood.BLL.bf_company_user_amount_notice();
                        if(bllNotice.GetRecordCount("UserId="+ item["id"].ToString() + " and (AddTime between '"
                            +DateTime.Now.ToString("yyyy-MM-dd")+ " 00:00:00' and '" + DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59')")==0)
                        {
                            bllNotice.Add(new BookingFood.Model.bf_company_user_amount_notice() {
                                AddTime=DateTime.Now,
                                UserId=int.Parse(item["id"].ToString())
                            });
                            decimal totalAmount = bllUserVoucher.GetModelList("UserId=" + int.Parse(item["id"].ToString()) + " and GetDate()<ExpireTime and Status=0").Sum(s => s.Amount);
                            tempData = new Dictionary<string, Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem>();
                            tempData.Add("first", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(config.vip_enough_title));//"您有新同事加入馍王贵司VIP，所有成员余额均增加3元！"
                            tempData.Add("keyword1", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(config.vip_join_welcome_name));//"中山西路1919号馍王"
                            tempData.Add("keyword2", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(totalAmount + "元"));
                            tempData.Add("remark", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(config.vip_enough_footer));//"赶紧介绍给更多的同事吧！"

                            accessToken = Senparc.Weixin.MP.CommonAPIs
                                .AccessTokenContainer.TryGetToken(config.mp_slave_appid, config.mp_slave_appsecret);
                            try
                            {
                                Senparc.Weixin.MP.AdvancedAPIs.Template.SendTemplateMessage(accessToken, item["user_name"].ToString(), "KPeoGA1cOZTDiqAf1IECWauABgqtz-P-y-zHME9hhp8"
                                , "#173177", "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wxc5776b5774a3d010&redirect_uri=http%3A%2F%2Fwww.4008317417.cn%2Fmp_join_company.aspx%3Fshowwxpaytitle%3D1&response_type=code&scope=snsapi_userinfo&state=slave#wechat_redirect", tempData);
                            }
                            catch (Exception ex) { }
                        }
                        
                    }
                }

                //超过50人 并且超过完成时间30天以上时发送消息给组内其他人

                //if (modelCompany.CompleteTime!=null && ((DateTime)modelCompany.CompleteTime).AddDays(20) < DateTime.Now)
                //{
                //    DataTable dt = bllUsers.GetList(0, "company_id=" + modelCompany.Id + " and id!="+modelUser.id, "id asc").Tables[0];
                    
                //    foreach (DataRow item in dt.Rows)
                //    {
                //        bllUserVoucher.Add(new BookingFood.Model.bf_user_voucher() {
                //            AddTime=DateTime.Now,
                //            Amount=3,
                //            CompanyId=modelCompany.Id,
                //            ExpireTime=DateTime.Now.AddMonths(2),
                //            UserId= modelUser.id
                //        });
                //        decimal totalAmount = bllUserVoucher.GetModelList("UserId=" + modelUser.id + " and CompanyId="+ modelCompany.Id + " and GetDate()<ExpireTime").Sum(s=>s.Amount);
                //        tempData = new Dictionary<string, Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem>();
                //        tempData.Add("first", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(config.vip_enough_title));//"您有新同事加入馍王贵司VIP，所有成员余额均增加3元！"
                //        tempData.Add("keyword1", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(config.vip_join_welcome_companyname));//"中山西路1919号馍王"
                //        tempData.Add("keyword2", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(totalAmount + "元"));
                //        tempData.Add("remark", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(config.vip_enough_footer));//"赶紧介绍给更多的同事吧！"

                //        accessToken = Senparc.Weixin.MP.CommonAPIs
                //            .AccessTokenContainer.TryGetToken(config.mp_slave_appid, config.mp_slave_appsecret);
                //        try
                //        {
                //            Senparc.Weixin.MP.AdvancedAPIs.Template.SendTemplateMessage(accessToken, modelUser.user_name, "KPeoGA1cOZTDiqAf1IECWauABgqtz-P-y-zHME9hhp8"
                //            , "#173177", "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wxc5776b5774a3d010&redirect_uri=http%3A%2F%2Fwww.4008317417.cn%2Fmp_join_company.aspx%3Fshowwxpaytitle%3D1&response_type=code&scope=snsapi_userinfo&state=slave#wechat_redirect", tempData);
                //        }
                //        catch (Exception ex) { }
                //    }
                //}
                
                
                //发送申请通过消息
                tempData = new Dictionary<string, Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem>();
                tempData.Add("first", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(config.vip_join_welcome_title));
                tempData.Add("cardNumber", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(modelUser.id.ToString()));
                tempData.Add("type", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(config.vip_join_welcome_name));//商户
                tempData.Add("address", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(config.vip_join_welcome_companyname));//"中山西路1919号馍王"
                tempData.Add("VIPName", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(modelUser.nick_name));
                tempData.Add("VIPPhone", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(modelUser.telphone));
                tempData.Add("expDate", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(DateTime.Now.AddMonths(2).ToString("yyyy年MM月dd日")));
                tempData.Add("remark", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(config.vip_join_welcome_footer));//"满50位同事后，更有1分钱吃馍专享活动！"
                accessToken = Senparc.Weixin.MP.CommonAPIs
                            .AccessTokenContainer.TryGetToken(config.mp_slave_appid, config.mp_slave_appsecret);
                try
                {
                    Senparc.Weixin.MP.AdvancedAPIs.Template.SendTemplateMessage(accessToken, modelUser.user_name, "XeehJQ-7hsp_JxTnXRPO1zKRtWf1raMi-5jScgbXfLU"
                    , "#173177", "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wxc5776b5774a3d010&redirect_uri=http%3A%2F%2Fwww.4008317417.cn%2Fmp_join_company.aspx%3Fshowwxpaytitle%3D1&response_type=code&scope=snsapi_userinfo&state=slave#wechat_redirect", tempData);
                }
                catch (Exception ex) { }
            }
            else
            {

            }

            return strongResponseMessage;
        }
    }
}