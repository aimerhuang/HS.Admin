
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 
	/// </summary>
    /// <remarks>
    /// 2015-08-06 王耀发 T4生成
    /// </remarks>
	[Serializable]
    public partial class CrCustomerRebatesRecord
	{
        /// <summary>
        /// SysNo
        /// </summary>	
        [Description("SysNo")]
        public int SysNo { get; set; }
        /// <summary>
        /// RecommendSysNo
        /// </summary>	
        [Description("RecommendSysNo")]
        public int RecommendSysNo { get; set; }
        /// <summary>
        /// 注册/关注 人id
        /// </summary>	
        [Description("注册/关注 人id")]
        public int ComplySysNo { get; set; }
        /// <summary>
        /// 动作(0:注册 1:关注)
        /// </summary>	
        [Description("动作(0:注册 1:关注)")]
        public string Action { get; set; }
        /// <summary>
        /// 返利类型  1:一级佣金 2:二级佣金
        /// </summary>	
        [Description("返利类型  1:一级佣金 2:二级佣金")]
        public string Genre { get; set; }
        /// <summary>
        /// 返利金额
        /// </summary>	
        [Description("返利金额")]
        public decimal Rebates { get; set; }
        /// <summary>
        /// RebatesTime
        /// </summary>	
        [Description("RebatesTime")]
        public DateTime RebatesTime { get; set; }
        /// <summary>
        /// 订单系统编号
        /// </summary>	
        [Description("订单系统编号")]
        public int OrderSysNo { get; set; }
        /// <summary>
        /// 状态(0:冻结 1:成功 2:作废)
        /// </summary>	
        [Description("状态(0:冻结 1:成功 2:作废 3:部分作废)")]
        public string Status { get; set; }
        /// <summary>
        /// 经销商编号
        /// </summary>
        [Description("经销商编号")]
        public int DealerSysNo { get; set; }  
 	}
}

	