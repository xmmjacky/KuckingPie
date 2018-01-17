using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DTcms.Common;

namespace DTcms.Web.admin
{
    public partial class index : Web.UI.ManagePage
    {
        protected Model.manager admin_info;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                admin_info = GetAdminInfo();
                BookingFood.BLL.bf_area bllArea = new BookingFood.BLL.bf_area();
                List<BookingFood.Model.bf_area> listArea = bllArea.GetModelList(" ParentId=0 and Id In ("+admin_info.area+")");
                string areaid = Session["AreaId"].ToString();
                foreach (var item in listArea)
                {
                    ltlArea.Text += string.Format("<li {1} data-id=\"{2}\">{0}</li>"
                        , item.Title, string.IsNullOrEmpty(ltlArea.Text) ? "class=\"hover\"" : ""
                        , item.Id.ToString());
                    //List<BookingFood.Model.bf_area> listSubArea = bllArea.GetModelList(" ParentId=" + item.Id.ToString());
                    //ltlSubArea.Text += "<div class=\"tab_con\">";
                    //foreach (var sub in listSubArea)
                    //{
                    //    ltlSubArea.Text += string.Format("<a data-id='{1}' {2}>{0}</a> ", sub.Title, sub.Id
                    //        , sub.Id.ToString() == areaid ? "class=\"hover\"":"");
                    //}
                    //ltlSubArea.Text += "</div>";
                }
                BLL.siteconfig bll = new BLL.siteconfig();
                Model.siteconfig model = bll.loadConfig(Utils.GetXmlMapPath(DTKeys.FILE_SITE_XML_CONFING));
                //ltlSwitch.Text = string.Format("<div class=\"switch {0}\"></div>", model.busy);
            }
        }

        //安全退出
        protected void lbtnExit_Click(object sender, EventArgs e)
        {
            Session[DTKeys.SESSION_ADMIN_INFO] = null;
            Utils.WriteCookie("AdminName", "DTcms", -14400);
            Utils.WriteCookie("AdminPwd", "DTcms", -14400);
            Response.Redirect("login.aspx");
        }
    }
}