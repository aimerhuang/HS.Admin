using Hyt.BLL.CRM;
using Hyt.Model.WorkflowStatus;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Hyt.Model;
using System.Collections.Generic;

namespace Hyt.UnitTest
{

    /// <summary>
    ///这是 CrShoppingCartBoTest 的测试类，旨在
    ///包含所有 CrShoppingCartBoTest 单元测试
    ///</summary>
    [TestClass()]
    public class CrShoppingCartBoTest
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
            Infrastructure.Initialize.Init();
        }
        //使用 ClassCleanup 在运行完类中的所有测试后再运行代码
        [ClassCleanup()]
        public static void MyClassCleanup()
        {
        }

        //使用 TestInitialize 在运行每个测试前先运行代码
        [TestInitialize()]
        public void MyTestInitialize()
        {
        }

        //使用 TestCleanup 在运行完每个测试后运行代码
        [TestCleanup()]
        public void MyTestCleanup()
        {
        }

        #endregion

        /// <summary>
        ///Add 的测试
        ///</summary>
        [TestMethod()]
        public void Add()
        {
            CrShoppingCartBo target = new CrShoppingCartBo();
            target.Add(1060, 12, 1, CustomerStatus.购物车商品来源.PC网站);
        }

        /// <summary>
        ///UpdateQuantity 的测试
        ///</summary>
        [TestMethod()]
        public void UpdateQuantity()
        {
            CrShoppingCartBo target = new CrShoppingCartBo();
            target.UpdateQuantity(61, new int[] { 67 }, 8);
        }

        /// <summary>
        ///UncheckedItem 的测试
        ///</summary>
        [TestMethod()]
        public void UncheckedItem()
        {
            CrShoppingCartBo target = new CrShoppingCartBo();
            target.UncheckedItem(61, new int[] { 67 });
        }

        /// <summary>
        ///CheckedItem 的测试
        ///</summary>
        [TestMethod()]
        public void CheckedItem()
        {
            CrShoppingCartBo target = new CrShoppingCartBo();
            target.CheckedItem(1060, new int[] { 93 });
        }

        /// <summary>
        ///CheckedItem 的测试
        ///</summary>
        [TestMethod()]
        public void GetShoppingCart()
        {
            CrShoppingCartBo target = new CrShoppingCartBo();
            CrShoppingCart shoppingCart = target.GetShoppingCart(new[] { PromotionStatus.促销使用平台.PC商城 }, 1061);

            var sysno = new List<int>();
            foreach (var scg in shoppingCart.ShoppingCartGroups)
            {
                foreach (var sci in scg.ShoppingCartItems)
                {
                    if (sci.IsChecked == 1)
                    {
                        sysno.Add(sci.SysNo);
                    }
                }
            }

            //shoppingCart = target.GetShoppingCart(61, sysno.ToArray(), null, null, "", "");
            //shoppingCart = target.GetShoppingCart(61, sysno.ToArray(), null, null, null, "DSFDSFFFDS");
        }

        /// <summary>
        ///Remove 的测试
        ///</summary>
        [TestMethod()]
        public void Remove()
        {
            CrShoppingCartBo target = new CrShoppingCartBo();
            target.Remove(61, new int[] { 69 });
        }

        /// <summary>
        ///AddGift 的测试
        ///</summary>
        [TestMethod()]
        public void AddGift()
        {
            CrShoppingCartBo target = new CrShoppingCartBo();
            target.AddGift(1061, 61, 3, CustomerStatus.购物车商品来源.PC网站);
        }

        /// <summary>
        ///Add 的测试
        ///</summary>
        [TestMethod()]
        public void AddTest()
        {
            CrShoppingCartBo target = new CrShoppingCartBo();
            int customerSysNo = 196;
            int groupSysNo = 66;
            int quantity = 1;
            int promotionSysNo = 24;//24团购，42组合
            CustomerStatus.购物车商品来源 source = new CustomerStatus.购物车商品来源();
            target.Add(customerSysNo, groupSysNo, quantity, promotionSysNo, source);
        }

        #region "获取购物车"

        /// <summary>
        ///获取购物车
        ///</summary>
        [TestMethod()]
        public void GetShoppingCartByOrder()
        {
            CrShoppingCartBo target = new CrShoppingCartBo();
            int customerSysNo = 1061;
            int orderSysNo = 2027;
            //var shppingCart = target.GetShoppingCartByOrder(customerSysNo,orderSysNo);
        }

        #endregion

    }
}
