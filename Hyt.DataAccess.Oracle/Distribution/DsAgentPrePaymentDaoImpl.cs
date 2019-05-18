using System;
using Hyt.Model;
using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.DataAccess.Distribution;

namespace Hyt.DataAccess.Oracle.Distribution
{
    /// <summary>
    /// 代理商预存款数据层
    /// </summary>
    /// <remarks> 2016-04-14 刘伟豪 创建 </remarks>
    public class DsAgentPrePaymentDaoImpl : IDsAgentPrePaymentDao
    {
        /// <summary>
        /// 创建代理商预存款
        /// </summary>
        /// <param name="model">代理商预存款主表实体</param>
        /// <returns>返回系统编号</returns>
        /// <remarks> 2016-04-14 刘伟豪 创建</remarks>
        public override int Create(DsAgentPrePayment entity)
        {
            entity.SysNo = Context.Insert("DsAgentPrePayment", entity)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
            return entity.SysNo;
        }

        /// <summary>
        /// 获取代理商预存款实体
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <remarks> 2016-04-18 刘伟豪 创建</remarks>
        public override DsAgentPrePayment GetModel(int sysNo)
        {
            const string sql = @"Select * From DsAgentPrePayment Where SysNo=@0";
            return Context.Sql(sql, sysNo)
                          .QuerySingle<DsAgentPrePayment>();
        }

        /// <summary>
        /// 根据代理商编号，获取预存款实体
        /// </summary>
        /// <param name="agentSysNo">代理商系统编号</param>
        /// <remarks> 2016-04-18 刘伟豪 创建</remarks>
        public override DsAgentPrePayment GetDsAgentPrePayment(int agentSysNo)
        {
            const string sql = @"Select * From DsAgentPrePayment Where AgentSysNo=@0";
            return Context.Sql(sql, agentSysNo)
                          .QuerySingle<DsAgentPrePayment>();
        }

        /// <summary>
        /// 减少 预存款可用余额.对应转入冻结金额
        /// </summary>
        /// <param name="agentSysNo">代理商系统编号.</param>
        /// <param name="cashValue">提现金额.</param>
        /// <param name="operatorSysNo">操作人sysno.</param>
        /// <remarks> 2016-04-18 刘伟豪 创建</remarks>
        public override bool SubtractAvailableAmount(int agentSysNo, decimal cashValue, int operatorSysNo)
        {
            var res = Context.Sql(@"update DsAgentPrePayment set AvailableAmount = (AvailableAmount-@0),FrozenAmount=(FrozenAmount+@0),LastUpdateBy=@1,LastUpdateDate=@2
                                        where AvailableAmount >=@0
                                        and AgentSysNo=@3", Math.Abs(cashValue), operatorSysNo, DateTime.Now, agentSysNo).Execute();
            return res > 0;
        }
    }
}