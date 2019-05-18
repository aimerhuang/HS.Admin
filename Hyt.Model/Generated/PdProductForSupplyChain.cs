using System;
using System.ComponentModel;

namespace Hyt.Model
{
    /// <summary>
    /// 供应链商品信息表
    /// </summary>
    public class PdProductForSupplyChain
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }
        /// <summary>
        /// 供应链商品信息
        /// </summary>
        [Description("供应链商品信息")]
        public string JsonString { get; set; }
        /// <summary>
        /// 供应链编号
        /// </summary>
        [Description("供应链编号")]
        public int SupplyChainCode { get; set; }
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
