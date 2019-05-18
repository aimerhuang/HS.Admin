using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 自定义异常处理
    /// </summary>
    /// <remarks>
    /// 2013-06-20 吴文强 创建
    /// </remarks>
    [Serializable]
    public class HytException : System.Exception
    {
        /// <summary>
        ///  初始化 HytException 类的新实例。
        /// </summary>
        /// <returns></returns>
        /// <remarks>2013-06-20 吴文强 创建</remarks>
        public HytException()
        {
        }

        /// <summary>
        ///  使用指定的错误消息初始化 HytException 类的新实例。
        /// </summary>
        /// <param name="message">描述错误的消息。</param>
        /// <returns></returns>
        /// <remarks>2013-06-20 吴文强 创建</remarks>
        public HytException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///  使用指定的错误消息初始化 HytException 类的新实例。
        /// </summary>
        /// <param name="messageFormat">描述错误的消息模板</param>
        /// <param name="args">描述错误的消息参数</param>
        /// <returns></returns>
        /// <remarks>2013-06-20 吴文强 创建</remarks>
        public HytException(string messageFormat, params object[] args)
            : base(string.Format(messageFormat, args))
        {
        }

        /// <summary>
        /// 用序列化数据初始化 HytException 类的新实例。
        /// </summary>
        /// <param name="info">它存有有关所引发异常的序列化的对象数据。</param>
        /// <param name="context">它包含有关源或目标的上下文信息。</param>
        /// <returns></returns>
        /// <remarks>2013-06-20 吴文强 创建</remarks>
        public HytException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        ///  使用指定错误消息和对作为此异常原因的内部异常的引用来初始化 HytException 类的新实例。
        /// </summary>
        /// <param name="message">解释异常原因的错误消息。</param>
        /// <param name="innerException">导致当前异常的异常；如果未指定内部异常，则是一个 null 引用。</param>
        /// <returns></returns>
        /// <remarks>2013-06-20 吴文强 创建</remarks>
        public HytException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
