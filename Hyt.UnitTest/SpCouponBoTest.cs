﻿using Hyt.BLL.CRM;
using Hyt.BLL.Promotion;
using Hyt.Model.WorkflowStatus;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Hyt.Model;
using System.Collections.Generic;

namespace Hyt.UnitTest
{

    /// <summary>
    ///这是 SpCouponBoTest 的测试类，旨在
    ///包含所有 SpCouponBoTest 单元测试
    ///</summary>
    [TestClass()]
    public class SpCouponBoTest
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
        ///GetCurrentCartValidCoupons 的测试
        ///</summary>
        [TestMethod()]
        public void GetCurrentCartValidCouponsTest()
        {
            CrShoppingCartBo target1 = new CrShoppingCartBo();

            SpCouponBo target = new SpCouponBo();
            int customerSysNo = 196;
            IList<SpCoupon> actual;
            actual = target.GetCurrentCartValidCoupons(customerSysNo,
                                                       target1.GetShoppingCart(
                                                           new[]
                                                               {
                                                                   PromotionStatus.促销使用平台.PC商城,
                                                                   PromotionStatus.促销使用平台.门店,
                                                                   PromotionStatus.促销使用平台.手机商城
                                                               }, customerSysNo),
                                                       new[]
                                                           {
                                                               PromotionStatus.促销使用平台.PC商城, PromotionStatus.促销使用平台.门店,
                                                               PromotionStatus.促销使用平台.手机商城
                                                           });
        }
    }
}
