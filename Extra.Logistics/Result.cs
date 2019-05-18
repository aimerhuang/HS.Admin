using System;
using System.Collections.Generic;

namespace Extra.Logistics
{
    /// <summary>
    /// 基础返回json结果
    /// </summary>
    /// <remarks>
    /// 2013-03-12 杨浩 创建
    /// </remarks>
    [Serializable]
    public class Result 
    {
        /// <summary>
        /// 是否
        /// </summary>
        public bool Status;

        /// <summary>
        /// 状态代码
        /// </summary>
        public int StatusCode;

        /// <summary>
        /// 消息
        /// </summary>
        public String Message = string.Empty;

    }

    /// <summary>
    /// 带数据的json返回结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks> 2013-3-12 杨浩 创建 </remarks>
    public class Result<T> : Result
    {
        /// <summary>
        /// 数据
        /// </summary>
        public T Data;
    }

}