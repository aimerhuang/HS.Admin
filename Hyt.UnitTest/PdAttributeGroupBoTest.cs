using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hyt.BLL.Product;
using Hyt.Model;

namespace Hyt.UnitTest
{
    [TestClass]
    public class PdAttributeGroupBoTest
    {
        //使用 ClassInitialize 在运行类中的第一个测试前先运行代码
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            Hyt.Infrastructure.Initialize.Init();
        }

        /// <summary>
        /// 测试读取分类对应的属性组
        /// </summary>
        [TestMethod]
        public void GetPdCategoryAttributeGroupListTest()
        {
            //分类“Power Twins[能量双星]” sysno=150
            //6个属性组
            IList<PdAttributeGroup> list = PdAttributeGroupBo.Instance.GetPdCategoryAttributeGroupList(150);
            Assert.IsInstanceOfType(list, typeof(IList<PdAttributeGroup>));

            foreach (PdAttributeGroup p in list)
            {
                Assert.IsTrue(p.SysNo==1);
                Assert.IsTrue(p.Name=="基本信息");
            }

            Assert.IsTrue(list.Count == 6);
            Assert.IsTrue(true);
        }
    }
}
