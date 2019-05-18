using Hyt.DataAccess.Stores;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Stores
{
    /// <summary>
    /// 保证金订单
    /// </summary>
    /// <remarks>2016-5-15 杨浩 创建</remarks>
    public class DsDealerBailOrderBo : BOBase<DsDealerBailOrderBo>
    {
        /// <summary>
        /// 根据客户编号获取保证金订单详情
        /// </summary>
        /// <param name="customerSysNo">客户编号</param>
        /// <returns></returns>
        public  DsDealerBailOrder GetDsDealerBailOrder(int customerSysNo)
        {
            return IDsDealerBailOrderDao.Instance.GetDsDealerBailOrder(customerSysNo);
        }
        /// <summary>
        /// 获取保证金订单详情
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns></returns>
        public  DsDealerBailOrder GetModel(int sysNo)
        {
              return IDsDealerBailOrderDao.Instance.GetModel(sysNo);
        }
        /// <summary>
        /// 更新保证金订单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public  int Update(DsDealerBailOrder model)
        {
            return IDsDealerBailOrderDao.Instance.Update(model);
        }
        /// <summary>
        /// 创建保证金订单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Create(DsDealerBailOrder model)
        {
            return IDsDealerBailOrderDao.Instance.Create(model);
        }
        /// <summary>
        /// 更新保证金订单状态
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <param name="statusType">状态类型（0：状态 1:支付状态）</param>
        /// <param name="status">状态值</param>
        /// <returns></returns>
        public int UpdateStatus(int sysNo, int statusType, int status)
        {
            return IDsDealerBailOrderDao.Instance.UpdateStatus(sysNo,statusType,status);
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>分页</returns>
        public Pager<CBDsDealerBailOrder> Query(ParaDsDealerBailOrderFilter filter)
        {
            return IDsDealerBailOrderDao.Instance.Query(filter);
        }
    }
}
