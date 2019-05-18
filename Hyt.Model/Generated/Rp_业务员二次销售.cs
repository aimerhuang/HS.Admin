using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 2014-09-23 朱成果 创建
    /// </remarks>
    [Serializable]
    public  class Rp_业务员二次销售
    {
        /// <summary>
        /// 
        /// </summary>
        [Description("业务员SysNo")]
        public int  DeliveryUserSysNo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("业务员姓名SysNo")]
        public string  DeliveryUserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("仓库SysNo")]
        public int StockSysNo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("仓库名称")]
        public string  StockName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("订单SysNo")]
        public int OrderSysNo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("订单金额")]
        public decimal  OrderAmount { get; set; }

        [Description("创建时间")]
        public DateTime  CreateDate{get; set;}
    }
}
