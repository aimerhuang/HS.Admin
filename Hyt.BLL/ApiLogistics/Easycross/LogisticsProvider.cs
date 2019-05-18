using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Util.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Hyt.BLL.ApiLogistics.Easycross
{
    /// <summary>
    /// 珠海易跨境物流接口
    /// </summary>
    /// <remarks>2015-10-12 陈海裕 创建</remarks>
    public class LogisticsProvider : ILogisticsProvider
    {
        public LogisticsProvider() { }

        public override Hyt.Model.CommonEnum.物流代码 Code
        {
            get { return Hyt.Model.CommonEnum.物流代码.珠海易跨境; }
        }

        private static string FTPName = "ikanFTP";
        private static string FTPPassword = "ikanFTP@2016";
        private static string FTPUri = "ftp://202.175.103.58:21/";

        public override Result GetProduct(string productId)
        {
            throw new NotImplementedException();
        }

        public override Result AddOrderTrade(int orderSysno)
        {
            Result result = new Result();

            try
            {
                SoOrder order = BLL.Order.SoOrderBo.Instance.GetEntity(orderSysno);
                if (order == null)
                {
                    result.Message = "该订单不存在";
                    return result;
                }
                order.ReceiveAddress = BLL.Order.SoOrderBo.Instance.GetOrderReceiveAddress(order.ReceiveAddressSysNo);
                order.OrderItemList = DataAccess.Order.ISoOrderItemDao.Instance.GetOrderItemsByOrderSysNo(order.SysNo);
                ParaVoucherFilter voucherFilter = new ParaVoucherFilter();
                voucherFilter.SourceSysNo = order.SysNo;
                CBFnReceiptVoucher recVoucher = BLL.Finance.FinanceBo.Instance.GetReceiptVouchers(voucherFilter).Rows.FirstOrDefault();
                recVoucher.VoucherItems = BLL.Finance.FnReceiptVoucherBo.Instance.GetReceiptVoucherItem(recVoucher.SysNo);
                // 收货人 区 市 省
                BsArea receiverDistrict = BLL.Basic.BasicAreaBo.Instance.GetArea(order.ReceiveAddress.AreaSysNo);
                BsArea receiverCity = BLL.Basic.BasicAreaBo.Instance.GetArea(receiverDistrict.ParentSysNo);
                BsArea receiverProvince = BLL.Basic.BasicAreaBo.Instance.GetArea(receiverCity.ParentSysNo);
                // 发货人 市
                CBWhWarehouse warehouse = BLL.Warehouse.WhWarehouseBo.Instance.GetWarehouse(order.DefaultWarehouseSysNo);
                BsArea shipperCity = BLL.Basic.BasicAreaBo.Instance.GetArea(warehouse.CitySysNo);

                DsDealer dealer = BLL.Distribution.DsDealerBo.Instance.GetDsDealer(order.DealerSysNo);

                LgDeliveryType deliveryType = BLL.Logistics.DeliveryTypeBo.Instance.GetDeliveryType(order.DeliveryTypeSysNo);

                EasycrossOrder newOrder = new EasycrossOrder();
                newOrder.OrderHead = new OrderHead();
                newOrder.OrderList = new List<OrderList>();
                newOrder.OrderPaymentLogistics = new OrderPaymentLogistics();

                newOrder.OrderHead.accessType = "1";
                newOrder.OrderHead.agentCode = "4404580006";
                newOrder.OrderHead.agentName = "珠海易跨境电子商务服务有限公司";
                newOrder.OrderHead.appStatus = "2";
                newOrder.OrderHead.appTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                newOrder.OrderHead.appUid = "1105910159";
                newOrder.OrderHead.appUname = "东方口岸";
                newOrder.OrderHead.batchNumbers = "";
                newOrder.OrderHead.cbeCode = "D00236";
                newOrder.OrderHead.cbeName = "珠海爱勤电子科技有限公司";
                newOrder.OrderHead.charge = order.OrderAmount.ToString();
                newOrder.OrderHead.consignee = order.ReceiveAddress.Name;
                newOrder.OrderHead.consigneeAddress = order.ReceiveAddress.StreetAddress;
                newOrder.OrderHead.consigneeCountry = "";
                newOrder.OrderHead.consigneeTelephone = !string.IsNullOrWhiteSpace(order.ReceiveAddress.MobilePhoneNumber)
                    ? order.ReceiveAddress.MobilePhoneNumber : order.ReceiveAddress.PhoneNumber;
                newOrder.OrderHead.currency = "142";
                newOrder.OrderHead.customer = order.ReceiveAddress.Name;
                newOrder.OrderHead.customerId = order.ReceiveAddress.IDCardNo;
                newOrder.OrderHead.ecpCode = "W0098";
                newOrder.OrderHead.ecpName = "珠海爱勤电子科技有限公司";
                newOrder.OrderHead.freight = order.FreightAmount.ToString();
                newOrder.OrderHead.goodsValue = order.ProductAmount.ToString();
                newOrder.OrderHead.idType = "1";
                newOrder.OrderHead.ieType = "I";
                newOrder.OrderHead.modifyMark = "1";
                newOrder.OrderHead.note = "";
                newOrder.OrderHead.orderNo = order.SysNo.ToString();
                newOrder.OrderHead.other = "";
                newOrder.OrderHead.shipper = BLL.Stores.StoresBo.Instance.GetStoreById(0).ErpName;
                newOrder.OrderHead.shipperAddress = "";
                newOrder.OrderHead.shipperCountry = "142";
                newOrder.OrderHead.shipperTelephone = "";
                newOrder.OrderHead.tax = order.TaxFee.ToString();
                newOrder.OrderHead.totalLogisticsNo = "";
                newOrder.OrderHead.tradeCountry = "";
                newOrder.OrderHead.wrapType = "";
                foreach (SoOrderItem item in order.OrderItemList)
                {
                    PdProduct product = BLL.Product.PdProductBo.Instance.GetProduct(item.SysNo);
                    PdProductStock productStock = BLL.Warehouse.PdProductStockBo.Instance.GetEntityByWP(order.DefaultWarehouseSysNo, item.ProductSysNo);

                    string brandName = "无";
                    PdBrand brand = BLL.Product.PdBrandBo.Instance.GetEntity(product.BrandSysNo);
                    if (brand != null)
                    {
                        brandName = brand.Name;
                    }

                    OrderList orderList = new OrderList();
                    orderList.barCode = "";
                    orderList.brand = brandName;
                    orderList.codeTs = "";
                    orderList.country = "";
                    orderList.currency = "142";
                    orderList.describe = "";
                    orderList.discount = "";
                    orderList.giftFlag = "";
                    orderList.goodsModel = "";
                    orderList.goodsName = item.ProductName;
                    orderList.goodsNo = productStock.ProductSku;
                    orderList.itemNo = "";
                    orderList.note = "";
                    orderList.packNum = "";
                    orderList.price = item.SalesUnitPrice.ToString();
                    orderList.priceTotal = item.SalesAmount.ToString();
                    orderList.purposeCode = "";
                    orderList.quantity = item.Quantity.ToString();
                    orderList.shelfGoodsName = item.ProductName;
                    orderList.taxCode = "";
                    orderList.unit = "007";
                    orderList.wasteMaterials = "1";
                    orderList.wrapType = "";
                    newOrder.OrderList.Add(orderList);
                }
                //newOrder.OrderPaymentLogistics.logisticsCode = "";
                newOrder.OrderPaymentLogistics.logisticsName = deliveryType.DeliveryTypeName;
                //newOrder.OrderPaymentLogistics.logisticsNo = "";
                //newOrder.OrderPaymentLogistics.paymentCode = "";
                //newOrder.OrderPaymentLogistics.paymentName = "";
                //newOrder.OrderPaymentLogistics.paymentNo = "";
                //newOrder.OrderPaymentLogistics.paymentType = "";
                //newOrder.OrderPaymentLogistics.trackNo = "";

                XmlSerializerNamespaces xmlNs = new XmlSerializerNamespaces();
                xmlNs.Add("nousexmlns", "http://www.chinaport.gov.cn/ecss");
                string xmlData = this.XmlSerialize<EasycrossOrder>(newOrder, xmlNs);
                xmlData = xmlData.Replace(":nousexmlns", "");
                xmlData = xmlData.Replace("encoding=\"utf-16\"", "encoding=\"utf-8\"");

                // 上传文件
                FtpUtil ftp = new FtpUtil(FTPUri, FTPName, FTPPassword);
                string ftpResponse = "";
                ftp.UploadFile(FTPUri, "Testfilename.xml", Encoding.UTF8.GetBytes(xmlData), out ftpResponse);
            }
            catch (Exception ex)
            {
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

        public Result GetOrderRec(string orderNo)
        {
            Result result = new Result();
            result.Message = "暂未生成相应的回执";
            try
            {
                FtpUtil ftp = new FtpUtil("", "name", "password");
                string ftpResponse = "";
                Stream stream = ftp.FileStream("", ref ftpResponse);
                // 设置当前流的位置为流的开始，防止读取位置错误造成无法读取完整流的内容
                stream.Seek(0, SeekOrigin.Begin);
                using (StreamReader reader = new StreamReader(stream))
                {
                    string data = reader.ReadToEnd();
                    // 接收回执
                    if (data.Contains("OrderRec"))
                    {
                        OrderRec orderRec = Util.Serialization.SerializationUtil.XmlDeserialize<OrderRec>(data);
                        if (orderRec.orderNo == orderNo)
                        {
                            // TODO:更新订单推送状态
                            result.Status = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
            }
            return result;
        }

        public override Result GetOrderExpressno(string orderId)
        {
            throw new NotImplementedException();
        }

        public override Result GetOrderTrade(string orderId)
        {
            throw new NotImplementedException();
        }

        public override Result<string> GetLogisticsTracking(int orderSysNo)
        {
            throw new NotImplementedException();
        }

        #region 实体类

        /// <summary>
        /// 订单
        /// </summary>
        [XmlRoot(ElementName = "Order")]
        public class EasycrossOrder
        {
            [XmlElement(ElementName = "OrderHead")]
            public OrderHead OrderHead { get; set; }

            [XmlElement(ElementName = "OrderList")]
            public List<OrderList> OrderList { get; set; }

            [XmlElement(ElementName = "OrderPaymentLogistics")]
            public OrderPaymentLogistics OrderPaymentLogistics { get; set; }
        }

        /// <summary>
        /// 订单信息表头
        /// </summary>
        public class OrderHead
        {
            /// <summary>
            /// 电商代码 是
            /// </summary>
            [XmlElement(ElementName = "cbeCode")]
            public string cbeCode { get; set; }
            /// <summary>
            /// 电商名称 是
            /// </summary>
            [XmlElement(ElementName = "cbeName")]
            public string cbeName { get; set; }
            /// <summary>
            /// 电商平台代码 是
            /// </summary>
            [XmlElement(ElementName = "ecpCode")]
            public string ecpCode { get; set; }
            /// <summary>
            /// 电商平台名称 是
            /// </summary>
            [XmlElement(ElementName = "ecpName")]
            public string ecpName { get; set; }
            /// <summary>
            /// 订单编号 是
            /// </summary>
            [XmlElement(ElementName = "orderNo")]
            public string orderNo { get; set; }
            /// <summary>
            /// 总费用 是
            /// </summary>
            [XmlElement(ElementName = "charge")]
            public string charge { get; set; }
            /// <summary>
            /// 货值 是
            /// </summary>
            [XmlElement(ElementName = "goodsValue")]
            public string goodsValue { get; set; }
            /// <summary>
            /// 运费 否
            /// </summary>
            [XmlElement(ElementName = "freight")]
            public string freight { get; set; }
            /// <summary>
            /// 其他费用 否
            /// </summary>
            [XmlElement(ElementName = "other")]
            public string other { get; set; }
            /// <summary>
            /// 币制 是 海关标准的参数代码
            /// </summary>
            [XmlElement(ElementName = "currency")]
            public string currency { get; set; }
            /// <summary>
            /// 进口行邮税 否
            /// </summary>
            [XmlElement(ElementName = "tax")]
            public string tax { get; set; }
            /// <summary>
            /// 客户姓名 否
            /// </summary>
            [XmlElement(ElementName = "customer")]
            public string customer { get; set; }
            /// <summary>
            /// 发货人名称 否
            /// </summary>
            [XmlElement(ElementName = "shipper")]
            public string shipper { get; set; }
            /// <summary>
            /// 发货人地址 否
            /// </summary>
            [XmlElement(ElementName = "shipperAddress")]
            public string shipperAddress { get; set; }
            /// <summary>
            /// 发货人电话 否
            /// </summary>
            [XmlElement(ElementName = "shipperTelephone")]
            public string shipperTelephone { get; set; }
            /// <summary>
            /// 发货人所在国 否	海关国家参数
            /// </summary>
            [XmlElement(ElementName = "shipperCountry")]
            public string shipperCountry { get; set; }
            /// <summary>
            /// 收货人名称 是
            /// </summary>
            [XmlElement(ElementName = "consignee")]
            public string consignee { get; set; }
            /// <summary>
            /// 收货人地址 是
            /// </summary>
            [XmlElement(ElementName = "consigneeAddress")]
            public string consigneeAddress { get; set; }
            /// <summary>
            /// 收货人电话 是
            /// </summary>
            [XmlElement(ElementName = "consigneeTelephone")]
            public string consigneeTelephone { get; set; }
            /// <summary>
            /// 收货人所在国 出口必填,进口非必填 海关国家参数
            /// </summary>
            [XmlElement(ElementName = "consigneeCountry")]
            public string consigneeCountry { get; set; }
            /// <summary>
            /// 证件类型 进口必填,出口非必填 1-身份证/2-军官证/3-护照/4-其它
            /// </summary>
            [XmlElement(ElementName = "idType")]
            public string idType { get; set; }
            /// <summary>
            /// 证件号码 进口必填,出口非必填
            /// </summary>
            [XmlElement(ElementName = "customerId")]
            public string customerId { get; set; }
            /// <summary>
            /// 接入类型 是 1-总署/2-地方
            /// </summary>
            [XmlElement(ElementName = "accessType")]
            public string accessType { get; set; }
            /// <summary>
            /// 进出口标志 是 I-进口/E-出口
            /// </summary>
            [XmlElement(ElementName = "ieType")]
            public string ieType { get; set; }
            /// <summary>
            /// 批次号 否 新增字段
            /// </summary>
            [XmlElement(ElementName = "batchNumbers")]
            public string batchNumbers { get; set; }
            /// <summary>
            /// 总运单号 否 新增字段
            /// </summary>
            [XmlElement(ElementName = "totalLogisticsNo")]
            public string totalLogisticsNo { get; set; }
            /// <summary>
            /// 贸易国别 否 新增字段
            /// </summary>
            [XmlElement(ElementName = "tradeCountry")]
            public string tradeCountry { get; set; }
            /// <summary>
            /// 代理企业 否 新增字段
            /// </summary>
            [XmlElement(ElementName = "agentCode")]
            public string agentCode { get; set; }
            /// <summary>
            /// 代理企业名称 否 新增字段
            /// </summary>
            [XmlElement(ElementName = "agentName")]
            public string agentName { get; set; }
            /// <summary>
            /// 包装种类 否
            /// </summary>
            [XmlElement(ElementName = "wrapType")]
            public string wrapType { get; set; }
            /// <summary>
            /// 操作类型 是 1-新增/2-修改/3-删除
            /// </summary>
            [XmlElement(ElementName = "modifyMark")]
            public string modifyMark { get; set; }
            /// <summary>
            /// 备注 否
            /// </summary>
            [XmlElement(ElementName = "note")]
            public string note { get; set; }
            /// <summary>
            /// 申报时间 是	格式:YYYYMMDDhhmmss
            /// </summary>
            [XmlElement(ElementName = "appTime")]
            public string appTime { get; set; }
            /// <summary>
            /// 业务状态 是 1-暂存/2-申报,默认为2
            /// </summary>
            [XmlElement(ElementName = "appStatus")]
            public string appStatus { get; set; }
            /// <summary>
            /// 用户编号 是 电子口岸持卡人IC卡或UKEY编号
            /// </summary>
            [XmlElement(ElementName = "appUid")]
            public string appUid { get; set; }
            /// <summary>
            /// 用户名称 是 电子口岸持卡人姓名
            /// </summary>
            [XmlElement(ElementName = "appUname")]
            public string appUname { get; set; }
        }

        /// <summary>
        /// 订单信息表体
        /// </summary>
        public class OrderList
        {
            /// <summary>
            /// 海关备案商品编号 否
            /// </summary>
            [XmlElement(ElementName = "itemNo")]
            public string itemNo { get; set; }
            /// <summary>
            /// 商品货号 是
            /// </summary>
            [XmlElement(ElementName = "goodsNo")]
            public string goodsNo { get; set; }
            /// <summary>
            /// 商品上架品名 是
            /// </summary>
            [XmlElement(ElementName = "shelfGoodsName")]
            public string shelfGoodsName { get; set; }
            /// <summary>
            /// 商品描述 否
            /// </summary>
            [XmlElement(ElementName = "describe")]
            public string describe { get; set; }
            /// <summary>
            /// HS编码 否
            /// </summary>
            [XmlElement(ElementName = "codeTs")]
            public string codeTs { get; set; }
            /// <summary>
            /// 申报品名 否
            /// </summary>
            [XmlElement(ElementName = "goodsName")]
            public string goodsName { get; set; }
            /// <summary>
            /// 规格型号 否
            /// </summary>
            [XmlElement(ElementName = "goodsModel")]
            public string goodsModel { get; set; }
            /// <summary>
            /// 行邮税号 否
            /// </summary>
            [XmlElement(ElementName = "taxCode")]
            public string taxCode { get; set; }
            /// <summary>
            /// 成交单价 否 单价与总价二选一必填
            /// </summary>
            [XmlElement(ElementName = "price")]
            public string price { get; set; }
            /// <summary>
            /// 币制 是 海关币制参数
            /// </summary>
            [XmlElement(ElementName = "currency")]
            public string currency { get; set; }
            /// <summary>
            /// 数量 是
            /// </summary>
            [XmlElement(ElementName = "quantity")]
            public string quantity { get; set; }
            /// <summary>
            /// 成交总价 是 单价与总价二选一必填
            /// </summary>
            [XmlElement(ElementName = "priceTotal")]
            public string priceTotal { get; set; }
            /// <summary>
            /// 计量单位 是 海关计量单位参数
            /// </summary>
            [XmlElement(ElementName = "unit")]
            public string unit { get; set; }
            /// <summary>
            /// 折扣优惠 否
            /// </summary>
            [XmlElement(ElementName = "discount")]
            public string discount { get; set; }
            /// <summary>
            /// 是否赠品 否 0-否/1-是
            /// </summary>
            [XmlElement(ElementName = "giftFlag")]
            public string giftFlag { get; set; }
            /// <summary>
            /// 原产国 否
            /// </summary>
            [XmlElement(ElementName = "country")]
            public string country { get; set; }
            /// <summary>
            /// 用途 否 1-种用或繁殖/2-食用/3- 奶用/4- 观赏或演艺/5-伴侣/6-实验/7-药用/8-饲用 9-其他/10-介质土/A-食品包装材料/B-食品加工设备/C-食品添加剂/D-食品容器/E-食品洗涤剂/F-食品消毒剂
            /// </summary>
            [XmlElement(ElementName = "purposeCode")]
            public string purposeCode { get; set; }
            /// <summary>
            /// 废旧物品 是 1-正常/2-废品/5-旧品/9-其他
            /// </summary>
            [XmlElement(ElementName = "wasteMaterials")]
            public string wasteMaterials { get; set; }
            /// <summary>
            /// 包装种类 否
            /// </summary>
            [XmlElement(ElementName = "wrapType")]
            public string wrapType { get; set; }
            /// <summary>
            /// 件数 否
            /// </summary>
            [XmlElement(ElementName = "packNum")]
            public string packNum { get; set; }
            /// <summary>
            /// 商品条形码 否 商品条形码一般由前缀部分、制造厂商代码、商品代码和校验码组成
            /// </summary>
            [XmlElement(ElementName = "barCode")]
            public string barCode { get; set; }
            /// <summary>
            /// 品牌 是 没有填“无”
            /// </summary>
            [XmlElement(ElementName = "brand")]
            public string brand { get; set; }
            /// <summary>
            /// 备注 否
            /// </summary>
            [XmlElement(ElementName = "note")]
            public string note { get; set; }
        }

        /// <summary>
        /// 订单支付/物流表
        /// </summary>
        public class OrderPaymentLogistics
        {
            /// <summary>
            /// 支付企业代码 否
            /// </summary>
            [XmlElement(ElementName = "paymentCode")]
            public string paymentCode { get; set; }
            /// <summary>
            /// 支付企业名称 否
            /// </summary>
            [XmlElement(ElementName = "paymentName")]
            public string paymentName { get; set; }
            /// <summary>
            /// 支付类型 否 A-全款/S-商户款/X-行邮税
            /// </summary>
            [XmlElement(ElementName = "paymentType")]
            public string paymentType { get; set; }
            /// <summary>
            /// 支付交易号 否
            /// </summary>
            [XmlElement(ElementName = "paymentNo")]
            public string paymentNo { get; set; }
            /// <summary>
            /// 物流企业代码 否
            /// </summary>
            [XmlElement(ElementName = "logisticsCode")]
            public string logisticsCode { get; set; }
            /// <summary>
            /// 物流企业名称 是
            /// </summary>
            [XmlElement(ElementName = "logisticsName")]
            public string logisticsName { get; set; }
            /// <summary>
            /// 物流运单号 否
            /// </summary>
            [XmlElement(ElementName = "logisticsNo")]
            public string logisticsNo { get; set; }
            /// <summary>
            /// 物流跟踪号 否
            /// </summary>
            [XmlElement(ElementName = "trackNo")]
            public string trackNo { get; set; }
        }

        /// <summary>
        /// 订单信息回执
        /// </summary>
        public class OrderRec
        {
            /// <summary>
            /// 电商代码 否
            /// </summary>
            [XmlElement(ElementName = "cbeCode")]
            public string cbeCode { get; set; }
            /// <summary>
            /// 电商名称 是
            /// </summary>
            [XmlElement(ElementName = "cbeName")]
            public string cbeName { get; set; }
            /// <summary>
            /// 电商平台代码 否
            /// </summary>
            [XmlElement(ElementName = "ecpCode")]
            public string ecpCode { get; set; }
            /// <summary>
            /// 电商平台名称 是
            /// </summary>
            [XmlElement(ElementName = "ecpName")]
            public string ecpName { get; set; }
            /// <summary>
            /// 订单编号 是
            /// </summary>
            [XmlElement(ElementName = "orderNo")]
            public string orderNo { get; set; }
            /// <summary>
            /// 进出口标志 是 I-进口/E-出口
            /// </summary>
            [XmlElement(ElementName = "ieType")]
            public string ieType { get; set; }
            /// <summary>
            /// 处理结果状态 是 0-入库失败/1-入库成功
            /// </summary>
            [XmlElement(ElementName = "status")]
            public string status { get; set; }
            /// <summary>
            /// 详细处理结果 是
            /// </summary>
            [XmlElement(ElementName = "detail")]
            public string detail { get; set; }
        }

        #endregion

        public override Result CancelOrderTrade(int orderSysNo, string reason = "")
        {
            throw new NotImplementedException();
        }
    }
}
