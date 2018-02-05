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
    public partial class order_list : Web.UI.ManagePage
    {
        protected int totalCount;
        protected int page;
        protected int pageSize;
        protected string type;
        protected string areafilter = string.Empty;
        protected string telphone = string.Empty;

        string keyword = string.Empty;
        string area = string.Empty;
        int userid = 0;

        protected void Page_Load(object sender, EventArgs e)
        {

            keyword = DTRequest.GetQueryString("keyword");
            type = DTRequest.GetQueryString("type");
            area = DTRequest.GetQueryString("area");
            telphone = DTRequest.GetQueryString("telphone");
            userid = DTRequest.GetQueryInt("userid");
            this.pageSize = GetPageSize(15); //每页数量
            if (!Page.IsPostBack)
            {
                string areaid = Session["AreaId"].ToString();
                List<BookingFood.Model.bf_area> listArea = new BookingFood.BLL.bf_area().GetModelList(" ParentId=" + areaid);
                foreach (var item in listArea)
                {
                    cboChangeArea.Items.Add(new ListItem(item.Title, item.Id.ToString()));
                    areafilter += string.Format("<a href=\"order_list.aspx?type={0}&area={1}\">{2}</a> ", this.type, item.Id.ToString(),item.Title);
                }
                ChkAdminLevel("orders", DTEnums.ActionEnum.View.ToString()); //检查权限
                RptBind("id>0" + CombSqlTxt(this.keyword,this.area), "add_time desc,id desc");
            }
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
            string pageUrl = Utils.CombUrlTxt("order_list.aspx", "page={0}&keyword={1}&type={2}&area={3}&telphone={4}&userid={5}"
                ,"__id__",this.keyword,this.type,this.area,this.telphone,this.userid.ToString());
            PageContent.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
        }
        #endregion

        #region 组合SQL查询语句==========================
        protected string CombSqlTxt(string keyword,string _area)
        {
            StringBuilder strTemp = new StringBuilder();
            //if (!string.IsNullOrEmpty(Session["AreaId"].ToString()))
            //{
            //    strTemp.Append(" and area_id=" + Session["AreaId"].ToString());
            //}
            if (!string.IsNullOrEmpty(keyword))
            {
                strTemp.Append(string.Format(" and (address like '%{0}%' or telphone like '%{0}%' or email like '%{0}%' or order_no like '%{0}%' or accept_name like '%{0}%')", keyword));
            }
            if (!string.IsNullOrEmpty(telphone))
            {
                strTemp.Append(string.Format(" and telphone='{0}'", telphone));
            }
            if (!string.IsNullOrEmpty(_area))
            {
                strTemp.Append(string.Format(" and do.area_id="+ _area));
            }
            if (!string.IsNullOrEmpty(type))
            {
                if (string.Equals(type, "1"))
                {
                    strTemp.Append(" and OrderType!='线下订单'");
                }
                else if (string.Equals(type, "2"))
                {
                    strTemp.Append(" and OrderType='线下订单'");
                }
            }
            if (!string.IsNullOrEmpty(Session["AreaId"].ToString()))
            {
                strTemp.Append(string.Format(" and do.area_id in (SELECT ba.Id FROM bf_area ba WHERE ba.ParentId=" + Session["AreaId"].ToString() + ")"));
            }
            if(userid>0)
            {
                strTemp.Append(" and user_id="+userid.ToString() + " ");
            }
            strTemp.Append(" and status in (1,2,3) ");
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
        protected string GetOrderStatus(int _id,string upamount,string uptotal,string downamount,string downtotal,string type)
        {
            string _title = "";
            Model.orders model = new BLL.orders().GetModel(_id);
            if (type == "2")
            {
                if (model.payment_id == 3 || model.payment_id == 5 || model.payment_id == 6||model.payment_id==9 )
                {
                    switch (model.status)
                    {
                        case 1:
                            _title = "<span style=\"color:red;\">未下载<span>";
                            break;
                        case 2:
                            _title = "<span style=\"color:green;\">已下载<span>";
                            if (model.distribution_status > 1)
                            {
                                BookingFood.Model.bf_worker worker = new BookingFood.BLL.bf_worker().GetModel(model.worker_id);
                                if (worker != null)
                                {
                                    _title = string.Format("{0} <span style=\"color:red;\">{2}</span>", worker.Title, worker.Telphone
                                        , model.distribution_time != null ? ((DateTime)model.distribution_time).ToString("HH:mm") : ""
                                        , upamount, uptotal, downamount, downtotal);
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
                }
                else if(model.payment_id==1 || model.payment_id == 7 || model.payment_id == 8)
                {
                    if(model.status==4)
                    {
                        _title = "订单取消";
                    }
                    else if (model.status == 5)
                    {
                        _title = "订单作废";
                    }
                    else
                    {
                        switch (model.is_download)
                        {
                            case 0:
                                _title = "<span style=\"color:red;\">未下载<span>";
                                break;

                            case 1:

                                _title = "<span style=\"color:green;\">已下载<span>";
                                if (model.distribution_status > 1)
                                {
                                    BookingFood.Model.bf_worker worker = new BookingFood.BLL.bf_worker().GetModel(model.worker_id);
                                    if (worker != null)
                                    {
                                        _title = string.Format("{0} <span style=\"color:red;\">{2}</span>", worker.Title, worker.Telphone
                                            , model.distribution_time != null ? ((DateTime)model.distribution_time).ToString("HH:mm") : ""
                                            , upamount, uptotal, downamount, downtotal);
                                    }

                                }
                                break;
                        }
                    }
                    
                }
                else
                {
                    _title = "";
                }
            }
            else
            {
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
                            BookingFood.Model.bf_worker worker = new BookingFood.BLL.bf_worker().GetModel(model.worker_id);
                            if (worker != null)
                            {
                                _title = string.Format("{0} <span style=\"color:red;\">{2}</span>", worker.Title, worker.Telphone
                                    , model.distribution_time != null ? ((DateTime)model.distribution_time).ToString("HH:mm") : ""
                                    , upamount, uptotal, downamount, downtotal);
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
            }
            

            return _title;
        }
        protected string GetShortOrderStatus(int _id, string upamount, string uptotal, string downamount, string downtotal)
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
                        BookingFood.Model.bf_worker worker = new BookingFood.BLL.bf_worker().GetModel(model.worker_id);
                        if (worker != null)
                        {
                            _title = string.Format("<div style=\"display:inline;\">{1}<br/><span style=\"color:green;\">{3}({4}){5}({6})</span></div>", worker.Title, worker.Telphone
                                , model.distribution_time != null ? ((DateTime)model.distribution_time).ToString("HH:mm") : ""
                                , upamount, uptotal, downamount, downtotal);
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
            Response.Redirect("order_list.aspx?type="+type);
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
            if (model.OrderType != "线下订单")
            {
                rtn += "-----------------------------------------------<br />";
            }
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
                                rtn += string.Format("<span style=\"font-size:12px;\">*{0} {1}</span> "
                                    , sub.Split('‡')[2], (sub.Split('‡').Length == 4 || sub.Split('‡').Length == 5) && !string.IsNullOrEmpty(sub.Split('‡')[3]) ? "/" + sub.Split('‡')[3] : "");
                            }
                        }
                        rtn += "<br/>";
                    }                    
                }
                else if (item.type == "one" || item.type == "full" || item.type == "discount")
                {
                    rtn += string.Format("{0} {3}{4}{1}份&nbsp;&nbsp;&nbsp;&nbsp;{2}元<br/>"
                        , item.goods_name, item.quantity, item.quantity * item.real_price
                        , !string.IsNullOrEmpty(item.subgoodsid) ? "*"+item.subgoodsid.Split('‡')[2] : ""
                        , !string.IsNullOrEmpty(item.condition_price) ? "*" + item.condition_price : ""
                        );                                     
                }
                
	        }            
            return rtn;
        }

        protected void rptList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            ChkAdminLevel("orders", DTEnums.ActionEnum.Edit.ToString()); //检查权限
            int id = Convert.ToInt32(e.CommandArgument.ToString());
            BLL.orders bll = new BLL.orders();
            Model.orders model = bll.GetModel(id);
            switch (e.CommandName.ToLower())
            {
                case "delete":
                    bll.UpdateField(id, "status=5,restore_status="+model.status.ToString());
                    break;
            }
            RptBind("id>0" + CombSqlTxt(this.keyword,this.area), "add_time desc,id desc");
        }

        protected string GetPayment(string payment_status, string payment_id, string payment_time, string order_no)
        {
            string rtn = string.Empty;
            string gou = "";
            if(payment_status=="1")
            {
                if(payment_id=="3" || payment_id == "5" || payment_id == "6"|| payment_id == "9")
                {
                    rtn = "<span style=\"color:red;cursor:pointer;\" onclick=\"QueryPayStatus('" + order_no + "')\">＝</span>";
                }
                else
                {
                    rtn = "-";
                }
            }
            else if(payment_status=="2")
            {
                if(payment_id=="7")
                {
                    gou = "√√";
                }
                else if(payment_id=="8")
                {
                    gou = "√√√";
                }
                else if (payment_id == "9")
                {
                    gou = "卡√";
                }
                else
                {
                    gou = "√";
                }
                if(!string.IsNullOrEmpty(payment_time))
                {
                    rtn = "<span style=\"color:red;\">" + gou + DateTime.Parse(payment_time).ToString("HH:mm") + "</span>";
                }
                else
                {
                    rtn = "<span style=\"color:red;\">"+ gou +"</span>";
                }
            }
            else if(payment_status=="11")
            {
                rtn = "<span style=\"color:green;cursor:pointer;\" onclick=\"ConfirmSyncStatus('" + order_no + "')\">yes</span>";
            }
            else if (payment_status == "12")
            {
                rtn = "<span style=\"color:green;cursor:pointer;\" onclick=\"ConfirmSyncStatus('" + order_no + "')\">yes</span>";
            }
            return rtn;
        }

        protected string GetAddress(string email,string address)
        {
            if(string.Equals(email, "register@meituan.com") || string.Equals(email, "register@baidu.com") || string.Equals(email, "register@eleme.com"))
            {
                return "<font style=\"color:green;\">"+address+"</font>";
            }
            else
            {
                return address;
            }
        }
    }
}