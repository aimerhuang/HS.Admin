using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
     /// <summary>
    /// 仓库商品销量排行
    /// </summary>
    /// <remarks>2014-04-03 朱成果 创建</remarks>
   public  class ParaWarehouseProductSalesFilter
    {
        /// <summary>
        /// 开始日期(起)
        /// </summary>
        public DateTime? BeginDate { get; set; }

        /// <summary>
        /// 结束日期(止)
        /// </summary>
        public DateTime? EndDate { get; set; }

       /// <summary>
       /// 仓库编号
       /// </summary>
        public string WhWarehouseIDS { get; set; }

        /// <summary>
        /// 显示条数
        /// </summary>
        public int TakingCount { get; set; }

        /// <summary>
        /// 商品分类
        /// </summary>
        public List<int> ProductCategories { get; set; }

        public string ProductSaleType { get; set; }
    }
}
