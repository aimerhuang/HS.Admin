using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.B2CApp
{
    /// <summary>
    /// 仓库
    /// </summary>
    /// <remarks>2013-7-22 杨浩 添加</remarks>
    public class Warehouse
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WarehouseName { get; set; }

        /// <summary>
        /// 地区名
        /// </summary>
        public string AreaName { get; set; }

        /// <summary>
        /// 街道地址
        /// </summary>
        public string StreetAddress { get; set; }

        /// <summary>
        /// 负责人联系电话
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string Contact { get; set; }

    }

    /// <summary>
    /// 仓库
    /// </summary>
    /// <remarks>2013-7-22 杨浩 添加</remarks>
    public class NWarehouse : Warehouse
    {
        /// <summary>
        /// 纬度
        /// </summary>
        public double Latitude { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public double Longitude { get; set; }
        /// <summary>
        /// 距离(km)
        /// </summary>
        public double Distance { get; set; }
    }

    #region 配送方式

    /// <summary>
    /// 配送方式
    /// </summary>
    /// <remarks>2013-7-22 杨浩 添加</remarks>
    public class DeliveryType
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 配送方式名
        /// </summary>
        public string DeliveryTypeName { get; set; }

        /// <summary>
        /// 配送方式
        /// </summary>
        public int Type { get; set; }

    }

    /// <summary>
    /// 配送方式项
    /// </summary>
    /// <remarks>2013-7-22 杨浩 添加</remarks>
    public class DeliveryItem
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
    }

    #endregion
}
