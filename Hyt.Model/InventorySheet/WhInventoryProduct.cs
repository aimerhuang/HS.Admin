using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hyt.Model.InventorySheet
{
    /// <summary>
    /// 盘点商品详情
    /// </summary>
    public class WhInventoryProduct 
    {
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }

        /// 盘点单系统编号
        /// </summary>
        [Description("盘点单系统编号")]
        public int InventorySysNo { get; set; }

        /// 盘点商品仓库系统编号
        /// </summary>
        [Description("盘点商品仓库系统编号")]
        public int WarehouseSysNo { get; set; }

        /// <summary>
        /// 盘点商品系统编号
        /// </summary>
        public int ProductSysNo { get; set; }

        /// 账存数量
        /// </summary>
        [Description("账存数量")]
        public decimal ZhangCunQuantity { get; set; }


        /// 盘点数量
        /// </summary>
        [Description("盘点数量")]
        public decimal InventoryQuantity { get; set; }


        /// 调整数量
        /// </summary>
        [Description("调整数量")]
        public decimal adjustmenQuantity { get; set; }


        /// 盈亏数量
        /// </summary>
        [Description("盈亏数量")]
        public decimal Quantity { get; set; }


        /// 盈亏状态
        /// </summary>
        [Description("盈亏状态")]
        public int Status { get; set; }


        /// 备注
        /// </summary>
        [Description("备注")]
        public string Remarks { get; set; }
    }



    /// <summary>
    /// 盘点商品详情(包含商品表)
    /// </summary>
    public class WhInventoryProductDetail : PdProduct
    {
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }

        /// 盘点单系统编号
        /// </summary>
        [Description("盘点单系统编号")]
        public int InventorySysNo { get; set; }

        /// 盘点商品仓库系统编号
        /// </summary>
        [Description("盘点商品仓库系统编号")]
        public int WarehouseSysNo { get; set; }

        /// <summary>
        /// 盘点商品系统编号
        /// </summary>
        public int ProductSysNo { get; set; }


        /// 盘点数量
        /// </summary>
        [Description("盘点数量")]
        public decimal InventoryQuantity { get; set; }

        /// 账存数量
        /// </summary>
        [Description("账存数量")]
        public decimal ZhangCunQuantity { get; set; }



        /// 调整数量
        /// </summary>
        [Description("调整数量")]
        public decimal adjustmenQuantity { get; set; }


        /// 盈亏数量
        /// </summary>
        [Description("盈亏数量")]
        public int Quantity { get; set; }


        /// 盈亏状态
        /// </summary>
        [Description("盈亏状态")]
        public int Status { get; set; }


        /// 备注
        /// </summary>
        [Description("备注")]
        public string Remarks { get; set; }


        #region 扩展属性

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WarehouseNameDate { get; set; }

        /// <summary>
        /// 品牌名称
        /// </summary>
        public string BrandName { get; set; }

        #endregion
    }

    /// <summary>
    /// 盘点单导出Excel所用实体
    /// </summary>
    public class WhInventoryProductDetailOutput {

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WarehouseNameDate { get; set; }


        /// <summary>
        /// 商品编号
        /// </summary>
        [Description("商品编号")]
        public string PrCode { get; set; }


        /// <summary>
        /// 商品名称
        /// </summary>
        [Description("商品名称")]
        public string PrName { get; set; }

        /// 账存数量
        /// </summary>
        [Description("账存数量")]
        public decimal ZhangCunQuantity { get; set; }


        /// 盘点数量
        /// </summary>
        [Description("盘点数量")]
        public decimal InventoryQuantity { get; set; }

        /// 备注
        /// </summary>
        [Description("备注")]
        public string Remarks { get; set; }
    }


}
