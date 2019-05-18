using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Parameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.Stores
{
    /// <summary>
    /// 分销商保证金订单
    /// </summary>
    /// <remarks>2016-5-15 杨浩 创建</remarks>
    public abstract class IDsDealerBailOrderDao:DaoBase<IDsDealerBailOrderDao>
    {
        /// <summary>
        /// 根据客户编号获取保证金订单详情
        /// </summary>
        /// <param name="customerSysNo">客户编号</param>
        /// <returns></returns>
        public abstract DsDealerBailOrder GetDsDealerBailOrder(int customerSysNo);
        /// <summary>
        /// 获取保证金订单详情
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns></returns>
        public abstract DsDealerBailOrder GetModel(int sysNo);
        /// <summary>
        /// 更新保证金订单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public abstract int Update(DsDealerBailOrder model);
        /// <summary>
        /// 创建保证金订单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public abstract int Create(DsDealerBailOrder model);
        /// <summary>
        /// 更新保证金订单状态
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <param name="statusType">状态类型（0：状态 1:支付状态）</param>
        /// <param name="status">状态值</param>
        /// <returns></returns>
        public abstract int UpdateStatus(int sysNo,int statusType,int status);
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>分页</returns>
        public abstract Pager<CBDsDealerBailOrder> Query(ParaDsDealerBailOrderFilter filter);
    }
}
