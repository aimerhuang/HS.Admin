using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hyt.Model;
using Hyt.BLL.RMA;
using Hyt.Model.WorkflowStatus;
using System.Collections.Generic;

namespace Hyt.UnitTest
{
    /// <summary>
    ///这是 RmaBoTest 的测试类，旨在
    ///包含所有 RmaBo 单元测试
    ///</summary>
    [TestClass]
    public class RmaBoTest
    {
        #region 初始化
        public RmaBoTest()
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
        /// <summary>
        /// GetRMA 测试
        /// </summary>
        [TestMethod]
        public void GetRMATest()
        {
            CBRcReturn entity = Hyt.BLL.RMA.RmaBo.Instance.GetRMA(1000);
            TestContext.WriteLine("退换货单编号:" + entity.SysNo);
            TestContext.WriteLine("退换货单明细数量:" + entity.RMAItems.Count);
        }
        /// <summary>
        /// GetRcReturnEntity 测试
        /// </summary>
        [TestMethod]
        public void GetRcReturnEntityTest()
        {
            RcReturn entity = Hyt.BLL.RMA.RmaBo.Instance.GetRcReturnEntity(1000);
            TestContext.WriteLine("退换货单编号:" + entity.SysNo);
        }

        /// <summary>
        /// GetCanReturnStockOutList 测试
        /// </summary>
        [TestMethod]
        public void GetCanReturnStockOutListTest()
        {
            var list = Hyt.BLL.RMA.RmaBo.Instance.GetCanReturnStockOutList(1160, 15);
            TestContext.WriteLine("出库单数量:" + list.Count);
            if (list.Count > 0)
            {
                TestContext.WriteLine("出库商品:" + list[0].Items[0].ProductName);
            }
        }

        [TestMethod]
        public void GetRmasTest()
        {
            var filter = new Hyt.Model.Parameter.ParaRmaFilter();
            var r = Hyt.BLL.RMA.RmaBo.Instance.GetRmasForCallCenter(filter);
            TestContext.WriteLine("记录总数:" + r.Rows.Count);
        }

        [TestMethod]
        public void GetPickupTypeListByWarehouseTest()
        {
            var r = Hyt.BLL.Warehouse.WhWarehouseBo.Instance.GetPickupTypeListByWarehouse(1078);
            TestContext.WriteLine("记录总数:" + r.Count);
        }
        [TestMethod]
        public void GetLogList()
        {
            var a = Hyt.BLL.RMA.RmaBo.Instance.GetLogList(1046);
            TestContext.WriteLine("记录总数:" + a.Count);
            var b = Hyt.BLL.RMA.RmaBo.Instance.GetLogList("dad833aada084b88a0434ac4371d6749");
            TestContext.WriteLine("记录总数:" + b.Count);
        }
        [TestMethod]
        public void RMAInStockCallBackTest()
        {
            var user = Hyt.DataAccess.Sys.ISyUserDao.Instance.GetSyUser(1);
            Hyt.BLL.RMA.RmaBo.Instance.RMAInStockCallBack(1, user);
        }
        [TestMethod]
        public void GetProductErpCodeTest()
        {
            TestContext.WriteLine("产品编号:" + Hyt.BLL.Product.PdProductBo.Instance.GetProductErpCode(-1));
        }

        /// <summary>
        ///Get 的测试
        ///</summary>
        [TestMethod()]
        public void GetTest()
        {
            RmaBo target = new RmaBo(); // TODO: 初始化为适当的值
            int stockOutItemSysNo = 1884; // TODO: 初始化为适当的值

            Nullable<RmaStatus.退换货申请单来源> sourceType = new Nullable<RmaStatus.退换货申请单来源>(); // TODO: 初始化为适当的值

            IList<RcReturn> actual;
            //actual = target.Get(stockOutItemSysNo, RmaStatus.退换货申请单来源.部分签收);
            actual = target.Get(stockOutItemSysNo, sourceType);
            Assert.IsTrue(actual.Count > 0);

        }

        [TestMethod()]
        public void CalculateRmaAmountTest()
        {
            RmaBo target = new RmaBo();
            int orderSysNo = 2033;
            Dictionary<int, int> orderItemSysNo = new Dictionary<int, int>();
            orderItemSysNo.Add(3715, 1);
            var actual = target.CalculateRmaAmount(orderSysNo, orderItemSysNo);
            Assert.IsTrue(actual != null);
        }
    }
}
