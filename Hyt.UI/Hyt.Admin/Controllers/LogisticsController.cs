using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Hyt.BLL.Authentication;
using Hyt.BLL.Basic;
using Hyt.BLL.Log;
using Hyt.BLL.Logistics;
using Hyt.BLL.RMA;
using Hyt.BLL.Web;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.SystemPredefined;
using Hyt.Model.WorkflowStatus;
using Hyt.Model.Transfer;
using Hyt.BLL.Warehouse;
using Hyt.Util;
using WhWarehouseBo = Hyt.BLL.Warehouse.WhWarehouseBo;
using System.Text.RegularExpressions;
using Hyt.Model.Common;
using Hyt.Model.Generated;
using Hyt.BLL.FreightModule;
using Extra.SMS;
using Hyt.Admin.Models;
using Hyt.Model.DouShabaoModel;
using Hyt.Model.ExpressList;


namespace Hyt.Admin.Controllers
{
    /// <summary>
    /// 物流
    /// </summary>
    public class LogisticsController : BaseController
    {
        //
        // GET: /Logistics/
        private const string _strDropdownlistDef = "请选择";

        #region 配送方式 郑荣华 2013-06-17
        /// <summary>
        /// 配送方式维护主页面
        /// </summary>
        /// <param name="id">页码</param>
        /// <param name="filter">配送方式查询筛选字段</param>
        /// <returns>配送方式维护主页面</returns>
        /// <remarks> 
        /// 2013-06-14 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.LG1001101)]
        public ActionResult DeliveryType(int? id, ParaDeliveryTypeFilter filter)
        {
            if (Request.IsAjaxRequest())
            {
                //列表分页开始
                var model = new PagedList<CBLgDeliveryType>();

                var modelRef = new Pager<CBLgDeliveryType> { CurrentPage = id ?? 1, PageSize = model.PageSize };
                DeliveryTypeBo.Instance.GetLgDeliveryTypeList(ref modelRef, filter);
                model.TotalItemCount = modelRef.TotalRows;
                model.TData = modelRef.Rows;
                model.CurrentPageIndex = modelRef.CurrentPage;
                return PartialView("_AjaxPagerDeliveryType", model);
            }
            //传递第一级配送方式
            ViewBag.FirstType = DeliveryTypeBo.Instance.GetSubLgDeliveryTypeList(0);
            return View();
        }

        /// <summary>
        /// 配送信息新建视图
        /// </summary>
        /// <param name=""></param>
        /// <returns>配送信息新建页面</returns>
        /// <remarks> 
        /// 2013-06-18 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.LG1001201)]
        public ActionResult DeliveryTypeAdd()
        {
            var list = DeliveryTypeBo.Instance.GetSubLgDeliveryTypeList(0);
            return View(list);
        }

        /// <summary>
        /// 配送信息修改页面
        /// </summary>
        /// <param name="sysNo">配送方式系统编号</param>
        /// <returns>配送信息修改页面</returns>
        /// <remarks> 
        /// 2013-06-17 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.LG1001301)]
        public ActionResult DeliveryTypeUpdate(int sysNo)
        {
            //传递第一级配送方式
            ViewBag.FirstType = DeliveryTypeBo.Instance.GetSubLgDeliveryTypeList(0);

            var model = DeliveryTypeBo.Instance.GetDeliveryType(sysNo);
            return View(model);
        }

        /// <summary>
        /// 查看配送信息
        /// </summary>
        /// <param name="sysNo">配送方式系统编号</param>
        /// <returns>配送信息查看页面</returns>
        /// <remarks> 
        /// 2013-06-18 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.LG1001101)]
        public ActionResult DeliveryTypeView(int sysNo)
        {
            var model = DeliveryTypeBo.Instance.GetDeliveryType(sysNo);
            return View(model);
        }

        /// <summary>
        /// Ajax新增配送方式
        /// </summary>
        /// <param name="model">配送方式实体</param>
        /// <returns>
        /// 成功result.StatusCode>0,失败 result.StatusCode=0
        /// </returns>
        /// <remarks> 
        /// 2013-06-18 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.LG1001201)]
        public JsonResult LgDeliveryTypeCreate(LgDeliveryType model)
        {
            model.CreatedBy = CurrentUser.Base.SysNo;
            model.CreatedDate = DateTime.Now;
            model.LastUpdateBy = CurrentUser.Base.SysNo;
            model.LastUpdateDate = DateTime.Now;

            var result = new Result { Message = "", StatusCode = DeliveryTypeBo.Instance.Create(model) };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Ajax修改配送方式
        /// </summary>
        /// <param name="model">配送方式实体</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks> 
        /// 2013-06-18 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.LG1001301)]
        public JsonResult LgDeliveryTypeUpdate(LgDeliveryType model)
        {
            model.LastUpdateBy = CurrentUser.Base.SysNo;
            model.LastUpdateDate = DateTime.Now;

            return Json(DeliveryTypeBo.Instance.Update(model));
        }

        /// <summary>
        /// 通过ajax判断是否已有此配送方式
        /// </summary>
        /// <param name="deliveryTypeName">配送方式名称</param>
        /// <returns>
        /// 已有不允许添加返回false,未有允许添加返回true
        /// </returns>
        /// <remarks> 
        /// 2013-06-14 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.LG1001201)]
        public JsonResult IsCanAddDeliveryTypeName(string deliveryTypeName)
        {
            return Json(!DeliveryTypeBo.Instance.IsExistDeliveryType(deliveryTypeName),
                         JsonRequestBehavior.AllowGet);
            //[OutputCache(Location = System.Web.UI.OutputCacheLocation.None, NoStore = true)]   //清除缓存
        }

        /// <summary>
        /// 除去修改中的配送方式外，是否还存在相同配送方式名称
        /// </summary>
        /// <param name="deliveryTypeName">配送方式名称</param>
        /// <param name="sysNo">正在修改的配送方式系统编号</param>
        /// <returns>
        /// 已有不允许添加返回false,未有允许添加返回true
        /// 原因：验证jquery.validate只根据返回值 true 和false判断
        /// </returns>
        /// <remarks> 
        /// 2013-07-02 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.LG1001301)]
        public JsonResult IsCanModifyDeliveryTypeName(string deliveryTypeName, int sysNo)
        {
            return Json(!DeliveryTypeBo.Instance.IsExistDeliveryType(deliveryTypeName, sysNo),
                        JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 是否可以删除
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>可以返回true,不可以返回false</returns>
        /// <remarks> 
        /// 2013-06-17 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.LG1001401)]
        public JsonResult IsCanDelete(int sysNo)
        {
            return Json(DeliveryTypeBo.Instance.IsCanDelete(sysNo), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除配送方式
        /// </summary>
        /// <param name="sysNo">要删除的配送方式系统编号</param>
        /// <returns>是否删除成功</returns>
        /// <remarks> 
        /// 2013-06-17 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.LG1001401)]
        public bool DeleteDeliveryType(int sysNo)
        {
            return DeliveryTypeBo.Instance.Delete(sysNo);
        }

        /// <summary>
        /// 获取子配送方式
        /// </summary>
        /// <param name="parSysNo">父级配送方式编号</param>
        /// <returns>子配送方式</returns>
        /// <remarks> 
        /// 2013-08-09 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.LG1001101)]
        public JsonResult GetSubLgDeliveryType(int parSysNo)
        {
            return Json(DeliveryTypeBo.Instance.GetSubLgDeliveryTypeList(parSysNo), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取配送方式
        /// </summary>
        /// <param name="id">页码</param>
        /// <param name="filter">查询条件</param>
        /// <returns>配送方式页面</returns>
        /// <remarks> 
        /// 2013-07-09 郑荣华 创建
        /// </remarks>

        public ActionResult SelectDeliveryType(int? id, ParaDeliveryTypeFilter filter)
        {
            if (Request.IsAjaxRequest())
            {
                //列表分页开始
                var model = new PagedList<CBLgDeliveryType> { PageSize = 8 };
                var modelRef = new Pager<CBLgDeliveryType> { CurrentPage = id ?? 1, PageSize = model.PageSize };
                DeliveryTypeBo.Instance.GetLgDeliveryTypeList(ref modelRef, filter);
                model.TotalItemCount = modelRef.TotalRows;
                model.TData = modelRef.Rows;
                model.CurrentPageIndex = modelRef.CurrentPage;
                model.OnComplete = "AjaxOnComplete";
                return PartialView("_SelectDeliveryTypePager", model);
            }
            ViewBag.ParentSysNo = filter.ParentSysNo;
            ViewBag.Status = filter.Status;
            // ViewBag.SysNoFilter = filter.SysNoFilter;
            ViewBag.WareHouseName = WhWarehouseBo.Instance.GetWarehouseEntity(filter.WareHouseSysNo ?? 1).WarehouseName;

            var whFilter = new ParaWhDeliveryTypeFilter { WareHouseSysNo = filter.WareHouseSysNo };
            var list = WhWarehouseBo.Instance.GetLgDeliveryType(whFilter);
            return View("_SelectDeliveryType", list);
        }
        #endregion

        #region  配送员地理位置信息 郑荣华 2013-06-09
        /// <summary>
        /// 配送路径查询
        /// </summary>
        /// <param name=""></param>
        /// <returns>配送路径百度地图页面</returns>
        /// <remarks>
        /// 2013-06-09 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.LG1008101)]
        public ActionResult DeliveryUserHistoryLocation()
        {
            return View();
        }

        /// <summary>
        /// Ajax获取配送路径
        /// </summary>
        /// <param name="deliveryUserNo">配送员编号</param>
        /// <param name="selDate">日期格式“yyyy-MM-dd”</param>
        /// <param name="startTime">开始时间 格式“HH:mm”</param>
        /// <param name="endTime">结束时间 格式“HH:mm”</param>
        /// <returns>配送路径列表</returns>
        /// <remarks>
        /// 2013-06-19 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.LG1008101)]
        public JsonResult GetDeliveryUserHistoryLocation(int deliveryUserNo, string selDate, string startTime, string endTime)
        {
            var dateRange = new DateRange
                {
                    StartTime = DateTime.Parse(selDate + " " + startTime),
                    EndTime = DateTime.Parse(selDate + " " + endTime)
                };

            var list = DeliveryUserLocationBo.Instance.GetLogisticsDeliveryUserLocation(deliveryUserNo, dateRange);

            var json = new JsonResult
                {
                    Data = new { messageCount = list.Count, list = list }
                };
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据时间，仓库，查询在定位信息表中有记录的配送员
        /// </summary>
        /// <param name="whSysNo">仓库系统编号</param>
        /// <param name="selectDate">日期</param>
        /// <returns>配送员信息</returns>
        /// <remarks>
        /// 2013-07-03 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.LG1008101)]
        public JsonResult GetDeliveryUserForMap(int whSysNo, string selectDate)
        {
            var dateRange = new DateRange
                {
                    StartTime = DateTime.Parse(selectDate + " 00:01"),
                    EndTime = DateTime.Parse(selectDate + " 23:59")
                };

            var list = DeliveryUserLocationBo.Instance.GetDeliveryUserForMap(whSysNo, dateRange).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 查询配送单
        /// </summary>
        /// <param name="deliveryUserSysNo">配送员系统编号</param>
        /// <param name="selectDate">日期</param>
        /// <returns>配送单信息</returns>
        /// <remarks>
        /// 2013-07-03 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.LG1008101)]
        public JsonResult GetLgDeliveryForMap(int deliveryUserSysNo, string selectDate)
        {
            var dateRange = new DateRange
                {
                    StartTime = DateTime.Parse(selectDate + " 00:01"),
                    EndTime = DateTime.Parse(selectDate + " 23:59")
                };

            var list = DeliveryUserLocationBo.Instance.GetLgDeliveryForMap(deliveryUserSysNo, dateRange);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 业务员定位查询
        /// </summary>
        /// <param name=""></param>
        /// <returns>百度地图上显示业务员位置定位点页面</returns>
        /// <remarks>
        /// 2013-06-09 郑荣华 创建
        /// </remarks>
        public ActionResult DeliveryUserCurrentLocation()
        {
            return View();
        }

        /// <summary>
        ///  Ajax获取配送员最新位置
        /// </summary>
        /// <param name="deliveryUserSysNo">配送员编号逗号分隔的字符串</param>
        /// <returns>配送员位置列表</returns>
        /// <remarks>
        /// 2013-06-19 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.LG1009101)]
        public JsonResult GetDeliveryUserCurrentLocation(string deliveryUserSysNo)
        {
            if (string.IsNullOrEmpty(deliveryUserSysNo))
            {
                return Json(new JsonResult
                    {
                        Data = new { messageCount = 0, list = (IList<CBLgDeliveryUserLocation>)new List<CBLgDeliveryUserLocation>() }
                    }, JsonRequestBehavior.AllowGet);
            }
            var list = DeliveryUserLocationBo.Instance.GetLgDeliveryUserLastLocation(deliveryUserSysNo);
            var json = new JsonResult
                {
                    Data = new { messageCount = list.Count, list = list }
                };
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 下拉联动:仓库->配送人员
        /// </summary>
        /// <param name="whSysNo">仓库系统编号</param>
        /// <returns>配送员信息Json(list)</returns>
        /// <remarks> 
        /// 2013-06-21 郑荣华 重构
        /// </remarks>
        [Privilege(PrivilegeCode.LG1009101)]
        public JsonResult GetDeliveryUser(int whSysNo)
        {
            var list = WhWarehouseBo.Instance.GetWhDeliveryUser(whSysNo);
            var dic = list.ToDictionary(p => p.SysNo, p => p.UserName);
            return Json(dic.ToList(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 全国配送员最新位置
        /// </summary>
        /// <param></param>
        /// <returns>百度地图上显示业务员位置定位点页面</returns>
        /// <remarks>2014-03-10 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.LG1009101)]
        public ActionResult AllCurrentLocation()
        {
            return View();
        }

        /// <summary>
        /// 全国配送员最新位置
        /// </summary>
        /// <param></param>
        /// <param name="idlist">仓库系统编号列表</param>
        /// <param name="status">状态(0-全部,1-30分钟活动,2-当日活动,3-不在线)</param>
        /// <returns>全国配送员最新位置</returns>
        /// <remarks>2014-03-10 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.LG1009101)]
        public JsonResult GetAllCurrentLocation(List<int> idlist, int status)
        {
            var list = DeliveryUserLocationBo.Instance.AllDeliveryUserLastLocation(idlist, status);
            var data = new JsonResult
            {
                Data = new { messageCount = list.Count, list = list }
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 配送员信用维护 郑荣华 2013-06-20
        /// <summary>
        /// 配送员信用列表页面
        /// </summary>
        /// <param name="id">页码</param>
        /// <param name="filter">配送员信用查询筛选字段</param>
        /// <returns>配送员信用列表页面</returns>
        /// <remarks> 
        /// 2013-06-21 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.LG1007101)]
        public ActionResult DeliveryUserCredit(int? id, ParaDeliveryUserCreditFilter filter)
        {
            if (Request.IsAjaxRequest())
            {
                //列表分页开始
                var model = new PagedList<CBLgDeliveryUserCredit>();

                var modelRef = new Pager<CBLgDeliveryUserCredit> { CurrentPage = id ?? 1, PageSize = model.PageSize };
                DeliveryUserCreditBo.Instance.GetLgDeliveryUserCreditList(ref modelRef, filter);
                model.TotalItemCount = modelRef.TotalRows;
                model.TData = modelRef.Rows;
                model.CurrentPageIndex = modelRef.CurrentPage;
                return PartialView("_AjaxPagerDeliveryUserCredit", model);
            }

            return View();
        }

        /// <summary>
        /// 下拉联动:仓库->未录入信用信息的配送人员 
        /// </summary>
        /// <param name="whSysNo">仓库系统编号</param>
        /// <returns>配送员信息Json(list)</returns>
        /// <remarks> 
        /// 2013-06-21 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.LG1007201)]
        public JsonResult GetWhDeliveryUserDictForCredit(int whSysNo)
        {
            return Json(WhWarehouseBo.Instance.GetWhDeliveryUserDictForCredit(whSysNo).ToList(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 配送员信用新建视图
        /// </summary>
        /// <returns>配送员信用新建页面</returns>
        /// <remarks> 
        /// 2013-06-18 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.LG1007201)]
        public ActionResult DeliveryUserCreditAdd()
        {
            return View();
        }

        /// <summary>
        /// 配送员信用修改页面
        /// </summary>
        /// <param name="deliveryUserSysNo">配送员系统编号</param>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <returns>配送员信用修改页面</returns>
        /// <remarks> 
        /// 2013-06-17 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.LG1007301)]
        public ActionResult DeliveryUserCreditUpdate(int deliveryUserSysNo, int warehouseSysNo)
        {
            var model = DeliveryUserCreditBo.Instance.GetLgDeliveryUserCredit(deliveryUserSysNo, warehouseSysNo);
            return View(model);
        }

        /// <summary>
        /// Ajax新增配送员信用
        /// </summary>
        /// <param name="model">配送员信用实体</param>
        /// <returns>
        /// 成功返回true,失败返回false
        /// </returns>
        /// <remarks> 
        /// 2013-06-18 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.LG1007201)]
        public JsonResult LgDeliveryUserCreditCreate(LgDeliveryUserCredit model)
        {
            model.CreatedBy = CurrentUser.Base.SysNo;
            model.CreatedDate = DateTime.Now;
            model.LastUpdateBy = CurrentUser.Base.SysNo;
            model.LastUpdateDate = DateTime.Now;
            var result = DeliveryUserCreditBo.Instance.Create(model);
            if (result)
            {
                if (model.DeliveryCredit != 0)
                {
                    var log = string.Format("创建配送员 [配送信用额度:{0}],[配送可用额度:{1}]", model.DeliveryCredit, model.RemainingDeliveryCredit);
                    SysLog.Instance.Info(LogStatus.系统日志来源.后台, log, LogStatus.系统日志目标类型.配送员信用额度, model.DeliveryUserSysNo, CurrentUser.Base.SysNo);

                }
                if (model.BorrowingCredit != 0)
                {
                    var log = string.Format("创建配送员 [借货信用额度:{0},[借货可用额度:{1}]", model.BorrowingCredit, model.RemainingBorrowingCredit);
                    SysLog.Instance.Info(LogStatus.系统日志来源.后台, log, LogStatus.系统日志目标类型.配送员信用额度, model.DeliveryUserSysNo, CurrentUser.Base.SysNo);
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Ajax修改配送员信用
        /// </summary>
        /// <param name="model">配送员信用实体</param>
        /// <param name="addDeliveryCredit">增减配送额度</param>
        /// <param name="addBorrowingCredit">增减借货额度</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks> 
        /// 2013-06-18 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.LG1007301)]
        public JsonResult LgDeliveryUserCreditUpdate(LgDeliveryUserCredit model, int addDeliveryCredit, int addBorrowingCredit)
        {
            model.LastUpdateBy = CurrentUser.Base.SysNo;
            model.LastUpdateDate = DateTime.Now;

            if (addDeliveryCredit != 0)
            {
                var log = string.Format("修改配送员配送额度:{0}", addDeliveryCredit);
                SysLog.Instance.Info(LogStatus.系统日志来源.后台, log, LogStatus.系统日志目标类型.配送员信用额度, model.DeliveryUserSysNo, CurrentUser.Base.SysNo);
            }
            if (addBorrowingCredit != 0)
            {
                var log = string.Format("修改配送员借货额度:{0}", addBorrowingCredit);
                SysLog.Instance.Info(LogStatus.系统日志来源.后台, log, LogStatus.系统日志目标类型.配送员信用额度, model.DeliveryUserSysNo, CurrentUser.Base.SysNo);
            }

            return Json(DeliveryUserCreditBo.Instance.Update(model), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Ajax删除配送员信用
        /// </summary>
        /// <param name="deliveryUserSysNo">配送员系统编号</param>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks> 
        /// 2013-06-21 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.LG1007401)]
        public JsonResult LgDeliveryUserCreditDelete(int deliveryUserSysNo, int warehouseSysNo)
        {
            return Json(DeliveryUserCreditBo.Instance.Delete(deliveryUserSysNo, warehouseSysNo), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 配送单维护 沈强 2013-06-13

        /// <summary>
        /// 配送单维护主页面
        /// </summary>
        /// <param name="id">页索引</param>
        /// <returns>配送单维护页面</returns>
        /// <remarks>2013-06-13 沈强 创建</remarks>
        [Privilege(PrivilegeCode.LG1006104)]
        public ActionResult DeliveryNoteMaintain(int? id)
        {
            if (!Request.IsAjaxRequest())
            {
                return View();
            }
            int pageIndex = id ?? 1;

            ParaLogisticsFilter cbLgDelivery = new ParaLogisticsFilter();

            string sysNo = Request.Params["sysno"]; //配送单系统编号
            string stockSysNo = Request.Params["stock_sysno"]; //仓库系统编号
            string deliveryManSysNo = Request.Params["delivery_man_sysno"]; //配送员系统编号
            string status = Request.Params["status"]; //配送单状态
            string orderSysNo = Request.Params["order_sysno"]; //订单系统编号
            string stockOutNo = Request.Params["stock_out_no"]; //（出库单/取件单）号
            string createdName = Request.Params["created_name"]; //创建人姓名
            string createdStartDate = Request.Params["start_date"]; //查询创建日期的开始时间
            string createdEndDdate = Request.Params["end_date"]; //查询创建日期的截至时间

            #region 为实体赋值

            int temp = 0;

            int.TryParse(sysNo, out temp);
            cbLgDelivery.SysNo = temp;

            int.TryParse(stockSysNo, out temp);
            cbLgDelivery.StockSysNo = temp;

            int.TryParse(deliveryManSysNo, out temp);
            cbLgDelivery.DeliveryUserSysNo = temp;

            int.TryParse(status, out temp);
            cbLgDelivery.Status = temp;

            int.TryParse(orderSysNo, out temp);
            cbLgDelivery.SoOrderSysNo = temp;

            cbLgDelivery.StockOutNo = string.IsNullOrEmpty(stockOutNo) ? null : stockOutNo;
            cbLgDelivery.CreatedName = string.IsNullOrEmpty(createdName) ? null : createdName;
            cbLgDelivery.CreatedStartDate = string.IsNullOrEmpty(createdStartDate) ? null : createdStartDate;
            cbLgDelivery.CreatedEndDate = string.IsNullOrEmpty(createdEndDdate) ? null : createdEndDdate;

            #endregion


            Infrastructure.Pager.PagedList<CBLgDelivery> pageList = new Infrastructure.Pager.PagedList<CBLgDelivery>();

            Pager<ParaLogisticsFilter> pagerFilter = new Pager<ParaLogisticsFilter>();
            pagerFilter.Rows.Add(cbLgDelivery);
            pagerFilter.CurrentPage = pageIndex;
            pagerFilter.PageSize = 10;

            var hasAllWarehouse = BLL.Sys.SyUserGroupBo.Instance.IsHasAllWarehouse(CurrentUser.Base.SysNo);
            Pager<CBLgDelivery> pager = BLL.Logistics.LgDeliveryBo.Instance.GetLgDelivery(pagerFilter, CurrentUser.Base.SysNo, hasAllWarehouse);

            pageList.CurrentPageIndex = pager.CurrentPage;
            pageList.PageSize = pager.PageSize;
            pageList.TotalItemCount = pager.TotalRows;
            pageList.TData = pager.Rows;

            return this.PartialView("_DeliveryNoteList", pageList);
        }

        /// <summary>
        /// 将配送单导出为Excel
        /// </summary>
        /// <returns></returns>
        /// <remarks>2013-07-26 沈强 创建</remarks>
        [Privilege(PrivilegeCode.LG1006101)]
        public void ExportExcel()
        {
            ParaLogisticsFilter cbLgDelivery = new ParaLogisticsFilter();

            string sysNo = Request.Params["sysno"];                        //配送单系统编号
            string stockSysNo = Request.Params["stock_sysno"];             //仓库系统编号
            string deliveryManSysNo = Request.Params["delivery_man_sysno"];//配送员系统编号
            string status = Request.Params["status"];                      //配送单状态
            string orderSysNo = Request.Params["order_sysno"];             //订单系统编号
            string stockOutNo = Request.Params["stock_out_no"];            //（出库单/取件单）号
            string createdName = Request.Params["created_name"];           //创建人姓名
            string createdStartDate = Request.Params["start_date"];        //查询创建日期的开始时间
            string createdEndDdate = Request.Params["end_date"];           //查询创建日期的截至时间

            #region 为实体赋值
            int temp = 0;
            int.TryParse(sysNo, out temp);
            cbLgDelivery.SysNo = temp;

            int.TryParse(stockSysNo, out temp);
            cbLgDelivery.StockSysNo = temp;

            int.TryParse(deliveryManSysNo, out temp);
            cbLgDelivery.DeliveryUserSysNo = temp;

            int.TryParse(status, out temp);
            cbLgDelivery.Status = temp;

            int.TryParse(orderSysNo, out temp);
            cbLgDelivery.SoOrderSysNo = temp;

            cbLgDelivery.StockOutNo = string.IsNullOrEmpty(stockOutNo) ? null : stockOutNo;
            cbLgDelivery.CreatedName = string.IsNullOrEmpty(createdName) ? null : createdName;
            cbLgDelivery.CreatedStartDate = string.IsNullOrEmpty(createdStartDate) ? null : createdStartDate;
            cbLgDelivery.CreatedEndDate = string.IsNullOrEmpty(createdEndDdate) ? null : createdEndDdate;
            #endregion

            Pager<ParaLogisticsFilter> pagerFilter = new Pager<ParaLogisticsFilter>();
            pagerFilter.Rows.Add(cbLgDelivery);
            pagerFilter.CurrentPage = 1;
            pagerFilter.PageSize = Int32.MaxValue;

            //try
            //{
            var hasAllWarehouse = BLL.Sys.SyUserGroupBo.Instance.IsHasAllWarehouse(CurrentUser.Base.SysNo);

            Pager<CBLgDelivery> pager = BLL.Logistics.LgDeliveryBo.Instance.GetLgDelivery(pagerFilter, CurrentUser.Base.SysNo, hasAllWarehouse);

            //转换格式
            List<DeliveryExportExcel> excel = pager.Rows.Select(c => new DeliveryExportExcel()
                {
                    SysNo = c.SysNo,
                    DeliveryManName = c.DeliveryManName,
                    CreatedName = c.CreatedName,
                    CreatedDate = c.CreatedDate.ToString("yyyy-MM-dd HH:mm"),
                    PaidNoteCount = c.PaidNoteCount,
                    PaidAmount = c.PaidAmount.ToString("C"),
                    CODNoteCount = c.CODNoteCount,
                    CODAmount = c.CODAmount.ToString("C"),
                    Status = ((LogisticsStatus.配送单状态)c.Status).ToString()
                }).ToList();

            //导出Excel，并设置表头列名
            Util.ExcelUtil.Export<DeliveryExportExcel>(excel, new List<string>()
                    {
                        "配送单号",
                        "配送人/快递",
                        "创建人",
                        "创建日期",
                        "已付款单量",
                        "已付款金额",
                        "货到付款单量",
                        "货到付款金额",
                        "状态"
                    });
            //}
            //catch (Exception e)
            //{
            //Response.Write("<script type='text/javascript'>alert('导出时发生错误!');</script>");
            //Response.End();
            //}

        }

        /// <summary>
        /// 根据出库单查询条件获取出库单列表
        /// </summary>
        /// <param name="id">当前页.</param>
        /// <param name="condtion">过滤列表</param>
        /// <returns>出库单列表</returns>
        /// <remarks>2013-07-03 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.LG1006201)]
        public ActionResult SelectOutStockList(int? id, StockOutSearchCondition condtion)
        {
            if (Request.IsAjaxRequest())
            {
                var currentPage = id ?? 1;
                //查询某仓库待配送的出库单
                condtion.Status = (int)WarehouseStatus.出库单状态.待配送;
                condtion.CurrentPage = currentPage;

                var model = new PagedList() { CurrentPageIndex = currentPage };
                var list = WhWarehouseBo.Instance.SearchFilter(condtion, model.PageSize);

                return PartialView("_AjaxPagerOutStockItems", list);
            }
            ViewBag.WarehouseSysNo = condtion.WarehouseSysNo;
            ViewBag.DeliveryTypeSysNo = condtion.DeliveryTypeSysNo;
            ViewBag.SysNoFilter = condtion.SysNoFilter;
            return View();
        }

        /// <summary>
        /// 创建配送单--在地图上选择出库单
        /// </summary>
        /// <param name="condtion">过滤列表</param>
        /// <returns>在地图上选择出库单界面</returns>
        /// <remarks>2014-03-04 唐文均 创建</remarks>
        [Privilege(PrivilegeCode.LG1006201)]
        public ActionResult SelectOutStockListFromMap(StockOutSearchCondition condtion)
        {
            // 获取送货业务员位置
            string deliveryUserLoc = "0,0", deliveryUserSysNo = Request.QueryString["DeliveryUserSysNo"];
            if (!string.IsNullOrEmpty(deliveryUserSysNo))
            {
                var list = DeliveryUserLocationBo.Instance.GetLgDeliveryUserLastLocation(deliveryUserSysNo);
                if (list.Count > 0)
                {
                    deliveryUserLoc = list[0].Longitude + "," + list[0].Latitude;
                    ViewBag.DeliveryUserName = list[0].UserName + " " + list[0].GpsDate.ToString("yyyy-MM-dd hh:mm");
                }
            }
            else
                ViewBag.DeliveryUserName = "";
            ViewBag.DeliveryUserLoction = deliveryUserLoc;

            // 获取仓库信息
            var warehouse = new CBWhWarehouse();
            if (condtion.WarehouseSysNo.HasValue)
            {
                warehouse = WhWarehouseBo.Instance.GetWarehouse(condtion.WarehouseSysNo.Value);
            }
            ViewBag.SelectedCity = warehouse.CityName;

            // 获取选择仓库的城市的所有门店和仓库
            var wareHouseLoc = new List<Dictionary<string, object>>();
            var storeLoc = new List<Dictionary<string, object>>();

            var areaInfo = BasicAreaBo.Instance.GetCbArea(warehouse.AreaSysNo);     // 获取地区信息
            var areaList = BasicAreaBo.Instance.GetAreaList(areaInfo.ParentSysNo);  // 市下的所有地区
            foreach (var area in areaList)
            {
                // 仓库
                var wareHouseList = WhWarehouseBo.Instance.GetWhWarehouseListByArea(area.SysNo, (int)WarehouseStatus.仓库类型.仓库);
                foreach (var item in wareHouseList)
                {
                    wareHouseLoc.Add(new Dictionary<string, object>
                    {
                        {"Name",item.WarehouseName},
                        {"Longitude",item.Longitude},
                        {"Latitude",item.Latitude}
                    });
                }
                // 门店
                var storeList = WhWarehouseBo.Instance.GetWhWarehouseListByArea(area.SysNo, (int)WarehouseStatus.仓库类型.门店);
                foreach (var item in storeList)
                {
                    storeLoc.Add(new Dictionary<string, object>
                    {
                        {"Name",item.WarehouseName},
                        {"Longitude",item.Longitude},
                        {"Latitude",item.Latitude}
                    });
                }
            }
            ViewBag.WarehouseLoc = new JavaScriptSerializer().Serialize(wareHouseLoc);
            ViewBag.StoreLoc = new JavaScriptSerializer().Serialize(storeLoc);

            ViewBag.WarehouseSysNo = condtion.WarehouseSysNo;
            ViewBag.DeliveryTypeSysNo = condtion.DeliveryTypeSysNo;
            ViewBag.SysNoFilter = condtion.SysNoFilter;
            return View();
        }

        /// <summary>
        /// 根据出库单查询条件获取出库单列表（地图选择出库单）
        /// </summary>
        /// <param name="condtion">过滤列表</param>
        /// <returns>出库单列表</returns>
        /// <remarks>2014-03-04 唐文均 创建</remarks>
        [Privilege(PrivilegeCode.LG1006201)]
        public JsonResult GetOutStockList(StockOutSearchCondition condtion)
        {
            //查询某仓库待配送的出库单
            condtion.Status = (int)WarehouseStatus.出库单状态.待配送;

            var model = new PagedList() { CurrentPageIndex = condtion.CurrentPage };
            var list = WhWarehouseBo.Instance.SearchFilter(condtion, model.PageSize);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取取件单
        /// </summary>
        /// <param name="id">页码</param>
        /// <param name="filter">查询条件</param>
        /// <returns>取件单页面</returns>
        /// <remarks> 
        /// 2013-07-05 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.LG1006201)]
        public ActionResult SelectPickUp(int? id, ParaPickUpFilter filter)
        {
            if (Request.IsAjaxRequest())
            {
                //列表分页开始
                PagedList<CBLgPickUp> model = new PagedList<CBLgPickUp>();

                Pager<CBLgPickUp> modelRef = new Pager<CBLgPickUp>();
                modelRef.CurrentPage = id ?? 1;
                modelRef.PageSize = model.PageSize;
                LgPickUpBo.Instance.GetPickUpList(ref modelRef, filter);
                model.TotalItemCount = modelRef.TotalRows;
                model.TData = modelRef.Rows;
                model.CurrentPageIndex = modelRef.CurrentPage;
                model.OnComplete = "AjaxOnComplete";
                return PartialView("_SelectPickUpPager", model);
            }
            ViewBag.WarehouseSysNo = filter.WarehouseSysNo;
            ViewBag.Status = filter.Status;
            ViewBag.SysNoFilter = filter.SysNoFilter;
            return View("_SelectPickUp");
        }

        #region 配送单明细操作 沈强 2013-06-24

        /// <summary>
        /// 配送单明细
        /// </summary>
        /// <param name="id">页索引</param>
        /// <param name="source">用于检测是否为源页面</param>
        /// <returns>配送单明细页面</returns>
        /// <remarks>2013-06-19 沈强 创建</remarks>
        [Privilege(PrivilegeCode.LG1006104)]
        public ActionResult DeliveryNoteDetails(int id, string source)
        {
            if (HttpContext.Request.Params["DialogOpen"] != null && HttpContext.Request.Params["DialogOpen"] == "true")
            {
                ViewBag.backToList = "false";
            }
            //配送单系统编号
            int sysNo = id;

            if (sysNo == 0)
            {
                throw new Exception("配送单系统编号为零，不能查询此配送单！");
            }

            Infrastructure.Pager.PagedList<object[]> pageList = new Infrastructure.Pager.PagedList<object[]>();

            Pager<CBLgDelivery> pager = new Pager<CBLgDelivery>();
            pager.CurrentPage = 1;
            pager.PageSize = 1;
            CBLgDelivery cbLgDelivery = new CBLgDelivery();
            cbLgDelivery.SysNo = sysNo;
            pager.Rows.Add(cbLgDelivery);

            IList<CBLgDeliveryItem> lgDeliveryItems = null;
            IList<BsPaymentType> bsPaymentType = null;
            CBLgDeliveryType cbLgDeliveryType = null;

            //获取配送单主表信息
            LgDeliveryBo.Instance.GetLgDelivery(ref pager);

            if (!AdminAuthenticationBo.Instance.HasWareHousePrivilege(pager.Rows[0].StockSysNo))
            {

                var message = string.Format("没有权限读取仓库:{0}的信息",
                                            WhWarehouseBo.Instance.GetWarehouseName(pager.Rows[0].StockSysNo));
                return View("ErrorPrivilegeWithMessage", (object)(message));

            }

            //获取配送方式信息
            cbLgDeliveryType = BLL.Logistics.DeliveryTypeBo.Instance.GetDeliveryType(pager.Rows[0].DeliveryTypeSysNo);

            //获取配送单明细子表信息
            lgDeliveryItems = BLL.Logistics.LgDeliveryBo.Instance.GetCbDeliveryItems(sysNo);

            //获取所有有效支付方式类型
            bsPaymentType = BLL.Order.SoOrderBo.Instance.LoadAllPayType().Where(b => b.Status == 1).ToList();

            object[] objs = new object[4];
            objs[0] = pager.Rows[0]; //配送单主表信息
            objs[1] = lgDeliveryItems; //配送单明细子表信息
            objs[2] = bsPaymentType; //所有支付方式类型
            objs[3] = cbLgDeliveryType; //配送方式信息
            pageList.TData = new List<object[]>() { objs };

            ViewBag.source = source;//added  by huangwei for page return judgement

            return View(pageList);
        }

        /// <summary>
        /// 作废配送单
        /// </summary>
        /// <param name="deliverySysNo">配送单系统编号</param>
        /// <returns>操作结果</returns>
        /// <remarks>2013-06-19 沈强 创建</remarks>
        [Privilege(PrivilegeCode.LG1006501)]
        public ActionResult CancelDeliveryNote(int deliverySysNo)
        {
            Result result = null;
            try
            {
                using (var tran = new TransactionScope())
                {

                    result = BLL.Logistics.LgDeliveryBo.Instance.CancelDelivery(deliverySysNo, CurrentUser.Base.SysNo);

                    tran.Complete();
                }
            }
            catch (Exception ex)
            {
                result = new Result();
                result.Status = false;
                result.Message = "发生错误！！";
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, ex.Message, ex);
            }

            return this.Json(result);
        }

        #endregion

        #region 创建配送单操作 沈强 2013-06-25
        /// <summary>
        /// 创建配送单页面
        /// </summary>
        /// <param name=""></param>
        /// <returns>创建配送单页面</returns>
        /// <remarks>2013-06-14 沈强 创建</remarks>
        [Privilege(PrivilegeCode.LG1006201)]
        public ActionResult CreateDeliveryNote()
        {
            return View();
        }

        /// <summary>
        /// 根据配送员系统编号和仓库系统编号获取配送员信用额度信息（Ajax调用）
        /// </summary>
        /// <param name="sysNo">配送员系统编号sysNo</param>
        /// <param name="warehouseSysNo">仓库系统编号sysNo</param>
        /// <returns>配送员信息</returns>
        /// <remarks>2013-06-24 沈强 创建</remarks>
        [Privilege(PrivilegeCode.LG1006201)]
        public ActionResult GetDeliveryUserInfo(int sysNo, int warehouseSysNo)
        {
            //Result<CBLgDeliveryUserCredit> result = new Result<CBLgDeliveryUserCredit>();
            Result<object> result = new Result<object>();
            if (DeliveryUserCreditBo.Instance.IsExistDeliveryUser(sysNo))
            {
                try
                {
                    CBLgDeliveryUserCredit cbLgDeliveryUserCredit = DeliveryUserCreditBo.Instance.GetLgDeliveryUserCredit(sysNo, warehouseSysNo);

                    if (cbLgDeliveryUserCredit != null)
                    {
                        result.Status = true;
                        result.Data = new
                        {
                            RemainingDeliveryCredit = cbLgDeliveryUserCredit.RemainingDeliveryCredit.ToString("C"),
                            Price = cbLgDeliveryUserCredit.RemainingDeliveryCredit
                        };
                    }
                    else
                    {
                        result.Status = false;
                        result.Message = "此配送员还没有在此仓库下面配置信用额度！！";
                    }
                }
                catch (Exception ex)
                {
                    result.Status = false;
                    result.Message = "发生错误！！";
                    SysLog.Instance.Error(LogStatus.系统日志来源.后台, ex.Message, ex);
                }
            }
            else
            {
                result.Status = false;
                result.Message = "此配送员还没有添加信用额度信息！";
            }
            return Json(result);
        }

        #region 添加配送单



        /// <summary>
        /// 批量创建配送单
        /// </summary>
        /// <returns></returns>
        /// <remarks>2017-07-03 杨浩 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.LG1006201)]
        public JsonResult BatchCreateLgDelivery(List<BatchCreateLgDeliveryMdl> models)
        {
            var result = new Result<int>() { Message = "成功" };
            foreach (var note in models)
            {

                result = BLL.Logistics.DeliveryTypeBo.Instance.BatchCreateLgDelivery(note.WarehouseSysNo, note.DeliverTypeSysno, note.NoteType, note.NoteSysNo, note.ExpressNo);
                if (!result.Status)
                {
                    return Json(result);
                }

                #region 注释将代码转移到 BLL.Logistics.DeliveryTypeBo.Instance.BatchCreateLgDelivery
                //var itemList = new List<LgDeliveryItem>();
                //string NeedInStock = string.Empty;

                //if (string.IsNullOrWhiteSpace(note.ExpressNo))
                //{
                //    result.Status = false;
                //    result.Message = "配送单明细数据错误,不能创建配送单";
                //    return Json(result);
                //}

                //#region 判断快递单号是否重复
                //if (!string.IsNullOrEmpty(note.ExpressNo) && note.NoteType == (int)LogisticsStatus.配送单据类型.出库单)
                //{
                //    var flg = Hyt.BLL.Logistics.LgDeliveryBo.Instance.IsExistsExpressNo(note.DeliverTypeSysno, note.ExpressNo);
                //    if (flg)
                //    {
                //        result.Status = false;
                //        result.Message = "快递单号" + note.ExpressNo + "已经被使用，请更换快递单号";
                //        return Json(result);
                //    }
                //}
                //#endregion

                //#region 配送单作废会生成出库单对应的入库单，再次将此入库单加入配送,需检查此入库单是否已经完成入库

                //if (note.NoteType == (int)LogisticsStatus.配送单据类型.出库单)
                //{
                //    var rr = Hyt.BLL.Logistics.LgDeliveryBo.Instance.CheckInStock(note.NoteSysNo);
                //    if (rr.Status)
                //    {
                //        if (!string.IsNullOrEmpty(NeedInStock))
                //        {
                //            NeedInStock += ",";
                //        }
                //        NeedInStock += rr.StatusCode;
                //    }
                //}

                //#endregion

                //itemList.Add(new LgDeliveryItem()
                //{
                //    DeliverySysNo = note.DeliverTypeSysno,
                //    ExpressNo = note.ExpressNo,
                //    NoteType = note.NoteType,
                //    NoteSysNo = note.NoteSysNo
                //});

                //if (!string.IsNullOrEmpty(NeedInStock))//未入库的单子
                //{
                //    result.Status = false;
                //    result.Message = "请将先前配送单作废，拒收，未送达生成的入库单(" + NeedInStock + ")完成入库";
                //    return Json(result);
                //}

                //var options = new TransactionOptions
                //{
                //    IsolationLevel = IsolationLevel.ReadCommitted,
                //    Timeout = TransactionManager.DefaultTimeout
                //};

                ////配送方式  
                //var delivertType = DeliveryTypeBo.Instance.GetDeliveryType(note.DeliverTypeSysno);
                //int deliverySysno;
                //var deliveryMsgs = new List<Hyt.BLL.Logistics.LgDeliveryBo.DeliveryMsg>();

                //using (var tran = new TransactionScope(TransactionScopeOption.Required, options))
                //{
                //    deliverySysno = LgDeliveryBo.Instance.NewCreateLgDelivery(note.WarehouseSysNo, -1,
                //        delivertType,
                //        CurrentUser.Base.SysNo,itemList,true,ref deliveryMsgs, Request.ServerVariables["REMOTE_ADDR"]);


                //    //回填物流信息
                //    try
                //    {
                //        LgDeliveryBo.Instance.BackFillLogisticsInfo(deliverySysno, note.DeliverTypeSysno);
                //    }
                //    catch (Exception ex)
                //    {
                //        //Hyt.BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.分销工具, ex.Message, ex);
                //    }


                //    result.Status = true;
                //    result.Data = deliverySysno;
                //    result.Message = "确认发货完成";
                //    tran.Complete();
                //}
                #endregion
            }

            return Json(result);
        }

        /// <summary>
        /// 配送单确认发货（Ajax调用）
        /// </summary>
        ///<param name="warehouseSysNo">仓库系统编号</param>
        /// <param name="deliveryUserSysNo">配送员系统编号.</param>
        /// <param name="deliverTypeSysno">配送方式系统编号.</param>
        /// <param name="items">配送单明细 (单据类型,单据编号,快递单号)</param>
        /// <param name="isForce">是否允许超出配送信用额度配送 </param>
        /// <returns>返回Result</returns>
        /// <remarks>2013-06-27 沈强 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.LG1006201)]
        public JsonResult ConfrimDelivery(int warehouseSysNo, int deliveryUserSysNo, int deliverTypeSysno, string[] items, bool isForce)
        {
            var result = new Result<int>();
            try
            {

                var itemList = new List<LgDeliveryItem> { };
                string NeedInStock = string.Empty;
                foreach (var note in items.Select(item => item.Split(',')))
                {
                    if (note.Length < 2)
                    {
                        result.Message = "配送单明细数据错误,不能创建配送单";
                    }
                    LgDeliveryItem item = new LgDeliveryItem
                    {
                        NoteType = int.Parse(note[0]),
                        NoteSysNo = int.Parse(note[1]),
                        ExpressNo = note.Length >= 3 ? note[2].Trim() : ""
                    };

                    #region 判断快递单号是否重复(2014-04-11 朱成果)
                    if (!string.IsNullOrEmpty(item.ExpressNo) && item.NoteType == (int)LogisticsStatus.配送单据类型.出库单)
                    {
                        var flg = Hyt.BLL.Logistics.LgDeliveryBo.Instance.IsExistsExpressNo(deliverTypeSysno, item.ExpressNo);
                        if (flg)
                        {
                            result.Status = false;
                            result.Message = "快递单号" + item.ExpressNo + "已经被使用，请更换快递单号";
                            return Json(result);
                        }
                    }
                    #endregion

                    #region 配送单作废会生成出库单对应的入库单，再次将此入库单加入配送,需检查此入库单是否已经完成入库(2014-04-11 朱成果)

                    if (item.NoteType == (int)LogisticsStatus.配送单据类型.出库单)
                    {
                        var rr = Hyt.BLL.Logistics.LgDeliveryBo.Instance.CheckInStock(item.NoteSysNo);
                        if (rr.Status)
                        {
                            if (!string.IsNullOrEmpty(NeedInStock))
                            {
                                NeedInStock += ",";
                            }
                            NeedInStock += rr.StatusCode;
                        }
                    }

                    #endregion

                    itemList.Add(item);
                }
                if (!string.IsNullOrEmpty(NeedInStock)) //未入库的单子
                {
                    result.Status = false;
                    result.Message = "请将先前配送单作废，拒收，未送达生成的入库单(" + NeedInStock + ")完成入库";
                    return Json(result);
                }
                var options = new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadCommitted,
                    Timeout = TransactionManager.DefaultTimeout
                };

                //配送方式  
                var delivertType = DeliveryTypeBo.Instance.GetDeliveryType(deliverTypeSysno);

                int deliverySysno;
                var deliveryMsgs = new List<Hyt.BLL.Logistics.LgDeliveryBo.DeliveryMsg>();

                using (var tran = new TransactionScope(TransactionScopeOption.Required, options))
                {
                    //deliverySysno = LgDeliveryBo.Instance.CreateLgDelivery(warehouseSysNo, deliveryUserSysNo,
                    //    delivertType,
                    //    CurrentUser.Base.SysNo, itemList, isForce, Request.ServerVariables["REMOTE_ADDR"]);

                    deliverySysno = LgDeliveryBo.Instance.NewCreateLgDelivery(warehouseSysNo, deliveryUserSysNo,
                        delivertType,
                        CurrentUser.Base.SysNo, itemList, isForce, ref deliveryMsgs, Request.ServerVariables["REMOTE_ADDR"]);

                    result.Status = true;
                    result.Data = deliverySysno;
                    result.Message = "确认发货完成";
                    foreach (var note in items.Select(item => item.Split(',')))
                    {
                        if (note.Length < 2)
                        {
                            result.Message = "配送单明细数据错误,不能创建配送单";
                        }
                        LgDeliveryItem item = new LgDeliveryItem
                        {
                            NoteType = int.Parse(note[0]),
                            NoteSysNo = int.Parse(note[1]),
                            ExpressNo = note.Length >= 3 ? note[2].Trim() : ""
                        };

                        int orderID = Convert.ToInt32(DeliveryTypeBo.Instance.GetorderId(item.NoteSysNo)); //获取订单编号
                        var expensesAmount = Hyt.BLL.Order.SoOrderOtherExpensesBo.Instance.GetExpensesFee(orderID);
                        if (expensesAmount > 0)
                        {
                            DouShabaoOrder(orderID);
                        }
                    }
                    tran.Complete();
                }

                //2014-05-09 黄波/何永东/杨文兵 添加
                try
                {
                    #region 发送相关短消息
                    //发送相关消息
                    foreach (var msg in deliveryMsgs)
                    {
                        //Order.SoOrderBo.Instance.WriteSoTransactionLog(msg.StockOutTransactionSysNo,
                        //                                                      "出库单" + msg.StockOutSysNo + "已配送，待结算",
                        //                                                      msg.OperationUserName);
                        if (msg.IsThirdPartyExpress == 0)
                        {
                            //smsBo.发送自建物流发货通知短信(msg.MobilePhoneNum, msg.OrderSysNo.ToString(),msg.UserName, msg.UserMobilePhoneNum);
                            //new BLL.Extras.EmailBo().发送百城当日达发货邮件(msg.CustomerEmailAddress, msg.CustomerSysNo.ToString(),
                            //                                    msg.OrderSysNo.ToString(), msg.UserName,
                            //                                    msg.UserMobilePhoneNum);
                        }

                        if (msg.IsThirdPartyExpress == 1)
                        {
                            //2015-10-30 王耀发 创建
                            //获取订单信息
                            //SoOrder SData = Hyt.BLL.Order.SoOrderBo.Instance.GetEntityNoCache(msg.OrderSysNo);
                            //2015-10-30 王耀发 创建 对应分销商
                            //CBDsDealer Dealer = new Hyt.BLL.Distribution.DsDealerBo().GetDsDealer(SData.DealerSysNo);    
                            //string context = "尊敬的客户，您在商城下达的订单(" + SData.OrderNo.ToString() + ")已经由" + msg.DeliveryTypeName + "开始处理，单号" + msg.ExpressNo + "，您可前往" + msg.TraceUrl + "查看物流状态，谢谢！";
                            //Hyt.BLL.Extras.SmsBO.Instance.DealerSendMsg(msg.MobilePhoneNum, Dealer.DealerName, context, DateTime.Now);
                            //smsBo.发送第三方物流发货通知短信(msg.MobilePhoneNum, msg.OrderSysNo.ToString());
                            //new BLL.Extras.EmailBo().发送第三方物流发货邮件(msg.CustomerEmailAddress, msg.CustomerSysNo.ToString(),
                            //                                     msg.OrderSysNo.ToString(), msg.DeliveryTypeName, msg.ExpressNo);

                            //发送配送短信 2016-06-23 王耀发 创建 
                            using (var service = new Hyt.Infrastructure.Communication.ServiceProxy<Grand.Service.EC.Core.SMS.Contract.ISMSService>())
                            {
                                SoOrder order = Hyt.BLL.Order.SoOrderBo.Instance.GetEntityNoCache(msg.OrderSysNo);
                                CBDsDealer dealer = Hyt.BLL.Distribution.DsDealerBo.Instance.GetDsDealer(order.DealerSysNo);
                                SoReceiveAddress receive = Hyt.BLL.Order.OutboundReturnBo.Instance.GetSoReceiveAddressBysoOrderSysNo(msg.OrderSysNo);
                                var smsresult = new SmsResult
                                {
                                    RowCount = 0,
                                    Status = SmsResultStatus.Failue
                                };
                                try
                                {

                                    if (string.IsNullOrWhiteSpace(dealer.ErpCode) || dealer.ErpCode.Trim() != "1") //只有erp编码不为1的才能发短信
                                    {
                                        var smsParams = "{0},{1},{2},{3}";

                                        //获取仓库，用于判断是否完税
                                        var info = "";
                                        var warehouse = BLL.Warehouse.WhWarehouseBo.Instance.GetWarehouse(order.DefaultWarehouseSysNo);
                                        if (warehouse != null && (warehouse.WarehouseType == (int)Model.WorkflowStatus.WarehouseStatus.仓库类型.保税 || warehouse.WarehouseType == (int)Model.WorkflowStatus.WarehouseStatus.仓库类型.直邮))
                                            info = "已完成清关手续";

                                        smsParams = string.Format(smsParams,
                                            string.IsNullOrWhiteSpace(dealer.ErpName) ? dealer.DealerName : dealer.ErpName,
                                            order.OrderNo,
                                            info,
                                            msg.DeliveryTypeName
                                            );

                                        var response = service.Channel.SendMessage(
                                            "发送配送短信",
                                            receive.MobilePhoneNumber,
                                            smsParams,
                                            dealer.DealerName
                                            );


                                        if (response.IsError)
                                            smsresult.Status = SmsResultStatus.Failue;
                                        else
                                            smsresult.Status = SmsResultStatus.Success;

                                    }
                                }
                                catch (Exception ex)
                                {
                                    BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, "发送配送短信：" + ex.Message, ex);
                                }
                            }
                        }
                    }

                    #endregion

                    //回填物流信息
                    try
                    {
                        LgDeliveryBo.Instance.BackFillLogisticsInfo(deliverySysno, deliverTypeSysno);
                    }
                    catch (Exception ex)
                    {
                        Hyt.BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.分销工具, ex.Message, ex);
                    }

                    //调用快递100的订阅服务
                    try
                    {
                        //LgDeliveryBo.Instance.CallKuaiDi100(itemList, warehouseSysNo, delivertType);
                    }
                    catch (Exception ex)
                    {
                        Hyt.BLL.Log.SysLog.Instance.Error(LogStatus.系统日志来源.后台, ex.Message, ex);
                    }
                }
                catch (Exception ex)
                {
                    SysLog.Instance.Error(LogStatus.系统日志来源.分销工具, ex.Message, ex);
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = "发生错误！" + ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, ex.Message, ex);
            }
            return Json(result);
        }

        /// <summary>
        /// 获取仓库配送方式 
        /// </summary>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <returns>返回result</returns>
        /// <remarks>2013-07-02 沈强 创建</remarks>
        [Privilege(PrivilegeCode.LG1006201)]
        public ActionResult GetWarehouseDeliveryType(int warehouseSysNo)
        {
            Result<IList<LgDeliveryType>> result = new Result<IList<LgDeliveryType>>();
            try
            {

                IList<LgDeliveryType> lgDeliveryTypes =
                    DeliveryTypeBo.Instance.GetLgDeliveryTypeByWarehouse(warehouseSysNo);
                result.Status = true;
                result.Message = "成功获取";
                result.Data =
                    lgDeliveryTypes.Where(l => l.ParentSysNo != Model.SystemPredefined.DeliveryType.自提)
                                   .OrderBy(l => l.DeliveryTypeName)
                                   .ToList();
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = "发生错误！";
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, ex.Message, ex);
            }
            return Json(result);
        }

        /// <summary>
        /// 根据仓库系统编号获取配送员
        /// </summary>
        /// <param name="warehouseSysNo"></param>
        /// <returns>返回result</returns>
        /// <remarks>2013-07-03 沈强 创建</remarks>
        [Privilege(PrivilegeCode.LG1006201)]
        public ActionResult GetDeliverymanByWarehouseSysNo(int warehouseSysNo)
        {
            Result<IList<SyUser>> result = new Result<IList<SyUser>>();
            try
            {
                IList<SyUser> deliverymans = BLL.Warehouse.WhWarehouseBo.Instance
                    .GetWhDeliveryUser(warehouseSysNo, LogisticsStatus.配送员是否允许配送.是);
                result.Status = true;
                result.Data = deliverymans.Where(d => d.Status == (int)SystemStatus.系统用户状态.启用).ToList();
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = "发生错误！";
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, ex.Message, ex);
            }
            return Json(result);
        }

        /// <summary>
        /// 组装配送单明细，供添加出库/取件单用（Ajax调用）
        /// </summary>
        /// <param name="noteType">单据类型</param>
        /// <param name="noteSysNo">单据编号数组</param>
        /// <param name="deliveryTypeSysNo">配送类型系统编号</param>
        /// <param name="deliveryType">是否为第三方配送：是[1] 否[0]</param>
        /// <param name="deliveryUserSysNo">配送员系统编号</param>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <returns>返回组装后的配送单明细</returns>
        /// <remarks>2013-07-04 沈强 创建</remarks>
        [Privilege(PrivilegeCode.LG1006201)]
        public JsonResult GetDeliveryNoteItemList(int noteType, int[] noteSysNo, int deliveryTypeSysNo, int deliveryType, int deliveryUserSysNo, int warehouseSysNo)
        {
            var result = new Result<IList<CBLgDeliveryItemJson>>();
            try
            {
                result = LgDeliveryBo.Instance.GetCBLgDeliveryItems(noteType, noteSysNo, deliveryTypeSysNo, deliveryType, deliveryUserSysNo, warehouseSysNo);
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, ex.Message, ex);
            }
            //foreach (var item in noteSysNo)
            //{
            //    int orderID =Convert.ToInt32(DeliveryTypeBo.Instance.GetorderId(item)); //获取订单编号
            //    var expensesAmount = Hyt.BLL.Order.SoOrderOtherExpensesBo.Instance.GetExpensesFee(orderID);
            //    if (expensesAmount > 0)
            //    {
            //        DouShabaoOrder(orderID);
            //    }
            //}
            //GetDeliveryType
            return Json(result);
        }

        private ActionResult DouShabaoOrder(int orderID)
        {
            //根据订单获取订单信息
            var model = SoOrderBo.Instance.GetEntity(orderID);
            #region 获取参数
            var sparameter = SoOrderBo.Instance.GetSignparameter(orderID);  //获取签名参数
            var douShabaoParameter = SoOrderBo.Instance.Getotherparameter(orderID);  //获取配送方式，身份证，总价，(运费)，下单时间
            if (douShabaoParameter != null)
            {
                ProductList productlist = SoOrderBo.Instance.GetProductlist(orderID); //获取信营订单
                string orderNo = orderID.ToString();
                string totalAmount = sparameter.TotalAmount;
                string buyerName = sparameter.BuyerName;
                string buyerMobile = sparameter.BuyerMobile;
                string sign = Hyt.BLL.DouShabao.DouShabaoSign.Instance.GetSign(orderNo, totalAmount, buyerName, buyerMobile); //获取签名
                //豆沙包传入参数
                douShabaoParameter.OrderNo = orderID.ToString();
                string expressChannel = douShabaoParameter.ExpressChannel; //运输方式
                string idCard = douShabaoParameter.IdCard; //身份证
                string orderTime = douShabaoParameter.OrderTime.ToString("yyyy-MM-dd hh:mm:ss"); //订单时间
                string purchasOrderNo = douShabaoParameter.PurchasOrderNo; //订单号
                string expressNo = douShabaoParameter.ExpressNo; //物流单号
                double totalWeight = douShabaoParameter.TotalWeight; //总重量
                string transitLine = "EMS清关路线";
                string purchasOrderAmount = sparameter.TotalAmount;
                string salesMeasurementUnit = douShabaoParameter.SalesMeasurementUnit;
                if (salesMeasurementUnit == "kg") //千克
                {
                    totalWeight = douShabaoParameter.TotalWeight;
                }
                else if (salesMeasurementUnit == "g") //克
                {
                    totalWeight = douShabaoParameter.TotalWeight / 1000;
                }
                else if (salesMeasurementUnit == "lb") //磅
                {
                    totalWeight = douShabaoParameter.TotalWeight / 2.2046226;
                }
                else if (salesMeasurementUnit == "oz") //盎司
                {
                    totalWeight = douShabaoParameter.TotalWeight / 35.2739619;
                }
                else //其他
                {
                    totalWeight = douShabaoParameter.TotalWeight;
                }
                //if (expressNo == null)
                //{
                //    expressNo = "1234567890"; //若配送单为空，默认为1234567890
                //}
                string purchasOrderTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                Hyt.BLL.DouShabao.DouShabaoSign.Instance.DoushabaoRealize(orderNo, expressChannel, totalAmount, buyerName, buyerMobile, idCard, expressNo, transitLine, orderTime, totalWeight, sign, purchasOrderNo, purchasOrderAmount, purchasOrderTime, productlist);
            }
            else
            {
                return null;
            }
            #endregion

            return null;
        }
        #endregion

        #endregion

        #endregion

        #region 生成结算单操作 沈强 2013-06-28

        /// <summary>
        /// 创建结算单页面
        /// </summary>
        /// <param name="id">page no</param>
        /// <param name="deliveryUserSysNo">配送人员系统编号</param>
        /// <param name="warehouseSysNo">仓库系统编号</param>
        /// <returns>ActionResult</returns>
        /// <remarks>
        /// 2013-07-03 何方 创建
        /// </remarks>
        [Privilege(PrivilegeCode.LG1005201)]
        public ActionResult SettlementDeliveryList(int? id, int? deliveryUserSysNo, int warehouseSysNo = 0)
        {
            int? currentPage = id;
            var pageList = new Infrastructure.Pager.PagedList();
            var model = new PagedList<CBLgDelivery>();
            var pager = new Pager<CBLgDelivery> { CurrentPage = id ?? 1, PageSize = 10 };
            var cbLgDelivery = new CBLgDelivery();

            // cbLgDelivery.IsThirdPartyExpress=0; //只查询非第三番快递的
            if (deliveryUserSysNo != null && deliveryUserSysNo > 0)
            {
                cbLgDelivery.DeliveryUserSysNo = deliveryUserSysNo.Value;
            }
            cbLgDelivery.Status = (int)Model.WorkflowStatus.LogisticsStatus.配送单状态.配送在途;
            cbLgDelivery.IsThirdPartyExpress = "0";
            cbLgDelivery.StockSysNo = warehouseSysNo;
            //cbLgDelivery.StockSysNos = warehouses;//huangwei 11.27
            //cbLgDelivery.SysNo = sysNo;
            pager.Rows.Add(cbLgDelivery);

            LgDeliveryBo.Instance.GetLgDelivery(ref pager, CurrentUser);
            model.TData = pager.Rows;//.Where(p=>warehouses.Contains(p.StockSysNo)).ToList();
            model.TotalItemCount = pager.TotalRows;
            model.CurrentPageIndex = pager.CurrentPage;

            if (Request.IsAjaxRequest())
            {
                return PartialView("_AjaxPagerDeliveryList", model);
            }

            //仓库
            ViewBag.WarehouseList = CurrentUser.Warehouses;

            return View(model);

        }

        /// <summary>
        /// 检查配送单状态是否配送在途
        /// </summary>
        /// <param name="sysNo">配送单系统编号</param>
        /// <returns>ActionResult</returns>
        /// <remarks>2013-06-15 黄伟 创建</remarks>
        [Privilege(PrivilegeCode.LG1005201)]
        public ActionResult IsRightStatus(int sysNo)
        {
            var del = LgDeliveryBo.Instance.GetDelivery(sysNo);
            if (del == null || del.Status != LogisticsStatus.配送单状态.配送在途.GetHashCode())
            {
                return new JsonResult { Data = false, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return new JsonResult { Data = true, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        #endregion

        #region 补单操作 沈强 2013-07-10

        /// <summary>
        /// 补充订单
        /// </summary>
        /// <param name=""></param>
        /// <returns>返回补充订单页面</returns>
        /// <remarks>2013-07-10 沈强 创建</remarks> 
        [Privilege(PrivilegeCode.LG1004101)]
        public ActionResult AdditionalOrders()
        {
            IList<WhWarehouse> whWarehouses = new List<WhWarehouse>();
            try
            {
                whWarehouses = Hyt.BLL.Authentication.AdminAuthenticationBo.Instance.Current.Warehouses;
            }
            catch (Exception ex)
            {
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, ex.Message, ex);
            }
            return View(whWarehouses);
        }

        /// <summary>
        /// 获取指定配送员的借货商品 （ajax调用）
        /// </summary>
        /// <param name="deliverymanSysNo">配送员系统编号</param>
        /// <param name="productSysNo">商品系统编号数组</param>
        /// <param name="userGrade">会员等级系统编号</param>
        /// <returns>json结果</returns>
        /// <remarks>2013-07-11 沈强 创建</remarks>
        [Privilege(PrivilegeCode.LG1004101)]
        public JsonResult GetDeliverymanProductLend(int deliverymanSysNo, int[] productSysNo, int? userGrade)
        {
            Result<object> result = new Result<object>();
            try
            {
                IList<CBPdProductJson> cbPdProductJsons = BLL.Warehouse.WhWarehouseBo.Instance.GetProductLendGoods(deliverymanSysNo, userGrade);

                //提取只包含指定商品系统编号的借货单商品集合
                var tmp = cbPdProductJsons.Where(c => productSysNo.Contains(c.ProductSysNo)).ToList();
                #region
                //var tobj = tmp.GroupBy(o => o.ProductSysNo);

                //var objs =
                //    from item in tobj
                //    select
                //        new
                //            {
                //                ProductSysNo = item.Key,
                //                ProductNum = item.Sum(i => i.ProductNum),
                //                ProductLendItemSysNo = item.First(i => item.Key == i.ProductSysNo).SysNo,
                //                ProductName = item.First(i => item.Key == i.ProductSysNo).ProductName,
                //                ProductOrderNum = "1",  //商品订购数量，默认为1
                //                Price = item.First(i => item.Key == i.ProductSysNo).Price.ToString("C")
                //            };
                #endregion
                var objs = tmp.Select(c => new
                {
                    ProductSysNo = c.ProductSysNo.ToString(),
                    ProductName = c.ProductName.ToString(),
                    ProductNum = c.ProductNum,
                    ProductOrderNum = "1",  //商品订购数量，默认为1
                    Price = c.Price.ToString("C")
                }).ToList();

                result.Data = objs;
                result.Status = true;
                result.Message = "成功获取";
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = "发生错误！";
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, ex.Message, ex);
            }

            return Json(result);
        }

        /// <summary>
        /// 获取借货单商品部分视图
        /// </summary>
        /// <param name="deliverymanSysNo">配送员系统编号</param>
        /// <param name="productSysNos">已添加的商品系统编号数组</param>
        /// <param name="userGrade">会员等级系统编号</param>
        /// <returns>部分视图</returns>
        /// <remarks>2013-07-12 沈强 创建</remarks> 
        [Privilege(PrivilegeCode.LG1004101)]
        public ActionResult GetProductLendPartView(int deliverymanSysNo, int[] productSysNos, int? userGrade)
        {
            IList<CBPdProductJson> cbPdProductJsons = new List<CBPdProductJson>();
            try
            {
                IList<CBPdProductJson> tmp = BLL.Warehouse.WhWarehouseBo.Instance.GetProductLendGoods(deliverymanSysNo, userGrade);

                //过滤掉已经添加的商品
                if (productSysNos != null && productSysNos.Length > 0)
                {
                    cbPdProductJsons = tmp.Where(c => !productSysNos.Contains(c.ProductSysNo)).ToList();
                }
                else
                {
                    cbPdProductJsons = tmp;
                }

            }
            catch (Exception ex)
            {

                SysLog.Instance.Error(LogStatus.系统日志来源.后台, ex.Message, ex);
            }
            return PartialView("_ProductLendPartView", cbPdProductJsons);
        }

        /// <summary>
        /// 确认补单完成
        /// </summary>
        /// <param name="additionalOrders">补单信息参数</param>
        /// <returns>json结果</returns>
        /// <remarks>2013-07-16 沈强 创建</remarks>
        [Privilege(PrivilegeCode.LG1004101)]
        public JsonResult ConfirmAdditionalOrders(string additionalOrders)
        {
            var serializer = new JavaScriptSerializer();

            Result result = new Result();
            try
            {
                var orders = serializer.Deserialize<Hyt.Model.Parameter.ParaLogisticsControllerAdditionalOrders>(additionalOrders);
                result = BLL.Logistics.LogisticsOrderBo.Instance.AddOrders(orders, CurrentUser.Base);
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = "发生错误！";
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, ex.Message, ex);
            }
            return Json(result);
        }

        #endregion

        #region 结算单列表 黄伟 2013-06-15

        /// <summary>
        /// 结算单维护-index/查找/高级查找
        /// </summary>
        /// <param name="id">the pageindex passed from ajax pager</param>
        /// <param name="searchParas">查询条件model</param>
        /// <returns>结算单维护视图</returns>
        /// <remarks>2013-06-15 黄伟 创建</remarks>
        [Privilege(PrivilegeCode.LG1003102)]
        public ActionResult LgSettlement(int? id, ParaLogisticsLgsettlement searchParas)
        {

            var pagerResult = LgSettlementBo.Instance.Search(id, searchParas, CurrentUser.Warehouses.Select(p => p.SysNo).ToList());
            var model = new PagedList<CBLgSettlement>
            {
                TData = pagerResult.Rows,
                PageSize = pagerResult.PageSize,
                TotalItemCount = pagerResult.TotalRows,
                CurrentPageIndex = id ?? 1
            };

            if (Request.IsAjaxRequest())
            {
                return PartialView("pAjaxPager_LgSettlement", model);
            }

            //结算单状态下拉列表
            ViewBag.lstStatus = GetSettlementStatus();
            //仓库
            ViewBag.WarehouseList = CurrentUser.Warehouses;

            return View();
        }

        /// <summary>
        /// 获取结算单下拉列表
        /// </summary>
        /// <param name=" "></param>
        /// <returns>结算单下拉列表</returns>
        /// <remarks>2013-07-04 黄伟 创建</remarks>
        [Privilege(PrivilegeCode.LG1003102)]
        private List<SelectListItem> GetSettlementStatus()
        {
            var lstStatus = new List<SelectListItem> { new SelectListItem { Text = @"全部", Value = "0", Selected = true } };
            lstStatus.AddRange(from key in Enum.GetNames(typeof(LogisticsStatus.结算单状态))
                               let val = Enum.Parse(typeof(LogisticsStatus.结算单状态), key).GetHashCode()
                               select new SelectListItem
                               {
                                   Text = key,
                                   Value = val + "",
                                   Selected = false
                               });
            return lstStatus;
        }

        /// <summary>
        /// 分部视图 仓库与配送人员
        /// </summary>
        /// <param name="wareHouseId">指定仓库下拉选单id</param>
        /// <param name="deliveryManId">指定配送人员下拉选单id</param>
        /// <param name="lblWareHouseId">指定label仓库id</param>
        /// <param name="lblDeliveryManId">指定label配送人员id</param>
        /// <param name="inline">display style-true:inline;false:block</param>
        /// <returns>分布视图-仓库与配送人员</returns>
        /// <remarks>2013-06-27 黄伟 创建</remarks>
        [Privilege(PrivilegeCode.LG1003102)]
        [ActionName("WareHouseAndDeliveryMan")]
        public PartialViewResult GetWareHouseAndDeliveryMan(string wareHouseId = "selWareHouseBasic", string deliveryManId = "selDelManBasic",
            string lblWareHouseId = "lblWareHouseBasic", string lblDeliveryManId = "lblDelManBasic", bool inline = true)
        {
            //combobox list item
            //仓库
            var lstWh = new List<SelectListItem>() { new SelectListItem { Text = _strDropdownlistDef, Value = "0", Selected = true } };
            lstWh.AddRange(LgSettlementBo.Instance.GetWareHouse().Select(p => new SelectListItem { Text = p.Value, Value = p.Key + "" }));
            //配送人员
            var lstUser = new List<SelectListItem>() { new SelectListItem { Text = _strDropdownlistDef, Value = "0", Selected = true } };
            //lstUser.AddRange(boLgSettlementBo.GetDeliveryMan(null).Select(p => new SelectListItem { Text = p.Value, Value = p.Key + "" }));

            ViewBag.WareHouses = lstWh;
            ViewBag.Users = lstUser;
            //for id name
            ViewBag.whId = wareHouseId;
            ViewBag.userId = deliveryManId;
            ViewBag.lblWhId = lblWareHouseId;
            ViewBag.lblUserId = lblDeliveryManId;
            ViewBag.inLine = inline;

            return PartialView("_pWareHouseAndDeliveryMan");
        }

        #endregion

        #region 结算单明细 黄伟 2013-06-28

        /// <summary>
        /// 结算单明细
        /// </summary>
        /// <param name="id">结算单主表系统编号</param>
        /// <returns>结算单明细视图</returns>
        /// <remarks>2013-07-04 黄伟 创建</remarks>
        [Privilege(PrivilegeCode.LG1003102)]
        public ActionResult LgSettlementDetails(int id)
        {
            var settlement = LgSettlementBo.Instance.GetLgSettlementWithItems(id);
            if (settlement == null)
            {
                throw new Exception(string.Format("不存在编号为{0}的结算单!", id));
            }

            if (!AdminAuthenticationBo.Instance.HasWareHousePrivilege(settlement.WarehouseSysNo))
            {
                var message = string.Format("没有权限读取仓库:{0}的信息",
                                            WhWarehouseBo.Instance.GetWarehouseName(settlement.WarehouseSysNo));
                return View("ErrorPrivilegeWithMessage", (object)(message));
            }

            var model = LgSettlementBo.Instance.GetLgSettlementWithItems(id);

            //支付方式及支付金额 maybe used later
            //var dicPaymentList = new Dictionary<int, decimal>();
            //model.Items.GroupBy(m => m.PayType)
            //         .ToList().ForEach(p => dicPaymentList.Add(p.Key, p.Sum(i => i.PayAmount)));
            //ViewBag.dicPaymentList = dicPaymentList;

            //收款明细支付方式
            ViewBag.payType = model.Items.Any() ? LgSettlementBo.Instance.GetSettlementDetailsPayType(model.Items.First()) : "";

            if (!string.IsNullOrEmpty(Request["IsView"]))
            {
                ViewBag.IsView = true;
            }

            return View(model);
        }

        /// <summary>
        /// 作废结算单明细
        /// </summary>
        /// <returns>封装的实体(Status,StatusCode,Message)</returns>
        /// <param name="sysNo">结算单明细系统编号</param>
        /// <remarks>2013-08-21 黄伟 创建</remarks>
        /// <remarks>2014-04-21 何方 禁止作废</remarks>
        [Privilege(PrivilegeCode.LG1003105)]
        public JsonResult CancelSettlementItem(int sysNo)
        {
            using (var tran = new TransactionScope())
            {
                try
                {
                    LgSettlementBo.Instance.CancelSettlementItem(sysNo, CurrentUser.Base.SysNo, WebUtil.GetUserIp());
                    //throw new Exception("for test purpose,not save the data");//for test purpose
                    tran.Complete();
                }
                catch (Exception ex)
                {
                    return Json(new Result { Status = false, Message = "作废失败:" + ex.Message }, JsonRequestBehavior.AllowGet);
                }

            }

            return Json(new Result { Status = true, Message = "作废成功!" }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region  生成结算单 黄伟 2013-07-09

        /// <summary>
        /// 生成结算单视图
        /// </summary>
        /// <param name="deliverySysNos">配送单系统编号集合</param>
        /// <returns>返回结算单视图</returns>
        /// <remarks>
        /// 何永东 创建
        /// 2013-07-09 黄伟 修改
        /// 2014-06-06 余勇 修改 加入异常处理及写入日志
        /// </remarks>
        [Privilege(PrivilegeCode.LG1005201)]
        public ActionResult CreateSettlement(int[] deliverySysNos)
        {
            var model = new CBLgSettlement();
            try
            {
                model = LgSettlementBo.Instance.GetCbLgSettlement(deliverySysNos, CurrentUser.Base.SysNo);
            }
            catch (Exception ex)
            {
                var id = deliverySysNos != null ? deliverySysNos.Join(",") : "null";
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "生成结算单视图" + ex.Message + "。deliverySysNos:" + id, LogStatus.系统日志目标类型.结算单, 0, ex, CurrentUser == null ? 0 : CurrentUser.Base.SysNo);
            }

            //ViewBag.delSysNos = deliverySysNos;
            ViewBag.delSysNos = string.Join(",", deliverySysNos);
            //ViewBag.lst

            return View(model);
        }

        /// <summary>
        /// 确认结算
        /// </summary>
        /// <param name="cbCreateSettlement">json对应实体CBCreateSettlement</param>
        /// <returns>返回result实体</returns>
        /// <remarks>2013-07-09 黄伟 创建</remarks>
        [Privilege(PrivilegeCode.LG1005201)]
        [HttpPost]
        public JsonResult ConfirmSettlement(string cbCreateSettlement)
        {
            var options = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                Timeout = TransactionManager.DefaultTimeout
            };
            try
            {
                var cbSettlement = new JavaScriptSerializer().Deserialize<CBCreateSettlement>(cbCreateSettlement);
                cbSettlement.OperatorSysNo = CurrentUser.Base.SysNo;
                using (var tran = new TransactionScope(TransactionScopeOption.Required, options))
                {
                    var result = LgSettlementBo.Instance.CreateSettlement(cbSettlement, WebUtil.GetUserIp());
                    tran.Complete();
                    return Json(result);
                }
            }
            catch (Exception ex)
            {
                return Json(new Result<int>() { Status = false, Message = ex.Message });

            }
        }

        /// <summary>
        /// 获取app签收信息
        /// </summary>
        /// <param name="delSysNos">配送单系统编号列表</param>
        /// <returns>Result:
        /// data=list of LgAppSignInfo
        /// </returns>
        /// <remarks>2014-1-14 黄伟 创建</remarks>
        [Privilege(PrivilegeCode.LG1005201)]
        public JsonResult GetAppSignInfo(string delSysNos)
        {
            var lstDelSysNos = delSysNos.Split(',').Select(int.Parse).ToList();
            var result = LgSettlementBo.Instance.GetAppSignInfo(lstDelSysNos);
            var jsonResult = new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            return jsonResult;
        }

        /// <summary>
        /// (部分签收)出库单修改数量后返回应退金额
        /// </summary>
        /// <param name="cbRMAOrderInfo">退货订单相关信息(实体对应CBRMAOrderInfo)</param>
        /// <returns>应退金额</returns>
        /// <remarks>2013-07-09 黄伟 创建</remarks>
        [Privilege(PrivilegeCode.LG1005201)]
        public JsonResult GetProductPrice(string cbRMAOrderInfo)
        {
            var model = new JavaScriptSerializer().Deserialize<CBRMAOrderInfo>(cbRMAOrderInfo);
            return Json(LgSettlementBo.Instance.GetProductPrice(model), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 取件单维护 周唐炬 2013-08-12

        /// <summary>
        /// 取件单维护列表
        /// </summary>
        /// <param name="id">查询参数</param>
        /// <param name="filter">查询参数</param>
        /// <returns>取件单维护列表视图</returns>
        /// <remarks>2013-08-12 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.LG1010101)]
        public ActionResult PickupInvoice(int? id, ParaPickUpFilter filter)
        {
            try
            {
                var wareHouseList = CurrentUser.Warehouses;

                if (Request.IsAjaxRequest())
                {
                    var pageList = new PagedList<LgPickUp>();
                    var curPageIndex = id ?? 1;
                    filter.CurrentPage = curPageIndex;
                    filter.PageSize = pageList.PageSize;

                    if (!filter.WarehouseSysNo.HasValue && wareHouseList.Count > 0)
                    {
                        filter.WarehouseSysNoList = new List<int>();
                        wareHouseList.ForEach(x => filter.WarehouseSysNoList.Add(x.SysNo));
                    }
                    pageList = LgPickUpBo.Instance.GetPickUpList(filter);

                    return PartialView("_AjaxPagerPickupInvoiceList", pageList);
                }
                var item = new SelectListItem() { Text = @"全部", Value = "", Selected = true };

                var statustList = new List<SelectListItem>() { item };
                EnumUtil.ToListItem<LogisticsStatus.取件单状态>(ref statustList);
                ViewData["Status"] = new SelectList(statustList, "Value", "Text");

                var pickUpTypeList = new List<SelectListItem>() { item };
                LgPickUpTypeBo.Instance.GetLgPickupTypeList().ForEach(x => pickUpTypeList.Add(
                    new SelectListItem
                        {
                            Text = x.PickupTypeName,
                            Value = x.SysNo.ToString(CultureInfo.InvariantCulture)
                        }));

                ViewData["PickUpType"] = new SelectList(pickUpTypeList, "Value", "Text");
            }
            catch (Exception ex)
            {
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "取件单维护列表" + ex.Message, LogStatus.系统日志目标类型.借货单, 0, ex, CurrentUser == null ? 0 : CurrentUser.Base.SysNo);
            }
            return View();
        }

        /// <summary>
        /// 取件单明细
        /// </summary>
        /// <param name="id">取件单系统编号</param>
        /// <returns>取件单明细视图</returns>
        /// <remarks>2013-08-12 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.LG1010101)]
        public ActionResult PickupInvoiceDetails(int id)
        {
            LgPickUp model = null;
            try
            {
                if (Request.IsAjaxRequest())
                {
                    var items = LgPickUpBo.Instance.GetLgPickUpItem(id);
                    return PartialView("_AjaxPagerPickupInvoiceDetailProducts", items);
                }
                model = LgPickUpBo.Instance.GetPickUp(id);
            }
            catch (Exception ex)
            {
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "取件单明细-SysNo:" + id + ex.Message, LogStatus.系统日志目标类型.取件单, id, ex, CurrentUser == null ? 0 : CurrentUser.Base.SysNo);
            }
            return View(model);
        }

        #endregion

        #region 百城当日达区域维护 郑荣华 2013-08-12

        /// <summary>
        /// 百城当日达区域维护主页面
        /// </summary>
        /// <param name=""></param>
        /// <returns>百城当日达区域维护主页面</returns>
        /// <remarks> 
        /// 2013-08-02 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.LG1002101)]
        public ActionResult DeliveryScope()
        {
            //2016-3-26 王耀发 修改 659031 为中国 
            var model = BasicAreaBo.Instance.GetAreaList(659031);//省级地区
            return View(model);
        }

        /// <summary>
        /// 根据城市编号获取百城当日达区域信息
        /// </summary>
        /// <param name="areaSysNo">城市sysno</param>
        /// <returns>百城当日达区域信息列表</returns>
        /// <remarks> 
        /// 2013-08-02 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.LG1002101, PrivilegeCode.SO1001101)]
        public JsonResult GetDeliveryScope(int areaSysNo)
        {
            var model = LgDeliveryScopeBo.Instance.GetDeliveryScope(areaSysNo);
            return Json(model, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 根据城市系统编号删除百城当日达区域信息
        /// </summary>
        /// <param name="areaSysNo">要删除的城市的系统编号</param>
        /// <returns>成功true,失败false</returns>
        /// <remarks> 
        /// 2013-08-12 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.LG1002401)]
        public bool DeleteDeliveryScope(int areaSysNo)
        {
            return LgDeliveryScopeBo.Instance.DeleteByAreaSysNo(areaSysNo);
        }

        /// <summary>
        /// 保存城市百城当日达范围信息
        /// </summary>
        /// <param name="areaSysNo">地区系统编号</param>
        /// <param name="mapScopes">区域信息‘|’分隔</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks> 
        /// 2013-08-12 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.LG1002201)]
        public bool SaveDeliveryScope(int areaSysNo, string mapScopes)
        {
            if (mapScopes == "")
            {
                LgDeliveryScopeBo.Instance.DeleteByAreaSysNo(areaSysNo); //直接全部删除
                return true;
            }

            var model = new LgDeliveryScope
                {
                    AreaSysNo = areaSysNo,
                    CreatedBy = CurrentUser.Base.SysNo,
                    CreatedDate = DateTime.Now,
                    LastUpdateBy = CurrentUser.Base.SysNo,
                    LastUpdateDate = DateTime.Now,
                    Description = "百城当日达"
                };

            var mapScope = mapScopes.Split('|');//分隔符

            try
            {
                LgDeliveryScopeBo.Instance.DeleteByAreaSysNo(areaSysNo); //先全部删除                       
                foreach (var item in mapScope)//有多条记录(一般1-3条)，循环添加
                {
                    model.MapScope = item.Replace("NaN,0;\n", "");
                    LgDeliveryScopeBo.Instance.Create(model);
                }

                return true;
            }
            catch (Exception ex)
            {
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, ex.Message, LogStatus.系统日志目标类型.百城当日达区域, CurrentUser.Base.SysNo);
                return false;
            }




        }
        #endregion


        #region 门店创库百度地图 LYK 2015-08-07
        /// <summary>
        /// 门店创库百度地图显示
        /// </summary>
        /// <returns></returns>
        /// <remarks>2015-08-07 LYK 创建</remarks>
        [Privilege(PrivilegeCode.LG30001)]
        public ActionResult StoreWarehouse()
        {
            var model = BasicAreaBo.Instance.GetAreaList(0);//省级地区
            return View(model);
        }
        /// <summary>
        /// 根据城市编号获取仓库信息，用于百度地图显示
        /// </summary>
        /// <param name="areaSysNo">城市sysno</param>
        /// <returns>百城当日达区域信息列表</returns>
        /// <remarks> 
        /// 2015-08-06 LYK 创建
        /// </remarks>
        [Privilege(PrivilegeCode.LG30001)]
        public JsonResult GetWhWarehouseDiTu(int areaSysNo)
        {
            var model = LgDeliveryScopeBo.Instance.GetWhWarehouseDiTu(areaSysNo);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 物流号刷单

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">页索引</param>
        /// <param name="expressNo">物流单号</param>
        /// <returns></returns>
        /// <remarks>2013-12-19 沈强 创建</remarks>
        [Privilege(PrivilegeCode.LG1005101)]
        public ActionResult FakeInfo(int? id, string expressNo)
        {
            if (!Request.IsAjaxRequest())
            {
                return View();
            }
            var pageIndex = id ?? 1;

            var mkExpress = new MkExpressLog { ExpressNo = string.IsNullOrEmpty(expressNo) ? null : expressNo };

            var pagerFilter = new Pager<MkExpressLog> { PageFilter = mkExpress, CurrentPage = pageIndex, PageSize = 10 };

            var pager = MkExpressLogBo.Instance.GetLogisticsDeliveryItems(pagerFilter);

            return PartialView("_LogisticsNoRefreshList", pager.Map());
        }

        /// <summary>
        /// 获取物流日志详细信息
        /// </summary>
        /// <param name="expressNo">物流号</param>
        /// <returns></returns>
        /// <remarks>2013-12-19 沈强 创建</remarks>
        [Privilege(PrivilegeCode.LG1005101)]
        public ActionResult GetMKLogList(string expressNo)
        {
            var list = MkExpressLogBo.Instance.GetMkExpressLogList(expressNo).OrderBy(o => o.SysNo).ToList();
            return PartialView("_MKExpressLogList", list);
        }

        /// <summary>
        /// 生成物流日志
        /// </summary>
        /// <param name="expressNo">物流编号</param>
        /// <returns></returns>
        /// <remarks>2013-12-20 沈强 创建</remarks>
        /// <remark>2014-6-11 何明壮  修改</remark>
        [Privilege(PrivilegeCode.LG1005101)]
        public ActionResult GenerateLogisticsLog(string expressNo)
        {
            expressNo = expressNo.Trim();
            //验证数据是否合法
            Hyt.Util.Validator.VResult vResult =
                Hyt.Util.Validator.VHelper.ValidatorRule(new Hyt.Util.Validator.Rule.Rule_NotAllowNull(expressNo, "请输入物流号！"),
                new Hyt.Util.Validator.Rule.Rule_StringLenth(expressNo, 1, 14, "物流号为长度在1至14位的数字，请确认物流号长度！"));
            if (!vResult.IsPass)
            {
                //数据不合法
                return Json(new Result() { Status = false, Message = vResult.Message });
            }
            if (!Regex.IsMatch(expressNo, "^[1-9][0-9]*$"))
            {
                return Json(new Result() { Status = false, Message = "物流号只能为大于零的整数" });
            }
            var list = MkExpressLogBo.Instance.GetMkExpressLogList("T" + expressNo.PadLeft(14, '0'));
            if (list != null && list.Any())
            {
                return Json(new Result() { Message = "当前物流单号，已生成相关日志" });
            }
            MkExpressLogBo.Instance.GenerateLogisticsLog("T" + expressNo.PadLeft(14, '0'));
            return Json(new Result() { Status = true });
        }
        #endregion

        #region 加盟商当日达 朱成果 2014-10-08
        /// <summary>
        /// 加盟商当日达
        /// </summary>
        /// <returns></returns>
        /// <remarks>2014-10-08 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.LG2001)]
        public ActionResult WhDeliveryScope()
        {
            //2016-3-26 王耀发 修改 659031 为中国 
            var model = BasicAreaBo.Instance.GetAreaList(659031);//省级地区
            return View(model);
        }

        /// <summary>
        /// 加盟商仓库
        /// </summary>
        /// <param name="areaSysNo">地区编号</param>
        /// <returns></returns>
        /// <remarks>2014-10-08 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.LG2001)]
        public ActionResult WarehouseNotSelfSupport(int areaSysNo)
        {
            List<SelectListItem> dpdlst = GetWarehouseNotSelfSupportByArea(areaSysNo);

            return Json(dpdlst, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取加盟商
        /// </summary>
        /// <param name="cityNo">城市编号</param>
        /// <returns></returns>
        /// <remarks>2014-10-08 朱成果 创建</remarks>
        private List<SelectListItem> GetWarehouseNotSelfSupportByArea(int cityNo)
        {
            return WhDeliveryScopeBo.Instance.GetWhWarehouseForDeliveryScope(cityNo, (int)WarehouseStatus.仓库类型.仓库, (int)WarehouseStatus.是否自营.否, Hyt.Model.SystemPredefined.DeliveryType.百城当日达)
                .Select(m => new SelectListItem() { Text = m.WarehouseName, Value = m.SysNo.ToString() })
                .ToList();
        }


        /// <summary>
        /// 保存加盟商当日达区域信息
        /// </summary>
        /// <param name="areaSysNo">城市</param>
        /// <param name="warehouseSysNo">仓库</param>
        /// <param name="deliveryScope">配送区域</param>
        /// <returns></returns>
        /// <remarks>2014-10-08 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.LG2001)]
        public ActionResult SaveWhDeliveryScope(int? areaSysNo, int? warehouseSysNo, List<WhDeliveryScope> deliveryScope)
        {
            Result r = new Result() { Status = true };
            try
            {
                using (var tran = new TransactionScope())
                {

                    if (warehouseSysNo.HasValue && warehouseSysNo.Value > 0)
                    {
                        WhDeliveryScopeBo.Instance.DeleteByWarehouseSysNo(warehouseSysNo.Value);
                    }
                    else if (areaSysNo.HasValue && areaSysNo.Value > 0)
                    {
                        var lst = GetWarehouseNotSelfSupportByArea(areaSysNo.Value);
                        lst.ForEach((m) =>
                        {

                            WhDeliveryScopeBo.Instance.DeleteByWarehouseSysNo(int.Parse(m.Value));
                        });
                    }
                    if (deliveryScope != null)
                    {
                        foreach (var item in deliveryScope)
                        {
                            item.LastUpdateBy = item.CreatedBy = CurrentUser.Base.SysNo;
                            item.LastUpdateDate = item.CreatedDate = DateTime.Now;
                            WhDeliveryScopeBo.Instance.Insert(item);
                        }
                    }
                    tran.Complete();
                }
            }
            catch (Exception ex)
            {
                r.Status = false;
                r.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "仓库百城当日达区域错误", LogStatus.系统日志目标类型.百城当日达区域, CurrentUser.Base.SysNo, ex);
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取仓库配送范围
        /// </summary>
        /// <param name="areaSysNo">地区</param>
        /// <param name="warehouseSysNo">仓库</param>
        /// <returns></returns>
        /// <remarks>2014-10-08 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.LG2001, PrivilegeCode.SO1001101)]
        public ActionResult GetWhDeliveryScope(int areaSysNo, int? warehouseSysNo)
        {
            var lst = WhDeliveryScopeBo.Instance.GetList();
            if (warehouseSysNo.HasValue && warehouseSysNo.Value > 0)
            {
                return Json(lst.Where(m => m.WarehouseSysNo == warehouseSysNo.Value).ToList(), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(lst.Where(m => m.CityNo == areaSysNo).ToList(), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 运费模板 王耀发 2015-08-06
        /// <summary>
        /// 运费模板列表
        /// </summary>
        /// <returns>分页</returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.LG1015101)]
        public ActionResult FreightModuleList()
        {
            return View();
        }

        /// <summary>
        /// 运费模板界面
        /// </summary>
        /// <param name="id">分页页码</param>
        /// <returns></returns>
        //// <remarks>2015-08-06 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.CM1010002, PrivilegeCode.LG1015101)]
        public ActionResult FreightModule(int? id)
        {
            int pageIndex = id ?? 1;
            int status = 0;

            int.TryParse(Request.Params["status"], out status);
            string name = Request.Params["name"];
            string selector = Request.Params["selector"];
            if (!string.IsNullOrEmpty(selector) && selector == "selector")
            {
                status = (int)LogisticsStatus.运费模板状态.已审核;
            }
            var list = new PagedList<LgFreightModule>();

            var pager = new Pager<LgFreightModule>
            {
                CurrentPage = pageIndex,
                PageFilter = new LgFreightModule { Status = status, ModuleCode = name, ModuleName = name },
                PageSize = list.PageSize
            };

            pager = LgFreightModuleBo.Instance.GetFreightModuleList(pager);

            if (!string.IsNullOrEmpty(selector) && selector == "selector") //品牌组件view层
            {
                return PartialView("_AjaxFreightModulePagerSelector", pager.Map());
            }
            return View();
        }
        /// <summary>
        /// 获取运费模板信息
        /// </summary>
        /// <param name="sysNo">费模板编号</param>
        /// <returns></returns>
        /// <remarks>2015-09-09 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.LG1015101)]
        public JsonResult GetEntityByProductAddress()
        {
            int ProductAddress = int.Parse(this.Request["ProductAddress"]);
            LgFreightModule model = LgFreightModuleBo.Instance.GetEntityByProductAddress(ProductAddress);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 分页获取运费模板
        /// </summary>
        /// <param name="filter">传入的实体参数</param>
        /// <returns>运费模板列表</returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.LG1015101)]
        public ActionResult DoLgFreightModuleQuery(ParaFreightModule filter)
        {

            var pager = LgFreightModuleBo.Instance.GetLgFreightModuleList(filter);
            var list = new PagedList<LgFreightModule>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };
            return PartialView("_FreightModuleListPager", list);
        }

        /// <summary>
        /// 运费模板新建/编辑
        /// </summary>
        /// <param name="id">规则编号</param>
        /// <returns>视图</returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.LG1015101)]
        public ActionResult FreightModuleCreate(int? id)
        {
            LgFreightModule model;
            List<Hyt.Model.Generated.LgFreightValuationModule> list = new List<Model.Generated.LgFreightValuationModule>();
            if (id.HasValue)
            {
                model = LgFreightModuleBo.Instance.GetEntity(id.Value);
                list = LgFreightValuationModuleBo.Instance.GetFreightModel(id.Value);
            }
            else
            {
                model = new LgFreightModule();
            }
            ViewBag.ValuationList = list;
            return View(model);
        }
        /// <summary>
        /// 获取城市层级第一级内容
        /// </summary>
        /// <returns></returns>
        [Privilege(PrivilegeCode.LG1015101)]
        public JsonResult GetFreightModelBySysArea()
        {
            IList<BsArea> areaList = BasicAreaBo.Instance.SelectArea(0);
            return Json(areaList);
        }

        /// <summary>
        /// 运送地区
        /// </summary>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.LG1015101)]
        public ActionResult FreightModuleArea()
        {
            ViewData["id"] = this.Request["id"];
            return View();
        }

        /// <summary>
        /// 商品地址
        /// </summary>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.LG1015101)]
        public ActionResult ProductAddress()
        {
            ViewData["id"] = this.Request["id"];
            return View();
        }
        /// <summary>
        /// 保存运费模板信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns>字符串</returns>
        /// <remarks>
        /// 2015-08-26 王耀发 创建
        /// 2015-11-22 杨浩 重构
        /// </remarks>

        [HttpPost]
        [Privilege(PrivilegeCode.LG1015101)]
        public ActionResult SaveFreightModule()
        {

            Result result = new Result();
            try
            {
                LgFreightModule model = new LgFreightModule();
                model.SysNo = int.Parse(Request.Form["SysNo"]);
                model.ModuleCode = Request.Form["ModuleCode"];
                model.ModuleName = Request.Form["ModuleName"];
                model.ProductAddress = int.Parse(Request.Form["ProductAddress"]);
                model.DeliveryTime = Request.Form["DeliveryTime"];
                model.IsPost = int.Parse(Request.Form["IsPost"]);
                model.ValuationStyle = int.Parse(Request.Form["ValuationStyle"]);
                model.Express = int.Parse(Request.Form["Express"]);
                model.EMS = int.Parse(Request.Form["EMS"]);
                model.SurfaceMail = int.Parse(Request.Form["SurfaceMail"]);

                ///运费模板保价 2015-11-27 杨云奕
                model.IsValuation = int.Parse(Request.Form["IsValuation"]);
                decimal tValuationRate = 0;
                decimal tValuationMaxValue = 0;
                //decimal.TryParse(Request.Form["ValuationRate"].ToString(), out tValuationRate);
                //decimal.TryParse(Request.Form["ValuationMaxValue"].ToString(), out tValuationMaxValue);
                model.ValuationRate = tValuationRate;
                model.ValuationMaxValue = tValuationMaxValue;

                ///运费模板列表定义
                string valuationList = Request.Form["valuationList"];

                ///体积换算重量公式定义保存
                model.IsTurnVtW = int.Parse(Request.Form["IsTurnVtW"]);
                model.TurnVtWType = Request.Form["TurnVtWType"];
                model.TurnVtWFormula = Request.Form["TurnVtWFormula"];

                ///验证体积换算公式是否正确
                if (model.IsTurnVtW > 0)
                {
                    string tempTurnVtWFormula = model.TurnVtWFormula;
                    if (Hyt.Util.Extension.MathExtension.CheckFormulaTips(tempTurnVtWFormula, "[立方米]"))
                    {
                        tempTurnVtWFormula = Hyt.Util.Extension.MathExtension.FormulaChange("[立方米]", tempTurnVtWFormula, 1);
                    }
                    if (Hyt.Util.Extension.MathExtension.CheckFormulaTips(tempTurnVtWFormula, "[立方分米]"))
                    {
                        tempTurnVtWFormula = Hyt.Util.Extension.MathExtension.FormulaChange("[立方分米]", tempTurnVtWFormula, 1);
                    }
                    if (Hyt.Util.Extension.MathExtension.CheckFormulaTips(tempTurnVtWFormula, "[立方厘米]"))
                    {
                        tempTurnVtWFormula = Hyt.Util.Extension.MathExtension.FormulaChange("[立方厘米]", tempTurnVtWFormula, 1);
                    }
                    string msg = "";
                    Hyt.Util.Extension.MathExtension.FormulaCalculate(tempTurnVtWFormula, ref msg);
                    if (msg != "成功")
                    {
                        result.Message = msg;
                        result.Status = false;
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }

                //快递制定城市价格集合  城市ID1,城市ID2|运费模板价格字段1$运费模板价格字段2<运费模板价格编号>运费模板详情编号-...
                string FreightExpress = Server.UrlDecode(Request.Form["FreightExpress"]);
                string delLgFreightModuleItemPricesIds = Request.Form["delLgFreightModuleItemPricesIds"];
                string delLgFreightModuleDetailsIds = Request.Form["delLgFreightModuleDetailsIds"];
                string delLgFreightModuleItemValuationIds = Request.Form["delLgFreightModuleItemValuationIds"];


                //添加本地事务
                using (var tran = new TransactionScope())
                {
                    result = LgFreightModuleBo.Instance.SaveFreightModule(model, CurrentUser.Base);

                    /*自定义运费*/
                    if (model.IsPost == 1)
                    {
                        string[] ExpressArray = FreightExpress.Split('-');//将城市和价格重量分割成数组
                        LgFreightModuleDetails modelDetails = new LgFreightModuleDetails();

                        foreach (string sArray in ExpressArray)
                        {
                            #region 保存运费模板项 LgFreightModuleDetails
                            modelDetails.SysNo = int.Parse(sArray.Split('>')[1]);
                            string[] cityAndPrice = sArray.Split('>')[0].Split('|');
                            //地区项
                            string cityIds = cityAndPrice[0];

                            string prices = cityAndPrice[1];
                            //地区项对应的价格列表
                            string[] pricesArray = prices.Split(',');
                            modelDetails.FreightModuleSysNo = result.StatusCode;
                            modelDetails.IsPost = model.IsPost;
                            modelDetails.ValuationStyle = model.ValuationStyle;
                            modelDetails.DeliveryStyle = model.Express;
                            modelDetails.DeliveryArea = cityIds;

                            modelDetails.First = 0;
                            modelDetails.FirstPayment = 0;
                            modelDetails.Next = 0;
                            modelDetails.NextPayment = 0;
                            modelDetails.Offset = 0;

                            Result freightModuleDetailsResult = LgFreightModuleDetailsBo.Instance.SaveFreightModuleDetails(modelDetails, CurrentUser.Base);
                            #endregion

                            #region  新增和保存运费计算规则 FreightModuleItemPrices
                            LgFreightModuleItemPrices freightModuleItemPrices = new LgFreightModuleItemPrices();
                            foreach (var price in pricesArray)
                            {
                                freightModuleItemPrices.SysNo = int.Parse(price.Split('<')[1]);
                                var singlePrice = price.Split('<')[0].Split('$');

                                freightModuleItemPrices.LowestCost = decimal.Parse(singlePrice[0]);
                                freightModuleItemPrices.ServiceCost = decimal.Parse(singlePrice[1]);
                                freightModuleItemPrices.FirstWeight = decimal.Parse(singlePrice[2]);
                                freightModuleItemPrices.FirstCost = decimal.Parse(singlePrice[3]);
                                freightModuleItemPrices.ContinuousWeight = decimal.Parse(singlePrice[4]);
                                freightModuleItemPrices.ContinuousCost = decimal.Parse(singlePrice[5]);
                                freightModuleItemPrices.Offset = decimal.Parse(singlePrice[6]);
                                freightModuleItemPrices.MinWeight = decimal.Parse(singlePrice[7]);
                                freightModuleItemPrices.MaxWeight = decimal.Parse(singlePrice[8]);

                                freightModuleItemPrices.FreightModuleDetailsSysNo = freightModuleDetailsResult.StatusCode;
                                if (freightModuleItemPrices.SysNo <= 0)
                                    FreightModuleItemPricesBo.Instance.AddFreightModuleItemPrices(freightModuleItemPrices);
                                else
                                    FreightModuleItemPricesBo.Instance.UpdateFreightModuleItemPrices(freightModuleItemPrices);
                            }
                            #endregion
                        }
                    }

                    #region 保价定义操作控制
                    string[] valuationTrList = valuationList.Split('|');

                    foreach (string tr in valuationTrList)
                    {
                        if (tr != "")
                        {
                            string trId = tr.Split('<')[1];
                            string trData = tr.Split('<')[0];
                            string[] columData = trData.Split('$');
                            LgFreightValuationModule valuaMod = new LgFreightValuationModule();
                            valuaMod.SysNo = Convert.ToInt32(trId);
                            valuaMod.lgfvm_pid = result.StatusCode;
                            valuaMod.lgfvm_title = columData[0];
                            valuaMod.lgfvm_AreaSysNo = columData[1];

                            decimal tryValue = 0;
                            decimal.TryParse(columData[2], out tryValue);
                            valuaMod.lgfvm_MinValua = tryValue;

                            decimal.TryParse(columData[3], out tryValue);
                            valuaMod.lgfvm_MaxValua = tryValue;

                            valuaMod.lgfvm_ValueType = columData[4];

                            decimal.TryParse(columData[5], out tryValue);
                            valuaMod.lgfvm_FreightValue = tryValue;

                            LgFreightValuationModuleBo.Instance.InnerOrUpdateFreightModel(valuaMod);
                        }
                    }
                    #endregion

                    #region 删除运费计算规则和运费模板项
                    if (delLgFreightModuleItemPricesIds != "")
                        FreightModuleItemPricesBo.Instance.DeleteFreightModuleItemPricesBySysNos(delLgFreightModuleItemPricesIds);
                    if (delLgFreightModuleDetailsIds != "")
                        LgFreightModuleDetailsBo.Instance.DeleteBySysNos(delLgFreightModuleDetailsIds);
                    #endregion

                    #region 删除被移除的保价定义id
                    if (!string.IsNullOrEmpty(delLgFreightModuleItemValuationIds))
                    {
                        LgFreightValuationModuleBo.Instance.DeleteFreightModel(delLgFreightModuleItemValuationIds);
                    }
                    #endregion

                    result.Message = "保存成功！";

                    tran.Complete();
                }

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 审核运费模板
        /// </summary>
        /// <param name="id">模板编号</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2015-08-26 王耀发 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.LG1015101)]
        public ActionResult AuditFreightModule(int id, int status = 20)
        {
            Result r = new Result();
            try
            {
                r = LgFreightModuleBo.Instance.AuditFreightModule(id, CurrentUser.Base, status);
            }
            catch (Exception ex)
            {
                r.Status = false;
                r.Message = ex.Message;
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 作废运费模板
        /// </summary>
        /// <param name="id">模板编号</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2015-08-26 王耀发 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.LG1015101)]
        public ActionResult CancelFreightModule(int id)
        {
            Result r = new Result();
            try
            {
                r = LgFreightModuleBo.Instance.CancelFreightModule(id, CurrentUser.Base);
            }
            catch (Exception ex)
            {
                r.Status = false;
                r.Message = ex.Message;
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 运费模板
        /// </summary>
        /// <param></param>
        /// <returns>展示运费模板模型</returns>
        /// <remarks>2015-08-26 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.LG1015101)]
        public ActionResult FreightModuleSelector()
        {
            return View();
        }
        #endregion

        #region 快递单查询 廖移凤 2017-11-29

        /// <summary>
        /// 快递单查询页面
        /// </summary>
        /// <returns></returns>
        [Privilege(PrivilegeCode.None)]
        public ActionResult QueryExpress(int? id)
        {

            Pager<KuaiDiNumQuery> page = new Pager<KuaiDiNumQuery>();
            page.CurrentPage = id ?? 1;
            page.PageSize = 10;
            var query = BLL.ExpressList.ExpressListBLL.Instance.GetPage(page);
            PagedList<KuaiDiNumQuery> pagedList = new PagedList<KuaiDiNumQuery>();
            pagedList.CurrentPageIndex = page.CurrentPage;
            pagedList.TotalItemCount = page.TotalRows;
            pagedList.TData = page.Rows;
            pagedList.CurrentPageIndex = page.CurrentPage;
            pagedList.PageSize = page.PageSize;
            return View(pagedList);
        }

        /// <summary>
        /// 快递单查询
        /// </summary>
        /// <returns></returns>
        [Privilege(PrivilegeCode.None)]
        public JsonResult GetAll(int? id)
        {

            //var query = BLL.ExpressList.ExpressListBLL.Instance.GetKuaiDiNumQuery();
            Pager<KuaiDiNumQuery> page = new Pager<KuaiDiNumQuery>();
            page.CurrentPage = id ?? 1;
            page.PageSize = 10;
            var query = BLL.ExpressList.ExpressListBLL.Instance.GetPage(page);
            PagedList<KuaiDiNumQuery> pagedList = new PagedList<KuaiDiNumQuery>();
            pagedList.CurrentPageIndex = page.CurrentPage;
            pagedList.TotalItemCount = page.TotalRows;
            pagedList.TData = page.Rows;
            pagedList.CurrentPageIndex = page.CurrentPage;
            pagedList.PageSize = page.PageSize;

            return Json(pagedList, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 删除快递单
        /// </summary>
        /// <param name="KuaiDiNum"></param>
        /// <returns></returns>
        [Privilege(PrivilegeCode.None)]
        public ActionResult DeleteKuaiDiNum(int KuaiDiNum,int ? id)
        {
            var test = BLL.ExpressList.ExpressListBLL.Instance.DeleteKuaiDiNum(KuaiDiNum);

            return Content(test.ToString());
        }


        /// <summary>
        /// 搜索快递单
        /// </summary>
        /// <returns></returns>
        [Privilege(PrivilegeCode.None)]
        public JsonResult SelectKuaiDiNum(int KuaiDiNum)
        {

            var query = BLL.ExpressList.ExpressListBLL.Instance.SelectKuaiDiNum(KuaiDiNum);

            return Json(query, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// 将快递单导出为Excel
        /// </summary>
        /// <returns></returns>
        /// <remarks>2013-07-26 沈强 创建</remarks>
        [Privilege(PrivilegeCode.None)]
        public void KuaiDiNumExcel()
        {
            List<KuaiDiNumQuery> query = BLL.ExpressList.ExpressListBLL.Instance.GetKuaiDiNumQuery();

            //导出Excel，并设置表头列名
            Util.ExcelUtil.Export<KuaiDiNumQuery>(query, new List<string>()
                    {
                        "快递单号",
                        "出库单号",
                        "订单号",
                        "收货人",
                        "金额",
                        "状态",
                        "配送方式",
                    });

        }

        #endregion
    }
}
