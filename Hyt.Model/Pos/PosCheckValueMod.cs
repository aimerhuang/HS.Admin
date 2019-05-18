using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Pos
{
    public class PosCheckValueMod
    {
        public string DealerName { get; set; }
        public string PosName { get; set; }
        public string CheckTime { get; set; }
        public string PosOrderNum { get; set; }
        public string PosTotalValue { get; set; }

        public int TotalProductNumber { get; set; }
        public string PosRetOrderNum { get; set; }
        public string PosRetTotalValue { get; set; }
        public string posOrginTotalValue { get; set; }
        public string PosDisValue { get; set; }
        public string PosSellValue { get; set; }
        public string PointMoney { get; set; }
        public string Cash { get; set; }
        public string NoCash { get; set; }

        public string KimsVolume { get; set; }
        public string BankValue { get; set; }
        public string AliValue { get; set; }
        public string WXValue { get; set; }
    }
}
