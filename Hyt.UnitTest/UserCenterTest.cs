using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.ServiceModel.Web;
using Hyt.Service.Implement.B2CApp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using Hyt.Model.B2CApp;
using Hyt.Model;
using Moq;

namespace Hyt.UnitTest
{
    
    
    /// <summary>
    ///这是 UserCenterTest 的测试类，旨在
    ///包含所有 UserCenterTest 单元测试
    ///</summary>
    [TestClass()]
    public class UserCenterTest
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
        ///GetReturnSchedule 的测试
        ///</summary>
       [TestMethod]
        public void GetReturnScheduleTest()
        {
            Mock<IWebOperationContext> mockContext = new Mock<IWebOperationContext> { DefaultValue = DefaultValue.Mock };

            Result<ReturnHistorySub> r;

            using (new MockedWebOperationContext(mockContext.Object))
            {
                //调用服务器端接口方法

                var userCenterService = new Hyt.Service.Implement.B2CApp.UserCenter();

                r = userCenterService.GetReturnSchedule("1074");

            }

            //Mock返回类型，否则服务器端调用WebOperationContext.Current.OutgoingResponse.ContentType是回报null异常
            //mockContext.VerifySet(c => c.OutgoingResponse.ContentType, "application/json");

            Assert.IsTrue(r.Status, r.Message);

        }

       /// <summary>
       ///GetReturnSchedule 的测试
       ///</summary>
       [TestMethod]
       public void GetReturnScheduleTestErrorSysNo()
       {
           Mock<IWebOperationContext> mockContext = new Mock<IWebOperationContext> { DefaultValue = DefaultValue.Mock };

           Result<ReturnHistorySub> r;

           using (new MockedWebOperationContext(mockContext.Object))
           {
               //调用服务器端接口方法

               var userCenterService = new Hyt.Service.Implement.B2CApp.UserCenter();

               r = userCenterService.GetReturnSchedule("a1074");

           }

           //Mock返回类型，否则服务器端调用WebOperationContext.Current.OutgoingResponse.ContentType是回报null异常
           //mockContext.VerifySet(c => c.OutgoingResponse.ContentType, "application/json");

           Assert.AreEqual("参数错误,请输入正确退换货编号", r.Message);

       }

       /// <summary>
       ///GetReturnSchedule 的测试
       ///</summary>
       [TestMethod]
       public void GetReturnScheduleTestRmaNotExsit()
       {
           Mock<IWebOperationContext> mockContext = new Mock<IWebOperationContext> { DefaultValue = DefaultValue.Mock };

           Result<ReturnHistorySub> r;

           using (new MockedWebOperationContext(mockContext.Object))
           {
               //调用服务器端接口方法

               var userCenterService = new Hyt.Service.Implement.B2CApp.UserCenter();

               r = userCenterService.GetReturnSchedule("1");

           }

           //Mock返回类型，否则服务器端调用WebOperationContext.Current.OutgoingResponse.ContentType是回报null异常
           //mockContext.VerifySet(c => c.OutgoingResponse.ContentType, "application/json");

           Assert.AreEqual("找不到指定的退/换货单", r.Message);

       }
    }
}
