using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DTcms.Common;
using System.Linq;
using Senparc.Weixin.MP.AdvancedAPIs;

namespace DTcms.Web.UI.Page
{
    public partial class mp_full : Web.UI.BasePage
    {        
        protected BookingFood.Model.bf_carnival_user carnivalUserModel = null;
        protected BookingFood.Model.bf_carnival carnivalModel = null;
        protected DataTable carnivalDetail = null;
        
        protected override void ShowPage()
        {
            this.Init += mp_full_Init;
            
        }

        void mp_full_Init(object sender, EventArgs e)
        {
            InitByMp();
            ShopCart.Clear("0");
            BookingFood.BLL.bf_carnival bllCarnival = new BookingFood.BLL.bf_carnival();
            carnivalModel = bllCarnival.GetModelList(" Type=1 And GetDate() Between BeginTime And EndTime Order By BeginTime Asc").FirstOrDefault();
            if (carnivalModel != null)
            {
                carnivalDetail = new BLL.article().GetGoodsList(0, " category_id=" + carnivalModel.BusinessId, " sort_id asc,add_time desc").Tables[0];
                carnivalUserModel = new BookingFood.BLL.bf_carnival_user().GetModelList(" UserId=" + userModel.id + " and CarnivalId=" + carnivalModel.Id).FirstOrDefault();
            }
        }        
    }

}