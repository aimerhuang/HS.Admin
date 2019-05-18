using Hyt.DataAccess.Supply;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Supply
{
    /// <summary>
    /// 供应商商品
    /// </summary>
    /// <remarks> 2013-6-25 杨浩 创建</remarks>
    public class ScProductDaoImpl : IScProductDao
    {

        /// <summary>
        /// 检测产品sku是否已存在
        /// </summary>
        /// <param name="sku">sku</param>
        /// <param name="supplyCode">供应链代码</param>
        /// <returns></returns>
        /// <remarks> 2016-3-18  杨浩 创建</remarks>
        public override bool CheckProductSku(string sku, int supplyCode)
        {
            var r = Context.Sql("SELECT COUNT(1) FROM ScProduct WHERE sku=@sku and supplyCode=@supplyCode")
                  .Parameter("sku", sku)
                  .Parameter("supplyCode", supplyCode)
                  .QuerySingle<int>();
            return r > 0;
        }
        /// <summary>
        /// 添加供应链产品
        /// </summary>
        /// <param name="model">供应链商品实体</param>
        /// <returns></returns>
        /// <remarks> 2013-6-25 杨浩 创建</remarks>
        /// <remarks> 2016-3-21 刘伟豪 修改</remarks>
        public override int AddScProduct(ScProduct model)
        {
            int id = Context.Insert<ScProduct>("ScProduct", model)
                     .AutoMap(x => x.SysNo)
                     .ExecuteReturnLastId<int>("SysNo");
            return id;
        }
        /// <summary>
        /// 更新供应链产品
        /// </summary>
        /// <param name="model">供应链商品实体</param>
        /// <returns></returns>
        /// <remarks> 2013-6-25 杨浩 创建</remarks>
        public override int UpdateScProduct(ScProduct model)
        {
            var r = Context.Update<ScProduct>("ScProduct", model)
                  .AutoMap(x => x.SysNo)
                  .Where("SysNo", model.SysNo)
                  .Execute();
            return r;
        }
        /// <summary>
        /// 更新供应链商品
        /// </summary>
        /// <param name="SKU"></param>
        /// <param name="ProTitle"></param>
        /// <param name="Receipt"></param>
        /// <returns></returns>
        public override bool UpdateScProduct(string SKU,string ProTitle,string Receipt)
        {
            var r = Context.Sql("update ProductName=@ProductName,Receipt=@Receipt where SKU=@SKU")
                  .Parameter("ProductName", ProTitle)
                  .Parameter("Receipt", Receipt)
                  .Parameter("SKU", SKU)
                  .QuerySingle<int>();
            return r > 0;
        }


        /// <summary>
        /// 删除供应链产品
        /// </summary>
        /// <param name="sysNo">供应商产品编号</param>
        /// <returns></returns>
        /// <remarks> 2013-6-25 杨浩 创建</remarks>
        public override int DeleteScProduct(int sysNo)
        {
            return Context.Delete("ScProduct").Where("SysNo", sysNo).Execute();
        }
        /// <summary>
        /// 获取供应商产品详情
        /// </summary>
        /// <param name="sysNo">供应商产品编号</param>
        /// <returns></returns>
        /// <remarks> 2013-6-25 杨浩 创建</remarks>
        public override ScProduct GetScProductInfo(int sysNo)
        {
            return Context.Sql("SELECT * FROM ScProduct WHERE SysNo=@0", sysNo)
                         .QuerySingle<ScProduct>();
        }
        /// <summary>
        /// 获取供应商产品详情
        /// </summary>
        /// <param name="sku">sku</param>
        /// <param name="supplyCode">供应链代码</param>
        /// <returns></returns>
        /// <remarks> 2016-3-17  杨浩 创建</remarks>
        public override ScProduct GetScProductInfo(string sku, int supplyCode)
        {
            return Context.Sql("SELECT * FROM ScProduct  WHERE sku=@sku and supplyCode=@supplyCode")
                          .Parameter("sku", sku)
                          .Parameter("supplyCode", supplyCode)
                          .QuerySingle<ScProduct>();
        }

        /// <summary>
        /// 获取供应商产品详情，根据平台商品编号
        /// </summary>
        /// <param name="productSysNo">平台商品编号</param>
        /// <param name="supplyCode">供应链代码</param>
        /// <remarks> 2016-5-5 刘伟豪 创建</remarks>
        public override ScProduct GetScProductInfo(int productSysNo, int supplyCode)
        {
            return Context.Sql("SELECT * FROM ScProduct WHERE ProductSysNo>0 And ProductSysNo=@productSysNo and supplyCode=@supplyCode")
                          .Parameter("productSysNo", productSysNo)
                          .Parameter("supplyCode", supplyCode)
                          .QuerySingle<ScProduct>();
        }

        /// <summary>
        /// 获取供应链所有产品
        /// </summary>
        /// <param name="supplyCode">供应链代码</param>
        /// <returns></returns>
        /// <remarks> 2016-3-18  杨浩 创建</remarks>
        public override IList<ScProduct> GetScProductList(int supplyCode)
        {
            return Context.Sql("SELECT * FROM ScProduct WHERE supplyCode=@0", supplyCode)
                .QueryMany<ScProduct>();
        }
        /// <summary>
        /// 查询供应链产品
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        /// <remarks> 2013-6-25 杨浩 创建</remarks>
        /// <remarks> 2016-3-21 刘伟豪 修改</remarks>
        public override void Seach(ref Pager<CBScProduct> pager, ParaSupplyProductFilter condition)
        {
            using (var _context = Context.UseSharedConnection(true))
            {
                var sqlWhere = " 1=1 ";

                if (condition.SysNo > 0)
                    sqlWhere += " and p.SysNo=@SysNo ";

                if (condition.ProductSysNo > 0)
                    sqlWhere += " and p.ProductSysNo=@ProductSysNo ";

                if (condition.SupplyCode > 0)
                    sqlWhere += " and p.SupplyCode=@SupplyCode ";

                if (condition.HasStockIn.HasValue)
                {
                    if (condition.HasStockIn == 1)
                        sqlWhere += " and p.ProductSysNo>0";
                    else
                        sqlWhere += " and p.ProductSysNo=0";
                }

                if (condition.Status > 0)
                    sqlWhere += " and p.Status=@Status";

                if (!string.IsNullOrWhiteSpace(condition.SKU))
                    sqlWhere += " and p.SKU like @SKU ";

                if (!string.IsNullOrWhiteSpace(condition.ProductName))
                    sqlWhere += " and p.ProductName like @ProductName ";

                if (!string.IsNullOrWhiteSpace(condition.KeyWord))
                    sqlWhere += " and (p.SKU like @KeyWord or p.ProductName like @KeyWord) ";

                if (condition.StartTime != null)
                    sqlWhere += " and p.UpdateDate >= @StartTime";

                if (condition.EndTime != null)
                    sqlWhere += " and p.UpdateDate <= @EndTime";

                if (condition.CreateStartTime != null)
                    sqlWhere += " and p.CreateDate >= @CreateStartTime";

                if (condition.CreateEndTime != null)
                    sqlWhere += " and  p.CreateDate <= @CreateEndTime";

                pager.Rows = _context.Select<CBScProduct>(" p.* ")
                                     .From(@"ScProduct p ")
                                     .Where(sqlWhere)
                                     .Parameter("SysNo", condition.SysNo)
                                     .Parameter("ProductSysNo", condition.ProductSysNo)
                                     .Parameter("SupplyCode", condition.SupplyCode)
                                     .Parameter("Status", condition.Status)
                                     .Parameter("SKU", "%" + condition.SKU + "%")
                                     .Parameter("ProductName", "%" + condition.ProductName + "%")
                                     .Parameter("KeyWord", "%" + condition.KeyWord + "%")
                                     .Parameter("StartTime", condition.StartTime)
                                     .Parameter("EndTime", condition.EndTime)
                                     .Parameter("CreateStartTime", condition.CreateStartTime)
                                     .Parameter("CreateEndTime", condition.CreateEndTime)
                                     .OrderBy("p.ProductSysNo Desc, p.UpdateDate Desc, p.SysNo Desc ")
                                     .Paging(pager.CurrentPage, pager.PageSize)
                                     .QueryMany();

                pager.TotalRows = _context.Select<int>(" count(0) ")
                                          .From(@"ScProduct p ")
                                          .Where(sqlWhere)
                                          .Parameter("SysNo", condition.SysNo)
                                          .Parameter("ProductSysNo", condition.ProductSysNo)
                                          .Parameter("SupplyCode", condition.SupplyCode)
                                          .Parameter("Status", condition.Status)
                                          .Parameter("SKU", "%" + condition.SKU + "%")
                                          .Parameter("ProductName", "%" + condition.ProductName + "%")
                                          .Parameter("KeyWord", "%" + condition.KeyWord + "%")
                                          .Parameter("StartTime", condition.StartTime)
                                          .Parameter("EndTime", condition.EndTime)
                                          .Parameter("CreateStartTime", condition.CreateStartTime)
                                          .Parameter("CreateEndTime", condition.CreateEndTime)
                                          .QuerySingle();
            }
        }
        /// <summary>
        /// 更新表中的商品编号
        /// </summary>
        /// <param name="SysNo"></param>
        /// <param name="ProductSysNo"></param>
        /// <returns></returns>
        /// <remarks> 2016-4-25 王耀发 创建</remarks>
        public override bool UpdateProductSysNo(int SysNo, int ProductSysNo)
        {
            int result = Context.Sql("update ScProduct set ProductSysNo = @0,UpdateDate = @1 where SysNo = @2 ", ProductSysNo,
                                     DateTime.Now, SysNo).Execute();
            return result > 0;
        }


        /// <summary>
        /// 同步供应链商品到库存
        /// 王耀发 2016-5-5 创建
        /// </summary>
        /// <param name="Supply">供应链类型</param>
        /// <param name="PdProductSysNo">商品编号</param>
        /// <param name="StockQuantity">库存数量</param>
        /// <param name="CreatedBy">创建用户系统编号</param>
        /// <returns></returns>
        public override int ProCreateSupplyStock(int Supply,int PdProductSysNo,decimal StockQuantity, int CreatedBy)
        {
            string Sql = string.Format("pro_CreateSupplyStock {0},{1},{2},{3}", Supply, PdProductSysNo, StockQuantity, CreatedBy);
            int rowsAffected = Context.Sql(Sql).Execute();
            return rowsAffected;
        }

        /// <summary>
        /// 根据商品ID更新商品详情
        /// </summary>
        /// <param name="ProductSysNo"></param>
        /// <param name="ProductDesc"></param>
        /// <returns></returns>
        public override bool UpdatePdBySysNo(int ProductSysNo, string ProductDesc)
        {
            int result = Context.Sql("update PdProduct set ProductDesc = @0 where SysNo = @1 ", ProductDesc, ProductSysNo).Execute();
            return result > 0;
        }
    }
}