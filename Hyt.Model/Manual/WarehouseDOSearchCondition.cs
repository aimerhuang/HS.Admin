using System;

namespace Hyt.Model
{
    /// <summary>
    /// 出库单搜索条件类
    /// </summary>
    /// <remarks>2013-06-13 周瑜 创建</remarks>
    public class WarehouseDOSearchCondition
    {
        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 出库单系统编号
        /// </summary>
        public int StockOutSysNo { get; set; }

        /// <summary>
        /// 仓库系统编号
        /// </summary>
        public int WarehosueSysNo { get; set; }

        /// <summary>
        /// 商品系统编号
        /// </summary>
        public int ProductSysNo { get; set; }

        /// <summary>
        /// 订单系统编号
        /// </summary>
        public int SoSysNo { get; set; }

        /// <summary>
        /// 是否打印发票 0:所有 1：需要发票 2：不需要发票//todo: 假定
        /// </summary>
        public int IsInvoice { get; set; }

        /// <summary>
        /// 地区系统编号
        /// </summary>
        public int AreaSysNo { get; set; }

    }
}
