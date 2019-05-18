using Hyt.BLL.Logistics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Hyt.Model;

namespace Hyt.UnitTest
{

    /// <summary>
    ///这是 LgDeliveryScopeBoTest 的测试类，旨在
    ///包含所有 LgDeliveryScopeBoTest 单元测试
    ///</summary>
    [TestClass()]
    public class LgDeliveryScopeBoTest
    {

        public LgDeliveryScopeBoTest()
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
        ///IsInScope 的测试
        ///</summary>
        [TestMethod()]
        public void IsInScopeTest()
        {
            LgDeliveryScopeBo target = new LgDeliveryScopeBo(); // TODO: 初始化为适当的值
            int areaSysNo = 37; // 上海 TODO: 初始化为适当的值
            //Coordinate coordinate1 = new Coordinate {X = 116.365642, Y = 39.947864};
            Coordinate coordinate1 = new Coordinate { X = 116.331398, Y = 39.897445 }; // 在外部
            Coordinate coordinate2 = new Coordinate { X = 121.476401, Y = 31.298297 }; //顶点重合
            Coordinate coordinate3 = new Coordinate { X = 121.517507, Y = 31.2583 }; //在内部
            bool expected = false; // TODO: 初始化为适当的值
            bool actual1, actual2, actual3;
            actual1 = target.IsInScope(areaSysNo, coordinate1);
            actual2 = target.IsInScope(areaSysNo, coordinate2);
            actual3 = target.IsInScope(areaSysNo, coordinate3);
            Assert.AreEqual(false, actual1);
            Assert.AreEqual(true, actual2);
            Assert.AreEqual(true, actual3);
        }
    }
}
