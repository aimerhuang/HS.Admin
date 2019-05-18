using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Net.Sockets;
using Hyt.BLL.Base;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Hyt.Model.ExpressList;
using System.Web.Script.Serialization;
using System.Runtime.Serialization.Json;
using System.Xml.Serialization;
using System.Xml;
using Hyt.BLL.YTO;
using Hyt.Model.TYO;
using Extra.Express.Provider;


namespace Hyt.UnitTest
{
    /// <summary>
    /// OrderNumber 的摘要说明
    /// 对接快递100 API 
    /// </summary>
    [TestClass]
    public class OrderNumber
    {
        public OrderNumber()
        {
            DataProviderBo.Set(Activator.CreateInstance(Type.GetType("Hyt.DataAccess.Oracle.DataProvider,Hyt.DataAccess.Oracle")));
        }

        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，该上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 附加测试特性
        //
        // 编写测试时，可以使用以下附加特性: 
        //
        // 在运行类中的第一个测试之前使用 ClassInitialize 运行代码
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // 在类中的所有测试都已运行之后使用 ClassCleanup 运行代码
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 在运行每个测试之前，使用 TestInitialize 来运行代码
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // 在每个测试运行完之后，使用 TestCleanup 来运行代码
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion


        [TestMethod]
        public void TestMethod1()
        {
            // TODO:  在此处添加测试逻辑
            //var query = YTORequestOrder();
            //var query1 = KK();

            KdOrderParam pa = new KdOrderParam();
            pa = BLL.ExpressList.ExpressListBLL.Instance.GetKdOrderParam(22771);
            pa.recMan = BLL.ExpressList.ExpressListBLL.Instance.GetSRecMan(22771);
            pa.sendMan = BLL.ExpressList.ExpressListBLL.Instance.GetRecMan(22771);
            pa.kuaidicom = "zhaijisong";
            pa.sendMan.mobile = "13265552415";
            pa.sendMan.printAddr = "邵阳市";
            pa.payType = "";
            //var result = ExpressProvider.GetInstance(0).OrderTracesSubByJson(pa);
            //var result = BLL.Express.ElectronicsSurfaceBo.Instance.OrderTracesSubByJson(pa);
        }
        #region  圆通 电子面单  廖移凤 2017-12-8
        ///// <summary>
        ///// 圆通 电子面单
        ///// </summary>
        ///// <returns></returns>
        //public string YTORequestOrder()
        //{

        //    //测试密钥
        //    string clientId = "K21000119";
        //    string partnerId = "u2Z1F7Fh";
        //    string customerId = "K21000119";
        //    //测试请求url
        //    string ReqURL = "http://58.32.246.71:8000/CommonOrderModeBPlusServlet.action";
        //    #region  圆通 电子面单  参数
        //    RequestOrder sf1 = YTOGetParamBo.Instance.GetRequestOrder(22771);

        //    sf1.clientID = clientId;
        //    sf1.customerId = customerId;
        //    sf1.orderType = 1;
        //    sf1.txLogisticID = clientId + sf1.txLogisticID;
        //    sf1.logisticProviderID = "YTO";
        //    sf1.serviceType = 0;
        //    sf1.receiver = YTOGetParamBo.Instance.GetReceiver(22771);
        //    sf1.receiver.prov = "广东";
        //    sf1.receiver.city = "深圳市,龙岗区";
        //    sf1.receiver.mobile = "13265552555";
        //    sf1.receiver.address = "甘李路1号";
        //    sf1.sender = YTOGetParamBo.Instance.GetSender(22771);
        //    sf1.sender.prov = "广东";
        //    sf1.sender.city = "深圳市,龙岗区";
        //    sf1.sender.address = "甘李路1号";


        //    //RequestOrder sf = new RequestOrder()
        //    //{
        //    //    clientID = clientId,
        //    //    customerId = customerId,
        //    //    orderType = 1,
        //    //    logisticProviderID = "YTO",
        //    //    serviceType = 0,
        //    //    itemName = "电视",
        //    //    number = 1,
        //    //    txLogisticID = "K2100011919961215",
        //    //    receiver = new Receiver()
        //    //    {
        //    //        address = "甘李路1号",
        //    //        city = "深圳市,龙岗区",
        //    //        mobile = "13265552555",
        //    //        name = "哈哈",
        //    //        prov = "广东"
        //    //    },
        //    //    sender = new Sender()
        //    //    {
        //    //        address = "甘李路2号",
        //    //        city = "深圳市,龙岗区",
        //    //        mobile = "13266552555",
        //    //        name = "哈哈",
        //    //        prov = "广东"
        //    //    }
        //    //};
         
        //    //string str = "<RequestOrder>"
        //    //+ "    <clientID>" + clientId + "</clientID>"
        //    //+ "    <logisticProviderID>YTO</logisticProviderID>"
        //    //+ "    <customerId>" + customerId + "</customerId>"
        //    //+ "    <txLogisticID>PK10101010</txLogisticID>"
        //    //+ "    <tradeNo></tradeNo>"
        //    //+ "    <totalServiceFee>1</totalServiceFee>"
        //    //+ "    <codSplitFee>1</codSplitFee>"
        //    //+ "    <orderType>0</orderType>"
        //    //+ "    <serviceType>1</serviceType>"
        //    //+ "    <flag>1</flag>"
        //    //+ "    <sendStartTime></sendStartTime>"
        //    //+ "    <sendEndTime></sendEndTime>"
        //    //+ "    <goodsValue>1</goodsValue>"
        //    //+ "    <itemsValue>1</itemsValue>"
        //    //+ "    <insuranceValue></insuranceValue>"
        //    //+ "    <special>0</special>"
        //    //+ "    <remark></remark>"
        //    //+ "    <deliverNo>1</deliverNo>"
        //    //+ "    <type>0</type>"
        //    //+ "    <totalValue>1</totalValue>"
        //    //+ "    <itemsWeight>1</itemsWeight>"
        //    //+ "    <packageOrNot>1</packageOrNot>"
        //    //+ "    <orderSource>1</orderSource>"
        //    //+ "    <sender>"
        //    //+ "        <name>张三</name>"
        //    //+ "        <postCode>123456</postCode>"
        //    //+ "        <phone>1234567</phone>"
        //    //+ "        <mobile>18221885929</mobile>"
        //    //+ "        <prov>上海</prov>"
        //    //+ "        <city>上海,青浦区</city>"
        //    //+ "        <address>上海市青浦区华徐公路民兴大道</address>"
        //    //+ "    </sender>"
        //    //+ "    <receiver>"
        //    //+ "        <name>李四</name>"
        //    //+ "        <postCode>123456</postCode>"
        //    //+ "        <phone>1234567</phone>"
        //    //+ "        <mobile>18221885929</mobile>"
        //    //+ "        <prov>上海</prov>"
        //    //+ "        <city>崇明县,城桥镇</city>"
        //    //+ "        <address>上海崇明县城桥镇三沙洪路315弄西怡祥居23号楼</address>"
        //    //+ "    </receiver>"
        //    //+ "    <items>"
        //    //+ "        <item>"
        //    //+ "            <itemName>36ab0b08-3b5c-4423-a352-08477f050e55</itemName>"
        //    //+ "            <number>2</number>"
        //    //+ "            <itemValue>50</itemValue>"
        //    //+ "        </item>"
        //    //+ "        <item>"
        //    //+ "            <itemName>0a4e51b9-5616-4feb-b8a8-d2e1ba24401f</itemName>"
        //    //+ "            <number>2</number>"
        //    //+ "            <itemValue>50</itemValue>"
        //    //+ "        </item>"
        //    //+ "    </items>"
        //    //+ "</RequestOrder>";
        //    #endregion
        //    string str = XmlSerialize<RequestOrder>(sf1);
        //    MD5 md5Hasher = MD5.Create();
        //    Dictionary<string, string> param = new Dictionary<string, string>();
        //    param.Add("logistics_interface", HttpUtility.UrlEncode(str, Encoding.UTF8));
        //    param.Add("data_digest", HttpUtility.UrlEncode(Convert.ToBase64String(md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(str + partnerId))), Encoding.UTF8));
        //    param.Add("clientId", HttpUtility.UrlEncode(clientId, Encoding.UTF8));
        //    return SendPost(ReqURL, param);
        //}   

        //#endregion

        //#region  圆通 KK 广州跨境创建订单下发到快递接口 廖移凤 2017-12-5    XML  UTF-16  不能读取
        ///// <summary>
        ///// KK 海外 广州跨境创建订单下发到快递接口
        ///// </summary>
        ///// <returns></returns>
        //public string KK()
        //{
        //    //测试密钥
        //    // private string clientID = "SYM";
        //    // private string partnerId = "SYM";
        //    //正式密钥
        //    string clientID = "XRC";
        //    string partnerId = "XRC";
        //    //测试请求url
        //    //private string ReqURL = "http://180.153.190.90/globalunion/outcall/blcwaybillgzhg";
        //    //正式请求url
        //    string ReqURL = "http://lmtest.yto.net.cn/globalunion/outcall/blcwaybillgzhg";
        //    #region  圆通 KK 广州跨境创建订单下发到快递接口 参数
        //    tmsWayBillNotifyRequest tbr1 = TmsWayBillNotifyImplBo.Instance.GetTmsWayBillNotifyRequest(22771);
        //    tbr1.clientId = "TESTSTD";
        //    tbr1.customsID = "GZHG";
        //    tbr1.orderCode = "4598662258888";
        //    tbr1.waybill = "4598662258888";
        //    tbr1.customsCode = "4604";
        //    tbr1.importDateStr = "2016-06-17 16:50:46";
        //    tbr1.currCode = "142";
        //    tbr1.modifyMark = "A";
        //    //tmsWayBillNotifyRequest tbr = new tmsWayBillNotifyRequest()
        //    //{
        //    //    clientId = "TESTSTD",
        //    //    customsID = "GZHG",
        //    //    orderCode = "LP00080188528111",
        //    //    waybill = "4598662258876",

        //    //    sendArea = "深圳",

        //    //    tradeId = "6598528865",
        //    //    goodsName = "电视",
        //    //    consigneeArea = "深圳",
        //    //    consignee = "haha",
        //    //    consigneeAddress = "甘李路1号",
        //    //    consigneeTel = "13265552555",

        //    //    customsCode = "4604",
        //    //    importDateStr = "2016-06-17 16:50:46",
        //    //    currCode = "142",
        //    //    modifyMark = "A",
        //    //};
            
        //    // string str = "<?xml version='1.0' encoding='utf-8'?>" +
        //   //" <tmsWayBillNotifyRequest>" +
        //   //   "  <clientId>TESTSTD</clientId>" +
        //   //   "  <customsID>GZHG</customsID>" +
        //   //   "  <tradeId>JY161534</tradeId>" +
        //   //   "  <orderCode>LP001801080901111</orderCode>" +
        //   //   "  <waybill>8018823752111</waybill>" +
        //   //   "  <totalWayBill>8096583848122</totalWayBill>" +
        //   //   "  <packNo>1</packNo>" +
        //   //   "  <grossWeigt>5.3</grossWeigt>" +
        //   //   "  <netWeight>5.2</netWeight>" +
        //   //   "  <goodsName>主要货物名称</goodsName>" +
        //   //   "  <sendArea>发件地区</sendArea>" +
        //   //   "  <consigneeArea>收件地区</consigneeArea>" +
        //   //   "  <consignee>收件人名称</consignee>" +
        //   //   "  <consigneeAddress>收件人地址</consigneeAddress>" +
        //   //   "  <consigneeTel>15212792970</consigneeTel>" +
        //   //   "  <zipCode>230000</zipCode>" +
        //   //   "  <customsCode>4604</customsCode>" +
        //   //   "  <worth>20</worth>" +
        //   //   "  <importDateStr>2016-06-17 16:50:46</importDateStr>" +
        //   //   "  <currCode>142</currCode>" +
        //   //   "  <modifyMark>A</modifyMark>" +
        //   //   "  <businessType>1</businessType>" +
        //   //   "  <insuredFee>10</insuredFee>" +
        //   //   "  <freight>10</freight>" +
        //   //   "  <feature>扩展字段</feature>" +
        //    //" </tmsWayBillNotifyRequest>";
        //    #endregion
        //    string str = XmlSerialize<tmsWayBillNotifyRequest>(tbr1);
        //    MD5 md5Hasher = MD5.Create();
        //    Dictionary<string, string> param = new Dictionary<string, string>();
        //    param.Add("logistics_interface", str);
        //    param.Add("clientID", clientID);
        //    param.Add("data_digest",  HttpUtility.UrlEncode(Convert.ToBase64String(md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(partnerId + str + partnerId))), Encoding.UTF8));
        //    return SendPost(ReqURL, param);
        //}
        //#endregion

        //#region  圆通 走件流程查询接口  廖移凤 2017-12-8
        //////测试密钥
        ////private string clientId = "K21000119";
        ////private string partnerId = "u2Z1F7Fh";
        ////private string customerId = "K21000119";
        //////测试请求url
        ////private string ReqURL = "http://58.32.246.71:8000/CommonOrderModeBPlusServlet.action";

        /////// <summary>
        /////// 圆通 电子面单
        /////// </summary>
        /////// <returns></returns>
        ////public string OrderTracesSubByJson()
        ////{
        ////    string str = "<RequestOrder>"
        ////    + "    <clientID>" + clientId + "</clientID>"
        ////    + "    <logisticProviderID>YTO</logisticProviderID>"
        ////    + "    <customerId>" + customerId + "</customerId>"
        ////    + "    <txLogisticID>PK10101010</txLogisticID>"
        ////    + "    <tradeNo></tradeNo>"
        ////    + "    <totalServiceFee>1</totalServiceFee>"
        ////    + "    <codSplitFee>1</codSplitFee>"
        ////    + "    <orderType>0</orderType>"
        ////    + "    <serviceType>1</serviceType>"
        ////    + "    <flag>1</flag>"
        ////    + "    <sendStartTime></sendStartTime>"
        ////    + "    <sendEndTime></sendEndTime>"
        ////    + "    <goodsValue>1</goodsValue>"
        ////    + "    <itemsValue>1</itemsValue>"
        ////    + "    <insuranceValue></insuranceValue>"
        ////    + "    <special>0</special>"
        ////    + "    <remark></remark>"
        ////    + "    <deliverNo>1</deliverNo>"
        ////    + "    <type>0</type>"
        ////    + "    <totalValue>1</totalValue>"
        ////    + "    <itemsWeight>1</itemsWeight>"
        ////    + "    <packageOrNot>1</packageOrNot>"
        ////    + "    <orderSource>1</orderSource>"
        ////    + "    <sender>"
        ////    + "        <name>张三</name>"
        ////    + "        <postCode>123456</postCode>"
        ////    + "        <phone>1234567</phone>"
        ////    + "        <mobile>18221885929</mobile>"
        ////    + "        <prov>上海</prov>"
        ////    + "        <city>上海,青浦区</city>"
        ////    + "        <address>上海市青浦区华徐公路民兴大道</address>"
        ////    + "    </sender>"
        ////    + "    <receiver>"
        ////    + "        <name>李四</name>"
        ////    + "        <postCode>123456</postCode>"
        ////    + "        <phone>1234567</phone>"
        ////    + "        <mobile>18221885929</mobile>"
        ////    + "        <prov>上海</prov>"
        ////    + "        <city>崇明县,城桥镇</city>"
        ////    + "        <address>上海崇明县城桥镇三沙洪路315弄西怡祥居23号楼</address>"
        ////    + "    </receiver>"
        ////    + "    <items>"
        ////    + "        <item>"
        ////    + "            <itemName>36ab0b08-3b5c-4423-a352-08477f050e55</itemName>"
        ////    + "            <number>2</number>"
        ////    + "            <itemValue>50</itemValue>"
        ////    + "        </item>"
        ////    + "        <item>"
        ////    + "            <itemName>0a4e51b9-5616-4feb-b8a8-d2e1ba24401f</itemName>"
        ////    + "            <number>2</number>"
        ////    + "            <itemValue>50</itemValue>"
        ////    + "        </item>"
        ////    + "    </items>"
        ////    + "</RequestOrder>";
        ////    MD5 md5Hasher = MD5.Create();
        ////    Dictionary<string, string> param = new Dictionary<string, string>();
        ////    param.Add("logistics_interface", System.Web.HttpUtility.UrlEncode(str, Encoding.UTF8));
        ////    param.Add("data_digest",HttpUtility.UrlEncode(Convert.ToBase64String(md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(str + partnerId))), Encoding.UTF8));
        ////    param.Add("clientId",HttpUtility.UrlEncode(clientId, Encoding.UTF8));
        ////    return SendPost(ReqURL, param);
        ////}

        /////// <summary>
        ///////Sign签名 
        /////// </summary>
        /////// <param name="content">内容</param>
        /////// <param name="clientID">密钥</param>
        /////// <param name="charset">URL编码</param>
        /////// <returns>Sign签名</returns>
        ////private string Encrypt(String content, String partnerId)
        ////{
        ////    return EncodingUft8(EncryptMd5utf8(content + partnerId, 32));
        ////}



        //#endregion

        //#region  公用
        ///// <summary>  
        ///// Post方式提交数据 
        ///// </summary>  
        ///// <param name="url">发送请求的 URL</param>  
        ///// <param name="param">请求的参数集合</param>  
        ///// <returns>远程资源的响应结果</returns>  
        //private string SendPost(string url, Dictionary<string, string> param)
        //{
        //    string result = "";
        //    StringBuilder postData = new StringBuilder();
        //    if (param != null && param.Count > 0)
        //    {
        //        foreach (var p in param)
        //        {
        //            if (postData.Length > 0)
        //            {
        //                postData.Append("&");
        //            }
        //            postData.Append(p.Key);
        //            postData.Append("=");
        //            postData.Append(p.Value);
        //        }
        //    }
        //    byte[] byteData = Encoding.GetEncoding("UTF-8").GetBytes(postData.ToString());
        //    try
        //    {  //发送请求
        //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        //        request.ContentType = "application/x-www-form-urlencoded";
        //        //request.Referer = url;
        //        request.Timeout = 30 * 1000;
        //        request.Method = "POST";
        //        Stream stream = request.GetRequestStream();
        //        stream.Write(byteData, 0, byteData.Length);
        //        //发送成功后接收返回的XML信息
        //        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        //        Stream backStream = response.GetResponseStream();
        //        StreamReader sr = new StreamReader(backStream, Encoding.GetEncoding("UTF-8"));
        //        result = sr.ReadToEnd();

        //    }
        //    catch (Exception ex)
        //    {
        //        result = ex.Message;
        //    }
        //    return result;
        //}

        ///// <summary>
        ///// 对象转换为XML
        ///// </summary>
        ///// <typeparam name="T">类</typeparam>
        ///// <param name="obj">对象</param>
        ///// <returns>转换后的XML</returns>
        ///// <remarks>2017-12-11 廖移凤 创建</remarks>
        //public static string XmlSerialize<T>(T obj)
        //{
        //     MemoryStream ms = new MemoryStream();
        //     StreamWriter textWriter = new StreamWriter(ms, Encoding.GetEncoding("UTF-8"));//指定编码格式
        //     XmlSerializer serializer = new XmlSerializer(obj.GetType());
        //     serializer.Serialize(textWriter, obj);
        //     string xmlMessage = Encoding.UTF8.GetString(ms.GetBuffer());
        //     ms.Close();
        //     textWriter.Close();
        //    return xmlMessage;
        //}



        ///// <summary>
        ///// 字符编码
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //public static string EncodingUft8(string input)
        //{
        //    return HttpUtility.UrlEncode(input, Encoding.UTF8);
        //}
        /////<summary>  
        ///// 字符串MD5加密 
        /////</summary>  
        /////<param name="str">要加密的字符串</param>  
        /////<returns>密文</returns>
        //public string EncryptMd5utf8(string data, int weis)//MD5+base64
        //{
        //    if (weis == 16)
        //    {

        //        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        //        string ret = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(data)), 4, 8);
        //        var query = ret.Replace("-", "");
        //        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(query));
        //    }
        //    else
        //    {
        //        MD5 md5 = new MD5CryptoServiceProvider();
        //        return Convert.ToBase64String(md5.ComputeHash(Encoding.UTF8.GetBytes(data)));
        //    }


        //}


        ///// <summary>
        ///// DesKey
        ///// </summary>
        //private static byte[] btKeys = { 61, 4, 104, (byte)(0xff & -119), 38, (byte)(0xff & -68), (byte)(0xff & -88), (byte)(0xff & -45) };
        ///// <summary>
        ///// Des加密
        ///// </summary>
        ///// <param name="encryptString">加密内容</param>
        ///// <returns></returns>
        //private string DesEncrypt(string encryptString)
        //{
        //    byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
        //    string des = string.Empty;
        //    DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
        //    provider.Mode = CipherMode.ECB;
        //    MemoryStream mStream = new MemoryStream();
        //    CryptoStream cStream = new CryptoStream(mStream, provider.CreateEncryptor(btKeys, btKeys), CryptoStreamMode.Write);
        //    cStream.Write(inputByteArray, 0, inputByteArray.Length);
        //    cStream.FlushFinalBlock();
        //    des = Convert.ToBase64String(mStream.ToArray());
        //    return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(des));
        //}

        #endregion


    }
}