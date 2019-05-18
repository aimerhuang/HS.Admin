using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hyt.BLL.Base;
using Hyt.BLL.ApiFactory;

namespace Hyt.UnitTest.ApiPay
{
    [TestClass]
    public class PayProviderTest
    {
        public PayProviderTest()
        {
            DataProviderBo.Set(Activator.CreateInstance(Type.GetType("Hyt.DataAccess.Oracle.DataProvider,Hyt.DataAccess.Oracle")));
        }
       
        /// <summary>
        /// 查询订单支付状态
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-12-20 杨浩 创建</remarks> 
        [TestMethod()]
        public void QueryOrderState()
        {
            //int orderId = 0;
            //var instance = ApiProviderFactory.GetPayInstance((int)Hyt.Model.CommonEnum.PayCode.微信);
            //instance.QueryOrderState(orderId);
        }

    }
}
