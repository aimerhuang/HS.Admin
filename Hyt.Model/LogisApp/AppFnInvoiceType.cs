using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.LogisApp
{
    /// <summary>
    /// APP发票类型
    /// </summary>
    /// <remarks>2013-08-01 周唐炬 创建</remarks>
    public class AppFnInvoiceType : BaseEntity
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }

        /// <summary>
        /// 发票名称
        /// </summary>
        public string Name { get; set; }
    }
}
