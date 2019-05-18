using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.DataAccess.Base;
using Hyt.DataAccess.Web;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Oracle.Web
{
    /// <summary>
    /// 前台商品信息
    /// </summary>
    /// <remarks>2013-08-14 邵斌 创建</remarks>
    public class PdProductDaoImpl : IPdProductDao
    {

        /// <summary>
        /// 获取产品详情
        /// </summary>
        /// <param name="productSysNo">产品系统编号</param>
        /// <returns></returns>
        /// <remarks>2016-9-12 杨浩 创建</remarks>
        public override PdProduct GetProductInfo(int productSysNo)
        {
            return Context.Sql(string.Format("select * from pdProduct where sysNo={0}", productSysNo))
                .QuerySingle<PdProduct>();
        }
        /// <summary>
        /// 根据商品系统编号获取商品信息
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <returns>商品信息</returns>
        /// <remarks>2013-08-14 邵斌 创建</remarks>
        public override CBSimplePdProduct GetProduct(int productSysNo)
        {
            using (var context = Context.UseSharedConnection(true))
            {
                #region 获取基本信息

                #region 测试SQL
                /*
                select 
                    p.SysNo,p.BrandSysNo,p.ErpCode,p.Barcode,p.QrCode,p.ProductType,p.ProductName,p.ProductSubName,p.NameAcronymy,p.ProductShortTitle,p.ProductSummary,p.ProductSlogan,p.PackageDesc,p.ProductDesc,p.ProductImage,p.ViewCount,p.SeoTitle,p.SeoKeyword,p.SeoDescription,p.Status,p.DisplayOrder,pb.name as BrandName
                from 
                    PdProduct p
                    left join PdBrand pb on pb.sysno = p.brandsysno
                where 
                    p.SysNo=1 and p.canfrontendorder = 1
                 */
                #endregion

                var baseProduct = context.Sql(@"
                    select 
                        p.SysNo,p.BrandSysNo,p.ErpCode,p.Barcode,p.QrCode,p.ProductType,p.EasName,p.GrosWeight,p.NetWeight,p.SalesAddress,p.ProductName,p.ProductSubName,p.NameAcronymy,p.ProductShortTitle,p.ProductSummary,p.ProductSlogan,p.PackageDesc,p.ProductDesc,p.ProductImage,p.ViewCount,p.SeoTitle,p.SeoKeyword,p.SeoDescription,p.Status,p.DisplayOrder,pb.name as BrandName,p.CanFrontEndOrder,p.Volume,o.Origin_Name,p.AgentSysNo 
                    from 
                        PdProduct p
                        left join PdBrand pb on pb.sysno = p.brandsysno
                        left join Origin o on p.OriginSysNo = o.SysNo
                    where 
                        p.SysNo=@0
                ", productSysNo)
                 .QuerySingle<CBSimplePdProduct>();

                if (baseProduct == null)
                    return baseProduct;

                #endregion

                #region 读取商品评论信息

                #region  测试SQL 读取商品评论信息：只已审核的有效评论
                /*
                    select count(score) as ProductCommentScoreTotal,count(sysno) as CommentTimesCount, from FeProductComment fpc where fpc.productsysno = 1 and CommentStatus = 20
                */
                #endregion

                //查询评论信息
                var commetInof = context.Sql(@"
                    select isnull(sum(score),0) as SCORETOTAL,count(sysno) as TIMESCOUNT from FeProductComment fpc where fpc.productsysno = @0 and CommentStatus = @1
                ", productSysNo, (int)ForeStatus.商品评论状态.已审).QuerySingle<dynamic>();

                //绑定数据
                baseProduct.CommentTimesCount = commetInof.TIMESCOUNT;
                baseProduct.CommentScoreTotal = commetInof.SCORETOTAL;
                if (baseProduct.CommentScoreTotal != 0)
                    baseProduct.CommentScore = baseProduct.CommentScoreTotal / baseProduct.CommentTimesCount;

                #endregion

                #region 读取商品价格

                #region 测试SQL 只读取有效价格
                /*
                select * from pdprice pr where pr.productsysno=1 and pr.status = 1 order by pr.pricesource,pr.sourcesysno   --查询状态为可用的商品价格并按商品价格类型排序
                */
                #endregion

                baseProduct.Prices = context.Sql(@"
                                select * from pdprice pr where pr.productsysno=@0 and pr.status = @1 order by pr.pricesource,pr.sourcesysno
                            ", productSysNo, (int)ProductStatus.产品价格状态.有效).QueryMany<CBPdPrice>();

                //格式化价格名称
                foreach (var price in baseProduct.Prices)
                {
                    price.PriceName = "";// Enum.GetName(typeof(ProductStatus.产品价格来源), price.PriceSource);
                }

                #endregion

                #region 读取商品分类信息

                #region 测试SQL 只读取有效分类（状态为有效的）

                /*
                select c.*,ca.ismaster from 
                    pdcategory c 
                    inner join pdcategoryassociation ca on c.sysno = ca.categorysysno
                where 
                    ca.productsysno=1 and c.status = 1
                    order by ca.ismaster desc,c.DisplayOrder asc
                 * */

                baseProduct.Categories = context.Sql(@"
                select c.*,ca.ismaster from 
                    pdcategory c 
                    inner join pdcategoryassociation ca on c.sysno = ca.categorysysno
                where 
                    ca.productsysno=@0 
                    order by ca.ismaster desc,c.DisplayOrder asc
                ", productSysNo).QueryMany<PdCategory>();

                #endregion

                #endregion

                #region 读取商品图片

                #region 测试SQL

                /*
                 select * from PdProductImage where productsysno=1
                 */

                baseProduct.Images = context.Sql(@"select imageurl from PdProductImage where productsysno=@0  order by DisplayOrder", productSysNo).QueryMany<string>();

                #endregion

                #endregion

                return baseProduct;
            }
        }



        /// <summary>
        /// 根据商品ErpCode获取商品信息
        /// </summary>
        /// <param name="ErpCode">商品编号</param>
        /// <param name="Barcode">条形码</param>
        /// <returns>商品信息</returns>
        /// <remarks>2017-07-03 吴琨 创建</remarks>
        public override CBSimplePdProduct GetProductErpCode(string erpCode, string Barcode)
        {
            using (var context = Context.UseSharedConnection(true))
            {
                #region 获取基本信息
                var baseProduct = context.Sql(@"
                    select 
                        p.SysNo,p.BrandSysNo,p.ErpCode,p.Barcode,p.QrCode,p.ProductType,p.EasName,p.GrosWeight,p.NetWeight,p.SalesAddress,p.ProductName,p.ProductSubName,p.NameAcronymy,p.ProductShortTitle,p.ProductSummary,p.ProductSlogan,p.PackageDesc,p.ProductDesc,p.ProductImage,p.ViewCount,p.SeoTitle,p.SeoKeyword,p.SeoDescription,p.Status,p.DisplayOrder,pb.name as BrandName,p.CanFrontEndOrder,p.Volume,o.Origin_Name,p.AgentSysNo 
                    from 
                        PdProduct p
                        left join PdBrand pb on pb.sysno = p.brandsysno
                        left join Origin o on p.OriginSysNo = o.SysNo
                    where p.ErpCode=@0 or p.Barcode=@1
                ", (string.IsNullOrEmpty(erpCode) ? null : erpCode.Trim()), (string.IsNullOrEmpty(Barcode) ? null : Barcode.Trim()))
                 .QuerySingle<CBSimplePdProduct>();
                if (baseProduct == null)
                    return baseProduct;
                #endregion

                #region 读取商品评论信息
                //查询评论信息
                var commetInof = context.Sql(@"
                    select isnull(sum(score),0) as SCORETOTAL,count(sysno) as TIMESCOUNT from FeProductComment fpc where fpc.productsysno =(select sysNo from PdProduct  where ErpCode=@0) and CommentStatus = @1
                ", erpCode, (int)ForeStatus.商品评论状态.已审).QuerySingle<dynamic>();

                //绑定数据
                baseProduct.CommentTimesCount = commetInof.TIMESCOUNT;
                baseProduct.CommentScoreTotal = commetInof.SCORETOTAL;
                if (baseProduct.CommentScoreTotal != 0)
                    baseProduct.CommentScore = baseProduct.CommentScoreTotal / baseProduct.CommentTimesCount;

                #endregion


                #region 读取商品价格

                #region 测试SQL 只读取有效价格
                /*
                select * from pdprice pr where pr.productsysno=1 and pr.status = 1 order by pr.pricesource,pr.sourcesysno   --查询状态为可用的商品价格并按商品价格类型排序
                */
                #endregion

                baseProduct.Prices = context.Sql(@"
                                select * from pdprice pr  where pr.productsysno=(select sysNo from PdProduct  where ErpCode=@0) and pr.status = @1 order by pr.pricesource,pr.sourcesysno
                            ", erpCode, (int)ProductStatus.产品价格状态.有效).QueryMany<CBPdPrice>();

                //格式化价格名称
                foreach (var price in baseProduct.Prices)
                {
                    price.PriceName = "";// Enum.GetName(typeof(ProductStatus.产品价格来源), price.PriceSource);
                }

                #endregion

                #region 读取商品分类信息

                #region 测试SQL 只读取有效分类（状态为有效的）

                baseProduct.Categories = context.Sql(@"
                select c.*,ca.ismaster from 
                    pdcategory c 
                    inner join pdcategoryassociation ca on c.sysno = ca.categorysysno
                where 
                    ca.productsysno=(select sysNo from PdProduct  where ErpCode=@0)
                    order by ca.ismaster desc,c.DisplayOrder asc
                ", erpCode).QueryMany<PdCategory>();

                #endregion

                #endregion

                #region 读取商品图片

                #region 测试SQL

                /*
                 select * from PdProductImage where productsysno=1
                 */

                baseProduct.Images = context.Sql(@"select imageurl from PdProductImage where productsysno=(select sysNo from PdProduct  where ErpCode=@0)  order by DisplayOrder", erpCode).QueryMany<string>();

                #endregion

                #endregion

                return baseProduct;
            }
        }





        /// <summary>
        /// 获取同类商品的前5个商品
        /// </summary>
        /// <param name="categorySysNo">商品分类系统编号</param>
        /// <param name="excludeProductSysNo">商品系统编号:为避免推荐时自身又出现在推荐列表中,若有冗余排除</param>
        /// <param name="topNum">前N个商品</param>
        /// <returns>商品列表</returns>
        /// <remarks>2013-08-07 邵斌 创建</remarks>
        public override IList<CBPdProductDetail> GetProductFromCategory(int categorySysNo, int excludeProductSysNo, int topNum)
        {

            #region 测试SQL
            /*
                select
                    p.sysno,p.productname,p.productimage,pr.price as BasicPrice
                from
                    PdCategoryAssociation pc
                    left join
                        (select sum(score) as tscore,productsysno from FeProductComment  where productsysno <> 2 group by productsysno order by tscore desc) fpc on fpc.productsysno = pc.productsysno
                    left join pdproduct p on p.sysno = pc.productsysno
                    left join pdprice pr on pr.productsysno = pc.productsysno  
                where
                    pc.categorysysno = 32 and pr.pricesource = 0 and rownum < 6  and pr.status= 1
             */
            #endregion

            return Context.Sql(@"select
                                    p.sysno,p.productname,p.productimage,pr.price as BasicPrice
                                from
                                    PdCategoryAssociation pc
                                    left join
                                        (select sum(score) as tscore,productsysno from FeProductComment where productsysno <> @0 group by productsysno order by tscore desc) fpc on fpc.productsysno = pc.productsysno
                                    left join pdproduct p on p.sysno = pc.productsysno
                                    left join pdprice pr on pr.productsysno = pc.productsysno  
                                where
                                    pc.categorysysno = @1 and pr.pricesource = @2 and pr.status=@3 and rownum < @4 ", excludeProductSysNo, categorySysNo, (int)ProductStatus.产品价格来源.基础价格, (int)ProductStatus.产品价格状态.有效, topNum + 1).QueryMany<CBPdProductDetail>();
        }

        /// <summary>
        /// 获取好评的商品
        /// </summary>
        /// <param name="excludeProductSysNo">要排除的商品系统编号</param>
        /// <param name="topNum">前N个商品</param>
        /// <returns>商品列表</returns>
        /// <remarks>2013-08-07 邵斌 创建</remarks>
        public override IList<CBPdProductDetail> GetBestProductComment(int excludeProductSysNo, int topNum)
        {
            #region 测试SQL
            /*
                select 
                    p.sysno,p.productname,p.productimage,pr.price as BasicPrice
                from
                    pdProduct p
                    inner join (select sum(score) as totalScore,ProductSysNo from FeProductComment group by ProductSysNo order by totalScore desc) fpc on p.sysno = fpc.productsysno
                    inner join pdprice pr on p.sysno = pr.productsysno
                where
                    pr.pricesource=0 and p.sysno<>1 and rownum<3
             */
            #endregion

            return Context.Sql(@"
                select 
                    p.sysno,p.productname,p.productimage,pr.price as BasicPrice
                from
                    pdProduct p
                    inner join (select sum(score) as totalScore,ProductSysNo from FeProductComment group by ProductSysNo order by totalScore desc) fpc on p.sysno = fpc.productsysno
                    inner join pdprice pr on p.sysno = pr.productsysno
                where
                    pr.pricesource=@0 and p.sysno<>@1  and pr.status = @3 and rownum<@3
                            ", (int)ProductStatus.产品价格来源.基础价格, excludeProductSysNo, (int)ProductStatus.产品价格状态.有效, topNum + 1).QueryMany<CBPdProductDetail>();
        }

        /// <summary>
        /// 购买了同一商品的人还买了其他那些商品
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <param name="customerLevelSysNo">会员等级系统编号</param>
        /// <param name="recordNum">记录数</param>
        /// <returns>商品列表</returns>
        /// <remarks>2013-08-08 邵斌 创建</remarks>
        public override IList<CBPdProductDetail> GetOtherBuy(int productSysNo, int customerLevelSysNo, int recordNum)
        {
            #region 测试SQL
            /*
                    select
                     p.sysno,p.productname,p.productimage,pr.price  as BasicPrice ,cl.levelname as PriceName
                    from   
                      (
                        --查找买了同一产品的人同时（同一订单）还买了些其他什么东西
                        select
                         soi2.productsysno,max(so.createdate) as maxCreateDate
                        from
                         SoorderItem soI
                         inner join Soorder so on so.SYSNO = soI.Ordersysno
                         inner join Soorderitem soi2 on soi2.ordersysno = so.sysno and soi2.productsysno <> 1
                        where
                         soI.productsysno=1 and so.customersysno > 0 and so.status=100 and rownum < 5
                        group by soi2.productsysno
                        order by maxCreateDate desc
                       ) t1
                       inner join pdproduct p on p.sysno = t1.productsysno
                       inner join pdprice pr on pr.productsysno = p.sysno
                       inner join CrCustomerLevel cl on cl.sysno = pr.sourcesysno
                     where
                      pr.pricesource=10 and pr.status = 1 and pr.sourcesysno=1
                     order by p.sysno
             */
            #endregion

            IList<CBPdProductDetail> result = new List<CBPdProductDetail>();
            //读取买个同样商品的人同时还买了那些东西
            using (var context = Context.UseSharedConnection(true))
            {
                result = context.Sql(@"
                        select
                         p.sysno,p.productname,p.productimage,pr.price as BasicPrice, cl.levelname as PriceName
                        from   
                          (
                            --查找买了同一产品的人同时（同一订单）还买了些其他什么东西
                            select
                             soi2.productsysno,max(so.createdate) as maxCreateDate
                            from
                             SoorderItem soI
                             inner join Soorder so on so.SYSNO = soI.Ordersysno
                             inner join Soorderitem soi2 on soi2.ordersysno = so.sysno and soi2.productsysno <> 1
                            where
                             soI.productsysno=@ProductSysno and so.customersysno > 0 and so.status=@OrderStatus and rownum < @RecordNum
                            group by soi2.productsysno
                            order by maxCreateDate desc
                           ) t1
                           inner join pdproduct p on p.sysno = t1.productsysno
                           inner join pdprice pr on pr.productsysno = p.sysno
                           inner join CrCustomerLevel cl on cl.sysno = pr.sourcesysno
                         where
                          pr.pricesource=@PriceSource and pr.status = @PriceStatus and pr.sourcesysno=@levelSysNo
                         order by p.sysno
                    ", new object[]
                    {
                       productSysNo,
                       (int)OrderStatus.销售单状态.已完成,
                        recordNum+1,
                        (int)ProductStatus.产品价格来源.会员等级价,
                        (int)ProductStatus.产品价格状态.有效,
                        customerLevelSysNo
                    }).QueryMany<CBPdProductDetail>();

                //如果记录不够recordNum设置，将启用默认推荐
                if (result.Count < recordNum)
                {
                    var tempList = context.Sql(@"
                                select 
                                    p.sysno,p.productname,p.productimage,pr.price as BasicPrice, cl.levelname as PriceName
                                from
                                    pdProduct p
                                    inner join (select sum(score) as totalScore,ProductSysNo from FeProductComment group by ProductSysNo order by totalScore desc) fpc on p.sysno = fpc.productsysno
                                    inner join pdprice pr on p.sysno = pr.productsysno
                                    inner join CrCustomerLevel cl on cl.sysno = pr.sourcesysno
                                where
                                    pr.pricesource=@0 and pr.sourcesysno=@1 and p.sysno<>@2  and pr.status = @3 and rownum<@4
                            ", (int)ProductStatus.产品价格来源.会员等级价, customerLevelSysNo, productSysNo, (int)ProductStatus.产品价格状态.有效,
                                recordNum - result.Count + 1).QueryMany<CBPdProductDetail>();

                    foreach (var cbPdProductDetail in tempList)
                    {
                        result.Add(cbPdProductDetail);
                    }
                }

            }

            return result;
        }

        /// <summary>
        /// 读取商品的属性相关内容
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <returns>返回商品属性列表</returns>
        /// <remarks>2013-08-08 邵斌 创建</remarks>
        public override IList<CBPdProductAtttributeReadRelation> GetProductAttributeInfo(int productSysNo)
        {
            IList<CBPdProductAtttributeReadRelation> groupSysNoList = new List<CBPdProductAtttributeReadRelation>();
            using (var context = Context.UseSharedConnection(true))
            {
                groupSysNoList = context.Sql(@"select
                                                             g.sysno as AttributeGroupSysNo,g.name as AttributeGroupName
                                                            from 
                                                             PdAttributeGroup g 
                                                             inner join (
                                                                select
                                                                 ag.sysno
                                                                from
                                                                 PdProductAttribute pa
                                                                 left join PdAttributeGroup ag on ag.sysno = pa.AttributeGroupSysNo
                                                                where pa.productsysno=@0
                                                                group by ag.sysno
                                                              ) t1 on g.sysno = t1.sysno
                                                    ", productSysNo).QueryMany<CBPdProductAtttributeReadRelation>();

                for (int i = 0; i < groupSysNoList.Count; i++)
                {
                    groupSysNoList[i].ProductAtttributeList = context.Sql(@"select
                                     pa.Attributesysno as ProductAttributeSysno,pa.Attributetext,pa.Attributeimage ,a.isrelationflag,a.Attributename
                                    from
                                     PdProductAttribute pa
                                     left join PdAttribute a on a.sysno = pa.attributesysno
                                    where pa.productsysno=@0 and pa.AttributeGroupSysNo = @1 and pa.status=@2
                                    ", productSysNo, groupSysNoList[i].AttributeGroupSysNo, (int)ProductStatus.商品属性状态.启用).QueryMany<CBPdProductAtttributeRead>();
                }
            }

            return groupSysNoList;
        }

        /// <summary>
        /// 获取商品默认图片
        /// </summary>
        /// <param name="productSysNo">商品编号</param>
        /// <returns>返回图片路径</returns>
        /// <remarks>2013-08-16 唐永勤 创建</remarks>
        public override string GetImageDefaultImg(int productSysNo)
        {
            return Context.Select<string>("ProductImage")
                           .From("PdProduct")
                           .Where("sysno = @sysno")
                           .Parameter("sysno", productSysNo)
                           .QuerySingle();
        }

        /// <summary>
        /// 读取商品关联属性值
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <returns>返回商品的关联属性以及值列表</returns>
        /// <remarks>2013-08-16 邵斌 创建</remarks>
        public override IList<PdProductAttribute> GetProductAssociationAttributeValue(int productSysNo)
        {
            return Context.Sql(@"
                select
                 ppa.attributetext,ppa.attributesysno,ppa.attributename,ppa.attributeimage
                from 
                    PdProductAttribute ppa
                    inner join Pdproductassociation pa on ppa.attributesysno = pa.attributesysno and pa.productsysno=@0
                where 
                    ppa.productsysno=@1
            ", productSysNo, productSysNo).QueryMany<PdProductAttribute>();
        }

        /// <summary>
        /// 根据商品系统编号读取搭配销售商品
        /// </summary>
        /// <param name="productSysNo">商品系统编号（搭配销售主商品）</param>
        /// <returns>返回商品列表</returns>
        /// <remarks>2013-09-10 邵斌 创建</remarks>
        public override IList<CBSimplePdProduct> GetProductCollocationListByProductSysNo(int productSysNo)
        {
            //查找所有系统编号
            IList<int> productSysNoList = Context.Sql("select productsysno from PdProductCollocation where code=@0",
                                                      productSysNo).QueryMany<int>();
            //查找商品信息
            IList<CBSimplePdProduct> result = new List<CBSimplePdProduct>();       //结果集
            CBSimplePdProduct tempProduct;                                         //商品临时变量

            //添加本商品
            tempProduct = GetProduct(productSysNo);                                //获取当个商品信息
            result.Add(tempProduct);

            foreach (var tempProductSysNo in productSysNoList)
            {
                tempProduct = GetProduct(tempProductSysNo);                                //获取当个商品信息
                //判断商品是否存在
                if (tempProduct != null && tempProduct.Prices != null)
                {
                    //如果存在将添加到结果集中
                    result.Add(tempProduct);
                }
            }

            //返回结果
            return result;
        }

        /// <summary>
        /// 根据主商品系统编号获取组合套餐商品列表
        /// </summary>
        /// <param name="productSysNo"></param>
        /// <returns>返回商品列表</returns>
        /// <remarks>2013-09-10 邵斌 创建</remarks>
        public override IList<CBWebSpComboItem> GetSpComboByProductSysNo(int productSysNo)
        {
            #region 测试SQL  1、查找组合套餐明细主商品是当前商品并判断组合套装（SpCombo）各个状态 2、通过组合套装系统编号查找到所以组合套餐的明细并按主商品排序在最前

            /*
             select sic2.*,pd.productimage,sc2.promotionsysno from
            (
            select 
                sc.sysno 
            from  
                SpComboItem sci
                inner join SpCombo sc on sc.sysno = sci.combosysno
            where
                sci.productsysno = 850 and sci.ismaster = 1 and Status=20 and sysdate between sc.StartTime and sc.EndTime and sc.comboquantity > sc.salequantity
            ) t
            inner join SpComboItem sic2 on sic2.combosysno = t.sysno
            inner join pdproduct pd on pd.sysno = sic2.productsysno
            inner join SpCombo sc2 on sc2.sysno = sic2.combosysno
            order by sic2.ismaster desc
             */

            #endregion

            //查找所有系统编号
            IList<CBWebSpComboItem> productSysNoList = Context.Sql(@"select sic2.*,pd.productimage,sc2.promotionsysno from
                                                        (
                                                        select 
                                                          sc.sysno 
                                                        from  
                                                            SpComboItem sci
                                                            inner join SpCombo sc on sc.sysno = sci.combosysno
                                                        where
                                                            sci.productsysno = @0 and sci.ismaster = @1 and Status=@2 and sysdate between sc.StartTime and sc.EndTime and sc.comboquantity > sc.salequantity
                                                        ) t
                                                        inner join SpComboItem sic2 on sic2.combosysno = t.sysno
                                                        inner join pdproduct pd on pd.sysno = sic2.productsysno
                                                        inner join SpCombo sc2 on sc2.sysno = sic2.combosysno
                                                        order by sic2.ismaster desc",
                                                      productSysNo
                                                      , (int)PromotionStatus.是否是套餐主商品.是
                                                      , (int)PromotionStatus.组合套餐状态.已审核).QueryMany<CBWebSpComboItem>();
            //查找商品信息
            foreach (var tempProduct in productSysNoList)
            {
                tempProduct.Prices = Product.PdPriceDaoImpl.Instance.GetProductPrice(tempProduct.ProductSysNo); //获取商品价格

            }

            return productSysNoList;

        }

        /// <summary>
        /// 获取商品静态统计数据
        /// </summary>
        /// <param name="productSysNo">商品编号</param>
        /// <returns>商品静态统计数据</returns>
        /// <remarks>2013-12-26 黄波 创建</remarks>
        public override PdProductStatistics GetProductStatistics(int productSysNo)
        {
            return Context.Sql("select * from PdProductStatistics where ProductSysNo=@ProductSysNo")
                .Parameter("ProductSysNo", productSysNo)
                .QuerySingle<PdProductStatistics>();
        }

        /// <summary>
        /// 更新商品销售数量
        /// </summary>
        /// <param name="productSysNo">产品编号</param>
        /// <param name="accelerate">增加的数量</param>
        /// <returns></returns>
        /// <remarks>2013-11-1 黄波 创建</remarks>
        public override void UpdateProductSales(int productSysNo, int accelerate)
        {
            Context.Sql("update PdProductStatistics set Sales=Sales+@sales where productsysno=@productsysno")
                .Parameter("sales", accelerate)
                .Parameter("productsysno", productSysNo)
                .Execute();
        }

        /// <summary>
        /// 批量更新商品销售数量
        /// </summary>
        /// <param name="saleList">商品数据集合(key:productSysNo value:sales)</param>
        /// <returns></returns>
        /// <remarks>2013-11-4 黄波 创建</remarks>
        public override void UpdateProductSales(IDictionary<int, int> saleList)
        {
            using (var context = Context.UseSharedConnection(true))
            {
                foreach (var item in saleList)
                {
                    context.Sql("update PdProductStatistics set Sales=Sales+@sales where productsysno=@productsysno")
                        .Parameter("sales", item.Value)
                        .Parameter("productsysno", item.Key)
                        .Execute();
                }
            }
        }

        /// <summary>
        /// 更新商品喜欢数量
        /// </summary>
        /// <param name="productSysNo">产品编号</param>
        /// <param name="accelerate">增加的数量</param>
        /// <returns></returns>
        /// <remarks>2013-11-1 黄波 创建</remarks>
        public override void UpdateProductLiking(int productSysNo, int accelerate)
        {
            Context.Sql("update PdProductStatistics set liking=liking+@accelerate where productsysno=@productSysNo")
                .Parameter("accelerate", accelerate)
                .Parameter("productSysNo", productSysNo)
                .Execute();
        }

        /// <summary>
        /// 更新商品收藏数量
        /// </summary>
        /// <param name="productSysNo">产品编号</param>
        /// <param name="accelerate">增加的数量</param>
        /// <returns></returns>
        /// <remarks>2013-11-1 黄波 创建</remarks>
        public override void UpdateProductFavorites(int productSysNo, int accelerate)
        {
            Context.Sql("update PdProductStatistics set Favorites=Favorites+@accelerate where productsysno=@productSysNo")
                .Parameter("accelerate", accelerate)
                .Parameter("productSysNo", productSysNo)
                .Execute();
        }

        /// <summary>
        /// 更新商品评论数量
        /// </summary>
        /// <param name="productSysNo">产品编号</param>
        /// <param name="score">评分</param>
        /// <param name="accelerate">增加的数量</param>
        /// <returns></returns>
        /// <remarks>2013-11-1 黄波 创建</remarks>
        public override void UpdateProductComments(int productSysNo, int score, int accelerate)
        {
            Context.Sql("update PdProductStatistics set Comments=Comments+@accelerate,TotalScore=TotalScore+@score,AverageScore=(TotalScore+@score)/(Comments+@accelerate) where productsysno=@productSysNo")
                .Parameter("accelerate", accelerate)
                .Parameter("score", score)
                //.Parameter("accelerate", accelerate)
                .Parameter("productSysNo", productSysNo)
                .Execute();
        }

        /// <summary>
        /// 更新商品晒单数量
        /// </summary>
        /// <param name="productSysNo">产品编号</param>
        /// <param name="accelerate">增加的数量</param>
        /// <returns></returns>
        /// <remarks>2013-11-1 黄波 创建</remarks>
        public override void UpdateProductShares(int productSysNo, int accelerate)
        {
            Context.Sql("update PdProductStatistics set Shares=Shares+@accelerate where productsysno=@productSysNo")
                .Parameter("accelerate", accelerate)
                .Parameter("productSysNo", productSysNo)
                .Execute();
        }

        /// <summary>
        /// 更新商品咨询数量
        /// </summary>
        /// <param name="productSysNo">产品编号</param>
        /// <param name="accelerate">增加的数量</param>
        /// <returns></returns>
        /// <remarks>2013-11-1 黄波 创建</remarks>
        public override void UpdateProductQuestion(int productSysNo, int accelerate)
        {
            Context.Sql("update PdProductStatistics set Question=Question+@accelerate where productsysno=@productSysNo")
                .Parameter("accelerate", accelerate)
                .Parameter("productSysNo", productSysNo)
                .Execute();
        }

        /// <summary>
        /// 更新商品浏览量
        /// </summary>
        /// <param name="SysNo">产品编号</param>
        /// <param name="accelerate">修改的数量</param>
        /// <returns></returns>
        /// <remarks>2016-03-02 罗远康 创建</remarks>
        public override int UPdatePdProductViewCount(int SysNo, int accelerate)
        {
            return Context.Sql("UPDATE PdProduct SET ViewCount=@accelerate WHERE SysNo=@SysNo")
                .Parameter("accelerate", accelerate)
                .Parameter("SysNo", SysNo)
                .Execute();
        }

        /// <summary>
        /// 模糊查询商品
        /// </summary>
        /// <param name="KeyWord"></param>
        /// <returns></returns>
        public override List<CBSimplePdProduct> GetUtilLikePdProduct(string KeyWord)
        {
            if (!string.IsNullOrEmpty(KeyWord))
            {
                string sqlstr = @" select top 50 
                        p.SysNo,p.BrandSysNo,p.ErpCode,p.Barcode,p.QrCode,p.ProductType,p.EasName,p.GrosWeight,p.NetWeight,p.SalesAddress,p.ProductName,p.ProductSubName,p.NameAcronymy,p.ProductShortTitle,p.ProductSummary,p.ProductSlogan,p.PackageDesc,p.ProductDesc,p.ProductImage,p.ViewCount,p.SeoTitle,p.SeoKeyword,p.SeoDescription,p.Status,p.DisplayOrder,pb.name as BrandName,p.CanFrontEndOrder,p.Volume,o.Origin_Name,p.AgentSysNo 
                    from 
                        PdProduct p
                        left join PdBrand pb on pb.sysno = p.brandsysno
                        left join Origin o on p.OriginSysNo = o.SysNo
                    where 
                        ('%" + KeyWord + "%' is null or p.ErpCode like '%" + KeyWord + "%') or ('%" + KeyWord + "%' is null or p.EasName like '%" + KeyWord + "%') or ('%" + KeyWord + "%' is null or p.Barcode like '%" + KeyWord + "%')  ";
                return Context.Sql(sqlstr).QueryMany<CBSimplePdProduct>();
            }
            else
            {
                List<CBSimplePdProduct> list = new List<CBSimplePdProduct>();
                return list;
            }
        }

        /// <summary>
        /// 根据商品代码查询商品
        /// </summary>
        /// <param name="KeyWord"></param>
        /// <returns></returns>
        public override CBSimplePdProduct GetUtilLikePdProductCode(string KeyWord)
        {
            if (!string.IsNullOrEmpty(KeyWord))
            {
                string sqlstr = @" select top 1 
                        p.SysNo,p.BrandSysNo,p.ErpCode,p.Barcode,p.QrCode,p.ProductType,p.EasName,p.GrosWeight,p.NetWeight,p.SalesAddress,p.ProductName,p.ProductSubName,p.NameAcronymy,p.ProductShortTitle,p.ProductSummary,p.ProductSlogan,p.PackageDesc,p.ProductDesc,p.ProductImage,p.ViewCount,p.SeoTitle,p.SeoKeyword,p.SeoDescription,p.Status,p.DisplayOrder,pb.name as BrandName,p.CanFrontEndOrder,p.Volume,o.Origin_Name,p.AgentSysNo 
                    from 
                        PdProduct p
                        left join PdBrand pb on pb.sysno = p.brandsysno
                        left join Origin o on p.OriginSysNo = o.SysNo
                    where ('%" + KeyWord + "%' is null or p.ErpCode like '%" + KeyWord + "%')  ";
                return Context.Sql(sqlstr).QuerySingle<CBSimplePdProduct>();
            }
            else
            {
                CBSimplePdProduct list = new CBSimplePdProduct();
                return list;
            }
        }
    }
}
