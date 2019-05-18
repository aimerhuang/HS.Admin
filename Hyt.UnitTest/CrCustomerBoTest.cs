using Hyt.BLL.Base;
using Hyt.BLL.Web;
using Hyt.DataAccess.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
namespace Hyt.UnitTest
{
    
    
    /// <summary>
    ///这是 CrCustomerBoTest 的测试类，旨在
    ///包含所有 CrCustomerBoTest 单元测试
    ///</summary>
    [TestClass()]
    public class CrCustomerBoTest : DaoBase<CrCustomerBoTest>
    {
        public CrCustomerBoTest()
        {
            DataProviderBo.Set(Activator.CreateInstance(Type.GetType("Hyt.DataAccess.Oracle.DataProvider,Hyt.DataAccess.Oracle")));
        }
   

        /// <summary>
        ///GetVerifyMailorCellPhoneCount 的测试
        ///</summary>
        [TestMethod()]
        public void GetVerifyMailorCellPhoneCountTest()
        {
            CrCustomerBo target = new CrCustomerBo(); // TODO: 初始化为适当的值
            int sysNo = 1001; // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.GetSafePhoneVerifyCodeCount(sysNo);
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        /// 重置返利余额
        /// </summary>
        [TestMethod()]
        public void ResetBrokerage()
        {
            var rebatesRecordList = Context.Sql("select * from [CrCustomerRebatesRecord]")
                .QueryMany<Hyt.Model.CrCustomerRebatesRecord>();
            var customerList = Context.Sql("select * from CrCustomer where sysNo in( select RecommendSysNo from [CrCustomerRebatesRecord] group by [RecommendSysNo])")
                .QueryMany<Hyt.Model.CrCustomer>();
            foreach (var customerInfo in customerList)
            {
                var brokerage=rebatesRecordList.Where(x => x.RecommendSysNo == customerInfo.SysNo && x.Status == "1").Sum(x => x.Rebates);
                var brokerageTotal = rebatesRecordList.Where(x => x.RecommendSysNo == customerInfo.SysNo && (x.Status == "1" || x.Status == "0")).Sum(x => x.Rebates);
                var brokerageFreeze = rebatesRecordList.Where(x => x.RecommendSysNo == customerInfo.SysNo && x.Status == "0").Sum(x => x.Rebates);
                //Context.Sql("update CrCustomer set BrokerageTotal=" + brokerageTotal + ",Brokerage=" + brokerage + ",BrokerageFreeze=" + brokerageFreeze + " where sysNo=" + customerInfo.SysNo).Execute();
            }
        }
    }
}
