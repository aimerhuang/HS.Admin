using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Transactions;
using Extra.Erp.Model;
using Hyt.BLL.Basic;
using Hyt.BLL.CRM;
using Hyt.BLL.Finance;
using Hyt.BLL.Log;
using Hyt.BLL.Logistics;
using Hyt.BLL.MallSeller;
using Hyt.BLL.Product;
using Hyt.BLL.Promotion;
using Hyt.BLL.Sys;
using Hyt.BLL.Warehouse;
using Hyt.DataAccess.CRM;
using Hyt.DataAccess.Promotion;
using Hyt.Infrastructure.Caching;
using Hyt.Infrastructure.Memory;
using Hyt.Model;
using Hyt.Model.B2CApp;
using Hyt.Model.WorkflowStatus;
using Hyt.Model.Parameter;
using Hyt.Model.SystemPredefined;
using Hyt.Util;
using Hyt.Util.Extension;
using Hyt.Util.Validator;
using Hyt.Util.Validator.Rule;
using Hyt.Infrastructure.Pager;
using Hyt.DataAccess.Order;
using Hyt.DataAccess.Sys;
using Hyt.DataAccess.Log;
using Hyt.Model.Transfer;
using Hyt.Model.Manual;
using DeliveryType = Hyt.Model.SystemPredefined.DeliveryType;
using Hyt.BLL.LevelPoint;
using Hyt.Model.Common;
using Result = Hyt.Model.Result;
using Extra.Erp;
using Hyt.BLL.Cart;
using Hyt.DataAccess.Warehouse;
using Hyt.BLL.Authentication;
using Hyt.BLL.Distribution;
using Hyt.Model.UpGrade;
using Extra.UpGrade.Model;
using Hyt.DataAccess.Finance;
using Extra.Erp.Kis;
using Hyt.Model.Generated;
using Hyt.BLL.Balance;
using Grand.Platform.Api.Contract.Model;
using Grand.Platform.Api.Contract.DataContract;
using Hyt.Model.Order;
using System.Web.Script.Serialization;
using Hyt.Model.DouShabaoModel;
using Hyt.DataAccess.Product;
using Extra.UpGrade.HaiDaiModel;
using Extra.UpGrade.Provider;
using System.Diagnostics;
using Hyt.DataAccess.MallSeller;
using Extra.Erp.XingYe;


namespace Hyt.BLL.Order
{
    /// <summary>
    /// 订单管理
    /// </summary>
    /// <remarks>2013-07-08 朱成果 创建</remarks>
    public class SoOrderBo : BOBase<SoOrderBo>
    {

        /// <summary>
        /// 更新支付和在线支付状态
        /// </summary>
        /// <param name="sysno">订单号</param>
        /// <param name="payStatus"></param>
        /// <param name="onlineStatus"></param>
        /// <param name="status">订单状态</param>
        /// <param name="customsPayStatus">支付推海关状态</param>
        /// <param name="tradeCheckStatus">支付推国检状态</param>
        /// <param name="payTypeSysNo">支付类型系统编号</param>
        /// <remarks>2017-09-27 杨浩 创建</remarks>
        public bool UpdatePayStatusAndOnlineStatusAndStatus(int sysno, int payStatus, string onlineStatus, int status, int customsPayStatus, int tradeCheckStatus, int payTypeSysNo)
        {
            return ISoOrderDao.Instance.UpdatePayStatusAndOnlineStatusAndStatus(sysno, payStatus, onlineStatus, status, customsPayStatus, tradeCheckStatus, payTypeSysNo) > 0;
        }
        /// <summary>
        /// 更新订单支付状态（批量支付专用同时申报海关、国检）
        /// </summary>
        /// <param name="payment">支付信息</param>
        /// <param name="tradeCheckStatus">国检推送状态</param>
        /// <returns></returns>
        /// <remarks>2017-09-27 杨浩 创建</remarks>
        public bool UpdateOrderPaySuatus(FnOnlinePayment onlinePayment, int tradeCheckStatus = (int)OrderStatus.支付报关状态.未提交, int customsPayStatus = (int)OrderStatus.支付报关状态.未提交)
        {
            /*
             * 1.支付方式类型为预付&&未付款的订单
             * 2.修改订单相关：支付状态改为已支付 订单状态如果为待支付改为待创建出库单
             * 3.根据收款单单据来源和单据来源编号，创建收款单明细
             */

            if (onlinePayment == null)
                throw new ArgumentNullException("onlinePayment");

            onlinePayment.Source = (int)FinanceStatus.网上支付单据来源.销售单;
            onlinePayment.Status = (int)FinanceStatus.网上支付状态.有效;
            int orderID = onlinePayment.SourceSysNo;
            var soOrder = ISoOrderDao.Instance.GetSimplifyOrderInfo(orderID);
            var payStatus = soOrder.PayStatus;
            var paymentType = 10;//预付Basic.PaymentTypeBo.Instance.GetEntity(soOrder.PayTypeSysNo).PaymentType;

            if (payStatus == 20)
                return false;

            //创建网上支付记录
            var r = 0;
            if (paymentType == (int)BasicStatus.支付方式类型.预付 &&
                payStatus == (int)OrderStatus.销售单支付状态.未支付)
            {
                r = IFnOnlinePaymentDao.Instance.Insert(onlinePayment);
            }

            if (r == 0) return false;


            var userName = "系统执行";//创建人姓名
            SoOrderBo.Instance.WriteSoTransactionLog(soOrder.TransactionSysNo,
                                                                   string.Format(Constant.ORDER_TRANSACTIONLOG_PAY,
                                                                                 Util.FormatUtil.FormatCurrency(
                                                                                     onlinePayment.Amount, 2)),
                                                                   userName);
            #region 创建收款单明细
            var receiptVoucherItem = new FnReceiptVoucherItem
            {
                Amount = onlinePayment.Amount,
                CreatedBy = onlinePayment.CreatedBy,
                LastUpdateBy = onlinePayment.CreatedBy,
                VoucherNo = onlinePayment.VoucherNo,
                PaymentTypeSysNo = onlinePayment.PaymentTypeSysNo,
                TransactionSysNo = soOrder.TransactionSysNo,
                CreatedDate = DateTime.Now,
                LastUpdateDate = DateTime.Now,
                Status = (int)FinanceStatus.收款单明细状态.有效,
                ReceivablesSideType = (int)Hyt.Model.WorkflowStatus.FinanceStatus.收款方类型.财务中心,

            };
            //插入收款单,收款明细，

            //收款单
            //var order = SoOrderBo.Instance.GetEntity(orderID);
            int sourceFromOrder = (int)Hyt.Model.WorkflowStatus.FinanceStatus.收款来源类型.销售单;
            var entity = Hyt.DataAccess.Finance.IFnReceiptVoucherDao.Instance.GetEntity(sourceFromOrder, onlinePayment.SourceSysNo);
            if (entity == null) //不存在收款单创建一条记录
            {
                var _entity = Hyt.DataAccess.Finance.IFnReceiptVoucherDao.Instance.GetEntity(sourceFromOrder, soOrder.SysNo);
                if (_entity == null)
                {
                    var rv = new FnReceiptVoucher()
                    {
                        TransactionSysNo = soOrder.TransactionSysNo,
                        Status = (int)FinanceStatus.收款单状态.待确认,
                        Source = sourceFromOrder,
                        CreatedBy = soOrder.OrderCreatorSysNo,
                        CreatedDate = DateTime.Now,
                        LastUpdateBy = soOrder.OrderCreatorSysNo,
                        LastUpdateDate = DateTime.Now,
                        IncomeAmount = soOrder.CashPay,
                        IncomeType = 10,
                        SourceSysNo = soOrder.SysNo
                    };
                    rv.SysNo = Hyt.DataAccess.Finance.IFnReceiptVoucherDao.Instance.Insert(rv);
                    entity = rv;
                }
            }
            receiptVoucherItem.ReceiptVoucherSysNo = entity.SysNo;//收款单编号
            receiptVoucherItem.TransactionSysNo = entity.TransactionSysNo;
            receiptVoucherItem.Status = (int)Hyt.Model.WorkflowStatus.FinanceStatus.收款单明细状态.有效;
            entity.ReceivedAmount += receiptVoucherItem.Amount;

            string onlineStatus = Constant.OlineStatusType.待审核;
            int _payStatus = (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单支付状态.已支付;
            int _status = (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.待审核;

            switch (soOrder.Status)
            {
                case (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.待审核:
                    onlineStatus = Constant.OlineStatusType.待审核;
                    break;
                default:
                    onlineStatus = Constant.OlineStatusType.待出库; //Status
                    _status = (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.待创建出库单;
                    break;
            }

            SoOrderBo.Instance.UpdatePayStatusAndOnlineStatusAndStatus(orderID, _payStatus, onlineStatus, _status, customsPayStatus, tradeCheckStatus, onlinePayment.PaymentTypeSysNo);
            var itemSysNo = Hyt.DataAccess.Finance.IFnReceiptVoucherItemDao.Instance.Insert(receiptVoucherItem);
            Hyt.DataAccess.Finance.IFnReceiptVoucherDao.Instance.Update(entity);

            #endregion


            return true;
        }
        /// <summary>
        /// 商城发货
        /// </summary>
        /// <param name="mallType">商城类型</param>
        /// <param name="companyCode">商城物流公司代码</param>
        /// <param name="expressNo">快递编号</param>
        /// <param name="mallOrderId">商城订单号</param>
        /// <returns></returns>
        /// <remarks>2016-7-16 杨浩 创建</remarks>
        public Result MallSendDelivery(int mallType, string companyCode, string expressNo, string mallOrderId)
        {
            var result = new Result
            {
                Status = true
            };
            var para = new Extra.UpGrade.Model.DeliveryParameters();

            para.CompanyCode = companyCode;
            para.HytExpressNo = expressNo;
            para.MallOrderId = mallOrderId;

            var upGradeInstance = Extra.UpGrade.Provider.UpGradeProvider.GetInstance(mallType);
            result = upGradeInstance.SendDelivery(para, null);
            
            return result;
        }
        /// <summary>
        /// 获取分销商商城系统编号
        /// </summary>
        /// <param name="webshopID">ERP店铺代码</param>
        /// <returns></returns>
        /// <remarks>2017-3-8 杨浩 创建</remarks>
        private int GetDealerMallSysNo(string webshopID)
        {
            int dealerMallSysNo = 0;
            switch (webshopID)
            {
                case "JOS_7":
                    dealerMallSysNo = 15; //京东
                    break;
                case "SUNING_3":
                    dealerMallSysNo = 14; //苏宁
                    break;
                case "YHD_6":
                    dealerMallSysNo = 16;//一号店
                    break;
                case "TOP_2":
                    dealerMallSysNo = 12;//淘宝
                    break;
                case "Gome_13":
                    dealerMallSysNo = 17;//国美在线
                    break;
                case "TOP_14":
                    dealerMallSysNo = 18;//淘宝
                    break;
                case "JOS_15": //京东
                    dealerMallSysNo = 21;
                    break;           
                case "TOP_9": //淘宝店铺-信营购
                    dealerMallSysNo = 23;
                    break;
                case "JOS_16": //京东
                    dealerMallSysNo = 26;
                    break;
                case "JOS_17": //京东
                    dealerMallSysNo = 27;
                    break;
                case "JOS_19": //京东
                    dealerMallSysNo = 29;
                    break;
                case "SUNING_18"://苏宁
                    dealerMallSysNo = 28;
                    break;
                case "SUNING_20"://苏宁
                     dealerMallSysNo = 31;
                    break;

            }
            return dealerMallSysNo;
        }
        /// <summary>
        /// 导入erp订单100的订单
        /// </summary>
        /// <returns></returns>
        /// <remarks>2017-2-27 杨浩 创建</remarks>
        public Result  ImportERPOrder100()
        {
            int dealerSysNo = 29;
            var result = new Result() { Status = true };

            using (var service = new Pisen.Framework.Service.Proxy.ServiceProxy<Grand.Platform.Api.Contract.IMallService>())
            {
                var request = new Grand.Platform.Api.Contract.DataContract.GetOrder100Request()
                {
                    CurrentPage = 1,
                    PageSize = 20,
                    FTransactionStatus = 38192,//已付款
                    FSiteOrderDate = DateTime.Now.AddDays(-5),
                    FSiteOrderDateEnd = DateTime.Now.AddDays(1),
                    FWebshopID = "JOS_7",
                };

                var pager = new Pager<PdProduct>() { PageSize = 999999, CurrentPage = 1 };
                pager = BLL.Product.PdProductBo.Instance.GetPdProductList(pager);

                var webshopIds = new string[] { "SUNING_3", "YHD_6", "JOS_7","JOS_19", "TOP_2", "Gome_13", "TOP_14", "JOS_15", "TOP_9", "JOS_16", "JOS_17", "SUNING_18","SUNING_20" };
                foreach (var webshopId in webshopIds)
                {
                    var upOrderDic = new Dictionary<string, List<UpGradeOrderItem>>();

                    request.FWebshopID = webshopId;
                    request.CurrentPage = 1;
                    var response = service.Channel.GetOrder100(request);

                    #region 店铺

                    int dealerMallSysNo = GetDealerMallSysNo(webshopId);
                    var dealerMall = DsOrderBo.Instance.GetDsDealerMall(dealerMallSysNo);
                    if (dealerMall == null)
                    {
                        result.Status = false;
                        result.Message = "分销商城编号【" + dealerMallSysNo + "】还没有在系统里配置，请配置完再重试！";
                        return result;
                    }

                    var operatorName = dealerMall != null ? dealerMall.ShopName : string.Empty;

                    dealerSysNo = dealerMall.DealerSysNo;
                    #endregion

                    #region 遍历下载订单100
                    while (true)
                    {
                        bool isBreak = false;
                        if (response.IsError)
                        {
                            result.Status = false;
                            result.Message = response.ErrMsg;
                            return result;
                        }

                        if (response.ICWeb2ERPOrders == null || response.ICWeb2ERPOrders.Count <= 0)
                        {
                            result.Status = false;
                            result.Message = "无订单导入！";
                            break;
                        }

                        var orderReceives = new Dictionary<string, BsArea>();
                        foreach (var order in response.ICWeb2ERPOrders)
                        {
                            var areaInfo = BLL.Basic.BasicAreaBo.Instance.GetYouZanMatchDistrict(order.FDeliveryCity, order.FDeliveryDistrict);
                            if (areaInfo == null)
                            {
                                result.Status = false;
                                result.Message = "店铺【" + webshopId + "】 订单号【" + order.FSiteOrderID + "】 省【" + order.FDeliveryProvince + "】城市【" + order.FDeliveryCity + "】地区【" + order.FDeliveryDistrict + "】在系统中不存在，请到基础管理->地区信息管理添加再试！";

                                return result;
                            }
                            orderReceives.Add(order.FOrderID, areaInfo);
                        }

                        #region 遍历订单
                        foreach (var order in response.ICWeb2ERPOrders)
                        {
                            if (!DsOrderBo.Instance.ExistsDsOrder(dealerMallSysNo, order.FSiteOrderID))
                            {
                                foreach (ICWeb2ERPOrdersEntry mitem in order.ICWeb2ERPOrdersEntryList)
                                {
                                    string code = string.IsNullOrWhiteSpace(mitem.FNumber) ? "" : mitem.FNumber;
                                    if (!pager.Rows.Any(x => x.ErpCode == code.Trim()))
                                    {
                                        result.Status = false;
                                        result.Message = "店铺代码【" + webshopId + "】订单(" + mitem.FSiteOrderID + ")中的商家编码【" + mitem.FOuterID + "】物料代码:" + mitem.FNumber + " 在系统中不存在！";
                                        return result;
                                    }
                                }

                                var defaultorder = order;
                                Hyt.Model.CrCustomer cr = null;
                                var isNewUser = true;
                                string strPassword = "123456";//初始密码
                                var options = new TransactionOptions
                                {
                                    IsolationLevel = IsolationLevel.ReadCommitted,
                                    Timeout = TransactionManager.DefaultTimeout
                                };

                                var areaInfo = orderReceives[order.FOrderID];
                                var AreaInfo = areaInfo;
                                #region 会员
                                using (var trancustomer = new System.Transactions.TransactionScope(TransactionScopeOption.Required, options))//会员创建事物
                                {
                                    var customerlst = Hyt.BLL.Order.SoOrderBo.Instance.SearchCustomer(defaultorder.FMobile);
                                    if (customerlst != null && customerlst.Count > 0)
                                    {
                                        cr = customerlst.First();
                                        isNewUser = false;
                                    }
                                    else //创建会员
                                    {
                                        cr = new Model.CrCustomer()
                                        {
                                            Account = defaultorder.FMobile,
                                            MobilePhoneNumber = defaultorder.FMobile,
                                            AreaSysNo = areaInfo.SysNo,
                                            Gender = (int)Hyt.Model.WorkflowStatus.CustomerStatus.性别.保密,
                                            EmailStatus = (int)Hyt.Model.WorkflowStatus.CustomerStatus.邮箱状态.未验证,
                                            LevelSysNo = Hyt.Model.SystemPredefined.CustomerLevel.初级,
                                            Name = defaultorder.FConsignee,
                                            NickName = defaultorder.FBuyerNick,
                                            RegisterDate = DateTime.Now,
                                            Password = strPassword, // Hyt.Util.EncryptionUtil.EncryptWithMd5AndSalt(strPassword), 余勇修改 2014-09-12
                                            Status = (int)Hyt.Model.WorkflowStatus.CustomerStatus.会员状态.有效,
                                            MobilePhoneStatus = (int)Hyt.Model.WorkflowStatus.CustomerStatus.手机状态.未验证,
                                            RegisterSource = (int)Hyt.Model.WorkflowStatus.CustomerStatus.注册来源.分销工具,
                                            RegisterSourceSysNo = dealerSysNo.ToString(),
                                            StreetAddress = defaultorder.FDeliveryAddress,
                                            IsReceiveShortMessage = (int)CustomerStatus.是否接收短信.是,
                                            IsReceiveEmail = (int)CustomerStatus.是否接收邮件.是,
                                            LastLoginDate = DateTime.Now,
                                            Birthday = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                                            CreatedDate = DateTime.Now,
                                        };
                                        Hyt.Model.CrReceiveAddress crr = new Model.CrReceiveAddress()
                                        {
                                            AreaSysNo = areaInfo.SysNo,
                                            Name = defaultorder.FConsignee,
                                            MobilePhoneNumber = defaultorder.FMobile,
                                            StreetAddress = defaultorder.FDeliveryAddress,
                                            IsDefault = 1
                                        };
                                        Hyt.BLL.Order.SoOrderBo.Instance.CreateCustomer(cr, crr);
                                    }
                                    trancustomer.Complete();//会员创建事物
                                }
                                if (cr == null || cr.SysNo < 1)
                                {
                                    result.Status = false;
                                    result.Message = "会员信息读取失败";
                                    return result;
                                }
                                #endregion

                                #region 订单及订单明细
                                bool IsSelfSend = false;//是否自己的物流来配送

                                Hyt.Model.SoOrder hytorder = new Model.SoOrder()
                                {
                                    CustomerSysNo = cr.SysNo,
                                    OrderNo = BLL.Basic.ReceiptNumberBo.Instance.GetOrderNo(),
                                    LevelSysNo = cr.LevelSysNo,
                                    CustomerMessage = defaultorder.FBuyerMessage,
                                    InternalRemarks = defaultorder.FSellerMessage,
                                    CreateDate = (DateTime)defaultorder.FSiteOrderDate,//FCreatedate,
                                    Status = (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.待审核,
                                    OnlineStatus = Constant.OlineStatusType.待审核,
                                    SalesType = (int)Hyt.Model.WorkflowStatus.OrderStatus.销售方式.经销订单,
                                    OrderSource = (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单来源.分销商升舱,
                                    OrderSourceSysNo = dealerMallSysNo,//下单来源为 分销商商城编号
                                    PayStatus = 20,
                                    DeliveryTypeSysNo = Hyt.Model.SystemPredefined.DeliveryType.第三方快递,
                                    PayTypeSysNo = Hyt.Model.SystemPredefined.PaymentType.分销商预存,
                                    FreightAmount = defaultorder.FTotalFreight,
                                    AuditorDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                                    CancelDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                                    LastUpdateDate = DateTime.Now,
                                    Stamp = DateTime.Now,
                                    DealerSysNo = dealerSysNo,
                                    ProductDiscountAmount = order.FDiscountFee,
                                    OrderDiscountAmount = order.FBillDiscountFee,

                                };
                                var warehouse = Hyt.BLL.Warehouse.WhWarehouseBo.Instance.GetWhWareHouse(areaInfo.SysNo, null, hytorder.DeliveryTypeSysNo, Hyt.Model.WorkflowStatus.WarehouseStatus.仓库状态.启用).FirstOrDefault();
                                if (warehouse != null)
                                {
                                    hytorder.DefaultWarehouseSysNo = warehouse.SysNo;
                                }
                                else
                                {
                                    hytorder.DefaultWarehouseSysNo = 23;
                                }


                                var hytreceive = new Model.SoReceiveAddress()
                                {
                                    IDCardNo = "",
                                    AreaSysNo = areaInfo.SysNo,
                                    Name = defaultorder.FConsignee,
                                    MobilePhoneNumber = defaultorder.FMobile,
                                    StreetAddress = defaultorder.FDeliveryAddress,
                                    PhoneNumber = defaultorder.FMobile,
                                };
                                List<Hyt.Model.SoOrderItem> hytitems = new List<Model.SoOrderItem>();


                                if (!upOrderDic.ContainsKey(order.FSiteOrderID))
                                    upOrderDic.Add(order.FSiteOrderID, new List<UpGradeOrderItem>());

                                decimal payMoney = 0;
                                bool isCombineProduct = false;//是否合并相同产品的明细

                                foreach (var mitem in order.ICWeb2ERPOrdersEntryList)
                                {
                                    if (mitem.FNumber == null)
                                    {
                                        result.Status = false;
                                        result.Message = "ERP网上订单号【" + order.FSiteOrderID + "】产品代码为空！";
                                        return result;
                                    }
                                    var productInfo = pager.Rows.Where(x => x.ErpCode == mitem.FNumber.Trim()).FirstOrDefault();
                                    decimal price = mitem.FPrice;// Hyt.BLL.Product.PdPriceBo.Instance.GetUserRankPrice(productInfo.SysNo, cr.LevelSysNo);
                                    if (productInfo == null)
                                    {
                                        result.Status = false;
                                        result.Message = "ERP网上订单号【" + order.FSiteOrderID + "】产品编码【" + mitem.FNumber + "】在系统中不存在！";
                                        return result;
                                    }
                                    Hyt.Model.SoOrderItem si = hytitems.FirstOrDefault(m => m.ProductSysNo == productInfo.SysNo);

                                    upOrderDic[order.FSiteOrderID].Add(new UpGradeOrderItem()
                                    {
                                        MallProductCode = mitem.FSKUID,
                                        MallProductName = productInfo.ProductName,
                                        MallProductAttrId = mitem.FAttrCode,
                                        MallProductAttrs = "",
                                        OrderId = order.FWebshopID,
                                        MallPrice = mitem.FPrice,
                                        MallAmount = mitem.FAmount,
                                        Quantity = mitem.FQuantity,
                                        DiscountFee = mitem.FDiscountFee,
                                        MallOrderItemId = mitem.FItemID,
                                        HytProductSysNo = productInfo.SysNo,
                                    });


                                    if (!isCombineProduct || si == null)//不允许相同产品明细合并或者产品不再购物车中
                                    {
                                        #region 计算价格信息
                                        si = new Model.SoOrderItem()
                                        {
                                            ProductSysNo = productInfo.SysNo,
                                            ProductName = productInfo.ProductName,
                                            Quantity = mitem.FQuantity,
                                            OriginalPrice = price,
                                            SalesUnitPrice = mitem.FPrice,
                                            SalesAmount = mitem.FAmount, //price * mitem.FQuantity,
                                            ProductSalesType = (int)CustomerStatus.商品销售类型.普通,
                                            DiscountAmount = mitem.FDiscountFee,
                                        };
                                        #endregion

                                        Hyt.BLL.MallSeller.DsOrderBo.Instance.SetProductAssociation(dealerMallSysNo, mitem.FNumber.Trim(), mitem.FAttrCode, productInfo.SysNo);  //非赠品设置商城产品与分销产品映射

                                        hytitems.Add(si);//加入购物车
                                        payMoney += si.SalesAmount;

                                    }
                                    else
                                    {
                                        si.Quantity += mitem.FQuantity;
                                        si.SalesAmount += mitem.FAmount - mitem.FDiscountFee;
                                        payMoney += mitem.FAmount;
                                    }
                                }


                                hytorder.OrderAmount = order.Famount;// payMoney;
                                hytorder.CashPay = hytorder.OrderAmount - order.FDiscountFee;
                                hytorder.ProductAmount = payMoney;
                                #endregion

                                //取得订单图片标识
                                hytorder.ImgFlag = DsOrderBo.Instance.GetImgFlag(dealerMallSysNo,
                                    "", "", null);
                                using (var tran = new System.Transactions.TransactionScope(TransactionScopeOption.Required, options))
                                {
                                    #region 数据提交
                                    var r = Hyt.BLL.Order.SoOrderBo.Instance.ImportSoOrder(hytorder, hytreceive, hytitems.ToArray(), operatorName);

                                    result.Status = r.Status;
                                    result.StatusCode = r.StatusCode;
                                    result.Message = r.Message;//事物编号
                                    //写入分销商订单信息
                                    if (r.Status)
                                    {
                                        #region 升舱订单表

                                        // 获取分销商城类型
                                        var mall = DsOrderBo.Instance.GetDsDealerMall(dealerMallSysNo);
                                        var mallType = DsMallTypeBo.Instance.GetDsMallType(mall.MallTypeSysNo);
                                        var dso = new Hyt.Model.DsOrder()
                                        {
                                            City = order.FDeliveryCity,
                                            County = order.FDeliveryDistrict,
                                            Province = order.FDeliveryProvince,
                                            StreetAddress = order.FDeliveryAddress,
                                            BuyerNick = order.FBuyerNick,
                                            DealerMallSysNo = dealerMallSysNo,
                                            DiscountAmount = order.FBillDiscountFee,
                                            MallOrderId = order.FSiteOrderID,
                                            PostFee = defaultorder.FTotalFreight,
                                            Payment = order.Famount,
                                            ServiceFee = 0,
                                            PayTime = (DateTime)order.FSitePayDate,
                                            UpgradeTime = DateTime.Now,
                                            Status = (int)Hyt.Model.WorkflowStatus.DistributionStatus.升舱订单状态.升舱中,
                                            DeliveryTime = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                                            SignTime = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                                            LastCallbackTime = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                                        };
                                        var cbs = new List<Hyt.Model.Transfer.CBDsOrderItem>();
                                        var orderItems = upOrderDic[order.FSiteOrderID];
                                        foreach (var mii in orderItems)
                                        {
                                            Hyt.Model.DsOrderItem n = new Model.DsOrderItem()
                                            {
                                                Quantity = mii.Quantity,
                                                Price = mii.MallPrice,
                                                MallProductId = mii.MallProductCode,
                                                MallProductName = mii.MallProductName,
                                                DiscountAmount = mii.DiscountFee,
                                                MallProductAttribute = mii.MallProductAttrs
                                            };

                                            if (!string.IsNullOrEmpty(mii.MallOrderItemId))//如果数据转换错误就不写入淘宝订单明细编号
                                            {
                                                try
                                                {
                                                    n.MallItemNo = long.Parse(mii.MallOrderItemId);
                                                }
                                                catch { }
                                            }
                                            Hyt.Model.Transfer.CBDsOrderItem f = new Model.Transfer.CBDsOrderItem(n);

                                            mii.HytOrderItem = hytitems.Where(x => x.ProductSysNo == mii.HytProductSysNo).FirstOrDefault();

                                            f.CurrectDsOrderItemAssociations = new DsOrderItemAssociation()//升仓明细关联
                                            {
                                                OrderTransactionSysNo = result.Message,//事物编号
                                                SoOrderItemSysNo = mii.HytOrderItem.SysNo                //明细编号
                                            };
                                            cbs.Add(f);
                                        }
                                        DsOrderBo.Instance.SaveDsOrder(dso, cbs, r.StatusCode);

                                        #endregion
                                        if (hytorder.PayStatus == (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单支付状态.已支付 && hytorder.CashPay > 0)
                                        {

                                            var adminSysNo = BLL.Authentication.AdminAuthenticationBo.Instance.IsLogin ? BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo : 0;
                                            Hyt.BLL.MallSeller.DsOrderBo.Instance.FreezeDsPrePayment(r.StatusCode, dealerSysNo, hytorder.CashPay, adminSysNo, order.FSitePayDate);//冻结金额
                                        }
                                    }
                                    tran.Complete();
                                    #endregion
                                }
                            }
                        }
                        #endregion
                        if (isBreak)
                            break;

                        if (response.UseHasNext)
                        {
                            request.CurrentPage++;
                            response = service.Channel.GetOrder100(request);
                        }
                        else
                        {
                            break;
                        }
                    }
                    #endregion
                }
            }
            return result;
        }

        /// <summary>
        /// 导入三方商城订单
        /// </summary>
        /// <returns></returns>
        /// <remarks>2017-11-02 杨浩 创建</remarks>
        public Result ImportDsMallOrder()
        {
            var result = new Result() { Status = true };
            int[] mallTypes =
            { 
                (int)DistributionStatus.商城类型预定义.海带网,
                (int)DistributionStatus.商城类型预定义.国内货栈,
                (int)DistributionStatus.商城类型预定义.格格家
            };

            foreach (int mallType in mallTypes)
            {
                var mallList=BLL.Distribution.DsDealerMallBo.Instance.GetDealerMallByMallTypeSysNo(mallType);
                foreach (var mallInfo in mallList)
                {
                    #region 同步时间设置
                    var param = new OrderParameters();                
                    var threeMallSyncLogInfo = BLL.Distribution.DsThreeMallSyncLogBo.Instance.GetThreeMallSyncLogInfo(mallInfo.SysNo,10);
                    if (threeMallSyncLogInfo == null)
                    {
                        threeMallSyncLogInfo = new Model.Stores.DsThreeMallSyncLog();                     
                        threeMallSyncLogInfo.MallSysNo = mallInfo.SysNo;
                        threeMallSyncLogInfo.SyncType = 10;
                        param.StartDate = DateTime.Now.AddDays(-4);                                            
                    }
                    else                   
                        param.StartDate = threeMallSyncLogInfo.LastSyncTime;          
                    
                    threeMallSyncLogInfo.LastSyncTime = DateTime.Now;

                    param.StartDate = param.StartDate.AddHours(-2);
                    param.EndDate = DateTime.Now.AddHours(2);

                    //param.StartDate = DateTime.Parse("2018-04-17");
                    //param.EndDate = DateTime.Parse("2018-04-18");
                    #endregion

                    var appInfo = BLL.Distribution.DsDealerAppBo.Instance.GetDsDealerApp(mallInfo.DealerAppSysNo);
                    var auth = new AuthorizationParameters()
                    {
                        MallType = mallType,
                        ShopAccount =mallInfo.ShopAccount,
                        DealerApp = appInfo,
                        AuthorizationCode =mallInfo.AuthCode,
                        DealerMall = mallInfo,
                    };
                 
                    #region 记时
                    string syncData = "";
                    var watch = new Stopwatch();
                    watch.Start();
                    try
                    {                                            
                        //调用接口,执行同步
                        var _result = UpGradeProvider.GetInstance(mallType).GetOrderList(param, auth);

                        if (!_result.Status&&(_result.Data == null || _result.Data.Count <= 0))
                        {
                            result.StatusCode = _result.StatusCode;
                            result.Status = false;
                            result.Message = string.IsNullOrWhiteSpace(_result.Message) ? "无三方订单导入！" : _result.Message;
                        }
                        else
                        {
                            result = ImportMallOrder(_result.Data, null, mallInfo.DefaultWarehouse);                          
                        }                                                              
                        watch.Stop();
                    }
                    catch (Exception ex)
                    {
                        result.StatusCode = 0;
                        result.Status = false;
                        result.Message = ex.Message;
                    }
                
                    #endregion

                    #region 导入失败写入日志
                    if (!result.Status && result.StatusCode != 9999)
                    {                      
                        var entity = new DsDealerLog();

                        var newResult = result as Hyt.Model.Result<UpGradeOrder>;

                        UpGradeOrder orderInfo = null;

                        if (newResult != null)
                            orderInfo = newResult.Data;

                        entity.MallTypeSysNo = auth.MallType;
                        entity.MallSysNo = mallInfo.SysNo;
                        entity.CreatedDate = DateTime.Now;
                        entity.CreatedBy = 0;
                        entity.LastUpdateBy = 0;
                        entity.LastUpdateDate = DateTime.Now;
                        entity.LogContent = syncData;
                        entity.Message = result.Message;
                        entity.Status = 10;

                        if (orderInfo != null)
                        {
                            syncData =LitJson.JsonMapper.ToJson(orderInfo);
                            entity.LogContent = syncData;
                            entity.MallOrderId = orderInfo.MallOrderBuyer.MallOrderId;
                            if(!BLL.MallSeller.DsDealerLogBo.Instance.ChekMallOrderId(entity.MallOrderId,10,mallInfo.SysNo))                            
                                BLL.MallSeller.DsDealerLogBo.Instance.Insert(entity);                                                                                
                        }else                                                                                     
                            BLL.MallSeller.DsDealerLogBo.Instance.Insert(entity);
                  
                    }
                    #endregion

                    #region 更新最后同步日期
                    if (result.Status)
                    {
                        if (threeMallSyncLogInfo.SysNo <= 0)
                            BLL.Distribution.DsThreeMallSyncLogBo.Instance.Add(threeMallSyncLogInfo);
                        else
                            BLL.Distribution.DsThreeMallSyncLogBo.Instance.Update(threeMallSyncLogInfo);
                    }
                    
                    #endregion
                }
            }

            return result;
        }
        /// <summary>
        /// 导入三方订单
        /// </summary>
        /// <returns></returns>
        /// <remarks>2017-6-20 罗勤尧 创建</remarks>
        public Result ImportERPOrderHaiDai(HaiOrderParameters parameter)
        {
            //1、判断已匹配的区域编号是否存在
            //2、判断已匹配的商城商品编号是否存在
            //3、B店判断已匹配商品编号是否已维护当前分销商价格
            //4、若该订单的收货手机已在商城注册，则直接使用该商城账号创建商城订单，若该收货手机未注册商城，首先使用该手机注册商城。
            //5、若C店订单，导入后商城订单金额为0，商品明细价格为0，同时自动产生一条收款金额为0的收款单
            //6、若B店订单，导入后商城订单商品明细价格为商城系统维护的升舱结算价格，同时产生一条收款金额为结算金额的收款单
            //7、发送升舱成功短信，若新注册用户，同时发送商城账号密码。                    
            var result = new Result() { Status = true };
            //var customerlsttest = Hyt.BLL.Order.SoOrderBo.Instance.SearchCustomer("27331754");
            var parameters = new OrderParameters();
            List<string> sysNos = null;
            var _result = new Model.Result<List<UpGradeOrder>>() { Status = true, Message = "", Data =new List<UpGradeOrder>(), StatusCode = 0 };
            //var g = new Extra.UpGrade.UpGrades.HaiDaiUpGrade();
            var g = Extra.UpGrade.Provider.UpGradeProvider.GetInstance(parameter.Type);
            AuthorizationParameters au = new AuthorizationParameters();
            au.MallType = parameter.Type;
            //如果是根据订单号查询
            if (parameter.Status)
            {
                if (string.IsNullOrWhiteSpace(parameter.OrderList))
                {
                    result.Status = false;
                    result.Message = "请输入订单号！";
                    return result;
                }
                try {
                    string orders = parameter.OrderList.Trim().TrimEnd(',').TrimStart(',');
                    sysNos = new JavaScriptSerializer().Deserialize<List<string>>(orders);
                    foreach (string orderID in sysNos)
                    {
                        parameters.OrderID = orderID;
                        var orderResult = g.GetOrderDetail(parameters, au);
                        if (orderResult!=null)
                        {
                             _result.Data.Add(orderResult.Data);
                        }
                       
                    }
                }
                catch(Exception e)
                {
                    result.Status = false;
                    result.Message = "读取错误！请检查输入数据是否正确！";
                    return result;
                }
               
               
            }
            //根据时间范围查询
            else
            {
                parameters.EndDate = parameter.EndDate;
                parameters.StartDate = parameter.StartDate;
                parameters.PageIndex = 1;
                parameters.PageSize = 500;
                 _result = g.GetOrderList(parameters, au);
            }

            //var threeMallSyncLogInfo = BLL.Distribution.DsThreeMallSyncLogBo.Instance.GetThreeMallSyncLogInfo(408, (int)DistributionStatus.商城类型预定义.海带网, 0);

            //if (threeMallSyncLogInfo == null)
            //{
            //    threeMallSyncLogInfo = new Model.Stores.DsThreeMallSyncLog();
            //    threeMallSyncLogInfo.LastSyncTime = parameters.EndDate;
            //    threeMallSyncLogInfo.MallTypeSysNo = (int)DistributionStatus.商城类型预定义.海带网;
            //    threeMallSyncLogInfo.DealerSysNo = 408;
            //    threeMallSyncLogInfo.SyncType = 0;
            //    parameters.StartDate = DateTime.Now.AddYears(-100);

            //}
            //else
            //{
            //    parameters.StartDate = threeMallSyncLogInfo.LastSyncTime.AddDays(-10);
            //    threeMallSyncLogInfo.LastSyncTime = parameters.EndDate;
            //}
           
           
            //parameters.OrderID=_resultt.Data.
            //var _result = g.GetCombineOrders(parameters, null);

            if (_result.Data == null || _result.Data.Count <= 0)
            {
                result.Status = false;
                result.Message = string.IsNullOrWhiteSpace(_result.Message) ? "无三方订单导入！" : _result.Message;
                return result;
            }
            var productList = new List<PdProduct>();
            //第三方订单id集合
            List<int> orderlist = new List<int>();
            foreach (var order in _result.Data)
            {
                
                foreach (UpGradeOrderItem mitem in order.UpGradeOrderItems)
                {
                    if (parameter.Type == (int)DistributionStatus.商城类型预定义.海带网)
                    { orderlist.Add(int.Parse(mitem.OrderId)); }
                    //根据货号查询商品
                   
                    PdProduct pro = BLL.Product.PdProductBo.Instance.GetProductByErpCode(mitem.MallProductCode);
                    //if (parameter.Type == (int)DistributionStatus.商城类型预定义.格格家 && pro==null)
                    //{
                       
                    //   //商品条码去查
                    //    pro = IPdProductDao.Instance.GetEntityByBarcode(mitem.MallProductCode);
                    //}
                    if (pro != null)
                    {
                        productList.Add(pro);
                    }
                    else
                    {
                        result.Status = false;
                        result.Message = "三方订单(" + mitem.OrderId + ")中的商品编码:" + mitem.MallProductCode + ",在系统中不存在！";
                        return result;
                    }
                }
            }
            var pager = new Pager<PdProduct>() { PageSize = 999999, CurrentPage = 1 };
            //pager = BLL.Product.PdProductBo.Instance.GetPdProductList(pager);
            pager.TotalRows = productList.Count;
            pager.Rows = productList;
            pager.PageSize = productList.Count;
            pager.CurrentPage = 1;
            var orderReceives = new Dictionary<string, BsArea>();
            foreach (var order in _result.Data)
            {
                var areaInfo = BLL.Basic.BasicAreaBo.Instance.GetHaiDaiMatchDistrict(order.MallOrderReceive.City, order.MallOrderReceive.District);
                if (areaInfo == null)
                {
                    result.Status = false;
                    result.Message = "城市【" + order.MallOrderReceive.City + "】地区【" + order.MallOrderReceive.District + "】在系统中不存在，请到基础管理->地区信息管理添加再试！";
                    return result;
                }
                orderReceives.Add(order.MallOrderBuyer.MallOrderId, areaInfo);
            }
            foreach (var order in _result.Data)
            {
                if (!DsOrderBo.Instance.ExistsDsOrder(order.HytOrderDealer.DealerMallSysNo, order.MallOrderBuyer.MallOrderId))
                {
                    //foreach (UpGradeOrderItem mitem in order.UpGradeOrderItems)
                    //{
                    //    if (!pager.Rows.Any(x => x.ErpCode == mitem.MallProductCode))
                    //    {
                    //        result.Status = false;
                    //        result.Message = "海带订单(" + mitem.OrderId + ")中的商品编码:" + mitem.MallProductCode + ",在系统中不存在！";
                    //        return result;
                    //    }
                    //}
                    var defaultorder = order;
                    Hyt.Model.CrCustomer cr = null;
                    var isNewUser = true;
                    string strPassword = "123456";//初始密码
                    var options = new TransactionOptions
                    {
                        IsolationLevel = IsolationLevel.ReadCommitted,
                        Timeout = TransactionManager.DefaultTimeout
                    };
                    #region 会员
                    using (var trancustomer = new System.Transactions.TransactionScope(TransactionScopeOption.Required, options))//会员创建事物
                    {
                        //var customerlst = Hyt.BLL.Order.SoOrderBo.Instance.SearchCustomer(defaultorder.MallOrderReceive.Mobile);
                        var customerlst = Hyt.BLL.Order.SoOrderBo.Instance.SearchCustomer("27331754");
                        if (customerlst != null && customerlst.Count > 0)
                        {
                            cr = customerlst.First();
                            isNewUser = false;
                        }
                        else //创建会员
                        {
                            cr = new Model.CrCustomer()
                            {
                                Account = defaultorder.MallOrderReceive.Mobile,
                                MobilePhoneNumber = defaultorder.MallOrderReceive.Mobile,
                                //AreaSysNo = defaultorder.MallOrderReceive.AreaSysNo,
                                AreaSysNo = orderReceives[order.MallOrderBuyer.MallOrderId].SysNo,
                                Gender = (int)Hyt.Model.WorkflowStatus.CustomerStatus.性别.保密,
                                EmailStatus = (int)Hyt.Model.WorkflowStatus.CustomerStatus.邮箱状态.未验证,
                                LevelSysNo = Hyt.Model.SystemPredefined.CustomerLevel.初级,
                                Name = defaultorder.MallOrderReceive.ReceiveContact,
                                NickName = defaultorder.MallOrderBuyer.BuyerNick,
                                RegisterDate = DateTime.Now,
                                Password = strPassword, // Hyt.Util.EncryptionUtil.EncryptWithMd5AndSalt(strPassword), 余勇修改 2014-09-12
                                Status = (int)Hyt.Model.WorkflowStatus.CustomerStatus.会员状态.有效,
                                MobilePhoneStatus = (int)Hyt.Model.WorkflowStatus.CustomerStatus.手机状态.未验证,
                                RegisterSource = (int)Hyt.Model.WorkflowStatus.CustomerStatus.注册来源.分销工具,
                                RegisterSourceSysNo = defaultorder.HytOrderDealer.DealerSysNo.ToString(),
                                StreetAddress = defaultorder.MallOrderReceive.ReceiveAddress,
                                IsReceiveShortMessage = (int)CustomerStatus.是否接收短信.是,
                                IsReceiveEmail = (int)CustomerStatus.是否接收邮件.是,
                                LastLoginDate = DateTime.Now,
                                Birthday = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                                CreatedDate = DateTime.Now,
                            };
                            Hyt.Model.CrReceiveAddress crr = new Model.CrReceiveAddress()
                            {
                                AreaSysNo = defaultorder.MallOrderReceive.AreaSysNo,
                                Name = defaultorder.MallOrderReceive.ReceiveContact,
                                MobilePhoneNumber = defaultorder.MallOrderReceive.Mobile,
                                StreetAddress = defaultorder.MallOrderReceive.ReceiveAddress,
                                IsDefault = 1
                            };
                            Hyt.BLL.Order.SoOrderBo.Instance.CreateCustomer(cr, crr);
                        }
                        trancustomer.Complete();//会员创建事物
                    }
                    if (cr == null || cr.SysNo < 1)
                    {
                        result.Status = false;
                        result.Message = "会员信息读取失败";
                        return result;
                    }
                    #endregion

                    #region 订单及订单明细
                    bool IsSelfSend = false;//是否自己的物流来配送

                    var areaInfo = orderReceives[order.MallOrderBuyer.MallOrderId];// BLL.Basic.BasicAreaBo.Instance.GetYouZanMatchDistrict(order.MallOrderReceive.City, order.MallOrderReceive.District);
                    defaultorder.MallOrderReceive.AreaSysNo = areaInfo == null ? 0 : areaInfo.SysNo;
                    var AreaInfo = areaInfo;// Hyt.BLL.Basic.BasicAreaBo.Instance.GetArea(defaultorder.MallOrderReceive.AreaSysNo);
                    //if (AreaInfo != null)
                    //{
                    //    IsSelfSend = Hyt.BLL.Logistics.LgDeliveryScopeBo.Instance.IsInScope(AreaInfo.ParentSysNo, new Coordinate() { X = map_x, Y = map_y });
                    //}
                    int desysno = 0;
                    //if()
                    //{

                    //}
                    Hyt.Model.SoOrder hytorder = new Model.SoOrder()
                    {
                        CustomerSysNo = cr.SysNo,
                        OrderNo = BLL.Basic.ReceiptNumberBo.Instance.GetOrderNo(),
                        LevelSysNo = cr.LevelSysNo,
                        CustomerMessage = defaultorder.MallOrderBuyer.BuyerMessage,
                        InternalRemarks = defaultorder.MallOrderBuyer.SellerMessage,
                        CreateDate = DateTime.Now,
                        Status = (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.待审核,
                        OnlineStatus = Constant.OlineStatusType.待审核,
                        SalesType = (int)Hyt.Model.WorkflowStatus.OrderStatus.销售方式.经销订单,
                        OrderSource = (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单来源.分销商升舱,
                        OrderSourceSysNo = defaultorder.HytOrderDealer.DealerMallSysNo,//下单来源为 分销商商城编号
                        PayStatus = defaultorder.HytOrderDealer.HytPayStatus,
                        DeliveryTypeSysNo = IsSelfSend ? Hyt.Model.SystemPredefined.DeliveryType.百世汇通 : Hyt.Model.SystemPredefined.DeliveryType.百世汇通,
                        PayTypeSysNo = order.HytOrderDealer.HytPayType,
                        FreightAmount = defaultorder.MallOrderPayment.PostFee,
                        AuditorDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                        CancelDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                        LastUpdateDate = DateTime.Now,
                        Stamp = DateTime.Now,
                        //分销商编号
                        DealerSysNo = defaultorder.HytOrderDealer.DealerSysNo,

                    };
                    var warehouse = Hyt.BLL.Warehouse.WhWarehouseBo.Instance.GetWhWareHouse(AreaInfo.SysNo, null, hytorder.DeliveryTypeSysNo, Hyt.Model.WorkflowStatus.WarehouseStatus.仓库状态.启用).FirstOrDefault();
                    if (warehouse != null)
                    {
                        //
                        //hytorder.DefaultWarehouseSysNo = warehouse.SysNo;
                        if (parameter.Type == (int)DistributionStatus.商城类型预定义.海带网)
                        {
                            HaiDaiConfig config = UpGradeConfig.GetHaiDaiConfig();
                            hytorder.DefaultWarehouseSysNo = config.DefaultWarehouseSysNo;
                        }else
                        {
                            hytorder.DefaultWarehouseSysNo = 59;
                        }
                       
                    }
                    else
                    {
                        hytorder.DefaultWarehouseSysNo = 59;
                    }
                    if (parameter.Type == (int)DistributionStatus.商城类型预定义.海带网)
                    {
                        HaiDaiConfig config = UpGradeConfig.GetHaiDaiConfig();
                        hytorder.DefaultWarehouseSysNo = config.DefaultWarehouseSysNo;
                    }

                    var hytreceive = new Model.SoReceiveAddress()
                    {
                        IDCardNo = defaultorder.MallOrderReceive.IdCard,
                        AreaSysNo = AreaInfo.SysNo,
                        Name = defaultorder.MallOrderReceive.ReceiveContact,
                        MobilePhoneNumber = defaultorder.MallOrderReceive.Mobile,
                        StreetAddress = defaultorder.MallOrderReceive.ReceiveAddress,
                        PhoneNumber = defaultorder.MallOrderReceive.TelPhone
                    };
                    List<Hyt.Model.SoOrderItem> hytitems = new List<Model.SoOrderItem>();
                    decimal payMoney = 0;
                    int pcs=0;
                    bool isCombineProduct = false;//是否合并相同产品的明细

                    foreach (UpGradeOrderItem mitem in order.UpGradeOrderItems)
                    {
                        pcs++;
                        var productInfo = pager.Rows.Where(x => x.ErpCode == mitem.MallProductCode).FirstOrDefault();
                        if (productInfo != null)
                        {
                            mitem.HytProductSysNo = productInfo.SysNo;
                            mitem.HytProductName = productInfo.ProductName;
                            mitem.HytPrice = Hyt.BLL.Product.PdPriceBo.Instance.GetUserRankPrice(productInfo.SysNo, cr.LevelSysNo);

                        }

                        decimal price = mitem.HytPrice;
                        Hyt.Model.SoOrderItem si = hytitems.FirstOrDefault(m => m.ProductSysNo == mitem.HytProductSysNo);
                        if (!isCombineProduct || si == null)//不允许相同产品明细合并或者产品不再购物车中
                        {
                            if (mitem.ProductSalesType == (int)CustomerStatus.商品销售类型.赠品)//赠品
                            {
                                si = new Model.SoOrderItem()
                                {
                                    ProductSysNo = mitem.HytProductSysNo,
                                    ProductName = mitem.HytProductName,
                                    Quantity = mitem.Quantity,
                                    OriginalPrice = mitem.MallPrice,//带入作为原单价
                                    SalesUnitPrice = 0,
                                    SalesAmount = 0,
                                    ProductSalesType = (int)CustomerStatus.商品销售类型.普通,
                                    GroupName = "淘宝赠品"
                                };
                                mitem.MallPrice = 0;//升仓明细价格为0
                            }
                            else
                            {
                                #region 计算价格信息
                                if (order.HytOrderDealer.IsPreDeposit == (int)Hyt.Model.WorkflowStatus.DistributionStatus.商城是否使用预存款.是 && order.HytOrderDealer.IsSelfSupport == (int)Hyt.Model.WorkflowStatus.DistributionStatus.是否自营.否)
                                {
                                    price = Hyt.BLL.MallSeller.DsOrderBo.Instance.GetDsSpecialPrice(order.HytOrderDealer.DealerSysNo, mitem.HytProductSysNo);//再次获取一下分销商价格，防止客户端数据窜改
                                }
                                si = new Model.SoOrderItem()
                                {
                                    ProductSysNo = mitem.HytProductSysNo,
                                    ProductName = mitem.HytProductName,
                                    Quantity = mitem.Quantity,
                                    OriginalPrice = price,
                                    SalesUnitPrice = price,
                                    SalesAmount = price * mitem.Quantity,
                                    ProductSalesType = (int)CustomerStatus.商品销售类型.普通
                                };



                                if (order.HytOrderDealer.IsSelfSupport == (int)Hyt.Model.WorkflowStatus.DistributionStatus.是否自营.是)//自营存在改价格情况
                                {
                                    si.SalesAmount = mitem.MallAmount;
                                }
                                #endregion
                                Hyt.BLL.MallSeller.DsOrderBo.Instance.SetProductAssociation(order.HytOrderDealer.DealerMallSysNo, mitem.MallProductCode, mitem.MallProductAttrId, mitem.HytProductSysNo);  //非赠品设置商城产品与分销产品映射
                            }
                            hytitems.Add(si);//加入购物车
                            payMoney += si.SalesAmount;
                            mitem.HytOrderItem = si;//升仓订单明细与商城订单明细关联
                        }
                        else
                        {
                            si.Quantity += mitem.Quantity;
                            if (order.HytOrderDealer.IsSelfSupport == (int)Hyt.Model.WorkflowStatus.DistributionStatus.是否自营.是)//自营存在改价格情况
                            {
                                si.SalesAmount += mitem.MallAmount;
                                payMoney += mitem.MallAmount;
                            }
                            else
                            {
                                si.SalesAmount += si.SalesUnitPrice * mitem.Quantity;
                                payMoney += si.SalesUnitPrice * mitem.Quantity;
                            }
                        }
                        
                    }           
                    hytorder.OrderAmount = payMoney;
                    hytorder.CashPay = payMoney ;
                    hytorder.ProductAmount = payMoney ;
                    #endregion.

                    if (parameter.Type == (int)DistributionStatus.商城类型预定义.格格家)
                    {
                        payMoney = order.MallOrderPayment.Payment;
                        hytorder.OrderAmount = payMoney + 11 * pcs;
                        hytorder.CashPay = payMoney + 11 * pcs;
                        hytorder.ProductAmount = payMoney + 11 * pcs;
                    }

                    var dealerMall = DsOrderBo.Instance.GetDsDealerMall(defaultorder.HytOrderDealer.DealerMallSysNo);
                    var operatorName = dealerMall != null ? dealerMall.ShopName : string.Empty;


                    //取得订单图片标识
                    hytorder.ImgFlag = DsOrderBo.Instance.GetImgFlag(defaultorder.HytOrderDealer.DealerMallSysNo,
                        defaultorder.MallOrderBuyer.SellerMessage, defaultorder.MallOrderBuyer.SellerFlag,
                        defaultorder.UpGradeOrderItems);

                    using (var tran = new System.Transactions.TransactionScope(TransactionScopeOption.Required, options))
                    {
                        #region 数据提交
                        var r = Hyt.BLL.Order.SoOrderBo.Instance.ImportSoOrder(hytorder, hytreceive, hytitems.ToArray(), operatorName);
                        result.Status = r.Status;
                        result.StatusCode = r.StatusCode;
                        result.Message = r.Message;//事物编号
                        //写入分销商订单信息
                        if (r.Status)
                        {
                            #region 升舱订单表

                            // 获取分销商城类型
                            var mall = DsOrderBo.Instance.GetDsDealerMall(order.HytOrderDealer.DealerMallSysNo);
                            var mallType = DsMallTypeBo.Instance.GetDsMallType(mall.MallTypeSysNo);
                            string MallOrderId = "";
                            if (parameter.Type == (int)DistributionStatus.商城类型预定义.海带网)
                            {
                                MallOrderId = order.MallOrderBuyer.SN;
                            }else
                            {
                                MallOrderId = mallType.SysNo == (int)DistributionStatus.商城类型预定义.淘宝分销 ? order.MallOrderBuyer.MallPurchaseId : order.MallOrderBuyer.MallOrderId;
                            }
                            var dso = new Hyt.Model.DsOrder()
                            {
                                City = order.MallOrderReceive.City,
                                County = order.MallOrderReceive.District,
                                Province = order.MallOrderReceive.Province,
                                StreetAddress = order.MallOrderReceive.ReceiveAddress,
                                BuyerNick = order.MallOrderBuyer.BuyerNick,
                                DealerMallSysNo = order.HytOrderDealer.DealerMallSysNo,
                                DiscountAmount = order.MallOrderPayment.DiscountFee,
                                MallOrderId = MallOrderId,
                                PostFee = order.MallOrderPayment.PostFee,
                                Payment = order.MallOrderPayment.Payment,
                                ServiceFee = order.MallOrderPayment.ServiceFee,
                                PayTime = order.MallOrderPayment.PayTime,
                                UpgradeTime = DateTime.Now,
                                Status = (int)Hyt.Model.WorkflowStatus.DistributionStatus.升舱订单状态.升舱中,
                                DeliveryTime = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                                SignTime = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                                LastCallbackTime = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                            };
                            List<Hyt.Model.Transfer.CBDsOrderItem> cbs = new List<Hyt.Model.Transfer.CBDsOrderItem>();
                            foreach (UpGradeOrderItem mii in order.UpGradeOrderItems)
                            {
                                Hyt.Model.DsOrderItem n = new Model.DsOrderItem()
                                {
                                    Quantity = mii.Quantity,
                                    Price = mii.MallPrice,
                                    MallProductId = mii.MallProductCode,
                                    MallProductName = mii.MallProductName,
                                    DiscountAmount = mii.DiscountFee,
                                    MallProductAttribute = mii.MallProductAttrs
                                };

                                if (!string.IsNullOrEmpty(mii.MallOrderItemId))//如果数据转换错误就不写入淘宝订单明细编号
                                {
                                    try
                                    {
                                        n.MallItemNo = long.Parse(mii.MallOrderItemId);
                                    }
                                    catch { }
                                }
                                Hyt.Model.Transfer.CBDsOrderItem f = new Model.Transfer.CBDsOrderItem(n);
                                f.CurrectDsOrderItemAssociations = new DsOrderItemAssociation()//升仓明细关联
                                {
                                    OrderTransactionSysNo = mii.HytOrderItem.TransactionSysNo,//事物编号
                                    SoOrderItemSysNo = mii.HytOrderItem.SysNo                //明细编号
                                };
                                cbs.Add(f);
                            }
                            DsOrderBo.Instance.SaveDsOrder(dso, cbs, r.StatusCode);

                            #endregion
                            if (hytorder.PayStatus == (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单支付状态.已支付 && hytorder.CashPay > 0)
                            {
                                Hyt.BLL.MallSeller.DsOrderBo.Instance.FreezeDsPrePayment(r.StatusCode, defaultorder.HytOrderDealer.DealerSysNo, hytorder.CashPay, BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);//冻结金额
                            }
                        }
                        tran.Complete();
                        #endregion
                    }

                }
            }
            //if (result.Status && parameter.Type == (int)DistributionStatus.商城类型预定义.海带网)
            //{
            //    //接单成功
            //    try {
            //       var  gg= new Extra.UpGrade.UpGrades.HaiDaiUpGrade();
            //       var res = gg.GetMallOrderDetail(orderlist, "");
            //    }
            //    catch(Exception e)
            //    {
            //        BLL.Log.LocalLogBo.Instance.Write(e.StackTrace, "haidaiLog");
            //    }                
            //}
            return result;
        }

        /// <summary>
        /// 导入三方商城订单
        /// </summary>
        /// <returns></returns>
        /// <remarks>2017-11-02 罗勤瑶 创建</remarks>
        public Result ImportDsMallOrderNew()
        {

            var result = new Result() { Status = true };

            int[] mallTypes =
            { 
                (int)DistributionStatus.商城类型预定义.海带网,
                (int)DistributionStatus.商城类型预定义.国内货栈,
                (int)DistributionStatus.商城类型预定义.格格家
            };

            foreach (int mallType in mallTypes)
            {
                var mallList = BLL.Distribution.DsDealerMallBo.Instance.GetDealerMallByMallTypeSysNo(mallType);
                foreach (var mallInfo in mallList)
                {
                    #region 同步时间设置
                    var param = new OrderParameters();
                    var threeMallSyncLogInfo = BLL.Distribution.DsThreeMallSyncLogBo.Instance.GetThreeMallSyncLogInfo(mallInfo.SysNo, 10);
                    if (threeMallSyncLogInfo == null)
                    {
                        threeMallSyncLogInfo = new Model.Stores.DsThreeMallSyncLog();
                        threeMallSyncLogInfo.MallSysNo = mallInfo.SysNo;
                        threeMallSyncLogInfo.SyncType = 10;
                        param.StartDate = DateTime.Now.AddDays(-4);
                    }
                    else
                        param.StartDate = threeMallSyncLogInfo.LastSyncTime;

                    threeMallSyncLogInfo.LastSyncTime = DateTime.Now;

                    param.StartDate = param.StartDate.AddHours(-2);
                    param.EndDate = DateTime.Now.AddHours(2);

                    //param.StartDate = DateTime.Parse("2018-04-17");
                    //param.EndDate = DateTime.Parse("2018-04-18");
                    #endregion

                    var appInfo = BLL.Distribution.DsDealerAppBo.Instance.GetDsDealerApp(mallInfo.DealerAppSysNo);
                    var auth = new AuthorizationParameters()
                    {
                        MallType = mallType,
                        ShopAccount = mallInfo.ShopAccount,
                        DealerApp = appInfo,
                        AuthorizationCode = mallInfo.AuthCode,
                        DealerMall = mallInfo,
                    };

                    #region 记时
                    string syncData = "";
                    var watch = new Stopwatch();
                    watch.Start();
                    try
                    {
                        //调用接口,执行同步
                        var _result = UpGradeProvider.GetInstance(mallType).GetOrderList(param, auth);

                        if (!_result.Status && (_result.Data == null || _result.Data.Count <= 0))
                        {
                            result.StatusCode = _result.StatusCode;
                            result.Status = false;
                            result.Message = string.IsNullOrWhiteSpace(_result.Message) ? "无三方订单导入！" : _result.Message;
                        }
                        else
                        {
                            result = ImportMallOrder(_result.Data, null, mallInfo.DefaultWarehouse);
                        }
                        watch.Stop();
                    }
                    catch (Exception ex)
                    {
                        result.StatusCode = 0;
                        result.Status = false;
                        result.Message = ex.Message;
                    }

                    #endregion

                    #region 导入失败写入日志
                    if (!result.Status && result.StatusCode != 9999)
                    {
                        var entity = new DsDealerLog();

                        var newResult = result as Hyt.Model.Result<UpGradeOrder>;

                        UpGradeOrder orderInfo = null;

                        if (newResult != null)
                            orderInfo = newResult.Data;

                        entity.MallTypeSysNo = auth.MallType;
                        entity.MallSysNo = mallInfo.SysNo;
                        entity.CreatedDate = DateTime.Now;
                        entity.CreatedBy = 0;
                        entity.LastUpdateBy = 0;
                        entity.LastUpdateDate = DateTime.Now;
                        entity.LogContent = syncData;
                        entity.Message = result.Message;
                        entity.Status = 10;

                        if (orderInfo != null)
                        {
                            syncData = LitJson.JsonMapper.ToJson(orderInfo);
                            entity.LogContent = syncData;
                            entity.MallOrderId = orderInfo.MallOrderBuyer.MallOrderId;
                            if (!BLL.MallSeller.DsDealerLogBo.Instance.ChekMallOrderId(entity.MallOrderId, 10, mallInfo.SysNo))
                                BLL.MallSeller.DsDealerLogBo.Instance.Insert(entity);
                        }
                        else
                            BLL.MallSeller.DsDealerLogBo.Instance.Insert(entity);

                    }
                    #endregion

                    #region 更新最后同步日期
                    if (result.Status)
                    {
                        if (threeMallSyncLogInfo.SysNo <= 0)
                            BLL.Distribution.DsThreeMallSyncLogBo.Instance.Add(threeMallSyncLogInfo);
                        else
                            BLL.Distribution.DsThreeMallSyncLogBo.Instance.Update(threeMallSyncLogInfo);
                    }

                    #endregion
                }
            }

            return result;
        }
        /// <summary>
        /// 商城订单导入商城
        /// </summary>
        /// <param name="upGradeOrderList">商城订单集合</param>
        /// <param name="productList">产品集合</param>
        /// <param name="defaultWarehouse">默认仓库系统编号</param>
        /// <returns>订单导入结果</returns>
        /// <remarks>2016-7-13 杨浩 创建</remarks>
        public Hyt.Model.Result ImportMallOrder(IList<UpGradeOrder> upGradeOrderList, IList<PdProduct> productList, string defaultWarehouse = null)
        {
            var result = new Hyt.Model.Result<UpGradeOrder>() { Status = true };

            var orderReceives = new Dictionary<string, BsArea>();
            foreach (var order in upGradeOrderList)
            {
                var areaInfo = BLL.Basic.BasicAreaBo.Instance.GetYouZanMatchDistrict(order.MallOrderReceive.City, order.MallOrderReceive.District);
                if (areaInfo == null)
                {
                    result.Status = false;
                    result.Message = "城市【" + order.MallOrderReceive.City + "】地区【" + order.MallOrderReceive.District + "】在系统中不存在，请到基础管理->地区信息管理添加再试！";
                    result.Data = order;
                    return result;
                }
                orderReceives.Add(order.MallOrderBuyer.MallOrderId, areaInfo);
            }

            bool isRedProduct = productList == null; //是否读取产品

            foreach (var order in upGradeOrderList)
            {

                if (order.MallOrderPayment == null)
                {
                    result.Status = false;
                    result.Message = "订单(" + order.MallOrderBuyer.MallOrderId + ")支付信息没有！";
                    return result;
                }

                if (!DsOrderBo.Instance.ExistsDsOrder(order.HytOrderDealer.DealerMallSysNo, order.MallOrderBuyer.MallOrderId))
                {

                    if (isRedProduct)
                    {
                        var dic = order.UpGradeOrderItems.ToDictionary(x => x.MallProductCode);
                        var erpCodeList = dic.Keys.ToList();
                        productList = BLL.Product.PdProductBo.Instance.GetProductListByErpCode(erpCodeList);
                    }

                    foreach (UpGradeOrderItem mitem in order.UpGradeOrderItems)
                    {
                        var productInfo = productList.Where(x => x.ErpCode == mitem.MallProductCode).SingleOrDefault();
                        if (productInfo == null)
                        {
                            result.Status = false;
                            result.Message = "订单(" + order.MallOrderBuyer.MallOrderId + ")中的商品编码:" + mitem.MallProductCode + ",在系统中不存在！";
                            result.Data = order;
                            return result;
                        }
                    }

                    var defaultorder = order;
                    Hyt.Model.CrCustomer cr = null;

                    string strPassword = "123456";//初始密码
                    var options = new TransactionOptions
                    {
                        IsolationLevel = IsolationLevel.ReadCommitted,
                        Timeout = TransactionManager.DefaultTimeout
                    };

                    #region 会员
                    using (var trancustomer = new System.Transactions.TransactionScope(TransactionScopeOption.Required, options))//会员创建事物
                    {
                        var customerlst = Hyt.BLL.Order.SoOrderBo.Instance.SearchCustomer(defaultorder.MallOrderReceive.Mobile);
                        if (customerlst != null && customerlst.Count > 0)
                        {
                            cr = customerlst.First();
                        }
                        else //创建会员
                        {
                            cr = new Model.CrCustomer()
                            {
                                Account = defaultorder.MallOrderReceive.Mobile,
                                MobilePhoneNumber = defaultorder.MallOrderReceive.Mobile,
                                AreaSysNo = defaultorder.MallOrderReceive.AreaSysNo,
                                Gender = (int)Hyt.Model.WorkflowStatus.CustomerStatus.性别.保密,
                                EmailStatus = (int)Hyt.Model.WorkflowStatus.CustomerStatus.邮箱状态.未验证,
                                LevelSysNo = Hyt.Model.SystemPredefined.CustomerLevel.初级,
                                Name = defaultorder.MallOrderReceive.ReceiveContact,
                                NickName = defaultorder.MallOrderBuyer.BuyerNick,
                                RegisterDate = DateTime.Now,
                                Password = strPassword, // Hyt.Util.EncryptionUtil.EncryptWithMd5AndSalt(strPassword), 余勇修改 2014-09-12
                                Status = (int)Hyt.Model.WorkflowStatus.CustomerStatus.会员状态.有效,
                                MobilePhoneStatus = (int)Hyt.Model.WorkflowStatus.CustomerStatus.手机状态.未验证,
                                RegisterSource = (int)Hyt.Model.WorkflowStatus.CustomerStatus.注册来源.分销工具,
                                RegisterSourceSysNo = defaultorder.HytOrderDealer.DealerSysNo.ToString(),
                                StreetAddress = defaultorder.MallOrderReceive.ReceiveAddress,
                                IsReceiveShortMessage = (int)CustomerStatus.是否接收短信.是,
                                IsReceiveEmail = (int)CustomerStatus.是否接收邮件.是,
                                LastLoginDate = DateTime.Now,
                                Birthday = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                                CreatedDate = DateTime.Now,
                            };
                            Hyt.Model.CrReceiveAddress crr = new Model.CrReceiveAddress()
                            {
                                AreaSysNo = defaultorder.MallOrderReceive.AreaSysNo,
                                Name = defaultorder.MallOrderReceive.ReceiveContact,
                                MobilePhoneNumber = defaultorder.MallOrderReceive.Mobile,
                                StreetAddress = defaultorder.MallOrderReceive.ReceiveAddress,
                                IsDefault = 1
                            };
                            Hyt.BLL.Order.SoOrderBo.Instance.CreateCustomer(cr, crr);
                        }
                        trancustomer.Complete();//会员创建事物
                    }
                    if (cr == null || cr.SysNo < 1)
                    {
                        result.Status = false;
                        result.Message = "会员信息读取失败";
                        result.Data = order;
                        return result;
                    }
                    #endregion

                    #region 订单及订单明细

                    var areaInfo = orderReceives[order.MallOrderBuyer.MallOrderId];

                    defaultorder.MallOrderReceive.AreaSysNo = areaInfo == null ? 0 : areaInfo.SysNo;


                    var AreaInfo = areaInfo;

                    Hyt.Model.SoOrder hytorder = new Model.SoOrder()
                    {
                        CustomerSysNo = cr.SysNo,
                        OrderNo = BLL.Basic.ReceiptNumberBo.Instance.GetOrderNo(),
                        LevelSysNo = cr.LevelSysNo,
                        CustomerMessage = defaultorder.MallOrderBuyer.BuyerMessage,
                        InternalRemarks = defaultorder.MallOrderBuyer.SellerMessage,
                        CreateDate = DateTime.Now,
                        Status = (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.待审核,
                        OnlineStatus = Constant.OlineStatusType.待审核,
                        SalesType = (int)Hyt.Model.WorkflowStatus.OrderStatus.销售方式.经销订单,
                        OrderSource = (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单来源.分销商升舱,
                        OrderSourceSysNo = defaultorder.HytOrderDealer.DealerMallSysNo,//下单来源为 分销商商城编号
                        PayStatus = defaultorder.HytOrderDealer.HytPayStatus,
                        DeliveryTypeSysNo = 10,
                        PayTypeSysNo = order.HytOrderDealer.HytPayType,
                        FreightAmount = defaultorder.MallOrderPayment.PostFee,
                        AuditorDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                        CancelDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                        LastUpdateDate = DateTime.Now,
                        Stamp = DateTime.Now,
                        DealerSysNo = order.HytOrderDealer.DealerSysNo,
                        TaxFee = order.MallOrderPayment.TotalTaxAmount,

                    };

                    int defaultWarehouseSysNo = 59;
                    if (string.IsNullOrEmpty(defaultWarehouse))
                        defaultWarehouse = "";
                                       
                    var arryWp = defaultWarehouse.Split('_');

                    int.TryParse(arryWp[0], out defaultWarehouseSysNo);
                    hytorder.DefaultWarehouseSysNo = defaultWarehouseSysNo;
                    if (arryWp.Length > 1)
                    {
                        int deliveryTypeSysNo = 10;
                        int.TryParse(arryWp[1], out deliveryTypeSysNo);
                        hytorder.DeliveryTypeSysNo = deliveryTypeSysNo;
                    }

                    var hytreceive = new Model.SoReceiveAddress()
                    {
                        IDCardNo = defaultorder.MallOrderReceive.IdCard,
                        AreaSysNo = defaultorder.MallOrderReceive.AreaSysNo,
                        Name = defaultorder.MallOrderReceive.ReceiveContact,
                        MobilePhoneNumber = defaultorder.MallOrderReceive.Mobile,
                        StreetAddress = defaultorder.MallOrderReceive.ReceiveAddress,
                        PhoneNumber = defaultorder.MallOrderReceive.TelPhone
                    };
                    List<Hyt.Model.SoOrderItem> hytitems = new List<Model.SoOrderItem>();
                    decimal payMoney = 0;
                    bool isCombineProduct = false;//是否合并相同产品的明细

                    foreach (UpGradeOrderItem mitem in order.UpGradeOrderItems)
                    {
                        var productInfo = productList.Where(x => x.ErpCode == mitem.MallProductCode).SingleOrDefault();
                        if (productInfo != null)
                        {
                            mitem.HytProductSysNo = productInfo.SysNo;
                            mitem.HytProductName = productInfo.ProductName;
                            mitem.HytPrice = Hyt.BLL.Product.PdPriceBo.Instance.GetUserRankPrice(productInfo.SysNo, cr.LevelSysNo);

                        }

                        decimal price = mitem.HytPrice;
                        Hyt.Model.SoOrderItem si = hytitems.FirstOrDefault(m => m.ProductSysNo == mitem.HytProductSysNo);
                        if (!isCombineProduct || si == null)//不允许相同产品明细合并或者产品不再购物车中
                        {
                            if (mitem.ProductSalesType == (int)CustomerStatus.商品销售类型.赠品)//赠品
                            {
                                si = new Model.SoOrderItem()
                                {
                                    ProductSysNo = mitem.HytProductSysNo,
                                    ProductName = mitem.HytProductName,
                                    Quantity = mitem.Quantity,
                                    OriginalPrice = mitem.MallPrice,//带入作为原单价
                                    SalesUnitPrice = 0,
                                    SalesAmount = 0,
                                    ProductSalesType = (int)CustomerStatus.商品销售类型.普通,
                                    GroupName = "淘宝赠品"
                                };
                                mitem.MallPrice = 0;//升仓明细价格为0
                            }
                            else if (mitem.ProductSalesType == (int)CustomerStatus.商品销售类型.格格家)
                            {
                                si = new Model.SoOrderItem()
                                {
                                    ProductSysNo = mitem.HytProductSysNo,
                                    ProductName = mitem.HytProductName,
                                    Quantity = mitem.Quantity,
                                    OriginalPrice = mitem.MallPrice,//带入作为原单价
                                    SalesUnitPrice = mitem.MallPrice,//带入作为原单价
                                    SalesAmount = mitem.MallAmount,
                                    ProductSalesType = (int)CustomerStatus.商品销售类型.普通,
                                   
                                };
                               }
                            else
                            {
                                #region 计算价格信息
                                //if (order.HytOrderDealer.IsPreDeposit == (int)Hyt.Model.WorkflowStatus.DistributionStatus.商城是否使用预存款.是 && order.HytOrderDealer.IsSelfSupport == (int)Hyt.Model.WorkflowStatus.DistributionStatus.是否自营.否)
                                //{
                                //    price = Hyt.BLL.MallSeller.DsOrderBo.Instance.GetDsSpecialPrice(order.HytOrderDealer.DealerSysNo, mitem.HytProductSysNo);//再次获取一下分销商价格，防止客户端数据窜改
                                //}

                                si = new Model.SoOrderItem()
                                {
                                    ProductSysNo = mitem.HytProductSysNo,
                                    ProductName = mitem.HytProductName,
                                    Quantity = mitem.Quantity,
                                    OriginalPrice = price,
                                    SalesUnitPrice = price,
                                    SalesAmount = price * mitem.Quantity,
                                    ProductSalesType = (int)CustomerStatus.商品销售类型.普通
                                };

                                if (order.HytOrderDealer.IsSelfSupport == (int)Hyt.Model.WorkflowStatus.DistributionStatus.是否自营.是)//自营存在改价格情况
                                {
                                    si.SalesAmount = mitem.MallAmount;
                                    si.SalesUnitPrice = mitem.MallPrice;
                                }
                                #endregion
                                Hyt.BLL.MallSeller.DsOrderBo.Instance.SetProductAssociation(order.HytOrderDealer.DealerMallSysNo, mitem.MallProductCode, mitem.MallProductAttrId, mitem.HytProductSysNo);  //非赠品设置商城产品与分销产品映射
                            }

                            hytitems.Add(si);//加入购物车
                            payMoney += si.SalesAmount;
                            mitem.HytOrderItem = si;//升仓订单明细与商城订单明细关联
                        }
                        //else
                        //{
                        //    si.Quantity += mitem.Quantity;
                        //    if (order.HytOrderDealer.IsSelfSupport == (int)Hyt.Model.WorkflowStatus.DistributionStatus.是否自营.是)//自营存在改价格情况
                        //    {
                        //        si.SalesAmount += mitem.MallAmount;
                        //        payMoney += mitem.MallAmount;
                        //    }
                        //    else
                        //    {
                        //        si.SalesAmount += si.SalesUnitPrice * mitem.Quantity;
                        //        payMoney += si.SalesUnitPrice * mitem.Quantity;
                        //    }
                        //}
                    }


                    hytorder.OrderAmount = order.MallOrderPayment.Payment;
                    hytorder.CashPay = order.MallOrderPayment.Payment;
                    hytorder.ProductAmount = payMoney;
                    #endregion.

                    var dealerMall = DsOrderBo.Instance.GetDsDealerMall(defaultorder.HytOrderDealer.DealerMallSysNo);
                    var operatorName = dealerMall != null ? dealerMall.ShopName : string.Empty;

                    //取得订单图片标识
                    hytorder.ImgFlag = DsOrderBo.Instance.GetImgFlag(defaultorder.HytOrderDealer.DealerMallSysNo,
                        defaultorder.MallOrderBuyer.SellerMessage, defaultorder.MallOrderBuyer.SellerFlag,
                        defaultorder.UpGradeOrderItems);

                    using (var tran = new System.Transactions.TransactionScope(TransactionScopeOption.Required, options))
                    {
                        #region 数据提交
                        var r = Hyt.BLL.Order.SoOrderBo.Instance.ImportSoOrder(hytorder, hytreceive, hytitems.ToArray(), operatorName);
                        result.Status = r.Status;
                        result.StatusCode = r.StatusCode;
                        result.Message = r.Message;//事物编号
                        //写入分销商订单信息
                        if (r.Status)
                        {
                            #region 升舱订单表

                            // 获取分销商城类型
                            var mall = DsOrderBo.Instance.GetDsDealerMall(order.HytOrderDealer.DealerMallSysNo);
                            var mallType = DsMallTypeBo.Instance.GetDsMallType(mall.MallTypeSysNo);
                            var dso = new Hyt.Model.DsOrder()
                            {
                                City = order.MallOrderReceive.City,
                                County = order.MallOrderReceive.District,
                                Province = order.MallOrderReceive.Province,
                                StreetAddress = order.MallOrderReceive.ReceiveAddress,
                                BuyerNick = order.MallOrderBuyer.BuyerNick,
                                DealerMallSysNo = order.HytOrderDealer.DealerMallSysNo,
                                DiscountAmount = order.MallOrderPayment.DiscountFee,
                                MallOrderId = mallType.SysNo == (int)DistributionStatus.商城类型预定义.淘宝分销 ? order.MallOrderBuyer.MallPurchaseId : order.MallOrderBuyer.MallOrderId,
                                PostFee = order.MallOrderPayment.PostFee,
                                Payment = order.MallOrderPayment.Payment,
                                ServiceFee = order.MallOrderPayment.ServiceFee,
                                PayTime = order.MallOrderPayment.PayTime,
                                UpgradeTime = DateTime.Now,
                                Status = (int)Hyt.Model.WorkflowStatus.DistributionStatus.升舱订单状态.升舱中,
                                DeliveryTime = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                                SignTime = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                                LastCallbackTime = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                            };
                            List<Hyt.Model.Transfer.CBDsOrderItem> cbs = new List<Hyt.Model.Transfer.CBDsOrderItem>();
                            foreach (UpGradeOrderItem mii in order.UpGradeOrderItems)
                            {
                                Hyt.Model.DsOrderItem n = new Model.DsOrderItem()
                                {
                                    Quantity = mii.Quantity,
                                    Price = mii.MallPrice,
                                    MallProductId = mii.MallProductCode,
                                    MallProductName = mii.MallProductName,
                                    DiscountAmount = mii.DiscountFee,
                                    MallProductAttribute = mii.MallProductAttrs
                                };

                                if (!string.IsNullOrEmpty(mii.MallOrderItemId))//如果数据转换错误就不写入淘宝订单明细编号
                                {
                                    try
                                    {
                                        n.MallItemNo = long.Parse(mii.MallOrderItemId);
                                    }
                                    catch ( Exception e){ }
                                }
                                Hyt.Model.Transfer.CBDsOrderItem f = new Model.Transfer.CBDsOrderItem(n);
                                f.CurrectDsOrderItemAssociations = new DsOrderItemAssociation()//升仓明细关联
                                {
                                    OrderTransactionSysNo = mii.HytOrderItem.TransactionSysNo,//事物编号
                                    SoOrderItemSysNo = mii.HytOrderItem.SysNo                //明细编号
                                };
                                cbs.Add(f);
                            }
                            DsOrderBo.Instance.SaveDsOrder(dso, cbs, r.StatusCode);

                            #endregion
                            if (hytorder.PayStatus == (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单支付状态.已支付 && hytorder.CashPay > 0)
                            {
                                Hyt.BLL.MallSeller.DsOrderBo.Instance.FreezeDsPrePayment(r.StatusCode, defaultorder.HytOrderDealer.DealerSysNo, hytorder.CashPay, 0);//冻结金额
                            }
                        }
                        tran.Complete();
                        #endregion
                    }
                }
            }

            return result;
        }


        ///// <summary>
        ///// 商城订单导入商城
        ///// </summary>
        ///// <param name="upGradeOrderList">商城订单集合</param>
        ///// <param name="productList">产品集合</param>
        ///// <param name="defaultWarehouse">默认仓库系统编号</param>
        ///// <returns>订单导入结果</returns>
        ///// <remarks>2016-7-13 杨浩 创建</remarks>
        //public Hyt.Model.Result ImportMallOrder(IList<UpGradeOrder> upGradeOrderList, IList<PdProduct> productList, string defaultWarehouse=null)
        //{
        //    var result = new Hyt.Model.Result<UpGradeOrder>() { Status = true };

        //    var orderReceives = new Dictionary<string, BsArea>();
        //    foreach (var order in upGradeOrderList)
        //    {
        //        var areaInfo = BLL.Basic.BasicAreaBo.Instance.GetYouZanMatchDistrict(order.MallOrderReceive.City, order.MallOrderReceive.District);
        //        if (areaInfo == null)
        //        {
        //            result.Status = false;
        //            result.Message = "城市【" + order.MallOrderReceive.City + "】地区【" + order.MallOrderReceive.District + "】在系统中不存在，请到基础管理->地区信息管理添加再试！";
        //            result.Data = order;
        //            return result;
        //        }
        //        orderReceives.Add(order.MallOrderBuyer.MallOrderId, areaInfo);
        //    }

        //    bool isRedProduct = productList == null; //是否读取产品

        //    foreach (var order in upGradeOrderList)
        //    {
        //        if (!DsOrderBo.Instance.ExistsDsOrder(order.HytOrderDealer.DealerMallSysNo, order.MallOrderBuyer.MallOrderId))
        //        {

        //            if (isRedProduct)
        //            {
        //                var dic = order.UpGradeOrderItems.ToDictionary(x => x.MallProductCode);
        //                var erpCodeList = dic.Keys.ToList();
        //                productList = BLL.Product.PdProductBo.Instance.GetProductListByErpCode(erpCodeList);
        //            }
              
        //            foreach (UpGradeOrderItem mitem in order.UpGradeOrderItems)
        //            {
        //                var productInfo = productList.Where(x => x.ErpCode == mitem.MallProductCode).SingleOrDefault();
        //                if (productInfo==null)
        //                {
        //                    result.Status = false;
        //                    result.Message = "订单(" + order.MallOrderBuyer.MallOrderId + ")中的商品编码:" + mitem.MallProductCode + ",在系统中不存在！";
        //                    result.Data = order;
        //                    return result;
        //                }
        //            }

        //            var defaultorder = order;
        //            Hyt.Model.CrCustomer cr = null;
                   
        //            string strPassword = "123456";//初始密码
        //            var options = new TransactionOptions
        //            {
        //                IsolationLevel = IsolationLevel.ReadCommitted,
        //                Timeout = TransactionManager.DefaultTimeout
        //            };

        //            #region 会员
        //            using (var trancustomer = new System.Transactions.TransactionScope(TransactionScopeOption.Required, options))//会员创建事物
        //            {
        //                var customerlst = Hyt.BLL.Order.SoOrderBo.Instance.SearchCustomer(defaultorder.MallOrderReceive.Mobile);
        //                if (customerlst != null && customerlst.Count > 0)
        //                {
        //                    cr = customerlst.First();                          
        //                }
        //                else //创建会员
        //                {
        //                    cr = new Model.CrCustomer()
        //                    {
        //                        Account = defaultorder.MallOrderReceive.Mobile,
        //                        MobilePhoneNumber = defaultorder.MallOrderReceive.Mobile,
        //                        AreaSysNo = defaultorder.MallOrderReceive.AreaSysNo,
        //                        Gender = (int)Hyt.Model.WorkflowStatus.CustomerStatus.性别.保密,
        //                        EmailStatus = (int)Hyt.Model.WorkflowStatus.CustomerStatus.邮箱状态.未验证,
        //                        LevelSysNo = Hyt.Model.SystemPredefined.CustomerLevel.初级,
        //                        Name = defaultorder.MallOrderReceive.ReceiveContact,
        //                        NickName = defaultorder.MallOrderBuyer.BuyerNick,
        //                        RegisterDate = DateTime.Now,
        //                        Password = strPassword, // Hyt.Util.EncryptionUtil.EncryptWithMd5AndSalt(strPassword), 余勇修改 2014-09-12
        //                        Status = (int)Hyt.Model.WorkflowStatus.CustomerStatus.会员状态.有效,
        //                        MobilePhoneStatus = (int)Hyt.Model.WorkflowStatus.CustomerStatus.手机状态.未验证,
        //                        RegisterSource = (int)Hyt.Model.WorkflowStatus.CustomerStatus.注册来源.分销工具,
        //                        RegisterSourceSysNo = defaultorder.HytOrderDealer.DealerSysNo.ToString(),
        //                        StreetAddress = defaultorder.MallOrderReceive.ReceiveAddress,
        //                        IsReceiveShortMessage = (int)CustomerStatus.是否接收短信.是,
        //                        IsReceiveEmail = (int)CustomerStatus.是否接收邮件.是,
        //                        LastLoginDate = DateTime.Now,
        //                        Birthday = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
        //                        CreatedDate = DateTime.Now,
        //                    };
        //                    Hyt.Model.CrReceiveAddress crr = new Model.CrReceiveAddress()
        //                    {
        //                        AreaSysNo = defaultorder.MallOrderReceive.AreaSysNo,
        //                        Name = defaultorder.MallOrderReceive.ReceiveContact,
        //                        MobilePhoneNumber = defaultorder.MallOrderReceive.Mobile,
        //                        StreetAddress = defaultorder.MallOrderReceive.ReceiveAddress,
        //                        IsDefault = 1
        //                    };
        //                    Hyt.BLL.Order.SoOrderBo.Instance.CreateCustomer(cr, crr);
        //                }
        //                trancustomer.Complete();//会员创建事物
        //            }
        //            if (cr == null || cr.SysNo < 1)
        //            {
        //                result.Status = false;
        //                result.Message = "会员信息读取失败";
        //                result.Data = order;
        //                return result;
        //            }
        //            #endregion

        //            #region 订单及订单明细
                  
        //            var areaInfo = orderReceives[order.MallOrderBuyer.MallOrderId];

        //            defaultorder.MallOrderReceive.AreaSysNo = areaInfo == null ? 0 : areaInfo.SysNo;


        //            var AreaInfo = areaInfo;
                 
        //            Hyt.Model.SoOrder hytorder = new Model.SoOrder()
        //            {
        //                CustomerSysNo = cr.SysNo,
        //                OrderNo = BLL.Basic.ReceiptNumberBo.Instance.GetOrderNo(),
        //                LevelSysNo = cr.LevelSysNo,
        //                CustomerMessage = defaultorder.MallOrderBuyer.BuyerMessage,
        //                InternalRemarks = defaultorder.MallOrderBuyer.SellerMessage,
        //                CreateDate = DateTime.Now,
        //                Status = (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.待审核,
        //                OnlineStatus = Constant.OlineStatusType.待审核,
        //                SalesType = (int)Hyt.Model.WorkflowStatus.OrderStatus.销售方式.经销订单,
        //                OrderSource = (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单来源.分销商升舱,
        //                OrderSourceSysNo = defaultorder.HytOrderDealer.DealerMallSysNo,//下单来源为 分销商商城编号
        //                PayStatus = defaultorder.HytOrderDealer.HytPayStatus,
        //                DeliveryTypeSysNo =10,
        //                PayTypeSysNo = order.HytOrderDealer.HytPayType,
        //                FreightAmount = defaultorder.MallOrderPayment.PostFee,
        //                AuditorDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
        //                CancelDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
        //                LastUpdateDate = DateTime.Now,
        //                Stamp = DateTime.Now,
        //                DealerSysNo = order.HytOrderDealer.DealerSysNo,
        //                TaxFee=order.MallOrderPayment.TotalTaxAmount,

        //            };    
            
        //            int defaultWarehouseSysNo=59;

        //            var arryWp = defaultWarehouse.Split('_');

        //            int.TryParse(arryWp[0], out defaultWarehouseSysNo);                                    
        //            hytorder.DefaultWarehouseSysNo = defaultWarehouseSysNo;
        //            if (arryWp.Length > 1)
        //            {
        //                int deliveryTypeSysNo = 10;
        //                int.TryParse(arryWp[1], out deliveryTypeSysNo);
        //                hytorder.DeliveryTypeSysNo = deliveryTypeSysNo;
        //            }

        //            var hytreceive = new Model.SoReceiveAddress()
        //            {
        //                IDCardNo = defaultorder.MallOrderReceive.IdCard,
        //                AreaSysNo = defaultorder.MallOrderReceive.AreaSysNo,
        //                Name = defaultorder.MallOrderReceive.ReceiveContact,
        //                MobilePhoneNumber = defaultorder.MallOrderReceive.Mobile,
        //                StreetAddress = defaultorder.MallOrderReceive.ReceiveAddress,
        //                PhoneNumber = defaultorder.MallOrderReceive.TelPhone
        //            };
        //            List<Hyt.Model.SoOrderItem> hytitems = new List<Model.SoOrderItem>();
        //            decimal payMoney = 0;
        //            bool isCombineProduct = false;//是否合并相同产品的明细

        //            foreach (UpGradeOrderItem mitem in order.UpGradeOrderItems)
        //            {
        //                var productInfo = productList.Where(x => x.ErpCode == mitem.MallProductCode).SingleOrDefault();
        //                if (productInfo != null)
        //                {
        //                    mitem.HytProductSysNo = productInfo.SysNo;
        //                    mitem.HytProductName = productInfo.ProductName;
        //                    mitem.HytPrice = Hyt.BLL.Product.PdPriceBo.Instance.GetUserRankPrice(productInfo.SysNo,cr.LevelSysNo);

        //                }

        //                decimal price = mitem.HytPrice;
        //                Hyt.Model.SoOrderItem si = hytitems.FirstOrDefault(m => m.ProductSysNo == mitem.HytProductSysNo);
        //                if (!isCombineProduct || si == null)//不允许相同产品明细合并或者产品不再购物车中
        //                {
        //                    if (mitem.ProductSalesType == (int)CustomerStatus.商品销售类型.赠品)//赠品
        //                    {
        //                        si = new Model.SoOrderItem()
        //                        {
        //                            ProductSysNo = mitem.HytProductSysNo,
        //                            ProductName = mitem.HytProductName,
        //                            Quantity = mitem.Quantity,
        //                            OriginalPrice = mitem.MallPrice,//带入作为原单价
        //                            SalesUnitPrice = 0,
        //                            SalesAmount = 0,
        //                            ProductSalesType = (int)CustomerStatus.商品销售类型.普通,
        //                            GroupName = "淘宝赠品"
        //                        };
        //                        mitem.MallPrice = 0;//升仓明细价格为0
        //                    }
        //                    else
        //                    {
        //                        #region 计算价格信息
        //                        //if (order.HytOrderDealer.IsPreDeposit == (int)Hyt.Model.WorkflowStatus.DistributionStatus.商城是否使用预存款.是 && order.HytOrderDealer.IsSelfSupport == (int)Hyt.Model.WorkflowStatus.DistributionStatus.是否自营.否)
        //                        //{
        //                        //    price = Hyt.BLL.MallSeller.DsOrderBo.Instance.GetDsSpecialPrice(order.HytOrderDealer.DealerSysNo, mitem.HytProductSysNo);//再次获取一下分销商价格，防止客户端数据窜改
        //                        //}

        //                        si = new Model.SoOrderItem()
        //                        {
        //                            ProductSysNo = mitem.HytProductSysNo,
        //                            ProductName = mitem.HytProductName,
        //                            Quantity = mitem.Quantity,
        //                            OriginalPrice = price,
        //                            SalesUnitPrice = price,
        //                            SalesAmount = price * mitem.Quantity,
        //                            ProductSalesType = (int)CustomerStatus.商品销售类型.普通
        //                        };

        //                        if (order.HytOrderDealer.IsSelfSupport == (int)Hyt.Model.WorkflowStatus.DistributionStatus.是否自营.是)//自营存在改价格情况
        //                        {
        //                            si.SalesAmount = mitem.MallAmount;
        //                            si.SalesUnitPrice = mitem.MallPrice;
        //                        }
        //                        #endregion
        //                        Hyt.BLL.MallSeller.DsOrderBo.Instance.SetProductAssociation(order.HytOrderDealer.DealerMallSysNo, mitem.MallProductCode, mitem.MallProductAttrId, mitem.HytProductSysNo);  //非赠品设置商城产品与分销产品映射
        //                    }

        //                    hytitems.Add(si);//加入购物车
        //                    payMoney += si.SalesAmount;
        //                    mitem.HytOrderItem = si;//升仓订单明细与商城订单明细关联
        //                }
        //                //else
        //                //{
        //                //    si.Quantity += mitem.Quantity;
        //                //    if (order.HytOrderDealer.IsSelfSupport == (int)Hyt.Model.WorkflowStatus.DistributionStatus.是否自营.是)//自营存在改价格情况
        //                //    {
        //                //        si.SalesAmount += mitem.MallAmount;
        //                //        payMoney += mitem.MallAmount;
        //                //    }
        //                //    else
        //                //    {
        //                //        si.SalesAmount += si.SalesUnitPrice * mitem.Quantity;
        //                //        payMoney += si.SalesUnitPrice * mitem.Quantity;
        //                //    }
        //                //}
        //            }


        //            hytorder.OrderAmount = order.MallOrderPayment.Payment;
        //            hytorder.CashPay = order.MallOrderPayment.Payment;
        //            hytorder.ProductAmount = payMoney;
        //            #endregion.

        //            var dealerMall = DsOrderBo.Instance.GetDsDealerMall(defaultorder.HytOrderDealer.DealerMallSysNo);
        //            var operatorName = dealerMall != null ? dealerMall.ShopName : string.Empty;

        //            //取得订单图片标识
        //            hytorder.ImgFlag = DsOrderBo.Instance.GetImgFlag(defaultorder.HytOrderDealer.DealerMallSysNo,
        //                defaultorder.MallOrderBuyer.SellerMessage, defaultorder.MallOrderBuyer.SellerFlag,
        //                defaultorder.UpGradeOrderItems);

        //            using (var tran = new System.Transactions.TransactionScope(TransactionScopeOption.Required, options))
        //            {
        //                #region 数据提交
        //                var r = Hyt.BLL.Order.SoOrderBo.Instance.ImportSoOrder(hytorder, hytreceive, hytitems.ToArray(), operatorName);
        //                result.Status = r.Status;
        //                result.StatusCode = r.StatusCode;
        //                result.Message = r.Message;//事物编号
        //                //写入分销商订单信息
        //                if (r.Status)
        //                {
        //                    #region 升舱订单表

        //                    // 获取分销商城类型
        //                    var mall = DsOrderBo.Instance.GetDsDealerMall(order.HytOrderDealer.DealerMallSysNo);
        //                    var mallType = DsMallTypeBo.Instance.GetDsMallType(mall.MallTypeSysNo);
        //                    var dso = new Hyt.Model.DsOrder()
        //                    {
        //                        City = order.MallOrderReceive.City,
        //                        County = order.MallOrderReceive.District,
        //                        Province = order.MallOrderReceive.Province,
        //                        StreetAddress = order.MallOrderReceive.ReceiveAddress,
        //                        BuyerNick = order.MallOrderBuyer.BuyerNick,
        //                        DealerMallSysNo = order.HytOrderDealer.DealerMallSysNo,
        //                        DiscountAmount = order.MallOrderPayment.DiscountFee,
        //                        MallOrderId = mallType.SysNo == (int)DistributionStatus.商城类型预定义.淘宝分销 ? order.MallOrderBuyer.MallPurchaseId : order.MallOrderBuyer.MallOrderId,
        //                        PostFee = order.MallOrderPayment.PostFee,
        //                        Payment = order.MallOrderPayment.Payment,
        //                        ServiceFee = order.MallOrderPayment.ServiceFee,
        //                        PayTime = order.MallOrderPayment.PayTime,
        //                        UpgradeTime = DateTime.Now,
        //                        Status = (int)Hyt.Model.WorkflowStatus.DistributionStatus.升舱订单状态.升舱中,
        //                        DeliveryTime = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
        //                        SignTime = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
        //                        LastCallbackTime = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
        //                    };
        //                    List<Hyt.Model.Transfer.CBDsOrderItem> cbs = new List<Hyt.Model.Transfer.CBDsOrderItem>();
        //                    foreach (UpGradeOrderItem mii in order.UpGradeOrderItems)
        //                    {
        //                        Hyt.Model.DsOrderItem n = new Model.DsOrderItem()
        //                        {
        //                            Quantity = mii.Quantity,
        //                            Price = mii.MallPrice,
        //                            MallProductId = mii.MallProductCode,
        //                            MallProductName = mii.MallProductName,
        //                            DiscountAmount = mii.DiscountFee,
        //                            MallProductAttribute = mii.MallProductAttrs
        //                        };

        //                        if (!string.IsNullOrEmpty(mii.MallOrderItemId))//如果数据转换错误就不写入淘宝订单明细编号
        //                        {
        //                            try
        //                            {
        //                                n.MallItemNo = long.Parse(mii.MallOrderItemId);
        //                            }
        //                            catch { }
        //                        }
        //                        Hyt.Model.Transfer.CBDsOrderItem f = new Model.Transfer.CBDsOrderItem(n);
        //                        f.CurrectDsOrderItemAssociations = new DsOrderItemAssociation()//升仓明细关联
        //                        {
        //                            OrderTransactionSysNo = mii.HytOrderItem.TransactionSysNo,//事物编号
        //                            SoOrderItemSysNo = mii.HytOrderItem.SysNo                //明细编号
        //                        };
        //                        cbs.Add(f);
        //                    }
        //                    DsOrderBo.Instance.SaveDsOrder(dso, cbs, r.StatusCode);

        //                    #endregion
        //                    if (hytorder.PayStatus == (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单支付状态.已支付 && hytorder.CashPay > 0)
        //                    {
        //                        Hyt.BLL.MallSeller.DsOrderBo.Instance.FreezeDsPrePayment(r.StatusCode, defaultorder.HytOrderDealer.DealerSysNo, hytorder.CashPay,0);//冻结金额
        //                    }
        //                }
        //                tran.Complete();
        //                #endregion
        //            }
        //        }
        //    }

        //    return result;
        //}
        /// <summary>
        /// 商城订单导入商城
        /// </summary>
        /// <returns>订单导入结果</returns>
        /// <remarks>2016-7-13 杨浩 创建</remarks>
        public Result ImportMallOrder()
        {
            //1、判断已匹配的区域编号是否存在
            //2、判断已匹配的商城商品编号是否存在
            //3、B店判断已匹配商品编号是否已维护当前分销商价格
            //4、若该订单的收货手机已在商城注册，则直接使用该商城账号创建商城订单，若该收货手机未注册商城，首先使用该手机注册商城。
            //5、若C店订单，导入后商城订单金额为0，商品明细价格为0，同时自动产生一条收款金额为0的收款单
            //6、若B店订单，导入后商城订单商品明细价格为商城系统维护的升舱结算价格，同时产生一条收款金额为结算金额的收款单
            //7、发送升舱成功短信，若新注册用户，同时发送商城账号密码。                    
            var result = new Result() { Status = true };
            var parameters = new OrderParameters();
            parameters.EndDate = DateTime.Now;
            var threeMallSyncLogInfo = BLL.Distribution.DsThreeMallSyncLogBo.Instance.GetThreeMallSyncLogInfo(0, (int)DistributionStatus.商城类型预定义.有赞, 0);
            if (threeMallSyncLogInfo == null)
            {
                threeMallSyncLogInfo = new Model.Stores.DsThreeMallSyncLog();
                threeMallSyncLogInfo.LastSyncTime = parameters.EndDate;
              
                threeMallSyncLogInfo.SyncType = 0;
                parameters.StartDate = DateTime.Now.AddYears(-100);

            }
            else
            {
                parameters.StartDate = threeMallSyncLogInfo.LastSyncTime.AddDays(-10);
                threeMallSyncLogInfo.LastSyncTime = parameters.EndDate;
            }
            var g = new Extra.UpGrade.UpGrades.YouZanUpGrade();
            var _result = g.GetOrderList(parameters, null);

            if (_result.Data == null || _result.Data.Count <= 0)
            {
                result.Status = false;
                result.Message = "无三方订单导入！";
                return result;
            }

            var pager = new Pager<PdProduct>() { PageSize = 999999, CurrentPage = 1 };
            pager = BLL.Product.PdProductBo.Instance.GetPdProductList(pager);


            var orderReceives = new Dictionary<string, BsArea>();
            foreach (var order in _result.Data)
            {
                var areaInfo = BLL.Basic.BasicAreaBo.Instance.GetYouZanMatchDistrict(order.MallOrderReceive.City, order.MallOrderReceive.District);
                if (areaInfo == null)
                {
                    result.Status = false;
                    result.Message = "城市【" + order.MallOrderReceive.City + "】地区【" + order.MallOrderReceive.District + "】在系统中不存在，请到基础管理->地区信息管理添加再试！";
                    return result;
                }
                orderReceives.Add(order.MallOrderBuyer.MallOrderId, areaInfo);
            }



            foreach (var order in _result.Data)
            {


                if (!DsOrderBo.Instance.ExistsDsOrder(order.HytOrderDealer.DealerMallSysNo, order.MallOrderBuyer.MallOrderId))
                {

                    foreach (UpGradeOrderItem mitem in order.UpGradeOrderItems)
                    {
                        if (!pager.Rows.Any(x => x.ErpCode == mitem.MallProductCode))
                        {
                            result.Status = false;
                            result.Message = "有赞订单(" + mitem.OrderId + ")中的商品编码:" + mitem.MallProductCode + ",在系统中不存在！";
                            return result;
                        }
                    }

                    var defaultorder = order;
                    Hyt.Model.CrCustomer cr = null;
                    var isNewUser = true;
                    string strPassword = "123456";//初始密码
                    var options = new TransactionOptions
                    {
                        IsolationLevel = IsolationLevel.ReadCommitted,
                        Timeout = TransactionManager.DefaultTimeout
                    };
                    #region 会员
                    using (var trancustomer = new System.Transactions.TransactionScope(TransactionScopeOption.Required, options))//会员创建事物
                    {
                        var customerlst = Hyt.BLL.Order.SoOrderBo.Instance.SearchCustomer(defaultorder.MallOrderReceive.Mobile);
                        if (customerlst != null && customerlst.Count > 0)
                        {
                            cr = customerlst.First();
                            isNewUser = false;
                        }
                        else //创建会员
                        {
                            cr = new Model.CrCustomer()
                            {
                                Account = defaultorder.MallOrderReceive.Mobile,
                                MobilePhoneNumber = defaultorder.MallOrderReceive.Mobile,
                                AreaSysNo = defaultorder.MallOrderReceive.AreaSysNo,
                                Gender = (int)Hyt.Model.WorkflowStatus.CustomerStatus.性别.保密,
                                EmailStatus = (int)Hyt.Model.WorkflowStatus.CustomerStatus.邮箱状态.未验证,
                                LevelSysNo = Hyt.Model.SystemPredefined.CustomerLevel.初级,
                                Name = defaultorder.MallOrderReceive.ReceiveContact,
                                NickName = defaultorder.MallOrderBuyer.BuyerNick,
                                RegisterDate = DateTime.Now,
                                Password = strPassword, // Hyt.Util.EncryptionUtil.EncryptWithMd5AndSalt(strPassword), 余勇修改 2014-09-12
                                Status = (int)Hyt.Model.WorkflowStatus.CustomerStatus.会员状态.有效,
                                MobilePhoneStatus = (int)Hyt.Model.WorkflowStatus.CustomerStatus.手机状态.未验证,
                                RegisterSource = (int)Hyt.Model.WorkflowStatus.CustomerStatus.注册来源.分销工具,
                                RegisterSourceSysNo = defaultorder.HytOrderDealer.DealerSysNo.ToString(),
                                StreetAddress = defaultorder.MallOrderReceive.ReceiveAddress,
                                IsReceiveShortMessage = (int)CustomerStatus.是否接收短信.是,
                                IsReceiveEmail = (int)CustomerStatus.是否接收邮件.是,
                                LastLoginDate = DateTime.Now,
                                Birthday = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                                CreatedDate = DateTime.Now,
                            };
                            Hyt.Model.CrReceiveAddress crr = new Model.CrReceiveAddress()
                            {
                                AreaSysNo = defaultorder.MallOrderReceive.AreaSysNo,
                                Name = defaultorder.MallOrderReceive.ReceiveContact,
                                MobilePhoneNumber = defaultorder.MallOrderReceive.Mobile,
                                StreetAddress = defaultorder.MallOrderReceive.ReceiveAddress,
                                IsDefault = 1
                            };
                            Hyt.BLL.Order.SoOrderBo.Instance.CreateCustomer(cr, crr);
                        }
                        trancustomer.Complete();//会员创建事物
                    }
                    if (cr == null || cr.SysNo < 1)
                    {
                        result.Status = false;
                        result.Message = "会员信息读取失败";
                        return result;
                    }
                    #endregion

                    #region 订单及订单明细
                    //bool IsSelfSend = false;//是否自己的物流来配送

                    var areaInfo = orderReceives[order.MallOrderBuyer.MallOrderId];// BLL.Basic.BasicAreaBo.Instance.GetYouZanMatchDistrict(order.MallOrderReceive.City, order.MallOrderReceive.District);

                    defaultorder.MallOrderReceive.AreaSysNo = areaInfo == null ? 0 : areaInfo.SysNo;


                    //var AreaInfo = areaInfo;// Hyt.BLL.Basic.BasicAreaBo.Instance.GetArea(defaultorder.MallOrderReceive.AreaSysNo);
                    //if (AreaInfo != null)
                    //{
                    //    IsSelfSend = Hyt.BLL.Logistics.LgDeliveryScopeBo.Instance.IsInScope(AreaInfo.ParentSysNo, new Coordinate() { X = map_x, Y = map_y });
                    //}
                    Hyt.Model.SoOrder hytorder = new Model.SoOrder()
                    {
                        CustomerSysNo = cr.SysNo,
                        OrderNo = BLL.Basic.ReceiptNumberBo.Instance.GetOrderNo(),
                        LevelSysNo = cr.LevelSysNo,
                        CustomerMessage = defaultorder.MallOrderBuyer.BuyerMessage,
                        InternalRemarks = defaultorder.MallOrderBuyer.SellerMessage,
                        CreateDate = DateTime.Now,
                        Status = (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.待审核,
                        OnlineStatus = Constant.OlineStatusType.待审核,
                        SalesType = (int)Hyt.Model.WorkflowStatus.OrderStatus.销售方式.经销订单,
                        OrderSource = (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单来源.分销商升舱,
                        OrderSourceSysNo = defaultorder.HytOrderDealer.DealerMallSysNo,//下单来源为 分销商商城编号
                        PayStatus = defaultorder.HytOrderDealer.HytPayStatus,
                        DeliveryTypeSysNo = Hyt.Model.SystemPredefined.DeliveryType.第三方快递,
                        PayTypeSysNo = order.HytOrderDealer.HytPayType,
                        FreightAmount = defaultorder.MallOrderPayment.PostFee,
                        AuditorDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                        CancelDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                        LastUpdateDate = DateTime.Now,
                        Stamp = DateTime.Now,

                    };
                    var warehouse = Hyt.BLL.Warehouse.WhWarehouseBo.Instance.GetWhWareHouse(defaultorder.MallOrderReceive.AreaSysNo, null, hytorder.DeliveryTypeSysNo, Hyt.Model.WorkflowStatus.WarehouseStatus.仓库状态.启用).FirstOrDefault();
                    if (warehouse != null)
                    {
                        hytorder.DefaultWarehouseSysNo = warehouse.SysNo;
                    }
                    else
                    {
                        hytorder.DefaultWarehouseSysNo = 38;
                    }


                    var hytreceive = new Model.SoReceiveAddress()
                    {
                        IDCardNo = defaultorder.MallOrderReceive.IdCard,
                        AreaSysNo = defaultorder.MallOrderReceive.AreaSysNo,
                        Name = defaultorder.MallOrderReceive.ReceiveContact,
                        MobilePhoneNumber = defaultorder.MallOrderReceive.Mobile,
                        StreetAddress = defaultorder.MallOrderReceive.ReceiveAddress,
                        PhoneNumber = defaultorder.MallOrderReceive.TelPhone
                    };
                    List<Hyt.Model.SoOrderItem> hytitems = new List<Model.SoOrderItem>();
                    decimal payMoney = 0;
                    bool isCombineProduct = false;//是否合并相同产品的明细

                    foreach (UpGradeOrderItem mitem in order.UpGradeOrderItems)
                    {
                        var productInfo = pager.Rows.Where(x => x.ErpCode == mitem.MallProductCode).SingleOrDefault();
                        if (productInfo != null)
                        {
                            mitem.HytProductSysNo = productInfo.SysNo;
                            mitem.HytProductName = productInfo.ProductName;
                            mitem.HytPrice = Hyt.BLL.Product.PdPriceBo.Instance.GetUserRankPrice(productInfo.SysNo, cr.LevelSysNo);

                        }

                        decimal price = mitem.HytPrice;
                        Hyt.Model.SoOrderItem si = hytitems.FirstOrDefault(m => m.ProductSysNo == mitem.HytProductSysNo);
                        if (!isCombineProduct || si == null)//不允许相同产品明细合并或者产品不再购物车中
                        {
                            if (mitem.ProductSalesType == (int)CustomerStatus.商品销售类型.赠品)//赠品
                            {
                                si = new Model.SoOrderItem()
                                {
                                    ProductSysNo = mitem.HytProductSysNo,
                                    ProductName = mitem.HytProductName,
                                    Quantity = mitem.Quantity,
                                    OriginalPrice = mitem.MallPrice,//带入作为原单价
                                    SalesUnitPrice = 0,
                                    SalesAmount = 0,
                                    ProductSalesType = (int)CustomerStatus.商品销售类型.普通,
                                    GroupName = "赠品"
                                };
                                mitem.MallPrice = 0;//升仓明细价格为0
                            }
                            else
                            {
                                #region 计算价格信息
                                if (order.HytOrderDealer.IsPreDeposit == (int)Hyt.Model.WorkflowStatus.DistributionStatus.商城是否使用预存款.是 && order.HytOrderDealer.IsSelfSupport == (int)Hyt.Model.WorkflowStatus.DistributionStatus.是否自营.否)
                                {
                                    price = Hyt.BLL.MallSeller.DsOrderBo.Instance.GetDsSpecialPrice(order.HytOrderDealer.DealerSysNo, mitem.HytProductSysNo);//再次获取一下分销商价格，防止客户端数据窜改
                                }
                                si = new Model.SoOrderItem()
                                {
                                    ProductSysNo = mitem.HytProductSysNo,
                                    ProductName = mitem.HytProductName,
                                    Quantity = mitem.Quantity,
                                    OriginalPrice = price,
                                    SalesUnitPrice = price,
                                    SalesAmount = price * mitem.Quantity,
                                    ProductSalesType = (int)CustomerStatus.商品销售类型.普通
                                };



                                if (order.HytOrderDealer.IsSelfSupport == (int)Hyt.Model.WorkflowStatus.DistributionStatus.是否自营.是)//自营存在改价格情况
                                {
                                    si.SalesAmount = mitem.MallAmount;
                                }
                                #endregion
                                Hyt.BLL.MallSeller.DsOrderBo.Instance.SetProductAssociation(order.HytOrderDealer.DealerMallSysNo, mitem.MallProductCode, mitem.MallProductAttrId, mitem.HytProductSysNo);  //非赠品设置商城产品与分销产品映射
                            }
                            hytitems.Add(si);//加入购物车
                            payMoney += si.SalesAmount;
                            mitem.HytOrderItem = si;//升仓订单明细与商城订单明细关联
                        }
                        else
                        {
                            si.Quantity += mitem.Quantity;
                            if (order.HytOrderDealer.IsSelfSupport == (int)Hyt.Model.WorkflowStatus.DistributionStatus.是否自营.是)//自营存在改价格情况
                            {
                                si.SalesAmount += mitem.MallAmount;
                                payMoney += mitem.MallAmount;
                            }
                            else
                            {
                                si.SalesAmount += si.SalesUnitPrice * mitem.Quantity;
                                payMoney += si.SalesUnitPrice * mitem.Quantity;
                            }
                        }
                    }


                    hytorder.OrderAmount = payMoney;
                    hytorder.CashPay = payMoney;
                    hytorder.ProductAmount = payMoney;
                    #endregion.


                    var dealerMall = DsOrderBo.Instance.GetDsDealerMall(defaultorder.HytOrderDealer.DealerMallSysNo);
                    var operatorName = dealerMall != null ? dealerMall.ShopName : string.Empty;


                    //取得订单图片标识
                    hytorder.ImgFlag = DsOrderBo.Instance.GetImgFlag(defaultorder.HytOrderDealer.DealerMallSysNo,
                        defaultorder.MallOrderBuyer.SellerMessage, defaultorder.MallOrderBuyer.SellerFlag,
                        defaultorder.UpGradeOrderItems);

                    using (var tran = new System.Transactions.TransactionScope(TransactionScopeOption.Required, options))
                    {
                        #region 数据提交
                        var r = Hyt.BLL.Order.SoOrderBo.Instance.ImportSoOrder(hytorder, hytreceive, hytitems.ToArray(), operatorName);
                        result.Status = r.Status;
                        result.StatusCode = r.StatusCode;
                        result.Message = r.Message;//事物编号
                        //写入分销商订单信息
                        if (r.Status)
                        {
                            #region 升舱订单表

                            // 获取分销商城类型
                            var mall = DsOrderBo.Instance.GetDsDealerMall(order.HytOrderDealer.DealerMallSysNo);
                            var mallType = DsMallTypeBo.Instance.GetDsMallType(mall.MallTypeSysNo);
                            var dso = new Hyt.Model.DsOrder()
                            {
                                City = order.MallOrderReceive.City,
                                County = order.MallOrderReceive.District,
                                Province = order.MallOrderReceive.Province,
                                StreetAddress = order.MallOrderReceive.ReceiveAddress,
                                BuyerNick = order.MallOrderBuyer.BuyerNick,
                                DealerMallSysNo = order.HytOrderDealer.DealerMallSysNo,
                                DiscountAmount = order.MallOrderPayment.DiscountFee,
                                MallOrderId = mallType.SysNo == (int)DistributionStatus.商城类型预定义.淘宝分销 ? order.MallOrderBuyer.MallPurchaseId : order.MallOrderBuyer.MallOrderId,
                                PostFee = order.MallOrderPayment.PostFee,
                                Payment = order.MallOrderPayment.Payment,
                                ServiceFee = order.MallOrderPayment.ServiceFee,
                                PayTime = order.MallOrderPayment.PayTime,
                                UpgradeTime = DateTime.Now,
                                Status = (int)Hyt.Model.WorkflowStatus.DistributionStatus.升舱订单状态.升舱中,
                                DeliveryTime = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                                SignTime = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                                LastCallbackTime = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                            };
                            List<Hyt.Model.Transfer.CBDsOrderItem> cbs = new List<Hyt.Model.Transfer.CBDsOrderItem>();
                            foreach (UpGradeOrderItem mii in order.UpGradeOrderItems)
                            {
                                Hyt.Model.DsOrderItem n = new Model.DsOrderItem()
                                {
                                    Quantity = mii.Quantity,
                                    Price = mii.MallPrice,
                                    MallProductId = mii.MallProductCode,
                                    MallProductName = mii.MallProductName,
                                    DiscountAmount = mii.DiscountFee,
                                    MallProductAttribute = mii.MallProductAttrs
                                };

                                if (!string.IsNullOrEmpty(mii.MallOrderItemId))//如果数据转换错误就不写入淘宝订单明细编号
                                {
                                    try
                                    {
                                        n.MallItemNo = long.Parse(mii.MallOrderItemId);
                                    }
                                    catch { }
                                }
                                Hyt.Model.Transfer.CBDsOrderItem f = new Model.Transfer.CBDsOrderItem(n);
                                f.CurrectDsOrderItemAssociations = new DsOrderItemAssociation()//升仓明细关联
                                {
                                    OrderTransactionSysNo = mii.HytOrderItem.TransactionSysNo,//事物编号
                                    SoOrderItemSysNo = mii.HytOrderItem.SysNo                //明细编号
                                };
                                cbs.Add(f);
                            }
                            DsOrderBo.Instance.SaveDsOrder(dso, cbs, r.StatusCode);

                            #endregion
                            if (hytorder.PayStatus == (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单支付状态.已支付 && hytorder.CashPay > 0)
                            {
                                Hyt.BLL.MallSeller.DsOrderBo.Instance.FreezeDsPrePayment(r.StatusCode, defaultorder.HytOrderDealer.DealerSysNo, hytorder.CashPay, BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);//冻结金额
                            }
                        }
                        tran.Complete();
                        #endregion
                    }
                }
            }


            if (threeMallSyncLogInfo.SysNo <= 0)
                BLL.Distribution.DsThreeMallSyncLogBo.Instance.Add(threeMallSyncLogInfo);
            else
                BLL.Distribution.DsThreeMallSyncLogBo.Instance.Update(threeMallSyncLogInfo);

            return result;
        }



        /// <summary>
        /// 根据订单系统编号列表获取出库待接收的订单列表
        /// </summary>
        /// <param name="sysNos">订单系统编号集（多个逗号分隔）</param>
        /// <returns></returns>
        /// <remarks>2016-6-26 杨浩 创建</remarks>
        public List<SoOrder> GetOrderListBySysNos(string sysNos)
        {
            return ISoOrderDao.Instance.GetOrderListBySysNosAndStatus(sysNos, (int)OrderStatus.销售单状态.出库待接收);
        }

        /// <summary>
        /// 根据订单编号获取配送单
        /// </summary>
        /// <param name="sysNo">订单编号</param>
        /// <returns></returns>
        /// <remarks>2016-09-09 杨浩 创建</remarks>
        public List<LgDeliveryItem> GetDeliveryItem(int sysNo)
        {
            return ISoOrderDao.Instance.GetDeliveryItem(sysNo);
        }

        #region 一键发货
        /// <summary>
        /// 发货
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <param name="expressNo">快递单号</param>
        /// <param name="orderInfo">订单实体</param>
        /// <returns></returns>
        /// <remarks>2016-03-22 杨浩 创建</remarks>
        public Result Ship(int orderSysNo, string expressNo, SoOrder orderInfo)
        {
            var res = new Hyt.Model.Result { Status = true };

            if (orderInfo == null)
            {
                res.Status = false;
                res.Message = "订单不存在！";
                return res;
            }

            if (orderInfo.Status != 30)
            {
                res.Status = false;
                res.Message = "订单所属状态不能发货！";
                return res;
            }

            //配送员系统编号.
            int deliveryUserSysNo = AdminAuthenticationBo.Instance.Current != null ? AdminAuthenticationBo.Instance.Current.Base.SysNo : 0;

            //仓库下的配送人员
            var syUsers = Hyt.DataAccess.Warehouse.IWhWarehouseDao.Instance.GetWhDeliveryUser(orderInfo.DefaultWarehouseSysNo,
                                                               Hyt.Model.WorkflowStatus.LogisticsStatus
                                                                  .配送员是否允许配送.是);

            var deliveryType = BLL.Logistics.DeliveryTypeBo.Instance.GetDeliveryType(orderInfo.DeliveryTypeSysNo);
            if (deliveryType == null)
            {
                res.Status = false;
                res.Message = "配送方式不存在！";
                return res;
            }

            if (orderInfo.PayStatus != 20)
            {
                res.Status = false;
                res.Message = "未支付的订单不能发货！";
                return res;
            }



            int isThirdpartyExpress = deliveryType.IsThirdPartyExpress;

            int deliveryTypeSysNo = orderInfo.DeliveryTypeSysNo;

            //如果不是三方快递发货则需要检查是权限操作
            if (isThirdpartyExpress == 0 && syUsers.Any(x => x.SysNo == deliveryUserSysNo))
            {
                res.Status = false;
                res.Message = "您没有权限发货此仓库的订单！";
                return res;
            }


            using (var tran = new TransactionScope())
            {
                //orderInfo.OrderItemList.ForEach(x => x.RealStockOutQuantity = x.Quantity);
                //创建出库单
                res = CreateOutStock(orderInfo.OrderItemList, orderInfo.DefaultWarehouseSysNo, orderInfo.DeliveryTypeSysNo);

                if (res.Status)
                {
                    var stockOutInfo = ((Hyt.Model.Result<WhStockOut>)res).Data;


                    //出库单明细系统编号,商品数量字符串
                    string checkedStockItemSysNo = "";

                    foreach (var item in stockOutInfo.Items)
                    {
                        checkedStockItemSysNo += item.SysNo + "," + item.ProductQuantity + ";";
                    }

                    //出库
                    var stockOutResult = StockOut(stockOutInfo.SysNo, checkedStockItemSysNo, deliveryTypeSysNo, isThirdpartyExpress, stockOutInfo.Stamp.ToString());

                    if (stockOutResult.Status)
                    {

                        //出库单系统编号
                        string stockOutSysNo = ((Hyt.Model.Result<int>)stockOutResult).Data.ToString();

                        //配送单明细 (单据类型,单据编号,快递单号)
                        string[] items = new string[] { ((int)LogisticsStatus.配送单据类型.出库单).ToString() + "," + stockOutSysNo + "," + expressNo };

                        //是否允许超出配送信用额度配送
                        bool isForce = true;

                        Hyt.DataAccess.Warehouse.IOutStockDao.Instance.UpdateStatus(stockOutInfo.SysNo, WarehouseStatus.出库单状态.待配送);
                        //确认发货
                        res = ConfrimDelivery(stockOutInfo.WarehouseSysNo, deliveryUserSysNo, deliveryTypeSysNo, items, isForce, false);


                    }
                    else
                    {
                        res.Status = false;
                        res.Message = stockOutResult.Message;
                    }

                }
                if (res.Status)
                {
                    tran.Complete();
                }


            }
            return res;



        }


        /// <summary>
        /// 发货
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <param name="expressNo">快递单号</param>
        /// <returns></returns>
        /// <remarks>2016-03-22 杨浩 创建</remarks>
        public Result Ship(int orderSysNo, string expressNo)
        {
            //获取的订单详情
            var orderInfo = BLL.Web.SoOrderBo.Instance.GetEntity(orderSysNo);
            return Ship(orderSysNo, expressNo, orderInfo);
        }


        #region 创建出库单
        /// <summary>
        /// 创建出库单
        /// </summary>
        /// <param name="data">出库商品列表</param>
        /// <param name="warehouseSysNo">仓库编号</param>
        /// <param name="deliveryTypeSysNo">出库单配送方式</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2016-03-22 杨浩 创建</remarks>
        public Result CreateOutStock(IList<SoOrderItem> data, int warehouseSysNo, int? deliveryTypeSysNo)
        {
            var res = new Hyt.Model.Result<WhStockOut> { Status = true };
            var identity = string.Format("创建订单{0}的出库单", data[0].OrderSysNo);
            if (YwbUtil.Enter(identity) == false)
            {
                res.Status = false;
                res.Message = "其它人正在处理这个订单，请稍后重试";
                res.StatusCode = 0;
                return res;
            }

            try
            {
                SyUser syUser = new SyUser();
                if (BLL.Authentication.AdminAuthenticationBo.Instance.Current != null)
                {
                    syUser = BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base;
                }
                var outStock = SoOrderBo.Instance.CreateOutStock(data, warehouseSysNo, syUser, deliveryTypeSysNo);
                res.Data = outStock;
                res.StatusCode = outStock.SysNo;
                SysLog.Instance.Info(LogStatus.系统日志来源.后台, "创建出库单", LogStatus.系统日志目标类型.出库单, outStock.SysNo, 0);

            }
            catch (Exception ex)
            {
                res.Status = false;
                res.Message = ex.Message;
                res.StatusCode = 0;
            }
            YwbUtil.Exit(identity);

            return res;
        }
        #endregion

        #region  出库
        /// <summary>
        /// 单出库单出库
        /// </summary>
        /// <param name="sysNo">出库单系统编号</param>
        /// <param name="checkedStockItemSysNo">选中的出库单明细系统编号,商品数量字符串</param>
        /// <param name="deliveryTypeSysNo">配送方式系统编号</param>
        /// <param name="isThirdpartyExpress">是否为第三方快递</param>
        /// <param name="stamp">出库时的时间戳</param>
        /// <returns>返回操作状态(Result.StatusCode>=0成功,小于 0失败)</returns>
        /// <returns>罗勤尧 2017-09-09增加对锁库存数的判断</returns>
        /// <remarks>2016-03-22 杨浩 创建</remarks>
        public Result StockOut(int sysNo, string checkedStockItemSysNo, int deliveryTypeSysNo, int isThirdpartyExpress, string stamp)
        {
            var result = new Hyt.Model.Result<int> { StatusCode = -1 };

            if (string.IsNullOrEmpty(checkedStockItemSysNo))
            {
                result.Message = "没有扫描任何商品， 不能出库。";
                return result;
            }
            if (checkedStockItemSysNo.LastIndexOf(';', checkedStockItemSysNo.Length - 1) > 0)
            {
                checkedStockItemSysNo = checkedStockItemSysNo.Remove(checkedStockItemSysNo.Length - 1, 1);
            }
            //出库单明细系统编号与商品数量集合字符串
            var stockItemSysNoAndProductNumList = checkedStockItemSysNo.Split(';');
            if (stockItemSysNoAndProductNumList.Length == 0)
            {
                result.Message = "没有扫描任何商品， 不能出库。";
                return result;
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
                result.Message = "没有扫描任何商品， 不能出库。";
                return result;
            }

            var master = WhWarehouseBo.Instance.Get(sysNo);
            //检查时间戳是否改变
            if (master.Stamp.ToString() != stamp)
            {
                result.Message = "此出库单已被其他人修改，请关闭当前窗口后刷新页面！";
                return result;
            }
            if (master.Status != (int)WarehouseStatus.出库单状态.待出库)
            {
                result.Message = "此出库单不是待出库状态，不能出库！";
                return result;
            }

            //第三方快递,订单未收收款,不允许出库
            if (isThirdpartyExpress == 1)
            {
                var order = SoOrderBo.Instance.GetEntity(master.OrderSysNO);
                if (order.PayStatus != OrderStatus.销售单支付状态.已支付.GetHashCode())
                {
                    result.Message = "第三方快递订单未收款,不能出库。";
                    return result;
                }
            }

            //判断库存数量 王耀发 2016-3-14 创建
            GeneralConfig config = BLL.Config.Config.Instance.GetGeneralConfig();
            int warehouseSysNo = master.WarehouseSysNo;
            IList<WhStockOutItem> outData = Hyt.BLL.Warehouse.WhWarehouseBo.GetWhStockOutItemByOut(sysNo);
            //减库存标识：1-支付后减库存，0-出库后减库存
            if (config.ReducedInventory == 0)
            {
                bool flag = false;
                foreach (var Item in outData)
                {
                    PdProductStock entity = IPdProductStockDao.Instance.GetEntityByWP(warehouseSysNo, Item.ProductSysNo);
                    var wh = Hyt.DataAccess.Warehouse.IWhWarehouseDao.Instance.GetWarehouseEntity(warehouseSysNo);//发货仓库信息
                    if (wh != null && wh.IsSelfSupport == (int)WarehouseStatus.是否自营.是)
                    {  //是否自营
                        if (entity != null)
                        {
                            if (entity.StockQuantity-entity.LockStockQuantity < Item.ProductQuantity)
                            {
                                result.Message = Item.ProductName + "库存不够,不能出库。";
                                flag = true;
                                break;
                            }
                        }
                        else
                        {
                            result.Message = Item.ProductName + "对应库存不存在,不能出库。";
                            flag = true;
                            break;
                        }
                    }//发货仓库非自营

                }
                if (flag)
                {
                    return result;
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
                master.StockOutBy = 0;
                master.LastUpdateBy = 0;
                master.LastUpdateDate = DateTime.Now;
                master.Status = (int)WarehouseStatus.出库单状态.待拣货;

                //WhWarehouseBo.Instance.OutStock(master, CurrentUser.Base.SysNo);
                WhWarehouseBo.Instance.OutStock(master);

                //减库存 王耀发 2016-3-11 创建
                //减库存标识：1-支付后减库存，0-出库后减库存 更新库存数量 王耀发 2016-3-11 创建 
                if (config.ReducedInventory == 0)
                {
                    foreach (var Item in outData)
                    {
                        Hyt.BLL.Warehouse.PdProductStockBo.Instance.UpdateStockQuantity(warehouseSysNo, Item.ProductSysNo, Item.ProductQuantity);
                    }
                }
                //SoOrderBo.Instance.WriteSoTransactionLog(master.TransactionSysNo, "出库单" + master.SysNo + "已出库，待拣货",
                //                                         CurrentUser.Base.UserName);
                //SoOrderBo.Instance.WriteSoTransactionLog(master.TransactionSysNo, "您的订单已出库，待拣货",
                //                                         CurrentUser.Base.UserName);
                var delivery = DeliveryTypeBo.Instance.GetDeliveryType(deliveryTypeSysNo);
                var deliveryName = (delivery == null)
                    ? "未能找到编号为" + deliveryTypeSysNo + "的配送方式"
                    : delivery.DeliveryTypeName;
                var logTxt = "订单生成配送方式:<span style=\"color:red\">" + deliveryName + "</span>，待拣货打包";
                SoOrderBo.Instance.WriteSoTransactionLog(master.TransactionSysNo, logTxt,
                    "系统");
                result.Status = true;
                result.Message = master.SysNo + " 出库成功。";
                result.StatusCode = 0;
                result.Data = master.SysNo;//保存出库单系统编号
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, ex.Message, LogStatus.系统日志目标类型.出库单, master.SysNo, ex,
                    0);
            }

            return result;
        }
        #endregion

        #region 确认发货
        /// <summary>
        /// 确认发货（Ajax调用）
        /// </summary>
        ///<param name="warehouseSysNo">仓库系统编号</param>
        /// <param name="deliveryUserSysNo">配送员系统编号.</param>
        /// <param name="deliverTypeSysno">配送方式系统编号.</param>
        /// <param name="items">配送单明细 (单据类型,单据编号,快递单号)</param>
        /// <param name="isForce">是否允许超出配送信用额度配送 </param>
        /// <remarks>2016-06-27 杨浩 创建</remarks>
        /// <remarks>2016-04-6 杨云奕 修改，添加事务执行判断</remarks>
        public Hyt.Model.Result ConfrimDelivery(int warehouseSysNo, int deliveryUserSysNo, int deliverTypeSysno, string[] items, bool isForce, bool IsUserTransactionScope = true)
        {
            var result = new Hyt.Model.Result<int>();
            try
            {
                var itemList = new List<LgDeliveryItem> { };
                string NeedInStock = string.Empty;
                foreach (var note in items.Select(item => item.Split(',')))
                {
                    if (note.Length < 2)
                    {
                        result.Message = "配送单明细数据错误,不能创建配送单";
                    }
                    LgDeliveryItem item = new LgDeliveryItem
                    {
                        NoteType = int.Parse(note[0]),
                        NoteSysNo = int.Parse(note[1]),
                        ExpressNo = note.Length >= 3 ? note[2].Trim() : ""
                    };

                    #region 判断快递单号是否重复(2014-04-11 朱成果)
                    if (!string.IsNullOrEmpty(item.ExpressNo) && item.NoteType == (int)LogisticsStatus.配送单据类型.出库单)
                    {
                        var flg = Hyt.BLL.Logistics.LgDeliveryBo.Instance.IsExistsExpressNo(deliverTypeSysno, item.ExpressNo);
                        if (flg)
                        {
                            result.Status = false;
                            result.Message = "快递单号" + item.ExpressNo + "已经被使用，请更换快递单号";
                            return result;
                        }
                    }
                    #endregion

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
                if (!string.IsNullOrEmpty(NeedInStock)) //未入库的单子
                {
                    result.Status = false;
                    result.Message = "请将先前配送单作废，拒收，未送达生成的入库单(" + NeedInStock + ")完成入库";
                    return result;
                }
                var options = new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadCommitted,
                    Timeout = TransactionManager.DefaultTimeout
                };

                //配送方式  
                var delivertType = DeliveryTypeBo.Instance.GetDeliveryType(deliverTypeSysno);

                int deliverySysno;
                var deliveryMsgs = new List<Hyt.BLL.Logistics.LgDeliveryBo.DeliveryMsg>();

                ///事务执行判断，当为true时表示执行事务
                if (IsUserTransactionScope)
                {
                    using (var tran = new TransactionScope(TransactionScopeOption.Required, options))
                    {
                        //deliverySysno = LgDeliveryBo.Instance.CreateLgDelivery(warehouseSysNo, deliveryUserSysNo,
                        //    delivertType,
                        //    CurrentUser.Base.SysNo, itemList, isForce, Request.ServerVariables["REMOTE_ADDR"]);

                        deliverySysno = LgDeliveryBo.Instance.NewCreateLgDelivery(warehouseSysNo, deliveryUserSysNo,
                            delivertType,
                            0, itemList, isForce, ref deliveryMsgs, "");

                        result.Status = true;
                        result.Data = deliverySysno;
                        result.Message = "确认发货完成";
                        tran.Complete();
                    }
                }
                else //当为False时表示不执行事务
                {
                    deliverySysno = LgDeliveryBo.Instance.NewCreateLgDelivery(warehouseSysNo, deliveryUserSysNo,
                            delivertType,
                            0, itemList, isForce, ref deliveryMsgs, "");

                    result.Status = true;
                    result.Data = deliverySysno;
                    result.Message = "确认发货完成";
                }
                //2014-05-09 黄波/何永东/杨文兵 添加
                try
                {
                    #region 发送相关短消息
                    //发送相关消息
                    foreach (var msg in deliveryMsgs)
                    {
                        //Order.SoOrderBo.Instance.WriteSoTransactionLog(msg.StockOutTransactionSysNo,
                        //                                                      "出库单" + msg.StockOutSysNo + "已配送，待结算",
                        //                                                      msg.OperationUserName);
                        //获取订单信息
                        SoOrder SData = Hyt.BLL.Order.SoOrderBo.Instance.GetEntityNoCache(msg.OrderSysNo);
                        //不是三方快递
                        if (msg.IsThirdPartyExpress == 0)
                        {

                            //smsBo.发送自建物流发货通知短信(msg.MobilePhoneNum, msg.OrderSysNo.ToString(),msg.UserName, msg.UserMobilePhoneNum);
                            //new BLL.Extras.EmailBo().发送百城当日达发货邮件(msg.CustomerEmailAddress, msg.CustomerSysNo.ToString(),
                            //                                    msg.OrderSysNo.ToString(), msg.UserName,
                            //                                    msg.UserMobilePhoneNum);

                            //2015-10-30 王耀发 创建 对应分销商
                            CBDsDealer Dealer = new Hyt.BLL.Distribution.DsDealerBo().GetDsDealer(SData.DealerSysNo);
                            if (Dealer == null)
                            {
                                Dealer = new CBDsDealer();
                            }
                            string context = "尊敬的客户，您在商城下达的订单(" + SData.OrderNo.ToString() + ")已经由" + msg.DeliveryTypeName + "开始处理，单号" + msg.ExpressNo + "，您可前往" + msg.TraceUrl + "查看物流状态，谢谢！";
                            Hyt.BLL.Extras.SmsBO.Instance.DealerSendMsg(msg.MobilePhoneNum, Dealer.DealerName, context, DateTime.Now);

                            SData.Status = (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.出库待接收;
                            SData.OnlineStatus = Constant.OlineStatusType.已发货;
                            UpdateOrder(SData);
                        }

                        if (msg.IsThirdPartyExpress == 1)
                        {
                            //2015-10-30 王耀发 创建 对应分销商
                            CBDsDealer Dealer = new Hyt.BLL.Distribution.DsDealerBo().GetDsDealer(SData.DealerSysNo);
                            if (Dealer == null)
                            {
                                Dealer = new CBDsDealer();
                            }
                            string context = "尊敬的客户，您在商城下达的订单(" + SData.OrderNo.ToString() + ")已经由" + msg.DeliveryTypeName + "开始处理，单号" + msg.ExpressNo + "，您可前往" + msg.TraceUrl + "查看物流状态，谢谢！";
                            Hyt.BLL.Extras.SmsBO.Instance.DealerSendMsg(msg.MobilePhoneNum, Dealer.DealerName, context, DateTime.Now);

                        }

                        #region 发送微信模板消息

                        GeneralConfig config = BLL.Config.Config.Instance.GetGeneralConfig();
                        if (config.IsSendWeChatTempMessage == 1)
                        {
                            SendMessage(SData.SysNo, msg.DeliveryTypeName, msg.ExpressNo);
                        }

                        #endregion
                    }

                    #endregion


                    //回填物流信息
                    try
                    {
                        LgDeliveryBo.Instance.BackFillLogisticsInfo(deliverySysno, deliverTypeSysno);
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

            return result;

        }
        #endregion
        #endregion

        #region 发送微信模板消息
        private void SendMessage(int orderSysno, string DeliveryTypeName, string ExpressNo)
        {
            var orderInfo = BLL.Order.SoOrderBo.Instance.GetEntity(orderSysno);
            var payInfo = BLL.Basic.PaymentTypeBo.Instance.GetEntity(orderInfo.PayTypeSysNo);
            var customer = BLL.CRM.CrCustomerBo.Instance.GetModel(orderInfo.CustomerSysNo);
            #region 发送微信模板消息
            var dealerInfo = Hyt.BLL.Stores.StoresBo.Instance.GetStoreById(orderInfo.DealerSysNo);
            //var syUserInfo = Hyt.BLL.Sys.SyUserBo.Instance.GetSyUser(dealerInfo.UserSysNo);
            SoReceiveAddress address = Hyt.BLL.Order.SoOrderBo.Instance.GetOrderReceiveAddress(orderInfo.ReceiveAddressSysNo);
            string keynote1 = DateTime.Now.ToString("yyyyMMdd mmss");
            //string keynote2 = "...";
            string keynote3 = "";
            string remark = "";
            string LinkUrl = "";

            if (address != null)
            {
                string areaFullName = BLL.Basic.BasicAreaBo.Instance.GetAreaFullName((address.AreaSysNo));
                remark = "收货详情：" + address.Name + "-" + address.MobilePhoneNumber + "-" + areaFullName + address.StreetAddress;
            }
            string data = "";
            int i = 0;
            foreach (var item in Hyt.BLL.Order.SoOrderBo.Instance.GetOrderItemsByOrderId(orderSysno))
            {
                if (Hyt.BLL.Order.SoOrderBo.Instance.GetOrderItemsByOrderId(orderSysno).Count > 1)
                {
                    i++;
                    keynote3 += i + "." + item.ProductName + ";";
                }
                else
                {
                    keynote3 += item.ProductName;
                }
            }
            string tempcodeone = "y91mDfeSLInbn3_QgjFbjP5IxuZ0GIEltto2rDo1FKo";//默认为总部公众号模板
            //int storeid = BLL.Stores.WebStoreContext.Instance.StoreId;
            switch (orderInfo.DealerSysNo)
            {
                case 0://总部
                    tempcodeone = "y91mDfeSLInbn3_QgjFbjP5IxuZ0GIEltto2rDo1FKo";//订单发货
                    break;
            }

            LinkUrl = "http://m.yoyo2o.com/Member/GetMemberOrders?orderStatus=30";
            data = Hyt.BLL.Weixin.WeiXinTemplateBo.Instance.GetNewOrderNotice(customer.Openid, tempcodeone, LinkUrl, "HI，客官，您已购买的商品已经发货了，订单编号：" + orderInfo.OrderNo + "。点击查看详情。", keynote3, DeliveryTypeName, ExpressNo, remark, "感谢亲对我们的支持！客服电话：" + dealerInfo.PhoneNumber);

            Hyt.BLL.Weixin.CallCenterReplyBo.Instance.SendTemplateMessage(orderInfo.DealerSysNo, data);
            #endregion
        }
        #endregion


        /// <summary>
        /// 获取超时为确认收货的订单编号列表
        /// </summary>
        /// <param name="timeOutDay">超时天数</param>
        /// <returns></returns>
        /// <remarks>2016-5-13 杨浩 创建</remarks>
        public IList<SoOrder> GetConfirmReceiptOrderSysNoList(int timeOutDay)
        {
            return ISoOrderDao.Instance.GetConfirmReceiptOrderSysNoList(timeOutDay);
        }
        /// <summary>
        /// 自动确认收货
        /// </summary>
        /// <param name="timeOutDay">订单超时天数</param>
        /// <remarks>2016-5-13 杨浩 创建</remarks>
        public void AutoConfirmationOfReceipt(int timeOutDay)
        {
            var orderList = GetConfirmReceiptOrderSysNoList(timeOutDay);
            foreach (var orderInfo in orderList)
            {
                UpdateOrderStatusAndOnlineStatus(orderInfo, "系统",
                       Constant.OlineStatusType.已完成, (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.已完成);

            }
        }

        /// <summary>
        /// 更新订单状态
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <param name="orderStatusText">订单状态文本</param>
        /// <param name="orderStatus">订单状态</param>
        /// <returns></returns>
        /// <remarks>2016-5-13 杨浩 创建</remarks>
        public int UpdateOrderStatus(int orderSysNo, string orderStatusText, int orderStatus)
        {
            return ISoOrderDao.Instance.UpdateOrderStatus(orderSysNo, orderStatusText, orderStatus);
        }
        /// <summary>
        /// 更新订单配送方式
        /// </summary>
        /// <param name="orderSysNo"></param>
        /// <param name="DeliveryTypeSysNo"></param>
        /// <returns></returns>
        public int UpdateOrderDeliveryType(int orderSysNo, int DeliveryTypeSysNo)
        {
            return ISoOrderDao.Instance.UpdateOrderDeliveryType(orderSysNo, DeliveryTypeSysNo);
        }
        /// <summary>
        /// 更新订单在线状态
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <param name="userName">用户名</param>
        /// <param name="orderStatusText">订单状态值</param>
        /// <param name="orderStatus">订单状态</param>
        /// <remarks>2016-1-5 杨浩 添加注释</remarks>
        public void UpdateOrderStatusAndOnlineStatus(SoOrder orderInfo, string userName, string orderStatusText, int orderStatus)
        {
            ISoOrderDao.Instance.UpdateOrderStatus(orderInfo.SysNo, orderStatusText, orderStatus);

            IList<LgDeliveryItem> deItemlist = Hyt.DataAccess.Logistics.ILgDeliveryDao.Instance.GetLgDeliveryItemListByTransactionSysNo(orderInfo.TransactionSysNo);
            foreach (LgDeliveryItem item in deItemlist)
            {
                Hyt.DataAccess.Logistics.ILgDeliveryDao.Instance.UpdateDeliveryItemStatus(item.SysNo, LogisticsStatus.配送单明细状态.已签收);
            }
            WriteSoTransactionLog(orderInfo.TransactionSysNo, "确认收货，订单完成", userName);
        }

        /// <summary>
        /// 更新订单API执行状态
        /// </summary>
        /// <param name="status">状态值</param>
        /// <param name="type">API类型(0:海关支付报关状态 1:商检状态 2:海关订单状态)</param>
        /// <param name="orderSysNo">订单编号</param>
        /// <returns></returns>
        /// <remarks>2015-12-28 杨浩 创建</remarks>
        public bool UpdateOrderApiStatus(int status, int apiType, int orderSysNo)
        {
            return ISoOrderDao.Instance.UpdateOrderApiStatus(status, apiType, orderSysNo) > 0;
        }

        #region 通过购物车修改订单 杨文兵

        /// <summary>
        /// 修改订单之前的操作
        /// </summary>
        /// <param name="editOrderModel">修改之前的界面Model</param>
        /// <returns>void</returns>
        /// <remarks>2013-11-15 创建 杨文兵</remarks>
        public void EditOrderBefore(EditOrderModel model)
        {
            var cacheKey = "EditOrder" + model.Order.SysNo;
            Hyt.BLL.CRM.CrShoppingCartToCacheBo.Instance.RemoveAll(cacheKey, model.Order.CustomerSysNo);
            var cartItemList = model.ShoppingCart.GetShoppingCartItem(model.Order.CustomerSysNo);
            Hyt.BLL.CRM.CrShoppingCartToCacheBo.Instance.SetCacheItems(cacheKey, cartItemList);
        }

        /// <summary>
        /// 刷新购物车金额，不保存到数据库
        /// </summary>
        /// <param name="model">购物车</param>
        /// <param name="newAreaNo">新的地区</param>
        /// <param name="newDeliveryTypeSysNo">新的配送方式</param>
        /// <returns>新购物车 </returns>
        /// <remarks>2013-11-15 创建 杨文兵</remarks>
        public EditOrderModel RefreshOrderCash(EditOrderModel model, int? newAreaNo, int? newDeliveryTypeSysNo)
        {
            var oldOrderItemList = model.OrderItems;
            model.Order = GetEntity(model.Order.SysNo);  //重新获取订单数据，让订单Model保持最新
            var cacheKey = "EditOrder" + model.Order.SysNo;
            Hyt.BLL.CRM.CrShoppingCartToCacheBo.Instance.CheckedAll(cacheKey, model.Order.CustomerSysNo);
            var newCart = Hyt.BLL.CRM.CrShoppingCartToCacheBo.Instance.GetShoppingCart(cacheKey, new[] { model.Order.GetPromotionPlatformType() }, model.Order.CustomerSysNo, null,
                newAreaNo, newDeliveryTypeSysNo, null, model.ShoppingCart.CouponCode);
            model.Order.ProductAmount = newCart.ProductAmount;
            model.Order.ProductDiscountAmount = newCart.ProductDiscountAmount;
            model.Order.FreightDiscountAmount = newCart.FreightDiscountAmount;
            model.Order.OrderAmount = newCart.SettlementAmount;
            model.Order.FreightAmount = newCart.FreightAmount;
            model.Order.OrderDiscountAmount = newCart.SettlementDiscountAmount;
            model.Order.CouponAmount = newCart.CouponAmount;
            model.Order.CashPay = model.Order.OrderAmount - model.Order.CoinPay;
            model.ShoppingCart = newCart;
            return model;
        }

        /// <summary>
        /// 修改订单之后的业务操作
        /// </summary>
        /// <param name="model">修改之前的界面Model.</param>
        /// <returns>订单修改界面交互Model</returns>
        /// <remarks>
        /// 1.传入的修改之前的Model 返回修改之后的Model
        /// 2.注意：修改之前的Model 包括调价之后的价格。调价字段为特殊处理
        /// 余勇 修改获取发票方法，改为执行bll方法 2014-05-14
        /// </remarks>
        public EditOrderModel EditOrderAfter(EditOrderModel model)
        {
            var oldOrderItemList = model.OrderItems;
            model.Order = GetEntity(model.Order.SysNo);  //重新获取订单数据，让订单Model保持最新
            var cacheKey = "EditOrder" + model.Order.SysNo;
            int? addressAreaNo = null;
            if (model.Order.ReceiveAddressSysNo > 0)
            {
                var receiveAddress = SoOrderBo.Instance.GetOrderReceiveAddress(model.Order.ReceiveAddressSysNo);
                if (receiveAddress != null) addressAreaNo = receiveAddress.AreaSysNo;
            }
            Hyt.BLL.CRM.CrShoppingCartToCacheBo.Instance.CheckedAll(cacheKey, model.Order.CustomerSysNo);
            var newCart = Hyt.BLL.CRM.CrShoppingCartToCacheBo.Instance.GetShoppingCart(cacheKey, new[] { model.Order.GetPromotionPlatformType() }, model.Order.CustomerSysNo, null,
                addressAreaNo, model.Order.DeliveryTypeSysNo, null, model.ShoppingCart.CouponCode, false, false);
            var newOrderItemList = Hyt.BLL.CRM.CrShoppingCartToCacheBo.Instance.ShoppingCartToOrderItem(cacheKey, model.Order.SysNo, model.Order.TransactionSysNo, newCart);

            //删除旧的明细数据
            foreach (var item in oldOrderItemList)
            {
                ISoOrderItemDao.Instance.Delete(item.SysNo);
            }
            //删除订单所有的优惠券
            ISoOrderDao.Instance.DeleteSoCoupon(model.Order.SysNo);
            //处理明细调价
            foreach (var item in newOrderItemList)
            {
                var olditem = oldOrderItemList.FirstOrDefault(p => p.ProductSysNo == item.ProductSysNo && string.IsNullOrEmpty(p.GroupCode) && p.ProductSalesType != (int)CustomerStatus.商品销售类型.赠品);
                if (olditem != null && string.IsNullOrEmpty(item.GroupCode) && item.ProductSalesType != (int)CustomerStatus.商品销售类型.赠品)
                {
                    item.ChangeAmount = olditem.ChangeAmount;
                }
            }
            //插入新的明细数据
            foreach (var item in newOrderItemList)
            {
                ISoOrderItemDao.Instance.Insert(item);
            }
            //step3：更新销售单主表金额相关字段
            model.Order.ProductAmount = newCart.ProductAmount;
            model.Order.ProductDiscountAmount = newCart.ProductDiscountAmount;
            model.Order.ProductChangeAmount = 0;
            model.Order.FreightDiscountAmount = newCart.FreightDiscountAmount;
            model.Order.FreightChangeAmount = 0;
            model.Order.OrderAmount = newCart.SettlementAmount;
            model.Order.FreightAmount = newCart.FreightAmount;
            model.Order.OrderDiscountAmount = newCart.SettlementDiscountAmount;
            model.Order.CouponAmount = newCart.CouponAmount;
            model.Order.CashPay = model.Order.OrderAmount - model.Order.CoinPay;
            model.Order.ProductChangeAmount = newOrderItemList.Sum(p => p.ChangeAmount);

            //调价处理

            UpdateOrder(model.Order); //更新订单 余勇 修改为调用业务层方法
            //ISoOrderDao.Instance.Update(model.Order);

            //使用了优惠券
            if (string.IsNullOrEmpty(newCart.CouponCode) == false)
            {
                var coupon = Hyt.BLL.Promotion.PromotionBo.Instance.GetCoupon(newCart.CouponCode);
                if (coupon != null)
                {
                    ISoOrderDao.Instance.InsertSoCoupon(new SoCoupon()
                    {
                        CouponSysNo = coupon.SysNo,
                        OrderSysNo = model.Order.SysNo
                    });
                }
            }
            //更新收款单
            Hyt.BLL.Finance.FnReceiptVoucherBo.Instance.UpdateOrderIncomeAmount(model.Order.SysNo, model.Order.CashPay);//更新订单收款单，应收金额
            //更新发票金额
            if (model.Order.InvoiceSysNo > 0)
            {
                var invoice = FnInvoiceBo.Instance.GetFnInvoice(model.Order.InvoiceSysNo); //Hyt.DataAccess.Order.IFnInvoiceDao.Instance.GetFnInvoice(model.Order.InvoiceSysNo);
                invoice.InvoiceAmount = model.Order.CashPay;
                UpdateOrderInvoice(invoice);
            }
            model.OrderItems = newOrderItemList;
            model.ShoppingCart = newCart;
            return model;

        }

        /// <summary>
        /// 修改订单相关数据和订单明细数据
        /// </summary>
        /// <param name="so">订单对象</param>
        /// <param name="editModel">订单修改界面交互Model</param>
        /// <returns>是否修改成功</returns>
        ///  <param name="expensesAmount">太平洋保险</param>
        /// <remarks>
        /// 2013-11-15 创建 杨文兵
        /// </remarks>
        public bool EditOrder(SoOrder so, EditOrderModel editModel, decimal expensesAmount = 0M)
        {
            var cartModel = new EditOrderCart(editModel.Order.CustomerSysNo, editModel.JsonCartItem
                    , editModel.AreaSysNo
                    , editModel.DeliveryTypeSysNo
                    , editModel.CouponCode
                    , editModel.UsedPromotionSysNo
                    , so
                );

            var oldOrderItemList = Hyt.BLL.Order.SoOrderBo.Instance.GetOrderItemsByOrderId(so.SysNo);
            var newCart = cartModel.ToCrShoppingCart(new[] { so.GetPromotionPlatformType() }, expensesAmount);
            //调用接口将购物车数据转换成购物车明细，当前调用的接口第一个参数有问题
            var newOrderItemList = Hyt.BLL.CRM.CrShoppingCartToCacheBo.Instance.ShoppingCartToOrderItem(
                "", so.SysNo, so.TransactionSysNo, newCart);


            //删除旧的明细数据
            foreach (var item in oldOrderItemList)
            {
                ISoOrderItemDao.Instance.Delete(item.SysNo);
            }
            //删除订单所有的优惠券
            ISoOrderDao.Instance.DeleteSoCoupon(editModel.Order.SysNo);
            //处理明细调价
            foreach (var item in newOrderItemList)
            {
                var olditem = editModel.OrderItems.FirstOrDefault(p => p.ProductSysNo == item.ProductSysNo && string.IsNullOrEmpty(p.GroupCode) && p.ProductSalesType != (int)CustomerStatus.商品销售类型.赠品);
                if (olditem != null && string.IsNullOrEmpty(item.GroupCode) && item.ProductSalesType != (int)CustomerStatus.商品销售类型.赠品)
                {
                    item.ChangeAmount = olditem.ChangeAmount;
                }
            }
            //插入新的明细数据
            foreach (var item in newOrderItemList)
            {
                ISoOrderItemDao.Instance.Insert(item);
            }
            //更新销售单主表金额相关字段
            so.ProductAmount = newCart.ProductAmount;
            so.ProductDiscountAmount = newCart.ProductDiscountAmount;
            so.FreightDiscountAmount = newCart.FreightDiscountAmount;
            so.FreightChangeAmount = 0;
            so.OrderAmount = newCart.SettlementAmount;
            so.FreightAmount = newCart.FreightAmount;
            so.TaxFee = newCart.TaxFee;
            so.OrderDiscountAmount = newCart.SettlementDiscountAmount;
            so.CouponAmount = newCart.CouponAmount;
            so.ProductChangeAmount = newOrderItemList.Sum(p => p.ChangeAmount);

            //调价
            so.OrderAmount = so.OrderAmount + so.ProductChangeAmount;
            so.CashPay = so.OrderAmount - so.CoinPay;
            UpdateOrder(so); //更新订单 余勇 修改为调用业务层方法
            //Hyt.DataAccess.Order.ISoOrderDao.Instance.Update(so);

            SoOrderBo.Instance.SaveSoReceiveAddress(so.ReceiveAddress);

            //使用了优惠券
            if (string.IsNullOrEmpty(newCart.CouponCode) == false)
            {
                var coupon = Hyt.BLL.Promotion.PromotionBo.Instance.GetCoupon(newCart.CouponCode);
                if (coupon != null)
                {
                    ISoOrderDao.Instance.InsertSoCoupon(new SoCoupon()
                    {
                        CouponSysNo = coupon.SysNo,
                        OrderSysNo = so.SysNo
                    });
                }
            }
            //更新收款单
            Hyt.BLL.Finance.FnReceiptVoucherBo.Instance.UpdateOrderIncomeAmount(so.SysNo, so.CashPay);//更新订单收款单，应收金额
            //更新发票金额
            if (so.InvoiceSysNo > 0)
            {
                var invoice = FnInvoiceBo.Instance.GetFnInvoice(so.InvoiceSysNo); // Hyt.DataAccess.Order.IFnInvoiceDao.Instance.GetFnInvoice(so.InvoiceSysNo);
                invoice.InvoiceAmount = so.CashPay;
                UpdateOrderInvoice(invoice);
            }


            return true;
        }

        #endregion

        #region 选择创建会员
        /// <summary>
        /// 查询会员
        /// </summary>
        /// <param name="sysNo">sysNo</param>
        /// <returns>会员</returns>
        /// <remarks>2013－06-13 黄志勇 创建</remarks>
        public Model.CrCustomer SearchCustomer(int sysNo)
        {
            return Hyt.DataAccess.CRM.ICrCustomerDao.Instance.GetCrCustomerItem(sysNo);
        }

        /// <summary>
        /// 查询会员(模糊查询,返回前30条数据,手机号4位开始查询)
        /// </summary>
        /// <param name="word">手机，会员名称</param>
        /// <returns>会员列表</returns>
        /// <remarks>2013－06-09 黄志勇 创建</remarks>
        /// <remarks>2013－07-25 黄志勇 修改</remarks>
        public IList<Model.CrCustomer> SearchCustomer(string word)
        {
            return SearchCustomer(word, 30, 4);
        }

        /// <summary>
        /// 查询会员(模糊查询)
        /// </summary>
        /// <param name="word">手机，会员名称</param>
        /// <param name="rownum">返回最大条数</param>
        /// <param name="minLength">手机号最小长度</param>
        /// <returns>会员列表</returns>
        /// <remarks>2013－07-25 黄志勇 创建</remarks>
        public IList<Model.CrCustomer> SearchCustomer(string word, int rownum, int minLength = 0)
        {
            if (VHelper.ValidatorRule(new Rule_Number(word)).IsPass)
            {
                if (word.Length < minLength) return null;
                return ICrCustomerDao.Instance.SearchCustomerByAccount(word, rownum);
            }
            return ICrCustomerDao.Instance.SearchCustomerByName(word, rownum);
        }

        /// <summary>
        /// 根据经销商筛选会员
        /// </summary>
        /// <param name="word">关键字</param>
        /// <param name="dealer">经销商id</param>
        /// <returns></returns>
        public IList<Model.CrCustomer> SearchCustomer(string word, int dealer)
        {
            return ICrCustomerDao.Instance.SearchCustomer(word, dealer);
        }

        /// <summary>
        /// 创建一个会员
        /// </summary>
        /// <param name="customer">会员</param>
        /// <param name="address">收货地址</param>
        /// <returns>true:成功 false:失败</returns>
        /// <remarks>2013－06-09 黄志勇 创建</remarks>
        /// <remarks>2014－06-27 杨浩 修改</remarks>
        public bool CreateCustomer(CrCustomer customer, CrReceiveAddress address)
        {
            //调用wcf服务获取SSO客户信息并将SsOId和CustomerSysNo保存到关系表 (余勇 2014-06-26)
            //var ssoid = CrSsoCustomerAssociationBo.Instance.InsertCrSsoCustomerAssociation(customer);
            //if (ssoid > 0)
            //{
            //var ssoinfo = CrSsoCustomerAssociationBo.Instance.GetBySsoID(ssoid);
            //if (ssoinfo == null)
            //{
            //添加会员
            //给用户密码加密 杨浩 2014-09-12
            customer.Password = EncryptionUtil.EncryptWithMd5AndSalt(customer.Password);
            var customerSysNo = ICrCustomerDao.Instance.CreateCustomer(customer);
            //给customer系统编号重新赋值，以便后续方法使用(杨浩 2014-06-26)
            customer.SysNo = customerSysNo;
            //添加收货地址
            if (address != null && !string.IsNullOrEmpty(address.Name) &&
                !string.IsNullOrEmpty(address.StreetAddress) &&
                address.AreaSysNo > 0 && VHelper.Do(address.MobilePhoneNumber, VType.Mobile))
            {
                address.CustomerSysNo = customerSysNo;
                ICrCustomerDao.Instance.CreateCustomerReceiveAddress(address);
            }

            ICrCustomerDao.Instance.UpdateCustomerSysNos(customer.SysNo, string.Format(",{0},", customer.SysNo));

            //将返回的SsOId和CustomerSysNo保存到关系表
            //CrSsoCustomerAssociationBo.Instance.Insert(new CrSsoCustomerAssociation
            //{
            //    CustomerSysNo = customer.SysNo,
            //SsoId = ssoid
            //});
            return true;
            //   }
            //   else
            //   {
            //       customer.SysNo = ssoinfo.CustomerSysNo;
            //   }
            //   return true;
            //}
            //return false;
        }

        #endregion

        #region 省市区连动相关方法
        /// <summary>
        /// 加载所有的省份
        /// </summary>
        /// <returns>省份列表</returns>
        /// <remarks>2013－06-09 朱成果 创建</remarks>
        public IList<BsArea> LoadProvince()
        {
            //优化 return Hyt.DataAccess.BaseInfo.IBsAreaDao.Instance.LoadProvince();
            return BasicAreaBo.Instance.GetAll().Where(m => m.Status == 1 && m.AreaLevel == 1).ToList();
        }

        /// <summary>
        /// 加载省份的所有城市
        /// </summary>
        /// <param name="provinceSysNo">省份编号</param>
        /// <returns>省份下面的市列表</returns>
        /// <remarks>2013－06-09 朱成果 创建</remarks>
        public IList<BsArea> LoadCity(int provinceSysNo)
        {
            //优化 return Hyt.DataAccess.BaseInfo.IBsAreaDao.Instance.LoadCity(provinceSysNo);
            return BasicAreaBo.Instance.SelectArea(provinceSysNo);
        }

        /// <summary>
        /// 加载城市下的所有区域
        /// </summary>
        /// <param name="citySysNo">城市编号</param>
        /// <returns>获取城市下面的地区列表</returns>
        /// <remarks>2013－06-09 朱成果 创建</remarks>
        public IList<BsArea> LoadArea(int citySysNo)
        {
            //优化 return Hyt.DataAccess.BaseInfo.IBsAreaDao.Instance.LoadArea(citySysNo);
            return BasicAreaBo.Instance.SelectArea(citySysNo);
        }

        /// <summary>
        /// 根据地区编号，获取省市区信息
        /// </summary>
        /// <param name="sysNo">地区编号</param>
        /// <param name="cityEntity">城市信息</param>
        /// <param name="areaEntity">地区信息</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-06-14 朱成果 创建
        /// </remarks>
        public Model.BsArea GetProvinceEntity(int sysNo, out Model.BsArea cityEntity, out Model.BsArea areaEntity)
        {
            //优化 return Hyt.DataAccess.BaseInfo.IBsAreaDao.Instance.GetProvinceEntity(sysNo, out cityEntity, out areaEntity);
            return BasicAreaBo.Instance.GetProvinceEntity(sysNo, out cityEntity, out areaEntity);
        }
        #endregion

        #region 收货地址相关业务

        /// <summary>
        /// 查询用户的收货地址
        /// </summary>
        /// <param name="customerSysNo">会员ID</param>
        /// <returns>收货地址列表</returns>
        /// <remarks>2013－06-09 黄志勇 创建</remarks>
        public IList<CrReceiveAddress> LoadCustomerAddress(int customerSysNo)
        {
            return Hyt.DataAccess.CRM.ICrCustomerDao.Instance.LoadCustomerAddress(customerSysNo);
        }

        /// <summary>
        /// 查询用户的默认收货地址
        /// </summary>
        /// <param name="customerSysNo">会员ID</param>
        /// <returns>收货地址列表</returns>
        /// <remarks>2014-03-18 周唐炬 创建</remarks>
        public CrReceiveAddress LoadCustomerDefaultAddress(int customerSysNo)
        {
            return ICrCustomerDao.Instance.LoadCustomerDefaultAddress(customerSysNo);
        }

        /// <summary>
        /// 根据收货地址ID获取收货地址
        /// </summary>
        /// <param name="sysno">地址</param>
        /// <returns>收货地址</returns>
        /// <remarks>2013－06-09 黄志勇 创建</remarks>
        public CrReceiveAddress GetCustomerAddressBySysNo(int sysno)
        {
            return Hyt.DataAccess.CRM.ICrCustomerDao.Instance.GetCustomerAddressBySysNo(sysno);
        }

        /// <summary>
        /// 根据收货地址ID获取收货地址
        /// </summary>
        /// <param name="soReceiveAddressSysNp">收件地址编号.</param>
        /// <returns>
        /// 收货地址
        /// </returns>
        /// <remarks>
        /// 2013/7/6 何方 创建
        /// </remarks>
        public string GetCustomerAddress(int soReceiveAddressSysNp)
        {
            var address = SoOrderBo.Instance.GetOrderReceiveAddress(soReceiveAddressSysNp);
            return address == null ? "未找到该收件地址" : BasicAreaBo.Instance.GetAreaFullName(address.AreaSysNo) + address.StreetAddress;
        }

        /// <summary>
        /// 修改会员收货地址
        /// </summary>
        /// <param name="address">收货地址实体</param>
        /// <returns>影响行数</returns>
        /// <remarks>2013－07-02 余勇 创建</remarks>
        public int UpdateReceiveAddress(Model.CrReceiveAddress address)
        {
            return Hyt.DataAccess.CRM.ICrReceiveAddressDao.Instance.UpdateReceiveAddress(address);
        }
        /// <summary>
        /// 添加会员收货地址
        /// </summary>
        /// <param name="address">收货地址实体</param>
        /// <returns>收货地址系统编号</returns>
        /// <remarks>2013－07-02 余勇 创建</remarks>
        public int InsertReceiveAddress(Model.CrReceiveAddress address)
        {
            return Hyt.DataAccess.CRM.ICrReceiveAddressDao.Instance.InsertReceiveAddress(address);
        }

        #endregion

        #region 配送方式 支付方式

        /// <summary>
        /// 获取所有配送方式
        /// </summary>
        /// <returns>配送方式列表</returns>
        /// <remarks>2013－06-13 黄志勇 创建</remarks>
        public IList<LgDeliveryType> LoadAllDeliveryType()
        {
            return Hyt.DataAccess.BaseInfo.IBasicShipDao.Instance.LoadAllDeliveryType();
        }

        /// <summary>
        /// 获取配送方式
        /// </summary>
        /// <param name="sysNo">配送方式编号</param>
        /// <returns>配送方式</returns>
        /// <remarks>2013－07-18 黄志勇 创建</remarks>
        public LgDeliveryType GetDeliveryType(int sysNo)
        {
            return Hyt.DataAccess.BaseInfo.IBasicShipDao.Instance.GetEntity(sysNo);
        }

        /// <summary>
        /// 获取所有支付方式
        /// </summary>
        /// <returns>配送方式列表</returns>
        /// <remarks>2013－06-13 黄志勇 创建</remarks>
        public IList<BsPaymentType> LoadAllPayType()
        {
            return Hyt.DataAccess.Basic.IBasicPayDao.Instance.LoadAllPayType();
        }

        /// <summary>
        /// 根据配送方式获取对应的支付方式列表
        /// </summary>
        /// <param name="deliverySysNo">配送方式</param>
        /// <returns>根据配送方式获取对应的支付方式列表</returns>
        /// <remarks>
        /// 2013-06-17 朱成果 创建
        /// </remarks>
        public IList<BsPaymentType> LoadPayTypeListByDeliverySysNo(int deliverySysNo)
        {
            return Hyt.DataAccess.Basic.IBasicPayDao.Instance.LoadPayTypeListByDeliverySysNo(deliverySysNo);
        }

        /// <summary>
        /// 是否支持同城配送(方法已废弃，验证百城当日送用地图上的点坐标来处理)
        /// </summary>
        /// <param name="areaSysNo">区域地址</param>
        /// <returns>true:支持，false:不支持</returns>
        /// <remarks>2013－06-13 黄志勇 创建</remarks>
        public bool IsInDeliveryScope(int areaSysNo)
        {
            //todo:朱成果
            return Hyt.DataAccess.BaseInfo.IBsAreaDao.Instance.InDeliveryArea(areaSysNo);
        }

        /// <summary>
        ///获取该区域支持的配送方式 
        /// </summary>
        /// <returns></returns>
        /// <remarks>2013－06-13 朱成果 创建</remarks>
        public IList<LgDeliveryType> LoadDeliveryTypeByAreaNo(int areaNo)
        {
            // var list= Hyt.DataAccess.Logistics.ILgDeliveryTypeDao.Instance.GetSubLgDeliveryTypeList(0);
            // List<LgDeliveryType> supportList = new List<LgDeliveryType>();
            //foreach (LgDeliveryType item in list)
            //{
            //    //检查是否支持同城配送
            //    if (item.SysNo == DeliveryType.百城当日达 && !IsInDeliveryScope(areaNo))
            //    {
            //        continue;
            //    }
            //    supportList.Add(item);
            //}
            IList<LgDeliveryType> list = Hyt.DataAccess.BaseInfo.IBasicShipDao.Instance.LoadAllDeliveryType();
            List<LgDeliveryType> supportList = new List<LgDeliveryType>();
            BuildSubList(0, areaNo, list, supportList, false);
            return supportList;
        }

        /// <summary>
        /// 获取该区域支持的配送方式
        /// </summary>
        /// <param name="areaNo">地区</param>
        /// <param name="cityNo">城市</param>
        /// <param name="x">经度</param>
        /// <param name="y">维度</param>
        /// <returns></returns>
        /// <remarks>2013－11-19 朱成果 创建</remarks>
        public IList<LgDeliveryType> LoadDeliveryTypeByAreaNo(int areaNo, int cityNo, double x, double y)
        {
            bool isInMap = Hyt.BLL.Logistics.LgDeliveryScopeBo.Instance.IsInScope(cityNo, new Coordinate() { X = x, Y = y });
            return LoadDeliveryTypeByAreaNo(areaNo, cityNo, isInMap);
        }

        /// <summary>
        /// 获取该区域支持的配送方式
        /// </summary>
        /// <param name="areaNo">地区</param>
        /// <param name="cityNo">城市</param>
        /// <param name="isInMap">是否在同城配送地图范围内</param>
        /// <returns></returns>
        /// <remarks>2013－11-19 朱成果 创建</remarks>
        public IList<LgDeliveryType> LoadDeliveryTypeByAreaNo(int areaNo, int cityNo, bool isInMap)
        {
            IList<LgDeliveryType> list = Hyt.DataAccess.BaseInfo.IBasicShipDao.Instance.LoadAllDeliveryType();
            List<LgDeliveryType> supportList = new List<LgDeliveryType>();
            BuildSubList(0, areaNo, list, supportList, isInMap);
            //BuildSubList(3, areaNo, list, supportList, isInMap);
            return supportList;
        }

        /// <summary>
        /// 构建配送方式选择级联显示列表
        /// </summary>
        /// <param name="ParentSysNo">上级编号</param>
        /// <param name="areaNo">地区编号</param>
        /// <param name="list">全部配送方式</param>
        /// <param name="supportList">可选的配送方式</param>
        /// <param name="isInMap">是否在同城配送地图内</param>
        /// <returns></returns>
        /// <remarks>2013－06-13 朱成果 创建</remarks>
        private void BuildSubList(int ParentSysNo, int areaNo, IList<LgDeliveryType> list, List<LgDeliveryType> supportList, bool isInMap = false)
        {
            var sublist = list.Where(m => m.ParentSysNo == ParentSysNo).ToList();
            if (sublist != null && sublist.Count > 0)
            {
                foreach (LgDeliveryType item in sublist)
                {
                    //if ((item.SysNo == DeliveryType.百城当日达 || item.ParentSysNo == DeliveryType.百城当日达) && !isInMap)
                    //{
                    //    item.DeliveryTypeName += "（强制)";
                    //}
                    if (ParentSysNo != 0)
                    {
                        item.DeliveryTypeName = "&nbsp;&nbsp;" + item.DeliveryTypeName;
                    }
                    supportList.Add(item);

                    //if (item.SysNo != DeliveryType.第三方快递)
                    //{
                    BuildSubList(item.SysNo, areaNo, list, supportList, isInMap);
                    //}
                }
            }
        }

        /// <summary>
        /// 获取该区域支持的配送方式
        /// </summary>
        /// <param name="areaNo">地区</param>
        /// <param name="cityNo">城市</param>
        /// <param name="isInMap">是否在同城配送地图范围内</param>
        /// <returns></returns>
        /// <remarks>2013－11-19 朱成果 创建</remarks>
        public IList<LgDeliveryType> LoadDeliveryTypeByAreaSysNo(int areaNo, int cityNo, bool isInMap)
        {
            IList<LgDeliveryType> list = Hyt.DataAccess.BaseInfo.IBasicShipDao.Instance.LoadAllDeliveryType();
            List<LgDeliveryType> supportList = new List<LgDeliveryType>();
            BuildSubListByAreaSysNo(0, areaNo, list, supportList, isInMap);
            return supportList;
        }

        /// <summary>
        /// 构建配送方式选择级联显示列表
        /// </summary>
        /// <param name="ParentSysNo">上级编号</param>
        /// <param name="areaNo">地区编号</param>
        /// <param name="list">全部配送方式</param>
        /// <param name="supportList">可选的配送方式</param>
        /// <param name="isInMap">是否在同城配送地图内</param>
        /// <returns></returns>
        /// <remarks>2013－06-13 朱成果 创建</remarks>
        private void BuildSubListByAreaSysNo(int ParentSysNo, int areaNo, IList<LgDeliveryType> list, List<LgDeliveryType> supportList, bool isInMap = false)
        {
            var sublist = list.Where(m => m.ParentSysNo == ParentSysNo).ToList();
            if (sublist != null && sublist.Count > 0)
            {
                foreach (LgDeliveryType item in sublist)
                {
                    //if ((item.SysNo == DeliveryType.百城当日达 || item.ParentSysNo == DeliveryType.百城当日达) && !isInMap)
                    //{
                    //    item.DeliveryTypeName += "（强制)";
                    //}
                    if (ParentSysNo != 0)
                    {
                        item.DeliveryTypeName = item.DeliveryTypeName;
                    }
                    supportList.Add(item);

                    //if (item.SysNo != DeliveryType.第三方快递)
                    //{
                    BuildSubListByAreaSysNo(item.SysNo, areaNo, list, supportList, isInMap);
                    //}
                }
            }
        }
        #endregion

        #region 商品选择相关
        ////商品搜索组件	

        ////    1.输入商品名称，编号等参数选择商品
        ////    2.可返回选择的单个商品和多个商品。

        ///// <summary>
        ///// 传入商品ID和会员Id，获取此商品的价格
        ///// </summary>
        ///// <param name="productSysNo"></param>
        ///// <param name="customerSysNo"></param>
        ///// <returns></returns>
        //public decimal GetProductPrice(int productSysNo, int customerSysNo)
        //{
        //    throw new NotImplementedException();
        //}

        #endregion

        #region 销售单相关
        /// <summary>
        /// 获取订单时间搓
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        ///<remarks>2013-11-7 朱成果 创建</remarks>
        public DateTime GetOrderStamp(int sysNo)
        {
            return ISoOrderDao.Instance.GetOrderStamp(sysNo);
        }

        /// <summary>
        /// 获取订单详情
        /// </summary>
        /// <param name="sysNo">订单编号</param>
        /// <returns>订单</returns>
        ///<remarks>2013-06-13 朱成果 创建
        ///2014-5-14 杨文兵 订单业务数据缓存5分钟，对订单业务数据的修改需要移除缓存
        ///2017-6-2 罗勤尧改为读取数据库实时数据 解决调价之后订单未支付问题
        /// </remarks>
        public SoOrder GetEntity(int sysNo)
        {
            //var cacheKey = string.Format("CACHE_SOORDER_{0}", sysNo);
            //return Hyt.Infrastructure.Memory.MemoryProvider.Default.Get<SoOrder>(cacheKey, 5, () =>
            //{
            //    return ISoOrderDao.Instance.GetEntity(sysNo);
            //}, CachePolicy.Absolute);
            return ISoOrderDao.Instance.GetEntity(sysNo);
        }
        /// <summary>
        /// 获取豆沙包签名需要的参数
        /// </summary>
        /// <param name="sysNo">订单号</param>
        /// <returns>2017-07-07 罗熙 创建</returns>
        public Signparameter GetSignparameter(int sysNo)
        {
            return ISoOrderDao.Instance.GetSignparameter(sysNo);
        }
        /// <summary>
        /// 获取配送方式，身份证，总价，(运费)，下单时间
        /// </summary>
        /// <param name="sysNo">订单号</param>
        /// <returns>2017-07-08 罗熙 创建</returns>
        public DouShabaoOrderParameter Getotherparameter(int sysNo)
        {
            return ISoOrderDao.Instance.Getotherparameter(sysNo);
        }
        /// <summary>
        /// 获取商品列表所需要的参数
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns>2017-07-10 罗熙 创建</returns>
        public ProductList GetProductlist(int sysNo)
        {
            return ISoOrderDao.Instance.GetProductlist(sysNo);
        }
        /// <summary>
        /// 获取订单详情列表商品图片
        /// </summary>
        /// <param name="sysNo">订单编号</param>
        /// <returns>订单商品图片</returns>
        ///<remarks>2016-08-04 罗远康 创建</remarks>
        public IList<PdProductImage> GetOrderProductImgUrl(int sysNo)
        {
            return ISoOrderDao.Instance.GetOrderProductImgUrl(sysNo);
        }
        /// <summary>
        /// 获取未缓存订单详情
        /// </summary>
        /// <param name="sysNo">订单编号</param>
        /// <returns>订单</returns>
        /// <returns>2014-06-27 周唐炬 创建</returns>
        public SoOrder GetEntityNoCache(int sysNo)
        {
            return ISoOrderDao.Instance.GetEntity(sysNo);
        }
        /// <summary>
        /// 根据订单编号获取订单详情
        /// </summary>
        /// <param name="orderNo">订单编号</param>
        /// <returns></returns>
        /// <remarks>2016-5-9 杨浩 创建</remarks>
        public SoOrder GetOrderByOrderNo(string orderNo)
        {
            return ISoOrderDao.Instance.GetOrderByOrderNo(orderNo);
        }
        /// <summary>
        /// 根据订单编号获取订单详情利嘉模板
        /// </summary>
        /// <param name="orderNo">订单编号</param>
        /// <returns></returns>
        /// <remarks>2016-5-9 杨浩 创建</remarks>
        public LiJiaOrderModel GetLiJiaOrderByOrderNo(int orderNo)
        {
            return ISoOrderDao.Instance.GetLiJiaOrderByOrderNo(orderNo);
        }
        /// <summary>
        /// 通过事物编号来获取订单
        /// </summary>
        /// <param name="transactionSysNo">事务编号</param>
        /// <returns>订单</returns>
        /// <remarks> 2013-09-06 朱家宏 创建</remarks>
        public SoOrder GetByTransactionSysNo(string transactionSysNo)
        {
            var sysno = Convert.ToInt32(System.Text.RegularExpressions.Regex.Replace(transactionSysNo, "T0*", ""
                , System.Text.RegularExpressions.RegexOptions.Compiled | System.Text.RegularExpressions.RegexOptions.IgnoreCase));

            return SoOrderBo.Instance.GetEntity(sysno);
        }

        /// <summary>
        /// 通过出库单号来获取订单
        /// </summary>
        /// <param name="outStockSysNo">出库单号</param>
        /// <returns>订单</returns>
        /// <remarks> 2013-09-13 郑荣华 创建</remarks>
        public SoOrder GetByOutStockSysNo(int outStockSysNo)
        {
            return ISoOrderDao.Instance.GetByOutStockSysNo(outStockSysNo);
        }

        /// <summary>
        /// 仅仅返回订单的时间戳和金额相关的信息
        /// </summary>
        /// <param name="sysNo">订单编号</param>
        /// <returns></returns>
        ///<remarks>2013-08-30 朱成果 创建</remarks>
        public SoOrderAmount GetSoOrderAmount(int sysNo)
        {
            return ISoOrderDao.Instance.GetSoOrderAmount(sysNo);
        }

        /// <summary>
        /// 获取订单收货地址
        /// </summary>
        /// <param name="sysNo">订单收货地址编号</param>
        /// <returns></returns>
        ///<remarks>2013-06-13 朱成果 创建</remarks>
        public SoReceiveAddress GetOrderReceiveAddress(int sysNo)
        {
            //缓存时间0.5小时  2014-05-14 朱家宏 修改
            return MemoryProvider.Default.Get(string.Format(KeyConstant.OrderReceiveAddress, sysNo), 30,
                () => ISoReceiveAddressDao.Instance.GetOrderReceiveAddress(sysNo));
        }

        /// <summary>
        /// 后台创建销售单
        /// </summary>
        /// <param name="customerSysno">会员编号</param>
        /// <param name="soReceiveAddress">收货地址实体</param>
        /// <param name="deliveryTypeSysNo">配送方式编号</param>
        /// <param name="payTypeSysNo">支付方式编号</param>
        /// <param name="defaultWarehouseSysNo">默认仓库编号</param>
        /// <param name="deliveryRemarks">配送备注</param>
        /// <param name="deliveryTime">配送时间段</param>
        /// <param name="customerMessage">会员留言</param>
        /// <param name="internalRemark">对内备注</param>
        /// <param name="contactBeforeDelivery">配送前是否联系</param>
        /// <param name="couponCode">优惠券代码.</param>
        /// <param name="soItems">销售单明细实体 （此参数不传 改为从购物车中获取明细实体 2013-9-25 by ywb）</param>
        /// <param name="invoice">发票实体</param>
        /// <param name="user">当前用户实体</param>
        /// <param name="coinPay">支付会员币</param>
        /// <param name="dealerSysNo">经销系统编号</param>
        /// <returns>
        /// true:成功，失败抛出异常
        /// </returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <remarks>
        /// 2013-06-21 杨浩 创建
        /// </remarks>
        public Result CreateSoOrder(int customerSysno
            , SoReceiveAddress soReceiveAddress
            , int deliveryTypeSysNo
            , int payTypeSysNo
            , int defaultWarehouseSysNo
            , string deliveryRemarks
            , string deliveryTime
            , string customerMessage
            , string internalRemark
            , int contactBeforeDelivery
            , string couponCode
            , SoOrderItem[] soItems, FnInvoice invoice, SyUser user, int coinPay = 0, int dealerSysNo = 0)
        {
            var currentTime = DateTime.Now;
            var so = new SoOrder();
            so.ContactBeforeDelivery = contactBeforeDelivery;
            so.CustomerSysNo = customerSysno;
            so.DeliveryTypeSysNo = deliveryTypeSysNo;
            so.PayTypeSysNo = payTypeSysNo;
            so.DefaultWarehouseSysNo = defaultWarehouseSysNo;
            so.DeliveryRemarks = deliveryRemarks;
            so.DeliveryTime = deliveryTime;
            so.CustomerMessage = customerMessage;
            so.InternalRemarks = internalRemark;
            so.CreateDate = currentTime;
            so.LastUpdateBy = user.SysNo;
            so.LastUpdateDate = currentTime;
            so.OrderCreatorSysNo = user.SysNo;
            so.OrderSource = (int)OrderStatus.销售单来源.客服下单;
            so.OrderSourceSysNo = user.SysNo;
            so.PayStatus = (int)OrderStatus.销售单支付状态.未支付;
            so.SalesType = (int)OrderStatus.销售方式.普通订单;
            so.Status = (int)OrderStatus.销售单状态.待审核;
            so.SendStatus = (int)OrderStatus.销售单推送状态.未推送;
            var payType = PaymentTypeBo.Instance.GetPaymentTypeFromMemory(payTypeSysNo);
            so.OnlineStatus = payType.PaymentType == (int)BasicStatus.支付方式类型.到付 ? Constant.OlineStatusType.待审核 : Constant.OlineStatusType.待支付;
            so.DealerSysNo = dealerSysNo;
            var result = new Result { Status = false, Message = string.Empty, StatusCode = 0 };
            try
            {

                //获取购物车对象
                var shoppingCart = CrShoppingCartBo.Instance.GetShoppingCart(new[] { PromotionStatus.促销使用平台.PC商城 }, so.CustomerSysNo, null, soReceiveAddress.AreaSysNo, deliveryTypeSysNo, null, couponCode, false, false, defaultWarehouseSysNo);

                if (so == null || shoppingCart.ShoppingCartGroups.Count < 1 || user == null)
                    throw new ArgumentNullException();

                //创建订单
                var order = CreateOrder(user.SysNo, so.CustomerSysNo, soReceiveAddress, so.DefaultWarehouseSysNo,
                                        so.DeliveryTypeSysNo, so.PayTypeSysNo, shoppingCart, coinPay,
                                        invoice, (OrderStatus.销售单来源)so.OrderSource, so.OrderSourceSysNo,
                                        (OrderStatus.销售方式)so.SalesType, so.SalesSysNo,
                                        (OrderStatus.销售单对用户隐藏)so.IsHiddenToCustomer, so.CustomerMessage,
                                        so.InternalRemarks, so.DeliveryRemarks,
                                        so.DeliveryTime, (OrderStatus.配送前是否联系)so.ContactBeforeDelivery, so.Remarks, null, null, dealerSysNo);

                result.Status = true;
                result.StatusCode = order.SysNo;

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;

        }

        /// <summary>
        /// 后台创建销售单
        /// </summary>
        /// <param name="so">销售单实体</param>
        ///  <param name="soReceiveAddress">收货地址实体</param>
        /// <param name="soItems">销售单明细实体</param>
        /// <param name="invoice">发票实体</param>
        /// <param name="user">当前用户实体</param>
        /// <returns>返回json结果</returns>
        /// <remarks>
        /// 2013-07-30 黄志勇 创建
        /// 2013-09-16 朱家宏 修改:添加购物车 以CreateOrder方法创建订单
        /// 2013-9-25 杨文兵  后台客服创建订单不在调用此方法，此方法对“创建APP订单”使用
        /// </remarks>
        public Result CreateSoOrder(SoOrder so, SoReceiveAddress soReceiveAddress, SoOrderItem[] soItems,
                                    FnInvoice invoice, SyUser user)
        {
            var result = new Result { Status = false, Message = string.Empty, StatusCode = 0 };
            try
            {

                if (so == null || soItems == null || user == null)
                    throw new ArgumentNullException();

                //添加订单明细商品到购物车
                foreach (var item in soItems)
                {
                    CrShoppingCartBo.Instance.Add(so.CustomerSysNo, item.ProductSysNo, item.Quantity,
                                                  CustomerStatus.购物车商品来源.客服下单);
                }

                //获取购物车对象
                var shoppingCart = CrShoppingCartBo.Instance.GetShoppingCart(new[] { so.GetPromotionPlatformType() }, so.CustomerSysNo);

                //创建订单
                var order = CreateOrder(user.SysNo, so.CustomerSysNo, soReceiveAddress, so.DefaultWarehouseSysNo,
                                        so.DeliveryTypeSysNo, so.PayTypeSysNo, shoppingCart, 0,
                                        invoice, (OrderStatus.销售单来源)so.OrderSource, so.OrderSourceSysNo,
                                        (OrderStatus.销售方式)so.SalesType, so.SalesSysNo,
                                        (OrderStatus.销售单对用户隐藏)so.IsHiddenToCustomer, so.CustomerMessage,
                                        so.InternalRemarks, so.DeliveryRemarks,
                                        so.DeliveryTime, (OrderStatus.配送前是否联系)so.ContactBeforeDelivery, so.Remarks);

                //删除购物车对应商品
                var productSysNoList = new List<int>();
                foreach (var item in soItems)
                {
                    productSysNoList.Add(item.ProductSysNo);
                }
                CrShoppingCartBo.Instance.Remove(so.CustomerSysNo, productSysNoList.ToArray());

                result.Status = true;
                result.StatusCode = order.SysNo;

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 根据会员号和商品编号返回商品价格
        /// </summary>
        /// <param name="customerSysNo">会员ID</param>
        /// <param name="productSysNo">产品ID</param>
        /// <returns>商品价格</returns>
        /// <remarks>2013-06-21 黄志勇 创建</remarks>
        public decimal GetOriginalPrice(int customerSysNo, int productSysNo)
        {
            var list = Product.PdPriceBo.Instance.GetProductPrice(productSysNo);
            var levelItem = Hyt.DataAccess.CRM.ICrCustomerDao.Instance.GetCrCustomerItem(customerSysNo);
            if (levelItem == null)
            {
                throw new Exception("会员信息不存在");
            }
            int level = levelItem.LevelSysNo;
            var item = list.FirstOrDefault(i => i.PriceSource == (int)ProductStatus.产品价格来源.会员等级价 && i.SourceSysNo == level);
            if (item != null)
            {
                return item.Price;
            }
            else
            {
                throw new Exception("所选商品的当前会员等级对应的价格信息不存在.");
            }
        }

        /// <summary>
        /// 以数据库事物的方式保存订单
        /// 保存订单收货地址
        /// </summary>
        /// <returns>true:成功，失败抛出异常</returns>
        /// <remarks>2013-06-14 朱成果 创建</remarks>
        public bool SaveOrder(SoOrder soOrder)
        {
            UpdateOrder(soOrder); //更新订单 余勇 修改为调用业务层方法
            //Hyt.DataAccess.Order.ISoOrderDao.Instance.Update(soOrder);

            SoOrderBo.Instance.SaveSoReceiveAddress(soOrder.ReceiveAddress);

            return true;
        }

        /// <summary>
        /// 根据orderId获取订单明细
        /// </summary>
        /// <param name="orderId">订单id</param>
        /// <returns>订单明细集合</returns>
        /// <remarks>2013-06-21 朱成果 创建</remarks>
        public IList<SoOrderItem> GetOrderItemsByOrderId(int orderId)
        {
            return Hyt.DataAccess.Order.ISoOrderItemDao.Instance.GetOrderItemsByOrderSysNo(orderId);
        }

        /// <summary>
        /// 通过订单id集合获取商品明细
        /// </summary>
        /// <param name="SysNos"></param>
        /// <returns></returns>
        /// <remarks>2016-06-02 杨云奕 创建</remarks>
        public List<CBSoOrderItem> GetOrderItemsByOrderId(int[] SysNos)
        {
            return ISoOrderDao.Instance.GetOrderItemsByOrderId(SysNos);
        }

        /// <summary>
        /// 根据orderId获取订单明细
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <returns>订单明细集合</returns>
        /// <remarks>2013-06-21 王耀发 创建</remarks>
        public IList<CBSoOrderItem> GetCBOrderItemsByOrderId(int orderId)
        {
            return Hyt.DataAccess.Order.ISoOrderItemDao.Instance.GetCBSoOrderItemsByOrderSysNo(orderId);
        }

        ///<summary>
        /// 根据orderId获取订单明细
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <returns>订单明细(包括商品ERP code）集合</returns>
        /// <remarks>2013-07-26 朱成果 创建</remarks>
        public IList<CBSoOrderItem> GetOrderItemsWithErpCodeByOrderSysNo(int orderId)
        {
            return Hyt.DataAccess.Order.ISoOrderItemDao.Instance.GetOrderItemsWithErpCodeByOrderSysNo(orderId);
        }

        /// <summary>
        /// 根据订单号获取订单发票信息
        /// </summary>
        /// <param name="orderID">订单编号</param>
        /// <returns>发票实体</returns>
        /// <remarks>2013-06-21 朱成果 创建</remarks>
        public FnInvoice GetFnInvoiceByOrderID(int orderID)
        {
            var model = Hyt.DataAccess.Order.IFnInvoiceDao.Instance.GetFnInvoiceByOrderID(orderID);
            if (model == null)
            {
                model = new FnInvoice();
            }
            return model;
        }

        /// <summary>
        /// 获取订单发票信息
        /// </summary>
        /// <param name="sysNO">发票编号</param>
        /// <returns>订单发票实体</returns>
        /// <remarks>2013-06-21 朱成果 创建</remarks>
        public FnInvoice GetFnInvoice(int sysNO)
        {
            var model = FnInvoiceBo.Instance.GetFnInvoice(sysNO); //Hyt.DataAccess.Order.IFnInvoiceDao.Instance.GetFnInvoice(sysNO);
            if (model == null)
            {
                model = new FnInvoice();
            }
            return model;
        }

        /// <summary>
        /// 根据用户系统号获取订单明细
        /// </summary>
        /// <param name="customerSysNo">用户系统号</param>
        /// <returns>订单明细集合</returns>
        /// <remarks>2013-08-16 杨晗 创建</remarks>
        public IList<SoOrderItem> GetOrderItemsByCustomerSysNo(int customerSysNo)
        {
            return ISoOrderItemDao.Instance.GetOrderItemsByCustomerSysNo(customerSysNo);
        }

        /// <summary>
        /// 获取订单商品明细
        /// </summary>
        /// <param name="sysNo">明细编号</param>
        /// <returns>订单商品明细</returns>
        /// <remarks>2013-11-14 余勇 创建</remarks>
        public SoOrderItem GetOrderItem(int sysNo)
        {
            return ISoOrderItemDao.Instance.GetEntity(sysNo);
        }

        /// <summary>
        /// 判断某个用户是否购买此商品
        /// </summary>
        /// <param name="customerSysNo">用户系统号</param>
        /// <param name="productSysNo">商品系统号</param>
        /// <returns>true是，false否</returns>
        /// <remarks>2013-08-16 杨晗 创建</remarks>
        public bool IsCustomerSoProduct(int customerSysNo, int productSysNo)
        {
            var orderItems = GetOrderItemsByCustomerSysNo(customerSysNo);
            if (orderItems == null || orderItems.Count == 0)
            {
                return false;
            }
            var orderItem = orderItems.FirstOrDefault(c => c.ProductSysNo == productSysNo);
            return orderItem != null;
        }

        /// <summary>
        /// 判断某个用户是否购买此商品
        /// </summary>
        /// <param name="customerSysNo">用户系统号</param>
        /// <param name="productSysNo">商品系统号</param>
        /// <param name="soOrderSysNo">销售单系统号</param>
        /// <returns>true是，false否</returns>
        /// <remarks>2013-08-16 杨晗 创建</remarks>
        public bool IsCustomerSoProduct(int customerSysNo, int productSysNo, int soOrderSysNo)
        {
            var orderItems = GetOrderItemsByCustomerSysNo(customerSysNo);
            if (orderItems == null || orderItems.Count == 0)
            {
                return false;
            }
            var orderItem = orderItems.FirstOrDefault(c => c.ProductSysNo == productSysNo && c.OrderSysNo == soOrderSysNo);
            return orderItem != null;
        }
        #endregion

        #region 分销工具导入销售单

        /// <summary>
        /// 导入三方商城订单
        /// </summary>
        /// <param name="so">订单信息</param>
        /// <param name="soReceiveAddress">收货地址</param>
        /// <param name="soItems">订单明细</param>
        /// <param name="operatorName">操作人</param>
        /// <param name="onlinePayment">在线支付信息</param>
        /// <returns></returns>
        /// <remarks>2016-9-7 杨浩 创建</remarks>
        public Result ImportSoOrder(SoOrder so, SoReceiveAddress soReceiveAddress, SoOrderItem[] soItems, string operatorName, FnOnlinePayment onlinePayment, ref int exceptionPoint)
        {
            var result = new Result { Status = false, Message = string.Empty, StatusCode = 0 };
            //保存收货地址
            if (soReceiveAddress == null)
            {
                result.Message = "收货地址不能为空";
                return result;
            }


            //保存订单明细
            if (soItems == null || soItems.Length == 0)
            {
                result.Message = "销售单明细不能为空";
                return result;
            }

            ISoReceiveAddressDao.Instance.InsertEntity(soReceiveAddress);
            exceptionPoint = 100;
            so.ReceiveAddressSysNo = soReceiveAddress.SysNo;
            //保存订单
            ISoOrderDao.Instance.InsertEntity(so);
            exceptionPoint = 101;
            so = SoOrderBo.Instance.GetEntity(so.SysNo);
            exceptionPoint = 102;

            foreach (var item in soItems)
            {
                item.OrderSysNo = so.SysNo;
                item.TransactionSysNo = so.TransactionSysNo;
                item.SysNo = ISoOrderItemDao.Instance.Insert(item);
            }
            exceptionPoint = 103;
            //so.CashPay = SynchronousOrderAmount(so.SysNo, false);//同步订单价格


            if (onlinePayment != null)
            {
                onlinePayment.SourceSysNo = so.SysNo;
                IFnOnlinePaymentDao.Instance.Insert(onlinePayment);
                exceptionPoint = 107;

                Finance.FnReceiptVoucherBo.Instance.CreateReceiptVoucherByOrder(so);//创建订单收款单
                exceptionPoint = 104;
                var fvitem = new FnReceiptVoucherItem()
                {
                    Amount = so.CashPay,
                    CreatedDate = DateTime.Now,
                    PaymentTypeSysNo = Hyt.Model.SystemPredefined.PaymentType.易宝支付,
                    TransactionSysNo = so.TransactionSysNo,
                    Status = (int)Hyt.Model.WorkflowStatus.FinanceStatus.收款单明细状态.有效,
                    ReceivablesSideType = (int)Hyt.Model.WorkflowStatus.FinanceStatus.收款方类型.财务中心,//收款方来源
                    ReceivablesSideSysNo = so.OrderSourceSysNo,
                    VoucherNo = onlinePayment.VoucherNo,
                };

                Finance.FnReceiptVoucherBo.Instance.InsertOrderReceiptVoucher(so.SysNo, fvitem);
                //同步支付时间的到订单主表
                ISoOrderDao.Instance.UpdateOrderPayDteById(so.SysNo);
                exceptionPoint = 105;
                WriteSoTransactionLog(so.TransactionSysNo
                                      , string.Format("分销商订单创建成功，等待客服确认", so.SysNo)
                                      , operatorName);
                exceptionPoint = 106;

            }

            //写订单池记录
            SyJobPoolPublishBo.Instance.OrderAuditBySysNo(so.SysNo);
            exceptionPoint = 108;
            SyJobDispatcherBo.Instance.WriteJobLog(string.Format("分销商订单创建成功创建订单审核任务，销售单编号:{0}",
                          so.SysNo), so.SysNo, null, 0);
            exceptionPoint = 109;
            result.Status = true;
            result.StatusCode = so.SysNo;
            result.Message = so.TransactionSysNo;//事物编号

            return result;
        }

        /// <summary>
        /// 分销工具导入销售单
        /// </summary>
        /// <param name="so">销售单</param>
        /// <param name="soReceiveAddress">收货地址</param>
        /// <param name="soItems">订单明细</param>
        /// <param name="operatorName">操作人姓名</param>
        /// <returns></returns>
        /// <remarks>2013-09-09 朱成果 创建</remarks>
        /// <remarks>2013-10-28 黄志勇 修改</remarks>
        public Result ImportSoOrder(SoOrder so, SoReceiveAddress soReceiveAddress, SoOrderItem[] soItems, string operatorName)
        {
            var result = new Result { Status = false, Message = string.Empty, StatusCode = 0 };
            //保存收货地址
            if (soReceiveAddress == null)
            {
                result.Message = "收货地址不能为空";
                return result;
            }
            ISoReceiveAddressDao.Instance.InsertEntity(soReceiveAddress);
            so.ReceiveAddressSysNo = soReceiveAddress.SysNo;
            //保存订单
            ISoOrderDao.Instance.InsertEntity(so);
            so = SoOrderBo.Instance.GetEntity(so.SysNo);
            //保存订单明细
            if (soItems == null || soItems.Length == 0)
            {
                result.Message = "销售单明细不能为空";
                return result;
            }
            foreach (var item in soItems)
            {
                item.OrderSysNo = so.SysNo;
                item.TransactionSysNo = so.TransactionSysNo;
                item.SysNo = ISoOrderItemDao.Instance.Insert(item);
            }
            so.CashPay = SynchronousOrderAmount(so.SysNo, false);//同步订单价格
            Finance.FnReceiptVoucherBo.Instance.CreateReceiptVoucherByOrder(so);//创建订单收款单
            FnReceiptVoucherItem fvitem = new FnReceiptVoucherItem()
            {
                Amount = so.CashPay,
                CreatedDate = DateTime.Now,
                PaymentTypeSysNo = Hyt.Model.SystemPredefined.PaymentType.分销商预存,
                TransactionSysNo = so.TransactionSysNo,
                Status = (int)Hyt.Model.WorkflowStatus.FinanceStatus.收款单明细状态.有效,
                ReceivablesSideType = (int)Hyt.Model.WorkflowStatus.FinanceStatus.收款方类型.分销中心,//收款方来源
                ReceivablesSideSysNo = so.OrderSourceSysNo
            };
            Finance.FnReceiptVoucherBo.Instance.InsertOrderReceiptVoucher(so.SysNo, fvitem);
            //同步支付时间的到订单主表
            ISoOrderDao.Instance.UpdateOrderPayDteById(so.SysNo);
            WriteSoTransactionLog(so.TransactionSysNo
                                  , string.Format("订单升舱成功，等待客服确认", so.SysNo)
                                  , operatorName);

            //写订单池记录
            SyJobPoolPublishBo.Instance.OrderAuditBySysNo(so.SysNo);
            SyJobDispatcherBo.Instance.WriteJobLog(string.Format("订单升舱成功创建订单审核任务，销售单编号:{0}",
                          so.SysNo), so.SysNo, null, 0);
            result.Status = true;
            result.StatusCode = so.SysNo;
            result.Message = so.TransactionSysNo;//事物编号
            try
            {
                Finance.FnReceiptVoucherBo.Instance.AutoConfirmReceiptVoucher(so.SysNo, new SyUser { SysNo = 0, UserName = operatorName });
            }
            catch (Exception ex)
            {

                Hyt.BLL.Log.SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "分销商升舱自动确认收款单",
                                        LogStatus.系统日志目标类型.EAS, so.SysNo, ex, string.Empty, 0);
            }
            return result;
        }

        #endregion

        #region [订单分页查询]

        /// <summary>
        /// 订单分页查询
        /// </summary>
        /// <param name="pager">分页数据</param>
        /// <param name="filter">查询条件</param>
        /// <returns>void</returns>
        /// <remarks>2013-06-19 朱家宏 创建</remarks>
        /// <remarks>2013-11-28 黄志勇 修改 查询升舱订单商城类型</remarks>
        public void DoSoOrderQuery(ref Pager<CBSoOrder> pager, ParaOrderFilter filter)
        {
            filter = filter ?? new ParaOrderFilter();


            //var quickSearchKeyword = filter.Keyword;
            //if (!string.IsNullOrWhiteSpace(quickSearchKeyword))
            //{
            //    //手机号
            //    if (VHelper.ValidatorRule(new Rule_Mobile(quickSearchKeyword)).IsPass && quickSearchKeyword.Length >= 11)
            //        filter.ReceiveTel = quickSearchKeyword;
            //    //订单号
            //    else
            //    {
            //        var orderSysNo = 0;
            //        if (VHelper.ValidatorRule(new Rule_Number(quickSearchKeyword)).IsPass && int.TryParse(quickSearchKeyword, out orderSysNo))
            //            filter.OrderSysNo = orderSysNo;
            //        //会员名称
            //        //else
            //    }
            //    filter.CustomerAccount = quickSearchKeyword;
            //}

            ISoOrderDao.Instance.GetSoOrders(ref pager, filter);
            DsOrderBo.Instance.SetMallType(pager);
        }


        /// <summary>
        /// 订单分页查询
        /// </summary>
        /// <param name="pager">分页数据</param>
        /// <param name="filter">查询条件</param>
        /// <returns>void</returns>
        /// <remarks>2017-09-19 罗勤遥 创建</remarks>
        public void DoSoOrderQueryNew(ref Pager<CBSoOrder> pager, ParaOrderFilter filter)
        {
            filter = filter ?? new ParaOrderFilter();


            //var quickSearchKeyword = filter.Keyword;
            //if (!string.IsNullOrWhiteSpace(quickSearchKeyword))
            //{
            //    //手机号
            //    if (VHelper.ValidatorRule(new Rule_Mobile(quickSearchKeyword)).IsPass && quickSearchKeyword.Length >= 11)
            //        filter.ReceiveTel = quickSearchKeyword;
            //    //订单号
            //    else
            //    {
            //        var orderSysNo = 0;
            //        if (VHelper.ValidatorRule(new Rule_Number(quickSearchKeyword)).IsPass && int.TryParse(quickSearchKeyword, out orderSysNo))
            //            filter.OrderSysNo = orderSysNo;
            //        //会员名称
            //        //else
            //    }
            //    filter.CustomerAccount = quickSearchKeyword;
            //}
            ISoOrderDao.Instance.GetSoOrdersB2C(ref pager, filter);
            
            //ISoOrderDao.Instance.GetSoOrdersNew(ref pager, filter);
            DsOrderBo.Instance.SetMallType(pager);
        }

        /// <summary>
        /// 缺货订单
        /// </summary>
        /// <param name="pager">分页数据</param>
        /// <param name="filter">查询条件</param>
        /// <returns>void</returns>
        /// <remarks>2013-06-19 朱家宏 创建</remarks>
        public void DoSoOrderQueryForOutOfStock(ref Pager<CBSoOrder> pager, ParaOrderFilter filter)
        {
            //为0表示为缺货订单查询
            if (filter.StoreSysNoList == null)
                filter.StoreSysNoList = new List<int>();

            filter.StoreSysNoList.Add(0);
            DoSoOrderQuery(ref pager, filter);
        }

        /// <summary>
        /// 今日订单
        /// </summary>
        /// <param name="pager">分页数据</param>
        /// <param name="filter">查询条件</param>
        /// <returns>void</returns>
        /// <remarks>2013-06-19 朱家宏 创建</remarks>
        public void DoSoOrderQueryForToday(ref Pager<CBSoOrder> pager, ParaOrderFilter filter)
        {
            filter.BeginDate = DateTime.Today;
            DoSoOrderQuery(ref pager, filter);
        }

        #region 门店

        /// <summary>
        /// 门店订单查询
        /// </summary>
        /// <param name="pager">分页数据</param>
        /// <param name="filter">查询条件</param>
        /// <returns>void</returns>
        /// <remarks>2013-06-24 朱家宏 创建</remarks>
        public void DoSoOrderQueryFromStore(ref Pager<CBSoOrder> pager, ParaOrderFilter filter)
        {
            BuildBasicConditionForStore(filter);
            filter.DeliveryTypeSysNo = DeliveryType.门店自提;
            DoSoOrderQuery(ref pager, filter);
        }

        /// <summary>
        /// 门店今日订单
        /// </summary>
        /// <param name="pager">分页数据</param>
        /// <param name="filter">查询条件</param>
        /// <returns>void</returns>
        /// <remarks>2013-06-24 朱家宏 创建</remarks>
        public void DoSoOrderQueryFromStoreForToday(ref Pager<CBSoOrder> pager, ParaOrderFilter filter)
        {
            BuildBasicConditionForStore(filter);
            filter.DeliveryTypeSysNo = DeliveryType.门店自提;
            filter.BeginDate = DateTime.Today;

            DoSoOrderQuery(ref pager, filter);
        }

        /// <summary>
        /// 门店今日转快递
        /// </summary>
        /// <param name="pager">分页数据</param>
        /// <param name="filter">查询条件</param>
        /// <returns>void</returns>
        /// <remarks>2013-06-24 朱家宏 创建</remarks>
        public void DoSoOrderQueryFromStoreForDeliveryChanged(ref Pager<CBSoOrder> pager, ParaOrderFilter filter)
        {
            BuildBasicConditionForStore(filter);

            filter.BeginDate = DateTime.Today;

            //筛选不为门店自提的订单
            filter.ExceptedDeliveryTypeSysNo = DeliveryType.门店自提;

            filter.OrderSourceSysNoList = filter.StoreSysNoList;
            filter.OrderSource = (int)OrderStatus.销售单来源.门店下单;
            filter.StoreSysNoList = null;

            DoSoOrderQuery(ref pager, filter);
        }

        /// <summary>
        /// 门店今日已提货
        /// </summary>
        /// <param name="pager">分页数据</param>
        /// <param name="filter">查询条件</param>
        /// <returns>void</returns>
        /// <remarks>2013-06-24 朱家宏 创建</remarks>
        public void DoSoOrderQueryFromStoreForDelivered(ref Pager<CBSoOrder> pager, ParaOrderFilter filter)
        {
            BuildBasicConditionForStore(filter);
            filter.DeliveryTypeSysNo = DeliveryType.门店自提;

            filter.BeginDate = DateTime.Today;
            filter.OrderStatus = (int)OrderStatus.销售单状态.已完成;

            DoSoOrderQuery(ref pager, filter);
        }

        /// <summary>
        /// 门店订单查询基础过滤
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns>void</returns>
        /// <remarks>2013-07-08 朱家宏 创建</remarks>
        protected void BuildBasicConditionForStore(ParaOrderFilter filter)
        {
            if (filter.StoreSysNoList == null || !filter.StoreSysNoList.Any())
            {
                //门店筛选 
                if (filter.Warehouses != null && filter.Warehouses.Any())
                {
                    filter.StoreSysNoList =
                        filter.Warehouses.Where(o => o.WarehouseType == (int)WarehouseStatus.仓库类型.门店)
                              .Select(o => o.SysNo)
                              .ToList();
                }
                else
                {
                    filter.StoreSysNoList = new List<int> { -1 };
                }
            }
        }

        #endregion

        #endregion

        #region 订单审核 作废 主管锁定(锁定暂不实现)

        /// <summary>
        /// 审核订单操作
        /// </summary>
        /// <param name="orderSysNo">订单sysNo</param>
        /// <param name="operatorId">操作人</param>
        /// <param name="iscreatenewjob">是否创建一条出库的任务池记录(默认true)</param>
        /// <returns>true成功 false失败</returns>
        /// <remarks>2013-06-14 朱家宏 创建</remarks>
        /// <remarks>2013-07-22 朱成果 修改</remarks>
        /// <remarks>2013-10-12 黄志勇 修改</remarks>
        public bool AuditSoOrder(int orderSysNo, int operatorId, bool iscreatenewjob = true)
        {
            /*
             * 1、如果(订单支付状态==未支付&&支付类型==预付)则(销售单状态=待支付)
             * 2、如果(订单支付状态==已支付||支付类型==到付)则(销售单状态=待出库)
             * 3、升舱订单审核，首先判断上表EAS字段是否为空，若为空提示EAS编号未维护，审核不通过
             */
            var lstitem = Hyt.DataAccess.Order.ISoOrderItemDao.Instance.GetOrderItemsByOrderSysNo(orderSysNo);
            if (lstitem == null || lstitem.Count < 1)
            {
                throw new Exception("订购商品不能为空");
            }
            var soOrder = GetEntity(orderSysNo);
            if (soOrder.Status != (int)OrderStatus.销售单状态.待审核)
            {
                throw new Exception("订单状态不满足审核条件");
            }
            if (soOrder.OrderSource == (int)OrderStatus.销售单来源.分销商升舱)
            {
                var dsOrder = DsOrderBo.Instance.GetEntityByTransactionSysNo(soOrder.TransactionSysNo);
                if (dsOrder == null || dsOrder.Count == 0) throw new Exception("分销商升舱订单数据为空");
                var dealerSysNo = dsOrder.First().DealerMallSysNo;
                //var msg = DsEasBo.Instance.AuditOrder(dealerSysNo);
                //if (!string.IsNullOrEmpty(msg)) throw new Exception(msg);
            }
            var ptype = PaymentTypeBo.Instance.GetPaymentTypeFromMemory(soOrder.PayTypeSysNo);
            int payType = 0;
            if (ptype==null)
            {
                payType = (int)BasicStatus.支付方式类型.到付;
            }
            else
            {
                payType = ptype.PaymentType;
            }
            
            var modified = false;
            if (soOrder.PayStatus == (int)OrderStatus.销售单支付状态.未支付 &&
                payType == (int)BasicStatus.支付方式类型.预付)
            {
                soOrder.Status = (int)OrderStatus.销售单状态.待支付;
                soOrder.OnlineStatus = Constant.OlineStatusType.待支付;
                modified = true;
            }
            else if (soOrder.PayStatus == (int)OrderStatus.销售单支付状态.已支付 ||
                     payType == (int)BasicStatus.支付方式类型.到付)
            {
                soOrder.Status = (int)OrderStatus.销售单状态.待创建出库单;
                soOrder.OnlineStatus = Constant.OlineStatusType.待出库;
                modified = true;
            }

            soOrder.AuditorSysNo = operatorId;
            DateTime LastAuditorDate = soOrder.AuditorDate;//上次审核时间
            soOrder.AuditorDate = DateTime.Now;
            if (!modified) return false;


            try
            {
                var r = UpdateOrder(soOrder); //更新订单 余勇 修改为调用业务层方法 ISoOrderDao.Instance.Update(soOrder);


                if (!r) return false;
                if (LastAuditorDate <= System.Data.SqlTypes.SqlDateTime.MinValue)//第一次审核,更新销售量  
                {
                    foreach (SoOrderItem p in lstitem)
                    {
                        Hyt.BLL.Web.PdProductBo.Instance.UpdateProductSales(p.ProductSysNo, p.Quantity);//更新销售量

                    }
                }

                //List<WhWarehouseChangeLog> logList = WhWarehouseChangeLogBo.Instance.WarehouseLogList(soOrder.OrderNo);
                /////添加库存变动明细
                //foreach (SoOrderItem p in lstitem)
                //{
                //    WhWarehouseChangeLog log = logList.Find(q => q.ProSysNo == p.ProductSysNo);
                //    if (log == null)
                //    {
                //        WhWarehouseChangeLog log1 = new WhWarehouseChangeLog()
                //        {
                //            WareSysNo = soOrder.DefaultWarehouseSysNo,
                //            ProSysNo = p.ProductSysNo,
                //            ChageDate = soOrder.CreateDate,
                //            CreateDate = DateTime.Now,
                //            ChangeQuantity = Convert.ToInt32(p.Quantity) * -1,
                //            BusinessTypes = "网络订单出库",
                //            LogData = "出库单号：" + soOrder.OrderNo
                //        };
                //        WhWarehouseChangeLogBo.Instance.CreateMod(log1);
                //    }
                //    else if (log.ChangeQuantity != p.Quantity*-1)
                //    {
                //        log.ChangeQuantity = p.Quantity * -1;
                //        WhWarehouseChangeLogBo.Instance.UpdateMod(log);
                //    }
                //}

                if (iscreatenewjob)//任务池添加出库任务
                {
                    string customerName = soOrder.Customer != null ? soOrder.Customer.Name : "";
                    string mobilePhoneNumber = soOrder.Customer != null ? soOrder.Customer.MobilePhoneNumber : "";
                    //在任务池添加订单出库任务,同时将已处理的订单在任务池删除
                    SyJobPoolPublishBo.Instance.OrderWaitStockOut(soOrder.SysNo, customerName,
                                                                  mobilePhoneNumber, operatorId, (int)SystemStatus.任务对象类型.客服订单审核);
                    SyJobDispatcherBo.Instance.WriteJobLog(string.Format("订单审核后创建出库审核任务，销售单编号:{0}", soOrder.SysNo), soOrder.SysNo, null, operatorId);
                }

                //写订单事务日志
                var user = ISyUserDao.Instance.GetSyUser(new List<int> { operatorId }).SingleOrDefault();

                if (user != null)
                    WriteSoTransactionLog(soOrder.TransactionSysNo,
                                          string.Format(Constant.ORDER_TRANSACTIONLOG_AUDIT, soOrder.SysNo),
                                          user.UserName);
                return true;
            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// 订单对应锁定库存释放操作
        /// </summary>
        /// <param name="orderSysNo">订单sysNo</param>
        /// <param name="operatorId">操作人</param>
        /// <param name="operatorType">操作人类型</param>
        /// <param name="message">返回消息</param>
        /// <param name="reason">订单库存回滚原因</param>
        /// <returns>true成功 false失败</returns>
        /// <remarks>
        /// 2017-09-09 罗勤尧 超时未支付释放锁定库存
        /// </remarks>
        public bool CancelSoOrderStock(int orderSysNo, int operatorId, OrderStatus.销售单锁定库存释放类型 operatorType, ref string message, string reason = null)
        {
            /* 
             * 1.判断soOrderItem中的RealStockOutQuantity（实际出库数量）全部为0才给予作废
             * 2.写操作时间、写操作人、写状态值
             * 3.写日志
             * 6.还原锁定库存
             */
            var soOrder = GetEntity(orderSysNo);
            if (soOrder.Status != (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.待审核)
            {
                message = "非待审核的订单不能够回滚";
                return false;
            }

            try
            {
                var orderItems =
                    ISoOrderItemDao.Instance.GetOrderItemsByOrderSysNo(orderSysNo).ToList();
                if (orderItems.Exists(o => o.RealStockOutQuantity > 0))
                {
                    message = "该订单存在已经出库的商品不能回滚";
                    return false;
                }


                //回滚库存
                if ((DateTime.Now-soOrder.CreateDate).Days>=2&& soOrder.PayStatus == (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单支付状态.未支付 && BLL.Config.Config.Instance.GetGeneralConfig().LockInventoryRollback == 1)
                {
                    var _orderItems = BLL.Order.SoOrderBo.Instance.GetCBOrderItemsByOrderId(orderSysNo);

                    ///
                    List<WhWarehouseChangeLog> logList = WhWarehouseChangeLogBo.Instance.WarehouseLogList("订单超时未支付入库编号：" + soOrder.OrderNo);

                    foreach (var item in _orderItems)
                    {
                        Hyt.BLL.Warehouse.PdProductStockBo.Instance.RollbackLockStockQuantity(soOrder.DefaultWarehouseSysNo, item.ProductSysNo, Math.Abs(item.Quantity));
                        /// 添加记录，取消订单记录
                        WhWarehouseChangeLog log = logList.Find(q => q.ProSysNo == item.ProductSysNo);
                        if (log == null)
                        {
                            WhWarehouseChangeLog log1 = new WhWarehouseChangeLog()
                            {
                                WareSysNo = soOrder.DefaultWarehouseSysNo,
                                ProSysNo = item.ProductSysNo,
                                ChageDate = soOrder.LastUpdateDate,
                                CreateDate = DateTime.Now,
                                ChangeQuantity = Convert.ToInt32(item.Quantity),
                                BusinessTypes = "订单锁定的库存释放",
                                LogData = "订单锁定的库存释放编号：" + soOrder.OrderNo
                            };
                            WhWarehouseChangeLogBo.Instance.CreateMod(log1);
                        }


                    }
                }

                //写订单事务日志
                SyUser user = null;
                var operatorName = string.Empty;
                if (operatorId > 0)
                {
                    if (operatorType == OrderStatus.销售单锁定库存释放类型.后台用户)
                    {
                        user = ISyUserDao.Instance.GetSyUser(new List<int> { operatorId }).SingleOrDefault();
                        operatorName = user.UserName;
                    }
                    else
                    {
                        var customer = CrCustomerBo.Instance.GetModel(operatorId);
                        operatorName = customer.Name;
                    }
                }
                if (user == null)
                {
                    user = new SyUser { SysNo = 0, UserName = "[系统]" };
                }

               
               
                SyJobDispatcherBo.Instance.WriteJobLog(string.Format("订单到期未支付锁定库存释放任务，销售单编号:{0}", soOrder.SysNo),
                    soOrder.SysNo, null, operatorId);

                var logContent = Constant.ORDER_TRANSACTIONLOG_CANCEL;
                if (!string.IsNullOrWhiteSpace(reason))
                {
                    logContent += "。原因：" + reason;
                }
                WriteSoTransactionLog(soOrder.TransactionSysNo, logContent, operatorName);

             
                return true;
            }
            catch (HytException ex)
            {
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "订单锁定库存释放错误", LogStatus.系统日志目标类型.订单, orderSysNo, ex, operatorId);
                return false;
            }


        }

        /// <summary>
        /// 订单作废操作
        /// </summary>
        /// <param name="orderSysNo">订单sysNo</param>
        /// <param name="operatorId">操作人</param>
        /// <param name="operatorType">操作人类型</param>
        /// <param name="message">返回消息</param>
        /// <param name="reason">订单作废原因</param>
        /// <returns>true成功 false失败</returns>
        /// <remarks>
        /// 2013-06-17 朱家宏 创建
        /// 2014-01-03 朱家宏 增加作废订单后返回惠源币功能，增加operatorType参数区别作废人类型
        /// 2014-12-15 杨浩 添加支付时减库存时回滚库存
        /// </remarks>
        public bool CancelSoOrder(int orderSysNo, int operatorId, OrderStatus.销售单作废人类型 operatorType, ref string message, string reason = null)
        {
            /* 
             * 1.判断soOrderItem中的RealStockOutQuantity（实际出库数量）全部为0才给予作废
             * 2.写操作时间、写操作人、写状态值
             * 3.写作废日志
             * 4.已支付退款
             * 5.返还会员币
             * 6.判断是否支付时减库存则需要还原库存
             */
            var soOrder = GetEntity(orderSysNo);
            if (soOrder.Status != (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.待审核)
            {
                message = "非待审核的订单不能够作废";
                return false;
            }

            try
            {
                var orderItems =
                    ISoOrderItemDao.Instance.GetOrderItemsByOrderSysNo(orderSysNo).ToList();
                if (orderItems.Exists(o => o.RealStockOutQuantity > 0))
                {
                    message = "该订单存在已经出库的商品不能作废";
                    return false;
                }

                #region 返还分销商预付款 2013-09-13 朱成果

                if (soOrder.PayStatus == (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单支付状态.已支付 &&
                    soOrder.OrderSource == (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单来源.分销商升舱)
                {
                    Hyt.BLL.MallSeller.DsOrderBo.Instance.CancelOrder(orderSysNo, operatorId, soOrder.CashPay > 0, soOrder.DealerSysNo);
                }

                #endregion

                soOrder.Status = (int)OrderStatus.销售单状态.作废;
                soOrder.OnlineStatus = Constant.OlineStatusType.作废;
                soOrder.CancelUserSysNo = operatorId;
                soOrder.CancelDate = DateTime.Now;
                soOrder.CancelUserType = (int)operatorType;
                soOrder.LastUpdateBy = operatorId;
                soOrder.LastUpdateDate = DateTime.Now;

                var r = UpdateOrder(soOrder); //更新订单 余勇 修改为调用业务层方法ISoOrderDao.Instance.Update(soOrder);

                if (!r) return false;

                //回滚库存
                if (soOrder.PayStatus == (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单支付状态.已支付 && BLL.Config.Config.Instance.GetGeneralConfig().ReducedInventory == 1)
                {
                    var _orderItems = BLL.Order.SoOrderBo.Instance.GetCBOrderItemsByOrderId(orderSysNo);

                    ///
                    List<WhWarehouseChangeLog> logList = WhWarehouseChangeLogBo.Instance.WarehouseLogList("订单作废入库编号：" + soOrder.OrderNo);

                    foreach (var item in _orderItems)
                    {
                        Hyt.BLL.Warehouse.PdProductStockBo.Instance.UpdateStockQuantity(soOrder.DefaultWarehouseSysNo, item.ProductSysNo, -Math.Abs(item.Quantity));
                        /// 添加记录，取消订单记录
                        WhWarehouseChangeLog log = logList.Find(q => q.ProSysNo == item.ProductSysNo);
                        if (log == null)
                        {
                            WhWarehouseChangeLog log1 = new WhWarehouseChangeLog()
                            {
                                WareSysNo = soOrder.DefaultWarehouseSysNo,
                                ProSysNo = item.ProductSysNo,
                                ChageDate = soOrder.LastUpdateDate,
                                CreateDate = DateTime.Now,
                                ChangeQuantity = Convert.ToInt32(item.Quantity),
                                BusinessTypes = "订单作废入库",
                                LogData = "订单作废入库编号：" + soOrder.OrderNo
                            };
                            WhWarehouseChangeLogBo.Instance.CreateMod(log1);
                        }


                    }
                }

                //写订单事务日志
                SyUser user = null;
                var operatorName = string.Empty;
                if (operatorId > 0)
                {
                    if (operatorType == OrderStatus.销售单作废人类型.后台用户)
                    {
                        user = ISyUserDao.Instance.GetSyUser(new List<int> { operatorId }).SingleOrDefault();
                        operatorName = user.UserName;
                    }
                    else
                    {
                        var customer = CrCustomerBo.Instance.GetModel(operatorId);
                        operatorName = customer.Name;
                    }
                }
                if (user == null)
                {
                    user = new SyUser { SysNo = 0, UserName = "[系统]" };
                }

                Hyt.BLL.Finance.FnReceiptVoucherBo.Instance.CancelOrderReceipt(soOrder, user,
                    (soOrder.OrderSource == (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单来源.分销商升舱));
                //作废收款单(朱成果 2013-07-08）

                //将已处理的订单在任务池删除
                SyJobPoolManageBo.Instance.DeleteJobPool(soOrder.SysNo, (int)SystemStatus.任务对象类型.客服订单审核);
                SyJobDispatcherBo.Instance.WriteJobLog(string.Format("订单作废取消订单审核任务，销售单编号:{0}", soOrder.SysNo),
                    soOrder.SysNo, null, operatorId);

                var logContent = Constant.ORDER_TRANSACTIONLOG_CANCEL;
                if (!string.IsNullOrWhiteSpace(reason))
                {
                    logContent += "。原因：" + reason;
                }
                WriteSoTransactionLog(soOrder.TransactionSysNo, logContent, operatorName);

                //返还会员币
                //if (soOrder.CoinPay > 0)
                //{
                //    PointBo.Instance.CancelOrderIncreaseExperienceCoin(soOrder.CustomerSysNo,
                //        user.SysNo,
                //        soOrder.SysNo,
                //        (int)soOrder.CoinPay,
                //        soOrder.TransactionSysNo);
                //}
                if (soOrder.CoinPay > 0)
                {
                    PointBo.Instance.ReturnAvailablePoint(soOrder.CustomerSysNo, soOrder.SysNo, Hyt.BLL.LevelPoint.PointBo.Instance.CoinToPoint((int)soOrder.CoinPay), soOrder.TransactionSysNo);
                }

                return true;
            }
            catch (HytException ex)
            {
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "订单作废错误", LogStatus.系统日志目标类型.订单, orderSysNo, ex, operatorId);
                return false;
            }


        }

        /// <summary>
        /// 取消审核订单
        /// </summary>
        /// <param name="orderSysNo">销售单号</param>
        /// <param name="operatorId">操作人</param>
        /// <returns>true成功 false失败</returns>
        /// <remarks>2013-07-03 朱家宏 修改</remarks>
        public bool CancelAuditedOrder(int orderSysNo, int operatorId)
        {
            /* 
             * 订单为待出库或待支付状态才可做取消审核操作
             * 订单状态改为待审核，写日志
             */
            var r = false;
            var order = SoOrderBo.Instance.GetEntity(orderSysNo);
            if (order.Status == (int)OrderStatus.销售单状态.待创建出库单 ||
                order.Status == (int)OrderStatus.销售单状态.待支付)
            {

                order.Status = (int)OrderStatus.销售单状态.待审核;
                order.OnlineStatus = Constant.OlineStatusType.待审核;

                r = UpdateOrder(order); //更新订单 余勇 修改为调用业务层方法ISoOrderDao.Instance.Update(order);

                //写订单日志
                var user = ISyUserDao.Instance.GetSyUser(new List<int> { operatorId }).SingleOrDefault();

                string customerName = order.Customer != null ? order.Customer.Name : "";
                string mobilePhoneNumber = order.Customer != null ? order.Customer.MobilePhoneNumber : "";
                //在任务池添加订单审核任务,同时在任务池删除出库任务
                SyJobPoolPublishBo.Instance.OrderAuditBySysNo(order.SysNo, operatorId, (int)SystemStatus.任务对象类型.客服订单提交出库);
                SyJobDispatcherBo.Instance.WriteJobLog(string.Format("取消出库审核，创建订单审核任务，销售单编号:{0}",
                    order.SysNo), order.SysNo, null, operatorId);

                if (user != null)
                    WriteSoTransactionLog(order.TransactionSysNo,
                                          string.Format(Constant.ORDER_TRANSACTIONLOG_AUDIT_CANCEL, order.SysNo),
                                          user.UserName);


            }
            return r;
        }

        /// <summary>
        /// 取消作废订单
        /// </summary>
        /// <param name="orderSysNo">订单号</param>
        /// <param name="operatorId">操作人</param>
        /// <returns>t:成功 f:失败</returns>
        /// <remarks>2013-07-03 朱家宏 创建</remarks>
        private bool CancelObsoleteSoOrder(int orderSysNo, int operatorId)
        {
            /*
             * TODO:暂不实现
             */
            return false;
        }

        #endregion

        #region 订单事务日志
        /// <summary>
        /// 写订单事务日志
        /// </summary>
        /// <param name="transactionSysNo">事务编码</param>
        /// <param name="content">内容</param>
        /// <param name="oper">操作人</param>
        /// <param name="isDisplayToCustomer">是否展示给客户看</param>
        /// <returns>void</returns>
        /// <remarks>2013-06-19 朱家宏 修改</remarks>
        public void WriteSoTransactionLog(string transactionSysNo, string content, string oper, int isDisplayToCustomer = 1)
        {
            var log = new SoTransactionLog
            {
                LogContent = content,
                TransactionSysNo = transactionSysNo,
                OperateDate = DateTime.Now,
                Operator = oper,
                //IsDisplayToCustomer = isDisplayToCustomer
            };
            ISoTransactionLogDao.Instance.CreateSoTransactionLog(log);
        }

        /// <summary>
        /// 写订单事务日志
        /// </summary>
        /// <param name="transactionSysNo">事务编码</param>
        /// <param name="content">内容</param>
        /// <param name="oper">操作人</param>
        /// <returns>void</returns>
        /// <remarks>2013-06-19 朱家宏 修改</remarks>
        public void WriteSoTransactionLog(string transactionSysNo, string content, string oper)
        {
            var log = new SoTransactionLog
                {
                    LogContent = content,
                    TransactionSysNo = transactionSysNo,
                    OperateDate = DateTime.Now,
                    Operator = oper
                };
            ISoTransactionLogDao.Instance.CreateSoTransactionLog(log);
        }

        /// <summary>
        /// 获取订单日志分页数据
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="pager"></param>
        /// <remarks>2013-06-20 朱成果 创建</remarks>
        public PagedList<SoTransactionLog> GetTransactionLogPageData(int orderID, int PageIndex, int PageSize)
        {
            Pager<SoTransactionLog> pager = new Pager<SoTransactionLog>();
            pager.CurrentPage = PageIndex;
            pager.PageSize = PageSize;
            ISoTransactionLogDao.Instance.GetPageDataByOrderID(orderID, ref pager);
            PagedList<SoTransactionLog> list = new PagedList<SoTransactionLog>();
            list.TData = pager.Rows;
            list.CurrentPageIndex = pager.CurrentPage;
            list.PageSize = PageSize;
            list.TotalItemCount = pager.TotalRows;
            return list;
        }

        #endregion

        #region 订单出库


        /// <summary>
        /// 订单分配出库(当前处理不使用事物提交，事物请包裹在外面)
        /// </summary>
        /// <param name="datas">出库商品列表:Model.Quantity 为出库数量</param>
        /// <param name="warehouseSysNo">仓库编号</param>
        /// <param name="user">操作人</param>
        /// <param name="isSendMessage">是否发送短信（true为发送）</param>
        ///  <param name="outstockdeliveryTypeSysNo">出库单配送方式 为空获取订单的配送方式</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-06-24 杨浩 创建
        /// 2013-12-19 杨浩 修改订单日志
        /// </remarks>
        public WhStockOut CreateOutStock(IList<Model.SoOrderItem> datas, int warehouseSysNo, SyUser user, bool isSendMessage, int? outstockdeliveryTypeSysNo = null)
        {
            if (datas == null || datas.Count() < 1) return null;
            if (warehouseSysNo < 1) throw new ArgumentNullException("必需选择一个仓库");
            var so = Hyt.BLL.Order.SoOrderBo.Instance.GetEntity(datas[0].OrderSysNo);
            if (
                !(so.Status == (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.待创建出库单 ||
                  so.Status == (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.部分创建出库单))
            {
                throw new Exception("当前状态下的订单不允许出库。");
            }
            var soItems = SoOrderBo.Instance.GetOrderItemsByOrderId(so.SysNo);
            var payType = PaymentTypeBo.Instance.GetEntity(so.PayTypeSysNo);
            int currectDeliveryTypeSysNo = so.DeliveryTypeSysNo;
            if (outstockdeliveryTypeSysNo.HasValue && outstockdeliveryTypeSysNo.Value > 0)
            {
                //是否选择了出库单配送方式
                currectDeliveryTypeSysNo = outstockdeliveryTypeSysNo.Value;
            }
            if (currectDeliveryTypeSysNo == Hyt.Model.SystemPredefined.DeliveryType.第三方快递 && so.PayStatus != Hyt.Model.WorkflowStatus.OrderStatus.销售单支付状态.已支付.GetHashCode())
            {
                throw new Exception("第三方快递配送，必须先完成订单付款。");
            }
            WhStockOut whStockOut = new WhStockOut()
            {
                ContactBeforeDelivery = so.ContactBeforeDelivery,
                CreatedBy = user.SysNo,
                CreatedDate = DateTime.Now,
                ReceiveAddressSysNo = so.ReceiveAddressSysNo,
                CustomerMessage = so.CustomerMessage,
                DeliveryRemarks = so.DeliveryRemarks,
                DeliveryTime = so.DeliveryTime,
                DeliveryTypeSysNo = currectDeliveryTypeSysNo,
                IsCOD = payType.PaymentType == (int)Hyt.Model.WorkflowStatus.BasicStatus.支付方式类型.到付 ? 1 : 0,
                IsPrintedPackageCover = 0,
                IsPrintedPickupCover = 0,
                LastUpdateBy = user.SysNo,
                LastUpdateDate = DateTime.Now,
                OrderSysNO = so.SysNo,
                Receivable =
                    payType.PaymentType == (int)Hyt.Model.WorkflowStatus.BasicStatus.支付方式类型.到付
                        ? so.CashPay
                        : 0m,
                Remarks = so.Remarks,
                Status = (int)Hyt.Model.WorkflowStatus.WarehouseStatus.出库单状态.待出库,
                TransactionSysNo = so.TransactionSysNo,
                WarehouseSysNo = warehouseSysNo,
                //InvoiceSysNo = so.InvoiceSysNo
            };
            int otherSysNo;
            bool existNeedPaid = Hyt.BLL.Order.ShopOrderBo.Instance.GetUnPaidStockOutNo(so.SysNo, out otherSysNo);//存在需要支付的出库单
            if (so.PayStatus == (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单支付状态.已支付)
            {
                //已支付
                whStockOut.Receivable = 0;
            }
            else if ((so.DeliveryTypeSysNo == Hyt.Model.SystemPredefined.DeliveryType.门店自提 || so.DeliveryTypeSysNo == Hyt.Model.SystemPredefined.DeliveryType.自提) && existNeedPaid)
            {
                //门店自提已创建有收款的出库单，收款金额为0
                //未付款 不处理,全部收款金额为订单金额
                whStockOut.Receivable = 0;
            }
            if (so.Status == (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.待创建出库单)
            {
                whStockOut.InvoiceSysNo = so.InvoiceSysNo;//发票在第一张出库单上
            }
            var outStockItemAmount = CalculateOutStockItemAmount(so, soItems, datas);
            whStockOut.StockOutAmount = outStockItemAmount.Sum(m => m.Value);
            //note:调用保存出库单主表的方法
            whStockOut.SysNo = Hyt.DataAccess.Warehouse.IOutStockDao.Instance.InsertMain(whStockOut); //朱成果 添加
            //记录出库单操作日志 杨浩 2014-12-22 添加
            WhStockOutLogBo.Instance.WriteLog(whStockOut.SysNo, "订单分配出库,在出库单主表添加一条记录", user);
            foreach (var data in datas)
            {
                var whStockOutItem = new WhStockOutItem()
                {
                    CreatedBy = user.SysNo,
                    CreatedDate = DateTime.Now,
                    LastUpdateBy = user.SysNo,
                    LastUpdateDate = DateTime.Now,
                    Measurement = "",
                    OrderSysNo = so.SysNo,
                    OriginalPrice = data.OriginalPrice,
                    ProductName = data.ProductName,
                    ProductQuantity = data.Quantity,
                    //2013-11-22 吴文强 分摊后的实际销售金额
                    RealSalesAmount = outStockItemAmount[data.SysNo],
                    ProductSysNo = data.ProductSysNo,
                    Status = 1,
                    StockOutSysNo = whStockOut.SysNo,
                    TransactionSysNo = so.TransactionSysNo,
                    Weight = 0m,
                    OrderItemSysNo = data.SysNo
                };

                //调用保存出库单明细表的方法
                Hyt.DataAccess.Warehouse.IOutStockDao.Instance.InsertItem(whStockOutItem); //朱成果 添加
                var soItem = soItems.First(p => p.SysNo == data.SysNo);
                //更新当前出库明细中的出库数量
                soItem.RealStockOutQuantity += data.Quantity;
                //出库数量到数据库
                Hyt.DataAccess.Order.ISoOrderItemDao.Instance.UpdateOutStockQuantity(soItem.SysNo,
                                                                                     soItem.RealStockOutQuantity);
                // 朱成果 更新出库数量
            }
            //记录出库单操作日志 缪竞华 2014-12-22 添加 
            WhStockOutLogBo.Instance.WriteLog(whStockOut.SysNo, "订单分配出库,在出库单明细表添加记录", user);
            //更新销售单主表
            so.Status = (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.已创建出库单;
            so.OnlineStatus = Constant.OlineStatusType.待出库;
            foreach (var soItem in soItems)
            {
                if (soItem.RealStockOutQuantity > soItem.Quantity) throw new Exception("异常：实际出库数量大于订购数量");
                if (soItem.RealStockOutQuantity < soItem.Quantity)
                {
                    so.Status = (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.部分创建出库单;
                    so.OnlineStatus = Constant.OlineStatusType.待出库;
                }
            }
            //调用更新销售单主表方法
            so.DefaultWarehouseSysNo = warehouseSysNo;//修改仓库
            so.DeliveryTypeSysNo = currectDeliveryTypeSysNo;//修改配送方式
            UpdateOrder(so); //更新订单 余勇 修改为调用业务层方法 //Hyt.DataAccess.Order.ISoOrderDao.Instance.Update(so); //更新订单状态，默认出库仓库

            // Hyt.DataAccess.Order.ISoOrderDao.Instance.UpdateOrderStatus(so.SysNo, so.Status); //更新订单出库状态
            if (so.Status == (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.已创建出库单)
            {
                //将已处理的订单在任务池删除
                SyJobPoolManageBo.Instance.DeleteJobPool(so.SysNo, (int)SystemStatus.任务对象类型.客服订单提交出库);
                SyJobDispatcherBo.Instance.WriteJobLog(string.Format("已创建出库单,审核任务完成，销售单编号:{0}", so.SysNo), so.SysNo, null, user.SysNo);
            }
            var warehouseName = WhWarehouseBo.Instance.GetWarehouseEntity(warehouseSysNo).WarehouseName;
            WriteSoTransactionLog(so.TransactionSysNo
                                  , string.Format(Constant.ORDER_TRANSACTIONLOG_OUTSTOCK_CREATE, warehouseName, whStockOut.SysNo)
                                  , user.UserName);
            #region 发送订单分配到仓库短信提醒

            //if (isSendMessage)
            //{
            //    var config = WhWarehouseConfigBo.Instance.GetEntityByWarehouseSysNo(warehouseSysNo);
            //    if (config != null && config.IsReceiveMessage == (int)Hyt.Model.WorkflowStatus.WarehouseStatus.是否接收分配出库短信.是 && !string.IsNullOrEmpty(config.ReceiveMobileNo))
            //    {
            //        Hyt.BLL.Extras.SmsBO.Instance.发生订单分配仓库提醒短信(config.ReceiveMobileNo, String.Empty, whStockOut.SysNo.ToString());
            //    }
            //}
            #endregion

            //Hyt.BLL.QiMen.QiMenBo.Instance.NoticeOutStock(whStockOut, user);//通知奇门

            return whStockOut;
        }



        /// <summary>
        /// 订单分配出库(当前处理不使用事物提交，事物请包裹在外面)
        /// </summary>
        /// <param name="datas">出库商品列表:Model.Quantity 为出库数量</param>
        /// <param name="warehouseSysNo">仓库编号</param>
        /// <param name="user">操作人</param>
        ///  <param name="outstockdeliveryTypeSysNo">出库单配送方式 为空获取订单的配送方式</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-06-24 杨文兵 创建
        /// 2013-06-25 朱成果 修改
        /// 2013-12-19 黄志勇 修改订单日志
        /// </remarks>
        public WhStockOut CreateOutStock(IList<Model.SoOrderItem> datas, int warehouseSysNo, SyUser user, int? outstockdeliveryTypeSysNo = null)
        {
            if (datas == null || datas.Count() < 1) return null;
            if (warehouseSysNo < 1) throw new ArgumentNullException("必需选择一个仓库");
            var so = Hyt.BLL.Order.SoOrderBo.Instance.GetEntity(datas[0].OrderSysNo);
            if (
                !(so.Status == (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.待创建出库单 ||
                  so.Status == (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.部分创建出库单))
            {
                throw new Exception("当前状态下的订单不允许出库。");
            }
            var soItems = SoOrderBo.Instance.GetOrderItemsByOrderId(so.SysNo);
            var payType = PaymentTypeBo.Instance.GetEntity(so.PayTypeSysNo);
            int currectDeliveryTypeSysNo = so.DeliveryTypeSysNo;
            if (outstockdeliveryTypeSysNo.HasValue && outstockdeliveryTypeSysNo.Value > 0)
            {
                //是否选择了出库单配送方式
                currectDeliveryTypeSysNo = outstockdeliveryTypeSysNo.Value;
            }
            if (currectDeliveryTypeSysNo == Hyt.Model.SystemPredefined.DeliveryType.第三方快递 && so.PayStatus != Hyt.Model.WorkflowStatus.OrderStatus.销售单支付状态.已支付.GetHashCode())
            {
                throw new Exception("第三方快递配送，必须先完成订单付款。");
            }
            WhStockOut whStockOut = new WhStockOut()
            {
                ContactBeforeDelivery = so.ContactBeforeDelivery,
                CreatedBy = user.SysNo,
                CreatedDate = DateTime.Now,
                ReceiveAddressSysNo = so.ReceiveAddressSysNo,
                CustomerMessage = so.CustomerMessage,
                DeliveryRemarks = so.DeliveryRemarks,
                DeliveryTime = so.DeliveryTime,
                DeliveryTypeSysNo = currectDeliveryTypeSysNo,
                IsCOD =payType==null?1: payType.PaymentType == (int)Hyt.Model.WorkflowStatus.BasicStatus.支付方式类型.到付 ? 1 : 0,
                IsPrintedPackageCover = 0,
                IsPrintedPickupCover = 0,
                LastUpdateBy = user.SysNo,
                LastUpdateDate = DateTime.Now,
                OrderSysNO = so.SysNo,
                Receivable =
                   payType == null ? so.CashPay : payType.PaymentType == (int)Hyt.Model.WorkflowStatus.BasicStatus.支付方式类型.到付
                        ? so.CashPay
                        : 0m,
                Remarks = so.Remarks,
                Status = (int)Hyt.Model.WorkflowStatus.WarehouseStatus.出库单状态.待出库,
                TransactionSysNo = so.TransactionSysNo,
                WarehouseSysNo = warehouseSysNo,
                //InvoiceSysNo = so.InvoiceSysNo
            };
            int otherSysNo;
            bool existNeedPaid = Hyt.BLL.Order.ShopOrderBo.Instance.GetUnPaidStockOutNo(so.SysNo, out otherSysNo);//存在需要支付的出库单
            if (so.PayStatus == (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单支付状态.已支付)
            {
                //已支付
                whStockOut.Receivable = 0;
            }
            else if ((so.DeliveryTypeSysNo == Hyt.Model.SystemPredefined.DeliveryType.门店自提 || so.DeliveryTypeSysNo == Hyt.Model.SystemPredefined.DeliveryType.自提) && existNeedPaid)
            {
                //门店自提已创建有收款的出库单，收款金额为0
                //未付款 自建物流不处理,全部收款金额为订单金额
                whStockOut.Receivable = 0;
            }
            if (so.Status == (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.待创建出库单)
            {
                whStockOut.InvoiceSysNo = so.InvoiceSysNo;//发票在第一张出库单上
            }
            var outStockItemAmount = CalculateOutStockItemAmount(so, soItems, datas);
            whStockOut.StockOutAmount = outStockItemAmount.Sum(m => m.Value);
            //note:调用保存出库单主表的方法
            whStockOut.SysNo = Hyt.DataAccess.Warehouse.IOutStockDao.Instance.InsertMain(whStockOut); //朱成果 添加
            foreach (var data in datas)
            {
                var whStockOutItem = new WhStockOutItem()
                    {
                        CreatedBy = user.SysNo,
                        CreatedDate = DateTime.Now,
                        LastUpdateBy = user.SysNo,
                        LastUpdateDate = DateTime.Now,
                        Measurement = "",
                        OrderSysNo = so.SysNo,
                        OriginalPrice = data.OriginalPrice,
                        ProductName = data.ProductName,
                        ProductQuantity = data.Quantity,
                        //分摊后的实际销售金额
                        RealSalesAmount = outStockItemAmount[data.SysNo],
                        ProductSysNo = data.ProductSysNo,
                        Status = 1,
                        StockOutSysNo = whStockOut.SysNo,
                        TransactionSysNo = so.TransactionSysNo,
                        Weight = 0m,
                        OrderItemSysNo = data.SysNo
                    };

                //调用保存出库单明细表的方法
                whStockOutItem.SysNo = Hyt.DataAccess.Warehouse.IOutStockDao.Instance.InsertItem(whStockOutItem); //朱成果 添加
                var soItem = soItems.First(p => p.SysNo == data.SysNo);
                //更新当前出库明细中的出库数量
                soItem.RealStockOutQuantity += data.Quantity;
                //出库数量到数据库
                Hyt.DataAccess.Order.ISoOrderItemDao.Instance.UpdateOutStockQuantity(soItem.SysNo,
                                                                                     soItem.RealStockOutQuantity);
                // 朱成果 更新出库数量
                ///添加出库单明细出库实体中 2016-04-06 杨云奕 添加
                if (whStockOut.Items == null)
                    whStockOut.Items = new List<WhStockOutItem>();
                whStockOut.Items.Add(whStockOutItem);
            }
            //更新销售单主表
            so.Status = (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.已创建出库单;
            so.OnlineStatus = Constant.OlineStatusType.待出库;
            foreach (var soItem in soItems)
            {
                if (soItem.RealStockOutQuantity > soItem.Quantity) throw new Exception("异常：实际出库数量大于订购数量");
                if (soItem.RealStockOutQuantity < soItem.Quantity)
                {
                    so.Status = (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.部分创建出库单;
                    so.OnlineStatus = Constant.OlineStatusType.待出库;
                }
            }
            //调用更新销售单主表方法
            so.DefaultWarehouseSysNo = warehouseSysNo;//修改仓库
            so.DeliveryTypeSysNo = currectDeliveryTypeSysNo;//修改配送方式
            UpdateOrder(so); //更新订单 余勇 修改为调用业务层方法 //Hyt.DataAccess.Order.ISoOrderDao.Instance.Update(so); //更新订单状态，默认出库仓库

            // Hyt.DataAccess.Order.ISoOrderDao.Instance.UpdateOrderStatus(so.SysNo, so.Status); //更新订单出库状态
            if (so.Status == (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.已创建出库单)
            {
                //将已处理的订单在任务池删除
                SyJobPoolManageBo.Instance.DeleteJobPool(so.SysNo, (int)SystemStatus.任务对象类型.客服订单提交出库);
                SyJobDispatcherBo.Instance.WriteJobLog(string.Format("已创建出库单,审核任务完成，销售单编号:{0}", so.SysNo), so.SysNo, null, user.SysNo);
            }
            var warehouseName = WhWarehouseBo.Instance.GetWarehouseEntity(warehouseSysNo).WarehouseName;
            WriteSoTransactionLog(so.TransactionSysNo
                                  , string.Format(Constant.ORDER_TRANSACTIONLOG_OUTSTOCK_CREATE, warehouseName, whStockOut.SysNo)
                                  , user.UserName);

            return whStockOut;
        }

        /// <summary>
        /// 计算出库单实际销售金额
        /// </summary>
        /// <param name="order">订单</param>
        /// <param name="orderItems">销售单明细</param>
        /// <param name="outStockOrderItems">创建出库单的销售单明细</param>
        /// <returns>出库单明细实际销售金额</returns>
        /// <remarks>2013-11-22 杨浩 创建</remarks>
        public Dictionary<int, decimal> CalculateOutStockItemAmount(SoOrder order, IList<SoOrderItem> orderItems,
                                                               IList<SoOrderItem> outStockOrderItems)
        {
            //需要分摊的商品总数（不包含赠品和销售金额为0的商品）
            var totalApportionNum = 0;
            var allStockOutQuantity = 0;
            //订单赠品价格合计
            decimal orderGiftAmount = 0;
            //订单销售明细价格合计
            decimal orderSalesAmount = 0;



            orderGiftAmount = orderItems.Where(t => t.ProductSalesType == (int)CustomerStatus.商品销售类型.赠品)
                                        .Sum(t => t.SalesAmount - t.DiscountAmount + t.ChangeAmount);

            orderSalesAmount = order.CashPay;// order.GetFreight().RealFreightAmount - order.TaxFee;
            //减去运费后，订单金额为负数时，订单金额作为0做分摊计算。
            orderSalesAmount = orderSalesAmount < 0 ? 0 : orderSalesAmount;

            foreach (var orderItem in orderItems)
            {
                if ((orderItem.ProductSalesType != (int)CustomerStatus.商品销售类型.赠品
                    || orderGiftAmount > orderSalesAmount - orderGiftAmount)
                    && (orderItem.SalesAmount - orderItem.DiscountAmount + orderItem.ChangeAmount) > 0)
                {
                    totalApportionNum += orderItem.Quantity;
                }
            }

            var outStockAmount = new Dictionary<int, decimal>();

            //获取当前销售单明细编号已创建分配出库的出库单明细
            var allStockOutItems =
                WhWarehouseBo.Instance.GetStockOutItems(outStockOrderItems.Select(soi => soi.SysNo).ToArray());

            var newOrderGiftAmount = orderGiftAmount > orderSalesAmount - orderGiftAmount ? 0 : orderGiftAmount;

            //明细销售总金额-总明细总折扣金额+明细总调价金额
            var itemTotalSalesAmount = orderItems.Sum(t => t.SalesAmount - t.DiscountAmount + t.ChangeAmount) - newOrderGiftAmount;

            //需分摊金额(明细总销售金额-(订单现金支付金额-运费))
            var apportionAmount = itemTotalSalesAmount - (orderSalesAmount - newOrderGiftAmount);

         

            //已创建出库单数
            var stockOutItems = WhWarehouseBo.Instance.GetStockOutItems(orderItems.Select(oi => oi.SysNo).ToArray());

            //已出库非赠品金额合计
            decimal stockNotGiftAmount = 0;

            //计算已出库的商品数量（不包含赠品和销售金额为0的商品）
            foreach (var stockOutItem in stockOutItems)
            {
                var orderItem = orderItems.First(oi => oi.SysNo == stockOutItem.OrderItemSysNo);

                if ((orderItem.ProductSalesType != (int)CustomerStatus.商品销售类型.赠品
                    || orderGiftAmount > orderSalesAmount - orderGiftAmount)
                   && (orderItem.SalesAmount - orderItem.DiscountAmount + orderItem.ChangeAmount) > 0)
                {
                    allStockOutQuantity += stockOutItem.ProductQuantity;
                    stockNotGiftAmount += stockOutItem.RealSalesAmount;
                }
            }

            //当前非赠品金额合计
            decimal currNotGiftAmount = 0;

            //循环出库商品
            foreach (var outStockOrderItem in outStockOrderItems)
            {
                decimal currItemAmount = 0;
                var orderItem = orderItems.First(oi => oi.SysNo == outStockOrderItem.SysNo);

                if ((orderItem.ProductSalesType != (int)CustomerStatus.商品销售类型.赠品
                    || orderGiftAmount > orderSalesAmount - orderGiftAmount)
                    && (orderItem.SalesAmount - orderItem.DiscountAmount + orderItem.ChangeAmount) > 0)
                {
                    allStockOutQuantity += outStockOrderItem.Quantity;
                }

                //最后一个商品出库时：订单商品数量==已出库商品数
                //出库金额=订单支付金额-所有已创建出库金额
                if (totalApportionNum == allStockOutQuantity &&
                    (orderItem.ProductSalesType != (int)CustomerStatus.商品销售类型.赠品
                    || orderGiftAmount > orderSalesAmount - orderGiftAmount)
                    && (orderItem.SalesAmount - orderItem.DiscountAmount + orderItem.ChangeAmount) > 0)
                {
                    outStockAmount.Add(outStockOrderItem.SysNo,
                                       decimal.Round(
                                          orderSalesAmount
                                          - stockNotGiftAmount
                                          - currNotGiftAmount
                                          - newOrderGiftAmount
                                          , 2));
                    continue;
                }

                var itemSalesAmount = orderItem.SalesAmount - orderItem.DiscountAmount + orderItem.ChangeAmount;

                //当前明细需分摊金额
                decimal itemRealSalesAmount = 0;
                if (itemTotalSalesAmount != 0 &&
                    (orderItem.ProductSalesType != (int)CustomerStatus.商品销售类型.赠品
                    || orderGiftAmount > orderSalesAmount - orderGiftAmount)
                    && (orderItem.SalesAmount - orderItem.DiscountAmount + orderItem.ChangeAmount) > 0)
                {
                    itemRealSalesAmount = itemTotalSalesAmount == apportionAmount
                                              ? 0
                                              : (itemSalesAmount -
                                                itemSalesAmount / itemTotalSalesAmount * apportionAmount).RoundToShe(2);
                }
                else
                {
                    itemRealSalesAmount = itemSalesAmount;
                }

                if (orderItem.Quantity == orderItem.RealStockOutQuantity + outStockOrderItem.RealStockOutQuantity)
                {
                    //当前销售单已全出库时：计算剩余明细金额
                    var stockOutAmount =
                        allStockOutItems.Where(soi => soi.OrderItemSysNo == orderItem.SysNo)
                                        .Sum(soi => soi.RealSalesAmount);
                    //查询出库单当已出库金额
                    currItemAmount = itemRealSalesAmount - stockOutAmount;
                }
                else
                {
                    //当前销售单已全出库时：计算部分明细金额
                    currItemAmount = (itemRealSalesAmount / orderItem.Quantity * outStockOrderItem.RealStockOutQuantity).RoundToShe(2);
                }

                outStockAmount.Add(outStockOrderItem.SysNo, currItemAmount);

                if (orderItem.ProductSalesType != (int)CustomerStatus.商品销售类型.赠品
                    || orderGiftAmount > orderSalesAmount - orderGiftAmount)
                {
                    currNotGiftAmount += (currItemAmount);
                }
            }

            return outStockAmount;
        }
        #endregion

        #region 订购商品明细

        /// <summary>
        /// 订单编辑，更新订购商品列表
        /// </summary>
        /// <param name="data">订购商品明细</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2013-06-24 朱成果 创建</remarks>
        /// <remarks>2013-09-17 黄志勇 修改</remarks>
        public bool InsertOrderItems(IEnumerable<Model.SoOrderItem> data)
        {

            bool flg = false;
            int orderId = data.FirstOrDefault<Model.SoOrderItem>().OrderSysNo;
            //原始列表
            IList<Model.SoOrderItem> list = Hyt.BLL.Order.SoOrderBo.Instance.GetOrderItemsByOrderId(orderId);
            //商品列表
            var ProductList = list.Select(m => m.ProductSysNo).ToList();
            //更新的列表
            List<Model.SoOrderItem> updateList = data.Where(m => ProductList.Contains(m.ProductSysNo)).ToList();
            //新添加的商品
            List<Model.SoOrderItem> insertList = data.Where(m => !ProductList.Contains(m.ProductSysNo)).ToList();

            if (insertList != null)
            {
                foreach (SoOrderItem p in insertList)
                {
                    Hyt.DataAccess.Order.ISoOrderItemDao.Instance.Insert(p);
                }
            }
            if (updateList != null)
            {
                foreach (SoOrderItem u in updateList)
                {
                    var item = list.Where(m => m.ProductSysNo == u.ProductSysNo).FirstOrDefault();
                    u.SysNo = item.SysNo;
                    u.Quantity = item.Quantity + u.Quantity;
                    u.ChangeAmount = item.ChangeAmount;
                    Hyt.DataAccess.Order.ISoOrderItemDao.Instance.UpdateOrderItemQuantity(u.SysNo, u.Quantity, u.ChangeAmount);
                }
            }
            //更新总价
            var money = SynchronousOrderAmount(orderId, true);
            Hyt.BLL.Finance.FnReceiptVoucherBo.Instance.UpdateOrderIncomeAmount(orderId, money);//更新订单收款单，应收金额

            flg = true;

            return flg;
        }

        /// <summary>
        /// 删除订单明细
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="sysNo">订单商品明细编号</param>
        /// <returns>操作是否成功 </returns>
        /// <remarks>2013-06-24 朱成果 创建</remarks>
        public bool DeleteOrderItem(int orderId, int sysNo)
        {

            //删除一条记录
            Hyt.DataAccess.Order.ISoOrderItemDao.Instance.Delete(sysNo);
            //更新总价
            var money = SynchronousOrderAmount(orderId, true);
            Hyt.BLL.Finance.FnReceiptVoucherBo.Instance.UpdateOrderIncomeAmount(orderId, money);//更新订单收款单，应收金额


            return true;

        }

        /// <summary>
        /// 更新订购商品明细的数量
        /// </summary>
        /// <param name="sysNo">明细编号</param>
        /// <param name="quantity">数量</param>
        /// <param name="orderID">订单编号</param>
        /// <param name="changeAmount">调价金额</param>
        /// <returns>操作是否成功 </returns>
        /// <remarks>2013-06-24 朱成果 创建</remarks>
        public bool UpdateOrderItemQuantity(int sysNo, int quantity, int orderID, decimal changeAmount)
        {

            //删除一条记录
            Hyt.DataAccess.Order.ISoOrderItemDao.Instance.UpdateOrderItemQuantity(sysNo, quantity, changeAmount);
            //更新总价
            var money = SynchronousOrderAmount(orderID, true);
            Hyt.BLL.Finance.FnReceiptVoucherBo.Instance.UpdateOrderIncomeAmount(orderID, money);//更新订单收款单，应收金额


            return true;
        }
        #endregion

        #region 门店下单

        /// <summary>
        /// 门店下单支付提货
        /// </summary>
        /// <param name="shopSysNo">门店SysNo.</param>
        /// <param name="customerSysNo">会员SysNo.</param>
        /// <param name="receiveName">收货人姓名.</param>
        /// <param name="receiveMobilePhoneNumber">收货人手机号码.</param>
        /// <param name="internalRemarks">对内备注.</param>
        /// <param name="orderItems">订购商品清单.</param>
        /// <param name="invoice">发票信息.</param>
        /// <param name="payTypeNo">支付方式.</param>
        /// <param name="user">The user.</param>
        /// <param name="voucherNo">刷卡流水号</param>
        /// <param name="easReceiptCode">EAS收款科目编码</param>
        /// <param name="experienceCoin">惠源币</param>
        /// <param name="couponCode">优惠券</param>
        /// <param name="changpriectitem">调价项 Key=购物车编号 value=调价值</param>
        /// <exception cref="System.ArgumentNullException">必需选择一个门店</exception>
        /// <returns>出库单，订单系统编号</returns>
        /// <remarks>
        /// 2016-5-19 杨浩 创建
        /// </remarks>
        public Tuple<int, int> CreateShopOrderFromOuter(int shopSysNo, int customerSysNo, string receiveName,
                                    string receiveMobilePhoneNumber
                                    , string internalRemarks, IList<Model.SoOrderItem> orderItems, FnInvoice invoice,
                                    int payTypeNo, SyUser user, string voucherNo, string easReceiptCode = null, int experienceCoin = 0, string couponCode = null, Dictionary<int, decimal> changpriectitem = null)
        {
            int orderCreatorSysNo = user.SysNo;
            var shop = WhWarehouseBo.Instance.GetWarehouseEntity(shopSysNo);
            if (shop == null) throw new ArgumentNullException("shopSysNo", "必需选择一个门店");

            if (shop.WarehouseType != (int)WarehouseStatus.仓库类型.门店)
            {
                //2013/11/6 朱家宏 修改 门店下单限制
                throw new HytException("门店下单必须为门店");
            }
            //订单发票处理
            if (invoice != null && invoice.InvoiceTypeSysNo < 1)
            {
                invoice.InvoiceTypeSysNo = InvoiceType.一般发票;
                invoice.Status = (int)FinanceStatus.发票状态.已开票;
                invoice.CreatedBy = user.SysNo;
                invoice.CreatedDate = DateTime.Now;
                invoice.LastUpdateDate = DateTime.Now;
                invoice.LastUpdateBy = user.SysNo;
            }
            //订单地址
            var receiveAddress = new SoReceiveAddress()
            {
                MobilePhoneNumber = receiveMobilePhoneNumber,
                Name = receiveName,
                AreaSysNo = shop.AreaSysNo
            };
            int? defaultWarehouseSysNo = shopSysNo;
            int deliveryTypeSysNo = DeliveryType.门店自提;
            int payTypeSysNo = payTypeNo;

            var orderSource = OrderStatus.销售单来源.门店下单;
            int? orderSourceSysNo = shopSysNo;
            OrderStatus.销售方式 salesType = OrderStatus.销售方式.普通订单;
            int? salesSysNo = null;
            var isHiddenToCustomer = OrderStatus.销售单对用户隐藏.否;
            string customerMessage = string.Empty;
            string deliveryRemarks = string.Empty;
            string deliveryTime = string.Empty;
            var contactBeforeDelivery = OrderStatus.配送前是否联系.是;
            string remarks = string.Format("门店【{0}】下单", shop.WarehouseName);
            var so = CreateOrderFromOuter(orderCreatorSysNo, customerSysNo,
                                    receiveAddress,
                                    defaultWarehouseSysNo, deliveryTypeSysNo, payTypeSysNo,
                                    null, experienceCoin, invoice,
                                    orderSource, orderSourceSysNo,
                                    salesType, salesSysNo,
                                    isHiddenToCustomer, customerMessage,
                                    internalRemarks, deliveryRemarks, deliveryTime,
                                    contactBeforeDelivery,
                                    remarks, changpriectitem);
            if (invoice != null && invoice.SysNo > 0)
            {
                invoice.InvoiceAmount = so.CashPay; //发票金额
                invoice.TransactionSysNo = so.TransactionSysNo;
                //IFnInvoiceDao.Instance.UpdateEntity(invoice);
                Hyt.BLL.Order.SoOrderBo.Instance.UpdateOrderInvoice(invoice); //更新发票 余勇 修改 改为调用业务层方法
            }
            WriteSoTransactionLog(so.TransactionSysNo
                                    ,
                                    string.Format(Constant.ORDER_TRANSACTIONLOG_SHOPORDER_CREATE, shop.WarehouseName,
                                                so.SysNo)
                                    , user.UserName);

            Finance.FnReceiptVoucherBo.Instance.CreateReceiptVoucherByOrder(so); //创建订单收款单
            //orderItems = ISoOrderItemDao.Instance.GetOrderItemsByOrderSysNo(so.SysNo);
            if (orderItems != null)
            {
                foreach (SoOrderItem oo in orderItems)
                {
                    oo.RealStockOutQuantity = oo.Quantity;
                }
            }
            var outStock = CreateOutStock(orderItems, shopSysNo, user); //订单出库
            ShopOrderBo.Instance.PickUp(outStock.SysNo, so.CashPay, payTypeNo, user, null, voucherNo, false, easReceiptCode); //门店提货
            return Tuple.Create(outStock.SysNo, so.SysNo);  //返回出库单号和订单号 2013/11/25 朱家宏添加 2013/12/6 黄志勇修改

        }



        /// <summary>
        /// 门店下单支付提货
        /// </summary>
        /// <param name="shopSysNo">门店SysNo.</param>
        /// <param name="customerSysNo">会员SysNo.</param>
        /// <param name="receiveName">收货人姓名.</param>
        /// <param name="receiveMobilePhoneNumber">收货人手机号码.</param>
        /// <param name="internalRemarks">对内备注.</param>
        /// <param name="orderItems">订购商品清单.</param>
        /// <param name="invoice">发票信息.</param>
        /// <param name="payTypeNo">支付方式.</param>
        /// <param name="user">The user.</param>
        /// <param name="voucherNo">刷卡流水号</param>
        /// <param name="easReceiptCode">EAS收款科目编码</param>
        /// <param name="experienceCoin">惠源币</param>
        /// <param name="couponCode">优惠券</param>
        /// <param name="changpriectitem">调价项 Key=购物车编号 value=调价值</param>
        /// <exception cref="System.ArgumentNullException">必需选择一个门店</exception>
        /// <param name="balance">会员卡余额</param>
        /// <returns>出库单，订单系统编号</returns>
        /// <remarks>
        /// 2013-10-14 朱家宏 增加 voucherNo参数
        /// 2013-12-6 黄志勇 修改 增加返回订单号
        /// </remarks>
        public Tuple<int, int> CreateShopOrder(int shopSysNo, int customerSysNo, string receiveName,
                                    string receiveMobilePhoneNumber
                                    , string internalRemarks, IList<Model.SoOrderItem> orderItems, FnInvoice invoice,
                                    int payTypeNo, SyUser user, string voucherNo, string easReceiptCode = null, int experienceCoin = 0, string couponCode = null, Dictionary<int, decimal> changpriectitem = null, List<SelectListItemNew> ListItemNew = null, decimal balance = 0m, int dsSysno = 0)
        {
            int orderCreatorSysNo = user.SysNo;
            var shop = WhWarehouseBo.Instance.GetWarehouseEntity(shopSysNo);
            if (shop == null) throw new ArgumentNullException("shopSysNo", "必需选择一个门店");

            if (shop.WarehouseType != (int)WarehouseStatus.仓库类型.门店)
            {
                //2013/11/6 朱家宏 修改 门店下单限制
                throw new HytException("门店下单必须为门店");
            }


            #region 减平台库存
            var products = BLL.Product.PdProductBo.Instance.GetProductListBySysnoList(orderItems.Select(x => x.ProductSysNo).ToList());
            if (products.Count != orderItems.Count)
            {
                throw new HytException("商品信息有误！");
            }
            var stockOutItem = orderItems.Select(x => new WhStockOutItem()
            {
                ProductQuantity = x.Quantity,
                ProductErpCode = products.Where(y => y.SysNo == x.ProductSysNo).FirstOrDefault().ErpCode,
                WarehouseErpCode = shop.ErpCode,
            }).ToList();

            var result = Hyt.BLL.Warehouse.WhWarehouseBo.Instance.ReduceStock(-1, shop.SysNo, stockOutItem);
            if (!result.Status)
            {
                throw new HytException(result.Message);
            }
            #endregion

            //订单发票处理
            if (invoice != null && invoice.InvoiceTypeSysNo < 1)
            {
                invoice.InvoiceTypeSysNo = InvoiceType.一般发票;
                invoice.Status = (int)FinanceStatus.发票状态.已开票;
                invoice.CreatedBy = user.SysNo;
                invoice.CreatedDate = DateTime.Now;
                invoice.LastUpdateDate = DateTime.Now;
                invoice.LastUpdateBy = user.SysNo;
            }
            //订单地址
            var receiveAddress = new SoReceiveAddress()
                {
                    MobilePhoneNumber = receiveMobilePhoneNumber,
                    Name = receiveName,
                    AreaSysNo = shop.AreaSysNo
                };
            int? defaultWarehouseSysNo = shopSysNo;
            int deliveryTypeSysNo = DeliveryType.门店自提;
            int payTypeSysNo = payTypeNo;
            //2013-9-28 杨文兵 订单明细内容不在由界面传入，改为从数据库中获取获取购物车
            //foreach (var item in orderItems)
            //{
            //    CrShoppingCartBo.Instance.Add(customerSysNo, item.ProductSysNo, item.Quantity,
            //                                  CustomerStatus.购物车商品来源.门店下单);
            //}
            CrShoppingCart shoppingCart = null;
            if (string.IsNullOrEmpty(couponCode))
            {
                shoppingCart = CrShoppingCartBo.Instance.GetShoppingCart(new[] { PromotionStatus.促销使用平台.门店 }, customerSysNo, false, false);
            }
            else
            {
                shoppingCart = CrShoppingCartBo.Instance.GetShoppingCart(new[] { PromotionStatus.促销使用平台.门店 }, customerSysNo, null, receiveAddress.AreaSysNo, deliveryTypeSysNo, null, couponCode, false, false);
            }
            var orderSource = OrderStatus.销售单来源.门店下单;
            int? orderSourceSysNo = shopSysNo;
            OrderStatus.销售方式 salesType = OrderStatus.销售方式.普通订单;
            int? salesSysNo = null;
            var isHiddenToCustomer = OrderStatus.销售单对用户隐藏.否;
            string customerMessage = string.Empty;
            string deliveryRemarks = string.Empty;
            string deliveryTime = string.Empty;
            var contactBeforeDelivery = OrderStatus.配送前是否联系.是;
            string remarks = string.Format("门店【{0}】下单", shop.WarehouseName);

            #region 获取当前仓库所属经销商
            int _dealerSysNo = 0;

            var current = BLL.Authentication.AdminAuthenticationBo.Instance.Current;
            if (current != null)
            {
                _dealerSysNo = current.IsBindDealer && current.Dealer != null ? current.Dealer.SysNo : 0;
            }
            //var dealerWhareHouseInfo = BLL.Distribution.DsDealerWharehouseBo.Instance.GetByWarehousSysNo((int)defaultWarehouseSysNo);
            //if (dealerWhareHouseInfo != null)
            //    _dealerSysNo = dealerWhareHouseInfo.DealerSysNo;

            if (_dealerSysNo == 0)
            {
                _dealerSysNo = dsSysno;
            }
            #endregion

            var so = CreateOrder(orderCreatorSysNo, customerSysNo,
                                    receiveAddress,
                                    defaultWarehouseSysNo, deliveryTypeSysNo, payTypeSysNo,
                                    shoppingCart, experienceCoin, invoice,
                                    orderSource, orderSourceSysNo,
                                    salesType, salesSysNo,
                                    isHiddenToCustomer, customerMessage,
                                    internalRemarks, deliveryRemarks, deliveryTime,
                                    contactBeforeDelivery,
                                    remarks, changpriectitem, ListItemNew, _dealerSysNo, balance);

            if (invoice != null && invoice.SysNo > 0)
            {
                invoice.InvoiceAmount = so.CashPay; //发票金额
                invoice.TransactionSysNo = so.TransactionSysNo;
                //IFnInvoiceDao.Instance.UpdateEntity(invoice);
                Hyt.BLL.Order.SoOrderBo.Instance.UpdateOrderInvoice(invoice); //更新发票 余勇 修改 改为调用业务层方法
            }
            WriteSoTransactionLog(so.TransactionSysNo
                                    ,
                                    string.Format(Constant.ORDER_TRANSACTIONLOG_SHOPORDER_CREATE, shop.WarehouseName,
                                                so.SysNo)
                                    , user.UserName);

            Finance.FnReceiptVoucherBo.Instance.CreateReceiptVoucherByOrder(so); //创建订单收款单
            orderItems = ISoOrderItemDao.Instance.GetOrderItemsByOrderSysNo(so.SysNo);
            if (orderItems != null)
            {
                foreach (SoOrderItem oo in orderItems)
                {
                    oo.RealStockOutQuantity = oo.Quantity;
                }
            }

            var outStock = CreateOutStock(orderItems, shopSysNo, user); //订单出库


            ShopOrderBo.Instance.PickUp(outStock.SysNo, so.CashPay, payTypeNo, user, null, voucherNo, false, easReceiptCode); //门店提货


            #region 出库单出库的时候不再减EAS库存，此处减EAS库存 因暂无对接erp所以注释


            // todo:减EAS库存
            Hyt.BLL.Warehouse.WhWarehouseBo.Instance.UpdateErpProductNumber(outStock.SysNo);

            #endregion

            return Tuple.Create(outStock.SysNo, so.SysNo);  //返回出库单号和订单号 2013/11/25 朱家宏添加 2013/12/6 黄志勇修改

        }

        /// <summary>
        /// 门店下单延迟自提
        /// </summary>
        /// <param name="shopSysNo">门店SysNo.</param>
        /// <param name="customerSysNo">会员SysNo.</param>
        /// <param name="receiveName">收货人姓名.</param>
        /// <param name="receiveMobilePhoneNumber">收货人手机号码.</param>
        /// <param name="internalRemarks">对内备注.</param>
        /// <param name="delayReason">延迟原因.</param>
        /// <param name="pickUpDate">延迟自提时间.</param>
        /// <param name="orderItems">订购商品清单.</param>
        /// <param name="invoice">发票信息.</param>
        /// <param name="user">The user.</param>
        /// <param name="experienceCoin">支付惠源币</param>
        /// <param name="couponCode">使用优惠券</param>
        /// <param name="changpriectitem">调价项 Key=购物车编号 value=调价值</param>
        /// <returns>出库单系统编号</returns>
        /// <remarks>2014-1-21　黄志勇　添加注释</remarks>
        public Tuple<int, int> CreateShopOrderDelayPickUp(int shopSysNo, int customerSysNo, string receiveName, string receiveMobilePhoneNumber
            , string internalRemarks, string delayReason, DateTime pickUpDate, IList<Model.SoOrderItem> orderItems, FnInvoice invoice, SyUser user, int experienceCoin, string couponCode = null, List<SelectListItemNew> ListItemNew = null, Dictionary<int, decimal> changpriectitem = null, int dssysno = 0)
        {

            int orderCreatorSysNo = user.SysNo;
            var shop = WhWarehouseBo.Instance.GetWarehouseEntity(shopSysNo);
            if (shop == null) throw new ArgumentNullException("shopSysNo", "必需选择一个门店");
            //订单发票处理
            if (invoice != null && invoice.InvoiceTypeSysNo < 1)
            {
                invoice.InvoiceTypeSysNo = InvoiceType.一般发票;
                invoice.Status = (int)FinanceStatus.发票状态.待开票;
                invoice.CreatedBy = user.SysNo;
                invoice.CreatedDate = DateTime.Now;
            }
            //订单地址
            var receiveAddress = new SoReceiveAddress()
            {
                MobilePhoneNumber = receiveMobilePhoneNumber,
                Name = receiveName,
                AreaSysNo = shop.AreaSysNo
            };

            int? defaultWarehouseSysNo = shopSysNo;
            int deliveryTypeSysNo = DeliveryType.门店自提;
            int payTypeSysNo = Hyt.Model.SystemPredefined.PaymentType.现金; // 现金支付方式

            //2013-9-28 杨文兵 订单明细内容不在由界面传入，改为从数据库中获取获取购物车
            //foreach (var item in orderItems)
            //{
            //    CrShoppingCartBo.Instance.Add(customerSysNo, item.ProductSysNo, item.Quantity,
            //                                  CustomerStatus.购物车商品来源.门店下单);
            //}
            CrShoppingCart shoppingCart = null;
            if (string.IsNullOrEmpty(couponCode))
            {
                shoppingCart = CrShoppingCartBo.Instance.GetShoppingCart(new[] { PromotionStatus.促销使用平台.门店 }, customerSysNo, false, false);
            }
            else
            {
                //使用优惠券
                shoppingCart = CrShoppingCartBo.Instance.GetShoppingCart(new[] { PromotionStatus.促销使用平台.门店 }, customerSysNo, null, receiveAddress.AreaSysNo, deliveryTypeSysNo, null, couponCode, false, false);
            }
            var orderSource = OrderStatus.销售单来源.门店下单;
            int? orderSourceSysNo = shopSysNo;
            OrderStatus.销售方式 salesType = OrderStatus.销售方式.普通订单;
            int? salesSysNo = null;
            var isHiddenToCustomer = OrderStatus.销售单对用户隐藏.否;
            string customerMessage = string.Empty;
            string deliveryRemarks = string.Empty;
            string deliveryTime = string.Empty;
            var contactBeforeDelivery = OrderStatus.配送前是否联系.是;
            string remarks = string.Format("门店【{0}】下单延迟自提", shop.WarehouseName);
            var so = CreateOrder(orderCreatorSysNo, customerSysNo,
                                 receiveAddress,
                                 defaultWarehouseSysNo, deliveryTypeSysNo, payTypeSysNo,
                                 shoppingCart, experienceCoin, invoice,
                                 orderSource, orderSourceSysNo,
                                 salesType, salesSysNo,
                                 isHiddenToCustomer, customerMessage,
                                 internalRemarks, deliveryRemarks, deliveryTime,
                                 contactBeforeDelivery,
                                 remarks, changpriectitem, ListItemNew, dssysno);
            if (invoice != null && invoice.SysNo > 0)
            {
                invoice.InvoiceAmount = so.CashPay; //发票金额
                invoice.TransactionSysNo = so.TransactionSysNo;
                //IFnInvoiceDao.Instance.UpdateEntity(invoice);
                Hyt.BLL.Order.SoOrderBo.Instance.UpdateOrderInvoice(invoice); //更新发票 余勇 修改 改为调用业务层方法
            }
            WriteSoTransactionLog(so.TransactionSysNo
                , string.Format(Constant.ORDER_TRANSACTIONLOG_SHOPORDER_CREATE, shop.WarehouseName, so.SysNo)
                , user.UserName);

            Finance.FnReceiptVoucherBo.Instance.CreateReceiptVoucherByOrder(so); //创建订单收款单
            orderItems = ISoOrderItemDao.Instance.GetOrderItemsByOrderSysNo(so.SysNo);
            if (orderItems != null)
            {
                foreach (SoOrderItem oo in orderItems)
                {
                    oo.RealStockOutQuantity = oo.Quantity;
                }
            }
            var outStock = CreateOutStock(orderItems, shopSysNo, user); //订单出库
            outStock.PickUpDate = pickUpDate;
            outStock.Remarks = delayReason;
            Hyt.DataAccess.Warehouse.IOutStockDao.Instance.Update(outStock);
            return Tuple.Create(outStock.SysNo, so.SysNo);  //返回出库单号和订单号 
            //return outStock.SysNo;

        }

        /// <summary>
        /// 门店下单转快递
        /// </summary>
        /// <param name="shopSysNo">门店SysNo.</param>
        /// <param name="customerSysNo">会员SysNo.</param>
        /// <param name="receiveAddress">收货地址信息.</param>
        /// <param name="internalRemarks">对内备注.</param>
        /// <param name="customerMessage">会员留言.</param>
        /// <param name="orderItems">订购商品清单.</param>
        /// <param name="invoice">发票信息.</param>
        /// <param name="payTypeNo">付款方式.</param>
        /// <param name="user">The user.</param>
        /// <param name="voucherNo">收款单凭证号</param>
        /// <param name="easReceiptCode">EAS收款科目编码</param>
        /// <param name="experienceCoin">支付惠源币</param>
        /// <param name="couponCode">使用的优惠券</param>
        /// <param name="changpriectitem">调价项 Key=购物车编号 value=调价值</param>
        /// <exception cref="System.ArgumentNullException">必需选择一个门店</exception>
        /// <returns>订单系统编号</returns>
        /// <remarks>2014-1-21　黄志勇　添加注释</remarks>
        public int CreateShopOrderToCourier(int shopSysNo, int customerSysNo, SoReceiveAddress receiveAddress, string internalRemarks, string customerMessage, IList<Model.SoOrderItem> orderItems, FnInvoice invoice, int payTypeNo, SyUser user, string voucherNo, string easReceiptCode = null, int experienceCoin = 0, string couponCode = null, Dictionary<int, decimal> changpriectitem = null)
        {
            int orderCreatorSysNo = user.SysNo;
            var shop = WhWarehouseBo.Instance.GetWarehouseEntity(shopSysNo);
            if (shop == null) throw new ArgumentNullException("shopSysNo", "必需选择一个门店");
            //订单发票处理
            if (invoice != null && invoice.InvoiceTypeSysNo < 1)
            {
                invoice.InvoiceTypeSysNo = InvoiceType.一般发票;
                invoice.Status = (int)FinanceStatus.发票状态.待开票;
                invoice.CreatedBy = user.SysNo;
                invoice.CreatedDate = DateTime.Now;
            }
            int deliveryTypeSysNo = DeliveryType.第三方快递;
            int payTypeSysNo = payTypeNo;
            int? defaultWarehouseSysNo = null;
            var warehouse = Hyt.BLL.Warehouse.WhWarehouseBo.Instance.GetWhWareHouse(receiveAddress.AreaSysNo, null, deliveryTypeSysNo, WarehouseStatus.仓库状态.启用).FirstOrDefault();
            if (warehouse != null)
            {
                defaultWarehouseSysNo = warehouse.SysNo;
            }
            CrShoppingCart shoppingCart = null;
            if (string.IsNullOrEmpty(couponCode))
            {
                shoppingCart = CrShoppingCartBo.Instance.GetShoppingCart(new[] { PromotionStatus.促销使用平台.门店 }, customerSysNo, false, false);
            }
            else
            {
                //使用优惠券
                shoppingCart = CrShoppingCartBo.Instance.GetShoppingCart(new[] { PromotionStatus.促销使用平台.门店 }, customerSysNo, null, receiveAddress.AreaSysNo, deliveryTypeSysNo, null, couponCode, false, false);
            }
            var orderSource = OrderStatus.销售单来源.门店下单;
            int? orderSourceSysNo = shopSysNo;
            OrderStatus.销售方式 salesType = OrderStatus.销售方式.普通订单;
            int? salesSysNo = null;
            var isHiddenToCustomer = OrderStatus.销售单对用户隐藏.否;
            string deliveryRemarks = string.Empty;
            string deliveryTime = string.Empty;
            var contactBeforeDelivery = OrderStatus.配送前是否联系.是;
            string remarks = string.Format("门店【{0}】下单转快递", shop.WarehouseName);
            var so = CreateOrder(orderCreatorSysNo, customerSysNo,
                                    receiveAddress,
                                    defaultWarehouseSysNo, deliveryTypeSysNo, payTypeSysNo,
                                    shoppingCart, experienceCoin, invoice,
                                    orderSource, orderSourceSysNo,
                                    salesType, salesSysNo,
                                    isHiddenToCustomer, customerMessage,
                                    internalRemarks, deliveryRemarks, deliveryTime,
                                    contactBeforeDelivery,
                                    remarks, changpriectitem);
            if (invoice != null && invoice.SysNo > 0)
            {
                invoice.InvoiceAmount = so.CashPay; //发票金额
                invoice.TransactionSysNo = so.TransactionSysNo;
                //IFnInvoiceDao.Instance.UpdateEntity(invoice);
                Hyt.BLL.Order.SoOrderBo.Instance.UpdateOrderInvoice(invoice); //更新发票 余勇 修改 改为调用业务层方法
            }
            #region 收款单及明细
            Hyt.BLL.Finance.FnReceiptVoucherBo.Instance.CreateReceiptVoucherByOrder(so);//创建订单收款单
            FnReceiptVoucherItem ritem = new FnReceiptVoucherItem()
                {
                    Amount = so.CashPay,
                    CreatedBy = user.SysNo,
                    CreatedDate = DateTime.Now,
                    PaymentTypeSysNo = payTypeNo,
                    TransactionSysNo = so.TransactionSysNo,
                    Status = (int)Model.WorkflowStatus.FinanceStatus.收款单明细状态.有效,
                    VoucherNo = voucherNo,
                    EasReceiptCode = easReceiptCode,
                    ReceivablesSideType = (int)Hyt.Model.WorkflowStatus.FinanceStatus.收款方类型.仓库,//收款方仓库门店
                    ReceivablesSideSysNo = shopSysNo //门店编号
                };
            Hyt.BLL.Finance.FnReceiptVoucherBo.Instance.InsertOrderReceiptVoucher(so.SysNo, ritem);//创建收款单明细
            #endregion
            //收现金自动确认收款单
            if (so.PayTypeSysNo == (int)Hyt.Model.SystemPredefined.PaymentType.现金 || so.PayTypeSysNo == (int)Hyt.Model.SystemPredefined.PaymentType.现金预付)
            {
                Hyt.BLL.Finance.FnReceiptVoucherBo.Instance.AutoConfirmReceiptVoucher(so.SysNo, user);//收现金自动确认收款单
            }
            //同步支付时间的到订单主表
            ISoOrderDao.Instance.UpdateOrderPayDteById(so.SysNo);
            WriteSoTransactionLog(so.TransactionSysNo
                    , string.Format(Constant.ORDER_TRANSACTIONLOG_SHOPORDER_CREATE, shop.WarehouseName, so.SysNo)
                    , user.UserName);
            return so.SysNo;
        }

        #endregion

        #region 发票相关

        /// <summary>
        /// 创建发票
        /// </summary>
        /// <param name="entity">发票实体</param>
        /// <returns>发票编号</returns>
        ///<remarks>2013-06-25 朱成果 创建</remarks> 
        public int InsertOrderInvoice(FnInvoice entity)
        {
            return Hyt.DataAccess.Order.IFnInvoiceDao.Instance.InsertEntity(entity);
        }

        /// <summary>
        /// 更新发票
        /// </summary>
        /// <param name="entity">发票实体</param>
        ///<remarks>2013-06-25 朱成果 创建</remarks> 
        public void UpdateOrderInvoice(FnInvoice entity)
        {
            Hyt.DataAccess.Order.IFnInvoiceDao.Instance.UpdateEntity(entity);
            MemoryProvider.Default.Remove(string.Format(KeyConstant.FnInvoice, entity.SysNo));
        }

        /// <summary>
        /// 更新订单发票编号
        /// </summary>
        /// <param name="orderSysNo">订单系统编号</param>
        /// <param name="invoiceSysno">发票系统编号</param>
        /// <returns>是否成功</returns>
        ///<remarks>2013-11-14 周唐炬 创建</remarks>
        public void UpdateOrderInvoice(int orderSysNo, int invoiceSysno)
        {
            ISoOrderDao.Instance.UpdateInvoiceNo(orderSysNo, invoiceSysno);
            var cacheKey = string.Format("CACHE_SOORDER_{0}", orderSysNo);
            Hyt.Infrastructure.Memory.MemoryProvider.Default.Remove(cacheKey);
        }
        #endregion

        #region 出库单作废，签收更新订单状态
        /// <summary>
        /// 出库单作废更新订单状态 （需要订一个合适的接口名字）
        /// </summary>
        /// <param name="whStockOutSysNo">出库单SysNo</param>
        /// <param name="user">操作人</param>
        /// <param name="reason">作废原因</param>
        ///<remarks>2013-06-28 杨文兵 创建</remarks> 
        ///<remarks>2013-06-28 朱成果 修改</remarks>
        ///<remarks>2013-07-03 何方 修改方法名</remarks>
        public void UpdateSoStatusForSotckOutCancel(int whStockOutSysNo, SyUser user, string reason = null)
        {
            //1.获取出库单主表 和明细表
            var whStockOutEntity = Hyt.DataAccess.Warehouse.IOutStockDao.Instance.GetModel(whStockOutSysNo);
            if (whStockOutEntity == null || whStockOutEntity.Items == null)
            {
                throw new ArgumentNullException("出库单或者出库单明细为空");
            }
            if (whStockOutEntity.Status != (int)Hyt.Model.WorkflowStatus.WarehouseStatus.出库单状态.作废)
            {
                throw new Exception("出库单没有完成作废！");
            }
            //销售单明细
            var orderItems = Hyt.DataAccess.Order.ISoOrderItemDao.Instance.GetOrderItemsByOrderSysNo(whStockOutEntity.OrderSysNO);

            string transactionSysNo = string.Empty;
            foreach (WhStockOutItem item in whStockOutEntity.Items)
            {

                // var oItem = orderItems.FirstOrDefault(p => p.ProductSysNo == item.ProductSysNo);
                var oItem = orderItems.FirstOrDefault(p => p.SysNo == item.OrderItemSysNo);
                if (oItem == null)
                {
                    throw new Exception("销售单明细数据有误!");
                }
                if (oItem.RealStockOutQuantity < item.ProductQuantity)
                {
                    throw new Exception("出库单商品数量大于实际出库数量!");
                }

                transactionSysNo = oItem.TransactionSysNo;
                oItem.RealStockOutQuantity = oItem.RealStockOutQuantity - item.ProductQuantity;
                Hyt.DataAccess.Order.ISoOrderItemDao.Instance.UpdateOutStockQuantity(oItem.SysNo, oItem.RealStockOutQuantity);
            }
            //4.更新销售单主表状态 为 待出库 或者部分出库
            int status = (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.待创建出库单;
            foreach (var p in orderItems)
            {
                if (p.RealStockOutQuantity > 0)
                {
                    status = (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.部分创建出库单;
                    break;
                }
            }
            var order = SoOrderBo.Instance.GetEntity(whStockOutEntity.OrderSysNO);
            order.Status = status;
            //order.DefaultWarehouseSysNo = 0;//缺货订单
            //if (!string.IsNullOrEmpty(reason))
            //{
            //    order.InternalRemarks = reason;//缺货原因对内备注
            //}
            UpdateOrder(order); //更新订单 余勇 修改为调用业务层方法 Hyt.DataAccess.Order.ISoOrderDao.Instance.Update(order);//更新订单信息
            if (user != null)
            {
                WriteSoTransactionLog(transactionSysNo
                    , string.Format(Constant.ORDER_OUTSTOCK_CANCEL, whStockOutSysNo, string.IsNullOrEmpty(reason) ? string.Empty : reason)
                                    , user.UserName);
            }

        }

        /// <summary>
        /// 出库单签收时候更新订单状态
        /// </summary>
        /// <param name="entity">出库单实体</param>
        /// <param name="user">操作人</param>
        ///　<remarks>2013-06-28 朱成果 修改</remarks>
        ///　<remarks>2014-01-17 黄志勇 更新新增会员明细</remarks>
        public void UpdateSoStatusForSotckOutSign(WhStockOut entity, SyUser user)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("出库单为空");
            }
            if (entity.Status != (int)Hyt.Model.WorkflowStatus.WarehouseStatus.出库单状态.已签收)
            {
                throw new Exception("出库单未完成签收");
            }
            if (user != null)
            {
                var order = GetEntity(entity.OrderSysNO);//获取订单详情
                //2015-10-08 王耀发禁用
                //if (order.DeliveryTypeSysNo != Hyt.Model.SystemPredefined.DeliveryType.第三方快递)
                //{
                //    WriteSoTransactionLog(entity.TransactionSysNo
                //                       , string.Format(Constant.ORDER_OUTSTOCK_SIGN, entity.SysNo)
                //                       , user.UserName);
                //}
                if (Hyt.DataAccess.Order.ISoOrderDao.Instance.CheckedOrderFinish(entity.OrderSysNO))//满足完结状态
                {

                    #region 扣除分销商预存款 2013-09-13 朱成果
                    if (order.OrderSource == (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单来源.分销商升舱)
                    {
                        Hyt.BLL.MallSeller.DsOrderBo.Instance.CompleteOrder(order.SysNo, order.CashPay > 0);
                    }
                    #endregion

                    //所有第三方快递订单前台状态显示为已发货
                    UpdateOnlineStatusByOrderID(entity.OrderSysNO,
                        order.DeliveryTypeSysNo == Hyt.Model.SystemPredefined.DeliveryType.第三方快递
                            ? Constant.OlineStatusType.已发货
                            : Constant.OlineStatusType.已完成);   //更新订单前台显示状态 余勇修改为调用业务层方法
                    UpdateOrderStatus(entity.OrderSysNO, (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.已完成);  //更新订单状态 余勇修改为调用业务层方法
                    //Hyt.DataAccess.Order.ISoOrderDao.Instance.UpdateOrderStatus(entity.OrderSysNO, (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.已完成); //更新订单完结状态

                    //送积分 2015-11-2 王耀发 注释 改为收款送积分
                    //Hyt.BLL.LevelPoint.PointBo.Instance.OrderIncreasePoint(order.CustomerSysNo, order.SysNo, (int)order.CashPay, order.TransactionSysNo);//增加积分

                    WriteSoTransactionLog(entity.TransactionSysNo
                                   , string.Format(Constant.ORDER_FINISH, entity.OrderSysNO)
                                   , user.UserName);
                }
                LgSettlementBo.Instance.WriteShopNewCustomerDetail(order.CustomerSysNo, entity.StockOutAmount);
            }
        }
        #endregion

        #region 更新订单前台显示状态

        //public SoOrder UpdateOrderOnlineStatus(SoOrder order, OrderStatus.销售单状态 OrderStatus)
        //{
        //    order.OnlineStatus=""
        //    return order;
        //}
        /// <summary>
        /// 根据订单号更新订单前台显示状态
        /// </summary>
        /// <param name="orderID">订单号</param>
        ///  <param name="onlineStatus">前台显示状态</param>
        /// <remarks>2013-07-04 朱成果  创建</remarks> 
        public void UpdateOnlineStatusByOrderID(int orderID, string onlineStatus)
        {
            Hyt.DataAccess.Order.ISoOrderDao.Instance.UpdateOnlineStatusByOrderID(orderID, onlineStatus);
            var cacheKey = string.Format("CACHE_SOORDER_{0}", orderID);
            Hyt.Infrastructure.Memory.MemoryProvider.Default.Remove(cacheKey);
        }

        /// <summary>
        /// 根据事物编号更新订单前台显示状态
        /// </summary>
        /// <param name="transactionSysNo">事物编号</param>
        ///  <param name="onlineStatus">前台显示状态</param>
        /// <remarks>2013-07-04 朱成果  创建</remarks> 
        public void UpdateOnlineStatusByTransactionSysNo(string transactionSysNo, string onlineStatus)
        {
            Hyt.DataAccess.Order.ISoOrderDao.Instance.UpdateOnlineStatusByTransactionSysNo(transactionSysNo, onlineStatus);

            var sysno = Convert.ToInt32(System.Text.RegularExpressions.Regex.Replace(transactionSysNo, "T0*", ""));
            var cacheKey = string.Format("CACHE_SOORDER_{0}", sysno);
            Hyt.Infrastructure.Memory.MemoryProvider.Default.Remove(cacheKey);
        }
        #endregion

        #region 更新订单价格信息,发票总额
        /// <summary>
        /// 更新订单价格信息,返回优惠及打折后的金额
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="isUpdateInvoice">是否更新发票金额</param>
        /// <returns>订单.CashPay</returns>
        /// <remarks>2013-06-24 朱成果 创建</remarks>
        public decimal SynchronousOrderAmount(int orderId, bool isUpdateInvoice = false)
        {
            decimal money = Hyt.DataAccess.Order.ISoOrderItemDao.Instance.SynchronousOrderAmount(orderId);
            //更新发票信息
            if (isUpdateInvoice)
            {
                var fn = Hyt.DataAccess.Order.IFnInvoiceDao.Instance.GetFnInvoiceByOrderID(orderId);
                if (fn != null)
                {
                    fn.InvoiceAmount = money;
                    fn.LastUpdateDate = DateTime.Now;
                    //Hyt.DataAccess.Order.IFnInvoiceDao.Instance.UpdateEntity(fn);
                    Hyt.BLL.Order.SoOrderBo.Instance.UpdateOrderInvoice(fn); //更新发票 余勇 修改 改为调用业务层方法
                }
            }
            return money;
        }
        #endregion

        #region 所有出库单拒收，作废订单
        /// <summary>
        /// 所有出库单拒收，作废订单
        /// </summary>
        /// <param name="orderID">订单号</param>
        /// <param name="user">操作人</param>
        /// <remarks>2013-07-30 朱成果 创建</remarks> 
        public void DeclinedOrder(int orderID, SyUser user)
        {
            //所有出库单拒收，作废订单
            //作废收款单
            var soOrder = SoOrderBo.Instance.GetEntity(orderID);
            if (soOrder == null)
            {
                throw new Exception("订单信息不存在");
            }
            bool IsCanDeclined = true;
            IList<WhStockOut> list = Hyt.DataAccess.Warehouse.IOutStockDao.Instance.GetWhStockOutListByOrderID(orderID);
            if (list != null && list.Count > 0)
            {
                foreach (WhStockOut item in list)
                {
                    if (item.Status == (int)Hyt.Model.WorkflowStatus.WarehouseStatus.出库单状态.作废 || item.Status == (int)Hyt.Model.WorkflowStatus.WarehouseStatus.出库单状态.拒收)
                    {
                        continue;
                    }
                    else
                    {
                        IsCanDeclined = false;
                        throw new Exception("存在未拒收或者作废的出库单:" + item.SysNo);
                    }
                }
            }
            if (IsCanDeclined)
            {
                CancelOrderForWarehouse(soOrder, user);
            }
        }
        #endregion

        #region 获取待支付订单数

        /// <summary>
        /// 获取待支付订单数
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <returns>待支付订单数</returns>
        /// <remarks>2013-08-27 周瑜 创建</remarks>
        public int GetUnPaidOrderCount(int customerSysNo)
        {
            return ISoOrderDao.Instance.GetUnPaidOrderCount(customerSysNo);
        }
        /// <summary>
        /// 获取未评论商品数量
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <returns>未评论商品数量</returns>
        /// <remarks>2013-08-27 周瑜 创建</remarks>
        public int GetUnCommentsCount(int customerSysNo)
        {
            return ISoOrderDao.Instance.GetUnCommentsCount(customerSysNo);
        }

        #endregion

        #region 同步订单状态
        /// <summary>
        /// 更新订单支付状态
        /// </summary>
        /// <param name="sysno">订单系统编号.</param>
        /// <param name="payStatus">订单支付状态</param>
        /// <remarks>
        /// 2013-10-18 何方 创建
        /// </remarks>
        public void UpdatePayStatus(int sysno, OrderStatus.销售单支付状态 payStatus)
        {
            ISoOrderDao.Instance.UpdatePayStatus(sysno, (int)payStatus);
            var cacheKey = string.Format("CACHE_SOORDER_{0}", sysno);
            Hyt.Infrastructure.Memory.MemoryProvider.Default.Remove(cacheKey);
        }

        /// <summary>
        /// 根据已出库数量返回相应的订单状态
        /// </summary>
        /// <param name="items">订单明细</param>
        /// <returns>只返回 待创建出库单 部分创建出库单 已创建出库单 </returns>
        /// <remarks>2013-08-29 朱成果 创建</remarks>
        private Hyt.Model.WorkflowStatus.OrderStatus.销售单状态 GetOrderStatusByRealStockOutQuantity(IList<SoOrderItem> items)
        {
            Hyt.Model.WorkflowStatus.OrderStatus.销售单状态 s = Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.待创建出库单;
            int buycount = 0;
            int outcount = 0;
            foreach (SoOrderItem p in items)
            {
                buycount += p.Quantity;
                outcount += p.RealStockOutQuantity;
            }
            if (outcount > 0)//有出库
            {
                if (outcount < buycount)
                {
                    s = Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.部分创建出库单;
                }
                else
                {
                    s = Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.已创建出库单;
                }
            }
            return s;
        }

        /// <summary>
        /// 给物流使用的订单作废方法
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="user">用户</param>
        /// <remarks>2013-09-02 朱成果 创建</remarks>
        private void CancelOrderForWarehouse(SoOrder order, SyUser user)
        {
            order.Status = (int)OrderStatus.销售单状态.作废;
            order.OnlineStatus = Constant.OlineStatusType.作废;
            order.LastUpdateBy = user.SysNo;
            order.LastUpdateDate = DateTime.Now;
            order.CancelUserSysNo = user.SysNo;
            order.CancelDate = DateTime.Now;
            order.CancelUserType = 20;
            order.InternalRemarks = "出库单拒收,订单作废。【系统】";
            UpdateOrder(order); //更新订单 余勇 修改为调用业务层方法 ISoOrderDao.Instance.Update(order);

            //物流组LgSettlementBo.Return 方法已经处理退款，此处不处理退款
            //Hyt.BLL.Finance.FnReceiptVoucherBo.Instance.CancelOrderReceipt(order, user);
            WriteSoTransactionLog(order.TransactionSysNo,
                                      string.Format(Constant.ORDER_TRANSACTIONLOG_CANCEL, order.SysNo),
                                      user.UserName);
        }

        /// <summary>
        /// 同步订单状态(给物流组使用，不建议其他地方使用)
        /// </summary>
        /// <param name="orderId">订单编号</param>
        /// <param name="user">用户</param>
        /// <remarks>2013-08-29 朱成果 创建</remarks>
        public void SynchronousOrderStatus(int orderId, SyUser user)
        {
            /*
            
            
     待创建出库单	销售单明细数据中“实际出库数量”都等于0时			
     部分创建出库单	销售单明细数据中“实际出库数量”都>0时,并且不等于“订购数量”时			
     已创建出库单	销售单明细数据中“实际出库数量”等于“订购数量”时			
     已完成	满足“红色”条件，查找所有出库单，排除“已作废”出库单，剩下的出库单状态在“已签收，部分退货，全部退货”中，则将订单状态改为已完成。			
     作废	（只有货到付款有这种情况）查找所有出库单，排除“已作废”出库单，剩下的出库单状态全部是“拒收”，则将订单状态改为已作废。
            
           
             */

            int currectStatus = Hyt.DataAccess.Order.ISoOrderDao.Instance.GetOrderStatus(orderId);//获取当前订单状态
            IList<SoOrderItem> lstItem;
            SoOrder order;
            int newStatus;
            switch (currectStatus)
            {
                case (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.待审核:
                    //状态手动变动
                    break;
                case (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.待支付:
                    //状态手动变动
                    break;
                case (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.待创建出库单:
                    //满足一定条件状态变成:部分创建出库单
                    //满足一定条件状态变成:已创建出库单
                    lstItem = Hyt.DataAccess.Order.ISoOrderItemDao.Instance.GetOrderItemsByOrderSysNo(orderId);
                    newStatus = (int)GetOrderStatusByRealStockOutQuantity(lstItem);
                    if (newStatus != currectStatus)
                    {
                        //更改状态
                        UpdateOrderStatus(orderId, newStatus);  //更新订单状态 余勇修改为调用业务层方法 Hyt.DataAccess.Order.ISoOrderDao.Instance.UpdateOrderStatus(orderId, newStatus);
                    }
                    break;
                case (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.部分创建出库单:
                    //满足一定条件状态变成:以创建出库单
                    lstItem = Hyt.DataAccess.Order.ISoOrderItemDao.Instance.GetOrderItemsByOrderSysNo(orderId);
                    newStatus = (int)GetOrderStatusByRealStockOutQuantity(lstItem);
                    if (newStatus != currectStatus)
                    {
                        //更改状态
                        UpdateOrderStatus(orderId, newStatus);  //更新订单状态 余勇修改为调用业务层方法 Hyt.DataAccess.Order.ISoOrderDao.Instance.UpdateOrderStatus(orderId, newStatus);
                    }
                    break;
                case (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.已创建出库单:
                    //满足一定条件状态变成:待创建出库单
                    //满足一定条件状态变成:部分创建出库单
                    lstItem = Hyt.DataAccess.Order.ISoOrderItemDao.Instance.GetOrderItemsByOrderSysNo(orderId);
                    newStatus = (int)GetOrderStatusByRealStockOutQuantity(lstItem);
                    if (newStatus != currectStatus)
                    {
                        //更改状态
                        UpdateOrderStatus(orderId, newStatus);  //更新订单状态 余勇修改为调用业务层方法 Hyt.DataAccess.Order.ISoOrderDao.Instance.UpdateOrderStatus(orderId, newStatus);
                    }
                    else
                    {
                        //满足一定条件状态变成:完成
                        //满足一定条件状态变成:作废
                        if (Hyt.DataAccess.Order.ISoOrderDao.Instance.CheckedOrderFinish(orderId))//满足完结状态
                        {
                            UpdateOrderStatus(orderId, (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.已完成);  //更新订单状态 余勇修改为调用业务层方法  Hyt.DataAccess.Order.ISoOrderDao.Instance.UpdateOrderStatus(orderId, (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.已完成); //更新订单完结状态
                            #region 默认增加积分为订单金额，但是注意拒收退换货应减去积分
                            order = GetEntity(orderId);
                            Hyt.BLL.LevelPoint.PointBo.Instance.OrderIncreasePoint(order.CustomerSysNo, order.SysNo, (int)order.CashPay, order.TransactionSysNo);//增加积分
                            #endregion
                            #region 扣除分销商预存款 2013-09-13 朱成果
                            if (order.OrderSource == (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单来源.分销商升舱)
                            {
                                Hyt.BLL.MallSeller.DsOrderBo.Instance.CompleteOrder(order.SysNo, order.CashPay > 0);
                            }
                            #endregion
                            UpdateOnlineStatusByOrderID(orderId, order.DeliveryTypeSysNo == Hyt.Model.SystemPredefined.DeliveryType.第三方快递 ? Constant.OlineStatusType.已发货 : Constant.OlineStatusType.已完成);  //更新订单前台显示状态 余勇修改为调用业务层方法
                            WriteSoTransactionLog(order.TransactionSysNo
                                           , string.Format(Constant.ORDER_FINISH, orderId)
                                           , user.UserName);
                        }
                        else if (Hyt.DataAccess.Order.ISoOrderDao.Instance.CheckedOrderCancel(orderId))//满足作废条件
                        {
                            order = GetEntity(orderId);
                            CancelOrderForWarehouse(order, user);
                        }
                    }
                    break;
                case (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.已完成:
                    if (!Hyt.DataAccess.Order.ISoOrderDao.Instance.CheckedOrderFinish(orderId))//不满足完结状态
                    {
                        lstItem = Hyt.DataAccess.Order.ISoOrderItemDao.Instance.GetOrderItemsByOrderSysNo(orderId);
                        newStatus = (int)GetOrderStatusByRealStockOutQuantity(lstItem);
                        if (newStatus == (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.已创建出库单 && Hyt.DataAccess.Order.ISoOrderDao.Instance.CheckedOrderCancel(orderId))//满足作废
                        {
                            order = GetEntity(orderId);
                            CancelOrderForWarehouse(order, user);
                        }
                        else
                        {
                            UpdateOrderStatus(orderId, newStatus); //更新订单状态 余勇修改为调用业务层方法  Hyt.DataAccess.Order.ISoOrderDao.Instance.UpdateOrderStatus(orderId, newStatus);
                            UpdateOnlineStatusByOrderID(orderId, Constant.OlineStatusType.待出库);  //更新订单前台显示状态 余勇修改为调用业务层方法
                        }
                    }
                    break;
                case (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.作废:
                    //状态不变动
                    break;
            }

        }
        #endregion

        #region 创建销售单相关数据

        /// <summary>
        /// 创建销售单
        /// 影响数据表:"收货(常用)地址","订单收货地址表","销售单","销售单明细","发票","销售单优惠券"
        ///             ,"惠源币日志","客户"的"惠源币","收款单"
        /// 支付状态:未支付（10）；销售单状态:待审核（10）；前台显示状态:待审核(到付销售单),待支付(预付销售单)
        /// </summary>
        /// <param name="orderCreatorSysNo">下单人编号</param>
        /// <param name="customerSysNo">会员编号</param>
        /// <param name="receiveAddress">订单收货地址</param>
        /// <param name="defaultWarehouseSysNo">默认仓库编号</param>
        /// <param name="deliveryTypeSysNo">配送方式编号</param>
        /// <param name="payTypeSysNo">支付方式编号</param>
        /// <param name="shoppingCart">购物车对象</param>
        /// <param name="experienceCoin">使用惠源币数量</param>
        /// <param name="invoice">发票</param>
        /// <param name="orderSource">下单来源</param>
        /// <param name="orderSourceSysNo">下单来源编号</param>
        /// <param name="salesType">销售方式</param>
        /// <param name="salesSysNo">销售方式编号</param>
        /// <param name="isHiddenToCustomer">此销售单对用户是否隐藏</param>
        /// <param name="customerMessage">客户留言</param>
        /// <param name="internalRemarks">对内备注</param>
        /// <param name="deliveryRemarks">配送备注</param>
        /// <param name="deliveryTime">配送时间段</param>
        /// <param name="contactBeforeDelivery">配送前是否联系</param>
        /// <param name="remarks">备注</param>
        /// <param name="changePriceItem">调价项 Key=购物车编号 value=调价值</param>
        /// <returns>订单对象</returns>
        /// <remarks>2013-09-09 吴文强 创建方法</remarks>
        /// <remarks>2013-09-23 黄志勇 修改</remarks>
        public SoOrder CreateOrderFromOuter(int orderCreatorSysNo, int customerSysNo,
                                   SoReceiveAddress receiveAddress,
                                   int? defaultWarehouseSysNo, int deliveryTypeSysNo, int payTypeSysNo,
                                   List<SoOrderItem> orderItemList, int experienceCoin, FnInvoice invoice,
                                   OrderStatus.销售单来源 orderSource, int? orderSourceSysNo,
                                   OrderStatus.销售方式 salesType, int? salesSysNo,
                                   OrderStatus.销售单对用户隐藏 isHiddenToCustomer, string customerMessage,
                                   string internalRemarks, string deliveryRemarks, string deliveryTime,
                                   OrderStatus.配送前是否联系 contactBeforeDelivery,
                                   string remarks, Dictionary<int, decimal> changePriceItem = null)
        {                          //,decimal FreightAmount

            var customer = CrCustomerBo.Instance.GetModel(customerSysNo);


            //1.判断配送方式是否支持支付方式
            if (OrderStatus.销售单来源.门店下单 != orderSource)//门店下单转快递不能通过，排除掉
            {
                if (BsDeliveryPaymentBo.Instance.GetBsDeliveryPaymentCount(payTypeSysNo, deliveryTypeSysNo) == 0)
                    throw new ArgumentException("配送方式和支付方式不匹配");
            }

            var currentTime = DateTime.Now;
            //订单主表基本信息
            var so = new SoOrder();

            so.ContactBeforeDelivery = (int)contactBeforeDelivery;
            so.CustomerSysNo = customerSysNo;
            so.LevelSysNo = customer.LevelSysNo;
            so.DeliveryTypeSysNo = deliveryTypeSysNo;
            so.PayTypeSysNo = payTypeSysNo;
            so.DeliveryRemarks = deliveryRemarks;
            so.DeliveryTime = deliveryTime;
            so.CustomerMessage = customerMessage;
            so.InternalRemarks = internalRemarks;
            so.CreateDate = currentTime;
            so.LastUpdateBy = orderCreatorSysNo;
            so.LastUpdateDate = currentTime;
            so.OrderCreatorSysNo = orderCreatorSysNo;
            so.OrderSource = (int)orderSource;
            so.IsHiddenToCustomer = (int)isHiddenToCustomer;
            if (orderSourceSysNo.HasValue) so.OrderSourceSysNo = orderSourceSysNo.Value;
            so.PayStatus = (int)OrderStatus.销售单支付状态.未支付;  //支付方式需要处理
            so.SalesType = (int)salesType;
            if (salesSysNo.HasValue) so.SalesSysNo = salesSysNo.Value;
            if (OrderStatus.销售单来源.门店下单 == orderSource && deliveryTypeSysNo != (int)Hyt.Model.SystemPredefined.DeliveryType.第三方快递)//排除门店下单转快递
            {
                so.Status = (int)OrderStatus.销售单状态.待创建出库单;
                so.AuditorSysNo = orderCreatorSysNo;
                so.AuditorDate = DateTime.Now;

            }
            else
                so.Status = (int)OrderStatus.销售单状态.待审核;
            so.SendStatus = (int)OrderStatus.销售单推送状态.未推送;
            var payType = PaymentTypeBo.Instance.GetPaymentTypeFromMemory(payTypeSysNo);
            so.OnlineStatus = payType.PaymentType == (int)BasicStatus.支付方式类型.到付 ? Constant.OlineStatusType.待审核 : Constant.OlineStatusType.待支付;
            //默认仓库
            if (defaultWarehouseSysNo == null || defaultWarehouseSysNo == default(int))
            {
                if (receiveAddress != null)
                {
                    var warehouse = Hyt.BLL.Warehouse.WhWarehouseBo.Instance.GetWhWareHouse(receiveAddress.AreaSysNo, null, so.DeliveryTypeSysNo, WarehouseStatus.仓库状态.启用).FirstOrDefault();
                    so.DefaultWarehouseSysNo = warehouse == null ? 0 : warehouse.SysNo;
                }
            }
            else
            {
                so.DefaultWarehouseSysNo = (int)defaultWarehouseSysNo;
            }
            //保存发票信息
            if (invoice != null)
            {
                IFnInvoiceDao.Instance.InsertEntity(invoice);
                so.InvoiceSysNo = invoice.SysNo;
            }
            //保存收货地址 非门店下单的订单收货地址不允许为空
            if (orderSource != OrderStatus.销售单来源.门店下单 && receiveAddress == null)
            {
                throw new Exception("收货地址不能为空！");
            }
            ISoReceiveAddressDao.Instance.InsertEntity(receiveAddress);
            so.ReceiveAddressSysNo = receiveAddress.SysNo;

            //保存订单
            ISoOrderDao.Instance.InsertEntity(so);
            so = SoOrderBo.Instance.GetEntity(so.SysNo);
            //调用获取销售单明细数据接口，并保存明细数据
            //List<SoOrderItem> orderItemList = ShoppingCartToOrderItem(customerSysNo, so.SysNo, so.TransactionSysNo, shoppingCart, changePriceItem);
            foreach (var orderItem in orderItemList)
            {
                so.ProductAmount += orderItem.SalesAmount;
                ISoOrderItemDao.Instance.Insert(orderItem);
                if (OrderStatus.销售单来源.门店下单 == orderSource && deliveryTypeSysNo != (int)Hyt.Model.SystemPredefined.DeliveryType.第三方快递)//排除门店下单转快递
                {
                    Hyt.BLL.Web.PdProductBo.Instance.UpdateProductSales(orderItem.ProductSysNo, orderItem.Quantity);//更新销量
                }
            }
            //更新销售单主表金额相关字段
            //so.ProductAmount = shoppingCart.ProductAmount;
            so.ProductDiscountAmount = 0;
            so.ProductChangeAmount = 0;//调价金额合计
            so.FreightDiscountAmount = 0;
            so.FreightChangeAmount = 0;
            so.OrderAmount = so.ProductAmount;
            so.FreightAmount = 0;
            so.TaxFee = 0;
            so.OrderDiscountAmount = 0;
            so.CouponAmount = 0;
            so.CoinPay = 0;
            so.CashPay = so.OrderAmount - so.CoinPay;
            if (so.CashPay < 0)
            {
                throw new Exception("支付金额不能小于零");
            }
            UpdateOrder(so); //更新订单 余勇 修改为调用业务层方法 ISoOrderDao.Instance.Update(so);

            if (invoice != null)//记录发票事物编号
            {
                invoice.InvoiceAmount = so.CashPay;
                invoice.TransactionSysNo = so.TransactionSysNo;
                //Hyt.DataAccess.Order.IFnInvoiceDao.Instance.UpdateEntity(invoice);
                Hyt.BLL.Order.SoOrderBo.Instance.UpdateOrderInvoice(invoice); //更新发票 余勇 修改 改为调用业务层方法
            }

            ////使用了优惠券
            //if (string.IsNullOrEmpty(shoppingCart.CouponCode) == false)
            //{
            //    var coupon = Hyt.BLL.Promotion.PromotionBo.Instance.GetCoupon(shoppingCart.CouponCode);
            //    if (coupon != null)
            //    {
            //        ISoOrderDao.Instance.InsertSoCoupon(new SoCoupon()
            //        {
            //            CouponSysNo = coupon.SysNo,
            //            OrderSysNo = so.SysNo
            //        });
            //    }
            //}



            //创建收款单
            Finance.FnReceiptVoucherBo.Instance.CreateReceiptVoucherByOrder(so);//创建订单收款单

            //插入订单池
            if (so.OrderSource != (int)OrderStatus.销售单来源.业务员补单 && so.OrderSource != (int)(int)OrderStatus.销售单来源.门店下单)//业务员补单不进行任务分配
            {
                int assignTo = 0;//指定下一个订单操作人
                if (so.OrderSource == (int)OrderStatus.销售单来源.客服下单) assignTo = orderCreatorSysNo;//客服下单 默认分配给自己
                if (assignTo > 0)//已经指定了分配人
                {
                    SyJobPoolPublishBo.Instance.OrderAuditBySysNo(so.SysNo, assignTo);
                    SyJobDispatcherBo.Instance.WriteJobLog(string.Format("{0}创建订单并给自己分配订单审核任务，销售单编号:{1}", SyUserBo.Instance.GetUserName(orderCreatorSysNo),
                       so.SysNo), so.SysNo, null, orderCreatorSysNo);
                }
                else//未指定，系统根据规则自动分配
                {
                    //写订单池记录
                    SyJobPoolPublishBo.Instance.OrderAuditBySysNo(so.SysNo);
                    SyJobDispatcherBo.Instance.WriteJobLog(string.Format("{0}创建订单并生成订单审核任务，销售单编号:{1}", SyUserBo.Instance.GetUserName(orderCreatorSysNo),
                      so.SysNo), so.SysNo, null, orderCreatorSysNo);

                }

            }
            //门店下单转快递 ,加入任务池
            else if (so.OrderSource == (int)OrderStatus.销售单来源.门店下单 && so.DeliveryTypeSysNo == (int)DeliveryType.第三方快递)
            {
                SyJobPoolPublishBo.Instance.OrderAuditBySysNo(so.SysNo);
                SyJobDispatcherBo.Instance.WriteJobLog(string.Format("{0}门店下单转快递生成订单审核任务，销售单编号:{1}", SyUserBo.Instance.GetUserName(orderCreatorSysNo),
                     so.SysNo), so.SysNo, null, orderCreatorSysNo);
            }
            var isSysUser = so.OrderSource == (int)OrderStatus.销售单来源.门店下单 ||
                            so.OrderSource == (int)OrderStatus.销售单来源.客服下单 ||
                            so.OrderSource == (int)OrderStatus.销售单来源.业务员下单 ||
                            so.OrderSource == (int)OrderStatus.销售单来源.业务员补单 ||
                            so.OrderSource == (int)OrderStatus.销售单来源.分销商升舱;
            if (isSysUser && orderCreatorSysNo > 0)//系统用户创建
            {
                var userName = SyUserBo.Instance.GetUserName(orderCreatorSysNo);
                WriteSoTransactionLog(so.TransactionSysNo
                               , string.Format(Constant.ORDER_TRANSACTIONLOG_CREATE, so.SysNo)
                               , userName);
            }
            else//会员创建
            {
                WriteSoTransactionLog(so.TransactionSysNo
                                , string.Format(Constant.ORDER_TRANSACTIONLOG_CREATE, so.SysNo)
                                , "会员:" + so.CustomerSysNo);
            }

            //更新促销使用信息
            //UpdateUsedPromtionInfo(orderItemList, shoppingCart);

            //0.检查参数传入是否正确

            //1.根据receiveAddress创建"订单收货地址表"

            //2.创建"销售单"

            //3.创建"销售单明细"

            //4.创建"发票"

            //5.创建"销售单优惠券"

            //6.创建"惠源币日志"
            //扣减"客户"中的"惠源币"

            //7.创建"收款单"
            return so;
        }


        /// <summary>
        /// 创建销售单
        /// 影响数据表:"收货(常用)地址","订单收货地址表","销售单","销售单明细","发票","销售单优惠券"
        ///             ,"惠源币日志","客户"的"惠源币","收款单"
        /// 支付状态:未支付（10）；销售单状态:待审核（10）；前台显示状态:待审核(到付销售单),待支付(预付销售单)
        /// </summary>
        /// <param name="orderCreatorSysNo">下单人编号</param>
        /// <param name="customerSysNo">会员编号</param>
        /// <param name="receiveAddress">订单收货地址</param>
        /// <param name="defaultWarehouseSysNo">默认仓库编号</param>
        /// <param name="deliveryTypeSysNo">配送方式编号</param>
        /// <param name="payTypeSysNo">支付方式编号</param>
        /// <param name="shoppingCart">购物车对象</param>
        /// <param name="experienceCoin">使用惠源币数量</param>
        /// <param name="invoice">发票</param>
        /// <param name="orderSource">下单来源</param>
        /// <param name="orderSourceSysNo">下单来源编号</param>
        /// <param name="salesType">销售方式</param>
        /// <param name="salesSysNo">销售方式编号</param>
        /// <param name="isHiddenToCustomer">此销售单对用户是否隐藏</param>
        /// <param name="customerMessage">客户留言</param>
        /// <param name="internalRemarks">对内备注</param>
        /// <param name="deliveryRemarks">配送备注</param>
        /// <param name="deliveryTime">配送时间段</param>
        /// <param name="contactBeforeDelivery">配送前是否联系</param>
        /// <param name="remarks">备注</param>
        /// <param name="changePriceItem">调价项 Key=购物车编号 value=调价值</param>
        /// <param name="balance">会员卡余额</param>
        /// <returns>订单对象</returns>
        /// <remarks>2013-09-09 吴文强 创建方法</remarks>
        /// <remarks>2013-09-23 黄志勇 修改</remarks>
        public SoOrder CreateOrder(int orderCreatorSysNo, int customerSysNo,
                                   SoReceiveAddress receiveAddress,
                                   int? defaultWarehouseSysNo, int deliveryTypeSysNo, int payTypeSysNo,
                                   CrShoppingCart shoppingCart, int experienceCoin, FnInvoice invoice,
                                   OrderStatus.销售单来源 orderSource, int? orderSourceSysNo,
                                   OrderStatus.销售方式 salesType, int? salesSysNo,
                                   OrderStatus.销售单对用户隐藏 isHiddenToCustomer, string customerMessage,
                                   string internalRemarks, string deliveryRemarks, string deliveryTime,
                                   OrderStatus.配送前是否联系 contactBeforeDelivery,
                                   string remarks, Dictionary<int, decimal> changePriceItem = null, List<SelectListItemNew> ListItemNew = null, int dealerSysNo = 0, decimal balance = 0m)
        {                          //,decimal FreightAmount

            var customer = CrCustomerBo.Instance.GetModel(customerSysNo);

            if (shoppingCart.ShoppingCartGroups.Count < 1)
                throw new ArgumentNullException("购物车中没有商品,会员编号:" + customerSysNo);
            /*王耀发 2015-09-07 注释*/
            //if (deliveryTypeSysNo == DeliveryType.自建物流 || deliveryTypeSysNo == DeliveryType.自提)
            //{ 
            //    throw new Exception("自建物流或自提，请传入下一级配送方式");
            //}

            //订单主表基本信息
            var so = new SoOrder();

            //1.判断配送方式是否支持支付方式
            if (OrderStatus.销售单来源.门店下单 != orderSource)//门店下单转快递不能通过，排除掉
            {
                so.OrderNo = Hyt.BLL.Basic.ReceiptNumberBo.Instance.GetOrderNo();
                if (BsDeliveryPaymentBo.Instance.GetBsDeliveryPaymentCount(payTypeSysNo, deliveryTypeSysNo) == 0)
                    throw new ArgumentException("配送方式和支付方式不匹配");
            }
            else
            {
                so.OrderNo = Hyt.BLL.Basic.ReceiptNumberBo.Instance.GetStoresOrderNo();
            }

            //2 默认仓库编号 有值 则必需有效
            int canPayMaxCoin = Hyt.BLL.LevelPoint.PointBo.Instance.SettleAccountsUseExperienceCoinQuantity(shoppingCart, customerSysNo);//最多能够使用的会员币 2014-01-22 朱成果
            if (experienceCoin > 0 && experienceCoin > canPayMaxCoin)
            {
                throw new ArgumentException("当前订单最多能够使用" + canPayMaxCoin + "个会员币支付.");
            }
            var currentTime = DateTime.Now;

            if (shoppingCart.GroupPromotions != null)
            {
                foreach (var promotion in shoppingCart.GroupPromotions)
                {
                    so.UsedPromotions += promotion.PromotionSysNo + ";";
                }
            }



            so.ContactBeforeDelivery = (int)contactBeforeDelivery;
            so.CustomerSysNo = customerSysNo;
            so.LevelSysNo = customer.LevelSysNo;
            so.DeliveryTypeSysNo = deliveryTypeSysNo;
            so.PayTypeSysNo = payTypeSysNo;
            so.DeliveryRemarks = deliveryRemarks;
            so.DeliveryTime = deliveryTime;
            so.CustomerMessage = customerMessage;
            so.InternalRemarks = internalRemarks;
            so.CreateDate = currentTime;
            so.LastUpdateBy = orderCreatorSysNo;
            so.LastUpdateDate = currentTime;
            so.OrderCreatorSysNo = orderCreatorSysNo;
            so.OrderSource = (int)orderSource;
            so.IsHiddenToCustomer = (int)isHiddenToCustomer;
            if (orderSourceSysNo.HasValue) so.OrderSourceSysNo = orderSourceSysNo.Value;
            so.PayStatus = (int)OrderStatus.销售单支付状态.未支付;  //支付方式需要处理
            so.SalesType = (int)salesType;
            if (salesSysNo.HasValue) so.SalesSysNo = salesSysNo.Value;
            if (OrderStatus.销售单来源.门店下单 == orderSource && deliveryTypeSysNo != (int)Hyt.Model.SystemPredefined.DeliveryType.第三方快递)//排除门店下单转快递
            {
                so.Status = (int)OrderStatus.销售单状态.待创建出库单;
                so.AuditorSysNo = orderCreatorSysNo;
                so.AuditorDate = DateTime.Now;

            }
            else
                so.Status = (int)OrderStatus.销售单状态.待审核;
            so.SendStatus = (int)OrderStatus.销售单推送状态.未推送;
            var payType = PaymentTypeBo.Instance.GetPaymentTypeFromMemory(payTypeSysNo);
            so.OnlineStatus = payType.PaymentType == (int)BasicStatus.支付方式类型.到付 ? Constant.OlineStatusType.待审核 : Constant.OlineStatusType.待支付;
            //默认仓库
            if (defaultWarehouseSysNo == null || defaultWarehouseSysNo == default(int))
            {
                if (receiveAddress != null)
                {
                    var warehouse = Hyt.BLL.Warehouse.WhWarehouseBo.Instance.GetWhWareHouse(receiveAddress.AreaSysNo, null, so.DeliveryTypeSysNo, WarehouseStatus.仓库状态.启用).FirstOrDefault();
                    so.DefaultWarehouseSysNo = warehouse == null ? 0 : warehouse.SysNo;
                }
            }
            else
            {
                so.DefaultWarehouseSysNo = (int)defaultWarehouseSysNo;
            }

            #region 设置订单所属经销商
            //so.DealerSysNo = 0;         
            //var dealerWhareHouseInfo = BLL.Distribution.DsDealerWharehouseBo.Instance.GetByWarehousSysNo((int)defaultWarehouseSysNo);
            //if (dealerWhareHouseInfo != null)
            //   so.DealerSysNo = dealerWhareHouseInfo.DealerSysNo; 

            so.DealerSysNo = dealerSysNo;
            #endregion

            //保存发票信息
            if (invoice != null)
            {
                IFnInvoiceDao.Instance.InsertEntity(invoice);
                so.InvoiceSysNo = invoice.SysNo;
            }
            //保存收货地址 非门店下单的订单收货地址不允许为空
            if (orderSource != OrderStatus.销售单来源.门店下单 && receiveAddress == null)
            {
                throw new Exception("收货地址不能为空！");
            }
            ISoReceiveAddressDao.Instance.InsertEntity(receiveAddress);
            so.ReceiveAddressSysNo = receiveAddress.SysNo;

            //保存订单
            ISoOrderDao.Instance.InsertEntity(so);
            so = SoOrderBo.Instance.GetEntity(so.SysNo);
            //调用获取销售单明细数据接口，并保存明细数据
            List<SoOrderItem> orderItemList = ShoppingCartToOrderItem(customerSysNo, so.SysNo, so.TransactionSysNo, shoppingCart, changePriceItem, ListItemNew);

            List<int> proList = new List<int>();
            foreach (var mod in orderItemList)
            {
                proList.Add(mod.ProductSysNo);
            }
       
            foreach (var orderItem in orderItemList)
            {
                if (ListItemNew != null)
                {
                    foreach (var item in ListItemNew)
                    {
                        if (int.Parse(item.Text) == orderItem.SysNo)
                        {
                            orderItem.SalesAmount = int.Parse(item.Pcs) * decimal.Parse(item.Value);
                            orderItem.SalesUnitPrice = decimal.Parse(item.Value);

                        }
                    }

                }
           
                ISoOrderItemDao.Instance.Insert(orderItem);
              
                if (OrderStatus.销售单来源.门店下单 == orderSource && deliveryTypeSysNo != (int)Hyt.Model.SystemPredefined.DeliveryType.第三方快递)//排除门店下单转快递
                {
                    Hyt.BLL.Web.PdProductBo.Instance.UpdateProductSales(orderItem.ProductSysNo, orderItem.Quantity);//更新销量
                }
            }
            //更新销售单主表金额相关字段
            //so.ProductAmount = shoppingCart.ProductAmount;
            so.ProductAmount = orderItemList.Sum(i => i.SalesAmount);
            so.ProductDiscountAmount = shoppingCart.ProductDiscountAmount;
            so.ProductChangeAmount = orderItemList.Sum(i => i.ChangeAmount);//调价金额合计
            so.FreightDiscountAmount = shoppingCart.FreightDiscountAmount;
            so.FreightChangeAmount = 0;
            //so.OrderAmount = shoppingCart.SettlementAmount;
            so.OrderAmount = so.ProductAmount + so.FreightAmount;
            so.FreightAmount = shoppingCart.FreightAmount;
            so.TaxFee = shoppingCart.TaxFee;
            so.OrderDiscountAmount = shoppingCart.SettlementDiscountAmount;
            so.CouponAmount = shoppingCart.CouponAmount;
            so.CoinPay = Hyt.BLL.LevelPoint.PointBo.Instance.ExperienceCoinToMoney(experienceCoin);
            so.BalancePay = balance;
            so.CashPay = so.OrderAmount - so.CoinPay - so.BalancePay;


            if (so.CashPay < 0)
            {
                throw new Exception("支付金额不能小于零");
            }

            so.ProductCatleValue = orderItemList.Sum(x => x.Catle);
            UpdateOrder(so); //更新订单 余勇 修改为调用业务层方法 ISoOrderDao.Instance.Update(so);

            if (invoice != null)//记录发票事物编号
            {
                invoice.InvoiceAmount = so.CashPay;
                invoice.TransactionSysNo = so.TransactionSysNo;
                //Hyt.DataAccess.Order.IFnInvoiceDao.Instance.UpdateEntity(invoice);
                Hyt.BLL.Order.SoOrderBo.Instance.UpdateOrderInvoice(invoice); //更新发票 余勇 修改 改为调用业务层方法
            }

            //使用了优惠券
            if (string.IsNullOrEmpty(shoppingCart.CouponCode) == false)
            {
                var coupon = Hyt.BLL.Promotion.PromotionBo.Instance.GetCoupon(shoppingCart.CouponCode);
                if (coupon != null)
                {
                    ISoOrderDao.Instance.InsertSoCoupon(new SoCoupon()
                    {
                        CouponSysNo = coupon.SysNo,
                        OrderSysNo = so.SysNo
                    });
                }
            }

            //使用会员币
            if (experienceCoin > 0)
            {
                PointBo.Instance.OrderDeductionExperienceCoin(customerSysNo, so.SysNo, experienceCoin, so.TransactionSysNo);
            }

            //使用会员卡余额
            if (balance > 0)
            {
                CrRechargeBo.Instance.OrderDeductionBalance(customerSysNo, so.SysNo, balance, so.TransactionSysNo);
            }
            //创建收款单
            Finance.FnReceiptVoucherBo.Instance.CreateReceiptVoucherByOrder(so);//创建订单收款单

            //插入订单池
            if (so.OrderSource != (int)OrderStatus.销售单来源.业务员补单 && so.OrderSource != (int)(int)OrderStatus.销售单来源.门店下单)//业务员补单不进行任务分配
            {
                int assignTo = 0;//指定下一个订单操作人
                if (so.OrderSource == (int)OrderStatus.销售单来源.客服下单) assignTo = orderCreatorSysNo;//客服下单 默认分配给自己
                if (assignTo > 0)//已经指定了分配人
                {
                    SyJobPoolPublishBo.Instance.OrderAuditBySysNo(so.SysNo, assignTo);
                    SyJobDispatcherBo.Instance.WriteJobLog(string.Format("{0}创建订单并给自己分配订单审核任务，销售单编号:{1}", SyUserBo.Instance.GetUserName(orderCreatorSysNo),
                       so.SysNo), so.SysNo, null, orderCreatorSysNo);
                }
                else//未指定，系统根据规则自动分配
                {
                    //写订单池记录
                    SyJobPoolPublishBo.Instance.OrderAuditBySysNo(so.SysNo);
                    SyJobDispatcherBo.Instance.WriteJobLog(string.Format("{0}创建订单并生成订单审核任务，销售单编号:{1}", SyUserBo.Instance.GetUserName(orderCreatorSysNo),
                      so.SysNo), so.SysNo, null, orderCreatorSysNo);

                }

            }
            //门店下单转快递 ,加入任务池
            else if (so.OrderSource == (int)OrderStatus.销售单来源.门店下单 && so.DeliveryTypeSysNo == (int)DeliveryType.第三方快递)
            {
                SyJobPoolPublishBo.Instance.OrderAuditBySysNo(so.SysNo);
                SyJobDispatcherBo.Instance.WriteJobLog(string.Format("{0}门店下单转快递生成订单审核任务，销售单编号:{1}", SyUserBo.Instance.GetUserName(orderCreatorSysNo),
                     so.SysNo), so.SysNo, null, orderCreatorSysNo);
            }
            var isSysUser = so.OrderSource == (int)OrderStatus.销售单来源.门店下单 ||
                            so.OrderSource == (int)OrderStatus.销售单来源.客服下单 ||
                            so.OrderSource == (int)OrderStatus.销售单来源.业务员下单 ||
                            so.OrderSource == (int)OrderStatus.销售单来源.业务员补单 ||
                            so.OrderSource == (int)OrderStatus.销售单来源.分销商升舱;
            if (isSysUser && orderCreatorSysNo > 0)//系统用户创建
            {
                var userName = SyUserBo.Instance.GetUserName(orderCreatorSysNo);
                WriteSoTransactionLog(so.TransactionSysNo
                               , string.Format(Constant.ORDER_TRANSACTIONLOG_CREATE, so.SysNo)
                               , userName);
            }
            else//会员创建
            {
                WriteSoTransactionLog(so.TransactionSysNo
                                , string.Format(Constant.ORDER_TRANSACTIONLOG_CREATE, so.SysNo)
                                , "会员:" + so.CustomerSysNo);
            }

            //更新促销使用信息
            UpdateUsedPromtionInfo(orderItemList, shoppingCart);

            //0.检查参数传入是否正确

            //1.根据receiveAddress创建"订单收货地址表"

            //2.创建"销售单"

            //3.创建"销售单明细"

            //4.创建"发票"

            //5.创建"销售单优惠券"

            //6.创建"惠源币日志"
            //扣减"客户"中的"惠源币"

            //7.创建"收款单"
            return so;
        }



        /// <summary>
        /// 购物车转销售单明细
        /// </summary>
        /// <param name="shoppingCart">购物车</param>
        /// <param name="changePriceItem">调价项 Key=购物车编号 value=调价值</param>
        /// <returns>销售单明细集合</returns>
        /// <remarks>2013-09-10 吴文强 创建</remarks>
        /// <remarks>2014-04-18 朱成果 修改</remarks>
        private List<SoOrderItem> ShoppingCartToOrderItem(int customerSysNo, int orderSysNo, string transactionSysNo, CrShoppingCart shoppingCart, Dictionary<int, decimal> changePriceItem = null, List<SelectListItemNew> ListItemNew = null)
        {
            var orderItems = new List<SoOrderItem>();
            var removeProductSysNo = new List<int>();

            var customer = CrCustomerBo.Instance.GetModel(customerSysNo);

            foreach (var cartGroup in shoppingCart.ShoppingCartGroups)
            {
                #region 添加商品
                //添加商品
                foreach (var cartItem in cartGroup.ShoppingCartItems)
                {
                    var groupName = string.Empty;
                    var usedPromotions = string.Empty;

                    if (cartItem.IsLock == (int)CustomerStatus.购物车是否锁定.是)
                    {
                        groupName = cartGroup.GroupPromotions != null && cartGroup.GroupPromotions.Count > 0
                                        ? cartGroup.GroupPromotions[0].Description
                                        : string.Empty;
                        usedPromotions = cartGroup.Promotions;
                    }
                    else
                    {
                        if (cartItem.Promotions != null)
                        {
                            var proms =
                                cartItem.Promotions.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries)
                                        .Select(t => Convert.ToInt32(t));
                            if (cartGroup.GroupPromotions != null)
                                usedPromotions = string.Join(";",
                                                             cartGroup.GroupPromotions.Where(
                                                                 t => proms.Contains(t.PromotionSysNo) && t.IsUsed)
                                                                      .Select(s => s.PromotionSysNo.ToString()));
                        }
                    }

                    var item = new SoOrderItem()
                    {
                        OrderSysNo = orderSysNo,
                        TransactionSysNo = transactionSysNo,
                        ProductSysNo = cartItem.ProductSysNo,
                        ProductName = cartItem.ProductName,
                        Quantity = cartItem.Quantity,
                        OriginalPrice = cartItem.OriginPrice,
                        SalesUnitPrice = cartItem.SalesUnitPrice,
                        SalesAmount = cartItem.SaleTotalAmount,
                        DiscountAmount = cartItem.DiscountAmount,
                        ChangeAmount = 0,
                        RealStockOutQuantity = 0,
                        ProductSalesType = cartItem.ProductSalesType,
                        ProductSalesTypeSysNo = cartItem.ProductSalesTypeSysNo,
                        GroupCode = cartItem.GroupCode,
                        GroupName = groupName,
                        UsedPromotions = usedPromotions,
                        Catle = cartItem.Catle,
                        UnitCatle = cartItem.UnitCatle,

                    };
                    //修改商品销售单价为调整单价
                    if (ListItemNew != null)//购物车明细存在单价信息
                    {
                        foreach (var items in ListItemNew)
                        {
                            if (int.Parse(items.Text) == cartItem.SysNo)
                            {
                                item.SalesUnitPrice = decimal.Parse(items.Value);//单价信息

                                item.SalesAmount = decimal.Parse(items.Value) * cartItem.Quantity;
                            }
                        }

                    }
                    //if (changePriceItem != null && changePriceItem.ContainsKey(cartItem.SysNo))//购物车明细存在调价信息
                    //{
                    //    item.ChangeAmount = changePriceItem[cartItem.SysNo];//调价信息
                    //}

                    orderItems.Add(item);
                    //需要从购物车中移除的系统编号
                    removeProductSysNo.Add(cartItem.ProductSysNo);
                }
                #endregion

                #region 添加商品组赠品
                //添加赠品
                if (cartGroup.GroupPromotions != null)
                {
                    foreach (var groupPromotion in cartGroup.GroupPromotions)
                    {
                        if (groupPromotion.UsedGiftProducts == null) continue;
                        foreach (var giftProduct in groupPromotion.UsedGiftProducts)
                        {
                            var product = PdProductBo.Instance.GetProduct(giftProduct.ProductSysNo);
                            if (product == null) continue;

                            //从购物车获取数据
                            orderItems.Add(new SoOrderItem()
                                {
                                    OrderSysNo = orderSysNo,
                                    TransactionSysNo = transactionSysNo,

                                    ProductSysNo = giftProduct.ProductSysNo,
                                    ProductName = product.ProductName,
                                    Quantity = 1,
                                    OriginalPrice = PdPriceBo.Instance.GetUserRankPrice(giftProduct.ProductSysNo, customer.LevelSysNo),
                                    SalesUnitPrice = giftProduct.PurchasePrice,
                                    SalesAmount = giftProduct.PurchasePrice,
                                    DiscountAmount = 0,
                                    ChangeAmount = 0,
                                    RealStockOutQuantity = 0,
                                    ProductSalesType = (int)CustomerStatus.商品销售类型.赠品,

                                    ProductSalesTypeSysNo = giftProduct.SysNo,
                                    GroupCode = string.Empty,
                                    GroupName = string.Empty,
                                    UsedPromotions = giftProduct.PromotionSysNo.ToString()
                                });

                            //需要从购物车中移除的系统编号
                            removeProductSysNo.Add(giftProduct.ProductSysNo);
                        }
                    }
                }
                #endregion
            }

            #region 添加订单赠品
            //添加赠品
            if (shoppingCart.GroupPromotions != null)
            {
                foreach (var groupPromotion in shoppingCart.GroupPromotions)
                {
                    if (groupPromotion.UsedGiftProducts == null) continue;
                    foreach (var giftProduct in groupPromotion.UsedGiftProducts)
                    {
                        var product = PdProductBo.Instance.GetProduct(giftProduct.ProductSysNo);
                        if (product == null) continue;

                        //从购物车获取数据
                        orderItems.Add(new SoOrderItem()
                            {
                                OrderSysNo = orderSysNo,
                                TransactionSysNo = transactionSysNo,

                                ProductSysNo = giftProduct.ProductSysNo,
                                ProductName = giftProduct.ProductName,
                                Quantity = 1,
                                OriginalPrice = PdPriceBo.Instance.GetUserRankPrice(giftProduct.ProductSysNo, customer.LevelSysNo),
                                SalesUnitPrice = giftProduct.PurchasePrice,
                                SalesAmount = giftProduct.PurchasePrice,
                                DiscountAmount = 0,
                                ChangeAmount = 0,
                                RealStockOutQuantity = 0,
                                ProductSalesType = (int)CustomerStatus.商品销售类型.赠品,

                                ProductSalesTypeSysNo = giftProduct.SysNo,
                                GroupCode = string.Empty,
                                GroupName = string.Empty,
                                UsedPromotions = giftProduct.PromotionSysNo.ToString()
                            });

                        //需要从购物车中移除的系统编号
                        removeProductSysNo.Add(giftProduct.ProductSysNo);
                    }
                }
            }
            #endregion

            //从购物车中移除的系统编号
            CrShoppingCartBo.Instance.RemoveByProductSysNo(customerSysNo, removeProductSysNo.ToArray());

            return orderItems;
        }

        /// <summary>
        /// 更新促销使用信息
        /// </summary>
        /// <param name="orderItems">销售单明细集合</param>
        /// <param name="shoppingCart">购物车对象</param>
        /// <remarks>2013-10-09 吴文强 创建</remarks>
        private void UpdateUsedPromtionInfo(List<SoOrderItem> orderItems, CrShoppingCart shoppingCart)
        {
            //使用了优惠券
            if (!string.IsNullOrEmpty(shoppingCart.CouponCode))
            {
                var coupon = PromotionBo.Instance.GetCoupon(shoppingCart.CouponCode);
                if (coupon.UseQuantity > coupon.UsedQuantity)
                {
                    //更新优惠券已使用数量
                    ISpCouponDao.Instance.UpdateUsedQuantity(coupon.CouponCode);
                }
            }

            var promotionKeys = new List<string>();

            #region 遍历商品使用情况
            foreach (var orderItem in orderItems)
            {
                if (string.IsNullOrEmpty(orderItem.UsedPromotions))
                {
                    continue;
                }
                var keys = string.Format("{0},{1}", orderItem.UsedPromotions, orderItem.GroupCode);
                if (promotionKeys.Contains(keys))
                {
                    continue;
                }
                promotionKeys.Add(keys);

                if (orderItem.ProductSalesType == (int)CustomerStatus.商品销售类型.赠品)
                {
                    //更新已销售赠品数
                    int promotionSysNo;
                    int.TryParse(orderItem.UsedPromotions, out promotionSysNo);
                    ISpPromotionGiftDao.Instance.UpdateUsedSaleQuantity(promotionSysNo, orderItem.ProductSysNo,
                                                                        orderItem.Quantity);
                }
                else
                {
                    foreach (var promotionSysNo in orderItem.UsedPromotions
                   .Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        int sysNo, groupCode;
                        int.TryParse(promotionSysNo, out sysNo);
                        int.TryParse(orderItem.GroupCode, out groupCode);

                        if (sysNo == Model.SystemPredefined.Promotion.组合套餐)
                        {
                            //更新组合套餐数量
                            ISpComboDao.Instance.UpdateSaleQuantity(groupCode, orderItem.Quantity);
                        }
                        else if (sysNo == Model.SystemPredefined.Promotion.团购)
                        {
                            //更新团购数量
                            IGsGroupShoppingDao.Instance.UpdateHaveQuantity(groupCode, orderItem.Quantity);
                        }
                        else
                        {
                            //更新促销使用数量
                            ISpPromotionDao.Instance.UpdateUsedQuantity(sysNo);
                        }
                    }
                }
            }
            #endregion

            #region 遍历购物车组促销使用情况

            if (shoppingCart.GroupPromotions != null)
            {
                foreach (var groupPromotion in shoppingCart.GroupPromotions)
                {
                    if (!groupPromotion.IsUsed)
                    {
                        continue;
                    }
                    var keys = string.Format("{0},{1}", groupPromotion.PromotionSysNo, string.Empty);
                    if (promotionKeys.Contains(keys))
                    {
                        continue;
                    }
                    //更新促销使用数量
                    ISpPromotionDao.Instance.UpdateUsedQuantity(groupPromotion.PromotionSysNo);
                }
            }
            #endregion
        }

        #endregion

        #region 获取订单详情

        /// <summary>
        /// 获取订单详情
        /// </summary>
        /// <param name="sysNo">订单系统编号</param>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <returns>订单详情对象</returns>
        /// <remarks>2013-09-10 沈强 创建</remarks>
        public OrderDetail GetOrderDetail(int sysNo, int customerSysNo)
        {
            //获取订单
            var soOrder = GetEntityNoCache(sysNo);

            #region 获取订单详细收货地址
            //获取订单收货地址
            var orderAddress = GetOrderReceiveAddress(soOrder.ReceiveAddressSysNo);
            //组装订单详细需要的收货地址
            var address = new SoAddress
                {
                    AreaName = BasicAreaBo.Instance.GetAreaFullName(orderAddress.AreaSysNo),
                    MobilePhoneNumber = orderAddress.MobilePhoneNumber,
                    Name = orderAddress.Name,
                    StreetAddress = orderAddress.StreetAddress
                };
            #endregion

            //获取配送方式
            var cbDeliveryType = Logistics.DeliveryTypeBo.Instance.GetDeliveryType(soOrder.DeliveryTypeSysNo);

            //获取支付方式
            var bsPaymentType = PaymentTypeBo.Instance.GetEntity(soOrder.PayTypeSysNo);

            #region 获取订单商品明细

            var soOrderItems = ISoOrderItemDao.Instance.GetOrderItemsByOrderSysNo(sysNo);
            var orderItems = new List<OrderItem>();
            foreach (var soOrderItem in soOrderItems)
            {
                var orderItem = new OrderItem
                    {
                        LevelPrice = soOrderItem.SalesUnitPrice - soOrderItem.DiscountAmount,
                        Price = soOrderItem.OriginalPrice,
                        ProductName = soOrderItem.ProductName,
                        ProductSysNo = soOrderItem.ProductSysNo,
                        Quantity = soOrderItem.Quantity,
                        Specification = "",
                        Thumbnail = Hyt.BLL.Web.ProductImageBo.Instance.GetProductImagePath(Hyt.BLL.Web.ProductThumbnailType.Image180, soOrderItem.ProductSysNo)
                    };
                orderItems.Add(orderItem);
            }

            #endregion

            var orderDetail = new OrderDetail
                {
                    ReceiveAddress = address,
                    CreateDate = soOrder.CreateDate,
                    PayStatus = soOrder.PayStatus,
                    SysNo = soOrder.SysNo,
                    ContactBeforeDelivery = soOrder.ContactBeforeDelivery,
                    DeliveryTime = soOrder.DeliveryTime,
                    DeliveryTypeName = cbDeliveryType.DeliveryTypeName,
                    FreightAmount = soOrder.FreightAmount,
                    OrderAmount = soOrder.OrderAmount,
                    PaymentName = bsPaymentType.PaymentName,
                    ProductAmount = soOrder.ProductAmount,
                    TotalDiscountAmount = soOrder.CouponAmount + soOrder.ProductDiscountAmount + soOrder.FreightDiscountAmount + soOrder.OrderDiscountAmount,
                    Products = orderItems,
                    OnlineStatus = soOrder.OnlineStatus,
                    Status = soOrder.Status
                };

            return orderDetail;
        }

        #endregion

        #region 销售单优惠券

        /// <summary>
        /// 根据订单号获取订单优惠券
        /// </summary>
        /// <param name="orderSysNo">订单号</param>
        /// <returns>订单优惠券集合</returns>
        /// <remarks>2013-09-13 吴文强 创建</remarks>
        public IList<SpCoupon> GetCouponByOrderSysNo(int orderSysNo)
        {
            return ISoOrderDao.Instance.GetCouponByOrderSysNo(orderSysNo);
        }

        /// <summary>
        /// 更新订单优惠券
        /// </summary>
        /// <param name="orderSysNo">订单号</param>
        /// <param name="couponCode">优惠券编号</param>
        /// <remarks>2013-11-18 朱成果 创建</remarks>
        public void UpdateOrderCoupon(int orderSysNo, string couponCode)
        {
            //删除订单所有的优惠券
            ISoOrderDao.Instance.DeleteSoCoupon(orderSysNo);
            //使用了优惠券
            if (string.IsNullOrEmpty(couponCode) == false)
            {
                var coupon = Hyt.BLL.Promotion.PromotionBo.Instance.GetCoupon(couponCode);
                if (coupon != null)
                {
                    ISoOrderDao.Instance.InsertSoCoupon(
                     new SoCoupon()
                    {
                        CouponSysNo = coupon.SysNo,
                        OrderSysNo = orderSysNo
                    }
                    );
                }
            }
        }
        #endregion

        /// <summary>
        /// 更新订单主表
        /// </summary>
        /// <param name="order">订单</param>
        /// <returns>true or false</returns>
        /// <remarks>2013-06-14 朱家宏 创建
        /// 2014-5-14 杨文兵 增加移除缓存代码
        /// </remarks>
        public bool UpdateOrder(SoOrder order)
        {
            var cacheKey = string.Format("CACHE_SOORDER_{0}", order.SysNo);
            var result = Hyt.DataAccess.Order.ISoOrderDao.Instance.Update(order);
            Hyt.Infrastructure.Memory.MemoryProvider.Default.Remove(cacheKey);
            return result;
        }

        /// <summary>
        /// 更新订单配送备注
        /// </summary>
        /// <param name="order">订单</param>
        /// <returns>true or false</returns>
        /// <remarks>2017-05-5 罗勤尧 创建
        /// </remarks>
        public int UpdateOrderDeliveryRemarks(int orderSysNo, string DeliveryRemarks)
        {
            var cacheKey = string.Format("CACHE_SOORDER_{0}", orderSysNo);
            var result = Hyt.DataAccess.Order.ISoOrderDao.Instance.UpdateOrderDeliveryRemarks(orderSysNo, DeliveryRemarks);
            Hyt.Infrastructure.Memory.MemoryProvider.Default.Remove(cacheKey);
            return result;
        }

        /// <summary>
        /// 更新订单支付方式
        /// </summary>
        /// <param name="soID">订单编号</param>
        /// <param name="payType">支付方式</param>
        /// <returns></returns>
        /// <remarks>2013-12-11 黄波 创建</remarks>
        public void UpdateOrderPayType(int soID, int payType)
        {
            Hyt.DataAccess.Order.ISoOrderDao.Instance.UpdateOrderPayType(soID, payType);

            var cacheKey = string.Format("CACHE_SOORDER_{0}", soID);
            Hyt.Infrastructure.Memory.MemoryProvider.Default.Remove(cacheKey);
        }

        /// <summary>
        /// 更新销售单状态值
        /// </summary>
        /// <param name="orderId">销售单编号</param>
        /// <param name="newStatus">状态值</param>
        /// <remarks>2014-04-14 余勇 创建</remarks>
        public void UpdateOrderStatus(int orderId, int newStatus)
        {
            Hyt.DataAccess.Order.ISoOrderDao.Instance.UpdateOrderStatus(orderId, newStatus);
            var cacheKey = string.Format("CACHE_SOORDER_{0}", orderId);
            Hyt.Infrastructure.Memory.MemoryProvider.Default.Remove(cacheKey);
        }

        #region 后台首页销售单统计

        /// <summary>
        /// 销售单统计
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="filter">分销商权限信息 2016-1-27 王耀发 创建</param>
        /// <returns>有效的销售单总数</returns>
        /// <remarks>2013-09-26 邵斌 创建</remarks>
        public CBDefaultPageCountInfo GetOrderTotalInfo(ParaIsDealerFilter filter)
        {

            //return CacheManager.Get(CacheKeys.Items.BackendIndexTotalInfo, DateTime.Now.AddMinutes(5), delegate
            //    {
            CBDefaultPageCountInfo information = new CBDefaultPageCountInfo();

            //当天开始时间结束时间
            var startTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00");
            var endTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59");

            //订单相关信息统计
            Hyt.DataAccess.Order.ISoOrderDao.Instance.GetOrderTotalInformation(DateTime.Now.AddYears(-90), DateTime.Now, filter,
                                                                               ref information);

            //当日订单统计信息
            Hyt.DataAccess.Order.ISoOrderDao.Instance.GetTodayOrderTotalInformation(startTime, endTime, filter,
                                                                                    ref information);

            //退换货相关信息统计
            Hyt.DataAccess.RMA.IRcReturnDao.Instance.GetRMATotalInformation(DateTime.Now.AddYears(-90), DateTime.Now, filter,
                                                                            ref information);

            //当然退换货相关信息统计
            Hyt.DataAccess.RMA.IRcReturnDao.Instance.GetTodayRMATotalInformation(startTime, endTime, filter,
                                                                                 ref information);

            return information;
            //});

        }

        #endregion

        #region 仓库库存
        /// <summary>
        /// 库存查询
        /// </summary>
        /// <param name="erpCode">商品编码</param>
        /// <param name="erpWarehouseSysNo">仓库编号</param>
        /// <returns></returns>
        /// <remarks>2016-8-8 杨浩 创建</remarks>
        public List<Hyt.Model.Inventory> GetInventory(string[] erpCode, string erpWarehouseSysNo)
        {
            var list = new List<Hyt.Model.Inventory>();
            //string oraganizationCode = "";
            if (!string.IsNullOrEmpty(erpWarehouseSysNo))
            {
                list = PdProductStockBo.Instance.GetInventory(erpCode, int.Parse(erpWarehouseSysNo)).ToList();
                //var wareHouse = Warehouse.WhWarehouseBo.Instance.GetWarehouseEntity(int.Parse(erpWarehouseSysNo));
                //var oraganization = Hyt.BLL.Basic.OrganizationBo.Instance.GetOrganization(wareHouse.SysNo);
                //if (oraganization != null)
                //    oraganizationCode = oraganization.Code;
                //var client = EasProviderFactory.CreateProvider();
                //var result = client.GetInventory(oraganizationCode, erpCode, wareHouse.ErpCode, wareHouse.SysNo);
                //if (result != null && result.Status && result.Data != null) list = result.Data.ToList();
            }
            return list;
        }


        /// <summary>
        /// ERP库存查询
        /// </summary>
        /// <param name="erpCode">商品编码</param>
        /// <param name="erpWarehouseSysNo">仓库编号</param>
        /// <returns></returns>
        /// <remarks>2016-8-8 杨浩 创建</remarks>
        public List<Extra.Erp.Model.Inventory> GetErpInventory(string[] erpCode, string erpWarehouseSysNo)
        {
            var list = new List<Extra.Erp.Model.Inventory>();
            string oraganizationCode = "";
            if (!string.IsNullOrEmpty(erpWarehouseSysNo))
            {
                 
                var wareHouse = Warehouse.WhWarehouseBo.Instance.GetWarehouseEntity(int.Parse(erpWarehouseSysNo));
                var oraganization = Hyt.BLL.Basic.OrganizationBo.Instance.GetOrganization(wareHouse.SysNo);
                if (oraganization != null)
                    oraganizationCode = oraganization.Code;
                int warehouseSysNo = Convert.ToInt32(erpWarehouseSysNo);
                if (warehouseSysNo == 59 || warehouseSysNo == 61 || warehouseSysNo == 4 || warehouseSysNo == 30)
                {
                    var client = XingYeProviderFactory.CreateProvider();
                    var result = client.GetInventory(oraganizationCode, erpCode, wareHouse.ErpCode, wareHouse.SysNo);
                    if (result != null && result.Status && result.Data != null) list = result.Data.ToList();
                }
                else
                {
                    var client = KisProviderFactory.CreateProvider();
                    var result = client.GetInventory(oraganizationCode, erpCode, wareHouse.ErpCode, wareHouse.SysNo);
                    if (result != null && result.Status && result.Data != null) list = result.Data.ToList();
                }
            }

            return list;
        }
        #endregion

        #region 增加产品销量
        /// <summary>
        /// 增加产品销量
        /// </summary>
        /// <param name="ordersNO">订单编号</param>
        /// <remarks>2013-11-8 朱成果 创建</remarks>
        public void UpdateOrderProductSales(int ordersNO)
        {
            var items = Hyt.DataAccess.Order.ISoOrderItemDao.Instance.GetOrderItemsByOrderSysNo(ordersNO);
            if (items != null)
            {
                foreach (SoOrderItem p in items)
                {
                    Hyt.BLL.Web.PdProductBo.Instance.UpdateProductSales(p.ProductSysNo, p.Quantity);
                }
            }
        }
        #endregion

        #region 清理预付款、未支付订单
        /// <summary>
        /// 预付款、未支付订单
        /// 1、必须是支付方式为预付的订单
        /// 2、订单支付状态为未支付且订单生成时间超过3个工作日 
        /// 3、超时未支付订单若已审核则需先取消审核然后作废订单
        /// </summary>
        /// <returns>返回未付款、未审核订单列表</returns>
        /// <param name="days">天</param>
        /// <remarks>2013-11-8 苟治国 创建</remarks>
        public void ClearOrder(int days)
        {
            string msg = string.Empty;
            //获取预付款、未支付订单
            var items = ISoOrderDao.Instance.GetClearList();
            if (items != null)
            {
                foreach (var soOrderItem in items)
                {
                    //在订单创建时间加3天
                    var orderCreateDate = soOrderItem.CreateDate.AddDays(days);
                    var nowDate = DateTime.Now;
                    //如果小于当前时间
                    if (orderCreateDate < nowDate)
                    {
                        //超时未支付订单若已审核则需先取消审核然后作废订单
                        if (soOrderItem.Status == (int)OrderStatus.销售单状态.待支付)
                        {
                            //取消审核
                            if (Hyt.BLL.Order.SoOrderBo.Instance.CancelAuditedOrder(soOrderItem.SysNo, 0))
                            {
                                //作废订单
                                CancelSoOrder(soOrderItem.SysNo, 0, OrderStatus.销售单作废人类型.后台用户, ref msg);
                            }
                        }
                        else
                        {
                            //作废订单
                            CancelSoOrder(soOrderItem.SysNo, 0, OrderStatus.销售单作废人类型.后台用户, ref msg);
                        }
                    }
                }
            }
        }
        #endregion

        #region 定时释放未支付订单的锁定库存
        /// <summary>
        ///定时释放未支付订单的锁定库存
        /// 1、必须是支付方式为预付的订单
        /// 2、订单支付状态为未支付且订单生成时间超过2个工作日 
        /// 3、超时未支付订单若已审核则需先取消审核
        /// </summary>
        /// <returns>返回未付款、未审核订单列表</returns>
        /// <param name="days">天</param>
        /// <remarks>2017-09-8 罗勤尧 创建</remarks>
        public void OrderStock(int days)
        {
            string msg = string.Empty;
            //获取预付款、未支付订单
            var items = ISoOrderDao.Instance.GetClearList();
            if (items != null)
            {
                foreach (var soOrderItem in items)
                {
                    //在订单创建时间加上天数
                    var orderCreateDate = soOrderItem.CreateDate.AddDays(days);
                    var nowDate = DateTime.Now;
                    //如果小于当前时间
                    if (orderCreateDate < nowDate)
                    {
                        //超时未支付订单若已审核则需先取消审核然后作废订单
                        if (soOrderItem.Status == (int)OrderStatus.销售单状态.待支付)
                        {
                            //取消审核
                            if (Hyt.BLL.Order.SoOrderBo.Instance.CancelAuditedOrder(soOrderItem.SysNo, 0))
                            {
                                //作废订单
                                CancelSoOrder(soOrderItem.SysNo, 0, OrderStatus.销售单作废人类型.后台用户, ref msg);
                            }
                        }
                        else
                        {
                            //作废订单
                            CancelSoOrder(soOrderItem.SysNo, 0, OrderStatus.销售单作废人类型.后台用户, ref msg);
                        }
                    }
                }
            }
        }
        #endregion
        /// <summary>
        /// 更新收货地址信息
        /// </summary>
        /// <param name="address">订单收货地址</param>
        /// <remarks>2014-05-14 朱家宏 创建</remarks>
        public void SaveSoReceiveAddress(SoReceiveAddress address)
        {
            ISoReceiveAddressDao.Instance.UpdateEntity(address);
            MemoryProvider.Default.Remove(string.Format(KeyConstant.OrderReceiveAddress, address.SysNo));
        }

        #region 获取订单图标标示
        /// <summary>
        /// 任务池查询获取商城图标
        /// </summary>
        /// <param name="sysNo">任务对象编号</param>
        /// <param name="taskType">任务对象类型</param>
        /// <returns>商城图标</returns>
        /// <remarks>2014-05-21 余勇 创建</remarks>
        public string GetMallType(int sysNo, int taskType = 0)
        {
            if (taskType > 0 && taskType != (int)SystemStatus.任务对象类型.客服订单审核 &&
                taskType != (int)SystemStatus.任务对象类型.客服订单提交出库)
                return "";
            var model = SoOrderBo.Instance.GetEntity(sysNo);
            if (model != null)
            {
                if (model.OrderSource != (int)OrderStatus.销售单来源.分销商升舱) return "";

                if (!string.IsNullOrEmpty(model.ImgFlag))
                {
                    return GetMallImg(model.ImgFlag);
                }
                else
                {
                    var orderTransactionSysNo = "'" + model.TransactionSysNo + "'";
                    var mallType = DsOrderBo.Instance.GetMallType(orderTransactionSysNo).FirstOrDefault();
                    if (mallType == null) return "";
                    return GetMallType(mallType.MallTypeSysNo);
                }
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 订单查询获取商城图标
        /// </summary>
        /// <param name="imgFlag">图片标识</param>
        /// <param name="type">商城类型</param>
        /// <returns>商城图标</returns>
        /// <remarks>2014-05-21 余勇 创建</remarks>
        public string GetMallImg(string imgFlag, int type = 0)
        {
            if (!string.IsNullOrEmpty(imgFlag))
            {
                return GetOrderImgByFlag(imgFlag);
            }
            else
            {
                return GetMallType(type);
            }
        }

        /// <summary>
        /// 通过标识获取订单图片
        /// </summary>
        /// <returns>订单图片</returns>
        /// <remarks>2014-05-21 余勇 创建</remarks>
        private string GetOrderImgByFlag(string flag)
        {
            var imgFlagConfig = MemoryProvider.Default.Get(string.Format(KeyConstant.MallTypeImgList), () => Hyt.BLL.Config.Config.Instance.GetOrderImgFlagConfig());
            var imgFlagOption =
                ((List<Hyt.Model.Common.ImgConfigOption>)imgFlagConfig.ImgConfig).Find(
                    o => o.ImgFlag.Trim() == flag.Trim());

            if (imgFlagOption != null)
            {
                return string.Format("<img src='{0}' title={1} />", imgFlagOption.ImgUrl, imgFlagOption.Descript);
            }
            return "";
        }

        /// <summary>
        /// 通过商城类型获取商城图标
        /// </summary>
        /// <param name="type">商城类型</param>
        /// <returns>商城图标</returns>
        /// <remarks>2014-05-21 余勇 创建
        /// 将类型名称用自定义类型代替 2014-07-09 余勇 修改
        /// </remarks>
        private string GetMallType(int type)
        {
            var flag = string.Empty;
            switch (type)
            {
                case (int)DistributionStatus.商城类型预定义.天猫商城:
                    flag = MallTypeFlag.天猫商城旗舰店; // "tmall";
                    break;
                case (int)DistributionStatus.商城类型预定义.淘宝分销:
                    flag = MallTypeFlag.淘宝分销; // "taobao";
                    break;
                case (int)DistributionStatus.商城类型预定义.拍拍网购:
                    flag = MallTypeFlag.拍拍网购; // "paipai";
                    break;
                case (int)DistributionStatus.商城类型预定义.亚马逊:
                    flag = MallTypeFlag.亚马逊; // "yamaxun";
                    break;
                case (int)DistributionStatus.商城类型预定义.百度众测:
                    flag = MallTypeFlag.百度众测; // "baidugongce";
                    break;
                case (int)DistributionStatus.商城类型预定义.一号店:
                    flag = MallTypeFlag.一号店; // "yihaodian";
                    break;
                case (int)DistributionStatus.商城类型预定义.国美在线:
                    flag = MallTypeFlag.国美在线; // "guomeizaixian";
                    break;
                case (int)DistributionStatus.商城类型预定义.百度微购:
                    flag = MallTypeFlag.百度微购; // "baiduweigou";
                    break;
                case (int)DistributionStatus.商城类型预定义.阿里巴巴经销批发:
                    flag = MallTypeFlag.阿里巴巴经销批发; // "alibaba";
                    break;
                case (int)DistributionStatus.商城类型预定义.京东商城:
                    flag = MallTypeFlag.京东商城; // "alibaba";
                    break;
                case (int)DistributionStatus.商城类型预定义.有赞:
                    flag = MallTypeFlag.有赞; // "youzan";
                    break;
                case (int)DistributionStatus.商城类型预定义.苏宁易购:
                    flag = MallTypeFlag.苏宁易购; // "suningyigou";
                    break;
                case (int)DistributionStatus.商城类型预定义.海拍客:
                    flag = MallTypeFlag.海拍客; // "HaiPaiKe";
                    break;

                case (int)DistributionStatus.商城类型预定义.海带网:
                    flag = MallTypeFlag.海带网; // "HaiDai";
                    break;
            }
            if (!string.IsNullOrWhiteSpace(flag))
            {
                return GetOrderImgByFlag(flag);
            }
            return "";
        }

        #endregion

        #region 获取订单升仓赠品列表
        /// <summary>
        /// 获取升仓赠品列表
        /// </summary>
        /// <param name="orderid">订单编号</param>
        /// <returns>升仓赠品列表</returns>
        /// <remarks>2014-07-03 朱成果 创建</remarks>
        public IList<CBSoOrderItem> GetMallGiftItems(int orderid)
        {
            return Hyt.DataAccess.Order.ISoOrderItemDao.Instance.GetMallGiftItems(orderid);
        }
        #endregion

        #region 保存淘宝升仓赠品
        /// <summary>
        /// 保存升舱赠品
        /// </summary>
        /// <param name="orderid">订单编号</param>
        /// <param name="transactionSysNo">事物编号</param>
        /// <param name="items">明细编号</param>
        /// <param name="user">用户</param>
        /// <returns></returns>
        /// <remarks>2014-07-03 朱成果 创建</remarks>
        public void SaveMallGift(int orderid, string transactionSysNo, List<CBSoOrderItem> items, SyUser user)
        {
            if (items == null)
            {
                items = new List<CBSoOrderItem>();
            }
            var lst = GetMallGiftItems(orderid);
            foreach (var p in items)
            {
                if (p.Quantity < 1 || p.ProductSysNo < 1)
                {
                    throw new HytException("商品数量有误");
                }
                p.SalesUnitPrice = 0;
                p.SalesAmount = 0;
                p.ProductSalesType = (int)CustomerStatus.商品销售类型.普通;
                p.GroupName = "淘宝赠品";
                p.OrderSysNo = orderid;
                var olditem = lst.FirstOrDefault(m => m.SysNo == p.SysNo);
                if (olditem != null)//存在商城明细
                {
                    if (olditem.Quantity != p.Quantity)//调整了数量
                    {
                        Hyt.DataAccess.Order.ISoOrderItemDao.Instance.UpdateOrderItemQuantity(olditem.SysNo, p.Quantity, 0);
                        var dsitem = Hyt.DataAccess.MallSeller.IDsOrderDao.Instance.GetDsOrderItemsByHytItems(p.SysNo);
                        if (dsitem != null)//老的升仓明细
                        {
                            dsitem.Quantity = p.Quantity;
                            Hyt.DataAccess.MallSeller.IDsOrderDao.Instance.UpdateOrderItem(dsitem);
                        }
                    }
                }
                else
                {
                    Hyt.BLL.MallSeller.DsOrderBo.Instance.InsertDsOrderItemByHytItems(p);
                }
            }
            var deletelst = lst.Where(m => !items.Any(y => y.SysNo == m.SysNo)).ToList();
            if (deletelst != null && deletelst.Count > 0)
            {
                deletelst.ForEach(m =>
                {
                    Hyt.BLL.MallSeller.DsOrderBo.Instance.DeleteDsOrderItemByHytItems(m.SysNo);

                });
            }
            SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "编辑升舱订单(" + orderid + ")赠品信息", LogStatus.系统日志目标类型.订单, orderid, null, null, user.SysNo);
        }
        #endregion

        /// <summary>
        /// 获得推送订单需要的参数信息
        /// </summary>
        /// <param name="orderSysNo"></param>
        /// <returns>2015-09-02 王耀发 创建</returns>
        public IList<SendSoOrderModel> GetSendSoOrderModelByOrderSysNo(int orderSysNo)
        {
            return Hyt.DataAccess.Order.ISoOrderItemDao.Instance.GetSendSoOrderModelByOrderSysNo(orderSysNo);
        }

        /// <summary>
        /// 获得推送订单需要的参数信息
        /// </summary>
        /// <param name="orderSysNo"></param>
        /// <returns>2015-09-02 王耀发 创建</returns>
        public SendSoOrderTitleModel GetSendSoOrderTitleModelByOrderSysNo(int orderSysNo)
        {
            return Hyt.DataAccess.Order.ISoOrderItemDao.Instance.GetSendSoOrderTitleModelByOrderSysNo(orderSysNo);
        }

        public Result InsertSendOrderReturn(SendOrderReturn model, SyUser user)
        {
            Result r = new Result();
            model.CreatedDate = DateTime.Now;
            model.CreatedBy = user.SysNo;
            model.LastUpdateBy = user.SysNo;
            model.LastUpdateDate = DateTime.Now;
            Hyt.DataAccess.Order.ISoOrderItemDao.Instance.InsertSendOrderReturn(model);
            r.StatusCode = model.SysNo;
            r.Status = true;
            return r;
        }

        public static void UpdateOrderStatus(int soOrderSysNo, OrderStatus.销售单状态 销售单状态)
        {
            throw new NotImplementedException();
        }

        public static void UpdateOrderStatusNew(int soOrderSysNo, int newStatus)
        {
            Hyt.DataAccess.Order.ISoOrderDao.Instance.UpdateOrderStatus(soOrderSysNo, newStatus);
        }
        /// <summary>
        /// 更新销售单发送状态
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="SendStatus"></param>
        /// <remarks>2015-09-17 王耀发  创建</remarks>
        public static void UpdateOrderSendStatus(int soOrderSysNo, int SendStatus)
        {
            Hyt.DataAccess.Order.ISoOrderDao.Instance.UpdateOrderSendStatus(soOrderSysNo, SendStatus);
        }
        /// <summary>
        /// 获取销售单实体信息
        /// </summary>
        /// <param name="soOrderSysNo">销售单编号</param>
        /// <returns>销售单实体</returns>
        /// <remarks>2015-10-16 杨云奕 添加</remarks>
        public SoOrderMods GetSoOrderMods(int soOrderSysNo)
        {
            return Hyt.DataAccess.Order.ISoOrderDao.Instance.GetSoOrderMods(soOrderSysNo);
        }
        /// <summary>
        /// 获取订单地址信息
        /// </summary>
        /// <param name="ReceiveAddressSysNo">订单地址编号</param>
        /// <returns></returns>
        /// <remarks>2015-10-16 杨云奕 添加</remarks>
        public SoReceiveAddressMod GetOrderReceiveAddress2(int ReceiveAddressSysNo)
        {
            return Hyt.DataAccess.Order.ISoOrderDao.Instance.GetOrderReceiveAddress2(ReceiveAddressSysNo);
        }

        #region 订单导出

        /// <summary>
        /// 订单导出
        /// </summary>
        /// <param name="orderSysNos"></param>
        /// <param name="userIp"></param>
        /// <param name="operatorSysno"></param>
        /// <remarks>2016-3-16 王耀发 创建</remarks>
        public void ExportOrders(List<int> orderSysNos, string userIp, int operatorSysno, string dateFormat = "yyyy-MM-dd HH:mm")
        {
            try
            {
                // 查询订单
                List<CBOutputSoOrders> exportOrders = BLL.Order.SoOrderBo.Instance.GetExportOrderList(orderSysNos);

                var fileName = string.Format("销售订单({0})", DateTime.Now.ToString("yyyyMMddHHmmss"));

                /*
                    序号
                    出货仓
                    销售日期
                    销售部门
                    收款账号
                    内部订单号
                    销售订单号
                    电商平台名称
                    电商平台备案号
                    电商客户名称
                    电商商户备案号
                    电商客户电话
                    订单人姓名
                    订单人证件类型
                    订单人证件号
                    订单人电话
                    订单日期
                    收件人姓名
                    收件人证件号
                    收件人地址
                    收件人电话
                    商品品名
                    商品货号
                    申报数量
                    申报单价
                    申报总价
                    调价金额
                    优惠劵金额 
                    运费
                    保价费
                    税款
                    毛重
                    净重
                    选用的快递公司
                    网址
                    备注
                    客户代号
                    平台编码
                    支付交易号
                    支付企业名称
                    商品单位
                    商品条形码
                 */

                //导出Excel，并设置表头列名
                Util.ExcelUtil.ExportSoOrders<CBOutputSoOrders>(exportOrders,
                                    new List<string> {  "序号", "店铺", "会员名", "订单号", "销售订单号", "订单人姓名", "订单人证件号", "订单人电话","销售日期", "下单时间", 
                        "付款时间","订单总额", "支付类型","订单状态", "支付状态","买家留言" , "收件人地址", "商品品名", "商品货号", "商品条形码", "申报数量", "申报单价", "申报总价","调价金额", "优惠劵金额","运费", 
                        "税款", "毛重", "净重", "交易号", "买家支付账号", "快递单号","商品单价"},
                                    fileName, dateFormat);
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "销售订单导出excel",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, null, userIp, operatorSysno);
            }
            catch (Exception ex)
            {

                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "销售订单导出excel",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, ex, userIp, operatorSysno);

            }
        }

        /// <summary>
        /// 获取需要导出的订单
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns></returns>
        /// <remarks>2016-3-16 王耀发 创建</remarks>
        public List<CBOutputSoOrders> GetExportOrderList(List<int> sysNos)
        {
            return DataAccess.Order.ISoOrderDao.Instance.GetExportOrderList(sysNos);
        }

        /// <summary>
        /// 订单导出
        /// </summary>
        /// <param name="orderSysNos"></param>
        /// <param name="userIp"></param>
        /// <param name="operatorSysno"></param>
        /// <remarks>2016-3-16 王耀发 创建</remarks>
        public void ExportOrdersByDoSearch(ParaOrderFilter filter, string userIp, int operatorSysno, string dateFormat = "yyyy-MM-dd HH:mm")
        {
            try
            {
                // 查询订单
                List<CBOutputSoOrders> exportOrders = BLL.Order.SoOrderBo.Instance.GetExportOrderListByDoSearch(filter);

                var fileName = string.Format("销售订单({0})", DateTime.Now.ToString("yyyyMMddHHmmss"));

                /*
                    序号
                    出货仓
                    销售日期
                    销售部门
                    收款账号
                    内部订单号
                    销售订单号
                    电商平台名称
                    电商平台备案号
                    电商客户名称
                    电商商户备案号
                    电商客户电话
                    订单人姓名
                    订单人证件类型
                    订单人证件号
                    订单人电话
                    订单日期
                    收件人姓名
                    收件人证件号
                    收件人地址
                    收件人电话
                    商品品名
                    商品货号
                    申报数量
                    申报单价
                    申报总价
                    调价金额
                    优惠劵金额
                    运费
                    保价费
                    税款
                    毛重
                    净重
                    选用的快递公司
                    网址
                    备注
                    客户代号
                    平台编码
                    支付交易号
                    支付企业名称
                    商品单位
                    商品条形码
                 */

                //导出Excel，并设置表头列名
                Util.ExcelUtil.ExportSoOrders<CBOutputSoOrders>(exportOrders,
                                    new List<string> { "序号", "店铺", "会员名", "订单号", "销售订单号", "订单人姓名", "订单人证件号", "订单人电话","销售日期", "下单时间", 
                        "付款时间","订单总额", "支付类型","订单状态", "支付状态","买家留言" , "收件人地址", "商品品名", "商品货号", "商品条形码", "申报数量", "申报单价", "申报总价","调价金额", "优惠劵金额","运费", 
                        "税款", "毛重", "净重", "交易号", "买家支付账号", "快递单号","商品单价"},
                                    fileName, dateFormat);
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "销售订单导出excel",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, null, userIp, operatorSysno);
            }
            catch (Exception ex)
            {

                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "销售订单导出excel",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, ex, userIp, operatorSysno);

            }
        }

        /// <summary>
        /// 获取需要导出的订单
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns></returns>
        /// <remarks>2016-3-16 王耀发 创建</remarks>
        public List<CBOutputSoOrders> GetExportOrderListByDoSearch(ParaOrderFilter filter)
        {
            return DataAccess.Order.ISoOrderDao.Instance.GetExportOrderListByDoSearch(filter);
        }

        /// <summary>
        /// 订单导出
        /// </summary>
        /// <param name="orderSysNos"></param>
        /// <param name="userIp"></param>
        /// <param name="operatorSysno"></param>
        /// <remarks>2016-3-16 王耀发 创建</remarks>
        public void ExportOrderListByDoComplexSearch(ParaOrderFilter filter, string userIp, int operatorSysno, string dateFormat = "yyyy-MM-dd HH:mm")
        {
            try
            {
                // 查询订单
                List<CBOutputSoOrders> exportOrders = BLL.Order.SoOrderBo.Instance.GetExportOrderListByDoComplexSearch(filter);

                var fileName = string.Format("销售订单({0})", DateTime.Now.ToString("yyyyMMddHHmmss"));

                /*
                    序号
                    出货仓
                    销售日期
                    销售部门
                    收款账号
                    内部订单号
                    销售订单号
                    电商平台名称
                    电商平台备案号
                    电商客户名称
                    电商商户备案号
                    电商客户电话
                    订单人姓名
                    订单人证件类型
                    订单人证件号
                    订单人电话
                    订单日期
                    收件人姓名
                    收件人证件号
                    收件人地址
                    收件人电话
                    商品品名
                    商品货号
                    申报数量
                    申报单价
                    申报总价
                    调价金额
                    优惠劵金额
                    运费
                    保价费
                    税款
                    毛重
                    净重
                    选用的快递公司
                    网址
                    备注
                    客户代号
                    平台编码
                    支付交易号
                    支付企业名称
                    商品单位
                    商品条形码
                 */

                //导出Excel，并设置表头列名
                Util.ExcelUtil.ExportSoOrders<CBOutputSoOrders>(exportOrders,
                                    new List<string> { "序号", "店铺", "会员名", "订单号", "销售订单号", "订单人姓名", "订单人证件号", "订单人电话","销售日期", "下单时间", 
                        "付款时间","订单总额", "支付类型","订单状态", "支付状态","买家留言" , "收件人地址", "商品品名", "商品货号", "商品条形码", "申报数量", "申报单价", "申报总价","调价金额", "优惠劵金额","运费", 
                        "税款", "毛重", "净重", "交易号", "买家支付账号", "快递单号","商品单价"},
                                    fileName, dateFormat);
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "销售订单导出excel",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, null, userIp, operatorSysno);
            }
            catch (Exception ex)
            {

                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "销售订单导出excel",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, ex, userIp, operatorSysno);

            }
        }

        /// <summary>
        /// 获取需要导出的订单
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns></returns>
        /// <remarks>2016-3-16 王耀发 创建</remarks>
        public List<CBOutputSoOrders> GetExportOrderListByDoComplexSearch(ParaOrderFilter filter)
        {
            return DataAccess.Order.ISoOrderDao.Instance.GetExportOrderListByDoComplexSearch(filter);
        }

        #region 导出订单表带商品图片（信营）
        /// <summary>
        /// 获取需要导出的订单（信营）
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns></returns>
        /// <remarks>2016-8-4 罗远康 复制添加导出图片</remarks>
        public void GetXXExportOrderList(ref Pager<CBXXOutputSoOrders> pager, List<int> sysNos)
        {
            DataAccess.Order.ISoOrderDao.Instance.GetXXExportOrderList(ref pager, sysNos);
        }
        /// <summary>
        /// 获取需要导出的订单（信营）
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns></returns>
        /// <remarks>2016-8-4 罗远康 复制添加导出图片</remarks>
        public void GetXXExportOrderListByDoSearch(ref Pager<CBXXOutputSoOrders> pager, ParaOrderFilter filter)
        {
            DataAccess.Order.ISoOrderDao.Instance.GetXXExportOrderListByDoSearch(ref pager, filter);
        }
        /// <summary>
        /// 获取需要导出的订单（信营）
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns></returns>
        /// <remarks>2016-8-4 罗远康 复制添加导出图片</remarks>
        public void GetXXExportOrderListByDoComplexSearch(ref Pager<CBXXOutputSoOrders> pager, ParaOrderFilter filter)
        {
            DataAccess.Order.ISoOrderDao.Instance.GetXXExportOrderListByDoComplexSearch(ref pager, filter);
        }
        /// <summary>
        /// 订单导出（信营）选择
        /// </summary>
        /// <param name="orderSysNos"></param>
        /// <param name="userIp"></param>
        /// <param name="operatorSysno"></param>
        /// <remarks>2016-8-4 罗远康 复制添加导出图片</remarks>
        /// <remarks>2017-5-3 罗勤尧 增加第三方标记和支付时间</remarks>
        public void ExportOrdersPic(List<int> orderSysNos, string userIp, int operatorSysno)
        {
            //try
            //{
            var pager = new Pager<CBXXOutputSoOrders>
            {
                CurrentPage = 1,
                PageSize = 999999999
            };
            // 查询订单
            //List<CBXXOutputSoOrders> exportOrders = BLL.Order.SoOrderBo.Instance.GetXXExportOrderList(orderSysNos);
            BLL.Order.SoOrderBo.Instance.GetXXExportOrderList(ref pager, orderSysNos);
            var fileName = string.Format("销售订单({0})", DateTime.Now.ToString("yyyyMMddHHmmss"));

            string dataFormat = "yyyy-MM-dd HH:mm:ss";
            string Filepath = Config.Config.Instance.GetAttachmentConfig().FileServer;
            IList<object> OrderList = new List<object>();

            string orderSysno = "";
            foreach (var x in pager.Rows)
            {
                if (orderSysno != x.订单号)
                {
                    string payDate = string.IsNullOrWhiteSpace(x.付款日期) ? x.第三方付款日期 : x.付款日期;
                    if (!string.IsNullOrWhiteSpace(payDate))
                        payDate = DateTime.Parse(payDate).ToString("yyyy/MM/dd HH:mm:ss");


                    OrderList.Add(new
                    {
                        订单日期 = DateTime.Parse(x.订单日期).ToString("yyyy/MM/dd HH:mm:ss"),
                        所属网店 = "自营信营线上",
                        交易状态 = x.订单状态,
                        本地订单号 = x.订单号,
                        前端显示订单号 = x.销售订单号,
                        交易号 = x.交易号,
                        支付方式 = x.支付方式,
                        订单人证件 = x.订单人证件号,
                        买家昵称 = x.店铺,
                        收货人名 = x.订单人姓名,
                        手机号码 = x.订单人电话,
                        国家 = "中国",
                        省 = x.收件人地址 == null || !x.收件人地址.Contains("|") ? "" : x.收件人地址.Split('|')[0].ToString(),
                        市 = x.收件人地址 == null || !x.收件人地址.Contains("|") ? "" : x.收件人地址.Split('|')[1].ToString(),
                        区县 = "",
                        收货地址 = x.收件人地址,
                        总_金额 = x.申报总价,
                        总_优惠金额 = "",
                        总_运费 = x.运费,
                        实收金额 = x.申报总价,
                        制单人 = "",
                        邮政编码 = "",
                        物流公司 = x.运送方式,
                        快递单号 = x.快递单号 == null ? "" : x.快递单号,
                        标记 = x.标记,
                        付款日期 = payDate,
                        订单来源 = "",
                        运送方式 = x.运送方式,
                        买家留言 = x.买家留言 == null ? "" : x.买家留言,
                        整单优惠金额 = "",
                        买家邮箱 = "",
                        税费 = x.税款,
                        毛重 = x.毛重,
                        净重 = x.净重,
                        商品代码 = x.商品货号,
                        商品名称 = x.商品品名,
                        辅助属性 = "",
                        单位 = x.单位,
                        数量 = x.申报数量,
                        单价 = x.申报单价,
                        单品_金额 = (Convert.ToDecimal(x.申报数量) * Convert.ToDecimal(x.申报单价)).ToString("0.00"),
                        单品_优惠金额 = "",
                        折后金额 = "",
                        单品_运费 = "",
                        商品条形 = x.商品条形码,
                        发货仓库 = x.发货仓库,
                        网上订单号 = "",
                        对内备注 = x.对内备注,
                        配送备注 = x.配送备注,
                        代理商 = x.代理商
                    });
                }
                else
                {
                    OrderList.Add(new
                    {
                        订单日期 = "",
                        所属网店 = "",
                        交易状态 = "",
                        本地订单号 = "",
                        前端显示订单号 = "",
                        交易号 = "",
                        支付方式 = "",
                        订单人证件 = "",
                        买家昵称 = "",
                        收货人名 = "",
                        手机号码 = "",
                        国家 = "",
                        省 = "",
                        市 = "",
                        区县 = "",
                        收货地址 = "",
                        总_金额 = "",
                        总_优惠金额 = "",
                        总_运费 = "",
                        实收金额 = "",
                        制单人 = "",
                        邮政编码 = "",
                        物流公司 = "",
                        快递单号 = "",
                        标记 = "",
                        付款日期 = "",
                        订单来源 = "",
                        运送方式 = "",
                        买家留言 = "",
                        整单优惠金额 = "",
                        买家邮箱 = "",
                        税费 = "",
                        毛重 = x.毛重,
                        净重 = x.净重,
                        商品代码 = x.商品货号,
                        商品名称 = x.商品品名,
                        辅助属性 = "",
                        单位 = x.单位,
                        数量 = x.申报数量,
                        单价 = x.申报单价,
                        单品_金额 = (Convert.ToDecimal(x.申报数量) * Convert.ToDecimal(x.申报单价)).ToString("0.00"),
                        单品_优惠金额 = "",
                        折后金额 = "",
                        单品_运费 = "",
                        商品条形 = x.商品条形码,
                        发货仓库 = x.发货仓库,
                        网上订单号 = "",
                        对内备注 = x.对内备注,
                        配送备注 = x.配送备注,
                        代理商 = ""
                    });
                }
                orderSysno = x.订单号;
            }
            var _OrderList = OrderList.Select(x => new
            {
                订单日期 = x.GetType().GetProperty("订单日期").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("订单日期").GetValue(x, null).ToString(),
                所属网店 = x.GetType().GetProperty("所属网店").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("所属网店").GetValue(x, null).ToString(),
                代理商 = x.GetType().GetProperty("代理商").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("代理商").GetValue(x, null).ToString(),
                交易状态 = x.GetType().GetProperty("交易状态").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("交易状态").GetValue(x, null).ToString(),
                本地订单号 = x.GetType().GetProperty("本地订单号").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("本地订单号").GetValue(x, null).ToString(),
                前端显示订单号 = x.GetType().GetProperty("前端显示订单号").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("前端显示订单号").GetValue(x, null).ToString(),
                交易号 = (x.GetType().GetProperty("交易号").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("交易号").GetValue(x, null).ToString()),
                支付方式 = x.GetType().GetProperty("支付方式").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("支付方式").GetValue(x, null).ToString(),
                订单人证件 = x.GetType().GetProperty("订单人证件").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("订单人证件").GetValue(x, null).ToString(),
                买家昵称 = x.GetType().GetProperty("买家昵称").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("买家昵称").GetValue(x, null).ToString(),
                收货人名 = x.GetType().GetProperty("收货人名").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("收货人名").GetValue(x, null).ToString(),
                手机号码 = x.GetType().GetProperty("手机号码").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("手机号码").GetValue(x, null).ToString(),
                国家 = x.GetType().GetProperty("国家").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("国家").GetValue(x, null).ToString(),
                省 = x.GetType().GetProperty("省").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("省").GetValue(x, null).ToString(),
                市 = x.GetType().GetProperty("市").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("市").GetValue(x, null).ToString(),
                区县 = x.GetType().GetProperty("区县").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("区县").GetValue(x, null).ToString(),
                收货地址 = x.GetType().GetProperty("收货地址").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("收货地址").GetValue(x, null).ToString(),
                总_金额 = x.GetType().GetProperty("总_金额").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("总_金额").GetValue(x, null).ToString(),
                总_优惠金额 = x.GetType().GetProperty("总_优惠金额").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("总_优惠金额").GetValue(x, null).ToString(),
                总_运费 = x.GetType().GetProperty("总_运费").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("总_运费").GetValue(x, null).ToString(),
                实收金额 = x.GetType().GetProperty("实收金额").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("实收金额").GetValue(x, null).ToString(),
                制单人 = x.GetType().GetProperty("制单人").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("制单人").GetValue(x, null).ToString(),
                邮政编码 = x.GetType().GetProperty("邮政编码").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("邮政编码").GetValue(x, null).ToString(),
                物流公司 = x.GetType().GetProperty("物流公司").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("物流公司").GetValue(x, null).ToString(),
                快递单号 = x.GetType().GetProperty("快递单号").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("快递单号").GetValue(x, null).ToString(),
                标记 = x.GetType().GetProperty("标记").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("标记").GetValue(x, null).ToString(),
                付款日期 = x.GetType().GetProperty("付款日期").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("付款日期").GetValue(x, null).ToString(),
                订单来源 = x.GetType().GetProperty("订单来源").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("订单来源").GetValue(x, null).ToString(),
                运送方式 = x.GetType().GetProperty("运送方式").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("运送方式").GetValue(x, null).ToString(),
                买家留言 = x.GetType().GetProperty("买家留言").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("买家留言").GetValue(x, null).ToString(),
                整单优惠金额 = x.GetType().GetProperty("整单优惠金额").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("整单优惠金额").GetValue(x, null).ToString(),
                买家邮箱 = x.GetType().GetProperty("买家邮箱").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("买家邮箱").GetValue(x, null).ToString(),
                税费 = x.GetType().GetProperty("税费").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("税费").GetValue(x, null).ToString(),
                毛重 = x.GetType().GetProperty("毛重").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("毛重").GetValue(x, null).ToString(),
                净重 = x.GetType().GetProperty("净重").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("净重").GetValue(x, null).ToString(),
                商品代码 = x.GetType().GetProperty("商品代码").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("商品代码").GetValue(x, null).ToString(),
                商品名称 = x.GetType().GetProperty("商品名称").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("商品名称").GetValue(x, null).ToString(),
                辅助属性 = x.GetType().GetProperty("辅助属性").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("辅助属性").GetValue(x, null).ToString(),
                单位 = x.GetType().GetProperty("单位").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("单位").GetValue(x, null).ToString(),
                数量 = x.GetType().GetProperty("数量").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("数量").GetValue(x, null).ToString(),
                单价 = x.GetType().GetProperty("单价").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("单价").GetValue(x, null).ToString(),
                单品_金额 = x.GetType().GetProperty("单品_金额").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("单品_金额").GetValue(x, null).ToString(),
                单品_优惠金额 = x.GetType().GetProperty("单品_优惠金额").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("单品_优惠金额").GetValue(x, null).ToString(),
                折后金额 = x.GetType().GetProperty("折后金额").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("折后金额").GetValue(x, null).ToString(),
                单品_运费 = x.GetType().GetProperty("单品_运费").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("单品_运费").GetValue(x, null).ToString(),
                商品条形 = x.GetType().GetProperty("商品条形").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("商品条形").GetValue(x, null).ToString(),
                发货仓库 = x.GetType().GetProperty("发货仓库").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("发货仓库").GetValue(x, null).ToString(),
                网上订单号 = x.GetType().GetProperty("网上订单号").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("网上订单号").GetValue(x, null).ToString(),
                对内备注 = x.GetType().GetProperty("对内备注").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("对内备注").GetValue(x, null).ToString(),
                配送备注 = x.GetType().GetProperty("配送备注").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("配送备注").GetValue(x, null).ToString(),
            }).ToList();
            /* var OrderList = pager.Rows.Select(x =>
             new
             {
                 订单日期 = x.销售日期,
                 所属网店 = "自营信营线上",
                 交易状态 = x.订单状态,
                 本地订单号 = x.订单号,
                 交易号 = x.交易号,
                 支付方式 = x.支付方式,
                 订单人证件 = x.订单人证件号,
                 买家昵称 = x.店铺,
                 收货人名 = x.订单人姓名,
                 手机号码 = x.订单人电话,
                 国家 = "中国",
                 省 = x.收件人地址.Split('-')[0].ToString(),
                 市 = x.收件人地址.Split('-')[1].ToString(),
                 区县 = "",
                 收货地址 = x.收件人地址.Split('-')[2].ToString(),
                 总_金额 = x.申报总价,
                 总_优惠金额 = "",
                 总_运费 = x.运费,
                 实收金额 = x.申报总价,
                 制单人 = "",
                 邮政编码 = "",
                 物流公司 = x.运送方式,
                 快递单号 = x.快递单号,
                 标记 = "",
                 付款日期 = x.付款日期,
                 订单来源 = "",
                 运送方式 = x.运送方式,
                 买家留言 = x.买家留言,
                 整单优惠金额 = "",
                 买家邮箱 = "",
                 税费 = x.税款,
                 毛重 = x.毛重,
                 净重 = x.净重,
                 商品代码 = x.商品货号,
                 商品名称 = x.商品品名,
                 辅助属性 = "",
                 单位 = x.单位,
                 数量 = x.申报数量,
                 单价 = x.申报单价,
                 单品_金额 = (Convert.ToDecimal(x.申报数量) * Convert.ToDecimal(x.申报单价)).ToString("0.00"),
                 单品_优惠金额 = "",
                 折后金额 = "",
                 单品_运费 = "",
                 商品条形 = x.商品条形码,
                 发货仓库 = x.发货仓库,
                 网上订单号 = ""
             }
             ).ToList(); */
            Util.ExcelUtil.ExportSoOrdersPic(_OrderList,
                new List<string> { "订单日期", "所属网店","代理商", "交易状态", "本地订单号","前端显示订单号", "交易号", "支付方式", "订单人证件", "买家昵称", "收货人名", 
                        "手机号码", "国家", "省" ,"市", "区/县", "收货地址", "金额", "优惠金额", "运费","实收金额", "制单人", "邮政编码","物流公司","快递单号", "标记", 
                        "付款日期", "订单来源", "运送方式", "买家留言", "整单优惠金额", "买家邮箱", "税费", "毛重", "净重", "商品代码", "商品名称", "辅助属性", "单位"
                        , "数量", "单价", "金额", "优惠金额", "折后金额", "运费", "商品条形", "发货仓库", "网上订单号","对内备注","配送备注"},
                 fileName, dataFormat, Filepath);
            //导出Excel，并设置表头列名
            //Util.ExcelUtil.ExportSoOrdersPic<CBXXOutputSoOrders>(exportOrders,
            //                    new List<string> { "序号", "店铺", "会员名", "订单号", "销售订单号", "订单人姓名", "订单人证件号", "订单人电话", "销售日期", 
            //        "订单日期", "订单状态", "买家留言" ,"对内备注", "收件人地址", "商品品名", "商品货号", "商品条形码", "商品图片","申报数量", "申报单价", "申报总价","调价金额","优惠劵金额", "运费", 
            //        "税款", "毛重", "净重", "交易号", "买家支付账号", "快递单号"},
            //                    fileName, dataFormat, Filepath);
            SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "销售订单导出excel",
                                     LogStatus.系统日志目标类型.ExcelExporting, 0, null, userIp, operatorSysno);
            //}
            //catch (Exception ex)
            //{

            //    SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "销售订单导出excel",
            //                             LogStatus.系统日志目标类型.ExcelExporting, 0, ex, userIp, operatorSysno);

            //}
        }

        /// <summary>
        /// 订单导出（信营）
        /// </summary>
        /// <param name="orderSysNos"></param>
        /// <param name="userIp"></param>
        /// <param name="operatorSysno"></param>
        /// <remarks>2016-8-4 罗远康 复制添加导出图片</remarks>
        public void ExportOrdersByDoSearchPic(ParaOrderFilter filter, string userIp, int operatorSysno)
        {
            try
            {
                var pager = new Pager<CBXXOutputSoOrders>
                {
                    CurrentPage = 1,
                    PageSize = 999999999
                };
                // 查询订单
                //List<CBXXOutputSoOrders> exportOrders = BLL.Order.SoOrderBo.Instance.GetXXExportOrderListByDoSearch(filter);
                BLL.Order.SoOrderBo.Instance.GetXXExportOrderListByDoSearch(ref pager, filter);
                var fileName = string.Format("销售订单({0})", DateTime.Now.ToString("yyyyMMddHHmmss"));

                string dataFormat = "yyyy-MM-dd HH:mm";
                string Filepath = Config.Config.Instance.GetAttachmentConfig().FileServer;
                IList<object> OrderList = new List<object>();

                string orderSysno = "";
                foreach (var x in pager.Rows)
                {
                    if (orderSysno != x.订单号)
                    {

                        string payDate = string.IsNullOrWhiteSpace(x.付款日期) ? x.第三方付款日期 : x.付款日期;
                        if (!string.IsNullOrWhiteSpace(payDate))
                            payDate = DateTime.Parse(payDate).ToString("yyyy/MM/dd HH:mm:ss");

                        OrderList.Add(new
                        {
                            订单日期 =DateTime.Parse(x.订单日期).ToString("yyyy/MM/dd HH:mm:ss"),
                            所属网店 = "自营信营线上",
                            交易状态 = x.订单状态,
                            本地订单号 = x.订单号,
                            前端显示订单号=x.销售订单号,
                            交易号 = x.交易号,
                            支付方式 = x.支付方式,
                            订单人证件 = x.订单人证件号,
                            买家昵称 = x.店铺,
                            收货人名 = x.订单人姓名,
                            手机号码 = x.订单人电话,
                            国家 = "中国",
                            省 = x.收件人地址 == null || !x.收件人地址.Contains("|") ? "" : x.收件人地址.Split('|')[0].ToString(),
                            市 = x.收件人地址 == null || !x.收件人地址.Contains("|") ? "" : x.收件人地址.Split('|')[1].ToString(),
                            区县 = "",
                            收货地址 = x.收件人地址,
                            总_金额 = x.申报总价,
                            总_优惠金额 = "",
                            总_运费 = x.运费,
                            实收金额 = x.申报总价,
                            制单人 = "",
                            邮政编码 = "",
                            物流公司 = x.运送方式,
                            快递单号 = x.快递单号 == null ? "" : x.快递单号,
                            标记 = x.标记,
                            付款日期 = payDate,
                            订单来源 = "",
                            运送方式 = x.运送方式,
                            买家留言 = x.买家留言 == null ? "" : x.买家留言,
                            整单优惠金额 = "",
                            买家邮箱 = "",
                            税费 = x.税款,
                            毛重 = x.毛重,
                            净重 = x.净重,
                            商品代码 = x.商品货号,
                            商品名称 = x.商品品名,
                            辅助属性 = "",
                            单位 = x.单位,
                            数量 = x.申报数量,
                            单价 = x.申报单价,
                            单品_金额 = (Convert.ToDecimal(x.申报数量) * Convert.ToDecimal(x.申报单价)).ToString("0.00"),
                            单品_优惠金额 = "",
                            折后金额 = "",
                            单品_运费 = "",
                            商品条形 = x.商品条形码,
                            发货仓库 = x.发货仓库,
                            网上订单号 = "",
                            对内备注 = x.对内备注,
                            配送备注 = x.配送备注,
                            代理商 = x.代理商
                        });
                    }
                    else
                    {
                        OrderList.Add(new
                        {
                            订单日期 = "",
                            所属网店 = "",
                            交易状态 = "",
                            本地订单号 = "",
                            前端显示订单号 ="",
                            交易号 = "",
                            支付方式 = "",
                            订单人证件 = "",
                            买家昵称 = "",
                            收货人名 = "",
                            手机号码 = "",
                            国家 = "",
                            省 = "",
                            市 = "",
                            区县 = "",
                            收货地址 = "",
                            总_金额 = "",
                            总_优惠金额 = "",
                            总_运费 = "",
                            实收金额 = "",
                            制单人 = "",
                            邮政编码 = "",
                            物流公司 = "",
                            快递单号 = "",
                            标记 = "",
                            付款日期 = "",
                            订单来源 = "",
                            运送方式 = "",
                            买家留言 = "",
                            整单优惠金额 = "",
                            买家邮箱 = "",
                            税费 = "",
                            毛重 = x.毛重,
                            净重 = x.净重,
                            商品代码 = x.商品货号,
                            商品名称 = x.商品品名,
                            辅助属性 = "",
                            单位 = x.单位,
                            数量 = x.申报数量,
                            单价 = x.申报单价,
                            单品_金额 = (Convert.ToDecimal(x.申报数量) * Convert.ToDecimal(x.申报单价)).ToString("0.00"),
                            单品_优惠金额 = "",
                            折后金额 = "",
                            单品_运费 = "",
                            商品条形 = x.商品条形码,
                            发货仓库 = x.发货仓库,
                            网上订单号 = "",
                            对内备注 = x.对内备注,
                            配送备注 = x.配送备注,
                            代理商 = ""
                        });
                    }
                    orderSysno = x.订单号;
                }
                var _OrderList = OrderList.Select(x => new
                {
                    订单日期 = x.GetType().GetProperty("订单日期").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("订单日期").GetValue(x, null).ToString(),
                    所属网店 = x.GetType().GetProperty("所属网店").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("所属网店").GetValue(x, null).ToString(),
                    代理商 = x.GetType().GetProperty("代理商").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("代理商").GetValue(x, null).ToString(),
                    交易状态 = x.GetType().GetProperty("交易状态").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("交易状态").GetValue(x, null).ToString(),
                    本地订单号 = x.GetType().GetProperty("本地订单号").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("本地订单号").GetValue(x, null).ToString(),
                    前端显示订单号 = x.GetType().GetProperty("前端显示订单号").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("前端显示订单号").GetValue(x, null).ToString(),
                    交易号 = (x.GetType().GetProperty("交易号").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("交易号").GetValue(x, null).ToString()),
                    支付方式 = x.GetType().GetProperty("支付方式").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("支付方式").GetValue(x, null).ToString(),
                    订单人证件 = x.GetType().GetProperty("订单人证件").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("订单人证件").GetValue(x, null).ToString(),
                    买家昵称 = x.GetType().GetProperty("买家昵称").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("买家昵称").GetValue(x, null).ToString(),
                    收货人名 = x.GetType().GetProperty("收货人名").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("收货人名").GetValue(x, null).ToString(),
                    手机号码 = x.GetType().GetProperty("手机号码").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("手机号码").GetValue(x, null).ToString(),
                    国家 = x.GetType().GetProperty("国家").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("国家").GetValue(x, null).ToString(),
                    省 = x.GetType().GetProperty("省").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("省").GetValue(x, null).ToString(),
                    市 = x.GetType().GetProperty("市").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("市").GetValue(x, null).ToString(),
                    区县 = x.GetType().GetProperty("区县").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("区县").GetValue(x, null).ToString(),
                    收货地址 = x.GetType().GetProperty("收货地址").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("收货地址").GetValue(x, null).ToString(),
                    总_金额 = x.GetType().GetProperty("总_金额").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("总_金额").GetValue(x, null).ToString(),
                    总_优惠金额 = x.GetType().GetProperty("总_优惠金额").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("总_优惠金额").GetValue(x, null).ToString(),
                    总_运费 = x.GetType().GetProperty("总_运费").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("总_运费").GetValue(x, null).ToString(),
                    实收金额 = x.GetType().GetProperty("实收金额").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("实收金额").GetValue(x, null).ToString(),
                    制单人 = x.GetType().GetProperty("制单人").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("制单人").GetValue(x, null).ToString(),
                    邮政编码 = x.GetType().GetProperty("邮政编码").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("邮政编码").GetValue(x, null).ToString(),
                    物流公司 = x.GetType().GetProperty("物流公司").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("物流公司").GetValue(x, null).ToString(),
                    快递单号 = x.GetType().GetProperty("快递单号").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("快递单号").GetValue(x, null).ToString(),
                    标记 = x.GetType().GetProperty("标记").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("标记").GetValue(x, null).ToString(),
                    付款日期 = x.GetType().GetProperty("付款日期").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("付款日期").GetValue(x, null).ToString(),
                    订单来源 = x.GetType().GetProperty("订单来源").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("订单来源").GetValue(x, null).ToString(),
                    运送方式 = x.GetType().GetProperty("运送方式").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("运送方式").GetValue(x, null).ToString(),
                    买家留言 = x.GetType().GetProperty("买家留言").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("买家留言").GetValue(x, null).ToString(),
                    整单优惠金额 = x.GetType().GetProperty("整单优惠金额").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("整单优惠金额").GetValue(x, null).ToString(),
                    买家邮箱 = x.GetType().GetProperty("买家邮箱").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("买家邮箱").GetValue(x, null).ToString(),
                    税费 = x.GetType().GetProperty("税费").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("税费").GetValue(x, null).ToString(),
                    毛重 = x.GetType().GetProperty("毛重").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("毛重").GetValue(x, null).ToString(),
                    净重 = x.GetType().GetProperty("净重").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("净重").GetValue(x, null).ToString(),
                    商品代码 = x.GetType().GetProperty("商品代码").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("商品代码").GetValue(x, null).ToString(),
                    商品名称 = x.GetType().GetProperty("商品名称").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("商品名称").GetValue(x, null).ToString(),
                    辅助属性 = x.GetType().GetProperty("辅助属性").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("辅助属性").GetValue(x, null).ToString(),
                    单位 = x.GetType().GetProperty("单位").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("单位").GetValue(x, null).ToString(),
                    数量 = x.GetType().GetProperty("数量").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("数量").GetValue(x, null).ToString(),
                    单价 = x.GetType().GetProperty("单价").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("单价").GetValue(x, null).ToString(),
                    单品_金额 = x.GetType().GetProperty("单品_金额").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("单品_金额").GetValue(x, null).ToString(),
                    单品_优惠金额 = x.GetType().GetProperty("单品_优惠金额").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("单品_优惠金额").GetValue(x, null).ToString(),
                    折后金额 = x.GetType().GetProperty("折后金额").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("折后金额").GetValue(x, null).ToString(),
                    单品_运费 = x.GetType().GetProperty("单品_运费").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("单品_运费").GetValue(x, null).ToString(),
                    商品条形 = x.GetType().GetProperty("商品条形").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("商品条形").GetValue(x, null).ToString(),
                    发货仓库 = x.GetType().GetProperty("发货仓库").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("发货仓库").GetValue(x, null).ToString(),
                    网上订单号 = x.GetType().GetProperty("网上订单号").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("网上订单号").GetValue(x, null).ToString(),
                    对内备注 = x.GetType().GetProperty("对内备注").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("对内备注").GetValue(x, null).ToString(),
                    配送备注 = x.GetType().GetProperty("配送备注").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("配送备注").GetValue(x, null).ToString(),
                }).ToList();
                //var OrderList = pager.Rows.Select(x =>
                //new
                //{
                //    订单日期 = x.销售日期,
                //    所属网店 = "自营信营线上",
                //    交易状态 = x.订单状态,
                //    本地订单号 = x.订单号,
                //    交易号 = x.交易号,
                //    支付方式 = x.支付方式,
                //    订单人证件 = x.订单人证件号,
                //    买家昵称 = x.店铺,
                //    收货人名 = x.订单人姓名,
                //    手机号码 = x.订单人电话,
                //    国家 = "中国",
                //    省 = x.收件人地址.Split('-')[0].ToString(),
                //    市 = x.收件人地址.Split('-')[1].ToString(),
                //    区县 = "",
                //    收货地址 = x.收件人地址.Split('-')[2].ToString(),
                //    总_金额 = x.申报总价,
                //    总_优惠金额 = "",
                //    总_运费 = x.运费,
                //    实收金额 = x.申报总价,
                //    制单人 = "",
                //    邮政编码 = "",
                //    物流公司 = x.运送方式,
                //    快递单号 = x.快递单号,
                //    标记 = "",
                //    付款日期 = x.付款日期,
                //    订单来源 = "",
                //    运送方式 = x.运送方式,
                //    买家留言 = x.买家留言,
                //    整单优惠金额 = "",
                //    买家邮箱 = "",
                //    税费 = x.税款,
                //    毛重 = x.毛重,
                //    净重 = x.净重,
                //    商品代码 = x.商品货号,
                //    商品名称 = x.商品品名,
                //    辅助属性 = "",
                //    单位 = x.单位,
                //    数量 = x.申报数量,
                //    单价 = x.申报单价,
                //    单品_金额 = (Convert.ToDecimal(x.申报数量) * Convert.ToDecimal(x.申报单价)).ToString("0.00"),
                //    单品_优惠金额 = "",
                //    折后金额 = "",
                //    单品_运费 = "",
                //    商品条形 = x.商品条形码,
                //    发货仓库 = x.发货仓库,
                //    网上订单号 = ""
                //}
                //).ToList(); 
                Util.ExcelUtil.ExportSoOrdersPic(_OrderList,
                    new List<string> { "订单日期", "所属网店","代理商", "交易状态", "本地订单号","前端显示订单号", "交易号", "支付方式", "订单人证件", "买家昵称", "收货人名", 
                        "手机号码", "国家", "省" ,"市", "区/县", "收货地址", "金额", "优惠金额", "运费","实收金额", "制单人", "邮政编码","物流公司","快递单号", "标记", 
                        "付款日期", "订单来源", "运送方式", "买家留言", "整单优惠金额", "买家邮箱", "税费", "毛重", "净重", "商品代码", "商品名称", "辅助属性", "单位"
                        , "数量", "单价", "金额", "优惠金额", "折后金额", "运费", "商品条形", "发货仓库", "网上订单号","对内备注","配送备注"},
                     fileName, dataFormat, Filepath);
                //导出Excel，并设置表头列名
                //Util.ExcelUtil.ExportSoOrdersPic<CBXXOutputSoOrders>(exportOrders,
                //                    new List<string> { "序号", "店铺", "会员名", "订单号", "销售订单号", "订单人姓名", "订单人证件号", "订单人电话", "销售日期", 
                //        "订单日期", "订单状态", "买家留言" ,"对内备注" , "收件人地址", "商品品名", "商品货号", "商品条形码", "商品图片","申报数量", "申报单价", "申报总价","调价金额","优惠劵金额", "运费", 
                //        "税款", "毛重", "净重", "交易号", "买家支付账号", "快递单号"},
                //                    fileName, dataFormat, Filepath);
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "销售订单导出excel",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, null, userIp, operatorSysno);
            }
            catch (Exception ex)
            {

                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "销售订单导出excel",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, ex, userIp, operatorSysno);

            }
        }

        /// <summary>
        /// 订单导出（信营）
        /// </summary>
        /// <param name="orderSysNos"></param>
        /// <param name="userIp"></param>
        /// <param name="operatorSysno"></param>
        /// <remarks>2016-8-4 罗远康 复制添加导出图片</remarks>
        /// <remarks>2017-5-3 罗勤尧 增加第三方标记和支付时间</remarks>
        public void ExportOrderListByDoComplexSearchPic(ParaOrderFilter filter, string userIp, int operatorSysno)
        {
            try
            {
                var pager = new Pager<CBXXOutputSoOrders>
                {
                    CurrentPage = 1,
                    PageSize = 999999999
                };
                // 查询订单
                //List<CBXXOutputSoOrders> exportOrders = BLL.Order.SoOrderBo.Instance.GetXXExportOrderListByDoComplexSearch(filter);
                BLL.Order.SoOrderBo.Instance.GetXXExportOrderListByDoComplexSearch(ref pager, filter);
                var fileName = string.Format("销售订单({0})", DateTime.Now.ToString("yyyyMMddHHmmss"));

                string dataFormat = "yyyy-MM-dd HH:mm";
                string Filepath = Config.Config.Instance.GetAttachmentConfig().FileServer;
                IList<object> OrderList = new List<object>();

                string orderSysno = "";
                foreach (var x in pager.Rows)
                {
                    if (orderSysno != x.订单号)
                    {

                        string payDate = string.IsNullOrWhiteSpace(x.付款日期) ? x.第三方付款日期 : x.付款日期;
                        if (!string.IsNullOrWhiteSpace(payDate))
                            payDate = DateTime.Parse(payDate).ToString("yyyy/MM/dd HH:mm:ss");


                        OrderList.Add(new
                        {
                            订单日期 = DateTime.Parse(x.订单日期).ToString("yyyy/MM/dd HH:mm:ss"),
                            所属网店 = "自营信营线上",
                            交易状态 = x.订单状态,
                            本地订单号 = x.订单号 == null ? "" : x.订单号,
                            前端显示订单号 = x.销售订单号,
                            交易号 = x.交易号 ?? "",
                            支付方式 = x.支付方式,
                            订单人证件 = x.订单人证件号 == null ? "" : x.订单人证件号,
                            买家昵称 = x.店铺 == null ? "" : x.店铺,
                            收货人名 = x.订单人姓名,
                            手机号码 = x.订单人电话,
                            国家 = "中国",
                            省 = x.收件人地址 == null || !x.收件人地址.Contains("|") ? "" : x.收件人地址.Split('|')[0].ToString(),
                            市 = x.收件人地址 == null || !x.收件人地址.Contains("|") ? "" : x.收件人地址.Split('|')[1].ToString(),
                            区县 = "",
                            收货地址 = x.收件人地址,
                            总_金额 = x.申报总价,
                            总_优惠金额 = "",
                            总_运费 = x.运费,
                            实收金额 = x.申报总价,
                            制单人 = "",
                            邮政编码 = "",
                            物流公司 = x.运送方式 == null ? "" : x.运送方式,
                            快递单号 = x.快递单号 == null ? "" : x.快递单号,
                            标记 = x.标记 == null ? "" : x.标记,
                            付款日期 = payDate,
                            订单来源 = "",
                            运送方式 = x.运送方式,
                            买家留言 = x.买家留言 == null ? "" : x.买家留言,
                            整单优惠金额 = "",
                            买家邮箱 = "",
                            税费 = x.税款,
                            毛重 = x.毛重,
                            净重 = x.净重,
                            商品代码 = x.商品货号,
                            商品名称 = x.商品品名,
                            辅助属性 = "",
                            单位 = x.单位,
                            数量 = x.申报数量,
                            单价 = x.申报单价,
                            单品_金额 = (Convert.ToDecimal(x.申报数量) * Convert.ToDecimal(x.申报单价)).ToString("0.00"),
                            单品_优惠金额 = "",
                            折后金额 = "",
                            单品_运费 = "",
                            商品条形 = x.商品条形码,
                            发货仓库 = x.发货仓库,
                            网上订单号 = "",
                            对内备注 = x.对内备注,
                            配送备注 = x.配送备注,
                            代理商 = x.代理商 == null ? "0" : x.代理商
                        });
                    }
                    else
                    {
                        OrderList.Add(new
                        {
                            订单日期 = "",
                            所属网店 = "",
                            交易状态 = "",
                            本地订单号 = "",
                            前端显示订单号 = "",
                            交易号 = "",
                            支付方式 = "",
                            订单人证件 = "",
                            买家昵称 = "",
                            收货人名 = "",
                            手机号码 = "",
                            国家 = "",
                            省 = "",
                            市 = "",
                            区县 = "",
                            收货地址 = "",
                            总_金额 = "",
                            总_优惠金额 = "",
                            总_运费 = "",
                            实收金额 = "",
                            制单人 = "",
                            邮政编码 = "",
                            物流公司 = "",
                            快递单号 = "",
                            标记 = "",
                            付款日期 = "",
                            订单来源 = "",
                            运送方式 = "",
                            买家留言 = "",
                            整单优惠金额 = "",
                            买家邮箱 = "",
                            税费 = "",
                            毛重 = x.毛重,
                            净重 = x.净重,
                            商品代码 = x.商品货号,
                            商品名称 = x.商品品名,
                            辅助属性 = "",
                            单位 = x.单位,
                            数量 = x.申报数量,
                            单价 = x.申报单价,
                            单品_金额 = (Convert.ToDecimal(x.申报数量) * Convert.ToDecimal(x.申报单价)).ToString("0.00"),
                            单品_优惠金额 = "",
                            折后金额 = "",
                            单品_运费 = "",
                            商品条形 = x.商品条形码,
                            发货仓库 = x.发货仓库,
                            网上订单号 = "",
                            对内备注 = x.对内备注,
                            配送备注 = x.配送备注,
                            代理商 = ""
                        });
                    }
                    orderSysno = x.订单号;
                }
                var _OrderList = OrderList.Select(x => new
                {
                    订单日期 = x.GetType().GetProperty("订单日期").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("订单日期").GetValue(x, null).ToString(),
                    所属网店 = x.GetType().GetProperty("所属网店").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("所属网店").GetValue(x, null).ToString(),
                    代理商 = x.GetType().GetProperty("代理商").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("代理商").GetValue(x, null).ToString(),
                    交易状态 = x.GetType().GetProperty("交易状态").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("交易状态").GetValue(x, null).ToString(),
                    本地订单号 = x.GetType().GetProperty("本地订单号").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("本地订单号").GetValue(x, null).ToString(),
                    前端显示订单号 = x.GetType().GetProperty("前端显示订单号").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("前端显示订单号").GetValue(x, null).ToString(),
                    交易号 = (x.GetType().GetProperty("交易号").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("交易号").GetValue(x, null).ToString()),
                    支付方式 = x.GetType().GetProperty("支付方式").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("支付方式").GetValue(x, null).ToString(),
                    订单人证件 = x.GetType().GetProperty("订单人证件").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("订单人证件").GetValue(x, null).ToString(),
                    买家昵称 = x.GetType().GetProperty("买家昵称").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("买家昵称").GetValue(x, null).ToString(),
                    收货人名 = x.GetType().GetProperty("收货人名").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("收货人名").GetValue(x, null).ToString(),
                    手机号码 = x.GetType().GetProperty("手机号码").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("手机号码").GetValue(x, null).ToString(),
                    国家 = x.GetType().GetProperty("国家").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("国家").GetValue(x, null).ToString(),
                    省 = x.GetType().GetProperty("省").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("省").GetValue(x, null).ToString(),
                    市 = x.GetType().GetProperty("市").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("市").GetValue(x, null).ToString(),
                    区县 = x.GetType().GetProperty("区县").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("区县").GetValue(x, null).ToString(),
                    收货地址 = x.GetType().GetProperty("收货地址").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("收货地址").GetValue(x, null).ToString(),
                    总_金额 = x.GetType().GetProperty("总_金额").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("总_金额").GetValue(x, null).ToString(),
                    总_优惠金额 = x.GetType().GetProperty("总_优惠金额").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("总_优惠金额").GetValue(x, null).ToString(),
                    总_运费 = x.GetType().GetProperty("总_运费").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("总_运费").GetValue(x, null).ToString(),
                    实收金额 = x.GetType().GetProperty("实收金额").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("实收金额").GetValue(x, null).ToString(),
                    制单人 = x.GetType().GetProperty("制单人").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("制单人").GetValue(x, null).ToString(),
                    邮政编码 = x.GetType().GetProperty("邮政编码").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("邮政编码").GetValue(x, null).ToString(),
                    物流公司 = x.GetType().GetProperty("物流公司").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("物流公司").GetValue(x, null).ToString(),
                    快递单号 = x.GetType().GetProperty("快递单号").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("快递单号").GetValue(x, null).ToString(),
                    标记 = x.GetType().GetProperty("标记").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("标记").GetValue(x, null).ToString(),
                    付款日期 = x.GetType().GetProperty("付款日期").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("付款日期").GetValue(x, null).ToString(),
                    订单来源 = x.GetType().GetProperty("订单来源").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("订单来源").GetValue(x, null).ToString(),
                    运送方式 = x.GetType().GetProperty("运送方式").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("运送方式").GetValue(x, null).ToString(),
                    买家留言 = x.GetType().GetProperty("买家留言").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("买家留言").GetValue(x, null).ToString(),
                    整单优惠金额 = x.GetType().GetProperty("整单优惠金额").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("整单优惠金额").GetValue(x, null).ToString(),
                    买家邮箱 = x.GetType().GetProperty("买家邮箱").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("买家邮箱").GetValue(x, null).ToString(),
                    税费 = x.GetType().GetProperty("税费").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("税费").GetValue(x, null).ToString(),
                    毛重 = x.GetType().GetProperty("毛重").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("毛重").GetValue(x, null).ToString(),
                    净重 = x.GetType().GetProperty("净重").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("净重").GetValue(x, null).ToString(),
                    商品代码 = x.GetType().GetProperty("商品代码").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("商品代码").GetValue(x, null).ToString(),
                    商品名称 = x.GetType().GetProperty("商品名称").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("商品名称").GetValue(x, null).ToString(),
                    辅助属性 = x.GetType().GetProperty("辅助属性").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("辅助属性").GetValue(x, null).ToString(),
                    单位 = x.GetType().GetProperty("单位").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("单位").GetValue(x, null).ToString(),
                    数量 = x.GetType().GetProperty("数量").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("数量").GetValue(x, null).ToString(),
                    单价 = x.GetType().GetProperty("单价").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("单价").GetValue(x, null).ToString(),
                    单品_金额 = x.GetType().GetProperty("单品_金额").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("单品_金额").GetValue(x, null).ToString(),
                    单品_优惠金额 = x.GetType().GetProperty("单品_优惠金额").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("单品_优惠金额").GetValue(x, null).ToString(),
                    折后金额 = x.GetType().GetProperty("折后金额").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("折后金额").GetValue(x, null).ToString(),
                    单品_运费 = x.GetType().GetProperty("单品_运费").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("单品_运费").GetValue(x, null).ToString(),
                    商品条形 = x.GetType().GetProperty("商品条形").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("商品条形").GetValue(x, null).ToString(),
                    发货仓库 = x.GetType().GetProperty("发货仓库").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("发货仓库").GetValue(x, null).ToString(),
                    网上订单号 = x.GetType().GetProperty("网上订单号").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("网上订单号").GetValue(x, null).ToString(),
                    对内备注 = x.GetType().GetProperty("对内备注").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("对内备注").GetValue(x, null).ToString(),
                    配送备注 = x.GetType().GetProperty("配送备注").GetValue(x, null) == null ? "错误" : x.GetType().GetProperty("配送备注").GetValue(x, null).ToString(),
                }).ToList();
                Util.ExcelUtil.ExportSoOrdersPic(_OrderList,
                    new List<string> { "订单日期", "所属网店","代理商", "交易状态", "本地订单号","前端显示订单号", "交易号", "支付方式", "订单人证件", "买家昵称", "收货人名", 
                        "手机号码", "国家", "省" ,"市", "区/县", "收货地址", "金额", "优惠金额", "运费","实收金额", "制单人", "邮政编码","物流公司","快递单号", "标记", 
                        "付款日期", "订单来源", "运送方式", "买家留言", "整单优惠金额", "买家邮箱", "税费", "毛重", "净重", "商品代码", "商品名称", "辅助属性", "单位"
                        , "数量", "单价", "金额", "优惠金额", "折后金额", "运费", "商品条形", "发货仓库", "网上订单号","对内备注","配送备注"},
                     fileName, dataFormat, Filepath);
                //导出Excel，并设置表头列名
                //Util.ExcelUtil.ExportSoOrdersPic<CBXXOutputSoOrders>(exportOrders,
                //                    new List<string> { "序号", "店铺", "会员名", "订单号", "销售订单号", "订单人姓名", "订单人证件号", "订单人电话", "销售日期", 
                //        "订单日期", "订单状态", "买家留言" ,"对内备注", "收件人地址", "商品品名", "商品货号", "商品条形码", "商品图片","申报数量", "申报单价", "申报总价","调价金额", "优惠劵金额","运费", 
                //        "税款", "毛重", "净重", "交易号", "买家支付账号", "快递单号"},
                //                    fileName, dataFormat, Filepath);
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "销售订单导出excel",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, null, userIp, operatorSysno);
            }
            catch (Exception ex)
            {

                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "销售订单导出excel",
                                         LogStatus.系统日志目标类型.ExcelExporting, 0, ex, userIp, operatorSysno);

            }
        }
        #endregion
        #endregion

        /// <summary>
        /// 更新订单广州机场商检状态
        /// </summary>
        /// <param name="soID">订单编号</param>
        /// <param name="GZJCStatus">状态</param>
        /// <returns></returns>
        /// <remarks>2016-4-6 王耀发 创建</remarks>
        public void UpdateOrderGZJCStatus(int soID, int GZJCStatus)
        {
            Hyt.DataAccess.Order.ISoOrderDao.Instance.UpdateOrderGZJCStatus(soID, GZJCStatus);
        }

        /// <summary>
        /// 更新订单南沙商检状态
        /// </summary>
        /// <param name="soID">订单编号</param>
        /// <param name="NsStatus">状态</param>
        /// <returns></returns>
        /// <remarks>2016-4-6 王耀发 创建</remarks>
        public void UpdateOrderNsStatus(int soID, int NsStatus)
        {
            Hyt.DataAccess.Order.ISoOrderDao.Instance.UpdateOrderNsStatus(soID, NsStatus);
        }

        public List<Model.Manual.EntityStatisticMod> GetEntityStatisticDataList(int? defaultWareSysNo, DateTime? startTime, DateTime? endTime, string type)
        {
            return ISoOrderDao.Instance.GetEntityStatisticDataList(defaultWareSysNo, startTime, endTime, type);
        }

        public List<CBSoOrder> GetAllOrderByDateTime(DateTime startTime, DateTime endTime)
        {
            return ISoOrderDao.Instance.GetAllOrderByDateTime(startTime, endTime);
        }

        #region 搭配销售的相关方法
        /// <summary>
        /// 获取订单明细对应的搭配销售
        /// </summary>
        /// <param name="orderitemsysno">明细编号</param>
        /// <returns>是否是搭配明细</returns>
        /// <remarks>2014-12-22 杨浩 创建</remarks>
        public Hyt.Model.Result<string> CheckOrderItemForTieinSales(int orderitemsysno)
        {
            Hyt.Model.Result<string> result = new Hyt.Model.Result<string>() { Status = false, Data = string.Empty };
            var soorderitem = GetOrderItem(orderitemsysno);
            if (soorderitem != null && soorderitem.ProductSalesType == Hyt.Model.WorkflowStatus.CustomerStatus.商品销售类型.搭配销售.GetHashCode())
            {
                result.Status = true;
                //result.StatusCode = soorderitem.GroupCode;
                result.Message = soorderitem.GroupName;
                result.Data = soorderitem.GroupName;
            }
            return result;
        }
        #endregion
        #region 获取订单的最原始订单编号
        /// <summary>
        /// 获取订单的最原始订单编号，换货订单返回原始订单号，非换货订单就返回当前订单编号
        /// </summary>
        /// <param name="currectorderid">当前订单</param>
        /// <returns></returns>
        /// <remarks>2015-04-29 杨浩 创建</remarks>
        public int GetOriginalOrderID(int currectorderid)
        {
            return ISoOrderDao.Instance.GetOriginalOrderID(currectorderid);
        }

        #endregion
        #region 判断当前订单是否需要补货
        /// <summary>
        /// 判断当前订单是否需要补货(公司收钱，加盟商发货或者退货）
        /// </summary>
        /// <param name="orderid">订单编号</param>
        /// <param name="warehouseSysNo">仓库编号(入库，或者出库仓库编号）</param>
        /// <returns></returns>
        /// <remarks>2015-04-29 杨浩 创建</remarks>
        public bool IsNeedReplenishment(int orderid, int warehouseSysNo)
        {
            bool flg = false;
            var oldorder = GetOriginalOrderID(orderid);//最原始订单编号
            var wh = Hyt.DataAccess.Warehouse.IWhWarehouseDao.Instance.GetWarehouseEntity(warehouseSysNo);//发货仓库信息
            if (wh != null && wh.IsSelfSupport == (int)WarehouseStatus.是否自营.否)//发货仓库非自营
            {
                var fn = FnReceiptVoucherBo.Instance.GetReceiptVoucherByOrder(orderid);//订单收款单
                if (fn != null && fn.IncomeType == FinanceStatus.收款单收入类型.预付.GetHashCode())//预付订单
                {
                    var fnitems = FnReceiptVoucherBo.Instance.GetReceiptVoucherItem(fn.SysNo);
                    if (fnitems != null)
                    {
                        var fs = fnitems.Where(m => m.ReceivablesSideType == (int)Hyt.Model.WorkflowStatus.FinanceStatus.收款方类型.仓库).ToList();
                        if (fs != null && fs.Count > 0)//存在仓库自己收钱行为
                        {
                            foreach (var item in fs)
                            {
                                if (item.ReceivablesSideSysNo != warehouseSysNo)//收款仓库不是当前发货仓库
                                {
                                    var ww = Hyt.DataAccess.Warehouse.IWhWarehouseDao.Instance.GetWarehouseEntity(item.ReceivablesSideSysNo);
                                    if (ww != null && ww.IsSelfSupport == (int)WarehouseStatus.是否自营.是)//自营仓库收钱，非自营仓库发货，要补货
                                    {
                                        flg = true;//自营仓库收钱，非自营仓库发货，要补货
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            flg = true;//不存在仓库自己收钱的情况，钱都是品胜公司收的款
                        }
                    }
                }
            }
            return flg;
        }
        #endregion

        /// <summary>
        /// 查找订单创建人所在企业的EAS编码
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <returns>企业的EAS编码</returns>
        /// <remarks>2015-01-29 杨浩 创建</remarks>
        public string GetOrderEnterpriseForERPCode(int orderSysNo)
        {
            string enterpriseERPCode = string.Empty;

            try
            {

                var order = SoOrderBo.Instance.GetEntity(orderSysNo);

                if (order != null)
                {
                    //经销商升舱订单获取对应店铺的ERP Code
                    if (order.OrderSource == Model.WorkflowStatus.OrderStatus.销售单来源.分销商升舱.GetHashCode())
                    {
                        var mall = DsEasBo.Instance.Get(order.OrderSourceSysNo);
                        if (mall != null)
                        {
                            enterpriseERPCode = mall.Code;
                        }
                        else
                        {
                            throw new Exception(string.Format("找不到系统编号为{0}的经销商商城", order.OrderSourceSysNo));
                        }
                    }
                    else if (order.OrderSource == Model.WorkflowStatus.OrderStatus.销售单来源.手机商城.GetHashCode()
                            || order.OrderSource == Model.WorkflowStatus.OrderStatus.销售单来源.PC网站.GetHashCode()
                        // || order.OrderSource == Model.WorkflowStatus.OrderStatus.销售单来源.千机网.GetHashCode()
                        // || order.OrderSource == Model.WorkflowStatus.OrderStatus.销售单来源..GetHashCode()
                            || order.OrderSource == Model.WorkflowStatus.OrderStatus.销售单来源.客服下单.GetHashCode()
                        )
                    {
                        enterpriseERPCode = Extra.Erp.Model.EasConstant.HytCustomer;
                    }
                    else
                    {
                        try
                        {
                            //var ssoUserSysNo = SySsoUserAssociationBo.Instance.GetSsoIdByUserSysNo(order.OrderCreatorSysNo);
                            //var ssoUser = SySsoUserAssociationBo.Instance.GetSsoUserInfoBySsoUserId(ssoUserSysNo);
                            var ssoUser = SySsoUserAssociationBo.Instance.GetSsoUserInfoBySsoUserId(order.OrderCreatorSysNo);


                            enterpriseERPCode = DsDealerBo.Instance.GetDealerByEnterpriseID(ssoUser.EnterpriseNO).ErpCode;
                        }
                        catch
                        {
                            throw new Exception(string.Format("查找订单下单人所属企业信息时出现异常,订单编号:{0}", orderSysNo));
                        }
                    }
                }
                else
                {
                    throw new Exception(string.Format("找不到编号为:{0}的订单", orderSysNo));
                }
            }
            catch { }

            return enterpriseERPCode;
        }

        #region 订单导入

        /// <summary>
        /// import cols mapping between DB and excel
        /// </summary>
        /// <remarks>2016-07-2 王耀发 创建</remarks>
        private static readonly Dictionary<string, string> DicColsMapping = new Dictionary<string, string>
            {               
                {"Account", "会员账号"},
                {"DeliveryTypeName", "配送方式"},
                {"WarehouseName", "仓库"},
                {"PaymentName", "支付方式"},
                {"ReceiveAddress","收货地址(收货人,省份,市,区,街道,联系手机,身份证号码[选填项])"},
                {"OrderItem","订购商品(商品编码,数量,单价;)"},
                {"DealerName","分销商"},
                {"FreightAmount","运费"},
                {"TaxFee","税费"},
                {"DeliveryTime", "配送时间段"},
                {"DeliveryRemarks", "配送备注"},
                {"ContactBeforeDelivery", "送货前联系"},
                {"CustomerMessage", "会员留言"},
                {"InternalRemarks","对内备注"},                            
                {"InvoiceType","发票类型"},
                {"InvoiceTitle","发票抬头"},
                {"InvoiceRemarks","发票备注"},
                {"SysNo","订单系统编号"},
                {"CurrentTime","下单时间(如2017-05-22 14:42[选填项])"}
            };

        /// <summary>
        /// 导入excel
        /// </summary>
        /// <param name="stream">导入的excel stream format</param>
        /// <param name="operatorSysno">操作人员编号</param>
        /// <returns>封装的泛型result对象</returns>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        public Result ImportExcel(Stream stream, int operatorSysno)
        {
            string operatorName = SyUserBo.Instance.GetUserName(operatorSysno);
            System.Data.DataTable dt = null;
            var cols = DicColsMapping.Select(p => p.Value).ToArray();
            try
            {
                dt = ExcelUtil.ImportExcel(stream, cols);
            }
            catch (Exception ex)
            {
                //exception happened,some not caughted
                return new Result
                {
                    Message = string.Format("数据导入错误,请选择正确的excel文件"),
                    Status = false
                };
            }
            if (dt == null)
            {
                //not all the cols mapped
                return new Result
                {
                    Message = string.Format("请选择正确的excel文件!"),
                    Status = false
                };
            }
            if (dt.Rows.Count == 0)
            {
                //not all the cols mapped
                return new Result
                {
                    Message = string.Format("导入的excel文件不能为空!"),
                    Status = false
                };
            }
            var excellst = new List<SoOrderList>();
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                int excelRow = i;
                for (var j = 0; j < dt.Columns.Count; j++)
                {
                    //前8列不可以为空
                    if (j < 8)
                    {
                        if ((dt.Rows[i][j] == null || string.IsNullOrEmpty(dt.Rows[i][j].ToString())))
                        {
                            return new Result
                            {
                                Message = string.Format("excel表第{0}行第{1}列数据不能有空值", (excelRow + 1), (j + 1)),
                                Status = false
                            };
                        }
                    }
                }


                var dealerName = dt.Rows[i][DicColsMapping["DealerName"]].ToString().Trim();
                var dealer = DsDealerBo.Instance.GetDsDealerByName(dealerName);
                if (dealer == null)
                {
                    return new Result
                    {
                        Message = string.Format("excel表第{0}行分销商不存在", (excelRow + 1)),
                        Status = false
                    };
                }
                //判断分销商状态
                if (dealer.Status == 0)
                {
                    return new Result
                    {
                        Message = string.Format("excel表第{0}行分销商状态不是启用", (excelRow + 1)),
                        Status = false
                    };
                }

                //收货地址
                var receiveAddress = dt.Rows[i][DicColsMapping["ReceiveAddress"]].ToString().Trim();

                #region 地区
                var addArray = receiveAddress.Split(',');
                if (addArray.Length < 6)
                {
                    return new Result
                    {
                        Message = string.Format("excel表第{0}行收货地址不正确", (excelRow + 1)),
                        Status = false
                    };
                }
                var name = addArray[0];
                if (string.IsNullOrWhiteSpace(name))
                {
                    return new Result
                    {
                        Message = string.Format("excel表第{0}行收货人不能为空", (excelRow + 1)),
                        Status = false
                    };
                }
                var province = addArray[1];
                if (string.IsNullOrWhiteSpace(province))
                {
                    return new Result
                    {
                        Message = string.Format("excel表第{0}行省份不能为空", (excelRow + 1)),
                        Status = false
                    };
                }

                var city = addArray[2];
                if (string.IsNullOrWhiteSpace(city))
                {
                    return new Result
                    {
                        Message = string.Format("excel表第{0}行城市不能为空", (excelRow + 1)),
                        Status = false
                    };
                }

                var area = addArray[3];
                if (string.IsNullOrWhiteSpace(city))
                {
                    return new Result
                    {
                        Message = string.Format("excel表第{0}行区不能为空", (excelRow + 1)),
                        Status = false
                    };
                }

                var streetAddress = addArray[4];
                if (string.IsNullOrWhiteSpace(streetAddress))
                {
                    return new Result
                    {
                        Message = string.Format("excel表第{0}行街道不能为空", (excelRow + 1)),
                        Status = false
                    };
                }
                var mobilePhoneNumber = addArray[5];
                if (string.IsNullOrWhiteSpace(mobilePhoneNumber))
                {
                    return new Result
                    {
                        Message = string.Format("excel表第{0}行手机号码不能为空", (excelRow + 1)),
                        Status = false
                    };
                }
                var iDCardNo = "";
                if (addArray.Length >= 7)
                    iDCardNo = addArray[6];

                var areaInfo = BLL.Basic.BasicAreaBo.Instance.GetYouZanMatchDistrict(city, area);
                if (areaInfo == null)
                {
                    return new Result()
                    {
                        Status = false,
                        Message = "省【" + province + "】城市【" + city + "】地区【" + area + "】在系统中不存在，请到基础管理->地区信息管理添加再试！",
                    };

                }
                var hytreceive = new Model.SoReceiveAddress()
                {
                    IDCardNo = iDCardNo,
                    AreaSysNo = areaInfo.SysNo,
                    Name = name,
                    MobilePhoneNumber = mobilePhoneNumber,
                    StreetAddress = streetAddress,
                    PhoneNumber = mobilePhoneNumber,
                };
                #endregion

                #region 会员

                //会员账号
                var account = dt.Rows[i][DicColsMapping["Account"]].ToString().Trim();
                Hyt.Model.CrCustomer cr = null;
                string strPassword = "123456";//初始密码

                var options = new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadCommitted,
                    Timeout = TransactionManager.DefaultTimeout
                };

                using (var trancustomer = new System.Transactions.TransactionScope(TransactionScopeOption.Required, options))//会员创建事物
                {
                    var customerlst = Hyt.BLL.Order.SoOrderBo.Instance.SearchCustomer(account);
                    if (customerlst != null && customerlst.Count > 0)
                    {
                        cr = customerlst.First();
                    }
                    else //创建会员
                    {
                        cr = new Model.CrCustomer()
                        {
                            Account = hytreceive.MobilePhoneNumber,
                            MobilePhoneNumber = hytreceive.MobilePhoneNumber,
                            AreaSysNo = areaInfo.SysNo,
                            Gender = (int)Hyt.Model.WorkflowStatus.CustomerStatus.性别.保密,
                            EmailStatus = (int)Hyt.Model.WorkflowStatus.CustomerStatus.邮箱状态.未验证,
                            LevelSysNo = Hyt.Model.SystemPredefined.CustomerLevel.初级,
                            Name = name,
                            NickName = name,
                            RegisterDate = DateTime.Now,
                            Password = strPassword, // Hyt.Util.EncryptionUtil.EncryptWithMd5AndSalt(strPassword), 余勇修改 2014-09-12
                            Status = (int)Hyt.Model.WorkflowStatus.CustomerStatus.会员状态.有效,
                            MobilePhoneStatus = (int)Hyt.Model.WorkflowStatus.CustomerStatus.手机状态.未验证,
                            RegisterSource = (int)Hyt.Model.WorkflowStatus.CustomerStatus.注册来源.分销工具,
                            RegisterSourceSysNo = dealer.SysNo.ToString(),
                            StreetAddress = streetAddress,
                            IsReceiveShortMessage = (int)CustomerStatus.是否接收短信.是,
                            IsReceiveEmail = (int)CustomerStatus.是否接收邮件.是,
                            LastLoginDate = DateTime.Now,
                            Birthday = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                            CreatedDate = DateTime.Now,
                        };


                        Hyt.Model.CrReceiveAddress crr = new Model.CrReceiveAddress()
                        {
                            AreaSysNo = areaInfo.SysNo,
                            Name = name,
                            MobilePhoneNumber = hytreceive.MobilePhoneNumber,
                            StreetAddress = streetAddress,
                            IsDefault = 1
                        };

                        Hyt.BLL.Order.SoOrderBo.Instance.CreateCustomer(cr, crr);
                    }
                    trancustomer.Complete();//会员创建事物
                }

                if (cr == null || cr.SysNo < 1)
                {
                    return new Result
                    {
                        Message = "会员信息读取失败",
                        Status = false
                    };
                }
                #endregion

                #region 配送方式

                //配送方式
                var DeliveryTypeName = dt.Rows[i][DicColsMapping["DeliveryTypeName"]].ToString().Trim();

                bool delFlag = false;
                string delList = "";
                int delSysNo = 0;
                var deliveryTypeList = LoadDeliveryTypeByAreaSysNo(areaInfo.SysNo, areaInfo.ParentSysNo, false);
                foreach (LgDeliveryType delType in deliveryTypeList)
                {
                    if (delList == "")
                    {
                        delList += delType.DeliveryTypeName;
                    }
                    else
                    {
                        delList += "，" + delType.DeliveryTypeName;
                    }
                    if (DeliveryTypeName == delType.DeliveryTypeName)
                    {
                        delSysNo = delType.SysNo;
                        delFlag = true;
                        break;
                    }
                }
                if (!delFlag)
                {
                    return new Result
                    {
                        Message = string.Format("excel表第{0}行配送方式不存在配送范围内，<br/>必须为{1}", (excelRow + 1), delList),
                        Status = false
                    };
                }

                #endregion

                #region 仓库
                //获取订单仓库
                var WarehouseName = dt.Rows[i][DicColsMapping["WarehouseName"]].ToString().Trim();
                bool wareFlag = false;
                string wareList = "";
                int wareSysNo = 0;
                //获取配送对应仓库列
                IList<WhWarehouse> WhWarehouse = WhWarehouseBo.Instance.GetWhWarehouseListByDeliveryType(delSysNo);
                foreach (WhWarehouse warehouse in WhWarehouse)
                {
                    if (wareList == "")
                    {
                        wareList += warehouse.BackWarehouseName;
                    }
                    else
                    {
                        wareList += "，" + warehouse.BackWarehouseName;
                    }
                    if (WarehouseName == warehouse.BackWarehouseName)
                    {
                        wareSysNo = warehouse.SysNo;
                        wareFlag = true;
                        break;
                    }
                }
                if (!wareFlag)
                {
                    return new Result
                    {
                        Message = string.Format("excel表第{0}行仓库不存在配送范围内，<br/>必须为{1}", (excelRow + 1), wareList),
                        Status = false
                    };
                }
                #endregion

                #region 支付方式
                //获取支付方式
                var paymentName = dt.Rows[i][DicColsMapping["PaymentName"]].ToString().Trim();
                bool payFlag = false;
                string payList = "";
                int paySysNo = 0;
                var paymentTypeList = LoadPayTypeListByDeliverySysNo(delSysNo);
                foreach (var paymenttype in paymentTypeList)
                {
                    if (payList == "")
                    {
                        payList += paymenttype.PaymentName;
                    }
                    else
                    {
                        payList += "，" + paymenttype.PaymentName;
                    }
                    if (paymentName == paymenttype.PaymentName)
                    {
                        paySysNo = paymenttype.SysNo;
                        payFlag = true;
                        break;
                    }
                }
                if (!payFlag)
                {
                    return new Result
                    {
                        Message = string.Format("excel表第{0}行支付方式不存在配送支付范围内，<br/>必须为{1}", (excelRow + 1), payList),
                        Status = false
                    };
                }
                #endregion

                //订购商品
                int arraylen = 0;
                var OrderItem = dt.Rows[i][DicColsMapping["OrderItem"]].ToString().Trim();
                var ItemListArray = OrderItem.Split(';');
                foreach (var item in ItemListArray)
                {
                    if (item != null && !string.IsNullOrEmpty(item))
                    {
                        var ItemArray = item.Split(',');
                        String ErpCode = ItemArray[0].ToString().Trim();
                        String Quantity = ItemArray[1].ToString().Trim();
                        String SalesUnitPrice = ItemArray[2].ToString().Trim();
                        if (ErpCode == null || string.IsNullOrEmpty(ErpCode))
                        {
                            return new Result
                            {
                                Message = string.Format("excel表第{0}行订购商品的商品编码不能有空值", (excelRow + 1)),
                                Status = false
                            };
                        }
                        if (Quantity == null || string.IsNullOrEmpty(Quantity))
                        {
                            return new Result
                            {
                                Message = string.Format("excel表第{0}行订购商品的数量不能有空值", (excelRow + 1)),
                                Status = false
                            };
                        }
                        Decimal typeQuantity = 0;
                        if (!Decimal.TryParse(Quantity, out typeQuantity))
                        {
                            return new Result
                            {
                                Message = string.Format("excel表第{0}行订购商品的数量必须为数值", (excelRow + 1)),
                                Status = false
                            };
                        }
                        if (SalesUnitPrice == null || string.IsNullOrEmpty(SalesUnitPrice))
                        {
                            return new Result
                            {
                                Message = string.Format("excel表第{0}行订购商品的单价不能有空值", (excelRow + 1)),
                                Status = false
                            };
                        }
                        Decimal typeOriginalPrice = 0;
                        if (!Decimal.TryParse(SalesUnitPrice, out typeOriginalPrice))
                        {
                            return new Result
                            {
                                Message = string.Format("excel表第{0}行订购商品的原单价必须为数值", (excelRow + 1)),
                                Status = false
                            };
                        }
                        PdProduct pEntity = PdProductBo.Instance.GetProductByErpCode(ErpCode);
                        if (pEntity == null)
                        {
                            return new Result
                            {
                                Message = string.Format("excel表第{0}行订购商品的商品编号:{1}不存在", (excelRow + 1), ErpCode),
                                Status = false
                            };
                        }

                        //PdProductStock psEntity = IPdProductStockDao.Instance.GetEntityByWP(wareSysNo, pEntity.SysNo);
                        //if (psEntity == null || psEntity.StockQuantity == 0)
                        //{
                        //    return new Result
                        //    {
                        //        Message = string.Format("excel表第{0}行订购商品的商品编号:{1}不存在库存", (excelRow + 1), ErpCode),
                        //        Status = false
                        //    };
                        //}
                        arraylen++;
                    }
                    else
                    {
                        return new Result
                        {
                            Message = string.Format("excel表第{0}行订购商品有空值", (excelRow + 1)),
                            Status = false
                        };
                    }
                }


                //分销商账号
                int dealerSysNo = dealer.SysNo;
                //运费
                var FreightAmount = dt.Rows[i][DicColsMapping["FreightAmount"]].ToString().Trim();
                Decimal typeFreightAmount = 0;
                if (!Decimal.TryParse(FreightAmount, out typeFreightAmount))
                {
                    return new Result
                    {
                        Message = string.Format("excel表第{0}行订购商品的运费必须为数值", (excelRow + 1)),
                        Status = false
                    };
                }
                //税费
                var TaxFee = dt.Rows[i][DicColsMapping["TaxFee"]].ToString().Trim();
                Decimal typeTaxFee = 0;
                if (!Decimal.TryParse(TaxFee, out typeTaxFee))
                {
                    return new Result
                    {
                        Message = string.Format("excel表第{0}行订购商品的税费必须为数值", (excelRow + 1)),
                        Status = false
                    };
                }
                //配送时间段 1.一周之内全天可送达 2.周一至周五送货 3.双休日及公众假期送货
                var DeliveryTime = dt.Rows[i][DicColsMapping["DeliveryTime"]].ToString().Trim();
                if (DeliveryTime != "一周之内全天可送达" && DeliveryTime != "周一至周五送货" && DeliveryTime != "双休日及公众假期送货")
                {
                    return new Result
                    {
                        Message = string.Format("excel表第{0}行配送时间段不存在范围内，<br/>必须为{1}", (excelRow + 1), "一周之内全天可送达，周一至周五送货，双休日及公众假期送货"),
                        Status = false
                    };
                }
                //配送备注 
                var DeliveryRemarks = dt.Rows[i][DicColsMapping["DeliveryRemarks"]].ToString().Trim();
                //送货前联系 
                var ContactBeforeDelivery = dt.Rows[i][DicColsMapping["ContactBeforeDelivery"]].ToString().Trim();
                if (ContactBeforeDelivery != "是" && ContactBeforeDelivery != "否")
                {
                    return new Result
                    {
                        Message = string.Format("excel表第{0}行送货前联系不存在，<br/>必须为{1}", (excelRow + 1), "是，否"),
                        Status = false
                    };
                }
                //会员留言
                var CustomerMessage = dt.Rows[i][DicColsMapping["CustomerMessage"]].ToString().Trim();
                //对内备注
                var InternalRemarks = dt.Rows[i][DicColsMapping["InternalRemarks"]].ToString().Trim();
                //发票类型
                var InvoiceType = dt.Rows[i][DicColsMapping["InvoiceType"]].ToString().Trim();
                bool invFlag = false;
                string invList = "";
                int invSysNo = 0;
                IList<FnInvoiceType> FnInvoiceType = FnInvoiceBo.Instance.GetFnInvoiceTypeList();
                if (!String.IsNullOrEmpty(InvoiceType))
                {
                    foreach (FnInvoiceType invoiceType in FnInvoiceType)
                    {
                        if (invList == "")
                        {
                            invList += invoiceType.Name;
                        }
                        else
                        {
                            invList += "，" + invoiceType.Name;
                        }
                        if (paymentName == invoiceType.Name)
                        {
                            invSysNo = invoiceType.SysNo;
                            invFlag = true;
                            break;
                        }
                    }
                    if (!invFlag)
                    {
                        return new Result
                        {
                            Message = string.Format("excel表第{0}行发票类型不存在，<br/>必须为{1}", (excelRow + 1), invList),
                            Status = false
                        };
                    }
                }
                //发票抬头
                var InvoiceTitle = dt.Rows[i][DicColsMapping["InvoiceTitle"]].ToString().Trim();
                //发票备注
                var InvoiceRemarks = dt.Rows[i][DicColsMapping["InvoiceRemarks"]].ToString().Trim();
                //订单系统编号
                SoOrder OrderModel = new SoOrder();
                var SysNo = dt.Rows[i][DicColsMapping["SysNo"]].ToString().Trim();
                if (!String.IsNullOrEmpty(SysNo))
                {
                    int typeSysNo = 0;
                    if (!int.TryParse(SysNo, out typeSysNo))
                    {
                        return new Result
                        {
                            Message = string.Format("excel表第{0}行订单系统编号必须为整数值", (excelRow + 1)),
                            Status = false
                        };
                    }
                    OrderModel = SoOrderBo.Instance.GetEntity(int.Parse(SysNo));
                    if (OrderModel == null)
                    {
                        return new Result
                        {
                            Message = string.Format("excel表第{0}行订单系统编号不存在订单", (excelRow + 1)),
                            Status = false
                        };
                    }
                    if (OrderModel.Status != (int)OrderStatus.销售单状态.待审核 || OrderModel.PayStatus != (int)OrderStatus.销售单支付状态.未支付)
                    {
                        return new Result
                        {
                            Message = string.Format("excel表第{0}行订单系统编号对应订单不是待审核和未支付，不能修改", (excelRow + 1)),
                            Status = false
                        };
                    }
                }
                //下单时间
                var ordertime = dt.Rows[i][DicColsMapping["CurrentTime"]].ToString().Trim();
                var currentTime = DateTime.Now;
                if (!String.IsNullOrEmpty(ordertime))
                {
                    try { currentTime = Convert.ToDateTime(ordertime); }
                    catch (Exception ex)
                    {
                        return new Result
                        {
                            Message = string.Format("excel表第{0}行下单时间格式填写错误，正确格式如(2017-05-22" + " 14:42)", (excelRow + 1)),
                            Status = false
                        };
                    }

                }
                //获取订单数据

                SoOrder SoOrder = new SoOrder();
                if (OrderModel.SysNo > 0)
                {
                    SoOrder = OrderModel;
                }
                else
                {
                    SoOrder.Remarks = "";
                    SoOrder.Status = 10;
                    SoOrder.CoinPay = 0;
                    SoOrder.ProductDiscountAmount = 0;
                    SoOrder.ProductChangeAmount = 0;//调价金额合计
                    SoOrder.FreightDiscountAmount = 0;
                    SoOrder.FreightChangeAmount = 0;
                    SoOrder.OrderDiscountAmount = 0;
                    SoOrder.CouponAmount = 0;
                    SoOrder.CoinPay = 0;

                    SoOrder.OrderSource = (int)OrderStatus.销售单来源.客服下单;
                    SoOrder.PayStatus = (int)OrderStatus.销售单支付状态.未支付;
                    SoOrder.SalesType = (int)OrderStatus.销售方式.普通订单;
                    SoOrder.Status = (int)OrderStatus.销售单状态.待审核;
                    SoOrder.SendStatus = (int)OrderStatus.销售单推送状态.未推送;
                    SoOrder.OnlineStatus = Constant.OlineStatusType.待支付;
                    SoOrder.OrderNo = Hyt.BLL.Basic.ReceiptNumberBo.Instance.GetOrderNo();
                }
                SoOrder.CustomerSysNo = cr.SysNo;
                SoOrder.DefaultWarehouseSysNo = wareSysNo;
                SoOrder.DeliveryTypeSysNo = delSysNo;
                SoOrder.PayTypeSysNo = paySysNo;
                SoOrder.DeliveryRemarks = DeliveryRemarks;
                SoOrder.DeliveryTime = DeliveryTime;
                SoOrder.ContactBeforeDelivery = ContactBeforeDelivery == "是" ? 1 : 0;
                SoOrder.CustomerMessage = CustomerMessage;
                SoOrder.InternalRemarks = InternalRemarks;
                SoOrder.FreightAmount = decimal.Parse(FreightAmount);
                SoOrder.TaxFee = decimal.Parse(TaxFee);
                SoOrder.AuditorDate = currentTime;
                SoOrder.CancelDate = currentTime;
                SoOrder.Stamp = currentTime;
                SoOrder.CreateDate = currentTime;
                SoOrder.LastUpdateBy = operatorSysno;
                SoOrder.LastUpdateDate = currentTime;
                SoOrder.OrderCreatorSysNo = operatorSysno;
                SoOrder.OrderSourceSysNo = operatorSysno;
                SoOrder.DealerSysNo = dealerSysNo;

                #region 发票
                //获取发票
                FnInvoice Invoice = new FnInvoice();
                if (invSysNo != 0)
                {
                    Invoice.InvoiceTypeSysNo = invSysNo;
                    Invoice.InvoiceTitle = InvoiceTitle;
                    Invoice.InvoiceRemarks = InvoiceRemarks;
                }
                if (Invoice.InvoiceTypeSysNo == 0)
                {
                    Invoice = null;
                }
                else
                {
                    Invoice.CreatedBy = operatorSysno;
                    Invoice.CreatedDate = currentTime;
                    Invoice.LastUpdateBy = operatorSysno;
                    Invoice.LastUpdateDate = currentTime;
                    Invoice.Status = (int)FinanceStatus.发票状态.待开票;
                }
                #endregion

                //获取销售单明细
                //订单金额
                decimal ProductAmount = 0;
                List<SoOrderItem> orderItemList = new List<SoOrderItem>();
                foreach (var item in ItemListArray)
                {
                    var ItemArray = item.Split(',');
                    String ErpCode = ItemArray[0].ToString().Trim();
                    String Quantity = ItemArray[1].ToString().Trim();
                    String SalesUnitPrice = ItemArray[2].ToString().Trim();
                    ProductAmount += decimal.Parse(Quantity) * decimal.Parse(SalesUnitPrice);
                    PdProduct pEntity = PdProductBo.Instance.GetProductByErpCode(ErpCode);
                    var model = new SoOrderItem
                    {
                        ProductSysNo = pEntity.SysNo,
                        ProductName = pEntity.ProductName,
                        Quantity = int.Parse(Quantity),
                        OriginalPrice = decimal.Parse(SalesUnitPrice),
                        SalesUnitPrice = decimal.Parse(SalesUnitPrice),
                        SalesAmount = int.Parse(Quantity) * decimal.Parse(SalesUnitPrice),

                    };
                    orderItemList.Add(model);
                }
                SoOrder.ProductAmount = ProductAmount;
                SoOrder.OrderAmount = ProductAmount + SoOrder.FreightAmount + SoOrder.TaxFee;
                SoOrder.CashPay = SoOrder.OrderAmount;

                try
                {
                    using (var tran = new TransactionScope())
                    {
                        //如果为订单修改，删除对应发票信息和收货地址
                        if (OrderModel.SysNo > 0)
                        {
                            //删除对应发票信息
                            IFnInvoiceDao.Instance.DeleteEntity(SoOrder.InvoiceSysNo);
                            //删除收货地址
                            //ISoReceiveAddressDao.Instance.DeleteEntity(SoOrder.ReceiveAddressSysNo);
                            //删除订单明细
                            ISoOrderItemDao.Instance.DeleteByOrderSysNo(SoOrder.SysNo);
                            //删除收款单
                            IFnReceiptVoucherDao.Instance.DeleteBySource(SoOrder.SysNo, (int)Hyt.Model.WorkflowStatus.FinanceStatus.收款来源类型.销售单);
                        }
                        //保存发票信息
                        if (Invoice != null)
                        {
                            IFnInvoiceDao.Instance.InsertEntity(Invoice);
                            SoOrder.InvoiceSysNo = Invoice.SysNo;
                        }

                        //保存订单收货地址
                        hytreceive = ISoReceiveAddressDao.Instance.InsertEntity(hytreceive);
                        SoOrder.ReceiveAddressSysNo = hytreceive.SysNo;

                        if (OrderModel.SysNo > 0)
                        {
                            //更新订单
                            UpdateOrder(SoOrder);
                        }
                        else
                        {
                            if (SoOrder.PayTypeSysNo == Hyt.Model.SystemPredefined.PaymentType.分销商预存 && SoOrder.CashPay > 0)
                            {
                                SoOrder.Status = (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.待审核;
                                SoOrder.OnlineStatus = Constant.OlineStatusType.待审核;
                                SoOrder.SalesType = (int)Hyt.Model.WorkflowStatus.OrderStatus.销售方式.经销订单;
                                SoOrder.OrderSource = (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单来源.业务员下单;
                                SoOrder.PayStatus = (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单支付状态.已支付;
                            }

                            //保存订单
                            SoOrder = ISoOrderDao.Instance.Inser(SoOrder);
                            SoOrder = ISoOrderDao.Instance.GetEntity(SoOrder.SysNo);
                            if (SoOrder.PayTypeSysNo == Hyt.Model.SystemPredefined.PaymentType.分销商预存 && SoOrder.CashPay > 0)
                                Hyt.BLL.MallSeller.DsOrderBo.Instance.FreezeDsPrePayment(SoOrder.SysNo, dealerSysNo, SoOrder.CashPay, BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);//冻结金额

                        }
                        //调用获取销售单明细数据接口，并保存明细数据
                        foreach (var orderItem in orderItemList)
                        {
                            orderItem.OrderSysNo = SoOrder.SysNo;
                            ISoOrderItemDao.Instance.Insert(orderItem);
                        }
                        if (Invoice != null)//记录发票事物编号
                        {
                            Invoice.InvoiceAmount = SoOrder.CashPay;
                            Invoice.TransactionSysNo = SoOrder.TransactionSysNo;
                            Hyt.BLL.Order.SoOrderBo.Instance.UpdateOrderInvoice(Invoice); //更新发票 余勇 修改 改为调用业务层方法
                        }

                        if (SoOrder.PayTypeSysNo == Hyt.Model.SystemPredefined.PaymentType.分销商预存)
                        {
                            #region 分销升舱
                            if (OrderModel.SysNo <= 0)
                            {
                                Finance.FnReceiptVoucherBo.Instance.CreateReceiptVoucherByOrder(SoOrder);//创建订单收款单
                                FnReceiptVoucherItem fvitem = new FnReceiptVoucherItem()
                                {
                                    Amount = SoOrder.CashPay,
                                    CreatedDate = SoOrder.CreateDate,
                                    PaymentTypeSysNo = Hyt.Model.SystemPredefined.PaymentType.分销商预存,
                                    TransactionSysNo = SoOrder.TransactionSysNo,
                                    Status = (int)Hyt.Model.WorkflowStatus.FinanceStatus.收款单明细状态.有效,
                                    ReceivablesSideType = (int)Hyt.Model.WorkflowStatus.FinanceStatus.收款方类型.分销中心,//收款方来源
                                    ReceivablesSideSysNo = SoOrder.OrderSourceSysNo
                                };
                                Finance.FnReceiptVoucherBo.Instance.InsertOrderReceiptVoucher(SoOrder.SysNo, fvitem);
                                WriteSoTransactionLog(SoOrder.TransactionSysNo
                                                      , string.Format("订单导入成功，等待客服确认", SoOrder.SysNo)
                                                      , operatorName);

                                //写订单池记录
                                SyJobPoolPublishBo.Instance.OrderAuditBySysNo(SoOrder.SysNo);
                                SyJobDispatcherBo.Instance.WriteJobLog(string.Format("订单导入成功创建订单审核任务，销售单编号:{0}",
                                              SoOrder.SysNo), SoOrder.SysNo, null, 0);

                                try
                                {
                                    Finance.FnReceiptVoucherBo.Instance.AutoConfirmReceiptVoucher(SoOrder.SysNo, new SyUser { SysNo = 0, UserName = operatorName });


                                }
                                catch (Exception ex)
                                {

                                    Hyt.BLL.Log.SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "分销商预存款付款订单导入自动确认收款单",
                                                            LogStatus.系统日志目标类型.EAS, SoOrder.SysNo, ex, string.Empty, 0);
                                }
                            }

                            #endregion
                        }
                        else
                        {
                            //创建收款单
                            Finance.FnReceiptVoucherBo.Instance.CreateReceiptVoucherByOrder(SoOrder);//创建订单收款单
                            if (OrderModel.SysNo <= 0)
                            {
                                //插入订单池
                                if (SoOrder.OrderSource != (int)OrderStatus.销售单来源.业务员补单 && SoOrder.OrderSource != (int)(int)OrderStatus.销售单来源.门店下单)//业务员补单不进行任务分配
                                {
                                    int assignTo = 0;//指定下一个订单操作人
                                    if (SoOrder.OrderSource == (int)OrderStatus.销售单来源.客服下单) assignTo = SoOrder.OrderCreatorSysNo;//客服下单 默认分配给自己
                                    if (assignTo > 0)//已经指定了分配人
                                    {
                                        SyJobPoolPublishBo.Instance.OrderAuditBySysNo(SoOrder.SysNo, assignTo);
                                        SyJobDispatcherBo.Instance.WriteJobLog(string.Format("{0}创建订单并给自己分配订单审核任务，销售单编号:{1}", SyUserBo.Instance.GetUserName(SoOrder.OrderCreatorSysNo),
                                           SoOrder.SysNo), SoOrder.SysNo, null, SoOrder.OrderCreatorSysNo);
                                    }
                                    else//未指定，系统根据规则自动分配
                                    {
                                        //写订单池记录
                                        SyJobPoolPublishBo.Instance.OrderAuditBySysNo(SoOrder.SysNo);
                                        SyJobDispatcherBo.Instance.WriteJobLog(string.Format("{0}创建订单并生成订单审核任务，销售单编号:{1}", SyUserBo.Instance.GetUserName(SoOrder.OrderCreatorSysNo),
                                          SoOrder.SysNo), SoOrder.SysNo, null, SoOrder.OrderCreatorSysNo);
                                    }

                                }
                                //门店下单转快递 ,加入任务池
                                else if (SoOrder.OrderSource == (int)OrderStatus.销售单来源.门店下单 && SoOrder.DeliveryTypeSysNo == (int)Hyt.Model.SystemPredefined.DeliveryType.第三方快递)
                                {
                                    SyJobPoolPublishBo.Instance.OrderAuditBySysNo(SoOrder.SysNo);
                                    SyJobDispatcherBo.Instance.WriteJobLog(string.Format("{0}门店下单转快递生成订单审核任务，销售单编号:{1}", SyUserBo.Instance.GetUserName(SoOrder.OrderCreatorSysNo),
                                         SoOrder.SysNo), SoOrder.SysNo, null, SoOrder.OrderCreatorSysNo);
                                }
                            }
                        }
                        //同步支付时间的到订单主表
                        ISoOrderDao.Instance.UpdateOrderPayDteById(SoOrder.SysNo);
                        tran.Complete();
                    }
                }
                catch (Exception ex)
                {

                    return new Result
                    {
                        Message = ex.Message,
                        Status = true
                    };
                }
            }
            return new Result
            {
                Message = "成功导入订单",
                Status = true
            };
        }
        /// <summary>
        /// 根据区县编号获取省市区全称
        /// </summary>
        /// <param name="sysNo">区县编号</param>
        /// <returns>地址全称</returns>
        /// <remarks>2013-07-4 黄志勇 创建</remarks>
        private string GetFullAreaName(int sysNo)
        {
            BsArea area;
            BsArea city;
            BsArea province = BasicAreaBo.Instance.GetProvinceEntity(sysNo, out city, out area);
            return (province != null ? province.AreaName : "") + "," + (city != null ? city.AreaName : "") + "," + (area != null ? area.AreaName : "");
        }

        #endregion

        /// <summary>
        /// 订单商品毛重总和
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <returns></returns>
        /// <remarks>2016-7-25 杨浩 创建</remarks>
        public decimal TotalOrderProductWeight(int orderSysNo)
        {
            return ISoOrderDao.Instance.TotalOrderProductWeight(orderSysNo);
        }

        #region 又一城订单添加推送
        /// <summary>
        /// 根据订单号查询是否提推送过又一城详情
        /// </summary>
        /// <param name="OrderSysNo"></param>
        /// <returns></returns>
        public SoAddOrderToU1City GetU1CityEntity(int OrderSysNo)
        {
            return ISoOrderDao.Instance.GetU1CityEntity(OrderSysNo);
        }

        /// <summary>
        /// 添加一条记录状态
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int InserU1City(SoAddOrderToU1City entity)
        {
            return ISoOrderDao.Instance.InserU1City(entity);
        }

        /// <summary>
        /// 添加推送又一城返回参数记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public SoOrderToU1CityInfomation InserToU1CityInfomation(SoOrderToU1CityInfomation entity)
        {
            return ISoOrderDao.Instance.InserToU1CityInfomation(entity);
        }

        /// <summary>
        /// 查询是否存在记录
        /// </summary>
        /// <param name="ProductSysNo"></param>
        /// <param name="OrderSysNo"></param>
        /// <returns></returns>
        public int GetToU1CityInfomation(string TransactionPdSku, int OrderSysNo)
        {
            return ISoOrderDao.Instance.GetToU1CityInfomation(TransactionPdSku, OrderSysNo);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="OrderSysNo"></param>
        /// <returns></returns>
        public SoReturnOrderToU1City GetReturnOrderToU1City(int OrderSysNo)
        {
            return ISoOrderDao.Instance.GetReturnOrderToU1City(OrderSysNo);
        }
        #endregion

        /// <summary>
        /// 获取物流单号及物流编码
        /// </summary>
        /// <param name="ordersysno"></param>
        /// <returns></returns>
        /// <remarks>2015-10-23 陈海裕 创建</remarks>
        public Hyt.Model.Common.LgExpressModel GetDeliveryCodeData(int ordersysno)
        {
            return ISoOrderDao.Instance.GetDeliveryCodeData(ordersysno);
        }

        public List<SoOrder> GetAllOrderBySysNos(string SysNos)
        {
            return ISoOrderDao.Instance.GetAllOrderBySysNos(SysNos);
        }


        public void DoSoOrderQueryForYesterday(ref Pager<CBSoOrder> pager, ParaOrderFilter filter)
        {
            filter.BeginDate = DateTime.Now.AddDays(-1).Date;
            filter.EndDate = DateTime.Now.AddDays(-1).Date;
            DoSoOrderQuery(ref pager, filter);
        }

        public List<CBSoReceiveAddress> GetOrderReceiveAddressByList(string SysNos)
        {
            return ISoReceiveAddressDao.Instance.GetOrderReceiveAddressByList(SysNos);
        }

        /// <summary>
        /// 更新订单表订单积分
        /// </summary>
        /// <param name="OrderSysNo"></param>
        /// <param name="point"></param>
        public void UpdateOrderPoint(int OrderSysNo, int point)
        {
            ISoOrderDao.Instance.UpdateOrderPoint(OrderSysNo, point);
        }

        public List<CDSoOrderItem> GetSoOrderItemByWarehouseProduct(int warehouseSysNo, int productSysNo)
        {
            return ISoOrderDao.Instance.GetSoOrderItemByWarehouseProduct(warehouseSysNo, productSysNo);
        }

        /// <summary>
        /// 是否全部发货
        /// </summary>
        /// <param name="orderSysno">订单系统编号</param>
        /// <returns></returns>
        /// <remarks>2017-08-24 杨浩 创建</remarks>
        public  bool IsAllShip(int orderSysno)
        {
           return ISoOrderDao.Instance.IsAllShip(orderSysno);
        }




        /// <summary>
        /// 获取销售订单详情
        /// </summary>
        /// <param name="sysNo">订单编号</param>
        /// <returns>订单</returns>
        ///<remarks>2017-08-25 吴琨 创建
        /// </remarks>
        public WhStockOut GetEntityTo(int sysNo)
        {
            var cacheKey = string.Format("CACHE_SOORDER_{0}", sysNo);
            return Hyt.Infrastructure.Memory.MemoryProvider.Default.Get<WhStockOut>(cacheKey, 5, () =>
            {
                return ISoOrderDao.Instance.GetEntityTo(sysNo);
            }, CachePolicy.Absolute);

        }


        /// <summary>
        /// 查询销售单表
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns>2017-08-25 吴琨 创建</returns>
        public SoOrder GetModel(int sysNo)
        {
            return ISoOrderDao.Instance.GetModel(sysNo);
        }

        /// <summary>
        /// 根据订单编号修改支付方式
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public  bool UpPayTypeSysNo(int id, int PayTypeSysNo)
        {
            return ISoOrderDao.Instance.UpPayTypeSysNo(id, PayTypeSysNo);
        }
    }
}

