using Hyt.DataAccess.Order;
using Hyt.Model.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Order
{
    /// <summary>
    /// 订单支付单推送日志
    /// </summary>
    /// <remarks>2017-08-14 杨浩 创建</remarks>
    public class SoOrderPayPushLogBo : BOBase<SoOrderPayPushLogBo>
    {
        /// <summary>
        /// 插入订单支付单日志
        /// </summary>
        /// <param name="item"></param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2017-08-14 杨浩 创建</remarks>
        public SoOrderPayPushLog Insert(SoOrderPayPushLog entity)
        {
            return ISoOrderPayPushLogDao.Instance.Insert(entity);
        }

        /// <summary>
        /// 获取订单商品明细
        /// </summary>
        /// <param name="sysNo">订单编号</param>
        /// <returns></returns>
        /// <remarks>2017-08-14 杨浩 创建</remarks>
        public  IList<SoOrderPayPushLog> GetOrderPayPushLogList(int sysNo)
        {
            return ISoOrderPayPushLogDao.Instance.GetOrderPayPushLogList(sysNo);
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ///  <remarks>2017-08-14 杨浩 创建</remarks>
        public int Update(SoOrderPayPushLog entity)
        {
            return ISoOrderPayPushLogDao.Instance.Update(entity);
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="markId"></param>
        /// <returns></returns>
        /// <remarks>2017-08-15 杨浩 创建</remarks>
        public  SoOrderPayPushLog GetModel(string markId)
        {
            return ISoOrderPayPushLogDao.Instance.GetModel(markId);
        }

        /// <summary>
        /// 获取订单支付状态不是已支付的推送日志列表
        /// </summary>
        /// <param name="paymentTypeSysNo">支付类系统编号</param>
        /// <returns></returns>
        /// <remarks>2017-08-15 杨浩 创建</remarks>
        public  IList<SoOrderPayPushLog> GetOrderNoPayLog(int paymentTypeSysNo)
        {
            return ISoOrderPayPushLogDao.Instance.GetOrderNoPayLog(paymentTypeSysNo);
        }
    }
}
