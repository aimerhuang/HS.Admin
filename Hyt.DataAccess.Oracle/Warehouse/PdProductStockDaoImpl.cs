using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.DataAccess.Warehouse;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Util;
namespace Hyt.DataAccess.Oracle.Warehouse
{
    /// <summary>
    /// 取定制商品数据访问类
    /// </summary>
    /// <remarks>
    /// 2015-08-26 王耀发 创建
    /// </remarks>
    public class PdProductStockDaoImpl : IPdProductStockDao
    {
        /// <summary>
        /// 检查产品指定仓库库存是否足够减（包含锁定库存）
        /// </summary>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <param name="productSysNo">产品系统编号</param>
        /// <param name="qty">待减的数量</param>
        /// <returns></returns>
        /// <remarks>2018-01-17 杨浩 创建</remarks>
        public override bool HasProductStock(int warehouseSysNo, int productSysNo, int qty)
        {
            return Context.Sql("select count(1) from PdProductStock where WarehouseSysNo=@WarehouseSysNo and PdProductSysNo=@ProductSysNo and (StockQuantity-LockStockQuantity)<@qty")
                   .Parameter("WarehouseSysNo", warehouseSysNo)
                   .Parameter("ProductSysNo", productSysNo)
                   .Parameter("qty", qty)
                   .QuerySingle<int>() > 0;
        }
        /// <summary>
        /// 库存查询
        /// </summary>
        /// <param name="erpCode">商品编码</param>
        /// <param name="warehouseSysNo">仓库编号</param>
        /// <returns></returns>
        /// <remarks>2013-8-8 杨浩 添加</remarks>
        public override IList<Inventory> GetInventory(string[] erpCode, int warehouseSysNo)
        {
            string sqlWhere = "";
            string codes = "";
            foreach (var code in erpCode)
            {
                if (codes != "")
                    codes += ",";
                codes += "'"+code+"'";
            }
            if (codes != "")
            {
                sqlWhere = "and pd.ErpCode in(" + codes + ")";
            }
            var strSql = string.Format("SELECT ps.StockQuantity as Quantity,pd.ErpCode as MaterialNumber FROM PdProductStock as ps inner join PdProduct  as pd on ps.PdProductSysNo=pd.SysNo  where ps.WarehouseSysNo={0} " + sqlWhere, warehouseSysNo);
            return Context.Sql(strSql).QueryMany<Inventory>();
        }
        /// <summary>
        /// 获取产品所在的仓库列表
        /// </summary>
        /// <param name="productSysNo">产品系统编号</param>
        /// <returns>仓库列表</returns>
        /// <remarks>2015-9-9 杨浩 创建</remarks>
        public override IList<PdProductStock> GetProductStockListByProductSysNo(int productSysNo)
        {
            var strSql = string.Format("SELECT ps.SysNo,ps.WarehouseSysNo,ps.PdProductSysNo,ps.StockQuantity,ps.CreatedBy,ps.CreatedDate,ps.LastUpdateBy,ps.LastUpdateDate FROM PdProductStock as ps inner join WhWarehouse as wh  on wh.SysNo=ps.WarehouseSysNo WHERE ps.PdProductSysNo={0} and wh.Status=1 Order by ps.StockQuantity desc", productSysNo);
            return Context.Sql(strSql).QueryMany<PdProductStock>();
        }

        /// <summary>
        /// 获取产品所在的仓库列表
        /// </summary>
        /// <param name="productSysNo">产品系统编号</param>
        /// <returns>仓库列表</returns>
        /// <remarks>2016-6-3 王耀发 创建</remarks>
        public override IList<PdProductStockList> GetProStockListByProductSysNo(int productSysNo)
        {
            var strSql = string.Format("SELECT ps.PdProductSysNo,ps.StockQuantity,wh.BackWarehouseName FROM PdProductStock as ps inner join WhWarehouse as wh  on wh.SysNo=ps.WarehouseSysNo WHERE ps.PdProductSysNo={0} and ps.StockQuantity > 0 and wh.Status=1 Order by ps.StockQuantity desc", productSysNo);
            return Context.Sql(strSql).QueryMany<PdProductStockList>();
        }

        /// <summary>
        /// 库存信息
        /// </summary>
        /// <param name="filter">库存信息</param>
        /// <returns>返回库存信息</returns>
        /// <remarks>2015-08-27 王耀发 创建</remarks>
        public override Pager<CBPdProductStockList> GetPdProductStockList(ParaProductStockFilter filter)
        {
            Decimal typeFrom_StockQuantity = 0;

            Decimal typeTo_StockQuantity = 0;

            if (!string.IsNullOrWhiteSpace(filter.From_StockQuantity) && !Decimal.TryParse(filter.From_StockQuantity, out typeFrom_StockQuantity))
            {
                filter.From_StockQuantity = "0";
            }
            if (!string.IsNullOrWhiteSpace(filter.To_StockQuantity) && !Decimal.TryParse(filter.To_StockQuantity, out typeTo_StockQuantity))
            {
                filter.To_StockQuantity = "0";
            }

          

            string where = "WHERE 1=1";
            if (filter.Status != "全部")
            {
                where += " AND Status = '" + filter.Status + "'";
            }
            if (!string.IsNullOrWhiteSpace(filter.ProductSysNos))
            {
                where += " AND ProductSysNo in(" + filter.ProductSysNos + ")";
            }
//            string sql = @"(select p.SysNo as ProductSysNo,p.ErpCode,p.EasName,s.*,w.BackWarehouseName as BackWarehouseName,w.StreetAddress,case when s.sysno is null then '未入库' else '已入库' end as Status, PdPrice.Price
//                            
//                            " + (filter.ProductCategory > 0?",PdCategory.SysNos as PdCategorySysNos  ":" ")+@"
//                            from PdProduct p  
//                            "+(filter.ProductCategory > 0?@"
//
//                            inner join PdCategoryAssociation on PdCategoryAssociation.ProductSysNo=p.SysNo and PdCategoryAssociation.IsMaster = 1 
//                            inner join PdCategory on PdCategoryAssociation.CategorySysNo=PdCategory.SysNo  ":"")+ @"
//                            left join (select * from PdProductStock where WarehouseSysNo = @WarehouseSysNo ) as s on p.SysNo = s.PdProductSysNo
//                            left join WhWarehouse w on s.WarehouseSysNo = w.SysNo  left join PdPrice on PdPrice.ProductSysNo=p.SysNo and PriceSource=10 and SourceSysNo=1 ";

            string sql = @"(select p.SysNo as ProductSysNo,p.ErpCode,p.EasName,s.*,w.BackWarehouseName as BackWarehouseName,w.StreetAddress,case when s.sysno is null then '未入库' else '已入库' end as Status, 0 as Price
                            ,PdCategory.SysNos as PdCategorySysNos
                            from PdProduct p  
                            inner join PdCategoryAssociation on PdCategoryAssociation.ProductSysNo=p.SysNo and PdCategoryAssociation.IsMaster = 1 
                            inner join PdCategory on PdCategoryAssociation.CategorySysNo=PdCategory.SysNo
                            left join (select * from PdProductStock where WarehouseSysNo = @WarehouseSysNo ) as s on p.SysNo = s.PdProductSysNo
                            left join WhWarehouse w on s.WarehouseSysNo = w.SysNo ";

            if (filter.WhPositionSysNo > 0)
            {
                sql += @" INNER JOIN WhProductWarehousePositionAssociation WPA ON s.SysNo=WPA.ProductStockSysNo
                            INNER JOIN WhWarehousePosition WP ON WPA.WarehousePositionSysNo=WP.SysNo ";
            }
            sql += @" where p.[Status] <> 2 and    
                            (@ErpCode is null or p.ErpCode like @ErpCode) and 
                            (@Barcode is null or p.Barcode like @Barcode) and       
                            (@EasName is null or p.EasName like @EasName) and
                            (@From_StockQuantity is null or s.StockQuantity >= @From_StockQuantity) and 
                            (@To_StockQuantity is null or s.StockQuantity <= @To_StockQuantity) ";
            if (!string.IsNullOrWhiteSpace(filter.KeyWord))
            {
                sql += "and (p.ErpCode like @KeyWord or p.Barcode like @KeyWord or p.EasName like @KeyWord or p.ProductName like @KeyWord)";
            }
            if (filter.WhPositionSysNo > 0)
            {
                sql += " AND WP.SysNo=" + filter.WhPositionSysNo;
            }

            if (filter.ProductCategory > 0)
            {
                sql += " AND PdCategory.SysNos like '%," + filter.ProductCategory+",%'";
            }

            if (filter.DsDealerSysNo > 0)
            {
                sql += " AND p.DealerSysNo=" + filter.DsDealerSysNo;
            }

            sql += " ) tb " + where;

            var dataList = Context.Select<CBPdProductStockList>("tb.*").From(sql)
                .Parameter("WarehouseSysNo", filter.WarehouseSysNo)
                .Parameter("ErpCode", "%" + filter.ErpCode + "%")
                .Parameter("Barcode", "%" + filter.Barcode + "%")
                .Parameter("EasName", "%" + filter.EasName + "%")
                .Parameter("From_StockQuantity", filter.From_StockQuantity)
                .Parameter("To_StockQuantity", filter.To_StockQuantity)
                .Parameter("KeyWord","%" + filter.KeyWord + "%");
            var dataCount = Context.Select<int>("count(1)").From(sql)
                .Parameter("WarehouseSysNo", filter.WarehouseSysNo)
                .Parameter("ErpCode", "%" + filter.ErpCode + "%")
                .Parameter("Barcode", "%" + filter.Barcode + "%")
                .Parameter("EasName", "%" + filter.EasName + "%")
                .Parameter("From_StockQuantity", filter.From_StockQuantity)
                .Parameter("To_StockQuantity", filter.To_StockQuantity)
                .Parameter("KeyWord","%" + filter.KeyWord + "%");

            var pager = new Pager<CBPdProductStockList>
            {
                CurrentPage = filter.Id,
                PageSize = filter.PageSize
            };
            var totalRows = dataCount.QuerySingle();
            var rows = dataList.OrderBy("tb.LastUpdateDate desc").Paging(pager.CurrentPage, pager.PageSize).QueryMany();

            pager.TotalRows = totalRows;
            pager.Rows = rows;

            return pager;
        }



        

        /// <summary>
        /// 库存信息
        /// </summary>
        /// <param name="filter">库存信息</param>
        /// <returns>返回库存信息</returns>
        /// <remarks>2017-08-8 吴琨 创建</remarks>
        public override List<CBPdProductStockList> GetPdProductStockListData(string WhInventoryStr, string whereStr, string BrandSysNo, string PdCategoryId)
        {
            string sql = @"select p.SysNo as ProductSysNo,p.ErpCode,p.EasName,s.*,w.BackWarehouseName as BackWarehouseName,w.StreetAddress,case when s.sysno is null then '未入库' else '已入库' end as Status, 0 as Price
                            
                            ,PdCategory.SysNos as PdCategorySysNos
                            from PdProduct p  
                            inner join PdCategoryAssociation on PdCategoryAssociation.ProductSysNo=p.SysNo and PdCategoryAssociation.IsMaster = 1 
                            inner join PdCategory on PdCategoryAssociation.CategorySysNo=PdCategory.SysNo
                            left join (select * from PdProductStock where WarehouseSysNo in( " + WhInventoryStr + " ) ) as s on p.SysNo = s.PdProductSysNo left join WhWarehouse w on s.WarehouseSysNo = w.SysNo  where p.[Status] <> 2 and s.sysno is not null ";
          
            if (!string.IsNullOrEmpty(whereStr))  sql += " and "+whereStr;

            if (!string.IsNullOrEmpty(BrandSysNo)) sql += " and BrandSysNo in(" + BrandSysNo + ") ";

            if (!string.IsNullOrEmpty(PdCategoryId)) sql += " and PdCategory.SysNo in(" + PdCategoryId + ") ";
        
            return Context.Sql(sql).QueryMany<CBPdProductStockList>();
        }










        /// <summary>
        /// 
        /// </summary>
        /// <param name="WarehouseSysNo"></param>
        /// <param name="PdProductSysNo"></param>
        /// <returns></returns>
        /// <remarkss>2015-08-06 王耀发 创建</remarks>
        public override PdProductStock GetEntityByWP(int WarehouseSysNo, int PdProductSysNo)
        {

            return Context.Sql("select a.* from PdProductStock a where a.WarehouseSysNo=@WarehouseSysNo and a.PdProductSysNo=@PdProductSysNo")
                   .Parameter("WarehouseSysNo", WarehouseSysNo)
                   .Parameter("PdProductSysNo", PdProductSysNo)
              .QuerySingle<PdProductStock>();
        }

        #region 数据记录增，删，改，查
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2015-08-21  王耀发 创建</remarks>
        public override int Insert(PdProductStock entity)
        {
            entity.SysNo = Context.Insert("PdProductStock", entity)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");

            ///更新库存信息，修改商品档案的更新时间
            string sql = " update PdProduct set LastUpdateDate='" + DateTime.Now.ToString() + "' where SysNo='" + entity.PdProductSysNo + "' ";
            return entity.SysNo;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>修改记录编号</returns>
        /// <remarks>2015-08-21  王耀发 创建</remarks>
        public override int Update(PdProductStock entity)
        {

            return Context.Update("PdProductStock", entity)
                   .AutoMap(o => o.SysNo)
                   .Where("SysNo", entity.SysNo)
                   .Execute();
        }
        /// <summary>
        /// 获得商品库存，根据商品编号字符串
        /// </summary>
        /// <param name="warehouseSysNo"></param>
        /// <param name="productSysNos"></param>
        /// <returns></returns>
        public override IList<PdProductStock> GetPdProductStockList(int warehouseSysNo, int[] productSysNos)
        {
            string sysNosString = string.Join(",", productSysNos);
            //            string sql = @"select * from WhStockOutItem a 
            //                            where exists(
            //                                            select 1 from splitstr(@0,',') b where b.col = a.sysno
            //                                        )";
            string sql = @"select * from PdProductStock where WarehouseSysNo = @warehouseSysNo and PdProductSysNo in (select distinct col from [dbo].[splitstr](@sysNosString,','))";
            return Context.Sql(sql)
                   .Parameter("warehouseSysNo", warehouseSysNo)
                   .Parameter("sysNosString", sysNosString)
                .QueryMany<PdProductStock>();
        }
        /// <summary>
        /// 更新库存数量
        /// </summary>
        /// <param name="WarehouseSysNo">仓库编号</param>
        /// <param name="PdProductSysNo">产品编号</param>
        /// <param name="Quantity">商品数</param>
        /// <remarks>2016-5-25 杨浩 添加注释并将返回值改为影响行数</remarks>
        /// <remarks>2017-9-09 罗勤尧 减库存的同时 减锁定的库存数</remarks>
        public override int UpdateStockQuantity(int WarehouseSysNo, int PdProductSysNo, decimal Quantity)
        {
            //先注释，以免造成数据异常
            //string sql = @"update PdProductStock set StockQuantity = StockQuantity - (" + Quantity + "),LockStockQuantity = LockStockQuantity - (" + Quantity + "),LastUpdateDate='" + DateTime.Now.ToString() + "' where WarehouseSysNo = " + WarehouseSysNo + " and PdProductSysNo = " + PdProductSysNo + "";
            string sql = @"update PdProductStock set StockQuantity = StockQuantity - (" + Quantity + "),LastUpdateDate='" + DateTime.Now.ToString() + "' where WarehouseSysNo = " + WarehouseSysNo + " and PdProductSysNo = " + PdProductSysNo + "";
            return Context.Sql(sql).Execute();
        }
        /// <summary>
        /// 更新锁定库存数，
        /// </summary>
        /// <param name="warehouseSysNo"></param>
        /// <param name="productSysNo"></param>
        /// <param name="quantity"></param>
        /// <remarks>2017-9-08 罗勤尧 创建</remarks>
        public override void UpdateLockStockQuantity(int warehouseSysNo, int productSysNo, int quantity)
        {
            string sql = @"Update PdProductStock Set LockStockQuantity = Case When LockStockQuantity + @Quantity < 0 Then 0 Else LockStockQuantity + @Quantity End Where WarehouseSysNo = @WarehouseSysNo And PdProductSysNo = @PdProductSysNo";
            Context.Sql(sql)
            .Parameter("WarehouseSysNo", warehouseSysNo)
            .Parameter("PdProductSysNo", productSysNo)
            .Parameter("Quantity", quantity).Execute();
        }

        /// <summary>
        /// 释放锁定库存数，
        /// </summary>
        /// <param name="warehouseSysNo"></param>
        /// <param name="productSysNo"></param>
        /// <param name="quantity"></param>
        /// <remarks>2017-9-08 罗勤尧 创建</remarks>
        public override void RollbackLockStockQuantity(int warehouseSysNo, int productSysNo, int quantity)
        {
            string sql = @"Update PdProductStock Set LockStockQuantity = Case When LockStockQuantity - @Quantity < 0 Then 0 Else LockStockQuantity - @Quantity End Where WarehouseSysNo = @WarehouseSysNo And PdProductSysNo = @PdProductSysNo";
            Context.Sql(sql)
            .Parameter("WarehouseSysNo", warehouseSysNo)
            .Parameter("PdProductSysNo", productSysNo)
            .Parameter("Quantity", quantity).Execute();
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>删除数据编号</returns>
        /// <remarks>2016-08-1 罗远康 创建</remarks>
        public override int Delete(int sysNo)
        {
            return Context.Sql("DELETE FROM PdProductStock WHERE StockQuantity=0 AND SysNo=@SysNo")
              .Parameter("SysNo", sysNo)
              .Execute();
        }
        #endregion

        /// <summary>
        /// 获取所有库存信息
        /// </summary>
        /// <returns>库存信息集合</returns>
        /// <remarks>2015-12-15 王耀发 创建</remarks>
        public override IList<PdProductStock> GetAllProductStock()
        {
            const string strSql = @"select * from PdProductStock";
            var entity = Context.Sql(strSql)
                                .QueryMany<PdProductStock>();
            return entity;
        }

        /// <summary>
        /// 获取当前仓库对应的库存记录
        /// </summary>
        /// <param name="WarehouseSysNo"></param>
        /// <returns></returns>
        /// <remarks>2015-12-15 王耀发 创建</remarks>
        public override IList<PdProductStock> GetProStockByWarehouseSysNo(int WarehouseSysNo)
        {
            const string strSql = @"select * from PdProductStock where WarehouseSysNo = @WarehouseSysNo";
            var entity = Context.Sql(strSql)
                .Parameter("WarehouseSysNo", WarehouseSysNo)
                .QueryMany<PdProductStock>();
            return entity;
        }

        /// <summary>
        /// 新增商品库存信息
        /// </summary>
        /// <param name="models">商品信息列表</param>
        /// <returns>空</returns>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        public override void CreateExcelProductStock(List<PdProductStock> models)
        {
            foreach (PdProductStock model in models)
            {
                PdProductStock PdStockData = new PdProductStock();
                PdStockData.WarehouseSysNo = model.WarehouseSysNo;
                PdStockData.PdProductSysNo = model.PdProductSysNo;
                PdStockData.Barcode = model.Barcode;
                PdStockData.CustomsNo = model.CustomsNo;
                PdStockData.CostPrice = model.CostPrice;
                PdStockData.StockQuantity = model.StockQuantity;
                PdStockData.LockStockQuantity = model.LockStockQuantity;
                PdStockData.CreatedBy = model.CreatedBy;
                PdStockData.CreatedDate = model.CreatedDate;
                PdStockData.LastUpdateBy = model.LastUpdateBy;
                PdStockData.LastUpdateDate = model.LastUpdateDate;
                PdStockData.InStockTime = model.InStockTime;
                PdStockData.Remark = model.Remark;
                int ProductSysNo = Context.Insert<PdProductStock>("PdProductStock", PdStockData)
                                       .AutoMap(o => o.SysNo)
                                       .ExecuteReturnLastId<int>("SysNo");
            }
        }
        public override void CreateExcelProductStockYS(List<PdProductStock> models)
        {
            foreach (PdProductStock model in models)
            {
                PdProductStock PdStockData = new PdProductStock();
                PdStockData.WarehouseSysNo = model.WarehouseSysNo;
                PdStockData.PdProductSysNo = model.PdProductSysNo;
                PdStockData.Barcode = model.Barcode;
                PdStockData.ProductSku = model.ProductSku;
                PdStockData.CustomsNo = model.CustomsNo;
                PdStockData.CostPrice = model.CostPrice;
                PdStockData.StockQuantity = model.StockQuantity;
                PdStockData.LockStockQuantity = model.LockStockQuantity;
                PdStockData.CreatedBy = model.CreatedBy;
                PdStockData.CreatedDate = model.CreatedDate;
                PdStockData.LastUpdateBy = model.LastUpdateBy;
                PdStockData.LastUpdateDate = model.LastUpdateDate;
                int ProductSysNo = Context.Insert<PdProductStock>("PdProductStock", PdStockData)
                                       .AutoMap(o => o.SysNo)
                                       .ExecuteReturnLastId<int>("SysNo");
            }
        }
        /// <summary>
        /// 更新商品库存信息
        /// </summary>
        /// <param name="models">商品信息列表</param>
        /// <returns>空</returns>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        public override void UpdateExcelProductStock(List<PdProductStock> models)
        {
            foreach (PdProductStock model in models)
            {
                string sql = @"update PdProductStock set StockQuantity = @StockQuantity,Barcode = @Barcode,CustomsNo = @CustomsNo,CostPrice = @CostPrice,InStockTime=@InStockTime,Remark=@Remark where WarehouseSysNo = @WarehouseSysNo and PdProductSysNo = @PdProductSysNo";
                Context.Sql(sql)
                .Parameter("WarehouseSysNo", model.WarehouseSysNo)
                .Parameter("PdProductSysNo", model.PdProductSysNo)
                .Parameter("StockQuantity", model.StockQuantity)
                .Parameter("Barcode", model.Barcode)
                .Parameter("CustomsNo", model.CustomsNo)
                .Parameter("CostPrice", model.CostPrice)
                .Parameter("InStockTime", model.InStockTime)
                .Parameter("Remark", model.Remark).Execute();
            }
        }

        /// <summary>
        /// 更新商品库存日期信息
        /// </summary>
        /// <param name="models">商品信息列表</param>
        /// <returns>空</returns>
        /// <remarks>2017-07-10 罗勤尧 创建</remarks>
        public override void UpdateExcelProductStockDate(List<PdProductStock> models)
        {
            foreach (PdProductStock model in models)
            {
                string sql = @"update PdProductStock set Barcode = @Barcode,CustomsNo = @CustomsNo,CostPrice = @CostPrice,InStockTime=@InStockTime where WarehouseSysNo = @WarehouseSysNo and PdProductSysNo = @PdProductSysNo";
                Context.Sql(sql)
                .Parameter("WarehouseSysNo", model.WarehouseSysNo)
                .Parameter("PdProductSysNo", model.PdProductSysNo)
                .Parameter("Barcode", model.Barcode)
                .Parameter("CustomsNo", model.CustomsNo)
                .Parameter("CostPrice", model.CostPrice)
                .Parameter("InStockTime", model.InStockTime).Execute();
            }
        }
        public override void UpdateExcelProductStockYS(List<PdProductStock> models)
        {
            foreach (PdProductStock model in models)
            {
                string sql = @"update PdProductStock set StockQuantity = @StockQuantity,Barcode = @Barcode,ProductSku=@ProductSku,CustomsNo = @CustomsNo,CostPrice = @CostPrice where WarehouseSysNo = @WarehouseSysNo and PdProductSysNo = @PdProductSysNo";
                Context.Sql(sql)
                .Parameter("WarehouseSysNo", model.WarehouseSysNo)
                .Parameter("PdProductSysNo", model.PdProductSysNo)
                .Parameter("StockQuantity", model.StockQuantity)
                .Parameter("Barcode", model.Barcode)
                .Parameter("ProductSku",model.ProductSku)
                .Parameter("CustomsNo", model.CustomsNo)
                .Parameter("CostPrice", model.CostPrice).Execute();
            }
        }
        /// <summary>
        /// 查询导出商品库存列表
        /// </summary>
        /// <param name="sysNos"></param>
        /// <param name="sysNos"></param>
        /// <returns></returns>
        /// <remarks>2015-12-30 王耀发 创建</remarks>
        public override List<CBOutputPdProductStocks> GetExportProductStockList(string warehouseSysNo, List<int> sysNos)
        {
            string sqlText = @"
            select b.BackWarehouseName AS 仓库名称,c.ErpCode AS 商品编码,c.EasName AS 后台显示名称,a.Barcode AS 条形码,a.CustomsNo AS 海关备案号,a.CostPrice AS 采购价格,a.StockQuantity AS 库存数量,PdPrice.price as 会员价格
                   
            from PdProductStock a left join WhWarehouse b on a.WarehouseSysNo = b.SysNo
            left join PdProduct c on a.PdProductSysNo = c.SysNo 
            left join PdPrice on PdPrice.ProductSysNo=c.SysNo and PriceSource=10 and SourceSysNo=1 
            where a.WarehouseSysNo = " + warehouseSysNo;

            if (sysNos.Count != 0)
            {
                sqlText += " and a.SysNo in (" + string.Join(",", sysNos) + ")";
            }

            List<CBOutputPdProductStocks> outProductStocks = Context.Sql(sqlText).QueryMany<CBOutputPdProductStocks>();

            return outProductStocks;
        }

        /// <summary>
        /// 查询导出商品库存列表
        /// </summary>
        /// <param name="sysNos"></param>
        /// <param name="sysNos"></param>
        /// <returns></returns>
        /// <remarks>2015-12-30 王耀发 创建</remarks>
        public override List<CBOutputPdProductAlarmStocks> GetExportProductStockList(string warehouseSysNo, bool bAlarm=false)
        {
            string sqlWhere = "";
            string sqlText = @"
            select b.BackWarehouseName AS 仓库名称,c.ErpCode AS 商品编码,c.EasName AS 商品名称,a.Barcode AS 条形码,a.CustomsNo AS 海关备案号,a.CostPrice AS 采购价格,a.StockQuantity AS 库存数量
                    ,d.Lowerlimit as 下限,d.Upperlimit as 上限
            from PdProductStock a left join WhWarehouse b on a.WarehouseSysNo = b.SysNo
            left join PdProduct c on a.PdProductSysNo = c.SysNo 
            left join WhInventoryAlarm d on d.ProductStockSysNo=a.SysNo ";
           if(!string.IsNullOrEmpty(warehouseSysNo))
           {
               sqlWhere += "  where a.WarehouseSysNo = " + warehouseSysNo;
           }
           if (string.IsNullOrEmpty(sqlWhere))
            {
                sqlWhere += "  where c.EasName is not null ";
            }
            else
            {
                sqlWhere += "  and c.EasName is not null ";
            }

           if (bAlarm)
           {
               if (!string.IsNullOrEmpty(sqlWhere))
               {
                   sqlWhere += "  and ";
               }
               else
               {
                   sqlWhere += "  where ";
               }
               sqlWhere += " ((a.StockQuantity> d.Upperlimit and d.Upperlimit>0) or (a.StockQuantity < d.Lowerlimit and d.Lowerlimit>0)) ";
           }
            sqlText += sqlWhere;

           List<CBOutputPdProductAlarmStocks> outProductStocks = Context.Sql(sqlText).QueryMany<CBOutputPdProductAlarmStocks>();

            return outProductStocks;
        }

        public override List<CBOutputPdProductStocksYS> GetExportProductStockListYS(string warehouseSysNo, List<int> sysNos)
        {
            string sqlText = @"select b.BackWarehouseName AS 仓库名称,c.ErpCode AS 商品编码,c.EasName AS 后台显示名称,a.Barcode AS 条形码,a.ProductSku AS 商品SKU,a.CustomsNo AS 海关备案号,a.CostPrice AS 采购价格,a.StockQuantity AS 库存数量
            from PdProductStock a left join WhWarehouse b on a.WarehouseSysNo = b.SysNo
            left join PdProduct c on a.PdProductSysNo = c.SysNo where a.WarehouseSysNo = " + warehouseSysNo;

            if (sysNos.Count != 0)
            {
                sqlText += " and a.SysNo in (" + string.Join(",", sysNos) + ")";
            }

            List<CBOutputPdProductStocksYS> outProductStocks = Context.Sql(sqlText).QueryMany<CBOutputPdProductStocksYS>();

            return outProductStocks;
        }
        /// <summary>
        /// 更新商品库存信息
        /// </summary>
        /// <param name="models">商品信息列表</param>
        /// <returns>空</returns>
        /// <remarks>
        /// 2015-09-10 王耀发 创建
        /// 2016-04-09 陈海裕 修改 添加ProductSku
        /// 2016-9-20 修改商品库存，更新商品档案时间
        /// </remarks>
        public override void UpdateProductStockInfo(PdProductStock model)
        {
            string sql = @"update PdProductStock set Barcode = @Barcode,CustomsNo = @CustomsNo,CostPrice = @CostPrice,StockQuantity = @StockQuantity,LastUpdateBy = @LastUpdateBy,
                    LastUpdateDate = @LastUpdateDate,ProductSku=@ProductSku ,InStockTime=@InStockTime,Remark=@Remark where SysNo = @SysNo ";
            Context.Sql(sql)
            .Parameter("Barcode", model.Barcode)
            .Parameter("CustomsNo", model.CustomsNo)
            .Parameter("CostPrice", model.CostPrice)
            .Parameter("StockQuantity", model.StockQuantity)
            .Parameter("LastUpdateBy", model.LastUpdateBy)
            .Parameter("LastUpdateDate", model.LastUpdateDate)
            .Parameter("ProductSku", model.ProductSku)
            .Parameter("InStockTime", model.InStockTime)
            .Parameter("Remark", model.Remark)
            .Parameter("SysNo", model.SysNo).Execute();

            ///更新库存信息，修改商品档案的更新时间
            PdProductStock tempMod = Context.Sql(" select * from PdProductStock where SysNo='" + model.SysNo + "' ").QuerySingle<PdProductStock>();
            sql = " update PdProduct set LastUpdateDate='" + DateTime.Now.ToString() + "' where SysNo='" + tempMod.PdProductSysNo + "' ";
            Context.Sql(sql).Execute();
        }

        /// <summary>
        /// 获取库存中商品明细
        /// </summary>
        /// <param name="warehouseNo"></param>
        /// <returns></returns>
        public override List<PdProductStock> GetPdProductStockList(int warehouseNo)
        {
            string strSql = @"select * from PdProductStock  where WarehouseSysNo = @WarehouseSysNo";
            var entity = Context.Sql(strSql)
                .Parameter("WarehouseSysNo", warehouseNo)
                .QueryMany<PdProductStock>();
            return entity;
        }

        /// <summary>
        /// 查分销商绑定库存商品列表的分页数据列表
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        ///<remarks>
        /// 2016-03-14 杨云奕 添加
        /// </remarks>
        public override Pager<PdProductStockList> DoDealerPdProductStockDetailQuery(ParaProductStockFilter filter)
        {
            string sql = @"(select 
                                    PdProductStock.* , PdProduct.ErpCode , PdProduct.EasName ,DsDealer.DealerName as BackWarehouseName
                                    from PdProduct  
                                    inner join PdProductStock on PdProduct.SysNo=PdProductStock.PdProductSysNo
                                    inner join DsDealerWharehouse on DsDealerWharehouse.WarehouseSysNo=PdProductStock.WarehouseSysNo
                                    inner join DsDealer on DsDealer.SysNo=DsDealerWharehouse.DealerSysNo  ";
            if (filter.DsDealerSysNo > 0)
            {
                sql += " where  DsDealer.SysNo = " + filter.DsDealerSysNo;
            }
            sql += " ) tb ";

            var dataList = Context.Select<PdProductStockList>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(1)").From(sql);

            var pager = new Pager<PdProductStockList>
            {
                CurrentPage = filter.Id,
                PageSize = filter.PageSize
            };
            var totalRows = dataCount.QuerySingle();
            var rows = dataList.OrderBy("tb.LastUpdateDate desc").Paging(pager.CurrentPage, pager.PageSize).QueryMany();

            pager.TotalRows = totalRows;
            pager.Rows = rows;

            return pager;
        }

        public override PdProductStock GetPdProductStockBySysNo(int SysNo)
        {
            string sql = " select * from PdProductStock where SysNo='" + SysNo + "' ";
            return Context.Sql(sql).QuerySingle<PdProductStock>();
        }

        public override IList<PdProductStock> GetAllStockList(int warehouseSysNo,IList<int> proSysNos)
        {
            string sql = " select * from PdProductStock where WarehouseSysNo='" + warehouseSysNo + "' and PdProductSysNo in (" + string.Join(",", proSysNos.ToArray()) + ") ";
            try
            {
                return Context.Sql(sql).QueryMany<PdProductStock>();
            }
            catch (Exception)
            {
                return null;
            }
            
        }

        /// <summary>
        /// 同步库存
        /// </summary>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <param name="productSysNo">产品系统编号</param>
        /// <param name="quantity">库存数</param>
        /// <returns></returns>
        /// <remarks>2017-1-11 杨浩 创建</remarks>
        public override int SynchronizeStock(int warehouseSysNo, int productSysNo, int quantity)
        {
            string sql = @"update PdProductStock set StockQuantity =" + quantity + ",LastUpdateDate='" + DateTime.Now.ToString() + "' where WarehouseSysNo = " + warehouseSysNo + " and PdProductSysNo = " + productSysNo + "";
            return Context.Sql(sql).Execute();
        }

        /// <summary>
        /// 获取无效库存的产品系统编号列表
        /// </summary>
        /// <returns></returns>
        /// <remarks>2017-3-23 杨浩 创建</remarks>
        public override IList<int> GetInvalidInventoryProductSysNoList()
        {
            return Context.Sql("select [PdProductSysNo] from [PdProductStock] group by [PdProductSysNo] having sum(StockQuantity)<=0").QueryMany<int>();
        }


        /// <summary>
        /// 根据商品编号获取条码
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        public override string GetBarcode(string Code)
        {
            return Context.Sql("select top 1 Barcode from PdProduct where ErpCode=@ErpCode").Parameter("ErpCode", Code).QuerySingle<string>();
        }

    }
}
