
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
    public partial class PdProductFreight
	{
        /// <summary>
        /// 系统编号
        /// </summary>	
        [Description("系统编号")]
        public int SysNo { get; set; }
        /// <summary>
        /// 商品编号
        /// </summary>	
        [Description("商品编号")]
        public int? PdProductSysNo { get; set; }
        /// <summary>
        /// 运费
        /// </summary>	
        [Description("运费")]
        public decimal? Freigh { get; set; }
        /// <summary>
        /// 运费模板编号
        /// </summary>	
        [Description("运费模板编号")]
        public int? LgFreightModuleSysNo { get; set; }
        /// <summary>
        /// 计算值
        /// </summary>	
        [Description("计算值")]
        public decimal? ValuationValue { get; set; }
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

	