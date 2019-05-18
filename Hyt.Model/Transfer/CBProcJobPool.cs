using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 任务池处理
    /// </summary>
    /// <remarks>2014-01-13 ZTJ 添加注释</remarks>
    public class CBProcJobPool
    {   
        //是否锁定
        public bool IsLocked { get; set; }
        //是否禁用
        public bool IsDisabled { get; set; }
    }
}
