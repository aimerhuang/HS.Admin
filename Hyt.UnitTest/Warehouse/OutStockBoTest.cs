using System.Collections.Generic;
using Hyt.BLL.Warehouse;
using Hyt.DataAccess.Oracle.Warehouse;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.UnitTest.Warehouse
{
    [TestClass]
    public class OutStockBoTest
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
        [ClassInitialize]
        public static void MyClassInitialize(TestContext testContext)
        {
            Infrastructure.Initialize.Init();
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
        /// 获取入库单列表
        /// </summary>
        [TestMethod]
        public void GetSingleBatchDOTest()
        {
            var daoImpl = new WhWarehouseBo();
           
            var list = daoImpl.GetSingleBatchDO();
            Assert.IsNull(list);
        }
        
    }
}
