using Hyt.BLL.Distribution;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Hyt.Model;
using System.Collections.Generic;
using Hyt.Model.WorkflowStatus;

namespace Hyt.UnitTest
{
    
    
    /// <summary>
    ///这是 DsMallTypeBoTest 的测试类，旨在
    ///包含所有 DsMallTypeBoTest 单元测试
    ///</summary>
    [TestClass()]
    public class DsMallTypeBoTest
    {
        public DsMallTypeBoTest()
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
        ///DsMallTypeBo 构造函数 的测试
        ///</summary>
        [TestMethod()]
        public void DsMallTypeBoConstructorTest()
        {
            DsMallTypeBo target = new DsMallTypeBo();
            Assert.Inconclusive("TODO: 实现用来验证目标的代码");
        }

        /// <summary>
        ///Create 的测试
        ///</summary>
        [TestMethod()]
        public void CreateTest()
        {
            DsMallTypeBo target = new DsMallTypeBo(); // TODO: 初始化为适当的值
            DsMallType model = null; // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.Create(model);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///GetDsMallType 的测试
        ///</summary>
        [TestMethod()]
        public void GetDsMallTypeTest()
        {
            DsMallTypeBo target = new DsMallTypeBo(); // TODO: 初始化为适当的值
            string mallCode = string.Empty; // TODO: 初始化为适当的值
            DsMallType expected = null; // TODO: 初始化为适当的值
            DsMallType actual;
            actual = target.GetDsMallType(mallCode);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///GetDsMallType 的测试
        ///</summary>
        [TestMethod()]
        public void GetDsMallTypeTest1()
        {
            DsMallTypeBo target = new DsMallTypeBo(); // TODO: 初始化为适当的值
            int sysNo = 0; // TODO: 初始化为适当的值
            DsMallType expected = null; // TODO: 初始化为适当的值
            DsMallType actual;
            actual = target.GetDsMallType(sysNo);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///GetDsMallTypeList 的测试
        ///</summary>
        [TestMethod()]
        public void GetDsMallTypeListTest()
        {
            DsMallTypeBo target = new DsMallTypeBo(); // TODO: 初始化为适当的值
            string mallName = string.Empty; // TODO: 初始化为适当的值
            Nullable<int> isPreDeposit = new Nullable<int>(); // TODO: 初始化为适当的值
            Nullable<int> status = new Nullable<int>(); // TODO: 初始化为适当的值
            IList<DsMallType> expected = null; // TODO: 初始化为适当的值
            IList<DsMallType> actual;
            actual = target.GetDsMallTypeList(mallName, isPreDeposit, status);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///Update 的测试
        ///</summary>
        [TestMethod()]
        public void UpdateTest()
        {
            DsMallTypeBo target = new DsMallTypeBo(); // TODO: 初始化为适当的值
            DsMallType model = null; // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.Update(model);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

       
    }
}
