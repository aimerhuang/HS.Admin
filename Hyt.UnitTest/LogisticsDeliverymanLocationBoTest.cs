using Hyt.BLL.Warehouse;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Hyt.Model;
using System.Collections.Generic;
namespace Hyt.UnitTest
{

    /// <summary>
    ///这是 LogisticsDeliveryUserLocationBoTest 的测试类，旨在
    ///包含所有 LogisticsDeliveryUserLocationBoTest 单元测试
    ///</summary>
    [TestClass()]
    public class LogisticsDeliveryUserLocationBoTest
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
            Hyt.Infrastructure.Initialize.Init();
        }

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
        ///Create 的测试
        ///</summary>
        [TestMethod()]
        public void CreateTest()
        {
            // LogisticsDeliveryUserLocationBo target = new LogisticsDeliveryUserLocationBo(); // TODO: 初始化为适当的值
            var Model = new LgDeliveryUserLocation(); // TODO: 初始化为适当的值
            Model.DeliveryUserSysNo = 33;
            Model.Longitude = Convert.ToDecimal(100.22);
            Model.Latitude = Convert.ToDecimal(80.22);
         
            Model.GpsDate = DateTime.Now;
            Model.CreatedDate = DateTime.Now;
            int actual;
            actual = Hyt.BLL.Logistics.DeliveryUserLocationBo.Instance.Create(Model);
            Assert.IsTrue(actual > 0);
            // Assert.Inconclusive("验证此测试方法的正确性。");
        }
        [TestMethod()]
        public void GetLogisticsDeliveryUserLocationTest()
        {
            int DeliveryUserNo = 33;
            List<int> listDeliveryUserNo = new List<int> { 33, 32 };
            DateTime StartTime = DateTime.Now.AddDays(-2);
            DateTime endTime = DateTime.Now;
            //var list = Hyt.BLL.Warehouse.LogisticsDeliveryUserLocationBo.Instance.GetLogisticsDeliveryUserLocation(DeliveryUserNo, StartTime, endTime, 1, 10);
            //var list = Hyt.BLL.Logistics.LogisticsDeliveryUserLocationBo.Instance.GetLgDeliveryUserLastLocation(listDeliveryUserNo, 1, 10);
            // Assert.IsTrue(list.Count > 0);
        }

    }
}
