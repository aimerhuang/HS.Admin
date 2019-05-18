
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
    public partial class PdProductStockInDetailList
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
        /// 仓库名称
        /// </summary>
        [Description("仓库名称")]
        public string WarehouseName { get; set; }
        /// <summary>
        /// 街道地址
        /// </summary>
        [Description("街道地址")]
        public string StreetAddress { get; set; }
        /// <summary>
        /// 商品编号
        /// </summary>
        [Description("商品编号")]
        public string ErpCode { get; set; }
        /// <summary>
        /// 后台显示名称
        /// </summary>
        [Description("后台显示名称")]
        public string EasName { get; set; }
        /// <summary>
        /// 推送状态
        /// </summary>	
        [Description("推送状态")]
        public int SendStatus { get; set; }
 	}
}

	