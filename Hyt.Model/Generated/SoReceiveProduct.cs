using System;
using System.ComponentModel;

namespace Hyt.Model
{
    /// <summary>
    /// 12元商品领取记录表
    /// </summary>
    /// <remarks>
    /// 2016-08-01 周 创建
    [Serializable]
    public partial class SoReceiveProduct
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }
        /// <summary>
        /// 会员编号
        /// </summary>
        [Description("会员编号")]
        public int CustomerSysNo { get; set; }
        /// <summary>
        /// 商品编号
        /// </summary>
        [Description("商品编号")]
        public int ProductSysNo { get; set; }
        /// <summary>
        /// 分销商编号
        /// </summary>
        [Description("分销商编号")]
        public int DealerSysNo { get; set; }
        /// <summary>
        /// 仓库编号
        /// </summary>
        [Description("仓库编号")]
        public int WarehoseSysNo { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Description("创建时间")]
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// 备用字段1
        /// </summary>
        [Description("备用字段1")]
        public int SpareOne { get; set; }
        /// <summary>
        /// 备用字段2
        /// </summary>
        [Description("备用字段2")]
        public string SpareTwo { get; set; }
    }
}
