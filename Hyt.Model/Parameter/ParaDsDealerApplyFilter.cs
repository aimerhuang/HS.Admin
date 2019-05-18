using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 联系人
    /// </summary>
    /// <remarks>
    /// 2016-04-16 王耀发 创建
    /// </remarks>
    public class ParaDsDealerApplyFilter
    {
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }  
        /// <summary>
        /// 联系人
        /// </summary>
        public string ContactName { get; set; }
        /// <summary>
        /// 联系方式
        /// </summary>
        public string ContactWay { get; set; }     
    }
}
