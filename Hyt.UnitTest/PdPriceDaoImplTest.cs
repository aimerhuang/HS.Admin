using Hyt.DataAccess.Oracle.Product;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Hyt.Model;
using System.Collections.Generic;

namespace Hyt.UnitTest
{
    
    
    /// <summary>
    ///这是 PdPriceDaoImplTest 的测试类，旨在
    ///包含所有 PdPriceDaoImplTest 单元测试
    ///</summary>
    [TestClass()]
    public class PdPriceDaoImplTest
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
        ///GetProductPrice 的测试
        ///</summary>
        [TestMethod()]
        public void GetProductPriceTest()
        {
            PdPriceDaoImpl target = new PdPriceDaoImpl(); // TODO: 初始化为适当的值
            int productSysNo = 59; // TODO: 初始化为适当的值
            IList<PdPrice> expected = null; // TODO: 初始化为适当的值
            IList<PdPrice> actual;
            actual = target.GetProductPrice(productSysNo);
            Assert.AreEqual(3, actual.Count);
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///GetPriceTypeItems 的测试
        ///</summary>
        [TestMethod()]
        public void GetPriceTypeItemsTest()
        {
            PdPriceDaoImpl target = new PdPriceDaoImpl(); // TODO: 初始化为适当的值
            IList<PdPriceType> expected = null; // TODO: 初始化为适当的值
            IList<PdPriceType> actual;
            actual = target.GetPriceTypeItems();
            Assert.AreEqual(6, actual.Count);
        }

        /// <summary>
        ///GetPriceTypeName 的测试
        ///</summary>
        [TestMethod()]
        public void GetPriceTypeNameTest()
        {
            //PdPriceDaoImpl target = new PdPriceDaoImpl(); // TODO: 初始化为适当的值
            //int priceSource = 80; // TODO: 初始化为适当的值
            //int sourceSysNo = 0; // TODO: 初始化为适当的值
            //string expected = "未知类型"; // TODO: 初始化为适当的值
            //string actual;
            //actual = target.GetPriceTypeName(priceSource, sourceSysNo);
            //Assert.AreEqual(expected, actual);
        }
    }
}
