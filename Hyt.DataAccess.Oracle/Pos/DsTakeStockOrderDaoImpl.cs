using Hyt.DataAccess.Pos;
using Hyt.Model.Pos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Pos
{
    public class DsTakeStockOrderDaoImpl : IDsTakeStockOrderDao
    {
        public override int Insert(Model.Pos.DsTakeStockOrder mod)
        {
            return Context.Insert("DsTakeStockOrder", mod).AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }

        public override void InsertItem(Model.Pos.DsTakeStockItem mod)
        {
            Context.Insert("DsTakeStockItem", mod).AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }
        /// <summary>
        /// 盘点单列表分页
        /// </summary>
        /// <param name="pager"></param>
        public override void GetTakeStockOrderListPagerByDsSysNo(ref Model.Pager<Model.Pos.CBDsTakeStockOrder> pager)
        {
            #region sql条件
            string sqlWhere = @"(DsTakeStockOrder.DsSysNo=@DsSysNo   or " + pager.PageFilter.DsSysNo + "=0 ) ";
            if (pager.PageFilter.DateTime!=null)
            {
                sqlWhere += " and (CheckTime>='" + pager.PageFilter.DateTime.Value.ToString("yyyy-MM-dd") + " 00:00:00' and CheckTime<='" + pager.PageFilter.DateTime.Value.ToString("yyyy-MM-dd") + " 23:59:59') ";
            }
            #endregion

            using (var context = Context.UseSharedConnection(true))
            {
                pager.Rows = context
                    .Select<CBDsTakeStockOrder>("DsTakeStockOrder.*,DsDealer.DealerName , TotalAmount=(select sum(PdPrice.Price*(DsTakeStockItem.ProNowNum-DsTakeStockItem.ProOldNum)) from DsTakeStockItem inner join PdPrice on PdPrice.ProductSysNo=DsTakeStockItem.ProductSysNo  and PdPrice.SourceSysNo=0  where DsTakeStockItem.PSysNo=DsTakeStockOrder.SysNo ) ")
                     .From(" DsTakeStockOrder inner join DsDealer on DsTakeStockOrder.DsSysNo=DsDealer.SysNo ")
                           .Where(sqlWhere)
                           .Parameter("DsSysNo", pager.PageFilter.DsSysNo)
                           .Paging(pager.CurrentPage, pager.PageSize)
                           .OrderBy("DsTakeStockOrder.SysNo desc")
                           .QueryMany();
                pager.TotalRows = context.Select<int>("count(1)")
                           .From(" DsTakeStockOrder ")
                           .Where(sqlWhere)
                           .Parameter("DsSysNo", pager.PageFilter.DsSysNo)
                           .QuerySingle();
            }
        }
        /// <summary>
        /// 盘点单信息
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public override DsTakeStockOrder GetTakeStockOrder(int SysNo)
        {
            string sql = "select * from DsTakeStockOrder where SysNo='" + SysNo + "'";
            return Context.Sql(sql).QuerySingle<DsTakeStockOrder>();
        }
        /// <summary>
        /// 盘点单明细
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public override List<CBDsTakeStockItem> GetTakeStockOrderItems(int SysNo)
        {
            string sql = " select DsTakeStockItem.*,PdPrice.Price as BasePrice from  DsTakeStockItem left join PdPrice on DsTakeStockItem.ProductSysNo=PdPrice.ProductSysNo and SourceSysNo=0  where PSysNo = '" + SysNo + "'";
            return Context.Sql(sql).QueryMany<CBDsTakeStockItem>();
        }
        /// <summary>
        /// 盘点单列表
        /// </summary>
        /// <param name="dsSysNo">经销商编号</param>
        /// <param name="datetime">时间</param>
        /// <returns></returns>
        public override List<CBDsTakeStockOrder> GetTakeStockOrderList(int? dsSysNo, DateTime? datetime)
        {
            string sql = " select DsTakeStockOrder.*,DsDealer.DealerName  from DsTakeStockOrder inner join DsDealer on DsTakeStockOrder.DsSysNo=DsDealer.SysNo ";
            string sqlWhere = "";
            if (dsSysNo!=null)
            {
                if(sqlWhere=="")
                {
                    sqlWhere += " where ";
                }
                else
                {
                    sqlWhere += " and ";
                }
                sqlWhere += " DsSysNo = " + dsSysNo.Value + " ";
            }
            if (datetime!=null)
            {
                if (sqlWhere == "")
                {
                    sqlWhere += " where ";
                }
                else
                {
                    sqlWhere += " and ";
                }
                sqlWhere += " ( CheckTime >= '" + datetime.Value.ToString("yyyy-MM-dd") + " 00:00:00' ";
                sqlWhere += " and CheckTime <= '" + datetime.Value.ToString("yyyy-MM-dd") + " 23:59:59' ) ";
            }
            sql = sql + sqlWhere;

            return Context.Sql(sql).QueryMany<CBDsTakeStockOrder>();
        }

        public override List<DsTakeStockItem> GettTakeStockItems(List<int> SysNos)
        {
            string sql = " select * from DsTakeStockItem where PSysNo in (" + string.Join(",",SysNos.ToArray()) + ") ";
            return Context.Sql(sql).QueryMany<DsTakeStockItem>();
        }
    }
}
