
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 分销商等级
	/// </summary>
    /// <remarks>
    /// 2013-09-10 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class DsDealerLevel
	{
	  	/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 等级名称
		/// </summary>
		[Description("等级名称")]
		public string LevelName { get; set; }
 		/// <summary>
		/// 等级描述
		/// </summary>
		[Description("等级描述")]
		public string LevelDescription { get; set; }
 		/// <summary>
		/// 状态:启用(1),禁用(0)
		/// </summary>
		[Description("状态:启用(1),禁用(0)")]
		public int Status { get; set; }
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
        /// 直接推荐人返利比例
        /// </summary>	
        [Description("直接推荐人返利比例")]
        public decimal Direct { get; set; }
        /// <summary>
        /// 间1返利比例
        /// </summary>	
        [Description("间1返利比例")]
        public decimal Indirect1 { get; set; }
        /// <summary>
        /// 间2返利比例
        /// </summary>	
        [Description("间2返利比例")]
        public decimal Indirect2 { get; set; }
        /// <summary>
        /// 销售价调整上限
        /// </summary>	
        [Description("销售价调整上限")]
        public decimal SalePriceUpper { get; set; }
        /// <summary>
        /// 销售价调整下限
        /// </summary>	
        [Description("销售价调整下限")]
        public decimal SalePriceLower { get; set; }
        /// <summary>
        /// 利润比例
        /// </summary>	
        [Description("利润比例")]
        public decimal ProfitRatio { get; set; }
        /// <summary>
        /// 操作费（千分比）
        /// </summary>
        [Description("操作费")]
        public decimal OperatFee { get; set; }

        /// <summary>
        /// 分销商返点比例
        /// </summary>
        /// <remarks> 2016-04-14 刘伟豪 添加 </remarks>
        [Description("分销商返点比例")]
        public decimal DealerRatio { get; set; }

        /// <summary>
        /// 代理商返点比例
        /// </summary>
        /// <remarks> 2016-06-17 刘伟豪 添加 </remarks>
        [Description("代理商返点比例")]
        public decimal AgentRatio { get; set; }

        /// <summary>
        /// 特殊返点比例
        /// </summary>
        /// <remarks> 2016-06-17 刘伟豪 添加 </remarks>
        [Description("特殊返点比例")]
        public decimal SpecialRatio { get; set; }
 	}
}