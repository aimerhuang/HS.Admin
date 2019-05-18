using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using System.Web.Mvc;
using System.Collections;
using Hyt.BLL.Authentication;
using Hyt.BLL.Feedback;
using Hyt.BLL.Front;
using Hyt.BLL.LevelPoint;
using Hyt.BLL.Log;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.SystemPredefined;
using Hyt.Model.WorkflowStatus;
using Hyt.Util;
using Hyt.BLL.CRM;
using Hyt.BLL.Extras;
using Hyt.Model.Transfer;
using Extra.SMS;
using Hyt.BLL.Sys;

namespace Hyt.Admin.Controllers
{
    /// <summary>
    /// 客户管理
    /// </summary>
    /// <remarks></remarks>
    public class CRMController : BaseController
    {
        /// <summary>
        /// 创建会员
        /// </summary>
        /// <returns>视图</returns>
        /// <remarks>2013-10-14 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.SO1002201, PrivilegeCode.SO1004201, PrivilegeCode.CR1004201, PrivilegeCode.LG1004101)]
        public ActionResult CreateCustomer()
        {
            return View();
        }

        #region 大宗采购
        /// <summary>
        /// 大宗采购分页查询视图
        /// </summary>
        /// <param name="id">起始页</param>
        /// <param name="commitDate">提交时间</param>
        /// <param name="searchStaus">状态</param>
        /// <param name="searchCompany">联系人公司</param>
        /// <param name="searchName">联系人名称</param>
        /// <returns>大宗采购信息列表</returns>
        /// <remarks>2013－06-25 杨晗 创建</remarks>
        [Privilege(PrivilegeCode.CR1004081)]
        public ActionResult CrBulkPurchase(int? id, DateTime? commitDate,
                                           int? searchStaus, string searchCompany = null,
                                           string searchName = null)
        {
            IDictionary<int, string> dictList = EnumUtil.ToDictionary(typeof(CustomerStatus.大宗采购状态));
            ViewBag.DictList = dictList;
            id = id ?? 1;
            searchStaus = searchStaus ?? (int)CustomerStatus.大宗采购状态.待处理;
            var model = CrBulkPurchaseBo.Instance.Seach((int)id, commitDate, searchStaus, searchCompany, searchName);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_AjaxCrBulkPurchaseList", model);
            }
            return View(model);
        }

        /// <summary>
        /// 处理大宗采购视图
        /// </summary>
        /// <param name="sysNo">大宗采购系统号</param>
        /// <returns>大宗采购某条数据视图</returns>
        /// <remarks>2013－06-26 杨晗 创建</remarks>
        [Privilege(PrivilegeCode.CR1004082)]
        public ActionResult CrBulkPurchaseEdit(int sysNo)
        {
            CrBulkPurchase model = CrBulkPurchaseBo.Instance.GetModel(sysNo);
            return View(model);
        }

        /// <summary>
        /// 处理大宗采购
        /// </summary>
        /// <param name="sysNo">大宗采购系统号</param>
        /// <param name="status">处理状态</param>
        /// <returns>处理大宗采购某条数据成功或失败信息</returns>
        /// <remarks>2013－06-26 杨晗 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.CR1004082)]
        public ActionResult CrBulkPurchaseEdit(int sysNo, int status)
        {

            CrBulkPurchase model = CrBulkPurchaseBo.Instance.GetModel(sysNo);
            model.HandleDate = DateTime.Now;
            model.HandlerSysNo = CurrentUser.Base.SysNo;
            model.Status = status;
            int count = CrBulkPurchaseBo.Instance.Update(model);
            return Json(new { IsPass = count > 0 });
        }

        #endregion

        #region 客户投诉

        /// <summary>
        /// 会员投诉分页查询
        /// </summary>
        /// <param name="id">分页索引</param>
        /// <param name="status">状态</param>
        /// <param name="complainType">类型</param>
        /// <param name="orderSysNo">订单编号</param>
        /// <param name="replyerType">投诉回复类型</param>
        /// <param name="customerSysNo">会员系统编号</param>
        /// <param name="mobilePhoneNumber">会员投诉手机号</param>
        /// <returns>view CrComplaint</returns>
        /// <remarks>2014-01-24 黄伟 添加缺少的注释</remarks>
        [Privilege(PrivilegeCode.CR1001001)]
        public ActionResult CrComplaint(int? id, int? status, int? complainType, int? orderSysNo, int? replyerType, int? customerSysNo, string mobilePhoneNumber = null)
        {
            IDictionary<int, string> dictstatus = EnumUtil.ToDictionary(typeof(CustomerStatus.会员投诉状态));
            ViewBag.Dictstatus = dictstatus;
            IDictionary<int, string> dictComplainType = EnumUtil.ToDictionary(typeof(CustomerStatus.会员投诉类型));
            ViewBag.dictComplainType = dictComplainType;
            IDictionary<int, string> dictReplyerType = EnumUtil.ToDictionary(typeof(CustomerStatus.会员投诉回复类型));
            ViewBag.dictReplyerType = dictReplyerType;
            id = id ?? 1;
            var model = BLL.CRM.CrComplaintBo.Instance.Seach((int)id, status, complainType, replyerType, orderSysNo, customerSysNo, mobilePhoneNumber);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_AjaxCrComplaint", model);
            }
            return View(model);
        }

        /// <summary>
        /// 处理会员投诉视图
        /// </summary>
        /// <param name="sysNo">会员投诉统号</param>
        /// <returns>会员投诉据视图</returns>
        /// <remarks>2013－07-09 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.CR1001002)]
        public ActionResult CrComplaintEdit(int sysNo)
        {
            CBCrComplaint model = BLL.CRM.CrComplaintBo.Instance.GetModel(sysNo);
            ViewBag.List = BLL.CRM.CrComplaintReplyBo.Instance.Seach(sysNo);
            return View(model);
        }

        /// <summary>
        /// 处理会员投诉视图
        /// </summary>
        /// <param name="sysNo">会员投诉统号</param>
        /// <returns>会员投诉据视图</returns>
        /// <remarks>2013－07-09 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.CR1001002)]
        public ActionResult CrComplaintView(int sysNo)
        {
            CBCrComplaint model = BLL.CRM.CrComplaintBo.Instance.GetModel(sysNo);
            ViewBag.List = BLL.CRM.CrComplaintReplyBo.Instance.Seach(sysNo);
            return View(model);
        }

        /// <summary>
        /// 作废会员投诉
        /// </summary>
        /// <param name="sysNo">会员投诉统号</param>
        /// <returns>会员投诉据视图</returns>
        /// <remarks>2013－07-09 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.CR1001501)]
        public ActionResult CrComplaintUpdate(int sysNo)
        {
            bool result = false;
            string info = "操作失败";
            var model = BLL.CRM.CrComplaintBo.Instance.GetModelSingle(sysNo);
            model.Status = (int)CustomerStatus.会员投诉状态.作废;
            if (BLL.CRM.CrComplaintBo.Instance.Update(model) > 0)
            {
                result = true;
                info = "作废成功！";
            }
            else
            {
                result = true;
                info = "作废失败！";
            }
            return Json(new { result = result, info = info });
        }

        /// <summary>
        /// 作废会员投诉  关闭投诉/已处理
        /// </summary>
        /// <param name="sysNo">会员投诉统号</param>
        /// <returns>会员投诉据视图</returns>
        /// <remarks>2013－07-09 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.CR1001501)]
        public ActionResult CrComplaintClose(int sysNo)
        {
            bool result = false;
            string info = "操作失败";

            var replymodel = BLL.CRM.CrComplaintReplyBo.Instance.GetReplyTop(sysNo);
            if (replymodel != null)
            {
                if (replymodel.ReplyerType == (int)CustomerStatus.会员投诉回复类型.客服回复)
                {
                    var model = BLL.CRM.CrComplaintBo.Instance.GetModelSingle(sysNo);
                    model.Status = (int)CustomerStatus.会员投诉状态.已处理;
                    if (BLL.CRM.CrComplaintBo.Instance.Update(model) > 0)
                    {
                        result = true;
                        info = "关闭投诉成功！";
                    }
                    else
                    {
                        result = true;
                        info = "关闭投诉失败！";
                    }
                }
                else
                {
                    info = "操作失败,您还没有回复";
                }
            }
            else
            {
                info = "操作失败,您还没有回复记录";
            }
            return Json(new { result = result, info = info });
        }
        #endregion

        #region 会员投诉回复
        /// <summary>
        /// 处理会员投诉视图
        /// </summary>
        /// <param name="sysNo">会员投诉统号</param>
        /// <returns>会员投诉据视图</returns>
        /// <remarks>2013－07-09 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.CR1001002)]
        public JsonResult CrComplaintReply(int sysNo)
        {
            var list = BLL.CRM.CrComplaintReplyBo.Instance.Seach(sysNo);
            ViewBag.List = list;
            return Json(from item in list
                        select
                            new
                                {
                                    UserName = item.UserName,
                                    ReplyContent = item.ReplyContent,
                                    ReplyDate = item.ReplyDate.ToString()
                                });
        }

        /// <summary>
        /// 处理会员投诉视图
        /// </summary>
        /// <returns>会员投诉回复据视图</returns>
        /// <remarks>2013－07-09 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.CR1001301)]
        public ActionResult CrComplaintReplyAddOrEdit()
        {
            string strContent = Request.Form["replyContent"];
            string hidSysNo = Request.Form["hidSysNo"];
            bool result = false;
            string info = "操作失败";
            string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            if (!string.IsNullOrEmpty(hidSysNo))
            {
                var model = new Model.CrComplaintReply();
                model.ComplaintSysNo = Convert.ToInt32(hidSysNo);
                model.ReplyerSysNo = AdminAuthenticationBo.Instance.GetAuthenticatedUser().SysNo;
                model.ReplyContent = strContent;
                model.ReplyerType = (int)CustomerStatus.会员投诉回复类型.客服回复;
                model.ReplyDate = Convert.ToDateTime(date);

                //先插入回复记录
                if (CrComplaintReplyBo.Instance.Insert(model) > 0)
                {
                    //在更新投诉状态
                    var complaint = BLL.CRM.CrComplaintBo.Instance.GetModelSingle(Convert.ToInt32(hidSysNo));
                    complaint.Status = (int)CustomerStatus.会员投诉状态.处理中;
                    complaint.LastUpdateBy = AdminAuthenticationBo.Instance.GetAuthenticatedUser().SysNo;
                    complaint.LastUpdateDate = Convert.ToDateTime(date);
                    if (BLL.CRM.CrComplaintBo.Instance.Update(complaint) > 0)
                    {
                        result = true;
                        info = "回复成功！";
                    }
                }
                else
                {
                    result = false;
                    info = "回复失败！";
                }
            }
            return Json(new { result = result, info = info });
        }
        #endregion

        #region 会员等级
        /// <summary>
        /// 会员等级管理
        /// </summary>
        /// <param name="canPayForProduct">惠源币是否可用于支付货款</param>
        /// <param name="canPayForService">惠源币是否可用于支付服务</param>
        /// <returns>等级管理页面</returns>
        /// <remarks>2013-07-09 黄波 创建</remarks>
        /// <remarks>2013-07-11 苟治国 修改</remarks>
        [Privilege(PrivilegeCode.CR1005001)]
        public ActionResult CrLevel(int? canPayForProduct, int? canPayForService)
        {
            IDictionary<int, string> dictPayForProduct = EnumUtil.ToDictionary(typeof(CustomerStatus.惠源币是否可用于支付货款));
            ViewBag.dictPayForProduct = dictPayForProduct;
            IDictionary<int, string> dictPayForService = EnumUtil.ToDictionary(typeof(CustomerStatus.惠源币是否可用于支付服务));
            ViewBag.dictPayForService = dictPayForService;

            if (canPayForProduct == null)
                canPayForProduct = -1;
            if (canPayForService == null)
                canPayForService = -1;

            ViewBag.LevelList = BLL.CRM.CrCustomerLevelBo.Instance.Seach(canPayForProduct, canPayForService);
            if (Request.IsAjaxRequest())
            {
                var model = new Model.CrCustomerLevel();
                return PartialView("_AjaxCrLevel", model);
            }
            return View();
        }

        /// <summary>
        /// 会员等级编辑
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>会员等级编辑页面</returns>
        /// <remarks>2013-07-09 黄波 创建</remarks>
        /// <remarks>2013-07-11 苟治国 修改</remarks>
        [Privilege(PrivilegeCode.CR1005201)]
        public ActionResult CrLevelEdit(int sysNo)
        {
            IDictionary<int, string> dictPayForProduct = EnumUtil.ToDictionary(typeof(CustomerStatus.惠源币是否可用于支付货款));
            ViewBag.dictPayForProduct = dictPayForProduct;
            IDictionary<int, string> dictPayForService = EnumUtil.ToDictionary(typeof(CustomerStatus.惠源币是否可用于支付服务));
            ViewBag.dictPayForService = dictPayForService;

            CrCustomerLevel model;
            if (sysNo != 0)
            {
                model = BLL.CRM.CrCustomerLevelBo.Instance.GetCustomerLevel(sysNo);
            }
            else
            {
                model = new CrCustomerLevel();
            }
            return View(model);
        }

        /// <summary>
        /// 获取会员等级区间数
        /// </summary>
        /// <param name="list">会员等级列表</param>
        /// <returns>区间数据</returns>
        /// <remarks>2013-07-11 苟治国 修改</remarks>
        private ArrayList GetArr(IList<Model.CrCustomerLevel> list)
        {
            ArrayList al = new ArrayList();
            foreach (var item in list)
            {
                for (int i = item.LowerLimit; i <= item.UpperLimit; i++)
                {
                    al.Add(i);
                }
            }
            return al;
        }

        /// <summary>
        /// 会员等级添加/编辑
        /// </summary>
        /// <returns>会员等级编辑页面</returns>
        /// <remarks>2013-07-09 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.CR1005201)]
        public ActionResult CrLevelAddOrEdit()
        {
            bool result = false, isresult = false;
            string info = "操作失败";
            int hidSysNo = -1;

            string sysNo = Request.Form["hidSysNo"];

            string LevelName = Request.Form["LevelName"];
            string LowerLimit = Request.Form["LowerLimit"];
            string UpperLimit = Request.Form["UpperLimit"];
            string CanPayForProduct = Request.Form["CanPayForProduct"];
            string ProductPaymentUpperLimit = Request.Form["ProductPaymentUpperLimit"];
            string ProductPaymentPercentage = Request.Form["ProductPaymentPercentage"];
            string CanPayForService = Request.Form["CanPayForService"];
            string ServicePaymentUpperLimit = Request.Form["ServicePaymentUpperLimit"];
            string ServicePaymentPercentage = Request.Form["ServicePaymentPercentage"];
            string LevelDescription = Request.Form["LevelDescription"];

            int seachSysNo = -1;
            if (!sysNo.Equals("0"))
            {
                seachSysNo = Convert.ToInt32(sysNo);
            }

            try
            {
                ArrayList arr = GetArr(BLL.CRM.CrCustomerLevelBo.Instance.compare(seachSysNo));
                if (arr.Contains(Convert.ToInt32(LowerLimit)))
                {
                    isresult = true;
                }
                if (arr.Contains(Convert.ToInt32(UpperLimit)))
                {
                    isresult = true;
                }
            }
            catch
            {
                result = false;
                info = "请检查您所设置的等级衔接是否存在问题！";
            }
            if (!isresult)
            {
                var model = new CrCustomerLevel();
                if (!sysNo.Equals("0"))
                {
                    model = BLL.CRM.CrCustomerLevelBo.Instance.GetCustomerLevel(Convert.ToInt32(sysNo));

                    model.LevelName = LevelName;
                    model.LowerLimit = string.IsNullOrWhiteSpace(LowerLimit) ? 0 : Convert.ToInt32(LowerLimit);
                    model.UpperLimit = string.IsNullOrWhiteSpace(UpperLimit) ? 0 : Convert.ToInt32(UpperLimit);
                    model.CanPayForProduct = string.IsNullOrWhiteSpace(CanPayForProduct) ? 0 : Convert.ToInt32(CanPayForProduct);
                    model.ProductPaymentUpperLimit = string.IsNullOrWhiteSpace(ProductPaymentUpperLimit) ? 0 : Convert.ToInt32(ProductPaymentUpperLimit);
                    model.ProductPaymentPercentage = string.IsNullOrWhiteSpace(ProductPaymentPercentage) ? 0m : Convert.ToDecimal(ProductPaymentPercentage);
                    model.CanPayForService = string.IsNullOrWhiteSpace(CanPayForService) ? 0 : Convert.ToInt32(CanPayForService);
                    model.ServicePaymentUpperLimit = string.IsNullOrWhiteSpace(ServicePaymentUpperLimit) ? 0 : Convert.ToInt32(ServicePaymentUpperLimit);
                    model.ServicePaymentPercentage = string.IsNullOrWhiteSpace(ServicePaymentPercentage) ? 0m : Convert.ToDecimal(ServicePaymentPercentage);
                    model.LevelDescription = LevelDescription;
                }
                else
                {
                    model.LevelName = LevelName;
                    model.LowerLimit = string.IsNullOrWhiteSpace(LowerLimit) ? 0 : Convert.ToInt32(LowerLimit);
                    model.UpperLimit = string.IsNullOrWhiteSpace(UpperLimit) ? 0 : Convert.ToInt32(UpperLimit);
                    model.CanPayForProduct = string.IsNullOrWhiteSpace(CanPayForProduct) ? 0 : Convert.ToInt32(CanPayForProduct);
                    model.ProductPaymentUpperLimit = string.IsNullOrWhiteSpace(ProductPaymentUpperLimit) ? 0 : Convert.ToInt32(ProductPaymentUpperLimit);
                    model.ProductPaymentPercentage = string.IsNullOrWhiteSpace(ProductPaymentPercentage) ? 0m : Convert.ToDecimal(ProductPaymentPercentage);
                    model.CanPayForService = string.IsNullOrWhiteSpace(CanPayForService) ? 0 : Convert.ToInt32(CanPayForService);
                    model.ServicePaymentUpperLimit = string.IsNullOrWhiteSpace(ServicePaymentUpperLimit) ? 0 : Convert.ToInt32(ServicePaymentUpperLimit);
                    model.ServicePaymentPercentage = string.IsNullOrWhiteSpace(ServicePaymentPercentage) ? 0m : Convert.ToDecimal(ServicePaymentPercentage);
                    model.LevelDescription = LevelDescription;
                }
                if (!sysNo.Equals("0"))
                {
                    if (CrCustomerLevelBo.Instance.Update(model) > 0)
                    {
                        result = true;
                        info = "修改会员等级成功！";
                    }
                    else
                    {
                        result = false;
                        info = "修改会员等级失败！";
                    }
                }
                else
                {
                    if (CrCustomerLevelBo.Instance.Insert(model) > 0)
                    {
                        result = true;
                        info = "添加会员等级成功！";
                    }
                    else
                    {
                        result = false;
                        info = "添加会员等级失败！";
                    }
                }
            }
            else
            {
                info = "等级积分上限/下限有存在";
            }
            return Json(new { result = result, info = info });
        }
        #endregion

        #region 会员管理

        #region 会员
        /// <summary>
        /// 会员管理
        /// </summary>
        /// <param name="id">分面索引</param>
        /// <param name="status">状态</param>
        /// <param name="levelSysNo">会员等级</param>
        /// <param name="emailStatus">邮箱状态</param>
        /// <param name="mobilePhoneStatus">手机状态</param>
        /// <param name="isReceiveEmail">是否接收邮</param>
        /// <param name="isReceiveShortMessage">是否接收短信</param>
        /// <param name="isPublicAccount">是否是公共账户</param>
        /// <param name="isLevelFixed">等级是否固定</param>
        /// <param name="isExperiencePointFixed">经验积分是否固定</param>
        /// <param name="isExperienceCoinFixed">惠源币是否固定</param>
        /// <param name="account">账号</param>
        /// <returns>会员管理页面</returns>
        /// <remarks>2013-07-11 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.CR1004001, PrivilegeCode.CR1004201, PrivilegeCode.CR1004101, PrivilegeCode.CR1004601)]
        public ActionResult CrCustomer(int? id, int? status, int? levelSysNo, int? emailStatus, int? mobilePhoneStatus, int? isReceiveEmail, int? isReceiveShortMessage, int? isPublicAccount, int? isLevelFixed, int? isExperiencePointFixed, int? isExperienceCoinFixed, string account = null, int agentSysNo = 0, int dealerSysNo = 0)
        {
            IDictionary<int, string> dictStatus = EnumUtil.ToDictionary(typeof(CustomerStatus.会员状态));
            ViewBag.dictStatus = dictStatus;
            IDictionary<int, string> dictEmailStatus = EnumUtil.ToDictionary(typeof(CustomerStatus.邮箱状态));
            ViewBag.dictEmailStatus = dictEmailStatus;
            IDictionary<int, string> dictMobilePhoneStatus = EnumUtil.ToDictionary(typeof(CustomerStatus.手机状态));
            ViewBag.dictMobilePhoneStatus = dictMobilePhoneStatus;

            #region 2014-02-18 苟治国新增
            IDictionary<int, string> dictIsReceiveEmail = EnumUtil.ToDictionary(typeof(CustomerStatus.是否接收邮件));
            ViewBag.dictIsReceiveEmail = dictIsReceiveEmail;

            IDictionary<int, string> dictIsReceiveShortMessage = EnumUtil.ToDictionary(typeof(CustomerStatus.是否接收短信));
            ViewBag.dictIsReceiveShortMessage = dictIsReceiveShortMessage;

            IDictionary<int, string> dictIsPublicAccount = EnumUtil.ToDictionary(typeof(CustomerStatus.是否是公共账户));
            ViewBag.dictIsPublicAccount = dictIsPublicAccount;

            IDictionary<int, string> dictIsLevelFixed = EnumUtil.ToDictionary(typeof(CustomerStatus.等级是否固定));
            ViewBag.dictIsLevelFixed = dictIsLevelFixed;

            IDictionary<int, string> dictIsExperiencePointFixed = EnumUtil.ToDictionary(typeof(CustomerStatus.经验积分是否固定));
            ViewBag.dictIsExperiencePointFixed = dictIsExperiencePointFixed;

            IDictionary<int, string> dictIsExperienceCoinFixed = EnumUtil.ToDictionary(typeof(CustomerStatus.惠源币是否固定));
            ViewBag.dictIsExperienceCoinFixed = dictIsExperienceCoinFixed;
            #endregion

            ViewBag.Level = BLL.CRM.CrCustomerLevelBo.Instance.Seach(-1, -1);

            id = id ?? 1;

            //当前用户对应分销商，2016-02-17 王耀发 创建
            ParaIsDealerFilter filter = new ParaIsDealerFilter();
            if (CurrentUser.IsBindDealer)
            {
                int DealerSysNo = CurrentUser.Dealer.SysNo;
                filter.DealerSysNo = DealerSysNo;
                filter.IsBindDealer = CurrentUser.IsBindDealer;
            }
            //是否绑定所有经销商
            filter.IsBindAllDealer = CurrentUser.IsBindAllDealer;
            filter.DealerCreatedBy = CurrentUser.Base.SysNo;
            filter.SelectedAgentSysNo = agentSysNo;
            filter.SelectedDealerSysNo = dealerSysNo;

            var model = BLL.CRM.CrCustomerBo.Instance.Seach((int)id, status, levelSysNo, emailStatus, mobilePhoneStatus, isReceiveEmail, isReceiveShortMessage, isPublicAccount, isLevelFixed, isExperiencePointFixed, isExperienceCoinFixed, account, filter);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_AjaxCrCustomer", model);
            }
            ViewBag.CreatorSysNo = CurrentUser.Base.SysNo;
            return View(model);
        }

        /// <summary>
        /// 会员管理
        /// </summary>
        /// <param name="id">分面索引</param>
        /// <param name="status">状态</param>
        /// <param name="levelSysNo">会员等级</param>
        /// <param name="emailStatus">邮箱状态</param>
        /// <param name="mobilePhoneStatus">手机状态</param>
        /// <param name="isReceiveEmail">是否接收邮</param>
        /// <param name="isReceiveShortMessage">是否接收短信</param>
        /// <param name="isPublicAccount">是否是公共账户</param>
        /// <param name="isLevelFixed">等级是否固定</param>
        /// <param name="isExperiencePointFixed">经验积分是否固定</param>
        /// <param name="isExperienceCoinFixed">惠源币是否固定</param>
        /// <param name="account">账号</param>
        /// <returns>会员管理页面</returns>
        /// <remarks>2015-09-12 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.CR1004001, PrivilegeCode.CR1004201, PrivilegeCode.CR1004101, PrivilegeCode.CR1004601)]
        public ActionResult CrCustomerSelector(int? id, int? status, int? levelSysNo, int? emailStatus, int? mobilePhoneStatus, int? isReceiveEmail, int? isReceiveShortMessage, int? isPublicAccount, int? isLevelFixed, int? isExperiencePointFixed, int? isExperienceCoinFixed, string account = null)
        {
            IDictionary<int, string> dictStatus = EnumUtil.ToDictionary(typeof(CustomerStatus.会员状态));
            ViewBag.dictStatus = dictStatus;
            IDictionary<int, string> dictEmailStatus = EnumUtil.ToDictionary(typeof(CustomerStatus.邮箱状态));
            ViewBag.dictEmailStatus = dictEmailStatus;
            IDictionary<int, string> dictMobilePhoneStatus = EnumUtil.ToDictionary(typeof(CustomerStatus.手机状态));
            ViewBag.dictMobilePhoneStatus = dictMobilePhoneStatus;

            #region 2014-02-18 苟治国新增
            IDictionary<int, string> dictIsReceiveEmail = EnumUtil.ToDictionary(typeof(CustomerStatus.是否接收邮件));
            ViewBag.dictIsReceiveEmail = dictIsReceiveEmail;

            IDictionary<int, string> dictIsReceiveShortMessage = EnumUtil.ToDictionary(typeof(CustomerStatus.是否接收短信));
            ViewBag.dictIsReceiveShortMessage = dictIsReceiveShortMessage;

            IDictionary<int, string> dictIsPublicAccount = EnumUtil.ToDictionary(typeof(CustomerStatus.是否是公共账户));
            ViewBag.dictIsPublicAccount = dictIsPublicAccount;

            IDictionary<int, string> dictIsLevelFixed = EnumUtil.ToDictionary(typeof(CustomerStatus.等级是否固定));
            ViewBag.dictIsLevelFixed = dictIsLevelFixed;

            IDictionary<int, string> dictIsExperiencePointFixed = EnumUtil.ToDictionary(typeof(CustomerStatus.经验积分是否固定));
            ViewBag.dictIsExperiencePointFixed = dictIsExperiencePointFixed;

            IDictionary<int, string> dictIsExperienceCoinFixed = EnumUtil.ToDictionary(typeof(CustomerStatus.惠源币是否固定));
            ViewBag.dictIsExperienceCoinFixed = dictIsExperienceCoinFixed;
            #endregion

            ViewBag.Level = BLL.CRM.CrCustomerLevelBo.Instance.Seach(-1, -1);

            id = id ?? 1;
            status = (int)CustomerStatus.会员状态.有效;
            mobilePhoneStatus = (int)CustomerStatus.手机状态.已验证;

            //当前用户对应分销商，2016-02-17 王耀发 创建
            ParaIsDealerFilter filter = new ParaIsDealerFilter();
            if (CurrentUser.IsBindDealer)
            {
                int DealerSysNo = CurrentUser.Dealer.SysNo;
                filter.DealerSysNo = DealerSysNo;
                filter.IsBindDealer = CurrentUser.IsBindDealer;
            }
            //是否绑定所有经销商
            filter.IsBindAllDealer = CurrentUser.IsBindAllDealer;
            filter.DealerCreatedBy = CurrentUser.Base.SysNo;
            filter.SelectedAgentSysNo = -1;
            filter.SelectedDealerSysNo = 0;

            var model = BLL.CRM.CrCustomerBo.Instance.Seach((int)id, status, levelSysNo, emailStatus, mobilePhoneStatus, isReceiveEmail, isReceiveShortMessage, isPublicAccount, isLevelFixed, isExperiencePointFixed, isExperienceCoinFixed, account, filter);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_AjaxCrCustomerSelector", model);
            }
            ViewBag.CreatorSysNo = CurrentUser.Base.SysNo;
            return View(model);
        }

        /// <summary>
        /// 查看会员详细
        /// </summary>
        /// <param name="sysNo">会员编号</param>
        /// <returns>会员详细页面</returns>
        /// <remarks>2013-07-11 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.CR1004101)]
        public ActionResult CrCustomerDetail(int sysNo)
        {
            string strArea = "", strCity = "", strProvince = "";
            var model = Hyt.BLL.CRM.CrCustomerBo.Instance.GetModel(sysNo);
            if (model.AreaSysNo != 0)
            {
                var areaModel = BLL.Basic.BasicAreaBo.Instance.GetArea(model.AreaSysNo);
                if (areaModel != null)
                {
                    strArea = areaModel.AreaName;
                    var cityModel = BLL.Basic.BasicAreaBo.Instance.GetArea(areaModel.ParentSysNo);
                    if (cityModel != null)
                    {
                        strCity = cityModel.AreaName;
                        var provinceModel = BLL.Basic.BasicAreaBo.Instance.GetArea(cityModel.ParentSysNo);
                        if (provinceModel != null)
                        {
                            strProvince = provinceModel.AreaName;
                        }
                    }
                }
            }
            ViewBag.Area = strProvince + strCity + strArea + " " + model.StreetAddress;
            return View(model);
        }

        /// <summary>
        /// 查询用户的收货地址
        /// </summary>
        /// <param name="customerSysNo">会员ID</param>
        /// <returns>收货地址列表</returns>
        /// <remarks>2016－04-16 王耀发 创建</remarks>

        [Privilege(PrivilegeCode.CR1004101)]
        public ActionResult LoadCustomerAddress(int sysNo)
        {
            var model = Hyt.BLL.CRM.CrCustomerBo.Instance.LoadCustomerAddress(sysNo);
            return View(model);
        }

        /// <summary>
        /// Crm查看会员详细
        /// </summary>
        /// <param name="sysNo">会员编号</param>
        /// <returns>会员详细页面</returns>
        /// <remarks>2014-04-15 余勇 创建</remarks>
        [Privilege(PrivilegeCode.CR1004101)]
        public ActionResult CrCustomerEditDetail(int sysNo)
        {
            string strArea = "", strCity = "", strProvince = "";
            var model = Hyt.BLL.CRM.CrCustomerBo.Instance.GetModel(sysNo);
            if (model.AreaSysNo != 0)
            {
                var areaModel = BLL.Basic.BasicAreaBo.Instance.GetArea(model.AreaSysNo);
                if (areaModel != null)
                {
                    strArea = areaModel.AreaName;
                    var cityModel = BLL.Basic.BasicAreaBo.Instance.GetArea(areaModel.ParentSysNo);
                    if (cityModel != null)
                    {
                        strCity = cityModel.AreaName;
                        var provinceModel = BLL.Basic.BasicAreaBo.Instance.GetArea(cityModel.ParentSysNo);
                        if (provinceModel != null)
                        {
                            strProvince = provinceModel.AreaName;
                        }
                    }
                }
            }
            ViewBag.Area = strProvince + strCity + strArea;
            return View(model);
        }

        /// <summary>
        /// 更新会员状态
        /// </summary>
        /// <param name="sysNo">会员编号</param>
        /// <returns>会员空视图</returns>
        /// <remarks>2013-07-11 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.CR1004601)]
        public ActionResult CrCustomerUpdateStatus(int sysNo)
        {
            bool result = false;
            string info = "操作失败";

            var model = Hyt.BLL.CRM.CrCustomerBo.Instance.GetCrCustomerItem(sysNo);
            model.Status = model.Status == (int)CustomerStatus.会员状态.有效 ? (int)CustomerStatus.会员状态.无效 : (int)CustomerStatus.会员状态.有效;
            if (BLL.CRM.CrCustomerBo.Instance.Update(model) > 0)
            {
                result = true;
                info = "更新成功！";
            }
            else
            {
                result = true;
                info = "更新失败！";
            }
            return Json(new { result = result, info = info });
        }

        /// <summary>
        /// 更新会员密码
        /// </summary>
        /// <param name="customerSysNo">会员编号</param>
        /// <returns>更新会员视图</returns>
        /// <remarks>2013-07-24 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.CR1004202)]
        public ActionResult CrCustomerPassword(int customerSysNo)
        {
            var model = new CrCustomer();
            model.SysNo = customerSysNo;
            model.Password = Hyt.Util.WebUtil.Number(6, true);
            return View(model);
        }

        /// <summary>
        /// 更新会员密码
        /// </summary>
        /// <param name="sysNo">会员编号</param>
        /// <returns>成功或失败信息</returns>
        /// <remarks>2013-07-24 苟治国 创建</remarks>
        /// <remarks>2014-06-27 余勇 修改 调用SSO服务</remarks>
        [Privilege(PrivilegeCode.CR1004202)]
        public ActionResult CrCustomerEditPassword(int sysNo)
        {
            string oldPass = string.Empty;
            string newPass = Request.Form["Password"];

            var model = BLL.CRM.CrCustomerBo.Instance.GetCrCustomerItem(sysNo);
            oldPass = model.Password;
            model.Password = Util.EncryptionUtil.EncryptWithMd5AndSalt(newPass);

            //发短信
            var result = Hyt.BLL.Extras.SmsBO.Instance.发送新密码短信(model.MobilePhoneNumber, model.Name, newPass);
            //若短信发送失败记入日志
            if (result.Status != Extra.SMS.SmsResultStatus.Success)
            {
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, string.Format("更新会员{0}密码短信发送失败", model.Account), LogStatus.系统日志目标类型.客户管理, CurrentUser.Base.SysNo, null);
            }
            //修改为发短信成功不影响修改密码 余勇 2014-07-15
            //if (result.Status == Extra.SMS.SmsResultStatus.Success)
            //{
            if (BLL.CRM.CrCustomerBo.Instance.Update(model) > 0)
            {
                bool r;
                try
                {
                    //调用SSO服务修改客户密码 (余勇 2014-06-27)
                    r = CrCustomerBo.Instance.UpdateSSOPassword(model.SysNo, oldPass, newPass); //取消加密 余勇 2014-09-12
                }
                catch (Exception ex)
                {
                    return Json(new { result = false, info = ex.Message });
                }
                var msg = r ? "重置密码成功！" : "重置密码失败！";
                return Json(new { result = r, info = msg });
            }
            else
            {
                return Json(new { result = false, info = "重置密码失败！" });
            }

            //}
            //else
            //{
            //    return Json(new { result = true, info = "发送短信失败！" });
            //}

        }

        /// <summary>
        /// 客户修改视图
        /// </summary>
        /// <param name="sysNo">会员编号</param>
        /// <returns>空视图</returns>
        /// <remarks>2013-07-25 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.CR1004201)]
        public ActionResult CrCustomerEdit(int sysNo)
        {
            IDictionary<int, string> dictIsReceiveEmail = EnumUtil.ToDictionary(typeof(CustomerStatus.是否接收邮件));
            ViewBag.dictIsReceiveEmail = dictIsReceiveEmail;

            IDictionary<int, string> dictIsReceiveShortMessage = EnumUtil.ToDictionary(typeof(CustomerStatus.是否接收短信));
            ViewBag.dictIsReceiveShortMessage = dictIsReceiveShortMessage;

            IDictionary<int, string> dictIsPublicAccount = EnumUtil.ToDictionary(typeof(CustomerStatus.是否是公共账户));
            ViewBag.dictIsPublicAccount = dictIsPublicAccount;

            IDictionary<int, string> dictIsLevelFixed = EnumUtil.ToDictionary(typeof(CustomerStatus.等级是否固定));
            ViewBag.dictIsLevelFixed = dictIsLevelFixed;

            IDictionary<int, string> dictIsExperiencePointFixed = EnumUtil.ToDictionary(typeof(CustomerStatus.经验积分是否固定));
            ViewBag.dictIsExperiencePointFixed = dictIsExperiencePointFixed;

            IDictionary<int, string> dictIsExperienceCoinFixed = EnumUtil.ToDictionary(typeof(CustomerStatus.惠源币是否固定));
            ViewBag.dictIsExperienceCoinFixed = dictIsExperienceCoinFixed;



            var model = BLL.CRM.CrCustomerBo.Instance.GetCrCustomerItem(sysNo);
            //客户生日
            var dateTimeNull = DateTime.Parse("1753-1-1 12:00:00");
            if (model.Birthday != null && model.Birthday != DateTime.MinValue && model.Birthday != dateTimeNull)
            {
                ViewBag.Year = model.Birthday.Year;
                ViewBag.Month = model.Birthday.Month;
                ViewBag.Day = model.Birthday.Day;
            }
            ViewBag.Today = DateTime.Now.ToString("yyyy-M-d");
            return View(model);
        }

        /// <summary>
        /// 客户修改
        /// </summary>
        /// <param name="sysNo">会员编号</param>
        /// <param name="customer">会员实体</param>
        /// <returns>更新失败 成功</returns>
        /// <remarks>2013-07-25 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.CR1004201)]
        public ActionResult CrCustomerUpdate(int sysNo, CrCustomer customer)
        {

            var model = BLL.CRM.CrCustomerBo.Instance.GetCrCustomerItem(sysNo);
            model.IsReceiveEmail = customer.IsReceiveEmail;
            model.IsReceiveShortMessage = customer.IsReceiveShortMessage;
            model.IsPublicAccount = customer.IsPublicAccount;
            model.IsLevelFixed = customer.IsLevelFixed;
            model.IsExperiencePointFixed = customer.IsExperiencePointFixed;
            model.IsExperienceCoinFixed = customer.IsExperienceCoinFixed;
            if (customer.Birthday != null)
            {
                model.Birthday = customer.Birthday;
            }
            if (BLL.CRM.CrCustomerBo.Instance.Update(model) > 0)
            {
                return Json(new { result = true, info = "修改成功！" });
            }
            else
            {
                return Json(new { result = false, info = "修改失败！" });
            }
        }
        #endregion

        #region 等级
        /// <summary>
        /// 等级日志
        /// </summary>
        /// <param name="id">分页索引</param>
        /// <param name="sysNo">惠源币日志系统号</param>
        /// <param name="changeType">惠源币日志系统号</param>
        /// <returns>等级日志视图</returns>
        /// <remarks>2013-07-12 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.CR1004002)]
        public ActionResult CrLevelLog(int? id, int sysNo, int? changeType)
        {
            IDictionary<int, string> dictType = EnumUtil.ToDictionary(typeof(CustomerStatus.等级日志变更类型));
            ViewBag.dictType = dictType;

            ViewBag.sysNo = sysNo;

            var model = PointBo.Instance.GetLevelLog(sysNo, changeType, id);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_AjaxCrLevelLog", model);
            }
            return View(model);
        }

        /// <summary>
        /// 等级日志详情
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>等级变更详情页面</returns>
        /// <remarks>2013-07-12 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.CR1004002)]
        public ActionResult CrLevelLogDetail(int sysNo)
        {
            var model = PointBo.Instance.GetLevelLogModel(sysNo);
            return View(model);
        }
        #endregion

        #region 等级积分
        /// <summary>
        /// 等级积分日志
        /// </summary>
        /// <param name="id">分页索引</param>
        /// <param name="customerSysNo">惠源币日志系统号</param>
        /// <param name="changeType">惠源币日志系统号</param>
        /// <returns>等级积分日志详情页面</returns>
        /// <remarks>2013-07-12 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.CR1004003, PrivilegeCode.CR1004203)]
        public ActionResult CrLevelPointLog(int? id, int customerSysNo, int? changeType)
        {
            IDictionary<int, string> dictType = EnumUtil.ToDictionary(typeof(CustomerStatus.等级积分日志变更类型));
            ViewBag.dictType = dictType;

            ViewBag.customerSysNo = customerSysNo;

            var model = PointBo.Instance.GetLevelPointLog(customerSysNo, changeType, id);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_AjaxCrLevelPointLog", model);
            }
            return View(model);
        }

        /// <summary>
        /// 调整客户等级积分
        /// </summary>
        /// <returns>调整客户等级积分成功或失败信息</returns>
        /// <remarks>2013-07-15 苟治国 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.CR1004203)]
        public ActionResult CrLevelPointLogOperate()
        {
            int reased;
            string hidSysNo = Request.Form["hidSysNo"];
            int changeType = int.Parse(Request.Form["changeType"]);
            int quantity = int.Parse(Request.Form["quantity"]);
            string pointDescription = Request.Form["pointDescription"];

            if (changeType == 1)
            {
                reased = Math.Abs(quantity);
            }
            else
            {
                reased = -Math.Abs(quantity);
            }
            PointBo.Instance.AdjustLevelPoint(int.Parse(hidSysNo), AdminAuthenticationBo.Instance.GetAuthenticatedUser().SysNo, reased, pointDescription);

            return Json(new { result = true, info = "调整成功" });
        }

        /// <summary>
        /// 调整会员等级积分
        /// </summary>
        /// <param name="customerSysNo">会员编号</param>
        /// <returns>会员等级积分详情页面</returns>
        /// <remarks>2013-07-12 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.CR1004003, PrivilegeCode.CR1004203)]
        public ActionResult CrLevelPointLogOperate(int customerSysNo)
        {
            IDictionary<int, string> dictChangeType = EnumUtil.ToDictionary(typeof(CustomerStatus.等级积分日志变更类型));
            ViewBag.dictChangeType = dictChangeType;
            var model = CrCustomerBo.Instance.GetCrCustomerItem(customerSysNo);
            return View(model);
        }

        /// <summary>
        /// 会员等级积分视图
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>会员等级积分视图</returns>
        /// <remarks>2013-07-12 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.CR1004003, PrivilegeCode.CR1004203)]
        public ActionResult CrLevelPointLogDetail(int sysNo)
        {
            var model = PointBo.Instance.GetGetLevelPointLogModel(sysNo);
            return View(model);
        }
        #endregion

        #region 经验积分
        /// <summary>
        /// 会员经验积分日志
        /// </summary>
        /// <param name="id">分页索引</param>
        /// <param name="customerSysNo">会员编号</param>
        /// <param name="pointType">积分变更类型</param>
        /// <returns>会员经验积分日详情页面</returns>
        /// <remarks>2013-07-12 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.CR1004004, PrivilegeCode.CR1004204)]
        public ActionResult CrExperiencePointLog(int? id, int customerSysNo, int? pointType)
        {
            IDictionary<int, string> dictType = EnumUtil.ToDictionary(typeof(CustomerStatus.经验积分变更类型));
            ViewBag.dictType = dictType;

            ViewBag.customerSysNo = customerSysNo;

            var model = PointBo.Instance.GetExperiencePointLog(customerSysNo, pointType, id);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_AjaxCrExperiencePointLog", model);
            }
            return View(model);
        }

        /// <summary>
        /// 修改会员经验积分日志
        /// </summary>
        /// <param name="customerSysNo">会员编号</param>
        /// <returns>调整客户等级积分成功或失败信息</returns>
        /// <remarks>2013-07-12 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.CR1004204)]
        public ActionResult CrExperiencePointLogOperate(int customerSysNo)
        {

            ViewBag.customerSysNo = customerSysNo;
            var model = CrCustomerBo.Instance.GetCrCustomerItem(customerSysNo);
            return View(model);
        }

        /// <summary>
        /// 调整经验积分
        /// </summary>
        /// <returns>调整经验积分成功或失败信息</returns>
        /// <remarks>2013-07-15 苟治国 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.CR1004204)]
        public ActionResult CrExperiencePointLogOperate()
        {
            int reased;
            string hidSysNo = Request.Form["hidSysNo"];
            int pointType = int.Parse(Request.Form["pointType"]);
            int quantity = int.Parse(Request.Form["quantity"]);
            string pointDescription = Request.Form["pointDescription"];

            if (pointType == 1)
            {
                reased = Math.Abs(quantity);
            }
            else
            {
                reased = -Math.Abs(quantity);
            }
            PointBo.Instance.AdjustExperiencePoint(int.Parse(hidSysNo), AdminAuthenticationBo.Instance.GetAuthenticatedUser().SysNo, reased, pointDescription);

            return Json(new { result = true, info = "调整成功" });
        }

        /// <summary>
        /// 客户经验积分变更详情
        /// </summary>
        /// <param name="sysNo">客户经验积分系统编号</param>
        /// <returns>客户经验积分变更视图</returns>
        /// <remarks>2013-07-16 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.CR1004004, PrivilegeCode.CR1004204)]
        public ActionResult CrExperiencePointLogDetail(int sysNo)
        {
            var model = PointBo.Instance.GetCrExperiencePointLogModel(sysNo);
            return View(model);
        }
        #endregion

        #region 惠源币
        /// <summary>
        /// 惠源币日志
        /// </summary>
        /// <param name="id">惠源币日志系统号</param>
        /// <param name="customerSysNo">惠源币日志系统号</param>
        /// <param name="changeType">惠源币日志系统号</param>
        /// <returns>惠源币日志详情页面</returns>
        /// <remarks>2013-07-12 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.CR1004005, PrivilegeCode.CR1004205)]
        public ActionResult CrExperienceCoinLog(int? id, int customerSysNo, int? changeType)
        {

            IDictionary<int, string> dictList = EnumUtil.ToDictionary(typeof(CustomerStatus.惠源币变更类型));
            ViewBag.DictList = dictList;
            ViewBag.CustomerSysNo = customerSysNo;
            var model = PointBo.Instance.GetExperienceCoinLog(customerSysNo, changeType, id);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_AjaxCrExperienceCoinLog", model);
            }
            return View(model);
        }

        /// <summary>
        /// 调整客户惠源币视图
        /// </summary>
        /// <param name="customerSysNo">客户系统编号</param>
        /// <returns>调整客户惠源币视图</returns>
        /// <remarks>2013-07-15 杨晗 创建</remarks>
        //[Privilege(PrivilegeCode.CR1004005, PrivilegeCode.CR1004205)]
        //public ActionResult CrExperienceCoinLogOperate(int customerSysNo)
        //{
        //    var model = CrCustomerBo.Instance.GetModel(customerSysNo);

        //    return View(model);
        //}

        /// <summary>
        /// 调整客户惠源币
        /// </summary>
        /// <returns>调整客户惠源币成功或失败信息</returns>
        /// <remarks>2013-07-15 杨晗 创建</remarks>
        //[HttpPost]
        //[Privilege(PrivilegeCode.CR1004205)]
        //public ActionResult CrExperienceCoinLogOperate()
        //{
        //    int reased;
        //    string hidSysNo = Request.Form["hidSysNo"];
        //    int selChangeType = int.Parse(Request.Form["selChangeType"]);
        //    int txtCreased = int.Parse(Request.Form["txtCreased"]);
        //    string txtChangeDescription = Request.Form["txtChangeDescription"];
        //    if (selChangeType == 0)
        //    {
        //        reased = Math.Abs(txtCreased);
        //    }
        //    else
        //    {
        //        reased = -Math.Abs(txtCreased);
        //    }
        //    PointBo.Instance.AdjustExperienceCoin(int.Parse(hidSysNo),
        //                                             AdminAuthenticationBo.Instance.GetAuthenticatedUser().SysNo,
        //                                              reased, txtChangeDescription);

        //    return Json(new {IsPass = true});
        //}

        /// <summary>
        /// 客户惠源币变更详情
        /// </summary>
        /// <param name="sysNo">惠源币日志系统号</param>
        /// <returns>惠源币变更详情视图</returns>
        /// <remarks>2013-07-15 杨晗 创建</remarks>
        [Privilege(PrivilegeCode.CR1004005, PrivilegeCode.CR1004205)]
        public ActionResult CrExperienceCoinLogDetail(int sysNo)
        {
            var model = PointBo.Instance.GetCbCrExperienceCoinLog(sysNo);
            return View(model);
        }
        #endregion

        #endregion

        #region 历史订单

        /// <summary>
        /// 查看客户历史订单
        /// </summary>
        /// <param name="id">分页索引</param>
        /// <param name="orderNo">订单号</param>
        /// <param name="customerAccount">会员账号</param>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <returns>客户历史订单列表</returns>
        /// <remarks>2013－07-18 苟治国 创建</remarks>
        [Privilege(PrivilegeCode.CR1004102)]
        public ActionResult CrOrder(int? id, int? orderNo, string customerAccount, string beginDate = null, string endDate = null)
        {
            id = id ?? 1;
            ViewBag.customerAccount = customerAccount;

            var pager = new Pager<Hyt.Model.CBSoOrder> { CurrentPage = (int)id };
            var filter = new ParaOrderFilter { CustomerAccount = customerAccount };
            if (orderNo != null)
            {
                filter.OrderSysNo = orderNo;
            }
            if (beginDate != null && endDate != null)
            {
                filter.BeginDate = Convert.ToDateTime(beginDate);
                filter.EndDate = Convert.ToDateTime(endDate);
            }
            filter.HasAllWarehouse = true;
            filter.IsBindAllDealer = true;
            filter.SelectedAgentSysNo = -1;
            BLL.Order.SoOrderBo.Instance.DoSoOrderQuery(ref pager, filter);

            var list = new PagedList<CBSoOrder>
            {
                PageSize = 10,
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };
            if (Request.IsAjaxRequest())
            {
                return PartialView("_AjaxCrOrder", list);
            }
            return View(list);
        }
        #endregion

        #region 意见反馈管理
        /// <summary>
        /// 获取意见反馈管理页面
        /// </summary>
        /// <param name="id">页索引</param>
        /// <returns>意见反馈管理页面</returns>
        /// <remarks>2013-09-03 沈强 创建</remarks>
        [Privilege(PrivilegeCode.CR1004201)]
        public ActionResult FeedbackManage(int? id)
        {
            if (!Request.IsAjaxRequest())
            {
                var names = BLL.Feedback.FeedbackTypeBo.Instance.GetFeedbackTypeNameList();
                return View(names.ToList());
            }
            int pageIndex = id ?? 1;
            //意见类型
            string suggestType = Request.Params["suggestType"];
            //客户帐号
            string account = Request.Params["account"];
            //来源
            string soucre = Request.Params["soucre"];

            #region 为查询条件实体赋值
            CBCrFeedback cbCrFeedback = new CBCrFeedback();
            cbCrFeedback.Source = string.IsNullOrEmpty(soucre) ? 0 : int.Parse(soucre);
            cbCrFeedback.SuggestType = suggestType == "0" ? null : suggestType;
            cbCrFeedback.Account = (account == "请输入客户帐号" || account == "") ? null : account;
            #endregion

            PagedList pageList = new PagedList();

            Pager<CBCrFeedback> pager = new Pager<CBCrFeedback>();
            pager.PageFilter = cbCrFeedback;
            pager.CurrentPage = pageIndex;
            pager.PageSize = 2;

            BLL.Feedback.FeedbackBo.Instance.GetFeedbacks(ref pager);

            pageList.CurrentPageIndex = pager.CurrentPage;
            pageList.PageSize = pager.PageSize;
            pageList.TotalItemCount = pager.TotalRows;
            pageList.Data = pager.Rows;

            return PartialView("_FeedbackList", pageList);
        }

        #endregion

        #region 客户通话记录
        /// <summary>
        /// 客户通话记录
        /// </summary>
        /// <param name="id">会员咨询编号</param>
        /// <returns>空视图</returns>
        /// <remarks>20134-05-04 余勇 创建</remarks>
        [Privilege(PrivilegeCode.CR1002002)]
        public ActionResult CrCustomerCallRecord(int id)
        {
            var model = BLL.CRM.CrCustomerBo.Instance.GetCrCustomerItem(id);
            return View(model);
        }

        /// <summary>
        /// 获取通迅记录列表
        /// </summary>
        /// <param name="id">页索引</param>
        /// <returns>通迅记录列表</returns>
        /// <remarks>20134-05-04 余勇 创建</remarks>
        [Privilege(PrivilegeCode.CR1002002)]
        public ActionResult CrCustomerCallRecordQuery(int? id)
        {
            int pageIndex = id ?? 1;
            //客户编号
            string customerSysNo = Request.Params["sysNo"];

            #region 为查询条件实体赋值
            CBCrFeedback cbCrFeedback = new CBCrFeedback();
            cbCrFeedback.Source = (int)CustomerStatus.意见反馈类型来源.Crm;
            cbCrFeedback.CustomerSysNo = int.Parse(customerSysNo);
            #endregion

            PagedList pageList = new PagedList();
            Pager<CBCrFeedback> pager = new Pager<CBCrFeedback>();
            pager.PageFilter = cbCrFeedback;
            pager.CurrentPage = pageIndex;
            pager.PageSize = 10;

            BLL.Feedback.FeedbackBo.Instance.GetFeedbacks(ref pager);

            pageList.CurrentPageIndex = pager.CurrentPage;
            pageList.PageSize = pager.PageSize;
            pageList.TotalItemCount = pager.TotalRows;
            pageList.Data = pager.Rows;

            return PartialView("_AjaxCrCustomerCallRecord", pageList);
        }

        /// <summary>
        /// 保存通话记录
        /// </summary>
        /// <returns>空视图</returns>
        /// <remarks>20134-05-04 余勇 创建</remarks>
        [Privilege(PrivilegeCode.CR1002002)]
        public ActionResult CrCustomerCallRecordSave()
        {
            bool result = false;
            string info = "操作失败";

            string sysNo = Request.Form["SysNo"];
            string content = Request.Form["Content"];

            if (!string.IsNullOrEmpty(sysNo))
            {
                var model = BLL.CRM.CrCustomerBo.Instance.GetCrCustomerItem(int.Parse(sysNo));

                var res = FeedbackBo.Instance.Create(new CrFeedback
                {
                    Content = content,
                    CreatedBy = CurrentUser.Base.SysNo,
                    CreatedDate = DateTime.Now,
                    CustomerSysNo = model.SysNo,
                    Email = model.EmailAddress,
                    FeedbackTypeSysNo = 1,
                    Name = model.Name,
                    Phone = model.MobilePhoneNumber,
                    Source = (int)CustomerStatus.意见反馈类型来源.Crm,
                });
                if (res > 0)
                {
                    result = true;
                    info = "保存成功！";
                }
                else
                {
                    result = false;
                    info = "保存失败！";
                }
            }

            return Json(new { result = result, info = info });
        }

        #endregion

        #region 客户短信咨询

        /// <summary>
        /// 客户咨询管理
        /// </summary>
        /// <param name="id">分页页码</param>
        /// <param name="status">过滤状态</param>
        /// <param name="answer">回复人</param>
        /// <param name="stringParameter">过滤电话号码或内容</param>
        /// <returns>返回视图</returns>
        /// <remarks>2014-02-24 邵斌 创建</remarks>
        [Privilege(PrivilegeCode.CR1004091)]
        public ActionResult CrSmsQuestion(int? id, int? status, string answer, string stringParameter = null)
        {
            ViewBag.CanReply = true;

            if (id.HasValue)
            {
                id = id ?? 1;
                PagedList<CBCrSmsQuestion> pager = new PagedList<CBCrSmsQuestion>();
                pager.CurrentPageIndex = id.Value;

                if (!string.IsNullOrWhiteSpace(stringParameter))
                {
                    stringParameter = stringParameter.Trim();
                }
                var list = BLL.CRM.CrSmsQuestionBo.Instance.List(status, stringParameter, answer, stringParameter, pager);
                return View("_AjaxCrSmsQuestion_Pager", pager);
            }
            return View();
        }

        /// <summary>
        /// 客户咨询管理（任务调整）
        /// </summary>
        /// <param name="sysNo">短信咨询系统编号</param>
        /// <param name="jobPoolSysNo">任务系统编号</param>
        /// <param name="taskType">任务类型</param>
        /// <returns>返回视图</returns>
        /// <remarks>2014-02-24 邵斌 创建</remarks>
        [Privilege(PrivilegeCode.CR1004091)]
        public ActionResult ShowCrSmsQuestionTaskJob(int sysNo, int jobPoolSysNo, int taskType)
        {
            ViewBag.OpenDetial = sysNo;
            var question = CrSmsQuestionBo.Instance.Get(sysNo);
            if (question != null && question.Status != (int)CustomerStatus.短信咨询状态.作废 && question.Status != (int)CustomerStatus.短信咨询状态.已回复)
            {
                ViewBag.CanReply = true;
            }

            BLL.Sys.SyJobPoolManageBo.Instance.DeleteJobPool(jobPoolSysNo);

            return View("CrSmsQuestion");
        }

        /// <summary>
        /// 客户短信咨询详情
        /// </summary>
        /// <param name="sysNo">短信系统编号</param>
        /// <param name="isView">只是查看</param>
        /// <returns>返回视图</returns>
        /// <remarks>2014-02-24 邵斌 创建</remarks>
        [Privilege(PrivilegeCode.CR1004091)]
        public ActionResult CrSmsQuestionDetails(int sysNo, bool isView = false)
        {
            var model = CrSmsQuestionBo.Instance.GetSmsQuestionListBySysNo(sysNo);
            ViewBag.IsView = isView;
            return View(model);
        }

        #endregion

        #region 客户短信发送
        [Privilege(PrivilegeCode.CR1004094)]
        public ActionResult CrSmsSendCreate()
        {
            return View();
        }

        /// <summary>
        /// 发送短信信息
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 2015-09-12 王耀发 创建
        /// 2016-06-13 陈海裕 修改 调用wcf服务
        /// </remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.CR1004094)]
        public ActionResult CrSmsSend()
        {
            string mobilelist = this.Request["mobilelist"];
            string Msg = this.Request["Msg"];
            //Hyt.BLL.Extras.SmsBO obj = new Hyt.BLL.Extras.SmsBO();
            //obj.SendMsg(mobilelist, Msg, DateTime.Now);

            string[] phoneNum = mobilelist.Split(';');
            foreach (string ph in phoneNum)
            {
                BLL.Web.CrCustomerBo.Instance.Send(ph, Msg, BLL.Stores.StoresBo.Instance.GetStoreById(0).ErpName);
            }

            Result result = new Result();
            result.Status = true;
            result.Message = "发送成功！";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        /// <summary>
        /// 查询所有的用户
        /// </summary>     
        /// <returns>用户信息列表</returns>
        /// <remarks> 
        /// 2015-09-19 王耀发 创建
        [Privilege(PrivilegeCode.CR1004101)]
        public JsonResult GetCrCustomerZTreeList()
        {

            var list = CrCustomerBo.Instance.GetCrCustomerList();
            //通过Linq生产zTree节点结果集
            var nodes = from c in list
                        select new
                        {
                            id = c.SysNo
                            ,
                            name = string.IsNullOrWhiteSpace(c.Name) ? c.Account : c.Name
                            ,
                            title = string.IsNullOrWhiteSpace(c.Name) ? c.Account : c.Name
                            ,
                            open = false
                            ,
                            pId = c.PSysNo
                            ,
                            status = c.Status,

                        };

            //返回json数组
            return Json(nodes.ToList(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获得客户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns>2015-09-19 王耀发 创建</returns>
        [Privilege(PrivilegeCode.CR1004101, PrivilegeCode.DS1003004)]
        public JsonResult GetModel()
        {
            int SysNo = int.Parse(this.Request["CrCustomerSysNo"]);
            CBCrCustomer model = Hyt.BLL.CRM.CrCustomerBo.Instance.GetModel(SysNo);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 会员充值记录
        /// </summary>
        [Privilege(PrivilegeCode.CR1004084)]
        public ActionResult CrRecharge(ParaCrRechargeFilter filter)
        {
            int pageindex = filter.id ?? 1;
            int pagesize = filter.PageSize ?? 15;

            var pager = new Pager<CBCrRecharge>
            {
                CurrentPage = pageindex,
                PageSize = pagesize
            };

            if (Request.IsAjaxRequest())
            {
                BLL.Balance.CrRechargeBo.Instance.Search(ref pager, filter);
                return PartialView("_AjaxCrRechargePager", pager.Map());
            }

            //传递分销商等级
            //ViewBag.PaymentType = BLL.
            //Model.CommonEnum.PayCode.支付宝
            return View();
        }
    }
}