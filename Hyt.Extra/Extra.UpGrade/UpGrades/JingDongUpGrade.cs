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
using Extra.UpGrade.SDK.JingDong.Request;
using Extra.UpGrade.SDK.JingDong;
using Extra.UpGrade.SDK.JingDong.Response;

namespace Extra.UpGrade.UpGrades
{
    #region 京东订单接口实现
    /// <summary>
    /// 京东订单接口实现
    /// </summary>
    /// <remarks>2017-08-11 黄杰 创建</remarks>
    public class JingDongUpGrade : IUpGrade
    {
        /// <summary>
        /// 京东订单接口配置信息
        /// </summary>
        private static readonly JingDongConfig config = UpGradeConfig.GetJingDongConfig();

        //基于REST的TOP客户端
        private static readonly IJdClient client = new DefaultJdClient(config.ApiUrl, config.AppKey, config.AppSecret);


        #region 京东支付类型转为系统支付类型
        /// <summary>
        /// 京东支付类型转为系统支付类型
        /// </summary>
        /// <param name="payType">京东支付类型</param>
        /// <returns></returns>
        /// <reamrks>2017-08-16 黄杰 创建</reamrks>
        public int JingDongToPayTypeSysNo(string payType)
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

        #region 获取已升舱待发货订单
        /// <summary>
        /// 获取已升舱待发货订单
        /// </summary>
        /// <param name="param">条件参数</param>
        /// <param name="auth">授权参数</param>
        /// <returns>订单列表</returns>
        /// <reamrks>2017-08-16 黄杰 创建</reamrks>
        public Result<List<UpGradeOrder>> GetUpGradedWaitSend(OrderParameters param, AuthorizationParameters auth)
        {

            return null;
        }
        #endregion

        #region 批量获取指定时间区间的订单

        /// <summary>
        /// 批量获取指定时间区间的订单
        /// (待升舱的订单)
        /// </summary>
        /// <param name="param">参数实体</param>
        /// <param name="auth">授权参数</param>
        /// <returns>订单列表</returns>
        /// <reamrks>2017-08-16 黄杰 创建</reamrks>
        public Result<List<UpGradeOrder>> GetOrderList(OrderParameters param, AuthorizationParameters auth)
        {

            var dealerInfo = new HytOrderDealerInfo()
            {
                DealerSysNo = 2323,
                DealerMallSysNo = 17,
                HytPayStatus = 20,
                IsSelfSupport = 1,
            };

            var result = new Result<List<UpGradeOrder>> { Status = true, Message = "", Data = null, StatusCode = 0 };
            var orderList = new List<UpGradeOrder>();
            var _param = new Dictionary<string, string>();

            //创建PopOrderSearchRequest对象
            var req = new PopOrderSearchRequest();
            //多订单状态
            req.orderState = "WAIT_SELLER_STOCK_OUT";
            req.orderState = "TRADE_CANCELED";
            //需返回的字段列表
            req.optionalFields = "orderId,venderId,orderType,payType,orderTotalPrice,orderSellerPrice,orderPayment,freightPrice,sellerDiscount,orderState,orderStateRemark,deliveryType,invoiceInfo,invoiceCode,orderRemark,orderStartTime,orderEndTime,consigneeInfo,itemInfoList,couponDetailList,venderRemark,balanceUsed,pin,returnOrder,paymentConfirmTime,waybill,logisticsId,vatInfo,modified,directParentOrderId,parentOrderId,customs,customsModel,orderSource,storeOrder,idSopShipmenttype,scDT,serviceFee,pauseBizInfo,taxFee,tuiHuoWuYou,storeId";

            //查询的页数
            req.page = param.PageIndex.ToString();
            //每页的条数（最大page_size 100条）
            req.pageSize = param.PageSize.ToString();

            var response = client.Execute(req, config.AccessToken, DateTime.Now.ToLocalTime());
            param.PageIndex = 1;

            if (response.searchorderinfoResult == null)
                return result;

            while (true)
            {

                var trade = JObject.Parse(response.searchorderinfoResult);

                int total = int.Parse(trade["orderTotal"].ToString());

                int totalPage = total / param.PageSize;
                if (total % param.PageSize != 0)
                    totalPage++;

                var trades = trade["orderInfoList"];

                bool has_next = param.PageIndex < totalPage;

                //trade = JObject.Parse(response.ToString());
                foreach (var i in trades)
                {
                    var order = new UpGradeOrder();

                    Map(i, order);
                    order.HytOrderDealer = dealerInfo;
                    // 支付方式（1货到付款, 2邮局汇款, 3自提, 4在线支付, 5公司转账, 6银行卡转账）  
                    order.HytOrderDealer.HytPayType = JingDongToPayTypeSysNo(i["payType"].ToString());
                    orderList.Add(order);
                }

                if (has_next)
                {
                    param.PageIndex++;
                    req.page = param.PageIndex.ToString();
                    response = client.Execute(req, auth.AuthorizationCode, DateTime.Now.ToLocalTime());
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
                BuyerNick = trade["pin"].ToString(),
                MallOrderId = trade["orderId"].ToString(),
                BuyerMessage = trade["orderRemark"].ToString(),
                SellerMessage = trade["venderRemark"].ToString()
            };

            order.UpGradeOrderItems = new List<UpGradeOrderItem>();
            var orders = trade["itemInfoList"];

            for (int i = 0; i < orders.Count(); i++)
            {
                var item = orders[i];
                var code = item["outerSkuId"].ToString();
                order.UpGradeOrderItems.Add(new UpGradeOrderItem()
                {
                    MallProductCode = code,
                    MallProductName = item["skuName"].ToString(),
                    MallProductAttrId = item["skuId"].ToString(),
                    MallProductAttrs = item["skuName"].ToString(),
                    OrderId = trade["orderId"].ToString(),
                    MallPrice = decimal.Parse(item["jdPrice"].ToString()),
                    MallAmount = decimal.Parse(item["jdPrice"].ToString()),
                    Quantity = int.Parse(item["itemTotal"].ToString()),
                    DiscountFee = 0,// decimal.Parse(item["discount_fee"].ToString()),
                    MallOrderItemId = item["skuId"].ToString()
                });

            }


            decimal discountFee = 0;
            if (trade["coupondetail"] != null)
            {
                foreach (var item in trade["coupondetail"])
                {
                    discountFee += decimal.Parse(item["couponPrice"].ToString());
                }
            }

            //第三方订单交易信息
            order.MallOrderPayment = new MallOrderPaymentInfo()
            {
                Payment = decimal.Parse(trade["orderPayment"].ToString()),
                AlipayNo = "",
                PostFee = decimal.Parse(trade["freightPrice"].ToString()),
                DiscountFee = discountFee,
                PayTime = DateTime.Parse(trade["paymentConfirmTime"].ToString() == "0001-01-01 00:00:00" ? "1900-01-01" : trade["paymentConfirmTime"].ToString())
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
                City = trade["consigneeInfo"]["city"].ToString(),
                Province = trade["consigneeInfo"]["province"].ToString(),
                District = trade["consigneeInfo"]["county"].ToString(),
                ReceiveAddress = trade["consigneeInfo"]["fullAddress"].ToString(),
                ReceiveContact = trade["consigneeInfo"]["fullname"].ToString(),
                TelPhone = trade["consigneeInfo"]["telephone"].ToString(),
                Mobile = trade["consigneeInfo"]["mobile"].ToString(),
                PostCode = "",
            };


            //订单备注标示
            //order.MallOrderBuyer.SellerFlag = trade["seller_flag"].ToString();
        }
        #endregion

        #region 获取单笔订单详情
        /// <summary>
        /// 获取单笔订单详情
        /// </summary>
        /// <param name="param">参数实体</param>
        /// <param name="auth">授权参数</param>
        /// <returns>单笔订单详情</returns>
        /// <reamrks>2017-08-16 黄杰 创建</reamrks>
        public Result<UpGradeOrder> GetOrderDetail(OrderParameters param, AuthorizationParameters auth)
        {

            var result = new Result<UpGradeOrder> { Status = true, Message = "", Data = null, StatusCode = 0 };
            var req = new PopOrderGetRequest();
            req.orderId = param.OrderID;
            //需返回的字段列表
            req.optionalFields = "orderId,venderId,orderType,payType,orderTotalPrice,orderSellerPrice,orderPayment,freightPrice,sellerDiscount,orderState,orderStateRemark,deliveryType,invoiceInfo,invoiceCode,orderRemark,orderStartTime,orderEndTime,consigneeInfo,itemInfoList,couponDetailList,venderRemark,balanceUsed,pin,returnOrder,paymentConfirmTime,waybill,logisticsId,vatInfo,modified,directParentOrderId,parentOrderId,customs,customsModel,orderSource,storeOrder,idSopShipmenttype,scDT,serviceFee,pauseBizInfo,taxFee,tuiHuoWuYou,storeId";

            var response = client.Execute(req, config.AccessToken, DateTime.Now.ToLocalTime());

            var order = new UpGradeOrder();
            var trade = JObject.Parse(response.orderDetailInfo);
            Map(trade["orderInfo"], order);
            order.HytOrderDealer = new HytOrderDealerInfo();
            // 支付方式（1货到付款, 2邮局汇款, 3自提, 4在线支付, 5公司转账, 6银行卡转账）  
            order.HytOrderDealer.HytPayType = JingDongToPayTypeSysNo(trade["orderInfo"]["payType"].ToString());

            result.Data = order;
            return result;
        }
        #endregion

        #region 京东出库、联系发货（线下物流）
        /// <summary>
        /// 用户调用该接口可实现自己联系发货（线下物流）
        /// </summary>
        /// <param name="param">参数实体</param>
        /// <param name="auth">授权参数</param>
        /// <returns>处理结果</returns>
        /// <reamrks>2017-08-16 黄杰 创建</reamrks>
        public Result SendDelivery(DeliveryParameters param, AuthorizationParameters auth)
        {

            var result = new Result() { Status = true };
            var _param = new Dictionary<string, string>();

            #region 海外购订单出库
            //海外购订单出库
            var req = new OverseasOrderSopOutstorageRequest();
            //订单ID
            req.orderId = param.MallOrderId;
            //流水号
            req.tradeNo = "";

            var response = client.Execute(req,auth.AuthorizationCode,DateTime.Now.ToLocalTime());

            var _response = JObject.Parse(response.Body);

            #endregion

            //返回的错误,如果返回的结果为false的话，则进入这里面
            if (_response.Property("error_response") != null && _response["error_response"]["code"].ToString() != "10400001")
            {
               /*{
  "error_response": {
    "code": "10400001",
    "zh_desc": "63883901756订单已出库",
    "en_desc": "orders have been out of library"
  }
}*/
                result.Status = false;
                result.Message = _response["error_response"]["zh_desc"].ToString() + "|code=" + _response["error_response"]["code"].ToString();
                result.StatusCode = int.Parse(_response["error_response"]["code"].ToString());
            }
            else
            {
                #region 海外购订单发货
                #region 京东快递获取
                /*
                 * {"logistics_list":[
                 * 
                 * {"sequence":"3","logistics_id":463,"logistics_name":"圆通快递", "logistics_remark":"","agree_flag":"协议物流","is_cod":false},
                 * {"sequence":"6","logistics_id":465,"logistics_name":"邮政EMS","logistics_remark":"","agree_flag":"协议物流","is_cod":false},
                 * {"sequence":"8","logistics_id":467,"logistics_name":"顺丰快递","logistics_remark":"","agree_flag":"协议物流","is_cod":false},
                 * {"sequence":"9","logistics_id":1274,"logistics_name":"厂家自送","logistics_remark":"百世快递","agree_flag":"协议物流","is_cod":false},
                 * {"sequence":"11","logistics_id":1327,"logistics_name":"韵达快递","logistics_remark":"韵达物流","agree_flag":"协议物流","is_cod":false},
                 * {"sequence":"1","logistics_id":1499,"logistics_name":"中通速递","logistics_remark":"","agree_flag":"协议物流","is_cod":false},
                 * {"sequence":"2","logistics_id":1747,"logistics_name":"优速快递","logistics_remark":"","agree_flag":"协议物流","is_cod":false},
                 * {"sequence":"12","logistics_id":2094,"logistics_name":"快捷速递","logistics_remark":"新的快递公司","agree_flag":"协议物流","is_cod":false},
                 * {"sequence":"4","logistics_id":2170,"logistics_name":"邮政快递包裹","logistics_remark":"","agree_flag":"协议物流","is_cod":false},
                 * {"sequence":"7","logistics_id":313214,"logistics_name":"如风达","logistics_remark":"DHL德国邮政","agree_flag":"协议物流","is_cod":false},
                 * {"sequence":"13","logistics_id":332098,"logistics_name":"用户自提","logistics_remark":"百世汇通","agree_flag":"协议物流","is_cod":false}
                 * 
                 * ] ,"vender_id":202210}
                 * 
                 */
                //var logisticsReqs = new DeliveryLogisticsGetRequest();

                //var _res = client.Execute(logisticsReqs,auth.AuthorizationCode, DateTime.Now.ToLocalTime());
                #endregion

                //海外购订单发货
                var reqs = new OverseasOrderSopDeliveryRequest();
                //订单ID
                reqs.orderId = req.orderId;
                //物流公司ID
                reqs.logisticsId = param.CompanyCode;
                //运单号
                reqs.waybill = param.HytExpressNo;
                //流水号
                reqs.tradeNo = "";

                var responses = client.Execute(reqs,auth.AuthorizationCode, DateTime.Now.ToLocalTime());

                var _responses = JObject.Parse(responses.Body);

                #endregion

                //返回的错误
                if (_responses.Property("error_response") != null)
                {
                    string code=_response["error_response"]["code"].ToString();
                    if (code == "10400001")//已发货                  
                        result.Status = true;
                    else
                        result.Status = false;
                    result.Message = _responses["error_response"]["zh_desc"].ToString() + "|code=" + code;
                    result.StatusCode = int.Parse(code);
                }
            }
            return result;
        }
        #endregion

        #region 获取可合并升舱订单列表
        /// <summary>
        /// 获取可合并升舱订单列表
        /// </summary>
        /// <param name="param">参数实体</param>    
        /// <param name="auth">授权参数</param>
        /// <returns>可合并升舱订单列表</returns>
        /// <reamrks>2017-08-16 黄杰 创建</reamrks> 
        public Result<List<UpGradeOrder>> GetCombineOrders(OrderParameters param, AuthorizationParameters auth)
        {
            return null;
        }
        #endregion

        #region 使用授权码获取登录令牌
        /// <summary>
        /// 使用授权码获取登录令牌
        /// </summary>
        /// <param name="code">授权码</param>
        /// <returns>登录令牌</returns>
        /// <reamrks>
        /// 2017-08-16 黄杰 创建
        /// 2017-11-1 杨浩 重构
        /// </reamrks>
        public Result<AccessTokenResult> GetAuthorizationCode(string code)
        {
            var result=new Result<AccessTokenResult>();
            var dic = new Dictionary<string, string>();

            var headers = System.Web.HttpContext.Current.Request.Headers;
            if (code == null)
            {
                var _form= System.Web.HttpContext.Current.Request.Form;                              
                dic.Add("response_type","code");
                dic.Add("client_id", headers.Get("AppKey"));//访问参数    
                dic.Add("redirect_uri", config.JingDongCallBack);
                dic.Add("state", Convert.ToString(_form["shopid"]));
                result.Message= new Extra.UpGrade.Api.WebUtils().BuildGetUrl(config.AuthorizeUrl, dic);
                result.Status = true;
                return result;
            }


            dic.Add("client_id", headers.Get("AppKey"));
            dic.Add("client_secret",headers.Get("AppSecret"));
            dic.Add("grant_type", "authorization_code");
            dic.Add("code", code);
            dic.Add("redirect_uri", config.JingDongCallBack);
            dic.Add("state", "1212");

            string requestStr = new Extra.UpGrade.Api.WebUtils().DoPost(config.AccessTokenUrl, dic);

            var objectJson =JObject.Parse(requestStr);

         
            if (objectJson.Property("refresh_token") != null)
            {         
                result.Data = new AccessTokenResult()
                {
                    AccessToken = objectJson["access_token"].ToString(),
                };
                result.Status = true;
            }
            else
            {
                result.Status = false;
                result.Message = requestStr;
            }

            return result;
            
        }
        #endregion

        #region 更新订单备注信息
        /// <summary>
        /// 更新订单备注信息
        /// </summary>
        /// <param name="remarks">备注内容</param>
        /// <param name="auth">授权参数</param>
        /// <reamrks>2017-08-16 黄杰 创建</reamrks>
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
