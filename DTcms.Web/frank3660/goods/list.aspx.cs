using System;
using System.Text;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DTcms.Common;
using System.Linq;

namespace DTcms.Web.admin.goods
{
    public partial class list : Web.UI.ManagePage
    {
        protected int channel_id;
        protected int area_id;
        protected string is_lock;
        protected int totalCount;
        protected int page;
        protected int pageSize;

        protected int category_id;
        protected string property = string.Empty;
        protected string keywords = string.Empty;

        private int firstCategoryId = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.channel_id = DTRequest.GetQueryInt("channel_id");
            this.category_id = DTRequest.GetQueryInt("category_id");
            this.area_id = DTRequest.GetQueryInt("area_id");
            this.is_lock = DTRequest.GetQueryString("is_lock");
            this.keywords = DTRequest.GetQueryString("keywords");
            this.property = DTRequest.GetQueryString("property");

            if (channel_id == 0)
            {
                JscriptMsg("频道参数不正确！", "back", "Error");
                return;
            }
            this.pageSize = GetPageSize(7); //每页数量
            if (!Page.IsPostBack)
            {
                ChkAdminLevel("caipinguanli", DTEnums.ActionEnum.View.ToString()); //检查权限
                DoInitCategory();
                DoInitArea();
                if (this.category_id == 0 && firstCategoryId > 0)
                {
                    this.category_id = firstCategoryId;
                }
                RptBind("id>0" + CombSqlTxt(this.channel_id, this.category_id,this.is_lock, this.keywords, this.property), "sort_id asc,add_time desc");
            }
        } 
        

        #region 数据绑定=================================
        private void RptBind(string _strWhere, string _orderby)
        {
            this.page = DTRequest.GetQueryInt("page", 1);
            //图表或列表显示
            BLL.article bll = new BLL.article();
            this.rptList1.DataSource = bll.GetGoodsListUnionCombo(this.pageSize, this.page, _strWhere, _orderby, out this.totalCount);
            this.rptList1.DataBind();
            //绑定页码
            txtPageNum.Text = this.pageSize.ToString();
            string pageUrl = Utils.CombUrlTxt("list.aspx", "channel_id={0}&category_id={1}&keywords={2}&property={3}&page={4}",
                this.channel_id.ToString(), this.category_id.ToString(), this.keywords, this.property, "__id__");
            PageContent.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
        }
        #endregion

        #region 组合SQL查询语句==========================
        protected string CombSqlTxt(int _channel_id, int _category_id,string is_lock, string _keywords, string _property)
        {
            StringBuilder strTemp = new StringBuilder();
            if (_channel_id > 0)
            {
                strTemp.Append(" and channel_id!=1");
            }
            if (_category_id > 0)
            {
                strTemp.Append(" and category_id in(select id from dt_category where id=" + _category_id + " and class_list like '%," + _category_id + ",%')");
            }            
            if (area_id > 0 && !string.IsNullOrEmpty(Session["AreaId"].ToString()))
            {
                strTemp.Append(" and id in(SELECT ArticleId FROM bf_area_article WHERE AreaId=" + area_id + " and AreaId in (SELECT ba.Id FROM bf_area ba WHERE ba.ParentId=" + Session["AreaId"].ToString() + "))");
                strTemp.Append(" and id in(SELECT baa.ArticleId FROM bf_area_article baa,bf_area ba WHERE baa.AreaId=ba.Id AND baa.AreaId=" + area_id + " AND ba.ParentId=" + Session["AreaId"].ToString() + ")");
            }
            //if (!string.IsNullOrEmpty(Session["AreaId"].ToString()))
            //{
            //    strTemp.Append(" and id in(SELECT baa.ArticleId FROM bf_area_article baa,bf_area ba WHERE baa.AreaId=ba.Id AND ba.ParentId=" + Session["AreaId"].ToString() + ")");
            //}
            if (!string.IsNullOrEmpty(is_lock))
            {
                strTemp.Append(" and is_lock=" + is_lock);
            }
            _keywords = _keywords.Replace("'", "");
            if (!string.IsNullOrEmpty(_keywords))
            {
                strTemp.Append(" and title like '%" + _keywords + "%'");
            }
            if (!string.IsNullOrEmpty(_property))
            {
                switch (_property)
                {
                    case "isLock":
                        strTemp.Append(" and is_lock=1");
                        break;
                    case "unIsLock":
                        strTemp.Append(" and is_lock=0");
                        break;
                    case "isMsg":
                        strTemp.Append(" and is_msg=1");
                        break;
                    case "isTop":
                        strTemp.Append(" and is_top=1");
                        break;
                    case "isRed":
                        strTemp.Append(" and is_red=1");
                        break;
                    case "isHot":
                        strTemp.Append(" and is_hot=1");
                        break;
                    case "isSlide":
                        strTemp.Append(" and is_slide=1");
                        break;
                }
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

        //设置操作
        protected void rptList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            ChkAdminLevel("caipinguanli", DTEnums.ActionEnum.Edit.ToString()); //检查权限
            int id = Convert.ToInt32(((HiddenField)e.Item.FindControl("hidId")).Value);
            string arg = e.CommandArgument.ToString();
            if (arg == "one")
            {
                BLL.article bll = new BLL.article();
                Model.article_goods model = bll.GetGoodsModel(id);
                switch (e.CommandName.ToLower())
                {
                    case "lock":
                        if (model.is_lock == 1)
                            bll.UpdateField(id, "is_lock=0");
                        else
                            bll.UpdateField(id, "is_lock=1");
                        break;
                    case "delete":
                        bll.Delete(id);
                        break;
                }
            }
            else
            {
                BookingFood.BLL.bf_good_combo bll = new BookingFood.BLL.bf_good_combo();
                BookingFood.Model.bf_good_combo model = bll.GetModel(id);
                switch (e.CommandName.ToLower())
                {
                    case "lock":
                        if (model.Is_Lock == 1)
                        {
                            model.Is_Lock = 0;
                        }

                        else
                        {
                            model.Is_Lock = 1;
                        }
                        bll.Update(model);
                        break;
                    case "delete":
                        bll.Delete(id);
                        break;
                }
            }
            this.RptBind("id>0" + CombSqlTxt(this.channel_id, this.category_id,this.is_lock, this.keywords, this.property), "sort_id asc,add_time desc");
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
            Response.Redirect(Utils.CombUrlTxt("list.aspx", "channel_id={0}&category_id={1}&keywords={2}&property={3}",
                this.channel_id.ToString(), this.category_id.ToString(), this.keywords, this.property));
        }

        //批量删除
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            ChkAdminLevel("caipinguanli", DTEnums.ActionEnum.Delete.ToString()); //检查权限
            BLL.article bll = new BLL.article();
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
            JscriptMsg("批量删除成功啦！", Utils.CombUrlTxt("list.aspx", "channel_id={0}&category_id={1}&keywords={2}&property={3}",
                this.channel_id.ToString(), this.category_id.ToString(), this.keywords, this.property), "Success");
        }

        //设置套餐显示
        protected void DoInitCategory()
        {
            string rtn = "";
            DataTable dt = new BLL.category().GetChildList(0, 2);
            foreach (DataRow item in dt.Rows)
            {
                if (firstCategoryId == 0)
                {
                    firstCategoryId = int.Parse(item["id"].ToString());
                }
                if (category_id.ToString() == item["id"].ToString())
                {
                    rtn += string.Format("<li class=\"reverse\" onclick=\"location.href='list.aspx?channel_id=2&category_id={1}'\">{0}</li>"
                        , item["title"].ToString(),item["id"].ToString());
                }
                else
                {
                    rtn += string.Format("<li onclick=\"location.href='list.aspx?channel_id=2&category_id={1}'\">{0}</li>"
                        , item["title"].ToString(), item["id"].ToString());
                }
            }
            dt = new BLL.category().GetChildList(0, 3);
            foreach (DataRow item in dt.Rows)
            {
                if (category_id.ToString() == item["id"].ToString())
                {
                    rtn += string.Format("<li class=\"reverse\" onclick=\"location.href='list.aspx?channel_id=2&category_id={1}'\">{0}</li>"
                        , item["title"].ToString(), item["id"].ToString());
                }
                else
                {
                    rtn += string.Format("<li class=\"green\" onclick=\"location.href='list.aspx?channel_id=2&category_id={1}'\">{0}</li>"
                        , item["title"].ToString(), item["id"].ToString());
                }
            }
            ltlCategory.Text = rtn;
        }

        //设置区域显示
        protected void DoInitArea()
        {
            string rtn = "";
            string areaid = Session["areaid"].ToString();
            DataTable dt = new BookingFood.BLL.bf_area().GetList(" IsShow=1 AND ParentId=" + areaid + " Order By SortId").Tables[0];
            foreach (DataRow item in dt.Rows)
            {
                if (area_id.ToString() == item["id"].ToString())
                {
                    rtn += string.Format("<span class=\"gray\" onclick=\"location.href='list.aspx?channel_id=2&area_id={1}'\">{0}</span>"
                        , item["Title"].ToString(), item["Id"].ToString());
                }
                else
                {
                    rtn += string.Format("<span onclick=\"location.href='list.aspx?channel_id=2&area_id={1}'\">{0}</span>"
                        , item["Title"].ToString(), item["Id"].ToString());
                }
            }            
            ltlArea.Text = rtn;
        }

        protected string GetAreaByArtecleId(string articleid,string type)
        {
            if (articleid != "")
            {
                type = string.Equals("combo", type) ? "category" : type;
                string parentareaid = Session["AreaId"].ToString();
                List<BookingFood.Model.bf_area> listAll =
                    new BookingFood.BLL.bf_area().GetModelList
                    (" ParentId=" + parentareaid + " And Id In (select AreaId from bf_area_article where ArticleId=" + articleid + " and Type='" + type + "')");
                BookingFood.BLL.bf_area bll = new BookingFood.BLL.bf_area();
                if (type == "combo") type = "category";
                List<BookingFood.Model.bf_area_article> list = new BookingFood.BLL.bf_area_article().GetModelList(" ArticleId=" + articleid + " and Type='"+type+"'" );                
                string rtn = string.Empty;
                BookingFood.Model.bf_area_article temp = null;
                foreach (var item in listAll)
                {
                    temp = list.FirstOrDefault(s => s.AreaId == item.Id);
                    if (temp == null) continue;
                    if(temp.IsLock==0)
                    {
                        if(temp.GuQingDate!=null && ((DateTime)temp.GuQingDate).Date==DateTime.Now.Date)
                        {
                            rtn += string.Format("<span name=\"articlearea\" data-areaid=\"{1}\" data-articleid=\"{2}\" data-type=\"{3}\" style=\"cursor:pointer;\">{0}"
                                + "<font style=\"color: red;\">(估清)</font></span>"
                            , item.Title, item.Id, articleid, type);
                        }
                        else
                        {
                            rtn += string.Format("<span name=\"articlearea\" data-areaid=\"{1}\" data-articleid=\"{2}\" data-type=\"{3}\" style=\"cursor:pointer;\">{0}</span>"
                            , item.Title, item.Id, articleid, type);
                        }
                    }
                    else
                    {
                        rtn += string.Format("<span name=\"articlearea\" data-areaid=\"{1}\" data-articleid=\"{2}\" data-type=\"{3}\" style=\"color:black;cursor:pointer;\">{0}</span>"
                            , item.Title, item.Id, articleid, type);
                    }
                    
                    //else
                    //{
                    //    rtn += string.Format("<span name=\"articlearea\" data-areaid=\"{1}\" data-articleid=\"{2}\" data-type=\"{3}\" style=\"color:black;cursor:pointer;\">{0}</span>"
                    //        , item.Title, item.Id, articleid, type);
                    //}
                }
                return rtn;
            }
            return "";
        }

        protected string GetTaste(string taste)
        {
            string rtn = string.Empty;
            string[] tastes = taste.Split(',');
            foreach (var item in tastes)
            {
                rtn += string.Format("<span>{0}</span>",item);
            }
            return rtn;
        }

        protected string GetComboDetail(string articleid)
        {
            if (string.IsNullOrEmpty(articleid)) return "";
            List<BookingFood.Model.bf_good_combo_detail> list = 
                new BookingFood.BLL.bf_good_combo_detail().GetModelList(" GoodComboId=" + articleid + " Order By SortId Asc");
            string rtn = string.Empty;
            foreach (var item in list)
            {
                rtn += string.Format("<span>{0}</span>", item.BUsinessTitle);
            }
            return rtn;
        }
    }
}