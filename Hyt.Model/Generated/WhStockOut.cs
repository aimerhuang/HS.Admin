
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 
	/// </summary>
    /// <remarks>
    /// 2014-01-17 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class WhStockOut
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 事务编号
		/// </summary>
		[Description("事务编号")]
		public string TransactionSysNo { get; set; }
 		/// <summary>
		/// 仓库编号
		/// </summary>
		[Description("仓库编号")]
		public int WarehouseSysNo { get; set; }
 		/// <summary>
		/// 订单编号
		/// </summary>
		[Description("订单编号")]
		public int OrderSysNO { get; set; }
 		/// <summary>
		/// 收货地址编号
		/// </summary>
		[Description("收货地址编号")]
		public int ReceiveAddressSysNo { get; set; }
 		/// <summary>
		/// 是否到付：是（1）、否（0）
		/// </summary>
		[Description("是否到付：是（1）、否（0）")]
		public int IsCOD { get; set; }
 		/// <summary>
		/// 出库单金额
		/// </summary>
		[Description("出库单金额")]
		public decimal StockOutAmount { get; set; }
 		/// <summary>
		/// 应收金额
		/// </summary>
		[Description("应收金额")]
		public decimal Receivable { get; set; }
 		/// <summary>
		/// 状态：待出库（10）、待拣货（20）、待打包（30）、待
		/// </summary>
		[Description("状态：待出库（10）、待拣货（20）、待打包（30）、待")]
		public int Status { get; set; }
 		/// <summary>
		/// 签收时间
		/// </summary>
		[Description("签收时间")]
		public DateTime SignTime { get; set; }
 		/// <summary>
		/// 是否已经打印包裹单：是（1）、否（0）
		/// </summary>
		[Description("是否已经打印包裹单：是（1）、否（0）")]
		public int IsPrintedPackageCover { get; set; }
 		/// <summary>
		/// 是否已经打印拣货单：是（1）、否（0）
		/// </summary>
		[Description("是否已经打印拣货单：是（1）、否（0）")]
		public int IsPrintedPickupCover { get; set; }
 		/// <summary>
		/// 客户留言
		/// </summary>
		[Description("客户留言")]
		public string CustomerMessage { get; set; }
 		/// <summary>
		/// 自提时间
		/// </summary>
		[Description("自提时间")]
		public DateTime PickUpDate { get; set; }
 		/// <summary>
		/// 配送备注
		/// </summary>
		[Description("配送备注")]
		public string DeliveryRemarks { get; set; }
 		/// <summary>
		/// 配送时间段
		/// </summary>
		[Description("配送时间段")]
		public string DeliveryTime { get; set; }
 		/// <summary>
		/// 配送前是否联系：是（1）、否（0）
		/// </summary>
		[Description("配送前是否联系：是（1）、否（0）")]
		public int ContactBeforeDelivery { get; set; }
 		/// <summary>
		/// 发票系统编号
		/// </summary>
		[Description("发票系统编号")]
		public int InvoiceSysNo { get; set; }
 		/// <summary>
		/// 配送方式
		/// </summary>
		[Description("配送方式")]
		public int DeliveryTypeSysNo { get; set; }
 		/// <summary>
		/// 签收图片
		/// </summary>
		[Description("签收图片")]
		public string SignImage { get; set; }
 		/// <summary>
		/// 备注
		/// </summary>
		[Description("备注")]
		public string Remarks { get; set; }
 		/// <summary>
		/// 创建人
		/// </summary>
		[Description("创建人")]
		public int CreatedBy { get; set; }
 		/// <summary>
		/// 创建时间
		/// </summary>
		[Description("创建时间")]
		public DateTime CreatedDate { get; set; }
 		/// <summary>
		/// 出库人
		/// </summary>
		[Description("出库人")]
		public int StockOutBy { get; set; }
 		/// <summary>
		/// 出库时间
		/// </summary>
		[Description("出库时间")]
		public DateTime StockOutDate { get; set; }
 		/// <summary>
		/// 最后更新人
		/// </summary>
		[Description("最后更新人")]
		public int LastUpdateBy { get; set; }
 		/// <summary>
		/// 最后更新时间
		/// </summary>
		[Description("最后更新时间")]
		public DateTime LastUpdateDate { get; set; }
 		/// <summary>
		/// 时间戳
		/// </summary>
		[Description("时间戳")]
        public DateTime Stamp { get; set; }
 	}
}

	