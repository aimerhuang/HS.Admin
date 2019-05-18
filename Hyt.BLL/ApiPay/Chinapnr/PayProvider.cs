using Hyt.Model;
using Hyt.Model.Logis.XinYi;
using Hyt.Model.Manual;
using Hyt.Model.WorkflowStatus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Newtonsoft.Json.Linq;
using Hyt.Model.Parameter;
using Hyt.Model.Common;
using com.ekhing.OnlinePay;
using com.ekhing.Entity;
using com.ehking.utils;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Net.Security;
using System.Text.RegularExpressions;
using Hyt.Model.Api;
using Hyt.BLL.Log;
using Hyt.BLL.Finance;
using System.Transactions;

namespace Hyt.BLL.ApiPay.Chinapnr
{
    /// <summary>
    /// 汇付天下报关接口
    /// </summary>
    /// <remarks>2017-07-13 杨浩 创建</remarks>
    public class PayProvider:IPayProvider
    {
        private PayConfig config = Hyt.BLL.Config.Config.Instance.GetPayConfig();

        private readonly string defaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";

        //private string url = "https://mertest.chinapnr.com/custom/applyImptDec.do";
        private string url = "https://global.chinapnr.com/custom/applyImptDec.do"; //正式
        /// <summary>
        /// 商户号
        /// </summary>
        private string merchantAcctId = "10042864953" + "01";//正式
   
        /// <summary>
        /// 终端号
        /// </summary>
        private string terminalId = "XY002"; //正式XY002
       
        public PayProvider()
        {
            //key = config.EhkingKey;
            //merchantId = config.EhkingMerhantId;
        }
        /// <summary>
        /// 支付企业代码
        /// </summary>
        /// <remarks>2015-12-26 杨浩 创建</remarks>
        public override CommonEnum.PayCode Code
        {
            get{return CommonEnum.PayCode.汇付天下;}            
        }
        /// <summary>
        /// 海关通道
        /// </summary>
        /// <remarks>2015-12-28 杨浩 创建</remarks>
        public string CustomsChannel
        {
            get
            {
                return "OFFICAL";             
            }
        }

        /// <summary>  
        /// 创建POST方式的HTTP请求  
        /// </summary>  
        /// <param name="url">请求的URL</param>  
        /// <param name="parameters">随同请求POST的参数名称及参数值字典</param>  
        /// <param name="timeout">请求的超时时间</param>  
        /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>  
        /// <param name="requestEncoding">发送HTTP请求时所用的编码</param>  
        /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>  
        /// <returns></returns>  
        private  HttpWebResponse CreatePostHttpResponse(string url, IDictionary<string, string> parameters, int? timeout, string userAgent, Encoding requestEncoding, CookieCollection cookies,ref string message)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            if (requestEncoding == null)
            {
                throw new ArgumentNullException("requestEncoding");
            }
            HttpWebRequest request = null;
            //如果是发送HTTPS请求  
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            if (!string.IsNullOrEmpty(userAgent))
            {
                request.UserAgent = userAgent;
            }
            else
            {
                request.UserAgent = defaultUserAgent;
            }

            if (timeout.HasValue)
            {
                request.Timeout = timeout.Value;
            }
            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            //如果需要POST数据  
            if (!(parameters == null || parameters.Count == 0))
            {
                StringBuilder buffer = new StringBuilder();
                int i = 0;
                foreach (string key in parameters.Keys)
                {
                    if (i > 0)
                    {
                        buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                    }
                    else
                    {
                        buffer.AppendFormat("{0}={1}", key, parameters[key]);
                    }
                    i++;
                }              

                var postData = string.Join("&", parameters.Select(
                    p => string.Format("{0}={1}", p.Key, System.Web.HttpUtility.UrlEncode(p.Value, requestEncoding))).ToArray());

                message = postData;

                byte[] data = requestEncoding.GetBytes(postData);
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            return request.GetResponse() as HttpWebResponse;
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受  
        }

        /// <summary>
        /// 验签
        /// </summary>
        /// <param name="respTxt"></param>
        private Result ReceiveData(String respTxt)
        {

            var _result = new Result<string>()
            {
                Status = false
            };

            #region 解析参数
            Dictionary<string, string> dic = new Dictionary<string, string>();
            string[] respArray = Regex.Split(respTxt, "&");

            foreach (String resp in respArray)
            {
                string[] keyArray = Regex.Split(resp, "=");

                if ((keyArray != null) && (keyArray.Length == 2))
                {
                    dic.Add(keyArray[0], keyArray[1]);
                }
                else if ((keyArray != null) && (keyArray.Length == 1))
                {
                    dic.Add(keyArray[0], "");
                }
            }
            string version = dic["version"];
            string bgUrl = dic["bgUrl"];
            string signType = dic["signType"];
            string merchantAcctId = dic["merchantAcctId"];
            string terminalId = dic["terminalId"];
            string dealId = "";
            if (dic.ContainsKey("dealId"))
            {
                dealId = dic["dealId"];
            }
            string competCustom = "";
            if (dic.ContainsKey("competCustom"))
            {
                competCustom = dic["competCustom"];
            }
            string customCode = "";
            if (dic.ContainsKey("customCode"))
            {
                customCode = dic["customCode"];
            }
            string customVersion = dic["customVersion"];
            string merCustomCode = dic["merCustomCode"];
            string merCustomName = dic["merCustomName"];
            string ecpDomainName = "";
            if (dic.ContainsKey("ecpDomainName"))
            {
                ecpDomainName = dic["ecpDomainName"];
            }
            string ecpShortName = "";
            if (dic.ContainsKey("ecpShortName"))
            {
                ecpShortName = dic["ecpShortName"];
            }
            string iaqInstCode = "";
            if (dic.ContainsKey("iaqInstCode"))
            {
                iaqInstCode = dic["iaqInstCode"];
            }
            string payerIdType = dic["payerIdType"];
            string payerName = dic["payerName"];
            string payerIdNumber = dic["payerIdNumber"];
            string orderId = dic["orderId"];
            string orderCurrency = dic["orderCurrency"];
            string orderAmt = dic["orderAmt"];
            string freightAmt = dic["freightAmt"];
            string goodsAmt = dic["goodsAmt"];
            string taxAmt = dic["taxAmt"];
            string offsetAmt = dic["offsetAmt"];
            string bizType = dic["bizType"];
            string ext1 = dic["ext1"];
            string ext2 = dic["ext2"];
            string declareId = "";
            if (dic.ContainsKey("declareId"))
            {
                declareId = dic["declareId"];
            }
            string decResult = dic["decResult"];
            string errorCode = dic["errorCode"];
            string errorMsg = "";
            if (dic.ContainsKey("errorMsg"))
            {
                errorMsg = dic["errorMsg"];
            }
            string signMsg = dic["signMsg"];

            string signMsgVal = "";
            signMsgVal = appendParam(signMsgVal, "competCustom", competCustom);
            signMsgVal = appendParam(signMsgVal, "customCode", customCode);
            signMsgVal = appendParam(signMsgVal, "customVersion", customVersion);
            signMsgVal = appendParam(signMsgVal, "dealId", dealId);
            signMsgVal = appendParam(signMsgVal, "decResult", decResult);
            signMsgVal = appendParam(signMsgVal, "declareId", declareId);
            signMsgVal = appendParam(signMsgVal, "ecpDomainName", ecpDomainName);
            signMsgVal = appendParam(signMsgVal, "ecpShortName", ecpShortName);
            signMsgVal = appendParam(signMsgVal, "errorCode", errorCode);
            signMsgVal = appendParam(signMsgVal, "errorMsg", errorMsg);
            signMsgVal = appendParam(signMsgVal, "freightAmt", freightAmt);
            signMsgVal = appendParam(signMsgVal, "goodsAmt", goodsAmt);
            signMsgVal = appendParam(signMsgVal, "iaqInstCode", iaqInstCode);
            signMsgVal = appendParam(signMsgVal, "merCustomCode", merCustomCode);
            signMsgVal = appendParam(signMsgVal, "merCustomName", merCustomName);
            signMsgVal = appendParam(signMsgVal, "merchantAcctId", merchantAcctId);
            signMsgVal = appendParam(signMsgVal, "offsetAmt", offsetAmt);
            signMsgVal = appendParam(signMsgVal, "orderAmt", orderAmt);
            signMsgVal = appendParam(signMsgVal, "orderCurrency", orderCurrency);
            signMsgVal = appendParam(signMsgVal, "orderId", orderId);
            signMsgVal = appendParam(signMsgVal, "payerIdNumber", payerIdNumber);
            signMsgVal = appendParam(signMsgVal, "payerIdType", payerIdType);
            signMsgVal = appendParam(signMsgVal, "payerName", payerName);
            signMsgVal = appendParam(signMsgVal, "taxAmt", taxAmt);
            signMsgVal = appendParam(signMsgVal, "terminalId", terminalId);
            signMsgVal = appendParam(signMsgVal, "bizType", bizType);
            signMsgVal = appendParam(signMsgVal, "version", version);

            ///UTF-8编码  GB2312编码  用户可以根据自己网站的编码格式来选择加密的编码方式
            ///byte[] bytes = Encoding.GetEncoding("GB2312").GetBytes(signMsgVal);
            String signMsgDecode = HttpUtility.UrlDecode(signMsg);
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(signMsgVal);
            byte[] SignatureByte = Convert.FromBase64String(signMsgDecode);
            X509Certificate2 cert = new X509Certificate2(Hyt.Util.WebUtil.MapPath("/ChinaPnR.rsa.cer"), "");  
            RSACryptoServiceProvider rsapri = (RSACryptoServiceProvider)cert.PublicKey.Key;
            rsapri.ImportCspBlob(rsapri.ExportCspBlob(false));
            RSAPKCS1SignatureDeformatter f = new RSAPKCS1SignatureDeformatter(rsapri);
            byte[] result;
            f.SetHashAlgorithm("SHA1");
            SHA1CryptoServiceProvider sha = new SHA1CryptoServiceProvider();
            result = sha.ComputeHash(bytes);

            #endregion

            _result.Data = errorCode;
            if (f.VerifySignature(result, SignatureByte))
            {
                if (errorCode == "000000")
                {
                    _result.Status = true;
                    _result.Message = "申报成功！";
                }
                else
                {
                    _result.Status = false;
                    _result.Message = errorMsg;
                }
                
                //verifySignature = "true";

            }
            else
            {
                _result.Status = false;
                _result.Message = "签名失败";
                //verifySignature = "false";
            }

            return _result;
            //Response.Redirect("show.aspx?errorCode=" + errorCode + "&decResult=" + decResult + "&errorMsg=" + errorMsg + "&verifySignature=" + verifySignature);

        }

        /// <summary>
        /// 功能函数。将变量值不为空的参数组成字符串
        /// </summary>
        /// <param name="returnStr"></param>
        /// <param name="paramId"></param>
        /// <param name="paramValue"></param>
        /// <returns></returns>
        private String appendParam(String returnStr, String paramId, String paramValue)
        {
            if (returnStr != "")
            {
                if (paramValue != "")
                {
                    returnStr += "&" + paramId + "=" + paramValue;
                }
            }
            else
            {
                if (paramValue != "")
                {
                    returnStr = paramId + "=" + paramValue;
                }
            }
            return returnStr;
        }
        /// <summary>
        /// 海关报关
        /// </summary>
        /// <returns></returns>
        /// <remarks>2017-07-13 杨浩 创建</remarks>
        public override Result ApplyToCustoms(SoOrder order)
        {
            var result = new Result()
            {
                Status=false
            };

            try
            {

              

                string V_PFREIGHT_NO = "";
                var httpCurrent = System.Web.HttpContext.Current;
                if (httpCurrent != null && httpCurrent.Request != null && httpCurrent.Request.Form != null)
                    V_PFREIGHT_NO = httpCurrent.Request.Form["V_PFREIGHT_NO"];


                if (order != null)
                {
                    Hyt.BLL.Order.SoOrderBo.Instance.UpdateOrderPayType(order.SysNo, (int)this.Code);

                    var receiveInfo = BLL.Order.SoOrderBo.Instance.GetOrderReceiveAddress(order.ReceiveAddressSysNo);

                    var warehouseInfo=BLL.Warehouse.WhWarehouseBo.Instance.GetWarehouse(order.DefaultWarehouseSysNo);
          
                    //if (warehouseInfo.Logistics == (int)Hyt.Model.CommonEnum.物流代码.新银河世纪)
                    //{                  
                    //    if (string.IsNullOrWhiteSpace(order.ExpressNo))
                    //    {
                    //        result.Message = "订单号【" + order.SysNo + "】未填快递单号！";
                    //        return result;
                    //    }

                    //    if (string.IsNullOrWhiteSpace(V_PFREIGHT_NO))
                    //    {
                    //        result.Message = "未填写运单号！";
                    //        return result;
                    //    }                                             
                    //}
                    #region 参数说明
                    //版本号，固定值：1.0
                    string version = "1.0";
                    //服务器接收支付结果的后台地址
                    string bgUrl = "http://admin.singingwhale.cn/OrderResult/ReceiveBg";
                    //签名类型,该值为4，代表PKI加密方式,该参数必填。
                    string signType = "4";

                    //支付流水号,不可空。
                    string dealId = "";
                    //海关代码,不可空。
                    string customCode = "SZHG";  //
                    //海关版本，可空。
                    string customVersion = "1.0";
                    //商户海关备案号，不可空
                    string merCustomCode = "4403160YDP";
                    //商户海关备案名称，不可空。
                    string merCustomName = "深圳市信营国际电子商务有限公司";
                    //证件类型，不可空，1-身份证
                    string payerIdType = "1";
                    //姓名，不可空。
                    string payerName = receiveInfo.Name;//
                    //身份证号，不可空。
                    string payerIdNumber = receiveInfo.IDCardNo;//321281199302133913
                    //商户订单号，不可空。
                    string orderId = order.SysNo.ToString()+DateTime.Now.ToString("_yyMMddHHmmss");
                    //订单币种，不可空
                    string orderCurrency = "CNY";
                    //订单金额
                    string orderAmt = ((int)(order.OrderAmount * 100)).ToString();
                    //物流费
                    string freightAmt = ((int)(order.FreightAmount * 100)).ToString();
                    //货款
                    string goodsAmt = ((int)(order.CashPay * 100)).ToString();
                    //关税
                    string taxAmt =((int)(order.TaxFee * 100)).ToString();;
                    //抵扣金额
                    string offsetAmt = "0";
                    //电商平台企业简写（四位），不可空
                    string ecpShortName = "1500002394";
                    //电商平台域名
                    String ecpDomainName = "http://demo.singingwhale.cn";
                    //主管海关代码
                    String competCustom = "5166";
                    //检验检疫机构代码
                    String iaqInstCode = "440330";// "1500002394";
                    //业务类型
                    String bizType = "";
                    //扩展字段1
                    String ext1 = Guid.NewGuid().ToString();
                    //扩展字段2
                    String ext2 = "";

                    string signMsg = "";
                    #endregion
                    //拼接字符串
                    string signMsgVal = "";
                    signMsgVal = appendParam(signMsgVal, "version", version);
                    signMsgVal = appendParam(signMsgVal, "bgUrl", bgUrl);
                    signMsgVal = appendParam(signMsgVal, "signType", signType);
                    signMsgVal = appendParam(signMsgVal, "merchantAcctId", merchantAcctId);
                    signMsgVal = appendParam(signMsgVal, "terminalId", terminalId);
                    signMsgVal = appendParam(signMsgVal, "dealId", dealId);
                    signMsgVal = appendParam(signMsgVal, "customCode", customCode);
                    signMsgVal = appendParam(signMsgVal, "customVersion", customVersion);
                    signMsgVal = appendParam(signMsgVal, "merCustomCode", merCustomCode);
                    signMsgVal = appendParam(signMsgVal, "merCustomName", merCustomName);
                    signMsgVal = appendParam(signMsgVal, "payerIdType", payerIdType);
                    signMsgVal = appendParam(signMsgVal, "payerName", payerName);  
                    signMsgVal = appendParam(signMsgVal, "payerIdNumber", payerIdNumber);  
                    signMsgVal = appendParam(signMsgVal, "orderId", orderId);
                    signMsgVal = appendParam(signMsgVal, "orderCurrency", orderCurrency);
                    signMsgVal = appendParam(signMsgVal, "orderAmt", orderAmt);
                    signMsgVal = appendParam(signMsgVal, "freightAmt", freightAmt);
                    signMsgVal = appendParam(signMsgVal, "goodsAmt", goodsAmt);
                    signMsgVal = appendParam(signMsgVal, "taxAmt", taxAmt);
                    signMsgVal = appendParam(signMsgVal, "offsetAmt", offsetAmt);
                    ///PKI加密
                    ///编码方式UTF-8 GB2312  用户可以根据自己系统的编码选择对应的加密方式
                    ///byte[] OriginalByte=Encoding.GetEncoding("GB2312").GetBytes(OriginalString);
                    byte[] bytes = System.Text.Encoding.UTF8.GetBytes(signMsgVal);

                    X509Certificate2 cert = new X509Certificate2(Hyt.Util.WebUtil.GetMapPath("/10042864953.pfx"), "123456", X509KeyStorageFlags.MachineKeySet);
                    RSACryptoServiceProvider rsapri = (RSACryptoServiceProvider)cert.PrivateKey;
                    RSAPKCS1SignatureFormatter f = new RSAPKCS1SignatureFormatter(rsapri);
                    byte[] _result;
                    f.SetHashAlgorithm("SHA1");
                    SHA1CryptoServiceProvider sha = new SHA1CryptoServiceProvider();
                    _result = sha.ComputeHash(bytes);
                    signMsg = System.Convert.ToBase64String(f.CreateSignature(_result)).ToString();
                    Encoding encoding = Encoding.GetEncoding("utf-8");
                    var parameters = new Dictionary<string, string>();
                    parameters.Add("version", "1.0");
                    parameters.Add("bgUrl", bgUrl);
                    parameters.Add("signType", "4");
                    parameters.Add("merchantAcctId", merchantAcctId);
                    parameters.Add("terminalId", terminalId);
                    parameters.Add("dealId", "");//支付流水号
                    parameters.Add("customCode", customCode);//海关代码
                    parameters.Add("customVersion", customVersion); //海关版本 可空
                    parameters.Add("merCustomCode", merCustomCode);//商户海关备案号
                    parameters.Add("merCustomName", merCustomName);//商户海关备案名称
                    parameters.Add("payerIdType", "1");//证件类型
                    parameters.Add("payerName", payerName);//姓名
                    parameters.Add("payerIdNumber", payerIdNumber);//身份证号
                    parameters.Add("orderId", orderId);//商户订单号
                    parameters.Add("orderCurrency", "CNY");//订单币种
                    parameters.Add("orderAmt", orderAmt);//订单金额  型数字 以分为单位。比方10元，提交时金额应为1000,商户页面显示金额可以转换成以元为单位显示
                    parameters.Add("freightAmt", freightAmt);//物流费
                    parameters.Add("goodsAmt", goodsAmt);//货款
                    parameters.Add("taxAmt", taxAmt);//关税
                    parameters.Add("offsetAmt", offsetAmt);//抵扣金额
                    parameters.Add("ecpShortName", ecpShortName);//电商平台企业简写
                    parameters.Add("ecpDomainName", ecpDomainName);//电商平台域名
                    parameters.Add("competCustom", competCustom);//主管海关代码
                    parameters.Add("iaqInstCode", iaqInstCode);//检验检疫机构代码
                    parameters.Add("bizType", bizType);//业务类型
                    parameters.Add("ext1", ext1);//扩展字段1 可空
                    parameters.Add("ext2", ext2);//扩展字段1 可空
                    parameters.Add("signMsg", signMsg);//签名信息

                    string posData="";
                    HttpWebResponse response = CreatePostHttpResponse(url, parameters, null, null, encoding, null,ref posData);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        System.IO.Stream responseStream = response.GetResponseStream();
                        System.IO.StreamReader reader = new System.IO.StreamReader(responseStream, Encoding.UTF8);
                        string respTxt = reader.ReadToEnd();
                        if (respTxt.Length == 0)
                        {
                            result.Status = false;
                            result.Message = "网关系统异常,返回内容为空";
                            return result;
                        }

                        result=ReceiveData(respTxt);

                        if (result.Status)
                        {

                            Hyt.BLL.Order.SoOrderBo.Instance.UpdateOrderApiStatus((int)OrderStatus.支付报关状态.处理中, 0, order.SysNo);
                            Hyt.BLL.Order.SoOrderBo.Instance.UpdateOrderApiStatus((int)OrderStatus.支付报关状态.处理中, 1, order.SysNo);  
                          
                            var current=BLL.Authentication.AdminAuthenticationBo.Instance.Current;
                            var model = new SoOrderPayPushLog()
                            {
                                PaymentTypeSysNo = 19,
                                OrderSysNo = order.SysNo,
                                Message = posData,
                                ErrorCode=((Result<string>)result).Data,//
                                ErroMsg=result.Message,
                                CreateBy=current==null?0:current.Base.SysNo,
                                CreateDate=DateTime.Now,
                                MarkId=ext1,
                                Data =V_PFREIGHT_NO,
                            };

                           //BLL.Order.SoOrderPayPushLogBo.Instance.Insert(model);

                            #region 推送物流
                            //var  a_result=BLL.Order.SoOrderBo.Instance.AutoThreeSinglePush(order,true,false,false);
                            //if (!a_result.Status)
                            //    return a_result;
                            #endregion
                        }                                         
                        return result;
                    }
                    result.Status = false;
                    result.Message = "网关系统异常,返回码[" + response.StatusCode + "]";
                }
                else
                {
                    result.Status = false;
                    result.Message = "订单不存在！";
                }
            }
            catch (Exception ex)
            {
                result.Message = "报错！";
                result.Status = false;
                BLL.Log.LocalLogBo.Instance.Write("报错！" + ex.Message, "EhkingCustomsLog");                  
            }  

            return result;
        }
        /// <summary>
        /// 异步回执
        /// </summary>
        /// <param name="requestStr">http请求信息</param>
        /// <returns></returns>
        /// <remarks>2015-10-12 杨浩 创建</remarks>
        public override Result NotifyReceipt(string requestStr)
        {
            var result = base.NotifyReceipt(requestStr);

            try
            {
                string str = requestStr;
                if (string.IsNullOrWhiteSpace(requestStr))
                {
                    var s = System.Web.HttpContext.Current.Request.InputStream;
                    byte[] b = new byte[s.Length];
                    s.Read(b, 0, (int)s.Length);
                    str = Encoding.UTF8.GetString(b);
                    //BLL.Log.LocalLogBo.Instance.Write(str+"\r\n", "NotifyReceiptRequestLog");
                }
              

                var request = new Dictionary<string, string>();
                var paramsList=str.Split('&');
                foreach (var item in paramsList)
                {
                    if (!string.IsNullOrWhiteSpace(item))
                    {
                        var arry = item.Split('=');
                        string key = arry[0];
                        string value = HttpUtility.UrlDecode(arry[1]);
                        request.Add(key, value);
                    }               
                }                     

                #region 参数

                string version = request["version"].ToString();
                string bgUrl = request["bgUrl"].ToString();
                string signType = request["signType"].ToString();
                string merchantAcctId = request["merchantAcctId"].ToString();
                string terminalId = request["terminalId"].ToString();
                string dealId = "";
                if (request.ContainsKey("dealId"))
                {
                    dealId = request["dealId"].ToString();
                }
                string competCustom = "";
                if (request.ContainsKey("competCustom"))
                {
                    competCustom = request["competCustom"].ToString();
                }
                string customCode = "";
                if (request.ContainsKey("customCode"))
                {
                    customCode = request["customCode"].ToString();
                }
                string customVersion = request["customVersion"].ToString();
                string merCustomCode = request["merCustomCode"].ToString();
                string merCustomName = request["merCustomName"].ToString();
                string ecpDomainName = "";
                if (request.ContainsKey("ecpDomainName"))
                {
                    ecpDomainName = request["ecpDomainName"].ToString();
                }
                string ecpShortName = "";
                if (request.ContainsKey("ecpShortName"))
                {
                    ecpShortName = request["ecpShortName"].ToString();
                }
                string iaqInstCode = "";
                if (request.ContainsKey("iaqInstCode"))
                {
                    iaqInstCode = request["iaqInstCode"].ToString();
                }

                string payerIdType = request["payerIdType"].ToString();
                string payerName = request["payerName"].ToString();
                string payerIdNumber = request["payerIdNumber"].ToString();
                string orderId = request["orderId"].ToString();
                string orderCurrency = request["orderCurrency"].ToString();
                string orderAmt = request["orderAmt"].ToString();
                string freightAmt = request["freightAmt"].ToString();
                string goodsAmt = request["goodsAmt"].ToString();
                string taxAmt = request["taxAmt"].ToString();
                string offsetAmt = request["offsetAmt"].ToString();

                string bizType = "";
                if (request.ContainsKey("bizType"))              
                    bizType=request["bizType"].ToString(); 
                             
                string ext1 = "";
                if (request.ContainsKey("ext1"))
                    ext1 = request["ext1"].ToString(); 

                string ext2 = "";

                if (request.ContainsKey("ext2"))
                    ext2 = request["ext2"].ToString(); 


                string declareId = "";
                if (request.ContainsKey("declareId"))
                {
                    declareId = request["declareId"].ToString();
                }
                string decResult = request["decResult"].ToString();

                string errorCode ="";
                if (request.ContainsKey("errorCode"))
                {
                   errorCode = request["errorCode"].ToString();
                }
                
                string errorMsg = "";
                if (request.ContainsKey("errorMsg"))
                {
                    errorMsg = request["errorMsg"].ToString();
                }
                string signMsg = request["signMsg"].ToString();

                string signMsgVal = "";
                signMsgVal = appendParam(signMsgVal, "competCustom", competCustom);
                signMsgVal = appendParam(signMsgVal, "customCode", customCode);
                signMsgVal = appendParam(signMsgVal, "customVersion", customVersion);
                signMsgVal = appendParam(signMsgVal, "dealId", dealId);
                signMsgVal = appendParam(signMsgVal, "decResult", decResult);
                signMsgVal = appendParam(signMsgVal, "declareId", declareId);
                signMsgVal = appendParam(signMsgVal, "ecpDomainName", ecpDomainName);
                signMsgVal = appendParam(signMsgVal, "ecpShortName", ecpShortName);
                signMsgVal = appendParam(signMsgVal, "errorCode", errorCode);
                signMsgVal = appendParam(signMsgVal, "errorMsg", errorMsg);
                signMsgVal = appendParam(signMsgVal, "freightAmt", freightAmt);
                signMsgVal = appendParam(signMsgVal, "goodsAmt", goodsAmt);
                signMsgVal = appendParam(signMsgVal, "iaqInstCode", iaqInstCode);
                signMsgVal = appendParam(signMsgVal, "merCustomCode", merCustomCode);
                signMsgVal = appendParam(signMsgVal, "merCustomName", merCustomName);
                signMsgVal = appendParam(signMsgVal, "merchantAcctId", merchantAcctId);
                signMsgVal = appendParam(signMsgVal, "offsetAmt", offsetAmt);
                signMsgVal = appendParam(signMsgVal, "orderAmt", orderAmt);
                signMsgVal = appendParam(signMsgVal, "orderCurrency", orderCurrency);
                signMsgVal = appendParam(signMsgVal, "orderId", orderId);
                signMsgVal = appendParam(signMsgVal, "payerIdNumber", payerIdNumber);
                signMsgVal = appendParam(signMsgVal, "payerIdType", payerIdType);
                signMsgVal = appendParam(signMsgVal, "payerName", payerName);
                signMsgVal = appendParam(signMsgVal, "taxAmt", taxAmt);
                signMsgVal = appendParam(signMsgVal, "terminalId", terminalId);
                signMsgVal = appendParam(signMsgVal, "bizType", bizType);
                signMsgVal = appendParam(signMsgVal, "version", version);


              

                //BLL.Log.LocalLogBo.Instance.Write(signMsgVal, "HFNotifyReceiptLog");
                #endregion

                var  _model=BLL.Order.SoOrderPayPushLogBo.Instance.GetModel(ext1);
                _model.ReceiptDate = DateTime.Now;
                _model.ReceiptMessage = str;
                _model.ErroMsg = signMsg;
                _model.ErrorCode = errorCode;
                BLL.Order.SoOrderPayPushLogBo.Instance.Update(_model);

                try
                {

                    ///UTF-8编码  GB2312编码  用户可以根据自己网站的编码格式来选择加密的编码方式
                    ///byte[] bytes = Encoding.GetEncoding("GB2312").GetBytes(signMsgVal);
                    String signMsgDecode = HttpUtility.UrlDecode(signMsg);
                    String signMsgValDecode = HttpUtility.UrlDecode(signMsgVal);
                    byte[] bytes = System.Text.Encoding.UTF8.GetBytes(signMsgValDecode);
                    byte[] SignatureByte = Convert.FromBase64String(signMsgDecode);
                    X509Certificate2 cert = new X509Certificate2(Hyt.Util.WebUtil.MapPath("/ChinaPnR.rsa.cer"), "");
                    RSACryptoServiceProvider rsapri = (RSACryptoServiceProvider)cert.PublicKey.Key;
                    rsapri.ImportCspBlob(rsapri.ExportCspBlob(false));
                    RSAPKCS1SignatureDeformatter f = new RSAPKCS1SignatureDeformatter(rsapri);
                    byte[] _result;
                    f.SetHashAlgorithm("SHA1");
                    SHA1CryptoServiceProvider sha = new SHA1CryptoServiceProvider();
                    _result = sha.ComputeHash(bytes);

                    //System.Web.HttpContext.Current.Request.Form.Add("V_PFREIGHT_NO", _model.Data);
                }
                catch {}
               
                //if (f.VerifySignature(_result, SignatureByte))
                //{
                    if (decResult == "20"||decResult=="21")
                    {

                        int orderSysno = int.Parse(orderId.Split('_')[0]);
                        var model = new FnOnlinePayment();
                        model.Amount = decimal.Parse(orderAmt) / 100;
                        model.BusinessOrderSysNo = orderId;
                        model.Status = 1;
                        model.SourceSysNo = orderSysno;
                        model.VoucherNo = dealId;
                        model.PaymentTypeSysNo =(int)CommonEnum.PayCode.汇付天下;
                        model.Source = 10;
                        model.CreatedBy =0;
                        model.CreatedDate = DateTime.Now;
                        model.LastUpdateBy =0;
                        model.LastUpdateDate = DateTime.Now;
                        var r = new Result { Status = false };
                        r.Status = Hyt.BLL.Order.SoOrderBo.Instance.UpdateOrderPaySuatus(model, (int)OrderStatus.支付报关状态.成功,(int)OrderStatus.支付报关状态.成功);
                        if (r.Status)
                        {
                            //Log 创建网上支付
                            //SysLog.Instance.Info(LogStatus.系统日志来源.后台, "创建网上支付", LogStatus.系统日志目标类型.网上支付, model.SysNo,
                            //    0);

                        }

                        result.Status = true;
                    }
                    else
                    {
                        result.Status = false;
                    }


                    //BLL.Log.LocalLogBo.Instance.Write("验签成功\r\n" + signMsgVal, "HFNotifyReceiptSuccessLog");
                   


                    //System.Diagnostics.Debug.Write("验签成功");
                    //Response.Write(RESP_MSG);
               // }
              //  else
               // {
               //     result.Status = false;
                //    result.Message = "验签失";
                //    BLL.Log.LocalLogBo.Instance.Write("验签失", "HFNotifyReceiptLog");
                //    //System.Diagnostics.Debug.Write("验签失败");
                //}
      
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
                BLL.Log.LocalLogBo.Instance.Write("报错！" + ex.Message, "NotifyReceiptLog");     
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
            Result result = new Result()
            {
                Status = false
            };

            //var order = BLL.Order.SoOrderBo.Instance.GetEntity(orderId);

            //var ehkingMerchantId = config.EhkingMerhantId;
            //var ehkingKey = config.EhkingKey;

            //var warehouse = BLL.Warehouse.WhWarehouseBo.Instance.GetWarehouse(order.DefaultWarehouseSysNo);
            //if (warehouse != null && warehouse.WarehouseType == (int)Model.WorkflowStatus.WarehouseStatus.仓库类型.门店)
            //{
            //    var dealer = BLL.Stores.StoresBo.Instance.GetStoreByWarehouseId(warehouse.SysNo);
            //    if (dealer != null)
            //    {
            //        var dealerPayType = BLL.Stores.StoresBo.Instance.GetStorePayType(dealer.SysNo);
            //        if (!string.IsNullOrWhiteSpace(dealerPayType.AppKey) && !string.IsNullOrWhiteSpace(dealerPayType.AppSecret) && dealerPayType.PaymentTypeSysNo == (int)Model.CommonEnum.PayCode.易宝)
            //        {
            //            ehkingMerchantId = dealerPayType.AppKey;
            //            ehkingKey = dealerPayType.AppSecret;
            //        }
            //    }
            //}

            //var onlinePaymentFilter = new ParaOnlinePaymentFilter()
            //{
            //    OrderSysNo = orderId,
            //    Id = 1
            //};
            //var onlinePayments = Hyt.BLL.Finance.FinanceBo.Instance.GetOnlinePayments(onlinePaymentFilter).Rows;
            //if(onlinePayments.Count()<=0)
            //{
            //    result.Status = false;
            //    result.Message = "订单没有支付信息！";
            //}
            //else
            //{
            //    string responsestr = "";
            //    string data = "";
            //    try
            //    {
            //        string serialNumber = onlinePayments.First().VoucherNo;
            //        string url = "https://api.ehking.com/customs/query";//查询网关
            //        string hmac = Digest.HmacSign(ehkingMerchantId + serialNumber, ehkingKey);
            //        HttpClient client = new HttpClient(url);
            //        data = "{\"merchantId\":\"" + ehkingMerchantId + "\",\"paySerialNumber\":\"" + serialNumber + "\",\"hmac\":\"" + hmac + "\"}";
            //        responsestr = client.Post(data);

            //        var back = JObject.Parse(responsestr);
            //        var customsInfos = back.Property("customsInfos") != null ? back["customsInfos"][0] : back;
            //        OrderStatus.支付报关状态 status = OrderStatus.支付报关状态.失败;
            //        if (customsInfos["status"].ToString() == "SUCCESS")
            //        {

            //            result.Message = "SUCCESS";
            //            result.Status = true;
            //            status = OrderStatus.支付报关状态.成功;
            //        }
            //        else if ("PROCESSING" == customsInfos["status"].ToString())
            //        {
            //            result.Message = "海关正在审核...，稍后再查询！";
            //            status = OrderStatus.支付报关状态.处理中;
            //        }
            //        else
            //        {
            //            //BLL.Log.LocalLogBo.Instance.Write("失败！" + responsestr, "CustomsQueryLog");
            //            result.Message ="易宝返回状态:"+customsInfos["status"].ToString()+"("+ (back.Property("customsInfos") == null?customsInfos["error"].ToString():"")+")";
            //        }
                        

            //        Hyt.BLL.Order.SoOrderBo.Instance.UpdateOrderApiStatus((int)status, 0, orderId);                     
                  
            //    }
            //    catch (Exception ex)
            //    {
            //        result.Status = false;
            //        result.Message = "系统异常！";
            //        BLL.Log.LocalLogBo.Instance.Write("\r\n报错！" + ex.Message + ";responsestr=" + responsestr + ";data=" + data, "CustomsQuery");     
            //    }
             
            //}
            
            return result;
        }
        ///// <summary>
        ///// 查询订单支付状态
        ///// </summary>
        ///// <returns></returns>
        ///// <remarks>2016-12-20 杨浩 创建</remarks> 
        //public override Result QueryOrderState(string orderId)
        //{
        //    var result = new Result()
        //    {
        //        Status = false
        //    };
        //    //string key = "0627eb791eadb57cd947dc32e59e563e";
        //    //var payOrder = new OnlinePayOrder(key);
        //    //payOrder.MerchantId = "120140222";//商户号
        //    //payOrder.RequestId = "9942"; //订单号
        //    //string hmac = Digest.HmacSign(payOrder.MerchantId + payOrder.RequestId, key);
        //    //var client = new com.ekhing.Web.HttpClient("https://api.ehking.com/onlinePay/query");

        //    //string data = "{\"merchantId\":\"" + payOrder.MerchantId +
        //    //               "\",\"requestId\":\"" + payOrder.RequestId +
        //    //               "\",\"hmac\":\"" + hmac + "\"}";


        //    //string responseStr = client.Post(data);
        //    //var _result=JObject.Parse(responseStr);
            
        //    //string status=_result["status"].ToString();
        //    //if (status == "SUCCESS")
        //    //{
        //    //    string _hmac = _result["hmac"].ToString();
        //    //    string requestId = _result["requestId"].ToString();
        //    //    string serialNumber = _result["serialNumber"].ToString();
             
        //    //    string orderAmount = _result["orderAmount"].ToString();
        //    //    result.Status = true;
        //    //}
        //    //else if (status == "FAILED")
        //    //{
        //    //    result.Message = _result["error"].ToString();
        //    //}
           
        //    return result;
        //}

        


        

    }
}