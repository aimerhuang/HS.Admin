using Hyt.DataAccess.Base;
using Hyt.Model.Generated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Order
{
    /// <summary>
    /// 订单支付日志
    /// </summary>
    /// <remarks>2017-04-2 杨浩 创建</remarks>
    public abstract class ISoOrderPayLogDao:DaoBase<ISoOrderPayLogDao>
    {
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="SysNo">系统编号</param>
        /// <returns></returns>
        ///<remarks>2017-04-2 杨浩 创建</remarks>
        public abstract SoOrderPayLog GetEntity(int sysNo);

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="SysNo">订单编号</param>
        /// <param name="paymentTypeSysNo">支付类型</param>
        /// <returns></returns>
        ///<remarks>2017-04-2 杨浩 创建</remarks>
        public abstract SoOrderPayLog GetOrderPayLogByOrderSysNo(int orderSysNo, int paymentTypeSysNo);

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="submitOrderNumber">提交单号</param>
        /// <param name="paymentTypeSysNo">支付类型</param>
        /// <returns></returns>
        ///<remarks>2017-04-2 杨浩 创建</remarks>
        public abstract SoOrderPayLog GetOrderPayLogBySubmitOrderNumber(int submitOrderNumber, int paymentTypeSysNo);

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="entity">销售单主表实体</param>
        /// <returns>订单支付日志实体（带编号）</returns>
        /// <remarks>2017-04-2 杨浩 创建</remarks>
        public abstract SoOrderPayLog InsertEntity(SoOrderPayLog entity);
        /// <summary>
        /// 更新状态值
        /// </summary>
        /// <param name="orderSysNo">销售单编号</param>
        /// <param name="paymentTypeSysNo">支付类型</param>
        /// <param name="status">状态值</param>
        /// <returns></returns>
        /// <remarks>2017-04-2 杨浩 创建</remarks>
        public abstract bool UpdateOrderPayLogStatus(int orderSysNo,int paymentTypeSysNo,int status);

        /// <summary>
        /// 更新订单支付日志
        /// </summary>
        /// <param name="soOrder">订单支付日志实体</param>
        /// <returns>t:成功 f:失败</returns>
        /// <remarks>2017-04-2 杨浩 创建</remarks>
        public abstract bool Update(SoOrderPayLog entity);
    }
}
