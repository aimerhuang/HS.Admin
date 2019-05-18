using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.BLL.CRM;
using Hyt.BLL.Log;
using Hyt.BLL.Logistics;
using Hyt.DataAccess.Promotion;
using Hyt.Model;
using Hyt.Model.Manual;
using Hyt.Model.WorkflowStatus;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using Hyt.BLL.Product;

namespace Hyt.BLL.Promotion
{
    /// <summary>
    /// 促销引擎
    /// </summary>
    /// <remarks>2013-09-03 吴文强 创建</remarks>
    public class SpPromotionEngineBo : BOBase<SpPromotionEngineBo>
    {
        /// <summary>
        /// 获取商品支持的促销提示信息
        /// </summary>
        /// <param name="platformType">使用平台</param>
        /// <param name="productSysNo">商品系统编号</param>
        /// <param name="containGroup">是否包含组促销信息(true:包含组合,团购组促销;false:默认值,不包含组促销信息)</param>
        /// <returns>促销提示信息集合</returns>
        /// <remarks>2013-09-03 吴文强 创建</remarks>
        public IList<SpPromotionHint> CheckPromotionHints(PromotionStatus.促销使用平台[] platformType, int productSysNo, bool containGroup = false)
        {
            var promotionHints = new List<SpPromotionHint>();

            var promotions = SpPromotionBo.Instance.GetValidPromotions(platformType);

            var shoppingCartItems = new List<CBCrShoppingCartItem>
                {
                    new CBCrShoppingCartItem() {ProductSysNo = productSysNo}
                };
            //商品,分类促销信息
            shoppingCartItems = CheckPromotionProduct(null, promotions, shoppingCartItems).ToList();
            var cbCrShoppingCartItem = shoppingCartItems.FirstOrDefault();
            if (cbCrShoppingCartItem != null && cbCrShoppingCartItem.PromotionHints != null) promotionHints.AddRange(cbCrShoppingCartItem.PromotionHints);

            //组合团购促销信息
            if (containGroup)
            {
                promotionHints.AddRange(CheckGroupPromotionHints(productSysNo));
            }

            return promotionHints;
        }

        /// <summary>
        /// 计算购物车对象(可适合无购物车对象计算,如:前端未登录用户,公共账户,退换货)
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
        /// <param name="warehouseSysNo">发货仓库.</param>
        /// <param name="isExpired">是否过期(是否忽略已使用和过期促销)</param>
        /// <param name="promotionToPython">促销计算元数据</param>
        /// <param name="expensesAmount">太平洋保险</param>
        /// <returns>购物车对象</returns>
        /// <remarks>2013-09-03 吴文强 创建</remarks>
        public CrShoppingCart CalculateShoppingCart(PromotionStatus.促销使用平台[] platformType, CrCustomer customer,
                                                    IList<CBCrShoppingCartItem> shoppingCartItems,
                                                    int[] promotionSysNo, string couponCode = null,
                                                    bool isAutoPromotion = false, int? areaSysNo = null,
                                                    int? deliveryTypeSysNo = null,
                                                    string promotionCode = null, int? warehouseSysNo = null, bool isExpired = true, SpPromotionToPython promotionToPython = null, decimal expensesAmount = 0M)
        {
            var promotions = SpPromotionBo.Instance.GetPromotions(promotionSysNo);

            if (isAutoPromotion)
            {
                promotions.AddRange(SpPromotionBo.Instance.GetValidPromotions(platformType));
            }

            return CalculateShoppingCart(customer, shoppingCartItems, promotions, areaSysNo, deliveryTypeSysNo,
                                         promotionCode, couponCode, warehouseSysNo, isExpired, promotionToPython, expensesAmount);
        }

        /// <summary>
        /// 计算购物车对象
        /// </summary>
        /// <param name="platformType">使用平台</param>
        /// <param name="customer">客户对象</param>
        /// <param name="shoppingCartItems">购物车明细集合</param>
        /// <param name="areaSysNo">区域系统编号</param>
        /// <param name="deliveryTypeSysNo">配送方式系统编号</param>
        /// <param name="promotionCode">促销代码</param>
        /// <param name="couponCode">优惠券代码</param>
        /// <param name="warehouseSysNo">发货仓库.</param>
        /// <returns>购物车对象</returns>
        /// <remarks>2013-09-03 吴文强 创建</remarks>
        public CrShoppingCart CalculateShoppingCart(PromotionStatus.促销使用平台[] platformType, CrCustomer customer, IList<CBCrShoppingCartItem> shoppingCartItems,
                                                    int? areaSysNo, int? deliveryTypeSysNo, string promotionCode,
                                                    string couponCode,int? warehouseSysNo)
        {
            var promotions = SpPromotionBo.Instance.GetValidPromotions(platformType);
            return CalculateShoppingCart(customer, shoppingCartItems, promotions, areaSysNo, deliveryTypeSysNo,
                                         promotionCode, couponCode, warehouseSysNo);
        }

        /// <summary>
        /// 计算购物车对象
        /// </summary>
        /// <param name="customer">客户对象</param>
        /// <param name="shoppingCartItems">购物车明细集合</param>
        /// <param name="promotions">计算促销集合</param>
        /// <param name="areaSysNo">区域系统编号</param>
        /// <param name="deliveryTypeSysNo">配送方式系统编号</param>
        /// <param name="promotionCode">促销代码</param>
        /// <param name="couponCode">优惠券代码</param>
        /// <param name="warehouseSysNo">发货仓库.</param>
        /// <param name="isExpired">是否过期(是否忽略已使用和过期促销)</param>
        /// <param name="promotionToPython">促销计算元数据</param>
        /// <param name="expensesAmount">太平洋保险</param>
        /// <returns>购物车对象</returns>
        /// <remarks>2013-09-03 吴文强 创建</remarks>
        private CrShoppingCart CalculateShoppingCart(CrCustomer customer, IList<CBCrShoppingCartItem> shoppingCartItems,
                                                    List<CBSpPromotion> promotions, int? areaSysNo, int? deliveryTypeSysNo,
                                                    string promotionCode, string couponCode, int? warehouseSysNo, bool isExpired = true, SpPromotionToPython promotionToPython = null, decimal expensesAmount = 0M)
        {
            var cartGiftItems = shoppingCartItems.Where(item => item.ProductSalesType == (int)CustomerStatus.商品销售类型.赠品).ToList();
            shoppingCartItems = shoppingCartItems.Where(item => item.ProductSalesType != (int)CustomerStatus.商品销售类型.赠品).ToList();

            CrShoppingCart shoppingCart;

            //获取促销代码
            if (!string.IsNullOrEmpty(promotionCode))
            {
                var promotion = SpPromotionBo.Instance.GetValidPromotions(new[] { promotionCode });
                promotions.AddRange(promotion);
            }

            var newPromotions = new List<CBSpPromotion>();
            foreach (var promotion in promotions)
            {
                if (newPromotions.All(p => p.SysNo != promotion.SysNo))
                {
                    newPromotions.Add(promotion);
                }
            }
            promotions = newPromotions;

            //检查购物车明细商品支持的促销
            shoppingCartItems = CheckPromotionProduct(customer, promotions, shoppingCartItems, promotionToPython);

            //根据促销创建购物车组并转换为购物车对象
            shoppingCart = CreateShoppingCart(promotions, shoppingCartItems);
            shoppingCart.AllPromotions = promotions;

            //计算购物车商品
            shoppingCart = CalculateCart(customer, shoppingCart, cartGiftItems, promotionToPython);

            //计算组价格
            shoppingCart = CalculateGroupPrice(shoppingCart);

            //计算运费 2016-1-25 王耀发 添加
            shoppingCart = CalculateFreigh(shoppingCart,deliveryTypeSysNo,areaSysNo,warehouseSysNo);

            //计算订单
            shoppingCart = CalculateOrder(customer, shoppingCart, cartGiftItems, promotionToPython);

            #region 计算优惠券

            //计算优惠券
            if (!string.IsNullOrEmpty(couponCode))
            {
                var coupon = PromotionBo.Instance.GetCoupon(couponCode);


                shoppingCart.CouponCode = couponCode;     //解决修改订单时优惠券丢失问题  by ywb 2014-01-03
                //判断促销是否有效
                if (coupon != null && SpCouponEngineBo.Instance.CheckCoupon(customer.SysNo, new[] { coupon }, shoppingCart, isExpired).Count == 1)
                {
                    //有效促销，更新购物车促销金额
                    shoppingCart.CouponCode = couponCode;
                    shoppingCart.CouponAmount = coupon.CouponAmount;
                }
                else
                {
                    shoppingCart.CouponCode = string.Empty;  //解决修改订单时优惠券丢失问题  by ywb 2014-01-03
                    shoppingCart.CouponAmount = 0m;          //解决修改订单时优惠券丢失问题  by ywb 2014-01-03
                }
            }

            #endregion

            //已使用成功的促销码
            if (!string.IsNullOrEmpty(promotionCode))
            {
                var promotion = SpPromotionBo.Instance.GetValidPromotions(new[] { promotionCode }).FirstOrDefault();
                if (promotion != null)
                {
                    foreach (var cbSpPromotion in shoppingCart.GroupPromotions)
                    {
                        if (cbSpPromotion.IsUsed && cbSpPromotion.PromotionSysNo == promotion.SysNo)
                        {
                            shoppingCart.PromotionCode = promotionCode;
                            break;
                        }
                    }
                    foreach (var item in shoppingCart.GetShoppingCartItem())
                    {
                        if (item.UsedPromotions.Split(';').Contains(promotion.SysNo.ToString()))
                        {
                            shoppingCart.PromotionCode = promotionCode;
                            break;
                        }
                    }
                }
            }

            //计算税费 2016-1-25 王耀发 添加
            shoppingCart = CalculateTaxFee(shoppingCart, warehouseSysNo);

            //计算购物车结算金额
            shoppingCart = CalculateSettlementAmount(shoppingCart, expensesAmount);

            return shoppingCart;
        }
        /// <summary>
        /// 计算运费
        /// </summary>
        /// <param name="shoppingCart"></param>
        /// <param name="deliveryTypeSysNo"></param>
        /// <param name="areaSysNo"></param>
        /// <param name="warehouseSysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-01-25 王耀发 创建</remarks>
        private CrShoppingCart CalculateFreigh(CrShoppingCart shoppingCart, int? deliveryTypeSysNo, int? areaSysNo, int? warehouseSysNo)
        {
            if (deliveryTypeSysNo != null && areaSysNo != null && warehouseSysNo != null)
            {
                //获得对应仓库明细
                WhWarehouse wdata = Hyt.BLL.Warehouse.WhWarehouseBo.GetEntity((int)warehouseSysNo);
                //获取仓库物流关联详情
                var warehouseDeliveryType = Hyt.BLL.Warehouse.WhWarehouseBo.Instance.GetWarehouseDeliveryType((int)warehouseSysNo, (int)deliveryTypeSysNo);
                //获得对应商品和数量 王耀发 2016-1-13 创建
                string productSysNoAndNumber = "";
                if (shoppingCart.ShoppingCartGroups != null)
                {
                    foreach (var item in shoppingCart.ShoppingCartGroups)
                    {
                        foreach (var productItem in item.ShoppingCartItems)
                        {
                            if (productSysNoAndNumber == "")
                            {
                                productSysNoAndNumber = productItem.ProductSysNo.ToString() + '_' + productItem.Quantity.ToString();
                            }
                            else
                            {
                                productSysNoAndNumber += "," + productItem.ProductSysNo.ToString() + '_' + productItem.Quantity.ToString();
                            }
                        }
                    }
                    if (warehouseDeliveryType != null && productSysNoAndNumber != "")
                    {
                        var freight = Hyt.BLL.FreightModule.FreightModuleDaoBo.Instance.GetFareTotal((int)areaSysNo, (int)warehouseSysNo, productSysNoAndNumber, (int)deliveryTypeSysNo);
                        if (freight == null)
                        {
                            shoppingCart.FreightAmount = 0;
                        }
                        else
                        {
                            shoppingCart.FreightAmount = (freight.Freigh == -1 ? 0 : freight.Freigh);
                        }
                    }
                    else
                    {
                        shoppingCart.FreightAmount = 0;
                    }
                }
                else
                {
                    shoppingCart.FreightAmount = 0;
                }
            }
            else
            {
                shoppingCart.FreightAmount = 0;
            }
            return shoppingCart;
        }
        /// <summary>
        /// 计算税费
        /// </summary>
        /// <param name="shoppingCart"></param>
        /// <param name="warehouseSysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-01-25 王耀发 创建</remarks>
        private CrShoppingCart CalculateTaxFee(CrShoppingCart shoppingCart, int? warehouseSysNo)
        {
            decimal TaxFee = 0;
            if (warehouseSysNo != null && warehouseSysNo != 0)
            {
                //获得对应仓库明细
                WhWarehouse wdata = Hyt.BLL.Warehouse.WhWarehouseBo.GetEntity((int)warehouseSysNo);
                //订单税费 王耀发 2016-1-13 创建               
                if (shoppingCart.ShoppingCartGroups != null)
                {
                    foreach (var item in shoppingCart.ShoppingCartGroups)
                    {
                        foreach (var productItem in item.ShoppingCartItems)
                        {                    
                            //如果对应仓库类型为保税和直邮，计算税费 王耀发 2016-1-13 创建
                            if (wdata.WarehouseType == (int)Hyt.Model.WorkflowStatus.WarehouseStatus.仓库类型.保税 || wdata.WarehouseType == (int)Hyt.Model.WorkflowStatus.WarehouseStatus.仓库类型.直邮)
                            {
                                PdProduct Product = PdProductBo.Instance.GetProductBySysNo(productItem.ProductSysNo);
                                if (Product != null)
                                {
                                    if (Product.Tax != 0)
                                    {
                                        var SalesAmount = productItem.SalesUnitPrice * productItem.Quantity - productItem.DiscountAmount;
                                        //2016-4-22 王耀发 修改
                                        TaxFee += (SalesAmount * (Product.Tax / 100));
                                    }
                                }
                            }
                        }
                        //2016-4-22 王耀发 修改
                        if (TaxFee != 0)
                        {
                            TaxFee += (shoppingCart.FreightAmount * Convert.ToDecimal(0.119));
                        }
                    }
                }
            }
            shoppingCart.TaxFee = TaxFee;
            return shoppingCart;
        }

        /// <summary>
        /// 检查商品支持的促销规则
        /// </summary>
        /// <param name="customer">客户对象</param>
        /// <param name="promotions">有效促销集合</param>
        /// <param name="shoppingCartItems">购物车明细集合</param>
        /// <param name="promotionToPython">促销计算元数据</param>
        /// <returns>购物车明细集合</returns>
        /// <remarks>2013-09-03 吴文强 创建</remarks>
        private IList<CBCrShoppingCartItem> CheckPromotionProduct(CrCustomer customer, IList<CBSpPromotion> promotions, IList<CBCrShoppingCartItem> shoppingCartItems, SpPromotionToPython promotionToPython = null)
        {
            if (shoppingCartItems == null || shoppingCartItems.Count == 0)
            {
                return new List<CBCrShoppingCartItem>();
            }

            //检查促销支持的商品
            ScriptEngine engine = Python.CreateEngine();
            ScriptScope scope = engine.CreateScope();
            var currentPromotions = promotions.Where(p =>
                p.PromotionType == (int)PromotionStatus.促销应用类型.应用到分类 ||
                p.PromotionType == (int)PromotionStatus.促销应用类型.应用到商品 ||
                p.PromotionType == (int)PromotionStatus.促销应用类型.团购 ||
                p.PromotionType == (int)PromotionStatus.促销应用类型.组合套餐).OrderBy(p => p.PromotionType).ThenByDescending(p => p.Priority);

            foreach (var rule in currentPromotions)
            {
                ScriptSource sourceCode = engine.CreateScriptSourceFromString(rule.PromotionRule.RuleScript);
                scope.SetVariable("CurrSpPromotion", rule);
                scope.SetVariable("CurrCrCustomer", customer);
                scope.SetVariable("CurrAllPromotion", promotions);
                scope.SetVariable("CurrSpPromotionToPython", promotionToPython);
                sourceCode.Execute(scope);
                try
                {
                    var pyCheckPromotionProduct = scope.GetVariable<Func<IList<CBCrShoppingCartItem>, IList<CBCrShoppingCartItem>>>("CheckPromotionProduct");
                    shoppingCartItems = pyCheckPromotionProduct(shoppingCartItems);
                }
                catch (Exception ex)
                {
                    SysLog.Instance.Error(LogStatus.系统日志来源.后台, ex.Message, LogStatus.系统日志目标类型.促销, rule.SysNo, ex);
                }
            }
            return shoppingCartItems;
        }

        /// <summary>
        /// 计算购物车
        /// </summary>
        /// <param name="customer">客户对象</param>
        /// <param name="shoppingCart">购物车对象</param>
        /// <param name="cartGiftItems">赠品明细集合</param>
        /// <param name="promotionToPython">促销计算元数据</param>
        /// <returns>购物车对象</returns>
        /// <remarks>2013-09-03 吴文强 创建</remarks>
        private CrShoppingCart CalculateCart(CrCustomer customer, CrShoppingCart shoppingCart, IList<CBCrShoppingCartItem> cartGiftItems, SpPromotionToPython promotionToPython = null)
        {
            if (shoppingCart.AllPromotions == null)
            {
                return shoppingCart;
            }

            //检查促销支持的商品
            ScriptEngine engine = Python.CreateEngine();
            ScriptScope scope = engine.CreateScope();
            var currentPromotions = shoppingCart.AllPromotions.Where(p =>
               p.PromotionType == (int)PromotionStatus.促销应用类型.应用到分类 ||
               p.PromotionType == (int)PromotionStatus.促销应用类型.应用到商品 ||
               p.PromotionType == (int)PromotionStatus.促销应用类型.团购 ||
               p.PromotionType == (int)PromotionStatus.促销应用类型.组合套餐).OrderBy(p => p.PromotionType).ThenByDescending(p => p.Priority);

            foreach (var rule in currentPromotions)
            {
                ScriptSource sourceCode = engine.CreateScriptSourceFromString(rule.PromotionRule.RuleScript);
                scope.SetVariable("CurrSpPromotion", rule);
                scope.SetVariable("CurrCrCustomer", customer);
                scope.SetVariable("CurrAllPromotion", shoppingCart.AllPromotions);
                scope.SetVariable("CurrCartGiftItems", cartGiftItems);
                scope.SetVariable("CurrSpPromotionToPython", promotionToPython);

                //if (rule.PromotionType == (int)PromotionStatus.促销应用类型.团购)
                //{
                //   GsGroupShopping groupShopping= GroupShoppingBo.Instance.Get()
                //    scope.SetVariable("CurrGroupShopping", );
                //}
                //if (rule.PromotionType == (int)PromotionStatus.促销应用类型.组合套餐)
                //{
                //    scope.SetVariable("CurrCombo", cartGiftItems);
                //}

                sourceCode.Execute(scope);
                try
                {
                    var pyCalculateCart = scope.GetVariable<Func<CrShoppingCart, CrShoppingCart>>("CalculateCart");
                    shoppingCart = pyCalculateCart(shoppingCart);
                }
                catch (Exception ex)
                {
                    SysLog.Instance.Error(LogStatus.系统日志来源.后台, ex.Message, LogStatus.系统日志目标类型.促销, rule.SysNo, ex);
                }
            }

            return shoppingCart;
        }

        /// <summary>
        /// 计算订单
        /// </summary>
        /// <param name="customer">客户对象</param>
        /// <param name="shoppingCart">购物车对象</param>
        /// <param name="cartGiftItems">赠品明细集合</param>
        /// <param name="promotionToPython">促销计算元数据</param>
        /// <returns>购物车对象</returns>
        /// <remarks>2013-09-03 吴文强 创建</remarks>
        private CrShoppingCart CalculateOrder(CrCustomer customer, CrShoppingCart shoppingCart, IList<CBCrShoppingCartItem> cartGiftItems, SpPromotionToPython promotionToPython = null)
        {
            if (shoppingCart.AllPromotions == null)
            {
                return shoppingCart;
            }

            //检查促销支持的商品
            ScriptEngine engine = Python.CreateEngine();
            ScriptScope scope = engine.CreateScope();

            var currentPromotions = shoppingCart.AllPromotions.Where(p =>
               p.PromotionType == (int)PromotionStatus.促销应用类型.应用到商品合计 ||
               p.PromotionType == (int)PromotionStatus.促销应用类型.应用到订单合计 ||
               p.PromotionType == (int)PromotionStatus.促销应用类型.应用到运费).OrderBy(p => p.PromotionType).ThenByDescending(p => p.Priority);

            //检查支持的促销
            foreach (var rule in currentPromotions)
            {
                ScriptSource sourceCode = engine.CreateScriptSourceFromString(rule.PromotionRule.RuleScript);
                scope.SetVariable("CurrSpPromotion", rule);
                scope.SetVariable("CurrCrCustomer", customer);
                scope.SetVariable("CurrAllPromotion", shoppingCart.AllPromotions);
                scope.SetVariable("CurrCartGiftItems", cartGiftItems);
                scope.SetVariable("CurrSpPromotionToPython", promotionToPython);
                sourceCode.Execute(scope);
                try
                {
                    var pyCalculateCart = scope.GetVariable<Func<CrShoppingCart, CrShoppingCart>>("CheckPromotionProduct");
                    shoppingCart = pyCalculateCart(shoppingCart);
                }
                catch (Exception ex)
                {
                    SysLog.Instance.Error(LogStatus.系统日志来源.后台, ex.Message, LogStatus.系统日志目标类型.促销, rule.SysNo, ex);
                }
            }

            //计算促销
            foreach (var rule in currentPromotions)
            {
                ScriptSource sourceCode = engine.CreateScriptSourceFromString(rule.PromotionRule.RuleScript);
                scope.SetVariable("CurrSpPromotion", rule);
                scope.SetVariable("CurrCrCustomer", customer);
                scope.SetVariable("CurrAllPromotion", shoppingCart.AllPromotions);
                scope.SetVariable("CurrSpPromotionToPython", promotionToPython);
                sourceCode.Execute(scope);
                try
                {
                    var pyCalculateCart = scope.GetVariable<Func<CrShoppingCart, CrShoppingCart>>("CalculateCart");
                    shoppingCart = pyCalculateCart(shoppingCart);
                }
                catch (Exception ex)
                {
                    SysLog.Instance.Error(LogStatus.系统日志来源.后台, ex.Message, LogStatus.系统日志目标类型.促销, rule.SysNo, ex);
                }
            }

            #region 2013-12-31 吴文强 添加赠品加购计算
            //计算赠品加购价
            var sumGpPurchasePrice = shoppingCart.GroupPromotions == null ? 0 :
                shoppingCart.GroupPromotions.Where(gp => gp.UsedGiftProducts != null)
                            .Sum(gp => gp.UsedGiftProducts.Sum(ugp => ugp.PurchasePrice));

            //2013-12-31 吴文强 计算订单赠品加购价添加至商品合计金额中（促销商品合计需排除加购价的商品金额）
            //计算订单赠品加购价
            shoppingCart.ProductAmount += sumGpPurchasePrice;

            //计算购物车结算金额
            shoppingCart = CalculateSettlementAmount(shoppingCart);
            #endregion

            return shoppingCart;
        }

        /// <summary>
        /// 计算购物车结算金额
        /// </summary>
        /// <param name="shoppingCart">购物车对象</param>
        /// <param name="expensesAmount">太平洋保险</param>
        /// <returns>购物车对象</returns>
        /// <remarks>2013-09-03 吴文强 创建</remarks>
        private CrShoppingCart CalculateSettlementAmount(CrShoppingCart shoppingCart, decimal expensesAmount = 0M)
        {
            var productAmount = shoppingCart.ProductAmount - shoppingCart.ProductDiscountAmount -
                                shoppingCart.SettlementDiscountAmount;
            var freightAmount = shoppingCart.FreightAmount - shoppingCart.FreightDiscountAmount;
            var couponAmount = shoppingCart.CouponAmount;
            //2016-4-22 王耀发 修改  
            var taxfee = shoppingCart.TaxFee;
            //if (taxfee <= 50)
            //{
            //    taxfee = 0;
            //}
            //商品金额>优惠券金额
            if (productAmount > couponAmount)
            {
                //商品金额=商品金额-优惠券金额
                productAmount = productAmount - couponAmount;
            }
            else
            {
                //优惠券金额=商品金额，商品金额=0
                couponAmount = productAmount;
                productAmount = 0;
            }

            shoppingCart.CouponAmount = couponAmount;

            //结算金额=商品金额-优惠券金额+运费（优惠券不能抵消运费）+ 税费+太平洋保险
            shoppingCart.SettlementAmount = productAmount + freightAmount + taxfee + expensesAmount;

            return shoppingCart;
        }

        /// <summary>
        /// 根据促销创建购物车商品组并构建购物车对象
        /// </summary>
        /// <param name="promotions">有效促销集合</param>
        /// <param name="shoppingCartItems">购物车明细集合</param>
        /// <returns>购物车对象</returns>
        /// <remarks>2013-09-03 吴文强 创建</remarks>
        private CrShoppingCart CreateShoppingCart(List<CBSpPromotion> promotions, IList<CBCrShoppingCartItem> shoppingCartItems)
        {
            //构建购物车对象
            var newShoppingCart = new CrShoppingCart();
            newShoppingCart.ShoppingCartGroups = new List<CrShoppingCartGroup>();

            //按照使用促销最多的商品倒序排列
            var tempShoppingCarts =
                shoppingCartItems.OrderByDescending(sc =>
                    (sc.Promotions ?? string.Empty).Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries).Count());

            var noPromotionsGroup = new CrShoppingCartGroup();
            noPromotionsGroup.ShoppingCartItems = new List<CBCrShoppingCartItem>();

            //遍历购物车商品
            foreach (var shoppingCartItem in tempShoppingCarts)
            {
                if (string.IsNullOrEmpty(shoppingCartItem.Promotions))
                {
                    noPromotionsGroup.ShoppingCartItems.Add(shoppingCartItem);
                    continue;
                }

                //当前商品可使用的促销
                var scPromotions =
                    Array.ConvertAll(
                        (shoppingCartItem.Promotions ?? string.Empty).Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries),
                        int.Parse);

                CrShoppingCartGroup scg = null;
                if (shoppingCartItem.IsLock == 1)
                {
                    //查询已锁定的促销组
                    scg =
                        newShoppingCart.ShoppingCartGroups.FirstOrDefault(
                            g => g.IsLock &&
                            g.Promotions == shoppingCartItem.Promotions &&
                            g.GroupCode == shoppingCartItem.GroupCode);
                }
                else
                {
                    //查询不为锁定的促销组
                    scg =
                        newShoppingCart.ShoppingCartGroups.FirstOrDefault(
                            g => !g.IsLock &&
                                 g.GroupPromotions.Any(
                                     scgp => scPromotions.Any(s => s == scgp.PromotionSysNo)));
                }

                if (scg != null)
                {
                    //存在促销组则添加当前商品到促销组
                    foreach (var s in scPromotions)
                    {
                        if (scg.GroupPromotions.FirstOrDefault(scgp => scgp.PromotionSysNo == s) == null)
                        {
                            var promotion = promotions.FirstOrDefault(p => p.SysNo == s);
                            if (promotion != null)
                                scg.GroupPromotions.Add(new CrShoppingCartGroupPromotion()
                                    {
                                        PromotionSysNo = s,
                                        RuleType = promotion.PromotionRule.RuleType
                                    });
                        }
                    }

                    scg.ShoppingCartItems.Add(shoppingCartItem);
                }
                else
                {
                    //不存在促销组则新建促销组
                    newShoppingCart.ShoppingCartGroups.Add(new CrShoppingCartGroup()
                    {
                        IsLock = (shoppingCartItem.IsLock == (int)CustomerStatus.购物车是否锁定.是),
                        Promotions = shoppingCartItem.Promotions,
                        GroupCode = shoppingCartItem.GroupCode,
                        ShoppingCartItems = new List<CBCrShoppingCartItem>() { shoppingCartItem },
                        GroupPromotions = scPromotions.Select(s =>
                            {
                                var promotion = promotions.FirstOrDefault(p => p.SysNo == s);
                                return promotion != null ? new CrShoppingCartGroupPromotion() { PromotionSysNo = s, RuleType = promotion.PromotionRule.RuleType } : null;
                            }).ToList()
                    });
                }
            }

            //如果无促销商品中存在数据，则将该组添加到购物车
            if (noPromotionsGroup.ShoppingCartItems.Count > 0)
            {
                newShoppingCart.ShoppingCartGroups.Add(noPromotionsGroup);
            }

            return newShoppingCart;
        }

        /// <summary>
        /// 计算组价格
        /// </summary>
        /// <param name="shoppingCart">购物车对象</param>
        /// <returns>购物车对象</returns>
        /// <remarks>2013-09-03 吴文强 创建</remarks>
        private CrShoppingCart CalculateGroupPrice(CrShoppingCart shoppingCart)
        {
            foreach (var scg in shoppingCart.ShoppingCartGroups)
            {
                scg.DiscountAmount = scg.ShoppingCartItems.Where(item => item.IsChecked == (int)CustomerStatus.是否选中.是).Sum(a => a.DiscountAmount);
                scg.TotalAmount = scg.ShoppingCartItems.Where(item => item.IsChecked == (int)CustomerStatus.是否选中.是).Sum(a => a.SaleTotalAmount);
                //计算赠品加购价
                var sumGpPurchasePrice = scg.GroupPromotions == null ? 0 :
                    scg.GroupPromotions.Where(gp => gp != null && gp.UsedGiftProducts != null)
                                .Sum(gp => gp.UsedGiftProducts.Sum(ugp => ugp.PurchasePrice));
                scg.TotalAmount += sumGpPurchasePrice;
            }

            shoppingCart.ProductDiscountAmount = shoppingCart.ShoppingCartGroups.Sum(a => a.DiscountAmount);
            shoppingCart.ProductAmount = shoppingCart.ShoppingCartGroups.Sum(a => a.TotalAmount);

            //未使用促销的商品排在前面显示
            shoppingCart.ShoppingCartGroups =
                shoppingCart.ShoppingCartGroups.OrderBy(scg => (scg.GroupPromotions ?? new List<CrShoppingCartGroupPromotion>()).Count()).ToList();

            return shoppingCart;
        }

        /// <summary>
        /// 检查组合套餐和团购促销提示
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <returns>组促销提示</returns>
        /// <remarks>2013-09-03 吴文强 创建</remarks>
        private IList<SpPromotionHint> CheckGroupPromotionHints(int productSysNo)
        {
            var promotionHints = new List<SpPromotionHint>();

            //检查是否有优惠套餐
            var combo = ISpComboDao.Instance.GetComboByMasterProductSysNo(productSysNo);
            if (combo != null && combo.Count > 0)
            {
                var promotionHint = new SpPromotionHint
                    {
                        FrontText = "套餐",
                        PromotionSysNo = combo[0].PromotionSysNo,
                        RuleType = (int)PromotionStatus.促销规则类型.组合,
                        SourceSysNo = combo[0].SysNo,
                    };
                promotionHints.Add(promotionHint);
            }

            //检查是否有团购
            var groupShopping = IGsGroupShoppingDao.Instance.GetGroupShoppingByProductSysNo(productSysNo);
            if (groupShopping != null && groupShopping.Count > 0)
            {
                var promotionHint = new SpPromotionHint
                {
                    FrontText = "团购",
                    PromotionSysNo = groupShopping[0].PromotionSysNo,
                    RuleType = (int)PromotionStatus.促销规则类型.团购,
                    SourceSysNo = groupShopping[0].SysNo,
                };
                promotionHints.Add(promotionHint);
            }
            return promotionHints;
        }
    }
}
