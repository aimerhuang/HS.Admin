using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.BLL.Logistics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hyt.BLL.Base;
namespace Hyt.BLL.Logistics.Tests
{
    [TestClass()]
    public class WhDeliveryScopeBoTests
    {

       
        public WhDeliveryScopeBoTests()
        {
            DataProviderBo.Set(Activator.CreateInstance(Type.GetType("Hyt.DataAccess.Oracle.DataProvider,Hyt.DataAccess.Oracle")));
        }

        [TestMethod()]
        public void IsInScopeTest()
        {
            var r = WhDeliveryScopeBo.Instance.IsInScope(6148, 104.179744, 30.82559);
            Assert.IsTrue(r.Status);
        }

        /// <summary>
        /// 有信达物流测试
        /// </summary>
        [TestMethod()]
        public void YourSenderLogisticsTrackingTest()
        {
            var a = ApiFactory.ApiProviderFactory.GetLogisticsInstance((int)Model.CommonEnum.物流代码.有信达).GetLogisticsTracking(6483);
        }

        //[TestMethod()]
        //public void YourSenderTest()
        //{
        //    var a = ApiFactory.ApiProviderFactory.GetLogisticsInstance((int)Model.CommonEnum.物流代码.有信达).PushOrder(null);
        //}
        [TestMethod()]
        public void GaoJieTest()
        {
            var a = ApiFactory.ApiProviderFactory.GetLogisticsInstance((int)Model.CommonEnum.物流代码.高捷个人物品).GetOrderExpressno("4407");
        }
  
    }
}
