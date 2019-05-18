using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model.Manual;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 权限扩展
    /// </summary>
    /// <remarks>2014-01-13 ZTJ 添加注释</remarks>
   public  class CBSyPrivilege : SyPrivilege
    {
       /// <summary>
       /// 菜单编号
       /// </summary>
       public int MenuSysNo { get; set; }
    }
}
