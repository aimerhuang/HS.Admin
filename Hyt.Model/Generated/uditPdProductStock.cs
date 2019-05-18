using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hyt.Model.Generated
{
    /// <summary>
    /// 2017-08-14  吴琨   商品库存
    /// </summary>
    public class uditPdProductStock 
    {
        #region 商品信息
        /// <summary>
        /// 商品系统编号
        /// </summary>
        [Description("商品系统编号")]
        public int PrSysNo { get; set; }


        /// <summary>
        /// 商品Erp编号
        /// </summary>
        [Description("商品Erp编号")]
        public string PrErpCode { get; set; }


        /// <summary>
        /// 商品后台显示名称
        /// </summary>
        [Description("商品后台显示名称")]
        public string PrEasName { get; set; }


        /// <summary>
        /// 商品销售计量单位
        /// </summary>
        [Description("商品销售计量单位")]
        public string PrSalesMeasurementUnit { get; set; }
        #endregion

        #region 仓库信息

        /// <summary>
        /// 仓库系统编号
        /// </summary>
        [Description("仓库系统编号")]
        public string WhSysId { get; set; }

        /// <summary>
        /// 仓库ERP编号
        /// </summary>
        [Description("仓库ERP编号")]
        public string WhErpCode { get; set; }

        /// <summary>
        /// 后台仓库名称
        /// </summary>
        [Description("后台仓库名称")]
        public string WhBackWarehouseName { get; set; }

        #endregion


        #region 仓库库存信息

        /// <summary>
        /// 库存数量
        /// </summary>	
        [Description("库存数量")]
        public decimal WhStockQuantity { get; set; }

        /// <summary>
        /// 采购价格
        /// </summary>
        [Description("采购价格")]
        public decimal WhCostPrice { get; set; }

        #endregion


        /// <summary>
        /// 实存数
        /// </summary>
        public decimal ShiQuantity { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        public string BeiZhu { get; set; }
    }
}
