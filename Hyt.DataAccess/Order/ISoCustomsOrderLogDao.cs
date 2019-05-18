using Hyt.DataAccess.Base;
using Hyt.Model.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Order
{
    /// <summary>
    /// 海关订单日志
    /// </summary>
    /// <remarks>2016-1-2 杨浩 创建</remarks>
    public abstract class ISoCustomsOrderLogDao : DaoBase<ISoCustomsOrderLogDao>
    {
        /// <summary>
        /// 获取海关订单日志详情
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <param name="customsChannel">海关通道</param>
        /// <returns></returns>
        public abstract SoCustomsOrderLog GetCustomsOrderLogInfo(int orderSysNo, int customsChannel);
        /// <summary>
        /// 新增海关订单日志
        /// </summary>
        /// <param name="model">海关订单日志实体类</param>
        /// <returns></returns>
        public abstract int AddCustomsOrderLog(SoCustomsOrderLog model);
        /// <summary>
        /// 更新海关订单日志
        /// </summary>
        /// <param name="model">海关订单日志实体类</param>
        /// <returns></returns>
        public abstract int UpdateCustomsOrderLog(SoCustomsOrderLog model);
        /// <summary>
        /// 是否唯一海关订单日志
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <param name="customsChannel">海关通道</param>
        /// <returns></returns>
        public abstract int IsOnly(int orderSysNo, int customsChannel);

    }
}
