using Extra.UpGrade.Model;
using Hyt.BLL.Base;
using Hyt.Model.WorkflowStatus;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.UnitTest.UpGrade
{
    [TestClass()]
    public class YouZanUpGradeTest
    {
       
        #region 附加测试特性
        [ClassInitialize]
        public static void MyClassInitialize(TestContext testContext)
        {
            DataProviderBo.Set(Activator.CreateInstance(Type.GetType("Hyt.DataAccess.Oracle.DataProvider,Hyt.DataAccess.Oracle")));
        }
        #endregion

        [TestMethod()]
        public void DeliveryTest()
        {
            Extra.UpGrade.Provider.UpGradeProvider.GetInstance((int)DistributionStatus.商城类型预定义.有赞).SendDelivery(
                   new Extra.UpGrade.Model.DeliveryParameters { CompanyCode = "3", HytExpressNo = "00777777777777", MallOrderId = "E20160801181043085576557" },
                   null);
        }

        [TestMethod()]
        public void GetOrderListB2B()
        {
            OrderParameters parameters = new OrderParameters();
            Extra.UpGrade.UpGrades.B2BUpGrade g = new Extra.UpGrade.UpGrades.B2BUpGrade();
            var result = g.GetUpGradedWaitSend(parameters, null);
        }


        [TestMethod()]
        public void GetOrderList()
        {
            var parameters = new OrderParameters();
            parameters.EndDate = DateTime.Now;
            var threeMallSyncLogInfo = BLL.Distribution.DsThreeMallSyncLogBo.Instance.GetThreeMallSyncLogInfo(0, 5, 0);

            if (threeMallSyncLogInfo == null)
            {
                threeMallSyncLogInfo = new Model.Stores.DsThreeMallSyncLog();
                threeMallSyncLogInfo.LastSyncTime = parameters.EndDate;
                //threeMallSyncLogInfo.MallTypeSysNo = 5;
                //threeMallSyncLogInfo.DealerSysNo = 0;
                threeMallSyncLogInfo.SyncType = 0;
                parameters.StartDate = DateTime.Now.AddYears(-100);
                BLL.Distribution.DsThreeMallSyncLogBo.Instance.Add(threeMallSyncLogInfo);
            }
            else
            {
                threeMallSyncLogInfo.LastSyncTime = parameters.EndDate;
                BLL.Distribution.DsThreeMallSyncLogBo.Instance.Update(threeMallSyncLogInfo);
                parameters.StartDate = threeMallSyncLogInfo.LastSyncTime.AddDays(-1);
            }
          
           
            Extra.UpGrade.UpGrades.YouZanUpGrade g = new Extra.UpGrade.UpGrades.YouZanUpGrade();
            var result=g.GetOrderList(parameters, null);

            //BLL.Order.SoOrderBo.Instance.ImportMallOrder();
            //Hyt.Service.Implement.MallSeller.MallOrder mallOrder = new Service.Implement.MallSeller.MallOrder();
            //foreach (var item in result.Data)
            //{
            //    mallOrder.ImportMallOrder(item);
            //}
        
            //Assert.IsTrue(null!= null);
        }
    }
}
