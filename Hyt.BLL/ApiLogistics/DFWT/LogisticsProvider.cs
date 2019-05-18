
using Hyt.BLL.Finance;
using Hyt.BLL.Order;
using Hyt.Model;
using Hyt.Model.Common;
using Hyt.Model.Transfer;
using Hyt.Util.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.ApiLogistics.DFWT
{
    public class LogisticsProvider : ILogisticsProvider
    {
        /// <summary>
        /// 物流配置
        /// </summary>
        /// <remarks>2016-3-8 杨浩 创建</remarks>
        protected static LogisticsConfig2 config2 = BLL.Config.Config.Instance.GetLogisticsConfig2();
        public override Model.CommonEnum.物流代码 Code
        {
            get { return Model.CommonEnum.物流代码.东方物通科技; }
        }

        public override Model.Result GetProduct(string productId)
        {
            throw new NotImplementedException();
        }

        public override Model.Result AddOrderTrade(int orderSysno)
        {
            Result result = new Result();
            try
            {
                var order = BLL.Order.SoOrderBo.Instance.GetEntity(orderSysno);

                var config = Hyt.BLL.Config.Config.Instance.GetAnnaConfig();
                CrCustomer customer = Hyt.BLL.CRM.CrCustomerBo.Instance.GetModel(order.CustomerSysNo);

                WhWarehouse warehouseMod = Hyt.BLL.Warehouse.WhWarehouseBo.Instance.GetWarehouse(order.DefaultWarehouseSysNo);

                BsArea wareDistrictEntity = Hyt.BLL.Basic.BasicAreaBo.Instance.GetArea(warehouseMod.AreaSysNo);
                BsArea wareCityEntity = Hyt.BLL.Basic.BasicAreaBo.Instance.GetArea(wareDistrictEntity.ParentSysNo);
                BsArea wareProvinceEntity = Hyt.BLL.Basic.BasicAreaBo.Instance.GetArea(wareDistrictEntity.ParentSysNo);

                SoReceiveAddress srenity = Hyt.BLL.Order.OutboundReturnBo.Instance.GetSoReceiveAddressBysoOrderSysNo(order.SysNo);
                string buyer_idcard = "";
                if (!string.IsNullOrEmpty(srenity.IDCardNo))
                {
                    buyer_idcard = srenity.IDCardNo.Trim().ToUpper();
                }

                BsArea DistrictEntity = Hyt.BLL.Basic.BasicAreaBo.Instance.GetArea(srenity.AreaSysNo);
                string District = DistrictEntity.AreaName.Trim();
                BsArea CityEntity = Hyt.BLL.Basic.BasicAreaBo.Instance.GetArea(DistrictEntity.ParentSysNo);
                string City = CityEntity.AreaName.Trim();
                BsArea ProvinceEntity = Hyt.BLL.Basic.BasicAreaBo.Instance.GetArea(CityEntity.ParentSysNo);
                string Province = ProvinceEntity.AreaName.Trim();

                CBFnOnlinePayment payment = FinanceBo.Instance.GetOnPaymentBySourceSysNo(order.SysNo);
                payment.CusPaymentCode = config.PaymentCode;
                IList<CBSoOrderItem> datao = SoOrderBo.Instance.GetCBOrderItemsByOrderId(order.SysNo);

                LgDeliveryType deliveryType=Hyt.BLL.Logistics.DeliveryTypeBo.Instance.GetDeliveryType(order.DeliveryTypeSysNo);

                order.OrderItemList = new List<SoOrderItem>();
                List<int> ProSysNo = new List<int>();
                foreach (CBSoOrderItem item in datao)
                {
                    ProSysNo.Add(item.ProductSysNo);

                    order.OrderItemList.Add(item);
                }
                IList<CBPdProduct> productList = Hyt.BLL.Product.PdProductBo.Instance.GetProductInfoList(ProSysNo);
                foreach (var mod in productList)
                {

                    var tempitem = datao.First(p => p.ProductSysNo == mod.SysNo);
                    if (tempitem != null)
                    {
                        tempitem.OrginCountry = Hyt.BLL.Basic.OriginBo.Instance.GetEntity(mod.OriginSysNo).Origin_Name;
                    }
                }

                Order pushOrder = new Order();
                pushOrder.OrderHead = new OrderHead();
                pushOrder.OrderPaymentLogistics = new OrderPaymentLogistics();
                pushOrder.itemList = new List<OrderList>();
                BindOrderHeadData(order, pushOrder.OrderHead, payment, srenity, buyer_idcard, District, City, Province, 
                    warehouseMod, wareDistrictEntity, wareCityEntity, wareProvinceEntity);
                BindOrderItemListData(datao, pushOrder.itemList);
                OrderPaymentLogistic(pushOrder.OrderPaymentLogistics, payment, deliveryType);

                string str = Hyt.Util.Serialization.SerializationUtil.XmlSerialize<Order>(pushOrder);
                str = str.Replace("encoding=\"utf-16\"", "encoding=\"utf-8\"");
                str = str.Replace("Root", "ROOT");
                str = str.Replace("<itemList>", "");
                str = str.Replace("</itemList>", "");

                FtpUtil ftp = new FtpUtil(config2.RequestUrl, config2.Account, config2.Password);
                string msg = "";
                string fileName = DateTime.Now.ToString("yyyyMMddhhmmssffff") + ".xml";
                try
                {
                    ftp.UploadFile(config2.RequestUrl, fileName, Encoding.UTF8.GetBytes(str), out msg);
                    
                }
                catch (Exception ex)
                {
                    result.Status = false;
                    result.Message = ex.Message;
                }

                result.Message = str;
            }
            catch (Exception e)
            {
                result.Status = false;
                result.Message = e.Message;
            }
            return result;
        }

        void BindOrderHeadData(SoOrder order ,OrderHead head,CBFnOnlinePayment payment,SoReceiveAddress srenity ,string buyer_idcard ,string District,string City,string Province,
             WhWarehouse warehouseMod,BsArea wareDistrictEntity,BsArea wareCityEntity, BsArea wareProvinceEntity)
        {
            head.cbeCode = config2.CIECode;
            head.cbeName = config2.CIEName;
            head.ecpCode = config2.CIECode;
            head.ecpName = config2.CIEName;
            head.orderNo = payment.BusinessOrderSysNo;
            head.charge = order.OrderAmount;
            head.goodsValue = order.ProductAmount;
            head.freight = order.FreightAmount;
            head.other = 0;
            head.tax = order.TaxFee;
            head.currency = "142";
            head.customer = srenity.Name;
            head.shipper = warehouseMod.Contact;
            head.shipperAddress = wareProvinceEntity.AreaName + " " + wareCityEntity.AreaName + " " + wareDistrictEntity.AreaName + " " + warehouseMod.StreetAddress;
            head.shipperTelephone = warehouseMod.Phone;
            head.consignee = srenity.Name;
            head.consigneeAddress = Province + " " + City + " " + District + " " + srenity.StreetAddress;
            head.consigneeTelephone = srenity.MobilePhoneNumber;
            head.idType="1";
            head.customerId = srenity.IDCardNo;
            head.accessType = "1";
            head.ieType = "I";
            head.modifyMark = "1";
            head.appTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            head.appStatus = "2";


        }
        void BindOrderItemListData(IList<CBSoOrderItem> datao, List<OrderList> itemList)
        { 
            foreach(var mod in datao)
            {
                OrderList item = new OrderList() 
                {
                    goodsNo = mod.ErpCode,
                    shelfGoodsName = mod.ProductName,
                    currency="142",
                    quantity = mod.Quantity.ToString(),
                    priceTotal = mod.SalesAmount,
                    unit = "035",
                    wasteMaterials="1",
                    brand="无",
                     itemNo="",
                };
                itemList.Add(item);
            }
        }

        public void OrderPaymentLogistic(OrderPaymentLogistics logistic, CBFnOnlinePayment payment,LgDeliveryType deliveryType)
        {
            logistic.logisticsName = deliveryType.DeliveryTypeName;
            logistic.paymentName = payment.PaymentName;
            logistic.paymentNo = payment.VoucherNo;
            logistic.paymentType = "A";
        }

        public override Model.Result GetOrderExpressno(string orderId)
        {
            throw new NotImplementedException();
        }

        public override Model.Result GetOrderTrade(string orderId)
        {
            throw new NotImplementedException();
        }

        public override Model.Result<string> GetLogisticsTracking(int orderSysNo)
        {
            throw new NotImplementedException();
        }

        public override Model.Result CancelOrderTrade(int orderSysNo, string reason = "")
        {
            throw new NotImplementedException();
        }
    }

    public class Order
    {
        public OrderHead OrderHead { get; set; }
        public List<OrderList> itemList { get; set; }
        public OrderPaymentLogistics OrderPaymentLogistics { get; set; }
    }

    /// <summary>
    /// 订单报文头部
    /// </summary>
    public class OrderHead
    {
        public string cbeCode { get; set; }
        public string cbeName { get; set; }
        public string ecpCode { get; set; }
        public string ecpName { get; set; }
        public string orderNo { get; set; }
        public decimal charge { get; set; }
        public decimal goodsValue { get; set; }
        public decimal freight { get; set; }
        public decimal other { get; set; }
        public string currency { get; set; }
        public decimal tax { get; set; }
        public string customer { get; set; }
        public string shipper { get; set; }
        public string shipperAddress { get; set; }
        public string shipperTelephone { get; set; }
        public string shipperCountry { get; set; }
        public string consignee { get; set; }
        public string consigneeAddress { get; set; }
        public string consigneeTelephone { get; set; }
        public string consigneeCountry { get; set; }
        public string idType { get; set; }
        public string customerId { get; set; }
        public string accessType { get; set; }
        public string ieType { get; set; }
        public string batchNumbers { get; set; }
        public string totalLogisticsNo { get; set; }
        public string tradeCountry { get; set; }
        public string agentCode { get; set; }
        public string agentName { get; set; }
        public string wrapType { get; set; }
        public string modifyMark { get; set; }
        public string note { get; set; }
        public string appTime { get; set; }
        public string appStatus { get; set; }
        public string appUid { get; set; }
        public string appUname { get; set; }
    }
    /// <summary>
    /// 订单列表
    /// </summary>
    public class OrderList 
    {
        public string itemNo { get; set; }
        public string goodsNo { get; set; }
        public string shelfGoodsName { get; set; }
        public string describe { get; set; }
        public string codeTs { get; set; }
        public string goodsName { get; set; }
        public string goodsModel { get; set; }
        public string taxCode { get; set; }
        public string price { get; set; }
        public string currency { get; set; }
        public string quantity { get; set; }
        public decimal priceTotal { get; set; }
        public string unit { get; set; }
        public string discount { get; set; }
        public string giftFlag { get; set; }
        public string country { get; set; }
        public string purposeCode { get; set; }
        public string wasteMaterials { get; set; }
        public string wrapType { get; set; }
        public string packNum{get;set;}
        public string barCode{get;set;}
        public string brand { get; set; }
        public string note { get; set; }
    }
    /// <summary>
    /// 订单支付/物流表
    /// </summary>
    public class OrderPaymentLogistics
    {
        public string paymentCode { get; set; }
        public string paymentName { get; set; }
        public string paymentType { get; set; }
        public string paymentNo { get; set; }
        public string logisticsCode { get; set; }
        public string logisticsName { get; set; }
        public string logisticsNo { get; set; }
        public string trackNo { get; set; }
    }
}
