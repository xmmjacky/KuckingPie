using System;
using System.Text;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DTcms.Common;
using System.Text.RegularExpressions;

namespace DTcms.Web.admin.area
{
    public partial class edit : Web.UI.ManagePage
    {
        private string action = DTEnums.ActionEnum.Add.ToString(); //操作类型
        private int id = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            string _action = DTRequest.GetQueryString("action");
            
            if (!string.IsNullOrEmpty(_action) && _action == DTEnums.ActionEnum.Edit.ToString())
            {
                this.action = DTEnums.ActionEnum.Edit.ToString();//修改类型
                this.id = DTRequest.GetQueryInt("id");
                if (this.id == 0)
                {
                    JscriptMsg("传输参数不正确！", "back", "Error");
                    return;
                }
                if (!new BookingFood.BLL.bf_area().Exists(this.id))
                {
                    JscriptMsg("信息不存在或已被删除！", "back", "Error");
                    return;
                }
            }
            if (!Page.IsPostBack)
            {
                BookingFood.BLL.bf_area bll = new BookingFood.BLL.bf_area();
                List<BookingFood.Model.bf_area> list = bll.GetModelList(" ParentId=0 and id!="+this.id.ToString());
                foreach (var item in list)
                {
                    ddlParentId.Items.Add(new ListItem(item.Title, item.Id.ToString()));
                }
                DataTable dt = new BLL.manager().GetList(" is_lock=0").Tables[0];
                foreach (DataRow item in dt.Rows)
                {
                    ddlManager.Items.Add(new ListItem(item["user_name"].ToString(), item["id"].ToString()));
                }
                //对应区域
                
                list = bll.GetModelList(" ParentId=1");
                foreach (var item in list)
                {
                    ddlOppositeId.Items.Add(new ListItem(item.Title, item.Id.ToString()));
                }
                list = bll.GetModelList("ParentId=1 And IsShow=1 And Id!=" + this.id.ToString());
                foreach (var item in list)
                {
                    cblChangeOrderArea.Items.Add(new ListItem(item.Title, item.Id.ToString()));
                }
                if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
                {
                    ShowInfo(this.id);
                }                
            }
        }

        #region 赋值操作=================================
        private void ShowInfo(int _id)
        {
            BookingFood.BLL.bf_area bll = new BookingFood.BLL.bf_area();
            BookingFood.Model.bf_area model = bll.GetModel(_id);
            txtArea.Text = model.Title;
            txtSort.Text = model.SortId.ToString();
            txtDescription.Text = model.Description;
            ddlParentId.SelectedValue = model.ParentId.ToString();
            ddlManager.SelectedValue = model.ManagerId.ToString();
            cboIsShow.Checked = model.IsShow == 1 ? true : false;
            cboIsBusy.Checked = model.IsBusy == 1 ? true : false;
            cboIsLock.Checked = model.IsLock == 1 ? true : false;
            hfDistributionArea.Value = model.DistributionArea;
            hfDistributionArea_2.Value = model.DistributionArea_2;
            hfLag.Value = model.Lat.ToString();
            hfLng.Value = model.Lng.ToString();
            txtAddress.Text = model.Address;
            ddlOppositeId.SelectedValue = model.OppositeId.ToString();
            cboIsUnWelcome.Checked = model.IsUnWelcome == 1 ? true : false;
            txtMeituanCookie.Text = model.MeituanCookie;
            txtBaiduCookie.Text = model.BaiduCookie;
            txtElemeCookie.Text = model.ElemeCookie;

            if (!string.IsNullOrEmpty(model.ChangeOrderArea))
            {
                foreach (var item in model.ChangeOrderArea.Split(','))
                {
                    ListItem listitem = cblChangeOrderArea.Items.FindByValue(item);
                    if (listitem != null) listitem.Selected = true;
                } 
            }
        }
        #endregion

        #region 增加操作=================================
        private bool DoAdd()
        {
            bool result = true;
            BookingFood.Model.bf_area model = new BookingFood.Model.bf_area();
            BookingFood.BLL.bf_area bll = new BookingFood.BLL.bf_area();

            model.Title = txtArea.Text.Trim();
            model.OppositeId = int.Parse(ddlOppositeId.SelectedValue);
            model.SortId = int.Parse(txtSort.Text.Trim());
            model.Description = txtDescription.Text;
            model.ParentId = int.Parse(ddlParentId.SelectedValue);
            model.ManagerId = int.Parse(ddlManager.SelectedValue);
            model.ManagerName = ddlManager.SelectedItem.Text;
            if (ddlParentId.SelectedValue != "0")
            {
                model.ParentTitle = ddlParentId.SelectedItem.Text;
            }
            model.IsShow = cboIsShow.Checked ? 1 : 0;
            model.IsLock = cboIsLock.Checked ? 1 : 0;
            model.IsBusy = cboIsBusy.Checked ? 1 : 0;
            model.DistributionArea = hfDistributionArea.Value.TrimEnd('|');
            model.DistributionArea_2 = hfDistributionArea_2.Value.TrimEnd('|');
            if (!string.IsNullOrEmpty(hfLag.Value) && !string.IsNullOrEmpty(hfLng.Value))
            {
                model.Lat = decimal.Parse(hfLag.Value);
                model.Lng = decimal.Parse(hfLng.Value);
            }
            model.Address = txtAddress.Text.Trim();
            foreach (ListItem item in cblChangeOrderArea.Items)
            {
                if(item.Selected)
                {
                    model.ChangeOrderArea += item.Value + ",";
                }
            }
            if(!string.IsNullOrEmpty(model.ChangeOrderArea))
            {
                model.ChangeOrderArea = model.ChangeOrderArea.TrimEnd(',');
            }
            model.IsUnWelcome = cboIsUnWelcome.Checked ? 1 : 0;
            model.MeituanCookie = txtMeituanCookie.Text.Trim();
            model.BaiduCookie = txtBaiduCookie.Text.Trim();
            model.ElemeCookie = txtElemeCookie.Text.Trim();

            int areaid = bll.Add(model);
            if (areaid < 1)
            {
                result = false;
            }
            //将所有单品和套餐关联当前增加的区域
            int totalCount = 0;
            DataTable dt = new BLL.article().GetGoodsListUnionCombo(999999, 1, " 1=1 ", "sort_id asc,add_time desc", out totalCount).Tables[0];
            BookingFood.BLL.bf_area_article bllAreaArticle = new BookingFood.BLL.bf_area_article();
            foreach (DataRow item in dt.Rows)
            {
                if(item["type"].ToString() == "one")
                {
                    bllAreaArticle.Add(new BookingFood.Model.bf_area_article()
                    {
                        AreaId = areaid,
                        ArticleId = int.Parse(item["id"].ToString()),
                        Type = "one",
                        IsLock = 0
                    });
                }else if (item["type"].ToString() == "combo")
                {
                    bllAreaArticle.Add(new BookingFood.Model.bf_area_article()
                    {
                        AreaId = areaid,
                        ArticleId = int.Parse(item["id"].ToString()),
                        Type = "category",
                        IsLock = 0
                    });
                }
            }
            return result;
        }
        #endregion

        #region 修改操作=================================
        private bool DoEdit(int _id)
        {
            bool result = true;
            BookingFood.BLL.bf_area bll = new BookingFood.BLL.bf_area();
            BookingFood.Model.bf_area model = bll.GetModel(_id);

            model.Title = txtArea.Text.Trim();
            model.OppositeId = int.Parse(ddlOppositeId.SelectedValue);
            model.SortId = int.Parse(txtSort.Text.Trim());
            model.Description = txtDescription.Text;
            model.ParentId = int.Parse(ddlParentId.SelectedValue);
            model.ParentTitle = "";
            if (ddlParentId.SelectedValue != "0")
            {
                model.ParentTitle = ddlParentId.SelectedItem.Text;
            }
            model.IsShow = cboIsShow.Checked ? 1 : 0;
            model.IsLock = cboIsLock.Checked ? 1 : 0;
            model.IsBusy = cboIsBusy.Checked ? 1 : 0;
            model.ManagerId = int.Parse(ddlManager.SelectedValue);
            model.ManagerName = ddlManager.SelectedItem.Text;
            model.DistributionArea = hfDistributionArea.Value.TrimEnd('|');
            model.DistributionArea_2 = hfDistributionArea_2.Value.TrimEnd('|');
            if (!string.IsNullOrEmpty(hfLag.Value) && !string.IsNullOrEmpty(hfLng.Value))
            {
                model.Lat = decimal.Parse(hfLag.Value);
                model.Lng = decimal.Parse(hfLng.Value);
            }
            model.Address = txtAddress.Text.Trim();
            model.ChangeOrderArea = string.Empty;
            foreach (ListItem item in cblChangeOrderArea.Items)
            {
                if (item.Selected)
                {
                    model.ChangeOrderArea += item.Value + ",";
                }
            }
            model.ChangeOrderArea = model.ChangeOrderArea.TrimEnd(',');
            model.IsUnWelcome = cboIsUnWelcome.Checked ? 1 : 0;
            model.MeituanCookie = txtMeituanCookie.Text.Trim();
            model.BaiduCookie = txtBaiduCookie.Text.Trim();
            model.ElemeCookie = txtElemeCookie.Text.Trim();

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
                ChkAdminLevel("bf_area", DTEnums.ActionEnum.Edit.ToString()); //检查权限
                if (!DoEdit(this.id))
                {
                    JscriptMsg("保存过程中发生错误啦！", "", "Error");
                    return;
                }
                JscriptMsg("修改商品成功啦！", "list.aspx", "Success");
            }
            else //添加
            {
                ChkAdminLevel("bf_area", DTEnums.ActionEnum.Add.ToString()); //检查权限
                if (!DoAdd())
                {
                    JscriptMsg("保存过程中发生错误啦！", "", "Error");
                    return;
                }
                JscriptMsg("添加商品成功啦！", "list.aspx", "Success");
            }
        }

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
    }
}