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
    /// 2013-08-27 杨合余 T4生成
    /// </remarks>
   [Serializable]
   public  class RcNoReturnExchange
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        /// <remarks>2013-08-27 杨合余 T4生成</remarks>
        [Description("系统编号")]
        public int SysNo { get; set; }

        /// <summary>
        /// 原订单编号
        /// </summary>
        /// <remarks>2013-08-27 杨合余 T4生成</remarks>
        [Description("原订单编号")]
        public int OrderSysNo { get; set; }

        /// <summary>
        /// 退换货系统编号
        /// </summary>
        /// <remarks>2013-08-27 杨合余 T4生成</remarks>
        [Description("原订单编号")]
        public int ReturnSysNo { get; set; }

        /// <summary>
        /// 新订单编号
        /// </summary>
        /// <remarks>2013-08-27 杨合余 T4生成</remarks>
        [Description("新订单编号")]
        public int NewOrderSysNo { get; set; }

        /// <summary>
        /// 经销商商城系统编号
        /// </summary>
        /// <remarks>2013-08-27 杨合余 T4生成</remarks>
        [Description("经销商商城系统编号")]
        public int DealerMallSysNo { get; set; }

        /// <summary>
        /// 第三方订单号
        /// </summary>
        /// <remarks>2013-08-27 杨合余 T4生成</remarks>
        [Description("第三方订单号")]
        public string MallOrderId { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        /// <remarks>2013-08-27 杨合余 T4生成</remarks>
        [Description("创建人")]
        public int  CreateBy { get; set; }


        /// <summary>
        /// 创建时间
        /// </summary>
        /// <remarks>2013-08-27 杨合余 T4生成</remarks>
        [Description("创建时间")]
        public DateTime CreateDate { get; set; }

    }
}
