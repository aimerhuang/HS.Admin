using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.BLL.Sys;
using Hyt.Model.Parameter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hyt.BLL.Base;
using Pisen.Service.Share.SSO.Contract.DataContract;
using Newtonsoft.Json.Linq;

namespace Hyt.BLL.Sys.Tests
{
    [TestClass()]
    public class SySsoUserAssociationBoTests
    {
        #region 附加测试特性
        [ClassInitialize]
        public static void MyClassInitialize(TestContext testContext)
        {
            DataProviderBo.Set(Activator.CreateInstance(Type.GetType("Hyt.DataAccess.Oracle.DataProvider,Hyt.DataAccess.Oracle")));
            Infrastructure.Initialize.Init();
        }
        #endregion
        [TestMethod()]
        public void GetSsoUserInfoTest()
        {
            string dd="\"a:3:{s:7:'pkgCode';s:18:\"\u5e7f\u5dde\u5206\u62e8\u4e2d\u5fc3\";s:19:\"billProvideSiteName\";s:18:\"\u4e0a\u6d77\u8f6c\u8fd0\u4e2d\u5fc3\";s:19:\"billProvideSiteCode\";s:0:\"\";}";
            //var expressnoInfo = JObject.Parse(dd);
            var UTF8 = new System.Text.UTF8Encoding();
            Byte[] BytesStr = UTF8.GetBytes(dd);

            Hyt.Util.Serialization.PHPSerializer.UnSerialize(BytesStr);

           //var result = SySsoUserAssociationBo.Instance.GetSsoUserInfoBySsoUserId(1);
            //Assert.IsNotNull(result);
        }

 
        [TestMethod()]
        public void GetEnterprisePageListTest()
        {
            ParaEnterpriseUserFilter para=new ParaEnterpriseUserFilter();
            para.Key = null;
            para.PageSize = 10;
            para.PageIndex = 1;
           // var result = SySsoUserAssociationBo.Instance.GetEnterprisePageList(para);
            
            //Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void GetAllEnterpriseUserTest()
        {
            ParaEnterpriseUserFilter para = new ParaEnterpriseUserFilter();
            para.Key = null;
            para.PageSize = 10;
            para.PageIndex = 1;
            var result = SySsoUserAssociationBo.Instance.GetAllEnterpriseUserPager(para);

            Assert.IsNotNull(result);
        }
    }
}
