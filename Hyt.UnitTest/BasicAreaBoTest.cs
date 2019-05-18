using Hyt.BLL.Basic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Hyt.Model;
using System.Collections.Generic;

namespace Hyt.UnitTest
{
      
    
    /// <summary>
    ///这是 BasicAreaBoTest 的测试类，旨在
    ///包含所有 BasicAreaBoTest 单元测试
    ///</summary>
    [TestClass()]
    public class BasicAreaBoTest
    {

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
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            Hyt.Infrastructure.Initialize.Init();
        }
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
        ///Search 的测试
        ///</summary>
        [TestMethod()]
        public void SearchTest()
        {
            BasicAreaBo target = new BasicAreaBo(); // TODO: 初始化为适当的值
            string keyword = "03000"; // TODO: 初始化为适当的值
           
            IList<BsArea> actual;
            actual = target.Search(keyword);
            Assert.IsNotNull( actual);
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///GetProvinceEntity 的测试
        ///</summary>
        [TestMethod()]
        public void GetProvinceEntityTest()
        {
            BasicAreaBo target = new BasicAreaBo(); // TODO: 初始化为适当的值
            int sysNo = 901002; // TODO: 初始化为适当的值
            BsArea cityEntity = null; // TODO: 初始化为适当的值
            BsArea cityEntityExpected = null; // TODO: 初始化为适当的值
            BsArea areaEntity = null; // TODO: 初始化为适当的值
            BsArea areaEntityExpected = null; // TODO: 初始化为适当的值
            BsArea expected = null; // TODO: 初始化为适当的值
            BsArea actual;
            actual = target.GetProvinceEntity(sysNo, out cityEntity, out areaEntity);
            Assert.AreEqual(cityEntityExpected, cityEntity);
            Assert.AreEqual(areaEntityExpected, areaEntity);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }
     
    }
}
