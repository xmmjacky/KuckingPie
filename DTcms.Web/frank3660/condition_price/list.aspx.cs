using System;
using System.Text;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DTcms.Common;

namespace DTcms.Web.admin.condition_price
{
    public partial class list : Web.UI.ManagePage
    {        
        protected int totalCount;
        protected int page;
        protected int pageSize;        

        protected void Page_Load(object sender, EventArgs e)
        {            
            this.pageSize = GetPageSize(7); //每页数量
            if (!Page.IsPostBack)
            {
                ChkAdminLevel("bf_condition_price", DTEnums.ActionEnum.View.ToString()); //检查权限                
                RptBind("Id>0", "Id asc");
            }
        } 
        

        #region 数据绑定=================================
        private void RptBind(string _strWhere, string _orderby)
        {
            BookingFood.BLL.bf_condition_price bll = new BookingFood.BLL.bf_condition_price();
            this.rptList1.DataSource = bll.GetList(" 1=1 Order By SortId Asc");
            this.rptList1.DataBind();            
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

        //批量删除
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            ChkAdminLevel("bf_condition_price", DTEnums.ActionEnum.Delete.ToString()); //检查权限
            BookingFood.BLL.bf_condition_price bll = new BookingFood.BLL.bf_condition_price();
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
    }
}