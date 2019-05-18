using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 分销商申请信息
    /// </summary>
    /// 2016-04-16 王耀发 创建 
    public class CBDsDealerApply : DsDealerApply
    {
        /// <summary>
        /// 处理人
        /// </summary>
        [Description("处理人")]
        public string HandlerName { get; set; }
    }
}
