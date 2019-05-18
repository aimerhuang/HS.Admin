using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.BLL.CRM;
using Hyt.DataAccess.Web;
using Hyt.Infrastructure.Caching;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Model.B2CApp;
using Hyt.BLL.Authentication;

namespace Hyt.BLL.Web
{
    /// <summary>
    /// 前台商品信息操作
    /// </summary>
    /// <remarks>2013-08-07 邵斌 创建</remarks>
    public class PdProductBo : BOBase<PdProductBo>
    {

        /// <summary>
        /// 根据商品系统编号获取商品信息
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <returns>商品信息</returns>
        /// <remarks>2013-08-14 邵斌 创建</remarks>
        public CBSimplePdProduct GetProduct(int productSysNo)
        {
            return CacheManager.Get<CBSimplePdProduct>(CacheKeys.Items.ProductDetailInfo_, productSysNo.ToString(), delegate()
            {
                var product = Hyt.DataAccess.Web.IPdProductDao.Instance.GetProduct(productSysNo);
                if (product != null)
                {
                    //判断商品主分类状态是否是可以购买（分类禁用和不显示视为无效）
                    if (product.Categories.Count > 0 &&
                        product.Categories[0].IsOnline == (int)ProductStatus.是否前端展示.是 &&
                        product.Categories[0].Status == (int)ProductStatus.商品分类状态.有效)
                    {
                        product.Status = (int)ProductStatus.商品状态.上架;
                    }
                    else
                    {
                        product.Status = (int)ProductStatus.商品状态.下架;
                    }

                    if (product != null)
                    {
                        product.ProductAssociationRelationCode = Hyt.BLL.Product.PdProductAssociationBo.Instance.GetRelationCode(productSysNo);
                        PdCategory masterCategory = product.Categories.Any() ? product.Categories[0] : null;

                        //判断商品是否设置SEO
                        if (string.IsNullOrWhiteSpace(product.SeoTitle))
                        {
                            //如果商品有主分类则将分类SEO传递给商品
                            if (masterCategory != null)
                            {
                                product.SeoTitle = product.SeoTitle ?? masterCategory.SeoTitle;
                                product.SeoKeyword = product.SeoKeyword ?? masterCategory.SeoKeyword;
                                product.SeoDescription = product.SeoDescription ?? masterCategory.SeoDescription;
                            }
                            else
                            {
                                product.SeoTitle = product.SeoTitle ?? product.ProductName;
                                product.SeoKeyword = product.SeoKeyword ?? product.ProductName;
                                product.SeoDescription = product.SeoDescription ?? product.ProductName;
                            }
                        }

                        //获取商品团购信息
                        product.GroupShoppingSysNo = DataAccess.Tuan.IGsGroupShoppingDao.Instance.GetGroupShoppingSysNoByProduct(product.SysNo);
                        //组合商品价格名称
                        IList<PdPriceType> priceTypes = GetPriceTypeItems();
                        PdPriceType tempType;
                        foreach (var price in product.Prices)
                        {
                            tempType = priceTypes.FirstOrDefault(pn => pn.PriceSource == price.PriceSource && pn.SourceSysNo == price.SourceSysNo);
                            price.PriceName = (tempType != null) ? tempType.TypeName : "";
                        }
                    }

                    return product;
                }
                else
                {
                    return null;
                }
            });
        }
        /// <summary>
        /// 获取商品实体
        /// </summary>
        /// <param name="productSysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-03-29 王耀发 创建</remarks>
        public CBSimplePdProduct GetProductEntity(int productSysNo)
        {
            var product = Hyt.DataAccess.Web.IPdProductDao.Instance.GetProduct(productSysNo);
            return product;
        }


        /// <summary>
        /// 获取商品实体
        /// </summary>
        /// <param name="erpCode">erpCode</param>
        /// <returns></returns>
        /// <remarks>2017-07-03 吴琨 创建</remarks>
        public CBSimplePdProduct GetProductErpCode(string erpCode, string Barcode)
        {
            var product = Hyt.DataAccess.Web.IPdProductDao.Instance.GetProductErpCode(erpCode,Barcode);
            return product;
        }
        


        /// <summary>
        /// 获取产品详情
        /// </summary>
        /// <param name="productSysNo">产品系统编号</param>
        /// <returns></returns>
        /// <remarks>2016-9-12 杨浩 创建</remarks>
        public PdProduct GetProductInfo(int productSysNo)
        {
            return Hyt.DataAccess.Web.IPdProductDao.Instance.GetProductInfo(productSysNo);
        }
        /// <summary>
        /// 商品下架显示的推荐商品列表（猜您喜欢：所属分类存在，显示所属分类的商品;所属分类不存在,推荐好评高的商品）
        /// </summary>
        /// <param name="categorySysNo">商品所属分类系统编号</param>
        /// <param name="excludeProductSysNo">商品系统编号:为避免推荐时自身又出现在推荐列表中，若有冗余排除</param>
        /// <param name="recordNum"></param>
        /// <returns>商品下架显示的推荐商品列表</returns>
        /// <remarks>2013-08-07 邵斌 创建 </remarks>
        public IList<CBPdProductDetail> OffsaleRcdList(int categorySysNo, int excludeProductSysNo, int recordNum)
        {
            return CacheManager.Get(CacheKeys.Items.ProductDetialOffsaleRcdList_, categorySysNo.ToString(), DateTime.Now.AddMinutes(15),
                              delegate
                              {
                                  //返回结果
                                  IList<CBPdProductDetail> result;

                                  //读取同一分类下的商品推荐
                                  result = IPdProductDao.Instance.GetProductFromCategory(categorySysNo, excludeProductSysNo, recordNum);

                                  //如果同类下无法读满recordNum个数将推荐好评高的商品
                                  if (result.Count < recordNum)
                                  {
                                      IList<CBPdProductDetail> tmpProductList =
                                          IPdProductDao.Instance.GetBestProductComment(excludeProductSysNo, recordNum - result.Count);

                                      //将好评商品追加到商品列表中
                                      foreach (var cbPdProductDetail in tmpProductList)
                                      {
                                          result.Add(cbPdProductDetail);
                                      }

                                  }
                                  return result;
                              });

        }

        /// <summary>
        /// 购买了同一商品的人还买了其他那些商品
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <param name="customerLevel">客户等级</param>
        /// <param name="recordNum">记录数</param>
        /// <returns>商品列表</returns>
        /// <remarks>2013-08-08 邵斌 创建</remarks>
        public IList<CBPdProductDetail> GetOtherBuy(int productSysNo, int customerLevel, int recordNum)
        {
            return CacheManager.Get(CacheKeys.Items.OtherCustmerBought_, productSysNo.ToString(), delegate
                {
                    return IPdProductDao.Instance.GetOtherBuy(productSysNo, customerLevel, recordNum);
                });
        }

        /// <summary>
        /// 读取商品的属性相关内容
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <returns>返回商品属性列表</returns>
        /// <remarks>2013-08-08 邵斌 创建</remarks>
        public IList<CBPdProductAtttributeReadRelation> GetProductAttributeInfo(int productSysNo)
        {
            return CacheManager.Get(CacheKeys.Items.ProductAttribute_, productSysNo.ToString(), delegate
            {
                return IPdProductDao.Instance.GetProductAttributeInfo(productSysNo);
            });
        }

        /// <summary>
        /// 读取商品的所以关联属性
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <returns>返回商品属性列表</returns>
        /// <remarks>2013-08-12 邵斌 创建</remarks>
        public IList<PdProductAttribute> GetProductAssociationAttribute(int productSysNo)
        {

            return CacheManager.Get(CacheKeys.Items.ProductAssociationAttribute_, productSysNo.ToString(), delegate
                {
                    //string relationCode = Hyt.BLL.Product.PdProductAssociationBo.Instance.GetRelationCode(productSysNo);
                    return
                        Hyt.DataAccess.Web.IPdProductDao.Instance.GetProductAssociationAttributeValue(productSysNo);
                });
        }

        /// <summary>
        /// 读取商品的所以关联属性
        /// </summary>
        /// <param name="relationCode">关联关系码</param>
        /// <returns>返回商品属性列表</returns>
        /// <remarks>2013-08-12 邵斌 创建</remarks>
        public IList<CBProductAttribute> GetProductAllAssociationAttribute(string relationCode)
        {
            return CacheManager.Get(CacheKeys.Items.ProductAssociationAllAssociationAttribute_, relationCode, delegate
                {

                    IList<PdProductAttribute> list = Hyt.DataAccess.Product.IPdProductAssociationDao.Instance
                                                        .GetAssociationAttributeList(
                                                            relationCode);

                    IList<CBProductAttribute> result = new List<CBProductAttribute>();
                    //属性分组,进行属性归类
                    var group = from a in list
                                group a.AttributeSysNo by a.AttributeSysNo
                                    into g
                                    select g;
                    var attrSysNoList = group.Select(g => g.Key).ToList();

                    CBProductAttribute tempAttribute;

                    //属性归类处理
                    foreach (var sysNo in attrSysNoList)
                    {
                        tempAttribute = new CBProductAttribute();
                        tempAttribute.SysNo = sysNo;

                        //生产选项值列
                        tempAttribute.AttributeOptions =
                            list.Where(a => a.AttributeSysNo == sysNo)
                                .Select(
                                    a =>
                                    new CBAttributeOption()
                                        {
                                            AttributeText = a.AttributeText,
                                            AttributeOptionSysNo = a.AttributeOptionSysNo,
                                            Image = a.AttributeImage
                                        })
                                .ToList();

                        tempAttribute.AttributeName = list.FirstOrDefault(p => p.AttributeSysNo == sysNo).AttributeName;

                        result.Add(tempAttribute);

                    }

                    return result;
                });
        }

        /// <summary>
        /// 获取价格来源类型列表
        /// </summary>
        /// <returns>价格来源类型列表</returns>
        /// <remarks>2013-07-17 黄波 创建</remarks>
        public IList<PdPriceType> GetPriceTypeItems()
        {
            return CacheManager.Get(CacheKeys.Items.ProductPriceTypesNew, delegate
                {
                    return Hyt.DataAccess.Product.IPdPriceDao.Instance.GetPriceTypeItems().OrderBy(p => p.SourceSysNo).ToList();
                });
        }

        /// <summary>
        /// 获取商品默认图片
        /// </summary>
        /// <param name="productSysno">商品编号</param>
        /// <returns>返回图片路径</returns>
        /// <remarks>2013-08-16 唐永勤 创建</remarks>
        public string GetImageDefaultImg(int productSysno)
        {
            return CacheManager.Get(CacheKeys.Items.ProductDefaultImage_, productSysno.ToString(), delegate
            {
                string imgPath = Hyt.DataAccess.Web.IPdProductDao.Instance.GetImageDefaultImg(productSysno);
                if (string.IsNullOrEmpty(imgPath))
                {
                    return "";
                }
                else
                {
                    return imgPath;
                }
            });
        }

        /// <summary>
        /// 商品分类销售排行
        /// </summary>
        /// <param name="categorySysNo"></param>
        /// <param name="recordNum"></param>
        /// <returns>返回lucene结果列表</returns>
        /// <remarks>2013-08-15 邵斌 创建</remarks>
        public IList<CBSimplePdProduct> GetSalesRanking(int categorySysNo, int recordNum)
        {

            return CacheManager.Get(CacheKeys.Items.CategorySaleRanking_, categorySysNo.ToString() + "_" + recordNum.ToString(), delegate
                {

                    //通过lucene返回
                    int pageCount = 0;
                    int recordCount = 0;
                    int pageIndex = 1;
                    IList<PdProductIndex> productList = Hyt.BLL.Web.ProductIndexBo.Instance.Search("", categorySysNo, null, recordNum, ref pageIndex,
                                                                             ref pageCount, ref recordCount);

                    //查找用户
                    IList<CBSimplePdProduct> result = new List<CBSimplePdProduct>();
                    foreach (var pdProductIndex in productList)
                    {
                        result.Add(GetProduct(pdProductIndex.SysNo));
                    }

                    return result;

                });
        }

        /// <summary>
        /// 获取商品指定类型价格（一组价格）
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <param name="priceSourceType">商品价格来源类型</param>
        /// <param name="levelSysNo">等级系统编号</param>
        /// <returns>返回一组同一类型价格</returns>
        /// <remarks>2013-08-19 邵斌 创建</remarks>
        public IList<CBPdPrice> GetProductPrice(int productSysNo, ProductStatus.产品价格来源 priceSourceType)
        {
            CBSimplePdProduct product = GetProduct(productSysNo);

            if (product == null)
            {
                return null;
            }
            else if (product.Prices == null)
            {
                return null;
            }

            int sourctType = (int)priceSourceType;
            IList<CBPdPrice> prices = product.Prices.Where(p => p.PriceSource == sourctType).ToList();

            return prices;
        }

        /// <summary>
        /// 获取商品指定类型价格（一组价格）
        /// </summary>
        /// <param name="prices">商品所以价格列表</param>
        /// <param name="priceSourceType">商品价格来源类型</param>
        /// <returns>返回一组同一类型价格</returns>
        /// <remarks>2013-08-19 邵斌 创建</remarks>
        public IList<CBPdPrice> GetProductPrice(IList<CBPdPrice> prices, ProductStatus.产品价格来源 priceSourceType)
        {
            int sourctType = (int)priceSourceType;
            IList<CBPdPrice> result = prices.Where(p => p.PriceSource == sourctType).ToList();

            return result;
        }

        /// <summary>
        /// 获取商品指定类型价格（单个价格）
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <param name="priceSourceType">商品价格来源类型</param>
        /// <param name="levelSysNo">等级系统编号</param>
        /// <returns>返回单个系统价格对象</returns>
        /// <remarks>2013-08-19 邵斌 创建</remarks>
        public CBPdPrice GetProductPrice(int productSysNo, ProductStatus.产品价格来源 priceSourceType,
                                                 int levelSysNo = 0)
        {
            IList<CBPdPrice> prices = GetProductPrice(productSysNo, priceSourceType);
            if (prices == null)
                return null;
            return prices.FirstOrDefault(p => p.SourceSysNo == levelSysNo);
        }

        /// <summary>
        /// 根据商品系统编号读取搭配销售商品
        /// </summary>
        /// <param name="productSysNo">商品系统编号（搭配销售主商品）</param>
        /// <returns>返回商品列表</returns>
        /// <remarks>2013-09-10 邵斌 创建</remarks>
        public IList<CBSimplePdProduct> GetProductCollocationListByProductSysNo(int productSysNo)
        {
            return CacheManager.Get(Hyt.Infrastructure.Caching.CacheKeys.Items.ProductCollocation_,
                                    productSysNo.ToString(),
                                    delegate
                                    {
                                        var tempObj =
                                        Hyt.DataAccess.Web.IPdProductDao.Instance
                                           .GetProductCollocationListByProductSysNo(productSysNo);
                                        if (tempObj.Count == 0)
                                            return null;
                                        return tempObj;
                                    });
        }

        /// <summary>
        /// 根据主商品系统编号获取组合套餐商品列表
        /// </summary>
        /// <param name="productSysNo"></param>
        /// <returns>返回商品列表</returns>
        /// <remarks>2013-09-10 邵斌 创建</remarks>
        public IList<CBWebSpComboItem> GetSpComboByProductSysNo(int productSysNo)
        {
            return CacheManager.Get(Hyt.Infrastructure.Caching.CacheKeys.Items.ProductSpCombo_,
                                   productSysNo.ToString(),
                                   delegate
                                   {
                                       var tempObj =
                                           Hyt.DataAccess.Web.IPdProductDao.Instance
                                              .GetSpComboByProductSysNo(productSysNo);
                                       if (tempObj.Count == 0)
                                           return null;
                                       return tempObj;
                                   });
        }

        /// <summary>
        /// 获取商品价格（根据当前用户等级来获取等级价格）
        /// </summary>
        /// <param name="prices">商品价格列表</param>
        /// <param name="userLevelSysNo">会员等级系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-09-10 邵斌 创建</remarks>
        public PdPrice GetLevelPrice(IList<PdPrice> prices, int userLevelSysNo)
        {
            //获取等级价格
            var price = prices.FirstOrDefault(p => p.PriceSource == (int)ProductStatus.产品价格来源.会员等级价 && p.SourceSysNo == userLevelSysNo);
            //判断等级价格是否取得
            if (price == null)
            {
                //如果当前等级价格没有将读取最低的会员等级价格
                var junior = CrCustomerLevelBo.Instance.GetJuniorLevel();
                int levelSysNo = (junior == null) ? 1 : junior.SysNo;
                price = prices.FirstOrDefault(p => p.PriceSource == (int)ProductStatus.产品价格来源.会员等级价 && p.SourceSysNo == levelSysNo);

                //判断是否有等级价格
                if (price == null)
                {
                    //如果最低等级价格也没有将用基础价格作为返回值
                    price = GetBasePrice(prices);
                }
            }
            return price;

        }

        /// <summary>
        /// 获取商品价格（根据当前用户等级来获取等级价格）
        /// </summary>
        /// <param name="prices">商品价格列表</param>
        /// <param name="userLevelSysNo">会员等级系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-09-10 邵斌 创建</remarks>
        public CBPdPrice GetLevelPrice(IList<CBPdPrice> prices, int userLevelSysNo)
        {
            //获取等级价格
            var price = prices.FirstOrDefault(p => p.PriceSource == (int)ProductStatus.产品价格来源.会员等级价 && p.SourceSysNo == userLevelSysNo);

            //判断等级价格是否取得
            if (price == null)
            {
                //如果当前等级价格没有将读取最低的会员等级价格
                var junior = CrCustomerLevelBo.Instance.GetJuniorLevel();
                int levelSysNo = (junior == null) ? 1 : junior.SysNo;
                price = prices.FirstOrDefault(p => p.PriceSource == (int)ProductStatus.产品价格来源.会员等级价 && p.SourceSysNo == levelSysNo);

                //判断是否有等级价格
                if (price == null)
                {
                    //如果最低等级价格也没有将用基础价格作为返回值
                    price = GetBasePrice(prices);
                }
            }
            return price;
        }

        /// <summary>
        /// 获取基础价格 
        /// </summary>
        /// <param name="prices">商品价格列表</param>
        /// <returns>商品价格（必须>0 如果返回-1表示没有该商品价格</returns>
        /// <remarks>2013-10-15 邵斌 创建</remarks>
        public PdPrice GetBasePrice(IList<PdPrice> prices)
        {
            return prices.FirstOrDefault(p => p.PriceSource == (int)ProductStatus.产品价格来源.基础价格);
        }

        /// <summary>
        /// 获取基础价格 
        /// </summary>
        /// <param name="prices">简单商品价格列表</param>
        /// <returns>商品价格（必须>0 如果返回-1表示没有该商品价格</returns>
        /// <remarks>2013-10-15 邵斌 创建</remarks>
        public CBPdPrice GetBasePrice(IList<CBPdPrice> prices)
        {
            return prices.FirstOrDefault(p => p.PriceSource == (int)ProductStatus.产品价格来源.基础价格);

        }

        /// <summary>
        /// 个人可能会喜欢的商品
        /// </summary>
        /// <returns></returns>
        /// <remarks>2013-09-10 邵斌 创建</remarks>
        public IList<PdProductIndex> MayLike()
        {
            int reCount = 0, pageIndex = 1, pageCount = 2;
            return Hyt.BLL.Web.ProductIndexBo.Instance.Search(string.Empty, 32, null, 12, ref pageIndex, ref pageCount, ref reCount, false);
        }

        /// <summary>
        /// 更新商品销售数量
        /// </summary>
        /// <param name="productSysNo">产品编号</param>
        /// <param name="accelerate">增加的数量</param>
        /// <returns>void</returns>
        /// <remarks>2013-11-1 黄波 创建</remarks>
        public void UpdateProductSales(int productSysNo, int accelerate = 1)
        {
            IPdProductDao.Instance.UpdateProductSales(productSysNo, accelerate);
        }

        /// <summary>
        /// 批量更新商品销售数量
        /// </summary>
        /// <param name="saleList">商品数据集合(key:productSysNo value:sales)</param>
        /// <returns>void</returns> 
        ///  <remarks>2013-11-4 黄波 创建</remarks>
        public void UpdateProductSales(IDictionary<int, int> saleList)
        {
            IPdProductDao.Instance.UpdateProductSales(saleList);
        }

        /// <summary>
        /// 获取商品静态统计数据
        /// </summary>
        /// <param name="productSysNo">商品编号</param>
        /// <returns>商品静态统计数据</returns>
        /// <remarks>2013-12-26 黄波 创建</remarks>
        public PdProductStatistics GetProductStatistics(int productSysNo)
        {
            return IPdProductDao.Instance.GetProductStatistics(productSysNo);
        }

        /// <summary>
        /// 更新商品喜欢数量
        /// </summary>
        /// <param name="productSysNo">产品编号</param>
        /// <param name="accelerate">增加的数量</param>
        /// <returns>void</returns>
        /// <remarks>2013-11-1 黄波 创建</remarks>
        public void UpdateProductLiking(int productSysNo, int accelerate = 1)
        {
            IPdProductDao.Instance.UpdateProductLiking(productSysNo, accelerate);
        }

        /// <summary>
        /// 更新商品收藏数量
        /// </summary>
        /// <param name="productSysNo">产品编号</param>
        /// <param name="accelerate">增加的数量</param>
        /// <returns>void</returns>
        /// <remarks>2013-11-1 黄波 创建</remarks>
        public void UpdateProductFavorites(int productSysNo, int accelerate = 1)
        {
            IPdProductDao.Instance.UpdateProductFavorites(productSysNo, accelerate);
        }

        /// <summary>
        /// 更新商品评论数量
        /// </summary>
        /// <param name="productSysNo">产品编号</param>
        /// <param name="score">评分</param>
        /// <param name="accelerate">增加的数量</param>
        /// <returns>void</returns>
        /// <remarks>2013-11-1 黄波 创建</remarks>
        public void UpdateProductComments(int productSysNo, int score, int accelerate = 1)
        {
            IPdProductDao.Instance.UpdateProductComments(productSysNo, score, accelerate);
        }

        /// <summary>
        /// 更新商品晒单数量
        /// </summary>
        /// <param name="productSysNo">产品编号</param>
        /// <param name="accelerate">增加的数量</param>
        /// <returns>void</returns>
        /// <remarks>2013-11-1 黄波 创建</remarks>
        public void UpdateProductShares(int productSysNo, int accelerate = 1)
        {
            IPdProductDao.Instance.UpdateProductShares(productSysNo, accelerate);
        }

        /// <summary>
        /// 更新商品咨询数量
        /// </summary>
        /// <param name="productSysNo">产品编号</param>
        /// <param name="accelerate">增加的数量</param>
        /// <returns>void</returns>
        /// <remarks>2013-11-1 黄波 创建</remarks>
        public void UpdateProductQuestion(int productSysNo, int accelerate = 1)
        {
            IPdProductDao.Instance.UpdateProductQuestion(productSysNo, accelerate);
        }


        /// <summary>
        ///  更新商品销量
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <param name="userSysNo">用户系统编号</param>
        /// <param name="Sales">销量</param>
        /// <returns>更新商品销量是否成功</returns>
        /// <remarks>2016-03-02 罗远康 创建</remarks>
        public Result UPdatePdProductSales(int productSysNo, int userSysNo, int Sales)
        {
            Result result = new Result();

            var model = Product.PdProductStatisticsBo.Instance.Get(productSysNo);
            model.Sales = Sales;

            try
            {
                result.StatusCode = Product.PdProductStatisticsBo.Instance.Update(model);
                result.Status = result.StatusCode > 0;
                //用户操作日志
                BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, string.Format("更新{0}商品销量量成功", productSysNo), LogStatus.系统日志目标类型.商品基本信息, productSysNo,
                    AdminAuthenticationBo.Instance.Current.Base.SysNo);
            }
            catch (Exception ex)
            {
                result.StatusCode = -1;
                result.Status = false;
                //用户操作日志
                BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, string.Format(ex.Message), LogStatus.系统日志目标类型.商品基本信息, productSysNo,
                    AdminAuthenticationBo.Instance.Current.Base.SysNo);
            }

            return result;
        }

        /// <summary>
        /// 更新商品浏览量
        /// </summary>
        /// <param name="SysNo">产品编号</param>
        /// <param name="accelerate">修改的数量</param>
        /// <returns></returns>
        /// <remarks>2016-03-02 罗远康 创建</remarks>>
        public Result UPdatePdProductViewCount(int SysNo, int userSysNo, int accelerate)
        {
            Result result = new Result();
            try
            {
                result.StatusCode = IPdProductDao.Instance.UPdatePdProductViewCount(SysNo, accelerate);
                result.Status = result.StatusCode > 0;
                //用户操作日志
                BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, string.Format("更新{0}商品浏览量成功", SysNo), LogStatus.系统日志目标类型.商品基本信息, SysNo,
                    AdminAuthenticationBo.Instance.Current.Base.SysNo);
            }
            catch (Exception ex)
            {
                result.StatusCode = -1;
                result.Status = false;
                //用户操作日志
                BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, string.Format(ex.Message), LogStatus.系统日志目标类型.商品基本信息, SysNo,
                    AdminAuthenticationBo.Instance.Current.Base.SysNo);
            }
            return result;
        }

        /// <summary>
        /// 商品评价评价
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <returns>评价对象</returns>
        /// <remarks>2013-11-19 邵斌 创建</remarks>
        public CBReviewSatisfactionInfo GetAverageReviewSatisfaction(int productSysNo)
        {
            return CacheManager.Get<CBReviewSatisfactionInfo>(CacheKeys.Items.ProductAverageReviewSatisfaction_, productSysNo.ToString(), delegate()
              {

                  /*
                   * 由于三期没有设计等细节评分所以现在使用模拟评分
                   */
                  var reviewSatisfactionInfo = new CBReviewSatisfactionInfo();

                  var random = new System.Random();

                  reviewSatisfactionInfo.AppraiseScore = random.Next(95, 100);
                  reviewSatisfactionInfo.DesignScore = random.Next(95, 100);
                  reviewSatisfactionInfo.Materialscore = 100;
                  reviewSatisfactionInfo.OperateScore = random.Next(95, 100);
                  reviewSatisfactionInfo.PerformanceScore = random.Next(95, 100);
                  reviewSatisfactionInfo.PriceScore = 100;

                  return reviewSatisfactionInfo;
              }
              );
        }


        /// <summary>
        /// 模糊查询商品
        /// </summary>
        /// <param name="KeyWord"></param>
        /// <returns></returns>
        public  List<CBSimplePdProduct> GetUtilLikePdProduct(string KeyWord)
        {
            return IPdProductDao.Instance.GetUtilLikePdProduct(KeyWord);
        }


        /// <summary>
        /// 根据商品代码查询商品
        /// </summary>
        /// <param name="KeyWord"></param>
        /// <returns></returns>
        public  CBSimplePdProduct GetUtilLikePdProductCode(string KeyWord)
        {
            return IPdProductDao.Instance.GetUtilLikePdProductCode(KeyWord);
        }

    }
}
