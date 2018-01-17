using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DTcms.Common;

namespace DTcms.Web.admin.category
{
    public partial class edit : DTcms.Web.UI.ManagePage
    {
        private string action = DTEnums.ActionEnum.Add.ToString(); //操作类型
        
        private int id = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            string _action = DTRequest.GetQueryString("action");            
            this.id = DTRequest.GetQueryInt("id");
            
            if (!string.IsNullOrEmpty(_action) && _action == DTEnums.ActionEnum.Edit.ToString())
            {
                this.action = DTEnums.ActionEnum.Edit.ToString();//修改类型
                if (this.id == 0)
                {
                    JscriptMsg("传输参数不正确！", "back", "Error");
                    return;
                }
                if (!new DTcms.BLL.category().Exists(this.id))
                {
                    JscriptMsg("类别不存在或已被删除！", "back", "Error");
                    return;
                }
            }
            if (!Page.IsPostBack)
            {
                if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
                {
                    ChkAdminLevel("category", DTEnums.ActionEnum.Edit.ToString()); //检查权限
                    ShowInfo(this.id);
                }
                else
                {
                    ChkAdminLevel("category", DTEnums.ActionEnum.Add.ToString()); //检查权限
                    
                }
            }
        }        

        #region 赋值操作=================================
        private void ShowInfo(int _id)
        {
            BLL.category bll = new BLL.category();
            Model.category model = bll.GetModel(_id);

            ddlChannel.SelectedValue = model.channel_id.ToString();
            txtTitle.Text = model.title;
            txtSortId.Text = model.sort_id.ToString();            
            txtImgUrl.Text = model.img_url;
            txtContent.Value = model.content;
        }
        #endregion

        #region 增加操作=================================
        private bool DoAdd()
        {
            try
            {
                Model.category model = new Model.category();
                BLL.category bll = new BLL.category();

                model.channel_id = int.Parse(ddlChannel.SelectedValue);                
                model.title = txtTitle.Text.Trim();
                model.parent_id = 0;
                model.sort_id = int.Parse(txtSortId.Text.Trim());               
                model.img_url = txtImgUrl.Text.Trim();
                model.content = txtContent.Value;
                if (bll.Add(model) < 1)
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        #endregion

        #region 修改操作=================================
        private bool DoEdit(int _id)
        {
            try
            {
                BLL.category bll = new BLL.category();
                Model.category model = bll.GetModel(_id);                

                model.channel_id = int.Parse(ddlChannel.SelectedValue);     
                model.title = txtTitle.Text.Trim();                
                model.sort_id = int.Parse(txtSortId.Text.Trim());                
                model.img_url = txtImgUrl.Text.Trim();
                model.content = txtContent.Value;
                if (!bll.Update(model))
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        #endregion

        //保存类别
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
            {
                if (!DoEdit(this.id))
                {
                    JscriptMsg("保存过程中发生错误啦！", "", "Error");
                    return;
                }
                JscriptMsg("修改类别成功啦！", "../goods/list.aspx?channel_id=2", "Success");
            }
            else //添加
            {
                if (!DoAdd())
                {
                    JscriptMsg("保存过程中发生错误啦！", "", "Error");
                    return;
                }
                JscriptMsg("添加类别成功啦！", "../goods/list.aspx?channel_id=2", "Success");
            }
        }

    }
}