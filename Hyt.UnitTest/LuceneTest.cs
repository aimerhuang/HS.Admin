using System;
using Hyt.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hyt.UnitTest
{
    [TestClass]
    public class LuceneTest
    {
         #region 初始化
        public LuceneTest()
        {
            Hyt.Infrastructure.Initialize.Init();
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
       #endregion

    
        /// <summary>
        /// 创建索引文件
        /// </summary>
        [TestMethod]
        public void CreateIndexTest()
        {
            Hyt.Infrastructure.Lucene.ProductIndex.Instance.CreateIndex(true);
            Hyt.Infrastructure.Lucene.ProductIndex.Instance.CloseWithoutOptimize();
        }
    
       /// <summary>
       /// 增加索引
       /// </summary>
        [TestMethod]
        public void IndexStringTest()
        {
            int i = 0;
            var lst = Hyt.BLL.Product.PdProductBo.Instance.GetAllProduct();
            foreach (PdProductIndex item in lst)
            {
                i++;
                if (i > 100) continue;
                Hyt.Infrastructure.Lucene.ProductIndex.Instance.AddIndex(item);
            }
        }
        PdProductIndex FirstOrDefault = null;
        /// <summary>
        /// 查询所有记录
        /// </summary>
        [TestMethod]
        public void Search()
        {
            int pageindex = 1;
            int pagecount=1;
            int recordcount=0;
            var lst = Hyt.Infrastructure.Lucene.ProductIndex.Instance.QueryDoc("ProductName", "电池", pageindex, 20, out recordcount);
            if (lst != null && lst.Count > 0)
            {
                FirstOrDefault = lst[0];
                TestContext.WriteLine("第一条编号:" + FirstOrDefault.SysNo);
            }
            TestContext.WriteLine("当前页" + pageindex.ToString() + " 页数" + pagecount.ToString() + " 总数" + recordcount.ToString());
        }
       /// <summary>
        /// 删除
        /// </summary>
        [TestMethod]
        public void Delete()
        {
            var model = new PdProductIndex()
            {
                SysNo=1

            };
            Hyt.Infrastructure.Lucene.ProductIndex.Instance.DeleteIndex(model);
                TestContext.WriteLine("删除成功"); 
        }

    }
}
