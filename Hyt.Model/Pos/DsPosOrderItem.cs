using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Pos
{
    /// <summary>
    /// Pos管理订单商品列表信息
    /// </summary>
    public class DsPosOrderItem
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// Pos订单父id
        /// </summary>
        public int pSysNo { get; set; }
        /// <summary>
        /// 商品编号
        /// </summary>
        public int ProSysNo { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProName { get; set; }
        /// <summary>
        /// 商品条码
        /// </summary>
        public string ProBarCode { get; set; }
        /// <summary>
        /// 商品原价
        /// </summary>
        public decimal ProPrice { get; set; }
        /// <summary>
        /// 商品销售数量
        /// </summary>
        public int ProNum { get; set; }
        /// <summary>
        /// 商品销售折扣
        /// </summary>
        public decimal ProDiscount { get; set; }
        /// <summary>
        /// 商品折扣金额
        /// </summary>
        public decimal ProDisPrice { get; set; }
        /// <summary>
        /// 总销售金额
        /// </summary>
        public decimal ProTotalValue { get; set; }
        /// <summary>
        /// 商品库房存留
        /// </summary>
        public int WareNum { get; set; }

    }
}
