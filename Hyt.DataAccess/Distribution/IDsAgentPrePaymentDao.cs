using System;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Parameter;

namespace Hyt.DataAccess.Distribution
{
    /// <summary>
    /// 代理商预存款接口层
    /// </summary>
    /// <remarks> 2016-04-14 刘伟豪 创建 </remarks>
    public abstract class IDsAgentPrePaymentDao : DaoBase<IDsAgentPrePaymentDao>
    {
        /// <summary>
        /// 创建代理商预存款
        /// </summary>
        /// <param name="model">代理商预存款主表实体</param>
        /// <returns>返回系统编号</returns>
        /// <remarks> 2016-04-14 刘伟豪 创建</remarks>
        public abstract int Create(DsAgentPrePayment model);

        /// <summary>
        /// 获取代理商预存款实体
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <remarks> 2016-04-18 刘伟豪 创建</remarks>
        public abstract DsAgentPrePayment GetModel(int sysNo);

        /// <summary>
        /// 根据代理商编号，获取预存款实体
        /// </summary>
        /// <param name="agentSysNo">代理商系统编号</param>
        /// <remarks> 2016-04-18 刘伟豪 创建</remarks>
        public abstract DsAgentPrePayment GetDsAgentPrePayment(int agentSysNo);


        /// <summary>
        /// 减少 预存款可用余额.对应转入冻结金额
        /// </summary>
        /// <param name="agentSysNo">代理商系统编号.</param>
        /// <param name="cashValue">提现金额.</param>
        /// <param name="operatorSysNo">操作人sysno.</param>
        /// <remarks> 2016-04-18 刘伟豪 创建</remarks>
        public abstract bool SubtractAvailableAmount(int agentSysNo, decimal cashValue, int operatorSysNo);
    }
}