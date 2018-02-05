using DTcms.Common;
using Orm.Son.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DTcms.Web.frank3660.vipUser
{
    public partial class confirm_Charge_list : Web.UI.ManagePage
    {
        private readonly SonConnection db= new SonConnection("ConnectionString");
        protected int totalCount;
        protected int page;
        protected int pageSize;
        string keyword = string.Empty;
        string area = string.Empty;
        int userid = 0;
        protected string type;
        protected string areafilter = string.Empty;
        public confirm_Charge_list()
        {
           
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            keyword = DTRequest.GetQueryString("keyword");
            area = DTRequest.GetQueryString("area");
            userid = DTRequest.GetQueryInt("userid");
            this.pageSize = GetPageSize(15); //每页数量
            type = DTRequest.GetQueryString("type");
            if (!Page.IsPostBack)
            {
                string areaid = "";
                if (Session["AreaId"] != null)
                {
                    areaid=Session["AreaId"].ToString();
                }
                List<BookingFood.Model.bf_area> listArea = new BookingFood.BLL.bf_area().GetModelList(" ParentId=" + areaid);
                foreach (var item in listArea)
                {
                    cboChangeArea.Items.Add(new ListItem(item.Title, item.Id.ToString()));
                    areafilter += string.Format("<a href=\"confirm_Charge_list.aspx?type={0}&area={1}\">{2}</a> ", this.type, item.Id.ToString(), item.Title);
                }
                //ChkAdminLevel("orders", DTEnums.ActionEnum.View.ToString()); //检查权限
                RptBind( CombSqlTxt(this.keyword, this.area), "CreateTime desc,Id desc");
            }
        }
        #region 数据绑定=================================
        private void RptBind(string _strWhere, string _orderby)
        {

            var res = db.ExecuteQuery(_strWhere);
            this.rptList.DataSource = res.Tables[0];
            this.rptList.DataBind();
            if (res != null) this.totalCount = 10;
            //绑定页码
            txtPageNum.Text = this.pageSize.ToString();
            string pageUrl = Utils.CombUrlTxt("confirm_Charge_list.aspx", "page={0}&keyword={1}&area={2}&userid={3}"
                , "__id__", this.keyword,  this.area,  this.userid.ToString());
            PageContent.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
        }
        #endregion

        #region 组合SQL查询语句==========================
        protected string CombSqlTxt(string keyword, string _area)
        {
            this.page = DTRequest.GetQueryInt("page", 1);
            StringBuilder strTemp = new StringBuilder();
          
            if (!string.IsNullOrEmpty(keyword))
            {
                strTemp.Append(string.Format(" and (w1.NickName like '%{0}%')", keyword));
            }
           
            if (!string.IsNullOrEmpty(_area))
            {
                strTemp.Append(string.Format(" and w1.AreaId=" + _area));
            }
            
            if (!string.IsNullOrEmpty(Session["AreaId"].ToString()))
            {
                strTemp.Append(string.Format(" and w1.AreaId in (SELECT ba.Id FROM bf_area ba WHERE ba.ParentId=" + Session["AreaId"].ToString() + ")"));
            }
            if (userid > 0)
            {
                strTemp.Append(" and w1.UserId=" + userid.ToString() + " ");
            }
           
            var sqlstr = string.Format(@"SELECT w2.n, w1.* FROM dt_user_Top_up w1,
(SELECT TOP {0} row_number() OVER (ORDER BY CreateTime DESC, Id DESC) n, Id FROM dt_user_Top_up) w2 
WHERE w1.Id = w2.Id AND w2.n > {1} AND w1.Paystate=1 AND w1.Type=0 AND w1.State=0 {2} ORDER BY w2.n ASC ", this.pageSize * this.page, (this.page - 1) * this.pageSize,strTemp);
            return sqlstr.ToString();
        }
        #endregion
        #region 返回用户每页数量=========================
        private int GetPageSize(int _default_size)
        {
            int _pagesize;
            if (int.TryParse(Utils.GetCookie("charge_list_page_size"), out _pagesize))
            {
                if (_pagesize > 0)
                {
                    return _pagesize;
                }
            }
            return _default_size;
        }
        #endregion

        protected void rptList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            //ChkAdminLevel("orders", DTEnums.ActionEnum.Edit.ToString()); //检查权限
            int id = Convert.ToInt32(e.CommandArgument.ToString());

        }
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
            Response.Redirect("confirm_Charge_list.aspx?type=" + type);
        }
    }
}