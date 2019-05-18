using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Extra.UpGrade.Model;
using Extra.UpGrade.Provider;
using Hyt.Model;
using Hyt.Model.UpGrade;
using Hyt.Model.WorkflowStatus;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Security.Cryptography;
using System.IO;

namespace Extra.UpGrade.UpGrades
{
    #region 国美订单接口实现
    /// <summary>
    /// 国美订单接口实现
    /// </summary>
    /// <remarks>2017-08-31 黄杰 创建</remarks>
    public class GuoMeiUpGrade : IUpGrade
    {

        /// <summary>
        /// 国美订单接口配置信息
        /// </summary>
        private static readonly GuoMeiConfig config = UpGradeConfig.GetGuoMeiConfig();

        #region 给一个字符串进行MD5加密
        /// <summary>  
        /// 给一个字符串进行MD5加密  
        /// </summary>  
        /// <param   name="strText">待加密字符串</param>  
        /// <returns>加密后的字符串</returns>  
        /// <remarks>2013-10-22 杨浩 添加</remarks>
        public string MD5Encrypt(string strText)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(strText));

            string outString = "";
            for (int i = 0; i < result.Length; i++)
            {
                outString += result[i].ToString("x2");
            }

            return outString;

        }
        #endregion

        #region 国美支付类型转为系统支付类型
        /// <summary>
        /// 国美支付类型转为系统支付类型
        /// </summary>
        /// <param name="payType">国美支付类型</param>
        /// <returns></returns>
        /// <reamrks>2017-08-18 黄杰 创建</reamrks>
        public int GuoMeiToPayTypeSysNo(string payType)
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

        #region 国美获取获取已升舱待发货订单
        /// <summary>
        /// 获取已升舱待发货订单
        /// </summary>
        /// <param name="param">条件参数</param>
        /// <param name="auth">授权参数</param>
        /// <returns>订单列表</returns>
        /// <reamrks>2017-08-31 黄杰 创建</reamrks>
        public Result<List<UpGradeOrder>> GetUpGradedWaitSend(OrderParameters param, AuthorizationParameters auth)
        {

            return null;
        }
        #endregion

        #region SHA1 加密，返回大写字符串
        public static string SHA1(string content, Encoding encode)
        {
            try
            {
                SHA1 sha1 = new SHA1CryptoServiceProvider();
                byte[] bytes_in = encode.GetBytes(content);
                byte[] bytes_out = sha1.ComputeHash(bytes_in);
                sha1.Dispose();
                string result = BitConverter.ToString(bytes_out);
                result = result.Replace("-", "");
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("SHA1加密出错：" + ex.Message);
            }
        }
        #endregion

        #region 国美批量获取指定时间区间的订单
        /// <summary>
        /// 批量获取指定时间区间的订单
        /// (待升舱的订单)
        /// </summary>
        /// <param name="param">参数实体</param>
        /// <param name="auth">授权参数</param>
        /// <returns>订单列表</returns>
        /// <reamrks>2017-09-06 黄杰 创建</reamrks>
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

            //签名参数
            var signParameter = config.AppSecret + "$methodgome.order.orders.get$token" + config.AccessToken + "$v1.0.0endDate" + param.EndDate.ToString("yyyy-MM-dd HH:mm:ss") + "pageNo" + param.PageIndex + "pageSize" + param.PageSize + "startDate" + param.StartDate.ToString("yyyy-MM-dd HH:mm:ss") + "statusPR" + config.AppSecret;

            var sign = SHA1(signParameter, Encoding.UTF8).ToUpper();

            string req = "$token=" + config.AccessToken + "&$method=gome.order.orders.get&$v=1.0.0&$sign=" + sign + "&startDate=" + param.StartDate.ToString("yyyy-MM-dd HH:mm:ss") + "&endDate=" + param.EndDate.ToString("yyyy-MM-dd HH:mm:ss") + "&status=PR&pageNo=" + param.PageIndex + "&pageSize=" + param.PageSize + "";

            var response = Hyt.Util.WebUtil.Get(config.ApiUrl + "?" + req);
            param.PageIndex = 1;

            while (true)
            {

                var trade = JObject.Parse(response);

                int total = int.Parse(trade["gome_order_orders_get_response"]["result"]["totalResult"].ToString());

                int totalPage = total / param.PageSize;
                if (total % param.PageSize != 0)
                    totalPage++;

                var trades = trade["gome_order_orders_get_response"]["result"]["orders"];

                bool has_next = param.PageIndex < totalPage;

                //trade = JObject.Parse(response.ToString());
                foreach (var i in trades)
                {
                    var order = new UpGradeOrder();

                    Map(i, order);
                    order.HytOrderDealer = dealerInfo;
                    // 支付方式（1货到付款, 2邮局汇款, 3自提, 4在线支付, 5公司转账, 6银行卡转账）  
                    order.HytOrderDealer.HytPayType = GuoMeiToPayTypeSysNo(i["payType"].ToString());
                    orderList.Add(order);
                }

                if (has_next)
                {
                    param.PageIndex++;
                    response = Hyt.Util.WebUtil.Get(config.ApiUrl + "?" + req);
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

            //第三方买家订单信息
            order.MallOrderBuyer = new MallOrderBuyerInfo()
            {
                BuyerNick = trade["consignee"]["name"].ToString(),
                MallOrderId = trade["orderId"].ToString(),
                BuyerMessage = "",
                SellerMessage = trade["opinionDesc"].ToString()
            };

            order.UpGradeOrderItems = new List<UpGradeOrderItem>();
            var orders = trade["orderDetails"];

            for (int i = 0; i < orders.Count(); i++)
            {
                var item = orders[i];
                var code = item["mainItemId"].ToString();
                order.UpGradeOrderItems.Add(new UpGradeOrderItem()
                {
                    MallProductCode = code,
                    MallProductName = item["itemName"].ToString(),
                    MallProductAttrId = item["itemId"].ToString(),
                    MallProductAttrs = item["itemName"].ToString(),
                    OrderId = trade["orderId"].ToString(),
                    MallPrice = decimal.Parse(item["price"].ToString()),
                    MallAmount = decimal.Parse(item["price"].ToString()),
                    Quantity = int.Parse(item["count"].ToString()),
                    DiscountFee = decimal.Parse((trade["partDiscountPrice"].ToString()) + (trade["couponValue"].ToString())),
                    MallOrderItemId = item["mainItemId"].ToString()
                });

            }

            //第三方订单交易信息
            order.MallOrderPayment = new MallOrderPaymentInfo()
            {
                Payment = decimal.Parse(trade["orderTotalPrice"].ToString()),
                AlipayNo = "",
                PostFee = decimal.Parse(trade["freightPrice"].ToString()),
                DiscountFee = decimal.Parse((trade["partDiscountPrice"].ToString()) + (trade["couponValue"].ToString())),
                PayTime = DateTime.Parse(trade["payTime"].ToString() == "" ? "1900-01-01" : trade["payTime"].ToString())
            };

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

            //订单收货信息
            order.MallOrderReceive = new MallOrderReceiveInfo()
            {
                City = trade["consignee"]["city"].ToString(),
                Province = trade["consignee"]["province"].ToString(),
                District = trade["consignee"]["county"].ToString(),
                ReceiveAddress = trade["consignee"]["address"].ToString(),
                ReceiveContact = trade["idName"].ToString(),
                TelPhone = "",//trade["consignee"]["telephone"].ToString(),
                Mobile = trade["consignee"]["mobilephone"].ToString(),
                PostCode = ""//trade["consignee"]["post"].ToString(),
            };

            //订单备注标示
            //order.MallOrderBuyer.SellerFlag = trade["seller_flag"].ToString();
        }
        #endregion

        #region 国美获取单笔订单详情
        /// <summary>
        /// 获取单笔订单详情
        /// </summary>
        /// <param name="param">参数实体</param>
        /// <param name="auth">授权参数</param>
        /// <returns>单笔订单详情</returns>
        /// <reamrks>2017-08-31 黄杰 创建</reamrks>
        public Result<UpGradeOrder> GetOrderDetail(OrderParameters param, AuthorizationParameters auth)
        {
            return null;
        }
        #endregion

        #region 国美订单出库、联系发货（线下物流）
        /// <summary>
        /// 用户调用该接口可实现自己联系发货（线下物流）
        /// </summary>
        /// <param name="param">参数实体</param>
        /// <param name="auth">授权参数</param>
        /// <returns>处理结果</returns>
        /// <reamrks>2017-09-06 黄杰 创建</reamrks>
        public Result SendDelivery(DeliveryParameters param, AuthorizationParameters auth)
        {
            var result = new Result() { Status = true };
            var _param = new Dictionary<string, string>();

            //签名参数
            var signParameter = config.AppSecret + "$methodgome.order.order.send$token" + config.AccessToken + "$v1.0.0carriersId" + param.CompanyCode + "logisticsNumber" + param.HytExpressNo + "orderId" + param.MallOrderId + config.AppSecret;

            //SHA1加密,获得签名
            var sign = SHA1(signParameter, Encoding.UTF8).ToUpper();

            string req = "$token=" + config.AccessToken + "&$method=gome.order.order.send&$v=1.0.0&$sign=" + sign + "&orderId=" + param.MallOrderId + "&carriersId=" + param.CompanyCode + "&logisticsNumber=" + param.HytExpressNo + "";

            var response = Hyt.Util.WebUtil.Get(config.ApiUrl + "?" + req);

            var _response = JObject.Parse(response.ToString());

            if (_response.Property("error_response") != null)
            {
                result.Status = false;
                result.Message = _response["error_response"]["msg"].ToString();
                result.errCode = _response["error_response"]["code"].ToString();
            }

            return result;
        }
        #endregion

        #region 国美获取可合并升舱订单列表
        /// <summary>
        /// 获取可合并升舱订单列表
        /// </summary>
        /// <param name="param">参数实体</param>    
        /// <param name="auth">授权参数</param>
        /// <returns>可合并升舱订单列表</returns>
        /// <reamrks>2017-08-31 黄杰 创建</reamrks>
        public Result<List<UpGradeOrder>> GetCombineOrders(OrderParameters param, AuthorizationParameters auth)
        {
            return null;
        }
        #endregion

        #region 国美使用授权码获取登录令牌
        /// <summary>
        /// 使用授权码获取登录令牌
        /// </summary>
        /// <param name="code">授权码</param>
        /// <returns>登录令牌</returns>
        /// <reamrks>2017-08-31 黄杰 创建</reamrks>
        public Result<AccessTokenResult> GetAuthorizationCode(string code)
        {
            return null;
        }
        #endregion

        #region 国美更新订单备注信息
        /// <summary>
        /// 更新订单备注信息
        /// </summary>
        /// <param name="remarks">备注内容</param>
        /// <param name="auth">授权参数</param>
        /// <reamrks>2017-08-31 黄杰 创建</reamrks>
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
