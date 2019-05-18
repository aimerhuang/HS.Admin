using Hyt.BLL.Log;
using Hyt.BLL.Order;
using Hyt.BLL.OrderRule;
using Hyt.Model.WorkflowStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ApiResult = Hyt.Model.ApiResult;
using Hyt.Model;



using System.Transactions;
using Hyt.BLL.Authentication;
using Hyt.BLL.Basic;
using Hyt.BLL.MallSeller;
using Hyt.BLL.Sys;
using Hyt.Infrastructure.Pager;

using Hyt.Admin.Models;
using Hyt.Model.Parameter;
using Hyt.Util;
using Hyt.BLL.CRM;
using Hyt.Model.Common;
using Hyt.Model.SystemPredefined;
using Hyt.BLL.Cart;
using System.Threading.Tasks;

using System.Text;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;
using Hyt.BLL.Finance;
using Hyt.DataAccess.Order;
using Extra.UpGrade.Provider;
using Extra.UpGrade.Model;
using Hyt.Model.UpGrade;


namespace Hyt.Admin.Controllers
{
    /// <summary>
    /// 提供给外部程序使用的API，稍后再将API实现改为服务，现在移涉及的类太多
    /// </summary>
    [CustomActionFilter(false)]
    public class ApiController : BaseController
    {
        //MD5加密
        private string MD5Encrypt(string strText)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(strText));

            string outString = "";
            for (int i = 0; i < result.Length; i++)
            {
                outString += result[i].ToString("x2");
            }

            return outString;

        }
        /// <summary>
        /// 海拍客订单回执
        /// </summary>
        /// <returns></returns>
        /// <remarks>2017-10-16 杨浩 创建</remarks>
        public ContentResult HipacOrderReceipt()
        {

            var __result=new Result(){Status=false};

            //return Content("");
            var param=new  OrderParameters();
            var auth = new AuthorizationParameters()
            {
                MallType = (int)DistributionStatus.商城类型预定义.海拍客,
            };

            string sendId = DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(100000,999999).ToString();
            string sign = "";
            string appKey="";
            var sr = new StreamReader(Request.InputStream);
            string responseStr = sr.ReadToEnd();
            param.Xml = responseStr;

            BLL.Log.LocalLogBo.Instance.Write(param.Xml,"HipacOrderReceiptLog");
         
            int dealerMallSysNo = 25;
            var entity = new DsDealerLog();
            entity.MallTypeSysNo = auth.MallType;
            entity.CreatedDate = DateTime.Now;
            entity.CreatedBy = 0;
            entity.LastUpdateBy = 0;
            entity.LastUpdateDate = DateTime.Now;
            entity.MallSysNo=dealerMallSysNo;
            entity.MallTypeSysNo=auth.MallType;
            entity.LogContent = param.Xml; 
    
            string retCode = "FAIL";         
            string retMsg = "失败";
            try
            {
                var instance = UpGradeProvider.GetInstance(auth.MallType);
                var result = instance.GetUpGradedWaitSend(param, auth);
                if (result.Status)
                {
                    var mallInfo = BLL.Distribution.DsDealerMallBo.Instance.GetEntity(dealerMallSysNo);
                    var dic = result.Data.First().UpGradeOrderItems.ToDictionary(x => x.MallProductCode);
                    var erpCodeList = dic.Keys.ToList();
                    var productList = BLL.Product.PdProductBo.Instance.GetProductListByErpCode(erpCodeList);
                    var appInfo = BLL.Distribution.DsDealerAppBo.Instance.GetDsDealerApp(mallInfo.DealerAppSysNo);

                    var upGradeOrder = result.Data.First();
                    upGradeOrder.HytOrderDealer.DealerMallSysNo = dealerMallSysNo;
                    upGradeOrder.HytOrderDealer.DealerSysNo = mallInfo.DealerSysNo;
                    upGradeOrder.HytOrderDealer.IsSelfSupport = mallInfo.IsSelfSupport;
                    upGradeOrder.HytOrderDealer.IsPreDeposit =0;
                    var orderInfo = result.Data.First();
                    entity.MallOrderId = orderInfo.MallOrderBuyer.MallOrderId;

                    appKey = appInfo.AppKey;
                    string signStr = "appKey=" + appInfo.AppKey + "&sendID=" + sendId + "&key=" + appInfo.AppSecret;
                    sign = MD5Encrypt(signStr).ToUpper();
                    var _result = BLL.Order.SoOrderBo.Instance.ImportMallOrder(new List<UpGradeOrder>() { upGradeOrder }, productList, mallInfo.DefaultWarehouse);                   
                    if (!_result.Status)
                    {                      
                        __result.Status = false;
                        __result.Message=_result.Message;                                       
                        //sendID,appKey,orderNum,service,custName,payNo                                                           
                    }
                    else
                    {
                        __result.Status=true;
                        retCode = "SUCCESS";
                        retMsg = "成功";
                    }
                }
                else
                {
                    __result.Status = false;
                    __result.Message= result.Message; 
                    entity.MallOrderId = result.Data.First().MallOrderBuyer.MallOrderId;                                     
                }
            }
            catch(Exception ex)
            {
                __result.Status=false;
                __result.Message=ex.Message;                                   
            }


            if(!__result.Status)
            {
                entity.Message =__result.Message;
                entity.Status = 10;   
                if(!BLL.MallSeller.DsDealerLogBo.Instance.ChekMallOrderId(entity.MallOrderId,10,entity.MallSysNo)||
                    string.IsNullOrWhiteSpace(entity.MallOrderId))               
                   BLL.MallSeller.DsDealerLogBo.Instance.Insert(entity);                          
            }
                

            string content = "<?xml version=\"1.0\" encoding=\"UTF-8\"?> ";
                   content +="<HipacPush>";
                   content +="<Head>";
                   content +="  <version>1.0</version>";
                   content +="  <service>pushOrderInfo</service>";
                   content += "  <sendID>" + sendId + "</sendID>";
                   content += "  <appKey>" + appKey + "</appKey> ";
                   content += " <retCode>" + retCode + "</retCode> ";
                   content += "  <sign>" + sign + "</sign>  ";    
                   content +="</Head>";
                   content +="<Body>";
                   content += "   <bizCode>" + retCode + "</bizCode>";
                   content += "   <retMsg>" + retMsg + "</retMsg>";
                   content +=" </Body>  ";
                   content +="</HipacPush>";

             return Content(content);
        }
        /// <summary>
        /// 易扫购订单异步接口
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-5-19 杨浩 创建</remarks>
        public ContentResult Order()
        {           
            var result = new Result();
            var systemManageUser = BLL.Sys.SyUserBo.Instance.GetSyUser(1);
            StreamReader sr = new StreamReader(Request.InputStream);
            string responseStr = sr.ReadToEnd();

            BLL.Log.LocalLogBo.Instance.Write(responseStr, "ApiController_OrderLog");

            var model = JObject.Parse(responseStr);       
            try
            {               
                /*
                 {
                    "hmac":"32f8b1f0d629cedd5ea231ac0be1de81",
                    "merchantId":"120140419",
                    "orderAmount":110,
                    "productDetails":
                    [
                        {"goodsAmount":225000,
                        "goodsCode":"600008532",
                        "goodsCount":1,
                        "goodsId":"a3399ff22e1a435ba6c554bce50ea7ca",
                        "goodsName":"全新天梭手表 圆盘黑底圈钢带"}],
                        "requestId":"146345778638539428",
                        "serialNumber":"8a0be1f254ba6dc60154bce1af6f001f"
                  }
                */
                //Grand.Wap.Models.MallEhkingMdl model = System.Web.Helpers.Json.Decode<Grand.Wap.Models.MallEhkingMdl>(jsonResult);

                string merchantId = model["merchantId"].ToString();

                var order = SoOrderBo.Instance.GetOrderByOrderNo(model["requestId"].ToString());
                if(order==null)
                    return Content("FAIL");

                var dealerPayTypeInfo=BLL.Distribution.DsDealerPayTypeBo.Instance.GetDealerPayTypeByMerchantId(merchantId);
                if (dealerPayTypeInfo == null)                
                    return Content("FAIL");               

                var wareHouseInfo=BLL.Distribution.DsDealerWharehouseBo.Instance.GetByDsUserSysNo(dealerPayTypeInfo.DealerSysNo);
                if(wareHouseInfo==null)
                    return Content("FAIL"); 

                var dealerInfo=BLL.Distribution.DsDealerBo.Instance.GetDsDealer(dealerPayTypeInfo.DealerSysNo);
                if(dealerInfo==null)
                    return Content("FAIL"); 

                if (model.Property("address") == null && model.Property("payer") == null)//易扫购下单异步通知
                {
                    #region 添加易宝异步传过来的订单
                    var existOrder = SoOrderBo.Instance.GetOrderByOrderNo(model["requestId"].ToString());

                    if (existOrder != null && existOrder.SysNo > 0)
                    {
                        return Content("SUCCESS");
                    }
                 
                    ParaShopOrderCreateFilter shopOrderCreateFilter = new ParaShopOrderCreateFilter();

                    shopOrderCreateFilter.orderItem = new List<SoOrderItem>();

                    var productDetails = model["productDetails"];
                    foreach (var item in productDetails)
                    {
                        var orderItem = new SoOrderItem();

                        var product = BLL.Product.PdProductBo.Instance.GetProductByErpCode(item["goodsCode"].ToString());

                        if (product == null || product.SysNo <= 0)
                        {
                            throw new Exception("不存在的商品编码！");
                        }

                        var productPrice = BLL.Product.PdPriceBo.Instance.GetProductPrice(product.SysNo, new ProductStatus.产品价格来源[] { ProductStatus.产品价格来源.会员等级价 }, 0).FirstOrDefault();

                        orderItem.SalesAmount = decimal.Parse(item["goodsAmount"].ToString()) / 100;
                        orderItem.SalesUnitPrice = productPrice.Price;
                        orderItem.OriginalPrice = orderItem.SalesUnitPrice * decimal.Parse(item["goodsCount"].ToString());                    

                        orderItem.ProductSysNo = product.SysNo;

                        orderItem.Quantity = int.Parse(item["goodsCount"].ToString());

                        shopOrderCreateFilter.orderItem.Add(orderItem);
                    }

       
                    shopOrderCreateFilter.shopSysNo = wareHouseInfo.WarehouseSysNo;
                    shopOrderCreateFilter.PayType =PaymentType.易宝支付;
                    shopOrderCreateFilter.customerSysNo=1;//系统定义的客户编号
                    shopOrderCreateFilter.receiveMobilePhoneNumber = dealerInfo.PhoneNumber;
                    shopOrderCreateFilter.receiveName = dealerInfo.UserName;
                   
                    using (var tran = new TransactionScope())
                    {
                        var t = SoOrderBo.Instance.CreateShopOrderFromOuter(shopOrderCreateFilter.shopSysNo,
                                                           shopOrderCreateFilter.customerSysNo,
                                                           shopOrderCreateFilter.receiveName,
                                                           shopOrderCreateFilter.receiveMobilePhoneNumber,
                                                           shopOrderCreateFilter.internalRemarks,
                                                           shopOrderCreateFilter.orderItem, null,
                                                           shopOrderCreateFilter.PayType, systemManageUser,
                                                           shopOrderCreateFilter.VoucherNo, shopOrderCreateFilter.EasReceiptCode, shopOrderCreateFilter.CoinPay, shopOrderCreateFilter.CouponCode, null);


                        //写系统日志
                        var ip = Hyt.Util.WebUtil.GetUserIp();
                        Hyt.BLL.Log.SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "门店订单-创建销售单",
                                                 LogStatus.系统日志目标类型.订单, t.Item2, null, ip,
                                                 0);
                        tran.Complete();
                    }
                    #endregion

                    #region 废弃

                    //testString += "开始处理易扫购下单\n";
                    //SoOrder order = new SoOrder();
                    //order.OrderNo = model["requestId"].ToString();
                    //order.OrderInvoice = null;
                    //order.CustomerSysNo = 5;
                    //order.DefaultWarehouseSysNo = 23;
                    //order.DeliveryTypeSysNo = 7;
                    //order.PayTypeSysNo = 12;
                    //order.OrderSource = 20;
                    //order.OrderSourceSysNo = 0;
                    //order.SalesType = 10;
                    //order.SalesSysNo = 0;
                    //order.IsHiddenToCustomer = 0;
                    //order.CustomerMessage = "";
                    //order.InternalRemarks = "";
                    //order.DeliveryRemarks = "";
                    //order.DeliveryTime = "周一至周五送货";
                    //order.ContactBeforeDelivery = 1;
                    //order.Remarks = "";
                    //order.OrderAmount = 0;
                    //order.FreightAmount = 0;
                    //order.DealerSysNo = 0;

                    //testString += "下单简单信息收集完成\n";
                    //order.ReceiveAddress = new SoReceiveAddress();
                    //order.ReceiveAddress = DataAccess.Order.ISoReceiveAddressDao.Instance.GetOrderReceiveAddress(18);
                    //testString += "下单收货信息收集完成\n";
                    //order.OrderItemList = new List<SoOrderItem>();

                    ////var productDetails = model["productDetails"];
                    //foreach (var item in productDetails)
                    //{
                    //    SoOrderItem orderItem = new SoOrderItem();

                    //    PdProduct product = BLL.Product.PdProductBo.Instance.GetProductByErpCode(item["goodsCode"].ToString());

                    //    if (product == null || product.SysNo <= 0)
                    //    {
                    //        throw new Exception("不存在的商品编码！");
                    //    }

                    //    CBPdPrice productPrice = BLL.Product.PdPriceBo.Instance.GetProductPrice(product.SysNo, new ProductStatus.产品价格来源[] { ProductStatus.产品价格来源.会员等级价 }, 0).FirstOrDefault();

                    //    order.OrderAmount += productPrice.Price * decimal.Parse(item["goodsCount"].ToString());

                    //    //if (order.OrderItemList.Count == 0)
                    //    //{
                    //    //    orderItem.ChangeAmount = (model.orderAmount - model.productDetails.Sum(k => k.goodsAmount)) / 100;
                    //    //}

                    //    orderItem.ProductSysNo = product.SysNo;

                    //    orderItem.Quantity = int.Parse(item["goodsCount"].ToString());

                    //    order.OrderItemList.Add(orderItem);
                    //}

                    //if (order.OrderItemList.Count > 0)
                    //{
                    //    order.OrderItemList[order.OrderItemList.Count - 1].ChangeAmount = Convert.ToDecimal(model["orderAmount"].ToString()) / 100 - order.OrderAmount;
                    //}

                    //testString += "下单商品信息收集完成\n";

                    //finallyResult = BLL.Order.SoOrderBo.Instance.CreateSoOrder(order, order.ReceiveAddress, order.OrderItemList.ToArray(), order.OrderInvoice, systemManageUser);

                    //if (finallyResult.Status)
                    //{
                    //    var soOrder = BLL.Order.SoOrderBo.Instance.GetEntity(order.SysNo);
                    //    soOrder.OrderNo = model["requestId"].ToString();
                    //    finallyResult.Status = BLL.Order.SoOrderBo.Instance.UpdateOrder(soOrder);

                    //    if (!finallyResult.Status)
                    //    {
                    //        finallyResult.Message = "订单号回写失败！";
                    //    }
                    //}
                    #endregion               
                }
                else if (model.Property("address") == null)//易扫购支付异步通知
                {                                                  
                    if (order.PayStatus == 20)                    
                        return Content("SUCCESS");
                    
                    //FnOnlinePayment payment = new FnOnlinePayment();
                    //payment.SourceSysNo = order.SysNo;
                    //payment.Amount = decimal.Parse(model["orderAmount"].ToString());
                    //payment.VoucherNo = model["serialNumber"].ToString();
                    //payment.BusinessOrderSysNo = model["requestId"].ToString();
                    //payment.Amount = payment.Amount / 100;
                    //payment.CreatedDate = DateTime.Now;
                    //payment.LastUpdateBy = 0;
                    //payment.LastUpdateDate = DateTime.Now;
                    //payment.PaymentTypeSysNo = (int)Model.SystemPredefined.PaymentType.易宝支付;
                    //payment.Source = (int)Hyt.Model.WorkflowStatus.FinanceStatus.收款来源类型.销售单;

                    //if (!FinanceBo.Instance.UpdateOrderPayStatus(payment,PaymentType.易宝支付))
                    //{
                    //    result.Status = false;
                    //    result.Message = "支付回调失败";
                    //}

                    var amount = decimal.Parse(model["orderAmount"].ToString());
                    SoOrderBo.Instance.WriteSoTransactionLog(order.TransactionSysNo,
                        string.Format(Constant.ORDER_TRANSACTIONLOG_PAY,
                            Util.FormatUtil.FormatCurrency(
                                (amount / 100), 2)),
                        systemManageUser.UserName);

                    //创建收款单明细
                    var receiptVoucherItem = new FnReceiptVoucherItem
                    {
                        Amount = (amount / 100),
                        CreatedBy = systemManageUser.SysNo,
                        LastUpdateBy = systemManageUser.SysNo,
                        VoucherNo = model["serialNumber"].ToString(),
                        PaymentTypeSysNo = order.PayTypeSysNo,
                        TransactionSysNo = order.TransactionSysNo,
                        CreatedDate = DateTime.Now,
                        LastUpdateDate = DateTime.Now,
                        Status = (int)FinanceStatus.收款单明细状态.有效,
                        ReceivablesSideType = (int)Hyt.Model.WorkflowStatus.FinanceStatus.收款方类型.财务中心,

                    };
                    //插入收款单,收款明细，
                    FnReceiptVoucherBo.Instance.InsertOrderReceiptVoucher(order, receiptVoucherItem);
                    //同步支付时间的到订单主表
                    ISoOrderDao.Instance.UpdateOrderPayDteById(order.SysNo);
                }
                else//易扫购发货异步通知
                {

                    #region 暂时保留注释测试完成去掉
                    //if (!string.IsNullOrEmpty(order.InternalRemarks))
                    //{
                    //    return Content("SUCCESS");
                    //}

                    //string name = model["address"]["receiverName"].ToString();
                    //string gender = "0";
                    //string phoneNumber = "";
                    //string mobilePhoneNumber = model["address"]["receiverTel"].ToString();
                    //string faxNumber = "";
                    //string emailAddress = "";
                    ////string country = "中国";
                    //string province = model["address"]["province"].ToString();
                    //string city = model["address"]["city"].ToString();
                    //string districts = model["address"]["region"].ToString();

                    //List<BsArea> areaList = BLL.Basic.BasicAreaBo.Instance.GetAll();
                    //List<BsArea> provinceList = areaList.Where(k => province.Contains(k.AreaName.Trim(new char[] { '\n', '\r' }))).ToList();

                    //if (provinceList.Count != 1)
                    //{
                    //    throw new Exception("省份不正确！");
                    //}

                    //provinceList = areaList.Where(k => k.ParentSysNo == provinceList[0].SysNo).Where(k => city.Contains(k.AreaName.Trim(new char[] { '\n', '\r' }))).ToList();

                    //if (provinceList.Count != 1)
                    //{
                    //    throw new Exception("市区不正确！");
                    //}

                    //provinceList = areaList.Where(k => k.ParentSysNo == provinceList[0].SysNo).Where(k => districts.Contains(k.AreaName.Trim(new char[] { '\n', '\r' }))).ToList();

                    //if (provinceList.Count != 1)
                    //{
                    //    throw new Exception("地区不正确！");
                    //}

                    //int iareaSysNo = provinceList[0].SysNo;
                    //string streetAddress = model["address"]["detail"].ToString();
                    //string zipCode = model["address"]["postCode"].ToString();
                    //string sIDCardImgs = "";
                    //string sIDCardNo = model["payer"]["idNum"].ToString();

                    //int receiveAddressNo = DataAccess.Order.ISoReceiveAddressDao.Instance.ExistReceiveAddress(name, mobilePhoneNumber, iareaSysNo.ToString(), streetAddress, sIDCardNo);

                    //var addr = new SoReceiveAddress();

                    //if (receiveAddressNo > 0)
                    //{
                    //    addr = DataAccess.Order.ISoReceiveAddressDao.Instance.GetOrderReceiveAddress(receiveAddressNo);
                    //}
                    //else
                    //{
                    //    addr.Name = name;
                    //    addr.Gender = Convert.ToInt32(gender);
                    //    addr.PhoneNumber = phoneNumber;
                    //    addr.MobilePhoneNumber = mobilePhoneNumber;
                    //    addr.FaxNumber = faxNumber;
                    //    addr.EmailAddress = emailAddress;
                    //    addr.AreaSysNo = iareaSysNo;
                    //    addr.StreetAddress = streetAddress;
                    //    addr.ZipCode = zipCode;
                    //    addr.IDCardImgs = sIDCardImgs;
                    //    addr.IDCardNo = sIDCardNo;


                    //    addr = DataAccess.Order.ISoReceiveAddressDao.Instance.InsertEntity(addr);
                    //}

                    //order.ReceiveAddressSysNo = addr.SysNo;
                    //order.InternalRemarks = "易扫购已发货";
                    //if (!Hyt.BLL.Order.SoOrderBo.Instance.UpdateOrder(order))
                    //{
                    //    result.Status = false;
                    //    result.Message = "发货回调失败";
                    //}
                    #endregion

                }

                if (!result.Status)
                {
                    //Grand.BLL.Log.SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Warn, LogStatus.系统日志来源.前台, "订单回写失败！",
                    //LogStatus.系统日志目标类型.订单, systemManageUser.SysNo, new Exception(finallyResult.Message + "\n" + (isTesting ? testString : jsonResult)), null, systemManageUser.SysNo);
                    return  Content("FAIL");
                }

               
            }
            catch (Exception ex)
            {
                //Grand.BLL.Log.SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.前台, "订单回调失败！",
                //LogStatus.系统日志目标类型.订单, systemManageUser.SysNo, new Exception((isTesting ? testString + "\n" : jsonResult + "\n") + ex.Message + "\n" + ex.StackTrace), null, systemManageUser.SysNo);

                return Content("FAIL");
            }

             
            return Content("SUCCESS");
        }

        /// <summary>
        /// 升舱订单处理
        /// </summary>
        /// <param name="orderSysNo">升舱订单的系统编号</param>
        /// <returns></returns>
        /// <remarks>2014-09-11 杨文兵 创建</remarks>
        public ContentResult UpgradeOrderHandler(string orderSysNo) 
        {
            //读取升舱订单相关数据，调用 Hyt.BLL.OrderRule.Instance.HandlerUpGradesOrder()方法。
            //将接收升舱订单编号写入日志
            SysLog.Instance.Info(LogStatus.系统日志来源.后台, "接收升舱订单的系统编号:" + orderSysNo, LogStatus.系统日志目标类型.订单,
                                    0, 0);
            string result = "UpgradeOrderHandler 参数错误";
            int sysno = 0;//订单编号
            int.TryParse(orderSysNo, out sysno);
            if (sysno > 0)
            {
                OrderData data = new OrderData();
                data.Order = SoOrderBo.Instance.GetEntity(sysno);
                data.OrderItems = SoOrderBo.Instance.GetOrderItemsByOrderId(sysno);
                if (data.Order != null && data.OrderItems != null)
                {
                    try
                    {
                        Hyt.BLL.OrderRule.OrderEngine.Instance.HandlerUpGradesOrder(data);
                        result = "ok";
                    }
                    catch(Exception ex)
                    {
                        result = ex.Message;
                        SysLog.Instance.Error(LogStatus.系统日志来源.后台, "升舱订单处理接口错误", LogStatus.系统日志目标类型.订单, sysno, ex, 0);
                    }
                }
            }
            return Content(result);
        }
        /// <summary>
        /// 包裹入库状态接收接口
        /// </summary>
        /// <returns></returns>
        /// <remarks>2015-09-16 王耀发 创建</remarks>
        public JsonResult InboundUpdateOrderStata()
        { 
            ApiResult result = new ApiResult();
            try
            {
                string RequestText = new System.IO.StreamReader(System.Web.HttpContext.Current.Request.InputStream, System.Text.Encoding.UTF8).ReadToEnd();
                //RequestText = Uri.UnescapeDataString(RequestText);
                //Hyt.BLL.Log.LocalLogBo.Instance.Write(RequestText, "CeShi");
                //RequestText = "{\"timestamp\":\"1442914213\",\"appid\":\"rennibiwang\",\"token\":\"0CC46F0B77B83273F0F56F998EF6672D\",\"data\":{\"Status\":\"2\",\"Weight\":1.0,\"Volume\":\"\",\"OverseaCarrier\":\"YDSD\",\"OverseaTrackingNo\":\"201509221722\",\"CommodityList\":[{\"SKU\":\"20150916115\",\"UPC\":\"\",\"CommodityName\":\"澳洲直邮Swisse Calcium+Vit D维生素D3钙片150片成人青少年补钙\",\"InboundQuantity\":1,\"StockQuantity\":1},{\"SKU\":\"20150916119\",\"UPC\":\"\",\"CommodityName\":\"美国直邮L'il Critters小熊糖宝宝钙含维生素D儿童钙片200粒\",\"InboundQuantity\":1,\"StockQuantity\":1}]}}";
                var jsonObject =JObject.Parse(RequestText);
                
                string timestamp = jsonObject["timestamp"].ToString();
                string token = jsonObject["token"].ToString();
                string OverseaCarrier = jsonObject["data"]["OverseaCarrier"].ToString();
                string OverseaTrackingNo = jsonObject["data"]["OverseaTrackingNo"].ToString();
                //判断加密
                if (System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile("1CCC82C61D664A36A28AF867AF9B3C3D" + timestamp, "MD5").ToLower() == token.ToLower())
                {
                    Hyt.Model.SendOrderReturn entity = SendOrderReturnBo.Instance.GetEntityByOversea(OverseaCarrier, OverseaTrackingNo);
                    if (entity != null)
                    {
                        result.code = "0";
                        result.msg = "调用成功！";
                        // 创建出库单
                        //CreateOutStock(entity.soOrderSysNo, jsonObject);
                    }
                    else
                    {
                        result.code = "1";
                        result.msg = "1调用失败 ！";
                    }
                }
                else
                {
                    result.code = "1";
                    result.msg = "2调用失败 ！";
                }
            }
            catch (Exception ex)
            {
                result.code = "1";
                result.msg = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 创建出库单
        /// </summary>
        /// <param name="soOrderSysNo"></param>
        /// <returns></returns>
        /// <remarks>2015-09-16 王耀发 创建</remarks>
        public bool CreateOutStock(int soOrderSysNo, JObject jsonObject)
        {
            SoOrder sEntity = SoOrderBo.Instance.GetEntityNoCache(soOrderSysNo);
            int warehouseSysNo = sEntity.DefaultWarehouseSysNo;
            int? deliveryTypeSysNo = sEntity.DeliveryTypeSysNo;
            IList<SoOrderItem> data = SoOrderBo.Instance.GetOrderItemsByOrderId(soOrderSysNo);
            
            foreach (SoOrderItem Item in data)
            {
                Item.RealStockOutQuantity = Item.Quantity;
            }

            var identity = string.Format("创建订单{0}的出库单", soOrderSysNo);
            if (YwbUtil.Enter(identity) == false)
            {
                return false;
            }

            try
            {
                using (var tran = new TransactionScope())
                {
                    //创建出库单
                    var outStock = SoOrderBo.Instance.CreateOutStock(data, warehouseSysNo, CurrentUser.Base, deliveryTypeSysNo);
                    SysLog.Instance.Info(LogStatus.系统日志来源.后台, "创建出库单", LogStatus.系统日志目标类型.出库单, outStock.SysNo, CurrentUser.Base.SysNo);
                    //确定出库
                    string checkedStockItemSysNo = "";
                    IList<WhStockOutItem> outData = Hyt.BLL.Warehouse.WhWarehouseBo.GetWhStockOutItemByOut(outStock.SysNo);
                    foreach (var Item in outData)
                    {
                        checkedStockItemSysNo += Item.SysNo.ToString() + ',' + Item.ProductQuantity.ToString() + ';';
                    }
                    StockOut(outStock.SysNo, checkedStockItemSysNo, (int)deliveryTypeSysNo, 1, outStock.Stamp.ToString());
                    //更新订单状态
                    Hyt.BLL.Order.SoOrderBo.UpdateOrderStatusNew(soOrderSysNo, (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.已创建出库单);
                    //推送运单
                    string OutboundOrderNo = OutboundCreate(soOrderSysNo, jsonObject);
                    //更新库存数量
                    foreach (var Item in outData)
                    {
                        Hyt.BLL.Warehouse.PdProductStockBo.Instance.UpdateStockQuantity(warehouseSysNo, Item.ProductSysNo, Item.ProductQuantity);
                    }

                    int NoteType = (int)Hyt.Model.WorkflowStatus.LogisticsStatus.配送单据类型.出库单;
                    int NoteSysNo = outStock.SysNo;
                    string ExpressNo = OutboundOrderNo;
                    string str = NoteType + "," + NoteSysNo + "," + ExpressNo;
                    string[] items = { str };

                    //创建配送单  
                    ConfrimDelivery(warehouseSysNo, -1, (int)deliveryTypeSysNo, items, false);
                    tran.Complete();
                }
            }
            catch (Exception ex)
            {

            }
            YwbUtil.Exit(identity);
            return true;
        }




        /// <summary>
        /// 创建出库单通过推送
        /// </summary>
        /// <param name="soOrderSysNo"></param>
        /// <returns></returns>
        /// <remarks>2015-10-20 王耀发 创建</remarks>
        public bool CreateOutStockBySendMsg(int soOrderSysNo, string OutboundOrderNo)
        {
            SoOrder sEntity = SoOrderBo.Instance.GetEntityNoCache(soOrderSysNo);
            int warehouseSysNo = sEntity.DefaultWarehouseSysNo;
            int? deliveryTypeSysNo = sEntity.DeliveryTypeSysNo;
            //固定为一号仓物流
            deliveryTypeSysNo = 9;

            IList<SoOrderItem> data = SoOrderBo.Instance.GetOrderItemsByOrderId(soOrderSysNo);

            foreach (SoOrderItem Item in data)
            {
                Item.RealStockOutQuantity = Item.Quantity;
            }

            var identity = string.Format("创建订单{0}的出库单", soOrderSysNo);
            if (YwbUtil.Enter(identity) == false)
            {
                return false;
            }

            try
            {
                using (var tran = new TransactionScope())
                {
                    //创建出库单
                    var outStock = SoOrderBo.Instance.CreateOutStock(data, warehouseSysNo, CurrentUser.Base, deliveryTypeSysNo);
                    SysLog.Instance.Info(LogStatus.系统日志来源.后台, "创建出库单", LogStatus.系统日志目标类型.出库单, outStock.SysNo, CurrentUser.Base.SysNo);
                    //确定出库
                    string checkedStockItemSysNo = "";
                    IList<WhStockOutItem> outData = Hyt.BLL.Warehouse.WhWarehouseBo.GetWhStockOutItemByOut(outStock.SysNo);
                    foreach (var Item in outData)
                    {
                        checkedStockItemSysNo += Item.SysNo.ToString() + ',' + Item.ProductQuantity.ToString() + ';';
                    }
                    StockOut(outStock.SysNo, checkedStockItemSysNo, (int)deliveryTypeSysNo, 1, outStock.Stamp.ToString());
                    //更新订单状态
                    Hyt.BLL.Order.SoOrderBo.UpdateOrderStatusNew(soOrderSysNo, (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.已创建出库单);

                    //更新库存数量
                    foreach (var Item in outData)
                    {
                        Hyt.BLL.Warehouse.PdProductStockBo.Instance.UpdateStockQuantity(warehouseSysNo, Item.ProductSysNo, Item.ProductQuantity);
                    }

                    int NoteType = (int)Hyt.Model.WorkflowStatus.LogisticsStatus.配送单据类型.出库单;
                    int NoteSysNo = outStock.SysNo;
                    string ExpressNo = OutboundOrderNo;
                    string str = NoteType + "," + NoteSysNo + "," + ExpressNo;
                    string[] items = { str };

                    //创建配送单  
                    ConfrimDelivery(warehouseSysNo, -1, (int)deliveryTypeSysNo, items, false);
                    tran.Complete();
                }
            }
            catch (Exception ex)
            {

            }
            YwbUtil.Exit(identity);
            return true;
        }



        /// <summary>
        /// 确定出库
        /// </summary>
        /// <param name="sysNo"></param>
        /// <param name="checkedStockItemSysNo"></param>
        /// <param name="deliveryTypeSysNo"></param>
        /// <param name="isThirdpartyExpress"></param>
        /// <param name="stamp"></param>
        /// <remarks>2015-09-16 王耀发 创建</remarks>
        public bool StockOut(int sysNo, string checkedStockItemSysNo, int deliveryTypeSysNo, int isThirdpartyExpress, string stamp)
        {
            string Message = "";
            //开始事务
            using (var scope = new TransactionScope())
            {
                if (string.IsNullOrEmpty(checkedStockItemSysNo))
                {
                    Message = "没有扫描任何商品， 不能出库。";
                    SysLog.Instance.Error(LogStatus.系统日志来源.后台, Message, LogStatus.系统日志目标类型.出库单, sysNo, "",CurrentUser.Base.SysNo);
                    return false;
                }
                if (checkedStockItemSysNo.LastIndexOf(';', checkedStockItemSysNo.Length - 1) > 0)
                {
                    checkedStockItemSysNo = checkedStockItemSysNo.Remove(checkedStockItemSysNo.Length - 1, 1);
                }
                //出库单明细系统编号与商品数量集合字符串
                var stockItemSysNoAndProductNumList = checkedStockItemSysNo.Split(';');
                if (stockItemSysNoAndProductNumList.Length == 0)
                {
                    Message = "没有扫描任何商品， 不能出库。";
                    SysLog.Instance.Error(LogStatus.系统日志来源.后台, Message, LogStatus.系统日志目标类型.出库单, sysNo, "", CurrentUser.Base.SysNo);
                    return false;
                }
                //检查是否所有的商品数量是否都为0或者为空
                var isItemScaned = false;
                foreach (var s in stockItemSysNoAndProductNumList)
                {
                    string productNum = s.Split(',')[1];
                    if (!string.IsNullOrEmpty(productNum) && productNum != "0")
                    {
                        isItemScaned = true;
                        break;
                    }
                }
                if (!isItemScaned)
                {
                    Message = "没有扫描任何商品， 不能出库。";
                    SysLog.Instance.Error(LogStatus.系统日志来源.后台, Message, LogStatus.系统日志目标类型.出库单, sysNo, "", CurrentUser.Base.SysNo);
                    return false;
                }

                var master = Hyt.BLL.Warehouse.WhWarehouseBo.Instance.Get(sysNo);
                //检查时间戳是否改变
                if (master.Stamp.ToString() != stamp)
                {
                    Message = "此出库单已被其他人修改，请关闭当前窗口后刷新页面！";
                    SysLog.Instance.Error(LogStatus.系统日志来源.后台, Message, LogStatus.系统日志目标类型.出库单, sysNo, "", CurrentUser.Base.SysNo);
                    return false;
                }
                if (master.Status != (int)WarehouseStatus.出库单状态.待出库)
                {
                    Message = "此出库单不是待出库状态，不能出库！";
                    SysLog.Instance.Error(LogStatus.系统日志来源.后台, Message, LogStatus.系统日志目标类型.出库单, sysNo, "", CurrentUser.Base.SysNo);
                    return false;
                }

                //第三方快递,订单未收收款,不允许出库
                if (isThirdpartyExpress == 1)
                {
                    var order = SoOrderBo.Instance.GetEntity(master.OrderSysNO);
                    if (order.PayStatus != OrderStatus.销售单支付状态.已支付.GetHashCode())
                    {
                        Message = "第三方快递订单未收款,不能出库。";
                        SysLog.Instance.Error(LogStatus.系统日志来源.后台, Message, LogStatus.系统日志目标类型.出库单, sysNo, "", CurrentUser.Base.SysNo);
                        return false;
                    }
                }

                foreach (var item in master.Items)
                {
                    foreach (var s in stockItemSysNoAndProductNumList)
                    {
                        if (item.SysNo == Convert.ToInt32(s.Split(',')[0])
                            && !string.IsNullOrEmpty(s.Split(',')[1])
                            && Convert.ToInt32(s.Split(',')[1]) > 0)
                        {
                            item.IsScaned = true;
                            item.ScanedQuantity =
                                Convert.ToInt32(string.IsNullOrEmpty(s.Split(',')[1]) ? "0" : s.Split(',')[1]);
                        }
                    }
                }

                try
                {
                    master.DeliveryTypeSysNo = deliveryTypeSysNo;
                    master.StockOutDate = DateTime.Now;
                    master.StockOutBy = CurrentUser.Base.SysNo;
                    master.LastUpdateBy = CurrentUser.Base.SysNo;
                    master.LastUpdateDate = DateTime.Now;
                    master.Status = (int)WarehouseStatus.出库单状态.待配送;

                    Hyt.BLL.Warehouse.WhWarehouseBo.Instance.OutStockApi(master, CurrentUser.Base);

                    var delivery = Hyt.BLL.Logistics.DeliveryTypeBo.Instance.GetDeliveryType(deliveryTypeSysNo);
                    var deliveryName = (delivery == null)
                        ? "未能找到编号为" + deliveryTypeSysNo + "的配送方式"
                        : delivery.DeliveryTypeName;
                    var logTxt = "订单生成配送方式:<span style=\"color:red\">" + deliveryName + "</span>，待拣货打包";

                    SoOrderBo.Instance.WriteSoTransactionLog(master.TransactionSysNo, logTxt, CurrentUser.Base.UserName);
                    Message = master.SysNo + " 出库成功。";
                    SysLog.Instance.Error(LogStatus.系统日志来源.后台, Message, LogStatus.系统日志目标类型.出库单, sysNo, "", CurrentUser.Base.SysNo);
                }
                catch (Exception ex)
                {
                    Message = ex.Message;
                    SysLog.Instance.Error(LogStatus.系统日志来源.后台, ex.Message, LogStatus.系统日志目标类型.出库单, master.SysNo, ex, CurrentUser.Base.SysNo);
                }
                scope.Complete();
            }
            return true;
        }
        /// <summary>
        /// 创建一号仓运单接口
        /// </summary>
        /// <param name="soOrderSysNo"></param>
        /// <returns></returns>
        /// <remarks>2015-09-16 王耀发 创建</remarks>
        public string OutboundCreate(int soOrderSysNo, JObject jsonObject)
        {
            SoReceiveAddress srEnity = Hyt.BLL.Order.OutboundReturnBo.Instance.GetSoReceiveAddressBysoOrderSysNo(soOrderSysNo);
            string IDCardNo = "";
            string IDCardImgs = "";
            string IdentityCardImageURL1 = "";
            string IdentityCardImageURL2 = "";
            if (!string.IsNullOrEmpty(srEnity.IDCardNo))
            {
                IDCardNo = srEnity.IDCardNo;
            }
            if (!string.IsNullOrEmpty(srEnity.IDCardImgs))
            {
                IDCardImgs = srEnity.IDCardImgs;
                string[] img = IDCardImgs.Split(';');
                //获得对应身份证，正反面
                string FileServer = Hyt.BLL.Config.Config.Instance.GetAttachmentConfig().FileServer;
                if (!string.IsNullOrEmpty(img[0]))
                {
                    IdentityCardImageURL1 = FileServer + img[0];
                }
                if (!string.IsNullOrEmpty(img[1]))
                {
                    IdentityCardImageURL2 = FileServer + img[1];
                }
            }
            
            BsArea DistrictEntity = Hyt.BLL.Basic.BasicAreaBo.Instance.GetArea(srEnity.AreaSysNo);
            string District = DistrictEntity.AreaName;
            BsArea CityEntity = Hyt.BLL.Basic.BasicAreaBo.Instance.GetArea(DistrictEntity.ParentSysNo);
            string City = CityEntity.AreaName;
            BsArea ProvinceEntity = Hyt.BLL.Basic.BasicAreaBo.Instance.GetArea(CityEntity.ParentSysNo);
            string Province = ProvinceEntity.AreaName;

            string WarehouseId = "1";  //海外仓库编号，目前一号仓免税州只有一个仓库，代码是1,请固定填写 1
            string strJson = "{\"WarehouseId\":\"" + WarehouseId + "\",\"CustomerReference\":\"\",\"ServiceCode\":\"\",\"ServiceChannelCode\":\"\",\"IsExtractInvoice\":false,\"Consignee\":\"" + srEnity.Name + "\",\"Phone\":\"" + srEnity.PhoneNumber + "\",\"Province\":\"" + Province + "\",";
            strJson += "\"City\":\"" + City + "\",\"District\":\"" + District + "\",\"Postcode\":\"" + srEnity.ZipCode + "\",\"Address\":\"" + srEnity.StreetAddress + "\",\"IdentityCard\":\"" + IDCardNo + "\",\"IdentityCardImageURL1\":\"" + IdentityCardImageURL1 + "\",\"IdentityCardImageURL2\":\"" + IdentityCardImageURL2 + "\",\"Remark\":\"\",";

            string OverseaCarrier = jsonObject["data"]["OverseaCarrier"].ToString();
            string OverseaTrackingNo = jsonObject["data"]["OverseaTrackingNo"].ToString();
            strJson += "\"CommodityList\":";
            strJson += "[";
            string str = "";
            int i = 0;
            foreach (JObject item in jsonObject["data"]["CommodityList"])
            {
                if (i > 0)
                {
                    str += ",{";
                }
                else
                {
                    str += "{";
                }
                str += "\"SKU\":\"" + item["SKU"].ToString();
                str += "\",\"UPC\":\"" + item["UPC"].ToString();
                str += "\",\"CommodityName\":\"" + item["CommodityName"].ToString();
                str += "\",\"Quantity\":" + item["InboundQuantity"].ToString();
                str += ",\"CustomerReference\":\"\",\"OverseaTrackingNo\":\"" + OverseaTrackingNo + "\",\"OverseaCarrier\":\"" + OverseaCarrier + "\"";
                str += "}";
                i++;
            }
            strJson += str;
            strJson += "]}"; 
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("data", strJson);
            //获得接口返回值
            string OutboundOrderNo = "";
            string Message = "";
            var sAPIResult= "";
            try
            {
                sAPIResult = Extra.Logistics.Client.Post(Extra.Logistics.ApiUrl.OutboundCreate, data);
                sAPIResult = sAPIResult.Trim('{').Trim('}');
                string[] a = sAPIResult.Split(',');
                string[] b;
                string c = "";
                foreach (string sArray in a)
                {
                    b = sArray.Split(':');
                    if (c == "")
                    {
                        c = b[1].Trim('"');
                    }
                    else
                    {
                        c += ',' + b[1].Trim('"');
                    }
                }
                string d = c.Trim('"');
                string[] e = d.Split(',');
                OutboundReturn m = new OutboundReturn();
                m.soOrderSysNo = soOrderSysNo;
                m.Code = e[0];
                m.Msg = e[1];
                if (m.Code == "0")
                {
                    m.OutboundOrderNo = e[2];
                }
                if (m.Code == "1")
                {
                    m.OutboundOrderNo = "";
                }
                OutboundOrderNo = m.OutboundOrderNo;
                Hyt.BLL.Order.OutboundReturnBo.Instance.InsertOutboundReturn(m, 1);  
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
            return OutboundOrderNo;
        }
        /// <summary>
        /// 运单转运状态接收接口
        /// </summary>
        /// <returns></returns>
        /// <remarks>2015-09-16 王耀发 创建</remarks>
        public JsonResult InboundQueryAndSendMsg()
        {
            ApiResult result = new ApiResult();           
            try
            {
                string RequestText = new System.IO.StreamReader(System.Web.HttpContext.Current.Request.InputStream, System.Text.Encoding.UTF8).ReadToEnd();
                //RequestText = Uri.UnescapeDataString(RequestText);
                //RequestText = "{\"timestamp\":\"1442917271\",\"appid\":\"rennibiwang\",\"token\":\"39B5066BF2A379610EC4CCA0F27A81FC\",\"data\":{\"express_status\":\"20\",\"express_no\":\"OCABZAU150974074\",\"cn_transfer_company\":\"\",\"cn_transfer_no\":\"\",\"Weight\":\"1\",\"Volume\":\"\",\"express_infos\":[{\"time\":\"2015-09-16 00:00:00\",\"context\":\"航班从啊啊啊啊起飞，前往xxxx\"},{\"time\":\"2015-09-16 00:00:00\",\"context\":\"航班经学学学转机想想\"},{\"time\":\"2015-09-22 18:18:32\",\"context\":\"货物到达波特兰仓库处理中心，进行货物处理\"},{\"time\":\"2015-09-23 10:00:00\",\"context\":\"波特兰仓库处理中心出库。即将交给航空公司\"}]}}";
                var jsonObject = JObject.Parse(RequestText);

                string timestamp = jsonObject["timestamp"].ToString();
                string token = jsonObject["token"].ToString();
                string express_no = jsonObject["data"]["express_no"].ToString();
                string express_status = jsonObject["data"]["express_status"].ToString();
                string cn_transfer_company = jsonObject["data"]["cn_transfer_company"].ToString();
                string cn_transfer_no = jsonObject["data"]["cn_transfer_no"].ToString();
                string time = "";
                string context = "";
                //判断加密
                if (System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile("1CCC82C61D664A36A28AF867AF9B3C3D" + timestamp, "MD5").ToLower() == token.ToLower())
                {
                    OutboundReturn oEntity = Hyt.BLL.Order.OutboundReturnBo.Instance.GetEntityByOutboundOrderNo(express_no);
                    //Hyt.BLL.Log.LocalLogBo.Instance.Write(express_status, "201510211135");
                    //创建出库单
                    if (express_status == "20")
                    {
                        CreateOutStockBySendMsg(oEntity.soOrderSysNo, express_no);
                    }

                    if (oEntity != null && oEntity.soOrderSysNo > 0)
                    {
                        //获取订单地址信息
                        SoReceiveAddress srEnity = Hyt.BLL.Order.OutboundReturnBo.Instance.GetSoReceiveAddressBysoOrderSysNo(oEntity.soOrderSysNo);
                        //获取订单信息
                        SoOrder SData = Hyt.BLL.Order.SoOrderBo.Instance.GetEntityNoCache(oEntity.soOrderSysNo);

                        result.code = "0";
                        result.msg = "调用成功！";
                        //状态20 发往国内，25到达国内,30清关中，40国内配送才发送短信
                        if (express_status == "20" || express_status == "25" || express_status == "30" || express_status == "40")
                        {
                            Hyt.BLL.Extras.SmsBO obj = new Hyt.BLL.Extras.SmsBO();
                            //发送短信
                            int count = jsonObject["data"]["express_infos"].Count();
                            int i = 0;
                            foreach (JObject item in jsonObject["data"]["express_infos"])
                            {
                                i = i + 1;
                                if (i == count)
                                {
                                    time = item["time"].ToString();
                                    if (express_status != "40")
                                    {
                                        context = "尊敬的客户，您在商城下的订单(" + SData.OrderNo.ToString() + ")，" + item["context"].ToString() + "，请留意，谢谢。";
                                    }
                                    else
                                    {
                                        context = "尊敬的客户，您在商城下的订单(" + SData.OrderNo.ToString() + ")已经转国内" + cn_transfer_company + "配送，快递单号" + cn_transfer_no + "，请留意，谢谢。";
                                    }
                                    obj.SendMsg(srEnity.MobilePhoneNumber, context, DateTime.Now);
                                }
                            }
                        }
                    }
                    else
                    {
                        
                        result.code = "1";
                        result.msg = "调用失败 ！";
                    }
                }
                else
                {
                    result.code = "1";
                    result.msg = "调用失败 ！";
                }
            }
            catch (Exception ex)
            {
                result.code = "1";
                result.msg = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #region 添加配送单
        /// <summary>
        /// 配送单确认发货（Ajax调用）
        /// </summary>
        ///<param name="warehouseSysNo">仓库系统编号</param>
        /// <param name="deliveryUserSysNo">配送员系统编号.</param>
        /// <param name="deliverTypeSysno">配送方式系统编号.</param>
        /// <param name="items">配送单明细 (单据类型,单据编号,快递单号)</param>
        /// /// <param name="isForce">是否允许超出配送信用额度配送 </param>
        /// <returns>返回Result</returns>
        /// <remarks>2013-06-27 沈强 创建</remarks>
        public void ConfrimDelivery(int warehouseSysNo, int deliveryUserSysNo, int deliverTypeSysno, string[] items, bool isForce)
        {
            var result = new Result<int>();
            try
            {

                var itemList = new List<LgDeliveryItem> { };
                string NeedInStock = string.Empty;
                foreach (var note in items.Select(item => item.Split(',')))
                {
                    //if (note.Length < 2)
                    //{
                    //    result.Message = "配送单明细数据错误,不能创建配送单";
                    //    SysLog.Instance.Error(LogStatus.系统日志来源.后台, result.Message, null);
                    //}
                    LgDeliveryItem item = new LgDeliveryItem
                    {
                        NoteType = int.Parse(note[0]),
                        NoteSysNo = int.Parse(note[1]),
                        //ExpressNo = note.Length >= 3 ? note[2].Trim() : ""
                        ExpressNo = note[2].Trim()
                    };

                    //#region 判断快递单号是否重复(2014-04-11 朱成果)
                    //if (!string.IsNullOrEmpty(item.ExpressNo) && item.NoteType == (int)LogisticsStatus.配送单据类型.出库单)
                    //{
                    //    var flg = Hyt.BLL.Logistics.LgDeliveryBo.Instance.IsExistsExpressNo(deliverTypeSysno, item.ExpressNo);
                    //    if (flg)
                    //    {
                    //        result.Status = false;
                    //        result.Message = "快递单号" + item.ExpressNo + "已经被使用，请更换快递单号";
                    //        SysLog.Instance.Error(LogStatus.系统日志来源.后台, result.Message, null);
                    //        //return Json(result);
                    //    }
                    //}
                    //#endregion

                    #region 配送单作废会生成出库单对应的入库单，再次将此入库单加入配送,需检查此入库单是否已经完成入库(2014-04-11 朱成果)

                    if (item.NoteType == (int)LogisticsStatus.配送单据类型.出库单)
                    {
                        var rr = Hyt.BLL.Logistics.LgDeliveryBo.Instance.CheckInStock(item.NoteSysNo);
                        if (rr.Status)
                        {
                            if (!string.IsNullOrEmpty(NeedInStock))
                            {
                                NeedInStock += ",";
                            }
                            NeedInStock += rr.StatusCode;
                        }
                    }

                    #endregion

                    itemList.Add(item);
                }
                //if (!string.IsNullOrEmpty(NeedInStock)) //未入库的单子
                //{
                //    result.Status = false;
                //    result.Message = "请将先前配送单作废，拒收，未送达生成的入库单(" + NeedInStock + ")完成入库";
                //    SysLog.Instance.Error(LogStatus.系统日志来源.后台, result.Message, null);
                //    //return Json(result);
                //}
                var options = new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadCommitted,
                    Timeout = TransactionManager.DefaultTimeout
                };

                //配送方式  
                var delivertType = Hyt.BLL.Logistics.DeliveryTypeBo.Instance.GetDeliveryType(deliverTypeSysno);

                int deliverySysno;
                var deliveryMsgs = new List<Hyt.BLL.Logistics.LgDeliveryBo.DeliveryMsg>();

                //using (var tran = new TransactionScope(TransactionScopeOption.Required, options))
                //{
                    deliverySysno = Hyt.BLL.Logistics.LgDeliveryBo.Instance.NewCreateLgDelivery(warehouseSysNo, deliveryUserSysNo,
                        delivertType,CurrentUser.Base.SysNo, itemList, isForce, ref deliveryMsgs, Request.ServerVariables["REMOTE_ADDR"]);

                    result.Status = true;
                    result.Data = deliverySysno;
                    result.Message = "确认发货完成";
                    //tran.Complete();
                //}

                //2014-05-09 黄波/何永东/杨文兵 添加
                try
                {
                    #region 发送相关短消息
                    //var smsBo = new BLL.Extras.SmsBO();
                    ////发送相关消息
                    //foreach (var msg in deliveryMsgs)
                    //{

                    //    if (msg.IsThirdPartyExpress == 0)
                    //    {
                    //        smsBo.发送自建物流发货通知短信(msg.MobilePhoneNum, msg.OrderSysNo.ToString(),
                    //                                           msg.UserName, msg.UserMobilePhoneNum);
                    //    }

                    //    if (msg.IsThirdPartyExpress == 1)
                    //    {
                    //        smsBo.发送第三方物流发货通知短信(msg.MobilePhoneNum, msg.OrderSysNo.ToString());
                    //    }
                    //}

                    #endregion

                    //回填物流信息
                    try
                    {
                        Hyt.BLL.Logistics.LgDeliveryBo.Instance.BackFillLogisticsInfo(deliverySysno, deliverTypeSysno);
                    }
                    catch (Exception ex)
                    {
                        Hyt.BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.分销工具, ex.Message, ex);
                    }

                    //调用快递100的订阅服务
                    try
                    {
                        //LgDeliveryBo.Instance.CallKuaiDi100(itemList, warehouseSysNo, delivertType);
                    }
                    catch (Exception ex)
                    {
                        Hyt.BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, ex.Message, ex);
                    }
                }
                catch (Exception ex)
                {
                    SysLog.Instance.Error(LogStatus.系统日志来源.分销工具, ex.Message, ex);
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = "发生错误！" + ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, ex.Message, ex);
            }
            //return Json(result);
        }
        #endregion

        /// <summary>
        /// 接受发送数据 
        /// </summary>
        /// <returns></returns>
        /// <remarks>2015-10-07 王耀发 创建</remarks>
        public JsonResult ReceiveCustomsOrder()
        {
            ApiResult result = new ApiResult();
            try
            {
                string RequestText = new System.IO.StreamReader(System.Web.HttpContext.Current.Request.InputStream, System.Text.Encoding.UTF8).ReadToEnd();
                string FileName = Extra.Customs.Xml.UploadXmlFile(RequestText);
                result.code = "0";
                result.msg = FileName;
            }
            catch (Exception ex)
            {
                result.code = "1";
                result.msg = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 返回回执报文 
        /// </summary>
        /// <returns></returns>
        /// <remarks>2015-10-07 王耀发 创建</remarks>
        public JsonResult ReturnCustomsOrder()
        {
            ApiResult result = new ApiResult();
            try
            {
                string RequestText = new System.IO.StreamReader(System.Web.HttpContext.Current.Request.InputStream, System.Text.Encoding.UTF8).ReadToEnd();
                string Xml = Extra.Customs.Xml.DownloadXmlFile(RequestText);
                result.code = "0";
                result.msg = Xml;
            }
            catch (Exception ex)
            {
                result.code = "1";
                result.msg = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
