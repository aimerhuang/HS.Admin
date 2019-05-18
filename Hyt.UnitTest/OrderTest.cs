using System;
using System.Collections.Generic;
using System.Linq;
using Hyt.DataAccess.RMA;
using Hyt.Model.Parameter;
using Hyt.Model.WorkflowStatus;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hyt.Model;
using System.Collections;
using Hyt.BLL.CRM;
using Newtonsoft.Json.Linq;
using Hyt.BLL.Base;
using Hyt.BLL.Order;
using Hyt.Model.SystemPredefined;
using Hyt.BLL.SellBusiness;
using Hyt.BLL.ApiPay;
using Hyt.BLL.ApiFactory;

namespace Hyt.UnitTest
{
    /// <summary>
    /// 订单测试
    /// </summary>
    [TestClass]
    public class OrderTest
    {
        //使用 ClassInitialize 在运行类中的第一个测试前先运行代码
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            DataProviderBo.Set(Activator.CreateInstance(Type.GetType("Hyt.DataAccess.Oracle.DataProvider,Hyt.DataAccess.Oracle")));

        }

        public TestContext TestContext { get; set; }


        [TestMethod]
        public void CalculateOutStockItemAmount()
        {
          var order = BLL.Web.SoOrderBo.Instance.GetEntity(42366);
          Hyt.BLL.Order.SoOrderBo.Instance.CalculateOutStockItemAmount(order, order.OrderItemList,order.OrderItemList);
        }


         [TestMethod]
        public void BatchPayOrder()
        {
            IPayProvider instance = ApiProviderFactory.GetPayInstance((int)CommonEnum.PayCode.钱袋宝);

            string sysnoList = "33772";

            var orders = BLL.Order.SoOrderBo.Instance.GetAllOrderBySysNos(sysnoList);

            foreach (var o in orders)
            {
                var orderPayLogInfo = BLL.Order.SoOrderPayLogBo.Instance.GetOrderPayLogByOrderSysNo(o.SysNo);
                if (orderPayLogInfo != null)
                    continue;
               instance.SubmitOrderToPay(o);
            }
       }

        /// <summary>
        /// 分页
        /// </summary>
        [TestMethod]
        public void SoPagerTest()
        {

            var orderInfo=Hyt.BLL.Web.SoOrderBo.Instance.GetEntity(4347);
            //var orderItem=Hyt.BLL.Web.SoOrderBo.Instance.GetOrderItemListByOrderSysNo(4347);


            orderInfo.ReceiveAddress = SoOrderBo.Instance.GetOrderReceiveAddress(orderInfo.ReceiveAddressSysNo);
            orderInfo.Customer = Hyt.BLL.CRM.CrCustomerBo.Instance.GetModel(orderInfo.CustomerSysNo);

            orderInfo.OnlinePayment = Hyt.BLL.Finance.FinanceBo.Instance.GetOnlinePaymentBySourceSysNo(Model.WorkflowStatus.FinanceStatus.网上支付单据来源.销售单, orderInfo.SysNo);

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(orderInfo);

            var _orderInfo=Hyt.Util.Serialization.JsonUtil.ToObject<SoOrder>(json);

          

            //var pager = new Pager<CBSoOrder>();
            //var filter = new Model.Parameter.ParaOrderFilter
            //    {

            //    };

            //Hyt.BLL.Order.SoOrderBo.Instance.DoSoOrderQuery(ref pager, filter);

            //Assert.IsTrue(pager.Rows.Count > 0);
        }

        /// <summary>
        /// 写日志
        /// </summary>
        [TestMethod]
        public void SoLogTest()
        {
            Hyt.BLL.SellBusiness.CrCustomerRebatesRecordBo.Instance.CrCustomerRebatesRecordToCustomer(14);

            //Hyt.Util.FormatUtil.FormatCurrency("fds", 2);
            //Hyt.BLL.Order.SoOrderBo.Instance.WriteSoTransactionLog("1", "审核通过", "苍老师");
        }

        /// <summary>
        /// 分页
        /// </summary>
        [TestMethod]
        public void OutStockOrderPagerTest()
        {
            var filter = new Model.Parameter.ParaOutStockOrderFilter
                {

                };

            var pager = Hyt.BLL.Order.ShopOrderBo.Instance.GetOutStockOrders(filter);

            Assert.IsTrue(pager.Rows.Count > 0);
        }

        [TestMethod]
        public void FinanceQueryTest()
        {
            var filter = new Model.Parameter.ParaVoucherFilter
                {
                    Status = (int)Model.WorkflowStatus.FinanceStatus.收款单状态.作废
                };
            var r = Hyt.BLL.Finance.FinanceBo.Instance.GetReceiptVouchers(filter);

            TestContext.WriteLine("记录总数:" + r.Rows.Count);
        }

        [TestMethod]
        public void OtherTest()
        {
            //var rcReturns = IRcReturnDao.Instance.GetAll(filter).Rows.ToList();
            List<CBRcReturn> rcReturns = new List<CBRcReturn>
                {
                    new CBRcReturn() {Status = 1},
                    new CBRcReturn() {Status = 10}
                };

            var rmaStatuses = new[]
                {
                    (int) RmaStatus.退换货状态.待入库,
                    (int) RmaStatus.退换货状态.待审核,
                    (int) RmaStatus.退换货状态.待退款
                };

            TestContext.WriteLine(rcReturns.Exists(o => rmaStatuses.Contains(o.Status)).ToString());
        }

        /// <summary>
        /// 返利冻结转完结
        /// </summary>
        [TestMethod]
        public void CrCustomerRebatesRecordToCustomer()
        {
            int delayDay=-2;
            int dealerSysNo = 0;

       
            CrCustomerRebatesRecordBo.Instance.CrCustomerRebatesRecordToCustomer(delayDay, dealerSysNo,6011);
        }
        /// <summary>
        /// 自动确认收货
        /// </summary>
        /// <param name="timeOutDay">订单超时天数</param>
        /// <remarks>2016-5-13 杨浩 创建</remarks>
        [TestMethod]
        public void AutoConfirmationOfReceipt()
        {
            //var orderList = GetConfirmReceiptOrderSysNoList(timeOutDay);
            var orderList = BLL.Order.SoOrderBo.Instance.GetOrderListBySysNos("6011");
            foreach (var orderInfo in orderList)
            {
                Hyt.BLL.Order.SoOrderBo.Instance.UpdateOrderStatusAndOnlineStatus(orderInfo, "系统",
                       Constant.OlineStatusType.已完成, (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.已完成);

            }
        }

        
       



        [TestMethod]
        public void CreateOrderTest()
        {
            var target = new CrShoppingCartBo();

            //target.Add(10,)
            var shoppingCart = new CrShoppingCart()
            {
                SettlementAmount=400,
                TaxFee=0,
                ProductAmount=400,
                FreightAmount=0,
         
                ShoppingCartGroups = new List<CrShoppingCartGroup>()
                {
                    new CrShoppingCartGroup()
                    {
                        ShoppingCartItems=new List<CBCrShoppingCartItem>()
                        {
                            new CBCrShoppingCartItem()
                            {
                                Catle=100,
                                UnitCatle=50,
                                ProductSysNo=5804,
                                IsLock=0,
                                SalesUnitPrice=200,
                                SaleTotalAmount=400,
                                Quantity=2,

                            },
                        },
                    },
                },
            };
            //CrShoppingCart shoppingCart = target.GetShoppingCart(new[] { PromotionStatus.促销使用平台.PC商城 }, 1061);

            var so = Hyt.BLL.Order.SoOrderBo.Instance.CreateOrder(15762, 15762, new SoReceiveAddress() 
            {
                Name = "杨浩",
                Gender = 0,
                MobilePhoneNumber = "13719648105",
                AreaSysNo = 441302,
                StreetAddress = "花边北路11号"

            },38,13,12,shoppingCart,0,null,OrderStatus.销售单来源.手机商城
                , null, OrderStatus.销售方式.普通订单, null, OrderStatus.销售单对用户隐藏.否, "客户留言", "对内备注", "配送备注", "周末送货"
                , OrderStatus.配送前是否联系.否,"订单备注",null,null,0);

            Assert.IsTrue(so.SysNo > 0);

        }

         /// <summary>
         /// 创建出库单
         /// </summary>
         /// <returns></returns>
        [TestMethod]
        public void CreateOutStock()
        {

            int orderSysno = 42564;
            var soItems = SoOrderBo.Instance.GetOrderItemsByOrderId(orderSysno);

            var so = Hyt.BLL.Order.SoOrderBo.Instance.GetEntity(orderSysno);


            IList<Model.SoOrderItem> datas=new List<Model.SoOrderItem>();
            foreach (var item in soItems)
            {              
                item.RealStockOutQuantity = 2;
                item.SalesAmount = item.RealStockOutQuantity * item.SalesUnitPrice;
                datas.Add(item);
            }
            int warehouseSysNo = so.DefaultWarehouseSysNo;
            SyUser user = new SyUser() { SysNo=0};
            int? outstockdeliveryTypeSysNo = so.DeliveryTypeSysNo;


            if (
                !(so.Status == (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.待创建出库单 ||
                  so.Status == (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.部分创建出库单))
            {
                throw new Exception("当前状态下的订单不允许出库。");
            }
       
            var payType = Hyt.BLL.Basic.PaymentTypeBo.Instance.GetEntity(so.PayTypeSysNo);
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
                CreatedBy =0,
                CreatedDate = DateTime.Now,
                ReceiveAddressSysNo = so.ReceiveAddressSysNo,
                CustomerMessage = so.CustomerMessage,
                DeliveryRemarks = so.DeliveryRemarks,
                DeliveryTime = so.DeliveryTime,
                DeliveryTypeSysNo = currectDeliveryTypeSysNo,
                IsCOD = payType == null ? 1 : payType.PaymentType == (int)Hyt.Model.WorkflowStatus.BasicStatus.支付方式类型.到付 ? 1 : 0,
                IsPrintedPackageCover = 0,
                IsPrintedPickupCover = 0,
                LastUpdateBy = 0,
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
            var outStockItemAmount =BLL.Order.SoOrderBo.Instance.CalculateOutStockItemAmount(so, soItems, datas);
            whStockOut.StockOutAmount = outStockItemAmount.Sum(m => m.Value);
            //note:调用保存出库单主表的方法
            //whStockOut.SysNo = Hyt.DataAccess.Warehouse.IOutStockDao.Instance.InsertMain(whStockOut); //朱成果 添加
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
                //whStockOutItem.SysNo = Hyt.DataAccess.Warehouse.IOutStockDao.Instance.InsertItem(whStockOutItem); //朱成果 添加
                var soItem = soItems.First(p => p.SysNo == data.SysNo);
                //更新当前出库明细中的出库数量
                soItem.RealStockOutQuantity += data.Quantity;
                //出库数量到数据库
                //Hyt.DataAccess.Order.ISoOrderItemDao.Instance.UpdateOutStockQuantity(soItem.SysNo,
                                                                                     //soItem.RealStockOutQuantity);
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
            //UpdateOrder(so); //更新订单 余勇 修改为调用业务层方法 //Hyt.DataAccess.Order.ISoOrderDao.Instance.Update(so); //更新订单状态，默认出库仓库

            // Hyt.DataAccess.Order.ISoOrderDao.Instance.UpdateOrderStatus(so.SysNo, so.Status); //更新订单出库状态
            //if (so.Status == (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单状态.已创建出库单)
            //{
            //    //将已处理的订单在任务池删除
            //    SyJobPoolManageBo.Instance.DeleteJobPool(so.SysNo, (int)SystemStatus.任务对象类型.客服订单提交出库);
            //    SyJobDispatcherBo.Instance.WriteJobLog(string.Format("已创建出库单,审核任务完成，销售单编号:{0}", so.SysNo), so.SysNo, null, user.SysNo);
            //}
            //var warehouseName = WhWarehouseBo.Instance.GetWarehouseEntity(warehouseSysNo).WarehouseName;
            //WriteSoTransactionLog(so.TransactionSysNo
            //                      , string.Format(Constant.ORDER_TRANSACTIONLOG_OUTSTOCK_CREATE, warehouseName, whStockOut.SysNo)
            //                      , user.UserName);

           
        }
  

    }
}
