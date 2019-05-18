using System.Collections.Generic;
using Hyt.BLL.Warehouse;
using Hyt.DataAccess.Oracle.Warehouse;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.UnitTest.Warehouse
{
    [TestClass]
    public class OutStockDaoImplTest
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
            Infrastructure.Initialize.Init();
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
        public void SearchTest()
        {
            var daoImpl = new OutStockDaoImpl();
            var condition = new StockOutSearchCondition
                {
                    InvoiceSysNo = 1
                    
                };
            var list = daoImpl.Search(condition, 1, 10);
            //foreach (var cbWhStockOut in list)
            //{
            //    Assert.IsTrue(cbWhStockOut.InvoiceSysNo > 0);    
            //}
            
        }
        [TestMethod]
        public void UpdateTest()
        {
            var daoImpl = new OutStockDaoImpl();
            const int sysno = 4;
            var model = daoImpl.GetModel(sysno);

            model.CustomerMessage = "updated message";
            daoImpl.Update(model);

            var updateModel = daoImpl.GetModel(sysno);
            Assert.AreEqual(updateModel.CustomerMessage, model.CustomerMessage);
        }

        /// <summary>
        /// 使用复合类型CBWhStockOut来进行update
        /// </summary>
        [TestMethod]
        public void CBUpdateTest()
        {
            //var master = OutStockBo.Instance.Search(new StockOutSearchCondition
            //{
            //    StockOutSysNo = 1
            //}, 1, 10);
            //if (master == null || master.Count == 0)
            //{
            //    return ;
            //}

            //master[0].CustomerMessage = "updated message";
            //master[0].LastUpdateBy = 52;//todo: change to login user
            //master[0].LastUpdateDate = DateTime.Now;
            //var daoImpl = new OutStockDaoImpl();
            //daoImpl.Update(master[0]);

            //var updateModel = daoImpl.GetModel(1);
            //Assert.AreEqual(updateModel.CustomerMessage, master[0].CustomerMessage);
        }
        [TestMethod]
        public void InsertTest()
        {
            var daoImpl = new OutStockDaoImpl();
            var model = new WhStockOut
                {
                    TransactionSysNo = "T000001",
                    WarehouseSysNo = 1,
                    OrderSysNO = 3,
                    ReceiveAddressSysNo = 1,
                    IsCOD = 1,
                    StockOutAmount = new decimal(123.1)
                };

            model.Receivable = model.StockOutAmount;
            model.Status = 10;
            model.SignTime = DateTime.Now.AddDays(1);
            model.IsPrintedPackageCover = 1;
            model.IsPrintedPickupCover = 1;
            model.CustomerMessage = "insert test";
            model.Remarks = "备注";
            model.CreatedBy = 52;
            model.CreatedDate = DateTime.Now;
            //Todo:送货时间/周末周日
            //model.DeliveryTimeType = 1; 
            
            model.Status = (int)WarehouseStatus.入库单状态.待入库;
            model.Items = new List<WhStockOutItem>
                {
                    new WhStockOutItem
                        {
                            StockOutSysNo = model.SysNo,
                            TransactionSysNo = model.TransactionSysNo,
                            OrderSysNo = model.OrderSysNO,
                            ProductName = "test product name",
                            ProductSysNo = 1,
                            Weight = 12,

                            Measurement = "1inch",//
                            OriginalPrice = 100,
                            RealSalesAmount = 99,
                            
                            ProductQuantity = 1,
                            Remarks = "remark"
                        },
                    new WhStockOutItem
                        {
                            StockOutSysNo = model.SysNo,
                            TransactionSysNo = "T0000012",
                            OrderSysNo = model.OrderSysNO,
                            ProductName = "test product name 2",
                            ProductSysNo = 2,
                            Weight = 22,
                            Measurement = "2inch",
                            OriginalPrice = 200,
                            RealSalesAmount = 199,
                            ProductQuantity = 2,
                            Remarks = "remark"
                        }
                };
            var result = daoImpl.Insert(model);
            Assert.IsTrue(result > 0);
        }

        [TestMethod]
        public void RemoveItemTest()
        {
            var daoImpl = new OutStockDaoImpl();
            const int sysno = 22;
            var model = daoImpl.GetModel(sysno);
            if (model == null || model.Items == null || model.Items.Count == 0)
            {
                Assert.Fail("OutStock order havn't any items.");
            }

            daoImpl.RemoveItem(model.Items);
            
        }

        [TestMethod]
        public void GetModelTest()
        {
            var daoImpl = new OutStockDaoImpl();
            const int sysno = 22;
            var model = daoImpl.GetModel(sysno);

            Assert.IsNotNull(model);
        }
    }
}
