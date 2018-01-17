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
    public partial class bi_area_order_list : Web.UI.ManagePage
    {
        protected int totalCount;
        protected int page;
        protected int pageSize;
        protected string type;

        string begintime = string.Empty;
        string endtime = string.Empty;
        protected double totalamount = 0;
        

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
            totalamount = bll.GetSum(_strWhere + " and OrderType IN ('网页','转单(区域)','线下订单','电话','微信') and status in (1,2,3) AND (payment_id = 1 OR ((payment_id = 3 OR payment_id = 5 OR payment_id = 2 OR payment_id = 6) AND payment_status = 2 ))", "order_amount");

            string sqlarea = string.Empty;
            if (!string.IsNullOrEmpty(Session["AreaId"].ToString()))
            {
                sqlarea = string.Format(" and ParentId=" + Session["AreaId"].ToString() );
            }

            string rtn = string.Empty;
            List<BookingFood.Model.bf_area> listArea = new BookingFood.BLL.bf_area().GetModelList(" 1=1" + sqlarea + " order by IsShow desc");
            if (listArea.Count == 0)
            {
                rtn += "<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"msgtable\">";
                rtn += "<tr><td align=\"center\" >暂无记录</td></tr>";
                rtn += "</table>";
                ltlTable.Text = rtn;
                return;
            }
            string area = string.Empty;
            foreach (var item in listArea)
            {
                area += string.Format("[{0}],[{0}_On],", item.Title);
            }
            area = area.TrimEnd(',');
            DataTable dt = bll.GetListForAreaBi(this.pageSize, this.page, _strWhere, out this.totalCount,area).Tables[0];
            int cellwidth = 130;
            rtn += "<table  border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"msgtable\" style=\"width:" + ((listArea.Count + 4) * cellwidth) + "px\">";
            rtn += "<tr>";
            rtn += "<th width=\"" + cellwidth + "px\">日期</th>";
            foreach (var item in listArea)
            {
                rtn += "<th width=\"" + cellwidth + "px\">" + item.Title + "</th>";
            }
            rtn += "<th width=\"" + cellwidth + "px\">网上订单</th>";
            rtn += "<th width=\"" + cellwidth + "px\">实体销售</th>";
            rtn += "<th width=\"" + cellwidth + "px\">总销售</th>";
            rtn += "</tr>";
            bool isOut = false;
            foreach (DataRow item in dt.Rows)
            {
                rtn += "<tr>";
                rtn += "<td  align=\"center\">" + item["riqi"].ToString().Replace('.', '-') + "</td>";
                foreach (var itemarea in listArea)
                {
                    rtn += "<td align=\"center\">" + item[itemarea.Title].ToString() 
                        + (!string.IsNullOrEmpty(item[itemarea.Title+"_On"].ToString())? "<font style=\"color:red;\">"+item[itemarea.Title+"_On"].ToString()+"</font>" : "") + "</td>";
                }
                rtn += "<td  align=\"center\">" + item["onlineamount"].ToString() + "</td>";
                rtn += "<td  align=\"center\">" + item["offlineamount"].ToString() + "</td>";
                rtn += "<td  align=\"center\">" + item["totalamount"].ToString() + "</td>";                
                rtn += "</tr>";
            }
            rtn += "</table>";
            ltlTable.Text = rtn;

            //绑定页码
            txtPageNum.Text = this.pageSize.ToString();
            string pageUrl = Utils.CombUrlTxt("bi_area_order_list.aspx", "page={0}&begintime={1}&endtime={2}","__id__",this.begintime,this.endtime);
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
            Response.Redirect("bi_area_order_list.aspx");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect(Utils.CombUrlTxt("bi_area_order_list.aspx", "begintime={0}&endtime={1}", txtBeginTime.Text.Trim(), txtEndTime.Text.Trim()));
        }
    }
}