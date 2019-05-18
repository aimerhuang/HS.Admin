using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Transactions;
using System.Web.Configuration;
using System.Web.Mvc;
using Hyt.BLL.Basic;
using Hyt.BLL.Distribution;
using Hyt.BLL.Log;
using Hyt.BLL.Logistics;
using Hyt.BLL.Warehouse;
using Hyt.BLL.Web;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.SystemPredefined;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Util;
using IsolationLevel = System.Transactions.IsolationLevel;
using WhWarehouseBo = Hyt.BLL.Warehouse.WhWarehouseBo;


namespace Hyt.Admin.Controllers
{
    /// <summary>
    /// 基础数据管理
    /// </summary>
    public class BasicController : BaseController
    {
        #region 组织机构 2013-10-08 周唐炬 创建
        /// <summary>
        /// 组织机构管理
        /// </summary>
        /// <param name="sysNo">当前组织机构系统编号</param>
        /// <param name="id">页码</param>
        /// <returns>组织机构管理视图</returns>
        /// <remarks>2013-10-08 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.BS1004101)]
        public ActionResult OrganizationManage(int? sysNo, int? id)
        {
            if (Request.IsAjaxRequest())
            {
                PagedList<BsOrganizationWarehouse> list = null;
                if (sysNo.HasValue)
                {
                    var model = OrganizationBo.Instance.GetEntity(sysNo.Value);
                    if (model == null) return null;
                    ViewBag.SysNo = sysNo;
                    var status = TempData["Status"] as int?;
                    ViewBag.Status = status == (int)BasicStatus.组织机构状态.禁用 ? status : model.Status;

                    var currentPage = id ?? 1;
                    list = OrganizationBo.Instance.GetItems(model.SysNo, currentPage);
                }
                return PartialView("_AjaxPagerOrganizationItemList", list);
            }
            return View();
        }

        /// <summary>
        /// 获取组织机构
        /// </summary>
        /// <param name="id">组织机构系统编号</param>
        /// <param name="status"></param>
        /// <returns>返回结果</returns>
        /// <remarks>2013-10-08 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.BS1004101)]
        public ActionResult OrganizationDetail(int id, BasicStatus.组织机构状态 status)
        {
            var model = OrganizationBo.Instance.GetEntity(id);
            if (status == BasicStatus.组织机构状态.禁用)
            {
                TempData["Status"] = (int)BasicStatus.组织机构状态.禁用;
            }
            return PartialView("OrganizationDetail", model);
        }

        /// <summary>
        /// 菜单上移/下移
        /// </summary>
        /// <param name="sourceNodeId">原节点</param>
        /// <param name="targetNodeId">目标节点</param>
        /// <param name="direction">移动方向</param>
        /// <returns>返回结果</returns>
        /// <remarks>2013-10-08 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.BS1004301)]
        public JsonResult MoveTreeNode(int sourceNodeId, int targetNodeId, string direction)
        {
            var result = new Result() { StatusCode = -1 };
            try
            {
                OrganizationBo.Instance.OrganizationDisplayOrderMove(sourceNodeId, targetNodeId);
                result.Status = true;
                result.StatusCode = 0;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "组织机构菜单上移/下移" + ex.Message, LogStatus.系统日志目标类型.组织机构, CurrentUser.Base.SysNo, ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取组织机构列表
        /// </summary>
        /// <returns>关联仓库</returns>
        /// <remarks>2013-10-08 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.BS1004101)]
        public JsonResult OrganizationMenu()
        {
            var result = new Result<List<ZTreeNode>>() { StatusCode = -1 };
            try
            {
                var list = OrganizationBo.Instance.GetAll().Select(x => new ZTreeNode()
                    {
                        id = x.SysNo,
                        pId = x.ParentSysNo,
                        name = x.Name,
                        status = x.Status,
                        open = false
                    }).ToList();

                result.Data = list;
                result.Status = true;
                result.StatusCode = 0;
            }
            catch (Exception ex)
            {
                result.Message = "获取组织机构列表异常，请稍后重试！";
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "获取组织机构列表" + ex.Message, LogStatus.系统日志目标类型.组织机构, CurrentUser.Base.SysNo, ex);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加组织机构
        /// </summary>
        /// <param name="id">组织机构系统编号</param>
        /// <returns>添加组织机构视图</returns>
        /// <remarks>2013-10-08 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.BS1004201)]
        public ActionResult OrganizationCreate(int id)
        {
            var model = new BsOrganization();
            if (id > 0)
            {
                model = OrganizationBo.Instance.GetEntity(id);
            }
            InitOrganizationViewData(null);
            return View(model);
        }

        /// <summary>
        /// 添加组织机构
        /// </summary>
        /// <param name="model">组织机构实体</param>
        /// <returns>返回结果</returns>
        /// <remarks>2013-10-08 周唐炬 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.BS1004201)]
        public JsonResult OrganizationCreate(BsOrganization model)
        {
            var result = new Result() { StatusCode = -1 };
            try
            {
                if (ModelState.IsValid)
                {
                    model.CreatedBy = model.LastUpdateBy = CurrentUser.Base.SysNo;
                    model.CreatedDate = model.LastUpdateDate = DateTime.Now;
                    var id = OrganizationBo.Instance.OrganizationCreate(model);
                    if (id > 0)
                    {
                        result.Status = true;
                        result.StatusCode = id;
                        SysLog.Instance.Info(LogStatus.系统日志来源.后台, "添加组织机构:" + model.Name, LogStatus.系统日志目标类型.组织机构, id, CurrentUser.Base.SysNo);
                    }
                }
                else
                {
                    result.Message = "数据错误,请重试!";
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "添加组织机构" + ex.Message, LogStatus.系统日志目标类型.组织机构, CurrentUser.Base.SysNo, ex);
            }

            return Json(result);
        }

        /// <summary>
        /// 修改组织机构
        /// </summary>
        /// <param name="id">组织机构系统编号</param>
        /// <returns>修改组织机构视图</returns>
        /// <remarks>2013-10-08 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.BS1004301)]
        public ActionResult OrganizationEdit(int id)
        {
            var model = OrganizationBo.Instance.GetEntity(id);
            InitOrganizationViewData(model);
            return View(model);
        }

        /// <summary>
        /// 修改组织机构
        /// </summary>
        /// <param name="model">组织机构实体</param>
        /// <returns>返回结果</returns>
        /// <remarks>2013-10-08 周唐炬 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.BS1004301)]
        public JsonResult OrganizationEdit(BsOrganization model)
        {
            var result = new Result() { StatusCode = -1 };
            try
            {
                if (ModelState.IsValid)
                {
                    model.LastUpdateBy = CurrentUser.Base.SysNo;
                    model.LastUpdateDate = DateTime.Now;
                    var id = OrganizationBo.Instance.OrganizationUpdate(model);
                    if (id > 0)
                    {
                        result.Status = true;
                        result.StatusCode = 0;
                        SysLog.Instance.Info(LogStatus.系统日志来源.后台, "修改组织机构:" + model.Name, LogStatus.系统日志目标类型.组织机构, id, CurrentUser.Base.SysNo);
                    }
                }
                else
                {
                    result.Message = "数据错误,请重试!";
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "修改组织机构" + ex.Message, LogStatus.系统日志目标类型.组织机构, CurrentUser.Base.SysNo, ex);
            }

            return Json(result);
        }

        /// <summary>
        /// 删除组织机构
        /// </summary>
        /// <param name="id">组织机构系统编号</param>
        /// <returns>返回结果</returns>
        /// <remarks>2013-10-08 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.BS1004401)]
        public JsonResult OrganizationRemove(int id)
        {
            var result = new Result() { StatusCode = -1 };
            try
            {

                OrganizationBo.Instance.OrganizationRemove(id);
                result.Status = true;
                result.StatusCode = 0;
                SysLog.Instance.Info(LogStatus.系统日志来源.后台, "删除组织机构:" + id, LogStatus.系统日志目标类型.组织机构, id, CurrentUser.Base.SysNo);

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "删除组织机构" + ex.Message, LogStatus.系统日志目标类型.组织机构, CurrentUser.Base.SysNo, ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 组织机构状态变更
        /// </summary>
        /// <param name="id">组织机构系统编号</param>
        /// <returns>返回结果</returns>
        /// <remarks>2013-10-08 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.BS1004301)]
        public JsonResult OrganizationStatusChange(int id)
        {
            var result = new Result() { StatusCode = -1 };
            try
            {
                OrganizationBo.Instance.OrganizationStatusChange(id);
                result.Status = true;
                result.StatusCode = 0;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "组织机构状态变更" + ex.Message, LogStatus.系统日志目标类型.组织机构, CurrentUser.Base.SysNo, ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 批量添加组织机构关联仓库
        /// </summary>
        /// <param name="id">组织机构系统编号</param>
        /// <param name="whlist">仓库编号列表</param>
        /// <returns>返回结果</returns>
        /// <remarks>2013-10-08 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.BS1004202)]
        public JsonResult OrganizationItemAddRange(int id, List<int> whlist)
        {
            var result = new Result() { StatusCode = -1 };
            try
            {

                OrganizationBo.Instance.OrganizationItemAddRange(id, whlist, CurrentUser.Base.SysNo);
                result.Status = true;
                result.StatusCode = 0;
                SysLog.Instance.Info(LogStatus.系统日志来源.后台, "添加组织机构关联仓库:" + id, LogStatus.系统日志目标类型.组织机构, id, CurrentUser.Base.SysNo);

            }
            catch (HytException hytex) //自定义提示信息不记录日志
            {
                result.Message = hytex.Message;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "添加组织机构关联仓库" + ex.Message, LogStatus.系统日志目标类型.组织机构, CurrentUser.Base.SysNo, ex);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除组织机构关联仓库
        /// </summary>
        /// <param name="id">组织机构系统编号</param>
        /// <returns>返回结果</returns>
        /// <remarks>2013-10-08 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.BS1004402)]
        public JsonResult OrganizationItemRemove(int id)
        {
            var result = new Result() { StatusCode = -1 };
            try
            {
                OrganizationBo.Instance.ItemRemove(id);
                result.Status = true;
                result.StatusCode = 0;
                SysLog.Instance.Info(LogStatus.系统日志来源.后台, "删除组织机构关联仓库:" + id, LogStatus.系统日志目标类型.组织机构, id, CurrentUser.Base.SysNo);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "删除组织机构关联仓库" + ex.Message, LogStatus.系统日志目标类型.组织机构, CurrentUser.Base.SysNo, ex);
            }

            return Json(result);
        }

        /// <summary>
        /// 初始化组织机构页面数据
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        /// <remarks>2013-10-08 周唐炬 创建</remarks>
        private void InitOrganizationViewData(BsOrganization model)
        {
            var statusts = new List<SelectListItem>();
            EnumUtil.ToListItem<BasicStatus.组织机构状态>(ref statusts);
            ViewBag.Status = model != null ? new SelectList(statusts, "Value", "Text", model.Status) : new SelectList(statusts, "Value", "Text", (int)BasicStatus.组织机构状态.启用);
        }
        #endregion

        #region 支付方式维护 周唐炬
        /// <summary>
        /// 支付方式维护
        /// </summary>
        /// <param name="filter">过滤条件</param>
        /// <returns>支付方式列表</returns>
        /// <remarks>2013-09-06 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.BS1003101)]
        public ActionResult PaymentTypeManage(ParaPaymentTypeFilter filter)
        {
            if (Request.IsAjaxRequest())
            {
                var models = PaymentTypeBo.Instance.GetPaymentTypeList(filter);
                return PartialView("_AjaxPagerPaymentTypeList", models);
            }
            InitPaymentTypeViewData(true, null);
            return View();
        }

        /// <summary>
        /// 添加新支付方式
        /// </summary>
        /// <returns>视图页</returns>
        /// <remarks>2013-09-06 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.BS1003201)]
        public ActionResult PaymentTypeCreate()
        {
            InitPaymentTypeViewData(false, null);
            return View();
        }

        /// <summary>
        /// 添加新支付方式
        /// </summary>
        /// <param name="model">支付方式实体</param>
        /// <returns>返回结果</returns>
        /// <remarks>2013-09-06 周唐炬 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.BS1003201)]
        public JsonResult PaymentTypeCreate(BsPaymentType model)
        {
            var result = new Result() { StatusCode = -1 };
            try
            {
                if (ModelState.IsValid)
                {
                    model.CreatedBy = model.LastUpdateBy = CurrentUser.Base.SysNo;
                    model.CreatedDate = model.LastUpdateDate = DateTime.Now;
                    var id = PaymentTypeBo.Instance.PaymentTypeCreate(model);
                    if (id > 0)
                    {
                        result.Status = true;
                        result.StatusCode = 0;
                        SysLog.Instance.Info(LogStatus.系统日志来源.后台, "添加新支付方式" + model.PaymentName, LogStatus.系统日志目标类型.支付方式, id, CurrentUser.Base.SysNo);
                    }
                }
                else
                {
                    result.Message = "支付方式数据有误，请检查重试！";
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "添加新支付方式" + ex.Message, LogStatus.系统日志目标类型.支付方式, CurrentUser.Base.SysNo, ex);
            }

            return Json(result);
        }

        /// <summary>
        ///修改支付方式 
        /// </summary>
        /// <param name="id">支付方式系统编号</param>
        /// <returns>视图页</returns>
        /// <remarks>2013-09-06 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.BS1003301)]
        public ActionResult PaymentTypeEdit(int id)
        {
            var model = PaymentTypeBo.Instance.GetEntity(id);
            InitPaymentTypeViewData(false, model);
            return View(model);
        }

        /// <summary>
        /// 修改支付方式
        /// </summary>
        /// <param name="model">支付方式实体</param>
        /// <returns>返回结果</returns>
        /// <remarks>2013-09-06 周唐炬 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.BS1003301)]
        public JsonResult PaymentTypeEdit(BsPaymentType model)
        {
            var result = new Result() { StatusCode = -1 };
            try
            {
                if (ModelState.IsValid)
                {
                    model.LastUpdateBy = CurrentUser.Base.SysNo;
                    model.LastUpdateDate = DateTime.Now;
                    var id = PaymentTypeBo.Instance.PaymentTypeUpdate(model);
                    if (id > 0)
                    {
                        result.Status = true;
                        result.StatusCode = 0;
                        SysLog.Instance.Info(LogStatus.系统日志来源.后台, "修改支付方式" + model.PaymentName, LogStatus.系统日志目标类型.支付方式, id, CurrentUser.Base.SysNo);
                    }
                }
                else
                {
                    result.Message = "支付方式数据有误，请检查重试！";
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "修改支付方式" + ex.Message, LogStatus.系统日志目标类型.支付方式, CurrentUser.Base.SysNo, ex);
            }

            return Json(result);
        }

        /// <summary>
        /// 支付方式详细
        /// </summary>
        /// <param name="id">支付方式系统编号</param>
        /// <returns>视图页</returns>
        /// <remarks>2013-09-06 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.BS1003101)]
        public ActionResult PaymentTypeDetail(int id)
        {
            var model = PaymentTypeBo.Instance.GetEntity(id);
            return View(model);
        }

        /// <summary>
        /// 支付方式状态变更
        /// </summary>
        /// <param name="id">支付方式系统编号</param>
        /// <param name="status">支付方式状态</param>
        /// <returns>返回结果</returns>
        /// <remarks>2013-09-06 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.BS1003701)]
        public JsonResult PaymentTypeStatusChange(int id, BasicStatus.支付方式状态 status)
        {
            var result = new Result() { StatusCode = -1 };
            try
            {
                var model = PaymentTypeBo.Instance.GetEntity(id);
                if (model != null)
                {
                    model.Status = (int)status;
                    model.LastUpdateBy = CurrentUser.Base.SysNo;
                    model.LastUpdateDate = DateTime.Now;
                    var rowsAffected = PaymentTypeBo.Instance.PaymentTypeUpdate(model);
                    if (rowsAffected > 0)
                    {
                        result.Status = true;
                        result.StatusCode = 0;
                        SysLog.Instance.Info(LogStatus.系统日志来源.后台, "支付方式状态变更" + model.PaymentName, LogStatus.系统日志目标类型.支付方式, id, CurrentUser.Base.SysNo);
                    }
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "支付方式状态变更" + ex.Message, LogStatus.系统日志目标类型.支付方式, CurrentUser.Base.SysNo, ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 支付名称验证
        /// </summary>
        /// <param name="paymentName">支付名称</param>
        /// <param name="id">支付方式系统编号</param>
        /// <returns>验证结果</returns>
        /// <remarks>2013-09-06 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.BS1003201, PrivilegeCode.BS1003301)]
        public JsonResult PaymentTypeVerify(string paymentName, int? id)
        {
            var result = false;
            if (!string.IsNullOrWhiteSpace(paymentName))
            {
                result = PaymentTypeBo.Instance.PaymentTypeVerify(paymentName, id);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 初始化支付方式页面数据
        /// </summary>
        /// <param name="viewStyle">视图中下拉显示模式(true列表模式,false添加修改模式)</param>
        /// <param name="model">支付方式实体</param>
        /// <returns></returns>
        /// <remarks>2013-09-06 周唐炬 创建</remarks>
        private void InitPaymentTypeViewData(bool viewStyle, BsPaymentType model)
        {
            var text = viewStyle ? "全部" : "请选择";
            var item = new SelectListItem() { Text = text, Value = "", Selected = true };

            var onlinePays = new List<SelectListItem>() { item };
            EnumUtil.ToListItem<BasicStatus.支付方式是否网上支付>(ref onlinePays);

            var onlineVisibles = new List<SelectListItem>() { item };
            EnumUtil.ToListItem<BasicStatus.支付方式前台是否可见>(ref onlineVisibles);

            var paymentTypes = new List<SelectListItem>() { item };
            EnumUtil.ToListItem<BasicStatus.支付方式类型>(ref paymentTypes);

            var requiredCardNumbers = new List<SelectListItem>() { item };
            EnumUtil.ToListItem<BasicStatus.是否需要卡号>(ref requiredCardNumbers);

            var statusts = new List<SelectListItem>() { item };
            EnumUtil.ToListItem<BasicStatus.支付方式状态>(ref statusts);
            if (model != null)
            {
                ViewBag.IsOnlinePay = new SelectList(onlinePays, "Value", "Text", model.IsOnlinePay);
                ViewBag.IsOnlineVisible = new SelectList(onlineVisibles, "Value", "Text", model.IsOnlineVisible);
                ViewBag.PaymentType = new SelectList(paymentTypes, "Value", "Text", model.PaymentType);
                ViewBag.RequiredCardNumber = new SelectList(requiredCardNumbers, "Value", "Text", model.RequiredCardNumber);
                ViewBag.Status = new SelectList(statusts, "Value", "Text", model.Status);
            }
            else
            {
                ViewBag.IsOnlinePay = new SelectList(onlinePays, "Value", "Text");
                ViewBag.IsOnlineVisible = new SelectList(onlineVisibles, "Value", "Text");
                ViewBag.PaymentType = new SelectList(paymentTypes, "Value", "Text");
                ViewBag.RequiredCardNumber = new SelectList(requiredCardNumbers, "Value", "Text");
                ViewBag.Status = new SelectList(statusts, "Value", "Text");
            }
        }

        #endregion

        #region 地区和仓库模糊搜索 2013-06-24 何方

        /// <summary>
        /// 地区模糊搜索
        /// </summary>
        /// <param name="keyword">The keyword.</param>
        /// <returns>返回地区信息</returns>
        /// <remarks>
        /// 2013-06-24 何方 创建
        /// </remarks>
        [OutputCache(Duration = 60)]
        public JsonResult Area(string keyword)
        {
            //var areas = BasicAreaBo.Instance.GetAll();

            var result = new Result<IList<BsArea>> { Status = false };
            try
            {

                result.Data = BasicAreaBo.Instance.Search(keyword);

                result.Status = true;
            }
            catch (Exception ex)
            {
                result.Message = "系统异常" + ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 地区仓库模糊搜索
        /// </summary>
        /// <param name="keyword">The keyword.</param>
        /// <returns>返回仓库信息</returns>
        /// <remarks>
        /// 2013-06-24 何方 创建
        /// </remarks>
        public JsonResult Warehouse(string keyword)
        {
            var result = new Result<IList<BsArea>> { Status = false };
            try
            {

                result.Data = BasicAreaBo.Instance.Search(keyword);

                result.Status = true;
            }
            catch (Exception ex)
            {
                result.Message = "系统异常" + ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region  地区信息维护 郑荣华 2013-08-05

        /// <summary>
        /// 地区信息维护主页面
        /// </summary>
        /// <returns>地区信息维护主页面</returns>
        /// <remarks>2013-08-06 郑荣华 创建</remarks>  
        [Privilege(PrivilegeCode.BS1001101)]
        public ActionResult BsArea()
        {
            return View();
        }

        /// <summary>
        /// 获取地区信息
        /// </summary>     
        /// <param name="all">是否是全部节点</param>
        /// <param name="keyword">地区名</param>
        /// <returns>结合ztree树形控件展现</returns>
        /// <remarks>2013-08-06 郑荣华 创建</remarks>  
        [Privilege(PrivilegeCode.BS1001101)]
        public JsonResult GetBsAreaZTreeList(bool all = false, string keyword = "")
        {
            var list = BasicAreaBo.Instance.GetAll();
            if (!all) //只要省市的节点
                list = list.Where(item => item.AreaLevel != 3).ToList();
            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.ToLower(); //全部小写
                //子节点
                list = BasicAreaBo.Instance.QueryAreas(keyword, list);

            }
            //通过Linq生产zTree节点结果集
            var nodes = from c in list
                        select new
                        {
                            id = c.SysNo
                               ,
                            name = c.AreaName
                               ,
                            title = c.AreaName
                                ,
                            open = false
                               ,
                            pId = c.ParentSysNo
                                ,
                            status = c.Status,
                        };

            //返回json数组
            return Json(nodes.ToList(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 地区列表选择器
        /// </summary>
        /// <param name="isMultipleSelect">分类选择是否是多选</param>
        /// <param name="width">选择器宽度</param>
        /// <param name="height">选择器高度</param>
        /// <param name="isAll">是否包括省市区，为false只包括省市</param>
        /// <param name="onlyLeaftSelect">只能选择叶子节点</param>
        /// <returns>地区列表视图</returns>
        /// <remarks>2013-08-06 郑荣华 创建</remarks>   
        [Privilege(PrivilegeCode.BS1001101)]
        public ActionResult AreaSelector(bool? isMultipleSelect, int? width, int? height, bool isAll = true,
                                                    bool onlyLeaftSelect = true)
        {
            ViewBag.IsMultipleSelect = isMultipleSelect.HasValue && isMultipleSelect.Value;
            ViewBag.Width = width.HasValue ? width.Value : 290;
            ViewBag.Height = height.HasValue ? height.Value : 290;
            ViewBag.OnlyLeaftSelect = onlyLeaftSelect;
            ViewBag.IsAll = isAll;

            return View();
        }

        /// <summary>
        /// 地区树分部视图
        /// </summary>
        /// <returns>地区树分部视图</returns>
        /// <remarks>余勇 2014-08-13 创建</remarks>
        [Privilege(PrivilegeCode.BS1001101)]
        public PartialViewResult AreaTree()
        {
            return PartialView();
        }

        /// <summary>
        /// 更新地区信息,只更新了，编码，区名称，父级sysno
        /// </summary>
        /// <param name="model">地区实体</param>
        /// <returns>地区信息</returns>
        /// <remarks> 
        /// 2013-08-05 郑荣华 创建
        /// </remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.BS1001201)]
        public JsonResult UpdateBsArea(BsArea model)
        {
            var success = false;
            string msg = null;

            if (model != null)
            {
                model.LastUpdateBy = CurrentUser.Base.SysNo;
                success = BasicAreaBo.Instance.Update(model);
            }
            else
            {
                msg = "请正确输入地区基本信息";
            }

            return Json(new { success = success, msg = msg }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 检查地区名称重名
        /// </summary>
        /// <param name="areaName">地区名称</param>
        /// <returns>成功true,失败false</returns>
        /// <remarks> 
        /// 2013-08-05 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.BS1001201)]
        public JsonResult IsCanAddArea(string areaName)
        {
            return Json(BasicAreaBo.Instance.IsCanAddArea(areaName),
                         JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 新增区
        /// </summary>
        /// <param name="model">地区实体</param>
        /// <returns>新增的区系统编号</returns>
        /// <remarks> 
        /// 2013-08-05 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.BS1001201)]
        public JsonResult CreateBsArea(BsArea model)
        {
            var success = false;
            string msg = null;
            var sysNo = 0;
            if (model != null)
            {
                BsArea parentmodel = BasicAreaBo.Instance.GetArea(model.ParentSysNo);

                if (parentmodel == null)
                {
                    model.AreaLevel = 0;
                    model.DisplayOrder = 0;
                }
                else
                {
                    model.AreaLevel = parentmodel.AreaLevel + 1; 
                }
                char[] sep = { '（', '(' };
                model.NameAcronym = CHS2PinYin.Convert(model.AreaName.Split(sep)[0], false); //汉字转拼音，只保留（）以前的
                model.Status = (int)BasicStatus.地区状态.有效;              
                model.CreatedBy = CurrentUser.Base.SysNo;
                model.CreatedDate = DateTime.Now;
                model.LastUpdateBy = CurrentUser.Base.SysNo;
                model.LastUpdateDate = DateTime.Now;
                sysNo = BasicAreaBo.Instance.Create(model);
                success = sysNo > 0;
            }
            else
            {
                msg = "请正确输入地区基本信息";
            }
            return Json(new { success = success, msg = msg, newid = sysNo }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 改变地区信息状态
        /// </summary>
        /// <param name="sysNo">地区信息系统编号</param>
        /// <param name="enabled">地区信息状态</param>
        /// <returns>成功true,失败false</returns>
        /// <remarks> 
        /// 2013-08-05 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.BS1001201)]
        public JsonResult ChangeAreaStatus(int sysNo, bool enabled)
        {
            var success = false;

            //判断地区信息是否有效
            if (sysNo > 0)
            {
                var status = BasicStatus.地区状态.有效;
                if (!enabled)
                    status = BasicStatus.地区状态.无效;

                success = BasicAreaBo.Instance.ChangeAreaStatus(sysNo, status,
                                                            CurrentUser.Base.SysNo);
            }

            return Json(success, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取地区基本信息
        /// </summary>
        /// <param name="sysNo">地区信息系统编号</param>
        /// <returns>地区基本信息</returns>
        /// <remarks> 
        /// 2013-08-05 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.BS1001101)]
        public JsonResult GetArea(int sysNo)
        {
            var model = BasicAreaBo.Instance.GetCbArea(sysNo);
            model.AreaLevelName = ((BasicStatus.地区级别)model.AreaLevel).ToString();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 交换显示地区的显示排序
        /// </summary>
        /// <param name="originalSysNo">交换源对象系统编号</param>
        /// <param name="objectiveSysNo">要进行位置交换的目标对象系统编号</param>
        /// <returns>返回： true 操作成功  false 操作失败</returns>
        /// <remarks>注意：该方法值适用于在同一父级中进行移动变更</remarks>
        /// <remarks> 
        /// 2013-08-05 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.BS1001201)]
        public JsonResult AreaSwapDisplayOrder(int originalSysNo, int objectiveSysNo)
        {
            string msg = "";
            bool success = false;

            if (originalSysNo <= 0 || objectiveSysNo <= 0)
            {
                msg = "请指定要进行位置交换的目标地区和原地区";
            }
            else
            {
                success = BasicAreaBo.Instance.AreaSwapDisplayOrder(originalSysNo, objectiveSysNo,CurrentUser.Base.SysNo);
            }

            return Json(new { success = success, msg = msg }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取所有子级地区
        /// </summary>
        /// <param name="parentSysNo">父级地区编号</param>
        /// <returns>所有子级地区</returns>
        /// <remarks> 
        /// 2013-08-12 郑荣华 创建 百城当日达也使用
        /// </remarks>
        [Privilege(PrivilegeCode.BS1001101, PrivilegeCode.LG1002101)]
        public JsonResult GetAreaList(int parentSysNo)
        {
            var model = BasicAreaBo.Instance.GetAreaList(parentSysNo);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据地区名称模糊查询，取第一条
        /// </summary>
        /// <param name="areaName">地区名称</param>
        /// <returns>地区信息</returns>
        /// <remarks> 
        /// 2013-11-30 郑荣华 创建 百城当日达范围设置使用
        /// </remarks>
        [Privilege(PrivilegeCode.BS1001101, PrivilegeCode.LG1002101)]
        public JsonResult GetAreaByName(string areaName)
        {
            var model = BasicAreaBo.Instance.GetArea(areaName);
            var ret = new BsArea();//初始化ParentSysNo=0
            if (model != null && model.Count > 0)
            {
                ret = model[0];
            }
            return Json(ret, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 配送方式支付方式关联维护 郑荣华 2013-08-09

        #region 列表页面

        /// <summary>
        /// 配送方式支付方式关联列表页面
        /// </summary>      
        /// <param name="id">页码</param>
        /// <param name="filter">配送方式支付方式关联查询筛选字段</param>
        /// <returns>配送方式支付方式关联列表页面</returns>
        /// <remarks> 
        /// 2013-08-08 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.BS1002101)]
        public ActionResult BsDeliveryPayment(int? id, ParaBsDeliveryPaymentFilter filter)
        {
            if (Request.IsAjaxRequest())
            {
                //列表分页开始
                var model = new PagedList<CBBsDeliveryPayment>();

                var modelRef = new Pager<CBBsDeliveryPayment> { CurrentPage = id ?? 1, PageSize = model.PageSize };
                BsDeliveryPaymentBo.Instance.GetListByPayment(ref modelRef, filter);
                model.TotalItemCount = modelRef.TotalRows;
                model.TData = modelRef.Rows;
                model.CurrentPageIndex = modelRef.CurrentPage;
                return PartialView("_AjaxPagerDeliveryPayment", model);
            }
            //传递第一级配送方式,即配送类型
            ViewBag.FirstType = DeliveryTypeBo.Instance.GetSubLgDeliveryTypeList(0);
            //支付方式
            ViewBag.BsPaymentType = PaymentTypeBo.Instance.GetAll();
            return View();
        }

        /// <summary>
        /// 删除配送方式支付方式关联
        /// </summary>
        /// <param name="sysNo">要删除的配送方式支付方式关联系统编号</param>
        /// <returns>是否删除成功</returns>
        /// <remarks> 
        /// 2013-08-08 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.BS1002401)]
        public bool DeleteDeliveryPayment(string sysNo)
        {
            var listSysNo = Array.ConvertAll(sysNo.Split(','), int.Parse).ToList();
            foreach (var item in listSysNo)
            {
                BsDeliveryPaymentBo.Instance.Delete(item);
            }
            return true;
        }

        #endregion

        #region 配置页面

        /// <summary>
        /// 配送方式支付方式关联维护配置界面
        /// </summary>
        /// <returns>配送方式支付方式关联维护配置界面</returns>
        /// <remarks> 
        /// 2013-08-08 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.BS1002201)]
        public ActionResult BsDeliveryPaymentSet()
        {

            return View();
        }

        /// <summary>
        /// 添加配送方式支付方式关联
        /// </summary>
        /// <param name="paymentSysNo">支付方式系统编号</param>
        /// <param name="deliverySysNo">配送方式系统编号组(2,3,4,5,6)</param>
        /// <returns>是否成功</returns>
        /// <remarks> 
        /// 2013-08-08 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.BS1002201)]
        public bool CreateDeliveryPayment(int paymentSysNo, string deliverySysNo)
        {
            if (deliverySysNo == "")
            {
                deliverySysNo = "0";
            }
            var dtSysNo = deliverySysNo.Split(',');

            var model = new BsDeliveryPayment
                {
                    CreatedBy = CurrentUser.Base.SysNo,
                    CreatedDate = DateTime.Now,
                    LastUpdateBy = CurrentUser.Base.SysNo,
                    LastUpdateDate = DateTime.Now,
                    PaymentSysNo = paymentSysNo
                };

            var list = Array.ConvertAll(dtSysNo, int.Parse).ToList();//最终结果
            var listHadOwn = BsDeliveryPaymentBo.Instance.GetListByPayment(paymentSysNo).Select(p => p.DeliverySysNo).ToList();//当前情况

            var listDel = listHadOwn.Except(list).ToList();//要删除的
            var listAdd = list.Except(listHadOwn).ToList();//要添加的
            if (deliverySysNo == "0")
            {
                listAdd.Remove(0);
            }
            //// 事务，已经有的不插入、多余的要删除
            //var options = new TransactionOptions
            //    {
            //        IsolationLevel = IsolationLevel.ReadCommitted,
            //        Timeout = TransactionManager.DefaultTimeout
            //    };
            //using (var scope = new TransactionScope(TransactionScopeOption.Required, options))
            //{
            try
            {
                foreach (var item in listAdd)//添加
                {
                    model.DeliverySysNo = item;
                    BsDeliveryPaymentBo.Instance.Create(model);
                }
                foreach (var item in listDel)//删除
                {
                    BsDeliveryPaymentBo.Instance.Delete(paymentSysNo, item);
                }
                //scope.Complete();
                return true;
            }
            catch (Exception ex)
            {
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "配送方式支付方式关联维护:" + ex.Message,
                                         LogStatus.系统日志目标类型.配送方式支付方式关联, paymentSysNo, ex);
                return false;
            }

            //}

        }

        /// <summary>
        /// 根据支付方式查询配送方式支付方式关联列表信息
        /// </summary>
        /// <param name="paymentSysNo">支付方式系统编号</param>
        /// <returns>配送方式支付方式关联列表信息</returns>
        /// <remarks> 
        /// 2013-08-08 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.BS1002201)]
        public JsonResult GetDeliveryPayment(int paymentSysNo)
        {
            var list = BsDeliveryPaymentBo.Instance.GetListByPayment(paymentSysNo);
            var rArray = list.Select(x => x.DeliverySysNo).ToArray();
            return Json("," + string.Join(",", rArray) + ",", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取支付方式
        /// </summary>     
        /// <returns>结合ztree树形控件展现</returns>
        /// <remarks>2013-08-06 郑荣华 创建</remarks>   
        [Privilege(PrivilegeCode.BS1002201)]
        public JsonResult GetPaymentZTreeList()
        {
            var list = PaymentTypeBo.Instance.GetAll();
            list = list.Where(p => p.Status == (int)BasicStatus.支付方式状态.启用).ToList();
            //模拟添加父级列，支付类型 
            var par1 = new CBBsPaymentType { SysNo = -10, PaymentName = "预付", PaymentType = 0, Status = 1 };

            list.Insert(0, par1);

            var par2 = new CBBsPaymentType { SysNo = -20, PaymentName = "到付", PaymentType = 0, Status = 1 };

            list.Insert(0, par2);
            //通过Linq生产zTree节点结果集
            var nodes = from c in list
                        select new
                            {
                                id = c.SysNo
                                ,
                                name = c.PaymentName
                                ,
                                title = c.PaymentName
                                ,
                                open = false
                                ,
                                pId = -c.PaymentType //加负号，避免冲突
                                ,
                                status = c.Status,
                            };

            //返回json数组
            return Json(nodes.ToList(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取配送方式
        /// </summary>     
        /// <returns>结合ztree树形控件展现</returns>
        /// <remarks>2013-08-06 郑荣华 创建</remarks>   
        [Privilege(PrivilegeCode.BS1002201)]
        public JsonResult GetDeliveryTypeZTreeList()
        {
            var list = DeliveryTypeBo.Instance.GetLgDeliveryTypeList();

            //通过Linq生产zTree节点结果集
            var nodes = from c in list
                        select new
                            {
                                id = c.SysNo
                                ,
                                name = c.DeliveryTypeName
                                ,
                                title = c.DeliveryTypeName
                                ,
                                open = false
                                ,
                                pId = c.ParentSysNo
                                ,
                                status = c.Status,
                            };

            //返回json数组
            return Json(nodes.ToList(), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion


        #region 配送方式管理 廖移凤 2017-11-21

        #endregion

        #region 地区仓库

        #region 仓库覆盖地区增加删除 2013-08-12 周瑜 创建 已注释
        ///// <summary>
        ///// 仓库覆盖地区增加删除
        ///// </summary>
        ///// <param name="areaSysNo">传入的地区系统编号 用","链接</param>
        ///// <param name="warehouseSysNo">仓库系统编号</param>
        ///// <returns>返回结果</returns>
        ///// <remarks>2013-08-12 周瑜 创建</remarks>
        ///// <remarks>2013-11-06 郑荣华 重构 </remarks>
        //[Privilege(PrivilegeCode.WH1002201)]
        //public JsonResult WarehouseAreaSave(string areaSysNo, int warehouseSysNo)
        //{
        //    if (areaSysNo == "")
        //    {
        //        areaSysNo = "0:0";
        //    }
        //    var dtSysNo = areaSysNo.Split(',');

        //    var model = new WhWarehouseArea
        //    {
        //        CreatedBy = CurrentUser.Base.SysNo,
        //        CreatedDate = DateTime.Now,
        //        LastUpdateBy = CurrentUser.Base.SysNo,
        //        LastUpdateDate = DateTime.Now,
        //        WarehouseSysNo = warehouseSysNo,
        //        IsDefault =(int)WarehouseStatus.是否默认仓库.否
        //    };
        //    var dicList = dtSysNo.ToDictionary(item => int.Parse(item.Split(':')[0]), item => int.Parse(item.Split(':')[1]));

        //    var list = dicList.Select(p => p.Key).ToList();

        //    var dicHadOwn = BasicAreaBo.Instance.GetAreaByWarehouse(warehouseSysNo)
        //                                .ToDictionary(item => item.SysNo, item => item.IsDefault);

        //    var listHadOwn = dicHadOwn.Select(p => p.Key).ToList();//当前情况
        //    var listDel = listHadOwn.Except(list).ToList();//要删除的

        //    //要添加的
        //    var dicListAdd = dicList.Where(item => ! listHadOwn.Contains(item.Key)).ToDictionary(item => item.Key, item => item.Value);

        //    if (areaSysNo == "0:0")
        //    {
        //        dicListAdd.Remove(0);
        //    }
        //    //改状态dicList中不一样的，传入的为基准，去掉新加中未设为默认仓库的，去掉交集中状态相同的
        //    var dicListExcept = dicList.Except(dicHadOwn).ToDictionary(item => item.Key, item => item.Value);//去掉交集中状态相同的
        //    var dictemp = dicListAdd.Where(item => item.Value == 0).ToDictionary(item => item.Key, item => item.Value);// 新加中未设为默认仓库的
        //    dicListExcept = dicListExcept.Except(dictemp).ToDictionary(item => item.Key, item => item.Value);
        //    // 事务，已经有的不插入、多余的要删除
        //    var options = new TransactionOptions
        //    {
        //        IsolationLevel = IsolationLevel.ReadCommitted,
        //        Timeout = TransactionManager.DefaultTimeout
        //    };
        //    using (var scope = new TransactionScope(TransactionScopeOption.Required, options))
        //    {
        //        try
        //        {
        //            foreach (var item in dicListAdd)//添加
        //            {
        //                model.AreaSysNo = item.Key;                       
        //                WhWarehouseAreaBo.Instance.Insert(model);
        //            }
        //            foreach (var item in listDel)//删除
        //            {
        //                WhWarehouseAreaBo.Instance.Delete(item, warehouseSysNo);
        //            }
        //            foreach (var item in dicListExcept)//改状态
        //            {
        //                var warehouseArea = new WhWarehouseArea
        //                {
        //                    WarehouseSysNo = warehouseSysNo,
        //                    LastUpdateBy = CurrentUser.Base.SysNo,
        //                    LastUpdateDate = DateTime.Now,
        //                    AreaSysNo = item.Key
        //                };
        //                WhWarehouseAreaBo.Instance.SetDefault(warehouseArea, (WarehouseStatus.是否默认仓库)item.Value);
        //            }
        //            scope.Complete();
        //        }
        //        catch (Exception ex)
        //        {
        //            BLL.Log.TransactionLog.Instance.WriteLog("仓库覆盖地区增加删除", ex.ToString(), CurrentUser.Base.UserName);
        //            return Json(false);
        //        }
        //        finally
        //        {
        //            scope.Dispose();
        //        }
        //    }

        //    return Json(true);

        //}
        #endregion

        /// <summary>
        /// 获取所有地区, 构成地区树
        /// </summary>
        /// <param></param>
        /// <returns>所有地区</returns>
        /// <remarks>2013-08-13 周瑜 创建</remarks>
        [Privilege(PrivilegeCode.WH1002101)]
        public JsonResult GetAreaTree()
        {
            var list = BasicAreaBo.Instance.GetAllAreaForTree().Select(m => new
                {
                    id = m.SysNo,
                    pId = m.ParentSysNo,
                    name = m.AreaName
                });
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据仓库系统编号获取该仓库的覆盖地区
        /// </summary>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <returns>地区;列表</returns>
        /// <remarks>2013-08-13 周瑜 创建</remarks>
        [Privilege(PrivilegeCode.WH1002101)]
        public JsonResult GetWhWarehouseArea(int? warehouseSysNo)
        {
            if (warehouseSysNo == null)
            {
                return null;
            }
            var list = BasicAreaBo.Instance.GetAreaByWarehouse((int)warehouseSysNo);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据地区编号查询覆盖该地区的所有仓库
        /// </summary>
        /// <param name="areaSysNo">地区系统编号</param>
        /// <returns>新增记录的系统编号</returns>
        /// <remarks>2013-08-16 周瑜 创建</remarks>
        [Privilege(PrivilegeCode.WH1001101)]
        public ActionResult GetWarehouseForArea(int areaSysNo)
        {

            var list = WhWarehouseAreaBo.Instance.GetWarehouseForArea(areaSysNo);
            return View("WarehouseAreaSetDefault", list);
        }

        /// <summary>
        /// 获取地区仓库列表视图
        /// </summary>
        /// <param name="id">页码</param>
        /// <param name="areaSysNo">地区系统编号</param>
        /// <returns>返回地区仓库列表视图</returns>
        /// <remarks>2013-08-14 周瑜 创建</remarks>
        [Privilege(PrivilegeCode.WH1001101)]
        public ActionResult WarehouseArea(int? id, int? areaSysNo)
        {
            var pageIndex = id ?? 1;
            const int pageSize = 10;
            if (Request.IsAjaxRequest())
            {
                var list = WhWarehouseBo.Instance.GetWarehouseForArea(areaSysNo, pageIndex, pageSize);
                return PartialView("_AjaxPagerWarehouseAreaList", list);
            }
            return View();
        }

        /// <summary>
        /// 修改地区状态
        /// </summary>
        /// <param name="area">地区实体</param>
        /// <returns>是否成功</returns>
        /// <remarks>2013-08-16 周瑜 创建</remarks>
        /// <remarks>2013-11-05 郑荣华 重构</remarks>
        [Privilege(PrivilegeCode.WH1001301)]
        public JsonResult BsAreaUpdateStatus(BsArea area)
        {
            area.LastUpdateBy = CurrentUser.Base.SysNo;
            area.LastUpdateDate = DateTime.Now;

            return Json(BsAreaBo.Instance.UpdateStatus(area) > 0, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 设置地区默认仓库
        /// </summary>
        /// <param name="whWarehouseArea">地区仓库实体</param>
        /// <returns>返回设置消息</returns>
        /// <remarks>2013-08-16 周瑜 创建</remarks>
        [Privilege(PrivilegeCode.WH1001301)]
        public JsonResult SetAreaDefaultWareouse(WhWarehouseArea whWarehouseArea)
        {
            var result = new Result { StatusCode = -1 };
            try
            {
                whWarehouseArea.LastUpdateBy = CurrentUser.Base.SysNo;
                whWarehouseArea.LastUpdateDate = DateTime.Now;

                WhWarehouseAreaBo.Instance.SetDefault(whWarehouseArea);
                result.Status = true;
                result.StatusCode = 0;
                result.Message = "保存成功.";
            }
            catch (Exception ex)
            {
                result.StatusCode = -1;
                result.Status = false;
                result.Message = ex.Message;
            }
            return Json(result);
        }

        #endregion

        #region 仓库维护
        /// <summary>
        /// 获取仓库列表
        /// </summary>
        /// <param name="id">页索引</param>
        /// <param name="condition">查询条件</param>
        /// <returns>仓库列表</returns>
        /// <remarks>2013-08-05 周瑜 创建</remarks>
        [Privilege(PrivilegeCode.WH1002101)]
        public ActionResult WarehouseMaintenance(int? id, WarehouseSearchCondition condition)
        {
            int pageIndex = id ?? 1;
            const int pageSize = 10;

            var statusList = new List<SelectListItem>
                {
                        new SelectListItem {Text = @"全部", Value = "", Selected = true}
                };
            EnumUtil.ToListItem<WarehouseStatus.仓库状态>(ref statusList);
            ViewData["statusList"] = new SelectList(statusList, "Value", "Text");

            if (Request.IsAjaxRequest())
            {
                condition.Warehouses = CurrentUser.Warehouses;
                condition.IsAllWarehouse = CurrentUser.IsAllWarehouse;
                var data = WhWarehouseBo.Instance.QuickSearch(condition, pageIndex, pageSize);
                return PartialView("_AjaxPagerWarehouseList", data);
            }
            return View();
        }

        /// <summary>
        /// 显示出库操作页面， 用于扫描商品，出库等操作
        /// </summary>
        /// <param name="sysNo">仓库系统编号</param>
        /// <returns>出库单明细列表</returns>
        /// <remarks>2013-08-05 周瑜 创建</remarks>
        [Privilege(PrivilegeCode.WH1002101)]
        public ActionResult WarehouseInfo(int sysNo)
        {
            var warehouse = WhWarehouseBo.Instance.GetWarehouse(sysNo);//仓库信息
            var dealer = DsDealerBo.Instance.GetDsDealerByWarehousSysNo(sysNo);
            ViewBag.DearlName = dealer != null ? dealer.DealerName : "";
            return View("WarehouseInfo", warehouse);
        }

        /// <summary>
        /// 修改仓库信息
        /// </summary>
        /// <param name="sysNo">仓库系统编号</param>
        /// <returns>返回仓库信息分部视图</returns>
        /// <remarks>2013-08-05 周瑜 创建</remarks>
        [Privilege(PrivilegeCode.WH1002201)]
        public ActionResult WarehouseEdit(int? sysNo)
        {
            var warehouse = new CBWhWarehouse();
            var dealerWarehouse = new DsDealerWharehouse();

            if (sysNo.HasValue)
            {
                warehouse = WhWarehouseBo.Instance.GetWarehouse(sysNo.Value);
                var _dealerWarehouse = DsDealerWharehouseBo.Instance.GetByWarehousSysNo(sysNo.Value);
                if (_dealerWarehouse != null)
                {
                    dealerWarehouse = _dealerWarehouse;
                }
            }
            var warehouseTypeList = new List<SelectListItem>();
            EnumUtil.ToListItem<WarehouseStatus.仓库类型>(ref warehouseTypeList);
            ViewData["warehouseTypeList"] = new SelectList(warehouseTypeList, "Value", "Text", warehouse.WarehouseType);

            var statusList = new List<SelectListItem>();
            EnumUtil.ToListItem<WarehouseStatus.仓库状态>(ref statusList);
            ViewData["statusList"] = new SelectList(statusList, "Value", "Text", warehouse.Status);

            var dsList = new List<SelectListItem>();
            dsList.Add(new SelectListItem
            {
                Text = @"--请选择--",
                Value = "0",
            });
            dsList.AddRange(from l in DsDealerBo.Instance.GetDsDealerList().Where(x => x.Status == 1).OrderBy(x => x.DealerName).ToList()
                            select new SelectListItem
                            {
                                Value = l.SysNo.ToString(CultureInfo.InvariantCulture),
                                Text = l.DealerName
                            });
            ViewData["dsDealerList"] = new SelectList(dsList, "Value", "Text", dealerWarehouse.DealerSysNo);
            ViewBag.DealerSysNo = dealerWarehouse.DealerSysNo;
            return View("WarehouseEdit", warehouse);
        }

        #region 新增仓库 2013-08-05 周瑜 创建 已注释

        //[Privilege(PrivilegeCode.WH1002201)]
        //public JsonResult WarehouseInsert(WhWarehouse warehouse)
        //{
        //    var result = new Result { StatusCode = -1 };
        //    try
        //    {
        //        warehouse.CreatedBy = CurrentUser.Base.SysNo;
        //        warehouse.CreatedDate = DateTime.Now;
        //        //检查是否有重复的仓库名
        //        var isExists = WhWarehouseBo.Instance.QuickSearch(new WarehouseSearchCondition
        //            {
        //                WarehouseName = warehouse.WarehouseName
        //            }, 1, Int32.MaxValue);
        //        if (isExists.TotalItemCount > 0)
        //        {
        //            result.Status = false;
        //            result.StatusCode = -1;
        //            result.Message = "该仓库名已经存在了, 请修改.";
        //            return Json(result);
        //        }

        //        WhWarehouseBo.Instance.Insert(warehouse);
        //        result.Status = true;
        //        result.StatusCode = 0;
        //        result.Message = "保存成功.";
        //    }
        //    catch (Exception ex)
        //    {
        //        result.StatusCode = -1;
        //        result.Status = false;
        //        result.Message = ex.Message;
        //    }
        //    return Json(result);
        //}
        #endregion

        /// <summary>
        /// 新增仓库
        /// </summary>
        /// <returns></returns>
        /// <remarks>2013-08-05 周瑜 创建</remarks>
        /// <summary>
        /// 新增仓库
        /// </summary>
        /// <param name="warehouse">地区仓库实体</param>
        /// <param name="areaSysNos">区域系统编号</param>
        /// <param name="dealerSysNo">分销商编号</param>
        /// <returns>返回json数据</returns>
        /// <remarks>2013-08-05 周瑜 创建</remarks>
        /// <remarks>2013-11-21 沈强 重构 </remarks>
        [Privilege(PrivilegeCode.WH1002201)]
        public JsonResult WarehouseInsert(WhWarehouse warehouse, string areaSysNos, int dealerSysNo)
        {
            var result = new Result { StatusCode = -1 };

            warehouse.CreatedBy = CurrentUser.Base.SysNo;
            //检查是否有重复的仓库名
            //var isExists = WhWarehouseBo.Instance.QuickSearch(new WarehouseSearchCondition
            //{
            //    WarehouseName = warehouse.WarehouseName
            //}, 1, Int32.MaxValue);
            
            var isExist = WhWarehouseBo.Instance.CheckWarehouseName(new WarehouseSearchCondition { WarehouseName = warehouse.WarehouseName });
            if (isExist == true)
            {
                result.Status = false;
                result.StatusCode = -1;
                result.Message = "该仓库名已经存在了, 请修改.";
                return Json(result);
            }
            try
            {
                warehouse.LastUpdateDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                int warehouseSysNo = WhWarehouseBo.Instance.Insert(warehouse);
                WhWarehouseAreaBo.Instance.SaveWarehouseArea(areaSysNos, warehouseSysNo, CurrentUser.Base.SysNo);
                if (dealerSysNo > 0)
                {
                    DsDealerWharehouseBo.Instance.Insert(new DsDealerWharehouse
                    {
                        DealerSysNo = dealerSysNo,
                        WarehouseSysNo = warehouseSysNo
                    });
                }
                result.Status = true;
                result.StatusCode = 0;
                result.Message = "保存成功.";

            }
            catch (Exception ex)
            {
                result.StatusCode = -1;
                result.Status = false;
                result.Message = ex.Message;
            }
            return Json(result);
        }

        /// <summary>
        /// 仓库定位地图
        /// </summary>
        /// <param name="address">具体地址</param>
        /// <param name="city">仓库所在城市</param>
        /// <returns>地图</returns>
        /// <remarks>2013-10-25 郑荣华 创建</remarks>
        [Privilege(PrivilegeCode.WH1002301, PrivilegeCode.WH1002201)]
        public ActionResult WarehouseMap(string address, string city)
        {
            ViewBag.Address = address;
            ViewBag.City = city;
            return View();
        }

        #region 修改仓库的ajax操作 2013-08-05 周瑜 创建 已注释

        //[Privilege(PrivilegeCode.WH1002301)]
        //public JsonResult WarehouseUpdate(WhWarehouse warehouse)
        //{
        //    var result = new Result { StatusCode = -1 };
        //    try
        //    {
        //        warehouse.LastUpdateBy = CurrentUser.Base.SysNo;
        //        warehouse.LastUpdateDate = DateTime.Now;

        //        WhWarehouseBo.Instance.Update(warehouse);
        //        result.Status = true;
        //        result.StatusCode = 0;
        //        result.Message = "保存成功.";
        //    }
        //    catch (Exception ex)
        //    {
        //        result.StatusCode = -1;
        //        result.Status = false;
        //        result.Message = ex.Message;
        //    }
        //    return Json(result);
        //}
        #endregion

        /// <summary>
        /// 修改仓库的ajax操作
        /// </summary>
        /// <returns></returns>
        /// <remarks>2013-08-05 周瑜 创建</remarks>
        /// <summary>
        /// 修改仓库的ajax操作
        /// </summary>
        /// <param name="warehouse">地区仓库实体</param>
        /// <param name="areaSysNos">区域系统编号</param>
        /// <param name="dealerSysNo">分销商编号</param>
        /// <returns>返回json数据</returns>
        /// <remarks>2013-08-05 周瑜 创建</remarks>
        /// <remarks>2013-11-21 沈强 重构 </remarks>
        [Privilege(PrivilegeCode.WH1002301)]
        public JsonResult WarehouseUpdate(WhWarehouse warehouse, string areaSysNos, int dealerSysNo)
        {
            var result = new Result { StatusCode = -1 };
            warehouse.LastUpdateBy = CurrentUser.Base.SysNo;
            warehouse.LastUpdateDate = DateTime.Now;
            try
            {

                WhWarehouseBo.Instance.Update(warehouse);
                WhWarehouseAreaBo.Instance.SaveWarehouseArea(areaSysNos, warehouse.SysNo, CurrentUser.Base.SysNo);
                DsDealerWharehouseBo.Instance.Update(new DsDealerWharehouse
                {
                    DealerSysNo = dealerSysNo,
                    WarehouseSysNo = warehouse.SysNo
                });

                result.Status = true;
                result.StatusCode = 0;
                result.Message = "保存成功.";

            }
            catch (Exception ex)
            {
                result.StatusCode = -1;
                result.Status = false;
                result.Message = ex.Message;
            }
            return Json(result);
        }

        /// <summary>
        /// 修改仓库状态: 禁用/启用
        /// </summary>
        /// <param name="warehouse">仓库实体</param>
        /// <returns>返回json数据</returns>
        /// <remarks>2013-08-05 周瑜 创建</remarks>
        [Privilege(PrivilegeCode.WH1002301)]
        public JsonResult WarehouseUpdateStatus(WhWarehouse warehouse)
        {
            return Json(WhWarehouseBo.Instance.UpdateStatus(warehouse) > 0, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 选择常用文本
        /// <summary>
        /// 选择常用文本
        /// </summary>
        /// <param name="dataurl">数据查询url</param>
        /// <remarks>2013-10-14 余勇 创建</remarks>
        [Privilege(PrivilegeCode.SO1005601, PrivilegeCode.SO1005201)]
        public ActionResult BsSelectText(string dataurl)
        {
            ViewBag.DataUrl = dataurl;
            return View("_BsSelectText");
        }

        /// <summary>
        /// 常用文本列表查询
        /// </summary>
        /// <param name="type">文本类型</param>
        /// <returns>常用文本列表列表</returns>
        /// <remarks>2013-10-14 余勇 创建</remarks>
        [Privilege(PrivilegeCode.SO1005601, PrivilegeCode.SO1005201)]
        public ActionResult DoSelectTextQuery(int type)
        {
            var dataList = new List<BsCode>();
            if (type == Code.门店转快递原因)
            {
                dataList = BsCodeBo.Instance.门店转快递原因();
            }
            else if (type == Code.门店延迟自提原因)
            {
                dataList = BsCodeBo.Instance.门店延迟自提原因();
            }

            var list = new PagedList<BsCode>
            {
                TData = dataList,
                TotalItemCount = dataList.Count
            };
            return PartialView("_BsSelectTextPager", list);
        }

        /// <summary>
        /// 获取常用文本列表
        /// </summary>
        /// <returns>常用文本列表</returns>
        /// <remarks>2014-06-17 余勇 创建</remarks>
        [Privilege(PrivilegeCode.SO1005601, PrivilegeCode.SO1005201, PrivilegeCode.SY1010201)]
        public JsonResult GetCodeList(int type)
        {
            var list = BsCodeBo.Instance.GetCodeList(type).Select(i => new
            {
                text = i.CodeName,
                value = i.SysNo
            });
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 码表管理

        /// <summary>
        /// 码表管理界面
        /// </summary>
        /// <param name="id">分页页码</param>
        /// <returns></returns>
        /// <remarks>2013-10-14 唐永勤 创建</remarks>
        /// <remarks>2013-12-04 周唐炬 重构</remarks>
        [Privilege(PrivilegeCode.BS1005001)]
        public ActionResult BsCode(int? id)
        {
            if (Request.IsAjaxRequest())
            {
                List<BsCode> listCode = null;
                if (id.HasValue)
                {
                    listCode = BsCodeBo.Instance.GetCodeList(id.Value);
                }
                return PartialView("_AjaxPagerBsCodeList", listCode);
            }
            var list = BsCodeBo.Instance.GetCodeList(0);
            return View(list);
        }

        /// <summary>
        /// 删除码表
        /// </summary>
        /// <param name="id">码表系统编号</param>
        /// <returns>结果</returns>
        /// <remarks>2013-12-04 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.BS1005002)]
        public JsonResult RemoveCode(int id)
        {
            var result = new Result() { StatusCode = -1 };
            try
            {

                var t = BsCodeBo.Instance.RemoveCode(id);
                if (t > 0)
                {
                    result.Status = true;
                    result.StatusCode = 0;
                    SysLog.Instance.Info(LogStatus.系统日志来源.后台, "删除码表:" + id, LogStatus.系统日志目标类型.码表, id, CurrentUser.Base.SysNo);
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "删除码表" + ex.Message, LogStatus.系统日志目标类型.码表, CurrentUser.Base.SysNo, ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 码表添加/修改界面
        /// </summary>
        /// <returns>码表编辑界面</returns>
        /// <remarks>2013-10-15 唐永勤 创建</remarks>
        [Privilege(PrivilegeCode.BS1005002)]
        public ActionResult BsCodeCreate()
        {
            BsCode model = new BsCode();
            //初始化时默认为启用状态
            model.Status = (int)BasicStatus.码表状态.启用;
            int sysno = 0;
            int.TryParse(Request.Params["sysno"], out sysno);
            if (sysno > 0)
            {
                model = BsCodeBo.Instance.GetEntity(sysno);
            }
            return View(model);
        }

        /// <summary>
        /// 码表添加/修改功能
        /// </summary>
        /// <param name="model">码表实体</param>
        /// <returns></returns>
        /// <remarks>2013-12-04 周唐炬 重构</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.BS1005002)]
        public ActionResult BsCodeCreate(BsCode model)
        {
            if (ModelState.IsValid)
            {
                model.Status = BasicStatus.码表状态.启用.GetHashCode();
            }
            var result = BsCodeBo.Instance.BsCodeSave(model);
            return Json(result);
        }

        /// <summary>
        /// 更新码表状态
        /// </summary>
        /// <returns>返回成功的行数</returns>
        /// <remarks>2013-06-24 唐永勤 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.BS1005003)]
        public ActionResult UpdateBsCodeStatus()
        {
            int status = 0, sysno = 0, _result = 0;
            int.TryParse(Request.Params["status"], out status);
            int.TryParse(Request.Params["sysno"], out sysno);

            //转化为int防止注入
            if (sysno > 0 && !string.IsNullOrEmpty(Request.Params["status"]))
            {
                _result = BsCodeBo.Instance.UpdateStatus((BasicStatus.码表状态)status, sysno);
            }
            return Json(new { result = _result });
        }

        #endregion

        #region 物流App下载
        /// <summary>
        /// 物流App下载
        /// </summary>
        /// <returns>物流App下载视图</returns>
        /// <param></param>
        /// <remarks>苟志国 创建</remarks>
        /// <remarks>2014-01-07 周唐炬 添加注释.</remarks>
        public ActionResult bsPhyApp()
        {
            return View();
        }
        #endregion

        #region 获取仓库树 周唐炬 2013-12-01
        /// <summary>
        /// 仓库树分部视图
        /// </summary>
        /// <returns>仓库树分部视图</returns>
        /// <param></param>
        /// <remarks>周唐炬 2013-12-01 创建</remarks>
        [Privilege(PrivilegeCode.CM1005804, PrivilegeCode.CM1005808)]
        public PartialViewResult WarehouseTree(int? isSelfSupport = null)
        {
            ViewBag.isSelfSupport = isSelfSupport;
            return PartialView();
        }

        /// <summary>
        /// 根据关键字获取仓库树
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="isSelfSupport">是否自营</param>
        /// <returns>仓库树</returns>
        /// <remarks>周唐炬 2013-12-01 创建</remarks>
        [Privilege(PrivilegeCode.CM1005804, PrivilegeCode.CM1005808)]
        public JsonResult SearchWarehouse(string keyword, int? isSelfSupport = null)
        {
            var list = new List<ZTreeNode>
                    {
                        new ZTreeNode()
                            {
                                pId = 99999,
                                id = 0,
                                name = "全国",
                                open = true,
                                nodetype = 0,
                                nocheck = false
                            }
                    };

            //2014-04-04 朱家宏 修改 未找到结果报错
            var result = WhWarehouseBo.Instance.SearchWharehouseNew(keyword, false, false, isSelfSupport);
            if (result == null)
            {
                list = new List<ZTreeNode>
                {
                    new ZTreeNode()
                    {
                        pId = 0,
                        id = 0,
                        name = "未找到仓库",
                        open = true,
                        nodetype = 0,
                        nocheck = false
                    }
                };
            }
            else
            {
                list.AddRange(result);
            }

            list.ForEach(x =>
            {
                if (x.nocheck)
                {
                    x.nocheck = !x.nocheck;
                }
            });
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 获取组织机构树 周唐炬 2013-12-12
        /// <summary>
        /// 获取组织机构
        /// </summary>
        /// <returns>组织机构视图</returns>
        /// <remarks>2013-09-06 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.BS1004101)]
        public PartialViewResult OrganizationTree()
        {
            return PartialView();
        }

        #endregion

        #region 国家
        /// <summary>
        /// 国家列表查询
        /// </summary>
        /// <returns>国家列表</returns>
        /// <remarks>2015-08-30 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.OR1001)]
        public ActionResult OriginList()
        {
            return View();
        }
        /// <summary>
        /// 分页获取国家
        /// </summary>
        /// <param name="filter">传入的实体参数</param>
        /// <returns>国家列表</returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.OR1001)]
        public ActionResult DoOriginQuery(ParaOriginFilter filter)
        {
            filter.PageSize = 10;
            var pager = OriginBo.Instance.GetOriginList(filter);
            var list = new PagedList<Origin>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };
            return PartialView("_OriginPager", list);
        }
        /// <summary>
        /// 新增国家
        /// </summary>
        /// <param name="id">国家</param>
        /// <returns>视图</returns>
        /// <remarks>2015-08-30 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.OR1001)]
        public ActionResult OriginCreate(int? id)
        {
            Origin model = new Origin();
            if (id.HasValue)
            {
                model = Hyt.BLL.Basic.OriginBo.Instance.GetEntity(id.Value);
            }
            return View(model);
        }

        /// <summary>
        /// 保存国家
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2015-08-30 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.SP1010201)]
        public JsonResult SaveOrigin(Origin model)
        {
            var result = new Result();
            try
            {
                result = OriginBo.Instance.SaveOrigin(model, CurrentUser.Base);
                result.Status = true;
                result.Message = "保存成功！";
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除任务池优先级
        /// </summary>
        /// <param name="id">系统编号</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2015-08-30 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.SP1010201)]
        public JsonResult Delete(int id)
        {
            var result = new Result();
            try
            {
                if (id > 0)
                {
                    result = OriginBo.Instance.Delete(id);
                    result.Status = true;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
