using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 电子面单批量创建配送单
    /// </summary>
    /// <remarks>2015-10-10 谭显锋 创建</remarks>
    [Serializable]
    public class CBElectronOutStockPrint
    {
        /// <summary>
        /// 仓库编号
        /// </summary>
        public int WarehouseSysNo { get; set; }
        /// <summary>
        /// 单据编号(出库单号)
        /// </summary>
        public int NoteSysNo { get; set; }
    }
}
