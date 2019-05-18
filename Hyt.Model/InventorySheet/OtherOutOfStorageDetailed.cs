using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hyt.Model.InventorySheet
{
    /// <summary>
    /// 其他出入库商品明细
    /// </summary>
    public class OtherOutOfStorageDetailed
    {
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int? SysNo { get; set; }


        /// 其他出入库关联系统编号
        /// </summary>
        [Description("其他出入库关联系统编号")]
        public int? OtherOutOfStorageCode { get; set; }

        /// 商品系统编号
        /// </summary>
        [Description("商品系统编号")]
        public int ProductSysNo { get; set; }

        /// 商品代码
        /// </summary>
        [Description("商品代码")]
        public string ProductCode { get; set; }


        /// 商品条形码
        /// </summary>
        [Description("商品条形码")]
        public string BarCode { get; set; }

        /// 商品名称
        /// </summary>
        [Description("商品名称")]
        public string ProductName { get; set; }


        /// 实收数量
        /// </summary>
        [Description("实收数量")]
        public decimal Count { get; set; }

        /// 单价
        /// </summary>
        [Description("单价")]
        public decimal UnitPrice { get; set; }


        /// 金额
        /// </summary>
        [Description("金额")]
        public decimal Price { get; set; }

        /// 备注
        /// </summary>
        [Description("备注")]
        public string Remarks { get; set; }

        /// 收货仓库系统编号
        /// </summary>
        [Description("收货仓库系统编号")]
        public int CollectWarehouseSysNo { get; set; }


        /// 收货仓库名称
        /// </summary>
        [Description("收货仓库名称")]
        public string CollectWarehouseName { get; set; }


        #region 扩展属性
        /// 仓库Erp编号
        /// </summary>
        [Description("仓库Erp编号")]
        public string CollectWarehouseCode { get; set; }


        /// 帐存数量
        /// </summary>
        [Description("帐存数量")]
        public decimal ZhangCount { get; set; }
        #endregion
    }
}
