using System;
using System.ComponentModel;

namespace Hyt.Model
{
    /// <summary>
    /// 供应链商品分类关联表
    /// </summary>
    [Serializable]
    public partial class PdCategoryRelatedSupplyChain
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }
        /// <summary>
        /// 供应链编号
        /// </summary>
        [Description("供应链编号")]
        public int SupplyChainCode { get; set; }
        /// <summary>
        /// 供应链商品分类名称
        /// </summary>
        [Description("供应链商品分类名称")]
        public string SupplyChainCategoryName { get; set; }
        /// <summary>
        /// 本商城商品分类编号
        /// </summary>
        [Description("本商城商品分类编号")]
        public int CategorySysNo { get; set; }
        /// <summary>
        /// 商品分类编号路由
        /// </summary>
        [Description("商品分类编号路由")]
        public string CategorySysNos { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Description("备注")]
        public string Remarks { get; set; }
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
        /// 更新人
        /// </summary>
        [Description("更新人")]
        public int LastUpdateBy { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        [Description("更新时间")]
        public DateTime LastUpdateDate { get; set; }
    }
}
