using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// App推送服务扩展对象
    /// </summary>
    /// <remarks> 2014-01-14 邵斌 创建  </remarks>
    [Serializable]
    public class CBApPushService:ApPushService
    {
        /// <summary>
        /// 客户账号
        /// </summary>
        /// <remarks> 2014-01-14 邵斌 创建  </remarks>
        public string CustomerAccount { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        /// <remarks> 2014-01-14 邵斌 创建  </remarks>
        public string CustomerName { get; set; }

        /// <summary>
        /// 系统更新用户
        /// </summary>
        /// <remarks> 2014-01-14 邵斌 创建  </remarks>
        public string LastUpdateUser { get; set; }

        /// <summary>
        /// url参数
        /// </summary>
        /// <remarks> 2014-01-14 邵斌 创建  </remarks>
        public string Url { get; set; }
    }
}
