using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Hyt.Infrastructure.Caching;
using Hyt.Model;
using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using PanGu;
using Hyt.Model.WorkflowStatus;
using Hyt.Model.SystemPredefined;
using Hyt.DataAccess.Product;
using System.Data;
using Hyt.Infrastructure.Memory;
using Hyt.BLL.Warehouse;

namespace Hyt.BLL.Web
{
    /// <summary>
    /// 产品搜索业务逻辑
    /// </summary>
    /// <remarks>2013-7-7 黄波 创建</remarks>
    public class ProductIndexBo : BOBase<ProductIndexBo>
    {
        #region 创建索引
        /// <summary>
        /// 创建商品索引
        /// </summary>
        /// <param name="model">商品实体</param>
        /// <returns>是否</returns>
        /// <remarks>2013-12-1 黄波 创建</remarks>
        public bool CreateProductIndex(PdProductIndex model)
        {
            var result = true;
            try
            {
                if (!HasProductIndex(model.SysNo))
                {
                    Hyt.Infrastructure.Lucene.ProductIndex.Instance.AddIndex(model);
                }
                else
                {
                    Hyt.Infrastructure.Lucene.ProductIndex.Instance.UpdateIndex(model);
                }
            }
            catch (Exception ex)
            {
                Hyt.BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, "创建索引", ex);
                result = false;
            }

            return result;
        }

        /// <summary>
        /// 创建商品索引
        /// </summary>
        /// <param name="productSysNo">商品编号</param>
        /// <returns>是否</returns>
        /// <remarks>2013-12-26 黄波 创建</remarks>
        public bool CreateProductIndex(int productSysNo)
        {
            var result = true;

            var productModel = BLL.Product.PdProductBo.Instance.GetAllProduct(productSysNo);
            if (productModel != null)
            {
                try
                {
                    if (!HasProductIndex(productSysNo))
                    {
                        Hyt.Infrastructure.Lucene.ProductIndex.Instance.AddIndex(productModel);
                    }
                    else
                    {
                        Hyt.Infrastructure.Lucene.ProductIndex.Instance.UpdateIndex(productModel);
                    }
                    result = true;
                }
                catch (Exception ex)
                {
                    Hyt.BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, "创建索引", ex);
                }
            }

            return result;
        }
        #endregion

        #region 删除索引
        /// <summary>
        /// 删除索引
        /// </summary>
        /// <param name="productSysNo">商品编号</param>
        /// <returns>是否</returns>
        /// <remarks>2013-12-1 黄波 创建</remarks>
        public bool DeleteProductIndex(int productSysNo)
        {
            var result = false;
            try
            {
                if (HasProductIndex(productSysNo))
                {
                    Hyt.Infrastructure.Lucene.ProductIndex.Instance.DeleteIndex(productSysNo);
                }
                result = true;
            }
            catch (Exception ex)
            {
                Hyt.BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, "删除索引", ex);
            }
            return result;
        }
        #endregion

        #region 更新索引
        /// <summary>
        /// 更新索引
        /// </summary>
        /// <param name="model">商品实体</param>
        /// <returns>是否</returns>
        /// <remarks>2013-12-1 黄波 创建</remarks>
        public bool UpdateProductIndex(PdProductIndex model)
        {
            var result = false;
            try
            {
                if (HasProductIndex(model.SysNo))
                {
                    Hyt.Infrastructure.Lucene.ProductIndex.Instance.UpdateIndex(model);
                }
                else
                {
                    Hyt.Infrastructure.Lucene.ProductIndex.Instance.AddIndex(model);
                }
                result = true;
            }
            catch (Exception ex)
            {
                Hyt.BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, "更新索引", ex);
            }
            return result;
        }

        /// <summary>
        /// 更新索引
        /// </summary>
        /// <param name="productSysNo">商品编号</param>
        /// <returns>是否</returns>
        /// <remarks>2013-12-26 黄波 创建</remarks>
        public bool UpdateProductIndex(int productSysNo)
        {
            var result = false;

            var productModel = BLL.Product.PdProductBo.Instance.GetAllProduct(productSysNo);
            if (productModel != null)
            {
                try
                {
                    if (HasProductIndex(productSysNo))
                    {
                        Hyt.Infrastructure.Lucene.ProductIndex.Instance.UpdateIndex(productModel);
                    }
                    else
                    {
                        Hyt.Infrastructure.Lucene.ProductIndex.Instance.AddIndex(productModel);
                    }
                    result = true;
                }
                catch (Exception ex)
                {
                    Hyt.BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, "更新索引", ex);
                }
            }

            return result;
        }
        #endregion

        #region 商品是否存在索引
        /// <summary>
        /// 商品是否存在索引
        /// </summary>
        /// <param name="productSysNo">商品编号</param>
        /// <returns>是否</returns>
        /// <remarks>2013-12-1 黄波 创建</remarks>
        public bool HasProductIndex(int productSysNo)
        {
            var returnValue = true;
            BooleanQuery query = new BooleanQuery();

            query.Add(new TermQuery(new Term("SysNo", productSysNo.ToString())),
                        BooleanClause.Occur.SHOULD);
            //搜索
            var searchIndex = Hyt.Infrastructure.Lucene.ProductIndex.Searcher;
            Hits hits = searchIndex.Search(query);

            returnValue = hits.Length() > 0;

         //   searchIndex.Close();
            return returnValue;
        }

        #endregion

        #region 搜索商品
        /// <summary>
        /// 搜索商品(从索引)
        /// </summary>
        /// <param name="filter">搜索条件</param>
        /// <returns>商品列表</returns>
        /// <remarks>2014-1-8 黄波 创建</remarks>
        public IList<PdProductIndex> Search(Model.Parameter.ParaLuceneSearchFilter filter)
        {
            int pageIndex = filter.PageIndex;
            int pageCount = filter.PageCount;
            int recCount = filter.RecCount;

            var reslut = Search(
                filter.Key,
                filter.CategorySysNo,
                filter.AttributeOptions,
                filter.PageSize,
                ref pageIndex,
                ref pageCount,
                ref recCount,
                filter.HighLight,
                (int)filter.Sort,
                 filter.IsDescending,
                 filter.ProductSysNo,
                 filter.PriceSource,
                 filter.LevelSysNo
            );

            filter.PageIndex = pageIndex;
            filter.PageCount = pageCount;
            filter.RecCount = recCount;

            return reslut;
        }

        #region 搜索关键词分词
        /// <summary>
        /// 搜索关键词分词
        /// </summary>
        /// <param name="keywords">搜索关键字</param>
        /// <returns>分词结果</returns>
        /// <remarks>2014-1-8 黄波 创建</remarks>
        private string GetKeyWordsSplitBySpace(string keywords)
        {
            StringBuilder result = new StringBuilder("");

            ICollection<WordInfo> words = new PanGuTokenizer().SegmentToWordInfos(keywords);

            foreach (WordInfo word in words)
            {
                if (word == null)
                {
                    continue;
                }              
                result.AppendFormat("{0}^{1}.0 ", word.Word, (int)Math.Pow(3, word.Rank));
            }

            return result.ToString().Trim();
        }
        #endregion

        /// <summary>
        /// 检查关键字是否为ERP编码
        /// </summary>
        /// <param name="val">关键字</param>
        /// <returns>true/false</returns>
        /// <remarks>2014-2-14 黄波 创建</remarks>
        private bool IsErpCode(string val)
        {
            return Regex.IsMatch(val, @"^03\d{10}$",RegexOptions.None);
        }

        /// <summary>
        /// 产品搜索
        /// 搜索商品ID
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <returns>商品</returns>
        /// <remarks>2013-11-15 邵斌 创建</remarks>
        public PdProductIndex SearchProduct(int productSysNo)
        {
            int defaultSetting = 1;
            var result = Search(null, null, null, 1, ref defaultSetting, ref defaultSetting, ref defaultSetting, false, 1, false, productSysNo);

            if (result != null && result.Count == 1)
            {
                return result[0];
            }
            return null;
        }
        /// <summary>
        /// 获取分销商所有商品信息
        /// </summary>
        /// <param name="dealerSysNo">经销商系统编号</param>
        /// <returns></returns>
        /// <remarks>2017-01-04 杨浩 创建</remarks>
        public DataTable GetDealerAllProductToDataTable(int dealerSysNo)
        {
            return MemoryProvider.Default.Get<DataTable>(string.Format(KeyConstant.DealerAllProduct_, dealerSysNo), 60 * 24 * 360, () =>
            {
                return IPdProductDao.Instance.GetDealerAllProductToDataTable(dealerSysNo);
            });
        }
        /// <summary>
        /// 获取全部商品信息(用于生成索引文件)
        /// </summary>
        /// <returns></returns>
        /// <remarks>2013-08-02 杨浩 创建</remarks>
        public DataTable GetAllProductToDataTable()
        {
            var dt = MemoryProvider.Default.Get<DataTable>(KeyConstant.AllProduct, 60 * 24 * 360, () =>
            {
                return IPdProductDao.Instance.GetAllProductToDataTable();
            });

            return dt;
        }
        /// <summary>
        /// RowFilter特殊字符转移
        /// </summary>
        /// <param name="oldStr"></param>
        /// <returns></returns>
        /// <remarks>2016-1-5 杨浩 添加</remarks>
        private  string Replace(string oldStr)
        {
            if (string.IsNullOrEmpty(oldStr))
            {
                return "";
            }
            string str2 = Regex.Replace(oldStr, @"[\[\+\\\|\(\)\^\*\""\]'%~#-&]", delegate(Match match)
            {
                if (match.Value == "'")
                {
                    return "''";
                }
                else
                {
                    return "[" + match.Value + "]";
                }
            });
            return str2;
        }
        /// <summary>
        /// 产品搜索
        /// 从索引
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
        /// <remarks>2016-12-26 杨浩 创建</remarks>
        public IList<PdProductIndex> SearchFromDataBase(string key
            , int? categorySysNo
            , string attributes
            , int pageSize
            , ref int pageIndex
            , ref int pageCount
            , ref int recCount
            , bool highLight = false
            , int sort = 1
            , bool isDescending = false
            , int productSysNo = 0
            , ProductStatus.产品价格来源 priceSource = ProductStatus.产品价格来源.会员等级价
            , int priceSourceSysNo = CustomerLevel.初级
            , bool showNotFrontEndOrder = false
            , int brandSysNo = 0
            , string ProductGroupCode = null
            , int originSysNo = 0
            , int dealerSysNo = 0
            , int warehouseSysNo = 0
            , int productType = 0
            )
        {
            if (BLL.Config.Config.Instance.GetGeneralConfig().IsDealerMall == 1)
                dealerSysNo = 0;
            var returnValue = new List<PdProductIndex>();

            if (dealerSysNo == 347 || dealerSysNo == 336 || dealerSysNo == 44 || dealerSysNo == 14)
                dealerSysNo = 0;

            var dt = GetDealerAllProductToDataTable(dealerSysNo);  
            
            pageIndex = pageIndex == 0 ? 1 : pageIndex;
            key = key ?? ""; //查询设置初始值
            string where = " IsFrontDisplay=1 and Status=1 ";

            
            if (warehouseSysNo == -1)//只查询有库存的
            {
                var productSysNoList=PdProductStockBo.Instance.GetInvalidInventoryProductSysNoList();
                if (productSysNoList != null && productSysNoList.Count>0)
                    where += " and SysNo not in(" + string.Join(",",productSysNoList.ToArray()) + ")";
            }

            #region 关键字搜索
            string keywords = Replace(key.Trim());
            if (!string.IsNullOrWhiteSpace(keywords))
            {
                if (!IsErpCode(keywords))
                    where += " and (ProductName like '*" + keywords + "*' or ProductSubName like '*" + keywords + "*')";
                else
                    where += " and ErpCode='" + keywords + "'";
            }
            #endregion

            #region 经销商系统编号
            //if (dealerSysNo > 0 && BLL.Config.Config.Instance.GetGeneralConfig().IsDealerMall == 0)
            //{
            //   where += " and DealerSysNos like '*,"+dealerSysNo+",*'";                  
            //}
            #endregion

            #region 分类搜索
            if (categorySysNo.HasValue && categorySysNo.Value>0)
            {
                where += " and (Category=" + categorySysNo.Value.ToString() + " or  AssociationCategory like '*," + categorySysNo.Value.ToString() + ",*' ";
                //where += " or SysNos like '*," + categorySysNo.Value.ToString() + ",*' ";  
                //所有子分类
                var childCategoryList = Hyt.BLL.Web.PdCategoryBo.Instance.GetChildAllCategory(categorySysNo.Value);
                foreach (var item in childCategoryList)
                {
                    where += " or Category=" + item.SysNo.ToString();                          
                }
                where += " ) ";              
            }
            #endregion
            if (!showNotFrontEndOrder)
            {
               where += " and  CanFrontEndOrder=1 ";               
            }
            #region 仓库系统编号
            if (warehouseSysNo > 0)
            {
               where += " and WarehouseSysNos like '*,"+warehouseSysNo+",*'";           
            }
            #endregion
            #region 属性搜索
            if (attributes != ""&&attributes!=null)
            {
              var _attributes=attributes.Split(',');
              foreach (var item in _attributes)
              {
                  if(item!="")
                    where += " and Attributes like '*:"+item.ToString()+",*'";    
              }                          
            }
            #endregion
            #region 品牌搜索
            if (brandSysNo != 0)
            {
               where += " and BrandSysNo="+brandSysNo.ToString();                       
            }
            #endregion
            #region 商品组搜索
            if (!string.IsNullOrWhiteSpace(ProductGroupCode))
            {
              where += " and ProductGroupCode like '*,"+ProductGroupCode+",*'";            
            }
           #endregion             
            #region 原产地搜索
            if (originSysNo != 0)
            {
               where += " and OriginSysNo="+ originSysNo.ToString();  
            }
            #endregion
            #region 产品类型
            if (productType > 0)
            {
              where += " and ProductType="+ productType.ToString();  
            }
            #endregion
            #region 商品系统编号搜索

            if (productSysNo > 0)
            {
               where += " and SysNo="+ productSysNo.ToString();  
            }
            #endregion
            #region 产品类型
            if (productType > 0)
            {
               where += " and ProductType="+ productType.ToString();  
            }
            #endregion

            dt.DefaultView.RowFilter = where;
      
            #region 排序
            //isDescending true为降序 false为升序
            switch (Math.Abs(sort))
            {
                case 1://销量
                    dt.DefaultView.Sort = "SalesCount " + (isDescending ? "desc" : "");
                    break;
                case 2://价格
                    dt.DefaultView.Sort = "Price " + (isDescending ? "desc" : "");                
                    break;
                case 3://评分
                    dt.DefaultView.Sort = "CommentCount " + (isDescending ? "desc" : "");      
                  
                    break;
                case 4://上架时间
                    dt.DefaultView.Sort = "CreatedDate " + (isDescending ? "desc" : "");                      
                    break;
                default:
                    dt.DefaultView.Sort = "SalesCount " + (isDescending ? "desc" : "");           
                    break;
            }
            #endregion
            #endregion
            var _dt=dt.DefaultView.ToTable();
            recCount = _dt.Rows.Count;
            pageCount = (int)Math.Ceiling((double)recCount / (double)pageSize);
            if (pageIndex <= 0) pageIndex = 1;
            if (pageIndex > pageCount) pageIndex = pageCount;
            var recordIndex = Math.Max((pageIndex - 1) * pageSize, 0);
            PdProductIndex pdProductIndex;                  
            while (recordIndex < recCount && returnValue.Count < pageSize)
            {
                try
                {
                   pdProductIndex = Hyt.Infrastructure.Lucene.ProductIndex.Instance.DataRowToModel(_dt.Rows[recordIndex]);
                   if (dealerSysNo == 0)
                   {
                       var rankPrice = GetProductRankPrice(pdProductIndex.Prices, pdProductIndex.BasicPrice, priceSource, priceSourceSysNo);
                       pdProductIndex.RankPrice = rankPrice;
                   }
                   else
                   {
                       pdProductIndex.RankPrice = pdProductIndex.Price;
                   }
                   var pdimgmodel = Hyt.BLL.Product.PdProductImageBo.Instance.GetModelByPdSysNo(pdProductIndex.SysNo);
                   if (pdimgmodel != null)
                   {
                       pdProductIndex.ProductImage = Hyt.BLL.Web.ProductImageBo.Instance.GetProductImagePath(Hyt.BLL.Product.PdProductImageBo.Instance.GetProductImg(pdProductIndex.SysNo).Count == 0 ? "" : (Hyt.BLL.Product.PdProductImageBo.Instance.GetModelByPdSysNo(pdProductIndex.SysNo).ImageUrl), Hyt.BLL.Web.ProductThumbnailType.Base);
                   }
                   else
                   {
                       pdProductIndex.ProductImage = Hyt.BLL.Web.ProductImageBo.Instance.GetProductImagePath(Hyt.BLL.Web.ProductThumbnailType.Image460, pdProductIndex.SysNo);
                   }                                    
                   returnValue.Add(pdProductIndex);
                 }
                 catch (Exception ex)
                 {
                        BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, ex.Message, ex);
                        throw;
                 }
                 finally
                 {
                    recordIndex++;
                 }
            }
            return returnValue;
        }
        /// <summary>
        /// 产品搜索
        /// 从索引
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
        /// <param name="showNotFrontEndOrder">搜索前台不能下单的商品(2014-2-14 黄波 添加 物流APP需要)</param>
        /// <returns>商品列表</returns>
        /// <remarks>2013-08-08 黄波 创建</remarks>
        /// <remarks>2013-11-12 邵斌 修改 添加搜索商品系统编号：该方法暂时闲置</remarks>
        /// <remarks>2013-12-23 邵斌 添加前台是否下单字段</remarks>
        /// <remarks>2014-02-14 黄波 添加是否搜索前台不能下单的商品(物流APP需要)</remarks>
        public IList<PdProductIndex> Search(string key
            , int? categorySysNo
            , List<int> attributes
            , int pageSize
            , ref int pageIndex
            , ref int pageCount
            , ref int recCount
            , bool highLight = false
            , int sort = 1
            , bool isDescending = false
            , int productSysNo = 0
            , ProductStatus.产品价格来源 priceSource = ProductStatus.产品价格来源.会员等级价
            , int priceSourceSysNo = CustomerLevel.初级
            , bool showNotFrontEndOrder = false)
        {
            var returnValue = new List<PdProductIndex>();
            var indexSearch = Hyt.Infrastructure.Lucene.ProductIndex.Searcher;
            try
            {
                pageIndex = pageIndex == 0 ? 1 : pageIndex;
                key = key ?? ""; //查询设置初始值

                #region 搜索条件
                BooleanQuery query = new BooleanQuery();
                BooleanQuery childQuery;
                BooleanQuery esenQuery;

                #region 关键字搜索
                string keywords = key.Trim();
                if (!string.IsNullOrWhiteSpace(keywords))
                {
                    childQuery = new BooleanQuery();
                    esenQuery = new BooleanQuery();
                    if (!IsErpCode(keywords))
                    {                     
                        ////全词去空格
                        //esenQuery.Add(new TermQuery(new Term("ProductName", Regex.Replace(keywords, @"\s", ""))),
                        //        BooleanClause.Occur.SHOULD);
                        //esenQuery.SetBoost(3.0F);
                        //esenQuery.Add(new TermQuery(new Term("ProductSubName", Regex.Replace(keywords, @"\s", ""))),
                        //        BooleanClause.Occur.SHOULD);
                        //esenQuery.SetBoost(3.0F);
                        //childQuery.Add(esenQuery, BooleanClause.Occur.SHOULD);

                        esenQuery = new BooleanQuery();
                        //分词 盘古分词
                        var keyWordsSplitBySpace = GetKeyWordsSplitBySpace(keywords);

                        if (string.IsNullOrWhiteSpace(keyWordsSplitBySpace))
                        {
                            return null;
                        }

                        keyWordsSplitBySpace = string.Format("{0}^{1}.0", keywords, (int)Math.Pow(3, 5));                   

                        QueryParser productNameQueryParser = new QueryParser(Lucene.Net.Util.Version.LUCENE_29, "ProductName", new PanGuAnalyzer(true));
                        Query productNameQuery = productNameQueryParser.Parse(keyWordsSplitBySpace);
                        childQuery.Add(productNameQuery, BooleanClause.Occur.SHOULD);

                        //以什么开头，输入“ja”就可以搜到包含java和javascript两项结果了
                        Query prefixQuery_productName = new PrefixQuery(new Term("ProductName", key));
                        //直接模糊匹配,假设你想搜索跟‘wuzza’相似的词语,你可能得到‘fuzzy’和‘wuzzy’。
                        Query fuzzyQuery_productName = new FuzzyQuery(new Term("ProductName", key));
                        //通配符搜索
                        Query wildcardQuery_productName = new WildcardQuery(new Term("ProductName",string.Format("*{0}*",key.Trim())));


                        childQuery.Add(prefixQuery_productName, BooleanClause.Occur.SHOULD);
                        childQuery.Add(fuzzyQuery_productName, BooleanClause.Occur.SHOULD);
                        childQuery.Add(wildcardQuery_productName, BooleanClause.Occur.SHOULD);
                        //esenQuery.Add(new QueryParser("ProductName", new PanGuAnalyzer(true)).Parse(keyWordsSplitBySpace), BooleanClause.Occur.SHOULD);
                        //esenQuery.Add(new QueryParser("ProductSubName", new PanGuAnalyzer(true)).Parse(keyWordsSplitBySpace), BooleanClause.Occur.SHOULD);

                        ////分词  按空格
                        //var keyColl = keywords.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        //foreach (var item in keyColl)
                        //{
                        //    esenQuery.Add(new TermQuery(new Term("ProductName", item)),
                        //        BooleanClause.Occur.SHOULD);
                        //    esenQuery.Add(new TermQuery(new Term("ProductSubName", item)),
                        //        BooleanClause.Occur.SHOULD);
                        //}
                        //esenQuery.SetBoost(2.9F);
                        //childQuery.Add(esenQuery, BooleanClause.Occur.SHOULD);
                    }
                    else
                    {
                        esenQuery.Add(new TermQuery(new Term("ErpCode", Regex.Replace(keywords, @"\s", ""))),
                                BooleanClause.Occur.SHOULD);
                        esenQuery.SetBoost(3.0F);
                        childQuery.Add(esenQuery, BooleanClause.Occur.SHOULD);
                    }

                    query.Add(childQuery, BooleanClause.Occur.MUST);
                }
                #endregion

                #region 分类搜索
                if (categorySysNo.HasValue && categorySysNo.Value != 0)
                {
                    childQuery = new BooleanQuery();

                    esenQuery = new BooleanQuery();
                    esenQuery.Add(new TermQuery(new Term("Category", categorySysNo.Value.ToString())),
                               BooleanClause.Occur.SHOULD);
                    esenQuery.SetBoost(3.0F);
                    childQuery.Add(esenQuery, BooleanClause.Occur.SHOULD);

                    esenQuery = new BooleanQuery();
                    esenQuery.Add(new WildcardQuery(new Term("AssociationCategory", string.Format("*,{0},*", categorySysNo.Value.ToString()))),
                            BooleanClause.Occur.SHOULD);
                    esenQuery.SetBoost(2.8F);
                    childQuery.Add(esenQuery, BooleanClause.Occur.SHOULD);

                    //所有子分类
                    var childCategoryList = Hyt.BLL.Web.PdCategoryBo.Instance.GetChildAllCategory(categorySysNo.Value);
                    foreach (var item in childCategoryList)
                    {
                        esenQuery = new BooleanQuery();
                        esenQuery.Add(new TermQuery(new Term("Category", item.SysNo.ToString())),
                                   BooleanClause.Occur.SHOULD);
                        esenQuery.SetBoost(3.0F);
                        childQuery.Add(esenQuery, BooleanClause.Occur.SHOULD);

                        esenQuery = new BooleanQuery();
                        esenQuery.Add(new WildcardQuery(new Term("AssociationCategory", string.Format("*,{0},*", item.SysNo.ToString()))),
                                BooleanClause.Occur.SHOULD);
                        childQuery.Add(esenQuery, BooleanClause.Occur.SHOULD);
                    }

                    query.Add(childQuery, BooleanClause.Occur.MUST);
                }
                #endregion

                #region 属性搜索
                if (attributes != null)
                {
                    childQuery = new BooleanQuery();
                    esenQuery = new BooleanQuery();

                    foreach (var item in attributes)
                    {
                        esenQuery.Add(new WildcardQuery(new Term("Attributes", string.Format("*,*:{0},*", item.ToString()))),
                                BooleanClause.Occur.MUST);
                    }
                    childQuery.Add(esenQuery, BooleanClause.Occur.MUST);

                    query.Add(childQuery, BooleanClause.Occur.MUST);
                }
                #endregion

                #region 品牌搜索
                //if (brandSysNo.Value != 0)
                //{
                //    childQuery = new BooleanQuery();
                //    childQuery.Add(new TermQuery(new Term("BrandSysNo", brandSysNo.Value.ToString())),
                //               BooleanClause.Occur.SHOULD);
                //    childQuery.SetBoost(3.0F);
                //    query.Add(childQuery, BooleanClause.Occur.MUST);
                //}
                #endregion

                #region 仅搜索有效的商品

                childQuery = new BooleanQuery();
                childQuery.Add(new TermQuery(new Term("Status", ((int)Hyt.Model.WorkflowStatus.ProductStatus.产品上线状态.有效).ToString())),
                           BooleanClause.Occur.SHOULD);
                query.Add(childQuery, BooleanClause.Occur.MUST);

                //2013-12-23 邵斌 添加前台是否下单字段
                if (!showNotFrontEndOrder)
                {
                    childQuery = new BooleanQuery();
                    childQuery.Add(new TermQuery(new Term("CanFrontEndOrder", ((int)Hyt.Model.WorkflowStatus.ProductStatus.商品是否前台下单.是).ToString())),
                               BooleanClause.Occur.SHOULD);
                    query.Add(childQuery, BooleanClause.Occur.MUST);
                }

                #endregion

                #region 排序

                //isDescending true为降序 false为升序
                SortField sf = null;
                switch (Math.Abs(sort))
                {
                    case 1://销量
                        sf = new SortField("SalesCount", SortField.INT, isDescending);
                        break;
                    case 2://价格
                        sf = new SortField("BasicPrice", SortField.FLOAT, isDescending);
                        break;
                    case 3://评分
                        sf = new SortField("CommentCount", SortField.INT, isDescending);
                        break;
                    case 4://上架时间
                        sf = new SortField("CreatedDate", SortField.STRING, isDescending);
                        break;
                    default:
                        sf = new SortField(null, SortField.SCORE, false);
                        break;
                }

                Sort luceneSort;                            //排序对象

                //默认匹配度，表明是对固定信息进行搜索，所以就要进行先安匹配度来排序。这样用户搜索的商品将排在前面，方便用户筛选
                if (Math.Abs(sort) != (int)CommonEnum.LuceneProductSortType.默认匹配度)
                {
                    //无搜索关键字的时候就按模式设置的进行排序
                    luceneSort = new Sort();
                    luceneSort.SetSort(sf);
                }
                else
                {
                    //收搜索关键字时，就要先安匹配度进行排序，然后才是设置排序。
                    luceneSort = new Sort(new SortField[] { SortField.FIELD_SCORE, sf });
                }

                #endregion

                #region 商品系统编号搜索

                if (productSysNo > 0)
                {
                    childQuery = new BooleanQuery();

                    esenQuery = new BooleanQuery();
                    esenQuery.Add(new TermQuery(new Term("SysNo", productSysNo.ToString())),
                               BooleanClause.Occur.SHOULD);
                    esenQuery.SetBoost(3.0F);
                    childQuery.Add(esenQuery, BooleanClause.Occur.SHOULD);

                    query.Add(childQuery, BooleanClause.Occur.MUST);
                }

                #endregion

                #endregion

                Hits hits = indexSearch.Search(query, luceneSort);

                recCount = hits.Length();
                pageCount = (int)Math.Ceiling((double)recCount / (double)pageSize);
                if (pageIndex <= 0) pageIndex = 1;
                if (pageIndex > pageCount) pageIndex = pageCount;

                var recordIndex = Math.Max((pageIndex - 1) * pageSize, 0);

                PdProductIndex pdProductIndex;
                var simpleHtmlFormatter = new PanGu.HighLight.SimpleHTMLFormatter("<font color=\"red\">", "</font>");
                var T_ProductName = "";
                while (recordIndex < recCount && returnValue.Count < pageSize)
                {
                    try
                    {
                        pdProductIndex = Hyt.Infrastructure.Lucene.ProductIndex.Instance.DocumentToModel(hits.Doc(recordIndex));
                        string productName = pdProductIndex.ProductName;
                        if (highLight && !string.IsNullOrEmpty(key))
                        {
                            var highlighter = new PanGu.HighLight.Highlighter(simpleHtmlFormatter, new PanGu.Segment())
                            {
                                FragmentSize = 50
                            };
                            T_ProductName = highlighter.GetBestFragment(key.Trim(), pdProductIndex.ProductName);
                            if (!string.IsNullOrWhiteSpace(T_ProductName))
                            {
                                pdProductIndex.ProductName = T_ProductName;
                            }
                        }
                        pdProductIndex.RankPrice = GetProductRankPrice(pdProductIndex.Prices, pdProductIndex.BasicPrice, priceSource, priceSourceSysNo);

                        returnValue.Add(pdProductIndex);
                    }
                    catch (Exception ex)
                    {
                        BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, ex.Message, ex);
                    }
                    finally
                    {
                        recordIndex++;
                    }
                }
            }
            catch (Exception ex)
            {
                BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, ex.Message, ex);
            }
            finally
            {
               // indexSearch.Close();
            }
            return returnValue;
        }
      

        #region 搜索广告组商品

        /// <summary>
        /// 搜索广告组商品(仅返回有效的商品)
        /// </summary>
        /// <param name="items">商品系统编号列表(多个逗号分隔)</param>
        /// <param name="dealerSysNo">经销商系统编号</param>
        /// <param name="priceSourceSysNo">产品价格来源</param>
        /// <param name="priceSourceSysNo">产品价格来源编号</param>
        /// <param name="isFrontDisplay">前台显示</param>
        /// <returns>广告组商品详细信息</returns>
        /// <remarks>2013-08-12 杨浩 创建</remarks>
        public IList<PdProductIndex> Search(int dealerSysNo,string productSysNoList
            ,ProductStatus.产品价格来源 priceSource = ProductStatus.产品价格来源.会员等级价
            , int priceSourceSysNo = CustomerLevel.初级, int isFrontDisplay=1)
        {
            if (BLL.Config.Config.Instance.GetGeneralConfig().IsDealerMall == 1)
                dealerSysNo = 0;

            var returnValue = new List<PdProductIndex>();
            var dt = GetDealerAllProductToDataTable(dealerSysNo);
            if (productSysNoList == "")
                return returnValue;

            productSysNoList = productSysNoList.Trim(',');

            string where = "Status=1 and SysNo in("+productSysNoList+")";
            if (isFrontDisplay >= 0)
                where += " and IsFrontDisplay=" + isFrontDisplay;
            dt.DefaultView.RowFilter = where;
            var rows=dt.DefaultView.ToTable().Rows;

            PdProductIndex pdProductIndex;

            foreach (DataRow row in rows)
            {
                try
                {
                    pdProductIndex = Hyt.Infrastructure.Lucene.ProductIndex.Instance.DataRowToModel(row);
                    if (dealerSysNo == 0)
                    {
                        var rankPrice = GetProductRankPrice(pdProductIndex.Prices, pdProductIndex.BasicPrice, priceSource, priceSourceSysNo);
                        pdProductIndex.RankPrice = rankPrice;
                    }
                    else
                    {
                        pdProductIndex.RankPrice = pdProductIndex.Price;
                    }
                    returnValue.Add(pdProductIndex);
                }
                catch (Exception ex)
                {
                    BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, ex.Message, ex);
                    throw;
                }             
            }

            return returnValue;
        }
        /// <summary>
        /// 搜索广告组商品(仅返回有效的商品)
        /// </summary>
        /// <param name="items">广告组商品列表</param>
        /// <returns>广告组商品详细信息</returns>
        /// <remarks>2013-08-12 黄波 创建</remarks>
        public IList<PdProductIndex> Search(List<FeProductItem> items
            , ProductStatus.产品价格来源 priceSource = ProductStatus.产品价格来源.会员等级价
            , int priceSourceSysNo = CustomerLevel.初级)
        {

           


            var returnValue = new List<PdProductIndex>();
            if (items == null || items.Count <= 0)
            {
                return returnValue;
            }

            var query = new BooleanQuery();
            var childQuery = new BooleanQuery();

            foreach (var item in items)
            {
                childQuery = new BooleanQuery();
                childQuery.Add(new TermQuery(new Term("SysNo", item.ProductSysNo.ToString())),
                          BooleanClause.Occur.MUST);
                childQuery.Add(new TermQuery(new Term("Status", ((int)Hyt.Model.WorkflowStatus.ProductStatus.产品上线状态.有效).ToString())),
                          BooleanClause.Occur.MUST);
                childQuery.Add(new TermQuery(new Term("CanFrontEndOrder", ((int)Hyt.Model.WorkflowStatus.ProductStatus.商品是否前台下单.是).ToString())),
                          BooleanClause.Occur.MUST);
                query.Add(childQuery, BooleanClause.Occur.SHOULD);
            }

            //搜索
            var searchIndex = Hyt.Infrastructure.Lucene.ProductIndex.Searcher;
            Hits hits = searchIndex.Search(query);

            PdProductIndex pdProductIndex;
            FeProductItem tmpFeProductItem;

            for (var i = 0; i < hits.Length(); i++)
            {
                try
                {
                    pdProductIndex = Hyt.Infrastructure.Lucene.ProductIndex.Instance.DocumentToModel(hits.Doc(i));
                    tmpFeProductItem = items.Find(o =>
                    {
                        return (o.ProductSysNo == pdProductIndex.SysNo);
                    });
                    if (tmpFeProductItem != null)
                    {
                        pdProductIndex.DisplayOrder = tmpFeProductItem.DisplayOrder;
                        pdProductIndex.DispalySymbol = tmpFeProductItem.DispalySymbol;
                    }

                    pdProductIndex.RankPrice = GetProductRankPrice(pdProductIndex.Prices, pdProductIndex.BasicPrice, priceSource, priceSourceSysNo);

                    returnValue.Add(pdProductIndex);
                }
                catch { }
            }
            //searchIndex.Close();

            returnValue.Sort(
                delegate(PdProductIndex x, PdProductIndex y) { return y.DisplayOrder.CompareTo(x.DisplayOrder); });

            return returnValue;
        }
        #endregion

        #region 根据产品编号搜索产品
        /// <summary>
        /// 根据产品编号搜索产品
        /// （从索引）
        /// </summary>
        /// <param name="productSysNoList">产品编号列表</param>
        /// <returns>产品列表</returns>
        /// <remarks>2013-08-12 黄波 创建</remarks>
        public IList<PdProductIndex> Search(List<int> productSysNoList
            , ProductStatus.产品价格来源 priceSource = ProductStatus.产品价格来源.会员等级价
            , int priceSourceSysNo = CustomerLevel.初级)
        {
            var returnValue = new List<PdProductIndex>();
            if (productSysNoList.Count <= 0)
            {
                return returnValue;
            }

            BooleanQuery query = new BooleanQuery();
            //如果没有搜索条件 则返回所有
            foreach (var item in productSysNoList)
            {
                query.Add(new TermQuery(new Term("SysNo", item.ToString())),
                        BooleanClause.Occur.SHOULD);
            }
            //搜索
            var searchIndex = Hyt.Infrastructure.Lucene.ProductIndex.Searcher;
            Hits hits = searchIndex.Search(query);

            PdProductIndex pdProductIndex;

            for (var i = 0; i < hits.Length(); i++)
            {
                try
                {
                    pdProductIndex = Hyt.Infrastructure.Lucene.ProductIndex.Instance.DocumentToModel(hits.Doc(i));

                    pdProductIndex.RankPrice = GetProductRankPrice(pdProductIndex.Prices, pdProductIndex.BasicPrice, priceSource, priceSourceSysNo);

                    returnValue.Add(pdProductIndex);
                }
                catch { }
            }

          //  searchIndex.Close();
            return returnValue;
        }
        #endregion

        #region 根据产品编号搜索产品
        /// <summary>
        /// 根据产品编号搜索产品
        /// （从索引）
        /// </summary>
        /// <param name="productSysNo">产品编号</param>
        /// <returns>产品列表</returns>
        /// <remarks>2013-08-12 黄波 创建</remarks>
        public PdProductIndex Search(int productSysNo
            , ProductStatus.产品价格来源 priceSource = ProductStatus.产品价格来源.会员等级价
            , int priceSourceSysNo = CustomerLevel.初级)
        {
            var returnValue = new PdProductIndex();
            BooleanQuery query = new BooleanQuery();

            query.Add(new TermQuery(new Term("SysNo", productSysNo.ToString())),
                        BooleanClause.Occur.SHOULD);
            //搜索
            var searchIndex = Hyt.Infrastructure.Lucene.ProductIndex.Searcher;
            Hits hits = searchIndex.Search(query);

            for (var i = 0; i < hits.Length(); i++)
            {
                try
                {
                    var pdProductIndex = Hyt.Infrastructure.Lucene.ProductIndex.Instance.DocumentToModel(hits.Doc(i));

                    pdProductIndex.RankPrice = GetProductRankPrice(pdProductIndex.Prices, pdProductIndex.BasicPrice, priceSource, priceSourceSysNo);

                    returnValue = pdProductIndex;
                }
                catch { }
            }

         //   searchIndex.Close();
            return returnValue;
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 获取商品等级价格
        /// 获取不到则返回基础价格
        /// </summary>
        /// <param name="prices">等级价格字符串</param>
        /// <param name="defPrice">默认价格（一般是基础价格）</param>
        /// <param name="priceSource">产品价格来源</param>
        /// <param name="priceSourceSysNo">产品价格来源编号(会员等级编号)</param>
        /// <returns>商品等级价格</returns>
        /// <remarks>2013-11-30 黄波 创建</remarks>
        private decimal GetProductRankPrice(
            string prices
            , decimal defPrice
            , ProductStatus.产品价格来源 priceSource
            , int priceSourceSysNo)
        {
            var rankPrice = defPrice;

            var regexPattern = ",[0-9/.]*" + ":" + ((int)priceSource).ToString() + ":" + priceSourceSysNo.ToString() + ",";
            var regexPrice = Regex.Match(prices, regexPattern);
            if (regexPrice.Success)
            {
                try
                {
                    rankPrice = decimal.Parse(regexPrice.Value.Split(new char[] { ':' })[0].TrimStart(new char[] { ',' }));
                }
                catch { }
            }
            return rankPrice;
        }
        #endregion
    }
}