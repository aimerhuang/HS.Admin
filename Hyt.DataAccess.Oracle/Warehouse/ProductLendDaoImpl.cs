using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Warehouse;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Oracle.Warehouse
{
    /// <summary>
    /// 借货单维护DAL
    /// </summary>
    /// <remarks>2013-07-09 周唐炬 创建</remarks>
    public class ProductLendDaoImpl : IProductLendDao
    {
        /// <summary>
        /// 业务员库存查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>业务员库存</returns>
        /// <remarks>2013-12-11 周唐炬 创建</remarks>
        public override Pager<WhProductLendItem> GetInventoryProductList(ParaProductLendFilter filter)
        {
            var pager = new Pager<WhProductLendItem>();
            const string sql = @"(select b.productsysno,
                                           b.productname,
                                           sum(b.lendquantity) as lendquantity,
                                           sum(b.salequantity) as salequantity,
                                           sum(b.returnquantity) as returnquantity
                                      from whproductlend a
                                     inner join whproductlenditem b
                                        on b.productlendsysno = a.sysno
                                     where (@0 is null or a.warehousesysno = @0)
                                       and a.deliveryusersysno = @1
                                       and a.status = @2
                                       and b.lendquantity > (b.salequantity + b.returnquantity + b.forcecompletequantity)
                                     group by b.productsysno, b.productname) tb";

            var dataList = Context.Select<WhProductLendItem>("tb.*").From(sql);
            var dataCount = Context.Select<int>("COUNT(1)").From(sql);
            var paras = new object[]
                {
                    filter.WarehouseSysNo,
                    filter.DeliveryUserSysNo,
                    filter.Status
                };
            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            pager.Rows = dataList.OrderBy("tb.productsysno").Paging(filter.CurrentPage, filter.PageSize).QueryMany();
            pager.TotalRows = dataCount.QuerySingle();
            return pager;
        }

        /// <summary>
        /// 获取借货单列表
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns>借货单列表</returns>
        /// <remarks>2013-07-09 周唐炬 创建</remarks>
        public override Pager<WhProductLend> GetWhProductLendList(ParaProductLendFilter filter)
        {
            var pager = new Pager<WhProductLend>();
            const string sql = @"(SELECT W.*
                                    FROM WHPRODUCTLEND W
                                        WHERE (@0 IS NULL OR EXISTS
                                                (SELECT 1
                                                   FROM splitstr(@0, ',') tmp
                                                  WHERE tmp.col = W.Warehousesysno))
                                            AND (@1 IS NULL OR W.Status=@1)
                                            AND (@2 IS NULL OR W.DeliveryUserSysNo=@2)
                                            AND (@3 IS NULL OR W.CreatedBy=@3)
                                            AND (@4 IS NULL OR CONVERT(varchar(10),W.CreatedDate,120) = CONVERT(varchar(10),@4,120))
                                            AND (@5 IS NULL OR W.StockOutBy=@5)
                                            AND (@4 IS NULL OR CONVERT(varchar(10),W.CreatedDate,120) = CONVERT(varchar(10),@4,120))
                                            AND (@7 IS NULL OR W.LastUpdateBy=@7)
                                            AND (@4 IS NULL OR CONVERT(varchar(10),W.CreatedDate,120) = CONVERT(varchar(10),@4,120))
                                            ) tb";

            var dataList = Context.Select<WhProductLend>("tb.*").From(sql);
            var dataCount = Context.Select<int>("COUNT(1)").From(sql);
            var warehouseSysNoList = string.Empty;
            if (filter.WarehouseSysNoList != null && filter.WarehouseSysNoList.Any())
            {
                warehouseSysNoList = string.Join(",", filter.WarehouseSysNoList);
            }

            var paras = new object[]
                {
                    warehouseSysNoList,
                    filter.Status,
                    filter.DeliveryUserSysNo,
                    filter.CreatedBy,
                    filter.CreatedDate,
                    filter.StockOutBy,
                    filter.StockOutDate,
                    filter.LastUpdateBy,
                    filter.LastUpdateDate
                };
            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            pager.Rows = dataList.OrderBy("tb.SysNO desc").Paging(filter.CurrentPage, filter.PageSize).QueryMany();
            pager.TotalRows = dataCount.QuerySingle();

            return pager;
        }

        /// <summary>
        /// 根据配送员系统编号获取取件单
        /// </summary>
        /// <param name="deliverySysNo">配送员系统编号</param>
        /// <returns>借货单列表</returns>
        /// <remarks>2013-09-28 沈强 创建</remarks>
        public override IList<WhProductLend> GetWhProductLendList(int deliverySysNo)
        {
            return
                Context.Sql("select * from WHPRODUCTLEND where DeliveryUserSysNo = @0")
                       .Parameters(deliverySysNo)
                       .QueryMany<WhProductLend>();
        }

        /// <summary>
        /// 获取配送员未完结借货单仓库系统编号列表
        /// </summary>
        /// <param name="deliveryUserSysNo">配送员系统编号</param>
        /// <returns>仓库系统编号列表</returns>
        /// <remarks>2013-07-15 周唐炬 创建</remarks>
        public override List<int> GetWhProductLendWarehouseList(int deliveryUserSysNo)
        {
            const string sql = @"SELECT WarehouseSysNo
                                    FROM WHPRODUCTLEND 
                                        WHERE DeliveryUserSysNo=@0                                               
                                               AND Status<>@1 
                                               AND Status<>@2";
            var paras = new object[]
                {
                    deliveryUserSysNo,
                    (int)WarehouseStatus.借货单状态.已完成,
                    (int)WarehouseStatus.借货单状态.作废
                };
            return Context.Sql(sql).Parameters(paras).QueryMany<int>();
        }

        /// <summary>
        /// 借货单列表导出Excel
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns>借货单列表</returns>
        /// <remarks>2013-07-09 周唐炬 创建</remarks>
        public override List<CBWhProductLend> WhProductLendExportExcel(ParaProductLendFilter filter)
        {
            const string sql = @"(SELECT W.SysNo,W.IsEnforceAllow,W.Amount,W.Status,W.Remarks,W.CreatedDate,W.StockOutDate,W.LastUpdateDate,
                                        A.WAREHOUSENAME AS WarehouseName,
                                        B.USERNAME AS CreatedByName,
                                        C.USERNAME AS LastUpdatedByName,
                                        D.USERNAME AS StockOutByName,
                                        E.USERNAME AS DeliveryUserName
                                    FROM WHPRODUCTLEND W
                                    INNER JOIN WhWarehouse A ON A.SysNo=W.WarehouseSysNo
                                    LEFT JOIN SYUSER B ON B.SYSNO=W.CREATEDBY
                                    LEFT JOIN SYUSER C ON C.SYSNO=W.LASTUPDATEBY
                                    LEFT JOIN SYUSER D ON D.SYSNO=W.StockOutBy
                                    LEFT JOIN SYUSER E ON E.SYSNO=W.DeliveryUserSysNo
                                        WHERE (@0 IS NULL OR EXISTS
                                                (SELECT 1
                                                   FROM splitstr(@0, ',') tmp
                                                  WHERE tmp.col = W.Warehousesysno))
                                            AND (@1 IS NULL OR W.Status=@1)
                                            AND (@2 IS NULL OR W.DeliveryUserSysNo=@2)
                                            AND (@3 IS NULL OR W.CreatedBy=@3)
                                            AND (@4 IS NULL OR Convert(nvarchar(10),W.CreatedDate,120) = Convert(nvarchar(10),@4,120))
                                            AND (@5 IS NULL OR W.StockOutBy=@5)
                                            AND (@6 IS NULL OR Convert(nvarchar(10),W.CreatedDate,120) = Convert(nvarchar(10),@6,120))
                                            AND (@7 IS NULL OR W.LastUpdateBy=@7)
                                            AND (@8 IS NULL OR Convert(nvarchar(10),W.CreatedDate,120) = Convert(nvarchar(10),@8,120))
                                            ) tb";

            var dataList = Context.Select<CBWhProductLend>("tb.*").From(sql);
            var warehouseSysNoList = string.Join(",", filter.WarehouseSysNoList);
            var paras = new object[]
                {
                    warehouseSysNoList,
                    filter.Status,
                    filter.DeliveryUserSysNo,
                    filter.CreatedBy,
                    filter.CreatedDate,
                    filter.StockOutBy,
                    filter.StockOutDate,
                    filter.LastUpdateBy,
                    filter.LastUpdateDate
                };
            dataList.Parameters(paras);

            var list = dataList.OrderBy("tb.SysNO desc").Paging(filter.CurrentPage, filter.PageSize).QueryMany();
            return list;
        }

        /// <summary>
        /// 通过借货单编号获取借货单
        /// </summary>
        /// <param name="sysNo">借货单系统编号</param>
        /// <returns>借货单</returns>
        /// <remarks>2013-07-09 周唐炬 创建</remarks>
        public override WhProductLend GetWhProductLend(int sysNo)
        {
            return Context.Sql(@"SELECT W.* FROM WHPRODUCTLEND W WHERE W.Sysno =@SysNo")
                .Parameter("SysNo", sysNo)
                .QuerySingle<WhProductLend>();
        }

        /// <summary>
        /// 创建借货单
        /// </summary>
        /// <param name="model">借货单实体</param>
        /// <returns>受影响的行</returns>
        /// <remarks>2013-07-09 周唐炬 创建</remarks>
        public override int CreateWhProductLend(WhProductLend model)
        {
            model.SysNo = Context.Insert<WhProductLend>("WHPRODUCTLEND", model)
                                  .AutoMap(x => x.SysNo, x => x.ItemList)
                                  .ExecuteReturnLastId<int>("SysNo");
            return model.SysNo;
        }

        /// <summary>
        /// 更新借货单
        /// </summary>
        /// <param name="model">借货单实体</param>
        /// <returns>受影响的行</returns>
        /// <remarks>2013-07-09 周唐炬 创建</remarks>
        public override int UpdateWhProductLend(WhProductLend model)
        {
            var rowsAffected = 0;
            rowsAffected = Context.Update<WhProductLend>("WHPRODUCTLEND", model)
                                 .AutoMap(x => x.SysNo, x => x.ItemList)
                                 .Where(x => x.SysNo)
                                 .Execute();
            return rowsAffected;
        }

        /// <summary>
        /// 通过配送员系统编号、商品系统编号获取借货单明细
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns>借货单明细列表</returns>
        /// <remarks>2013-07-22 周唐炬 创建</remarks>
        public override List<int> GetWhProductLendItemList(ParaWhProductLendItemFilter filter)
        {
            const string sql = @"SELECT a.SysNo
                                 FROM WhProductLendItem a
                                 INNER JOIN WhProductLend b ON a.productlendsysno = b.sysno
                                   AND b.deliveryusersysno = @0 AND b.status=@1
                                 WHERE (a.lendquantity - a.salequantity - a.returnquantity - a.forcecompletequantity) > 0
                                    AND a.productsysno = @2
                                 ORDER BY a.sysno";
            var paras = new object[]
                {
                    filter.DeliveryUserSysNo,
                    filter.Status,
                    filter.ProductSysNo
                };
            return Context.Sql(sql).Parameters(paras).QueryMany<int>();
        }

        /// <summary>
        /// 通过借货单系统编号获取未还货或销售完成的借货单明细条数
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns>借货单明细列表</returns>
        /// <remarks>2013-07-22 周唐炬 创建</remarks>
        public override int GetWhProductLendItemListCount(ParaWhProductLendItemFilter filter)
        {
            const string sql = @"SELECT count(1)
                                        FROM WhProductLendItem a
                                        INNER JOIN WhProductLend b ON a.productlendsysno = b.sysno
	                                        AND b.STATUS = @0
	                                        AND b.sysno = @1
                                        WHERE (a.lendquantity - a.salequantity - a.returnquantity - a.forcecompletequantity) > 0";
            var paras = new object[]
                {
                    filter.Status,
                    filter.ProductLendSysNo
                };
            return Context.Sql(sql).Parameters(paras).QuerySingle<int>();
        }

        /// <summary>
        /// 通过借货单系统编号获取借货单明细列表
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns>借货单明细列表</returns>
        /// <remarks>2013-07-09 周唐炬 创建</remarks>
        public override Pager<CBWhProductLendItem> GetWhProductLendItemPagerList(ParaWhProductLendItemFilter filter)
        {
            var pager = new Pager<CBWhProductLendItem>();
            const string sql = @"(SELECT A.*
	                                    ,B.Price AS Price
                                    FROM WhProductLendItem A
                                    LEFT JOIN WHPRODUCTLENDPRICE B ON B.Productlenditemsysno = A.Sysno
	                                    AND (@0 IS NULL OR B.Pricesource = @0)
	                                    AND (@1 IS NULL OR B.Sourcesysno = @1)
                                    WHERE (
                                            @2 IS NULL
                                           OR ProductLendSysNo = @2
                                            )       
                                  ) tb";

            var dataList = Context.Select<CBWhProductLendItem>("tb.*").From(sql);
            var dataCount = Context.Select<int>("COUNT(1)").From(sql);
            var paras = new object[]
                {
                    filter.PriceSource,
                    filter.SourceSysNo,
                    filter.ProductLendSysNo
                };
            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            pager.Rows = dataList.OrderBy("tb.SysNO").Paging(filter.CurrentPage, filter.PageSize).QueryMany();
            pager.TotalRows = dataCount.QuerySingle();

            return pager;
        }

        /// <summary>
        /// 通过借货单编号获取借货单明细
        /// </summary>
        /// <param name="sysNo">借货单明细系统编号</param>
        /// <returns>借货单明细</returns>
        /// <remarks>2013-07-09 周唐炬 创建</remarks>
        public override WhProductLendItem GetWhProductLendItem(int sysNo)
        {
            return Context.Sql(@"SELECT * FROM WhProductLendItem WHERE SysNo =@SysNo")
                .Parameter("SysNo", sysNo)
                .QuerySingle<WhProductLendItem>();
        }

        /// <summary>
        /// 获取带信用等级价格的借货单明细
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns>借货单明细</returns>
        /// <remarks>2013-07-22 周唐炬 创建</remarks>
        public override CBWhProductLendItem GetWhProductLendItemInfo(ParaWhProductLendItemFilter filter)
        {
            const string sql = @"SELECT A.*
	                                    ,B.Price AS Price
                                    FROM WhProductLendItem A
                                    INNER JOIN WHPRODUCTLENDPRICE B ON B.Productlenditemsysno = A.Sysno
	                                    AND (@0 IS NULL OR B.Pricesource = @0)
	                                    AND (@1 IS NULL OR B.Sourcesysno = @1)
                                     INNER JOIN WHPRODUCTLEND C ON C.SYSNO = A.PRODUCTLENDSYSNO
                                       AND (@2 IS NULL OR C.DELIVERYUSERSYSNO=@2)
                                    WHERE (@3 IS NULL OR A.ProductLendSysNo = @3)       
                                    AND (@4 IS NULL OR A.ProductSysNo=@4)
                                    AND (@5 IS NULL OR A.SysNo =@5)";
            var paras = new object[]
                {
                    filter.PriceSource,
                    filter.SourceSysNo,
                    filter.DeliveryUserSysNo,
                    filter.ProductLendSysNo,
                    filter.ProductSysNo,
                    filter.SysNo
                };
            return Context.Sql(sql).Parameters(paras).QuerySingle<CBWhProductLendItem>();
        }

        /// <summary>
        /// 创建借货单明细
        /// </summary>
        /// <param name="model">借货单明细实体</param>
        /// <returns>受影响的行</returns>
        /// <remarks>2013-07-09 周唐炬 创建</remarks>
        public override int CreateWhProductLendItem(WhProductLendItem model)
        {
            int rowsAffected = 0;
            rowsAffected = Context.Insert<WhProductLendItem>("WhProductLendItem", model)
                                  .AutoMap(x => x.SysNo)
                                  .ExecuteReturnLastId<int>("SysNo");
            return rowsAffected;
        }

        /// <summary>
        /// 更新借货单明细
        /// </summary>
        /// <param name="model">借货单明细实体</param>
        /// <returns>受影响的行</returns>
        /// <remarks>2013-07-09 周唐炬 创建</remarks>
        public override int UpdateWhProductLendItem(WhProductLendItem model)
        {
            var rowsAffected = 0;
            rowsAffected = Context.Update<WhProductLendItem>("WhProductLendItem", model)
                                  .AutoMap(x => x.SysNo).Where(x => x.SysNo)
                                  .Execute();
            return rowsAffected;
        }

        /// <summary>
        /// 创建借货商品价
        /// </summary>
        /// <param name="model">借货商品价实体</param>
        /// <returns>受影响的行</returns>
        /// <remarks>2013-07-16 周唐炬 创建</remarks>
        public override int CreateWhProductLendPrice(WhProductLendPrice model)
        {
            int rowsAffected = 0;
            rowsAffected = Context.Insert<WhProductLendPrice>("WhProductLendPrice", model)
                                  .AutoMap(x => x.SysNo)
                                  .ExecuteReturnLastId<int>("SysNo");
            return rowsAffected;
        }

        /// <summary>
        /// 更新借货商品价
        /// </summary>
        /// <param name="model">借货商品价实体</param>
        /// <returns>受影响的行</returns>
        /// <remarks>2013-07-16 周唐炬 创建</remarks>
        public override int UpdateWhProductLendPrice(WhProductLendPrice model)
        {
            int rowsAffected = 0;
            rowsAffected = Context.Update<WhProductLendPrice>("WhProductLendPrice", model)
                                  .AutoMap(x => x.SysNo).Where(x => x.SysNo)
                                  .Execute();
            return rowsAffected;
        }

        /// <summary>
        /// 获取历史借货商品价格列表
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns>历史借货商品价格列表</returns>
        /// <remarks>2013-07-16 周唐炬 创建</remarks>
        public override List<decimal> GetHistoryPrice(ParaWhProductLendItemFilter filter)
        {
            const string sql = @"
                            SELECT C.PRICE
                            FROM whproductlenditem A
                            INNER JOIN whproductlend B ON B.Sysno = A.PRODUCTLENDSYSNO
	                            AND (@0 is null or B.STATUS = @0)
                                AND (@1 is null or B.DeliveryUserSysNo=@1)
                            LEFT JOIN whproductlendprice C ON C.PRODUCTLENDITEMSYSNO = A.Sysno
	                            AND (@2 is null or C.PRICESOURCE = @2)
                            WHERE (@3 is null or A.Productsysno = @3)";
            var paras = new object[]
                {
                    filter.Status,
                    filter.DeliveryUserSysNo,
                    filter.PriceSource,
                    filter.ProductSysNo
                };
            var list = Context.Sql(sql).Parameters(paras).QueryMany<decimal>();
            return list;
        }

        /// <summary>
        /// 根据配送人员系统编号及商品系统编号获取仓库系统编号
        /// </summary>
        /// <param name="delSysNo">配送人员系统编号</param>
        /// <param name="pSysNo">商品系统编号</param>
        /// <returns>仓库系统编号</returns>
        /// <remarks>2013-07-19 黄伟 创建</remarks>
        public override int GetWhSysNoByDelUserAndProduct(int delSysNo, int pSysNo)
        {
            return Context.Sql(@"select m.warehousesysno from whproductlend m
                                    inner join whproductlenditem d
                                    on m.sysno=d.productlendsysno
                                    where m.deliveryusersysno=@delSysNo
                                    and d.productsysno=@pSysNo
                                    and rownum=1")
                          .Parameter("delSysNo", delSysNo)
                          .Parameter("pSysNo", pSysNo)
                          .QuerySingle<int>();
        }
    }
}
