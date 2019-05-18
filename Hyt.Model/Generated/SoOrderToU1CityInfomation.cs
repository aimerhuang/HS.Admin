using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 推送又一城订单返回部分参数与本地参数综合（退货使用）
    /// </summary>
    [Serializable]
    public partial class SoOrderToU1CityInfomation
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }
        /// <summary>
        /// 本地订单编号
        /// </summary>
        [Description("本地订单编号")]
        public int OrderSysNo { get; set; }
        /// <summary>
        /// 传输过去返回单号
        /// </summary>
        [Description("传输过去返回单号")]
        public string OrderNo { get; set; }
        /// <summary>
        /// 本地订单编号
        /// </summary>
        [Description("本地订单编号")]
        public int ProductSysNo { get; set; }
        /// <summary>
        /// 商品数量
        /// </summary>
        [Description("商品数量")]
        public int ProductNum { get; set; }
        /// <summary>
        /// 又一城商品SKU
        /// </summary>
        [Description("又一城商品SKU")]
        public string TransactionPdSku { get; set; }
        /// <summary>
        /// 又一城返回订单编号
        /// </summary>
        [Description("又一城返回订单编号")]
        public string TransactionOrderNo { get; set; }
        /// <summary>
        /// 又一城商品出库单号
        /// </summary>
        [Description("又一城商品出库单号")]
        public string TransactionOutBillNo { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Description("创建时间")]
        public DateTime CreateDate { get; set; }


    }
}
