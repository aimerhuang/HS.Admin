using Hyt.BLL.Distribution;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.Parameter;
using System.Collections.Generic;
using Hyt.Model.WorkflowStatus;

namespace Hyt.UnitTest
{
    
    
    /// <summary>
    ///这是 DsDealerBoTest 的测试类，旨在
    ///包含所有 DsDealerBoTest 单元测试
    ///</summary>
    [TestClass()]
    public class DsDealerBoTest
    {
        public DsDealerBoTest()
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
        ///DsDealerBo 构造函数 的测试
        ///</summary>
        [TestMethod()]
        public void DsDealerBoConstructorTest()
        {
            DsDealerBo target = new DsDealerBo();
            Assert.Inconclusive("TODO: 实现用来验证目标的代码");
        }

        /// <summary>
        ///Create 的测试
        ///</summary>
        [TestMethod()]
        public void CreateTest()
        {
            DsDealerBo target = new DsDealerBo(); // TODO: 初始化为适当的值
            DsDealer model = null; // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.Create(model);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///GetDsDealer 的测试
        ///</summary>
        [TestMethod()]
        public void GetDsDealerTest()
        {
            DsDealerBo target = new DsDealerBo(); // TODO: 初始化为适当的值
            int sysNo = 0; // TODO: 初始化为适当的值
            CBDsDealer expected = null; // TODO: 初始化为适当的值
            CBDsDealer actual;
            actual = target.GetDsDealer(sysNo);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///GetDsDealerList 的测试
        ///</summary>
        [TestMethod()]
        public void GetDsDealerListTest()
        {
            DsDealerBo target = new DsDealerBo(); // TODO: 初始化为适当的值
            Pager<CBDsDealer> pager = null; // TODO: 初始化为适当的值
            Pager<CBDsDealer> pagerExpected = null; // TODO: 初始化为适当的值
            ParaDsDealerFilter filter = null; // TODO: 初始化为适当的值
            target.GetDsDealerList(ref pager, filter);
            Assert.AreEqual(pagerExpected, pager);
            Assert.Inconclusive("无法验证不返回值的方法。");
        }

        /// <summary>
        ///GetDsDealerList 的测试
        ///</summary>
        [TestMethod()]
        public void GetDsDealerListTest1()
        {
            DsDealerBo target = new DsDealerBo(); // TODO: 初始化为适当的值
            ParaDsDealerFilter filter = null; // TODO: 初始化为适当的值
            IList<CBDsDealer> expected = null; // TODO: 初始化为适当的值
            IList<CBDsDealer> actual;
            actual = target.GetDsDealerList(filter);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///GetDsDealerList 的测试
        ///</summary>
        [TestMethod()]
        public void GetDsDealerListTest2()
        {
            DsDealerBo target = new DsDealerBo(); // TODO: 初始化为适当的值
            int userSysNo = 0; // TODO: 初始化为适当的值
            IList<CBDsDealer> expected = null; // TODO: 初始化为适当的值
            IList<CBDsDealer> actual;
            actual = target.GetDsDealerList(userSysNo);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///GetDsDealerList 的测试
        ///</summary>
        [TestMethod()]
        public void GetDsDealerListTest3()
        {
            DsDealerBo target = new DsDealerBo(); // TODO: 初始化为适当的值
            string dealerName = string.Empty; // TODO: 初始化为适当的值
            IList<CBDsDealer> expected = null; // TODO: 初始化为适当的值
            IList<CBDsDealer> actual;
            actual = target.GetDsDealerList(dealerName);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///GetDsDealerList 的测试
        ///</summary>
        [TestMethod()]
        public void GetDsDealerListTest4()
        {
            DsDealerBo target = new DsDealerBo(); // TODO: 初始化为适当的值
            int userSysNo = 0; // TODO: 初始化为适当的值
            int sysNo = 0; // TODO: 初始化为适当的值
            IList<CBDsDealer> expected = null; // TODO: 初始化为适当的值
            IList<CBDsDealer> actual;
            actual = target.GetDsDealerList(userSysNo, sysNo);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///GetDsDealerMallByShopAccount 的测试
        ///</summary>
        [TestMethod()]
        public void GetDsDealerMallByShopAccountTest()
        {
            DsDealerBo target = new DsDealerBo(); // TODO: 初始化为适当的值
            string shopAccount = string.Empty; // TODO: 初始化为适当的值
            int mallTypeSysNo = 0; // TODO: 初始化为适当的值
            DsDealerMall expected = null; // TODO: 初始化为适当的值
            DsDealerMall actual;
            //actual = target.GetDsDealerMallByShopAccount(shopAccount, mallTypeSysNo);
            //Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///Update 的测试
        ///</summary>
        [TestMethod()]
        public void UpdateTest()
        {
            DsDealerBo target = new DsDealerBo(); // TODO: 初始化为适当的值
            DsDealer model = null; // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.Update(model);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

       
    }
}
