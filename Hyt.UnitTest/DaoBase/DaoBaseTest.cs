using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hyt.BLL.Base;
using System.Data;
using System.Collections.Generic;
using System.Linq;
namespace Hyt.UnitTest.DaoBase
{
    /// <summary>
    /// 数据抽象类测试
    /// </summary>
    /// <remarks>2016-12-13 杨浩 创建</remarks>
    [TestClass]
    public class DaoBaseTest : Hyt.DataAccess.Base.DaoBase<DaoBaseTest>
    {
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            DataProviderBo.Set(Activator.CreateInstance(Type.GetType("Hyt.DataAccess.Oracle.DataProvider,Hyt.DataAccess.Oracle")));
        }

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
        [TestMethod]
        public void TestMethod1()
        {
            string sqlStr = @"select 
                        SUM(SoOrderItem.Quantity) as SalesQuantity, PdProduct.SysNo as ProductSysNo
                       
                        from 
                        SoOrder inner join SoOrderItem on SoOrderItem.OrderSysNo=SoOrder.SysNo inner join PdProduct on SoOrderItem.ProductSysNo=PdProduct.SysNo  where SoOrder.Status>=30 and SoOrder.PayStatus=20 

						group by PdProduct.ErpCode,PdProduct.EasName,PdProduct.Barcode, PdProduct.SysNo  order by SalesQuantity Desc";
           var proudctSalesList=Context.Sql(sqlStr).QuerySingle<DataTable>();

           var productStatisticsList = Context.Sql("select ProductSysNo from PdProductStatistics").QueryMany<int>();

           foreach (DataRow row in proudctSalesList.Rows)
           {
               string sqlSetStr = "update PdProductStatistics set [Sales]=" + row["SalesQuantity"].ToString() + " where [ProductSysNo]=" + row["ProductSysNo"].ToString();

               if (!productStatisticsList.Any(x => x.ToString() == row["ProductSysNo"].ToString()))
               {
                   sqlSetStr = string.Format("INSERT INTO [PdProductStatistics] ([ProductSysNo],[Sales],[Liking],[Favorites],[Comments],[Shares],[Question],[TotalScore],[AverageScore]) VALUES ({0},{1},0,0,0,0,0,0,0)", row["ProductSysNo"].ToString(), row["SalesQuantity"].ToString());
               }
               Context.Sql(sqlSetStr).Execute();
           }
           
        }
    }
}
