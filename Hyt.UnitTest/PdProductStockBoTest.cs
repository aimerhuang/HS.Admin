using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Syndication;
using System.ServiceModel.Web;
using System.Threading;
using System.Web;
using System.Web.SessionState;
using Hyt.BLL.Base;
using Hyt.BLL.Log;
using Hyt.BLL.Product;
using Hyt.Model.WorkflowStatus;
using Hyt.Service.Contract.LogisApp;
using Hyt.Service.Implement.LogisApp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using Hyt.Model;
using Moq;
using Newtonsoft.Json;
using Hyt.BLL.Warehouse;
using Hyt.DataAccess.Warehouse;

namespace Hyt.UnitTest
{
    
    
    /// <summary>
    ///这是 PdProductStockBoTest 的测试类，旨在
    ///包含所有 PdProductStockBoTest 单元测试
    ///</summary>
    [TestClass()]
    public class PdProductStockBoTest
    {
        public PdProductStockBoTest()
        {
            DataProviderBo.Set(Activator.CreateInstance(Type.GetType("Hyt.DataAccess.Oracle.DataProvider,Hyt.DataAccess.Oracle")));
        }

       

     
        [TestMethod]
        public void SynchronizeErpStockTest()
        {

           
            PdProductStockBo.Instance.SynchronizeErpStock(34);
            Assert.IsNotNull(null);
        }
        [TestMethod]
        public void SynchronizeStock()
        {
            int rows = IPdProductStockDao.Instance.SynchronizeStock(34, 705777, 48);
        }
    }
  

   

 

}
