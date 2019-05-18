using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.BLL.Order;
using Hyt.BLL.Product;
using Hyt.BLL.Promotion;
using Hyt.DataAccess.Promotion;
using Hyt.Model;
using Hyt.DataAccess.CRM;
using Hyt.Model.SystemPredefined;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.CRM
{
    /// <summary>
    /// 购物车扩展
    /// </summary>
    /// <remarks>2013-09-26 吴文强 创建</remarks>
    public static class CrShoppingCartExtensions
    {
        /// <summary>
        /// 赠品转购物车明细
        /// </summary>
        /// <param name="promotionGifts">赠品集合</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <returns>购物车明细集合</returns>
        /// <remarks>2013-09-26 吴文强 创建</remarks>
        public static IList<CBCrShoppingCartItem> ConvertShoppingCartItems(this IList<CBSpPromotionGift> promotionGifts, int customerSysNo = -1)
        {
            if (promotionGifts == null) return new List<CBCrShoppingCartItem>();
            var items = promotionGifts.Select(giftProduct => new CBCrShoppingCartItem()
                {
                    CustomerSysNo = customerSysNo,
                    IsChecked = (int)CustomerStatus.是否选中.是,
                    ProductSysNo = giftProduct.ProductSysNo,
                    ProductName = giftProduct.ProductName,
                    Quantity = 1,
                    IsLock = (int)CustomerStatus.购物车是否锁定.否,
                    Promotions = giftProduct.PromotionSysNo.ToString(),
                    UsedPromotions = giftProduct.PromotionSysNo.ToString(),
                    ProductSalesType = (int)CustomerStatus.商品销售类型.赠品
                }).ToList();
            return items;
        }

        /// <summary>
        /// 购物车转购物车明细
        /// </summary>
        /// <param name="shoppingCart">购物车</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <returns>购物车明细</returns>
        /// <remarks>2013-09-26 吴文强 创建</remarks>
        public static List<CBCrShoppingCartItem> GetShoppingCartItem(this CrShoppingCart shoppingCart, int customerSysNo = -1)
        {
            var items = new List<CBCrShoppingCartItem>();

            //读取赠品
            if (shoppingCart.GroupPromotions != null)
            {
                foreach (var groupPromotion in shoppingCart.GroupPromotions)
                {
                    items.AddRange(groupPromotion.UsedGiftProducts.ConvertShoppingCartItems(customerSysNo));
                }
            }

            //购物车组
            if (shoppingCart.ShoppingCartGroups != null)
            {
                foreach (var cartGroup in shoppingCart.ShoppingCartGroups)
                {
                    //读取赠品
                    if (cartGroup.GroupPromotions != null)
                    {
                        foreach (var groupPromotion in cartGroup.GroupPromotions)
                        {
                            if (groupPromotion != null && groupPromotion.UsedGiftProducts != null)
                            {
                                items.AddRange(groupPromotion.UsedGiftProducts.ConvertShoppingCartItems(customerSysNo));
                            }
                        }
                    }
                    items.AddRange(cartGroup.ShoppingCartItems);
                }
            }
            return items;
        }

        /// <summary>
        /// 购物车转购物车明细简单对象(用于Json存储)
        /// </summary>
        /// <param name="shoppingCart">购物车</param>
        /// <returns>Json格式的购物车明细集合</returns>
        /// <remarks>2013-09-26 吴文强 创建</remarks>
        public static List<JsonCartItem> ConvertJson(this CrShoppingCart shoppingCart)
        {
            var jsonCartItems = new List<JsonCartItem>();
            var items = shoppingCart.GetShoppingCartItem();

            var promotionKeys = new List<string>();
            foreach (var item in items)
            {
                if (item.IsLock == (int)CustomerStatus.购物车是否锁定.是 && !string.IsNullOrEmpty(item.GroupCode))
                {
                    var keys = string.Format("{0},{1}", item.Promotions, item.GroupCode);
                    if (promotionKeys.Contains(keys))
                    {
                        continue;
                    }
                    promotionKeys.Add(keys);
                }

                jsonCartItems.Add(new JsonCartItem()
                    {
                        IsChecked = item.IsChecked,
                        GroupCode = item.GroupCode,
                        IsLock = item.IsLock,
                        ProductSysNo = item.ProductSysNo,
                        Promotions = item.Promotions,
                        Quantity = item.Quantity,
                        ProductSalesType = item.ProductSalesType
                    });
            }
            return jsonCartItems;
        }

        /// <summary>
        /// 购物车简单对象转购物车商品明细集合（使用当前商品价格）
        /// </summary>
        /// <param name="jsonCartItems">Json格式的购物车明细</param>
        /// <param name="customerLevelSysNo">客户系统编号</param>
        /// <param name="source">购物车商品来源</param>
        /// <returns>购物车商品明细集合</returns>
        /// <remarks>2013-11-14 吴文强 创建</remarks>
        public static IList<CBCrShoppingCartItem> ConvertShoppingCartItems(this IList<JsonCartItem> jsonCartItems, int customerLevelSysNo,
            CustomerStatus.购物车商品来源 source = CustomerStatus.购物车商品来源.PC网站)
        {
            var shoppingCartItems = new List<CBCrShoppingCartItem>();
            var promotionKeys = new List<string>();

            foreach (var item in jsonCartItems)
            {
                if (item.ProductSalesType == (int)CustomerStatus.商品销售类型.赠品)
                {
                    shoppingCartItems.Add(
                        CrShoppingCartConvertBo.Instance.GetCartItemByGift(item.ProductSysNo, item.Quantity,
                                                                           int.Parse(item.Promotions), source));
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

                        shoppingCartItems.AddRange(
                        CrShoppingCartConvertBo.Instance.GetCartItemByGroup(int.Parse(item.GroupCode), item.Quantity,
                                                                           int.Parse(item.Promotions), source, (CustomerStatus.是否选中)item.IsChecked));
                    }
                    else
                    {
                        shoppingCartItems.Add(
                            CrShoppingCartConvertBo.Instance.GetCartItemByProduct
                                (item.ProductSysNo, item.Quantity, source, (CustomerStatus.是否选中)item.IsChecked));
                    }
                }
            }

            //购物车明细
            shoppingCartItems =
                CrShoppingCartConvertBo.Instance.InitItemPriceByProduct(customerLevelSysNo, shoppingCartItems).ToList();

            return shoppingCartItems;
        }

        /// <summary>
        /// 购物车商品数(不包含赠品)
        /// </summary>
        /// <param name="shoppingCart">购物车</param>
        /// <returns>购物车商品数</returns>
        /// <remarks>2013-10-16 吴文强 创建</remarks>
        public static int CartItemNumber(this CrShoppingCart shoppingCart)
        {
            return shoppingCart.GetShoppingCartItem()
                               .Where(t => t.ProductSalesType != (int)CustomerStatus.商品销售类型.赠品).Sum(t => t.Quantity);
        }

        /// <summary>
        /// 获取购物车中所有赠品
        /// </summary>
        /// <param name="shoppingCart">购物车对象</param>
        /// <returns>赠品集合</returns>
        /// <remarks>2013-10-24 吴文强 创建</remarks>
        public static IList<CBSpPromotionGift> Gifts(this CrShoppingCart shoppingCart)
        {
            var promotionGifts = new List<CBSpPromotionGift>();
            promotionGifts.AddRange(shoppingCart.ProductGroupGifts());
            promotionGifts.AddRange(shoppingCart.OrderGroupGifts());
            return promotionGifts;
        }

        /// <summary>
        /// 商品组的赠品
        /// </summary>
        /// <param name="shoppingCart">购物车对象</param>
        /// <returns>赠品集合</returns>
        /// <remarks>2013-11-29 吴文强 创建</remarks>
        public static IList<CBSpPromotionGift> ProductGroupGifts(this CrShoppingCart shoppingCart)
        {
            var promotionGifts = new List<CBSpPromotionGift>();
            foreach (var shoppingCartGroup in shoppingCart.ShoppingCartGroups)
            {
                if (shoppingCartGroup.GroupPromotions == null)
                {
                    continue;
                }

                foreach (
                    var groupPromotion in
                        shoppingCartGroup.GroupPromotions.Where(
                            groupPromotion => groupPromotion != null && groupPromotion.UsedGiftProducts != null))
                {
                    promotionGifts.AddRange(groupPromotion.UsedGiftProducts);
                }
            }
            return promotionGifts;
        }

        /// <summary>
        /// 订单的赠品（不包含商品组的赠品）
        /// </summary>
        /// <param name="shoppingCart">购物车对象</param>
        /// <returns>赠品集合</returns>
        /// <remarks>2013-11-29 吴文强 创建</remarks>
        public static IList<CBSpPromotionGift> OrderGroupGifts(this CrShoppingCart shoppingCart)
        {
            var promotionGifts = new List<CBSpPromotionGift>();
            if (shoppingCart.GroupPromotions != null)
            {
                foreach (
                    var groupPromotion in
                        shoppingCart.GroupPromotions.Where(
                            groupPromotion => groupPromotion != null && groupPromotion.UsedGiftProducts != null))
                {
                    promotionGifts.AddRange(groupPromotion.UsedGiftProducts);
                }
            }
            return promotionGifts;
        }

    }
}
