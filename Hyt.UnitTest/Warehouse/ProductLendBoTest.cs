using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.BLL.Warehouse;
using Hyt.Model.WorkflowStatus;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hyt.UnitTest.Warehouse
{
     [TestClass]
    public class ProductLendBoTest
    {
        /// <summary>
        ///获取或设置测试上下文，上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext { get; set; }

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
        #endregion

        [TestMethod]
        public void CompleteProductLendTest()
        {
            const int sysno = 28;
            var result = ProductLendBo.Instance.CompleteProductLend(sysno,0);
        }
         [TestMethod]
         public void ProductReturnTest()
         {
             var model = new Model.WhStockIn();
             model.CreatedBy = model.LastUpdateBy = 41;
             model.CreatedDate = model.LastUpdateDate = DateTime.Now;
             model.Status = (int)WarehouseStatus.入库单状态.待入库;
             model.SourceType = (int)WarehouseStatus.入库单据类型.借货单;
             model.IsPrinted = (int)WarehouseStatus.是否已经打印拣货单.否;
             model.DeliveryType = (int)WarehouseStatus.入库物流方式.还货;

             var result = ProductLendBo.Instance.ProductReturn(41, model);
         }
    }
}
