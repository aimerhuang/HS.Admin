using Hyt.BLL.Order;
using Hyt.Model;
using Hyt.Model.Order;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Util;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.ApiLogistics.Wtd
{
    /// <summary>
    /// 威时沛运物流接口
    /// </summary>
    /// <remarks>
    /// 2016-3-8 杨浩 创建
    /// 2016-4-19 陈海裕 重构
    /// </remarks>
    public class LogisticsProvider : ILogisticsProvider
    {
        #region 字段
        //const string OnNumber = "12386818";
        //const string WhNumber = "STORE_GZNS";
        //const string CopGNo = "HT-B74-000009";
        //const string skucode = "HT-B74-000009-001";
        /// <summary>
        /// API协议版本
        /// </summary>
        private string v = "1.0";
        /// <summary>
        /// 格式化
        /// </summary>
        private string format = "json";
        /// <summary>
        /// api服务url
        /// </summary>
        private string apiUrl = "http://120.26.115.70:6060/wtdex-service/ws/openapi/rest/route";
        //测试环境
        //private string apiUrl = "http://wtd.nat123.net/wtdex-service/ws/openapi/rest/route";
        #endregion

        #region 构造函数
        public LogisticsProvider() { }
        #endregion

        #region 私有方法
        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="source">待加密的明文</param>
        /// <returns></returns>
        /// <remarks>2016-3-8 杨浩 创建</remarks>
        private string EncodeBase64(string source)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(source);
            return Convert.ToBase64String(bytes);
        }
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="param">需要加密的参数</param>
        /// <returns></returns>
        /// <remarks>2016-3-10 杨浩 创建</remarks>
        private string MD5(string param)
        {
            string md5Str = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(param, "MD5").ToLower();
            return md5Str;
            ////传输参数前处理
            //byte[] text = Encoding.UTF8.GetBytes(param);
            //MD5 md5 = new MD5CryptoServiceProvider();
            //byte[] output = md5.ComputeHash(text);
            //return BitConverter.ToString(output).Replace("-", "").ToLower();
        }


        /// <summary>
        /// 获得签名
        /// a)	按照如下顺序拼成字符串：
        ///     secret+appkey+format+ method + params+ timestamp + tocken+ v + secret
        /// b)	使用md5加密
        ///     字符串要保证为utf-8格式的，并使用md5算法签名(结果是小写的)
        /// c)	Base64加密
        ///     将md5加密结果再使用Base64加密，该字符串即为sign。
        /// </summary>
        /// <param name="method">api方法名称</param>
        /// <param name="_params">参数</param>
        /// <param name="timestamp">时间戳</param>
        /// <returns></returns>
        /// <remarks>2016-3-8 杨浩 创建</remarks>
        private string GetSign(string method, string _params, string timestamp)
        {
            string sign = config.Secret + config.AppKey + format + method + _params + timestamp + config.Token + v + config.Secret;
            sign = MD5(sign);
            sign = EncodeBase64(sign);
            return sign;
        }
        /// <summary>
        /// 初始化api
        /// </summary>
        /// <returns></returns>
        /// <param name="method">api方法名称</param>
        /// <param name="_params">参数</param>
        /// <param name="timestamp">时间戳</param>
        /// <remarks>2016-3-8 杨浩 创建</remarks>
        private string InitParams(string method, string _params)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string url = "appkey=" + config.AppKey + "&sign=" + GetSign(method, _params, timestamp) + "&token=" + config.Token + "&timestamp=" + timestamp + "&v=" + v + "&format=" + format + "&method=" + method + "&params=" + System.Web.HttpUtility.UrlEncode(_params, System.Text.UTF8Encoding.UTF8);
            return url;
        }
        #endregion
        /// <summary>
        /// 物流标示
        /// </summary>
        /// <remarks>2016-3-8 杨浩 创建</remarks>
        public override Hyt.Model.CommonEnum.物流代码 Code
        {
            get { return Hyt.Model.CommonEnum.物流代码.威时沛; }
        }
        /// <summary>
        /// 获取回调结果
        /// </summary>
        /// <param name="result">api反馈结果</param>
        /// <returns></returns>
        ///  <remarks>2016-3-8 杨浩 创建</remarks>
        private Result<JToken> GetResponseResult(string responseStr)
        {
            var result = new Result<JToken>()
            {
                Status = false
            };
            var jobject = JObject.Parse(responseStr);
            if (jobject.Property("response") != null)
            {
                var response = jobject["response"];
                result.Data = response;
                string code = response["code"].ToString();
                if (code == "1")
                    result.Status = true;
                else
                {
                    var errorInfos = response["errorInfoList"]["errorInfos"];
                    foreach (var info in errorInfos)
                    {
                        result.Message += info["errMsg"].ToString() + "\r\n";
                    }
                }
            }
            else
            {
                result.Message = responseStr;
            }

            return result;
        }

        ///// <summary>
        ///// 添加产品
        ///// </summary>
        ///// <param name="product">产品</param>
        ///// <remarks>2016-3-8 杨浩 创建</remarks>
        //public override Result AddProduct(PdProduct product)
        //{
        //    string _params = "{\"barcode\":\"20014011\",\"brand\":\"宝洁\",\"categoryName\":\"paper\",\"channel\":\"www.jd.com\",\"childProductList\":{\"childProducts\":[]},\"currency\":\"cny\",\"foreign\":\"china\",\"grossWt\":1,\"imageUrl\":null,\"model\":\"大中型\",\"netWt\":1,\"notes\":null,\"origin\":\"gz\",\"prodAttributeInfo\":\"11:22,33:44\",\"productId\":\"1001\",\"productName\":\"帮宝适纸内裤\",\"quality\":\"perfect\",\"salePrice\":100,\"saleUnits\":\"CNY\",\"unitPrice\":200,\"menuFact\":\"中国\"}";


        //    string method = "wtdex.prod.product.add";

        //    _params = InitParams(method, _params);

        //    var _result = Hyt.Util.WebUtil.PostForm(apiUrl, _params);
        //    return GetResponseResult(_result);
        //}
        /// <summary>
        /// 获取产品详情
        /// </summary>
        /// <param name="productId">商品编码</param>
        /// <returns></returns>
        public override Result GetProduct(string productId)
        {
            string _params = "";
            string method = "wtdex.prod.product.get";
            _params = InitParams(method, _params);

            var _result = Hyt.Util.WebUtil.PostForm(apiUrl, _params);
            return GetResponseResult(_result);
        }
     
        /// <summary>
        /// 添加交易订单
        /// </summary>
        /// <param name="orderId">销售订单系统编号</param>
        /// <returns></returns>
        /// <remarks>
        /// 2016-3-21 杨浩 创建
        /// 2016-4-19 陈海裕 重构
        /// </remarks>
        public override Result AddOrderTrade(int orderId)
        {
            var result = new Result()
            {
                Status = false,
                StatusCode = 0,
                Message = "向" + this.Code + "物流推送订单失败"
            };

   
            try
            {
                var order = Hyt.BLL.Order.SoOrderBo.Instance.GetEntity(orderId);
                if (order == null)
                {
                    result.Status = false;
                    result.Message = string.Format("订单号{0}不存在！", orderId);
                    result.StatusCode = -100;
                }
  
     
                #region 旧参数

                //var receiveAddress = BLL.Order.SoOrderBo.Instance.GetOrderReceiveAddress(order.ReceiveAddressSysNo);
                //if (receiveAddress == null)
                //{
                //    result.Status = false;
                //    result.Message = string.Format("订单中的收货地址编号{0}不存在！", order.ReceiveAddressSysNo);
                //    result.StatusCode = -101;
                //}
                //var area = BLL.Basic.BasicAreaBo.Instance.GetArea(receiveAddress.AreaSysNo);
                //if (area == null)
                //{
                //    result.Status = false;
                //    result.Message = string.Format("地区({0})不存在！", receiveAddress.AreaSysNo);
                //    result.StatusCode = -103;
                //}
                //var city = BLL.Basic.BasicAreaBo.Instance.GetArea(area.ParentSysNo);
                //if (city == null)
                //{
                //    result.Status = false;
                //    result.Message = string.Format("城市({0})不存在！", area.ParentSysNo);
                //    result.StatusCode = -104;
                //}
                //var province = BLL.Basic.BasicAreaBo.Instance.GetArea(city.ParentSysNo);
                //if (province == null)
                //{
                //    result.Status = false;
                //    result.Message = string.Format("省份({0})不存在！", city.ParentSysNo);
                //    result.StatusCode = -105;
                //}

                //if (!result.Status)
                //    return result;
                //var param = "{\"orderId\":\"" + order.OrderNo + "\",";
                //param += "\"addr\":\"" + receiveAddress.StreetAddress + "\",";
                //param += "\"phone\":\"" + receiveAddress.MobilePhoneNumber + "\",";
                //param += "\"freight\":" + order.FreightAmount + ",";//运费
                //param += "\"orderDate\":\"" + order.CreateDate.ToString("yyyy-MM-dd") + "\",";
                //param += "\"province\":\"" + JsonStrEscape(province.AreaName) + "\",";
                //param += "\"city\":\"" + JsonStrEscape(city.AreaName) + "\",";
                //param += "\"area\":\"" + JsonStrEscape(area.AreaName) + "\",";
                //param += "\"total\":" + order.ProductAmount + ",";//订单总金额
                //param += "\"insuredFee\":0,";  //保价费用
                //param += "\"cardType\":\"0\","; //证件类型 (默认身份证则填0)
                //param += "\"expressCode\":\"SF\","; //物流公司编码
                //param += "\"busiMode\":\"BBC\","; //所属业务模式 (BBC/备货BC/直邮BC/个人物品)
                //param += "\"whCode\":\"FLC\",";   //发货仓库代号
                //param += "\"portCode\":\"5141\","; //口岸编码
                //param += "\"platformCode\":\"IE150604146855\",";//海关企业备案号
                //param += "\"cardNo\":\"" + receiveAddress.IDCardNo + "\",";//顾客身份证
                //param += "\"name\":\"" + JsonStrEscape(receiveAddress.Name) + "\",";
                ////param += "\"senderContact\":\"测试公司\",";
                ////param += "\"senderPhone\":\"13500000000\",";
                ////param += "\"tax\":\"" + orderInfo.TaxFee + "\",";
                //param += "\"orderItemList\":{";
                //param += "\"orderItems\":[";
                //var orderItemList = Hyt.BLL.Web.SoOrderBo.Instance.GetOrderItemListByOrderSysNo(orderId);
                //string orderItem = "";
                //foreach (var item in orderItemList)
                //{
                //    if (orderItem != "")
                //        orderItem += ",";
                //    orderItem += "{";
                //    //8712400110037
                //    //orderItem += "\"qty\":" + item.Quantity + ",\"price\":" + item.SalesUnitPrice + ",\"total\":" + item.SalesAmount + ",\"productId\":\""+item.ErpCode+"\",\"productName\":\"" + JsonStrEscape(item.ProductName) + "\"";     
                //    orderItem += "\"qty\":" + item.Quantity + ",\"price\":" + item.SalesUnitPrice + ",\"total\":" + item.SalesAmount + ",\"productId\":\"8712400110136\",\"productName\":\"" + JsonStrEscape(item.ProductName) + "\"";
                //    orderItem += "}";
                //}

                //param += orderItem;
                //param += "]}}";
                #endregion
                string method = "wtdex.trade.order.add";

                order.ReceiveAddress = BLL.Order.SoOrderBo.Instance.GetOrderReceiveAddress(order.ReceiveAddressSysNo);
                order.OrderItemList = DataAccess.Order.ISoOrderItemDao.Instance.GetOrderItemsByOrderSysNo(order.SysNo);
                //ParaVoucherFilter voucherFilter = new ParaVoucherFilter();
                //voucherFilter.SourceSysNo = order.SysNo;
                //CBFnReceiptVoucher recVoucher = BLL.Finance.FinanceBo.Instance.GetReceiptVouchers(voucherFilter).Rows.FirstOrDefault();
                //recVoucher.VoucherItems = BLL.Finance.FnReceiptVoucherBo.Instance.GetReceiptVoucherItem(recVoucher.SysNo);


                var filter = new ParaOnlinePaymentFilter();
                filter.OrderSysNo = orderId;
                var onlinePayment = BLL.Finance.FinanceBo.Instance.GetOnlinePayments(filter).Rows.Where(o => o.Status == 1).FirstOrDefault();

                if (onlinePayment == null)
                {
                    result.Message = "订单不存在在线支付记录";
                    return result;
                }
                // 收货人 区 市 省
                BsArea receiverDistrict = BLL.Basic.BasicAreaBo.Instance.GetArea(order.ReceiveAddress.AreaSysNo);
                BsArea receiverCity = BLL.Basic.BasicAreaBo.Instance.GetArea(receiverDistrict.ParentSysNo);
                BsArea receiverProvince = BLL.Basic.BasicAreaBo.Instance.GetArea(receiverCity.ParentSysNo);
                // 发货人 市
                CBWhWarehouse warehouse = BLL.Warehouse.WhWarehouseBo.Instance.GetWarehouse(order.DefaultWarehouseSysNo);
                BsArea shipperCity = BLL.Basic.BasicAreaBo.Instance.GetArea(warehouse.CitySysNo);

                DsDealer dealer = BLL.Distribution.DsDealerBo.Instance.GetDsDealer(order.DealerSysNo);

                LgDeliveryType deliveryType = BLL.Logistics.DeliveryTypeBo.Instance.GetDeliveryType(order.DeliveryTypeSysNo);

        

                var newOrder = new WTDOrder();
                newOrder.addr = TConvert.ToString(order.ReceiveAddress.StreetAddress).Trim();
                newOrder.area = TConvert.ToString(receiverDistrict.AreaName).Trim();
                newOrder.busiMode = "BC";
                newOrder.cardNo = TConvert.ToString(order.ReceiveAddress.IDCardNo);
                newOrder.cardType = "0";
                newOrder.city = TConvert.ToString(receiverCity.AreaName).Trim();
                newOrder.consumerAddr = TConvert.ToString(order.ReceiveAddress.StreetAddress).Trim();
                newOrder.consumerArea = TConvert.ToString(receiverDistrict.AreaName).Trim();
                newOrder.consumerCity = TConvert.ToString(receiverCity.AreaName).Trim();
                newOrder.consumerName = TConvert.ToString(order.ReceiveAddress.Name);
                newOrder.consumerPhone = !string.IsNullOrWhiteSpace(order.ReceiveAddress.MobilePhoneNumber) ? order.ReceiveAddress.MobilePhoneNumber : order.ReceiveAddress.PhoneNumber;
                newOrder.consumerProvince = TConvert.ToString(receiverProvince.AreaName).Trim();
                newOrder.destCity = null;
                newOrder.expressCode = "YT";// "SF";
                newOrder.expressNo = null;
                newOrder.freight = (double)order.FreightAmount;
                newOrder.ieFlag = null;
                newOrder.insuredFee = 0d;
                newOrder.invoice = null;
                newOrder.invoiceHead = null;
                newOrder.invoiceType = null;
                newOrder.isgac =1;
                newOrder.isactpay ="0";
                newOrder.isactlogistics = "1";



                newOrder.mobile = null;
                newOrder.name = TConvert.ToString(order.ReceiveAddress.Name);
                newOrder.notes = TConvert.ToString(order.Remarks).Trim();
                newOrder.orderDate = order.CreateDate.ToString("yyyy-MM-dd");
                newOrder.orderId = onlinePayment.BusinessOrderSysNo;
                newOrder.paymentCode = null;
                newOrder.phone = !string.IsNullOrWhiteSpace(order.ReceiveAddress.MobilePhoneNumber) ? order.ReceiveAddress.MobilePhoneNumber : order.ReceiveAddress.PhoneNumber;

                newOrder.pickMode = "0";//BC、PS情况下必填0:散货/包裹1:大货/备货,有货物在我们仓库
                newOrder.platformCode = config.PlatformCode;
                newOrder.portCode = config.PortCode;
                newOrder.prodFlag = null;
                newOrder.province = TConvert.ToString(receiverProvince.AreaName).Trim();
                newOrder.seller = null;
                newOrder.senderContact = TConvert.ToString(dealer.ErpName);
                newOrder.senderPhone = !string.IsNullOrWhiteSpace(dealer.MobilePhoneNumber) ? dealer.MobilePhoneNumber : dealer.PhoneNumber;
                newOrder.tax = (double)order.TaxFee;
                newOrder.total = (double)order.ProductAmount;
                //newOrder.total = 100d;
                newOrder.whCode = TConvert.ToString(warehouse.LogisWarehouseCode);
                newOrder.zipCode = null;
                newOrder.orderItemList = new WTDOrderItemList();
                newOrder.orderItemList.orderItems = new List<WTDOrderItem>();
                WTDOrderItem orderItem = new WTDOrderItem();
                foreach (var item in order.OrderItemList)
                {
                    var product = BLL.Product.PdProductBo.Instance.GetProductNoCache(item.ProductSysNo);
                    orderItem.groupBarcode = null;
                    orderItem.isGroup = null;
                    orderItem.postTax = null;
                    orderItem.price = (double)item.SalesUnitPrice;
                    orderItem.productId = product.ErpCode;
                    orderItem.productName = TConvert.ToString(item.ProductName);
                    orderItem.qty = item.Quantity;
                    orderItem.total = (double)item.SalesAmount;
                    newOrder.orderItemList.orderItems.Add(orderItem);
                    //orderItem.groupBarcode = null;
                    //orderItem.isGroup = null;
                    //orderItem.postTax = null;
                    //orderItem.price = 50d;
                    //orderItem.productId = "8712400110136";
                    //orderItem.productName = TConvert.ToString(item.ProductName);
                    //orderItem.qty = 2;
                    //orderItem.total = 100d;
                    //newOrder.orderItemList.orderItems.Add(orderItem);
                    //break;
                }

                string postData = Util.Serialization.JsonUtil.ToJson2(newOrder);
                postData = InitParams(method, postData);

                var _result = Hyt.Util.WebUtil.PostForm(apiUrl, postData);

                result = GetResponseResult(_result);

                if (result.Status == true)
                {

                    var soOrderSyncLogisticsLog = new SoOrderSyncLogisticsLog();

                    soOrderSyncLogisticsLog.OrderSysNo = orderId;
                    soOrderSyncLogisticsLog.Code = (int)this.Code;

                    soOrderSyncLogisticsLog.LastUpdateDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    soOrderSyncLogisticsLog.StatusCode = "";
                    soOrderSyncLogisticsLog.StatusMsg = "";
                    soOrderSyncLogisticsLog.Packets = postData;
                    soOrderSyncLogisticsLog.ReceiptContent = _result;

                    soOrderSyncLogisticsLog.LastUpdateBy = 0;
                    soOrderSyncLogisticsLog.LogisticsOrderId ="";

                    soOrderSyncLogisticsLog.CreateDate = DateTime.Now;
                    soOrderSyncLogisticsLog.CreatedBy = BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo;
                    SoOrderSyncLogisticsLogBo.Instance.Insert(soOrderSyncLogisticsLog);


                    BLL.Order.SoOrderBo.Instance.UpdateOrderApiStatus(1, 3, orderId);

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
        /// 查询订单运单号信息
        /// </summary>
        /// <param name="orderId">订单号(多个用逗号隔开,每次最大50个订单)</param>
        /// <returns></returns>
        /// <remarks>2016-3-22 杨浩 创建</remarks>
        public override Result GetOrderExpressno(string orderId)
        {
            var param = "{\"orderId\":\"" + orderId + "\"}";

            string method = "wtdex.trade.orderexpressno.get";
            param = InitParams(method, param);

            var _result = Hyt.Util.WebUtil.PostForm(apiUrl, param);
            var result = GetResponseResult(_result);
            if (result.Status == true)
            {
                if (BLL.CrossBorderLogistics.CrossBorderLogisticsOrderBo.Instance.GetEntityByOrderSysNo(TConvert.ToInt32(orderId)) == null)
                {
                    try
                    {
                        Model.CrossBorderLogisticsOrder logisticsOrder = new Model.CrossBorderLogisticsOrder();
                        logisticsOrder.SoOrderSysNo = TConvert.ToInt32(orderId);
                        logisticsOrder.LogisticsOrderId = JObject.Parse(_result)["response"]["orderList"]["orders"][0]["expressNo"].ToString();
                        logisticsOrder.LogisticsCode = (int)this.Code;

                        BLL.CrossBorderLogistics.CrossBorderLogisticsOrderBo.Instance.InsertEntity(logisticsOrder);
                    }
                    catch (Exception ex)
                    {
                        result.Message = "向" + this.Code + "物流查询订单运单号报错：" + ex.StackTrace;
                        return result;
                    }
                }
            }

            return result;
        }
        /// <summary>
        /// 查询订单信息
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <returns></returns>
        /// <remarks>2016-3-22 杨浩 创建</remarks>
        public override Result GetOrderTrade(string orderId)
        {
            var param = "{\"fields\":\"notes,state,wtdOrderId,expreeNo,expressName\",\"orderId\":\"" + orderId + "\"}";

            string method = "wtdex.trade.orderexpressno.get";
            param = InitParams(method, param);
            var _result = Hyt.Util.WebUtil.PostForm(apiUrl, param);
            var result = GetResponseResult(_result);
            if (result.Status)
            {
                var orders = result.Data["orderList"]["orders"];
            }
            return result;
        }

        /// <summary>
        /// 查询订单物流信息
        /// </summary>
        /// <param name="orderSysNo">订单号</param>
        /// <returns></returns>
        /// <remarks>2016-04-18 陈海裕 创建</remarks>
        /// <remarks>2016-08-18 杨  浩 重构</remarks>
        public override Result<string> GetLogisticsTracking(int orderSysNo)
        {
            var result = new Result<string>();
            result.Status = false;
            result.StatusCode = 0;
            result.Message = "接口调用失败";

         
            var orderSyncLogisticsLogInfo=BLL.Order.SoOrderSyncLogisticsLogBo.Instance.GetModel(orderSysNo,(int)this.Code);

            if (orderSyncLogisticsLogInfo == null)
            {
                result.Message = "没有找到物流订单号!";
                return result;
            }

            //var param = "{\"orderId\":\"" + orderSysNo + "\"}";
            //11001000345610S000a 11001000345610S0004

            var param = "{\"orderId\":\"" + orderSyncLogisticsLogInfo.LogisticsOrderId + "\"}";

            string method = "wtdex.trade.express.get";
            param = InitParams(method, param);

            var _result = Hyt.Util.WebUtil.PostForm(apiUrl, param);

            var JResult = GetResponseResult(_result);

            var responseData = JObject.Parse(JResult.Data.ToString());
            if (JResult.Status == true)
            {
                result.Status = true;
                result.StatusCode = 1;
                result.Message = "接口调用成功";
                result.Data = responseData["expressRoute"]["expressRouteItemList"]["expressRouteItems"].ToString();
                string expressNo = responseData["expressRoute"]["expressNo"].ToString();

                //{"response":{"code":"1","desc":"操作成功","errorInfoList":null,"expressRoute":{"expressName":"圆通速递","expressNo":"807567688076","orderId":"600","wtdOrderId":"16082316551125039","expressRouteItemList":{"expressRouteItems":[{"state":"20100","notes":"已收到订单信息  收到订单信息","optime":"2016-08-23 17:27:45","opercode":""},{"state":"20200","notes":"海外仓待入库  海外仓待入库","optime":"2016-08-23 17:30:47","opercode":""},{"state":"20250","notes":"海外仓库分拣包装完成，等待出库  海外仓待出库","optime":"2016-08-23 17:34:21","opercode":""},{"state":"20400","notes":"订单已出库，发往国内  国际运输","optime":"2016-09-14 17:57:37","opercode":""},{"state":"20600","notes":"海关已审批通过，海关进境清关编码为：51412016I000239039  进境清关","optime":"2016-09-18 13:31:03","opercode":""},{"state":"20700","notes":"  国内派送","optime":"2016-09-20 14:43:38","opercode":""},{"state":"50","notes":"【广东省广州市花都区白云机场公司】 取件人: 李芳 已收件  ","optime":"2016-09-20 15:43:38","opercode":null},{"state":"3036","notes":"【广东省广州市花都区白云机场公司】 已收件  ","optime":"2016-09-20 20:06:34","opercode":null},{"state":"3036","notes":"【广东省广州市花都区白云机场公司】 已打包  ","optime":"2016-09-20 20:07:40","opercode":null},{"state":"3036","notes":"【广东省广州市花都区白云机场公司】 已发出 下一站 【广州转运中心】  ","optime":"2016-09-20 20:08:58","opercode":null},{"state":"3036","notes":"【广州转运中心】 已收入  ","optime":"2016-09-20 22:29:55","opercode":null},{"state":"3036","notes":"【广州转运中心】 已发出 下一站 【新疆乌鲁木齐市公司】  ","optime":"2016-09-20 22:30:04","opercode":null}]}}}}
                
                //List<TrackingData> trackingData = Util.Serialization.JsonUtil.ToObject<List<TrackingData>>(responseData["expressRoute"]["expressRouteItemList"]["expressRouteItems"].ToString());
                //StringBuilder htmlStr = new StringBuilder();
                //foreach (var item in trackingData)
                //{
                //    htmlStr.Append("<tr><td width=\"25%\">" + item.optime + "</td><td width=\"75%\">" + item.notes + "</td></tr>");
                //}

                //result.Data = htmlStr.ToString();
            }
            else
            {
                result.Message= JResult.Message.ToString();
            }

            return result;
        }
    
        /// <summary>
        /// 取消交易订单
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <returns></returns>
        /// <remarks>2016-8-20 杨浩 创建</remarks>
        public override Result CancelOrderTrade(int orderSysNo, string reason = "")
        {
            throw new NotImplementedException();
        }
    }

    #region 实体
    public class WTDOrder
    {
        /// <summary>
        /// String	订单号	是	订单号必须唯一
        /// </summary>
        public string orderId { get; set; }
        /// <summary>
        /// String	订单日期	是
        /// </summary>
        public string orderDate { get; set; }
        /// <summary>
        /// String	收件人姓名	是
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// String	送货省	是
        /// </summary>
        public string province { get; set; }
        /// <summary>
        /// String	送货市	是
        /// </summary>
        public string city { get; set; }
        /// <summary>
        /// String	送货区/县	是
        /// </summary>
        public string area { get; set; }
        /// <summary>
        /// String	送货地址	是
        /// </summary>
        public string addr { get; set; }
        /// <summary>
        /// String	邮政编码	否	请大写C zipCode
        /// </summary>
        public string zipCode { get; set; }
        /// <summary>
        /// String	证件类型	是	默认身份证则填0
        /// </summary>
        public string cardType { get; set; }
        /// <summary>
        /// String	消费者(买家)身份证	是	谁购买的填谁的
        /// </summary>
        public string cardNo { get; set; }
        /// <summary>
        /// String	消费者姓名	是	与身份证一致
        /// </summary>
        public string consumerName { get; set; }
        /// <summary>
        /// String	消费者电话	是
        /// </summary>
        public string consumerPhone { get; set; }
        /// <summary>
        /// String	消费者省	是
        /// </summary>
        public string consumerProvince { get; set; }
        /// <summary>
        /// String	消费者城市	是
        /// </summary>
        public string consumerCity { get; set; }
        /// <summary>
        /// String	消费者地区	是
        /// </summary>
        public string consumerArea { get; set; }
        /// <summary>
        /// String	消费者详细地址	是	街道门牌号
        /// </summary>
        public string consumerAddr { get; set; }
        /// <summary>
        /// String	联系电话1	是
        /// </summary>
        public string phone { get; set; }
        /// <summary>
        /// String	联系电话2	否
        /// </summary>
        public string mobile { get; set; }
        /// <summary>
        /// String	发票抬头	否
        /// </summary>
        public string invoiceHead { get; set; }
        /// <summary>
        /// String	发票类型	否
        /// </summary>
        public string invoiceType { get; set; }
        /// <summary>
        /// String	发票内容	否
        /// </summary>
        public string invoice { get; set; }
        /// <summary>
        /// Double	订单总金额	是	商品总和
        /// </summary>
        public double total { get; set; }
        /// <summary>
        /// Double	运费	是
        /// </summary>
        public double freight { get; set; }
        /// <summary>
        /// Double	保价费用	是
        /// </summary>
        public double insuredFee { get; set; }
        /// <summary>
        /// Double	总税金	是	预收税金
        /// </summary>
        public double tax { get; set; }
        /// <summary>
        /// String	商家	否
        /// </summary>
        public string seller { get; set; }
        /// <summary>
        /// String	备注	否	作为面单发货人自定义字段一般不要填写
        /// </summary>
        public string notes { get; set; }
        /// <summary>
        /// String	口岸编码	是	口岸编码定义
        /// </summary>
        public string portCode { get; set; }
        /// <summary>
        /// String	进出口标志	否	I进口  E出口
        /// </summary>
        public string ieFlag { get; set; }
        /// <summary>
        /// String	所属业务模式	是	业务模式定义
        /// </summary>
        public string busiMode { get; set; }
        /// <summary>
        /// String	捡货类型	否	BC情况下必填 0:散货/包裹,个人直邮 1:大货/备货,有货物在我们仓库
        /// </summary>
        public string pickMode { get; set; }
        /// <summary>
        /// String	发货仓库代号	是	收货仓库,见仓库代码定义
        /// </summary>
        public string whCode { get; set; }
        /// <summary>
        /// String	物流公司编码	是	物流公司编码,见物流公司代码定义
        /// </summary>
        public string expressCode { get; set; }
        /// <summary>
        /// String	运单号	否
        /// </summary>
        public string expressNo { get; set; }
        /// <summary>
        /// String	目的城市	否	编码代号/名称,和expressNo必须同时有值
        /// </summary>
        public string destCity { get; set; }
        /// <summary>
        /// Integer	正品不良品标记	否	0：良品，1：不良品
        /// </summary>
        public int? prodFlag { get; set; }
        /// <summary>
        /// String	发货联系人	是	公司简称
        /// </summary>
        public string senderContact { get; set; }
        /// <summary>
        /// String	发货联系电话	是	例如:4000-948-949
        /// </summary>
        public string senderPhone { get; set; }
        /// <summary>
        /// String	订单销售平台代码	是	平台备案号 （淘宝、京东平台等）
        /// </summary>
        public string platformCode { get; set; }
        /// <summary>
        /// String	订单支付企业代码	否
        /// </summary>
        public string paymentCode { get; set; }
 
        /// <summary>
        /// OrderItemList	订单明细	是
        /// </summary>
        public WTDOrderItemList orderItemList { get; set; }
        //public string extraInfoList	ExtraInfoList	扩展字段数据	否

        #region 新增
        /// <summary>
        /// 是否发总署(0不发,1发送总署)
        /// </summary>
        public int isgac { get; set; }
        /// <summary>
        /// 是否代推订单(0否,1是)
        /// </summary>
        public string isactorder { get; set; }
        /// <summary>
        /// 是否代推运单(0否,1是)
        /// </summary>
        public string isactlogistics { get; set; }
        /// <summary>
        /// 是否代推支付(0否,1是)
        /// </summary>
        public string isactpay { get; set; }
        /// <summary>
        /// 支付企业简称【CFT（财付通、微信）PES（易智付）CMB（招商银行）若有其他企业请与我方联系】
        /// </summary>
        public string paymethod { get; set; }
        /// <summary>
        /// 支付交易编号
        /// </summary>
        public string payid { get; set; }
        /// <summary>
        /// 支付金额
        /// </summary>
        public decimal payaccount { get; set; }
        /// <summary>
        /// 支付时间
        /// </summary>
        public string paytime { get; set; }
        /// <summary>
        /// 非现金抵扣金额
        /// </summary>
        public decimal noncashpartamount { get; set; }
        /// <summary>
        /// 订购人注册号，订购人的交易平台注册号
        /// </summary>
        public string customerregisterno { get; set; }
        #endregion
    }
    public class WTDOrderItemList
    {
        public List<WTDOrderItem> orderItems { get; set; }
    }
    public class WTDOrderItem
    {
        /// <summary>
        /// String	商品编码	是
        /// </summary>
        public string productId { get; set; }
        /// <summary>
        /// String	商品名称	是
        /// </summary>
        public string productName { get; set; }
        /// <summary>
        /// Integer	商品数量	是
        /// </summary>
        public int qty { get; set; }
        /// <summary>
        /// Double	商品单价	是
        /// </summary>
        public double price { get; set; }
        /// <summary>
        /// Double	付款金额	是
        /// </summary>
        public double total { get; set; }
        /// <summary>
        /// String	是否组合商品	否	Y/N
        /// </summary>
        public string isGroup { get; set; }
        /// <summary>
        /// String	组合商品条形码	否	只要上面字段填写则必填
        /// </summary>
        public string groupBarcode { get; set; }
        /// <summary>
        /// Double	行邮税	否	用于计算商品价格显示在清单给买家看
        /// </summary>
        public double? postTax { get; set; }
    }
    #endregion
}
