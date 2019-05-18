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
using Top.Api;
using Top.Api.Domain;
using Top.Api.Request;
using Top.Api.Response;

namespace Extra.UpGrade.UpGrades
{
    #region 淘宝接口实现

    /// <summary>
    /// 淘宝接口实现
    /// </summary>
    /// <remarks>2014-01-03 陶辉 创建</remarks>
    public class TaobaoUpGrade : IUpGrade
    {
        /// <summary>
        /// 淘宝接口配置信息
        /// </summary>
        private static readonly TaobaoConfig config = UpGradeConfig.GetTaobaoConfig();

        //基于REST的TOP客户端
        private static readonly ITopClient client = new DefaultTopClient(config.ApiUrl, config.AppKey, config.AppSecret, "json");

        #region 获取已升舱待发货订单
        /// <summary>
        /// 获取已升舱待发货订单
        /// </summary>
        /// <param name="param">条件参数</param>
        /// <param name="auth">授权参数</param>
        /// <returns>订单列表</returns>
        /// <reamrks>2014-04-08 黄波 创建</reamrks>
        public Result<List<UpGradeOrder>> GetUpGradedWaitSend(OrderParameters param, AuthorizationParameters auth)
        {
            var result = new Result<List<UpGradeOrder>> { Status = true, Message = "", Data = null, StatusCode = 0 };

            var order = new List<UpGradeOrder>();

            //获取所有待发货订单
            if (auth.MallType == (int)DistributionStatus.商城类型预定义.天猫商城)
            {
                result.Data = GetShopWaitSendOrder(param, auth);
            }
            else if (auth.MallType == (int)DistributionStatus.商城类型预定义.淘宝分销)
            {
                result.Data = GetAgentWaitSendOrder(param, auth);
            }
            else
            {
                result.Status = false;
                result.Message = "错误的商城类型!";
            }

            if (result.Data == null || result.Data.Count <= 0)
            {
                result.Status = false;
                result.Message = "没有查询到订单!";
            }

            ////筛选已升舱订单
            //if (result.Status)
            //{
            //    result.Data = order.FindAll(o => o.MallOrderBuyer.SellerFlag == config.ExcludeFlag.ToString());
            //}

            //关联商城订单状态

            return result;
        }
        #endregion

        #region 获取店铺等待发货的订单
        /// <summary>
        /// 获取店铺等待发货的订单
        /// </summary>
        /// <param name="param">条件参数</param>
        /// <param name="auth">授权参数</param>
        /// <returns>订单列表</returns>
        /// <reamrks>2014-04-08 黄波 创建</reamrks>
        private List<UpGradeOrder> GetShopWaitSendOrder(OrderParameters param, AuthorizationParameters auth)
        {
            var result = new List<UpGradeOrder>();

            //增量查询今日订单请求
            var req = new TradesSoldIncrementGetRequest
            {
                Fields =
                    "seller_flag,seller_nick,buyer_nick,alipay_no,has_buyer_message,title,tid,status,payment,discount_fee,post_fee,adjust_fee,pay_time,receiver_name,receiver_state,receiver_city,receiver_district,receiver_address,receiver_zip,receiver_mobile,receiver_phone,orders.title,orders.price,orders.num,orders.iid,orders.num_iid,orders.sku_id,orders.payment,orders.discount_fee,orders.sku_properties_name,orders.outer_iid,orders.outer_sku_id,orders.oid,orders.refund_status",
                Status = "WAIT_SELLER_SEND_GOODS",
                StartModified = param.StartDate,
                EndModified = param.EndDate,
                PageSize = 20L,
                UseHasNext = true
            };

            var nextPage = true;
            var pageNo = 1L;
            var response = new TradesSoldIncrementGetResponse();

            while (nextPage)
            {
                try
                {
                    req.PageNo = pageNo;
                    response = client.Execute(req, auth.AuthorizationCode);
                    if (!response.IsError)
                    {
                        if (response.HasNext)
                        {
                            pageNo++;
                        }
                        else
                        {
                            nextPage = false;
                        }
                        foreach (Trade trade in response.Trades)
                        {
                            var order = new UpGradeOrder();
                            Map(trade, order);
                            order.MallOrderBuyer.SellerFlag = trade.SellerFlag.ToString();
                            result.Add(order);
                        }
                    }
                    else { nextPage = false; }
                }
                catch { nextPage = false; }
            }

            return result;
        }
        #endregion

        #region 获取代销等待发货的订单
        /// <summary>
        /// 获取店铺等待发货的订单
        /// </summary>
        /// <param name="param">条件参数</param>
        /// <param name="auth">授权参数</param>
        /// <returns>订单列表</returns>
        /// <reamrks>2014-04-08 黄波 创建</reamrks>
        private List<UpGradeOrder> GetAgentWaitSendOrder(OrderParameters param, AuthorizationParameters auth)
        {
            //分销采购单请求
            var req = new FenxiaoOrdersGetRequest
            {
                Status = "WAIT_SELLER_SEND_GOODS",
                StartCreated = param.StartDate,
                EndCreated = param.EndDate,
                PageSize = 20L
            };
            //交易状态为待发货

            FenxiaoOrdersGetResponse response = client.Execute(req, auth.AuthorizationCode);

            //是否异常
            if (!response.IsError)
            {
                var list = new List<UpGradeOrder>();
                //总页数
                long pageCount = ((response.TotalResults + req.PageSize - 1) / req.PageSize).Value;
                while (pageCount > 0)
                {
                    //为避免漏单，先查最后一页
                    req.PageNo = pageCount;
                    if (response.PurchaseOrders != null)
                    {
                        //过滤查询条件
                        var purchaseOrders = FilterMallOrder(response.PurchaseOrders, param);
                        //筛选分销类型为代销的采购单
                        foreach (PurchaseOrder curchaseOrder in purchaseOrders.FindAll(o => o.TradeType == "AGENT"))
                        {
                            var order = new UpGradeOrder();
                            Map(curchaseOrder, order);
                            order.MallOrderBuyer.SellerFlag = curchaseOrder.SupplierFlag.ToString();
                            list.Add(order);
                        }
                        pageCount--;
                    }
                }

                return list;
            }
            else
            {
                return null;
            }
        }
        #endregion


        #region 批量获取指定时间区间的订单
        /// <summary>
        /// 批量获取指定时间区间的订单
        /// </summary>
        /// <param name="param">参数实体</param>
        /// <param name="auth">权限参数</param>
        /// <returns>订单列表</returns>
        /// <remarks>2103-8-27 陶辉 创建</remarks>
        public Result<List<UpGradeOrder>> GetOrderList(OrderParameters param, AuthorizationParameters auth)
        {
            if (auth.MallType == (int)DistributionStatus.商城类型预定义.天猫商城)
            {
                return GetMallOrderList(param, auth);
            }
            else if (auth.MallType == (int)DistributionStatus.商城类型预定义.淘宝分销)
            {
                return GetAgentOrderList(param, auth);
            }
            else
            {
                return new Result<List<UpGradeOrder>>()
                {
                    Status = false,
                    StatusCode = -1,
                    Message = "获取异常，请联系客服",
                    Data = null
                };
            }
        }
        #endregion

        #region 批量获取指定时间区间的天猫商城订单
        /// <summary>
        /// 批量获取指定时间区间的天猫商城订单
        /// </summary>
        /// <param name="param">参数实体</param>
        /// <param name="auth">权限参数</param>
        /// <returns>订单列表</returns>
        /// <remarks>2103-8-27 陶辉 创建</remarks>
        private Result<List<UpGradeOrder>> GetMallOrderList(OrderParameters param, AuthorizationParameters auth)
        {
            //增量查询今日订单请求
            var req = new TradesSoldIncrementGetRequest
            {
                Fields =
                    "seller_flag,seller_nick,buyer_nick,alipay_no,has_buyer_message,title,tid,status,payment,discount_fee,post_fee,adjust_fee,pay_time,receiver_name,receiver_state,receiver_city,receiver_district,receiver_address,receiver_zip,receiver_mobile,receiver_phone,orders.title,orders.price,orders.num,orders.iid,orders.num_iid,orders.sku_id,orders.payment,orders.discount_fee,orders.sku_properties_name,orders.outer_iid,orders.outer_sku_id,orders.oid,orders.refund_status,orders.total_fee",
                Status = "WAIT_SELLER_SEND_GOODS",
                StartModified = param.StartDate,
                EndModified = param.EndDate,
                PageSize = 20L,
                UseHasNext = false
            };

            var response = client.Execute(req, auth.AuthorizationCode);
            //是否异常
            if (!response.IsError)
            {
                var list = new List<UpGradeOrder>();
                //总页数
                var pageCount = ((response.TotalResults + req.PageSize - 1) / req.PageSize).Value;
                while (pageCount > 0)
                {
                    //为防止漏单，先查询最后一页
                    req.PageNo = pageCount;
                    req.UseHasNext = true;
                    response = client.Execute(req, auth.AuthorizationCode);
                    if (response.Trades != null && !response.IsError)
                    {
                        //过滤查询条件
                        var trades = FilterMallOrder(response.Trades, param);
                        foreach (Trade trade in trades)
                        {
                            var order = new UpGradeOrder();
                            Map(trade, order);
                            list.Add(order);
                        }
                        pageCount--;
                    }
                }

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
                    Message = GetErrorMsg(response.ErrCode, response.SubErrMsg + response.ErrMsg),
                    Data = null
                };
            }
        }
        #endregion

        #region 批量获取代销采购订单
        /// <summary>
        /// 批量获取代销采购订单
        /// </summary>
        /// <param name="param">参数实体</param>
        /// <param name="auth">权限参数</param>
        /// <returns>订单列表</returns>
        /// <remarks>2103-8-27 陶辉 创建</remarks>
        private Result<List<UpGradeOrder>> GetAgentOrderList(OrderParameters param, AuthorizationParameters auth)
        {
            //分销采购单请求
            var req = new FenxiaoOrdersGetRequest
            {
                Status = "WAIT_SELLER_SEND_GOODS",
                StartCreated = param.StartDate,
                EndCreated = param.EndDate,
                PageSize = 20L
            };
            //交易状态为待发货

            FenxiaoOrdersGetResponse response = client.Execute(req, auth.AuthorizationCode);

            //是否异常
            if (!response.IsError)
            {
                var list = new List<UpGradeOrder>();
                //总页数
                long pageCount = ((response.TotalResults + req.PageSize - 1) / req.PageSize).Value;
                while (pageCount > 0)
                {
                    //为避免漏单，先查最后一页
                    req.PageNo = pageCount;
                    if (response.PurchaseOrders != null)
                    {
                        //过滤查询条件
                        var purchaseOrders = FilterMallOrder(response.PurchaseOrders, param);
                        //筛选分销类型为代销的采购单
                        foreach (PurchaseOrder curchaseOrder in purchaseOrders.FindAll(o => o.TradeType == "AGENT"))
                        {
                            var order = new UpGradeOrder();
                            Map(curchaseOrder, order);
                            list.Add(order);
                        }
                        pageCount--;
                    }
                }

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
                    Message = GetErrorMsg(response.ErrCode, response.SubErrMsg + response.ErrMsg),
                    Data = null
                };
            }

        }
        #endregion

        #region 获取单笔订单详情
        /// <summary>
        /// 获取单笔订单详情
        /// </summary>
        /// <param name="param">参数实体</param>
        /// <param name="auth">权限参数</param>
        /// <returns>单笔订单详情</returns>
        /// <remarks>2103-8-27 陶辉 创建</remarks>
        public Result<UpGradeOrder> GetOrderDetail(OrderParameters param, AuthorizationParameters auth)
        {
            if (auth.MallType == (int)DistributionStatus.商城类型预定义.天猫商城)
            {
                return GetMallOrderDetail(param.OrderID, auth.AuthorizationCode);
            }
            else if (auth.MallType == (int)DistributionStatus.商城类型预定义.淘宝分销)
            {
                return GetAgentOrderDetail(param.OrderID, auth.AuthorizationCode);
            }
            else
            {
                return new Result<UpGradeOrder>()
                {
                    Status = false,
                    StatusCode = -1,
                    Message = "获取异常，请联系客服",
                    Data = null
                };
            }
        }
        #endregion

        #region 获取单笔天猫商城订单详情
        /// <summary>
        /// 获取单笔天猫商城订单详情
        /// </summary>
        /// <param name="orderId">订单编号数</param>
        /// <param name="authorizationcode">授权码</param>
        /// <returns>单笔订单详情</returns>
        /// <remarks>2103-8-27 陶辉 创建</remarks>
        private Result<UpGradeOrder> GetMallOrderDetail(string orderId, string authorizationcode)
        {

            //查询订单详情请求
            var req = new TradeFullinfoGetRequest
            {
                Fields =
                    "seller_flag,seller_nick,buyer_nick,alipay_no,buyer_message,seller_memo,title,tid,status,payment,discount_fee,post_fee,pay_time,receiver_name,receiver_state,receiver_city,receiver_district,receiver_address,receiver_zip,receiver_mobile,receiver_phone,orders.title,orders.price,orders.num,orders.iid,orders.num_iid,orders.sku_id,orders.payment,orders.total_fee,orders.discount_fee,orders.sku_properties_name,orders.outer_iid,orders.outer_sku_id,orders.oid",
                Tid = long.Parse(orderId)
            };
            //返回字段
            //订单号

            var response = client.Execute(req, authorizationcode);
            //是否异常
            if (!response.IsError)
            {
                var order = new UpGradeOrder();
                Map(response.Trade, order);

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
                    Message = GetErrorMsg(response.ErrCode, response.SubErrMsg + response.ErrMsg),
                    Data = null
                };
            }

        }
        #endregion

        #region 获取单笔代销采购订单信息
        /// <summary>
        /// 获取单笔代销采购订单信息
        /// </summary>
        /// <param name="orderid">主订单ID，即客户订单ID</param>
        /// <param name="authorizationcode">授权码</param>
        /// <returns>采购订单详情</returns>
        /// /// <remarks>2103-8-27 陶辉 创建</remarks>
        private Result<UpGradeOrder> GetAgentOrderDetail(string orderid, string authorizationcode)
        {
            //分销采购单请求
            var req = new FenxiaoOrdersGetRequest { Status = "WAIT_SELLER_SEND_GOODS", TcOrderId = long.Parse(orderid) };
            var response = client.Execute(req, authorizationcode);

            //是否异常
            if (!response.IsError)
            {
                var order = new UpGradeOrder();
                if (response.PurchaseOrders != null)
                {
                    Map(response.PurchaseOrders[0], order);
                }

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
                    Message = GetErrorMsg(response.ErrCode, response.SubErrMsg + response.ErrMsg),
                    Data = null
                };
            }
        }
        #endregion

        #region 用户调用该接口可实现自己联系发货（线下物流）
        /// <summary>
        /// 用户调用该接口可实现自己联系发货（线下物流）
        /// </summary>
        /// <param name="param">参数实体</param>
        ///<param name="auth">授权参数</param>
        /// <returns>处理结果</returns>
        /// <remarks>2103-8-27 陶辉 创建</remarks>
        public Result SendDelivery(DeliveryParameters param, AuthorizationParameters auth)
        {

            var req = new LogisticsOfflineSendRequest
            {
                Tid = long.Parse(param.MallOrderId),
                OutSid = param.HytExpressNo,
                CompanyCode = param.CompanyCode
            };
            LogisticsOfflineSendResponse offlineSendResponse = client.Execute(req, auth.AuthorizationCode);

            if (!offlineSendResponse.IsError)
            {
                if (offlineSendResponse.Shipping.IsSuccess)
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
                        Message = "发货异常，请重新操作"
                    };
                }
            }
            else
            {
                return new Result()
                {
                    Status = false,
                    StatusCode = -1,
                    Message = GetErrorMsg(offlineSendResponse.ErrCode, string.Format("OrderID:{6},ExpressNO:{7},CompanyCode:{8},Body:{0},ErrCode:{1},ErrMsg:{2},ReqUrl:{3},SubErrCode:{4},SubErrMsg:{5}",
                                        offlineSendResponse.Body,
                                        offlineSendResponse.ErrCode,
                                        offlineSendResponse.ErrMsg,
                                        "",
                                        offlineSendResponse.SubErrCode,
                                        offlineSendResponse.SubErrMsg,
                                        param.MallOrderId,
                                        param.HytExpressNo,
                                        param.CompanyCode
                                        )
                                        ),
                };
            }
        }
        #endregion

        #region 获取可合并升舱订单列表
        /// <summary>
        /// 获取可合并升舱订单列表
        /// </summary>
        /// <param name="param">参数实体</param>
        ///<param name="auth">授权参数</param>
        /// <returns>可合并升舱订单列表</returns>
        /// <remarks>2103-9-5 陶辉 创建</remarks>
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
        #endregion

        #region 使用授权码获取登录令牌
        /// <summary>
        /// 使用授权码获取登录令牌
        /// </summary>
        /// <param name="code">授权码</param>
        /// <returns>登录令牌</returns>
        /// <remarks>2013-9-9 陶辉 创建</remarks>
        public Result<AccessTokenResult> GetAuthorizationCode(string code)
        {
            IDictionary<string, string> dic = new Dictionary<string, string>();

            dic.Add("client_id", config.AppKey);
            dic.Add("client_secret", config.AppSecret);
            dic.Add("grant_type", "authorization_code");
            dic.Add("code", code);
            dic.Add("redirect_uri", config.TaobaoCallBack);
            dic.Add("view", "tmall");

            var objectJson = JObject.Parse(new Top.Api.Util.WebUtils().DoPost(config.AccessTokenUrl, dic));
            //var refreshtoken = string.Empty;
            AccessTokenResult data = null;
            if (objectJson.Property("refresh_token") != null)
            {
                data = new AccessTokenResult()
                {
                    AccessToken = objectJson["access_token"].ToString(),
                    UserNick = objectJson["taobao_user_nick"].ToString()
                };
            }

            //if (!string.IsNullOrEmpty(refreshtoken))
            //{
            //    dic.Clear();
            //    dic.Add("client_id", config.AppKey);
            //    dic.Add("client_secret", config.AppSecret);
            //    dic.Add("grant_type", "refresh_token");
            //    dic.Add("refresh_token", refreshtoken);
            //    dic.Add("redirect_uri", config.TaobaoCallBack);
            //    dic.Add("view", "tmall");

            //    var objJson = JObject.Parse(new Top.Api.Util.WebUtils().DoPost(config.AccessTokenUrl, dic));

            //    if (objJson.Property("access_token") != null)
            //    {
            //        data = new AccessTokenResult() {
            //            AccessToken = objJson["access_token"].ToString(),
            //            UserNick=objJson["taobao_user_nick"].ToString()
            //        };
            //    }
            //    //data = Hyt.Util.Serialization.JsonUtil.ToObject<AccessTokenResult>(new Top.Api.Util.WebUtils().DoPost(config.AccessTokenUrl, dic));
            //}

            return new Result<AccessTokenResult>()
            {
                Status = true,
                StatusCode = 1,
                Data = data
            };

        }
        #endregion

        #region 获取接口错误信息
        /// <summary>
        /// 获取接口错误信息
        /// </summary>
        /// <param name="code">错误编码</param>
        /// <param name="msg">错误信息</param>
        /// <returns>错误信息</returns>
        /// <remarks>2013-9-23 陶辉 创建</remarks>
        private string GetErrorMsg(string code, string msg)
        {
            switch (code)
            {
                case "26":
                case "53":
                    msg = "您的授权已过期，请退出后使用第三方账号登录重新授权。";
                    break;
                default:
                    break;
            }
            return msg;
        }
        #endregion

        #region MAP
        void Map(Trade trade, UpGradeOrder order)
        {
            //第三方买家订单信息
            order.MallOrderBuyer = new MallOrderBuyerInfo()
            {
                BuyerNick = trade.BuyerNick,
                MallOrderId = trade.Tid.ToString(),
                BuyerMessage = trade.BuyerMessage ?? string.Empty,
                SellerMessage = trade.SellerMemo ?? string.Empty
            };

            //订单明细列表
            order.UpGradeOrderItems = trade.Orders.Select(item => new UpGradeOrderItem()
            {
                MallProductCode = item.OuterSkuId ?? item.OuterIid ?? item.NumIid.ToString() ?? string.Empty,
                MallProductName = item.Title,
                MallProductAttrId = item.SkuId ?? string.Empty,
                MallProductAttrs = item.SkuPropertiesName ?? string.Empty,
                OrderId = trade.Tid.ToString(),
                MallPrice = decimal.Parse(trade.Payment ?? "0") == 0 ? 0 :
                                Math.Round((decimal.Parse(trade.Payment ?? "0")//实际支付金额
                                    - decimal.Parse(trade.PostFee ?? "0"))//运费
                                    / (decimal.Parse(trade.Payment ?? "0")
                                    - decimal.Parse(trade.PostFee ?? "0")
                                    - decimal.Parse(trade.AdjustFee ?? "0")//调整金额
                                    + decimal.Parse(trade.DiscountFee ?? "0")//折扣金额
                                    )//折扣金额
                                    * decimal.Parse(item.TotalFee ?? "0")//应付金额
                                    / int.Parse(item.Num.ToString())//数量
                                    , 2),
                MallAmount = decimal.Parse(trade.Payment ?? "0") == 0 ? 0 :
                                Math.Round((decimal.Parse(trade.Payment ?? "0")//实际支付金额
                                    - decimal.Parse(trade.PostFee ?? "0"))//运费
                                    / (decimal.Parse(trade.Payment ?? "0")
                                    - decimal.Parse(trade.PostFee ?? "0")
                                    - decimal.Parse(trade.AdjustFee ?? "0")//调整金额
                                    + decimal.Parse(trade.DiscountFee ?? "0")//折扣金额
                                    )//折扣金额
                                    * decimal.Parse(item.TotalFee ?? "0")//应付金额
                                    , 2),
                Quantity = int.Parse(item.Num.ToString()),
                DiscountFee = decimal.Parse(item.DiscountFee),
                MallOrderItemId = item.Oid.ToString()
            }).ToList();

            //第三方订单交易信息
            order.MallOrderPayment = new MallOrderPaymentInfo()
            {
                Payment = decimal.Parse(trade.Payment ?? "0"),
                AlipayNo = trade.AlipayNo ?? string.Empty,
                PostFee = decimal.Parse(trade.PostFee ?? "0"),
                DiscountFee = decimal.Parse(trade.DiscountFee ?? "0"),
                PayTime = DateTime.Parse(trade.PayTime ?? "1900-01-01")
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
                City = trade.ReceiverCity ?? string.Empty,
                Province = trade.ReceiverState ?? string.Empty,
                District = trade.ReceiverDistrict ?? string.Empty,
                ReceiveAddress = trade.ReceiverAddress ?? string.Empty,
                ReceiveContact = trade.ReceiverName ?? string.Empty,
                TelPhone = trade.ReceiverPhone ?? string.Empty,
                Mobile = trade.ReceiverMobile ?? string.Empty,
                PostCode = trade.ReceiverZip ?? string.Empty
            };

            //订单备注标示
            order.MallOrderBuyer.SellerFlag = trade.SellerFlag.ToString();
        }
        #endregion

        #region MAP
        void Map(PurchaseOrder purchaseOrder, UpGradeOrder order)
        {
            //第三方买家订单信息
            order.MallOrderBuyer = new MallOrderBuyerInfo()
            {
                BuyerNick = purchaseOrder.DistributorUsername ?? string.Empty,
                MallOrderId = purchaseOrder.TcOrderId.ToString(),
                MallPurchaseId = purchaseOrder.Id.ToString(),
                BuyerMessage = purchaseOrder.Memo ?? string.Empty,
                SellerMessage = purchaseOrder.SupplierMemo
            };

            //订单明细列表
            order.UpGradeOrderItems = purchaseOrder.SubPurchaseOrders.Select(item => new UpGradeOrderItem()
            {
                MallProductCode = item.SkuOuterId ?? item.ItemOuterId ?? string.Empty,
                MallProductName = item.Title ?? string.Empty,
                MallProductAttrId = item.SkuId.ToString(),
                MallProductAttrs = item.SkuProperties ?? string.Empty,
                OrderId = purchaseOrder.TcOrderId.ToString(),
                MallPrice = Math.Round(decimal.Parse(item.DistributorPayment ?? "0") / item.Num, 2),
                MallAmount = decimal.Parse(item.DistributorPayment ?? "0"),
                Quantity = int.Parse(item.Num.ToString()),
                DiscountFee = decimal.Parse((item.TcDiscountFee / 100).ToString()),
                MallOrderItemId = item.FenxiaoId.ToString()
            }).ToList();

            //第三方订单交易信息

            order.MallOrderPayment = new MallOrderPaymentInfo()
            {
                Payment = decimal.Parse(purchaseOrder.DistributorPayment ?? "0"),
                AlipayNo = purchaseOrder.AlipayNo ?? string.Empty,
                PostFee = decimal.Parse(purchaseOrder.PostFee ?? "0"),
                DiscountFee = decimal.Parse(purchaseOrder.SubPurchaseOrders.Sum(o => o.TcDiscountFee / 100).ToString()),
                PayTime = DateTime.Parse(purchaseOrder.PayTime ?? "2000-00-00 00:00:00")
            };

            //订单收货信息
            order.MallOrderReceive = new MallOrderReceiveInfo()
            {
                City = purchaseOrder.Receiver.City ?? string.Empty,
                Province = purchaseOrder.Receiver.State ?? string.Empty,
                District = purchaseOrder.Receiver.District ?? string.Empty,
                ReceiveAddress = purchaseOrder.Receiver.Address ?? string.Empty,
                ReceiveContact = purchaseOrder.Receiver.Name ?? string.Empty,
                TelPhone = purchaseOrder.Receiver.Phone ?? string.Empty,
                Mobile = purchaseOrder.Receiver.MobilePhone ?? string.Empty,
                PostCode = purchaseOrder.Receiver.Zip ?? string.Empty
            };
            //订单备注标示
            order.MallOrderBuyer.SellerFlag = purchaseOrder.SupplierFlag.ToString();
        }
        #endregion

        #region 店铺订单筛选
        //天猫
        static IEnumerable<Trade> FilterMallOrder(List<Trade> list, OrderParameters param)
        {
            //过滤已升舱的订单
            list = list.Where(o => o.SellerFlag != config.ExcludeFlag).ToList();

            //过滤优化促销订单
            list = list.Where(o => o.SellerFlag != (long)FlagType.黄色旗帜).ToList();
            list = list.Where(o => {
                if (!string.IsNullOrWhiteSpace(o.SellerMemo) && o.SellerMemo.ToLower().IndexOf("yhcx") > -1) return  false;
                if (!string.IsNullOrWhiteSpace(o.BuyerMemo) && o.BuyerMemo.ToLower().IndexOf("yhcx") > -1) return false;
                return true;

            }).ToList();
                
                //(!string.IsNullOrWhiteSpace(o.SellerMemo) && o.SellerMemo.ToLower().IndexOf("yhcx") < 0) && (!string.IsNullOrWhiteSpace(o.BuyerMemo) && o.BuyerMemo.ToLower().IndexOf("yhcx") < 0)).ToList();

            //筛选订单旗帜为升舱标识的订单和属性行包含"升舱"的
            if (param.IsUseFlag)
            {
                //list = list.Where(o => o.SellerFlag == AppConfig.Instance.GetTopConfig().SellerFlag).ToList();
                list = list.Where(o => o.Orders.Any(p => (p.SkuPropertiesName != null && p.SkuPropertiesName.IndexOf("升舱") > -1)) || o.SellerFlag == config.SellerFlag).ToList();
            }

            //订单号
            if (!string.IsNullOrEmpty(param.OrderID))
                list = list.Where(o => o.Tid.ToString() == param.OrderID).ToList();

            //昵称筛选
            if (!string.IsNullOrEmpty(param.BuyerNick))
                list = list.Where(o => o.BuyerNick == param.BuyerNick).ToList();

            //是有留言
            if (param.HasMessage == (int)DistributionStatus.买家是否备注.是)
                list = list.Where(o => o.HasBuyerMessage).ToList();

            //买家没留言
            if (param.HasMessage == (int)DistributionStatus.买家是否备注.否)
                list = list.Where(o => !o.HasBuyerMessage).ToList();

            //商品编码
            if (!string.IsNullOrEmpty(param.ProductCode))
                list = list.Where(o => o.Orders.Any(p => (p.OuterSkuId ?? p.OuterIid ?? p.NumIid.ToString()) == param.ProductCode)).ToList();

            //商品名称
            if (!string.IsNullOrEmpty(param.ProductName))
                list = list.Where(o => o.Orders.Any(p => p.Title.IndexOf(param.ProductName) != -1)).ToList();

            //是否海外地区(余勇 2014-07-03)
            if (param.IsAbroad)
            {
                if (!string.IsNullOrEmpty(param.AbroadAreaName))
                    //是否为指定的海外地区
                    list = list.Where(o => o.ReceiverState == param.AbroadAreaName).ToList();
                else
                    //是否海外地区
                    list = list.Where(o => AbroadArea.Areas.Contains(o.ReceiverState)).ToList();
            }
            else
            {
                //不包含任何海外地区
                list = list.Where(o => !AbroadArea.Areas.Contains(o.ReceiverState)).ToList();
            }

            //过滤已申请退款的
            var l = list.Where(item => !item.Orders.Any(p => p.RefundStatus != "NO_REFUND" && p.RefundStatus != "CLOSED")).ToList();
            l = (from o in l

                 orderby o.PayTime ascending
                 select o).ToList();

            return l;
        }
        #endregion

        #region 分销订单筛选
        //分销
        static List<PurchaseOrder> FilterMallOrder(List<PurchaseOrder> list, OrderParameters param)
        {
            //过滤已升舱的订单
            list = list.Where(o => o.SupplierFlag != config.ExcludeFlag).ToList();

            //过滤优化促销订单
            list = list.Where(o => o.SupplierFlag != (long)FlagType.黄色旗帜).ToList();
            list = list.Where(o =>
            {
                if (!string.IsNullOrWhiteSpace(o.SupplierMemo) && o.SupplierMemo.ToLower().IndexOf("yhcx") > -1) return false;
                if (!string.IsNullOrWhiteSpace(o.Memo) && o.Memo.ToLower().IndexOf("yhcx") > -1) return false;
                return true;

            }).ToList();
            //list = list.Where(o => (!string.IsNullOrWhiteSpace(o.SupplierMemo) && o.SupplierMemo.ToLower().IndexOf("yhcx") < 0) && (!string.IsNullOrWhiteSpace(o.Memo) && o.Memo.ToLower().IndexOf("yhcx") < 0)).ToList();

            //筛选订单旗帜为升舱标识的订单
            if (param.IsUseFlag)
            {
                list = list.Where(o => o.SupplierFlag == config.SellerFlag).ToList();
            }

            //订单号
            if (!string.IsNullOrEmpty(param.OrderID))
                list = list.Where(o => o.TcOrderId.ToString() == param.OrderID).ToList();

            //昵称筛选
            if (!string.IsNullOrEmpty(param.BuyerNick))
                list = list.Where(o => o.BuyerNick == param.BuyerNick).ToList();

            //是有留言
            if (param.HasMessage == (int)DistributionStatus.买家是否备注.是)
                list = list.Where(o => o.Memo != string.Empty).ToList();

            //买家没留言
            if (param.HasMessage == (int)DistributionStatus.买家是否备注.否)
                list = list.Where(o => o.Memo == string.Empty).ToList();

            //商品编码
            if (!string.IsNullOrEmpty(param.ProductCode))
                list = list.Where(o => o.SubPurchaseOrders.Any(p => (p.SkuOuterId ?? p.ItemOuterId) == param.ProductCode)).ToList();

            //商品名称
            if (!string.IsNullOrEmpty(param.ProductName))
                list = list.Where(o => o.SubPurchaseOrders.Any(p => p.Title.IndexOf(param.ProductName) != -1)).ToList();

            //过滤已申请退款的
            var l = list.Where(item => !item.SubPurchaseOrders.Any(p => !string.IsNullOrEmpty(p.RefundFee) && decimal.Parse(p.RefundFee) > 0)).ToList();

            //是否海外地区(余勇 2014-07-03)
            if (param.IsAbroad)
            {
                if (!string.IsNullOrEmpty(param.AbroadAreaName))
                    //是否为指定的海外地区
                    list = list.Where(o => o.Receiver.State == param.AbroadAreaName).ToList();
                else
                    //是否海外地区
                    list = list.Where(o => AbroadArea.Areas.Contains(o.Receiver.State)).ToList();
            }
            else
            {
                //不包含任何海外地区
                list = list.Where(o => !AbroadArea.Areas.Contains(o.Receiver.State)).ToList();
            }

            l = (from o in l
                 orderby o.PayTime ascending
                 select o).ToList();

            return l;
        }
        #endregion

        #region 更新订单备注信息
        /// <summary>
        /// 更新订单备注信息
        /// </summary>
        /// <param name="remarks">备注内容</param>
        /// <param name="auth">授权参数</param>
        /// <returns>2014-03-25</returns>
        public Result UpdateTradeRemarks(IRemarksParameters remarks, AuthorizationParameters auth)
        {
            var result = new Result();

            var taobaoRemarks = (TaobaoRemarksParameters)remarks;

            if (auth.MallType == (int)DistributionStatus.商城类型预定义.淘宝分销)
            {
                var fenxiaoReq = new FenxiaoOrderRemarkUpdateRequest()
                {
                    PurchaseOrderId = long.Parse(taobaoRemarks.MallOrderId),
                    SupplierMemoFlag = (int)taobaoRemarks.Flag,
                    SupplierMemo = string.Format("{0}.{1}SC.", taobaoRemarks.RemarksContent, DateTime.Now.ToString(), "SC.")
                };

                var response = client.Execute(fenxiaoReq, auth.AuthorizationCode);

                if (!response.IsError)
                {
                    result.Message = "修改备注成功!";
                    result.Status = true;
                    result.StatusCode = 1;
                }
                else
                {
                    result.Message = "修改备注失败,请重试!";
                    result.Status = false;
                    result.StatusCode = 0;
                }
            }
            else if (auth.MallType == (int)DistributionStatus.商城类型预定义.天猫商城)
            {
                var req = new TradeMemoUpdateRequest
                {
                    Flag = (int)taobaoRemarks.Flag,
                    Memo = string.Format("{0}.{1}SC.", taobaoRemarks.RemarksContent, DateTime.Now.ToString(), "SC."),
                    Reset = taobaoRemarks.Reset,
                    Tid = long.Parse(taobaoRemarks.MallOrderId)
                };

                var response = client.Execute(req, auth.AuthorizationCode);

                if (!response.IsError)
                {
                    result.Message = "修改备注成功!";
                    result.Status = true;
                    result.StatusCode = 1;
                }
                else
                {
                    result.Message = "修改备注失败,请重试!";
                    result.Status = false;
                    result.StatusCode = 0;
                }
            }
            return result;
        }
        #endregion


        public Result<UpGradeExpress> GetExpress(AuthorizationParameters auth)
        {
            throw new NotImplementedException();
        }
    }

    #endregion
}
