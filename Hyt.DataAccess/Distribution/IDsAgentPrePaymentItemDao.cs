using System;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.Parameter;

namespace Hyt.DataAccess.Distribution
{
    /// <summary>
    /// 代理商预存款来往明细接口层
    /// </summary>
    /// <remarks> 2016-04-18 刘伟豪 创建 </remarks>
    public abstract class IDsAgentPrePaymentItemDao : DaoBase<IDsAgentPrePaymentItemDao>
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pager">分页实体</param>
        /// <param name="filter">查询条件</param>
        /// <remarks> 2016-04-18 刘伟豪 创建</remarks>
        public abstract void Search(ref Pager<CBDsAgentPrePaymentItem> pager, ParaDsAgentPrePaymentItemFilter filter);

        /// <summary>
        /// 新建代理商预存款来往明细
        /// </summary>
        /// <param name="model">代理商预存款来往明细实体</param>
        /// <remarks> 2016-04-18 刘伟豪 创建</remarks>
        public abstract int Create(DsAgentPrePaymentItem model);
    }
}