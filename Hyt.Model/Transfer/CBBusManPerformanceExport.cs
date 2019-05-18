using System;

namespace Hyt.Model.Transfer
{
    public class CBBusManPerformanceExport
    {
        public string 姓名 { get; set; }
        public string 仓库 { get; set; }
        public int 配送次数白班 { get; set; }
        public int 配送次数夜班 { get; set; }
        public int 配送单量白班 { get; set; }
        public int 配送单量夜班 { get; set; }
        public decimal 升舱 { get; set; }
        public decimal 商城 { get; set; }
        public decimal 自销金额 { get; set; }
    }
}