using Hyt.BLL.ApiSupply.HYH;
using Hyt.BLL.Order;
using Hyt.Model;
using Hyt.Model.Order;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Hyt.BLL.ApiLogistics.HYH
{
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
            get { return Hyt.Model.CommonEnum.物流代码.七号洋行; }
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
            if (jobject.Property("content") != null)
            {
                var response = jobject["content"];
                result.Data = response;
                string code = response["state"].ToString();
                if (code == "true")
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

        /// <summary>
        /// 获取产品详情
        /// </summary>
        /// <param name="productId">商品编码</param>
        /// <returns></returns>
        public override Result GetProduct(string productId)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 添加交易订单
        /// </summary>
        /// <param name="orderId">销售订单系统编号</param>
        /// <returns></returns>
        /// <remarks></remarks>
        ///  <remarks>2016-3-8 杨浩 创建</remarks>
        ///  <remarks>2016-09-22 周 重构</remarks>
        public override Result AddOrderTrade(int orderId)
        {
            var result = new Result()
            {
                Status = false,
                StatusCode = 0,
                Message = "向" + this.Code + "物流推送订单失败"
            };

            #region
            try
            {
                var order = Hyt.BLL.Order.SoOrderBo.Instance.GetEntity(orderId);
                if (order == null)
                {
                    result.Status = false;
                    result.Message = string.Format("订单号{0}不存在！", orderId);
                    result.StatusCode = -100;
                }

                order.ReceiveAddress = BLL.Order.SoOrderBo.Instance.GetOrderReceiveAddress(order.ReceiveAddressSysNo);
                order.OrderItemList = DataAccess.Order.ISoOrderItemDao.Instance.GetOrderItemsByOrderSysNo(order.SysNo);
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

                var newOrder = new CreatedOrderApiRequest();
                newOrder.OutOrderNo = order.SysNo.ToString();
                newOrder.DeliveryCode = "3";//暂时默认1（后期此参数作废）
                newOrder.ReceiverName = TConvert.ToString(order.ReceiveAddress.Name);
                newOrder.ReceiverMobile = !string.IsNullOrWhiteSpace(order.ReceiveAddress.MobilePhoneNumber) ? order.ReceiveAddress.MobilePhoneNumber : order.ReceiveAddress.PhoneNumber;
                newOrder.ReceiverProvince = TConvert.ToString(receiverProvince.AreaName).Trim();
                newOrder.ReceiverCity = TConvert.ToString(receiverCity.AreaName).Trim();
                newOrder.ReceiverDistrict = TConvert.ToString(receiverDistrict.AreaName).Trim();
                newOrder.ReceiverAddress = TConvert.ToString(order.ReceiveAddress.StreetAddress).Trim();
                newOrder.ReceiverIdCard = TConvert.ToString(order.ReceiveAddress.IDCardNo);
                newOrder.ReceiverFrontIdCardImageUrl = HttpContext.Current.Request.Url.Host + TConvert.ToString(order.ReceiveAddress.IDCardImgs);
                newOrder.ReceiverOppositeIdCardImageUrl = HttpContext.Current.Request.Url.Host + TConvert.ToString(order.ReceiveAddress.IDCardImgs);
                newOrder.UserRemark = order.Remarks;

                newOrder.ThirdPlatformPaymentName = Enum.GetName(typeof(Hyt.Model.CommonEnum.PayCode),order.PayTypeSysNo);
                var payment = Hyt.BLL.Finance.FinanceBo.Instance.GetOnlinePaymentBySourceSysNo(orderId);
                string PaymentNumber = "";
                if (payment != null)
                    PaymentNumber = payment.VoucherNo;
                newOrder.ThirdPlatformPaymentNumber = PaymentNumber;

                newOrder.Items = new List<CreatedOrderItemRequest>();
                CreatedOrderItemRequest orderItem = new CreatedOrderItemRequest();

                foreach (var item in order.OrderItemList)
                {
                    var product = BLL.Product.PdProductBo.Instance.GetProductNoCache(item.ProductSysNo);
                    orderItem.SkuId = product.ErpCode;
                    orderItem.Quantity = item.Quantity;
                    newOrder.Items.Add(orderItem);
                }
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(newOrder);
                //推送订单
                var _result = CreatedOrder(newOrder);
                if(_result.ResultCode==200)
                {
                    var soOrderSyncLogisticsLog = new SoOrderSyncLogisticsLog();

                    soOrderSyncLogisticsLog.OrderSysNo = orderId;
                    soOrderSyncLogisticsLog.Code = (int)this.Code;

                    soOrderSyncLogisticsLog.LastUpdateDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    soOrderSyncLogisticsLog.StatusCode = "";
                    soOrderSyncLogisticsLog.StatusMsg = "";
                    soOrderSyncLogisticsLog.Packets = json;
                    soOrderSyncLogisticsLog.ReceiptContent = _result.Tag.ToString();

                    soOrderSyncLogisticsLog.LastUpdateBy = 0;
                    soOrderSyncLogisticsLog.LogisticsOrderId = "";

                    soOrderSyncLogisticsLog.CreateDate = DateTime.Now;
                    soOrderSyncLogisticsLog.CreatedBy = BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo;
                    SoOrderSyncLogisticsLogBo.Instance.Insert(soOrderSyncLogisticsLog);
                    BLL.Order.SoOrderBo.Instance.UpdateOrderApiStatus(1, 3, orderId);
                    result.Message = "向" + this.Code + "物流推送订单成功：回执单号：" + _result.Tag.ToString();
                }
                else
                {
                    result.Message = _result.Message;
                }
            }
            catch (Exception ex)
            {
                result.Message = "向" + this.Code + "物流推送订单报错：" + ex.StackTrace;
                return result;
            }
            #endregion

            return result;
        }
        /// <summary>
        /// 推送订单
        /// </summary>
        /// <param name="request">订单参数</param>
        /// <returns></returns>
        ///  <remarks>2016-09-22 周 创建</remarks>
        private ActionResponse CreatedOrder(CreatedOrderApiRequest request)
        {
            WebUtils.HttpPacket packet = WebUtils.DoPost(ApiConfig.ApiUrl + "Order/CreatedOrder?" + CommonUtils.GetUrlParameter(), JsonConvert.SerializeObject(request), "application/json");
            try
            {
                ApiResponse apiResponse = JsonConvert.DeserializeObject<ApiResponse>(packet.ResponseBody);
                if (apiResponse.State)
                {
                    return ActionResponse.CreateSuccessResponse(apiResponse.Content);
                }
                return ActionResponse.CreateFailResponse(apiResponse.Content.ToString());
            }
            catch (Exception)
            {
                return ActionResponse.CreateFailResponse(packet.ResponseBody);
            }
        }
        /// <summary>
        /// 查询订单运单号信息
        /// </summary>
        /// <param name="orderId">订单号(多个用逗号隔开,每次最大50个订单)</param>
        /// <returns></returns>
        /// <remarks>2016-3-22 杨浩 创建</remarks>
        public override Result GetOrderExpressno(string orderId)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 查询订单信息
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <returns></returns>
        /// <remarks>2016-3-22 杨浩 创建</remarks>
        ///  <remarks>2016-09-22 周 重构</remarks>
        public override Result GetOrderTrade(string orderId)
        {
            var result = new Result() { Status = false };
            WebUtils.HttpPacket packet = WebUtils.DoGet(ApiConfig.ApiUrl + "Order/SearchOrderPackage?" + CommonUtils.GetUrlParameter(new SortedDictionary<string, string>() { { "orderNo", orderId } }));
            //PackageResponse response = null;
            try
            {
                ApiResponse apiResponse = JsonConvert.DeserializeObject<ApiResponse>(packet.ResponseBody);
                result.Status = true;
                result.Message = apiResponse.Content.ToString();
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 查询订单包裹信息
        /// </summary>
        /// <param name="orderNo">订单号</param>
        /// <returns></returns>
        /// <remarks>2016-09-22 周 创建</remarks>
        private PackageResponse SearchOrderPackage(string orderNo)
        {
            WebUtils.HttpPacket packet = WebUtils.DoGet(ApiConfig.ApiUrl + "Order/SearchOrderPackage?" + CommonUtils.GetUrlParameter(new SortedDictionary<string, string>() { { "orderNo", orderNo } }));
            PackageResponse response = null;
            try
            {
                ApiResponse apiResponse = JsonConvert.DeserializeObject<ApiResponse>(packet.ResponseBody);
                if (apiResponse.State)
                {
                    response = JsonConvert.DeserializeObject<PackageResponse>(apiResponse.Content.ToString());
                }
            }
            catch (Exception)
            {
                // ignored
            }
            return response;
        }

        /// <summary>
        /// 查询订单物流信息
        /// </summary>
        /// <param name="orderSysNo">订单号</param>
        /// <returns></returns>
        /// <remarks>2016-04-18 陈海裕 创建</remarks>
        /// <remarks>2016-09-22 周 重构</remarks>
        public override Result<string> GetLogisticsTracking(int orderSysNo)
        {
            //throw new NotImplementedException();
            var result = new Result<string>();
            result.Status = false;
            result.StatusCode = 0;
            result.Message = "接口调用失败";


            var orderSyncLogisticsLogInfo = BLL.Order.SoOrderSyncLogisticsLogBo.Instance.GetModel(orderSysNo, (int)this.Code);

            if (orderSyncLogisticsLogInfo == null)
            {
                result.Message = "没有找到物流订单号!";
                return result;
            }
            string ExpNum = "", DeliveryName="";
            int DeliveryTypeSysNo = 0;
            var _result = SearchOrderPackage(orderSyncLogisticsLogInfo.ReceiptContent);
            if (!string.IsNullOrEmpty(_result.OrderNo) && _result.Item.Count > 0)
            {
                foreach(var item in _result.Item)
                {
                    switch(item.OrderStatus)
                    {
                        case 1:
                            result.Message = "订单已下单，但未付款";
                            break;
                        case 2:
                            result.Message = "订单待发货";
                            break;
                        case 3:
                            if (item.DeliveryCode == "1")
                            {
                                DeliveryName = "中国邮政";
                                DeliveryTypeSysNo = 10;//邮政EMS
                            }
                            else if (item.DeliveryCode == "2")
                            {
                                DeliveryName = "韵达快递";
                                DeliveryTypeSysNo = 15;//韵达
                            }
                            else
                            {
                                DeliveryName = "中通快递";
                                DeliveryTypeSysNo = 13;//中通
                            }
                            ExpNum = item.DeliveryNumber;
                            result.Message = "订单已发货，发货快递为" + DeliveryName + ",快递单号为：" + ExpNum;

                            var order = Hyt.BLL.Order.SoOrderBo.Instance.GetEntity(orderSysNo);
                            if(order!=null)
                            {
                                if (order.Status > 20)
                                {
                                    //result.Message = ExpNum;
                                    Hyt.Model.Common.LgExpressModel lgExpress = BLL.Order.SoOrderBo.Instance.GetDeliveryCodeData(orderSysNo);
                                    ThirdDeliveryConfig config = Hyt.BLL.Logistics.ThirdDeliveryConfigBo.Instance.GetThirdDeliveryConfigBySysNo(1);
                                    string apiurl = config.tdc_ThirdUrl.Replace("{ApiId}", config.tdc_ApiID).Replace("{ApiKey}", config.tdc_Sceret).Replace("{TypeCom}", lgExpress.OverseaCarrier)
                                        .Replace("{ExpNo}", lgExpress.ExpressNo);
                                    System.Net.WebRequest request = System.Net.WebRequest.Create(@apiurl);
                                    System.Net.WebResponse response = request.GetResponse();
                                    System.IO.Stream stream = response.GetResponseStream();
                                    System.Text.Encoding encode = System.Text.Encoding.Default;
                                    System.IO.StreamReader reader = new System.IO.StreamReader(stream, encode);

                                    result.Message = reader.ReadToEnd();
                                }
                                else
                                {
                                    if (DeliveryTypeSysNo != order.DeliveryTypeSysNo)
                                        BLL.Order.SoOrderBo.Instance.UpdateOrderDeliveryType(orderSysNo, DeliveryTypeSysNo);

                                    var res = BLL.Order.SoOrderBo.Instance.Ship(orderSysNo, ExpNum);//一建发货
                                    if (res.Status == false)
                                        result.Message = res.Message;
                                    else
                                        BLL.Order.SoOrderBo.Instance.UpdateOrderApiStatus((int)Hyt.Model.WorkflowStatus.OrderStatus.跨境物流推送状态.成功, 3, orderSysNo);
                                }
                            }
                           
                            break;
                        case 4:
                            if (item.DeliveryCode == "1")
                            {
                                DeliveryName = "中国邮政";
                                DeliveryTypeSysNo = 10;//邮政EMS
                            }
                            else if (item.DeliveryCode == "2")
                            {
                                DeliveryName = "韵达快递";
                                DeliveryTypeSysNo = 15;//韵达
                            }
                            else
                            {
                                DeliveryName = "中通快递";
                                DeliveryTypeSysNo = 13;//中通
                            }
                            ExpNum = item.DeliveryNumber;
                            result.Message = "订单（" + _result.OutOrderNo + "）已完成，发货快递为：" + DeliveryName + ",快递单号为：" + ExpNum;
                            break;
                        default:
                            result.Message = "订单（" + _result.OutOrderNo + "）已取消";
                            break;
                    }
                }
            }
            else
            {
                result.Message = "获取订单信息失败";
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

        #region 实体
        public class CreatedOrderApiRequest
        {
            /// <summary>
            /// 外部订单编号(必填)
            /// </summary>
            public string OutOrderNo { get; set; }

            public List<CreatedOrderItemRequest> Items { get; set; }

            /// <summary>
            /// 快递编码
            /// </summary>
            public string DeliveryCode { get; set; }

            /// <summary>
            /// 用户备注信息
            /// </summary>
            public string UserRemark { get; set; }

            #region 收货人信息
            public string ReceiverName { get; set; }
            public string ReceiverIdCard { get; set; }
            public string ReceiverProvince { get; set; }
            public string ReceiverCity { get; set; }
            public string ReceiverDistrict { get; set; }
            public string ReceiverAddress { get; set; }
            public string ReceiverMobile { get; set; }

            public string ReceiverFrontIdCardImageUrl { get; set; }
            public string ReceiverOppositeIdCardImageUrl { get; set; }

            #endregion

            /// <summary>
            /// 第三方销售平台支付方式
            /// </summary>
            public string ThirdPlatformPaymentName { get; set; }
            /// <summary>
            /// 第三方销售平台支付交易号
            /// </summary>
            public string ThirdPlatformPaymentNumber { get; set; }
        }

        public class CreatedOrderItemRequest
        {
            public string SkuId { get; set; }
            public int Quantity { get; set; }
        }


        public class DeliveryResponse
        {
            /// <summary>
            /// 快递ID
            /// </summary>
            public int DeliveryId { get; set; }
            /// <summary>
            /// 快递名称
            /// </summary>
            public string DeliveryName { get; set; }
            /// <summary>
            /// 快递自编码
            /// </summary>
            public string DeliveryCode { get; set; }
        }


        public class PackageResponse
        {
            public PackageResponse()
            {
                Item = new List<PackageItemResponse>();
            }
            public string OrderNo { get; set; }

            public string OutOrderNo { get; set; }

            public List<PackageItemResponse> Item { get; set; }

        }

        public class PackageItemResponse
        {
            public string OrderNo { get; set; }

            public int OrderStatus { get; set; }

            public string DeliveryCode { get; set; }

            public string DeliveryNumber { get; set; }

            public List<PackageItemProduct> OrderProductList { get; set; }
        }

        public class PackageItemProduct
        {
            public string SkuId { get; set; }

            public int Quantity { get; set; }
        }
        #endregion

    }
}
