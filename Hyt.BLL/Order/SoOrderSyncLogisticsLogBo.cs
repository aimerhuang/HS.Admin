using Hyt.DataAccess.Order;
using Hyt.Model.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Order
{
    /// <summary>
    /// 订单同步物流日志
    /// </summary>
    /// <remarks>2016-7-29 杨浩 创建</remarks>
    public class SoOrderSyncLogisticsLogBo : BOBase<SoOrderSyncLogisticsLogBo>
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        public int Insert(SoOrderSyncLogisticsLog model)
        {
           return ISoOrderSyncLogisticsLogDao.Instance.Insert(model);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <param name="code">物流代码</param>
        /// <returns></returns>
        public int DeleteByOrderSysNoAndCode(int orderSysNo, int code)
        {
            return ISoOrderSyncLogisticsLogDao.Instance.DeleteByOrderSysNoAndCode(orderSysNo,code);
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        public int Update(SoOrderSyncLogisticsLog model)
        {
            return ISoOrderSyncLogisticsLogDao.Instance.Update(model);
        }
        /// <summary>
        /// 获取订单同步物流日志实体
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <param name="code">物流代码</param>
        /// <returns></returns>
        public  SoOrderSyncLogisticsLog GetModel(int orderSysNo, int code)
        {
            return ISoOrderSyncLogisticsLogDao.Instance.GetModel(orderSysNo,code);
        }

        public List<SoOrderSyncLogisticsLog> GetModelList(int orderSysno)
        {
            return ISoOrderSyncLogisticsLogDao.Instance.GetModelList(orderSysno);
        }
    }
}
