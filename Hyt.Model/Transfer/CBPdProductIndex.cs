using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 索引详情
    /// </summary>
    public class CBPdProductIndex : PdProductIndex
    {
        /// <summary>
        /// 文档编号
        /// </summary>
        public int DocID { get; set; }
        /// <summary>
        /// 得分
        /// </summary>
        public float Score { get; set; }
    }
}
