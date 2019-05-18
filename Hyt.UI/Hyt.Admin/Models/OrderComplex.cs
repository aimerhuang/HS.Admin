using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hyt.Model;

namespace Hyt.Admin.Models
{
    /// <summary>
    /// 订单复杂对象，创建订单专用
    /// </summary>
    /// <remarks>2013－06-20 黄志勇 创建</remarks>
    public class OrderComplex
    {
        /// <summary>
        /// 订单
        /// </summary>
        public SoOrder SoOrder { get; set; }

        /// <summary>
        /// 收货地址
        /// </summary>
        public SoReceiveAddress SoReceiveAddress { get; set; }

        /// <summary>
        /// 订单明细
        /// </summary>
        public SoOrderItem[] Product { get; set; }

        /// <summary>
        /// 发票
        /// </summary>
        public FnInvoice Invoice { get; set; }

        /// <summary>
        /// 优惠券代码
        /// </summary>
        public string CouponCode { get; set; }
    }

}