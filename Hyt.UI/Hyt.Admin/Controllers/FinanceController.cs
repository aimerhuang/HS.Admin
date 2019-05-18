using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using Hyt.BLL.Authentication;
using Hyt.BLL.Basic;
using Hyt.BLL.Log;
using Hyt.BLL.Order;
using Hyt.BLL.Warehouse;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.SystemPredefined;
using Hyt.Model.Transfer;
using Hyt.BLL.Finance;
using Hyt.Model.WorkflowStatus;
using Hyt.Util;
using System.Threading.Tasks;
using Hyt.BLL.Distribution;
using Hyt.BLL.SellBusiness;
using Hyt.BLL.CRM;

namespace Hyt.Admin.Controllers
{
    /// <summary>
    /// 财务管理
    /// </summary>
    /// <remarks>2013-07-18 朱家宏 创建</remarks>
    public class FinanceController : BaseController
    {

        #region 网上支付

        /// <summary>
        /// 网上支付查询
        /// </summary>
        /// <returns>视图</returns>
        /// <remarks>2013-07-18 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.FN1003101)]
        public ActionResult OnlinePayment()
        {
            return View();
        }

        /// <summary>
        /// 网上支付分页
        /// </summary>
        /// <param name="filter">参数实体</param>
        /// <returns>网上支付分页列表</returns>
        /// <remarks>2013-07-18 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.FN1003101)]
        public ActionResult DoQuery(ParaOnlinePaymentFilter filter)
        {
            var pager = FinanceBo.Instance.GetOnlinePayments(filter);

            var list = new PagedList<CBFnOnlinePayment>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };
            return PartialView("_OnlinePaymentPager", list);
        }

        /// <summary>
        /// 获取订单信息
        /// </summary>
        /// <param name="orderSysNo">订单编号</param>
        /// <returns>json</returns>
        /// <remarks>2013-07-18 朱家宏 创建</remarks>
        [HttpGet]
        [Privilege(PrivilegeCode.FN1003101)]
        public JsonResult GetOrder(int orderSysNo)
        {
            var order = SoOrderBo.Instance.GetEntity(orderSysNo);
            var paymentName = PaymentTypeBo.Instance.GetEntity(order.PayTypeSysNo).PaymentName;
            var json = new JsonResult
            {
                Data = new
                {
                    orderSysNo = order.SysNo,
                    payTypeSysNo = order.PayTypeSysNo,
                    orderAmount = order.OrderAmount,
                    paymentName,
                    transactionSysNo = order.TransactionSysNo,
                    cashPay = order.CashPay
                }
            };
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 创建网上支付
        /// </summary>
        /// <param name="model">支付实体</param>
        /// <returns>json</returns>
        /// <remarks>2013-07-19 朱家宏 创建</remarks>
        /// <remarks>2013-09-26 黄志勇 修改</remarks>
        [Privilege(PrivilegeCode.FN1003201)]
        public JsonResult CreateOnlinePayment(FnOnlinePayment model)
        {
            model.CreatedBy = CurrentUser.Base.SysNo;
            model.CreatedDate = DateTime.Now;
            model.LastUpdateBy = CurrentUser.Base.SysNo;
            model.LastUpdateDate = DateTime.Now;
            var r = new Result { Status = false };
            var options = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                Timeout = TransactionManager.DefaultTimeout
            };
            using (var tran = new TransactionScope(TransactionScopeOption.Required, options))
            {
                model.SysNo = FinanceBo.Instance.CreateOnlinePaymentFromSoOrder(model);
                r.Status = model.SysNo > 0;
                if (r.Status)
                {//更新订单支付方式 
                  SoOrderBo.Instance.UpPayTypeSysNo(model.SourceSysNo, model.PaymentTypeSysNo);
                }
                tran.Complete();
            }
            if (r.Status)
            {
                //Log 创建网上支付 2013-11-15 朱成果
                SysLog.Instance.Info(LogStatus.系统日志来源.后台, "创建网上支付", LogStatus.系统日志目标类型.网上支付, model.SysNo,
                    CurrentUser.Base.SysNo);
            }
            return Json(r);
        }

        /// <summary>
        /// 网上支付查询导出
        /// </summary>
        /// <param name="filter">导出参数</param>
        /// <returns>void</returns>
        /// <remarks>2014-05-26 朱家宏 创建</remarks>
        [Privilege(PrivilegeCode.FN1003101)]
        public void ExportOnlinePayment(ParaOnlinePaymentFilter filter)
        {
            if (filter.PaymentTypeSysNos != null && filter.PaymentTypeSysNos.Any())
            {
                var sysNo = filter.PaymentTypeSysNos.FirstOrDefault();
                if (sysNo == 0)
                    filter.PaymentTypeSysNos = null;
            }
            var result = new List<CBFnOnlinePayment>();
            filter.PageSize = int.MaxValue;
            filter.Id = 1;
            var r = FinanceBo.Instance.GetOnlinePayments(filter);

            if (r != null && r.TotalRows > 0) result = r.Rows.ToList();
            var excel = result.Select(i => new
            {
                订单编号 = i.SourceSysNo,
                交易金额 = i.Amount,
                支付方式 = i.PaymentName,
                交易凭证 = i.VoucherNo,
                交易时间 = i.CreatedDate.ToString("yyyy-MM-dd HH:mm"),
                来源 = i.CreatedSource,
                状态 = EnumUtil.GetDescription(typeof(FinanceStatus.网上支付状态), i.Status)
            }).ToList();

            const string fileName = "在线支付查询表";
            Util.ExcelUtil.ExportLargeDataFromTemplate(excel, @"\Templates\Excel\OnlinePayment.xls", 2, fileName);
        }

        #endregion

        #region 付款单
        /// <summary>
        /// 付款单处理
        /// </summary>
        /// <param name="id">付款单编号</param>
        /// <returns>视图</returns>
        /// <remarks>2013-07-19 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.FN1004101)]
        public ActionResult PaymentDetail(int id)
        {
            Hyt.Model.Transfer.CBFnPaymentVoucher payment = Hyt.BLL.Finance.FinanceBo.Instance.GetPayment(id);
            List<SelectListItem> lst = new List<SelectListItem>();
            if (payment.Source == (int)Hyt.Model.WorkflowStatus.FinanceStatus.付款来源类型.退换货单)
            {
                var rma = Hyt.BLL.RMA.RmaBo.Instance.GetRcReturnEntity(payment.SourceSysNo);
                if (rma != null)
                {
                    var order = Hyt.BLL.Order.SoOrderBo.Instance.GetEntity(rma.OrderSysNo);
                    if (order != null && order.OrderSource == (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单来源.分销商升舱)
                    {
                        lst.Add(new SelectListItem() { Text = Hyt.Model.WorkflowStatus.FinanceStatus.付款单付款方式.分销商预存.ToString(), Value = ((int)Hyt.Model.WorkflowStatus.FinanceStatus.付款单付款方式.分销商预存).ToString() });
                    }
                    else
                    {
                        Hyt.Util.EnumUtil.ToListItem<Hyt.Model.WorkflowStatus.FinanceStatus.付款单付款方式>(ref lst);
                        var item = lst.FirstOrDefault(m => m.Value == ((int)Hyt.Model.WorkflowStatus.FinanceStatus.付款单付款方式.分销商预存).ToString());
                        if (item != null)
                        {
                            lst.Remove(item);
                        }
                    }
                }
            }
            else
            {
                Hyt.Util.EnumUtil.ToListItem<Hyt.Model.WorkflowStatus.FinanceStatus.付款单付款方式>(ref lst);
                var item = lst.FirstOrDefault(m => m.Value == ((int)Hyt.Model.WorkflowStatus.FinanceStatus.付款单付款方式.分销商预存).ToString());
                if (item != null)
                {
                    lst.Remove(item);
                }
            }
            lst.Insert(0, new SelectListItem { Text = "--请选择--", Value = string.Empty });
            ViewBag.PayTypeList = lst;
            return View(payment);
        }

        /// <summary>
        /// 付款单处理
        /// </summary>
        /// <param name="id">RMA 单号</param>
        /// <returns>视图</returns>
        /// <remarks>2013-07-23 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.FN1004101)]
        public ActionResult RMAPaymentDetail(int id)
        {
            Hyt.Model.Transfer.CBFnPaymentVoucher payment = Hyt.BLL.Finance.FinanceBo.Instance.GetPayment((int)Hyt.Model.WorkflowStatus.FinanceStatus.付款来源类型.退换货单, id);
            List<SelectListItem> lst = new List<SelectListItem>();
            if (payment.Source == (int)Hyt.Model.WorkflowStatus.FinanceStatus.付款来源类型.退换货单)
            {
                var rma = Hyt.BLL.RMA.RmaBo.Instance.GetRcReturnEntity(payment.SourceSysNo);
                if (rma != null)
                {
                    var order = Hyt.BLL.Order.SoOrderBo.Instance.GetEntity(rma.OrderSysNo);
                    if (order != null && order.OrderSource == (int)Hyt.Model.WorkflowStatus.OrderStatus.销售单来源.分销商升舱)
                    {
                        lst.Add(new SelectListItem() { Text = Hyt.Model.WorkflowStatus.FinanceStatus.付款单付款方式.分销商预存.ToString(), Value = ((int)Hyt.Model.WorkflowStatus.FinanceStatus.付款单付款方式.分销商预存).ToString() });
                    }
                    else
                    {
                        Hyt.Util.EnumUtil.ToListItem<Hyt.Model.WorkflowStatus.FinanceStatus.付款单付款方式>(ref lst);
                        var item = lst.FirstOrDefault(m => m.Value == ((int)Hyt.Model.WorkflowStatus.FinanceStatus.付款单付款方式.分销商预存).ToString());
                        if (item != null)
                        {
                            lst.Remove(item);
                        }
                    }
                }
            }
            else
            {
                Hyt.Util.EnumUtil.ToListItem<Hyt.Model.WorkflowStatus.FinanceStatus.付款单付款方式>(ref lst);
                var item = lst.FirstOrDefault(m => m.Value == ((int)Hyt.Model.WorkflowStatus.FinanceStatus.付款单付款方式.分销商预存).ToString());
                if (item != null)
                {
                    lst.Remove(item);
                }
            }
            lst.Insert(0, new SelectListItem { Text = "--请选择--", Value = string.Empty });
            ViewBag.PayTypeList = lst;
            return View("PaymentDetail", payment);
        }

        /// <summary>
        /// 添加付款明细
        /// </summary>
        /// <param name="model">付款明细</param>
        /// <returns>json</returns>
        /// <remarks>2013-07-19 朱成果 创建</remarks>
        /// <remarks>2013-09-26 黄志勇 修改</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.FN1004201)]
        public ActionResult AddVoucherItem(FnPaymentVoucherItem model)
        {
            model.Status = (int)FinanceStatus.付款单明细状态.待付款;
            model.CreatedBy = CurrentUser.Base.SysNo;
            model.CreatedDate = DateTime.Now;
            model.LastUpdateBy = CurrentUser.Base.SysNo;
            model.LastUpdateDate = DateTime.Now;
            model.PayDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            var r = new Result { Status = false };
            try
            {
                model.SysNo = FinanceBo.Instance.InsertPaymentVoucherItem(model);
                r.Status = model.SysNo > 0;
                if (r.Status)
                {
                    //Log 添加付款明细 2013-11-15 朱成果
                    SysLog.Instance.Info(LogStatus.系统日志来源.后台, "添加付款单明细", LogStatus.系统日志目标类型.付款单明细, model.SysNo,
                                    CurrentUser.Base.SysNo);
                }
            }
            catch (Exception ex)
            {
                r.Message = ex.Message;
            }
            return Json(r, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 作废付款单明细
        /// </summary>
        /// <param name="sysNo">明细编号</param>
        /// <returns>json</returns>
        /// <remarks>2013-07-22 朱成果 创建</remarks> 
        [HttpPost]
        [Privilege(PrivilegeCode.FN1004201)]
        public ActionResult CancelVoucherItem(int sysNo)
        {
            Result r = new Result
            {
                Status = true
            };
            try
            {
                FinanceBo.Instance.CancelPaymentVoucherItem(sysNo, CurrentUser.Base);

                //Log 作废付款单明细 2013-11-15 朱成果
                Hyt.BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "作废付款单明细", LogStatus.系统日志目标类型.付款单明细, sysNo,
                                CurrentUser.Base.SysNo);
            }
            catch (Exception ex)
            {
                r.Status = false;
                r.Message = ex.Message;
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 取消确认付款单明细
        /// </summary>
        /// <param name="sysNo">明细编号</param>
        /// <remarks>2013-07-22 黄志勇 创建</remarks> 
        [HttpPost]
        [Privilege(PrivilegeCode.FN1004201)]
        public ActionResult CancelConfirmPaymentVoucherItem(int sysNo)
        {
            Result r = new Result { Status = false };
            try
            {
                using (var tran = new TransactionScope())
                {
                    FinanceBo.Instance.CancelConfirmPaymentVoucherItem(sysNo, CurrentUser.Base);
                    tran.Complete();
                    r.Status = true;
                }
                //Log 取消确认付款单明细 2013-11-15 朱成果
                SysLog.Instance.Info(LogStatus.系统日志来源.后台, "取消确认付款单明细", LogStatus.系统日志目标类型.付款单明细, sysNo,
                                CurrentUser.Base.SysNo);
            }
            catch (Exception ex)
            {
                r.Message = ex.Message;
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "取消确认付款单明细",
                                         LogStatus.系统日志目标类型.付款单明细, sysNo, ex, WebUtil.GetUserIp(),
                                         CurrentUser.Base.SysNo);
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 确认付款单明细
        /// </summary>
        /// <param name="sysNo">明细编号</param>
        /// <remarks>2013-07-22 黄志勇 创建</remarks> 
        [HttpPost]
        [Privilege(PrivilegeCode.FN1004201)]
        public ActionResult ConfirmPaymentVoucherItem(int sysNo)
        {
            var r = new Result { Status = false };
            try
            {
                using (var tran = new TransactionScope())
                {
                    FinanceBo.Instance.ConfirmPaymentVoucherItem(sysNo, CurrentUser.Base);
                    tran.Complete();
                    r.Status = true;
                }
                //Log 确认付款单明细 2013-11-15 朱成果
                SysLog.Instance.Info(LogStatus.系统日志来源.后台, "确认付款单明细", LogStatus.系统日志目标类型.付款单明细, sysNo,
                                CurrentUser.Base.SysNo);
            }
            catch (Exception ex)
            {
                r.Message = ex.Message;
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "确认付款单明细",
                                         LogStatus.系统日志目标类型.付款单明细, sysNo, ex, WebUtil.GetUserIp(),
                                         CurrentUser.Base.SysNo);
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 完成付款
        /// </summary>
        /// <param name="sysNo">付款单号</param>
        /// <remarks>2013-07-22 朱成果 创建</remarks> 
        /// <remarks>2013-09-26 黄志勇 修改</remarks> 
        [HttpPost]
        [Privilege(PrivilegeCode.FN1004201)]
        public ActionResult CompletePayment(int sysNo)
        {
            var r = new Result { Status = false };
            try
            {
                using (var tran = new TransactionScope())
                {
                    FinanceBo.Instance.CompletePayment(sysNo, CurrentUser.Base);


                    //如果单据来源为提现单，插入提现记录到提现表中 2016-1-6 王耀发 创建
                    CBFnPaymentVoucher payModel = FinanceBo.Instance.GetPayment(sysNo);
                    string VoucherNoList = "";
                    string VoucherReturnList = "";
                    decimal returnBalance = 0M;
                    foreach (FnPaymentVoucherItem Item in payModel.VoucherItems)
                    {
                        if (VoucherNoList == "")
                        {
                            VoucherNoList = Item.VoucherNo;
                        }
                        else
                        {
                            VoucherNoList += ',' + Item.VoucherNo;
                        }

                        //用于退换货返回余额，只返回明细为余额付款的部分
                        if (Item.PaymentType == (int)FinanceStatus.付款单付款方式.余额 && Item.Status == 20)
                        {
                            if (VoucherReturnList == "")
                            {
                                VoucherReturnList = Item.VoucherNo;
                            }
                            else
                            {
                                VoucherReturnList += ',' + Item.VoucherNo;
                            }
                            returnBalance += Item.Amount;
                        }
                    }

                    #region 分销商提现
                    if ((int)Hyt.Model.WorkflowStatus.FinanceStatus.付款来源类型.分销商提现单 == payModel.Source)
                    {
                        CBDsDealer Dealer = DsDealerBo.Instance.GetDsDealer(payModel.CustomerSysNo);
                        CrPredepositCash cashModel = new CrPredepositCash();
                        cashModel.PdcTradeNo = VoucherNoList;
                        cashModel.PdcOutTradeNo = VoucherNoList;
                        cashModel.PdcUserId = Dealer.SysNo;
                        cashModel.PdcUserName = Dealer.DealerName;
                        cashModel.PdcAmount = payModel.PayableAmount;
                        cashModel.PdcPaymentName = Hyt.Util.EnumUtil.GetDescription(typeof(DistributionStatus.分销商提现支付类型), payModel.PaymentType);
                        cashModel.PdcPaymentId = payModel.PaymentType;
                        cashModel.PdcCashAccount = payModel.RefundAccount;
                        cashModel.PdcToName = payModel.RefundAccountName;
                        cashModel.PdcToBank = payModel.RefundBank;
                        cashModel.PdcAddTime = DateTime.Now;
                        cashModel.PdcPayState = (int)Hyt.Model.WorkflowStatus.FinanceStatus.提现支付状态.已支付;
                        cashModel.PdcAdminId = CurrentUser.Base.SysNo;
                        cashModel.PdType = 1; //1：分销商 0：会员
                        CrPredepositCashBo.Instance.Insert(cashModel);
                        //得到对应资金明细记录
                        DsPrePaymentItem PaymentItem = DsPrePaymentItemBo.Instance.GetEntity(payModel.SourceSysNo);
                        //更新明细记录状态
                        DsPrePaymentItemBo.Instance.UpdatePaymentItemStatus(payModel.SourceSysNo, (int)DistributionStatus.预存款明细状态.完结);
                        //更新资金记录
                        DsPrePaymentBo.Instance.UpdatePaymentValueConfirm(PaymentItem.SourceSysNo, PaymentItem.Decreased);
                    }
                    #endregion
                    #region 会员提现
                    if ((int)Hyt.Model.WorkflowStatus.FinanceStatus.付款来源类型.会员提现单 == payModel.Source)
                    {
                        CrCustomerBo obj = new CrCustomerBo();
                        CBCrCustomer Customer = obj.GetModel(payModel.CustomerSysNo);
                        CrPredepositCash cashModel = new CrPredepositCash();
                        cashModel.PdcTradeNo = VoucherNoList;
                        cashModel.PdcOutTradeNo = VoucherNoList;
                        cashModel.PdcUserId = Customer.SysNo;
                        cashModel.PdcUserName = Customer.Name;
                        cashModel.PdcAmount = payModel.PayableAmount;
                        cashModel.PdcPaymentName = Hyt.Util.EnumUtil.GetDescription(typeof(DistributionStatus.分销商提现支付类型), payModel.PaymentType);
                        cashModel.PdcPaymentId = payModel.PaymentType;
                        cashModel.PdcCashAccount = payModel.RefundAccount;
                        cashModel.PdcToName = payModel.RefundAccountName;
                        cashModel.PdcToBank = payModel.RefundBank;
                        cashModel.PdcAddTime = DateTime.Now;
                        cashModel.PdcPayState = (int)Hyt.Model.WorkflowStatus.FinanceStatus.提现支付状态.已支付;
                        cashModel.PdcAdminId = CurrentUser.Base.SysNo;
                        cashModel.PdType = 0; //1：分销商 0：会员
                        CrPredepositCashBo.Instance.Insert(cashModel);
                        //更新会员记录值
                        CrCustomerBo.Instance.UpdateCustomerValueConfirm(payModel.CustomerSysNo, payModel.PayableAmount);
                    }
                    #endregion
                    #region 退款返回余额，只返回详情中返回余额部分
                    if ((int)Hyt.Model.WorkflowStatus.FinanceStatus.付款来源类型.退换货单 == payModel.Source || (int)Hyt.Model.WorkflowStatus.FinanceStatus.付款来源类型.销售单 == payModel.Source)
                    {
                        if (returnBalance > 0)
                        {
                            //充值记录
                            CrRecharge model = new CrRecharge();
                            model.TradeNo = "Return_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                            model.OutTradeNo = VoucherReturnList;
                            model.CustomerSysNo = payModel.CustomerSysNo;
                            model.ReAmount = returnBalance;
                            model.RePaymentName = "退款，返回余额";
                            model.RePaymentId = Hyt.Model.SystemPredefined.PaymentType.余额支付;
                            model.ReAddTime = DateTime.Now;
                            model.State = 1;
                            model.ReMark = "退款，返回余额：" + returnBalance + "元";
                            int res = Hyt.BLL.Balance.CrRechargeBo.Instance.CreateCrRecharge(model);
                            if (res < 0)
                                throw new Exception("退款，返回余额出错!");
                            //更新余额
                            int isb = Hyt.BLL.Balance.CrRechargeBo.Instance.IsExistenceABalance(payModel.CustomerSysNo);
                            if (isb == 0)
                            {
                                CrAccountBalance balance = new CrAccountBalance();
                                balance.CustomerSysNo = payModel.CustomerSysNo;
                                balance.AvailableBalance = returnBalance;
                                balance.FrozenBalance = 0M;
                                balance.TolBlance = returnBalance;
                                balance.Remark = "";
                                balance.State = 0;
                                balance.AddTime = DateTime.Now;
                                int ba = Hyt.BLL.Balance.CrRechargeBo.Instance.CreateCrAccountBalance(balance);
                                if (ba < 0)
                                    throw new Exception("退款，返回余额出错!");

                            }
                            else
                            {
                                //更新会员余额
                                Hyt.BLL.Balance.CrRechargeBo.Instance.UpdateAccountBalance(returnBalance, model.CustomerSysNo);
                            }
                        }
                    }
                    #endregion
                    tran.Complete();
                    r.Status = true;
                }
                //Log 完成付款 2013-11-15 朱成果
                SysLog.Instance.Info(LogStatus.系统日志来源.后台, "完成付款", LogStatus.系统日志目标类型.付款单, sysNo,
                                CurrentUser.Base.SysNo);
            }
            catch (Exception ex)
            {
                r.Message = ex.Message;
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "完成付款",
                                         LogStatus.系统日志目标类型.付款单, sysNo, ex, WebUtil.GetUserIp(),
                                         CurrentUser.Base.SysNo);
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 付款单管理
        /// </summary>
        /// <remarks>2013-07-19 黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.FN1004101)]
        public ActionResult PaymentVoucherList()
        {
            var sourceList = new List<SelectListItem>();
            var statusList = new List<SelectListItem>();
            Util.EnumUtil.ToListItem<FinanceStatus.付款来源类型>(ref sourceList);
            Util.EnumUtil.ToListItem<FinanceStatus.付款单状态>(ref statusList);
            if (sourceList.Count > 0) ViewBag.sourceList = sourceList;
            if (statusList.Count > 0) ViewBag.statusList = statusList;
            return View();
        }

        /// <summary>
        /// 付款单列表分页
        /// </summary>
        /// <param name="filter">传入的实体参数</param>
        /// <returns>付款单列表</returns>
        /// <remarks>2013-07-19 黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.FN1004101)]
        public ActionResult DoPaymentVoucherQuery(ParaVoucherFilter filter)
        {
            var pager = FinanceBo.Instance.GetPaymentVouchers(filter);
            var list = new PagedList<CBPaymentVoucher>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };
            return PartialView("_PaymentVoucherListPager", list);
        }

        #endregion

        #region 收款单
        /// <summary>
        /// 收款单管理
        /// </summary>
        /// <returns>视图</returns>
        /// <remarks>2013-07-19 黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.FN1002101)]
        public ActionResult ReceiptVoucherList()
        {
            var typeList = new List<SelectListItem>();
            var statusList = new List<SelectListItem>();
            Util.EnumUtil.ToListItem<FinanceStatus.收款单收入类型>(ref typeList);
            Util.EnumUtil.ToListItem<FinanceStatus.收款单状态>(ref statusList);
            if (typeList.Count > 0) ViewBag.typeList = typeList;
            if (statusList.Count > 0) ViewBag.statusList = statusList;
            return View();
        }

        /// <summary>
        /// 收款单列表分页
        /// </summary>
        /// <param name="filter">传入的实体参数</param>
        /// <returns>收款单列表</returns>
        /// <remarks>2013-07-19 黄志勇 创建</remarks>
        [Privilege(PrivilegeCode.FN1002101)]
        public ActionResult DoReceiptVoucherQuery(ParaVoucherFilter filter)
        {
            var pager = Hyt.BLL.Finance.FinanceBo.Instance.GetReceiptVouchers(filter);
            var list = new PagedList<CBFnReceiptVoucher>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };
            return PartialView("_ReceiptVoucherListPager", list);
        }

        /// <summary>
        /// 导出收款单
        /// </summary>
        /// <param name="filter">传入的实体参数</param>
        /// <returns>导出的Excel</returns>
        /// <remarks>2014-07-02 朱成果  创建</remarks>
        [Privilege(PrivilegeCode.FN1002101)]
        public ActionResult ExportReceiptVoucher(ParaVoucherFilter filter)
        {
            return File(Hyt.BLL.Report.ReportBO.Instance.ExportReceiptVoucher(filter, 3), "application/octet-stream", "收款单查询数据.xls");
        }

        /// <summary>
        /// 收款单处理
        /// </summary>
        /// <param name="id">收款单编号</param>
        /// <returns>收款单处理页面</returns>
        /// <remarks>2013-07-22 余勇 创建</remarks>
        [Privilege(PrivilegeCode.FN1002101)]
        public ActionResult ReceiptVoucherDetail(int id)
        {
            CBFnReceiptVoucher model = Hyt.BLL.Finance.FnReceiptVoucherBo.Instance.GetReceiptVoucher(id);
            ViewBag.IsCanEdit = IsCanEdit(id);
            return View(model);
        }

        /// <summary>
        /// 收款单处理
        /// </summary>
        /// <param name="id">收款单编号</param>
        /// <returns>收款单处理页面</returns>
        /// <remarks>2013-07-22 余勇 创建</remarks>
        [Privilege(PrivilegeCode.FN1002101)]
        public ActionResult ViewReceiptVoucherDetail(int id)
        {
            CBFnReceiptVoucher model = Hyt.BLL.Finance.FnReceiptVoucherBo.Instance.GetReceiptVoucher(id);
            ViewBag.IsView = true;
            return View("ReceiptVoucherDetail", model);
        }

        /// <summary>
        /// 收款单明细列表
        /// </summary>
        /// <param name="id">收款单编号</param>
        /// <returns>收款单明细列表</returns>
        /// <remarks>2013-07-22 余勇 创建</remarks>
        [Privilege(PrivilegeCode.FN1002101)]
        public ActionResult ReceiptVoucherDetailList(int id)
        {
            var list = Hyt.BLL.Finance.FnReceiptVoucherBo.Instance.GetReceiptVoucherItem(id);
            ViewBag.IsCanEdit = IsCanEdit(id);
            return PartialView("_ReceiptVoucherDetailList", list);
        }

        /// <summary>
        /// 是否能增删明细
        /// </summary>                                                                                                                                                                                                                                  
        /// <param name="sysNo">收款单编号</param>
        /// <returns>是否成功</returns>
        /// <remarks>2013-07-22 余勇 创建</remarks>
        private bool IsCanEdit(int sysNo)
        {
            bool res = false;
            CBFnReceiptVoucher model = Hyt.BLL.Finance.FnReceiptVoucherBo.Instance.GetReceiptVoucher(sysNo);
            if (model.Status == (int)FinanceStatus.收款单状态.待确认)
            {
                res = true;
            }
            return res;
        }

        /// <summary>
        /// 添加收款明细
        /// </summary>
        /// <param name="model">收款明细</param>
        /// <returns>是否成功</returns>
        /// <remarks>2013-07-22 余勇 创建</remarks>
        /// <remarks>2013-09-26 黄志勇 修改</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.FN1002201)]
        public ActionResult AddReceiptVoucherItem(FnReceiptVoucherItem model)
        {
            var r = new Result<decimal>();
            model.CreatedBy = CurrentUser.Base.SysNo;
            model.CreatedDate = DateTime.Now;
            model.LastUpdateBy = CurrentUser.Base.SysNo;
            model.LastUpdateDate = DateTime.Now;
            model.Status = (int)FinanceStatus.收款单明细状态.有效;
            try
            {
                IList<FnReceiptVoucherItem> items = new List<FnReceiptVoucherItem>();
                items.Add(model);
                FnReceiptVoucherBo.Instance.CreateReceiptVoucherItem(items);
                // 更新订单支付类型
                var reVoucher = FnReceiptVoucherBo.Instance.GetReceiptVoucher(model.ReceiptVoucherSysNo);
                var soOrder = BLL.Order.SoOrderBo.Instance.GetEntity(reVoucher.SourceSysNo);
                if (soOrder != null)
                {
                    soOrder.PayTypeSysNo = model.PaymentTypeSysNo;
                    BLL.Order.SoOrderBo.Instance.UpdateOrder(soOrder);
                }
                //获取收款单实收金额
                decimal total = GetReceiveAmount(model.ReceiptVoucherSysNo);
                //修改收款单应收金额
                UpdateReceivedAmount(model.ReceiptVoucherSysNo);
                r.Data = total;
                //Log 添加收款明细 2013-11-15 朱成果
                SysLog.Instance.Info(LogStatus.系统日志来源.后台, "添加收款明细", LogStatus.系统日志目标类型.收款单明细, model.SysNo,
                                CurrentUser.Base.SysNo);
            }
            catch (Exception ex)
            {
                r.StatusCode = -1;
                r.Message = ex.Message;
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "添加收款明细",
                                         LogStatus.系统日志目标类型.收款单明细, model.SysNo, ex, WebUtil.GetUserIp(),
                                         CurrentUser.Base.SysNo);
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 修改收款单实收金额
        /// </summary>
        /// <param name="sysNo">收款单编号</param>
        /// <returns>空</returns>
        /// <remarks>2013-07-23 余勇 创建</remarks>
        private void UpdateReceivedAmount(int sysNo)
        {
            var receiptVoucher = FnReceiptVoucherBo.Instance.GetReceiptVoucher(sysNo);
            receiptVoucher.ReceivedAmount = GetReceiveAmount(sysNo);
            //修改收款单实收金额
            FnReceiptVoucherBo.Instance.Update(receiptVoucher);
        }

        /// <summary>
        /// 获取实收金额
        /// </summary>
        /// <param name="sysNo">收款单编号</param>
        /// <returns>收款单实收金额</returns>
        /// <remarks>2013-07-23 余勇 创建</remarks>
        private decimal GetReceiveAmount(int sysNo)
        {
            return FnReceiptVoucherBo.Instance.GetReceiveAmount(sysNo);
        }

        /// <summary>
        /// 删除收款明细
        /// </summary>
        /// <param name="id">收款明细编号</param>
        /// <param name="sysNo">收款单编号</param>
        /// <returns>是否成功</returns>
        /// <remarks>2013-07-22 余勇 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.FN1002201)]
        public ActionResult DeleteReceiptVoucherItem(int id, int sysNo)
        {
            Result<decimal> r = new Result<decimal>();
            try
            {
                FnReceiptVoucherBo.Instance.DeleteReceiptVoucherItem(id);
                //获取实收金额
                decimal total = GetReceiveAmount(sysNo);
                //修改收款单实收金额
                UpdateReceivedAmount(sysNo);
                r.Data = total;
                SysLog.Instance.Info(LogStatus.系统日志来源.后台, "删除收款明细", LogStatus.系统日志目标类型.收款单明细, id,
                                CurrentUser.Base.SysNo);
            }
            catch (Exception ex)
            {
                r.StatusCode = -1;
                r.Message = ex.Message;
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "删除收款明细",
                                         LogStatus.系统日志目标类型.收款单明细, id, ex, WebUtil.GetUserIp(),
                                         CurrentUser.Base.SysNo);
            }
            return Json(r, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 作废收款明细
        /// </summary>
        /// <param name="id">收款明细编号</param>
        /// <param name="sysNo">收款单编号</param>
        /// <returns>是否成功</returns>
        /// <remarks>2013-07-22 余勇 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.FN1002201)]
        public ActionResult InvalidReceiptVoucherItem(int id, int sysNo)
        {
            var r = new Result<decimal>();
            try
            {
                var res = FnReceiptVoucherBo.Instance.InvalidReceiptVoucherItem(id);
                if (res > 0)
                {
                    //获取实收金额
                    decimal total = GetReceiveAmount(sysNo);
                    //修改收款单实收金额
                    UpdateReceivedAmount(sysNo);
                    r.Data = total;
                }
                //Log 作废收款明细 2013-11-15 朱成果
                SysLog.Instance.Info(LogStatus.系统日志来源.后台, "作废收款明细", LogStatus.系统日志目标类型.收款单明细, id,
                                CurrentUser.Base.SysNo);
            }
            catch (Exception ex)
            {
                r.StatusCode = -1;
                r.Message = ex.Message;
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "作废收款明细",
                                         LogStatus.系统日志目标类型.收款单明细, id, ex, WebUtil.GetUserIp(),
                                         CurrentUser.Base.SysNo);
            }
            return Json(r, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 作废收款单
        /// </summary>
        /// <param name="id">收款单编号</param>
        /// <returns>是否成功</returns>
        /// <remarks>2013-07-22 余勇 创建</remarks> 
        [HttpPost]
        [Privilege(PrivilegeCode.FN1002201)]
        public ActionResult CancelReceiptVoucher(int id)
        {
            Result r = new Result();
            try
            {
                using (var tran = new TransactionScope())
                {
                    FnReceiptVoucherBo.Instance.CancelReceiptVoucher(id, CurrentUser.Base);
                    tran.Complete();
                }
                //Log 作废收款明细 2013-12-16 黄志勇
                SysLog.Instance.Info(LogStatus.系统日志来源.后台, "作废收款单", LogStatus.系统日志目标类型.收款单, id,
                                CurrentUser.Base.SysNo);
            }
            catch (Exception ex)
            {
                r.StatusCode = -1;
                r.Message = ex.Message;
                SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "作废收款单",
                                         LogStatus.系统日志目标类型.收款单, id, ex, WebUtil.GetUserIp(),
                                         CurrentUser.Base.SysNo);
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 确认收款单
        /// </summary>
        /// <param name="id">明细编号</param>
        /// <returns>是否成功</returns>
        /// <remarks>2013-07-22 余勇 创建</remarks> 
        /// <remarks>2013-09-26 黄志勇 修改</remarks> 
        [HttpPost]
        [Privilege(PrivilegeCode.FN1002201)]
        public ActionResult ConfirmReceiptVoucher(int id)
        {
            var r = new Result();
            var receiptVoucher = Hyt.BLL.Finance.FnReceiptVoucherBo.Instance.GetReceiptVoucher(id);
            decimal receivedAmount = receiptVoucher.IncomeAmount;
            decimal incomeAmount = GetReceiveAmount(id);
            if (incomeAmount != receivedAmount)
            {
                r.StatusCode = -1;
                r.Message = "实收金额与应收金额不相等,不能确认";
            }
            //当结算单状态为已确认，不再执行 余勇(2014-08-04)
            if (r.StatusCode == 0 && receiptVoucher.Status != (int)FinanceStatus.收款单状态.已确认)
            {
                try
                {
                    using (var tran = new TransactionScope())
                    {
                        FnReceiptVoucherBo.Instance.ConfirmReceiptVoucher(id, CurrentUser.Base);
                        tran.Complete();
                    }
                    //写EAS数据
                    //升舱订单不导入 2013-12-23 杨合余  
                    var order = Hyt.BLL.Order.SoOrderBo.Instance.GetEntity(receiptVoucher.SourceSysNo);
                    if (order.PayStatus == Hyt.Model.WorkflowStatus.OrderStatus.销售单支付状态.已支付.GetHashCode())
                    {
                        //进入订单分销提成系统 2015-10-28 王耀发 
                        Hyt.BLL.CRM.CrCustomerBo.Instance.ExecuteSellBusinessRebates(order);//导致数据回滚，迁出来
                    }
                    if (order.OrderSource != (int)OrderStatus.销售单来源.分销商升舱)
                        FnReceiptVoucherBo.Instance.WriteEasReceiptVoucher(id, CurrentUser.Base);

                    //Log 收款确认 2013-11-15 朱成果
                    SysLog.Instance.Info(LogStatus.系统日志来源.后台, "收款单确认", LogStatus.系统日志目标类型.收款单, id, CurrentUser.Base.SysNo);

                    //送积分 2015-11-2 王耀发创建 订单状态为待支付
                    if (order.Status == 10)
                    {
                        Hyt.BLL.LevelPoint.PointBo.Instance.OrderIncreasePoint(order.CustomerSysNo, order.SysNo, (int)order.CashPay, order.TransactionSysNo);//增加积分
                    }
                }
                catch (Exception ex)
                {
                    r.StatusCode = -1;
                    r.Message = ex.Message;
                    SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "收款单确认",
                                             LogStatus.系统日志目标类型.收款单, id, ex, WebUtil.GetUserIp(),
                                             CurrentUser.Base.SysNo);
                }
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 批量确认收款单
        /// </summary>
        /// <param name="sysNos">明细编号</param>
        /// <returns>是否成功</returns>
        /// <remarks>2014-07-10 余勇 创建</remarks>  
        [HttpPost]
        [Privilege(PrivilegeCode.FN1002201)]
        public ActionResult BatchConfirmReceiptVoucher(int[] sysNos)
        {
            var r = new Result();
            foreach (var id in sysNos)
            {
                var receiptVoucher = Hyt.BLL.Finance.FnReceiptVoucherBo.Instance.GetReceiptVoucher(id);
                if (receiptVoucher == null)
                    continue;
                if (receiptVoucher.Status != (int)FinanceStatus.收款单状态.待确认)
                    continue;
                decimal receivedAmount = receiptVoucher.IncomeAmount;
                decimal incomeAmount = GetReceiveAmount(id);
                if (incomeAmount != receivedAmount)
                {
                    r.StatusCode = -1;
                    r.Message = string.Format("收款单{0}实收金额与应收金额不相等,不能确认", receiptVoucher.SysNo);
                    break;
                }
                if (r.StatusCode == 0)
                {
                    try
                    {
                        using (var tran = new TransactionScope())
                        {
                            FnReceiptVoucherBo.Instance.ConfirmReceiptVoucher(id, CurrentUser.Base);
                            tran.Complete();
                        }

                        //写EAS数据
                        //升舱订单不导入 2013-12-23 杨合余
                        var order = Hyt.BLL.Order.SoOrderBo.Instance.GetEntity(receiptVoucher.SourceSysNo);
                        if (order.OrderSource != (int)OrderStatus.销售单来源.分销商升舱)
                            FnReceiptVoucherBo.Instance.WriteEasReceiptVoucher(id, CurrentUser.Base);

                        //Log 收款确认 2013-11-15 朱成果
                        SysLog.Instance.Info(LogStatus.系统日志来源.后台, "收款单确认", LogStatus.系统日志目标类型.收款单, id, CurrentUser.Base.SysNo);
                    }
                    catch (Exception ex)
                    {
                        r.StatusCode = -1;
                        r.Message = ex.Message;
                        SysLog.Instance.WriteLog(LogStatus.SysLogLevel.Error, LogStatus.系统日志来源.后台, "收款单批量确认",
                                                 LogStatus.系统日志目标类型.收款单, id, ex, WebUtil.GetUserIp(),
                                                 CurrentUser.Base.SysNo);
                        break;
                    }
                }

            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 取消确认收款单
        /// </summary>
        /// <param name="id">收款单编号</param>
        /// <returns>json</returns>
        /// <remarks>2013-07-22 余勇 创建</remarks> 
        /// <remarks>2013-09-26 黄志勇 修改</remarks> 
        /// <remarks>2013-09-26 朱家宏 取消改功能</remarks> 
        [HttpPost]
        [Obsolete("已取消该功能")]
        public ActionResult CancelConfirmReceiptVoucher(int id)
        {
            Result r = new Result();
            try
            {
                using (var tran = new TransactionScope())
                {
                    //FnReceiptVoucherBo.Instance.CancelConfirmReceiptVoucher(id, CurrentUser.Base);
                    tran.Complete();
                }
                //Log 取消收款单确认 2013-11-15 朱成果
                //Hyt.BLL.Log.SysLog.Instance.Info(LogStatus.系统日志来源.后台, "取消收款单确认", LogStatus.系统日志目标类型.收款单, id,
                //                   CurrentUser.Base.SysNo);
            }
            catch (Exception ex)
            {
                r.StatusCode = -1;
                r.Message = ex.Message;
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 收款科目管理 黄伟 2013-10-9

        /// <summary>
        /// 收款账目管理
        /// </summary>
        /// <param name="id">page index</param>
        /// <param name="para">FnReceiptTitle entity</param>
        /// <returns>ActionResult</returns>
        /// <remarks>2013-10-09 黄伟 created</remarks>
        [Privilege(PrivilegeCode.FN1005101)]
        public ActionResult ReceiptManagement(int? id, ParaBasicReceiptManagement para)
        {
            var dic = ReceiptManagementBo.Instance.QueryReceipt(para, id ?? 1);

            var model = new PagedList<FnReceiptTitleAssociation>
            {
                TData = dic.Any() ? dic.First().Value : null,
                CurrentPageIndex = id ?? 1,
                TotalItemCount = dic.Any() ? dic.First().Key : 0
            };

            if (Request.IsAjaxRequest())
            {
                return PartialView("pAjaxPager_Receipt", model);
            }

            //支付方式类型下拉列表 
            var lstTitleType = new List<SelectListItem> { new SelectListItem { Text = @"全部", Value = "-1" } };
            PaymentTypeBo.Instance.GetAll().ForEach(p => lstTitleType.Add(new SelectListItem
            {
                //Selected = false,
                Text = p.PaymentName,
                Value = p.SysNo + ""
            }));
            ViewBag.lstTitleType = lstTitleType;
            //状态下拉列表 是否默认
            var lstStatus = new List<SelectListItem> { new SelectListItem { Text = @"全部", Value = "-1" } };
            Enum.GetNames(typeof(FinanceStatus.是否默认收款科目)).ForEach(p => lstStatus.Add(new SelectListItem
            {
                //Selected = false,
                Text = p,
                Value = Enum.Parse(typeof(FinanceStatus.是否默认收款科目), p).GetHashCode() + ""
            }));
            ViewBag.lstStatus = lstStatus;

            return View(model);
        }

        /// <summary>
        /// set支付类型
        /// </summary>
        /// <param name="id">支付类型</param>
        /// <returns>void</returns>
        /// <remarks>2013-10-09 黄伟 created</remarks>
        [Privilege(PrivilegeCode.FN1005301)]
        [HttpPost]
        public void SetPayTypeForImported(int id)
        {
            TempData["payType"] = id;
        }

        /// <summary>
        /// 导入excel
        /// </summary>
        /// <returns>ActionResult</returns>
        /// <remarks>2010-10-10 黄伟 创建</remarks>
        [Privilege(PrivilegeCode.FN1005301)]
        public ActionResult ImportExcel()
        {
            //frm load
            if (Request.Files.Count == 0)
                return View();

            //files choosed,and submitted with the form
            var result = TempData["payType"] == null ?
                ReceiptManagementBo.Instance.ImportExcel(Request.Files[0].InputStream, HttpContext.Request.Params["Remote_Add"], CurrentUser.Base.SysNo)
                : ReceiptManagementBo.Instance.ImportExcel(Request.Files[0].InputStream, HttpContext.Request.Params["Remote_Add"], CurrentUser.Base.SysNo, int.Parse(TempData["payType"].ToString()));

            ViewBag.result = result.Message;

            //return to excute the page script
            return View();

        }

        /// <summary>
        /// 收款账目管理
        /// </summary>
        /// <param name="model">收款账目实体</param>
        /// <returns>ActionResult</returns>
        /// <remarks>2010-10-10 黄伟 创建</remarks>
        [Privilege(PrivilegeCode.FN1005301)]
        public ActionResult CreateOrUpdateReceipt(FnReceiptTitleAssociation model)
        {
            //支付方式类型下拉列表 
            var lstTitleType = new List<SelectListItem> { new SelectListItem { Text = @"请选择", Value = "-1" } };
            PaymentTypeBo.Instance.GetAll().ForEach(p => lstTitleType.Add(new SelectListItem
            {
                //Selected = false,
                Text = p.PaymentName,
                Value = p.SysNo + ""
            }));
            ViewBag.lstTitleType = lstTitleType;

            //状态下拉列表 是否默认
            var lstStatus = new List<SelectListItem> { new SelectListItem { Text = @"请选择", Value = "-1" } };
            Enum.GetNames(typeof(FinanceStatus.是否默认收款科目)).ForEach(p => lstStatus.Add(new SelectListItem
            {
                //Selected = false,
                Text = p,
                Value = Enum.Parse(typeof(FinanceStatus.是否默认收款科目), p).GetHashCode() + ""
            }));
            ViewBag.lstStatus = lstStatus;

            return View(model);

        }

        /// <summary>
        /// 新增收款账目
        /// </summary>
        /// <param name="model">收款账目实体</param>
        /// <returns>是否成功</returns>
        /// <remarks>2010-10-10 黄伟 创建</remarks>
        [Privilege(PrivilegeCode.FN1005201)]
        [HttpPost]
        public ActionResult CreateReceipt(FnReceiptTitleAssociation model)
        {
            var result = ReceiptManagementBo.Instance.CreateReceipt(new List<FnReceiptTitleAssociation> { model }, HttpContext.Request.ServerVariables["Remote_Add"], CurrentUser.Base.SysNo);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 修改收款账目
        /// </summary>
        /// <param name="model">收款账目实体</param>
        /// <returns>是否成功</returns>
        /// <remarks>2010-10-10 黄伟 创建</remarks>
        [Privilege(PrivilegeCode.FN1005301)]
        [HttpPost]
        public ActionResult UpdateReceipt(FnReceiptTitleAssociation model)
        {
            string CreatedDate = Request.Form["LastUpdateDate"];
            var result = ReceiptManagementBo.Instance.UpdateReceipt(new List<FnReceiptTitleAssociation> { model }, HttpContext.Request.ServerVariables["Remote_Add"], CurrentUser.Base.SysNo);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除收款账目
        /// </summary>
        /// <param name="arrDelSysnos">要删除的收款科目系统编号集合,comma separeated</param>
        /// <returns>是否左边</returns>
        /// <remarks>2010-10-10 黄伟 创建</remarks>
        [Privilege(PrivilegeCode.FN1005401)]
        [HttpPost]
        public ActionResult DeleteReceipt(string arrDelSysnos)
        {
            var lstDelSysnos = arrDelSysnos.Split(',').Select(int.Parse).ToList();

            var result = ReceiptManagementBo.Instance.DeleteReceipt(lstDelSysnos,
                                                             HttpContext.Request.ServerVariables["Remote_Add"],
                                                             CurrentUser.Base.SysNo);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 设置收款账目启用/禁用
        /// </summary>
        /// <param name="id">id of receipt</param>
        /// <param name="status">receipt status</param>
        /// <returns>ActionResult</returns>
        /// <remarks>2010-10-10 黄伟 创建</remarks>
        [Privilege(PrivilegeCode.FN1005301)]
        [HttpPost]
        public ActionResult SetReceiptStatus(int id, string status)
        {
            //var result = ReceiptManagementBo.Instance.SetReceiptStatus(id,Enum.Parse(typeof(FinanceStatus.收款科目状态),status).GetHashCode(), HttpContext.Request.ServerVariables["Remote_Add"], CurrentUser.Base.SysNo);
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 导出收款科目
        /// </summary>
        /// <returns>void</returns>
        /// <remarks>2010-10-10 黄伟 创建</remarks>
        [Privilege(PrivilegeCode.FN1005101)]
        public void ExportRecTemplate()
        {
            Util.ExcelUtil.Export<FnReceiptTitleAssociation>(new List<FnReceiptTitleAssociation>(), new List<string>
                {
                    "仓库名称","仓库编码","库存组织","地区","组织部门名称","组织部门编码","收款科目名称","收款科目编码"
                }, "收款科目管理导入模板");
        }

        #endregion

        #region 加盟商对账，支付宝对账
        /// <summary>
        /// 加盟商对账，支付宝对账
        /// </summary>
        /// <returns></returns>
        /// <remarks>2014-08-21 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.FN1002202)]
        public ActionResult ThirdpartyReconciliation()
        {
            var SourceList = new List<SelectListItem>();
            var StatusList = new List<SelectListItem>();
            Util.EnumUtil.ToListItem<FinanceStatus.第三方财务对账来源>(ref SourceList);
            Util.EnumUtil.ToListItem<FinanceStatus.第三方财务对账状态>(ref StatusList);
            ViewBag.SelectList = SourceList;
            ViewBag.StatusList = StatusList;
            return View();
        }

        /// <summary>
        /// 上传对账Excel
        /// </summary>
        /// <param name="source">来源</param>
        /// <returns></returns>
        /// <remarks>2014-08-21 朱成果 创建</remarks>
        [HttpPost]
        [Privilege(PrivilegeCode.FN1002202)]
        public ActionResult ImportReconciliation(int? source)
        {
            if (!source.HasValue)
            {
                return Content("请先选择账单来源！");
            }
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                if (file.ContentType == "application/vnd.ms-excel")
                {
                    List<FnThirdpartyReconciliation> lst = null;
                    try
                    {
                        lst = FinanceBo.Instance.GetReconciliationExcelData(file.InputStream, source.Value, CurrentUser.Base, file.FileName);//获取对账数据
                    }
                    catch (Exception ex)
                    {
                        return Content("文件格式错误！");
                    }
                    if (lst != null)
                    {
                        Task taskimport = new Task(() =>
                        {
                            lst.ForEach((item) =>
                            {
                                item.SysNo = FinanceBo.Instance.InsertReconciliation(item);//Excel表数据写入数据库
                            });
                        });
                        taskimport.ContinueWith(t =>
                        {
                            lst.ForEach((item) =>
                            {
                                FinanceBo.Instance.CheckReconciliation(item);//加盟商对账
                            });

                        });

                        taskimport.Start();
                    }
                    return Content("ok");
                }
                else
                {
                    return Content("请上传.xls类型文件!");
                }
            }
            else
            {
                return Content("没有获取到文件数据!");
            }
        }

        /// <summary>
        /// 获取对账数据
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns></returns>
        /// <remarks>2014-08-21 朱成果 创建</remarks>
        [Privilege(PrivilegeCode.FN1002202)]
        public ActionResult _ThirdpartyReconciliation(ParaReconciliationFilter filter)
        {
            if (filter.Id < 1)
            {
                filter.Id = 1;
            }
            filter.PageSize = 30;
            var lst = FinanceBo.Instance.QueryReconciliation(filter);
            PagedList<FnThirdpartyReconciliation> page = new PagedList<FnThirdpartyReconciliation>();
            page.CurrentPageIndex = lst.CurrentPage;
            page.TotalItemCount = lst.TotalRows;
            page.PageSize = filter.PageSize;
            page.TData = lst.Rows;
            return PartialView("_ThirdpartyReconciliation", page);
        }
        #endregion

    }
}
