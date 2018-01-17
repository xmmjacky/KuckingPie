using System;
using System.Text;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DTcms.Common;

namespace DTcms.Web.admin.goods
{
    public partial class combo : Web.UI.ManagePage
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
                if (!new BookingFood.BLL.bf_good_combo().Exists(this.id))
                {
                    JscriptMsg("信息不存在或已被删除！", "back", "Error");
                    return;
                }
            }
            if (!Page.IsPostBack)
            {
                hfUrlReferrer.Value = this.Request.UrlReferrer.ToString();
                TreeBind(this.channel_id); //绑定类别
                string areaid = Session["areaid"].ToString();
                List<BookingFood.Model.bf_area> listArea = new BookingFood.BLL.bf_area().GetModelList(" ParentId=" + areaid);
                foreach (var item in listArea)
                {
                    cblArea.Items.Add(new ListItem(item.Title, item.Id.ToString()));
                }
                List<BookingFood.Model.bf_condition> listCondition = new BookingFood.BLL.bf_condition().GetModelList("");
                foreach (var item in listCondition)
                {
                    cblTaste.Items.Add(new ListItem(item.Title, item.Title));
                }
                DataTable dt = new BLL.category().GetChildList(0, 3);
                foreach (DataRow item in dt.Rows)
                {
                    cblComboCategory.Items.Add(new ListItem(item["title"].ToString(), item["id"].ToString()));
                }
                if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
                {
                    ShowInfo(this.id);
                }
                else
                {
                    
                }
            }
        }

        #region 赋值操作=================================
        private void ShowInfo(int _id)
        {
            BookingFood.BLL.bf_good_combo bll = new BookingFood.BLL.bf_good_combo();
            BookingFood.Model.bf_good_combo model = bll.GetModel(_id);

            ddlCategoryId.SelectedValue = model.CategoryId.ToString();
            txtTitle.Text = model.Title;
            ddlIsLock.SelectedValue = model.Is_Lock.ToString();
            if (!string.IsNullOrEmpty(model.Taste))
            {
                string[] tastes = model.Taste.Split(',');
                foreach (var item in tastes)
                {
                    cblTaste.Items.FindByValue(item).Selected = true;
                }
            }
            List<BookingFood.Model.bf_good_combo_detail> listComboDetail = 
                new BookingFood.BLL.bf_good_combo_detail().GetModelList(" Type='category' AND GoodComboId=" + _id.ToString());
            foreach (var item in listComboDetail)
            {
                cblComboCategory.Items.FindByValue(item.BusinessId.ToString()).Selected = true;
            }
            List<BookingFood.Model.bf_area_article> listAreaArticle =
                new BookingFood.BLL.bf_area_article().GetModelList(" Type='category' and ArticleId=" + _id.ToString());
            foreach (var item in listAreaArticle)
            {
                if (cblArea.Items.FindByValue(item.AreaId.ToString()) != null)
                {
                    cblArea.Items.FindByValue(item.AreaId.ToString()).Selected = true;
                }
                
            }            
            txtSortId.Text = model.SortId.ToString();            
            txtContent.Value = model.Content;
            txtImgUrl.Text = model.Photo;
        }
        #endregion

        #region 绑定类别=================================
        private void TreeBind(int _channel_id)
        {
            BLL.category bll = new BLL.category();
            DataTable dt = bll.GetList(0, _channel_id);

            this.ddlCategoryId.Items.Clear();
            this.ddlCategoryId.Items.Add(new ListItem("请选择类别...", ""));
            foreach (DataRow dr in dt.Rows)
            {
                string Id = dr["id"].ToString();
                int ClassLayer = int.Parse(dr["class_layer"].ToString());
                string Title = dr["title"].ToString().Trim();

                if (ClassLayer == 1)
                {
                    this.ddlCategoryId.Items.Add(new ListItem(Title, Id));
                }
                else
                {
                    Title = "├ " + Title;
                    Title = Utils.StringOfChar(ClassLayer - 1, "　") + Title;
                    this.ddlCategoryId.Items.Add(new ListItem(Title, Id));
                }
            }
        }
        #endregion

        #region 增加操作=================================
        private bool DoAdd()
        {
            bool result = true;
            BookingFood.Model.bf_good_combo model = new BookingFood.Model.bf_good_combo();
            BookingFood.BLL.bf_good_combo bll = new BookingFood.BLL.bf_good_combo();

            
            model.Title = txtTitle.Text.Trim();
            model.CategoryId = int.Parse(ddlCategoryId.SelectedValue);
            model.Is_Lock = int.Parse(ddlIsLock.SelectedValue);
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
            model.Taste = taste;            
            model.Content = txtContent.Value;            
            model.SortId = int.Parse(txtSortId.Text.Trim());
            model.Photo = txtImgUrl.Text.Trim();
            model.AddDate = DateTime.Now;
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
                            Type = "category",
                            IsLock = 0
                        });
                }
                //else
                //{
                //    bllAreaArticle.Add(new BookingFood.Model.bf_area_article()
                //    {
                //        AreaId = int.Parse(item.Value),
                //        ArticleId = ret,
                //        Type = "category",
                //        IsLock = 1
                //    });
                //}
	        }
            BookingFood.BLL.bf_good_combo_detail bllComboDetail = new BookingFood.BLL.bf_good_combo_detail();
            DTcms.Model.category categoryModel = new Model.category();
            BLL.category bllCategory = new BLL.category();
            foreach (ListItem item in cblComboCategory.Items)
            {                
                if (item.Selected)
                {
                    categoryModel = bllCategory.GetModel(int.Parse(item.Value));
                    bllComboDetail.Add(new BookingFood.Model.bf_good_combo_detail()
                    {
                        Type = "category",
                        GoodComboId = ret,
                        BusinessId = categoryModel.id,
                        BUsinessTitle = categoryModel.title,
                        SortId = categoryModel.sort_id
                    });
                }
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
            BookingFood.BLL.bf_good_combo bll = new BookingFood.BLL.bf_good_combo();
            BookingFood.Model.bf_good_combo model = bll.GetModel(_id);

            
            model.Title = txtTitle.Text.Trim();
            model.CategoryId = int.Parse(ddlCategoryId.SelectedValue);
            model.Is_Lock = int.Parse(ddlIsLock.SelectedValue);
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
            model.Taste = taste;            
            model.Content = txtContent.Value;            
            model.SortId = int.Parse(txtSortId.Text.Trim());
            model.Photo = txtImgUrl.Text.Trim();
            BookingFood.BLL.bf_area_article bllAreaArticle = new BookingFood.BLL.bf_area_article();
            List<BookingFood.Model.bf_area_article> listArea = bllAreaArticle.GetModelList(" Type='category' and ArticleId=" + _id.ToString());
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
                        Type = "category",
                        IsLock = 0
                    });
                }
                //else
                //{
                //    bllAreaArticle.Add(new BookingFood.Model.bf_area_article()
                //    {
                //        AreaId = int.Parse(item.Value),
                //        ArticleId = _id,
                //        Type = "category",
                //        IsLock = 1
                //    });
                //}
            }
            BookingFood.BLL.bf_good_combo_detail bllComboDetail = new BookingFood.BLL.bf_good_combo_detail();
            List<BookingFood.Model.bf_good_combo_detail> listComboDetail =
                bllComboDetail.GetModelList(" Type='category' AND GoodComboId=" + _id.ToString());
            foreach (var item in listComboDetail)
            {
                bllComboDetail.Delete(item.Id);
            }
            DTcms.Model.category categoryModel = new Model.category();
            BLL.category bllCategory = new BLL.category();
            foreach (ListItem item in cblComboCategory.Items)
            {
                if (item.Selected)
                {
                    categoryModel = bllCategory.GetModel(int.Parse(item.Value));
                    bllComboDetail.Add(new BookingFood.Model.bf_good_combo_detail()
                    {
                        Type = "category",
                        GoodComboId = _id,
                        BusinessId = categoryModel.id,
                        BUsinessTitle = categoryModel.title,
                        SortId = categoryModel.sort_id
                    });
                }
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