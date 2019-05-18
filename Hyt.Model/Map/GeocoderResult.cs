using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
namespace Hyt.Model.Map
{
    /// <summary>
    /// 地理位置解析结果实体
    /// </summary>
    /// <remarks>2013-08-28 黄波 创建</remarks>
    public class GeocoderResult
    {
        /// <summary>
        /// 返回结果状态值
        /// 成功返回0，其他值请查看
        /// http://developer.baidu.com/map/webservice-geocoding.htm#.E6.8E.A5.E5.8F.A3.E7.A4.BA.E4.BE.8A
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 经度值
        /// </summary>
        public double Lng { get; set; }
        /// <summary>
        /// 纬度值
        /// </summary>
        public double Lat { get; set; }
        /// <summary>
        /// 位置的附加信息，是否精确查找。1为精确查找，0为不精确。
        /// </summary>
        public int Precise { get; set; }
        /// <summary>
        /// 可信度
        /// </summary>
        public int Confidence { get; set; }
        /// <summary>
        /// 地址类型
        /// </summary>
        public string Level { get; set; }
    }

    /// <summary>
    /// 百度地图
    /// </summary>
    /// <remarks>2013-08-28 黄波 创建</remarks>
    public class MapRef
    {
        /// <summary>
        /// 百度地图地址
        /// </summary>
        public static string MapRefAddress
        {
            get { return "http://api.map.baidu.com/api?v=2.0&ak=" + ConfigurationManager.AppSettings["BaiduMapKey"]; }
        }
        /// <summary>
        /// 百度地图Key
        /// </summary>
        public static string MapRefKey
        {
            get { return ConfigurationManager.AppSettings["BaiduMapKey"]; }
        }
    }

}
