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
    #region 京东订单测试

    [TestClass]
    public class JingDongUpGradeTest
    {
        #region 附加测试特性
        [ClassInitialize]
        public static void MyClassInitialize(TestContext testContext)
        {
            DataProviderBo.Set(Activator.CreateInstance(Type.GetType("Hyt.DataAccess.Oracle.DataProvider,Hyt.DataAccess.Oracle")));
        }
        #endregion

        #region 批量获取指定时间区间的订单测试


        [TestMethod]
        public void test()
        {

           var dd= JObject.Parse("{\"1\":{\"ce\":2},\"1\":{\"d\":3}}");
         
           string ce=dd["1"]["d"].ToString();
        }
        [TestMethod]
        public void GetOrderList()
        {
            //var parameters = new OrderParameters();
            //parameters.EndDate = DateTime.Now;
            //var threeMallSyncLogInfo = BLL.Distribution.DsThreeMallSyncLogBo.Instance.GetThreeMallSyncLogInfo(0, 5, 0);

            //if (threeMallSyncLogInfo == null)
            //{
            //    threeMallSyncLogInfo = new Model.Stores.DsThreeMallSyncLog();
            //    threeMallSyncLogInfo.LastSyncTime = parameters.EndDate;
            //    threeMallSyncLogInfo.MallTypeSysNo = 5;
            //    threeMallSyncLogInfo.DealerSysNo = 0;
            //    threeMallSyncLogInfo.SyncType = 0;
            //    parameters.StartDate = DateTime.Now.AddYears(-100);
            //    BLL.Distribution.DsThreeMallSyncLogBo.Instance.Add(threeMallSyncLogInfo);
            //}
            //else
            //{
            //    threeMallSyncLogInfo.LastSyncTime = parameters.EndDate;
            //    BLL.Distribution.DsThreeMallSyncLogBo.Instance.Update(threeMallSyncLogInfo);
            //    parameters.StartDate = threeMallSyncLogInfo.LastSyncTime.AddDays(-1);
            //}


            var param = new OrderParameters()
            {
                PageIndex = 1,
                PageSize = 10,
            };
            var auth = new AuthorizationParameters()
            {
                AuthorizationCode = "be25938c-63b9-4338-bed0-2acb7770a9a3",
            };


            var j = new Extra.UpGrade.UpGrades.JingDongUpGrade();
            var result = j.GetOrderList(param, auth);

        }
        #endregion

        #region 获取单笔订单详情
        /// <summary>
        /// 获取单笔订单详情
        /// </summary>
        /// <param name="param">参数实体</param>
        /// <param name="auth">授权参数</param>
        /// <returns>单笔订单详情</returns>
        /// <reamrks>2017-08-16 黄杰 创建</reamrks>
        [TestMethod]
        public void GetOrderDetail()
        {
            var param = new OrderParameters()
            {
                PageIndex = 1,
                PageSize = 10,
                OrderID = "60261546534",
            };
            var auth = new AuthorizationParameters()
            {
                AuthorizationCode = "be25938c-63b9-4338-bed0-2acb7770a9a3",
            };


            var j = new Extra.UpGrade.UpGrades.JingDongUpGrade();
            var result = j.GetOrderDetail(param, auth);
        }
        #endregion

        #region 京东出库发货
        [TestMethod]
        public void SendDelivery()
        {
            //订单号：MallOrderId   物流公司ID:CompanyCode  运单号：HytExpressNo

            //Extra.UpGrade.Provider.UpGradeProvider.GetInstance((int)DistributionStatus.商城类型预定义.京东商城).SendDelivery(
            //     new Extra.UpGrade.Model.DeliveryParameters { CompanyCode = "1327", HytExpressNo = "7700030018409", MallOrderId = "63883901756" },
            //     null);

            Hyt.BLL.Logistics.LgDeliveryBo.Instance.SynchroOrder(9909);

            //Extra.UpGrade.Provider.UpGradeProvider.GetInstance((int)DistributionStatus.商城类型预定义.京东商城).SendDelivery(
            //      new Extra.UpGrade.Model.DeliveryParameters { CompanyCode = "1274", HytExpressNo = "70092526531314", MallOrderId = "60625990838" },
            //      null);

        }
        #endregion

        #region 使用授权码获取登录令牌
        /// <summary>
        /// 使用授权码获取登录令牌
        /// </summary>
        [TestMethod]
        public void GetAuthorizationCode()
        {
            var B2BUpGrade = new Extra.UpGrade.UpGrades.B2BUpGrade();
            OrderParameters param =new OrderParameters();
            AuthorizationParameters auth =new AuthorizationParameters();
            var result = B2BUpGrade.GetUpGradedWaitSend(param, auth);
        }
        #endregion

    }
    #endregion
}
