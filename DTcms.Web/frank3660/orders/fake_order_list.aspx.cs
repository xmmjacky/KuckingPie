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
    public partial class fake_order_list : Web.UI.ManagePage
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
            this.btnClear.Click += new EventHandler(btnClear_Click);
            if (!Page.IsPostBack)
            {
                string areaid = Session["AreaId"].ToString();
                List<BookingFood.Model.bf_area> listArea = new BookingFood.BLL.bf_area().GetModelList(" ParentId=" + areaid);
                foreach (var item in listArea)
                {
                    cboChangeArea.Items.Add(new ListItem(item.Title, item.Id.ToString()));
                }
                ChkAdminLevel("dingdanhuishouzhan", DTEnums.ActionEnum.View.ToString()); //检查权限
                RptBind("id>0" + CombSqlTxt(this.keyword), "add_time desc,id desc");
            }
        }

        void btnClear_Click(object sender, EventArgs e)
        {
            BLL.orders bll = new BLL.orders();
            DataTable dt = bll.GetList(0, "id>0" + CombSqlTxt(this.keyword), "add_time desc,id desc").Tables[0];
            foreach (DataRow item in dt.Rows)
            {
                bll.Delete(int.Parse(item["id"].ToString()));
            }
            Response.Redirect(Request.Url.ToString());
        }

        #region 数据绑定=================================
        private void RptBind(string _strWhere, string _orderby)
        {
            this.page = DTRequest.GetQueryInt("page", 1);            
            BLL.orders bll = new BLL.orders();
            this.rptList.DataSource = bll.GetListForPage(this.pageSize, this.page, _strWhere, _orderby, out this.totalCount);
            this.rptList.DataBind();

            //绑定页码
            txtPageNum.Text = this.pageSize.ToString();
            string pageUrl = Utils.CombUrlTxt("fake_order_list.aspx", "page={0}&keyword={1}&type={2}","__id__",this.keyword,this.type);
            PageContent.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
        }
        #endregion

        #region 组合SQL查询语句==========================
        protected string CombSqlTxt(string keyword)
        {
            StringBuilder strTemp = new StringBuilder();
            //if (!string.IsNullOrEmpty(Session["AreaId"].ToString()))
            //{
            //    strTemp.Append(" and area_id=" + Session["AreaId"].ToString());
            //}
            strTemp.Append(" and status=5 ");
            if (!string.IsNullOrEmpty(keyword))
            {
                strTemp.Append(string.Format(" and (address like '%{0}%' or telphone like '%{0}%' or email like '%{0}%' or order_no like '%{0}%')", keyword));
            }
            if (!string.IsNullOrEmpty(type))
            {
                if (string.Equals(type, "0"))
                {
                    strTemp.Append(" and OrderType!='线下订单'");
                }
                else if (string.Equals(type, "1"))
                {
                    strTemp.Append(" and OrderType='线下订单'");
                }
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

        #region 返回订单状态=============================
        protected string GetOrderStatus(int _id,string upamount,string uptotal,string downamount,string downtotal)
        {
            string _title = "";
            Model.orders model = new BLL.orders().GetModel(_id);
            switch (model.status)
            {
                case 1:
                    _title = "<span style=\"color:red;\">未下载<span>";
                    //Model.payment payModel = new BLL.payment().GetModel(model.payment_id);
                    //if (payModel != null && payModel.type == 1)
                    //{
                    //    if (model.payment_status > 1)
                    //    {
                    //        _title = "付款成功";
                    //    }
                    //    else
                    //    {
                    //        _title = "等待付款";
                    //    }
                    //}
                    break;
                case 2:
                    _title = "<span style=\"color:green;\">已下载<span>";
                    if (model.distribution_status > 1)
                    {
                        BookingFood.Model.bf_worker worker  = new BookingFood.BLL.bf_worker().GetModel(model.worker_id);
                        if (worker != null)
                        {
                            _title = string.Format("{0} {1}<br/><span style=\"color:green;\">{3}({4}){5}({6})</span><br/><span style=\"color:red;\">{2}</span>", worker.Title, worker.Telphone
                                , model.distribution_time!=null?((DateTime)model.distribution_time).ToString("HH:mm:ss") : ""
                                , upamount,uptotal,downamount,downtotal);
                        }
                        
                    }
                    
                    break;
                case 3:
                    _title = "交易完成";
                    break;
                case 4:
                    _title = "订单取消";
                    break;
                case 5:
                    _title = "订单作废";
                    break;
            }

            return _title;
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
            Response.Redirect("fake_order_list.aspx");
        }        

        protected string GetManagerAndOrderCount(string areaid)
        {
            if (areaid=="0") return "";
            string rtn = string.Empty;
            BookingFood.Model.bf_area model = 
                new BookingFood.BLL.bf_area().GetModel(int.Parse(areaid));
            Model.manager manager = new BLL.manager().GetModel((int)model.ManagerId);
            if (manager != null)
            {
                rtn = string.Format("{0} ({1})单)",manager.user_name,manager.OrderCount);
            }
            return rtn;
        }

        protected string GetOrderDetail(string orderid)
        {
            string rtn = string.Empty;
            Model.orders model = new BLL.orders().GetModel(int.Parse(orderid));
            if (model.order_goods == null) return rtn;
            foreach (var item in model.order_goods)
	        {
                
                if (item.type == "combo")
                {
                    rtn += string.Format("{0}&nbsp;&nbsp;&nbsp;&nbsp;{1}份&nbsp;&nbsp;&nbsp;&nbsp;{2}元<br/>", item.goods_name, item.quantity, item.quantity * item.real_price);
                    if (!string.IsNullOrEmpty(item.subgoodsid))
                    {
                        string[] subgoods = item.subgoodsid.Split('†');
                        foreach (var sub in subgoods)
                        {
                            if (sub.Split('‡')[0] == "taste")
                            {
                                rtn += string.Format("口味 *{0} ", sub.Split('‡')[1]);
                            }
                            else
                            {
                                rtn += string.Format("*{0} ", sub.Split('‡')[2]);
                            }
                        }
                        rtn += "<br/>";
                    }                    
                }
                else if (item.type == "one")
                {
                    rtn += string.Format("{0} {3}{1}份&nbsp;&nbsp;&nbsp;&nbsp;{2}元<br/>"
                        , item.goods_name, item.quantity, item.quantity * item.real_price
                        , !string.IsNullOrEmpty(item.subgoodsid) ? "*"+item.subgoodsid.Split('‡')[2] : "");                                     
                }
                
	        }
            rtn += "-----------------------------------------------<br />";
            return rtn;
        }

        protected void rptList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            ChkAdminLevel("dingdanhuishouzhan", DTEnums.ActionEnum.Delete.ToString()); //检查权限
            int id = Convert.ToInt32(e.CommandArgument.ToString());
            BLL.orders bll = new BLL.orders();
            Model.orders model = bll.GetModel(id);
            switch (e.CommandName.ToLower())
            {
                case "delete":
                    bll.UpdateField(id, "status="+model.restore_status.ToString());
                    break;
            }
            RptBind("id>0" + CombSqlTxt(this.keyword), "add_time desc,id desc");
        }
    }
}