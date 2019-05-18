using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Hyt.BLL.CRM;
using Hyt.Infrastructure.Pager;
using Hyt.Model.WorkflowStatus;

namespace Hyt.Admin.Controllers
{
    /// <summary>
    /// 分页实例
    /// </summary>
    /// <remarks>
    ///  2013-6-15 杨浩 创建
    /// </remarks>
    public class PageTestController : Controller
    {

        //ajax 分页 实例
        public ActionResult Index(int? id)
        {
            var model = GetTestData(id ?? 1);
            if (Request.IsAjaxRequest())
            {
                //演示预加载
                System.Threading.Thread.Sleep(1000);
                return PartialView("_AjaxPager", model);
            }

            return View(model);
        }

        public ActionResult Cart1()
        {
            Session.Add("CustomerKey", "58965");
            var key = Session["CustomerKey"].ToString();
            CrShoppingCartToCacheBo.Instance.Add(key, 196, 1, 1, CustomerStatus.购物车商品来源.PC网站);

            return View();
        }

        public ActionResult Cart2()
        {
            var key = Session["CustomerKey"].ToString();
            CrShoppingCartToCacheBo.Instance.GetShoppingCart(key,new []{PromotionStatus.促销使用平台.PC商城, }, 196);

            return View();
        }

        //Excel 导出实例
        public ActionResult Index2(string name)
        {
            IList<PagedList> list = new List<PagedList>();
            for (int i = 0; i < 100; i++)
            {
                PagedList mode = new PagedList();
                mode.Data = i + "会乱码吗？但愿不会！";
                list.Add(mode);
            }

            Hyt.Util.ExcelUtil.Export<PagedList>(list);

            return View();
        }

        //标准分页实例
        public ActionResult UrlPage(int? id)
        {
            var model = GetTestData(id ?? 1);

            return View(model);
        }

        private PagedList GetTestData(int id)
        {
            PagedList model = new PagedList();
            int j = id * 10;
            List<Models.PageTest> tmp = new List<Models.PageTest>();
            for (int i = 1; i <= 20; i++)
            {
                Models.PageTest hello = new Models.PageTest();
                hello.Name = "test" + (i + j);
                hello.ID = (i + j);
                tmp.Add(hello);
            }
            model.Data = tmp;
            model.PageSize = 20;
            model.TotalItemCount = 500;
            model.CurrentPageIndex = id;
            return model;
        }

        public ActionResult StockSelector()
        {
            return View();
        }
    }
}
