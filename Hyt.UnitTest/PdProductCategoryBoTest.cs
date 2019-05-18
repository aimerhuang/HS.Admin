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
    public class PdProductCategoryBoTest
    {
        //使用 ClassInitialize 在运行类中的第一个测试前先运行代码
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            Hyt.Infrastructure.Initialize.Init();
        }

        [TestMethod]
        public void GetCategory()
        {
            //获取系统编号为1的商品分类
            PdCategory category = PdCategoryBo.Instance.GetCategory(1);
            Assert.IsNotNull(category);
            Assert.IsTrue(category.CategoryName == "苹果配件");
            Assert.IsTrue(category.SysNo == 1);
            Assert.IsTrue(category.ParentSysNo == 0);
            Assert.IsNull(category.Code);
            Assert.IsTrue(category.SeoTitle == "苹果配件_车载充电器,苹果手机壳,苹果移动电源尽在品胜·商城网上商城");
            Assert.IsTrue(category.SeoKeyword == "苹果移动电源,车载充电器,苹果手机壳");
            Assert.IsTrue(category.SeoDescription == "苹果配件_车载充电器,苹果手机壳,苹果移动电源尽在品胜PC商城");
            Assert.IsTrue(category.Status == (int)Hyt.Model.WorkflowStatus.ProductStatus.商品分类状态.有效);
        }

        [TestMethod]
        public void GetCategoryNavigationString()
        {
            int categoryId = 32;
            string[] list = Hyt.BLL.Web.PdCategoryBo.Instance.BuilderCategoryNavigationString(categoryId,
                                                                              "<a href=\"/Product/List/{0}\">{1}</a>",
                                                                              "&nbsp;>&nbsp;");
            Assert.IsTrue(list.Count() == 2);
        }
    }
}
