using Hyt.BLL.ApiFactory;
using Hyt.BLL.Base;
using Hyt.BLL.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;


namespace Hyt.UnitTest
{
    [TestClass()]
    public class LogisticsProviderTest
    {

        //使用 ClassInitialize 在运行类中的第一个测试前先运行代码
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
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

        /// <summary>
        /// 添加产品
        /// </summary>
        /// <param name="_params">参数</param>
        /// <remarks>2016-3-8 杨浩 创建</remarks>
        [TestMethod()]
        public  void AddProductTest()
        {
            string _params = "{\"barcode\":\"20014011\",\"brand\":\"宝洁\",\"categoryName\":\"paper\",\"channel\":\"www.jd.com\",\"childProductList\":{\"childProducts\":[]},\"currency\":\"cny\",\"foreign\":\"china\",\"grossWt\":1,\"imageUrl\":null,\"model\":\"大中型\",\"netWt\":1,\"notes\":null,\"origin\":\"gz\",\"prodAttributeInfo\":\"11:22,33:44\",\"productId\":\"1001\",\"productName\":\"帮宝适纸内裤\",\"quality\":\"perfect\",\"salePrice\":100,\"saleUnits\":\"CNY\",\"unitPrice\":200, \" menuFact\":\"中国\"}";
            
            //var result=ApiProviderFactory.GetLogisticsInstance((int)Hyt.Model.CommonEnum.物流代码.威时沛).AddProduct(null);
        }
        /// <summary>
        /// 添加交易订单
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-3-21 杨浩 创建</remarks>
        [TestMethod()]
        public void AddOrderTradeTest()
        {
            var result = ApiProviderFactory.GetLogisticsInstance((int)Hyt.Model.CommonEnum.物流代码.威时沛).AddOrderTrade(3853);
            Assert.IsTrue(result.Status);
        }
          /// <summary>
        /// 查询订单运单号信息
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-3-22 杨浩 创建</remarks>
        [TestMethod()]
        public void GetOrderExpressno()
        {
            var result = ApiProviderFactory.GetLogisticsInstance((int)Hyt.Model.CommonEnum.物流代码.威时沛).GetOrderExpressno("11001000345610S0004");
            Assert.IsTrue(result.Status);
        }
        /// <summary>
        /// 查询订单
        /// </summary>
        ///  <remarks>2016-3-22 杨浩 创建</remarks>
        [TestMethod()]
        public void GetOrderTrade()
        {
            var result = ApiProviderFactory.GetLogisticsInstance((int)Hyt.Model.CommonEnum.物流代码.威时沛).GetOrderTrade("11001000345610S0004");
            //var result = ApiProviderFactory.GetLogisticsInstance((int)Hyt.Model.CommonEnum.物流代码.威时沛).GetOrderTrade("3853");
            Assert.IsTrue(result.Status);
        }

        [TestMethod()]
        public void GetOrderTracking()
        {
            var result = ApiProviderFactory.GetLogisticsInstance((int)Hyt.Model.CommonEnum.物流代码.威时沛).GetLogisticsTracking(123);
            Assert.IsTrue(result.Status);
        }
    }
}
