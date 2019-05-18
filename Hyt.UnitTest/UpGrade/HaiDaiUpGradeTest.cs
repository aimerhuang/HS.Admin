using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hyt.BLL.Base;
using Extra.UpGrade.Model;
using Hyt.Model.WorkflowStatus;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model.UpGrade;

namespace Hyt.UnitTest.UpGrade
{
    #region 海带订单测试
    [TestClass]
    public class HaiDaiUpGradeTest
    {
        #region 附加测试特性
        [ClassInitialize]
        public static void MyClassInitialize(TestContext testContext)
        {
            DataProviderBo.Set(Activator.CreateInstance(Type.GetType("Hyt.DataAccess.Oracle.DataProvider,Hyt.DataAccess.Oracle")));
        }
        #endregion

        #region 格格家批量获取指定时间区间的订单
        private int GetUpGradeOrderId(List<UpGradeOrder> upGradeOrderList,string sn= "20180421629246910")
        {
            foreach (var order in upGradeOrderList)
            {
                var orderInfo=order.UpGradeOrderItems.Where(x => x.SN ==sn).FirstOrDefault();
                if (orderInfo != null)
                    return  int.Parse(orderInfo.OrderId);
            }
            return 0;
        }
        [TestMethod]
        public void GetOrderList()
        {

            int mallType = (int)DistributionStatus.商城类型预定义.海带网;
            var mallList = BLL.Distribution.DsDealerMallBo.Instance.GetDealerMallByMallTypeSysNo(mallType);
            foreach (var mallInfo in mallList)
            {
                var appInfo = BLL.Distribution.DsDealerAppBo.Instance.GetDsDealerApp(mallInfo.DealerAppSysNo);
                var auth = new AuthorizationParameters()
                {
                    MallType = mallType,
                    ShopAccount = mallInfo.ShopAccount,
                    DealerApp = appInfo,
                    AuthorizationCode = mallInfo.AuthCode,
                    DealerMall = mallInfo,
                };
                var param = new OrderParameters()
                {
                    PageIndex = 1,
                    PageSize = 1000,
                    StartDate = Convert.ToDateTime("2018-04-20 00:00:00"),
                    EndDate = Convert.ToDateTime("2018-04-28 00:00:00")
                };
                Extra.UpGrade.UpGrades.HaiDaiUpGrade g = new Extra.UpGrade.UpGrades.HaiDaiUpGrade();
                //var result = g.GetOrderDetail("20180421629246910", auth);
                var result= g.GetOrderList(param, auth);

                var orderList=result.Data.Where(x => x.MallOrderBuyer.MallOrderId == "20180421629246910").ToList();

                if (orderList != null && orderList.Count > 0)
                {
                    Hyt.BLL.Order.SoOrderBo.Instance.ImportMallOrder(orderList, null, mallInfo.DefaultWarehouse); 
                }
                //int orderId=GetUpGradeOrderId(result.Data, "20180421629246910");
                //if (orderId > 0)
                //{
                //    var _result = g.GetOrderDetail(orderId.ToString(), auth);

                       
                //}
               

                //result.Data.ed

            }

            //var param = new OrderParameters()
            //{
            //    PageIndex = 1,
            //    PageSize = 10,
            //    StartDate = Convert.ToDateTime("2018-04-16 00:00:00"),
            //    EndDate = Convert.ToDateTime("2018-04-18 00:00:00")
            //};
            //var appInfo = BLL.Distribution.DsDealerAppBo.Instance.GetDsDealerApp(9);
            //var auth = new AuthorizationParameters()
            //{
            //    MallType = 0,
            //    ShopAccount ="",
            //    DealerApp = appInfo,
            //    AuthorizationCode ="",
            //    DealerMall = null,
            //};

            //Extra.UpGrade.UpGrades.GeGeJiaUpGrade g = new Extra.UpGrade.UpGrades.GeGeJiaUpGrade();
            //var result = g.GetOrderList(param, auth);
        }



        #endregion

        #region 格格家联系发货（线下物流）
        [TestMethod]
        public void SendDelivery()
        {
            Extra.UpGrade.Provider.UpGradeProvider.GetInstance((int)DistributionStatus.商城类型预定义.格格家).SendDelivery(
                  new Extra.UpGrade.Model.DeliveryParameters { CompanyCode = "百世汇通", HytExpressNo = "70092528445198", MallOrderId = "170818459819521" },
                  null);
        }
        #endregion

    }
    #endregion
}
