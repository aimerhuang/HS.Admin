using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transport
{
    public class CBDsWhHistory : DsWhHistory
    {
        public string GMCreateTime { get; set; }
        
    }
    public class DsWhHistory
    {
        public int SysNo { get; set; }
        public string OrderCode { get; set; }
        public string CusOrderCode { get; set; }
        public string DisInfo { get; set; }
        public string CityInfo { get; set; }
        public DateTime CreateTime { get; set; }
        public string Operator { get; set; }
    }
}
