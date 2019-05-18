using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model.Transfer;

namespace Hyt.Model.Parameter
{
    /// <summary>
    /// 代理商查询参数
    /// </summary>
    /// <remarks> 2016-04-13 刘伟豪 创建 </remarks>
    public class ParaDsAgentFilter : CBDsAgent
    {
        /// <summary>
        /// 创建开始时间范围
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 创建结束时间范围
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 更新开始时间范围
        /// </summary>
        public DateTime? UpdateDateStartTime { get; set; }

        /// <summary>
        /// 更新结束时间范围
        /// </summary>
        public DateTime? UpdateDateEndTime { get; set; }

        /// <summary>
        /// 分页查询id
        /// </summary>
        public int? id { get; set; }

        /// <summary>
        /// 每页显示条数
        /// </summary>
        public int? PageSize { get; set; }

        /// <summary>
        /// 模糊查询关键字
        /// </summary>
        public string KeyWord { get; set; }

        /// <summary>
        /// 是否为代理商
        /// </summary>
        public bool IsAgent { get; set; }

        /// <summary>
        /// 是否为所有分销商
        /// </summary>
        public bool IsBindAllDealer { get; set; }

        /// <summary>
        /// 当前登录管理员系统编号
        /// </summary>
        public int? CurrentUserSysNo { get; set; }
    }
}