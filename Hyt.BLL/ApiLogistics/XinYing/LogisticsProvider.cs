using Hyt.BLL.Order;
using Hyt.Model;
using Hyt.Model.Api;
using Hyt.Model.Order;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Util;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Hyt.BLL.ApiLogistics.XinYing
{
    /// <summary>
    /// 信营接口
    /// </summary>
    /// <remarks>
    /// 2016-9-7 杨浩 创建
    /// </remarks>
    public class LogisticsProvider : ILogisticsProvider
    {      
        public LogisticsProvider() { } 
        /// <summary>
        /// 物流标示
        /// </summary>
        /// <remarks>2015-12-26 杨浩 创建</remarks>
        public override Hyt.Model.CommonEnum.物流代码 Code
        {
            get { return Hyt.Model.CommonEnum.物流代码.信营; }
        }
      
        public override Result GetProduct(string productId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 添加交易订单
        /// </summary>
        /// <param name="orderSysno"></param>
        /// <returns>2016-09-07 杨浩 创建</returns>
        public override Result AddOrderTrade(int orderSysno)
        {           
            var result = new Result<string>();
            result.Status = true;
            result.StatusCode = 0;
            result.Message = "接口调用成功";      

            if (orderSysno <= 0)
            {
                return result;
            }

            try
            {
                var orderInfo = Hyt.BLL.Web.SoOrderBo.Instance.GetEntity(orderSysno);                    

                var order = new Model.Api.Order();

                order.OnlinePayment = Hyt.BLL.Finance.FinanceBo.Instance.GetOnlinePaymentBySourceSysNo(Model.WorkflowStatus.FinanceStatus.网上支付单据来源.销售单, orderInfo.SysNo);
                order.ReceiveAddress = SoOrderBo.Instance.GetOrderReceiveAddress(orderInfo.ReceiveAddressSysNo);
                order.Customer = Hyt.BLL.CRM.CrCustomerBo.Instance.GetModel(orderInfo.CustomerSysNo);

                order.OrderItemList = new List<OrderItem>();

                var pager = new Pager<PdProduct>() { PageSize = 999999, CurrentPage = 1 };
                pager = BLL.Product.PdProductBo.Instance.GetPdProductList(pager);               

                #region 订单信息

                order.OrderNo = orderInfo.OrderNo;
              
                order.CashPay = orderInfo.CashPay;
                order.CBLogisticsSendStatus = orderInfo.CBLogisticsSendStatus;
                order.CoinPay = orderInfo.CoinPay;
                order.ContactBeforeDelivery = orderInfo.ContactBeforeDelivery;
                order.CouponAmount = orderInfo.CouponAmount;
                order.CreateDate = DateTime.Now;
                order.CustomerMessage = orderInfo.CustomerMessage;
                order.CustomsPayStatus = orderInfo.CustomsPayStatus;
                order.CustomsStatus = orderInfo.CustomsStatus;
                order.DefaultWarehouseSysNo = 0;
                order.DeliveryRemarks = orderInfo.DeliveryRemarks;
                order.DeliveryTime = orderInfo.DeliveryTime;
                order.DeliveryTypeSysNo = orderInfo.DeliveryTypeSysNo;
                order.FreightAmount = orderInfo.FreightAmount;
                order.FreightChangeAmount = orderInfo.FreightChangeAmount;
                order.FreightDiscountAmount = orderInfo.FreightDiscountAmount;
                order.GZJCStatus = orderInfo.GZJCStatus;
                order.ImgFlag = orderInfo.ImgFlag;
                order.InternalRemarks = orderInfo.InternalRemarks;
                order.InvoiceSysNo = orderInfo.InvoiceSysNo;
                order.IsHiddenToCustomer = orderInfo.IsHiddenToCustomer;
             
                order.NsStatus = 0;
                order.OperatFee = 0;
                order.OrderAmount = orderInfo.OrderAmount;
                order.OrderCreatorSysNo = 0;
                order.OrderDiscountAmount = orderInfo.OrderDiscountAmount;
                order.PayStatus = orderInfo.PayStatus;
                order.PayTypeSysNo = orderInfo.PayTypeSysNo;
                order.ProductAmount = orderInfo.ProductAmount;
                order.ProductChangeAmount = orderInfo.ProductChangeAmount;
                order.ProductDiscountAmount = orderInfo.ProductDiscountAmount;
                order.RebateRtio = orderInfo.RebateRtio;
                order.Remarks = orderInfo.Remarks;
                order.SalesSysNo = orderInfo.SalesSysNo;
                order.SalesType = orderInfo.SalesType;
                order.SendStatus = orderInfo.SendStatus;
                order.Stamp = orderInfo.Stamp;
                order.Status = 10;
                order.TaxFee = orderInfo.TaxFee;
                order.UsedPromotions = "";
                #endregion

                foreach (var item in orderInfo.OrderItemList)
                {
                    var productInfo=pager.Rows.Where(x => x.SysNo == item.ProductSysNo).FirstOrDefault();
                    if (productInfo ==null)
                    {
                        result.Status = false;
                        result.StatusCode = 1;
                        result.Message = "产品系统编号【"+item.ProductSysNo+"】在系统中不存在";  
                        return result;
                    }

                    var orderItem = new OrderItem();
                    orderItem.ProductCode = productInfo.ErpCode;
                    orderItem.OrderNo = orderInfo.OrderNo;
                    orderItem.Catle = item.Catle;
                    orderItem.ChangeAmount = item.ChangeAmount;
                    orderItem.DiscountAmount = item.DiscountAmount;
                    orderItem.GroupCode = item.GroupCode;
                    orderItem.GroupName = item.GroupName;
                    orderItem.OriginalPrice = item.OriginalPrice;
                    orderItem.OriginalSalesUnitPrice = item.OriginalSalesUnitPrice;
                    orderItem.ProductName = item.ProductName;
                    orderItem.ProductSalesType = item.ProductSalesType;
                    orderItem.ProductSalesTypeSysNo = item.ProductSalesTypeSysNo;
                    orderItem.Quantity = item.Quantity;
                    orderItem.RealStockOutQuantity = item.RealStockOutQuantity;
                    orderItem.RebatesStatus = item.RebatesStatus;
                    orderItem.SalesAmount = item.SalesAmount;
                    orderItem.SalesUnitPrice = item.SalesUnitPrice;
                    orderItem.UnitCatle = item.UnitCatle;
                    orderItem.UsedPromotions = item.UsedPromotions;                  
                    order.OrderItemList.Add(orderItem);
                }


                var json = Newtonsoft.Json.JsonConvert.SerializeObject(order);

                Encoding myEncode = Encoding.GetEncoding("UTF-8");
       
                byte[] postBytes = Encoding.UTF8.GetBytes(json);
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create("http://api.singingwhale.com.cn/B2CApp/Orders.svc/AddOrder");
                req.Method = "POST";           
                req.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";

                req.ContentLength = postBytes.Length;
                using (Stream reqStream = req.GetRequestStream())
                {
                    reqStream.Write(postBytes, 0, postBytes.Length);
                }

                using (WebResponse res = req.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(res.GetResponseStream(), myEncode))
                    {
                        string strResult = sr.ReadToEnd();
                        var jObject=Newtonsoft.Json.Linq.JObject.Parse(strResult);
                        int statusCode=int.Parse(jObject["StatusCode"].ToString());


                        var soOrderSyncLogisticsLog = new SoOrderSyncLogisticsLog();
                        soOrderSyncLogisticsLog.OrderSysNo = orderSysno;
                        soOrderSyncLogisticsLog.Code = (int)this.Code;
                        soOrderSyncLogisticsLog.LastUpdateDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                        soOrderSyncLogisticsLog.StatusCode = "";
                        soOrderSyncLogisticsLog.StatusMsg = "";
                        soOrderSyncLogisticsLog.Packets = json;
                        soOrderSyncLogisticsLog.ReceiptContent = strResult;                      
                        if (statusCode==0)
                        {
                            soOrderSyncLogisticsLog.LastUpdateBy = 0;
                            soOrderSyncLogisticsLog.LogisticsOrderId = statusCode.ToString();
                            soOrderSyncLogisticsLog.CreateDate = DateTime.Now;
                            soOrderSyncLogisticsLog.CreatedBy = BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo;
                            SoOrderSyncLogisticsLogBo.Instance.Insert(soOrderSyncLogisticsLog);
                            BLL.Order.SoOrderBo.Instance.UpdateOrderApiStatus((int)Hyt.Model.WorkflowStatus.OrderStatus.跨境物流推送状态.已推送, 3, orderSysno);
                        }
                        else
                        {
                            result.Status = false;
                            result.Message = jObject["Message"].ToString();      
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = "向" + this.Code + "物流推送订单报错：" + ex.StackTrace;
                return result;
            }

            return result;
        }

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="orderSysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-07-05 陈海裕 创建</remarks>
        public override Result CancelOrderTrade(int orderSysNo, string reason = "")
        {      
            Result<string> result = new Result<string>();
            result.Status = false;
            result.StatusCode = 0;
            result.Message = "接口调用失败";    

            if (orderSysNo <= 0)
            {
                return result;
            }

            try
            {

              
            }
            catch (Exception ex)
            {
                result.Message = "向" + this.Code + "物流取消订单报错：" + ex.StackTrace;
                return result;
            }

            return result;
        }


        /// <summary>
        /// 查询订单物流信息
        /// </summary>
        /// <param name="orderSysNo">订单号</param>
        /// <returns></returns>
        /// <remarks>2016-09-09 杨浩 创建</remarks>
        public override Result<string> GetLogisticsTracking(int orderSysNo)
        {
            var result = new Result<string>();
            result.Status = false;
            result.StatusCode = 0;
            result.Message = "接口调用失败";        

            if (orderSysNo <= 0)
            {
                return result;
            }

            var orderInfo = BLL.Order.SoOrderBo.Instance.GetEntity(orderSysNo);               
            try
            {
                string json = "";// "{\"orderNo\":\"" + orderInfo.OrderNo + "\"}";
                var myEncode = Encoding.GetEncoding("UTF-8");      
                byte[] postBytes = Encoding.UTF8.GetBytes(json);
                var req = (HttpWebRequest)HttpWebRequest.Create("http://api.singingwhale.com.cn/B2CApp/Orders.svc/GetLogisticsTracking?orderNo=" + orderInfo.OrderNo);
                req.Method = "GET";           
                req.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";

                req.ContentLength = postBytes.Length;
          
                //using (Stream reqStream = req.GetRequestStream())
                //{
                //    reqStream.Write(postBytes, 0, postBytes.Length);
                //}
                using (var res = req.GetResponse())
                {
                    using (var sr = new StreamReader(res.GetResponseStream(),myEncode))
                    {
                        string strResult = sr.ReadToEnd();
                        var jObject=Newtonsoft.Json.Linq.JObject.Parse(strResult);
                        string statusCode=jObject["StatusCode"].ToString();

                        result.Message = "没有找到物流信息";     
                        if (statusCode == "0")
                        {
                            if (jObject["Data"].Count() > 0)
                            {
                                string expressNo = jObject["Data"][0]["ExpressNo"].ToString();
                                result.Message = expressNo;        
                            }
                        }
                    }
                }
                return result; 
            }
            catch (Exception ex)
            {
                result.Message = "查询订单物流状态报错：" + ex.StackTrace;
                return result;
            }
        }

   

        /// <summary>
        /// 获取运单号
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        /// <remarks>2016-07-19 陈海裕 创建</remarks>
        public override Result GetOrderExpressno(string orderId)
        {
            Result<string> result = new Result<string>();
            result.Status = false;
            result.StatusCode = 0;
            result.Message = "接口调用失败";
           

                return result;
          
        }

        public override Result GetOrderTrade(string orderId)
        {
            throw new NotImplementedException();
        }

        public Result<string> GetBaseData(int type)
        {
            Result<string> result = new Result<string>();
            result.Status = false;
            result.StatusCode = 0;
            result.Message = "接口调用失败";

           

            return result;
        }
        


      
    }

  
}
