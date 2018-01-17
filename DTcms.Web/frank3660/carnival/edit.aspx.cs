using System;
using System.Text;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DTcms.Common;

namespace DTcms.Web.admin.carnival
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
                if (!new BookingFood.BLL.bf_carnival().Exists(this.id))
                {
                    JscriptMsg("信息不存在或已被删除！", "back", "Error");
                    return;
                }
            }
            if (!Page.IsPostBack)
            {
                hfUrlReferrer.Value = this.Request.UrlReferrer.ToString();
                string areaid = Session["areaid"].ToString();
                List<BookingFood.Model.bf_area> listArea = new BookingFood.BLL.bf_area().GetModelList(" ParentId=" + areaid);
                foreach (var item in listArea)
                {
                    cblArea.Items.Add(new ListItem(item.Title, item.Id.ToString()));
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
            BookingFood.BLL.bf_carnival bll = new BookingFood.BLL.bf_carnival();
            BookingFood.Model.bf_carnival model = bll.GetModel(_id);

            txtTitle.Text = model.Title;
            txtBeginTime.Text = model.BeginTime.ToString("yyyy-MM-dd HH:mm");
            txtEndTime.Text = model.EndTime.ToString("yyyy-MM-dd HH:mm");
            ListItem listitem = cblComboCategory.Items.FindByValue(model.BusinessId.ToString());
            if(listitem!=null)
            {
                listitem.Selected = true;
            }
            rblType.SelectedValue = model.Type.ToString();

            List<BookingFood.Model.bf_carnival_area> listAreaArticle =
                new BookingFood.BLL.bf_carnival_area().GetModelList(" CarnivalId=" + _id.ToString());
            foreach (var item in listAreaArticle)
            {

                if (cblArea.Items.FindByValue(item.AreaId.ToString()) != null)
                {
                    cblArea.Items.FindByValue(item.AreaId.ToString()).Selected = true;
                }
            }
        }
        #endregion

        #region 增加操作=================================
        private bool DoAdd()
        {
            bool result = true;
            BookingFood.Model.bf_carnival model = new BookingFood.Model.bf_carnival();
            BookingFood.BLL.bf_carnival bll = new BookingFood.BLL.bf_carnival();

            model.Title = txtTitle.Text.Trim();
            model.BeginTime = DateTime.Parse(txtBeginTime.Text.Trim());
            model.EndTime = DateTime.Parse(txtEndTime.Text.Trim());
            model.BusinessId = int.Parse(cblComboCategory.SelectedValue);
            model.BusinessTitle = cblComboCategory.SelectedItem.Text;
            model.Type = int.Parse(rblType.SelectedValue);
            int ret = bll.Add(model);
            BookingFood.BLL.bf_carnival_area bllAreaArticle = new BookingFood.BLL.bf_carnival_area();
            foreach (ListItem item in cblArea.Items)
            {
                if (item.Selected)
                {
                    bllAreaArticle.Add(new BookingFood.Model.bf_carnival_area()
                    {
                        AreaId = int.Parse(item.Value),
                        CarnivalId = ret
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
            BookingFood.BLL.bf_carnival bll = new BookingFood.BLL.bf_carnival();
            BookingFood.Model.bf_carnival model = bll.GetModel(_id);
            model.Title = txtTitle.Text.Trim();
            model.BeginTime = DateTime.Parse(txtBeginTime.Text.Trim());
            model.EndTime = DateTime.Parse(txtEndTime.Text.Trim());
            model.BusinessId = int.Parse(cblComboCategory.SelectedValue);
            model.BusinessTitle = cblComboCategory.SelectedItem.Text;
            model.Type = int.Parse(rblType.SelectedValue);
            BookingFood.BLL.bf_carnival_area bllAreaArticle = new BookingFood.BLL.bf_carnival_area();
            List<BookingFood.Model.bf_carnival_area> listArea = bllAreaArticle.GetModelList(" CarnivalId=" + _id.ToString());
            foreach (var item in listArea)
            {
                bllAreaArticle.Delete(item.Id);
            }
            foreach (ListItem item in cblArea.Items)
            {
                if (item.Selected)
                {
                    bllAreaArticle.Add(new BookingFood.Model.bf_carnival_area()
                    {
                        AreaId = int.Parse(item.Value),
                        CarnivalId = _id
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
                JscriptMsg("修改活动成功啦！", hfUrlReferrer.Value, "Success");
            }
            else //添加
            {
                ChkAdminLevel("caipinguanli", DTEnums.ActionEnum.Add.ToString()); //检查权限
                if (!DoAdd())
                {
                    JscriptMsg("保存过程中发生错误啦！", "", "Error");
                    return;
                }
                JscriptMsg("添加活动成功啦！", hfUrlReferrer.Value, "Success");
            }
        }
    }
}