using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Util.JPush
{
    /// <summary>
    /// 返回结果集
    /// </summary>
    /// <remarks>2014-01-17 邵斌 创建</remarks>
    public class PushResult
    {
        /// <summary>
        /// 返回代码
        /// </summary>
        /// <remarks>2014-01-17 邵斌 创建</remarks>
        public int Code { get; set; }

        /// <summary>
        /// 返回信息
        /// </summary>
        /// <remarks>2014-01-17 邵斌 创建</remarks>
        public string Message { get; set; }
    }
}
