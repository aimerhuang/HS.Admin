using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hyt.BLL.Base;

namespace Hyt.UnitTest
{
    [TestClass()]
    public class MkTrafficStatisticsBoTest
    {
        //使用 ClassInitialize 在运行类中的第一个测试前先运行代码
        [ClassInitialize()]
        public static void Init(TestContext testContext)
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

        [TestMethod]
        public void PVAndIPForRangeTime()
        {
            Hyt.BLL.Report.MkTrafficStatisticsBo.Instance.PVAndIPForRangeTime(DateTime.Parse("2013/11/10 15:29:57"), DateTime.Parse("2013/11/16 15:30:28"),6);
        }

        [TestMethod]
        public void GetSampleReportData()
        {
            Hyt.BLL.Report.MkTrafficStatisticsBo.Instance.GetSampleReportData();
        }

        [TestMethod]
        public void ViewerSevenDayTotalTop10Test()
        {
             BLL.Report.MkTrafficStatisticsBo.Instance.GetViewerSevenDayTotalTop10();
        }

        [TestMethod]
        public void LocationSevenDayTotalTop10()
        {
            BLL.Report.MkTrafficStatisticsBo.Instance.GetLocationSevenDayTotalTop10();
        }

        [TestMethod]
        public void ProductSevenDayTotalTop10()
        {
            BLL.Report.MkTrafficStatisticsBo.Instance.GetProductSevenDayTotalTop10();
        }

    }
}
