using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 销售单实体
    /// </summary>
    /// <remarks> 2013-06-14 朱家宏 创建</remarks>
    public partial class SoOrder
    {
        /// <summary>
        /// 关联会员
        /// </summary>
        public CrCustomer Customer { get; set; } 

        /// <summary>
        /// 关联收货地址
        /// </summary>
        public SoReceiveAddress ReceiveAddress { get; set; }

        /// <summary>
        /// 订购商品列表
        /// </summary>
        /// <remarks>2013-06-19 朱成果 创建</remarks>
        public  IList<SoOrderItem> OrderItemList { get; set; }
        /// <summary>
        /// 订单发票信息
        /// </summary>
        /// <remarks>2013-06-21 朱成果 创建</remarks>
        public FnInvoice OrderInvoice { get; set; }
        /// <summary>
        /// 在线支付信息
        /// </summary>
        /// <remarks>2016-9-7 杨浩 添加</remarks>
        public FnOnlinePayment OnlinePayment { get; set; }

    }
}
