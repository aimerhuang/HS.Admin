using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Kis
{
    /// <summary>
    /// 结果(Kis版)
    /// </summary>
    public class KisResult
    {
        /// <summary>
        /// 是否
        /// </summary>
        public bool success;

        /// <summary>
        /// 状态代码
        /// </summary>
        public int error_code;

        /// <summary>
        /// 消息
        /// </summary>
        public string message = string.Empty;
    }
    /// <summary>
    /// 返回结果(Kis版)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class KisResultT<T> : KisResult
    {
        /// <summary>
        /// 兼容Kis
        /// </summary>
        public T data { get; set; }
    }
}
