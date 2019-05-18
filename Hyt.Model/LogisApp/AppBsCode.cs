using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.LogisApp
{
    /// <summary>
    /// 码表内容
    /// </summary>
    public class AppBsCode
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 父级系统编号
        /// </summary>
        public int ParentSysNo { get; set; }
        /// <summary>
        /// 代码名称
        /// </summary>
        public string CodeName { get; set; }
    }
}
