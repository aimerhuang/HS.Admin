
using System;
using System.Collections.Generic;

namespace Hyt.Model.Transfer
{
    /// <summary>
	/// 生成结算单
	/// </summary>
    /// <remarks>
    /// 2013-06-26 黄伟 创建
    /// </remarks>
	public class CBLgSettlement
    {
		/// <summary>
		/// 配送员编号
		/// </summary>
		public int DeliveryUserSysNo { get; set; }

		/// <summary>
		/// 创建人
		/// </summary>
		public int CreatedBy { get; set; }

        /// <summary>
        /// 结算单系统编号
        /// </summary>
        public int SysNo { get; set; }

        /// <summary>
        /// 仓库系统编号
        /// </summary>
        public int WareHouseSysNo { get; set; }

        /// <summary>
        /// 结算单状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 应缴金额
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 实缴金额
        /// </summary>
        public decimal PaidAmount { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// 出库单
        /// </summary>
        public IList<CBWhStockOut> WhStockOuts { get; set; }

        /// <summary>
        /// 出库单明细
        /// </summary>
        public IList<WhStockOutItem> WhStockOutItems { get; set; }

        /// <summary>
        /// 出库单
        /// </summary>
        public IList<LgPickUp> LgPickUps { get; set; }

        /// <summary>
        /// 配送单明细-1:1出库单
        /// </summary>
        public IList<LgDeliveryItem> LgDeliveryItems { get; set; }

        /// <summary>
        /// 配送方式
        /// </summary>
        public IList<BsPaymentType> BsPaymentTypes { get; set; }

        /// <summary>
        /// 配送单-1:n出库单
        /// </summary>
        public IList<LgDelivery> LgDeliverys { get; set; }

    }
}

	