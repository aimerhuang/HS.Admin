using Hyt.Model;
using Hyt.Model.Api;
using Hyt.Model.Stores;
using Hyt.Model.WorkflowStatus;
using Hyt.Service.Contract.B2CApp;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Transactions;
namespace Hyt.Service.Implement.B2CApp
{
 
    /// <summary>
    /// 订单
    /// </summary>
    /// <remarks>2016-9-7 杨浩 创建</remarks>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Orders : IOrders
    {
        /// <summary>
        /// 添加订单
        /// </summary>
        /// <param name="stream">订单数据（json格式）</param>
        /// <returns></returns>
        /// <remarks>2016-9-7 杨浩 创建</remarks>
        public Result<string> AddOrder(Stream stream)
        {
            int exceptionPoint = 0; 
            string data = "";
            int dealerSysNo = 329;
            var result = new Result<string>()
            {
                Status=true,
                StatusCode=0
            };

            try
            {
                var reader = new StreamReader(stream);
                data = reader.ReadToEnd();          
                exceptionPoint = 1;
                var orderInfo = Hyt.Util.Serialization.JsonUtil.ToObject<Order>(data);
                exceptionPoint = 2;
                var orderAssociationInfo=Hyt.BLL.Stores.DsOrderAssociationBo.Instance.GetOrderAssociationInfo(dealerSysNo, orderInfo.OrderNo);
                exceptionPoint = 3;
                if (orderAssociationInfo != null)
                {
                    result.Status = false;
                    result.Message = "订单已存在";
                    result.StatusCode = 1;
                    return result;
                }

                if (orderInfo.OnlinePayment == null)
                {
                    result.Status = false;
                    result.Message = "没有支付记录";
                    result.StatusCode = 2;
                    return result;
                }


              
                var onlinePayments = Hyt.BLL.Finance.FinanceBo.Instance.GetOnlinePaymentByVoucherNo(orderInfo.OnlinePayment.PaymentTypeSysNo,orderInfo.OnlinePayment.VoucherNo);

                if (onlinePayments!=null)
                {
                    result.Status = false;
                    result.Message = "订单中的支付单已存在";
                    result.StatusCode = 3;
                    return result;
                }

                #region 订单信息
                SoOrder so = new SoOrder();
                so.OrderNo = BLL.Basic.ReceiptNumberBo.Instance.GetOrderNo();
                so.OrderSource=(int)Hyt.Model.WorkflowStatus.OrderStatus.销售单来源.三方商城;
                so.DealerSysNo=dealerSysNo;
                so.AuditorDate =(DateTime)SqlDateTime.MinValue;
                so.AuditorSysNo = 0;
                so.CancelDate = (DateTime)SqlDateTime.MinValue;

                so.CashPay = orderInfo.CashPay;
                so.CBLogisticsSendStatus = orderInfo.CBLogisticsSendStatus;
                so.CoinPay = orderInfo.CoinPay;
                so.ContactBeforeDelivery = orderInfo.ContactBeforeDelivery;
                so.CouponAmount = orderInfo.CouponAmount;
                so.CreateDate = DateTime.Now;
                so.CustomerMessage = orderInfo.CustomerMessage;
                so.CustomsPayStatus = orderInfo.CustomsPayStatus;
                so.CustomsStatus = orderInfo.CustomsStatus;
                so.DefaultWarehouseSysNo = 0;
                so.DeliveryRemarks = orderInfo.DeliveryRemarks;
                so.DeliveryTime = orderInfo.DeliveryTime;
                so.DeliveryTypeSysNo = orderInfo.DeliveryTypeSysNo;
                so.FreightAmount = orderInfo.FreightAmount;
                so.FreightChangeAmount = orderInfo.FreightChangeAmount;
                so.FreightDiscountAmount = orderInfo.FreightDiscountAmount;
                so.GZJCStatus = orderInfo.GZJCStatus;
                so.ImgFlag = orderInfo.ImgFlag;
                so.InternalRemarks = orderInfo.InternalRemarks;
                so.InvoiceSysNo = orderInfo.InvoiceSysNo;
                so.IsHiddenToCustomer = orderInfo.IsHiddenToCustomer;
                so.LastUpdateDate = (DateTime)SqlDateTime.MinValue;
                so.NsStatus = 0;
                so.OperatFee = 0;
                so.OrderAmount = orderInfo.OrderAmount;
                so.OrderCreatorSysNo = 0;
                so.OrderDiscountAmount = orderInfo.OrderDiscountAmount;
                so.PayStatus = orderInfo.PayStatus;
                so.PayTypeSysNo = orderInfo.PayTypeSysNo;
                so.ProductAmount = orderInfo.ProductAmount;
                so.ProductChangeAmount = orderInfo.ProductChangeAmount;
                so.ProductDiscountAmount = orderInfo.ProductDiscountAmount;
                so.RebateRtio = orderInfo.RebateRtio;
                so.Remarks = orderInfo.Remarks;
                so.SalesSysNo = orderInfo.SalesSysNo;
                so.SalesType = orderInfo.SalesType;
                so.SendStatus = orderInfo.SendStatus;
                so.Stamp = orderInfo.Stamp;
                so.Status = 10;
                so.TaxFee = orderInfo.TaxFee;
                so.UsedPromotions = "";
                #endregion

                #region 订单明细

                var soItems = new List<SoOrderItem>();

                var soItem = new SoOrderItem();
                var pager = new Pager<PdProduct>() { PageSize = 999999, CurrentPage = 1 };
                pager = BLL.Product.PdProductBo.Instance.GetPdProductList(pager);

                var dealerInfo=BLL.Stores.StoresBo.Instance.GetStoreById(dealerSysNo);
                var dealerLevelInfo=BLL.Distribution.DsDealerLevelBo.Instance.GetDealerLevelByDealerSysNo(dealerInfo.LevelSysNo);

                //订单总价
                decimal orderTotalPrice= orderInfo.OrderItemList.Sum(x => x.SalesUnitPrice * x.Quantity);

                if (orderTotalPrice != orderInfo.CashPay)
                {
                    result.Status = false;
                    result.Message = "订单明细中的商品总价和订单的支付金额不一致";
                    result.StatusCode = 4;
                    return result;
                }

                foreach (var item in orderInfo.OrderItemList)
                {
                    var productInfo = pager.Rows.Where(x => x.ErpCode == item.ProductCode).FirstOrDefault();
                    if (productInfo == null)
                    {
                        result.Status = false;
                        result.Message = "商品编码【" + item.ProductCode + "】在系统中不存在";
                        result.StatusCode = 5;
                        return result;
                    }

                    //价格下限
                    decimal lower=productInfo.TradePrice* (100 - dealerLevelInfo.SalePriceLower) * 0.01m;

                    //价格上限
                    decimal upper = productInfo.TradePrice * (100+dealerLevelInfo.SalePriceUpper) * 0.01m;

                    //检查销售价格下限
                    if (lower>item.SalesUnitPrice)
                    {
                        result.Status = false;
                        result.Message = "商品编码【" + item.ProductCode + "】的销售价不能低于￥" + lower;
                        result.StatusCode = 6;
                        return result;
                    }

                    //检查销售价格上限
                    //if (upper<item.SalesUnitPrice)
                    //{
                    //    result.Status = false;
                    //    result.Message = "商品编码【" + item.ProductCode + "】的销售价不能高于￥" + upper;
                    //    result.StatusCode = 7;
                    //    return result;
                    //}


                    //返点=（（销售价-批发价）*数量）*操作费
                    soItem.Catle = ((productInfo.TradePrice - item.SalesUnitPrice) * item.Quantity) * (1 - (dealerLevelInfo.OperatFee * 0.001m));
                    soItem.Catle = soItem.Catle < 0 ? 0 : soItem.Catle;

                    soItem.ChangeAmount = 0;
                    soItem.DiscountAmount = 0;
                    soItem.GroupCode = "";
                    soItem.GroupName = "";
                    soItem.OriginalPrice = item.OriginalPrice;
                    soItem.OriginalSalesUnitPrice = 0;
                    soItem.ProductName = item.ProductName;
                    soItem.ProductSalesType = 10;
                    soItem.ProductSalesTypeSysNo = item.ProductSalesTypeSysNo;
                    soItem.ProductSysNo = productInfo.SysNo;
                    soItem.Quantity = item.Quantity;
                    soItem.RealStockOutQuantity = 0;
                    soItem.RebatesStatus = 0;
                    soItem.SalesAmount = item.SalesAmount;
                    soItem.SalesUnitPrice = item.SalesUnitPrice;
                 
                    soItem.UnitCatle = 0;
                    soItem.UsedPromotions = "";
                
                    soItems.Add(soItem);
                }


                #endregion

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
                    cr=Hyt.BLL.CRM.CrCustomerBo.Instance.GetCrCustomer(orderInfo.Customer.Account);
                    exceptionPoint = 4;
                    //var customerlst = Hyt.BLL.Order.SoOrderBo.Instance.SearchCustomer();
                    if (cr != null)
                    {                       
                        isNewUser = false;
                    }
                    else //创建会员
                    {
                        cr = new Model.CrCustomer()
                        {
                            Account = orderInfo.Customer.Account,
                            MobilePhoneNumber =  orderInfo.Customer.MobilePhoneNumber,
                            AreaSysNo = orderInfo.Customer.AreaSysNo,
                            Gender = (int)Hyt.Model.WorkflowStatus.CustomerStatus.性别.保密,
                            EmailStatus = (int)Hyt.Model.WorkflowStatus.CustomerStatus.邮箱状态.未验证,
                            LevelSysNo = Hyt.Model.SystemPredefined.CustomerLevel.初级,
                            Name = orderInfo.Customer.Name,
                            NickName = orderInfo.Customer.NickName,
                            RegisterDate = DateTime.Now,
                            Password = strPassword, // Hyt.Util.EncryptionUtil.EncryptWithMd5AndSalt(strPassword), 余勇修改 2014-09-12
                            Status = (int)Hyt.Model.WorkflowStatus.CustomerStatus.会员状态.有效,
                            MobilePhoneStatus = (int)Hyt.Model.WorkflowStatus.CustomerStatus.手机状态.未验证,
                            RegisterSource = (int)Hyt.Model.WorkflowStatus.CustomerStatus.注册来源.三方商城,
                            RegisterSourceSysNo = dealerSysNo.ToString(),
                            StreetAddress =orderInfo.ReceiveAddress.StreetAddress,
                            IsReceiveShortMessage = (int)CustomerStatus.是否接收短信.是,
                            IsReceiveEmail = (int)CustomerStatus.是否接收邮件.是,
                            LastLoginDate = DateTime.Now,
                            Birthday = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                            CreatedDate = DateTime.Now,
                            DealerSysNo=dealerSysNo,
                        };

                        Hyt.Model.CrReceiveAddress crr = new Model.CrReceiveAddress()
                        {
                            AreaSysNo = orderInfo.ReceiveAddress.AreaSysNo,
                            Name = orderInfo.ReceiveAddress.Name,
                            MobilePhoneNumber = orderInfo.ReceiveAddress.MobilePhoneNumber,
                            StreetAddress = orderInfo.ReceiveAddress.StreetAddress,
                            IsDefault = 1
                        };
                        Hyt.BLL.Order.SoOrderBo.Instance.CreateCustomer(cr,crr);
                        exceptionPoint = 5;
                    }
                    trancustomer.Complete();//会员创建事物
                }
                if (cr == null || cr.SysNo < 1)
                {
                    result.Status = false;
                    result.Message = "会员信息读取失败";
                    return result;
                }
                exceptionPoint = 6;
                #endregion

                #region 数据提交

                so.CustomerSysNo = cr.SysNo;
                so.LevelSysNo = cr.LevelSysNo;
                so.PayTypeSysNo = 12;//易宝支付
                using (var tran = new System.Transactions.TransactionScope(TransactionScopeOption.Required,options))
                {
                    var r = Hyt.BLL.Order.SoOrderBo.Instance.ImportSoOrder(so, orderInfo.ReceiveAddress, soItems.ToArray(), "系统", orderInfo.OnlinePayment, ref exceptionPoint);
                    exceptionPoint = 7;
                    if (r.StatusCode > 0)
                    {
                        var model = new DsOrderAssociation()
                        {
                            DealerOrderNo = orderInfo.OrderNo,
                            DealerSysNo = dealerSysNo,
                            OrderSysNo = r.StatusCode,
                        };
                        Hyt.BLL.Stores.DsOrderAssociationBo.Instance.Add(model);
                       exceptionPoint = 8;                      
                    }               
                    exceptionPoint = 9;
                    tran.Complete();
                }
                #endregion      
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = "接口异常";
                result.StatusCode = -1;
                BLL.Log.LocalLogBo.Instance.Write(ex.Message + "----->exceptionPoint ="+exceptionPoint, "AddOrderExceptionLog");
            }
           
            return result;
        }

        /// <summary>
        /// 获取物流状态
        /// </summary>
        /// <param name="orderNo">订单编号</param>
        /// <returns></returns>
        /// <remarks>2016-09-09 杨浩 创建</remarks>
        public Result<string> GetLogisticsTracking(string orderNo)
        {
            int dealerSysNo = 329;
            var result = new Result<string>()
            {
                Status = true,
                StatusCode = 0
            };
            try
            {

                if (!string.IsNullOrWhiteSpace(orderNo))
                {
                    result.Status = false;
                    result.Message = "缺少订单编号";
                    result.StatusCode = 3;
                    return result;
                }
        
                var orderAssociationInfo = Hyt.BLL.Stores.DsOrderAssociationBo.Instance.GetOrderAssociationInfo(dealerSysNo,orderNo);
                if (orderAssociationInfo == null)
                {
                    result.Status = false;
                    result.Message = "订单编号不存在";
                    result.StatusCode = 4;
                    return result;
                }
          

                var deliveryItems=BLL.Order.SoOrderBo.Instance.GetDeliveryItem(orderAssociationInfo.OrderSysNo);

                if (deliveryItems == null || deliveryItems.Count <= 0)
                {
                    result.Status = false;
                    result.Message = "订单未发货";
                    result.StatusCode =5;
                    return result;
                }

                string newDeliveryItems = "";
                foreach (var deliveryItem in deliveryItems)
                {
                    var deliveryInfo=BLL.Logistics.LgDeliveryBo.Instance.GetDelivery(deliveryItem.DeliverySysNo);
                    newDeliveryItems += "{\"DeliverySysNo\":" + deliveryInfo.DeliveryTypeSysNo + ",\"ExpressNo\":\""+deliveryItem.ExpressNo+"\"}";
                }
                newDeliveryItems = "[" + newDeliveryItems + "]";
                result.Data =newDeliveryItems;
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = "接口异常";
                result.StatusCode = -1;
                BLL.Log.LocalLogBo.Instance.Write(ex.Message + "----->exceptionPoint =", "GetLogisticsTrackingExceptionLog");
            }
           


            return result;

        }
    }
}
