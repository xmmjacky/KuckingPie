using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DTcms.Common;
using System.Linq;
using Senparc.Weixin.MP.AdvancedAPIs;
using System.Web;

namespace DTcms.Web.UI.Page
{
    public partial class mp_additional : Web.UI.BasePage
    {        
        protected string msgbox = string.Empty;
        protected int msg = 0;
        
        protected override void ShowPage()
        {
            this.Init += mp_additional_Init;
            
            
        }

        void mp_additional_Init(object sender, EventArgs e)
        {
            InitByMp();
            DataTable dt = new BLL.orders().GetList(1
                , " user_name='" + openid + "' and OrderType!='催单' and OrderType!='通知' and is_additional=0", " id desc").Tables[0];

            if (dt.Rows.Count == 0)
            {
                msgbox = "您还没有提交过订单！";
                msg = 1;
                return;
            }
            //if (dt.Rows[0]["additional_count"].ToString() == "2")
            //{
            //    msgbox = "只能提交两次补单！";
            //    msg = 1;
            //    return;
            //}
            if (dt.Rows[0]["takeout"].ToString() != "0")
            {
                msgbox = "堂吃/外带无需补单，可直接提交新的订单！";
                msg = 1;
                return;
            }
            if (DateTime.Parse(dt.Rows[0]["add_time"].ToString()).AddMinutes(config.additional) >= DateTime.Now)
            {
                HttpContext.Current.Response.Redirect("/mp_index.aspx?showwxpaytitle=1&openid=" + openid + "&additional=1&state=slave&areaid=" + dt.Rows[0]["area_id"].ToString());
                return;
            }
        }
        
    }

}