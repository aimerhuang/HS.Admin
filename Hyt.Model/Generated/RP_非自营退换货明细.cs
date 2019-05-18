
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
    public partial class RP_非自营退换货明细
	{
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

        public int 订单来源编号 { get; set; }

        public DateTime 申请日期 { get; set; }

        public DateTime 入库日期 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public string 商城订单号 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public string 商城名称 { get; set; }
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
        public decimal 退款金额 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public decimal 实退金额 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public string 下单门店 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public string 入库仓库 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public int 仓库编号 { get; set; }

        public string 源单出库仓库 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public int 源单出库仓库编号 { get; set; }

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

        [Description("")]
        public int 退款方式 { get; set; }

        [Description("")]
        public int 配送编号 { get; set; }

 		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public string 配送方式 { get; set; }
        [Description("")]
        public int 售后方式编号 { get; set; }

        [Description("")]
        public string 售后方式 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public int 结算状态 { get; set; }

        public string 收款单状态 { get; set; }

        public DateTime 收款时间 { get; set; }

        public string 加盟商ERP编号 { get; set; }

        public string 加盟商ERP名称 { get; set; }
 	}
}

	