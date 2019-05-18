using Hyt.DataAccess.Procurement;
using Hyt.Model.Procurement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Procurement
{
    /// <summary>
    /// 新西兰采购
    /// </summary>
    public class PmProcurementDaoImpl : IPmProcurementDao
    {
        public override int CreatePmProcurementOrder(Model.Procurement.PmProcurementOrder order)
        {
            return Context.Insert<PmProcurementOrder>("PmProcurementOrder", order).AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }

        public override int CreatePmProcurementOrderItem(Model.Procurement.PmProcurementOrderItem order)
        {
            return Context.Insert<PmProcurementOrderItem>("PmProcurementOrderItem", order).AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }

        public override int CreatePmProcurementWebType(Model.Procurement.PmProcurementWebType webType)
        {
            return Context.Insert<PmProcurementWebType>("PmProcurementWebType", webType).AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }

        public override int CreatePmProcurementWebPrice(Model.Procurement.PmProcurementWebPrice webPrice)
        {
            return Context.Insert<PmProcurementWebPrice>("PmProcurementWebPrice", webPrice).AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }

        public override void UpdatePmProcurementOrder(Model.Procurement.PmProcurementOrder order)
        {
             Context.Update<PmProcurementOrder>("PmProcurementOrder", order).AutoMap(p => p.SysNo).Where(p=>p.SysNo).Execute();
        }

        public override void UpdatePmProcurementOrderItem(Model.Procurement.PmProcurementOrderItem order)
        {
            Context.Update<PmProcurementOrderItem>("PmProcurementOrderItem", order).AutoMap(p => p.SysNo).Where(p => p.SysNo).Execute();
        }

        public override void UpdatePmProcurementWebType(Model.Procurement.PmProcurementWebType webType)
        {
            Context.Update<PmProcurementWebType>("PmProcurementWebType", webType).AutoMap(p => p.SysNo).Where(p => p.SysNo).Execute();
        }

        public override void UpdatePmProcurementWebPrice(Model.Procurement.PmProcurementWebPrice webPrice)
        {
            Context.Update<PmProcurementWebPrice>("PmProcurementWebPrice", webPrice).AutoMap(p => p.SysNo).Where(p => p.SysNo).Execute();
        }

        public override void UpdatePmProcurementOrderStatus(int SysNo, int Status, int UpdateBy)
        {
            string sql = "update PmProcurementOrder set Po_Status='" + Status + "',Po_UpdateTime='" + DateTime.Now.ToString() + "',Po_UpdateBy='" + UpdateBy + "' where SysNo=" + SysNo + "";
            Context.Sql(sql).Execute();
        }

        public override List<Model.Procurement.PmProcurementOrderItem> GetProcurementOrderItemList(int pSysNo)
        {
            string sql = "select * from PmProcurementOrderItem where Poi_PSysNo='" + pSysNo + "'";
            return Context.Sql(sql).QueryMany<PmProcurementOrderItem>();
        }
        public override List<Model.Procurement.CBPmProcurementOrderItem> GetCBProcurementOrderItemList(int pSysNo)
        {
            string sql = "select  PmProcurementOrderItem.*,'' as Cb_Spec,PdProduct.ProductName as Cb_ProName,PdProduct.SalesMeasurementUnit as Cb_Unit "+
                " from PmProcurementOrderItem inner join PdProduct on PdProduct.SysNo=PmProcurementOrderItem.Poi_ProSysNo where Poi_PSysNo='" + pSysNo + "'";
            return Context.Sql(sql).QueryMany<CBPmProcurementOrderItem>();
        }
        public override List<Model.Procurement.PmProcurementWebType> GetProcurementWebTypeList()
        {
            string sql = "select * from PmProcurementWebType ";
            return Context.Sql(sql).QueryMany<PmProcurementWebType>();
        }

        public override List<Model.Procurement.CBPmProcurementWebPrice> GetProcurementWebPriceList(int[] itemSysNo)
        {
            string sql = "select PmProcurementWebPrice.*,PmProcurementWebType.Pwt_Name from PmProcurementWebPrice inner join PmProcurementWebType on PmProcurementWebPrice.Pwp_TypeSysNo=PmProcurementWebType.SysNo  where Pwp_OrderItemSysNo in (" 
                + string.Join(",",itemSysNo) +
                ")";
            return Context.Sql(sql).QueryMany<CBPmProcurementWebPrice>();
        }
        /// <summary>
        /// 网站类型
        /// </summary>
        /// <param name="pager"></param>
        public override void GetProcurmentWebTypePager(Model.Pager<PmProcurementWebType> pager)
        {
            string sqlTable = "";
            string sqlSelect = "";
            string sqlWhere = "";

            var dataList = Context.Select<PmProcurementWebType>(sqlSelect).From(sqlTable).Where(sqlWhere);
            var dataCount = Context.Select<int>("count(0)").From(sqlTable).Where(sqlWhere);

            var rows = dataList.OrderBy("SysNo desc").Paging(pager.CurrentPage, pager.PageSize).QueryMany();
            var totalRows = dataCount.QuerySingle();

            pager.TotalRows = totalRows;
            pager.Rows = rows;
        }
        /// <summary>
        /// 申请单页面
        /// </summary>
        /// <param name="pager"></param>
        public override void GetPmProcurementOrderPager(Model.Pager<CBPmProcurementOrder> pager)
        {
            string sqlTable = " PmProcurementOrder left join SyUser a on PmProcurementOrder.Po_CreateBy=a.SysNo left join SyUser b on PmProcurementOrder.Po_UpdateBy=b.SysNo";
            string sqlSelect = " PmProcurementOrder.* ,a.UserName as CreateName,b.UserName as UpdateName ";
            string sqlWhere = "";

            var dataList = Context.Select<CBPmProcurementOrder>(sqlSelect).From(sqlTable);
            var dataCount = Context.Select<int>("count(0)").From(sqlTable);

            var rows = dataList.OrderBy("SysNo desc").Paging(pager.CurrentPage, pager.PageSize).QueryMany();
            var totalRows = dataCount.QuerySingle();

            pager.TotalRows = totalRows;
            pager.Rows = rows;
        }

        public override CBPmProcurementOrder GetCBPmProcurementOrder(int SysNo)
        {
            CBPmProcurementOrder order = Context.Sql(" select PmProcurementOrder.*,SyUser.UserName as CreateName from PmProcurementOrder inner join SyUser on  PmProcurementOrder.Po_CreateBy=SyUser.SysNo where PmProcurementOrder.SysNo='" + SysNo + "' ").QuerySingle<CBPmProcurementOrder>();
            order.orderItemList = GetCBProcurementOrderItemList(SysNo);
            List<int> itemSysNos = new List<int>();
            foreach (PmProcurementOrderItem item in order.orderItemList)
            {
                itemSysNos.Add(item.SysNo);
            }
            order.webPriceList = GetProcurementWebPriceList(itemSysNos.ToArray());
            return order;
        }

        public override IList<CBPmProcurementOrderItem> GetCBProcurementOrderItemListByProList(string proIdList, int cgSysNo)
        {
            string sql = "select  PmProcurementOrderItem.*,'' as Cb_Spec,PdProduct.ProductName as Cb_ProName,PdProduct.SalesMeasurementUnit as Cb_Unit " +
                " from PmProcurementOrderItem inner join PdProduct on PdProduct.SysNo=PmProcurementOrderItem.Poi_ProSysNo where Poi_ProSysNo in (" + proIdList + ") and Poi_PSysNo='" + cgSysNo + "' ";
            return Context.Sql(sql).QueryMany<CBPmProcurementOrderItem>();
        }

        /// <summary>
        /// 修改采购商品的状态
        /// </summary>
        /// <param name="itemSysNo"></param>
        /// <param name="value"></param>
        public override void UpdatePmProcurementOrderItemStatus(int itemSysNo, int value)
        {
            string sql = " update PmProcurementOrderItem set Poi_Status='" + value + "' where SysNo='" + itemSysNo + "'  ";
            Context.Sql(sql).Execute();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public override CBPmProcurementOrder GetCBPmProcurementOrder(string pmNumber)
        {
            CBPmProcurementOrder order = Context.Sql(
                " select PmProcurementOrder.*,SyUser.UserName as CreateName from PmProcurementOrder inner join SyUser on  PmProcurementOrder.Po_CreateBy=SyUser.SysNo where PmProcurementOrder.Po_Number='" + pmNumber + "' ").QuerySingle<CBPmProcurementOrder>();
            if (order != null)
            {

                order.orderItemList = GetCBProcurementOrderItemList(order.SysNo);
                order.pointOrderList = PmPointsOrderDaoImpl.Instance.GetPointsOrderListBySinglePSysNo(order.SysNo);
                List<int> itemSysNos = new List<int>();
                foreach (PmProcurementOrderItem item in order.orderItemList)
                {
                    itemSysNos.Add(item.SysNo);
                }
                order.webPriceList = GetProcurementWebPriceList(itemSysNos.ToArray());

            }
            return order;
        }

        public override List<CBPmProcurementOrderItem> GetPmProcurementOrderItem(int[] pSysNo)
        {
            string sql = "select  PmProcurementOrderItem.*,'' as Cb_Spec,PdProduct.ProductName as Cb_ProName,PdProduct.SalesMeasurementUnit as Cb_Unit " +
               " from PmProcurementOrderItem inner join PdProduct on PdProduct.SysNo=PmProcurementOrderItem.Poi_ProSysNo where PmProcurementOrderItem.Poi_PSysNo in (" + string.Join(",", pSysNo) + ")  ";
            return Context.Sql(sql).QueryMany<CBPmProcurementOrderItem>();
        }
    }
}
