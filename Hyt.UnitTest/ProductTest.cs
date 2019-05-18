using System.ServiceModel.Syndication;
using System.ServiceModel.Web;
using Hyt.Model.WorkflowStatus;
using Hyt.Service.Implement.B2CApp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using Hyt.Model.B2CApp;
using System.Collections.Generic;
using Hyt.Model;
using Moq;
using Hyt.BLL.Base;
using Hyt.BLL.Product;

namespace Hyt.UnitTest
{
    
    
    /// <summary>
    ///这是 ProductTest 的测试类，旨在
    ///包含所有 ProductTest 单元测试
    ///</summary>
    [TestClass()]
    public class ProductTest
    {

        private TestContext testContextInstance;
        //使用 ClassInitialize 在运行类中的第一个测试前先运行代码
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            DataProviderBo.Set(Activator.CreateInstance(Type.GetType("Hyt.DataAccess.Oracle.DataProvider,Hyt.DataAccess.Oracle")));
        }
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

        [TestMethod]
        public void GetGroupListTestMock()
        {
            Mock<IWebOperationContext> mockContext = new Mock<IWebOperationContext> { DefaultValue = DefaultValue.Mock };
            IEnumerable<SyndicationItem> items;
            Result r;

            using (new MockedWebOperationContext(mockContext.Object))
            {
                //调用服务器端接口方法

                var productService = new Hyt.Service.Implement.B2CApp.Product();

                r = productService.GetGroupList(1);

            }

            //Mock返回类型，否则服务器端调用WebOperationContext.Current.OutgoingResponse.ContentType是回报null异常
            //mockContext.VerifySet(c => c.OutgoingResponse.ContentType, "application/json");

            Assert.IsTrue(r.Status, r.Message);

        }

        [TestMethod]
        public void GetUserRankPrice()
        {
            Assert.IsTrue(BLL.Product.PdPriceBo.Instance.GetUserRankPrice(1, 1, ProductStatus.产品价格来源.会员等级价) > 0);
        }

        /// <summary>
        ///Home 的测试
        ///</summary>
        // TODO: 确保 UrlToTest 特性指定一个指向 ASP.NET 页的 URL(例如，
        // http://.../Default.aspx)。这对于在 Web 服务器上执行单元测试是必需的，
        //无论要测试页、Web 服务还是 WCF 服务都是如此。
        [TestMethod()]
        public void HomeTest()
        {
            Hyt.Service.Implement.B2CApp.Product target = new Hyt.Service.Implement.B2CApp.Product(); // TODO: 初始化为适当的值
            Result<Home> expected = null; // TODO: 初始化为适当的值
            Result<Home> actual;
            actual = target.Home();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void UpdateProductPrice()
        {

            int sysNo = 6951;

            IList<PdPrice> prices=new List<PdPrice>();
            //返回结果集

            prices.Add(new PdPrice()
            {
                SysNo = 13569,
                ProductSysNo = sysNo,
                PriceSource=0,
                SourceSysNo=0,
                Price=158,
                Status=1,
            });

            prices.Add(new PdPrice()
            {
                SysNo = 13570,
                ProductSysNo = sysNo,
                PriceSource = 10,
                SourceSysNo = 1,
                Price = 105,
                Status = 1,
            });

            prices.Add(new PdPrice()
            {
                SysNo = 13571,
                ProductSysNo = sysNo,
                PriceSource = 90,
                SourceSysNo = 0,
                Price = 116,
                Status = 1,
            });


           PdPriceBo.Instance.UpdateProductPrice(sysNo, prices);

           

        }
    }
}
