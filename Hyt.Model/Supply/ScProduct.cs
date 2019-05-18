
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
    /// 供应商产品
    /// </summary>
    /// <remarks>
    /// 2016-03-17 杨浩 T4生成
    /// </remarks>
    [Serializable]
    public partial class ScProduct
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        [Description("产品名称")]
        public string ProductName { get; set; }
        /// <summary>
        /// sku
        /// </summary>
        [Description("sku")]
        public string SKU { get; set; }
        /// <summary>
        /// 供应商代码
        /// </summary>
        [Description("供应商代码")]
        public int SupplyCode { get; set; }
        /// <summary>
        /// 产品系统编号
        /// </summary>
        [Description("产品系统编号")]
        public int ProductSysNo { get; set; }
        /// <summary>
        /// 供应商产品接口回执
        /// </summary>
        [Description("供应商产品接口回执")]
        public string Receipt { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        [Description("状态")]
        public int Status { get; set; }
        /// <summary>
        /// 商品产地
        /// </summary>
        [Description("商品产地")]
        public string Cradle { get; set; }
        /// <summary>
        /// 品牌
        /// </summary>
        [Description("品牌")]
        public string Brands { get; set; }
        /// <summary>
        /// 税率
        /// </summary>
        [Description("税率")]
        public string Tariff { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        [Description("价格")]
        public decimal Price { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        [Description("创建日期")]
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 最后更新日期
        /// </summary>
        [Description("最后更新日期")]
        public DateTime UpdateDate { get; set; }
    }
}