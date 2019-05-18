using Hyt.DataAccess.Oracle.Warehouse;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Hyt.Model;
using Hyt.DataAccess.Oracle;
using Hyt.Model.WorkflowStatus;
using Hyt.BLL.Warehouse;

namespace Hyt.UnitTest
{
    [TestClass]
    public class CommodityWarehousingTest
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
        [ClassInitialize]
        public static void MyClassInitialize(TestContext testContext)
        {
            Hyt.Infrastructure.Initialize.Init();
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
        /// 获取入库单列表
        /// </summary>
        [TestMethod]
        public void GettWhStockInListTest()
        {
            //var daoImpl = new InStockDaoImpl();
            //var list = daoImpl.GetWhStockInList(0, 1, 1, DateTime.Now, 1, 10);
            //Assert.IsTrue(list.Count > 0);
        }
        /// <summary>
        /// 添加入库单
        /// </summary>
        [TestMethod]
        public void InserttWhStockInTest()
        {
            var daoImpl = new InStockDaoImpl();
            var model = new WhStockIn();
            //model.InStockroomType = 1;
            model.SourceSysNO = 123456;
            model.SourceType = 1;
            model.Remarks = "备注";
            model.Status = (int)WarehouseStatus.入库单状态.待入库;
            model.TransactionSysNo = "SW000001";
            model.WarehouseSysNo = 1;
            var result = daoImpl.InsertWhStockIn(model);
            Assert.IsTrue(result > 0);
        }
        /// <summary>
        /// 入库单添加商品
        /// </summary>
        [TestMethod]
        public void InsertWhStockInItem()
        {
            //var daoImpl = new InStockDaoImpl();
            //for (int i = 0; i < 10; i++)
            //{
            //    WhStockInItem item = new WhStockInItem();
            //    item.StockInSysNo = 1;
            //    item.ProductSysNo = 1;
            //    item.ProductName = string.Format("充电器{0}型", i);
            //    item.ProductSysNo = i + 1;
            //    item.CreatedDate = DateTime.Now;
            //    item.StockInQuantity = 10;
            //    int id = daoImpl.InsertWhStockInItem(item);
            //    Assert.IsTrue(id > 0);
            //}
        }
        /// <summary>
        /// 作废入库单
        /// </summary>
        [TestMethod]
        public void CancelInStockTest()
        {
            //var daoImpl = new InStockDaoImpl();
            //int id = 2;
            //var model = daoImpl.GetWhStockIn(id);
            //if (model != null)
            //{
            //    model.Status = (int)WarehouseStatus.入库单状态.作废;
            //    var result = daoImpl.CancelWhStockIn(model);
            //    Assert.IsTrue(result);
            //}
        }
    }
}
