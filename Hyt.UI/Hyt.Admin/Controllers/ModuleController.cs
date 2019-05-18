using System.Web.Mvc;

namespace Hyt.Admin.Controllers
{
    /// <summary>
    /// 组件
    /// </summary>
    /// <remarks>2014-1-24 黄志勇 注释</remarks>
    public class ModuleController : BaseController
    {
        /// <summary>
        /// 选择会员
        /// </summary>
        /// <returns>视图</returns>
        /// <remarks>2014-1-24 黄志勇 注释</remarks>
        public ActionResult SelectCustomer()
        {
            return View();
        }

        /// <summary>
        /// 选择仓库
        /// </summary>
        /// <returns>视图</returns>
        /// <remarks>2014-1-24 黄志勇 注释</remarks>
        public ActionResult SelectWarehouse()
        {
            return View();
        }

        /// <summary>
        /// 选择商品
        /// </summary>
        /// <returns>视图</returns>
        /// <remarks>2014-1-24 黄志勇 注释</remarks>
        public ActionResult SelectProduct()
        {
            return View();
        }
    }
}
