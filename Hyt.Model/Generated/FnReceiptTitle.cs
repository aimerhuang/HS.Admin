
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
    /// 收款科目
    /// </summary>
    /// <remarks>
    /// 2013-10-12 杨浩 T4生成
    /// </remarks>
    [Serializable]
    public partial class FnReceiptTitle
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }
        /// <summary>
        /// 父级系统编号
        /// </summary>
        [Description("父级系统编号")]
        public string ParentCode { get; set; }
        /// <summary>
        /// EAS科目编码
        /// </summary>
        [Description("EAS科目编码")]
        public string Code { get; set; }
        /// <summary>
        /// 科目名称
        /// </summary>
        [Description("科目名称")]
        public string Name { get; set; }
        /// <summary>
        /// 科目类型:现金科目(10),银行科目(20),其他科目(90)
        /// </summary>
        [Description("科目类型:现金科目(10),银行科目(20),其他科目(90)")]
        public int TitleType { get; set; }
        /// <summary>
        /// 状态:启用(1),禁用(0)
        /// </summary>
        [Description("状态:启用(1),禁用(0)")]
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

