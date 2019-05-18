using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hyt.Model;
using Hyt.Model.SystemPredefined;

namespace Hyt.Admin
{
    /// <summary>
    /// Action权限特性
    /// </summary>
    /// <remarks>
    /// 2013-7-1 杨浩 创建
    /// </remarks>
    public class PrivilegeAttribute : Attribute
    {
        /// <summary>
        /// 授权列表
        /// </summary>
        public PrivilegeCode[] Allow { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="allow"></param>
        public PrivilegeAttribute(params PrivilegeCode[] allow)
        {
            this.Allow = allow;
        }
    }
}