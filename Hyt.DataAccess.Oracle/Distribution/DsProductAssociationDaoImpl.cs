using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Product;
using Hyt.Model;
using Hyt.Model.LogisApp;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Util;
using Hyt.Model.Parameter;
using Hyt.DataAccess.Base;
using Hyt.DataAccess.Distribution;
namespace Hyt.DataAccess.Oracle.Distribution
{
    /// <summary>
    /// 商品关联关系对应表
    /// </summary>
    /// <remarks>2013-09-13  朱成果 创建</remarks>
    public class DsProductAssociationDaoImpl : IDsProductAssociationDao
    {

        #region 数据记录增，删，改，查
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2013-09-13  朱成果 创建</remarks>
        public override int Insert(DsProductAssociation entity)
        {
            entity.SysNo = Context.Insert("DsProductAssociation", entity)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
            return entity.SysNo;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        /// <remarks>2013-09-13  朱成果 创建</remarks>
        public override void Update(DsProductAssociation entity)
        {

            Context.Update("DsProductAssociation", entity)
                   .AutoMap(o => o.SysNo)
                   .Where("SysNo", entity.SysNo)
                   .Execute();
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2013-09-13  朱成果 创建</remarks>
        public override DsProductAssociation GetEntity(int sysNo)
        {

            return Context.Sql("select * from DsProductAssociation where SysNo=@SysNo")
                   .Parameter("SysNo", sysNo)
              .QuerySingle<DsProductAssociation>();
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns></returns>
        /// <remarks>2013-09-13  朱成果 创建</remarks>
        public override void Delete(int sysNo)
        {
            Context.Sql("Delete from DsProductAssociation where SysNo=@SysNo")
                 .Parameter("SysNo", sysNo)
            .Execute();
        }

        /// <summary>
        /// 获取分销产品和商城产品的对应关系
        /// </summary>
        /// <param name="dealerMallSysNo">分销商商城系统编号</param>
        /// <param name="mallProductId">商城商品编码</param>
        /// <returns>分销产品和商城产品的对应关系</returns>
        /// <remarks>2013-09-13  朱成果 创建</remarks>
        public override DsProductAssociation GetEntity(int dealerMallSysNo, string mallProductId)
        {

            return Context.Sql("select * from DsProductAssociation where DealerMallSysNo=@DealerMallSysNo and MallProductId=@MallProductId")
                   .Parameter("DealerMallSysNo", dealerMallSysNo)
                   .Parameter("MallProductId", mallProductId)
              .QuerySingle<DsProductAssociation>();
        }

        /// <summary>
        /// 获取关联的商城产品详情
        /// </summary>
        /// <param name="dealerMallSysNo">分销商商城系统编号</param>
        /// <param name="mallProductId">商城商品编码</param>
        /// <returns>获取关联的商城产品详情</returns>
        /// <remarks>2013-09-13  朱成果 创建</remarks>
        public override CBDsProductAssociation GetHytProduct(int dealerMallSysNo, string mallProductId)
        {
            return Context.Sql(@"
            select  tb0.*,tb1.productname as HytProductName,tb1.erpcode as HytProductErpCode,tb3.price as SpecialPrice,tb5.price as PdPrice
            from DsProductAssociation tb0
            inner join PdProduct      tb1
            on tb0.hytproductsysno=tb1.sysno
            inner join DsDealerMall   tb2
            on tb2.sysno=tb0.dealermallsysno
            inner join DsDealer tb4
            on tb4.sysno=tb2.dealersysno
            left outer join DsSpecialPrice tb3
            on tb3.dealersysno=tb2.dealersysno and tb3.productsysno=tb1.sysno
            inner join PdPrice tb5
            on tb5.productsysno=tb1.sysno and tb5.pricesource=@pricesource and tb5.sourcesysno=tb4.levelsysno
            where tb0.dealermallsysno=@DealerMallSysNo and tb0.mallproductid=@mallproductid")
            .Parameter("pricesource", (int)Hyt.Model.WorkflowStatus.ProductStatus.产品价格来源.分销商等级价)
            .Parameter("DealerMallSysNo", dealerMallSysNo)
            .Parameter("mallproductid", mallProductId)
            .QuerySingle<CBDsProductAssociation>();
        }

        #region 获取分销商商品详细信息列表
        /// <summary>
        /// 获取商品详细信息列表
        /// </summary>
        /// <param name="pager">商品详细信息查询列表</param>
        /// <param name="dealerMallSysNo">分销商商城系统编号</param>
        /// <param name="condition">动态条件，CBPdProductDetail里不包含的条件</param>
        /// <returns></returns>
        /// <remarks>2015-12-10 王耀发 创建</remarks>
        public override void GetDealerMallProductList(ref Pager<CBPdProductDetail> pager, int dealerMallSysNo, ParaProductFilter condition)
        {

            using (var _context = Context.UseSharedConnection(true))
            {
                pager.Rows = _context.Select<CBPdProductDetail>(" pa.SysNo as PAssociationSysNo,p.SysNo,p.ErpCode,p.EasName,p.ProductType,pa.Status, c.categoryname as ProductCategoryName, c.sysno as ProductCategorySysno, price.Price as BasicPrice, price1.Price as SalesPrice")
                              .From(@"DsProductAssociation pa left join 
                               PdProduct p on pa.HytProductSysNo = p.SysNo
                                    inner join (select productsysno from PdCategoryAssociation where  (@ProductCategorySysno = 0 or categorysysno=@ProductCategorySysno) group by productsysno ) ca1 on ca1.productsysno = p.sysno 
                                    left join (select productsysno,categorysysno from PdCategoryAssociation where IsMaster=@IsMaster) ca on ca.productsysno = p.sysno 
                                    left join  PdCategory c on ca.categorysysno = c.sysno
                                    left join  (select * from PdPrice where PriceSource =@PriceSource and (@SourceSysno =0 or sourcesysno=@sourcesysno)) price  on price.productsysno = p.sysno
                                    left join  (select * from PdPrice where PriceSource =@PriceSource1 and (@SourceSysno =0 or sourcesysno=@sourcesysno)) price1  on price1.productsysno = p.sysno")
                               .Where(@"(pa.DealerMallSysNo = @DealerMallSysNo) 
                                        and (@Status = -1 or pa.Status=@Status)
                                        and (@name is null or (p.EasName like @name1 or p.ErpCode = @name))
                                        and (@Sysno = 0 or p.Sysno=@Sysno)
                                        and (@ErpCode is null or p.ErpCode=@ErpCode)
                                        and (@StartTime is null or p.LastUpdateDate >= @StartTime)
                                        and (@EndTime is null or p.LastUpdateDate <= @EndTime)
                                        and (@CreateStartTime is null or p.CreatedDate >= @CreateStartTime)
                                        and (@CreateEndTime is null or p.CreatedDate <= @CreateEndTime) 
                                        and (@CanFrontEndOrder = -1 or CanFrontEndOrder = @CanFrontEndOrder)  
                                        ")
                              .Parameter("ProductCategorySysno", condition.ProductCategorySysno)
                              .Parameter("IsMaster", (int)ProductStatus.是否是主分类.是)
                              .Parameter("PriceSource", (int)ProductStatus.产品价格来源.基础价格)
                              .Parameter("PriceSource1", (int)ProductStatus.产品价格来源.会员等级价)
                              .Parameter("SourceSysno", condition.PriceSourceSysNo)
                              .Parameter("DealerMallSysNo", dealerMallSysNo)
                              .Parameter("Status", condition.Status)
                              .Parameter("name", condition.ProductName)
                              .Parameter("name1", "%" + condition.ProductName + "%")
                              .Parameter("Sysno", condition.SysNo)
                              .Parameter("ErpCode", condition.ErpCode)
                              .Parameter("StartTime", condition.StartTime)
                              .Parameter("EndTime", (condition.EndTime == null) ? condition.EndTime : ((DateTime)condition.EndTime).AddDays(1))
                              .Parameter("CreateStartTime", condition.CreateStartTime)
                              .Parameter("CreateEndTime", (condition.CreateEndTime == null) ? condition.CreateEndTime : ((DateTime)condition.CreateEndTime).AddDays(1))
                              .Parameter("CanFrontEndOrder", condition.CanFrontEndOrder)
                              .OrderBy("pa.Status desc, pa.sysno desc ")
                              .Paging(pager.CurrentPage, pager.PageSize)
                              .QueryMany();

                pager.TotalRows = _context.Select<int>("count(1)")
                              .From(@"DsProductAssociation pa left join 
                               PdProduct p on pa.HytProductSysNo = p.SysNo
                                    inner join (select productsysno from PdCategoryAssociation where  (@ProductCategorySysno = 0 or categorysysno=@ProductCategorySysno) group by productsysno ) ca1 on ca1.productsysno = p.sysno 
                                    left join (select productsysno,categorysysno from PdCategoryAssociation where IsMaster=@IsMaster) ca on ca.productsysno = p.sysno 
                                    left join  PdCategory c on ca.categorysysno = c.sysno
                                    left join  (select * from PdPrice where PriceSource =@PriceSource and (@SourceSysno =0 or sourcesysno=@sourcesysno)) price  on price.productsysno = p.sysno
                                    left join  (select * from PdPrice where PriceSource =@PriceSource1 and (@SourceSysno =0 or sourcesysno=@sourcesysno)) price1  on price1.productsysno = p.sysno")
                               .Where(@"(pa.DealerMallSysNo = @DealerMallSysNo) 
                                        and (@Status = -1 or pa.Status=@Status)
                                        and (@name is null or (p.EasName like @name1 or p.ErpCode = @name))
                                        and (@Sysno = 0 or p.Sysno=@Sysno)
                                        and (@ErpCode is null or p.ErpCode=@ErpCode)
                                        and (@StartTime is null or p.LastUpdateDate >= @StartTime)
                                        and (@EndTime is null or p.LastUpdateDate <= @EndTime)
                                        and (@CreateStartTime is null or p.CreatedDate >= @CreateStartTime)
                                        and (@CreateEndTime is null or p.CreatedDate <= @CreateEndTime) 
                                        and (@CanFrontEndOrder = -1 or CanFrontEndOrder = @CanFrontEndOrder)  
                                        ")
                              .Parameter("ProductCategorySysno", condition.ProductCategorySysno)
                              .Parameter("IsMaster", (int)ProductStatus.是否是主分类.是)
                              .Parameter("PriceSource", (int)ProductStatus.产品价格来源.基础价格)
                              .Parameter("PriceSource1", (int)ProductStatus.产品价格来源.会员等级价)
                              .Parameter("SourceSysno", condition.PriceSourceSysNo)
                              .Parameter("DealerMallSysNo", dealerMallSysNo)
                              .Parameter("Status", condition.Status)
                              .Parameter("name", condition.ProductName)
                              .Parameter("name1", "%" + condition.ProductName + "%")
                              .Parameter("Sysno", condition.SysNo)
                              .Parameter("ErpCode", condition.ErpCode)
                              .Parameter("StartTime", condition.StartTime)
                              .Parameter("EndTime", (condition.EndTime == null) ? condition.EndTime : ((DateTime)condition.EndTime).AddDays(1))
                              .Parameter("CreateStartTime", condition.CreateStartTime)
                              .Parameter("CreateEndTime", (condition.CreateEndTime == null) ? condition.CreateEndTime : ((DateTime)condition.CreateEndTime).AddDays(1))
                              .Parameter("CanFrontEndOrder", condition.CanFrontEndOrder)
                              .QuerySingle();
            }
        }

        /// <summary>
        /// 更新商品状态值
        /// </summary>
        /// <param name="SysNo">商品关系编号</param>
        /// <param name="Status">状态值</param>
        /// <returns></returns>
        /// <remarks>2015-12-10 王耀发  创建</remarks>
        public override void UpdateDealerMallProductStatus(int SysNo, int Status)
        {
            Context.Sql("update DsProductAssociation set Status=@Status where SysNo=@SysNo")
                   .Parameter("Status", Status)
                   .Parameter("SysNo", SysNo).Execute();
        }
        #endregion
        #endregion
    }
}
