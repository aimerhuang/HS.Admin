using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Icp.GZNanSha.RegRec
{
    /// <summary>
    /// 回折报文
    /// </summary>
    public class Record
    {
        /// <summary>
        /// 商品申请编号
        /// </summary>
        public string CargoBcode { get; set; }
        /// <summary>
        /// 跨境电商企业备案号
        /// </summary>
        public string CbeComcode { get; set; }
        /// <summary>
        /// 商品货号
        /// </summary>
        public string Gcode { get; set; }
        /// <summary>
        /// 商品备案号
        /// </summary>
        public string CIQGoodsNO { get; set; }
        /// <summary>
        /// ICIP回执状态
        /// </summary>
        public string RegStatus { get; set; }
        /// <summary>
        /// ICIP回执信息
        /// </summary>
        public string RegNotes { get; set; }
    }
}
