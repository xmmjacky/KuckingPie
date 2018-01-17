using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DTcms.Common;

namespace DTcms.Web.admin.orders
{
    public partial class old_order_list : Web.UI.ManagePage
    {
        protected int totalCount;
        protected int page;
        protected int pageSize;
        protected string type;

        string keyword = string.Empty;
        

        protected void Page_Load(object sender, EventArgs e)
        {

            keyword = DTRequest.GetQueryString("keyword");
            type = DTRequest.GetQueryString("type");
            this.pageSize = GetPageSize(15); //每页数量
            if (!Page.IsPostBack)
            {
                string areaid = Session["AreaId"].ToString();
                ChkAdminLevel("tongjiguanli", DTEnums.ActionEnum.View.ToString()); //检查权限
                RptBind("id>0" + CombSqlTxt(this.keyword), "id desc");
            }
        }

        #region 数据绑定=================================
        private void RptBind(string _strWhere, string _orderby)
        {
            this.ddlType.SelectedValue = this.type;

            this.page = DTRequest.GetQueryInt("page", 1);
            BookingFood.BLL.bf_old_order bllOldOrder = new BookingFood.BLL.bf_old_order();
            BookingFood.BLL.bf_old_order_offline bllOldOrderOffline = new BookingFood.BLL.bf_old_order_offline();

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
            if(string.Equals(type,"1"))
            {
                this.rptList.DataSource = bllOldOrder.GetListByPage(_strWhere, _orderby, StartIndex, EndIndex);
                this.rptList.DataBind();
                totalCount = bllOldOrder.GetList(_strWhere).Tables[0].Rows.Count;
            }
            else if (string.Equals(type, "2"))
            {
                this.rptList.DataSource = bllOldOrderOffline.GetListByPage(_strWhere, _orderby, StartIndex, EndIndex);
                this.rptList.DataBind();
                totalCount = bllOldOrderOffline.GetList(_strWhere).Tables[0].Rows.Count;
            }
            

            //绑定页码
            txtPageNum.Text = this.pageSize.ToString();
            string pageUrl = Utils.CombUrlTxt("old_order_list.aspx", "page={0}&keyword={1}&type={2}","__id__",this.keyword,this.type);
            PageContent.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
        }
        #endregion

        #region 组合SQL查询语句==========================
        protected string CombSqlTxt(string keyword)
        {
            StringBuilder strTemp = new StringBuilder();            
            if (!string.IsNullOrEmpty(keyword))
            {
                strTemp.Append(string.Format(" and (ctry like '%{0}%' or adress like '%{0}%' or email like '%{0}%' or tel like '%{0}%')", keyword));
            }            
            return strTemp.ToString();
        }
        #endregion

        #region 返回用户每页数量=========================
        private int GetPageSize(int _default_size)
        {
            int _pagesize;
            if (int.TryParse(Utils.GetCookie("order_list_page_size"), out _pagesize))
            {
                if (_pagesize > 0)
                {
                    return _pagesize;
                }
            }
            return _default_size;
        }
        #endregion        

        //设置分页数量
        protected void txtPageNum_TextChanged(object sender, EventArgs e)
        {
            int _pagesize;
            if (int.TryParse(txtPageNum.Text.Trim(), out _pagesize))
            {
                if (_pagesize > 0)
                {
                    Utils.WriteCookie("order_list_page_size", _pagesize.ToString(), 43200);
                }
            }
            Response.Redirect("old_order_list.aspx");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect(Utils.CombUrlTxt("old_order_list.aspx", "keywords={0}&type={1}",txtKeywords.Text, ddlType.SelectedValue));
        }
    }
}