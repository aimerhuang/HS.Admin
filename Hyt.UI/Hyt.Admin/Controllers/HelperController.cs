using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hyt.Admin.Controllers
{
    /// <summary>
    /// 系统帮助
    /// </summary>
    /// <remarks>2013-11-12 唐勇勤 创建</remarks>
    [CustomActionFilter(false)]
    public class HelperController : BaseController
    {
        /// <summary>
        /// 帮助中心首页
        /// </summary>
        /// <param></param>
        /// <returns>帮助中心界面</returns>
        /// <remarks>2013-11-12 唐勇勤 创建</remarks>
        public ActionResult Index()
        {
            string view = Request.Params["v"];
            if (!string.IsNullOrEmpty(view))
            {
                return View(view);
            }
            else
            {
                return View();
            }
        }

    }
}
