using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Extra.UpGrade.Model;
using Extra.UpGrade.Provider;
using Hyt.Model;
using Hyt.Model.UpGrade;
using Hyt.Model.WorkflowStatus;
using QQBuySdk;

namespace Extra.UpGrade.UpGrades
{
    #region 拍拍接口实现

    /// <summary>
    /// 拍拍接口实现
    /// </summary>
    /// <remarks>2014-01-03 陶辉 创建</remarks>
    public class PaipaiUpGrade : IUpGrade
    {
        #region 全局变量

        /// <summary>
        /// 拍拍接口配置信息
        /// </summary>
        private static readonly PaipaiConfig config = UpGradeConfig.GetPaipaiConfig();

        #endregion

        #region 根据条件获取订单
        /// <summary>
        /// 获取已升舱待发货订单(未实现)
        /// </summary>
        /// <param name="param">条件参数</param>
        /// <param name="auth">授权参数</param>
        /// <returns>订单列表</returns>
        /// <reamrks>2014-04-08 黄波 创建</reamrks>
        public Result<List<UpGradeOrder>> GetUpGradedWaitSend(OrderParameters param, AuthorizationParameters auth)
        {
            return null;
        }
        #endregion 根据条件获取订单


        #region 拍拍接口实现
        /// <summary>
        /// 获取拍拍订单列表
        /// </summary>
        /// <param name="param">查询参数</param>
        /// <param name="auth">授权参数</param>
        /// <returns>拍拍订单列表</returns>
        /// <remarks>2013-11-10 陶辉 创建</remarks>
        public Result<List<UpGradeOrder>> GetOrderList(OrderParameters param, AuthorizationParameters auth)
        {
            #region 查询条件

            IDictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("format", "json");
            dic.Add("pureData", "1");
            dic.Add("sellerUin", auth.ShopAccount);
            dic.Add("timeType", "PAY");
            dic.Add("timeBegin", param.StartDate.ToString());
            dic.Add("timeEnd", param.EndDate.ToString());
            dic.Add("dealState", "DS_WAIT_SELLER_DELIVERY");
            if (param.IsUseFlag)
            {
                dic.Add("dealNoteType", config.DealNoteType);
            }
            if (!string.IsNullOrEmpty(param.ProductCode))
            {
                dic.Add("itemCode", param.ProductCode);
            }
            if (!string.IsNullOrEmpty(param.ProductName))
            {
                dic.Add("itemNameKey", param.ProductName);
            }
            if (!string.IsNullOrEmpty(param.OrderID))
            {
                dic.Add("dealCode", param.OrderID);
            }

            #endregion
            var client = new OpenApiOauth(config.AppOAuthID, config.SecretOAuthKey, long.Parse(auth.ShopAccount), auth.AuthorizationCode);
            var response = client.InvokeOpenApi("http://api.paipai.com/deal/sellerSearchDealList.xhtml", dic, null);
            var result = Hyt.Util.Serialization.JsonUtil.ToObject<PaipaiResult>(response);
            if (result.errorCode == "0")
            {

                var list = new List<UpGradeOrder>();
                //总页数
                var pageCount = result.pageTotal;
                while (pageCount > 0)
                {
                    if (dic.ContainsKey("pageIndex"))
                    {
                        dic["pageIndex"] = pageCount.ToString();
                    }
                    else
                    {
                        dic.Add("pageIndex", pageCount.ToString());
                    }
                    dic.Remove("sign");
                    response = client.InvokeOpenApi("http://api.paipai.com/deal/sellerSearchDealList.xhtml", dic, null);
                    result = Hyt.Util.Serialization.JsonUtil.ToObject<PaipaiResult>(response);
                    if (result.errorCode == "0")
                    {
                        foreach (DealInfo deal in result.dealList)
                        {
                            var order = new UpGradeOrder();

                            Map(deal, order);
                            order.MallOrderBuyer.SellerFlag = deal.dealNoteType;
                            list.Add(order);
                        }
                    }
                    pageCount--;
                }
                list = FilterMallOrder(list, param);
                return new Result<List<UpGradeOrder>>()
                {
                    Status = true,
                    StatusCode = 1,
                    Data = list
                };
            }
            else
            {
                return new Result<List<UpGradeOrder>>()
                {
                    Status = false,
                    StatusCode = -1,
                    Message = result.errorMessage,
                    Data = null
                };
            }

        }

        #region 同步订单条件过滤

        List<UpGradeOrder> FilterMallOrder(List<UpGradeOrder> list, OrderParameters param)
        {

            //昵称筛选
            if (!string.IsNullOrEmpty(param.BuyerNick))
                list = list.Where(o => o.MallOrderBuyer.BuyerNick == param.BuyerNick).ToList();

            //是有留言
            if (param.HasMessage == (int)DistributionStatus.买家是否备注.是)
                list = list.Where(o => o.MallOrderBuyer.BuyerMessage != string.Empty).ToList();

            //买家没留言
            if (param.HasMessage == (int)DistributionStatus.买家是否备注.否)
                list = list.Where(o => o.MallOrderBuyer.BuyerMessage == string.Empty).ToList();

            return list;
        }

        #endregion

        /// <summary>
        /// 获取拍拍订单详情
        /// </summary>
        /// <param name="param">查询参数</param>
        /// <param name="auth">授权参数</param>
        /// <returns>拍拍订单详情</returns>
        /// <remarks>2013-11-10 陶辉 创建</remarks>
        public Result<UpGradeOrder> GetOrderDetail(OrderParameters param, AuthorizationParameters auth)
        {
            IDictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("format", "json");
            dic.Add("pureData", "1");
            dic.Add("sellerUin", auth.ShopAccount);
            dic.Add("dealCode", param.OrderID);
            dic.Add("listItem", "1");

            var client = new OpenApiOauth(config.AppOAuthID, config.SecretOAuthKey, long.Parse(auth.ShopAccount), auth.AuthorizationCode);
            var response = client.InvokeOpenApi("http://api.paipai.com/deal/getDealDetail.xhtml", dic, null);
            var result = Hyt.Util.Serialization.JsonUtil.ToObject<DealInfo>(response);
            if (result.errorCode == "0")
            {

                var order = new UpGradeOrder();
                Map(result, order);

                return new Result<UpGradeOrder>()
                {
                    Status = true,
                    StatusCode = 1,
                    Data = order
                };
            }
            else
            {
                return new Result<UpGradeOrder>()
                {
                    Status = false,
                    StatusCode = -1,
                    Message = result.errorMessage,
                    Data = null
                };
            }
        }

        /// <summary>
        /// 发货
        /// </summary>
        /// <param name="param">查询参数</param>
        /// <param name="auth">授权参数</param>
        /// <returns></returns>
        public Result SendDelivery(DeliveryParameters param, AuthorizationParameters auth)
        {
            IDictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("format", "json");
            dic.Add("pureData", "1");
            dic.Add("sellerUin", auth.ShopAccount);
            dic.Add("dealCode", param.MallOrderId);
            dic.Add("logisticsName", param.CompanyCode);
            dic.Add("logisticsCode", param.HytExpressNo);
            dic.Add("arriveDays", "1");

            var client = new OpenApiOauth(config.AppOAuthID, config.SecretOAuthKey, long.Parse(auth.ShopAccount), auth.AuthorizationCode);
            var response = client.InvokeOpenApi("http://api.paipai.com/deal/sellerConsignDealItem.xhtml", dic, null);
            var result = Hyt.Util.Serialization.JsonUtil.ToObject<LogisticsResult>(response);
            if (result.errorCode == "0")
            {
                return new Result()
                {
                    Status = true,
                    StatusCode = 1
                };
            }
            else
            {
                return new Result()
                {
                    Status = false,
                    StatusCode = -1,
                    Message = result.errorMessage
                };
            }
        }

        /// <summary>
        /// 查找可合并升舱的拍拍订单
        /// </summary>
        /// <param name="param">查询参数</param>
        /// <param name="auth">授权参数</param>
        /// <returns>可合并升舱订单列表</returns>
        /// <remarks>2013-1-12 陶辉 创建</remarks>
        public Result<List<UpGradeOrder>> GetCombineOrders(OrderParameters param, AuthorizationParameters auth)
        {
            //查询订单详细信息
            var result = GetOrderDetail(param, auth).Data;

            //今日所有待发货订单
            param.StartDate = DateTime.Now.AddDays(-1);
            param.EndDate = DateTime.Now;
            var list = GetOrderList(param, auth).Data;

            //订单详情、待发货订单列表不可为空
            if (list == null || result == null)
                return new Result<List<UpGradeOrder>>() { Status = false, StatusCode = -1, Message = "授权失败，请重新授权后重试", Data = null };
            list = list.Where(o => o.MallOrderBuyer.MallOrderId != result.MallOrderBuyer.MallOrderId).ToList();
            //筛选今日可合并升舱订单
            list = (from order in list
                    where order.MallOrderBuyer.BuyerNick == result.MallOrderBuyer.BuyerNick
                    && order.MallOrderReceive.ReceiveContact == result.MallOrderReceive.ReceiveContact
                    && (order.MallOrderReceive.Mobile == result.MallOrderReceive.Mobile || order.MallOrderReceive.TelPhone == result.MallOrderReceive.TelPhone)
                    && order.MallOrderReceive.Province == result.MallOrderReceive.Province
                    && order.MallOrderReceive.City == result.MallOrderReceive.City
                    && order.MallOrderReceive.District == result.MallOrderReceive.District
                    orderby order.MallOrderBuyer.MallOrderId ascending
                    select order).ToList();

            return new Result<List<UpGradeOrder>>()
            {
                Status = true,
                StatusCode = 1,
                Data = list
            };
        }

        /// <summary>
        /// 获取授权令牌
        /// </summary>
        /// <param name="code">授权信息</param>
        /// <returns>授权令牌</returns>
        /// <remarks>2014-01-07 陶辉 创建</remarks>
        public Result<AccessTokenResult> GetAuthorizationCode(string code)
        {
            throw new NotImplementedException();
        }

        void Map(DealInfo deal, UpGradeOrder order)
        {
            //第三方买家订单信息
            order.MallOrderBuyer = new MallOrderBuyerInfo()
            {
                BuyerNick = deal.buyerName,
                MallOrderId = deal.dealCode,
                BuyerMessage = deal.buyerRemark ?? string.Empty,
                SellerMessage = deal.dealNote ?? string.Empty + "。" + deal.comboInfo ?? string.Empty
            };

            if (deal.itemList != null)
            {
                //订单明细列表
                order.UpGradeOrderItems = deal.itemList.Select(item => new UpGradeOrderItem
                {
                    MallProductCode = !string.IsNullOrEmpty(item.stockLocalCode) ? item.stockLocalCode : item.itemLocalCode,
                    MallProductName = item.itemName,
                    MallProductAttrId = item.skuId ?? string.Empty,
                    MallProductAttrs = item.stockAttr ?? string.Empty,
                    OrderId = deal.dealCode,
                    MallPrice = deal.dealPayFeeTotal == 0 ? 0 :
                                Math.Round((
                                        (deal.dealPayFeeTotal
                                            + decimal.Parse(string.IsNullOrEmpty(item.wanggouQuanAmt) ? "0" : item.wanggouQuanAmt))
                                            / (deal.dealPayFeeTotal - deal.couponFee)
                                            * (item.itemDealPrice - (item.itemAdjustPrice + item.itemDiscountFee) / item.itemDealCount)
                                            ) / 100, 2),
                    MallAmount = deal.dealPayFeeTotal == 0 ? 0 :
                                Math.Round((
                                        (deal.dealPayFeeTotal
                                            + decimal.Parse(string.IsNullOrEmpty(item.wanggouQuanAmt) ? "0" : item.wanggouQuanAmt))
                                            / (deal.dealPayFeeTotal - deal.couponFee)
                                            * (item.itemDealPrice - (item.itemAdjustPrice + item.itemDiscountFee) / item.itemDealCount)
                                            ) / 100, 2) * item.itemDealCount,
                    Quantity = item.itemDealCount,
                    DiscountFee = item.itemDiscountFee / 100,
                    MallOrderItemId = item.dealSubCode
                }).ToList();
            }

            //第三方订单交易信息
            order.MallOrderPayment = new MallOrderPaymentInfo()
            {
                Payment = deal.totalCash / 100,
                AlipayNo = deal.tenpayCode ?? string.Empty,
                PostFee = decimal.Parse(deal.freight ?? "0") / 100,
                DiscountFee = deal.couponFee / 100,
                PayTime = deal.payTime
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
                City = string.Empty,
                Province = string.Empty,
                District = string.Empty,
                ReceiveAddress = deal.receiverAddress ?? string.Empty,
                ReceiveContact = deal.receiverName ?? string.Empty,
                TelPhone = deal.receiverPhone ?? string.Empty,
                Mobile = deal.receiverMobile ?? string.Empty,
                PostCode = deal.receiverPostcode ?? string.Empty
            };
        }
        #endregion


        public Result UpdateTradeRemarks(IRemarksParameters remarks, AuthorizationParameters auth)
        {
            throw new NotImplementedException();
        }


        public Result<UpGradeExpress> GetExpress(AuthorizationParameters auth)
        {
            throw new NotImplementedException();
        }
    }

    #region 物流发货接口返回类型

    /// <summary>
    /// 物流发货接口返回类型
    /// </summary>
    public class LogisticsResult
    {
        /// <summary>
        /// 返回码
        /// </summary>
        public string errorCode { get; set; }

        /// <summary>
        /// 错误描述
        /// </summary>
        public string errorMessage { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string dealCode { get; set; }
    }

    #endregion

    #endregion
}
