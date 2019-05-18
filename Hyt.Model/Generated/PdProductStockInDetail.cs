
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
    public partial class PdProductStockInDetail
	{

        /// <summary>
        /// SysNo
        /// </summary>	
        [Description("SysNo")]
        public int SysNo { get; set; }
        /// <summary>
        /// ProductStockInSysNo
        /// </summary>	
        [Description("ProductStockInSysNo")]
        public int ProductStockInSysNo { get; set; }
        /// <summary>
        /// WarehouseSysNo
        /// </summary>	
        [Description("WarehouseSysNo")]
        public int WarehouseSysNo { get; set; }
        /// <summary>
        /// PdProductSysNo
        /// </summary>	
        [Description("PdProductSysNo")]
        public int PdProductSysNo { get; set; }
        /// <summary>
        /// 入库数量
        /// </summary>	
        [Description("入库数量")]
        public decimal StorageQuantity { get; set; }
        /// <summary>
        /// 已入库数量
        /// </summary>	
        [Description("已入库数量")]
        public decimal DoStorageQuantity { get; set; }
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

	