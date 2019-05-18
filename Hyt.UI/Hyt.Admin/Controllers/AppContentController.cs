using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Hyt.BLL.AppContent;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.SystemPredefined;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.Admin.Controllers
{
    /// <summary>
    /// app内容管理
    /// </summary>
    /// <remarks>2013-9-4 黄伟 创建</remarks>
    public class AppContentController : BaseController 
    {
        //
        // GET: /AppContent/

        #region App版本管理

        /// <summary>
        /// App版本管理
        /// </summary>
        /// <param name="id">page no</param>
        /// <param name="para">CBApVersion</param>
        /// <returns>view</returns>
        /// <remarks>2013-9-4 黄伟 创建</remarks>
        [Privilege(PrivilegeCode.AP1001101)]
        public ActionResult VersionManagement(int? id, CBApVersion para)
        {
            var dic = AppContentBo.Instance.QueryAppVersion(para, id ?? 1);

            var model = new PagedList<CBApVersion>
            {
                TData = dic.Any() ? dic.First().Value : null,
                CurrentPageIndex = id ?? 1,
                TotalItemCount = dic.Any() ? dic.First().Key : 0
            };

            if (Request.IsAjaxRequest())
            {
                return PartialView("pAjaxPager_AppVersion", model);
            }

            return View(model);

        }

        /// <summary>
        /// App版本管理
        /// </summary>
        /// <param name="sysNo">版本系统编号</param>
        /// <returns>view</returns>
        /// <remarks>2013-9-10 黄伟 创建</remarks>
        [Privilege(PrivilegeCode.AP1001201)]
        public ActionResult CreateOrUpdateVersion(int? sysNo)
        {
            var model = new CBApVersion();
            if (sysNo != null && sysNo > 0)
            {
                model = AppContentBo.Instance.GetAppVersion(sysNo ?? 0);
                return View(model);
            }
            return View(model);

        }

        /// <summary>
        /// 新增版本
        /// </summary>
        /// <param name="model">CBApVersion</param>
        /// <returns>json(result封装实体)</returns>
        /// <remarks>2013-9-10 黄伟 创建</remarks>
        [Privilege(PrivilegeCode.AP1001201)]
        [HttpPost]
        public ActionResult CreateVersion(CBApVersion model)
        {
            var result = AppContentBo.Instance.CreateVersion(model, HttpContext.Request.ServerVariables["Remote_Add"], CurrentUser.Base.SysNo);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 修改版本
        /// </summary>
        /// <param name="model">CBApVersion</param>
        /// <returns>json(result封装实体)</returns>
        /// <remarks>2013-9-10 黄伟 创建</remarks>
        [Privilege(PrivilegeCode.AP1001201)]
        [HttpPost]
        public ActionResult UpdateVersion(CBApVersion model)
        {
            var result = AppContentBo.Instance.UpdateVersion(model, HttpContext.Request.ServerVariables["Remote_Add"], CurrentUser.Base.SysNo);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除版本
        /// </summary>
        /// <param name="arrDelSysnos">sysnos of the version to delete</param>
        /// <returns>json(result封装实体)</returns>
        /// <remarks>2013-9-10 黄伟 创建</remarks>
        [Privilege(PrivilegeCode.AP1001401)]
        [HttpPost]
        public ActionResult DeleteVersion(string arrDelSysnos)
        {
            var lstDelSysnos = arrDelSysnos.Split(',').Select(int.Parse).ToList();

            var result = AppContentBo.Instance.DeleteVersion(lstDelSysnos,
                                                             HttpContext.Request.ServerVariables["Remote_Add"],
                                                             CurrentUser.Base.SysNo);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 商品浏览历史记录

        /// <summary>
        /// 商品浏览历史记录
        /// </summary>
        /// <param name="id">page no</param>
        /// <param name="para">CBCrBrowseHistory</param>
        /// <returns>view</returns>
        /// <remarks>2013-9-4 黄伟 创建</remarks>
        [Privilege(PrivilegeCode.CR1003101)]
        public ActionResult ProductBrowseHistory(int? id, CBCrBrowseHistory para)
        {

            var dicProductBrowseHistory = AppContentBo.Instance.QueryProBroHistory(para, id ?? 1);

            var model = new PagedList<CBCrBrowseHistory>
                {
                    TData = dicProductBrowseHistory.Any() ? dicProductBrowseHistory.First().Value : null,
                    CurrentPageIndex = id ?? 1,
                    TotalItemCount = dicProductBrowseHistory.Any() ? dicProductBrowseHistory.First().Key : 0
                };

            if (Request.IsAjaxRequest())
            {
                return PartialView("pAjaxPager_ProBrowseHis", model);
            }

            return View(model);
        }

        /// <summary>
        /// 删除浏览历史记录
        /// </summary>
        /// <param name="arrDelSysNos">要删除的历史记录编号集合</param>
        /// <returns>json(result封装实体)</returns>
        /// <remarks>2013-9-4 黄伟 创建</remarks>
        [Privilege(PrivilegeCode.CR1003401)]
        [HttpPost]
        public ActionResult DeleteBrowseHistory(string arrDelSysNos)
        {
            //var lstDelSysnos =new JavaScriptSerializer().Deserialize<>()
            var lstDelSysnos = arrDelSysNos.Split(',').Select(int.Parse).ToList();
            try
            {
                AppContentBo.Instance.DeleteBrowseHistory(lstDelSysnos);
            }
            catch (Exception ex)
            {
                return Json(new Result { Status = false, Message = string.Format("操作失败:{0}", ex.Message) }, JsonRequestBehavior.AllowGet);
            }

            return Json(new Result { Status = true, Message = string.Format("操作成功!") }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region APP推送服务

        /// <summary>
        /// 推送消息服务管理
        /// </summary>
        /// <returns>返回视图</returns>
        /// <remarks>2014-01-14 邵斌 创建</remarks>
        [Privilege(PrivilegeCode.AP1002101)]
        [HttpGet]
        public ActionResult AppPushServiceManage()
        {
            return View();
        }

        /// <summary>
        /// 推送消息服务管理
        /// </summary>
        /// <param name="filter">分页过滤参数</param>
        /// <returns>返回视图</returns>
        /// <remarks>2014-01-14 邵斌 创建</remarks>
        [Privilege(PrivilegeCode.AP1002101)]
        [HttpPost]
        public ActionResult AppPushServiceManage(ParaApPushServiceSearchFilter filter)
        {
            filter.id = (filter.id == 0 ? 1 : filter.id);
            var page = new PagedList<CBApPushService>();
            CBApPushService para = filter == null ? new CBApPushService() : (CBApPushService)filter;
            page.CurrentPageIndex = filter.id;

            //格式化数据
            filter.Title = (filter.Title ?? "").Trim();
            filter.Content = (filter.Content ?? "").Trim();
            filter.CustomerAccount = (filter.CustomerAccount ?? "").Trim();

            AppContentBo.Instance.GetApPushService(ref page, para);
            return View("_AppPushServiceManage_Pager", page);
        }

        /// <summary>
        /// 编辑推送信息
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <param name="isView">是否是查看</param>
        /// <returns>返回视图</returns>
        /// <remarks>2014-01-14 邵斌 创建</remarks>
        [Privilege(PrivilegeCode.AP1002102)]
        public ActionResult EditAppPushServiceManage(int sysNo, bool isView = false)
        {
            var model = AppContentBo.Instance.GetApPushService(sysNo);
            return View("_AppPushServericeDetial", model);
        }

        /// <summary>
        /// 编辑推送信息
        /// </summary>
        /// <returns>返回视图</returns>
        /// <remarks>2014-01-14 邵斌 创建</remarks>
        [Privilege(PrivilegeCode.AP1002102)]
        public ActionResult AddAppPushServiceManage()
        {
            return View("_AppPushServericeDetial");
        }

        #endregion

    }
}
