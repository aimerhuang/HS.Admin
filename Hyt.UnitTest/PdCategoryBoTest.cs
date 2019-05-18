using Hyt.BLL.Web;
using Hyt.Model.Transfer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Hyt.Model;
using Hyt.BLL.Base;

namespace Hyt.UnitTest
{
    
    
    /// <summary>
    ///这是 PdCategoryBoTest 的测试类，旨在
    ///包含所有 PdCategoryBoTest 单元测试
    ///</summary>
    [TestClass()]
    public class PdCategoryBoTest
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
        ///GetChildCategorySysNo 的测试
        ///</summary>
        [TestMethod()]
        public void GetChildCategorySysNoTest()
        {
            PdCategoryBo target = new PdCategoryBo(); // TODO: 初始化为适当的值
            int categorySysNo = 1; // TODO: 初始化为适当的值
            IList<CBPdCategory> expected = null; // TODO: 初始化为适当的值
            IList<CBPdCategory> actual;
            actual = target.GetChildAllCategory(categorySysNo);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///GetAllCategory 的测试
        ///</summary>
        [TestMethod()]
        public void GetAllCategoryTest()
        {
            PdCategoryBo target = new PdCategoryBo(); // TODO: 初始化为适当的值
            IList<CBPdCategory> expected = null; // TODO: 初始化为适当的值
            IList<CBPdCategory> actual;
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            actual = target.GetAllCategory();
            sw.Stop();
            var ElapsedMilliseconds1 = sw.ElapsedMilliseconds.ToString();
        }
    }
}
