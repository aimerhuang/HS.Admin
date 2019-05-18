using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.BLL.Order;
using Hyt.BLL.Product;
using Hyt.BLL.Promotion;
using Hyt.DataAccess.Promotion;
using Hyt.Infrastructure.Memory;
using Hyt.Model;
using Hyt.DataAccess.CRM;
using Hyt.Model.SystemPredefined;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.CRM
{
    /// <summary>
    /// 购物车业务（Cache存储购物车对象，使用前后需调用清除方法）
    /// </summary>
    /// <remarks>2013-09-24 吴文强 创建</remarks>
    public class CrShoppingCartToCacheBo : BOBase<CrShoppingCartToCacheBo>
    {
        #region Session 存储
        /// <summary>
        /// Session Key
        /// </summary>
        private const string CACHE_KEY_SHOPPINGCART = "Cache_Key_ShoppingCart_{0}";

        private string _cacheKey = string.Empty;
        private string CacheKey { get { return string.Format(CACHE_KEY_SHOPPINGCART, _cacheKey); } }

        /// <summary>
        /// 获取或设置购物车明显
        /// </summary>
        private IList<CBCrShoppingCartItem> CacheItems
        {
            set
            {
                MemoryProvider.Default.Remove(CacheKey);
                MemoryProvider.Default.Set(CacheKey, value, 1440);
            }
            get
            {
                var shoppingCartItems = (List<CBCrShoppingCartItem>)MemoryProvider.Default.Get(CacheKey) ?? new List<CBCrShoppingCartItem>();
                foreach (var item in shoppingCartItems)
                {
                    item.GetThumbnail = () => Web.ProductImageBo.Instance.GetProductImagePath(Web.ProductThumbnailType.Image180, item.ProductSysNo);
                }
                return shoppingCartItems;
            }
        }

        /// <summary>
        /// 自增系统编号
        /// </summary>
        private int SeqSysNo
        {
            get
            {
                int sysNo = (int?)MemoryProvider.Default.Get(CacheKey + "_SeqSysNo") ?? 0;
                sysNo++;
                MemoryProvider.Default.Remove(CacheKey + "_SeqSysNo");
                MemoryProvider.Default.Set(CacheKey + "_SeqSysNo", sysNo, 1440);
                return sysNo;
            }
        }

        /// <summary>
        /// 设置购物车明细
        /// </summary>
        /// <param name="cacheKey">缓存Key.</param>
        /// <param name="cartItemList">购物车明细列表.</param>
        public void SetCacheItems(string cacheKey, IList<CBCrShoppingCartItem> cartItemList)
        {
            _cacheKey = cacheKey;

            foreach (var cartItem in cartItemList)
            {
                if (cartItem.SysNo == 0) cartItem.SysNo = SeqSysNo;
            }

            CacheItems = cartItemList;
        }

        /// <summary>
        /// 清除购物车明细
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        public void Clear(string cacheKey)
        {
            _cacheKey = cacheKey;

            MemoryProvider.Default.Remove(CacheKey);
            MemoryProvider.Default.Remove(CacheKey + "_SeqSysNo");
        }

        #endregion

        #region 添加/移除/更新购物车商品

        /// <summary>
        /// 添加JsonCartItem至缓存购物车
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="jsonCartItems">jsonCartItems</param>
        /// <remarks>2013-10-16 吴文强 创建</remarks>
        public void JsonCartItemToCache(string cacheKey, int customerSysNo, List<JsonCartItem> jsonCartItems)
        {
            _cacheKey = cacheKey;
            Clear(cacheKey);
            var promotionKeys = new List<string>();

            foreach (var item in jsonCartItems)
            {
                if (item.ProductSalesType == (int)CustomerStatus.商品销售类型.赠品)
                {
                    AddGift(cacheKey, customerSysNo, item.ProductSysNo, int.Parse(item.Promotions), CustomerStatus.购物车商品来源.PC网站);
                }
                else
                {
                    if (item.IsLock == (int)CustomerStatus.购物车是否锁定.是 && !string.IsNullOrEmpty(item.GroupCode))
                    {
                        var keys = string.Format("{0},{1}", item.Promotions, item.GroupCode);
                        if (promotionKeys.Contains(keys))
                        {
                            continue;
                        }
                        promotionKeys.Add(keys);

                        Add(cacheKey, customerSysNo, int.Parse(item.GroupCode), item.Quantity, item.ProductSysNo, CustomerStatus.购物车商品来源.PC网站, (CustomerStatus.是否选中)item.IsChecked);
                    }
                    else
                    {
                        Add(cacheKey, customerSysNo, item.ProductSysNo, item.Quantity, CustomerStatus.购物车商品来源.PC网站, (CustomerStatus.是否选中)item.IsChecked);
                    }
                }
            }
        }

        /// <summary>
        /// 添加商品至购物车
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="productSysNo">商品系统编号</param>
        /// <param name="quantity">商品数量</param>
        /// <param name="source">购物车商品来源</param>
        /// <param name="isChecked">是否选中商品</param>
        /// <remarks>2013-08-13 吴文强 创建</remarks>
        public void Add(string cacheKey, int customerSysNo, int productSysNo, int quantity, CustomerStatus.购物车商品来源 source, CustomerStatus.是否选中 isChecked = CustomerStatus.是否选中.是)
        {
            _cacheKey = cacheKey;

            var productPrices = PdPriceBo.Instance.GetProductPrice(productSysNo,
                                                             new ProductStatus.产品价格来源[] { ProductStatus.产品价格来源.会员等级价 });
            var customer = CrCustomerBo.Instance.GetModel(customerSysNo);
            var firstOrDefault = productPrices.FirstOrDefault(p => customer != null ? p.SourceSysNo == customer.LevelSysNo : p.SourceSysNo == CustomerLevel.初级);
            if (firstOrDefault != null)
            {
                var shoppingCartItem = new CBCrShoppingCartItem
                    {
                        SysNo = SeqSysNo,
                        IsChecked = (int)isChecked,
                        CustomerSysNo = customerSysNo,
                        ProductSysNo = productSysNo,
                        ProductName = "",
                        Quantity = quantity,
                        OriginPrice = firstOrDefault.Price,
                        IsLock = (int)CustomerStatus.购物车是否锁定.否,
                        IsExpireReset = (int)CustomerStatus.购物车是否过期重置.是,
                        CreateDate = DateTime.Now,
                        Source = (int)source,
                        ProductSalesType = (int)CustomerStatus.商品销售类型.普通,
                    };

                #region 计算并写入Cache

                var items = CacheItems;
                var item = items.FirstOrDefault(sci => sci.ProductSysNo == shoppingCartItem.ProductSysNo);
                if (item == null)
                {
                    items.Add(shoppingCartItem);
                }
                else
                {
                    item.Quantity = item.Quantity + quantity;
                }
                CacheItems = items;

                #endregion
            }
        }

        /// <summary>
        /// 将组促销转换为购物车明细
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="groupSysNo">组系统编号(组合,团购主表系统编号)</param>
        /// <param name="quantity">组数量</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <param name="source">购物车商品来源</param>
        /// <param name="isChecked">是否选中商品</param>
        /// <returns>购物车明细集合</returns>
        /// <remarks>2013-09-01 吴文强 创建</remarks>
        public void Add(string cacheKey, int customerSysNo, int groupSysNo, int quantity, int promotionSysNo, CustomerStatus.购物车商品来源 source, CustomerStatus.是否选中 isChecked = CustomerStatus.是否选中.是)
        {
            _cacheKey = cacheKey;

            #region 计算并写入Cache

            var items = CacheItems;
            var groupItems = InitGroupToCartItem(customerSysNo, groupSysNo, quantity, promotionSysNo, source);

            //检查是否已存在未锁定商品，存在+1，不存在新增

            foreach (var shoppingCartItem in groupItems)
            {
                var item = items.FirstOrDefault(sci => sci.ProductSysNo == shoppingCartItem.ProductSysNo
                    && sci.CustomerSysNo == customerSysNo && sci.IsLock == shoppingCartItem.IsLock
                    && sci.GroupCode == shoppingCartItem.GroupCode && sci.Promotions == shoppingCartItem.Promotions);

                if (item == null)
                {
                    shoppingCartItem.SysNo = SeqSysNo;

                    items.Add(shoppingCartItem);
                }
                else
                {
                    item.Quantity = item.Quantity + quantity;
                }
            }

            CacheItems = items;

            #endregion
        }

        /// <summary>
        /// 添加促销赠品至购物车
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="productSysNo">商品(赠品)系统编号</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <param name="source">购物车商品来源</param>
        /// <remarks>2013-08-13 吴文强 创建</remarks>
        public void AddGift(string cacheKey, int customerSysNo, int productSysNo, int promotionSysNo, CustomerStatus.购物车商品来源 source)
        {
            _cacheKey = cacheKey;

            var shoppingCartItem = new CBCrShoppingCartItem
            {
                SysNo = SeqSysNo,
                IsChecked = (int)CustomerStatus.是否选中.是,
                CustomerSysNo = customerSysNo,
                ProductSysNo = productSysNo,
                ProductName = "",
                Quantity = 1,
                OriginPrice = 0,
                IsLock = (int)CustomerStatus.购物车是否锁定.否,
                IsExpireReset = (int)CustomerStatus.购物车是否过期重置.是,
                CreateDate = DateTime.Now,
                Source = (int)source,
                ProductSalesType = (int)CustomerStatus.商品销售类型.赠品,
                UsedPromotions = promotionSysNo.ToString(),
            };

            #region 计算并写入Cache

            var items = CacheItems;
            var item = items.FirstOrDefault(sci => sci.ProductSysNo == shoppingCartItem.ProductSysNo
                                                   && sci.ProductSalesType == (int)CustomerStatus.商品销售类型.赠品 &&
                                                   sci.UsedPromotions == promotionSysNo.ToString());
            if (item == null)
            {
                items.Add(shoppingCartItem);
            }
            else
            {
                item.Quantity = 1;
            }
            CacheItems = items;

            #endregion
        }

        /// <summary>
        /// 删除购物车明细
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="sysNo">购物车明细系统编号</param>
        /// <remarks>2013-08-13 吴文强 创建</remarks>
        public void Remove(string cacheKey, int customerSysNo, int[] sysNo)
        {
            _cacheKey = cacheKey;

            #region 计算并写入Cache

            var items = CacheItems;
            foreach (var i in sysNo)
            {
                var item = items.FirstOrDefault(sci => sci.SysNo == i);
                if (item != null)
                {
                    items.Remove(item);
                }
            }
            CacheItems = items;

            #endregion
        }

        /// <summary>
        /// 删除购物车选中的明细
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <remarks>2013-09-24 吴文强 创建</remarks>
        public void RemoveCheckedItem(string cacheKey, int customerSysNo)
        {
            _cacheKey = cacheKey;

            #region 计算并写入Cache

            var items = CacheItems;
            IList<CBCrShoppingCartItem> checkedItems = items.Where(i => i.IsChecked == 1).ToList();
            int[] sysnos = checkedItems.Select(c => c.ProductSysNo).ToArray();

            for (int i = 0; i < sysnos.Length; i++)
            {
                for (int j = 0; j < items.Count; j++)
                {
                    if (items[j].ProductSysNo == sysnos[i])
                    {
                        items.RemoveAt(j);
                        break;
                    }
                }
            }
            CacheItems = items;

            #endregion
        }

        /// <summary>
        /// 删除购物车所有明细
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <remarks>2013-09-24 吴文强 创建</remarks>
        public void RemoveAll(string cacheKey, int customerSysNo)
        {
            _cacheKey = cacheKey;

            CacheItems = new List<CBCrShoppingCartItem>();
        }

        /// <summary>
        /// 删除购物车组商品
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="groupCode">组代码</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <remarks>2013-08-13 吴文强 创建</remarks>
        public void Remove(string cacheKey, int customerSysNo, string groupCode, string promotionSysNo)
        {
            _cacheKey = cacheKey;

            #region 计算并写入Cache

            var items = CacheItems;

            var item = items.FirstOrDefault(i => i.CustomerSysNo == customerSysNo && i.GroupCode == groupCode && i.Promotions == promotionSysNo);
            if (item != null)
            {
                items.Remove(item);
            }

            CacheItems = items;

            #endregion
        }

        /// <summary>
        /// 删除促销赠品
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="productSysNo">商品(赠品)系统编号</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <remarks>2013-08-13 吴文强 创建</remarks>
        public void RemoveGift(string cacheKey, int customerSysNo, int productSysNo, int promotionSysNo)
        {
            _cacheKey = cacheKey;

            #region 计算并写入Cache

            var items = CacheItems;

            var item = items.FirstOrDefault(p => p.CustomerSysNo == customerSysNo && p.ProductSysNo == productSysNo && p.UsedPromotions == promotionSysNo.ToString());
            if (item != null)
            {
                items.Remove(item);
            }

            CacheItems = items;

            #endregion
        }

        /// <summary>
        /// 更新购物车明细商品数量
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="sysNo">购物车明细系统编号集合</param>
        /// <param name="quantity">商品数量</param>
        public void UpdateQuantity(string cacheKey, int customerSysNo, int[] sysNo, int quantity)
        {
            _cacheKey = cacheKey;

            #region 计算并写入Cache

            var items = CacheItems;
            foreach (var i in sysNo)
            {
                var item = items.FirstOrDefault(t => t.SysNo == i);
                if (item != null)
                {
                    item.Quantity = quantity;
                }
            }
            CacheItems = items;
            #endregion
        }

        /// <summary>
        /// 更新购物车组组商品数量
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="groupCode">组代码</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <param name="quantity">商品数量</param>
        public void UpdateQuantity(string cacheKey, int customerSysNo, string groupCode, string promotionSysNo, int quantity)
        {
            _cacheKey = cacheKey;

            #region 计算并写入Cache

            var items = CacheItems;
            var checkedItems =
                items.Where(
                    i => i.CustomerSysNo == customerSysNo && i.GroupCode == groupCode && i.Promotions == promotionSysNo);
            foreach (var item in checkedItems)
            {
                item.Quantity = quantity;
            }
            CacheItems = items;
            #endregion
        }
        #endregion

        #region 选中/取消选中购物车明细项目

        /// <summary>
        /// 选择购物车所有明细项目
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <remarks>2013-09-23 吴文强 创建</remarks>
        public void CheckedAll(string cacheKey, int customerSysNo)
        {
            _cacheKey = cacheKey;

            #region 计算并写入Cache

            var items = CacheItems;
            var checkedItems =
                items.Where(
                    i => i.CustomerSysNo == customerSysNo);
            foreach (var item in checkedItems)
            {
                item.IsChecked = (int)CustomerStatus.是否选中.是;
            }
            CacheItems = items;
            #endregion
        }

        /// <summary>
        /// 取消选择购物车所有明细项目
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <remarks>2013-09-23 吴文强 创建</remarks>
        public void UncheckedAll(string cacheKey, int customerSysNo)
        {
            _cacheKey = cacheKey;

            #region 计算并写入Cache

            var items = CacheItems;
            var checkedItems =
                items.Where(
                    i => i.CustomerSysNo == customerSysNo);
            foreach (var item in checkedItems)
            {
                item.IsChecked = (int)CustomerStatus.是否选中.否;
            }
            CacheItems = items;
            #endregion
        }

        /// <summary>
        /// 选择购物车明细项目
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="itemSysNo">购物车明细系统编号</param>
        /// <remarks>2013-08-27 吴文强 创建</remarks>
        public void CheckedItem(string cacheKey, int customerSysNo, int[] itemSysNo)
        {
            _cacheKey = cacheKey;

            #region 计算并写入Cache

            var items = CacheItems;
            foreach (var i in itemSysNo)
            {
                var item = items.FirstOrDefault(t => t.CustomerSysNo == customerSysNo && t.SysNo == i);
                if (item != null)
                {
                    item.IsChecked = (int)CustomerStatus.是否选中.是;
                }
            }
            CacheItems = items;
            #endregion
        }

        /// <summary>
        /// 取消选择购物车明细项目
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="itemSysNo">购物车明细系统编号</param>
        /// <remarks>2013-08-27 吴文强 创建</remarks>
        public void UncheckedItem(string cacheKey, int customerSysNo, int[] itemSysNo)
        {
            _cacheKey = cacheKey;

            #region 计算并写入Cache

            var items = CacheItems;
            foreach (var i in itemSysNo)
            {
                var item = items.FirstOrDefault(t => t.CustomerSysNo == customerSysNo && t.SysNo == i);
                if (item != null)
                {
                    item.IsChecked = (int)CustomerStatus.是否选中.否;
                }
            }
            CacheItems = items;
            #endregion
        }

        /// <summary>
        /// 选择购物车组明细项目
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="groupCode">组代码</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <remarks>2013-08-27 吴文强 创建</remarks>
        public void CheckedItem(string cacheKey, int customerSysNo, string groupCode, string promotionSysNo)
        {
            _cacheKey = cacheKey;

            #region 计算并写入Cache

            var items = CacheItems;
            var checkedItems =
                items.Where(
                    i => i.CustomerSysNo == customerSysNo && i.GroupCode == groupCode && i.Promotions == promotionSysNo);
            foreach (var item in checkedItems)
            {
                item.IsChecked = (int)CustomerStatus.是否选中.是;
            }
            CacheItems = items;
            #endregion
        }

        /// <summary>
        /// 取消选择购物车组明细项目
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="groupCode">组代码</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <remarks>2013-08-27 吴文强 创建</remarks>
        public void UncheckedItem(string cacheKey, int customerSysNo, string groupCode, string promotionSysNo)
        {
            _cacheKey = cacheKey;
            #region 计算并写入Cache

            var items = CacheItems;
            var checkedItems =
                items.Where(
                    i => i.CustomerSysNo == customerSysNo && i.GroupCode == groupCode && i.Promotions == promotionSysNo);
            foreach (var item in checkedItems)
            {
                item.IsChecked = (int)CustomerStatus.是否选中.否;
            }
            CacheItems = items;
            #endregion
        }

        #endregion

        #region 获取购物车对象

        /// <summary>
        /// 获取购物车对象
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="platformType">使用平台</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="isChecked">true:只查询选中的明细;false:查询全部,默认false</param>
        /// <param name="isFrontProduct">是否只允许前台下单的商品（默认：true）</param>
        /// <returns>购物车对象</returns>
        /// <remarks>2013-08-12 吴文强 创建</remarks>
        public CrShoppingCart GetShoppingCart(string cacheKey, PromotionStatus.促销使用平台[] platformType, int customerSysNo, bool isChecked = false, bool isFrontProduct = true)
        {
            _cacheKey = cacheKey;

            return GetShoppingCart(platformType, customerSysNo, (List<CBCrShoppingCartItem>)CacheItems, null, null, null, null, isFrontProduct);
        }

        /// <summary>
        /// 获取购物车对象
        /// </summary>
        /// <param name="platformType">使用平台</param>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="sysNo">购物车明细系统编号(null:购物车为服务器选中的对象)</param>
        /// <param name="areaSysNo">区域系统编号</param>
        /// <param name="deliveryTypeSysNo">配送方式系统编号</param>
        /// <param name="promotionCode">促销代码</param>
        /// <param name="couponCode">优惠券代码</param>
        /// <param name="isChecked">true:只查询选中的明细;false:查询全部,默认false</param>
        /// <param name="isFrontProduct">是否只允许前台下单的商品（默认：true）</param>
        /// <returns>购物车对象</returns>
        /// <remarks>2013-08-12 吴文强 创建</remarks>
        public CrShoppingCart GetShoppingCart(string cacheKey, PromotionStatus.促销使用平台[] platformType, int customerSysNo, int[] sysNo, int? areaSysNo, int? deliveryTypeSysNo, string promotionCode, string couponCode, bool isChecked = false, bool isFrontProduct = true)
        {
            _cacheKey = cacheKey;

            var shoppingCartItems = (List<CBCrShoppingCartItem>)CacheItems;
            shoppingCartItems = shoppingCartItems.Where(sci => sci.CustomerSysNo == customerSysNo
                                                               &&
                                                               (sysNo == null || sysNo.Count() == 0 ||
                                                                sysNo.Contains(sci.SysNo))
                                                               &&
                                                               (!isChecked ||
                                                                sci.IsChecked == (int)CustomerStatus.是否选中.是)).ToList();

            var shoppingCartGiftItems =
                shoppingCartItems.Where(sci => sci.ProductSalesType == (int)CustomerStatus.商品销售类型.赠品);
            foreach (var shoppingCartGiftItem in shoppingCartGiftItems)
            {
                if (shoppingCartItems.FirstOrDefault(sci => sci.SysNo == shoppingCartGiftItem.SysNo) == null)
                {
                    shoppingCartItems.Add(shoppingCartGiftItem);
                }
            }
            return GetShoppingCart(platformType, customerSysNo, shoppingCartItems, areaSysNo, deliveryTypeSysNo, promotionCode, couponCode, isFrontProduct);
        }

        /// <summary>
        /// 获取购物车对象(可适合无购物车对象计算,如:前端未登录用户,公共账户,退换货)
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="platformType">使用平台</param>
        /// <param name="customer">客户对象</param>
        /// <param name="shoppingCartItems">购物车明细集合</param>
        /// <param name="promotionSysNo">促销系统编号集合</param>
        /// <param name="couponCode">优惠券代码</param>
        /// <param name="isAutoPromotion">是否自动使用当前有效促销</param>
        /// <param name="areaSysNo">区域系统编号</param>
        /// <param name="deliveryTypeSysNo">配送方式系统编号</param>
        /// <param name="promotionCode">促销代码</param>
        /// <returns>购物车对象</returns>
        /// <remarks>2013-08-12 吴文强 创建</remarks>
        public CrShoppingCart GetShoppingCart(string cacheKey, PromotionStatus.促销使用平台[] platformType, CrCustomer customer,
                                              IList<CBCrShoppingCartItem> shoppingCartItems,
                                              int[] promotionSysNo, string couponCode = null,
                                              bool isAutoPromotion = false, int? areaSysNo = null,
                                              int? deliveryTypeSysNo = null, string promotionCode = null)
        {
            _cacheKey = cacheKey;

            return SpPromotionEngineBo.Instance.CalculateShoppingCart(platformType, customer,
                                                                      shoppingCartItems,
                                                                      promotionSysNo, couponCode,
                                                                      isAutoPromotion, areaSysNo, deliveryTypeSysNo,
                                                                      promotionCode);
        }

        #endregion

        /// <summary>
        /// 购物车转销售单明细
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="orderSysNo">订单号</param>
        /// <param name="transactionSysNo">事物编号</param>
        /// <param name="shoppingCart">购物车</param>
        /// <returns>销售单明细集合</returns>
        /// <remarks>2013-09-10 吴文强 创建</remarks>
        public List<SoOrderItem> ShoppingCartToOrderItem(string cacheKey, int orderSysNo, string transactionSysNo, CrShoppingCart shoppingCart)
        {
            var orderItems = new List<SoOrderItem>();
            var removeCartSysNo = new List<int>();

            //获取购物车中所有赠品
            var shoppingCartGiftItems =
                shoppingCart.GetShoppingCartItem().Where(sci => sci.ProductSalesType == (int)CustomerStatus.商品销售类型.赠品);

            var order = SoOrderBo.Instance.GetEntity(orderSysNo);
            var customer = CrCustomerBo.Instance.GetModel(order.CustomerSysNo);

            foreach (var cartGroup in shoppingCart.ShoppingCartGroups)
            {
                #region 添加商品
                //添加商品
                foreach (var cartItem in cartGroup.ShoppingCartItems)
                {
                    var groupName = string.Empty;
                    var usedPromotions = string.Empty;
                    if (cartItem.IsLock == (int)CustomerStatus.购物车是否锁定.是)
                    {
                        groupName = cartGroup.GroupPromotions != null && cartGroup.GroupPromotions.Count > 0
                                        ? cartGroup.GroupPromotions[0].Description
                                        : string.Empty;
                        usedPromotions = cartGroup.Promotions;
                    }
                    else
                    {
                        if (cartItem.Promotions != null)
                        {
                            var proms =
                                cartItem.Promotions.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries)
                                        .Select(t => Convert.ToInt32(t));
                            if (cartGroup.GroupPromotions != null)
                                usedPromotions = string.Join(";",
                                                             cartGroup.GroupPromotions.Where(
                                                                 t => proms.Contains(t.PromotionSysNo) && t.IsUsed)
                                                                      .Select(s => s.PromotionSysNo.ToString()));
                        }
                    }

                    orderItems.Add(new SoOrderItem()
                    {
                        OrderSysNo = orderSysNo,
                        TransactionSysNo = transactionSysNo,

                        ProductSysNo = cartItem.ProductSysNo,
                        ProductName = cartItem.ProductName,
                        Quantity = cartItem.Quantity,
                        OriginalPrice = cartItem.OriginPrice,
                        SalesUnitPrice = cartItem.SalesUnitPrice,
                        SalesAmount = cartItem.SaleTotalAmount,
                        DiscountAmount = cartItem.DiscountAmount,
                        ChangeAmount = 0,
                        RealStockOutQuantity = 0,
                        ProductSalesType = cartItem.ProductSalesType,
                        ProductSalesTypeSysNo = cartItem.ProductSalesTypeSysNo,
                        GroupCode = cartItem.GroupCode,
                        GroupName = groupName,
                        UsedPromotions = usedPromotions//UsedPromotions = cartItem.UsedPromotions
                    });
                    //需要从购物车中移除的系统编号
                    removeCartSysNo.Add(cartItem.SysNo);
                }
                #endregion

                #region 添加商品组赠品
                //添加赠品
                if (cartGroup.GroupPromotions != null)
                {
                    foreach (var groupPromotion in cartGroup.GroupPromotions)
                    {
                        if (groupPromotion.UsedGiftProducts == null) continue;
                        foreach (var giftProduct in groupPromotion.UsedGiftProducts)
                        {
                            var product = PdProductBo.Instance.GetProduct(giftProduct.ProductSysNo);
                            if (product == null) continue;

                            //从购物车获取数据
                            orderItems.Add(new SoOrderItem()
                            {
                                OrderSysNo = orderSysNo,
                                TransactionSysNo = transactionSysNo,

                                ProductSysNo = giftProduct.ProductSysNo,
                                ProductName = giftProduct.ProductName,
                                Quantity = 1,
                                OriginalPrice = PdPriceBo.Instance.GetUserRankPrice(giftProduct.ProductSysNo, customer.LevelSysNo),
                                SalesUnitPrice = giftProduct.PurchasePrice,
                                SalesAmount = giftProduct.PurchasePrice,
                                DiscountAmount = 0,
                                ChangeAmount = 0,
                                RealStockOutQuantity = 0,
                                ProductSalesType = (int)CustomerStatus.商品销售类型.赠品,

                                ProductSalesTypeSysNo = giftProduct.SysNo,
                                GroupCode = string.Empty,
                                GroupName = string.Empty,
                                UsedPromotions = giftProduct.PromotionSysNo.ToString()
                            });

                            //需要从购物车中移除的系统编号
                            removeCartSysNo.Add(giftProduct.ProductSysNo);
                        }
                    }
                }
                #endregion
            }

            #region 添加订单赠品
            //添加赠品
            if (shoppingCart.GroupPromotions != null)
            {
                foreach (var groupPromotion in shoppingCart.GroupPromotions)
                {
                    if (groupPromotion.UsedGiftProducts == null) continue;
                    foreach (var giftProduct in groupPromotion.UsedGiftProducts)
                    {
                        var product = PdProductBo.Instance.GetProduct(giftProduct.ProductSysNo);
                        if (product == null) continue;

                        //从购物车获取数据
                        orderItems.Add(new SoOrderItem()
                        {
                            OrderSysNo = orderSysNo,
                            TransactionSysNo = transactionSysNo,

                            ProductSysNo = giftProduct.ProductSysNo,
                            ProductName = giftProduct.ProductName,
                            Quantity = 1,
                            OriginalPrice = PdPriceBo.Instance.GetUserRankPrice(giftProduct.ProductSysNo, customer.LevelSysNo),
                            SalesUnitPrice = giftProduct.PurchasePrice,
                            SalesAmount = giftProduct.PurchasePrice,
                            DiscountAmount = 0,
                            ChangeAmount = 0,
                            RealStockOutQuantity = 0,
                            ProductSalesType = (int)CustomerStatus.商品销售类型.赠品,

                            ProductSalesTypeSysNo = giftProduct.SysNo,
                            GroupCode = string.Empty,
                            GroupName = string.Empty,
                            UsedPromotions = giftProduct.PromotionSysNo.ToString()
                        });

                        //需要从购物车中移除的系统编号
                        removeCartSysNo.Add(giftProduct.ProductSysNo);
                    }
                }
            }
            #endregion

            return orderItems;
        }

        #region 私有处理方法

        /// <summary>
        /// 计算购物车
        /// </summary>
        /// <param name="platformType">使用平台</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="shoppingCartItems">购物车明细集合</param>
        /// <param name="areaSysNo">区域系统编号</param>
        /// <param name="deliveryTypeSysNo">配送方式系统编号</param>
        /// <param name="promotionCode">促销代码</param>
        /// <param name="couponCode">优惠券代码</param>
        /// <param name="isFrontProduct">是否只允许前台下单的商品（默认：true）</param>
        /// <param name="dealerSysNo">经销商系统编号</param>
        /// <returns>购物车</returns>
        /// <remarks>
        /// 2013-08-31 吴文强 创建
        /// 2016-7-12 杨浩 添加经销商系统编号参数
        /// </remarks>
        private CrShoppingCart GetShoppingCart(PromotionStatus.促销使用平台[] platformType, int customerSysNo, IList<CBCrShoppingCartItem> shoppingCartItems, int? areaSysNo, int? deliveryTypeSysNo, string promotionCode, string couponCode, bool isFrontProduct = true, int? warehouseSysNo = null,int dealerSysNo = 0)
        {
            var customer = CrCustomerBo.Instance.GetCrCustomerItem(customerSysNo);
            var levelSysNo = customer != null ? customer.LevelSysNo : CustomerLevel.初级;

            //移除下架的商品
            shoppingCartItems = CrShoppingCartConvertBo.Instance.RemoveSoldoutProduct(shoppingCartItems, customerSysNo, isFrontProduct);

            //重置购物车明细商品
            shoppingCartItems = ResetItemByPromotion(platformType, shoppingCartItems);

            //购物车明细
            shoppingCartItems = CrShoppingCartBo.Instance.InitProductPrice(levelSysNo, shoppingCartItems, 0, dealerSysNo);

            //计算后的购物车对象
            var shoppingCart = SpPromotionEngineBo.Instance.CalculateShoppingCart(platformType, customer, shoppingCartItems, areaSysNo, deliveryTypeSysNo, promotionCode, couponCode, warehouseSysNo);

            //移除前台不能下单的赠品
            shoppingCart = CrShoppingCartConvertBo.Instance.RemoveSoldoutGift(shoppingCart, isFrontProduct);

            //获取当前使用有效赠品
            var containGifts = new List<int>();
            if (shoppingCart.GroupPromotions != null)
            {
                foreach (var groupPromotion in shoppingCart.GroupPromotions.Where(groupPromotion => groupPromotion.UsedGiftProducts != null))
                {
                    foreach (var usedGiftProduct in groupPromotion.UsedGiftProducts)
                    {
                        containGifts.Add(
                            shoppingCartItems.FirstOrDefault(
                                sci => sci.ProductSysNo.Equals(usedGiftProduct.ProductSysNo)
                                       && sci.UsedPromotions.Equals(usedGiftProduct.PromotionSysNo.ToString())).SysNo);
                    }
                }
            }

            foreach (var shoppingCartGroup in shoppingCart.ShoppingCartGroups)
            {
                if (shoppingCartGroup.GroupPromotions == null) continue;

                foreach (var groupPromotion in shoppingCartGroup.GroupPromotions.Where(groupPromotion => groupPromotion.UsedGiftProducts != null))
                {
                    foreach (var usedGiftProduct in groupPromotion.UsedGiftProducts)
                    {
                        containGifts.Add(
                            shoppingCartItems.FirstOrDefault(
                                sci => sci.ProductSysNo.Equals(usedGiftProduct.ProductSysNo)
                                       && sci.UsedPromotions.Equals(usedGiftProduct.PromotionSysNo.ToString())).SysNo);
                    }
                }
            }

            return shoppingCart;
        }

        /// <summary>
        /// 重新设置无效促销商品状态
        /// </summary>
        /// <param name="platformType">使用平台</param>
        /// <param name="shoppingCartItems">购物车明细集合</param>
        /// <returns>购物车明细集合</returns>
        /// <remarks>2013-09-01 吴文强 创建</remarks>
        private IList<CBCrShoppingCartItem> ResetItemByPromotion(PromotionStatus.促销使用平台[] platformType, IList<CBCrShoppingCartItem> shoppingCartItems)
        {
            var promotions = SpPromotionBo.Instance.GetValidPromotions(platformType);
            foreach (var shoppingCartItem in shoppingCartItems)
            {
                if (!string.IsNullOrEmpty(shoppingCartItem.Promotions) &&
                    promotions.FirstOrDefault(p => shoppingCartItem.Promotions.Equals(p.SysNo.ToString())) == null)
                {
                    shoppingCartItem.IsLock = (int)CustomerStatus.购物车是否锁定.否;
                    shoppingCartItem.Promotions = "";
                    shoppingCartItem.UsedPromotions = "";
                    shoppingCartItem.GroupCode = "";
                }
            }
            return shoppingCartItems;
        }

        /// <summary>
        /// 将组促销转换为购物车明细
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="groupSysNo">组系统编号(组合,团购主表系统编号)</param>
        /// <param name="quantity">组数量</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <param name="source">购物车商品来源</param>
        /// <param name="isChecked">是否选中商品</param>
        /// <returns>购物车明细集合</returns>
        /// <remarks>2013-09-01 吴文强 创建</remarks>
        private IList<CBCrShoppingCartItem> InitGroupToCartItem(int customerSysNo, int groupSysNo, int quantity,
                                                                int promotionSysNo, CustomerStatus.购物车商品来源 source, CustomerStatus.是否选中 isChecked = CustomerStatus.是否选中.是)
        {
            var result = new List<CBCrShoppingCartItem>();
            var promotion = SpPromotionBo.Instance.GetPromotions(new[] { promotionSysNo }).FirstOrDefault();
            if (promotion == null)
            {
                return result;
            }

            switch (promotion.PromotionType)
            {
                case (int)PromotionStatus.促销应用类型.组合套餐:
                    //获取套餐明细
                    var comboItems = ISpComboItemDao.Instance.GetListByComboSysNo(groupSysNo);

                    //将明细添加至购物车明细集合
                    result.AddRange(comboItems.Select(item => new CBCrShoppingCartItem
                    {
                        ProductName = item.ProductName,
                        CustomerSysNo = customerSysNo,
                        ProductSysNo = item.ProductSysNo,
                        Quantity = quantity,
                        OriginPrice = 0,
                        IsChecked = (int)isChecked,
                        IsLock = (int)CustomerStatus.购物车是否锁定.是,
                        IsExpireReset = (int)CustomerStatus.购物车是否过期重置.是,
                        CreateDate = DateTime.Now,
                        Source = (int)CustomerStatus.购物车商品来源.PC网站,
                        ProductSalesType = (int)CustomerStatus.商品销售类型.组合,
                        Promotions = promotionSysNo.ToString(),
                        UsedPromotions = promotionSysNo.ToString(),
                        GroupCode = groupSysNo.ToString(),
                        ProductSalesTypeSysNo = item.SysNo,
                    }));

                    break;
                case (int)PromotionStatus.促销应用类型.团购:
                    //获取套餐明细
                    var groupShoppingItem = IGsGroupShoppingItemDao.Instance.GetItem(groupSysNo);

                    //将明细添加至购物车明细集合
                    result.AddRange(groupShoppingItem.Select(item => new CBCrShoppingCartItem
                        {
                            ProductName = item.ProductName,
                            CustomerSysNo = customerSysNo,
                            ProductSysNo = item.ProductSysNo,
                            Quantity = quantity,
                            OriginPrice = 0,
                            IsChecked = (int)isChecked,
                            IsLock = (int)CustomerStatus.购物车是否锁定.是,
                            IsExpireReset = (int)CustomerStatus.购物车是否过期重置.是,
                            CreateDate = DateTime.Now,
                            Source = (int)CustomerStatus.购物车商品来源.PC网站,
                            ProductSalesType = (int)CustomerStatus.商品销售类型.团购,
                            Promotions = promotionSysNo.ToString(),
                            UsedPromotions = promotionSysNo.ToString(),
                            GroupCode = groupSysNo.ToString(),
                            ProductSalesTypeSysNo = item.SysNo,
                        }));
                    break;
            }
            return result;
        }

        //List<CBCrShoppingCartItem>)CacheItems
        #endregion
    }
}
