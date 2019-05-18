using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Icp.GZNanSha
{
    /// <summary>
    /// 商检申请单
    /// </summary>
    public class CommodityInspection
    {
        public int SysNo { get; set; }
        public string CargoBcode { get; set; }
        public string Remark { get; set; }
        public DateTime CreateDataTime { get; set; }
        public DateTime ModifyDataTime { get; set; }
        public int Status { get; set; }
        /// <summary>
        /// 海关机构
        /// </summary>
        public int CustomsCode { get; set; }

        public int PushProductMsgID { get; set; }
    }
}
