using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.BLL.Log;
using Hyt.DataAccess.Distribution;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.Distribution
{
    /// <summary>
    /// 代理商预存款业务类
    /// </summary>
    /// <remarks> 2016-04-14 刘伟豪 创建 </remarks>
    public class DsAgentPrePaymentBo : BOBase<DsAgentPrePaymentBo>
    {
        /// <summary>
        /// 创建代理商预存款
        /// </summary>
        /// <param name="model">代理商预存款主表实体</param>
        /// <returns>返回系统编号</returns>
        /// <remarks> 2016-04-14 刘伟豪 创建</remarks>
        public int Create(DsAgentPrePayment model)
        {
            var sysno = IDsAgentPrePaymentDao.Instance.Create(model);
            SysLog.Instance.Info(LogStatus.系统日志来源.后台, "创建代理商预存款", LogStatus.系统日志目标类型.代理商预存款, sysno);
            return sysno;
        }

        /// <summary>
        /// 获取代理商预存款实体
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <remarks> 2016-04-18 刘伟豪 创建</remarks>
        public DsAgentPrePayment GetModel(int sysNo)
        {
            return IDsAgentPrePaymentDao.Instance.GetModel(sysNo);
        }

        /// <summary>
        /// 根据代理商编号，获取预存款实体
        /// </summary>
        /// <param name="agentSysNo">代理商系统编号</param>
        /// <remarks> 2016-04-18 刘伟豪 创建</remarks>
        public DsAgentPrePayment GetDsAgentPrePayment(int agentSysNo)
        {
            return IDsAgentPrePaymentDao.Instance.GetDsAgentPrePayment(agentSysNo);
        }
    }
}