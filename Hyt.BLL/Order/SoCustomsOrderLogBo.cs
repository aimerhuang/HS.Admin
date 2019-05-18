using Hyt.Model.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Order
{
    /// <summary>
    /// 海关订单日志
    /// </summary>
    /// <remarks>2016-1-2 杨浩 创建</remarks>
    public class SoCustomsOrderLogBo : BOBase<SoCustomsOrderLogBo>
    {
        /// <summary>
        /// 获取海关订单日志详情
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <param name="customsChannel">海关通道</param>
        /// <returns></returns>
        public  SoCustomsOrderLog GetCustomsOrderLogInfo(int orderSysNo, int customsChannel)
        {
            return Hyt.DataAccess.Order.ISoCustomsOrderLogDao.Instance.GetCustomsOrderLogInfo(orderSysNo, customsChannel);
        }
        /// <summary>
        /// 新增海关订单日志
        /// </summary>
        /// <param name="model">海关订单日志实体类</param>
        /// <returns></returns>
        public int AddCustomsOrderLog(SoCustomsOrderLog model)
        {
            return Hyt.DataAccess.Order.ISoCustomsOrderLogDao.Instance.AddCustomsOrderLog(model);
        }
        /// <summary>
        /// 更新海关订单日志
        /// </summary>
        /// <param name="model">海关订单日志实体类</param>
        /// <returns></returns>
        public int UpdateCustomsOrderLog(SoCustomsOrderLog model)
        {
            return Hyt.DataAccess.Order.ISoCustomsOrderLogDao.Instance.UpdateCustomsOrderLog(model);
        }
        /// <summary>
        /// 是否唯一海关订单日志
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <param name="customsChannel">海关通道</param>
        /// <returns>存在返回true</returns>
        public bool IsOnly(int orderSysNo, int customsChannel)
        {   
            return Hyt.DataAccess.Order.ISoCustomsOrderLogDao.Instance.IsOnly(orderSysNo,customsChannel)>0;
        }
    }
}
