using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hyt.BLL.Base;
using Hyt.BLL.ApiFactory;

namespace Hyt.UnitTest.ApiLogistics.WuZhou
{
    /// <summary>
    /// 五洲接口测试
    /// </summary>
    [TestClass]
    public class LogisticsProviderTest
    {
        public LogisticsProviderTest()
        {
            DataProviderBo.Set(Activator.CreateInstance(Type.GetType("Hyt.DataAccess.Oracle.DataProvider,Hyt.DataAccess.Oracle")));
        }
        /// <summary>
        /// 推送订单
        /// </summary>
        [TestMethod]
        public void AddOrderTrade()
        {
            var instance=ApiProviderFactory.GetLogisticsInstance((int)Hyt.Model.CommonEnum.物流代码.五洲四海商务);
            instance.AddOrderTrade(442);
        }
  
    }
}
