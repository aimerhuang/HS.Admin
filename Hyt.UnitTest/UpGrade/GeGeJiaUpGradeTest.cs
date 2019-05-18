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
    #region 格格家订单测试
    [TestClass]
    public class GeGeJiaUpGradeTest
    {
        #region 附加测试特性
        [ClassInitialize]
        public static void MyClassInitialize(TestContext testContext)
        {
            DataProviderBo.Set(Activator.CreateInstance(Type.GetType("Hyt.DataAccess.Oracle.DataProvider,Hyt.DataAccess.Oracle")));
        }
        #endregion

        #region 格格家批量获取指定时间区间的订单
        [TestMethod]
        public void GetOrderList()
        {
            int mallType = (int)DistributionStatus.商城类型预定义.格格家;
            var mallList = BLL.Distribution.DsDealerMallBo.Instance.GetDealerMallByMallTypeSysNo(mallType);
            foreach (var mallInfo in mallList)
	        {
	        	    var param = new OrderParameters()
                    {
                        PageIndex = 1,
                        PageSize = 10,
                        StartDate = Convert.ToDateTime("2018-04-26 00:00:00"),
                        EndDate = Convert.ToDateTime("2018-05-01 00:00:00")
                    };
                    var appInfo = BLL.Distribution.DsDealerAppBo.Instance.GetDsDealerApp(9);
                    var auth = new AuthorizationParameters()
                    {
                        MallType = mallType,
                        ShopAccount = mallInfo.ShopAccount,
                        DealerApp = appInfo,
                        AuthorizationCode = mallInfo.AuthCode,
                        DealerMall = mallInfo,
                        //MallType = 0,
                        //ShopAccount ="",
                        //DealerApp = appInfo,
                        //AuthorizationCode ="",
                        //DealerMall = null,
                    };
            
                    Extra.UpGrade.UpGrades.GeGeJiaUpGrade g = new Extra.UpGrade.UpGrades.GeGeJiaUpGrade();
                    var result = g.GetOrderList(param, auth);
            
                    var orderList = result.Data.Where(x => x.MallOrderBuyer.MallOrderId == "180428878749325").ToList();
            
                    if (orderList != null && orderList.Count > 0)
                    {
                        Hyt.BLL.Order.SoOrderBo.Instance.ImportMallOrder(orderList, null, mallInfo.DefaultWarehouse);
                    }
	        }
          
        }
        #endregion

        #region 格格家联系发货（线下物流）
        [TestMethod]
        public void SendDelivery()
        {
            var appInfo = BLL.Distribution.DsDealerAppBo.Instance.GetDsDealerApp(9);
            var auth = new AuthorizationParameters()
            {
                MallType = 0,
                ShopAccount = "",
                DealerApp = appInfo,
                AuthorizationCode = "",
                DealerMall = null,
            };
            Extra.UpGrade.Provider.UpGradeProvider.GetInstance((int)DistributionStatus.商城类型预定义.格格家).SendDelivery(
                  new Extra.UpGrade.Model.DeliveryParameters { CompanyCode = "百世汇通", HytExpressNo = "70092528445198", MallOrderId = "170818459819521" },
                  auth);
        }
        #endregion

    }
    #endregion
}
