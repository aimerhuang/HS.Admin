using Hyt.DataAccess.Procurement;
using Hyt.Model;
using Hyt.Model.Procurement;
using Hyt.Model.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Procurement
{
    public class WhWarehouseMonthStockHistoryDaoImpl : IWhWarehouseMonthStockHistoryDao
    {
        public override void InsertOrUpdateStockHistory(int year, int month,int day,int warehouseSysNo)
        {
            string InWareGoodsSql = @"  select PdProductStockInDetail.PdProductSysNo as ProductSysNo, SUM(PdProductStockInDetail.DoStorageQuantity) as TotalQuantity 
                                        from 
                                        PdProductStockIn inner join PdProductStockInDetail on PdProductStockIn.SysNo=PdProductStockInDetail.ProductStockInSysNo
                                        where Status=50 and WarehouseSysNo=" +warehouseSysNo+
                                        @"  and PdProductStockIn.StorageTime<='"+year+"-"+month+"-"+day+@" 23:59:59'
                                        group by PdProductStockInDetail.PdProductSysNo";
            List<WarehouseMod> InWareList = Context.Sql(InWareGoodsSql).QueryMany<WarehouseMod>();

            string SalesGoodsSql = @"   select SoOrderItem.ProductSysNo ,sum(SoOrderItem.Quantity) as TotalQuantity  from SoOrder inner join SoOrderItem on SoOrderItem.OrderSysNo=SoOrder.SysNo
                                        where Status>=30 and SoOrder.DefaultWarehouseSysNo="+warehouseSysNo+
                                        @"  and SoOrder.CreateDate<='" + year + "-" + month + "-" + day + @" 23:59:59'  
                                        group by SoOrderItem.ProductSysNo";
            List<WarehouseMod> SalesList = Context.Sql(SalesGoodsSql).QueryMany<WarehouseMod>();

            string RetGoodsSql = @"     select RcReturnItem.ProductSysNo,sum(RcReturnItem.RmaQuantity) as TotalQuantity 
                                        from RcReturn inner join RcReturnItem on RcReturn.SysNo=RcReturnItem.ReturnSysNo inner join SoOrder on RcReturn.OrderSysNo=SoOrder.SysNo
                                        where RcReturn.RmaType=20 and RcReturn.Status=50 and  SoOrder.DefaultWarehouseSysNo=" + warehouseSysNo +
                                        @" and RcReturn.CreateDate<='" + year + "-" + month + "-" + day + @" 23:59:59' 
                                        group by RcReturnItem.ProductSysNo";
            List<WarehouseMod> RetList = Context.Sql(RetGoodsSql).QueryMany<WarehouseMod>();

            ///商品损耗列表
            string LossGoodsSql = @"select sum(PdProductLossItem.Quantity) as TotalQuantity,PdProductLossItem.ProSysNo as ProductSysNo from PdProductLoss inner join PdProductLossItem on PdProductLoss.sysNo=PdProductLossItem.PSysNo
                                    where  PdProductLoss.ComeByWareSysNo=" + warehouseSysNo + @"
                                     and PdProductLoss.CreateTime<='" + year + "-" + month + "-" + day + @" 23:59:59' 
                                    group by PdProductLossItem.ProSysNo";
            List<WarehouseMod> LossList = Context.Sql(LossGoodsSql).QueryMany<WarehouseMod>();

            List<Model.Procurement.WhWarehouseMonthStockHistory> haveList =
                GetWhWarehouseMonthStockHistory(year, month, warehouseSysNo);
            foreach (WarehouseMod mod in InWareList)
            {
                WhWarehouseMonthStockHistory history = haveList.Find(p => p.ProductSysNo == mod.ProductSysNo);
                WarehouseMod inWareMod = InWareList.Find(p => p.ProductSysNo == mod.ProductSysNo);
                WarehouseMod salesMod = SalesList.Find(p => p.ProductSysNo == mod.ProductSysNo);
                WarehouseMod retMod = RetList.Find(p => p.ProductSysNo == mod.ProductSysNo);
                WarehouseMod lossMod= LossList.Find(p => p.ProductSysNo == mod.ProductSysNo);
                if (history == null)
                {
                    WhWarehouseMonthStockHistory tempHistory = new WhWarehouseMonthStockHistory()
                    {
                        WarehouseSysNo = warehouseSysNo,
                        WhYear = year,
                        WhMonth = month,
                        InWareQuantity = (inWareMod == null ? 0 : inWareMod.TotalQuantity),
                        ProductSysNo = mod.ProductSysNo,
                        SalesQuantity = (salesMod == null ? 0 : salesMod.TotalQuantity),
                        RetQuantity = (retMod == null ? 0 : retMod.TotalQuantity),
                        TotalQuantity = (
                          (inWareMod == null ? 0 : inWareMod.TotalQuantity) +
                          (retMod == null ? 0 : retMod.TotalQuantity) -
                          (salesMod == null ? 0 : salesMod.TotalQuantity)
                        ),
                        LossQuantity = (lossMod==null?0:lossMod.TotalQuantity)
                    };
                    InsertEntity(tempHistory);
                }
                else
                {
                    history.InWareQuantity = (inWareMod == null ? 0 : inWareMod.TotalQuantity);
                    history.ProductSysNo = mod.ProductSysNo;
                    history.SalesQuantity = (salesMod == null ? 0 : salesMod.TotalQuantity);
                    history.RetQuantity = (retMod == null ? 0 : retMod.TotalQuantity);
                    history.TotalQuantity = (
                      (inWareMod == null ? 0 : inWareMod.TotalQuantity) +
                      (retMod == null ? 0 : retMod.TotalQuantity) -
                      (salesMod == null ? 0 : salesMod.TotalQuantity)
                    );
                    history.LossQuantity = (lossMod == null ? 0 : lossMod.TotalQuantity);
                    UpdateEntity(history);
                }
            }
            

        }
        public override int InsertEntity(WhWarehouseMonthStockHistory history)
        {
            return Context.Insert("WhWarehouseMonthStockHistory", history).AutoMap(p => p.SysNo).ExecuteReturnLastId<int>("SysNo");
        }
       
        public override void UpdateEntity(WhWarehouseMonthStockHistory history)
        {
            Context.Update("WhWarehouseMonthStockHistory", history).AutoMap(p => p.SysNo).Where(p => p.SysNo).Execute();
        }
        public override List<Model.Procurement.WhWarehouseMonthStockHistory> GetWhWarehouseMonthStockHistory(int year, int month, int warehouseSysNo)
        {
            string sql = " select * from WhWarehouseMonthStockHistory where WhYear = " + year + " and WhMonth = " + month;
            return Context.Sql(sql).QueryMany<WhWarehouseMonthStockHistory>();
        }
        public override void CheckAndUpdataMonthArea(DateTime starTime, DateTime endTime, int warehouseSysNo)
        {
            while (starTime <= endTime)
            {
                DateTime temp =  starTime.AddDays(-1);
                InsertOrUpdateStockHistory(temp.Year, temp.Month, temp.Day, warehouseSysNo);
                starTime=starTime.AddMonths(1);
            }
        }
        /// <summary>
        /// 分页显示
        /// </summary>
        /// <param name="pager"></param>
        public override void GetWhWarehouseMonthStockHistoryPager(ref Model.Pager<Model.Procurement.CBWhWarehouseMonthStockHistory> pager)
        {
            string sqlTable = "( select distinct ProductSysNo,WarehouseSysNo  from WhWarehouseMonthStockHistory where  WarehouseSysNo = '" + pager.PageFilter.WarehouseSysNo + "' and WhYear='" + pager.PageFilter.WhYear + "' ) as tab";
            string sqlSelect = @"  ProductSysNo ";
            string sqlWhere = "";

            var dataList = Context.Select<CBWhWarehouseMonthStockHistory>(sqlSelect).From(sqlTable).Where(sqlWhere);
            var dataCount = Context.Select<int>("count(0)").From(sqlTable).Where(sqlWhere);

            var rows = dataList.OrderBy("ProductSysNo desc").Paging(pager.CurrentPage, pager.PageSize).QueryMany();
            var totalRows = dataCount.QuerySingle();

            pager.TotalRows = totalRows;
            pager.Rows = rows;

            if (rows.Count > 0)
            {

                string strList = "";
                foreach (CBWhWarehouseMonthStockHistory mod in rows)
                {
                    if (!string.IsNullOrEmpty(strList))
                    {
                        strList += ",";
                    }
                    strList += mod.ProductSysNo;
                }
                string sql = " select WhWarehouseMonthStockHistory.*,"
                    + " PdProduct.ErpCode,PdProduct.EasName,'' as Spec,PdProduct.SpecUnit "
                    + " from WhWarehouseMonthStockHistory inner join PdProduct on  ";
                sql += " WhWarehouseMonthStockHistory.ProductSysNo=PdProduct.SysNo ";
                sql += " where ProductSysNo in (" + strList + ") and WhYear='"
                    + pager.PageFilter.WhYear + "' and WarehouseSysNo='" + pager.PageFilter.WarehouseSysNo 
                    + "'   order by  ProductSysNo  ";

                pager.Rows = Context.Sql(sql).QueryMany<CBWhWarehouseMonthStockHistory>();
            }
        }




        public override void GetWhWarehousMouthSaleHistoryPager(
            ref Model.Pager<CBWhWarehouseMonthStockHistory> pager)
        {
            DateTime upTime = new DateTime(pager.PageFilter.WhYear, pager.PageFilter.WhMonth, 1);
            upTime = upTime.AddMonths(-1);
//            string sql = @" select
//                            from WhWarehouseMonthStockHistory inner join PdProduct on  
//                            WhWarehouseMonthStockHistory.ProductSysNo=PdProduct.SysNo
//                            where  WhYear='" + pager.PageFilter.WhYear + "' and WhMonth='" + pager.PageFilter.WhMonth + "' and  WarehouseSysNo='" + pager.PageFilter.WarehouseSysNo + "' order by  ProductSysNo  ";
            string sqlTable = @" WhWarehouseMonthStockHistory inner join PdProduct on  
                              WhWarehouseMonthStockHistory.ProductSysNo=PdProduct.SysNo ";
            string sqlSelect = @"   WhWarehouseMonthStockHistory.*,
                            PdProduct.ErpCode,PdProduct.EasName,'' as Spec,PdProduct.SpecUnit ,
					        UpMonthQuantity=(select TotalQuantity from WhWarehouseMonthStockHistory as tab where  WhYear='" + upTime.Year + "' " +
                            @" and WhMonth='" + upTime.Month + "' and  WarehouseSysNo='" + pager.PageFilter.WarehouseSysNo + @"' and tab.ProductSysNo=PdProduct.SysNo ),
					        UpMonthInQuantity=(select InWareQuantity from WhWarehouseMonthStockHistory as tab where  WhYear='" + upTime.Year + "' " +
                            @" and WhMonth='" + upTime.Month + "' and  WarehouseSysNo='" + pager.PageFilter.WarehouseSysNo + @"' and tab.ProductSysNo=PdProduct.SysNo ),
					        UpMonthLossQuantity=(select LossQuantity from WhWarehouseMonthStockHistory as tab where  WhYear='" + upTime.Year + "' " +
                            @" and WhMonth='" + upTime.Month + "' and  WarehouseSysNo='" + pager.PageFilter.WarehouseSysNo + @"' and tab.ProductSysNo=PdProduct.SysNo ),
					        UpMonthSalesQuantity=(select SalesQuantity from WhWarehouseMonthStockHistory as tab where  WhYear='" + upTime.Year + "' " +
                            @" and WhMonth='" + upTime.Month + "' and  WarehouseSysNo='" + pager.PageFilter.WarehouseSysNo + @"' and tab.ProductSysNo=PdProduct.SysNo ) ,
					        UpMonthRetQuantity=(select RetQuantity from WhWarehouseMonthStockHistory as tab where  WhYear='" + upTime.Year + "' " +
                            @" and WhMonth='" + upTime.Month + "' and  WarehouseSysNo='" + pager.PageFilter.WarehouseSysNo + @"' and tab.ProductSysNo=PdProduct.SysNo )  ";

            string sqlWhere = " WhYear='" + pager.PageFilter.WhYear + "' and WhMonth='" + pager.PageFilter.WhMonth + "' and  WarehouseSysNo='" + pager.PageFilter.WarehouseSysNo + "'   ";

            var dataList = Context.Select<CBWhWarehouseMonthStockHistory>(sqlSelect).From(sqlTable).Where(sqlWhere);
            var dataCount = Context.Select<int>("count(0)").From(sqlTable).Where(sqlWhere);

            var rows = dataList.OrderBy("TotalQuantity desc").Paging(pager.CurrentPage, pager.PageSize).QueryMany();
            var totalRows = dataCount.QuerySingle();

            pager.TotalRows = totalRows;
            pager.Rows = rows;
        }


        public override void GetPaymentListDataPager(
          ref Model.Pager<CBFnReceiptVoucher> pager, int? PaymentTypeSysNo, int? WarehouseSysNo, DateTime? startTime, DateTime? endTime,
            string sysNoList)
        {
           
            string sqlTable = @" FnReceiptVoucher 
                                inner join FnReceiptVoucherItem on FnReceiptVoucher.SysNo=FnReceiptVoucherItem.ReceiptVoucherSysNo 
                                inner join SoOrder on SoOrder.SysNo=FnReceiptVoucher.SourceSysNo
                                inner join WhWarehouse on SoOrder.DefaultWarehouseSysNo = WhWarehouse.SysNo
                                inner join BsPaymentType on SoOrder.PayTypeSysNo=BsPaymentType.SysNo
                                left join SyUser on SyUser.SysNo=FnReceiptVoucher.CreatedBy
                                left join SyUser as tabUser2 on tabUser2.SysNo=SoOrder.OrderCreatorSysNo
                                left join CrCustomer as Customer1 on Customer1.SysNo=SoOrder.CustomerSysNo
                                left join SyUser as tabUser on tabUser.SysNo=FnReceiptVoucher.ConfirmedBy ";
            string sqlSelect = @"  SoOrder.SysNo, BsPaymentType.PaymentName ,SoOrder.CreateDate,tabUser2.UserName as CreatorName,Customer1.Name as CustomerName,tabUser.UserName as ConfirmeName,
                                FnReceiptVoucher.IncomeAmount,FnReceiptVoucher.ReceivedAmount,FnReceiptVoucherItem.CreditCardNumber,FnReceiptVoucherItem.VoucherNo,
                                WhWarehouse.WarehouseName ";

            string sqlWhere = " FnReceiptVoucher.Source=10 and FnReceiptVoucher.status=20   ";
            if (PaymentTypeSysNo != null && PaymentTypeSysNo.Value > 0)
            {
                sqlWhere += " and  SoOrder.PayTypeSysNo=" + PaymentTypeSysNo.Value;
            }
            if (WarehouseSysNo != null && WarehouseSysNo.Value > 0)
            {
                sqlWhere += " and  SoOrder.DefaultWarehouseSysNo=" + WarehouseSysNo.Value;
            }
            if(startTime!=null)
            {
                sqlWhere += " and  SoOrder.CreateDate>='" + startTime.Value.ToString("yyyy-MM-dd") + " 00:00:00' ";
            }
            if (endTime != null)
            {
                sqlWhere += " and  SoOrder.CreateDate<='" + endTime.Value.ToString("yyyy-MM-dd") + " 23:59:59' ";
            }
            if(!string.IsNullOrEmpty(sysNoList))
            {
                sqlWhere += " and  SoOrder.SysNo in (" + sysNoList + ") ";
            }
            var dataList = Context.Select<CBFnReceiptVoucher>(sqlSelect).From(sqlTable).Where(sqlWhere);
            var dataCount = Context.Select<int>("count(0)").From(sqlTable).Where(sqlWhere);

            var rows = dataList.OrderBy(" SourceSysNo desc").Paging(pager.CurrentPage, pager.PageSize).QueryMany();
            var totalRows = dataCount.QuerySingle();

            pager.TotalRows = totalRows;
            pager.Rows = rows;
        }
    }
}
