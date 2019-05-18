using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.BLL.Product;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.WorkflowStatus;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hyt.UnitTest
{
    [TestClass()]
    public class FeProductCommentTest
    {
        public FeProductCommentTest()
        {
            Hyt.Infrastructure.Initialize.Init();
        }

        /// <summary>
        ///获取或设置测试上下文，上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext { get; set; }

        //[TestMethod()]
        public void AddComment()
        {
            var pager = new Pager<ParaProductSearchFilter>
                {
                    PageFilter = new ParaProductSearchFilter()
                        {
                            ProductName = "电霸"
                        }
                };
            PdProductBo.Instance.ProductSelectorProductSearch(ref pager);
            if (pager.Rows == null || !pager.Rows.Any()) return;
            foreach (var item in pager.Rows)
            {
                AddComment(item.SysNo);
                AddShareOrders(item.SysNo);
            }

            pager = new Pager<ParaProductSearchFilter>
            {
                PageFilter = new ParaProductSearchFilter()
                {
                    ProductName = "备电"
                },
                Rows = null
            };
            PdProductBo.Instance.ProductSelectorProductSearch(ref pager);
            if (pager.Rows == null || !pager.Rows.Any()) return;
            foreach (var item in pager.Rows)
            {
                AddComment(item.SysNo);
                AddShareOrders(item.SysNo);
            }
        }
        private void AddShareOrders(int sysNo)
        {
            var postShareOrder = new FeProductComment()
            {
                ProductSysNo = sysNo,
                CustomerSysNo = 1061,
                ShareTitle = "5星商品",
                ShareContent = "5星商品，经久耐用,非常好!",
                IsShare = (int)ForeStatus.是否晒单.是,
                IsComment = (int)ForeStatus.是否评论.否,
                ShareTime = DateTime.Now,
                ShareStatus = (int)ForeStatus.商品晒单状态.待审
            };
            BLL.Front.FeProductCommentBo.Instance.Insert(postShareOrder);
        }
        private void AddComment(int sysNo)
        {
            // 添加商品评价
            var productCommentModel = new FeProductComment
            {
                Advantage = "经久耐用,非常好!",
                CommentStatus = (int)ForeStatus.商品评论状态.已审,
                CommentTime = DateTime.Now,
                Content = "5星商品，经久耐用,非常好!",
                CustomerSysNo = 1061,
                Disadvantage = "暂时没有发现缺点哦！",
                ProductSysNo = sysNo,
                Score = 4,
                Title = "5星商品",
                IsComment = (int)ForeStatus.是否评论.是,
                IsShare = (int)ForeStatus.是否晒单.否
            };
            BLL.Front.FeProductCommentBo.Instance.Insert(productCommentModel);
        }
    }
}
