using System.Collections.Generic;
using Hyt.DataAccess.Logistics;
using Hyt.Model.SystemPredefined;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Hyt.Model;

namespace Hyt.UnitTest
{
    
    
    /// <summary>
    ///这是 ILgPickUpDaoTest 的测试类，旨在
    ///包含所有 ILgPickUpDaoTest 单元测试
    ///</summary>
    [TestClass()]
    public class ILgPickUpDaoTest
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

        internal virtual ILgPickUpDao CreateILgPickUpDao()
        {
            // TODO: 实例化相应的具体类。
            ILgPickUpDao target = ILgPickUpDao.Instance;
            return target;
        }

        /// <summary>
        ///Create 的测试
        ///</summary>
        [TestMethod()]
        public void CreateTest()
        {
            ILgPickUpDao target = CreateILgPickUpDao(); // TODO: 初始化为适当的值

            LgPickUp model = new LgPickUp
                {
                    CreatedBy = 1,
                    CreatedDate = DateTime.Now,
                    PickupAddressSysNo = 0,
                    PickupTypeSysNo = PickupType.百城当日取件,
                    Remarks = "test",
                    Status = (int) Model.WorkflowStatus.LogisticsStatus.取件单状态.待取件,
                    StockInSysNo = 1,
                    WarehouseSysNo = 1078,
                Items=new List<LgPickUpItem>{
                    new LgPickUpItem{    CreatedBy = 1,
                    CreatedDate = DateTime.Now,
                     ProductQuantity=1,
                      ProductName="备电 4400mAh",
                      ProductSysNo=1
                    },
                    new LgPickUpItem{    CreatedBy = 1,
                    CreatedDate = DateTime.Now,
                     ProductQuantity=1,
                      ProductName="iPhone4 4S保护壳 京剧脸谱 红脸",
                      ProductSysNo=2
                    },
                    new LgPickUpItem{    CreatedBy = 1,
                    CreatedDate = DateTime.Now,
                     ProductQuantity=1,
                      ProductName="三星i8190保护壳(超薄磨砂)白色",
                      ProductSysNo=3
                    },
                    new LgPickUpItem{    CreatedBy = 1,
                    CreatedDate = DateTime.Now,
                     ProductQuantity=1,
                      ProductName="三星SAMSUNG i939  手机电池",
                      ProductSysNo=4
                    },
                }

                };

            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.Create(model);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }
    }
}
