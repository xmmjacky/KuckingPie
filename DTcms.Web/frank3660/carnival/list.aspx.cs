using System;
using System.Text;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DTcms.Common;

namespace DTcms.Web.admin.carnival
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
                ChkAdminLevel("bf_carnival", DTEnums.ActionEnum.View.ToString()); //检查权限                
                RptBind("id>0" + CombSqlTxt(this.keywords), "BeginTime desc");
            }
        } 
        

        #region 数据绑定=================================
        private void RptBind(string _strWhere, string _orderby)
        {
            this.page = DTRequest.GetQueryInt("page", 1);
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
            //图表或列表显示
            BookingFood.BLL.bf_carnival bll = new BookingFood.BLL.bf_carnival();
            this.rptList1.DataSource = bll.GetListByPage(_strWhere, _orderby, StartIndex, EndIndex);
            this.rptList1.DataBind();
            totalCount = bll.GetList(_strWhere).Tables[0].Rows.Count;
            //绑定页码
            txtPageNum.Text = this.pageSize.ToString();
            string pageUrl = Utils.CombUrlTxt("list.aspx", "keywords={0}&page={1}",this.keywords, "__id__");
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
                strTemp.Append(" and Title like '%" + _keywords + "%'");
            }            
            return strTemp.ToString();
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
            Response.Redirect(Utils.CombUrlTxt("list.aspx", "keywords={0}",this.keywords));
        }

        //批量删除
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            ChkAdminLevel("bf_carnival", DTEnums.ActionEnum.Delete.ToString()); //检查权限
            BookingFood.BLL.bf_carnival bll = new BookingFood.BLL.bf_carnival();
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
            JscriptMsg("批量删除成功啦！", Utils.CombUrlTxt("list.aspx", "keywords={0}",this.keywords), "Success");
        }


        //关健字查询
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect(Utils.CombUrlTxt("list.aspx", "keywords={0}",txtKeywords.Text));
        }
        
    }
}