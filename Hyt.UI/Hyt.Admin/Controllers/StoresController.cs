using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Transactions;
using System.Web.Configuration;
using System.Web.Mvc;
using Hyt.BLL.Stores;
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
using Hyt.BLL.Finance;

namespace Hyt.Admin.Controllers
{
    public class StoresController : BaseController
    {
        //
        // GET: /Stores/
        /// <summary>
        /// 保证金订单查询
        /// </summary>
        /// <returns></returns>
        [Privilege(PrivilegeCode.ST100101)]
        public ActionResult DsDealerBailOrderList()
        {
            return View();
        }
        /// <summary>
        /// 分页获取保证金订单
        /// </summary>
        /// <param name="filter">传入的实体参数</param>
        /// <returns>保证金订单列表</returns>
        /// <remarks>2016-05-16 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.ST100101)]
        public ActionResult DoDsDealerBailOrderQuery(ParaDsDealerBailOrderFilter filter)
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
            var pager = DsDealerBailOrderBo.Instance.Query(filter);
            var list = new PagedList<CBDsDealerBailOrder>
            {
                TData = pager.Rows,
                CurrentPageIndex = pager.CurrentPage,
                TotalItemCount = pager.TotalRows
            };
            return PartialView("_DsDealerBailOrderPager", list);
        }
        /// <summary>
        /// 保证金订单
        /// </summary>
        /// <param name="id">保证金订单编号</param>
        /// <returns>保证金订单处理页面</returns>
        /// <remarks>2016-05-16 王耀发 创建</remarks>
        [Privilege(PrivilegeCode.ST100101)]
        public ActionResult DealerBailOrderView(int id)
        {
            DsDealerBailOrder model = DsDealerBailOrderBo.Instance.GetModel(id);
            return View("DealerBailOrderDetail", model);
        }
        /// <summary>
        /// 审核保证金订单
        /// </summary>
        /// <param name="SysNo"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        /// <remarks>2016-05-16 王耀发 创建</remarks>
        /// <remarks>2016-07-06 罗远康 增加审核赠送10元</remarks>
        [Privilege(PrivilegeCode.ST100102)]
        public JsonResult Audit(int SysNo, int Status)
        {
            var result = new Result
            {
                Status = false
            };
            try
            {
                string IsSellBusiness = "";
                result.Status = true;
                //更新状态
                DsDealerBailOrderBo.Instance.UpdateStatus(SysNo, 0, Status);
                DsDealerBailOrder Model = DsDealerBailOrderBo.Instance.GetModel(SysNo);
                //更新会员是否为分销商的状态
                if (Status == (int)Hyt.Model.WorkflowStatus.StoresStatus.保证金订单状态.待审核)
                {
                    IsSellBusiness = "0";
                    Hyt.BLL.CRM.CrCustomerBo.Instance.UpdateIsSellBusiness(Model.CustomerSysNo, IsSellBusiness);
                }
                if (Status == (int)Hyt.Model.WorkflowStatus.StoresStatus.保证金订单状态.已审核)
                {
                    IsSellBusiness = "1";
                    Hyt.BLL.CRM.CrCustomerBo.Instance.UpdateIsSellBusiness(Model.CustomerSysNo, IsSellBusiness);
                    string OutTradeNo="DsDealerBailOrder_" + Model.SysNo;
                    int Recharge = BLL.Balance.CrRechargeBo.Instance.IsDealerCrRecharge(Model.CustomerSysNo, OutTradeNo);
                    if (Recharge == 0) 
                    {
                        decimal amount = 10;
                        CrRecharge model = new CrRecharge();
                        model.TradeNo = "RechargeWX_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                        model.OutTradeNo = "DsDealerBailOrder_" + Model.SysNo;
                        model.CustomerSysNo = Model.CustomerSysNo;
                        model.ReAmount = amount;
                        model.RePaymentName = "保证金返回";
                        model.RePaymentId = Hyt.Model.SystemPredefined.PaymentType.分销商预存;
                        model.ReAddTime = DateTime.Now;
                        model.ReMark = "分销商审核通过返回：" + amount + "元";
                        model.State = 1;
                        Hyt.BLL.Balance.CrRechargeBo.Instance.CreateCrRecharge(model);
                        int isb = Hyt.BLL.Balance.CrRechargeBo.Instance.IsExistenceABalance(Model.CustomerSysNo);
                        if (isb == 0)
                        {
                            CrAccountBalance balance = new CrAccountBalance();
                            balance.CustomerSysNo = Model.CustomerSysNo;
                            balance.AvailableBalance = 10;
                            balance.FrozenBalance = 0M;
                            balance.TolBlance = 10;
                            balance.Remark = "";
                            balance.State = 0;
                            balance.AddTime = DateTime.Now;
                            Hyt.BLL.Balance.CrRechargeBo.Instance.CreateCrAccountBalance(balance);
                        }
                        else 
                        {
                            Hyt.BLL.Balance.CrRechargeBo.Instance.UpdateAccountBalance(amount, Model.CustomerSysNo);
                        }
                    }
                    
                }
                if (Status == (int)Hyt.Model.WorkflowStatus.StoresStatus.保证金订单状态.作废)
                {
                    //插入记录到付款单
                    FnPaymentVoucher paymentVoucher = new FnPaymentVoucher();
                    paymentVoucher.Source = 20;
                    paymentVoucher.SourceSysNo = SysNo;
                    paymentVoucher.PayableAmount = Model.Money;
                    paymentVoucher.PaidAmount = 0;
                    paymentVoucher.CustomerSysNo = Model.CustomerSysNo;
                    paymentVoucher.Status = 10;
                    paymentVoucher.Remarks = "保证金作废";
                    paymentVoucher.CreatedDate = DateTime.Now;
                    paymentVoucher.CreatedBy = CurrentUser.Base.SysNo;
                    paymentVoucher.PayerSysNo = 0;
                    paymentVoucher.PayDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue; ;
                    paymentVoucher.LastUpdateDate = DateTime.Now;
                    paymentVoucher.LastUpdateBy = CurrentUser.Base.SysNo;
                    paymentVoucher.PaymentType = 0;
                    int vSysNo = FinanceBo.Instance.InsertPaymentVoucher(paymentVoucher);
                }
                
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
