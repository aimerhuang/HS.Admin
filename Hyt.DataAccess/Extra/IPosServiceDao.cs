using Hyt.DataAccess.Base;
using Hyt.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Extra
{
    public abstract class IPosServiceDao : DaoBase<IPosServiceDao>
    {
        /// <summary>
        /// 减库存失败的订单将存放在此表中，等待再次扣减库存
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <remarks>2016-05-24 陈海裕 创建</remarks>
        public abstract int AddToReducedPOSInventoryQueue(ReducedPOSInventoryQueue entity);

        /// <summary>
        /// 根据订单编号查询队列表
        /// </summary>
        /// <param name="orderSysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-05-24 陈海裕 创建</remarks>
        public abstract ReducedPOSInventoryQueue GetEntityByOrderSysNo(int orderSysNo);
    }
}
