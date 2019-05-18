using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hyt.BLL.Base;
using Extra.UpGrade.Model;
using Hyt.Model.WorkflowStatus;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Hyt.UnitTest.UpGrade
{
    #region 一号店订单测试
    [TestClass]
    public class YihaodianUpGrade
    {
        #region 附加测试特性
        [ClassInitialize]
        public static void MyClassInitialize(TestContext testContext)
        {
            DataProviderBo.Set(Activator.CreateInstance(Type.GetType("Hyt.DataAccess.Oracle.DataProvider,Hyt.DataAccess.Oracle")));
        }
        #endregion

        #region 获取一号店指定时间区间的订单
        /// <summary>
        /// 获取一号店指定时间区间的订单
        /// </summary>
        /// <param name="param">参数实体</param>
        /// <param name="auth">授权参数</param>
        /// <returns>获取一号店指定时间区间的订单</returns>
        /// <reamrks>2017-08-23 黄杰 创建</reamrks>
        [TestMethod]
        public void GetOrderList()
        {
            var param = new OrderParameters()
            {
                PageIndex = 1,
                PageSize = 10,
                StartDate = Convert.ToDateTime("2017-08-15 00:00:00"),
                EndDate = DateTime.Now
            };

            string a = "1包装";
            int v = GetNumberInt(a);

            var y = new Extra.UpGrade.UpGrades.B2BUpGrade();
            var result = y.GetOrderList(param, null);
        }

        /// <summary>
        /// 获取字符串中的数字
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>数字</returns>
        public static int GetNumberInt(string str)
        {
            int result = 0;
            if (str != null && str != string.Empty)
            {
                // 正则表达式剔除非数字字符（不包含小数点.）
                str = Regex.Replace(str, @"[^\d.\d]", "");
                // 如果是数字，则转换为decimal类型
                if (Regex.IsMatch(str, @"^[+-]?\d*[.]?\d*$"))
                {
                    result = int.Parse(str);
                }
            }
            return result;
        }
        #endregion

        #region 一号店联系发货
        /// <summary>
        /// 一号店联系发货
        /// </summary>
        /// <param name="param">参数实体</param>
        /// <param name="auth">授权参数</param>
        /// <returns>获取一号店联系发货信息</returns>
        /// <reamrks>2017-08-24 黄杰 创建</reamrks>
        [TestMethod]
        public void SendDelivery()
        {
            //CompanyCode:  1751：中通速递(标准)、1759：EMS(标准)、1755：圆通速递(标准)、17331：优速快递(标准)、10299：天天快递(标准)
            //17313：DHL快递(标准)、1760：汇通快运(标准)、1756：顺丰速运(标准)、28324：快捷快递、1754：韵达快运(标准)

            Extra.UpGrade.Provider.UpGradeProvider.GetInstance((int)DistributionStatus.商城类型预定义.一号店).SendDelivery(
                  new Extra.UpGrade.Model.DeliveryParameters { CompanyCode = "1751", HytExpressNo = "536018315314", MallOrderId = "13850189947403" },
                  null);
        }
        #endregion

    }
    #endregion
}
