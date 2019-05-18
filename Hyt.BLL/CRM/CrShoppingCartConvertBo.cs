using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.BLL.Order;
using Hyt.BLL.Product;
using Hyt.BLL.Promotion;
using Hyt.DataAccess.Promotion;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.CRM
{
    /// <summary>
    /// 购物车对象转换
    /// </summary>
    /// <remarks>2013-09-26 吴文强 创建</remarks>
    public class CrShoppingCartConvertBo : BOBase<CrShoppingCartConvertBo>
    {
        /// <summary>
        /// 订单转购物车对象
        /// </summary>
        /// <param name="platformType">使用平台</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="orderSysNo">订单系统编号</param>
        /// <returns>购物车</returns>
        /// <remarks>2013-09-26 吴文强 创建</remarks>
        public CrShoppingCart GetCartByOrder(PromotionStatus.促销使用平台[] platformType, int customerSysNo, int orderSysNo)
        {
            IList<CBCrShoppingCartItem> shoppingCartItems = new List<CBCrShoppingCartItem>();
            var orderItems = SoOrderBo.Instance.GetOrderItemsByOrderId(orderSysNo);
            var promotionSysNo = GetUsedPromotionSysNo(orderSysNo);
            var order = SoOrderBo.Instance.GetEntity(orderSysNo);
            //获取地址对应的实体，得到父级市，计算运费 王耀发 2016-1-26 创建
            var receiveAddress = SoOrderBo.Instance.GetOrderReceiveAddress(order.ReceiveAddressSysNo);

            foreach (var orderItem in orderItems)
            {
                shoppingCartItems.Add(new CBCrShoppingCartItem()
                    {
                        SysNo = orderItem.SysNo,
                        CustomerSysNo = customerSysNo,
                        IsChecked = (int)CustomerStatus.是否选中.是,
                        ProductSysNo = orderItem.ProductSysNo,
                        ProductName = orderItem.ProductName,
                        Quantity = orderItem.Quantity,
                        OriginPrice = orderItem.OriginalPrice,
                        SalesUnitPrice = orderItem.SalesUnitPrice,
                        SaleTotalAmount = orderItem.SalesUnitPrice * orderItem.Quantity,
                        IsLock = string.IsNullOrEmpty(orderItem.GroupCode)
                                     ? (int)CustomerStatus.购物车是否锁定.否
                                     : (int)CustomerStatus.购物车是否锁定.是,
                        Promotions = orderItem.UsedPromotions,
                        UsedPromotions = orderItem.UsedPromotions,
                        GroupCode = orderItem.GroupCode,
                        ProductSalesType = (int)orderItem.ProductSalesType,
                        ProductSalesTypeSysNo = orderItem.ProductSalesTypeSysNo,
                    });
            }

            var customer = CrCustomerBo.Instance.GetCrCustomerItem(customerSysNo);
            var promotionToPython = new SpPromotionToPython() { Order = order };
            //计算后的购物车对象
            var shoppingCart = SpPromotionEngineBo.Instance.CalculateShoppingCart(
                platformType, customer, shoppingCartItems, promotionSysNo, null, false, receiveAddress.AreaSysNo, order.DeliveryTypeSysNo, null, order.DefaultWarehouseSysNo, false, promotionToPython);

            return shoppingCart;
        }

        /// <summary>
        /// 获取订单已使用的促销系统编号集合
        /// </summary>
        /// <param name="orderSysNo">订单系统编号</param>
        /// <returns>订单已使用的促销系统编号集合</returns>
        /// <remarks>2013-11-14 吴文强 创建</remarks>
        public int[] GetUsedPromotionSysNo(int orderSysNo)
        {
            var promotionSysNo = new List<int>();
            var order = SoOrderBo.Instance.GetEntity(orderSysNo);
            var orderItems = SoOrderBo.Instance.GetOrderItemsByOrderId(orderSysNo);
            if (order == null)
            {
                return new int[] { };
            }
            promotionSysNo.AddRange((order.UsedPromotions ?? string.Empty)
                                                 .Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries)
                                                 .Select(int.Parse));

            foreach (var orderItem in orderItems)
            {
                promotionSysNo.AddRange((orderItem.UsedPromotions ?? string.Empty)
                                                 .Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries)
                                                 .Select(int.Parse));
            }

            return promotionSysNo.Distinct().ToArray();
        }

        /// <summary>
        /// 订单转购物车明细
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="orderSysNo">订单系统编号</param>
        /// <returns>购物车明细</returns>
        /// <remarks>2013-09-26 吴文强 创建</remarks>
        public List<CBCrShoppingCartItem> GetItemByOrder(int customerSysNo, int orderSysNo)
        {
            var shoppingCartItems = new List<CBCrShoppingCartItem>();
            var orderItems = SoOrderBo.Instance.GetOrderItemsByOrderId(orderSysNo);
            foreach (var orderItem in orderItems)
            {
                shoppingCartItems.Add(new CBCrShoppingCartItem()
                {
                    CustomerSysNo = customerSysNo,
                    IsChecked = (int)CustomerStatus.是否选中.是,
                    ProductSysNo = orderItem.ProductSysNo,
                    ProductName = orderItem.ProductName,
                    Quantity = orderItem.Quantity,
                    OriginPrice = orderItem.OriginalPrice,
                    SalesUnitPrice = orderItem.SalesUnitPrice,
                    SaleTotalAmount = orderItem.SalesUnitPrice * orderItem.Quantity,
                    IsLock = string.IsNullOrEmpty(orderItem.GroupCode)
                                 ? (int)CustomerStatus.购物车是否锁定.否
                                 : (int)CustomerStatus.购物车是否锁定.是,
                    Promotions = orderItem.UsedPromotions,
                    UsedPromotions = orderItem.UsedPromotions,
                    GroupCode = orderItem.GroupCode,
                    ProductSalesType = (int)orderItem.ProductSalesType,
                    ProductSalesTypeSysNo = orderItem.ProductSalesTypeSysNo,
                });
            }
            return shoppingCartItems;
        }

        #region 转换购物车明细对象(CBCrShoppingCartItem)

        /// <summary>
        /// 根据赠品转购物车明细
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <param name="quantity">数量</param>
        /// <param name="source">购物车商品来源</param>
        /// <param name="isChecked">是否选中</param>
        /// <returns>购物车明细</returns>
        /// <remarks>2013-11-14 吴文强 创建</remarks>
        public CBCrShoppingCartItem GetCartItemByProduct(int productSysNo, int quantity,
                                                      CustomerStatus.购物车商品来源 source, CustomerStatus.是否选中 isChecked = CustomerStatus.是否选中.是)
        {
            return new CBCrShoppingCartItem
            {
                IsChecked = (int)isChecked,
                ProductSysNo = productSysNo,
                ProductName = "",
                Quantity = quantity,
                OriginPrice = 0,
                IsLock = (int)CustomerStatus.购物车是否锁定.否,
                CreateDate = DateTime.Now,
                Source = (int)source,
                ProductSalesType = (int)CustomerStatus.商品销售类型.普通,
            };
        }

        /// <summary>
        /// 根据赠品转购物车明细
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <param name="quantity">数量</param>
        /// <param name="promotionSysNo">使用促销系统编号</param>
        /// <param name="source">购物车商品来源</param>
        /// <returns>购物车明细</returns>
        /// <remarks>2013-11-14 吴文强 创建</remarks>
        public CBCrShoppingCartItem GetCartItemByGift(int productSysNo, int quantity, int promotionSysNo,
                                                      CustomerStatus.购物车商品来源 source)
        {
            return new CBCrShoppingCartItem
            {
                IsChecked = (int)CustomerStatus.是否选中.是,
                ProductSysNo = productSysNo,
                ProductName = "",
                Quantity = quantity,
                OriginPrice = 0,
                IsLock = (int)CustomerStatus.购物车是否锁定.否,
                IsExpireReset = (int)CustomerStatus.购物车是否过期重置.是,
                CreateDate = DateTime.Now,
                Source = (int)source,
                ProductSalesType = (int)CustomerStatus.商品销售类型.赠品,
                UsedPromotions = promotionSysNo.ToString(),
            };
        }

        /// <summary>
        /// 将组促销转换为购物车明细
        /// </summary>
        /// <param name="groupSysNo">组系统编号(组合,团购主表系统编号)</param>
        /// <param name="quantity">组数量</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <param name="source">购物车商品来源</param>
        /// <param name="isChecked">是否选中</param>
        /// <returns>购物车明细集合</returns>
        /// <remarks>2013-09-01 吴文强 创建</remarks>
        public IList<CBCrShoppingCartItem> GetCartItemByGroup(int groupSysNo, int quantity,
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

        #endregion

        /// <summary>
        /// 初始化购物车商品价格
        /// </summary>
        /// <param name="customerLevel">客户等级</param>
        /// <param name="shoppingCartItems">购物车明细集合</param>
        /// <returns>已添加价格的购物车明细集合</returns>
        /// <remarks>2013-08-12 吴文强 创建</remarks>
        public IList<CBCrShoppingCartItem> InitItemPriceByProduct(int customerLevel, IList<CBCrShoppingCartItem> shoppingCartItems)
        {
            foreach (var crShoppingCart in shoppingCartItems)
            {
                var product = PdProductBo.Instance.GetProduct(crShoppingCart.ProductSysNo);
                var basePrice = product.PdPrice.Value.FirstOrDefault(
                    p => p.PriceSource == (int)ProductStatus.产品价格来源.基础价格);
                var levelPrice = product.PdPrice.Value.FirstOrDefault(
                    p => p.PriceSource == (int)ProductStatus.产品价格来源.会员等级价
                            && p.SourceSysNo == customerLevel);
                var price = (levelPrice == null ? basePrice.Price : levelPrice.Price);

                if (crShoppingCart.IsLock == 0)
                {
                    crShoppingCart.ProductName = product.ProductName;
                    crShoppingCart.OriginPrice = price;
                    crShoppingCart.SalesUnitPrice = price;
                    crShoppingCart.SaleTotalAmount = crShoppingCart.SalesUnitPrice * crShoppingCart.Quantity;
                    crShoppingCart.DiscountAmount = 0;
                }
                else
                {
                    //锁定的组商品
                    switch (crShoppingCart.ProductSalesType)
                    {
                        case (int)CustomerStatus.商品销售类型.组合:
                            //获取套餐明细
                            var comboItem = ISpComboItemDao.Instance.GetEntity(crShoppingCart.ProductSalesTypeSysNo);
                            crShoppingCart.OriginPrice = price;
                            crShoppingCart.SalesUnitPrice = price - comboItem.DiscountAmount;
                            crShoppingCart.SaleTotalAmount = crShoppingCart.SalesUnitPrice * crShoppingCart.Quantity;
                            crShoppingCart.DiscountAmount = 0;

                            break;

                        case (int)CustomerStatus.商品销售类型.团购:
                            //获取团购明细
                            var groupShoppingItems = IGsGroupShoppingItemDao.Instance.GetItem(int.Parse(crShoppingCart.GroupCode));
                            var groupShoppingItem =
                                groupShoppingItems.FirstOrDefault(
                                    item => item.ProductSysNo == crShoppingCart.ProductSysNo);

                            if (groupShoppingItem != null)
                            {
                                crShoppingCart.OriginPrice = price;
                                crShoppingCart.SalesUnitPrice = groupShoppingItem.GroupShoppingPrice;
                                crShoppingCart.SaleTotalAmount = crShoppingCart.SalesUnitPrice * crShoppingCart.Quantity;
                                crShoppingCart.DiscountAmount = 0;
                            }
                            break;
                    }
                }
            }
            return shoppingCartItems;
        }

        /// <summary>
        /// 移除购物车中下架的商品
        /// </summary>
        /// <param name="shoppingCartItems">购物车明细集合</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="isFrontProduct">是否只允许前台下单的商品</param>
        /// <returns>购物车明细集合</returns>
        /// <remarks>2013-12-24 吴文强 创建</remarks>
        public IList<CBCrShoppingCartItem> RemoveSoldoutProduct(IList<CBCrShoppingCartItem> shoppingCartItems, int? customerSysNo = null, bool isFrontProduct = true)
        {
            var products =
                PdProductBo.Instance.GetOnlineProduct(shoppingCartItems.Select(ci => ci.ProductSysNo).ToArray(), isFrontProduct);

            if (customerSysNo != null)
            {
                var soldout = shoppingCartItems.Where(ci => !products.Contains(ci.ProductSysNo)).Select(ci => ci.ProductSysNo).ToArray();
                CrShoppingCartBo.Instance.RemoveByProductSysNo((int)customerSysNo, soldout);
            }

            return shoppingCartItems.Where(ci => products.Contains(ci.ProductSysNo)).ToList();
        }

        /// <summary>
        /// 移除购物车中下架的赠品
        /// </summary>
        /// <param name="shoppingCart">购物车</param>
        /// <param name="isFrontProduct">是否只允许前台下单的商品(默认：true)</param>
        /// <returns>购物车</returns>
        /// <remarks>2013-12-24 吴文强 创建</remarks>
        public CrShoppingCart RemoveSoldoutGift(CrShoppingCart shoppingCart, bool isFrontProduct = true)
        {
            var allPromotionGifts = new List<CBSpPromotionGift>();
            foreach (var shoppingCartGroup in shoppingCart.ShoppingCartGroups)
            {
                if (shoppingCartGroup.GroupPromotions == null)
                {
                    continue;
                }

                foreach (var groupPromotion in shoppingCartGroup.GroupPromotions.Where(groupPromotion => groupPromotion.GiftProducts != null))
                {
                    allPromotionGifts.AddRange(groupPromotion.GiftProducts);
                }
            }

            if (shoppingCart.GroupPromotions != null)
            {
                foreach (var groupPromotion in shoppingCart.GroupPromotions.Where(groupPromotion => groupPromotion.GiftProducts != null))
                {
                    allPromotionGifts.AddRange(groupPromotion.GiftProducts);
                }
            }

            var giftProducts =
                PdProductBo.Instance.GetOnlineProduct(allPromotionGifts.Select(ci => ci.ProductSysNo).ToArray(), isFrontProduct);

            foreach (var shoppingCartGroup in shoppingCart.ShoppingCartGroups)
            {
                if (shoppingCartGroup.GroupPromotions == null)
                {
                    continue;
                }

                foreach (var groupPromotion in shoppingCartGroup.GroupPromotions.Where(groupPromotion => groupPromotion.GiftProducts != null))
                {
                    groupPromotion.GiftProducts =
                        groupPromotion.GiftProducts.Where(t => giftProducts.Contains(t.ProductSysNo)).ToList();
                }
            }

            if (shoppingCart.GroupPromotions != null)
            {
                foreach (var groupPromotion in shoppingCart.GroupPromotions.Where(groupPromotion => groupPromotion.GiftProducts != null))
                {
                    groupPromotion.GiftProducts =
                        groupPromotion.GiftProducts.Where(t => giftProducts.Contains(t.ProductSysNo)).ToList();
                }
            }
            return shoppingCart;
        }
    }
}
