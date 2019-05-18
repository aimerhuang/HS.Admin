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
    public class KisInventory
    {
        /// <summary>
        /// 商品编码
        /// </summary>
        public string FNumber { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string FName { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string FStockName { get; set; }
        /// <summary>
        /// 仓库编码
        /// </summary>
        public string FStockID { get; set; }

        /// <summary>
        /// 批号
        /// </summary>
        public string FBatchNo { get; set; }

        /// <summary>
        /// 保质期
        /// </summary>
        public string FKFPeriod { get; set; }

        /// <summary>
        /// 保质期至
        /// </summary>
        public string FKFDate { get; set; }

        /// <summary>
        /// 库存数量
        /// </summary>
        public int FQty { get; set; }


    }
}
