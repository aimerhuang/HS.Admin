using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Logis.XinYi
{
    /// <summary>
    /// 订单表头
    /// </summary>
    /// <remarks>2015-10-15 杨云奕 新建</remarks>
    public class OrderHead
    {
        /// <summary>
        /// 货主编号 由心怡分配编号 必选
        /// </summary>
        public string OnNumber { get; set; }
        /// <summary>
        /// 仓库编号 由心怡分配编号 必选
        /// </summary>
        public string WhNumber { get; set; }
        /// <summary>
        /// 进仓单编号 来源编号 必选
        /// </summary>
        public string RoNumber { get; set; }
        /// <summary>
        /// 订单日期 格式：yyyy-MM-ddHH:mm:ss 必选
        /// </summary>
        public string OrderDate { get; set; }
        /// <summary>
        /// 总货值 可选
        /// </summary>
        public decimal ProPriceSum { get; set; }
        /// <summary>
        /// 备注 可选
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 总数量 可选
        /// </summary>
        public int QtySum { get; set; }
        /// <summary>
        /// 优先级 可选
        /// </summary>
        public int OIOPOrderPriority { get; set; }
    }
}
