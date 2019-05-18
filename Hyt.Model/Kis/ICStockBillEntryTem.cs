using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hyt.Model.Kis
{
    /// <summary>
    /// 明细表
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    public class ICStockBillEntryTem
    {
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public Decimal Fauxqty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public Decimal FAuxQtyMust { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public Decimal FQty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public Decimal FQtyMust { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public Decimal Famount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public Decimal FAmtRef { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public Decimal Fauxprice { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public Decimal FTaxRate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public Decimal FAuxTaxPrice { get; set; }
        /// <summary>
        /// ()
        /// </summary>
        [Description("")]
        public Decimal FAllAmount { get; set; }
        /// <summary>
        /// ()
        /// </summary>
        [Description("()")]
        public string Fnote { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public string FItemID { get; set; }
        /// <summary>
        /// ()
        /// </summary>
        [Description("()")]
        public string FUnitID { get; set; }
        /// <summary>
        /// ()
        /// </summary>
        [Description("()")]
        public int FK3ItemID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public string FSCStockID { get; set; }
        /// <summary>
        /// ()
        /// </summary>
        [Description("()")]
        public int FK3SCStockID { get; set; }
        /// <summary>
        /// (，)
        /// </summary>
        [Description("()")]
        public string FDCStockID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public int FK3DCStockID { get; set; }
        /// <summary>
        /// ()
        /// </summary>
        [Description("()")]
        public int FK3UnitID { get; set; }
        /// <summary>
        /// (，)
        /// </summary>
        [Description("()")]
        public string FCurrencyID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public int FK3CurrencyID { get; set; }
        /// <summary>
        /// ()
        /// </summary>
        [Description("()")]
        public string FInBillNo { get; set; }
        /// <summary>
        /// (，)
        /// </summary>
        [Description("()")]
        public double FauxqtyActual { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public string FKFDate { get; set; }
        /// <summary>
        /// ()
        /// </summary>
        [Description("()")]
        public int FKFPeriod { get; set; }
        /// <summary>
        /// (，)
        /// </summary>
        [Description("()")]
        public string FPeriodDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public string FOrderNo { get; set; }
        /// <summary>
        /// ()
        /// </summary>
        [Description("()")]
        public string FFetchDate { get; set; }
        /// <summary>
        /// (，)
        /// </summary>
        [Description("()")]
        public string FConsignee { get; set; }
        /// <summary>
        /// ()
        /// </summary>
        [Description("()")]
        public string FDeliveryPlace { get; set; }
        /// <summary>
        /// (，)
        /// </summary>
        [Description("()")]
        public string FSettleDate { get; set; }
    }
}
