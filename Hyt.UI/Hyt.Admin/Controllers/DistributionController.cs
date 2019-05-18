using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Transactions;
using System.Web.Mvc;
using Hyt.BLL.Authentication;
using Hyt.BLL.Basic;
using Hyt.BLL.Distribution;
using Hyt.BLL.Log;
using Hyt.BLL.MallSeller;
using Hyt.BLL.Product;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.SmartConfig;
using Hyt.Model.SystemPredefined;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Util;
using Newtonsoft.Json;
using Hyt.BLL.CRM;
using Hyt.BLL.SellBusiness;
using Hyt.BLL.Finance;
using Hyt.BLL.Weixin;
using Hyt.BLL.Cache;
using Hyt.BLL.Sys;
using Hyt.Util.Validator;
using Hyt.Util.Validator.Rule;
using Hyt.BLL.Warehouse;
using Hyt.Util.Serialization;
using Hyt.Admin.Models;
using Newtonsoft.Json.Linq;
using LitJson;
using System.Collections;
using Hyt.Model.Generated;
using ThoughtWorks.QRCode.Codec;
using Extra.UpGrade.UpGrades;
using Extra.UpGrade.Model;
using Extra.UpGrade.SDK.JingDong;
using Extra.UpGrade.Api;
using Extra.UpGrade.Provider;
using Hyt.DataAccess.Oracle.Distribution;
namespace Hyt.Admin.Controllers
{
    /// <summary>
    /// 分销商产品特殊价格维护控制器
    /// </summary>
    /// <remarks>2013-09-04 周瑜 创建</remarks>
    public class DistributionController : BaseController
    {
        #region 分销商资金维护 2013-09-03 周唐炬 创建
        /// <summary>
        /// 分销商充值
        /// </summary>
        /// <param name="id">分销商系统编号</param>
        /// <returns>视图页</returns>
        /// <remarks>2013-09-10 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.DS1008301)]
        public ActionResult Prepaid(int id)
        {
            var model = DsDealerBo.Instance.GetDsDealer(id);
            return View(model);
        }

        /// <summary>
        /// 分销商充值
        /// </summary>
        /// <param name="id">分销商系统编号</param>
        /// <param name="amount">金额</param>
        /// <param name="remarks">备注</param>
        /// <returns>视图页</returns>
        /// <remarks>2013-09-10 周唐炬 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.DS1008301)]
        public ActionResult Prepaid(int id, decimal amount, string remarks)
        {
            var result = new Result() { StatusCode = -1 };
            try
            {
                if (amount > Decimal.Zero && amount <= 100000000)
                {

                    DsDealerBo.Instance.Prepaid(id, amount, CurrentUser.Base, remarks);
                    result.Status = true;
                    result.StatusCode = 0;
                    SysLog.Instance.Info(LogStatus.系统日志来源.后台, "分销商充值金额" + amount, LogStatus.系统日志目标类型.分销商预存款, id, CurrentUser.Base.SysNo);

                }
                else
                {
                    result.Message = "充值金额必须大于0小于1亿!";
                }

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "分销商充值" + ex.Message, LogStatus.系统日志目标类型.用户, CurrentUser.Base.SysNo, ex);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 分销商提现
        /// </summary>
        /// <param name="id">分销商系统编号</param>
        /// <returns>视图页</returns>
        /// <remarks>2013-09-10 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.DS1008302)]
        public ActionResult Withdraw(int id)
        {
            ViewBag.PrePaymentSysNo = id;
            DsPrePayment PayModel = DsPrePaymentBo.Instance.GetEntity(id);
            ViewBag.DealerSysNo = PayModel.DealerSysNo;
            var model = DsDealerBo.Instance.GetDsDealer(PayModel.DealerSysNo);
            return View(model);
        }

        /// <summary>
        /// 分销商提现
        /// </summary>
        /// <param name="id">分销商系统编号</param>
        /// <param name="sysno">支付单系统编号</param>
        /// <returns>视图页</returns>
        /// <remarks>2016-01-7 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.DS1008302)]
        public ActionResult WithdrawView(int id, int sysno)
        {
            var model = DsDealerBo.Instance.GetDsDealer(id);
            var model2 = FinanceBo.Instance.GetPaymentVoucher(sysno);
            model.PayableAmount = model2.PayableAmount;
            model.RefundBank = model2.RefundBank;
            model.RefundAccountName = model2.RefundAccountName;
            model.RefundAccount = model2.RefundAccount;
            model.PaymentType = model2.PaymentType;
            return View(model);
        }

        /// <summary>
        /// 分销商提现
        /// </summary>
        /// <param name="id">分销商系统编号</param>
        /// <param name="amount">金额</param>
        /// <param name="remarks">备注</param>
        /// <returns>视图页</returns>
        /// <remarks>2013-09-10 周唐炬 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.DS1008302)]
        public ActionResult Withdraw(int id, int dealersysno, decimal amount, string remarks, int PayType, string CashAccount, string CashName, string CashBank)
        {
            var result = new Result() { StatusCode = -1 };
            try
            {
                using (var tran = new TransactionScope())
                {
                    int ItemSysNo = DsDealerBo.Instance.Withdraw(dealersysno, amount, CurrentUser.Base, remarks);
                    //对应金额加入到冻结返利字段中 王耀发 2016-1-6 创建
                    DsPrePaymentBo.Instance.UpdatePaymentValueConfirm(id, -1 * amount);
                    //插入记录到财务管理表中 王耀发 2016-1-6 创建
                    FnPaymentVoucher pvEntity = new FnPaymentVoucher();
                    pvEntity.TransactionSysNo = id.ToString();
                    pvEntity.Source = (int)FinanceStatus.付款来源类型.分销商提现单;
                    pvEntity.SourceSysNo = ItemSysNo;
                    pvEntity.PayableAmount = amount;
                    pvEntity.PaidAmount = 0;
                    pvEntity.CustomerSysNo = dealersysno;
                    pvEntity.RefundBank = CashBank;
                    pvEntity.RefundAccountName = CashName;
                    pvEntity.RefundAccount = CashAccount;
                    pvEntity.Status = (int)FinanceStatus.付款单状态.待付款;
                    pvEntity.CreatedDate = DateTime.Now;
                    pvEntity.CreatedBy = CurrentUser.Base.SysNo;
                    pvEntity.PayDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                    pvEntity.LastUpdateBy = CurrentUser.Base.SysNo;
                    pvEntity.LastUpdateDate = DateTime.Now;
                    pvEntity.PaymentType = PayType;
                    int Voucher = FinanceBo.Instance.InsertPaymentVoucher(pvEntity);
                    tran.Complete();
                }
                result.Status = true;
                result.StatusCode = 0;
                SysLog.Instance.Info(LogStatus.系统日志来源.后台, "分销商提现金额" + amount, LogStatus.系统日志目标类型.分销商预存款, id, CurrentUser.Base.SysNo);

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "分销商提现" + ex.Message, LogStatus.系统日志目标类型.用户, CurrentUser.Base.SysNo, ex);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 分销商预存款往来账明细
        /// </summary>
        /// <param name="id">分销商系统编号</param>
        /// <param name="filter">获取分销商过滤条件</param>
        /// <returns>分销商预存款往来账明细列表</returns>
        ///  <remarks>2013-09-10 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.DS1008101)]
        public ActionResult HistoricalDetail(int? id, ParaDealerFilter filter)
        {
            try
            {
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
                if (Request.IsAjaxRequest())
                {
                    filter.CurrentPage = id ?? 1;
                    var list = DsPrePaymentItemBo.Instance.GetDsPrePaymentItemList(filter);
                    return PartialView("_AjaxPagerHistoricalItemList", list);
                }
                if (filter != null && filter.SysNo.HasValue)
                {
                    ViewBag.SysNo = filter.SysNo;
                }
                InitPageViewData(true);
            }
            catch (Exception ex)
            {
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "分销商预存款往来账明细" + ex.Message, LogStatus.系统日志目标类型.用户, CurrentUser.Base.SysNo, ex);
            }
            return View();
        }

        /// <summary>
        /// 分销商预存款往来账明细
        /// </summary>
        /// <param name="id">分销商系统编号</param>
        /// <param name="filter">获取分销商过滤条件</param>
        /// <returns>分销商预存款往来账明细列表</returns>
        ///  <remarks>2016-05-13 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.DS1008101)]
        public ActionResult DealerHistoricalDetail(int? id, int? dealersysno, ParaDealerFilter filter)
        {
            try
            {
                //当前用户自己对应分销商
                if (Request.IsAjaxRequest())
                {
                    filter.CurrentPage = id ?? 1;
                    var list = DsPrePaymentItemBo.Instance.GetDsPrePaymentItemList(filter);
                    return PartialView("_AjaxPagerHistoricalItemList", list);
                }
                if (filter != null && filter.SysNo.HasValue)
                {
                    ViewBag.SysNo = filter.SysNo;
                }
                InitPageViewData(true);
            }
            catch (Exception ex)
            {
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "分销商预存款往来账明细" + ex.Message, LogStatus.系统日志目标类型.用户, CurrentUser.Base.SysNo, ex);
            }
            ViewBag.DealerSysNo = dealersysno;
            return View();
        }

        /// <summary>
        /// 查看
        /// </summary>
        /// <param name="id"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        /// <remarks>2016-1-9 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.DS1008101)]
        public ActionResult DoHistoricalDetailViewQuery(int? id, ParaDealerFilter filter)
        {
            try
            {
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
                filter.SelectedDealerSysNo = -1;
                if (Request.IsAjaxRequest())
                {
                    filter.CurrentPage = id ?? 1;
                    var list = DsPrePaymentItemBo.Instance.GetDsPrePaymentItemList(filter);
                    return PartialView("_AjaxPagerHistoricalItemView", list);
                }
                if (filter != null && filter.SysNo.HasValue)
                {
                    ViewBag.SysNo = filter.SysNo;
                }
                InitPageViewData(true);
            }
            catch (Exception ex)
            {
                SysLog.Instance.Error(LogStatus.系统日志来源.后台, "分销商预存款往来账明细" + ex.Message, LogStatus.系统日志目标类型.用户, CurrentUser.Base.SysNo, ex);
            }
            return View();
        }

        /// <summary>
        /// 分销商列表
        /// </summary>
        /// <param name="id">页码</param>
        /// <param name="filter">获取分销商过滤条件</param>
        /// <returns>分销商列表</returns>
        /// <remarks>2013-09-03 周唐炬 创建</remarks>
        [Privilege(PrivilegeCode.DS1008101)]
        public ActionResult DealerList(int? id, ParaDealerFilter filter)
        {
            if (Request.IsAjaxRequest())
            {
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
                filter.CurrentPage = id ?? 1;
                var list = DsDealerBo.Instance.GetDealerList(filter);
                return PartialView("_AjaxPagerDealerList", list);
            }
            InitPageViewData(false);
            return View();
        }

        /// <summary>
        /// 初始化页面下拉
        /// </summary>
        /// <param name="isItemStatus">是否明细</param>
        /// <returns></returns>
        /// <remarks>2013-09-03 周唐炬 创建</remarks>
        private void InitPageViewData(bool isItemStatus)
        {
            var item = new SelectListItem() { Text = @"全部", Value = "", Selected = true };
            var statustList = new List<SelectListItem>() { item };
            if (isItemStatus)
            {
                EnumUtil.ToListItem<DistributionStatus.预存款明细状态>(ref statustList);
                var sourceList = new List<SelectListItem>() { item };
                EnumUtil.ToListItem<DistributionStatus.预存款明细来源>(ref sourceList);
                ViewBag.Source = new SelectList(sourceList, "Value", "Text");
            }
            else
            {
                EnumUtil.ToListItem<DistributionStatus.分销商状态>(ref statustList);
            }
            ViewBag.Status = new SelectList(statustList, "Value", "Text");
        }
        #endregion

        #region 分销商维护 2013-09-04 郑荣华 创建

        /// <summary>
        /// 分销商维护主页面
        /// </summary>
        /// <returns>分销商维护主页面</returns>
        /// <param name="id">页码</param>
        /// <param name="filter">分销商查询筛选字段</param>
        /// <remarks> 
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.DS1002101)]
        public ActionResult DsDealer(int? id, ParaDsDealerFilter filter)
        {
            if (Request.IsAjaxRequest())
            {
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
                //当前用户对应分销商，2015-12-19 王耀发 创建
                //var DealerSysNoList = CurrentUser.Dealers;
                //if (DealerSysNoList.Count > 0)
                //{
                //    filter.DealerSysNoList = new List<int>();
                //    DealerSysNoList.ForEach(x => filter.DealerSysNoList.Add(x.SysNo));
                //}
                //列表分页开始
                var model = new PagedList<CBDsDealer>();

                var modelRef = new Pager<CBDsDealer> { CurrentPage = id ?? 1, PageSize = model.PageSize };
                DsDealerBo.Instance.GetDsDealerList(ref modelRef, filter);
                model.TotalItemCount = modelRef.TotalRows;
                model.TData = modelRef.Rows;
                model.CurrentPageIndex = modelRef.CurrentPage;
                return PartialView("_AjaxPagerDsDealer", model);
            }
            //传递分销商等级
            ViewBag.DsDealerLevel = DsDealerLevelBo.Instance.GetDsDealerLevelList();
            return View();
        }
        /// <summary>
        /// 获取代理商列表
        /// 2016-1-29 王耀发 创建
        /// 2017-5-2 罗勤尧修改 
        /// </summary>
        /// <returns></returns>
        [Privilege(PrivilegeCode.DS1002101)]
        public ActionResult GetGetDaiLiShangListByCurUser()
        {
            string Type; //ZD(总代理),D(代理),F(分销商)
            int TypeSysNo; //传入的对应类型系统编号
            if (!CurrentUser.IsBindAllDealer)
            {
                //判断是否绑定分销商
                if (!CurrentUser.IsBindDealer)
                {
                    Type = "D";
                    TypeSysNo = CurrentUser.Base.SysNo;
                }
                else
                {
                    Type = "F";
                    TypeSysNo = CurrentUser.Dealer.SysNo;
                }
            }
            else
            {
                Type = "ZD";
                TypeSysNo = 0;
            }

            var list = DsDealerBo.Instance.GetDaiLiShangList(Type, TypeSysNo).Select(m => new
            {
                SysNo = m.SysNo,
                DealerName = m.DealerName,
                Status=m.Status
            });
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获得当前用户有权限看到的分销商
        /// 2016-1-4 王耀发 创建
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [Privilege(PrivilegeCode.DS1002101)]
        public ActionResult GetDealersListByCurUser()
        {
            ParaDsDealerFilter filter = new ParaDsDealerFilter();
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
            //var list = DsDealerBo.Instance.GetDealersListByCurUser(filter);
            var list = DsDealerBo.Instance.GetDealersListByCurUser(filter).Select(m => new
            {
                SysNo = m.SysNo,
                DealerName = m.DealerName
            });
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获得创建用户对应的分销商
        /// 2016-1-29 王耀发 创建
        /// </summary>
        /// <param name="DealerCreatedBy"></param>
        /// <returns></returns>
        [Privilege(PrivilegeCode.DS1002101)]
        public ActionResult GetDealersListByCreatedBy()
        {
            string DealerCreatedBy = this.Request["DealerCreatedBy"];
            string Type; //ZD(总代理),D(代理),F(分销商)
            int TypeSysNo; //传入的对应类型系统编号
            if (!CurrentUser.IsBindAllDealer)
            {
                //判断是否绑定分销商
                if (!CurrentUser.IsBindDealer)
                {
                    Type = "D";
                    TypeSysNo = 0;
                }
                else
                {
                    Type = "F";
                    TypeSysNo = CurrentUser.Dealer.SysNo;
                }
            }
            else
            {
                Type = "ZD";
                TypeSysNo = 0;
            }
            try
            {
                var list = DsDealerBo.Instance.GetDealersListByCreatedBy(int.Parse(DealerCreatedBy), Type, TypeSysNo).Select(m => new
                {
                    SysNo = m.SysNo,
                    DealerName = m.DealerName
                });
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                List<DsDealer> list = new List<DsDealer>();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 分销商信息新建视图
        /// </summary>
        /// <param></param>
        /// <returns>分销商信息新建页面</returns>
        /// <remarks> 
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.DS1002201)]
        public ActionResult DsDealerAdd()
        {
            var list = DsDealerLevelBo.Instance.GetDsDealerLevelList(DistributionStatus.分销商等级状态.启用);
            return View(list);
        }

        /// <summary>
        /// 分销商信息修改页面
        /// </summary>
        /// <param name="sysNo">分销商系统编号</param>
        /// <returns>分销商信息修改页面</returns>
        /// <remarks> 
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.DS1002301)]
        public ActionResult DsDealerUpdate(int sysNo)
        {
            //传递分销商等级
            ViewBag.DsDealerLevel = DsDealerLevelBo.Instance.GetDsDealerLevelList(DistributionStatus.分销商等级状态.启用);
            var model = DsDealerBo.Instance.GetDsDealer(sysNo);
          
            var extensions=model.Extensions.ToObject<StoreExtensions>();
            ViewBag.Extensions = extensions==null?new StoreExtensions():extensions;
      
            var _model=BLL.Distribution.DsDealerPayTypeBo.Instance.GetByDealerSysNo(sysNo);
            ViewBag.DealerPayType = _model == null ? (new DsDealerPayType()) : _model;
            return View(model);
        }

        /// <summary>
        /// 查看分销商信息
        /// </summary>
        /// <param name="sysNo">分销商系统编号</param>
        /// <returns>分销商信息查看页面</returns>
        /// <remarks> 
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.DS1002101)]
        public ActionResult DsDealerView(int sysNo)
        {
            var model = DsDealerBo.Instance.GetDsDealer(sysNo);
            AttachmentConfig attrConfig = Hyt.BLL.Config.Config.Instance.GetAttachmentConfig();
            model.ImageUrl = attrConfig.FileServer + model.ImageUrl;
            model.IcoUrl = attrConfig.FileServer + model.IcoUrl;
            return View(model);
        }

        /// <summary>
        /// Ajax新增分销商
        /// </summary>
        /// <param name="model">分销商实体</param>
        /// <returns>
        /// 成功result.StatusCode>0,失败 result.StatusCode=0
        /// </returns>
        /// <remarks> 
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.DS1002201)]
        public JsonResult AjaxDsDealerCreate(DsDealer model)
        {
        

            var list = DsDealerBo.Instance.GetDsDealerList(model.DealerName);
            if (list != null && list.Count != 0)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

            var userModel = BLL.Sys.SyUserBo.Instance.GetSyUser(model.UserSysNo);
            if (userModel == null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            if (userModel.Status == (int)SystemStatus.系统用户状态.禁用)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

            var dsDealerList = DsDealerBo.Instance.GetDsDealerList(model.UserSysNo);
            if (dsDealerList != null && dsDealerList.Count > 0)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            var hasDealerGroup = Hyt.BLL.Sys.SyUserGroupBo.Instance.IsHasDealerGroup(model.UserSysNo);
            if (!hasDealerGroup)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        
            if (!Hyt.BLL.Distribution.DsDealerBo.Instance.CheckErpCode(model.ErpCode))
            {
                return Json(new Result { Message = "商城编号在金蝶客户资料里不存在！", StatusCode = -2 });
            }
            model.CreatedDate = DateTime.Now;
            model.LastUpdateBy = CurrentUser.Base.SysNo;
            model.LastUpdateDate = DateTime.Now;
            int deralerSysNo=DsDealerBo.Instance.Create(model);

            #region 分销商支付接口秘钥配置
            if (deralerSysNo > 0)
            {
                var dealerPayType = new DsDealerPayType();
                dealerPayType.AppKey = Request["PayAppKey"];
                dealerPayType.AppSecret = Request["PayAppSecret"];
                dealerPayType.DealerSysNo = deralerSysNo;
                dealerPayType.PaymentTypeSysNo = 12;//易宝支付

                if (!string.IsNullOrWhiteSpace(dealerPayType.AppKey)&&!BLL.Distribution.DsDealerPayTypeBo.Instance.IsOnlyAppKey(dealerPayType.AppKey, deralerSysNo))
                {
                    return Json(new Result { Message = "商户号在系统中已存在！",StatusCode = -1 });
                }
              
                BLL.Distribution.DsDealerPayTypeBo.Instance.Create(dealerPayType);                                        
            }
            #endregion

            var result = new Result { Message = "", StatusCode = deralerSysNo };
            return Json(result);
        }

        /// <summary>
        /// Ajax修改分销商
        /// </summary>
        /// <param name="model">分销商实体</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks> 
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.DS1002301)]
        public JsonResult AjaxDsDealerUpdate(DsDealer model)
        {
            var userModel = BLL.Sys.SyUserBo.Instance.GetSyUser(model.UserSysNo);
            var Dealermodel = DsDealerBo.Instance.GetDsDealer(model.SysNo);
            if (userModel == null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            if (userModel.Status == (int)SystemStatus.系统用户状态.禁用)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

            var dsDealerList = DsDealerBo.Instance.GetDsDealerList(model.UserSysNo, model.SysNo);
            if (dsDealerList != null && dsDealerList.Count > 0)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            var hasDealerGroup = Hyt.BLL.Sys.SyUserGroupBo.Instance.IsHasDealerGroup(model.UserSysNo);
            if (!hasDealerGroup)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

            if (!Hyt.BLL.Distribution.DsDealerBo.Instance.CheckErpCode(model.ErpCode))
            {
                return Json(new Result { Message = "商城编号在金蝶客户资料里不存在！", StatusCode = -2 });
            }
            model.LastUpdateBy = CurrentUser.Base.SysNo;
            model.LastUpdateDate = DateTime.Now;
            //利嘉的标号
            model.LiJiaSysNo = Dealermodel.LiJiaSysNo;
            int rows=DsDealerBo.Instance.Update(model);
            if (rows > 0)
            {
                var dealerPayType = new DsDealerPayType();
                dealerPayType.AppKey = Request["PayAppKey"];
                dealerPayType.AppSecret = Request["PayAppSecret"];
                dealerPayType.DealerSysNo = model.SysNo;
                dealerPayType.PaymentTypeSysNo = 12;//易宝支付

                if (!string.IsNullOrWhiteSpace(dealerPayType.AppKey) && !BLL.Distribution.DsDealerPayTypeBo.Instance.IsOnlyAppKey(dealerPayType.AppKey,model.SysNo))
                {
                    return Json(new Result { Message = "商户号在系统中已存在！", StatusCode = -1 });
                }

                var _dealerInfo=BLL.Distribution.DsDealerPayTypeBo.Instance.GetByDealerSysNo(model.SysNo);
                dealerPayType.SysNo = _dealerInfo != null ? _dealerInfo.SysNo : 0;

               

                if (_dealerInfo == null)              
                    BLL.Distribution.DsDealerPayTypeBo.Instance.Create(dealerPayType);
                else
                    BLL.Distribution.DsDealerPayTypeBo.Instance.Update(dealerPayType);
            }

            var result = new Result { Status = rows > 0, Message = "", StatusCode = model.SysNo };
            return Json(result);
        }
        /// <summary>
        /// 判断微信AppID、AppSecret是否合法
        /// </summary>
        /// <param name="DealerSysNo"></param>
        /// <returns></returns>
        /// <remarks> 
        /// 2016-05-10 王耀发 创建
        /// </remarks>
        [Privilege(PrivilegeCode.DS1002301, PrivilegeCode.DS1002201)]
        public ActionResult GetCallBackIp(int DealerSysNo)
        {
            Result result = new Result();

            CBDsDealer Dealer = DsDealerBo.Instance.GetDsDealer(DealerSysNo);
            if (Dealer.AppID != null || Dealer.AppSecret != null)
            {
                using (var service = new Hyt.Infrastructure.Communication.ServiceProxy<Hyt.Service.Contract.WeiXin.IWebChatService>())
                {
                    result = service.Channel.GetCallBackIp(DealerSysNo);
                    if (result.Status == false)
                    {
                        result.Message += " : AppID、AppSecret 输入错误!";
                        service.Channel.RemoveWeiXinCache(DealerSysNo, "");
                        DsDealerBo.Instance.UpdateAppIDandSecret(DealerSysNo, null, null);
                    }
                }


            }
            else
            {
                result.Status = true;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Ajax修改分销商状态
        /// </summary>
        /// <param name="sysNo">分销商系统编号</param>
        /// <param name="status">分销商状态</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks> 
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.DS1002701)]
        public JsonResult AjaxDsDealerUpdateStatus(int sysNo, int status)
        {
            var t = (DistributionStatus.分销商状态)status;
            return Json(DsDealerBo.Instance.UpdateStatus(sysNo, t, CurrentUser.Base.SysNo) > 0, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 推送分销商到你他购
        /// </summary>
        /// <param name="sysNo">分销商系统编号</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks> 
        /// 2016-11-07 罗远康 创建
        /// </remarks>
        [Privilege(PrivilegeCode.DS1002301)]
        public JsonResult PostDistriToNitago(int sysNo)
        {
            string url = "http://www.nitago.com/api/third/merchant.php?do=regist";//分销商
            var result = new Result();
            result.Message = "";
            var model = DsDealerBo.Instance.GetDsDealer(sysNo);
            #region 判断数据是否为空
            if (model == null)
            {
                result.Message = "该分销商不存在！";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            if (model.ErpName == null || model.ErpName == "")
            {
                result.Message += "商城名称不为空！";
            }
            if (model.StreetAddress == null || model.StreetAddress == "")
            {
                result.Message += "门店所在地不为空！";
            }
            //if (model.DomainName == null || model.DomainName == "")
            //{
            //    result.Message += "域名不为空！";
            //}
            if (model.MobilePhoneNumber == null || model.MobilePhoneNumber == "")
            {
                result.Message += "手机号码不为空";
            }
            if (model.Description == null || model.Description == "")
            {
                result.Message += "描述不为空";
            }
            if (model.Keyword == null || model.Keyword == "")
            {
                result.Message += "关键词不为空";
            }
            if (model.Contact == null || model.Contact == "")
            {
                result.Message += "联系人不为空";
            }
            if (model.ImageUrl == null || model.ImageUrl == "")
            {
                result.Message += "Logo不为空";
            }
            if (result.Message != "")
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            #endregion
            #region 推送数据
            AttachmentConfig attrConfig = Hyt.BLL.Config.Config.Instance.GetAttachmentConfig();
            model.ImageUrl = attrConfig.FileServer + model.ImageUrl;//门店LOGO
            string Domain = "http://wx.singingwhale.cn/";

            string DomainName = Domain+model.SysNo;//门店域名，你他购以改链接跳转
            if (model.DomainName != null && model.DomainName != "")
            {
                Domain = "http://"+model.DomainName;
            }
            
            Dictionary<string, string> date = new Dictionary<string, string>();
            date.Add("third_id", Convert.ToString(model.SysNo));//经销商在信营系统中的ID[必填]
            date.Add("zone_code", Convert.ToString(model.AreaSysNo));//所在行政区的国家标准编码[必填]
            date.Add("name", model.ErpName);//门店名称[必填]
            date.Add("shop_addres", model.StreetAddress);//门店所在地址[必填]
            date.Add("shop_url", DomainName);//门店链接地址[必填]
            date.Add("map_lat", "114.144357");//纬度[必填]
            date.Add("map_lng", "22.586796");//经度[必填]
            date.Add("shop_tel", model.MobilePhoneNumber);//门店电话[必填]
            date.Add("shop_note", model.Description);//门店简介[必填]
            date.Add("shop_tags", model.Keyword);//门店标签[必填]
            date.Add("username", model.Account);//门店用户名[必填]
            date.Add("userpass", "123456");//门店B端登入密码[必填]
            date.Add("tel", model.MobilePhoneNumber);//联系人[必填]
            date.Add("imgurl", model.ImageUrl);//门店Logo[必填]
            #endregion
            #region 执行推送并返回数据
            ///调用接口
            Nitaga lijia = new Nitaga(url);
            ///获得接口返回值
            string sAPIResult = "";
            sAPIResult = lijia.Post(date);

            BLL.Log.LocalLogBo.Instance.Write(sAPIResult, "PostDistriToNitagoLog");
            
           // JsonData jsonDate = JsonMapper.ToObject(sAPIResult);
            var jsonDate=JObject.Parse(sAPIResult);
            int backcode = 0;

            if (jsonDate.Property("backcode") != null)
            {
                backcode = int.Parse(jsonDate["backcode"].ToString());
            }
            if (backcode == 1)
            {
                int merchant_id = -1;
                if (jsonDate.Property("merchant_id")!=null)
                {
                    merchant_id = Convert.ToInt32(jsonDate["merchant_id"].ToString());
                    if (merchant_id != -1)
                    {
                        var NitagoDateBind = BLL.Web.MKNitagoDateBindBo.Instance.Select(0, model.SysNo);
                        if (NitagoDateBind == null)//判断是否已绑定
                        {
                            //执行插入绑定利嘉（你他购）平台分销商
                            var nitago = new MKNitagoDateBind
                            {
                                BindDateTepy = 0,
                                XinyingDateSysNo = model.SysNo,
                                NitagoDateSysNo = merchant_id,
                                CreatedDate = DateTime.Now,
                            };
                            BLL.Web.MKNitagoDateBindBo.Instance.Insert(nitago);
                            result.Status = true;
                            result.Message = "推送成功";
                            BLL.Log.SysLog.Instance.Error(Model.WorkflowStatus.LogStatus.系统日志来源.外部应用, "成功推送分销商：" + model.SysNo + "到你他购!", new Exception());
                        }
                        else
                        {
                            result.Status = true;
                            result.Message = "已推送过！";
                        }
                    }
                } 
            }
            else
            {
                string error = "";
                if (jsonDate.Property("error") != null)
                {
                    error = jsonDate["error"].ToString();
                }
                result.Status = false;
                result.Message = error;
                BLL.Log.SysLog.Instance.Error(Model.WorkflowStatus.LogStatus.系统日志来源.外部应用, "推送分销商：" + model.SysNo + "到你他购失败！" + error, new Exception());
            }
            #endregion
           
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// 通过ajax判断是否已有此分销商
        /// </summary>
        /// <param name="dealerName">分销商名称</param>
        /// <returns>
        /// 已有不允许添加返回false,未有允许添加返回true
        /// </returns>
        /// <remarks> 
        /// 2013-09-04 郑荣华 创建
        /// </remarks>   
        [Privilege(PrivilegeCode.DS1002201)]
        public JsonResult IsCanAddDsDealerNameAdd(string dealerName)
        {
            var list = DsDealerBo.Instance.GetDsDealerList(dealerName);
            return Json(list == null || list.Count == 0, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 通过ajax判断是否已有此分销商
        /// </summary>
        /// <param name="AppID">AppID</param>
        /// <param name="sysNo">sysNo</param>
        /// <returns></returns>
        /// 2015-12-22 王耀发 创建
        [Privilege(PrivilegeCode.DS1002201)]
        public JsonResult IsCanAddDsDealerName(string dealerName, int? sysNo)
        {
            var result = new Result();
            var dsDealerList = sysNo == null ? DsDealerBo.Instance.GetDsDealerList(dealerName) : DsDealerBo.Instance.GetDsDealerListByDealerName(dealerName, (int)sysNo);
            if (dsDealerList != null && dsDealerList.Count > 0)
            {
                result.Message = "此分销商名称已存在";
                result.Status = false;
                result.StatusCode = -3;
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            result.Message = "此分销商名称合法";
            result.Status = true;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 通过ajax判断是否已有此分销商
        /// </summary>
        /// <param name="AppID">AppID</param>
        /// <returns></returns>
        /// 2015-12-22 王耀发 创建
        [Privilege(PrivilegeCode.DS1002201)]
        public JsonResult IsCanAddAppIDAdd(string appID)
        {
            var list = DsDealerBo.Instance.GetDsDealerListByAppID(appID);
            return Json(list == null || list.Count == 0, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 通过ajax判断是否已有此分销商
        /// </summary>
        /// <param name="AppID">AppID</param>
        /// <param name="sysNo">sysNo</param>
        /// <returns></returns>
        /// 2015-12-22 王耀发 创建
        [Privilege(PrivilegeCode.DS1002201)]
        public JsonResult IsCanAddAppID(string appID, int? sysNo)
        {
            var result = new Result();
            var dsDealerList = sysNo == null ? DsDealerBo.Instance.GetDsDealerListByAppID(appID) : DsDealerBo.Instance.GetDsDealerListByAppID(appID, (int)sysNo);
            if (dsDealerList != null && dsDealerList.Count > 0)
            {
                result.Message = "此AppID已被其它分销商关联";
                result.Status = false;
                result.StatusCode = -3;
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            result.Message = "AppID合法";
            result.Status = true;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 通过ajax判断是否已有此分销商
        /// </summary>
        /// <param name="AppSecret">AppSecret</param>
        /// <returns></returns>
        /// 2015-12-22 王耀发 创建
        [Privilege(PrivilegeCode.DS1002201)]
        public JsonResult IsCanAddAppSecretAdd(string appSecret)
        {
            var list = DsDealerBo.Instance.GetDsDealerListByAppSecret(appSecret);
            return Json(list == null || list.Count == 0, JsonRequestBehavior.AllowGet);
        }
        [Privilege(PrivilegeCode.DS1002201)]
        public JsonResult IsCanAddAppSecret(string appSecret, int? sysNo)
        {
            var result = new Result();
            var dsDealerList = sysNo == null ? DsDealerBo.Instance.GetDsDealerListByAppSecret(appSecret) : DsDealerBo.Instance.GetDsDealerListByAppSecret(appSecret, (int)sysNo);
            if (dsDealerList != null && dsDealerList.Count > 0)
            {
                result.Message = "此AppSecret已被其它分销商关联";
                result.Status = false;
                result.StatusCode = -3;
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            result.Message = "AppSecret合法";
            result.Status = true;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 通过ajax判断是否已有此分销商
        /// </summary>
        /// <param name="WeiXinNum">微信公众号</param>
        /// <returns></returns>
        /// 2015-12-22 王耀发 创建
        [Privilege(PrivilegeCode.DS1002201)]
        public JsonResult IsCanAddWeiXinNumAdd(string weiXinNum)
        {
            var list = DsDealerBo.Instance.GetDsDealerListByWeiXinNum(weiXinNum);
            return Json(list == null || list.Count == 0, JsonRequestBehavior.AllowGet);
        }
        [Privilege(PrivilegeCode.DS1002201)]
        public JsonResult IsCanAddWeiXinNum(string weiXinNum, int? sysNo)
        {
            var result = new Result();
            var dsDealerList = sysNo == null ? DsDealerBo.Instance.GetDsDealerListByWeiXinNum(weiXinNum) : DsDealerBo.Instance.GetDsDealerListByWeiXinNum(weiXinNum, (int)sysNo);
            if (dsDealerList != null && dsDealerList.Count > 0)
            {
                result.Message = "此微信公众号已被其它分销商关联";
                result.Status = false;
                result.StatusCode = -3;
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            result.Message = "微信公众号合法";
            result.Status = true;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 通过ajax判断是否已有此分销商
        /// </summary>
        /// <param name="DomainName">域名</param>
        /// <returns></returns>
        /// 2015-12-22 王耀发 创建
        [Privilege(PrivilegeCode.DS1002201)]
        public JsonResult IsCanAddDomainNameAdd(string domainName)
        {
            var list = DsDealerBo.Instance.GetDsDealerListByDomainName(domainName);
            return Json(list == null || list.Count == 0, JsonRequestBehavior.AllowGet);
        }
        [Privilege(PrivilegeCode.DS1002201)]
        public JsonResult IsCanAddDomainName(string domainName, int? sysNo)
        {
            var result = new Result();
            var dsDealerList = sysNo == null ? DsDealerBo.Instance.GetDsDealerListByDomainName(domainName) : DsDealerBo.Instance.GetDsDealerListByDomainName(domainName, (int)sysNo);
            if (dsDealerList != null && dsDealerList.Count > 0)
            {
                result.Message = "此域名已被其它分销商关联";
                result.Status = false;
                result.StatusCode = -3;
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            result.Message = "域名合法";
            result.Status = true;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 通过ajax判断关联的系统账号是否合法
        /// </summary>
        /// <param name="account">系统用户账号</param>
        /// <param name="sysNo">新建页面不传，修改页面传入分销商系统编号</param>
        /// <returns>
        /// 合法则StatusCode为取得的userSysNo
        /// </returns>
        /// <remarks> 
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.DS1002201, PrivilegeCode.DS1002301)]
        public JsonResult IsRightContactSysUser(string account, int? sysNo)
        {
            var result = new Result();
            var userModel = BLL.Sys.SyUserBo.Instance.GetSyUser(account);
            if (userModel == null)
            {
                result.Message = "不存在此账号";
                result.Status = false;
                result.StatusCode = -1;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            if (userModel.Status == (int)SystemStatus.系统用户状态.禁用)
            {
                result.Message = "此账号未启用";
                result.Status = false;
                result.StatusCode = -2;
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            var dsDealerList = sysNo == null ? DsDealerBo.Instance.GetDsDealerList(userModel.SysNo) : DsDealerBo.Instance.GetDsDealerList(userModel.SysNo, (int)sysNo);
            if (dsDealerList != null && dsDealerList.Count > 0)
            {
                result.Message = "此账号已被其它分销商关联";
                result.Status = false;
                result.StatusCode = -3;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            var hasDealerGroup = SyUserGroupBo.Instance.IsHasDealerGroup(userModel.SysNo);
            if (!hasDealerGroup)
            {
                result.Message = "此账号不是分销商组账号";
                result.Status = false;
                result.StatusCode = -4;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            result.Message = "系统账号合法";
            result.Status = true;
            result.StatusCode = userModel.SysNo;
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region 选择分销商

        /// <summary>
        /// 获取分销商
        /// </summary>
        /// <param name="id">页码</param>
        /// <param name="filter">查询条件</param>
        /// <returns>分销商页面</returns>
        /// <remarks> 
        /// 2013-09-05 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.DS1002101)]
        public ActionResult SelectDsDealer(int? id, ParaDsDealerFilter filter)
        {
            if (Request.IsAjaxRequest())
            {
                //列表分页开始
                var model = new PagedList<CBDsDealer>();

                var modelRef = new Pager<CBDsDealer> { CurrentPage = id ?? 1, PageSize = model.PageSize };
                DsDealerBo.Instance.GetDsDealerList(ref modelRef, filter);

                model.TotalItemCount = modelRef.TotalRows;
                model.TData = modelRef.Rows;
                model.CurrentPageIndex = modelRef.CurrentPage;
                model.OnComplete = "AjaxOnComplete";
                return PartialView("_SelectDsDealerPager", model);
            }
            ViewBag.ProductSysNo = filter.ProductSysNo;
            ViewBag.Status = filter.Status;
            var pdModel = Hyt.BLL.Product.PdProductBo.Instance.GetProduct(filter.ProductSysNo ?? 0);
            ViewBag.ProductName = pdModel == null ? "" : pdModel.ProductName;
            return View("_SelectDsDealer");
        }

        #endregion

        #region 分销商等级维护 2013-09-04 郑荣华 创建

        /// <summary>
        /// 分销商等级维护主页面
        /// </summary>
        /// <param></param>
        /// <returns>分销商等级维护主页面</returns>
        /// <remarks> 
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.DS1003101)]
        public ActionResult DsDealerLevel()
        {
            if (Request.IsAjaxRequest())
            {
                var model = DsDealerLevelBo.Instance.GetDsDealerLevelList();
                return PartialView("_AjaxListDsDealerLevel", model);
            }
            return View();
        }

        /// <summary>
        /// 分销商等级信息新建视图
        /// </summary>
        /// <param></param>
        /// <returns>分销商等级信息新建页面</returns>
        /// <remarks> 
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.DS1003201)]
        public ActionResult DsDealerLevelAdd()
        {
            return View();
        }

        /// <summary>
        /// 分销商等级信息修改页面
        /// </summary>
        /// <param name="sysNo">等级系统编号</param>
        /// <returns>分销商等级信息修改页面</returns>
        /// <remarks> 
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.DS1003301)]
        public ActionResult DsDealerLevelUpdate(int sysNo)
        {
            var model = DsDealerLevelBo.Instance.GetDsDealerLevel(sysNo);
            return View(model);
        }

        /// <summary>
        /// Ajax新增分销商等级
        /// </summary>
        /// <param name="model">分销商等级实体</param>
        /// <returns>
        /// 成功result.StatusCode>0,失败 result.StatusCode=0
        /// </returns>
        /// <remarks> 
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.DS1003201)]
        public JsonResult AjaxDsDealerLevelCreate(DsDealerLevel model)
        {
            model.CreatedBy = CurrentUser.Base.SysNo;
            model.CreatedDate = DateTime.Now;
            model.LastUpdateBy = CurrentUser.Base.SysNo;
            model.LastUpdateDate = DateTime.Now;

            var result = new Result { Message = "", StatusCode = DsDealerLevelBo.Instance.Create(model) };
            return Json(result);
        }

        /// <summary>
        /// Ajax修改分销商等级
        /// </summary>
        /// <param name="model">分销商等级实体</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks> 
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.DS1003301)]
        public JsonResult AjaxDsDealerLevelUpdate(DsDealerLevel model)
        {
            model.LastUpdateBy = CurrentUser.Base.SysNo;
            model.LastUpdateDate = DateTime.Now;

            return Json(DsDealerLevelBo.Instance.Update(model) > 0);
        }

        /// <summary>
        /// Ajax修改分销商等级状态
        /// </summary>
        /// <param name="sysNo">分销商等级系统编号</param>
        /// <param name="status">分销商等级状态</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks> 
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.DS1003701)]
        public JsonResult AjaxDsDealerLevelUpdateStatus(int sysNo, int status)
        {
            var t = (DistributionStatus.分销商等级状态)status;
            return Json(DsDealerLevelBo.Instance.UpdateStatus(sysNo, t, CurrentUser.Base.SysNo, DateTime.Now) > 0, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 通过ajax判断是否已有此分销商等级
        /// </summary>
        /// <param name="levelName">分销商等级名称</param>
        /// <returns>
        /// 已有不允许添加返回false,未有允许添加返回true
        /// </returns>
        /// <remarks> 
        /// 2013-09-04 郑荣华 创建
        /// </remarks>      
        [Privilege(PrivilegeCode.DS1003201)]
        public JsonResult IsCanAddDsDealerLevelName(string levelName)
        {
            var list = DsDealerLevelBo.Instance.GetDsDealerLevel(levelName);
            return Json(list == null, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 分销商城类型维护 2013-09-04 郑荣华 创建

        /// <summary>
        /// 分销商城类型维护主页面
        /// </summary>
        /// <param name="mallName"></param>
        /// <param name="isPreDeposit"></param>
        /// <param name="status"></param>
        /// <returns>分销商城类型维护主页面</returns>
        /// <remarks> 
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.DS1004101)]
        public ActionResult DsMallType(string mallName, int? isPreDeposit, int? status)
        {
            if (Request.IsAjaxRequest())
            {
                var model = DsMallTypeBo.Instance.GetDsMallTypeList(mallName, isPreDeposit, status);
                return PartialView("_AjaxListDsMallType", model);
            }
            return View();
        }

        /// <summary>
        /// 分销商城类型新建视图
        /// </summary>
        /// <param></param>
        /// <returns>分销商城类型新建页面</returns>
        /// <remarks> 
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.DS1004201)]
        public ActionResult DsMallTypeAdd()
        {
            return View();
        }

        /// <summary>
        /// 分销商城类型信息修改页面
        /// </summary>
        /// <param name="sysNo">分销商城类型系统编号</param>
        /// <returns>分销商城类型信息修改页面</returns>
        /// <remarks> 
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.DS1004301)]
        public ActionResult DsMallTypeUpdate(int sysNo)
        {
            var model = DsMallTypeBo.Instance.GetDsMallType(sysNo);
            return View(model);
        }

        /// <summary>
        /// Ajax新增分销商城类型
        /// </summary>
        /// <param name="model">分销商城类型实体</param>
        /// <returns>
        /// 成功result.StatusCode>0,失败 result.StatusCode=0
        /// </returns>
        /// <remarks> 
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.DS1004201)]
        public JsonResult AjaxDsMallTypeCreate(DsMallType model)
        {
            var result = new Result { Message = "", StatusCode = DsMallTypeBo.Instance.Create(model) };
            return Json(result);
        }

        /// <summary>
        /// Ajax修改分销商城类型
        /// </summary>
        /// <param name="model">分销商城类型实体</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks> 
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.DS1004301)]
        public JsonResult AjaxDsMallTypeUpdate(DsMallType model)
        {
            return Json(DsMallTypeBo.Instance.Update(model) > 0);
        }

        /// <summary>
        /// Ajax修改分销商城类型状态
        /// </summary>
        /// <param name="sysNo">商城类型系统编号</param>
        /// <param name="status">商城类型状态</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks> 
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.DS1004701)]
        public JsonResult AjaxDsMallTypeUpdateStatus(int sysNo, int status)
        {
            var t = (DistributionStatus.商城类型状态)status;
            return Json(DsMallTypeBo.Instance.UpdateStatus(sysNo, t) > 0, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 通过ajax判断是否已有此分销商城类型
        /// </summary>
        /// <param name="mallCode">分销商城类型代号</param>
        /// <returns>
        /// 已有不允许添加返回false,未有允许添加返回true
        /// </returns>
        /// <remarks> 
        /// 2013-09-04 郑荣华 创建
        /// </remarks>     
        [Privilege(PrivilegeCode.DS1004201)]
        public JsonResult IsCanAddDsMallTypeCode(string mallCode)
        {
            var list = DsMallTypeBo.Instance.GetDsMallType(mallCode);
            return Json(list == null, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 分销商特殊价格维护 2013-09-04 周瑜 创建

        /// <summary>
        /// 获取特殊价格状态列表
        /// </summary>
        /// <param name="id">当前页</param>
        /// <param name="condition">条件</param>
        /// <returns>特殊价格状态列表</returns>
        /// <remarks>2013-09-04 周瑜 创建</remarks>
        [Privilege(PrivilegeCode.DS1001101)]
        public ActionResult GetDsSpecialPriceList(int? id, DsSpecialPriceSearchCondition condition)
        {
            var pageIndex = id ?? 1;
            //当前用户自己对应分销商
            int DealerSysNo = CurrentUser.Dealer.SysNo;
            condition.DealerSysNo = DealerSysNo;
            //当前用户对应分销商，2015-12-19 王耀发 创建
            var DealerSysNoList = CurrentUser.Dealers;
            if (DealerSysNoList.Count > 0)
            {
                condition.DealerSysNoList = new List<int>();
                DealerSysNoList.ForEach(x => condition.DealerSysNoList.Add(x.SysNo));
            }
            if (Request.IsAjaxRequest())
            {
                var data = DsSpecialPriceBo.Instance.QuickSearch(condition, pageIndex);
                return PartialView("_AjaxPagerDsSpecialPriceList", data);
            }
            var statusList = new List<SelectListItem>
                {
                        new SelectListItem {Text = @"全部", Value = "", Selected = true}
                };
            EnumUtil.ToListItem<DistributionStatus.分销商特殊价格状态>(ref statusList);
            ViewData["statusList"] = new SelectList(statusList, "Value", "Text");
            return View("DsSpecialPrice");
        }

        /// <summary>
        /// 特殊价格状态修改
        /// </summary>
        /// <param name="model">特殊价格实体</param>
        /// <returns>修改结果</returns>
        /// <remarks>2013-09-04 周瑜 创建</remarks>
        [Privilege(PrivilegeCode.DS1001301)]
        public JsonResult DsSpecialPriceUpdateStatus(DsSpecialPrice model)
        {
            var result = new Result { StatusCode = -1 };
            try
            {
                model.LastUpdateBy = CurrentUser.Base.SysNo;
                model.LastUpdateDate = DateTime.Now;

                DsSpecialPriceBo.Instance.UpdateStatus(model);
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
        /// 显示特殊价格新增页面
        /// </summary>
        /// <param name="model">特殊价格实体</param>
        /// <returns>特殊价格新增页面</returns>
        /// <remarks>2013-09-04 周瑜 创建</remarks>
        [Privilege(PrivilegeCode.DS1001201)]
        public ActionResult DsSpecialPriceAdd(DsSpecialPrice model)
        {
            return View("DsSpecialPriceAdd");
        }
        /// <summary>
        /// 显示产品搜索页面
        /// </summary>
        /// <param name="model">特殊价格实体</param>
        /// <returns>产品搜索页面</returns>
        /// <remarks>2013-09-04 周瑜 创建</remarks>
        [Privilege(PrivilegeCode.DS1001201)]
        public ActionResult DsSpecialPriceProductSearch(DsSpecialPrice model)
        {
            return View("DsSpecialPriceProductSearch");
        }

        //public JsonResult GetProduct(int productSysNo)
        //{
        //    var product = PdProductBo.Instance.GetProduct(productSysNo);

        //    var result = new Result
        //        {
        //            Status = true,
        //            StatusCode = 1
        //        };
        //    return Json(new { result, product }, JsonRequestBehavior.AllowGet);
        //}
        /// <summary>
        /// 特殊价格商品搜索
        /// </summary>
        /// <param name="model">商品搜索条件实体</param>
        /// <returns>价格商品搜索页面</returns>
        /// <remarks>2013-09-04 周瑜 创建</remarks>
        [Privilege(PrivilegeCode.DS1001101)]
        public ActionResult DsSpecialPriceProductList(ParaProductFilter model)
        {
            int pageIndex = model.id ?? 1;
            var list = new PagedList<CBPdProductDetail>();
            var pager = new Pager<CBPdProductDetail> { CurrentPage = pageIndex, PageSize = list.PageSize };

            PdProductBo.Instance.GetPdProductDetailList(ref pager, model);

            list = new PagedList<CBPdProductDetail>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows,
                PageSize = pager.PageSize
            };
            return PartialView("_AjaxPagerDsSpecialPricePoductList", list);

        }

        /// <summary>
        /// 特殊价格新增
        /// </summary>
        /// <param name="productSysNo">商品系统编号</param>
        /// <param name="price">价格</param>
        /// <param name="dealerList">分销商列表</param>
        /// <returns>操作结果</returns>
        /// <remarks>2013-09-04 周瑜 创建</remarks>
        [Privilege(PrivilegeCode.DS1001201)]
        public JsonResult DsSpecialPriceInsert(int productSysNo, int price, List<int> dealerList)
        {
            var result = new Result { StatusCode = -1 };
            try
            {
                if (dealerList != null && dealerList.Any())
                {
                    dealerList.ForEach(x =>
                    {
                        var model = new DsSpecialPrice()
                        {
                            DealerSysNo = x,
                            ProductSysNo = productSysNo,
                            Price = price,
                            CreatedBy = CurrentUser.Base.SysNo,
                            CreatedDate = DateTime.Now,
                            Status = DistributionStatus.分销商特殊价格状态.启用.GetHashCode()
                        };
                        DsSpecialPriceBo.Instance.Create(model);
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
        /// 插入特殊价格记录
        /// 2015-12-16 王耀发 创建
        /// </summary>
        /// <returns></returns>
        [Privilege(PrivilegeCode.DS1011505)]
        public JsonResult CreateSpecialPrice()
        {
            int DealerSysNo = int.Parse(this.Request["DealerSysNo"]);
            string ProductSysNo = this.Request["ProductSysNo"];
            string Price = this.Request["Price"];
            string ShopPrice = this.Request["ShopPrice"];
            var result = new Result();    
            decimal spPrice = 0;
            decimal.TryParse(ShopPrice, out spPrice);
            DsSpecialPrice model = new DsSpecialPrice();
            model.DealerSysNo = DealerSysNo;
            model.ProductSysNo = int.Parse(ProductSysNo);
            model.Price = decimal.Parse(Price);
            model.Status = DistributionStatus.分销商特殊价格状态.启用.GetHashCode();
            model.CreatedBy = CurrentUser.Base.SysNo;
            model.CreatedDate = DateTime.Now;
            model.LastUpdateBy = CurrentUser.Base.SysNo;
            model.LastUpdateDate = DateTime.Now;
            model.ShopPrice = spPrice;
            try
            {
                DsSpecialPrice entity = DsSpecialPriceBo.Instance.GetEntityByDPSysNo(DealerSysNo, int.Parse(ProductSysNo));
                if (entity == null)
                {
                    DsSpecialPriceBo.Instance.Create(model);
                }
                else
                {
                    DsSpecialPriceBo.Instance.UpdatePriceStatus(model.Price, model.ShopPrice, model.Status, entity.SysNo);
                }
                result.Status = true;
                result.Message = "上架成功";
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = "上架成功";
            }
            return Json(result);
        }

        /// <summary>
        /// 批量上架
        /// 2016-3-3 王耀发 创建
        /// </summary>
        /// <returns></returns>
        [Privilege(PrivilegeCode.DS1011504)]
        public JsonResult CreatePLSpecialPrice()
        {
            int DealerSysNo = int.Parse(this.Request["DealerSysNo"]);
            string spProductSysNoList = this.Request["spProductSysNoList"];
            string ProductSysNoList = this.Request["ProductSysNoList"];

            var result = new Result();
            try
            {

                if (!string.IsNullOrEmpty(ProductSysNoList))
                {
                    string[] ProductArray = ProductSysNoList.Split(',');
                    for (int i = 0; i < ProductArray.Length; i++)
                    {
                        //如果是总部直营店不做处理
                        //if (DealerSysNo == 347 || DealerSysNo == 336 || DealerSysNo == 44 || DealerSysNo == 14 || DealerSysNo == 0)
                        //{
                        //    continue;
                        //}
                        string[] synopriceArray = ProductArray[i].Split(':');
                        DsSpecialPrice model = new DsSpecialPrice();
                        model.DealerSysNo = DealerSysNo;
                        model.ProductSysNo = int.Parse(synopriceArray[0]);
                        model.Price = decimal.Parse(synopriceArray[1]);
                        model.Status = DistributionStatus.分销商特殊价格状态.启用.GetHashCode();
                        model.CreatedBy = CurrentUser.Base.SysNo;
                        model.CreatedDate = DateTime.Now;
                        model.LastUpdateBy = CurrentUser.Base.SysNo;
                        model.LastUpdateDate = DateTime.Now;

                        DsSpecialPriceBo.Instance.Create(model);

                        ///更新商品档案的修改时间
                        var product = PdProductBo.Instance.GetProductBySysNo(model.ProductSysNo);
                        if (product != null)
                        {
                            product.LastUpdateDate = DateTime.Now;
                            product.LastUpdateBy = CurrentUser.Base.SysNo;
                            product.Stamp = DateTime.Now;
                            PdProductBo.Instance.Update(product);
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(spProductSysNoList))
                {
                    string[] spProductArray = spProductSysNoList.Split(',');
                    for (int i = 0; i < spProductArray.Length; i++)
                    {
                        //如果是总部直营店不做处理
                        //if (DealerSysNo == 347 || DealerSysNo == 336 || DealerSysNo == 44 || DealerSysNo == 14 || DealerSysNo == 0)
                        //{
                        //    continue;
                        //}
                        string[] spSynopriceArray = spProductArray[i].Split(':');
                        int SysNo = int.Parse(spSynopriceArray[0]);
                        Decimal Price = Decimal.Parse(spSynopriceArray[1]);
                        int Status = Hyt.Model.WorkflowStatus.ProductStatus.商品状态.上架.GetHashCode();
                        DsSpecialPriceBo.Instance.UpdatePriceStatus(Price, Status, SysNo);
                        ///更新商品档案的修改时间
                        var product = PdProductBo.Instance.GetProductBySysNo(SysNo);
                        if (product != null)
                        {
                            product.LastUpdateDate = DateTime.Now;
                            product.LastUpdateBy = CurrentUser.Base.SysNo;
                            product.Stamp = DateTime.Now;
                            PdProductBo.Instance.Update(product);
                        }
                    }
                }
                else
                {
                    //如果是总部直营店不做处理
                    //if (DealerSysNo == 347 || DealerSysNo == 336 || DealerSysNo == 44 || DealerSysNo == 14 || DealerSysNo == 0)
                    //{
                      
                    //}else
                    //{
                        int Status = Hyt.Model.WorkflowStatus.ProductStatus.商品状态.上架.GetHashCode();
                        DsSpecialPriceBo.Instance.UpdateAllPriceStatus(DealerSysNo, Status);
                    //} 
                }
                result.Status = true;
                result.Message = "上架成功";
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
            }
            return Json(result);
        }

        /// <summary>
        /// 更新特殊价格状态
        /// 2016-1-3 王耀发 创建
        /// </summary>
        /// <returns></returns>
        [Privilege(PrivilegeCode.DS1011505)]
        public JsonResult UpdateSSPriceStatus()
        {
            string SysNo = this.Request["SysNo"];
            var result = new Result();
            try
            {
                int Status = DistributionStatus.分销商特殊价格状态.禁用.GetHashCode();
                var res = DsSpecialPriceBo.Instance.UpdateSSPriceStatus(Status, int.Parse(SysNo));
                result.Status = res.Status;
                result.Message = "下架成功";
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
            }
            return Json(result);
        }
        /// <summary>
        /// 显示类别
        /// 2017-06-08 罗熙 创建
        /// </summary>
        /// <returns></returns>
        public ActionResult ListOption(int ParentSysNo)
        {
            List<PdCategory> weightList = Hyt.BLL.Product.PdCategoryBo.Instance.GetCategoryListByParentName(ParentSysNo).ToList().Select(s => s.ParentCategory).ToList();
            return Json(weightList, JsonRequestBehavior.AllowGet);

        }
    
        /// <summary>
        /// 删除特殊价格记录
        /// 2015-12-16 王耀发 创建
        /// </summary>
        /// <returns></returns>
        [Privilege(PrivilegeCode.DS1001201)]
        public JsonResult DeleteSpecialPrice()
        {
            string SysNo = this.Request["SysNo"];
            var result = new Result();
            try
            {
                Result res = DsSpecialPriceBo.Instance.Delete(int.Parse(SysNo));
                result.Status = res.Status;
                result.Message = "下架成功";
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
            }
            return Json(result);
        }
        /// <summary>
        /// 判断调价
        /// 2015-12-16 王耀发 创建
        /// </summary>
        /// <returns></returns>
        [Privilege(PrivilegeCode.DS1001201)]
        public JsonResult JudgeSpecialPrice()
        {
            string Price = this.Request["Price"];
            string SalesPrice = this.Request["SalesPrice"];
            string DealerSysNo = this.Request["DealerSysNo"];
            var result = new Result();
            Decimal typePrice = 0;
            if (!Decimal.TryParse(Price, out typePrice))
            {
                result.Message = "价格须为数值";
                result.Status = false;
                return Json(result);
            }
            DsDealerLevel dlModel = DsDealerLevelBo.Instance.GetLevelByDealerSysNo(int.Parse(DealerSysNo));
            decimal SalePriceUpper = dlModel.SalePriceUpper;
            decimal SalePriceLower = dlModel.SalePriceLower;

            decimal MaxPrice = decimal.Parse(SalesPrice) + decimal.Parse(SalesPrice) * (SalePriceUpper / 100);
            decimal MinPrice = decimal.Parse(SalesPrice) - decimal.Parse(SalesPrice) * (SalePriceLower / 100);

            if (decimal.Parse(Price) > MaxPrice || decimal.Parse(Price) < MinPrice)
            {
                result.Status = false;
                result.Message = "调价范围在" + decimal.Round(MinPrice, 2) + "~" + decimal.Round(MaxPrice, 2) + "之间";
            }
            else
            {
                result.Status = true;
            }
            return Json(result);
        }
        /// <summary>
        /// 特殊价格修改
        /// </summary>
        /// <param name="model">特殊价格实体</param>
        /// <returns>操作结果</returns>
        /// <remarks>2013-09-04 周瑜 创建</remarks>
        [Privilege(PrivilegeCode.DS1001301)]
        public JsonResult DsSpecialPriceUpdate(DsSpecialPrice model)
        {
            var result = new Result { StatusCode = -1 };
            try
            {
                model.LastUpdateBy = CurrentUser.Base.SysNo;
                model.LastUpdateDate = DateTime.Now;

                DsSpecialPriceBo.Instance.Update(model);
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
        /// 修改价格
        /// </summary>
        /// <param name="model">特殊价格实体</param>
        /// <returns>操作结果</returns>
        /// <remarks>2013-09-04 周瑜 创建</remarks>
        [Privilege(PrivilegeCode.DS1001301)]
        public JsonResult DsSpecialPriceUpdatePrice(DsSpecialPrice model)
        {
            var result = new Result { StatusCode = -1 };
            try
            {
                model.LastUpdateBy = CurrentUser.Base.SysNo;
                model.LastUpdateDate = DateTime.Now;

                DsSpecialPriceBo.Instance.UpdatePrice(model);
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
        /// 获取商品详细信息列表
        /// </summary>
        /// <param name="pager">商品详细信息查询列表</param>
        /// <param name="dealerMallSysNo">分销商商城系统编号</param>
        /// <param name="condition">动态条件，CBPdProductDetail里不包含的条件</param>
        /// <returns></returns>
        /// <remarks>2015-12-10 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.DS1001102)]
        public ActionResult SpecialPriceProductList(ParaProductFilter productDetail)
        {
            //当前用户对应分销商，2015-12-19 王耀发 创建
            if (CurrentUser.IsBindDealer)
            {
                int DealerSysNo = CurrentUser.Dealer.SysNo;
                productDetail.DealerSysNo = DealerSysNo;
                productDetail.IsBindDealer = CurrentUser.IsBindDealer;
            }
            //是否绑定所有经销商
            productDetail.IsBindAllDealer = CurrentUser.IsBindAllDealer;
            productDetail.DealerCreatedBy = CurrentUser.Base.SysNo;

            int pageIndex = productDetail.id ?? 1;
            int status = 0;
            int IsFrontDisplay = 0;
            int.TryParse(Request.Params["status"], out status);
            int.TryParse(Request.Params["IsFrontDisplay"], out IsFrontDisplay);
            string name = Request.Params["name"];
            string selector = Request.Params["selector"];
            if (!string.IsNullOrEmpty(selector) && selector == "selector")
            {
                status = (int)ProductStatus.商品属性分组状态.启用;
            }
            status = -1;
            IsFrontDisplay = 1;
            var list = new PagedList<CBPdProductDetail>();
            list.PageSize = 15;
            var pager = new Pager<CBPdProductDetail>
            {
                CurrentPage = pageIndex,
                PageSize = list.PageSize,
                IdRows = new List<int>()
            };

            if (Request.IsAjaxRequest())
            {
                BLL.Distribution.DsSpecialPriceBo.Instance.GetSpecialPriceProductList(ref pager, productDetail);

                return PartialView("_AjaxSpecialPriceProductPager", pager.Map());
            }

            return View(list);
        }
        #endregion

        #region 经销商升舱订单
        /// <summary>
        /// 经销商升舱订单查看
        /// </summary>
        /// <param name="dsDetail"></param>
        /// <param name="id"></param>
        /// <returns>2017-8-21 罗熙 创建</returns>
        public ActionResult DealerOrder(ParaDsOrderFilter dsDetail,int? id=1)
        {
            int status =Convert.ToInt32(Request.Params["status"]);
            string mallOrderId = Request.Params["mallOrderId"];
            dsDetail.PageIndex = (int)id;
            var list = new PagedList<DsOrder>();
            list.PageSize = 15;
            var pager = new Pager<DsOrder>
            {

                CurrentPage = dsDetail.PageIndex,
                PageSize = list.PageSize,
                IdRows = new List<int>()
            };
            if (Request.IsAjaxRequest())
            {
                BLL.Distribution.DsSpecialPriceBo.Instance.GetDealerOrder(ref pager, dsDetail);
                return PartialView("_AjaxDsOrderPage", pager.Map());
            }
            return View(list);
            //BLL.Distribution.DsSpecialPriceBo.Instance.GetDealerOrder(ref pager);
            //return PartialView("_AjaxDsOrderPage", pager.Map());
        }
        /// <summary>
        /// 经销商退换货订单
        /// </summary>
        /// <param name="dsRMADetail"></param>
        /// <param name="id"></param>
        /// <returns>2017-8-29 罗熙 创建</returns>
        public ActionResult DsRMAorder(ParaDsReturnFilter dsRMADetail, int? id = 1)
        {
            int status = Convert.ToInt32(Request.Params["status"]);
            string mallOrderId = Request.Params["mallOrderId"];
            dsRMADetail.PageIndex = (int)id;
            var list = new PagedList<DsReturn>();
            list.PageSize = 15;
            var pager = new Pager<DsReturn>
            {

                CurrentPage = dsRMADetail.PageIndex,
                PageSize = list.PageSize,
                IdRows = new List<int>()
            };
            if (Request.IsAjaxRequest())
            {
                BLL.Distribution.DsSpecialPriceBo.Instance.GetDsRMAorder(ref pager, dsRMADetail);
                return PartialView("_AjaxDsRMAOrderPage", pager.Map());
            }
            return View(list);
        }
        /// <summary>
        /// 查看分销商详情
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns>2017-8-23 罗熙 创建</returns>
        [Privilege(PrivilegeCode.DS1002101)]
        public ActionResult DealerInfo(int sysNo)
        {
            var model = BLL.Distribution.DsSpecialPriceBo.Instance.GetDealerInfo(sysNo);
            return View(model);
        }
        /// <summary>
        /// 查看经销商退换货订单信息
        /// </summary>
        /// <param name="sysNo">退换货订单号</param>
        /// <returns>2017-8-29 罗熙 创建</returns>
        public ActionResult RMADealerInfo(int sysNo)
        {
            var model = BLL.Distribution.DsSpecialPriceBo.Instance.GetRMADealerInfo(sysNo); //获取经销商退换货订单
            //ViewBag.dsRMAorderPdInfo = BLL.Distribution.DsSpecialPriceBo.Instance.GetDsRMAorderPdInfo(sysNo);    
            return View(model);
        }
        /// <summary>
        /// 升舱订单商品
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns>2017-8-24 罗熙 创建</returns>
        //public ActionResult DealerOrderPdInfo(int sysNo)
        //{
        //    var model = BLL.Distribution.DsSpecialPriceBo.Instance.GetDealerOrderPdInfo(sysNo);    
        //    return View(model);
        //}

        /// <summary>
        /// 升舱订单明细
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns>2017-8-23 罗熙 创建</returns>
        [Privilege(PrivilegeCode.DS1002101)]
        public ActionResult UpOrderInfo(int sysNo)
        {
            var model = BLL.Distribution.DsSpecialPriceBo.Instance.GetUpOrderInfo(sysNo);
            
            return View(model);
        }
        [Privilege(PrivilegeCode.CM1005802)]
        public ActionResult OrderBrowse(int sysNo)
        {
            var model = BLL.Distribution.DsSpecialPriceBo.Instance.GetUpOrderModel(sysNo); //获取升舱订单
            ViewBag.dealerOrderPdInfo = BLL.Distribution.DsSpecialPriceBo.Instance.GetDealerOrderPdInfo(sysNo);    
            return View(model);
        }
        
        #endregion

        #region 分销商等级价格维护 2013-09-11 周瑜 创建
        /// <summary>
        /// 获取分销商等级价格列表
        /// </summary>
        /// <returns>分销商等级价格列表</returns>
        /// <param name="productDetail">条件</param>
        /// <remarks>2013-09-11 周瑜 创建</remarks>
        [Privilege(PrivilegeCode.DS1005101)]
        public ActionResult DsLevelPrice(ParaProductFilter productDetail)
        {
            var list = new PagedList<CBPdProductDetail>();
            var pager = new Pager<CBPdProductDetail> { CurrentPage = productDetail.id ?? 1, PageSize = list.PageSize };

            if (Request.IsAjaxRequest())
            {
                PdProductBo.Instance.GetPdProductDetailList(ref pager, productDetail);

                list = new PagedList<CBPdProductDetail>
                {
                    TData = pager.Rows,
                    CurrentPageIndex = pager.CurrentPage,
                    TotalItemCount = pager.TotalRows,
                    PageSize = pager.PageSize
                };
                return PartialView("_AjaxDsLevelPriceList", list);
            }

            return View(list);
        }

        /// <summary>
        /// 获取价格用户等级,显示批量调价页面
        /// </summary>
        /// <param name="productSysNoList">产品系统编号集合</param>
        /// <returns>返回商品的价格包括用户等级名称的Json数组</returns>
        /// <remarks>2013-09-11 周瑜 创建</remarks>
        [Privilege(PrivilegeCode.DS1005301)]
        public ActionResult ShowLevelPriceMultiChange(string productSysNoList)
        {
            //返回显示用的价格集 
            //读取会员等级
            var dsDealerLevel = DsLevelPriceBo.Instance.GetDsDealerLevel();

            //将所有等级添加到结果集中用于前台显示
            IList<CBPdPrice> pricelist = dsDealerLevel.Select(level => new CBPdPrice
            {
                Price = 0,
                PriceName = level.LevelName,
                SourceSysNo = level.SysNo,
                PriceSource = (int)ProductStatus.产品价格来源.分销商等级价,
                SysNo = level.SysNo
            }).ToList();
            var sysNoList = productSysNoList.Split(',').Select(m => Convert.ToInt32(m)).ToArray();
            ViewBag.Products = JsonConvert.SerializeObject(sysNoList);

            //返回结果
            return View("DsLevelPriceMulChange", pricelist);

        }

        /// <summary>
        /// 获取价格用户等级,显示单个商品调价页面
        /// </summary>
        /// <param name="productSysNo">产品系统编号</param>
        /// <returns>返回商品的价格包括用户等级名称的Json数组</returns>
        /// <remarks>2013-09-13 周瑜 创建</remarks>
        [Privilege(PrivilegeCode.DS1005101)]
        public ActionResult ShowLevelPriceSingleChange(int productSysNo)
        {
            //返回显示用的价格集 
            //读取会员等级
            var list = DsLevelPriceBo.Instance.GetLevelPriceByProdouctSysNo(productSysNo);
            var product = PdProductBo.Instance.GetProduct(productSysNo);

            ViewBag.Product = product;
            //返回结果
            return View("DsLevelPriceSingleChange", list);

        }

        /// <summary>
        /// 保存价格申请数据到价格申请表
        /// </summary>
        /// <param name="priceHistories">申请调价Json数据（申请调价数组）</param>
        /// <param name="productSysNoList">商品系统编号</param>
        /// <returns>返回调价申请是否成功 true:添加成功 false:添加失败</returns>
        /// <remarks>2013-09-13 周瑜 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.PD1001202, PrivilegeCode.DS1005301)]
        public JsonResult SaveProductPrice(string priceHistories, string productSysNoList)
        {
            //返回标志
            var success = true;

            //返回JSON数据
            var jsonData = new Result
            {
                StatusCode = 0,
                Status = true,
                Message = ""
            };

            //判断参数
            if (string.IsNullOrWhiteSpace(priceHistories))
            {
                jsonData.Message = "保存失败，请正确填写您的调价申请！！";
                jsonData.StatusCode = -1;
                success = false;
                return Json(jsonData);
            }

            IList<CBPdPriceHistory> pdPriceHistoryList = null;  //调价对象
            IList<int> pdProdcutSysNoList = new List<int>();               //产品ID

            try
            {
                //反序列化Json字符串为调价申请数组对象
                pdPriceHistoryList = JsonConvert.DeserializeObject<IList<CBPdPriceHistory>>(priceHistories);

                //序列化产品ID
                pdProdcutSysNoList = JsonConvert.DeserializeObject<IList<int>>(productSysNoList);

                //在PdPrice表没有数据之前申请分销商等级价格, 要先创建产品价格(PdPrice)
                foreach (var productSysNo in pdProdcutSysNoList)
                {
                    foreach (var history in pdPriceHistoryList)
                    {
                        //查询已有的分销商等级价格
                        var dealerLevelPrice = PdPriceHistoryBo.Instance.GetDealerProductLevelPrice(productSysNo,
                                                                                                    history.SourceSysNo);
                        if (dealerLevelPrice.Count == 0)
                        {
                            var p = new PdPrice
                            {
                                ProductSysNo = productSysNo,
                                Price = 0, //创建时价格为0, 审核后就为申请价格
                                PriceSource = ProductStatus.产品价格来源.分销商等级价.GetHashCode(),
                                SourceSysNo = history.SourceSysNo,
                                Status = 0 //创建的时候设为无效,审核后启用
                            };
                            history.PriceSysNo = PdPriceBo.Instance.Create(p);
                        }
                    }
                }

                //价格验证
                var result = ValidPrice(pdPriceHistoryList);
                if (!result.Status)
                {
                    jsonData.Message = result.Message;
                    jsonData.StatusCode = -1;
                    jsonData.Status = false;
                    return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                //出错表示格式不正确
                jsonData.StatusCode = -1;
                jsonData.Message = "保存失败，填写的数据不合法！！";
                success = false;
            }

            //根据前面数据验证来决定执行操作
            if (success)
            {
                //如果productSysNoList为空，表示是对单个商品调价
                success = pdProdcutSysNoList.Count() == 1
                              ? PdPriceHistoryBo.Instance.SavePdPriceHistory(pdPriceHistoryList.ToArray<PdPriceHistory>())
                              : PdPriceHistoryBo.Instance.SavePdPriceHistories(pdPriceHistoryList.ToArray(), pdProdcutSysNoList.ToArray()).Status;

                if (!success)
                {
                    jsonData.StatusCode = -1;
                    jsonData.Message = "保存失败！！";
                    jsonData.Status = false;
                }
                else
                {
                    jsonData.Message = "保存成功!";
                    jsonData.Status = true;
                    jsonData.StatusCode = 0;
                }
            }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 验证价格
        /// </summary>
        /// <param name="pdPriceHistoryList">历史价格列表</param>
        /// <returns>验证结果</returns>
        /// <remarks>2013-09-13 周瑜 创建</remarks>
        private Result ValidPrice(IEnumerable<CBPdPriceHistory> pdPriceHistoryList)
        {
            var result = new Result
            {
                Status = true,
                StatusCode = 1
            };

            #region 检查会员等级价格

            //查找所有商品会员等级价格
            var queryLevelPrice = pdPriceHistoryList.Where(p => p.PriceSource == (int)ProductStatus.产品价格来源.分销商等级价).ToList();

            //必须有等级价格，并且等级价格要超过1个才有可比性
            if (queryLevelPrice.Count > 1)
            {
                //第一个对比价格
                var tempPrice = queryLevelPrice[0];

                //检查会员等级价格
                for (var i = 1; i < queryLevelPrice.Count; i++)
                {
                    if (queryLevelPrice[i].ApplyPrice > tempPrice.ApplyPrice)
                    {
                        result.Message = ProductStatus.产品价格来源.分销商等级价.ToString() + "设置不正确，价格必须从高到底设置";
                        result.Status = false;
                        result.StatusCode = -1;
                        return result;
                    }

                    //缓存对比价格
                    tempPrice = queryLevelPrice[i];
                }
            }

            #endregion

            return result;
        }
        #endregion

        #region  分销商等级价格审核 2013-09-16 周瑜 创建
        /// <summary>
        /// 分销商等级价格审核首页
        /// </summary>
        /// <param name="id">起始页码</param>
        /// <param name="status">审批状态</param>
        /// <param name="sysNo">商品编号</param>
        /// <param name="erpCode">商品编号</param>
        /// <returns>价格等级修改记录列表</returns>
        /// <remarks>2013-09-16 周瑜 创建</remarks>
        /// <remarks>2013-11-04 周唐炬 重构</remarks>
        [Privilege(PrivilegeCode.DS1007101)]
        public ActionResult ProductPriceHistory(int? id, int? status, int? sysNo, string erpCode)
        {
            if (Request.IsAjaxRequest())
            {
                var model = DsLevelPriceBo.Instance.GetPriceHistorieList(id, status, sysNo, erpCode);
                return PartialView("_AjaxProductPriceHistory", model);
            }
            var dictList = EnumUtil.ToDictionary(typeof(ProductStatus.产品价格变更状态));
            ViewBag.DictList = dictList;
            return View();
        }

        /// <summary>
        /// 审核商品调价视图
        /// </summary>
        /// <param name="relationCode">调价关系码</param>
        /// <returns>审核失败或成功信息视图</returns>
        /// <remarks>2013-09-16 周瑜 创建</remarks>
        [Privilege(PrivilegeCode.DS1007101)]
        public ActionResult ProductPriceHistoryAudit(string relationCode)
        {
            var model = DsLevelPriceBo.Instance.GetPriceHistorieListByRelationCode(relationCode);
            return View(model);
        }

        /// <summary>
        /// 审核商品调价
        /// </summary>
        /// <param name="relationCode">调价关系码</param>
        /// <param name="status">状态</param>
        /// <param name="opinion">意见</param>
        /// <returns>审核失败或成功信息</returns>
        /// <remarks>2013-09-16 周瑜 创建</remarks>
        [Privilege(PrivilegeCode.DS1007101)]
        [HttpPost]
        public ActionResult ProductPriceHistoryAudit(string relationCode, int status, string opinion)
        {
            bool isPass = true;
            var model = DsLevelPriceBo.Instance.GetPriceHistorieListByRelationCode(relationCode);

            int u = DsLevelPriceBo.Instance.Update(relationCode, opinion, status,
                                                     AdminAuthenticationBo.Instance.GetAuthenticatedUser().SysNo);
            if (status == (int)ProductStatus.产品价格变更状态.已审)
            {
                isPass = PdPriceBo.Instance.UpdateApplyPriceToPdPrice(model);
            }

            isPass = u > 0 && isPass;


            return Json(new { IsPass = isPass });
        }
        /// <summary>
        /// 统一调价
        /// </summary>
        /// <param name="productSysNoList">申请调价的商品编号数组</param>
        /// <param name="priceSourceType">调价价格类型列表</param>
        /// <param name="dealerSysNo">分销商编号</param>
        /// <returns>返回视图</returns>
        /// <remarks>2016-09-06 罗远康 创建</remarks>
        [Privilege(PrivilegeCode.DS1005301,PrivilegeCode.SY1008101)]
        public ActionResult AddPriceHistory(int[] productSysNoList, int dealerSysNo)
        {
            ViewBag.SalePriceUpper = 0;//上限
            ViewBag.SalePriceLower = 100;//下限
            if (dealerSysNo >= 0)
            {
                var dealerleve= BLL.Distribution.DsDealerLevelBo.Instance.GetDealerLevelByDealerSysNo(dealerSysNo);
                if (dealerleve != null)
                {
                    ViewBag.SalePriceUpper = 100 + dealerleve.SalePriceUpper;
                    ViewBag.SalePriceLower = 100 - dealerleve.SalePriceLower;
                }
            }
            string productSysnos = "";
            for (int i = 0; i < productSysNoList.Length; i++)
            {
                if (i == productSysNoList.Length - 1)
                {
                    productSysnos += productSysNoList[i].ToString();
                }
                else
                {
                    productSysnos += productSysNoList[i].ToString() + ",";
                }
            }

            ViewBag.dealerSysNo = dealerSysNo;
            ViewBag.Products = productSysnos;
            return View();
        }
        #endregion

        #region 分销商商城维护 2013-09-18 郑荣华 创建

        /// <summary>
        /// 分销商维护主页面
        /// </summary>
        /// <param name="id">页码</param>
        /// <param name="filter">分销商查询筛选字段</param>
        /// <returns>分销商维护主页面</returns>
        /// <remarks> 
        /// 2013-09-04 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.DS1006101)]
        public ActionResult DsDealerMall(int? id, ParaDsDealerMallFilter filter)
        {
            if (Request.IsAjaxRequest())
            {
                //列表分页开始
                var model = new PagedList<CBDsDealerMall>();

                var modelRef = new Pager<CBDsDealerMall> { CurrentPage = id ?? 1, PageSize = model.PageSize };
                DsDealerMallBo.Instance.GetDsDealerMallList(ref modelRef, filter);
                model.TotalItemCount = modelRef.TotalRows;
                model.TData = modelRef.Rows;
                model.CurrentPageIndex = modelRef.CurrentPage;
                return PartialView("_AjaxPagerDsDealerMall", model);
            }
            //传递分销商商城类型和分销商
            ViewBag.DsMallType = DsMallTypeBo.Instance.GetDsMallTypeList();
            ViewBag.DsDealer = DsDealerBo.Instance.GetDsDealerList();
            return View();
        }

        /// <summary>
        /// 分销商商城信息新建视图
        /// </summary>
        /// <param></param>
        /// <returns>分销商商城信息新建页面</returns>
        /// <remarks> 
        /// 2013-09-18 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.DS1006201)]
        public ActionResult DsDealerMallAdd()
        {
            //传递分销商商城类型和分销商
            ViewBag.DsMallType = DsMallTypeBo.Instance.GetDsMallTypeList();
            ViewBag.DsDealer = DsDealerBo.Instance.GetDsDealerList();
            ViewBag.Config = new C_DealerMall();
            return View();
        }

        /// <summary>
        /// 分销商商城信息修改页面
        /// </summary>
        /// <param name="sysNo">商城系统编号</param>
        /// <returns>分销商商城信息修改页面</returns>
        /// <remarks> 
        /// 2013-09-18 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.DS1006301)]
        public ActionResult DsDealerMallUpdate(int sysNo)
        {
            //传递分销商商城类型和分销商
            ViewBag.DsMallType = DsMallTypeBo.Instance.GetDsMallTypeList();
            ViewBag.DsDealer = DsDealerBo.Instance.GetDsDealerList();
            var smart = SmartConfigBo.Get<C_DealerMall>(sysNo.ToString());
            ViewBag.Config = smart.Config;
            var model = DsDealerMallBo.Instance.GetDsDealerMall(sysNo);
            return View(model);
        }

        /// <summary>
        /// Ajax新增分销商商城
        /// </summary>
        /// <param name="model">分销商商城实体</param>
        /// <param name="enabledAutoConfirmReceipt">升舱时自动确认收款单</param>
        /// <param name="enabledAddGift">添加赠品</param>
        /// <returns>
        /// 成功result.StatusCode>0,失败 result.StatusCode=0
        /// </returns>
        /// <remarks> 
        /// 2013-09-18 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.DS1006201)]
        public JsonResult AjaxDsDealerMallCreate(DsDealerMall model, bool enabledAutoConfirmReceipt, bool enabledAddGift)
        {
            var result = new Result();
            DsDealerApp appModel = null;
            if (model.DealerAppSysNo > 0)
            {
                var appKeyUseNum = DsDealerMallBo.Instance.GetAppKeyUseNum(model.DealerAppSysNo);
                appModel = DsDealerAppBo.Instance.GetDsDealerApp(model.DealerAppSysNo);
                var maxRelevance = appModel.MaxRelevance;
                //判断appkey使用次数是否已到最大限额
                if (appKeyUseNum >= maxRelevance)
                {
                    result.Message = string.Format("{0}使用数已达最大限额", appModel.AppName);
                    return Json(result);
                }
            }
            model.CreatedBy = CurrentUser.Base.SysNo;
            model.CreatedDate = DateTime.Now;
            model.LastUpdateBy = CurrentUser.Base.SysNo;
            model.LastUpdateDate = DateTime.Now;
            result.StatusCode = DsDealerMallBo.Instance.Create(model);

            //修改appkey使用次数
            if (appModel != null)
            {
                DsDealerAppBo.Instance.UpdateAppKeyUseNum(model.DealerAppSysNo);
            }

            //保存业务数据配置 余勇 2014-08-20
            var smart = SmartConfigBo.Get<C_DealerMall>(result.StatusCode.ToString());
            smart.Config.EnabledAutoConfirmReceipt = enabledAutoConfirmReceipt;
            smart.Config.EnabledAddGift = enabledAddGift;
            SmartConfigBo.Save(smart);

            return Json(result);
        }

        /// <summary>
        /// Ajax修改分销商商城
        /// </summary>
        /// <param name="model">分销商商城实体</param>
        /// <param name="enabledAutoConfirmReceipt">升舱时自动确认收款单</param>
        /// <param name="enabledAddGift">添加赠品</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks> 
        /// 2013-09-18 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.DS1006301)]
        public JsonResult AjaxDsDealerMallUpdate(DsDealerMall model, bool enabledAutoConfirmReceipt, bool enabledAddGift)
        {
            var result = new Result();
            var mallModel = DsDealerMallBo.Instance.GetEntity(model.SysNo);
            //如果修改了appkey关联则须判断appkey使用次数是否已到最大限额
            if (mallModel.DealerAppSysNo != model.DealerAppSysNo && model.DealerAppSysNo > 0)
            {
                var appKeyUseNum = DsDealerMallBo.Instance.GetAppKeyUseNum(model.DealerAppSysNo);
                var appModel = DsDealerAppBo.Instance.GetDsDealerApp(model.DealerAppSysNo);
                var maxRelevance = appModel.MaxRelevance;
                //判断appkey使用次数是否已到最大限额
                if (appKeyUseNum >= maxRelevance)
                {
                    result.Message = string.Format("{0}使用数已达最大限额", appModel.AppName);
                    return Json(result);
                }
            }
            model.LastUpdateBy = CurrentUser.Base.SysNo;
            model.LastUpdateDate = DateTime.Now;

            result.Status = DsDealerMallBo.Instance.Update(model) > 0;
            //当修改appkey关联时，须同时对appkey对应的使用数进行修改 余勇 2014-07-24
            if (mallModel.DealerAppSysNo != model.DealerAppSysNo)
            {
                DsDealerAppBo.Instance.UpdateAppKeyUseNum(mallModel.DealerAppSysNo);
                DsDealerAppBo.Instance.UpdateAppKeyUseNum(model.DealerAppSysNo);
            }

            //保存业务数据配置 余勇 2014-08-20
            var smart = SmartConfigBo.Get<C_DealerMall>(model.SysNo.ToString());
            smart.Config.EnabledAutoConfirmReceipt = enabledAutoConfirmReceipt;
            smart.Config.EnabledAddGift = enabledAddGift;
            SmartConfigBo.Save(smart);

            return Json(result);
        }

        /// <summary>
        /// Ajax修改分销商商城状态
        /// </summary>
        /// <param name="sysNo">分销商商城系统编号</param>
        /// <param name="status">分销商商城状态</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks> 
        /// 2013-09-18 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.DS1006701)]
        public JsonResult AjaxDsDealerMallUpdateStatus(int sysNo, int status)
        {
            var t = (DistributionStatus.分销商商城状态)status;
            return Json(DsDealerMallBo.Instance.UpdateStatus(sysNo, t, CurrentUser.Base.SysNo) > 0, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 通过ajax判断是否已有此分销商商城
        /// </summary>
        /// <param name="ridSysNo">排除的商城系统编号</param>
        /// <param name="mallTypeSysNo">商城类型系统编号</param>
        /// <param name="shopAccount">店铺账号</param>
        /// <returns>
        /// 已有不允许添加返回false,未有允许添加返回true
        /// </returns>
        /// <remarks> 
        /// 2013-09-18 郑荣华 创建
        /// </remarks>
        [Privilege(PrivilegeCode.DS1006201)]
        public JsonResult IsCanAddDsDealerMall(int? ridSysNo, int mallTypeSysNo, string shopAccount)
        {
            var list = DsDealerMallBo.Instance.GetDsDealerMall(ridSysNo, mallTypeSysNo, shopAccount);

            return Json(list == null, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 分销商EAS关联管理

        /// <summary>
        /// 分销商Eas关联列表
        /// </summary>
        /// <returns>视图</returns>
        /// <remarks>2013-10-16 黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.DS100901)]
        public ActionResult DealerEas()
        {
            var list = new List<SelectListItem>();
            var result = DsEasBo.Instance.GetAllMallType();
            if (result != null && result.Count > 0)
            {
                result.Apply(item =>
                {
                    if (item.Status == (int)DistributionStatus.商城类型状态.启用)
                    {
                        list.Add(new SelectListItem()
                        {
                            Text = item.MallName,
                            Value = item.SysNo.ToString()
                        });
                    }

                });
            }
            ViewBag.DsMallType = list;
            return View();
        }

        /// <summary>
        /// 分销商Eas关联列表
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>视图</returns>
        /// <remarks>2013-10-16 黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.DS100902, PrivilegeCode.DS100903)]
        public ActionResult DealerEasDetail(int? sysNo)
        {
            DsEasAssociation model = null;
            if (sysNo.HasValue) model = DsEasBo.Instance.GetEntity(sysNo.Value);
            var allEasAssociation = DsEasBo.Instance.GetAllDsEasAssociation();
            var list = new List<SelectListItem>();
            var dsMallName = string.Empty;
            var result = DsEasBo.Instance.GetAllMall();
            if (result != null && result.Count > 0)
            {
                result.Apply(item =>
                {
                    if (item.Status == (int)DistributionStatus.分销商商城状态.启用)
                    {
                        var mallName =
                            Enum.Parse(typeof(DistributionStatus.商城类型预定义), item.MallTypeSysNo.ToString())
                                .ToString() + " " + item.ShopAccount;
                        if (model != null && model.DealerMallSysNo == item.SysNo) dsMallName = mallName;
                        if (!sysNo.HasValue && !allEasAssociation.Contains(item.SysNo))
                        {
                            list.Add(new SelectListItem()
                            {
                                Text = mallName,
                                Value = item.SysNo.ToString()
                            });
                        }
                    }
                });
            }

            ViewBag.DsMallAll = list;
            if (sysNo.HasValue) ViewBag.DsMallName = dsMallName;
            return View(model);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>分销商EAS分页数据</returns>
        /// <remarks>2013-10-11 黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.DS100901)]
        public ActionResult Query(ParaDsEasFilter filter)
        {
            if (filter.Id == 0) filter.Id = 1;
            var pager = DsEasBo.Instance.Query(filter);
            var list = new PagedList<CBDsEasAssociation>
            {
                TData = pager.Rows,
                PageSize = filter.PageSize,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };
            return PartialView("_DealerEasPager", list);
        }

        /// <summary>
        /// 保存EAS
        /// </summary>
        /// <param name="info">EAS实体</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2013-10-11 黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.DS100902, PrivilegeCode.DS100903)]
        public ActionResult Save(DsEasAssociation info)
        {
            var result = new Result { Status = false };
            try
            {
                if (info.SysNo == 0)
                {
                    if (DsEasBo.Instance.Get(info.DealerMallSysNo, info.SellerNick) == null)
                    {
                        //新增
                        info.CreatedBy = CurrentUser.Base.SysNo;
                        info.CreatedDate = DateTime.Now;
                        info.LastUpdateBy = CurrentUser.Base.SysNo;
                        info.LastUpdateDate = DateTime.Now;
                        DsEasBo.Instance.Insert(info);
                        result.Status = true;
                        SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "分销商EAS关联新增",
                                         LogStatus.系统日志目标类型.EAS, info.SysNo, null, WebUtil.GetUserIp(),
                                         CurrentUser.Base.SysNo);
                    }
                    else
                    {
                        result.Message = string.Format("卖家昵称[{0}]已存在", info.SellerNick);
                    }
                }
                else
                {
                    //修改
                    var model = DsEasBo.Instance.GetEntity(info.SysNo);
                    model.LastUpdateBy = CurrentUser.Base.SysNo;
                    model.LastUpdateDate = DateTime.Now;
                    model.Code = info.Code;
                    model.Status = info.Status;
                    DsEasBo.Instance.Update(model);
                    result.Status = true;
                    SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "分销商EAS关联修改",
                                         LogStatus.系统日志目标类型.EAS, info.SysNo, null, WebUtil.GetUserIp(),
                                         CurrentUser.Base.SysNo);
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "分销商EAS保存" + ex.Message,
                                                         LogStatus.系统日志目标类型.EAS, info.SysNo, ex, WebUtil.GetUserIp(),
                                                         CurrentUser.Base.SysNo);
            }
            return Json(result);
        }

        /// <summary>
        /// 删除分销商EAS关联
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>操作是否成功</returns>
        /// <remarks>2013-10-11  黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.DS100904)]
        public ActionResult Delete(int sysNo)
        {
            var result = new Result { Status = false };
            try
            {
                DsEasBo.Instance.Delete(sysNo);
                result.Status = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "分销商EAS删除" + ex.Message,
                                                        LogStatus.系统日志目标类型.EAS, sysNo, ex, WebUtil.GetUserIp(),
                                                        CurrentUser.Base.SysNo);
            }
            return Json(result);
        }
        #endregion

        #region 禁止升舱商品管理
        /// <summary>
        /// 禁止升舱商品管理
        /// </summary>
        /// <returns>禁止升舱商品管理页面</returns>
        /// <remarks> 
        /// 2014-03-21 余勇 创建
        /// </remarks>
        [Privilege(PrivilegeCode.DS1009101)]
        public ActionResult DsForbidProduct()
        {
            return View();
        }

        /// <summary>
        /// 查询禁止升舱商品列表
        /// </summary>
        /// <param name="id">查询页号</param>
        /// <param name="product">商品名称或编号</param>
        /// <returns>返回查询列表</returns>
        /// <remarks>2014-03-21 余勇 创建</remarks>
        [Privilege(PrivilegeCode.DS1009101)]
        public ActionResult SearchDsForbidProduct(int? id, string product)
        {
            var currentPage = id ?? 1;
            const int pageSize = 10;
            var pager = DsForbidProductBo.Instance.GetPagerList(product, currentPage, pageSize);

            var list = new PagedList<DsForbidProduct>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };
            return PartialView("_DsForbidProduct", list);
        }

        /// <summary>
        /// 通过SysNo删除禁止升舱商品
        /// </summary>
        /// <param name="sysNo">自动分配编号</param>
        /// <returns>影响行数</returns>
        /// <remarks>2013-06-19 余勇 创建</remarks>
        [Privilege(PrivilegeCode.DS1009401)]
        public JsonResult DeleteDsForbidProduct(int sysNo)
        {
            var result = new Result();
            try
            {
                result = DsForbidProductBo.Instance.Delete(sysNo);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "删除禁止升舱商品错误:" + sysNo,
                          LogStatus.系统日志目标类型.分销商商城, 0, null, null, CurrentUser.Base.SysNo);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 插入禁止升舱商品
        /// </summary>
        /// <param name="sysNoList">商品编号数组</param>
        /// <returns>执行结果</returns>
        /// <remarks>2013-06-21 余勇 创建</remarks>
        [Privilege(PrivilegeCode.DS1009201)]
        public JsonResult InsertDsForbidProduct(int[] sysNoList)
        {
            var result = new Result();
            try
            {
                result = DsForbidProductBo.Instance.Create(sysNoList, CurrentUser.Base.SysNo);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "插入禁止升舱商品错误:" + sysNoList.FirstOrDefault(),
                           LogStatus.系统日志目标类型.分销商商城, 0, null, null, CurrentUser.Base.SysNo);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 分销商App维护 2014-05-06 余勇 创建

        /// <summary>
        /// 分销商App维护主页面
        /// </summary>
        /// <returns>分销商App维护主页面</returns>
        /// <remarks> 
        /// 2014-05-06 余勇 创建
        /// </remarks>
        [Privilege(PrivilegeCode.DS1010101)]
        public ActionResult DsDealerApp()
        {
            ViewBag.BaseEntity = new Hyt.Model.Extend.DsDealerAppExtend();
            return View();
        }

        /// <summary>
        /// 获取分销商App列表
        /// </summary>
        /// <param name="id">页索引</param>
        /// <returns>分销商App记录列表</returns>
        /// <remarks>20134-05-04 余勇 创建</remarks>
        [Privilege(PrivilegeCode.DS1010101)]
        public ActionResult DsDealerAppQuery(int? id)
        {
            int pageIndex = id ?? 1;

            var pageFilter = new ParaDsDealerAppFilter();
            pageFilter.PageSize = 10;
            pageFilter.CurrentPage = pageIndex;

            var list = DsDealerAppBo.Instance.GetDealerList(pageFilter);
            var pageList = new PagedList();
            pageList.CurrentPageIndex = pageFilter.CurrentPage;
            pageList.PageSize = pageFilter.PageSize;
            pageList.TotalItemCount = list.TotalRows;
            pageList.Data = list.Rows;

            return PartialView("_DsDealerApp", pageList);
        }


        /// <summary>
        /// Ajax新增分销商App
        /// </summary>
        /// <param name="model">分销商App实体</param>
        /// <returns>
        /// 成功result.StatusCode>0,失败 result.StatusCode=0
        /// </returns>
        /// <remarks> 
        /// 2014-05-06 余勇 创建
        /// </remarks>
        [Privilege(PrivilegeCode.DS1010201)]
        public JsonResult AjaxDsDealerAppCreate(DsDealerApp model)
        {
            model.AppName = WebUtil.StripHTML(model.AppName);
            var result = new Result { Message = "", Status = DsDealerAppBo.Instance.Create(model) > 0 };

            return Json(result);
        }

        /// <summary>
        /// Ajax修改分销商App
        /// </summary>
        /// <param name="model">分销商App实体</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks> 
        /// 2014-05-06 余勇 创建
        /// 2014-07-25 余勇 修改 先获取后修改
        /// </remarks>
        [Privilege(PrivilegeCode.DS1010301)]
        public JsonResult AjaxDsDealerAppUpdate(DsDealerApp model)
        {
            var appModel = DsDealerAppBo.Instance.GetDsDealerApp(model.SysNo);
            var hasRelevance = DsDealerMallBo.Instance.GetAppKeyUseNum(model.SysNo);
            if (model.MaxRelevance < hasRelevance)
            {
                return Json(new Result { Message = "最大可使用次数不能小于已使用数", Status = false });
            }
            appModel.AppKey = model.AppKey;
            appModel.AppName = WebUtil.StripHTML(model.AppName);
            appModel.AppSecret = model.AppSecret;
            //appModel.MallTypeSysNo = model.MallTypeSysNo;
            appModel.MaxRelevance = model.MaxRelevance;
            appModel.Status = model.Status;
            appModel.Extend = model.Extend;
            var result = new Result { Message = "", Status = DsDealerAppBo.Instance.Update(appModel) > 0 };
            return Json(result);
        }

        /// <summary>
        /// Ajax修改分销商App状态
        /// </summary>
        /// <param name="sysNo">分销商App系统编号</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks> 
        /// 2014-05-06 余勇 创建
        /// </remarks>
        [Privilege(PrivilegeCode.DS1010701)]
        public JsonResult AjaxDsDealerAppUpdateStatus(int sysNo)
        {
            var result = new Result { Message = "", Status = DsDealerAppBo.Instance.UpdateStatus(sysNo) > 0 };
            return Json(result);
        }
        /// <summary>
        /// Ajax修改分销商App
        /// </summary>
        /// <param name="model">分销商App实体</param>
        /// <returns>成功返回true,失败返回false</returns>
        /// <remarks> 
        /// 2014-05-06 余勇 创建
        /// </remarks>
        [Privilege(PrivilegeCode.DS1010301)]
        public JsonResult AjaxDsDealerAppIsRepeat(DsDealerApp model)
        {
            var result = new Result();
            if (DsDealerAppBo.Instance.GetDsDealerAppList(model.SysNo, model.AppKey).Any())
            {
                result = new Result { Message = "", Status = true };
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 通过分销商城类型系统编号获取AppKey列表
        /// </summary>
        /// <param name="mallType">mallType</param>
        /// <returns>分销商App信息列表</returns>
        /// <remarks> 
        /// 2014-07-24 余勇 创建 
        /// </remarks>   
        [Privilege(PrivilegeCode.DS1006201)]
        public JsonResult GetListByMallType(int mallType)
        {
            var list = DsDealerAppBo.Instance.GetListByMallType(mallType).Select(i => new
            {
                Text = i.AppName,
                Value = i.SysNo
            });
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #endregion

        /// <summary>
        /// 分销商升舱错误日志查询
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns>view</returns>
        /// <remarks>2014-03-31 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.DS1009501)]
        public ActionResult DealerLogs(ParaDsDealerLogFilter filter)
        {
            if (Request.IsAjaxRequest())
            {
                var result = DsDealerLogBo.Instance.GetPagerList(filter);
                var list = new PagedList<DsDealerLog>
                {
                    TData = result.Rows,
                    CurrentPageIndex = result.CurrentPage,
                    TotalItemCount = result.TotalRows
                };
                return PartialView("_DealerLogs", list);
            }

            return View();
        }

        /// <summary>
        /// 更改升舱日志状态
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>返回操作行</returns>
        /// <remarks>2014-03-31 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.DS1009501)]
        public ActionResult ChangeDealerLogStatus(int sysNo)
        {
            var result = new Result { Message = "", Status = false };
            try
            {
                result = DsDealerLogBo.Instance.ChangeDealerLogStatus(sysNo);
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "分销商升舱错误日志状态",
                                         LogStatus.系统日志目标类型.分销商升舱错误日志, sysNo, null, null, CurrentUser.Base.SysNo);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "分销商升舱错误日志状态错误:" + ex.Message,
                                           LogStatus.系统日志目标类型.分销商升舱错误日志, sysNo, ex, null, CurrentUser.Base.SysNo);
            }
            return Json(result);
        }

        #region 分销商用户
        /// <summary>
        /// 分销商用户
        /// </summary>
        /// <param name="dealersysno">分销商编号</param>
        /// <returns></returns>
        /// <remarks>2014-06-05 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.DS1003001)]
        public ActionResult DsUserList(int? dealersysno, string dealername)
        {
            List<DsUser> lst = new List<DsUser>();
            if (CurrentUser.IsBindDealer)
            {
                dealersysno = CurrentUser.Dealer.SysNo;
                dealername = CurrentUser.Dealer.DealerName;
                lst = Hyt.BLL.Distribution.DsUserBo.Instance.GetListByDealerSysNo(dealersysno.Value);

                ViewBag.DealerSysno = dealersysno;//分销商编号
                ViewBag.Dealername = dealername;//分销商名称
                ViewBag.dsSysNo = dealersysno.Value;
            }
            else
            {
                ViewBag.dsSysNo = 0;
                lst = Hyt.BLL.Distribution.DsUserBo.Instance.GetListByDealerSysNo();
            }


            return View("DsUserList", lst);
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="sysNo">用户编号</param>
        /// <returns></returns>
        // <remarks>2014-06-05 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.DS1003001)]
        public ActionResult GetDsUser(int sysNo)
        {
            var model = Hyt.BLL.Distribution.DsUserBo.Instance.GetEntity(sysNo);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 分销商用户
        /// </summary>
        /// <param name="dealersysno">分销商编号</param>
        /// <returns></returns>
        /// <remarks>2014-06-05 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.DS1003001)]
        public ActionResult _DsUserList(int dealersysno)
        {
            var lst = Hyt.BLL.Distribution.DsUserBo.Instance.GetListByDealerSysNo(dealersysno);
            return PartialView("_DsUserList", lst);
        }

        /// <summary>
        /// 判断账号信息是否存在
        /// </summary>
        /// <param name="dealersysno">分销商编号</param>
        /// <param name="account">账号编号</param>
        /// <param name="sysNO">当前分销帐号</param>
        /// <returns></returns>
        /// <remarks>2014-06-05 朱成果 创建</remarks> 
        [Privilege(PrivilegeCode.DS1003001)]
        public ActionResult NotExistDsUser(int dealersysno, string account, int sysNO)
        {
            var model = Hyt.BLL.Distribution.DsUserBo.Instance.GetEntity(account);
            bool flg = model == null || model.SysNo == sysNO;
            return Json(flg, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 启用禁用帐号
        /// </summary>
        /// <param name="sysNo">用户编号</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        /// <remarks>2014-06-05 朱成果 创建</remarks> 
        [Privilege(PrivilegeCode.DS1003001)]
        public ActionResult Disabled(int sysNo, int status)
        {
            Result res = new Result() { Status = true };
            var model = Hyt.BLL.Distribution.DsUserBo.Instance.GetEntity(sysNo);
            if (model != null)
            {
                model.Status = status;
                model.LastUpdateBy = CurrentUser.Base.SysNo;
                model.LastUpdateDate = DateTime.Now;
                Hyt.BLL.Distribution.DsUserBo.Instance.Update(model);
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存添加账户信息
        /// </summary>
        /// <param name="model">账户信息</param>
        /// <returns></returns>
        /// <remarks>2014-06-05 朱成果 创建</remarks> 
        [Privilege(PrivilegeCode.DS1003001)]
        public ActionResult SaveDsUser(DsUser model)
        {
            string strPassword = "123456";
            Result res = new Result() { Status = true };
            var oldmodel = Hyt.BLL.Distribution.DsUserBo.Instance.GetEntity(model.Account);//判断是否存在
            if (oldmodel != null && oldmodel.SysNo != model.SysNo)
            {
                res.Status = false;
                res.Message = "帐号已经存在";
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            try
            {
                if (model.SysNo > 0)
                {
                    var entity = Hyt.BLL.Distribution.DsUserBo.Instance.GetEntity(model.SysNo);
                    entity.Account = model.Account;
                    entity.Name = model.Name;
                    entity.Status = model.Status;
                    model.LastUpdateBy = CurrentUser.Base.SysNo;
                    model.LastUpdateDate = DateTime.Now;
                    Hyt.BLL.Distribution.DsUserBo.Instance.Update(entity);
                    DeleteCache.AccessToken(model.SysNo);
                }
                else
                {
                    model.Password = Hyt.Util.EncryptionUtil.EncryptWithMd5AndSalt(strPassword);
                    model.CreatedBy = CurrentUser.Base.SysNo;
                    model.CreatedDate = DateTime.Now;
                    model.LastUpdateBy = CurrentUser.Base.SysNo;
                    model.LastUpdateDate = DateTime.Now;
                    Hyt.BLL.Distribution.DsUserBo.Instance.Insert(model);
                }
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Status = false;
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "分销商账号错误",
                                           LogStatus.系统日志目标类型.分销商, model.DealerSysNo, ex, null, CurrentUser.Base.SysNo);
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="sysNo">用户信息</param>
        /// <returns></returns>
        /// <remarks>2014-06-05 朱成果 创建</remarks> 
        [Privilege(PrivilegeCode.DS1003001)]
        public ActionResult ResetPwd(int sysNo)
        {
            Result res = new Result() { Status = true };
            string pwd = Hyt.Util.WebUtil.Number(6, false);
            var model = Hyt.BLL.Distribution.DsUserBo.Instance.GetEntity(sysNo);
            if (model != null)
            {
                model.Password = Hyt.Util.EncryptionUtil.EncryptWithMd5AndSalt(pwd);
                Hyt.BLL.Distribution.DsUserBo.Instance.Update(model);
                res.Message = "密码重置成功.<br/>账号:" + model.Account + "<br/>新密码:" + pwd;
            }
            else
            {
                res.Status = false;
                res.Message = "帐号不存在";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 升舱商品关系维护
        /// <summary>
        /// 升舱商品关系维护分页查询
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        /// <remarks>2014-10-11 谭显锋 创建</remarks>
        [Privilege(PrivilegeCode.Ds1009502)]
        public ActionResult DsProductRelation(ParaDsProductRelationFilter filter)
        {
            if (filter.Id == 0) filter.Id = 1;
            var pager = DsProductRelationBo.Instance.Query(filter);
            var list = new PagedList<CBDsProductRelation>
            {
                TData = pager.Rows,
                PageSize = filter.PageSize,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };
            if (Request.IsAjaxRequest())
            {
                return PartialView("_AjaxDsProductRelationList", list);
            }
            return PartialView("DsProductRelationPager", list);
        }


        /// <summary>
        /// 通过SysNo删除升舱关系维护数据
        /// </summary>
        /// <param name="sysNo">自动分配编号</param>
        /// <returns>影响行数</returns>
        /// <remarks>2014-10-11 谭显锋 创建</remarks>
        [Privilege(PrivilegeCode.Ds1009502)]
        public JsonResult DeleteDsProductRelation(int sysNo)
        {
            var result = new Result();
            try
            {
                result = DsProductRelationBo.Instance.Delete(sysNo);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "删除升舱关系维护:" + sysNo,
                          LogStatus.系统日志目标类型.分销商商城, 0, null, null, CurrentUser.Base.SysNo);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 商城地区关联管理

        /// <summary>
        /// 商城地区关联页面
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        /// <remarks>缪竞华 2014-10-15 创建</remarks>
        [Privilege(PrivilegeCode.DS1011101)]
        public ActionResult DsMallAreaRelation(ParaDsMallAreaRelationFilter filter)
        {
            if (filter.Id == 0) filter.Id = 1;
            var pager = DsMallAreaRelationBo.Instance.Query(filter);
            var list = new PagedList<CBDsMallAreaRelation>
            {
                TData = pager.Rows,
                PageSize = filter.PageSize,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };
            if (Request.IsAjaxRequest())
            {
                return PartialView("_AjaxPagerDsMallAreaRelation", list);
            }
            return View(list);
        }

        /// <summary>
        /// 通过SysNo删除商城地区关联数据
        /// </summary>
        /// <param name="sysNo">系统编码</param>
        /// <returns></returns>
        /// <remarks>缪竞华 2014-10-15 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.DS1011401)]
        public JsonResult DeleteDsMallAreaRelation(int sysNo)
        {
            var result = new Result() { Status = false };
            try
            {
                result = DsMallAreaRelationBo.Instance.Delete(sysNo);
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "删除商城地区关联维护",
                                           LogStatus.系统日志目标类型.分销商商城地区关联, sysNo, null, null, CurrentUser.Base.SysNo);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "删除商城地区关联维护错误:" + ex.Message,
                          LogStatus.系统日志目标类型.分销商商城地区关联, sysNo, ex, null, CurrentUser.Base.SysNo);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 获取分销商商品详细信息列表
        /// <summary>
        /// 获取商品详细信息列表
        /// </summary>
        /// <param name="pager">商品详细信息查询列表</param>
        /// <param name="dealerMallSysNo">分销商商城系统编号</param>
        /// <param name="condition">动态条件，CBPdProductDetail里不包含的条件</param>
        /// <returns></returns>
        /// <remarks>2015-12-10 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.DS1011501)]
        public ActionResult DealerMallProductList(ParaProductFilter productDetail)
        {
            //当前用户对应商城SysNo
            CBDsDealerMall dmModel = DsDealerMallBo.Instance.GetDsDealerMallByDealerSysNo(CurrentUser.Dealer.SysNo);
            int dealerMallSysNo = dmModel.SysNo;
            int pageIndex = productDetail.id ?? 1;
            int status = 0;

            int.TryParse(Request.Params["status"], out status);
            string name = Request.Params["name"];
            string selector = Request.Params["selector"];
            if (!string.IsNullOrEmpty(selector) && selector == "selector")
            {
                status = (int)ProductStatus.商品属性分组状态.启用;
            }
            status = -1;
            var list = new PagedList<CBPdProductDetail>();
            var pager = new Pager<CBPdProductDetail>
            {
                CurrentPage = pageIndex,
                PageSize = list.PageSize
            };

            if (Request.IsAjaxRequest())
            {
                BLL.Distribution.DsProductAssociationBo.Instance.GetDealerMallProductList(ref pager, dealerMallSysNo, productDetail);

                return PartialView("_AjaxDealerMallProductPager", pager.Map());
            }

            return View(list);
        }
        /// <summary>
        /// 保存分销商商品
        /// </summary>
        /// <param name="model"></param>
        /// <returns>字符串</returns>
        /// <remarks>2015-08-26 王耀发 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.DS1011501)]
        public ActionResult SaveDealerMallProduct()
        {
            //当前用户对应商城SysNo
            CBDsDealerMall dmModel = DsDealerMallBo.Instance.GetDsDealerMallByDealerSysNo(CurrentUser.Dealer.SysNo);
            int dealerMallSysNo = dmModel.SysNo;
            string ProductSysNoList = this.Request["ProductSysNoList"];
            string[] ProductSysNoArray = ProductSysNoList.Split(',');
            Result result = new Result();
            try
            {
                for (int i = 0; i < ProductSysNoArray.Length; i++)
                {
                    DsProductAssociation model = new DsProductAssociation();
                    model.DealerMallSysNo = dealerMallSysNo;
                    model.HytProductSysNo = int.Parse(ProductSysNoArray[i]);
                    model.Status = 0;
                    DsProductAssociationBo.Instance.Insert(model);
                    result.Status = true;
                }
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
        /// 更新商品状态
        /// </summary>
        /// <param></param>
        /// <returns>返回操作结果</returns>
        /// <remarks>2015-08-26 王耀发 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.DS1011501)]
        public ActionResult UpdateDealerMallProductStatus()
        {
            int Status = 0, SysNo = 0;
            int.TryParse(Request.Params["SysNo"], out SysNo);
            int.TryParse(Request.Params["Status"], out Status);
            Result result = new Result();
            try
            {
                BLL.Distribution.DsProductAssociationBo.Instance.UpdateDealerMallProductStatus(SysNo, Status);
                result.Status = true;
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
            }
            return Json(result);
        }

        public ActionResult IsUpdateDealerMallProductStatus()
        {
            int Status = 0, SysNo = 0;
            int.TryParse(Request.Params["SysNo"], out SysNo);
            int.TryParse(Request.Params["Status"], out Status);
            string Price = this.Request["Price"];
            string SalesPrice = this.Request["SalesPrice"];
            string DealerSysNo = this.Request["DealerSysNo"];
            var result = new Result();
            Decimal typePrice = 0;
            if (!Decimal.TryParse(Price, out typePrice))
            {
                result.Message = "价格须为数值";
                result.Status = false;
                return Json(result);
            }
            DsDealerLevel dlModel = DsDealerLevelBo.Instance.GetLevelByDealerSysNo(int.Parse(DealerSysNo));
            decimal SalePriceUpper = dlModel.SalePriceUpper;
            decimal SalePriceLower = dlModel.SalePriceLower;

            decimal MaxPrice = decimal.Parse(SalesPrice) + decimal.Parse(SalesPrice) * (SalePriceUpper / 100);
            decimal MinPrice = decimal.Parse(SalesPrice) - decimal.Parse(SalesPrice) * (SalePriceLower / 100);

            if (decimal.Parse(Price) > MaxPrice || decimal.Parse(Price) < MinPrice)
            {
                result.Status = false;
                result.Message = "调价范围在" + decimal.Round(MinPrice, 2) + "~" + decimal.Round(MaxPrice, 2) + "之间";
            }
            else
            {
                result.Status = true;
            }
            return Json(result);
        }
        /// <summary>
        /// 删除关系商品
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        /// <remarks>2015-08-26 王耀发 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.DS1011501)]
        public ActionResult DeleteDealerMallProduct()
        {
            int SysNo = 0;
            int.TryParse(Request.Params["SysNo"], out SysNo);
            var result = new Result { Status = false };
            try
            {
                DsProductAssociationBo.Instance.Delete(SysNo);
                result.Status = true;
                result.Message = "删除成功！";
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
            }
            return Json(result);
        }

        #endregion

        #region 分销商关系图

        [Privilege(PrivilegeCode.DS1003004)]
        public ActionResult DsDealerCustomerRelationSchemaList()
        {
            return View();
        }
        /// <summary>
        /// 当前分销商属性图
        /// </summary>
        /// <returns></returns>
        /// 2016-1-20 王耀发 创建
        [Privilege(PrivilegeCode.DS1003004)]
        public JsonResult GetDealerTreeList()
        {
            string Type; //ZD(总代理),D(代理),F(分销商)
            int TypeSysNo; //传入的对应类型系统编号
            if (!CurrentUser.IsBindAllDealer)
            {
                //判断是否绑定分销商
                if (!CurrentUser.IsBindDealer)
                {
                    Type = "D";
                    TypeSysNo = CurrentUser.Base.SysNo;
                }
                else
                {
                    Type = "F";
                    TypeSysNo = CurrentUser.Dealer.SysNo;
                }
            }
            else
            {
                Type = "ZD";
                TypeSysNo = 0;
            }

            var list = DsDealerBo.Instance.GetDealerTreeList(Type, TypeSysNo);
            //通过Linq生产zTree节点结果集
            var nodes = from c in list
                        select new
                        {
                            id = c.SysNo
                            ,
                            name = c.DealerName
                            ,
                            title = c.DealerName
                            ,
                            open = false
                            ,
                            pId = c.PSysNo
                            ,
                            oSysNo = c.oSysNo
                            ,
                        };

            //返回json数组
            return Json(nodes.ToList(), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 查询当前分销商对应图谱
        /// </summary>     
        /// <returns>用户信息列表</returns>
        /// <remarks>
        /// 2016-1-14 王耀发 创建
        /// 2016-4-29 刘伟豪 添加关键字搜索
        /// </remarks>
        [Privilege(PrivilegeCode.DS1003004)]
        public JsonResult GetDealerCustomerTreeList()
        {
            int DealerSysNo = int.Parse(this.Request["DealerSysNo"]);
            var keyword = this.Request["keyword"] ?? "";

            var list = CrCustomerBo.Instance.GetCrCustomerListByDealerSyNo(DealerSysNo, keyword);

            //通过Linq生产zTree节点结果集
            var nodes = from c in list
                        select new
                        {
                            id = c.SysNo
                            ,
                            name = c.NickName == null || c.NickName == "未关注" ? (c.Name == null ? c.Account : c.Name) : c.NickName
                            ,
                            title = c.NickName == null || c.NickName == "未关注" ? (c.Name == null ? c.Account : c.Name) : c.NickName
                            ,
                            open = false
                            ,
                            pId = c.PSysNo
                            ,
                        };

            //返回json数组
            return Json(nodes.ToList(), JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 获得分销商信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns>2015-09-19 王耀发 创建</returns>
        [Privilege(PrivilegeCode.DS1002101)]
        public JsonResult GetModel()
        {
            int SysNo = int.Parse(this.Request["DealerSysNo"]);
            CBDsDealer model = Hyt.BLL.Distribution.DsDealerBo.Instance.GetCBDsDealer(SysNo);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 经销返利记录
        /// <summary>
        /// 返利记录列表查询
        /// </summary>
        /// <returns>返利记录列表</returns>
        /// <remarks>2015-12-21 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.DS1003002)]
        public ActionResult DealerRebatesRecordList()
        {
            return View();
        }

        /// <summary>
        /// 分页获取返利记录
        /// </summary>
        /// <param name="filter">传入的实体参数</param>
        /// <returns>返利记录列表</returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.DS1003002)]
        public ActionResult DoDealerRebatesRecordQuery(ParaDealerRebatesRecordFilter filter)
        {
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
           
            filter.PageSize = 10;
            var pager = DsDealerRebatesRecordBo.Instance.GetDsDealerRebatesRecordList(filter);
            var list = new PagedList<CBDsDealerRebatesRecord>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };
            return PartialView("_DealerRebatesRecordPager", list);
        }

        /// <summary>
        /// 返利记录列表查询
        /// </summary>
        /// <returns>返利记录列表</returns>
        /// <remarks>2015-12-21 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.DS1003002)]
        public ActionResult DealerRebatesRecordView(int sysno)
        {
            ViewData["OrderSysNo"] = sysno;
            return View();
        }
        [Privilege(PrivilegeCode.DS1003002)]
        public ActionResult DoDealerRebatesRecordViewQuery(ParaDealerRebatesRecordFilter filter)
        {
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

            filter.SelectedAgentSysNo = filter.SelectedAgentSysNo <= 0 ? -1 : filter.SelectedAgentSysNo;

            filter.PageSize = 5;
            var pager = DsDealerRebatesRecordBo.Instance.GetDsDealerRebatesRecordList(filter);
            var list = new PagedList<CBDsDealerRebatesRecord>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                PageSize = 100,
                TotalItemCount = pager.TotalRows
            };
            return PartialView("_DealerRebatesRecordViewPager", list);
        }
        /// <summary>
        /// 同步总部已上架商品到分销商商品表中
        /// 王耀发 2016-1-5 创建
        /// </summary>
        /// <param name="DealerSysNo">分销商系统编号</param>
        /// <param name="CreatedBy">创建用户系统编号</param>
        /// <returns></returns>
        [Privilege(PrivilegeCode.DS1011503)]
        public ActionResult ProCreateSpecialPrice(int DealerSysNo)
        {
            Result result = new Result();
            int affectRows = DsSpecialPriceBo.Instance.ProCreateSpecialPrice(DealerSysNo, CurrentUser.Base.SysNo);
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
        /// 批量下架
        /// 王耀发 2016-1-4 创建
        /// </summary>
        /// <returns></returns>
        [Privilege(PrivilegeCode.DS1011504)]
        public JsonResult UpdatePLSpecialPriceProductStatus()
        {
            string productSysNoList = Request.Params["productSysNoList"];
            string strstatus = Request.Params["status"];
            int DealerSysNo = int.Parse(this.Request["DealerSysNo"]);
            string[] ProductSysNo = productSysNoList.Split(',');
            Result result = new Result();
            try
            {
                if (!string.IsNullOrEmpty(productSysNoList))
                {
                    for (int i = 0; i < ProductSysNo.Length; i++)
                    {
                        int status = 0, sysno = 0;
                        int.TryParse(strstatus, out status);
                        int.TryParse(ProductSysNo[i], out sysno);

                        //转化为int防止注入
                        if (sysno > 0 && !string.IsNullOrEmpty(Request.Params["status"]))
                        {
                            //如果是总部直营店不做处理
                            if (DealerSysNo == 347 || DealerSysNo == 336 || DealerSysNo == 44 || DealerSysNo == 14 || DealerSysNo == 0)
                            { }
                            else
                            {
                                result = DsSpecialPriceBo.Instance.UpdateSSPriceStatus(status, sysno);
                            }
                           
                        }
                    }
                }
                else
                {
                    if (Convert.ToInt32(strstatus) == 0)
                    {
                         //如果是总部直营店不做处理
                        if (DealerSysNo == 347 || DealerSysNo == 336 || DealerSysNo == 44 || DealerSysNo == 14 || DealerSysNo == 0)
                        { }
                        else
                        {
                            int Status = Hyt.Model.WorkflowStatus.ProductStatus.商品状态.下架.GetHashCode();
                            DsSpecialPriceBo.Instance.UpdateAllPriceStatus(DealerSysNo, Status);
                        }
                    }
                }
                result.Status = true;
                result.Message = "下架成功";
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
            }
            return Json(result);
        }
        /// <summary>
        /// 创建二维码
        /// 王耀发 2016-2-24 创建
        /// 罗勤尧修改 2017-5-17 修改
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns></returns>
        [Privilege(PrivilegeCode.DS1011503, PrivilegeCode.DS1011504)]
        public ActionResult CreateDealerProductQRCode(string dealerSysNo, string sysNos)
        {
            ViewBag.dealerSysNo = dealerSysNo;
            ViewBag.sysNos = sysNos.Split(',');
            //string [] syaNolist=sysNos.Split(',');
            ////要生成二维码的字符串
            //string QRCodeTxet = "http://";
            //string DomainName = Hyt.BLL.Distribution.DsDealerBo.Instance.GetDsDealer(int.Parse(dealerSysNo)).DomainName;
            //var generalConfig = Hyt.BLL.Config.Config.Instance.GetGeneralConfig();
            //if (!string.IsNullOrEmpty(DomainName))
            //{
            //    QRCodeTxet += DomainName + "/" + dealerSysNo + "/product/details/";
            //}
            //else
            //{
            //    QRCodeTxet += generalConfig.Domain.Replace("http://", "") + "/" + dealerSysNo + "/product/details/";
            //}

            //foreach (var item in syaNolist)
            //{
               
            //    int ProductSysNo = Hyt.BLL.Distribution.DsSpecialPriceBo.Instance.GetEntityBySysNo(int.Parse(item)).ProductSysNo;
            //    CBSimplePdProduct Entity = Hyt.BLL.Web.PdProductBo.Instance.GetProduct(ProductSysNo);
            //    decimal salesprice = 0;
            //    foreach (var price in Entity.Prices)
            //    {
            //        if (price.PriceSource == 90)
            //        {
            //            salesprice = price.Price;
            //        }
            //    }

            //    QRCodeTxet += ProductSysNo;
            //    CreateQR(QRCodeTxet);

            //}
            return View("DealerProductQRCodeCreate");
        }


        /// <summary>
        /// 创建所有二维码
        /// 罗勤尧修改 2017-5-17 创建
        /// </summary>
        /// <param name="sysNos"></param>
        /// <returns></returns>
         [HttpPost]
        [Privilege(PrivilegeCode.DS1011503, PrivilegeCode.DS1011504)]
        public ActionResult CreateDealerProductQRCodeAll(string dealerSysNo, string sysNos)
        {
            ViewBag.dealerSysNo = dealerSysNo;
            ViewBag.sysNos = sysNos.Split(',');
            //string [] syaNolist=sysNos.Split(',');
            ////要生成二维码的字符串
            //string QRCodeTxet = "http://";
            //string DomainName = Hyt.BLL.Distribution.DsDealerBo.Instance.GetDsDealer(int.Parse(dealerSysNo)).DomainName;
            //var generalConfig = Hyt.BLL.Config.Config.Instance.GetGeneralConfig();
            //if (!string.IsNullOrEmpty(DomainName))
            //{
            //    QRCodeTxet += DomainName + "/" + dealerSysNo + "/product/details/";
            //}
            //else
            //{
            //    QRCodeTxet += generalConfig.Domain.Replace("http://", "") + "/" + dealerSysNo + "/product/details/";
            //}

            //foreach (var item in syaNolist)
            //{

            //    int ProductSysNo = Hyt.BLL.Distribution.DsSpecialPriceBo.Instance.GetEntityBySysNo(int.Parse(item)).ProductSysNo;
            //    CBSimplePdProduct Entity = Hyt.BLL.Web.PdProductBo.Instance.GetProduct(ProductSysNo);
            //    decimal salesprice = 0;
            //    foreach (var price in Entity.Prices)
            //    {
            //        if (price.PriceSource == 90)
            //        {
            //            salesprice = price.Price;
            //        }
            //    }

            //    QRCodeTxet += ProductSysNo;
            //    CreateQR(QRCodeTxet);

            //}
            return View("DealerProductQRCodeCreate");
        }
        #endregion
        /// <summary>
        /// 创建二维码
        /// 罗勤尧创建 2017-5-17 创建
        /// </summary>
        /// <param name="url">参数</param>
        /// <returns></returns>
        [Privilege(PrivilegeCode.DS1011503, PrivilegeCode.DS1011504)]
        public FileResult CreateQR(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                System.Drawing.Bitmap bmp;

                //创建二维码生成类  
                QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
                //设置编码模式  
                qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                //设置编码测量度  
                //qrCodeEncoder.QRCodeScale = 3;
                //设置编码版本  
                qrCodeEncoder.QRCodeVersion = 8;
                //设置编码错误纠正  
                qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;

                //生成二维码图片  
                bmp = qrCodeEncoder.Encode(url, System.Text.Encoding.UTF8);

                if (bmp != null)
                {
                    //贴图
                    //System.Drawing.Image img = System.Drawing.Image.FromFile(Server.MapPath("~/Content/164_2_80.png"));
                    //System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmp);
                    //g.DrawImage(img, bmp.Width / 2 - img.Width / 2, bmp.Width / 2 - img.Width / 2, img.Width, img.Height);

                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);

                    byte[] bytes = ms.GetBuffer();
                    ms.Close();
                    return File(bytes, @"image/bmp");
                }
            }

            return File("", @"image/bmp");
        }

        #region 微信公众号配置 刘伟豪 2016-1-8 16:05
        /// <summary>
        /// 微信公众号菜单配置
        /// </summary>
        /// <remarks>2016-1-8 16:12 刘伟豪 创建</remarks>
        public ActionResult WeiXinMenuSetup()
        {
            return null;
        }
        #endregion

        #region 经销商微信菜单 王耀发 2016-1-11 16:05
        /// <summary>
        /// 获取对应微信菜单
        /// 2016-1-11 王耀发 创建
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [Privilege(PrivilegeCode.DS1017101)]
        public ActionResult MkCustomizeMenuList(int? id, ParaMkCustomizeMenuFilter filter)
        {
            if (Request.IsAjaxRequest())
            {
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
                filter.CurrentPage = id ?? 1;
                var list = MkCustomizeMenuBo.Instance.GetMkCustomizeMenuList(filter);
                return PartialView("_AjaxPagerMkCustomizeMenuList", list);
            }
            InitPageViewData(false);
            return View();
        }

        /// <summary>
        /// 修改微信菜单
        /// </summary>
        /// <param name="id">系统编号</param>
        /// <returns>视图</returns>
        /// <remarks>2016-1-11 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.DS1017102, PrivilegeCode.DS1017103)]
        public ActionResult MkCustomizeMenuCreate(int? id)
        {
            MkCustomizeMenu model = new MkCustomizeMenu();
            if (id.HasValue)
            {
                model = Hyt.BLL.Weixin.MkCustomizeMenuBo.Instance.GetEntity(id.Value);
            }
            return View(model);
        }

        /// <summary>
        /// 同步信营全球购经销商的菜单，只能同步两级菜单
        /// 王耀发 2016-2-3 创建
        /// </summary>
        /// <param name="DealerSysNo">被同步的经销商系统编号</param>
        /// <returns></returns>
        [Privilege(PrivilegeCode.DS1017102, PrivilegeCode.DS1017103)]
        public ActionResult ProCreateMkCustomizeMenu(int DealerSysNo)
        {
            Result result = new Result();
            int affectRows = MkCustomizeMenuBo.Instance.ProCreateMkCustomizeMenu(DealerSysNo);
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
        /// 新增子级菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>2016-1-11 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.DS1017106, PrivilegeCode.DS1017107)]
        public ActionResult MkCustomizeSubMenuCreate(int? id)
        {
            ViewBag.Pid = this.Request["Pid"];
            string pid = this.Request["Pid"];
            MkCustomizeMenu model = new MkCustomizeMenu();
            if (id.HasValue)
            {
                model = Hyt.BLL.Weixin.MkCustomizeMenuBo.Instance.GetEntity(id.Value);
            }
            return View(model);
        }

        /// <summary>
        /// 保存微信菜单
        /// </summary>
        /// <param model>模型</param>
        /// <remarks>2016-1-11 王耀发 创建</remarks>       
        [Privilege(PrivilegeCode.DS1017102, PrivilegeCode.DS1017103)]
        public JsonResult SaveMkCustomizeMenu(MkCustomizeMenu model)
        {
            var result = new Result();
            try
            {
                result = MkCustomizeMenuBo.Instance.SaveMkCustomizeMenu(model, CurrentUser.Base);
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存微信子级菜单
        /// </summary>
        /// <param model>模型</param>
        /// <remarks>2016-1-11 王耀发 创建</remarks>       
        [Privilege(PrivilegeCode.DS1017106, PrivilegeCode.DS1017107)]
        public JsonResult SaveMkCustomizeSubMenu(MkCustomizeMenu model)
        {
            var result = new Result();
            try
            {
                MkCustomizeMenu Parentmodel = Hyt.BLL.Weixin.MkCustomizeMenuBo.Instance.GetEntity(model.Pid);
                model.DealerSysNo = Parentmodel.DealerSysNo;
                result = MkCustomizeMenuBo.Instance.SaveMkCustomizeMenu(model, CurrentUser.Base);
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 通过ajax判断是否已有此分销商
        /// </summary>
        /// <param name="DomainName">域名</param>
        /// <returns></returns>
        /// 2015-12-22 王耀发 创建
        [Privilege(PrivilegeCode.DS1017102, PrivilegeCode.DS1017103)]
        public JsonResult IsCanAddMkCustomizeMenu(string dealerSysNo)
        {
            var result = new Result();
            try
            {
                int rows = MkCustomizeMenuBo.Instance.GetMkCustomizeMenuCountInDealerParent(0, int.Parse(dealerSysNo));
                if (rows > 3)
                {
                    result.Status = false;
                    result.Message = "该分销商添加的一级菜单已超过三个，不能再添加！";
                }
                else
                {
                    result.Status = true;
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 删除分销商菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 2016-1-12 王耀发 创建
        [Privilege(PrivilegeCode.DS1017104)]
        public JsonResult DeleteMkCustomizeMenu(int id)
        {
            var result = new Result();
            try
            {
                if (id > 0)
                {
                    MkCustomizeMenuBo.Instance.DeleteMkCustomizeMenu(id);
                    result.Status = true;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取对应微信子菜单列表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        /// 2016-1-12 王耀发 创建
        [Privilege(PrivilegeCode.DS1017105)]
        public ActionResult MkCustomizeSubMenuList(int? id, ParaMkCustomizeMenuFilter filter)
        {
            ViewBag.Pid = this.Request["Pid"];
            string Pid = this.Request["Pid"];
            if (Request.IsAjaxRequest())
            {
                filter.CurrentPage = id ?? 1;
                var list = MkCustomizeMenuBo.Instance.GetMkCustomizeSubMenuList(filter);
                return PartialView("_AjaxPagerMkCustomizeSubMenuList", list);
            }
            InitPageViewData(false);
            return View();
        }
        #endregion

        #region 分销商账号管理 陈海裕 2016-3-24
        [Privilege(PrivilegeCode.DS1002101)]
        public ActionResult DsDealerUserList()
        {
            return View();
        }

        [Privilege(PrivilegeCode.DS1002101)]
        public ActionResult DoDealerUserQuery(ParaSyUserFilter filter)
        {
            if (!string.IsNullOrWhiteSpace(filter.Keyword))
            {
                if (VHelper.ValidatorRule(new Rule_Number(filter.Keyword)).IsPass)
                    filter.MobilePhoneNumber = filter.Keyword;
                else
                    filter.Account = filter.Keyword;
            }

            if (!CurrentUser.IsBindAllDealer)
            {
                filter.CreatedBy = CurrentUser.Base.SysNo;
            }
            else if (CurrentUser.IsBindDealer)
            {
                filter.Account = CurrentUser.Base.Account;
            }

            var pager = SyUserBo.Instance.GetSyUser(filter);
            var list = new PagedList<CBSyUser>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };

            return PartialView("_DealerUserListPager", list);
        }
        [Privilege(PrivilegeCode.DS1002101)]
        public ActionResult DealerUserInfo(int? id, int page)
        {
            CBSyUser model = null;
            if (id.HasValue)
            {
                var user = SyUserBo.Instance.GetSyUser((int)id);
                model = new CBSyUser
                {
                    Account = user.Account,
                    UserName = user.UserName,
                    MobilePhoneNumber = user.MobilePhoneNumber,
                    EmailAddress = user.EmailAddress,
                    Status = user.Status,
                    SysNo = user.SysNo
                };
                model.GroupUsers = SyUserBo.Instance.GetGroupUser((int)id) as List<SyGroupUser>;
                model.UserMeuns = SyUserBo.Instance.GetUserMenu((int)id);
                model.UserRoles = SyUserBo.Instance.GetUserRole((int)id);
            }
            ViewBag.Page = page;
            return View("DealerUserInfo", model);
        }

        [HttpPost]
        [Privilege(PrivilegeCode.DS1002101)]
        public ActionResult SaveDealerUser(SyUser syUser, IList<History> histories, string ssoId)
        {
            var result = new Result { Message = "", Status = false };
            try
            {
                syUser.Account = syUser.Account.Trim();
                syUser.UserName = syUser.UserName.Trim();
                syUser.EmailAddress = string.IsNullOrWhiteSpace(syUser.EmailAddress) ? null : syUser.EmailAddress.Trim();
                syUser.MobilePhoneNumber = syUser.MobilePhoneNumber.Trim();
                var isCreateUser = syUser.SysNo < 1;
                if (isCreateUser)
                {

                    //2016-4-15 杨浩 添加事务处理
                    using (var tran = new TransactionScope())
                    {
                        if (SyUserBo.Instance.GetSyUser(syUser.Account) != null)
                        {
                            result.Message = "用户账号已存在";
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                        syUser.CreatedBy = CurrentUser.Base.SysNo;
                        syUser.CreatedDate = DateTime.Now;
                        syUser.Password = Util.EncryptionUtil.EncryptWithMd5AndSalt("123456"); //创建用户默认密码
                        syUser.LastUpdateBy = CurrentUser.Base.SysNo;
                        syUser.LastUpdateDate = DateTime.Now;

                        //调用方法创建syUser; 
                        var CurrentsysNo = SyUserBo.Instance.InsertSyUser(syUser);
                        string groupid = string.IsNullOrEmpty(Request["groupid"]) ? "13" : Request["groupid"];//默认分销商组

                        //添加用户 分销商组关系
                        SyGroupUserBo.Instance.Insert(new SyGroupUser
                        {
                            UserSysNo = syUser.SysNo,
                            GroupSysNo = int.Parse(groupid),// 分销商组
                            CreatedBy = CurrentUser.Base.SysNo,
                            CreatedDate = DateTime.Now,
                            LastUpdateBy = CurrentUser.Base.SysNo,
                            LastUpdateDate = DateTime.Now
                        });

                        tran.Complete();
                    }
                }
                else
                {
                    var dbSyUser = SyUserBo.Instance.GetSyUser(syUser.SysNo);
                    if (dbSyUser.EmailAddress != syUser.EmailAddress || dbSyUser.MobilePhoneNumber != syUser.MobilePhoneNumber || dbSyUser.Status != syUser.Status || dbSyUser.UserName != syUser.UserName)
                    {
                        dbSyUser.EmailAddress = syUser.EmailAddress;
                        dbSyUser.MobilePhoneNumber = syUser.MobilePhoneNumber;
                        dbSyUser.Status = syUser.Status;
                        dbSyUser.UserName = syUser.UserName;
                        dbSyUser.LastUpdateBy = CurrentUser.Base.SysNo;
                        dbSyUser.LastUpdateDate = DateTime.Now;
                        syUser = dbSyUser;

                        //调用方法更新syUser;
                        SyUserBo.Instance.UpdateSyUser(syUser);
                    }
                }
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "保存系统用户信息",
                                        LogStatus.系统日志目标类型.系统管理, syUser.SysNo, null, null, CurrentUser.Base.SysNo);

                result.Status = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "保存系统用户信息错误:" + ex.Message,
                                            LogStatus.系统日志目标类型.系统管理, syUser.SysNo, ex, null, CurrentUser.Base.SysNo);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 操作历史记录
        /// </summary>
        public class History
        {
            /// <summary>
            /// 类型
            /// </summary>
            public string type { get; set; }
            /// <summary>
            /// 标识sysno
            /// </summary>
            public int sysno { get; set; }
            /// <summary>
            /// 动作 add、remove
            /// </summary>
            public string action { get; set; }
        }

        #endregion

        #region 经销商订单处理 杨云奕 2016-4-4 21：39
        /// <summary>
        /// 分销商商品分页列表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        [Privilege(PrivilegeCode.DS1009801)]
        public ActionResult GetDealerOrderPager(int? id, ParaOrderFilter filter)
        {
            if (Request.IsAjaxRequest())
            {
                if (CurrentUser.IsBindDealer)
                {
                    filter.DealerSysNo = CurrentUser.Dealer.SysNo;
                }
                else
                {
                    filter.IsBindAllDealer = true;
                }
                ///Ajax数据获取列表信息
                Pager<CBSoOrder> pageOrderList = new Pager<CBSoOrder>()
                {
                    CurrentPage = id ?? 1,
                    PageSize = 10
                };
                Hyt.BLL.Order.SoOrderBo.Instance.DoSoOrderQuery(ref pageOrderList, filter);
                var list = new PagedList<CBSoOrder>
                {
                    TData = pageOrderList.Rows,
                    CurrentPageIndex = pageOrderList.CurrentPage,
                    TotalItemCount = pageOrderList.TotalRows
                };

                return PartialView("_AjaxGetDealerOrderPager", list);
            }
            return View();
        }
        /// <summary>
        /// 商品列表明细
        /// </summary>
        /// <param name="pSysNo"></param>
        /// <returns></returns>
        [Privilege(PrivilegeCode.DS1009801)]
        public JsonResult GetDealerOrderItems(int pSysNo)
        {
            IList<CBSoOrderItem> items = Hyt.BLL.Order.SoOrderBo.Instance.GetCBOrderItemsByOrderId(pSysNo);
            return Json(items);
        }

        /// <summary>
        /// 经销商订单详情
        /// </summary>
        /// <param name="pSysNo">订单pSysNo</param>
        /// <returns></returns>
        [Privilege(PrivilegeCode.DS1009801)]
        public ActionResult GetDealerOrderInfo(int pSysNo)
        {
            Hyt.Model.Manual.SoOrderMods order = Hyt.BLL.Order.SoOrderBo.Instance.GetSoOrderMods(pSysNo);
            if (order.PayStatus == ((int)Hyt.Model.WorkflowStatus.OrderStatus.销售单支付状态.已支付))
            {
                BsArea CityArea = new BsArea();
                BsArea PrivArea = new BsArea();


                order.ReceiveAddress = Hyt.BLL.Order.SoOrderBo.Instance.GetOrderReceiveAddress(order.ReceiveAddressSysNo);
                BsArea tempArea = Hyt.BLL.Order.SoOrderBo.Instance.GetProvinceEntity(order.ReceiveAddress.AreaSysNo, out CityArea, out PrivArea);
                order.ReceiverProvince = PrivArea.AreaName;
                order.ReceiverCity = CityArea.AreaName;
                order.ReceiverArea = tempArea.AreaName;
                ViewBag.Order = order;
                ViewBag.ItemList = Hyt.BLL.Order.SoOrderBo.Instance.GetCBOrderItemsByOrderId(pSysNo);
            }
            else
            {
                order = new Hyt.Model.Manual.SoOrderMods();
                order.ReceiveAddress = new SoReceiveAddress();
                ViewBag.Order = order;
                ViewBag.ItemList = new List<Hyt.Model.Transfer.CBSoOrderItem>();
            }
            return View();
        }
        [Privilege(PrivilegeCode.DS1009801)]
        public JsonResult GetDealerOrderByShip(int SoOrderSysNo, string Express)
        {
            Result result = Hyt.BLL.Order.SoOrderBo.Instance.Ship(SoOrderSysNo, Express);
            return Json(result);
        }
        #endregion

        #region 经销商审核 王耀发 2016-4-18 10：39
        [Privilege(PrivilegeCode.DSA1002101)]
        public ActionResult DsDealerApply(int? id, ParaDsDealerApplyFilter filter)
        {
            if (Request.IsAjaxRequest())
            {
                //当前用户自己对应分销商
                //if (CurrentUser.IsBindDealer)
                //{
                //    int DealerSysNo = CurrentUser.Dealer.SysNo;
                //    filter.DealerSysNo = DealerSysNo;
                //    filter.IsBindDealer = CurrentUser.IsBindDealer;
                //}
                ////是否绑定所有经销商
                //filter.IsBindAllDealer = CurrentUser.IsBindAllDealer;
                //filter.DealerCreatedBy = CurrentUser.Base.SysNo;

                //列表分页开始
                var model = new PagedList<CBDsDealerApply>();

                var modelRef = new Pager<CBDsDealerApply> { CurrentPage = id ?? 1, PageSize = model.PageSize };
                DsDealerApplyBo.Instance.GetDsDealerApplyList(ref modelRef, filter);
                model.TotalItemCount = modelRef.TotalRows;
                model.TData = modelRef.Rows;
                model.CurrentPageIndex = modelRef.CurrentPage;
                return PartialView("_AjaxPagerDsDealerApply", model);
            }
            return View();
        }
        /// <summary>
        /// 更新经销山审核状态
        /// </summary>
        /// <param name="SysNo"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        /// <remarks>2016-04-18 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.DSA1002201, PrivilegeCode.DSA1002301)]
        [HttpPost]
        public ActionResult UpdateStatus(int SysNo, int Status)
        {
            Result result = new Result();
            try
            {
                DsDealerApplyBo.Instance.UpdateStatus(SysNo, Status, CurrentUser.Base.SysNo);
                result.Status = true;
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 经销商城快递代码管理

        /// <summary>
        /// 经销商城快递代码页面
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns></returns>
        /// <remarks>2015-1-19 缪竞华 创建</remarks>
        [Privilege(PrivilegeCode.DS1012101)]
        public ActionResult DsMallExpressCode(ParaDsMallExpressCodeFilter filter)
        {
            ViewBag.DeliveryType = BLL.Logistics.DeliveryTypeBo.Instance.GetLgDeliveryTypeList(); //配送方式
            ViewBag.MallType = BLL.Distribution.DsMallTypeBo.Instance.GetDsMallTypeList(null, null, 1); //商城类型

            var pager = DsMallExpressCodeBo.Instance.Query(filter);
            var list = new PagedList<CBDsMallExpressCode>
            {
                TData = pager.Rows,
                PageSize = filter.PageSize,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };
            if (Request.IsAjaxRequest())
            {
                return PartialView("_AjaxPagerDsMallExpressCode", list);
            }
            return View(list);
        }

        /// <summary>
        /// 添加经销商城快递代码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <remarks>2015-1-19 缪竞华 创建</remarks>
        [HttpGet]
        [Privilege(PrivilegeCode.DS1012201)]
        public ActionResult DsMallExpressCodeAdd()
        {
            ViewBag.DeliveryType = BLL.Logistics.DeliveryTypeBo.Instance.GetLgDeliveryTypeList(); //配送方式
            ViewBag.MallType = BLL.Distribution.DsMallTypeBo.Instance.GetDsMallTypeList(null, null, 1); //商城类型

            return View();
        }

        /// <summary>
        /// 修改经销商城快递代码
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns></returns>
        /// <remarks>2015-1-21 缪竞华 创建</remarks>
        [Privilege(PrivilegeCode.DS1012301)]
        public ActionResult DsMallExpressCodeUpdate(int sysNo)
        {
            DsMallExpressCode model = DsMallExpressCodeBo.Instance.Get(sysNo);
            if (model == null)
            {
                throw new Exception("经销商城快递代码不存在");
            }
            ViewBag.DeliveryType = BLL.Logistics.DeliveryTypeBo.Instance.GetLgDeliveryTypeList(); //配送方式
            ViewBag.MallType = BLL.Distribution.DsMallTypeBo.Instance.GetDsMallTypeList(null, null, 1); //商城类型
            return View(model);
        }

        /// <summary>
        /// 添加经销商城快递代码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <remarks>2015-1-20 缪竞华 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.DS1012201)]
        public JsonResult DsMallExpressCodeAdd(DsMallExpressCode model)
        {
            var result = new Result() { Status = false };
            try
            {
                result = DsMallExpressCodeBo.Instance.Add(model);
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "添加经销商城快递代码",
                                           LogStatus.系统日志目标类型.经销商城快递代码, 0, null, null, CurrentUser.Base.SysNo);
            }
            catch (Exception ex)
            {
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "添加经销商城快递代码:" + ex.Message,
                          LogStatus.系统日志目标类型.经销商城快递代码, 0, ex, null, CurrentUser.Base.SysNo);
            }
            return Json(result);
        }

        /// <summary>
        /// 修改经销商城快递代码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <remarks>2015-1-21 缪竞华 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.DS1012301)]
        public JsonResult DsMallExpressCodeUpdate(DsMallExpressCode model)
        {
            var result = new Result() { Status = false };
            try
            {
                result = DsMallExpressCodeBo.Instance.Update(model);
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "修改经销商城快递代码",
                                           LogStatus.系统日志目标类型.经销商城快递代码, model.SysNo, null, null, CurrentUser.Base.SysNo);
            }
            catch (Exception ex)
            {
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "修改经销商城快递代码:" + ex.Message,
                          LogStatus.系统日志目标类型.经销商城快递代码, model.SysNo, ex, null, CurrentUser.Base.SysNo);
            }
            return Json(result);
        }

        /// <summary>
        /// 通过SysNo删除经销商城快递代码
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns></returns>
        /// <remarks>2015-1-19 缪竞华 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.DS1012401)]
        public JsonResult DeleteDsMallExpressCode(int sysNo)
        {
            var result = new Result() { Status = false };
            try
            {
                result = DsMallExpressCodeBo.Instance.Delete(sysNo);
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Info, LogStatus.系统日志来源.后台, "删除经销商城快递代码",
                                           LogStatus.系统日志目标类型.经销商城快递代码, sysNo, null, null, CurrentUser.Base.SysNo);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "删除经销商城快递代码:" + ex.Message,
                          LogStatus.系统日志目标类型.经销商城快递代码, sysNo, ex, null, CurrentUser.Base.SysNo);
            }
            return Json(result);
        }

        #endregion

        #region 三方商城授权码获取
        /// <summary>
        /// 获取分销商授权码
        /// </summary>
        /// <param name="shopid">商城系统编号</param>
        /// <param name="shopType">商城类型</param>
        /// <param name="appSysNo">应用系统编号</param>
        /// <returns></returns>
        /// <remarks>2017-11-1 杨浩 创建</remarks>
        [Privilege(PrivilegeCode.DS1002101)]
        public JsonResult GetAuthorizationCode(int shopid, int shopType, int appSysNo) 
        {
            var result = new Result<string>() { StatusCode =0,Status=false};

            try
            {

                var appInfo=BLL.Distribution.DsDealerAppBo.Instance.GetDsDealerApp(appSysNo);

                System.Web.HttpContext.Current.Request.Headers.Add("AppKey", appInfo.AppKey);
                             
                var instance = Extra.UpGrade.Provider.UpGradeProvider.GetInstance(shopType);

                var _reslut = instance.GetAuthorizationCode(null);

                if (_reslut.Status)
                {
                    result.Status = true;
                    result.StatusCode = _reslut.StatusCode;
                    result.Data = _reslut.Message;
                }                    
                else
                {
                    result.StatusCode = 1;
                    result.Message = _reslut.Message;
                }
            }
            catch (Exception ex)
            {
                result.StatusCode = 2;
                result.Message = ex.Message;
            }
                           
           return Json(result,JsonRequestBehavior.AllowGet);

           #region 注释
            //var dic = new Dictionary<string, string>();
            //Extra.UpGrade.Api.WebUtils ex = new WebUtils();
            //var objectJson = "";
            //if (shopType=="wu")
            //{              
            //    dic.Add("response_type", "code");
            //    dic.Add("client_id", jdconfig.AppKey);//访问参数    
            //    // dic.Add("code", "code");
            //    dic.Add("redirect_uri", jdconfig.AuthorizeUrl);
            //    dic.Add("state", Convert.ToString(shopid));
            //    objectJson = ex.BuildGetUrl(jdconfig.AuthorizeUrl, dic);
                    
            //}else if(shopType=="一号店"){
              
            //    dic.Add("client_id",yiconfig.AppKey);
            //    dic.Add("response_type","code");
            //    dic.Add("redirect_uri", yiconfig.YihaodianCallBack);
            //    dic.Add("state", Convert.ToString(shopid));
            //    objectJson =ex.BuildGetUrl(yiconfig.AuthorizeUrl,dic);
              
            //}
            //else if ("淘宝" == shopType)
            //{
            //    dic.Add("client_id", tbconfig.AppKey);
            //    dic.Add("response_type", "code");
            //    dic.Add("redirect_uri", tbconfig.TaobaoCallBack);
            //    dic.Add("state",  Convert.ToString(shopid));
            //    objectJson = ex.BuildGetUrl(tbconfig.AuthorizeUrl,dic);             
            //}

            //return Json(objectJson,JsonRequestBehavior.AllowGet);
            #endregion
        }
    

        /// <summary>
        /// 保存令牌
        /// </summary>
        /// <param name="code">授权码</param>
        /// <param name="shopid">状态码</param>
        /// <returns>令牌</returns>
        /// <remarks>2017-11-1 杨浩 创建</remarks>
        [Privilege(PrivilegeCode.None)]
        public ActionResult GetRedirectUriCode(string code=null)
        {
            var _result = new Result() { Status = false };
            var accessToken = "";
            try
            {              
                int shopid = int.Parse(Request["state"]);
                var headers=System.Web.HttpContext.Current.Request.Headers;
                var mallInfo = BLL.Distribution.DsDealerMallBo.Instance.GetDsDealerMall(shopid);
                var appInfo = BLL.Distribution.DsDealerAppBo.Instance.GetDsDealerApp(mallInfo.DealerAppSysNo);
                var instance = Extra.UpGrade.Provider.UpGradeProvider.GetInstance(mallInfo.MallTypeSysNo);

                headers.Add("AppKey",appInfo.AppKey);
                headers.Add("AppSecret", appInfo.AppSecret);
                headers.Add("Extend", appInfo.Extend);

                var result = instance.GetAuthorizationCode(code);
                if (!result.Status)
                {
                    _result.Status = false;
                    _result.Message = result.Message;
                    return View(_result);
                }
                accessToken = result.Data.AccessToken;
                bool isSuccess = Hyt.BLL.Distribution.DsDealerMallBo.Instance.UpdateAuthCode(accessToken, shopid);
                _result.Status = true;
                _result.Message = "获取成功！";
            }
            catch (Exception ex)
            {
                _result.Status = false;
                _result.Message = ex.Message + "accessToken="+accessToken;        
            }

            return View(_result);
        }

        #endregion 
        #region 分销商城同步日志
        /// <summary>
        /// 分销商城同步日志
        /// </summary>
        /// <returns></returns>
        [Privilege(PrivilegeCode.None)]
        public ActionResult DsMallSyncLogs()
        {
            return View();
        }
        /// <summary>
        /// 分销商城同步日志查询
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [Privilege(PrivilegeCode.None)]
        public ActionResult DoDsMallSyncLogQuery(ParaDsMallSyncLogFilter filter)
        {
            if (filter.Id == 0) filter.Id = 1;
            var list= DsMallSyncLogBo.Instance.GetList(filter);
            return PartialView("_DsMallSyncLogListPager",list);
        }
        #endregion 

    }


    


}