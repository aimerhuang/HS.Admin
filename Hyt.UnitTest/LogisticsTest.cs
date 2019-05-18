using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Syndication;
using System.ServiceModel.Web;
using System.Threading;
using System.Web;
using System.Web.SessionState;
using Hyt.BLL.Base;
using Hyt.BLL.Log;
using Hyt.BLL.Product;
using Hyt.Model.WorkflowStatus;
using Hyt.Service.Contract.LogisApp;
using Hyt.Service.Implement.LogisApp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using Hyt.Model;
using Moq;
using Newtonsoft.Json;

namespace Hyt.UnitTest
{
    
    
    /// <summary>
    ///这是 LogisticsTest 的测试类，旨在
    ///包含所有 LogisticsTest 单元测试
    ///</summary>
    [TestClass()]
    public class LogisticsTest
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
        [ClassInitialize]
        public static void MyClassInitialize(TestContext testContext)
        {
            DataProviderBo.Set(Activator.CreateInstance(Type.GetType("Hyt.DataAccess.Oracle.DataProvider,Hyt.DataAccess.Oracle")));
            Infrastructure.Initialize.Init();
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
        ///Login  成功测试
        ///</summary>
        [TestMethod]
        public void LoginTestMock()
        {

            HttpContext.Current = FakeHttpContext();

            Mock<IWebOperationContext> mockContext = new Mock<IWebOperationContext> { DefaultValue = DefaultValue.Mock };
            IEnumerable<SyndicationItem> items;
            Result r;
            
            using (new MockedWebOperationContext(mockContext.Object))
            {
              

                //调用服务器端接口方法

                Logistics logisticsService = new Logistics();
                
                  r = logisticsService.Login("admin", "123456");
              
            }

            //Mock返回类型，否则服务器端调用WebOperationContext.Current.OutgoingResponse.ContentType是回报null异常
            mockContext.VerifySet(c => c.OutgoingResponse.ContentType, "application/json");

             

            Assert.IsTrue(r.Status,r.Message);
             
        }

        /// <summary>
        ///Login  失败测试
        ///</summary>
        [TestMethod]
        public void LoginFailedTestMock()
        {

            HttpContext.Current = FakeHttpContext();

            Mock<IWebOperationContext> mockContext = new Mock<IWebOperationContext> { DefaultValue = DefaultValue.Mock };
            IEnumerable<SyndicationItem> items;
            Result r;

            using (new MockedWebOperationContext(mockContext.Object))
            {

                //调用服务器端接口方法

                Logistics logisticsService = new Logistics();

                r = logisticsService.Login("admin", "0");

            }

            //Mock返回类型，否则服务器端调用WebOperationContext.Current.OutgoingResponse.ContentType是回报null异常
            mockContext.VerifySet(c => c.OutgoingResponse.ContentType, "application/json");

            Assert.IsFalse(r.Status, r.Message);

        }

        public static HttpContext FakeHttpContext()
        {
            var httpRequest = new HttpRequest("", "http://kindermusik/", "");
            var stringWriter = new StringWriter();
            var httpResponce = new HttpResponse(stringWriter);
            var httpContext = new HttpContext(httpRequest, httpResponce);

            var sessionContainer = new HttpSessionStateContainer("id", new SessionStateItemCollection(),
                                                    new HttpStaticObjectsCollection(), 10, true,
                                                    HttpCookieMode.AutoDetect,
                                                    SessionStateMode.InProc, false);

            httpContext.Items["AspSession"] = typeof(HttpSessionState).GetConstructor(
                                        BindingFlags.NonPublic | BindingFlags.Instance,
                                        null, CallingConventions.Standard,
                                        new[] { typeof(HttpSessionStateContainer) },
                                        null)
                                .Invoke(new object[] { sessionContainer });

            return httpContext;
        }

        public static HttpContextBase FakeHttpContextBase()
        {
            var context = new Mock<HttpContextBase>();
            var request = new Mock<HttpRequestBase>();
            var response = new Mock<HttpResponseBase>();
            var session = new Mock<HttpSessionStateBase>();
            var server = new Mock<HttpServerUtilityBase>();

            context.Setup(ctx => ctx.Request).Returns(request.Object);
            context.Setup(ctx => ctx.Response).Returns(response.Object);
            context.Setup(ctx => ctx.Session).Returns(session.Object);
            context.Setup(ctx => ctx.Server).Returns(server.Object);

            return context.Object;
        }

        [TestMethod]
        public void SelectProductPriceTest()
        {
             var productSysnos = "2724,2721,2415";
            var result = new Result<String> { StatusCode = -1, Status = false };
            try
            {
                var lst = new List<KeyValuePair<int, IList<CBPdPrice>>>();
                if (string.IsNullOrWhiteSpace(productSysnos))
                {
                    result.Message = "产品编号不能为空";
                    return;
                }
                var productids = productSysnos.Split(',').Select(m => int.Parse(m)).ToList();
                if (productids != null)
                {
                    productids.ForEach(p =>
                    {

                        var item = PdPriceBo.Instance.GetProductLevelPrice(p);
                        var pitem = new KeyValuePair<int, IList<CBPdPrice>>(p, item);
                        lst.Add(pitem);

                    });
                    result.Data = JsonConvert.SerializeObject(lst.Select(m => new { SysNo = m.Key, PriceList =m.Value.Select(i=> new{Name=i.PriceName,Price=i.Price}) }).ToList());
                }
                result.Status = true;
                result.StatusCode = 1;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.物流App, "获取产品价格" + ex.Message, ex);
            }
            Assert.IsNotNull(result.Data);
        }
    }
  

   

 

}
