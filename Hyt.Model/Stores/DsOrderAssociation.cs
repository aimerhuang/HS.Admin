using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Stores
{
    /// <summary>
    /// 经销商订单与系统订单关联表
    /// </summary>
    /// <remarks>2016-9-7 杨浩 创建</remarks>
    public class DsOrderAssociation
    {
        /// <summary>
        /// 系统订单编号
        /// </summary>
        public int OrderSysNo { get; set; }
        /// <summary>
        /// 经销商订单编号
        /// </summary>
        public string DealerOrderNo { get; set; }
        /// <summary>
        /// 经销商系统编号
        /// </summary>
        public int DealerSysNo { get; set; }
    }
}
