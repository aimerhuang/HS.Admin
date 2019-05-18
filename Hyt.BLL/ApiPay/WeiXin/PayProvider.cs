using Hyt.BLL.ApiPay.AliPay;
using Hyt.BLL.Finance;
using Hyt.BLL.Order;
using Hyt.Model;
using Hyt.Model.Common;
using Hyt.Model.Icp.GZNanSha;
using Hyt.Model.Logis.XinYi;
using Hyt.Model.Manual;
using Hyt.Model.WorkflowStatus;
using Hyt.Util;
using Hyt.Util.Tenpay;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml;
using System.Security.Cryptography;
using Hyt.Util.Xml;

namespace Hyt.BLL.ApiPay.WeiXin
{
    /// <summary>
    /// 微信报关接口
    /// </summary>
    /// <remarks>2015-10-12 杨浩 创建</remarks>
    public class PayProvider:IPayProvider
    {
        public PayProvider() { }
        private TenpayCustomsConfig customsConfig = Hyt.BLL.Config.Config.Instance.GetTenpayCustomsConfig();       
        #region 属性  
        /// <summary>
        /// 支付企业代码
        /// </summary>
        /// <remarks>2015-12-26 杨浩 创建</remarks>
        public override CommonEnum.PayCode Code
        {
            get{return CommonEnum.PayCode.微信;}            
        }
        /// <summary>
        /// 随机串包含字母或数字     
        /// </summary>
        /// <remarks>2016-12-21 杨浩 创建</remarks>
        private string GenerateNonceStr
        {
            get
            {
                return Guid.NewGuid().ToString().Replace("-", "");
            }
        }             

        /// <summary>
        /// 时间戳，标准北京时间，时区为东八区，自1970年1月1日 0点0分0秒以来的秒数
        /// </summary>
        ///<remarks>2016-12-21 杨浩 创建</remarks>
        public  string GenerateTimeStamp
        {
            get
            {
                TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                return Convert.ToInt64(ts.TotalSeconds).ToString();
            }       
        }
        #endregion

        /// <summary>
        /// 海关报关
        /// </summary>
        /// <returns></returns>
        /// <remarks>2015-10-12 杨浩 创建</remarks>
        public override Result ApplyToCustoms(SoOrder soorder)
        {
            Result result =new Result();
            try
            {
                IList<FnOnlinePayment> list = FinanceBo.Instance.GetOnlinePaymentList(soorder.SysNo);
                FnOnlinePayment payment;
                if (list.Count > 0)
                {
                    payment=list[0];
                }
                else
                {
                    result.Status=false;
                    result.Message="付款单信息无效，请核实订单是否付款？";
                    return result;
                }

                Hyt.Model.Manual.SoReceiveAddressMod address = SoOrderBo.Instance.GetOrderReceiveAddress2(soorder.ReceiveAddressSysNo);

                RequestHandler reqHandler = new RequestHandler(System.Web.HttpContext.Current, customsConfig.key, customsConfig.GATEWAY_NEW);
                reqHandler.init();
                //reqHandler.setGateUrl(url);
                reqHandler.setParameter("order_fee", ((int)(soorder.OrderAmount * 100)).ToString());			     //商品总金额,以分为单位
                reqHandler.setParameter("product_fee", ((int)((soorder.ProductAmount + (soorder.TaxFee)) * 100)).ToString());	 //商品金额,以分为单位
                reqHandler.setParameter("transport_fee", ((int)(soorder.FreightAmount * 100)).ToString());	         //运费金额,以分为单位
                reqHandler.setParameter("duty", ((int)(soorder.TaxFee * 100)).ToString());
                //协议参数
                reqHandler.setParameter("sign_type", "MD5");
                reqHandler.setParameter("service_version", "1.0");
                reqHandler.setParameter("input_charset", "UTF-8");
                reqHandler.setParameter("sign_key_index", "1");
                // 设置支付参数
                //-----------------------------
                reqHandler.setParameter("partner", customsConfig.partner);		        //商户号

                string out_trade_no = "";
                if (string.IsNullOrEmpty(payment.BusinessOrderSysNo))
                {
                    out_trade_no = payment.SourceSysNo.ToString();
                }
                else
                {
                    out_trade_no = payment.BusinessOrderSysNo;
                }

                reqHandler.setParameter("out_trade_no", out_trade_no);		    //商家订单号
                reqHandler.setParameter("transaction_id", payment.VoucherNo);	        //财付通订单号
                reqHandler.setParameter("fee_type", "CNY");                 //币种，1人民币
                reqHandler.setParameter("sub_order_no", out_trade_no);           //子订单号
                reqHandler.setParameter("customs", customsConfig.customs_place);	                //海关  0 无需上报海关1广州2杭州3宁波4深圳5郑州保税区(暂停)6重庆7西安8上海9 郑州
                reqHandler.setParameter("mch_customs_no", customsConfig.merchant_customs_code);//商户海关备案号IE150723865142
                reqHandler.setParameter("cert_type", "1");   //证件类型

                reqHandler.setParameter("cert_id", address.IDCardNo);   //收货人身份证号
                reqHandler.setParameter("name", address.Name);   //收货人姓名
                reqHandler.setParameter("action_type", "1");   //1新增2修改

                string uriPath = reqHandler.getRequestURL();

                string xml = MyHttp.GetResponse(uriPath);
                //Game.Utils.FileManager.WriteFile(Hyt.Util.WebUtil.GetMapPath("/TenpayCustomsMdl.txt"), xml);
                //string xmlTxt = System.IO.File.ReadAllText(Hyt.Util.WebUtil.GetMapPath("/TenpayCustomsMdl.txt"));
                AliAcquireCustomsBack backMod = SaveTenpayAcquireCustomsBackData(xml);

               // backMod.PushDataInfo = uriPath;
                backMod.OutReportXML = xml;

                if (string.IsNullOrEmpty(backMod.Success))
                {
                    backMod.Success = "F";
                }
                else
                {
                    backMod.Success = Enum.GetName(typeof(Hyt.Model.WorkflowStatus.OrderStatus.海关微信申报状态), Convert.ToInt32(backMod.Success));
                }
                //backMod.Type = 1;
                //AcquireCustomsBo.Instance.InnerAcquireCustoms(backMod);

                //soorder.CustomsResult = backMod.Success;
                //SoOrderBo.Instance.UpdateOrder(soorder);
                //(int)OrderStatus.支付报关状态.处理中
                Hyt.BLL.Order.SoOrderBo.Instance.UpdateOrderApiStatus(GetOrderPayCustomStatus(backMod.Success),0,soorder.SysNo);
                BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.前台, "订单编号：" + soorder.SysNo + ",支付信息报关提交成功！" + "回执：" + backMod.OutReportXML, LogStatus.系统日志目标类型.订单支付报关, soorder.SysNo, 0);      
                result.Status = true;
                result.Message = "报关成功";
            }
            catch (Exception e)
            {
                result.Status = false;
                result.Message = "报关失败-" + e.Message;
                try
                {
                    
                        BLL.Log.LocalLogBo.Instance.Write( result.Message, "WeiXinCustomsERRORLog");       
                    
                }
                catch (Exception ex)
                {
                    result.Status = false;
                    result.Message = "报关失败-" + ex.Message;
                      BLL.Log.LocalLogBo.Instance.Write( result.Message, "WeiXinCustomsERRORLog");       
                }
            }
            return result;
        }

        /// <summary>
        /// 海关支付报关查询
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <returns></returns>
        /// <remarks>2015-12-31 杨浩 创建</remarks

        public override Result CustomsQuery(int orderId)
        {
            Result result = new Result();
            //List<SoOrder> orderList = SoOrderBo.Instance.GetOrderByWeiXinCustomsData();
            SoOrder order = SoOrderBo.Instance.GetEntity(orderId);
            
            result.Status = false;
            try
            {
                IList<FnOnlinePayment> list = FinanceBo.Instance.GetOnlinePaymentList(order.SysNo);
                FnOnlinePayment payment;
                if (list.Count > 0)
                {
                    payment = list[0];
                }
                else
                {
                    result.Status = false;
                    result.Message = "付款单信息无效，请核实订单是否付款？";
                    return result;
                }

                SoReceiveAddress address = SoOrderBo.Instance.GetOrderReceiveAddress(order.ReceiveAddressSysNo);
                //List<AliAcquireCustomsBack> payBackList = AcquireCustomsBo.Instance.GetAllAcquireCustomsBackByOrderSysNos(order.SysNo.ToString());
                RequestHandler reqHandler = new RequestHandler(System.Web.HttpContext.Current, customsConfig.key, customsConfig.GATEWAY_NEW);
                //if (payBackList.Count == 0)
                //{
                //    continue;
                //}
                reqHandler.init();

                //协议参数
                reqHandler.setParameter("sign_type", "MD5");
                reqHandler.setParameter("service_version", "1.0");
                reqHandler.setParameter("input_charset", "UTF-8");
                reqHandler.setParameter("sign_key_index", "1");
                // 设置支付参数
                //-----------------------------
                reqHandler.setParameter("partner", customsConfig.partner);		        //商户号
                string out_trade_no = "";
                if (string.IsNullOrEmpty(payment.BusinessOrderSysNo))
                {
                    out_trade_no = payment.SourceSysNo.ToString();
                }
                else
                {
                    out_trade_no = payment.BusinessOrderSysNo;
                }
                reqHandler.setParameter("out_trade_no", out_trade_no);		        //商户号 payBackList[0].OutRequestNo
                //if (list.Count > 0)
                //{
                //    reqHandler.setParameter("transaction_id", list[0].VoucherNo);		        //商户号
                //}
                //reqHandler.setParameter("sub_order_no", payBackList[0].CustomsTradeNo);		        //商户号
                //reqHandler.setParameter("sub_order_id", order.SysNo.ToString());		        //商户号
                reqHandler.setParameter("customs", customsConfig.customs_place);		        //商户号

                string uriPath = reqHandler.getRequestURL("http://mch.tenpay.com/cgi-bin/mch_custom_query.cgi");

                string xml = MyHttp.GetResponse(uriPath);

                //Game.Utils.FileManager.WriteFile(Hyt.Util.WebUtil.GetMapPath("/TenpayCustomsMdl.txt"), xml);
                ///插入更新记录
                ///Hyt.Model.Customs.AliAcquireCustomsBack backMod = Pay.SaveTenpayAcquireCustomsBackData(xml);
                ///
                AliAcquireCustomsBack cusMod = SaveTenpayAcquireCustomsBackData(xml);

                object obj = GetTenpayReturnCustomsBack(xml);
                //AliAcquireCustomsBack backMod = payBackList[0];
                string status = obj.GetType().GetProperty("state").GetValue(obj, null).ToString();
                //backMod.PushDataInfo = uriPath;
                //backMod.Success = Enum.GetName(typeof(Hyt.Model.WorkflowStatus.OrderStatus.海关微信申报状态), Convert.ToInt32(status));
                //backMod.Type = 1;
                //backMod.OutReportXML = xml;
                //AcquireCustomsBo.Instance.UpdateAcquireCustoms(backMod);

                ///更新付款人信息
                ///
                BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.前台, "订单编号：" + order.SysNo + ",支付信息查询！" + "回执：" + xml, LogStatus.系统日志目标类型.订单支付报关, order.SysNo, 0);      
                if (status == "4")
                {
                    Hyt.BLL.Order.SoOrderBo.Instance.UpdateOrderApiStatus(GetOrderPayCustomStatus(GetTenpayStatus(status)), 0, orderId);  
                    /////获取付款人信息名称
                    //Model.Generated.SoPaymentInfo payInfo = SoPaymentInfoBo.Instance.GetPaymentInfo(order.SysNo);
                    //if (payInfo != null)
                    //{
                    //    payInfo.PaymentName = obj.GetType().GetProperty("name").GetValue(obj, null).ToString();
                    //    payInfo.PaymentIDC = obj.GetType().GetProperty("cert_id").GetValue(obj, null).ToString();
                    //    payInfo.PaymentPhone = address.MobilePhoneNumber;
                    //    SoPaymentInfoBo.Instance.UpdatePaymentInfo(payInfo);
                    //}
                    //else
                    //{
                    //    payInfo = new SoPaymentInfo();
                    //    payInfo.SoSysNo = order.SysNo;
                    //    payInfo.PaymentName = obj.GetType().GetProperty("name").GetValue(obj, null).ToString();
                    //    payInfo.PaymentIDC = obj.GetType().GetProperty("cert_id").GetValue(obj, null).ToString();
                    //    payInfo.PaymentPhone = address.MobilePhoneNumber;
                    //    SoPaymentInfoBo.Instance.InsertPaymentInfo(payInfo);
                    //}
                }

                ///更新支付报关情况。
                if (status == "5")
                {
                    Hyt.BLL.Order.SoOrderBo.Instance.UpdateOrderApiStatus(GetOrderPayCustomStatus(GetTenpayStatus(status)), 0, orderId);  
                    //backMod.Success = "";
                    //order.AutoPush = 0;
                }
                //order.CustomsResult = backMod.Success;
                //SoOrderBo.Instance.UpdateOrder(order);
                //content = order.SysNo + " 更新结束。";

                result.Message = xml;
                result.Status = true;


            }
            catch (Exception e)
            {
                result.Message = e.Message;
            }
            //return Json(new { content = content });
            return result;
        }

        public override Result RestartPushCustomsOrder(int orderId)
        {
            Result result = new Result();
            SoOrder order = SoOrderBo.Instance.GetEntity(orderId);
            RequestHandler reqHandler = new RequestHandler(System.Web.HttpContext.Current, customsConfig.key, customsConfig.GATEWAY_NEW);

            IList<FnOnlinePayment> list = FinanceBo.Instance.GetOnlinePaymentList(order.SysNo);
            FnOnlinePayment payment;
            if (list.Count > 0)
            {
                payment = list[0];
            }
            else
            {
                result.Status = false;
                result.Message = "付款单信息无效，请核实订单是否付款？";
                return result;
            }

            reqHandler.init();
            //协议参数
            reqHandler.setParameter("sign_type", "MD5");
            reqHandler.setParameter("service_version", "1.0");
            reqHandler.setParameter("input_charset", "UTF-8");
            reqHandler.setParameter("sign_key_index", "1");
            // 设置支付参数
            //-----------------------------
            reqHandler.setParameter("partner", customsConfig.partner);		        //商户号
            reqHandler.setParameter("out_trade_no", payment.BusinessOrderSysNo);		        //商户号 payBackList[0].OutRequestNo
            reqHandler.setParameter("action_type", "3");		        //商户号
            reqHandler.setParameter("customs", customsConfig.customs_place);		        //商户号
            reqHandler.setParameter("mch_customs_no", customsConfig.merchant_customs_code);		        //商户号

            string uriPath = reqHandler.getRequestURL();

            string xml = MyHttp.GetResponse(uriPath);

            return result;
        }
        /// <summary>
        /// 获取微信报关返回内容实体
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static AliAcquireCustomsBack SaveTenpayAcquireCustomsBackData(string xml)
        {
            AliAcquireCustomsBack backMod = new AliAcquireCustomsBack();
            backMod.Type = 1;
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlElement rootElem = doc.DocumentElement;
            XmlNodeList nodeList = rootElem.ChildNodes;
            foreach (XmlNode node in nodeList)
            {
                switch (node.LocalName)
                {
                    case "retcode":
                        backMod.ResultCode = node.InnerText;
                        break;
                    case "retmsg":
                        backMod.DetailErrorDes = node.InnerText;
                        break;
                    case "state":
                        backMod.Success = (node.InnerText);
                        break;
                    case "sub_order_no":
                        backMod.OrderSysNo = Convert.ToInt32(node.InnerText.Split('_')[0]);
                        break;
                    case "out_trade_no":
                        backMod.OutRequestNo = (node.InnerText);
                        break;
                    case "businees_type":
                        backMod.Type = Convert.ToInt32(node.InnerText);
                        break;
                    case "sub_order_id":
                        backMod.CustomsTradeNo = (node.InnerText);
                        break;

                }
            }
            return backMod;
        }
        public static object GetTenpayReturnCustomsBack(string xml)
        {
            XmlDocument doc = new XmlDocument();

            string state = "";
            string out_trade_no = "";
            string name = "";
            string cert_id = "";


            doc.LoadXml(xml);
            XmlElement rootElem = doc.DocumentElement;
            XmlNodeList nodeList = rootElem.ChildNodes;
            foreach (XmlNode node in nodeList)
            {
                if (node.LocalName.IndexOf("cert_id") != -1)
                {
                    cert_id = node.InnerText;
                }
                else if (node.LocalName.IndexOf("name") != -1)
                {
                    name = node.InnerText;
                }
                else if (node.LocalName.IndexOf("out_trade_no") != -1)
                {
                    out_trade_no = node.InnerText;
                }
                else if (node.LocalName.IndexOf("state") != -1)
                {
                    state = node.InnerText;
                }
            }
            return new { cert_id = cert_id, name = name, out_trade_no = out_trade_no, state = (state) };
        }
        static string GetTenpayStatus(string value)
        {
            switch (value)
            {
                case "1": return "待申报";
                case "2": return "待修改申报（订单已经送海关，商户重新申报，并且海关还有修改接口，那么记录的状态会是这个）";
                case "3": return "申报中";
                case "4": return "申报成功";
                case "5": return "申报失败";
                default: return value.ToString();
            }

        }
        public int GetOrderPayCustomStatus(string txt)
        {
            //待申报 = 1,
            //待修改申报 = 2,
            //申报中 = 3,
            //申报成功 = 4,
            //申报失败 = 5
            switch(txt)
            {
                case "待申报": return 10;
                case "待修改申报": return 10;
                case "申报中": return 10;
                case "申报成功": return 100;
                case "申报失败": return 20;
            }
            return 0;
        }
        /// <summary>
        /// 查询订单支付状态
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-12-20 杨浩 创建</remarks> 
        public override Result QueryOrderState(string orderId)
        {
            string url = "https://api.mch.weixin.qq.com/pay/orderquery";
 
            var result=base.QueryOrderState(orderId);
            string out_trade_no = "";
            string transaction_id = "5555";

            //检测必填参数
            if (out_trade_no=="" && transaction_id=="")
            {
                result.Message="订单查询接口中，out_trade_no、transaction_id至少填一个！";
                return result;
            }
     
            var values = new SortedDictionary<string, object>();
            values["transaction_id"]=transaction_id;
            values["appid"]= payConfig.WxAppid;
            values["mch_id"]= payConfig.WxMchId;
            values["nonce_str"]= GenerateNonceStr;
            //values.Add("out_trade_no", out_trade_no);
          
            //values.Add("sign_type", "MD5");
            values["sign"]= MakeSign(values);

            string xml = ToXml(values);

            var start = DateTime.Now;
            string response=Hyt.Util.WebUtil.Post(xml,url,false,3000);
        
            var end = DateTime.Now;
            int timeCost = (int)((end - start).TotalMilliseconds);//获得接口耗时

            //将xml格式的数据转化为对象以返回
            var xe = new XmlDocumentExtender();
            xe.LoadXml(response);
            string return_code=xe["return_code"].InnerText;
            string result_code = xe["result_code"].InnerText;

            if (return_code == "SUCCESS" && result_code == "SUCCESS")
            {

            }

            return result;
        }

        /// <summary>
        /// 生成签名，详见签名生成算法
        /// </summary>
        /// <param name="values">采用排序的Dictionary的好处是方便对数据包进行签名，不用再签名之前再做一次排序</param>
        /// <returns>sign字段不参加签名</returns>
        /// <remarks>2016-12-21 杨浩 创建</remarks>
        private string MakeSign(SortedDictionary<string, object> values)
        {
            //转url格式
            string str = "";

            foreach (KeyValuePair<string, object> pair in values)
            {
                if (pair.Value == null)
                {                
                    throw new Exception("内部含有值为null的字段!");
                }

                if (pair.Key != "sign" && pair.Value.ToString() != "")
                {
                    str += pair.Key + "=" + pair.Value + "&";
                }
            }
            str = str.Trim('&');

            //在string后加入API KEY
            str += "&key=" + payConfig.WxKey;
            //MD5加密
            var md5 = MD5.Create();
            var bs = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            var sb = new StringBuilder();
            foreach (byte b in bs)
            {
                sb.Append(b.ToString("x2"));
            }
            //所有字符转为大写
            return sb.ToString().ToUpper();
        }

        /// <summary>
        ///将Dictionary转成xml
       /// </summary>
        /// <returns>经转换得到的xml串</returns>
        /// <remarks>2016-12-21 杨浩 创建</remarks>
        private string ToXml(SortedDictionary<string, object> values)
        {
            //数据为空时不能转化为xml格式
            if (0 == values.Count)
            {            
                throw new Exception("数据为空!");
            }

            string xml = "<xml>";
            foreach (KeyValuePair<string, object> pair in values)
            {
                //字段值不能为null，会影响后续流程
                if (pair.Value == null)
                {                  
                    throw new Exception("内部含有值为null的字段!");
                }

                if (pair.Value.GetType() == typeof(int))
                {
                    xml += "<" + pair.Key + ">" + pair.Value + "</" + pair.Key + ">";
                }
                else if (pair.Value.GetType() == typeof(string))
                {
                    xml += "<" + pair.Key + ">" + "<![CDATA[" + pair.Value + "]]></" + pair.Key + ">";
                }
                else//除了string和int类型不能含有其他数据类型
                {
                  
                    throw new Exception("字段数据类型错误!");
                }
            }
            xml += "</xml>";
            return xml;
        }
        #region 实体类
        /// <summary>
        /// 微信支付查询订单状态请求参数
        /// </summary>
        /// <remarks>2016-12-21 杨浩 创建</remarks>
        private class QueryOrderStateRequest
        {
            /// <summary>
            /// 公众账号ID 【(微信支付分配的公众账号ID（企业号corpid即为此appId）】
            /// </summary>
            public string appid { get; set; }
            /// <summary>
            /// 商户号 (微信支付分配的商户号 )
            /// </summary>
            public string mch_id { get; set; }
            /// <summary>
            /// 微信订单号
            /// </summary>
            public string transaction_id { get; set; }
            /// <summary>
            /// 商户订单号
            /// </summary>
            public string out_trade_no { get; set; }
            /// <summary>
            /// 随机字符串 
            /// </summary>
            public string nonce_str { get; set; }
            /// <summary>
            /// 签名
            /// </summary>
            public string sign { get; set; }
            /// <summary>
            /// 签名类型(目前支持HMAC-SHA256和MD5，默认为MD5)
            /// </summary>
            public string sign_type { get; set; }

        }
        /// <summary>
        /// 微信支付查询订单状态返回数据
        /// </summary>
        /// <remarks>2016-12-21 杨浩 创建</remarks>
        public class QueryOrderStateResponse
        {

        }
        #endregion


    }
}
