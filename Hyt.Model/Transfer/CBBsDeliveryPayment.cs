using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 配送方式支付方式关联实体类
    /// </summary>
    /// <remarks>
    /// 2013-08-02 郑荣华 创建
    /// </remarks>
    public class CBBsDeliveryPayment : BsDeliveryPayment
    {
        /// <summary>
        /// 配送方式名称
        /// </summary>
        /// <remarks>2013-08-02 郑荣华 创建</remarks>
        public string DeliveryTypeName { get; set; }

        /// <summary>
        /// 配送类型，即父级系统编号
        /// </summary>
        /// <remarks>2013-08-02 郑荣华 创建</remarks>
        public string ParentSysNo { get; set; }

        /// <summary>
        /// 配送类型，即父级名称，用枚举
        /// </summary>
        /// <remarks>2013-08-02 郑荣华 创建</remarks>
        public string ParentName { get; set; }

        /// <summary>
        /// 支付方式名称
        /// </summary>
        /// <remarks>2013-08-02 郑荣华 创建</remarks>
        public string PaymentName { get; set; }

        /// <summary>
        /// 支付类型：预付（10）、到付（20）
        /// </summary>
        /// <remarks>2013-08-02 郑荣华 创建</remarks>
        public int PaymentType { get; set; }

        /// <summary>
        /// 是否网上支付：是（1）、否（0）
        /// </summary>
        /// <remarks>2013-08-02 郑荣华 创建</remarks>
        public int IsOnlinePay { get; set; }
    }
}
