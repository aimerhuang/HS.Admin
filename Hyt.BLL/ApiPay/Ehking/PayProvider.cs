using Hyt.Model;
using Hyt.Model.Logis.XinYi;
using Hyt.Model.Manual;
using Hyt.Model.WorkflowStatus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Newtonsoft.Json.Linq;
using com.ehking.utils;
using com.ekhing.Web;
using Hyt.Model.Parameter;
using Hyt.Model.Common;
using com.ekhing.OnlinePay;

namespace Hyt.BLL.ApiPay.Ehking
{
    /// <summary>
    /// 易宝报关接口
    /// </summary>
    /// <remarks>2015-10-12 杨浩 创建</remarks>
    public class PayProvider:IPayProvider
    {
        private PayConfig config = Hyt.BLL.Config.Config.Instance.GetPayConfig();
        public PayProvider(){ }
        /// <summary>
        /// 支付企业代码
        /// </summary>
        /// <remarks>2015-12-26 杨浩 创建</remarks>
        public override CommonEnum.PayCode Code
        {
            get{return CommonEnum.PayCode.易宝;}            
        }
        /// <summary>
        /// 海关通道
        /// </summary>
        /// <remarks>2015-12-28 杨浩 创建</remarks>
        public string CustomsChannel
        {
            get
            {
                return "OFFICAL";
            }
        }
        /// <summary>
        /// 海关报关
        /// </summary>
        /// <returns></returns>
        /// <remarks>2015-10-12 杨浩 创建</remarks>
        public override Result ApplyToCustoms(SoOrder order)
        {
            Result result = new Result()
            {
                Status = false
            };

            try
            {

                if (order != null)
                {
                    var ehkingMerchantId = config.EhkingMerhantId;
                    var ehkingKey = config.EhkingKey;

                    var warehouse = BLL.Warehouse.WhWarehouseBo.Instance.GetWarehouse(order.DefaultWarehouseSysNo);
                    if (warehouse != null && warehouse.WarehouseType == (int)Model.WorkflowStatus.WarehouseStatus.仓库类型.门店)
                    {
                        var dealer = BLL.Stores.StoresBo.Instance.GetStoreByWarehouseId(warehouse.SysNo);
                        if (dealer != null)
                        {
                            var dealerPayType = BLL.Stores.StoresBo.Instance.GetStorePayType(dealer.SysNo);
                            if (!string.IsNullOrWhiteSpace(dealerPayType.AppKey) && !string.IsNullOrWhiteSpace(dealerPayType.AppSecret) && dealerPayType.PaymentTypeSysNo == (int)Model.CommonEnum.PayCode.易宝)
                            {
                                ehkingMerchantId = dealerPayType.AppKey;
                                ehkingKey = dealerPayType.AppSecret;
                            }
                        }
                    }

                    var onlinePaymentFilter = new ParaOnlinePaymentFilter()
                    {
                        OrderSysNo = order.SysNo,
                        Id = 1
                    };

                    var onlinePayments = Hyt.BLL.Finance.FinanceBo.Instance.GetOnlinePayments(onlinePaymentFilter).Rows;

                    var receiveAddress = Hyt.BLL.Order.SoOrderBo.Instance.GetOrderReceiveAddress(order.ReceiveAddressSysNo);

                    string nodeAuthorizationURL = config.EhkingNodeAuthorizationUrl;
                    string merchantId = ehkingMerchantId;
                    string keyValue = ehkingKey;
                    string serialNumber = onlinePayments.First().VoucherNo;
                    string notifyUrl = "http://admin.singingwhale.cn/ajax/notifyReceipt";// config.EhkingCustomsAsyncUrl;
                    string amount = "0";
                    string Freight = "0";
                    string GoodsAmount = "0";
                    string Tax = "0";
                    //var customsConfig = Hyt.BLL.Config.Config.Instance.GetCustomsConfig();
                    string dxpid = "DXPENT0000012770";

                    //if(!string.IsNullOrEmpty(customsConfig.DxpId))
                    //    dxpid = customsConfig.DxpId;


                    StringBuilder sbHmac = new StringBuilder();

                    sbHmac.Append(merchantId);
                    sbHmac.Append(serialNumber);
                    sbHmac.Append(notifyUrl);

                    amount = (order.CashPay * 100).ToString("F0");
                    Freight = (order.FreightAmount * 100).ToString("F0");
                    GoodsAmount = (order.ProductAmount * 100).ToString("F0");
                    Tax = (order.TaxFee * 100).ToString("F0");

                    sbHmac.Append(receiveAddress.Name);
                    sbHmac.Append(receiveAddress.IDCardNo);
                    sbHmac.Append(receiveAddress.MobilePhoneNumber);

                    sbHmac.Append(CustomsChannel);
                    sbHmac.Append(amount);
                    sbHmac.Append(Freight);
                    sbHmac.Append(GoodsAmount);
                    sbHmac.Append(Tax);
                    sbHmac.Append(dxpid);


                    string hmac = Digest.HmacSign(sbHmac.ToString(), keyValue);

                    HttpClient client = new HttpClient(nodeAuthorizationURL);

                    string data = "{\"merchantId\":\"" + merchantId +
                                "\",\"paySerialNumber\":\"" + serialNumber +
                                "\",\"notifyUrl\":\"" + notifyUrl + "\"" +
                                ",\"payer\": {" +
                                   "\"payerName\": \"" + receiveAddress.Name + "\"," +
                                    "\"idNum\": \"" + receiveAddress.IDCardNo + "\"," +
                                    "\"phoneNum\": \"" + receiveAddress.MobilePhoneNumber + "\"" +
                                   "}" +
                                    ",\"customsInfos\":[{\"customsChannel\":\"" + CustomsChannel +
                                                   "\",\"amount\":\"" + amount +
                                                   "\",\"freight\":\"" + Freight +
                                                   "\",\"goodsAmount\":\"" + GoodsAmount +
                                                   "\",\"tax\":\"" + Tax + "\",\"dxpid\":\"" + dxpid + "\"}],\"hmac\":\"" + hmac + "\"}";


                    BLL.Log.LocalLogBo.Instance.Write("提交的数据：" + data, "EhkingCustomsLog");

                    string responsestr = client.Post(data);

                    var back = JObject.Parse(responsestr);

                    if (back["status"].ToString() == "SUCCESS")
                    {
                        result.Message = "提交成功！";
                        result.Status = true;
                        //更新订单支付报关状态为处理中
                        Hyt.BLL.Order.SoOrderBo.Instance.UpdateOrderApiStatus((int)OrderStatus.支付报关状态.处理中, 0, order.SysNo);
                        BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.前台, "订单编号：" + order.SysNo + ",支付信息报关提交成功！", LogStatus.系统日志目标类型.订单支付报关, order.SysNo, 0);
                    }
                    else if (back["status"].ToString() == "FAILED")
                    {
                        result.Message = "提交失败！";
                        result.Status = false;
                        BLL.Log.LocalLogBo.Instance.Write("提交失败！" + responsestr, "EhkingCustomsLog");
                    }
                    else if (back["status"].ToString() == "ERROR")
                    {
                        if (back["error"].ToString() == "exception.record.exists.customsOrder")//已提交则设置状态为待处理
                        {
                            result.Message = "此订单已提交，请查询！";
                            Hyt.BLL.Order.SoOrderBo.Instance.UpdateOrderApiStatus((int)OrderStatus.支付报关状态.处理中, 0, order.SysNo);
                        }
                        else
                            result.Message = "提交失败,错误代码:" + back["error"].ToString();

                        BLL.Log.LocalLogBo.Instance.Write("提交失败！" + responsestr, "EhkingCustomsERRORLog");
                        result.Status = false;
                    }
                    else
                    {
                        result.Message = "其他异常！";
                        result.Status = false;
                        BLL.Log.LocalLogBo.Instance.Write("其他异常！" + responsestr, "EhkingCustomsLog");
                    }
                    //推送国检
                    //Hyt.BLL.ApiPay.EhkingCip.PayProvider.ApplyToCustoms(order);
                }
            }
            catch (Exception ex)
            {
                result.Message = "报错！";
                result.Status = false;
                BLL.Log.LocalLogBo.Instance.Write("报错！" + ex.Message, "EhkingCustomsLog");
            }
            return result;
        }
        /// <summary>
        /// 异步回执
        /// </summary>
        /// <param name="requestStr">http请求信息</param>
        /// <returns></returns>
        /// <remarks>2015-10-12 杨浩 创建</remarks>
        public override Result NotifyReceipt(string requestStr)
        {
            var result = base.NotifyReceipt(requestStr);

            try
            {
                string key = config.EhkingKey;

                var back = JObject.Parse(requestStr);
                string paySerialNumber = back["paySerialNumber"].ToString();         


                var onlinePayment= Hyt.BLL.Finance.FinanceBo.Instance.GetOnlinePaymentByVoucherNo(paySerialNumber);
                if(onlinePayment==null)
                {
                    result.Status = false;
                    result.Message = "支付信息不存在！";
                    return result;
                }
                var soOrder = BLL.Order.SoOrderBo.Instance.GetEntity(onlinePayment.SourceSysNo);


                if (soOrder != null)
                {

                    var customsInfos =back["customsInfos"][0];
                    OrderStatus.支付报关状态 status = OrderStatus.支付报关状态.失败;
                    if (customsInfos["status"].ToString() == "SUCCESS")
                    {                      
                        status = OrderStatus.支付报关状态.成功;
                        result.Status =true;
                        result.Message = "报关成功！";
                        BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.前台, "订单编号：" + soOrder.SysNo + ",支付信息报关成功！", LogStatus.系统日志目标类型.订单支付报关, soOrder.SysNo, 0);
                    }
                    else if (customsInfos["status"].ToString() == "FAILED")
                    {
                        status = OrderStatus.支付报关状态.失败;
                        result.Status = false;
                        result.Message = "报关失败！";                      
                    }

                    if ("PROCESSING"!= customsInfos["status"].ToString())
                        Hyt.BLL.Order.SoOrderBo.Instance.UpdateOrderApiStatus((int)status,0, soOrder.SysNo);          
                   
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
                BLL.Log.LocalLogBo.Instance.Write("报错！" + ex.Message, "NotifyReceiptLog");     
            }

            return result;
        }

        /// <summary>
        /// 海关支付报关查询
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <returns></returns>
        /// <remarks>2015-12-31 杨浩 创建</remarks
        public override Result CustomsQuery(int orderId)
        {
            Result result = new Result()
            {
                Status = false
            };

            var order = BLL.Order.SoOrderBo.Instance.GetEntity(orderId);

            var ehkingMerchantId = config.EhkingMerhantId;
            var ehkingKey = config.EhkingKey;

            var warehouse = BLL.Warehouse.WhWarehouseBo.Instance.GetWarehouse(order.DefaultWarehouseSysNo);
            if (warehouse != null && warehouse.WarehouseType == (int)Model.WorkflowStatus.WarehouseStatus.仓库类型.门店)
            {
                var dealer = BLL.Stores.StoresBo.Instance.GetStoreByWarehouseId(warehouse.SysNo);
                if (dealer != null)
                {
                    var dealerPayType = BLL.Stores.StoresBo.Instance.GetStorePayType(dealer.SysNo);
                    if (!string.IsNullOrWhiteSpace(dealerPayType.AppKey) && !string.IsNullOrWhiteSpace(dealerPayType.AppSecret) && dealerPayType.PaymentTypeSysNo == (int)Model.CommonEnum.PayCode.易宝)
                    {
                        ehkingMerchantId = dealerPayType.AppKey;
                        ehkingKey = dealerPayType.AppSecret;
                    }
                }
            }

            var onlinePaymentFilter = new ParaOnlinePaymentFilter()
            {
                OrderSysNo = orderId,
                Id = 1
            };
            var onlinePayments = Hyt.BLL.Finance.FinanceBo.Instance.GetOnlinePayments(onlinePaymentFilter).Rows;
            if(onlinePayments.Count()<=0)
            {
                result.Status = false;
                result.Message = "订单没有支付信息！";
            }
            else
            {
                string responsestr = "";
                string data = "";
                try
                {
                    string serialNumber = onlinePayments.First().VoucherNo;
                    string url = "https://api.ehking.com/customs/query";//查询网关
                    string hmac = Digest.HmacSign(ehkingMerchantId + serialNumber, ehkingKey);
                    HttpClient client = new HttpClient(url);
                    data = "{\"merchantId\":\"" + ehkingMerchantId + "\",\"paySerialNumber\":\"" + serialNumber + "\",\"hmac\":\"" + hmac + "\"}";
                    responsestr = client.Post(data);

                    var back = JObject.Parse(responsestr);
                    var customsInfos = back.Property("customsInfos") != null ? back["customsInfos"][0] : back;
                    OrderStatus.支付报关状态 status = OrderStatus.支付报关状态.失败;
                    if (customsInfos["status"].ToString() == "SUCCESS")
                    {

                        result.Message = "SUCCESS";
                        result.Status = true;
                        status = OrderStatus.支付报关状态.成功;
                    }
                    else if ("PROCESSING" == customsInfos["status"].ToString())
                    {
                        result.Message = "海关正在审核...，稍后再查询！";
                        status = OrderStatus.支付报关状态.处理中;
                    }
                    else
                    {
                        //BLL.Log.LocalLogBo.Instance.Write("失败！" + responsestr, "CustomsQueryLog");
                        result.Message ="易宝返回状态:"+customsInfos["status"].ToString()+"("+ (back.Property("customsInfos") == null?customsInfos["error"].ToString():"")+")";
                    }
                        

                    Hyt.BLL.Order.SoOrderBo.Instance.UpdateOrderApiStatus((int)status, 0, orderId);                     
                  
                }
                catch (Exception ex)
                {
                    result.Status = false;
                    result.Message = "系统异常！";
                    BLL.Log.LocalLogBo.Instance.Write("\r\n报错！" + ex.Message + ";responsestr=" + responsestr + ";data=" + data, "CustomsQuery");     
                }
             
            }
            
            return result;
        }
        /// <summary>
        /// 查询订单支付状态
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-12-20 杨浩 创建</remarks> 
        public override Result QueryOrderState(string orderId)
        {
            var result = new Result()
            {
                Status = false
            };
            string key = "0627eb791eadb57cd947dc32e59e563e";
            var payOrder = new OnlinePayOrder(key);
            payOrder.MerchantId = "120140222";//商户号
            payOrder.RequestId = "9942"; //订单号
            string hmac = Digest.HmacSign(payOrder.MerchantId + payOrder.RequestId, key);
            var client = new com.ekhing.Web.HttpClient("https://api.ehking.com/onlinePay/query");

            string data = "{\"merchantId\":\"" + payOrder.MerchantId +
                           "\",\"requestId\":\"" + payOrder.RequestId +
                           "\",\"hmac\":\"" + hmac + "\"}";


            string responseStr = client.Post(data);
            var _result=JObject.Parse(responseStr);
            
            string status=_result["status"].ToString();
            if (status == "SUCCESS")
            {
                string _hmac = _result["hmac"].ToString();
                string requestId = _result["requestId"].ToString();
                string serialNumber = _result["serialNumber"].ToString();
             
                string orderAmount = _result["orderAmount"].ToString();
                result.Status = true;
            }
            else if (status == "FAILED")
            {
                result.Message = _result["error"].ToString();
            }
           
            return result;
        }

        #region 广州电子口岸海关申报
        /// <summary>
        /// 广州电子口岸海关申报
        /// </summary>
        /// <returns></returns>
        public override Result ApplyToCustomsDZ30(SoOrder order)
        {
            var result = base.ApplyToCustomsDZ30(order);
            try
            {
                if (order != null)
                {
                    var ehkingMerchantId = config.EhkingMerhantId;
                    var ehkingKey = config.EhkingKey;
                    string EhkingNodeAuthorizationUrl = "https://api.ehking.com/customs/order";
                    string EhkingCustomsAsyncUrl = "http://admin.gaopin999.com/Account/NotifyReceiptDZ30";

                    var warehouse = BLL.Warehouse.WhWarehouseBo.Instance.GetWarehouse(order.DefaultWarehouseSysNo);
                    if (warehouse != null && warehouse.WarehouseType == (int)Model.WorkflowStatus.WarehouseStatus.仓库类型.门店)
                    {
                        var dealer = BLL.Stores.StoresBo.Instance.GetStoreByWarehouseId(warehouse.SysNo);
                        if (dealer != null)
                        {
                            var dealerPayType = BLL.Stores.StoresBo.Instance.GetStorePayType(dealer.SysNo);
                            if (!string.IsNullOrWhiteSpace(dealerPayType.AppKey) && !string.IsNullOrWhiteSpace(dealerPayType.AppSecret) && dealerPayType.PaymentTypeSysNo == (int)Model.CommonEnum.PayCode.易宝)
                            {
                                ehkingMerchantId = dealerPayType.AppKey;
                                ehkingKey = dealerPayType.AppSecret;
                            }
                        }
                    }

                    var onlinePaymentFilter = new ParaOnlinePaymentFilter()
                    {
                        OrderSysNo = order.SysNo,
                        Id = 1
                    };

                    var onlinePayments = Hyt.BLL.Finance.FinanceBo.Instance.GetOnlinePayments(onlinePaymentFilter).Rows;

                    var receiveAddress = Hyt.BLL.Order.SoOrderBo.Instance.GetOrderReceiveAddress(order.ReceiveAddressSysNo);

                    string nodeAuthorizationURL = EhkingNodeAuthorizationUrl;//请求地址
                    string merchantId = ehkingMerchantId;//商户编号
                    string keyValue = ehkingKey;//商户密钥
                    string customsChannels = "GUANGZHOUSP";//报关通道:GUANGZHOUSP(广州公共服务)
                    string serialNumber = onlinePayments.First().VoucherNo;//支付流水号
                    string notifyUrl = EhkingCustomsAsyncUrl;//异步回调
                    string amount = "0";//报关金额
                    string Freight = "0";//运费
                    string GoodsAmount = "0";//支付货款
                    string Tax = "0";//税费
                    string insuredAmount = "0";//保费
                    string merchantCommerceName = "广州鼎球生物科技有限公司";//企业备案名称
                    string merchantCommerceCode = "C011111100393744";//企业在海关的备案号
                    string storeHouse = "";//商品所在仓
                    string customsCode = "5141";//广州机场    5141;南沙旅检    5165;南沙货港    5167;番禺东发    5169;广州邮办    5145;黄埔开发区  5208;太平海关驻沙田办事处  5216;广州萝岗    5130;佛山海关快件监管现场  5185
                    string ciqCode = "442300";//机场的国检组织机构代码是：000067；黄埔开发区的组织机构代码：000077，黄埔状元谷组织机构代码：443433
                    string functionCode = "BC";//单向海关-CUS,单向国检-CIQ,同时发送-BC
                    string businessType = "B2C";//B2B2C,B2C

                    StringBuilder sbHmac = new StringBuilder();

                    sbHmac.Append(merchantId);
                    sbHmac.Append(serialNumber);
                    sbHmac.Append(notifyUrl);

                    amount = (order.CashPay * 100).ToString("F0");
                    Freight = (order.FreightAmount * 100).ToString("F0");
                    GoodsAmount = (order.ProductAmount * 100).ToString("F0");
                    Tax = (order.TaxFee * 100).ToString("F0");

                    sbHmac.Append(receiveAddress.Name);
                    sbHmac.Append(receiveAddress.IDCardNo);
                    sbHmac.Append(receiveAddress.MobilePhoneNumber);

                    sbHmac.Append(customsChannels);
                    sbHmac.Append(amount);
                    sbHmac.Append(Freight);
                    sbHmac.Append(GoodsAmount);
                    sbHmac.Append(Tax);
                    sbHmac.Append(insuredAmount);
                    sbHmac.Append(merchantCommerceName);
                    sbHmac.Append(merchantCommerceCode);
                    sbHmac.Append(storeHouse);
                    sbHmac.Append(customsCode);
                    sbHmac.Append(ciqCode);
                    sbHmac.Append(functionCode);
                    sbHmac.Append(businessType);


                    string hmac = Digest.HmacSign(sbHmac.ToString(), keyValue);

                    HttpClient client = new HttpClient(nodeAuthorizationURL);

                    string data = "{\"merchantId\":\"" + merchantId +
                                "\",\"paySerialNumber\":\"" + serialNumber +
                                "\",\"notifyUrl\":\"" + notifyUrl + "\"" +
                                ",\"payer\": {" +
                                   "\"payerName\": \"" + receiveAddress.Name + "\"," +
                                    "\"idNum\": \"" + receiveAddress.IDCardNo + "\"," +
                                    "\"phoneNum\": \"" + receiveAddress.MobilePhoneNumber + "\"" +
                                   "}" +
                                    ",\"customsInfos\":[{\"customsChannel\":\"" + customsChannels +
                                                   "\",\"amount\":\"" + amount +
                                                   "\",\"freight\":\"" + Freight +
                                                   "\",\"goodsAmount\":\"" + GoodsAmount +
                                                   "\",\"tax\":\"" + Tax +
                                                   "\",\"insuredAmount\":\"" + insuredAmount +
                                                   "\",\"merchantCommerceName\":\"" + merchantCommerceName +
                                                   "\",\"merchantCommerceCode\":\"" + merchantCommerceCode +
                                                   "\",\"storeHouse\":\"" + storeHouse +
                                                   "\",\"customsCode\":\"" + customsCode +
                                                   "\",\"ciqCode\":\"" + ciqCode +
                                                   "\",\"functionCode\":\"" + functionCode +
                                                   "\",\"businessType\":\"" + businessType +
                                                   "\"}],\"hmac\":\"" + hmac + "\"}";



                    string responsestr = client.Post(data);

                    BLL.Log.LocalLogBo.Instance.Write("提交的数据：" + data + ",------返回数据：" + responsestr, "EhkingCustomsDZ30Log");

                    var back = JObject.Parse(responsestr);

                    if (back["status"].ToString() == "SUCCESS")
                    {
                        result.Message = "提交成功！";
                        result.Status = true;
                        //更新订单支付报关状态为处理中
                        Hyt.BLL.Order.SoOrderBo.Instance.UpdateOrderApiStatus((int)OrderStatus.支付报关状态.处理中, 0, order.SysNo);
                        BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.前台, "订单编号：" + order.SysNo + ",支付信息报关提交成功！", LogStatus.系统日志目标类型.订单支付报关, order.SysNo, 0);
                    }
                    else if (back["status"].ToString() == "FAILED")
                    {
                        result.Message = "提交失败！";
                        result.Status = false;
                        BLL.Log.LocalLogBo.Instance.Write("提交失败！" + responsestr, "EhkingCustomsDZ30Log");
                    }
                    else if (back["status"].ToString() == "ERROR")
                    {
                        if (back["error"].ToString() == "exception.record.exists.customsOrder")//已提交则设置状态为待处理
                        {
                            //result.Message = "此订单已提交，请查询！";
                            //Hyt.BLL.Order.SoOrderBo.Instance.UpdateOrderApiStatus((int)OrderStatus.支付报关状态.处理中, 0, order.SysNo);
                            result = CustomsQuery(order.SysNo);
                            return result;
                        }
                        else
                            result.Message = "提交失败,错误代码:" + back["error"].ToString();

                        BLL.Log.LocalLogBo.Instance.Write("提交失败！" + responsestr, "EhkingCustomsDZ30ERRORLog");
                        result.Status = false;
                    }
                    else
                    {
                        result.Message = "其他异常！";
                        result.Status = false;
                        BLL.Log.LocalLogBo.Instance.Write("其他异常！" + responsestr, "EhkingCustomsDZ30Log");
                    }

                }
            }
            catch (Exception ex)
            {
                result.Message = "报错！";
                result.Status = false;
                BLL.Log.LocalLogBo.Instance.Write("报错！" + ex.Message, "EhkingCustomsDZ30Log");                  
            }  
            return result;
        }

        /// <summary>
        /// 异步回执
        /// </summary>
        /// <param name="requestStr">http请求信息</param>
        /// <returns></returns>
        /// <remarks>2015-10-12 杨浩 创建</remarks>
        public override Result NotifyReceiptDZ30(string requestStr)
        {
            //{"customsInfos":[{"amount":"2940","customsChannel":"GUANGZHOUSP","freight":"0","goodsAmount":"1750","status":"SUCCESS","tax":"1190"}],"hmac":"32e08a6bd21d00b73f7e9ec4c95b7364","merchantId":"120140856","paySerialNumber":"03d47f0ab0834d9693ee60a32f832be9"}
            var result = base.NotifyReceiptDZ30(requestStr);

            try
            {
                string key = config.EhkingKey;

                var back = JObject.Parse(requestStr);
                string paySerialNumber = back["paySerialNumber"].ToString();

                //BLL.Log.LocalLogBo.Instance.Write("异步是否执到行此步骤：" + paySerialNumber, "NotifyReceiptDZ30Log1");

                var onlinePayment = Hyt.BLL.Finance.FinanceBo.Instance.GetOnlinePaymentByVoucherNo(paySerialNumber);
                if (onlinePayment == null)
                {
                    result.Status = false;
                    result.Message = "支付信息不存在！";
                    return result;
                }
                var soOrder = BLL.Order.SoOrderBo.Instance.GetEntity(onlinePayment.SourceSysNo);


                if (soOrder != null)
                {
                    var customsInfos = back["customsInfos"][0];
                    OrderStatus.支付报关状态 status = OrderStatus.支付报关状态.处理中;
                    if (customsInfos["status"].ToString() == "SUCCESS")
                    {
                        status = OrderStatus.支付报关状态.成功;
                        result.Status = true;
                        result.Message = "报关成功！";
                        BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "订单编号：" + soOrder.SysNo + ",支付信息报关成功！", LogStatus.系统日志目标类型.订单支付报关, soOrder.SysNo, 0);
                    }
                    else if (customsInfos["status"].ToString() == "FAILED")
                    {
                        status = OrderStatus.支付报关状态.失败;
                        result.Status = false;
                        result.Message = "报关失败！";
                    }

                    if ("PROCESSING" != customsInfos["status"].ToString())
                        Hyt.BLL.Order.SoOrderBo.Instance.UpdateOrderApiStatus((int)status, 0, soOrder.SysNo);

                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
                BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "支付信息报关失败："+ex.Message, LogStatus.系统日志目标类型.订单支付报关, 0, 0);

                BLL.Log.LocalLogBo.Instance.Write("报错！" + ex.Message, "NotifyReceiptDZ30Log2");
            }

            return result;
        }

        #endregion

    }
}