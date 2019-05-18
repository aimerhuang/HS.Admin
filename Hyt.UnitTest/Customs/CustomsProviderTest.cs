using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hyt.BLL.Base;
using Hyt.Model;
using Newtonsoft.Json.Linq;

namespace Hyt.UnitTest.Customs
{
    [TestClass]
    public class CustomsProviderTest
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
        /// 取消订单
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-3-23 杨浩 创建</remarks>
        [TestMethod]
        public void CancelOrder()
        {
            //2340,2339,2336
            int orderId = 2339;
            int warehouseSysNo = 25;
            var result = Hyt.BLL.ApiFactory.ApiProviderFactory.GetCustomsInstance((int)CommonEnum.海关.广州机场海关).CancelOrder(orderId, warehouseSysNo);
        }

        /// <summary>
        /// 查询海关订单
        /// </summary>
        /// <returns></returns>
        /// <remarks>2015-12-29 杨浩 创建</remarks>
        [TestMethod]
        public void SearchCustomsOrder()
        {
            int orderId = 2339;

            var result = Hyt.BLL.ApiFactory.ApiProviderFactory.GetCustomsInstance((int)CommonEnum.海关.广州机场海关).SearchCustomsOrder(orderId);

        }

        /// <summary>
        /// 推送订单
        /// </summary>
        /// <returns></returns>
        /// <remarks>2015-10-12 杨浩 创建</remarks>
        [TestMethod]
        public void PushOrder()
        {
            int orderId = 2339;
            int warehouseSysNo = 25;
            var result = Hyt.BLL.ApiFactory.ApiProviderFactory.GetCustomsInstance((int)CommonEnum.海关.广州机场海关).PushOrder(orderId, warehouseSysNo);
        }

        /// <summary>
        /// 海关总署接口测试
        /// </summary>
        /// <remarks>2016-10-12 杨浩 创建</remarks>
        [TestMethod]
        public void CustomsZSTest()
        {
            int orderId = 4346;
            int warehouseSysNo = 39;
            Hyt.BLL.ApiFactory.ApiProviderFactory.GetCustomsInstance((int)CommonEnum.海关.海关总署).PushOrder(orderId,warehouseSysNo);

            //var result=JObject.Parse("{\"alibaba_aliqin_fc_sms_num_send_response\":{\"result\":{\"err_code\":\"0\",\"model\":\"101200680144^1101691705128\",\"success\":true},\"request_id\":\"iv0ia4w4kdun\"}}");

            //var smspro = Extra.SMS.SmsProviderFactory.CreateProvider();
            //smspro.Send("13543052658", "", null);
        }
    }
}
