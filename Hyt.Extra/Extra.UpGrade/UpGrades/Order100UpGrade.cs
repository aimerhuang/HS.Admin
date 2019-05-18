using Extra.UpGrade.Model;
using Extra.UpGrade.Provider;
using Hyt.Model;
using Hyt.Model.UpGrade;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.UpGrade.UpGrades
{
    /// <summary>
    /// 订单100接口实现
    /// </summary>
    /// <remarks>2017-2-27 杨浩 创建</remarks>
    public class Order100UpGrade : IUpGrade
    {
        /// <summary>
        /// 获取已升舱待发货订单
        /// </summary>
        /// <param name="param">条件参数</param>
        /// <param name="auth">授权参数</param>
        /// <returns>订单列表</returns>
        /// <reamrks>2016-6-11 杨浩 创建</reamrks>
        public Result<List<UpGradeOrder>> GetUpGradedWaitSend(OrderParameters param, AuthorizationParameters auth)
        {

            return null;
        }

        /// <summary>
        /// 批量获取指定时间区间的订单
        /// (待升舱的订单)
        /// </summary>
        /// <param name="param">参数实体</param>
        /// <param name="auth">授权参数</param>
        /// <returns>订单列表</returns>
        /// <remarks>2016-6-11 杨浩 创建</remarks>
        public Result<List<UpGradeOrder>> GetOrderList(OrderParameters param, AuthorizationParameters auth)
        {
            var dealerInfo = new HytOrderDealerInfo()
            {
                DealerSysNo = 0,
                DealerMallSysNo = 5,
                HytPayStatus = 20,
                IsSelfSupport = 1,
            };
            var result = new Result<List<UpGradeOrder>> { Status = true, Message = "", Data = null, StatusCode = 0 };
            var orderList = new List<UpGradeOrder>();
            var _param = new Dictionary<string, string>();
            //_param.Add("fields", "tid,title,receiver_city,outer_tid,orders");
            _param.Add("status", "WAIT_SELLER_SEND_GOODS");//等待卖家发货，即：买家已付款

            _param.Add("use_has_next", "true");
            _param.Add("page_size", "40");
            _param.Add("page_no", "1");

            _param.Add("start_update", param.StartDate.ToString("yyyy-MM-dd HH:mm:ss"));
            _param.Add("end_update", param.EndDate.ToString("yyyy-MM-dd HH:mm:ss"));
            //_param.Add("start_created", param.StartDate.ToString("yyyy-MM-dd HH:mm:ss"));
            //_param.Add("end_created", param.EndDate.ToString("yyyy-MM-dd HH:mm:ss"));

            //var response = kit.get("kdt.trades.sold.get", _param);
            //while (true)
            //{
            //    var trade = JObject.Parse(response);
            //    var trades = trade["response"]["trades"];
            //    bool has_next = bool.Parse(trade["response"]["has_next"].ToString());

            //    trade = JObject.Parse(response);
            //    foreach (var i in trades)
            //    {
            //        var order = new UpGradeOrder();

            //        Map(i, order);
            //        order.HytOrderDealer = dealerInfo;
            //        //order.HytOrderDealer.HytPayType = YouZanPayToPayTypeSysNo(i["pay_type"].ToString());
            //        orderList.Add(order);
            //    }

            //    if (has_next)
            //    {
            //        _param["page_no"] = (int.Parse(_param["page_no"].ToString()) + 1).ToString();
            //        //response = kit.get("kdt.trades.sold.get", _param);
            //    }
            //    else
            //    {
            //        break;
            //    }
            //}

            //result.Data = orderList;
            return result;
        }
        #region MAP
        void Map(JToken trade, UpGradeOrder order)
        {
            //第三方买家订单信息
            order.MallOrderBuyer = new MallOrderBuyerInfo()
            {
                BuyerNick = trade["buyer_nick"].ToString(),
                MallOrderId = trade["tid"].ToString(),
                BuyerMessage = trade["buyer_message"].ToString(),
                SellerMessage = trade["trade_memo"].ToString()
            };

            order.UpGradeOrderItems = new List<UpGradeOrderItem>();
            var orders = trade["orders"];

            for (int i = 0; i < orders.Count(); i++)
            {


                var item = orders[i];
                var code = item["outer_sku_id"].ToString().Trim('\t').Trim() == "" ? item["outer_item_id"].ToString().Trim('\t').Trim() : item["outer_sku_id"].ToString().Trim('\t').Trim();
                code = code.Replace(" ", "");
                order.UpGradeOrderItems.Add(new UpGradeOrderItem()
                {
                    MallProductCode = code,
                    MallProductName = item["title"].ToString(),
                    MallProductAttrId = item["sku_id"].ToString(),
                    MallProductAttrs = item["sku_properties_name"].ToString(),
                    OrderId = trade["tid"].ToString(),
                    MallPrice = decimal.Parse(item["price"].ToString()),
                    MallAmount = decimal.Parse(item["payment"].ToString()),
                    Quantity = int.Parse(item["num"].ToString()),
                    DiscountFee = decimal.Parse(item["discount_fee"].ToString()),
                    MallOrderItemId = item["oid"].ToString()
                });

            }

            //第三方订单交易信息
            order.MallOrderPayment = new MallOrderPaymentInfo()
            {
                Payment = decimal.Parse(trade["payment"].ToString()),
                AlipayNo = trade["outer_tid"].ToString(),
                PostFee = decimal.Parse(trade["post_fee"].ToString()),
                DiscountFee = decimal.Parse(trade["discount_fee"].ToString()),
                PayTime = DateTime.Parse(trade["pay_time"].ToString() == "" ? "1900-01-01" : trade["pay_time"].ToString())
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
                City = trade["receiver_city"].ToString(),
                Province = trade["receiver_state"].ToString(),
                District = trade["receiver_district"].ToString(),
                ReceiveAddress = trade["receiver_address"].ToString(),
                ReceiveContact = trade["receiver_name"].ToString(),
                TelPhone = "",
                Mobile = trade["receiver_mobile"].ToString(),
                PostCode = trade["receiver_zip"].ToString()
            };

            string buyer_messages = orders[0]["buyer_messages"].ToString();
            if (buyer_messages != "" && (buyer_messages.ToLower().Contains("\"title\": \"身份证\"") || buyer_messages.ToLower().Contains("\"title\": \"身份证号码\"")))
            {

                order.MallOrderReceive.IdCard = orders[0]["buyer_messages"][0]["content"].ToString();

            }


            //订单备注标示
            order.MallOrderBuyer.SellerFlag = trade["seller_flag"].ToString();
        }
        #endregion
        /// <summary>
        /// 获取单笔订单详情
        /// </summary>
        /// <param name="param">参数实体</param>
        /// <param name="auth">授权参数</param>
        /// <returns>单笔订单详情</returns>
        /// <remarks>2016-6-11 杨浩 创建</remarks>
        public Result<UpGradeOrder> GetOrderDetail(OrderParameters param, AuthorizationParameters auth)
        {
            return null;
        }

        /// <summary>
        /// 用户调用该接口可实现自己联系发货（线下物流）
        /// </summary>
        /// <param name="param">参数实体</param>
        /// <param name="auth">授权参数</param>
        /// <returns>处理结果</returns>
        /// <remarks>2016-6-11 杨浩 创建</remarks>
        public Result SendDelivery(DeliveryParameters param, AuthorizationParameters auth)
        {
            var result = new Result() { Status = true };
            var _param = new Dictionary<string, string>();
            _param.Add("tid", param.MallOrderId);//交易编号 （否）
            //_param.Add("outer_tid", "X231958349");//外部交易编号也可以根据外部交易编号发货，tid、outer_tid 必须选其一  （否）
            _param.Add("out_stype", param.CompanyCode);// 快递公司类型（否）
            _param.Add("out_sid", param.HytExpressNo);// 快递单号（具体一个物流公司的真实快递单号）（否）
            //_param.Add("oids", "1");// 如果需要拆单发货，使用该字段指定要发货的交易明细的编号，多个明细编号用半角逗号分隔,不需要拆单发货，则改字段不传或值为空（否）
            _param.Add("is_no_express", "0");// 发货是否无需物流如果为 0 则必须传递物流参数，如果为 1 则无需传递物流参数out_stype和out_sid，默认为 0（否）        
            //var response = kit.post("kdt.logistics.online.confirm", _param, null, "");
            //var _response = JObject.Parse(response);

            //if (_response.Property("error_response") != null)
            //{
            //    result.Status = false;
            //    result.Message = _response["error_response"]["msg"].ToString();
            //    result.StatusCode = int.Parse(_response["error_response"]["code"].ToString());
            //}
            return result;
        }

        /// <summary>
        /// 获取可合并升舱订单列表
        /// </summary>
        /// <param name="param">参数实体</param>    
        /// <param name="auth">授权参数</param>
        /// <returns>可合并升舱订单列表</returns>
        /// <remarks>2016-6-11 杨浩 创建</remarks>
        public Result<List<UpGradeOrder>> GetCombineOrders(OrderParameters param, AuthorizationParameters auth)
        {
            return null;
        }

        /// <summary>
        /// 使用授权码获取登录令牌
        /// </summary>
        /// <param name="code">授权码</param>
        /// <returns>登录令牌</returns>
        /// <remarks>2016-6-11 杨浩 创建</remarks>
        public Result<AccessTokenResult> GetAuthorizationCode(string code)
        {
            return null;
        }

        /// <summary>
        /// 更新订单备注信息
        /// </summary>
        /// <param name="remarks">备注内容</param>
        /// <param name="auth">授权参数</param>
        /// <returns>2016-6-11 杨浩 创建</returns>
        public Result UpdateTradeRemarks(IRemarksParameters remarks, AuthorizationParameters auth)
        {
            return null;
        }


        public Result<UpGradeExpress> GetExpress(AuthorizationParameters auth)
        {
            throw new NotImplementedException();
        }
    }
}
