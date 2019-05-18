
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 
	/// </summary>
    /// <remarks>
    /// 2015-08-27 王耀发 T4生成
    /// </remarks>
	[Serializable]
    public partial class PdProductStockIn
	{
        /// <summary>
        /// SysNo
        /// </summary>	
        [Description("SysNo")]
        public int SysNo { get; set; }
        /// <summary>
        /// StockInNo
        /// </summary>	
        [Description("StockInNo")]
        public string StockInNo { get; set; }
        /// <summary>
        /// 入库时间
        /// </summary>	
        [Description("入库时间")]
        public DateTime StorageTime { get; set; }
        /// <summary>
        /// 状态
        /// </summary>	
        [Description("状态")]
        public int Status { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>	
        [Description("审核人")]
        public int AuditorSysNo { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>	
        [Description("审核时间")]
        public DateTime AuditDate { get; set; }
        /// <summary>
        /// CreatedBy
        /// </summary>	
        [Description("CreatedBy")]
        public int CreatedBy { get; set; }
        /// <summary>
        /// CreatedDate
        /// </summary>	
        [Description("CreatedDate")]
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// LastUpdateBy
        /// </summary>	
        [Description("LastUpdateBy")]
        public int LastUpdateBy { get; set; }
        /// <summary>
        /// LastUpdateDate
        /// </summary>	
        [Description("LastUpdateDate")]
        public DateTime LastUpdateDate { get; set; }     
 	}
}

	