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
using Hyt.Util.Xml;
using System.Security.Cryptography;

namespace Extra.UpGrade.UpGrades
{
    #region 海拍客订单接口实现
    /// <summary>
    /// 海拍客订单接口实现
    /// </summary>
    /// <remarks>2017-10-16 杨浩 创建</remarks>
    public class HipacUpGrade : IUpGrade
    {
    



        #region 海拍客支付类型转为系统支付类型
        /// <summary>
        /// 海拍客支付类型转为系统支付类型
        /// </summary>
        /// <param name="payType">海拍客支付类型</param>
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
            var result = new Result<List<UpGradeOrder>> { Status = true, Message = "", Data = null, StatusCode = 0 };
                  
            //// 支付方式（1货到付款, 2邮局汇款, 3自提, 4在线支付, 5公司转账, 6银行卡转账）  
            //order.HytOrderDealer.HytPayType = JingDongToPayTypeSysNo(trade["orderInfo"]["payType"].ToString());
            var xms = new Hyt.Util.Xml.XmlDocumentExtender();
            xms.LoadXml(param.Xml);

            var order = new UpGradeOrder();

            //商城订单明细
            order.UpGradeOrderItems = new System.Collections.Generic.List<UpGradeOrderItem>();

            var orders = xms.SelectNodes("/HipacPush/Body/OrderItemList/OrderItem");
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

                int itemQuantity = int.Parse(item["itemQuantity"].InnerText);
                order.UpGradeOrderItems.Add(new UpGradeOrderItem()
                {
                    MallProductName = item["itemName"].InnerText,
                    MallProductCode = code,
                    MallPrice = decimal.Parse(item["itemPrice"].InnerText),
                    MallAmount = itemTotal,
                    Quantity = int.Parse(item["itemQuantity"].InnerText),
                    HytPrice = itemTotal / itemQuantity,
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
            var _order = xms["HipacPush"]["Body"]["Order"];


            order.MallOrderBuyer = new MallOrderBuyerInfo()
            {
                MallOrderId = _order["orderNum"].InnerText,
            };
            order.HytOrderDealer = new HytOrderDealerInfo()
            {
                //第三方订单编号              
                HytPayType = Hyt.Model.SystemPredefined.PaymentType.分销商预存,//Convert.ToInt32(payInfo["payType"].InnerText),//支付类型（微信支付，支付宝，盛付通）
                HytPayTime = DateTime.Parse(payInfo["payTime"].InnerText),
                HytPayment = decimal.Parse(_order["totalPayAmount"].InnerText),
                DealerSysNo = 0,
                DealerMallSysNo = 0,
            };

            order.MallOrderPayment = new MallOrderPaymentInfo()
            {
                AlipayNo = payInfo["payNo"].InnerText,
                PayTime = DateTime.Parse(payInfo["payTime"].InnerText),
                Payment = decimal.Parse(_order["totalPayAmount"].InnerText),
                PostFee = decimal.Parse(_order["logisticsAmount"].InnerText),
                TotalTaxAmount = decimal.Parse(_order["totalTaxAmount"].InnerText),
            };

            //string sign = MD5Encrypt(config.AppKey + param.OrderID + config.AppKey).ToUpper();
            result.Data = new List<UpGradeOrder>();
            //订单信息   
            result.Data.Add(order);

            return result;
     
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



        //MD5加密
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
            //req.orderId = param.OrderID;
            
            ////需返回的字段列表
            //req.optionalFields = "orderId,venderId,orderType,payType,orderTotalPrice,orderSellerPrice,orderPayment,freightPrice,sellerDiscount,orderState,orderStateRemark,deliveryType,invoiceInfo,invoiceCode,orderRemark,orderStartTime,orderEndTime,consigneeInfo,itemInfoList,couponDetailList,venderRemark,balanceUsed,pin,returnOrder,paymentConfirmTime,waybill,logisticsId,vatInfo,modified,directParentOrderId,parentOrderId,customs,customsModel,orderSource,storeOrder,idSopShipmenttype,scDT,serviceFee,pauseBizInfo,taxFee,tuiHuoWuYou,storeId";
            //XmlDocumentExtender xms = new XmlDocumentExtender();
            //xms.LoadXml(param.ProductName = "推单请求报文.xml");
         
            //var response = client.Execute(req, config.AccessToken, DateTime.Now.ToLocalTime());

            //var order = new UpGradeOrder() { };
            //var trade = JObject.Parse(response.orderDetailInfo);
            //Map(trade["orderInfo"], order);
            //order.HytOrderDealer = new HytOrderDealerInfo();
            //// 支付方式（1货到付款, 2邮局汇款, 3自提, 4在线支付, 5公司转账, 6银行卡转账）  
            //order.HytOrderDealer.HytPayType = JingDongToPayTypeSysNo(trade["orderInfo"]["payType"].ToString());

          
            string xmlStr = System.IO.File.ReadAllText("推单请求报文.xml");
            Hyt.Util.Xml.XmlDocumentExtender xms = new Hyt.Util.Xml.XmlDocumentExtender();
            xms.LoadXml(param.ProductName="推单请求报文.xml");

            var order = new UpGradeOrder()
            {
            };

            //商城订单明细
            order.UpGradeOrderItems = new System.Collections.Generic.List<UpGradeOrderItem>();
            var orders = xms["HipacPush"]["Body"]["OrderItemList"].ChildNodes;

            for (int i = 0; i < orders.Count; i++)
            {
                var item = orders[i];
                var code = item["itemSupplyNo"].InnerText;
                order.UpGradeOrderItems.Add(new UpGradeOrderItem()
                {
                    MallProductName = item["itemName"].InnerText,
                    MallProductCode = item["itemSupplyNo"].InnerText,
                    MallPrice = decimal.Parse(item["itemPrice"].InnerText),
                    MallAmount = decimal.Parse(item["itemTotal"].InnerText),
                    Quantity = int.Parse(item["itemQuantity"].InnerText),


                });
            }

            // var aScource = order.UpGradeOrderItems;

            //订单收货信息   
            order.MallOrderReceive = new MallOrderReceiveInfo()
            {
                City = xms["HipacPush"]["Body"]["Customer"]["custCity"].InnerText,
                Province = xms["HipacPush"]["Body"]["Customer"]["custProvice"].InnerText,
                District = xms["HipacPush"]["Body"]["Customer"]["custArea"].InnerText,
                ReceiveAddress = xms["HipacPush"]["Body"]["Customer"]["custAddress"].InnerText,
                ReceiveContact = xms["HipacPush"]["Body"]["Customer"]["custName"].InnerText,
                Mobile = xms["HipacPush"]["Body"]["Customer"]["custPhone"].InnerText,
                IdCard = xms["HipacPush"]["Body"]["Customer"]["custIdNum"].InnerText,

            };
            // var bScource = order.MallOrderReceive;

            //订单信息   
            //  string dd= xms["HipacPush"]["Body"]["Order"]["payTime"].InnerText;
            var payInfo = xms["HipacPush"]["Body"]["PayInfo"];
            var _order = xms["HipacPush"]["Body"]["Order"];

            //支付信息
            order.HytOrderDealer = new HytOrderDealerInfo()
            {
                //第三方订单编号

                HytPayType = Convert.ToInt32(payInfo["payType"].InnerText),
                HytPayTime = DateTime.Now // DateTime.Parse(payInfo["payTime"].InnerText == "0001-01-01 00:00:00" ? "1900-01-01" : payInfo["payTime"].InnerText) 

            };
            order.MallOrderPayment = new MallOrderPaymentInfo()
            {

                AlipayNo = payInfo["payNo"].InnerText,
                PayTime = DateTime.Now,
                //  DateTime.Parse(payInfo["payTime"].InnerText.Trim() == "00010101 000000" ? "1900-01-01" : payInfo["payTime"].InnerText),      

                Payment = decimal.Parse(_order["totalPayAmount"].InnerText),
                PostFee = decimal.Parse(_order["logisticsAmount"].InnerText),
            };

           




            //订单信息   
            result.Data = order;



         
            return result;
        }




        /// <summary>
        /// 给海拍客请求签名。
        /// </summary>
        /// <param name="parameters">所有字符型的请求参数</param>
        /// <param name="secret">签名密钥</param>
        /// <param name="qhs">是否前后都加密钥进行签名</param>
        /// <returns>签名</returns>
        public static string SignHipcUpRequest(IDictionary<string, string> parameters, string secret, bool qhs)
        {
            // 第一步：把字典按Key的字母顺序排序
            IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(parameters);
            IEnumerator<KeyValuePair<string, string>> dem = sortedParams.GetEnumerator();

            // 第二步：把所有参数名和参数值串在一起
            StringBuilder query = new StringBuilder(secret);
            while (dem.MoveNext())
            {
                string key = dem.Current.Key;
                string value = dem.Current.Value;
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                {
                    query.Append(key).Append(value);
                }
            }
            if (qhs)
            {
                query.Append(secret);
            }
            // 第三步：使用MD5加密
            MD5 md5 = MD5.Create();
            byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(query.ToString()));

            // 第四步：把二进制转化为大写的十六进制
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                result.Append(bytes[i].ToString("X2"));
            }

            return result.ToString();
        
        }

        #endregion

        #region 海拍客出库、联系发货（线下物流）
        /// <summary>
        /// 用户调用该接口可实现自己联系发货（线下物流）
        /// </summary>
        /// <param name="param">参数实体</param>
        /// <param name="auth">授权参数</param>
        /// <returns>处理结果</returns>
        /// <reamrks>2017-08-16 杨浩 创建</reamrks>
        public Result SendDelivery(DeliveryParameters param, AuthorizationParameters auth)
        {

            var result = new Result() { Status = true };
            var _param = new Dictionary<string, string>();

            string sendId = DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(100000, 999999).ToString();
            string signStr = "appKey=" + auth.DealerApp.AppKey + "&sendID=" + sendId + "&key=" + auth.DealerApp.AppSecret;
            string sign = MD5Encrypt(signStr).ToUpper();

            var companyArry=param.CompanyCode.Split('_');
            string logisticsName="";
            string logisticsCode=companyArry[0];
            if(companyArry.Length>1)
                logisticsName=companyArry[1];
           

            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
xml +=string.Format(@"<HipacPush>
  <Head>
    <version>1.0</version>
    <service>backLogisticNo</service>
    <sendID>{0}</sendID>  
    <appKey>{1}</appKey> 
    <sign>{2}</sign> 
  </Head>
  <Body>
    <Orders>
		<Order>
		  <orderNum>{3}</orderNum>
		  <LogisticsInfoList>
			  <LogisticsInfo>
				<logisticsName>{4}</logisticsName>
				<logisticsNo>{5}</logisticsNo>
				<logisticsCode>{6}</logisticsCode>
			  </LogisticsInfo>
		  </LogisticsInfoList>
		</Order>
	</Orders>
  </Body>  
</HipacPush>",sendId, auth.DealerApp.AppKey, sign, param.MallOrderId, logisticsName, param.HytExpressNo, logisticsCode);
            var xms = new XmlDocumentExtender();
            string resultStr=Hyt.Util.WebUtil.PostString(string.Format("http://gw.hipac.cn/pub/{0}04030302/transNotify.do",auth.ShopAccount),xml,"text/xml");
            xms.LoadXml(resultStr);
            /*
             * 
<?xml version="1.0" encoding="UTF-8"?>
<HipacPush>
  <Head>
    <!-- 版本 默认为1.0 -->
    <version>1.0</version>
    <!-- 接口名称 -->
    <service>backLogisticNo</service>
    <!-- 推送报文唯一值YYYYDDMMHHmmss+6位随机数字 -->
    <sendID>20171102152126152072</sendID>  
    <appKey>XYGS20171020</appKey>
    <retCode>SUCCESS</retCode> 
    <sign>7CBCF21556820EEEF40528A6BF522727</sign> 
  </Head>
  <Body>
    <bizCode>SUCCESS</bizCode>
    <bizMsg>成功</bizMsg>
    <Order>
      <!-- 订单编号 -->
      <orderNum>YT201710311424931334</orderNum>
    </Order>
  </Body>  
</HipacPush>
             * 
             */
            var order= xms.SelectSingleNode("/HipacPush/Body");
            if (order["bizCode"].InnerText.ToUpper() != "SUCCESS")
            {
                result.Status = false;
                result.Message = order["bizMsg"].InnerText;
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
        /// <reamrks>2017-08-16 黄杰 创建</reamrks>
        public Result<AccessTokenResult> GetAuthorizationCode(string code)
        {
                var dic = new Dictionary<string, string>();



                return null;
            
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