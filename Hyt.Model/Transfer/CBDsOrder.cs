using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// DSORDER扩展
    /// </summary>
    /// <remarks>2013-09-05 朱家宏 创建</remarks>
    public class CBDsOrder : DsOrder
    {
        /// <summary>
        /// hyt订单支付状态
        /// </summary>
        /// <remarks>2013-09-05 朱家宏 创建</remarks>
        public int PayStatus { get; set; }

        /// <summary>
        /// 店铺名称，本公司用 商城 
        /// </summary>
        /// <remarks>2013-09-05 郑荣华 创建</remarks>
        public string ShopName { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        /// <remarks>2013-09-05 朱家宏 创建</remarks>
        public int OrderSysNo { get; set; }

        /// <summary>
        /// 店铺账号
        /// </summary>  
        /// <remarks>2013-09-05 郑荣华 创建</remarks> 
        public string ShopAccount { get; set; }

        /// <summary>
        /// 快递公司
        /// </summary>
        /// <remarks>2014-05-08 朱成果 创建</remarks> 
        public string DeliveryTypeName { get; set; }


        /// <summary>
        /// 快递单号
        /// </summary>
        /// <remarks>2014-05-08 朱成果 创建</remarks> 
        public string ExpressNo { get; set; }

        /// <summary>
        /// 是否自营
        /// </summary>
        /// <remarks>2014-07-21 余勇 创建</remarks> 
        public int IsSelfSupport { get; set; }

        /// <summary>
        /// 顾客姓名
        /// </summary>
        /// <remarks>2047-8-23 罗熙 创建</remarks>
        public string CustomerName { get; set; }

        /// <summary>
        /// 支付状态
        /// </summary>
        public string OnlineStatus { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        /// <remarks>2017-8-23 罗熙 创建</remarks>
        public string ProductName { get; set; }

        /// <summary>
        /// 购买数量
        /// </summary>
        /// <remarks>2017-8-23 罗熙 创建</remarks>
        public int Quantity { get; set; }

        /// <summary>
        /// 销售总价
        /// </summary>
        /// <remarks>2017-8-23 罗熙 创建</remarks>
        public decimal SaleAmount { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        /// <remarks>2017-8-23 罗熙 创建</remarks>
        public string MobilePhoneNumber { get; set; }
    }
}
