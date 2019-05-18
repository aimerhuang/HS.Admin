using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Model.Parameter;

namespace Hyt.DataAccess.Order
{
    /// <summary>
    /// 取国家数据访问类
    /// </summary>
    /// <remarks>
    /// 2015-08-26 王耀发 创建
    public abstract class ISendOrderReturnDao : Hyt.DataAccess.Base.DaoBase<ISendOrderReturnDao>
    {
        /// <summary>
        /// 推送返回信息
        /// </summary>
        /// <param name="filter">推送返回信息</param>
        /// <returns>返回推送返回信息</returns>
        /// <remarks>2015-09-05 王耀发 创建</remarks>
        public abstract Pager<SendOrderReturn> GetSendOrderReturnList(ParaSendOrderReturnFilter filter);

        /// <summary>
        /// 根据快递公司编号，快递单号编号返回记录
        /// </summary>
        /// <param name="OverseaCarrier"></param>
        /// <param name="OverseaTrackingNo"></param>
        /// <returns></returns>
        /// <remarks>2015-09-05 王耀发 创建</remarks>
        public abstract SendOrderReturn GetEntityByOversea(string OverseaCarrier, string OverseaTrackingNo);

        /// <summary>
        /// 根据包裹单号返回记录
        /// </summary>
        /// <param name="OverseaCarrier"></param>
        /// <param name="OverseaTrackingNo"></param>
        /// <returns></returns>
        /// <remarks>2015-09-05 王耀发 创建</remarks>
        public abstract SendOrderReturn GetEntityByOTrackingNo(string OverseaTrackingNo);

        /// <summary>
        /// 根据订单系统编号获取推送记录
        /// </summary>
        /// <param name="orderSysNo">订单系统编号</param>
        /// <remarks> 2016-5-11 刘伟豪 创建 </remarks>
        public abstract SendOrderReturn GetEntityByOrderSysNo(int orderSysNo);
    }
}