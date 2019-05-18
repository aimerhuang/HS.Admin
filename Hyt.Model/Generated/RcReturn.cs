
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 
	/// </summary>
    /// <remarks>
    /// 2013-10-28 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class RcReturn
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
		/// 退换货单据号
		/// </summary>
		[Description("退换货单据号")]
		public string RmaId { get; set; }
 		/// <summary>
		/// 销售单系统编号
		/// </summary>
		[Description("销售单系统编号")]
		public int OrderSysNo { get; set; }
 		/// <summary>
		/// 客户系统编号
		/// </summary>
		[Description("客户系统编号")]
		public int CustomerSysNo { get; set; }
 		/// <summary>
		/// 申请单来源:客户(10),客服(20),门店(30),部分签收(40)
		/// </summary>
		[Description("申请单来源:客户(10),客服(20),门店(30),部分签收(40)")]
		public int Source { get; set; }
 		/// <summary>
		/// 处理部门:客服中心(10),门店(20)
		/// </summary>
		[Description("处理部门:客服中心(10),门店(20)")]
		public int HandleDepartment { get; set; }
 		/// <summary>
		/// 仓库编号
		/// </summary>
		[Description("仓库编号")]
		public int WarehouseSysNo { get; set; }
 		/// <summary>
		/// 取件地址编号
		/// </summary>
		[Description("取件地址编号")]
		public int PickUpAddressSysNo { get; set; }
 		/// <summary>
		/// 收货地址编号
		/// </summary>
		[Description("收货地址编号")]
		public int ReceiveAddressSysNo { get; set; }
 		/// <summary>
		/// RMA类型:售后换货(10),售后退货(20)
		/// </summary>
		[Description("RMA类型:售后换货(10),售后退货(20)")]
		public int RmaType { get; set; }
 		/// <summary>
		/// 取件方式系统编号
		/// </summary>
		[Description("取件方式系统编号")]
		public int PickupTypeSysNo { get; set; }
 		/// <summary>
		/// 取件时间段
		/// </summary>
		[Description("取件时间段")]
		public string PickUpTime { get; set; }
 		/// <summary>
		/// 发票系统编号
		/// </summary>
		[Description("发票系统编号")]
		public int InvoiceSysNo { get; set; }
 		/// <summary>
		/// 是否取回发票:是(1),否(0)
		/// </summary>
		[Description("是否取回发票:是(1),否(0)")]
		public int IsPickUpInvoice { get; set; }
 		/// <summary>
		/// 换货配送方式系统编号
		/// </summary>
		[Description("换货配送方式系统编号")]
		public int ShipTypeSysNo { get; set; }
 		/// <summary>
		/// 退款方式:原路返回(10),至银行卡(20),门店退款(30),账
		/// </summary>
		[Description("退款方式:原路返回(10),至银行卡(20),门店退款(30),账")]
		public int RefundType { get; set; }
 		/// <summary>
		/// 应退金额(自动计算应退金额)
		/// </summary>
		[Description("应退金额(自动计算应退金额)")]
		public decimal OrginAmount { get; set; }
 		/// <summary>
		/// 应扣回积分(自动计算应退积分)
		/// </summary>
		[Description("应扣回积分(自动计算应退积分)")]
		public int OrginPoint { get; set; }
 		/// <summary>
		/// 应退惠源币(自动计算应退金币)
		/// </summary>
		[Description("应退惠源币(自动计算应退金币)")]
		public decimal OrginCoin { get; set; }
 		/// <summary>
		/// 扣回优惠券金额(自动计算优惠券金额，只做记录不运算)
		/// </summary>
		[Description("扣回优惠券金额(自动计算优惠券金额，只做记录不运算)")]
		public decimal CouponAmount { get; set; }
 		/// <summary>
		/// 发票扣款金额(无发票应扣款金额,人工修改为准)
		/// </summary>
		[Description("发票扣款金额(无发票应扣款金额,人工修改为准)")]
		public decimal DeductedInvoiceAmount { get; set; }
 		/// <summary>
		/// 现金补偿金额(当金币不够扣除时用现金补偿)
		/// </summary>
		[Description("现金补偿金额(当金币不够扣除时用现金补偿)")]
		public decimal RedeemAmount { get; set; }
 		/// <summary>
		/// 实退金额(不包含遗失发票扣款金额，人工修改应退金额)
		/// </summary>
		[Description("实退金额(不包含遗失发票扣款金额，人工修改应退金额)")]
		public decimal RefundProductAmount { get; set; }
 		/// <summary>
		/// 实扣回积分(与现金补偿互斥)
		/// </summary>
		[Description("实扣回积分(与现金补偿互斥)")]
		public int RefundPoint { get; set; }
 		/// <summary>
        /// 实退会员币
		/// </summary>
		[Description("实退会员币")]
		public decimal RefundCoin { get; set; }
 		/// <summary>
		/// 实退总金额(实退商品金额-发票扣款金额-现金补偿金额)
		/// </summary>
		[Description("实退总金额(实退商品金额-发票扣款金额-现金补偿金额)")]
		public decimal RefundTotalAmount { get; set; }
 		/// <summary>
		/// 对内备注
		/// </summary>
		[Description("对内备注")]
		public string InternalRemark { get; set; }
 		/// <summary>
		/// 客户备注
		/// </summary>
		[Description("客户备注")]
		public string RMARemark { get; set; }
 		/// <summary>
		/// 收款人开户行
		/// </summary>
		[Description("收款人开户行")]
		public string RefundBank { get; set; }
 		/// <summary>
		/// 收款人开户姓名
		/// </summary>
		[Description("收款人开户姓名")]
		public string RefundAccountName { get; set; }
 		/// <summary>
		/// 收款人银行账号
		/// </summary>
		[Description("收款人银行账号")]
		public string RefundAccount { get; set; }
 		/// <summary>
		/// 状态:待审核(10),待入库(20),待退款(30),已完成(50),
		/// </summary>
		[Description("状态:待审核(10),待入库(20),待退款(30),已完成(50),")]
		public int Status { get; set; }
 		/// <summary>
		/// 创建人
		/// </summary>
		[Description("创建人")]
		public int CreateBy { get; set; }
 		/// <summary>
		/// 创建时间
		/// </summary>
		[Description("创建时间")]
		public DateTime CreateDate { get; set; }
 		/// <summary>
		/// 作废人
		/// </summary>
		[Description("作废人")]
		public int CancelBy { get; set; }
 		/// <summary>
		/// 退换货作废时间
		/// </summary>
		[Description("退换货作废时间")]
		public DateTime CancelDate { get; set; }
 		/// <summary>
		/// 审核人
		/// </summary>
		[Description("审核人")]
		public int AuditorBy { get; set; }
 		/// <summary>
		/// 退换货审核时间
		/// </summary>
		[Description("退换货审核时间")]
		public DateTime AuditorDate { get; set; }
 		/// <summary>
		/// 退款人
		/// </summary>
		[Description("退款人")]
		public int RefundBy { get; set; }
 		/// <summary>
		/// 退款时间
		/// </summary>
		[Description("退款时间")]
		public DateTime RefundDate { get; set; }
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
 	}
}

	