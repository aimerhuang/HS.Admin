using Hyt.BLL.Order;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Hyt.Model;
using System.Collections.Generic;
using Hyt.BLL.Basic;

namespace Hyt.UnitTest
{

    /// <summary>
    ///这是 JobPoolBoTest 的测试类，旨在
    ///包含所有 JobPoolBoTest 单元测试
    ///</summary>
    [TestClass()]
    public class OrderJobPoolBoTest
    {

        public OrderJobPoolBoTest()
        {
            Hyt.Infrastructure.Initialize.Init();
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
        ///AddJob 的测试
        ///</summary>
        [TestMethod()]
        public void AddJobTest()
        {
            //OrderAuditJobPoolBo target = new OrderAuditJobPoolBo(); // TODO: 初始化为适当的值
            //int taskSysNo = 0; // TODO: 初始化为适当的值
            //string jobDescription = "订单审核：A001"; // TODO: 初始化为适当的值
            //string jobUrl = string.Empty; // TODO: 初始化为适当的值
            //SyJobPool expected = null; // TODO: 初始化为适当的值
            //SyJobPool actual;
            //actual = target.AddJob(taskSysNo, jobDescription, jobUrl);
            //Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("验证此测试方法的正确性。");
            // int logistics = 0;
            //    var warehouse = BLL.Warehouse.WhWarehouseBo.Instance.GetWarehouse();
            //    if (warehouse != null)
            //    {
            //        logistics = warehouse.Logistics;
            //    }

            //    if (logistics <= 0)
            //    {
                    
            //    }else{
            // var logisticeIns = BLL.ApiFactory.ApiProviderFactory.GetLogisticsInstance(logistics)
            //var test= logisticeIns.AddOrderTrade(405);
            //        }
        }
    }
}
