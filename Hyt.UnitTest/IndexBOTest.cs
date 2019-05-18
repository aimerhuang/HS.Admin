using Hyt.Infrastructure.Lucene;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Hyt.Model;
using System.Collections.Generic;
using Lucene.Net.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hyt.Model.WorkflowStatus;
using Hyt.Util;
using Hyt.Util.Serialization;
using Hyt.Model.SystemPredefined;

namespace Hyt.UnitTest
{
    
    
    /// <summary>
    ///这是 IndexBOTest 的测试类，旨在
    ///包含所有 IndexBOTest 单元测试
    ///</summary>
    [TestClass()]
    public class IndexBOTest
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
            ProductIndex target = new ProductIndex(); // TODO: 初始化为适当的值
            string key = "手机电池 电池 数据"; // TODO: 初始化为适当的值
            int pageIndex = 1; // TODO: 初始化为适当的值
            int pageSize = 100; // TODO: 初始化为适当的值
            int recCount = 0; // TODO: 初始化为适当的值
            int recCountExpected = 0; // TODO: 初始化为适当的值
            IList<PdProductIndex> expected = null; // TODO: 初始化为适当的值
            IList<PdProductIndex> actual;
            //actual = target.Search(key, pageIndex, pageSize, ref recCount);
            Assert.AreEqual(recCountExpected, recCount);
        }

        /// <summary>
        ///BaseSearch 的测试
        ///</summary>
        [TestMethod()]
        public void BaseSearchTest()
        {
            //ProductIndex target = new ProductIndex(); // TODO: 初始化为适当的值
            //string key = "电霸"; // TODO: 初始化为适当的值
            //Nullable<int> categorySysNo = new Nullable<int>(); // TODO: 初始化为适当的值
            //Nullable<int> brandSysNo = new Nullable<int>(); // TODO: 初始化为适当的值
            //Hits expected = null; // TODO: 初始化为适当的值
            //Hits actual;
            ////actual = target.BaseSearch("" ,131, 0);
            //Assert.AreEqual(expected, actual);

            var aa = new List<KeyValuePair<int, int>>();

            aa.Add(new System.Collections.Generic.KeyValuePair<int, int>(0,1));
            aa.Add(new System.Collections.Generic.KeyValuePair<int, int>(0, 2));
            aa.Add(new System.Collections.Generic.KeyValuePair<int, int>(3, 3));

            var b = aa.ToJson();

            var a = "[{key:0,value:1},{key:0,value:2},{key:3,value:3}]".ToObject<IList<KeyValuePair<int, int>>>();

            
        }
    }
}
