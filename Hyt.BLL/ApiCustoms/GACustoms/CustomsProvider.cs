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

namespace Hyt.BLL.ApiCustoms.GACustoms
{
    /// <summary>
    /// 海关总署接口
    /// </summary>
    public class CustomsProvider : ICustomsProvider
    {
       
        public override CommonEnum.海关 Code
        {
            get { return CommonEnum.海关.海关总署; }
        }



        #region 海关FTP
        private string EbcCode = config.EbcCode;// "4430160BTW";
        private string FTPName = config.EbcFTPName;// "szxinying";
        private string FTPPassword = config.EbcFtpPassword;// "CLwfxloxmU";
        /// <summary>
        /// 海关注册登记企业名称
        /// </summary>
        private string EbcName = config.EbcName;// "深圳信营贸易有限公司";
        private string FTPUri = config.EbcFtpUrl; // "ftp://59.42.252.24:21/";

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
        private GAOrder SetPayCodeAndPayName(int payTypeSysNo, GAOrder gAOrder)
        {
            switch (payTypeSysNo)
            {
                case 16://钱袋宝
                    gAOrder.OrderHead.payCode = "110896T001";
                    gAOrder.OrderHead.payName = "北京钱袋宝支付技术有限公司";
                    break;                
            }
            return gAOrder;
        }



        /// <summary>
        /// 上传订单至海关
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="warehouseSysNo">仓库编号</param>
        /// <param name="customsLogInfo">海关日志</param>
        /// <param name="appType">报文类型 企业报送类型。1-新增 2-变更 3-删除。默认为1。</param>
        /// <returns></returns>
        /// <remarks>2016-6-4 杨浩 创建</remarks>
        private Result Upload(int orderId, int warehouseSysNo, SoCustomsOrderLog customsLogInfo, string appType = "1")
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

                var CEBMessage = new CEB311Message();
                CEBMessage.GAOrder = new List<GAOrder>();
                CEBMessage.guid = System.Guid.NewGuid().ToString("D").ToUpper();
                CEBMessage.version = "1.0";

                var gaOrder = new GAOrder();
                gaOrder.OrderHead = new OrderHead();
                gaOrder.OrderHead.acturalPaid = order.CashPay.ToString();// (order.ProductAmount + order.ProductChangeAmount).ToString(); //recVoucher.ReceivedAmount.ToString();
                gaOrder.OrderHead.appStatus = "2";
                gaOrder.OrderHead.appTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                gaOrder.OrderHead.appType = appType;// "1";
                gaOrder.OrderHead.batchNumbers = "";
                gaOrder.OrderHead.buyerIdNumber = order.ReceiveAddress.IDCardNo;
                gaOrder.OrderHead.buyerIdType = "1";
                gaOrder.OrderHead.buyerName = order.ReceiveAddress.Name;
                gaOrder.OrderHead.buyerRegNo = order.CustomerSysNo.ToString();
                gaOrder.OrderHead.consignee = order.ReceiveAddress.Name;
                gaOrder.OrderHead.consigneeAddress = order.ReceiveAddress.StreetAddress;
                //gaOrder.OrderHead.consigneeDitrict = receiverDistrict.AreaCode;
                gaOrder.OrderHead.consigneeTelephone = order.ReceiveAddress.MobilePhoneNumber;
                gaOrder.OrderHead.currency = "142";
                gaOrder.OrderHead.discount = order.OrderDiscountAmount.ToString();
                gaOrder.OrderHead.ebcCode = EbcCode; //"44306609EP";
                gaOrder.OrderHead.ebcName = EbcCode; //"广州华迅捷通电子商务有限公司";
                gaOrder.OrderHead.ebpCode = EbcCode;
                gaOrder.OrderHead.ebpName = EbcName;
                gaOrder.OrderHead.freight = order.FreightAmount.ToString();
                gaOrder.OrderHead.goodsValue = order.ProductAmount.ToString();
                gaOrder.OrderHead.guid = System.Guid.NewGuid().ToString("D").ToUpper();
                //gaOrder.OrderHead.note = "";
                gaOrder.OrderHead.orderNo = orderNo;// order.SysNo.ToString();
                gaOrder.OrderHead.orderType = "I";

                gaOrder=SetPayCodeAndPayName(order.PayTypeSysNo, gaOrder);

                gaOrder.OrderHead.payTransactionId = onlinePayments[0].VoucherNo;// recVoucher.VoucherItems[0].VoucherNo;
                gaOrder.OrderHead.taxTotal = order.TaxFee.ToString(); // "0";
                gaOrder.OrderList = new List<OrderList>();
                int _gnum = 1;
                foreach (var item in order.OrderItemList)
                {
                    var orderList = new OrderList();
                    #region 条形码商品
                    var RefProductName = item.ProductName;
                    var RefProductSysNo = item.ProductSysNo;
                    var RefProductQuantity = item.Quantity;
                    BLL.Product.PdProductBo.Instance.RefProductQuantity(ref RefProductSysNo, ref RefProductQuantity, ref RefProductName, order.SysNo);
                    #endregion
                    //orderList.barCode = "";
                    orderList.country = "116";
                    orderList.currency = "142";
                    orderList.gnum = _gnum.ToString();
                    orderList.itemDescribe = "";
                    //orderList.itemName = item.ProductName;
                    //orderList.itemNo = item.ProductSysNo.ToString();// "";
                    orderList.itemName = RefProductName;
                    orderList.itemNo = RefProductSysNo.ToString();
                    //orderList.note = "";
                    orderList.price = item.SalesUnitPrice.ToString();
                    //orderList.qty = item.Quantity.ToString();
                    orderList.qty = RefProductQuantity.ToString();
                    orderList.totalPrice = (item.SalesAmount + item.ChangeAmount).ToString();
                    orderList.unit = "007";
                    gaOrder.OrderList.Add(orderList);

                    _gnum++;
                }
                CEBMessage.GAOrder.Add(gaOrder);

                CEBMessage.BaseTransfer = new BaseTransfer();
                //CEBMessage.BaseTransfer.copCode = EbcCode;
                //CEBMessage.BaseTransfer.copName = EbcName;
                CEBMessage.BaseTransfer.copCode = EbcCode;
                CEBMessage.BaseTransfer.copName = EbcName;
                // 电子口岸固定DXP
                CEBMessage.BaseTransfer.dxpId = DxpId;// "DXPENTTEST510001";
                CEBMessage.BaseTransfer.dxpMode = "DXP";
                //CEBMessage.BaseTransfer.note = "";

                XmlSerializerNamespaces xmlNs = new XmlSerializerNamespaces();
                xmlNs.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
                xmlNs.Add("nousexmlns", "http://www.chinaport.gov.cn/ceb");
                string xmlData = this.XmlSerialize<CEB311Message>(CEBMessage,xmlNs);
                xmlData = xmlData.Replace(":nousexmlns", "");
                xmlData = xmlData.Replace("encoding=\"utf-16\"", "encoding=\"UTF-8\"");

                // 上传文件
                var ftp = new FtpUtil(FTPUri, FTPName, FTPPassword);
                string ftpResponse = "";
                var random = new Random();
                string fileName = "GZEPORT_" + FTPName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + random.Next(10000, 100000).ToString() + ".xml";



                ftp.UploadFile(FTPUri + FolderIn, fileName, Encoding.UTF8.GetBytes(xmlData), out ftpResponse);

                #region 海关推送日志
                var customsOrderLog = new SoCustomsOrderLog();

                if (customsLogInfo != null)
                    customsOrderLog = customsLogInfo;

                customsOrderLog.Packets = xmlData;
                customsOrderLog.StatusCode = appType;

                if (appType == "1")//1-新增 2-变更 3-删除
                    customsOrderLog.StatusMsg = "新增中";
                else if (appType == "2")
                    customsOrderLog.StatusMsg = "变更中";
                else if (appType == "3")                              
                    customsOrderLog.StatusMsg = "删除中";
              

               
                customsOrderLog.LastUpdateBy = 0;
                if (BLL.Authentication.AdminAuthenticationBo.Instance.IsLogin)
                    customsOrderLog.LastUpdateBy = BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo;
                customsOrderLog.LastUpdateDate = DateTime.Now;
                customsOrderLog.CreateDate = DateTime.Now;
                customsOrderLog.CreatedBy = customsOrderLog.LastUpdateBy;
                customsOrderLog.ReceiptContent = "";
                customsOrderLog.FileName = fileName;
                customsOrderLog.OrderSysNo = orderId;
                customsOrderLog.CustomsChannel = (int)this.Code;
                customsOrderLog.StatusCode = appType;


                if (customsLogInfo == null)
                    BLL.Order.SoCustomsOrderLogBo.Instance.AddCustomsOrderLog(customsOrderLog);
                else
                    BLL.Order.SoCustomsOrderLogBo.Instance.UpdateCustomsOrderLog(customsOrderLog);

                BLL.Order.SoOrderBo.Instance.UpdateOrderApiStatus((int)Hyt.Model.WorkflowStatus.OrderStatus.海关报关状态.处理中, 2, orderId);        
                #endregion

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
        public class CEB311Message
        {
            [XmlAttribute("guid")]
            public string guid { get; set; }

            [XmlAttribute("version")]
            public string version { get; set; }

            [XmlElement(ElementName = "Order")]
            public List<GAOrder> GAOrder { get; set; }

            public BaseTransfer BaseTransfer { get; set; }

            public BaseSubscribe BaseSubscribe { get; set; }

            public Signature Signature { get; set; }

            public ExtendMessage ExtendMessage { get; set; }
        }

        public class GAOrder
        {
            public OrderHead OrderHead { get; set; }
            [XmlElement(ElementName = "OrderList")]
            public List<OrderList> OrderList { get; set; }
        }

        /// <summary>
        /// 电子订单表头
        /// </summary>
        public class OrderHead
        {
            /// <summary>
            /// 系统唯一序号 guid C36 是 企业系统生成36位唯一序号（英文字母大写）。
            /// </summary>
            [XmlElement(ElementName = "guid")]
            public string guid { get; set; }
            /// <summary>
            /// 报送类型 appType C1 是 企业报送类型。1-新增 2-变更 3-删除。默认为1。
            /// </summary>
            [XmlElement(ElementName = "appType")]
            public string appType { get; set; }
            /// <summary>
            /// 报送时间 appTime C14 是 企业报送时间。格式:YYYYMMDDhhmmss。
            /// </summary>
            [XmlElement(ElementName = "appTime")]
            public string appTime { get; set; }
            /// <summary>
            /// 业务状态 appStatus C..3 是 业务状态:1-暂存,2-申报,默认为2。
            /// </summary>
            [XmlElement(ElementName = "appStatus")]
            public string appStatus { get; set; }
            /// <summary>
            /// 订单类型 orderType C1 是 电子订单类型：I进口
            /// </summary>
            [XmlElement(ElementName = "orderType")]
            public string orderType { get; set; }
            /// <summary>
            /// 订单编号 orderNo C..60 是 交易平台的订单编号，同一交易平台的订单编号应唯一。订单编号长度不能超过60位。
            /// </summary>
            [XmlElement(ElementName = "orderNo")]
            public string orderNo { get; set; }
            /// <summary>
            /// 电商平台代码 ebpCode C..18 是 电商平台的海关注册登记编号；电商平台未在海关注册登记，由电商企业发送订单的，以中国电子口岸发布的电商平台标识编号为准。
            /// </summary>
            [XmlElement(ElementName = "ebpCode")]
            public string ebpCode { get; set; }
            /// <summary>
            /// 电商平台名称 ebpName C..100 是 电商平台的海关注册登记名称；电商平台未在海关注册登记，由电商企业发送订单的，以中国电子口岸发布的电商平台名称为准。
            /// </summary>
            [XmlElement(ElementName = "ebpName")]
            public string ebpName { get; set; }
            /// <summary>
            /// 电商企业代码 ebcCode C..18 是 电商企业的海关注册登记编号。
            /// </summary>
            [XmlElement(ElementName = "ebcCode")]
            public string ebcCode { get; set; }
            /// <summary>
            /// 电商企业名称 ebcName C..100 是 电商企业的海关注册登记名称。
            /// </summary>
            [XmlElement(ElementName = "ebcName")]
            public string ebcName { get; set; }
            /// <summary>
            /// 商品价格 goodsValue N19,5 是 商品实际成交价，含非现金抵扣金额。
            /// </summary>
            [XmlElement(ElementName = "goodsValue")]
            public string goodsValue { get; set; }
            /// <summary>
            /// 运杂费 freight N19,5 是 不包含在商品价格中的运杂费，无则填写"0"。
            /// </summary>
            [XmlElement(ElementName = "freight")]
            public string freight { get; set; }
            /// <summary>
            /// 非现金抵扣金额 discount N19,5 是 使用积分、虚拟货币、代金券等非现金支付金额，无则填写"0"。
            /// </summary>
            [XmlElement(ElementName = "discount")]
            public string discount { get; set; }
            /// <summary>
            /// 代扣税款 taxTotal N19,5 是 企业预先代扣的税款金额，无则填写“0”
            /// </summary>
            [XmlElement(ElementName = "taxTotal")]
            public string taxTotal { get; set; }
            /// <summary>
            /// 实际支付金额 acturalPaid N19,5 是 商品价格+运杂费+代扣税款-非现金抵扣金额，与支付凭证的支付金额一致。
            /// </summary>
            [XmlElement(ElementName = "acturalPaid")]
            public string acturalPaid { get; set; }
            /// <summary>
            /// 币制 currency C3 是 限定为人民币，填写“142”。
            /// </summary>
            [XmlElement(ElementName = "currency")]
            public string currency { get; set; }
            /// <summary>
            /// 订购人注册号 buyerRegNo C..60 是 订购人的交易平台注册号。
            /// </summary>
            [XmlElement(ElementName = "buyerRegNo")]
            public string buyerRegNo { get; set; }
            /// <summary>
            /// 订购人姓名 buyerName C..60 是 订购人的真实姓名。
            /// </summary>
            [XmlElement(ElementName = "buyerName")]
            public string buyerName { get; set; }
            /// <summary>
            /// 订购人证件类型 buyerIdType C1 是 1-身份证,2-其它。限定为身份证，填写“1”。
            /// </summary>
            [XmlElement(ElementName = "buyerIdType")]
            public string buyerIdType { get; set; }
            /// <summary>
            /// 订购人证件号码 buyerIdNumber C..60 是 订购人的身份证件号码。
            /// </summary>
            [XmlElement(ElementName = "buyerIdNumber")]
            public string buyerIdNumber { get; set; }
            /// <summary>
            /// 支付企业代码 payCode C..18 否 支付企业的海关注册登记编号。
            /// </summary>
            [XmlElement(ElementName = "payCode")]
            public string payCode { get; set; }
            /// <summary>
            /// 支付企业名称 payName C..100 否 支付企业在海关注册登记的企业名称。
            /// </summary>
            [XmlElement(ElementName = "payName")]
            public string payName { get; set; }
            /// <summary>
            /// 支付交易编号 payTransactionId C..60 否 支付企业唯一的支付流水号。
            /// </summary>
            [XmlElement(ElementName = "payTransactionId")]
            public string payTransactionId { get; set; }
            /// <summary>
            /// 商品批次号 batchNumbers C..100 否 商品批次号。
            /// </summary>
            [XmlElement(ElementName = "batchNumbers")]
            public string batchNumbers { get; set; }
            /// <summary>
            /// 收货人姓名 consignee C..100 是 收货人姓名，必须与电子运单的收货人姓名一致。
            /// </summary>
            [XmlElement(ElementName = "consignee")]
            public string consignee { get; set; }
            /// <summary>
            /// 收货人电话 consigneeTelephone C..50 是 收货人联系电话，必须与电子运单的收货人电话一致。
            /// </summary>
            [XmlElement(ElementName = "consigneeTelephone")]
            public string consigneeTelephone { get; set; }
            /// <summary>
            /// 收货地址 consigneeAddress C..200 是 收货地址，必须与电子运单的收货地址一致。
            /// </summary>
            [XmlElement(ElementName = "consigneeAddress")]
            public string consigneeAddress { get; set; }
            /// <summary>
            /// 收货地址行政区划代码 consigneeDitrict C6 否 参照国家统计局公布的国家行政区划标准填制。
            /// </summary>
            [XmlElement(ElementName = "consigneeDitrict")]
            public string consigneeDitrict { get; set; }
            /// <summary>
            /// 备注 note C..1000 否
            /// </summary>
            [XmlElement(ElementName = "note")]
            public string note { get; set; }
        }

        /// <summary>
        /// 电子订单商品表体
        /// </summary>
        public class OrderList
        {
            /// <summary>
            /// 商品序号 gnum N4 是 从1开始的递增序号。
            /// </summary>
            [XmlElement(ElementName = "gnum")]
            public string gnum { get; set; }
            /// <summary>
            /// 企业商品货号 itemNo C..30 否 电商企业自定义的商品货号（SKU）。
            /// </summary>
            [XmlElement(ElementName = "itemNo")]
            public string itemNo { get; set; }
            /// <summary>
            /// 企业商品名称 itemName C..250 是 交易平台销售商品的中文名称。
            /// </summary>
            [XmlElement(ElementName = "itemName")]
            public string itemName { get; set; }
            /// <summary>
            /// 企业商品描述 itemDescribe C..1000 否 交易平台销售商品的描述信息。
            /// </summary>
            [XmlElement(ElementName = "itemDescribe")]
            public string itemDescribe { get; set; }
            /// <summary>
            /// 条形码 barCode C..50 否 国际通用的商品条形码，一般由前缀部分、制造厂商代码、商品代码和校验码组成。
            /// </summary>
            [XmlElement(ElementName = "barCode")]
            public string barCode { get; set; }
            /// <summary>
            /// 单位 unit C3 是 填写海关标准的参数代码，参照《JGS-20 海关业务代码集》- 计量单位代码。
            /// </summary>
            [XmlElement(ElementName = "unit")]
            public string unit { get; set; }
            /// <summary>
            /// 数量 qty N19,5 是 商品实际数量。
            /// </summary>
            [XmlElement(ElementName = "qty")]
            public string qty { get; set; }
            /// <summary>
            /// 单价 price N19,5 是 商品单价。赠品单价填写为“0”。
            /// </summary>
            [XmlElement(ElementName = "price")]
            public string price { get; set; }
            /// <summary>
            /// 总价 totalPrice N19,5 是 商品总价，等于单价乘以数量。
            /// </summary>
            [XmlElement(ElementName = "totalPrice")]
            public string totalPrice { get; set; }
            /// <summary>
            /// 币制 currency C3 是 限定为人民币，填写“142”。
            /// </summary>
            [XmlElement(ElementName = "currency")]
            public string currency { get; set; }
            /// <summary>
            /// 原产国 country C3 是 填写海关标准的参数代码，参照《JGS-20 海关业务代码集》-国家（地区）代码表。
            /// </summary>
            [XmlElement(ElementName = "country")]
            public string country { get; set; }
            /// <summary>
            /// 备注 note C..1000 否 促销活动，商品单价偏离市场价格的，可以在此说明。
            /// </summary>
            [XmlElement(ElementName = "note")]
            public string note { get; set; }
        }

        public class BaseTransfer
        {
            /// <summary>
            /// 传输企业代码 copCode C..18 是 报文传输的企业代码（需要与接入客户端的企业身份一致）
            /// </summary>
            [XmlElement(ElementName = "copCode")]
            public string copCode { get; set; }
            /// <summary>
            /// 传输企业名称 copName C..100 是 报文传输的企业名称
            /// </summary>
            [XmlElement(ElementName = "copName")]
            public string copName { get; set; }
            /// <summary>
            /// 报文传输模式 dxpMode C3 是 默认为DXP；指中国电子口岸数据交换平台
            /// </summary>
            [XmlElement(ElementName = "dxpMode")]
            public string dxpMode { get; set; }
            /// <summary>
            /// 报文传输编号 dxpId C..30 是 向中国电子口岸数据中心申请数据交换平台的用户编号
            /// </summary>
            [XmlElement(ElementName = "dxpId")]
            public string dxpId { get; set; }
            /// <summary>
            /// 备注 note C..1000 否
            /// </summary>
            [XmlElement(ElementName = "note")]
            public string note { get; set; }
        }

        public class BaseSubscribe
        {
            /// <summary>
            /// 订阅状态 status C..100 是 用户订阅单证业务状态的信息, ALL-订阅数据和回执,  DATA-只订阅数据,RET- 只订阅回执
            /// </summary>
            [XmlElement(ElementName = "status")]
            public string status { get; set; }
            /// <summary>
            /// 订阅方传输模式 dxpMode C3 是 默认为DXP；指中国电子口岸数据交换平台
            /// </summary>
            [XmlElement(ElementName = "dxpMode")]
            public string dxpMode { get; set; }
            /// <summary>
            /// 订阅方传输地址 dxpAddress C..100 是 向中国电子口岸数据中心申请数据交换平台的用户编号
            /// </summary>
            [XmlElement(ElementName = "dxpAddress")]
            public string dxpAddress { get; set; }
            /// <summary>
            /// 备注 note C..1000 否
            /// </summary>
            [XmlElement(ElementName = "note")]
            public string note { get; set; }
        }

        public class Signature
        {
            /// <summary>
            /// 签名基本信息节点 SignedInfo  是
            /// </summary>
            [XmlElement(ElementName = "SignedInfo")]
            public string SignedInfo { get; set; }
            /// <summary>
            /// 规范化方法 CanonicalizationMethod C..50 是 Algorithm="http://www.w3.org/TR/2001/REC-xml-c14n-20010315"
            /// </summary>
            [XmlElement(ElementName = "CanonicalizationMethod")]
            public string CanonicalizationMethod { get; set; }
            /// <summary>
            /// 签名算法 SignatureMethod C..50 是 Algorithm="http://www.w3.org/2000/09/xmldsig#rsa-sha1"
            /// </summary>
            [XmlElement(ElementName = "SignatureMethod")]
            public string SignatureMethod { get; set; }
            /// <summary>
            /// 引用 Reference  是 
            /// </summary>
            [XmlElement(ElementName = "Reference")]
            public string Reference { get; set; }
            /// <summary>
            /// 转换方式 Transforms   Algorithm="http://www.w3.org/2000/09/xmldsig#enveloped-signature"
            /// </summary>
            [XmlElement(ElementName = "Transforms")]
            public string Transforms { get; set; }
            /// <summary>
            /// 摘要算法 DigestMethod C..100 是 Algorithm="http://www.w3.org/2000/09/xmldsig#sha1"
            /// </summary>
            [XmlElement(ElementName = "DigestMethod")]
            public string DigestMethod { get; set; }
            /// <summary>
            /// 摘要值 DigestValue C..100 是 摘要结果
            /// </summary>
            [XmlElement(ElementName = "DigestValue")]
            public string DigestValue { get; set; }
            /// <summary>
            /// 签名结果信息节点 SignatureValue C..1000 是 签名结果
            /// </summary>
            [XmlElement(ElementName = "SignatureValue")]
            public string SignatureValue { get; set; }
            /// <summary>
            /// 签名Key信息节点 KeyInfo  是 签名Key信息内容
            /// </summary>
            [XmlElement(ElementName = "KeyInfo")]
            public string KeyInfo { get; set; }
            /// <summary>
            /// 证书编号 KeyName C30 是 证书编号
            /// </summary>
            [XmlElement(ElementName = "KeyName")]
            public string KeyName { get; set; }
            /// <summary>
            /// 证书数据 X509Data  否 
            /// </summary>
            [XmlElement(ElementName = "X509Data")]
            public string X509Data { get; set; }
            /// <summary>
            /// 证书内容 X509Certificate C..3000 否 包含公钥的证书信息PEM
            /// </summary>
            [XmlElement(ElementName = "X509Certificate")]
            public string X509Certificate { get; set; }
        }

        public class ExtendMessage
        {
            /// <summary>
            /// 自定义报文名称 name C..30 是 自定义报文名称
            /// </summary>
            [XmlElement(ElementName = "name")]
            public string name { get; set; }
            /// <summary>
            /// 自定义报文版本 version C..30 是 自定义报文版本
            /// </summary>
            [XmlElement(ElementName = "version")]
            public string version { get; set; }
            /// <summary>
            /// 自定义报文实体 Message xml节点 是 自定义报文实体
            /// </summary>
            [XmlElement(ElementName = "Message")]
            public string Message { get; set; }
        }

        /// <summary>
        /// 电子订单回执
        /// </summary>
       [Serializable]
        public class CEB312Message
        {
            [XmlElement(ElementName = "OrderReturn")]
            public OrderReturn OrderReturn { get; set; }
        }
        [Serializable]
        public class OrderReturn
        {
            /// <summary>
            /// C36 是 电子口岸系统生成36位唯一序号（英文字母大写）
            /// </summary>
            [XmlElement(ElementName = "guid")]
            public string guid { get; set; }
            /// <summary>
            /// C..18 是 电商平台的海关注册登记编号；电商平台未在海关注册登记，由电商企业发送订单的，以中国电子口岸发布的电商平台标识编号为准。
            /// </summary>
            [XmlElement(ElementName = "ebpCode")]
            public string ebpCode { get; set; }
            /// <summary>
            /// C..18 是 电商企业的海关注册登记编号。
            /// </summary>
            [XmlElement(ElementName = "ebcCode")]
            public string ebcCode { get; set; }
            /// <summary>
            /// C..60 是 交易平台的订单编号，同一交易平台的订单编号应唯一。订单编号长度不能超过60位。
            /// </summary>
            [XmlElement(ElementName = "orderNo")]
            public string orderNo { get; set; }
            /// <summary>
            /// C1..10 是 操作结果（2电子口岸申报中/3发送海关成功/4发送海关失败/100海关退单/120海关入库）,若小于0数字表示处理异常回执
            /// </summary>
            [XmlElement(ElementName = "returnStatus")]
            public string returnStatus { get; set; }
            /// <summary>
            /// C14 是 操作时间(格式:YYYYMMDDhhmmss)
            /// </summary>
            [XmlElement(ElementName = "returnTime")]
            public string returnTime { get; set; }
            /// <summary>
            /// C..1000 是 备注（如:退单原因）
            /// </summary>
            [XmlElement(ElementName = "returnInfo")]
            public string returnInfo { get; set; }
        }

        #endregion
    }
}