using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hyt.Model.InventorySheet
{
    /// <summary>
    /// 盘点报告单商品详情
    /// </summary>
    public class WhIReporPrDetails
    {
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }


        /// 盘点报告单系统编号
        /// </summary>
        [Description("盘点报告单系统编号")]
        public int WhInventoryReporSysNo { get; set; }


        /// 商品代码
        /// </summary>
        [Description("商品代码")]
        public string ProduceCode { get; set; }


        /// 商品名称
        /// </summary>
        [Description("商品名称")]
        public string ProductName { get; set; }

        /// 规格型号
        /// </summary>
        [Description("规格型号")]
        public string Specification { get; set; }

        /// 辅助属性
        /// </summary>
        [Description("辅助属性")]
        public string AuxiliaryAttribute { get; set; }

        /// 批号
        /// </summary>
        [Description("批号")]
        public string BatchNumber { get; set; }

        /// 单位
        /// </summary>
        [Description("单位")]
        public string  Unit { get; set; }

        /// 账存数量
        /// </summary>
        [Description("账存数量")]
        public decimal ADQuantity { get; set; }

        /// 实存数量
        /// </summary>
        [Description("实存数量")]
        public decimal RealityQuantity { get; set; }

        /// 计划单价
        /// </summary>
        [Description("计划单价")]
        public decimal PlanPrice { get; set; }

        /// 单价
        /// </summary>
        [Description("单价")]
        public decimal UnitPrice { get; set; }

        /// 盈亏数量
        /// </summary>
        [Description("盈亏数量")]
        public decimal ProfitAndLoss { get; set; }

        /// 盈亏金额
        /// </summary>
        [Description("盈亏金额")]
        public decimal ProfitAndLossPrice { get; set; }

        /// 备注
        /// </summary>
        [Description("备注")]
        public string Remark { get; set; }


        /// 生产/采购日期
        /// </summary>
        [Description("生产/采购日期")]
        public DateTime? ProcurementTime { get; set; }

        /// 保质期/天
        /// </summary>
        [Description("保质期/天")]
        public int ShelfLife { get; set; }


        /// 有效期至
        /// </summary>
        [Description("有效期至")]
        public DateTime? ValidityTime { get; set; }


        /// 仓库名称
        /// </summary>
        [Description("仓库名称")]
        public string WarehouseName { get; set; }


        /// 仓库系统编号
        /// </summary>
        [Description("仓库系统编号")]
        public int WarehouseSysNo { get; set; }


        /// 仓位
        /// </summary>
        [Description("仓位")]
        public string WarehouseLocation { get; set; }
    }
}
