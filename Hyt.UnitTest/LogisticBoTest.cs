using Hyt.BLL.Base;
using Hyt.BLL.Logistics;
using Hyt.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Hyt.UnitTest
{
    
    
    /// <summary>
    ///这是 LogisticBoTest 的测试类，旨在
    ///包含所有 LogisticBoTest 单元测试
    ///</summary>
    [TestClass()]
    public class LogisticBoTest
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
        ///CreateDeliveryMaster 的测试
        ///</summary>
        [TestMethod()]
        public void CreateLgDeliveryTest()
        {
            //var sms=new Hyt.Web.Controllers.SmsReceiveController();
            //var args = "123456,132710,15388650501,%b2%e2%ca%d4%b6%cc%d0%c51,2012-5-23 11:38:46;123456,132710,15321858155,%b2%e2%ca%d4%b6%cc%d0%c52,2012-5-23 11:38:49;123456,132710,18008046562,%b2%e2%ca%d4%b6%cc%d0%c52,2012-5-23 11:38:49";
            //sms.Index(args);
           // new BLL.Sys.SyJobPoolPublishBo().SmsQuestion("yy", 211, 1);
            return;
            LgExpressBo target =new LgExpressBo();
            var list = target.GetExpressInfoByTransactionSysNo("T000000000269996");
            Assert.IsTrue(list.Count > 0);

            var loglist = target.GetLgExpressLogByTransactionSysNo("T000000000269996");
            Assert.IsTrue(loglist.Count > 0);
            //LgDeliveryBo target = new LgDeliveryBo(); // TODO: 初始化为适当的值
            //int warehouseSysNO = 1; // TODO: 初始化为适当的值
            //int userSysNO = 1; // TODO: 初始化为适当的值

            //var actual = target.CreateLgDelivery(warehouseSysNO, userSysNO, 1);
            ////Assert.Equals(0, actual);
            //Assert.IsTrue(actual.Status);
        }

       /// <summary>
        ///CreateDeliveryMaster 的测试
        ///</summary>
        [TestMethod()]
        public void BackFillLogisticsInfo()
        {
            //LgDeliveryBo.Instance.BackFillLogisticsInfo(560751, 4);

           LgDeliveryBo.Instance.CreateExpressLgDelivery(new rp_第三方快递发货量
           {
               CompanyName = "yy",
               CreateDate = DateTime.Now,
               ExpressNo = "123",
               Remarks = "aaa",
               StockSysNo = 1,
               StockName = "成都"
           });
        }
    }
}
