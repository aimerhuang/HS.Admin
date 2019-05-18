using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.BLL.AppContent;
using Hyt.BLL.Base;
using Hyt.Model;
using Hyt.Model.Transfer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hyt.UnitTest
{
    [TestClass()]
    public class AppContentBoTest
    {
        public TestContext TestContext { get; set; }

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            DataProviderBo.Set(Activator.CreateInstance(Type.GetType("Hyt.DataAccess.Oracle.DataProvider,Hyt.DataAccess.Oracle")));
            Infrastructure.Initialize.Init();
        }
       


        [TestMethod()]
        public void GetHistory()
        {
            try
            {
                const int customerSysNo = 1065;
                var list = AppContentBo.Instance.GetProBroHistory(customerSysNo);
                Assert.IsNotNull(list);
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}
