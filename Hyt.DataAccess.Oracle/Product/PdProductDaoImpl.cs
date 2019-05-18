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

namespace Hyt.DataAccess.Oracle.Product
{
    /// <summary>
    /// 商品基础信息
    /// </summary>
    /// <remarks>2013-06-25 黄波 创建</remarks>
    public class PdProductDaoImpl : IPdProductDao
    {

        /// <summary>
        /// 通过商品条形码获取商品系统编号
        /// </summary>
        /// <param name="barCode">条形码</param>
        /// <returns>商品系统编号</returns>
        /// <remarks>2016-05-25 杨浩 创建</remarks>
        public override int GetProductSysNoByBarCode(string barCode)
        {
            return Context.Sql("select sysno from pdproduct where barcode = @barCode")
                              .Parameter("barCode", barCode)
                              .QuerySingle<int>();
        }

        #region 创建商品信息
        /// <summary>
        /// 创建商品信息
        /// </summary>
        /// <param name="model">商品信息</param>
        /// <returns>是否创建成功</returns>
        /// <remarks>
        /// 2013-06-25 杨浩 创建
        /// 2016-3-12 杨浩 修复DealerPriceValue复制时Entity为null报错
        /// </remarks>
        public override int Create(PdProduct model)
        {
            var sysNo = -1;
            using (var context = Context.UseSharedConnection(true))
            {
                sysNo = context.Insert<PdProduct>("PdProduct", model)
                                       .AutoMap(o => o.SysNo, o => o.Stamp)
                                       .ExecuteReturnLastId<int>("SysNo");
                if (sysNo > 0)
                {
                    context.Insert<PdProductStatistics>("PdProductStatistics", new PdProductStatistics
                    {
                        AverageScore = 5,
                        Comments = 0,
                        Favorites = 0,
                        Liking = 0,
                        ProductSysNo = sysNo,
                        Question = 0,
                        Sales = 0,
                        Shares = 0,
                        TotalScore = 0,
                    })
                        .AutoMap(o => o.SysNo)
                        .Execute();

                    //计算分销商商品利润 王耀发 2016-3-7 创建
                    //PdPrice Entity = IPdPriceDao.Instance.GetSalesPrice(sysNo, (int)Hyt.Model.WorkflowStatus.ProductStatus.产品价格来源.会员等级价);

                    //Decimal DealerPriceValue = 0;

                    //if (Entity != null && model != null)
                    //    DealerPriceValue = (Entity.Price) * (model.PriceRate / 100);

                    //Context.Update("PdProduct")
                    //.Column("DealerPriceValue", DealerPriceValue)
                    //.Where("SysNo", sysNo)
                    //.Execute();
                }
            }
            return sysNo;
        }

        /// <summary>
        /// 同步创建商品信息到B2B平台
        /// </summary>
        /// <param name="model">商品信息</param>
        /// <returns>是否创建成功</returns>
        /// <remarks>
        /// 2017-10-11 罗勤瑶 创建
        /// </remarks>
        public override int CreateToB2B(PdProduct model)
        {
            var sysNo = -1;
            using (var context = ContextB2B.UseSharedConnection(true))
            {
                sysNo = ContextB2B.Insert<PdProduct>("PdProduct", model)
                                       .AutoMap(o => o.SysNo, o => o.Stamp)
                                       .ExecuteReturnLastId<int>("SysNo");
                if (sysNo > 0)
                {
                    ContextB2B.Insert<PdProductStatistics>("PdProductStatistics", new PdProductStatistics
                    {
                        AverageScore = 5,
                        Comments = 0,
                        Favorites = 0,
                        Liking = 0,
                        ProductSysNo = sysNo,
                        Question = 0,
                        Sales = 0,
                        Shares = 0,
                        TotalScore = 0,
                    })
                        .AutoMap(o => o.SysNo)
                        .Execute();

                    //计算分销商商品利润 王耀发 2016-3-7 创建
                    //PdPrice Entity = IPdPriceDao.Instance.GetSalesPrice(sysNo, (int)Hyt.Model.WorkflowStatus.ProductStatus.产品价格来源.会员等级价);

                    //Decimal DealerPriceValue = 0;

                    //if (Entity != null && model != null)
                    //    DealerPriceValue = (Entity.Price) * (model.PriceRate / 100);

                    //Context.Update("PdProduct")
                    //.Column("DealerPriceValue", DealerPriceValue)
                    //.Where("SysNo", sysNo)
                    //.Execute();
                }
            }
            return sysNo;
        }
        #endregion

        #region 更新商品信息
        /// <summary>
        /// 更新商品信息
        /// </summary>
        /// <param name="model">商品信息</param>
        /// <returns>是否创建成功</returns>
        /// <remarks>2013-06-25 黄波 创建</remarks>
        public override bool Update(PdProduct model)
        {
            var returnValue = Context.Update<PdProduct>("PdProduct", model)
                .AutoMap(o => o.SysNo)
                .Where("SysNo", model.SysNo)
                .Execute();

            //计算分销商商品利润 王耀发 2016-3-7 创建
            //PdPrice Entity = IPdPriceDao.Instance.GetSalesPrice(model.SysNo, (int)Hyt.Model.WorkflowStatus.ProductStatus.产品价格来源.会员等级价);

            //Decimal DealerPriceValue = 0;

            //if (Entity != null && model != null)
            //    DealerPriceValue = Entity.Price * (model.PriceRate / 100);

            //Context.Update("PdProduct")
            //.Column("DealerPriceValue", DealerPriceValue)
            //.Where("SysNo", model.SysNo)
            //.Execute();

            return returnValue > 0;
        }
        #endregion

        #region 根据商品系统编号获取商品信息
        /// <summary>
        /// 根据商品系统编号获取商品信息
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <returns>商品信息</returns>
        /// <remarks>2013-06-25 黄波 创建</remarks>
        /// <remarks>2013-08-06 邵斌 实现评分和评论次数</remarks>
        public override CBPdProduct GetProduct(int productSysNo)
        {
            CBPdProduct returnValue = null;

            var baseProduct = Context.Sql(@"
                    select 
                        p.*,pb.name as BrandName
                    from 
                        PdProduct p
                        left join PdBrand pb on pb.sysno = p.brandsysno
                    where 
                        p.SysNo=@0
                ", productSysNo)
                .QuerySingle<CBPdProduct>();

            if (baseProduct != null)
            {
                //using (var context = Context.UseSharedConnection(true))
                //{

                returnValue = new CBPdProduct();

                returnValue = (CBPdProduct)baseProduct;

                returnValue.PdProductAttribute = new Lazy<IList<PdProductAttribute>>(() =>
                {
                    return Context.Sql("select * from PdProductAttribute where ProductSysNo=@0", productSysNo)
                        .QueryMany<PdProductAttribute>();
                });

                returnValue.PdCategory = new Lazy<IList<PdCategory>>(() =>
                {
                    IList<PdCategory> categories = Context.Sql(@"select c.*,ca.ismaster from 
                                        pdcategory c 
                                        inner join pdcategoryassociation ca on c.sysno = ca.categorysysno
                                        where 
                                        ca.productsysno=@0
                                        order by ca.ismaster desc,c.DisplayOrder asc
                                        ", productSysNo)
                        .QueryMany<PdCategory>();
                    if (categories.Count > 0)
                        categories[0].IsMaster = true;
                    return categories;
                });

                returnValue.PdPrice = new Lazy<IList<PdPrice>>(() =>
                {
                    return Context.Sql("select * from PdPrice where ProductSysNo=@0 order by SourceSysNo", productSysNo)
                        .QueryMany<PdPrice>();
                });

                returnValue.PdProductAssociation = new Lazy<IList<PdProductAssociation>>(() =>
                {
                    //读取关联关系码
                    string relationCode =
                        Context.Sql("select relationcode from PdProductAssociation where productSysNo = @0",
                                    productSysNo).QuerySingle<string>();

                    //如果关联关系码为空则返回空
                    if (string.IsNullOrWhiteSpace(relationCode))
                        return new List<PdProductAssociation>();

                    //返回关联商品
                    return Context.Sql(@"select * from PdProductAssociation where relationcode=@0", relationCode)
                    .QueryMany<PdProductAssociation>();
                });

                returnValue.PdBrand = new Lazy<PdBrand>(() =>
                {
                    return Context.Sql("select * from PdBrand where SysNo=@0", baseProduct.BrandSysNo)
                        .QuerySingle<PdBrand>();
                });

                returnValue.PdCategoryAssociation = new Lazy<IList<PdCategoryAssociation>>(() =>
                {
                    return Context.Sql("select * from PdCategoryAssociation where productsysno=@0", productSysNo).QueryMany<PdCategoryAssociation>();
                });

                returnValue.PdProductImage = new Lazy<IList<PdProductImage>>(() =>
                {
                    return Context.Sql("select * from PdProductImage where productsysno=@0  order by DisplayOrder", productSysNo).QueryMany<PdProductImage>();
                });

                returnValue.PdProductCollocationRelation = new Lazy<IList<PdProductCollocation>>(() =>
                {
                    return Context.Sql("select * from PdProductCollocation where productsysno=@0", productSysNo).QueryMany<PdProductCollocation>();
                });

                returnValue.ProductCommentScore = new Lazy<decimal>(() =>
                {

                    //商品评分是0-5分制，所以score只查找0-5分的评分,评分总和/评价次数=评分
                    if (returnValue.ProductCommentScoreTotal.Value == 0)
                        return 0;
                    return Math.Round((returnValue.ProductCommentScoreTotal.Value / returnValue.CommentTimesCount.Value) * 10, 1) / 10;
                });

                returnValue.ProductCommentScoreTotal = new Lazy<decimal>(() =>
                {
                    return
                        Context.Sql(
                            "select sum(Score) ScoreTotal from FeProductComment where productsysno=@0 and Score<6 and CommentStatus = @1", productSysNo, (int)ForeStatus.商品评论状态.已审).QuerySingle<decimal>();
                });

                returnValue.CommentTimesCount = new Lazy<int>(() =>
                {
                    //商品评分是0-5分制，所以子统计了0-5分内的评价为有效评价
                    return Context.Sql(
                           "select count(sysno) as CountCommentTimes from FeProductComment where productsysno=@0 and Score<6 and CommentStatus = @1", productSysNo, (int)ForeStatus.商品评论状态.已审).QuerySingle<int>();
                });
                //}
            }

            return returnValue;
        }

        /// <summary>
        /// 获取所有商品信息
        /// </summary>
        /// <returns>商品信息集合</returns>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        public override IList<PdProduct> GetAllPdProduct()
        {
            const string strSql = @"select * from PdProduct where Status < 2 ";
            var entity = Context.Sql(strSql)
                                .QueryMany<PdProduct>();
            return entity;
        }

        /// <summary>
        /// 新增商品信息
        /// </summary>
        /// <param name="models">商品信息列表</param>
        /// <returns>空</returns>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        public override void CreatePdProduct(List<PdProductList> models)
        {
            foreach (PdProductList model in models)
            {
                //var sql = @"insert into PdProduct(BrandSysNo,ErpCode,EasName,Volume,GrosWeight,CreatedBy,CreatedDate,LastUpdateBy,LastUpdateDate) select @0,@1,@2,@3,@4,@5,@6,@7,@8";
                //Context.Sql(sql, model.BrandSysNo, model.ErpCode, model.EasName, model.Volume, model.GrosWeight, model.CreatedBy, model.CreatedDate, model.LastUpdateBy, model.LastUpdateDate).Execute();

                var PdData = new PdProduct();
                PdData.BrandSysNo = model.BrandSysNo;
                PdData.ErpCode = model.ErpCode;
                PdData.ProductName = model.ProductName;
                PdData.EasName = model.EasName;
                PdData.ProductType = model.ProductType;
                PdData.OriginSysNo = model.OriginSysNo;
                PdData.Barcode = model.Barcode;
                PdData.GrosWeight = model.GrosWeight;
                PdData.Tax = decimal.Parse(model.Tax);
                PdData.PriceRate = model.PriceRate;
                PdData.PriceValue = model.PriceValue;
                PdData.TradePrice = model.TradePrice;
                PdData.CostPrice = model.CostPrice;
                PdData.Status = model.Status;
                PdData.CanFrontEndOrder = model.CanFrontEndOrder;
                PdData.IsFrontDisplay = model.IsFrontDisplay;
                PdData.AgentSysNo = model.AgentSysNo;
                PdData.CreatedBy = model.CreatedBy;
                PdData.CreatedDate = model.CreatedDate;
                PdData.LastUpdateBy = model.LastUpdateBy;
                PdData.LastUpdateDate = model.LastUpdateDate;
                PdData.CanFrontEndOrder = 1;
                PdData.DealerSysNo = model.DealerSysNo;
                PdData.AgentSysNo = model.AgentSysNo;
                PdData.ProductDesc = model.ProductDesc;
                PdData.ProductShortTitle = model.ProductShortTitle;

                int productSysNo = Context.Insert<PdProduct>("PdProduct", PdData)
                                       .AutoMap(o => o.SysNo, o => o.Stamp)
                                       .ExecuteReturnLastId<int>("SysNo");
                model.PdPrice.ProductSysNo = productSysNo;
                model.PdSalePrice.ProductSysNo = productSysNo;
                model.PdStoreSalePrice.ProductSysNo = productSysNo;

                //判断类目是否存在
                var categorys = Context.Sql("select * from PdCategory where CategoryName = @CategoryName and Status = @Status ")          
                    .Parameter("CategoryName", model.PdCategorySql.CategoryName)            
                    .Parameter("Status", model.PdCategorySql.Status)
                    .QueryMany<PdCategorySql>();

                if (categorys != null && categorys.Count > 0)
                {
                    model.PdCategoryAssociation.CategorySysNo = categorys.First().SysNo;
                    model.PdCategoryAssociation.IsMaster = 1;
                    model.PdCategoryAssociation.ProductSysNo = productSysNo;
                    model.PdCategoryAssociation.LastUpdateDate =(DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    model.PdCategoryAssociation.CreatedDate = DateTime.Now;
                    int PdCategoryAssociationSysNo = Context.Insert<PdCategoryAssociation>("PdCategoryAssociation", model.PdCategoryAssociation)
                                           .AutoMap(o => o.SysNo)
                                           .ExecuteReturnLastId<int>("SysNo");
                }

                
                //else
                //{
                //    //int CategorySysNo = Context.Insert<PdCategorySql>("PdCategory", model.PdCategorySql)
                //    //                       .AutoMap(o => o.SysNo)
                //    //                       .ExecuteReturnLastId<int>("SysNo");
                //    //Context.Update("PdCategory").Column("SysNos", "," + CategorySysNo.ToString() + ",").Where("SysNo", CategorySysNo).Execute();

                //    //model.PdCategoryAssociation.CategorySysNo = CategorySysNo;
                //}
              
               

                //判断商品的基础价格是否存在
                PdPrice entity = Context.Select<PdPrice>("*")
                    .From("PdPrice")
                    .Where("ProductSysNo= @ProductSysNo and PriceSource = @PriceSource and SourceSysNo = @SourceSysNo and Status = @Status")
                    .Parameter("ProductSysNo", model.PdPrice.ProductSysNo)
                    .Parameter("PriceSource", model.PdPrice.PriceSource)
                    .Parameter("SourceSysNo", model.PdPrice.SourceSysNo)
                    .Parameter("Status", model.PdPrice.Status)
                    .QuerySingle();
                if (entity != null && entity.SysNo > 0)
                {
                    Context.Update("PdPrice")
                    .Column("Price", model.PdPrice.Price)
                    .Where("ProductSysNo", model.PdPrice.ProductSysNo)
                    .Where("PriceSource", model.PdPrice.PriceSource)
                    .Where("SourceSysNo", model.PdPrice.SourceSysNo)
                    .Where("Status", model.PdPrice.Status)
                    .Execute();
                }
                else
                {
                    int PdPriceSysNo = Context.Insert<PdPrice>("PdPrice", model.PdPrice)
                                           .AutoMap(o => o.SysNo)
                                           .ExecuteReturnLastId<int>("SysNo");
                }

                //判断商品的会员价格是否存在
                PdPrice saleentity = Context.Select<PdPrice>("*")
                    .From("PdPrice")
                    .Where("ProductSysNo= @ProductSysNo and PriceSource = @PriceSource and SourceSysNo = @SourceSysNo and Status = @Status")
                    .Parameter("ProductSysNo", model.PdSalePrice.ProductSysNo)
                    .Parameter("PriceSource", model.PdSalePrice.PriceSource)
                    .Parameter("SourceSysNo", model.PdSalePrice.SourceSysNo)
                    .Parameter("Status", model.PdSalePrice.Status)
                    .QuerySingle();
                if (saleentity != null && saleentity.SysNo > 0)
                {
                    Context.Update("PdPrice")
                    .Column("Price", model.PdSalePrice.Price)
                    .Where("ProductSysNo", model.PdSalePrice.ProductSysNo)
                    .Where("PriceSource", model.PdSalePrice.PriceSource)
                    .Where("SourceSysNo", model.PdSalePrice.SourceSysNo)
                    .Where("Status", model.PdSalePrice.Status)
                    .Execute();
                }
                else
                {
                    int PdPriceSysNo = Context.Insert<PdPrice>("PdPrice", model.PdSalePrice)
                                           .AutoMap(o => o.SysNo)
                                           .ExecuteReturnLastId<int>("SysNo");
                }


                if(model.PdVIPPrice!=null)
                {
                    model.PdVIPPrice.ProductSysNo = productSysNo;
                    //判断商品的会员价格是否存在
                    PdPrice VIPentity = Context.Select<PdPrice>("*")
                        .From("PdPrice")
                        .Where("ProductSysNo= @ProductSysNo and PriceSource = @PriceSource and SourceSysNo = @SourceSysNo and Status = @Status")
                        .Parameter("ProductSysNo", model.PdVIPPrice.ProductSysNo)
                        .Parameter("PriceSource", model.PdVIPPrice.PriceSource)
                        .Parameter("SourceSysNo", model.PdVIPPrice.SourceSysNo)
                        .Parameter("Status", model.PdVIPPrice.Status)
                        .QuerySingle();
                    if (VIPentity != null && VIPentity.SysNo > 0)
                    {
                        Context.Update("PdPrice")
                        .Column("Price", model.PdVIPPrice.Price)
                        .Where("ProductSysNo", model.PdVIPPrice.ProductSysNo)
                        .Where("PriceSource", model.PdVIPPrice.PriceSource)
                        .Where("SourceSysNo", model.PdVIPPrice.SourceSysNo)
                        .Where("Status", model.PdVIPPrice.Status)
                        .Execute();
                    }
                    else
                    {
                        int PdPriceSysNo = Context.Insert<PdPrice>("PdPrice", model.PdVIPPrice)
                                               .AutoMap(o => o.SysNo)
                                               .ExecuteReturnLastId<int>("SysNo");
                    }
                }

                if (model.PdDiamondPrice != null)
                {
                    model.PdDiamondPrice.ProductSysNo = productSysNo;
                    //判断商品的钻石会员价格是否存在
                    PdPrice Diamondentity = Context.Select<PdPrice>("*")
                        .From("PdPrice")
                        .Where("ProductSysNo= @ProductSysNo and PriceSource = @PriceSource and SourceSysNo = @SourceSysNo and Status = @Status")
                        .Parameter("ProductSysNo", model.PdDiamondPrice.ProductSysNo)
                        .Parameter("PriceSource", model.PdDiamondPrice.PriceSource)
                        .Parameter("SourceSysNo", model.PdDiamondPrice.SourceSysNo)
                        .Parameter("Status", model.PdDiamondPrice.Status)
                        .QuerySingle();
                    if (Diamondentity != null && Diamondentity.SysNo > 0)
                    {
                        Context.Update("PdPrice")
                        .Column("Price", model.PdDiamondPrice.Price)
                        .Where("ProductSysNo", model.PdDiamondPrice.ProductSysNo)
                        .Where("PriceSource", model.PdDiamondPrice.PriceSource)
                        .Where("SourceSysNo", model.PdDiamondPrice.SourceSysNo)
                        .Where("Status", model.PdDiamondPrice.Status)
                        .Execute();
                    }
                    else
                    {
                        int PdPriceSysNo = Context.Insert<PdPrice>("PdPrice", model.PdDiamondPrice)
                                               .AutoMap(o => o.SysNo)
                                               .ExecuteReturnLastId<int>("SysNo");
                    }
                }

                if (model.SaleUserPrice != null)
                {
                    model.SaleUserPrice.ProductSysNo = productSysNo;
                    //判断商品的钻石会员价格是否存在
                    PdPrice Diamondentity = Context.Select<PdPrice>("*")
                        .From("PdPrice")
                        .Where("ProductSysNo= @ProductSysNo and PriceSource = @PriceSource and SourceSysNo = @SourceSysNo and Status = @Status")
                        .Parameter("ProductSysNo", model.SaleUserPrice.ProductSysNo)
                        .Parameter("PriceSource", model.SaleUserPrice.PriceSource)
                        .Parameter("SourceSysNo", model.SaleUserPrice.SourceSysNo)
                        .Parameter("Status", model.SaleUserPrice.Status)
                        .QuerySingle();
                    if (Diamondentity != null && Diamondentity.SysNo > 0)
                    {
                        Context.Update("PdPrice")
                        .Column("Price", model.SaleUserPrice.Price)
                        .Where("ProductSysNo", model.SaleUserPrice.ProductSysNo)
                        .Where("PriceSource", model.SaleUserPrice.PriceSource)
                        .Where("SourceSysNo", model.SaleUserPrice.SourceSysNo)
                        .Where("Status", model.SaleUserPrice.Status)
                        .Execute();
                    }
                    else
                    {
                        int PdPriceSysNo = Context.Insert<PdPrice>("PdPrice", model.SaleUserPrice)
                                               .AutoMap(o => o.SysNo)
                                               .ExecuteReturnLastId<int>("SysNo");
                    }
                }

                //判断商品的门店销售价是否存在
                PdPrice ssaleentity = Context.Select<PdPrice>("*")
                    .From("PdPrice")
                    .Where("ProductSysNo= @ProductSysNo and PriceSource = @PriceSource and SourceSysNo = @SourceSysNo and Status = @Status")
                    .Parameter("ProductSysNo", model.PdStoreSalePrice.ProductSysNo)
                    .Parameter("PriceSource", model.PdStoreSalePrice.PriceSource)
                    .Parameter("SourceSysNo", model.PdStoreSalePrice.SourceSysNo)
                    .Parameter("Status", model.PdStoreSalePrice.Status)
                    .QuerySingle();
                if (ssaleentity != null && ssaleentity.SysNo > 0)
                {
                    Context.Update("PdPrice")
                    .Column("Price", model.PdStoreSalePrice.Price)
                    .Where("ProductSysNo", model.PdStoreSalePrice.ProductSysNo)
                    .Where("PriceSource", model.PdStoreSalePrice.PriceSource)
                    .Where("SourceSysNo", model.PdStoreSalePrice.SourceSysNo)
                    .Where("Status", model.PdStoreSalePrice.Status)
                    .Execute();
                }
                else
                {
                    int PdPriceSysNo = Context.Insert<PdPrice>("PdPrice", model.PdStoreSalePrice)
                                           .AutoMap(o => o.SysNo)
                                           .ExecuteReturnLastId<int>("SysNo");
                }
                if (model.Sales > 0)
                {
                    PdProductStatistics PdProductSale = PdProductStatisticsDaoImpl.Instance.Get(productSysNo);
                    if (PdProductSale == null)
                    {
                        PdProductSale = new PdProductStatistics();
                        PdProductSale.ProductSysNo = productSysNo;
                        PdProductSale.Sales = model.Sales;
                        PdProductSale.AverageScore = 5;
                        PdProductStatisticsDaoImpl.Instance.Create(PdProductSale);
                    }
                    else
                    {
                        PdProductSale.Sales = model.Sales;
                        PdProductStatisticsDaoImpl.Instance.Update(PdProductSale);
                    }
                }
                
            }
        }

        /// <summary>
        /// 更新EXCEL中导入的商品信息
        /// </summary>
        /// <param name="models">商品信息列表</param>
        /// <param name="productList">系统已有产品列表</param>
        /// <returns></returns>
        /// <remarks>2016-09-23 杨浩 创建</remarks>
        public override void UpdateExcelProduct(List<PdProductList> models, IList<PdProduct> productList)
        {
            foreach (PdProductList model in models)
            {
                var pdata = productList.Where(x => x.ErpCode == model.ErpCode).FirstOrDefault();

                int productSysNo = pdata.SysNo;

                Context.Update("PdProduct")
                .Column("BrandSysNo", model.BrandSysNo)
                .Column("ProductName", model.ProductName)
                .Column("EasName", model.EasName)
                .Column("ProductType", model.ProductType)
                .Column("OriginSysNo", model.OriginSysNo)
                .Column("Barcode", model.Barcode)
                .Column("GrosWeight", model.GrosWeight)
                .Column("Tax", model.Tax)
                .Column("PriceRate", model.PriceRate)
                .Column("PriceValue", model.PriceValue)
                .Column("TradePrice", model.TradePrice)
                .Column("LastUpdateBy", model.LastUpdateBy)
                .Column("LastUpdateDate", model.LastUpdateDate)
                .Where("SysNo", productSysNo)
                .Execute();

                model.PdPrice.ProductSysNo = productSysNo;
                model.PdSalePrice.ProductSysNo = productSysNo;
                model.PdStoreSalePrice.ProductSysNo = productSysNo;

                //判断类目是否存在
                var categorys = Context.Sql(" select * from PdCategory where CategoryName = @CategoryName and IsOnline = @IsOnline and Status = @Status  ")
                    .Parameter("CategoryName", model.PdCategorySql.CategoryName)
                    .Parameter("IsOnline", model.PdCategorySql.IsOnline)
                    .Parameter("Status", model.PdCategorySql.Status)
                    .QueryMany<PdCategorySql>();
                if (categorys != null && categorys.Count > 0)
                {
                    var categoryInfo = categorys.First();
                    model.PdCategoryAssociation.CategorySysNo = categoryInfo.SysNo;
                    model.PdCategoryAssociation.ProductSysNo = productSysNo;

                    int categoryCount=Context.Sql("select count(1) from PdCategoryAssociation where ProductSysNo="+productSysNo+" and IsMaster=1 and CategorySysNo="+categoryInfo.SysNo).QuerySingle<int>();
                    if(categoryCount<=0)
                    {
                        Context.Sql("Delete from PdCategoryAssociation where ProductSysNo=@ProductSysNo and IsMaster=1")
                        .Parameter("ProductSysNo", productSysNo)
                        .Execute();
                        int PdCategoryAssociationSysNo = Context.Insert<PdCategoryAssociation>("PdCategoryAssociation", model.PdCategoryAssociation)
                                               .AutoMap(o => o.SysNo)
                                               .ExecuteReturnLastId<int>("SysNo");
                    }
            
                }     

                //判断商品的基础价格是否存在
                PdPrice entity = Context.Select<PdPrice>("*")
                    .From("PdPrice")
                    .Where("ProductSysNo= @ProductSysNo and PriceSource = @PriceSource and SourceSysNo = @SourceSysNo and Status = @Status")
                    .Parameter("ProductSysNo", model.PdPrice.ProductSysNo)
                    .Parameter("PriceSource", model.PdPrice.PriceSource)
                    .Parameter("SourceSysNo", model.PdPrice.SourceSysNo)
                    .Parameter("Status", model.PdPrice.Status)
                    .QuerySingle();
                if (entity != null && entity.SysNo > 0)
                {
                    Context.Update("PdPrice")
                    .Column("Price", model.PdPrice.Price)
                    .Where("ProductSysNo", model.PdPrice.ProductSysNo)
                    .Where("PriceSource", model.PdPrice.PriceSource)
                    .Where("SourceSysNo", model.PdPrice.SourceSysNo)
                    .Where("Status", model.PdPrice.Status)
                    .Execute();
                }
                else
                {
                    int PdPriceSysNo = Context.Insert<PdPrice>("PdPrice", model.PdPrice)
                                           .AutoMap(o => o.SysNo)
                                           .ExecuteReturnLastId<int>("SysNo");
                }

                //判断商品的会员价格是否存在
                PdPrice saleentity = Context.Select<PdPrice>("*")
                    .From("PdPrice")
                    .Where("ProductSysNo= @ProductSysNo and PriceSource = @PriceSource and SourceSysNo = @SourceSysNo and Status = @Status")
                    .Parameter("ProductSysNo", model.PdSalePrice.ProductSysNo)
                    .Parameter("PriceSource", model.PdSalePrice.PriceSource)
                    .Parameter("SourceSysNo", model.PdSalePrice.SourceSysNo)
                    .Parameter("Status", model.PdSalePrice.Status)
                    .QuerySingle();
                if (saleentity != null && saleentity.SysNo > 0)
                {
                    Context.Update("PdPrice")
                    .Column("Price", model.PdSalePrice.Price)
                    .Where("ProductSysNo", model.PdSalePrice.ProductSysNo)
                    .Where("PriceSource", model.PdSalePrice.PriceSource)
                    .Where("SourceSysNo", model.PdSalePrice.SourceSysNo)
                    .Where("Status", model.PdSalePrice.Status)
                    .Execute();
                }
                else
                {
                    int PdPriceSysNo = Context.Insert<PdPrice>("PdPrice", model.PdSalePrice)
                                           .AutoMap(o => o.SysNo)
                                           .ExecuteReturnLastId<int>("SysNo");
                }

                if (model.PdVIPPrice != null)
                {
                    model.PdVIPPrice.ProductSysNo = productSysNo;
                    //判断商品的会员价格是否存在
                    PdPrice VIPentity = Context.Select<PdPrice>("*")
                        .From("PdPrice")
                        .Where("ProductSysNo= @ProductSysNo and PriceSource = @PriceSource and SourceSysNo = @SourceSysNo and Status = @Status")
                        .Parameter("ProductSysNo", model.PdVIPPrice.ProductSysNo)
                        .Parameter("PriceSource", model.PdVIPPrice.PriceSource)
                        .Parameter("SourceSysNo", model.PdVIPPrice.SourceSysNo)
                        .Parameter("Status", model.PdVIPPrice.Status)
                        .QuerySingle();
                    if (VIPentity != null && VIPentity.SysNo > 0)
                    {
                        Context.Update("PdPrice")
                        .Column("Price", model.PdVIPPrice.Price)
                        .Where("ProductSysNo", model.PdVIPPrice.ProductSysNo)
                        .Where("PriceSource", model.PdVIPPrice.PriceSource)
                        .Where("SourceSysNo", model.PdVIPPrice.SourceSysNo)
                        .Where("Status", model.PdVIPPrice.Status)
                        .Execute();
                    }
                    else
                    {
                       
                        int PdPriceSysNo = Context.Insert<PdPrice>("PdPrice", model.PdVIPPrice)
                                               .AutoMap(o => o.SysNo)
                                               .ExecuteReturnLastId<int>("SysNo");
                    }
                }

                if (model.PdDiamondPrice != null)
                {
                    model.PdDiamondPrice.ProductSysNo = productSysNo;
                    //判断商品的钻石会员价格是否存在
                    PdPrice Diamondentity = Context.Select<PdPrice>("*")
                        .From("PdPrice")
                        .Where("ProductSysNo= @ProductSysNo and PriceSource = @PriceSource and SourceSysNo = @SourceSysNo and Status = @Status")
                        .Parameter("ProductSysNo", model.PdDiamondPrice.ProductSysNo)
                        .Parameter("PriceSource", model.PdDiamondPrice.PriceSource)
                        .Parameter("SourceSysNo", model.PdDiamondPrice.SourceSysNo)
                        .Parameter("Status", model.PdDiamondPrice.Status)
                        .QuerySingle();
                    if (Diamondentity != null && Diamondentity.SysNo > 0)
                    {
                        Context.Update("PdPrice")
                        .Column("Price", model.PdDiamondPrice.Price)
                        .Where("ProductSysNo", model.PdDiamondPrice.ProductSysNo)
                        .Where("PriceSource", model.PdDiamondPrice.PriceSource)
                        .Where("SourceSysNo", model.PdDiamondPrice.SourceSysNo)
                        .Where("Status", model.PdDiamondPrice.Status)
                        .Execute();
                    }
                    else
                    {
                        int PdPriceSysNo = Context.Insert<PdPrice>("PdPrice", model.PdDiamondPrice)
                                               .AutoMap(o => o.SysNo)
                                               .ExecuteReturnLastId<int>("SysNo");
                    }
                }

                if (model.SaleUserPrice != null)
                {
                    model.SaleUserPrice.ProductSysNo = productSysNo;
                    //判断商品的钻石会员价格是否存在
                    PdPrice Diamondentity = Context.Select<PdPrice>("*")
                        .From("PdPrice")
                        .Where("ProductSysNo= @ProductSysNo and PriceSource = @PriceSource and SourceSysNo = @SourceSysNo and Status = @Status")
                        .Parameter("ProductSysNo", model.SaleUserPrice.ProductSysNo)
                        .Parameter("PriceSource", model.SaleUserPrice.PriceSource)
                        .Parameter("SourceSysNo", model.SaleUserPrice.SourceSysNo)
                        .Parameter("Status", model.SaleUserPrice.Status)
                        .QuerySingle();
                    if (Diamondentity != null && Diamondentity.SysNo > 0)
                    {
                        Context.Update("PdPrice")
                        .Column("Price", model.SaleUserPrice.Price)
                        .Where("ProductSysNo", model.SaleUserPrice.ProductSysNo)
                        .Where("PriceSource", model.SaleUserPrice.PriceSource)
                        .Where("SourceSysNo", model.SaleUserPrice.SourceSysNo)
                        .Where("Status", model.SaleUserPrice.Status)
                        .Execute();
                    }
                    else
                    {
                        int PdPriceSysNo = Context.Insert<PdPrice>("PdPrice", model.SaleUserPrice)
                                               .AutoMap(o => o.SysNo)
                                               .ExecuteReturnLastId<int>("SysNo");
                    }
                }

                //判断商品的门店销售价是否存在
                PdPrice ssaleentity = Context.Select<PdPrice>("*")
                    .From("PdPrice")
                    .Where("ProductSysNo= @ProductSysNo and PriceSource = @PriceSource and SourceSysNo = @SourceSysNo and Status = @Status")
                    .Parameter("ProductSysNo", model.PdStoreSalePrice.ProductSysNo)
                    .Parameter("PriceSource", model.PdStoreSalePrice.PriceSource)
                    .Parameter("SourceSysNo", model.PdStoreSalePrice.SourceSysNo)
                    .Parameter("Status", model.PdStoreSalePrice.Status)
                    .QuerySingle();
                if (ssaleentity != null && ssaleentity.SysNo > 0)
                {
                    Context.Update("PdPrice")
                    .Column("Price", model.PdStoreSalePrice.Price)
                    .Where("ProductSysNo", model.PdStoreSalePrice.ProductSysNo)
                    .Where("PriceSource", model.PdStoreSalePrice.PriceSource)
                    .Where("SourceSysNo", model.PdStoreSalePrice.SourceSysNo)
                    .Where("Status", model.PdStoreSalePrice.Status)
                    .Execute();
                }
                else
                {
                    int PdPriceSysNo = Context.Insert<PdPrice>("PdPrice", model.PdStoreSalePrice)
                                           .AutoMap(o => o.SysNo)
                                           .ExecuteReturnLastId<int>("SysNo");
                }

                if (model.Sales > 0)
                {
                    PdProductStatistics PdProductSale = PdProductStatisticsDaoImpl.Instance.Get(productSysNo);
                    if (PdProductSale == null)
                    {
                        PdProductSale = new PdProductStatistics();
                        PdProductSale.ProductSysNo = productSysNo;
                        PdProductSale.Sales = model.Sales;
                        PdProductSale.AverageScore = 5;
                        PdProductStatisticsDaoImpl.Instance.Create(PdProductSale);
                    }
                    else
                    {
                        PdProductSale.Sales = model.Sales;
                        PdProductStatisticsDaoImpl.Instance.Update(PdProductSale);
                    }
                }
            }
        }

        /// <summary>
        /// 更新EXCEL中导入的商品信息
        /// </summary>
        /// <param name="models">商品信息列表</param>
        /// <param name="productList">系统已有产品列表</param>
        /// <returns></returns>
        /// <remarks>2016-09-23 杨浩 创建</remarks>
        public override void UpdateExcelProductByYD(List<PdProductList> models, IList<PdProduct> productList)
        {
            foreach (PdProductList model in models)
            {
                var pdata = productList.Where(x => x.ErpCode == model.ErpCode).FirstOrDefault();

                int productSysNo = pdata.SysNo;

                Context.Update("PdProduct")
                .Column("BrandSysNo", model.BrandSysNo)
                .Column("ProductName", model.ProductName)
                .Column("EasName", model.EasName)
                .Column("ProductType", model.ProductType)
                .Column("OriginSysNo", model.OriginSysNo)
                .Column("Barcode", model.Barcode)
                .Column("GrosWeight", model.GrosWeight)
                .Column("Tax", model.Tax)
                .Column("PriceRate", model.PriceRate)
                .Column("PriceValue", model.PriceValue)
                .Column("TradePrice", model.TradePrice)
                .Column("CostPrice", model.CostPrice)
                .Column("Status", model.Status)
                .Column("CanFrontEndOrder", model.CanFrontEndOrder)
                .Column("IsFrontDisplay", model.IsFrontDisplay)
                .Column("LastUpdateBy", model.LastUpdateBy)
                .Column("LastUpdateDate", model.LastUpdateDate)
                .Where("SysNo", productSysNo)
                .Execute();

                model.PdPrice.ProductSysNo = productSysNo;
                model.PdSalePrice.ProductSysNo = productSysNo;
                model.PdStoreSalePrice.ProductSysNo = productSysNo;

                //判断类目是否存在
                var categorys = Context.Sql(" select * from PdCategory where CategoryName = @CategoryName and IsOnline = @IsOnline and Status = @Status  ")
                    .Parameter("CategoryName", model.PdCategorySql.CategoryName)
                    .Parameter("IsOnline", model.PdCategorySql.IsOnline)
                    .Parameter("Status", model.PdCategorySql.Status)
                    .QueryMany<PdCategorySql>();
                if (categorys != null && categorys.Count > 0)
                {
                    var categoryInfo = categorys.First();
                    model.PdCategoryAssociation.CategorySysNo = categoryInfo.SysNo;
                    model.PdCategoryAssociation.ProductSysNo = productSysNo;

                    int categoryCount = Context.Sql("select count(1) from PdCategoryAssociation where ProductSysNo=" + productSysNo + " and IsMaster=1 and CategorySysNo=" + categoryInfo.SysNo).QuerySingle<int>();
                    if (categoryCount <= 0)
                    {
                        Context.Sql("Delete from PdCategoryAssociation where ProductSysNo=@ProductSysNo and IsMaster=1")
                        .Parameter("ProductSysNo", productSysNo)
                        .Execute();
                        int PdCategoryAssociationSysNo = Context.Insert<PdCategoryAssociation>("PdCategoryAssociation", model.PdCategoryAssociation)
                                               .AutoMap(o => o.SysNo)
                                               .ExecuteReturnLastId<int>("SysNo");
                    }

                }

                //判断商品的基础价格是否存在
                PdPrice entity = Context.Select<PdPrice>("*")
                    .From("PdPrice")
                    .Where("ProductSysNo= @ProductSysNo and PriceSource = @PriceSource and SourceSysNo = @SourceSysNo and Status = @Status")
                    .Parameter("ProductSysNo", model.PdPrice.ProductSysNo)
                    .Parameter("PriceSource", model.PdPrice.PriceSource)
                    .Parameter("SourceSysNo", model.PdPrice.SourceSysNo)
                    .Parameter("Status", model.PdPrice.Status)
                    .QuerySingle();
                if (entity != null && entity.SysNo > 0)
                {
                    Context.Update("PdPrice")
                    .Column("Price", model.PdPrice.Price)
                    .Where("ProductSysNo", model.PdPrice.ProductSysNo)
                    .Where("PriceSource", model.PdPrice.PriceSource)
                    .Where("SourceSysNo", model.PdPrice.SourceSysNo)
                    .Where("Status", model.PdPrice.Status)
                    .Execute();
                }
                else
                {
                    int PdPriceSysNo = Context.Insert<PdPrice>("PdPrice", model.PdPrice)
                                           .AutoMap(o => o.SysNo)
                                           .ExecuteReturnLastId<int>("SysNo");
                }

                //判断商品的会员价格是否存在
                PdPrice saleentity = Context.Select<PdPrice>("*")
                    .From("PdPrice")
                    .Where("ProductSysNo= @ProductSysNo and PriceSource = @PriceSource and SourceSysNo = @SourceSysNo and Status = @Status")
                    .Parameter("ProductSysNo", model.PdSalePrice.ProductSysNo)
                    .Parameter("PriceSource", model.PdSalePrice.PriceSource)
                    .Parameter("SourceSysNo", model.PdSalePrice.SourceSysNo)
                    .Parameter("Status", model.PdSalePrice.Status)
                    .QuerySingle();
                if (saleentity != null && saleentity.SysNo > 0)
                {
                    Context.Update("PdPrice")
                    .Column("Price", model.PdSalePrice.Price)
                    .Where("ProductSysNo", model.PdSalePrice.ProductSysNo)
                    .Where("PriceSource", model.PdSalePrice.PriceSource)
                    .Where("SourceSysNo", model.PdSalePrice.SourceSysNo)
                    .Where("Status", model.PdSalePrice.Status)
                    .Execute();
                }
                else
                {
                    int PdPriceSysNo = Context.Insert<PdPrice>("PdPrice", model.PdSalePrice)
                                           .AutoMap(o => o.SysNo)
                                           .ExecuteReturnLastId<int>("SysNo");
                }

                //判断商品的门店销售价是否存在
                PdPrice ssaleentity = Context.Select<PdPrice>("*")
                    .From("PdPrice")
                    .Where("ProductSysNo= @ProductSysNo and PriceSource = @PriceSource and SourceSysNo = @SourceSysNo and Status = @Status")
                    .Parameter("ProductSysNo", model.PdStoreSalePrice.ProductSysNo)
                    .Parameter("PriceSource", model.PdStoreSalePrice.PriceSource)
                    .Parameter("SourceSysNo", model.PdStoreSalePrice.SourceSysNo)
                    .Parameter("Status", model.PdStoreSalePrice.Status)
                    .QuerySingle();
                if (ssaleentity != null && ssaleentity.SysNo > 0)
                {
                    Context.Update("PdPrice")
                    .Column("Price", model.PdStoreSalePrice.Price)
                    .Where("ProductSysNo", model.PdStoreSalePrice.ProductSysNo)
                    .Where("PriceSource", model.PdStoreSalePrice.PriceSource)
                    .Where("SourceSysNo", model.PdStoreSalePrice.SourceSysNo)
                    .Where("Status", model.PdStoreSalePrice.Status)
                    .Execute();
                }
                else
                {
                    int PdPriceSysNo = Context.Insert<PdPrice>("PdPrice", model.PdStoreSalePrice)
                                           .AutoMap(o => o.SysNo)
                                           .ExecuteReturnLastId<int>("SysNo");
                }
            }
        }

        /// <summary>
        /// 更新EXCEL中导入的商品信息
        /// </summary>
        /// <param name="models">商品信息列表</param>
        /// <param name="productList">系统已有产品列表</param>
        /// <returns></returns>
        /// <remarks>2016-09-23 杨浩 创建</remarks>
        public override void UpdateExcelProductByYS(List<PdProductList> models, IList<PdProduct> productList)
        {
            foreach (PdProductList model in models)
            {
                var pdata = productList.Where(x => x.ErpCode == model.ErpCode).FirstOrDefault();

                int productSysNo = pdata.SysNo;

                Context.Update("PdProduct")
                .Column("BrandSysNo", model.BrandSysNo)
                .Column("ProductName", model.ProductName)
                .Column("EasName", model.EasName)
                .Column("ProductType", model.ProductType)
                .Column("OriginSysNo", model.OriginSysNo)
                .Column("Barcode", model.Barcode)
                .Column("GrosWeight", model.GrosWeight)
                .Column("Tax", model.Tax)
                .Column("PriceRate", model.PriceRate)
                .Column("PriceValue", model.PriceValue)
                .Column("TradePrice", model.TradePrice)
                .Column("LastUpdateBy", model.LastUpdateBy)
                .Column("LastUpdateDate", model.LastUpdateDate)
                .Column("ProductShortTitle", model.ProductShortTitle)
                .Where("SysNo", productSysNo)
                .Execute();

                model.PdPrice.ProductSysNo = productSysNo;
                model.PdSalePrice.ProductSysNo = productSysNo;
                model.PdStoreSalePrice.ProductSysNo = productSysNo;

                //判断类目是否存在
                var categorys = Context.Sql(" select * from PdCategory where CategoryName = @CategoryName and IsOnline = @IsOnline and Status = @Status  ")
                    .Parameter("CategoryName", model.PdCategorySql.CategoryName)
                    .Parameter("IsOnline", model.PdCategorySql.IsOnline)
                    .Parameter("Status", model.PdCategorySql.Status)
                    .QueryMany<PdCategorySql>();
                if (categorys != null && categorys.Count > 0)
                {
                    var categoryInfo = categorys.First();
                    model.PdCategoryAssociation.CategorySysNo = categoryInfo.SysNo;
                    model.PdCategoryAssociation.ProductSysNo = productSysNo;

                    int categoryCount = Context.Sql("select count(1) from PdCategoryAssociation where ProductSysNo=" + productSysNo + " and IsMaster=1 and CategorySysNo=" + categoryInfo.SysNo).QuerySingle<int>();
                    if (categoryCount <= 0)
                    {
                        Context.Sql("Delete from PdCategoryAssociation where ProductSysNo=@ProductSysNo and IsMaster=1")
                        .Parameter("ProductSysNo", productSysNo)
                        .Execute();
                        int PdCategoryAssociationSysNo = Context.Insert<PdCategoryAssociation>("PdCategoryAssociation", model.PdCategoryAssociation)
                                               .AutoMap(o => o.SysNo)
                                               .ExecuteReturnLastId<int>("SysNo");
                    }

                }

                //判断商品的基础价格是否存在
                PdPrice entity = Context.Select<PdPrice>("*")
                    .From("PdPrice")
                    .Where("ProductSysNo= @ProductSysNo and PriceSource = @PriceSource and SourceSysNo = @SourceSysNo and Status = @Status")
                    .Parameter("ProductSysNo", model.PdPrice.ProductSysNo)
                    .Parameter("PriceSource", model.PdPrice.PriceSource)
                    .Parameter("SourceSysNo", model.PdPrice.SourceSysNo)
                    .Parameter("Status", model.PdPrice.Status)
                    .QuerySingle();
                if (entity != null && entity.SysNo > 0)
                {
                    Context.Update("PdPrice")
                    .Column("Price", model.PdPrice.Price)
                    .Where("ProductSysNo", model.PdPrice.ProductSysNo)
                    .Where("PriceSource", model.PdPrice.PriceSource)
                    .Where("SourceSysNo", model.PdPrice.SourceSysNo)
                    .Where("Status", model.PdPrice.Status)
                    .Execute();
                }
                else
                {
                    int PdPriceSysNo = Context.Insert<PdPrice>("PdPrice", model.PdPrice)
                                           .AutoMap(o => o.SysNo)
                                           .ExecuteReturnLastId<int>("SysNo");
                }

                //判断商品的会员价格是否存在
                PdPrice saleentity = Context.Select<PdPrice>("*")
                    .From("PdPrice")
                    .Where("ProductSysNo= @ProductSysNo and PriceSource = @PriceSource and SourceSysNo = @SourceSysNo and Status = @Status")
                    .Parameter("ProductSysNo", model.PdSalePrice.ProductSysNo)
                    .Parameter("PriceSource", model.PdSalePrice.PriceSource)
                    .Parameter("SourceSysNo", model.PdSalePrice.SourceSysNo)
                    .Parameter("Status", model.PdSalePrice.Status)
                    .QuerySingle();
                if (saleentity != null && saleentity.SysNo > 0)
                {
                    Context.Update("PdPrice")
                    .Column("Price", model.PdSalePrice.Price)
                    .Where("ProductSysNo", model.PdSalePrice.ProductSysNo)
                    .Where("PriceSource", model.PdSalePrice.PriceSource)
                    .Where("SourceSysNo", model.PdSalePrice.SourceSysNo)
                    .Where("Status", model.PdSalePrice.Status)
                    .Execute();
                }
                else
                {
                    int PdPriceSysNo = Context.Insert<PdPrice>("PdPrice", model.PdSalePrice)
                                           .AutoMap(o => o.SysNo)
                                           .ExecuteReturnLastId<int>("SysNo");
                }

                //判断商品的门店销售价是否存在
                PdPrice ssaleentity = Context.Select<PdPrice>("*")
                    .From("PdPrice")
                    .Where("ProductSysNo= @ProductSysNo and PriceSource = @PriceSource and SourceSysNo = @SourceSysNo and Status = @Status")
                    .Parameter("ProductSysNo", model.PdStoreSalePrice.ProductSysNo)
                    .Parameter("PriceSource", model.PdStoreSalePrice.PriceSource)
                    .Parameter("SourceSysNo", model.PdStoreSalePrice.SourceSysNo)
                    .Parameter("Status", model.PdStoreSalePrice.Status)
                    .QuerySingle();
                if (ssaleentity != null && ssaleentity.SysNo > 0)
                {
                    Context.Update("PdPrice")
                    .Column("Price", model.PdStoreSalePrice.Price)
                    .Where("ProductSysNo", model.PdStoreSalePrice.ProductSysNo)
                    .Where("PriceSource", model.PdStoreSalePrice.PriceSource)
                    .Where("SourceSysNo", model.PdStoreSalePrice.SourceSysNo)
                    .Where("Status", model.PdStoreSalePrice.Status)
                    .Execute();
                }
                else
                {
                    int PdPriceSysNo = Context.Insert<PdPrice>("PdPrice", model.PdStoreSalePrice)
                                           .AutoMap(o => o.SysNo)
                                           .ExecuteReturnLastId<int>("SysNo");
                }
            }
        }

        /// <summary>
        /// 更新商品信息
        /// </summary>
        /// <param name="models">商品信息列表</param>
        /// <returns>空</returns>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        public override void UpdateExcelPdProduct(List<PdProductList> models)
        {
            var productList=GetAllPdProduct();
            UpdateExcelProduct(models, productList);
        }

        #region 信营导入
        /// <summary>
        /// 新增商品信息（信营）
        /// </summary>
        /// <param name="models">商品信息列表</param>
        /// <returns>空</returns>
        public override void CreateXinYingPdProduct(List<PdProductList> models)
        {
            foreach (PdProductList model in models)
            {
                //var sql = @"insert into PdProduct(BrandSysNo,ErpCode,EasName,Volume,GrosWeight,CreatedBy,CreatedDate,LastUpdateBy,LastUpdateDate) select @0,@1,@2,@3,@4,@5,@6,@7,@8";
                //Context.Sql(sql, model.BrandSysNo, model.ErpCode, model.EasName, model.Volume, model.GrosWeight, model.CreatedBy, model.CreatedDate, model.LastUpdateBy, model.LastUpdateDate).Execute();

                PdProduct PdData = new PdProduct();
                PdData.BrandSysNo = model.BrandSysNo;
                PdData.ErpCode = model.ErpCode;
                PdData.ProductName = model.ProductName;
                PdData.EasName = model.EasName;
                PdData.ProductType = model.ProductType;
                PdData.OriginSysNo = model.OriginSysNo;
                PdData.Barcode = model.Barcode;
                PdData.GrosWeight = model.GrosWeight;
                PdData.NetWeight = model.NetWeight;
                PdData.Tax = decimal.Parse(model.Tax);
                PdData.PriceRate = model.PriceRate;
                PdData.PriceValue = model.PriceValue;
                PdData.TradePrice = model.TradePrice;
                PdData.AgentSysNo = model.AgentSysNo;
                PdData.CreatedBy = model.CreatedBy;
                PdData.CreatedDate = model.CreatedDate;
                PdData.LastUpdateBy = model.LastUpdateBy;
                PdData.LastUpdateDate = model.LastUpdateDate;
                PdData.CanFrontEndOrder = 1;
                int ProductSysNo = Context.Insert<PdProduct>("PdProduct", PdData)
                                       .AutoMap(o => o.SysNo, o => o.Stamp)
                                       .ExecuteReturnLastId<int>("SysNo");
                model.PdPrice.ProductSysNo = ProductSysNo;
                model.PdSalePrice.ProductSysNo = ProductSysNo;
                model.PdStoreSalePrice.ProductSysNo = ProductSysNo;
                //判断类目是否存在
                var categorys = Context.Sql(" select * from PdCategory where CategoryName = @CategoryName and IsOnline = @IsOnline and Status = @Status  ")
                    //.Parameter("ParentSysNo", model.PdCategorySql.ParentSysNo)
                    .Parameter("CategoryName", model.PdCategorySql.CategoryName)
                    .Parameter("IsOnline", model.PdCategorySql.IsOnline)
                    .Parameter("Status", model.PdCategorySql.Status)
                    .QueryMany<PdCategorySql>();
                if (categorys != null && categorys.Count > 0)
                {
                    var categoryInfo = categorys.First();
                    model.PdCategoryAssociation.CategorySysNo = categoryInfo.SysNo;
                    model.PdCategoryAssociation.ProductSysNo = ProductSysNo;
                    Context.Sql("Delete from PdCategoryAssociation where ProductSysNo=@ProductSysNo and IsMaster=1")
                    .Parameter("ProductSysNo", ProductSysNo)
                    .Execute();
                    int PdCategoryAssociationSysNo = Context.Insert<PdCategoryAssociation>("PdCategoryAssociation", model.PdCategoryAssociation)
                                           .AutoMap(o => o.SysNo)
                                           .ExecuteReturnLastId<int>("SysNo");
                }

                //model.PdCategoryAssociation.ProductSysNo = ProductSysNo;
                //int PdCategoryAssociationSysNo = Context.Insert<PdCategoryAssociation>("PdCategoryAssociation", model.PdCategoryAssociation)
                                       //.AutoMap(o => o.SysNo)
                                       //.ExecuteReturnLastId<int>("SysNo");

                //判断商品的基础价格是否存在
                PdPrice entity = Context.Select<PdPrice>("*")
                    .From("PdPrice")
                    .Where("ProductSysNo= @ProductSysNo and PriceSource = @PriceSource and SourceSysNo = @SourceSysNo and Status = @Status")
                    .Parameter("ProductSysNo", model.PdPrice.ProductSysNo)
                    .Parameter("PriceSource", model.PdPrice.PriceSource)
                    .Parameter("SourceSysNo", model.PdPrice.SourceSysNo)
                    .Parameter("Status", model.PdPrice.Status)
                    .QuerySingle();
                if (entity != null && entity.SysNo > 0)
                {
                    Context.Update("PdPrice")
                    .Column("Price", model.PdPrice.Price)
                    .Where("ProductSysNo", model.PdPrice.ProductSysNo)
                    .Where("PriceSource", model.PdPrice.PriceSource)
                    .Where("SourceSysNo", model.PdPrice.SourceSysNo)
                    .Where("Status", model.PdPrice.Status)
                    .Execute();
                }
                else
                {
                    int PdPriceSysNo = Context.Insert<PdPrice>("PdPrice", model.PdPrice)
                                           .AutoMap(o => o.SysNo)
                                           .ExecuteReturnLastId<int>("SysNo");
                }

                //判断商品的会员价格是否存在
                PdPrice saleentity = Context.Select<PdPrice>("*")
                    .From("PdPrice")
                    .Where("ProductSysNo= @ProductSysNo and PriceSource = @PriceSource and SourceSysNo = @SourceSysNo and Status = @Status")
                    .Parameter("ProductSysNo", model.PdSalePrice.ProductSysNo)
                    .Parameter("PriceSource", model.PdSalePrice.PriceSource)
                    .Parameter("SourceSysNo", model.PdSalePrice.SourceSysNo)
                    .Parameter("Status", model.PdSalePrice.Status)
                    .QuerySingle();
                if (saleentity != null && saleentity.SysNo > 0)
                {
                    Context.Update("PdPrice")
                    .Column("Price", model.PdSalePrice.Price)
                    .Where("ProductSysNo", model.PdSalePrice.ProductSysNo)
                    .Where("PriceSource", model.PdSalePrice.PriceSource)
                    .Where("SourceSysNo", model.PdSalePrice.SourceSysNo)
                    .Where("Status", model.PdSalePrice.Status)
                    .Execute();
                }
                else
                {
                    int PdPriceSysNo = Context.Insert<PdPrice>("PdPrice", model.PdSalePrice)
                                           .AutoMap(o => o.SysNo)
                                           .ExecuteReturnLastId<int>("SysNo");
                }

                //判断商品的门店销售价是否存在
                PdPrice ssaleentity = Context.Select<PdPrice>("*")
                    .From("PdPrice")
                    .Where("ProductSysNo= @ProductSysNo and PriceSource = @PriceSource and SourceSysNo = @SourceSysNo and Status = @Status")
                    .Parameter("ProductSysNo", model.PdStoreSalePrice.ProductSysNo)
                    .Parameter("PriceSource", model.PdStoreSalePrice.PriceSource)
                    .Parameter("SourceSysNo", model.PdStoreSalePrice.SourceSysNo)
                    .Parameter("Status", model.PdStoreSalePrice.Status)
                    .QuerySingle();
                if (ssaleentity != null && ssaleentity.SysNo > 0)
                {
                    Context.Update("PdPrice")
                    .Column("Price", model.PdStoreSalePrice.Price)
                    .Where("ProductSysNo", model.PdStoreSalePrice.ProductSysNo)
                    .Where("PriceSource", model.PdStoreSalePrice.PriceSource)
                    .Where("SourceSysNo", model.PdStoreSalePrice.SourceSysNo)
                    .Where("Status", model.PdStoreSalePrice.Status)
                    .Execute();
                }
                else
                {
                    int PdPriceSysNo = Context.Insert<PdPrice>("PdPrice", model.PdStoreSalePrice)
                                           .AutoMap(o => o.SysNo)
                                           .ExecuteReturnLastId<int>("SysNo");
                }
            }
        }
        /// <summary>
        /// 更新商品信息（信营）
        /// </summary>
        /// <param name="models">商品信息列表</param>
        /// <returns>空</returns>
        public override void UpdateXinYingExcelPdProduct(List<PdProductList> models)
        {
            foreach (PdProductList model in models)
            {
                PdProduct pdata = GetProductByErpCode(model.ErpCode);
                int ProductSysNo = pdata.SysNo;

                Context.Update("PdProduct")
                .Column("BrandSysNo", model.BrandSysNo).Column("ProductName", model.ProductName)
                .Column("EasName", model.EasName)
                .Column("ProductType", model.ProductType)
                .Column("OriginSysNo", model.OriginSysNo)
                .Column("Barcode", model.Barcode)
                .Column("GrosWeight", model.GrosWeight)
                .Column("NetWeight", model.NetWeight)
                .Column("Tax", model.Tax).Column("PriceRate", model.PriceRate)
                .Column("PriceValue", model.PriceValue)
                .Column("TradePrice", model.TradePrice)
                .Column("LastUpdateBy", model.LastUpdateBy)
                .Column("LastUpdateDate", model.LastUpdateDate)
                .Where("SysNo", ProductSysNo)
                .Execute();

                model.PdPrice.ProductSysNo = ProductSysNo;
                model.PdSalePrice.ProductSysNo = ProductSysNo;
                model.PdStoreSalePrice.ProductSysNo = ProductSysNo;

                //判断类目是否存在
                PdCategorySql PdCentity = Context.Select<PdCategorySql>("*")
                    .From("PdCategory")
                    .Where("CategoryName = @CategoryName and IsOnline = @IsOnline and Status = @Status")
                    //.Parameter("ParentSysNo", model.PdCategorySql.ParentSysNo)
                    .Parameter("CategoryName", model.PdCategorySql.CategoryName)
                    .Parameter("IsOnline", model.PdCategorySql.IsOnline)
                    .Parameter("Status", model.PdCategorySql.Status)
                    .QuerySingle();
                if (PdCentity != null && PdCentity.SysNo > 0)
                {
                    model.PdCategoryAssociation.CategorySysNo = PdCentity.SysNo;
                    model.PdCategoryAssociation.ProductSysNo = ProductSysNo;
                    Context.Sql("Delete from PdCategoryAssociation where ProductSysNo=@ProductSysNo and IsMaster=1")
                    .Parameter("ProductSysNo", ProductSysNo)
                    .Execute();
                    int PdCategoryAssociationSysNo = Context.Insert<PdCategoryAssociation>("PdCategoryAssociation", model.PdCategoryAssociation)
                                           .AutoMap(o => o.SysNo)
                                           .ExecuteReturnLastId<int>("SysNo");
                }
                //else
                //{
                //    int CategorySysNo = Context.Insert<PdCategorySql>("PdCategory", model.PdCategorySql)
                //                           .AutoMap(o => o.SysNo)
                //                           .ExecuteReturnLastId<int>("SysNo");
                //    Context.Update("PdCategory").Column("SysNos", "," + CategorySysNo.ToString() + ",").Where("SysNo", CategorySysNo).Execute();

                //    model.PdCategoryAssociation.CategorySysNo = CategorySysNo;
                //}
                //model.PdCategoryAssociation.ProductSysNo = ProductSysNo;
                //Context.Sql("Delete from PdCategoryAssociation where ProductSysNo=@ProductSysNo")
                //.Parameter("ProductSysNo", ProductSysNo)
                //.Execute();
                //int PdCategoryAssociationSysNo = Context.Insert<PdCategoryAssociation>("PdCategoryAssociation", model.PdCategoryAssociation)
                //                       .AutoMap(o => o.SysNo)
                //                       .ExecuteReturnLastId<int>("SysNo");

                //判断商品的基础价格是否存在
                PdPrice entity = Context.Select<PdPrice>("*")
                    .From("PdPrice")
                    .Where("ProductSysNo= @ProductSysNo and PriceSource = @PriceSource and SourceSysNo = @SourceSysNo and Status = @Status")
                    .Parameter("ProductSysNo", model.PdPrice.ProductSysNo)
                    .Parameter("PriceSource", model.PdPrice.PriceSource)
                    .Parameter("SourceSysNo", model.PdPrice.SourceSysNo)
                    .Parameter("Status", model.PdPrice.Status)
                    .QuerySingle();
                if (entity != null && entity.SysNo > 0)
                {
                    Context.Update("PdPrice")
                    .Column("Price", model.PdPrice.Price)
                    .Where("ProductSysNo", model.PdPrice.ProductSysNo)
                    .Where("PriceSource", model.PdPrice.PriceSource)
                    .Where("SourceSysNo", model.PdPrice.SourceSysNo)
                    .Where("Status", model.PdPrice.Status)
                    .Execute();
                }
                else
                {
                    int PdPriceSysNo = Context.Insert<PdPrice>("PdPrice", model.PdPrice)
                                           .AutoMap(o => o.SysNo)
                                           .ExecuteReturnLastId<int>("SysNo");
                }

                //判断商品的会员价格是否存在
                PdPrice saleentity = Context.Select<PdPrice>("*")
                    .From("PdPrice")
                    .Where("ProductSysNo= @ProductSysNo and PriceSource = @PriceSource and SourceSysNo = @SourceSysNo and Status = @Status")
                    .Parameter("ProductSysNo", model.PdSalePrice.ProductSysNo)
                    .Parameter("PriceSource", model.PdSalePrice.PriceSource)
                    .Parameter("SourceSysNo", model.PdSalePrice.SourceSysNo)
                    .Parameter("Status", model.PdSalePrice.Status)
                    .QuerySingle();
                if (saleentity != null && saleentity.SysNo > 0)
                {
                    Context.Update("PdPrice")
                    .Column("Price", model.PdSalePrice.Price)
                    .Where("ProductSysNo", model.PdSalePrice.ProductSysNo)
                    .Where("PriceSource", model.PdSalePrice.PriceSource)
                    .Where("SourceSysNo", model.PdSalePrice.SourceSysNo)
                    .Where("Status", model.PdSalePrice.Status)
                    .Execute();
                }
                else
                {
                    int PdPriceSysNo = Context.Insert<PdPrice>("PdPrice", model.PdSalePrice)
                                           .AutoMap(o => o.SysNo)
                                           .ExecuteReturnLastId<int>("SysNo");
                }

                //判断商品的门店销售价是否存在
                PdPrice ssaleentity = Context.Select<PdPrice>("*")
                    .From("PdPrice")
                    .Where("ProductSysNo= @ProductSysNo and PriceSource = @PriceSource and SourceSysNo = @SourceSysNo and Status = @Status")
                    .Parameter("ProductSysNo", model.PdStoreSalePrice.ProductSysNo)
                    .Parameter("PriceSource", model.PdStoreSalePrice.PriceSource)
                    .Parameter("SourceSysNo", model.PdStoreSalePrice.SourceSysNo)
                    .Parameter("Status", model.PdStoreSalePrice.Status)
                    .QuerySingle();
                if (ssaleentity != null && ssaleentity.SysNo > 0)
                {
                    Context.Update("PdPrice")
                    .Column("Price", model.PdStoreSalePrice.Price)
                    .Where("ProductSysNo", model.PdStoreSalePrice.ProductSysNo)
                    .Where("PriceSource", model.PdStoreSalePrice.PriceSource)
                    .Where("SourceSysNo", model.PdStoreSalePrice.SourceSysNo)
                    .Where("Status", model.PdStoreSalePrice.Status)
                    .Execute();
                }
                else
                {
                    int PdPriceSysNo = Context.Insert<PdPrice>("PdPrice", model.PdStoreSalePrice)
                                           .AutoMap(o => o.SysNo)
                                           .ExecuteReturnLastId<int>("SysNo");
                }
            }
        }
        #endregion
        /// <summary>
        /// 更新商品信息
        /// </summary>
        /// <param name="models">商品信息列表</param>
        /// <returns>空</returns>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        public override void UpdatePdProduct(List<PdProductList> models)
        {
            models.ForEach(model => Context.Update("PdProduct")
                //.Column("EasName", model.EasName)
                                           .Column("Status", model.Status)
                                           .Where("ErpCode", model.ErpCode).Execute());
        }
        #endregion

        #region 根据条件获取商品列表  未实现
        /// <summary>
        /// 根据条件获取商品列表 未实现
        /// </summary>
        /// <param name="filter">搜索条件</param>
        /// <returns>商品列表</returns>
        /// <remarks>2013-06-25 黄波 创建</remarks>
        public override IList<CBPdProduct> GetProducts(Model.Parameter.ParaProductFilter filter)
        {
            return new List<CBPdProduct>();
        }
        #endregion

        #region 商品选择组件产品查询
        /// <summary>
        /// 商品选择组件产品查询
        /// </summary>
        /// <param name="pager">分页查询参数对象</param>
        /// <returns></returns>
        /// <remarks>2013-07-11 邵斌 创建</remarks>
        /// <remarks>2013-12-03 邵斌 扩展：加入条件SyncWebFront 只显示前台同步能看到的商品</remarks>
        /// <remarks>2014-06-05 余勇 将记录数查询条件与列表查询条件改成一致</remarks>
        public override void ProductSelectorProductSearch(ref Pager<ParaProductSearchFilter> pager)
        {

            using (var _context = Context.UseSharedConnection(true))
            {
                string sqlFrom = @" PdProduct p left join (select productsysno,categorysysno from PdCategoryAssociation where IsMaster=@IsMaster) ca on ca.productsysno = p.sysno 
                                    left join  PdCategory c on ca.categorysysno = c.sysno
                                    left join pdprice pp on pp.productsysno = p.sysno and pp.pricesource=@pricesource";

                var sqlWhere = " 1=1 ";

                sqlWhere += " and p.Status=@Status and pp.status = @status1 ";

                if (!string.IsNullOrEmpty(pager.PageFilter.ProductName) && pager.PageFilter.ProductName != "")
                    sqlWhere += " and (charindex(lower(@ProductName),lower(p.EasName)) > 0 or charindex(lower(@ErpCode),lower(p.ErpCode)) > 0 or charindex(lower(@ProductName),lower(p.Barcode)) > 0)  ";


                if (pager.PageFilter.ProductCategorySysNo > 0)
                    sqlWhere += " and c.SysNos like '%," + pager.PageFilter.ProductCategorySysNo + ",%' ";

                if (pager.PageFilter.SelectStockProduct == true && pager.PageFilter.WarehouseSysNo > 0)
                {
                    sqlWhere += " and ps.WarehouseSysNo = @WarehouseSysNo  ";
                    sqlWhere += " and ps.StockQuantity > 0 ";
                    sqlFrom += "  inner join PdProductStock ps on p.SysNo = ps.PdProductSysNo";
                }

                pager.Rows = _context.Select<ParaProductSearchFilter>(" p.* ,c.sysno as ProductCategorySysNo,c.categoryname as ProductCategoryName,pp.price as BasicPrice")
                              .From(sqlFrom)
                                     .Where(sqlWhere)
                                     .Parameter("IsMaster", (int)ProductStatus.是否是主分类.是)
                                     .Parameter("pricesource", (int)ProductStatus.产品价格来源.基础价格)
                                     .Parameter("status", (int)ProductStatus.商品状态.上架)
                                     .Parameter("status1", (int)ProductStatus.产品价格状态.有效)
                                     .Parameter("ProductName", pager.PageFilter.ProductName)
                                     .Parameter("ErpCode", pager.PageFilter.ErpCode)
                                     .Parameter("WarehouseSysNo", pager.PageFilter.WarehouseSysNo)
                                     .OrderBy("p.LastUpdateDate desc,p.Createddate desc")
                                     .Paging(pager.CurrentPage, pager.PageSize)
                                     .QueryMany();
                pager.TotalRows = _context.Select<int>("count(1)")
                                    .From(sqlFrom)
                                     .Where(sqlWhere)
                                     .Parameter("IsMaster", (int)ProductStatus.是否是主分类.是)
                                     .Parameter("pricesource", (int)ProductStatus.产品价格来源.基础价格)
                                     .Parameter("status", (int)ProductStatus.商品状态.上架)
                                     .Parameter("status1", (int)ProductStatus.产品价格状态.有效)
                                     .Parameter("ProductName", pager.PageFilter.ProductName)
                                     .Parameter("WarehouseSysNo", pager.PageFilter.WarehouseSysNo)
                                     .Parameter("ErpCode", pager.PageFilter.ErpCode)
                                     .QuerySingle();
            }

        }
        /// <summary>
        /// 选择经销商对应的商品
        /// 2015-12-25 
        /// 王耀发 创建
        /// </summary>
        /// <param name="pager"></param>
        public override void DealerProductSearch(ref Pager<ParaProductSearchFilter> pager)
        {

            using (var _context = Context.UseSharedConnection(true))
            {
                string sqlFrom = @"DsSpecialPrice sp inner join 
                                    PdProduct p on sp.ProductSysNo = p.SysNo
                                    left join (select productsysno,categorysysno from PdCategoryAssociation where IsMaster=@IsMaster) ca on ca.productsysno = p.sysno 
                                    left join  PdCategory c on ca.categorysysno = c.sysno
                                    left join pdprice pp on pp.productsysno = p.sysno and pp.pricesource=@pricesource";


                if (pager.PageFilter.DealerSysNo == 0)
                {
                    sqlFrom = @"PdProduct p 
                                left join (select productsysno,categorysysno from PdCategoryAssociation where IsMaster=@IsMaster) ca on ca.productsysno = p.sysno 
                                left join  PdCategory c on ca.categorysysno = c.sysno
                                left join pdprice pp on pp.productsysno = p.sysno and pp.pricesource=@pricesource";
                }
              

              

                var sqlWhere = " 1=1 ";

                sqlWhere += " and p.Status=@Status and pp.status = @status1 ";
                if (pager.PageFilter.DealerSysNo > 0)
                {
                    sqlWhere += " and sp.DealerSysNo=@DealerSysNo  and sp.status = @status and pp.status = @status1 ";
                }

                if (!string.IsNullOrEmpty(pager.PageFilter.ProductName) && pager.PageFilter.ProductName != "")
                    sqlWhere += " and (charindex(lower(@ProductName),lower(p.EasName)) > 0 or charindex(lower(@ErpCode),lower(p.ErpCode)) > 0) ";


                if (pager.PageFilter.ProductCategorySysNo > 0)
                    sqlWhere += " and c.SysNos like '%," + pager.PageFilter.ProductCategorySysNo + ",%' ";

                if (pager.PageFilter.WarehouseSysNo > 0)
                {
                    sqlWhere += " and ps.WarehouseSysNo = @WarehouseSysNo  ";
                    sqlWhere += " and ps.StockQuantity > 0 ";
                    sqlFrom += "  inner join PdProductStock ps on p.SysNo = ps.PdProductSysNo";
                }


                pager.Rows = _context.Select<ParaProductSearchFilter>(" p.* ,c.sysno as ProductCategorySysNo,c.categoryname as ProductCategoryName,pp.Price as BasicPrice" + (pager.PageFilter.DealerSysNo > 0 ? ",sp.Price as Price" : ""))
                              .From(sqlFrom)
                                     .Where(sqlWhere)
                                     .Parameter("IsMaster", (int)ProductStatus.是否是主分类.是)
                                     .Parameter("pricesource", (int)ProductStatus.产品价格来源.基础价格)
                                     .Parameter("DealerSysNo", pager.PageFilter.DealerSysNo)
                                     .Parameter("status", (int)ProductStatus.商品状态.上架)
                                     .Parameter("status1", (int)ProductStatus.产品价格状态.有效)
                                     .Parameter("ProductName", pager.PageFilter.ProductName)
                                     .Parameter("ErpCode", pager.PageFilter.ErpCode)
                                     .Parameter("WarehouseSysNo", pager.PageFilter.WarehouseSysNo)
                                     .OrderBy("p.LastUpdateDate desc,p.Createddate desc")
                                     .Paging(pager.CurrentPage, pager.PageSize)
                                     .QueryMany();
                pager.TotalRows = _context.Select<int>("count(1)")
                                    .From(sqlFrom)
                                     .Where(sqlWhere)
                                     .Parameter("IsMaster", (int)ProductStatus.是否是主分类.是)
                                     .Parameter("pricesource", (int)ProductStatus.产品价格来源.基础价格)
                                     .Parameter("DealerSysNo", pager.PageFilter.DealerSysNo)
                                     .Parameter("status", (int)ProductStatus.商品状态.上架)
                                     .Parameter("status1", (int)ProductStatus.产品价格状态.有效)
                                     .Parameter("ProductName", pager.PageFilter.ProductName)
                                     .Parameter("ErpCode", pager.PageFilter.ErpCode)
                                     .Parameter("WarehouseSysNo", pager.PageFilter.WarehouseSysNo)
                                     .QuerySingle();
            }

        }

        #endregion

        #region 商品选择组件--定制查询方法

        /// <summary>
        /// 选择属性关联商品查询
        /// </summary>
        /// <param name="pager">查询条件：必须含有商品属性</param>
        /// <returns>返回可用的商品系统编号列表</returns>
        /// <remarks>2013-07-22 邵斌  实现功能</remarks>
        public override void SearchAttributeAssociationProduct(ref Pager<ParaProductSearchFilter> pager)
        {
            IList<int> result = null;

            //首先通过主商品编号查找到商品所以属性中可用作关联的商品属性，然后通过这些商品属性来关联查询商品
            using (var ShareDbContext = Context.UseSharedConnection(true))
            {

                #region 测试 SQL 通过商品编号查找商品能用做关联的属性

                /*
                select
                    pa.sysno
                from 
                    PdProductAttribute ppa
                    inner join PdAttribute pa on pa.sysno = ppa.attributesysno
                where 
                    ppa.productsysno=[商品系统编号]   --主商品系统编号
                    and pa.status=1             --商品属性要是可用属性
                    and pa.IsRelationFlag = 1   --可做关联的属性
                */

                #endregion

                result = ShareDbContext.Sql(@"
                    select
                     pa.*
                    from 
                        PdProductAttribute ppa
                        inner join PdAttribute pa on pa.sysno = ppa.attributesysno
                    where 
                        ppa.productsysno=@0
                        and pa.status=@1
                        and pa.IsRelationFlag =@2
                ", pager.PageFilter.SysNo, (int)ProductStatus.商品属性状态.启用, (int)ProductStatus.是否用做关联属性.是)
                                       .QueryMany<int>();

                //为避免查询出错，将设置一个空值（系统编号没有为0的数据）
                if (result.Count == 0)
                    result.Add(0);

                #region 测试 SQL 通过主商品的关联属性查询商品

                /*
                 * 通过对比关联属性个数来过来拿下商品是可以选择的。
                 select 
                    xx.sysno
                from 
                    (select
                        p.sysno, row_number() over(order by p.sysno) as rn 
                    from  
                        (select count(productsysno) as attributeCount,productsysno from PdProductAttribute where attributesysno in (182,183) and status = 1 group by productsysno ) ppa
                        inner join pdproduct p on p.sysno = ppa.productsysno
                    where 
                        attributeCount >= [关联属性个数]
                        and p.sysno <> [主商品编号]
                        order by p.sysno) xx 
                where 
                    xx.rn between [分页开始行] and [分页结束行]
                 * */
                #endregion

                //            result = ShareDbContext.Sql(@"
                //                select 
                //                    xx.sysno
                //                from 
                //                    (select
                //                        p.sysno, row_number() over(order by p.sysno) as rn 
                //                    from  
                //                        (select count(productsysno) as attributeCount,productsysno from PdProductAttribute where attributesysno in (" + result.AsDelimited(",") + @") and status = :paStatus group by productsysno ) ppa
                //                        inner join pdproduct p on p.sysno = ppa.productsysno
                //                    where 
                //                        attributeCount >= :count
                //                        and p.sysno <> :productsysno
                //                        order by p.sysno) xx 
                //                where 
                //                    xx.rn between :pageStart and :pageEnd
                //                ")
                //             .Parameter("paStatus", (int)ProductStatus.商品属性状态.启用)
                //             .Parameter("count", result.Count)
                //             .Parameter("productsysno", pager.PageFilter.SysNo)
                //             .Parameter("pageStart", (pager.CurrentPage - 1) * pager.PageSize)
                //             .Parameter("pageEnd", pager.CurrentPage * pager.PageSize)
                //             .QueryMany<int>();

                var sysNos = pager.PageFilter.ProductCategorySysNo == 0 ? " 1=1 " : " SysNos like '%," + pager.PageFilter.ProductCategorySysNo + ",%' ";

                string attributesysno = "";
                foreach (int sysNo in result)
                {
                    if (sysNo > 0)
                        attributesysno += sysNo.ToString()+",";
                }
                attributesysno = attributesysno.Trim(',');

                string attributeSql="";

                if (attributesysno != "")
                {
                     attributesysno = " attributesysno in (" + attributesysno + ")";
                     attributeSql=@" inner join(
                                        select
                                            p.sysno, row_number() over(order by p.sysno) as rn 
                                        from  
                                            (select count(productsysno) as attributeCount,productsysno from PdProductAttribute where " + attributesysno + @" and status = @paStatus group by productsysno ) ppa
                                            inner join pdproduct p on p.sysno = ppa.productsysno                                                                                    
                                        where                                             
                                            " + (string.IsNullOrWhiteSpace(pager.PageFilter.RelationCode) ? "1=1" : "NOT EXISTS(select productsysno from PdProductAssociation where relationcode is not null and p.sysno = productsysno group by productsysno)") + @"                                        
                                            and attributeCount > 0
                                            and p.sysno <> @productsysno
                                        
                                     ) filterTable on p.sysno = filterTable.sysno ";
                }
          
                    //attributesysno = " 1=1 ";
                

                pager.Rows = ShareDbContext.Select<ParaProductSearchFilter>("p.* ,c.sysno as ProductCategorySysNo,c.categoryname as ProductCategoryName,pp.price as Price")
                                  .From(@"pdproduct p "+attributeSql+" inner join PdCategoryAssociation pa on p.sysno = pa.productsysno "+
                                     " inner join (select * from pdcategory where Status=@Status2 and isonline=@isonline and " + sysNos + @") c on pa.categorysysno = c.sysno
                                     left join pdprice pp on pp.productsysno = p.sysno and pp.pricesource=0")
                                  .Where(@"
                                     (@ProductName is null or charindex(lower(@ProductName),lower(p.EasName)) > 0 or charindex(lower(@ErpCode),lower(p.ErpCode)) > 0)  and 
                                     p.CanFrontEndOrder = @CanFrontEndOrder 
                                     and pa.ismaster = 1 and p.status = @status
                                     and pp.status = @status1 
                              ")
                                  .Parameter("paStatus", (int)ProductStatus.商品属性状态.启用)
                    //.Parameter("count", result.Count)
                                  .Parameter("productsysno", pager.PageFilter.SysNo)
                                  .Parameter("Status2", (int)ProductStatus.商品分类状态.有效)
                                  .Parameter("isonline", (int)ProductStatus.是否前端展示.是)
                                  .Parameter("CategorySysNo", pager.PageFilter.ProductCategorySysNo)
                    //  .Parameter("CategorySysNo", pager.PageFilter.ProductCategorySysNo)
                                  .Parameter("ProductName", pager.PageFilter.ProductName)
                    //  .Parameter("ProductName", pager.PageFilter.ProductName)
                                  .Parameter("ErpCode", pager.PageFilter.ErpCode)
                                   .Parameter("CanFrontEndOrder", (int)ProductStatus.商品是否前台下单.是)
                                   .Parameter("status", (int)ProductStatus.商品状态.上架)
                                  .Parameter("status1", (int)ProductStatus.产品价格状态.有效)
                                  .OrderBy("p.LastUpdateDate desc")
                                  .Paging(pager.CurrentPage, pager.PageSize)
                                  .QueryMany();
               
                pager.TotalRows = ShareDbContext.Sql(@"select count(p.sysno)
                                     from 
                                     pdproduct p 
                                     "+attributeSql+@"
                                     inner join PdCategoryAssociation pa on p.sysno = pa.productsysno
                                     inner join (select * from pdcategory where  " + sysNos + @") c on pa.categorysysno = c.sysno
                                    where
                                     pa.ismaster = 1 and p.status = @status
                                     and (@ProductName is null or charindex(lower(@ProductName),lower(p.EasName)) > 0 or charindex(lower(@ErpCode),lower(p.ErpCode)) > 0)
                                    
                              ") // --
                              .Parameter("paStatus", (int)ProductStatus.商品属性状态.启用)
                              .Parameter("count", result.Count)
                              .Parameter("productsysno", pager.PageFilter.SysNo)
                              .Parameter("CategorySysNo", pager.PageFilter.ProductCategorySysNo)
                    //.Parameter("CategorySysNo", pager.PageFilter.ProductCategorySysNo)
                              .Parameter("status", (int)ProductStatus.商品状态.上架)
                              .Parameter("ProductName", pager.PageFilter.ProductName)
                    //.Parameter("ProductName", pager.PageFilter.ProductName)
                              .Parameter("ErpCode", pager.PageFilter.ErpCode)
                              .QuerySingle<int>();
            }
        }

        #endregion

        #region 获取已经选择的商品列表
        /// <summary>
        /// 获取已经选择的商品列表
        /// </summary>
        /// <param name="productList">商品列表</param>
        /// <returns>>返回 商品详细信息，包括所有价格</returns>
        /// <remarks>2013-07-11 邵斌  实现功能</remarks>
        public override IList<ParaProductSearchFilter> GetSelectedProductList(IList<int> productList)
        {
            return Context.Select<ParaProductSearchFilter>("p.* ,c.sysno as ProductCategorySysNo,c.categoryname as ProductCategoryName,pp.price as Price")
                              .From(@"
                                     pdproduct p 
                                     left join PdCategoryAssociation pa on p.sysno = pa.productsysno
                                     left join pdcategory c on pa.categorysysno = c.sysno
                                     left join pdprice pp on pp.productsysno = p.sysno and pp.pricesource=0")
                              .Where(@"
                                     pp.status=@status and pa.ismaster = 1 and " + (productList.Count > 0 ? " p.SysNo in (" + productList.Join(",") + ")" : "1=2") + @"
                              ")
                               .Parameter("status", (int)ProductStatus.产品价格状态.有效)
                              .OrderBy("p.sysno desc")
                              .QueryMany();
        }
        #endregion

        #region 获取已经选择商品的详细信息
        ///// <summary>
        ///// 获取已经选择商品的详细信息
        ///// </summary>
        ///// <param name="productList">商品列表</param>
        ///// <returns>返回 商品详细信息，包括所有价格</returns>
        ///// <remarks>2013-07-11 邵斌  实现功能</remarks>
        //public override IList<CBPdProduct> GetSelectedProductInfo(IList<int> productList)
        //{
        //    IList<CBPdProduct> list = new List<CBPdProduct>();

        //    for (int i = 0; i < productList.Count; i++)
        //        list.Add(GetProduct(productList[i]));
        //    return list;

        //}
        #endregion

        #region 获取商品详细信息列表
        /// <summary>
        /// 获取商品详细信息列表
        /// </summary>
        /// <param name="pager">商品详细信息查询列表</param>
        /// <param name="condition">动态条件，CBPdProductDetail里不包含的条件</param>
        /// <returns></returns>
        /// <remarks>2013-07-15 唐永勤 创建</remarks>
        public override void GetPdProductDetailList(ref Pager<CBPdProductDetail> pager, ParaProductFilter condition)
        {

            using (var _context = Context.UseSharedConnection(true))
            {

                var sqlWhere = " 1=1 ";
                //作废的商品不显示 2016-3-29 王耀发 创建
                sqlWhere += " and p.Status <> 2";

                if (condition.IsFrontDisplay >= 0)
                    sqlWhere += " and p.IsFrontDisplay=@IsFrontDisplay";

                if (condition.Status >= 0)
                    sqlWhere += " and p.Status=@Status";

                if (condition.SysNo > 0)
                    sqlWhere += " and p.Sysno=@Sysno";


                if (!string.IsNullOrEmpty(condition.ErpCode) && condition.ErpCode != "")
                    sqlWhere += " and p.ErpCode=@ErpCode";

                if (!string.IsNullOrEmpty(condition.Barcode) && condition.Barcode != "")
                    sqlWhere += " and p.Barcode=@Barcode";

                if (condition.ProductType > 0)
                    sqlWhere += " and p.ProductType=@ProductType";

                if (!string.IsNullOrEmpty(condition.ProductName) && condition.ProductName != "")
                    sqlWhere += " and (p.EasName like @name1 or  p.ErpCode = @name or p.Barcode like @name1)";

                if (condition.StartTime != null)
                    sqlWhere += " and p.LastUpdateDate >= @StartTime";

                if (condition.EndTime != null)
                    sqlWhere += " and p.LastUpdateDate <= @EndTime";

                if (condition.CreateStartTime != null)
                    sqlWhere += " and p.CreatedDate >= @CreateStartTime";

                if (condition.CreateEndTime != null)
                    sqlWhere += " and  p.CreatedDate <= @CreateEndTime";

                if (condition.ProductCategorySysno > 0)
                    sqlWhere += " and SysNos like '%," + condition.ProductCategorySysno + ",%' ";

                //判断是否绑定所有分销商 王耀发 2016-3-7 创建
                if (!condition.IsBindAllDealer)
                {
                    //判断是否绑定分销商
                    if (condition.IsBindDealer)
                    {
                        sqlWhere += " and d.SysNo = " + condition.DealerSysNo;
                    }
                    else
                    {
                        sqlWhere += " and d.CreatedBy = " + condition.DealerCreatedBy;
                    }
                }
                if (condition.SelectedAgentSysNo != -1)
                {
                    if (condition.SelectedDealerSysNo != -1)
                    {
                        sqlWhere += " and d.SysNo = " + condition.SelectedDealerSysNo;
                    }
                    else
                    {
                        sqlWhere += " and d.CreatedBy = " + condition.SelectedAgentSysNo;
                    }
                }
                if (condition.OriginSysNo > 0)
                {
                    sqlWhere += " and  p.OriginSysNo = @OriginSysNo";
                }
                var sqlFrom = "";
                if (condition.IsSelectedIsMaster == 1)//用于微格良品
                {
                    sqlFrom = @"PdProduct p 
                                    left join (select productsysno,categorysysno from PdCategoryAssociation) ca on ca.productsysno = p.sysno 
                                    left join  PdCategory c on ca.categorysysno = c.sysno
                                    left join  PdProductStatistics ps on ps.ProductSysNo = p.sysno
                                    left join  (select * from PdPrice where PriceSource =@PriceSource and SourceSysNo =@SourceSysNo and [Status] = @PriStatus) price  on price.productsysno = p.sysno
                                    left join  (select * from PdPrice where PriceSource =@PriceSource1 and SourceSysNo =@SourceSysNo1 and [Status] = @PriStatus1) price1  on price1.productsysno = p.sysno
                                    left join  (select * from PdPrice where PriceSource =@PriceSource2 and SourceSysNo =@SourceSysNo2 and [Status] = @PriStatus2) price2  on price2.productsysno = p.sysno
                                    left join DsDealer d on p.DealerSysNo = d.SysNo
                                    left join PdBrand as brand on brand.sysNo=p.BrandSysNo
                                    left join Origin on  Origin.sysNo=p.OriginSysNo";
                }
                else
                {
                    sqlFrom = @"PdProduct p 
                                    left join (select productsysno,categorysysno from PdCategoryAssociation where IsMaster=@IsMaster) ca on ca.productsysno = p.sysno 
                                    left join  PdCategory c on ca.categorysysno = c.sysno
                                    left join  PdProductStatistics ps on ps.ProductSysNo = p.sysno
                                    left join  (select * from PdPrice where PriceSource =@PriceSource and SourceSysNo =@SourceSysNo and [Status] = @PriStatus) price  on price.productsysno = p.sysno
                                    left join  (select * from PdPrice where PriceSource =@PriceSource1 and SourceSysNo =@SourceSysNo1 and [Status] = @PriStatus1) price1  on price1.productsysno = p.sysno
                                    left join  (select * from PdPrice where PriceSource =@PriceSource2 and SourceSysNo =@SourceSysNo2 and [Status] = @PriStatus2) price2  on price2.productsysno = p.sysno
                                    left join DsDealer d on p.DealerSysNo = d.SysNo
                                    left join PdBrand as brand on brand.sysNo=p.BrandSysNo
                                    left join Origin on  Origin.sysNo=p.OriginSysNo";
                }
                pager.Rows = _context.Select<CBPdProductDetail>(" p.*, c.categoryname as ProductCategoryName, c.sysno as ProductCategorySysno,c.SysNos as SysNos,price.Price as BasicPrice, price1.Price as SalesPrice,  price2.Price as spShopPrice, d.DealerName,ps.Sales as ProductSalesNum,brand.Name as BrandName,Origin.Origin_Name as OriginName ")
                              .From(sqlFrom)
                              .Where(sqlWhere)

                              .Parameter("IsMaster", (int)ProductStatus.是否是主分类.是)
                              .Parameter("PriceSource", (int)ProductStatus.产品价格来源.基础价格)
                              .Parameter("PriceSource1", (int)ProductStatus.产品价格来源.会员等级价)
                              .Parameter("SourceSysNo", 0)  //基础价
                              .Parameter("PriStatus", 1)  //商品价格有效
                              .Parameter("SourceSysNo1", 1) //会员价
                              .Parameter("PriStatus1", 1)  //商品价格有效

                              .Parameter("PriceSource2", (int)ProductStatus.产品价格来源.线下门店价)
                              .Parameter("SourceSysNo2", 0) //会员价
                              .Parameter("PriStatus2", 1)  //商品价格有效

                              .Parameter("IsFrontDisplay", condition.IsFrontDisplay)

                              .Parameter("Status", condition.Status)

                              .Parameter("name", condition.ProductName)
                              .Parameter("name1", "%" + condition.ProductName + "%")

                              .Parameter("Sysno", condition.SysNo)

                              .Parameter("ErpCode", condition.ErpCode)

                              .Parameter("Barcode", condition.Barcode)

                              .Parameter("ProductType", condition.ProductType)

                              .Parameter("StartTime", condition.StartTime)
                              .Parameter("EndTime", (condition.EndTime == null) ? condition.EndTime : ((DateTime)condition.EndTime).AddDays(1))

                              .Parameter("CreateStartTime", condition.CreateStartTime)

                              .Parameter("CreateEndTime", (condition.CreateEndTime == null) ? condition.CreateEndTime : ((DateTime)condition.CreateEndTime).AddDays(1))

                              .Parameter("CanFrontEndOrder", condition.CanFrontEndOrder)
                              .Parameter("OriginSysNo", condition.OriginSysNo)

                              .OrderBy("p.LastUpdateDate desc, p.sysno desc ")
                              .Paging(pager.CurrentPage, pager.PageSize)
                              .QueryMany();

                pager.TotalRows = _context.Select<int>("count(1)")
                              .From(sqlFrom)
                              .Where(sqlWhere)

                              .Parameter("IsMaster", (int)ProductStatus.是否是主分类.是)
                              .Parameter("PriceSource", (int)ProductStatus.产品价格来源.基础价格)
                              .Parameter("PriceSource1", (int)ProductStatus.产品价格来源.会员等级价)
                              .Parameter("SourceSysNo", 0)  //基础价
                              .Parameter("PriStatus", 1)  //商品价格有效
                              .Parameter("SourceSysNo1", 1) //会员价
                              .Parameter("PriStatus1", 1)  //商品价格有效

                              .Parameter("PriceSource2", (int)ProductStatus.产品价格来源.线下门店价)
                              .Parameter("SourceSysNo2", 0) //会员价
                              .Parameter("PriStatus2", 1)  //商品价格有效

                              .Parameter("IsFrontDisplay", condition.IsFrontDisplay)

                              .Parameter("Status", condition.Status)

                              .Parameter("name", condition.ProductName)
                              .Parameter("name1", "%" + condition.ProductName + "%")

                              .Parameter("Sysno", condition.SysNo)

                              .Parameter("ErpCode", condition.ErpCode)

                              .Parameter("Barcode", condition.Barcode)

                              .Parameter("ProductType", condition.ProductType)

                              .Parameter("StartTime", condition.StartTime)

                              .Parameter("EndTime", (condition.EndTime == null) ? condition.EndTime : ((DateTime)condition.EndTime).AddDays(1))

                              .Parameter("CreateStartTime", condition.CreateStartTime)

                              .Parameter("CreateEndTime", (condition.CreateEndTime == null) ? condition.CreateEndTime : ((DateTime)condition.CreateEndTime).AddDays(1))

                              .Parameter("CanFrontEndOrder", condition.CanFrontEndOrder)
                              .Parameter("OriginSysNo", condition.OriginSysNo)
                              .QuerySingle();
            }
        }

        /// <summary>
        /// 获取商品详细信息列表
        /// </summary>
        /// <param name="pager">商品详细信息查询列表</param>
        /// <param name="condition">动态条件，CBPdProductDetail里不包含的条件</param>
        /// <returns></returns>
        /// <remarks>2013-07-15 唐永勤 创建</remarks>
        public override Pager<PdProduct> GetPdProductList(Pager<PdProduct> pager)
        {
            #region sql条件

            //string sql = @" 1=1 (@Status=-1 or Status =@Status) and ((@ErpCode is null or ErpCode like @ErpCode1) or (@EasName is null or EasName like @EasName1))";
            string sql = @" 1=1 ";
            if (pager.PageFilter.Status != -10 && pager.PageFilter.Status > 0)
            {
                sql += " and Status =@Status";
            }
            if (!string.IsNullOrEmpty(pager.PageFilter.EasName))
            {
                sql += " and EasName like @EasName";
            }
            if (!string.IsNullOrEmpty(pager.PageFilter.ErpCode))
            {
                sql += " and ErpCode like @ErpCode";
            }
            #endregion

            using (var _context = Context.UseSharedConnection(true))
            {

                pager.Rows = _context.Select<PdProduct>("p.*")
                              .From("PdProduct p")
                              .Where(sql)
                              .Parameter("Status", pager.PageFilter.Status)
                              .Parameter("ErpCode", pager.PageFilter.ErpCode)
                              .Parameter("ErpCode1", "%" + pager.PageFilter.ErpCode + "%")
                              .Parameter("EasName", pager.PageFilter.EasName)
                              .Parameter("EasName1", "%" + pager.PageFilter.EasName + "%")
                              .OrderBy(" LastUpdateDate desc ")
                              .Paging(pager.CurrentPage, pager.PageSize)
                              .QueryMany();

                pager.TotalRows = _context.Select<int>("count(1)")
                              .From("PdProduct")
                              .Where(sql)
                              .Parameter("Status", pager.PageFilter.Status)
                              .Parameter("ErpCode", pager.PageFilter.ErpCode)
                              .Parameter("ErpCode1", "%" + pager.PageFilter.ErpCode + "%")
                              .Parameter("EasName", pager.PageFilter.EasName)
                              .Parameter("EasName1", "%" + pager.PageFilter.EasName + "%")
                              .QuerySingle();
            }
            return pager;
        }

        /// <summary>
        /// 获取商品详细信息列表
        /// </summary>
        /// <param name="pager">商品详细信息查询列表</param>
        /// <param name="condition">动态条件，CBPdProductDetail里不包含的条件</param>
        /// <returns></returns>
        /// <remarks>2013-07-15 唐永勤 创建</remarks>
        public override Pager<CBPdProduct> GetCBPdProductList(Pager<CBPdProduct> pager)
        {
            #region sql条件
            string sql = " 1=1 ";
            //没有商品组做条件
            if (pager.PageFilter.GroupSysNoList == null)
            {
                if (pager.PageFilter.Status >= 0)
                {
                    sql += " and p.Status =@Status";
                }
                if (!string.IsNullOrWhiteSpace(pager.PageFilter.ErpCode))
                {
                    sql += " and (charindex(@ErpCode,p.ErpCode)>0 or charindex(@EasName,p.EasName)>0)";
                }
               // sql = @" (@Status=-1 or p.Status =@Status) and ((@ErpCode is null or p.ErpCode like @ErpCode1) or (@EasName is null or p.EasName like @EasName1))";
            }
            else
            {
                sql = @" and (@Status=-1 or p.Status =@Status) and ((@ErpCode is null or p.ErpCode like @ErpCode1) or (@EasName is null or p.EasName like @EasName1)) and (@GroupSysNoList is null or f.GroupSysNo in (select col from [dbo].[splitstr](@GroupSysNoList1,',')))";
            }
            #endregion

            using (var _context = Context.UseSharedConnection(true))
            {
                //没有商品组做条件
                if (pager.PageFilter.GroupSysNoList == null)
                {
                    pager.Rows = _context.Select<CBPdProduct>("p.*")
                                  .From("PdProduct p ")
                                  .Where(sql)
                                  .Parameter("Status", pager.PageFilter.Status)
                                  .Parameter("ErpCode", pager.PageFilter.ErpCode)
                                  .Parameter("ErpCode1", "%" + pager.PageFilter.ErpCode + "%")
                                  .Parameter("EasName", pager.PageFilter.EasName)
                                  .Parameter("EasName1", "%" + pager.PageFilter.EasName + "%")
                                  .OrderBy(" p.LastUpdateDate desc ")
                                  .Paging(pager.CurrentPage, pager.PageSize)
                                  .QueryMany();

                    pager.TotalRows = _context.Select<int>("count(1)")
                                  .From("PdProduct p ")
                                  .Where(sql)
                                  .Parameter("Status", pager.PageFilter.Status)
                                  .Parameter("ErpCode", pager.PageFilter.ErpCode)
                                  .Parameter("ErpCode1", "%" + pager.PageFilter.ErpCode + "%")
                                  .Parameter("EasName", pager.PageFilter.EasName)
                                  .Parameter("EasName1", "%" + pager.PageFilter.EasName + "%")
                                  .QuerySingle();
                }
                else
                {
                    pager.Rows = _context.Select<CBPdProduct>("p.*,g.Name as GroupName")
                                  .From("PdProduct p left join FeProductItem f on f.ProductSysNo = p.SysNo left join FeProductGroup g on f.GroupSysNo = g.SysNo")
                                  .Where(sql)
                                  .Parameter("Status", pager.PageFilter.Status)
                                  .Parameter("ErpCode", pager.PageFilter.ErpCode)
                                  .Parameter("ErpCode1", "%" + pager.PageFilter.ErpCode + "%")
                                  .Parameter("EasName", pager.PageFilter.EasName)
                                  .Parameter("EasName1", "%" + pager.PageFilter.EasName + "%")
                                  .Parameter("GroupSysNoList", pager.PageFilter.GroupSysNoList)
                                  .Parameter("GroupSysNoList1", pager.PageFilter.GroupSysNoList)
                                  .OrderBy(" p.LastUpdateDate desc ")
                                  .Paging(pager.CurrentPage, pager.PageSize)
                                  .QueryMany();

                    pager.TotalRows = _context.Select<int>("count(1)")
                                  .From("PdProduct p left join FeProductItem f on f.ProductSysNo = p.SysNo left join FeProductGroup g on f.GroupSysNo = g.SysNo")
                                  .Where(sql)
                                  .Parameter("Status", pager.PageFilter.Status)
                                  .Parameter("ErpCode", pager.PageFilter.ErpCode)
                                  .Parameter("ErpCode1", "%" + pager.PageFilter.ErpCode + "%")
                                  .Parameter("EasName", pager.PageFilter.EasName)
                                  .Parameter("EasName1", "%" + pager.PageFilter.EasName + "%")
                                  .Parameter("GroupSysNoList", pager.PageFilter.GroupSysNoList)
                                  .Parameter("GroupSysNoList1", pager.PageFilter.GroupSysNoList)
                                  .QuerySingle();
                }
            }
            return pager;
        }


        /// <summary>
        /// 获取分销商可添加的商品详细信息列表
        /// </summary>
        /// <param name="pager">商品详细信息查询列表</param>
        /// <param name="dealerMallSysNo">分销商商城系统编号</param>
        /// <returns></returns>
        /// <remarks>2015-12-10 王耀发 创建</remarks>
        public override Pager<CBPdProduct> GetDealerMallProductList(Pager<CBPdProduct> pager, int dealerMallSysNo)
        {
            #region sql条件
            string sql = "";
            //没有商品组做条件
            if (pager.PageFilter.GroupSysNoList == null)
            {
                sql = @"(@Status=-1 or p.Status =@Status) and ((@ErpCode is null or p.ErpCode like @ErpCode1) or (@EasName is null or p.EasName like @EasName1)) and p.SysNo not in (select HytProductSysNo from DsProductAssociation where DealerMallSysNo = @DealerMallSysNo)";
            }
            else
            {
                sql = @"(@Status=-1 or p.Status =@Status) and ((@ErpCode is null or p.ErpCode like @ErpCode1) or (@EasName is null or p.EasName like @EasName1)) and (@GroupSysNoList is null or f.GroupSysNo in (select col from [dbo].[splitstr](@GroupSysNoList1,','))) and p.SysNo not in (select HytProductSysNo from DsProductAssociation where DealerMallSysNo = @DealerMallSysNo)";
            }
            #endregion

            using (var _context = Context.UseSharedConnection(true))
            {
                //没有商品组做条件
                if (pager.PageFilter.GroupSysNoList == null)
                {
                    pager.Rows = _context.Select<CBPdProduct>("p.*")
                                  .From("PdProduct p ")
                                  .Where(sql)
                                  .Parameter("Status", pager.PageFilter.Status)
                                  .Parameter("ErpCode", pager.PageFilter.ErpCode)
                                  .Parameter("ErpCode1", "%" + pager.PageFilter.ErpCode + "%")
                                  .Parameter("EasName", pager.PageFilter.EasName)
                                  .Parameter("EasName1", "%" + pager.PageFilter.EasName + "%")
                                  .Parameter("DealerMallSysNo", dealerMallSysNo)
                                  .OrderBy(" p.LastUpdateDate desc ")
                                  .Paging(pager.CurrentPage, pager.PageSize)
                                  .QueryMany();

                    pager.TotalRows = _context.Select<int>("count(1)")
                                  .From("PdProduct p ")
                                  .Where(sql)
                                  .Parameter("Status", pager.PageFilter.Status)
                                  .Parameter("ErpCode", pager.PageFilter.ErpCode)
                                  .Parameter("ErpCode1", "%" + pager.PageFilter.ErpCode + "%")
                                  .Parameter("EasName", pager.PageFilter.EasName)
                                  .Parameter("EasName1", "%" + pager.PageFilter.EasName + "%")
                                  .Parameter("DealerMallSysNo", dealerMallSysNo)
                                  .QuerySingle();
                }
                else
                {
                    pager.Rows = _context.Select<CBPdProduct>("p.*,g.Name as GroupName")
                                  .From("PdProduct p left join FeProductItem f on f.ProductSysNo = p.SysNo left join FeProductGroup g on f.GroupSysNo = g.SysNo")
                                  .Where(sql)
                                  .Parameter("Status", pager.PageFilter.Status)
                                  .Parameter("ErpCode", pager.PageFilter.ErpCode)
                                  .Parameter("ErpCode1", "%" + pager.PageFilter.ErpCode + "%")
                                  .Parameter("EasName", pager.PageFilter.EasName)
                                  .Parameter("EasName1", "%" + pager.PageFilter.EasName + "%")
                                  .Parameter("GroupSysNoList", pager.PageFilter.GroupSysNoList)
                                  .Parameter("GroupSysNoList1", pager.PageFilter.GroupSysNoList)
                                  .Parameter("DealerMallSysNo", dealerMallSysNo)
                                  .OrderBy(" p.LastUpdateDate desc ")
                                  .Paging(pager.CurrentPage, pager.PageSize)
                                  .QueryMany();

                    pager.TotalRows = _context.Select<int>("count(1)")
                                  .From("PdProduct p left join FeProductItem f on f.ProductSysNo = p.SysNo left join FeProductGroup g on f.GroupSysNo = g.SysNo")
                                  .Where(sql)
                                  .Parameter("Status", pager.PageFilter.Status)
                                  .Parameter("ErpCode", pager.PageFilter.ErpCode)
                                  .Parameter("ErpCode1", "%" + pager.PageFilter.ErpCode + "%")
                                  .Parameter("EasName", pager.PageFilter.EasName)
                                  .Parameter("EasName1", "%" + pager.PageFilter.EasName + "%")
                                  .Parameter("GroupSysNoList", pager.PageFilter.GroupSysNoList)
                                  .Parameter("GroupSysNoList1", pager.PageFilter.GroupSysNoList)
                                  .Parameter("DealerMallSysNo", dealerMallSysNo)
                                  .QuerySingle();
                }
            }
            return pager;
        }

        #endregion

        #region 更新商品状态
        /// <summary>
        /// 更新商品状态
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="sysNo">商品编号</param>
        /// <returns>更新行数</returns>
        /// <remarks>
        /// 2013-07-16 唐永勤 创建
        /// 2016-07-18 陈海裕 创建
        /// </remarks>
        public override Result UpdateStatus(int status, int sysNo)
        {
            Result result = new Result();
            using (var _context = Context.UseSharedConnection(true))
            {
                if (status == (int)Hyt.Model.WorkflowStatus.ProductStatus.商品状态.上架)
                {
                    //价格必须要有一个通过审核
                    int priceCount = _context.Select<int>("count(1)")
                                             .From("PdPrice")
                                             .Where("productsysno = @sysno and status = @status")
                                             .Parameter("sysno", sysNo)
                                             .Parameter("status", (int)Hyt.Model.WorkflowStatus.ProductStatus.产品价格状态.有效)
                                             .QuerySingle();
                    if (priceCount < 1)
                    {
                        result.Status = false;
                        result.Message = "商品未设置价格，不能上架";
                        result.StatusCode = -1;
                        return result;
                    }

                    //检测商品描述 2016-6-15 杨浩 注释上架时检测描述是否填写
                    //PdProduct entity = _context.Select<PdProduct>("*")
                    //                           .From("PdProduct")
                    //                           .Where("sysno = @sysno")
                    //                           .Parameter("sysno", sysNo)
                    //                           .QuerySingle();
                    //if (string.IsNullOrEmpty(entity.ProductDesc))
                    //{
                    //    result.Status = false;
                    //    result.Message = "商品描述未填写，不能上架";
                    //    result.StatusCode = -1;
                    //    return result;
                    //}
                    ////判断商品是否有封面图片
                    //int imgCount = _context.Select<int>("count(1)")
                    //                       .From("PdProductImage")
                    //                       .Where("productsysno = @sysno and Status = @status")
                    //                       .Parameter("sysno", sysNo)
                    //                       .Parameter("status", (int)Hyt.Model.WorkflowStatus.ProductStatus.商品图片状态.显示)//显示状态 代表封面
                    //                       .QuerySingle();

                    //if (imgCount < 1)
                    //{
                    //    result.Status = false;
                    //    result.Message = "商品设置封面图片，不能上架";
                    //    result.StatusCode = -1;
                    //    return result;
                    //}

                    //判断商品主分类是否被禁用
                    PdCategory category = _context.Select<PdCategory>("*")
                                                  .From("PdCategory")
                                                  .Where("sysno = (select categorysysno from PdCategoryAssociation where productsysno = @sysno and IsMaster = @isMaster)")
                                                  .Parameter("sysno", sysNo)
                                                  .Parameter("isMaster", (int)Hyt.Model.WorkflowStatus.ProductStatus.是否是主分类.是)
                                                  .QuerySingle();
                    if (category != null && category.Status == (int)Hyt.Model.WorkflowStatus.ProductStatus.商品分类状态.无效)
                    {
                        result.Status = false;
                        result.Message = "商品主分类已禁用，不能上架";
                        result.StatusCode = -1;
                        return result;
                    }

                }
                ///更变商品状态的时候同时修改更新时间  2015-06-17 杨云奕 添加
                string Sql = string.Format("update PdProduct set Status = {0},LastUpdateDate='{2}',Stamp='{2}' where SysNo = {1}", status, sysNo, DateTime.Now);
                // 作废商品时修改商品erpcode，防止erpcode被占用
                if (status == 2)
                {
                    Sql = string.Format("update PdProduct set Status={0},ErpCode='del-'+ErpCode,Barcode='del-'+Barcode,LastUpdateDate='{2}',Stamp='{2}' where SysNo = {1}", status, sysNo, DateTime.Now);
                }
                int rowsAffected = Context.Sql(Sql).Execute();
                if (rowsAffected > 0)
                {
                    result.Status = true;
                    result.Message = "更新成功";
                    result.StatusCode = 1;
                }
                else
                {
                    result.Status = false;
                    result.Message = "更新上平状态失败";
                    result.StatusCode = -1;
                    return result;
                }
                return result;

            }

        }
        #endregion

        #region 检查ERP编号是否重复
        /// <summary>
        /// 检查ERP编号是否重复
        /// </summary>
        /// <param name="erpCode">ERP编号</param>
        /// <param name="sourceProductSysNo">原商品系统编号：新建商品就默认为0</param>
        /// <returns>返回 true:可以 false:不可以</returns>
        /// <remarks>2013-07-17 邵斌 创建</remarks>
        public override bool CheckERPCode(string erpCode, int sourceProductSysNo = 0)
        {
            //查询erp编号
            int result = Context.Sql("select count(erpcode) as countNum from pdproduct where erpcode = @erpcode and (@sourceProductSysNo = 0 or sysno <> @sourceProductSysNo)")
                                .Parameter("erpcode", erpCode)
                                .Parameter("sourceProductSysNo", sourceProductSysNo)
                //.Parameter("sourceProductSysNo", sourceProductSysNo)
                                .QuerySingle<int>();

            //判断结果，如果sourceProductSysNo为0表示是新增商品
            return result == 0;
        }
        #endregion

        #region 检查二维码编号是否重复
        /// <summary>
        /// 检查二维码编号是否重复
        /// </summary>
        /// <param name="qrCode">二维码编号</param>
        /// <param name="sourceProductSysNo">原商品系统编号：新建商品就默认为0</param>
        /// <returns>返回 true:可以 false:不可以</returns>
        /// <remarks>2013-07-17 邵斌 创建</remarks>
        public override bool CheckQRCode(string qrCode, int sourceProductSysNo = 0)
        {
            //查询二维码编号
            int result = Context.Sql("select count(qrCode) as countNum from pdproduct where qrCode = @qrCode and (@sourceProductSysNo = 0 or sysno <> @sourceProductSysNo)")
                                .Parameter("qrCode", qrCode)
                                .Parameter("sourceProductSysNo", sourceProductSysNo)
                //.Parameter("sourceProductSysNo", sourceProductSysNo)
                                .QuerySingle<int>();

            //判断结果，如果sourceProductSysNo为0表示是新增商品
            return result == 0;
        }
        #endregion

        #region 检查条行码是否重复
        /// <summary>
        /// 检查条行码是否重复
        /// </summary>
        /// <param name="barcode">条行码</param>
        /// <param name="sourceProductSysNo">原商品系统编号：新建商品就默认为0</param>
        /// <returns>返回 true:可以 false:不可以</returns>
        /// <remarks>2013-07-17 邵斌 创建</remarks>
        public override bool CheckBarCode(string barcode, int sourceProductSysNo = 0)
        {
            //查询条行码编号
            int result = Context.Sql("select count(barcode) as countNum from pdproduct where barcode = @barcode and (@sourceProductSysNo = 0 or sysno <> @sourceProductSysNo)")
                                .Parameter("barcode", barcode)
                                .Parameter("sourceProductSysNo", sourceProductSysNo)
                //.Parameter("sourceProductSysNo", sourceProductSysNo)
                                .QuerySingle<int>();

            //判断结果，如果sourceProductSysNo为0表示是新增商品
            return result == 0;
        }
        #endregion

        #region 更新商品描述文档
        /// <summary>
        /// 更新商品描述文档
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <param name="html">商品描述</param>
        /// <param name="phoneHtml">商品手机版描述</param>
        /// <returns>返回 true：成功 false：失败</returns>
        /// <remarks>2013-07-25 邵斌 创建</remarks>
        public override bool UpdateProductDescription(int productSysNo, string html, string phoneHtml = "")
        {
            int result = Context.Sql("update PdProduct set ProductDesc = @0,ProductPhoneDesc=@1 where sysno = @2 ", html, phoneHtml,
                                     productSysNo).Execute();
            return result > 0;
        }

        /// <summary>
        /// 更新商品描述文档
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <param name="html">商品描述</param>
        /// <param name="phoneHtml">商品手机版描述</param>
        /// <returns>返回 true：成功 false：失败</returns>
        /// <remarks>2017-10-12 罗勤瑶 创建</remarks>
        public override bool UpdateB2BProductDescription(int productSysNo, string html, string phoneHtml = "")
        {
            int result = ContextB2B.Sql("update PdProduct set ProductDesc = @0,ProductPhoneDesc=@1 where sysno = @2 ", html, phoneHtml,
                                     productSysNo).Execute();
            return result > 0;
        }
        #endregion

        #region 获取ErpCode
        /// <summary>
        /// 获取ErpCode
        /// </summary>
        /// <param name="productSysNo">产品编号</param>
        /// <returns>产品ERP编号</returns>
        /// <remarks>2013-07-26 朱成果 创建</remarks>
        public override string GetProductErpCode(int productSysNo)
        {
            return Context.Sql("select top 1 ErpCode from PdProduct where SysNo=@SysNo").Parameter("SysNo", productSysNo).QuerySingle<string>();
        }
        #endregion
        #region 获取Barcode
        /// <summary>
        /// 获取条形码Barcode
        /// </summary>
        /// <param name="productSysNo">产品编号</param>
        /// <returns>Barcode</returns>
        /// <remarks>2016-09-27 罗远康 创建</remarks>
        public override string GetProductBarcode(int productSysNo)
        {
            return Context.Sql("select Barcode from PdProduct where SysNo=@SysNo").Parameter("SysNo", productSysNo).QuerySingle<string>();
        }
        #endregion

        #region 获取EasName
        /// <summary>
        /// 获取EASName
        /// </summary>
        /// <param name="productSysNo">产品编号</param>
        /// <returns>EasName</returns>
        /// <remarks>2013-12-16 何永东 创建</remarks>
        public override string GetProductEasName(int productSysNo)
        {
            return Context.Sql("select EasName from PdProduct where SysNo=@SysNo").Parameter("SysNo", productSysNo).QuerySingle<string>();
        }
        #endregion

        #region 通过分类系统编号查询商品基础信息和某个会员等级价格
        /// <summary>
        /// 通过分类系统编号查询商品基础信息和某个会员等级价格
        /// </summary>
        /// <param name="categorySysNo">分类系统编号</param>
        /// <param name="customerLevelSysNo">会员等级编号</param>
        /// <param name="keyword">查询关键字(ERP商品编号,商品名称)</param>
        /// <param name="currentPageIndex">当前索引</param>
        /// <param name="pageSize">每页显示数</param>
        /// <returns>返回商品信息和指定的会员等级价格</returns>
        /// <remarks>2013-07-30 邵斌 创建</remarks>
        /// <remarks>2013-08-01 周唐炬 加入分页 关键字查询</remarks>
        public override Pager<CBProductListItem> GetProductListAndPartPrice(int categorySysNo, int customerLevelSysNo, string keyword, int currentPageIndex, int pageSize)
        {
            #region 测试SQL

            /*
             select
                 p.*,pc1.categoryname as CategoryName,pr.price as Price,pr1.price as SingleCustomerLevelPrice
                from 
                 pdcategory pc
                 inner join pdcategory pc1 on charindex(pc1.code,pc.code)=0 
                 inner join PdCategoryAssociation pca on pca.categorysysno = pc1.sysno
                 inner join pdproduct p on p.sysno = pca.productsysno
                 inner join pdprice pr on pr.productsysno = p.sysno 
                 inner join pdprice pr1 on pr1.productsysno = p.sysno
                where 
                  pc.sysno=1 and pr.pricesource = 0 and (pr1.pricesource = 10 and pr1.sourcesysno = 1)
             * */

            #endregion

            const string sql = @"(
                                SELECT 
                                  p.sysno,p.productname,p.ProductImage,pc1.categoryname as CategoryName,pr.price as Price,pr1.price as SingleCustomerLevelPrice,ROUND(dbms_random.value(0, 5)) as ProductCommentScore,ROUND(dbms_random.value(0, 100)) as CommentTimesCount
                                FROM 
                                  pdcategory pc
                                  INNER JOIN pdcategory pc1 ON charindex(pc1.code, pc.code) = 0
                                  INNER JOIN PdCategoryAssociation pca ON pca.categorysysno = pc1.sysno
                                  INNER JOIN pdproduct p ON p.sysno = pca.productsysno AND p.status = @0 AND (@1 is null OR REGEXP_charindex(p.ProductName, @1, 1, 1, 1, 'i') > 0 OR REGEXP_charindex(p.ErpCode, @1, 1, 1, 1, 'i') > 0)
                                  INNER JOIN pdprice pr  ON pr.productsysno = p.sysno AND pr.status = @2 AND pr.pricesource = @3 INNER JOIN pdprice pr1 ON pr1.productsysno = p.sysno AND pr1.status = @2 AND pr1.pricesource = @4 AND (isnull(pr1.sourcesysno,0) = 0 or pr1.sourcesysno = @5)
                                WHERE 
                                  (@5 = 0 OR pc.sysno = @6)
                                GROUP BY p.sysno,p.productname,p.ProductImage,pc1.categoryname,pr.price,pr1.price
                                ) tb";

            var dataList = Context.Select<CBProductListItem>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(1)").From(sql);
            var paras = new object[]
                {
                    (int)ProductStatus.商品状态.上架,
                    keyword,
                    (int)ProductStatus.产品价格状态.有效,
                    (int)ProductStatus.产品价格来源.基础价格,
                    (int)ProductStatus.产品价格来源.会员等级价,
                    customerLevelSysNo,
                    categorySysNo
                };

            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<CBProductListItem>()
            {
                Rows = dataList.OrderBy(@"tb.SysNo desc").Paging(currentPageIndex, pageSize).QueryMany(),
                TotalRows = dataCount.QuerySingle()
            };
            return pager;
        }
        #endregion

        #region 获取全部商品信息(用于生成索引文件)
        /// <summary>
        /// 获取商品信息(用于生成索引文件)
        /// </summary>
        /// <param name="productSysNo">商品编号</param>
        /// <returns>商品实体</returns>
        /// <remarks>2013-08-02 黄波 创建</remarks>
        /// <remarks>
        /// 2013-12-23 邵斌 修改：取消过滤商品状态过滤，让所有状态商品都能生成索引
        /// 2015-12-21 杨浩 添加分销商编号,解决商品搜索不能筛选分销商
        /// </remarks>
        public override IList<PdProductIndex> GetAllProduct(List<int> productSysNos)
        {
            //            var sql = @"
            //                       select bt.*,pdps.sales SalesCount,pdps.liking,pdps.favorites,pdps.comments CommentCount,pdps.averagescore,pdps.shares,pdps.question,pdps.totalscore
            //                        , pc.SysNos as AssociationCategory    --关联分类--
            //                        from 
            //                            (select
            //                                    SysNo,Barcode,ErpCode,QRCode,ProductName,ProductSubName,BrandSysNo,NameAcronymy,ProductImage,DisplayOrder,Status,CreatedDate,CanFrontEndOrder,
            //                                    (select top 1 categorysysno from PdCategoryAssociation where IsMaster=@IsMaster and ProductSysNo=t.sysno) Category --主分类--
            //                                    ,(select top 1 Price from PdPrice ppt where ppt.PriceSource=@PriceSource and (ppt.SourceSysNo=@SourceSysNo or ppt.SourceSysNo is null) and ppt.Status=@Status and ppt.ProductSysNo=t.sysno) BasicPrice --基础价格--
            //                                  
            //                                    ,dbo.func_GetPriceSource(t.SysNo,@Status) as Prices --商品价格--
            //                                    ,dbo.func_GetProductAttributeOption(t.SysNo,@AttributeType,@IsSearchKey) as Attributes --商品属性--
            //                              from PdProduct t where (@ProductSysNo=-1 or t.SysNo=@ProductSysNo)) bt 
            //                            inner join PdCategory pc on bt.Category = pc.sysno
            //                            left join PdProductStatistics pdps on pdps.productsysno = bt.sysno 
            //                            where pc.isonline=@isonline 
            //                            ";
            #region 测试用sql
            //        select bt.*,pdps.sales SalesCount,pdps.liking,pdps.favorites,pdps.comments CommentCount,pdps.averagescore,pdps.shares,pdps.question,pdps.totalscore, [dbo].[func_GetAssociationCategory](bt.sysno,1) as AssociationCategory    --关联分类--
            //from 
            //(		
            //       select
            //        SysNo,Barcode,ErpCode,QRCode,ProductName,ProductSubName,BrandSysNo,NameAcronymy,ProductImage,DisplayOrder,Status,CreatedDate,CanFrontEndOrder,OriginSysNo
            //        ,(select top 1 Price from PdPrice ppt where ppt.PriceSource=0 and (ppt.SourceSysNo=0 or ppt.SourceSysNo is null) and ppt.Status=1 and ppt.ProductSysNo=t.sysno) BasicPrice --基础价格--                                 
            //        ,dbo.func_GetPriceSource(t.SysNo,1) as Prices --商品价格--
            //        ,(select top 1 categorysysno from PdCategoryAssociation where IsMaster=1 and ProductSysNo=t.sysno) Category --主分类--
            //        ,dbo.func_GetProductAttributeOption(t.SysNo,30,1) as Attributes --商品属性--
            //        ,dbo.func_GetProductDealerSysNos(t.SysNo) as DealerSysNos --分销商系统编号
            //        ,ISNULL(STUFF((SELECT ','+FPG.Code FROM FeProductGroup FPG 
            //        INNER JOIN FeProductItem FPI ON FPI.GroupSysNo=FPG.SysNo WHERE t.SysNo = FPI.ProductSysNo AND FPI.Status=20 for XML path('')),1,0,'')+',','') AS ProductGroupCode
            //        from PdProduct t where (-1=-1 or t.SysNo=-1)

            //) bt 
            //left join PdProductStatistics pdps on pdps.productsysno = bt.sysno
            #endregion
            var sql = @"
                        select bt.*,pdps.sales SalesCount,pdps.liking,pdps.favorites,pdps.comments CommentCount,pdps.averagescore,pdps.shares,pdps.question,pdps.totalscore, [dbo].[func_GetAssociationCategory](bt.sysno,@isonline) as AssociationCategory    --关联分类--
	                    from 
		                    (		
		                    select
				                    SysNo,Barcode,ErpCode,QRCode,ProductName,ProductSubName,BrandSysNo,NameAcronymy,ProductImage,DisplayOrder,Status,CreatedDate,CanFrontEndOrder,OriginSysNo,IsFrontDisplay
				                    ,(select top 1 Price from PdPrice ppt where ppt.PriceSource=@PriceSource and (ppt.SourceSysNo=@SourceSysNo or ppt.SourceSysNo is null) and ppt.Status=@Status and ppt.ProductSysNo=t.sysno) BasicPrice --基础价格--                                 
				                    ,(select top 1 Price from PdPrice ppt where ppt.PriceSource=@PriceSource1 and (ppt.SourceSysNo=@SourceSysNo1 or ppt.SourceSysNo is null) and ppt.Status=@Status and ppt.ProductSysNo=t.sysno) Price --会员价格--   
                                    ,dbo.func_GetPriceSource(t.SysNo,@Status) as Prices --商品价格--
                                    ,(select top 1 categorysysno from PdCategoryAssociation where IsMaster=1 and ProductSysNo=t.sysno) Category --主分类--
				                    ,dbo.func_GetProductAttributeOption(t.SysNo,@AttributeType,@IsSearchKey) as Attributes --商品属性--
                                    ,dbo.func_GetProductDealerSysNos(t.SysNo) as DealerSysNos --分销商系统编号
                                    ,dbo.func_GetProductWarehouseSysNos(t.SysNo) as WarehouseSysNos --仓库系统编号
                                    ,ISNULL(STUFF((SELECT ','+FPG.Code FROM FeProductGroup FPG INNER JOIN FeProductItem FPI ON FPI.GroupSysNo=FPG.SysNo WHERE t.SysNo = FPI.ProductSysNo AND FPI.Status=20 for XML path('')),1,0,'')+',','') AS ProductGroupCode
			                    from PdProduct t where (t.SysNo in ("+string.Join(",",productSysNos.ToArray())+@") )
			                    ) bt 
		                    left join PdProductStatistics pdps on pdps.productsysno = bt.sysno
             ";
            return Context.Sql(sql)
                      .Parameter("PriceSource", (int)ProductStatus.产品价格来源.基础价格)
                      .Parameter("SourceSysNo", 0)
                      .Parameter("PriceSource1", (int)ProductStatus.产品价格来源.会员等级价)
                      .Parameter("SourceSysNo1", 1)
                      .Parameter("Status", (int)ProductStatus.产品价格状态.有效)
                      .Parameter("AttributeType", (int)ProductStatus.商品属性类型.选项类型)
                      .Parameter("IsSearchKey", (int)ProductStatus.是否作为搜索条件.是)
                      .Parameter("isonline", (int)ProductStatus.是否前端展示.是)
                      .QueryMany<PdProductIndex>();
        }

        /// <summary>
        /// 获取分销商所有商品信息
        /// </summary>
        /// <param name="dealerSysNo">经销商系统编号</param>
        /// <returns></returns>
        /// <remarks>2017-01-04 杨浩 创建</remarks>
        public override DataTable GetDealerAllProductToDataTable(int dealerSysNo)
        {
            var sql = string.Format(@" select bt.*," + (dealerSysNo > 0 ? "sp.Price" : "0") + @"  as Price,pdps.sales SalesCount,pdps.liking,pdps.favorites,pdps.comments CommentCount,pdps.averagescore,pdps.shares,pdps.question,pdps.totalscore,[dbo].[func_GetAssociationCategory](bt.sysno,1) as AssociationCategory    --关联分类--
	                    from 
		                    (		
		                    select
				                    SysNo,Barcode,ErpCode,QRCode,ProductName,ProductSubName,BrandSysNo,NameAcronymy,ProductImage,DisplayOrder,Status,CreatedDate,CanFrontEndOrder,OriginSysNo,IsFrontDisplay,ProductType,Tax
				                    ,(select top 1 Price from PdPrice ppt where ppt.PriceSource=0 and (ppt.SourceSysNo=0 or ppt.SourceSysNo is null) and ppt.Status=1 and ppt.ProductSysNo=t.sysno) BasicPrice --基础价格--                                 
				                   --,(select top 1 Price from PdPrice ppt where ppt.PriceSource=10 and (ppt.SourceSysNo=10 or ppt.SourceSysNo is null) and ppt.Status=1 and ppt.ProductSysNo=t.sysno) Price --会员价格--   
                                   ,dbo.func_GetPriceSource(t.SysNo,1) as Prices --商品价格--
                                   ,(select top 1 categorysysno from PdCategoryAssociation where IsMaster=1 and ProductSysNo=t.sysno) Category --主分类--
				                    ,dbo.func_GetProductAttributeOption(t.SysNo,30,1) as Attributes --商品属性--
                                   ,',{0},' as DealerSysNos
                                   ,dbo.func_GetProductWarehouseSysNos(t.SysNo) as WarehouseSysNos --仓库系统编号
                                   ,ISNULL(STUFF((SELECT ','+FPG.Code FROM FeProductGroup FPG INNER JOIN FeProductItem FPI ON FPI.GroupSysNo=FPG.SysNo WHERE t.SysNo = FPI.ProductSysNo AND FPI.Status=20 GROUP BY FPG.Code for XML path('')),1,0,'')+',','') AS ProductGroupCode
			                    from PdProduct t where t.Status=1
			                    ) bt 						   
		                   left join PdProductStatistics pdps on pdps.productsysno = bt.sysno" + (dealerSysNo>0?
                          @" left join [DsSpecialPrice] as sp on sp.ProductSysNo=bt.SysNo
						   where sp.Status=1 and sp.DealerSysNo=@DealerSysNo" : ""), dealerSysNo);

            return Context.Sql(sql)
                     .Parameter("DealerSysNo", dealerSysNo)
                     .QuerySingle<DataTable>();

        }

        /// <summary>
        /// 获取全部商品信息(用于生成索引文件)
        /// </summary>
        /// <returns></returns>
        /// <remarks>2013-08-02 杨浩 创建</remarks>
        public override DataTable GetAllProductToDataTable()
        {
            var sql = @"
                        select bt.*,pdps.sales SalesCount,pdps.liking,pdps.favorites,pdps.comments CommentCount,pdps.averagescore,pdps.shares,pdps.question,pdps.totalscore, [dbo].[func_GetAssociationCategory](bt.sysno,@isonline) as AssociationCategory    --关联分类--
	                    from 
		                    (		
		                    select
				                    SysNo,Barcode,ErpCode,QRCode,ProductName,ProductSubName,BrandSysNo,NameAcronymy,ProductImage,DisplayOrder,Status,CreatedDate,CanFrontEndOrder,OriginSysNo,IsFrontDisplay,ProductType
				                    ,(select top 1 Price from PdPrice ppt where ppt.PriceSource=@PriceSource and (ppt.SourceSysNo=@SourceSysNo or ppt.SourceSysNo is null) and ppt.Status=@Status and ppt.ProductSysNo=t.sysno) BasicPrice --基础价格--                                 
				                    ,(select top 1 Price from PdPrice ppt where ppt.PriceSource=@PriceSource1 and (ppt.SourceSysNo=@SourceSysNo1 or ppt.SourceSysNo is null) and ppt.Status=@Status and ppt.ProductSysNo=t.sysno) Price --会员价格--   
                                    ,dbo.func_GetPriceSource(t.SysNo,@Status) as Prices --商品价格--
                                    ,(select top 1 categorysysno from PdCategoryAssociation where IsMaster=1 and ProductSysNo=t.sysno) Category --主分类--
				                    ,dbo.func_GetProductAttributeOption(t.SysNo,@AttributeType,@IsSearchKey) as Attributes --商品属性--
                                    ,dbo.func_GetProductDealerSysNos(t.SysNo) as DealerSysNos --分销商系统编号
                                    ,dbo.func_GetProductWarehouseSysNos(t.SysNo) as WarehouseSysNos --仓库系统编号
                                    ,ISNULL(STUFF((SELECT ','+FPG.Code FROM FeProductGroup FPG INNER JOIN FeProductItem FPI ON FPI.GroupSysNo=FPG.SysNo WHERE t.SysNo = FPI.ProductSysNo AND FPI.Status=20 for XML path('')),1,0,'')+',','') AS ProductGroupCode
			                    from PdProduct t where (@ProductSysNo=-1 or t.SysNo=@ProductSysNo)
			                    ) bt 
		                    left join PdProductStatistics pdps on pdps.productsysno = bt.sysno
             ";
            return Context.Sql(sql)
                      .Parameter("PriceSource", (int)ProductStatus.产品价格来源.基础价格)
                      .Parameter("SourceSysNo", 0)
                      .Parameter("PriceSource1", (int)ProductStatus.产品价格来源.会员等级价)
                      .Parameter("SourceSysNo1", 1)
                      .Parameter("Status", (int)ProductStatus.产品价格状态.有效)
                      .Parameter("AttributeType", (int)ProductStatus.商品属性类型.选项类型)
                      .Parameter("IsSearchKey", (int)ProductStatus.是否作为搜索条件.是)
                      .Parameter("ProductSysNo", -1)
                      .Parameter("isonline", (int)ProductStatus.是否前端展示.是)
                      .QuerySingle<DataTable>();
        }
        /// <summary>
        /// 获取商品信息(用于生成索引文件)
        /// </summary>
        /// <param name="productSysNo">商品编号</param>
        /// <returns>商品实体</returns>
        /// <remarks>2013-08-02 黄波 创建</remarks>
        /// <remarks>
        /// 2013-12-23 邵斌 修改：取消过滤商品状态过滤，让所有状态商品都能生成索引
        /// 2015-12-21 杨浩 添加分销商编号,解决商品搜索不能筛选分销商
        /// </remarks>
        public override IList<PdProductIndex> GetAllProduct(int productSysNo = -1)
        {
            //            var sql = @"
            //                       select bt.*,pdps.sales SalesCount,pdps.liking,pdps.favorites,pdps.comments CommentCount,pdps.averagescore,pdps.shares,pdps.question,pdps.totalscore
            //                        , pc.SysNos as AssociationCategory    --关联分类--
            //                        from 
            //                            (select
            //                                    SysNo,Barcode,ErpCode,QRCode,ProductName,ProductSubName,BrandSysNo,NameAcronymy,ProductImage,DisplayOrder,Status,CreatedDate,CanFrontEndOrder,
            //                                    (select top 1 categorysysno from PdCategoryAssociation where IsMaster=@IsMaster and ProductSysNo=t.sysno) Category --主分类--
            //                                    ,(select top 1 Price from PdPrice ppt where ppt.PriceSource=@PriceSource and (ppt.SourceSysNo=@SourceSysNo or ppt.SourceSysNo is null) and ppt.Status=@Status and ppt.ProductSysNo=t.sysno) BasicPrice --基础价格--
            //                                  
            //                                    ,dbo.func_GetPriceSource(t.SysNo,@Status) as Prices --商品价格--
            //                                    ,dbo.func_GetProductAttributeOption(t.SysNo,@AttributeType,@IsSearchKey) as Attributes --商品属性--
            //                              from PdProduct t where (@ProductSysNo=-1 or t.SysNo=@ProductSysNo)) bt 
            //                            inner join PdCategory pc on bt.Category = pc.sysno
            //                            left join PdProductStatistics pdps on pdps.productsysno = bt.sysno 
            //                            where pc.isonline=@isonline 
            //                            ";
            #region 测试用sql
            //        select bt.*,pdps.sales SalesCount,pdps.liking,pdps.favorites,pdps.comments CommentCount,pdps.averagescore,pdps.shares,pdps.question,pdps.totalscore, [dbo].[func_GetAssociationCategory](bt.sysno,1) as AssociationCategory    --关联分类--
            //from 
            //(		
            //       select
            //        SysNo,Barcode,ErpCode,QRCode,ProductName,ProductSubName,BrandSysNo,NameAcronymy,ProductImage,DisplayOrder,Status,CreatedDate,CanFrontEndOrder,OriginSysNo
            //        ,(select top 1 Price from PdPrice ppt where ppt.PriceSource=0 and (ppt.SourceSysNo=0 or ppt.SourceSysNo is null) and ppt.Status=1 and ppt.ProductSysNo=t.sysno) BasicPrice --基础价格--                                 
            //        ,dbo.func_GetPriceSource(t.SysNo,1) as Prices --商品价格--
            //        ,(select top 1 categorysysno from PdCategoryAssociation where IsMaster=1 and ProductSysNo=t.sysno) Category --主分类--
            //        ,dbo.func_GetProductAttributeOption(t.SysNo,30,1) as Attributes --商品属性--
            //        ,dbo.func_GetProductDealerSysNos(t.SysNo) as DealerSysNos --分销商系统编号
            //        ,ISNULL(STUFF((SELECT ','+FPG.Code FROM FeProductGroup FPG 
            //        INNER JOIN FeProductItem FPI ON FPI.GroupSysNo=FPG.SysNo WHERE t.SysNo = FPI.ProductSysNo AND FPI.Status=20 for XML path('')),1,0,'')+',','') AS ProductGroupCode
            //        from PdProduct t where (-1=-1 or t.SysNo=-1)

            //) bt 
            //left join PdProductStatistics pdps on pdps.productsysno = bt.sysno
            #endregion
            var sql = @"
                        select bt.*,pdps.sales SalesCount,pdps.liking,pdps.favorites,pdps.comments CommentCount,pdps.averagescore,pdps.shares,pdps.question,pdps.totalscore, [dbo].[func_GetAssociationCategory](bt.sysno,@isonline) as AssociationCategory    --关联分类--
	                    from 
		                    (		
		                    select
				                    SysNo,Barcode,ErpCode,QRCode,ProductName,ProductSubName,BrandSysNo,NameAcronymy,ProductImage,DisplayOrder,Status,CreatedDate,CanFrontEndOrder,OriginSysNo,IsFrontDisplay,ProductType
				                    ,(select top 1 Price from PdPrice ppt where ppt.PriceSource=@PriceSource and (ppt.SourceSysNo=@SourceSysNo or ppt.SourceSysNo is null) and ppt.Status=@Status and ppt.ProductSysNo=t.sysno) BasicPrice --基础价格--                                 
				                    ,(select top 1 Price from PdPrice ppt where ppt.PriceSource=@PriceSource1 and (ppt.SourceSysNo=@SourceSysNo1 or ppt.SourceSysNo is null) and ppt.Status=@Status and ppt.ProductSysNo=t.sysno) Price --会员价格--   
                                    ,dbo.func_GetPriceSource(t.SysNo,@Status) as Prices --商品价格--
                                    ,(select top 1 categorysysno from PdCategoryAssociation where IsMaster=1 and ProductSysNo=t.sysno) Category --主分类--
				                    ,dbo.func_GetProductAttributeOption(t.SysNo,@AttributeType,@IsSearchKey) as Attributes --商品属性--
                                    ,dbo.func_GetProductDealerSysNos(t.SysNo) as DealerSysNos --分销商系统编号
                                    ,dbo.func_GetProductWarehouseSysNos(t.SysNo) as WarehouseSysNos --仓库系统编号
                                    ,ISNULL(STUFF((SELECT ','+FPG.Code FROM FeProductGroup FPG INNER JOIN FeProductItem FPI ON FPI.GroupSysNo=FPG.SysNo WHERE t.SysNo = FPI.ProductSysNo AND FPI.Status=20 for XML path('')),1,0,'')+',','') AS ProductGroupCode
			                    from PdProduct t where (@ProductSysNo=-1 or t.SysNo=@ProductSysNo)
			                    ) bt 
		                    left join PdProductStatistics pdps on pdps.productsysno = bt.sysno
             ";
            return Context.Sql(sql)
                      .Parameter("PriceSource", (int)ProductStatus.产品价格来源.基础价格)
                      .Parameter("SourceSysNo", 0)
                      .Parameter("PriceSource1", (int)ProductStatus.产品价格来源.会员等级价)
                      .Parameter("SourceSysNo1", 1)
                      .Parameter("Status", (int)ProductStatus.产品价格状态.有效)
                      .Parameter("AttributeType", (int)ProductStatus.商品属性类型.选项类型)
                      .Parameter("IsSearchKey", (int)ProductStatus.是否作为搜索条件.是)
                      .Parameter("ProductSysNo", productSysNo)
                      .Parameter("isonline", (int)ProductStatus.是否前端展示.是)
                      .QueryMany<PdProductIndex>();
        }
        #endregion

        #region 通过分类系统编号查询商品基础信息和某个会员等级价格（物流App使用）
        /// <summary>
        /// 通过分类系统编号查询商品基础信息和某个会员等级价格（物流App使用）
        /// </summary>
        /// <param name="categorySysNo">分类系统编号</param>
        /// <param name="customerLevelSysNo">会员等级编号</param>
        /// <param name="keyword">查询关键字(ERP商品编号,商品名称)</param>
        /// <param name="currentPageIndex">当前索引</param>
        /// <param name="pageSize">每页显示数</param>
        /// <returns>返回物流APP商品信息和指定的会员等级价格</returns>
        /// <remarks>2014-06-05 余勇 创建</remarks>
        public override Pager<AppProduct> GetAppProductListAndPartPrice(int categorySysNo, int customerLevelSysNo, string keyword, int currentPageIndex, int pageSize)
        {
            string sql = @"(
                            SELECT distinct p.SysNo,pr.Price as Price,pr1.price as LevelPrice,p.EasName as ProductName,pdps.averagescore as ProductCommentScore,pdps.comments as CommentTimesCount,pdps.sales SalesCount
                                FROM 
                                  Pdproduct p 
                                  INNER JOIN PdCategoryAssociation pa on p.sysno = pa.productsysno
                                  INNER JOIN (select * from pdcategory where status=@0 start with (@1=0 or sysno=@1) Connect By " + (categorySysNo > 0 ? "Prior" : "") +
                                  @" sysno = parentsysno order by code) c on pa.categorysysno = c.sysno
                                  INNER JOIN pdprice pr  ON pr.productsysno = p.sysno AND pr.status = @2 AND pr.pricesource = @3 INNER JOIN pdprice pr1 ON pr1.productsysno = p.sysno AND pr1.status = @2 AND pr1.pricesource = @4 AND (isnull(pr1.sourcesysno,0) = 0 or pr1.sourcesysno = @5)
                                  LEFT JOIN PdProductStatistics pdps on pdps.productsysno = p.sysno  
                                  WHERE 
                                    p.status = @0  
                                    AND (@7 is null OR REGEXP_charindex(p.ProductName, @7, 1, 1, 1, 'i') > 0 OR REGEXP_charindex(p.ErpCode, @7, 1, 1, 1, 'i') > 0)
                                    AND   ((@1 = 0 and pa.ismaster = 1) or @1>0) 
                                    AND pr.status = @2
                                ) tb  ";

            var dataList = Context.Select<AppProduct>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(1)").From(sql);
            var paras = new object[]
                {
                    (int)ProductStatus.商品分类状态.有效,
                    categorySysNo,
                    (int)ProductStatus.产品价格状态.有效,
                    (int)ProductStatus.产品价格来源.基础价格,
                    (int)ProductStatus.产品价格来源.会员等级价,
                    customerLevelSysNo,
                    (int)ProductStatus.商品状态.上架,
                    keyword
                };

            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<AppProduct>()
            {
                Rows = dataList.OrderBy(@"tb.SalesCount desc,tb.SysNo desc").Paging(currentPageIndex, pageSize).QueryMany(),
                TotalRows = dataCount.QuerySingle(),
                PageSize = pageSize,
                CurrentPage = currentPageIndex
            };
            return pager;
        }
        #endregion

        #region 获取商品时间戳

        /// <summary>
        /// 获取商品主表时间戳
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <returns>返回时间戳</returns>
        /// <remarks>2013-10-21 邵斌 创建</remarks>
        public override DateTime GetPdProductStamp(int productSysNo)
        {
            return Context.Sql("select stamp from pdproduct where sysno=@0", productSysNo).QuerySingle<DateTime>();
        }

        #endregion

        /// <summary>
        /// 获取商品统计信息
        /// </summary>
        /// <param name="pdSysNo">商品系统编号</param>
        /// <returns>商品统计信息</returns>
        /// <remarks>2013-08-26 郑荣华 创建</remarks>
        public override PdProductStatistics GetPdProductStatistics(int pdSysNo)
        {
            return Context.Sql("select * from PdProductStatistics where ProductSysNo=@0", pdSysNo)
                          .QuerySingle<PdProductStatistics>();
        }

        /// <summary>
        /// 获取商品详情包括商品类型、价格等
        /// </summary>
        /// <param name="pdSysNo">商品系统编号</param>
        /// <returns>返回商品详情</returns>
        /// <remarks>2013-09-23 余勇 创建</remarks>
        /// <remarks>2013-11-15 沈强 重构</remarks>
        public override ParaProductSearchFilter GetPdProductBySysNo(int pdSysNo)
        {
            return GetPdProductBySysNo(pdSysNo, ProductStatus.产品价格来源.基础价格, ProductStatus.商品状态.上架,
                                       ProductStatus.产品价格状态.有效);
            #region 之前的代码
            //            return Context.Sql(@"select p.* ,c.sysno as ProductCategorySysNo,c.categoryname as ProductCategoryName,pp.price as Price from
            //                                     pdproduct p 
            //                                     left join PdCategoryAssociation pa on p.sysno = pa.productsysno 
            //                                     left join pdcategory c on pa.categorysysno = c.sysno
            //                                     left join pdprice pp on pp.productsysno = p.sysno and pp.pricesource=:pricesource
            //                                     where p.sysno=:sysno
            //                                     and  pa.ismaster = 1 and p.status = :status
            //                                     and pp.status = :status  ")
            //                              .Parameter("pricesource", (int)ProductStatus.产品价格来源.基础价格)
            //                              .Parameter("sysno", pdSysNo)
            //                              .Parameter("status", (int)ProductStatus.商品状态.上架)
            //                              .Parameter("status", (int)ProductStatus.产品价格状态.有效)
            //                              .QuerySingle<ParaProductSearchFilter>();
            #endregion

        }

        /// <summary>
        /// 获取商品详情包括商品类型、价格等
        /// </summary>
        /// <param name="pdSysNo">商品系统编号</param>
        /// <param name="priceSource">产品价格来源</param>
        /// <param name="status">商品状态</param>
        /// <param name="priceStatus">产品价格状态</param>
        /// <returns>返回商品详情</returns>
        /// <remarks>2013-11-15 沈强 创建</remarks>
        public ParaProductSearchFilter GetPdProductBySysNo(int pdSysNo, ProductStatus.产品价格来源? priceSource, ProductStatus.商品状态? status, ProductStatus.产品价格状态? priceStatus)
        {
            var sql = @"select p.* ,c.sysno as ProductCategorySysNo,c.categoryname as ProductCategoryName,pp.price as Price from
                                     pdproduct p 
                                     left join PdCategoryAssociation pa on p.sysno = pa.productsysno 
                                     left join pdcategory c on pa.categorysysno = c.sysno
                                     left join pdprice pp on pp.productsysno = p.sysno 
                                     where p.sysno=@sysno
                                     and (@pricesource is null or pp.pricesource = @pricesource1)
                                     and  pa.ismaster = 1 
                                     and (@status is null or p.status = @status1)
                                     and (@status2 is null or pp.status = @status3)";

            int priceSou = priceSource == null ? -1 : (int)priceSource;
            int sta = status == null ? -1 : (int)status;
            int priceSta = priceStatus == null ? -1 : (int)priceStatus;

            return Context.Sql(sql).Parameter("sysno", pdSysNo)
                              .Parameter("pricesource", priceSource)
                              .Parameter("pricesource1", priceSou)
                              .Parameter("status", status)
                              .Parameter("status1", sta)
                              .Parameter("status2", priceStatus)
                              .Parameter("status3", priceSta)
                              .QuerySingle<ParaProductSearchFilter>();
        }

        /// <summary>
        /// 通过商品编号获取商品信息
        /// </summary>
        /// <param name="sysNo">商品编号</param>
        /// <returns>商品信息</returns>
        /// <remarks>2013-09-23 余勇 创建</remarks>
        public override PdProduct GetProductBySysNo(int sysNo)
        {
            return Context.Sql("select * from PdProduct where SysNo=@0", sysNo)
                          .QuerySingle<PdProduct>();
        }

        /// <summary>
        /// 通过ERP编号获取商品信息
        /// </summary>
        /// <param name="erpCode">ERP编号</param>
        /// <returns>商品信息</returns>
        /// <remarks>2013-09-23 余勇 创建</remarks>
        public override PdProduct GetProductByErpCode(string erpCode)
        {
            //查询erp编号
            return Context.Sql("select * from pdproduct where erpcode = @erpcode")
                                .Parameter("erpcode", erpCode)
                                .QuerySingle<PdProduct>();
        }
        /// <summary>
        /// 通过商品编号获取b2b商品信息
        /// </summary>
        /// <param name="erpCode">商品编号</param>
        /// <returns>商品信息</returns>
        /// <returns>罗勤瑶 2017-10-11</returns>
        public override PdProduct GetB2BProductByErpCode(string erpCode)
        {
            //查询erp编号
            return ContextB2B.Sql("select * from pdproduct where erpcode = @erpcode")
                                .Parameter("erpcode", erpCode)
                                .QuerySingle<PdProduct>();
        }
        /// <summary>
        /// 获取商品会员等级价格，若无会员等级则取基础价格
        /// </summary>
        /// <param name="customerLevelSysNo">会员等级</param>
        /// <param name="productSysNo">商品编号</param>
        /// <returns>商品会员等级价格</returns>
        /// <remarks>2013-11-22 余勇 创建</remarks>
        public override CBPdProductDetail SelectProductPrice(int customerLevelSysNo, int productSysNo)
        {

//            return Context.Sql(@"
//                                select 
//                                    p.sysno,p.productname,p.productimage,pr.price as BasicPrice, cl.levelname as PriceName
//                                from
//                                    pdProduct p
//                                    inner join pdprice pr on p.sysno = pr.productsysno
//                                    left join CrCustomerLevel cl on cl.sysno = pr.sourcesysno
//                                where
//                                    (
//                                       (pr.pricesource=@1 and pr.sourcesysno=@2)
//                                       or (@3 = 0 and pr.pricesource=@4)
//                                    )
//                                    and p.sysno=@2  and pr.status = @3 
//                            ", customerLevelSysNo, (int)ProductStatus.产品价格来源.会员等级价, customerLevelSysNo, customerLevelSysNo, (int)ProductStatus.产品价格来源.基础价格, productSysNo,
//                                 (int)ProductStatus.产品价格状态.有效).QuerySingle<CBPdProductDetail>();

            string sqlStr="select p.sysno,p.productname,p.productimage,pr.price as BasicPrice, cl.levelname as PriceName from pdProduct p inner join pdprice pr on p.sysno = pr.productsysno left join CrCustomerLevel cl on cl.sysno = pr.sourcesysno";
            sqlStr += " where (pr.pricesource=" + (int)ProductStatus.产品价格来源.会员等级价 + " and pr.sourcesysno=" + customerLevelSysNo + ") and p.sysno=" + productSysNo + "  and pr.status =1";
            sqlStr+="";
            
            return Context.Sql(sqlStr).QuerySingle<CBPdProductDetail>();

        }

        /// <summary>
        /// 获取当前商品集合中上架商品系统编号
        /// </summary>
        /// <param name="productSysNo">商品编号集合</param>
        /// <param name="isFrontProduct">是否只允许前台下单的商品</param>
        /// <returns>上架商品系统编号</returns>
        /// <remarks>2013-12-24 吴文强 创建</remarks>
        public override IList<int> GetOnlineProduct(int[] productSysNo, bool isFrontProduct = true)
        {
            var onlineProducts = new List<int>();

            if (productSysNo.Count() == 0)
            {
                return new List<int>();
            }
            var numberPages = (int)Math.Ceiling(productSysNo.Count() / (double)1000);

            for (int i = 0; i < numberPages; i++)
            {
                var currProducts = productSysNo.Skip(i * 1000).Take(1000).ToArray();

                const string strSql = @"
                        select p.SysNo from PdProduct p
                        where p.Status = @status 
                          and p.SysNo in(@promotionSysNo)
                          and (0 = @isFrontProduct or p.CanFrontEndOrder = @isFrontProduct)
                        ";

                var entity = Context.Sql(strSql)
                                    .Parameter("status", (int)ProductStatus.商品状态.上架)
                                    .Parameter("promotionSysNo", currProducts)
                    //.Parameter("isFrontProduct", isFrontProduct ? (int)ProductStatus.商品是否前台下单.是 : (int)ProductStatus.商品是否前台下单.否)
                                    .Parameter("isFrontProduct", isFrontProduct ? (int)ProductStatus.商品是否前台下单.是 : (int)ProductStatus.商品是否前台下单.否)
                                    .QueryMany<int>();
                onlineProducts.AddRange(entity);
            }

            return onlineProducts;
        }

        /// <summary>
        /// 获取当前商品集合
        /// </summary>
        /// <param name="productSysNo">商品编号集合</param>
        /// <remarks>2013-12-24 吴文强 创建</remarks>
        public override IList<int> GetOnlineProduct(int[] productSysNo)
        {
            var onlineProducts = new List<int>();

            if (productSysNo.Count() == 0)
            {
                return new List<int>();
            }
            var numberPages = (int)Math.Ceiling(productSysNo.Count() / (double)1000);

            for (int i = 0; i < numberPages; i++)
            {
                var currProducts = productSysNo.Skip(i * 1000).Take(1000).ToArray();

                const string strSql = @"
                        select p.SysNo from PdProduct p
                        where p.SysNo in(@promotionSysNo)";

                var entity = Context.Sql(strSql)
                                    //.Parameter("status", (int)ProductStatus.商品状态.上架)
                                    .Parameter("promotionSysNo", currProducts)
                    //.Parameter("isFrontProduct", isFrontProduct ? (int)ProductStatus.商品是否前台下单.是 : (int)ProductStatus.商品是否前台下单.否)
                                    //.Parameter("isFrontProduct", isFrontProduct ? (int)ProductStatus.商品是否前台下单.是 : (int)ProductStatus.商品是否前台下单.否)
                                    .QueryMany<int>();
                onlineProducts.AddRange(entity);
            }

            return onlineProducts;
        }


        #region 更新商品前台显示字段
        /// <summary>
        /// 更新商品前台显示字段
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <param name="IsFrontDisplay">前台显示字段</param>
        /// <returns>返回 true：成功 false：失败</returns>
        /// <remarks>2015-12-24 王耀发 创建</remarks>
        public override bool UpdateProductIsFrontDisplay(int productSysNo, int IsFrontDisplay)
        {
            int result = Context.Sql("update PdProduct set IsFrontDisplay = @0 where sysno = @1 ", IsFrontDisplay,
                                     productSysNo).Execute();
            return result > 0;
        }
        #endregion

        /// <summary>
        /// 获取导出商品信息
        /// </summary>
        /// <param name="sysNos">商品系统编号集合</param>
        /// <param name="productDetail">查询条件</param>
        /// <returns></returns>
        /// <remarks>2016-11-28 杨浩 创建</remarks>
        public override System.Data.DataTable GetExportProductToDataTable(List<int> sysNos, ParaProductFilter productDetail)
        {
            string sqlText = @"select  p.SysNo,p.ErpCode,p.ProductName,p.EasName,c.CategoryName,b.Name as BrandName,case p.ProductType when 10 then '普通商品' when 20 then '虚拟商品' when 30 then '保税商品' when 40 then '直邮商品' ELSE '完税商品' END as TypeName,
                      o.Origin_Name as OriginName,p.Barcode,p.GrosWeight,p.Tax,p.PriceRate,p.PriceValue,price.Price, price1.Price as SalePrice,p.TradePrice, price2.Price,PdProductStatistics.Sales
                      ,price3.Price,price4.Price
            from PdProduct p 
            inner join PdProductStatistics on  PdProductStatistics.ProductSysNo = p.SysNo
            inner join (select productsysno from PdCategoryAssociation  group by productsysno ) ca1 on ca1.productsysno = p.sysno 
            left join (select productsysno,categorysysno from PdCategoryAssociation where IsMaster=1) ca on ca.productsysno = p.sysno 
            left join  PdCategory c on ca.categorysysno = c.sysno 
            left join  PdBrand b on p.BrandSysNo = b.SysNo
            left join Origin o on p.OriginSysNo = o.SysNo
            left join  (select * from PdPrice where PriceSource =0 and SourceSysNo = 0 and [Status] = 1) price  on price.productsysno = p.sysno
            left join  (select * from PdPrice where PriceSource =10 and SourceSysNo = 1 and [Status] = 1) price1  on price1.productsysno = p.sysno
            left join  (select * from PdPrice where PriceSource =20 and SourceSysNo = 0 and [Status] = 1) price2  on price2.productsysno = p.sysno
            left join  (select * from PdPrice where PriceSource =10 and SourceSysNo = 6 and [Status] = 1) price3  on price3.productsysno = p.sysno
            left join  (select * from PdPrice where PriceSource =10 and SourceSysNo = 7 and [Status] = 1) price4  on price4.productsysno = p.sysno
            where p.Status != 2 ";

            if (sysNos.Count > 0)
            {
                sqlText += " and p.SysNo in (" + string.Join(",", sysNos) + ")";
            }
            if (productDetail != null)
            {
                if (!string.IsNullOrEmpty(productDetail.ProductName))
                {
                    sqlText += " and p.ProductName like '%" + productDetail.ProductName.Trim().Replace(" ", "%") + "%'";
                }

                if (productDetail.Status > -1)
                {
                    sqlText += " and p.Status = " + productDetail.Status;
                }

                if (productDetail.CanFrontEndOrder > -1)
                {
                    sqlText += " and p.CanFrontEndOrder = " + productDetail.CanFrontEndOrder;
                }

                if (productDetail.IsFrontDisplay > -1)
                {
                    sqlText += " and p.IsFrontDisplay = " + productDetail.IsFrontDisplay;
                }

                if (!string.IsNullOrEmpty(productDetail.ErpCode))
                {
                    sqlText += " and p.ErpCode = " + productDetail.ErpCode;
                }

                if (!string.IsNullOrEmpty(productDetail.Barcode))
                {
                    sqlText += " and p.Barcode = " + productDetail.Barcode;
                }

                if ((productDetail.ProductType) > 0)
                {
                    sqlText += " and p.ProductType = " + productDetail.ProductType;
                }

                if ((productDetail.StartTime) != null)
                {
                    sqlText += " and p.LastUpdateDate >= '" + productDetail.StartTime.Value.ToString("yyyy-MM-dd") + " 00:00:00'";
                }
                if ((productDetail.EndTime) != null)
                {
                    sqlText += " and p.LastUpdateDate <= '" + productDetail.EndTime.Value.ToString("yyyy-MM-dd") + " 23:59:59'";
                }

                if ((productDetail.CreateStartTime) != null)
                {
                    sqlText += " and p.CreatedDate >= '" + productDetail.CreateStartTime.Value.ToString("yyyy-MM-dd") + " 00:00:00'";
                }
                if ((productDetail.CreateEndTime) != null)
                {
                    sqlText += " and p.CreatedDate <= '" + productDetail.CreateEndTime.Value.ToString("yyyy-MM-dd") + " 23:59:59'";
                }

                if ((productDetail.OriginSysNo) > 0)
                {
                    sqlText += " and p.OriginSysNo = " + productDetail.OriginSysNo;
                }

                if ((productDetail.ProductCategorySysno) > 0)
                {
                    sqlText += " and c.SysNos like '%," + productDetail.ProductCategorySysno + ",%' ";
                }

            }

            var outProducts = Context.Sql(sqlText).QuerySingle<System.Data.DataTable>();

            return outProducts;
        }
        /// <summary>
        /// 查询导出商品列表
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns></returns>
        /// <remarks>2015-12-30 王耀发 创建</remarks>
        public override List<CBOutputPdProducts> GetExportProductList(List<int> sysNos, ParaProductFilter productDetail)
        {
            string sqlText = @"select  p.SysNo AS 自动编码,p.ErpCode AS 商品编码,p.ProductName AS 前台显示名称,p.EasName AS 后台显示名称,c.CategoryName AS 分类,b.Name as 品牌,case p.ProductType when 10 then '普通商品' when 20 then '虚拟商品' when 30 then '保税商品' when 40 then '直邮商品' ELSE '完税商品' END as 类型,
                      o.Origin_Name AS 原产地,p.Barcode AS 条形码,p.GrosWeight AS 毛重,p.Tax AS 税率,p.PriceRate AS 直营利润比例,p.PriceValue AS 直营分销商利润金额,price.Price as 商品价格, price1.Price as 会员价,p.TradePrice as 批发价, price2.Price as 门店销售价,PdProductStatistics.Sales as 销量
                      ,price3.Price as VIP会员价,price4.Price as 钻石会员价,price5.Price as 销售合伙人
            from PdProduct p 
            inner join PdProductStatistics on  PdProductStatistics.ProductSysNo = p.SysNo
            inner join (select productsysno from PdCategoryAssociation  group by productsysno ) ca1 on ca1.productsysno = p.sysno 
            left join (select productsysno,categorysysno from PdCategoryAssociation where IsMaster=1) ca on ca.productsysno = p.sysno 
            left join  PdCategory c on ca.categorysysno = c.sysno 
            left join  PdBrand b on p.BrandSysNo = b.SysNo
            left join Origin o on p.OriginSysNo = o.SysNo
            left join  (select * from PdPrice where PriceSource =0 and SourceSysNo = 0 and [Status] = 1) price  on price.productsysno = p.sysno
            left join  (select * from PdPrice where PriceSource =10 and SourceSysNo = 1 and [Status] = 1) price1  on price1.productsysno = p.sysno
            left join  (select * from PdPrice where PriceSource =20 and SourceSysNo = 0 and [Status] = 1) price2  on price2.productsysno = p.sysno
            left join  (select * from PdPrice where PriceSource =10 and SourceSysNo = 6 and [Status] = 1) price3  on price3.productsysno = p.sysno
            left join  (select * from PdPrice where PriceSource =10 and SourceSysNo = 7 and [Status] = 1) price4  on price4.productsysno = p.sysno
            left join  (select * from PdPrice where PriceSource =10 and SourceSysNo = 8 and [Status] = 1) price5  on price5.productsysno = p.sysno
            where p.Status != 2 ";

            if (sysNos.Count > 0)
            {
                sqlText += " and p.SysNo in (" + string.Join(",", sysNos) + ")";
            }
            if (productDetail != null)
            {
                if (!string.IsNullOrEmpty(productDetail.ProductName))
                {
                    sqlText += " and p.ProductName like '%" + productDetail.ProductName.Trim().Replace(" ", "%") + "%'";
                }

                if (productDetail.Status > -1)
                {
                    sqlText += " and p.Status = " + productDetail.Status;
                }

                if (productDetail.CanFrontEndOrder > -1)
                {
                    sqlText += " and p.CanFrontEndOrder = " + productDetail.CanFrontEndOrder;
                }

                if (productDetail.IsFrontDisplay > -1)
                {
                    sqlText += " and p.IsFrontDisplay = " + productDetail.IsFrontDisplay;
                }

                if (!string.IsNullOrEmpty(productDetail.ErpCode))
                {
                    sqlText += " and p.ErpCode = " + productDetail.ErpCode;
                }

                if (!string.IsNullOrEmpty(productDetail.Barcode))
                {
                    sqlText += " and p.Barcode = " + productDetail.Barcode;
                }

                if ((productDetail.ProductType) > 0)
                {
                    sqlText += " and p.ProductType = " + productDetail.ProductType;
                }

                if ((productDetail.StartTime) !=null)
                {
                    sqlText += " and p.LastUpdateDate >= '" + productDetail.StartTime.Value.ToString("yyyy-MM-dd")+" 00:00:00'";
                }
                if ((productDetail.EndTime) != null)
                {
                    sqlText += " and p.LastUpdateDate <= '" + productDetail.EndTime.Value.ToString("yyyy-MM-dd") + " 23:59:59'";
                }

                if ((productDetail.CreateStartTime) != null)
                {
                    sqlText += " and p.CreatedDate >= '" + productDetail.CreateStartTime.Value.ToString("yyyy-MM-dd") + " 00:00:00'";
                }
                if ((productDetail.CreateEndTime) != null)
                {
                    sqlText += " and p.CreatedDate <= '" + productDetail.CreateEndTime.Value.ToString("yyyy-MM-dd") + " 23:59:59'";
                }

                if ((productDetail.OriginSysNo) > 0)
                {
                    sqlText += " and p.OriginSysNo = " + productDetail.OriginSysNo;
                }
                
                if ((productDetail.ProductCategorySysno) > 0)
                {
                    sqlText += " and c.SysNos like '%," + productDetail.ProductCategorySysno + ",%' ";
                }

            }

            List<CBOutputPdProducts> outProducts = Context.Sql(sqlText).QueryMany<CBOutputPdProducts>();

            return outProducts;
        }


        /// <summary>
        /// 查询导出商品列表利嘉模板
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns></returns>
        /// <remarks>2017-5-17 罗勤尧 创建</remarks>
        public override List<CBOutputPdProductsLijia> GetExportProductListLiJia(List<int> sysNos, ParaProductFilter productDetail)
        {
            string sqlText = @"select p.ErpCode AS 商品编码,p.ProductName AS 名称,c.CategoryName AS 商品分类,b.Name as 品牌,p.EasName AS 商品简称,
                    p.Volume AS 规格颜色,p.NetWeight AS 重量 
            from PdProduct p 
            inner join PdProductStatistics on  PdProductStatistics.ProductSysNo = p.SysNo
            inner join (select productsysno from PdCategoryAssociation  group by productsysno ) ca1 on ca1.productsysno = p.sysno 
            left join (select productsysno,categorysysno from PdCategoryAssociation where IsMaster=1) ca on ca.productsysno = p.sysno 
            left join  PdCategory c on ca.categorysysno = c.sysno 
            left join  PdBrand b on p.BrandSysNo = b.SysNo
            left join Origin o on p.OriginSysNo = o.SysNo
            where p.Status != 2 ";

            if (sysNos != null && sysNos.Count > 0)
            {
                sqlText += " and p.SysNo in (" + string.Join(",", sysNos) + ")";
            }
            if (productDetail != null)
            {
                if (!string.IsNullOrEmpty(productDetail.ProductName))
                {
                    sqlText += " and p.ProductName like '%" + productDetail.ProductName.Trim().Replace(" ", "%") + "%'";
                }

                if (productDetail.Status > -1)
                {
                    sqlText += " and p.Status = " + productDetail.Status;
                }

                if (productDetail.CanFrontEndOrder > -1)
                {
                    sqlText += " and p.CanFrontEndOrder = " + productDetail.CanFrontEndOrder;
                }

                if (productDetail.IsFrontDisplay > -1)
                {
                    sqlText += " and p.IsFrontDisplay = " + productDetail.IsFrontDisplay;
                }

                if (!string.IsNullOrEmpty(productDetail.ErpCode))
                {
                    sqlText += " and p.ErpCode = " + productDetail.ErpCode;
                }

                if (!string.IsNullOrEmpty(productDetail.Barcode))
                {
                    sqlText += " and p.Barcode = " + productDetail.Barcode;
                }

                if ((productDetail.ProductType) > 0)
                {
                    sqlText += " and p.ProductType = " + productDetail.ProductType;
                }

                if ((productDetail.StartTime) != null)
                {
                    sqlText += " and p.LastUpdateDate >= '" + productDetail.StartTime.Value.ToString("yyyy-MM-dd") + " 00:00:00'";
                }
                if ((productDetail.EndTime) != null)
                {
                    sqlText += " and p.LastUpdateDate <= '" + productDetail.EndTime.Value.ToString("yyyy-MM-dd") + " 23:59:59'";
                }

                if ((productDetail.CreateStartTime) != null)
                {
                    sqlText += " and p.CreatedDate >= '" + productDetail.CreateStartTime.Value.ToString("yyyy-MM-dd") + " 00:00:00'";
                }
                if ((productDetail.CreateEndTime) != null)
                {
                    sqlText += " and p.CreatedDate <= '" + productDetail.CreateEndTime.Value.ToString("yyyy-MM-dd") + " 23:59:59'";
                }

                if ((productDetail.OriginSysNo) > 0)
                {
                    sqlText += " and p.OriginSysNo = " + productDetail.OriginSysNo;
                }

                if ((productDetail.ProductCategorySysno) > 0)
                {
                    sqlText += " and c.SysNos like '%," + productDetail.ProductCategorySysno + ",%' ";
                }

            }

            List<CBOutputPdProductsLijia> outProducts = Context.Sql(sqlText).QueryMany<CBOutputPdProductsLijia>();

            return outProducts;
        }

        /// <summary>
        /// 查询导出商品列表
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns></returns>
        /// <remarks>2015-12-30 王耀发 创建</remarks>
        public override List<CBOutputPdProductsByYD> GetExportProductListByYD(List<int> sysNos, ParaProductFilter productDetail)
        {
            string sqlText = @"select  p.SysNo AS 自动编码,p.ErpCode AS 商品编码,p.ProductName AS 前台显示名称,p.EasName AS 后台显示名称,c.CategoryName AS 分类,b.Name as 品牌,case p.ProductType when 10 then '普通商品' when 20 then '虚拟商品' when 30 then '保税商品' when 40 then '直邮商品' ELSE '完税商品' END as 类型,
                      o.Origin_Name AS 原产地,p.Barcode AS 条形码,p.GrosWeight AS 毛重,p.Tax AS 税率,p.PriceRate AS 直营利润比例,p.PriceValue AS 直营分销商利润金额,price.Price as 商品价格, price1.Price as 会员价,p.TradePrice as 批发价, price2.Price as 门店销售价,p.CostPrice as 成本价,p.Status as 上下架,p.CanFrontEndOrder as 是否允许前台下单,p.IsFrontDisplay 是否前台显示
            from PdProduct p 
            
            left join (select productsysno,categorysysno from PdCategoryAssociation where IsMaster=1) ca on ca.productsysno = p.sysno 
            left join  PdCategory c on ca.categorysysno = c.sysno 
            left join  PdBrand b on p.BrandSysNo = b.SysNo
            left join Origin o on p.OriginSysNo = o.SysNo
            left join  (select * from PdPrice where PriceSource =0 and SourceSysNo = 0 and [Status] = 1) price  on price.productsysno = p.sysno
            left join  (select * from PdPrice where PriceSource =10 and SourceSysNo = 1 and [Status] = 1) price1  on price1.productsysno = p.sysno
            left join  (select * from PdPrice where PriceSource =90 and SourceSysNo = 0 and [Status] = 1) price2  on price2.productsysno = p.sysno
            where p.Status != 2 ";//inner join (select productsysno from PdCategoryAssociation  group by productsysno ) ca1 on ca1.productsysno = p.sysno 

            if (sysNos.Count > 0)
            {
                sqlText += " and p.SysNo in (" + string.Join(",", sysNos) + ")";
            }
            if (productDetail != null)
            {
                if (!string.IsNullOrEmpty(productDetail.ProductName))
                {
                    sqlText += " and p.ProductName like '%" + productDetail.ProductName.Trim().Replace(" ", "%") + "%'";
                }

                if (productDetail.Status > -1)
                {
                    sqlText += " and p.Status = " + productDetail.Status;
                }

                if (productDetail.CanFrontEndOrder > -1)
                {
                    sqlText += " and p.CanFrontEndOrder = " + productDetail.CanFrontEndOrder;
                }

                if (productDetail.IsFrontDisplay > -1)
                {
                    sqlText += " and p.IsFrontDisplay = " + productDetail.IsFrontDisplay;
                }

                if (!string.IsNullOrEmpty(productDetail.ErpCode))
                {
                    sqlText += " and p.ErpCode = " + productDetail.ErpCode;
                }

                if (!string.IsNullOrEmpty(productDetail.Barcode))
                {
                    sqlText += " and p.Barcode = " + productDetail.Barcode;
                }

                if ((productDetail.ProductType) > 0)
                {
                    sqlText += " and p.ProductType = " + productDetail.ProductType;
                }

                if ((productDetail.StartTime) != null)
                {
                    sqlText += " and p.LastUpdateDate >= '" + productDetail.StartTime.Value.ToString("yyyy-MM-dd") + " 00:00:00'";
                }
                if ((productDetail.EndTime) != null)
                {
                    sqlText += " and p.LastUpdateDate <= '" + productDetail.EndTime.Value.ToString("yyyy-MM-dd") + " 23:59:59'";
                }

                if ((productDetail.CreateStartTime) != null)
                {
                    sqlText += " and p.CreatedDate >= '" + productDetail.CreateStartTime.Value.ToString("yyyy-MM-dd") + " 00:00:00'";
                }
                if ((productDetail.CreateEndTime) != null)
                {
                    sqlText += " and p.CreatedDate <= '" + productDetail.CreateEndTime.Value.ToString("yyyy-MM-dd") + " 23:59:59'";
                }

                if ((productDetail.OriginSysNo) > 0)
                {
                    sqlText += " and p.OriginSysNo = " + productDetail.OriginSysNo;
                }

                if ((productDetail.ProductCategorySysno) > 0)
                {
                    sqlText += " and c.SysNos like '%," + productDetail.ProductCategorySysno + ",%' ";
                }

            }

            List<CBOutputPdProductsByYD> outProducts = Context.Sql(sqlText).QueryMany<CBOutputPdProductsByYD>();

            return outProducts;
        }

        /// <summary>
        /// 查询导出商品列表(信营)
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns></returns>
        public override List<CBXinyingOutputPdProducts> GetXinYingExportProductList(List<int> sysNos, ParaProductFilter productDetail)
        {
            string sqlText = @"select  p.SysNo AS 自动编码,p.ErpCode AS 商品编码,p.ProductName AS 前台显示名称,p.EasName AS 后台显示名称,c.CategoryName AS 分类,b.Name as 品牌,case p.ProductType when 10 then '普通商品' when 20 then '虚拟商品' when 30 then '保税商品' when 40 then '直邮商品' ELSE '完税商品' END as 类型,
                      o.Origin_Name AS 原产地,p.Barcode AS 条形码,p.GrosWeight AS 毛重,p.NetWeight AS 净重,p.Tax AS 税率,p.PriceRate AS 直营利润比例,p.PriceValue AS 直营分销商利润金额,price.Price as 商品价格, price1.Price as 会员价,p.TradePrice as 批发价, price2.Price as 门店销售价  ,(case when p.TradePrice=0 then 0 else (price1.Price - p.TradePrice)/p.TradePrice end) as 利润率
            from PdProduct p 
            inner join (select productsysno from PdCategoryAssociation  group by productsysno ) ca1 on ca1.productsysno = p.sysno 
            left join (select productsysno,categorysysno from PdCategoryAssociation where IsMaster=1) ca on ca.productsysno = p.sysno 
            left join  PdCategory c on ca.categorysysno = c.sysno 
            left join  PdBrand b on p.BrandSysNo = b.SysNo
            left join Origin o on p.OriginSysNo = o.SysNo
            left join  (select * from PdPrice where PriceSource =0 and SourceSysNo = 0 and [Status] = 1) price  on price.productsysno = p.sysno
            left join  (select * from PdPrice where PriceSource =10 and SourceSysNo = 1 and [Status] = 1) price1  on price1.productsysno = p.sysno
            left join  (select * from PdPrice where PriceSource =90 and SourceSysNo = 0 and [Status] = 1) price2  on price2.productsysno = p.sysno

            where p.Status != 2 ";

            if (sysNos!=null&&sysNos.Count > 0)
            {
                sqlText += " and p.SysNo in (" + string.Join(",", sysNos) + ")";
            }
            if (productDetail != null)
            {
                if (!string.IsNullOrEmpty(productDetail.ProductName))
                {
                    sqlText += " and p.ProductName like '%" + productDetail.ProductName.Trim().Replace(" ", "%") + "%'";
                }

                if (productDetail.Status > -1)
                {
                    sqlText += " and p.Status = " + productDetail.Status;
                }

                if (productDetail.CanFrontEndOrder > -1)
                {
                    sqlText += " and p.CanFrontEndOrder = " + productDetail.CanFrontEndOrder;
                }

                if (productDetail.IsFrontDisplay > -1)
                {
                    sqlText += " and p.IsFrontDisplay = " + productDetail.IsFrontDisplay;
                }

                if (!string.IsNullOrEmpty(productDetail.ErpCode))
                {
                    sqlText += " and p.ErpCode = '" + productDetail.ErpCode+"' ";
                }

                if (!string.IsNullOrEmpty(productDetail.Barcode))
                {
                    sqlText += " and p.Barcode = '" + productDetail.Barcode+"'";
                }

                if ((productDetail.ProductType) > 0)
                {
                    sqlText += " and p.ProductType = " + productDetail.ProductType;
                }

                if ((productDetail.StartTime) != null)
                {
                    sqlText += " and p.LastUpdateDate >= '" + productDetail.StartTime.Value.ToString("yyyy-MM-dd") + " 00:00:00'";
                }
                if ((productDetail.EndTime) != null)
                {
                    sqlText += " and p.LastUpdateDate <= '" + productDetail.EndTime.Value.ToString("yyyy-MM-dd") + " 23:59:59'";
                }

                if ((productDetail.CreateStartTime) != null)
                {
                    sqlText += " and p.CreatedDate >= '" + productDetail.CreateStartTime.Value.ToString("yyyy-MM-dd") + " 00:00:00'";
                }
                if ((productDetail.CreateEndTime) != null)
                {
                    sqlText += " and p.CreatedDate <= '" + productDetail.CreateEndTime.Value.ToString("yyyy-MM-dd") + " 23:59:59'";
                }

                if ((productDetail.OriginSysNo) > 0)
                {
                    sqlText += " and p.OriginSysNo = " + productDetail.OriginSysNo;
                }

                if ((productDetail.ProductCategorySysno) > 0)
                {
                    sqlText += " and c.SysNos like '%," + productDetail.ProductCategorySysno + ",%' ";
                }

            }

            List<CBXinyingOutputPdProducts> outProducts = Context.Sql(sqlText).QueryMany<CBXinyingOutputPdProducts>();

            return outProducts;
        }


        /// <summary>
        /// 商品同步信息
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns>2017 010 10 罗勤瑶</returns>
        public override List<CBXinyingSynPdProductsB2B> GetXinYingSynProductList(List<int> sysNos, ParaProductFilter productDetail)
        {
            string sqlText = @"select  p.*
            from PdProduct p 
            inner join (select productsysno from PdCategoryAssociation  group by productsysno ) ca1 on ca1.productsysno = p.sysno 
            left join (select productsysno,categorysysno from PdCategoryAssociation where IsMaster=1) ca on ca.productsysno = p.sysno 
            left join  PdCategory c on ca.categorysysno = c.sysno 
            left join  PdBrand b on p.BrandSysNo = b.SysNo
            left join Origin o on p.OriginSysNo = o.SysNo
            left join  (select * from PdPrice where PriceSource =0 and SourceSysNo = 0 and [Status] = 1) price  on price.productsysno = p.sysno
            left join  (select * from PdPrice where PriceSource =10 and SourceSysNo = 1 and [Status] = 1) price1  on price1.productsysno = p.sysno
            left join  (select * from PdPrice where PriceSource =90 and SourceSysNo = 0 and [Status] = 1) price2  on price2.productsysno = p.sysno

            where p.Status != 2 ";

            if (sysNos != null && sysNos.Count > 0)
            {
                sqlText += " and p.SysNo in (" + string.Join(",", sysNos) + ")";
            }
            if (productDetail != null)
            {
                if (!string.IsNullOrEmpty(productDetail.ProductName))
                {
                    sqlText += " and p.ProductName like '%" + productDetail.ProductName.Trim().Replace(" ", "%") + "%'";
                }

                if (productDetail.Status > -1)
                {
                    sqlText += " and p.Status = " + productDetail.Status;
                }

                if (productDetail.CanFrontEndOrder > -1)
                {
                    sqlText += " and p.CanFrontEndOrder = " + productDetail.CanFrontEndOrder;
                }

                if (productDetail.IsFrontDisplay > -1)
                {
                    sqlText += " and p.IsFrontDisplay = " + productDetail.IsFrontDisplay;
                }

                if (!string.IsNullOrEmpty(productDetail.ErpCode))
                {
                    sqlText += " and p.ErpCode = '" + productDetail.ErpCode + "' ";
                }

                if (!string.IsNullOrEmpty(productDetail.Barcode))
                {
                    sqlText += " and p.Barcode = '" + productDetail.Barcode + "'";
                }

                if ((productDetail.ProductType) > 0)
                {
                    sqlText += " and p.ProductType = " + productDetail.ProductType;
                }

                if ((productDetail.StartTime) != null)
                {
                    sqlText += " and p.LastUpdateDate >= '" + productDetail.StartTime.Value.ToString("yyyy-MM-dd") + " 00:00:00'";
                }
                if ((productDetail.EndTime) != null)
                {
                    sqlText += " and p.LastUpdateDate <= '" + productDetail.EndTime.Value.ToString("yyyy-MM-dd") + " 23:59:59'";
                }

                if ((productDetail.CreateStartTime) != null)
                {
                    sqlText += " and p.CreatedDate >= '" + productDetail.CreateStartTime.Value.ToString("yyyy-MM-dd") + " 00:00:00'";
                }
                if ((productDetail.CreateEndTime) != null)
                {
                    sqlText += " and p.CreatedDate <= '" + productDetail.CreateEndTime.Value.ToString("yyyy-MM-dd") + " 23:59:59'";
                }

                if ((productDetail.OriginSysNo) > 0)
                {
                    sqlText += " and p.OriginSysNo = " + productDetail.OriginSysNo;
                }

                if ((productDetail.ProductCategorySysno) > 0)
                {
                    sqlText += " and c.SysNos like '%," + productDetail.ProductCategorySysno + ",%' ";
                }

            }

            List<CBXinyingSynPdProductsB2B> outProducts = Context.Sql(sqlText).QueryMany<CBXinyingSynPdProductsB2B>();

            return outProducts;
        }


        /// <summary>
        /// 查询导出商品列表(信营)
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns></returns>
        public override List<CBOutputPdProductsLijia> GetXinYingExportProductListLiJia(List<int> sysNos, ParaProductFilter productDetail)
        {
            string sqlText = @"select p.ErpCode AS 商品编码,p.ProductName AS 名称,c.CategoryName AS 商品分类,b.Name as 品牌,p.EasName AS 商品简称,
                    p.Volume AS 规格颜色,p.NetWeight AS 重量  
            from PdProduct p 
           
            left join (select productsysno,categorysysno from PdCategoryAssociation where IsMaster=1) ca on ca.productsysno = p.sysno 
            left join  PdCategory c on ca.categorysysno = c.sysno 
            left join  PdBrand b on p.BrandSysNo = b.SysNo
            left join Origin o on p.OriginSysNo = o.SysNo
            left join  (select * from PdPrice where PriceSource =0 and SourceSysNo = 0 and [Status] = 1) price  on price.productsysno = p.sysno
            left join  (select * from PdPrice where PriceSource =10 and SourceSysNo = 1 and [Status] = 1) price1  on price1.productsysno = p.sysno
            left join  (select * from PdPrice where PriceSource =90 and SourceSysNo = 0 and [Status] = 1) price2  on price2.productsysno = p.sysno

            where p.Status != 2 ";

            if (sysNos != null && sysNos.Count > 0)
            {
                sqlText += " and p.SysNo in (" + string.Join(",", sysNos) + ")";
            }
            if (productDetail != null)
            {
                if (!string.IsNullOrEmpty(productDetail.ProductName))
                {
                    sqlText += " and p.ProductName like '%" + productDetail.ProductName.Trim().Replace(" ", "%") + "%'";
                }

                if (productDetail.Status > -1)
                {
                    sqlText += " and p.Status = " + productDetail.Status;
                }

                if (productDetail.CanFrontEndOrder > -1)
                {
                    sqlText += " and p.CanFrontEndOrder = " + productDetail.CanFrontEndOrder;
                }

                if (productDetail.IsFrontDisplay > -1)
                {
                    sqlText += " and p.IsFrontDisplay = " + productDetail.IsFrontDisplay;
                }

                if (!string.IsNullOrEmpty(productDetail.ErpCode))
                {
                    sqlText += " and p.ErpCode = '" + productDetail.ErpCode + "' ";
                }

                if (!string.IsNullOrEmpty(productDetail.Barcode))
                {
                    sqlText += " and p.Barcode = '" + productDetail.Barcode + "'";
                }

                if ((productDetail.ProductType) > 0)
                {
                    sqlText += " and p.ProductType = " + productDetail.ProductType;
                }

                if ((productDetail.StartTime) != null)
                {
                    sqlText += " and p.LastUpdateDate >= '" + productDetail.StartTime.Value.ToString("yyyy-MM-dd") + " 00:00:00'";
                }
                if ((productDetail.EndTime) != null)
                {
                    sqlText += " and p.LastUpdateDate <= '" + productDetail.EndTime.Value.ToString("yyyy-MM-dd") + " 23:59:59'";
                }

                if ((productDetail.CreateStartTime) != null)
                {
                    sqlText += " and p.CreatedDate >= '" + productDetail.CreateStartTime.Value.ToString("yyyy-MM-dd") + " 00:00:00'";
                }
                if ((productDetail.CreateEndTime) != null)
                {
                    sqlText += " and p.CreatedDate <= '" + productDetail.CreateEndTime.Value.ToString("yyyy-MM-dd") + " 23:59:59'";
                }

                if ((productDetail.OriginSysNo) > 0)
                {
                    sqlText += " and p.OriginSysNo = " + productDetail.OriginSysNo;
                }

                if ((productDetail.ProductCategorySysno) > 0)
                {
                    sqlText += " and c.SysNos like '%," + productDetail.ProductCategorySysno + ",%' ";
                }

            }

            List<CBOutputPdProductsLijia> outProducts = Context.Sql(sqlText).QueryMany<CBOutputPdProductsLijia>();

            return outProducts;
        }

        /// <summary>
        /// 查询导出商品列表（无净重，商品简介）
        /// </summary>
        /// <param name="sysNos"></param>
        /// <param name="productDetail"></param>
        /// <returns></returns>
        public override List<CBOutputPdProductsExcel> GetExportProductListExcel(List<int> sysNos, ParaProductFilter productDetail = null)
        {
            string sqlText = @"select  p.SysNo AS 自动编码,p.ErpCode AS 商品编码,p.ProductName AS 前台显示名称,p.EasName AS 后台显示名称,c.CategoryName AS 分类,b.Name as 品牌,case p.ProductType when 10 then '普通商品' when 20 then '虚拟商品' when 30 then '保税商品' when 40 then '直邮商品' ELSE '完税商品' END as 类型,
                      o.Origin_Name AS 原产地,p.Barcode AS 条形码,p.GrosWeight AS 毛重,p.Tax AS 税率,p.PriceRate AS 直营利润比例,p.PriceValue AS 直营分销商利润金额,price.Price as 商品价格, price1.Price as 会员价,p.TradePrice as 批发价, price2.Price as 门店销售价,p.ProductShortTitle as 商品简称
            from PdProduct p 
            
            left join (select productsysno,categorysysno from PdCategoryAssociation where IsMaster=1) ca on ca.productsysno = p.sysno 
            left join  PdCategory c on ca.categorysysno = c.sysno 
            left join  PdBrand b on p.BrandSysNo = b.SysNo
            left join Origin o on p.OriginSysNo = o.SysNo
            left join  (select * from PdPrice where PriceSource =0 and SourceSysNo = 0 and [Status] = 1) price  on price.productsysno = p.sysno
            left join  (select * from PdPrice where PriceSource =10 and SourceSysNo = 1 and [Status] = 1) price1  on price1.productsysno = p.sysno
            left join  (select * from PdPrice where PriceSource =90 and SourceSysNo = 0 and [Status] = 1) price2  on price2.productsysno = p.sysno

            where p.Status != 2 ";//inner join (select productsysno from PdCategoryAssociation  group by productsysno ) ca1 on ca1.productsysno = p.sysno 

            if (sysNos.Count > 0)
            {
                sqlText += " and p.SysNo in (" + string.Join(",", sysNos) + ")";
            }
            if (productDetail != null)
            {
                if (!string.IsNullOrEmpty(productDetail.ProductName))
                {
                    sqlText += " and p.ProductName like '%" + productDetail.ProductName.Trim().Replace(" ", "%") + "%'";
                }

                if (productDetail.Status > -1)
                {
                    sqlText += " and p.Status = " + productDetail.Status;
                }

                if (productDetail.CanFrontEndOrder > -1)
                {
                    sqlText += " and p.CanFrontEndOrder = " + productDetail.CanFrontEndOrder;
                }

                if (productDetail.IsFrontDisplay > -1)
                {
                    sqlText += " and p.IsFrontDisplay = " + productDetail.IsFrontDisplay;
                }

                if (!string.IsNullOrEmpty(productDetail.ErpCode))
                {
                    sqlText += " and p.ErpCode = " + productDetail.ErpCode;
                }

                if (!string.IsNullOrEmpty(productDetail.Barcode))
                {
                    sqlText += " and p.Barcode = " + productDetail.Barcode;
                }

                if ((productDetail.ProductType) > 0)
                {
                    sqlText += " and p.ProductType = " + productDetail.ProductType;
                }

                if ((productDetail.StartTime) != null)
                {
                    sqlText += " and p.LastUpdateDate >= '" + productDetail.StartTime.Value.ToString("yyyy-MM-dd") + " 00:00:00'";
                }
                if ((productDetail.EndTime) != null)
                {
                    sqlText += " and p.LastUpdateDate <= '" + productDetail.EndTime.Value.ToString("yyyy-MM-dd") + " 23:59:59'";
                }

                if ((productDetail.CreateStartTime) != null)
                {
                    sqlText += " and p.CreatedDate >= '" + productDetail.CreateStartTime.Value.ToString("yyyy-MM-dd") + " 00:00:00'";
                }
                if ((productDetail.CreateEndTime) != null)
                {
                    sqlText += " and p.CreatedDate <= '" + productDetail.CreateEndTime.Value.ToString("yyyy-MM-dd") + " 23:59:59'";
                }

                if ((productDetail.OriginSysNo) > 0)
                {
                    sqlText += " and p.OriginSysNo = " + productDetail.OriginSysNo;
                }

                if ((productDetail.ProductCategorySysno) > 0)
                {
                    sqlText += " and c.SysNos like '%," + productDetail.ProductCategorySysno + ",%' ";
                }

            }

            List<CBOutputPdProductsExcel> outProducts = Context.Sql(sqlText).QueryMany<CBOutputPdProductsExcel>();

            return outProducts;
        }

        /// <summary>
        /// 获取指定条形码的商品信息
        /// </summary>
        /// <param name="ErpCode">商品编码</param>
        /// <param name="Barcode">条形码</param>
        /// <returns>商品实体信息</returns>
        /// <remarks>2016-03-28 王耀发 创建</remarks>
        public override PdProduct GetEntityByBarcode(string ErpCode, string Barcode)
        {
            PdProduct entity = Context.Select<PdProduct>(" distinct *")
                .From("PdProduct")
                .Where("ErpCode <> @ErpCode and Barcode = @Barcode and Status <> 2")
                .Parameter("ErpCode", ErpCode)
                .Parameter("Barcode", Barcode)
                .QuerySingle();
            return entity;
        }
        /// <summary>
        /// 获取指定条形码的商品信息
        /// </summary>
        /// <param name="ErpCode">商品编码</param>
        /// <param name="Barcode">条形码</param>
        /// <returns>商品实体信息</returns>
        /// <remarks>2016-03-28 王耀发 创建</remarks>
        public override PdProduct GetEntityByBarcode( string Barcode)
        {
            return Context.Sql("select * from pdproduct where Barcode = @Barcode")
                               .Parameter("Barcode", Barcode)
                               .QuerySingle<PdProduct>();
        }
        /// <summary>
        /// 获取指定商品编码的商品信息
        /// </summary>
        /// <param name="ErpCode">商品编码</param>
        /// <returns>商品实体信息</returns>
        /// <remarks>2016-03-28 王耀发 创建</remarks>
        public override PdProduct GetEntityByErpCode(string ErpCode)
        {
            PdProduct entity = Context.Select<PdProduct>("*")
                .From("PdProduct")
                .Where("ErpCode=@ErpCode and Status <> 2")
                .Parameter("ErpCode", ErpCode)
                .OrderBy(" SysNo desc ")
                .QuerySingle();
            return entity;
        }
        /// <summary>
        /// 获取指定商品编码的商品信息利嘉版
        /// </summary>
        /// <param name="ErpCode">商品编码</param>
        /// <returns>商品实体信息</returns>
        /// <remarks>2017-05-28 罗勤尧 创建</remarks>
        public override CBOutputPdProductsLijia GetEntityLiJiaByErpCode(string ErpCode)
        {

            string sqlText = @"select p.ErpCode AS 商品编码,p.ProductName AS 名称,c.CategoryName AS 商品分类,b.Name as 品牌,p.EasName AS 商品简称,
                    p.Volume AS 规格颜色,p.NetWeight AS 重量  
            from PdProduct p 
           
            left join (select productsysno,categorysysno from PdCategoryAssociation where IsMaster=1) ca on ca.productsysno = p.sysno 
            left join  PdCategory c on ca.categorysysno = c.sysno 
            left join  PdBrand b on p.BrandSysNo = b.SysNo
            left join Origin o on p.OriginSysNo = o.SysNo
            left join  (select * from PdPrice where PriceSource =0 and SourceSysNo = 0 and [Status] = 1) price  on price.productsysno = p.sysno
            left join  (select * from PdPrice where PriceSource =10 and SourceSysNo = 1 and [Status] = 1) price1  on price1.productsysno = p.sysno
            left join  (select * from PdPrice where PriceSource =90 and SourceSysNo = 0 and [Status] = 1) price2  on price2.productsysno = p.sysno

            where p.Status != 2 ";
            if (!string.IsNullOrEmpty(ErpCode))
            {
                sqlText += " and p.ErpCode = '" + ErpCode + "' ";
            }
            CBOutputPdProductsLijia entity = Context.Sql(sqlText).QuerySingle<CBOutputPdProductsLijia>();
             
            return entity;
        }
        /// <summary>
        /// 获取南沙商品备案信息
        /// </summary>
        /// <param name="productList">商品列表</param>
        /// <returns>>返回备案信息</returns>
        /// <remarks>2016-4-4 王耀发  实现功能</remarks>
        public override IList<IcpGZNanShaGoodsInfo> GetIcpGZNanShaGoodsInfoList(IList<int> productList)
        {
            return Context.Select<IcpGZNanShaGoodsInfo>("p.*")
                          .From(@"IcpGZNanShaGoodsInfo p")
                          .Where(@"" + (productList.Count > 0 ? " p.ProductSysNo in (" + productList.Join(",") + ")" : "1=2") + @"")
                          .OrderBy("p.ProductSysNo desc")
                          .QueryMany();
        }
        /// <summary>
        /// 创建商品信息
        /// </summary>
        /// <param name="model">商品信息</param>
        /// <returns>是否创建成功</returns>
        /// <remarks>2016-04-25 王耀发 创建</remarks>
        public override int CreateProduct(PdProduct model)
        {
            int ProductSysNo = Context.Insert<PdProduct>("PdProduct", model)
                       .AutoMap(o => o.SysNo, o => o.Stamp)
                       .ExecuteReturnLastId<int>("SysNo");
            return ProductSysNo;
        }

        /// <summary>
        /// 通过商品编号集合获取商品数据详情
        /// </summary>
        /// <param name="proIdList">商品编号集合</param>
        /// <returns></returns>
        public override IList<CBPdProduct> GetProductInfoList(IList<int> proIdList)
        {
            if (proIdList.Count > 0)
            {
                string sql = "select pdproduct.*,IcpGZNanShaGoodsInfo.CIQGoodsNo,IcpGZNanShaGoodsInfo.CUSGoodsNo from pdproduct left join IcpGZNanShaGoodsInfo on IcpGZNanShaGoodsInfo.ProductSysNo=pdproduct.SysNo";
                sql += " where pdproduct.SysNo in (" + string.Join(",", proIdList) + ") ";
                return Context.Sql(sql).QueryMany<CBPdProduct>();
            }
            else
            {
                return new List<CBPdProduct>();
            }
        }

        /// <summary>
        /// 获取商品所属分类
        /// </summary>
        /// <param name="productSysNoList"></param>
        /// <returns></returns>
        /// <remarks>2016-05-06 陈海裕 创建</remarks>
        public override IList<int> GetProductsCategories(IList<int> productSysNoList)
        {
            if (productSysNoList.Count > 0)
            {
                string sqlText = "SELECT DISTINCT CategorySysNo FROM PdCategoryAssociation WHERE ProductSysNo IN (" + string.Join(",", productSysNoList) + ")";
                return Context.Sql(sqlText).QueryMany<int>();
            }
            else
            {
                return new List<int>();
            }
        }
        /// <summary>
        /// 通过条码获取商品
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <returns>商品实体</returns>
        public override PdProduct GetProductByBarcode(string barcode)
        {
            string sql = "select * from PdProduct where Barcode = '" + barcode + "' ";
            return Context.Sql(sql).QuerySingle<PdProduct>();
        }
        /// <summary>
        /// 更新商品档案中的时间和用户
        /// </summary>
        /// <param name="productSysNo">商品编号</param>
        /// <param name="dateTime">更新时间</param>
        /// <param name="UserSysNo">更新人员</param>
        /// <remarks>2016-06-16 杨云奕 创建</remarks>
        public override void UpdateLastTimeOrUser(int productSysNo, DateTime dateTime, int UserSysNo)
        {
            string sql = " UPDATE PdProduct set LastUpdateBy='" + UserSysNo + "',LastUpdateDate='" + dateTime.ToString() + "' where  SysNo = '" + productSysNo + "' ";
            Context.Sql(sql).Execute();
        }
        /// <summary>
        /// 分页查询商品条码列表
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        /// <remarks>2016-08-31 周 创建</remarks>
        public override Pager<PdProductBarcode> GetPdProductBarcodeList(Pager<PdProductBarcode> pager)
        {
            #region sql条件
            string sql = "";
            if (pager.PageFilter.Barcode!="")
            {
                sql = "Barcode like '%" + pager.PageFilter.Barcode + "%'";
            }
            #endregion

            using (var _context = Context.UseSharedConnection(true))
            {

                pager.Rows = _context.Select<PdProductBarcode>("p.*")
                              .From("PdProductBarcode p")
                              .Where(sql)
                              //.Parameter("Barcode", pager.PageFilter.Barcode)
                              //.Parameter("Barcode1", "%" + pager.PageFilter.Barcode + "%")
                              .OrderBy(" SysNo desc ")
                              .Paging(pager.CurrentPage, pager.PageSize)
                              .QueryMany();

                pager.TotalRows = _context.Select<int>("count(1)")
                              .From("PdProductBarcode")
                              .Where(sql)
                              //.Parameter("Barcode", pager.PageFilter.Barcode)
                              //.Parameter("Barcode1", "%" + pager.PageFilter.Barcode + "%")
                              .QuerySingle();
            }
            return pager;
        }
        /// <summary>
        /// 查询是否重复
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override bool IsExistsProductBarcode(PdProductBarcode model)
        {
            bool result = false;
            PdProductBarcode entity = Context.Select<PdProductBarcode>("*")
                .From("PdProductBarcode")
                .Where("Barcode= @Barcode and sysno != @sysno")
                .Parameter("Barcode", model.Barcode)
                .Parameter("sysno", model.SysNo)
                .QuerySingle();

            if (entity != null && entity.SysNo > 0)
            {
                result = true;
            }
            return result;
        }
        /// <summary>
        /// 更新商品条码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override bool UpdateProductBarcode(PdProductBarcode model)
        {
            var r = Context.Update("PdProductBarcode", model)
                           .AutoMap(o => o.SysNo)
                           .Where("SysNo", model.SysNo).Execute();
            return r > 0;

        }
        /// <summary>
        /// 创建商品条码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override int CreateProductBarcode(PdProductBarcode model)
        {
            int sysno = 0;
            sysno = Context.Insert<PdProductBarcode>("PdProductBarcode", model)
                        .AutoMap(x => x.SysNo)
                        .ExecuteReturnLastId<int>("Sysno");
            return sysno;
        }
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public override PdProductBarcode GetProductBarcodeEntity(int sysNo)
        {
            PdProductBarcode entity = Context.Select<PdProductBarcode>("*")
                .From("PdProductBarcode")
                .Where("sysno = @sysno")
                .Parameter("sysno", sysNo)
                .QuerySingle();
            return entity;
        }
        /// <summary>
        /// 条码在商品列表中是否存在
        /// </summary>
        /// <param name="Barcode"></param>
        /// <returns></returns>
        public override int IsExistsPdProductBarcode(string Barcode)
        {
            string sql = "select count(1) from PdProduct where Barcode='" + Barcode + "'";
            return Context.Sql(sql)
                .QuerySingle<int>();
        }
        /// <summary>
        /// 分页查询条形码列表，商品已存在条码不显示
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public override Pager<PdProductBarcode> BarcodeQuery(string keyword, int currentPage, int pageSize)
        {
            string where = " sysno not in(select ba.sysno from PdProductBarcode ba inner join PdProduct pd on ba.Barcode=pd.Barcode)";
            if (keyword!="")
            {
                where += " and Barcode like '%" + keyword + "%'";
            }

            var dataList = Context.Select<PdProductBarcode>("*").From(" PdProductBarcode ").Where(where);
            var dataCount = Context.Select<int>("count(0)").From(" PdProductBarcode ").Where(where);

            var pager = new Pager<PdProductBarcode>
            {
                PageSize = pageSize,
                CurrentPage = currentPage,
                TotalRows = dataCount.QuerySingle(),
                Rows = dataList.OrderBy("SysNo desc").Paging(currentPage, pageSize).QueryMany()
            };

            return pager;
        }
        /// <summary>
        /// 通过条码获取条形码详情
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <returns>商品实体</returns>
        public override PdProductBarcode GetProductBarcodeByBarcode(string barcode)
        {
            string sql = "select * from PdProductBarcode where Barcode = '" + barcode + "' ";
            return Context.Sql(sql).QuerySingle<PdProductBarcode>();
        }
        /// <summary>
        /// 商品列表
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>

        public override Pager<PdProduct> ProductListQuery(string keyword, int currentPage, int pageSize)
        {
            string where = " sysno not in(select ProductSysNo from PdProductBarcode) and Status=1 and CanFrontEndOrder=1";
            if (keyword != "")
            {
                where += " and ProductName like '%" + keyword + "%'";
            }

            var dataList = Context.Select<PdProduct>("*").From(" PdProduct ").Where(where);
            var dataCount = Context.Select<int>("count(0)").From(" PdProduct ").Where(where);

            var pager = new Pager<PdProduct>
            {
                PageSize = pageSize,
                CurrentPage = currentPage,
                TotalRows = dataCount.QuerySingle(),
                Rows = dataList.OrderBy("SysNo desc").Paging(currentPage, pageSize).QueryMany()
            };

            return pager;
        }

        #region 供应链商品数据
        /// <summary>
        /// 添加供应链商品数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override int CreatePdProductForSupplyChain(PdProductForSupplyChain model)
        {
            int id = Context.Insert<PdProductForSupplyChain>("PdProductForSupplyChain", model)
                     .AutoMap(x => x.SysNo)
                     .ExecuteReturnLastId<int>("Sysno");
            return id;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="SupplyChainCode"></param>
        /// <returns></returns>
        public override PdProductForSupplyChain GetPdProductForSupplyChainEntity(int SupplyChainCode)
        {
            PdProductForSupplyChain entity = Context.Select<PdProductForSupplyChain>("*")
                .From("PdProductForSupplyChain")
                .Where("SupplyChainCode = @SupplyChainCode")
                .Parameter("SupplyChainCode", SupplyChainCode)
                .QuerySingle();
            return entity;
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override bool UpdatePdProductForSupplyChain(PdProductForSupplyChain model)
        {
            int effect = Context.Update<PdProductForSupplyChain>("PdProductForSupplyChain", model)
                .AutoMap(x => x.SupplyChainCode)
                .Where("SupplyChainCode", model.SupplyChainCode)
                .Execute();
            return effect > 0;
        }
        #endregion


        /// <summary>
        /// 批量更新产品状态
        /// </summary>
        /// <param name="productSysNoList">产品系统编号列表</param>
        /// <param name="status">产品状态</param>
        /// <param name="dealerSysNo">经销商系统编号</param>
        /// <returns></returns>
        /// <remarks>2016-10-09 杨浩 创建</remarks>
        public override int BatchUpdateProductStatus(string productSysNoList, int status, int dealerSysNo)
        {
            string sqlWhere = " where 1=1 ";
            if (!string.IsNullOrWhiteSpace(productSysNoList))
                sqlWhere+= " and SysNo in (" + productSysNoList + ")";
            if(dealerSysNo>=0)
                sqlWhere += " and DealerSysNo=" + dealerSysNo;

            var r = Context.Sql("update PdProduct set [Status]=" + status + sqlWhere).Execute();
            return r ;
        }

        public override List<PdProduct> GetAllProductDataBase()
        {
            string sql = " select * from  PdProduct";
            return Context.Sql(sql).QueryMany<PdProduct>();
        }

        /// <summary>
        /// 根据产品系统编号列列表获取产品
        /// </summary>
        /// <param name="productSysnoList">产品系统编号列表</param>
        /// <returns></returns>
        /// <remarks>2017-06-30 杨浩 创建</remarks>
        public override IList<PdProduct> GetProductListBySysnoList(List<int> productSysnoList)
        {
            string sql = "select ErpCode,SysNo from  PdProduct where sysNo in("+string.Join(",",productSysnoList)+")";
            return Context.Sql(sql).QueryMany<PdProduct>();
        }

        /// <summary>
        /// 根据ErpCode获取产品列表
        /// </summary>
        /// <param name="productErpCodes">产品编码集合</param>
        /// <returns></returns>
        /// <remarks>2017-10-18 杨浩 创建</remarks>
        public override IList<PdProduct> GetProductListByErpCode(IList<string> productErpCodes)
        {           
            string inStr = "";
            string[] erpCodes = new string[productErpCodes.Count];
            for (int i=0;i<productErpCodes.Count;i++)
            {
                if (inStr != "")
                    inStr += ",";
                inStr += "@" + i; 
            
                erpCodes[i] = productErpCodes[i];
            }
      
            string sql = "select ErpCode,SysNo,ProductName from  PdProduct where ErpCode in ( "+inStr+")";
            return Context.Sql(sql)
                .Parameters(erpCodes)
                .QueryMany<PdProduct>();
        }

        /// <summary>
        /// 根据商品与仓库获取待配送的商品数量
        /// </summary>
        /// <returns></returns>
        public override int GetPdPending(int pdSysNo,int whSysNo)
        {
            return Context.Sql(@"select IsNull(sum(ProductQuantity),0) from WhStockOutItem where 
ProductSysNo = @0 
and StockOutSysNo in(
select SysNo from WhStockOut  where (status=20 or status=30 or status=40) and WarehouseSysNo=@1)", pdSysNo, whSysNo)
            .QuerySingle<int>();
           
        }
    }
}