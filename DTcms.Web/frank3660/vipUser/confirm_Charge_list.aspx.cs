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
    public partial class confirm_Charge_list : System.Web.UI.Page
    {
        private readonly SonConnection db;
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
            db = new SonConnection();
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
                string areaid = Session["AreaId"].ToString();
                List<BookingFood.Model.bf_area> listArea = new BookingFood.BLL.bf_area().GetModelList(" ParentId=" + areaid);
                foreach (var item in listArea)
                {
                    cboChangeArea.Items.Add(new ListItem(item.Title, item.Id.ToString()));
                    areafilter += string.Format("<a href=\"confirm_Charge_list.aspx?type={0}&area={1}\">{2}</a> ", this.type, item.Id.ToString(), item.Title);
                }
                //ChkAdminLevel("orders", DTEnums.ActionEnum.View.ToString()); //检查权限
                RptBind("id>0" + CombSqlTxt(this.keyword, this.area), "CreateTime desc,Id desc");
            }
        }
        #region 数据绑定=================================
        private void RptBind(string _strWhere, string _orderby)
        {

            var res = db.ExecuteSql(_strWhere);
            this.rptList.DataSource = res;
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
                strTemp.Append(string.Format(" and (w2.NickName like '%{0}%')", keyword));
            }
           
            if (!string.IsNullOrEmpty(_area))
            {
                strTemp.Append(string.Format(" and w2.area_id=" + _area));
            }
            
            if (!string.IsNullOrEmpty(Session["AreaId"].ToString()))
            {
                strTemp.Append(string.Format(" and w2.area_id in (SELECT ba.Id FROM bf_area ba WHERE ba.ParentId=" + Session["AreaId"].ToString() + ")"));
            }
            if (userid > 0)
            {
                strTemp.Append(" and user_id=" + userid.ToString() + " ");
            }
           
            var sqlstr = string.Format(@"SELECT w2.n, w1.* FROM dt_user_Top_up w1,
(SELECT TOP {0} row_number() OVER (ORDER BY CreateTime DESC, Id DESC) n, Id FROM dt_user_Top_up) w2 
WHERE w1.Id = w2.Id AND w2.n > {1} AND {2} ORDER BY w2.n ASC ", this.pageSize * this.page, (this.page - 1) * this.pageSize,strTemp);
            return strTemp.ToString();
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
    }
}