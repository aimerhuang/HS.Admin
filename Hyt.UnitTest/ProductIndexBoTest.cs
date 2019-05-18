using Hyt.BLL.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Hyt.Model;
using Hyt.BLL.Base;

namespace Hyt.UnitTest
{
    
    
    /// <summary>
    ///这是 ProductIndexBoTest 的测试类，旨在
    ///包含所有 ProductIndexBoTest 单元测试
    ///</summary>
    [TestClass()]
    public class ProductIndexBoTest
    {

        public ProductIndexBoTest()
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
        ///Search 的测试
        ///</summary>
        [TestMethod()]
        public void SearchTest()
        {
            ProductIndexBo target = new ProductIndexBo(); // TODO: 初始化为适当的值
            string key = string.Empty; // TODO: 初始化为适当的值
            Nullable<int> categorySysNo = new Nullable<int>(); // TODO: 初始化为适当的值
            List<int> attributes = null; // TODO: 初始化为适当的值
            int pageSize = 0; // TODO: 初始化为适当的值
            int pageIndex = 1; // TODO: 初始化为适当的值
            int pageIndexExpected = 0; // TODO: 初始化为适当的值
            int pageCount = 0; // TODO: 初始化为适当的值
            int pageCountExpected = 0; // TODO: 初始化为适当的值
            int recCount = 0; // TODO: 初始化为适当的值
            int recCountExpected = 0; // TODO: 初始化为适当的值
            bool highLight = false; // TODO: 初始化为适当的值
            IList<PdProductIndex> expected = null; // TODO: 初始化为适当的值
            IList<PdProductIndex> actual;
            actual = target.SearchFromDataBase("四段", 638, null, 80, ref pageIndex, ref pageCount, ref recCount, highLight);

        }
        [TestMethod()]
        public void SearchProductSysNoListTest()
        {
            var a = BLL.Web.ProductIndexBo.Instance.Search(new List<int>() { 158,13});
        }
    }
}
