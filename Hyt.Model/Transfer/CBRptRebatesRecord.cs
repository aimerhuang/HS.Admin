
using System;
namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 分销返利统计
	/// </summary>
    /// <remarks>
    /// 2016-05-17 王耀发
    /// </remarks>
	[Serializable]
    public partial class CBRptRebatesRecord
	{
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }

        /// <summary>
        /// 订单ID
        /// </summary>
        public int OrderSysNo { get; set; }

        /// <summary>
        /// 订单编号 	
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 订单日期 	
        /// </summary>
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// 购买人ID
        /// </summary>
        public int PurchaserSysNo { get; set; }

        /// <summary>
        /// 购买人账号 	
        /// </summary>
        public string PurchaserAccount { get; set; }
        
        /// <summary>
        /// 购买人名称
        /// </summary>
        public string PurchaserName { get; set; }

        /// <summary>
        /// 返利人ID
        /// </summary>
        public int RebatesSysNo { get; set; }

        /// <summary>
        /// 返利人账号 	
        /// </summary>
        public string RebatesAccount { get; set; }       
        
        /// <summary>
        /// 返利人名称
        /// </summary>
        public string RebatesName { get; set; }

        /// <summary>
        /// 订单利润
        /// </summary>
        public decimal OrderCatle { get; set; }

        /// <summary>
        /// 返利金额
        /// </summary>
        public decimal RebatesAmount { get; set; }

        /// <summary>
        /// 返利操作费
        /// </summary>
        public decimal OperatFee { get; set; }

        /// <summary>
        /// 实际返利
        /// </summary>
        public decimal ARebatesAmount { get; set; }

        /// <summary>
        /// 订单商品金额
        /// </summary>
        public decimal ProductAmount { get; set; }

        /// <summary>
        /// 订单运费
        /// </summary>
        public decimal FreightAmount { get; set; }

        /// <summary>
        /// 订单总额
        /// </summary>
        public decimal OrderAmount { get; set; }

        /// <summary>
        /// 返利状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 返利类型
        /// </summary>
        public string RebatesType { get; set; }

        /// <summary>
        /// 分销等级ID
        /// </summary>
        public int LevelSysNo { get; set; }

        /// <summary>
        /// 返利人分销等级
        /// </summary>
        public string Genre { get; set; }

	}
}

	