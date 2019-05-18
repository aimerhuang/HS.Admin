using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Infrastructure.Caching;
using Hyt.Infrastructure.Memory;

namespace Hyt.BLL.Cache
{
    /// <summary>
    /// 删除缓存
    /// </summary>
    /// <remarks>2013-08-07 杨浩 创建</remarks>
    public class DeleteCache
    {
        /// <summary>
        /// 删除微信授权码
        /// </summary>
        /// <param name="dealerSysNo">经销商编号</param>
        /// <remarks>
        /// 2016-1-14 杨浩 创建
        /// 2016-2-26 杨浩 将微信授权码转入Memcached
        /// </remarks>
        public static void AccessToken(int dealerSysNo)
        {
            using (var service = new Hyt.Infrastructure.Communication.ServiceProxy<Hyt.Service.Contract.WeiXin.IWebChatService>())
            {
               string key = string.Format(KeyConstant.WeixinAccessToken_,dealerSysNo);
               var result = service.Channel.RemoveCache(key);
               key = string.Format(KeyConstant.WeixinJsTicket_,dealerSysNo);
               service.Channel.RemoveCache(key);
            }
            CacheManager.RemoveCache(CacheKeys.Items.WeixinAccessToken_, dealerSysNo.ToString());
            CacheManager.RemoveCache(CacheKeys.Items.WeixinJsTicket_, dealerSysNo.ToString());
        }
        /// <summary>
        /// 删除分销商相关缓存
        /// </summary>
        /// <param name="dealerSysNo">分销商编号</param>
        /// <remarks>2016-1-14 杨浩 创建</remarks>
        public static void Store(int dealerSysNo)
        {
            DeleteCache.Delete(CacheKeys.Items.StoreAll);
            DeleteCache.Delete(CacheKeys.Items.StoresInfo_,dealerSysNo.ToString());
        }
        /// <summary>
        /// 删除分销商产品相关
        /// </summary>
        /// <param name="dealerSysNo">分销商编号</param>
        /// <remarks>2016-1-14 杨浩 创建</remarks>
        public static void StoresProductList(int dealerSysNo)
        {
            //Hyt.BLL.Sys.MemoryBo.Instance.DeleteByPrefix(CacheKeys.Items.StoresProduct_.ToString() + dealerSysNo.ToString()+"_");
            Hyt.BLL.Sys.MemoryBo.Instance.DeleteByPrefix(CacheKeys.Items.StoresProductList_.ToString() + dealerSysNo.ToString());
        }
        /// <summary>
        /// 删除分销商产品价格
        /// </summary>
        /// <param name="dealerSysNo">分销商编号</param>
        /// <param name="productSysNo">产品编号</param>
        /// <remarks>2016-1-14 杨浩 创建</remarks>
        public static void StoresProduct(int dealerSysNo,int productSysNo)
        {
            DeleteCache.Delete(CacheKeys.Items.StoresProduct_, dealerSysNo + "_" + productSysNo);
        }
        /// <summary>
        /// 删除搜索关键字缓存
        /// </summary>
        /// <remarks>2013-08-07 黄波 创建</remarks>
        public static void SearchKeys()
        {
            DeleteCache.Delete(CacheKeys.Items.SearchKeys);
        }

        /// <summary>
        /// 根据广告组代码删除网站广告缓存
        /// </summary>
        /// <param name="groupCode">广告组代码</param>
        /// <remarks>2013-08-07 黄波 创建</remarks>
        public static void WebAdvertItem(string groupCode)
        {
            DeleteCache.Delete(CacheKeys.Items.WebAdvertItem_, groupCode);
        }

        /// <summary>
        /// 删除商品消息信息
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <remarks>2013-08-07 邵斌 创建</remarks>
        public static void ProductInfo(int productSysNo)
        {
            string relationCode = Hyt.BLL.Product.PdProductAssociationBo.Instance.GetRelationCode(productSysNo);
            if (!string.IsNullOrWhiteSpace(relationCode))
            {
                DeleteCache.Delete(CacheKeys.Items.ProductAssociation_, relationCode);
                DeleteCache.Delete(CacheKeys.Items.ProductAssociationAllAssociationAttribute_, relationCode);
            }
            DeleteCache.Delete(CacheKeys.Items.ProductDetailInfo_, productSysNo.ToString());
            DeleteCache.Delete(CacheKeys.Items.ProductMasterCategoryRoute_, productSysNo.ToString());
            DeleteCache.Delete(CacheKeys.Items.ProductDetialOffsaleRcdList_, productSysNo.ToString());
            DeleteCache.Delete(CacheKeys.Items.ProductAssociationAttribute_, productSysNo.ToString());
            DeleteCache.Delete(CacheKeys.Items.ProductAttribute_, productSysNo.ToString());
            DeleteCache.Delete(CacheKeys.Items.OtherCustmerBought_, productSysNo.ToString());
            DeleteCache.Delete(CacheKeys.Items.FollowWithByProductSysNo_, productSysNo.ToString());
        }

        /// <summary>
        /// 清理商品分类缓存
        /// </summary>
        /// <param name="categorySysNo"></param>
        /// <remarks>2013-08-07 邵斌 创建</remarks>
        public static void ProductCategory(int categorySysNo)
        {
            DeleteCache.Delete(CacheKeys.Items.CategoryInfoWithParent_, categorySysNo.ToString());
            DeleteCache.Delete(CacheKeys.Items.SingleCategoryInfo_, categorySysNo.ToString());
            DeleteCache.Delete(CacheKeys.Items.ProductMasterCategoryRoute_, categorySysNo.ToString());
            DeleteCache.Delete(CacheKeys.Items.BackendAllPdCategoryZtreeNodeData);
        } 
        
        /// <summary>
        /// 删除商品评论相关缓存
        /// </summary>
        /// <param name="productSysno">商品系统编号</param>
        /// <remarks>2013-12-24 邵斌 创建</remarks>
        public static void ProductCommentInfo(int productSysno)
        {
            DeleteCache.Delete(CacheKeys.Items.ProductCommentTotalInfo_,productSysno.ToString()); 
        }

        /// <summary>
        /// 清理新闻分类缓存
        /// </summary>
        /// <param name="categorySysNo"></param>
        /// <remarks>2013-08-07 邵斌 创建</remarks>
        public static void NewsCategory(int categorySysNo)
        {
            DeleteCache.Delete(CacheKeys.Items.NewsCategoryInfoWithParent_, categorySysNo.ToString());
            DeleteCache.Delete(CacheKeys.Items.SingleNewsCategoryInfo_, categorySysNo.ToString());
            DeleteCache.Delete(CacheKeys.Items.BackendAllNewsCategoryZtreeNodeData);
        } 
        #region 删除方法
        /// <summary>
        /// 根据缓存KEY删除缓存
        /// </summary>
        /// <param name="key">缓存KEY</param>
        /// <remarks>2013-08-07 黄波 创建</remarks>
        private static void Delete(CacheKeys.Items key)
        {
            if (CacheManager.Instance.IsExists(key.ToString()))
            {
                CacheManager.Instance.Delete(key.ToString());
            }
        }
        /// <summary>
        /// 根据缓存KEY删除缓存
        /// </summary>
        /// <param name="key">缓存KEY</param>
        /// <param name="suffix">关键字后接字符串(可为空)</param>
        /// <remarks>2013-08-07 黄波 创建</remarks>
        private static void Delete(CacheKeys.Items key, string suffix)
        {
            var cacheKey = key.ToString() + suffix;
            if (CacheManager.Instance.IsExists(cacheKey))
            {
                CacheManager.Instance.Delete(cacheKey);
            }
        }
        #endregion
    }
}
