using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    public class CBWhInventoryAlarm : WhInventoryAlarm
    {
        /// <summary>
        /// 商品编码
        /// </summary>
        /// <remarks>
        /// 2016-05-17 王耀发 创建
        /// </remarks>
        public string ErpCode { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        /// <remarks>
        /// 2016-05-17 王耀发 创建
        /// </remarks>
        public string ProductName { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        /// <remarks>
        /// 2016-05-17 王耀发 创建
        /// </remarks>
        public string BackWarehouseName { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        /// <remarks>
        /// 2016-05-17 王耀发 创建
        /// </remarks>
        public decimal StockQuantity { get; set; }
    }
}
