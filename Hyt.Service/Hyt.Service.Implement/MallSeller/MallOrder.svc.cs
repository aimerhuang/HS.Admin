using Hyt.BLL.MallSeller;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using Hyt.Infrastructure.Pager;
using Hyt.BLL.Log;
using Hyt.Model.SystemPredefined;
using Hyt.Service.Contract.MallSeller;
using Hyt.Util.Validator;
using Hyt.Model.WorkflowStatus;
using Hyt.Model.UpGrade;
using Hyt.BLL.Distribution;
using System.Transactions;

namespace Hyt.Service.Implement.MallSeller
{
    /// <summary>
    /// 商城订单查询实现类
    /// </summary>
    /// <remarks>2013-8-28 陶辉 创建</remarks>
    public class MallOrder : BaseService, IMallOrder
    {
        /// <summary>
        /// 根据开始日期获取指定状态的升舱订单
        /// </summary>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="dearerMallSysNo">商城系统编号</param>
        /// <returns></returns>
        public List<CBDsOrder> GetSuccessedOrder(DateTime startDate,DateTime endDate,int dearerMallSysNo)
        {
            return DsOrderBo.Instance.GetSuccessedOrder(startDate, endDate,dearerMallSysNo, Hyt.Model.WorkflowStatus.DistributionStatus.升舱订单状态.已完成);
        }

        /// <summary>
        /// 查询分销商已升舱订单
        /// </summary>
        /// <param name="param">查询参数</param>
        /// <returns>已升舱订单列表</returns>
        /// <remarks>2013-8-28 陶辉 创建</remarks>
        /// <remarks>2013-09-04 朱家宏 实现</remarks>
        /// <remarks>2013-10-15 黄志勇 实现</remarks>
        public Result<PagedList<UpGradeOrder>> GetMallOrderList(MallOrderParameters param)
        {
            Pager<CBDsOrder> dsOrders;
            var filter = new ParaDsOrderFilter
            {
                PageIndex = param.PageIndex <= 0 ? 1 : param.PageIndex,
                PageSize = param.PageSize,
                MallProductName = param.ProductName,
                MallProductId = param.ProductCode,
                MallOrderId = param.OrderID,
                BuyerNick = param.BuyerName,
                BeginDate = param.StartDate,
                EndDate = param.EndDate,
                DealerMallSysNo = param.DealerMallSysNo,
                HytOrderStatus = param.LogisticsStatus,
                OrderSysNo = param.OrderSysNo,
                MobilePhoneNumber = param.MobilePhoneNumber,
                ReceiveName = param.ReceiveName
            };
            if (param.OrderState == 1)
            {
                dsOrders = DsOrderBo.Instance.GetPagerSuccessedList(filter);
            }
            else
            {
                dsOrders = DsOrderBo.Instance.GetPagerList(filter);
            }

            var list = new List<UpGradeOrder>();
            foreach (var order in dsOrders.Rows)
            {
                var mallOrderInfo = new UpGradeOrder
                {
                    HytOrderDealer = new HytOrderDealerInfo
                    {
                        HytOrderTime = order.UpgradeTime,
                        LogisticsTime = order.DeliveryTime,
                        DsOrderSysNo = order.SysNo,
                        HytPayStatus = order.PayStatus,
                        DeliveryStatus = order.Status.ToString(),
                        HytPayTime = order.PayTime,
                        HytOrderId = order.OrderSysNo.ToString()
                    },
                    MallOrderBuyer = new MallOrderBuyerInfo
                    {
                        MallOrderId = order.MallOrderId,
                        BuyerNick = order.BuyerNick
                    },
                    MallOrderPayment = new MallOrderPaymentInfo(),
                    MallOrderReceive = new MallOrderReceiveInfo()
                };
                list.Add(mallOrderInfo);
            }
            var result = new Result<PagedList<UpGradeOrder>>
            {
                Data = new PagedList<UpGradeOrder>
                {
                    TotalItemCount = dsOrders.TotalRows,
                    CurrentPageIndex = dsOrders.CurrentPage,
                    TData = list
                },
                Status = true
            };
            return result;
        }

        #region 升舱订单详情

        /// <summary>
        /// 获取已升舱订单详情
        /// </summary>
        /// <param name="sysNo">订单升舱编号</param>
        /// <returns>已升舱订单详情</returns>
        /// <remarks>2013-09-26 朱成果 实现</remarks>
        public Result<UpGradeOrder> GetDsOrder(int sysNo)
        {
            var dsorder = DsOrderBo.Instance.SelectBySysNo(sysNo);
            return BindMallOrder(dsorder);
        }

        /// <summary>
        /// 绑定第三方订单
        /// </summary>
        /// <param name="dsOrder">升舱订单</param>
        /// <returns>第三方订单</returns>
        ///  <remarks>2013-11-22 朱家宏 实现</remarks>
        private Result<UpGradeOrder> BindMallOrder(DsOrder dsOrder)
        {
            //var dsOrderItems = DsOrderBo.Instance.GetDsOrderItems(dsOrder.SysNo);
            var soOrder = DataAccess.Order.ISoOrderDao.Instance.GetByTransactionSysNo(dsOrder.OrderTransactionSysNo);
            if (soOrder == null)
                soOrder = new Model.SoOrder();
            var soOrderItems = DataAccess.Order.ISoOrderItemDao.Instance.GetOrderItemsWithErpCodeByOrderSysNo(soOrder.SysNo);
            var soReceiveAddress = BLL.Order.SoOrderBo.Instance.GetOrderReceiveAddress(soOrder.ReceiveAddressSysNo);
            if (soReceiveAddress == null)
            {
                soReceiveAddress = new SoReceiveAddress();
            }
            //data mapping
            //绑定订单基本信息和收货人信息
            var mallOrderInfo = new UpGradeOrder
            {
                HytOrderDealer = new HytOrderDealerInfo
                {
                    DeliveryStatus = dsOrder.Status.ToString(),
                    DsOrderSysNo = dsOrder.SysNo,
                    HytOrderId = soOrder.SysNo.ToString(),
                    HytOrderTime = dsOrder.UpgradeTime,
                    HytPayStatus = soOrder.PayStatus,
                    HytPayTime = dsOrder.PayTime,
                    HytPayment = soOrder.OrderAmount,
                    LogisticsTime = dsOrder.DeliveryTime,
                    DealerMessage = soOrder.InternalRemarks,
                    DealerMallSysNo = dsOrder.DealerMallSysNo,
                    OrderTransactionSysNo = dsOrder.OrderTransactionSysNo
                },
                MallOrderBuyer = new MallOrderBuyerInfo
                {
                    BuyerMessage = soOrder.CustomerMessage,
                    BuyerNick = dsOrder.BuyerNick,
                    MallOrderId = dsOrder.MallOrderId,
                    //OrderType = 0,
                    SellerMessage = ""

                },
                MallOrderPayment = new MallOrderPaymentInfo
                {
                    AlipayNo = "",
                    DiscountFee = dsOrder.DiscountAmount,
                    Payment = dsOrder.Payment,
                    PayTime = dsOrder.PayTime,
                    PostFee = dsOrder.PostFee,
                    ServiceFee = dsOrder.ServiceFee
                },
                MallOrderReceive = new MallOrderReceiveInfo
                {
                    AreaSysNo = soReceiveAddress.AreaSysNo,
                    City = dsOrder.City,
                    District = dsOrder.County,
                    Mobile = soReceiveAddress.MobilePhoneNumber,
                    PostCode = soReceiveAddress.ZipCode,
                    Province = dsOrder.Province,
                    ReceiveAddress = soReceiveAddress.StreetAddress,
                    ReceiveContact = soReceiveAddress.Name,
                    TelPhone = soReceiveAddress.PhoneNumber
                },
                UpGradeOrderItems = new List<UpGradeOrderItem>()
            };


            foreach (var item in soOrderItems)
            {
                //绑定订购商品信息
                mallOrderInfo.UpGradeOrderItems.Add(new UpGradeOrderItem
                {
                    HytProductErpCode = item.ErpCode,
                    HytPrice = item.SalesUnitPrice,
                    HytProductName = item.ProductName,
                    Quantity = item.Quantity,
                    ProductSalesType=item.GroupName=="淘宝赠品"?CustomerStatus.商品销售类型.赠品.GetHashCode():item.ProductSalesType
                    //MallPrice = item.SalesUnitPrice
                });
            }
            var result = new Result<UpGradeOrder>
            {
                Data = mallOrderInfo,
                Status = true,
                StatusCode = 0
            };
            return result;

        }

        /// <summary>
        /// 获取单条升舱订单信息
        /// </summary>
        /// <param name="orderId">淘宝订单编号</param>
        /// <returns>json</returns>
        /// <remarks>2013-8-28 陶辉 创建</remarks>
        /// <remarks>2013-09-06 朱家宏 实现</remarks>
        public Result<UpGradeOrder> GetMallOrder(string orderId)
        {
            var dsOrder = DsOrderBo.Instance.GetDsOrderByMallOrderId(orderId);
            return BindMallOrder(dsOrder);
        }

        #endregion

        /// <summary>
        /// 商城订单导入商城
        /// </summary>
        /// <param name="order">商城订单实体</param>
        /// <returns>订单导入结果</returns>
        /// <remarks>2013-8-28 陶辉 创建</remarks>
        public Result ImportMallOrder(UpGradeOrder order)
        {
            //1、判断已匹配的区域编号是否存在
            //2、判断已匹配的商城商品编号是否存在
            //3、B店判断已匹配商品编号是否已维护当前分销商价格
            //4、若该订单的收货手机已在商城注册，则直接使用该商城账号创建商城订单，若该收货手机未注册商城，首先使用该手机注册商城。
            //5、若C店订单，导入后商城订单金额为0，商品明细价格为0，同时自动产生一条收款金额为0的收款单
            //6、若B店订单，导入后商城订单商品明细价格为商城系统维护的升舱结算价格，同时产生一条收款金额为结算金额的收款单
            //7、发送升舱成功短信，若新注册用户，同时发送商城账号密码。                    
            var result = new Result() { Status = true };

            if (!DsOrderBo.Instance.ExistsDsOrder(order.HytOrderDealer.DealerMallSysNo, order.MallOrderBuyer.MallOrderId))
            {
                var pager = new Pager<PdProduct>() { PageSize = 999999, CurrentPage = 1 };
                BLL.Product.PdProductBo.Instance.GetPdProductList(pager);
                var defaultorder = order;
                Hyt.Model.CrCustomer cr = null;
                var isNewUser = true;
                string strPassword = "123456";//初始密码
                var options = new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadCommitted,
                    Timeout = TransactionManager.DefaultTimeout
                };
                #region 会员
                using (var trancustomer = new System.Transactions.TransactionScope(TransactionScopeOption.Required, options))//会员创建事物
                {
                    var customerlst = Hyt.BLL.Order.SoOrderBo.Instance.SearchCustomer(defaultorder.MallOrderReceive.Mobile);
                    if (customerlst != null && customerlst.Count > 0)
                    {
                        cr = customerlst.First();
                        isNewUser = false;
                    }
                    else //创建会员
                    {
                        cr = new Model.CrCustomer()
                        {
                            Account = defaultorder.MallOrderReceive.Mobile,
                            MobilePhoneNumber = defaultorder.MallOrderReceive.Mobile,
                            AreaSysNo = defaultorder.MallOrderReceive.AreaSysNo,
                            Gender = (int)Hyt.Model.WorkflowStatus.CustomerStatus.性别.保密,
                            EmailStatus = (int)Hyt.Model.WorkflowStatus.CustomerStatus.邮箱状态.未验证,
                            LevelSysNo = Hyt.Model.SystemPredefined.CustomerLevel.初级,
                            Name = defaultorder.MallOrderReceive.ReceiveContact,
                            NickName = defaultorder.MallOrderBuyer.BuyerNick,
                            RegisterDate = DateTime.Now,
                            Password = strPassword, // Hyt.Util.EncryptionUtil.EncryptWithMd5AndSalt(strPassword), 余勇修改 2014-09-12
                            Status = (int)Hyt.Model.WorkflowStatus.CustomerStatus.会员状态.有效,
                            MobilePhoneStatus = (int)Hyt.Model.WorkflowStatus.CustomerStatus.手机状态.未验证,
                            RegisterSource = (int)Hyt.Model.WorkflowStatus.CustomerStatus.注册来源.分销工具,
                            RegisterSourceSysNo = defaultorder.HytOrderDealer.DealerSysNo.ToString(),
                            StreetAddress = defaultorder.MallOrderReceive.ReceiveAddress,
                            IsReceiveShortMessage = (int)CustomerStatus.是否接收短信.是,
                            IsReceiveEmail = (int)CustomerStatus.是否接收邮件.是,
                            LastLoginDate = DateTime.Now,
                            Birthday = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                            CreatedDate = DateTime.Now,
                        };
                        Hyt.Model.CrReceiveAddress crr = new Model.CrReceiveAddress()
                        {
                            AreaSysNo = defaultorder.MallOrderReceive.AreaSysNo,
                            Name = defaultorder.MallOrderReceive.ReceiveContact,
                            MobilePhoneNumber = defaultorder.MallOrderReceive.Mobile,
                            StreetAddress = defaultorder.MallOrderReceive.ReceiveAddress,
                            IsDefault = 1
                        };
                        Hyt.BLL.Order.SoOrderBo.Instance.CreateCustomer(cr, crr);
                    }
                    trancustomer.Complete();//会员创建事物
                }
                if (cr == null || cr.SysNo < 1)
                {
                    result.Status = false;
                    result.Message = "会员信息读取失败";
                    return result;
                }
                #endregion

                #region 订单及订单明细
                bool IsSelfSend = false;//是否自己的物流来配送
                var AreaInfo = Hyt.BLL.Basic.BasicAreaBo.Instance.GetArea(defaultorder.MallOrderReceive.AreaSysNo);
                //if (AreaInfo != null)
                //{
                //    IsSelfSend = Hyt.BLL.Logistics.LgDeliveryScopeBo.Instance.IsInScope(AreaInfo.ParentSysNo, new Coordinate() { X = map_x, Y = map_y });
                //}
                Hyt.Model.SoOrder hytorder = new Model.SoOrder()
                {
                    CustomerSysNo = cr.SysNo,
                    OrderNo = BLL.Basic.ReceiptNumberBo.Instance.GetOrderNo(),
                    LevelSysNo = cr.LevelSysNo,
                    CustomerMessage = defaultorder.MallOrderBuyer.BuyerMessage,
                    InternalRemarks = defaultorder.MallOrderBuyer.SellerMessage,
                    CreateDate = DateTime.Now,
                    Status = (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.待审核,
                    OnlineStatus = Constant.OlineStatusType.待审核,
                    SalesType = (int)Hyt.Model.WorkflowStatus.OrderStatus.销售方式.经销订单,
                    OrderSource = (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单来源.分销商升舱,
                    OrderSourceSysNo = defaultorder.HytOrderDealer.DealerMallSysNo,//下单来源为 分销商商城编号
                    PayStatus = defaultorder.HytOrderDealer.HytPayStatus,
                    DeliveryTypeSysNo = IsSelfSend ? Hyt.Model.SystemPredefined.DeliveryType.普通百城当日达 : Hyt.Model.SystemPredefined.DeliveryType.第三方快递,
                    PayTypeSysNo = Hyt.Model.SystemPredefined.PaymentType.分销商预存,
                    FreightAmount = defaultorder.MallOrderPayment.PostFee,
                    AuditorDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                    CancelDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                    LastUpdateDate = DateTime.Now,
                    Stamp = DateTime.Now,

                };
                var warehouse = Hyt.BLL.Warehouse.WhWarehouseBo.Instance.GetWhWareHouse(defaultorder.MallOrderReceive.AreaSysNo, null, hytorder.DeliveryTypeSysNo, Hyt.Model.WorkflowStatus.WarehouseStatus.仓库状态.启用).FirstOrDefault();
                if (warehouse != null)
                {
                    hytorder.DefaultWarehouseSysNo = warehouse.SysNo;
                }
                Hyt.Model.SoReceiveAddress hytreceive = new Model.SoReceiveAddress()
                {
                    AreaSysNo = defaultorder.MallOrderReceive.AreaSysNo,
                    Name = defaultorder.MallOrderReceive.ReceiveContact,
                    MobilePhoneNumber = defaultorder.MallOrderReceive.Mobile,
                    StreetAddress = defaultorder.MallOrderReceive.ReceiveAddress,
                    PhoneNumber = defaultorder.MallOrderReceive.TelPhone
                };
                List<Hyt.Model.SoOrderItem> hytitems = new List<Model.SoOrderItem>();
                decimal payMoney = 0;
                bool isCombineProduct = false;//是否合并相同产品的明细

                foreach (UpGradeOrderItem mitem in order.UpGradeOrderItems)
                {
                    decimal price = mitem.HytPrice;
                    Hyt.Model.SoOrderItem si = hytitems.FirstOrDefault(m => m.ProductSysNo == mitem.HytProductSysNo);
                    if (!isCombineProduct || si == null)//不允许相同产品明细合并或者产品不再购物车中
                    {
                        if (mitem.ProductSalesType == (int)CustomerStatus.商品销售类型.赠品)//赠品
                        {
                            si = new Model.SoOrderItem()
                            {
                                ProductSysNo = mitem.HytProductSysNo,
                                ProductName = mitem.HytProductName,
                                Quantity = mitem.Quantity,
                                OriginalPrice = mitem.MallPrice,//带入作为原单价
                                SalesUnitPrice = 0,
                                SalesAmount = 0,
                                ProductSalesType = (int)CustomerStatus.商品销售类型.普通,
                                GroupName = "淘宝赠品"
                            };
                            mitem.MallPrice = 0;//升仓明细价格为0
                        }
                        else
                        {
                            #region 计算价格信息
                            if (order.HytOrderDealer.IsPreDeposit == (int)Hyt.Model.WorkflowStatus.DistributionStatus.商城是否使用预存款.是 && order.HytOrderDealer.IsSelfSupport == (int)Hyt.Model.WorkflowStatus.DistributionStatus.是否自营.否)
                            {
                                price = Hyt.BLL.MallSeller.DsOrderBo.Instance.GetDsSpecialPrice(order.HytOrderDealer.DealerSysNo, mitem.HytProductSysNo);//再次获取一下分销商价格，防止客户端数据窜改
                            }



                            si = new Model.SoOrderItem()
                            {
                                ProductSysNo = mitem.HytProductSysNo,
                                ProductName = mitem.HytProductName,
                                Quantity = mitem.Quantity,
                                OriginalPrice = price,
                                SalesUnitPrice = price,
                                SalesAmount = price * mitem.Quantity,
                                ProductSalesType = (int)CustomerStatus.商品销售类型.普通
                            };

                            var productInfo = pager.Rows.Where(x => x.ErpCode == mitem.MallProductCode).SingleOrDefault();
                            if (productInfo != null)
                            {
                                si.ProductSysNo = productInfo.SysNo;
                                si.ProductName = productInfo.ProductName;
                                mitem.HytProductSysNo = productInfo.SysNo;
                                mitem.HytProductName = productInfo.ProductName;
                            }

                            if (order.HytOrderDealer.IsSelfSupport == (int)Hyt.Model.WorkflowStatus.DistributionStatus.是否自营.是)//自营存在改价格情况
                            {
                                si.SalesAmount = mitem.MallAmount;
                            }
                            #endregion
                            Hyt.BLL.MallSeller.DsOrderBo.Instance.SetProductAssociation(order.HytOrderDealer.DealerMallSysNo, mitem.MallProductCode, mitem.MallProductAttrId, mitem.HytProductSysNo);  //非赠品设置商城产品与分销产品映射
                        }
                        hytitems.Add(si);//加入购物车
                        payMoney += si.SalesAmount;
                        mitem.HytOrderItem = si;//升仓订单明细与商城订单明细关联
                    }
                    else
                    {
                        si.Quantity += mitem.Quantity;
                        if (order.HytOrderDealer.IsSelfSupport == (int)Hyt.Model.WorkflowStatus.DistributionStatus.是否自营.是)//自营存在改价格情况
                        {
                            si.SalesAmount += mitem.MallAmount;
                            payMoney += mitem.MallAmount;
                        }
                        else
                        {
                            si.SalesAmount += si.SalesUnitPrice * mitem.Quantity;
                            payMoney += si.SalesUnitPrice * mitem.Quantity;
                        }
                    }
                }
                    
                
                hytorder.OrderAmount = payMoney;
                hytorder.CashPay = payMoney;
                hytorder.ProductAmount = payMoney;
                #endregion.


                var dealerMall = DsOrderBo.Instance.GetDsDealerMall(defaultorder.HytOrderDealer.DealerMallSysNo);
                var operatorName = dealerMall != null ? dealerMall.ShopName : string.Empty;


                //取得订单图片标识
                hytorder.ImgFlag = DsOrderBo.Instance.GetImgFlag(defaultorder.HytOrderDealer.DealerMallSysNo,
                    defaultorder.MallOrderBuyer.SellerMessage, defaultorder.MallOrderBuyer.SellerFlag,
                    defaultorder.UpGradeOrderItems);

                using (var tran = new System.Transactions.TransactionScope(TransactionScopeOption.Required, options))
                {
                    #region 数据提交
                    var r = Hyt.BLL.Order.SoOrderBo.Instance.ImportSoOrder(hytorder, hytreceive, hytitems.ToArray(), operatorName);
                    result.Status = r.Status;
                    result.StatusCode = r.StatusCode;
                    result.Message = r.Message;//事物编号
                    //写入分销商订单信息
                    if (r.Status)
                    {
                        #region 升舱订单表
                     
                            // 获取分销商城类型
                        var mall = DsOrderBo.Instance.GetDsDealerMall(order.HytOrderDealer.DealerMallSysNo);
                            var mallType = DsMallTypeBo.Instance.GetDsMallType(mall.MallTypeSysNo);
                            var dso = new Hyt.Model.DsOrder()
                            {
                                City = order.MallOrderReceive.City,
                                County = order.MallOrderReceive.District,
                                Province = order.MallOrderReceive.Province,
                                StreetAddress = order.MallOrderReceive.ReceiveAddress,
                                BuyerNick = order.MallOrderBuyer.BuyerNick,
                                DealerMallSysNo = order.HytOrderDealer.DealerMallSysNo,
                                DiscountAmount = order.MallOrderPayment.DiscountFee,
                                MallOrderId = mallType.SysNo == (int)DistributionStatus.商城类型预定义.淘宝分销 ? order.MallOrderBuyer.MallPurchaseId : order.MallOrderBuyer.MallOrderId,
                                PostFee = order.MallOrderPayment.PostFee,
                                Payment = order.MallOrderPayment.Payment,
                                ServiceFee = order.MallOrderPayment.ServiceFee,
                                PayTime = order.MallOrderPayment.PayTime,
                                UpgradeTime = DateTime.Now,
                                Status = (int)Hyt.Model.WorkflowStatus.DistributionStatus.升舱订单状态.升舱中,
                                DeliveryTime = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                                SignTime = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                                LastCallbackTime = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                            };
                            List<Hyt.Model.Transfer.CBDsOrderItem> cbs = new List<Hyt.Model.Transfer.CBDsOrderItem>();
                            foreach (UpGradeOrderItem mii in order.UpGradeOrderItems)
                            {
                                Hyt.Model.DsOrderItem n = new Model.DsOrderItem()
                                {
                                    Quantity = mii.Quantity,
                                    Price = mii.MallPrice,
                                    MallProductId = mii.MallProductCode,
                                    MallProductName = mii.MallProductName,
                                    DiscountAmount = mii.DiscountFee,
                                    MallProductAttribute = mii.MallProductAttrs
                                };

                                if (!string.IsNullOrEmpty(mii.MallOrderItemId))//如果数据转换错误就不写入淘宝订单明细编号
                                {
                                    try
                                    {
                                        n.MallItemNo = long.Parse(mii.MallOrderItemId);
                                    }
                                    catch { }
                                }
                                Hyt.Model.Transfer.CBDsOrderItem f = new Model.Transfer.CBDsOrderItem(n);
                                f.CurrectDsOrderItemAssociations = new DsOrderItemAssociation()//升仓明细关联
                                {
                                    OrderTransactionSysNo = mii.HytOrderItem.TransactionSysNo,//事物编号
                                    SoOrderItemSysNo = mii.HytOrderItem.SysNo                //明细编号
                                };
                                cbs.Add(f);
                            }
                            DsOrderBo.Instance.SaveDsOrder(dso, cbs, r.StatusCode);
                        
                        #endregion
                        if (hytorder.PayStatus == (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单支付状态.已支付 && hytorder.CashPay > 0)
                        {
                            Hyt.BLL.MallSeller.DsOrderBo.Instance.FreezeDsPrePayment(r.StatusCode, defaultorder.HytOrderDealer.DealerSysNo, hytorder.CashPay, CurrentUser.SysNo);//冻结金额
                        }
                    }
                    tran.Complete();
                    #endregion

                }

            }
                   
            return result;
        }

        /// <summary>
        /// 商城订单合并导入商城
        /// </summary>
        /// <param name="orders">商城订单实体列表</param>
        /// <param name="map_x">地图横坐标</param>
        /// <param name="map_y">地图纵坐标</param>
        /// <returns>订单导入结果</returns>
        /// <remarks>2013-8-28 陶辉 创建</remarks>
        /// <remarks>2013-09-09 朱成果 修改</remarks>
        /// <remarks>2013-10-12 黄志勇 修改</remarks>
        public Result CombineImportMallOrder(List<UpGradeOrder> orders, double map_x = 0, double map_y = 0)
        {
            //1、判断已匹配的区域编号是否存在
            //2、判断已匹配的商城商品编号是否存在
            //3、B店判断已匹配商品编号是否已维护当前分销商价格
            //4、若该订单的收货手机已在商城注册，则直接使用该商城账号创建商城订单，若该收货手机未注册商城，首先使用该手机注册商城。
            //5、
            //6、若B店订单，导入后商城订单商品明细价格为商城系统维护的升舱结算价格，同时产生一条收款金额为结算金额的收款单
            //7、发送升舱成功短信，若新注册用户，同时发送商城账号密码。
            //8、若合并的订单中存在升舱服务费订单，则需要分销商将升舱服务费填入正常订单后升舱
            var result = new Result() { Status = false };

            if (orders == null || orders.Count <= 0)
            {
                result.Status = false;
                result.Message ="无三方订单！";
                return result;
            }


            var pager=new Pager<PdProduct>(){PageSize=999999,CurrentPage=1};
            BLL.Product.PdProductBo.Instance.GetPdProductList(pager);
                          
            foreach (UpGradeOrder mi in orders)
            {
                if (DsOrderBo.Instance.ExistsDsOrder(mi.HytOrderDealer.DealerMallSysNo, mi.MallOrderBuyer.MallOrderId))
                {
                    result.Status = false;
                    result.Message = "存在已经升舱的订单:" + mi.MallOrderBuyer.MallOrderId;
                    result.StatusCode = -999;//
                    return result;
                }
            }
            try
            {
                var defaultorder = orders.First();
                Hyt.Model.CrCustomer cr = null;
                var isNewUser = true;
                string strPassword = "123456";//初始密码
                var options = new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadCommitted,
                    Timeout = TransactionManager.DefaultTimeout
                };
                #region 会员
                using (var trancustomer = new System.Transactions.TransactionScope(TransactionScopeOption.Required, options))//会员创建事物
                {
                    var customerlst = Hyt.BLL.Order.SoOrderBo.Instance.SearchCustomer(defaultorder.MallOrderReceive.Mobile);
                    if (customerlst != null && customerlst.Count > 0)
                    {
                        cr = customerlst.First();
                        isNewUser = false;
                    }
                    else //创建会员
                    {
                        cr = new Model.CrCustomer()
                        {
                            Account = defaultorder.MallOrderReceive.Mobile,
                            MobilePhoneNumber = defaultorder.MallOrderReceive.Mobile,
                            AreaSysNo = defaultorder.MallOrderReceive.AreaSysNo,
                            Gender = (int)Hyt.Model.WorkflowStatus.CustomerStatus.性别.保密,
                            EmailStatus = (int)Hyt.Model.WorkflowStatus.CustomerStatus.邮箱状态.未验证,
                            LevelSysNo = Hyt.Model.SystemPredefined.CustomerLevel.初级,
                            Name = defaultorder.MallOrderReceive.ReceiveContact,
                            NickName = defaultorder.MallOrderBuyer.BuyerNick,
                            RegisterDate = DateTime.Now,
                            Password = strPassword, // Hyt.Util.EncryptionUtil.EncryptWithMd5AndSalt(strPassword), 余勇修改 2014-09-12
                            Status = (int)Hyt.Model.WorkflowStatus.CustomerStatus.会员状态.有效,
                            MobilePhoneStatus = (int)Hyt.Model.WorkflowStatus.CustomerStatus.手机状态.未验证,
                            RegisterSource = (int)Hyt.Model.WorkflowStatus.CustomerStatus.注册来源.分销工具,
                            RegisterSourceSysNo = defaultorder.HytOrderDealer.DealerSysNo.ToString(),
                            StreetAddress = defaultorder.MallOrderReceive.ReceiveAddress,
                            IsReceiveShortMessage = (int)CustomerStatus.是否接收短信.是,
                            IsReceiveEmail = (int)CustomerStatus.是否接收邮件.是,
                            LastLoginDate=DateTime.Now,
                            Birthday = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                            CreatedDate=DateTime.Now,
                        };
                        Hyt.Model.CrReceiveAddress crr = new Model.CrReceiveAddress()
                        {
                            AreaSysNo = defaultorder.MallOrderReceive.AreaSysNo,
                            Name = defaultorder.MallOrderReceive.ReceiveContact,
                            MobilePhoneNumber = defaultorder.MallOrderReceive.Mobile,
                            StreetAddress = defaultorder.MallOrderReceive.ReceiveAddress,
                            IsDefault = 1
                        };
                        Hyt.BLL.Order.SoOrderBo.Instance.CreateCustomer(cr, crr);
                    }
                    trancustomer.Complete();//会员创建事物
                }
                if(cr==null||cr.SysNo<1)
                {
                    result.Status = false;
                    result.Message = "会员信息读取失败";
                    return result;
                }
                #endregion

                #region 订单及订单明细
                bool IsSelfSend = false;//是否自己的物流来配送
                var AreaInfo = Hyt.BLL.Basic.BasicAreaBo.Instance.GetArea(defaultorder.MallOrderReceive.AreaSysNo);
                //if (AreaInfo != null)
                //{
                //    IsSelfSend = Hyt.BLL.Logistics.LgDeliveryScopeBo.Instance.IsInScope(AreaInfo.ParentSysNo, new Coordinate() { X = map_x, Y = map_y });
                //}
                Hyt.Model.SoOrder hytorder = new Model.SoOrder()
                {
                    CustomerSysNo = cr.SysNo,
                    OrderNo=BLL.Basic.ReceiptNumberBo.Instance.GetOrderNo(),
                    LevelSysNo = cr.LevelSysNo,
                    CustomerMessage = defaultorder.MallOrderBuyer.BuyerMessage,
                    InternalRemarks = defaultorder.MallOrderBuyer.SellerMessage,
                    CreateDate = DateTime.Now,
                    Status = (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.待审核,
                    OnlineStatus = Constant.OlineStatusType.待审核,
                    SalesType = (int)Hyt.Model.WorkflowStatus.OrderStatus.销售方式.经销订单,
                    OrderSource = (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单来源.分销商升舱,
                    OrderSourceSysNo = defaultorder.HytOrderDealer.DealerMallSysNo,//下单来源为 分销商商城编号
                    PayStatus = defaultorder.HytOrderDealer.HytPayStatus,
                    DeliveryTypeSysNo = IsSelfSend ? Hyt.Model.SystemPredefined.DeliveryType.普通百城当日达 : Hyt.Model.SystemPredefined.DeliveryType.第三方快递,
                    PayTypeSysNo = Hyt.Model.SystemPredefined.PaymentType.分销商预存,
                    FreightAmount = defaultorder.MallOrderPayment.PostFee,
                    AuditorDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                    CancelDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                    LastUpdateDate=DateTime.Now,
                    Stamp =DateTime.Now,

                };
                var warehouse = Hyt.BLL.Warehouse.WhWarehouseBo.Instance.GetWhWareHouse(defaultorder.MallOrderReceive.AreaSysNo, null, hytorder.DeliveryTypeSysNo, Hyt.Model.WorkflowStatus.WarehouseStatus.仓库状态.启用).FirstOrDefault();
                if (warehouse != null)
                {
                    hytorder.DefaultWarehouseSysNo = warehouse.SysNo;
                }
                Hyt.Model.SoReceiveAddress hytreceive = new Model.SoReceiveAddress()
                {
                    AreaSysNo = defaultorder.MallOrderReceive.AreaSysNo,
                    Name = defaultorder.MallOrderReceive.ReceiveContact,
                    MobilePhoneNumber = defaultorder.MallOrderReceive.Mobile,
                    StreetAddress = defaultorder.MallOrderReceive.ReceiveAddress,
                    PhoneNumber = defaultorder.MallOrderReceive.TelPhone
                };
                List<Hyt.Model.SoOrderItem> hytitems = new List<Model.SoOrderItem>();
                decimal payMoney = 0;
                bool isCombineProduct = false;//是否合并相同产品的明细
                foreach (UpGradeOrder item in orders)
                {
                    foreach (UpGradeOrderItem mitem in item.UpGradeOrderItems)
                    {
                        decimal price = mitem.HytPrice;
                        Hyt.Model.SoOrderItem si = hytitems.FirstOrDefault(m => m.ProductSysNo == mitem.HytProductSysNo);
                        if (!isCombineProduct || si == null)//不允许相同产品明细合并或者产品不再购物车中
                        {
                            if (mitem.ProductSalesType == (int)CustomerStatus.商品销售类型.赠品)//赠品
                            {
                                si = new Model.SoOrderItem()
                                {
                                    ProductSysNo = mitem.HytProductSysNo,
                                    ProductName = mitem.HytProductName,
                                    Quantity = mitem.Quantity,
                                    OriginalPrice = mitem.MallPrice,//带入作为原单价
                                    SalesUnitPrice =0,
                                    SalesAmount = 0,
                                    ProductSalesType = (int)CustomerStatus.商品销售类型.普通,
                                    GroupName="淘宝赠品"
                                };
                                mitem.MallPrice = 0;//升仓明细价格为0
                            }
                            else
                            {
                                #region 计算价格信息
                                if (item.HytOrderDealer.IsPreDeposit == (int)Hyt.Model.WorkflowStatus.DistributionStatus.商城是否使用预存款.是 && item.HytOrderDealer.IsSelfSupport == (int)Hyt.Model.WorkflowStatus.DistributionStatus.是否自营.否)
                                {
                                    price = Hyt.BLL.MallSeller.DsOrderBo.Instance.GetDsSpecialPrice(item.HytOrderDealer.DealerSysNo, mitem.HytProductSysNo);//再次获取一下分销商价格，防止客户端数据窜改
                                }

                                
                               
                                si = new Model.SoOrderItem()
                                {
                                    ProductSysNo = mitem.HytProductSysNo,
                                    ProductName = mitem.HytProductName,
                                    Quantity = mitem.Quantity,
                                    OriginalPrice = price,
                                    SalesUnitPrice = price,
                                    SalesAmount = price * mitem.Quantity,
                                    ProductSalesType = (int)CustomerStatus.商品销售类型.普通
                                };

                                var productInfo = pager.Rows.Where(x => x.ErpCode == mitem.MallProductCode).SingleOrDefault();
                                if (productInfo != null)
                                {
                                    si.ProductSysNo=productInfo.SysNo;
                                    si.ProductName = productInfo.ProductName;
                                    mitem.HytProductSysNo = productInfo.SysNo;
                                    mitem.HytProductName = productInfo.ProductName;
                                }

                                if (item.HytOrderDealer.IsSelfSupport == (int)Hyt.Model.WorkflowStatus.DistributionStatus.是否自营.是)//自营存在改价格情况
                                {
                                    si.SalesAmount = mitem.MallAmount;
                                }
                                #endregion
                                Hyt.BLL.MallSeller.DsOrderBo.Instance.SetProductAssociation(item.HytOrderDealer.DealerMallSysNo, mitem.MallProductCode, mitem.MallProductAttrId, mitem.HytProductSysNo);  //非赠品设置商城产品与分销产品映射
                            }
                            hytitems.Add(si);//加入购物车
                            payMoney += si.SalesAmount;
                            mitem.HytOrderItem = si;//升仓订单明细与商城订单明细关联
                        }
                        else
                        {
                            si.Quantity += mitem.Quantity;
                            if (item.HytOrderDealer.IsSelfSupport == (int)Hyt.Model.WorkflowStatus.DistributionStatus.是否自营.是)//自营存在改价格情况
                            {
                                si.SalesAmount += mitem.MallAmount;
                                payMoney += mitem.MallAmount;
                            }
                            else
                            {
                                si.SalesAmount += si.SalesUnitPrice * mitem.Quantity;
                                payMoney += si.SalesUnitPrice * mitem.Quantity;
                            }
                        }
                    }
                }
                hytorder.OrderAmount = payMoney;
                hytorder.CashPay = payMoney;
                hytorder.ProductAmount = payMoney;
                #endregion.

                var dealerMall = DsOrderBo.Instance.GetDsDealerMall(defaultorder.HytOrderDealer.DealerMallSysNo);
                var operatorName = dealerMall != null ? dealerMall.ShopName : string.Empty;
            

                //取得订单图片标识
                hytorder.ImgFlag = DsOrderBo.Instance.GetImgFlag(defaultorder.HytOrderDealer.DealerMallSysNo,
                    defaultorder.MallOrderBuyer.SellerMessage, defaultorder.MallOrderBuyer.SellerFlag,
                    defaultorder.UpGradeOrderItems);

                using (var tran = new System.Transactions.TransactionScope(TransactionScopeOption.Required, options))
                {
                    #region 数据提交
                    var r = Hyt.BLL.Order.SoOrderBo.Instance.ImportSoOrder(hytorder, hytreceive, hytitems.ToArray(), operatorName);
                    result.Status = r.Status;
                    result.StatusCode = r.StatusCode;
                    result.Message = r.Message;//事物编号
                    //写入分销商订单信息
                    if (r.Status)
                    {
                        #region 升舱订单表
                        foreach (UpGradeOrder mi in orders)
                        {
                            // 获取分销商城类型
                            var mall = DsOrderBo.Instance.GetDsDealerMall(mi.HytOrderDealer.DealerMallSysNo);
                            var mallType = DsMallTypeBo.Instance.GetDsMallType(mall.MallTypeSysNo);
                            var dso = new Hyt.Model.DsOrder()
                            {
                                City = mi.MallOrderReceive.City,
                                County = mi.MallOrderReceive.District,
                                Province = mi.MallOrderReceive.Province,
                                StreetAddress = mi.MallOrderReceive.ReceiveAddress,
                                BuyerNick = mi.MallOrderBuyer.BuyerNick,
                                DealerMallSysNo = mi.HytOrderDealer.DealerMallSysNo,
                                DiscountAmount = mi.MallOrderPayment.DiscountFee,
                                MallOrderId =  mallType.SysNo == (int)DistributionStatus.商城类型预定义.淘宝分销 ? mi.MallOrderBuyer.MallPurchaseId : mi.MallOrderBuyer.MallOrderId,
                                PostFee = mi.MallOrderPayment.PostFee,
                                Payment = mi.MallOrderPayment.Payment,
                                ServiceFee = mi.MallOrderPayment.ServiceFee,
                                PayTime = mi.MallOrderPayment.PayTime,
                                UpgradeTime = DateTime.Now,
                                Status = (int)Hyt.Model.WorkflowStatus.DistributionStatus.升舱订单状态.升舱中,
                                DeliveryTime=(DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                                SignTime = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                                LastCallbackTime = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                            };
                            List<Hyt.Model.Transfer.CBDsOrderItem> cbs = new List<Hyt.Model.Transfer.CBDsOrderItem>();
                            foreach (UpGradeOrderItem mii in mi.UpGradeOrderItems)
                            {
                                Hyt.Model.DsOrderItem n = new Model.DsOrderItem()
                                {
                                    Quantity = mii.Quantity,
                                    Price = mii.MallPrice,
                                    MallProductId = mii.MallProductCode,
                                    MallProductName = mii.MallProductName,
                                    DiscountAmount = mii.DiscountFee,
                                    MallProductAttribute = mii.MallProductAttrs
                                };
                                if (!string.IsNullOrEmpty(mii.MallOrderItemId))//如果数据转换错误就不写入淘宝订单明细编号
                                {
                                    try
                                    {
                                        n.MallItemNo = long.Parse(mii.MallOrderItemId);
                                    }
                                    catch { }
                                }
                                Hyt.Model.Transfer.CBDsOrderItem f = new Model.Transfer.CBDsOrderItem(n);
                                f.CurrectDsOrderItemAssociations = new DsOrderItemAssociation()//升仓明细关联
                                {
                                    OrderTransactionSysNo = mii.HytOrderItem.TransactionSysNo,//事物编号
                                    SoOrderItemSysNo = mii.HytOrderItem.SysNo                //明细编号
                                };
                                cbs.Add(f);
                            }
                            DsOrderBo.Instance.SaveDsOrder(dso, cbs, r.StatusCode);
                        }
                        #endregion
                        if (hytorder.PayStatus == (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单支付状态.已支付 && hytorder.CashPay > 0)
                        {
                            Hyt.BLL.MallSeller.DsOrderBo.Instance.FreezeDsPrePayment(r.StatusCode, defaultorder.HytOrderDealer.DealerSysNo, hytorder.CashPay, CurrentUser.SysNo);//冻结金额
                        }
                    }
                    tran.Complete();
                    #endregion

                }
                #region 发送短信
                if (result.Status)
                {
                    var dealerMallSysNo = defaultorder.HytOrderDealer.DealerMallSysNo;
                    var shopName = DsOrderBo.Instance.GetDsDealerMall(dealerMallSysNo).ShopName;
                    var mallOrderId = defaultorder.MallOrderBuyer.MallOrderId;
                    if (VHelper.Do(hytreceive.MobilePhoneNumber, VType.Mobile))
                    {
                        if (isNewUser)
                        {
                            //BLL.Extras.SmsBO.Instance.发送升舱成功短信(hytreceive.MobilePhoneNumber, shopName, mallOrderId,
                                                              //cr.Account, strPassword);
                        }
                        else
                        {
                            //升舱短信修改内容，添加商城订单号参数 余勇 2014-07-18
                            //BLL.Extras.SmsBO.Instance.发送升舱成功短信(hytreceive.MobilePhoneNumber, shopName, mallOrderId, result.StatusCode.ToString());
                        }
                    }
                    if (VHelper.Do(hytreceive.MobilePhoneNumber, VType.Email))
                    {
                        //BLL.Extras.EmailBo.Instance.发送升舱成功邮件(hytreceive.EmailAddress, hytorder.CustomerSysNo.ToString(), shopName, mallOrderId);
                    }
                }
                #endregion

                #region 后续EAS处理

                //var model = new DsEasAssociation()
                //{
                //    Code = string.Empty,
                //    Status = (int)DistributionStatus.分销商EAS关联状态.启用,
                //    CreatedBy = CurrentUser.SysNo,
                //    CreatedDate = DateTime.Now,
                //    LastUpdateBy = CurrentUser.SysNo,
                //    LastUpdateDate = DateTime.Now,
                //    DealerMallSysNo = defaultorder.HytOrderDealer.DealerMallSysNo,
                //    SellerNick = string.Empty
                //};
                //var dsDealerMall = DsOrderBo.Instance.GetDsDealerMall(defaultorder.HytOrderDealer.DealerMallSysNo);
                //if (dsDealerMall != null) model.SellerNick = dsDealerMall.ShopAccount;
                //DsEasBo.Instance.CheckEas(model);

                #endregion
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
                SysLog.Instance.Error(Hyt.Model.WorkflowStatus.LogStatus.系统日志来源.分销工具, "商城订单合并导入商城 " + ex.Message, ex);

            }
            return result;
        }

        /// <summary>
        /// 获取订单日志
        /// </summary>
        /// <param name="orderId">淘宝订单号</param>
        /// <returns>物流日志列表</returns>
        /// <remarks>2013-8-28 陶辉 创建</remarks>
        /// <remarks>2013-09-06 朱家宏 实现</remarks>
        public Result<List<HytOrderLog>> GetLogisticsLog(string orderId)
        {
            var dsOrder = BLL.MallSeller.DsOrderBo.Instance.GetDsOrderByMallOrderId(orderId);
            return GetLogisticsLogByTransactionSysNo(dsOrder.OrderTransactionSysNo);
        }

        /// <summary>
        /// 订单日志
        /// </summary>
        /// <param name="transactionSysNo">商城订单事物编号</param>
        /// <returns>订单日志</returns>
        /// <remarks>2013-09-26 朱成果 创建</remarks>
        public Result<List<HytOrderLog>> GetLogisticsLogByTransactionSysNo(string transactionSysNo)
        {
            var soOrder = DataAccess.Order.ISoOrderDao.Instance.GetByTransactionSysNo(transactionSysNo);
            if (soOrder == null)
                soOrder = new Model.SoOrder();

            var soLogs = BLL.Order.SoOrderBo.Instance.GetTransactionLogPageData(soOrder.SysNo, 1, 50);

            //mapping
            var list = soLogs.TData.Select(log => new HytOrderLog
            {
                LogContent = log.LogContent,
                OperateDate = log.OperateDate,
                Operator = log.Operator,
                SysNo = log.SysNo,
                TransactionSysNo = log.TransactionSysNo
            });
            var result = new Result<List<HytOrderLog>>
            {
                Data = list.ToList(),
                Status = true
            };

            return result;
        }

        /// <summary>
        /// 获取商城所有的省份
        /// </summary>
        /// <returns>省份列表</returns>
        /// <remarks>2013-9-3 陶辉 创建</remarks>
        /// <remarks>2013-9-4 朱成果 修改</remarks>
        public Result<List<BsArea>> GetHytProvinceList()
        {
            var result = new Result<List<BsArea>>() { Status = false };
            try
            {
                var list = Hyt.BLL.Basic.BasicAreaBo.Instance.SelectArea(0).Select(x =>
                          new BsArea
                          {
                              SysNo = x.SysNo,
                              ParentSysNo = x.ParentSysNo,
                              AreaName = x.AreaName,
                              AreaCode = x.AreaCode,
                              AreaLevel = x.AreaLevel
                          }).ToList();
                result.Data = list;
                result.Status = true;
                result.StatusCode = 0;
            }
            catch (Exception ex)
            {
                result.Message = "系统异常，请稍后重试!";
                SysLog.Instance.Error(Hyt.Model.WorkflowStatus.LogStatus.系统日志来源.分销工具, "获取商城所有的省份 " + ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 获取商城省份的所有城市
        /// </summary>
        /// <param name="provinceSysNo">省份编号</param>
        /// <returns>省份下面的市列表</returns>
        /// <remarks>2013－09-03 陶辉 创建</remarks>
        /// <remarks>2013-09-04 朱成果 修改</remarks>
        public Result<List<BsArea>> GetHytCityList(int provinceSysNo)
        {
            var result = new Result<List<BsArea>>() { Status = false };
            try
            {
                var list = Hyt.BLL.Basic.BasicAreaBo.Instance.SelectArea(provinceSysNo).Select(x =>
                          new BsArea
                          {
                              SysNo = x.SysNo,
                              ParentSysNo = x.ParentSysNo,
                              AreaName = x.AreaName,
                              AreaCode = x.AreaCode,
                              AreaLevel = x.AreaLevel
                          }).ToList();
                result.Data = list;
                result.Status = true;
                result.StatusCode = 0;
            }
            catch (Exception ex)
            {
                result.Message = "系统异常，请稍后重试!";
                SysLog.Instance.Error(Hyt.Model.WorkflowStatus.LogStatus.系统日志来源.分销工具, "获取商城省份的所有城市 " + ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 查询商城城市下的所有区域
        /// </summary>
        /// <param name="citySysNo">城市编号</param>
        /// <returns>获取城市下面的地区列表</returns>
        /// <remarks>2013－09-03 陶辉 创建</remarks>
        /// <remarks>2013-09-04 朱成果 修改</remarks>
        public Result<List<BsArea>> GetHytAreaList(int citySysNo)
        {
            var result = new Result<List<BsArea>>() { Status = false };
            try
            {
                var list = Hyt.BLL.Basic.BasicAreaBo.Instance.SelectArea(citySysNo).Select(x =>
                          new BsArea
                          {
                              SysNo = x.SysNo,
                              ParentSysNo = x.ParentSysNo,
                              AreaName = x.AreaName,
                              AreaCode = x.AreaCode,
                              AreaLevel = x.AreaLevel
                          }).ToList();
                result.Data = list;
                result.Status = true;
                result.StatusCode = 0;
            }
            catch (Exception ex)
            {
                result.Message = "系统异常，请稍后重试!";
                SysLog.Instance.Error(Hyt.Model.WorkflowStatus.LogStatus.系统日志来源.分销工具, "查询商城城市下的所有区域 " + ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 根据地区编号，获取省市区信息
        /// </summary>
        /// <param name="sysNo">地区编号</param>
        /// <param name="cityEntity">城市信息</param>
        /// <param name="areaEntity">地区信息</param>
        /// <returns>省市区信息</returns>
        /// <remarks> 2013-09-03 陶辉 创建 </remarks>
        /// <remarks>2013-09-04 朱成果 修改</remarks>
        public Result<BsArea> GetHytProvinceEntity(int sysNo, out BsArea cityEntity, out BsArea areaEntity)
        {

            var result = new Result<BsArea>() { Status = false };
            cityEntity = null;
            areaEntity = null;
            try
            {
                Model.BsArea cityEntity0;
                Model.BsArea areaEntity0;
                var provinceEntity = Hyt.BLL.Basic.BasicAreaBo.Instance.GetProvinceEntity(sysNo, out cityEntity0, out areaEntity0);
                result.Data = BsAreaToModel(provinceEntity);
                cityEntity = BsAreaToModel(cityEntity0);
                areaEntity = BsAreaToModel(areaEntity0);
            }
            catch (Exception ex)
            {
                result.Message = "系统异常，请稍后重试!";
                SysLog.Instance.Error(Hyt.Model.WorkflowStatus.LogStatus.系统日志来源.分销工具, " 根据地区编号，获取省市区信息 " + ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 根据省市区名称匹配商城地区编号
        /// </summary>
        /// <param name="provinceName">省</param>
        /// <param name="cityName">市</param>
        /// <param name="districtName">区</param>
        /// <returns>商城地区编号</returns>
        /// <remarks> 2013-09-03 陶辉 创建 </remarks>
        /// <remarks>2013-09-13 朱成果 修改</remarks>
        public Result<int> GetDistrictSysNo(string provinceName, string cityName, string districtName)
        {
            Result<int> result = new Result<int>() { Status = false };
            try
            {
                var obj = Hyt.BLL.Basic.BasicAreaBo.Instance.GetMatchDistrict(cityName, districtName);
                if (obj != null)
                {
                    result.Status = true;
                    result.Data = obj.SysNo;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;

        }

        /// <summary>
        /// 获取分销商升舱订单
        /// </summary>
        /// <param name="shopAccount">店铺账号</param>
        /// <param name="mallTypeSysNo">分销商城类型系统编号</param>
        /// <param name="top">取前几条</param>
        /// <param name="isFinish">升舱完成</param>
        /// <returns>分销商升舱订单列表</returns>
        /// <remarks>2013-09-10 黄志勇 创建</remarks>
        public Result<List<DsOrder>> GetDsOrderInfo(string shopAccount, int mallTypeSysNo, int top, bool? isFinish)
        {
            var dsOrder = DsOrderBo.Instance.GetGetDsOrderInfo(shopAccount, mallTypeSysNo, top, isFinish);
            var list = new List<DsOrder>();
            if (dsOrder != null && dsOrder.Count > 0)
            {
                foreach (var model in dsOrder)
                {
                    var info = new DsOrder();
                    Hyt.Util.Reflection.ReflectionUtils.Transform(model, info);
                    list.Add(info);
                }
            }

            var result = new Result<List<DsOrder>>
            {
                Data = list,
                Status = true
            };
            return result;
        }

        /// <summary>
        /// 分销商预存款主表
        /// </summary>
        /// <param name="shopAccount">店铺账号</param>
        /// <param name="mallTypeSysNo">分销商城类型系统编号</param>
        /// <returns>分销商预存款列表</returns>
        /// <remarks>2013-09-10 黄志勇 创建</remarks>
        public Result<DsPrePayment> GetPrePaymentInfo(string shopAccount, int mallTypeSysNo)
        {
            var model = DsOrderBo.Instance.GetPrePayment(shopAccount, mallTypeSysNo);
            DsPrePayment dsPrePaymentInfo = null;
            if (model != null)
            {
                dsPrePaymentInfo = new DsPrePayment();
                Hyt.Util.Reflection.ReflectionUtils.Transform(model, dsPrePaymentInfo);
            }
            var result = new Result<DsPrePayment>
            {
                Data = dsPrePaymentInfo,
                Status = true
            };
            return result;
        }

        /// <summary>
        /// 更新订单备注信息
        /// </summary>
        /// <param name="remarks">备注内容</param>
        /// <param name="auth">授权参数</param>
        /// <returns>2014-03-25</returns>
        //public Result UpdateTradeRemarks(IRemarksParameters remarks, AuthorizationParameters auth)
        //{
        //    //shopaccount 
        //    return UpGradeProvider.GetInstance(auth.MallType).UpdateTradeRemarks(remarks, auth);
        //}

        #region 返回实体与数据库实体映射
        /// <summary>
        /// 地区表
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>地区实体</returns>
        /// <remarks>2013-09-04 朱成果 创建</remarks>
        private BsArea BsAreaToModel(Model.BsArea entity)
        {
            if (entity == null) return null;
            BsArea m = new BsArea()
            {
                AreaCode = entity.AreaCode,
                AreaLevel = entity.AreaLevel,
                AreaName = entity.AreaName,
                DisplayOrder = entity.DisplayOrder,
                NameAcronym = entity.NameAcronym,
                ParentSysNo = entity.ParentSysNo,
                Status = entity.Status,
                SysNo = entity.SysNo

            };
            return m;
        }
        #endregion
    }
}
