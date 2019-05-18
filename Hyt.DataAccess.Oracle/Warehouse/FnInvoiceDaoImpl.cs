using System;
using System.Collections.Generic;
using System.Linq;
using Hyt.DataAccess.Base;
using Hyt.DataAccess.Warehouse;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Oracle.Warehouse
{
    /// <summary>
    /// 发票数据层实现
    /// </summary>
    /// <remarks>2013-07-10 周瑜 创建</remarks>
    public class FnInvoiceDaoImpl : IInvoiceDao
    {
        /// <summary>
        /// 根据输入条件查询发票
        /// </summary>
        /// <param name="condition">搜索条件</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>发票实体列表</returns>
        /// <remarks>2013-07-10 周瑜 创建</remarks>
        /// <remarks>2013-11-13 周唐炬 重构</remarks>
        public override Pager<CBFnInvoice> Search(InvoiceSearchCondition condition, int pageSize)
        {
            //查询已审核且已付款的记录
            const string sql = @"(SELECT B.SYSNO AS OrderSysNo,
                                        B.TransactionSysNo as TransactionSysNo,
                                        A.SysNo,
                                        A.InvoiceTypeSysNo,
                                        A.InvoiceCode,
                                        A.InvoiceNo,
                                        A.InvoiceAmount,
                                        A.InvoiceTitle,
                                        A.Status,
                                        A.LastUpdateDate
                                      FROM SOORDER B
                                     LEFT JOIN FNINVOICE A
                                        ON B.TRANSACTIONSYSNO = A.TRANSACTIONSYSNO
                                     WHERE (@0 IS NULL OR charindex(A.INVOICECODE, @0) > 0)
                                       AND (@1 IS NULL OR charindex(A.INVOICENO, @1) > 0)
                                       AND (@2 IS NULL OR charindex(A.INVOICETITLE, @2) > 0)
                                       AND (@3 IS NULL OR A.STATUS = @3)
                                       AND (@4 IS NULL OR A.INVOICETYPESYSNO = @4)                                                                        
                                       AND B.STATUS <> @5 --不能是作废订单
                                       --AND B.PAYSTATUS = @PayStatus --订单已支付                                       
                                       AND B.TRANSACTIONSYSNO IS NOT NULL
                                       AND EXISTS
                                                (SELECT 1
                                                    FROM splitstr(@6, ',') tmp
                                                WHERE tmp.col = B.DEFAULTWAREHOUSESYSNO)
                                       AND (@7 IS NULL OR B.SYSNO = @7)
                                    )tb";

            var warehouseSysNoList = string.Empty;
            if (condition.WarehouseSysNoList != null && condition.WarehouseSysNoList.Any())
            {
                warehouseSysNoList = string.Join(",", condition.WarehouseSysNoList);
            }
            var paras = new object[]
            {
               condition.InvoiceCode,
               condition.InvoiceNo,
               condition.InvoiceTitle,
               condition.Status,
               condition.InvoiceTypeSysNo,
               OrderStatus.销售单状态.作废.GetHashCode(),
               warehouseSysNoList,
               condition.OrderSysNo
            };

            var dataList = Context.Select<CBFnInvoice>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(1)").From(sql);
            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<CBFnInvoice>
            {
                Rows = dataList.OrderBy(@"tb.OrderSysNo desc").Paging(condition.CurrentPage, pageSize).QueryMany(),
                TotalRows = dataCount.QuerySingle()
            };

            return pager;
        }

        /// <summary>
        /// 根据输入条件查询发票列表
        /// </summary>
        /// <param name="condition">搜索条件</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>发票实体列表</returns>
        /// <remarks>2013-07-09 周瑜 创建</remarks>
        /// <remarks>2013-11-13 周唐炬 重构</remarks>
        public override Pager<SoOrder> QuickSearch(InvoiceSearchCondition condition, int pageSize)
        {
            //查询已审核且已付款的记录
            const string sql = @"(SELECT A.SysNo
                                          FROM SOORDER A
                                         WHERE EXISTS (SELECT 1
                                                   FROM splitstr(@0, ',') tmp
                                                  WHERE tmp.col = A.DEFAULTWAREHOUSESYSNO)                                            
                                            AND A.STATUS <> @2 --不能是作废订单
                                            AND A.PAYSTATUS = @3 --订单已支付     
                                            AND (@1 IS NULL OR A.SYSNO = @1)       
                                    )tb";

            var warehouseSysNoList = string.Empty;
            if (condition.WarehouseSysNoList != null && condition.WarehouseSysNoList.Any())
            {
                warehouseSysNoList = string.Join(",", condition.WarehouseSysNoList);
            }
            var paras = new object[]
            {
               warehouseSysNoList,
               condition.OrderSysNo,
               OrderStatus.销售单状态.作废.GetHashCode(),
               OrderStatus.销售单支付状态.已支付.GetHashCode()
            };

            var dataList = Context.Select<SoOrder>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(1)").From(sql);
            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<SoOrder>
            {
                Rows = dataList.OrderBy(@"tb.SysNo desc").Paging(condition.CurrentPage, pageSize).QueryMany(),
                TotalRows = dataCount.QuerySingle()
            };

            return pager;
        }

        /// <summary>
        /// 根据系统编号获取发票
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>发票实体</returns>
        /// <remarks>2013-07-10 周瑜 创建</remarks>
        public override FnInvoice GetModel(int sysNo)
        {
            var model = Context.Sql("select * from FnInvoice a where a.sysno= @sysno")
                                     .Parameter("sysno", sysNo)
                                     .QuerySingle<FnInvoice>();
            return model;
        }

        /// <summary>
        /// 根据事务获取发票
        /// </summary>
        /// <param name="transactionSysNo">事务编号</param>
        /// <returns>发票实体</returns>
        /// <remarks>2013-11-12 周唐炬 创建</remarks>
        public override FnInvoice GetInvoiceByTransactionSysNo(string transactionSysNo)
        {
            return Context.Sql("select * from FnInvoice fn where fn.TransactionSysNo= @TransactionSysNo")
                                     .Parameter("TransactionSysNo", transactionSysNo)
                                     .QuerySingle<FnInvoice>();
        }

        /// <summary>
        /// 修改发票状态
        /// </summary>
        /// <param name="fnInvoice">用于修改发票的实体</param>
        /// <returns>影像的行数</returns>
        /// <remarks>2013-07-10 周瑜 创建</remarks>
        public override int Update(FnInvoice fnInvoice)
        {
            return Context.Update("FnInvoice", fnInvoice)
                          .AutoMap(x => x.SysNo)
                          .Where("SysNo", fnInvoice.SysNo)
                          .Execute();
        }
    }
}
