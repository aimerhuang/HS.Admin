using Hyt.DataAccess.Oracle.CRM;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Hyt.Model;

namespace Hyt.UnitTest
{

    /// <summary>
    ///这是 CrExperienceCoinLogDaoImplTest 的测试类，旨在
    ///包含所有 CrExperienceCoinLogDaoImplTest 单元测试
    ///</summary>
    [TestClass()]
    public class CrExperienceCoinLogDaoImplTest
    {
        //使用 ClassInitialize 在运行类中的第一个测试前先运行代码
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
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
        ///GetCrExperienceCoinLogItems 的测试
        ///</summary>
        [TestMethod()]
        public void GetCrExperienceCoinLogItemsTest()
        {
            CrExperienceCoinLogDaoImpl target = new CrExperienceCoinLogDaoImpl();
            Pager<CrExperienceCoinLog> pager = new Pager<CrExperienceCoinLog>
            {
                PageSize = 1,
                PageFilter = new CrExperienceCoinLog
                    {
                        CustomerSysNo = 61,
                        ChangeType = 10
                    }
            };

            target.GetCrExperienceCoinLogItems(ref pager);
            Assert.AreEqual(2, pager.TotalPages);
        }

        /// <summary>
        ///GetCrExperienceCoinSurplus 的测试
        ///</summary>
        [TestMethod()]
        public void GetCrExperienceCoinSurplusTest()
        {
            CrExperienceCoinLogDaoImpl target = new CrExperienceCoinLogDaoImpl(); // TODO: 初始化为适当的值
            int customerSysNo = 61; // TODO: 初始化为适当的值
            int expected = 1100; // TODO: 初始化为适当的值
            int actual;
            actual = target.GetCrExperienceCoinSurplus(customerSysNo);
            Assert.AreEqual(expected, actual);
        }
    }
}
