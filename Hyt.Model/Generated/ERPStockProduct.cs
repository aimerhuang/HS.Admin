using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Generated
{
    public class ERPStockProduct
    {
        //自动编号
        public int SysNo { get; set; }
        //商品编号
        public int ProductSysNo { get; set; }
        /// <summary>
        /// 商品编码
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// 仓库编号
        /// </summary>
        public int StockSysNo { get; set; }
        /// <summary>
        /// 保质期
        /// </summary>
        public int FKFPeriod { get; set; }
        /// <summary>
        /// ERP商品的商品名称
        /// </summary>
        public string ERPProductName { get; set; }
        /// <summary>
        /// ERP上的商品库存
        /// </summary>
        public decimal StockNum { get; set; }
    }
}
