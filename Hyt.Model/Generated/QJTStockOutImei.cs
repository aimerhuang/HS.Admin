using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
    ///千机团串码记录表
    ///</summary>
    /// <remarks> 2016-02-17 朱成果 生成</remarks>
    public partial class QJTStockOutImei : BaseEntity
    {

        ///<summary>
        ///系统编号
        ///</summary>
        [Description("系统编号")]
        public int SysNo { get; set; }

        ///<summary>
        ///出库单编号
        ///</summary>
        [Description("出库单编号")]
        public int StockOutSysNo { get; set; }

        ///<summary>
        ///出库单明细编号
        ///</summary>
        [Description("出库单明细编号")]
        public int StockOutItemSysNo { get; set; }

        ///<summary>
        ///商品系统编号
        ///</summary>
        [Description("商品系统编号")]
        public int ProductSysNo { get; set; }

        ///<summary>
        ///串码
        ///</summary>
        [Description("串码")]
        public string Imei { get; set; }

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

