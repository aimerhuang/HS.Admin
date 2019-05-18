using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Order
{
    /// <summary>
    /// 订单同步物流日志
    /// </summary>
    /// <remarks>2016-7-29 杨浩 创建</remarks>
    public class SoOrderSyncLogisticsLog
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public int OrderSysNo { get; set; }
        /// <summary>
        /// 物流订单号
        /// </summary>
        public string LogisticsOrderId { get; set; }
        /// <summary>
        /// 物流状态码
        /// </summary>
        public string StatusCode { get; set; }
        /// <summary>
        /// 物流状态消息
        /// </summary>
        public string StatusMsg { get; set; }
        /// <summary>
        /// 物流代码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 提交报文
        /// </summary>
        public string Packets { get; set; }
        /// <summary>
        /// 回执内容
        /// </summary>
        public string ReceiptContent { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// 最后更新人
        /// </summary>
        public int LastUpdateBy { get; set; }
        /// <summary>
        /// 最后更新日期
        /// </summary>
        public DateTime LastUpdateDate { get; set; }
    }
}
