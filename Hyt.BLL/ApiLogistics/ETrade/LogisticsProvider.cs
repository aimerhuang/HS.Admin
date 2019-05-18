using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.Model.Transfer;

namespace Hyt.BLL.ApiLogistics.ETrade
{
    /// <summary>
    /// E贸易运物流接口
    /// </summary>
    /// <remarks>2016-4-16 杨云奕 创建</remarks>
    class LogisticsProvider : ILogisticsProvider
    {
        /// <summary>
        /// 发货人名称
        /// </summary>
        public string Shipper { get; set; }
        /// <summary>
        /// 发货地址
        /// </summary>
        public string ShipperAddress { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string ShipperTelephone { get; set; }
        /// <summary>
        /// 发货人所在国（检）
        /// </summary>
        public string ShipperCountryCiq { get; set; }
        /// <summary>
        ///发货人所在国（关）
        /// </summary>
        public string ShipperCountryCus { get; set; }

        /// <summary>
        /// 推送订单的URL连接
        /// </summary>
        public string PushOrderUrl = "http://218.28.185.212:9092/BIService/service/order/pushOrder";
        /// <summary>
        /// 推送订单运单查询
        /// </summary>
        public string LogisticsMateUrl = "http://218.28.185.212:9090/BIService/service/query/logisticsMate";
        /// <summary>
        /// 订单清关状态查询
        /// </summary>
        public string OrderStatusUrl = "http://218.28.185.212:8080/BDIService/ws/retrieveInfo/orderStatus";

        public override Model.CommonEnum.物流代码 Code
        {
            get { return Hyt.Model.CommonEnum.物流代码.E贸易; }
        }

        /// <summary>
        /// Pos
        /// </summary>
        /// <param name="httpUrl"></param>
        /// <param name="dicList"></param>
        /// <returns></returns>
        string SendHttpPOSTRequest(string httpUrl,Dictionary<string, string> dicList)
        {
            string param="";
            foreach (string key in dicList.Keys)
            {
                if (!string.IsNullOrEmpty(param))
                {
                    param += "&";
                }
                param += key + "=" + dicList[key];
            }
            return Hyt.Util.WebUtil.PostForm(httpUrl, param);
        }
        /**
	 * 生成请求报文数据
	 * 
	 * @param encryptDataInfo
	 * @return
	 */
        public String GenerateXml(Dictionary<string, string> companyInfo,
                String encryptDataInfo, String signature)
        {
            StringBuilder xmlSb = new StringBuilder();
            xmlSb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
            xmlSb.Append("<Root>\n");
            xmlSb.Append("	<PubInfo>\n");
            xmlSb.Append("			<Version>1.0</Version>\n");
            xmlSb.Append("			<CompanyCode>").Append(companyInfo["companyCode"])
                    .Append("</CompanyCode>\n");
            xmlSb.Append("			<Key>").Append(companyInfo["companyKey"])
                    .Append("</Key>\n");
            xmlSb.Append("			<MsgType>O</MsgType>\n");
            xmlSb.Append("			<OptType>1</OptType>\n");
            xmlSb.Append("			<Signature>").Append(signature)
                    .Append("</Signature>\n");
            xmlSb.Append("			<CreatTime>2015-04-21 12:12:56</CreatTime>\n");
            xmlSb.Append("	</PubInfo>\n");
            xmlSb.Append("	<DataInfo>").Append(encryptDataInfo)
                    .Append("</DataInfo>\n");
            xmlSb.Append("</Root>\n");
            return xmlSb.ToString();
        }
        /// <summary>
        /// 生成订单的xml信息
        /// </summary>
        /// <param name="companyInfo">公司的信息集合</param>
        /// <param name="order">订单实体</param>
        /// <returns>内容信息</returns>
        public String GetOrderXmlStr(Dictionary<string, string> companyInfo, Model.SoOrder order)
        {
            ///商品详情信息
            IList<Hyt.Model.SoOrderItem> items = Hyt.BLL.Order.SoOrderBo.Instance.GetOrderItemsByOrderId(order.SysNo);
            IList<int> proIdList = new List<int>();
            foreach (Hyt.Model.SoOrderItem mod in items)
            {
                proIdList.Add(mod.SysNo);
            }
            IList<Hyt.Model.CBPdProduct> proList = Hyt.BLL.Product.PdProductBo.Instance.GetProductInfoList(proIdList);
            ///国家列表
            List<Hyt.Model.Origin> originList = Hyt.BLL.Basic.OriginBo.Instance.GetOrigin();
            ///地址信息，省 市 区/县
            Hyt.Model.BsArea cityArea;
            Hyt.Model.BsArea area;
            Hyt.Model.BsArea provinceArea = Hyt.BLL.Basic.BasicAreaBo.Instance.GetProvinceEntity(order.ReceiveAddress.AreaSysNo, out cityArea, out area);
            ///付款方式
            Hyt.Model.BsPaymentType payType = Hyt.BLL.Basic.PaymentTypeBo.Instance.GetEntity(order.PayTypeSysNo);
            ///在线付款商品编号
            Hyt.Model.FnOnlinePayment onlinePayment = Hyt.BLL.Finance.FinanceBo.Instance.GetOnlinePaymentBySourceSysNo(Hyt.Model.WorkflowStatus.FinanceStatus.网上支付单据来源.销售单, order.SysNo);
            ///获取商品的物流信息数据
            Hyt.Model.LgDeliveryType deliveryType = Hyt.BLL.Logistics.DeliveryTypeBo.Instance.GetDeliveryType(order.DeliveryTypeSysNo);
            ///物流编号数据
            Hyt.Model.CrossBorderLogisticsOrder logisticsOrderMod = Hyt.BLL.CrossBorderLogistics.CrossBorderLogisticsOrderBo.Instance.GetEntityByOrderSysNo(order.SysNo);
            ///报文数据
            StringBuilder orderXmlSb = new StringBuilder();
            orderXmlSb.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n");
            orderXmlSb.Append("<Order>\n");
            orderXmlSb.Append(" <OrderHead>\n");
            orderXmlSb.Append("     <ecCode>" + companyInfo["ecCode"] + "</ecCode>\n");
            orderXmlSb.Append("     <cbeCode>" + companyInfo["cbeCode"] + "</cbeCode>\n");
            orderXmlSb.Append("     <ecName>" + companyInfo["ecName"] + "</ecName>\n");
            orderXmlSb.Append("     <cbeName>" + companyInfo["cbeName"] + "</cbeName>\n");
            orderXmlSb.Append("     <ecpCodeCiq>" + companyInfo["ecpCodeCiq"] + "</ecpCodeCiq>\n");
            orderXmlSb.Append("     <ecpCodeCus>" + companyInfo["ecpCodeCus"] + "</ecpCodeCus>\n");
            orderXmlSb.Append("     <ecpNameCiq>" + companyInfo["ecpNameCiq"] + "</ecpNameCiq>\n");
            orderXmlSb.Append("     <ecpNameCus>" + companyInfo["ecpNameCus"] + "</ecpNameCus>\n");
            orderXmlSb.Append("     <orderNo>" + order.SysNo + "</orderNo>\n");
            orderXmlSb.Append("     <charge>" + order.OrderAmount + "</charge>\n");
            orderXmlSb.Append("     <goodsValue>" + (order.ProductAmount - order.ProductDiscountAmount - order.ProductChangeAmount) + "</goodsValue>\n");
            orderXmlSb.Append("     <freight>" + order.FreightAmount + "</freight>\n");
            orderXmlSb.Append("     <other></other>\n");
            orderXmlSb.Append("     <tax>" + order.TaxFee + "</tax>\n");
            orderXmlSb.Append("     <customer>" + order.Customer.Name + "</customer>\n");
            orderXmlSb.Append("     <shipper>" + Shipper + "</shipper>\n");
            orderXmlSb.Append("     <shipperAddress>" + ShipperAddress + "</shipperAddress>\n");
            orderXmlSb.Append("     <shipperTelephone>" + ShipperTelephone + "</shipperTelephone>\n");
            orderXmlSb.Append("     <shipperCountryCiq>" + ShipperCountryCiq + "</shipperCountryCiq>\n");
            orderXmlSb.Append("     <shipperCountryCus>" + ShipperCountryCus + "</shipperCountryCus>\n");
            orderXmlSb.Append("     <consignee>" + order.ReceiveAddress.Name + "</consignee>\n");
            orderXmlSb.Append("     <consigneeProvince>" + provinceArea.AreaName + "</consigneeProvince>\n");
            orderXmlSb.Append("     <consigneeCity>" + cityArea.AreaName + "</consigneeCity>\n");
            orderXmlSb.Append("     <consigneeZone>" + area.AreaName + "</consigneeZone>\n");
            orderXmlSb.Append("     <consigneeAddress>" + order.ReceiveAddress.StreetAddress + "</consigneeAddress>\n");
            orderXmlSb.Append("     <consigneeTelephone>" + order.ReceiveAddress.MobilePhoneNumber + "</consigneeTelephone>\n");
            orderXmlSb.Append("     <consigneeCountryCiq>" + ShipperCountryCiq + "</consigneeCountryCiq>\n");
            orderXmlSb.Append("     <consigneeCountryCus>" + ShipperCountryCus + "</consigneeCountryCus>\n");
            orderXmlSb.Append("     <idType>1</idType>\n");
            orderXmlSb.Append("     <idNumber>" + order.ReceiveAddress.IDCardNo + "</idNumber>\n");
            orderXmlSb.Append("     <ieType>I</ieType>\n");
            orderXmlSb.Append("     <stockFlag>2</stockFlag>\n");
            orderXmlSb.Append("     <batchNumbers>LY20150504001</batchNumbers>\n");
            orderXmlSb.Append("     <totalLogisticsNo>" + logisticsOrderMod.ExpressNo + "</totalLogisticsNo>\n");
            orderXmlSb.Append("     <tradeCountryCiq>156</tradeCountryCiq>\n");
            orderXmlSb.Append("     <tradeCountryCus>142</tradeCountryCus>\n");
            orderXmlSb.Append("     <agentCodeCiq></agentCodeCiq>\n");
            orderXmlSb.Append("     <agentCodeCus></agentCodeCus>\n");
            orderXmlSb.Append("     <agentNameCiq></agentNameCiq>\n");
            orderXmlSb.Append("     <agentNameCus></agentNameCus>\n");
            orderXmlSb.Append("     <packageTypeCiq>4M</packageTypeCiq>\n");
            orderXmlSb.Append("     <packageTypeCus>2</packageTypeCus>\n");
            orderXmlSb.Append("     <modifyMark>1</modifyMark>\n");
            orderXmlSb.Append("     <note></note>\n");
            orderXmlSb.Append(" </OrderHead>\n");
            //--商品列表报文
            orderXmlSb.Append(" <OrderList>\n");
            foreach (Hyt.Model.CBPdProduct pro in proList)
            {
                Hyt.Model.SoOrderItem tempItem = items.First(p => p.ProductSysNo == pro.SysNo);

                orderXmlSb.Append("     <itemNoCiq>" + pro.CIQGoodsNo + "</itemNoCiq>\n");
                orderXmlSb.Append("     <itemNoCus>" + pro.CUSGoodsNo + "</itemNoCus>\n");
                orderXmlSb.Append("     <goodsNo>" + pro.ErpCode + "</goodsNo>\n");
                orderXmlSb.Append("     <shelfGoodsName>" + pro.ProductName + "</shelfGoodsName>\n");
                orderXmlSb.Append("     <goodsName>" + pro.EasName + "</goodsName>\n");
                orderXmlSb.Append("     <describe></describe>\n");
                orderXmlSb.Append("     <codeTs></codeTs>\n");
                orderXmlSb.Append("     <ciqCode></ciqCode>\n");
                orderXmlSb.Append("     <goodsModel></goodsModel>\n");
                orderXmlSb.Append("     <taxCode></taxCode>\n");
                orderXmlSb.Append("     <price>" + Convert.ToDecimal((tempItem.SalesAmount + tempItem.ChangeAmount) / tempItem.Quantity).ToString("0.0000") + "</price>\n");
                orderXmlSb.Append("     <currencyCiq>156</currencyCiq>\n");
                orderXmlSb.Append("     <currencyCus>142</currencyCus>\n");
                orderXmlSb.Append("     <quantity>" + tempItem.Quantity + "</quantity>\n");
                orderXmlSb.Append("     <priceTotal>" + (tempItem.SalesAmount + tempItem.ChangeAmount).ToString("0.0000") + "</priceTotal>\n");
                orderXmlSb.Append("     <unitCiq>122</unitCiq>\n");
                orderXmlSb.Append("     <unitCus>122</unitCus>\n");
                orderXmlSb.Append("     <discount></discount>\n");
                orderXmlSb.Append("     <giftsFlag></giftsFlag>\n");
                orderXmlSb.Append("     <originCountryCiq>" + (originList.Find(p => p.SysNo == pro.OriginSysNo).CIQOriginNO) + "</originCountryCiq>\n");
                orderXmlSb.Append("     <originCountryCus>" + (originList.Find(p => p.SysNo == pro.OriginSysNo).CusOriginNO) + "</originCountryCus>\n");
                orderXmlSb.Append("     <usage></usage>\n");
                orderXmlSb.Append("     <wasteMaterials>1</wasteMaterials>\n");
                orderXmlSb.Append("     <wrapTypeCiq></wrapTypeCiq>\n");
                orderXmlSb.Append("     <wrapTypeCus></wrapTypeCus>\n");
                orderXmlSb.Append("     <packNum></packNum>\n");
            }

            orderXmlSb.Append(" </OrderList>\n");
            //--物流报文
            orderXmlSb.Append(" <OrderPaymentLogistics>\n");
            orderXmlSb.Append("     <paymentCode>" + payType.CusPaymentCode + "</paymentCode>\n");
            orderXmlSb.Append("     <paymentName>" + payType.CusPaymentName + "</paymentName>\n");
            orderXmlSb.Append("     <paymentType></paymentType>\n");
            orderXmlSb.Append("     <paymentNo>" + onlinePayment.VoucherNo + "</paymentNo>\n");
            orderXmlSb.Append("     <logisticsCodeCiq>" + deliveryType.CIQLogisticsNo + "</logisticsCodeCiq>\n");
            orderXmlSb.Append("     <logisticsCodeCus>" + deliveryType.CUSLogisticsNo + "</logisticsCodeCus>\n");
            orderXmlSb.Append("     <logisticsNameCiq>" + deliveryType.CIQLogisticsName + "</logisticsNameCiq>	\n");
            orderXmlSb.Append("     <logisticsNameCus>" + deliveryType.CUSLogisticsName + "</logisticsNameCus>\n");
            orderXmlSb.Append("     <totalLogisticsNo>" + logisticsOrderMod.ExpressNo + "</totalLogisticsNo>\n");
            orderXmlSb.Append("     <subLogisticsNo></subLogisticsNo>\n");
            orderXmlSb.Append("     <logisticsNo></logisticsNo>\n");
            orderXmlSb.Append("     <trackNo></trackNo>\n");
            orderXmlSb.Append("     <trackStatus></trackStatus>\n");
            orderXmlSb.Append("     <crossFreight></crossFreight>\n");
            orderXmlSb.Append("     <supportValue></supportValue>\n");
            orderXmlSb.Append("     <weight>906</weight>\n");
            orderXmlSb.Append("     <netWeight></netWeight>\n");
            orderXmlSb.Append("     <quantity>3</quantity>\n");
            orderXmlSb.Append("     <deliveryWay></deliveryWay>\n");
            orderXmlSb.Append("     <transportationWay>4</transportationWay>\n");
            orderXmlSb.Append("     <shipCode>32</shipCode>\n");
            orderXmlSb.Append("     <shipName></shipName>\n");
            orderXmlSb.Append("     <destinationPort></destinationPort>\n");
            orderXmlSb.Append(" </OrderPaymentLogistics>\n");
            orderXmlSb.Append("</Order>\n");
            return orderXmlSb.ToString();
        }
        /// <summary>
        /// 初始化公司信息
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> InitCompanyInfo()
        {
            // 准备企业信息
            Dictionary<string, string> companyInfo = new Dictionary<string, string>();
            // 企业编号,E贸易中注册的编号
            companyInfo.Add("companyCode", "D00999");
            // 企业名称,E贸易中注册的名称
            companyInfo.Add("companyName", "测试电商企业");
            // 国检备案的企业编码
            companyInfo.Add("ecCode", "4100300542");
            // 国检备案的企业名称
            companyInfo.Add("ecName", "广州洋车站贸易有限公司");
            // 海关备案的企业编码
            companyInfo.Add("cbeCode", "D00537");
            // 海关备案的企业名称
            companyInfo.Add("cbeName", "广州洋车站贸易有限公司");
            // 电商平台国检备案编码
            companyInfo.Add("ecpCodeCiq", "4100300542");
            // 电商平台国检备案名称
            companyInfo.Add("ecpNameCiq", "广州洋车站贸易有限公司");
            // 电商平台海关备案编码
            companyInfo.Add("ecpCodeCus", "W0331");
            // 电商平台海关备案名称
            companyInfo.Add("ecpNameCus", "广州洋车站贸易有限公司");
            // 企业公钥串,由E贸易提供
            companyInfo.Add("publicKey",
                            "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQC/EEOfH7PmcnMJda8BKfJ8agU+f/P1Is2cVIb/pBTWEzcvXPCzH0zm0vHJHKMkiuvWQbNpUtM1EFMHSadp4u0JVA7weAJBCw01xH//pR2lYkLv2JO2yeuE4omuH1wJOVFY8befDG7hD+D2GrxFvrxVMSQ7fiwcgDUpFhVWyfpmgQIDAQAB");
            // 企业key，由E贸易提供
            companyInfo.Add("companyKey", "b2e6261fc3257aea96c66692ab1f589c");
            return companyInfo;
        }


        public override Result GetProduct(string productId)
        {
            throw new NotImplementedException();
        }

        public override Result AddOrderTrade(int orderSysno)
        {
            var order = BLL.Order.SoOrderBo.Instance.GetEntity(orderSysno);
            var result = new Model.Result<string>();
            Dictionary<string, string> companyInfo = InitCompanyInfo();
            // 准备订单信息
            String dataInfo = GetOrderXmlStr(companyInfo, order);
            // RSA加密
            String encryptDataInfo = Hyt.Util.EncryptionUtil.RasEncreptData(dataInfo,
                companyInfo["publicKey"]);
            // 32位MD5签名
            String signature = Hyt.Util.EncryptionUtil.GetMd5Encode32(encryptDataInfo);
            // 生成请求报文
            String xml = GenerateXml(companyInfo, encryptDataInfo, signature);
            //System.out.println("======最终报文数据:======\n" + xml);
            Dictionary<string, string> map = new Dictionary<string, string>();
            map.Add("xml", xml);
            // 调用webservice服务
            string PostResult = SendHttpPOSTRequest(
                    PushOrderUrl,
                    map);
            // 查看响应回执

            ///获取xml状态数据信息
            string status = Hyt.Util.Serialization.SerializationUtil.GetStrForXmlDoc(PostResult, "Status");
            string msg = Hyt.Util.Serialization.SerializationUtil.GetStrForXmlDoc(PostResult, "Detail");
            if (status.Trim() == "0")
            {
                result.Status = true;
            }
            else
            {
                result.Status = false;
            }
            result.Data = PostResult;
            result.Message = msg;
            return result;
        }
        /// <summary>
        /// E贸易获取匹配运单号查询
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <returns></returns>
        public override Model.Result GetOrderExpressno(string orderId)
        {
            Model.Result<string> result = new Model.Result<string>();
            string PostResult = Hyt.Util.WebUtil.PostForm(LogisticsMateUrl, "xml=" + LogisticsMateXMLData(orderId));

            ///获取xml状态数据信息
            string status = Hyt.Util.Serialization.SerializationUtil.GetStrForXmlDoc(PostResult, "success");
            string msg = Hyt.Util.Serialization.SerializationUtil.GetStrForXmlDoc(PostResult, "errorMsg");
            if (status.Trim() == "0")
            {
                result.Status = true;
            }
            else
            {
                result.Status = false;
            }

            // 查看响应回执
            result.Data = PostResult;
            result.Message = msg;
            return result;
        }
        /// <summary>
        /// 获取物流清关
        /// </summary>
        /// <param name="orderSysNos"></param>
        /// <returns></returns>
        public string LogisticsMateXMLData(string orderSysNos)
        {
            Dictionary<string, string> companyInfo = InitCompanyInfo();
            string[] SysNos = orderSysNos.Split(',');
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sb.AppendLine("<root>");
            sb.AppendLine(" <pub>");
            sb.AppendLine("     <companyCode>" + companyInfo["companyCode"] + "</companyCode>");
            sb.AppendLine("     <companyKey>企业KEY（ETOUCH提供）</companyKey>");
            sb.AppendLine("     <requestTime>"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"</requestTime>");
            sb.AppendLine(" </pub>");
            sb.AppendLine(" <orders>");
            foreach (string sysNo in SysNos) 
            {
                sb.AppendLine("     <order>");
                sb.AppendLine("         <ecpCode>" + companyInfo["ecpCodeCus"] + "</ecpCode>");
                sb.AppendLine("         <orderNo>" + sysNo + "</orderNo>");
                sb.AppendLine("     </order>");
            }
            sb.AppendLine(" </orders>");
            sb.AppendLine("</root>");
            return sb.ToString();
        }
        /// <summary>
        /// E贸易获取匹配清关状态查询
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <returns></returns>
        public override Model.Result GetOrderTrade(string orderId)
        {
            Model.Result<string> result = new Model.Result<string>();
            string PostResult = Hyt.Util.WebUtil.PostForm(OrderStatusUrl, "xml=" + OrderStatusXMLData(orderId));
            ///获取xml状态数据信息
            string status = Hyt.Util.Serialization.SerializationUtil.GetStrForXmlDoc(PostResult, "status");
            string msg = Hyt.Util.Serialization.SerializationUtil.GetStrForXmlDoc(PostResult, "message");
            if (status.Trim() == "0")
            {
                result.Status = true;
            }
            else
            {
                result.Status = false;
            }
            // 查看响应回执
            result.Data=PostResult;
            result.Message = msg;
            return result;
        }
        /// <summary>
        /// 清关状态
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        private string OrderStatusXMLData(string orderSysNos)
        {
            Dictionary<string, string> companyInfo = InitCompanyInfo();
            string[] SysNos = orderSysNos.Split(',');
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sb.AppendLine("<root>");
            sb.AppendLine(" <pub>");
            sb.AppendLine("     <cbeCode>" + companyInfo["ecpCodeCus"] + "</cbeCode>");
            sb.AppendLine("     <requestTime>" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "</requestTime>");
            sb.AppendLine(" </pub>");
            sb.AppendLine(" <orders>");
            foreach (string sysNo in SysNos)
            {
                sb.AppendLine("     <order>");
                sb.AppendLine("         <orderNo>" + sysNo + "</orderNo>");
                sb.AppendLine("     </order>");
            }
            sb.AppendLine(" </orders>");
            sb.AppendLine("</root>");
            return sb.ToString();
        }
        
        public override Model.Result<string> GetLogisticsTracking(int orderSysNo)
        {
            throw new NotImplementedException();
        }

        public override Result CancelOrderTrade(int orderSysNo, string reason = "")
        {
            throw new NotImplementedException();
        }
    }
}
