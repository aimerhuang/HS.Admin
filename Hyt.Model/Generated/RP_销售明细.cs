
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 
	/// </summary>
    /// <remarks>
    /// 2013-12-06 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class RP_销售明细
	{
	  		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public DateTime 下单日期 { get; set; }
 		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public int 订单号 { get; set; }
 		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public int 订单来源 { get; set; }
 		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public int 订单来源编号 { get; set; }
 		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public int 配送编号 { get; set; }
 		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public string 配送方式 { get; set; }
 		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public int 客户编号 { get; set; }
 		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public int 收款方式编号 { get; set; }
 		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public string 收款方式 { get; set; }
 		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public DateTime 出库日期 { get; set; }
 		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public string 开票状态 { get; set; }
 		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public string 会员名 { get; set; }
 		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public string ERP编码 { get; set; }
 		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public string 产品名称 { get; set; }
 		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public int 数量 { get; set; }
 		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public decimal 单价 { get; set; }
 		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public decimal 优惠 { get; set; }
 		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public decimal 销售金额 { get; set; }
 		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public decimal 实收金额 { get; set; }
 		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public int 仓库编号 { get; set; }
 		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public string 出库仓库 { get; set; }
 		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public string 下单门店 { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public string 收货人 { get; set; }


 		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public string 收货地址 { get; set; }
 		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public string 收货电话 { get; set; }
 		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public string 客服 { get; set; }
 		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public string 送货员 { get; set; }
 		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public string 结算状态 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public string 店铺名称 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public string 商城订单号 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public string 省 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public string 市 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public string 区 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public int 收货地区编号 { get; set; }

        public int 结算单号 { get; set; }

        public string 快递单号 { get; set; }


        public string 对内备注 { get; set; }

        public int 出库单号 { get; set; }

        public DateTime? 发货日期 { get; set; }
 	}
}

	