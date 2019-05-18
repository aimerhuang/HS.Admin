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
using Extra.UpGrade.SDK.Yihaodian;
using Extra.UpGrade.SDK.Yihaodian.Object;
using Extra.UpGrade.SDK.Yihaodian.Object.Order;
using Extra.UpGrade.SDK.Yihaodian.Request;
using Extra.UpGrade.SDK.Yihaodian.Response;
using Hyt.Util;

namespace Extra.UpGrade.UpGrades
{
    #region  一号店接口实现

    /// <summary>
    /// 一号店接口实现
    /// </summary>
    /// <remarks>2017-08-23 黄杰 创建</remarks>
    public class YihaodianUpGrade : IUpGrade
    {
        /// <summary>
        /// 一号店订单接口配置信息
        /// </summary>
        private static readonly YihaodianConfig config = UpGradeConfig.GetYihaodianConfig();
        //基于REST的TOP客户端
        private static readonly YhdClient client = new YhdClient(config.ApiUrl, config.AppKey, config.AppSecret);


        #region 一号店支付类型转为系统支付类型
        /// <summary>
        /// 一号店支付类型转为系统支付类型
        /// </summary>
        /// <param name="payType">一号店支付类型</param>
        /// <returns></returns>
        /// <reamrks>2017-08-23 黄杰 创建</reamrks>
        public int YihaodianToPayTypeSysNo(string payType)
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

        #region 根据条件获取订单
        /// <summary>
        /// 获取已升舱待发货订单(未实现)
        /// </summary>
        /// <param name="param">条件参数</param>
        /// <param name="auth">授权参数</param>
        /// <returns>订单列表</returns>
        /// <remarks>2017-08-23 黄杰 创建</remarks>
        public Result<List<UpGradeOrder>> GetUpGradedWaitSend(OrderParameters param, AuthorizationParameters auth)
        {
            return null;
        }
        #endregion 根据条件获取订单

        #region 获取一号店指定时间区间的订单
        /// <summary>
        /// 获取一号店指定时间区间的订单
        /// </summary>
        /// <param name="param">查询参数</param>
        /// <param name="auth">授权参数</param>
        /// <returns>一号店订单列表</returns>
        /// <remarks>2017-08-23 黄杰 创建</remarks>
        public Result<List<UpGradeOrder>> GetOrderList(OrderParameters param, AuthorizationParameters auth)
        {
            var dealerInfo = new HytOrderDealerInfo()
            {
                DealerSysNo = 1565,
                DealerMallSysNo = 10,
                HytPayStatus = 20,
                IsSelfSupport = 1,
            };

            var result = new Result<List<UpGradeOrder>> { Status = true, Message = "", Data = null, StatusCode = 0 };
            var orderList = new List<UpGradeOrder>();
            var _param = new Dictionary<string, string>();

            var req = new OrdersGetRequest()
            {
                /**订单状态（逗号分隔）: 
                    ORDER_WAIT_PAY：已下单（货款未全收）、 
                    ORDER_PAYED：已下单（货款已收）、 
                    ORDER_WAIT_SEND：可以发货（已送仓库）、 
                    ORDER_ON_SENDING：已出库（货在途）、 
                    ORDER_RECEIVED：货物用户已收到、 
                    ORDER_FINISH：订单完成、 
                    ORDER_CANCEL：订单取消 */
                OrderStatusList = "ORDER_WAIT_SEND",//可以发货（已送仓库）
                DateType = 2,//日期类型(1：订单生成日期，2：订单付款日期，3：订单发货日期，4：订单收货日期，5：订单更新日期) 
                StartTime = param.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),//查询开始时间 格式2011-12-12 20:10:10
                EndTime = param.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),//查询结束时间（时间差为15天）
                CurPage = 1,//当前页数
                PageRows = 80//每页显示记录数，默认50，最大100
            };

            var response = client.Execute(req, config.SessionKey);

            //循环获得订单编号，并根据订单编号获取订单详情
            while (true)
            {
                var trades = JObject.Parse(response.ToString());

                int totals = int.Parse(trades["response"]["totalCount"].ToString());

                int totalPages = totals / param.PageSize;
                if (totals % param.PageSize != 0)
                    totalPages++;

                var tradess = trades["response"]["orderList"]["order"];

                bool has_nexts = param.PageIndex < totalPages;

                //trade = JObject.Parse(response.ToString());
                foreach (var i in tradess)
                {
                    //根据获取的订单号取得订单详情
                    var reqs = new OrdersDetailGetRequest()
                    {
                        OrderCodeList = i["orderId"].ToString()
                    };
                    var responseData = client.Execute(reqs, config.SessionKey);

                    #region 把订单详情和MAP绑定
                    while (true)
                    {

                        var trade = JObject.Parse(responseData.ToString());

                        int total = int.Parse(trade["response"]["totalCount"].ToString());

                        int totalPage = total / param.PageSize;
                        if (total % param.PageSize != 0)
                            totalPage++;

                        var trades1 = trade["response"]["orderInfoList"]["orderInfo"];

                        bool has_next = param.PageIndex < totalPage;

                        //trade = JObject.Parse(response.ToString());
                        foreach (var j in trades1)
                        {
                            var order = new UpGradeOrder();

                            Map(j, order);
                            order.HytOrderDealer = dealerInfo;
                            //// 支付方式（一号店无支付方式选项）  
                            //order.HytOrderDealer.HytPayType = YihaodianToPayTypeSysNo(i["payType"].ToString());
                            orderList.Add(order);
                        }

                        if (has_next)
                        {
                            param.PageIndex++;
                            req.CurPage = param.PageIndex;
                            response = client.Execute(req, config.SessionKey);
                        }
                        else
                        {
                            break;
                        }
                    }
                    #endregion

                }

                if (has_nexts)
                {
                    param.PageIndex++;
                    req.CurPage = param.PageIndex;
                    response = client.Execute(req, config.SessionKey);
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
                BuyerNick = trade["orderDetail"]["goodReceiverName"].ToString(),
                MallOrderId = trade["orderDetail"]["orderId"].ToString(),
                //一号店的买家留言无效
                //BuyerMessage = trade["orderDetail"]["deliveryRemark"].ToString(),
                SellerMessage = trade["orderDetail"]["merchantRemark"].ToString()
            };

            //第三方订单商品信息
            order.UpGradeOrderItems = new List<UpGradeOrderItem>();

            var orders = trade["orderItemList"]["orderItem"];

            for (int i = 0; i < orders.Count(); i++)
            {
                var item = orders[i];
                var code = item["productId"].ToString();
                order.UpGradeOrderItems.Add(new UpGradeOrderItem()
                {
                    MallProductCode = code,
                    MallProductName = item["productCName"].ToString(),
                    MallProductAttrId = item["productId"].ToString(),
                    MallProductAttrs = item["productCName"].ToString(),
                    OrderId = item["orderId"].ToString(),
                    MallPrice = decimal.Parse(item["orderItemPrice"].ToString()),
                    MallAmount = decimal.Parse(item["orderItemAmount"].ToString()),
                    Quantity = int.Parse(item["orderItemNum"].ToString()),
                    DiscountFee = decimal.Parse(item["promotionAmount"].ToString()),
                    MallOrderItemId = item["id"].ToString()
                });

            }

            //第三方订单交易信息
            order.MallOrderPayment = new MallOrderPaymentInfo()
            {
                Payment = decimal.Parse(trade["orderDetail"]["realAmount"].ToString()),
                AlipayNo = "",
                PostFee = decimal.Parse(trade["orderDetail"]["orderDeliveryFee"].ToString()),

                DiscountFee = (decimal.Parse(trade["orderDetail"]["orderCouponDiscount"].ToString()) + decimal.Parse(trade["orderDetail"]["orderPromotionDiscount"].ToString())),
                PayTime = DateTime.Parse(trade["orderDetail"]["orderPaymentConfirmDate"].ToString() == "" ? "1900-01-01" : trade["orderDetail"]["orderPaymentConfirmDate"].ToString())
            };

            #region 当明细金额合计与实收金额不等时，将差额分摊到最后一个商品上

            if ((order.MallOrderPayment.Payment - order.MallOrderPayment.PostFee) != order.UpGradeOrderItems.Sum(i => i.MallAmount))
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
                City = trade["orderDetail"]["goodReceiverCity"].ToString(),
                Province = trade["orderDetail"]["goodReceiverProvince"].ToString(),
                District = trade["orderDetail"]["goodReceiverCounty"].ToString(),
                ReceiveAddress = trade["orderDetail"]["goodReceiverAddress"].ToString(),
                ReceiveContact = trade["orderDetail"]["goodReceiverName"].ToString(),
                //TelPhone = trade["orderDetail"]["goodReceiverPhone"].ToString(),
                Mobile = trade["orderDetail"]["goodReceiverMoblie"].ToString(),
                PostCode = trade["orderDetail"]["goodReceiverPostCode"].ToString(),
            };

            //订单备注标示
            //order.MallOrderBuyer.SellerFlag = trade["seller_flag"].ToString();
        }
        #endregion

        #region 获取接口错误信息
        /// <summary>
        /// 获取接口错误信息
        /// </summary>
        /// <param name="errList">错误列表</param>
        /// <returns>错误信息</returns>
        /// <remarks>2017-08-23 黄杰 创建</remarks>
        private string GetErrorMsg(ErrDetailInfoList errList)
        {
            return string.Join("|", errList.ErrDetailInfo.Select(e => (e.ErrorDes + ";" + e.PkInfo)));
        }
        #endregion

        #region 获取一号店订单详情
        /// <summary>
        /// 获取一号店订单详情
        /// </summary>
        /// <param name="param">查询参数</param>
        /// <param name="auth">授权参数</param>
        /// <returns>一号店订单详情</returns>
        /// <remarks>2017-08-23 黄杰 创建</remarks>
        public Result<UpGradeOrder> GetOrderDetail(OrderParameters param, AuthorizationParameters auth)
        {
            //var req = new OrderDetailGetRequest()
            //{
            //    OrderCode = param.OrderID
            //};
            //OrderDetailGetResponse response = client.Execute(req, auth.AuthorizationCode);
            //if (response.ErrInfoList != null || response.ErrorCount > 0)
            //{
            //    return new Result<UpGradeOrder>()
            //    {
            //        Status = false,
            //        StatusCode = -1,
            //        Message = GetErrorMsg(response.ErrInfoList),
            //        Data = null
            //    };
            //}

            //var upgrade = new UpGradeOrder();

            //#region  映射升舱订单实体

            //var order = response.OrderInfo;

            //upgrade.MallOrderBuyer = new MallOrderBuyerInfo()
            //{
            //    BuyerNick = order.OrderDetail.EndUserId.ToString(),
            //    MallOrderId = order.OrderDetail.OrderCode,
            //    BuyerMessage = order.OrderDetail.DeliveryRemark ?? string.Empty,
            //    SellerMessage = order.OrderDetail.MerchantRemark ?? string.Empty
            //};

            ////订单明细列表
            //upgrade.UpGradeOrderItems = order.OrderItemList.OrderItem.Select(item => new UpGradeOrderItem()
            //{
            //    MallProductCode = item.OuterId ?? item.ProductId.ToString() ?? item.MerchantId.ToString() ?? string.Empty,
            //    MallProductName = item.ProductCName,
            //    MallProductAttrId = (item.ProductId ?? 0) == 0 ? string.Empty : item.ProductId.ToString(),
            //    MallProductAttrs = string.Empty,
            //    OrderId = order.OrderDetail.OrderCode,
            //    MallPrice = Convert.ToDecimal(item.OrderItemPrice ?? 0),

            //    Quantity = item.OrderItemNum ?? 0,
            //    DiscountFee = Convert.ToDecimal(
            //                item.PromotionAmount//促销活动立减分摊金额
            //                + item.CouponAmountMerchant//商家抵用券分摊金额
            //                + item.CouponPlatformDiscount//1mall平台抵用券分摊金额
            //                + item.SubsidyAmount//节能补贴金额
            //    ),
            //    MallAmount = Convert.ToDecimal(item.OrderItemAmount ?? 0),
            //            //- Convert.ToDecimal(
            //            //    item.PromotionAmount//促销活动立减分摊金额
            //            //    + item.CouponAmountMerchant//商家抵用券分摊金额
            //            //    + item.CouponPlatformDiscount//1mall平台抵用券分摊金额
            //            //    + item.SubsidyAmount//节能补贴金额
            //            //)

            //    MallOrderItemId = item.Id.ToString()
            //}).ToList();

            ////第三方订单交易信息
            //upgrade.MallOrderPayment = new MallOrderPaymentInfo()
            //{
            //    AlipayNo = string.Empty,
            //    PostFee = Convert.ToDecimal(order.OrderDetail.OrderDeliveryFee ?? 0),
            //    DiscountFee = Convert.ToDecimal(
            //                order.OrderDetail.OrderCouponDiscount//商家抵用券支付金额
            //                + order.OrderDetail.OrderPlatformDiscount//1mall平台抵用券支付金额
            //                + order.OrderDetail.OrderPromotionDiscount//参加促销活动立减金额
            //    ),
            //    Payment = Convert.ToDecimal(order.OrderDetail.OrderAmount ?? 0),
            //                    //- Convert.ToDecimal(
            //                    //            order.OrderDetail.OrderCouponDiscount//商家抵用券支付金额
            //                    //            + order.OrderDetail.OrderPlatformDiscount//1mall平台抵用券支付金额
            //                    //            + order.OrderDetail.OrderPromotionDiscount//参加促销活动立减金额
            //                    //)
            //    PayTime = DateTime.Parse(order.OrderDetail.OrderPaymentConfirmDate)
            //};

            //#region 当明细金额合计与实收金额不等时，将差额分摊到最后一个商品上
            //if ((upgrade.MallOrderPayment.Payment - upgrade.MallOrderPayment.PostFee) != upgrade.UpGradeOrderItems.Sum(i => i.MallAmount))
            //{
            //    var _amt = 0m;
            //    var _count = 0;
            //    foreach (var item in upgrade.UpGradeOrderItems)
            //    {
            //        _count++;
            //        if (_count == upgrade.UpGradeOrderItems.Count)
            //        {
            //            item.MallAmount = (upgrade.MallOrderPayment.Payment - upgrade.MallOrderPayment.PostFee) - _amt;
            //            break;
            //        }
            //        _amt += item.MallAmount;
            //    }
            //}

            //#endregion

            ////订单收货信息
            //upgrade.MallOrderReceive = new MallOrderReceiveInfo()
            //{
            //    City = order.OrderDetail.GoodReceiverCity ?? string.Empty,
            //    Province = order.OrderDetail.GoodReceiverProvince ?? string.Empty,
            //    District = order.OrderDetail.GoodReceiverCounty ?? string.Empty,
            //    ReceiveAddress = order.OrderDetail.GoodReceiverAddress ?? string.Empty,
            //    ReceiveContact = order.OrderDetail.GoodReceiverName ?? string.Empty,
            //    TelPhone = order.OrderDetail.GoodReceiverPhone ?? string.Empty,
            //    Mobile = order.OrderDetail.GoodReceiverMoblie ?? string.Empty,
            //    PostCode = order.OrderDetail.GoodReceiverPostCode ?? string.Empty
            //};

            //#endregion

            //return new Result<UpGradeOrder>()
            //{
            //    Status = false,
            //    StatusCode = 1,
            //    Data = upgrade
            //};
            return null;
        }
        #endregion

        #region 一号店 自己联系物流（线下物流）发货
        /// <summary>
        /// 一号店 自己联系物流（线下物流）发货
        /// </summary>
        /// <param name="param">发货参数</param>
        /// <param name="auth">授权参数</param>
        /// <returns>发货结果</returns>
        /// <remarks>2017-08-24 黄杰 创建</remarks>
        public Result SendDelivery(DeliveryParameters param, AuthorizationParameters auth)
        {
            var result = new Result() { Status = true };
            var _param = new Dictionary<string, string>();

            //订单发货
            var req = new LogisticsOfflineSendRequest()
            {
                //订单编号
                Tid = param.MallOrderId.LongCount(),
                //配送商ID
                CompanyCode = param.CompanyCode,
                //运单号
                OutSid = param.HytExpressNo
            };

            var response = client.Execute(req, config.SessionKey);

            var _response = JObject.Parse(response.ToString());

            if (_response.Remove("logistics_offline_send_response shipping is_success") == false)
            {
                result.Status = false;
                result.Message = _response["logistics_offline_send_response"]["sub_msg"].ToString();
                result.StatusCode = int.Parse(_response["logistics_offline_send_response"]["error_code"].ToString());
                result.errCode = _response["logistics_offline_send_response"]["sub_code"].ToString();
            }
            return result;
        }
        #endregion

        #region 获取一号店订单可合并升舱订单列表
        /// <summary>
        /// 获取一号店订单可合并升舱订单列表
        /// </summary>
        /// <param name="param">查询参数</param>
        /// <param name="auth">授权参数</param>
        /// <returns>可合并升舱订单列表</returns>
        /// <remarks>2017-08-23 黄杰 创建</remarks>
        public Result<List<UpGradeOrder>> GetCombineOrders(OrderParameters param, AuthorizationParameters auth)
        {
            ////查询订单详细信息
            //var result = GetOrderDetail(param, auth).Data;

            ////今日所有待发货订单
            //param.StartDate = DateTime.Now.AddDays(-1);
            //param.EndDate = DateTime.Now;
            //var list = GetOrderList(param, auth).Data;

            ////订单详情、待发货订单列表不可为空
            //if (list == null || result == null)
            //    return new Result<List<UpGradeOrder>>() { Status = false, StatusCode = -1, Message = "授权失败，请重新授权后重试", Data = null };
            //list = list.Where(o => o.MallOrderBuyer.MallOrderId != result.MallOrderBuyer.MallOrderId).ToList();
            ////筛选今日可合并升舱订单
            //list = (from order in list
            //        where order.MallOrderBuyer.BuyerNick == result.MallOrderBuyer.BuyerNick
            //        && order.MallOrderReceive.ReceiveContact == result.MallOrderReceive.ReceiveContact
            //        && (order.MallOrderReceive.Mobile == result.MallOrderReceive.Mobile || order.MallOrderReceive.TelPhone == result.MallOrderReceive.TelPhone)
            //        && order.MallOrderReceive.Province == result.MallOrderReceive.Province
            //        && order.MallOrderReceive.City == result.MallOrderReceive.City
            //        && order.MallOrderReceive.District == result.MallOrderReceive.District
            //        orderby order.MallOrderBuyer.MallOrderId ascending
            //        select order).ToList();

            //return new Result<List<UpGradeOrder>>()
            //{
            //    Status = true,
            //    StatusCode = 1,
            //    Data = list
            //};
            return null;
        }
        #endregion

        #region 获取授权令牌
        /// <summary>
        /// 获取授权令牌
        /// </summary>
        /// <param name="code">授权码</param>
        /// <returns>授权令牌信息</returns>
        /// <remarks>2017-08-23 黄杰 创建</remarks>
        public Result<AccessTokenResult> GetAuthorizationCode(string code)
        {
            //IDictionary<string, string> dic = new Dictionary<string, string>();

            //dic.Add("client_id", config.AppKey);
            //dic.Add("client_secret", config.AppSecret);
            //dic.Add("grant_type", "authorization_code");
            //dic.Add("code", code);
            //dic.Add("redirect_uri", config.YihaodianCallBack);
            //dic.Add("view", "web");

            //var refreshToken = string.Empty;
            //var objJson = JObject.Parse(new Top.Api.Util.WebUtils().DoPost(config.AccessTokenUrl, dic));
            //AccessTokenResult data = null;
            //if (objJson.Property("accessToken") != null)
            //{
            //    data = new AccessTokenResult()
            //    {
            //        AccessToken = objJson["accessToken"].ToString(),
            //        UserNick = objJson["userCode"].ToString()
            //    };
            //}

            //return new Result<AccessTokenResult>()
            //{
            //    Status = true,
            //    StatusCode = 1,
            //    Data = data
            //};
            return null;
        }
        #endregion

        #region 更新订单备注信息
        /// <summary>
        /// 更新订单备注信息
        /// </summary>
        /// <param name="remarks">备注内容</param>
        /// <param name="auth">授权参数</param>
        /// <reamrks>2017-08-23 黄杰 创建</reamrks>
        public Result UpdateTradeRemarks(IRemarksParameters remarks, AuthorizationParameters auth)
        {
            throw new NotImplementedException();
        }
        #endregion



        public Result<UpGradeExpress> GetExpress(AuthorizationParameters auth)
        {
            throw new NotImplementedException();
        }
    }

    #endregion
}
