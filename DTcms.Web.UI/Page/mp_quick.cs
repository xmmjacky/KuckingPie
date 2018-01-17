using System;
using System.Data;
using DTcms.Common;

namespace DTcms.Web.UI.Page
{
    public partial class mp_quick : Web.UI.BasePage
    {        
        protected string msgbox = string.Empty;
        protected int msg = 0;
        
        protected override void ShowPage()
        {
            this.Init += mp_quick_Init;
            
        }

        void mp_quick_Init(object sender, EventArgs e)
        {
            InitByMp();
            string time = DateTime.Now.ToString("yyyy-MM-dd");
            DataTable dt = new BLL.orders().GetList(1
                , " user_name='" + openid + "' and (add_time between '"
                + time + " 00:00:00' and '" + time + " 23:59:59') and OrderType!='催单' and is_additional=0", " id desc").Tables[0];
            if (dt.Rows.Count == 0)
            {
                msgbox = "您今天还没有提交过订单！";
                msg = 1;
                return;
            }
            if (dt.Rows[0]["takeout"].ToString() != "0")
            {
                msgbox = "抱歉，堂吃订单直达厨房，不支持催单。";
                msg = 1;
                return;
            }
            if (DateTime.Parse(dt.Rows[0]["add_time"].ToString()).AddMinutes(40) >= DateTime.Now)
            {
                msgbox = "抱歉，下单40分钟后可以催单";
                msg = 1;
                return;
            }
            if (DateTime.Parse("11:40") >= DateTime.Now)
            {
                msgbox = "11点开始配送，正在配送中，请稍候！";
                msg = 1;
                return;
            }
            if (!string.IsNullOrEmpty(dt.Rows[0]["distribution_time"].ToString()) &&
                DateTime.Parse(dt.Rows[0]["distribution_time"].ToString()).AddHours(1) < DateTime.Now)
            {
                msgbox = "派送后一小时内可用";
                msg = 1;
                return;
            }
            if (dt.Rows[0]["is_quick"].ToString() == "0")
            {
                BLL.orders bllOrder = new BLL.orders();
                int orderid = int.Parse(dt.Rows[0]["id"].ToString());
                Model.orders orderModel = bllOrder.GetModel(orderid);
                orderModel.OrderType = "催单";
                orderModel.message = "[催单]";
                orderModel.order_no = Utils.GetOrderNumber(); //订单号
                orderModel.add_time = DateTime.Now;
                orderModel.order_goods = null;
                orderModel.status = 1;
                orderModel.confirm_time = null;
                orderModel.worker_id = 0;
                orderModel.worker_name = "";
                orderModel.real_amount = 0;
                orderModel.real_freight = 0;
                orderModel.payable_amount = 0;
                orderModel.payable_freight = 0;
                orderModel.payment_fee = 0;
                orderModel.order_amount = 0;
                bllOrder.Add(orderModel);
                bllOrder.UpdateField(orderid, "is_quick=1");
            }
            msgbox = "催单成功，我们将极速处理";
        }
        
    }

}