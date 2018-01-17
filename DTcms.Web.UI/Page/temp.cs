using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DTcms.Common;

namespace DTcms.Web.UI.Page
{
    public partial class temp : Web.UI.BasePage
    {
        protected DataTable dtcategory = null;
        //protected DataTable dtcombo = null;
        //protected DataTable dtgoods = null;
        protected DataTable dtParentArea = null;
        protected DataTable dtArea = null;
        protected decimal totalprice = 0;
        protected string isnullcartaccount;
        protected string areabusy = string.Empty;
        protected string arealock = string.Empty;


        protected override void ShowPage()
        {
            ShopCart.Clear("0");
            dtcategory = new BLL.category().GetChildList(0, 2);
            //dtcombo = new BookingFood.BLL.bf_good_combo().GetList(" CategoryId=37 order by SortId asc").Tables[0];
            //dtgoods = new BLL.article().GetGoodsList(0, " channel_id!=3 and category_id=37", " sort_id asc").Tables[0];
            BookingFood.BLL.bf_area bllArea = new BookingFood.BLL.bf_area();
            dtParentArea = bllArea.GetList(" IsShow=1 AND ParentId=0 Order By SortId Asc").Tables[0];
            if (dtParentArea.Rows.Count > 0)
            {
                dtArea = bllArea.
                    GetList(" IsShow=1 AND ParentId=" + dtParentArea.Rows[0]["Id"].ToString() + " Order By SortId Asc").Tables[0];
            }
            else
            {
                dtArea = new DataTable();
            }
            Model.siteconfig config = new BLL.siteconfig().loadConfig(Utils.GetXmlMapPath(DTKeys.FILE_SITE_XML_CONFING));
            isnullcartaccount = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(config.IsNullCartAccount, "MD5").ToLower();
            if (this.Context.Request.Cookies["AreaId"] != null)
            {
                BookingFood.Model.bf_area areaModel = bllArea.GetModel(int.Parse(this.Context.Request.Cookies["AreaId"].Value));
                if (areaModel != null)
                {
                    areabusy = areaModel.IsBusy.ToString();
                    arealock = areaModel.IsLock.ToString();
                }
            }
        }
        
        protected string GetComboDetail(string comboid)
        {
            totalprice = 0;
            string rtn = string.Empty;
            List<BookingFood.Model.bf_good_combo_detail> list =
                new BookingFood.BLL.bf_good_combo_detail().GetModelList(" GoodComboId=" + comboid + " Order By SortId Asc");
            BLL.article bllArticle = new BLL.article();
            string dataid = string.Empty;
            decimal dataprice = 0;
            string datatitle = string.Empty;
            foreach (var item in list)
            {
                string sub = string.Empty;
                dataid = string.Empty;
                dataprice = 0;
                datatitle = string.Empty;
                DataSet ds = bllArticle.GetGoodsList(0, " channel_id!=3 and category_id=" + item.BusinessId.ToString(), " sort_id asc");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    sub += "<ul class=\"l_sub_item\">";
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            dataid = ds.Tables[0].Rows[i]["id"].ToString();
                            dataprice = decimal.Parse(ds.Tables[0].Rows[i]["sell_price"].ToString());
                            datatitle = ds.Tables[0].Rows[i]["title"].ToString();
                        }
                        sub += string.Format("<li class=\"info_ii orange\" data-id=\"{1}\" data-price=\"{2}\" onclick=\"SubItemClick(this);\">*{0}</li>"
                            , ds.Tables[0].Rows[i]["title"].ToString(), ds.Tables[0].Rows[i]["id"].ToString(), ds.Tables[0].Rows[i]["sell_price"].ToString());
                    }                    
                    sub += "</ul>";
                }
                rtn += string.Format("<div class=\"info_i orange\" data-id=\"{2}\" data-price=\"{3}\" data-title=\"{4}\" data-type=\"combo\">*{0}{1}</div>"
                    , datatitle, sub, dataid, dataprice, datatitle);
                totalprice += dataprice;
            }
            
            return rtn;
        }

        protected string GetTaste(string taste)
        {
            string rtn = string.Empty;
            if (!string.IsNullOrEmpty(taste))
            {
                string sub = string.Empty;
                string datatitle = string.Empty;
                string[] tastes = taste.Split(',');
                if (tastes.Length > 0)
                {
                    sub += "<ul class=\"l_sub_item\">";
                    foreach (var item in tastes)
                    {
                        if (string.IsNullOrEmpty(datatitle))
                        {
                            datatitle = item;
                        }
                        sub += string.Format("<li class=\"info_ii orange\" data-id=\"{0}\" onclick=\"SubItemClick(this);\">*{0}</li>"
                            , item);
                    }
                    sub += "</ul>";
                }
                rtn += string.Format("<div class=\"info_i orange\" data-id=\"{1}\" data-title=\"{1}\" data-type=\"taste\">{1}{0}</div>", sub, datatitle);
            }
            return rtn;
        }

        protected bool GetIsRun()
        {
            if (DateTime.Now < DateTime.Parse(config.starttime) || DateTime.Now > DateTime.Parse(config.endtime))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

}