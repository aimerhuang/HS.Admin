using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 百度地图构建对象
    /// </summary>
    /// <remarks>2014-05-29 朱成果 创建</remarks>
    public   class CBBaiduMap
    {
        /// <summary>
        /// 城市编号
        /// </summary>
        public int? CityNo { get; set; }

        /// <summary>
        /// 城市名称
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        /// 当前地点精度
        /// </summary>
        public double? LocalX { get; set; }

        /// <summary>
        /// 当前地点纬度
        /// </summary>
        public double? LocalY { get; set; }

        /// <summary>
        /// 当前地点名称
        /// </summary>
        public string LocalAddress { get; set; }

        /// <summary>
        /// 当前城市百城当日达配送范围
        /// </summary>
        public IList<LgDeliveryScope> CityDeliveryScope { get; set; }
    }
}
