using System.Linq;
using Hyt.BLL.Base;
using Hyt.BLL.Order;
using Hyt.BLL.Product;
using Hyt.BLL.Promotion;
using Hyt.BLL.Sys;
using Hyt.DataAccess.CRM;
using Hyt.Infrastructure.Caching;
using Hyt.Infrastructure.Pager;
using Hyt.Model.Parameter;
using Hyt.Model.UpGrade;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Hyt.Model;
using System.Collections.Generic;

namespace Hyt.UnitTest
{
    
    
    /// <summary>
    ///这是 DsOrderBoTest 的测试类，旨在
    ///包含所有 DsOrderBoTest 单元测试
    ///</summary>
    [TestClass()]
    public class DsOrderBoTest
    {

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



        #region 附加测试特性
        [ClassInitialize]
        public static void MyClassInitialize(TestContext testContext)
        {
            DataProviderBo.Set(Activator.CreateInstance(Type.GetType("Hyt.DataAccess.Oracle.DataProvider,Hyt.DataAccess.Oracle")));
            Infrastructure.Initialize.Init();
        }
        //
        //使用 ClassCleanup 在运行完类中的所有测试后再运行代码
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //使用 TestInitialize 在运行每个测试前先运行代码
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //使用 TestCleanup 在运行完每个测试后运行代码
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

       
        /// <summary>
        ///AssignJobs 的测试
        ///</summary>
        [TestMethod()]
        public void GetImgFlag()
        {
            var img = Hyt.BLL.MallSeller.DsOrderBo.Instance.GetImgFlag(6, null, "3", null);
           // Assert.IsTrue(string.Equals(img, "tmall"));
           // img= Hyt.BLL.MallSeller.DsOrderBo.Instance.GetImgFlag(1, "升舱服务", "4", new List<UpGradeOrderItem>
           // {new UpGradeOrderItem() {
           //     MallProductAttrs = "升舱"
           // }
           // });
           //Assert.IsTrue(string.Equals(img, "tmall"));
           // img = Hyt.BLL.MallSeller.DsOrderBo.Instance.GetImgFlag(6, "升舱服务", "1", new List<UpGradeOrderItem>
           // {new UpGradeOrderItem() {
           //     MallProductAttrs = ""
           // }
           // });
           // Assert.IsTrue(string.Equals(img, "tmallup"));
           //img = Hyt.BLL.MallSeller.DsOrderBo.Instance.GetImgFlag(6, "", "4", new List<UpGradeOrderItem>
           // {new UpGradeOrderItem() {
           //     MallProductAttrs = ""
           // }
           // });
           //Assert.IsTrue(string.Equals(img, "tmallup"));
           //img = Hyt.BLL.MallSeller.DsOrderBo.Instance.GetImgFlag(6, "", "3", new List<UpGradeOrderItem>
           // {new UpGradeOrderItem() {
           //     MallProductAttrs = "升舱"
           // }
           // });
           //Assert.IsTrue(string.Equals(img, "tmallup"));

           //Assert.IsTrue(string.Equals(img, "tmallup"));
           img = Hyt.BLL.MallSeller.DsOrderBo.Instance.GetImgFlag(6, "", "3", new List<UpGradeOrderItem>
            {new UpGradeOrderItem() 
            });
           Assert.IsTrue(string.Equals(img, "guomeizaixian"));
        }
    }
}
