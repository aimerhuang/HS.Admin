using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.Logis.XinYi;
using Hyt.Model.Manual;
using Hyt.Model.WorkflowStatus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Hyt.BLL.Finance;
using Hyt.BLL.Order;
using Hyt.Util.Serialization;
using Hyt.Util;
using Newtonsoft.Json.Linq;
using Hyt.Model.Parameter;
using System.Security.Cryptography;
using System.IO;
using System.Net;
using System.Xml;
using Hyt.Model.Order;

namespace Hyt.BLL.ApiLogistics.QiBang
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
        /// api服务url(http://wmstest02.keypon.cn:8877/exgw/wms)
        /// </summary>
        private string apiUrl = "http://exgw01.keypon.cn:51234/exgw/wms";//"http://exgw01.keypon.cn:51234/";
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
            get { return Hyt.Model.CommonEnum.物流代码.启邦国际物流; }
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
        ///  <remarks>2016-12-13 周 重构</remarks>
        public override Result AddOrderTrade(int orderId)
        {
            var result = new Result()
            {
                Status = false,
                StatusCode = 0,
                Message = "向" + this.Code + "物流推送订单失败"
            };

            #region begiin

            try
            {
                #region 获取本地推送的订单详情相关资料信息

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

                #endregion

                #region 推送相关参数处理

                var newOrder = new CreatedOrderApiRequest();

                var order_item = "";
                decimal TotleGrosWeight = 0M;//获取订单商品总毛重
                foreach (var item in SoOrderBo.Instance.GetOrderItemsByOrderId(orderId))
                {
                    var product = Hyt.BLL.Product.PdProductBo.Instance.GetProduct(item.ProductSysNo);
                    //获取启邦商品备案信息
                    var IcpQiBang = Hyt.BLL.ApiIcq.IcpBo.Instance.GetIcpQiBangGoodsInfoEntityByPid(item.ProductSysNo);

                    TotleGrosWeight += item.Quantity * product.GrosWeight;
                    PdProductStock productStock = BLL.Warehouse.PdProductStockBo.Instance.GetEntityByWP(order.DefaultWarehouseSysNo, item.ProductSysNo);
                    string CustomsNo = "", 
                           SkuNo = "", 
                           ProductSysNo = item.ProductSysNo.ToString(),
                           ProductName = product.ProductName, 
                           ErpCode = product.ErpCode, 
                           Spec = "", 
                           SalesUnitPrice = (item.SalesUnitPrice*100).ToString(),
                           cus_code = "DQSW";
                    
                    if(IcpQiBang!=null)
                    {
                        if (!string.IsNullOrEmpty(IcpQiBang.v_goods_regist_no))
                            CustomsNo = IcpQiBang.v_goods_regist_no;//备案客户SKU
                        if (!string.IsNullOrEmpty(IcpQiBang.item_id))
                            ProductSysNo = IcpQiBang.item_id;//备案商品编码
                        if (!string.IsNullOrEmpty(IcpQiBang.item_name))
                            ProductName = IcpQiBang.item_name;//备案商品名称
                        if (!string.IsNullOrEmpty(IcpQiBang.item_code))
                            ErpCode = IcpQiBang.item_code;//备案商品货号
                        if (!string.IsNullOrEmpty(IcpQiBang.cus_code))
                            cus_code = IcpQiBang.cus_code;//备案客户代码
                        if (!string.IsNullOrEmpty(IcpQiBang.goods_sku))
                            SkuNo = IcpQiBang.goods_sku;//备案商品SKU
                        if (!string.IsNullOrEmpty(IcpQiBang.item_spec))
                            Spec = IcpQiBang.item_spec;//备案规格
                        //if (!string.IsNullOrEmpty(IcpQiBang.item_price.ToString()))
                            //SalesUnitPrice = (IcpQiBang.item_price*100).ToString();//备案价格
                        if (!string.IsNullOrEmpty(IcpQiBang.n_kos.ToString()))
                            TotleGrosWeight += item.Quantity * IcpQiBang.n_kos;//备案毛重
                    }
                    else
                    {
                        if (productStock != null)
                        {
                            CustomsNo = productStock.CustomsNo;
                            SkuNo = productStock.ProductSku;
                        }
                    }

                    //子菜单对象，设置参数
                    PayData OrdeItems = new PayData();
                    OrdeItems.SetValue("order_item_id", item.ProductSysNo.ToString());//订单商品 ID，订单商品 ID，需要对推送过来的商品进行排序，填写自然数(Long(20))
                    OrdeItems.SetValue("v_goods_regist_no", CustomsNo);//商品备案号
                    OrdeItems.SetValue("item_id", ProductSysNo);//商品编码(Long(20))
                    OrdeItems.SetValue("item_name", ProductName);//商品名称
                    OrdeItems.SetValue("item_code", ErpCode);//商品货号
                    OrdeItems.SetValue("inventory_type", "1");//库存类型 1 可销售库存
                    OrdeItems.SetValue("item_quantity", item.Quantity.ToString());//商品数量
                    OrdeItems.SetValue("item_price", SalesUnitPrice);//销售价格
                    OrdeItems.SetValue("item_version", "1");//商品版本，ZY允许货主修改商品属性描述，每修改一次就对应一个商品版本信息,默认传1
                    OrdeItems.SetValue("attributes", "");//扩展字段（特殊业务才会下发这个信息，一般此字段是空值）
                    OrdeItems.SetValue("cus_code", cus_code);//商家代码
                    OrdeItems.SetValue("sku_code", SkuNo);//Sku代码
                    OrdeItems.SetValue("item_spec", Spec);//规格

                    order_item += OrdeItems.ToOrderItem();
                }

                //创建签名对象，设置参数
                PayData SignDate = new PayData();
                SignDate.SetValue("store_code", "WPH-00001");//仓储编码
                SignDate.SetValue("order_code", onlinePayment.BusinessOrderSysNo);//订单编号
                SignDate.SetValue("order_type", "201");//订单类型（销售单进口BBC）
                SignDate.SetValue("order_source", "gaopin999");//订单来源双方约定
                SignDate.SetValue("order_create_time", order.CreateDate.ToString("yyyy-MM-dd HH:mm:ss"));//订单创建时间
                SignDate.SetValue("v_ieflag", "I");//进出口标识，I:进口；E:出口
                SignDate.SetValue("v_transport_code", TConvert.ToString(order.ReceiveAddress.IDCardNo));//消费者证件号码
                SignDate.SetValue("v_card_type", "01");//01:身份证、02:护照、03:其他
                SignDate.SetValue("v_transport_code", "");//运输方式：1空运 2海运 3汽运 4铁运
                SignDate.SetValue("v_package_typecode", "");//包装方式：1木箱、2纸箱、3桶装、4散装、5托盘、6包、7油罐车等
                SignDate.SetValue("v_qy_state", "");//进口为起运国(地区)代码，出口为运抵国(地区)代码
                SignDate.SetValue("v_master_good_name", "");//主要商品名称
                SignDate.SetValue("n_kos", TotleGrosWeight.ToString());//毛重
                SignDate.SetValue("v_traf_name", "");//运输工具名称
                SignDate.SetValue("tms_service_code", "");//物流公司编码
                SignDate.SetValue("tms_order_code", "");//运单号,退货单有可能有运单号
                SignDate.SetValue("prev_order_code", order.OrderNo);//原订单编码
                SignDate.SetValue("totalmailno", "");//总运单号
                SignDate.SetValue("receiver_info", order.ReceiveAddress.ZipCode + "^^^" + receiverProvince.AreaName + "^^^" + receiverCity.AreaName + "^^^" + receiverDistrict.AreaName + "^^^" + order.ReceiveAddress.StreetAddress + "^^^" + order.ReceiveAddress.Name + "^^^" + ((order.ReceiveAddress.MobilePhoneNumber == "" || order.ReceiveAddress.MobilePhoneNumber==null)?"NA":order.ReceiveAddress.MobilePhoneNumber) + "^^^" + ((order.ReceiveAddress.PhoneNumber == "" || order.ReceiveAddress.PhoneNumber == null) ? "NA" : order.ReceiveAddress.PhoneNumber));//收货方信息:邮编^^^省^^^市^^^区^^^具体地址^^^收件方名称^^^手机^^^电话
                SignDate.SetValue("sender_info", "511400^^^广东省^^^广州市^^^南沙区^^^港荣三街3号402^^^广州市启邦国际物流有限公司^^^NA^^^0755-33079718");//发货方信息:邮编^^^省^^^市^^^区^^^具体地址^^^发货方名称^^^手机^^^电话
                SignDate.SetValue("order_item_list", order_item);//订单详情
                SignDate.SetValue("package_count", "1");//包裹数量
                SignDate.SetValue("delivery_method", "");//发货方式
                SignDate.SetValue("remark", "购物");//备注
                SignDate.SetValue("order_ename", order.ReceiveAddress.Name);//订单人姓名
                SignDate.SetValue("order_phone", !string.IsNullOrWhiteSpace(order.ReceiveAddress.MobilePhoneNumber) ? order.ReceiveAddress.MobilePhoneNumber : order.ReceiveAddress.PhoneNumber);//订单人电话
                SignDate.SetValue("order_cardno", order.ReceiveAddress.IDCardNo);//订单人身份证号码
                SignDate.SetValue("freight", order.FreightAmount.ToString());//运费
                SignDate.SetValue("Insurance_fee", "0");//保价费
                SignDate.SetValue("tax", order.TaxFee.ToString());//税费

                //MD5加密
                //待加密字符串
                var SignStr = SignDate.ToXml();

                var key = "774s3dc97s032de";
                var charset = "UTF-8";

                //获取签名
                var sign = EncodeBase64(Sign(SignStr, key, charset));
                var strPost = "sign_type=MD5&notify_type=COSCO_STOCK_OUT_ORDER&input_charset=UTF-8&sign=" + sign + "&content=<?xml version=\"1.0\" encoding=\"utf-8\"?>" + SignStr;

                var strResult = GetResponse(apiUrl, strPost);
                StringReader Reader = new StringReader(strResult);
                XmlDocument TempXml = new XmlDocument();
                TempXml.Load(Reader);
                string success = get_Xml_Nodes(TempXml, "//success");
                if (success == "false")
                {
                    string ApiResultMessage = get_Xml_Nodes(TempXml, "//errorMsg");
                    string errorCode = get_Xml_Nodes(TempXml, "//errorCode");

                    result.Status = false;
                    result.Message = ApiResultMessage;
                }
                else
                {
                    var soOrderSyncLogisticsLog = new SoOrderSyncLogisticsLog();

                    soOrderSyncLogisticsLog.OrderSysNo = orderId;
                    soOrderSyncLogisticsLog.Code = (int)this.Code;

                    soOrderSyncLogisticsLog.LastUpdateDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    soOrderSyncLogisticsLog.StatusCode = "";
                    soOrderSyncLogisticsLog.StatusMsg = "";
                    soOrderSyncLogisticsLog.Packets = strPost;
                    soOrderSyncLogisticsLog.ReceiptContent = strResult.ToString();

                    soOrderSyncLogisticsLog.LastUpdateBy = 0;
                    soOrderSyncLogisticsLog.LogisticsOrderId = "";

                    soOrderSyncLogisticsLog.CreateDate = DateTime.Now;
                    soOrderSyncLogisticsLog.CreatedBy = BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo;
                    SoOrderSyncLogisticsLogBo.Instance.Insert(soOrderSyncLogisticsLog);
                    BLL.Order.SoOrderBo.Instance.UpdateOrderApiStatus(1, 3, orderId);

                    result.Status = true;
                    result.Message = "订单物流推送成功！";
                }

                #endregion


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
        /// 查询订单运单号信息
        /// </summary>
        /// <param name="orderId">订单号(多个用逗号隔开,每次最大50个订单)</param>
        /// <returns></returns>
        /// <remarks>2016-12-13 周 重构</remarks>
        public override Result GetOrderExpressno(string orderId)
        {
            var result = new Result()
           {
               Status = false,
               StatusCode = 0,
               Message = "运单查询失败"
           };
            CrossBorderLogisticsOrder mod = Hyt.BLL.CrossBorderLogistics.CrossBorderLogisticsOrderBo.Instance.GetEntityByOrderSysNo(Convert.ToInt32(orderId));

            if (mod != null)
            {
                result.Status = true;
                result.Message = mod.ExpressNo;
            }
            else
            {
                #region begiin

                try
                {
                    var order = Hyt.BLL.Order.SoOrderBo.Instance.GetEntity(Convert.ToInt32(orderId));
                    if (order == null)
                    {
                        result.Status = false;
                        result.Message = string.Format("订单号{0}不存在！", orderId);
                        result.StatusCode = -100;
                    }
                    var filter = new ParaOnlinePaymentFilter();
                    filter.OrderSysNo =int.Parse(orderId);
                    var onlinePayment = BLL.Finance.FinanceBo.Instance.GetOnlinePayments(filter).Rows.Where(o => o.Status == 1).FirstOrDefault();

                    if (onlinePayment == null)
                    {
                        result.Message = "订单不存在在线支付记录";
                        return result;
                    }

                    PayData Items = new PayData();
                    Items.SetValue("order_code", onlinePayment.BusinessOrderSysNo);//仓库订单编码
                    Items.SetValue("ordermark", "BBC");//1,BBC 2,BC
                    Items.SetValue("order_type", "001");//订单子类型：001,一般进口；B002,BC备货模式；B003,BC转运模式；B004,BC直邮模式；
                    Items.SetValue("trading_orderno", onlinePayment.BusinessOrderSysNo);//交易订单号
                    Items.SetValue("Order_Source", "gaopin999");//平台编号(启邦与电商双方定义的)

                    var SignStr = Items.ToXml();

                    //var postUrl = "http://wmstest02.keypon.cn:8877/exgw/wms";
                    var key = "774s3dc97s032de";
                    var charset = "UTF-8";

                    //获取签名
                    var sign = EncodeBase64(Sign(SignStr, key, charset));
                    var strPost = "sign_type=MD5&notify_type=QB_QUERY_WAYBILLNO&input_charset=UTF-8&sign=" + sign + "&content=<?xml version=\"1.0\" encoding=\"utf-8\"?>" + SignStr;
                    var strResult = GetResponse(apiUrl, strPost);
                    BLL.Log.LocalLogBo.Instance.Write("提交的数据：" + strPost + "  ------返回数据：" + strResult, "QiBangLogisticsNo");  

                    StringReader Reader = new StringReader(strResult);
                    XmlDocument TempXml = new XmlDocument();
                    TempXml.Load(Reader);
                    string success = get_Xml_Nodes(TempXml, "//success");
                    if (success == "false")
                    {
                        string ApiResultMessage = get_Xml_Nodes(TempXml, "//errorMsg");
                        string errorCode = get_Xml_Nodes(TempXml, "//errorCode");

                        result.Status = false;
                        result.Message = ApiResultMessage;
                    }
                    else
                    {
                        result.Status = true;
                        result.Message = success;

                        if (!string.IsNullOrEmpty(get_Xml_Nodes(TempXml, "//orders")))
                        {
                            XmlNodeList ApiResultApi_SelectOrderInfo = TempXml.SelectNodes("//order");
                            string message = "";
                            if (ApiResultApi_SelectOrderInfo[0].InnerXml != "")
                            {
                                for (int i = 0; i < ApiResultApi_SelectOrderInfo.Count; i++)
                                {
                                    XmlDocument OrderXml = new XmlDocument();
                                    OrderXml.LoadXml("<order>" + ApiResultApi_SelectOrderInfo[i].InnerXml + "</order>");
                                    string order_code = get_Xml_Nodes(OrderXml, "//order_code");//订单编号
                                    string waybill_code = get_Xml_Nodes(OrderXml, "//waybill_code");//运单编号
                                    message += "订单：" + order_code + " 运单编号为：" + waybill_code + ";";

                                    Hyt.BLL.CrossBorderLogistics.CrossBorderLogisticsOrderBo.Instance.InsertEntity(
                                        new CrossBorderLogisticsOrder()
                                        {
                                            ExpressNo = waybill_code,
                                            LogisticsCode = 0,
                                            LogisticsOrderId = order_code,
                                            SoOrderSysNo = Convert.ToInt32(orderId),
                                        }
                                    );
                                }
                                result.Message = message;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    result.Message = "向" + this.Code + "查询运单号失败：" + ex.StackTrace;
                    return result;
                }
                #endregion
            }

            return result;
        }
        /// <summary>
        /// 查询订单信息
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <returns></returns>
        ///  <remarks>2016-09-22 周 重构</remarks>
        public override Result GetOrderTrade(string orderId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 查询订单物流信息
        /// </summary>
        /// <param name="orderSysNo">订单号</param>
        /// <returns></returns>
        /// <remarks>2016-04-18 陈海裕 创建</remarks>
        /// <remarks>2016-09-22 周 重构</remarks>
        public override Result<string> GetLogisticsTracking(int orderId)
        {
            var result = new Result<string>()
            {
                Status = false,
                StatusCode = 0,
                Message = "接口调用失败"
            };
            CrossBorderLogisticsOrder mod = Hyt.BLL.CrossBorderLogistics.CrossBorderLogisticsOrderBo.Instance.GetEntityByOrderSysNo(Convert.ToInt32(orderId));

            if (mod != null)
            {
                result.Status = true;
                using (var service = new Pisen.Framework.Service.Proxy.ServiceProxy<Grand.Platform.Express.Contract.IExpressService>())
                {
                    try
                    {
                        var response = service.Channel.GetExpressDeliveryLocusByOrderSysNo(orderId);
                        if (response.IsError)
                        {
                            result.Status = !response.IsError;
                            result.Message = response.ErrMsg;
                        }
                        else
                        {
                            result.Status = !response.IsError;
                           
                            var json = JObject.Parse(response.Data);
                            var jsonobj = json["data"].ToString();
                            result.Message = json["nu"].ToString();
                            result.StatusCode = 20;
                            result.Data = jsonobj;
                        }
                    }
                    catch (Exception ex)
                    {
                        result.Message = ex.Message;
                        BLL.Log.SysLog.Instance.Error(Hyt.Model.WorkflowStatus.LogStatus.系统日志来源.后台, "快递查询", ex);
                    }
                }
                return result;
            }
            else
            {
                #region begiin

                try
                {
                    var order = Hyt.BLL.Order.SoOrderBo.Instance.GetEntity(Convert.ToInt32(orderId));
                    if (order == null)
                    {
                        result.Status = false;
                        result.Message = string.Format("订单号{0}不存在！", orderId);
                        result.StatusCode = -100;
                    }
                    var filter = new ParaOnlinePaymentFilter();
                    filter.OrderSysNo = orderId;
                    var onlinePayment = BLL.Finance.FinanceBo.Instance.GetOnlinePayments(filter).Rows.Where(o => o.Status == 1).FirstOrDefault();

                    if (onlinePayment == null)
                    {
                        result.Message = "订单不存在在线支付记录";
                        return result;
                    }

                    PayData Items = new PayData();
                    Items.SetValue("order_code", onlinePayment.BusinessOrderSysNo);//仓库订单编码
                    Items.SetValue("ordermark", "BBC");//1,BBC 2,BC
                    Items.SetValue("order_type", "001");//订单子类型：001,一般进口；B002,BC备货模式；B003,BC转运模式；B004,BC直邮模式；
                    Items.SetValue("trading_orderno", onlinePayment.BusinessOrderSysNo);//交易订单号
                    Items.SetValue("Order_Source", "gaopin999");//平台编号(启邦与电商双方定义的)

                    var SignStr = Items.ToXml();

                    //var postUrl = "http://wmstest02.keypon.cn:8877/exgw/wms";
                    var key = "774s3dc97s032de";
                    var charset = "UTF-8";

                    //获取签名
                    var sign = EncodeBase64(Sign(SignStr, key, charset));
                    var strPost = "sign_type=MD5&notify_type=QB_QUERY_WAYBILLNO&input_charset=UTF-8&sign=" + sign + "&content=<?xml version=\"1.0\" encoding=\"utf-8\"?>" + SignStr;
                    var strResult = GetResponse(apiUrl, strPost);
                    BLL.Log.LocalLogBo.Instance.Write("提交的数据：" + strPost + "  ------返回数据：" + strResult, "QiBangLogisticsNo");

                    StringReader Reader = new StringReader(strResult);
                    XmlDocument TempXml = new XmlDocument();
                    TempXml.Load(Reader);
                    string success = get_Xml_Nodes(TempXml, "//success");
                    if (success == "false")
                    {
                        string ApiResultMessage = get_Xml_Nodes(TempXml, "//errorMsg");
                        string errorCode = get_Xml_Nodes(TempXml, "//errorCode");

                        result.Status = false;
                        result.Message = ApiResultMessage;
                    }
                    else
                    {
                        result.Status = true;
                        result.Message = success;

                        if (!string.IsNullOrEmpty(get_Xml_Nodes(TempXml, "//orders")))
                        {
                            XmlNodeList ApiResultApi_SelectOrderInfo = TempXml.SelectNodes("//order");
                            string message = "";
                            if (ApiResultApi_SelectOrderInfo[0].InnerXml != "")
                            {
                                for (int i = 0; i < ApiResultApi_SelectOrderInfo.Count; i++)
                                {
                                    XmlDocument OrderXml = new XmlDocument();
                                    OrderXml.LoadXml("<order>" + ApiResultApi_SelectOrderInfo[i].InnerXml + "</order>");
                                    string order_code = get_Xml_Nodes(OrderXml, "//order_code");//订单编号
                                    string waybill_code = get_Xml_Nodes(OrderXml, "//waybill_code");//运单编号
                                    message += "订单：" + order_code + " 运单编号为：" + waybill_code + ";";

                                    Hyt.BLL.CrossBorderLogistics.CrossBorderLogisticsOrderBo.Instance.InsertEntity(
                                        new CrossBorderLogisticsOrder()
                                        {
                                            ExpressNo = waybill_code,
                                            LogisticsCode = 0,
                                            LogisticsOrderId = order_code,
                                            SoOrderSysNo = Convert.ToInt32(orderId),
                                        }
                                    );
                                    using (var service = new Pisen.Framework.Service.Proxy.ServiceProxy<Grand.Platform.Express.Contract.IExpressService>())
                                    {
                                        try
                                        {
                                            var response = service.Channel.GetExpressDeliveryLocusByOrderSysNo(orderId);
                                            if (response.IsError)
                                            {
                                                result.Status = !response.IsError;
                                                result.Message = response.ErrMsg;
                                            }
                                            else
                                            {
                                                result.Status = !response.IsError;
                                                result.Message = response.Data;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            result.Message = ex.Message;
                                            BLL.Log.SysLog.Instance.Error(Hyt.Model.WorkflowStatus.LogStatus.系统日志来源.后台, "快递查询", ex);
                                        }
                                    }
                                    return result;
                                }
                                result.Message = message;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    result.Message = "向" + this.Code + "查询运单号失败：" + ex.StackTrace;
                    return result;
                }
                #endregion
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
            #region 协议参数
            //协议参数
            /// <summary>
            /// 签名类型，取值：MD5、RSA，默认固定填：MD5
            /// </summary>
            public string sign_type { get; set; }
            /// <summary>
            /// COSCO_STOCK_OUT_ORDER
            /// </summary>
            public string notify_type { get; set; }
            /// <summary>
            /// 字符编码,取值：GBK、UTF-8，默认：UTF-8
            /// </summary>
            public string input_charset { get; set; }
            /// <summary>
            /// 签名
            /// </summary>
            public string sign { get; set; }
            /// <summary>
            /// Xml报文内容
            /// </summary>
            public string content { get; set; }
            #endregion

            #region 业务参数/订单基本信息
            //业务参数
            //订单基本信息
            /// <summary>
            /// 仓储编码，电商平台指定
            /// </summary>
            public string store_code { get; set; }
            /// <summary>
            /// 订单编码
            /// </summary>
            public string order_code { get; set; }
            /// <summary>
            /// 操作子类型: 001 进口BC接口;  002 出口BC;  201销售单进口BBC;  601采购单进口BBC
            /// </summary>
            public int order_type { get; set; }
            /// <summary>
            /// 订单来源双方约定
            /// </summary>
            public int order_source { get; set; }
            /// <summary>
            /// 订单创建时间
            /// </summary>
            public string order_create_time { get; set; }
            /// <summary>
            /// 进出口标识，I:进口；E:出口
            /// </summary>
            public string v_ieflag { get; set; }
            /// <summary>
            /// 进口收货人填写境内消费者证件号码，如：身份证/护照等
            /// </summary>
            public string v_consignor_code { get; set; }
            /// <summary>
            /// 证件类型:01:身份证、02:护照、03:其他
            /// </summary>
            public string v_card_type { get; set; }
            /// <summary>
            /// 一个口岸时的运输方式填报；出境货物的运输方式是按货物运离我国关境最后一个口岸时的运输
            /// 方式填报采用《运输方式代码》，我国海关对运载货物进出关境的运输方式的类别标识代码，采用1 位数字表，
            /// 进境货物的运输方式是按货物运抵我国关境第一个口岸时的运输方式填报；
            /// 出境货物的运输方式是按货物运离我国关境最后一个口岸时的运输方式填报1空运2海运3汽运4铁运
            /// </summary>
            public string v_transport_code { get; set; }
            /// <summary>
            /// 海关对进出口货物实际采用的外部包装方式的标识代码，采用1 位数字表示，如：1木箱、2纸箱、3桶装、4散装、5托盘、6包、7油罐车等
            /// </summary>
            public string v_package_typecode { get; set; }
            /// <summary>
            /// 起运国/运抵国（地区）进口为起运国(地区)代码，出口为运抵国(地区)代码
            /// </summary>
            public string v_qy_state { get; set; }
            /// <summary>
            /// 主要商品名称
            /// </summary>
            public string v_master_good_name { get; set; }
            /// <summary>
            /// 毛重
            /// </summary>
            public float n_kos { get; set; }
            /// <summary>
            /// 件数
            /// </summary>
            public int PACKAGE_COUNT { get; set; }
            /// <summary>
            /// 运输工具名称
            /// </summary>
            public string v_traf_name { get; set; }
            #endregion

            #region 业务参数/实际配送信息
            /// <summary>
            /// 物流公司编码（BBC、BC选传）销退单会使用物流上门取货的情形则不用
            /// </summary>
            public string tms_service_code { get; set; }
            /// <summary>
            /// 运单号,退货单有可能有运单号
            /// </summary>
            public string tms_order_code { get; set; }
            /// <summary>
            /// 原ＺＹ订单编码，在退换货时会用到退货入库单 时可能会有
            /// </summary>
            public string prev_order_code { get; set; }
            /// <summary>
            /// 总运单号
            /// </summary>
            public string totalmailno { get; set; }
            #endregion

            #region 业务参数/收发货方信息
            /// <summary>
            /// 收货方： 手机和电话必选其一（BBC、BC必传）收货方信息:邮编^^^省^^^市^^^区^^^具体地址^^^收件方名称^^^手机^^^电话
            /// </summary>
            public string receiver_info { get; set; }
            /// <summary>
            /// 发货方：手机和电话必选其一（BBC、BC必传）邮编^^^省^^^市^^^区^^^具体地址^^^发件方名称^^^手机^^^电话
            /// </summary>
            public string sender_info { get; set; }

            #endregion

            #region 业务参数/订单商品信息
            /// <summary>
            /// 订单商品信息
            /// </summary>
            public List<ZY_order_item> order_item_list { get; set; }
            /// <summary>
            /// 包裹数量
            /// </summary>
            public int package_count { get; set; }
            /// <summary>
            /// 1启邦BC 2启邦BBC3 启邦保税直邮
            /// </summary>
            public string delivery_method { get; set; }
            /// <summary>
            /// 备注
            /// </summary>
            public string remark { get; set; }
            /// <summary>
            /// 订单人姓名
            /// </summary>
            public string order_ename { get; set; }
            /// <summary>
            /// 订单人电话
            /// </summary>
            public string order_phone { get; set; }
            /// <summary>
            /// 订单人身份证号码
            /// </summary>
            public string order_cardno { get; set; }
            /// <summary>
            /// 运费
            /// </summary>
            public decimal freight { get; set; }
            /// <summary>
            /// 保价费
            /// </summary>
            public decimal Insurance_fee { get; set; }
            /// <summary>
            /// 税费
            /// </summary>
            public decimal tax { get; set; }
            #endregion
        }

        public class ZY_order_item
        {
            /// <summary>
            /// 订单商品信息
            /// </summary>
            List<OrderItem> order_item { get; set; }
        }

        public class OrderItem
        {
            /// <summary>
            /// 商品备案号
            /// </summary>
            public string v_goods_regist_no { get; set; }
            /// <summary>
            /// 订单商品 ID，订单商品 ID，需要对推送过来的商品进行排序，填写自然数
            /// </summary>
            public long order_item_id { get; set; }
            /// <summary>
            /// 商品编码
            /// </summary>
            public long item_id { get; set; }
            /// <summary>
            /// 商品名称
            /// </summary>
            public string item_name { get; set; }
            /// <summary>
            /// 商品货号
            /// </summary>
            public string item_code { get; set; }
            /// <summary>
            /// 库存类型（BBC、BC必传）:1 可销售库存
            /// </summary>
            public int inventory_type { get; set; }
            /// <summary>
            /// 商品数量
            /// </summary>
            public long item_quantity { get; set; }
            /// <summary>
            /// 销售价格
            /// </summary>
            public long item_price { get; set; }
            /// <summary>
            /// 商品版本，ZY允许货主修改商品属性描述，每修改一次就对应一个商品版本信息,默认传1
            /// </summary>
            public int item_version { get; set; }
            /// <summary>
            /// 扩展字段（特殊业务才会下发这个信息，一般此字段是空值）
            /// </summary>
            public string attributes { get; set; }
            /// <summary>
            /// 商家代码
            /// </summary>
            public string cus_code { get; set; }
            /// <summary>
            /// Sku代码
            /// </summary>
            public string sku_code { get; set; }
            /// <summary>
            /// 规格型号
            /// </summary>
            public string item_spec { get; set; }
        }

        #endregion

        #region 私有方法
        static string GetResponse(string url, string param)
        {
            byte[] postData = Encoding.UTF8.GetBytes(param);

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.Accept = "application/json";
            req.ContentType = "application/x-www-form-urlencoded";
            req.KeepAlive = true;
            req.Timeout = 300000;
            req.ContentLength = postData.Length;

            Stream reqStream = req.GetRequestStream();

            reqStream.Write(postData, 0, postData.Length);

            reqStream.Close();
            HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();

            Stream stream = rsp.GetResponseStream();

            StreamReader sr = new StreamReader(stream, Encoding.UTF8);
            string result = sr.ReadToEnd();
            sr.Close();
            stream.Close();

            return result;
        }
        public static string get_Xml_Nodes(XmlDocument Xml, string xPath)
        {
            XmlNodeList Xml_Nodes = Xml.SelectNodes(xPath);
            try
            {
                return Xml_Nodes[0].InnerText.Replace("'", "`");
            }
            catch
            {
                return "";
            }
        }
        /// <summary>
        /// 签名字符串
        /// </summary>
        /// <param name="prestr">需要签名的字符串</param>
        /// <param name="key">密钥</param>
        /// <param name="_input_charset">编码格式</param>
        /// <returns>签名结果</returns>
        public string Sign(string prestr, string key, string _input_charset)
        {
            StringBuilder sb = new StringBuilder();

            prestr = prestr + key;

            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(Encoding.GetEncoding(_input_charset).GetBytes(prestr));
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            }

            return sb.ToString();
        }
        #endregion

    }
}
