using Hyt.BLL.LevelPoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Hyt.Model.WorkflowStatus;
using Hyt.BLL.Base;

namespace Hyt.UnitTest
{
    
    
    /// <summary>
    ///这是 PointBoTest 的测试类，旨在
    ///包含所有 PointBoTest 单元测试
    ///</summary>
    [TestClass()]
    public class PointBoTest
    {
        //使用 ClassInitialize 在运行类中的第一个测试前先运行代码
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
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
        ///CreateExperiencePoint 的测试
        ///</summary>
        [TestMethod()]
        public void CreateExperiencePointTest()
        {
            //var customer = Hyt.BLL.CRM.CrCustomerBo.Instance.GetCrCustomerItem(67);
            //PointBo target = new PointBo(); // TODO: 初始化为适当的值
            //int customerSysNo = 62; // TODO: 初始化为适当的值
            //int userSysNo = 20; // TODO: 初始化为适当的值
            //CustomerStatus.积分变更类型 changeType = CustomerStatus.积分变更类型.交易获得; // TODO: 初始化为适当的值
            //int point = 2000; // TODO: 初始化为适当的值
            //string description = "测试调整"; // TODO: 初始化为适当的值
            //int actual;
            //actual = target.CreateExperiencePoint(customer, userSysNo, changeType, point, description);
            //Assert.AreEqual(0, actual);
        }

        /// <summary>
        ///ResisterIncreasePoint 的测试
        ///</summary>
        [TestMethod()]
        public void ResisterIncreasePointTest()
        {
            //PointBo target = new PointBo(); // TODO: 初始化为适当的值
            //int customerSysNo = 61; // TODO: 初始化为适当的值
            //int expected = 0; // TODO: 初始化为适当的值
            //int actual;
            //actual = target.ResisterIncreasePoint(customerSysNo);
            //Assert.AreEqual(0, actual);
        }

        /// <summary>
        ///AdjustLevelPoint 的测试
        ///</summary>
        [TestMethod()]
        public void AdjustLevelPointTest()
        {
            //PointBo target = new PointBo(); // TODO: 初始化为适当的值
            //int customerSysNo = 61; // TODO: 初始化为适当的值
            //int userSysNo = 0; // TODO: 初始化为适当的值
            //int point = -1; // TODO: 初始化为适当的值
            //string description = "退价补差"; // TODO: 初始化为适当的值
            //int expected = 0; // TODO: 初始化为适当的值
            //int actual;
            //actual = target.AdjustLevelPoint(customerSysNo, userSysNo, point, description);
            //Assert.AreEqual(0, actual);
        }

        /// <summary>
        ///ActivityDecreasePoint 的测试
        ///</summary>
        [TestMethod()]
        public void ActivityDecreasePointTest()
        {
            //PointBo target = new PointBo(); // TODO: 初始化为适当的值
            //int customerSysNo = 61; // TODO: 初始化为适当的值
            //string activityName = "爱情连连看"; // TODO: 初始化为适当的值
            //int point = 2500; // TODO: 初始化为适当的值
            //int expected = 0; // TODO: 初始化为适当的值
            //int actual;
            //actual = target.ActivityDecreasePoint(customerSysNo, activityName, point);
            //Assert.AreEqual(0, actual);
        }

        /// <summary>
        ///OrderIncreasePoint 的测试
        ///</summary>
        [TestMethod()]
        public void OrderIncreasePointTest()
        {
            //PointBo target = new PointBo(); // TODO: 初始化为适当的值
            //int customerSysNo = 61; // TODO: 初始化为适当的值
            //int orderSysNo = 1010000; // TODO: 初始化为适当的值
            //int point = 1; // TODO: 初始化为适当的值
            //string transactionSysNo = string.Empty; // TODO: 初始化为适当的值
            //int expected = 0; // TODO: 初始化为适当的值
            //int actual;
            //actual = target.OrderIncreasePoint(customerSysNo, orderSysNo, point, transactionSysNo);
            //Assert.AreEqual(0, actual);
        }

        /// <summary>
        ///PointToCoin 的测试
        ///</summary>
        [TestMethod()]
        public void PointToCoinTest()
        {
            PointBo target = new PointBo(); // TODO: 初始化为适当的值
            int point = 5; // TODO: 初始化为适当的值
            int modPoint = 0; // TODO: 初始化为适当的值
            int modPointExpected = 0; // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.PointToCoin(point, ref modPoint);
        }

        /// <summary>
        ///OrderIncreasePoint 的测试
        ///</summary>
        [TestMethod()]
        public void OrderIncreasePointTest1()
        {
            PointBo target = new PointBo(); // TODO: 初始化为适当的值
            int customerSysNo = 151884; // TODO: 初始化为适当的值
            int orderSysNo = 110; // TODO: 初始化为适当的值
            int point = 100; // TODO: 初始化为适当的值
            string transactionSysNo = string.Empty; // TODO: 初始化为适当的值
            target.OrderIncreasePoint(customerSysNo, orderSysNo, point, transactionSysNo);
        }

        /// <summary>
        ///AvailablePointConvertToExperienceCoin 的测试
        ///</summary>
        [TestMethod()]
        public void AvailablePointConvertToExperienceCoinTest()
        {
            PointBo target = new PointBo(); // TODO: 初始化为适当的值
            int customerSysNo = 151884; // 黄波
            //int customerSysNo = 151869; // 叶婷婷
            int point = 4400000; // TODO: 初始化为适当的值
            string transactionSysNo = string.Empty; // TODO: 初始化为适当的值
            target.AvailablePointConvertToExperienceCoin(customerSysNo, point, transactionSysNo);
        }
    }
}
