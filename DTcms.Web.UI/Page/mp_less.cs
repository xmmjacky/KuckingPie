using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DTcms.Common;
using System.Linq;
using Senparc.Weixin.MP.AdvancedAPIs;

namespace DTcms.Web.UI.Page
{
    public partial class mp_less : Web.UI.BasePage
    {        
        protected string msgbox = string.Empty;
        protected int msg = 0;
        
        protected override void ShowPage()
        {
            this.Init += mp_less_Init;
            
        }

        void mp_less_Init(object sender, EventArgs e)
        {
            InitByMp();
            string time = DateTime.Now.ToString("yyyy-MM-dd");
            DataTable dt = new BLL.orders().GetList(1
                , " user_name='" + openid + "' and add_time between '" + time + " 00:00:00' and '" + time + " 23:59:59'"
                + " and OrderType!='催单' and OrderType!='通知' and is_additional=0 "
                , " id desc").Tables[0];
            if (dt.Rows.Count == 0)
            {
                msgbox = "您今天没有下单，无法补充送错信息！";
                msg = 1;
                return;
            }
            if (dt.Rows[0]["is_less"].ToString() == "2")
            {
                msgbox = "已经补充过信息，无法再次补充！";
                msg = 1;
                return;
            }
            if (dt.Rows[0]["takeout"].ToString() != "0")
            {
                msgbox = "抱歉，堂吃订单直达厨房，不支持更改。";
                msg = 1;
                return;
            }
        }
        
    }

}