using Hyt.BLL.ApiFactory;
using Hyt.BLL.Base;
using Hyt.BLL.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Hyt.UnitTest.Supply
{
    /// <summary>
    /// 供应链控制器测试类
    /// </summary>
    [TestClass()]
    public class SupplyControllerTest
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
        /// 同步供应商产品
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-3-18 杨浩 创建</remarks> 
        [TestMethod()]
        public void SynchronousSupplyProduct()
        {
            //供应链代码
            int supplyCode = 30;

            var result = ApiProviderFactory.GetSupplyInstance(supplyCode).GetGoodsSku("B0240180");

        }

        [TestMethod()]
        public void TestMetho()
        {
            ApiProviderFactory.GetSupplyInstance(40).SendOrder(3853);
        }
    }
}
