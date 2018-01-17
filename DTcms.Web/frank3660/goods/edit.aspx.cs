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
    public partial class edit : Web.UI.ManagePage
    {
        private string action = DTEnums.ActionEnum.Add.ToString(); //操作类型
        private int channel_id;
        private int id = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            
            string _action = DTRequest.GetQueryString("action");
            this.channel_id = DTRequest.GetQueryInt("channel_id");
            
            if (this.channel_id == 0)
            {
                JscriptMsg("频道参数不正确！", "back", "Error");
                return;
            }
            if (!string.IsNullOrEmpty(_action) && _action == DTEnums.ActionEnum.Edit.ToString())
            {
                this.action = DTEnums.ActionEnum.Edit.ToString();//修改类型
                this.id = DTRequest.GetQueryInt("id");
                if (this.id == 0)
                {
                    JscriptMsg("传输参数不正确！", "back", "Error");
                    return;
                }
                if (!new BLL.article().Exists(this.id))
                {
                    JscriptMsg("信息不存在或已被删除！", "back", "Error");
                    return;
                }
            }
            if (!Page.IsPostBack)
            {
                hfUrlReferrer.Value = this.Request.UrlReferrer.ToString();
                TreeBind(this.channel_id); //绑定类别
                List<BookingFood.Model.bf_good_nickname> listNickName = new BookingFood.BLL.bf_good_nickname().GetModelList("");
                foreach (var item in listNickName)
                {
                    ddlNickName.Items.Add(new ListItem(item.Title, item.Id.ToString()));
                }
                GroupBind(""); //绑定用户组
                string areaid = Session["areaid"].ToString();
                List<BookingFood.Model.bf_area> listArea = new BookingFood.BLL.bf_area().GetModelList(" ParentId=" + areaid);
                foreach (var item in listArea)
                {
                    cblArea.Items.Add(new ListItem(item.Title, item.Id.ToString()));
                }
                List<BookingFood.Model.bf_condition> listCondition = new BookingFood.BLL.bf_condition().GetModelList(" 1=1 Order By SortId Asc");
                foreach (var item in listCondition)
                {
                    cblTaste.Items.Add(new ListItem(item.Title, item.Title));
                }
                List<BookingFood.Model.bf_condition_price> listConditionPrice = new BookingFood.BLL.bf_condition_price().GetModelList(" 1=1 Order By SortId Asc");
                foreach (var item in listConditionPrice)
                {
                    cblConditionPrice.Items.Add(new ListItem(item.Title + ":" + item.Price + "元", item.Title + "†"+item.Price.ToString()));
                }
                if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
                {
                    ShowInfo(this.id);
                }
                else
                {
                    LitAttributeList.Text = GetAttributeHtml(null, this.channel_id, this.id);
                }
            }
        }

        #region 赋值操作=================================
        private void ShowInfo(int _id)
        {
            BLL.article bll = new BLL.article();
            Model.article_goods model = bll.GetGoodsModel(_id);

            ddlCategoryId.SelectedValue = model.category_id.ToString();
            ddlOppositionCategoryId.SelectedValue = model.opposition_id.ToString();
            cboOnlyCompany.Checked = model.only_company == 1 ? true : false;
            cboHeadLine.Checked = model.head_line == 1 ? true : false;
            txtTitle.Text = model.title;
            txtGoodsNo.Text = model.goods_no;
            ddlIsLock.SelectedValue = model.is_lock.ToString();
            ddlNickName.SelectedValue = model.nick_id.ToString();
            txtMark.Text = model.mark;
            txtHeadLineImgUrl.Text = model.headline_img_url;
            if (!string.IsNullOrEmpty(model.taste))
            {
                string[] tastes = model.taste.Split(',');
                ListItem selItem = null;
                foreach (var item in tastes)
                {
                    selItem = cblTaste.Items.FindByValue(item);
                    if(selItem!=null)
                    {
                        selItem.Selected = true;
                    }
                }
            }
            if (!string.IsNullOrEmpty(model.condition_price))
            {
                string[] tastes = model.condition_price.Split(',');
                ListItem selItem = null;
                foreach (var item in tastes)
                {
                    selItem = cblConditionPrice.Items.FindByValue(item);
                    if (selItem != null)
                    {
                        selItem.Selected = true;
                    }
                }
            }
            List<BookingFood.Model.bf_area_article> listAreaArticle = 
                new BookingFood.BLL.bf_area_article().GetModelList(" Type='one' and ArticleId=" + _id.ToString());
            foreach (var item in listAreaArticle)
            {

                if (cblArea.Items.FindByValue(item.AreaId.ToString()) != null )
                {
                    cblArea.Items.FindByValue(item.AreaId.ToString()).Selected = true;
                }
            }
            txtStockQuantity.Text = model.stock_quantity.ToString();
            txtMarketPrice.Text = model.market_price.ToString();
            txtSellPrice.Text = model.sell_price.ToString();
            txtZhenghuaPrice.Text = model.zhenghua_price.ToString();
            txtTaobaoPrice.Text = model.taobao_price.ToString();
            txtPoint.Text = model.point.ToString();
            txtLinkUrl.Text = model.link_url;
            if (model.is_msg == 1)
            {
                cblItem.Items[0].Selected = true;
            }
            if (model.is_top == 1)
            {
                cblItem.Items[1].Selected = true;
            }
            if (model.is_red == 1)
            {
                cblItem.Items[2].Selected = true;
            }
            if (model.is_hot == 1)
            {
                cblItem.Items[3].Selected = true;
            }
            if (model.is_slide == 1)
            {
                cblItem.Items[4].Selected = true;
            }
            if (model.is_lock == 1)
            {
                cblItem.Items[5].Selected = true;
            }
            txtSortId.Text = model.sort_id.ToString();
            txtClick.Text = model.click.ToString();
            txtDiggGood.Text = model.digg_good.ToString();
            txtDiggBad.Text = model.digg_bad.ToString();
            txtContent.Value = model.content;
            txtSeoTitle.Text = model.seo_title;
            txtSeoKeywords.Text = model.seo_keywords;
            txtSeoDescription.Text = model.seo_description;
            //赋值用户组价格
            if (model.goods_group_prices != null)
            {
                for (int i = 0; i < this.rptPrice.Items.Count; i++)
                {
                    int hideId = Convert.ToInt32(((HiddenField)this.rptPrice.Items[i].FindControl("hideGroupId")).Value);
                    foreach (Model.goods_group_price modelt in model.goods_group_prices)
                    {
                        if (hideId == modelt.group_id)
                        {
                            ((HiddenField)this.rptPrice.Items[i].FindControl("hidePriceId")).Value = modelt.id.ToString();
                            ((TextBox)this.rptPrice.Items[i].FindControl("txtGroupPrice")).Text = modelt.price.ToString();
                        }
                    }
                }
            }
            //赋值上传的相册
            focus_photo.Value = model.img_url; //封面图片
            LitAlbumList.Text = GetAlbumHtml(model.albums, model.img_url);
            //赋值属性列表
            LitAttributeList.Text = GetAttributeHtml(model.attribute_values, this.channel_id, _id);
            txtLowNum.Text = model.low_num.ToString();
            txtMpImgUrl.Text = model.mp_img_url;
            txtChangeNums.Text = model.change_nums.ToString();

            BookingFood.BLL.bf_article_low_num bllLowNum = new BookingFood.BLL.bf_article_low_num();
            List<BookingFood.Model.bf_article_low_num> listLowNum = bllLowNum.GetModelList("ArticleId=" + _id);
            for (int i = 0; i < listLowNum.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        txtBeginTime1.Text = listLowNum[i].BeginTime.ToString("HH:mm");
                        txtEndTime1.Text = listLowNum[i].EndTime.ToString("HH:mm"); ;
                        txtLowNum1.Text = listLowNum[i].LowNum.ToString();
                        break;
                    case 1:
                        txtBeginTime2.Text = listLowNum[i].BeginTime.ToString("HH:mm"); ;
                        txtEndTime2.Text = listLowNum[i].EndTime.ToString("HH:mm"); ;
                        txtLowNum2.Text = listLowNum[i].LowNum.ToString();
                        break;
                    case 2:
                        txtBeginTime3.Text = listLowNum[i].BeginTime.ToString("HH:mm"); ;
                        txtEndTime3.Text = listLowNum[i].EndTime.ToString("HH:mm"); ;
                        txtLowNum3.Text = listLowNum[i].LowNum.ToString();
                        break;
                }
            }
        }
        #endregion

        #region 绑定类别=================================
        private void TreeBind(int _channel_id)
        {
            BLL.category bll = new BLL.category();
            DataTable dt = bll.GetList(0, _channel_id);

            this.ddlCategoryId.Items.Clear();
            this.ddlOppositionCategoryId.Items.Clear();
            this.ddlCategoryId.Items.Add(new ListItem("请选择类别...", ""));
            this.ddlOppositionCategoryId.Items.Add(new ListItem("请选择类别...", "0"));
            foreach (DataRow dr in dt.Rows)
            {
                string Id = dr["id"].ToString();
                int ClassLayer = int.Parse(dr["class_layer"].ToString());
                string Title = dr["title"].ToString().Trim();

                if (ClassLayer == 1)
                {
                    this.ddlCategoryId.Items.Add(new ListItem(Title, Id));
                    this.ddlOppositionCategoryId.Items.Add(new ListItem(Title, Id));
                }
                else
                {
                    Title = "├ " + Title;
                    Title = Utils.StringOfChar(ClassLayer - 1, "　") + Title;
                    this.ddlCategoryId.Items.Add(new ListItem(Title, Id));
                    this.ddlOppositionCategoryId.Items.Add(new ListItem(Title, Id));
                }
            }
            dt = bll.GetList(0, 3);
            foreach (DataRow dr in dt.Rows)
            {
                string Id = dr["id"].ToString();
                int ClassLayer = int.Parse(dr["class_layer"].ToString());
                string Title = dr["title"].ToString().Trim();

                if (ClassLayer == 1)
                {
                    this.ddlCategoryId.Items.Add(new ListItem(Title, Id));
                    this.ddlOppositionCategoryId.Items.Add(new ListItem(Title, Id));
                }
                else
                {
                    Title = "├ " + Title;
                    Title = Utils.StringOfChar(ClassLayer - 1, "　") + Title;
                    this.ddlCategoryId.Items.Add(new ListItem(Title, Id));
                    this.ddlOppositionCategoryId.Items.Add(new ListItem(Title, Id));
                }
            }
        }
        #endregion

        #region 绑定会员组===============================
        private void GroupBind(string strWhere)
        {
            BLL.user_groups bll = new BLL.user_groups();
            DataSet ds = bll.GetList(0, strWhere, "grade asc,id desc");
            if (ds.Tables[0].Rows.Count > 0)
            {
                this.rptPrice.DataSource = ds;
                this.rptPrice.DataBind();
            }
        }
        #endregion

        #region 返回相册列表HMTL=========================
        private string GetAlbumHtml(List<Model.article_albums> models, string focus_photo)
        {
            StringBuilder strTxt = new StringBuilder();
            if (models != null)
            {
                foreach (Model.article_albums modelt in models)
                {
                    strTxt.Append("<li>\n");
                    strTxt.Append("<input type=\"hidden\" name=\"hide_photo_name\" value=\"" + modelt.id + "|" + modelt.big_img + "|" + modelt.small_img + "\" />\n");
                    strTxt.Append("<input type=\"hidden\" name=\"hide_photo_remark\" value=\"" + modelt.remark + "\" />\n");
                    strTxt.Append("<div onclick=\"focus_img(this);\" class=\"img_box");
                    if (focus_photo == modelt.small_img)
                    {
                        strTxt.Append(" current");
                    }
                    strTxt.Append("\">\n");
                    strTxt.Append("<img bigsrc=\"" + modelt.big_img + "\" src=\"" + modelt.small_img + "\" />");
                    strTxt.Append("<span class=\"remark\"><i>");
                    if (!string.IsNullOrEmpty(modelt.remark))
                    {
                        strTxt.Append(modelt.remark);
                    }
                    else
                    {
                        strTxt.Append("暂无描述...");
                    }
                    strTxt.Append("</i></span></div>\n");
                    strTxt.Append("<a onclick=\"show_remark(this);\" href=\"javascript:;\">描述</a><a onclick=\"del_img(this);\" href=\"javascript:;\">删除</a>\n");
                    strTxt.Append("</li>\n");
                }
            }
            return strTxt.ToString();
        }
        #endregion

        #region 返回属性列表HMTL=========================
        private string GetAttributeHtml(List<Model.attribute_value> models, int _channel_id, int _article_id)
        {
            StringBuilder strTxt = new StringBuilder();
            BLL.attributes bll = new BLL.attributes();
            DataSet ds = bll.GetList("channel_id=" + _channel_id);

            if (ds.Tables[0].Rows.Count > 0)
            {
                strTxt.Append("<tr><th>扩展属性：</th><td>\n");
                strTxt.Append("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"border_table\">\n");
                strTxt.Append(" <tbody><col width=\"80px\"><col>\n");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int _value_id = 0;
                    string _value_content = "";
                    if (models != null)
                    {
                        foreach (Model.attribute_value modelt in models)
                        {
                            if (modelt.attribute_id == Convert.ToInt32(dr["id"]) && modelt.article_id == _article_id)
                            {
                                _value_id = modelt.id;
                                _value_content = modelt.content;
                            }
                        }
                    }
                    strTxt.Append("<tr><th>" + dr["title"] + "</th><td>\n");
                    strTxt.Append(GetAttributeType(Convert.ToInt32(dr["id"]), dr["title"].ToString(), dr["default_value"].ToString(), Convert.ToInt32(dr["type"]),
                        _value_id, _value_content));
                    strTxt.Append("</td></tr>\n");
                }
                strTxt.Append("</tbody>\n");
                strTxt.Append("</table>\n");
                strTxt.Append("</td></tr>\n");
            }
            return strTxt.ToString();
        }
        #endregion

        #region 返回属性类型=============================
        /// <summary>
        /// 返回属性类型HTML
        /// </summary>
        /// <param name="_id">属性ID</param>
        /// <param name="_title">属性标题</param>
        /// <param name="_default_value">属性默认值</param>
        /// <param name="_type">属性类型</param>
        /// <param name="_value_id">属性值ID</param>
        /// <param name="_value">属性值内容</param>
        /// <returns>HTML代码</returns>
        private string GetAttributeType(int _id, string _title, string _default_value, int _type, int _value_id, string _value)
        {
            //分解默认值
            string[] valueArr = _default_value.Split(',');
            StringBuilder str = new StringBuilder();
            str.Append("<input type=\"hidden\" name=\"value_" + _id + "\" value=\"" + _value_id + "\"/>\n");
            switch (_type)
            {
                case (int)DTEnums.AttributeEnum.Text:
                    if (_value_id > 0)
                        _default_value = _value;
                    str.Append("<input type=\"text\" name=\"content_" + _id + "\" value=\"" + _default_value + "\" class=\"txtInput middle\" />\n");
                    break;
                case (int)DTEnums.AttributeEnum.Select:
                    str.Append("<select name=\"content_" + _id + "\" class=\"select2\">\n");
                    for (int i = 0; i < valueArr.Length; i++)
                    {
                        str.Append("<option value=\"" + valueArr[i] + "\"");
                        if (_value_id > 0 && _value == valueArr[i])
                            str.Append(" selected");
                        str.Append(">" + valueArr[i] + "</option>\n");
                    }
                    str.Append("</select>\n");
                    break;
                case (int)DTEnums.AttributeEnum.Radio:
                    for (int i = 0; i < valueArr.Length; i++)
                    {
                        str.Append("<label class=\"attr\"><input type=\"radio\" name=\"content_" + _id + "\" value=\"" + valueArr[i] + "\"");
                        if (_value_id > 0 && _value == valueArr[i])
                            str.Append(" checked");
                        str.Append("  />" + valueArr[i] + "</label>\n");
                    }
                    break;
                case (int)DTEnums.AttributeEnum.CheckBox:
                    for (int i = 0; i < valueArr.Length; i++)
                    {
                        str.Append("<label class=\"attr\"><input type=\"checkbox\" name=\"content_" + _id + "\" value=\"" + valueArr[i] + "\"");
                        if (_value_id > 0 && !string.IsNullOrEmpty(_value))
                        {
                            string[] _valueArr = _value.Split(',');
                            for (int j = 0; j < _valueArr.Length; j++)
                            {
                                if (valueArr[i] == _valueArr[j])
                                    str.Append(" checked");
                            }
                        }
                        str.Append(" />" + valueArr[i] + "</label>\n");
                    }
                    break;
            }
            return str.ToString();
        }
        #endregion

        #region 增加操作=================================
        private bool DoAdd()
        {
            bool result = true;
            Model.article_goods model = new Model.article_goods();
            BLL.article bll = new BLL.article();

            model.channel_id = new BLL.category().GetModel(int.Parse(ddlCategoryId.SelectedValue)).channel_id;
            model.title = txtTitle.Text.Trim();
            model.category_id = int.Parse(ddlCategoryId.SelectedValue);
            model.opposition_id = int.Parse(ddlOppositionCategoryId.SelectedValue);
            model.nick_id = int.Parse(ddlNickName.SelectedValue);
            model.is_lock = int.Parse(ddlIsLock.SelectedValue);
            model.mark = txtMark.Text.Trim();
            model.only_company = cboOnlyCompany.Checked ? 1 : 0;
            model.head_line = cboHeadLine.Checked ? 1 : 0;
            model.headline_img_url = txtHeadLineImgUrl.Text.Trim();
            string taste = string.Empty;
            foreach (ListItem item in cblTaste.Items)
            {
                if (item.Selected)
                {
                    if (!string.IsNullOrEmpty(taste))
                    {
                        taste += ",";
                    }
                    taste += item.Value;
                }
            }
            model.taste = taste;

            string conditionPrice = string.Empty;
            foreach (ListItem item in cblConditionPrice.Items)
            {
                if (item.Selected)
                {
                    if (!string.IsNullOrEmpty(conditionPrice))
                    {
                        conditionPrice += ",";
                    }
                    conditionPrice += item.Value;
                }
            }
            model.condition_price = conditionPrice;
            
            model.goods_no = txtGoodsNo.Text;
            model.stock_quantity = int.Parse(txtStockQuantity.Text);
            model.market_price = decimal.Parse(txtMarketPrice.Text);
            model.sell_price = decimal.Parse(txtSellPrice.Text);
            model.zhenghua_price = decimal.Parse(string.IsNullOrEmpty(txtSellPrice.Text)?"0":txtZhenghuaPrice.Text);
            model.taobao_price = decimal.Parse(string.IsNullOrEmpty(txtTaobaoPrice.Text) ? "0" : txtTaobaoPrice.Text);
            model.point = int.Parse(txtPoint.Text);
            model.link_url = txtLinkUrl.Text.Trim();
            model.img_url = focus_photo.Value;
            model.content = txtContent.Value;
            model.seo_title = txtSeoTitle.Text.Trim();
            model.seo_keywords = txtSeoKeywords.Text.Trim();
            model.seo_description = txtSeoDescription.Text.Trim();
            model.sort_id = int.Parse(txtSortId.Text.Trim());
            model.click = int.Parse(txtClick.Text.Trim());
            model.digg_good = int.Parse(txtDiggGood.Text.Trim());
            model.digg_bad = int.Parse(txtDiggBad.Text.Trim());
            model.is_msg = 0;
            model.is_top = 0;
            model.is_red = 0;
            model.is_hot = 0;
            model.is_slide = 0;
            if (cblItem.Items[0].Selected == true)
            {
                model.is_msg = 1;
            }
            if (cblItem.Items[1].Selected == true)
            {
                model.is_top = 1;
            }
            if (cblItem.Items[2].Selected == true)
            {
                model.is_red = 1;
            }
            if (cblItem.Items[3].Selected == true)
            {
                model.is_hot = 1;
            }
            if (cblItem.Items[4].Selected == true)
            {
                model.is_slide = 1;
            }
            if (cblItem.Items[5].Selected == true)
            {
                
            }
            //会员组价格
            List<Model.goods_group_price> priceList = new List<Model.goods_group_price>();
            for (int i = 0; i < rptPrice.Items.Count; i++)
            {
                int _groupid = Convert.ToInt32(((HiddenField)rptPrice.Items[i].FindControl("hideGroupId")).Value);
                decimal _price = Convert.ToDecimal(((TextBox)rptPrice.Items[i].FindControl("txtGroupPrice")).Text.Trim());
                priceList.Add(new Model.goods_group_price { group_id = _groupid, price = _price });
            }
            model.goods_group_prices = priceList;
            //保存相册
            string[] albumArr = Request.Form.GetValues("hide_photo_name");
            string[] remarkArr = Request.Form.GetValues("hide_photo_remark");
            if (albumArr != null && albumArr.Length > 0)
            {
                List<Model.article_albums> ls = new List<Model.article_albums>();
                for (int i = 0; i < albumArr.Length; i++)
                {
                    string[] imgArr = albumArr[i].Split('|');
                    if (imgArr.Length == 3)
                    {
                        if (!string.IsNullOrEmpty(remarkArr[i]))
                        {
                            ls.Add(new Model.article_albums { big_img = imgArr[1], small_img = imgArr[2], remark = remarkArr[i] });
                        }
                        else
                        {
                            ls.Add(new Model.article_albums { big_img = imgArr[1], small_img = imgArr[2] });
                        }
                    }
                }
                model.albums = ls;
            }

            //扩展属性
            BLL.attributes bll2 = new BLL.attributes();
            DataSet ds2 = bll2.GetList("channel_id=" + this.channel_id);

            List<Model.attribute_value> attrls = new List<Model.attribute_value>();
            foreach (DataRow dr in ds2.Tables[0].Rows)
            {
                int attr_id = int.Parse(dr["id"].ToString());
                string attr_title = dr["title"].ToString();
                string attr_value = Request.Form["content_" + attr_id];
                if (!string.IsNullOrEmpty(attr_value))
                {
                    attrls.Add(new Model.attribute_value { attribute_id = attr_id, title = attr_title, content = attr_value });
                }
            }
            model.attribute_values = attrls;
            model.low_num = int.Parse(txtLowNum.Text.Trim());
            model.mp_img_url = txtMpImgUrl.Text.Trim();
            model.change_nums = int.Parse(txtChangeNums.Text.Trim());
            int ret = bll.Add(model);
            BookingFood.BLL.bf_area_article bllAreaArticle = new BookingFood.BLL.bf_area_article();            
            foreach (ListItem item in cblArea.Items)
	        {
                if (item.Selected)
                {
                    bllAreaArticle.Add(new BookingFood.Model.bf_area_article()
                        {
                            AreaId = int.Parse(item.Value),
                            ArticleId = ret,
                            Type = "one",
                            IsLock = 0
                        });
                }
                //else
                //{
                //    bllAreaArticle.Add(new BookingFood.Model.bf_area_article()
                //    {
                //        AreaId = int.Parse(item.Value),
                //        ArticleId = ret,
                //        Type = "one",
                //        IsLock = 1
                //    });
                //}
	        }
            BookingFood.BLL.bf_article_low_num bllLowNum = new BookingFood.BLL.bf_article_low_num();
            BookingFood.Model.bf_article_low_num lownum = null;
            if (int.Parse(txtLowNum1.Text.Trim()) > 0)
            {
                lownum = new BookingFood.Model.bf_article_low_num();
                lownum.BeginTime = DateTime.Parse(txtBeginTime1.Text.Trim());
                lownum.EndTime = DateTime.Parse(txtEndTime1.Text.Trim() + ":59");
                lownum.LowNum = int.Parse(txtLowNum1.Text.Trim());
                lownum.ArticleId = ret;
                bllLowNum.Add(lownum);
            }
            if (int.Parse(txtLowNum2.Text.Trim()) > 0)
            {
                lownum = new BookingFood.Model.bf_article_low_num();
                lownum.BeginTime = DateTime.Parse(txtBeginTime2.Text.Trim());
                lownum.EndTime = DateTime.Parse(txtEndTime2.Text.Trim() + ":59");
                lownum.LowNum = int.Parse(txtLowNum2.Text.Trim());
                lownum.ArticleId = ret;
                bllLowNum.Add(lownum);
            }
            if (int.Parse(txtLowNum3.Text.Trim()) > 0)
            {
                lownum = new BookingFood.Model.bf_article_low_num();
                lownum.BeginTime = DateTime.Parse(txtBeginTime3.Text.Trim());
                lownum.EndTime = DateTime.Parse(txtEndTime3.Text.Trim() + ":59");
                lownum.LowNum = int.Parse(txtLowNum3.Text.Trim());
                lownum.ArticleId = ret;
                bllLowNum.Add(lownum);
            }
            if (ret < 1)
            {
                result = false;
            }
            return result;
        }
        #endregion

        #region 修改操作=================================
        private bool DoEdit(int _id)
        {
            bool result = true;
            BLL.article bll = new BLL.article();
            Model.article_goods model = bll.GetGoodsModel(_id);

            model.channel_id = new BLL.category().GetModel(int.Parse(ddlCategoryId.SelectedValue)).channel_id;
            model.title = txtTitle.Text.Trim();
            model.category_id = int.Parse(ddlCategoryId.SelectedValue);
            model.opposition_id = int.Parse(ddlOppositionCategoryId.SelectedValue);
            model.nick_id = int.Parse(ddlNickName.SelectedValue);
            model.is_lock = int.Parse(ddlIsLock.SelectedValue);
            model.mark = txtMark.Text.Trim();
            model.only_company = cboOnlyCompany.Checked ? 1 : 0;
            model.head_line = cboHeadLine.Checked ? 1 : 0;
            model.headline_img_url = txtHeadLineImgUrl.Text.Trim();
            string taste = string.Empty;
            foreach (ListItem item in cblTaste.Items)
            {
                if (item.Selected)
                {
                    if (!string.IsNullOrEmpty(taste))
                    {
                        taste += ",";
                    }
                    taste += item.Value;
                }
            }
            model.taste = taste;
            string conditionPrice = string.Empty;
            foreach (ListItem item in cblConditionPrice.Items)
            {
                if (item.Selected)
                {
                    if (!string.IsNullOrEmpty(conditionPrice))
                    {
                        conditionPrice += ",";
                    }
                    conditionPrice += item.Value;
                }
            }
            model.condition_price = conditionPrice;
            model.goods_no = txtGoodsNo.Text;
            model.stock_quantity = int.Parse(txtStockQuantity.Text);
            model.market_price = decimal.Parse(txtMarketPrice.Text);
            model.sell_price = decimal.Parse(txtSellPrice.Text);
            model.zhenghua_price = decimal.Parse(string.IsNullOrEmpty(txtSellPrice.Text) ? "0" : txtZhenghuaPrice.Text);
            model.taobao_price = decimal.Parse(string.IsNullOrEmpty(txtTaobaoPrice.Text) ? "0" : txtTaobaoPrice.Text);
            model.point = int.Parse(txtPoint.Text);
            model.link_url = txtLinkUrl.Text.Trim();
            model.img_url = focus_photo.Value;
            model.content = txtContent.Value;
            model.seo_title = txtSeoTitle.Text.Trim();
            model.seo_keywords = txtSeoKeywords.Text.Trim();
            model.seo_description = txtSeoDescription.Text.Trim();
            model.sort_id = int.Parse(txtSortId.Text.Trim());
            model.click = int.Parse(txtClick.Text.Trim());
            model.digg_good = int.Parse(txtDiggGood.Text.Trim());
            model.digg_bad = int.Parse(txtDiggBad.Text.Trim());
            model.is_msg = 0;
            model.is_top = 0;
            model.is_red = 0;
            model.is_hot = 0;
            model.is_slide = 0;
            
            if (cblItem.Items[0].Selected == true)
            {
                model.is_msg = 1;
            }
            if (cblItem.Items[1].Selected == true)
            {
                model.is_top = 1;
            }
            if (cblItem.Items[2].Selected == true)
            {
                model.is_red = 1;
            }
            if (cblItem.Items[3].Selected == true)
            {
                model.is_hot = 1;
            }
            if (cblItem.Items[4].Selected == true)
            {
                model.is_slide = 1;
            }
            if (cblItem.Items[5].Selected == true)
            {
                
            }
            //用户组价格
            List<Model.goods_group_price> priceList = new List<Model.goods_group_price>();
            for (int i = 0; i < rptPrice.Items.Count; i++)
            {
                int hidPriceId = 0;
                if (!string.IsNullOrEmpty(((HiddenField)rptPrice.Items[i].FindControl("hidePriceId")).Value))
                {
                    hidPriceId = Convert.ToInt32(((HiddenField)rptPrice.Items[i].FindControl("hidePriceId")).Value);
                }
                int hidGroupId = Convert.ToInt32(((HiddenField)rptPrice.Items[i].FindControl("hideGroupId")).Value);
                decimal _price = Convert.ToDecimal(((TextBox)rptPrice.Items[i].FindControl("txtGroupPrice")).Text.Trim());
                priceList.Add(new Model.goods_group_price { id = hidPriceId, article_id = _id, group_id = hidGroupId, price = _price });
            }
            model.goods_group_prices = priceList;
            //保存相册
            if (model.albums != null)
                model.albums.Clear();
            string[] albumArr = Request.Form.GetValues("hide_photo_name");
            string[] remarkArr = Request.Form.GetValues("hide_photo_remark");
            if (albumArr != null)
            {
                List<Model.article_albums> ls = new List<Model.article_albums>();
                for (int i = 0; i < albumArr.Length; i++)
                {
                    string[] imgArr = albumArr[i].Split('|');
                    int img_id = int.Parse(imgArr[0]);
                    if (imgArr.Length == 3)
                    {
                        if (!string.IsNullOrEmpty(remarkArr[i]))
                        {
                            ls.Add(new Model.article_albums { id = img_id, article_id = _id, big_img = imgArr[1], small_img = imgArr[2], remark = remarkArr[i] });
                        }
                        else
                        {
                            ls.Add(new Model.article_albums { id = img_id, article_id = _id, big_img = imgArr[1], small_img = imgArr[2] });
                        }
                    }
                }
                model.albums = ls;
            }

            //扩展属性
            BLL.attributes bll2 = new BLL.attributes();
            DataSet ds2 = bll2.GetList("channel_id=" + this.channel_id);

            List<Model.attribute_value> attrls = new List<Model.attribute_value>();
            foreach (DataRow dr in ds2.Tables[0].Rows)
            {
                int attr_id = int.Parse(dr["id"].ToString());
                string attr_title = dr["title"].ToString();
                string attr_value_id = Request.Form["value_" + attr_id];
                string attr_value_content = Request.Form["content_" + attr_id];
                if (!string.IsNullOrEmpty(attr_value_id) && !string.IsNullOrEmpty(attr_value_content))
                {
                    attrls.Add(new Model.attribute_value { id =  Convert.ToInt32(attr_value_id), article_id = _id, attribute_id = attr_id, title = attr_title, content = attr_value_content });
                }
            }
            model.attribute_values = attrls;
            model.low_num = int.Parse(txtLowNum.Text.Trim());
            model.mp_img_url = txtMpImgUrl.Text.Trim();
            model.change_nums = int.Parse(txtChangeNums.Text.Trim());
            BookingFood.BLL.bf_area_article bllAreaArticle = new BookingFood.BLL.bf_area_article();
            List<BookingFood.Model.bf_area_article> listArea = bllAreaArticle.GetModelList(" Type='one' and ArticleId=" + _id.ToString());
            foreach (var item in listArea)
            {
                bllAreaArticle.Delete(item.Id);
            }
            foreach (ListItem item in cblArea.Items)
            {
                if (item.Selected)
                {
                    bllAreaArticle.Add(new BookingFood.Model.bf_area_article()
                    {
                        AreaId = int.Parse(item.Value),
                        ArticleId = _id,
                        Type = "one",
                        IsLock = 0
                    });
                }
                //else
                //{
                //    bllAreaArticle.Add(new BookingFood.Model.bf_area_article()
                //    {
                //        AreaId = int.Parse(item.Value),
                //        ArticleId = _id,
                //        Type = "one",
                //        IsLock = 1
                //    });
                //}
            }
            BookingFood.BLL.bf_article_low_num bllLowNum = new BookingFood.BLL.bf_article_low_num();
            string delIds = string.Join(",", bllLowNum.GetModelList("ArticleId=" + _id).Select(s => s.Id));
            if(!string.IsNullOrEmpty(delIds))
            {
                bllLowNum.DeleteList(delIds);
            }
            BookingFood.Model.bf_article_low_num lownum = null;
            if(int.Parse(txtLowNum1.Text.Trim())>0)
            {
                lownum = new BookingFood.Model.bf_article_low_num();
                lownum.BeginTime = DateTime.Parse(txtBeginTime1.Text.Trim());
                lownum.EndTime = DateTime.Parse(txtEndTime1.Text.Trim() + ":59");
                lownum.LowNum = int.Parse(txtLowNum1.Text.Trim());
                lownum.ArticleId = _id;
                bllLowNum.Add(lownum);
            }
            if (int.Parse(txtLowNum2.Text.Trim()) > 0)
            {
                lownum = new BookingFood.Model.bf_article_low_num();
                lownum.BeginTime = DateTime.Parse(txtBeginTime2.Text.Trim());
                lownum.EndTime = DateTime.Parse(txtEndTime2.Text.Trim() + ":59");
                lownum.LowNum = int.Parse(txtLowNum2.Text.Trim());
                lownum.ArticleId = _id;
                bllLowNum.Add(lownum);
            }
            if (int.Parse(txtLowNum3.Text.Trim()) > 0)
            {
                lownum = new BookingFood.Model.bf_article_low_num();
                lownum.BeginTime = DateTime.Parse(txtBeginTime3.Text.Trim());
                lownum.EndTime = DateTime.Parse(txtEndTime3.Text.Trim() + ":59");
                lownum.LowNum = int.Parse(txtLowNum3.Text.Trim());
                lownum.ArticleId = _id;
                bllLowNum.Add(lownum);
            }
            if (!bll.Update(model))
            {
                result = false;
            }
            return result;
        }
        #endregion

        //保存
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
            {
                ChkAdminLevel("caipinguanli", DTEnums.ActionEnum.Edit.ToString()); //检查权限
                if (!DoEdit(this.id))
                {
                    JscriptMsg("保存过程中发生错误啦！", "", "Error");
                    return;
                }
                JscriptMsg("修改商品成功啦！", hfUrlReferrer.Value, "Success");
            }
            else //添加
            {
                ChkAdminLevel("caipinguanli", DTEnums.ActionEnum.Add.ToString()); //检查权限
                if (!DoAdd())
                {
                    JscriptMsg("保存过程中发生错误啦！", "", "Error");
                    return;
                }
                JscriptMsg("添加商品成功啦！", hfUrlReferrer.Value, "Success");
            }
        }
    }
}