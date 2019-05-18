using System;

namespace Hyt.Util
{
    /// <summary>
    /// 地图计算
    /// </summary>
    /// <remarks>
    /// 2013-09-11 郑荣华 创建
    /// </remarks>
    public class MapUtil
    {
        //地球赤道上环绕地球一周走一圈共40075.04公里,而@一圈分成360°,而每1°(度)有60,每一度一秒在赤道上的长度计算如下： 
        //40075.04km/360°=111.31955km 
        //111.31955km/60=1.8553258km=1855.3m 
        //而每一分又有60秒,每一秒就代表1855.3m/60=30.92m 
        //任意两点距离计算公式为 
        //d＝111.12cos{1/[sinΦAsinΦB十cosΦAcosΦBcos(λB—λA)]} 
        //其中A点经度，纬度分别为λA和ΦA，B点的经度、纬度分别为λB和ΦB，d为距离。

        private const double EarthRadius = 6378.137; //地球半径

        /// <summary>
        /// 计算经纬度
        /// </summary>
        /// <param name="d">纬度</param>
        /// <returns></returns>
        /// <remarks>2013-09-11 郑荣华 创建</remarks>
        private static double Rad(double d)
        {
            return d * Math.PI / 180.0;
        }
        /// <summary>
        /// 计算两个坐标间距离
        /// </summary>
        /// <param name="lat1">1纬度</param>
        /// <param name="lng1">1经度</param>
        /// <param name="lat2">2纬度</param>
        /// <param name="lng2">2经度</param>
        /// <returns>坐标间距离（km）</returns>
        /// <remarks>2013-09-11 郑荣华 创建</remarks>
        public static double GetDistance(double lat1, double lng1, double lat2, double lng2)
        {
            double radLat1 = Rad(lat1);
            double radLat2 = Rad(lat2);
            double a = radLat1 - radLat2;
            double b = Rad(lng1) - Rad(lng2);
            double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) +
             Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
            s = s * EarthRadius;
            s = Math.Round(s * 10000) / 10000;
            return s;
        }
    }
}
