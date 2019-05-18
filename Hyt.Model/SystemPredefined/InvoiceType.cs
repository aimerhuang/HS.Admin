using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.SystemPredefined
{
    /// <summary>
    /// 发票类型 系统预定义
    /// 数据表：FnInvoiceType
    /// </summary>
    /// <remarks>2013-07-11 吴文强 创建</remarks>
    public static class InvoiceType
    {
        /// <summary>
        /// 一般发票 系统编号
        /// </summary>
        public static int 一般发票 { get { return 1; } }

        /// <summary>
        /// 增值税普通发票 系统编号
        /// </summary>
        public static int 增值税普通发票 { get { return 2; } }

        /// <summary>
        /// 增值税专用发票 系统编号
        /// </summary>
        public static int 增值税专用发票 { get { return 3; } }
    }
}
