using Hyt.BLL.Product;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;

namespace Hyt.UnitTest
{
    [TestClass]
    public class PdProductCollocationBoTest
    {
        //使用 ClassInitialize 在运行类中的第一个测试前先运行代码
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            Hyt.Infrastructure.Initialize.Init();
        }

        [TestMethod]
        public void Delete()
        {
            bool success = PdProductCollocationBo.Instance.Delete(1);
            Assert.IsTrue(success);

            IList<CBProductListItem> list = PdProductCollocationBo.Instance.GetList(1);
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count == 0);
        }

        [TestMethod]
        public void Create()
        {
            PdProductCollocation p = new PdProductCollocation()
                {
                    Code = 1,
                    CreatedBy = 1,
                    CreatedDate = DateTime.Now,
                    ProductSysNo = 2
                };
            bool success = PdProductCollocationBo.Instance.Create(p);
            Assert.IsTrue(success);

            IList<CBProductListItem> list = PdProductCollocationBo.Instance.GetList(1);
            Assert.IsTrue(list.Count == 1);

            Assert.AreEqual(list[0].SysNo, 2);

        }

        [TestMethod]
        public void GetList()
        {
            IList<CBProductListItem> list = PdProductCollocationBo.Instance.GetList(1);

            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count == 1);
        }

        [TestMethod]
        public void IsExistSingle()
        {
            Assert.IsTrue(PdProductCollocationBo.Instance.IsExist(1, 2));
        }

        [TestMethod]
        public void IsExistList()
        {
            Assert.IsTrue(PdProductCollocationBo.Instance.IsExist(1,new int[]
            {
                2,3
            }));
        }

        [TestMethod]
        public void ProductSelectorSearch()
        {
            Pager<ParaProductSearchFilter> pager = new Pager<ParaProductSearchFilter>();
            pager.CurrentPage = 1;
            pager.PageFilter = new ParaProductSearchFilter
            {
                ProductName = "ip",
                ErpCode = "ip",
                ProductCategorySysNo = 0
            };

            PagedList<ParaProductSearchFilter> pageList;

            PdProductBo.Instance.ProductSelectorProductSearch(ref pager,out pageList);

            Assert.IsTrue(pageList.TData.Count > 0);
        }

        [TestMethod]
         public void GetSelectedProductInfoTest()
        {
            PdProductBo.Instance.GetSelectedProductInfo(new int[] {1, 2, 3});
            Assert.IsTrue(1==1);
        }

        [TestMethod]
        public void GetSelectedProductListTest()
        {
            IList<int> list = new List<int>();
            list.Add(1);
            list.Add(2);
            list.Add(3);
            Assert.IsTrue(PdProductBo.Instance.GetSelectedProductList(list).Count > 0);
            
        }

        [TestMethod]
        public void dtest()
        {
            Assert.AreEqual("002002", PdCategoryBo.Instance.GetFreeCodeNum(1));
        }

    }
}
