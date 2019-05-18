using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 属性扩展类
    /// </summary>
    /// <remarks>2013-07-05 唐永勤 创建</remarks>    
    public  class CBPdAttribute : PdAttribute
    {
        /// <summary>
        /// 最后更新人
        /// </summary>
        public string LastUpdateUserName { get; set; }
    }
}
