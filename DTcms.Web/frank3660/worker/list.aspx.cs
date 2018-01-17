using System;
using System.Text;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DTcms.Common;

namespace DTcms.Web.admin.worker
{
    public partial class list : Web.UI.ManagePage
    {        
        protected int totalCount;
        protected int page;
        protected int pageSize;
        protected string area_id = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {            
            this.pageSize = GetPageSize(7); //每页数量
            this.area_id = DTRequest.GetQueryString("area_id");
            DoInitArea();
            if (!Page.IsPostBack)
            {
                ChkAdminLevel("worker", DTEnums.ActionEnum.View.ToString()); //检查权限                
                RptBind("id>0" + CombSqlTxt(this.area_id), "SortId asc");
            }
        }

        #region 组合SQL查询语句==========================
        protected string CombSqlTxt(string _areaid)
        {
            StringBuilder strTemp = new StringBuilder();
            if (!string.IsNullOrEmpty(_areaid))
            {
                strTemp.Append(" and Id In (SELECT baw.WorkerId FROM bf_area_worker baw WHERE baw.AreaId="+_areaid+")");
            }
            if (!string.IsNullOrEmpty(Session["AreaId"].ToString()))
            {
                strTemp.Append(" and id in(SELECT baa.WorkerId FROM bf_area_worker baa,bf_area ba WHERE baa.AreaId=ba.Id AND ba.ParentId=" + Session["AreaId"].ToString() + ")");
            }
            return strTemp.ToString();
        }
        #endregion

        #region 数据绑定=================================
        private void RptBind(string _strWhere, string _orderby)
        {
            this.page = DTRequest.GetQueryInt("page", 1);            
            //图表或列表显示
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
            BookingFood.BLL.bf_worker bll = new BookingFood.BLL.bf_worker();
            this.rptList1.DataSource = bll.GetListByPage(_strWhere, _orderby, StartIndex, EndIndex);
            this.rptList1.DataBind();
            totalCount = bll.GetList(_strWhere).Tables[0].Rows.Count;
            //绑定页码
            txtPageNum.Text = this.pageSize.ToString();
            string pageUrl = Utils.CombUrlTxt("list.aspx", "page={0}", "__id__");
            PageContent.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
        }
        #endregion
        

        #region 返回图文每页数量=========================
        private int GetPageSize(int _default_size)
        {
            int _pagesize;
            if (int.TryParse(Utils.GetCookie("goods_page_size"), out _pagesize))
            {
                if (_pagesize > 0)
                {
                    return _pagesize;
                }
            }
            return _default_size;
        }
        #endregion

        //设置操作
        protected void rptList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            ChkAdminLevel("worker", DTEnums.ActionEnum.Delete.ToString()); //检查权限
            int id = Convert.ToInt32(((HiddenField)e.Item.FindControl("hidId")).Value);
            string arg = e.CommandArgument.ToString();
            BookingFood.BLL.bf_worker bll = new BookingFood.BLL.bf_worker();            
            switch (e.CommandName.ToLower())
            {                
                case "delete":
                    bll.Delete(id);
                    break;
            }
            this.RptBind("id>0", "SortId asc");
        }        

        //设置分页数量
        protected void txtPageNum_TextChanged(object sender, EventArgs e)
        {
            int _pagesize;
            if (int.TryParse(txtPageNum.Text.Trim(), out _pagesize))
            {
                if (_pagesize > 0)
                {
                    Utils.WriteCookie("goods_page_size", _pagesize.ToString(), 43200);
                }
            }
            Response.Redirect(Utils.CombUrlTxt("list.aspx", "",""));
        }               

        //设置区域显示
        protected void DoInitArea()
        {
            string rtn = "";
            string areaid = Session["areaid"].ToString();
            DataTable dt = new BookingFood.BLL.bf_area().GetList(" ParentId=" + areaid + " Order By SortId").Tables[0];
            foreach (DataRow item in dt.Rows)
            {
                if (area_id.ToString() == item["id"].ToString())
                {
                    rtn += string.Format("<li class=\"reverse\" onclick=\"location.href='list.aspx?area_id={1}'\">{0}</li>"
                        , item["Title"].ToString(), item["Id"].ToString());
                }
                else
                {
                    rtn += string.Format("<li onclick=\"location.href='list.aspx?area_id={1}'\">{0}</li>"
                        , item["Title"].ToString(), item["Id"].ToString());
                }
            }
            ltlArea.Text = rtn;
        }
    }
}