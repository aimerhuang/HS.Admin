using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Distribution
{
    /// <summary>
    /// 代理商接口层
    /// </summary>
    /// <remarks> 2016-04-13 刘伟豪 创建 </remarks>
    public abstract class IDsAgentDao : DaoBase<IDsAgentDao>
    {
        /// <summary>
        /// 分页查询代理商
        /// </summary>
        /// <param name="pager">分页</param>
        /// <param name="filter">查询条件</param>
        /// <remarks> 2016-04-13 刘伟豪 创建 </remarks>
        public abstract void Search(ref Pager<CBDsAgent> pager, ParaDsAgentFilter filter);

        /// <summary>
        /// 分页查询代理商资金列表
        /// </summary>
        /// <param name="pager">分页</param>
        /// <param name="filter">查询条件</param>
        /// <remarks> 2016-04-18 刘伟豪 创建 </remarks>
        public abstract void SeachAgentPrePayment(ref Pager<CBDsAgent> pager, ParaDsAgentFilter filter);

        /// <summary>
        /// 用于更新检查用户系统编号不重复，查询代理商信息，修改时用
        /// </summary>
        /// <param name="userSysNo">用户系统编号</param>
        /// <param name="sysNo">要排除的代理商系统编号</param>
        /// <remarks> 2016-04-14 刘伟豪 创建 </remarks>
        public abstract IList<CBDsAgent> List(int userSysNo, int sysNo);

        /// <summary>
        /// 用于新建检查用户系统编号不重复，查询代理商信息，新增时用
        /// </summary>
        /// <param name="userSysNo">用户系统编号</param>
        /// <remarks> 2016-04-14 刘伟豪 创建 </remarks>
        public abstract IList<CBDsAgent> List(int userSysNo);

        /// <summary>
        /// 根据名称查询代理商列表，用于判断名称唯一性，新增时用
        /// </summary>
        /// <param name="agentName">代理商名称</param>
        /// <remarks> 2016-04-14 刘伟豪 创建 </remarks>
        public abstract IList<CBDsAgent> List(string agentName);

        /// <summary>
        /// 根据名称查询代理商列表，用于判断名称唯一性，修改时用
        /// </summary>
        /// <param name="agentName">代理商名称</param>
        /// <param name="sysNo">要排除的代理商系统编号</param>
        /// <remarks> 2016-04-15 刘伟豪 创建 </remarks>
        public abstract IList<CBDsAgent> List(string agentName, int sysNo);

        /// <summary>
        /// 新建代理商
        /// </summary>
        /// <param name="model">代理商实体</param>
        /// <remarks> 2016-04-14 刘伟豪 创建 </remarks>
        public abstract int Create(DsAgent model);

        /// <summary>
        /// 获取代理商实体
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <remarks> 2016-04-14 刘伟豪 创建 </remarks>
        public abstract CBDsAgent GetDsAgent(int sysNo);

        /// <summary>
        /// 根据管理员系统编号，获取代理商实体
        /// </summary>
        /// <param name="userSysNo">管理员系统编号</param>
        /// <remarks> 2016-04-15 刘伟豪 创建 </remarks>
        public abstract CBDsAgent GetDsAgentByUserSysNo(int userSysNo);

            /// <summary>
        /// 修改代理商
        /// </summary>
        /// <param name="model">代理商实体</param>
        /// <remarks> 2016-04-15 刘伟豪 创建 </remarks>
        public abstract bool Update(DsAgent model);

        /// <summary>
        /// 修改代理商时，同时统一更新下级分销商等级
        /// </summary>
        /// <param name="agentSysNo">代理商系统编号</param>
        /// <param name="levelSysNo">分销商等级编号</param>
        /// <remarks> 2016-04-15 刘伟豪 创建 </remarks>
        public abstract bool UpdateDealersLevel(int agentSysNo, int levelSysNo);

        /// <summary>
        /// 代理商状态更新，同时修改关联账号状态,状态枚举相同
        /// </summary>
        /// <param name="sysNo">代理商系统编号</param>
        /// <param name="status">代理商状态</param>
        /// <param name="lastUpdateBy">最后更新人</param>
        /// <remarks> 2016-04-15 刘伟豪 创建 </remarks>
        public abstract bool UpdateStatus(int sysNo, DistributionStatus.代理商状态 status, int lastUpdateBy);
    }
}