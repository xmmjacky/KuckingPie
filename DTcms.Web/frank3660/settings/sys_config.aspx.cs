using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DTcms.Common;
using System.Net;
using System.IO;
using System.Text;

namespace DTcms.Web.admin.settings
{
    public partial class sys_config : Web.UI.ManagePage
    {
        string defaultpassword = "0|0|0|0"; //默认显示密码
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ChkAdminLevel("sys_config", DTEnums.ActionEnum.View.ToString()); //检查权限
                ShowInfo();
            }
        }

        #region 赋值操作=================================
        private void ShowInfo()
        {
            BLL.siteconfig bll = new BLL.siteconfig();
            Model.siteconfig model = bll.loadConfig(Utils.GetXmlMapPath(DTKeys.FILE_SITE_XML_CONFING));
            
            webname.Text = model.webname;
            webcompany.Text = model.webcompany;
            weburl.Text = model.weburl;
            webtel.Text = model.webtel;
            webfax.Text = model.webfax;
            webmail.Text = model.webmail;
            webcrod.Text = model.webcrod;
            webtitle.Text = model.webtitle;
            webkeyword.Text = model.webkeyword;
            webdescription.Text = model.webdescription;
            webcopyright.Text = model.webcopyright;
            webpath.Text = model.webpath;
            webmanagepath.Text = model.webmanagepath;
            webstatus.Text = model.webstatus.ToString();
            webclosereason.Text = model.webclosereason;
            webcountcode.Text = model.webcountcode;
            
            staticstatus.SelectedValue = model.staticstatus.ToString();
            staticextension.Text = model.staticextension;
            memberstatus.SelectedValue = model.memberstatus.ToString();
            commentstatus.SelectedValue = model.commentstatus.ToString();
            logstatus.SelectedValue = model.logstatus.ToString();

            emailstmp.Text = model.emailstmp;
            emailport.Text = model.emailport.ToString();
            emailfrom.Text = model.emailfrom;
            emailusername.Text = model.emailusername;
            if (!string.IsNullOrEmpty(model.emailpassword))
            {
                emailpassword.Attributes["value"] = defaultpassword;
            }
            emailnickname.Text = model.emailnickname;
            
            attachpath.Text = model.attachpath;
            attachextension.Text = model.attachextension;
            attachsave.SelectedValue = model.attachsave.ToString();
            attachfilesize.Text = model.attachfilesize.ToString();
            attachimgsize.Text = model.attachimgsize.ToString();
            attachimgmaxheight.Text = model.attachimgmaxheight.ToString();
            attachimgmaxwidth.Text = model.attachimgmaxwidth.ToString();
            thumbnailheight.Text = model.thumbnailheight.ToString();
            thumbnailwidth.Text = model.thumbnailwidth.ToString();
            watermarktype.SelectedValue = model.watermarktype.ToString();
            watermarkposition.Text = model.watermarkposition.ToString();
            watermarkimgquality.Text = model.watermarkimgquality.ToString();
            watermarkpic.Text = model.watermarkpic;
            watermarktransparency.Text = model.watermarktransparency.ToString();
            watermarktext.Text = model.watermarktext;
            watermarkfont.Text = model.watermarkfont;
            watermarkfontsize.Text = model.watermarkfontsize.ToString();
            ddlStartTime.SelectedValue = model.starttime;
            ddlEndTime.SelectedValue = model.endtime;
            ddlStartTimeHere.SelectedValue = model.starttime_here;
            ddlEndTimeHere.SelectedValue = model.endtime_here;
            txtLowAmount.Text = model.lowamount.ToString();
            txtLowAmount_2.Text = model.lowamount_2.ToString();
            txtFreeDisAmount.Text = model.freedisamount.ToString();
            txtDisAmount.Text = model.disamount.ToString();
            txtTaoDianDianAccount.Text = model.TaoDianDianAccount;
            txtIsNullCartAccount.Text = model.IsNullCartAccount;
            
            txtImgUrl1.Text = model.HeadPhoto1;
            txtImgUrl2.Text = model.HeadPhoto2;
            txtImgUrl3.Text = model.HeadPhoto3;
            txtImgUrl4.Text = model.HeadPhoto4;
            txtImgUrl5.Text = model.HeadPhoto5;
            txtImgUrl6.Text = model.HeadPhoto6;
            txtImgUrl7.Text = model.HeadPhoto7;
            txtImgUrl8.Text = model.HeadPhoto8;

            txtMpBackgroundImage.Text = model.mp_backgroundimage;
            txtMpBackgroundImage2.Text = model.mp_backgroundimage2;
            txtMpWelcomeContent.Text = model.mp_welcomecontent;
            txtMpWelcomeImage.Text = model.mp_welcomeimage;
            txtMpWelcomeTitle.Text = model.mp_welcometitle;
            txtAppId.Text = model.mp_appid;
            txtAppSecret.Text = model.mp_appsecret;
            txtAESKey.Text = model.mp_aeskey;
            txtMenu.Text = model.mp_menu;
            txtSlaveAppId.Text = model.mp_slave_appid;
            txtSlaveAppSecret.Text = model.mp_slave_appsecret;
            txtSlaveAESKey.Text = model.mp_slave_aeskey;
            txtSlaveMenu.Text = model.mp_slave_menu;
            
            txtAdditional.Text = model.additional.ToString();
            txtAdditionalForce.Text = model.additional_force.ToString();
            txt_mp_temp_submitorder_first.Text = model.mp_temp_submitorder_first;
            txt_mp_temp_submitorder_remark.Text = model.mp_temp_submitorder_remark;
            txt_mp_temp_distribution_takeout_first.Text = model.mp_temp_distribution_takeout_first;
            txt_mp_temp_distribution_takeout_keyword5.Text = model.mp_temp_distribution_takeout_keyword5;
            txt_mp_temp_distribution_takeout_remark.Text = model.mp_temp_distribution_takeout_remark;
            txt_mp_temp_error_area_first.Text = model.mp_temp_error_area_first;
            txt_mp_temp_error_area_remark.Text = model.mp_temp_error_area_remark;
            txt_mp_temp_range_out_first.Text = model.mp_temp_range_out_first;
            txt_mp_temp_range_out_remark.Text = model.mp_temp_range_out_remark;
            txt_mp_temp_much_condition_first.Text = model.mp_temp_much_condition_first;
            txt_mp_temp_much_condition_remark.Text = model.mp_temp_much_condition_remark;
            txt_mp_temp_call_takeout_first.Text = model.mp_temp_call_takeout_first;
            txt_mp_temp_call_takeout_remark.Text = model.mp_temp_call_takeout_remark;
            
            txtAlarmMessageLowAmount.Text = model.alarm_message_lowamount;
            txtDeliverPayMaxAmountForMp.Text = model.DeliverPayMaxAmountForMp.ToString();
            txtDeliverPayMaxAmountForWeb.Text = model.DeliverPayMaxAmountForWeb.ToString();
            rblThirdOrder.SelectedValue = model.RunLoopThirdOrder.ToString();
            rblTigoon.SelectedValue = model.RunTigoon.ToString();
            rblSyncOrderDownload.SelectedValue = model.SyncOrderToDownload.ToString();
            txtMeituanCookie.Text = model.MeituanCookie;
            txtBaiduCookie.Text = model.BaiduCookie;
            txtElemeCookie.Text = model.ElemeCookie;
            txtVipJoinWelcomeTitle.Text = model.vip_join_welcome_title;
            txtvip_join_welcome_name.Text = model.vip_join_welcome_name;
            txtvip_join_welcome_companyname.Text = model.vip_join_welcome_companyname;
            txtvip_join_welcome_footer.Text = model.vip_join_welcome_footer;
            txtvip_enough_title.Text = model.vip_enough_title;
            txtvip_enough_footer.Text = model.vip_enough_footer;
            txtvip_join_approve_title.Text = model.vip_join_approve_title;
            cboenable_waimai_vip.Checked = model.enable_waimai_vip;
            rblSendVoucher.SelectedValue = model.enable_register_company_send_voucher ? "1" : "0";
        }
        #endregion

        /// <summary>
        /// 保存配置信息
        /// </summary>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            ChkAdminLevel("sys_config", DTEnums.ActionEnum.Edit.ToString()); //检查权限
            BLL.siteconfig bll = new BLL.siteconfig();
            Model.siteconfig model = bll.loadConfig(Utils.GetXmlMapPath(DTKeys.FILE_SITE_XML_CONFING));
            try
            {
                model.webname = webname.Text;
                model.webcompany = webcompany.Text;
                model.weburl = weburl.Text;
                model.webtel = webtel.Text;
                model.webfax = webfax.Text;
                model.webmail = webmail.Text;
                model.webcrod = webcrod.Text;
                model.webtitle = webtitle.Text;
                model.webkeyword = webkeyword.Text;
                model.webdescription = Utils.DropHTML(webdescription.Text);
                model.webcopyright = webcopyright.Text;
                model.webpath = webpath.Text;
                model.webmanagepath = webmanagepath.Text;
                model.webstatus = int.Parse(webstatus.Text.Trim());
                model.webclosereason = webclosereason.Text;
                model.webcountcode = webcountcode.Text;

                model.staticstatus = int.Parse(staticstatus.SelectedValue);
                model.staticextension = staticextension.Text;
                model.memberstatus = int.Parse(memberstatus.SelectedValue);
                model.commentstatus = int.Parse(commentstatus.SelectedValue);
                model.logstatus = int.Parse(logstatus.SelectedValue);

                model.emailstmp = emailstmp.Text;
                model.emailport = int.Parse(emailport.Text.Trim());
                model.emailfrom = emailfrom.Text;
                model.emailusername = emailusername.Text;
                //判断密码是否更改
                if (emailpassword.Text.Trim() != defaultpassword)
                {
                    model.emailpassword = DESEncrypt.Encrypt(emailpassword.Text, model.sysencryptstring);
                }
                model.emailnickname = emailnickname.Text;

                model.attachpath = attachpath.Text;
                model.attachextension = attachextension.Text;
                model.attachsave = int.Parse(attachsave.SelectedValue);
                model.attachfilesize = int.Parse(attachfilesize.Text.Trim());
                model.attachimgsize = int.Parse(attachimgsize.Text.Trim());
                model.attachimgmaxheight = int.Parse(attachimgmaxheight.Text.Trim());
                model.attachimgmaxwidth = int.Parse(attachimgmaxwidth.Text.Trim());
                model.thumbnailheight = int.Parse(thumbnailheight.Text.Trim());
                model.thumbnailwidth = int.Parse(thumbnailwidth.Text.Trim());
                model.watermarktype = int.Parse(watermarktype.SelectedValue);
                model.watermarkposition = int.Parse(watermarkposition.Text.Trim());
                model.watermarkimgquality = int.Parse(watermarkimgquality.Text.Trim());
                model.watermarkpic = watermarkpic.Text;
                model.watermarktransparency = int.Parse(watermarktransparency.Text.Trim());
                model.watermarktext = watermarktext.Text;
                model.watermarkfont = watermarkfont.Text;
                model.watermarkfontsize = int.Parse(watermarkfontsize.Text.Trim());
                model.starttime = ddlStartTime.SelectedValue;
                model.endtime = ddlEndTime.SelectedValue;
                model.starttime_here = ddlStartTimeHere.SelectedValue;
                model.endtime_here = ddlEndTimeHere.SelectedValue;
                model.lowamount = decimal.Parse(txtLowAmount.Text.Trim());
                model.freedisamount = decimal.Parse(txtFreeDisAmount.Text.Trim());
                model.disamount = decimal.Parse(txtDisAmount.Text.Trim());
                model.lowamount_2 = decimal.Parse(txtLowAmount_2.Text.Trim());
                model.TaoDianDianAccount = txtTaoDianDianAccount.Text.Trim();
                model.IsNullCartAccount = txtIsNullCartAccount.Text.Trim();
                model.additional = int.Parse(txtAdditional.Text.Trim());
                model.additional_force = int.Parse(txtAdditionalForce.Text.Trim());

                model.HeadPhoto1 = txtImgUrl1.Text.Trim();
                model.HeadPhoto2 = txtImgUrl2.Text.Trim();
                model.HeadPhoto3 = txtImgUrl3.Text.Trim();
                model.HeadPhoto4 = txtImgUrl4.Text.Trim();
                model.HeadPhoto5 = txtImgUrl5.Text.Trim();
                model.HeadPhoto6 = txtImgUrl6.Text.Trim();
                model.HeadPhoto7 = txtImgUrl7.Text.Trim();
                model.HeadPhoto8 = txtImgUrl8.Text.Trim();

                model.mp_backgroundimage = txtMpBackgroundImage.Text.Trim();
                model.mp_backgroundimage2 = txtMpBackgroundImage2.Text.Trim();
                model.mp_welcomecontent = txtMpWelcomeContent.Text.Trim();
                model.mp_welcomeimage = txtMpWelcomeImage.Text.Trim();
                model.mp_welcometitle = txtMpWelcomeTitle.Text.Trim();
                model.mp_appid = txtAppId.Text.Trim();
                model.mp_appsecret = txtAppSecret.Text.Trim();
                model.mp_menu = txtMenu.Text.Trim();
                model.mp_aeskey = txtAESKey.Text.Trim();
                model.mp_slave_appid = txtSlaveAppId.Text.Trim();
                model.mp_slave_appsecret = txtSlaveAppSecret.Text.Trim();
                model.mp_slave_aeskey = txtSlaveAESKey.Text.Trim();
                model.mp_slave_menu = txtSlaveMenu.Text.Trim();


                model.mp_temp_submitorder_first = txt_mp_temp_submitorder_first.Text.Trim();
                model.mp_temp_submitorder_remark = txt_mp_temp_submitorder_remark.Text.Trim();
                model.mp_temp_distribution_takeout_first = txt_mp_temp_distribution_takeout_first.Text.Trim();
                model.mp_temp_distribution_takeout_keyword5 = txt_mp_temp_distribution_takeout_keyword5.Text.Trim();
                model.mp_temp_distribution_takeout_remark = txt_mp_temp_distribution_takeout_remark.Text.Trim();
                model.mp_temp_error_area_first=txt_mp_temp_error_area_first.Text.Trim();
                model.mp_temp_error_area_remark=txt_mp_temp_error_area_remark.Text.Trim();
                model.mp_temp_range_out_first=txt_mp_temp_range_out_first.Text.Trim();
                model.mp_temp_range_out_remark = txt_mp_temp_range_out_remark.Text.Trim();
                model.mp_temp_much_condition_first = txt_mp_temp_much_condition_first.Text.Trim();
                model.mp_temp_much_condition_remark = txt_mp_temp_much_condition_remark.Text.Trim();
                model.mp_temp_call_takeout_first = txt_mp_temp_call_takeout_first.Text.Trim();
                model.mp_temp_call_takeout_remark = txt_mp_temp_call_takeout_remark.Text.Trim();

                model.alarm_message_lowamount = txtAlarmMessageLowAmount.Text.Trim();
                model.DeliverPayMaxAmountForMp = decimal.Parse(txtDeliverPayMaxAmountForMp.Text.Trim());
                model.DeliverPayMaxAmountForWeb = decimal.Parse(txtDeliverPayMaxAmountForWeb.Text.Trim());
                model.RunLoopThirdOrder = int.Parse(rblThirdOrder.SelectedValue);
                model.RunTigoon = int.Parse(rblTigoon.SelectedValue);
                model.SyncOrderToDownload = int.Parse(rblSyncOrderDownload.SelectedValue);
                model.MeituanCookie = txtMeituanCookie.Text.Trim();
                model.BaiduCookie = txtBaiduCookie.Text.Trim();
                model.ElemeCookie = txtElemeCookie.Text.Trim();
                model.vip_join_welcome_title = txtVipJoinWelcomeTitle.Text.Trim();
                model.vip_join_welcome_name = txtvip_join_welcome_name.Text.Trim();
                model.vip_join_welcome_companyname = txtvip_join_welcome_companyname.Text.Trim();
                model.vip_join_welcome_footer = txtvip_join_welcome_footer.Text.Trim();
                model.vip_enough_title = txtvip_enough_title.Text.Trim();
                model.vip_enough_footer = txtvip_enough_footer.Text.Trim();
                model.vip_join_approve_title = txtvip_join_approve_title.Text.Trim();
                model.enable_waimai_vip = cboenable_waimai_vip.Checked;
                model.enable_register_company_send_voucher = rblSendVoucher.SelectedValue == "1" ? true : false;

                bll.saveConifg(model, Utils.GetXmlMapPath(DTKeys.FILE_SITE_XML_CONFING));
                JscriptMsg("修改系统信息成功啦！", "sys_config.aspx", "Success");
            }
            catch
            {
                JscriptMsg("文件写入失败，请检查是否有权限！", "", "Error");
            }
        }

        protected void btnPublish_Click(object sender, EventArgs e)
        {
            ChkAdminLevel("sys_config", DTEnums.ActionEnum.Edit.ToString()); //检查权限
            BLL.siteconfig bll = new BLL.siteconfig();
            Model.siteconfig model = bll.loadConfig(Utils.GetXmlMapPath(DTKeys.FILE_SITE_XML_CONFING));
            try
            {
                model.mp_appid = txtAppId.Text.Trim();
                model.mp_appsecret = txtAppSecret.Text.Trim();
                model.mp_menu = txtMenu.Text.Trim();

                model.mp_slave_appid = txtSlaveAppId.Text.Trim();
                model.mp_slave_appsecret = txtSlaveAppSecret.Text.Trim();
                model.mp_slave_menu = txtSlaveMenu.Text.Trim();

                bll.saveConifg(model, Utils.GetXmlMapPath(DTKeys.FILE_SITE_XML_CONFING));
                JscriptMsg("修改微信菜单成功！", "sys_config.aspx", "Success");
            }
            catch
            {
                JscriptMsg("文件写入失败，请检查是否有权限！", "", "Error");
            }

            string accessToken = Senparc.Weixin.MP.CommonAPIs
                    .AccessTokenContainer.TryGetToken(siteConfig.mp_appid, siteConfig.mp_appsecret);
            if (!string.IsNullOrEmpty(accessToken))
            {
                menu(model.mp_menu, accessToken);
            }
            else
            {
                JscriptMsg("APPID或APPSECRET配置错误，无法取得AccessToken！", "", "Error");
            }
            accessToken = Senparc.Weixin.MP.CommonAPIs
                    .AccessTokenContainer.TryGetToken(siteConfig.mp_slave_appid, siteConfig.mp_slave_appsecret);
            if (!string.IsNullOrEmpty(accessToken))
            {
                menu(model.mp_slave_menu, accessToken);
            }
            else
            {
                JscriptMsg("APPID或APPSECRET配置错误，无法取得AccessToken！", "", "Error");
            }
        }

        private string getAccessToken(string url)
        {

            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            Stream instream = response.GetResponseStream();
            StreamReader sr = null;
            Encoding encoding = System.Text.Encoding.GetEncoding("UTF-8");
            sr = new StreamReader(instream, encoding);
            string content = sr.ReadToEnd();
            return content;

            //WebClient wc = new WebClient();
            //wc.Encoding = Encoding.UTF8;            
            //return wc.DownloadString(url);
        }

        private void menu(string postData, string accessToken)
        {
            Stream outstream = null;
            Stream instream = null;
            StreamReader sr = null;
            HttpWebResponse response = null;
            HttpWebRequest request = null;
            Encoding encoding = System.Text.Encoding.GetEncoding("UTF-8");
            byte[] data = encoding.GetBytes(postData);
            // 准备请求...  
            try
            {
                // 设置参数  
                request = WebRequest.Create("https://api.weixin.qq.com/cgi-bin/menu/create?access_token="
                    + accessToken) as HttpWebRequest;
                request.AllowAutoRedirect = true;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                outstream = request.GetRequestStream();
                outstream.Write(data, 0, data.Length);
                outstream.Close();
                //发送请求并获取相应回应数据  
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求  
                instream = response.GetResponseStream();
                sr = new StreamReader(instream, encoding);
                //返回结果网页（html）代码  
                string content = sr.ReadToEnd();
                if (content.IndexOf("-1") > -1)
                {
                    JscriptMsg("微信系统繁忙，请重新提交！", "", "Error");
                    return;
                }
                JscriptMsg("微信菜单发布成功！", "", "Success");
            }
            catch (Exception ex)
            {
                JscriptMsg(ex.Message, "", "Error");
            }
        }
    }
}