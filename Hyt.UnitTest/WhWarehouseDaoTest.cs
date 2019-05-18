using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.BLL.Base;
using Hyt.DataAccess.Warehouse;
using Hyt.Model.WorkflowStatus;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hyt.UnitTest
{
    [TestClass]
    public class WhWarehouseDaoTest
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
            DataProviderBo.Set(Activator.CreateInstance(Type.GetType("Hyt.DataAccess.Oracle.DataProvider,Hyt.DataAccess.Oracle")));

        }
        #endregion

        [TestMethod]
        public void GetProductLendGoodsTest()
        {
            var list = IWhWarehouseDao.Instance.GetProductLendGoods(41, 1, null, WarehouseStatus.借货单状态.已出库, ProductStatus.产品价格来源.配送员进货价);
        }

        [TestMethod]
        public void tt()
        {
            var list = IWhWarehouseDao.Instance.GetWarehouseForArea(new int[]{1}, 1, 10);
            Assert.IsNotNull(list);
        }

        [TestMethod]
        public void GetWhWareHouseTest()
        {
            var list = IWhWarehouseDao.Instance.GetWhWareHouse(110, WarehouseStatus.仓库类型.仓库, 10);//10: EMS快递
            var list2 = IWhWarehouseDao.Instance.GetWhWareHouse();
            var list3 = IWhWarehouseDao.Instance.GetWhWareHouse(901019);
            var list4 = IWhWarehouseDao.Instance.GetWhWareHouse(null, WarehouseStatus.仓库类型.门店);
            var list5 = IWhWarehouseDao.Instance.GetWhWareHouse(null, null, 11);
            var list6 = IWhWarehouseDao.Instance.GetWhWareHouse(901019, null, null, WarehouseStatus.仓库状态.启用);
            var list7 = IWhWarehouseDao.Instance.GetWhWareHouse(901019, null, null, WarehouseStatus.仓库状态.禁用);
        }
    }
}
