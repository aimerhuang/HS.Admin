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
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.CRM
{
    /// <summary>
    /// 购物车业务
    /// </summary>
    /// <remarks>2013-08-13 吴文强 创建</remarks>
    public class CrShoppingCartBo : BOBase<CrShoppingCartBo>
    {
        #region 添加/移除/更新购物车商品

        /// <summary>
        /// 添加商品至购物车
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="productSysNo">商品系统编号</param>
        /// <param name="quantity">商品数量</param>
        /// <param name="source">购物车商品来源</param>
        /// <returns></returns>
        /// <remarks>2013-08-13 吴文强 创建</remarks>
        public void Add(int customerSysNo, int productSysNo, int quantity, CustomerStatus.购物车商品来源 source)
        {
            var productPrices = PdPriceBo.Instance.GetProductPrice(productSysNo,
                                                             new ProductStatus.产品价格来源[] { ProductStatus.产品价格来源.会员等级价 });
            var customer = CrCustomerBo.Instance.GetModel(customerSysNo);
            var firstOrDefault = productPrices.FirstOrDefault(p => customer != null && p.SourceSysNo == customer.LevelSysNo);
            if (firstOrDefault != null)
            {
                var shoppingCartItem = new CrShoppingCartItem
                    {
                        IsChecked = (int)CustomerStatus.是否选中.是,
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
                var list = new List<CrShoppingCartItem>();
                list.Add(shoppingCartItem);
                ICrShoppingCartItemDao.Instance.Add(list);
            }
        }

        /// <summary>
        /// 将组促销转换为购物车明细
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="groupSysNo">组系统编号(组合,团购主表系统编号)</param>
        /// <param name="quantity">组数量</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <param name="source">购物车商品来源</param>
        /// <returns></returns>
        /// <remarks>2013-09-01 吴文强 创建</remarks>
        public void Add(int customerSysNo, int groupSysNo, int quantity, int promotionSysNo, CustomerStatus.购物车商品来源 source)
        {
            ICrShoppingCartItemDao.Instance.Add(InitGroupToCartItem(customerSysNo, groupSysNo, quantity, promotionSysNo,
                                                                    source).ToList());
        }

        /// <summary>
        /// 添加促销赠品至购物车
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="productSysNo">商品(赠品)系统编号</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <param name="source">购物车商品来源</param>
        /// <returns></returns>
        /// <remarks>2013-08-13 吴文强 创建</remarks>
        public void AddGift(int customerSysNo, int productSysNo, int promotionSysNo, CustomerStatus.购物车商品来源 source)
        {
            var shoppingCartItem = new CrShoppingCartItem
            {
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
                Promotions = promotionSysNo.ToString(),
                UsedPromotions = promotionSysNo.ToString(),
            };
            var list = new List<CrShoppingCartItem>();
            list.Add(shoppingCartItem);
            ICrShoppingCartItemDao.Instance.Add(list);
        }

        /// <summary>
        /// 删除购物车明细
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="sysNo">购物车明细系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-13 吴文强 创建</remarks>
        public void Remove(int customerSysNo, int[] sysNo)
        {
            if (sysNo.Count() != 0)
            {
                ICrShoppingCartItemDao.Instance.Delete(customerSysNo, sysNo);
            }
        }

        /// <summary>
        /// 删除购物车明细
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="sysNo">购物车商品系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-13 吴文强 创建</remarks>
        public void RemoveByProductSysNo(int customerSysNo, int[] sysNo)
        {
            if (sysNo.Count() != 0)
            {
                ICrShoppingCartItemDao.Instance.DeleteByProductSysNo(customerSysNo, sysNo);
            }
        }

        /// <summary>
        /// 删除购物车选中的明细
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-09-24 吴文强 创建</remarks>
        public void RemoveCheckedItem(int customerSysNo)
        {
            ICrShoppingCartItemDao.Instance.Delete(customerSysNo, true);
        }

        /// <summary>
        /// 删除购物车所有明细
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-09-24 吴文强 创建</remarks>
        public void RemoveAll(int customerSysNo)
        {
            ICrShoppingCartItemDao.Instance.Delete(customerSysNo, false);
        }

        /// <summary>
        /// 删除购物车组商品
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="groupCode">组代码</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-13 吴文强 创建</remarks>
        public void Remove(int customerSysNo, string groupCode, string promotionSysNo)
        {
            ICrShoppingCartItemDao.Instance.Delete(customerSysNo, groupCode, promotionSysNo);
        }

        /// <summary>
        /// 删除促销赠品
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="productSysNo">商品(赠品)系统编号</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-13 吴文强 创建</remarks>
        public void RemoveGift(int customerSysNo, int productSysNo, int promotionSysNo)
        {
            ICrShoppingCartItemDao.Instance.Delete(customerSysNo, productSysNo, promotionSysNo);
        }

        /// <summary>
        /// 更新购物车明细商品数量
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="sysNo">购物车明细系统编号集合</param>
        /// <param name="quantity">商品数量</param>
        /// <returns></returns>
        /// <remarks>2013-08-13 吴文强 创建</remarks>
        public void UpdateQuantity(int customerSysNo, int[] sysNo, int quantity)
        {
            ICrShoppingCartItemDao.Instance.UpdateQuantity(customerSysNo, sysNo, quantity);
        }

        /// <summary>
        /// 更新购物车组组商品数量
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="groupCode">组代码</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <param name="quantity">商品数量</param>
        /// <returns></returns>
        /// <remarks>2013-08-13 吴文强 创建</remarks>
        public void UpdateQuantity(int customerSysNo, string groupCode, string promotionSysNo, int quantity)
        {
            ICrShoppingCartItemDao.Instance.UpdateQuantity(customerSysNo, groupCode, promotionSysNo, quantity);
        }
        #endregion

        #region 选中/取消选中购物车明细项目

        /// <summary>
        /// 选择购物车所有明细项目
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-09-23 吴文强 创建</remarks>
        public void CheckedAll(int customerSysNo)
        {
            ICrShoppingCartItemDao.Instance.UpdateCheckedItem(customerSysNo, CustomerStatus.是否选中.是);
        }

        /// <summary>
        /// 取消选择购物车所有明细项目
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-09-23 吴文强 创建</remarks>
        public void UncheckedAll(int customerSysNo)
        {
            ICrShoppingCartItemDao.Instance.UpdateCheckedItem(customerSysNo, CustomerStatus.是否选中.否);
        }

        /// <summary>
        /// 选择购物车明细项目
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="itemSysNo">购物车明细系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-27 吴文强 创建</remarks>
        public void CheckedItem(int customerSysNo, int[] itemSysNo)
        {
            ICrShoppingCartItemDao.Instance.UpdateCheckedItem(customerSysNo, itemSysNo, CustomerStatus.是否选中.是);
        }

        /// <summary>
        /// 取消选择购物车明细项目
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="itemSysNo">购物车明细系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-27 吴文强 创建</remarks>
        public void UncheckedItem(int customerSysNo, int[] itemSysNo)
        {
            ICrShoppingCartItemDao.Instance.UpdateCheckedItem(customerSysNo, itemSysNo, CustomerStatus.是否选中.否);
        }

        /// <summary>
        /// 选择购物车组明细项目
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="groupCode">组代码</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-27 吴文强 创建</remarks>
        public void CheckedItem(int customerSysNo, string groupCode, string promotionSysNo)
        {
            ICrShoppingCartItemDao.Instance.UpdateCheckedItem(customerSysNo, groupCode, promotionSysNo, CustomerStatus.是否选中.是);
        }

        /// <summary>
        /// 取消选择购物车组明细项目
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="groupCode">组代码</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-27 吴文强 创建</remarks>
        public void UncheckedItem(int customerSysNo, string groupCode, string promotionSysNo)
        {
            ICrShoppingCartItemDao.Instance.UpdateCheckedItem(customerSysNo, groupCode, promotionSysNo, CustomerStatus.是否选中.否);
        }

        #endregion

        #region 获取购物车对象

        /// <summary>
        /// 获取购物车对象
        /// </summary>
        /// <param name="platformType">使用平台</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="isChecked">true:只查询选中的明细;false:查询全部,默认false</param>
        /// <param name="isFrontProduct">是否只允许前台下单的商品（默认：true）</param>
        /// <returns>购物车对象</returns>
        /// <remarks>2013-08-12 吴文强 创建</remarks>
        public CrShoppingCart GetShoppingCart(PromotionStatus.促销使用平台[] platformType, int customerSysNo, bool isChecked = false, bool isFrontProduct = true)
        {
            var shoppingCartItems = this.GetShoppingCartItems(customerSysNo, null, isChecked);
            return GetShoppingCart(platformType, customerSysNo, shoppingCartItems, null, null, null, null, isFrontProduct);
        }

        /// <summary>
        /// 获取购物车对象
        /// </summary>
        /// <param name="platformType">使用平台</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="sysNo">购物车明细系统编号(null:购物车为服务器选中的对象)</param>
        /// <param name="areaSysNo">区域系统编号</param>
        /// <param name="deliveryTypeSysNo">配送方式系统编号</param>
        /// <param name="promotionCode">促销代码</param>
        /// <param name="couponCode">优惠券代码</param>
        /// <param name="isChecked">true:只查询选中的明细;false:查询全部,默认false</param>
        /// <param name="isFrontProduct">是否只允许前台下单的商品（默认：true）</param>
        /// <param name="warehouseSysNo">发货仓库.</param>
        /// <param name="dealerSysNo">经销商系统编号</param>
        /// <returns>购物车对象</returns>
        /// <remarks>2013-08-12 吴文强 创建</remarks>
        public CrShoppingCart GetShoppingCart(PromotionStatus.促销使用平台[] platformType, int customerSysNo, int[] sysNo, int? areaSysNo, int? deliveryTypeSysNo, string promotionCode, string couponCode, bool isChecked = false, bool isFrontProduct = true, int? warehouseSysNo = null,int dealerSysNo=0)
        {
            var shoppingCartItems = this.GetShoppingCartItems(customerSysNo, sysNo, isChecked);
            var shoppingCartGiftItems = ICrShoppingCartItemDao.Instance.GetShoppingCartGiftItems(customerSysNo);
            foreach (var shoppingCartGiftItem in shoppingCartGiftItems)
            {
                if (shoppingCartItems.FirstOrDefault(sci => sci.SysNo == shoppingCartGiftItem.SysNo) == null)
                {
                    shoppingCartItems.Add(shoppingCartGiftItem);
                }
            }
            return GetShoppingCart(platformType, customerSysNo, shoppingCartItems, areaSysNo, deliveryTypeSysNo, promotionCode, couponCode, isFrontProduct, warehouseSysNo, dealerSysNo);
        }

        /// <summary>
        /// 获取购物车对象(可适合无购物车对象计算,如:前端未登录用户,公共账户,退换货)
        /// </summary>
        /// <param name="platformType">使用平台</param>
        /// <param name="customer">客户对象</param>
        /// <param name="shoppingCartItems">购物车明细集合</param>
        /// <param name="promotionSysNo">促销系统编号集合</param>
        /// <param name="couponCode">优惠券代码</param>
        /// <param name="isAutoPromotion">是否自动使用当前有效促销</param>
        /// <param name="areaSysNo">区域系统编号</param>
        /// <param name="deliveryTypeSysNo">配送方式系统编号</param>
        /// <param name="promotionCode">促销代码</param>
        /// <param name="isFrontProduct">是否只允许前台下单的商品（默认：true）</param>
        /// <param name="promotionToPython">促销计算元数据</param>
        /// <param name="warehouseSysNo">发货仓库.</param>
        /// <param name="expensesAmount">太平洋保险</param>
        /// <returns>购物车对象</returns>
        /// <remarks>2013-08-12 吴文强 创建</remarks>
        public CrShoppingCart GetShoppingCart(PromotionStatus.促销使用平台[] platformType, CrCustomer customer,
                                              IList<CBCrShoppingCartItem> shoppingCartItems,
                                              int[] promotionSysNo, string couponCode = null,
                                              bool isAutoPromotion = false, int? areaSysNo = null,
                                              int? deliveryTypeSysNo = null, string promotionCode = null, bool isFrontProduct = true, SpPromotionToPython promotionToPython = null, int? warehouseSysNo = null, decimal expensesAmount = 0M)
        {
            //移除下架的商品
            shoppingCartItems = CrShoppingCartConvertBo.Instance.RemoveSoldoutProduct(shoppingCartItems, customer.SysNo, isFrontProduct);

            var shoppingCart = SpPromotionEngineBo.Instance.CalculateShoppingCart(platformType, customer,
                                                                                  shoppingCartItems,
                                                                                  promotionSysNo, couponCode,
                                                                                  isAutoPromotion, areaSysNo,
                                                                                  deliveryTypeSysNo,
                                                                                  promotionCode, warehouseSysNo, true, promotionToPython, expensesAmount);
            //移除前台不能下单的赠品
            shoppingCart = CrShoppingCartConvertBo.Instance.RemoveSoldoutGift(shoppingCart, isFrontProduct);

            return shoppingCart;
        }

        #endregion

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
        /// <param name="warehouseSysNo">发货仓库.</param>
        /// <param name="dealerSysNo">经销商系统编号</param>
        /// <returns>购物车</returns>
        /// <remarks>2013-08-31 吴文强 创建</remarks>
        private CrShoppingCart GetShoppingCart(PromotionStatus.促销使用平台[] platformType, int customerSysNo, IList<CBCrShoppingCartItem> shoppingCartItems, int? areaSysNo, int? deliveryTypeSysNo, string promotionCode, string couponCode, bool isFrontProduct = true, int? warehouseSysNo = null, int dealerSysNo=0)
        {
            var customer = CrCustomerBo.Instance.GetCrCustomerItem(customerSysNo);

            if (customer == null || shoppingCartItems == null)
            {
                return new CrShoppingCart();
            }

            //移除下架的商品
            shoppingCartItems = CrShoppingCartConvertBo.Instance.RemoveSoldoutProduct(shoppingCartItems, customerSysNo, isFrontProduct);

            //重置购物车明细商品
            shoppingCartItems = ResetItemByPromotion(platformType, shoppingCartItems);

            //购物车明细
            shoppingCartItems = InitProductPrice(customer.LevelSysNo, shoppingCartItems, (platformType.Length > 0 ? (int)platformType[0] : 0), dealerSysNo);

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
                                       && usedGiftProduct.PromotionSysNo.ToString().Equals(sci.UsedPromotions)).SysNo);
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
                                sci => sci.ProductSysNo.Equals(usedGiftProduct.ProductSysNo) && sci.UsedPromotions != null
                                       && usedGiftProduct.PromotionSysNo.ToString().Equals(sci.UsedPromotions)).SysNo);
                    }
                }
            }

            //移除无效购物车赠品
            ICrShoppingCartItemDao.Instance.RemoveInvalidGift(customerSysNo, containGifts.ToArray());

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
        /// 初始化购物车商品价格
        /// </summary>
        /// <param name="customerLevel">客户等级</param>
        /// <param name="shoppingCartItems">购物车明细集合</param>
        /// <param name="platformType">平台类型</param>
        /// <param name="dealerSysNo">经销商系统编号</param>
        /// <returns>已添加价格的购物车明细集合</returns>
        /// <remarks>
        /// 2013-08-12 吴文强 创建
        /// 2016-7-12 杨浩 添加经销商系统编号
        /// </remarks>
        public IList<CBCrShoppingCartItem> InitProductPrice(int customerLevel, IList<CBCrShoppingCartItem> shoppingCartItems, int platformType = 0, int dealerSysNo=0)
        {
            foreach (var crShoppingCart in shoppingCartItems)
            {
                var product = PdProductBo.Instance.GetProduct(crShoppingCart.ProductSysNo);
                var basePrice = product.PdPrice.Value.FirstOrDefault(
                    p => p.PriceSource == (int)ProductStatus.产品价格来源.基础价格);

                decimal price;

               
                if (platformType ==(int)PromotionStatus.促销使用平台.PC商城)
                {
                   var levelPrice = product.PdPrice.Value.FirstOrDefault(
                 p => p.PriceSource == (int)ProductStatus.产品价格来源.会员等级价
                         && p.SourceSysNo == customerLevel);
                   price = (levelPrice == null ? basePrice.Price : levelPrice.Price);

                   if (dealerSysNo>0)
                   {              
                      price=Hyt.BLL.Stores.DsSpecialPriceBo.Instance.GetSpecialPricesBySysNo(dealerSysNo, crShoppingCart.ProductSysNo);
                   }
                }
                else//(int)PromotionStatus.促销使用平台.门店)
                {
                  
                    var specialPrices=Hyt.BLL.Stores.DsSpecialPriceBo.Instance.GetSpecialPricesBySysNo(dealerSysNo);
                    var specialInfo=specialPrices.Where(x => x.ProductSysNo == crShoppingCart.ProductSysNo).FirstOrDefault();
                    if (specialInfo != null && specialInfo.ShopPrice > 0)
                    {
                        price = specialInfo.ShopPrice;
                    }
                    else
                    {
                       var levelPrice = product.PdPrice.Value.FirstOrDefault(
       p => p.PriceSource == (int)ProductStatus.产品价格来源.门店销售价
               && p.SourceSysNo == 0);
                       price = (levelPrice == null ? basePrice.Price : levelPrice.Price);
                    }
            
                   
                }
             
             

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
                            if (comboItem != null)
                            {
                                crShoppingCart.OriginPrice = price;
                                crShoppingCart.SalesUnitPrice = price - comboItem.DiscountAmount;
                                crShoppingCart.SaleTotalAmount = crShoppingCart.SalesUnitPrice * crShoppingCart.Quantity;
                                crShoppingCart.DiscountAmount = 0;
                            }
                            

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
        /// 初始化购物车商品价格
        /// </summary>
        /// <param name="customerLevel">客户等级</param>
        /// <param name="shoppingCartItems">购物车明细集合</param>
        /// <param name="platformType">平台类型</param>
        /// <param name="dealerSysNo">经销商系统编号</param>
        /// <returns>已添加价格的购物车明细集合</returns>
        /// <remarks>
        /// 2013-08-12 吴文强 创建
        /// 2016-7-12 杨浩 添加经销商系统编号
        /// 
        ///  2017-6-7  添加单价修改
        /// </remarks>
        public IList<CBCrShoppingCartItem> InitProductPriceNew(int customerLevel, IList<CBCrShoppingCartItem> shoppingCartItems, int platformType = 0, int dealerSysNo = 0)
        {
            foreach (var crShoppingCart in shoppingCartItems)
            {
                var product = PdProductBo.Instance.GetProduct(crShoppingCart.ProductSysNo);
                var basePrice = product.PdPrice.Value.FirstOrDefault(
                    p => p.PriceSource == (int)ProductStatus.产品价格来源.基础价格);

                decimal price;


                if (platformType == (int)PromotionStatus.促销使用平台.PC商城)
                {
                    var levelPrice = product.PdPrice.Value.FirstOrDefault(
                  p => p.PriceSource == (int)ProductStatus.产品价格来源.会员等级价
                          && p.SourceSysNo == customerLevel);
                    price = (levelPrice == null ? basePrice.Price : levelPrice.Price);

                    if (dealerSysNo > 0)
                    {
                        price = Hyt.BLL.Stores.DsSpecialPriceBo.Instance.GetSpecialPricesBySysNo(dealerSysNo, crShoppingCart.ProductSysNo);
                    }
                }
                else//(int)PromotionStatus.促销使用平台.门店)
                {

                    var specialPrices = Hyt.BLL.Stores.DsSpecialPriceBo.Instance.GetSpecialPricesBySysNo(dealerSysNo);
                    var specialInfo = specialPrices.Where(x => x.ProductSysNo == crShoppingCart.ProductSysNo).FirstOrDefault();
                    if (specialInfo != null && specialInfo.ShopPrice > 0)
                    {
                        price = specialInfo.ShopPrice;
                    }
                    else
                    {
                        var levelPrice = product.PdPrice.Value.FirstOrDefault(
        p => p.PriceSource == (int)ProductStatus.产品价格来源.门店销售价
                && p.SourceSysNo == 0);
                        price = (levelPrice == null ? basePrice.Price : levelPrice.Price);
                    }


                }



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
                            if (comboItem != null)
                            {
                                crShoppingCart.OriginPrice = price;
                                crShoppingCart.SalesUnitPrice = price - comboItem.DiscountAmount;
                                crShoppingCart.SaleTotalAmount = crShoppingCart.SalesUnitPrice * crShoppingCart.Quantity;
                                crShoppingCart.DiscountAmount = 0;
                            }


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
        /// 将组促销转换为购物车明细
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="groupSysNo">组系统编号(组合,团购主表系统编号)</param>
        /// <param name="quantity">组数量</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <param name="source">购物车商品来源</param>
        /// <returns>购物车明细集合</returns>
        /// <remarks>2013-09-01 吴文强 创建</remarks>
        private IList<CrShoppingCartItem> InitGroupToCartItem(int customerSysNo, int groupSysNo, int quantity,
                                                                int promotionSysNo, CustomerStatus.购物车商品来源 source)
        {
            var result = new List<CrShoppingCartItem>();
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
                    result.AddRange(comboItems.Select(item => new CrShoppingCartItem
                    {
                        ProductName = item.ProductName,
                        CustomerSysNo = customerSysNo,
                        ProductSysNo = item.ProductSysNo,
                        Quantity = quantity,
                        OriginPrice = 0,
                        IsChecked = (int)CustomerStatus.是否选中.是,
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
                    result.AddRange(groupShoppingItem.Select(item => new CrShoppingCartItem
                        {
                            ProductName = item.ProductName,
                            CustomerSysNo = customerSysNo,
                            ProductSysNo = item.ProductSysNo,
                            Quantity = quantity,
                            OriginPrice = 0,
                            IsChecked = (int)CustomerStatus.是否选中.是,
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

        /// <summary>
        /// 获取购物车明细
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="sysNo">购物车明细系统编号</param>
        /// <param name="isChecked">true:只查询选中的明细;false:查询全部,默认查询全部</param>
        /// <returns>购物车明细</returns>
        /// <remarks>2013-08-13 吴文强 创建</remarks>
        private IList<CBCrShoppingCartItem> GetShoppingCartItems(int customerSysNo, int[] sysNo, bool isChecked = false)
        {
            var shoppingCartItems = ICrShoppingCartItemDao.Instance.GetShoppingCartItems(customerSysNo, null, isChecked);
            foreach (var item in shoppingCartItems)
            {
                item.GetThumbnail = () => Web.ProductImageBo.Instance.GetProductImagePath(Web.ProductThumbnailType.Image180, item.ProductSysNo);
            }
            return shoppingCartItems;
        }
        #endregion
    }
}
