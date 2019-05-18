
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    ///  2013-11-11 杨浩 T4生成
    /// </remarks>
    public partial class FnReceiptTitleAssociation : BaseEntity
    {
        ///<summary>
        ///系统编号
        ///</summary>
        [Description("系统编号")]
        public int SysNo { get; set; }

        ///<summary>
        ///仓库系统编号
        ///</summary>
        [Description("仓库系统编号")]
        public int WarehouseSysNo { get; set; }

        ///<summary>
        ///支付方式系统编号
        ///</summary>
        [Description("支付方式系统编号")]
        public int PaymentTypeSysNo { get; set; }

        ///<summary>
        ///EAS收款科目编码
        ///</summary>
        [Description("EAS收款科目编码")]
        public string EasReceiptCode { get; set; }

        ///<summary>
        ///科目名称
        ///</summary>
        [Description("科目名称")]
        public string EasReceiptName { get; set; }

        ///<summary>
        ///是否默认:是(1);否(0)
        ///</summary>
        [Description("是否默认:是(1);否(0)")]
        public int IsDefault { get; set; }

        ///<summary>
        ///创建人
        ///</summary>
        [Description("创建人")]
        public int CreatedBy { get; set; }

        ///<summary>
        ///创建时间
        ///</summary>
        [Description("创建时间")]
        public DateTime CreatedDate { get; set; }

        ///<summary>
        ///最后更新人
        ///</summary>
        [Description("最后更新人")]
        public int LastUpdateBy { get; set; }

        ///<summary>
        ///最后更新时间
        ///</summary>
        [Description("最后更新时间")]
        public DateTime LastUpdateDate { get; set; }

    }
}
