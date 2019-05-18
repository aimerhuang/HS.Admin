using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Transactions;
using Extra.Erp;
using Extra.Erp.Model.Borrowing;
using Extra.Erp.Model.Sale;
using Hyt.BLL.Basic;
using Hyt.BLL.Finance;
using Hyt.BLL.Order;
using Hyt.BLL.Product;
using Hyt.BLL.Sys;
using Hyt.BLL.Warehouse;
using Hyt.DataAccess.Finance;
using Hyt.DataAccess.Order;
using Hyt.DataAccess.Warehouse;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.SystemPredefined;
using Hyt.Model.WorkflowStatus;
using Hyt.DataAccess.Logistics;

namespace Hyt.BLL.Logistics
{
    /// <summary>
    /// 调用订单相关BO
    /// </summary>
    /// <remarks>2013-07-16 黄伟 创建</remarks>
    public class LogisticsOrderBo : BOBase<LogisticsOrderBo>
    {

        /// <summary>
        /// 补单
        /// </summary>
        /// <param name="model">补单实体</param>
        /// <param name="user">当前用户实体</param>
        /// <returns>true:成功，失败抛出异常</returns>
        /// <remarks>2013-07-16 黄伟 创建</remarks>
        /// <remarks>2013-11-15 周唐炬 加入商品借货、EAS业务、恢复配送员信用</remarks>
        private Result CreateOrder(ParaLogisticsControllerAdditionalOrders model, SyUser user)
        {
            var result = new Result { Status = false, StatusCode = -1, Message = "补单失败!" };
            var so = new SoOrder();
            var currentTime = DateTime.Now;
            var client = EasProviderFactory.CreateProvider();//EAS Provider

            var borrowInfoGroupList = new List<BorrowInfoGroup>();//EAS还货数据
            var saleInfoGroupList = new List<SaleInfoGroup>();//EAS出库分组数据
            var deliveryUser = SyUserBo.Instance.GetSyUser(model.DeliverymanSysNo);
            #region 补单实体数据
            //保存收货地址
            var address = model.ReceiveAddress;
            var soReceiveAddress = new SoReceiveAddress
                {
                    AreaSysNo = address.AreaSysNo,
                    MobilePhoneNumber = address.MobilePhoneNumber,
                    Name = address.Name,
                    PhoneNumber = address.PhoneNumber,
                    StreetAddress = address.Address,
                    ZipCode = address.ZipCode
                };
            //创建收货地址
            ISoReceiveAddressDao.Instance.InsertEntity(soReceiveAddress);

            so.ReceiveAddressSysNo = soReceiveAddress.SysNo;
            so.CustomerSysNo = model.UserSysNo;
            so.LevelSysNo = model.LevelSysNo;

            var deliveryType = GetDelTypeByNameLike("普通百城当日"); //普通百城当日
            so.DeliveryTypeSysNo = deliveryType.SysNo;
            so.Remarks = so.DeliveryRemarks = "补单";
            so.DeliveryTime = deliveryType.DeliveryTime; //全天

            so.PayTypeSysNo = model.PaymentTypeSysNo;
            so.DefaultWarehouseSysNo = model.WarehouseSysNo;
            so.CreateDate = currentTime;
            so.LastUpdateBy = user.SysNo;
            so.LastUpdateDate = currentTime;
            so.OrderCreatorSysNo = user.SysNo;
            //order status related
            so.OrderSource = (int)OrderStatus.销售单来源.业务员补单;
            so.OrderSourceSysNo = model.DeliverymanSysNo;
            so.PayStatus = (int)OrderStatus.销售单支付状态.已支付;
            so.SalesSysNo = 0;
            so.SalesType = (int)OrderStatus.销售方式.普通订单;
            so.Status = (int)OrderStatus.销售单状态.已完成;
            so.OnlineStatus = Constant.OlineStatusType.已发货;

            //创建订单主表
            ISoOrderDao.Instance.InsertEntity(so);
            so = SoOrderBo.Instance.GetEntity(so.SysNo);
            var lstSoOrderItem = new List<SoOrderItem>();
            var soItems = model.OrderInformations;
            #endregion

            //该商品配送员等级价总金额
            var deliveryPrice = decimal.Zero;

            foreach (var item in soItems)
            {
                var originalPrice = SoOrderBo.Instance.GetOriginalPrice(model.UserSysNo, item.ProductSysNo);
                var soItem = new SoOrderItem()
                    {
                        OrderSysNo = so.SysNo,
                        OriginalPrice = originalPrice,
                        ProductName = item.ProductName,
                        ProductSysNo = item.ProductSysNo,
                        TransactionSysNo = so.TransactionSysNo,
                        Quantity = item.ProductOrderNumber,
                        SalesAmount = originalPrice * item.ProductOrderNumber,
                        RealStockOutQuantity = item.ProductOrderNumber,
                        ProductSalesType = (int)CustomerStatus.商品销售类型.普通,
                        SalesUnitPrice = originalPrice
                    };
                so.OrderAmount += originalPrice * item.ProductOrderNumber;
                //创建订单明细
                soItem.SysNo = ISoOrderItemDao.Instance.Insert(soItem);
                lstSoOrderItem.Add(soItem);

                #region 计算商品配送员进货价
                var productLendItem = IProductLendDao.Instance.GetWhProductLendItemInfo(new ParaWhProductLendItemFilter()
                {
                    DeliveryUserSysNo = model.DeliverymanSysNo,
                    ProductSysNo = item.ProductSysNo,
                    PriceSource = ProductStatus.产品价格来源.配送员进货价.GetHashCode()
                });
                if (productLendItem != null)
                {
                    deliveryPrice += item.ProductOrderNumber * productLendItem.Price;
                }
                #endregion

                #region 修改借货数量
                //配送员补单的时候，eas 要做还货，然后再做销售出库
                //(即补单完成后，要先调用还货接口、再调用销售出库接口)
                ProductReturn(model.DeliverymanSysNo, item, user.SysNo);

                #endregion

                #region EAS业务数据

                var product = PdProductBo.Instance.GetProduct(item.ProductSysNo);
                if (product == null) continue;
                //重新获取到包含信用等级价格的借货明细
                var productLend = IProductLendDao.Instance.GetWhProductLendItemInfo(new ParaWhProductLendItemFilter()
                {
                    ProductSysNo = product.SysNo,
                    DeliveryUserSysNo = model.DeliverymanSysNo,
                    PriceSource = ProductStatus.产品价格来源.配送员进货价.GetHashCode()
                });
                var warehouse = WhWarehouseBo.Instance.GetWarehouse(model.WarehouseSysNo);
                var warehouseErpCode = string.Empty;
                string organizationCode = string.Empty;//组织机构代码
                if (warehouse != null)
                {
                    warehouseErpCode = warehouse.ErpCode;
                    var oraganization = OrganizationBo.Instance.GetOrganization(warehouse.SysNo);
                    if (oraganization != null)
                    {
                        organizationCode = oraganization.Code;
                    }
                }
                if (productLend == null) continue;

                #region EAS还货数据

                var borrowInfoGroup = borrowInfoGroupList.SingleOrDefault(x => x.ProductLendSysNo == productLend.ProductLendSysNo) ?? new BorrowInfoGroup() { ProductLendSysNo = productLend.ProductLendSysNo };
                var borrowInfo = new BorrowInfo()
                    {
                        ErpCode = product.ErpCode,
                        Quantity = item.ProductOrderNumber,
                        WarehouseNumber = warehouseErpCode,
                        Amount = productLend.Price,
                        Remark = string.Empty,
                        WarehouseSysNo=model.WarehouseSysNo
                    };
                //入库单
                var stockin = IInStockDao.Instance.GetStockInBySource(WarehouseStatus.入库单据类型.借货单.GetHashCode(), productLend.SysNo);
                if (stockin != null)
                {
                    borrowInfoGroup.StockInSysno = stockin.SysNo;
                }
                borrowInfoGroup.BorrowInfoList.Add(borrowInfo);
                //添加到GroupList中
                if (borrowInfoGroupList.All(x => x.ProductLendSysNo != productLend.ProductLendSysNo))
                {
                    borrowInfoGroupList.Add(borrowInfoGroup);
                }
                #endregion

                #region EAS销售出库数据

                //根据借货单编号查询分组数据中的SaleInfo信息
                var saleInfoGroup = saleInfoGroupList.SingleOrDefault(x => x.ProductLendSysNo == productLend.ProductLendSysNo) ?? new SaleInfoGroup() { ProductLendSysNo = productLend.ProductLendSysNo };
                var saleInfo = new SaleInfo()
                {
                    ErpCode = product.ErpCode,
                    Quantity = item.ProductOrderNumber,
                    WarehouseNumber = warehouseErpCode,
                    WarehouseSysNo=model.WarehouseSysNo,
                    OrganizationCode=organizationCode,
                    Amount = originalPrice,
                    //销售出库接口的备注格式：JC[Hyt借货单系统编号]-[借货员姓名]-XS[Hyt订单号]
                    Remark = string.Format("JC[{0}]-[{1}]-XS[{2}],配送方式:百城当日达(补单)", saleInfoGroup.ProductLendSysNo, deliveryUser != null ? deliveryUser.UserName : model.DeliverymanSysNo.ToString(CultureInfo.InvariantCulture), so.SysNo)
                };
                saleInfoGroup.SaleInfoList.Add(saleInfo);
                //添加到Groups中
                if (saleInfoGroupList.All(x => x.ProductLendSysNo != productLend.ProductLendSysNo))
                {
                    saleInfoGroupList.Add(saleInfoGroup);
                }
                #endregion

                #endregion
            }
            #region 恢复配送员信用

            if (deliveryPrice > decimal.Zero)
            {
                DeliveryUserCreditBo.Instance.UpdateRemaining(model.WarehouseSysNo, model.DeliverymanSysNo, 0, deliveryPrice, "补单，单号：" + so.SysNo);
            }
            #endregion

            #region 更新订单、创建收款单、加入订单池、创建配送单配送在途
            //从接口调用金额计算
            so.CashPay = ISoOrderItemDao.Instance.SynchronousOrderAmount(so.SysNo);//同步订单价格
            //so.CashPay = so.OrderAmount; //订单现金支付金额

            //更新订单金额
            so.ProductAmount = so.OrderAmount;

            SoOrderBo.Instance.UpdateOrder(so); //更新订单 余勇修改为调用业务层方法 ISoOrderDao.Instance.Update(so);

            //创建订单收款单
            FnReceiptVoucherBo.Instance.CreateReceiptVoucherByOrder(so);
            SoOrderBo.Instance.WriteSoTransactionLog(so.TransactionSysNo
                                                     , string.Format(Constant.ORDER_TRANSACTIONLOG_CREATE, so.SysNo)
                                                     , user.UserName);
            //补单不用加入订单池
            //创建出库单已签收
            var stockOut = CreateOutStock(lstSoOrderItem, model.WarehouseSysNo, user);

            //配送方式  
            var delivertType = DeliveryTypeBo.Instance.GetDeliveryType(so.DeliveryTypeSysNo);

            //创建配送单配送在途
            LgDeliveryBo.Instance.CreateLgDelivery(model.WarehouseSysNo, model.DeliverymanSysNo, delivertType,
                                                   user.SysNo, new List<LgDeliveryItem> {new LgDeliveryItem
                                                       {
                                                           NoteType =(int)LogisticsStatus.配送单据类型.出库单,
                                                           NoteSysNo = stockOut.SysNo,
                                                           ExpressNo = ""
                                                       }}, true);

            #endregion

            #region EAS还货
            if (borrowInfoGroupList.Any())
            {
                //摘要：JC[Hyt借出编号]-[借货员姓名]-RK[Hyt入库编号]
                //（如果还货的商品在不同 借货单中，就以借货单明细分开调用）
                borrowInfoGroupList.ForEach(x =>
                {
                    if (x.BorrowInfoList.Count > 0)
                    {
                        client.Return(x.BorrowInfoList, string.Format("JC[{0}]-[{1}]-RK[{2}]", x.ProductLendSysNo,
                            deliveryUser != null ? deliveryUser.UserName : model.DeliverymanSysNo.ToString(),
                            x.StockInSysno),x.ProductLendSysNo.ToString());
                    }
                });
            }
            #endregion

            #region EAS出库
            if (saleInfoGroupList.Any())
            {
                saleInfoGroupList.ForEach(x =>
                {
                    if (x.SaleInfoList.Count > 0)
                    {
                        client.SaleOutStock(x.SaleInfoList, Extra.Erp.Model.EasConstant.HytCustomer, so.SysNo.ToString(),so.TransactionSysNo);
                    }
                });
            }
            #endregion

            //新增会员明细 2014-1-17 黄志勇 添加
            LgSettlementBo.Instance.WriteShopNewCustomerDetail(so.CustomerSysNo, stockOut.StockOutAmount);

            result.Status = true;
            result.StatusCode = so.SysNo;
            result.Message = "补单成功!";
            return result;
        }

        /// <summary>
        /// 修改借货数量
        /// </summary>
        /// <param name="deliveryUserSysNo">配送员系统编号</param>
        /// <param name="orderItem">商品销售数据</param>
        /// <param name="lastUpdateBySysNo">最后更新人系统编号</param>
        /// <returns></returns>
        /// <remarks>2013-11-15 周唐炬 创建</remarks>
        private void ProductReturn(int deliveryUserSysNo, OrderInformation orderItem, int lastUpdateBySysNo)
        {
            if (orderItem.ProductOrderNumber <= 0) throw new HytException("补单商品数量必需大于0个");
            //配送员所有未完结借货单明细
            var list = IProductLendDao.Instance.GetWhProductLendItemList(new ParaWhProductLendItemFilter()
                        {
                            DeliveryUserSysNo = deliveryUserSysNo,
                            ProductSysNo = orderItem.ProductSysNo,
                            Status = (int)WarehouseStatus.借货单状态.已出库
                        });
            if (list == null || !list.Any())
            {
                throw new HytException("没有可补单的商品");
            }

            var productlends = new List<int>();//借货单系统编号列表
            //本次销售数量
            var saleQuantity = orderItem.ProductOrderNumber;
            //所有需要销售的数量
            var allQuantity = 0;
            list.ForEach(x =>
                {
                    //销售数量用完直接跳出
                    if (saleQuantity <= 0) return;

                    var model = IProductLendDao.Instance.GetWhProductLendItem(x);
                    if (model == null) return;
                    //计算该借货明细完成需要销售数量的
                    var quantity = model.LendQuantity - model.ReturnQuantity - model.SaleQuantity;
                    allQuantity += quantity;
                    //该条借货明细记录可销售数量大于或等于本次销售数量
                    if (quantity > saleQuantity)
                    {
                        model.SaleQuantity += saleQuantity;
                        saleQuantity = 0;
                    }
                    else//该条借货明细记录的可销售数量小于本次销售数量
                    {
                        //补满销售数量
                        model.SaleQuantity += quantity;
                        saleQuantity = saleQuantity - quantity;
                    }

                    IProductLendDao.Instance.UpdateWhProductLendItem(model);

                    //借货数量等于销售数量加还货数量，该条记录表于需要检查借货单完成情况
                    if (model.SaleQuantity + model.ReturnQuantity != model.LendQuantity) return;
                    if (productlends.All(c => c != model.ProductLendSysNo))
                    {
                        productlends.Add(model.ProductLendSysNo);
                    }
                });
            if (saleQuantity > allQuantity)
            {
                throw new HytException("补单的商品数量超过本商品最大可销售商品数量，操作失败!");
            }
            // 完结销售完的借货单
            if (productlends.Any())
            {
                productlends.ForEach(x => ProductLendBo.Instance.CompleteProductLend(x, lastUpdateBySysNo));
            }
        }
        #region 从购物车补单   适合手机App补单
        /// <summary>
        /// 补单（订单-出库单-配送单-结算单）
        /// </summary>
        /// <param name="deliverUserSysNo">配送员系统编号</param>
        /// <param name="customerSysNo">顾客系统编号</param>
        /// <param name="receiveAddress">收货地址</param>
        /// <param name="defaultWarehouseSysNo">仓库系统编号</param>
        /// <param name="shoppingCart">购物车</param>
        /// <param name="user">登录用户信息</param>
        /// <returns>补单完成情况</returns>
        /// <remarks>
        /// 2013-09-25 郑荣华 创建
        /// 2014-04-21 何方 不再使用补单功能
        /// </remarks>
        public Result RemedyOrder(int deliverUserSysNo, int customerSysNo, SoReceiveAddress receiveAddress,
                                  int defaultWarehouseSysNo, CrShoppingCart shoppingCart, SyUser user)
        {
            var result = new Result() { Message = "开始补单", Status = false, StatusCode = 0 };
            var deliveryTypeSysNo = DeliveryType.百城当日达;
            
                try
                {
                    //创建订单
                    var order = SoOrderBo.Instance.CreateOrder(user.SysNo, customerSysNo, receiveAddress, defaultWarehouseSysNo,
                                      deliveryTypeSysNo, PaymentType.现金, shoppingCart, 0, null,
                                      OrderStatus.销售单来源.业务员补单, null,
                                      OrderStatus.销售方式.售后订单, null,
                                      OrderStatus.销售单对用户隐藏.否, "", "补单", "补单", "补单任意时间段",
                                      OrderStatus.配送前是否联系.否, "补单");

                    result.Message = "订单创建完成";
                    //修改订单状态
                    order.Status = (int)OrderStatus.销售单状态.待创建出库单;

                    SoOrderBo.Instance.UpdateOrder(order); //更新订单 余勇修改为调用业务层方法 ISoOrderDao.Instance.Update(order);

                    result.Message = "订单状态修改完成";

                    //获取订单明细
                    order.OrderItemList = SoOrderBo.Instance.GetOrderItemsByOrderId(order.SysNo);
                    if (order.OrderItemList != null)
                    {
                        foreach (var oo in order.OrderItemList)
                        {
                            oo.RealStockOutQuantity = oo.Quantity;
                        }
                    }

                    //创建出库单
                    var stockout = SoOrderBo.Instance.CreateOutStock(order.OrderItemList, defaultWarehouseSysNo, user);
                    result.Message = "出库单创建完成";

                    //更新出库单状态处于待配送状态
                    WhWarehouseBo.Instance.UpdateOrderStockOutStatus(order.SysNo, WarehouseStatus.出库单状态.待配送, user.SysNo);

                    //创建配送单+结算单
                    var item = new LgDeliveryItem()
                    {
                        NoteType = (int)LogisticsStatus.配送单据类型.出库单,
                        NoteSysNo = stockout.SysNo,
                        ExpressNo = ""
                    };
                    var list = new List<LgDeliveryItem> { item };

                    //配送方式  
                    var delivertType = DeliveryTypeBo.Instance.GetDeliveryType(deliveryTypeSysNo);

                    LgDeliveryBo.Instance.CreateLgDelivery(defaultWarehouseSysNo, deliverUserSysNo, delivertType, user.SysNo, list, true);
                    //新增会员明细 2014-1-17 黄志勇 添加
                    LgSettlementBo.Instance.WriteShopNewCustomerDetail(order.CustomerSysNo, stockout.StockOutAmount);

                    result.Status = true;
                    result.StatusCode = 1;
                    result.Message = "补单操作完成";
                   
                    return result;
                }
                catch (Exception ex)
                {
                    Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, result.Message, LogStatus.系统日志目标类型.补单, 0, ex);
                    return result;
                }
              
            
        }
        #endregion

        /// <summary>
        /// 创建订单收款单
        /// </summary>
        /// <param name="soOrder">订单</param>
        /// <returns>FnReceiptVoucher</returns>
        /// <remarks>2013-07-16 黄伟 创建</remarks>
        private FnReceiptVoucher CreateReceiptVoucherByOrder(SoOrder soOrder)
        {

            var entity = IFnReceiptVoucherDao.Instance.GetEntity((int)FinanceStatus.收款来源类型.销售单, soOrder.SysNo);
            if (entity != null) return null;
            var payType = Hyt.BLL.Basic.PaymentTypeBo.Instance.GetEntity(soOrder.PayTypeSysNo);
            var rv = new FnReceiptVoucher()
            {
                TransactionSysNo = soOrder.TransactionSysNo,
                Status = (int)FinanceStatus.收款单状态.已确认,
                ConfirmedBy = soOrder.OrderCreatorSysNo,
                ConfirmedDate = DateTime.Now,
                Source = (int)FinanceStatus.收款来源类型.销售单,
                CreatedBy = soOrder.OrderCreatorSysNo,
                CreatedDate = DateTime.Now,
                IncomeAmount = soOrder.CashPay,
                IncomeType = payType.PaymentType,
                SourceSysNo = soOrder.SysNo
            };
            rv.SysNo = IFnReceiptVoucherDao.Instance.Insert(rv);
            return rv;
        }

        /// <summary>
        /// 模糊查询配送方式名返回系统编号
        /// </summary>
        /// <param name="name">配送方式名</param>
        /// <returns>系统编号</returns>
        /// <remarks>2013-07-16 黄伟 创建</remarks>
        private LgDeliveryType GetDelTypeByNameLike(string name)
        {
            return ILogisticsOrderDao.Instance.GetDelTypeByNameLike(name);
        }

        /// <summary>
        /// 检查补单实体
        /// </summary>
        /// <returns>返回检查结果</returns>
        /// <remarks>2013-07-16 沈强 创建</remarks>
        private Result CheckOrder(ParaLogisticsControllerAdditionalOrders model)
        {
            var result = new Result { Status = true, Message = "检查通过" };

            #region 检查配送员是否存在
            IList<SyUser> syUsers = Hyt.DataAccess.Warehouse.IWhWarehouseDao.Instance.GetWhDeliveryUser(model.WarehouseSysNo,
                                                                                Hyt.Model.WorkflowStatus.LogisticsStatus
                                                                                   .配送员是否允许配送.是);
            int count = syUsers.Count(s => s.SysNo == model.DeliverymanSysNo);
            if (count != 1)
            {
                result.Status = false;
                result.Message = "配送员不存在！";
                return result;
            }
            #endregion

            #region 会员是否存在

            CrCustomer crCustomer = Hyt.DataAccess.CRM.ICrCustomerDao.Instance.GetCrCustomerItem(model.UserSysNo);
            if (crCustomer == null)
            {
                result.Status = false;
                result.Message = "会员不存在！";
                return result;
            }
            #endregion

            #region 检查收货地址、收货人手机号、收货地区编号是否输入

            if (string.IsNullOrEmpty(model.ReceiveAddress.Address.Trim()))
            {
                result.Status = false;
                result.Message = "请输入收货地址！";
                return result;
            }
            if (string.IsNullOrEmpty(model.ReceiveAddress.MobilePhoneNumber.Trim()))
            {
                result.Status = false;
                result.Message = "请输入收货人手机号！";
                return result;
            }
            if (model.ReceiveAddress.AreaSysNo == -1)
            {
                result.Status = false;
                result.Message = "请选择收货地区！";
                return result;
            }

            #endregion

            #region 支付方式是否为货到付款类型

            BsPaymentType bsPaymentType = PaymentTypeBo.Instance.GetPaymentTypeFromMemory(model.PaymentTypeSysNo);
            if (bsPaymentType == null ||
                bsPaymentType.PaymentType != (int)Hyt.Model.WorkflowStatus.BasicStatus.支付方式类型.到付)
            {
                result.Status = false;
                result.Message = "支付方式不是货到付款类型！";
                return result;
            }

            #endregion

            #region 检查是否添加了订单

            if (model.OrderInformations == null || model.OrderInformations.Count == 0)
            {
                result.Status = false;
                result.Message = "请添加订单再提交！";
                return result;
            }

            #endregion

            #region 检查订单中的商品订购数量是否超过库存数量

            IList<CBPdProductJson> cbPdProductJsons = Hyt.DataAccess.Warehouse.IWhWarehouseDao.Instance.GetProductLendGoods(model.DeliverymanSysNo,
                                                                                  model.LevelSysNo);
            //获取订购数量大于借货单存货数量的订单数
            int orderCount = (from c in cbPdProductJsons
                              join o in model.OrderInformations
                                  on c.ProductSysNo equals o.ProductSysNo
                              where o.ProductOrderNumber > c.ProductNum
                              select o).Count();

            if (orderCount > 0)
            {
                result.Status = false;
                result.Message = "订单中的商品订购数量超过库存数量！";
                return result;
            }

            #endregion

            return result;
        }

        /// <summary>
        /// 订单分配出库
        /// </summary>
        /// <param name="datas">出库商品列表:Model.Quantity 为出库数量</param>
        /// <param name="warehouseSysNo">仓库编号</param>
        /// <param name="user">操作人</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-07-16 黄伟 创建
        /// 2013-12-19 黄志勇 修改订单日志
        /// </remarks>
        private WhStockOut CreateOutStock(IList<Model.SoOrderItem> datas, int warehouseSysNo, SyUser user)
        {
            if (datas == null || !datas.Any()) return null;
            if (warehouseSysNo < 1) throw new ArgumentNullException("warehouseSysNo", @"必需选择一个仓库");
            var so = SoOrderBo.Instance.GetEntity(datas[0].OrderSysNo);
            var soItems = SoOrderBo.Instance.GetOrderItemsByOrderId(so.SysNo);

            var whStockOut = new WhStockOut()
                {
                    ContactBeforeDelivery = so.ContactBeforeDelivery,
                    CreatedBy = user.SysNo,
                    CreatedDate = DateTime.Now,
                    ReceiveAddressSysNo = so.ReceiveAddressSysNo,
                    CustomerMessage = so.CustomerMessage,
                    DeliveryRemarks = so.DeliveryRemarks,
                    DeliveryTime = so.DeliveryTime,
                    DeliveryTypeSysNo = so.DeliveryTypeSysNo,
                    InvoiceSysNo = so.InvoiceSysNo,
                    IsCOD = 1,
                    IsPrintedPackageCover = 1,
                    IsPrintedPickupCover = 1,
                    LastUpdateBy = user.SysNo,
                    LastUpdateDate = DateTime.Now,
                    OrderSysNO = so.SysNo,
                    Receivable = so.CashPay,
                    Remarks = so.Remarks,
                    Status = (int)WarehouseStatus.出库单状态.待配送,
                    StockOutAmount = so.OrderAmount,
                    TransactionSysNo = so.TransactionSysNo,
                    WarehouseSysNo = warehouseSysNo,
                    StockOutDate = DateTime.Now,
                    StockOutBy = user.SysNo
                };

            //note:调用保存出库单主表的方法
            whStockOut.SysNo = IOutStockDao.Instance.InsertMain(whStockOut);

            foreach (var data in datas)
            {
                var whStockOutItem = new WhStockOutItem()
                    {
                        CreatedBy = user.SysNo,
                        CreatedDate = DateTime.Now,
                        LastUpdateBy = user.SysNo,
                        LastUpdateDate = DateTime.Now,
                        Measurement = "",
                        OrderSysNo = so.SysNo,
                        OriginalPrice = data.OriginalPrice,
                        ProductName = data.ProductName,
                        ProductQuantity = data.Quantity,
                        RealSalesAmount = data.SalesAmount / data.Quantity * data.RealStockOutQuantity,
                        ProductSysNo = data.ProductSysNo,
                        Status = 1,
                        StockOutSysNo = whStockOut.SysNo,
                        TransactionSysNo = so.TransactionSysNo,
                        Weight = 0m,
                        OrderItemSysNo = data.SysNo
                    };

                //调用保存出库单明细表的方法
                IOutStockDao.Instance.InsertItem(whStockOutItem); //朱成果 添加
                var soItem = soItems.First(p => p.SysNo == data.SysNo);
                //更新当前出库明细中的出库数量
                soItem.RealStockOutQuantity += data.Quantity;
                //出库数量到数据库
                ISoOrderItemDao.Instance.UpdateOutStockQuantity(soItem.SysNo, soItem.RealStockOutQuantity);

            }

            //更新销售单主表
            so.Status = (int)OrderStatus.销售单状态.已创建出库单;
            so.OnlineStatus = Constant.OlineStatusType.已发货;

            //调用更新销售单主表方法
            so.DefaultWarehouseSysNo = warehouseSysNo;

            SoOrderBo.Instance.UpdateOrder(so); //更新订单 余勇修改为调用业务层方法  ISoOrderDao.Instance.Update(so); //更新订单状态，默认出库仓库

            var warehouseName = WhWarehouseBo.Instance.GetWarehouseEntity(warehouseSysNo).WarehouseName;
            SoOrderBo.Instance.WriteSoTransactionLog(so.TransactionSysNo,
                                                     string.Format(Constant.ORDER_TRANSACTIONLOG_OUTSTOCK_CREATE,
                                                                   warehouseName, whStockOut.SysNo), user.UserName);

            return whStockOut;

        }

        /// <summary>
        /// 添加订单操作
        /// </summary>
        /// <param name="model">补单对象</param>
        /// <param name="user">操作人</param>
        /// <returns>成功返回Result；失败抛出异常</returns>
        /// <remarks>2013-07-16 黄伟 创建</remarks>
        public Result AddOrders(Hyt.Model.Parameter.ParaLogisticsControllerAdditionalOrders model, SyUser user)
        {
            Result result = CheckOrder(model);
            if (result.Status)
            {
                //不再使用补单,取消事务方法 何方 2014/04/41
                result = CreateOrder(model, user);


            }
            return result;
        }
    }
}
