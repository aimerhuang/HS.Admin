using Hyt.DataAccess.Pos;
using Hyt.Model.Pos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Pos
{
    public class DsPosReturnOrderDaoImpl : IDsPosReturnOrderDao
    {
        public override int Insert(Model.Pos.DsPosReturnOrder rOrder)
        {
            return Context.Insert<DsPosReturnOrder>("DsPosReturnOrder", rOrder)
                .AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }

        public override int InsertItem(Model.Pos.DsPosReturnOrderItem rOrderItem)
        {
            return Context.Insert<DsPosReturnOrderItem>("DsPosReturnOrderItem", rOrderItem)
                .AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }

        public override void Update(Model.Pos.DsPosReturnOrder rOrder)
        {
            Context.Update<DsPosReturnOrder>("DsPosReturnOrder", rOrder)
                .AutoMap(p => p.SysNo).Where(p => p.SysNo).Execute();
        }

        public override void UpdateItem(Model.Pos.DsPosReturnOrderItem rOrderItem)
        {
            Context.Update<DsPosReturnOrderItem>("DsPosReturnOrderItem", rOrderItem)
                .AutoMap(p => p.SysNo).Where(p => p.SysNo).Execute();
        }

        public override Model.Pos.DsPosReturnOrder GetEntity(int SysNo)
        {
            string sql = "select * from DsPosReturnOrder where SysNo = '" + SysNo + "' ";
            return Context.Sql(sql).QuerySingle<DsPosReturnOrder>();
        }

        public override List<Model.Pos.CBDsPosReturnOrderItem> GetEntityItemList(int pSysNo)
        {
            string sql = "select DsPosReturnOrderItem.*,PdProduct.ProductName,PdProduct.Barcode as CodeBar  from DsPosReturnOrderItem left join PdProduct on DsPosReturnOrderItem.ProSysNo=PdProduct.SysNo where pSysNo = '" + pSysNo + "' ";
            return Context.Sql(sql).QueryMany<CBDsPosReturnOrderItem>();
        }
        /// <summary>
        /// 当天退货单数据集合
        /// </summary>
        /// <param name="posSysNo">pos机编号</param>
        /// <param name="today">今天时间</param>
        /// <param name="DsSysNo">经销商编号</param>
        /// <returns></returns>
        public override List<DsPosReturnOrder> GetPosReturnOrderList(int posSysNo, DateTime startday, DateTime endday, int DsSysNo)
        {
            string sql = "select DsPosReturnOrder.* from DsPosReturnOrder inner join DsPosOrder on DsPosReturnOrder.OrderSysNo=DsPosOrder.SysNo ";
            sql += " where DsPosSysNo='" + posSysNo + "' and (DsSysNo='" + DsSysNo + "' or " + DsSysNo + "=0 )  and  ReturnTime>='" + startday.ToString("yyyy-MM-dd") + " 00:00:00'  and  ReturnTime<='" + endday.ToString("yyyy-MM-dd") + " 23:59:59'";
            return Context.Sql(sql).QueryMany<DsPosReturnOrder>();
        }

        public override void GetPosReturnOrderListPagerByDsSysNo(ref Model.Pager<CBDsPosReturnOrder> pager)
        {
            #region sql条件
            string sqlWhere = @"(DsPosOrder.DsSysNo=@DsSysNo   or " + pager.PageFilter.DsSysNo + "=0) ";
            if (!string.IsNullOrEmpty(pager.PageFilter.PosName))
            {
                sqlWhere += " and  DsPosOrder.DsPosSysNo = '" + pager.PageFilter.PosName + "' ";
            }
            #endregion

            using (var context = Context.UseSharedConnection(true))
            {
                pager.Rows = context
                    .Select<CBDsPosReturnOrder>("DsPosReturnOrder.*,DsPosOrder.SerialNumber as SellOrderNumber,DsPosOrder.DsSysNo ,DsDealer.DealerName as StoreName")
                     .From(" DsPosReturnOrder inner join DsPosOrder on DsPosReturnOrder.OrderSysNo=DsPosOrder.SysNo inner join DsDealer on DsPosOrder.DsSysNo=DsDealer.SysNo ")
                           .Where(sqlWhere)
                           .Parameter("DsSysNo", pager.PageFilter.DsSysNo)
                           .Paging(pager.CurrentPage, pager.PageSize)
                           .OrderBy("DsPosReturnOrder.SysNo desc")
                           .QueryMany();
                pager.TotalRows = context.Select<int>("count(1)")
                           .From(" DsPosReturnOrder inner join DsPosOrder on DsPosReturnOrder.OrderSysNo=DsPosOrder.SysNo ")
                           .Where(sqlWhere)
                           .Parameter("DsSysNo", pager.PageFilter.DsSysNo)
                           .QuerySingle();
            }
        }

        public override CBDsPosReturnOrder GetCBPosReturnOrder(int SysNo)
        {
            string sql = "select DsPosReturnOrder.* from DsPosReturnOrder inner join DsPosOrder on DsPosReturnOrder.OrderSysNo=DsPosOrder.SysNo ";
            sql += " where DsPosReturnOrder.SysNo='" + SysNo + "' ";
            return Context.Sql(sql).QuerySingle<CBDsPosReturnOrder>();
        }

        public override List<DsPosReturnOrder> GetAllReturnOrder()
        {
            string sql = "  select * from DsPosReturnOrder  ";
            return Context.Sql(sql).QueryMany<DsPosReturnOrder>();
        }
        public override  List<DsPosReturnOrderItem> GetAllReturnOrderItem()
        {
            string sql = "  select * from DsPosReturnOrderItem  ";
            return Context.Sql(sql).QueryMany<DsPosReturnOrderItem>();
        }
    }
}
