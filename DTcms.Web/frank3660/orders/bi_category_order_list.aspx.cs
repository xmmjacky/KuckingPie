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
    public partial class bi_category_order_list : Web.UI.ManagePage
    {
        protected int totalCount;
        protected int page;
        protected int pageSize;
        protected string category;
        protected double totalamount = 0;

        string begintime = string.Empty;
        string endtime = string.Empty;
        

        protected void Page_Load(object sender, EventArgs e)
        {

            begintime = DTRequest.GetQueryString("begintime");
            endtime = DTRequest.GetQueryString("endtime");
            category = DTRequest.GetQueryString("category");
            this.pageSize = GetPageSize(15); //每页数量
            if (!Page.IsPostBack)
            {
                string areaid = Session["AreaId"].ToString();
                ChkAdminLevel("tongjiguanli", DTEnums.ActionEnum.View.ToString()); //检查权限
                if (!string.IsNullOrEmpty(this.category))
                {
                    RptBindForGoods("do.id>0" + CombSqlTxt(this.begintime, this.endtime), "id desc");
                }
                else
                {
                    RptBind("do.id>0" + CombSqlTxt(this.begintime, this.endtime), "id desc");
                }
                
            }
        }

        #region 数据绑定=================================
        private void RptBind(string _strWhere, string _orderby)
        {
            this.txtBeginTime.Text = this.begintime;
            this.txtEndTime.Text = this.endtime;
            this.page = DTRequest.GetQueryInt("page", 1);
            DTcms.BLL.orders bll = new BLL.orders();
            string tempStrWhere = string.Empty;
            if (!string.IsNullOrEmpty(Session["AreaId"].ToString()))
            {
                tempStrWhere = _strWhere + string.Format(" and area_id in (SELECT ba.Id FROM bf_area ba WHERE ba.ParentId=" + Session["AreaId"].ToString()+") ");
            }
            totalamount = bll.GetSum(tempStrWhere + " and OrderType IN ('网页','转单(区域)','线下订单','电话') and status in (1,2,3)", "real_amount");

            string rtn = string.Empty;
            DataTable dtcategory = bll.GetOrderGoodsDistinct(" category_title is not null and dog.order_id in (select id from dt_orders do where " + tempStrWhere 
                    + " and OrderType IN ('网页','转单(区域)','线下订单','电话') and status in (1,2,3))", "category_title").Tables[0];
            if (dtcategory.Rows.Count == 0)
            {
                rtn += "<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"msgtable\">";
                rtn += "<tr><td align=\"center\" >暂无记录</td></tr>";
                rtn += "</table>";
                ltlTable.Text = rtn;
                return;
            }
            string area = string.Empty;
            foreach (DataRow item in dtcategory.Rows)
            {
                area += string.Format("[{0}],", item["category_title"].ToString());
                if (this.category == item["category_title"].ToString())
                {
                    ltlCategory.Text += string.Format("<span style=\"cursor:pointer;display: inline-block;font-size: 16px;margin-left: 10px;width: 90px;text-align: center;background-color: rgb(183, 233, 69);border-radius: 5px;color: white;\" onclick=\"location.href='bi_category_order_list.aspx'\">{0}</span>", item["category_title"].ToString());
                }
                else
                {
                    ltlCategory.Text += string.Format("<span style=\"cursor:pointer;display: inline-block;font-size: 16px;margin-left: 10px;width: 90px;text-align: center;\" onclick=\"location.href='bi_category_order_list.aspx?category={0}'\">{0}</span>", item["category_title"].ToString());
                }
                
            }
            area = area.TrimEnd(',');
            if (!string.IsNullOrEmpty(Session["AreaId"].ToString()))
            {
                tempStrWhere = _strWhere + string.Format(" AND ba.ParentId=" + Session["AreaId"].ToString());
            }
            DataTable dt = bll.GetListForCategoryBi(this.pageSize, this.page, tempStrWhere, out this.totalCount, area).Tables[0];

            rtn += "<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"msgtable\" style=\"width:" + ((dtcategory.Rows.Count + 1) * 100) + "px\">";
            rtn += "<tr > ";
            rtn += "<th width=\"100px\" >日期</th>";
            foreach (DataRow item in dtcategory.Rows)
            {
                rtn += "<th width=\"100px\" >" + item["category_title"].ToString() + "</th>";
            }            
            rtn += "</tr>";
            foreach (DataRow item in dt.Rows)
            {
                rtn += "<tr>";
                rtn += "<td  align=\"center\">" + item["addtime"].ToString().Replace('.', '-') + "</td>";
                foreach (DataRow subitem in dtcategory.Rows)
                {
                    rtn += "<td  align=\"center\">" + item[subitem["category_title"].ToString()].ToString() + "</td>";
                }                
                rtn += "</tr>";
            }
            rtn += "</table>";
            ltlTable.Text = rtn;

            //绑定页码
            txtPageNum.Text = this.pageSize.ToString();
            string pageUrl = Utils.CombUrlTxt("bi_category_order_list.aspx", "page={0}&begintime={1}&endtime={2}","__id__",this.begintime,this.endtime);
            PageContent.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
        }

        private void RptBindForGoods(string _strWhere, string _orderby)
        {
            this.txtBeginTime.Text = this.begintime;
            this.txtEndTime.Text = this.endtime;
            this.page = DTRequest.GetQueryInt("page", 1);
            DTcms.BLL.orders bll = new BLL.orders();
            string tempStrWhere = string.Empty;
            if (!string.IsNullOrEmpty(Session["AreaId"].ToString()))
            {
                tempStrWhere = _strWhere + string.Format(" and area_id in (SELECT ba.Id FROM bf_area ba WHERE ba.ParentId=" + Session["AreaId"].ToString() + ") ");
            }
            totalamount = bll.GetOrderGoodsSum(" dog.category_title='"+this.category+"' and dog.order_id in (select id from dt_orders do where " + tempStrWhere + " and OrderType IN ('网页','转单(区域)','线下订单','电话') and status in (1,2,3))", "goods_price*quantity");

            string rtn = string.Empty;

            DataTable dtcategorytitle = bll.GetOrderGoodsDistinct(" category_title is not null and dog.order_id in (select id from dt_orders do where " + tempStrWhere
                    + " and OrderType IN ('网页','转单(区域)','线下订单','电话') and status in (1,2,3))", "category_title").Tables[0];
            foreach (DataRow item in dtcategorytitle.Rows)
            {
                if (this.category == item["category_title"].ToString())
                {
                    ltlCategory.Text += string.Format("<span style=\"cursor:pointer;display: inline-block;font-size: 16px;margin-left: 10px;width: 90px;text-align: center;background-color: rgb(183, 233, 69);border-radius: 5px;color: white;\" onclick=\"location.href='bi_category_order_list.aspx'\">{0}</span>", item["category_title"].ToString());
                }
                else
                {
                    ltlCategory.Text += string.Format("<span style=\"cursor:pointer;display: inline-block;font-size: 16px;margin-left: 10px;width: 90px;text-align: center;\" onclick=\"location.href='bi_category_order_list.aspx?category={0}'\">{0}</span>", item["category_title"].ToString());
                }

            }

            DataTable dtcategory = bll.GetOrderGoodsDistinct(" dog.category_title='" + this.category + "' and dog.order_id in (select id from dt_orders do where " + tempStrWhere 
                    + " and OrderType IN ('网页','转单(区域)','线下订单','电话') and status in (1,2,3))", "goods_name").Tables[0];
            if (dtcategory.Rows.Count == 0)
            {
                rtn += "<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"msgtable\">";
                rtn += "<tr><td align=\"center\" >暂无记录</td></tr>";
                rtn += "</table>";
                ltlTable.Text = rtn;
                return;
            }
            string area = string.Empty;
            foreach (DataRow item in dtcategory.Rows)
            {
                area += string.Format("[{0}],", item["goods_name"].ToString());
            }
            area = area.TrimEnd(',');
            if (!string.IsNullOrEmpty(Session["AreaId"].ToString()))
            {
                tempStrWhere = _strWhere + " and dog.category_title='" + this.category + "' " + string.Format(" AND ba.ParentId=" + Session["AreaId"].ToString());
            }
            DataTable dt = bll.GetListForGoodsBi(this.pageSize, this.page, tempStrWhere, out this.totalCount, area).Tables[0];

            rtn += "<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"msgtable\" style=\"width:" + ((dtcategory.Rows.Count+1)*100) + "px\">";
            rtn += "<tr>";
            rtn += "<th width=\"100px\"  >日期</th>";
            foreach (DataRow item in dtcategory.Rows)
            {
                rtn += "<th width=\"100px\"  >" + item["goods_name"].ToString() + "</th>";
            }
            rtn += "</tr>";
            foreach (DataRow item in dt.Rows)
            {
                rtn += "<tr>";
                rtn += "<td  align=\"center\">" + item["addtime"].ToString().Replace('.', '-') + "</td>";
                foreach (DataRow subitem in dtcategory.Rows)
                {
                    rtn += "<td  align=\"center\">" + item[subitem["goods_name"].ToString()].ToString() + "</td>";
                }
                rtn += "</tr>";
            }
            rtn += "</table>";
            ltlTable.Text = rtn;

            //绑定页码
            txtPageNum.Text = this.pageSize.ToString();
            string pageUrl = Utils.CombUrlTxt("bi_category_order_list.aspx", "page={0}&begintime={1}&endtime={2}&category={3}", "__id__", this.begintime, this.endtime,this.category);
            PageContent.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
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
            Response.Redirect("bi_category_order_list.aspx");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect(Utils.CombUrlTxt("bi_category_order_list.aspx", "begintime={0}&endtime={1}", txtBeginTime.Text.Trim(), txtEndTime.Text.Trim()));
        }
    }
}