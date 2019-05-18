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
using Extra.UpGrade.GeGeJiaModel;
using Hyt.Util;
using System.Security.Cryptography;
using System.Net;
using System.IO;
using Hyt.DataAccess.Product;

namespace Extra.UpGrade.UpGrades
{
    #region 格格家订单接口实现
    /// <summary>
    /// 格格家订单接口实现
    /// </summary>
    /// <remarks>2017-08-18 黄杰 创建</remarks>
    public class GeGeJiaUpGrade : IUpGrade
    {
        /// <summary>
        /// 格格家订单接口配置信息
        /// </summary>
      //  private static readonly GeGeJiaConfig config = UpGradeConfig.GetGeGeJiaConfig();

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

        #region Post json 数据
        /// <summary>
        /// Post json 数据
        /// </summary>
        /// <param name="url">接收数据链接</param>
        /// <param name="param">json参数</param>
        /// <returns></returns>
        /// <remarks>2015-12-29 杨浩 创建</remarks>
        public string PostJson(string url, string param, string sign)
        {
            byte[] postData = param == "" ? new byte[0] : Encoding.UTF8.GetBytes(param);

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/json;charset=utf-8";
            //req.ContentType = "application/vnd.ehking-v1.0+json;charset=UTF-8";
            req.KeepAlive = true;
            req.Timeout = 300000;
            req.ContentLength = postData.Length;
            req.Headers.Add("sign", sign);

            Stream reqStream = req.GetRequestStream();

            reqStream.Write(postData, 0, postData.Length);

            reqStream.Close();
            HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();

            Stream stream = rsp.GetResponseStream();

            StreamReader sr = new StreamReader(stream, Encoding.UTF8);
            string result = sr.ReadToEnd();
            sr.Close();
            stream.Close();
            return result;
        }
        #endregion

        #region 格格家支付类型转为系统支付类型
        /// <summary>
        /// 格格家支付类型转为系统支付类型
        /// </summary>
        /// <param name="payType">格格家支付类型</param>
        /// <returns></returns>
        /// <reamrks>2017-08-18 黄杰 创建</reamrks>
        public int GeGeJiaToPayTypeSysNo(string payType)
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

        #region 格格家获取获取已升舱待发货订单
        /// <summary>
        /// 获取已升舱待发货订单
        /// </summary>
        /// <param name="param">条件参数</param>
        /// <param name="auth">授权参数</param>
        /// <returns>订单列表</returns>
        /// <reamrks>2017-08-18 黄杰 创建</reamrks>
        public Result<List<UpGradeOrder>> GetUpGradedWaitSend(OrderParameters param, AuthorizationParameters auth)
        {

            return null;
        }
        #endregion

        #region 格格家批量获取指定时间区间的订单
        /// <summary>
        /// 批量获取指定时间区间的订单
        /// (待升舱的订单)
        /// </summary>
        /// <param name="param">参数实体</param>
        /// <param name="auth">授权参数</param>
        /// <returns>订单列表</returns>
        /// <reamrks>2017-08-18 黄杰 创建</reamrks>
        /// /// <reamrks>2018-03-18 罗勤瑶 修改</reamrks>
        public Result<List<UpGradeOrder>> GetOrderList(OrderParameters param, AuthorizationParameters auth)
        {

            //var dealerInfo = new HytOrderDealerInfo()
            //{
            //    DealerSysNo = 129,
            //    DealerMallSysNo = 24,
            //    HytPayStatus = 20,
            //    IsSelfSupport = 1,
            //};
            var dealerInfo = new HytOrderDealerInfo()
            {
                DealerSysNo = auth.DealerMall.DealerSysNo, //
                DealerMallSysNo = auth.DealerMall.SysNo, //
                HytPayStatus = 20,
                IsSelfSupport = auth.DealerMall.IsSelfSupport, //
            };
            var result = new Result<List<UpGradeOrder>> { Status = true, Message = "", Data = null, StatusCode = 0 };
            var orderList = new List<UpGradeOrder>();
            var _param = new Dictionary<string, string>();
            param.PageSize = 999;
            param.PageIndex = 1;


            //请求的参数类调用
            GeGeJiaParameter ggjParameter = new GeGeJiaParameter();

            //startTime付款起始时间、endTime付款结束时间。starTime必须小于endTime，并且起始时间跟结束时间差不得超过30天
            //page页码，不传默认展示第一页
            //pageSize每页数量，不传默认一页50条
            //status订单状态；1：未付款，2：待发货，3：已发货，4：交易成功，5：用户取消（待退款团购），6：超时取消（已退款团购），7：团购进行中(团购)
            //partner商家身份标识
            //timestamp时间戳

            string parm = "{\"params\":{\"startTime\":\"" + param.StartDate.ToString("yyyy-MM-dd HH:mm:ss") + "\",\"endTime\":\"" + param.EndDate.ToString("yyyy-MM-dd HH:mm:ss") + "\",\"page\":" + param.PageIndex + ", \"pageSize\":" + param.PageSize
            + ", \"status\":2},\"partner\":\"" + auth.DealerApp.AppSecret + "\",\"timestamp\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\"}";


            #region 时间控制
            string weekstr = DateTime.Now.DayOfWeek.ToString();
            bool isTest = true;
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

            //MD5加密签名
            string sign = MD5Encrypt(auth.DealerApp.AppKey + parm + auth.DealerApp.AppKey).ToUpper();

            var response = PostJson("http://openapi.gegejia.com:8902/api/order/findOrders", parm, sign);
            param.PageIndex = 1;

            while (true)
            {
                var trade = JObject.Parse(response);
                if (trade["success"].ToString() == "false")
                {
                    return new Result<List<UpGradeOrder>>()
                    {
                        Status = false,
                        StatusCode = 9999,
                        Message = trade["errMsg"].ToString(),
                        Data = null
                    };
                }
                int total = int.Parse(trade["totalCount"].ToString());

                int totalPage = total / param.PageSize;
                if (total % param.PageSize != 0)
                    totalPage++;

                var trades = trade["orders"];
                if (trades == null )
                {
                    return new Result<List<UpGradeOrder>>()
                    {
                        Status = false,
                        StatusCode = 9999,
                        Message = "没有订单",
                        Data = null
                    };
                }
                bool has_next = param.PageIndex < totalPage;

                //trade = JObject.Parse(response.ToString());
                foreach (var i in trades)
                {
                    //未冻结订单才能接受
                    if (i["freezeStatus"].ToString() == "0" || i["freezeStatus"].ToString() == "2")
                    {
                        var order = new UpGradeOrder();

                        Map(i, order);
                        order.HytOrderDealer = dealerInfo;
                        // 支付方式（1货到付款, 2邮局汇款, 3自提, 4在线支付, 5公司转账, 6银行卡转账）  
                        order.HytOrderDealer.HytPayType = GeGeJiaToPayTypeSysNo(i["payChannel"].ToString());
                        orderList.Add(order);
                    }                  
                }

                if (has_next)
                {
                    param.PageIndex++;
                    ggjParameter.paramss.page = param.PageIndex;

                    parm = "{\"params\":{\"startTime\":\"" + param.StartDate.ToString("yyyy-MM-dd HH:mm:ss") + "\",\"endTime\":\"" + param.EndDate.ToString("yyyy-MM-dd HH:mm:ss") + "\",\"page\":" + param.PageIndex + ", \"pageSize\":" + param.PageSize
                    + ", \"status\":2},\"partner\":\"" + auth.DealerApp.AppSecret + "\",\"timestamp\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\"}";
                    response = PostJson("http://openapi.gegejia.com:8902/api/order/findOrders", parm, sign);
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
        //2018-03-18 罗勤瑶 修改
        void Map(JToken trade, UpGradeOrder order)
        {

            //第三方买家订单信息
            order.MallOrderBuyer = new MallOrderBuyerInfo()
            {
                BuyerNick = trade["receiver"]["receiverName"].ToString(),
                MallOrderId = trade["number"].ToString(),
                //格格家无买家留言、卖家备注
                BuyerMessage = "",
                SellerMessage = ""
            };

            order.UpGradeOrderItems = new List<UpGradeOrderItem>();
            var orders = trade["items"];

            for (int i = 0; i < orders.Count(); i++)
            {
                var item = orders[i];
                var code = item["itemCode"].ToString();
                var code2 = "";
                //第三方商品编码统一格式 商品编号*数量-商品类型:商品单价，作为拆分条件罗勤尧
                var pcs = "1";
                //取出商品名字里面的规格数量
                var itemName = item["itemName"].ToString();
                var count = 1;
                if (itemName.Contains("*"))
                {
                    int index = itemName.IndexOf("*");
                    string result = itemName.Substring(index + 1,1);
                    int.TryParse(result, out count);
                    if (count==0)
                    {
                        count = 1;
                    }
                }else
                {
                    count =1;
                }

                int x = itemName.IndexOf("【") + 1;
                int j = itemName.IndexOf("】");
                string str = itemName.Substring(i, j - i+1);
                string con = str.Substring(1, 1);
                int.TryParse(con, out count);
                if (count == 0)
                {
                    count = 1;
                }
                var type=0;
                var price = 0m;
                if (code.Contains(":"))
                {
                    var c = code.Split(':')[0];
                    if (c.Contains("*"))
                    {
                        if (c.Split('*').Length > 1 && c.Split('*')[1].Contains("-"))
                        {
                            pcs = c.Split('*')[1].Split('-')[0];
                            if (c.Split('*')[1].Split('-').Length > 1 && !string.IsNullOrWhiteSpace(c.Split('*')[1].Split('-')[1]))
                            {
                                if (c.Split('*')[1].Split('-')[1] == "1")
                                {
                                    type = 1;
                                }
                                else if (c.Split('*')[1].Split('-')[1] == "2")
                                {
                                    type = 2;
                                }
                                else if (c.Split('*')[1].Split('-')[1] == "3")
                                {
                                    type = 3;
                                }
                            }
                        }
                        else if (c.Split('*').Length > 1 && !c.Split('*')[1].Contains("-"))
                        {
                            pcs = c.Split('*')[1];
                        }
                        code2 = c.Split('*')[0];                        
                    }else
                    {
                        code2 = c;
                    }
                    if (!string.IsNullOrWhiteSpace(code.Split(':')[1]))
                    {
                        price = decimal.Parse(code.Split(':')[1]);
                    }
                   // code2 = code;
                }
                else if (code.Contains("*"))
                {
                    code2 = code.Split('*')[0];
                }else
                {
                    code2 = code;
                }
                //系统商品信息
               var product =IPdProductDao.Instance.GetProductByErpCode(code2);
               if (product!=null)
                {
                    var pricesel = 0m;
                    if (count==1)
                   {
                       pricesel = product.GeGeJiaSupplyPrice;
                   }
                    else if (count == 2)
                    {
                        pricesel = product.GeGeJiaSupplyPriceSpecTwo;
                    }
                    else if (count==3)
                    {
                       pricesel = product.GeGeJiaSupplyPriceSpecThree;
                   }
                    order.UpGradeOrderItems.Add(new UpGradeOrderItem()
                    {
                        MallProductCode = code2,
                        MallProductName = item["itemName"].ToString(),
                        MallProductAttrId = item["itemCode"].ToString(),
                        MallProductAttrs = item["itemName"].ToString(),
                        OrderId = trade["number"].ToString(),
                        MallPrice = pricesel / count,
                        MallAmount = pricesel * int.Parse(item["itemCount"].ToString()),
                        // Quantity = int.Parse(item["itemCount"].ToString()) * int.Parse(pcs),
                        Quantity = int.Parse(item["itemCount"].ToString()) * count,
                        DiscountFee = decimal.Parse(trade["couponPrice"].ToString()),
                        // MallOrderItemId = item["itemCode"].ToString(),
                        MallOrderItemId = "",
                        ProductType = type,
                        ProductSalesType = 91
                    });
                }else
               {
                   order.UpGradeOrderItems.Add(new UpGradeOrderItem()
                   {
                       MallProductCode = code2,
                       MallProductName = item["itemName"].ToString(),
                       MallProductAttrId = item["itemCode"].ToString(),
                       MallProductAttrs = item["itemName"].ToString(),
                       OrderId = trade["number"].ToString(),
                       MallPrice = price / count,
                       MallAmount = price * int.Parse(item["itemCount"].ToString()),
                       // Quantity = int.Parse(item["itemCount"].ToString()) * int.Parse(pcs),
                       Quantity = int.Parse(item["itemCount"].ToString()) * count,
                       DiscountFee = decimal.Parse(trade["couponPrice"].ToString()),
                       // MallOrderItemId = item["itemCode"].ToString(),
                       MallOrderItemId = "",
                       ProductType = type,
                       ProductSalesType = 91
                   });
               }
                

            }

            //第三方订单交易信息
            order.MallOrderPayment = new MallOrderPaymentInfo()
            {
                Payment = decimal.Parse(trade["realPrice"].ToString()),
                AlipayNo = "",
                PostFee = decimal.Parse(trade["freight"].ToString()),
                DiscountFee = decimal.Parse(trade["couponPrice"].ToString()),
                PayTime = DateTime.Parse(trade["payTime"].ToString() == "" ? "1900-01-01" : trade["payTime"].ToString())
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
                City = trade["receiver"]["cityName"].ToString(),
                Province = trade["receiver"]["provinceName"].ToString(),
                District = trade["receiver"]["districtName"].ToString(),
                ReceiveAddress = trade["receiver"]["detailAddress"].ToString(),
                ReceiveContact = trade["receiver"]["receiverName"].ToString(),
                Mobile = trade["receiver"]["receiverMobile"].ToString(),
                PostCode = trade["receiver"]["districtCode"].ToString(),
                IdCard = trade["receiver"]["receiverIdCard"].ToString(),
            };

            //订单备注标示
            //order.MallOrderBuyer.SellerFlag = trade["seller_flag"].ToString();
        }
        #endregion

        #region 格格家获取单笔订单详情
        /// <summary>
        /// 获取单笔订单详情
        /// </summary>
        /// <param name="param">参数实体</param>
        /// <param name="auth">授权参数</param>
        /// <returns>单笔订单详情</returns>
        /// <reamrks>2017-08-18 罗勤瑶 创建</reamrks>
        public Result<UpGradeOrder> GetOrderDetail(OrderParameters param, AuthorizationParameters auth)
        {
            //};
            var dealerInfo = new HytOrderDealerInfo()
            {
                DealerSysNo = auth.DealerMall.DealerSysNo, //
                DealerMallSysNo = auth.DealerMall.SysNo, //
                HytPayStatus = 20,
                IsSelfSupport = auth.DealerMall.IsSelfSupport, //
            };
            var result = new Result<UpGradeOrder> { Status = true, Message = "", Data = null, StatusCode = 0 };
            var order = new UpGradeOrder();
            var _param = new Dictionary<string, string>();
            param.PageSize = 999;
            param.PageIndex = 1;


            //请求的参数类调用
            GeGeJiaParameter ggjParameter = new GeGeJiaParameter();

        ///格格家没有单个订单的接口
        ///
            //string parm = "{\"params\":{\"startTime\":\"" + param.StartDate.ToString("yyyy-MM-dd HH:mm:ss") + "\",\"endTime\":\"" + param.EndDate.ToString("yyyy-MM-dd HH:mm:ss") + "\",\"page\":" + param.PageIndex + ", \"pageSize\":" + param.PageSize
            //+ ", \"status\":2},\"partner\":\"" + auth.DealerApp.AppSecret + "\",\"timestamp\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\"}";


            return null;
        }
        #endregion

        #region 格格家联系发货（线下物流）
        /// <summary>
        /// 用户调用该接口可实现自己联系发货（线下物流）
        /// </summary>
        /// <param name="param">参数实体</param>
        /// <param name="auth">授权参数</param>
        /// <returns>处理结果</returns>
        /// <reamrks>2018-04-10 罗勤瑶 创建</reamrks>
        public Result SendDelivery(DeliveryParameters param, AuthorizationParameters auth)
        {
            var result = new Result() { Status = true };
            var _param = new Dictionary<string, string>();

            //海外购订单发货
            GeGeJiaDeliverGoods req = new GeGeJiaDeliverGoods();
            //订单ID
            req.orderNumber = param.MallOrderId;
            //物流公司名称
            req.expressName = param.CompanyCode;
            //运单号
            req.expressNo = param.HytExpressNo;

            //订单类型，0：渠道订单，1：格格家订单，2：格格团订单，3：格格团全球购订单，4：环球捕手订单，5：燕网订单，6：b2b订单，7：手q，8：云店
            string parm = "{\"params\":{\"type\":1,\"orderNumber\":\"" + req.orderNumber + "\",\"expressName\":\"" + req.expressName + "\",\"expressNo\":\"" + req.expressNo + "\"},\"partner\":\"" + auth.DealerApp.AppSecret + "\",\"timestamp\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\"}";

            //MD5加密签名
            string sign = MD5Encrypt(auth.DealerApp.AppKey + parm + auth.DealerApp.AppKey).ToUpper();

            var response = PostJson("http://openapi.gegejia.com:8902/api/order/sendOrder", parm, sign);

            var _response = JObject.Parse(response.ToString());

            if (_response.Property("errMsg") != null)
            {
                result.Status = false;
                result.Message = _response.Property("errMsg").ToString();
                result.errCode =_response.Property("errCode").ToString();
            }
            return result;
        }
        #endregion

        #region 格格家获取可合并升舱订单列表
        /// <summary>
        /// 获取可合并升舱订单列表
        /// </summary>
        /// <param name="param">参数实体</param>    
        /// <param name="auth">授权参数</param>
        /// <returns>可合并升舱订单列表</returns>
        /// <reamrks>2017-08-18 黄杰 创建</reamrks>
        public Result<List<UpGradeOrder>> GetCombineOrders(OrderParameters param, AuthorizationParameters auth)
        {
            return null;
        }
        #endregion

        #region 格格家使用授权码获取登录令牌
        /// <summary>
        /// 使用授权码获取登录令牌
        /// </summary>
        /// <param name="code">授权码</param>
        /// <returns>登录令牌</returns>
        /// <reamrks>2017-08-18 黄杰 创建</reamrks>
        public Result<AccessTokenResult> GetAuthorizationCode(string code)
        {
            return null;
        }
        #endregion

        #region 格格家更新订单备注信息
        /// <summary>
        /// 更新订单备注信息
        /// </summary>
        /// <param name="remarks">备注内容</param>
        /// <param name="auth">授权参数</param>
        /// <reamrks>2017-08-18 黄杰 创建</reamrks>
        public Result UpdateTradeRemarks(IRemarksParameters remarks, AuthorizationParameters auth)
        {
            return null;
        }
        #endregion


        /// <summary>
        ///获取第三方支持的快递
        /// </summary>
        /// <param name="auth">授权参数</param>
        /// <returns>2018-03-25 罗勤尧</returns>
        public Result<UpGradeExpress> GetExpress(AuthorizationParameters auth)
        {
            var result = new Result<UpGradeExpress>() { Status = true };
            var _param = new Dictionary<string, string>();

            //海外购订单发货
            GeGeJiaDeliverGoods req = new GeGeJiaDeliverGoods();
          
            //订单类型，0：渠道订单，1：格格家订单，2：格格团订单，3：格格团全球购订单，4：环球捕手订单，5：燕网订单，6：b2b订单，7：手q，8：云店
            string parm = "{\"params\":{},\"partner\":\"" + auth.DealerApp.AppSecret + "\",\"timestamp\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\"}";

            //MD5加密签名
            string sign = MD5Encrypt(auth.DealerApp.AppKey + parm + auth.DealerApp.AppKey).ToUpper();

            var response = PostJson("http://openapi.gegejia.com:8902/api/express/names", parm, sign);
            var Assreturn = JsonSerializationHelper.JsonToObject<UpGradeExpress>(response);
            var _response = JObject.Parse(response.ToString());
            //var names = _response.Property("names");
            if (_response.Property("errMsg") != null)
            {
                result.Status = false;
                result.Message = _response.Property("errMsg").ToString();
                result.errCode = _response.Property("errCode").ToString();
            }else
            {
                result.Data = Assreturn;
                result.Message = "获取成功";
            }
            return result;
        }
    }
    #endregion
}
