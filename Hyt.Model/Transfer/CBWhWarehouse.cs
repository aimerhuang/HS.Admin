using System;
using System.Collections.Generic;

namespace Hyt.Model
{
    /// <summary>
    /// 仓库扩展实体
    /// </summary>
    /// <remarks> 2013-06-18 周瑜 创建</remarks>
    /// <remarks> 2013-08-28 邵斌 追加：仓库支持取货方式</remarks>
    /// <remarks> 2013-09-11 郑荣华 追加：与某坐标距离</remarks>
    [Serializable]
    public class CBWhWarehouse : WhWarehouse
    {
        /// <summary>
        /// 区名称
        /// </summary>
        public string AreaName{ get; set; }

        /// <summary>
        /// 市编号
        /// </summary>
        public int CitySysNo { get; set; }

        /// <summary>
        /// 市名称
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        /// 省编号
        /// </summary>
        public int ProvinceSysNo { get; set; }

        /// <summary>
        /// 省名称
        /// </summary>
        public string ProvinceName { get; set; }

        /// <summary>
        /// 是否是默认仓库
        /// </summary>
        public int IsDefault { get; set; }

        /// <summary>
        /// 仓库提货方式
        /// </summary>
        public IList<int> PickUpType { get; set; }

        /// <summary>
        /// 与某坐标距离(km)
        /// </summary>
        public double Distance { get; set; }

        /// <summary>
        /// 仓库库存数量
        /// </summary>
        public decimal SumStockQuantity { get; set; }
    }
}