
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
    public partial class LgFreightModuleDetails
	{
        /// <summary>
        /// 系统编号
        /// </summary>	
        [Description("系统编号")]
        public int SysNo { get; set; }
        /// <summary>
        /// 运费模板编号
        /// </summary>	
        [Description("运费模板编号")]
        public int FreightModuleSysNo { get; set; }
        /// <summary>
        /// 是否包邮
        /// </summary>	
        [Description("是否包邮")]
        public int IsPost { get; set; }
        /// <summary>
        /// 计价方式
        /// </summary>	
        [Description("计价方式")]
        public int ValuationStyle { get; set; }
        /// <summary>
        /// 运送方式
        /// </summary>	
        [Description("运送方式")]
        public int DeliveryStyle { get; set; }
        /// <summary>
        /// 运送地区
        /// </summary>	
        [Description("运送地区")]
        public string DeliveryArea { get; set; }
        /// <summary>
        /// 首(件、重、体积)
        /// </summary>	
        [Description("首(件、重、体积)")]
        public decimal First { get; set; }
        /// <summary>
        /// 首费
        /// </summary>	
        [Description("首费")]
        public decimal FirstPayment { get; set; }
        /// <summary>
        /// 续(件、重、体积)
        /// </summary>	
        [Description("续(件、重、体积)")]
        public decimal Next { get; set; }
        /// <summary>
        /// 续费
        /// </summary>	
        [Description("续费")]
        public decimal NextPayment { get; set; }
        /// <summary>
        /// 偏差校验
        /// </summary>	
        [Description("偏差校验")]
        public decimal Offset { get; set; }
        /// <summary>
        /// 状态
        /// </summary>	
        [Description("状态")]
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
 	}
}

	