using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.DataAccess.Order;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.Order
{
    /// <summary>
    /// 推送返回信息
    /// </summary>
    /// <remarks>
    /// 2015-09-05 王耀发 创建
    /// </remarks>
    public class SendOrderReturnBo : BOBase<SendOrderReturnBo>
    {

        #region 推送返回信息
        /// <summary>
        /// 推送返回信息
        /// </summary>
        /// <param name="filter">推送返回信息</param>
        /// <returns>返回推送返回信息</returns>
        /// <remarks>2015-09-05 王耀发 创建</remarks>
        public Pager<SendOrderReturn> GetSendOrderReturnList(ParaSendOrderReturnFilter filter)
        {
            return ISendOrderReturnDao.Instance.GetSendOrderReturnList(filter);
        }
        /// <summary>
        /// 根据快递公司编号，快递单号编号返回记录
        /// </summary>
        /// <param name="OverseaCarrier"></param>
        /// <param name="OverseaTrackingNo"></param>
        /// <returns></returns>
        /// <remarks>2015-09-05 王耀发 创建</remarks>
        public SendOrderReturn GetEntityByOversea(string OverseaCarrier, string OverseaTrackingNo)
        {
            return ISendOrderReturnDao.Instance.GetEntityByOversea(OverseaCarrier, OverseaTrackingNo);
        }

        /// <summary>
        /// 根据包裹单号返回记录
        /// </summary>
        /// <param name="OverseaCarrier"></param>
        /// <param name="OverseaTrackingNo"></param>
        /// <returns></returns>
        /// <remarks>2015-09-05 王耀发 创建</remarks>
        public SendOrderReturn GetEntityByOTrackingNo(string OverseaTrackingNo)
        {
            return ISendOrderReturnDao.Instance.GetEntityByOTrackingNo(OverseaTrackingNo);
        }

        /// <summary>
        /// 根据订单系统编号获取推送记录
        /// </summary>
        /// <param name="orderSysNo">订单系统编号</param>
        /// <remarks> 2016-5-11 刘伟豪 创建 </remarks>
        public SendOrderReturn GetEntityByOrderSysNo(int orderSysNo)
        {
            return ISendOrderReturnDao.Instance.GetEntityByOrderSysNo(orderSysNo);
        }
        #endregion
    }
}
