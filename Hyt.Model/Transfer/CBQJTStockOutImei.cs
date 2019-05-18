using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 出库单商品串码记录
    /// </summary>
    /// <remarks>2016-02-18 谭显锋 创建</remarks>
    public class CBQJTStockOutImei : QJTStockOutImei
    {
        /// <summary>
        /// 商品Eas名称
        /// </summary>
        public string ProductEasName { get; set; }

        /// <summary>
        /// 商品Erp编号
        /// </summary>
        public string ProductErpSysno { get; set; }

        /// <summary>
        /// 仓库编号
        /// </summary>
        public int WarehouseSysNo { get; set; }
    }
}
