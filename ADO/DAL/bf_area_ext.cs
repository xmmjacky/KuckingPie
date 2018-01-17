using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Maticsoft.DBUtility;//Please add references
namespace BookingFood.DAL
{
	/// <summary>
	/// 数据访问类:bf_area
	/// </summary>
	public partial class bf_area
	{
        public DataTable GetListForList(int parent_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from bf_area");
            strSql.Append(" order by SortId asc,Id desc");
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            DataTable oldData = ds.Tables[0] as DataTable;
            if (oldData == null)
            {
                return null;
            }
            //复制结构
            DataTable newData = oldData.Clone();
            //调用迭代组合成DAGATABLE
            GetChilds(oldData, newData, parent_id);
            return newData;
        }

        private void GetChilds(DataTable oldData, DataTable newData, int parent_id)
        {
            DataRow[] dr = oldData.Select("ParentId=" + parent_id);
            for (int i = 0; i < dr.Length; i++)
            {
                //添加一行数据
                DataRow row = newData.NewRow();
                row["Id"] = int.Parse(dr[i]["Id"].ToString());
                row["Title"] = dr[i]["Title"].ToString();
                row["SortId"] = int.Parse(dr[i]["SortId"].ToString());
                row["ParentId"] = int.Parse(dr[i]["ParentId"].ToString());
                row["ParentTitle"] = dr[i]["ParentTitle"].ToString();
                row["ManagerId"] = int.Parse(dr[i]["ManagerId"].ToString());
                row["ManagerName"] = dr[i]["ManagerName"].ToString();
                row["Description"] = dr[i]["Description"].ToString();
                row["IsBusy"] = dr[i]["IsBusy"].ToString();
                row["IsLock"] = dr[i]["IsLock"].ToString();
                row["IsShow"] = dr[i]["IsShow"].ToString();
                newData.Rows.Add(row);
                //调用自身迭代
                this.GetChilds(oldData, newData, int.Parse(dr[i]["id"].ToString()));
            }
        }

        public DataSet GetListByPositionByTransform(int top, string lng, string lat, string strwhere)
        {

            StringBuilder strSql = new StringBuilder();
            double _lng = double.Parse(lng);
            double _lat = double.Parse(lat);
            double newLng, newLat;
            EvilTransform.transform(_lat, _lng, out newLat, out newLng);
            strSql.Append("SELECT TOP " + top.ToString() + " * FROM ( ");
            strSql.Append("SELECT bc.*, ABS(bc.lng - " + newLng.ToString() + ") AS X ,ABS(bc.lat-" + newLat.ToString() + ") AS Y  ");
            //strSql.Append(", SQRT(ABS(bc.lng - " + newLng.ToString() + ")*ABS(bc.lng - " + newLng.ToString() + ")*COS(" + newLat.ToString() + ")*111712.69*111712.69 + ABS(bc.lat-" + newLat.ToString() + ")*ABS(bc.lat-" + newLat.ToString() + ")*111712.69*111712.69) AS calcdistance ");
            strSql.Append(", ACOS((SIN(bc.lat*3.14159265/180)*SIN(" + newLat.ToString() + "*3.14159265/180))+(COS(bc.lat*3.14159265/180)*COS(" + newLat.ToString() + "*3.14159265/180)*COS(bc.lng*3.14159265/180-" + newLng.ToString() + "*3.14159265/180)))*180/3.14159265*111189.57696 AS calcdistance ");
            strSql.Append("FROM bf_area bc ");
            strSql.Append("WHERE bc.lng IS NOT NULL ");
            strSql.Append(") AS temp ");
            strSql.Append("Where 1=1 " + strwhere);
            strSql.Append("ORDER BY temp.calcdistance ASC  ");

            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            return ds;
        }

        public class EvilTransform
        {
            const double pi = 3.14159265358979324;
            // 
            // Krasovsky 1940 // // a = 6378245.0, 1/f = 298.3 // b = a * (1 - f) // ee = (a^2 - b^2) / a^2; 
            const double a = 6378245.0;
            const double ee = 0.00669342162296594323;
            // 
            // World Geodetic System ==> Mars Geodetic System 
            public static void transform(double wgLat, double wgLon, out double mgLat, out double mgLon)
            {
                if (outOfChina(wgLat, wgLon))
                {
                    mgLat = wgLat;
                    mgLon = wgLon;
                    return;
                }
                double dLat = transformLat(wgLon - 105.0, wgLat - 35.0);
                double dLon = transformLon(wgLon - 105.0, wgLat - 35.0);
                double radLat = wgLat / 180.0 * pi;
                double magic = Math.Sin(radLat);
                magic = 1 - ee * magic * magic;
                double sqrtMagic = Math.Sqrt(magic);
                dLat = (dLat * 180.0) / ((a * (1 - ee)) / (magic * sqrtMagic) * pi);
                dLon = (dLon * 180.0) / (a / sqrtMagic * Math.Cos(radLat) * pi);
                mgLat = wgLat + dLat; mgLon = wgLon + dLon;
            }
            static bool outOfChina(double lat, double lon)
            {
                if (lon < 72.004 || lon > 137.8347) return true;
                if (lat < 0.8293 || lat > 55.8271) return true;
                return false;
            }
            static double transformLat(double x, double y)
            {
                double ret = -100.0 + 2.0 * x + 3.0 * y + 0.2 * y * y + 0.1 * x * y + 0.2 * Math.Sqrt(Math.Abs(x));
                ret += (20.0 * Math.Sin(6.0 * x * pi) + 20.0 * Math.Sin(2.0 * x * pi)) * 2.0 / 3.0;
                ret += (20.0 * Math.Sin(y * pi) + 40.0 * Math.Sin(y / 3.0 * pi)) * 2.0 / 3.0;
                ret += (160.0 * Math.Sin(y / 12.0 * pi) + 320 * Math.Sin(y * pi / 30.0)) * 2.0 / 3.0; return ret;
            }
            static double transformLon(double x, double y)
            {
                double ret = 300.0 + x + 2.0 * y + 0.1 * x * x + 0.1 * x * y + 0.1 * Math.Sqrt(Math.Abs(x));
                ret += (20.0 * Math.Sin(6.0 * x * pi) + 20.0 * Math.Sin(2.0 * x * pi)) * 2.0 / 3.0;
                ret += (20.0 * Math.Sin(x * pi) + 40.0 * Math.Sin(x / 3.0 * pi)) * 2.0 / 3.0;
                ret += (150.0 * Math.Sin(x / 12.0 * pi) + 300.0 * Math.Sin(x / 30.0 * pi)) * 2.0 / 3.0;
                return ret;
            }
        }
	}
}

