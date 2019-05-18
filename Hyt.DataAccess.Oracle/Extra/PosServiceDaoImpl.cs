using Hyt.DataAccess.Extra;
using Hyt.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Extra
{
    public class PosServiceDaoImpl : IPosServiceDao
    {
        /// <summary>
        /// 减库存失败的订单将存放在此表中，等待再次扣减库存
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <remarks>2016-05-24 陈海裕 创建</remarks>
        public override int AddToReducedPOSInventoryQueue(ReducedPOSInventoryQueue entity)
        {
            return Context.Insert("ReducedPOSInventoryQueue", entity).AutoMap(o => o.SysNo).Execute();
        }

        /// <summary>
        /// 根据订单编号查询队列表
        /// </summary>
        /// <param name="orderSysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-05-24 陈海裕 创建</remarks>
        public override ReducedPOSInventoryQueue GetEntityByOrderSysNo(int orderSysNo)
        {
            return Context.Sql("SELECT * FROM ReducedPOSInventoryQueue WHERE OrderSysNo=@0", orderSysNo)
                       .QuerySingle<ReducedPOSInventoryQueue>();
        }
    }
}
