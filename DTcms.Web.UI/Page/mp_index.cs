using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DTcms.Common;
using System.Linq;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.Helpers;
using Senparc.Weixin.MP.CommonAPIs;
using System.Web.Script.Serialization;
using System.Collections;

namespace DTcms.Web.UI.Page
{
    public partial class mp_index : Web.UI.BasePage
    {
        protected DataTable dtcategory = null;        
        protected DataTable dtParentArea = null;
        protected DataTable dtArea = null;
        protected DataTable dtOfflineArea = null;
        protected decimal totalprice = 0;        
        protected string areabusy = string.Empty;
        protected string arealock = string.Empty;        
        protected BookingFood.Model.bf_carnival carnivalModel = null;
        protected DataTable carnivalDetail = null;
        protected BookingFood.Model.bf_carnival_user carnivalUserModel = null;
        protected int tab = 0;
        protected string additional = string.Empty;
        protected string[] divShowAndHide = { "", "display:none;", "display:none;" };//0:在线/堂吃选择,1:在线区域选择,2:线下区域选择
        protected string defaultAreaId = string.Empty;
        protected string lastOnlineAreaId = string.Empty;
        protected string lastOnlineArea = string.Empty;
        protected string lastOfflineAreaId = string.Empty;
        protected string lastOfflineArea = string.Empty;
        protected string lastOfflineAreaAddress = string.Empty;
        protected string isNoDiscountGood = string.Empty;

        protected string jsnoncestr = string.Empty;
        protected string jstimestamp = string.Empty;
        protected string mp_signature = string.Empty,distributionArea = string.Empty, userAddress=string.Empty;

        protected int avaliableCompanyAmount = 0;
        protected BookingFood.Model.bf_company modelCompany = null;

        protected override void ShowPage()
        {            
            
            this.Init += mp_index_Init;
        }

        void mp_index_Init(object sender, EventArgs e)
        {
            InitByMp();
            tab = DTRequest.GetQueryInt("tab", 1);
            additional = DTRequest.GetQueryString("additional");
            if (string.Equals(additional, "1"))
            {
                defaultAreaId = DTRequest.GetQueryString("areaid");
                divShowAndHide[0] = "display:none;";
            }
            ShopCart.Clear("0");
            userModel = new BLL.users().GetModel(openid);
            if (userModel == null) userModel = new Model.users();
            dtcategory = new BLL.category().GetChildList(0, 2);
            BookingFood.BLL.bf_area bllArea = new BookingFood.BLL.bf_area();
            dtParentArea = bllArea.GetList(" IsShow=1 AND ParentId=0 Order By SortId Asc").Tables[0];
            if (dtParentArea.Rows.Count > 0)
            {
                dtArea = bllArea.
                    GetList(" IsShow=1 AND IsLock=0 AND ParentId=" + dtParentArea.Rows[0]["Id"].ToString() + " Order By SortId Asc").Tables[0];
                foreach (DataRow item in dtArea.Rows)
                {
                    if (string.IsNullOrEmpty(item["DistributionArea"].ToString())) continue;
                    distributionArea += item["Id"].ToString() + "-" + item["Title"].ToString() + "-" + item["DistributionArea"].ToString() + "_";
                }
                distributionArea = distributionArea.TrimEnd('_');
                dtOfflineArea = bllArea.
                    GetList(" IsShow=0 AND ParentId=" + dtParentArea.Rows[0]["Id"].ToString() + " Order By SortId Asc").Tables[0];
            }
            else
            {
                dtArea = new DataTable();
                dtOfflineArea = new DataTable();
            }

            if (this.Context.Request.Cookies["AreaId"] != null)
            {
                BookingFood.Model.bf_area areaModel = bllArea.GetModel(int.Parse(this.Context.Request.Cookies["AreaId"].Value));
                if (areaModel != null)
                {
                    areabusy = areaModel.IsBusy.ToString();
                    arealock = areaModel.IsLock.ToString();
                }
            }
            BookingFood.BLL.bf_carnival bllCarnival = new BookingFood.BLL.bf_carnival();
            carnivalModel = bllCarnival.GetModelList(" Type=1 And GetDate() Between BeginTime And EndTime Order By BeginTime Asc").FirstOrDefault();
            if (carnivalModel != null)
            {
                carnivalDetail = new BLL.article().GetGoodsList(0, " category_id=" + carnivalModel.BusinessId, " sort_id asc,add_time desc").Tables[0];
                carnivalUserModel = new BookingFood.BLL.bf_carnival_user().GetModelList(" UserId=" + userModel.id + " and CarnivalId=" + carnivalModel.Id).FirstOrDefault();
            }
            GetLastAreaId();

            jsnoncestr = JSSDKHelper.GetNoncestr();
            jstimestamp = JSSDKHelper.GetTimestamp();
            string jsapi_token = JsApiTicketContainer.TryGetTicket(config.mp_slave_appid, config.mp_slave_appsecret);
            JSSDKHelper helper = new JSSDKHelper();
            mp_signature = helper.GetSignature(jsapi_token, jsnoncestr, jstimestamp, Context.Request.Url.ToString().Split('#')[0].Replace("/aspx", ""));
            BookingFood.BLL.bf_user_voucher bllUserVoucher = new BookingFood.BLL.bf_user_voucher();
            avaliableCompanyAmount = (int)bllUserVoucher.GetModelList("UserId=" + userModel.id +  " and GetDate()<ExpireTime and Status=0").Sum(s => s.Amount);
            modelCompany = new BookingFood.BLL.bf_company().GetModel(userModel.company_id);
            //外卖定位时,送餐地址列表
            List<BookingFood.Model.bf_user_address> listAddress =
                new BookingFood.BLL.bf_user_address().GetModelList("UserId=" + userModel.id);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            ArrayList listUserArea = new ArrayList();
            foreach (var item in listAddress)
            {
                BookingFood.Model.bf_area modelArea = bllArea.GetModel(item.AreaId);
                listUserArea.Add(new {Id=item.Id,item.NickName,item.Address, item.Telphone, item.AreaId, item.AreaType, AreaTitle= modelArea.Title, AreaAddress=modelArea.Address });
            }
            userAddress = serializer.Serialize(listUserArea);
            userAddress = userAddress.Replace("\"", "'");
        }

        protected string GetIsRun()
        {
            if (DateTime.Now < DateTime.Parse(config.starttime) || DateTime.Now > DateTime.Parse(config.endtime))
            {
                return "false";
            }
            else
            {
                return "true";
            }
        }

        protected string GetForHereIsRun()
        {
            if (DateTime.Now < DateTime.Parse(config.starttime_here) || DateTime.Now > DateTime.Parse(config.endtime_here))
            {
                return "false";
            }
            else
            {
                return "true";
            }
        }

        protected void GetLastAreaId()
        {
            string uninitarea = DTRequest.GetQueryString("uninitarea");
            if (!string.IsNullOrEmpty(uninitarea)) return;
            BLL.orders bll = new BLL.orders();
            DataTable dt = bll.GetList(1, " user_name='" + openid + "' And takeout=0", " id desc").Tables[0];
            lastOnlineAreaId = dt.Rows.Count > 0 ? dt.Rows[0]["area_id"].ToString() : "";
            lastOnlineArea = dt.Rows.Count > 0 ? dt.Rows[0]["area_title"].ToString() : "";
            dt = bll.GetList(1, " user_name='" + openid + "' And takeout in (1,2)", " id desc").Tables[0];
            BookingFood.BLL.bf_area bllArea = new BookingFood.BLL.bf_area();
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["area_id"].ToString() != dt.Rows[0]["before_change_area_id"].ToString() && dt.Rows[0]["before_change_area_id"].ToString() == "0")
                {
                    if(dt.Rows[0]["takeout"].ToString()=="1")
                    {
                        lastOfflineAreaId = dt.Rows[0]["area_id"].ToString();
                        lastOfflineArea = bllArea.GetModel(int.Parse(lastOfflineAreaId)).Title;
                    }
                    else if(dt.Rows[0]["takeout"].ToString() == "2")
                    {
                        lastOfflineAreaId = dt.Rows[0]["area_id"].ToString();
                        BookingFood.Model.bf_area _beforeChangeArea = bllArea.GetModel(int.Parse(lastOfflineAreaId));
                        _beforeChangeArea = bllArea.GetModel(_beforeChangeArea.OppositeId);
                        lastOfflineAreaId = _beforeChangeArea.Id.ToString();
                        lastOfflineArea = _beforeChangeArea.Title;
                    }
                }
                else if (dt.Rows[0]["area_id"].ToString()!= dt.Rows[0]["before_change_area_id"].ToString())
                {
                    lastOfflineAreaId = dt.Rows[0]["before_change_area_id"].ToString();
                    lastOfflineArea = bllArea.GetModel(int.Parse(dt.Rows[0]["before_change_area_id"].ToString())).Title;
                }
                else
                {
                    lastOfflineAreaId = dt.Rows.Count > 0 ? dt.Rows[0]["area_id"].ToString() : "";
                    lastOfflineArea = dt.Rows.Count > 0 ? dt.Rows[0]["area_title"].ToString() : "";
                }
            }
            
            if(!string.IsNullOrEmpty(lastOfflineAreaId) && lastOfflineAreaId != "0")
            {
                lastOfflineAreaAddress = bllArea.GetModel(int.Parse(lastOfflineAreaId)).Address;
            }
            //单独屏蔽宜山路店 ID 7
            if(lastOfflineAreaId=="7")
            {
                lastOfflineAreaId = string.Empty;
                lastOfflineAreaAddress = string.Empty;
            }
            
        }
    }

}