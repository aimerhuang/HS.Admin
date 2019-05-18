using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hyt.BLL.Base;
using Extra.UpGrade.Model;
using Hyt.Model.WorkflowStatus;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Hyt.UnitTest.UpGrade
{
    #region 苏宁易购订单测试
    [TestClass]
    public class SUNINGUpGradeTest
    {
        #region 附加测试特性
        [ClassInitialize]
        public static void MyClassInitialize(TestContext testContext)
        {
            DataProviderBo.Set(Activator.CreateInstance(Type.GetType("Hyt.DataAccess.Oracle.DataProvider,Hyt.DataAccess.Oracle")));
        }
        #endregion

        #region 苏宁批量获取指定时间区间的订单测试
        [TestMethod]
        public void GetOrderList()
        {
            var param = new OrderParameters()
            {
                PageIndex = 1,
                PageSize = 20,
                StartDate = Convert.ToDateTime("2016-01-01 00:00:00"),
                EndDate = DateTime.Now
            };

            var s = new Extra.UpGrade.UpGrades.B2BUpGrade();
            var result = s.GetOrderList(param, null);
        }
        #endregion

        #region 苏宁订单发货
        [TestMethod]
        public void SendDelivery()
        {
            var param = new OrderParameters()
            {
                PageIndex = 1,
                PageSize = 20,
                StartDate = Convert.ToDateTime("2017-09-01 00:00:00"),
                EndDate = Convert.ToDateTime("2017-09-14 00:00:00")
            };

            ////AAE-中国:A01    BHT:B01    百福东方:B02     中国邮政国内包裹/挂号信:B03    DHL:D01     D速快递:D02
            ////大田物流:D03    中环快递:HW7      EMS:E01     FedEx(国外):F02
            Extra.UpGrade.Provider.UpGradeProvider.GetInstance((int)DistributionStatus.商城类型预定义.苏宁易购).SendDelivery(
            new Extra.UpGrade.Model.DeliveryParameters { CompanyCode = "B03", HytExpressNo = "6512145351", MallOrderId = "60625990838" ,OrderParam=param},
            null);
        }
        #endregion

    }
    #endregion
}
