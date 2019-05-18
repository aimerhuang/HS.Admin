using Hyt.BLL.Base;
using Hyt.BLL.Warehouse;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Hyt.Model.WorkflowStatus;
using Hyt.Model;
using System.Collections.Generic;
using Hyt.Model.Transfer;
using Hyt.Infrastructure.Pager;
using Hyt.DataAccess.Warehouse;

namespace Hyt.UnitTest
{
    
    
    /// <summary>
    ///这是 WhWarehouseBoTest 的测试类，旨在
    ///包含所有 WhWarehouseBoTest 单元测试
    ///</summary>
    /// <remarks></remarks>
    [TestClass()]
    public class WhWarehouseBoTest
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
        ///GetWhWareHouse 的测试
        ///</summary>
        [TestMethod()]
        public void GetWhWareHouseTest()
        {
            WhWarehouseBo target = new WhWarehouseBo(); // TODO: 初始化为适当的值
            Nullable<int> supportArea = 901019; // TODO: 初始化为适当的值
            Nullable<WarehouseStatus.仓库类型> warehouseType = new Nullable<WarehouseStatus.仓库类型>(); // TODO: 初始化为适当的值
            Nullable<int> deliveryType = new Nullable<int>(); // TODO: 初始化为适当的值
            IList<WhWarehouse> expected = null; // TODO: 初始化为适当的值
            IList<WhWarehouse> actual;
            actual = target.GetWhWareHouse(supportArea, warehouseType, deliveryType);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///GetWarehouseForArea 的测试
        ///</summary>
        [TestMethod()]
        public void GetWarehouseForAreaTest()
        {
            WhWarehouseBo target = new WhWarehouseBo();  
    
            int pageIndex =1; 
            int pageSize = 5;

            PagedList<CBAreaWarehouse> actual = target.GetWarehouseForArea(9490, pageIndex, pageSize);
            Assert.IsNotNull( actual);
          

        }
        /// <summary>
        /// 更新仓库库存
        /// </summary>
        /// <remarks>2016-2-27 杨浩 创建</remarks>
        [TestMethod()]
        public void UpdateStockQuantity()
        {
            int quantity = 3;
            int warehouseSysNo = 31;
            int productSysNo = 4309;
            int row = IPdProductStockDao.Instance.UpdateStockQuantity(warehouseSysNo, productSysNo, quantity);

            //没更新则添加负的库存
            if (row <= 0)
            {
                var productStockInfo = new PdProductStock();
                productStockInfo.StockQuantity = -quantity;
                productStockInfo.WarehouseSysNo = warehouseSysNo;
                productStockInfo.PdProductSysNo = productSysNo;
                productStockInfo.LastUpdateDate = DateTime.Now;
                productStockInfo.LastUpdateBy = 0;
                productStockInfo.CreatedBy = productStockInfo.LastUpdateBy;
                productStockInfo.CreatedDate = DateTime.Now;
                IPdProductStockDao.Instance.Insert(productStockInfo);

            }
        }
    }
}
