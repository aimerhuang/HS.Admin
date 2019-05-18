using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Hyt.Admin.Controllers;
using Hyt.BLL.Basic;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.WorkflowStatus;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hyt.UnitTest.ControllerTest
{
    [TestClass]
    public class BaseControllerTest
    {
        public TestContext TestContext { get; set; }

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            Infrastructure.Initialize.Init();
        }

        [TestMethod]
        public void PaymentTypeManage()
        {
            try
            {
                var filter = new ParaPaymentTypeFilter()
                {
                    PaymentName = "刷",
                    IsOnlineVisible = (int)BasicStatus.支付方式前台是否可见.是,
                    Status = (int)BasicStatus.支付方式状态.启用
                };

                var result = PaymentTypeBo.Instance.GetPaymentTypeList(filter);
                if (result != null) Assert.AreEqual("支付方式维护", result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [TestMethod]
        public void PaymentTypeCreate()
        {
            try
            {
                var model = new BsPaymentType()
                {
                    PaymentName = "TestPayment",
                    PaymentDescription = "TestPaymentDescription",
                    IsOnlinePay = (int)BasicStatus.支付方式是否网上支付.是,
                    IsOnlineVisible = (int)BasicStatus.支付方式前台是否可见.是,
                    PaymentType = (int)BasicStatus.支付方式类型.预付,
                    RequiredCardNumber = (int)BasicStatus.是否需要卡号.是,
                    DisplayOrder = 1,
                    Status = (int)BasicStatus.支付方式状态.启用
                };
                model.CreatedBy = model.LastUpdateBy = 1;
                model.CreatedDate = model.LastUpdateDate = DateTime.Now;
                var id = PaymentTypeBo.Instance.PaymentTypeCreate(model);
                Assert.IsTrue(id > 0);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [TestMethod]
        public void PaymentTypeEdit()
        {
            try
            {
                var model = PaymentTypeBo.Instance.GetEntity(1);
                if (model == null) return;
                model.Status = (int)BasicStatus.支付方式状态.启用;
                model.LastUpdateBy = 1;
                model.LastUpdateDate = DateTime.Now;
                var id = PaymentTypeBo.Instance.PaymentTypeUpdate(model);
                Assert.IsTrue(id > 0);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
