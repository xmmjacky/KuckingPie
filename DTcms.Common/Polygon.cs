using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DTcms.Common
{
    public static class Polygon
    {
        public static bool GetResult(string point, string area)
        {
            //var result = false;
            try
            {

           
            if (point.Split(',').Length != 2) return false;
            MyPoint position = new MyPoint();
            decimal x = 0, y = 0;
            if (!decimal.TryParse(point.Split(',')[0], out x)) return false;
            if (!decimal.TryParse(point.Split(',')[1], out y)) return false;
            position.X = x;
            position.Y = y;
            string[] areas = area.Split('|');
            MyPoint[] poly = new MyPoint[areas.Length];
            for (int i = 0; i < areas.Length; i++)
            {
                    try
                    {
                        poly[i] = new MyPoint(decimal.Parse(areas[i].Split(',')[0]), decimal.Parse(areas[i].Split(',')[1]));

                    }
                    catch (Exception ex)
                    {

                    }
            }
            return MyPointInFences(position, poly);
            }
            catch(Exception ex)
            {
                Log.Info("定位异常:" + ex.Message);
                return false;
            }
            //for (int i = 0; i < areas.Length; i++)
            //{
            //    result = GetDistance(Convert.ToDouble(x), Convert.ToDouble(y), Convert.ToDouble(areas[i].Split(',')[0]), Convert.ToDouble(areas[i].Split(',')[1])) <= 0.6;
            //    if (result) return true;
            //}
            //return result;
        }
        private static decimal isLeft(MyPoint P0, MyPoint P1, MyPoint P2)
        {
            decimal abc = ((P1.X - P0.X) * (P2.Y - P0.Y) - (P2.X - P0.X) * (P1.Y - P0.Y));
            return abc;
        }
        private static bool MyPointInFences(MyPoint pnt1, MyPoint[] fencePnts)
        {
            int wn = 0, j = 0; //wn 计数器 j第二个点指针
            for (int i = 0; i < fencePnts.Length; i++)
            {//开始循环
                if (i == fencePnts.Length - 1)
                    j = 0;//如果 循环到最后一点 第二个指针指向第一点
                else
                    j = j + 1; //如果不是 ，则找下一点


                if (fencePnts[i].Y <= pnt1.Y) // 如果多边形的点 小于等于 选定点的 Y 坐标
                {
                    if (fencePnts[j].Y > pnt1.Y) // 如果多边形的下一点 大于于 选定点的 Y 坐标
                    {
                        if (isLeft(fencePnts[i], fencePnts[j], pnt1) > 0)
                        {
                            wn++;
                        }
                    }
                }
                else
                {
                    if (fencePnts[j].Y <= pnt1.Y)
                    {
                        if (isLeft(fencePnts[i], fencePnts[j], pnt1) < 0)
                        {
                            wn--;
                        }
                    }
                }
            }
            if (wn == 0)
                return false;
            else
                return true;
        }

        private class MyPoint
        {
            public decimal X { get; set; }
            public decimal Y { get; set; }
            public MyPoint() { }
            public MyPoint(decimal x, decimal y)
            {
                X = x;
                Y = y;
            }
        }

        public static double GetDistance(double lat1, double lng1, double lat2, double lng2)
        {
            //地球半径，单位米
            double EARTH_RADIUS = 6378137;
            double radLat1 = Rad(lat1);
            double radLng1 = Rad(lng1);
            double radLat2 = Rad(lat2);
            double radLng2 = Rad(lng2);
            double a = radLat1 - radLat2;
            double b = radLng1 - radLng2;
            double result = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) + Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2))) * EARTH_RADIUS;
            return result / 1000;
        }
        /// <summary>
        /// 经纬度转化成弧度
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        private static double Rad(double d)
        {
            return (double)d * Math.PI / 180d;
        }
    }
}
