using Hyt.BLL.Distribution;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Hyt.Model;
using System.Collections.Generic;
using Hyt.Model.WorkflowStatus;

namespace Hyt.UnitTest
{
    
    
    /// <summary>
    ///这是 DsDealerLevelBoTest 的测试类，旨在
    ///包含所有 DsDealerLevelBoTest 单元测试
    ///</summary>
    [TestClass()]
    public class DsDealerLevelBoTest
    {
        public DsDealerLevelBoTest()
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
        ///DsDealerLevelBo 构造函数 的测试
        ///</summary>
        [TestMethod()]
        public void DsDealerLevelBoConstructorTest()
        {
            DsDealerLevelBo target = new DsDealerLevelBo();
            Assert.Inconclusive("TODO: 实现用来验证目标的代码");
        }

        /// <summary>
        ///Create 的测试
        ///</summary>
        [TestMethod()]
        public void CreateTest()
        {
            DsDealerLevelBo target = new DsDealerLevelBo(); // TODO: 初始化为适当的值
            DsDealerLevel model = null; // TODO: 初始化为适当的值
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
            DsDealerLevelBo target = new DsDealerLevelBo(); // TODO: 初始化为适当的值
            string levelName = "本杰明"; // TODO: 初始化为适当的值
            DsDealerLevel expected = null; // TODO: 初始化为适当的值
            DsDealerLevel actual;
            actual = target.GetDsDealerLevel(levelName);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///GetDsDealer 的测试
        ///</summary>
        [TestMethod()]
        public void GetDsDealerTest1()
        {
            DsDealerLevelBo target = new DsDealerLevelBo(); // TODO: 初始化为适当的值
            int sysNo = 0; // TODO: 初始化为适当的值
            DsDealerLevel expected = null; // TODO: 初始化为适当的值
            DsDealerLevel actual;
            actual = target.GetDsDealerLevel(sysNo);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///GetDsDealerLevelList 的测试
        ///</summary>
        [TestMethod()]
        public void GetDsDealerLevelListTest()
        {
            DsDealerLevelBo target = new DsDealerLevelBo(); // TODO: 初始化为适当的值
            IList<DsDealerLevel> expected = null; // TODO: 初始化为适当的值
            IList<DsDealerLevel> actual;
            actual = target.GetDsDealerLevelList();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///Update 的测试
        ///</summary>
        [TestMethod()]
        public void UpdateTest()
        {
            DsDealerLevelBo target = new DsDealerLevelBo(); // TODO: 初始化为适当的值
            DsDealerLevel model = null; // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.Update(model);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

      
    }
}
