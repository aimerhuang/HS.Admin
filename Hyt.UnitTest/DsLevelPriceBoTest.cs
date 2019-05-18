using Hyt.BLL.Distribution;
using Hyt.BLL.Warehouse;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Hyt.Model.WorkflowStatus;
using Hyt.Model;
using System.Collections.Generic;

namespace Hyt.UnitTest
{
    
    
    /// <summary>
    ///这是 WhWarehouseBoTest 的测试类，旨在
    ///包含所有 WhWarehouseBoTest 单元测试
    ///</summary>
    [TestClass]
    public class DsLevelPriceBoTest
    {
        /// <summary>
        ///获取或设置测试上下文，上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext { get; set; }

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
        ///GetWhWareHouse 的测试
        ///</summary>
        [TestMethod()]
        public void GetWhWareHouseTest()
        {
            var target = new DsLevelPriceBo(); 

            IList<CBPdPrice> actual = target.GetLevelPriceByProdouctSysNo(1971);

            Assert.IsNotNull(actual);
        }
    }
}
