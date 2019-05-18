using System.Linq;
using Hyt.BLL.Base;
using Hyt.BLL.Order;
using Hyt.BLL.Product;
using Hyt.BLL.Promotion;
using Hyt.BLL.Sys;
using Hyt.DataAccess.CRM;
using Hyt.Infrastructure.Caching;
using Hyt.Infrastructure.Pager;
using Hyt.Model.Parameter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Hyt.Model;
using System.Collections.Generic;

namespace Hyt.UnitTest
{
    
    
    /// <summary>
    ///这是 JobPoolBoTest 的测试类，旨在
    ///包含所有 JobPoolBoTest 单元测试
    ///</summary>
    [TestClass()]
    public class JobPoolBoTest
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
        [ClassInitialize]
        public static void MyClassInitialize(TestContext testContext)
        {
            DataProviderBo.Set(Activator.CreateInstance(Type.GetType("Hyt.DataAccess.Oracle.DataProvider,Hyt.DataAccess.Oracle")));
            Infrastructure.Initialize.Init();
        }
        //
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
        ///GetJobSpoolList 的测试
        ///</summary>
        [TestMethod()]
        public void GetJobSpoolListTest()
        {
            SyJobPoolManageBo target = new SyJobPoolManageBo(); // TODO: 初始化为适当的值
            Pager<CBSyJobPool> pager = new Pager<CBSyJobPool>();
            target.GetJobSpoolList(pager);
            Assert.IsTrue(pager.Rows.Count > 0);

            pager.PageFilter.TaskSysNo = 1100110011;
            target.GetJobSpoolList(pager);
            Assert.IsTrue(pager.Rows.Count > 0);

            pager.PageFilter.Status = 1;
            target.GetJobSpoolList(pager);
            Assert.IsTrue(pager.Rows.Count > 0);

            pager.PageFilter.Status = 1;
            pager.PageFilter.ExecutorSysNo = 1;
            pager.PageFilter.TaskSysNo = 1100110011;
            target.GetJobSpoolList(pager);
            Assert.IsTrue(pager.Rows.Count > 0);
        }

        /// <summary>
        ///AssignJobs 的测试
        ///</summary>
        [TestMethod()]
        public void AssignJobsTest()
        {
           new BLL.Sys.SyJobPoolPublishBo().SmsQuestion("yy", 211, 1);
        }

        /// <summary>
        ///AutoAssignJob 的测试
        ///</summary>
        [TestMethod()]
        public void AutoAssignJobTest()
        {
           var res= Hyt.DataAccess.Distribution.IDsPrePaymentDao.Instance.AddAvailableAmount(10, 100, 1);
            res = Hyt.DataAccess.Distribution.IDsPrePaymentDao.Instance.AddTotalPrestoreAmount(10,100,1);
             res = Hyt.DataAccess.Distribution.IDsPrePaymentDao.Instance.SubtractAvailableAmount(10, 100,1);
             res = Hyt.DataAccess.Distribution.IDsPrePaymentDao.Instance.UpdateAlertAmount(10, 100);
            
            Assert.IsTrue(res);
        }

        /// <summary>
        ///ReleaseJob 的测试
        ///</summary>
        [TestMethod()]
        public void ReleaseJobTest()
        {
            SyJobPoolManageBo target = new SyJobPoolManageBo(); // TODO: 初始化为适当的值
            string soSysNos = string.Empty; // TODO: 初始化为适当的值
            bool expected = false; // TODO: 初始化为适当的值
            bool actual;
            actual = target.ReleaseJob(soSysNos,0);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///InsertJobPool 的测试
        ///</summary>
        [TestMethod()]
        public void InsertJobPoolTest()
        {
        }

        /// <summary>
        ///GetSystemUserDict 的测试
        ///</summary>
        [TestMethod()]
        public void GetSystemUserDictTest()
        {
             CacheManager.Get<IList<CrCustomerLevel>>(CacheKeys.Items.CustomerLevel, () => ICrCustomerLevelDao.Instance.List());  
        }

        /// <summary>
        ///GetJobStatus 的测试
        ///</summary>
        [TestMethod()]
        public void GetJobStatusTest()
        {
            SyJobPoolManageBo target = new SyJobPoolManageBo(); // TODO: 初始化为适当的值
            int sysNo = 137; // TODO: 初始化为适当的值
            int expected = 10; // TODO: 初始化为适当的值
            int actual;
            actual = target.GetJobStatus(sysNo);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void PromotionTest()
        {
            //var promotionActual = new List<int>();
            //var promotionProducts = PromotionBo.Instance.GetAllPromotionProduct();
            //var pager = new Pager<ParaProductSearchFilter>() { PageSize = 999999 };
            //PagedList<ParaProductSearchFilter> list = new PagedList<ParaProductSearchFilter>() {PageSize = 99999}; //分页结果集
            //PdProductBo.Instance.ProductSelectorProductSearch(ref pager, out list); //执行查询
            //foreach (var promotionProduct in pager.Rows)
            //{
            //    var promotionList = PdProductBo.Instance.GetProductPromotionHintsFromCache(promotionProduct.SysNo);
            //    if (promotionList != null && promotionList.Any())
            //    {
            //        promotionActual.Add(promotionProduct.SysNo);
            //    }
            //}
            //int expected;
            //int actual;
            //expected = promotionProducts.Count;
            //actual = promotionProducts.Count;
            //Assert.AreEqual(expected, actual);
        }
    }
}
