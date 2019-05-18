using Hyt.BLL.Product;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Hyt.BLL.Web;
using Hyt.BLL.Base;
using PdProductBo = Hyt.BLL.Product.PdProductBo;
using Hyt.Model;
using System.Linq;
namespace Hyt.UnitTest
{
    
    
    /// <summary>
    ///这是 PdProductBoTest 的测试类，旨在
    ///包含所有 PdProductBoTest 单元测试
    ///</summary>
    [TestClass()]
    public class PdProductBoTest : Hyt.DataAccess.Base.DaoBase<PdProductBoTest>
    {
        //使用 ClassInitialize 在运行类中的第一个测试前先运行代码
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            DataProviderBo.Set(Activator.CreateInstance(Type.GetType("Hyt.DataAccess.Oracle.DataProvider,Hyt.DataAccess.Oracle")));
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

        #region 附加测试特性
        // 
        //编写测试时，还可使用以下特性:
        //
        //使用 ClassInitialize 在运行类中的第一个测试前先运行代码
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
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
        ///CreateProductThumbnail 的测试
        ///</summary>
        [TestMethod()]
        public void CreateProductThumbnailTest()
        {


            var pager = new Pager<PdProduct>() { PageSize = 999999, CurrentPage = 1 };
            pager = BLL.Product.PdProductBo.Instance.GetPdProductList(pager);

            if (!pager.Rows.Any(x => x.ErpCode == "02.03.01.01"))
            {
               

               
            }
            //PdProductBo target = new PdProductBo(); // TODO: 初始化为适当的值
            //string imgUri = "http://192.168.10.127:21/Product/{0}/201307/c5183e9ddc8a4dcda59ba8f9430de8ad.jpg"; // TODO: 初始化为适当的值
            //bool expected = true; // TODO: 初始化为适当的值
            //bool actual;
            //actual = target.CreateProductThumbnail(imgUri,"");
            //Assert.AreEqual(expected, actual);

        }

        /// <summary>
        /// 获取商品属性测试
        /// </summary>
        [TestMethod]
        public void GetProductAttrib()
        {
            IList<Hyt.Model.CBPdProductAtttributeReadRelation> list = Hyt.BLL.Web.PdProductBo.Instance.GetProductAttributeInfo(1942);

             Assert.IsTrue(list.Count == 2);
             Assert.IsTrue(list[0].ProductAtttributeList.Count == 4);
             Assert.IsTrue(list[1].ProductAtttributeList.Count == 1);
        }

        /// <summary>
        /// 评论前五位用户
        /// </summary>
        [TestMethod]
        public void GetReviewTop5()
        {
            IDictionary<int, string> reult = Hyt.BLL.Web.FeProductCommentBo.Instance.GetFirstReviewTop5(1);
            Assert.IsTrue(reult != null);
            Assert.IsTrue(reult.Count > 0);
        }

        /// <summary>
        /// 商品评论次数详细信息
        /// </summary>
        [TestMethod]
        public void GetReviewTimesDetailInfo()
        {
            IDictionary<string, int> reult = Hyt.BLL.Web.FeProductCommentBo.Instance.GetProductCommentTimesDetialInfo(1);
            Assert.IsTrue(reult != null);
            Assert.IsTrue(reult.Count == 5);
        }

        /// <summary>
        /// 更新商品评论支持
        /// </summary>
        [TestMethod]
        public void UpdateFeCommentSupport()
        {
            Assert.IsTrue(Hyt.BLL.Web.FeCommentSupportBo.Instance.Update(true, 1000,1).Status);
            Assert.IsTrue(Hyt.BLL.Web.FeCommentSupportBo.Instance.Update(false, 1000,1).Status);
        }

        /// <summary>
        /// 测试评价回复
        /// </summary>
        [TestMethod]
        public void UpdateFeCommentReplay()
        {
            Assert.IsTrue(Hyt.BLL.Web.FeProductCommentBo.Instance.Replay(1000, "test", 1));
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void PdProductAssociationBoGetProductList()
        {
            Assert.IsTrue(Hyt.BLL.Product.PdProductAssociationBo.Instance.GetProductList(1852).Count > 0);
        }

        /// <summary>
        ///UpdateProductSales 的测试
        ///</summary>
        [TestMethod()]
        public void UpdateProductSalesTest()
        {
            Hyt.BLL.Web.PdProductBo target = new Hyt.BLL.Web.PdProductBo(); // TODO: 初始化为适当的值
            int productSysNo = 10; // TODO: 初始化为适当的值
            int accelerate = 10; // TODO: 初始化为适当的值
            target.UpdateProductSales(productSysNo, accelerate);
        }

        /// <summary>
        ///UpdateProductLiking 的测试
        ///</summary>
        [TestMethod()]
        public void UpdateProductLikingTest()
        {
            Hyt.BLL.Web.PdProductBo target = new Hyt.BLL.Web.PdProductBo(); // TODO: 初始化为适当的值
            int productSysNo = 10; // TODO: 初始化为适当的值
            int accelerate = 10; // TODO: 初始化为适当的值
            target.UpdateProductLiking(productSysNo, accelerate);
        }

        /// <summary>
        ///UpdateProductComments 的测试
        ///</summary>
        [TestMethod()]
        public void UpdateProductCommentsTest()
        {
            Hyt.BLL.Web.PdProductBo target = new Hyt.BLL.Web.PdProductBo(); // TODO: 初始化为适当的值
            int productSysNo = 10; // TODO: 初始化为适当的值
            int score = 4; // TODO: 初始化为适当的值
            int accelerate = 1; // TODO: 初始化为适当的值
            target.UpdateProductComments(productSysNo, score, accelerate);
        }

        /// <summary>
        ///UpdateProductShares 的测试
        ///</summary>
        [TestMethod()]
        public void UpdateProductSharesTest()
        {
            Hyt.BLL.Web.PdProductBo target = new Hyt.BLL.Web.PdProductBo(); // TODO: 初始化为适当的值
            int productSysNo = 10; // TODO: 初始化为适当的值
            int accelerate = 10; // TODO: 初始化为适当的值
            target.UpdateProductShares(productSysNo, accelerate);

        }

        /// <summary>
        ///UpdateProductQuestion 的测试
        ///</summary>
        [TestMethod()]
        public void UpdateProductQuestionTest()
        {
            Hyt.BLL.Web.PdProductBo target = new Hyt.BLL.Web.PdProductBo(); // TODO: 初始化为适当的值
            int productSysNo = 10; // TODO: 初始化为适当的值
            int accelerate = 10; // TODO: 初始化为适当的值
            target.UpdateProductQuestion(productSysNo, accelerate);
        }

        /// <summary>
        ///GetProductEasName 的测试
        ///</summary>
        [TestMethod()]
        public void GetProductEasNameTest()
        {
            var target = new PdProductBo(); // TODO: 初始化为适当的值
            const int productSysNo =1; // TODO: 初始化为适当的值
           
            var actual = target.GetProductEasName(productSysNo);
            Console.WriteLine(actual);
            Assert.IsTrue(!string.IsNullOrWhiteSpace(actual));

        }
    }
}
