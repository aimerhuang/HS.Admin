using com.ehking.utils;
using com.ekhing.OnlinePay;
using com.ekhing.Web;
using Hyt.Model;
using Hyt.Model.Common;
using Hyt.Model.Parameter;
using Hyt.Model.WorkflowStatus;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Hyt.BLL.ApiPay.EhkingCip
{
    /// <summary>
    /// 易宝支付推送国检
    /// </summary>
    public class PayProvider
    {
        private static PayConfig config = Hyt.BLL.Config.Config.Instance.GetPayConfig();
        /// <summary>
        /// 支付商户号
        /// </summary>
        private static string EhkingMerhantId = config.EhkingMerhantId;
        /// <summary>
        /// 支付密钥
        /// </summary>
        private static string EhkingKey = config.EhkingKey;
        /// <summary>
        /// 接口请求URL
        /// </summary>
        private static string EhkingNodeAuthorizationUrl = "https://api.ehking.com/ciq/order";
        /// <summary>
        /// 国检组织代码:000069 广州 ，空值  西安
        /// </summary>
        private static string ciqCode = "000069";
        /// <summary>
        /// 国检备案号
        /// </summary>
        private static string commerceCode = "";
        /// <summary>
        /// 国检备案名称
        /// </summary>
        private static string commerceName = "";
        /// <summary>
        /// 支付企业代码
        /// </summary>
        /// <remarks>2016-12-26 周 创建</remarks>
        public static CommonEnum.PayCode Code
        {
            get { return CommonEnum.PayCode.易宝; }
        }
        /// <summary>
        /// 海关通道
        /// </summary>
        /// <remarks>2016-12-26 周 创建</remarks>
        public static string CustomsChannel = "GUANGZHOU";
       
        /// <summary>
        /// 海关报关
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-12-26 周 创建</remarks>
        public static Result ApplyToCustoms(SoOrder order)
        {
            Result result = new Result()
            {
                Status = false
            };

            try
            {

                if (order != null)
                {
                    var ehkingMerchantId = EhkingMerhantId;
                    var ehkingKey = EhkingKey;

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

                    string merchantId = ehkingMerchantId;//商户号
                    string paySerialNumber = onlinePayments.First().VoucherNo;//支付流水号
                    string notifyUrl = "http://admin.gaopin999.com/Account/NotifyReceiptCip";//支付成功后易汇金支付会向该地址发送两次成功通知，该地址可以带参数

                    string nodeAuthorizationURL = EhkingNodeAuthorizationUrl;
                    string keyValue = ehkingKey;
                    string amount = "0";

                    StringBuilder sbHmac = new StringBuilder();

                    sbHmac.Append(merchantId);
                    sbHmac.Append(paySerialNumber);
                    sbHmac.Append(notifyUrl);

                    amount = (order.OrderAmount * 100).ToString("F0");

                    sbHmac.Append(receiveAddress.Name);//用户姓名
                    sbHmac.Append(receiveAddress.IDCardNo);//证件号码
                    sbHmac.Append(receiveAddress.MobilePhoneNumber);//手机号码

                    sbHmac.Append(CustomsChannel);//海关通道
                    sbHmac.Append(ciqCode);//国检组织代码
                    sbHmac.Append(amount);//支付金额，单位：分
                    sbHmac.Append(commerceCode);//国检备案号
                    sbHmac.Append(commerceName);//国检组织代码

                    string hmac = Digest.HmacSign(sbHmac.ToString(), keyValue);

                    HttpClient client = new HttpClient(nodeAuthorizationURL);

                    string data = "{\"merchantId\":\"" + merchantId +
                                "\",\"paySerialNumber\":\"" + paySerialNumber +
                                "\",\"notifyUrl\":\"" + notifyUrl + "\"" +
                                ",\"payer\": {" +
                                   "\"payerName\": \"" + receiveAddress.Name + "\"," +
                                    "\"idNum\": \"" + receiveAddress.IDCardNo + "\"," +
                                    "\"phoneNum\": \"" + receiveAddress.MobilePhoneNumber + "\"" +
                                   "}" +
                                    ",\"ciqInfos\":[{\"ciqChannel\":\"" + CustomsChannel +
                                                   "\",\"ciqCode\":\"" + ciqCode +
                                                   "\",\"amount\":\"" + amount +
                                                   "\",\"commerceCode\":\"" + commerceCode +
                                                   "\",\"commerceName\":\"" + commerceName + "\"}],\"hmac\":\"" + hmac + "\"}";

                    string responsestr = client.Post(data);

                    BLL.Log.LocalLogBo.Instance.Write("提交的数据：" + data + ",-------返回数据：" + responsestr, "EhkingCipLog");

                    var back = JObject.Parse(responsestr);

                    if (back["status"].ToString() == "SUCCESS")
                    {
                        result.Message = "提交成功！";
                        result.Status = true;
                        //更新订单支付报关状态为处理中
                        Hyt.BLL.Order.SoOrderBo.Instance.UpdateOrderApiStatus((int)OrderStatus.支付申报国检状态.处理中, 4, order.SysNo);
                        BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "订单编号：" + order.SysNo + ",支付信息报检提交成功！", LogStatus.系统日志目标类型.订单支付申报国检, order.SysNo, 0);
                    }
                    else if (back["status"].ToString() == "FAILED")
                    {
                        result.Message = "提交失败！";
                        result.Status = false;
                        BLL.Log.LocalLogBo.Instance.Write("提交失败！" + responsestr, "EhkingCipERRORLog");
                    }
                    else if (back["status"].ToString() == "ERROR")
                    {
                        if (back["error"].ToString() == "exception.record.exists")
                        {
                            //Hyt.BLL.Order.SoOrderBo.Instance.UpdateOrderApiStatus((int)OrderStatus.支付申报国检状态.处理中, 4, order.SysNo);
                            result = CustomsQuery(order.SysNo);
                            return result;
                        }
                        else
                            result.Message = "提交失败,错误代码:" + back["error"].ToString();

                        BLL.Log.LocalLogBo.Instance.Write("提交失败！" + responsestr, "EhkingCipERRORLog");
                        result.Status = false;
                    }
                    else
                    {
                        result.Message = "其他异常！";
                        result.Status = false;
                        BLL.Log.LocalLogBo.Instance.Write("其他异常！" + responsestr, "EhkingCipLog");
                    }

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
        public static Result NotifyReceipt(string requestStr)
        {
            var result = NotifyReceipt(requestStr);

            try
            {
                string key = config.EhkingKey;

                var back = JObject.Parse(requestStr);
                string paySerialNumber = back["paySerialNumber"].ToString();


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

                    var customsInfos = back["ciqInfos"][0];
                    OrderStatus.支付申报国检状态 status = OrderStatus.支付申报国检状态.处理中;
                    if (customsInfos["status"].ToString() == "SUCCESS")
                    {
                        status = OrderStatus.支付申报国检状态.成功;
                        result.Status = true;
                        result.Message = "异步报检成功！";
                    }
                    else if (customsInfos["status"].ToString() == "FAILED")
                    {
                        status = OrderStatus.支付申报国检状态.失败;
                        result.Status = false;
                        result.Message = "异步报检失败！";
                    }
                    BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "订单编号：" + soOrder.SysNo + ",支付信息异步报检" + (result.Status==true?"成功":"失败"), LogStatus.系统日志目标类型.订单支付申报国检, soOrder.SysNo, 0);
                    
                    if ("PROCESSING" != customsInfos["status"].ToString())
                        Hyt.BLL.Order.SoOrderBo.Instance.UpdateOrderApiStatus((int)status, 4, soOrder.SysNo);

                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
                BLL.Log.LocalLogBo.Instance.Write("报错！" + ex.Message, "NotifyReceiptCIPLog");
            }

            return result;
        }

        /// <summary>
        /// 海关支付报关查询
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <returns></returns>
        /// <remarks>2016-12-26 周 创建</remarks>
        public static Result CustomsQuery(int orderId)
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
            if (onlinePayments.Count() <= 0)
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
                    string url = "https://api.ehking.com/ciq/query";//查询网关
                    string hmac = Digest.HmacSign(ehkingMerchantId + serialNumber, ehkingKey);
                    HttpClient client = new HttpClient(url);
                    data = "{\"merchantId\":\"" + ehkingMerchantId + "\",\"paySerialNumber\":\"" + serialNumber + "\",\"hmac\":\"" + hmac + "\"}";
                    responsestr = client.Post(data);

                    var back = JObject.Parse(responsestr);
                    var customsInfos = back.Property("ciqInfos") != null ? back["ciqInfos"][0] : back;
                    OrderStatus.支付申报国检状态 status = OrderStatus.支付申报国检状态.失败;
                    if (customsInfos["status"].ToString() == "SUCCESS")
                    {
                        result.Message = "支付推送国检成功";
                        result.Status = true;
                        status = OrderStatus.支付申报国检状态.成功;
                    }
                    else if ("PROCESSING" == customsInfos["status"].ToString())
                    {
                        result.Message = "国检正在审核...，稍后再查询！";
                        status = OrderStatus.支付申报国检状态.处理中;
                    }
                    else
                    {
                        //BLL.Log.LocalLogBo.Instance.Write("失败！" + responsestr, "CustomsQueryLog");
                        result.Message = "易宝返回状态:" + customsInfos["status"].ToString() + "(" + (back.Property("customsInfos") == null ? customsInfos["error"].ToString() : "") + ")";
                    }


                    Hyt.BLL.Order.SoOrderBo.Instance.UpdateOrderApiStatus((int)status, 4, orderId);

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
        public static Result QueryOrderState(int orderId)
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
            var _result = JObject.Parse(responseStr);

            string status = _result["status"].ToString();
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

    }
}
