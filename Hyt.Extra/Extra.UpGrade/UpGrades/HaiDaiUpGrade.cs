using Extra.UpGrade.Api;
using Extra.UpGrade.HaiDaiModel;
using Extra.UpGrade.Model;
using Extra.UpGrade.Provider;
using Hyt.Model;
using Hyt.Model.UpGrade;
using Hyt.Model.WorkflowStatus;
using Hyt.Util;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Top.Api.Request;

namespace Extra.UpGrade.UpGrades
{
    /// <summary>
    /// 海带接口实现
    /// </summary>
    /// <remarks>2017-06-13 杨浩 创建</remarks>
    public class HaiDaiUpGrade : IUpGrade
    {
        public int i = 0;
       
        /// <summary>
        /// 海带接口配置信息
        /// </summary>
        private static readonly HaiDaiConfig config = UpGradeConfig.GetHaiDaiConfig();

        //客户端
        //private  HaiDaiApi client = new HaiDaiApi(config.AppSecret, config.AppKey, config.UserName, config.PassWord);
        //客户端
        //private static readonly HaiDaiApi client = new HaiDaiApi(config.AppSecret, config.AppKey, config.UserName, config.PassWord);
        //登录
        //private  HaiDaiResultLogin login = client.LoginApi(config.ApiUrlTest + "/api/mobile/member!hdLogin.do");

        
       
      
        /// <summary>
        /// 获取客户端对象
        /// </summary>
        /// <param name="appSecret"></param>
        /// <param name="appKey"></param>
        /// <param name="userdData">用户数据:{"userName":"账号","passWord":"密码"}</param>
        /// <returns></returns>
        /// <remarks>2017-11-03 杨浩 创建</remarks>
        private HaiDaiApi GetClient(string appSecret, string appKey, string userdData)
        {
            var userInfo=JObject.Parse(userdData);
            var client = new HaiDaiApi(appSecret, appKey, userInfo["userName"].ToString(), userInfo["passWord"].ToString());          
            return client;
        }
        private string MD5Encrypt(string strText)
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
        /// <summary>
        /// 获取已升舱待发货订单
        /// </summary>
        /// <param name="param">条件参数</param>
        /// <param name="auth">授权参数</param>
        /// <returns>订单列表</returns>
        /// <reamrks>2016-6-11 杨浩 创建</reamrks>
        public Result<List<UpGradeOrder>> GetUpGradedWaitSend(OrderParameters param, AuthorizationParameters auth)
        {

            var result = new Result<List<UpGradeOrder>> { Status = true, Message = "", Data = null, StatusCode = 0 };

            result.Data = new List<UpGradeOrder>();

            var xms = new Hyt.Util.Xml.XmlDocumentExtender();

            xms.LoadXml(param.Xml);

            var order = new UpGradeOrder();

         

            //商城订单明细
            order.UpGradeOrderItems = new System.Collections.Generic.List<UpGradeOrderItem>();

            var orders = xms.SelectNodes("/HipacPush/Body/OrderItemList/OrderItem");

            var _order = xms["HipacPush"]["Body"]["Order"];

            for (int i = 0; i < orders.Count; i++)
            {
                var item = orders[i];
                var code = item["itemSupplyNo"].InnerText;

                int specNum = int.Parse(item["specNum"].InnerText);//规格数
                decimal tariffRate = decimal.Parse(item["tariffRate"].InnerText);//关税
                decimal exciseRate = decimal.Parse(item["exciseRate"].InnerText);//消费税
                decimal addTaxRate = decimal.Parse(item["addTaxRate"].InnerText);//增值税

                decimal itemTotalTax = decimal.Parse(item["itemTotalTax"].InnerText);//总税款
                decimal itemTotal = decimal.Parse(item["itemTotal"].InnerText);//商品总价

                order.UpGradeOrderItems.Add(new UpGradeOrderItem()
                {
                    OrderId=_order["orderNum"].InnerText,                   
                    MallProductName = item["itemName"].InnerText,
                    MallProductCode = code,
                    MallPrice = decimal.Parse(item["itemPrice"].InnerText),
                    MallAmount = itemTotal,
                    Quantity = int.Parse(item["itemQuantity"].InnerText),
                });

            }

            var receiveInfo = xms.SelectSingleNode("/HipacPush/Body/Customer");

            //订单收货信息   
            order.MallOrderReceive = new MallOrderReceiveInfo()
            {
                City = receiveInfo["custCity"].InnerText,
                Province = receiveInfo["custProvice"].InnerText,
                District = receiveInfo["custArea"].InnerText,
                ReceiveAddress = receiveInfo["custAddress"].InnerText,
                ReceiveContact = receiveInfo["custName"].InnerText,
                Mobile = receiveInfo["custPhone"].InnerText,
                IdCard = receiveInfo["custIdNum"].InnerText,
            };



            var payInfo = xms["HipacPush"]["Body"]["PayInfo"];
          




            order.HytOrderDealer = new HytOrderDealerInfo()
            {
                //第三方订单编号              
                HytPayType =Hyt.Model.SystemPredefined.PaymentType.分销商预存,// Convert.ToInt32(payInfo["payType"].InnerText),//支付类型（微信支付，支付宝，盛付通）
                HytPayTime = DateTime.Parse(payInfo["payTime"].InnerText),
                HytPayment = decimal.Parse(_order["totalPayAmount"].InnerText),
                DealerSysNo = 0,
                DealerMallSysNo = 0,
            };

            order.MallOrderBuyer = new MallOrderBuyerInfo()
            {
                MallOrderId = _order["orderNum"].InnerText,   
            };

            order.MallOrderPayment = new MallOrderPaymentInfo()
            {
                AlipayNo = payInfo["payNo"].InnerText,
                PayTime = DateTime.Parse(payInfo["payTime"].InnerText),
                Payment = decimal.Parse(_order["totalPayAmount"].InnerText),
                PostFee =0,// decimal.Parse(_order["logisticsAmount"].InnerText),
                TotalTaxAmount =0// decimal.Parse(_order["totalTaxAmount"].InnerText),
            };

            result.Data.Add(order);

            return result;
        }

        /// <summary>
        /// 海带批量获取指定时间区间的订单并接单
        /// (待升舱的订单)(未发货的订单)
        /// </summary>
        /// <param name="param">参数实体</param>
        /// <param name="auth">授权参数</param>
        /// <returns>订单列表</returns>
        /// <remarks>2016-6-11 杨浩 创建</remarks>
        public Result<List<UpGradeOrder>> GetOrderList(OrderParameters param, AuthorizationParameters auth)
        {
            var dealerInfo = new HytOrderDealerInfo()
            {
                DealerSysNo =auth.DealerMall.DealerSysNo, //408,
                DealerMallSysNo =auth.DealerMall.SysNo, //22,
                HytPayStatus = 20,
                IsSelfSupport =auth.DealerMall.IsSelfSupport, //1,
            };
                 
            var client = GetClient(auth.DealerApp.AppSecret, auth.DealerApp.AppKey,auth.DealerApp.Extend.Trim());      

            var result = new Result<List<UpGradeOrder>> { Status = true, Message = "", Data = null, StatusCode = 0 };

            #region 时间控制
            string weekstr = DateTime.Now.DayOfWeek.ToString();
             bool isTest=true;
             if ((weekstr == "Sunday" || weekstr == "Saturday" || (DateTime.Now.Hour > 16 && DateTime.Now.Minute > 30) || (DateTime.Now.Hour < 9)) && !isTest)
            {
                return new Result<List<UpGradeOrder>>()
                {
                    Status = false,
                    StatusCode = 9999,
                    Message = "不是接单时间",
                    Data = null
                };
            }
            #endregion

            #region 参数
            var pa = new HaiDaiOrderParameter();
            pa.payStartTime =param.StartDate.ToString("yyyy-MM-dd");
            pa.payEndTime = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            //type=3 待接收订单  0:待发货
            pa.type = 3;
            pa.PageSize =9999999;
            pa.PageNum = 1;        
            var dicOrder = SetParameters(auth.AuthorizationCode);       
            dicOrder.Add("type", pa.type.ToString());       
            dicOrder.Add("abnormalStatus", "0");

            if (!(pa.PageNum == 0))           
                dicOrder.Add("pageNum", pa.PageNum.ToString());
            
            if (!(pa.PageSize == 0))           
                dicOrder.Add("pageSize", pa.PageSize.ToString());
            
            
            //时间范围暂时注释
            if (!string.IsNullOrEmpty(pa.payStartTime))           
                dicOrder.Add("payStartTime", pa.payStartTime);
            
            if (!string.IsNullOrEmpty(pa.payEndTime))           
                dicOrder.Add("payEndTime", pa.payEndTime);
            
            #endregion
  
            //正式服务器接口
            string url = config.ApiUrlTest + "/api/depot/order!getOrderList.do";
            //第三方订单id集合
            var orders = new List<int>();
            var ordlist = client.InvokeOpenApi(url, dicOrder, null);

            var list = new List<UpGradeOrder>();

            #region 待接收订单
            if (ordlist.result == 1)            
            {                             
                foreach (resultList deal in ordlist.data.result)
                {
                    orders.Add(deal.orderId);
                    var order = new UpGradeOrder();

                    var orderdetailResult = OrderDetail(deal.orderId.ToString(), auth);
                    if (!orderdetailResult.Status)
                    {
                        result.Status = false;
                        result.Message = orderdetailResult.Message;
                        return result;
                    }
                    Map(deal, order, orderdetailResult.Data);

                    order.HytOrderDealer = dealerInfo;
                    list.Add(order);
                }                              
            }
            else
            {
                result.Status = false;
                result.Message = ordlist.message;
                return result;
            }
            #endregion

            #region 待发货订单
            dicOrder["type"]="0";
            //dicOrder["type"] = "1";
            dicOrder.Remove("timestamp");
            dicOrder.Remove("topSign");
            ordlist = client.InvokeOpenApi(url,dicOrder, null);
            if (ordlist.result == 1)
            {
                foreach (resultList deal in ordlist.data.result)
                {
                    orders.Add(deal.orderId);
                    var order = new UpGradeOrder();
                    var orderdetailResult = OrderDetail(deal.orderId.ToString(), auth);
                    if (!orderdetailResult.Status)
                    {
                        result.Status = false;
                        result.Message = orderdetailResult.Message;
                        return result;
                    }

                
                    Map(deal, order, orderdetailResult.Data);
                    order.HytOrderDealer = dealerInfo;
                    order.HytOrderDealer.HytPayType = Hyt.Model.SystemPredefined.PaymentType.分销商预存;

                    
                    list.Add(order);

                }
            }
            else
            {
                result.Status = false;
                result.Message = ordlist.message;
                return result;
            }             
            #endregion


            //if (isTest)
            //{
            //    return new Result<List<UpGradeOrder>>()
            //    {
            //        Status = true,
            //        StatusCode = 9999,
            //        Data = new List<UpGradeOrder>()
            //    };
            //}

            //接单
            var res = GetMallOrderDetail(orders,client,auth);
        
            return new Result<List<UpGradeOrder>>()
            {
                Status = true,
                StatusCode = 1,
                Data = list
            };

        }
        #region MAP
        void Map(resultList deal, UpGradeOrder order, DataDetail detail)
        {

            order.HytOrderDealer = new HytOrderDealerInfo();
       
            //第三方买家订单信息
            order.MallOrderBuyer = new MallOrderBuyerInfo()
            {
                BuyerNick = deal.shipName,
                MallOrderId =deal.sn.ToString(),// deal.orderId.ToString(),
                SN = deal.sn.ToString(),
                BuyerMessage = string.Empty,
                SellerMessage = string.Empty
            };

            if (detail.orderItemList != null)
            {
                //var item = deal.orderItemList.Select(item => item.name == haidai.data.orderItemList.Select(i=> i.name==item.name).First().Equals);



                order.UpGradeOrderItems = new List<UpGradeOrderItem>();
                //订单明细列表
                foreach (var item in detail.orderItemList)
                {
                    var orderItem = new UpGradeOrderItem();
                    string productCode=item.psn;
                    if(item.psn.Contains('-'))                 
                        productCode=item.psn.Split('-')[0];

                    orderItem.MallProductCode = productCode;
                    orderItem.HytProductErpCode = productCode;
                    orderItem.MallProductName = item.name;
                    orderItem.MallProductAttrId = string.Empty;
                    orderItem.MallProductAttrs = string.Empty;
                    orderItem.OrderId = deal.orderId.ToString();
                    orderItem.MallPrice = item.supplyPrice/item.totalNum;
                    orderItem.HytPrice = orderItem.MallPrice; //单价
                    orderItem.MallAmount = item.supplyPrice*item.num;//供货价*数量
                    orderItem.Quantity =item.totalNum;// item.num*int.Parse(item.specName.ToCharArray()[0].ToString()),
                    orderItem.DiscountFee = 0;
                    orderItem.MallOrderItemId = item.itemId.ToString();
                    orderItem.SN = deal.sn;
                    var _item = order.UpGradeOrderItems.Where(x => x.MallProductCode == productCode).FirstOrDefault();
                    if (_item != null)
                    {                      
                        orderItem.Quantity = orderItem.Quantity + _item.Quantity;
                        orderItem.MallAmount = _item.MallAmount + orderItem.MallAmount;
                        order.UpGradeOrderItems.Remove(_item);                     
                    }
                 
                    order.UpGradeOrderItems.Add(orderItem);                     
                }


                //order.UpGradeOrderItems = deal.orderItemList.Select(item => new UpGradeOrderItem
                //{
                //    MallProductCode = haidai.orderItemList.Where(i => i.name == item.name).First().psn.Split('-').Length > 1 ? haidai.orderItemList.Where(i => i.name == item.name).First().psn.Split('-')[0] : haidai.orderItemList.Where(i => i.name == item.name).First().psn,
                //    HytProductErpCode = haidai.orderItemList.Where(i => i.name == item.name).First().psn.Split('-').Length > 1 ? haidai.orderItemList.Where(i => i.name == item.name).First().psn.Split('-')[0] : haidai.orderItemList.Where(i => i.name == item.name).First().psn,
                //    MallProductName = item.name,
                //    MallProductAttrId = string.Empty,
                //    MallProductAttrs = string.Empty,
                //    OrderId = deal.orderId.ToString(),
                //    MallPrice = item.supplyPrice ,
                //    MallAmount = item.supplyPrice,
                //    Quantity =item.totalNum,// item.num*int.Parse(item.specName.ToCharArray()[0].ToString()),
                //    DiscountFee = 0,
                //    MallOrderItemId = item.itemId.ToString(),
                //    SN = deal.sn,
                //}).ToList();
            }

            //第三方订单交易信息
            order.MallOrderPayment = new MallOrderPaymentInfo()
            {
                TotalTaxAmount=deal.tax,
                Payment = deal.goodsCost,
                AlipayNo = string.Empty,
                PostFee = decimal.Parse(deal.shipMoney ?? "0") ,
                DiscountFee = 0,
                PayTime = DateTime.Parse(deal.paySuccessTime)
            };

            #region 当明细金额合计与实收金额不等时，将差额分摊到最后一个商品上

            //if ((order.MallOrderPayment.Payment - order.MallOrderPayment.PostFee) != order.UpGradeOrderItems.Sum(i => i.MallAmount))
            //{
            //    var _amt = 0m;
            //    var _count = 0;
            //    foreach (var item in order.UpGradeOrderItems)
            //    {
            //        _count++;
            //        if (_count == order.UpGradeOrderItems.Count)
            //        {
            //            item.MallAmount = (order.MallOrderPayment.Payment - order.MallOrderPayment.PostFee) - _amt;
            //            break;
            //        }
            //        _amt += item.MallAmount;
            //    }
            //}

            #endregion

            //订单收货信息
            order.MallOrderReceive = new MallOrderReceiveInfo()
            {
                City = detail.city,
                Province = detail.province,
                District = detail.region,
                ReceiveAddress = detail.shipAddr,
                ReceiveContact = detail.shipName,
                TelPhone = string.Empty,
                Mobile = detail.shipMobile,
                PostCode = string.Empty,
                IdCard = detail.idNumber
            };
        }
        #endregion
        /// <summary>
        /// 获取海带单笔订单详情
        /// </summary>
        /// <param name="OrderID">海带订单id</param>
        /// <param name="auth">授权参数</param>
        /// <returns>单笔订单详情</returns>
        /// <remarks>2016-6-11 杨浩 创建</remarks>
        /// <remarks>2017-6-15 罗勤尧 修改</remarks>
        public Result<UpGradeOrder> GetOrderDetail(string orderID, AuthorizationParameters auth)
        {        

              var reslut = new Result<UpGradeOrder>() { Status=true};        

                var _result=OrderDetail(orderID,auth);

                if(!_result.Status)
                {
                    reslut.Message = _result.Message;
                    reslut.Status = false;
                    return reslut;
                }

                var orders =_result.Data;
                if (orders == null)
                {
                    reslut.Message = "获取异常，请联系客服";
                    reslut.Status = false;
                    return reslut;                
                }
                UpGradeOrder ord = new UpGradeOrder();
                ord.HytOrderDealer = new HytOrderDealerInfo() { };
                ord.MallOrderBuyer = new MallOrderBuyerInfo() {
                    BuyerNick = "海带",
                    MallOrderId = orders.orderId.ToString(),
                    BuyerMessage = "",
                    SellerMessage = ""
                };
                ord.MallOrderPayment = new MallOrderPaymentInfo() {
                    Payment =0,
                    AlipayNo = string.Empty,
                    PostFee = orders.shipMoney,
                    DiscountFee = 0,
                    PayTime =DateTime.Now
                    //Orders.data.paySuccessTimeStr
                };
                ord.MallOrderReceive = new MallOrderReceiveInfo() {
                    City = orders.city,
                    Province = orders.province,
                    District = orders.region,
                    ReceiveAddress =orders.shipAddr,
                    ReceiveContact = orders.shipName,
                    TelPhone = string.Empty,
                    Mobile = orders.shipMobile,
                    PostCode = string.Empty,
                    IdCard= orders.idNumber
                };
                ord.UpGradeOrderItems = orders.orderItemList.Select(item => new UpGradeOrderItem
                {
                    MallProductCode = item.psn,
                    MallProductName = item.name,
                    MallProductAttrId = string.Empty,
                    MallProductAttrs = string.Empty,
                    OrderId = orders.orderId.ToString(),
                    MallPrice = item.supplyPrice,
                    MallAmount = item.supplyPrice,
                    Quantity = item.num,
                    DiscountFee = 0,
                    MallOrderItemId = item.itemId.ToString()
                }).ToList();

                reslut.Data = ord;
                reslut.StatusCode = 1;
                return reslut;
           
        }

        #region 登录获取Token
        ///// <summary>
        ///// 登录获取Token
        ///// </summary>
        ///// <param name="dic">参数</param>
        ///// <returns>返回对象</returns>
        ///// <remarks>2017-6-14 罗勤尧 创建</remarks>
        //private HaiDaiResultLogin ApiLogin(IDictionary<string, string> dic)
        //{
        //    //正式服务器登陆接口
        //    //string url = config.ApiUrl + "/api/mobile/member!hdLogin.do";
        //    //测试服务器登陆接口
        //    string url = config.ApiUrlTest + "/api/mobile/member!hdLogin.do";

        //    //var response = client.LoginApi(url);

        //    return response;

        //}
        #endregion
        /// <summary>
        /// 获取海带单笔订单详情
        /// </summary>
        /// <param name="param">查询参数</param>
        /// <param name="auth">授权参数</param>
        /// <returns>单笔订单详情</returns>
        /// <remarks>2016-6-11 杨浩 创建</remarks>
        /// <remarks>2017-6-15 罗勤尧 修改</remarks>
        public Result<UpGradeOrder> GetOrderDetail(OrderParameters param, AuthorizationParameters auth)
        {

            var OrderDetail = GetOrderDetail(param.OrderID, auth);
            return OrderDetail;
        }
        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="authorizationCode">授权码</param>
        /// <returns></returns>
        private Dictionary<string, string> SetParameters(string authorizationCode)
        {
            string memberId = authorizationCode.Substring(0, authorizationCode.IndexOf("_"));
            string token = authorizationCode.Substring(authorizationCode.IndexOf("_")+1);
            var dicOrder = new Dictionary<string, string>();
            dicOrder.Add("memberId", memberId);
            dicOrder.Add("token", token);
            return dicOrder;
        }
        #region 获取订单列表
        
        #endregion

        #region 接单
        /// <summary>
        /// 接单
        /// </summary>
        /// <param name="orderIds">订单编号</param>
        /// <param name="authorizationcode">授权码</param>
        /// <returns>单笔订单详情</returns>
        /// <remarks>2017-6-15 罗勤尧 创建</remarks>
        public HaiDaiResultOrder GetMallOrderDetail(List<int> orderIds, HaiDaiApi client, AuthorizationParameters auth)
        {         
                  
            var dicOrder = SetParameters(auth.AuthorizationCode);
      
            dicOrder.Add("orderIds_str", string.Join(",", orderIds.ToArray()));
            //返回字段
            //订单号
            //测试服务器接口
            string url = config.ApiUrlTest + "/api/depot/order!receiveOrder.do";
            var response = client.InvokeOpenApi(url, dicOrder);

            return response;          
        }
        #endregion      

        #region 订单详情
        /// <summary>
        /// 订单详情
        /// </summary>
        /// <param name="orderIds">订单编号</param>
        /// <param name="authorizationcode">授权码</param>
        /// <returns>单笔订单详情</returns>
        /// <remarks>2017-6-15 罗勤尧 创建</remarks>
        public Result<DataDetail> OrderDetail(string orderIds, AuthorizationParameters auth)
        {

            var client = GetClient(auth.DealerApp.AppSecret,auth.DealerApp.AppKey,auth.DealerApp.Extend.Trim());

            var dicOrder = SetParameters(auth.AuthorizationCode);
            dicOrder.Add("orderId", orderIds.ToString());

            //返回字段
            //订单号
            //测试服务器接口
            string url = config.ApiUrlTest + "/api/depot/order!getOrderDetail.do";

            var response = client.OrderDetail(url, dicOrder);
                      
            return response;
          

        }
        #endregion

        #region 快递信息
        /// <summary>
        /// 快递信息
        /// </summary>
        /// <param name="orderSn">订单编号</param>
        /// <param name="authorizationcode">授权码</param>
        /// <returns>单笔订单详情</returns>
        /// <remarks>2017-6-15 罗勤尧 创建</remarks>
        public HaiDaiResultKuaiDi KuaiDiDetail(string orderSn, string authorizationcode)
        {
            //if (login.result != 1)
            //{
            //    return null;

            //}
          
            //IDictionary<string, string> dicOrder = new Dictionary<string, string>();
            //dicOrder.Add("member_id", login.member.member_id);
            //dicOrder.Add("token", login.token);
            //dicOrder.Add("orderSn", orderSn.ToString());

            ////返回字段
            ////订单号
            ////测试服务器接口
            //string url = config.ApiUrlTest + "/api/mobile/order!hdOrderKuaidiByOrderSn.do";
            //var response = client.KuaiDiDetail(url, dicOrder);
            return null;
        

        }
        #endregion

        /// <summary>
        /// 用户调用该接口可实现自己联系发货（线下物流）
        /// </summary>
        /// <param name="param">参数实体</param>
        /// <param name="auth">授权参数</param>
        /// <returns>处理结果</returns>
        /// <remarks>2016-6-11 杨浩 创建</remarks>
        public Result SendDelivery(DeliveryParameters param, AuthorizationParameters auth)
        {
            var client=new HaiDaiApi(auth.DealerApp.AppSecret, auth.DealerApp.AppKey, auth.ShopAccount,auth.DealerApp.Extend.Trim()); 
 
            var result = new Result() { Status = false };
          

            var dicOrder= SetParameters(auth.AuthorizationCode);
          
            dicOrder.Add("shipNo", param.HytExpressNo);
            dicOrder.Add("shipType", param.CompanyCode);
            dicOrder.Add("sn", param.MallOrderId.ToString());
            //返回字段
            //订单号
            //测试服务器接口
            string url = config.ApiUrlTest + "/api/depot/order!addShipGoods.do";
            var response = client.ShipGoods(url, dicOrder);
        
   
            if (response.result == 1 ||response.message.Contains("已发货"))
            {
                result.Status=true;
                result.StatusCode = 1;
                result.Message = response.message;
            }else
            {
                result.Status = false;
                result.StatusCode = -1;
                result.Message = response.message;
            }

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
                    && order.MallOrderReceive.ReceiveAddress == result.MallOrderReceive.ReceiveAddress
                    && order.MallOrderReceive.IdCard == result.MallOrderReceive.IdCard
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
        /// 使用授权码获取登录令牌
        /// </summary>
        /// <param name="code">授权码</param>
        /// <returns>登录令牌</returns>
        /// <remarks>2016-6-11 杨浩 创建</remarks>
        public Result<AccessTokenResult> GetAuthorizationCode(string code)
        {
            var result=new Result<AccessTokenResult>();
            var headers = System.Web.HttpContext.Current.Request.Headers;
            if (code == null)
            {
                 var _form= System.Web.HttpContext.Current.Request.Form;
                 result.Message = "/Distribution/GetRedirectUriCode?state="+_form["shopid"]+"&code=1";
                 result.StatusCode =10000;
                 result.Status = true;
            }
            else
            {
                string appSecret=headers.Get("AppSecret");
                string appKey=headers.Get("AppKey");
                string userdData=headers.Get("Extend");
                var client=GetClient(appSecret, appKey, userdData);
                var login = client.LoginApi(config.ApiUrlTest + "/api/mobile/member!hdLogin.do");

                if (login.result != 1)
                {
                    return new Result<AccessTokenResult>()
                    {
                        Message = login.message,
                        Status = false,
                    };
                }
                else
                {
                    return new Result<AccessTokenResult>()
                    {
                        Data = new AccessTokenResult()
                        {
                            AccessToken =login.member.member_id+"_"+login.token,
                            UserNick =login.member.member_id
                        },
                        Message =login.message,
                        Status = true,
                    };
                }
            }

            return  result;

           
            
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
