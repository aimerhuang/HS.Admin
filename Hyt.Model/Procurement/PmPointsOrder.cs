using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Procurement
{
    public class CBPmPointsOrder : PmPointsOrder
    {
        public string FContact { get; set; }
        public string BankName { get; set; }
        public string BankIDCard { get;set; }
        public string ManufacturerCode { get; set; }
        public string CreateName { get; set; }
        public string UpdateName { get; set; }
        public List<CBPmPointsOrderItem> listItems = new List<CBPmPointsOrderItem>();
    }
    public class PmPointsOrder
    {
        public int SysNo { get; set; }
        public string Po_Number { get; set; }
        public int Po_ProcurementSysNo { get; set; }
        public int Po_CreateSysNo { get; set; }
        public DateTime Po_CreateTime { get; set; }
        public int Po_UpdateSysNo { get; set; }
        public DateTime Po_UpdateTime { get; set; }
        public string Po_FactoryName { get; set; }
        public int Po_Status { get; set; }
    }
}
