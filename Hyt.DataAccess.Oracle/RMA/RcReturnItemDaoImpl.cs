using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.RMA;
using Hyt.Model;

namespace Hyt.DataAccess.Oracle.RMA
{
    /// <summary>
    /// 退换货商品明细
    /// </summary>
    /// <remarks>2013-08-07 朱成果 创建</remarks> 
    public class RcReturnItemDaoImpl : IRcReturnItemDao
    {
        /// <summary>
        /// 根据退换货编号获取退换货商品明细
        /// </summary>
        /// <param name="returnSysNo">退换货编号</param>
        /// <returns>退换货商品明细列表</returns>
        /// <remarks>2013-07-11 朱成果 创建</remarks>
        public override List<RcReturnItem> GetListByReturnSysNo(int returnSysNo)
        {
          return  Context.Sql("select * from RcReturnItem where ReturnSysNo=@ReturnSysNo")
                .Parameter("ReturnSysNo", returnSysNo).QueryMany<RcReturnItem>();
        }

        /// <summary>
        /// 插入退换货明细
        /// </summary>
        /// <param name="item">退换货明细实体</param>
        /// <returns>插入的编号</returns>
        /// <remarks>2013-07-11 朱成果 创建</remarks> 
        public override int Insert(RcReturnItem item)
        {
            var sysNO = Context.Insert("RcReturnItem", item)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
            return sysNO;
        }

        /// <summary>
        /// 删除退换货明细列表
        /// </summary>
        /// <param name="returnSysNo">退换货编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-07 朱成果 创建</remarks> 
        public override  void DeleteByReturnSysNo(int returnSysNo)
        {
            Context.Delete("RcReturnItem").Where("returnSysNo", returnSysNo).Execute();
        }

        /// <summary>
        /// 获取退换货明细
        /// </summary>
        /// <param name="orderNo">订单号</param>
        /// <returns>退换货明细列表</returns>
        /// <remarks>2013-08-07 朱成果 创建</remarks> 
        public override List<CBRmaReturnItem> GetListByOrder(int orderNo)
        {
            return Context.Sql(@"
select StockOutItemSysNo,RmaQuantity,t1.status as RMAStatus,t1.sysno as RMAID,t1.RmaType,t2.ProductQuantity
from RcReturnItem  t0
inner join RcReturn t1
on t0.returnsysno=t1.sysno
inner join WhStockOutItem t2
on t2.sysno=t0.stockoutitemsysno
where t1.ordersysno=@orderNo").Parameter("orderNo", orderNo).QueryMany<CBRmaReturnItem>();

        }
        
    }
}
