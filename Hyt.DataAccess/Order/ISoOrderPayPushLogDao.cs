using Hyt.DataAccess.Base;
using Hyt.Model.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Order
{
    /// <summary>
    /// 订单支付单推送日志
    /// </summary>
    /// <remarks>2017-08-14 杨浩 创建</remarks>
    public abstract class ISoOrderPayPushLogDao : DaoBase<ISoOrderPayPushLogDao>
    {
        /// <summary>
        /// 插入订单支付单日志
        /// </summary>
        /// <param name="item"></param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2017-08-14 杨浩 创建</remarks>
        public abstract SoOrderPayPushLog Insert(SoOrderPayPushLog item);

        /// <summary>
        /// 获取订单商品明细
        /// </summary>
        /// <param name="sysNo">订单编号</param>
        /// <returns></returns>
        /// <remarks>2017-08-14 杨浩 创建</remarks>
        public abstract IList<SoOrderPayPushLog> GetOrderPayPushLogList(int sysNo);
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ///  <remarks>2017-08-14 杨浩 创建</remarks>
        public abstract int Update(SoOrderPayPushLog model);
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="markId"></param>
        /// <returns></returns>
        /// <remarks>2017-08-15 杨浩 创建</remarks>
        public abstract SoOrderPayPushLog GetModel(string markId);
        /// <summary>
        /// 获取订单支付状态不是已支付的推送日志列表
        /// </summary>
        /// <param name="paymentTypeSysNo">支付类系统编号</param>
        /// <returns></returns>
        /// <remarks>2017-08-15 杨浩 创建</remarks>
        public abstract IList<SoOrderPayPushLog> GetOrderNoPayLog(int paymentTypeSysNo);
    }
}
