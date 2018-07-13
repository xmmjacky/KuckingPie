using System;
using System.Text;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DTcms.Common;
using System.Linq;

namespace DTcms.Web.admin.company
{
    public partial class list : Web.UI.ManagePage
    {        
        protected int totalCount;
        protected int page;
        protected int pageSize;
        protected string keywords = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.keywords = DTRequest.GetQueryString("keywords");
            this.pageSize = GetPageSize(7); //每页数量
            if (!Page.IsPostBack)
            {
                ChkAdminLevel("bf_company", DTEnums.ActionEnum.View.ToString()); //检查权限                
                RptBind("Id>0"+ CombSqlTxt(this.keywords), "[Status] asc,Id desc");
            }
        } 
        

        #region 数据绑定=================================
        private void RptBind(string _strWhere, string _orderby)
        {
            this.page = DTRequest.GetQueryInt("page", 1);
            BookingFood.BLL.bf_company bll = new BookingFood.BLL.bf_company();
            int StartIndex = 0;
            int EndIndex = 0;
            if (this.page == 1)
            {
                StartIndex = 1;
                EndIndex = this.pageSize;
            }
            else
            {
                StartIndex = (this.page - 1) * this.pageSize + 1;
                EndIndex = this.page * this.pageSize;
            }

            this.rptList1.DataSource = bll.GetListByPage(_strWhere, _orderby, StartIndex, EndIndex);
            this.rptList1.DataBind();
            totalCount = bll.GetList(_strWhere).Tables[0].Rows.Count;

            //绑定页码
            txtPageNum.Text = this.pageSize.ToString();
            string pageUrl = Utils.CombUrlTxt("list.aspx", "keywords={0}&page={1}",
                this.keywords, "__id__");
            PageContent.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
        }
        #endregion

        #region 组合SQL查询语句==========================
        protected string CombSqlTxt(string _keywords)
        {
            StringBuilder strTemp = new StringBuilder();
            _keywords = _keywords.Replace("'", "");
            if (!string.IsNullOrEmpty(_keywords))
            {
                strTemp.Append(" and (CompanyName like '%" + _keywords + "%' or Address like '%" + _keywords + "%')");
            }
            strTemp.Append(" and [Status]!=2");
            return strTemp.ToString();
        }
        #endregion

        #region 返回图文每页数量=========================
        private int GetPageSize(int _default_size)
        {
            int _pagesize;
            if (int.TryParse(Utils.GetCookie("bf_company_page_size"), out _pagesize))
            {
                if (_pagesize > 0)
                {
                    return _pagesize;
                }
            }
            return _default_size;
        }
        #endregion

        //批量删除
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            ChkAdminLevel("bf_company", DTEnums.ActionEnum.Delete.ToString()); //检查权限
            BookingFood.BLL.bf_company bll = new BookingFood.BLL.bf_company();
            Repeater rptList = new Repeater();
            rptList = this.rptList1;
            for (int i = 0; i < rptList.Items.Count; i++)
            {
                int id = Convert.ToInt32(((HiddenField)rptList.Items[i].FindControl("hidId")).Value);
                CheckBox cb = (CheckBox)rptList.Items[i].FindControl("chkId");
                if (cb.Checked)
                {
                    bll.Delete(id);
                }
            }
            JscriptMsg("批量删除成功啦！", Utils.CombUrlTxt("list.aspx", "",""), "Success");
        }

        public string GetStatus(string status)
        {
            switch(status)
            {
                case "0":
                    return "申请";
                case "1":
                    return "通过";
                case "2":
                    return "合并";
                case "3":
                    return "驳回";
                default:
                    return "";
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect(Utils.CombUrlTxt("list.aspx", "keywords={0}",
                txtKeywords.Text));
        }

        //设置分页数量
        protected void txtPageNum_TextChanged(object sender, EventArgs e)
        {
            int _pagesize;
            if (int.TryParse(txtPageNum.Text.Trim(), out _pagesize))
            {
                if (_pagesize > 0)
                {
                    Utils.WriteCookie("bf_company_page_size", _pagesize.ToString(), 43200);
                }
            }
            Response.Redirect(Utils.CombUrlTxt("list.aspx", "keywords={0}", this.keywords
                ));
        }

        protected void btnContact_Click(object sender, EventArgs e)
        {
            BLL.users bllUsers = new BLL.users();
            BookingFood.BLL.bf_company bll = new BookingFood.BLL.bf_company();
            Repeater rptList = new Repeater();
            rptList = this.rptList1;
            string readyContact = string.Empty, contactId =string.Empty;
            for (int i = 0; i < rptList.Items.Count; i++)
            {
                int id = Convert.ToInt32(((HiddenField)rptList.Items[i].FindControl("hidId")).Value);
                CheckBox cb = (CheckBox)rptList.Items[i].FindControl("chkId");
                if (cb.Checked)
                {
                    readyContact += id + ",";
                    contactId = id.ToString();
                }
            }
            readyContact = readyContact.Replace(contactId + ",", "").TrimEnd(',');
            //设置群组为合并状态
            foreach (var item in readyContact.Split(','))
            {
                BookingFood.Model.bf_company modelCompany = bll.GetModel(int.Parse(item));
                modelCompany.Status = 2;
                bll.Update(modelCompany);
            }
            DataTable dt = bllUsers.GetList(0,"company_id in (" + readyContact + ")","id desc").Tables[0];
            Dictionary<string, Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem> tempData = new Dictionary<string, Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem>();
            BookingFood.BLL.bf_user_voucher bllUserVoucher = new BookingFood.BLL.bf_user_voucher();
            BookingFood.BLL.bf_company_user_amount_notice bllNotice = new BookingFood.BLL.bf_company_user_amount_notice();
            foreach (DataRow item in dt.Rows)
            {
                int userId = int.Parse(item["id"].ToString());
                bllUsers.UpdateField(userId, "company_id=" + contactId);
                Model.users modelUsers = bllUsers.GetModel(userId);
                BookingFood.BLL.bf_company_user_log bllCompanyUser = new BookingFood.BLL.bf_company_user_log();
                //判断是否加入过这个群组
                if (bllCompanyUser.GetRecordCount("UserId=" + modelUsers.id + " and CompanyId=" + contactId) == 0)
                {
                    BookingFood.BLL.bf_company bllCompany = new BookingFood.BLL.bf_company();
                    BookingFood.Model.bf_company modelCompany = bllCompany.GetModel(int.Parse(contactId));
                    modelCompany.PersonCount += 1;
                    bllCompany.Update(modelCompany);

                    bllCompanyUser.Add(new BookingFood.Model.bf_company_user_log()
                    {
                        AddTime = DateTime.Now,
                        CompanyId = modelCompany.Id,
                        UserId = modelUsers.id
                    });
                    //每新增加一位,则群组下所有人增加2元
                    DataTable dtUser = bllUsers.GetList(0, "company_id=" + modelCompany.Id, "id asc").Tables[0];

                    foreach (DataRow item1 in dtUser.Rows)
                    {
                        bllUserVoucher.Add(new BookingFood.Model.bf_user_voucher()
                        {
                            AddTime = DateTime.Now,
                            Amount = 2,
                            CompanyId = modelCompany.Id,
                            ExpireTime = DateTime.Now.AddMonths(1),
                            UserId = int.Parse(item1["id"].ToString())
                        });
                        if (bllNotice.GetRecordCount("UserId=" + item1["id"].ToString() + " and (AddTime between '"
                            + DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00' and '" + DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59')") == 0)
                        {
                            bllNotice.Add(new BookingFood.Model.bf_company_user_amount_notice()
                            {
                                AddTime = DateTime.Now,
                                UserId = int.Parse(item1["id"].ToString())
                            });
                            decimal totalAmount = bllUserVoucher.GetModelList("UserId=" + int.Parse(item1["id"].ToString()) + " and GetDate()<ExpireTime and Status=0").Sum(s => s.Amount);
                            tempData = new Dictionary<string, Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem>();
                            tempData.Add("first", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(siteConfig.vip_enough_title));//"您有新同事加入馍王贵司VIP，所有成员余额均增加3元！"
                            tempData.Add("keyword1", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(siteConfig.vip_join_welcome_companyname));//"中山西路1919号馍王"
                            tempData.Add("keyword2", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(totalAmount + "元"));
                            tempData.Add("remark", new Senparc.Weixin.MP.AdvancedAPIs.TemplateDataItem(siteConfig.vip_enough_footer));//"赶紧介绍给更多的同事吧！"

                            string _accessToken = Senparc.Weixin.MP.CommonAPIs
                                .AccessTokenContainer.TryGetToken(siteConfig.mp_slave_appid, siteConfig.mp_slave_appsecret);
                            try
                            {
                                Senparc.Weixin.MP.AdvancedAPIs.Template.SendTemplateMessage(_accessToken, item1["user_name"].ToString(), "KPeoGA1cOZTDiqAf1IECWauABgqtz-P-y-zHME9hhp8"
                                , "#173177", "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wxc5776b5774a3d010&redirect_uri=https%3A%2F%2Fwww.4008317417.cn%2Fmp_join_company.aspx%3Fshowwxpaytitle%3D1&response_type=code&scope=snsapi_userinfo&state=slave#wechat_redirect", tempData);
                            }
                            catch (Exception ex) { }
                        }
                            
                    }
                }
            }
            Response.Redirect(Utils.CombUrlTxt("list.aspx", "keywords={0}",
                this.keywords));
        }
    }
}