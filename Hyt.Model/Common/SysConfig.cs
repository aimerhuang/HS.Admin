using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Common
{
    /// <summary>
    /// 系统配置
    /// </summary>
    /// <remarks>2014-1-20 黄波 创建</remarks>
    public class SysConfig : ConfigBase
    {
        /// <summary>
        /// 在线支付对账日志目录
        /// </summary>
        public string PaySyscLogDir { get; set; }
    }
}
