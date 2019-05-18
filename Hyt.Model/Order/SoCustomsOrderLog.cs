using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Order
{
    /// <summary>
    /// 海关订单日志
    /// </summary>
    /// <remarks>2016-1-2 杨浩 创建</remarks>
    public class SoCustomsOrderLog
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// ftp文件名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 海关回执状态消息
        /// </summary>
        public string StatusMsg { get; set; }
        /// <summary>
        /// 海关回执代码
        /// </summary>
        public string StatusCode { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public int OrderSysNo { get; set; }
        /// <summary>
        /// 海关通道
        /// </summary>
        public int CustomsChannel { get; set; }
        /// <summary>
        /// 回执内容
        /// </summary>
        public string ReceiptContent { get; set; }
        /// <summary>
        /// 报文
        /// </summary>
        public string Packets { get; set; }
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
