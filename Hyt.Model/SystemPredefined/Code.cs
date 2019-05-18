using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.SystemPredefined
{
    /// <summary>
    /// 码表 系统编号
    /// </summary>
    /// <remarks>2014-1-21 吴文强 创建</remarks>
    public class Code
    {
        /// <summary>
        /// 配送时间段 系统编号
        /// </summary>
        public static int 配送时间段 { get { return 1; } }

        /// <summary>
        /// 出库单作废原因 系统编号
        /// </summary>
        public static int 出库单作废原因 { get { return 2; } }

        /// <summary>
        /// 门店转快递原因 系统编号
        /// </summary>
        public static int 门店转快递原因 { get { return 3; } }

        /// <summary>
        /// 门店延迟自提原因 系统编号
        /// </summary>
        public static int 门店延迟自提原因 { get { return 4; } }

        /// <summary>
        /// 物流App配送拒收 系统编号
        /// </summary>
        public static int 物流App配送拒收 { get { return 5; } }

        /// <summary>
        /// 物流App配送未送达 系统编号
        /// </summary>
        public static int 物流App配送未送达 { get { return 6; } }

        /// <summary>
        /// 订单作废原因
        /// </summary>
        public static int 订单作废原因
        {
            get { return 7; }
        }

        /// <summary>
        /// 锁定任务原因
        /// </summary>
        public static int 锁定任务原因 { get { return 8; } }

    }
}
