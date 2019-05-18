using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Extra.Erp;
using Extra.Erp.Model;
using Extra.Erp.Model.Borrowing;
using Extra.Erp.Model.Receiving;
using Extra.Erp.Model.Sale;
using Hyt.DataAccess.Provider;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using Extra.Erp.Kis;
using Extra.Erp.DataContract;
using Extra.UpGrade.SDK.JingDong.Domain;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;
using Hyt.BLL.Basic;
using Hyt.BLL.Warehouse;
using Hyt.BLL.Order;
using Hyt.BLL.Product;
using Hyt.BLL.RMA;
using Hyt.BLL.MallSeller;

namespace Hyt.UnitTest
{

    /// <summary>
    ///这是 EasCoreTest 的测试类，旨在
    ///包含所有 EasCoreTest 单元测试
    ///</summary>
    [TestClass()]
    public class EasCoreTest
    {

        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 附加测试特性
        // 
        //编写测试时，还可使用以下特性:
        //
        //使用 ClassInitialize 在运行类中的第一个测试前先运行代码
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            ProviderManager.Set<IDataProvider>(
                (IDataProvider)
                Activator.CreateInstance(Type.GetType("Hyt.DataAccess.Oracle.DataProvider,Hyt.DataAccess.Oracle")));
        }
        //
        //使用 ClassCleanup 在运行完类中的所有测试后再运行代码
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //使用 TestInitialize 在运行每个测试前先运行代码
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //使用 TestCleanup 在运行完每个测试后运行代码
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        [TestMethod()]
        public void UpdateErpProductNumber()
        {           
           var stockoutSysNos="75006,75010,75021,75022,75025,75051,75052,75053,75059,75060,75061,75062,75063,75067,75068,75073,75074,75078,75079,75080,75081,75083,75087,75088,75089,75092,75109,75137,75145,75143,75114,75131,75133,75113,75112,75115,75130,75132,75014,75091,75084,75085,75076,75020,75023,75024,75072,75077,75019,75082,75086,75064,75242,75248,75255,75256,75251,75252,75250,75253,75254,75257,75249".Split(',');

            stockoutSysNos = "75062,75063,75133,75112,75115,75130,75132,75085,75024,75077,75086,75247,75249".Split(',');
            stockoutSysNos = "75025,75074".Split(',');
            foreach(var sysno in stockoutSysNos)
           {
               int stockoutSysNo =int.Parse(sysno);
               BLL.Warehouse.WhWarehouseBo.Instance.WhStockOutToErp(stockoutSysNo);
           }        
        }

        [TestMethod()]
        public void EasInStock()
        {

            WhStockIn model = BLL.Warehouse.InStockBo.Instance.GetStockIn(1167);

            model.ItemList = BLL.Warehouse.InStockBo.Instance.GetStockInItemList(model.SysNo);
            List<SaleInfo> saleInfoList = null;
            var customer = string.Empty;
            var description = string.Empty;//订单系统编号
            if (model.ItemList != null && model.ItemList.Any())
            {
                saleInfoList = new List<SaleInfo>();

                //仓库
                var warehouse = WhWarehouseBo.Instance.GetWarehouse(model.WarehouseSysNo);
                var warehouseErpCode = string.Empty;
                string organizationCode = string.Empty;//组织机构代码
                if (warehouse != null)
                {
                    warehouseErpCode = warehouse.ErpCode;
                    var oraganization = OrganizationBo.Instance.GetOrganization(warehouse.SysNo);
                    if (oraganization != null)
                    {
                        organizationCode = oraganization.Code;
                    }
                }
                var orderSysNo = 0;
                //是否为RMA换货
                bool isRma = false;
                //商城或第三方订单号
                var orderNo = string.Empty;

                SoOrder order = null;
                if (model.SourceType == WarehouseStatus.入库单据类型.RMA单.GetHashCode())
                {
                    //创建Eas出库单摘要，商城订单时使用商城订单号，分销商时使用分销商订单号
                    var rmaSysNo = model.SourceSysNO;

                    var rmaInfo = BLL.RMA.RmaBo.Instance.GetRMA(rmaSysNo);
                    if (rmaInfo == null)
                    {
                        throw new Exception(string.Format("找不到编号为:{0}的RMA单", rmaSysNo));
                    }

                    order = SoOrderBo.Instance.GetEntity(rmaInfo.OrderSysNo);

                    if (order == null)
                    {
                        throw new Exception(string.Format("找不到编号为:{0}的订单", rmaInfo.OrderSysNo));
                    }
                    var dealerInfo = BLL.Distribution.DsDealerBo.Instance.GetDsDealer(order.DealerSysNo);
                    organizationCode = dealerInfo.CreatedBy.ToString();
                }



                //用户账号
                string account = string.Empty;

                foreach (var x in model.ItemList)
                {
                    var product = PdProductBo.Instance.GetProduct(x.ProductSysNo);
                    if (product == null) return;
                    var price = decimal.Zero;

                    //取得退货单
                    if (model.SourceType == WarehouseStatus.入库单据类型.RMA单.GetHashCode())
                    {
                        var rma = RmaBo.Instance.GetRMA(model.SourceSysNO);

                        if (rma != null && rma.RMAItems != null && rma.RMAItems.Any())
                        {
                            isRma = (rma.RmaType == (int)Model.WorkflowStatus.RmaStatus.RMA类型.售后换货);
                            orderSysNo = rma.OrderSysNo;
                            //取得退货单对应商品
                            var item = rma.RMAItems.FirstOrDefault(c => c.SysNo == x.SourceItemSysNo);
                            if (item != null && item.RmaQuantity > 0)
                            {
                                if (item.RefundProductAmount < 0)//退货分摊为负，不导入
                                    continue;
                                if (rma.DeductedInvoiceAmount > 0)//如果存在扣发票金额，将其分摊到明细
                                {
                                    item.RefundProductAmount = decimal.Round(item.RefundProductAmount - (item.RefundProductAmount / rma.RefundProductAmount) * rma.DeductedInvoiceAmount, 2);
                                }

                                if (rma.RefundCoin > 0)//如果存在实退会员币，将其分摊到明细
                                {
                                    item.RefundProductAmount = decimal.Round(item.RefundProductAmount - (item.RefundProductAmount / rma.RefundProductAmount) * rma.RefundCoin, 2);
                                }

                                price = (decimal)x.RealStockInQuantity / (decimal)item.RmaQuantity * item.RefundProductAmount;//退货价为实退金额除退货数量
                            }
                            if (item != null && isRma) //如果为售后换货取原单实际销售金额
                            {
                                var whStockOut = BLL.Warehouse.WhWarehouseBo.Instance.GetWhStockOutItem(item.StockOutItemSysNo);
                                var itx = Hyt.BLL.RMA.RmaBo.Instance.GetSoReturnOrderItem(whStockOut.OrderItemSysNo);
                                if (itx != null)//换货生成的新订单再次换货
                                    price = decimal.Round(itx.FromStockOutItemAmount * ((decimal)x.RealStockInQuantity / whStockOut.ProductQuantity), 2);
                                else//原订单换货
                                    price = decimal.Round(whStockOut.RealSalesAmount * ((decimal)x.RealStockInQuantity / whStockOut.ProductQuantity), 2);
                            }
                        }
                    }
                    else if (model.SourceType == WarehouseStatus.入库单据类型.出库单.GetHashCode())
                    {
                        //出库单中商品
                        var itemList = WhWarehouseBo.Instance.GetWhStockOutItemList(model.SourceSysNO);
                        if (itemList != null && itemList.Any())
                        {
                            orderSysNo = itemList.First().OrderSysNo;
                            //customer = WhWarehouseBo.Instance.GetErpCustomerCode(itemList.First().OrderSysNo);
                            var item = itemList.FirstOrDefault(c => c.SysNo == x.SourceItemSysNo);
                            if (item != null && item.RealSalesAmount > 0 && item.ProductQuantity > 0)
                            {
                                price = decimal.Round(x.RealStockInQuantity / (decimal)item.ProductQuantity * item.RealSalesAmount, 2);
                            }
                        }
                    }

                    saleInfoList.Add(new SaleInfo()
                    {
                        ErpCode = product.ErpCode,
                        Quantity = x.RealStockInQuantity,
                        WarehouseNumber = warehouseErpCode,
                        WarehouseSysNo = model.WarehouseSysNo,
                        OrganizationCode = organizationCode,
                        Amount = price,
                        DiscountAmount = decimal.Zero,
                        IsPresent = isRma ? 0 : (price == 0 ? 1 : 0), //价格为0的商品，传入Eas为赠品
                        ItemID = model.SysNo,
                    });

                };

                //如果入库单为升舱订单就获取导入分销订单号
                //var order = SoOrderBo.Instance.GetEntity(orderSysNo);
                orderNo = orderSysNo.ToString();
                if (order != null)
                {
                    if (order.OrderSource ==Hyt.Model.WorkflowStatus.OrderStatus.销售单来源.分销商升舱.GetHashCode())
                    {
                        var dsOrders = Hyt.BLL.MallSeller.DsOrderBo.Instance.GetEntityByHytOrderID(orderSysNo);
                        if (dsOrders != null && dsOrders.Count > 0)
                            orderNo = string.Join(";", dsOrders.Select(o => o.MallOrderId));
                    }
                }
                customer = WhWarehouseBo.Instance.GetErpCustomerCode(orderSysNo);
                description = orderNo;
               // var client = Extra.Erp.Kis.KisProviderFactory.CreateProvider();
               // client.SaleInStock(saleInfoList, customer, description, order == null ? string.Empty : order.TransactionSysNo);
            }
        }

        [TestMethod()]
        public void SaleOutStock()
        {
            var list=Hyt.BLL.Sys.EasBo.Instance.GetNoSyncStockOutList(47);
            foreach (var item in list)
            {
                UpdateErpProductNumber(item.SysNo);
            }         
        }
        /// <summary>
        /// 获取分销商，分销订单号
        /// </summary>
        /// <param name="order">订单</param>
        /// <returns>分销订单号</returns>
        private string GetDsOrderSysNo(SoOrder order)
        {
            var orderSysNo = order.SysNo.ToString();
            if (order.OrderSource == Hyt.Model.WorkflowStatus.OrderStatus.销售单来源.分销商升舱.GetHashCode())
            {
                var dsOrders = DsOrderBo.Instance.GetEntityByHytOrderID(order.SysNo);
                if (dsOrders == null || dsOrders.Count == 0)
                {
                    throw new Exception(string.Format("找不到系统编号为{0}的分销商商城", order.OrderSourceSysNo));
                }
                orderSysNo = string.Join(";", dsOrders.Select(o => o.MallOrderId));
            }
            return orderSysNo;
        }
        public void UpdateErpProductNumber(int stockoutSysNo)
        {
            try
            {
                var stockout = WhWarehouseBo.Instance.Get(stockoutSysNo);
                if (stockout == null)
                {
                    throw new HytException(string.Format("找不到编号为:{0}的出库单", stockoutSysNo));
                }
                var warehouse =Hyt.BLL.Warehouse.WhWarehouseBo.Instance.GetWarehouse(stockout.WarehouseSysNo);
                string organizationCode = string.Empty;//组织机构代码
                //if (warehouse != null)
                //{
                //    var oraganization = OrganizationBo.Instance.GetOrganization(warehouse.SysNo);
                //    if (oraganization != null)
                //    {
                //        organizationCode = oraganization.Code;
                //    }
                //}
                //创建Eas出库单摘要，商城订单时使用商城订单号，分销商时使用分销商订单号
                var orderSysNo = stockout.OrderSysNO.ToString();
                //事务编号
                var transactionSysNo = stockout.TransactionSysNo;
                var order = SoOrderBo.Instance.GetEntity(stockout.OrderSysNO);
                if (order == null)
                {
                    throw new Exception(string.Format("找不到编号为:{0}的订单", stockout.OrderSysNO));
                }
                var dealerInfo = BLL.Distribution.DsDealerBo.Instance.GetDsDealer(order.DealerSysNo);
                organizationCode = dealerInfo.CreatedBy.ToString();

                //获取分销商，分销订单号
                orderSysNo = GetDsOrderSysNo(order);
                //是否为RMA换货下单
                bool isRma = order.OrderSource == (int)Model.WorkflowStatus.OrderStatus.销售单来源.RMA下单;
                IList<SoReturnOrderItem> soReturnOrderItem = new List<SoReturnOrderItem>();
                if (isRma)
                {
                    soReturnOrderItem = Hyt.BLL.RMA.RmaBo.Instance.GetSoReturnOrderItem(order.TransactionSysNo);
                    var rma = Hyt.BLL.RMA.RmaBo.Instance.GetRcReturnEntity(order.OrderSourceSysNo);
                    if (rma != null)
                    {
                        orderSysNo = rma.OrderSysNo.ToString();
                        var rmaOrder = SoOrderBo.Instance.GetEntity(rma.OrderSysNo);
                        transactionSysNo = rmaOrder.TransactionSysNo;
                        orderSysNo = GetDsOrderSysNo(rmaOrder);
                    }
                }
                string deliveryTypeName = Hyt.BLL.Logistics.DeliveryTypeBo.Instance.GetDeliveryType(stockout.DeliveryTypeSysNo).DeliveryTypeName;
                string account = BLL.CRM.CrCustomerBo.Instance.GetModel(order.CustomerSysNo).Account;
                //获取分销商erp编码
                var customerErpCode = Hyt.BLL.Warehouse.WhWarehouseBo.Instance.GetErpCustomerCode(stockout.OrderSysNO);
                var query = new List<SaleInfo>();

                foreach (var product in stockout.Items.Where(x => x.Status == WarehouseStatus.出库单明细状态.有效.GetHashCode()))
                {
                    var erpCode = PdProductBo.Instance.GetProductErpCode(product.ProductSysNo);
                    //如果为换货下单取原销售金额
                    decimal rmaAmount = 0;
                    if (isRma)
                    {
                        var model = soReturnOrderItem.SingleOrDefault(x => x.OrderItemSysNo == product.OrderItemSysNo);
                        if (model != null)
                            rmaAmount = model.FromStockOutItemAmount * ((decimal)model.OrderItemQuantity / model.FromStockOutItemQuantity);
                    }
                    query.Add(
                        new SaleInfo
                        {
                            ErpCode = erpCode,
                            Quantity = product.ProductQuantity,
                            WarehouseNumber = warehouse.ErpCode,
                            WarehouseSysNo = stockout.WarehouseSysNo,
                            OrganizationCode = organizationCode,
                            Amount = isRma ? (rmaAmount) : product.RealSalesAmount, //2013-11-26 吴文强 修改为商品实际销售金额合计
                            DiscountAmount = 0,
                            IsPresent = isRma ? 0 : (product.RealSalesAmount == 0 ? 1 : 0),
                            //2013-11-26 吴文强 价格为0的商品，传入Eas为赠品
                            Remark = account + "," + deliveryTypeName + (isRma ? "(换货)" : ""),
                            ItemID = stockoutSysNo,
                            DeliveryCustomer = dealerInfo.ErpCode
                        });
                }

                //var client = KisProviderFactory.CreateProvider();
                //client.SaleOutStock(query, customerErpCode, orderSysNo, transactionSysNo);
                //SysLog.Instance.Info(LogStatus.系统日志来源.后台, "配送修改Eas库存", LogStatus.系统日志目标类型.EAS, stockoutSysNo, Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
            }
            catch (Exception ex)
            {
               // SysLog.Instance.Error(LogStatus.系统日志来源.后台, "配送修改Eas库存" + stockoutSysNo, LogStatus.系统日志目标类型.EAS, stockoutSysNo, ex);
            }
        }


         /// <summary>
        /// 获取Kis全部计量单位
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-11-22 杨浩 添加</remarks>
        [TestMethod()]
        public void GetAllUnit()
        {
            var client = KisProviderFactory.CreateProvider();
            var units=client.GetAllUnit();
            Assert.AreEqual(true, units.Status);
        }
        /// <summary>
        ///库存 的测试
        ///</summary>
        [TestMethod()]
        public void WebInventory()
        {

            
            var client = KisProviderFactory.CreateProvider();
            //var b = client.GetInventory("",new[] { "031160404051", "031130104001", "031120101032" }, "BSC028001",1);
            var b = client.GetInventory("", new[] { "01.01.01.03", "01.01.01.02" }, "05.001", 1);
            Assert.AreEqual(true, b.Status);
            //var s = Hyt.Util.EncryptionUtil.EncryptWithMd5AndSalt("pshyt");
        }

        /// <summary>
        ///借货、还货 的异步测试
        ///</summary>
        [TestMethod()]
        public void AsynBorrowTest()
        {
            var query = new List<BorrowInfo>
                {
                    new BorrowInfo
                        {
                            ErpCode = "031160203008",
                            Quantity = 1,
                            WarehouseNumber = "BSC073301"
                        }
                };
            var client = EasProviderFactory.CreateProvider();
            client.Borrow(query, "JC[428]-[杨冠军]", "Test000013");
        }

        /// <summary>
        ///借货、还货 的测试
        ///</summary>
        [TestMethod()]
        public void ErpOtherIssueBillFacadeTest()
        {
            var query = new List<BorrowInfo>
                {
                    new BorrowInfo
                        {
                            ErpCode = "031110111002",
                            Quantity = 1,
                            WarehouseNumber = "ZK0280001"
                        }
                };
            var client = EasProviderFactory.CreateProvider();
            var b = client.Borrow(query, "","");
            Assert.AreEqual(true, b.Status);
        }

        /// <summary>J:\SVN项目\信营新系统\Admin\Hyt.UI\Hyt.Admin\Xml\
        ///销售出库、退货 的测试
        ///</summary>
        [TestMethod()]
        public void SaleIssueBillFacade()
        {
            var query = new List<SaleInfo>
                {
                    new SaleInfo
                        {
                            ErpCode = "01.01.02.02",//031150301001
                            Quantity = 1,
                            WarehouseNumber = "01.001",
                            Amount=(decimal)50,
                            DiscountAmount=0,
                            IsPresent=0,
                            Remark="Hyt订单号:252879,配送方式:门店自提",
                            WarehouseSysNo=22,

                        }
                };

            var client = KisProviderFactory.CreateProvider();
            var b = client.SaleOutStock(query,"3003999997", "594976745933808", "");
            Assert.AreEqual(true, b.Status);
        }

        ///// <summary>
        /////销售出库、退货 的测试
        /////</summary>
        //[TestMethod()]
        //public void SaleIssueBillFacadeCode()
        //{

        //    var pars = new Hashtable();
        //    String Url = @"http://192.168.8.8:6888/ormrpc/services/EASLogin";

        //    var wsCaller = new WebServiceCaller("xxx", "xxx");

        //    pars.Add("userName", "hytformal");
        //    pars.Add("slnName", "eas");
        //    pars.Add("dcName", "a01");
        //    pars.Add("language", "L2");

        //    string strReturnValue = wsCaller.QuerySoapWebService(Url, "logout", pars);  
        //}

        /// <summary>
        ///导入收款单据 的测试
        ///</summary>
        [TestMethod()]
        public void ReceivingBillFacade()
        {
            var query = new List<ReceivingInfo>
            {
                new ReceivingInfo
                {
                    Remark = string.Empty,
                    Amount = 150,
                    PayeeAccount = "1002.01",
                    OrganizationCode = "",
                    WarehouseNumber = "BSC028001",
                    SettlementType = "014",
                    OrderSysNo = "471199",
                     PayeeAccountBank="10089"
                }
            };
            var client = EasProviderFactory.CreateProvider();
            var b = client.Receiving(query, 收款单类型.商品收款单, "3003999997", false, "471199", "Test00004711990");
            Assert.AreEqual(true, b.Status);
        }

        /// <summary>
        /// 定时执行Eas同步队列
        /// </summary>
        [TestMethod()]
        public void Execute()
        {
            var list = Hyt.BLL.Sys.EasBo.Instance.GetSyncList(10);
            var client = Extra.Erp.EasProviderFactory.CreateProvider();
            int index = 0;
            for (; index < list.Count; index++)
            {
                var result = client.Resynchronization(list[index].SysNo);
                //StatusCode == "9999"  标示已经导入过了
                if (result.Status || result.StatusCode == "9999")
                {
                    continue;
                }
                else
                {
                    if (result.StatusCode == "9998")//传递中
                        index = index - 1;
                    index = list.FindLastIndex(x => x.FlowIdentify == list[index].FlowIdentify);
                }
            }
            Assert.AreEqual(true,true);
        }
        /// <summary>
        /// 销售单查询
        /// </summary>
        /// <param name="request">查询参数</param>
        /// <returns></returns>
        /// <remarks>2016-12-12 杨浩 创建</remarks>
        [TestMethod()]
        public void SaleSearch()
        {
            var request = new SaleSearchRequest()
            {
                FStartDate = DateTime.Now.AddYears(-10).ToString("yyyy-MM-dd"),
                FEndDate = DateTime.Now.ToString("yyyy-MM-dd"),
                PageIndex=1,
                PageSize=1,
            };

            var client =KisProviderFactory.CreateProvider();
            var result =client.SaleSearch(request);
        }
        /// <summary>
        /// 定时执行Eas同步队列
        /// </summary>
        [TestMethod()]
        public void Execute2()
        {
            var data = "55,57,59,61,63,65,67,69,71,73,75,77,79,81,83,85,87,99,101,103,105,107,109,113,115,117,119,134,165,168,171,176,178,180,182,184,258,262,290,292,294,296,297,300,301,304,305,308,310,311,314,315,318,319,321,324,326,328,330,332,339,435,545,886,901,903,909,935,963,969,975,1016,1017,1019,1020,1021,1024,1029,1039";

            var data2 = @"99,103,105,115,134,435,909,1019,101,107,109,117,119,339,886,296,300,324,326,975,290,292,294,297,301,304,308,311,963,314,319,1024,1039,315,318,321,1016,1017,1021,305,310,903,165,168,171,176,258,178,180,262,182,184,55,57,59,61,63,65,67,69,71,73,75,77,79,81,83,85,87,328,330,332,545,901,935,969,1020,1029";
            
            var arr = data.Split(',');

            var arr2 = data2.Split(',');

            var list ="";
            var list2 = "";

            foreach (var item1 in arr)
            {
                if (!arr2.Contains(item1))
                {
                    list += item1 + ",";
                }
            }

            foreach (var item1 in arr2)
            {
                if (!arr.Contains(item1))
                {
                    list2 += item1 + ",";
                }
            }
        }
    }

    public class TestData
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
