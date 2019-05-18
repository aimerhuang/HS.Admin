using Hyt.BLL.Base;
using Hyt.BLL.Sys;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Hyt.UnitTest
{
    
    
    /// <summary>
    ///这是 SyTaskBoTest 的测试类，旨在
    ///包含所有 SyTaskBoTest 单元测试
    ///</summary>
    [TestClass()]
    public class SyTaskBoTest
    {

        public SyTaskBoTest()
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

        #region 附加测试特性
        // 
        //编写测试时，还可使用以下特性:
        //
        //使用 ClassInitialize 在运行类中的第一个测试前先运行代码
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
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
        /// 返点测试
        /// </summary>
        [TestMethod()]
        public void RebatesRecordTaskTest()
        {
            //Hyt.BLL.SellBusiness.CrCustomerRebatesRecordBo.Instance.CrCustomerRebatesRecordToCustomer(14);
            Hyt.BLL.Order.SoOrderBo.Instance.AutoConfirmationOfReceipt(14);
        }
       

        /// <summary>
        ///AdjustmentGradeTask 的测试
        ///</summary>
        [TestMethod()]
        public void AdjustmentGradeTaskTest()
        {
            SyTaskBo target = new SyTaskBo(); // TODO: 初始化为适当的值
            int month = 12; // TODO: 初始化为适当的值
            target.AdjustmentGradeTask(month);
        }

        /// <summary>
        ///ProductIndex 的测试
        ///</summary>
        [TestMethod()]
        public void ProductIndexTest()
        {
            SyTaskBo target = new SyTaskBo(); // TODO: 初始化为适当的值
            target.ProductIndex();
        }

        [TestMethod()]
        public void CrCustomerRebatesRecordToCustomer()
        {
            Hyt.BLL.SellBusiness.CrCustomerRebatesRecordBo.Instance.CrCustomerRebatesRecordToCustomer(1,0);
        }
    }
}
