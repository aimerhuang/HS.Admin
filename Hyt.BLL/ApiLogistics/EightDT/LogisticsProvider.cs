using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Util;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Hyt.BLL.ApiLogistics.EightDT
{
    /// <summary>
    /// 八达通物流接口
    /// </summary>
    /// <remarks>
    /// 2016-05-14 陈海裕 创建
    /// </remarks>
    public class LogisticsProvider : ILogisticsProvider
    {
        #region 测试环境
        //const string Secretkey = "ba02c653-7159-42eb-89ca-0debec9fc5eb80040";
        //const string CoustomerID = "80040";
        #endregion
        const string Secretkey = "879ac1ce-9587-40f4-9e47-7c087bbcf19780107";
        const string CoustomerID = "80107";

        public LogisticsProvider() { }

        private static object lockHelper = new object();

        /// <summary>
        /// 订单id，用于日志记录
        /// </summary>
        private int _orderSysNo = 0;

        /// <summary>
        /// 物流
        /// </summary>
        public override Hyt.Model.CommonEnum.物流代码 Code
        {
            get { return Hyt.Model.CommonEnum.物流代码.八达通; }
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

            Result result = new Result();
            result.Status = false;
            result.StatusCode = 0;
            result.Message = "接口调用失败";

            if (orderSysno <= 0)
            {
                return result;
            }

            lock (lockHelper)
            {
                try
                {
                    SoOrder order = BLL.Order.SoOrderBo.Instance.GetEntity(orderSysno);
                    if (order == null)
                    {
                        result.Message = "该订单不存在";
                        return result;
                    }
                    if (order.CBLogisticsSendStatus == 1)
                    {
                        result.Message = "该订单已推送，不能重复操作";
                        return result;
                    }
                    order.ReceiveAddress = BLL.Order.SoOrderBo.Instance.GetOrderReceiveAddress(order.ReceiveAddressSysNo);
                    order.OrderItemList = DataAccess.Order.ISoOrderItemDao.Instance.GetOrderItemsByOrderSysNo(order.SysNo);
                    ParaVoucherFilter voucherFilter = new ParaVoucherFilter();
                    voucherFilter.SourceSysNo = order.SysNo;
                    CBFnReceiptVoucher recVoucher = BLL.Finance.FinanceBo.Instance.GetReceiptVouchers(voucherFilter).Rows.FirstOrDefault();
                    recVoucher.VoucherItems = BLL.Finance.FnReceiptVoucherBo.Instance.GetReceiptVoucherItem(recVoucher.SysNo);
                    // 收货人 区 市 省
                    BsArea receiverDistrict = BLL.Basic.BasicAreaBo.Instance.GetArea(order.ReceiveAddress.AreaSysNo);
                    BsArea receiverCity = BLL.Basic.BasicAreaBo.Instance.GetArea(receiverDistrict.ParentSysNo);
                    BsArea receiverProvince = BLL.Basic.BasicAreaBo.Instance.GetArea(receiverCity.ParentSysNo);
                    // 发货人 市
                    CBWhWarehouse warehouse = BLL.Warehouse.WhWarehouseBo.Instance.GetWarehouse(order.DefaultWarehouseSysNo);
                    BsArea shipperCity = BLL.Basic.BasicAreaBo.Instance.GetArea(warehouse.CitySysNo);

                    DsDealer dealer = BLL.Distribution.DsDealerBo.Instance.GetDsDealer(order.DealerSysNo);

                    LgDeliveryType deliveryType = BLL.Logistics.DeliveryTypeBo.Instance.GetDeliveryType(order.DeliveryTypeSysNo);

                    EDTOrder newOrder = new EDTOrder();
                    newOrder.Address1 = order.ReceiveAddress.StreetAddress;
                    //newOrder.Base_ChannelInfoID = "1";
                    newOrder.Base_ChannelInfoID = "HKKJ";
                    newOrder.City = receiverCity.AreaName;
                    newOrder.ConsigneeName = order.ReceiveAddress.Name;
                    newOrder.Contact = !string.IsNullOrWhiteSpace(order.ReceiveAddress.MobilePhoneNumber) ? order.ReceiveAddress.MobilePhoneNumber : order.ReceiveAddress.PhoneNumber;
                    newOrder.Country = "CN";
                    newOrder.CusRemark = order.Remarks;
                    newOrder.GFF_CustomerID = CoustomerID;
                    newOrder.OrderStatus = "3";
                    newOrder.State = receiverProvince.AreaName;
                    newOrder.Style = "1";
                    //newOrder.ShippingService = "GZBC";
                    List<EDTOrderProduct> productList = new List<EDTOrderProduct>();
                    EDTOrderProduct product = null;
                    foreach (var item in order.OrderItemList)
                    {
                        PdProductStock productStock = BLL.Warehouse.PdProductStockBo.Instance.GetEntityByWP(order.DefaultWarehouseSysNo, item.ProductSysNo);
                        product = new EDTOrderProduct();
                        product.CnName = item.ProductName;
                        product.EnName = "";
                        product.MaterialRefNo = TConvert.ToString(productStock.ProductSku);
                        product.Price = item.SalesUnitPrice.ToString();
                        product.Quantity = item.Quantity.ToString();
                        product.Weight = "";
                        product.WarehouseID = warehouse.LogisWarehouseCode;
                        productList.Add(product);
                    }

                    if (product == null)
                    {
                        result.Message = "订单明细不存在";
                        return result;
                    }

                    StringBuilder strorderinfo = new StringBuilder();
                    strorderinfo.Append("Style:" + newOrder.Style + ";");
                    strorderinfo.Append("GFF_CustomerID:" + newOrder.GFF_CustomerID + ";");
                    strorderinfo.Append("GFF_ReceiveSendAddressID:;");
                    strorderinfo.Append("ConsigneeName:" + newOrder.ConsigneeName + ";");
                    strorderinfo.Append("Country:" + newOrder.Country + ";");
                    strorderinfo.Append("Base_ChannelInfoID:" + newOrder.Base_ChannelInfoID + ";");
                    strorderinfo.Append("State:" + newOrder.State + ";");
                    strorderinfo.Append("City:" + newOrder.City + ";");
                    strorderinfo.Append("OrderStatus:" + newOrder.OrderStatus + ";");
                    strorderinfo.Append("Address1:" + newOrder.Address1 + ";");
                    strorderinfo.Append("Address2:;");
                    strorderinfo.Append("CsRefNo:;");
                    strorderinfo.Append("Zipcode:;");
                    strorderinfo.Append("Contact:" + newOrder.Contact + ";");
                    strorderinfo.Append("CusRemark:" + newOrder.CusRemark + ";");
                    strorderinfo.Append("TrackingNo:;");
                    //strorderinfo.Append("ShippingService:" + newOrder.ShippingService + ";");
                    StringBuilder strorderproduct = new StringBuilder();
                    for (int i = 0; i < productList.Count; i++)
                    {
                        strorderproduct.Append("MaterialRefNo:" + productList[i].MaterialRefNo + ",");
                        //strorderproduct.Append("MaterialRefNo:123456,");
                        strorderproduct.Append("Quantity:" + productList[i].Quantity + ",");
                        strorderproduct.Append("Price:" + productList[i].Price + ",");
                        strorderproduct.Append("Weight:" + productList[i].Weight + ",");
                        strorderproduct.Append("EnName:" + productList[i].EnName + ",");
                        strorderproduct.Append("WarehouseID:" + productList[i].WarehouseID + ",");
                        strorderproduct.Append("ProducingArea:,");
                        productList[i].CnName = Regex.Replace(productList[i].CnName, @"[/\(\)（）,]", "");
                        strorderproduct.Append("CnName:" + productList[i].CnName + ",;");
                    }

                    ServiceRefEightDT.APIWebServiceSoapClient newService = new ServiceRefEightDT.APIWebServiceSoapClient();

                    string strResult = newService.InsertUpdateOrder(strorderinfo.ToString(), strorderproduct.ToString(), "", Secretkey);
                    if (strResult.Contains("成功"))
                    {
                        try
                        {
                            CrossBorderLogisticsOrder cbOrder = BLL.CrossBorderLogistics.CrossBorderLogisticsOrderBo.Instance.GetEntityByOrderSysNo(orderSysno);
                            if (cbOrder == null)
                            {
                                Model.CrossBorderLogisticsOrder logisticsOrder = new Model.CrossBorderLogisticsOrder();
                                logisticsOrder.SoOrderSysNo = orderSysno;
                                logisticsOrder.LogisticsOrderId = Regex.Match(strResult, "[a-zA-Z0-9]+").ToString();
                                logisticsOrder.LogisticsCode = (int)this.Code;
                                BLL.CrossBorderLogistics.CrossBorderLogisticsOrderBo.Instance.InsertEntity(logisticsOrder);
                            }
                            // 更新订单状态
                            BLL.Order.SoOrderBo.Instance.UpdateOrderApiStatus(1, 3, orderSysno);
                        }
                        catch (Exception ex)
                        {
                            BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, "订单" + orderSysno + "保存跨境物流返回的单号失败。", ex);
                        }

                        result.Status = true;
                    }

                    result.Message = strResult;
                }
                catch (Exception ex)
                {
                    result.Message = "向" + this.Code + "物流推送订单报错：" + ex.StackTrace;
                    return result;
                }
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
            //var tempResult = GetOrderExpressno(orderSysNo.ToString());
            //result.Status = tempResult.Status;
            //result.Message = tempResult.Message;
            //if (result.Status == false)
            //{
            //    return result;
            //}

            CrossBorderLogisticsOrder cbLogiOrder = BLL.CrossBorderLogistics.CrossBorderLogisticsOrderBo.Instance.GetEntityByOrderSysNo(Convert.ToInt32(orderSysNo));
            if (cbLogiOrder == null)
            {
                return result;
            }
            string apiurl = "http://183.11.227.9:90/bdtsys/services/htapi/track/" + cbLogiOrder.LogisticsOrderId;
            System.Net.WebRequest request = System.Net.WebRequest.Create(apiurl);
            request.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            System.Net.WebResponse response = request.GetResponse();
            System.IO.Stream stream = response.GetResponseStream();
            System.Text.Encoding encode = System.Text.Encoding.UTF8;
            System.IO.StreamReader reader = new System.IO.StreamReader(stream, encode);
            result.Message = reader.ReadToEnd();
            JObject jObj = JObject.Parse(result.Message);
            List<TrackingData> logisticsTrackList = new List<TrackingData>();
            if (jObj != null)
            {
                if (jObj.Property("trackSize") != null)
                {
                    TrackingData trackingData = null;
                    int trackSize = TConvert.ToInt32(jObj["trackSize"].ToString());
                    for (int i = trackSize - 1; i >= 0; i--)
                    {
                        trackingData = new TrackingData();
                        trackingData.time = TConvert.ToString(jObj["track" + i]["statustime"].ToString());
                        trackingData.content = TConvert.ToString(jObj["track" + i]["place"].ToString()) + TConvert.ToString(jObj["track" + i]["reason"].ToString());
                        logisticsTrackList.Add(trackingData);
                    }

                    StringBuilder htmlStr = new StringBuilder();
                    foreach (var item in logisticsTrackList)
                    {
                        htmlStr.Append("<tr><td width=\"25%\">" + item.time + "</td><td width=\"75%\">" + item.content + "</td></tr>");
                    }

                    result.Data = htmlStr.ToString();
                    result.Status = true;
                }
            }

            return result;
        }
        /// <summary>
        /// 展示用统一跟踪实体
        /// </summary>
        /// <remarks>2016-05-25 陈海裕 创建</remarks>
        private class TrackingData
        {
            public string time { get; set; }
            public string content { get; set; }
            public string code { get; set; }
        }

        /// <summary>
        /// 获取物流单号
        /// </summary>
        /// <param name="orderSysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-05-25 陈海裕 创建</remarks>
        public override Result GetOrderExpressno(string orderSysNo)
        {
            Result result = new Result();
            try
            {
                CrossBorderLogisticsOrder cbLogiOrder = BLL.CrossBorderLogistics.CrossBorderLogisticsOrderBo.Instance.GetEntityByOrderSysNo(Convert.ToInt32(orderSysNo));
                if (cbLogiOrder != null)
                {
                    ServiceRefEightDT.APIWebServiceSoapClient newService = new ServiceRefEightDT.APIWebServiceSoapClient();
                    string tempR = newService.getPackage(cbLogiOrder.LogisticsOrderId, CoustomerID, Secretkey);
                    List<Package> packageList = Util.Serialization.JsonUtil.ToObject<List<Package>>(tempR);
                    Package package = packageList.FirstOrDefault();
                    if (!string.IsNullOrWhiteSpace(package.TrackingNo))
                    {
                        result.Message = package.TrackingNo;
                        result.Status = true;
                    }
                }
                else
                {
                    result.Message = "找不到对应订单的跨境物流订单号";
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.ToString();
            }

            return result;
        }

        /// <summary>
        /// 根据跨境物流返回信息更新系统订单状态（作废）
        /// </summary>
        /// <param name="orderSysNo"></param>
        /// <param name="OrderStatus"></param>
        /// <remarks>2016-05-30 陈海裕 创建</remarks>
        private void UpdateOrderStatus(int orderSysNo, string OrderStatus)
        {
            /*
             * 八达通订单状态码：
             * 已确认       状态码：3
             * 已付款待发货 状态码：4
             * 未付款待发货 状态码：5
             * 已发货       状态码：6
             * 已完成       状态码：7
             * 已删除       状态码：8
             * 问题件       状态码：9
             * 退件         状态码：10
             * 处理中       状态码：11
             * 缺货         状态码：12
             * 草稿         状态码：1
             */
            int sysOrderStatus = -1;
            switch (OrderStatus)
            {
                case "6":
                    sysOrderStatus = (int)Model.WorkflowStatus.OrderStatus.销售单状态.出库待接收;
                    break;
                case "7":
                    sysOrderStatus = (int)Model.WorkflowStatus.OrderStatus.销售单状态.已完成;
                    break;
            }

            BLL.Order.SoOrderBo.UpdateOrderStatusNew(orderSysNo, sysOrderStatus);
        }

        public override Result GetOrderTrade(string orderSysNo)
        {
            throw new NotImplementedException();
        }

        private string GetResponse(string url, Dictionary<string, string> paramsData)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 转换返回结果
        /// </summary>
        /// <param name="returnStr"></param>
        /// <returns></returns>
        /// <remarks>2016-05-25 陈海裕 创建</remarks>
        private Result GetResult(string returnStr)
        {
            Result result = new Result();
            string outStr = "";
            Regex reg = new Regex(@"(?i)\\u([0-9a-f]{4})");
            outStr = reg.Replace(returnStr, delegate(Match m1)
            {
                return ((char)Convert.ToInt32(m1.Groups[1].Value, 16)).ToString();
            });
            var jObj = JObject.Parse("{}");

            Util.Serialization.JsonUtil.ToObject<Package>(returnStr);

            if (jObj.Property("state") != null && jObj["state"].ToString() == "True")
            {
                result.Status = true;
                result.StatusCode = 1;
                result.Message = "接口调用成功";
            }
            else
            {
                result.Message += jObj["message"].ToString();
            }

            return result;
        }

        private class Package
        {
            public string OrderNo { get; set; }
            public string TrackingNo { get; set; }
            public string field1 { get; set; }
            public string OrderStatus2 { get; set; }
            public string TotalAmount { get; set; }
            public string ChannelCode { get; set; }
            public string EnName { get; set; }
            public string OrderStatus { get; set; }
        }

        public string CreateDigest(string postData, string timerStamp)
        {
            Byte[] bytes = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(postData + Secretkey + timerStamp));

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

        public override Result CancelOrderTrade(int orderSysNo, string reason = "")
        {
            throw new NotImplementedException();
        }
    }

    public class EDTOrder
    {
        /// <summary>
        /// String 必须 订单类型（仓储订单或普通订购单）仓储订单为1，普通订单为2
        /// </summary>
        public string Style { get; set; }
        /// <summary>
        /// String 必须 客户ID 80000
        /// </summary>
        public string GFF_CustomerID { get; set; }
        /// <summary>
        /// String 选填 发件人ID 189
        /// </summary>
        public string GFF_ReceiveSendAddressID { get; set; }
        /// <summary>
        /// String 必须 收件人 小明
        /// </summary>
        public string ConsigneeName { get; set; }
        /// <summary>
        /// String 必须 国家 US;美国;223 (二次代码;中文名称;国家ID都支持填一即可)
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// String 必须 渠道 EUB;89(渠道代码;渠道ID 填一即可)
        /// </summary>
        public string Base_ChannelInfoID { get; set; }
        /// <summary>
        /// String 必须 州 San Fernando
        /// </summary>
        public string State { get; set; }
        /// <summary>
        /// String 必须 城市 Bernardo Ohiggins
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// String 必须 订单状态--(草稿=1),(确认=3) 1
        /// </summary>
        public string OrderStatus { get; set; }
        /// <summary>
        /// String 必须 收件人地址行 1 United States,,,MN.USA
        /// </summary>
        public string Address1 { get; set; }
        /// <summary>
        /// String 可选 收件人地址行 2 United States,,,MN.USA
        /// </summary>
        public string Address2 { get; set; }
        /// <summary>
        /// String 可选 客户参考号 1122
        /// </summary>
        public string CsRefNo { get; set; }
        /// <summary>
        /// String 可选 邮编 2134342
        /// </summary>
        public string Zipcode { get; set; }
        /// <summary>
        /// String 可选 联系方式 180344345665
        /// </summary>
        public string Contact { get; set; }
        /// <summary>
        /// String 可选 客户订单备注 Null
        /// </summary>
        public string CusRemark { get; set; }
        /// <summary>
        /// String 可选 跟踪号 RT209114500HK
        /// </summary>
        public string TrackingNo { get; set; }
        /// <summary>
        /// 说明运输方式代码
        /// </summary>
        //public string ShippingService { get; set; }
    }

    public class EDTOrderProduct
    {
        /// <summary>
        /// String 必须 物品1 产品编号 SKU1
        /// </summary>
        public string MaterialRefNo { get; set; }
        /// <summary>
        /// String 必须 物品1 数量 10
        /// </summary>
        public string Quantity { get; set; }
        /// <summary>
        /// String 必须 物品1 单位价值（美元） 10.2
        /// </summary>
        public string Price { get; set; }
        /// <summary>
        /// String 必须 物品1 重量（KG） 10
        /// </summary>
        public string Weight { get; set; }
        /// <summary>
        /// String 必须 物品1 产品英文名 MP31232
        /// </summary>
        public string EnName { get; set; }
        /// <summary>
        /// String 必须 物品1 仓储ID 302，可使用仓储信息查询api获取
        /// </summary>
        public string WarehouseID { get; set; }
        /// <summary>
        /// String 可选 物品1 原产地 
        /// </summary>
        public string ProducingArea { get; set; }
        /// <summary>
        /// String 必须 物品1 产品中文名 中文名
        /// </summary>
        public string CnName { get; set; }
    }
}
