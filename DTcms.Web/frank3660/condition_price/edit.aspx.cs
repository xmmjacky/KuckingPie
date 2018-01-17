using System;
using System.Text;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DTcms.Common;
using System.Text.RegularExpressions;

namespace DTcms.Web.admin.condition_price
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
                if (!new BookingFood.BLL.bf_condition_price().Exists(this.id))
                {
                    JscriptMsg("信息不存在或已被删除！", "back", "Error");
                    return;
                }
            }
            if (!Page.IsPostBack)
            {                
                if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
                {
                    ShowInfo(this.id);
                }                
            }
        }

        #region 赋值操作=================================
        private void ShowInfo(int _id)
        {
            BookingFood.BLL.bf_condition_price bll = new BookingFood.BLL.bf_condition_price();
            BookingFood.Model.bf_condition_price model = bll.GetModel(_id);
            txtTitle.Text = model.Title;
            txtSort.Text = model.SortId.ToString();
            txtPrice.Text = model.Price.ToString("0.00");
        }
        #endregion

        #region 增加操作=================================
        private bool DoAdd()
        {
            bool result = true;
            BookingFood.Model.bf_condition_price model = new BookingFood.Model.bf_condition_price();
            BookingFood.BLL.bf_condition_price bll = new BookingFood.BLL.bf_condition_price();

            model.Title = txtTitle.Text.Trim();
            model.SortId = int.Parse(txtSort.Text.Trim());
            model.Price = decimal.Parse(txtPrice.Text.Trim());
            if (bll.Add(model) < 1)
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
            BookingFood.BLL.bf_condition_price bll = new BookingFood.BLL.bf_condition_price();
            BookingFood.Model.bf_condition_price model = bll.GetModel(_id);

            model.Title = txtTitle.Text.Trim();
            model.SortId = int.Parse(txtSort.Text.Trim());
            model.Price = decimal.Parse(txtPrice.Text.Trim());

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
                ChkAdminLevel("bf_condition_price", DTEnums.ActionEnum.Edit.ToString()); //检查权限
                if (!DoEdit(this.id))
                {
                    JscriptMsg("保存过程中发生错误啦！", "", "Error");
                    return;
                }
                JscriptMsg("修改商品成功啦！", "list.aspx", "Success");
            }
            else //添加
            {
                ChkAdminLevel("bf_condition_price", DTEnums.ActionEnum.Add.ToString()); //检查权限
                if (!DoAdd())
                {
                    JscriptMsg("保存过程中发生错误啦！", "", "Error");
                    return;
                }
                JscriptMsg("添加商品成功啦！", "list.aspx", "Success");
            }
        }
        
    }
}