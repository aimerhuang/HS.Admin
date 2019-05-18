using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Hyt.BLL.Log;
using Hyt.BLL.Union;
using Hyt.BLL.Weixin;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.SystemPredefined;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Util;
using Hyt.Util.Serialization;

namespace Hyt.Admin.Controllers
{
    /// <summary>
    /// 联盟
    /// </summary>
    /// <remarks>2013-10-16 周唐炬 创建</remarks>
    public class UnionController : BaseController
    {
        #region 联盟网站管理 2013-10-16 周唐炬

        /// <summary>
        /// 联盟网站管理
        /// </summary>
        /// <param name="id">当前页</param>
        /// <param name="filter">查询过滤器</param>
        /// <returns>联盟网站管理视图</returns>
        /// <remarks>2013-10-16 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.UN1001101)]
        public ActionResult UnionSiteManage(int? id, ParaUnUnionSiteFilter filter)
        {
            try
            {
                if (Request.IsAjaxRequest())
                {
                    filter.CurrentPage = id ?? 1;
                    var list = UnUnionSiteBo.Instance.GetUnionSiteList(filter);
                    return PartialView("_UnionSiteList", list);
                }
                InitUnionSitePageViewData(true, null);
            }
            catch (Exception ex)
            {
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "联盟网站管理" + ex.Message, LogStatus.系统日志目标类型.联盟网站, CurrentUser.Base.SysNo, ex);
            }
            return View();
        }

        /// <summary>
        /// 添加联盟网站
        /// </summary>
        /// <param name=" "></param>
        /// <returns>视图页</returns>
        /// <remarks>2013-10-16 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.UN1001201)]
        public ActionResult UnionSiteCreate()
        {
            InitUnionSitePageViewData(false, null);
            return View();
        }

        /// <summary>
        /// 添加联盟网站
        /// </summary>
        /// <param name="model">联盟网站实体</param>
        /// <returns>返回结果</returns>
        /// <remarks>2013-10-16 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.UN1001201)]
        [HttpPost]
        public JsonResult UnionSiteCreate(UnUnionSite model)
        {
            var result = new Result() { StatusCode = -1 };
            try
            {
                if (ModelState.IsValid)
                {
                    var id = UnUnionSiteBo.Instance.Create(model);
                    if (id > 0)
                    {
                        result.Status = true;
                        result.StatusCode = 0;
                        SysLog.Instance.Info(LogStatus.系统日志来源.后台, "添加联盟网站" + model.SiteName, LogStatus.系统日志目标类型.联盟网站, id, CurrentUser.Base.SysNo);
                    }
                }
                else
                {
                    result.Message = "联盟网站数据有误，请检查重试！";
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "添加联盟网站" + ex.Message, LogStatus.系统日志目标类型.联盟网站, CurrentUser.Base.SysNo, ex);
            }

            return Json(result);
        }

        /// <summary>
        /// 修改联盟网站
        /// </summary>
        /// <param name="id">联盟网站系统编号</param>
        /// <returns>视图页</returns>
        /// <remarks>2013-10-16 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.UN1001301)]
        public ActionResult UnionSiteEdit(int id)
        {
            var model = UnUnionSiteBo.Instance.GetModel(id);
            InitUnionSitePageViewData(false, model);
            return View(model);
        }

        /// <summary>
        /// 修改联盟网站
        /// </summary>
        /// <param name="model">联盟网站实体</param>
        /// <returns>返回结果</returns>
        /// <remarks>2013-10-16 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.UN1001301)]
        [HttpPost]
        public JsonResult UnionSiteEdit(UnUnionSite model)
        {
            var result = new Result() { StatusCode = -1 };
            try
            {
                if (ModelState.IsValid)
                {
                    var id = UnUnionSiteBo.Instance.Update(model);
                    if (id > 0)
                    {
                        result.Status = true;
                        result.StatusCode = 0;
                        SysLog.Instance.Info(LogStatus.系统日志来源.后台, "修改联盟网站" + model.SiteName, LogStatus.系统日志目标类型.联盟网站, model.SysNo, CurrentUser.Base.SysNo);
                    }
                }
                else
                {
                    result.Message = "联盟网站数据有误，请检查重试！";
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "修改联盟网站" + ex.Message, LogStatus.系统日志目标类型.联盟网站, CurrentUser.Base.SysNo, ex);
            }

            return Json(result);
        }

        /// <summary>
        /// 查看联盟网站
        /// </summary>
        /// <param name="id">联盟网站系统编号</param>
        /// <returns>视图页</returns>
        /// <remarks>2013-10-16 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.UN1001101)]
        public ActionResult UnionSiteDetail(int id)
        {
            var model = UnUnionSiteBo.Instance.GetModel(id);
            return View(model);
        }

        /// <summary>
        /// 联盟网站状态变更
        /// </summary>
        /// <param name="id">联盟网站系统编号</param>
        /// <param name="status">联盟网站状态</param>
        /// <returns>返回结果</returns>
        /// <remarks>2013-10-16 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.UN1001701)]
        public JsonResult UnionSiteStatusChange(int id, UnionStatus.联盟网站状态 status)
        {
            var result = new Result() { StatusCode = -1 };
            try
            {
                var model = UnUnionSiteBo.Instance.GetModel(id);
                if (model != null)
                {
                    model.Status = (int)status;
                    var rowsAffected = UnUnionSiteBo.Instance.Update(model);
                    if (rowsAffected > 0)
                    {
                        result.Status = true;
                        result.StatusCode = 0;
                        SysLog.Instance.Info(LogStatus.系统日志来源.后台, "联盟网站状态变更" + model.SiteName, LogStatus.系统日志目标类型.联盟网站, id, CurrentUser.Base.SysNo);
                    }
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "联盟网站状态变更" + ex.Message, LogStatus.系统日志目标类型.联盟网站, CurrentUser.Base.SysNo, ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 联盟网站名称验证
        /// </summary>
        /// <param name="siteName">联盟网站名称</param>
        /// <param name="id">联盟网站系统编号</param>
        /// <returns>验证结果</returns>
        /// <remarks>2013-09-06 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.UN1001201, PrivilegeCode.UN1001301)]
        public JsonResult UnionSiteVerify(string siteName, int? id)
        {
            var result = false;
            if (!string.IsNullOrWhiteSpace(siteName))
            {
                result = UnUnionSiteBo.Instance.UnionSiteVerify(siteName, id);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 初始化联盟网站页面数据
        /// </summary>
        /// <param name="viewStyle">视图中下拉显示模式(true列表查看模式,false添加修改模式)</param>
        /// <param name="model">联盟网站实体</param>
        /// <returns></returns>
        /// <remarks>2013-09-06 周唐炬 创建</remarks>
        private void InitUnionSitePageViewData(bool viewStyle, UnUnionSite model)
        {
            var text = viewStyle ? "全部" : "请选择";
            var item = new SelectListItem() { Text = text, Value = "", Selected = true };
            var statusts = new List<SelectListItem>() { item };
            EnumUtil.ToListItem<UnionStatus.联盟网站状态>(ref statusts);
            ViewBag.Status = model != null ? new SelectList(statusts, "Value", "Text", model.Status) : new SelectList(statusts, "Value", "Text");
        }
        #endregion

        #region 联盟广告管理 2013-10-16 周唐炬

        /// <summary>
        /// 联盟广告管理
        /// </summary>
        /// <param name="id">当前页</param>
        /// <param name="filter">查询过滤器</param>
        /// <returns>联盟广告管理视图</returns>
        /// <remarks>2013-10-16 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.UN1002101)]
        public ActionResult AdvertisementManage(int? id, ParaUnAdvertisementFilter filter)
        {
            try
            {
                if (Request.IsAjaxRequest())
                {
                    filter.CurrentPage = id ?? 1;
                    var list = UnAdvertisementBo.Instance.GetAdvertisementList(filter);
                    return PartialView("_AdvertisementList", list);
                }
                InitAdvertisementPageViewData(true, null);
            }
            catch (Exception ex)
            {
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "联盟广告管理" + ex.Message, LogStatus.系统日志目标类型.联盟网站, CurrentUser.Base.SysNo, ex);
            }
            return View();
        }

        /// <summary>
        /// 添加联盟网站
        /// </summary>
        /// <param name=" "></param>
        /// <returns>视图页</returns>
        /// <remarks>2013-10-16 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.UN1002201)]
        public ActionResult AdvertisementCreate()
        {
            InitAdvertisementPageViewData(false, null);
            return View();
        }

        /// <summary>
        /// 添加联盟广告
        /// </summary>
        /// <param name="model">联盟广告实体</param>
        /// <returns>返回结果</returns>
        /// <remarks>2013-10-16 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.UN1002201)]
        [HttpPost]
        public JsonResult AdvertisementCreate(UnAdvertisement model)
        {
            var result = new Result() { StatusCode = -1 };
            try
            {
                if (ModelState.IsValid)
                {

                        var id = UnAdvertisementBo.Instance.Create(model);
                        if (id > 0)
                        {
                            result.Status = true;
                            result.StatusCode = 0;
                            SysLog.Instance.Info(LogStatus.系统日志来源.后台, "添加联盟广告", LogStatus.系统日志目标类型.联盟网站, id, CurrentUser.Base.SysNo);
                        }
                      
                }
                else
                {
                    result.Message = "联盟广告数据有误，请检查重试！";
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "添加联盟广告" + ex.Message, LogStatus.系统日志目标类型.联盟网站, CurrentUser.Base.SysNo, ex);
            }

            return Json(result);
        }

        /// <summary>
        /// 修改联盟广告
        /// </summary>
        /// <param name="id">联盟广告系统编号</param>
        /// <returns>视图页</returns>
        /// <remarks>2013-10-16 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.UN1002301)]
        public ActionResult AdvertisementEdit(int id)
        {
            var model = UnAdvertisementBo.Instance.GetModel(id);
            InitAdvertisementPageViewData(false, model);
            if (model != null)
            {
                model.ItemList = UnCpsProductBo.Instance.GetList(model.SysNo);
                ViewBag.ItemList = model.ItemList.ToJson2();
            }
            return View(model);
        }

        /// <summary>
        /// 修改联盟广告
        /// </summary>
        /// <param name="model">联盟广告实体</param>
        /// <returns>返回结果</returns>
        /// <remarks>2013-10-16 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.UN1002301)]
        [HttpPost]
        public JsonResult AdvertisementEdit(UnAdvertisement model)
        {
            var result = new Result() { StatusCode = -1 };
            try
            {
                if (ModelState.IsValid)
                {

                        var id = UnAdvertisementBo.Instance.Update(model);
                        if (id > 0)
                        {
                            result.Status = true;
                            result.StatusCode = 0;
                            SysLog.Instance.Info(LogStatus.系统日志来源.后台, "修改联盟广告", LogStatus.系统日志目标类型.联盟网站, id, CurrentUser.Base.SysNo);
                        }
                      
                }
                else
                {
                    result.Message = "联盟广告数据有误，请检查重试！";
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "修改联盟广告" + ex.Message, LogStatus.系统日志目标类型.联盟网站, CurrentUser.Base.SysNo, ex);
            }

            return Json(result);
        }

        /// <summary>
        /// 查看联盟广告
        /// </summary>
        /// <param name="id">联盟广告系统编号</param>
        /// <returns>视图页</returns>
        /// <remarks>2013-10-16 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.UN1002101)]
        public ActionResult AdvertisementDetail(int id)
        {
            var model = UnAdvertisementBo.Instance.GetModel(id);
            if (model != null)
            {
                model.ItemList = UnCpsProductBo.Instance.GetList(model.SysNo);
            }
            return View(model);
        }

        /// <summary>
        /// 联盟广告状态变更
        /// </summary>
        /// <param name="id">联盟广告系统编号</param>
        /// <param name="status">联盟广告状态</param>
        /// <returns>返回结果</returns>
        /// <remarks>2013-10-16 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.UN1002701)]
        public JsonResult AdvertisementStatusChange(int id, UnionStatus.联盟广告状态 status)
        {
            var result = new Result() { StatusCode = -1 };
            try
            {
                var model = UnAdvertisementBo.Instance.GetModel(id);
                if (model != null)
                {
                    model.Status = (int)status;
                    var rowsAffected = UnAdvertisementBo.Instance.Update(model);
                    if (rowsAffected > 0)
                    {
                        result.Status = true;
                        result.StatusCode = 0;
                        SysLog.Instance.Info(LogStatus.系统日志来源.后台, "联盟广告状态变更", LogStatus.系统日志目标类型.联盟网站, id, CurrentUser.Base.SysNo);
                    }
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "联盟广告状态变更" + ex.Message, LogStatus.系统日志目标类型.联盟网站, CurrentUser.Base.SysNo, ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 初始化联盟广告页面数据
        /// </summary>
        /// <param name="viewStyle">视图中下拉显示模式(true列表模式,false添加修改模式)</param>
        /// <param name="model">联盟广告实体</param>
        /// <remarks>2013-10-17 周唐炬 创建</remarks>
        /// <returns>void</returns>
        private void InitAdvertisementPageViewData(bool viewStyle, UnAdvertisement model)
        {
            var text = viewStyle ? "全部" : "请选择";
            var item = new SelectListItem() { Text = text, Value = "", Selected = true };
            var statusts = new List<SelectListItem>() { item };
            EnumUtil.ToListItem<UnionStatus.联盟广告状态>(ref statusts);

            var modelType = new List<SelectListItem>() { item };
            EnumUtil.ToListItem<UnionStatus.联盟广告类型>(ref modelType);

            var unionSiteList = new List<SelectListItem>() { item };
            var list = UnUnionSiteBo.Instance.GetAll();
            unionSiteList.AddRange(list.Select(site => new SelectListItem()
                {
                    Text = site.SiteName,
                    Value = site.SysNo.ToString(CultureInfo.InvariantCulture)
                }));
            if (model != null)
            {
                ViewBag.Status = new SelectList(statusts, "Value", "Text", model.Status);
                ViewBag.AdvertisementType = new SelectList(modelType, "Value", "Text", model.AdvertisementType);
                ViewBag.UnionSiteSysNo = new SelectList(unionSiteList, "Value", "Text", model.UnionSiteSysNo);
            }
            else
            {
                ViewBag.Status = new SelectList(statusts, "Value", "Text");
                ViewBag.AdvertisementType = new SelectList(modelType, "Value", "Text");
                ViewBag.UnionSiteSysNo = new SelectList(unionSiteList, "Value", "Text");
            }
        }
        #endregion

        #region 联盟广告日志 2013-10-18 周唐炬
        /// <summary>
        /// 联盟广告日志
        /// </summary>
        /// <param name="id">页码</param>
        /// <param name="filter">过滤条件</param>
        /// <returns>查询数据</returns>
        /// <remarks>2013-10-16 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.UN1003101)]
        public ActionResult AdvLog(int? id, ParaUnAdvertisementLogFilter filter)
        {
            try
            {
                if (Request.IsAjaxRequest())
                {
                    filter.CurrentPage = id ?? 1;
                    var list = UnAdvertisementLogBo.Instance.GetList(filter);
                    return PartialView("_AdvLogList", list);
                }
                InitAdvLogPageViewData();
            }
            catch (Exception ex)
            {
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "查询联盟广告日志" + ex.Message, LogStatus.系统日志目标类型.联盟网站, CurrentUser.Base.SysNo, ex);
            }
            return View();
        }

        /// <summary>
        /// 联盟广告日志详细
        /// </summary>
        /// <param name="id">联盟广告日志系统编号</param>
        /// <returns>联盟广告日志视图</returns>
        /// <remarks>2013-10-16 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.UN1003101)]
        public ActionResult AdvLogDetail(int id)
        {
            var model = UnAdvertisementLogBo.Instance.GetModel(id);
            return View(model);
        }

        /// <summary>
        /// 联盟广告日志导出Excel
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns>Excel数据</returns>
        /// <remarks>2013-10-16 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.UN1003101)]
        public ActionResult ExportExcel(ParaUnAdvertisementLogFilter filter)
        {
            try
            {
                filter.CurrentPage = 1;
                filter.PageSize = Int32.MaxValue;
                var list = UnAdvertisementLogBo.Instance.ExportExcel(filter);
                if (list != null && list.Any())
                {
                    SysLog.Instance.Info(LogStatus.系统日志来源.后台, "联盟广告日志导出Excel", LogStatus.系统日志目标类型.联盟网站, 0, CurrentUser == null ? 0 : CurrentUser.Base.SysNo);
                    ExcelUtil.Export<CBUnAdvertisementLog>(list);
                }
            }
            catch (Exception ex)
            {
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "联盟广告日志导出Excel" + ex.Message, LogStatus.系统日志目标类型.联盟网站, 0, ex, CurrentUser == null ? 0 : CurrentUser.Base.SysNo);
            }
            return RedirectToAction("AdvLog");
        }

        /// <summary>
        /// 初始化联盟广告日志各状态
        /// </summary>
        /// <param name=" ">无</param>
        /// <returns>void</returns>
        /// <remarks>2013-10-16 周唐炬 创建</remarks>
        private void InitAdvLogPageViewData()
        {
            var item = new SelectListItem() { Text = @"全部", Value = "", Selected = true };
            var isValid = new List<SelectListItem>() { item };
            EnumUtil.ToListItem<UnionStatus.广告日志是否有效>(ref isValid);

            var modelType = new List<SelectListItem>() { item };
            EnumUtil.ToListItem<UnionStatus.联盟广告类型>(ref modelType);

            var unionSiteList = new List<SelectListItem>() { item };
            var list = UnUnionSiteBo.Instance.GetAll();
            unionSiteList.AddRange(list.Select(site => new SelectListItem()
            {
                Text = site.SiteName,
                Value = site.SysNo.ToString(CultureInfo.InvariantCulture)
            }));

            ViewBag.IsValid = new SelectList(isValid, "Value", "Text");
            ViewBag.AdvertisementType = new SelectList(modelType, "Value", "Text");
            ViewBag.UnionSiteSysNo = new SelectList(unionSiteList, "Value", "Text");

        }
        #endregion

        #region 微信公众平台自动回复 2013-10-23 黄伟

        /// <summary>
        /// 微信自动回复配置UN1004101
        /// </summary>
        /// <param name=" "></param>
        /// <returns>ActionResult</returns>
        [Privilege(PrivilegeCode.UN1004101)]
        public ActionResult WeChatAutoReplyConfig()
        {
            var model = WeChatBo.Instance.GetWeixinConfig();

            return View(model ?? new MkWeixinConfig());
        }

        /// <summary>
        /// 微信自动回复关键词列表
        /// </summary>
        /// <param name=" "></param>
        /// <returns>ActionResult</returns>
        [Privilege(PrivilegeCode.UN1005101)]
        public ActionResult WeChatAutoReplyKeyWordsList(int? id, ParaMkWeixinKeywords para)
        {
            //当前用户自己对应分销商
            if (CurrentUser.IsBindDealer)
            {
                int DealerSysNo = CurrentUser.Dealer.SysNo;
                para.DealerSysNo = DealerSysNo;
                para.IsBindDealer = CurrentUser.IsBindDealer;
            }
            //是否绑定所有经销商
            para.IsBindAllDealer = CurrentUser.IsBindAllDealer;
            para.DealerCreatedBy = CurrentUser.Base.SysNo;

            var dic = WeChatBo.Instance.QueryKeyWords(para, id ?? 1);

            var model = new PagedList<CBMkWeixinKeywords>
            {
                TData = dic.Any() ? dic.First().Value : null,
                CurrentPageIndex = id ?? 1,
                TotalItemCount = dic.Any() ? dic.First().Key : 0
            };

            if (Request.IsAjaxRequest())
            {
                return PartialView("pAjaxPager_WeChatAutoReplyWordsList", model);
            }

            return View(model);
        }

        /// <summary>
        /// 添加/修改关键词
        /// </summary>
        /// <param name=" "></param>
        /// <returns>view添加/修改关键词</returns>
        /// <remarks>2013-10-24 黄伟 创建</remarks>
        [Privilege(PrivilegeCode.UN1005301)]
        [HttpGet]
        public ActionResult CreateOrUpdateKeyWords(int? id, int? sysNo)
        {
            //update or just reload after creation
            if (sysNo != null)
            {
                MkWeixinKeywords Entity = new MkWeixinKeywords();
                Entity = WeChatBo.Instance.GetMkWeixinKeywordsBySysNo((int)sysNo);

                //update keywords,load the existed keywords content list
                var mkWeixinKeywordsReply = new MkWeixinKeywordsReply { WeixinKeywordsSysNo = (int)sysNo };
                var dicContent = WeChatBo.Instance.QueryKeyWordsContent(mkWeixinKeywordsReply);

                var model = new PagedList<MkWeixinKeywordsReply>
                    {
                        TData = dicContent.Any() ? dicContent.First().Value : null,
                        CurrentPageIndex = id ?? 1,
                        TotalItemCount = dicContent.Any() ? dicContent.First().Key : 0
                    };
                //reload
                if (Request.IsAjaxRequest())
                {
                    return PartialView("QueryKeyWordsContent", model);
                }
                ViewBag.AgentSysNo = Entity.AgentSysNo;
                ViewBag.DealerSysNo = Entity.DealerSysNo;
                ViewBag.keyWordSysNo = sysNo;
                return View(model);
            }

            return View();

        }

        /// <summary>
        /// 新增关键词
        /// </summary>
        /// <param name=" "></param>
        /// <returns>ActionResult</returns>
        /// <remarks>2013-10-25 黄伟 创建</remarks>
        [Privilege(PrivilegeCode.UN1005401)]
        [HttpPost]
        public ActionResult CreateKeyWords(MkWeixinKeywords model)
        {
            var result = WeChatBo.Instance.CreateKeyWords(model, HttpContext.Request.ServerVariables["Remote_Add"], CurrentUser.Base.SysNo);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加关键词内容
        /// </summary>
        /// <param name="type">text or mix(text and image)</param>
        /// <param name="mkWeixinKeywordsReplySysNo">关键词内容回复系统编号</param>
        /// <returns>ActionResult</returns>
        [Privilege(PrivilegeCode.UN1006101)]
        [HttpGet]
        public ActionResult AddKeyWordsContent(string type, int? mkWeixinKeywordsReplySysNo)
        {
            if (type == null)
                return null;
            ViewBag.option = type;
            if (mkWeixinKeywordsReplySysNo != null)
                return View(new MkWeixinKeywordsReply { SysNo = (int)mkWeixinKeywordsReplySysNo });

            return View();
        }

        /// <summary>
        /// 新增关键词回复内容
        /// </summary>
        /// <param name="model">MkWeixinKeywordsReply</param>
        /// <returns>ActionResult</returns>
        /// <remarks>2013-10-25 黄伟 创建</remarks>
        [Privilege(PrivilegeCode.UN1006301)]
        [HttpPost]
        public ActionResult AddKeyWordsContent(MkWeixinKeywordsReply model)
        {
            var result = WeChatBo.Instance.CreateKeyWordsContent(model, HttpContext.Request.ServerVariables["Remote_Add"], CurrentUser.Base.SysNo);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        ///// <summary>
        ///// 新增关键词回复内容
        ///// </summary>
        ///// <returns></returns>
        ///// <remarks>2013-10-25 黄伟 创建</remarks>
        //[Privilege(PrivilegeCode.FN1005201)]
        //public ActionResult QueryKeyWordsContentAll(MkWeixinKeywordsReply model)
        //{
        //    var result = WeChatBo.Instance.QueryKeyWordsContentAll(model);
        //    return View(result);
        //}

        /// <summary>
        /// 更新关键词回复内容
        /// </summary>
        /// <param name="model">MkWeixinKeywordsReply</param>
        /// <returns>ActionResult</returns>
        /// <remarks>2013-10-25 黄伟 创建</remarks>
        [Privilege(PrivilegeCode.UN1006301)]
        [HttpPost]
        public ActionResult UpdateKeyWordsContent(MkWeixinKeywordsReply model)
        {
            var result = WeChatBo.Instance.UpdateKeyWordsContent(new List<MkWeixinKeywordsReply> { model }, HttpContext.Request.ServerVariables["Remote_Add"], CurrentUser.Base.SysNo);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存自动回复配置
        /// </summary>
        /// <param name="model">MkWeixinConfig</param>
        /// <returns>ActionResult</returns>
        /// <remarks>2013-10-25 黄伟 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.UN1004101)]
        public ActionResult SaveAutoRelpyConf(MkWeixinConfig model)
        {
            var result = WeChatBo.Instance.Create(model, Request.Params["Remote_add"], CurrentUser.Base.SysNo);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 设置关键词启用/禁用
        /// </summary>
        /// <param name="sysNo">keyWord sysno</param>
        /// <param name="status">keyWord status</param>
        /// <returns>ActionResult</returns>
        /// <remarks>2010-10-10 黄伟 创建</remarks>
        [Privilege(PrivilegeCode.UN1005301)]
        [HttpPost]
        public ActionResult SetKeyWordsStatus(int sysNo, string status)
        {
            var result = WeChatBo.Instance.SetKeyWordsStatus(sysNo, Enum.Parse(typeof(MarketingStatus.微信关键词状态), status).GetHashCode(), Request.Params["Remote_add"], CurrentUser.Base.SysNo);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 设置关键词内容启用/禁用
        /// </summary>
        /// <param name="sysNo">keyWordcontent sysno</param>
        /// <param name="status">keyWordcontent status</param>
        /// <returns>ActionResult</returns>
        /// <remarks>2010-10-10 黄伟 创建</remarks>
        [Privilege(PrivilegeCode.UN1006301)]
        [HttpPost]
        public ActionResult SetKeyWordsContentStatus(int sysNo, string status)
        {
            var result = WeChatBo.Instance.SetKeyWordsContentStatus(sysNo, Enum.Parse(typeof(MarketingStatus.微信关键词回复状态), status).GetHashCode(), Request.Params["Remote_add"], CurrentUser.Base.SysNo);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除关键字
        /// </summary>
        /// <param name="arrDelSysnos">要删除的关键词系统编号(逗号分隔)字符串</param>
        /// <returns>ActionResult</returns>
        /// <remarks>2010-10-10 黄伟 创建</remarks>
        [Privilege(PrivilegeCode.UN1005401)]
        [HttpPost]
        public ActionResult DeleteKeyWords(string arrDelSysnos)
        {
            var lstDelSysnos = arrDelSysnos.Split(',').Select(int.Parse).ToList();

            var result = WeChatBo.Instance.DeleteKeyWords(lstDelSysnos,
                                                             HttpContext.Request.ServerVariables["Remote_Add"],
                                                             CurrentUser.Base.SysNo);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除关键字内容
        /// </summary>
        /// <param name="arrDelSysnos">要删除的关键词内容系统编号(逗号分隔)字符串</param>
        /// <returns>ActionResult</returns>
        /// <remarks>2013-10-31 黄伟 创建</remarks>
        [Privilege(PrivilegeCode.UN1006401)]
        [HttpPost]
        public ActionResult DeleteKeyWordsContent(string arrDelSysnos)
        {
            var lstDelSysnos = arrDelSysnos.Split(',').Select(int.Parse).ToList();

            var result = WeChatBo.Instance.DeleteKeyWordsContent(lstDelSysnos,
                                                             HttpContext.Request.ServerVariables["Remote_Add"],
                                                             CurrentUser.Base.SysNo);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 微信咨询客服回复 2013-11-08 郑荣华
        /// <summary>
        /// 微信咨询主页面
        /// </summary>
        /// <returns>微信咨询主页面</returns>
        /// <param name="id">页码</param>
        /// <param name="weixinId">微信号</param>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <remarks> 
        /// 2013-11-08 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.UN1007101)]
        public ActionResult WeChat(int? id, string weixinId, string startDate, string endDate)
        {
            if (Request.IsAjaxRequest())
            {
                //列表分页开始
                var model = new PagedList<CBMkWeixinQuestion>();

                var filter = new ParaMkWeixinQuestionFilter { WeixinId = weixinId };
                if (!string.IsNullOrEmpty(startDate)) filter.StartTime = DateTime.Parse(startDate);
                if (!string.IsNullOrEmpty(endDate)) filter.EndTime = DateTime.Parse(endDate);
                if (filter.EndTime != null)
                    filter.EndTime = filter.EndTime.Value.AddDays(1);
                var modelRef = new Pager<CBMkWeixinQuestion> { CurrentPage = id ?? 1, PageSize = model.PageSize };
                WeChatBo.Instance.GetMkWeixinQuestionStaticsList(ref modelRef, filter);
                model.TotalItemCount = modelRef.TotalRows;
                model.TData = modelRef.Rows;
                model.CurrentPageIndex = modelRef.CurrentPage;
                return PartialView("_AjaxPagerWeChat", model);
            }
            return View();
        }

        /// <summary>
        /// Ajax修改微信咨询状态为已读
        /// </summary>
        /// <param name="weixinId">微信号‘,’分隔</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks> 
        /// 2013-11-08 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.UN1007101)]
        public JsonResult AjaxWeixinQuestionUpdateStatus(string weixinId)
        {
            var list = weixinId.Split(',').ToList();

                try
                {
                    foreach (var item in list)
                    {
                        WeChatBo.Instance.UpdateStatus(item);
                    }
                }
                catch (Exception ex)
                {
                    SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "修改微信咨询状态为已读:" + ex.Message,
                                             LogStatus.系统日志目标类型.微信, 0, ex);
                    return Json(false);
                }
            
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 微信咨询回复页面
        /// </summary>
        /// <returns>微信咨询回复页面</returns>
        /// <param name="id">页码</param>
        /// <param name="weixinId">微信号</param>
        /// <remarks> 
        /// 2013-11-08 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.UN1007201)]
        public ActionResult WeChatReply(int? id, string weixinId)
        {
            if (Request.IsAjaxRequest())
            {
                //列表分页开始
                var model = new PagedList<CBMkWeixinQuestion> { PageSize = 4 };
                var modelRef = new Pager<CBMkWeixinQuestion> { CurrentPage = id ?? 1, PageSize = model.PageSize };
                WeChatBo.Instance.GetMkWeixinQuestionList(ref modelRef, weixinId);
                model.TotalItemCount = modelRef.TotalRows;
                model.TData = modelRef.Rows;
                model.CurrentPageIndex = modelRef.CurrentPage;
                return PartialView("_AjaxPagerWeChatReplay", model);
            }
            //更新已读
            WeChatBo.Instance.UpdateStatus(weixinId);
            ViewBag.WeixinId = weixinId;
            ViewBag.Account = CurrentUser.Base.Account;
            return View();
        }

        /// <summary>
        /// 客服回复微信
        /// </summary>
        /// <param name="weixinId">微信号</param>
        /// <param name="message">回复信息</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks> 
        /// 2013-11-11 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.UN1007201)]
        public JsonResult AjaxWeChatReplay(string weixinId, string message)
        {
            var result = CallCenterReplyBo.Instance.WriteBackMessage(weixinId, message, CurrentUser.Base.SysNo);

            return Json(result.Status, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 微信Token验证

        /// <summary>
        /// 响应微信发送Token验证
        /// </summary>
        /// <param name=" "></param>
        /// <returns>void</returns>
        /// <remarks>2013-01-07 huangwei 添加注释</remarks>
        [CustomActionFilter(false)]
        public void WeixinToken()
        {
            var method = Request.HttpMethod.ToLower();

            if (method == "post")
            {
                Stream s = System.Web.HttpContext.Current.Request.InputStream;
                byte[] b = new byte[s.Length];
                s.Read(b, 0, (int)s.Length);
                string postStr = Encoding.UTF8.GetString(b);
                if (!string.IsNullOrEmpty(postStr)) //请求处理
                {
                    AutoReplyBo.Instance.Handle(postStr);
                }
            }
            else
            {
                string echoStr = System.Web.HttpContext.Current.Request.QueryString["echoStr"];

                if (CheckSignature())
                {
                    if (!string.IsNullOrEmpty(echoStr))
                    {
                        System.Web.HttpContext.Current.Response.Write(echoStr);
                        System.Web.HttpContext.Current.Response.End();
                    }
                }
            }
        }

        /// <summary>
        /// 验证微信签名
        /// </summary>
        /// <param name=" "></param>
        /// * 将token、timestamp、nonce三个参数进行字典序排序
        /// * 将三个参数字符串拼接成一个字符串进行sha1加密
        /// * 开发者获得加密后的字符串可与signature对比，标识该请求来源于微信。
        /// <returns>bool</returns>
        /// <remarks>2013-01-07 huangwei 添加注释</remarks>
        [CustomActionFilter(false)]
        private bool CheckSignature()
        {
            string signature = System.Web.HttpContext.Current.Request.QueryString["signature"];
            string timestamp = System.Web.HttpContext.Current.Request.QueryString["timestamp"];
            string nonce = System.Web.HttpContext.Current.Request.QueryString["nonce"];
            var weixinConfig = WeChatBo.Instance.GetWeixinConfig();
            if (weixinConfig == null)
                return false;
            string[] ArrTmp = { weixinConfig.Token, timestamp, nonce };
            Array.Sort(ArrTmp);     //字典排序
            string tmpStr = string.Join("", ArrTmp);
            tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
            tmpStr = tmpStr.ToLower();
            if (tmpStr == signature)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        
        /// <summary>
        /// 微信自动回复配置
        /// </summary>
        /// <param name=" "></param>
        /// <returns>ActionResult</returns>
        [Privilege(PrivilegeCode.UN1008101)]
        public ActionResult MkWeixinConfigList(int? id, ParaMkWeixinConfigFilter filter)
        {
            if (Request.IsAjaxRequest())
            {
                if (filter.Condition != null)
                {
                    filter.Token = filter.Condition;
                }
                //当前用户自己对应分销商
                if (CurrentUser.IsBindDealer)
                {
                    int DealerSysNo = CurrentUser.Dealer.SysNo;
                    filter.DealerSysNo = DealerSysNo;
                    filter.IsBindDealer = CurrentUser.IsBindDealer;
                }
                //是否绑定所有经销商
                filter.IsBindAllDealer = CurrentUser.IsBindAllDealer;
                filter.DealerCreatedBy = CurrentUser.Base.SysNo;
                //列表分页开始
                var model = new PagedList<CBMkWeixinConfig>();

                var modelRef = new Pager<CBMkWeixinConfig> { CurrentPage = id ?? 1, PageSize = model.PageSize };
                WeChatBo.Instance.GetMkWeixinConfigList(ref modelRef, filter);
                model.TotalItemCount = modelRef.TotalRows;
                model.TData = modelRef.Rows;
                model.CurrentPageIndex = modelRef.CurrentPage;
                return PartialView("_AjaxPagerMkWeixinConfig", model);
            }
            return View();
        }
        /// <summary>
        /// 新增回复配置
        /// </summary>
        /// <param name="id">回复配置</param>
        /// <returns>视图</returns>
        /// <remarks>2015-08-30 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.UN1008102)]
        public ActionResult MkWeixinConfigCreate(int? id)
        {
            MkWeixinConfig model = new MkWeixinConfig();
            if (id.HasValue)
            {
                model = WeChatBo.Instance.GetEntity(id.Value);
            }
            return View(model);
        }

        /// <summary>
        /// 保存回复配置
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2015-08-30 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.UN1008102)]
        public JsonResult SaveMkWeixinConfig(MkWeixinConfig model)
        {
            var result = new Result();
            try
            {
                result = WeChatBo.Instance.SaveMkWeixinConfig(model, CurrentUser.Base);
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
