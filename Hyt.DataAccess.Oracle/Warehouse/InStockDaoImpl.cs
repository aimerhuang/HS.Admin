using System.Transactions;
using Hyt.DataAccess.Base;
using Hyt.DataAccess.Warehouse;
using Hyt.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Model.Parameter;

namespace Hyt.DataAccess.Oracle.Warehouse
{
    /// <summary>
    /// 入库单维护DAL
    /// </summary>
    /// <remarks>2013-06-08 周唐炬 创建</remarks>
    public class InStockDaoImpl : IInStockDao
    {
        #region 入库单维护
        /// <summary>
        /// 插入入库单
        /// </summary>
        /// <param name="model">入库单明细</param>
        /// <returns>入库单系统编号</returns>
        /// <remarks>2013-06-09 周唐炬 创建</remarks>
        public override int InsertWhStockIn(WhStockIn model)
        {
            var sysNo = Context.Insert<WhStockIn>("WhStockIn", model)
               .AutoMap(x => x.SysNo, x => x.ItemList, x => x.InvoiceSysNo)
               .ExecuteReturnLastId<int>("SysNo");
            model.SysNo = sysNo;
            #region 添加入库单明细
            if (null != model.ItemList && model.ItemList.Count > 0)
            {
                foreach (var item in model.ItemList)
                {
                    item.StockInSysNo = sysNo;
                    InsertWhStockInItem(item);
                }
            }
            #endregion
            return sysNo;
        }

        /// <summary>
        /// 更新入库单
        /// </summary>
        /// <param name="model">入库单明细</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-06-09 周唐炬 创建</remarks>
        public override int UpdateWhStockIn(WhStockIn model)
        {
            var rowsAffected = Context.Update<WhStockIn>("WhStockIn", model)
                .AutoMap(x => x.SysNo, x => x.ItemList, x => x.InvoiceSysNo).Where(x => x.SysNo)
                .Execute();
            if (model.ItemList != null)
            {
                foreach (var item in model.ItemList)
                {
                    UpdateWhStockInItem(item);
                }
            }
            return rowsAffected;
        }

        /// <summary>
        /// 删除入库单
        /// </summary>
        /// <param name="sysNo">入库单系统编号</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013-06-09 周唐炬 创建</remarks>
        public override bool DelWhStockIn(int sysNo)
        {
            var rowsAffected = Context.Delete("WhStockIn")
            .Where("SysNo", sysNo)
            .Execute();
            return rowsAffected > 0;
        }

        /// <summary>
        /// 获取入库单列表
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>返回入库单列表 Ilist</returns>
        /// <remarks>2013-06-08 周唐炬 创建</remarks>
        public override Pager<WhStockIn> GetWhStockInList(ParaInStockFilter filter, int pageSize)
        {
            #region  原始调试Sql
            /*
             --此处构造参数
             select count(1) 
                from WhStockIn w                                                                        --入库单
                  inner join WhWarehouse b 
                  ON w.WarehouseSysNo = b.Sysno                                                         --仓库系统编号
                where (:WarehouseSysNo is null or w.WarehouseSysNo = :WarehouseSysNo)               
                   and (:SourceType is null or w.SourceType = :SourceType)
                   and (:SourceSysNo is null or w.SourceSysNo = :SourceSysNo)
                   and (:CreatedDate is null 
                       or to_char(w.CreatedDate,'YYYY MM DD') = to_char(:CreatedDate,'YYYY MM DD'))     --日期=YYYY MM DD
             */
            #endregion

            #region Sql 获取入库单列表
            const string sql =
              @"(SELECT w.*
                FROM WhStockIn w
                WHERE (@0 IS NULL OR EXISTS
                        (SELECT 1
                            FROM splitstr(@0, ',') tmp
                        WHERE tmp.col = w.Warehousesysno))
                   AND (@1 IS NULL OR @1<>0 AND w.SourceType = @1)
                   AND (@2 IS NULL OR w.SourceSysNo = @2)
                   AND (@3 IS NULL OR w.Status=@3)
                   AND (@4 IS NULL OR w.IsPrinted=@4)
                   AND (@5 IS NULL OR Convert(nvarchar(10),w.CreatedDate,120) = Convert(nvarchar(10),@5,120))
                ) tb     
            ";
            #endregion

            var dataList = Context.Select<WhStockIn>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(1)").From(sql);
            var warehouseSysNoList = string.Empty;
            if (null != filter.WarehouseSysNoList && filter.WarehouseSysNoList.Any())
            {
                warehouseSysNoList = string.Join(",", filter.WarehouseSysNoList);
            }
            var paras = new object[]
                {
                    warehouseSysNoList,
                    filter.SourceType,
                    filter.SourceSysNo,
                    filter.Status,
                    filter.IsPrinted,
                    filter.CreatedDate
                };
            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<WhStockIn>
                {
                    Rows = dataList.OrderBy(@"tb.SysNo desc").Paging(filter.CurrentPage, pageSize).QueryMany(),
                    TotalRows = dataCount.QuerySingle()
                };

            return pager;
        }

        /// <summary>
        /// 根据来源单据和类型获取入库单
        /// </summary>
        /// <param name="sourceType">来源类型</param>
        /// <param name="sourceNo">来源单据系统编号</param>
        /// <returns>入库单</returns>
        /// <remarks>2013-9-3 黄伟 创建</remarks>
        public override WhStockIn GetStockInBySource(int sourceType, int sourceNo)
        {
            return Context.Sql(@"select * from whstockin where sourceType=@sourceType and sourcesysno=@sourceNo")
                          .Parameter("sourceType", sourceType)
                          .Parameter("sourceNo", sourceNo)
                          .QuerySingle<WhStockIn>();
        }

        /// <summary>
        /// 通过系统编号获取入库单明细
        /// </summary>
        /// <param name="sysNo">入库单系统编号</param>
        /// <returns>返回入库单明细,包含入库商品列表</returns>
        /// <remarks>2013-06-08 周唐炬 创建</remarks>
        public override WhStockIn GetWhStockIn(int sysNo)
        {
            return Context.Sql(@"SELECT w.*
                                 FROM WhStockIn w WHERE w.Sysno =@SysNo")
                .Parameter("SysNo", sysNo)
                .QuerySingle<WhStockIn>();
        }

        /// <summary>
        /// 通过事务编号获取入库单明细
        /// </summary>
        /// <param name="transactionSysNo">事务编号</param>
        /// <returns>返回入库单明细,包含入库商品列表</returns>
        /// <remarks>2013-06-08 周唐炬 创建</remarks>
        public override WhStockIn GetWhStockInByTransactionSysNo(string transactionSysNo)
        {
            return Context.Sql(@"SELECT * FROM WhStockIn WHERE transactionSysNo =@TransactionSysNo")
                .Parameter("TransactionSysNo", transactionSysNo)
                .QuerySingle<WhStockIn>();
        }
        #endregion

        #region 商品入库
        /// <summary>
        /// 商品入库
        /// </summary>
        /// <param name="model">入库单明细</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-06-08 周唐炬 创建</remarks>
        public override int InsertWhStockInItem(WhStockInItem model)
        {
            if (model.StockInSysNo <= 0)
            {
                throw new Exception("入库单明细的入库单系统编号不能小于等于0");
            }
            var id = Context.Insert<WhStockInItem>("WhStockInItem", model)
                .AutoMap(x => x.SysNo,x=>x.ProductErpCode)
                .ExecuteReturnLastId<int>("SysNo");
            return id;
        }

        /// <summary>
        /// 更新商品入库信息
        /// </summary>
        /// <param name="model">入库单明细</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2013-06-08 周唐炬 创建</remarks>
        public override int UpdateWhStockInItem(WhStockInItem model)
        {
            var rowsAffected = Context.Update<WhStockInItem>("WhStockInItem", model)
                .AutoMap(x => x.SysNo, x => x.ProductErpCode).Where(x => x.SysNo)
                .Execute();
            return rowsAffected;
        }

        /// <summary>
        /// 删除商品入库信息
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks>2013-06-09 周唐炬 创建</remarks>
        public override bool DelWhStockInItem(int sysNo)
        {
            var rowsAffected = Context.Delete("WhStockInItem")
            .Where("SysNo", sysNo)
            .Execute();
            return rowsAffected > 0;
        }

        /// <summary>
        /// 通过入库单ID获取所有商品列表
        /// </summary>
        /// <param name="stockInSysNo">入库单系统SysNO</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>返回入库单商品列表</returns>
        /// <remarks>2013-06-09 周唐炬 创建</remarks>
        public override IList<WhStockInItem> GetWhStockInItemListByStockInSysNo(int stockInSysNo, int pageIndex, int pageSize)
        {        
            var listResult = Context.Select<WhStockInItem>("t.*,pd.ErpCode as ProductErpCode")
                .From("WhStockInItem as t left join PdProduct as pd on pd.SysNo=t.ProductSysNo")
                .Where("t.StockInSysNo =@StockInSysNo")
                .Parameter("StockInSysNo", stockInSysNo)
                .OrderBy("t.SysNo")
                .Paging(pageIndex, pageSize)
                .QueryMany();

            return listResult;
        }

        /// <summary>
        /// 通过入库单ID获取所有商品总数
        /// </summary>
        /// <param name="stockInSysNo">入库单系统stockInSysNo</param>
        /// <returns>返回入库单所有商品总数</returns>
        /// <remarks>2013-06-09 周唐炬 创建</remarks>
        public override int GetWhStockInItemListByStockInSysNoCount(int stockInSysNo)
        {
            return Context.Sql(@"SELECT count(*) FROM WhStockInItem WHERE StockInSysNo =@StockInSysNo").Parameter("StockInSysNo", stockInSysNo).QuerySingle<int>();
        }

        /// <summary>
        /// 通过系统编号获取入库商品信息
        /// </summary>
        /// <param name="stockInSysNo">入库单系统编号</param>
        /// <param name="productSysNo">商品系统编号</param>
        /// <returns>入库商品信息</returns>
        /// <remarks>2013-06-09 周唐炬 创建</remarks>
        public override WhStockInItem GetWhStockInItemBySysNo(int stockInSysNo, int productSysNo)
        {
            return Context.Sql(@"SELECT * FROM WhStockInItem WHERE ProductSysNo=@ProductSysNo AND StockInSysNo=@StockInSysNo")
                .Parameter("ProductSysNo", productSysNo)
                .Parameter("StockInSysNo", stockInSysNo)
                .QuerySingle<WhStockInItem>();
        }

        /// <summary>
        /// 通过入库明细系统编号获取入库明细
        /// </summary>
        /// <param name="sysNo">入库明细系统编号</param>
        /// <returns>入库明细</returns>
        /// <remarks>2013-06-09 周唐炬 创建</remarks>
        public override WhStockInItem GetWhStockInItem(int sysNo)
        {
            return Context.Sql(@"SELECT * FROM WhStockInItem WHERE SysNo=@SysNo")
                .Parameter("SysNo", sysNo)
                .QuerySingle<WhStockInItem>();
        }

        /// <summary>
        /// 通过入库单ID获取所有商品列表
        /// </summary>
        /// <param name="stockInSysNo">入库单系统stockInSysNo</param>
        /// <returns>返回入库单商品列表</returns>
        /// <remarks>2013-06-24 郑荣华 创建</remarks>
        public override List<WhStockInItem> GetWhStockInItemList(int stockInSysNo)
        {
            var listResult = Context.Select<WhStockInItem>("t.*")
                .From("WhStockInItem t")
                .Where("t.StockInSysNo =@StockInSysNo")
                .Parameter("StockInSysNo", stockInSysNo)
                .OrderBy("t.SysNo")
                .QueryMany();

            return listResult;
        }

        /// <summary>
        /// 根据单据来源获取入库单
        /// </summary>
        /// <param name="source">单据来源</param>
        /// <param name="sourceSysNo">单据编号</param>
        /// <returns>入库单</returns>
        /// <remarks>2013-7-25 朱家宏 创建 </remarks>
        public override WhStockIn GetWhStockInByVoucherSource(int source, int sourceSysNo)
        {
            return Context.Sql(@"SELECT * FROM WhStockIn  where SourceType=@source and sourceSysNo=@sourceSysNo")
                          .Parameter("source", source)
                          .Parameter("sourceSysNo", sourceSysNo)
                          .QuerySingle<WhStockIn>();
        }
        /// <summary>
        /// 根据单据来源和状态获取入库单
        /// </summary>
        /// <param name="sourceType">单据来源</param>
        /// <param name="sourceNo">单据编号</param>
        /// <param name="Status">状态</param>
        /// <returns></returns>
        /// <remarks>2014-04-11 朱成果 创建 </remarks>
        public override WhStockIn GetStockInBySourceAndStatus(int sourceType, int sourceNo, int? Status)
        {
            if(Status.HasValue)
            {
                return Context.Sql(@"SELECT * FROM WhStockIn  where SourceType=@source and sourceSysNo=@sourceSysNo and Status=@Status")
                         .Parameter("source", sourceType)
                         .Parameter("sourceSysNo", sourceNo)
                          .Parameter("Status", Status.Value)
                         .QuerySingle<WhStockIn>();
            }
            else
            {
                return GetWhStockInByVoucherSource(sourceType, sourceNo);
            }
        }
        #endregion

        public override List<WhStockIn> GetStockInListByDate(DateTime dateTime)
        {
            string sql = " select * from WhStockIn where Status=50 and LastUpdateDate>='"+dateTime.ToString("yyyy-MM-dd")+" 00:00:00' ";
            return Context.Sql(sql).QueryMany<WhStockIn>();
        }
    }
}
