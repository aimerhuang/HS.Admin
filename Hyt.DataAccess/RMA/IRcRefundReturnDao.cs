using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.WorkflowStatus;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.RMA
{
    /// <summary>
    /// 退款
    /// </summary>
    public abstract class IRcRefundReturnDao : DaoBase<IRcRefundReturnDao>
    {
        /// <summary>
        /// 更新退款数据
        /// </summary>
        /// <param name="entity">退换货实体</param>
        /// <returns></returns>
        /// <remarks>2016-08-26 罗远康 创建</remarks>
        public abstract void Update(RcRefundReturn entity);

        /// <summary>
        /// 获取退款单实体
        /// </summary>
        /// <param name="sysNo">退换货单编号</param>
        /// <returns>退换货单实体</returns>
        /// <remarks>2016-08-26 罗远康 创建</remarks>
        public abstract RcRefundReturn GetEntity(int sysNo);


        /// <summary>
        /// 根据订单获取退款单实体（已经审核待付款）
        /// </summary>
        /// <param name="OrderSysNo">订单编号</param>
        /// <returns>退换货单实体</returns>
        /// <remarks>2016-08-26 罗远康 创建</remarks>
        public abstract RcRefundReturn GetOrderEntity(int OrderSysNo);

        /// <summary>
        /// 退款单分页查询
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns>退换货表</returns>
        /// <remarks>2016-08-26 罗远康 创建</remarks>
        public abstract Pager<RcRefundReturn> GetAll(ParaRefundFilter filter);
    }
}
