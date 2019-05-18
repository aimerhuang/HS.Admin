using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hyt.Model.Kis
{
   public  class JsonItem
    {
        /// <summary>
        /// ()
        /// </summary>
        [Description("()")]
       public string FBatchNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public string FConsignAmount { get; set; }
        /// <summary>
        /// ()
        /// </summary>
        [Description("()")]
        public string FConsignPrice { get; set; }
        /// <summary>
        /// ()
        /// </summary>
        [Description("()")]
        public string FDCStockID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public string FDiscountAmount { get; set; }
        /// <summary>
        /// ()
        /// </summary>
        [Description("()")]
        public string FDiscountRate { get; set; }
        /// <summary>
        /// (，)
        /// </summary>
        [Description("()")]
        public string FItemID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public string FItemName { get; set; }
        /// <summary>
        /// ()
        /// </summary>
        [Description("()")]
        public string FKFDate { get; set; }
        /// <summary>
        /// (，)
        /// </summary>
        [Description("()")]
        public string FKFPeriod { get; set; }

        /// <summary>
        /// (，)
        /// </summary>
        [Description("()")]
        public string FPeriodDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public string FUnitID { get; set; }
        /// <summary>
        /// ()
        /// </summary>
        [Description("()")]
        public string Fauxqty { get; set; }
        /// <summary>
        /// (，)
        /// </summary>
        [Description("()")]
        public string Fnote { get; set; }
    }
}
