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
    public partial class bi_map_area_order_list : Web.UI.ManagePage
    {
        protected int totalCount;
        protected int page;
        protected int pageSize;
        protected string type;

        string begintime = string.Empty;
        string endtime = string.Empty;
        protected double totalamount = 0;
        protected string x_title = string.Empty;
        protected string rtn = string.Empty;
        protected string totalwidth = string.Empty;
        

        protected void Page_Load(object sender, EventArgs e)
        {

            begintime = DTRequest.GetQueryString("begintime");
            endtime = DTRequest.GetQueryString("endtime");
            type = DTRequest.GetQueryString("type");
            this.pageSize = GetPageSize(15); //每页数量
            if (!Page.IsPostBack)
            {
                string areaid = Session["AreaId"].ToString();
                ChkAdminLevel("tongjiguanli", DTEnums.ActionEnum.View.ToString()); //检查权限
                if (string.IsNullOrEmpty(this.begintime)) this.begintime = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                if (string.IsNullOrEmpty(this.endtime)) this.endtime = DateTime.Now.ToString("yyyy-MM-dd");
                RptBind("do.id>0" + CombSqlTxt(this.begintime,this.endtime), "id desc");
            }
        }

        #region 数据绑定=================================
        private void RptBind(string _strWhere, string _orderby)
        {
            
            this.txtBeginTime.Text = this.begintime;
            this.txtEndTime.Text = this.endtime;
            this.page = DTRequest.GetQueryInt("page", 1);
            DTcms.BLL.orders bll = new BLL.orders();
            totalamount = bll.GetSum(_strWhere + " and OrderType IN ('网页','转单(区域)','线下订单','电话') and status in (1,2,3)", "order_amount");

            string sqlarea = string.Empty;
            if (!string.IsNullOrEmpty(Session["AreaId"].ToString()))
            {
                sqlarea = string.Format(" and ParentId=" + Session["AreaId"].ToString() );
            }

            List<BookingFood.Model.bf_area> listArea = new BookingFood.BLL.bf_area().GetModelList(" 1=1" + sqlarea + " order by IsShow desc");
            if (listArea.Count == 0)
            {
                return;
            }
            string area = string.Empty;
            foreach (var item in listArea)
            {
                area += string.Format("[{0}],[{0}_On],", item.Title);
            }
            area = area.TrimEnd(',');

            DataTable dt = bll.GetListForAreaBi(9999, this.page, _strWhere, out this.totalCount, area, " riqi asc").Tables[0];
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (DataRow item in dt.Rows)
            {
                x_title += "'" + item["riqi"].ToString().Substring(item["riqi"].ToString().IndexOf('.')+1) + "',";
                foreach (var itemarea in listArea)
                {
                    if(dic.ContainsKey(itemarea.Title))
                    {
                        dic[itemarea.Title] += (decimal.Parse(!string.IsNullOrEmpty(item[itemarea.Title].ToString()) ? item[itemarea.Title].ToString() : "0")
                            + decimal.Parse((!string.IsNullOrEmpty(item[itemarea.Title + "_On"].ToString()) ? item[itemarea.Title + "_On"].ToString() : "0"))).ToString()+",";
                    }
                    else
                    {
                        dic.Add(itemarea.Title, (decimal.Parse(!string.IsNullOrEmpty(item[itemarea.Title].ToString())?item[itemarea.Title].ToString():"0")
                            + decimal.Parse(!string.IsNullOrEmpty(item[itemarea.Title + "_On"].ToString()) ? item[itemarea.Title + "_On"].ToString() : "0")).ToString() + ",");
                    }
                }
            }
            x_title = x_title.TrimEnd(',');

            foreach (var item in dic.Keys)
            {
                rtn += string.Format("{{name: '{0}', data: [{1}]}},", item, dic[item].TrimEnd(','));
            }
            rtn = rtn.TrimEnd(',');
        }
        #endregion

        #region 组合SQL查询语句==========================
        protected string CombSqlTxt(string _begintime,string _endtime)
        {
            StringBuilder strTemp = new StringBuilder();
            if (!string.IsNullOrEmpty(_begintime) )
            {
                strTemp.Append(string.Format(" and do.add_time >='{0}'", _begintime + " 00:00:00"));
            }
            if (!string.IsNullOrEmpty(_endtime))
            {
                strTemp.Append(string.Format(" and do.add_time <='{0}'", _endtime + " 23:59:59"));
            }
            if (!string.IsNullOrEmpty(Session["AreaId"].ToString()))
            {
                strTemp.Append(string.Format(" and do.area_id in (SELECT ba.Id FROM bf_area ba WHERE ba.ParentId=" + Session["AreaId"].ToString() + ")"));
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


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect(Utils.CombUrlTxt("bi_map_area_order_list.aspx", "begintime={0}&endtime={1}", txtBeginTime.Text.Trim(), txtEndTime.Text.Trim()));
        }
    }
}