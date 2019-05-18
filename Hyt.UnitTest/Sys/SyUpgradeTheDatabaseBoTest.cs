using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hyt.BLL.Base;
using Hyt.BLL.Sys;

namespace Hyt.UnitTest.Sys
{
    [TestClass]
    public class SyUpgradeTheDatabaseBoTest
    {
        public SyUpgradeTheDatabaseBoTest()
        {
           DataProviderBo.Set(Activator.CreateInstance(Type.GetType("Hyt.DataAccess.Oracle.DataProvider,Hyt.DataAccess.Oracle")));
        }

        [TestMethod()]
        public void UpgradeTest()
        {
            SyUpgradeTheDatabaseBo.Upgrade();
        }
        
    }
}
