using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Pos
{
    public class CBDsTakeStockOrder : DsTakeStockOrder
    {
        /// <summary>
        /// 经销商名称
        /// </summary>
        public string DealerName { get; set; }
        public List<DsTakeStockItem> itemList = new List<DsTakeStockItem>();
        /// <summary>
        /// 盈亏总额
        /// </summary>
        public decimal TotalAmount { get; set; }
        public DateTime? DateTime { get; set; }
        public int? ProfitLoss { get; set; }
        public string ProductName { get; set; }
        public string ProductBarCode { get; set; }
        public int ProOldNum { get; set; }
        public int ProNowNum { get; set; }
    }
    /// <summary>
    /// 数据盘点
    /// </summary>
    public class DsTakeStockOrder
    {
        public int DsSysNo { get; set; }
        public int SysNo { get; set; }
        public string Number { get; set; }
        public string CheckUser { get; set; }
        public DateTime CheckTime { get; set; }
        public int OnLineWebType { get; set; }
    }
}
