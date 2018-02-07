using BookingFood.BLL;
using DTcms.BLL;
using DTcms.Common;
using DTcms.Web.tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuckingPieTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Test();
        }


            static void Test()
            {
                string position = "31.229018,121.470183";
                string openid = "omc8At4ULCBtNZxm9aogl7aBLCEs";
               
               bf_area bllArea = new BookingFood.BLL.bf_area();
                string prevAreaId = string.Empty;
                if (!string.IsNullOrEmpty(openid))
                {
                    orders bll = new orders();
                    DataTable dt = bll.GetList(1, " user_name='" + openid + "' And takeout in (1,2)", " id desc").Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["takeout"].ToString() == "2")
                        {
                            BookingFood.Model.bf_area areaModel = bllArea.GetModelList("OppositeId=" + dt.Rows[0]["area_id"].ToString())[0];
                            prevAreaId = areaModel.Id.ToString();
                        }
                        else
                        {
                            prevAreaId = dt.Rows[0]["area_id"].ToString();
                        }

                    }
                }
                //Log.Info("ip:" + Common.DTRequest.GetIP() + " " + position + " " + openid);

                DataTable dtArea = bllArea.
                        GetList(" IsShow=0 AND IsLock=0 AND ParentId=1 Order By SortId Asc").Tables[0];
                string rtn = "{\"Status\":0}";
                foreach (DataRow item in dtArea.Rows)
                {
                    if (string.IsNullOrEmpty(item["DistributionArea"].ToString().Trim())) continue;
                    bool isInArea = Polygon.GetResult(position, item["DistributionArea"].ToString());
                    if (isInArea)
                    {
                        if (string.IsNullOrEmpty(prevAreaId) || prevAreaId != item["Id"].ToString())
                        {
                            rtn = "{\"Status\":1,\"Id\":" + item["Id"].ToString() + ",\"Title\":\"" + item["Title"].ToString()
                                + "\",\"ShowConfirm\":1,\"Address\":\"" + item["Address"].ToString() + "\"}";
                        }
                        else
                        {
                            rtn = "{\"Status\":1,\"Id\":" + item["Id"].ToString() + ",\"Title\":\"" + item["Title"].ToString() + "\",\"ShowConfirm\":0}";
                        }

                        break;
                    }
                }

            }
        
    }
}
