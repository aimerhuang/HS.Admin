using Hyt.BLL.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;

namespace Hyt.UnitTest
{
    
    
    /// <summary>
    ///这是 BsAreaBoTest 的测试类，旨在
    ///包含所有 BsAreaBoTest 单元测试
    ///</summary>
    [TestClass()]
    public class BsAreaBoTest
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
        ///GetChildArea 的测试
        ///</summary>
        [TestMethod()]
        public void GetChildAreaTest()
        {
            var code = "02.01.01.02-02".Substring(0,0);
           //BsAreaBo target = new BsAreaBo(); // TODO: 初始化为适当的值
           //int parentSysNo = 100000; // TODO: 初始化为适当的值
           //IEnumerable expected = null; // TODO: 初始化为适当的值
           //IEnumerable actual;
           //actual = target.GetChildArea(parentSysNo);
           //Assert.AreEqual(expected, actual);
        }
    }
}
