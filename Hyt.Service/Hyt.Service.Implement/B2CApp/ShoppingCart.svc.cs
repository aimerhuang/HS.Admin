using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using System.Transactions;
using Hyt.BLL.CRM;
using Hyt.BLL.Promotion;
using Hyt.Model;
using Hyt.Model.B2CApp;
using Hyt.Model.SystemPredefined;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Service.Contract.B2CApp;
using DeliveryType = Hyt.Model.B2CApp.DeliveryType;
using PaymentType = Hyt.Model.B2CApp.PaymentType;

namespace Hyt.Service.Implement.B2CApp
{
    /// <summary>
    /// 购物车接口相关
    /// </summary>
    /// <remarks>杨浩 2013-7-1 创建</remarks>
    public class ShoppingCart : BaseService, IShoppingCart
    {
        /// <summary>
        /// 促销使用平台
        /// </summary>
        readonly PromotionStatus.促销使用平台[] _platformTypes = new[] { PromotionStatus.促销使用平台.手机商城 };

        /// <summary>
        /// 获取购物车数量
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <returns>购物车数量</returns>
        /// <remarks>2013-10-10 杨浩 创建</remarks>
        public Result<int> GetShoppingQuantity(int customerSysNo)
        {
            return new Result<int>
                {
                    Data = IsLogin ? CrShoppingCartBo.Instance.GetShoppingCart(_platformTypes, customerSysNo).CartItemNumber() : 0,
                    Status = true,
                    StatusCode = 1
                };
        }

        /// <summary>
        /// 获取购物车对象
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <returns>购物车对象</returns>
        /// <remarks>2013-08-30 杨浩 创建</remarks>
        public Result<ShoppingCartApp> GetShoppingCart(int customerSysNo)
        {
            var temp = CrShoppingCartBo.Instance.GetShoppingCart(_platformTypes, customerSysNo);
            var data = new ShoppingCartApp
                {
                    GroupPromotions = temp.GroupPromotions,
                    ProductAmount = temp.ProductAmount,
                    SettlementAmount = temp.SettlementAmount,
                    ShoppingCartGroups = temp.ShoppingCartGroups,
                    TotalDiscountAmount = temp.CouponAmount + temp.FreightDiscountAmount + temp.ProductDiscountAmount + temp.SettlementDiscountAmount
                };

            return new Result<ShoppingCartApp>
                {
                    Data = data,
                    Status = true,
                    StatusCode = 1
                };
        }

        /// <summary>
        /// 修改购物车明细
        /// </summary>
        /// <param name="customerSysNo">客户编号</param>
        /// <param name="sysNo">购物车编号</param>
        /// <param name="quantity">商品数量</param>
        /// <returns>购物车明细</returns>
        /// <remarks> 2013-7-8 杨浩 创建</remarks>
        public Result<ShoppingCartApp> UpdateQuantity(int customerSysNo, int[] sysNo, int quantity)
        {
            CrShoppingCartBo.Instance.UpdateQuantity(CurrentUser.SysNo, sysNo, quantity);

            return GetShoppingCart(customerSysNo);
        }

        /// <summary>
        /// 删除购物车明细
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="sysNo">购物车编号</param>
        /// <returns>购物车明细</returns>
        /// <remarks> 2013-7-8 杨浩 创建</remarks>
        public Result<ShoppingCartApp> Remove(int customerSysNo, int[] sysNo)
        {
            CrShoppingCartBo.Instance.Remove(customerSysNo, sysNo);
            return GetShoppingCart(customerSysNo);
        }

        /// <summary>
        /// 删除促销赠品
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="productSysNo">商品(赠品)系统编号</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <returns>购物车明细</returns>
        /// <remarks>2013-08-13 杨浩 创建</remarks>
        public Result<ShoppingCartApp> RemoveGift(int customerSysNo, int productSysNo, int promotionSysNo)
        {
            CrShoppingCartBo.Instance.RemoveGift(customerSysNo, productSysNo, promotionSysNo);
            return GetShoppingCart(customerSysNo);
        }

        /// <summary>
        /// 获取用户收货地址
        /// </summary>
        /// <param name="customerSysNo">用户编号</param>
        /// <returns>用户收货地址</returns>
        /// <remarks> 
        /// 2013-7-12 杨浩 创建
        /// 2013-08-20 郑荣华 实现
        /// </remarks>
        public Result<IList<ReceiveAddress>> GetShippingAddress(int customerSysNo)
        {
            var crList = Hyt.BLL.Web.CrCustomerBo.Instance.GetCustomerReceiveAddress(CurrentUser.SysNo);
            IList<ReceiveAddress> rList = crList.Select(item => new ReceiveAddress
                {
                    AreaName = item.Province + item.City + item.Region,
                    IsDefault = item.IsDefault,
                    MobilePhoneNumber = item.MobilePhoneNumber,
                    Name = item.Name,
                    StreetAddress = item.StreetAddress,
                    SysNo = item.SysNo,
                    Title = item.Title,
                    AreaSysNo = item.AreaSysNo,
                    ZipCode = item.ZipCode,
                    EmailAddress = item.EmailAddress
                }).ToList();

            return new Result<IList<ReceiveAddress>>
                {
                    Data = rList,
                    Status = true,
                    StatusCode = 1
                };
        }

        /// <summary>
        /// 获取用户优惠券
        /// </summary>
        /// <param name="customerSysNo">用户账户</param>
        /// <returns>优惠券</returns>
        /// <remarks> 2013-7-12 杨浩 创建</remarks>
        /// <remarks>2013-12-30 吴文强 修改 添加优惠券使用平台类型</remarks>
        public Result<IList<Coupon>> GetCoupons(int customerSysNo)
        {
            //2013-12-30 吴文强 修改 添加优惠券使用平台类型
            var temp = SpCouponBo.Instance.GetCurrentCartValidCoupons(CurrentUser.SysNo,
                                                                      CrShoppingCartBo.Instance.GetShoppingCart(_platformTypes,
                                                                          customerSysNo), _platformTypes);
            var data = temp.Select(t => new Coupon
                {
                    SysNo = t.SysNo,
                    CouponAmount = t.CouponAmount,
                    CouponCode = t.CouponCode,
                    CustomerSysNo = t.CustomerSysNo,
                    Description = t.Description,
                    PromotionSysNo = t.PromotionSysNo,
                    RequirementAmount = t.RequirementAmount,
                    StartTime = t.StartTime,
                    EndTime = t.EndTime,
                    Color = "#3390d0"//bc33d0
                }).ToList();
            return new Result<IList<Coupon>>
                {
                    Data = data,
                    Status = true,
                    StatusCode = 1
                };
        }

        /// <summary>
        /// 获取支付方式
        /// </summary>
        /// <returns></returns>
        /// <param name="receiveAddressSysNo">收货地址编号</param>
        /// <returns>支付方式</returns>
        /// <remarks>
        /// 2013-7-12 杨浩 创建
        /// 2013-08-20 郑荣华 实现
        /// 2013-9-5 杨浩 修改
        /// </remarks>
        public Result<IList<PaymentType>> GetPaymentType(int receiveAddressSysNo)
        {
            var item = new PaymentType
                {
                    SysNo = Model.SystemPredefined.PaymentType.支付宝,
                    Type = Model.SystemPredefined.PaymentType.支付宝,
                    PaymentName = "支付宝"
                };

            var data = new List<PaymentType> { item };
            var isInScope = Hyt.BLL.Logistics.LgDeliveryScopeBo.Instance.IsInScope(receiveAddressSysNo);

            var itemo = new PaymentType();
            if (isInScope)
            {
                itemo.SysNo = Model.SystemPredefined.PaymentType.现金;
                itemo.Type = Model.SystemPredefined.PaymentType.现金;
                itemo.PaymentName = "货到付款";
                data.Add(itemo);
            }
            return new Result<IList<PaymentType>>
                {
                    Data = data,
                    Status = true,
                    StatusCode = 1
                };
        }

        /// <summary>
        /// 获取商品清单
        /// </summary>
        /// <returns></returns>
        /// <param name="customerSysNo">客户编号</param>
        /// <returns>商品清单</returns>
        /// <remarks> 2013-9-5 杨浩 创建</remarks>
        public Result<IList<SimplProductItem>> GetProducts(int customerSysNo)
        {
            var data = new List<SimplProductItem>();
            var temp = CrShoppingCartBo.Instance.GetShoppingCart(_platformTypes, customerSysNo, true);
            if (temp != null)
            {
                //取购物车明细
                if (temp.ShoppingCartGroups != null)
                {
                    foreach (var item in temp.ShoppingCartGroups)
                    {
                        var ruleIcon = string.Empty;
                        if (item.GroupPromotions != null)
                        {
                            ruleIcon = item.GroupPromotions.FirstOrDefault().RuleIcon;
                        }
                        if (item.ShoppingCartItems == null) continue;
                        var t = item.ShoppingCartItems.Select(g => new SimplProductItem
                            {
                                SysNo = g.ProductSysNo,
                                ProductName = g.ProductName,
                                Icon = ruleIcon,
                                Specification = "",
                                LevelPrice = g.SalesUnitPrice - g.DiscountAmount,
                                Thumbnail = g.Thumbnail,
                                Price = g.OriginPrice,
                                Quantity = g.Quantity
                            }).ToList();
                        data.AddRange(t);
                    }
                }
                //取订单赠品
                if (temp.GroupPromotions != null)
                {
                    foreach (var gift in temp.GroupPromotions)
                    {
                        if (gift.UsedGiftProducts == null) continue;
                        var t = gift.UsedGiftProducts.Select(g => new SimplProductItem
                            {
                                SysNo = g.ProductSysNo,
                                ProductName = g.ProductName,
                                Icon = g.GiftIcon,
                                Specification = "",
                                LevelPrice = g.PurchasePrice,
                                Thumbnail = g.Thumbnail,
                                Price = 0,
                                Quantity = gift.UsedGiftProducts.Count()
                            }).ToList();
                        data.AddRange(t);
                    }
                }
                //取购物车明细赠品
                if (temp.ShoppingCartGroups != null)
                {
                    foreach (var item in temp.ShoppingCartGroups)
                    {
                        if (item.GroupPromotions == null) continue;
                        foreach (var gift in item.GroupPromotions)
                        {
                            if (gift.UsedGiftProducts == null) continue;
                            var t = gift.UsedGiftProducts.Select(g => new SimplProductItem
                                {
                                    SysNo = g.ProductSysNo,
                                    ProductName = g.ProductName,
                                    Icon = g.GiftIcon,
                                    Specification = "",
                                    LevelPrice = g.PurchasePrice,
                                    Thumbnail = g.Thumbnail,
                                    Price = 0,
                                    Quantity = gift.UsedGiftProducts.Count()
                                }).ToList();
                            data.AddRange(t);
                        }
                    }
                }
            }

            return new Result<IList<SimplProductItem>>
                {
                    Data = data,
                    Status = true,
                    StatusCode = 1
                };
        }

        /// <summary>
        /// 添加商品到购物车
        /// </summary>
        /// <param name="customerSysNo">客户编号</param>
        /// <param name="productSysNo">产品编号</param>
        /// <param name="quantity">数量</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-08-30 周唐炬 实现
        /// </remarks>
        public Result AddToCart(int customerSysNo, int productSysNo, int quantity)
        {
            var result = new Result() { StatusCode = 1 };
            CrShoppingCartBo.Instance.Add(CurrentUser.SysNo, productSysNo, quantity, CustomerStatus.购物车商品来源.手机商城);
            result.Status = true;
            return result;
        }

        /// <summary>
        /// 添加组合或团购商品到购物车
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="groupSysNo">组系统编号(组合,团购主表系统编号)</param>
        /// <param name="quantity">组数量</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-09-01 杨浩 创建</remarks>
        public Result AddToCart(int customerSysNo, int groupSysNo, int quantity, int promotionSysNo)
        {
            CrShoppingCartBo.Instance.Add(customerSysNo, groupSysNo, quantity, promotionSysNo, CustomerStatus.购物车商品来源.手机商城);
            return new Result
                {
                    Status = true,
                    StatusCode = 1
                };
        }

        /// <summary>
        /// 添加促销赠品至购物车
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="productSysNo">商品(赠品)系统编号</param>
        /// <param name="promotionSysNo">促销系统编号</param>
        /// <returns>购物车</returns>
        /// <remarks>2013-09-3 杨浩 创建</remarks>
        public Result<ShoppingCartApp> AddGift(int customerSysNo, int productSysNo, int promotionSysNo)
        {
            CrShoppingCartBo.Instance.AddGift(customerSysNo, productSysNo, promotionSysNo, CustomerStatus.购物车商品来源.手机商城);

            return GetShoppingCart(customerSysNo);
        }

        /// <summary>
        /// 选择购物车明细项目
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="sysNo">购物车明细系统编号</param>
        /// <returns>购物车明细</returns>
        /// <remarks>2013-09-3 杨浩 创建</remarks>
        public Result<ShoppingCartApp> CheckedItem(int customerSysNo, int[] sysNo)
        {
            CrShoppingCartBo.Instance.CheckedItem(CurrentUser.SysNo, sysNo);

            return GetShoppingCart(customerSysNo);
        }

        /// <summary>
        /// 取消选择购物车明细项目
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <param name="sysNo">购物车明细系统编号</param>
        /// <returns>购物车明细</returns>
        /// <remarks>2013-09-3 杨浩 创建</remarks>
        public Result<ShoppingCartApp> UncheckedItem(int customerSysNo, int[] sysNo)
        {
            CrShoppingCartBo.Instance.UncheckedItem(CurrentUser.SysNo, sysNo);

            return GetShoppingCart(customerSysNo);
        }

        /// <summary>
        /// 配送方式
        /// </summary>
        /// <param name="receiveAddressSysNo">收货地址编号</param>
        /// <returns>配送方式</returns>
        /// <remarks>2013-09-3 杨浩 创建</remarks>
        public Result<IList<DeliveryType>> GetDeliveryType(int receiveAddressSysNo)
        {
            //TODO:待传支付编号
            var item1 = new DeliveryType { SysNo = Model.SystemPredefined.DeliveryType.第三方快递, Type = Model.SystemPredefined.DeliveryType.第三方快递, DeliveryTypeName = "第三方快递" };
            var item2 = new DeliveryType { SysNo = Model.SystemPredefined.DeliveryType.普通百城当日达, Type = Model.SystemPredefined.DeliveryType.普通百城当日达, DeliveryTypeName = "百城当日达" };
            var shop = new DeliveryType { SysNo = Model.SystemPredefined.DeliveryType.门店自提, Type = Model.SystemPredefined.DeliveryType.门店自提, DeliveryTypeName = "门店自提" };
            //var item3 = new DeliveryType { SysNo = Model.SystemPredefined.DeliveryType.加急百城当日达, Type = Model.SystemPredefined.DeliveryType.加急百城当日达, DeliveryTypeName = "一小时加急送" };
            //var item4 = new DeliveryType { SysNo = Model.SystemPredefined.DeliveryType.定时百城当日达, Type = Model.SystemPredefined.DeliveryType.定时百城当日达, DeliveryTypeName = "定时送" };  

            var data = new List<DeliveryType>();
            var address = Hyt.BLL.Web.CrCustomerBo.Instance.GetCustomerReceiveAddressBySysno(receiveAddressSysNo);
            var temp = Hyt.BLL.Warehouse.WhWarehouseBo.Instance.GetWhWareHouse(address.AreaSysNo, null, Hyt.Model.SystemPredefined.DeliveryType.门店自提);
            if (temp != null && temp.Count > 0)
            {
                data.Add(shop);
            }
            var isInScope = Hyt.BLL.Logistics.LgDeliveryScopeBo.Instance.IsInScope(address);
            if (isInScope)
                data.Add(item2);
            else
            {
                data.Add(item1);
            }
            return new Result<IList<DeliveryType>>
                {
                    Data = data,
                    Status = true,
                    StatusCode = 1
                };
        }

        /// <summary>
        /// 获取可选自提门店
        /// </summary>
        /// <param name="areaSysNo">地区编码</param>
        /// <returns>自提门店</returns>
        /// <remarks> 2013-9-5 杨浩 创建</remarks>
        /// <remarks>2013-10-29 黄波 修改获取自提点方式</remarks>
        public Result<IList<NWarehouse>> GetShop(int areaSysNo)
        {
            var temp = Hyt.BLL.Warehouse.WhWarehouseBo.Instance.GetWhWareHouse(areaSysNo, null, Hyt.Model.SystemPredefined.DeliveryType.门店自提);

            var data = temp.Select(t => new NWarehouse
                {
                    SysNo = t.SysNo,
                    WarehouseName = t.WarehouseName,
                    Latitude = t.Latitude,
                    Longitude = t.Longitude,
                    AreaName = "",
                    StreetAddress = t.StreetAddress

                }).ToList();

            return new Result<IList<NWarehouse>>
                {
                    Data = data,
                    Status = true,
                    StatusCode = 1
                };
        }

        /// <summary>
        /// 获取配送时间
        /// </summary>
        /// <returns>配送时间</returns>
        /// <remarks> 2013-9-5 杨浩 创建</remarks>
        public Result<IList<string>> GetDeliveryTime()
        {
            var data = new List<string> { "只工作日送货（双休日、节假日不送", "作日、双休日和节假日均送货", "只双休日、节假日送货（工作时间不送货）" };
            return new Result<IList<string>>
                {
                    Data = data,
                    Status = true,
                    StatusCode = 1
                };
        }

        /// <summary>
        /// 选择地址、配送方式、优惠劵 后计算金额
        /// </summary>
        /// <param name="customerSysNo">客户编号</param>
        /// <param name="areaSysNo">区域编号</param>
        /// <param name="deliveryTypeSysNo">配送方式编号</param>
        /// <param name="couponCode">优惠劵码</param>
        /// <returns>结算金额</returns>
        /// <remarks>2013-9-6 杨浩 创建</remarks>
        public Result<ShoppingAmount> CalculateAmount(int customerSysNo, int areaSysNo, int deliveryTypeSysNo, string couponCode)
        {
            var temp = CrShoppingCartBo.Instance.GetShoppingCart(_platformTypes, customerSysNo, null, areaSysNo, deliveryTypeSysNo, "", couponCode, true);
            var data = new ShoppingAmount
                {
                    CouponAmount = temp.CouponAmount,
                    FreightAmount = temp.FreightAmount,
                    ProductAmount = temp.ProductAmount,
                    SettlementAmount = temp.SettlementAmount,
                    TotalDiscountAmount = temp.FreightDiscountAmount + temp.ProductDiscountAmount + temp.SettlementDiscountAmount
                };
            return new Result<ShoppingAmount>
                {
                    Data = data,
                    Status = true,
                    StatusCode = 1
                };
        }

        /// <summary>
        /// 提交订单
        /// </summary>
        /// <param name="order">新建订单模型</param>
        /// <returns>返回新订单信息</returns>
        /// <remarks>2013-8-15 杨浩 创建</remarks>
        /// <remarks>2014-0616 陶辉 增加订单来源逻辑</remarks>
        public Result<OrderResult> CreateOrder(CreateOrder order)
        {
            var receiveAddressTemp = Hyt.BLL.CRM.CrReceiveAddressBo.Instance.GetCrReceiveAddress(order.ReceiveAddressSysNo);
            var receiveAddress = new SoReceiveAddress
                {
                    SysNo = receiveAddressTemp.SysNo,
                    AreaSysNo = receiveAddressTemp.AreaSysNo,
                    Name = receiveAddressTemp.Name,
                    EmailAddress = receiveAddressTemp.EmailAddress,
                    MobilePhoneNumber = receiveAddressTemp.MobilePhoneNumber,
                    PhoneNumber = receiveAddressTemp.PhoneNumber,
                    StreetAddress = receiveAddressTemp.StreetAddress,
                    ZipCode = receiveAddressTemp.ZipCode,
                    Gender = receiveAddressTemp.Gender,
                    FaxNumber = receiveAddressTemp.FaxNumber
                };
            var contactBeforeDelivery = OrderStatus.配送前是否联系.否;
            if (order.ContactBeforeDelivery)
                contactBeforeDelivery = OrderStatus.配送前是否联系.是;
            var shoppingCart = CrShoppingCartBo.Instance.GetShoppingCart(_platformTypes, order.CustomerSysNo, null, null, null, null, order.CouponCode, true);

            SoOrder newOrder = new SoOrder();
            using (var tran = new TransactionScope())
            {
                //newOrder = Hyt.BLL.Order.SoOrderBo.Instance.CreateOrder(User.SystemUser, order.CustomerSysNo, receiveAddress,
                                                             //order.DefaultWarehouseSysNo, order.DeliveryTypeSysNo,
                                                             //order.PayTypeSysNo, shoppingCart, 0, null,
                                                             //order.OrderSource==null? OrderStatus.销售单来源.手机商城:(OrderStatus.销售单来源)order.OrderSource, null, OrderStatus.销售方式.普通订单, null,
                                                             //OrderStatus.销售单对用户隐藏.否, string.Empty, string.Empty,
                                                             //string.Empty, order.DeliveryTime, contactBeforeDelivery,
                                                             //string.Empty,0);
                tran.Complete();
            }
            
            //返回新订单信息
            var data = new OrderResult
                {
                    OrderSysNo = newOrder.SysNo.ToString(),
                    Subject = "商城订单号：" + newOrder.SysNo,
                    SettlementAmount = shoppingCart.SettlementAmount,
                    CreateDate = newOrder.CreateDate,
                    PaymentType = GetPaymentType(order.ReceiveAddressSysNo).Data.SingleOrDefault(x => x.SysNo == order.PayTypeSysNo)
                };
            
            return new Result<OrderResult> { Data = data, Status = true, StatusCode = 1 };
        }
    }
}
