using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
    ///调货单
    ///</summary>
    /// <remarks> 2016-04-01 朱成果 生成</remarks>
    public partial class TransferCargo : BaseEntity
    {

        ///<summary>
        ///编号
        ///</summary>
        [Description("编号")]
        public int SysNo { get; set; }

        ///<summary>
        ///订单号
        ///</summary>
        [Description("订单号")]
        public int OrderSysNo { get; set; }

        ///<summary>
        ///出库单编号
        ///</summary>
        [Description("出库单编号")]
        public int StockOutSysNo { get; set; }

        ///<summary>
        ///申请调货仓库
        ///</summary>
        [Description("申请调货仓库")]
        public int ApplyWarehouseSysNo { get; set; }

        ///<summary>
        ///配货仓库编号
        ///</summary>
        [Description("配货仓库编号")]
        public int DeliveryWarehouseSysNo { get; set; }

        ///<summary>
        ///调货状态
        ///</summary>
        [Description("调货状态")]
        public int Status { get; set; }

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

        ///<summary>
        ///备注
        ///</summary>
        [Description("备注")]
        public string Remarks { get; set; }

    }
}

