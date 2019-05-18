using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 仓库当日达扩展类
    /// </summary>
    /// <remarks>2014-10-9  朱成果 创建</remarks>
    public class CBWhDeliveryScope : WhDeliveryScope
    {
        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WarehouseName { get; set; } 

        /// <summary>
        /// 地区编号
        /// </summary>
        public int AreaNo { get; set; }
        /// <summary>
        /// 地区名称
        /// </summary>
        public string  AreaName { get; set; }

        /// <summary>
        /// 城市编号
        /// </summary>
        public int CityNo { get; set; }

        /// <summary>
        /// 城市名称
        /// </summary>
        public string CityName { get; set; }
    }
}
