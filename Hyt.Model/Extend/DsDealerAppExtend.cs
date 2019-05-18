using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hyt.Model.Extend
{
    /// <summary>
    /// 经销商应用扩展类
    /// </summary>
    /// <remarks>2016-7-2 杨浩 创建</remarks>
    public class DsDealerAppExtend:BaseEntity
    {
        /// <summary>
        /// 海关代码
        /// </summary>
        [Description("海关代码")]
        public string CustomerCode { get; set; }
        /// <summary>
        /// app 网关
        /// </summary>
        [Description("APP网关")]
        public string Gateway { get; set; }
    }
}
