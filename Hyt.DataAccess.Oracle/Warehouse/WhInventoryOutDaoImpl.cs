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
    /// 出库单维护DAL
    /// </summary>
    /// <remarks>2013-06-08 周唐炬 创建</remarks>
    public class WhInventoryOutDaoImpl : IWhInventoryOutDao
    {
        #region 出库单维护
        /// <summary>
        /// 插入出库单
        /// </summary>
        /// <param name="model">出库单明细</param>
        /// <returns>出库单系统编号</returns>
        /// <remarks>2016-06-24 王耀发 创建</remarks>
        public override int InsertWhInventoryOut(WhInventoryOut model)
        {
            var sysNo = Context.Insert<WhInventoryOut>("WhInventoryOut", model)
               .AutoMap(x => x.SysNo, x => x.ItemList)
               .ExecuteReturnLastId<int>("SysNo");
            model.SysNo = sysNo;
            #region 添加出库单明细
            if (null != model.ItemList && model.ItemList.Count > 0)
            {
                foreach (var item in model.ItemList)
                {
                    item.InventoryOutSysNo = sysNo;
                    InsertWhInventoryOutItem(item);
                }
            }
            #endregion
            return sysNo;
        }

        /// <summary>
        /// 商品出库
        /// </summary>
        /// <param name="model">出库单明细</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2016-06-24 王耀发 创建</remarks>
        public override int InsertWhInventoryOutItem(WhInventoryOutItem model)
        {
            if (model.InventoryOutSysNo <= 0)
            {
                throw new Exception("入库单明细的入库单系统编号不能小于等于0");
            }
            var id = Context.Insert<WhInventoryOutItem>("WhInventoryOutItem", model)
                .AutoMap(x => x.SysNo)
                .ExecuteReturnLastId<int>("SysNo");
            return id;
        }

        /// <summary>
        /// 更新出库单
        /// </summary>
        /// <param name="model">出库单明细</param>
        /// <returns>出库单系统编号</returns>
        /// <remarks>2016-06-24 王耀发 创建</remarks>
        public override int UpdateWhInventoryOut(WhInventoryOut model)
        {
            var rowsAffected = Context.Update<WhInventoryOut>("WhInventoryOut", model)
                .AutoMap(x => x.SysNo, x => x.ItemList).Where(x => x.SysNo)
                .Execute();
            if (model.ItemList != null)
            {
                foreach (var item in model.ItemList)
                {
                    UpdateWhInventoryOutItem(item);                   
                }
            }
            return rowsAffected;
        }

//        /// <summary>
//        /// 删除入库单
//        /// </summary>
//        /// <param name="sysNo">入库单系统编号</param>
//        /// <returns>成功返回true,失败返回false</returns>
//        /// <remarks>2013-06-09 周唐炬 创建</remarks>
//        public override bool DelWhStockIn(int sysNo)
//        {
//            var rowsAffected = Context.Delete("WhStockIn")
//            .Where("SysNo", sysNo)
//            .Execute();
//            return rowsAffected > 0;
//        }

        /// <summary>
        /// 获取出库单列表
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>返回入库单列表 Ilist</returns>
        /// <remarks>
        /// 2016-06-24 王耀发 创建
        /// 2016-07-01 陈海裕 修改 新增SourceType筛选条件
        /// </remarks>
        public override Pager<WhInventoryOut> GetWhInventoryOutList(ParaInventoryOutFilter filter, int pageSize)
        {
            #region Sql 获取入库单列表
            string sql =
              @"(SELECT distinct w.*
                FROM WhInventoryOut w inner join WhInventoryOutItem i on w.SysNo=i.InventoryOutSysNo
                WHERE SourceType=10 and  (@0 IS NULL OR EXISTS
                        (SELECT 1
                            FROM splitstr(@0, ',') tmp
                        WHERE tmp.col = w.Warehousesysno))
                   AND (@1 IS NULL OR w.SourceSysNo = @1)
                   AND (@2 IS NULL OR w.Status=@2)
                   AND (@3 IS NULL OR Convert(nvarchar(10),w.CreatedDate,120) = Convert(nvarchar(10),@3,120))
                    AND (@4 IS NULL OR (@4=0 OR @4=w.SourceType))
                      AND (" + (string.IsNullOrEmpty(filter.SourceData) ? "null" : filter.SourceData) + @" IS NULL OR w.TransactionSysNo like '%" + filter.SourceData + @"%' OR w.SourceSysNo = '" + filter.SourceData + @"' OR i.ProductName like '%" + filter.SourceData + @"%' )
                ) tb     
            ";
            #endregion

            var dataList = Context.Select<WhInventoryOut>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(1)").From(sql);
            var warehouseSysNoList = string.Empty;
            if (null != filter.WarehouseSysNoList && filter.WarehouseSysNoList.Any())
            {
                warehouseSysNoList = string.Join(",", filter.WarehouseSysNoList);
            }
            var paras = new object[]
                {
                    warehouseSysNoList,
                    filter.SourceSysNo,
                    filter.Status,
                    filter.CreatedDate,
                    filter.SourceType
                };
            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<WhInventoryOut>
                {
                    Rows = dataList.OrderBy(@"tb.SysNo desc").Paging(filter.CurrentPage, pageSize).QueryMany(),
                    TotalRows = dataCount.QuerySingle()
                };

            return pager;
        }





        /// <summary>
        /// 获取调拨出库单列表
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>返回入库单列表 Ilist</returns>
        /// <remarks>
        /// 2016-06-24 王耀发 创建
        /// 2016-07-01 陈海裕 修改 新增SourceType筛选条件
        /// </remarks>
        public override Pager<WhInventoryOut> GetWhInventoryOutListTo(ParaInventoryOutFilter filter, int pageSize)
        {
            #region Sql 获取入库单列表
            string sql =
              @"(SELECT distinct w.*
                FROM WhInventoryOut w inner join WhInventoryOutItem i on w.SysNo=i.InventoryOutSysNo
                WHERE SourceType=20 and  (@0 IS NULL OR EXISTS
                        (SELECT 1
                            FROM splitstr(@0, ',') tmp
                        WHERE tmp.col = w.Warehousesysno))
                   AND (@1 IS NULL OR w.SourceSysNo = @1)
                   AND (@2 IS NULL OR w.Status=@2)
                   AND (@3 IS NULL OR Convert(nvarchar(10),w.CreatedDate,120) = Convert(nvarchar(10),@3,120))
                    AND (@4 IS NULL OR (@4=0 OR @4=w.SourceType))
                      AND (" + (string.IsNullOrEmpty(filter.SourceData) ? "null" : filter.SourceData) + @" IS NULL OR w.TransactionSysNo like '%" + filter.SourceData + @"%' OR w.SourceSysNo = '" + filter.SourceData + @"' OR i.ProductName like '%" + filter.SourceData + @"%' )
                ) tb     
            ";
            #endregion

            var dataList = Context.Select<WhInventoryOut>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(1)").From(sql);
            var warehouseSysNoList = string.Empty;
            if (null != filter.WarehouseSysNoList && filter.WarehouseSysNoList.Any())
            {
                warehouseSysNoList = string.Join(",", filter.WarehouseSysNoList);
            }
            var paras = new object[]
                {
                    warehouseSysNoList,
                    filter.SourceSysNo,
                    filter.Status,
                    filter.CreatedDate,
                    filter.SourceType
                };
            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<WhInventoryOut>
            {
                Rows = dataList.OrderBy(@"tb.SysNo desc").Paging(filter.CurrentPage, pageSize).QueryMany(),
                TotalRows = dataCount.QuerySingle()
            };

            return pager;
        }

//        /// <summary>
//        /// 根据来源单据和类型获取入库单
//        /// </summary>
//        /// <param name="sourceType">来源类型</param>
//        /// <param name="sourceNo">来源单据系统编号</param>
//        /// <returns>入库单</returns>
//        /// <remarks>2013-9-3 黄伟 创建</remarks>
//        public override WhStockIn GetStockInBySource(int sourceType, int sourceNo)
//        {
//            return Context.Sql(@"select * from whstockin where sourceType=@sourceType and sourcesysno=@sourceNo")
//                          .Parameter("sourceType", sourceType)
//                          .Parameter("sourceNo", sourceNo)
//                          .QuerySingle<WhStockIn>();
//        }

        /// <summary>
        /// 通过系统编号获取入库单明细
        /// </summary>
        /// <param name="sysNo">出库单系统编号</param>
        /// <returns>返回入库单明细,包含入库商品列表</returns>
        /// <remarks>2016-06-24 王耀发 创建</remarks>
        public override WhInventoryOut GetWhInventoryOut(int sysNo)
        {
            return Context.Sql(@"SELECT w.*
                                 FROM WhInventoryOut w WHERE w.Sysno =@SysNo")
                .Parameter("SysNo", sysNo)
                .QuerySingle<WhInventoryOut>();
        }

//        /// <summary>
//        /// 通过事务编号获取入库单明细
//        /// </summary>
//        /// <param name="transactionSysNo">事务编号</param>
//        /// <returns>返回入库单明细,包含入库商品列表</returns>
//        /// <remarks>2013-06-08 周唐炬 创建</remarks>
//        public override WhStockIn GetWhStockInByTransactionSysNo(string transactionSysNo)
//        {
//            return Context.Sql(@"SELECT * FROM WhStockIn WHERE transactionSysNo =@TransactionSysNo")
//                .Parameter("TransactionSysNo", transactionSysNo)
//                .QuerySingle<WhStockIn>();
//        }

        #endregion

        #region 商品出库
        ///// <summary>
        ///// 商品入库
        ///// </summary>
        ///// <param name="model">入库单明细</param>
        ///// <returns>返回受影响行</returns>
        ///// <remarks>2013-06-08 周唐炬 创建</remarks>
        //public override int InsertWhStockInItem(WhStockInItem model)
        //{
        //    if (model.StockInSysNo <= 0)
        //    {
        //        throw new Exception("入库单明细的入库单系统编号不能小于等于0");
        //    }
        //    var id = Context.Insert<WhStockInItem>("WhStockInItem", model)
        //        .AutoMap(x => x.SysNo)
        //        .ExecuteReturnLastId<int>("SysNo");
        //    return id;
        //}

        /// <summary>
        /// 更新商品出库信息
        /// </summary>
        /// <param name="model">出库单明细</param>
        /// <returns>返回受影响行</returns>
        /// <remarks>2016-06-24 王耀发 创建</remarks>
        public override int UpdateWhInventoryOutItem(WhInventoryOutItem model)
        {
            var rowsAffected = Context.Update<WhInventoryOutItem>("WhInventoryOutItem", model)
                .AutoMap(x => x.SysNo).Where(x => x.SysNo)
                .Execute();
            return rowsAffected;
        }

        ///// <summary>
        ///// 删除商品入库信息
        ///// </summary>
        ///// <param name="sysNo">系统编号</param>
        ///// <returns>成功返回true,失败返回false</returns>
        ///// <remarks>2013-06-09 周唐炬 创建</remarks>
        //public override bool DelWhStockInItem(int sysNo)
        //{
        //    var rowsAffected = Context.Delete("WhStockInItem")
        //    .Where("SysNo", sysNo)
        //    .Execute();
        //    return rowsAffected > 0;
        //}

        /// <summary>
        /// 通过出库单ID获取所有商品列表
        /// </summary>
        /// <param name="stockInSysNo">出库单系统SysNO</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>返回出库单商品列表</returns>
        /// <remarks>2016-06-24 王耀发 创建</remarks>
        public override IList<WhInventoryOutItem> GetWhInventoryOutItemListByInventoryOutSysNo(int InventoryOutSysNo, int pageIndex, int pageSize)
        {
            var listResult = Context.Select<WhInventoryOutItem>("t.*")
                .From("WhInventoryOutItem t")
                .Where("t.InventoryOutSysNo =@InventoryOutSysNo")
                .Parameter("InventoryOutSysNo", InventoryOutSysNo)
                .OrderBy("t.SysNo")
                .Paging(pageIndex, pageSize)
                .QueryMany();

            return listResult;
        }

        /// <summary>
        /// 通过出库单ID获取所有商品总数
        /// </summary>
        /// <param name="stockInSysNo">入库单系统stockInSysNo</param>
        /// <returns>返回出库单所有商品总数</returns>
        /// <remarks>2016-06-24 王耀发 创建</remarks>
        public override int GetWhInventoryOutItemListByInventoryOutSysNoCount(int InventoryOutSysNo)
        {
            return Context.Sql(@"SELECT count(*) FROM WhInventoryOutItem WHERE InventoryOutSysNo =@InventoryOutSysNo").Parameter("InventoryOutSysNo", InventoryOutSysNo).QuerySingle<int>();
        }

        ///// <summary>
        ///// 通过系统编号获取入库商品信息
        ///// </summary>
        ///// <param name="stockInSysNo">入库单系统编号</param>
        ///// <param name="productSysNo">商品系统编号</param>
        ///// <returns>入库商品信息</returns>
        ///// <remarks>2013-06-09 周唐炬 创建</remarks>
        //public override WhStockInItem GetWhStockInItemBySysNo(int stockInSysNo, int productSysNo)
        //{
        //    return Context.Sql(@"SELECT * FROM WhStockInItem WHERE ProductSysNo=@ProductSysNo AND StockInSysNo=@StockInSysNo")
        //        .Parameter("ProductSysNo", productSysNo)
        //        .Parameter("StockInSysNo", stockInSysNo)
        //        .QuerySingle<WhStockInItem>();
        //}

        /// <summary>
        /// 通过出库明细系统编号获取入库明细
        /// </summary>
        /// <param name="sysNo">出库明细系统编号</param>
        /// <returns>出库明细</returns>
        /// <remarks>2016-06-24 王耀发 创建</remarks>
        public override WhInventoryOutItem GetWhInventoryOutItem(int sysNo)
        {
            return Context.Sql(@"SELECT * FROM WhInventoryOutItem WHERE SysNo=@SysNo")
                .Parameter("SysNo", sysNo)
                .QuerySingle<WhInventoryOutItem>();
        }

        ///// <summary>
        ///// 通过入库单ID获取所有商品列表
        ///// </summary>
        ///// <param name="stockInSysNo">入库单系统stockInSysNo</param>
        ///// <returns>返回入库单商品列表</returns>
        ///// <remarks>2013-06-24 郑荣华 创建</remarks>
        //public override List<WhStockInItem> GetWhStockInItemList(int stockInSysNo)
        //{
        //    var listResult = Context.Select<WhStockInItem>("t.*")
        //        .From("WhStockInItem t")
        //        .Where("t.StockInSysNo =@StockInSysNo")
        //        .Parameter("StockInSysNo", stockInSysNo)
        //        .OrderBy("t.SysNo")
        //        .QueryMany();

        //    return listResult;
        //}

        ///// <summary>
        ///// 根据单据来源获取入库单
        ///// </summary>
        ///// <param name="source">单据来源</param>
        ///// <param name="sourceSysNo">单据编号</param>
        ///// <returns>入库单</returns>
        ///// <remarks>2013-7-25 朱家宏 创建 </remarks>
        //public override WhStockIn GetWhStockInByVoucherSource(int source, int sourceSysNo)
        //{
        //    return Context.Sql(@"SELECT * FROM WhStockIn  where SourceType=@source and sourceSysNo=@sourceSysNo")
        //                  .Parameter("source", source)
        //                  .Parameter("sourceSysNo", sourceSysNo)
        //                  .QuerySingle<WhStockIn>();
        //}
        ///// <summary>
        ///// 根据单据来源和状态获取入库单
        ///// </summary>
        ///// <param name="sourceType">单据来源</param>
        ///// <param name="sourceNo">单据编号</param>
        ///// <param name="Status">状态</param>
        ///// <returns></returns>
        ///// <remarks>2014-04-11 朱成果 创建 </remarks>
        //public override WhStockIn GetStockInBySourceAndStatus(int sourceType, int sourceNo, int? Status)
        //{
        //    if (Status.HasValue)
        //    {
        //        return Context.Sql(@"SELECT * FROM WhStockIn  where SourceType=@source and sourceSysNo=@sourceSysNo and Status=@Status")
        //                 .Parameter("source", sourceType)
        //                 .Parameter("sourceSysNo", sourceNo)
        //                  .Parameter("Status", Status.Value)
        //                 .QuerySingle<WhStockIn>();
        //    }
        //    else
        //    {
        //        return GetWhStockInByVoucherSource(sourceType, sourceNo);
        //    }
        //}
        #endregion

        public override List<WhInventoryOut> GetWhInventoryOutList(DateTime dateTime)
        {
            string sql = " select * from WhInventoryOut where Status=50 and LastUpdateDate>='"+dateTime.ToString("yyyy-MM-dd")+" 00:00:00' ";
            return Context.Sql(sql).QueryMany<WhInventoryOut>();
        }

        /// <summary>
        /// 通过来源单号获取入库单明细
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns>2018-01-16 罗熙 创建</returns>
        public override WhInventoryOut GetWhInventoryOutToSourceSysNo(int sysNo)
        {
            return Context.Sql(@"SELECT w.*
                                 FROM WhInventoryOut w WHERE w.SourceSysNO =@SysNo")
                .Parameter("SysNo", sysNo)
                .QuerySingle<WhInventoryOut>();
        }
    }
}
