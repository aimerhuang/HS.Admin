using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hyt.BLL.Base;
using Extra.UpGrade.Model;
using Hyt.Model.WorkflowStatus;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.UnitTest.UpGrade
{
    [TestClass]
    public class TaobaoUpGradeTest
    {

        #region 附加测试特性
        [ClassInitialize]
        public static void MyClassInitialize(TestContext testContext)
        {
            DataProviderBo.Set(Activator.CreateInstance(Type.GetType("Hyt.DataAccess.Oracle.DataProvider,Hyt.DataAccess.Oracle")));
        }
        #endregion

        /// <summary>
        /// 获取指定时间区间的订单
        /// </summary>
        [TestMethod]
        public void GetOrderList()
        {
            Extra.UpGrade.UpGrades.TaobaoUpGrade t = new Extra.UpGrade.UpGrades.TaobaoUpGrade();
            var result = t.GetOrderList(null, null);
        }

        /// <summary>
        /// 联系发货
        /// </summary>
        [TestMethod]
        public void SendDelivery()
        {
            Extra.UpGrade.Provider.UpGradeProvider.GetInstance((int)DistributionStatus.商城类型预定义.淘宝分销).SendDelivery(
            new Extra.UpGrade.Model.DeliveryParameters { CompanyCode = "3", HytExpressNo = "00777777777777", MallOrderId = "E20160801181043085576557" },
            null);

            Extra.UpGrade.Provider.UpGradeProvider.GetInstance((int)DistributionStatus.商城类型预定义.天猫商城).SendDelivery(
                new Extra.UpGrade.Model.DeliveryParameters { CompanyCode = "3", HytExpressNo = "00777777777777", MallOrderId = "E20160801181043085576557" },
                null);
        }

    }
}
