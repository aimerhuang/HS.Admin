using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.ExpressList
{
   public class KuaiDi
    {
        /// <summary>
        /// 快递单日期
        /// </summary>
        public string Date { get; set; }
        /// <summary>
        /// 快递公司
        /// </summary>
        public string Company { get; set; }
        /// <summary>
        /// 快递单号
        /// </summary>
        public string TrackingNo { get; set; }
    }
}
