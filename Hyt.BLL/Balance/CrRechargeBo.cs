using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using Hyt.Infrastructure.Memory;
using Hyt.Model;
using Hyt.DataAccess.BaseInfo;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;
using Hyt.Infrastructure.Caching;
using Hyt.Util;
using Hyt.DataAccess.Balance;
using Hyt.Infrastructure.Pager;
using Hyt.Model.Exception;

namespace Hyt.BLL.Balance
{
    /// <summary>
    /// 充值
    /// </summary>
    /// <remarks>2016-06-06 周 创建</remarks>
    public class CrRechargeBo : BOBase<CrRechargeBo>
    {
        /// <summary>
        /// 订单消费余额
        /// </summary>
        /// <param name="customerSysNo">客户编号</param>
        /// <param name="orderSysNo">订单编号</param>
        /// <param name="amount">消费余额 不能为0</param>
        /// <param name="transactionSysNo">事务编号</param>
        /// <exception cref="Exception"></exception>
        /// <returns>void</returns>
        /// <remarks>2017-01-17 杨浩 创建</remarks>
        public void OrderDeductionBalance(int customerSysNo, int orderSysNo, decimal amount, string transactionSysNo)
        {
            if (amount == 0) throw new Exception("会员余额不能为0.");
            var balanceInfo = GetCrABalanceEntity(customerSysNo);        
            if (balanceInfo== null)
            {
                throw new Hyt.Model.Exception.UserNotMatchException(customerSysNo);
            }

            if (balanceInfo.AvailableBalance < amount)
            {
                throw new Exception("会员余额不足.");
            }

           
            var  model = new CrBalancePayOrderLog()
            {
                CustomerSysNo = customerSysNo,
                OrderSysNo = orderSysNo,
                PayAmount = amount,
                MemberBalance = balanceInfo.AvailableBalance-amount,
                State=1,
                PayType=Hyt.Model.SystemPredefined.PaymentType.余额支付,
                PayTime=DateTime.Now,
                Remark = "订单交易使用,订单号:" + orderSysNo.ToString(),
            };

            UpdateABalanceForPayOrder(amount,customerSysNo);
            CreateCrBalancePayOrderLog(model);
        }
        /// <summary>
        /// 充值
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int CreateCrRecharge(CrRecharge model)
        {
            return CrRechargeDao.Instance.CreateCrRecharge(model);
        }
        /// <summary>
        /// 充值详情
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public CrRecharge GetCrRechargeEntity(int SysNo)
        {
            return CrRechargeDao.Instance.GetCrRechargeEntity(SysNo);
        }
        /// <summary>
        /// 更新充值记录
        /// </summary>
        /// <param name="rSysNo"></param>
        /// <param name="State"></param>
        /// <param name="ReAmount"></param>
        /// <param name="OutTradeNo"></param>
        /// <returns></returns>
        public int UpdateCrRecharge(int rSysNo, int State, decimal ReAmount, string OutTradeNo)
        {
            return CrRechargeDao.Instance.UpdateCrRecharge(rSysNo, State, ReAmount, OutTradeNo);
        }
        /// <summary>
        /// 更新充值记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateCrRecharge(CrRecharge model)
        {
            return CrRechargeDao.Instance.UpdateCrRecharge(model);
        }
        /// <summary>
        /// 充值记录
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        public Pager<CrRecharge> GetCrRechargeList(Pager<CrRecharge> pager)
        {
            return CrRechargeDao.Instance.GetCrRechargeList(pager);
        }
        /// <summary>
        /// 创建余额记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int CreateCrAccountBalance(CrAccountBalance model)
        {
            return CrRechargeDao.Instance.CreateCrAccountBalance(model);
        }
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="CustomerSysNo"></param>
        /// <returns></returns>
        public CrAccountBalance GetCrABalanceEntity(int CustomerSysNo)
        {
            return CrRechargeDao.Instance.GetCrABalanceEntity(CustomerSysNo);
        }
        /// <summary>
        /// 是否存在会员记录
        /// </summary>
        /// <param name="CustomerSysNo"></param>
        /// <returns></returns>
        public int IsExistenceABalance(int CustomerSysNo)
        {
            return CrRechargeDao.Instance.IsExistenceABalance(CustomerSysNo);
        }
        /// <summary>
        /// 更新会员余额
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateABalance(CrAccountBalance model)
        {
            return CrRechargeDao.Instance.UpdateABalance(model);
        }
        /// <summary>
        /// 更新会员余额
        /// </summary>
        /// <param name="AvailableBalance"></param>
        /// <param name="CustomerSysNo"></param>
        /// <returns></returns>
        public int UpdateAccountBalance(decimal AvailableBalance, int CustomerSysNo)
        {
            return CrRechargeDao.Instance.UpdateAccountBalance(AvailableBalance, CustomerSysNo);
        }
        /// <summary>
        /// 更新会员余额
        /// </summary>
        /// <param name="AvailableBalance"></param>
        /// <param name="CustomerSysNo"></param>
        /// <returns></returns>
        public int UpdateABalanceForPayOrder(decimal AvailableBalance, int CustomerSysNo)
        {
            return CrRechargeDao.Instance.UpdateABalanceForPayOrder(AvailableBalance, CustomerSysNo);
        }
        /// <summary>
        /// 增加会员余额支付订单记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int CreateCrBalancePayOrderLog(CrBalancePayOrderLog model)
        {
            return CrRechargeDao.Instance.CreateCrBalancePayOrderLog(model);
        }

        /// <summary>
        /// 充值记录
        /// </summary>
        /// <param name="customerSysNo"></param>
        /// <param name="changeType"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public PagedList<CrRecharge> GetCrRechargeLog(int customerSysNo, int? changeType, int? pageIndex)
        {
            if (changeType == null)
            {
                changeType = -1;
            }
            var returnValue = new PagedList<CrRecharge>();

            var pager = new Pager<CrRecharge>
            {
                PageFilter = new CrRecharge
                {
                    CustomerSysNo = customerSysNo,
                    State = changeType ?? 0
                },
                CurrentPage = pageIndex ?? 1,
                PageSize = returnValue.PageSize
            };
            Hyt.DataAccess.Balance.CrRechargeDao.Instance.GetCrRechargeLog(ref pager);

            returnValue.TData = pager.Rows;
            returnValue.CurrentPageIndex = pager.CurrentPage;
            returnValue.TotalItemCount = pager.TotalRows;

            return returnValue;
        }

        /// <summary>
        /// 是否存在会员保证金返利记录
        /// </summary>
        /// <param name="CustomerSysNo">用户ID</param>
        /// <param name="tradeNo">外部订单号</param>
        /// <returns>2016-7-6 罗远康 创建</returns>
        public int IsDealerCrRecharge(int CustomerSysNo, string OutTradeNo)
        {
            return CrRechargeDao.Instance.IsDealerCrRecharge(CustomerSysNo, OutTradeNo);
        }

        /// <summary>
        /// 分页查询充值记录
        /// </summary>
        /// <param name="pager">分页</param>
        /// <param name="filter">查询条件</param>
        /// <remarks> 2016-08-17 刘伟豪 创建 </remarks>
        public void Search(ref Pager<CBCrRecharge> pager, ParaCrRechargeFilter filter)
        {
            CrRechargeDao.Instance.Search(ref pager, filter);
        }
    }
}