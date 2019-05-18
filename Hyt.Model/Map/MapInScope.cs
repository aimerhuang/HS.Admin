using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Map
{
    /// <summary>
    /// 查询
    /// </summary>
    public class MapBaiChengScope
    {
        /// <summary>
        /// 是否在配送范围
        /// </summary>
        public bool IsInScope { get; set; }

        /// <summary>
        /// 默认配送仓库编号 
        /// </summary>
        public int WarehouseNo { get; set; }

        /// <summary>
        /// 默认仓库名称
        /// </summary>
        public string WarehouseName { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public double Lat { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public double Lng { get; set; }

        /// <summary>
        /// 高德地图
        /// </summary>
        public MapBaiChengScope Gd { get; set; }
    }
}
