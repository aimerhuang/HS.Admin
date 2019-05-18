using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.Erp.Model
{
    /// <summary>
    /// 库存查询
    /// </summary>
    /// <remarks>2013-9-18 杨浩 创建</remarks>
    public class Inventory
    {
        /// <summary>
        /// 仓库编码
        /// </summary>
        public string WarehouseNumber { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WarehouseName { get; set; }

        /// <summary>
        /// 商品编码
        /// </summary>
        public string MaterialNumber { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 仓库数量
        /// </summary>
        public int Quantity { get; set; }
    }
}
