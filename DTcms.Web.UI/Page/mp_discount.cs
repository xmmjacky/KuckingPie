using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DTcms.Common;
using System.Linq;
using Senparc.Weixin.MP.AdvancedAPIs;

namespace DTcms.Web.UI.Page
{
    public partial class mp_discount : Web.UI.BasePage
    {        
        protected DataTable dtOfflineArea = null;        
        
        protected List<BookingFood.Model.bf_carnival> carnivalOffline = null;
        

        protected override void ShowPage()
        {
            this.Init += mp_discount_Init;       
            
        }

        void mp_discount_Init(object sender, EventArgs e)
        {
            InitByMp();
            ShopCart.Clear("0");
            BookingFood.BLL.bf_carnival bllCarnival = new BookingFood.BLL.bf_carnival();
            carnivalOffline = bllCarnival.GetModelList(" Type=2 And GetDate() Between BeginTime And EndTime Order By BeginTime Asc");
        }        
    }

}