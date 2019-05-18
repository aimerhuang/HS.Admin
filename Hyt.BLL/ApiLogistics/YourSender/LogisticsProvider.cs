using Hyt.BLL.Order;
using Hyt.Model;
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

namespace Hyt.BLL.ApiLogistics.YourSender
{
    /// <summary>
    /// 有信物流接口
    /// </summary>
    /// <remarks>
    /// 2015-10-12 杨浩 创建
    /// 2016-03-25 陈海裕 修改
    /// </remarks>
    public class LogisticsProvider : ILogisticsProvider
    {
        string CustomerCode = config.CustomerCode;
        #region 测试环境
        //const string Key = "4737d0d4b107d1ce1d9a79636a2e1231";
        //const string Token = "38B27CD0163F9884";
        //const string RequestUrl = "http://121.40.111.43:8080/customerApi/";
        #endregion

        string Key = config.AppKey;
        string Token = config.Token;
        string RequestUrl = "http://ccs.ehaiwaigou.cn/customerApi/";

        // 顺丰账号密码
        string SFCustomerCode = config.SFCustomerCode;
        string SFKey = config.SFKey;
        string SFToken = config.SFToken;

        public LogisticsProvider() { }

        /// <summary>
        /// 订单id，用于日志记录
        /// </summary>
        private int _orderSysNo = 0;

        /// <summary>
        /// 物流标示
        /// </summary>
        /// <remarks>2015-12-26 杨浩 创建</remarks>
        public override Hyt.Model.CommonEnum.物流代码 Code
        {
            get { return Hyt.Model.CommonEnum.物流代码.有信达; }
        }
      
        public override Result GetProduct(string productId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 添加交易订单
        /// </summary>
        /// <param name="orderSysno"></param>
        /// <returns>2016-04-09 陈海裕 创建</returns>
        public override Result AddOrderTrade(int orderSysno)
        {
            // 订单id，用于日志记录
            _orderSysNo = orderSysno;

            var result = new Result<string>();
            result.Status = false;
            result.StatusCode = 0;
            result.Message = "接口调用失败";
            string url = RequestUrl + "order/create";

            if (orderSysno <= 0)
            {
                return result;
            }

            try
            {
                var order = BLL.Order.SoOrderBo.Instance.GetEntity(orderSysno);
                if (order == null)
                {
                    result.Message = "该订单不存在";
                    return result;
                }
                order.ReceiveAddress = BLL.Order.SoOrderBo.Instance.GetOrderReceiveAddress(order.ReceiveAddressSysNo);
                order.OrderItemList = DataAccess.Order.ISoOrderItemDao.Instance.GetOrderItemsByOrderSysNo(order.SysNo);
                var voucherFilter = new ParaVoucherFilter();
                voucherFilter.SourceSysNo = order.SysNo;
                var recVoucher = BLL.Finance.FinanceBo.Instance.GetReceiptVouchers(voucherFilter).Rows.FirstOrDefault();
                recVoucher.VoucherItems = BLL.Finance.FnReceiptVoucherBo.Instance.GetReceiptVoucherItem(recVoucher.SysNo);
                // 收货人 区 市 省
                var receiverDistrict = BLL.Basic.BasicAreaBo.Instance.GetArea(order.ReceiveAddress.AreaSysNo);
                var receiverCity = BLL.Basic.BasicAreaBo.Instance.GetArea(receiverDistrict.ParentSysNo);
                var receiverProvince = BLL.Basic.BasicAreaBo.Instance.GetArea(receiverCity.ParentSysNo);
                // 发货人 市
                var warehouse = BLL.Warehouse.WhWarehouseBo.Instance.GetWarehouse(order.DefaultWarehouseSysNo);

                var shipperCity = BLL.Basic.BasicAreaBo.Instance.GetArea(warehouse.CitySysNo);

                if(string.IsNullOrEmpty(warehouse.LogisWarehouseCode))
                {
                    Result<string> whResult = GetWarehouseId();
                    if (!whResult.Status)
                    {
                        return whResult;
                    }
                    else
                    {
                        warehouse.LogisWarehouseCode = whResult.Data.Split(':')[0].Replace("{", "").Replace("}", "").Replace("\"", "").Trim();
                        BLL.Warehouse.WhWarehouseBo.Instance.Update(warehouse);
                    }
                }

                if (string.IsNullOrEmpty(receiverDistrict.AreaCode) || string.IsNullOrEmpty(receiverCity.AreaCode)
                    || string.IsNullOrEmpty(receiverProvince.AreaCode) || string.IsNullOrEmpty(shipperCity.AreaCode))
                {
                    Result<List<YSArea>> areaResult = new Result<List<YSArea>>();
                    areaResult.Data = new List<YSArea>();
                    for (int i = 0; i < 8; i++)
                    {
                        areaResult.Data.AddRange(GetYSAreaData(i + 1).Data);
                    }
                    if (string.IsNullOrEmpty(receiverProvince.AreaCode))
                    {

                        YSArea ysArea = areaResult.Data.Find((p => p.area_name == receiverProvince.AreaName.Trim() || p.old_name == receiverProvince.AreaName.Trim()));
                        if (ysArea != null)
                        {
                            receiverProvince.AreaCode = ysArea.id.ToString();
                            BLL.Basic.BasicAreaBo.Instance.Update(receiverProvince);
                        }
                    }
                   
                    if (string.IsNullOrEmpty(receiverCity.AreaCode))
                    {
                        YSArea ysArea = areaResult.Data.Find(p => (p.area_name == receiverCity.AreaName.Trim() || p.old_name == receiverCity.AreaName.Trim()) && p.pid.ToString() == receiverProvince.AreaCode);
                        if (ysArea != null)
                        {
                            receiverCity.AreaCode = ysArea.id.ToString();
                            BLL.Basic.BasicAreaBo.Instance.Update(receiverCity);
                        }
                    }
                    if (string.IsNullOrEmpty(receiverDistrict.AreaCode))
                    {
                        YSArea ysArea = areaResult.Data.Find(p => (p.area_name == receiverDistrict.AreaName.Trim() || p.old_name == receiverDistrict.AreaName.Trim()) && p.pid.ToString() == receiverCity.AreaCode);
                        if (ysArea != null)
                        {
                            receiverDistrict.AreaCode = ysArea.id.ToString();
                            BLL.Basic.BasicAreaBo.Instance.Update(receiverDistrict);
                        }
                    }
                   

                    if (string.IsNullOrEmpty(shipperCity.AreaCode))
                    {
                        if (string.IsNullOrEmpty(shipperCity.AreaCode))
                        {
                            shipperCity.AreaCode = shipperCity.SysNo.ToString();
                        }
                       
                        //YSArea ysArea = areaResult.Data.Find(p => p.area_name == receiverProvince.AreaName.Trim() || p.old_name == receiverDistrict.AreaName.Trim());
                        //if (ysArea != null)
                        //{
                        //    shipperCity.AreaCode = ysArea.id.ToString();
                        //    BLL.Basic.BasicAreaBo.Instance.Update(shipperCity);
                        //}
                    }
                }



             

                var dealer = BLL.Distribution.DsDealerBo.Instance.GetDsDealer(order.DealerSysNo);

                var deliveryType = BLL.Logistics.DeliveryTypeBo.Instance.GetDeliveryType(order.DeliveryTypeSysNo);

                var filter = new ParaOnlinePaymentFilter();
                filter.OrderSysNo = orderSysno;
                var onlinePayment = BLL.Finance.FinanceBo.Instance.GetOnlinePayments(filter).Rows.Where(o => o.Status == 1).FirstOrDefault();

                if (onlinePayment == null)
                {
                    result.Message = "订单不存在在线支付记录";
                    return result;
                }

                string _deliveryType = "";
                if (deliveryType.DeliveryTypeName.Contains("顺丰"))
                {
                    _deliveryType = "SF";
                    //CustomerCode = SFCustomerCode;
                    //Key = SFKey;
                    Token = SFToken;
                }
                else if (deliveryType.DeliveryTypeName.Contains("中通"))
                {
                    _deliveryType = "ZTO";
                }
                if (_deliveryType == "")
                {
                    result.Message = "友信达物流目前只有支持顺丰和中通两种快递方式";
                    return result;
                }

                // 易宝支付有限公司 PTE51001409230000001
                var paramsData = new Dictionary<string, string>();

                decimal grossWeight = 0m;
                decimal netWeight = 0m;
                var error = "";
                string productSku = "";
                string supplierCode = "";
                string productName = "";
                string qty = "";
                string price = "";
                for (int i = 0; i < order.OrderItemList.Count; i++)
                {
                    var product = BLL.Product.PdProductBo.Instance.GetProduct(order.OrderItemList[i].ProductSysNo);
                    grossWeight += (product.GrosWeight) * order.OrderItemList[i].Quantity;
                    netWeight += (product.NetWeight) * order.OrderItemList[i].Quantity;
                    var productStock = BLL.Warehouse.PdProductStockBo.Instance.GetEntityByWP(order.DefaultWarehouseSysNo, order.OrderItemList[i].ProductSysNo);

                    if (productStock == null)
                    {
                        error += order.OrderItemList[i].ProductSysNo + ",";
                        continue;
                    }

                    #region 条形码商品
                    var RefProductName = order.OrderItemList[i].ProductName;
                    var RefProductSysNo = order.OrderItemList[i].ProductSysNo;
                    var RefProductQuantity = order.OrderItemList[i].Quantity;
                    BLL.Product.PdProductBo.Instance.RefProductQuantity(ref RefProductSysNo, ref RefProductQuantity, ref RefProductName, order.SysNo);
                    #endregion

                    //// 订单商品
                    //paramsData.Add("products[" + i + "][product_sku]", product.ErpCode);
                    //paramsData.Add("products[" + i + "][supplier_code]", "");
                    ////paramsData.Add("products[" + i + "][product_name]", order.OrderItemList[i].ProductName);
                    ////paramsData.Add("products[" + i + "][qty]", order.OrderItemList[i].Quantity.ToString());
                    //paramsData.Add("products[" + i + "][product_name]", RefProductName);
                    //paramsData.Add("products[" + i + "][qty]", RefProductQuantity.ToString());
                    ////paramsData.Add("products[" + i + "][price]",(order.OrderItemList[i].SalesUnitPrice + order.OrderItemList[i].ChangeAmount).ToString());
                    productSku += "&products[" + i + "][product_sku]=" + product.ErpCode;
                    productSku += "&products[" + i + "][supplier_code]=";
                    productSku += "&products[" + i + "][product_name]=" + order.OrderItemList[i].ProductName;
                    productSku += "&products[" + i + "][qty]=" + RefProductQuantity;
                    decimal amount=Math.Round(((order.OrderItemList[i].SalesAmount + order.OrderItemList[i].ChangeAmount) / order.OrderItemList[i].Quantity),4);
                    productSku += "&products[" + i + "][price]=" + amount;
                    //paramsData.Add("products[" + i + "][price]", amount.ToString());
                    //order.OrderItemList[i].SalesAmount+order.OrderItemList[i].ChangeAmount=(order.OrderItemList[i].SalesUnitPrice*order.OrderItemList[i].Quantity)  =>
                    //order.OrderItemList[i].SalesUnitPrice=(order.OrderItemList[i].SalesAmount + order.OrderItemList[i].ChangeAmount) / order.OrderItemList[i].Quantity
                }
              
                //2016-10-28 18:39 临时处理报错问题
                if (!string.IsNullOrWhiteSpace(error))
                {
                    result.Message = this.Code + "：商品编号（" + error.TrimEnd(',') + "）在库存中找不到，无法获取SKU，请入库。";
                    return result;
                }
                paramsData.Add("address_remark","");
                paramsData.Add("products", "products");
                paramsData.Add("card_no", order.ReceiveAddress.IDCardNo);
                paramsData.Add("card_type", "01");
                paramsData.Add("currency_code", "RMB");
                paramsData.Add("customize_category", "");
                paramsData.Add("express_method", "1");
                paramsData.Add("tracking_number", "");
                paramsData.Add("inter_tracking_number", "");
                paramsData.Add("note", "");
                paramsData.Add("freight", order.FreightAmount.ToString());
                paramsData.Add("gross_weight", grossWeight.ToString());
                paramsData.Add("net_weight", netWeight.ToString());
                paramsData.Add("is_valuation", "0");
                paramsData.Add("valuation_fee", order.TaxFee.ToString("0.00"));
                paramsData.Add("valuation_value", order.OrderAmount.ToString("0.00"));
                paramsData.Add("order_time", CreateTimeStamp(order.CreateDate).ToString());
                paramsData.Add("pay_company", "易宝支付有限公司");
                paramsData.Add("pay_company_no", config.CIECode);// 信营商检企业备案号
                paramsData.Add("pay_no", recVoucher.VoucherItems.FirstOrDefault().VoucherNo);
                paramsData.Add("receiver_city_id", receiverCity.AreaCode);
                paramsData.Add("receiver_district_id", receiverDistrict.AreaCode);
                paramsData.Add("receiver_name", order.ReceiveAddress.Name);
                //paramsData.Add("receiver_postcode", order.ReceiveAddress.ZipCode);
                paramsData.Add("receiver_postcode", "000000");// 无邮政编码，默认六个零
                paramsData.Add("receiver_province_id", receiverProvince.AreaCode);
                paramsData.Add("receiver_fax", receiverProvince.AreaCode);
                paramsData.Add("receiver_tel", order.ReceiveAddress.MobilePhoneNumber);
                paramsData.Add("receiver_tel2", order.ReceiveAddress.PhoneNumber);
                paramsData.Add("reference_no", onlinePayment.BusinessOrderSysNo);
                paramsData.Add("receiver_address", order.ReceiveAddress.StreetAddress);
                paramsData.Add("shipper_city", shipperCity.AreaName);
                paramsData.Add("shipper_country", "CN");
                //paramsData.Add("shipper_country", "HK");
                paramsData.Add("shipper_name", !string.IsNullOrWhiteSpace(warehouse.BackWarehouseName) ? warehouse.BackWarehouseName : dealer.ErpName);
                paramsData.Add("sm_code", _deliveryType);
                paramsData.Add("tax_amount", order.TaxFee.ToString());
                paramsData.Add("total_amount", order.OrderAmount.ToString());
                paramsData.Add("total_product_amount", (order.ProductAmount + order.ProductChangeAmount).ToString());
                //paramsData.Add("wrap_type", "2C1");
                paramsData.Add("wrap_type", "4M");
                paramsData.Add("warehouse_id", warehouse.LogisWarehouseCode);//广州白云
                paramsData.Add("shipper_tel", string.IsNullOrWhiteSpace(dealer.MobilePhoneNumber) ? dealer.PhoneNumber : dealer.MobilePhoneNumber);
                paramsData.Add("shipper_address", warehouse.StreetAddress);
                string postData = InitParams(paramsData);
                string timerstamp = CreateTimeStamp();
                //postData += Key + timerstamp;
                postData = postData.Replace("&products=products", productSku + supplierCode + productName + qty + price);
                string responseStr = GetResponse(url, postData, timerstamp, Key );

                result = GetResult(JObject.Parse(responseStr), result);

                var soOrderSyncLogisticsLog = new SoOrderSyncLogisticsLog();

                soOrderSyncLogisticsLog.OrderSysNo = orderSysno;
                soOrderSyncLogisticsLog.Code = (int)this.Code;

                soOrderSyncLogisticsLog.LastUpdateDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                soOrderSyncLogisticsLog.StatusCode = "";
                soOrderSyncLogisticsLog.StatusMsg = "";
                soOrderSyncLogisticsLog.Packets = postData;
                soOrderSyncLogisticsLog.ReceiptContent = responseStr;
                if (result.Status == true)
                {
                    soOrderSyncLogisticsLog.LastUpdateBy = 0;
                    soOrderSyncLogisticsLog.LogisticsOrderId = JObject.Parse(responseStr)["data"]["order_sn"].ToString();

                    soOrderSyncLogisticsLog.CreateDate = DateTime.Now;
                    soOrderSyncLogisticsLog.CreatedBy = BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo;
                    SoOrderSyncLogisticsLogBo.Instance.Insert(soOrderSyncLogisticsLog);

                    BLL.Order.SoOrderBo.Instance.UpdateOrderApiStatus((int)Hyt.Model.WorkflowStatus.OrderStatus.跨境物流推送状态.已推送, 3, orderSysno);

                }
            }
            catch (Exception ex)
            {
                result.Message = "向" + this.Code + "物流推送订单报错：" + ex.StackTrace;
                return result;
            }

            return result;
        }

        /// <summary>
        ///  获取仓库编号
        /// </summary>
        /// <returns></returns>
        public Result<string> GetWarehouseId()
        {
            var result = new Result<string>();
            result.Status = false;
            result.StatusCode = 0;
            result.Message = "接口调用失败";
            string url = RequestUrl + "common/customerWarehouse";
            var paramsData = new Dictionary<string, string>();
            paramsData.Add("page", "1");
            paramsData.Add("pagesize", "20");
            string postData = InitParams(paramsData);
            string timerstamp= CreateTimeStamp();
            postData += Key + timerstamp;
            string responseStr = GetResponse(url, postData,timerstamp);
            result = GetResult(JObject.Parse(responseStr), result);
            return result;
        }

        public Result<List<YSArea>> GetYSAreaData(int page)
        {
            var result = new Result<List<YSArea>>();
            var firstResult = new Result<string>();
            result.Status = false;
            result.StatusCode = 0;
            result.Message = "接口调用失败";
            string url = RequestUrl + "common/area";
            var paramsData = new Dictionary<string, string>();
            paramsData.Add("page", page.ToString());
            paramsData.Add("pagesize", "500");
            string postData = InitParams(paramsData);
            string timerstamp = CreateTimeStamp();
            postData += Key + timerstamp;
            string responseStr = GetResponse(url, postData, timerstamp);
            firstResult = GetResult(JObject.Parse(responseStr), firstResult);
            if (firstResult.Status)
            {
                JObject back = JObject.Parse(firstResult.Data);
                string rowData = back["rows"].ToString();
                result.Data=Hyt.Util.Serialization.JsonUtil.ToObject<List<YSArea>>(rowData);
                result.Status = true;
            }
            else
            {
                result.Status = firstResult.Status;
                result.Message = firstResult.Message;
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
            // 订单id，用于日志记录
            _orderSysNo = orderSysNo;

            Result<string> result = new Result<string>();
            result.Status = false;
            result.StatusCode = 0;
            result.Message = "接口调用失败";
            string url = RequestUrl + "order/del";

            if (orderSysNo <= 0)
            {
                return result;
            }

            try
            {
                SoOrder order = BLL.Order.SoOrderBo.Instance.GetEntity(orderSysNo);
                if (order == null)
                {
                    result.Message = "该订单不存在";
                    return result;
                }
                ParaOnlinePaymentFilter filter = new ParaOnlinePaymentFilter();
                filter.OrderSysNo = orderSysNo;
                var onlinePayment = BLL.Finance.FinanceBo.Instance.GetOnlinePayments(filter).Rows.Where(o => o.Status == 1).FirstOrDefault();
                if (onlinePayment == null)
                {
                    result.Message = "订单不存在在线支付记录";
                    return result;
                }

                Dictionary<string, string> paramsData = new Dictionary<string, string>();
                paramsData.Add("reference_no", onlinePayment.BusinessOrderSysNo);
                string postData = InitParams(paramsData);
                string responseStr = GetResponse(url, postData);
                result = GetResult(JObject.Parse(responseStr), result);
                if (result.Status == true)
                {
                    // 删除物流返回订单记录
                    //BLL.CrossBorderLogistics.CrossBorderLogisticsOrderBo.Instance.DeleteByOrderSysNo(orderSysNo);
                    BLL.Order.SoOrderSyncLogisticsLogBo.Instance.DeleteByOrderSysNoAndCode(orderSysNo, (int)Code);
                    BLL.Order.SoOrderBo.Instance.UpdateOrderApiStatus(0, 3, orderSysNo);
                }
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
        /// <remarks>2016-04-09 陈海裕 创建</remarks>
        public override Result<string> GetLogisticsTracking(int orderSysNo)
        {
            Result<string> result = new Result<string>();
            result.Status = false;
            result.StatusCode = 0;
            result.Message = "接口调用失败";
            string url = RequestUrl + "order/tracking";

            if (orderSysNo <= 0)
            {
                return result;
            }

            var orderInfo = BLL.Order.SoOrderBo.Instance.GetEntity(orderSysNo);
            ParaOnlinePaymentFilter filter = new ParaOnlinePaymentFilter();
            filter.OrderSysNo = orderSysNo;
            var onlinePayment = BLL.Finance.FinanceBo.Instance.GetOnlinePayments(filter).Rows.Where(o => o.Status == 1).FirstOrDefault();

            if (onlinePayment == null)
            {
                result.Message = "订单不存在在线支付记录";
                return result;
            }

            try
            {
                Dictionary<string, string> paramsData = new Dictionary<string, string>();
                paramsData.Add("reference_no", onlinePayment.BusinessOrderSysNo);
                paramsData.Add("type", "reference_no");

                string responseStr = GetResponse(url, InitParams(paramsData));
                var jsonResult = JObject.Parse(responseStr);

                if (jsonResult.Property("state") != null && jsonResult["state"].ToString() == "True")
                {
                    result.Status = true;
                    result.StatusCode = 1;
                    result.Message = "接口调用成功";

                    #region 查询订单
                    url = RequestUrl + "order/view";
                    responseStr = GetResponse(url, InitParams(paramsData));
                    var _jsonResult = JObject.Parse(responseStr);
                    if (_jsonResult.Property("state") != null && _jsonResult["state"].ToString().ToLower() == "true")
                    {
                        string tracking_number = _jsonResult["data"]["tracking_number"].ToString();
                        if (tracking_number != "" && orderInfo.CBLogisticsSendStatus != (int)Hyt.Model.WorkflowStatus.OrderStatus.跨境物流推送状态.成功)
                        {

                            if (orderInfo.Status == (int)OrderStatus.销售单状态.待创建出库单)
                            {
                                //BLL.Order.SoOrderBo.Instance.Ship(orderSysNo,tracking_number);   
                            }

                            //BLL.Order.SoOrderBo.Instance.UpdateOrderApiStatus((int)Hyt.Model.WorkflowStatus.OrderStatus.跨境物流推送状态.成功, 3, orderSysNo);
                        }
                        else
                        {
                            result.Status = false;
                            result.Message = "等待友信达发货,请稍后再试！";
                        }


                    }
                    else
                    {
                        result.Status = false;
                        result.Message = _jsonResult["message"].ToString();
                    }

                    #endregion


                    result.Status = true;
                    result.Message = "接口调用成功";
                    result.StatusCode = 10;
                    result.Data = jsonResult["data"].ToString();
                }
                else
                {
                    result.Message += jsonResult["message"].ToString();
                }

                return result;
            }
            catch (Exception ex)
            {
                result.Message = "查询订单物流状态报错：" + ex.StackTrace;
                return result;
            }
        }

        private class TrackingData
        {
            public string time { get; set; }
            public string content { get; set; }
            public string code { get; set; }
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
            string url = RequestUrl + "express/create";

            try
            {
                SoOrder order = BLL.Order.SoOrderBo.Instance.GetEntity(TConvert.ToInt32(orderId));
                if (order == null)
                {
                    result.Message = "该订单不存在";
                    return result;
                }
                LgDeliveryType deliveryType = BLL.Logistics.DeliveryTypeBo.Instance.GetDeliveryType(order.DeliveryTypeSysNo);
                ParaOnlinePaymentFilter filter = new ParaOnlinePaymentFilter();
                filter.OrderSysNo = TConvert.ToInt32(orderId);
                var onlinePayment = BLL.Finance.FinanceBo.Instance.GetOnlinePayments(filter).Rows.Where(o => o.Status == 1).FirstOrDefault();
                CBWhWarehouse warehouse = BLL.Warehouse.WhWarehouseBo.Instance.GetWarehouse(order.DefaultWarehouseSysNo);
                string _deliveryType = "";
                if (deliveryType.DeliveryTypeName.Contains("顺丰"))
                {
                    _deliveryType = "SF";
                    //CustomerCode = SFCustomerCode;
                    //Key = SFKey;
                    Token = SFToken;
                }
                else if (deliveryType.DeliveryTypeName.Contains("中通"))
                {
                    _deliveryType = "ZTO";
                }
                if (_deliveryType == "")
                {
                    result.Message = "友信达物流目前只有支持顺丰和中通两种快递方式";
                    return result;
                }

                Dictionary<string, string> paramsData = new Dictionary<string, string>();
                paramsData.Add("reference_no", onlinePayment.BusinessOrderSysNo);
                paramsData.Add("warehouse_id", warehouse.LogisWarehouseCode);
                paramsData.Add("sm_code", _deliveryType);
                string responseStr = GetResponse(url, InitParams(paramsData));
                result = GetResult(JObject.Parse(responseStr), result);
                if (result.Status == true)
                {
                    // 生成出库单

                }

                return result;
            }
            catch (Exception ex)
            {
                result.Message = "查询订单运单号报错：" + ex.StackTrace;
                return result;
            }
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

            string url = RequestUrl + "common";
            /*
             * customer/common/资源名/page/页码/page/pagesize/每页返回多少条
             * 
             * 仓库的运输（快递）类型：shipmentMethod
             * 仓库信息：customerWarehouse
             * 国家信息：country
             * 地区信息：area
             * 货币类型：currency
             * 港口信息：port
             * 包装类型：warpType
             * 产品单位：productUom
             * 证件类型：cardType
             */
            switch (type)
            {
                case 1:
                    url += "/shipmentMethod";
                    break;
                case 2:
                    url += "/customerWarehouse";
                    break;
                case 3:
                    url += "/country";
                    break;
                case 4:
                    url += "/area";
                    break;
                case 5:
                    url += "/currency";
                    break;
                case 6:
                    url += "/port";
                    break;
                case 7:
                    url += "/warpType";
                    break;
                case 8:
                    url += "/productUom";
                    break;
                case 9:
                    url += "/cardType";
                    break;
                default:
                    result.Message = "资源名不正确";
                    return result;
            }

            Dictionary<string, string> paramsData = new Dictionary<string, string>();
            paramsData.Add("page", "1");
            paramsData.Add("pagesize", "500");
            string responseStr = GetResponse(url, InitParams(paramsData));
            result = GetResult(JObject.Parse(responseStr), result);

            return result;
        }
        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="paramsData">pos参数</param>
        /// <returns></returns>
        /// <remarks>2016-7-29 杨浩创建</remarks>
        private string InitParams(Dictionary<string, string> paramsData)
        {
            paramsData = paramsData.OrderBy(o => o.Key).ToDictionary(k => k.Key, v => v.Value);
            string timerstamp = CreateTimeStamp();
            string postData = "";
            foreach (var i in paramsData)
            {
                postData += i.Key + "=" + i.Value + "&";
            }
            if (postData.Length > 0)
            {
                postData = postData.Substring(0, postData.Length - 1);
            }
            return postData;
        }
        /// <summary>
        /// 提交报文至有信达
        /// </summary>
        /// <param name="url">有信达API网关</param>
        /// <param name="postData">报文内容</param>
        /// <returns></returns>
        /// <remarks>2016-7-29 杨浩 重构</remarks>
        private string GetResponse(string url, string postData, string timerstamp="",string keydata="")
        {
            if(string.IsNullOrEmpty(timerstamp))
            {
                timerstamp = CreateTimeStamp();
            }
           
            // 记录推送前参数
            //BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "物流接口" + this.Code + ",url:" + url + "，Before Post：\n" + postData, LogStatus.系统日志目标类型.订单, _orderSysNo, 0);

            HttpWebRequest request = WebUtil.GetWebRequest(url, "post");
            request.Headers.Add("CUSTOMERCODE", CustomerCode);
            request.Headers.Add("DIGEST", CreateDigest(postData, timerstamp));
            request.Headers.Add("REQUESTTIME", timerstamp);
            request.ContentType = "application/x-www-form-urlencoded;charset=utf-8";

            //postData = postData.Replace(keydata,"");

            byte[] data = Encoding.UTF8.GetBytes(postData);

            request.ContentLength = data.Length;
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.111 Safari/537.36";
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            string respStr = "";
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    respStr = reader.ReadToEnd();
                }
            }

            // 记录推送后返回结果
            //BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "物流接口" + this.Code + ",url:" + url + "，Response：\n" + JObject.Parse(respStr),
            // LogStatus.系统日志目标类型.订单, _orderSysNo, 0);

            return respStr;
        }

        private Result<string> GetResult(JObject back, Result<string> result)
        {
            if (back.Property("state") != null && back["state"].ToString() == "True")
            {
                result.Status = true;
                result.StatusCode = 1;
                result.Message = "接口调用成功";
                result.Data = back["data"].ToString();
            }
            else
            {
                result.Message += back["message"].ToString();
            }

            return result;
        }

        public string CreateDigest(string postData, string timerStamp)
        {
            Byte[] bytes = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(postData + Key + timerStamp));

            string ret = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                ret += Convert.ToString(bytes[i], 16).PadLeft(2, '0');
            }

            return ret.PadLeft(32, '0').ToUpper();
        }

        public string CreateTimeStamp(DateTime? oTime = null)
        {
            DateTime _time = oTime == null ? DateTime.Now : (DateTime)oTime;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return ((int)(_time - startTime).TotalSeconds).ToString();
        }
    }

    public class YSArea
    {
        public int id { get; set; }
        public string area_name { get; set; }
        public string old_name { get; set; }
        public int pid { get; set; }
        public int level { get; set; }
    }

    /// <summary>
    /// 有信达订单实体类
    /// </summary>
    /// <remarks>2016-03-25 陈海裕 创建</remarks>
    public class YSOrder
    {
        /// <summary>
        /// 交货仓ID（交货仓接口获取）int 必填
        /// </summary>
        public string warehouse_id { get; set; }
        /// <summary>
        /// 交易订单号 String(128) 必填
        /// </summary>
        public string reference_no { get; set; }
        /// <summary>
        /// 支付号（平台返回的交易号，三单对碰使用） String(64) 必填
        /// </summary>
        public string pay_no { get; set; }
        /// <summary>
        /// 支付企业名称 String(32) 必填
        /// </summary>
        public string pay_company { get; set; }
        /// <summary>
        /// 支付企业在跨境公共平台备案后获得的备案编号，
        /// 无支付企业的备案编号时可填电商企业的备案编号 String(64) 必填
        /// </summary>
        public string pay_company_no { get; set; }
        /// <summary>
        /// 快递方式（通过接口获取）String(16) 必填
        /// </summary>
        public string sm_code { get; set; }
        /// <summary>
        /// 币种（通过接口获取）String(8) 必填
        /// </summary>
        public string currency_code { get; set; }
        /// <summary>
        /// 运费 Float 必填
        /// </summary>
        public float freight { get; set; }
        /// <summary>
        /// 是否报价(0 ：是， 1：否 )运输方式 SF 支持 int  必填
        /// </summary>
        public int is_valuation { get; set; }
        /// <summary>
        /// 毛重 float 必填
        /// </summary>
        public float gross_weight { get; set; }
        /// <summary>
        /// 收件人省份 int 必填
        /// </summary>
        public int receiver_province_id { get; set; }
        /// <summary>
        /// 收件人城市 int 必填
        /// </summary>
        public int receiver_city_id { get; set; }
        /// <summary>
        /// 收件人区或者县，地接口获取 int 必填
        /// </summary>
        public int receiver_district_id { get; set; }
        /// <summary>
        /// 收件人地址，不带省市区县 string(128) 必填
        /// </summary>
        public string receiver_address { get; set; }
        /// <summary>
        /// 收件人姓名 String(128) 必填
        /// </summary>
        public string receiver_name { get; set; }
        /// <summary>
        /// 收件人邮编 String(6) 必填
        /// </summary>
        public string receiver_postcode { get; set; }
        /// <summary>
        /// 收件人手机（手机电话二选一）String(11) 必填
        /// </summary>
        public string receiver_tel { get; set; }
        /// <summary>
        /// 收件人电话（手机电话二选一） String(32) 必填
        /// </summary>
        public string receiver_tel2 { get; set; }
        /// <summary>
        /// 证件类型接口获取 String(10) 必填
        /// </summary>
        public string card_type { get; set; }
        /// <summary>
        /// 证件号码 String(64) 必填
        /// </summary>
        public string card_no { get; set; }
        /// <summary>
        /// 发件人名称 String(32) 必填
        /// </summary>
        public string shipper_name { get; set; }
        /// <summary>
        /// 发件人国家代码(接口获取) String(3) 必填
        /// </summary>
        public string shipper_country { get; set; }
        /// <summary>
        /// 发件人城市 String(64) 必填
        /// </summary>
        public string shipper_city { get; set; }
        /// <summary>
        /// 发件人电话 string(32) 必填
        /// </summary>
        public string shipper_tel { get; set; }
        /// <summary>
        /// 发件人地址 string(256) 必填
        /// </summary>
        public string shipper_address { get; set; }
        /// <summary>
        /// 订单交易时间，unix时间戳 int 必填
        /// </summary>
        public int order_time { get; set; }
        /// <summary>
        /// 订单包装种类编码 string(32) 必填
        /// </summary>
        public string wrap_type { get; set; }
        /// <summary>
        /// 订单税金，精确2位小数 float 必填
        /// </summary>
        public float tax_amount { get; set; }
        /// <summary>
        /// 订单商品总额，精确2位小数 float 必填
        /// </summary>
        public float total_product_amount { get; set; }
        /// <summary>
        /// 订单支付总额，精确2位小数，为商品总额、运费、税金、保费之和 float 必填
        /// </summary>
        public float total_amount { get; set; }
        /// <summary>
        /// 订单商品详情 items（集合） 必填
        /// </summary>
        public List<YSProduct> products { get; set; }
    }

    public class YSProduct
    {
        /// <summary>
        /// 产品sku String(32)
        /// </summary>
        public string product_sku { get; set; }
        /// <summary>
        /// 供应商编号，如果货品没有供应商管理管理则传空 String(32)
        /// </summary>
        public string supplier_code { get; set; }
        /// <summary>
        /// 产品名称 String(64)
        /// </summary>
        public string product_name { get; set; }
        /// <summary>
        /// 产品数量 int
        /// </summary>
        public int qty { get; set; }
        /// <summary>
        /// 成交单价 float
        /// </summary>
        public float price { get; set; }
    }

    public class YSResult
    {
        /// <summary>
        /// OMS系统订单号 String(32)
        /// </summary>
        public string order_sn { get; set; }
    }
}
