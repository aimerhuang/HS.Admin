using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hyt.Model;

namespace Hyt.UnitTest
{
    [TestClass]
    public class FinanceBoTest
    {
        #region 初始化
        public FinanceBoTest()
        {
            Hyt.Infrastructure.Initialize.Init();
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
       #endregion

        [TestMethod]
        public void GetPaymentTest()
        {
            Hyt.BLL.Finance.FinanceBo.Instance.GetPayment(1024);
        }

        [TestMethod]
        public void GetPaymentVoucherTest()
        {
            Hyt.BLL.Finance.FinanceBo.Instance.GetPaymentVoucher(1024);
        }
    }
}
