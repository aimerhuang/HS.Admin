using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Extra.UpGrade.Model;
using Extra.UpGrade.Provider;
using Hyt.Model;
using Hyt.Model.UpGrade;
using Hyt.Model.WorkflowStatus;
using Extra.UpGrade.Api;
using Newtonsoft.Json.Linq;
using Hyt.Util;
using System.Security.Cryptography;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace Extra.UpGrade.UpGrades
{
    #region 苏宁接口实现
    /// <summary>
    /// 苏宁订单接口实现
    /// </summary>
    /// <remarks>2017-09-07 黄杰 创建</remarks>
    public class SUNINGUpGrade : IUpGrade
    {
        /// <summary>
        /// 苏宁订单接口配置信息
        /// </summary>
        private static readonly SUNINGConfig config = UpGradeConfig.SUNINGConfig();


        #region 苏宁得到sign方法()
        ///1.业务数据（业务数据中间不能有回车换行、空格）进行base64编码(base64结果不需要回车换行) 
        //2.按照顺序依次拼接appMethod的值，appRequestTime的值，appkey的值，versionNo的值和第一步编码后的值 
        //3.将第二步的数据进行md5加密，得到签名信息
        public static string getSignInfo(string appSecret, string appMethod, string appRequestTime, string appKey, string versionNo, string resparam)
        {
            byte[] parambts = Encoding.UTF8.GetBytes(resparam);
            string base64Param = Convert.ToBase64String(parambts);

            StringBuilder signData = new StringBuilder();
            signData.Append(appSecret)
                         .Append(appMethod)
                         .Append(appRequestTime)
                         .Append(appKey)
                         .Append(versionNo)
                         .Append(base64Param);
            MD5 md5Hasher = MD5.Create();
            byte[] hashData = md5Hasher.ComputeHash(Encoding.Default.GetBytes(signData.ToString()));

            StringBuilder alSignData = new StringBuilder();
            //format each one as a hexadecimal string
            for (int i = 0; i < hashData.Length; i++)
            {
                alSignData.Append(hashData[i].ToString("x2"));
            }
            return alSignData.ToString();
        }
        #endregion

        #region 苏宁发送Post请求方法
        public static string GetPost(string appMethod, string appKey, string signInfo, string resparams, string appRequestTime, string format, string versionNo, string apiUrl)
        {
            string str = "";
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream stream = null;
            StreamReader sr = null;
            Stream reqStream = null;
            try
            {
                Encoding gbk = Encoding.UTF8;
                byte[] bs = gbk.GetBytes(resparams);
                ServicePointManager.Expect100Continue = false;
                request = (HttpWebRequest)WebRequest.Create(apiUrl);
                request.Method = "POST";
                request.ContentType = "text/html";
                request.KeepAlive = false;
                request.Headers.Add("AppMethod", appMethod);
                request.Headers.Add("AppRequestTime", appRequestTime);
                request.Headers.Add("format", format);
                request.Headers.Add("AppKey", appKey);
                request.Headers.Add("VersionNo", versionNo);
                request.Headers.Add("signInfo", signInfo);
                request.ContentLength = bs.Length;
                reqStream = request.GetRequestStream();
                reqStream.Write(bs, 0, bs.Length);
                response = (HttpWebResponse)request.GetResponse();
                stream = response.GetResponseStream();
                sr = new StreamReader(stream, gbk);
                str = sr.ReadToEnd();
                return str;
            }
            catch (Exception ex)
            {
                str = ex.Message;
            }
            finally
            {

                if (response != null)
                {
                    response.Close();
                }

                if (stream != null)
                {
                    stream.Dispose();
                }

                if (sr != null)
                {
                    sr.Dispose();
                }
                if (reqStream != null)
                {
                    reqStream.Dispose();
                }
            }

            return "";
        }
        #endregion

        #region 苏宁支付类型转为系统支付类型
        /// <summary>
        /// 苏宁支付类型转为系统支付类型
        /// </summary>
        /// <param name="payType">苏宁支付类型</param>
        /// <returns></returns>
        /// <remarks>2017-09-07 黄杰 创建</remarks>
        public int SUNINGToPayTypeSysNo(string payType)
        {
            int payTypeSysNo = Hyt.Model.SystemPredefined.PaymentType.分销商预存;
            switch (payType)
            {
                case "WEIXIN":
                case "WEIXIN_DAIXIAO":
                    payTypeSysNo = Hyt.Model.SystemPredefined.PaymentType.微信支付;
                    break;
                case "ALIPAY":
                    payTypeSysNo = Hyt.Model.SystemPredefined.PaymentType.支付宝;
                    break;
            }
            return payTypeSysNo;
        }
        #endregion

        #region 苏宁获取获取已升舱待发货订单
        /// <summary>
        /// 获取已升舱待发货订单
        /// </summary>
        /// <param name="param">条件参数</param>
        /// <param name="auth">授权参数</param>
        /// <returns>订单列表</returns>
        /// <remarks>2017-09-07 黄杰 创建</remarks>
        public Result<List<UpGradeOrder>> GetUpGradedWaitSend(OrderParameters param, AuthorizationParameters auth)
        {

            return null;
        }
        #endregion

        #region 苏宁批量获取指定时间区间的订单
        /// <summary>
        /// 批量获取指定时间区间的订单
        /// (待升舱的订单)
        /// </summary>
        /// <param name="param">参数实体</param>
        /// <param name="auth">授权参数</param>
        /// <returns>订单列表</returns>
        /// <remarks>2017-09-07 黄杰 创建</remarks>
        public Result<List<UpGradeOrder>> GetOrderList(OrderParameters param, AuthorizationParameters auth)
        {

            var dealerInfo = new HytOrderDealerInfo()
            {
                DealerSysNo = 129,
                DealerMallSysNo = 24,
                HytPayStatus = 20,
                IsSelfSupport = 1,
            };

            var result = new Result<List<UpGradeOrder>> { Status = true, Message = "", Data = null, StatusCode = 0 };
            var orderList = new List<UpGradeOrder>();
            var _param = new Dictionary<string, string>();

            string appMethod = "suning.custom.order.query";
            string appRequestTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string versionNo = "v1.2";
            //orderStatus订单头状态（10：买家已付款，20：卖家已发货，21：部分发货，30：交易成功，40：交易关闭）
            string resparam = "<sn_request><sn_body><orderQuery><startTime>" + param.StartDate.ToString("yyyy-MM-dd HH:mm:ss") + "</startTime><pageNo>" + param.PageIndex + "</pageNo><pageSize>" + param.PageSize + "</pageSize><endTime>" + param.EndDate.ToString("yyyy-MM-dd HH:mm:ss") + "</endTime><orderStatus>10</orderStatus></orderQuery></sn_body></sn_request>";
            string format = "json";

            string sign = getSignInfo(config.AppSecret, appMethod, appRequestTime, config.AppKey, versionNo, resparam);
            string response = GetPost(appMethod, config.AppKey, sign, resparam, appRequestTime, format, versionNo, config.ApiUrl);

            param.PageIndex = 1;

            while (true)
            {

                var trade = JObject.Parse(response);

                int total = int.Parse(trade["sn_responseContent"]["sn_head"]["totalSize"].ToString());

                int totalPage = total / param.PageSize;
                if (total % param.PageSize != 0)
                    totalPage++;

                var trades = trade["sn_responseContent"]["sn_body"]["orderQuery"];

                bool has_next = param.PageIndex < totalPage;

                //trade = JObject.Parse(response.ToString());
                foreach (var i in trades)
                {
                    var order = new UpGradeOrder();

                    Map(i, order);
                    order.HytOrderDealer = dealerInfo;

                    //获取orderDetail的值
                    var job = i["orderDetail"];
                    //去括号
                    //var delbracket = job.ToString().Trim('{').Trim('}');

                    for (int s = 0; s < job.Count(); s++)
                    {
                        var item = job[s];

                        //获取paymentList的值
                        var paymentList = item["paymentList"];

                        for (int j = 0; j < paymentList.Count(); j++)
                        {
                            var items = paymentList[j];
                            order.HytOrderDealer.HytPayType = SUNINGToPayTypeSysNo(items["paycode"].ToString());
                            orderList.Add(order);
                        }
                    }
                }

                if (has_next)
                {
                    param.PageIndex++;
                    response = GetPost(appMethod, config.AppKey, sign, resparam, appRequestTime, format, versionNo, config.ApiUrl);
                }
                else
                {
                    break;
                }
            }

            result.Data = orderList;

            return result;
        }
        #endregion

        #region MAP
        void Map(JToken trade, UpGradeOrder order)
        {
            //获取orderDetail数组的值
            var orders = trade["orderDetail"];

            for (int i = 0; i < orders.Count(); i++)
            {
                var item = orders[i];
                //第三方买家订单信息
                order.MallOrderBuyer = new MallOrderBuyerInfo()
                {
                    BuyerNick = item["payerCustomerName"].ToString(),
                    MallOrderId = trade["orderCode"].ToString(),
                    BuyerMessage = trade["buyerOrdRemark"].ToString(),
                    SellerMessage = trade["sellerOrdRemark"].ToString()
                };
            }

            order.UpGradeOrderItems = new List<UpGradeOrderItem>();


            for (int i = 0; i < orders.Count(); i++)
            {
                var item = orders[i];
                var code = item["productCode"].ToString();
                order.UpGradeOrderItems.Add(new UpGradeOrderItem()
                {
                    MallProductCode = code,
                    MallProductName = item["productName"].ToString(),
                    MallProductAttrId = item["productCode"].ToString(),
                    MallProductAttrs = item["productName"].ToString(),
                    OrderId = trade["orderCode"].ToString(),
                    MallPrice = decimal.Parse(item["unitPrice"].ToString()),
                    MallAmount = decimal.Parse(item["unitPrice"].ToString()),
                    Quantity = int.Parse(item["saleNum"].ToString().TrimEnd('0').TrimEnd('.')),
                    DiscountFee = decimal.Parse((item["coupontotalMoney"].ToString()) + (item["vouchertotalMoney"].ToString())),
                    MallOrderItemId = item["orderLineNumber"].ToString()
                });

            }

            for (int i = 0; i < orders.Count(); i++)
            {
                var item = orders[i];
                //第三方订单交易信息
                order.MallOrderPayment = new MallOrderPaymentInfo()
                {
                    Payment = decimal.Parse(item["payAmount"].ToString()),
                    AlipayNo = "",
                    PostFee = decimal.Parse(item["transportFee"].ToString()),
                    DiscountFee = decimal.Parse((item["coupontotalMoney"].ToString()) + (item["vouchertotalMoney"].ToString())),
                    PayTime = DateTime.Parse(trade["orderSaleTime"].ToString() == "" ? "1900-01-01" : trade["orderSaleTime"].ToString())
                };
            }

            #region 当明细金额合计与实收金额不等时，将差额分摊到最后一个商品上

            if ((order.MallOrderPayment.Payment - order.MallOrderPayment.PostFee) != order.UpGradeOrderItems.Sum(i => (i.MallAmount * i.Quantity)))
            {
                var _amt = 0m;
                var _count = 0;
                foreach (var item in order.UpGradeOrderItems)
                {
                    _count++;
                    if (_count == order.UpGradeOrderItems.Count)
                    {
                        item.MallAmount = (order.MallOrderPayment.Payment - order.MallOrderPayment.PostFee) - _amt;
                        break;
                    }
                    _amt += item.MallAmount;
                }
            }

            #endregion

            for (int i = 0; i < orders.Count(); i++)
            {
                var item = orders[i];
                //订单收货信息
                order.MallOrderReceive = new MallOrderReceiveInfo()
                {
                    City = trade["cityName"].ToString(),
                    Province = trade["provinceName"].ToString(),
                    District = trade["districtName"].ToString(),
                    ReceiveAddress = trade["customerAddress"].ToString(),
                    ReceiveContact = trade["customerName"].ToString(),
                    TelPhone = "",
                    Mobile = trade["mobNum"].ToString(),
                    PostCode = item["receivezipCode"].ToString()
                };
            }

            //订单备注标示
            //order.MallOrderBuyer.SellerFlag = trade["seller_flag"].ToString();
        }
        #endregion

        #region 苏宁获取单笔订单详情
        /// <summary>
        /// 获取单笔订单详情
        /// </summary>
        /// <param name="param">参数实体</param>
        /// <param name="auth">授权参数</param>
        /// <returns>单笔订单详情</returns>
        /// <remarks>2017-09-07 黄杰 创建</remarks>
        public Result<UpGradeOrder> GetOrderDetail(OrderParameters param, AuthorizationParameters auth)
        {
            return null;
        }
        #endregion

        #region 苏宁订单发货（线下物流）
        /// <summary>
        /// 苏宁订单发货（线下物流）
        /// </summary>
        /// <param name="param">参数实体</param>
        /// <param name="auth">授权参数</param>
        /// <returns>处理结果</returns>
        /// <remarks>2017-09-14 黄杰 创建</remarks>
        public Result SendDelivery(DeliveryParameters param, AuthorizationParameters auth)
        {
            var result = new Result() { Status = true };
            var _param = new Dictionary<string, string>();

            #region 获取订单行项目号

            string appMethods = "suning.custom.order.query";
            string appRequestTimes = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string versionNos = "v1.2";
            //orderStatus订单头状态（10：买家已付款，20：卖家已发货，21：部分发货，30：交易成功，40：交易关闭）
            string resparams = "<sn_request><sn_body><orderQuery><startTime>" + param.OrderParam.StartDate.ToString("yyyy-MM-dd HH:mm:ss") + "</startTime><pageNo>" + param.OrderParam.PageIndex + "</pageNo><pageSize>" + param.OrderParam.PageSize + "</pageSize><endTime>" + param.OrderParam.EndDate.ToString("yyyy-MM-dd HH:mm:ss") + "</endTime><orderStatus>10</orderStatus></orderQuery></sn_body></sn_request>";
            string formats = "json";

            string signs = getSignInfo(config.AppSecret, appMethods, appRequestTimes, config.AppKey, versionNos, resparams);
            string responses = GetPost(appMethods, config.AppKey, signs, resparams, appRequestTimes, formats, versionNos, config.ApiUrl);

            var trade = JObject.Parse(responses);

            var trades = trade["sn_responseContent"]["sn_body"]["orderQuery"];

            var paymentList = "";

            foreach (var i in trades)
            {

                //获取orderDetail的值
                var job = i["orderDetail"];

                for (int s = 0; s < job.Count(); s++)
                {
                    var item = job[s];

                    //获取b2c行项目号
                    paymentList = item["bLineNumber"].ToString();
                }
            }

            #endregion

            string appMethod = "suning.custom.seaorderdelivery.add";
            string appRequestTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string versionNo = "v1.2";
            //deliveryType发货方式按01：海外直邮方式发货02：商家保税区发货；03：苏宁保税区发货；
            string resparam = "<sn_request><sn_body><seaOrderDelivery><deliveryDetails><phoneIdentifyCode></phoneIdentifyCode><deliveryType>01</deliveryType><expressNo>" + param.HytExpressNo + "</expressNo><orderLineNumber>" + paymentList + "</orderLineNumber><expressCompanyCode>" + param.CompanyCode + "</expressCompanyCode></deliveryDetails><orderCode>" + param.MallOrderId + "</orderCode></seaOrderDelivery></sn_body></sn_request>";

            string format = "json";

            string sign = getSignInfo(config.AppSecret, appMethod, appRequestTime, config.AppKey, versionNo, resparam);
            string response = GetPost(appMethod, config.AppKey, sign, resparam, appRequestTime, format, versionNo, config.ApiUrl);

            var _response = JObject.Parse(response.ToString());

            if (_response.Property("sn_responseContent") != null)
            {
                result.Status = false;
                result.Message = "";//_response["sn_responseContent"]["sn_error"]["error_msg"].ToString();
                result.errCode = _response["sn_responseContent"]["sn_error"]["error_code"].ToString();
            }

            return result;
        }
        #endregion

        #region 苏宁获取可合并升舱订单列表
        /// <summary>
        /// 获取可合并升舱订单列表
        /// </summary>
        /// <param name="param">参数实体</param>    
        /// <param name="auth">授权参数</param>
        /// <returns>可合并升舱订单列表</returns>
        /// <remarks>2017-09-07 黄杰 创建</remarks>
        public Result<List<UpGradeOrder>> GetCombineOrders(OrderParameters param, AuthorizationParameters auth)
        {
            return null;
        }
        #endregion

        #region 苏宁使用授权码获取登录令牌
        /// <summary>
        /// 使用授权码获取登录令牌
        /// </summary>
        /// <param name="code">授权码</param>
        /// <returns>登录令牌</returns>
        /// <remarks>2017-09-07 黄杰 创建</remarks>
        public Result<AccessTokenResult> GetAuthorizationCode(string code)
        {
            return null;
        }
        #endregion

        #region 苏宁更新订单备注信息
        /// <summary>
        /// 更新订单备注信息
        /// </summary>
        /// <param name="remarks">备注内容</param>
        /// <param name="auth">授权参数</param>
        /// <remarks>2017-09-07 黄杰 创建</remarks>
        public Result UpdateTradeRemarks(IRemarksParameters remarks, AuthorizationParameters auth)
        {
            return null;
        }
        #endregion



        public Result<UpGradeExpress> GetExpress(AuthorizationParameters auth)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
}
