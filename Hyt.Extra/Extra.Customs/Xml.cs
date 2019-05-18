using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Web;
using System.IO;
using Newtonsoft.Json.Linq;
using Hyt.Util.Net;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Net;

namespace Extra.Customs
{
    public class Xml
    {
        /// <summary>
        /// 上传xml文件 
        /// 2015-10-09 王耀发 创建
        /// </summary>
        /// <param name="RequestText"></param>
        public static string UploadXmlFile(string RequestText)
        {
            //获取当前时间
            int Year = DateTime.Now.Year;
            int Month = DateTime.Now.Month;
            int Day = DateTime.Now.Day;
            int Hour = DateTime.Now.Hour;
            int Minute = DateTime.Now.Minute;
            int Second = DateTime.Now.Second;
            string SendTime = Year.ToString() + (Month < 10 ? "0" + Month.ToString() : Month.ToString()) + (Day < 10 ? "0" + Day.ToString() : Day.ToString());
            SendTime += (Hour < 10 ? "0" + Hour.ToString() : Hour.ToString()) + (Minute < 10 ? "0" + Minute.ToString() : Minute.ToString()) + (Second < 10 ? "0" + Second.ToString() : Second.ToString());

            //RequestText = "{\"MessageIDNo\":\"0000\",\"Declaration\": {\"EOrder\": {\"OrderId\": \"141226133900128\",\"IEFlag\": \"I\",\"OrderStatus\": \"S\",";
            //RequestText += "\"EntRecordNo\": \"PTE51001412020000001\",\"EntRecordName\": \"顺丰速运集团（上海）速运有限公司\",\"OrderName\": \"刘青青\",\"OrderDocType\": \"01\",";
            //RequestText += "\"OrderDocId\": \"342401198207050086\",\"OrderPhone\": \"18682088326\",\"OrderGoodTotal\": \"198\",\"OrderGoodTotalCurr\": \"142\",";
            //RequestText += "\"Freight\": \"7\",\"FreightCurr\": \"142\",\"Tax\": \"0\",\"TaxCurr\": \"142\",\"Note\": \"任你你测试\",\"OrderDate\": \"2014-12-26 15:52:02\"},";
            //RequestText += "\"EOrderGoods\": [{\"GNo\": \"商品序号1\",\"ChildOrderNo\": \"子订单编号\",\"StoreRecordNo\": \"PTE51001412020000001\",\"StoreRecordName\": \"顺丰速运集团（上海）速运有限公司\",";
            //RequestText += "\"CopGNo\": \"99\",\"CustomsListNO\": \"GDO51411406260000181\",\"DecPrice\": \"99\",\"Unit\": \"125\",\"GQty\": \"2\",\"DeclTotal\": \"198\",\"Notes\": \"任你你测试\"},";
            //RequestText += "{";
            //RequestText += "\"GNo\": \"商品序号2\",\"ChildOrderNo\": \"子订单编号\",\"StoreRecordNo\": \"PTE51001412020000001\",\"StoreRecordName\": \"顺丰速运集团（上海）速运有限公司\",\"CopGNo\": \"99\",";
            //RequestText += "\"CustomsListNO\": \"GDO51411406260000181\",\"DecPrice\": \"99\",\"Unit\": \"125\",\"GQty\": \"2\",\"DeclTotal\": \"198\",\"Notes\": \"任你你测试\"}]}}";
            var jsonObject = JObject.Parse(RequestText);

            string MessageIDNo = jsonObject["MessageIDNo"].ToString();
            string OrderId = jsonObject["Declaration"]["EOrder"]["OrderId"].ToString();
            string IEFlag = jsonObject["Declaration"]["EOrder"]["IEFlag"].ToString();
            string OrderStatus = jsonObject["Declaration"]["EOrder"]["OrderStatus"].ToString();
            string EntRecordNo = jsonObject["Declaration"]["EOrder"]["EntRecordNo"].ToString();
            string EntRecordName = jsonObject["Declaration"]["EOrder"]["EntRecordName"].ToString();
            string OrderName = jsonObject["Declaration"]["EOrder"]["OrderName"].ToString();
            string OrderDocType = jsonObject["Declaration"]["EOrder"]["OrderDocType"].ToString();
            string OrderDocId = jsonObject["Declaration"]["EOrder"]["OrderDocId"].ToString();
            string OrderPhone = jsonObject["Declaration"]["EOrder"]["OrderPhone"].ToString();
            string OrderGoodTotal = jsonObject["Declaration"]["EOrder"]["OrderGoodTotal"].ToString();
            string OrderGoodTotalCurr = jsonObject["Declaration"]["EOrder"]["OrderGoodTotalCurr"].ToString();
            string Freight = jsonObject["Declaration"]["EOrder"]["Freight"].ToString();
            string FreightCurr = jsonObject["Declaration"]["EOrder"]["FreightCurr"].ToString();
            string Tax = jsonObject["Declaration"]["EOrder"]["Tax"].ToString();
            string TaxCurr = jsonObject["Declaration"]["EOrder"]["TaxCurr"].ToString();
            string Note = jsonObject["Declaration"]["EOrder"]["Note"].ToString();
            string OrderDate = jsonObject["Declaration"]["EOrder"]["OrderDate"].ToString();


            string strxml = "";
            strxml += "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
            strxml += "<Manifest>";
            strxml += "<Head>";
            strxml += "<MessageID>" + ApiConfig.MessageType + SendTime + MessageIDNo + "</MessageID>";
            strxml += "<MessageType>" + ApiConfig.MessageType + "</MessageType>";
            strxml += "<SenderID>" + ApiConfig.XinYinSenderID + "</SenderID>";
            strxml += "<SendTime>" + SendTime + "</SendTime>";
            strxml += "<Version>" + ApiConfig.Version + "</Version>";
            strxml += "</Head>";
            strxml += "<Declaration>";
            strxml += "<EOrder>";
            strxml += "<OrderId>" + OrderId + "</OrderId>";
            strxml += "<IEFlag>" + IEFlag + "</IEFlag>";
            strxml += "<OrderStatus>" + OrderStatus + "</OrderStatus>";
            strxml += "<EntRecordNo>" + EntRecordNo + "</EntRecordNo>";
            strxml += "<EntRecordName>" + EntRecordName +"</EntRecordName>";
            strxml += "<OrderName>" + OrderName + "</OrderName>";
            strxml += "<OrderDocType>" + OrderDocType + "</OrderDocType>";
            strxml += "<OrderDocId>" + OrderDocId + "</OrderDocId>";
            strxml += "<OrderPhone>" + OrderPhone + "</OrderPhone>";
            strxml += "<OrderGoodTotal>" + OrderGoodTotal + "</OrderGoodTotal>";
            strxml += "<OrderGoodTotalCurr>" + OrderGoodTotalCurr + "</OrderGoodTotalCurr>";
            strxml += "<Freight>" + Freight + "</Freight>";
            strxml += "<FreightCurr>" + FreightCurr + "</FreightCurr>";
            strxml += "<Tax>" + Tax + "</Tax>";
            strxml += "<TaxCurr>" + TaxCurr + "</TaxCurr>";
            strxml += "<Note>" + Note + "</Note>";
            strxml += "<OrderDate>" + OrderDate + "</OrderDate>";
            strxml += "</EOrder>";
            strxml += "<EOrderGoods>";

            foreach (JObject item in jsonObject["Declaration"]["EOrderGoods"])
            {
                strxml += "<EOrderGood>";
                strxml += "<GNo>" + item["GNo"].ToString() + "</GNo>";
                strxml += "<ChildOrderNo>" + item["ChildOrderNo"].ToString() + "</ChildOrderNo>";
                strxml += "<StoreRecordNo>" + item["StoreRecordNo"].ToString() + "</StoreRecordNo>";
                strxml += "<StoreRecordName>" + item["StoreRecordName"].ToString() + "</StoreRecordName>";
                strxml += "<CopGNo>" + item["CopGNo"].ToString() + "</CopGNo>";
                strxml += "<CustomsListNO>" + item["CustomsListNO"].ToString() + "</CustomsListNO>";
                strxml += "<DecPrice>" + item["DecPrice"].ToString() + "</DecPrice>";
                strxml += "<Unit>" + item["Unit"].ToString() + "</Unit>";
                strxml += "<GQty>" + item["GQty"].ToString() + "</GQty>";
                strxml += "<DeclTotal>" + item["DeclTotal"].ToString() + "</DeclTotal>";
                strxml += "<Notes>" + item["Notes"].ToString() + "</Notes>";
                strxml += "</EOrderGood>";
            }
            strxml += "</EOrderGoods>";
            strxml += "</Declaration>";
            strxml += "</Manifest>";
            try
            {
                strxml = AESHelper.Encrypt(strxml);
                //上传xml文件
                MemoryStream stream = new MemoryStream();
                byte[] buffer = Encoding.Default.GetBytes(strxml);
                string msg = "";
                string _ftpImageServer = ApiConfig.FtpUrl + "UPLOAD/";
                string _ftpUserName = ApiConfig.FtpUserName;
                string _ftpPassword = ApiConfig.FtpPassword;
                FtpUtil ftp = new FtpUtil(_ftpImageServer, _ftpUserName, _ftpPassword);
                //上传xml文件
                ftp.UploadFile(_ftpImageServer, ApiConfig.MessageType + SendTime + MessageIDNo + ".xml", buffer, out msg);
                return ApiConfig.MessageType + SendTime + MessageIDNo + ".xml";
            }
            catch (Exception e)
            {
                //显示错误信息  
                return "";
            }
        }
        /// <summary>
        /// 下载xml文件 
        /// 2015-10-09 王耀发 创建
        /// </summary>
        /// <param name="RequestText"></param>
        public static string DownloadXmlFile(string RequestText)
        {
            //RequestText = "{\"FileName\":\"880020201510101023450000.xml\"}";
            var jsonObject = JObject.Parse(RequestText);
            string FileName = jsonObject["FileName"].ToString();
            string _ftpImageServer = ApiConfig.FtpUrl + "DOWNLOAD/";
            string _ftpUserName = ApiConfig.FtpUserName;
            string _ftpPassword = ApiConfig.FtpPassword;
            FtpUtil ftp = new FtpUtil(_ftpImageServer, _ftpUserName, _ftpPassword);
            string msg = "";
            ftp.DownloadFile(_ftpImageServer + FileName, HttpContext.Current.Server.MapPath("~/Xml"), out msg);
            StreamReader objReader = new StreamReader(HttpContext.Current.Server.MapPath("~/Xml") + "\\" + FileName);
            string sLine = objReader.ReadToEnd();
            objReader.Close();
            //删除对应文件
            if (File.Exists(HttpContext.Current.Server.MapPath("~/Xml") + "\\" + FileName))
            {
                File.Delete(HttpContext.Current.Server.MapPath("~/Xml") + "\\" + FileName);
            }
            return AESHelper.Decrypt(sLine);
        }
        //===================================================================
        #region 心怡科技物流
        public static string appkey_xinyi = "1111";
        public static string AppSecret_xinyi = "1111";
        public static string datatype_xinyi = "json";
        public static string method_xinyi = "OMS_PUSH_PLT_PRODUCT_SKU_V2";
        public static string url_xinyi = "http://183.63.175.210:9919/gateway.aspx";
        public static string timestamp_xinyi = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        /// <summary>
        /// 推送订单到心怡科技物流
        /// </summary>
        /// <returns>2015-10-10 王耀发 创建</returns>
        public static string SendOrderToXinYi(string postdata)
        {
            postdata = "{\"Goods\": [{\"ShortName\": \"试商\",\"ShorthandCodes\": \"123\",\"SecBARCode\": \"123456789\",\"Manufactory\": \"心怡\",";
            postdata += "\"Brand\": \"第一品牌\",\"Quality\": \"12\",\"Original\": \"110\",\"PurchasePlace\": \"1101\",\"PackageType\": \"005\",\"QualityCertify\": \"合格\",\"GoodsBatchNo\": \"合格\",";
            postdata += "\"IttNumber\": \"5000000\",\"GrossWt\": \"30\",\"NetWt\": \"28\",\"Volume\": \"10\",\"ExpirationDate\": \"360\",\"CodeTS\": \"123456\",\"DecPrice\": \"99\",\"PostTariffCode\": 2060000,\"IEFlag\": \"I\",";
            postdata += "\"PostTariffName\": \"行邮税\",\"GNote\": \"http://www.baidu.com\",\"HSTax\": \"0\",\"PostTax\": \"0\",\"HSCode\": \"1234\",\"TradeCountry\": \"110\",\"WarehouseCode\": \"STORE_GZAP\",\"OnNumber\": \"1013\",";
            postdata += "\"GNo\": \"5\",\"CopGNo\": \"HT-B75-000009\",\"ProGNo\": \"871101201510121033\",\"GName\": \"全绿商品881101201503253550\",\"GModel\": \"测试规格\",\"BARCode\": \"1234567890\",\"Unit\": \"7\",";
            postdata += "\"SecUnit\": \"7\",\"GoodsMes\": \"测试商品\",\"Notes\": \"Test\",\"ItSkuColor\": \"颜色\",\"ItSkuSize\": \"大小\",\"OpType\": \"新增\"}]}";

            ///获得接口返回值
            var sAPIResult = "";
            try
            {
                sAPIResult = Post(url_xinyi, postdata);
            }
            catch (Exception ex)
            {
                sAPIResult = ex.Message;
            }
            return sAPIResult;
        }
        /// <summary>
        /// Post数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <returns>2015-10-10 王耀发 创建</returns>
        public static string Post(string url, string postdata)
        {
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("appkey", appkey_xinyi);
            parameters.Add("method", method_xinyi);
            parameters.Add("timestamp", timestamp_xinyi);
            parameters.Add("datatype", datatype_xinyi);
            parameters.Add("postdata", System.Web.HttpUtility.UrlEncode(postdata));
            string sign = CreateSign(postdata, timestamp_xinyi, method_xinyi, AppSecret_xinyi);

            url += "?appkey=" + parameters["appkey"];
            url += "&method=" + parameters["method"];
            url += "&timestamp=" + parameters["timestamp"];
            url += "&datatype=" + parameters["datatype"];
            url += "&sign=" + sign;
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("postdata={0}", parameters["postdata"]);
            return HttpResponse(url, "utf-8", sb.ToString(), "post");
        }
        /// <summary>
        /// post 请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="encode"></param>
        /// <param name="data"></param>
        /// <param name="method"></param>
        /// <returns>2015-10-10 王耀发 创建</returns>
        static string HttpResponse(string url, string encode = "utf-8", string data = "", string method = "get")
        {
            HttpWebRequest request;
            HttpWebResponse response;
            request = WebRequest.Create(url) as HttpWebRequest;
            request.ProtocolVersion = HttpVersion.Version10;
            request.AllowAutoRedirect = true;
            request.KeepAlive = true;
            request.Headers.Add("Accept-Language", "zh-cn");
            request.Accept = "*/*";
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.2; Trident/4.0; .NET CLR 2.0.50727; .NET CLR 3.0.04506.30; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729; InfoPath.2; CIBA; .NET4.0C; .NET4.0E)";
            request.Method = (string.IsNullOrEmpty(method) ? "get" : method);


            if (request.Method.ToLower().Equals("post") && !string.IsNullOrEmpty(data))
            {
                byte[] b = Encoding.GetEncoding(encode).GetBytes(data);
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = b.Length;
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(b, 0, b.Length);
                }
            }
            string html = string.Empty;
            using (response = request.GetResponse() as HttpWebResponse)
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encode)))
                {

                    html = reader.ReadToEnd();
                }
            }
            return html;
        }

        /// <summary>
        /// 给请求签名
        /// </summary>
        /// <param name="postData">请求的数据包</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="method">API接口名称</param>
        /// <param name="secret">签名密钥</param>
        /// <returns>签名</returns>
        public static string CreateSign(string postData, string timestamp, string method, string secret)
        {
            string content = postData + timestamp + method + secret;
            return EncrypMD5(content);

        }
        /// <summary>
        /// base64 MD5加密
        /// </summary>
        /// <param name="content">要加密的字串</param>
        /// <returns>加密后的数字字串</returns>
        public static string EncrypMD5(string content)
        {
            Byte[] bytes = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(content));
            return Convert.ToBase64String(bytes);
        }
        #endregion


        #region 支付宝对接海关
        #region 字段
        //支付宝网关地址（新）
        private static string GATEWAY_NEW = "https://mapi.alipay.com/gateway.do?";
        //商户的私钥
        private static string _key = "ag6658faeld0ga325xsoktysuvguhzhh";
        //编码格式
        private static string _input_charset = "utf-8";
        //签名方式
        private static string _sign_type = "MD5";
        #endregion

        public static string partner = "2088021763016271";
        public static string service = "alipay.acquire.customs";
        //public static string out_request_no = DateTime.Now.ToString("yyyyMMdd") + "0002";
        //public static string trade_no = "2015051446800462";
        public static string merchant_customs_code = "IE150928733362";
        public static string merchant_customs_name = "全绿商贸";
        //public static string amount = "200.15";
        public static string customs_place = "GUANGZHOU";

        //把请求参数打包成数组
        public static string SendOrderToAlipay(string postdata)
        {
            postdata = "{\"out_request_no\": \"20151015001\",\"trade_no\": \"2015051446800462\",\"amount\": \"200.15\"}";
            var jsonObject = JObject.Parse(postdata);
            string out_request_no = jsonObject["out_request_no"].ToString();
            string trade_no = jsonObject["trade_no"].ToString();
            string amount = jsonObject["amount"].ToString();

            SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            sParaTemp.Add("partner", partner);
            sParaTemp.Add("_input_charset", _input_charset.ToLower());
            sParaTemp.Add("service", service);
            sParaTemp.Add("out_request_no", out_request_no);
            sParaTemp.Add("trade_no", trade_no);
            sParaTemp.Add("merchant_customs_code", merchant_customs_code);
            sParaTemp.Add("merchant_customs_name", merchant_customs_name);
            sParaTemp.Add("amount", amount);
            sParaTemp.Add("customs_place", customs_place);

            //建立请求
            string sHtmlText = BuildRequest(sParaTemp);
            string msg = "";
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.LoadXml(sHtmlText);
                string is_success = xmlDoc.SelectSingleNode("/alipay/is_success").InnerText;
                if (is_success == "F")
                {
                    string error = xmlDoc.SelectSingleNode("/alipay/error").InnerText;
                    switch (error)
                    {
                        case "ILLEGAL_SIGN":
                            msg = "签名不正确";
                            break;
                        case "ILLEGAL_DYN_MD5_KEY":
                            msg = "动态密钥信息错误";
                            break;
                        case "ILLEGAL_ENCRYPT":
                            msg = "加密不正确";
                            break;
                        case "ILLEGAL_SERVICE":
                            msg = "Service参数不正确";
                            break;
                        case "ILLEGAL_USER":
                            msg = "用户ID不正确";
                            break;
                        case "ILLEGAL_PARTNER":
                            msg = "合作伙伴ID不正确";
                            break;
                        case "ILLEGAL_EXTERFACE":
                            msg = "接口配置不正确";
                            break;
                        case "ILLEGAL_PARTNER_EXTERFACE":
                            msg = "合作伙伴接口信息不正确";
                            break;
                        case "ILLEGAL_SECURITY_PROFILE":
                            msg = "未找到匹配的密钥配置";
                            break;
                        case "ILLEGAL_AGENT":
                            msg = "代理ID不正确";
                            break;
                        case "ILLEGAL_SIGN_TYPE":
                            msg = "签名类型不正确";
                            break;
                        case "ILLEGAL_CHARSET":
                            msg = "字符集不合法";
                            break;
                        case "HAS_NO_PRIVILEGE":
                            msg = "无权访问";
                            break;
                        case "INVALID_CHARACTER_SET":
                            msg = "字符集无效";
                            break;
                        case "SYSTEM_ERROR":
                            msg = "支付宝系统错误";
                            break;
                        case "SESSION_TIMEOUT":
                            msg = "session 超时";
                            break;
                        case "ILLEGAL_TARGET_SERVICE":
                            msg = "错误的target_service";
                            break;
                        case "ILLEGAL_ACCESS_SWITCH_SYSTEM":
                            msg = "partner 不允许访问该类型的系统";
                            break;
                        case "EXTERFACE_IS_CLOSED":
                            msg = "接口已关闭";
                            break;
                        default:
                            msg = "";
                            break;
                    }
                }
                if (is_success == "T")
                {

                }
                //Response.Write(strXmlResponse);
            }
            catch (Exception exp)
            {
                //Response.Write(sHtmlText);
            }
            return "";
        }
        

        #region sumit
        /// <summary>
        /// 生成请求时的签名
        /// </summary>
        /// <param name="sPara">请求给支付宝的参数数组</param>
        /// <returns>签名结果</returns>
        private static string BuildRequestMysign(Dictionary<string, string> sPara)
        {
            //把数组所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串
            string prestr = CreateLinkString(sPara);

            //把最终的字符串签名，获得签名结果
            string mysign = "";
            switch (_sign_type)
            {
                case "MD5":
                    mysign = Sign(prestr, _key, _input_charset);
                    break;
                default:
                    mysign = "";
                    break;
            }

            return mysign;
        }

        /// <summary>
        /// 生成要请求给支付宝的参数数组
        /// </summary>
        /// <param name="sParaTemp">请求前的参数数组</param>
        /// <returns>要请求的参数数组</returns>
        private static Dictionary<string, string> BuildRequestPara(SortedDictionary<string, string> sParaTemp)
        {
            //待签名请求参数数组
            Dictionary<string, string> sPara = new Dictionary<string, string>();
            //签名结果
            string mysign = "";

            //过滤签名参数数组
            sPara = FilterPara(sParaTemp);

            //获得签名结果
            mysign = BuildRequestMysign(sPara);

            //签名结果与签名方式加入请求提交参数组中
            sPara.Add("sign", mysign);
            sPara.Add("sign_type", _sign_type);

            return sPara;
        }

        /// <summary>
        /// 生成要请求给支付宝的参数数组
        /// </summary>
        /// <param name="sParaTemp">请求前的参数数组</param>
        /// <param name="code">字符编码</param>
        /// <returns>要请求的参数数组字符串</returns>
        private static string BuildRequestParaToString(SortedDictionary<string, string> sParaTemp, Encoding code)
        {
            //待签名请求参数数组
            Dictionary<string, string> sPara = new Dictionary<string, string>();
            sPara = BuildRequestPara(sParaTemp);

            //把参数组中所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串，并对参数值做urlencode
            string strRequestData = CreateLinkStringUrlencode(sPara, code);

            return strRequestData;
        }

        /// <summary>
        /// 建立请求，以表单HTML形式构造（默认）
        /// </summary>
        /// <param name="sParaTemp">请求参数数组</param>
        /// <param name="strMethod">提交方式。两个值可选：post、get</param>
        /// <param name="strButtonValue">确认按钮显示文字</param>
        /// <returns>提交表单HTML文本</returns>
        public static string BuildRequest(SortedDictionary<string, string> sParaTemp, string strMethod, string strButtonValue)
        {
            //待请求参数数组
            Dictionary<string, string> dicPara = new Dictionary<string, string>();
            dicPara = BuildRequestPara(sParaTemp);

            StringBuilder sbHtml = new StringBuilder();

            sbHtml.Append("<form id='alipaysubmit' name='alipaysubmit' action='" + GATEWAY_NEW + "_input_charset=" + _input_charset + "' method='" + strMethod.ToLower().Trim() + "'>");

            foreach (KeyValuePair<string, string> temp in dicPara)
            {
                sbHtml.Append("<input type='hidden' name='" + temp.Key + "' value='" + temp.Value + "'/>");
            }

            //submit按钮控件请不要含有name属性
            sbHtml.Append("<input type='submit' value='" + strButtonValue + "' style='display:none;'></form>");

            sbHtml.Append("<script>document.forms['alipaysubmit'].submit();</script>");

            return sbHtml.ToString();
        }


        /// <summary>
        /// 建立请求，以模拟远程HTTP的POST请求方式构造并获取支付宝的处理结果
        /// </summary>
        /// <param name="sParaTemp">请求参数数组</param>
        /// <returns>支付宝处理结果</returns>
        public static string BuildRequest(SortedDictionary<string, string> sParaTemp)
        {
            Encoding code = Encoding.GetEncoding(_input_charset);

            //待请求参数数组字符串
            string strRequestData = BuildRequestParaToString(sParaTemp,code);

            //把数组转换成流中所需字节数组类型
            byte[] bytesRequestData = code.GetBytes(strRequestData);

            //构造请求地址
            string strUrl = GATEWAY_NEW + "_input_charset=" + _input_charset;

            //请求远程HTTP
            string strResult = "";
            try
            {
                //设置HttpWebRequest基本信息
                HttpWebRequest myReq = (HttpWebRequest)HttpWebRequest.Create(strUrl);
                myReq.Method = "post";
                myReq.ContentType = "application/x-www-form-urlencoded";

                //填充POST数据
                myReq.ContentLength = bytesRequestData.Length;
                Stream requestStream = myReq.GetRequestStream();
                requestStream.Write(bytesRequestData, 0, bytesRequestData.Length);
                requestStream.Close();

                //发送POST数据请求服务器
                HttpWebResponse HttpWResp = (HttpWebResponse)myReq.GetResponse();
                Stream myStream = HttpWResp.GetResponseStream();

                //获取服务器返回信息
                StreamReader reader = new StreamReader(myStream, code);
                StringBuilder responseData = new StringBuilder();
                String line;
                while ((line = reader.ReadLine()) != null)
                {
                    responseData.Append(line);
                }

                //释放
                myStream.Close();

                strResult = responseData.ToString();
            }
            catch (Exception exp)
            {
                strResult = "报错："+exp.Message;
            }

            return strResult;
        }

        /// <summary>
        /// 建立请求，以模拟远程HTTP的POST请求方式构造并获取支付宝的处理结果，带文件上传功能
        /// </summary>
        /// <param name="sParaTemp">请求参数数组</param>
        /// <param name="strMethod">提交方式。两个值可选：post、get</param>
        /// <param name="fileName">文件绝对路径</param>
        /// <param name="data">文件数据</param>
        /// <param name="contentType">文件内容类型</param>
        /// <param name="lengthFile">文件长度</param>
        /// <returns>支付宝处理结果</returns>
        public static string BuildRequest(SortedDictionary<string, string> sParaTemp, string strMethod, string fileName, byte[] data, string contentType, int lengthFile)
        {

            //待请求参数数组
            Dictionary<string, string> dicPara = new Dictionary<string, string>();
            dicPara = BuildRequestPara(sParaTemp);

            //构造请求地址
            string strUrl = GATEWAY_NEW + "_input_charset=" + _input_charset;

            //设置HttpWebRequest基本信息
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(strUrl);
            //设置请求方式：get、post
            request.Method = strMethod;
            //设置boundaryValue
            string boundaryValue = DateTime.Now.Ticks.ToString("x");
            string boundary = "--" + boundaryValue;
            request.ContentType = "\r\nmultipart/form-data; boundary=" + boundaryValue;
            //设置KeepAlive
            request.KeepAlive = true;
            //设置请求数据，拼接成字符串
            StringBuilder sbHtml = new StringBuilder();
            foreach (KeyValuePair<string, string> key in dicPara)
            {
                sbHtml.Append(boundary + "\r\nContent-Disposition: form-data; name=\"" + key.Key + "\"\r\n\r\n" + key.Value + "\r\n");
            }
            sbHtml.Append(boundary + "\r\nContent-Disposition: form-data; name=\"withhold_file\"; filename=\"");
            sbHtml.Append(fileName);
            sbHtml.Append("\"\r\nContent-Type: " + contentType + "\r\n\r\n");
            string postHeader = sbHtml.ToString();
            //将请求数据字符串类型根据编码格式转换成字节流
            Encoding code = Encoding.GetEncoding(_input_charset);
            byte[] postHeaderBytes = code.GetBytes(postHeader);
            byte[] boundayBytes = Encoding.ASCII.GetBytes("\r\n" + boundary + "--\r\n");
            //设置长度
            long length = postHeaderBytes.Length + lengthFile + boundayBytes.Length;
            request.ContentLength = length;

            //请求远程HTTP
            Stream requestStream = request.GetRequestStream();
            Stream myStream;
            try
            {
                //发送数据请求服务器
                requestStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
                requestStream.Write(data, 0, lengthFile);
                requestStream.Write(boundayBytes, 0, boundayBytes.Length);
                HttpWebResponse HttpWResp = (HttpWebResponse)request.GetResponse();
                myStream = HttpWResp.GetResponseStream();
            }
            catch (WebException e)
            {
                return e.ToString();
            }
            finally
            {
                if (requestStream != null)
                {
                    requestStream.Close();
                }
            }

            //读取支付宝返回处理结果
            StreamReader reader = new StreamReader(myStream, code);
            StringBuilder responseData = new StringBuilder();

            String line;
            while ((line = reader.ReadLine()) != null)
            {
                responseData.Append(line);
            }
            myStream.Close();
            return responseData.ToString();
        }

        /// <summary>
        /// 用于防钓鱼，调用接口query_timestamp来获取时间戳的处理函数
        /// 注意：远程解析XML出错，与IIS服务器配置有关
        /// </summary>
        /// <returns>时间戳字符串</returns>
        public static string Query_timestamp()
        {
            string url = GATEWAY_NEW + "service=query_timestamp&partner=" + partner + "&_input_charset=" + _input_charset;
            string encrypt_key = "";

            XmlTextReader Reader = new XmlTextReader(url);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(Reader);

            encrypt_key = xmlDoc.SelectSingleNode("/alipay/response/timestamp/encrypt_key").InnerText;

            return encrypt_key;
        }
        #endregion

        #region Core
       /// <summary>
        /// 除去数组中的空值和签名参数并以字母a到z的顺序排序
        /// </summary>
        /// <param name="dicArrayPre">过滤前的参数组</param>
        /// <returns>过滤后的参数组</returns>
        public static Dictionary<string, string> FilterPara(SortedDictionary<string, string> dicArrayPre)
        {
            Dictionary<string, string> dicArray = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> temp in dicArrayPre)
            {
                if (temp.Key.ToLower() != "sign" && temp.Key.ToLower() != "sign_type" && temp.Value != "" && temp.Value != null)
                {
                    dicArray.Add(temp.Key, temp.Value);
                }
            }

            return dicArray;
        }

        /// <summary>
        /// 把数组所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串
        /// </summary>
        /// <param name="sArray">需要拼接的数组</param>
        /// <returns>拼接完成以后的字符串</returns>
        public static string CreateLinkString(Dictionary<string, string> dicArray)
        {
            StringBuilder prestr = new StringBuilder();
            foreach (KeyValuePair<string, string> temp in dicArray)
            {
                prestr.Append(temp.Key + "=" + temp.Value + "&");
            }

            //去掉最後一個&字符
            int nLen = prestr.Length;
            prestr.Remove(nLen-1,1);

            return prestr.ToString();
        }

        /// <summary>
        /// 把数组所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串，并对参数值做urlencode
        /// </summary>
        /// <param name="sArray">需要拼接的数组</param>
        /// <param name="code">字符编码</param>
        /// <returns>拼接完成以后的字符串</returns>
        public static string CreateLinkStringUrlencode(Dictionary<string, string> dicArray, Encoding code)
        {
            StringBuilder prestr = new StringBuilder();
            foreach (KeyValuePair<string, string> temp in dicArray)
            {
                prestr.Append(temp.Key + "=" + HttpUtility.UrlEncode(temp.Value, code) + "&");
            }

            //去掉最後一個&字符
            int nLen = prestr.Length;
            prestr.Remove(nLen - 1, 1);

            return prestr.ToString();
        }

        /// <summary>
        /// 写日志，方便测试（看网站需求，也可以改成把记录存入数据库）
        /// </summary>
        /// <param name="sWord">要写入日志里的文本内容</param>
        public static void LogResult(string sWord)
        {
            string strPath = HttpContext.Current.Server.MapPath("log");
            strPath = strPath + "\\" + DateTime.Now.ToString().Replace(":", "") + ".txt";
            StreamWriter fs = new StreamWriter(strPath, false, System.Text.Encoding.Default);
            fs.Write(sWord);
            fs.Close();
        }

        /// <summary>
        /// 获取文件的md5摘要
        /// </summary>
        /// <param name="sFile">文件流</param>
        /// <returns>MD5摘要结果</returns>
        public static string GetAbstractToMD5(Stream sFile)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(sFile);
            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < result.Length; i++)
            {
                sb.Append(result[i].ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取文件的md5摘要
        /// </summary>
        /// <param name="dataFile">文件流</param>
        /// <returns>MD5摘要结果</returns>
        public static string GetAbstractToMD5(byte[] dataFile)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(dataFile);
            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < result.Length; i++)
            {
                sb.Append(result[i].ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString();
        }

        #endregion

        #region MD5
        /// <summary>
        /// 签名字符串
        /// </summary>
        /// <param name="prestr">需要签名的字符串</param>
        /// <param name="key">密钥</param>
        /// <param name="_input_charset">编码格式</param>
        /// <returns>签名结果</returns>
        public static string Sign(string prestr, string key, string _input_charset)
        {
            StringBuilder sb = new StringBuilder(32);

            prestr = prestr + key;

            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(Encoding.GetEncoding(_input_charset).GetBytes(prestr));
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            }

            return sb.ToString();
        }

        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="prestr">需要签名的字符串</param>
        /// <param name="sign">签名结果</param>
        /// <param name="key">密钥</param>
        /// <param name="_input_charset">编码格式</param>
        /// <returns>验证结果</returns>
        public static bool Verify(string prestr, string sign, string key, string _input_charset)
        {
            string mysign = Sign(prestr, key, _input_charset);
            if (mysign == sign)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
        #endregion
    }
}
