using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.UpGrade.RongEgouModel
{
    /// <summary>
    /// 融e购订单接口响应报文
    /// </summary>
    /// <remarks>2018-03-19 罗熙 创建</remarks>
    public class RegOrderSelectResultPara
    {
        /// <summary>
        /// 订单列表节点
        /// </summary>
        public Orderlist order_list { get; set; }
    }

    public class Orderlist
    {
        /// <summary>
        /// 单个订单节点
        /// </summary>
        public List<Order> order { get; set; }
    }

    /// <summary>
    /// 单个订单节点
    /// </summary>
    public class Order
    {
        /// <summary>
        /// 订单ID
        /// </summary>
        public string order_id { get; set; }

        /// <summary>
        /// 下单时间
        /// </summary>
        public string order_create_time { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public string order_modify_time { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public int order_status { get; set; }
    }
}
