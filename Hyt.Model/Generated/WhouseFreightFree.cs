using System;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Hyt.Model
{
    /// <summary>
    /// 仓库免运费表
    /// </summary>
    /// <remarks>
    /// 2016-04-20 王耀发 T4生成
    /// </remarks>
    [Serializable]
    public partial class WhouseFreightFree
    {
        /// <summary>
        /// 系统编号
        /// </summary>	
        [Description("系统编号")]
        public int SysNo { get; set; }
        /// <summary>
        /// 仓库系统编号
        /// </summary>	
        [Description("仓库系统编号")]
        public int WarehouseSysNo { get; set; }
        /// <summary>
        /// 免邮金额
        /// </summary>	
        [Description("免邮金额")]
        public decimal? FreightFreeAmount { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>	
        [Description("是否启用")]
        public int IsUse { get; set; }
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
