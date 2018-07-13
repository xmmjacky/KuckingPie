using System;
using System.Text;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DTcms.Common;
using System.Text.RegularExpressions;
using ZXing.Common;
using ZXing;
using ZXing.QrCode;
using System.Drawing;
using System.Drawing.Imaging;
using Senparc.Weixin.MP.AdvancedAPIs;
using System.Linq;

namespace DTcms.Web.admin.company
{
    public partial class edit : Web.UI.ManagePage
    {
        private string action = DTEnums.ActionEnum.Add.ToString(); //操作类型
        private int id = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            string _action = DTRequest.GetQueryString("action");
            
            if (!string.IsNullOrEmpty(_action) && _action == DTEnums.ActionEnum.Edit.ToString())
            {
                this.action = DTEnums.ActionEnum.Edit.ToString();//修改类型
                this.id = DTRequest.GetQueryInt("id");
                if (this.id == 0)
                {
                    JscriptMsg("传输参数不正确！", "back", "Error");
                    return;
                }
                if (!new BookingFood.BLL.bf_company().Exists(this.id))
                {
                    JscriptMsg("信息不存在或已被删除！", "back", "Error");
                    return;
                }
            }
            if (!Page.IsPostBack)
            {
                List<BookingFood.Model.bf_company> list = new BookingFood.BLL.bf_company().GetModelList("status=1");
                foreach (var item in list)
                {
                    ddlContactCompany.Items.Add(new ListItem(item.CompanyName, item.Id.ToString()));
                }
                if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
                {
                    ShowInfo(this.id);
                }                
            }
        }

        #region 赋值操作=================================
        private void ShowInfo(int _id)
        {
            BookingFood.BLL.bf_company bll = new BookingFood.BLL.bf_company();
            BookingFood.Model.bf_company model = bll.GetModel(_id);
            txtCompanyName.Text = model.CompanyName;
            txtAddress.Text = model.Address;
            txtPersonCount.Text = model.PersonCount.ToString();
            txtAcceptName.Text = model.AcceptName;
            txtTelphone.Text = model.Telphone;
            ddlStatus.SelectedValue = "2";//model.Status.ToString();
            ddlContactCompany.SelectedValue = model.ContactCompanyId.ToString();
            imgQr.ImageUrl = "/company_qr/" + model.Id.ToString() + ".jpg";
            if (model.BeginTime!=null)
            {
                txtBeginTime.Text = ((DateTime) model.BeginTime).ToString("yyyy-MM-dd HH:mm");
            }
            if (model.EndTime != null)
            {
                txtEndTime.Text = ((DateTime)model.EndTime).ToString("yyyy-MM-dd HH:mm");
            }
        }
        #endregion

        #region 增加操作=================================
        private bool DoAdd()
        {
            bool result = true;
            BookingFood.Model.bf_company model = new BookingFood.Model.bf_company();
            BookingFood.BLL.bf_company bll = new BookingFood.BLL.bf_company();

            model.CompanyName = txtCompanyName.Text.Trim();
            model.Address = txtAddress.Text.Trim();
            model.PersonCount = int.Parse(txtPersonCount.Text.Trim());
            model.AcceptName = txtAcceptName.Text.Trim();
            model.Telphone = txtTelphone.Text.Trim();
            model.Status = int.Parse(ddlStatus.SelectedValue);
            model.AddTime = DateTime.Now;
            if (!string.IsNullOrEmpty(txtBeginTime.Text.Trim()))
            {
                model.BeginTime = DateTime.Parse(txtBeginTime.Text.Trim());
            }
            if (!string.IsNullOrEmpty(txtEndTime.Text.Trim()))
            {
                model.BeginTime = DateTime.Parse(txtEndTime.Text.Trim());
            }
            if (!string.IsNullOrEmpty(ddlContactCompany.SelectedValue))
            {
                model.ContactCompanyId = int.Parse(ddlContactCompany.SelectedValue);
            }
            
            model.Id = bll.Add(model);
            if (model.Id < 1)
            {
                result = false;
            }
            else
            {
                if(model.Status==1)
                {
                    GenerateCompanyWelcome(model.Id);
                }
            }
            return result;
        }
        #endregion

        #region 修改操作=================================
        private bool DoEdit(int _id)
        {
            bool result = true;
            BookingFood.BLL.bf_company bll = new BookingFood.BLL.bf_company();
            BookingFood.Model.bf_company model = bll.GetModel(_id);
            int oldStatus = model.Status;
            model.CompanyName = txtCompanyName.Text.Trim();
            model.Address = txtAddress.Text.Trim();
            model.PersonCount = int.Parse(txtPersonCount.Text.Trim());
            model.AcceptName = txtAcceptName.Text.Trim();
            model.Telphone = txtTelphone.Text.Trim();
            model.Status = int.Parse(ddlStatus.SelectedValue);
            model.BeginTime = null;
            model.EndTime = null;
            if(!string.IsNullOrEmpty(txtBeginTime.Text.Trim()))
            {
                model.BeginTime = DateTime.Parse(txtBeginTime.Text.Trim());
            }
            if (!string.IsNullOrEmpty(txtEndTime.Text.Trim()))
            {
                model.EndTime = DateTime.Parse(txtEndTime.Text.Trim());
            }
            if (!string.IsNullOrEmpty(ddlContactCompany.SelectedValue))
            {
                model.ContactCompanyId = int.Parse(ddlContactCompany.SelectedValue);
            }

            if (!bll.Update(model))
            {
                result = false;
            }
            else
            {
                BookingFood.BLL.bf_user_voucher bllUserVoucher = new BookingFood.BLL.bf_user_voucher();
                Dictionary<string, Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem> tempData = new Dictionary<string, Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem>();
                if (oldStatus!= 2 && model.Status == 2 && model.RequestUserId > 0)
                {
                    BLL.users bllUsers = new BLL.users();
                    bllUsers.UpdateField(model.RequestUserId, "company_id=" + model.ContactCompanyId);
                    Model.users modelUsers = bllUsers.GetModel(model.RequestUserId);
                    BookingFood.BLL.bf_company_user_log bllCompanyUser = new BookingFood.BLL.bf_company_user_log();
                    //判断是否加入过这个群组
                    if(bllCompanyUser.GetRecordCount("UserId="+modelUsers.id+" and CompanyId="+model.ContactCompanyId)==0)
                    {
                        BookingFood.BLL.bf_company bllCompany = new BookingFood.BLL.bf_company();
                        BookingFood.Model.bf_company modelCompany = bllCompany.GetModel(model.ContactCompanyId);
                        modelCompany.PersonCount += 1;
                        bllCompany.Update(modelCompany);
                        //新通过的人取消默认的88元 17-01-15 改为本群组内所有人增加2元
                        //bllUserVoucher.Add(new BookingFood.Model.bf_user_voucher()
                        //{
                        //    AddTime = DateTime.Now,
                        //    Amount = 2,
                        //    CompanyId = modelCompany.Id,
                        //    ExpireTime = DateTime.Now.AddMonths(2),
                        //    UserId = model.RequestUserId
                        //});
                        bllCompanyUser.Add(new BookingFood.Model.bf_company_user_log() {
                            AddTime=DateTime.Now,
                            CompanyId=model.ContactCompanyId,
                            UserId=modelUsers.id
                        });
                        //每新增加一位,则群组下所有人增加2元
                        DataTable dtUser = bllUsers.GetList(0, "company_id=" + modelCompany.Id, "id asc").Tables[0];
                        
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
                            decimal totalAmount = bllUserVoucher.GetModelList("UserId=" + int.Parse(item["id"].ToString()) + " and GetDate()<ExpireTime and Status=0").Sum(s => s.Amount);
                            tempData = new Dictionary<string, Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem>();
                            tempData.Add("first", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(siteConfig.vip_enough_title));//"您有新同事加入馍王贵司VIP，所有成员余额均增加3元！"
                            tempData.Add("keyword1", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(siteConfig.vip_join_welcome_companyname));//"中山西路1919号馍王"
                            tempData.Add("keyword2", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(totalAmount + "元"));
                            tempData.Add("remark", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(siteConfig.vip_enough_footer));//"赶紧介绍给更多的同事吧！"

                            string _accessToken = Senparc.Weixin.MP.CommonAPIs
                                .AccessTokenContainer.TryGetToken(siteConfig.mp_slave_appid, siteConfig.mp_slave_appsecret);
                            try
                            {
                                Senparc.Weixin.MP.AdvancedAPIs.Template.SendTemplateMessage(_accessToken, item["user_name"].ToString(), "KPeoGA1cOZTDiqAf1IECWauABgqtz-P-y-zHME9hhp8"
                                , "#173177", "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wxc5776b5774a3d010&redirect_uri=https%3A%2F%2Fwww.4008317417.cn%2Fmp_join_company.aspx%3Fshowwxpaytitle%3D1&response_type=code&scope=snsapi_userinfo&state=slave#wechat_redirect", tempData);
                            }
                            catch (Exception ex) { }
                        }
                    }

                    //发送申请通过消息
                    //tempData = new Dictionary<string, Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem>();
                    //tempData.Add("first", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(siteConfig.vip_join_approve_title));//"您申请的贵司VIP卡已通过,其他同事扫码加入即获88元现金券"
                    //tempData.Add("cardNumber", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(modelUsers.id.ToString()));
                    //tempData.Add("type", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(siteConfig.vip_join_welcome_name));//"商户"
                    //tempData.Add("address", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(siteConfig.vip_join_welcome_companyname));//"中山西路1919号馍王"
                    //tempData.Add("VIPName", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(modelUsers.nick_name));
                    //tempData.Add("VIPPhone", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(modelUsers.telphone));
                    //tempData.Add("expDate", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(DateTime.Now.AddMonths(2).ToString("yyyy年MM月dd日")));
                    //tempData.Add("remark", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(siteConfig.vip_join_welcome_footer));//"满50位同事后，更有1分钱吃馍专享活动！"
                    //string accessToken = Senparc.Weixin.MP.CommonAPIs
                    //            .AccessTokenContainer.TryGetToken(siteConfig.mp_slave_appid, siteConfig.mp_slave_appsecret);
                    //try
                    //{
                    //    Senparc.Weixin.MP.AdvancedAPIs.Template.SendTemplateMessage(accessToken, modelUsers.user_name, "XeehJQ-7hsp_JxTnXRPO1zKRtWf1raMi-5jScgbXfLU"
                    //    , "#173177", "", tempData);
                    //}
                    //catch (Exception) { }
                }
                else if (oldStatus != 1 && model.Status == 1 && model.RequestUserId > 0)
                {
                    BLL.users bllUsers = new BLL.users();
                    bllUsers.UpdateField(model.RequestUserId, "company_id=" + model.Id);
                    Model.users modelUsers = bllUsers.GetModel(model.RequestUserId);
                    GenerateCompanyWelcome(model.Id);
                    BookingFood.BLL.bf_company_user_log bllCompanyUser = new BookingFood.BLL.bf_company_user_log();
                    //判断是否加入过这个群组
                    if (bllCompanyUser.GetRecordCount("UserId=" + modelUsers.id + " and CompanyId=" + model.Id) == 0)
                    {
                        BookingFood.BLL.bf_company bllCompany = new BookingFood.BLL.bf_company();
                        BookingFood.Model.bf_company modelCompany = bllCompany.GetModel(model.Id);
                        modelCompany.PersonCount += 1;
                        if (modelCompany.PersonCount == 30)
                        {
                            modelCompany.CompleteTime = DateTime.Now;
                        }
                        bllCompany.Update(modelCompany);
                        //新通过的人取消默认的88元 17-01-15 改为2元
                        bllUserVoucher.Add(new BookingFood.Model.bf_user_voucher()
                        {
                            AddTime = DateTime.Now,
                            Amount = 2,
                            CompanyId = modelCompany.Id,
                            ExpireTime = DateTime.Now.AddMonths(1),
                            UserId = model.RequestUserId
                        });
                        bllCompanyUser.Add(new BookingFood.Model.bf_company_user_log()
                        {
                            AddTime = DateTime.Now,
                            CompanyId = model.Id,
                            UserId = modelUsers.id
                        });
                    }

                    //发送申请通过消息
                    tempData = new Dictionary<string, Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem>();
                    tempData.Add("first", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(siteConfig.vip_join_approve_title));//"您申请的贵司VIP卡已通过,其他同事扫码加入即获88元现金券"
                    tempData.Add("cardNumber", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(modelUsers.id.ToString()));
                    tempData.Add("type", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(siteConfig.vip_join_welcome_name));//"商户"
                    tempData.Add("address", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(siteConfig.vip_join_welcome_companyname));//"中山西路1919号馍王"
                    tempData.Add("VIPName", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(modelUsers.nick_name));
                    tempData.Add("VIPPhone", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(modelUsers.telphone));
                    tempData.Add("expDate", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(DateTime.Now.AddMonths(2).ToString("yyyy年MM月dd日")));
                    tempData.Add("remark", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(siteConfig.vip_join_welcome_footer)); //"满50位同事后，更有1分钱吃馍专享活动！"
                    string accessToken = Senparc.Weixin.MP.CommonAPIs
                                .AccessTokenContainer.TryGetToken(siteConfig.mp_slave_appid, siteConfig.mp_slave_appsecret);
                    try
                    {
                        Senparc.Weixin.MP.AdvancedAPIs.Template.SendTemplateMessage(accessToken, modelUsers.user_name, "XeehJQ-7hsp_JxTnXRPO1zKRtWf1raMi-5jScgbXfLU"
                        , "#173177", "", tempData);
                    }
                    catch (Exception) { }
                }
                
            }
            return result;
        }
        #endregion

        //保存
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
            {
                ChkAdminLevel("bf_company", DTEnums.ActionEnum.Edit.ToString()); //检查权限
                if (!DoEdit(this.id))
                {
                    JscriptMsg("保存过程中发生错误啦！", "", "Error");
                    return;
                }
                JscriptMsg("修改群组成功啦！", "list.aspx", "Success");
            }
            else //添加
            {
                ChkAdminLevel("bf_company", DTEnums.ActionEnum.Add.ToString()); //检查权限
                if (!DoAdd())
                {
                    JscriptMsg("保存过程中发生错误啦！", "", "Error");
                    return;
                }
                JscriptMsg("添加群组成功啦！", "list.aspx", "Success");
            }
        }
        
        private void GenerateCompanyWelcome(int id)
        {
            string accessToken = Senparc.Weixin.MP.CommonAPIs
                    .AccessTokenContainer.TryGetToken(siteConfig.mp_slave_appid, siteConfig.mp_slave_appsecret);
            CreateQrCodeResult qrResut = QrCode.Create(accessToken, 0, id);
            string qr_url = QrCode.GetShowQrCodeUrl(qrResut.ticket);
            qr_url = Utils.DownloadImg(qr_url, "/company_qr/","qr_"+id.ToString()+".jpg");

            DTcms.Common.WaterMark.AddImageToPic("/templates/green/images/qr_company.jpg", "/company_qr/"+id.ToString()+".jpg"
                , qr_url, 2, 100, 10);
        }

    }
}