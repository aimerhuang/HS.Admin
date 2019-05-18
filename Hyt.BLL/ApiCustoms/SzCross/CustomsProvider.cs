using Hyt.BLL.ApiPay;
using Hyt.Model;
using Hyt.Model.Order;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Util;
using Hyt.Util.Net;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Hyt.Util.Xml;
using Hyt.Model.Common;

namespace Hyt.BLL.ApiCustoms.SzCross
{
    /// <summary>
    /// 深圳跨境贸易电子商务通关服务平台
    /// </summary>
    /// <remarks>2017-06-13 杨浩 创建</remarks>
    public class CustomsProvider : ICustomsProvider
    {


        Dictionary<string, string> SySpecList = new Dictionary<string, string>();
        Dictionary<string, string> dicUnit ;
        protected static Customs3Config config3 = BLL.Config.Config.Instance.GetCustoms3Config();
        public override CommonEnum.海关 Code
        {
            get { return CommonEnum.海关.深圳跨境贸易电子商务通关服务平台; }
        }

        public CustomsProvider()
        {
            //SySpecList.Add("kg", "千克");
            //SySpecList.Add("g", "克");
            //SySpecList.Add("lb", "镑");
            //SySpecList.Add("oz", "盎司");
            //SySpecList.Add("case", "盒");
            //SySpecList.Add("bottle", "瓶");
            //SySpecList.Add("pot", "罐");


            //SySpecList.Add("count", "支");
            //SySpecList.Add("piece", "块");
            //SySpecList.Add("pcs", "PCS");
            //SySpecList.Add("other", "其他");

            //dicUnit = new Dictionary<string, string>();
            //dicUnit.Add("千克", "035");
            //dicUnit.Add("克", "036");
            //dicUnit.Add("镑", "076");
            //dicUnit.Add("盎司", "083");
            //dicUnit.Add("盒", "140");
            //dicUnit.Add("瓶", "142");
            //dicUnit.Add("罐", "122");
            //dicUnit.Add("支", "012");
            //dicUnit.Add("块", "017");
            //dicUnit.Add("其他", "999");
        }

        #region 海关FTP
        private string EbcCode = config3.EbcCode;// "4430160BTW";
        private string FTPName = config3.EbcFTPName;// "szxinying";
        private string FTPPassword = config3.EbcFtpPassword;// "CLwfxloxmU";
        /// <summary>
        /// 海关注册登记企业名称
        /// </summary>
        private string EbcName = config3.EbcName;// "深圳信营贸易有限公司";
        private string FTPUri = config3.EbcFtpUrl; // "ftp://59.42.252.24:21/";

        /// <summary>
        /// FTP上传文件名称
        /// </summary>
        /// <remarks>2016-8-19 杨浩 添加</remarks>
        private string FolderIn
        {
            get
            {
                //没有申请Dxp的文件夹路径为cein/
                if (string.IsNullOrEmpty(config.DxpId))
                    return "cebin/";

                return "in/";
            }
        }
        /// <summary>
        /// FTP回执文件夹
        /// </summary>
        /// <remarks>2016-8-19 杨浩 添加</remarks>
        private string FolderOut
        {
            get
            {
                //没有申请Dxp的文件夹路径为ceout/
                if (string.IsNullOrEmpty(config.DxpId))
                    return "cebout/";

                return "out/";
            }
        }
        /// <summary>
        /// Dxp编码
        /// </summary>
        /// <remarks>2016-8-19 杨浩 添加</remarks>
        private string DxpId
        {
            get
            {
                //没有申请Dxp则用共用的
                if (string.IsNullOrEmpty(config.DxpId))
                    return "DXPENTTEST510001";//

                return config.DxpId;
            }
        }
        #endregion
        /// <summary>
        /// 修改订单
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <param name="warehouseSysNo">仓库编号</param>
        /// <returns></returns>
        /// <remarks>2016-8-24 杨浩 添加</remarks>
        public override Result ModifyOrder(int orderSysNo,int warehouseSysNo) 
        {
            var result = new Result()
            {
                Status = false
            };

            try
            {
                var customsLogInfo = BLL.Order.SoCustomsOrderLogBo.Instance.GetCustomsOrderLogInfo(orderSysNo, (int)Code);
                result = Upload(orderSysNo, warehouseSysNo, customsLogInfo, "2");
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message =ex.Message;
            }

            return result;
        }
        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="warehouseSysNo">仓库编号</param>
        /// <returns></returns>
        /// <remarks>2016-8-24 杨浩 添加</remarks>
        public override Result CancelOrder(int orderId, int warehouseSysNo)
        {
            var result = new Result()
            {
                Status = false
            };

            try
            {
                var customsLogInfo = BLL.Order.SoCustomsOrderLogBo.Instance.GetCustomsOrderLogInfo(orderId, (int)Code);
                result = Upload(orderId, warehouseSysNo, customsLogInfo, "3");
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message =ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 设置支付企业代码和名称
        /// </summary>
        /// <param name="payTypeSysNo">支付系统编号</param>
        /// <param name="gAOrder">海关总署订单报文</param>
        /// <remarks>2016-12-8 杨浩 创建</remarks>
        //private GAOrder SetPayCodeAndPayName(int payTypeSysNo, GAOrder gAOrder)
        //{
        //    switch (payTypeSysNo)
        //    {
        //        case 16://钱袋宝
        //            gAOrder.OrderHead.payCode = "110896T001";
        //            gAOrder.OrderHead.payName = "北京钱袋宝支付技术有限公司";
        //            break;                
        //    }
        //    return gAOrder;
        //}



        /// <summary>
        /// 上传订单至海关
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="warehouseSysNo">仓库编号</param>
        /// <param name="customsLogInfo">海关日志</param>
        /// <param name="appType">报文类型 企业报送类型。A-新增 M-变更 D-删除。默认为1。</param>
        /// <returns></returns>
        /// <remarks>2016-6-4 杨浩 创建</remarks>
        private Result Upload(int orderId, int warehouseSysNo, SoCustomsOrderLog customsLogInfo, string appType = "A")
        {
            var result = new Result()
            {
                Status = false
            };

            try
            {
                var order = BLL.Order.SoOrderBo.Instance.GetEntity(orderId);
                if (order == null)
                {
                    result.Message = "该订单不存在";
                    return result;
                }
                order.ReceiveAddress = BLL.Order.SoOrderBo.Instance.GetOrderReceiveAddress(order.ReceiveAddressSysNo);
                order.OrderItemList = DataAccess.Order.ISoOrderItemDao.Instance.GetOrderItemsByOrderSysNo(order.SysNo);
                order.Customer =Hyt.BLL.CRM.CrCustomerBo.Instance.GetModel(order.CustomerSysNo);

                ///获取商品档案数据
                IList<int> proSysNos = new List<int>();
                foreach(var mod in order.OrderItemList)
                {
                    proSysNos.Add(mod.ProductSysNo);
                }
                IList<CBPdProduct> productList = Hyt.BLL.Product.PdProductBo.Instance.GetProductInfoList(proSysNos);

                IList<PdProductStock> stockList = Hyt.BLL.Warehouse.PdProductStockBo.Instance.GetAllStockList(order.DefaultWarehouseSysNo, proSysNos);

                List<Origin> orginList = Hyt.BLL.Basic.OriginBo.Instance.GetOrigin();
                

                var onlinePaymentFilter = new Hyt.Model.Parameter.ParaOnlinePaymentFilter()
                {
                    OrderSysNo = orderId,
                    Id = 1
                };
                var onlinePayments = Hyt.BLL.Finance.FinanceBo.Instance.GetOnlinePayments(onlinePaymentFilter).Rows;

                if (onlinePayments == null || (onlinePayments != null && onlinePayments.Count() <= 0))
                {
                    result.Status = false;
                    result.Message = "订单没有支付单！";
                    return result;
                }

                string orderNo = onlinePayments[0].BusinessOrderSysNo;
                if (string.IsNullOrEmpty(orderNo))
                {
                    result.Status = false;
                    result.Message = "报关订单编号不能为空，请检查支付单字段 BusinessOrderSysNo";
                    return result;
                }

                // 收货人 区 市 省
                var receiverDistrict = BLL.Basic.BasicAreaBo.Instance.GetArea(order.ReceiveAddress.AreaSysNo);
                var receiverCity = BLL.Basic.BasicAreaBo.Instance.GetArea(receiverDistrict.ParentSysNo);
                var receiverProvince = BLL.Basic.BasicAreaBo.Instance.GetArea(receiverCity.ParentSysNo);
                // 发货人 市
                var warehouse = BLL.Warehouse.WhWarehouseBo.Instance.GetWarehouse(warehouseSysNo);
                var shipperCity = BLL.Basic.BasicAreaBo.Instance.GetArea(warehouse.CitySysNo);

                var dealer = BLL.Distribution.DsDealerBo.Instance.GetDsDealer(order.DealerSysNo);

                var deliveryType = BLL.Logistics.DeliveryTypeBo.Instance.GetDeliveryType(order.DeliveryTypeSysNo);



              
              

                result.Status = true;
                result.Message = "提交成功！";
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
            }
            return result;
        }
        /// <summary>
        /// 推送订单
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="warehouseSysNo">仓库编号</param>
        /// <returns></returns>
        public override Result PushOrder(int orderId, int warehouseSysNo)
        {
            var result = new Result()
            {
                Status = false
            };

            try
            {
                var customsLogInfo = BLL.Order.SoCustomsOrderLogBo.Instance.GetCustomsOrderLogInfo(orderId, (int)Code);
                result = Upload(orderId, warehouseSysNo, customsLogInfo);
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = "向" + this.Code + "物流推送订单报错：" + ex.StackTrace;
                return result;
            }

            return result;
        }

        private string XmlSerialize<T>(T item, XmlSerializerNamespaces xmlNs)
        {
            var serializer = new XmlSerializer(typeof(T));
            var stringBuilder = new StringBuilder();
            using (var writer = new StringWriter(stringBuilder))
            {
                serializer.Serialize(writer, item, xmlNs);
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 查询海关订单
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <returns></returns>
        /// <remarks>2016-8-18 杨浩 重构</remarks>
        public override Result SearchCustomsOrder(int orderId)
        {
            var result = new Result()
            {
                Status = true
            };

            try
            {
                var resultDic = new Dictionary<int, string>();

                var ftp = new FtpUtil(FTPUri, FTPName, FTPPassword);
                string ftpResponse = "";


                #region 
                /*
                 <?xml version="1.0" encoding="UTF-8"?>
<DocRec>
	<Head>
		<MessageID><![CDATA[KJDOCREC_KJGGPT2016110315520669308]]></MessageID>
		<MessageType><![CDATA[KJDOCREC]]></MessageType>
		<Sender><![CDATA[KJGGPT]]></Sender>
		<Receiver><![CDATA[GZXINRAO]]></Receiver>
		<SendTime><![CDATA[20161103155206]]></SendTime>
		<FunctionCode><![CDATA[]]></FunctionCode>
		<SignerInfo><![CDATA[]]></SignerInfo>
		<Version><![CDATA[2.0]]></Version>
	</Head>
	<Declaration>
		<OrgMessageID><![CDATA[F9F53112-3A28-4912-92EC-BD3085085FC8]]></OrgMessageID>
		<OrgMessageType><![CDATA[CEB311Message]]></OrgMessageType>
		<OrgSenderID><![CDATA[GZXINRAO]]></OrgSenderID>
		<OrgReceiverID><![CDATA[KJCUSTOM]]></OrgReceiverID>
		<OrgRecTime><![CDATA[20161103155206]]></OrgRecTime>
		<RespondBy>01</RespondBy>
		<RespondStatus><![CDATA[F]]></RespondStatus>
		<RespondNotes><![CDATA[1.4.2.业务验证失败：
1.广州海关单号【501_20161029101958】重复申报...
]]></RespondNotes>			
		<Status>F</Status>
		<Notes><![CDATA[1.4.2.业务验证失败：
1.广州海关单号【501_20161029101958】重复申报...
]]></Notes>
	</Declaration>
</DocRec>
                 * */

                #endregion
                // 获取文件列表
                //string[] fileList = ftp.GetFileList(FTPUri + "cebout/");
                string[] fileList = ftp.GetFileList(FTPUri + FolderOut);

                if (fileList != null && fileList.Length > 0)
                {       
                    foreach (string fileName in fileList)
                    {
                        //Stream stream = ftp.FileStream(FTPUri + "cebout/" + fileName, ref ftpResponse);
                        var stream = ftp.FileStream(FTPUri + FolderOut + fileName, ref ftpResponse);
                        #region 更新订单海关推送状态

                        string xmlData = "";
                        stream.Seek(0, SeekOrigin.Begin);
                        using (var reader = new StreamReader(stream))
                        {
                            xmlData = reader.ReadToEnd();                         
                        }

                        if (!string.IsNullOrEmpty(xmlData))
                        {
                            int id = 0;
                            var customsOrderLog = new SoCustomsOrderLog();

                            var xml = new XmlDocumentExtender();
                            xml.LoadXml(xmlData);
                            var _xml = xml["CEB312Message"];
                     
                            if (_xml != null)
                            {
                                var orderReturn = _xml["OrderReturn"];

                                //订单编号
                                if (!int.TryParse(orderReturn["orderNo"].InnerText.Split('_')[0], out id))                              
                                    continue;

                                var customsLogInfo = BLL.Order.SoCustomsOrderLogBo.Instance.GetCustomsOrderLogInfo(id, (int)Code);

                                int status = -999;

                                switch (orderReturn["returnStatus"].InnerText)
                                {
                                    case "":// 电子口岸申报中
                                        result.Message = "电子口岸申报中！";
                                        status = (int)OrderStatus.海关报关状态.处理中;
                                        break;
                                    case "2":// 发送海关成功
                                        status = (int)OrderStatus.海关报关状态.处理中;
                                        result.Message = orderReturn["returnInfo"].InnerText;
                                        if (customsLogInfo.StatusCode == "3")
                                        {
                                            status = (int)OrderStatus.海关报关状态.作废;
                                            result.Message = orderReturn["returnInfo"].InnerText;
                                        }                                       
                                        break;
                                    case "4":// 发送海关失败
                                    case "100":// 海关退单
                                        status = (int)OrderStatus.海关报关状态.失败;
                                        result.Message = "海关退单！";
                                        break;
                                    case "120":// 海关入库
                                        status = (int)OrderStatus.海关报关状态.成功;
                                        result.Message = "海关入库成功！";
                                        break;
                                    default:
                                        status = (int)OrderStatus.海关报关状态.失败;
                                        result.Message = "海关推送失败！";
                                        break;
                                }
                                customsOrderLog.StatusCode = "";
                                //customsOrderLog.StatusCode = orderReturn["returnStatus"].InnerText;
                                customsOrderLog.StatusMsg = orderReturn["returnInfo"].InnerText;

                                if(resultDic.ContainsKey(orderId))
                                    resultDic[orderId] = customsOrderLog.StatusMsg;
                                else
                                    resultDic.Add(orderId, customsOrderLog.StatusMsg);


                                BLL.Order.SoOrderBo.Instance.UpdateOrderApiStatus((int)status, 2, id);

                                
                                if (customsLogInfo != null)
                                {
                                    customsOrderLog.LastUpdateBy = 0;
                                    if (BLL.Authentication.AdminAuthenticationBo.Instance.IsLogin)
                                        customsOrderLog.LastUpdateBy = BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo;

                                    customsOrderLog.LastUpdateDate = DateTime.Now;
                                    customsOrderLog.ReceiptContent = xmlData;
                                    customsOrderLog.SysNo = customsLogInfo.SysNo;
                                    customsOrderLog.FileName = customsLogInfo.FileName;
                                    customsOrderLog.Packets = customsLogInfo.Packets;
                                    BLL.Order.SoCustomsOrderLogBo.Instance.UpdateCustomsOrderLog(customsOrderLog);
                                }
                              
                            }
                            else
                            {
                                _xml = xml["DocRec"];
                                if (_xml != null)
                                {
                                    var status = _xml["Declaration"]["Status"].InnerText;                                                                 
                                }
                            }
                        }

                        ftp.DeleteFile(FTPUri + FolderOut + fileName);//删除回执文件
                        #endregion                    
                    }


                    if (resultDic.ContainsKey(orderId))
                        result.Message = resultDic[orderId];
                    else
                        result.Message ="暂无回执";
                  
                }
                else
                {
                    var customsLogInfo = BLL.Order.SoCustomsOrderLogBo.Instance.GetCustomsOrderLogInfo(orderId,(int)Code);
                    result.Status = false;
                    result.Message =customsLogInfo!=null?customsLogInfo.StatusMsg:"没有找到回执文件，请稍后重试！";
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
            }
            return result;
        }

        #region 实体类

        /// <summary>
        /// CEB311Message电子订单
        /// </summary>
        /// 
        [Serializable]
        public class InternationalTrade
        {
            [XmlElement(ElementName = "Head")]
            public Head head { get; set; }

            [XmlElement(ElementName = "Declaration")]
            public Declaration declaration { get; set; }

            
        }


        /// <summary>
        /// 电子订单表头
        /// </summary>
        /// 
        [Serializable]
        public class Head
        {
            [XmlElement(ElementName = "MessageID")]
            public string MessageID { get; set; }
            [XmlElement(ElementName = "MessageType")]
            public string MessageType { get; set; }
            [XmlElement(ElementName = "Sender")]
            public string Sender { get; set; }
             [XmlElement(ElementName = "Receiver")]
            public string Receiver { get; set; }
             [XmlElement(ElementName = "SendTime")]
            public string SendTime { get; set; }
             [XmlElement(ElementName = "FunctionCode")]
            public string FunctionCode { get; set; }
             [XmlElement(ElementName = "SignerInfo")]
            public string SignerInfo { get; set; }
             [XmlElement(ElementName = "Version")]
            public string Version { get; set; }

        }
        [Serializable]
        public class Declaration
        {
           [XmlElement(ElementName = "OrderHead")]
            public OrderHead orderHead { get; set; }
            [XmlElement(ElementName = "OrderList")]
            public OrderList orderList { get; set; }

        }
        [Serializable]
        public class OrderHead{
              [XmlElement(ElementName = "DeclEntNo")]
            public string DeclEntNo { get; set; }
             [XmlElement(ElementName = "DeclEntName")]
            public string DeclEntName { get; set; }
             [XmlElement(ElementName = "EBEntNo")]
            public string EBEntNo { get; set; }
             [XmlElement(ElementName = "EBEntName")]
            public string EBEntName { get; set; }
             [XmlElement(ElementName = "EBPEntNo")]
            public string EBPEntNo { get; set; }
             [XmlElement(ElementName = "EBPEntName")]
            public string EBPEntName { get; set; }
             [XmlElement(ElementName = "InternetDomainName")]
            public string InternetDomainName { get; set; }
             [XmlElement(ElementName = "DeclTime")]
            public string DeclTime { get; set; }
             [XmlElement(ElementName = "OpType")]
            public string OpType { get; set; }
             [XmlElement(ElementName = "IeFlag")]
            public string IeFlag { get; set; }
             [XmlElement(ElementName = "CustomsCode")]
            public string CustomsCode { get; set; }
             [XmlElement(ElementName = "CIQOrgCode")]
            public string CIQOrgCode { get; set; }
        }
        [Serializable]
        public class OrderList
        {
             [XmlElement(ElementName = "OrderContent")]
            public OrderContent orderContent { get; set; }
        }
        [Serializable]
        public class OrderContent
        {
               [XmlElement(ElementName = "OrderDetail")]
            public OrderDetail orderDetail { get; set; }
              [XmlElement(ElementName = "OrderWaybillRel")]
            public OrderWaybillRel orderWaybillRel { get; set; }
              [XmlElement(ElementName = "OrderPaymentRel")]
            public OrderPaymentRel orderPaymentRel { get; set; }

        }
        [Serializable]
        public class OrderPaymentRel
        {
              [XmlElement(ElementName = "PayEntNo")]
            public string PayEntNo { get; set; }
              [XmlElement(ElementName = "PayEntName")]
            public string PayEntName { get; set; }
              [XmlElement(ElementName = "PayNo")]
            public string PayNo { get; set; }
              [XmlElement(ElementName = "Notes")]
            public string Notes { get; set; }
        }
        [Serializable]
        public class OrderWaybillRel
        {
             [XmlElement(ElementName = "EHSEntNo")]
            public string EHSEntNo { get; set; }
             [XmlElement(ElementName = "EHSEntName")]
            public string EHSEntName { get; set; }
             [XmlElement(ElementName = "WaybillNo")]
            public string WaybillNo { get; set; }
             [XmlElement(ElementName = "Notes")]
            public string Notes { get;set;}
        }
        [Serializable]
        public class OrderDetail
        {
             [XmlElement(ElementName = "EntOrderNo")]
            public string EntOrderNo { get; set; }
             [XmlElement(ElementName = "OrderStatus")]
            public int OrderStatus { get; set; }
             [XmlElement(ElementName = "PayStatus")]
            public int PayStatus { get; set; }
             [XmlElement(ElementName = "OrderGoodTotal")]
            public decimal OrderGoodTotal { get; set; }
             [XmlElement(ElementName = "OrderGoodTotalCurr")]
            public decimal OrderGoodTotalCurr { get; set; }
             [XmlElement(ElementName = "Freight")]
            public decimal Freight { get; set; }
             [XmlElement(ElementName = "Tax")]
            public decimal Tax { get; set; }
             [XmlElement(ElementName = "OtherPayment")]
            public decimal OtherPayment { get; set; }
             [XmlElement(ElementName = "OtherPayNotes")]
            public string OtherPayNotes { get; set; }
             [XmlElement(ElementName = "OtherCharges")]
            public decimal OtherCharges { get; set; }
             [XmlElement(ElementName = "ActualAmountPaid")]
            public decimal ActualAmountPaid { get; set; }
             [XmlElement(ElementName = "RecipientName")]
            public string RecipientName { get; set; }
             [XmlElement(ElementName = "RecipientAddr")]
            public string RecipientAddr { get; set; }
             [XmlElement(ElementName = "RecipientTel")]
            public string RecipientTel { get; set; }
             [XmlElement(ElementName = "RecipientCountry")]
            public int RecipientCountry { get; set; }
             [XmlElement(ElementName = "RecipientProvincesCode")]
            public string RecipientProvincesCode { get; set; }
             [XmlElement(ElementName = "OrderDocAcount")]
            public string OrderDocAcount { get; set; }
             [XmlElement(ElementName = "OrderDocName")]
            public string OrderDocName { get; set; }
             [XmlElement(ElementName = "OrderDocType")]
            public string OrderDocType { get; set; }
             [XmlElement(ElementName = "OrderDocId")]
            public string OrderDocId { get; set; }
             [XmlElement(ElementName = "OrderDocTel")]
            public string OrderDocTel { get; set; }
             [XmlElement(ElementName = "OrderDate")]
            public string OrderDate { get; set; }
             [XmlElement(ElementName = "BatchNumbers")]
            public string BatchNumbers { get; set; }
             [XmlElement(ElementName = "InvoiceType")]
            public string InvoiceType { get; set; }
             [XmlElement(ElementName = "InvoiceNo")]
            public string InvoiceNo { get; set; }
             [XmlElement(ElementName = "InvoiceTitle")]
            public string InvoiceTitle { get; set; }
             [XmlElement(ElementName = "InvoiceIdentifyID")]
            public string InvoiceIdentifyID { get; set; }
             [XmlElement(ElementName = "InvoiceDesc")]
            public string InvoiceDesc { get; set; }
             [XmlElement(ElementName = "InvoiceAmount")]
            public decimal InvoiceAmount { get; set; }
             [XmlElement(ElementName = "InvoiceDate")]
            public string InvoiceDate { get; set; }
             [XmlElement(ElementName = "Notes")]
            public string Notes { get; set; }
             [XmlElement(ElementName = "GoodsList")]
            public GoodsList goodsList { get; set; }

        }
        [Serializable]
        public class GoodsList
        {
             [XmlElement(ElementName = "OrderGoodsList")]
            public List<OrderGoodsList> orderGoodsList { get; set; }
        }
        [Serializable]
        public class OrderGoodsList
        {
             [XmlElement(ElementName = "Seq")]
            public int Seq { get; set; }
             [XmlElement(ElementName = "EntGoodsNo")]
            public string EntGoodsNo { get; set; }
             [XmlElement(ElementName = "CIQGoodsNo")]
            public string CIQGoodsNo { get; set; }
             [XmlElement(ElementName = "CusGoodsNo")]
            public string CusGoodsNo { get; set; }
             [XmlElement(ElementName = "HSCode")]
            public string HSCode { get; set; }
             [XmlElement(ElementName = "GoodsName")]
            public string GoodsName { get; set; }
             [XmlElement(ElementName = "GoodsStyle")]
            public string GoodsStyle { get; set; }
             [XmlElement(ElementName = "GoodsDescrib")]
            public string GoodsDescrib { get; set; }
             [XmlElement(ElementName = "OriginCountry")]
            public string OriginCountry { get; set; }
             [XmlElement(ElementName = "BarCode")]
            public string BarCode { get; set; }
             [XmlElement(ElementName = "Brand")]
            public string Brand { get; set; }
             [XmlElement(ElementName = "Qty")]
            public int Qty { get; set; }
             [XmlElement(ElementName = "Unit")]
            public string Unit { get; set; }
             [XmlElement(ElementName = "Price")]
            public decimal Price { get; set; }
             [XmlElement(ElementName = "Total")]
            public decimal Total { get; set; }
             [XmlElement(ElementName = "CurrCode")]
            public int CurrCode { get; set; }
             [XmlElement(ElementName = "Notes")]
            public string Notes { get; set; }
             [XmlElement(ElementName = "GoodsDescribe")]

            public string GoodsDescribe { get; set; }
        }



        #endregion
    }
}