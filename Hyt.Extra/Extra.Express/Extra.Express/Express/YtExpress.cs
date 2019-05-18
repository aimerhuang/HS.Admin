using Extra.Express.Model;
using Extra.Express.Provider;
using Extra.Express.Public;
using Hyt.BLL.YTO;
using Hyt.Model;
using Hyt.Model.ExpressList;
using Hyt.Model.TYO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml;
namespace Extra.Express.Express
{
    /// <summary>
    /// 圆通快递接口实现
    /// </summary>
    /// <remarks>2017-12-13 杨浩 创建</remarks>
    public class YtExpress : IExpress
    {
        #region  获取快递单号 2017-12-13 廖移凤
        /// <summary>
        /// 获取快递单号
        /// </summary>
        /// <param name="ro">快递接口参数</param>
        /// <returns></returns>
        /// <remarks>2017-12-13 杨浩 创建</remarks>
        /// <remarks>2017-12-13 廖移凤 </remarks>
        public override Result<string> GetCourierNo(ExpressParameters param)
        {
            var result = new Result<string>();
            var request =new RequestOrder();
            #region  圆通 电子面单  参数
            request.agencyFund = param.AgencyFund;
            request.clientID = Config.Yt.ClientId;  //param.clientID;
            request.codSplitFee = param.CodSplitFee;
            request.customerId = Config.Yt.CustomerId; //param.customerId;
            request.flag = param.Flag;
            request.goodsValue = param.GoodsValue;
            request.insuranceValue = param.InsuranceValue;
            request.itemName = param.ItemName;
            request.itemsValue = param.ItemsValue;
            request.itemsWeight = param.ItemsWeight;
            request.itemValue = param.ItemValue;
            request.logisticProviderID = param.LogisticProviderID;
            request.mailNo = param.MailNo;
            request.number = param.Number;
            request.sendStartTime = param.SendStartTime;
            request.orderType = param.OrderType;
            request.remark = param.Remark;
            request.sendEndTime = param.SendEndTime;
            request.serviceType = param.ServiceType;
            request.special = param.Special;
            request.totalServiceFee = param.TotalServiceFee;
            request.totalValue = param.TotalValue;
            request.tradeNo = param.TradeNo;
            request.txLogisticID = param.TxLogisticID;
            request.type = param.Type;
            //发件人信息
            request.sender.address = param.Saddress;
            request.sender.city = param.Scity;
            request.sender.mobile = param.Smobile;
            request.sender.name = param.Sname;
            request.sender.phone = param.Sphone;
            request.sender.postcode = param.Spostcode;
            request.sender.prov = param.Sprov;
            //收件人信息
            request.receiver.address = param.Raddress;
            request.receiver.city = param.Rcity;
            request.receiver.mobile = param.Rmobile;
            request.receiver.name = param.Rname;
            request.receiver.phone = param.Rphone;
            request.receiver.postcode = param.Rpostcode;
            request.receiver.prov = param.Rprov;
            //RequestOrder sf1 = YTOGetParamBo.Instance.GetRequestOrder(22771);
            //sf1.clientID = Config.Yt.ClientId;
            //sf1.customerId = Config.Yt.CustomerId;
            //sf1.orderType = 1;
            //sf1.txLogisticID = Config.Yt.CustomerId + sf1.txLogisticID;
            //sf1.logisticProviderID = "YTO";
            //sf1.serviceType = 0;
            //sf1.receiver = YTOGetParamBo.Instance.GetReceiver(22771);
            //sf1.receiver.prov = "广东";
            //sf1.receiver.city = "深圳市,龙岗区";
            //sf1.receiver.mobile = "13265552555";
            //sf1.receiver.address = "甘李路1号";
            //sf1.sender = YTOGetParamBo.Instance.GetSender(22771);
            //sf1.sender.prov = "广东";
            //sf1.sender.city = "深圳市,龙岗区";
            //sf1.sender.address = "甘李路1号";

            #region  圆通 电子面单  全部参数
            //RequestOrder sf = new RequestOrder()
            //{
            //    clientID = clientId,
            //    customerId = customerId,
            //    orderType = 1,
            //    logisticProviderID = "YTO",
            //    serviceType = 0,
            //    itemName = "电视",
            //    number = 1,
            //    txLogisticID = "K2100011919961215",
            //    receiver = new Receiver()
            //    {
            //        address = "甘李路1号",
            //        city = "深圳市,龙岗区",
            //        mobile = "13265552555",
            //        name = "哈哈",
            //        prov = "广东"
            //    },
            //    sender = new Sender()
            //    {
            //        address = "甘李路2号",
            //        city = "深圳市,龙岗区",
            //        mobile = "13266552555",
            //        name = "哈哈",
            //        prov = "广东"
            //    }
            //};

            //string str = "<RequestOrder>"
            //+ "    <clientID>" + clientId + "</clientID>"
            //+ "    <logisticProviderID>YTO</logisticProviderID>"
            //+ "    <customerId>" + customerId + "</customerId>"
            //+ "    <txLogisticID>PK10101010</txLogisticID>"
            //+ "    <tradeNo></tradeNo>"
            //+ "    <totalServiceFee>1</totalServiceFee>"
            //+ "    <codSplitFee>1</codSplitFee>"
            //+ "    <orderType>0</orderType>"
            //+ "    <serviceType>1</serviceType>"
            //+ "    <flag>1</flag>"
            //+ "    <sendStartTime></sendStartTime>"
            //+ "    <sendEndTime></sendEndTime>"
            //+ "    <goodsValue>1</goodsValue>"
            //+ "    <itemsValue>1</itemsValue>"
            //+ "    <insuranceValue></insuranceValue>"
            //+ "    <special>0</special>"
            //+ "    <remark></remark>"
            //+ "    <deliverNo>1</deliverNo>"
            //+ "    <type>0</type>"
            //+ "    <totalValue>1</totalValue>"
            //+ "    <itemsWeight>1</itemsWeight>"
            //+ "    <packageOrNot>1</packageOrNot>"
            //+ "    <orderSource>1</orderSource>"
            //+ "    <sender>"
            //+ "        <name>张三</name>"
            //+ "        <postCode>123456</postCode>"
            //+ "        <phone>1234567</phone>"
            //+ "        <mobile>18221885929</mobile>"
            //+ "        <prov>上海</prov>"
            //+ "        <city>上海,青浦区</city>"
            //+ "        <address>上海市青浦区华徐公路民兴大道</address>"
            //+ "    </sender>"
            //+ "    <receiver>"
            //+ "        <name>李四</name>"
            //+ "        <postCode>123456</postCode>"
            //+ "        <phone>1234567</phone>"
            //+ "        <mobile>18221885929</mobile>"
            //+ "        <prov>上海</prov>"
            //+ "        <city>崇明县,城桥镇</city>"
            //+ "        <address>上海崇明县城桥镇三沙洪路315弄西怡祥居23号楼</address>"
            //+ "    </receiver>"
            //+ "    <items>"
            //+ "        <item>"
            //+ "            <itemName>36ab0b08-3b5c-4423-a352-08477f050e55</itemName>"
            //+ "            <number>2</number>"
            //+ "            <itemValue>50</itemValue>"
            //+ "        </item>"
            //+ "        <item>"
            //+ "            <itemName>0a4e51b9-5616-4feb-b8a8-d2e1ba24401f</itemName>"
            //+ "            <number>2</number>"
            //+ "            <itemValue>50</itemValue>"
            //+ "        </item>"
            //+ "    </items>"
            //+ "</RequestOrder>";
            #endregion
            #endregion
            string str = SendData.XmlSerialize<RequestOrder>(request);
            MD5 md5Hasher = MD5.Create();
            Dictionary<string, string> diy = new Dictionary<string, string>();
            diy.Add("logistics_interface", HttpUtility.UrlEncode(str, Encoding.UTF8));
            diy.Add("data_digest", HttpUtility.UrlEncode(Convert.ToBase64String(md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(str + Config.Yt.PartnerId))), Encoding.UTF8));
            diy.Add("clientId", HttpUtility.UrlEncode(Config.Yt.ClientId, Encoding.UTF8));
            string str1 = SendData.SendPost(Config.Yt.ReqURL, diy);
            #region 解析XML
            DataSet ds = new DataSet();
            StringReader stream = new StringReader(str1);//读取字符串为数据量
            XmlTextReader reader = new XmlTextReader(stream);//对XML的数据流的只进只读访问
            ds.ReadXml(reader);//吧数据读入DataSet
            DataTable dt = ds.Tables["Response"];
            if (dt.Rows.Count > 0)
            {
                    DataRow dr = ds.Tables[0].Rows[0];
                    result.Message = dr["mailNo"].ToString();//快递单号
            }
            #endregion
            result.Data = str1;
            result.Status = true;
            return result;
        }
        #endregion

        #region 跨境订单下单 2017-12-13 廖移凤
        /// <summary>
        /// 跨境订单下单
        /// </summary>
        /// <param name="twbnr">参数</param>
        /// <returns></returns>
        /// <remarks>2017-12-13 廖移凤 创建</remarks>
        public override Result CreateCrossOrder(ExpressParameters param)
        {
            var result = new Result();
            var request = new tmsWayBillNotifyRequest();
            #region  圆通 KK 广州跨境创建订单下发到快递接口 参数
            request.businessType = param.BusinessType;
            request.clientId = param.ClientId;
            request.consignee = param.Consignee;
            request.consigneeAddress = param.ConsigneeAddress;
            request.consigneeArea = param.ConsigneeArea;
            request.consigneeTel = param.ConsigneeTel;
            request.currCode = param.CurrCode;
            request.customsCode = param.CustomsCode;
            request.customsID = param.CustomsID;
            request.feature = param.Feature;
            request.freight = param.Freight;
            request.goodsName = param.GoodsName;
            request.grossWeigt = param.GrossWeigt;
            request.importDateStr = param.ImportDateStr;
            request.insuredFee = param.InsuredFee;
            request.modifyMark = param.ModifyMark;
            request.netWeight = param.NetWeight;
            request.orderCode = param.OrderCode;
            request.packNo = param.PackNo;
            request.sendArea = param.SendArea;
            request.totalWayBill = param.TotalWayBill;
            request.tradeId = param.TradeId;
            request.waybill = param.Waybill;
            request.worth = param.Worth;
            request.zipCode = param.ZipCode;
            //tmsWayBillNotifyRequest tbr1 = TmsWayBillNotifyImplBo.Instance.GetTmsWayBillNotifyRequest(22771);
            //tbr1.clientId = "TESTSTD";
            //tbr1.customsID = "GZHG";
            //tbr1.orderCode = "4598662258888";
            //tbr1.waybill = "4598662258888";
            //tbr1.customsCode = "4604";
            //tbr1.importDateStr = "2016-06-17 16:50:46";
            //tbr1.currCode = "142";
            //tbr1.modifyMark = "A";
            #region  圆通 KK 广州跨境创建订单下发到快递接口 全部参数
            //tmsWayBillNotifyRequest tbr = new tmsWayBillNotifyRequest()
            //{
            //    clientId = "TESTSTD",
            //    customsID = "GZHG",
            //    orderCode = "LP00080188528111",
            //    waybill = "4598662258876",

            //    sendArea = "深圳",

            //    tradeId = "6598528865",
            //    goodsName = "电视",
            //    consigneeArea = "深圳",
            //    consignee = "haha",
            //    consigneeAddress = "甘李路1号",
            //    consigneeTel = "13265552555",

            //    customsCode = "4604",
            //    importDateStr = "2016-06-17 16:50:46",
            //    currCode = "142",
            //    modifyMark = "A",
            //};

            // string str = "<?xml version='1.0' encoding='utf-8'?>" +
            //" <tmsWayBillNotifyRequest>" +
            //   "  <clientId>TESTSTD</clientId>" +
            //   "  <customsID>GZHG</customsID>" +
            //   "  <tradeId>JY161534</tradeId>" +
            //   "  <orderCode>LP001801080901111</orderCode>" +
            //   "  <waybill>8018823752111</waybill>" +
            //   "  <totalWayBill>8096583848122</totalWayBill>" +
            //   "  <packNo>1</packNo>" +
            //   "  <grossWeigt>5.3</grossWeigt>" +
            //   "  <netWeight>5.2</netWeight>" +
            //   "  <goodsName>主要货物名称</goodsName>" +
            //   "  <sendArea>发件地区</sendArea>" +
            //   "  <consigneeArea>收件地区</consigneeArea>" +
            //   "  <consignee>收件人名称</consignee>" +
            //   "  <consigneeAddress>收件人地址</consigneeAddress>" +
            //   "  <consigneeTel>15212792970</consigneeTel>" +
            //   "  <zipCode>230000</zipCode>" +
            //   "  <customsCode>4604</customsCode>" +
            //   "  <worth>20</worth>" +
            //   "  <importDateStr>2016-06-17 16:50:46</importDateStr>" +
            //   "  <currCode>142</currCode>" +
            //   "  <modifyMark>A</modifyMark>" +
            //   "  <businessType>1</businessType>" +
            //   "  <insuredFee>10</insuredFee>" +
            //   "  <freight>10</freight>" +
            //   "  <feature>扩展字段</feature>" +
            //" </tmsWayBillNotifyRequest>";
            #endregion
            #endregion
            string str = SendData.XmlSerialize<tmsWayBillNotifyRequest>(request);
            MD5 md5Hasher = MD5.Create();
            Dictionary<string, string> diy = new Dictionary<string, string>();
            diy.Add("logistics_interface", str);
            diy.Add("clientID", Config.Kk.ClientID);
            diy.Add("data_digest", HttpUtility.UrlEncode(Convert.ToBase64String(md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(Config.Kk.PartnerId + str + Config.Kk.PartnerId))), Encoding.UTF8));
            result.Message = SendData.SendPost(Config.Kk.ReqURL, diy);
            result.Status = true;
            return result;
        } 
        #endregion


    }
}
