using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    [Serializable]
    public partial class SoAddOrderToU1City
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [Description("系统编号")]
        public int SysNo { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        [Description("订单编号")]
        public int OrderSysNo { get; set; }
        /// <summary>
        /// 状态0未推送1已推送
        /// </summary>
        [Description("状态0未推送1已推送")]
        public int Status { get; set; }
        /// <summary>
        /// 操作人编号
        /// </summary>
        [Description("操作人编号")]
        public int OperatorSysNo { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Description("创建时间")]
        public DateTime CreatedDate { get; set; }
       
    }
}
