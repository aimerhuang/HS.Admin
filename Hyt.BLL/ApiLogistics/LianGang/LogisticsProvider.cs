using Hyt.BLL.Order;
using Hyt.Model;
using Hyt.Model.Order;
using Hyt.Model.Parameter;
using Hyt.Model.SystemPredefined;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Util;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Hyt.BLL.ApiLogistics.LianGang
{
    /// <summary>
    /// 联港物流
    /// </summary>
    /// <remarks>2016-8-19 杨浩 添加注释</remarks>
    public class LogisticsProvider : ILogisticsProvider
    {
        #region 测试环境
        string Email = "zhaq@upl.com";
        string Password = "Z7_aLqs5";
        #endregion

        string CustomerCode = config.CustomerCode;
        string Key = config.AppKey;
        string Token = config.Token;
        string RequestUrl = "http://m.upllogistics.com.cn/";
        /// <summary>
        /// 身份凭证,调用接口前需要先获取此凭证
        /// </summary>
        private static string IdentifyPass = string.Empty;
        private static object _lockObject = new object();
     
   
        public LogisticsProvider() { }


        /// <summary>
        /// 物流标示
        /// </summary>
        public override Hyt.Model.CommonEnum.物流代码 Code
        {
            get { return Hyt.Model.CommonEnum.物流代码.联港物流; }
        }
        /// <summary>
        /// 获取支付方式英文代码
        /// </summary>
        /// <param name="payTypeSysNo">支付类型编号</param>
        /// <returns></returns>
        /// <remarks>2016-8-20 杨浩 创建</remarks>
        private string GetPayType(int payTypeSysNo)
        {
            if (payTypeSysNo == (int)PaymentType.支付宝)
                return "alipay";
            else if (payTypeSysNo == (int)PaymentType.微信支付)
                return "weixin";
            else if(payTypeSysNo==(int)PaymentType.通联支付)
                return "allinpay";

            return "allinpay";
        }
        public override Result GetProduct(string productId)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 设置登录凭证
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-8-20 杨浩 添加注释</remarks>
        public void LogisticsLogin()
        {            
            if (string.IsNullOrEmpty(IdentifyPass))
            {
                lock (_lockObject)
                {
                    string url = RequestUrl + "ClientApi/GetToken";
                    url += "?Email=" + Email + "&Password=" + Password;

                    HttpWebRequest request = WebUtil.GetWebRequest(url, "GET");
                    request.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
                    request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                    request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.111 Safari/537.36";
                    string respStr = "";

                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                        {
                            respStr = reader.ReadToEnd();
                        }
                    }

                    var result = new Result<string>();
                    GetResult(JObject.Parse(respStr), result);

                    if (result.Status == true)                    
                        IdentifyPass = result.Data;                   
                }
            }
        }

        /// <summary>
        /// 添加交易订单
        /// </summary>
        /// <param name="orderSysno"></param>
        /// <returns>
        /// 2016-04-09 陈海裕 创建
        /// 2016-08-20 杨浩   重构
        /// </returns>
        public override Result AddOrderTrade(int orderSysno)
        {
            LogisticsLogin();  

            var result = new Result<string>();
            result.Status = false;
            result.StatusCode = 0;
            result.Message = "接口调用失败";
            string url = RequestUrl + "Express/uploadOrders";

            if (orderSysno <= 0)
            {
                return result;
            }

            try
            {
                var order = BLL.Order.SoOrderBo.Instance.GetEntity(orderSysno);
                if (order == null)
                {
                    result.Message = "该订单不存在";
                    return result;
                }
              
                order.ReceiveAddress = BLL.Order.SoOrderBo.Instance.GetOrderReceiveAddress(order.ReceiveAddressSysNo);
                order.OrderItemList = DataAccess.Order.ISoOrderItemDao.Instance.GetOrderItemsByOrderSysNo(order.SysNo);
                var voucherFilter = new ParaVoucherFilter();
                voucherFilter.SourceSysNo = order.SysNo;
                var recVoucher = BLL.Finance.FinanceBo.Instance.GetReceiptVouchers(voucherFilter).Rows.FirstOrDefault();
                recVoucher.VoucherItems = BLL.Finance.FnReceiptVoucherBo.Instance.GetReceiptVoucherItem(recVoucher.SysNo);
                // 收货人 区 市 省
                BsArea receiverDistrict = BLL.Basic.BasicAreaBo.Instance.GetArea(order.ReceiveAddress.AreaSysNo);
                BsArea receiverCity = BLL.Basic.BasicAreaBo.Instance.GetArea(receiverDistrict.ParentSysNo);
                BsArea receiverProvince = BLL.Basic.BasicAreaBo.Instance.GetArea(receiverCity.ParentSysNo);
                // 发货人 市
                CBWhWarehouse warehouse = BLL.Warehouse.WhWarehouseBo.Instance.GetWarehouse(order.DefaultWarehouseSysNo);
                BsArea shipperDistrict = BLL.Basic.BasicAreaBo.Instance.GetAreaList(warehouse.CitySysNo).FirstOrDefault();
                BsArea shipperCity = BLL.Basic.BasicAreaBo.Instance.GetArea(warehouse.CitySysNo);
                BsArea shipperProvince = BLL.Basic.BasicAreaBo.Instance.GetArea(warehouse.ProvinceSysNo);

                DsDealer dealer = BLL.Distribution.DsDealerBo.Instance.GetDsDealer(order.DealerSysNo);

                LgDeliveryType deliveryType = BLL.Logistics.DeliveryTypeBo.Instance.GetDeliveryType(order.DeliveryTypeSysNo);

                ParaOnlinePaymentFilter filter = new ParaOnlinePaymentFilter();
                filter.OrderSysNo = orderSysno;
                var onlinePayment = BLL.Finance.FinanceBo.Instance.GetOnlinePayments(filter).Rows.Where(o => o.Status == 1).FirstOrDefault();
                Regex regex = new Regex("\t|\n|\r");

                OrderList newOrderList = new OrderList();
                newOrderList.PrintType = "@1,@2";
                newOrderList.list = new List<LGOrder>();
                LGOrder newOrder = new LGOrder();
                newOrder.Addr = "中国," + receiverProvince.AreaName + "," + receiverCity.AreaName + "," + receiverDistrict.AreaName + "," + order.ReceiveAddress.StreetAddress;
                newOrder.Addr = regex.Replace(newOrder.Addr,"");
                newOrder.CardNo = order.ReceiveAddress.IDCardNo;
                newOrder.Create_Addr = "中国," + shipperProvince.AreaName + "," + shipperCity.AreaName + "," + shipperDistrict.AreaName + ",";
                newOrder.Create_Addr = regex.Replace(newOrder.Create_Addr, "");
                newOrder.Create_CardNo = "";
                newOrder.Create_Name = "澳门爱勤";
                newOrder.Create_Phone = !string.IsNullOrWhiteSpace(dealer.MobilePhoneNumber) ? dealer.MobilePhoneNumber : dealer.PhoneNumber;
                newOrder.id = order.SysNo.ToString();
                newOrder.Name = order.ReceiveAddress.Name;
                newOrder.OrderType = "A";
                newOrder.PayCardNo = order.ReceiveAddress.IDCardNo;
                newOrder.PayCurrency = "CNY";
                newOrder.PayName = order.ReceiveAddress.Name;
                newOrder.PayNo = recVoucher.VoucherItems[0].VoucherNo;
                newOrder.PayPhone = !string.IsNullOrWhiteSpace(order.ReceiveAddress.MobilePhoneNumber) ? order.ReceiveAddress.MobilePhoneNumber : order.ReceiveAddress.PhoneNumber;
                newOrder.PayRemark = "";
                newOrder.PayTime = order.CreateDate.ToString();
                newOrder.PayType = GetPayType(order.PayTypeSysNo);// "alipay";
                newOrder.PayUser = order.CustomerSysNo.ToString();
                newOrder.Phone = newOrder.PayPhone;
                newOrder.Price = order.OrderAmount;
                newOrder.Remark = order.DeliveryRemarks;
                newOrder.TaxFee = order.TaxFee;
                newOrder.TranFee = order.FreightAmount;
                newOrder.Content = "";
                foreach (var item in order.OrderItemList)
                {
                    var productStock = BLL.Warehouse.PdProductStockBo.Instance.GetEntityByWP(order.DefaultWarehouseSysNo, item.ProductSysNo);
                    newOrder.Content += productStock.Barcode + "," + item.Quantity + "," + item.SalesUnitPrice + "|";
                }
                if (!string.IsNullOrWhiteSpace(newOrder.Content))
                {
                    newOrder.Content = newOrder.Content.Substring(0, newOrder.Content.Length - 1);
                }
                newOrderList.list.Add(newOrder);

                Dictionary<string, string> paramsData = new Dictionary<string, string>();
                paramsData.Add("token", IdentifyPass);
                paramsData.Add("postdata", Util.Serialization.JsonUtil.ToJson(newOrderList));
                string postData = InitParams(paramsData);
                string responseStr = GetResponse(url, postData);


                var jObject = JObject.Parse(responseStr);


                var soOrderSyncLogisticsLog = new SoOrderSyncLogisticsLog();

                soOrderSyncLogisticsLog.OrderSysNo = orderSysno;
                soOrderSyncLogisticsLog.Code = (int)this.Code;

                soOrderSyncLogisticsLog.LastUpdateDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                soOrderSyncLogisticsLog.StatusCode = "";
                soOrderSyncLogisticsLog.StatusMsg = "";
                soOrderSyncLogisticsLog.Packets = postData;
                soOrderSyncLogisticsLog.ReceiptContent = responseStr;

                string status = jObject["status"].ToString();

                if (status == "-1")
                {
                    result.Status = false;
                    result.Message = "未登录，请重试！";
                    IdentifyPass = string.Empty;//清空身份凭证
                }
                else if (status == "1")
                {               
                    for(int i=0;i<jObject["data"].Count();i++)
                    {
                        if (jObject["data"][i]["status"].ToString() == "1")
                        {
                            soOrderSyncLogisticsLog.LastUpdateBy = 0;
                            soOrderSyncLogisticsLog.LogisticsOrderId = jObject["data"][i]["id"].ToString();
                            soOrderSyncLogisticsLog.CreateDate = DateTime.Now;
                            soOrderSyncLogisticsLog.CreatedBy = BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo;
                            SoOrderSyncLogisticsLogBo.Instance.Insert(soOrderSyncLogisticsLog);
                            BLL.Order.SoOrderBo.Instance.UpdateOrderApiStatus((int)Hyt.Model.WorkflowStatus.OrderStatus.跨境物流推送状态.已推送, 3, orderSysno);
                            result.Status = true;
                            result.Message = "推单成功";
                        }
                        else
                        {
                            soOrderSyncLogisticsLog.CreateDate = DateTime.Now;
                            SoOrderSyncLogisticsLogBo.Instance.Insert(soOrderSyncLogisticsLog);
                            result.Status = false;
                            result.Message = jObject["data"][i]["message"].ToString();
                        }
                    }
                    
                }
                else
                {
                    try
                    {
                        SoOrderSyncLogisticsLogBo.Instance.Insert(soOrderSyncLogisticsLog);
                    }
                    catch { }
                    
                    result.Status = false;
                    result.Message =jObject["message"].ToString();
                }
            }
            catch (Exception ex)
            {
                result.Message = "向" + this.Code + "物流推送订单报错：" + ex.StackTrace;
                return result;
            }

            return result;
        }

        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="orderSysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-07-05 陈海裕 创建</remarks>
        public override Result CancelOrderTrade(int orderSysNo, string reason = "")
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 查询订单物流信息
        /// </summary>
        /// <param name="orderSysNo">订单号</param>
        /// <returns></returns>
        /// <remarks>2016-04-09 陈海裕 创建</remarks>
        public override Result<string> GetLogisticsTracking(int orderSysNo)
        {
            LogisticsLogin();

            Result<string> result = new Result<string>();
            result.Status = false;
            result.StatusCode = 0;
            result.Message = "接口调用失败";
            string url = RequestUrl + "Express/GetmyExpress";

            if (orderSysNo <= 0)
            {
                return result;
            }

            try
            {
                Dictionary<string, string> paramsData = new Dictionary<string, string>();
                paramsData.Add("token", IdentifyPass);
                paramsData.Add("kwd", orderSysNo.ToString());
                paramsData.Add("pc", "1");
                paramsData.Add("ps", "100");
                paramsData.Add("status", "10");
                paramsData.Add("grade", "40");

                string responseStr = GetResponse(url,InitParams(paramsData));
                result = GetResult(JObject.Parse(responseStr), result);

                if (result.Status == true)
                {
                    result.Status = true;
                    result.StatusCode = 1;
                    result.Message = "接口调用成功";

                    List<TrackingData> trackingData = Util.Serialization.JsonUtil.ToObject<List<TrackingData>>(result.Message);
                    StringBuilder htmlStr = new StringBuilder();
                    for (int i = trackingData.Count - 1; i >= 0; i--)
                    {
                        htmlStr.Append("<tr><td width=\"25%\">" + trackingData[i].CreateTime + "</td><td width=\"75%\">" + trackingData[i].PackageCode + "</td></tr>");
                    }

                    result.Data = htmlStr.ToString();
                }

                return result;
            }
            catch (Exception ex)
            {
                result.Message = "查询订单物流状态报错：" + ex.StackTrace;
                return result;
            }
        }

       

        public override Result GetOrderExpressno(string orderId)
        {
            throw new NotImplementedException();
        }

        public override Result GetOrderTrade(string orderId)
        {
            throw new NotImplementedException();
        } 
        
        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="paramsData">参数集合</param>
        /// <returns></returns>
        /// <remarks>2016-8-20 杨浩 创建</remarks>
        private string InitParams(Dictionary<string,string> paramsData)
        {
            paramsData = paramsData.OrderBy(o => o.Key).ToDictionary(k => k.Key, v => v.Value);
            string timerstamp = CreateTimeStamp();
            string postData = "";
            foreach (var i in paramsData)
            {
                postData += i.Key + "=" + i.Value + "&";
            }
            if (postData.Length > 0)
            {
                postData = postData.Substring(0, postData.Length - 1);
            }
            return postData;
        }

        private string GetResponse(string url,string postData)
        {
            //paramsData = paramsData.OrderBy(o => o.Key).ToDictionary(k => k.Key, v => v.Value);
            //string timerstamp = CreateTimeStamp();
            //string postData = "";
            //foreach (var i in paramsData)
            //{
            //    postData += i.Key + "=" + i.Value + "&";
            //}
            //if (postData.Length > 0)
            //{
            //    postData = postData.Substring(0, postData.Length - 1);
            //}

            byte[] data = Encoding.UTF8.GetBytes(postData);

            // 记录推送前参数
            //BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "物流接口" + this.Code + ",url:" + url + "，Before Post：\n" + postData, LogStatus.系统日志目标类型.订单, _orderSysNo, 0);

            HttpWebRequest request = WebUtil.GetWebRequest(url + "?" + postData, "post");

            request.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            request.ContentLength = data.Length;
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.111 Safari/537.36";
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            string respStr = "";
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    respStr = reader.ReadToEnd();
                }
            }

            // 记录推送后返回结果
            //BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "物流接口" + this.Code + ",url:" + url + "，Response：\n" + JObject.Parse(respStr),
             //   LogStatus.系统日志目标类型.订单, _orderSysNo, 0);

            return respStr;
        }

        private Result<string> GetResult(JObject back, Result<string> result)
        {
            if (back.Property("status") != null && back["status"].ToString() == "1")
            {
                result.Status = true;
                result.StatusCode = 1;
                result.Message = "接口调用成功";
                result.Data = back["data"].ToString();
            }
            else
            {
                result.Message += back["message"].ToString();
            }

            return result;
        }

        public string CreateDigest(string postData, string timerStamp)
        {
            Byte[] bytes = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(postData + Key + timerStamp));

            string ret = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                ret += Convert.ToString(bytes[i], 16).PadLeft(2, '0');
            }

            return ret.PadLeft(32, '0').ToUpper();
        }

        public string CreateTimeStamp(DateTime? oTime = null)
        {
            DateTime _time = oTime == null ? DateTime.Now : (DateTime)oTime;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return ((int)(_time - startTime).TotalSeconds).ToString();
        }
        private class TrackingData
        {
            /// <summary>
            /// 订单序号 VARCHAR(30) N
            /// </summary>
            public string id { get; set; }
            /// <summary>
            /// 收件人 VARCHAR(30) N
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// 电话，显示后四位 VARCHAR(13) N
            /// </summary>
            public string Phone { get; set; }
            /// <summary>
            /// 地址 VARCHAR(120) N
            /// </summary>
            public string Addr { get; set; }
            /// <summary>
            /// 身份证号，只显示后面四位 VARCHAR(20) N
            /// </summary>
            public string CardNo { get; set; }
            /// <summary>
            /// 货品描述信息,以"货品名 数量,货品名 数量"方式描述 ARCHAR(MAX) N
            /// </summary>
            public string Content { get; set; }
            /// <summary>
            /// 订单状态值，状态值参考文档中对订单状态的描述表5 VARCHAR(2) N
            /// </summary>
            public string Stat { get; set; }
            /// <summary>
            /// 状态描述信息 VARCHAR(120) y
            /// </summary>
            public string StatDisc { get; set; }
            /// <summary>
            /// 订单提交人 VARCHAR(120) N
            /// </summary>
            public string Creater { get; set; }
            /// <summary>
            /// 订单提交时间 VARCHAR(20) N
            /// </summary>
            public string CreateTime { get; set; }
            /// <summary>
            /// 物品打包人 VARCHAR(30) N
            /// </summary>
            public string Packager { get; set; }
            /// <summary>
            /// 物品打包时间 VARCHAR(20) N
            /// </summary>
            public string PackageTime { get; set; }
            /// <summary>
            /// 快递运单号 VARCHAR(30) N
            /// </summary>
            public string PackageCode { get; set; }
            /// <summary>
            /// 快递操作人,快递公司人员 VARCHAR(30) N
            /// </summary>
            public string Traner { get; set; }
            /// <summary>
            /// 快递收包时间 VARCHAR(20) N
            /// </summary>
            public string TranTime { get; set; }
            /// <summary>
            /// 快递公司 VARCHAR(60) N
            /// </summary>
            public string TranCompany { get; set; }
            /// <summary>
            /// 订单号,此订单号为联港物流系统中的运单号 VARCHAR(30) N
            /// </summary>
            public string OrderNumber { get; set; }
            /// <summary>
            /// 发件人 VARCHAR(60) N
            /// </summary>
            public string Create_Name { get; set; }
            /// <summary>
            /// 发件人电话 VARCHAR(13) N
            /// </summary>
            public string Create_Phone { get; set; }
            /// <summary>
            /// 发件人地址 VARCHAR(120) N
            /// </summary>
            public string Create_Addr { get; set; }
            /// <summary>
            /// 对应电商平台的唯一订单编号 VARCHAR(36) N
            /// </summary>
            public string QRCode { get; set; }
            /// <summary>
            /// 备注 VARCHAR(120) Y
            /// </summary>
            public string Remark { get; set; }
            /// <summary>
            /// 状态消息，失败时的消息 VARCHAR(120) Y
            /// </summary>
            public string message { get; set; }
        }
        public class OrderList
        {
            public string PrintType { get; set; }
            public List<LGOrder> list { get; set; }
        }

        public class LGOrder
        {
            /// <summary>
            /// 对应电商平台的唯一订单编号 VARCHAR(30) N
            /// </summary>
            public string id { get; set; }
            /// <summary>
            /// 收件人 VARCHAR (20) N
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// 收件人电话号码 VARCHAR(13) N
            /// </summary>
            public string Phone { get; set; }
            /// <summary>
            /// 收件人地址（格式要严格按照此要求，以英文逗号分成5段，区域划分请参考海关区域划分表） VARCHAR(MAX) N
            /// </summary>
            public string Addr { get; set; }
            /// <summary>
            /// 收件人身份证号 VARCHAR(20) N
            /// </summary>
            public string CardNo { get; set; }
            /// <summary>
            /// 发件人 VARCHAR(60) N
            /// </summary>
            public string Create_Name { get; set; }
            /// <summary>
            /// 发件人电话 VARCHAR(13) N
            /// </summary>
            public string Create_Phone { get; set; }
            /// <summary>
            /// 发件人地址（格式要严格按照此要求，以英文逗号分成5段，区域划分请参考海关区域划分表） VARCHAR(MAX) N
            /// </summary>
            public string Create_Addr { get; set; }
            /// <summary>
            /// 发件人身份证号 VARCHAR(36)
            /// </summary>
            public string Create_CardNo { get; set; }
            /// <summary>
            /// 订单货品：多个货品以“|”分隔，单个货品格式：“货品型号或条码,货品数量,单价"，货品型号或条码需已提交海关备案，条码大于10位 VARCHAR(MAX) N
            /// </summary>
            public string Content { get; set; }
            /// <summary>
            /// 订单类型："A"表示BC海外直邮订单，“B”表示BBC保税仓发货订单 VARCHAR(10) N
            /// </summary>
            public string OrderType { get; set; }
            /// <summary>
            /// 支付单号 VARCHAR(60) N
            /// </summary>
            public string PayNo { get; set; }
            /// <summary>
            /// 支付方式："alipay"为支付宝,"weixin"为微信支付,"allinpay"为通联支付。（其他支付方式请以支付公司英文代码上传，具体与物流公司技术支持沟通） VARCHAR(20) N
            /// </summary>
            public string PayType { get; set; }
            /// <summary>
            /// 货款总额（货品单价*数量总和）,为空或零时系统自动计算 Decimal(10,2) N
            /// </summary>
            public decimal Price { get; set; }
            /// <summary>
            /// 总税费（货款总税+运费税费之和） Decimal(10,2) N
            /// </summary>
            public decimal TaxFee { get; set; }
            /// <summary>
            /// 运费，默认为零 Decimal(10,2) N
            /// </summary>
            public decimal TranFee { get; set; }
            /// <summary>
            /// 订单抵扣金额，优惠金额 Decimal(10,2) N
            /// </summary>
            public decimal Fee { get; set; }
            /// <summary>
            /// 订单支付完成时间 DateTime
            /// </summary>
            public string PayTime { get; set; }
            /// <summary>
            /// 订单货币类型：“CNY"为人民币，默认为人民币 VARCHAR(10)
            /// </summary>
            public string PayCurrency { get; set; }
            /// <summary>
            /// 下单用户名，电商平台下单账号，海关总署版接口要求 VARCHAR(50) N
            /// </summary>
            public string PayUser { get; set; }
            /// <summary>
            /// 下单人真实姓名，海关总署版接口要求 VARCHAR(30) N
            /// </summary>
            public string PayName { get; set; }
            /// <summary>
            /// 下单人联系电话，海关总署版接口要求 VARCHAR(30) N
            /// </summary>
            public string PayPhone { get; set; }
            /// <summary>
            /// 下单人身份证号，海关总署版接口要求 VARCHAR(50) N
            /// </summary>
            public string PayCardNo { get; set; }
            /// <summary>
            /// 下单备注
            /// </summary>
            public string PayRemark { get; set; }
            /// <summary>
            /// 订单备注
            /// </summary>
            public string Remark { get; set; }
        }
    }
}
