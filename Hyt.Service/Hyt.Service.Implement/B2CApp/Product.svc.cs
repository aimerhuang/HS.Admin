using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Hyt.BLL.AppContent;
using Hyt.BLL.Front;
using Hyt.BLL.Product;
using Hyt.BLL.Tuan;
using Hyt.BLL.Web;
using Hyt.Model;
using Hyt.Model.B2CApp;
using Hyt.Model.Parameter;
using Hyt.Model.SystemPredefined;
using Hyt.Model.WorkflowStatus;
using Hyt.Service.Contract.B2CApp;
using FeProductItemBo = Hyt.BLL.Web.FeProductItemBo;
using PdProductBo = Hyt.BLL.Product.PdProductBo;
using System.Collections;
using Hyt.Infrastructure.Memory;

namespace Hyt.Service.Implement.B2CApp
{
    public class Product : BaseService, IProduct
    {
        #region Common

        /// <summary>
        /// 获取所有产品类别
        /// </summary>
        /// <returns>返回产品类别</returns>
        /// <remarks> 2013-7-5 杨浩 创建 </remarks>
        /// <remarks>2013-08-20 周瑜 实现逻辑</remarks>
        public Result<IList<ParentClassApp>> GetCategories()
        {
            var list = new List<ParentClassApp>();

            var categoryList = Hyt.BLL.Web.PdCategoryBo.Instance.GetAllCategory().ToList();
            var parent = categoryList.FindAll(o => o.ParentSysNo == 0);
            var sub = categoryList.FindAll(o => o.ParentSysNo > 0);

            foreach (var p in parent)
            {
                var pa = new ParentClassApp
                    {
                        SysNo = p.SysNo,
                        Title = p.CategoryName,
                        Items = new List<SubClassApp>(),
                        ImgUrl = p.CategoryImage
                    };
                list.Add(pa);
                string summary = string.Empty;
                int summaryCount = 0;
                foreach (var s in sub.Where(s => s.ParentSysNo == p.SysNo))
                {
                    pa.Items.Add(new SubClassApp
                        {
                            SysNo = s.SysNo,
                            Title = s.CategoryName
                        });
                    //为防止summary过长, 只取前3个
                    summaryCount++;
                    if (summaryCount <= 3)
                    {
                        summary += s.CategoryName + " \\ ";
                    }

                }
                if (summary.Length > 0)
                {
                    summary = summary.Substring(0, summary.LastIndexOf("\\", StringComparison.Ordinal));

                }
                pa.Summary = summary;
            }

            var model = new Result<IList<ParentClassApp>>
                {
                    Status = true,
                    StatusCode = 1,
                    Data = list
                };

            return model;
        }

        /// <summary>
        /// 获取搜索推荐关键字
        /// </summary>
        /// <returns>推荐关键字</returns>
        /// <remarks> 2013-7-5 杨浩 创建</remarks>
        public Result<string> GetSearchKeys()
        {
            return new Result<string>
                {
                    Status = true,
                    StatusCode = 1,
                    Message = "获取信息成功",
                    Data = "商城 数码配件 移动电源"
                };
        }

        /// <summary>
        /// 获取用户搜索历史
        /// </summary>
        /// <param name="customerSysNo">客户编号</param>
        /// <returns>搜索历史</returns>
        /// <remarks> 2013-7-5 杨浩 创建 </remarks>
        public Result<IList<string>> GetHistorySearch(int customerSysNo)
        {
            IList<string> keys = new List<string>();
            if (!string.IsNullOrEmpty(IMEI))
            {
                var list = Hyt.Infrastructure.Memory.MemoryProvider.Default.Get(IMEI) as Stack<string>;
                if (list != null)
                    foreach (var k in list)
                    {
                        keys.Add(k);
                    }
            }
            return new Result<IList<string>>
                {
                    Data = keys,
                    Status = true,
                    StatusCode = 1,
                };
        }

        /// <summary>
        /// 获取商品组
        /// </summary>
        /// <param name="code">商品组编码</param>
        /// <returns>返回商品组</returns>
        /// <remarks>2013-7-5 杨浩 创建 </remarks>
        /// <remarks>2013-08-20 周瑜 实现逻辑</remarks>
        public Result<IList<ProductGroup>> GetAdvertProducts(string code)
        {
            var group = FeProductGroupBo.Instance.GetModelByGroupcode(code, ForeStatus.商品组平台类型.手机商城);
            var list = (from g in @group
                        let products = FeProductItemBo.Instance.GetFeProductItems(g.SysNo)
                        let items = products.Select(ad => new SimplProduct
                        {
                            SysNo = ad.ProductSysNo,
                            ProductName = ad.ProductName,
                            Thumbnail = ProductImageBo.Instance.GetProductImagePath(ProductThumbnailType.Image240, ad.ProductSysNo),
                            Price = PdPriceBo.Instance.GetProductPrice(ad.ProductSysNo).First(m => m.SourceSysNo == 0).Price, //基础价格
                            LevelPrice = PdPriceBo.Instance.GetProductPrice(ad.ProductSysNo).First(m => m.SourceSysNo == CurrentUser.LevelSysNo).Price, //会员价格
                            Specification = "",
                        }).ToList()
                        select new ProductGroup
                        {
                            DisplayOrder = g.DisplayOrder,
                            Name = g.Name,
                            ProductGroupIcon = ProductImageBo.Instance.GetFeAdvertImagePath(g.ProductGroupIcon),
                            NameColor = g.NameColor,
                            Items = items.ToList(),
                        }).ToList();

            var model = new Result<IList<ProductGroup>>
            {
                Data = list,
                Message = null,
                Status = true,
                StatusCode = 1
            };
            return model;

        }

        /// <summary>
        /// 获取广告组
        /// </summary>
        /// <param name="code">广告组编码</param>
        /// <returns>返回广告组</returns>
        /// <remarks>2013-08-20 周瑜 实现逻辑</remarks>
        /// <remarks>2014-05-04 周唐炬 修改返回图片路径问题</remarks>
        public Result<IList<AdvertGroup>> GetAdverts(string code)
        {
            string advertGroupName = string.Empty;
            switch (code)
            {
                case "appad000": advertGroupName = "首页轮换广告展示"; break;
                case "appad001": advertGroupName = "保护配饰系列产品"; break;
                case "appad002": advertGroupName = "苹果配件产品"; break;
                case "appad003": advertGroupName = "移动电源系列"; break;
                case "appad005": advertGroupName = "云"; break;
            }
            var adList = FeAdvertGroupBo.Instance.GetWebAdvertItems(ForeStatus.广告组平台类型.手机商城, code);
            var items = adList.Select(ad => new AdvertItem
                {
                    Name = ad.Name,
                    ImageUrl = ad.ImageUrl,
                    LinkUrl = ad.LinkUrl,
                    ProductSysNo = Hyt.Util.Validator.VHelper.ValidatorRule(new Hyt.Util.Validator.Rule.Rule_Number(ad.LinkUrl)) ? int.Parse(ad.LinkUrl) : 0
                }).ToList();

            var data = new AdvertGroup { Name = advertGroupName, Items = items };
            var list = new List<AdvertGroup> { data };
            var model = new Result<IList<AdvertGroup>>
                {
                    Data = list,
                    Message = null,
                    Status = true,
                    StatusCode = 1
                };
            return model;
        }

        /// <summary>
        /// 查看用户产品浏览历史
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <returns>用户产品浏览历史列表</returns>
        /// <remarks>2013-09-05 周唐炬 实现</remarks>
        public ResultPager<IList<SimplProduct>> GetHistory(int customerSysNo)
        {
            var result = new ResultPager<IList<SimplProduct>>() { StatusCode = -1 };
            var data = AppContentBo.Instance.GetProBroHistory(customerSysNo);
            if (data != null && data.Any())
            {
                result.Data = data;
                result.Status = true;
                result.StatusCode = 0;
            }
            else
            {
                result.Message = "无浏览历史!";
            }

            return result;
        }

        /// <summary>
        /// 删除用户产品浏览历史
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <returns>返回结果</returns>
        /// <remarks>2013-09-05 周唐炬 实现</remarks>
        public Result DeleteHistory(int customerSysNo)
        {
            var result = new Result() { StatusCode = -1 };
            var rowsAffected = AppContentBo.Instance.DeleteHistory(customerSysNo);
            if (rowsAffected > 0)
            {
                result.Message = "删除产品浏览历史成功";
                result.Status = true;
                result.StatusCode = 0;
            }
            return result;
        }

        /// <summary>
        /// 摇一摇匹配搜索结果
        /// </summary>
        /// <param name="brand">手机品牌</param>
        /// <param name="type">手机型号</param>
        /// <returns>搜索结果</returns>
        /// <remarks> 
        /// 2013-7-17 杨浩 创建 
        /// 2013-09-11 郑荣华 实现
        /// </remarks>
        public ResultPager<IList<SimplProduct>> GetShake(string brand, string type)
        {
            const int pageSize = 10;
            var pageIndex = 1;
            var pageCount = 1;
            var count = 10;
            var keyword = (brand ?? "") + " " + (type ?? "");
            //索引 品牌+型号
            IList<PdProductIndex> list;
            if (keyword.Trim().Length > 0)
            {
                list = ProductIndexBo.Instance.Search(keyword, null, null, pageSize, ref pageIndex,
                                                                    ref pageCount, ref count);
            }
            else //无查询条件则返回热卖商品
            {
                var filter = new ParaLuceneSearchFilter
                {
                    RecCount = count,
                    PageSize = pageSize,
                    PageCount = pageCount,
                    PageIndex = pageIndex,
                    Sort = CommonEnum.LuceneProductSortType.销量,
                    IsDescending = true,
                    CategorySysNo = 0
                };
                list = ProductIndexBo.Instance.Search(filter);
            }
            var data = list.Select(item => new SimplProduct
            {
                LevelPrice = item.RankPrice,
                Price = item.BasicPrice,
                ProductName = item.ProductName,
                SysNo = item.SysNo,
                Thumbnail = ProductImageBo.Instance.GetProductImagePath(ProductThumbnailType.Image180, item.SysNo)
            }).ToList();
            return new ResultPager<IList<SimplProduct>>
            {
                Data = data,
                Status = true,
                StatusCode = 1
            };
        }

        /// <summary>
        /// 获取商品分享内容
        /// </summary>
        /// <param name="sysNo">商品系统号</param>
        /// <param name="productName">商品名称</param>
        /// <returns>分享内容</returns>
        /// <remarks> 2013-10-10 杨浩 创建 </remarks>
        public Result<Share> GetShare(int sysNo, string productName)
        {
            return new Result<Share>
                {
                    Data = new Share
                        {
                            Content = "我在@商城 寻到了一件宝贝:" + productName + " " + "http://www.huiyuanti.com/product/details/" + sysNo + "  下载手机客户端:http://www.huiyuanti.com/app",
                            Thumbnail = ProductImageBo.Instance.GetProductImagePath(ProductThumbnailType.Image460, sysNo)
                        },
                    Status = true,
                    StatusCode = 1
                };
        }

        /// <summary>
        /// 获取团购分享内容
        /// </summary>
        /// <param name="sysNo">团购系统号</param>
        /// <returns>分享内容</returns>
        /// <remarks> 2013-10-10 杨浩 创建 </remarks>
        public Result<Share> GetGroupShare(int sysNo)
        {
            var temp = Hyt.BLL.Tuan.GsGroupShoppingBo.Instance.Get(sysNo);
            return new Result<Share>
            {
                Data = new Share
                {
                    Content = temp.Title + " 更多团购尽在商城,下载手机客户端:http://www.huiyuanti.com/app",
                    Thumbnail = ProductImageBo.Instance.GetGroupShoppingImagePath(temp.IconUrl)
                },
                Status = true,
                StatusCode = 1
            };
        }

        #endregion

        #region 首页数据

        /// <summary>
        /// App 首页数据
        /// </summary>
        /// <returns>返回首页数据</returns>
        /// <remarks>
        /// 2013-7-17 杨浩 创建
        /// 2013-9-17 周瑜 实现
        /// </remarks>
        public Result<Home> Home()
        {
            var t = GsGroupShoppingBo.Instance.GetNewGroupShoppingForApp();
            //为空团购隐藏
            GroupShopping topGroupBuy = null;
            if (t != null)
            {
                topGroupBuy = new GroupShopping
                    {
                        SysNo = t.SysNo,
                        Title = t.Title,
                        Subtitle = t.Subtitle,
                        IconUrl = ProductImageBo.Instance.GetGroupShoppingImagePath(t.IconUrl),
                        RemainingTime = t.EndTime == null ? 0 : t.EndTime.Value.Subtract(DateTime.Now).TotalMilliseconds,
                        GroupPrice = t.GroupPrice,
                        TotalPrice = t.TotalPrice,
                        Discount = t.Discount,
                        HaveQuantity = t.HaveQuantity
                    };
            }

            #region 构造新版临时数据

            var items = new List<AdvertItem>
            {
               new AdvertItem{ ProductSysNo=2302, ImageUrl="http://img.huiyuanti.com/app/b2capp/01.png", Name="彩豆 5000mAh"},
               new AdvertItem{ ProductSysNo=2017, ImageUrl="http://img.huiyuanti.com/app/b2capp/02.png", Name="电霸 四代 9000mAh"},
               new AdvertItem{ ProductSysNo=2277, ImageUrl="http://img.huiyuanti.com/app/b2capp/03.png", Name="品胜 手机支架 "}
            };
            var newRecommendProducts = new List<AdvertGroup>
            {
                new AdvertGroup{ SysNo=1, Name="新品首发",NameColor="#531673", Items=items },
                new AdvertGroup{SysNo=2,Name="移动电源系列产品",NameColor="#13a0e2",Items=items},
                new AdvertGroup{SysNo=3,Name="苹果配件系列产品",NameColor="#e15517",Items=items}
            };

            #endregion

            var data = new Home
                {
                    TopAdverts = GetAdverts("appad000").Data.FirstOrDefault(),
                    RecommendProducts = GetAdvertProducts("Homerp1").Data,
                    NewRecommendProducts = newRecommendProducts,
                    TopGroupBuy = topGroupBuy,
                    BottomProducts = GetAdvertProducts("Homerp2").Data.FirstOrDefault(),
                };
            return new Result<Home>
                {
                    Data = data,
                    Status = true,
                    StatusCode = 1
                };

        }

        /// <summary>
        /// 获取首页更多
        /// </summary>
        /// <param name="code">编码</param>
        /// <returns>返回首页数据</returns>
        /// <remarks>
        /// 2013-7-17 杨浩 创建
        /// 2013-9-17 周瑜 实现
        /// </remarks>
        public Result<HomeMore> GetHomeMore(string code)
        {
            var model = new HomeMore
            {
                TopAdverts = GetAdverts("appad000").Data.FirstOrDefault(),
                BottomProducts = GetAdvertProducts(code).Data.FirstOrDefault()
            };
            return new Result<HomeMore>
            {
                Data = model,
                Status = true,
                StatusCode = 1
            };
        }

        #endregion

        #region 商品检索

        #region private

        /// <summary>
        /// 获取价格等级名称
        /// </summary>
        /// <param name="level">用户等级</param>
        /// <returns>等级名称</returns>
        /// <remarks>2013-7-17 杨浩 创建 </remarks>
        private string GetLevelPriceName(int level)
        {
            var name = string.Empty;
            switch (level)
            {
                case CustomerLevel.初级:
                    name = "初级会员价";
                    break;
                case CustomerLevel.中级:
                    name = "中级会员价";
                    break;
                case CustomerLevel.高级:
                    name = "高级会员价";
                    break;
            }
            return name;
        }

        /// <summary>
        /// 缓存用户历史搜索
        /// </summary>
        /// <param name="key"></param>
        private void SetKeyCache(string key)
        {
            if (!string.IsNullOrEmpty(IMEI) && !string.IsNullOrEmpty(key))
            {
                var keys = Hyt.Infrastructure.Memory.MemoryProvider.Default.Get(IMEI) as Stack<string>;
                if (keys != null)
                {
                    if (!keys.Contains(key) && keys.Count < 10)
                        keys.Push(key);
                }
                else
                {
                    keys = new Stack<string>();
                    keys.Push(key);
                }
                Hyt.Infrastructure.Memory.MemoryProvider.Default.Set(IMEI, keys, int.MaxValue);
            }
        }

        #endregion

        /// <summary>
        /// 获取产品详情
        /// </summary>
        /// <param name="sysNo">产品系统号</param>
        /// <returns>产品详情</returns>
        /// <remarks> 
        /// 2013-7-17 杨浩 创建 
        /// 2013-08-22 郑荣华 实现
        /// </remarks>
        public Result<ProductDetail> GetProductDetails(string sysNo)
        {
            var pdSysNo = int.Parse(sysNo);
            var model = Hyt.BLL.Web.PdProductBo.Instance.GetProduct(pdSysNo);

            if (model == null)
            {
                return new Result<ProductDetail>
                {
                    Message = "未找到此商品",
                    Data = null,
                    Status = false,
                    StatusCode = -1
                };
            }
            //调用前台图片获取方法
            var rImgList = model.Images.Select(path => new Images
                {
                    Thumbnail = ProductImageBo.Instance.GetProductImagePath(path, ProductThumbnailType.Small),
                    OriginalImg = ProductImageBo.Instance.GetProductImagePath(path, ProductThumbnailType.Big)

                }).ToList();

            //是否存在 团购，组合 判断
            var hasTuan = false;
            var hasGroup = false;
            var promotionList = Hyt.BLL.Promotion.SpPromotionEngineBo.Instance.CheckPromotionHints(new[] { PromotionStatus.促销使用平台.手机商城 }, pdSysNo, true);
            foreach (var item in promotionList)
            {
                if (item.RuleType == (int)PromotionStatus.促销规则类型.团购)
                    hasTuan = true;
                if (item.RuleType == (int)PromotionStatus.促销规则类型.组合)
                    hasGroup = true;

            }
            //统计信息，当前数据不对 2013-08-30
            var pdStatistics = Hyt.BLL.Product.PdProductBo.Instance.GetPdProductStatistics(pdSysNo);
            pdStatistics = pdStatistics ?? new PdProductStatistics();

            // 价格组
            var prices = model.Prices.Where(x => x.PriceSource == (int)ProductStatus.产品价格来源.会员等级价).Select(m => new Prices
                {
                    Price = m.Price,
                    PriceName = m.PriceName + "会员价",
                    LevelSysNo = m.SourceSysNo

                }).ToList();

            var basePrice = model.Prices.SingleOrDefault(x => x.PriceSource == (int)ProductStatus.产品价格来源.基础价格);
            var levelPrice = model.Prices.SingleOrDefault(x => x.PriceSource == (int)ProductStatus.产品价格来源.会员等级价 && x.SourceSysNo == CurrentUser.LevelSysNo);


            var bPrice = basePrice == null ? 99999 : basePrice.Price;//查询不到设为99999
            var lPrice = levelPrice == null ? 99999 : levelPrice.Price;//查询不到设为99999

            var grade = Math.Round(model.CommentScore, 1);
            var data = new Model.B2CApp.ProductDetail
            {
                SysNo = model.SysNo,
                ProductName = model.ProductName,
                ImgList = rImgList,
                ProductSlogan = model.ProductSlogan ?? "",
                Price = bPrice,
                Grade = (double)grade,
                LevelName = CurrentUser.LevelName,
                LevelPrice = lPrice,
                GroupShoppingPrice = hasTuan ? GsGroupShoppingBo.Instance.GetGroupShoppingPrice(model.SysNo) : 0,
                ShowOrderNum = BLL.Front.FeProductCommentBo.Instance.GetShowCount(pdSysNo),
                ConsultingNum = Hyt.BLL.Web.CustomerQuestionBo.Instance.GetCustomerQuestionsCount(model.SysNo),
                SavePrice = bPrice - lPrice,//基础价格-用户等级价格 
                Attributes = Hyt.BLL.Product.PdProductAssociationBo.Instance.GetProductList(pdSysNo),
                HasTuan = hasTuan,
                HasGroup = hasGroup,
                MemberPrices = prices,
                IsAttention = IsLogin && Hyt.BLL.CRM.CrFavoritesBo.Instance.IsAttention(CurrentUser.SysNo, pdSysNo)
            };

            #region 添加浏览记录
            if (IsLogin)
            {
                var category = model.Categories.FirstOrDefault(c => c.IsMaster = true);
                var bhModel = new CrBrowseHistory
                    {
                        BrowseType = (int)CustomerStatus.商品浏览方式.手机商城,
                        IsPageDown = 0, //不翻页
                        CategorySysNo = category != null ? category.SysNo : 0,
                        CustomerSysNo = CurrentUser.SysNo,
                        KeyWord = "",
                        ProductSysNo = pdSysNo
                    };
                Hyt.BLL.CRM.CrBrowseHistoryBo.Instance.AddCrBrowseHistory(bhModel);
            }
            #endregion

            return new Result<ProductDetail>
            {
                Data = data,
                Status = true,
                StatusCode = 1
            };
        }
        /// <summary>
        /// 重置产品缓存
        /// </summary>
        /// <param name="dealerSysNo">店铺编号</param>
        /// <remarks>2013-7-20 杨浩 创建</remarks>     
        public Result ResetProductCache(int dealerSysNo)
        {
            var result=new Result()
            {               
                Status = true,
                Message = "删除成功！",
            };

              
            if (dealerSysNo >= 0)
            {
                MemoryProvider.Default.Remove(string.Format(KeyConstant.DealerAllProduct_,dealerSysNo));    
            }

            else
            {
                MemoryProvider.Default.RemoveByPattern(string.Format(KeyConstant.DealerAllProduct_, ""));
                BLL.Web.ProductIndexBo.Instance.GetDealerAllProductToDataTable(0); 
            }           
                
                             
            return result;
        }
        /// <summary>
        /// 搜索产品
        /// </summary>
        /// <param name="key">搜索关键字</param>
        /// <param name="categorySysNo">分类编号</param>
        /// <param name="attributes">属性列表</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageCount">分页总数</param>
        /// <param name="recCount">总记录数</param>
        /// <param name="highLight">是否高亮关键字</param>
        /// <param name="sort">排序(0:相关度 1:销量 2:价格 3:评分 4:上架时间)</param>
        /// <param name="isDescending">true 为降序 false为升序</param>
        /// <param name="productSysNo">商品编号</param>
        /// <param name="priceSource">产品价格来源</param>
        /// <param name="priceSourceSysNo">产品价格来源编号(会员等级编号)</param>
        /// <param name="showNotFrontEndOrder">搜索前台不能下单的商品</param>
        /// <returns>商品列表</returns>
        /// <remarks> 2013-7-20 杨浩 创建 </remarks>
        public ResultPager<IList<PdProductIndex>> SearchFromDataBase(string key
             , int categorySysNo
             , string attributes
             , int pageSize
             , int pageIndex
             , bool highLight = false
             , int sort = 1
             , bool isDescending = false
             , int productSysNo = 0
             , int priceSource=10
             , int priceSourceSysNo = CustomerLevel.初级
             , bool showNotFrontEndOrder = false
             , int brandSysNo = 0
             , string productGroupCode = null
             , int originSysNo = 0
             , int dealerSysNo = 0
             , int warehouseSysNo = 0
             , int productType = 0
             )
        {

            try
            {
                int pageCount = 0; // TODO: 初始化为适当的值
                int recCount = 0; // TODO: 初始化为适当的值

                var returnValue = ProductIndexBo.Instance.SearchFromDataBase(key, categorySysNo, attributes, pageSize, ref pageIndex, ref pageCount, ref recCount, highLight, sort, isDescending, productSysNo, (ProductStatus.产品价格来源)priceSource, priceSourceSysNo, showNotFrontEndOrder
                    , brandSysNo
                    , productGroupCode
                    , originSysNo
                    , dealerSysNo
                    , warehouseSysNo
                    , productType
                    );

                return new ResultPager<IList<PdProductIndex>>
                {
                    Data = returnValue,
                    Status = true,
                    StatusCode = 1,
                    PageCount = pageCount,
                    RecCount = recCount,
                    PageIndex = pageIndex,
                    HasMore = pageCount > pageIndex
                };
            }
            catch (Exception ex)
            {

                return new ResultPager<IList<PdProductIndex>>
                {
                    Data = null,
                    Status = false,
                    StatusCode = 1,
                    PageCount = 0,
                    RecCount = 0,
                    PageIndex = pageIndex,
                    HasMore = false,
                    Message=ex.Message,
                };
            }
      
        }
        /// <summary>
        /// 根据产品编号搜索产品
        /// </summary>
        /// <param name="dealerSysNo">经销商系统编号</param>
        /// <param name="productSysNoList">产品编号列表</param>
        /// <param name="priceSource">产品价格来源</param>
        /// <param name="priceSourceSysNo">客户等级编号</param>
        /// <returns></returns>
        /// <remarks>2017-1-12 杨浩 创建</remarks>
        public Result<IList<PdProductIndex>> Search(int dealerSysNo, string productSysNoList, int priceSource, int priceSourceSysNo,int isFrontDisplay=1)
        {
            
            var result = new Result<IList<PdProductIndex>>()
            {
                Status=true,
            };

            try
            {
              
                result.Data = ProductIndexBo.Instance.Search(dealerSysNo, productSysNoList
                 ,(ProductStatus.产品价格来源)priceSource
                 , priceSourceSysNo, isFrontDisplay);
               
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
            }
               
            return result;
        }

        /// <summary>
        /// 获取产品列表
        /// </summary>
        /// <param name="sysNo">分类系统号 PdCategory</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="sort">排序(1：销量，2：价格，3：好评.[正数为降序,负数升序])</param>
        /// <param name="attributeOptionSysNo">商品属性值编号字串?格式 PdAttribute</param>
        /// <returns>产品列表</returns>
        /// <remarks> 2013-7-17 杨浩 创建 </remarks>
        /// <remarks> 2013-8-23 周瑜 实现</remarks>
        /// <remarks> 2013-10-28 杨浩 重构 </remarks>
        public ResultPager<IList<SimplProduct>> GetProductList(string sysNo, int pageIndex, int sort, string attributeOptionSysNo)
        {
            #region 数据准备
            int level = CurrentUser.LevelSysNo;
            const int pageSize = 8;
            int pageCount = 0;
            int count = 0;
            List<int> attributes = null;
            if (!string.IsNullOrEmpty(attributeOptionSysNo) && attributeOptionSysNo != "0")
                attributes = attributeOptionSysNo.Split(',').Where(x => x != "").ToList().ConvertAll(int.Parse);
            pageIndex = pageIndex == 0 ? 1 : pageIndex;
            const ProductStatus.产品价格来源 priceSource = ProductStatus.产品价格来源.会员等级价;
            #endregion

            //Lucene检索
            var list = ProductIndexBo.Instance.Search(string.Empty, int.Parse(sysNo), attributes, pageSize, ref pageIndex, ref pageCount, ref count, false, Math.Abs(sort), sort > 0, 0, priceSource, level);

            var rlist = list.Where(x=>x.CanFrontEndOrder==(int)ProductStatus.是否前端展示.是).Select(x => new SimplProduct
                {
                    SysNo = x.SysNo,
                    Thumbnail = ProductImageBo.Instance.GetProductImagePath(ProductThumbnailType.Image180, x.SysNo),
                    ProductName = x.ProductName,
                    Price = x.BasicPrice,
                    LevelPrice = x.RankPrice,//BLL.Product.PdPriceBo.Instance.GetUserRankPrice(x.SysNo, level),
                    LevelName = GetLevelPriceName(level)

                }).ToList();

            return new ResultPager<IList<SimplProduct>>
                {
                    Data = rlist,
                    Status = true,
                    StatusCode = 1,
                    HasMore = pageCount > pageIndex
                };
        }

        /// <summary>
        /// 搜索产品
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="sort">排序(1：销量，2：价格，3：好评.[正数为降序,负数升序])</param>
        /// <param name="attributeOptionSysNo">商品属性值编号字串</param>
        /// <returns>产品列表</returns>
        /// <remarks> 2013-07-20 杨浩 创建 </remarks>
        /// <remarks> 2013-08-23 周  瑜 实现 </remarks>
        /// <remarks> 2013-10-27 杨浩 修改 </remarks>
        public ResultPager<PdSearch> Search(string key, int pageIndex, int sort, string attributeOptionSysNo)
        {
            SetKeyCache(key);

            int level = CurrentUser.LevelSysNo;
            List<int> attributes = null;
            if (!string.IsNullOrEmpty(attributeOptionSysNo) && attributeOptionSysNo != "0")
                attributes = attributeOptionSysNo.Split(',').ToList().ConvertAll(int.Parse);

            var priceSource = ProductStatus.产品价格来源.会员等级价;

            int pageCount = 0, recCount = 0;
            var pdList = ProductIndexBo.Instance.Search(key, 0, attributes, 10, ref pageIndex, ref pageCount, ref recCount, false, Math.Abs(sort), sort > 0, 0, priceSource, level);
            //取刷选属性的分类
            var categorySysNo = (from a in pdList
                                 group a by a.Category into g
                                 select new
                                     {
                                         g.Key,
                                         count = g.Count()

                                     }).OrderByDescending(p => p.count).FirstOrDefault();

            var products = pdList.Select(m => new SimplProduct
                {
                    SysNo = m.SysNo,
                    Price = m.BasicPrice,
                    ProductName = m.ProductName,
                    Thumbnail = ProductImageBo.Instance.GetProductImagePath(ProductThumbnailType.Image180, m.SysNo),
                    LevelPrice = m.RankPrice,//BLL.Product.PdPriceBo.Instance.GetUserRankPrice(m.SysNo, level),
                    Specification = string.Empty,
                    LevelName = GetLevelPriceName(level)

                }).ToList();

            return new ResultPager<PdSearch>
                {
                    Data = new PdSearch
                            {
                                Products = products,
                                CategorySysNo = categorySysNo == null ? 0 : categorySysNo.Key
                            },
                    Status = true,
                    StatusCode = 1,
                    HasMore = pageCount > pageIndex
                };
        }

        /// <summary>
        /// 获取商品规格
        /// </summary>
        /// <param name="sysNo">产品编号</param>
        /// <returns>商品规格</returns>
        /// <remarks> 
        /// 2013-7-17 杨浩 创建 
        /// 2013-08-22 郑荣华 实现
        /// </remarks>
        public Result<IList<Specification>> GetSpecification(int sysNo)
        {
            var list = Hyt.BLL.Product.PdAttributeBo.Instance.GetPdProductAttributeList(sysNo).Select(x => new Specification
                {
                    AttributeName = x.AttributeName,
                    AttributeText = x.AttributeText ?? ""
                }).ToList();
            return new Result<IList<Specification>>
                {
                    Data = list,
                    Status = true,
                    StatusCode = 1
                };
        }

        /// <summary>
        /// 获取热门搜索关键字
        /// </summary>
        /// <returns>关键字</returns>
        /// <remarks> 2013-7-10 杨浩 创建 </remarks>
        public Result<IList<string>> GetHotSearchKeys()
        {

            return new Result<IList<string>>
                {
                    Data = new List<string>
                        {
                            "上门贴膜",
                            "电霸",
                            "易充",
                            "备电",
                            "手机电池"
                        },
                    Status = true,
                    StatusCode = 1,
                };
        }

        /// <summary>
        /// 根据产品类别获取筛选属性
        /// </summary>
        /// <param name="categorySysNo">产品类别编号</param>
        /// <returns>产品类别筛选属性</returns>
        /// <remarks> 
        /// 2013-7-17 杨浩 创建 
        /// 2013-08-22 郑荣华 实现
        /// </remarks>
        public Result<IList<ProductAttribute>> GetFilterAttribute(int categorySysNo)
        {
            var list = Hyt.BLL.Product.PdAttributeBo.Instance.GetPdAttributeList(categorySysNo);
            var attributes = list.Select(x => new ProductAttribute
                {
                    SysNo = x.SysNo,
                    AttributeName = x.AttributeName,
                    AttributeOptions = Hyt.BLL.Product.PdAttributeBo.Instance.GetAttributeOptions(x.SysNo).Select(a => new AttributeOption
                        {
                            AttributeText = a.AttributeText,
                            AttributeOptionSysNo = a.SysNo
                        }).ToList()
                }).ToList();

            return new Result<IList<ProductAttribute>>
                {
                    Data = attributes,
                    Status = true,
                    StatusCode = 1
                };
        }

        #endregion

        #region 商品咨询

        /// <summary>
        /// 获取商品咨询列表
        /// </summary>
        /// <param name="productSysNo">商品系统号</param>
        /// <param name="pageIndex">页码</param>
        /// <returns>商品咨询列表</returns>
        /// <remarks> 
        /// 2013-8-5 杨浩 创建 
        /// 2013-08-22 郑荣华 实现
        /// </remarks>
        public ResultPager<IList<CustomerQuestion>> GetProductQuestions(string productSysNo, int pageIndex)
        {
            var pdSysNo = int.Parse(productSysNo);//pagesize默认10
            var list = CrCustomerQuestionBo.Instance.GetListByPdSysNo(pdSysNo, pageIndex);
            var rlist = list.Select(item => new CustomerQuestion
                {
                    Question = item.Question,
                    QuestionDate = item.QuestionDate,
                    Answer = item.Answer ?? "等待回复",
                    AnswerDate = item.AnswerDate == default(DateTime) ? DateTime.Now : item.AnswerDate,
                    ProductSysNo = item.ProductSysNo,
                    NickName = item.NickName ?? Hyt.Util.WebUtil.HideMobilePhone(item.MobilePhoneNumber)
                }).ToList();

            return new ResultPager<IList<CustomerQuestion>>
            {
                Data = rlist,
                Status = true,
                StatusCode = 1
            };
        }

        /// <summary>
        /// 添加商品咨询
        /// </summary>
        /// <param name="question">商品咨询模型</param>
        /// <returns>添加结果</returns>
        /// <remarks> 
        /// 2013-8-5 杨浩 创建 
        /// 2013-08-22 郑荣华 实现
        /// </remarks>
        public Result AddProductQuestions(CustomerQuestionAdd question)
        {
            var model = new CrCustomerQuestion
                {
                    CustomerSysNo = CurrentUser.SysNo,
                    Question = question.Question,
                    ProductSysNo = question.ProductSysNo,
                    QuestionType = question.QuestionType,
                    QuestionDate = DateTime.Now,
                    Status = (int)CustomerStatus.会员咨询状态.待回复
                };
            Hyt.BLL.Web.CustomerQuestionBo.Instance.Insert(model);
            return new Result
                {
                    Status = true,
                    StatusCode = 1
                };
        }

        /// <summary>
        /// 获取商品咨询类型
        /// </summary>
        /// <returns>商品咨询类型</returns>
        /// <remarks> 2013-8-5 杨浩 创建 </remarks>
        public Result<IDictionary<int, string>> GetProductQuestionType()
        {
            var dic = Hyt.Util.EnumUtil.ToDictionary(typeof(CustomerStatus.会员咨询类型));
            var temp = new Dictionary<int, string>();

            foreach (var k in dic)
            {
                temp[k.Key] = k.Value + "咨询";
            }

            return new Result<IDictionary<int, string>>
                {
                    Data = temp,
                    Status = true,
                    StatusCode = 1
                };
        }

        #endregion

        #region 商品评价

        /// <summary>
        /// 获取商品评价
        /// </summary>
        /// <param name="productSysNo">商品编号</param>
        /// <param name="type">5(All) 10（满意） 15(一般) 20(不满意)</param>
        /// <param name="pageIndex">页码</param>
        /// <returns>商品评价</returns>
        /// <remarks> 2013-8-5 杨浩 创建 </remarks>
        /// <remarks>2013-08-27 周唐炬 实现</remarks>
        public ResultPager<PdCommentTotal> GetProductComments(string productSysNo, int type, int pageIndex)
        {
            var result = new ResultPager<PdCommentTotal>() { StatusCode = -1 };
            if (!string.IsNullOrWhiteSpace(productSysNo))
            {
                var data = BLL.Front.FeProductCommentBo.Instance.Seach(pageIndex, (int)ForeStatus.是否晒单.否,
                                                                       (int)ForeStatus.是否评论.是,
                                                                       (int)ForeStatus.商品评论状态.已审,
                                                                       0, 0, DateTime.MinValue, DateTime.MaxValue, null,
                                                                       null, productSysNo);

                if (null != data && data.TData != null && data.TData.Count > 0)
                {
                    var commentTotal = new PdCommentTotal();
                    //计算评分分级数量
                    IEnumerable<CBFeProductComment> list = null;
                    switch (type) //Score:4或5分为满意，2或3分为一般，1分为不满意
                    {
                        case 5: //All
                            list = data.TData;
                            commentTotal.Satisfaction = list.Count(x => x.Score >= 4 && x.Score <= 5);
                            commentTotal.General = list.Count(x => x.Score >= 2 && x.Score <= 3);
                            commentTotal.Dissatisfied = list.Count(x => x.Score == 1);
                            break;
                        case 10: //满意
                            list = data.TData.Where(x => x.Score >= 4 && x.Score <= 5);
                            commentTotal.Satisfaction = list.Count(x => x.Score >= 4 && x.Score <= 5);
                            break;
                        case 15: //一般
                            list = data.TData.Where(x => x.Score >= 2 && x.Score <= 3);
                            commentTotal.General = list.Count(x => x.Score >= 2 && x.Score <= 3);
                            break;
                        case 20: //不满意
                            list = data.TData.Where(x => x.Score <= 1);
                            commentTotal.Dissatisfied = list.Count(x => x.Score <= 1);
                            break;
                    }
                    if (list != null && list.Any())
                    {
                        commentTotal.CommentList = new List<PdCommentList>();
                        foreach (var x in list)
                        {
                            if (x == null) continue;
                            var comment = new PdCommentList()
                                {
                                    ProductSysNo = x.ProductSysNo,
                                    Account = Hyt.Util.WebUtil.HideMobilePhone(x.Account),
                                    Content = x.Content,
                                    Score = x.Score,
                                    CommentTime = x.CommentTime,
                                };
                            var order = SoOrderBo.Instance.GetEntity(x.OrderSysNo);
                            comment.BuyTime = order != null ? order.CreateDate : DateTime.Now;
                            commentTotal.CommentList.Add(comment);
                        }
                        //计算总体平均评分
                        commentTotal.OverallScore = list.Average(x => x.Score);
                        commentTotal.TotalComments = list.Count();
                    }
                    commentTotal.UserCount = data.TotalItemCount;

                    result.Data = commentTotal;
                    result.Status = true;
                    result.StatusCode = 1;
                    result.HasMore = pageIndex < data.TotalPageCount;
                }
                else
                {
                    result.Message = "暂无商品评论!";
                }
            }
            else
            {
                result.Message = "商品编号不能为空!";
            }
            return result;
        }

        /// <summary>
        /// 添加商品评价
        /// </summary>
        /// <param name="comment">评价实体</param>
        /// <returns>结果</returns>
        /// <remarks> 
        /// 2013-8-5 杨浩 创建 
        /// 2013-08-22 郑荣华 实现
        /// 2013-08-27 周唐炬 重构
        /// </remarks>
        public Result AddComment(PdComment comment)
        {
            var result = new Result() { StatusCode = -1 };
            if (comment != null)
            {
                var model = new FeProductComment
                {
                    Advantage = comment.Advantage,
                    CommentStatus = (int)ForeStatus.商品评论状态.待审,
                    CommentTime = DateTime.Now,
                    Content = comment.Content,
                    CustomerSysNo = comment.CustomerSysNo,
                    Disadvantage = comment.Disadvantage,
                    ProductSysNo = comment.ProductSysNo,
                    OrderSysNo = comment.OrderSysNo,
                    Score = comment.Score,
                    Title = comment.Title,
                    IsComment = (int)ForeStatus.是否评论.是,
                    IsShare = (int)ForeStatus.是否晒单.否
                };

                var id = BLL.Front.FeProductCommentBo.Instance.Insert(model);
                if (id > 0)
                {
                    var statisticsModel = PdProductBo.Instance.GetPdProductStatistics(id);
                    if (statisticsModel != null)
                    {
                        statisticsModel.Comments++;
                        var data = BLL.Front.FeProductCommentBo.Instance.Seach(1, (int)ForeStatus.是否晒单.否, (int)ForeStatus.是否评论.是, (int)ForeStatus.商品评论状态.已审, 0, 0, DateTime.MinValue, DateTime.MaxValue, null, null, id.ToString());
                        if (null != data && data.TData != null && data.TData.Count > 0)
                        {
                            var average = data.TData.Average(x => x.Score);
                            if (average > double.MinValue)
                            {
                                statisticsModel.TotalScore = Convert.ToInt32(average);
                                statisticsModel.AverageScore = Convert.ToInt32(average / data.TData.Count);
                            }
                        }
                        PdProductStatisticsBo.Instance.Update(statisticsModel);
                    }
                    else
                    {
                        statisticsModel = new PdProductStatistics()
                            {
                                ProductSysNo = id,
                                Comments = 1,
                                TotalScore = 1,
                                AverageScore = 1
                            };
                        PdProductStatisticsBo.Instance.Create(statisticsModel);
                    }

                    result.Status = true;
                    result.StatusCode = 0;
                }
                else
                {
                    result.Message = "服务器错误，请稍后重试!";
                }
            }
            else
            {
                result.Message = "商品评价为不能为空,请重试!";
            }
            return result;
        }

        #endregion

        #region 商品晒单

        /// <summary>
        /// 新增晒单
        /// </summary>
        /// <param name="share">新增晒单模型</param>
        /// <returns>结果</returns>
        /// <remarks> 2013-8-5 杨浩 创建 </remarks>
        /// <remarks>2013-08-23 周唐炬 实现</remarks> 
        public Result AddShareOrders(PostShareOrder share)
        {
            var result = new Result();

            #region 参数检查
            if (string.IsNullOrEmpty(share.ShareTitle))
            {
                result.Message = "请填写晒单标题";
                result.Status = false;
                return result;
            }
            if (string.IsNullOrEmpty(share.ShareContent))
            {
                result.Message = "请填写晒单内容";
                result.Status = false;
                return result;
            }
            if (!share.Pictures.Any())
            {
                result.Message = "请上传晒单图片";
                result.Status = false;
                return result;
            }
            #endregion

            var model = new FeProductComment()
                {
                    ProductSysNo = share.ProductSysNo,
                    OrderSysNo = share.OrderSysNo,
                    CustomerSysNo = share.CustomerSysNo,
                    ShareTitle = share.ShareTitle,
                    ShareContent = share.ShareContent,
                    IsShare = (int)ForeStatus.是否晒单.是,
                    IsComment = (int)ForeStatus.是否评论.否,
                    ShareStatus = (int)ForeStatus.商品晒单状态.待审,
                    ShareTime = DateTime.Now
                };
            var id = BLL.Front.FeProductCommentBo.Instance.Insert(model);
            if (id > 0 && share.Pictures != null && share.Pictures.Any())
            {
                foreach (var item in share.Pictures)
                {
                    var stream = new MemoryStream(Convert.FromBase64String(item));

                    //上传到FTP的路径
                    var subpath = DateTime.Now.ToString("yyyy/MM/dd");
                    var ran = new Random((int)(DateTime.Now.Ticks & 0xffffffffL) | (int)(DateTime.Now.Ticks >> 32));
                    var fileName = string.Format("{0}_{1}", DateTime.Now.ToString("HHmmssfff"), ran.Next());
                    string ftpPath = string.Format("{0}/{1}/Thumbnail460/{2}/{3}.jpg", FtpImageServer,
                                                   ProductThumbnailType.ShowOrder, subpath, fileName);
                    //上传
                    var ftp = new Hyt.Util.Net.FtpUtil(FtpImageServer, FtpUserName, FtpPassword);
                    ftp.Upload(stream, ftpPath);
                    stream.Dispose();

                    var picModel = new FeProductCommentImage()
                        {
                            CommentSysNo = id,
                            CustomerSysNo = share.CustomerSysNo,
                            ImagePath = ftpPath.Replace(FtpImageServer, ""),
                            Status = (int)ForeStatus.商品晒单状态.待审
                        };
                    FeProductCommentImageBo.Instance.Insert(picModel);
                }
                //产品统计分数
                var statisticsModel = PdProductBo.Instance.GetPdProductStatistics(id);
                if (statisticsModel != null)
                {
                    statisticsModel.Shares++;
                    PdProductStatisticsBo.Instance.Update(statisticsModel);
                }
                else
                {
                    statisticsModel = new PdProductStatistics()
                        {
                            ProductSysNo = id,
                            Shares = 1
                        };
                    PdProductStatisticsBo.Instance.Create(statisticsModel);
                }
                result.Status = true;
                result.Message = "晒单成功!";
            }

            return result;
        }

        /// <summary>
        /// 获取商品晒单列表
        /// </summary>
        /// <param name="productSysNo">产品编号</param>
        /// <param name="pageIndex">页码</param>
        /// <returns>商品晒单列表</returns>
        /// <remarks> 2013-8-5 杨浩 创建 </remarks>
        ///  <remarks>2013-08-23 周唐炬 实现</remarks>
        public ResultPager<IList<ShareOrderList>> GetShareOrders(string productSysNo, int pageIndex)
        {
            var result = new ResultPager<IList<ShareOrderList>>() { StatusCode = -1 };
            if (!string.IsNullOrWhiteSpace(productSysNo))
            {
                var obj = BLL.Front.FeProductCommentBo.Instance.Seach(pageIndex, (int)ForeStatus.是否晒单.是, (int)ForeStatus.是否评论.否, (int)ForeStatus.商品晒单状态.已审,
                    0, 0, null, null, null, null, productSysNo);
                var data = obj.TData;

                if (null != data)
                {
                    var list = data.Select(x =>
                        new ShareOrderList
                        {
                            SysNo = x.SysNo,
                            ProductSysNo = x.ProductSysNo,
                            Account = Hyt.Util.WebUtil.HideMobilePhone(x.Account),
                            ReplyCount = x.ReplyCount,
                            ShareTime = x.ShareTime,
                            ShareTitle = x.ShareTitle
                        }).ToList();
                    result.Data = list;
                    result.Status = true;
                    result.StatusCode = 0;
                    result.HasMore = pageIndex < obj.TotalPageCount;
                }
            }
            else
            {
                result.Message = "商品编号不能为空!";
            }
            return result;
        }

        /// <summary>
        /// 获取商品晒单详细
        /// </summary>
        /// <param name="sysNo">晒单系统编号</param>
        /// <returns>商品晒单详细</returns>
        /// <remarks> 2013-8-5 杨浩 创建 </remarks>
        ///  <remarks>2013-08-23 周唐炬 实现</remarks>
        public Result<ShareOrderDetails> GetShareOrderDetails(string sysNo)
        {
            var result = new Result<ShareOrderDetails>() { StatusCode = -1 };
            int id = int.Parse(sysNo);
            if (!string.IsNullOrWhiteSpace(sysNo))
            {
                var model = BLL.Front.FeProductCommentBo.Instance.GetModel(id);
                if (model != null)
                {
                    var data = new ShareOrderDetails()
                    {
                        SysNo = model.SysNo,
                        ProductSysNo = model.ProductSysNo,
                        Account = Hyt.Util.WebUtil.HideMobilePhone(model.Account),
                        ReplyCount = model.ReplyCount,
                        ShareTime = model.ShareTime,
                        ShareTitle = model.ShareTitle,
                        ShareContent = model.ShareContent
                    };
                    model.ShowMyProductImage = FeProductCommentImageBo.Instance.GetFeProductCommentImageByCommentSysNo(id, ForeStatus.晒单图片状态.已审);
                    if (model.ShowMyProductImage != null)
                    {
                        data.Images = model.ShowMyProductImage.Select
                            (
                                x => new ProductCommentImage { ImagePath = FileServer + x.ImagePath }
                            ).ToList();
                    }
                    var replay = FeProductCommentReplyBo.Instance.GetReplyByCommentSysNo(id, ForeStatus.商品评论回复状态.已审);
                    if (replay != null)
                    {
                        model.ReplyCount = replay.Count;
                        data.Replies = replay.Select(x => new ProductCommentReply
                                                              {
                                                                  Account = Hyt.Util.WebUtil.HideMobilePhone(CrCustomerBo.Instance.GetModel(x.CustomerSysNo).Account),
                                                                  ReplyContent = x.ReplyContent,
                                                                  ReplyDate = x.ReplyDate
                                                              }).ToList();
                    }
                    result.Data = data;
                    result.Status = true;
                    result.StatusCode = 1;
                }
            }
            else
            {
                result.Message = "晒单编号不能为空!";
            }
            return result;
        }

        #endregion

        #region 商品团购

        /// <summary>
        /// 获取团购列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <returns>返回团购列表</returns>
        /// <remarks> 2013-8-12 杨浩 创建 </remarks>
        /// <remarks> 2013-9-16 何方 实现 </remarks>
        public ResultPager<IList<GroupShopping>> GetGroupList(int pageIndex)
        {
            var page = GsGroupShoppingBo.Instance.GetGroupShoppingList(pageIndex, 10, null, null, GroupShoppingStatus.团购类型.手机App);

            var items = page.TData.Select(g => new GroupShopping
                {
                    SysNo = g.SysNo,
                    Title = g.Title,
                    Subtitle = g.Subtitle,
                    IconUrl = ProductImageBo.Instance.GetGroupShoppingImagePath(g.IconUrl),
                    RemainingTime = (g.EndTime.Value - DateTime.Now).TotalMilliseconds < 0 ? 0 : (g.EndTime.Value - DateTime.Now).TotalMilliseconds,
                    GroupPrice = g.GroupPrice,
                    TotalPrice = g.TotalPrice,
                    Discount = g.Discount,
                    HaveQuantity = g.HaveQuantity,
                    DisplayOrder = g.DisplayOrder,
                    LimitQuantity = g.LimitQuantity,
                    PromotionSysNo = g.PromotionSysNo,
                    SavePrice = g.TotalPrice - g.GroupPrice,
                    Status = g.Status,
                    RemainingQuantity = g.MaxQuantity - g.HaveQuantity

                }).ToList();

            var Data = new ResultPager<IList<GroupShopping>>

                {
                    Data = items,
                    Status = true,
                    StatusCode = 1,
                    HasMore = page.CurrentPageIndex < page.TotalPageCount
                };

            return Data;

        }

        /// <summary>
        /// 获取团购详情
        /// </summary>
        /// <param name="sysNo">团购编号</param>
        /// <param name="type">类型：5：团购，10：商品</param>
        /// <returns>团购详情</returns>
        /// <remarks> 2013-9-6 杨浩 创建 </remarks>
        public Result<GroupShoppingDetails> GetGroupDetails(string sysNo, int type)
        {
            int id = int.Parse(sysNo);
            if (type == 10)
            {
                id = GsGroupShoppingBo.Instance.GetGroupSysNo(id);
            }

            var temp = Hyt.BLL.Tuan.GsGroupShoppingBo.Instance.Get(id);
            var item = Hyt.BLL.Promotion.GroupShoppingBo.Instance.GetGroupShoppingItem(id);
            var groupShoppingItems = item.Select(t => new GroupShoppingItem
                {
                    ProductSysNo = t.ProductSysNo,
                    ProductName = t.ProductName,
                    Thumbnail = Hyt.BLL.Web.ProductImageBo.Instance.GetProductImagePath(ProductThumbnailType.Image180, t.ProductSysNo)
                }).ToList();

            var data = new GroupShoppingDetails
                {
                    Discount = temp.Discount,
                    DisplayOrder = temp.DisplayOrder,
                    GroupPrice = temp.GroupPrice,
                    HaveQuantity = temp.HaveQuantity,
                    IconUrl = ProductImageBo.Instance.GetGroupShoppingImagePath(temp.IconUrl),
                    LimitQuantity = temp.LimitQuantity,
                    Status = temp.Status,
                    SysNo = temp.SysNo,
                    Title = temp.Title,
                    Subtitle = temp.Subtitle,
                    TotalPrice = temp.TotalPrice,
                    RemainingTime = (temp.EndTime - DateTime.Now).Value.TotalMilliseconds,
                    PromotionSysNo = temp.PromotionSysNo,
                    SavePrice = temp.TotalPrice - temp.GroupPrice,
                    GroupShoppingItems = groupShoppingItems,
                    RemainingQuantity = temp.MaxQuantity - temp.HaveQuantity
                };
            return new Result<GroupShoppingDetails>
                {
                    Data = data,
                    Status = true,
                    StatusCode = 1
                };
        }

        #endregion

        #region 组合商品

        /// <summary>
        /// 获取组合商品
        /// </summary>
        /// <param name="sysNo">主商品编号</param>
        /// <returns>组合商品</returns>
        /// <remarks> 2013-9-9 杨浩 创建 </remarks>
        public Result<IList<Combo>> GetCombo(int sysNo)
        {
            var temp = Hyt.BLL.Promotion.SpComboBo.Instance.GetComboByMasterProductSysNo(sysNo);
            var data = new List<Combo>();
            foreach (var item in temp)
            {
                var t = Hyt.BLL.Promotion.SpComboBo.Instance.GetListByComboSysNo(item.SysNo);
                var comboItem = t.Select(x => new ComboItem
                    {
                        ProductSysNo = x.ProductSysNo,
                        ProductName = x.ProductName,
                        Thumbnail = ProductImageBo.Instance.GetProductImagePath(ProductThumbnailType.Image180, x.SysNo)
                    }).ToList();

                var d = new Combo
                    {
                        SysNo = item.SysNo,
                        PromotionSysNo = item.PromotionSysNo,
                        Title = item.Title,
                        ComboItem = comboItem,
                        //TODO:价格待计算
                        ComboPrice = 100,
                        BasicPrice = 120,
                        SavePrice = 20
                    };
                data.Add(d);
            }
            return new Result<IList<Combo>>
                {
                    Data = data,
                    Status = true,
                    StatusCode = 1
                };
        }

        #endregion

    }
}
