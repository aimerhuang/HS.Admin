using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 根据坐标获取区域仓库回传对象
    /// </summary>
    /// <remarks>2016-3-26 杨浩 创建</remarks>
    [Serializable]
    public class CBWarehouseArea
    {
        /// <summary>
        /// 仓库编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WarehouseName { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public string Latitude{ get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public string Longitude { get; set; }
        /// <summary>
        /// 地区名称
        /// </summary>

        public string AreaName { get; set; }
        /// <summary>
        /// 地区编号
        /// </summary>
        public int AreaId { get; set; }
        /// <summary>
        /// 城市名称
        /// </summary>
        public string CityName { get; set; }
        /// <summary>
        /// 城市系统编号
        /// </summary>
        public int CityId { get; set; }
        /// <summary>
        /// 地区行政代码
        /// </summary>
        public string AreaCode { get; set; }
        /// <summary>
        /// 经销商编号
        /// </summary>
        public int DealerSysNo { get; set; }
    }
}
