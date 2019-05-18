using Hyt.DataAccess.Procurement;
using Hyt.Model.Procurement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Procurement
{
    public class PmPointsOrderDaoImpl : IPmPointsOrderDao
    {
        /// <summary>
        /// 创建分货单
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public override int CreatePointsOrder(Model.Procurement.PmPointsOrder order)
        {
            return Context.Insert<PmPointsOrder>("PmPointsOrder", order).AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }
        /// <summary>
        /// 修改分货单
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public override void UpdatePointsOrder(Model.Procurement.PmPointsOrder order)
        {
            Context.Update<PmPointsOrder>("PmPointsOrder", order).AutoMap(p => p.SysNo).Where(p=>p.SysNo).Execute();
        }
        /// <summary>
        /// 创建分货单明细
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public override int CreatePointsOrderItem(PmPointsOrderItem order)
        {
            return Context.Insert<PmPointsOrderItem>("PmPointsOrderItem", order).AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }
        /// <summary>
        /// 修改分货单明细
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public override void UpdatePointsOrderItem(PmPointsOrderItem order)
        {
            Context.Update<PmPointsOrderItem>("PmPointsOrderItem", order).AutoMap(p => p.SysNo).Where(p => p.SysNo).Execute();
        }

        /// <summary>
        /// 获取分货单信息
        /// </summary>
        /// <param name="PSysNo"></param>
        /// <returns></returns>
        public override List<CBPmPointsOrderItem> GetPointsOrderItems(int PSysNo)
        {
            string sql = "";
            sql = "select PmPointsOrderItem.* ,PmProcurementOrderItem.Poi_PSysNo,PmProcurementOrderItem.Poi_ProSysNo,PmProcurementOrderItem.Poi_ProName,PmProcurementOrderItem.Poi_TradePrice,PmProcurementOrderItem.Poi_JoinPrice,PmProcurementOrderItem.Poi_SalePrice,PmProcurementOrderItem.Poi_DisInfo,PmProcurementOrderItem.Poi_ProQuity" +
                        ",'' as Cb_Spec,PdProduct.ProductName as Cb_ProName,PdProduct.SalesMeasurementUnit as Cb_Unit " +
                " from PmProcurementOrderItem inner join PdProduct on PdProduct.SysNo=PmProcurementOrderItem.Poi_ProSysNo inner join PmPointsOrderItem on PmPointsOrderItem.Poi_ProcurementItemSysNo=PmProcurementOrderItem.SysNo"+
                " where PmPointsOrderItem.Poi_PSysNo='" + PSysNo + "'";
            return Context.Sql(sql).QueryMany<CBPmPointsOrderItem>();
        }

        /// <summary>
        /// 采购分货单
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public override CBPmPointsOrder GetPmPointsOrder(int SysNo)
        {
            string sql = " select PmPointsOrder.*,a.UserName as CreateName,b.UserName as UpdateName "+
                " from PmPointsOrder left join SyUser a on a.SysNo=PmPointsOrder.Po_CreateSysNo left join SyUser b on b.SysNo=PmPointsOrder.Po_UpdateSysNo  " +
                " where PmPointsOrder.SysNo = '" + SysNo + "' ";
            return Context.Sql(sql).QuerySingle<CBPmPointsOrder>();
        }

        public override void GetPmPointsOrderPager(ref Model.Pager<CBPmPointsOrder> pager)
        {
            string sqlTable = " PmPointsOrder left join SyUser a on PmPointsOrder.Po_CreateSysNo=a.SysNo left join SyUser b on PmPointsOrder.Po_UpdateSysNo=b.SysNo";
            string sqlSelect = " PmPointsOrder.* ,a.UserName as CreateName,b.UserName as UpdateName ";
            string sqlWhere = "";

            var dataList = Context.Select<CBPmPointsOrder>(sqlSelect).From(sqlTable);
            var dataCount = Context.Select<int>("count(0)").From(sqlTable);

            var rows = dataList.OrderBy("SysNo desc").Paging(pager.CurrentPage, pager.PageSize).QueryMany();
            var totalRows = dataCount.QuerySingle();

            pager.TotalRows = totalRows;
            pager.Rows = rows;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="delSysNos"></param>
        public override void DeletePointsOrderData(string delSysNos)
        {
            string sql = " delete from PmPointsOrderItem where Poi_ProcurementItemSysNo in (" + delSysNos + ") ";
            Context.Sql(sql).Execute();
            sql = " update PmProcurementOrderItem set Poi_Status='0' where SysNo in (" + delSysNos + ") ";
            Context.Sql(sql).Execute();
        }

        public override void UpdatePointsOrderStatus(int SysNo, int Status)
        {
            string sql = " update PmPointsOrder set Po_Status='" + Status + "' where SysNo in (" + SysNo + ") ";
            Context.Sql(sql).Execute();

            sql = " update PmPointsOrderItem set Poi_Status='" + Status + "' where Poi_PSysNo in (" + SysNo + ") ";
            Context.Sql(sql).Execute();
        }

        public override List<CBPmPointsOrder> GetPointsOrderListByPSysNo(string pSysNoList)
        {
            if (!string.IsNullOrEmpty(pSysNoList))
            {

                string sql = " select PmPointsOrder.*,a.UserName as CreateName,b.UserName as UpdateName " +
                    " from PmPointsOrder left join SyUser a on a.SysNo=PmPointsOrder.Po_CreateSysNo left join SyUser b on b.SysNo=PmPointsOrder.Po_UpdateSysNo  " +
                    " where PmPointsOrder.Po_ProcurementSysNo in (" + pSysNoList + ") ";
                return Context.Sql(sql).QueryMany<CBPmPointsOrder>();
            }
            else
            {
                return new List<CBPmPointsOrder>();
            }
        }

        public override List<CBPmPointsOrderItem> GetPointsOrderItemListByStatus(string pSysNoList)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 获取采购分货单厂家生产单列表
        /// </summary>
        /// <param name="PSysNo"></param>
        /// <returns></returns>
        public override List<CBPmPointsOrder> GetPointsOrderListBySinglePSysNo(int PSysNo)
        {
            string sql = "select PmPointsOrder.*,PmManufacturer.FContact,PmManufacturer.BankName,PmManufacturer.BankIDCard,PmManufacturer.ManufacturerCode,a.UserName as CreateName,b.UserName as UpdateName  " +
                    " from PmPointsOrder left join SyUser a on a.SysNo=PmPointsOrder.Po_CreateSysNo left join SyUser b on b.SysNo=PmPointsOrder.Po_UpdateSysNo  " +
                    " inner join PmManufacturer on PmPointsOrder.Po_FactoryName=PmManufacturer.FName " +
                    " where PmPointsOrder.Po_ProcurementSysNo = '" + PSysNo + "' ";
            return Context.Sql(sql).QueryMany<CBPmPointsOrder>();
        }
    }
}
