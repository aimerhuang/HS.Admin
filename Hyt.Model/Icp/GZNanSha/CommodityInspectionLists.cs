using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Icp.GZNanSha
{
    public class CommodityInspectionLists : Hyt.Model.Icp.GZNanSha.Record1
    {
        public int SysNo { get; set; }
        public int RegStatus { get; set; }
        public DateTime AppplyTime { get; set; }

        public string CIQGoodsNO { get; set; }
        public string RegNotes { get; set; }
        public int ciSysNo { get; set; }

        public string EroCode { get; set; }
    }
}
