using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 物流刷单日志表
    /// </summary>
    [Serializable]
    public class MkExpressLog
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }

        /// <summary>
        /// 物流编号
        /// </summary>
        public string ExpressNo { get; set; }

        /// <summary>
        /// 日志内容  
        /// </summary>
        public string LogContent { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public string OperatorName { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperateDate { get; set; }
    }
}
