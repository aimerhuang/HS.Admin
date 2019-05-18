using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.Erp.Model.BaseData
{
    public class EasStockProduct : EasBaseAccount
    {
        public string FNumber { get; set; }
        public string FName { get; set; }
        public string FStockName { get; set; }
        public string FBatchNo { get; set; }
        public string FKFPeriod { get; set; }
        public string FKFDate { get; set; }
        public string FQty { get; set; }
    }
}
