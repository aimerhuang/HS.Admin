using Hyt.BLL.Base;
using Hyt.BLL.Order;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.UnitTest
{
    /// <summary>
    ///这是 SoOrderBoTest 的测试类，旨在
    ///包含所有 SoOrderBoTest 单元测试
    ///</summary>
    [TestClass()]
    public class SoOrderBoTest
    {

        public SoOrderBoTest()
        {
            DataProviderBo.Set(Activator.CreateInstance(Type.GetType("Hyt.DataAccess.Oracle.DataProvider,Hyt.DataAccess.Oracle")));
        }
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
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
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

        /// <summary>
        ///SoOrderBo 构造函数 的测试
        ///</summary>
        [TestMethod()]
        public void SoOrderBoConstructorTest()
        {
            SoOrderBo target = new SoOrderBo();
        }

        /// <summary>
        ///AuditSoOrder 的测试
        ///</summary>
        [TestMethod()]
        public void AuditSoOrderTest()
        {
            SoOrderBo target = new SoOrderBo(); // TODO: 初始化为适当的值
            int soIdList = 0; // TODO: 初始化为适当的值
            int operatorId = 0; // TODO: 初始化为适当的值
            target.AuditSoOrder(soIdList, operatorId);

        }

        /// <summary>
        ///CancelSoOrder 的测试
        ///</summary>
        [TestMethod()]
        public void CancelSoOrderTest()
        {
            SoOrderBo target = new SoOrderBo(); // TODO: 初始化为适当的值
            int soIdList = 0; // TODO: 初始化为适当的值
            int operatorId = 0; // TODO: 初始化为适当的值
            //target.CancelSoOrder(soIdList, operatorId);

        }

        /// <summary>
        ///CreateCustomer 的测试
        ///</summary>
        [TestMethod()]
        public void CreateCustomerTest()
        {
            SoOrderBo target = new SoOrderBo(); // TODO: 初始化为适当的值
            CrCustomer customer = null; // TODO: 初始化为适当的值
            CrReceiveAddress address = null; // TODO: 初始化为适当的值
            target.CreateCustomer(customer, address);

        }

        /// <summary>
        ///GetCustomerAddressBySysNo 的测试
        ///</summary>
        [TestMethod()]
        public void GetCustomerAddressBySysNoTest()
        {
            SoOrderBo target = new SoOrderBo(); // TODO: 初始化为适当的值
            int sysno = 0; // TODO: 初始化为适当的值
            CrReceiveAddress expected = null; // TODO: 初始化为适当的值
            CrReceiveAddress actual;
            actual = target.GetCustomerAddressBySysNo(sysno);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///GetEntity 的测试
        ///</summary>
        [TestMethod()]
        public void GetEntityTest()
        {
            SoOrderBo target = new SoOrderBo(); // TODO: 初始化为适当的值
            int sysNo = 0; // TODO: 初始化为适当的值
            SoOrder expected = null; // TODO: 初始化为适当的值
            SoOrder actual;
            actual = target.GetEntity(sysNo);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///GetOrderReceiveAddress 的测试
        ///</summary>
        [TestMethod()]
        public void GetOrderReceiveAddressTest()
        {
            SoOrderBo target = new SoOrderBo(); // TODO: 初始化为适当的值
            int sysNo = 0; // TODO: 初始化为适当的值
            SoReceiveAddress expected = null; // TODO: 初始化为适当的值
            SoReceiveAddress actual;
            actual = target.GetOrderReceiveAddress(sysNo);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///GetProvinceEntity 的测试
        ///</summary>
        [TestMethod()]
        public void GetProvinceEntityTest()
        {
            SoOrderBo target = new SoOrderBo(); // TODO: 初始化为适当的值
            int sysNo = 901002; // TODO: 初始化为适当的值
            BsArea cityEntity = null; // TODO: 初始化为适当的值
            BsArea cityEntityExpected = null; // TODO: 初始化为适当的值
            BsArea areaEntity = null; // TODO: 初始化为适当的值
            BsArea areaEntityExpected = null; // TODO: 初始化为适当的值
            BsArea expected = null; // TODO: 初始化为适当的值
            BsArea actual;
            actual = target.GetProvinceEntity(sysNo, out cityEntity, out areaEntity);
            Assert.AreEqual(cityEntityExpected, cityEntity);
            Assert.AreEqual(areaEntityExpected, areaEntity);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///GetSoByStatus 的测试
        ///</summary>
        [TestMethod()]
        public void GetSoByStatusTest()
        {
            SoOrderBo target = new SoOrderBo(); // TODO: 初始化为适当的值
            Pager<CBSoOrder> pager = null; // TODO: 初始化为适当的值
            Pager<CBSoOrder> pagerExpected = null; // TODO: 初始化为适当的值
            int orderStatus = 0; // TODO: 初始化为适当的值
            Nullable<int> operatorId = new Nullable<int>(); // TODO: 初始化为适当的值
            List<CBSoOrder> expected = null; // TODO: 初始化为适当的值
            List<CBSoOrder> actual;
            //actual = target.GetSoByStatus(ref pager, orderStatus, operatorId);
            Assert.AreEqual(pagerExpected, pager);
            //Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///GetSoForAdvancedSearch 的测试
        ///</summary>
        [TestMethod()]
        public void GetSoForAdvancedSearchTest()
        {
            SoOrderBo target = new SoOrderBo(); // TODO: 初始化为适当的值
            Pager<CBSoOrder> pager = null; // TODO: 初始化为适当的值
            Pager<CBSoOrder> pagerExpected = null; // TODO: 初始化为适当的值
            string receiveName = string.Empty; // TODO: 初始化为适当的值
            string receiveTel = string.Empty; // TODO: 初始化为适当的值
            string creatorSysNo = string.Empty; // TODO: 初始化为适当的值
            string auditorSysNo = string.Empty; // TODO: 初始化为适当的值
            string productName = string.Empty; // TODO: 初始化为适当的值
            string whStockOutSysNo = string.Empty; // TODO: 初始化为适当的值
            string minOrderAmount = string.Empty; // TODO: 初始化为适当的值
            string maxOrderAmount = string.Empty; // TODO: 初始化为适当的值
            string beginDate = string.Empty; // TODO: 初始化为适当的值
            string endDate = string.Empty; // TODO: 初始化为适当的值
            string soId = string.Empty; // TODO: 初始化为适当的值
            string orderSource = string.Empty; // TODO: 初始化为适当的值
            string payTypeSysNo = string.Empty; // TODO: 初始化为适当的值
            string deliveryTypeSysNo = string.Empty; // TODO: 初始化为适当的值
            Nullable<int> operatorId = new Nullable<int>(); // TODO: 初始化为适当的值
            List<CBSoOrder> expected = null; // TODO: 初始化为适当的值
            List<CBSoOrder> actual;
            //actual = target.GetSoForAdvancedSearch(ref pager, receiveName, receiveTel, creatorSysNo, auditorSysNo, productName, whStockOutSysNo, minOrderAmount, maxOrderAmount, beginDate, endDate, soId, orderSource, payTypeSysNo, deliveryTypeSysNo, operatorId);
            Assert.AreEqual(pagerExpected, pager);
            //Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///GetSoForQuickSearch 的测试
        ///</summary>
        [TestMethod()]
        public void GetSoForQuickSearchTest()
        {
            SoOrderBo target = new SoOrderBo(); // TODO: 初始化为适当的值
            Pager<CBSoOrder> pager = new Pager<CBSoOrder>();
            Pager<CBSoOrder> pagerExpected = null; // TODO: 初始化为适当的值
            IDictionary<string, object> filter = null; // TODO: 初始化为适当的值
            string condition = string.Empty; // TODO: 初始化为适当的值
            Nullable<int> operatorId = new Nullable<int>(); // TODO: 初始化为适当的值
            List<CBSoOrder> expected = null; // TODO: 初始化为适当的值
            List<CBSoOrder> actual;
            //actual = target.GetSoForQuickSearch(ref pager, condition,filter, operatorId);
            Assert.AreEqual(pagerExpected, pager);
            //Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///GetSoOrders 的测试
        ///</summary>
        [TestMethod()]
        public void GetSoOrdersTest()
        {
            SoOrderBo target = new SoOrderBo(); // TODO: 初始化为适当的值
            Pager<CBSoOrder> pager = null; // TODO: 初始化为适当的值
            Pager<CBSoOrder> pagerExpected = null; // TODO: 初始化为适当的值
            IDictionary<string, object> filter = null; // TODO: 初始化为适当的值
            Nullable<int> operatorId = new Nullable<int>(); // TODO: 初始化为适当的值
            //target.GetSoOrders(ref pager, filter, operatorId);
            Assert.AreEqual(pagerExpected, pager);

        }

        /// <summary>
        ///LoadAllDeliveryType 的测试
        ///</summary>
        [TestMethod()]
        public void LoadAllDeliveryTypeTest()
        {
            SoOrderBo target = new SoOrderBo(); // TODO: 初始化为适当的值
            IList<LgDeliveryType> expected = null; // TODO: 初始化为适当的值
            IList<LgDeliveryType> actual;
            actual = target.LoadAllDeliveryType();
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///LoadAllPayType 的测试
        ///</summary>
        [TestMethod()]
        public void LoadAllPayTypeTest()
        {
            SoOrderBo target = new SoOrderBo(); // TODO: 初始化为适当的值
            IList<BsPaymentType> expected = null; // TODO: 初始化为适当的值
            IList<BsPaymentType> actual;
            actual = target.LoadAllPayType();
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///LoadArea 的测试
        ///</summary>
        [TestMethod()]
        public void LoadAreaTest()
        {
            SoOrderBo target = new SoOrderBo(); // TODO: 初始化为适当的值
            int citySysNo = 0; // TODO: 初始化为适当的值
            IList<BsArea> expected = null; // TODO: 初始化为适当的值
            IList<BsArea> actual;
            actual = target.LoadArea(citySysNo);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///LoadCity 的测试
        ///</summary>
        [TestMethod()]
        public void LoadCityTest()
        {
            SoOrderBo target = new SoOrderBo(); // TODO: 初始化为适当的值
            int provinceSysNo = 0; // TODO: 初始化为适当的值
            IList<BsArea> expected = null; // TODO: 初始化为适当的值
            IList<BsArea> actual;
            actual = target.LoadCity(provinceSysNo);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///LoadCustomerAddress 的测试
        ///</summary>
        [TestMethod()]
        public void LoadCustomerAddressTest()
        {
            SoOrderBo target = new SoOrderBo(); // TODO: 初始化为适当的值
            int customerSysNo = 0; // TODO: 初始化为适当的值
            IList<CrReceiveAddress> expected = null; // TODO: 初始化为适当的值
            IList<CrReceiveAddress> actual;
            actual = target.LoadCustomerAddress(customerSysNo);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///LoadDeliveryTypeByAreaNo 的测试
        ///</summary>
        [TestMethod()]
        public void LoadDeliveryTypeByAreaNoTest()
        {
            SoOrderBo target = new SoOrderBo(); // TODO: 初始化为适当的值
            int areaNo = 0; // TODO: 初始化为适当的值
            IList<LgDeliveryType> expected = null; // TODO: 初始化为适当的值
            IList<LgDeliveryType> actual;
            actual = target.LoadDeliveryTypeByAreaNo(areaNo);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///LoadPayTypeListByDeliverySysNo 的测试
        ///</summary>
        [TestMethod()]
        public void LoadPayTypeListByDeliverySysNoTest()
        {
            SoOrderBo target = new SoOrderBo(); // TODO: 初始化为适当的值
            int deliverySysNo = 0; // TODO: 初始化为适当的值
            IList<BsPaymentType> expected = null; // TODO: 初始化为适当的值
            IList<BsPaymentType> actual;
            actual = target.LoadPayTypeListByDeliverySysNo(deliverySysNo);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///LoadProvince 的测试
        ///</summary>
        [TestMethod()]
        public void LoadProvinceTest()
        {
            SoOrderBo target = new SoOrderBo(); // TODO: 初始化为适当的值
            IList<BsArea> expected = null; // TODO: 初始化为适当的值
            IList<BsArea> actual;
            actual = target.LoadProvince();
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///SaveOrder 的测试
        ///</summary>
        [TestMethod()]
        public void SaveOrderTest()
        {
            SoOrderBo target = new SoOrderBo(); // TODO: 初始化为适当的值
            SoOrder soOrder = null; // TODO: 初始化为适当的值
            bool expected = false; // TODO: 初始化为适当的值
            bool actual;
            actual = target.SaveOrder(soOrder);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///SearchCustomer 的测试
        ///</summary>
        [TestMethod()]
        public void SearchCustomerTest()
        {
            SoOrderBo target = new SoOrderBo(); // TODO: 初始化为适当的值
            string word = string.Empty; // TODO: 初始化为适当的值
            IList<CrCustomer> expected = null; // TODO: 初始化为适当的值
            IList<CrCustomer> actual;
            actual = target.SearchCustomer(word);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///SearchCustomer 的测试
        ///</summary>
        [TestMethod()]
        public void SearchCustomerTest1()
        {
            SoOrderBo target = new SoOrderBo(); // TODO: 初始化为适当的值
            int sysNo = 0; // TODO: 初始化为适当的值
            CrCustomer expected = null; // TODO: 初始化为适当的值
            CrCustomer actual;
            actual = target.SearchCustomer(sysNo);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///WriteSoTransactionLog 的测试
        ///</summary>
        [TestMethod()]
        public void WriteSoTransactionLogTest()
        {
            SoOrderBo target = new SoOrderBo(); // TODO: 初始化为适当的值
            string transactionSysNo = string.Empty; // TODO: 初始化为适当的值
            string content = string.Empty; // TODO: 初始化为适当的值
            string oper = string.Empty; // TODO: 初始化为适当的值
            target.WriteSoTransactionLog(transactionSysNo, content, oper);

        }

        /// <summary>
        ///SaveSoOrder 的测试
        ///</summary>
        [TestMethod()]
        public void SaveSoOrderTest()
        {
            SoOrderBo target = new SoOrderBo(); // TODO: 初始化为适当的值
            SoOrder soOrder = new SoOrder { CustomerSysNo=1, OrderAmount=10 }; // TODO: 初始化为适当的值
            SoReceiveAddress soReceiveAddress = new SoReceiveAddress {  AreaSysNo=1, Name="yy" }; // TODO: 初始化为适当的值
            SoOrderItem[] product = { new SoOrderItem { ProductName = "zz" }, new SoOrderItem { ProductName = "xx" } }; // TODO: 初始化为适当的值
            FnInvoice invoice = null; // TODO: 初始化为适当的值
            bool expected = false; // TODO: 初始化为适当的值
            bool actual;
            //actual = target.SaveSoOrder(soOrder, soReceiveAddress, product, invoice);
            //Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///CreateShopOrder 的测试
        ///</summary>
        [TestMethod()]
        public void CreateShopOrderTest()
        {
            SoOrderBo target = new SoOrderBo(); // TODO: 初始化为适当的值
            int shopSysNo = 1; // TODO: 初始化为适当的值
            int customerSysNo = 1; // TODO: 初始化为适当的值
            string receiveName = "yy"; // TODO: 初始化为适当的值
            string receiveMobilePhoneNumber = "13609009800"; // TODO: 初始化为适当的值
            string remarks = string.Empty; // TODO: 初始化为适当的值
            IList<SoOrderItem> orderItems = new List<SoOrderItem> { new SoOrderItem { ProductName = "zz" }, new SoOrderItem { ProductName = "xx" } }; // TODO: 初始化为适当的值
            FnInvoice invoice = null; // TODO: 初始化为适当的值
            SyUser user = new SyUser { SysNo=1, UserName="yy"  }; // TODO: 初始化为适当的值
            target.CreateShopOrder(shopSysNo, customerSysNo, receiveName, receiveMobilePhoneNumber, remarks, orderItems, invoice,1, user,"");
        }

        /// <summary>
        ///ClearOrder 的测试
        ///</summary>
        [TestMethod()]
        public void ClearOrderTest()
        {
            SoOrderBo target = new SoOrderBo(); // TODO: 初始化为适当的值
            int days = 3; // TODO: 初始化为适当的值
            target.ClearOrder(days);
        }

        /// <summary>
        ///ClearOrder 的测试
        ///</summary>
        [TestMethod()]
        public void ClearOrderTest1()
        {
            SoOrderBo target = new SoOrderBo(); // TODO: 初始化为适当的值
            int days = 0; // TODO: 初始化为适当的值
            target.ClearOrder(days);
            Assert.Inconclusive("无法验证不返回值的方法。");
        }


         /// <summary>
        /// 计算出库单实际销售金额
        /// </summary>
        /// <param name="order">订单</param>
        /// <param name="orderItems">销售单明细</param>
        /// <param name="outStockOrderItems">创建出库单的销售单明细</param>
        /// <returns>出库单明细实际销售金额</returns>
        /// <remarks>2013-11-22 杨浩 创建</remarks>
        [TestMethod()]
        public void CalculateOutStockItemAmount()
        {
            int orderId=68678;
            SoOrder order=BLL.Order.SoOrderBo.Instance.GetEntity(orderId);
                
             IList<SoOrderItem> orderItems=BLL.Order.SoOrderBo.Instance.GetOrderItemsByOrderId(orderId);

             IList<SoOrderItem> outStockOrderItems = orderItems;
             var stockAmount=BLL.Order.SoOrderBo.Instance.CalculateOutStockItemAmount(order, orderItems, outStockOrderItems);

        }
    }
}
