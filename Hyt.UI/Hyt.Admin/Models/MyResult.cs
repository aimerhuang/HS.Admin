using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hyt.Admin.Models
{
    /// <summary>
    /// 基础返回json结果
    /// </summary>
    /// <remarks>2017-08-28 罗勤尧 创建</remarks>
    public class MyResult
    {
        /// <summary>
        /// 状态
        /// 公约{
        /// 未登录：-1
        /// 成功：1
        /// 失败：0
        /// }
        /// </summary>
        public int Status;
        /// <summary>
        /// 消息
        /// </summary>
        public string Message;

    }

    /// <summary>
    /// 带数据的json返回结果
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    /// <remarks>2017-08-28 罗勤尧 创建</remarks>
    public class MyResult<T> : MyResult
    {
        /// <summary>
        /// 数据
        /// </summary>
        public T Data;
    }
}