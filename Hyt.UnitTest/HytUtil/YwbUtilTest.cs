using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hyt.UnitTest.HytUtil
{
    [TestClass]
    public class YwbUtilTest
    {
        [TestMethod]
        public void Test_Enter_Exit()
        {

           string pwd=Util.EncryptionUtil.EncryptWithMd5AndSalt
                        ("123456");
            var dd=typeof(Hyt.Model.Transfer.CBSoOrderItem).GetProperties();


            //Assert.IsTrue(Hyt.Util.YwbUtil.Enter("ywb", 10));
            
            //Assert.IsFalse(Hyt.Util.YwbUtil.Enter("ywb", 10));
            //System.Threading.Thread.Sleep(1000 * 2);
            //Assert.IsFalse(Hyt.Util.YwbUtil.Enter("ywb", 10));

            //Assert.IsTrue(Hyt.Util.YwbUtil.Enter("ywb1"));
            //Assert.IsTrue(Hyt.Util.YwbUtil.Enter("ywb2"));

            //Hyt.Util.YwbUtil.Exit("ywb");

            //Assert.IsTrue(Hyt.Util.YwbUtil.Enter("ywb"));

            http://admin.fanlaigo.com/DsPosManage/WXPay
            string postJson= "{\"itemsList\":[{\"LocalProSysNo\":73073,\"ServiceProSysNo\":9208,\"SysNo\":0,\"pSysNo\":0,\"ProSysNo\":9208,\"ProName\":\"三利和酱汁鱼牛油味\",\"ProBarCode\":\"6940480001248\",\"ProPrice\":0.01,\"ProNum\":1,\"ProDiscount\":10,\"ProDisPrice\":0.009,\"ProTotalValue\":0.01,\"SellOrderNumber\":null,\"WareNum\":0}],\"key\":\"11\",\"Mac\":\"08:62:66:81:F5:10\",\"PosName\":\"111\",\"items\":\"[{\\\"LocalProSysNo\\\":73073,\\\"ServiceProSysNo\\\":9208,\\\"SysNo\\\":0,\\\"pSysNo\\\":0,\\\"ProSysNo\\\":9208,\\\"ProName\\\":\\\"三利和酱汁鱼牛油味\\\",\\\"ProBarCode\\\":\\\"6940480001248\\\",\\\"ProPrice\\\":0.01,\\\"ProNum\\\":1,\\\"ProDiscount\\\":10,\\\"ProDisPrice\\\":0.009,\\\"ProTotalValue\\\":0.01,\\\"SellOrderNumber\\\":null,\\\"WareNum\\\":0}]\",\"PayAuthCode\":\"130206211988611474\",\"SysNo\":0,\"DsSysNo\":0,\"DsPosSysNo\":0,\"SerialNumber\":\"SO160929180149\",\"Clerker\":\"收银员\",\"SaleTime\":\"2016-09-29 18:02:07\",\"TotalNum\":1,\"TotalSellValue\":0.01,\"TotalOrigValue\":0.01,\"TotalDisValue\":0.00,\"TotalPayValue\":0.010,\"TotalGetValue\":0.000,\"OnLineWebType\":0,\"CardNumber\":\"\",\"CardName\":\"\",\"HavePrivilege\":0,\"OrderPoint\":0,\"UsePoint\":0,\"PointMoney\":0,\"CouponSysNo\":0,\"CouponSysDis\":null,\"PayType\":null,\"PayTime\":\"2016-9-29 00:00:00\"}";

        }
        [TestMethod]
        public void DelAllWebSiteCache()
        {
            Hyt.BLL.Sys.MemoryBo.Instance.DelAllWebSiteCache();
        }
    }
}
