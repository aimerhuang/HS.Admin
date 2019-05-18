using Hyt.DataAccess.Pos;
using Hyt.Model.Pos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Pos
{
    public class DsPosOrderDaoImpl : IDsPosOrderDao
    {
        public override int Insert(Model.Pos.DsPosOrder order)
        {
           return  Context.Insert<Model.Pos.DsPosOrder>("DsPosOrder", order).AutoMap(p=>p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }

        public override void Update(Model.Pos.DsPosOrder order)
        {
            Context.Update<Model.Pos.DsPosOrder>("DsPosOrder", order).AutoMap(p => p.SysNo).Where(p => p.SysNo).Execute();
        }

        public override int InsertItem(Model.Pos.DsPosOrderItem item)
        {
            return Context.Insert<Model.Pos.DsPosOrderItem>("DsPosOrderItem", item).AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }

        public override void UpdateItem(Model.Pos.DsPosOrderItem item)
        {
            Context.Update<Model.Pos.DsPosOrderItem>("DsPosOrderItem", item).AutoMap(p => p.SysNo).Where(p => p.SysNo).Execute();
        }

        public override Model.Pos.DsPosOrder GetEntity(int sysNo)
        {
            string sql = "select * from DsPosOrder where SysNo = '"+sysNo+"'";
            return Context.Sql(sql).QuerySingle<Model.Pos.DsPosOrder>();
        }

        public override List<Model.Pos.DsPosOrderItem> GetEntityItems(int pSysNo)
        {
            string sql = "select * from DsPosOrderItem where pSysNo = '" + pSysNo + "'";
            return Context.Sql(sql).QueryMany<Model.Pos.DsPosOrderItem>();
        }

        public override List<Model.Pos.DsPosOrder> GetList(int dsSysNo)
        {
            string sql = "select * from DsPosOrder where DsSysNo = '" + dsSysNo + "' or " + dsSysNo + " = 0 ";
            return Context.Sql(sql).QueryMany<Model.Pos.DsPosOrder>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="posSysNo"></param>
        /// <param name="today"></param>
        /// <param name="DsSysNo"></param>
        /// <returns></returns>
        public override List<Model.Pos.DBDsPosOrder> GetPosOrderList(int posSysNo, DateTime startday, DateTime endday, int DsSysNo)
        {
            string sql = " select DsPosOrder.*, isnull(SpCoupon.CouponAmount,0.00)  as CouponAmount  from DsPosOrder left join SpCoupon on DsPosOrder.CouponSysNo=SpCoupon.SysNo where ((DsSysNo='" + DsSysNo + "' and DsPosSysNo='" + posSysNo + "') or " + DsSysNo + " = 0 )";
            sql += "  and SaleTime>='" + startday.ToString("yyyy-MM-dd") + " 00:00:00' and SaleTime<='" + endday.ToString("yyyy-MM-dd") + " 23:59:59' ";
            return Context.Sql(sql).QueryMany<Hyt.Model.Pos.DBDsPosOrder>();
        }

        public override void GetPosOrderListPagerByDsSysNo(ref Model.Pager<Model.Pos.DBDsPosOrder> pager)
        {
            #region sql条件
            string sqlWhere = @"(DsSysNo=@DsSysNo or " + pager.PageFilter.DsSysNo + " = 0  ) ";
            if (pager.PageFilter.BeginDate!=null)
            {
                sqlWhere += " and   SaleTime >='" + pager.PageFilter.BeginDate.Value.ToString("yyyy-MM-dd") + " 00:00:00' ";
            }
            if (pager.PageFilter.EndDate != null)
            {
                sqlWhere += " and   SaleTime <='" + pager.PageFilter.EndDate.Value.ToString("yyyy-MM-dd") + " 23:59:59' ";
            }
            if (pager.PageFilter.DsPosSysNo>0)
            {
                sqlWhere += " and   DsPosOrder.DsPosSysNo = " + pager.PageFilter.DsPosSysNo + " ";
            }
            #endregion

            using (var context = Context.UseSharedConnection(true))
            {
                pager.Rows = context.Select<DBDsPosOrder>("DsPosOrder.*,DsDealer.DealerName as StoreName ")
                           .From(" DsPosOrder inner join DsDealer on DsPosOrder.DsSysNo=DsDealer.SysNo ")
                           .Where(sqlWhere)
                           .Parameter("DsSysNo", pager.PageFilter.DsSysNo)
                           .Paging(pager.CurrentPage, pager.PageSize)
                           .OrderBy("DsPosOrder.SysNo desc")
                           .QueryMany();
                pager.TotalRows = context.Select<int>("count(1)")
                           .From("  DsPosOrder inner join DsDealer on DsPosOrder.DsSysNo=DsDealer.SysNo  ")
                           .Where(sqlWhere)
                           .Parameter("DsSysNo", pager.PageFilter.DsSysNo)
                           .QuerySingle();
            }
        }

        public override DsPosOrder GetEntityBySellNumber(int dsSysNo,string sellNumber)
        {
            string sql = "select * from DsPosOrder where SerialNumber = '" + sellNumber + "' and DsSysNo='" + dsSysNo + "' ";
            return Context.Sql(sql).QuerySingle<Model.Pos.DsPosOrder>();
        }

        public override List<DsPosOrderItem> GetEntityItems(List<int> soSysNoList)
        {
            string sql = "select * from DsPosOrderItem where pSysNo in (" +string.Join(",",soSysNoList.ToArray())+ ")";
            return Context.Sql(sql).QueryMany<Model.Pos.DsPosOrderItem>();
        }

        public override List<DBDsPosOrder> GetDBModList(int dsSysNo
            , DateTime? BeginDate, DateTime? EndDate)
        {
            string sql = "select DsPosOrder.*,DsDealer.DealerName from DsPosOrder inner join DsDealer on DsDealer.sysno=DsPosOrder.DsSysNo";
            string sqlWhere = "";
            if (dsSysNo > 0)
            {
                sqlWhere += " where  DsPosOrder.DsSysNo='" + dsSysNo + "'  ";
            }
            if (BeginDate != null)
            {
                if (sqlWhere=="")
                {
                    sqlWhere += " where ";
                }
                else
                {
                    sqlWhere += " and ";
                }
                sqlWhere += "    SaleTime >='" + BeginDate.Value.ToString("yyyy-MM-dd") + "  00:00:00' ";
            }
            if (EndDate != null)
            {
                if (sqlWhere == "")
                {
                    sqlWhere += " where ";
                }
                else
                {
                    sqlWhere += " and ";
                }
                sqlWhere += "    SaleTime <='" + EndDate.Value.ToString("yyyy-MM-dd") + " 23:59:59' ";
            }
            sql = sql + sqlWhere;
            return Context.Sql(sql).QueryMany<DBDsPosOrder>();
        }

        public override List<DsPosOrder> GetEntityBySellNumbers(int dsSysNo, List<string> orderNos)
        {
            string sql = " select * from DsPosOrder  where DsSysNo = '" + dsSysNo + "' and SerialNumber in ('" + string.Join("','", orderNos) + "') ";
            return Context.Sql(sql).QueryMany<DsPosOrder>();
        }

        public override List<DsPosOrder> GetRepeatOrderList()
        {
            string sql = "select * from DsPosOrder where SerialNumber in (select SerialNumber from DsPosOrder group by SerialNumber having count(1) >= 2)";
            return Context.Sql(sql).QueryMany<DsPosOrder>();
        }

        public override void DeleteRepeatBySysNo(int SysNo)
        {
            string sql = " delete from DsPosOrder where SysNo='" + SysNo + "' ";
            string sqlItem = " delete from DsPosOrderItem where pSysNo='" + SysNo + "' ";
            Context.Sql(sql).Execute();
            Context.Sql(sqlItem).Execute();
        }

        public override List<DBDsPosOrderItemData> GetPosOrderListByNoBindSale()
        {
            string sql = @" select DsPosOrder.*,
                                   DsPosOrderItem.ProName,
                                    DsPosOrderItem.ProBarCode ,
                                    DsPosOrderItem.ProPrice ,
                                    DsPosOrderItem.ProNum ,
                                    DsPosOrderItem.ProDiscount ,
                                    DsPosOrderItem.ProDisPrice ,
                                    DsPosOrderItem.ProTotalValue 
                            from DsPosOrder inner join  DsPosOrderItem on DsPosOrder.SysNo = DsPosOrderItem.pSysNo
                            where BindSaleDetail=0 ";
            return Context.Sql(sql).QueryMany<DBDsPosOrderItemData>();
        }

        public override void DeleteDsPosOrder(int SysNo)
        {
            string sql = " delete from DsPosOrder where SysNo='" + SysNo + "' ";
            string itemSql = " delete from DsPosOrderItem where DsPosOrderItem.pSysNo='" + SysNo + "' ";
            Context.Sql(sql).Execute();
            Context.Sql(itemSql).Execute();
        }

        public override List<WareDsPosOrder> GetPosOrderListByDate(DateTime dateTime)
        {
            string sql = " select DsPosOrder.*,DsDealerWharehouse.WarehouseSysNo from DsPosOrder inner join DsDealerWharehouse on DsDealerWharehouse.DealerSysNo=DsPosOrder.DsSysNo where SaleTime >='" + dateTime.ToString("yyyy-MM-dd") + " 00:00:00' ";
            return Context.Sql(sql).QueryMany<WareDsPosOrder>();
        }
    }
}
