using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Finance;
using Hyt.DataAccess.Logistics;
using Hyt.DataAccess.Oracle.Finance;
using Hyt.DataAccess.Oracle.Warehouse;
using Hyt.DataAccess.RMA;
using Hyt.DataAccess.Warehouse;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Oracle
{
    /// <summary>
    /// DaoImpl结算单明细
    /// </summary>
    /// <remarks>2013-06-15 黄伟 创建</remarks>
    public class LgSettlementItemDaoImpl : ILgSettlementItemDao
    {
        /// <summary>
        /// 获取结算单明细
        /// </summary>
        /// <param name="settlementSysNo">结算单主表系统编号</param>
        /// <returns>结算单明细列表</returns>
        /// <remarks>2013-06-28 黄伟 创建</remarks>
        public override List<LgSettlementItem> GetLgSettlementItems(int settlementSysNo)
        {
            return Context.Sql(@"select *
                                from LgSettlementitem
                                where settlementsysno=@settlementsysno"
                )
                          .Parameter("settlementsysno", settlementSysNo)
                          .QueryMany<LgSettlementItem>();
        }

        /// <summary>
        /// 获取结算单明细
        /// </summary>
        /// <param name="sysNo">结算单明细系统编号</param>
        /// <returns>结算单明细</returns>
        /// <remarks>2013-06-28 黄伟 创建</remarks>
        public override LgSettlementItem GetLgSettlementItem(int sysNo)
        {
            return Context.Sql(@"select *
                                from LgSettlementitem
                                where sysno=@sysno"
                )
                          .Parameter("sysno", sysNo)
                          .QuerySingle<LgSettlementItem>();
        }

        /// <summary>
        /// 根据出库单号获取结算单明细
        /// </summary>
        /// <param name="stockOutSysNo">出库单系统编号</param>
        /// <returns>结算单明细</returns>
        /// <remarks>2013-08-22 黄志勇 创建</remarks>
        public override LgSettlementItem GetLgSettlementItemByStockOut(int stockOutSysNo)
        {
            return Context.Sql(@"select *
                                from LgSettlementitem
                                where StockOutSysNo=@stockOutSysNo order by CreatedDate desc"
                )
                          .Parameter("stockOutSysNo", stockOutSysNo)
                          .QuerySingle<LgSettlementItem>();
        }

        /// <summary>
        /// 获取结算单主表及其明细
        /// </summary>
        /// <param name="settlementSysNo">结算单主表系统编号</param>
        /// <returns>结算单明细列表</returns>
        /// <remarks>2013-07-04 黄伟 创建</remarks>
        public override LgSettlement GetLgSettlementWithItems(int settlementSysNo)
        {
            var lgSettlement = ILgSettlementDao.Instance.GetLgSettlement(settlementSysNo);
            //toget the DeliveryItemStatus
            //            var sql = @" select s.*,d.status as DeliveryItemStatus
            //                                from LgSettlementitem s
            //                                inner join lgdeliveryitem d on s.stockoutsysno=d.notesysno and d.notetype=10
            //                                where s.settlementsysno=:settlementsysno
            //                                 and d.status!=-10";
            if (!lgSettlement.Any())
                return null;

            var sql = @" select s.* 
                                from LgSettlementitem s
                                where s.settlementsysno=@settlementsysno
                                 ";
            lgSettlement[0].Items = Context.Sql(sql)
                                           .Parameter("settlementsysno", settlementSysNo)
                                           .QueryMany<LgSettlementItem>();
            return lgSettlement[0];
        }

        /// <summary>
        /// 获取出库单明细
        /// </summary>
        /// <param name="sysNo">结算单明细系统编号</param>
        /// <returns>出库单明细</returns>
        /// <remarks>2013-07-05 黄伟 创建</remarks>
        public override List<WhStockOutItem> GetWhStockOutDetails(int sysNo)
        {
            return Context.Sql(@"select o.* from whstockoutitem o
                                 where o.stockoutsysno=(select stockoutsysno from LgSettlementitem
                                                        where sysno=@sysno
                                 )")
                          .Parameter("sysno", sysNo)
                          .QueryMany<WhStockOutItem>();
        }

        /// <summary>
        /// 创建结算单明细
        /// </summary>
        /// <param name="mode">结算单明细实体.</param>
        /// <returns>返回创建的结算单明细的系统编号</returns>
        /// <remarks>
        /// 2013-07-12 何方 创建
        /// </remarks>
        public override int Create(LgSettlementItem mode)
        {
            if (mode.LastUpdateDate == DateTime.MinValue)
            {
                mode.LastUpdateDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            }
            var sysno = Context.Insert<LgSettlementItem>("LgSettlementItem", mode)
                               .AutoMap(p => p.SysNo, p => p.DeliveryItemStatus)
                               .ExecuteReturnLastId<int>("SysNo");
            return sysno;
        }

        /// <summary>
        /// 更新结算单明细
        /// </summary>
        /// <param name="item">结算单明细</param>
        /// <returns>受影响的行数</returns>
        /// <remarks>2013-8-21 黄伟 创建</remarks>
        public override int Update(LgSettlementItem item)
        {
            return Context.Update("LgSettlementItem", item)
                          .AutoMap(p => p.DeliveryItemStatus, p => p.SysNo)
                          .Where(p => p.SysNo).Execute();
        }

    }
}