using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Infrastructure.Pager;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.DataAccess.SellBusiness;
using Hyt.Model.WorkflowStatus;
using System.Transactions;

namespace Hyt.BLL.SellBusiness
{
    /// <summary>
    /// 会员提现信息
    /// </summary>
    /// <param name="filter">会员提现信息</param>
    /// <returns>返回会员提现信息</returns>
    /// <remarks>2015-09-15 王耀发 创建</remarks>
    public class CrPredepositCashBo : BOBase<CrPredepositCashBo>
    {

        #region 会员提现
        /// <summary>
        /// 分页获取会员提现
        /// </summary>
        /// <param name="filter">筛选条件</param>
        /// <returns>分页列表</returns>
        /// <remarks>2015-08-06 王耀发 创建</remarks>
        public Pager<CrPredepositCash> GetCrPredepositCashList(ParaCrPredepositCashFilter filter)
        {
            return ICrPredepositCashDao.Instance.GetCrPredepositCashList(filter);
        }

        /// <summary>
        /// 更新提现状态
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        public void UpdatePdcPayState(int SysNo, int PdcPayState)
        {
            ICrPredepositCashDao.Instance.UpdatePdcPayState(SysNo, PdcPayState);
        }

        /// <summary>
        /// 插入记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <remarks>2016-1-6 王耀发 创建</remarks>
        public Result Insert(CrPredepositCash model)
        {
            Result r = new Result()
            {
                Status = false
            };
            ICrPredepositCashDao.Instance.Insert(model);
            r.Status = true;
            return r;
        }

        /// <summary>
        /// 更新提现订单审核状态
        /// </summary>
        /// <param name="sysNo">提现订单系统编号</param>
        /// <param name="status">审核状态（0:未审核 1:已审核 -1 作废）</param>
        /// <returns></returns>
        /// <remarks>2016-1-10 杨浩 创建</remarks>
        public bool UpdateStatus(int sysNo,int status)
        {
           return ICrPredepositCashDao.Instance.UpdateStatus(sysNo,status);
        }
        /// <summary>
        /// 审核提现订单
        /// </summary>
        /// <param name="sysNo">提现订单系统编号</param>
        /// <returns></returns>
        /// <remarks>2016-1-10 杨浩 创建</remarks>
        public bool AduitPredepositCash(int sysNo)
        {
            using (var tran = new TransactionScope())
            {
                ICrPredepositCashDao.Instance.UpdateStatus(sysNo,1);
                #region 付款单

                var predepositCashInfo = GetModel(sysNo);
                var pvEntity = new FnPaymentVoucher();
                pvEntity.Source = (int)FinanceStatus.付款来源类型.会员提现单;
                pvEntity.SourceSysNo = sysNo;
                pvEntity.PayableAmount = predepositCashInfo.PdcAmount;
                pvEntity.PaidAmount = 0;
                pvEntity.CustomerSysNo = predepositCashInfo.PdcUserId;
                pvEntity.RefundBank = predepositCashInfo.PdcToBank;
                pvEntity.RefundAccountName = predepositCashInfo.PdcToName;
                pvEntity.RefundAccount = predepositCashInfo.PdcCashAccount;
                pvEntity.Status = (int)FinanceStatus.付款单状态.待付款;
                pvEntity.CreatedDate = DateTime.Now;
                pvEntity.PayDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
                pvEntity.LastUpdateDate = DateTime.Now;
                pvEntity.PaymentType = predepositCashInfo.PdcPaymentId;
                var paymentVoucherItem = new List<FnPaymentVoucherItem>();

                #region 提现到余额
                if (predepositCashInfo.PdcPaymentId == (int)FinanceStatus.付款单付款方式.余额)
                {
                    var customerInfo = BLL.CRM.CrCustomerBo.Instance.GetModel(pvEntity.CustomerSysNo);

                    pvEntity.Status = (int)FinanceStatus.付款单状态.已付款;
                    pvEntity.PayDate = DateTime.Now;
                    pvEntity.PayableAmount = 0;
                    pvEntity.PaidAmount = predepositCashInfo.PdcAmount;

                    var item = new FnPaymentVoucherItem();
                    item.PaymentType = pvEntity.PaymentType;
                    item.Amount = pvEntity.PayableAmount;
                    item.VoucherNo = Guid.NewGuid().ToString("N");
                    item.Status = (int)FinanceStatus.付款单状态.已付款;
                    item.RefundBank = "转为商城余额";
                    item.RefundAccountName = customerInfo.NickName;
                    item.RefundAccount = customerInfo.Account;
                    item.CreatedDate = DateTime.Now;
                    item.PayDate = DateTime.Now;
                    item.LastUpdateDate = DateTime.Now;
                    paymentVoucherItem.Add(item);

                    //充值记录
                    var model = new CrRecharge();
                    model.TradeNo = Guid.NewGuid().ToString("N"); //"RechargeWX_" + dtNow.ToString("yyMMddHHmmssfff");
                    model.OutTradeNo = predepositCashInfo.PdcOutTradeNo;
                    model.CustomerSysNo = customerInfo.SysNo;
                    model.ReAmount = predepositCashInfo.PdcAmount;
                    model.RePaymentName = "佣金转余额";
                    model.RePaymentId = Hyt.Model.SystemPredefined.PaymentType.转账;
                    model.ReAddTime = DateTime.Now;
                    model.State = 1;
                    model.ReMark = "佣金：" + predepositCashInfo.PdcAmount + "元,转为余额";

                    int res = Hyt.BLL.Balance.CrRechargeBo.Instance.CreateCrRecharge(model);

                    //更新余额
                    int isb = Hyt.BLL.Balance.CrRechargeBo.Instance.IsExistenceABalance(customerInfo.SysNo);
                    if (isb == 0)
                    {
                        var balance = new CrAccountBalance();
                        balance.CustomerSysNo = customerInfo.SysNo;
                        balance.AvailableBalance = predepositCashInfo.PdcAmount;
                        balance.FrozenBalance = 0M;
                        balance.TolBlance = predepositCashInfo.PdcAmount;
                        balance.Remark = "";
                        balance.State = 0;
                        balance.AddTime = DateTime.Now;
                        int ba = Hyt.BLL.Balance.CrRechargeBo.Instance.CreateCrAccountBalance(balance);
                    }
                    else
                    {
                        //更新会员余额
                        Hyt.BLL.Balance.CrRechargeBo.Instance.UpdateAccountBalance(predepositCashInfo.PdcAmount, customerInfo.SysNo);
                    }

                    BLL.CRM.CrCustomerBo.Instance.UpdateCustomerBrokerageFreeze(customerInfo.SysNo, predepositCashInfo.PdcAmount);
                }
                #endregion

                int voucher = Hyt.BLL.Finance.FinanceBo.Instance.CreatePaymentVoucher(pvEntity, paymentVoucherItem);
                tran.Complete();

                #endregion

                return true;
            }
        }

        /// <summary>
        /// 获取提现订单详情
        /// </summary>
        /// <param name="sysNo">提现订单系统编号</param>
        /// <returns></returns>
        /// <remarks>2016-1-10 杨浩 创建</remarks>
        public CrPredepositCash GetModel(int sysNo)
        {
            return ICrPredepositCashDao.Instance.GetModel(sysNo);
        }
        #endregion
    }
}
