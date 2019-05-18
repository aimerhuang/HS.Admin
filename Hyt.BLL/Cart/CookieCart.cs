using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.BLL.Product;
using Hyt.BLL.Promotion;
using Hyt.DataAccess.Promotion;
using Hyt.Model;
using Hyt.Model.SystemPredefined;
using Hyt.Util.Serialization;
using Hyt.Model.WorkflowStatus;
using Hyt.Util;
using Hyt.BLL.CRM;

namespace Hyt.BLL.Cart
{
    /// <summary>
    /// 本地购物车操作
    /// </summary>
    /// <remarks>2013-10-16 黄波 创建</remarks>
    public class CookieCart : ICart, ICookieCart
    {
        #region 操作方法
        /// <summary>
        /// 购物车对象
        /// </summary>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        private List<JsonCartItem> _cookieCart = null;

        /// <summary>
        /// 创建用于操作CookieCart的新实例
        /// </summary>
        /// <param name="customerKey">用户识别号</param>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        public CookieCart(string customerKey)
        {
            _cookieCart = Hyt.Util.CookieUtil.Get<List<JsonCartItem>>(customerKey) ?? new List<JsonCartItem>();
        }

        /// <summary>
        /// 设置cart内容
        /// </summary>
        /// <return></return>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        private void SetCookieCart()
        {
            try
            {
                Hyt.Util.CookieUtil.SetCookie(
                    Hyt.Model.SystemPredefined.Constant.CART_COOKIE_NAME
                    , _cookieCart.ToJson()
                    , DateTime.Now.Add(new TimeSpan(Hyt.Model.SystemPredefined.Constant.CART_COOKIE_EXPIRY, 0, 0, 0))
                );
            }
            catch { }
        }
        #endregion

        /// <summary>
        /// 获取购物车对象
        /// </summary>
        /// <param name="isChecked">true:只查询选中的明细;false:查询全部,默认false</param>
        /// <returns>购物车对象</returns>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        public override CrShoppingCart GetShoppingCart(bool isChecked = false)
        {
            SetCookieCart();
            var shoppingCartItems = new List<CBCrShoppingCartItem>();
            foreach (var jsonCartItem in _cookieCart)
            {
                int groupSysNo, promotionSysNo;
                //组合套餐，团购数据处理
                if (!string.IsNullOrEmpty(jsonCartItem.GroupCode) && !string.IsNullOrEmpty(jsonCartItem.Promotions)
                    && int.TryParse(jsonCartItem.GroupCode, out groupSysNo) && int.TryParse(jsonCartItem.Promotions, out promotionSysNo))
                {
                    shoppingCartItems.AddRange(GroupToCartItem(groupSysNo, jsonCartItem.Quantity, promotionSysNo, CustomerStatus.购物车商品来源.PC网站,
                                    (CustomerStatus.是否选中)jsonCartItem.IsChecked));

                }
                else if (jsonCartItem.ProductSalesType == (int)CustomerStatus.商品销售类型.赠品)
                {
                    shoppingCartItems.Add(ProductToCartItem(jsonCartItem.ProductSysNo, jsonCartItem.Quantity,
                                                            CustomerStatus.购物车商品来源.PC网站, CustomerStatus.商品销售类型.赠品, CustomerStatus.是否选中.是, 0, jsonCartItem.Promotions));
                }
                else
                {
                    shoppingCartItems.Add(ProductToCartItem(jsonCartItem.ProductSysNo, jsonCartItem.Quantity,
                                                            CustomerStatus.购物车商品来源.PC网站, CustomerStatus.商品销售类型.普通,
                                                            (CustomerStatus.是否选中)jsonCartItem.IsChecked, jsonCartItem.SysNo));
                }
            }

            return GetShoppingCart(new[] { PromotionStatus.促销使用平台.PC商城 }, shoppingCartItems);
        }

        /// <summary>
        /// 获取购物车对象
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="sysNo">购物车明细系统编号(null:购物车为服务器选中的对象)</param>
        /// <param name="areaSysNo">区域系统编号</param>
        /// <param name="deliveryTypeSysNo">配送方式系统编号</param>
        /// <param name="promotionCode">促销代码</param>
        /// <param name="couponCode">优惠券代码</param>
        /// <param name="isChecked">true:只查询选中的明细;false:查询全部,默认false</param>
        /// <returns>购物车对象</returns>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        public override CrShoppingCart GetShoppingCart(int[] sysNo, int? areaSysNo, int? deliveryTypeSysNo, string promotionCode, string couponCode, bool isChecked = false)
        {
            throw new NotImplementedException("未登录的不应该调用到此方法！");
        }

        /// <summary>
        /// 添加商品至购物车
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <param name="quantity">商品数量</param>
        /// <param name="source">购物车商品来源</param>
        /// <return></return>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        public override void Add(int productSysNo, int quantity, Model.WorkflowStatus.CustomerStatus.购物车商品来源 source)
        {
            var productItems = _cookieCart.Find(o => o.ProductSysNo == productSysNo);
            if (productItems != null)
            {
                productItems.Quantity += quantity;
            }
            else
            {
                _cookieCart.Add(new JsonCartItem
                {
                    SysNo = CreateSysNo(),
                    GroupCode = "",
                    IsLock = (int)CustomerStatus.购物车是否锁定.否,
                    IsChecked = (int)CustomerStatus.是否选中.是,
                    ProductSalesType = (int)CustomerStatus.商品销售类型.普通,
                    ProductSysNo = productSysNo,
                    Promotions = "",
                    Quantity = quantity
                });
            }
            SetCookieCart();
        }

        /// <summary>
        /// 添加促销商品至购物车
        /// </summary>
        /// <param name="groupSysNo">组系统编号(组合,团购主表系统编号)</param>
        /// <param name="quantity">组数量</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <param name="source">购物车商品来源</param>
        /// <returns></returns>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        public override void Add(int groupSysNo, int quantity, int promotionSysNo, CustomerStatus.购物车商品来源 source)
        {
            var productItems = _cookieCart.FindAll(o => o.GroupCode == groupSysNo.ToString() && o.Promotions == promotionSysNo.ToString()).ToArray();

            if (productItems != null && productItems.Any())
            {
                foreach (var jsonCartItem in productItems)
                {
                    jsonCartItem.Quantity += quantity;
                }
            }
            else
            {
                var groupItems = GroupToCartItem(groupSysNo, quantity, promotionSysNo, source);

                foreach (var shoppingCartItem in groupItems)
                {
                    _cookieCart.Add(new JsonCartItem
                    {
                        SysNo = CreateSysNo(),
                        GroupCode = groupSysNo.ToString(),
                        IsLock = (int)CustomerStatus.购物车是否锁定.是,
                        IsChecked = (int)CustomerStatus.是否选中.是,
                        ProductSalesType = shoppingCartItem.ProductSalesType,
                        ProductSysNo = shoppingCartItem.ProductSysNo,
                        Promotions = promotionSysNo.ToString(),
                        Quantity = quantity
                    });
                }
            }

            SetCookieCart();
        }

        /// <summary>
        /// 更新购物车明细商品数量
        /// </summary>
        /// <param name="sysNo">购物车明细系统编号集合</param>
        /// <param name="quantity">商品数量</param>
        /// <return></return>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        public override void UpdateQuantity(int[] sysNo, int quantity)
        {
            JsonCartItem productItem = null;
            foreach (var sysno in sysNo)
            {
                productItem = _cookieCart.Find(o => o.SysNo == sysno && o.IsLock == (int)CustomerStatus.购物车是否锁定.否);
                if (productItem != null)
                {
                    productItem.Quantity = quantity;
                }
            }
            SetCookieCart();
        }

        /// <summary>
        /// 更新购物车组组商品数量
        /// </summary>
        /// <param name="groupCode">组代码</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <param name="quantity">商品数量</param>
        /// <return></return>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        public override void UpdateQuantity(string groupCode, string promotionSysNo, int quantity)
        {
            var productItem = _cookieCart.Find(o => o.GroupCode == groupCode && o.Promotions == promotionSysNo);
            if (productItem != null)
            {
                _cookieCart.Find(o => o.GroupCode == groupCode && o.Promotions == promotionSysNo).Quantity = quantity;
            }

            SetCookieCart();
        }

        /// <summary>
        /// 选择购物车所有明细项目
        /// </summary>
        /// <return></return>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        public override void CheckedAll()
        {
            _cookieCart.ForEach(delegate(JsonCartItem item)
            {
                item.IsChecked = (int)CustomerStatus.是否选中.是;
            });
            SetCookieCart();
        }

        /// <summary>
        /// 取消选择购物车所有明细项目
        /// </summary>
        /// <return></return>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        public override void UncheckedAll()
        {
            _cookieCart.ForEach(delegate(JsonCartItem item)
            {
                item.IsChecked = (int)CustomerStatus.是否选中.否;
            });
            SetCookieCart();
        }

        /// <summary>
        /// 选择购物车明细项目
        /// </summary>
        /// <param name="itemSysNo">购物车明细系统编号</param>
        /// <return></return>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        public override void CheckedItem(int[] itemSysNo)
        {
            JsonCartItem productItem = null;
            itemSysNo.ForEach(delegate(int sysno)
            {
                productItem = _cookieCart.Find(o => o.SysNo == sysno && o.IsLock == (int)CustomerStatus.购物车是否锁定.否);
                if (productItem != null)
                {
                    productItem.IsChecked = (int)CustomerStatus.是否选中.是;
                }
            });
            SetCookieCart();
        }

        /// <summary>
        /// 取消选择购物车明细项目
        /// </summary>
        /// <param name="itemSysNo">购物车明细系统编号</param>
        /// <return></return>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        public override void UncheckedItem(int[] itemSysNo)
        {
            JsonCartItem productItem = null;
            itemSysNo.ForEach(delegate(int sysno)
            {
                productItem = _cookieCart.Find(o => o.SysNo == sysno && o.IsLock == (int)CustomerStatus.购物车是否锁定.否);
                if (productItem != null)
                {
                    productItem.IsChecked = (int)CustomerStatus.是否选中.否;
                }
            });
            SetCookieCart();
        }

        /// <summary>
        /// 选择购物车组明细项目
        /// </summary>
        /// <param name="groupCode">组代码</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <return></return>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        public override void CheckedItem(string groupCode, string promotionSysNo)
        {
            var groupProductList = _cookieCart.FindAll(o => o.GroupCode == groupCode && o.Promotions == promotionSysNo);
            if (groupProductList != null && groupProductList.Any())
            {
                groupProductList.ForEach(o => o.IsChecked = (int)CustomerStatus.是否选中.是);
            }
            SetCookieCart();
        }

        /// <summary>
        /// 取消选择购物车组明细项目
        /// </summary>
        /// <param name="groupCode">组代码</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <return></return>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        public override void UncheckedItem(string groupCode, string promotionSysNo)
        {
            var groupProductList = _cookieCart.FindAll(o => o.GroupCode == groupCode && o.Promotions == promotionSysNo);
            if (groupProductList != null && groupProductList.Any())
            {
                groupProductList.ForEach(o => o.IsChecked = (int)CustomerStatus.是否选中.否);
            }
            SetCookieCart();
        }

        /// <summary>
        /// 删除购物车明细
        /// </summary>
        /// <param name="sysNo">购物车明细系统编号</param>
        /// <return></return>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        public override void Remove(int[] sysNo)
        {
            var productItem = -1;
            sysNo.ForEach(delegate(int sysno)
            {
                productItem = _cookieCart.FindIndex(o => o.SysNo == sysno && o.IsLock == (int)CustomerStatus.购物车是否锁定.否);
                if (productItem != -1)
                {
                    _cookieCart.RemoveAt(productItem);
                }
            });
            SetCookieCart();
        }

        /// <summary>
        /// 删除购物车组商品
        /// </summary>
        /// <param name="groupCode">组代码</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <return></return>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        public override void Remove(string groupCode, string promotionSysNo)
        {
            var productItems = _cookieCart.FindAll(o => o.GroupCode == groupCode && o.Promotions == promotionSysNo);
            if (productItems != null)
            {
                foreach (var cartItem in productItems)
                {
                    _cookieCart.Remove(cartItem);
                }
            }
            SetCookieCart();
        }

        /// <summary>
        /// 删除购物车选中的明细
        /// </summary>
        /// <return></return>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        public override void RemoveCheckedItem()
        {
            for (var i = 0; i < _cookieCart.Count; i++)
            {
                if (_cookieCart[i].IsChecked == (int)CustomerStatus.是否选中.是)
                {
                    _cookieCart.RemoveAt(i);
                    i--;
                }
            }
            SetCookieCart();
        }

        /// <summary>
        /// 删除购物车所有明细
        /// </summary>
        /// <return></return>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        public override void RemoveAll()
        {
            _cookieCart = new List<JsonCartItem>();
            SetCookieCart();
        }

        /// <summary>
        /// 添加促销赠品至购物车
        /// </summary>
        /// <param name="productSysNo">商品(赠品)系统编号</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <param name="source">购物车商品来源</param>
        /// <return></return>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        public override void AddGift(int productSysNo, int promotionSysNo, Model.WorkflowStatus.CustomerStatus.购物车商品来源 source)
        {
            var giftItem = _cookieCart.Find(o =>
                                                o.ProductSysNo == productSysNo
                                                && o.Promotions == promotionSysNo.ToString()
                                                && o.ProductSalesType == (int)CustomerStatus.商品销售类型.赠品
                                                );
            if ((giftItem == null))
            {
                _cookieCart.Add(new JsonCartItem
                    {
                        SysNo = CreateSysNo(),
                        GroupCode = "",
                        IsLock = (int)CustomerStatus.购物车是否锁定.是,
                        IsChecked = (int)CustomerStatus.是否选中.是,
                        ProductSalesType = (int)CustomerStatus.商品销售类型.赠品,
                        ProductSysNo = productSysNo,
                        Promotions = promotionSysNo.ToString(),
                        Quantity = 1
                    });
            }
            SetCookieCart();
        }

        /// <summary>
        /// 删除促销赠品
        /// </summary>
        /// <param name="productSysNo">商品(赠品)系统编号</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <return></return>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        public override void RemoveGift(int productSysNo, int promotionSysNo)
        {
            var giftItem = _cookieCart.Find(o =>
                                                   o.ProductSysNo == productSysNo
                                                   && o.Promotions == promotionSysNo.ToString()
                                                   && o.ProductSalesType == (int)CustomerStatus.商品销售类型.赠品
                                                   );
            if (giftItem != null)
            {
                _cookieCart.Remove(giftItem);
            }

            SetCookieCart();
        }

        /// <summary>
        /// 将本地购物车存到客户数据库购物车中
        /// </summary>
        /// <param name="customerSysNo">客户编号</param>
        /// <return></return>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        public void ToDatabase(int customerSysNo)
        {
            if (_cookieCart.Any())
            {
                //商品
                var singleProductList = _cookieCart.FindAll(o => o.IsLock == (int)CustomerStatus.购物车是否锁定.否);
                if (singleProductList.Any())
                {
                    singleProductList.ForEach(delegate(JsonCartItem item)
                    {
                        CrShoppingCartBo.Instance.Add(customerSysNo, item.ProductSysNo, item.Quantity, CustomerStatus.购物车商品来源.PC网站);
                    });
                }

                //组合商品
                var resultDistinct = from item in _cookieCart
                                     group item by new
                                     {
                                         item.GroupCode,
                                         item.Promotions
                                     }
                                         into t
                                         where t.FirstOrDefault().IsLock == (int)CustomerStatus.购物车是否锁定.是 && t.FirstOrDefault().ProductSalesType != (int)CustomerStatus.商品销售类型.赠品
                                         select new JsonCartItem
                                         {
                                             IsChecked = t.FirstOrDefault().IsChecked,
                                             GroupCode = t.FirstOrDefault().GroupCode,
                                             IsLock = t.FirstOrDefault().IsLock,
                                             Quantity = t.FirstOrDefault().Quantity,
                                             Promotions = t.FirstOrDefault().Promotions,
                                             ProductSysNo = t.FirstOrDefault().ProductSysNo,
                                             ProductSalesType = t.FirstOrDefault().ProductSalesType
                                         };
                if (resultDistinct.Any())
                {
                    foreach (var item in resultDistinct)
                    {
                        CrShoppingCartBo.Instance.Add(customerSysNo, Convert.ToInt32(item.GroupCode), item.Quantity, Convert.ToInt32(item.Promotions), CustomerStatus.购物车商品来源.PC网站);
                    }
                }

                //赠品
                var giftProductList = _cookieCart.FindAll(o => o.ProductSalesType == (int)CustomerStatus.商品销售类型.赠品);
                if (giftProductList.Any())
                {
                    giftProductList.ForEach(delegate(JsonCartItem item)
                    {
                        CrShoppingCartBo.Instance.AddGift(customerSysNo, item.ProductSysNo, Convert.ToInt32(item.Promotions), CustomerStatus.购物车商品来源.PC网站);
                    });
                }
            }
        }

        #region 私有方法
        /// <summary>
        /// 生成临时系统编号
        /// </summary>
        /// <returns>临时系统编号</returns>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        private int CreateSysNo()
        {
            var sysNo = 1;
            while (_cookieCart.Any(o => o.SysNo == sysNo))
            {
                sysNo = new Random().Next(0, 10000);
            }
            return sysNo;
        }
        /// <summary>
        /// 将组促销转换为购物车明细
        /// </summary>
        /// <param name="groupSysNo">组系统编号(组合,团购主表系统编号)</param>
        /// <param name="quantity">组数量</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <param name="source">购物车商品来源</param>
        /// <param name="isChecked">是否选中商品</param>
        /// <returns>购物车明细集合</returns>
        /// <remarks>2013-09-01 吴文强 创建</remarks>
        private IList<CBCrShoppingCartItem> GroupToCartItem(int groupSysNo, int quantity,
                                                                int promotionSysNo, CustomerStatus.购物车商品来源 source = CustomerStatus.购物车商品来源.PC网站, CustomerStatus.是否选中 isChecked = CustomerStatus.是否选中.是)
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
                        CustomerSysNo = -1,
                        ProductSysNo = item.ProductSysNo,
                        Quantity = quantity,
                        OriginPrice = 0,
                        IsChecked = (int)isChecked,
                        IsLock = (int)CustomerStatus.购物车是否锁定.是,
                        IsExpireReset = (int)CustomerStatus.购物车是否过期重置.是,
                        CreateDate = DateTime.Now,
                        Source = (int)source,
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
                        CustomerSysNo = -1,
                        ProductSysNo = item.ProductSysNo,
                        Quantity = quantity,
                        OriginPrice = 0,
                        IsChecked = (int)isChecked,
                        IsLock = (int)CustomerStatus.购物车是否锁定.是,
                        IsExpireReset = (int)CustomerStatus.购物车是否过期重置.是,
                        CreateDate = DateTime.Now,
                        Source = (int)source,
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

        /// <summary>
        /// 计算购物车
        /// </summary>
        /// <param name="platformType">使用平台</param>
        /// <param name="shoppingCartItems">购物车明细集合</param>
        /// <returns>购物车</returns>
        /// <remarks>2013-08-31 吴文强 创建</remarks>
        private CrShoppingCart GetShoppingCart(PromotionStatus.促销使用平台[] platformType, IList<CBCrShoppingCartItem> shoppingCartItems)
        {
            var levelSysNo = CustomerLevel.初级;

            //移除下架的商品
            shoppingCartItems = CrShoppingCartConvertBo.Instance.RemoveSoldoutProduct(shoppingCartItems);

            //重置购物车明细商品
            shoppingCartItems = ResetItemByPromotion(platformType, shoppingCartItems);

            //购物车明细
            shoppingCartItems = CrShoppingCartBo.Instance.InitProductPrice(levelSysNo, shoppingCartItems);

            //计算后的购物车对象
            var shoppingCart = SpPromotionEngineBo.Instance.CalculateShoppingCart(platformType, new CrCustomer(), shoppingCartItems,
                                                                                  null, null, true);
            //移除前台不能下单的赠品
            shoppingCart = CrShoppingCartConvertBo.Instance.RemoveSoldoutGift(shoppingCart);

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
        /// 添加商品至购物车
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <param name="quantity">商品数量</param>
        /// <param name="source">购物车商品来源</param>
        /// <param name="sysNo">系统编号</param>
        /// <returns>购物车商品明细</returns>
        /// <remarks>2013-08-13 吴文强 创建</remarks>
        private CBCrShoppingCartItem ProductToCartItem(int productSysNo, int quantity, CustomerStatus.购物车商品来源 source = CustomerStatus.购物车商品来源.PC网站,
                                                       CustomerStatus.商品销售类型 productSalesType = CustomerStatus.商品销售类型.普通,
            CustomerStatus.是否选中 isChecked = CustomerStatus.是否选中.是, int sysNo = 0, string promotions = null)
        {
            var shoppingCartItem = new CBCrShoppingCartItem();

            var productPrices = PdPriceBo.Instance.GetProductPrice(productSysNo,
                                                                   new ProductStatus.产品价格来源[] { ProductStatus.产品价格来源.会员等级价 });
            var firstOrDefault = productPrices.FirstOrDefault(p => p.SourceSysNo == CustomerLevel.初级);
            if (firstOrDefault != null)
            {
                shoppingCartItem = new CBCrShoppingCartItem
                    {
                        SysNo = sysNo,
                        IsChecked = (int)isChecked,
                        CustomerSysNo = -1,
                        ProductSysNo = productSysNo,
                        ProductName = "",
                        Quantity = quantity,
                        OriginPrice = firstOrDefault.Price,
                        IsLock = (int)CustomerStatus.购物车是否锁定.否,
                        IsExpireReset = (int)CustomerStatus.购物车是否过期重置.是,
                        CreateDate = DateTime.Now,
                        Source = (int)source,
                        ProductSalesType = (int)productSalesType,
                        Promotions = promotions,
                        UsedPromotions = promotions,
                    };
            }

            return shoppingCartItem;
        }

        #endregion
    }
}
