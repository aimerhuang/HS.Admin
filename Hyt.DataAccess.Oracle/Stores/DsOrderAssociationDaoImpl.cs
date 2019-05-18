using Hyt.DataAccess.Stores;
using Hyt.Model.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Stores
{
    /// <summary>
    /// 经销商订单与系统订单关联
    /// </summary>
    /// <remarks>2016-9-7 杨浩 创建</remarks>
    public class DsOrderAssociationDaoImpl : IDsOrderAssociationDao
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override bool Add(DsOrderAssociation model)
        {
            var result = Context.Insert<DsOrderAssociation>("DsOrderAssociation",model)
                                  .AutoMap()
                                  .Execute();
            return result>0;
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override int Update(DsOrderAssociation model)
        {
            int rowsAffected = Context.Update<DsOrderAssociation>("DsOrderAssociation", model)
                                     .AutoMap()
                                     .Where(x => x.DealerOrderNo)
                                     .Where(x=>x.DealerOrderNo)
                                     .Execute();
            return rowsAffected;
        }
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="orderSysNo">系统订单编号</param>
        /// <param name="dealerSysNo">经销商系统编号</param>
        /// <returns></returns>
        public override DsOrderAssociation GetOrderAssociationInfo(int dealerSysNo, string dealerOrderNo)
        {
            return Context.Sql("select * from DsOrderAssociation where DealerSysNo=@DealerSysNo and dealerOrderNo=@dealerOrderNo")
                .Parameter("DealerSysNo", dealerSysNo)
                .Parameter("dealerOrderNo", dealerOrderNo)
                .QuerySingle<DsOrderAssociation>();
        }
    }
}
