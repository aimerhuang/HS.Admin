using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Api
{
    /// <summary>
    /// 订单支付推送日志
    /// </summary>
    /// <remarks>2017-08-14 杨浩 创建</remarks>
    public class SoOrderPayPushLog
    {
        public int SysNo { get; set; }
        /// <summary>
        /// 唯一标识
        /// </summary>
        public string MarkId { get; set; }
        /// <summary>
        /// 支付类型系统编号
        /// </summary>
        public int PaymentTypeSysNo { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public int OrderSysNo { get; set; }
        /// <summary>
        /// 推送报文
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 回执报文
        /// </summary>
        public string ReceiptMessage { get; set; }
        /// <summary>
        /// 错误代码
        /// </summary>
        public string ErrorCode { get; set; }
        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErroMsg{ get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public int CreateBy { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 回执日期
        /// </summary>
        public DateTime? ReceiptDate { get; set; }
        /// <summary>
        /// 扩展数据
        /// </summary>
        public string Data { get; set; }
    }
}
