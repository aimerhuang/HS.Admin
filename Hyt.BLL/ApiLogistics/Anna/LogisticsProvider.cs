using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.Logis.XinYi;
using Hyt.Model.Manual;
using Hyt.Model.WorkflowStatus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Hyt.BLL.Finance;
using Hyt.BLL.Order;
using Hyt.Util.Serialization;
using Hyt.Util;

namespace Hyt.BLL.ApiLogistics.Anna
{
    /// <summary>
    /// 心怡物流接口
    /// </summary>
    /// <remarks>2015-10-12 杨浩 创建</remarks>
    public class LogisticsProvider:ILogisticsProvider
    {
        const string OnNumber = "aoll";
        const string WhNumber = "STORE_46501";
        const string CopGNo = "HT-B74-000009";
        const string skucode = "HT-B74-000009-001";

        //const string testUrl = "http://183.63.175.210:7510/top-portal/rest.htm";
        //const string factoryUrl = "http://dsc.cangyibao.com:7510/top-portal/rest.htm";
        public LogisticsProvider(){ }
        /// <summary>
        /// 物流标示
        /// </summary>
        /// <remarks>2015-12-26 杨浩 创建</remarks>
        public override Hyt.Model.CommonEnum.物流代码 Code
        {
            get{return Hyt.Model.CommonEnum.物流代码.心怡;}           
        }
        #region 作废
        ///// <summary>
        ///// 推送订单
        ///// </summary>
        ///// <returns></returns>
        ///// <remarks>2015-10-12 杨浩 创建</remarks>
        //public override Result PushOrder(SoOrder order)
        //{
        //    Result result =new Result();
  
        //    SaleOrder saleOrder = new SaleOrder();
        //    saleOrder.OnNumber = OnNumber;
        //    saleOrder.WhNumber = WhNumber;
        //    saleOrder.SoNumber = "SO" + order.SysNo.ToString().PadLeft(8, '0');//order.SysNo.ToString();
        //    saleOrder.Date = order.CreateDate.ToString("yyyy-MM-dd HH:mm:ss");
        //    saleOrder.Remark = order.Remarks;
        //    saleOrder.Priority = 0;
        //    saleOrder.OrderName = order.ReceiveAddress.Name;
        //    saleOrder.OrderDocType = "01";
        //    //saleOrder.OrderDocId = order.ReceiveAddress.DocID;
        //    saleOrder.OrderPhone = string.IsNullOrEmpty(order.ReceiveAddress.MobilePhoneNumber) ? order.ReceiveAddress.PhoneNumber : order.ReceiveAddress.MobilePhoneNumber;
        //    saleOrder.IEFlag = "I";
        //    saleOrder.Freight = order.FreightAmount;
        //    //saleOrder.Tax = order.TaxFee;
        //    //saleOrder.ValuationFee = 22;
        //    saleOrder.RecipientName = order.ReceiveAddress.Name;
        //    saleOrder.RecipientCode = "142";
        //    saleOrder.ReceiverProvince = order.ReceiveAddress.AreaSysNo.ToString();
        //    saleOrder.RecipientDetailedAddress = order.ReceiveAddress.StreetAddress;
        //    saleOrder.RecipientPhone = string.IsNullOrEmpty(order.ReceiveAddress.MobilePhoneNumber) ? order.ReceiveAddress.PhoneNumber : order.ReceiveAddress.MobilePhoneNumber; ;
        //    saleOrder.ReceiverProvince = ((SoOrderMods)order).ReceiverProvince;
        //    saleOrder.ReceiverCity = ((SoOrderMods)order).ReceiverCity;
        //    saleOrder.ReceiverArea = ((SoOrderMods)order).ReceiverArea;
        //    //saleOrder.NoticeNo = "xdfd001";
        //    saleOrder.RecDocType = "01";
        //    //saleOrder.RecDocId = order.ReceiveAddress.DocID;
        //    saleOrder.ReceiverZipCode = order.ReceiveAddress.ZipCode;
        //    saleOrder.ReceiverPhone = order.ReceiveAddress.MobilePhoneNumber;
        //    ///付款信息
        //    saleOrder.payTransactionNo = ((SoOrderMods)order).ReceiptVoucherItemList[0].VoucherNo;
        //    saleOrder.payAmount = ((SoOrderMods)order).ReceiptVoucherItemList[0].Amount;
        //    saleOrder.payGoodsAmount = ((SoOrderMods)order).ReceiptVoucherItemList[0].Amount;
        //    saleOrder.payTimeStr = ((SoOrderMods)order).ReceiptVoucherItemList[0].CreatedDate.ToString("yyyy-MM-dd HH:mm:ss");
        //    //saleOrder.PayEntNo = "1234";
        //    //saleOrder.HZpurchaserId = "123";
        //    //saleOrder.payEnterpriseName = "张001";
        //    //saleOrder.bank = "中国银行";
        //    //saleOrder.payAccount = "1";
        //    //saleOrder.payerName = "李四";
        //    //saleOrder.payerDocumentType = "1";
        //    //saleOrder.payerDocumentNumber = "支付人证件号码0";
        //    //saleOrder.payerPhoneNumber = "20510000";
        //    //saleOrder.assoBankAccount = "123321";
        //    //saleOrder.orderSourceCode = "123456";
        //    //saleOrder.ReceivingParty = "收货方0";
        //    //saleOrder.Consignor = "测试发货人";
        //    //saleOrder.DeliveryAddress = "测试收货地址";

        //    System.Collections.Generic.List<Model.Logis.XinYi.SaleOrderDetail> Details = new System.Collections.Generic.List<Model.Logis.XinYi.SaleOrderDetail>();
        //    for (int i = 0; i < order.OrderItemList.Count; i++)
        //    {
        //        Model.Logis.XinYi.SaleOrderDetail soDetail = new Model.Logis.XinYi.SaleOrderDetail();
        //        soDetail.RoNumber = saleOrder.SoNumber;
        //        soDetail.SoNumber = saleOrder.SoNumber;
        //        soDetail.CopGNo = order.OrderItemList[i].ProductSysNo.ToString();
        //        soDetail.RowNum = i + 1;
        //        soDetail.SkuCode = order.OrderItemList[i].ProductSysNo.ToString();
        //        //soDetail.ItSkuColor = "蓝色";
        //        //soDetail.ItSkuSize = "M";
        //        soDetail.Qty = order.OrderItemList[i].Quantity;
        //        soDetail.ProPrice = (int)(order.OrderItemList[i].SalesUnitPrice * 100);
        //        Details.Add(soDetail);
        //    }
        //    System.Collections.Generic.List<SaleOrder> saleOrders = new System.Collections.Generic.List<SaleOrder>();
        //    saleOrders.Add(saleOrder);

        //    var jsonData = new { saleOrder = saleOrders, SaleOrderDetail = Details };

        //    //设置过滤值为null的字段
        //    var jSetting = new Newtonsoft.Json.JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
        //    string json = JsonConvert.SerializeObject(jsonData, Formatting.None, jSetting);

        //    using (var service = new Hyt.Infrastructure.Communication.ServiceProxy<Hyt.Service.Contract.LogisApp.ILogistics>())
        //    {
        //        //result = service.Channel.PushSoOrder(json);
            
        //    }
        //    return result;
        //}
   
        ///// <summary>
        ///// 推送商品
        ///// </summary>
        ///// <param name="product"></param>
        ///// <returns></returns>
        //public override Result PushProduct(PdProduct product)
        //{
        //    Result result = new Result();
        //    Goods goods = new Goods();
        //    goods.WarehouseCode = WhNumber;
        //    goods.OnNumber = OnNumber;
        //    goods.GNo = product.SysNo.ToString();
        //    goods.CopGNo = product.SysNo.ToString();
        //    goods.GName = product.ProductName;
        //    goods.GModel = product.Volume;
        //    goods.BARCode = product.Barcode;
        //    goods.Notes = product.ProductSummary;
        //    goods.Unit = "007";
        //    goods.GoodsMes = product.ProductDeclare;
        //   // goods.OpType = product.IsPushLogistics == 0 ? "新增" : "修改";
        //    goods.ShortName = product.ProductShortTitle;
        //    //goods.Manufactory = product.Manufactory;
        //    goods.Brand = ((CBPdProduct)product).BrandName;
        //    goods.Original = "110";//((CBPdProduct)product).Original;
        //   // goods.PurchasePlace = ((CBPdProduct)product).Original;
        //    goods.GrossWt = product.GrosWeight;
        //    goods.NetWt = product.NetWeight;
        //    goods.CodeTS = product.ErpCode ;
        //    goods.DecPrice = (int)((CBPdProduct)product).PdPrice.Value.First(p => p.PriceSource == 0).Price * 100;
        //    goods.IEFlag = "I";
        //    goods.GNote = product.PackageDesc;
        //    List<Goods> goodsList = new List<Goods>();
        //    goodsList.Add(goods);
        //    var jsonData = new { Goods = goodsList };
        //    var jSetting = new Newtonsoft.Json.JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
        //    string json = JsonConvert.SerializeObject(jsonData, Formatting.None, jSetting);

        //    using (var service = new Hyt.Infrastructure.Communication.ServiceProxy<Hyt.Service.Contract.LogisApp.ILogistics>())
        //    {
        //       // result = service.Channel.PushProduct(json);
               
        //    }

        //    return result;
        //}
        ///// <summary>
        ///// 推送订单
        ///// </summary>
        ///// <param name="stockIn"></param>
        ///// <param name="itemList"></param>
        ///// <returns></returns>
        //public override Result PushInOrder(PdProductStockIn stockIn, List<PdProductStockInDetailList> itemList)
        //{
        //    Result result = new Result();
        //    OrderHead headData = new OrderHead();
        //    headData.OnNumber = OnNumber;
        //    headData.WhNumber = WhNumber;
        //    headData.RoNumber = stockIn.SysNo.ToString();
        //    headData.OrderDate = stockIn.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss");
            
        //    List<OrderDetail> detailList = new List<OrderDetail>();
        //    int indx=0;
        //    foreach (PdProductStockInDetailList item in itemList)
        //    {
        //        indx++;
        //        detailList.Add(new OrderDetail() {
        //            RowNum = indx,
        //            RoNumber = stockIn.SysNo.ToString(),
        //            CopGNo = item.PdProductSysNo.ToString(),
        //            GoodsBatchNo = stockIn.StockInNo,
        //            SkuCode = item.PdProductSysNo.ToString(),
        //            Qty = (int)(item.StorageQuantity),
        //           // ProPrice = (int)(item.Price*100)
        //        });
        //    }
        //    List<OrderHead> headDataList = new List<OrderHead>();
        //    headDataList.Add(headData);
        //    var jsonData = new { OrderHead = headDataList, OrderDetail = detailList };
        //    var jSetting = new Newtonsoft.Json.JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
        //    string json = JsonConvert.SerializeObject(jsonData, Formatting.None, jSetting);

        //    using (var service = new Hyt.Infrastructure.Communication.ServiceProxy<Hyt.Service.Contract.LogisApp.ILogistics>())
        //    {
        //       // result = service.Channel.PushInOrder(json);
                
        //    }

        //    return result;
        //}
        #endregion

        public override Result GetProduct(string productId)
        {
            throw new NotImplementedException();
        }

        public override Result AddOrderTrade(int orderSysno)
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
                    if(tempitem!=null)
                    {
                        tempitem.OrginCountry = Hyt.BLL.Basic.OriginBo.Instance.GetEntity(mod.OriginSysNo).Origin_Name;
                    }
                }
                

                OrderShip orderShip = new OrderShip();
                orderShip.storeCode = config.WhNumber;
                orderShip.orderCode = payment.BusinessOrderSysNo;//order.OrderNo;
                orderShip.orderType = "TRANS";
                orderShip.createTime = order.CreateDate.ToString("yyy-MM-dd hh:mm:ss");
                orderShip.checker = order.AuditorSysNo.ToString();
                orderShip.checkTime = order.AuditorDate.ToString("yyy-MM-dd hh:mm:ss");
                orderShip.payTime = payment.CreatedDate.ToString("yyy-MM-dd hh:mm:ss");
                orderShip.sourcePlatformCode = "ZY";
                orderShip.sourceOrderCode = order.SysNo.ToString();
                orderShip.receiver = new Receiver()
                {
                    receiverAddress = srenity.StreetAddress.Replace("\n", "").Replace("\r", ""),
                    receiverProvince = Province.Replace("\n", "").Replace("\r", ""),
                    receiverCity = City.Replace("\n","").Replace("\r",""),
                    receiverDistrict = District.Replace("\n", "").Replace("\r", ""),
                    receiverCountry = "中国",
                    receiverIdNumber = buyer_idcard,
                    receiverName = srenity.Name,
                    receiverMobile = srenity.MobilePhoneNumber,
                    receiverPhone = srenity.PhoneNumber
                };
                orderShip.invoiceFlag = "N";
                orderShip.codFlag = "N";
                orderShip.gotAmount = order.OrderAmount;
                orderShip.arAmount = order.OrderAmount;
                orderShip.insuranceFlag = "N";
                orderShip.orderTotalAmount = order.ProductAmount + order.TaxFee + order.FreightAmount + order.ProductChangeAmount;
                orderShip.orderActualAmount = order.OrderAmount;
                orderShip.totalAmount = order.OrderAmount;
                orderShip.orderGoodsAmount = order.ProductAmount + order.ProductChangeAmount;
                orderShip.feeAmount = order.FreightAmount;
                orderShip.orderTaxAmount = order.TaxFee;
                orderShip.payEntNo = payment.CusPaymentCode;
                orderShip.payEnterpriseName = payment.PaymentName;//"支付宝（中国）网络技术有限公司";
                orderShip.recipientProvincesCode = "440000";

                var deliverTypeInfo=BLL.Logistics.DeliveryTypeBo.Instance.GetDeliveryType(order.DeliveryTypeSysNo);
                if(deliverTypeInfo!=null&&string.IsNullOrWhiteSpace(deliverTypeInfo.OverseaCarrier)|| deliverTypeInfo==null)
                        orderShip.courierCode = config.CourierCode;
                else
                    orderShip.courierCode = deliverTypeInfo.OverseaCarrier;

                string importType = "";
                if (warehouseMod.WarehouseType == 30)
                {
                    importType = "BBC";
                }
                else if (warehouseMod.WarehouseType == 40)
                {
                    importType = "BC";
                }
                orderShip.buyerId = customer.Account;
                orderShip.importType = importType;
                orderShip.portload = "142";
                orderShip.items = new List<Items>();
                int rowIndex = 0;
                foreach (var item in datao)
                {
                    rowIndex++;

                    item.SalesUnitPrice = ((item.SalesAmount + item.ChangeAmount) / item.Quantity);
                    orderShip.items.Add(new Items()
                    {
                        actualPrice = item.SalesUnitPrice,
                        barCode = item.BarCode,
                        discountAmount = item.DiscountAmount,
                        inventoryType = "ZP",
                        itemCode = item.ErpCode,
                        itemName = item.ProductName.Replace("+",""),
                        orderLineNo = rowIndex.ToString(),
                        originCountry = item.OrginCountry,
                        quantity = item.Quantity,
                        retailPrice = item.OriginalPrice,
                        subOrderCode = item.SysNo.ToString(),
                        supplierCode = config.supplierCode,
                        supplierName = config.supplierName,
                        weight=Convert.ToInt32((item.NetWeight*1000))
                    });
                }
                orderShip.sender = new Sender()
                {
                    senderAddress = warehouseMod.StreetAddress.Trim(),
                    senderProvince = wareProvinceEntity.AreaName.Trim(),
                    senderCity = wareCityEntity.AreaName.Trim(),
                    senderDistrict = wareDistrictEntity.AreaName.Trim(),
                    senderCountry = "中国",
                    senderName = config.SenderUser.Trim(),
                    senderMobile = warehouseMod.Phone,
                    senderPhone = warehouseMod.Phone,
                    senderIdNumbe = config.SenderIdNumber
                };

                Dictionary<string, string> dicKeyList = new Dictionary<string, string>();
                dicKeyList.Add("notifyId", DateTime.Now.ToString("yyMMddHHmmssffff"));
                dicKeyList.Add("notifyTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                dicKeyList.Add("format", "json");
                dicKeyList.Add("signMethod", "md5");
                dicKeyList.Add("busiType", "alog.ordership.notify");
                dicKeyList.Add("partnerCode", config.OnNumber);
                dicKeyList.Add("ownerCode", config.OnNumber);
                dicKeyList.Add("content", JsonUtil.ToJson(orderShip));//
                dicKeyList["content"] = dicKeyList["content"].Replace("\"courierCode\":null,", "");
                Dictionary<string, string> dic1Asc1
                 = (from d in dicKeyList
                    orderby d.Key ascending
                    select d).ToDictionary(k => k.Key, v => v.Value);
                string strParams = "";
                foreach (string key in dic1Asc1.Keys)
                {
                    if (!string.IsNullOrEmpty(strParams))
                    {
                        strParams += "&";
                    }
                    strParams += key + "=" + dic1Asc1[key];
                }
                string sign = Hyt.Util.Security.UserMd5(config.secretKey + strParams.Replace("&", "").Replace("=", "") + config.secretKey).ToUpper();
                strParams = "sign=" + sign + "&" + strParams;
                string testData = MyHttp.GetResponse(config.URLPath, strParams, "utf-8");
                //{"success":true,"errCode":"","errMsg":"","courierCode":"SF","mailNo":"444746031132"}
                PosDataResult postResult = JsonUtil.ToObject<PosDataResult>(testData);
                if (postResult.success)
                {
                    Hyt.BLL.CrossBorderLogistics.CrossBorderLogisticsOrderBo.Instance.InsertEntity(
                        new CrossBorderLogisticsOrder()
                        {
                            ExpressNo = postResult.courierCode + "_" + postResult.mailNo,
                            LogisticsCode = 0,
                            LogisticsOrderId = strParams,
                            SoOrderSysNo = orderSysno,
                        }
                    );
                    string express = postResult.courierCode + "_" + postResult.mailNo;
                    BLL.Order.SoOrderBo.Instance.UpdateOrderApiStatus((int)Hyt.Model.WorkflowStatus.OrderStatus.跨境物流推送状态.成功, 3, order.SysNo);
                    result.Status = postResult.success;
                }
                else
                {
                    result.Status = postResult.success;
                    result.Message = testData;
                }
            }catch(Exception e)
            {
                result.Status = false;
                result.Message = e.Message;
            }
            return result;
        }

        public override Result GetOrderExpressno(string orderId)
        {
            CrossBorderLogisticsOrder mod = Hyt.BLL.CrossBorderLogistics.CrossBorderLogisticsOrderBo.Instance.GetEntityByOrderSysNo(Convert.ToInt32(orderId));
            if(mod!=null)
            {
                return new Result()
                {
                    Status = true,
                    Message = mod.ExpressNo
                };
            }
            else
            {
                return new Result()
                {
                    Status = false,
                    Message = "未找到当前推送的物流信息"
                };
            }
           
        }

        public override Result GetOrderTrade(string orderId)
        {
            throw new NotImplementedException();
        }

        public override Result<string> GetLogisticsTracking(int orderSysNo)
        {
            throw new NotImplementedException();
        }

        public override Result CancelOrderTrade(int orderSysNo,string reason="")
        {
            Result result = new Result();
            try
            {
                var order = BLL.Order.SoOrderBo.Instance.GetEntity(orderSysNo);
                var config = Hyt.BLL.Config.Config.Instance.GetAnnaConfig();

                CBFnOnlinePayment payment = FinanceBo.Instance.GetOnPaymentBySourceSysNo(order.SysNo);

                CancelOrder cancelOreder = new CancelOrder();
                cancelOreder.storeCode = config.WhNumber;
                cancelOreder.orderType = "TRANS";
                cancelOreder.orderCode = payment.BusinessOrderSysNo;
                cancelOreder.reason = reason;


                Dictionary<string, string> dicKeyList = new Dictionary<string, string>();
                dicKeyList.Add("notifyId", DateTime.Now.ToString("yyMMddHHmmssffff"));
                dicKeyList.Add("notifyTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                dicKeyList.Add("format", "json");
                dicKeyList.Add("signMethod", "md5");
                dicKeyList.Add("busiType", "alog.order.cancel");
                dicKeyList.Add("partnerCode", config.OnNumber);
                dicKeyList.Add("ownerCode", config.OnNumber);
                dicKeyList.Add("content", JsonUtil.ToJson(cancelOreder));//
                dicKeyList["content"] = dicKeyList["content"].Replace("\"courierCode\":null,", "");
                Dictionary<string, string> dic1Asc1
                 = (from d in dicKeyList
                    orderby d.Key ascending
                    select d).ToDictionary(k => k.Key, v => v.Value);
                string strParams = "";
                foreach (string key in dic1Asc1.Keys)
                {
                    if (!string.IsNullOrEmpty(strParams))
                    {
                        strParams += "&";
                    }
                    strParams += key + "=" + dic1Asc1[key];
                }
                string sign = Hyt.Util.Security.UserMd5(config.secretKey + strParams.Replace("&", "").Replace("=", "") + config.secretKey).ToUpper();
                strParams = "sign=" + sign + "&" + strParams;
                string testData = MyHttp.GetResponse(config.URLPath, strParams, "utf-8");
                PosDataResult postResult = JsonUtil.ToObject<PosDataResult>(testData);
                if (postResult.success)
                {
                    string msg = "";
                        //SoOrderBo.Instance.CancelSoOrder(orderSysNo, BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo, OrderStatus.销售单作废人类型.后台用户,
                        //ref msg, reason);
                    result.Status = postResult.success;
                    result.Message = "取消成功";
                }
                else
                {
                    result.Status = postResult.success;
                    result.Message = testData;
                }
            }
            catch (Exception e)
            {
                result.Status = false;
                result.Message = e.Message;
            }
            return result;
        }
    }

    #region 数据推送实体类
    class CancelOrder
    {
        public string storeCode { get; set; }
        public string orderType { get; set; }
        public string orderCode { get; set; }
        public string reason { get; set; }
    }
    class PosDataResult { 
        //{"success":true,"errCode":"","errMsg":"","courierCode":"SF","mailNo":"444746031132"}
        public bool success { get; set; }
        public string errCode { get; set; }
        public string courierCode { get; set; }
        public string mailNo { get; set; }
    }
    class OrderShip
    {
        public string storeCode { get; set; }
        public string orderCode { get; set; }
        public string orderType { get; set; }
        public string createTime { get; set; }
        public string checker { get; set; }
        public string checkTime { get; set; }
        public string payTime { get; set; }
        public string sourcePlatformCode { get; set; }
        public string sourceOrderCode { get; set; }
        public string shopCode { get; set; }
        public string shopName { get; set; }
        public string buyerNick { get; set; }
        public string courierCode { get; set; }
        public Receiver receiver { get; set; }
        public string invoiceFlag { get; set; }
        //public List<Invoice> invoices { get; set; }
        public string codFlag { get; set; }
        public decimal gotAmount { get; set; }
        public decimal arAmount { get; set; }
       // public DeliverRequirement deliverRequirement { get; set; }
        public string isUrgency { get; set; }
        public string insuranceFlag { get; set; }
        //public Insurance insurance { get; set; }
        public string buyerMessage { get; set; }
        public string sellerMessage { get; set; }
        public string remark { get; set; }
        public List<Items> items { get; set; }
        public Sender sender { get; set; }
        public decimal orderTotalAmount { get; set; }
        /// <summary>
        /// 应收总额
        /// </summary>
        public decimal orderActualAmount { get; set; }
        public decimal totalAmount { get; set; }
        public decimal orderGoodsAmount { get; set; }
        public decimal feeAmount { get; set; }
        public decimal orderTaxAmount { get; set; }
        public string payEntNo { get; set; }
        public string payEnterpriseName { get; set; }
        public string buyerId { get; set; }
        public string recipientProvincesCode { get; set;}
        public string importType { get; set; }
        public string portload { get; set; }
    }
    class Sender
    {
        public string senderName { get; set; }
        public string senderMobile { get; set; }
        public string senderPhone { get; set; }
        public string senderAddress { get; set; }
        public string senderProvince { get; set; }
        public string senderCity { get; set; }
        public string senderDistrict { get; set; }
        public string senderTown { get; set; }
        public string senderZip { get; set; }
        public string senderCountry { get; set; }
        public string senderIdNumbe{get;set;}
    }
    class Items
    {
        public string orderLineNo { get; set; }
        public string subOrderCode { get; set; }
        public string itemCode { get; set; }
        public string barCode { get; set; }
        public string itemName { get; set; }
        public string unitName { get; set; }
        public string skuProperty { get; set; }
        public string inventoryType { get; set; }
        public long quantity { get; set; }
        public decimal retailPrice { get; set; }
        public decimal actualPrice { get; set; }
        public decimal discountAmount { get; set; }
        public string originCountry { get; set; }
        public string supplierCode { get; set; }
        public string supplierName { get; set; }
        public long weight { get; set; }
    }
    class DeliverRequirement {
        public string scheduleType { get; set; }
        public DateTime scheduleDay { get; set; }
        public string scheduleStartTime { get; set; }
        public string scheduleEndTime { get; set; }
        public string deliveryType { get; set; }
    }
    class Insurance
    {
        public string type { get; set; }
        public decimal amount { get; set; }
    }
    class Receiver
    {
        public string receiverName { get; set; }
        public string receiverMobile { get; set; }
        public string receiverPhone { get; set; }
        public string receiverAddress { get; set; }
        public string receiverProvince { get; set; }
        public string receiverCity { get; set; }
        public string receiverDistrict { get; set; }
        public string receiverTown { get; set; }
        public string receiverZip { get; set; }
        public string receiverIdNumber { get; set; }
        public string receiverCountry { get; set; }
    }
    class Invoice
    {
        public string invoiceType { get; set; }
        public string invoiceTitle { get; set; }
        public string invoiceAmount { get; set; }
        public InvoiceItem invoiceItems { get; set; }
    }
    class InvoiceItem
    {
        public string itemName { get; set; }
        public string unit { get; set; }
        public decimal price { get; set; }
        public long quantity { get; set; }
        public decimal amount { get; set; }
    }
    #endregion
}
