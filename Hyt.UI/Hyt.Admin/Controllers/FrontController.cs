using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hyt.BLL.Authentication;
using Hyt.BLL.Front;
using Hyt.BLL.LevelPoint;
using Hyt.BLL.Log;
using Hyt.BLL.Sys;
using Hyt.Infrastructure.Caching;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.SystemPredefined;
using Hyt.Model.WorkflowStatus;
using Hyt.Util;
using Hyt.Admin;
using System.Web.Script.Serialization;
using Hyt.Model.Parameter;

namespace Hyt.Admin.Controllers
{
    /// <summary>
    /// 前台管理
    /// </summary>
    /// <remarks></remarks>
    public class FrontController : BaseController
    {

        #region 广告管理

        #region 广告组

        /// <summary>
        /// 广告分组列表
        /// </summary>
        /// <param name="id">分页索引</param>
        /// <param name="type">类型</param>
        /// <param name="status">状态</param>
        /// <param name="platformType">平台</param>
        /// <param name="commentSysNo">广告组编号</param>
        /// <param name="key"></param>
        /// <returns>返回列表</returns>
        /// <remarks>2013-06-17 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.FE1008001)]
        public ActionResult FeAdvertGroup(int? id, int? type, int? status, int? platformType, int? commentSysNo, string key = "")
        {
            IDictionary<int, string> typeList = EnumUtil.ToDictionary(typeof(ForeStatus.广告组类型));
            ViewBag.TypeList = typeList;
            IDictionary<int, string> statusList = EnumUtil.ToDictionary(typeof(ForeStatus.广告组状态));
            ViewBag.StatusList = statusList;
            //删除任务DeleteJobPool
            if (commentSysNo != null)
                Hyt.BLL.Sys.SyJobPoolManageBo.Instance.DeleteJobPool((int)commentSysNo, (int)SystemStatus.任务对象类型.通知);
            //广告组平台类型
            IDictionary<int, string> platformList = EnumUtil.ToDictionary(typeof(ForeStatus.广告组平台类型));
            ViewBag.PlatformList = platformList;

            if (platformType == null)
                platformType = -1;

            id = id ?? 1;
            var model = FeAdvertGroupBO.Instance.Seach((int)id, type, status, (int)platformType, key);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_AjaxFeAdvertGroup", model);
            }
            return View(model);
        }

        /// <summary>
        /// 广告组添加视图
        /// </summary>
        /// <returns>空视图</returns>
        /// <remarks>2013-06-17 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.FE1008201)]
        public ActionResult FeAdvertGroupAdd()
        {
            IDictionary<int, string> dictList = EnumUtil.ToDictionary(typeof(ForeStatus.广告组类型));
            ViewBag.DictList = dictList;

            #region 平台类型 黄伟 2013-9-2 添加

            var lstPlatformType = new List<SelectListItem> { new SelectListItem { Selected = true, Text = @"请选择", Value = "-1" } };
            lstPlatformType.AddRange(Enum.GetNames(typeof(ForeStatus.广告组平台类型)).Select(name => new SelectListItem { Selected = false, Text = name, Value = Enum.Parse(typeof(ForeStatus.广告组平台类型), name).GetHashCode() + "" }).ToList());
            ViewBag.lstPlatformType = lstPlatformType;

            #endregion

            var model = new FeAdvertGroup();
            return View(model);
        }

        /// <summary>
        /// 广告组修改视图
        /// </summary>
        /// <param name="id">广告组编号</param>
        /// <returns>空视图</returns>
        /// <remarks>2013-06-17 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.FE1008201)]
        public ActionResult FeAdvertGroupUpdate(int id)
        {
            IDictionary<int, string> dictList = EnumUtil.ToDictionary(typeof(ForeStatus.广告组类型));
            ViewBag.DictList = dictList;

            //广告组平台类型
            IDictionary<int, string> platformList = EnumUtil.ToDictionary(typeof(ForeStatus.广告组平台类型));
            ViewBag.PlatformList = platformList;

            FeAdvertGroup model = FeAdvertGroupBO.Instance.GetModel(id);
            return View(model);
        }

        /// <summary>
        /// 广告组增加或修改
        /// </summary>
        /// <returns>保存失败或成功信息</returns>
        /// <remarks>2013-06-18 苟治国 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.FE1008201)]
        public ActionResult FeAdvertGroupAddOrEdit()
        {
            bool result = false;
            string info = "操作失败";

            string sysNo = Request.Form["hidSysNo"];
            string name = Request.Form["Name"];
            string type = Request.Form["Type"];
            string code = Request.Form["Code"];
            string displayOrder = Request.Form["DisplayOrder"];
            string platformType = Request.Form["selPlatformType"];
            var model = new FeAdvertGroup();
            if (!sysNo.Equals("0"))
            {
                model = FeAdvertGroupBO.Instance.GetModel(Convert.ToInt32(sysNo));
                model.Name = name;
                model.Code = code;
                model.DisplayOrder = Convert.ToInt32(displayOrder);
                model.LastUpdateDate = DateTime.Now;
                model.LastUpdateBy = CurrentUser.Base.SysNo;
                model.PlatformType = Convert.ToInt32(platformType);
            }
            else
            {
                model.Name = name;
                model.Code = code;
                model.Type = Convert.ToInt32(type);
                model.PlatformType = Convert.ToInt32(platformType);
                model.CreatedBy = CurrentUser.Base.SysNo;
                model.CreatedDate = DateTime.Now;
                model.DisplayOrder = Convert.ToInt32(displayOrder);
            }

            if (!sysNo.Equals("0"))
            {
                if (FeAdvertGroupBO.Instance.Update(model) > 0)
                {
                    result = true;
                    info = "修改广告组成功！";
                }
                else
                {
                    result = false;
                    info = "修改广告组失败！";
                }
            }
            else
            {
                if (FeAdvertGroupBO.Instance.Insert(model) > 0)
                {
                    result = true;
                    info = "添加广告组成功！";
                }
                else
                {
                    result = false;
                    info = "添加广告组失败！";
                }
            }
            return Json(new { result = result, info = info });
        }

        /// <summary>
        /// 广告组验证广告名称
        /// </summary>
        /// <returns>返回真、假</returns>
        /// <remarks>2013-07-05 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.FE1008201)]
        public ActionResult FeAdvertGroupKeyChk()
        {
            bool result = false;

            string name = Request.Form["Name"];
            string hidSysNo = Request.Form["hidSysNo"];
            if (BLL.Front.FeAdvertGroupBO.Instance.FeAdvertGroupChk(name, Convert.ToInt32(hidSysNo)) > 0)
            {
                result = false;
            }
            else
            {
                result = true;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 单条更新状态
        /// </summary>
        /// <param name="collocation">广告组视图</param>
        /// <returns>更新广告项状态</returns>
        /// <remarks>2013-06-25 苟治国 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.FE1008601)]
        public ActionResult FeAdvertGroupStatus(IList<FeAdvertGroup> collocation)
        {
            bool result = false;
            string info = "操作失败";

            string str = "";
            if (collocation[0].Status == (int)ForeStatus.广告组状态.启用)
                str = "启用";
            else
                str = "禁用";
            if (FeAdvertGroupBO.Instance.UpdateStatus(collocation) > 0)
            {
                result = true;
                info = str + "广告组成功！";
            }
            else
            {
                result = true;
                info = str + "广告组失败！";
            }
            return Json(new { result = result, info = info });
        }

        /// <summary>
        /// 批量更新状态
        /// </summary>
        /// <param name="collocation">广告组视图</param>
        /// <returns>更新广告项状态</returns>
        /// <remarks>2013-06-25 苟治国 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.FE1008601)]
        public ActionResult FeAdvertGroupStatusAll(IList<FeAdvertGroup> collocation)
        {
            bool result = false;
            string info = "操作失败";
            if (FeAdvertGroupBO.Instance.UpdateStatus(collocation) > 0)
            {
                result = true;
                info = "批量启用成功！";
            }
            else
            {
                result = true;
                info = "批量启用失败！";
            }
            return Json(new { result = result, info = info });
        }
        #endregion

        #region 广告项

        /// <summary>
        ///     广告项列表
        /// </summary>
        /// <param name="groupSysNo">广告组编号</param>
        /// <param name="id">索引</param>
        /// <param name="status">状态</param>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param> 
        /// <param name="commentSysNo">广告项编号</param>
        /// <param name="linkTitle">搜索关键字</param>
        /// <returns>广告项列表</returns>
        /// <remarks>2013-06-17 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.FE1008002, PrivilegeCode.FE1008202)]
        public ActionResult FeAdvertItem(int groupSysNo, int? id, int? status, DateTime? beginDate, DateTime? endDate, int? commentSysNo,int SelectedDealerSysNo = -1, string linkTitle = null)
        {
            id = id ?? 1; //索引
            if (status == null)
                status = (int)ForeStatus.广告项状态.待审;
            else
                status = status ?? -1;
            //当前分组编号
            ViewBag.GroupSysNo = groupSysNo.ToString();
            //删除任务DeleteJobPool
            if (commentSysNo != null)
                Hyt.BLL.Sys.SyJobPoolManageBo.Instance.DeleteJobPool((int)commentSysNo, (int)SystemStatus.任务对象类型.通知);
            //菜单
            IDictionary<int, string> statusList = EnumUtil.ToDictionary(typeof(ForeStatus.广告项状态));
            ViewBag.StatusList = statusList;
            //当前分组类型
            FeAdvertGroup group = FeAdvertGroupBO.Instance.GetModel(groupSysNo);
            ViewBag.Type = group.Type;

            var filter = new ParaAdvertItemFilter { id = (int)id, groupSysNo = groupSysNo, status = (int)status, beginDate = beginDate, endDate = endDate, SelectedDealerSysNo = SelectedDealerSysNo,linkTitle = linkTitle };
            //当前用户对应分销商，2015-12-19 王耀发 创建
            if (CurrentUser.IsBindDealer)
            {
                int DealerSysNo = CurrentUser.Dealer.SysNo;
                filter.DealerSysNo = DealerSysNo;
                filter.IsBindDealer = CurrentUser.IsBindDealer;
            }
            //是否绑定所有经销商
            filter.IsBindAllDealer = CurrentUser.IsBindAllDealer;
            filter.DealerCreatedBy = CurrentUser.Base.SysNo;

            var model = FeAdvertItemBo.Instance.Seach(filter);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_AjaxFeAdvertItem", model);
            }
            return View(model);
        }

        /// <summary>
        ///     添加/修改广告文字
        /// </summary>
        /// <param name="id">广告项编号</param>
        /// <param name="mid">广告组编号</param>
        /// <returns>添加/修改广告文字视图</returns>
        /// <remarks>2013-06-19 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.FE1008202)]
        public ActionResult FeAdvertItemAddOrUpdateAndFont(int? id, int? mid)
        {
            IDictionary<int, string> dictList = EnumUtil.ToDictionary(typeof(ForeStatus.广告组类型));
            ViewBag.DictList = dictList;

            if (!string.IsNullOrEmpty(id.ToString()))
            {
                FeAdvertItem model = FeAdvertItemBo.Instance.GetModel((int)id);
                return View(model);
            }
            else
            {
                var model = new FeAdvertItem();
                ViewBag.mid = mid;
                return View(model);
            }
        }

        /// <summary>
        ///     广告项文字增加或修改
        /// </summary>
        /// <returns>保存失败或成功信息</returns>
        /// <remarks>2013-06-18 苟治国 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.FE1008202)]
        public ActionResult FeAdvertItemAddOrEditAndFont()
        {
            bool result = false;
            string info = "操作失败";

            string sysNo = Request.Form["hidSysNo"];
            string sysMid = Request.Form["hidMid"];
            string Name = Request.Form["Name"];
            string Content = Request.Form["Content"];
            string BeginDate = Request.Form["BeginDate"];
            string EndDate = Request.Form["EndDate"];
            string DisplayOrder = Request.Form["DisplayOrder"];

            string Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var model = new FeAdvertItem();
            if (!sysNo.Equals("0"))
            {
                model = FeAdvertItemBo.Instance.GetModel(Convert.ToInt32(sysNo));
                model.LastUpdateDate = Convert.ToDateTime(Date);
                model.LastUpdateBy = CurrentUser.Base.SysNo;
            }
            else
            {
                model.GroupSysNo = Convert.ToInt32(sysMid);
                model.Status = (int)ForeStatus.广告项状态.待审;
                model.CreatedDate = Convert.ToDateTime(Date);
                model.CreatedBy = CurrentUser.Base.SysNo;
            }
            model.Name = Name;
            model.Content = Content;
            model.BeginDate = Convert.ToDateTime(BeginDate);
            model.EndDate = Convert.ToDateTime(EndDate);
            model.DisplayOrder = Convert.ToInt32(DisplayOrder);

            if (!sysNo.Equals("0"))
            {
                if (FeAdvertItemBo.Instance.Update(model) > 0)
                {
                    result = true;
                    info = "修改广告项成功！";
                }
                else
                {
                    result = false;
                    info = "修改广告项失败！";
                }
            }
            else
            {
                if (FeAdvertItemBo.Instance.Insert(model) > 0)
                {
                    result = true;
                    info = "添加广告项成功！";
                }
                else
                {
                    result = false;
                    info = "添加广告失败！";
                }
            }
            return Json(new { result = result, info = info });
        }

        /// <summary>
        /// 添加/修改广告图片
        /// </summary>
        /// <param name="id">广告项编号</param>
        /// <param name="mid">广告组编号</param>
        /// <returns>添加/修改广告图片视图</returns>
        /// <remarks>2013-06-19 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.FE1008202)]
        public ActionResult FeAdvertItemAddOrUpdateAndImg(int? id, int? mid)
        {
            IDictionary<int, string> dictList = EnumUtil.ToDictionary(typeof(ForeStatus.广告组类型));
            ViewBag.DictList = dictList;

            IDictionary<int, string> OpenList = EnumUtil.ToDictionary(typeof(ForeStatus.广告打开方式));
            ViewBag.OpenList = OpenList;

            if (!string.IsNullOrEmpty(id.ToString()))
            {
                FeAdvertItem model = FeAdvertItemBo.Instance.GetModel((int)id);
                return View(model);
            }
            else
            {
                var model = new FeAdvertItem();
                ViewBag.mid = mid;
                return View(model);
            }
        }

        /// <summary>
        ///     广告项图片增加或修改
        /// </summary>
        /// <returns>保存失败或成功信息</returns>
        /// <remarks>2013-06-18 苟治国 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.FE1008202)]
        public ActionResult FeAdvertItemAddOrEditAndImg()
        {
            bool result = false;
            string info = "操作失败";

            string sysNo = Request.Form["hidSysNo"];
            string sysMid = Request.Form["hidMid"];
            string ImageUrl = Request.Form["ImageUrl"];
            string LinkUrl = Request.Form["LinkUrl"];
            string LinkTitle = Request.Form["LinkTitle"];
            string BeginDate = Request.Form["BeginDate"];
            string EndDate = Request.Form["EndDate"];
            string OpenType = Request.Form["OpenType"];
            string DisplayOrder = Request.Form["DisplayOrder"];
            string DealerSysNo = Request.Form["DealerSysNo"];

            string Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var model = new FeAdvertItem();

            if (!sysNo.Equals("0"))
            {
                model = FeAdvertItemBo.Instance.GetModel(Convert.ToInt32(sysNo));
                model.LastUpdateDate = Convert.ToDateTime(Date);
            }
            else
            {
                model.GroupSysNo = Convert.ToInt32(sysMid);
                model.CreatedBy = CurrentUser.Base.SysNo;
                model.Status = (int)ForeStatus.广告项状态.待审;
                model.CreatedDate = Convert.ToDateTime(Date);
                model.LastUpdateDate = model.LastUpdateDate == DateTime.MinValue ? (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue : model.LastUpdateDate;
            }
            model.ImageUrl = ImageUrl;
            model.LinkUrl = LinkUrl;
            model.LinkTitle = LinkTitle;
            model.BeginDate = Convert.ToDateTime(BeginDate);
            model.EndDate = Convert.ToDateTime(EndDate);
            model.OpenType = Convert.ToInt32(OpenType);
            model.DisplayOrder = Convert.ToInt32(DisplayOrder);
            model.DealerSysNo = int.Parse(DealerSysNo);
            if (!sysNo.Equals("0"))
            {
                if (FeAdvertItemBo.Instance.Update(model) > 0)
                {
                    result = true;
                    info = "修改广告项成功！";
                }
                else
                {
                    result = false;
                    info = "修改广告项失败！";
                }
            }
            else
            {
                if (FeAdvertItemBo.Instance.Insert(model) > 0)
                {
                    result = true;
                    info = "添加广告项成功！";
                }
                else
                {
                    result = false;
                    info = "添加广告项失败！";
                }
            }
            return Json(new { result = result, info = info });
        }

        /// <summary>
        /// 批量更新状态
        /// </summary>
        /// <param name="collocation">广告项编号</param>
        /// <returns>更新广告项状态</returns>
        /// <remarks>2013-06-25 苟治国 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.FE1008602, PrivilegeCode.FE1008202)]
        public ActionResult FeAdvertItemStatus(IList<FeAdvertItem> collocation)
        {
            bool result = false;
            string info = "操作失败";
            if (FeAdvertItemBo.Instance.UpdateStatus(collocation) > 0)
            {
                result = true;
                info = "置为成功！";
            }
            else
            {
                result = true;
                info = "置为失败！";
            }
            return Json(new { result = result, info = info });
        }
        #endregion

        #endregion

        #region 商品管理

        #region 商品组

        /// <summary>
        /// 商品组列表
        /// </summary>
        /// <param name="id">分页索引</param>
        /// <param name="status">状态</param>
        /// <param name="platformType">平台类型</param>
        /// <param name="commentSysNo">商品组编号</param>
        /// <param name="key">搜索关键字</param>
        /// <returns>商品组页面</returns>
        /// <remarks>2013-06-20 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.FE1009001, PrivilegeCode.FE1009201)]
        public ActionResult FeProductGroup(int? id, int? status, int? platformType, int? commentSysNo, string key = null)
        {
            //商品组状态
            IDictionary<int, string> statusList = EnumUtil.ToDictionary(typeof(ForeStatus.商品组状态));
            ViewBag.StatusList = statusList;
            //商品组平台类型
            IDictionary<int, string> platformList = EnumUtil.ToDictionary(typeof(ForeStatus.商品组平台类型));
            ViewBag.PlatformList = platformList;
            //删除任务DeleteJobPool
            if (commentSysNo != null)
                Hyt.BLL.Sys.SyJobPoolManageBo.Instance.DeleteJobPool((int)commentSysNo, (int)SystemStatus.任务对象类型.通知);
            if (platformType == null)
                platformType = -1;

            id = id ?? 1;
            var model = FeProductGroupBo.Instance.Seach((int)id, platformType, status, key);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_AjaxFeProductGroup", model);
            }
            return View(model);
        }

        /// <summary>
        /// 新增商品组
        /// </summary>
        /// <returns>空视图</returns>
        /// <remarks>2013-06-21 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.FE1009201)]
        public ActionResult FeProductGroupAdd()
        {
            IDictionary<int, string> statusList = EnumUtil.ToDictionary(typeof(ForeStatus.商品组状态));
            ViewBag.StatusList = statusList;

            #region 平台类型 黄伟 2013-9-2 添加

            var lstPlatformType = new List<SelectListItem> { new SelectListItem { Selected = true, Text = @"请选择", Value = "-1" } };
            lstPlatformType.AddRange(Enum.GetNames(typeof(ForeStatus.商品组平台类型)).Select(name => new SelectListItem { Selected = false, Text = name, Value = Enum.Parse(typeof(ForeStatus.商品组平台类型), name).GetHashCode() + "" }).ToList());
            ViewBag.lstPlatformType = lstPlatformType;

            #endregion

            var product = new FeProductGroup();
            return View(product);
        }

        /// <summary>
        ///     更新商品组
        /// </summary>
        /// <param name="id">商品组编号</param>
        /// <returns>空视图</returns>
        /// <remarks>2013-06-21 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.FE1009201)]
        public ActionResult FeProductGroupUpdate(int id)
        {
            FeProductGroup model = FeProductGroupBo.Instance.GetModel(id);

            //商品组平台类型
            IDictionary<int, string> platformList = EnumUtil.ToDictionary(typeof(ForeStatus.商品组平台类型));
            ViewBag.PlatformList = platformList;
            return View(model);
        }

        /// <summary>
        ///     商品增加或修改
        /// </summary>
        /// <returns>保存失败或成功信息</returns>
        /// <remarks>2013-06-21 苟治国 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.FE1009201)]
        public ActionResult FeProductGroupAddOrEdit()
        {
            bool result = false;
            string info = "操作失败";

            string sysNo = Request.Form["hidSysNo"];
            string name = Request.Form["Name"];
            string code = Request.Form["Code"];
            string displayOrder = Request.Form["DisplayOrder"];
            string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //商品图标和平台类型
            string productGroupIcon = Request.Form["txtImgUrl"];
            string platformType = Request.Form["selPlatformType"];

            var model = new FeProductGroup();
            if (!sysNo.Equals("0"))
            {
                model = FeProductGroupBo.Instance.GetModel(Convert.ToInt32(sysNo));
                model.Name = name;
                model.Code = code;
                model.ProductGroupIcon = productGroupIcon;
                model.DisplayOrder = Convert.ToInt32(displayOrder);
                model.LastUpdateBy = CurrentUser.Base.SysNo;
                model.LastUpdateDate = Convert.ToDateTime(date);
                model.PlatformType = Convert.ToInt32(platformType);
                model.ProductGroupIcon = productGroupIcon;
            }
            else
            {
                //验证数据是否合法
                if (platformType.Equals("-1"))
                    return Json(new { result = false, info = "参数错误，请刷新重新提交！" });
                model.Name = name;
                model.Code = code;
                model.DisplayOrder = Convert.ToInt32(displayOrder);
                model.CreatedDate = Convert.ToDateTime(date);
                model.CreatedBy = CurrentUser.Base.SysNo;
                model.Status = (int)ForeStatus.商品组状态.禁用;
                model.PlatformType = Convert.ToInt32(platformType);
                model.ProductGroupIcon = productGroupIcon;
            }

            if (!sysNo.Equals("0"))
            {
                if (FeProductGroupBo.Instance.Update(model) > 0)
                {
                    result = true;
                    info = "修改商品组成功！";
                }
                else
                {
                    result = false;
                    info = "修改商品组失败！";
                }
            }
            else
            {
                if (FeProductGroupBo.Instance.Insert(model) > 0)
                {
                    result = true;
                    info = "添加商品组成功！";
                }
                else
                {
                    result = false;
                    info = "添加商品组失败！";
                }
            }

            return Json(new { result = result, info = info });
        }

        /// <summary>
        /// 验证商品组名称
        /// </summary>
        /// <returns>返回真、假</returns>
        /// <remarks>2013-07-05 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.FE1009201)]
        public ActionResult FeProductGroupKeyChk()
        {
            bool result = false;
            string info = "操作失败";

            string name = Request.Form["Name"];
            string hidSysNo = Request.Form["hidSysNo"];
            if (BLL.Front.FeProductGroupBo.Instance.FeProductGroupChk(name, Convert.ToInt32(hidSysNo)) > 0)
            {
                result = false;
            }
            else
            {
                result = true;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 单条记录更新
        /// </summary>
        /// <param name="collocation">商品组视图</param>
        /// <returns>更新商品组状态</returns>
        /// <remarks>2013-06-25 苟治国 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.FE1009601)]
        public ActionResult FeProductStatus(IList<FeProductGroup> collocation)
        {
            string str = "";
            if (collocation[0].Status == (int)ForeStatus.商品组状态.启用)
                str = "启用";
            else
                str = "禁用";

            bool result = false;
            string info = "操作失败";
            if (FeProductGroupBo.Instance.UpdateStatus(collocation) > 0)
            {
                result = true;
                info = str + "商品组成功！";
            }
            else
            {
                result = true;
                info = str + "商品组失败！";
            }
            return Json(new { result = result, info = info });
        }

        /// <summary>
        /// 批量更新状态
        /// </summary>
        /// <param name="collocation">商品组视图</param>
        /// <returns>更新商品组状态</returns>
        /// <remarks>2013-06-25 苟治国 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.FE1009601)]
        public ActionResult UpdateStatus(IList<FeProductGroup> collocation)
        {
            bool result = false;
            string info = "操作失败";
            if (FeProductGroupBo.Instance.UpdateStatus(collocation) > 0)
            {
                result = true;
                info = "批量启用成功！";
            }
            else
            {
                result = true;
                info = "批量启用失败！";
            }
            return Json(new { result = result, info = info });
        }

        #endregion

        #region 商品项

        /// <summary>
        /// 商品项列表
        /// </summary>
        /// <param name="id">分页索引</param>
        /// <param name="groupSysNo">产品组编号</param>
        /// <param name="status">状态</param>
        /// <param name="commentSysNo">商品项编号</param>
        /// <param name="erpCode">关键字【商品ESA编号】</param>
        /// <param name="productName">关键字【商品名称】</param>
        /// <returns>商品项页面</returns>
        /// <remarks>2013-06-20 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.FE1009002, PrivilegeCode.FE1009202)]
        public ActionResult FeProductItem(int? id, int groupSysNo, int? status, int? commentSysNo, string erpCode = null, string productName = null, int SelectedDealerSysNo = -1)
        {
            IDictionary<int, string> statusList = EnumUtil.ToDictionary(typeof(ForeStatus.商品项状态));
            ViewBag.StatusList = statusList;

            var productGroup = BLL.Front.FeProductGroupBo.Instance.GetModel(groupSysNo);
            if (productGroup != null)
            {
                ViewBag.productGroupName = productGroup.Name;
            }

            ViewBag.Mid = groupSysNo;

            id = id ?? 1;
            if (status == null)
                status = -1;
            //删除任务DeleteJobPool
            if (commentSysNo != null)
                Hyt.BLL.Sys.SyJobPoolManageBo.Instance.DeleteJobPool((int)commentSysNo, (int)SystemStatus.任务对象类型.通知);

            var model = FeProductItemBo.Instance.Seach((int)id, groupSysNo, (int)status, erpCode, CurrentUser.Dealer.SysNo, CurrentUser.IsBindDealer, CurrentUser.IsBindAllDealer, CurrentUser.Base.SysNo, productName, SelectedDealerSysNo);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_AjaxFeProductItem", model);
            }
            return View(model);
        }

        /// <summary>
        ///     修改商品项
        /// </summary>
        /// <param name="id">商品项编号</param>
        /// <returns>空视图</returns>
        /// <remarks>2013-06-21 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.FE1009202)]
        public ActionResult FeProductItemUpdate(int id)
        {
            //商品组状态
            IDictionary<int, string> StatusList = EnumUtil.ToDictionary(typeof(ForeStatus.显示标志));
            ViewBag.StatusList = StatusList;

            FeProductItem model = FeProductItemBo.Instance.GetModel(id);
            return View(model);
        }

        /// <summary>
        ///     修改商品项
        /// </summary>
        /// <returns>空视图</returns>
        /// <remarks>2013-06-24 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.FE1009202)]
        public ActionResult FeProductItemEdit()
        {
            bool result = false;
            string info = "操作失败";

            string sysNo = Request.Form["hidSysNo"];
            string DispalySymbol = Request.Form["DispalySymbol"];
            string DisplayOrder = Request.Form["DisplayOrder"];
            var model = new FeProductItem();
            if (!sysNo.Equals("0"))
            {
                model = FeProductItemBo.Instance.GetModel(Convert.ToInt32(sysNo));
                model.DispalySymbol = Convert.ToInt32(DispalySymbol);
                model.DisplayOrder = Convert.ToInt32(DisplayOrder);
                model.LastUpdateBy = CurrentUser.Base.SysNo;
                model.LastUpdateDate = DateTime.Now;
            }

            if (!sysNo.Equals("0"))
            {
                if (FeProductItemBo.Instance.Update(model) > 0)
                {
                    result = true;
                    info = "修改商品项成功！";
                }
                else
                {
                    result = false;
                    info = "修改商品项失败！";
                }
            }
            else
            {
                if (FeProductItemBo.Instance.Insert(model) > 0)
                {
                    result = true;
                    info = "添加商品项成功！";
                }
                else
                {
                    result = false;
                    info = "添加商品项失败！";
                }
            }

            return Json(new { result = result, info = info });
        }

        /// <summary>
        ///     批量插入商品
        /// </summary>
        /// <param name="mid">商品组编号</param>
        /// <param name="collocation">产品集合</param>
        /// <returns>返回插入成功OR失败</returns>
        /// <remarks>2013-06-24 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.FE1009202)]
        public ActionResult FeProductItemInsert(int mid,int dealersysno, string[] collocation)
        {
            bool result = false;
            string info = "操作失败";

            try
            {
                //改为一次判断  2014-09-10 何方
                #region 新的在循环外做判断  
                
                var exsitList = FeProductItemBo.Instance.GetListByGroup(mid,dealersysno);

                var needInsertList = from id in collocation
                                     where !exsitList.Select(x => x.ProductSysNo).ToArray().Contains(int.Parse(id))
                                     select id;

                foreach (var arr in needInsertList)
                {
                     
                        var productItem = new FeProductItem()
                        {
                            GroupSysNo = mid,
                            ProductSysNo = Convert.ToInt32(arr),
                            BeginDate = DateTime.Now,
                            EndDate = DateTime.Now,
                            DispalySymbol = (int)ForeStatus.显示标志.正常,
                            Status = (int)ForeStatus.商品项状态.待审,
                            CreatedBy = CurrentUser.Base.SysNo,
                            CreatedDate = DateTime.Now,
                            LastUpdateDate = DateTime.Now,
                            DealerSysNo = dealersysno
                        };
                        if (FeProductItemBo.Instance.Insert(productItem) > 0)
                        {
                            result = true;
                            info = "添加商品项成功！";
                        }
                        else
                        {
                            result = false;
                            info = "添加商品项失败！";
                        }
                     
                }
                #endregion
                #region 旧的循环判断
                //foreach (var arr in collocation)
                //{
                //    if (FeProductItemBo.Instance.GetCount(mid, Convert.ToInt32(arr)) <= 0)
                //    {
                //        var productItem = new FeProductItem()
                //            {
                //                GroupSysNo = mid,
                //                ProductSysNo = Convert.ToInt32(arr),
                //                BeginDate = DateTime.Now,
                //                EndDate = DateTime.Now,
                //                DispalySymbol = (int)ForeStatus.显示标志.正常,
                //                Status = (int)ForeStatus.商品项状态.待审,
                //                CreatedBy = CurrentUser.Base.SysNo,
                //                CreatedDate = DateTime.Now,
                //                LastUpdateDate = DateTime.Now
                //            };
                //        if (FeProductItemBo.Instance.Insert(productItem) > 0)
                //        {
                //            result = true;
                //            info = "添加商品项成功！";
                //        }
                //        else
                //        {
                //            result = false;
                //            info = "添加商品项失败！";
                //        }
                //    }
                //    else
                //    {
                //        result = false;
                //        info = "已有的产品不能添加！";
                //    }
                //}
                #endregion
            }
            catch (Exception)
            {
                result = false;
                info = "添加商品项失败！";
                throw;
            }
            return Json(new { result = result, info = info }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///  根据商品组获取商品列表
        /// </summary>
        /// <param name="groupSysNo">商品组编号</param>
        /// <returns>返回插入成功OR失败</returns>
        /// <remarks>2013-11-13 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.FE1009202)]
        public JsonResult FeProductItemProduct(int groupSysNo, int dealersysno)
        {
            var list = FeProductItemBo.Instance.GetListByGroup(groupSysNo, dealersysno);
            return Json(from item in list select new { ProductSysNo = item.ProductSysNo }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 单条记录更新状态
        /// </summary>
        /// <param name="collocation">商品项视图</param>
        /// <returns>更新商品项状态</returns>
        /// <remarks>2013-06-25 苟治国 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.FE1009602, PrivilegeCode.FE1009202)]
        public ActionResult UpdateFeProductItemStatus(IList<FeProductItem> collocation)
        {
            bool result = false;
            string info = "操作失败";

            string str = "";
            if (collocation[0].Status == (int)ForeStatus.商品项状态.作废)
                str = "作废";
            else if (collocation[0].Status == (int)ForeStatus.商品项状态.已审)
                str = "已审";
            else
            {
                str = "取消已审";
            }

            if (FeProductItemBo.Instance.UpdateStatus(collocation) > 0)
            {
                result = true;
                info = str + "成功！";
            }
            else
            {
                result = true;
                info = str + "失败！";
            }
            return Json(new { result = result, info = info });
        }

        /// <summary>
        /// 批量更新状态
        /// </summary>
        /// <param name="collocation">商品项视图</param>
        /// <returns>更新商品项状态</returns>
        /// <remarks>2013-06-25 苟治国 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.FE1009602, PrivilegeCode.FE1009202)]
        public ActionResult UpdateFeProductItemStatusAll(IList<FeProductItem> collocation)
        {
            bool result = false;
            string info = "操作失败";
            if (FeProductItemBo.Instance.UpdateStatus(collocation) > 0)
            {
                result = true;
                info = "批量审核成功！";
            }
            else
            {
                result = true;
                info = "批量审核失败！";
            }
            return Json(new { result = result, info = info });
        }

        /// <summary>
        /// 删除商品项
        /// </summary>
        /// <param name="id">系统编号</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2015-10-26 王耀发 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.FE1009602, PrivilegeCode.FE1009202)]
        public JsonResult Delete(int id)
        {
            bool rvalue;
            var result = new Result();
            try
            {
                if (id > 0)
                {
                    rvalue = FeProductItemBo.Instance.Delete(id);
                    if (rvalue)
                    {
                        result.Status = true;
                    }
                    else
                    {
                        result.Status = false;
                        result.Message ="删除失败!";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion

        #region 商品咨询

        /// <summary>
        /// 根据条件获取会员咨询的列表
        /// </summary>
        /// <param name="id">分页索引</param>
        /// <param name="type">会员咨询类型</param>
        /// <param name="staus">会员咨询状态</param>
        /// <param name="customerSysNo">会员编号</param>
        /// <param name="productSysNo">商品编号</param>
        /// <param name="productName">商品名称</param>
        /// <param name="beginDate">查询开始时间</param>
        /// <param name="endDate">查询结束时间</param>
        /// <returns>会员咨询列表</returns>
        /// <remarks>2013－06-25 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.CR1002001)]
        public ActionResult CrCustomerQuestion(int? id, int? type, int? staus, int? customerSysNo, string productSysNo = null,
                                               string productName = null, string beginDate = null, string endDate = null)
        {
            //商品咨询类别
            IDictionary<int, string> typeList = EnumUtil.ToDictionary(typeof(CustomerStatus.会员咨询类型));
            ViewBag.TypeList = typeList;
            //商品咨询状态
            IDictionary<int, string> statusList = EnumUtil.ToDictionary(typeof(CustomerStatus.会员咨询状态));
            ViewBag.StatusList = statusList;

            id = id ?? 1;
            if (staus == null)
            {
                staus = (int)CustomerStatus.会员咨询状态.待回复;
            }
            if (type == null)
            {
                type = -1;
            }
            if (customerSysNo == null)
            {
                customerSysNo = -1;
            }
            var model = new PagedList();

            IList<CBCrCustomerQuestion> list = CrCustomerQuestionBo.Instance.Seach((int)id, model.PageSize, type, staus, customerSysNo,
                                                                                   productSysNo, productName, beginDate,
                                                                                   endDate);
            //int count = CrCustomerQuestionBo.Instance.GetCount(type, staus, customerSysNo, productSysNo, productName, beginDate,
            //                                                   endDate);

            model.Data = list;
            model.TotalItemCount = list.Count;
            model.CurrentPageIndex = (int)id;
            if (Request.IsAjaxRequest())
            {
                return PartialView("_AjaxCrCustomerQuestion", model);
            }
            return View(model);
        }

        /// <summary>
        /// 修改会员咨询
        /// </summary>
        /// <param name="id">会员咨询编号</param>
        /// <returns>空视图</returns>
        /// <remarks>2013-06-26 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.CR1002002)]
        public ActionResult CrCustomerQuestionUpdate(int id)
        {
            CBCrCustomerQuestion model = CrCustomerQuestionBo.Instance.GetGroupModel(id);
            return View(model);
        }

        /// <summary>
        /// 回复会员咨询
        /// </summary>
        /// <returns>空视图</returns>
        /// <remarks>2013-06-26 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.CR1002601)]
        public ActionResult CrCustomerQuestionAddOrEdit()
        {
            bool result = false;
            string info = "操作失败";

            string sysNo = Request.Form["hidSysNo"];
            string answer = Request.Form["answer"];

            var model = new CrCustomerQuestion();
            if (!sysNo.Equals("0"))
            {
                model = CrCustomerQuestionBo.Instance.GetModel(Convert.ToInt32(sysNo));
                model.Answer = answer;
                model.Status = (int)CustomerStatus.会员咨询状态.已回复;
                model.AnswerSysNo = CurrentUser.Base.SysNo;
                model.AnswerDate = DateTime.Now;
            }

            if (!sysNo.Equals("0"))
            {
                if (CrCustomerQuestionBo.Instance.Update(model) > 0)
                {
                    result = true;
                    info = "回复成功！";
                }
                else
                {
                    result = false;
                    info = "回复失败！";
                }
            }

            return Json(new { result = result, info = info });
        }

        /// <summary>
        /// 更新会员咨询状态
        /// </summary>
        /// <returns>更新状态</returns>
        /// <remarks>2013-06-26 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.CR1002501)]
        public ActionResult CrCustomerQuestionUpdateStatus()
        {
            bool result = false;
            string info = "操作失败";
            string sysNo = Request.Form["hidSysNo"];

            var model = new CrCustomerQuestion();
            if (!sysNo.Equals("0"))
            {
                try
                {
                    model = CrCustomerQuestionBo.Instance.GetModel(Convert.ToInt32(sysNo));
                    model.Status = (int)CustomerStatus.会员咨询状态.作废;
                    if (!sysNo.Equals("0"))
                    {
                        if (CrCustomerQuestionBo.Instance.Update(model) > 0)
                        {
                            result = true;
                            info = "作废成功！";
                        }
                        else
                        {
                            result = false;
                            info = "作废失败！";
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                info = "参数错误！";
            }
            return Json(new { result = result, info = info });
        }

        #endregion

        #region 商品评论

        /// <summary>
        /// 商品评论主页面
        /// </summary>
        /// <param name="id">商品评论列表起始页数</param>
        /// <param name="status">状态</param>
        /// <param name="isBest">是否精华</param>
        /// <param name="isTop">是否置顶</param>
        /// <param name="beginDate">评论开始时间</param>
        /// <param name="endDate">评论结束时间</param>
        /// <param name="customerName">会员名称</param>
        /// <param name="productSysNo">产品编号</param>
        /// <returns>当前页的商品评论列表</returns>
        /// <remarks>2013-07-09 杨晗 创建</remarks>
        [Privilege(PrivilegeCode.FE1001001)]
        public ActionResult FeProductComment(int? id, int? status, int? isBest, int? isTop,
                                                        DateTime? beginDate, DateTime? endDate,
                                                        string customerName = null, string productSysNo = null)
        {
            var dictList = EnumUtil.ToDictionary(typeof(ForeStatus.商品评论状态));
            ViewBag.DictList = dictList;
            var dictIsBest = EnumUtil.ToDictionary(typeof(ForeStatus.是否精华));
            ViewBag.DictIsBest = dictIsBest;
            var dictIsTop = EnumUtil.ToDictionary(typeof(ForeStatus.是否置顶));
            ViewBag.DictIsTop = dictIsTop;
            status = status ?? (int)ForeStatus.商品评论状态.待审;
            var model = FeProductCommentBo.Instance.Seach(id, (int)ForeStatus.是否晒单.否, (int)ForeStatus.是否评论.是, status, isBest, isTop, beginDate, endDate, customerName, productSysNo);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_AjaxFeProductComment", model);
            }
            return View(model);

        }

        /// <summary>
        /// 设置商品评论是否精华
        /// </summary>
        /// <param name="sysNo">商品评论系统号</param>
        /// <param name="status">是否精华状态</param>
        /// <returns>设置成功为true，否则为false</returns>
        /// <remarks>2013-07-10 杨晗 创建</remarks>
        [Privilege(PrivilegeCode.FE1001601, PrivilegeCode.FE1002601)]
        public ActionResult FeProductCommentIsBest(int sysNo, ForeStatus.是否精华 status)
        {
            var model = FeProductCommentBo.Instance.GetProductComment(sysNo);
            model.IsBest = (int)status;
            var u = FeProductCommentBo.Instance.Update(model);
            SysLog.Instance.Info(LogStatus.系统日志来源.后台,
                                 "设置了系统号为" + sysNo + "的评价/晒单为精华", LogStatus.系统日志目标类型.新闻帮助管理, sysNo, null, "",
                                  CurrentUser.Base.SysNo);
            return Json(new { IsPass = u > 0 });
        }

        /// <summary>
        /// 设置商品评论是否为置顶
        /// </summary>
        /// <param name="sysNo">商品评论系统号</param>
        /// <param name="status">是否置顶状态</param>
        /// <returns>设置成功为true，否则为false</returns>
        /// <remarks>2013-07-10 杨晗 创建</remarks>
        [Privilege(PrivilegeCode.FE1001602, PrivilegeCode.FE1002602)]
        public ActionResult FeProductCommentIsTop(int sysNo, ForeStatus.是否置顶 status)
        {
            var model = FeProductCommentBo.Instance.GetProductComment(sysNo);
            model.IsTop = (int)status;
            var u = FeProductCommentBo.Instance.Update(model);
            SysLog.Instance.Info(LogStatus.系统日志来源.后台,
                                 "设置了系统号为" + sysNo + "的评价/晒单为顶置", LogStatus.系统日志目标类型.新闻帮助管理, sysNo, null, "",
                                  CurrentUser.Base.SysNo);
            return Json(new { IsPass = u > 0 });
        }

        /// <summary>
        /// 商品评论详情查看(包含回复列表审核)
        /// </summary>
        /// <param name="id">回复列表起始页码</param>
        /// <param name="commentSysNo">商品评论系统号</param>
        /// <param name="searchStaus">状态</param>
        /// <returns>当前页回复列表</returns>
        /// <remarks>2013-07-09 杨晗 创建</remarks>
        [Privilege(PrivilegeCode.FE1001002)]
        public ActionResult FeProductCommentAudit(int? id, int commentSysNo, ForeStatus.商品评论回复状态? searchStaus)
        {

            var commentModel = FeProductCommentBo.Instance.GetModel(commentSysNo);
            ViewBag.CommentModel = commentModel;
            var model = FeProductCommentReplyBo.Instance.Seach(id, commentSysNo, searchStaus);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_AjaxFeProductCommentReply", model);
            }
            return View(model);
        }

        /// <summary>
        /// 更改商品评论状态
        /// </summary>
        /// <param name="sysNo">商品评论系统号</param>
        /// <param name="status">评论状态</param>
        /// <param name="type">是否晒单</param>
        /// <returns>更新状态成功返回true，失败false</returns>
        /// <remarks>2013-07-11 杨晗 创建</remarks>
        /// <remarks>2013-12-26 黄波 优化代码</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.FE1001603, PrivilegeCode.FE1001401, PrivilegeCode.FE1002603, PrivilegeCode.FE1002501)]
        public ActionResult FeProductCommentAudit(int sysNo, ForeStatus.商品评论状态 status, int? type)
        {
            var model = FeProductCommentBo.Instance.GetProductComment(sysNo);
            var customerSysNo = CurrentUser.Base.SysNo;
            var excuteStatus = false;

            if (type != null && (int)type == (int)ForeStatus.是否晒单.是)
            {
                model.ShareStatus = (int)status;
                excuteStatus = FeProductCommentBo.Instance.Update(model) > 0;

                if (excuteStatus)
                {
                    if (status == ForeStatus.商品评论状态.已审)
                    {
                        BLL.Web.PdProductBo.Instance.UpdateProductShares(model.ProductSysNo, model.Score);
                        //PointBo.Instance.ShareOrderIncreasePoint(model.CustomerSysNo, model.OrderSysNo);
                        SyJobPoolManageBo.Instance.DeleteJobPool(sysNo, (int)SystemStatus.任务对象类型.商品晒单审核);
                    }
                    else if (status == ForeStatus.商品评论状态.待审)
                    {
                        SyJobPoolManageBo.Instance.AssignJobByTaskType((int)SystemStatus.任务对象类型.商品晒单审核, sysNo, 0);
                    }
                    else if (status == ForeStatus.商品评论状态.作废)
                    {
                        CacheManager.Instance.Delete("UserUnCommentNumber_" + model.CustomerSysNo);
                        SyJobPoolManageBo.Instance.DeleteJobPool(sysNo, (int)SystemStatus.任务对象类型.商品晒单审核);
                    }

                    SysLog.Instance.Info(LogStatus.系统日志来源.后台, string.Format("晒单状态变更为:{0}。", status.ToString(), sysNo), LogStatus.系统日志目标类型.评价晒单管理, sysNo, null, "", customerSysNo);
                }
            }
            else
            {
                model.CommentStatus = (int)status;
                excuteStatus = FeProductCommentBo.Instance.Update(model) > 0;

                if (excuteStatus)
                {
                    if (status == ForeStatus.商品评论状态.已审)
                    {
                        BLL.Web.PdProductBo.Instance.UpdateProductComments(model.ProductSysNo, model.Score);
                        //PointBo.Instance.CommentIncreasePoint(model.CustomerSysNo, model.ProductSysNo);
                        SyJobPoolManageBo.Instance.DeleteJobPool(sysNo, (int)SystemStatus.任务对象类型.商品评论审核);
                    }
                    else if (status == ForeStatus.商品评论状态.待审)
                    {
                        SyJobPoolManageBo.Instance.AssignJobByTaskType((int)SystemStatus.任务对象类型.商品评论审核, sysNo, 0);
                    }
                    else if (status == ForeStatus.商品评论状态.作废)
                    {
                        CacheManager.Instance.Delete("UserUnCommentNumber_" + model.CustomerSysNo);
                        SyJobPoolManageBo.Instance.DeleteJobPool(sysNo, (int)SystemStatus.任务对象类型.商品评论审核);
                    }

                    SysLog.Instance.Info(LogStatus.系统日志来源.后台, string.Format("评论状态变更为:{0}。", status.ToString(), sysNo), LogStatus.系统日志目标类型.评价晒单管理, sysNo, null, "", customerSysNo);
                }
            }

            //删除缓存评论相关缓存
            if (excuteStatus)
            {
                BLL.Cache.DeleteCache.ProductCommentInfo(model.ProductSysNo);
            }

            return Json(new { IsPass = excuteStatus });
        }

        /// <summary>
        /// 查看全部商品晒单和评论的回复列表页
        /// </summary>
        /// <param name="id">回复列表的起始页码</param>
        /// <param name="searchStaus">状态</param>
        /// <returns>当前页回复列表</returns>
        /// <remarks>2013-07-09 杨晗 创建</remarks>
        [Privilege(PrivilegeCode.FE1001003, PrivilegeCode.FE1002003)]
        public ActionResult FeProductCommentReply(int? id, ForeStatus.商品评论回复状态? searchStaus)
        {

            IDictionary<int, string> dictList = EnumUtil.ToDictionary(typeof(ForeStatus.商品评论回复状态));
            ViewBag.DictList = dictList;
            var model = FeProductCommentReplyBo.Instance.Seach(id, null, searchStaus);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_AjaxFeProductCommentReplyAll", model);
            }
            return View(model);
        }

        /// <summary>
        /// 更新商品评论回复的状态
        /// </summary>
        /// <param name="sysNo">商品评论回复系统号</param>
        /// <param name="status">商品评论回复状态</param>
        /// <returns>更新状态成功返回true，失败false</returns>
        /// <remarks>2013-07-11 杨晗 创建</remarks>
        [Privilege(PrivilegeCode.FE1001603, PrivilegeCode.FE1002603, PrivilegeCode.FE1001401, PrivilegeCode.FE1002501)]
        public ActionResult FeProductCommentReplyAudit(int sysNo, ForeStatus.商品评论回复状态 status)
        {
            var model = FeProductCommentReplyBo.Instance.GetModel(sysNo);
            model.Status = (int)status;
            if (status == ForeStatus.商品评论回复状态.已审)
            {
                SyJobPoolManageBo.Instance.DeleteJobPool(sysNo, (int)SystemStatus.任务对象类型.商品评论回复审核);
            }
            else if (status == ForeStatus.商品评论回复状态.待审)
            {
                SyJobPoolManageBo.Instance.AssignJobByTaskType((int)SystemStatus.任务对象类型.商品评论回复审核, sysNo, 0);
            }
            else if (status == ForeStatus.商品评论回复状态.作废)
            {
                SyJobPoolManageBo.Instance.DeleteJobPool(sysNo, (int)SystemStatus.任务对象类型.商品评论回复审核);
            }
            var u = FeProductCommentReplyBo.Instance.Update(model);

            return Json(new { IsPass = u > 0 });
        }

        /// <summary>
        /// 批量审核或取消审核回复记录
        /// </summary>
        /// <param name="sysNos">回复记录系统编号集合</param>
        /// <param name="type">类型(0批量审核，1批量取消审核)</param>
        /// <returns>更新状态成功返回true，失败false</returns>
        /// <remarks>2013-07-11 杨晗 创建</remarks>
        [Privilege(PrivilegeCode.FE1001603)]
        public ActionResult FeProductCommentReplyAuditAll(string sysNos, int type)
        {
            var bl = true;
            var s = sysNos.Split(',');
            int status;
            if (type == 0)
            {
                status = (int)ForeStatus.商品评论回复状态.已审;
            }
            else
            {
                status = (int)ForeStatus.商品评论回复状态.待审;
            }
            foreach (var sysNo in s)
            {
                if (string.IsNullOrEmpty(sysNo)) continue;
                var model = FeProductCommentReplyBo.Instance.GetModel(Convert.ToInt32(sysNo));
                model.Status = status;
                var u = FeProductCommentReplyBo.Instance.Update(model);
                if (u <= 0)
                    bl = false;
                if (type == 0)
                {
                    SyJobPoolManageBo.Instance.DeleteJobPool(Convert.ToInt32(sysNo), (int)SystemStatus.任务对象类型.商品评论回复审核);
                }
                else
                {
                    SyJobPoolManageBo.Instance.AssignJobByTaskType((int)SystemStatus.任务对象类型.商品评论回复审核, Convert.ToInt32(sysNo), 0);
                }
            }
            return Json(new { IsPass = bl });
        }

        /// <summary>
        /// 商品晒单主页面
        /// </summary>
        /// <param name="id">商品评论列表起始页数</param>
        /// <param name="status">状态</param>
        /// <param name="isBest">是否精华</param>
        /// <param name="isTop">是否置顶</param>
        /// <param name="beginDate">评论开始时间</param>
        /// <param name="endDate">评论结束时间</param>
        /// <param name="customerName">会员名称</param>
        /// <param name="productSysNo">产品编号</param>
        /// <returns>商品晒单当前页列表</returns>
        /// <remarks>2013-07-09 杨晗 创建</remarks>
        [Privilege(PrivilegeCode.FE1002001)]
        public ActionResult FeProductShare(int? id, int? status, int? isBest, int? isTop,
                                                        DateTime? beginDate, DateTime? endDate,
                                                        string customerName = null, string productSysNo = null)
        {
            var dictList = EnumUtil.ToDictionary(typeof(ForeStatus.商品评论状态));
            ViewBag.DictList = dictList;
            var dictIsBest = EnumUtil.ToDictionary(typeof(ForeStatus.是否精华));
            ViewBag.DictIsBest = dictIsBest;
            var dictIsTop = EnumUtil.ToDictionary(typeof(ForeStatus.是否置顶));
            ViewBag.DictIsTop = dictIsTop;
            status = status ?? (int)ForeStatus.商品晒单状态.待审;
            var model = FeProductCommentBo.Instance.Seach(id, (int)ForeStatus.是否晒单.是, (int)ForeStatus.是否评论.否, status, isBest, isTop, beginDate, endDate, customerName, productSysNo);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_AjaxFeProductShare", model);
            }
            return View(model);

        }

        /// <summary>
        /// 商品晒单详情页(包含晒单图片审核，和晒单回复审核列表)
        /// </summary>
        /// <param name="id">晒单回复列表起始页码</param>
        /// <param name="commentSysNo">商品评论系统号</param>
        /// <param name="searchStaus">状态</param>
        /// <returns>晒单回复列表</returns>
        /// <remarks>2013-07-09 杨晗 创建</remarks>
        [Privilege(PrivilegeCode.FE1002002)]
        public ActionResult FeProductShareAudit(int? id, int commentSysNo, ForeStatus.商品评论回复状态? searchStaus)
        {
            var commentModel = FeProductCommentBo.Instance.GetModel(commentSysNo);
            ViewBag.CommentModel = commentModel;

            //晒单图片
            var commentImage = FeProductCommentImageBo.Instance.GetFeProductCommentImageByCommentSysNo(commentSysNo);
            ViewBag.CommentImage = commentImage;

            var model = FeProductCommentReplyBo.Instance.Seach(id, commentSysNo, searchStaus);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_AjaxFeProductCommentReply", model);
            }
            return View(model);
        }

        /// <summary>
        /// 商品晒单图片
        /// </summary>
        /// <param name="commentSysNo">商品评论系统号</param>
        /// <returns>晒单图片列表</returns>
        /// <remarks>2014-05-27 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.FE1002002)]
        public ActionResult FeProductShareImage(int commentSysNo)
        {
            ViewBag.CommentModel = FeProductCommentBo.Instance.GetModel(commentSysNo);
            //晒单图片
            ViewBag.CommentImage = FeProductCommentImageBo.Instance.GetFeProductCommentImageByCommentSysNo(commentSysNo);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_AjaxFeProductShareImage");
            }
            return View("_AjaxFeProductShareImage");
        }

        /// <summary>
        /// 更新晒单图片的状态
        /// </summary>
        /// <param name="sysNo">晒单图片系统号</param>
        /// <param name="status">晒单图片状态</param>
        /// <returns>更新状态成功返回true，失败false</returns>
        /// <remarks>2013-07-11 杨晗 创建</remarks>
        [Privilege(PrivilegeCode.FE1002501)]
        public ActionResult FeProductCommentImageAudit(int sysNo, ForeStatus.晒单图片状态 status)
        {
            var model = FeProductCommentImageBo.Instance.GetModel(sysNo);
            model.Status = (int)status;
            var u = FeProductCommentImageBo.Instance.Update(model);
            return Json(new { IsPass = u > 0 });
        }

        #endregion

        #region 帮助中心

        #region 帮助中心文章分类

        /// <summary>
        ///     帮助中心文章分类
        /// </summary>
        /// <param name="id">页码(以后改名字)</param>
        /// <param name="searchStaus">状态</param>
        /// <param name="searchName">分类名称</param>
        /// <returns>帮助中心文章分类列表</returns>
        /// <remarks>2013-06-17 杨晗 创建</remarks>
        [Privilege(PrivilegeCode.FE1007001)]
        public ActionResult FeHelpClass(int? id, int? searchStaus, string searchName = "")
        {
            IDictionary<int, string> dictList = EnumUtil.ToDictionary(typeof(ForeStatus.文章分类状态));
            ViewBag.DictList = dictList;

            var model = FeArticleCategoryBo.Instance.Seach(id, ForeStatus.文章分类类型.帮助,
                                                           searchStaus, searchName);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_AjaxFeArticleCategoryList", model);
            }
            return View(model);
        }

        /// <summary>
        ///     帮助中心分类添加视图
        /// </summary>
        /// <returns>空视图</returns>
        /// <remarks>2013-06-17 杨晗 创建</remarks>
        [Privilege(PrivilegeCode.FE1007201)]
        public ActionResult FeHelpClassAdd()
        {
            return View();
        }

        /// <summary>
        ///     帮助中心分类修改视图
        /// </summary>
        /// <param name="id">某条分类数据的系统号</param>
        /// <returns>空视图</returns>
        /// <remarks>2013-06-17 杨晗 创建</remarks>
        [Privilege(PrivilegeCode.FE1007201)]
        [ValidateInput(false)]
        public ActionResult FeHelpClassUpdate(int id)
        {
            IDictionary<int, string> dictList = EnumUtil.ToDictionary(typeof(ForeStatus.文章分类状态));
            ViewBag.DictList = dictList;
            FeArticleCategory model = FeArticleCategoryBo.Instance.GetModel(id);

            return View(model);
        }

        /// <summary>
        ///     帮助中心分类增加或修改
        /// </summary>
        /// <returns>保存失败或成功信息</returns>
        /// <remarks>2013-06-18 杨晗 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.FE1007201)]
        [ValidateInput(false)]
        public ActionResult FeHelpClassAddOrEdit()
        {
            string sysNo = Request.Form["hidSysNo"];
            string realName = Request.Form["txtRealName"];
            string displayOrder = Request.Form["txtDisplayOrder"];
            string description = Request.Form["txtDescription"];

            bool isPass;
            var model = new FeArticleCategory();
            if (!string.IsNullOrEmpty(sysNo))
            {
                model = FeArticleCategoryBo.Instance.GetModel(Convert.ToInt32(sysNo));

            }
            model.Name = HttpUtility.HtmlEncode(realName);
            model.DisplayOrder = Convert.ToInt32(displayOrder);
            model.Description = HttpUtility.HtmlEncode(description);

            model.Type = (int)ForeStatus.文章分类类型.帮助;
            try
            {
                if (string.IsNullOrEmpty(sysNo))
                {
                    model.Status = (int)ForeStatus.文章分类状态.待审;
                    int i = FeArticleCategoryBo.Instance.Insert(model);
                    isPass = i > 0;
                }
                else
                {
                    int u = FeArticleCategoryBo.Instance.Update(model);
                    isPass = u > 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(new { IsPass = isPass });
        }

        /// <summary>
        ///     删除帮助中心分类数据
        /// </summary>
        /// <param name="sysNo">帮助中心数据系统号</param>
        /// <returns>删除成功或失败信息</returns>
        /// <remarks>2013-06-18 杨晗 创建</remarks>
        [HttpPost]
        public ActionResult FeHelpClassDelete(int sysNo)
        {
            IList<FeArticle> list = FeArticleBo.Instance.GetListByCategory(sysNo);
            if (list != null && list.Any())
            {
                return Json(new { IsPass = false, Message = "该分类旗下有文章，请先删除旗下的文章" });
            }
            bool isPass = FeArticleCategoryBo.Instance.Delete(sysNo);
            return Json(new { IsPass = isPass });
        }

        #endregion

        #region 帮助中心文章

        /// <summary>
        ///     帮助中心文章
        /// </summary>
        /// <param name="id">页码(以后改名字)</param>
        /// <param name="searchStaus">状态</param>
        /// <param name="searchClass">帮助分类</param>
        /// <param name="searchName">标题</param>
        /// <returns>帮助中心文章列表</returns>
        /// <remarks>2013-06-19 杨晗 创建</remarks>
        [Privilege(PrivilegeCode.FE1006001)]
        public ActionResult FeHelpArticle(int? id, int? searchStaus, int? searchClass, string searchName = "",int SelectedDealerSysNo = -1)
        {
            IDictionary<int, string> dictList = EnumUtil.ToDictionary(typeof(ForeStatus.文章状态));
            ViewBag.DictList = dictList;
            IList<FeArticleCategory> clist = FeArticleCategoryBo.Instance.GetAll(ForeStatus.文章分类类型.帮助);
            ViewBag.Category = clist;
            List<int> ids = (from m in clist select m.SysNo).ToList();
            if (searchClass != null && searchClass != 0)
            {
                ids = new List<int> { (int)searchClass };
            }
            var filter = new ParaArticleFilter { pageIndex = id, ids = ids, searchStaus = searchStaus, searchName = searchName, SelectedDealerSysNo = SelectedDealerSysNo };
            //当前用户对应分销商，2015-12-19 王耀发 创建
            if (CurrentUser.IsBindDealer)
            {
                int DealerSysNo = CurrentUser.Dealer.SysNo;
                filter.DealerSysNo = DealerSysNo;
                filter.IsBindDealer = CurrentUser.IsBindDealer;
            }
            //是否绑定所有经销商
            filter.IsBindAllDealer = CurrentUser.IsBindAllDealer;
            filter.DealerCreatedBy = CurrentUser.Base.SysNo;

            var model = FeArticleBo.Instance.Seach(filter);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_AjaxFeArticleList", model);
            }
            return View(model);
        }

        /// <summary>
        ///     帮助文章增加或修改视图
        /// </summary>
        /// <param name="id">页码</param>
        /// <returns>增加或修改视图</returns>
        /// <remarks>2013-06-20 杨晗 创建</remarks>
        [Privilege(PrivilegeCode.FE1006201)]
        public ActionResult FeHelpArticleAddOrEdit(int? id)
        {
            ViewBag.DictList = EnumUtil.ToDictionary(typeof(ForeStatus.文章状态));

            ViewBag.Category = FeArticleCategoryBo.Instance.GetAll(ForeStatus.文章分类类型.帮助);

            var model = new FeArticle();
            if (id != null)
                model = FeArticleBo.Instance.GetModel((int)id);

            return View(model);
        }

        /// <summary>
        ///     帮助文章增加或修改功能
        /// </summary>
        /// <returns>成功或失败信息</returns>
        /// <remarks>2013-06-20 杨晗 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.FE1006201)]
        public ActionResult FeHelpArticleAddOrEdit()
        {
            string sysNo = Request.Form["hidSysNo"];
            string txtTitle = Request.Form["txtTitle"];
            string txtContent = Request.Form["editor_Content"];
            string category = Request.Form["selClass"];
            string dealerSysNo = Request.Form["dealerSysNo"];

            bool isPass;
            var model = new FeArticle();
            if (!string.IsNullOrEmpty(sysNo) && Convert.ToInt32(sysNo) != 0)
            {
                model = FeArticleBo.Instance.GetModel(Convert.ToInt32(sysNo));

                model.LastUpdateBy = CurrentUser.Base.SysNo;
                model.LastUpdateDate = DateTime.Now;
            }
            model.Title = txtTitle;
            model.Content = txtContent;

            model.CategorySysNo = Convert.ToInt32(category);
            try
            {
                if (!string.IsNullOrEmpty(sysNo) && Convert.ToInt32(sysNo) != 0)
                {
                    int u = FeArticleBo.Instance.Update(model);
                    isPass = u > 0;
                }
                else
                {
                    model.Status = (int)ForeStatus.文章分类状态.待审;
                    model.CreatedBy = CurrentUser.Base.SysNo;
                    model.CreatedDate = DateTime.Now;
                    model.LastUpdateBy = CurrentUser.Base.SysNo;
                    model.LastUpdateDate = DateTime.Now;
                    model.DealerSysNo = int.Parse(dealerSysNo);
                    int i = FeArticleBo.Instance.Insert(model);
                    isPass = i > 0;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return Json(new { IsPass = isPass });
        }

        /// <summary>
        ///     帮助文章删除
        /// </summary>
        /// <param name="sysNo">帮助文章系统号</param>
        /// <returns>成功或失败信息</returns>
        /// <remarks>2013-06-20 杨晗 创建</remarks>
        [HttpPost]
        public ActionResult FeHelpArticleDelete(int sysNo)
        {
            bool isPass = FeArticleBo.Instance.Delete(sysNo);
            return Json(new { IsPass = isPass });
        }

        #endregion

        #endregion

        #region 文章管理

        #region 文章分类管理

        /// <summary>
        ///     文章分类
        /// </summary>
        /// <param name="id">页码(以后改名字)</param>
        /// <param name="searchStaus">状态</param>
        /// <param name="searchName">分类名称</param>
        /// <returns>文章分类列表</returns>
        /// <remarks>2013-06-21 杨晗 创建</remarks>
        [Privilege(PrivilegeCode.FE1004001)]
        public ActionResult FeArticleCategory(int? id, int? searchStaus, string searchName = "")
        {
            IDictionary<int, string> dictList = EnumUtil.ToDictionary(typeof(ForeStatus.文章分类状态));
            ViewBag.DictList = dictList;
            var model = FeArticleCategoryBo.Instance.Seach(id, ForeStatus.文章分类类型.新闻,
                                                           searchStaus, searchName);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_AjaxFeArticleCategoryList", model);
            }
            return View(model);
        }

        /// <summary>
        ///     文章分类
        /// </summary>
        /// <param name="id">某条分类数据的系统号</param>
        /// <returns>文章分类添加视图</returns>
        /// <remarks>2013-06-21 杨晗 创建</remarks>
        [Privilege(PrivilegeCode.FE1004201)]
        [ValidateInput(false)]
        public ActionResult FeArticleCategoryAddOrUpdate(int? id)
        {
            IDictionary<int, string> dictList = EnumUtil.ToDictionary(typeof(ForeStatus.文章分类状态));
            ViewBag.DictList = dictList;
            var model = new FeArticleCategory();
            if (id != null && (int)id != 0)
            {
                model = FeArticleCategoryBo.Instance.GetModel((int)id);
            }

            return View(model);
        }

        /// <summary>
        ///     文章分类
        /// </summary>
        /// <returns>文章分类添加或修改</returns>
        /// <remarks>2013-06-21 杨晗 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.FE1004201)]
        [ValidateInput(false)]
        public ActionResult FeArticleCategoryAddOrUpdate()
        {
            string sysNo = Request.Form["hidSysNo"];
            string realName = Request.Form["txtRealName"];
            string displayOrder = Request.Form["txtDisplayOrder"];
            string description = Request.Form["txtDescription"];

            bool isPass;
            var model = new FeArticleCategory();
            if (!string.IsNullOrEmpty(sysNo) && Convert.ToInt32(sysNo) != 0)
            {
                model = FeArticleCategoryBo.Instance.GetModel(Convert.ToInt32(sysNo));

            }
            model.Name = HttpUtility.HtmlEncode(realName);
            model.DisplayOrder = Convert.ToInt32(displayOrder);
            model.Description = HttpUtility.HtmlEncode(description);

            model.Type = (int)ForeStatus.文章分类类型.新闻;
            try
            {
                if (!string.IsNullOrEmpty(sysNo) && Convert.ToInt32(sysNo) != 0)
                {
                    var u = FeArticleCategoryBo.Instance.Update(model);
                    isPass = u > 0;
                }
                else
                {
                    model.Status = (int)ForeStatus.文章分类状态.待审;
                    var i = FeArticleCategoryBo.Instance.Insert(model);
                    isPass = i > 0;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return Json(new { IsPass = isPass });
        }

        /// <summary>
        ///     删除文章分类
        /// </summary>
        /// <param name="sysNo">文章分类系统号</param>
        /// <returns>成功或失败信息</returns>
        /// <remarks>2013-06-21 杨晗 创建</remarks>
        [HttpPost]
        public ActionResult FeArticleCategoryDelete(int sysNo)
        {
            IList<FeArticle> list = FeArticleBo.Instance.GetListByCategory(sysNo);
            if (list != null && list.Any())
            {
                return Json(new { IsPass = false, Message = "该分类旗下有文章，请先删除旗下的文章" });
            }
            var isPass = FeArticleCategoryBo.Instance.Delete(sysNo);
            return Json(new { IsPass = isPass });
        }

        /// <summary>
        /// 审核文章分类
        /// </summary>
        /// <param name="sysNo">文章分类系统编号</param>
        /// <param name="status">文章分类状态</param>
        /// <returns>设置成功或失败信息</returns>
        /// <remarks>2013-07-01 杨晗 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.FE1004601, PrivilegeCode.FE1004501, PrivilegeCode.FE1007601, PrivilegeCode.FE1007501)]
        public ActionResult FeArticleCategoryAudit(int sysNo, ForeStatus.文章分类状态 status)
        {
            var model = FeArticleCategoryBo.Instance.GetModel(Convert.ToInt32(sysNo));
            model.Status = (int)status;
            var u = FeArticleCategoryBo.Instance.Update(model);
            var isPass = u > 0;
            return Json(new { IsPass = isPass });
        }

        /// <summary>
        /// 文章分类验证是否重复
        /// </summary>
        /// <returns>重复为false,否则为true</returns>
        /// <remarks>2013-07-05 杨晗 创建</remarks>
        [Privilege(PrivilegeCode.FE1004201)]
        [ValidateInput(false)]
        public ActionResult FeArticleCategoryVerify()
        {
            var txtRealName = Request.Form["txtRealName"];
            var sysNo = Request.Form["hidSysNo"];
            var type = (ForeStatus.文章分类类型)int.Parse(Request.Form["type"]);
            bool bl;
            if (!string.IsNullOrEmpty(sysNo) && Convert.ToInt32(sysNo) != 0)
            {
                var model = FeArticleCategoryBo.Instance.GetModel(Convert.ToInt32(sysNo));
                bl = txtRealName != model.Name &&
                     FeArticleCategoryBo.Instance.FeArticleCategoryVerify(txtRealName, type);
            }
            else
            {
                bl = FeArticleCategoryBo.Instance.FeArticleCategoryVerify(txtRealName, type);
            }
            return Json(!bl, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 文章详情管理

        /// <summary>
        ///     文章详情
        /// </summary>
        /// <param name="id">页码(以后改名字)</param>
        /// <param name="searchStaus">状态</param>
        /// <param name="searchClass">文章分类</param>
        /// <param name="searchName">标题</param>
        /// <returns>文章列表</returns>
        /// <remarks>2013-06-19 杨晗 创建</remarks>
        [Privilege(PrivilegeCode.FE1005001)]
        public ActionResult FeArticle(int? id, int? searchStaus, int? searchClass, string searchName = "")
        {
            IDictionary<int, string> dictList = EnumUtil.ToDictionary(typeof(ForeStatus.文章状态));
            ViewBag.DictList = dictList;

            IList<FeArticleCategory> clist = FeArticleCategoryBo.Instance.GetAll(ForeStatus.文章分类类型.新闻);
            ViewBag.Category = clist;
            List<int> ids = (from m in clist select m.SysNo).ToList();
            if (searchClass != null && searchClass != 0)
            {
                ids = new List<int> { (int)searchClass };
            }

            var filter = new ParaArticleFilter { pageIndex = id, ids = ids, searchStaus = searchStaus, searchName = searchName, SelectedDealerSysNo = -1 };
            //当前用户对应分销商，2015-12-19 王耀发 创建
            if (CurrentUser.IsBindDealer)
            {
                int DealerSysNo = CurrentUser.Dealer.SysNo;
                filter.DealerSysNo = DealerSysNo;
                filter.IsBindDealer = CurrentUser.IsBindDealer;
            }
            //是否绑定所有经销商
            filter.IsBindAllDealer = CurrentUser.IsBindAllDealer;
            filter.DealerCreatedBy = CurrentUser.Base.SysNo;

            var model = FeArticleBo.Instance.Seach(filter);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_AjaxFeArticleList", model);
            }
            return View(model);
        }

        /// <summary>
        ///     文章增加或修改视图
        /// </summary>
        /// <param name="id">页码</param>
        /// <returns>增加或修改视图</returns>
        /// <remarks>2013-06-20 杨晗 创建</remarks>
        [Privilege(PrivilegeCode.FE1005201)]
        public ActionResult FeArticleAddOrEdit(int? id)
        {
            ViewBag.DictList = EnumUtil.ToDictionary(typeof(ForeStatus.文章状态));

            ViewBag.Category = FeArticleCategoryBo.Instance.GetAll(ForeStatus.文章分类类型.新闻);

            var model = new FeArticle();
            if (id != null)
                model = FeArticleBo.Instance.GetModel((int)id);

            return View(model);
        }

        /// <summary>
        ///     文章增加或修改功能
        /// </summary>
        /// <returns>成功或失败信息</returns>
        /// <remarks>2013-06-20 杨晗 创建</remarks>
        [ValidateInput(false)]
        [HttpPost]
        [Privilege(PrivilegeCode.FE1005201)]
        public ActionResult FeArticleAddOrEdit()
        {
            string sysNo = Request.Form["hidSysNo"];
            string txtTitle = Request.Form["txtTitle"];
            string txtContent = Request.Form["editor_Content"];
            string category = Request.Form["selClass"];

            bool isPass;
            var model = new FeArticle();
            if (!string.IsNullOrEmpty(sysNo) && Convert.ToInt32(sysNo) != 0)
            {
                model = FeArticleBo.Instance.GetModel(Convert.ToInt32(sysNo));

                model.LastUpdateBy = CurrentUser.Base.SysNo;
                model.LastUpdateDate = DateTime.Now;
            }
            model.Title = txtTitle;
            model.Content = txtContent;

            model.CategorySysNo = Convert.ToInt32(category);
            try
            {
                if (!string.IsNullOrEmpty(sysNo) && Convert.ToInt32(sysNo) != 0)
                {
                    var u = FeArticleBo.Instance.Update(model);
                    isPass = u > 0;
                }
                else
                {
                    model.Status = (int)ForeStatus.文章分类状态.待审;
                    model.CreatedBy = CurrentUser.Base.SysNo;
                    model.CreatedDate = DateTime.Now;
                    model.LastUpdateBy = CurrentUser.Base.SysNo;
                    model.LastUpdateDate = DateTime.Now;
                    var i = FeArticleBo.Instance.Insert(model);
                    isPass = i > 0;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return Json(new { IsPass = isPass });
        }

        /// <summary>
        ///     文章删除
        /// </summary>
        /// <param name="sysNo">文章系统号</param>
        /// <returns>返回成功或失败</returns>
        /// <remarks>2013-06-20 杨晗 创建</remarks>
        [HttpPost]
        public ActionResult FeArticleDelete(int sysNo)
        {
            bool isPass = FeArticleBo.Instance.Delete(sysNo);
            return Json(new { IsPass = isPass });
        }

        /// <summary>
        /// 审核文章
        /// </summary>
        /// <param name="sysNo">文章系统编号</param>
        /// <param name="status">文章状态</param>
        /// <returns>设置成功或失败信息</returns>
        /// <remarks>2013-07-01 杨晗 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.FE1005601, PrivilegeCode.FE1005501, PrivilegeCode.FE1006601, PrivilegeCode.FE1006501)]
        public ActionResult FeArticleAudit(int sysNo, ForeStatus.文章分类状态 status)
        {
            var model = FeArticleBo.Instance.GetModel(sysNo);
            model.Status = (int)status;
            var u = FeArticleBo.Instance.Update(model);
            var isPass = u > 0;
            return Json(new { IsPass = isPass });
        }

        /// <summary>
        /// 文章标题验证是否重复
        /// </summary>
        /// <returns>重复为false,否则为true</returns>
        /// <remarks>2013-07-05 杨晗 创建</remarks>
        [Privilege(PrivilegeCode.FE1005201)]
        public ActionResult FeArticleVerify()
        {
            var txtTitle = Request.Form["txtTitle"];
            var sysNo = Request.Form["hidSysNo"];
            bool bl;
            if (!string.IsNullOrEmpty(sysNo) && Convert.ToInt32(sysNo) != 0)
            {
                var model = FeArticleBo.Instance.GetModel(Convert.ToInt32(sysNo));
                bl = txtTitle != model.Title && FeArticleBo.Instance.FeArticleVerify(txtTitle);
            }
            else
            {
                bl = FeArticleBo.Instance.FeArticleVerify(txtTitle);
            }
            return Json(!bl, JsonRequestBehavior.AllowGet);
        }

        #region 公司公告
        /// <summary>
        /// 公司公告
        /// </summary>
        /// <param name="id">当前页码</param>
        /// <param name="searchName">公告标题</param>
        /// <returns>公司公告</returns>
        /// <remarks>2013-10-22 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.FE1005102)]
        public ActionResult Announcement(int? id, string searchName)
        {
            if (Request.IsAjaxRequest())
            {
                var model = new PagedList<CBFeArticle>();
                var clist = FeArticleCategoryBo.Instance.GetAll(ForeStatus.文章分类类型.新闻);
                //var ids = (from m in clist select m.SysNo).ToList();
                var category = clist.FirstOrDefault(x => x.Name.Contains(@"公司公告"));

                if (category != null)
                {
                    var ids = new List<int> { category.SysNo };
                    var searchStaus = ForeStatus.文章状态.已审.GetHashCode();

                    var filter = new ParaArticleFilter { pageIndex = id, ids = ids, searchStaus = searchStaus, searchName = searchName, SelectedDealerSysNo = -1 };
                    //当前用户对应分销商，2015-12-19 王耀发 创建
                    if (CurrentUser.IsBindDealer)
                    {
                        int DealerSysNo = CurrentUser.Dealer.SysNo;
                        filter.DealerSysNo = DealerSysNo;
                        filter.IsBindDealer = CurrentUser.IsBindDealer;
                    }
                    //是否绑定所有经销商
                    filter.IsBindAllDealer = CurrentUser.IsBindAllDealer;
                    filter.DealerCreatedBy = CurrentUser.Base.SysNo;
                    model = FeArticleBo.Instance.Seach(filter);
                }
                return PartialView("_AnnouncementList", model);
            }
            return View();
        }
        /// <summary>
        /// 查看文章
        /// </summary>
        /// <param name="id">文章系统编号</param>
        /// <returns>文章</returns>
        /// <remarks>2013-10-22 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.FE1005102)]
        public ActionResult ArticleDetail(int id)
        {
            var model = FeArticleBo.Instance.GetModel(id);
            ViewBag.Category = FeArticleCategoryBo.Instance.GetAll(ForeStatus.文章分类类型.新闻);
            return View(model);
        }
        #endregion

        #endregion

        #endregion

        #region 搜索关键字

        /// <summary>
        ///     搜索关键字分页列表视图
        /// </summary>
        /// <param name="id">起始页码</param>
        /// <param name="status">状态</param>
        /// <param name="hitsCount">点击次数</param>
        /// <param name="createdDateStart">创建时间起始值</param>
        /// <param name="createdDateEnd">创建时间结束值</param>
        /// <param name="searchName">关键字</param>
        /// <returns>搜索关键字信息当前页列表数据</returns>
        /// <remarks>2013-06-27 杨晗 创建</remarks>
        [Privilege(PrivilegeCode.FE1003001)]
        public ActionResult FeSearchKeyword(int? id, int? status, int? hitsCount, DateTime? createdDateStart,
                                            DateTime? createdDateEnd, string searchName = null)
        {
            id = id ?? 1;
            var model = new PagedList<FeSearchKeyword>();
            IDictionary<int, string> dictList = EnumUtil.ToDictionary(typeof(ForeStatus.搜索关键字状态));
            ViewBag.DictList = dictList;
            IList<FeSearchKeyword> list = FeSearchKeywordBo.Instance.Seach((int)id, model.PageSize, status,
                                                                           hitsCount, createdDateStart, createdDateEnd,
                                                                           searchName);
            int count = FeSearchKeywordBo.Instance.GetCount(status, hitsCount, createdDateStart,
                                                            createdDateEnd, searchName);

            model.TData = list;
            model.TotalItemCount = count;
            model.CurrentPageIndex = (int)id;
            if (Request.IsAjaxRequest())
            {
                return PartialView("_AjaxFeSearchKeywordList", model);
            }
            return View(model);
        }

        /// <summary>
        /// 增加或修改关键字配置视图
        /// </summary>
        /// <param name="sysNo">关键字系统号</param>
        /// <returns>对应视图</returns>
        /// <remarks>2013-06-27 杨晗 创建</remarks>
        [Privilege(PrivilegeCode.FE1003201)]
        public ActionResult FeSearchKeywordAddOrEdit(int? sysNo)
        {
            var model = new FeSearchKeyword();
            if (sysNo != null)
            {
                model = FeSearchKeywordBo.Instance.GetModel((int)sysNo);
            }
            return View(model);
        }

        /// <summary>
        /// 增加或修改关键字配置
        /// </summary>
        /// <returns>操作成功或失败信息</returns>
        /// <remarks>2013-06-27 杨晗 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.FE1003201)]
        [ValidateInput(false)]
        public ActionResult FeSearchKeywordAddOrEdit()
        {
            string sysNo = Request.Form["hidSysNo"];
            string txtKeyword = Request.Form["txtKeyword"];
            string txtHitsCount = Request.Form["txtHitsCount"];
            bool isPass;
            var model = new FeSearchKeyword();
            if (!string.IsNullOrEmpty(sysNo) && Convert.ToInt32(sysNo) != 0)
            {
                model = FeSearchKeywordBo.Instance.GetModel(Convert.ToInt32(sysNo));
                model.LastUpdateBy = CurrentUser.Base.SysNo;
                model.LastUpdateDate = DateTime.Now;
            }

            try
            {
                model.keyword = HttpUtility.HtmlEncode(txtKeyword);
                model.HitsCount = Convert.ToInt32(txtHitsCount);
                if (!string.IsNullOrEmpty(sysNo) && Convert.ToInt32(sysNo) != 0)
                {
                    int u = FeSearchKeywordBo.Instance.Update(model);
                    isPass = u > 0;
                }
                else
                {
                    model.Status = (int)ForeStatus.搜索关键字状态.后台记录;
                    model.CreatedBy = CurrentUser.Base.SysNo;
                    model.CreatedDate = DateTime.Now;
                    model.LastUpdateBy = CurrentUser.Base.SysNo;
                    model.LastUpdateDate = DateTime.Now;
                    int i = FeSearchKeywordBo.Instance.Insert(model);
                    isPass = i > 0;
                }

            }
            catch (Exception ex)
            {
                throw;
            }

            return Json(new { IsPass = isPass });
        }

        /// <summary>
        /// 删除搜索关键字信息
        /// </summary>
        /// <param name="sysNo">搜索关键字系统号</param>
        /// <returns>删除成功或失败信息</returns>
        /// <remarks>2013-06-27 杨晗 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.FE1003401)]
        public ActionResult FeSearchKeywordDelete(int sysNo)
        {
            bool isPass = FeSearchKeywordBo.Instance.Delete(sysNo);
            return Json(new { IsPass = isPass });
        }

        /// <summary>
        /// 更新搜索关键字状态
        /// </summary>
        /// <param name="sysNo">搜索关键字系统号</param>
        /// <param name="status">搜索关键字状态</param>
        /// <returns>更新成功或失败信息</returns>
        /// <remarks>2013-06-27 杨晗 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.FE1003601)]
        public ActionResult FeSearchKeywordSetStatus(int sysNo, ForeStatus.搜索关键字状态 status)
        {
            var model = FeSearchKeywordBo.Instance.GetModel(sysNo);
            model.Status = (int)status;
            int u = FeSearchKeywordBo.Instance.Update(model);
            return Json(new { IsPass = u > 0 });
        }

        /// <summary>
        /// 判断搜索关键字是否重复
        /// </summary>
        /// <returns>重复为false,否则为true</returns>
        /// <remarks>2013-07-05 杨晗 创建</remarks>
        [Privilege(PrivilegeCode.FE1003201)]
        [ValidateInput(false)]
        public ActionResult FeSearchKeywordVerify()
        {
            var keyword = Request.Form["txtKeyword"];
            var sysNo = Request.Form["hidSysNo"];
            bool bl;
            if (!string.IsNullOrEmpty(sysNo) && Convert.ToInt32(sysNo) != 0)
            {
                var model = FeSearchKeywordBo.Instance.GetModel(Convert.ToInt32(sysNo));
                bl = keyword != model.keyword && FeSearchKeywordBo.Instance.FeSearchKeywordVerify(HttpUtility.HtmlEncode(keyword));
            }
            else
            {
                bl = FeSearchKeywordBo.Instance.FeSearchKeywordVerify(HttpUtility.HtmlEncode(keyword));
            }
            return Json(!bl, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 友情链接
        /// <summary>
        /// 友情链接
        /// </summary>
        /// <param name="id">页码(以后改名字)</param>
        /// <param name="status">状态</param>
        /// <param name="webSiteName">分类名称</param>
        /// <returns>友情链接列表</returns>
        /// <remarks>2013-06-21 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.FE1010001)]
        public ActionResult FeLinks(int? id, int? status, string webSiteName = "")
        {
            id = id ?? 1;
            if (status == null)
                status = -1;

            IDictionary<int, string> dictList = EnumUtil.ToDictionary(typeof(MarketingStatus.友情链接管理状态));
            ViewBag.DictList = dictList;

            var model = MkBlogrollBo.Instance.Seach((int)id, status, webSiteName);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_AjaxFeLinks", model);
            }
            return View(model);
        }

        /// <summary>
        /// 查看友情链接
        /// </summary>
        /// <param name="id">索引</param>
        /// <returns>友情链接视图</returns>
        /// <remarks>2013-06-21 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.FE1010002)]
        public ActionResult FeLinksView(int? id)
        {
            var model = BLL.Front.MkBlogrollBo.Instance.GetModel((int)id);
            return View(model);
        }

        /// <summary>
        /// 验证友情链接名称
        /// </summary>
        /// <param name="id">索引</param>
        /// <returns>添加情链接视图</returns>
        /// <remarks>2013-06-21 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.FE1010002)]
        public ActionResult FeLinksAdd(int? id)
        {

            if (!string.IsNullOrEmpty(id.ToString()))
            {
                var model = BLL.Front.MkBlogrollBo.Instance.GetModel((int)id);
                return View(model);
            }
            else
            {
                var model = new MkBlogroll();
                return View(model);
            }
        }

        /// <summary>
        /// 添加、修改友情链接
        /// </summary>
        /// <param name="frm">实体</param>
        /// <returns>成功、失败</returns>
        /// <remarks>2013-12-11 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.FE1010002)]
        public JsonResult FeLinksAddOrEdit(FormCollection frm)
        {
            bool result = false;
            string info = "操作失败";

            string sysNo = frm["SysNo"];
            string webSiteName = frm["WebSiteName"];
            string webSiteUrl = frm["WebSiteUrl"];
            string emailAddress = frm["EmailAddress"];
            string displayOrder = frm["DisplayOrder"];
            string siteDescription = frm["SiteDescription"];
            var model = new MkBlogroll();
            if (!sysNo.Equals("0"))
            {
                model = BLL.Front.MkBlogrollBo.Instance.GetModel(Convert.ToInt32(sysNo));
                model.WebSiteName = webSiteName;
                model.WebSiteUrl = webSiteUrl;
                model.EmailAddress = emailAddress;
                model.DisplayOrder = Convert.ToInt32(displayOrder);
                model.SiteDescription = siteDescription;
                model.Status = (int)MarketingStatus.友情链接管理状态.待审;
                model.CreatedBy = CurrentUser.Base.SysNo;
                model.CreatedDate = DateTime.Now;
            }
            else
            {
                model.WebSiteName = webSiteName;
                model.WebSiteUrl = webSiteUrl;
                model.EmailAddress = emailAddress;
                model.DisplayOrder = Convert.ToInt32(displayOrder);
                model.SiteDescription = siteDescription;
                model.Status = (int)MarketingStatus.友情链接管理状态.待审;
                model.CreatedBy = CurrentUser.Base.SysNo;
                model.CreatedDate = DateTime.Now;
            }

            if (!sysNo.Equals("0"))
            {
                if (BLL.Front.MkBlogrollBo.Instance.Update(model) > 0)
                {
                    result = true;
                    info = "修改友情链接成功！";
                }
                else
                {
                    result = false;
                    info = "修改友情链接失败！";
                }
            }
            else
            {
                //后台添加验证，防止重复提交 -- by 唐永勤  2014-05-06
                if (BLL.Front.MkBlogrollBo.Instance.Verify(model.WebSiteName, Convert.ToInt32(sysNo)))
                {
                    result = false;
                    info = "网站名称重复，添加友情链接失败！";
                }
                else
                {
                    if (BLL.Front.MkBlogrollBo.Instance.Insert(model) > 0)
                    {
                        result = true;
                        info = "添加友情链接成功！";
                    }
                    else
                    {
                        result = false;
                        info = "添加友情链接失败！";
                    }
                }
            }
            return Json(new { result = result, info = info });
        }

        /// <summary>
        /// 验证友情链接名称
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <param name="webSiteName">网站名称</param>
        /// <returns>成功、失败</returns>
        /// <remarks>2013-06-21 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.FE1010002)]
        public ActionResult FeLinksKeyChk(int sysNo, string webSiteName = null)
        {

            bool result = false;

            if (BLL.Front.MkBlogrollBo.Instance.Verify(webSiteName, sysNo))
            {
                result = false;
            }
            else
            {
                result = true;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 单条更新状态
        /// </summary>
        /// <param name="collocation">友情链接视图</param>
        /// <returns>更新友情链接状态</returns>
        /// <remarks>2013-06-25 苟治国 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.FE1010003)]
        public ActionResult FeLinksStatus(IList<Model.MkBlogroll> collocation)
        {
            bool result = false;
            string info = "操作失败";

            string str = "";
            if (collocation[0].Status == (int)MarketingStatus.友情链接管理状态.已审)
                str = "审核";
            else
                str = "取消审核";
            if (MkBlogrollBo.Instance.UpdateStatus(collocation, CurrentUser.Base.SysNo) > 0)
            {
                result = true;
                info = str + "成功！";
            }
            else
            {
                result = true;
                info = str + "失败！";
            }
            return Json(new { result = result, info = info });
        }

        /// <summary>
        /// 批量更新状态
        /// </summary>
        /// <param name="collocation">友情链接视图</param>
        /// <returns>更新友情链接状态</returns>
        /// <remarks>2013-06-25 苟治国 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.FE1010003)]
        public ActionResult FeLinksStatusAll(IList<Model.MkBlogroll> collocation)
        {
            bool result = false;
            string info = "操作失败";
            if (MkBlogrollBo.Instance.UpdateStatus(collocation, CurrentUser.Base.SysNo) > 0)
            {
                result = true;
                info = "批量启用成功！";
            }
            else
            {
                result = true;
                info = "批量启用失败！";
            }
            return Json(new { result = result, info = info });
        }
        #endregion

        #region 新闻管理
        /// <summary>
        /// 新闻管理
        /// </summary>
        /// <param name="id">索引</param>
        /// <param name="status">状态</param>
        /// <param name="title">标题</param>
        /// <returns>新闻管理</returns>
        /// <remarks>2014-01-15 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.FE1013001)]
        public ActionResult FeNews(int? id, int? status, string title = null)
        {
            id = id ?? 1;

            IDictionary<int, string> dictList = EnumUtil.ToDictionary(typeof(ForeStatus.新闻状态));
            ViewBag.DictList = dictList;

            var model = FeNewsBo.Instance.Seach((int)id, status, title);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_AjaxFeNews", model);
            }

            return View(model);
        }

        /// <summary>
        /// 添加、修改
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>添加、修改视图</returns>
        /// <remarks>2014-01-15 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.FE1013002)]
        public ActionResult FeNewsAddOrEdit(int sysNo)
        {
            var model = new Model.FeNews();
            if (!sysNo.Equals(0))
            {
                model = BLL.Front.FeNewsBo.Instance.GetModel(sysNo);
            }

            return View(model);
        }

        /// <summary>
        /// 单条更新状态
        /// </summary>
        /// <param name="collocation">新闻视图</param>
        /// <returns>更新新闻状态</returns>
        /// <remarks>2014-01-15 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.FE1013002)]
        public ActionResult FeNewsStatus(IList<Model.FeNews> collocation)
        {
            Result result = new Result();

            string str = "";
            if (collocation[0].Status == (int)MarketingStatus.友情链接管理状态.已审)
                str = "审核";
            else
                str = "取消审核";
            if (FeNewsBo.Instance.UpdateStatus(collocation, CurrentUser.Base.SysNo) > 0)
            {
                result.Status = true;
                result.Message = str + "成功！";
            }
            else
            {
                result.Status = true;
                result.Message = str + "失败！";
            }
            return Json(result);
        }

        /// <summary>
        /// 提交新闻数据
        /// </summary>
        /// <param name="feNew">新闻实体</param>
        /// <returns>Result</returns>
        /// <remarks>2014-01-15 苟治国 创建</remarks>
        /// <remarks>2016-06-12 罗远康 修改增加字段</remarks>
        [Privilege(PrivilegeCode.FE1013002)]
        public JsonResult FeNewsSubmit(Model.FeNews feNew)
        {
            Result result = new Result();

            var model = new Model.FeNews();
            DateTime ReleaseDate = DateTime.Now;
            if (feNew.ReleaseDate != DateTime.MinValue)
            {
                ReleaseDate = feNew.ReleaseDate;
            }


            if (!feNew.SysNo.Equals(0))
            {
                model = BLL.Front.FeNewsBo.Instance.GetModel(feNew.SysNo);
                model.Title = feNew.Title;
                model.HeadLine = feNew.HeadLine;
                model.Content = feNew.Content;
                model.Source = feNew.Source;
                model.SourceUri = feNew.SourceUri;
                model.Author = feNew.Author;
                model.DisplayOrder = feNew.DisplayOrder;
                model.Status = (int)ForeStatus.新闻状态.待审;
                model.CategorySysNo = (int)feNew.CategorySysNo;
                model.CoverImage = feNew.CoverImage;
                model.IsTop = feNew.IsTop;
                model.IsHot = feNew.IsHot;
                model.IsRecommend = feNew.IsRecommend;
                model.Tags = feNew.Tags;
                model.SeoTitle = feNew.SeoTitle;
                model.SeoKeyword = feNew.SeoKeyword;
                model.SeoDescription = feNew.SeoDescription;
                model.ReleaseDate = ReleaseDate;
            }
            else
            {
                model.Title = feNew.Title;
                model.HeadLine = feNew.HeadLine;
                model.Content = feNew.Content;
                model.Source = feNew.Source;
                model.SourceUri = feNew.SourceUri;
                model.Author = feNew.Author;
                model.DisplayOrder = feNew.DisplayOrder;
                model.Status = (int)ForeStatus.新闻状态.待审;
                model.CreatedBy = CurrentUser.Base.SysNo;
                model.CreatedDate = DateTime.Now;
                model.CategorySysNo = (int)feNew.CategorySysNo;
                model.CoverImage = feNew.CoverImage;
                model.IsTop = feNew.IsTop;
                model.IsHot = feNew.IsHot;
                model.IsRecommend = feNew.IsRecommend;
                model.Tags = feNew.Tags;
                model.SeoTitle = feNew.SeoTitle;
                model.SeoKeyword = feNew.SeoKeyword;
                model.SeoDescription = feNew.SeoDescription;
                model.ReleaseDate = ReleaseDate;
            }

            //数据重复性检测
            bool isExists = FeNewsBo.Instance.IsExists(model);
            if (isExists)
            {
                result.Status = false;
                result.StatusCode = -1;
                result.Message = "新闻名称已存在";
                return Json(result);
            }

            if (!feNew.SysNo.Equals(0))
            {
                int rows = BLL.Front.FeNewsBo.Instance.Update(model);
                if (rows > 0)
                {
                    result.Status = true;
                    result.Message = "修改新闻成功！";
                }
                else
                {
                    result.Status = false;
                    result.Message = "修改新闻失败！";
                }
            }
            else
            {
                int rows = BLL.Front.FeNewsBo.Instance.Insert(model);
                if (rows > 0)
                {
                    result.Status = true;
                    result.Message = "添加新闻成功！";
                }
                else
                {
                    result.Status = false;
                    result.Message = "添加新闻失败！";
                }
            }
            return Json(result);
        }

        /// <summary>
        /// 新闻商品关联
        /// </summary>
        /// <param name="newsSysNo">新闻编号</param>
        /// <returns>新闻商品关视图</returns>
        /// <remarks>2014-01-16 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.FE1013005)]
        public ActionResult FeNewsProductAssociation(int newsSysNo)
        {
            ViewBag.newsSysNo = newsSysNo;

            var model = FeNewsProductAssociationBo.Instance.Seach(newsSysNo);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_AjaxFeNewsProductAssociation", model);
            }
            return View(model);
        }

        /// <summary>
        /// 修改新闻关联商品
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>修改新闻关联商品页面</returns>
        /// <remarks>2014-01-16 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.FE1013006)]
        public ActionResult FeNewsProductAssociationEdit(int sysNo)
        {
            var model = FeNewsProductAssociationBo.Instance.GetModel(sysNo);
            return View(model);
        }

        /// <summary>
        /// 修改新闻关联商品
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <param name="displayOrder">排序号</param>
        /// <returns>Result</returns>
        /// <remarks>2014-01-16 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.FE1013006)]
        public ActionResult FeNewsProductAssociationSubmit(int sysNo, int displayOrder)
        {
            Result result = new Result();
            var model = FeNewsProductAssociationBo.Instance.GetEntity(sysNo);
            if (model != null)
            {
                model.DisplayOrder = displayOrder;
                if (FeNewsProductAssociationBo.Instance.Update(model) > 0)
                {
                    result.Status = true;
                    result.Message = "修改新闻关联商品成功！";
                }
                else
                {
                    result.Status = false;
                    result.Message = "修改新闻关联商品失败！";
                }
            }
            else
            {
                result.Status = false;
                result.Message = "修改新闻关联商品不存在！";
            }
            return Json(result);
        }

        /// <summary>
        /// 删除新闻关联商品
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>Result</returns>
        /// <remarks>2014-01-16 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.FE1013007)]
        public JsonResult FeNewsProductAssociationRemove(int sysNo)
        {
            var result = new Result();
            if (FeNewsProductAssociationBo.Instance.Delete(sysNo))
            {
                result.Status = true;
                result.Message = "删除商品成功！";
            }
            else
            {
                result.Status = true;
                result.Message = "删除商品失败！";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 插入关联商品
        /// </summary>
        /// <param name="newsSysNo">新闻编号</param>
        /// <param name="collocation">商品</param>
        /// <returns>Result</returns>
        /// <remarks>2014-01-16 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.FE1013006)]
        public JsonResult FeNewsProductAssociationInsert(int newsSysNo, string[] collocation)
        {
            Result result = new Result();
            try
            {
                int row = 0;

                foreach (var arr in collocation)
                {
                    int count = FeNewsProductAssociationBo.Instance.GetCount(newsSysNo, 0);
                    if (count < 12)
                    {
                        if (FeNewsProductAssociationBo.Instance.GetCount(newsSysNo, Convert.ToInt32(arr)) <= 0)
                        {
                            var model = new FeNewsProductAssociation()
                                {
                                    NewsSysNo = newsSysNo,
                                    ProductSysNo = Convert.ToInt32(arr),
                                    DisplayOrder = row
                                };
                            if (FeNewsProductAssociationBo.Instance.Insert(model) > 0)
                            {
                                result.Status = true;
                                result.Message = "添加商品成功！";
                            }
                            else
                            {
                                result.Status = false;
                                result.Message = "添加商品失败！";
                            }
                        }
                        else
                        {
                            result.Status = false;
                            result.Message = "添加的产品已存在！";
                        }
                        row++;
                    }
                    else
                    {
                        result.Status = false;
                        result.Message = "新闻关联产品只能添加12个！";
                    }
                }
            }
            catch (Exception)
            {
                result.Status = false;
                result.Message = "添加商品失败！";
                throw;
            }

            return Json(result);
        }

        #endregion

        #region 软件管理
        /// <summary>
        /// 软件分类管理界面
        /// </summary>
        /// <param name="id">分页页码</param>
        /// <returns></returns>
        /// <remarks>2014-01-16 唐永勤 创建</remarks>

        [Privilege(PrivilegeCode.FE1011001)]
        public ActionResult FeSoftCategory(int? id)
        {
            int pageIndex = id ?? 1;
            int status = 0;

            int.TryParse(Request.Params["status"], out status);
            string name = Request.Params["name"];
            string selector = Request.Params["selector"];
            if (!string.IsNullOrEmpty(selector) && selector == "selector")
            {
                status = (int)ForeStatus.软件分类状态.启用;
            }

            PagedList<FeSoftCategory> list = new PagedList<FeSoftCategory>();

            Pager<FeSoftCategory> pager = new Pager<FeSoftCategory>();
            pager.CurrentPage = pageIndex;
            pager.PageFilter = new FeSoftCategory { Status = status, Name = name };
            pager.PageSize = list.PageSize;

            if (Request.IsAjaxRequest())
            {
                pager = FeSoftCategoryBo.Instance.GetFeSoftCategoryList(pager);

                list = new PagedList<FeSoftCategory>
                {
                    TData = pager.Rows,
                    CurrentPageIndex = pager.CurrentPage,
                    TotalItemCount = pager.TotalRows,
                    PageSize = pager.PageSize
                };

                if (!string.IsNullOrEmpty(selector) && selector == "selector") //分类选择组件view层
                {
                    return PartialView("_AjaxFeSoftCategorySelector", list);
                }
                return PartialView("_AjaxFeSoftCategoryList", list);
            }
            return View();
        }

        /// <summary>
        /// 软件分类添加/修改界面
        /// </summary>
        /// <param></param>
        /// <returns>软件分类添加/修改界面</returns>
        /// <remarks>2014-01-16 唐永勤 创建</remarks>
        [Privilege(PrivilegeCode.FE1011201)]
        public ActionResult FeSoftCategoryAddOrEdit()
        {
            FeSoftCategory model = new FeSoftCategory();
            int sysno = 0;
            int.TryParse(Request.Params["sysno"], out sysno);
            if (sysno > 0)
            {
                model = FeSoftCategoryBo.Instance.GetEntity(sysno);
            }
            return View(model);
        }

        /// <summary>
        /// 软件分类添加/修改信息保存
        /// </summary>
        /// <param name="model">软件分类实体</param>
        /// <returns>保存结果</returns>
        /// <remarks>2014-01-16 唐永勤 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.FE1011201)]
        public ActionResult FeSoftCategoryAddOrEdit(FeSoftCategory model)
        {
            string des = model.SysNo > 0 ? "修改软件分类" : "创建软件分类";
            Result result = FeSoftCategoryBo.Instance.FeSoftCategorySave(model);
            if (result.Status)
            {
                BLL.Log.SysLog.Instance.Info(Model.WorkflowStatus.LogStatus.系统日志来源.后台, des, Model.WorkflowStatus.LogStatus.系统日志目标类型.用户, model.SysNo, CurrentUser.Base.SysNo);
            }
            return Json(result);
        }

        /// <summary>
        /// 更新软件分类状态
        /// </summary>
        /// <param></param>
        /// <returns>返回成功的行数</returns>
        /// <remarks>2013-06-24 唐永勤 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.FE1011601)]
        public ActionResult UpdateFeSoftCategoryStatus()
        {
            int status = 0, _result = 0, sysno = 0;
            int.TryParse(Request.Params["status"], out status);
            int.TryParse(Request.Params["sysno"], out sysno);
            //转化为int防止注入
            if (sysno > 0 && !string.IsNullOrEmpty(Request.Params["status"]))
            {
                _result = FeSoftCategoryBo.Instance.UpdateStatus((Hyt.Model.WorkflowStatus.ForeStatus.软件分类状态)status, sysno);
                if (_result > 0)
                {
                    string des = Hyt.Model.WorkflowStatus.ForeStatus.软件分类状态.启用.GetHashCode() == status ? "启用软件分类" : "禁用软件分类";
                    BLL.Log.SysLog.Instance.Info(Model.WorkflowStatus.LogStatus.系统日志来源.后台, des, Model.WorkflowStatus.LogStatus.系统日志目标类型.用户, sysno, CurrentUser.Base.SysNo);
                }
            }
            return Json(new { result = _result });
        }

        /// <summary>
        /// 软件分类选择组件
        /// </summary>
        /// <param></param>
        /// <returns>展示组件模型</returns>
        /// <remarks>2014-01-17 唐永勤 创建</remarks>
        [Privilege(PrivilegeCode.FE1011001)]
        public ActionResult FeSoftCategorySelector()
        {
            return View();
        }

        /// <summary>
        /// 软件下载管理界面
        /// </summary>
        /// <param name="id">分页页码</param>
        /// <returns></returns>
        /// <remarks>2014-01-17 唐永勤 创建</remarks>
        [Privilege(PrivilegeCode.FE1012001)]
        public ActionResult FeSoftware(int? id)
        {
            int pageIndex = id ?? 1;
            int status = 0;

            int.TryParse(Request.Params["status"], out status);
            string title = Request.Params["title"];

            PagedList<FeSoftware> list = new PagedList<FeSoftware>();

            Pager<FeSoftware> pager = new Pager<FeSoftware>();
            pager.CurrentPage = pageIndex;
            pager.PageFilter = new FeSoftware { Status = status, Title = title };
            pager.PageSize = list.PageSize;

            if (Request.IsAjaxRequest())
            {
                pager = FeSoftwareBo.Instance.GetList(pager);

                list = new PagedList<FeSoftware>
                {
                    TData = pager.Rows,
                    CurrentPageIndex = pager.CurrentPage,
                    TotalItemCount = pager.TotalRows,
                    PageSize = pager.PageSize
                };
                return PartialView("_AjaxFeSoftwareList", list);
            }
            return View();
        }

        /// <summary>
        /// 软件分类添加/修改界面
        /// </summary>
        /// <param></param>
        /// <returns>软件分类添加/修改界面</returns>
        /// <remarks>2014-01-17 唐永勤 创建</remarks>
        [Privilege(PrivilegeCode.FE1012201)]
        public ActionResult FeSoftwareAddOrEdit()
        {
            FeSoftware model = new FeSoftware();
            ViewBag.SoftwareList = new List<FeSoftwareList>();
            ViewBag.SoftwareCategoryName = "";

            int sysno = 0;
            int.TryParse(Request.Params["sysno"], out sysno);
            if (sysno > 0)
            {
                model = FeSoftwareBo.Instance.GetEntity(sysno);
                ViewBag.SoftwareList = FeSoftwareBo.Instance.GetListBySoftwareSysNo(sysno);
                ViewBag.SoftwareCategoryName = FeSoftCategoryBo.Instance.GetEntity(model.SoftCategorySysNo).Name;
            }
            return View(model);
        }

        /// <summary>
        /// 软件添加/修改信息保存
        /// </summary>
        /// <param name="model">软件实体</param>
        /// <returns>保存结果</returns>
        /// <remarks>2014-01-17 唐永勤 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.FE1012201)]
        public ActionResult FeSoftwareAddOrEdit(FeSoftware model)
        {
            string des = model.SysNo > 0 ? "修改软件下载" : "创建软件下载";
            IList<FeSoftwareList> list = new List<FeSoftwareList>();
            model.Description = HttpUtility.UrlDecode(model.Description);
            if (!string.IsNullOrEmpty(Request["items"]))
            {
                string items = Request["items"];
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                list = serializer.Deserialize<List<FeSoftwareList>>(items);
            }
            Result result = FeSoftwareBo.Instance.Save(model, list);
            if (result.Status)
            {
                BLL.Log.SysLog.Instance.Info(Model.WorkflowStatus.LogStatus.系统日志来源.后台, des, Model.WorkflowStatus.LogStatus.系统日志目标类型.用户, model.SysNo, CurrentUser.Base.SysNo);
            }
            return Json(result);
        }

        /// <summary>
        /// 更新软件状态
        /// </summary>
        /// <param></param>
        /// <returns>返回成功的行数</returns>
        /// <remarks>2014-01-21 唐永勤 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.FE1012601, PrivilegeCode.FE1012301)]
        public ActionResult UpdateFeSoftwareStatus()
        {
            int status = 0, _result = 0, sysno = 0;
            int.TryParse(Request.Params["status"], out status);
            int.TryParse(Request.Params["sysno"], out sysno);
            //转化为int防止注入
            if (sysno > 0 && !string.IsNullOrEmpty(Request.Params["status"]))
            {
                _result = FeSoftwareBo.Instance.UpdateStatus((Hyt.Model.WorkflowStatus.ForeStatus.软件下载状态)status, sysno);
                if (_result > 0)
                {
                    string des = "";
                    switch (status)
                    {
                        case (int)Hyt.Model.WorkflowStatus.ForeStatus.软件下载状态.待审:
                            des = "更新软件状态为待审";
                            break;
                        case (int)Hyt.Model.WorkflowStatus.ForeStatus.软件下载状态.已审:
                            des = "更新软件状态为已审";
                            break;
                        case (int)Hyt.Model.WorkflowStatus.ForeStatus.软件下载状态.作废:
                            des = "更新软件状态为作废";
                            break;
                        default:
                            des = "更新状态未知";
                            break;
                    }
                    BLL.Log.SysLog.Instance.Info(Model.WorkflowStatus.LogStatus.系统日志来源.后台, des, Model.WorkflowStatus.LogStatus.系统日志目标类型.用户, sysno, CurrentUser.Base.SysNo);
                }
            }
            return Json(new { result = _result });
        }

        /// <summary>
        /// 软件列表添加/修改界面
        /// </summary>
        /// <param></param>
        /// <returns>软件列表添加/修改界面</returns>
        /// <remarks>2014-01-17 唐永勤 创建</remarks>
        [Privilege(PrivilegeCode.FE1012201)]
        public ActionResult FeSoftwareListAddOrEdit()
        {
            FeSoftwareList model = new FeSoftwareList();
            int sysno = 0, index = 0;
            int.TryParse(Request.Params["sysno"], out sysno);
            int.TryParse(Request.Params["Index"], out index);
            ViewBag.Index = 0;
            if (sysno > 0)
            {
                model = FeSoftwareBo.Instance.GetFeSoftwareListEntity(sysno);
            }
            else if (index > 0)
            {
                int DisplayOrder = 0, SoftIcon = 0;
                int.TryParse(Request.Params["DisplayOrder"], out DisplayOrder);
                int.TryParse(Request.Params["SoftIcon"], out SoftIcon);
                model.DisplayOrder = DisplayOrder;
                model.DownloadUrl = Request.Params["DownloadUrl"];
                model.Name = Request.Params["Name"];
                model.SoftIcon = SoftIcon;
                model.SysNo = sysno;
                model.VersionNumber = Request.Params["VersionNumber"];
                ViewBag.Index = index;
            }
            return View(model);
        }

        /// <summary>
        /// 软件列表添加/修改界面
        /// </summary>
        /// <param></param>
        /// <returns>软件列表添加/修改界面</returns>
        /// <remarks>2014-01-17 唐永勤 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.FE1012201)]
        public ActionResult FeSoftwareListAddOrEdit(FeSoftwareList model)
        {
            string des = model.SysNo > 0 ? "修改软件列表" : "创建软件列表";
            int result = FeSoftwareBo.Instance.CreateSoftwareList(model);
            if (result > 0)
            {
                BLL.Log.SysLog.Instance.Info(Model.WorkflowStatus.LogStatus.系统日志来源.后台, des, Model.WorkflowStatus.LogStatus.系统日志目标类型.用户, model.SysNo, CurrentUser.Base.SysNo);
            }
            return Json(result);
        }
        #endregion

        /// <summary>
        /// 同步广告
        /// 王耀发 2016-1-12 创建
        /// </summary>
        /// <param name="DealerSysNo">分销商系统编号</param>
        /// <param name="CreatedBy">创建用户系统编号</param>
        /// <returns></returns>
        [Privilege(PrivilegeCode.FE1008701)]
        public ActionResult ProCreateFeAdvertItem()
        {
            int GroupSysNo = int.Parse(this.Request["GroupSysNo"]);
            int DealerSysNo = int.Parse(this.Request["DealerSysNo"]);
            Result result = new Result();
            int affectRows = FeAdvertItemBo.Instance.ProCreateFeAdvertItem(GroupSysNo,DealerSysNo, CurrentUser.Base.SysNo);
            if (affectRows >= 0)
            {
                result.Status = true;
                result.Message = "同步成功";
            }
            else
            {
                result.Status = false;
                result.Message = "同步失败";
            }
            return Json(result);
        }
        /// <summary>
        /// 同步商品项
        /// 王耀发 2016-1-12 创建
        /// </summary>
        /// <param name="DealerSysNo">分销商系统编号</param>
        /// <param name="CreatedBy">创建用户系统编号</param>
        /// <returns></returns>
        [Privilege(PrivilegeCode.FE1009701)]
        public ActionResult ProCreateFeProductItem()
        {
            int GroupSysNo = int.Parse(this.Request["GroupSysNo"]);
            int DealerSysNo = int.Parse(this.Request["DealerSysNo"]);
            Result result = new Result();
            int affectRows = FeProductItemBo.Instance.ProCreateFeProductItem(GroupSysNo, DealerSysNo, CurrentUser.Base.SysNo);
            if (affectRows >= 0)
            {
                result.Status = true;
                result.Message = "同步成功";
            }
            else
            {
                result.Status = false;
                result.Message = "同步失败";
            }
            return Json(result);
        }
    }
}