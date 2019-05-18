using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.DouShabaoModel
{
    /// <summary>
    /// 豆沙包接口返回
    /// </summary>
    public class DouShabaoResultMessage
    {
        /// <summary>
        /// 处理结果代码
        /// </summary>
        public string ReturnCode { get; set; }
        /// <summary>
        /// 处理结果说明
        /// </summary>
        public string ResultMsg { get; set; }
    }
}
