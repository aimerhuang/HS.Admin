using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 仓库快递方式组合实体
    /// </summary>
    /// <remarks> 2013-07-09 郑荣华 创建</remarks>
    public class CBWhWarehouseDeliveryType : WhWarehouseDeliveryType
    {
        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WareHouseName { get; set; }

        /// <summary>
        /// 配送方式名称
        /// </summary>
        public string DeliveryTypeName { get; set; }
   
        /// <summary>
        /// 配送级别(0-5级,级别越高,处理优先级越高)
        /// </summary>
        public int DeliveryLevel { get; set; }

        /// <summary>
        /// 配送耗时
        /// </summary>
        public string DeliveryTime { get; set; }

        /// <summary>
        /// 运费
        /// </summary>
        public decimal Freight { get; set; }
    }
}
