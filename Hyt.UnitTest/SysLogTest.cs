using System.Collections.Generic;
using Hyt.BLL.Base;
using Hyt.BLL.Log;
using Hyt.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Hyt.Model.WorkflowStatus;

namespace Hyt.UnitTest
{
    
    
    /// <summary>
    ///这是 ISysLogTest 的测试类，旨在
    ///包含所有 ISysLogTest 单元测试
    ///</summary>
    [TestClass()]
    public class SysLogTest
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
            DataProviderBo.Set(Activator.CreateInstance(Type.GetType("Hyt.DataAccess.Oracle.DataProvider,Hyt.DataAccess.Oracle")));
        }

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

        internal virtual SysLog CreateISysLog()
        {
            // TODO: 实例化相应的具体类。
            SysLog target = SysLog.Instance;
            return target;
        }

        /// <summary>
        ///Error 的测试
        ///</summary>
        [TestMethod()]
        public void ErrorTest()
        {
            ISysLog target = CreateISysLog(); // TODO: 初始化为适当的值
            const LogStatus.系统日志来源 source = LogStatus.系统日志来源.物流App; // TODO: 初始化为适当的值
            const string message = "ErrorTest测试测试"; // TODO: 初始化为适当的值
            var exception = new Exception("测试测试"); // TODO: 初始化为适当的值
            target.Error(source, message, exception);
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void SyMenuTest()
        {
            List<SyMenu> menus = new List<SyMenu>();
            BLL.Sys.SyMenuBO.Instance.DoChildNodeRead(237, ref menus);
        }
    }
}
