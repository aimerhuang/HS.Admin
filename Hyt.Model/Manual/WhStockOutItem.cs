using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model
{

    /// <summary>
    /// </summary>
    /// <remarks>
    /// 2013/7/16 何方 创建
    /// </remarks>
   public partial  class WhStockOutItem
    {

        #region 扩展属性
        /// <summary>
        /// 该商品是否已被扫描
        /// </summary>
        public bool IsScaned { get; set; }

        /// <summary>
        /// 已扫描的商品数量
        /// </summary>
        public int ScanedQuantity { get; set; }
        /// <summary>
        /// 产品Erp编码
        /// </summary>
        public string ProductErpCode { get;set;}
    
        /// <summary>
        /// 仓库Erp编码
        /// </summary>
        public string WarehouseErpCode { get; set; }
        #endregion
    }
}
