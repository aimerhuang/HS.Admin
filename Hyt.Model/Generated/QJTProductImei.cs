using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
    ///千机团串码设置表
    ///</summary>
    /// <remarks> 2016-02-17 朱成果 生成</remarks>
    public partial class QJTProductImei : BaseEntity
    {

        ///<summary>
        ///系统编号
        ///</summary>
        [Description("系统编号")]
        public int SysNo { get; set; }

        ///<summary>
        ///商品类别编号
        ///</summary>
        [Description("商品类别编号")]
        public int ProductCategorySysNo { get; set; }

        ///<summary>
        ///商品系统编号
        ///</summary>
        [Description("商品系统编号")]
        public int ProductSysNo { get; set; }

        ///<summary>
        ///是否应用分类所有商品
        ///</summary>
        [Description("是否应用分类所有商品")]
        public int IsUseCategory { get; set; }

        ///<summary>
        ///创建时间
        ///</summary>
        [Description("创建时间")]
        public DateTime CreatedDate { get; set; }

        ///<summary>
        ///创建人
        ///</summary>
        [Description("创建人")]
        public int CreatedBy { get; set; }

        ///<summary>
        ///最后更新时间
        ///</summary>
        [Description("最后更新时间")]
        public DateTime LastUpdateDate { get; set; }

        ///<summary>
        ///最后更新人
        ///</summary>
        [Description("最后更新人")]
        public int LastUpdateBy { get; set; }

    }
}

