using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Icp.GZNanSha
{
    /// <summary>
    /// 支付宝
    /// </summary>
    public class AliAcquireCustomsBack
    {
        /// <summary>
        /// 自动编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public int OrderSysNo { get; set; }
        /// <summary>
        /// 支付宝推送到海关返回的单据号
        /// </summary>
        public string CustomsTradeNo { get; set; }
        /// <summary>
        /// 支付宝交易单据号
        /// </summary>
        public string AlipayTradeNo { get; set; }
        /// <summary>
        /// 返回结果
        /// </summary>
        public string Success { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string Error { get; set; }
        /// <summary>
        /// 响应吗
        /// </summary>
        public string ResultCode { get; set; }
        /// <summary>
        /// 错误码
        /// </summary>
        public string DetailErrorCode { get; set; }
        /// <summary>
        /// 错误描述
        /// </summary>
        public string DetailErrorDes { get; set; }
        /// <summary>
        /// 报关流水号
        /// </summary>
        public string OutRequestNo { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 反馈报文
        /// </summary>
        public string OutReportXML { get; set; }
    }
}
