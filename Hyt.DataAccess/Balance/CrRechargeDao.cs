using Hyt.DataAccess.Base;
using Hyt.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Balance
{
    /// <summary>
    /// 充值
    /// </summary>
    /// <remarks>2016-06-06 周 创建</remarks>
    public abstract class CrRechargeDao : DaoBase<CrRechargeDao>
    {
        /// <summary>
        /// 充值
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public abstract int CreateCrRecharge(CrRecharge model);
        /// <summary>
        /// 充值详情
        /// </summary>
        /// <param name="SysNo"></param>
        /// <returns></returns>
        public abstract CrRecharge GetCrRechargeEntity(int SysNo);
        /// <summary>
        /// 更新充值记录
        /// </summary>
        /// <param name="rSysNo"></param>
        /// <param name="State"></param>
        /// <param name="ReAmount"></param>
        /// <param name="OutTradeNo"></param>
        /// <returns></returns>
        public abstract int UpdateCrRecharge(int rSysNo, int State, decimal ReAmount, string OutTradeNo);
        /// <summary>
        /// 更新充值记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public abstract int UpdateCrRecharge(CrRecharge model);
        /// <summary>
        /// 充值记录
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        public abstract Pager<CrRecharge> GetCrRechargeList(Pager<CrRecharge> pager);
        /// <summary>
        /// 创建余额记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public abstract int CreateCrAccountBalance(CrAccountBalance model);
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="CustomerSysNo"></param>
        /// <returns></returns>
        public abstract CrAccountBalance GetCrABalanceEntity(int CustomerSysNo);
        /// <summary>
        /// 是否存在会员记录
        /// </summary>
        /// <param name="CustomerSysNo"></param>
        /// <returns></returns>
        public abstract int IsExistenceABalance(int CustomerSysNo);
        /// <summary>
        /// 更新会员余额
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public abstract int UpdateABalance(CrAccountBalance model);
        /// <summary>
        /// 更新会员余额
        /// </summary>
        /// <param name="AvailableBalance"></param>
        /// <param name="CustomerSysNo"></param>
        /// <returns></returns>
        public abstract int UpdateAccountBalance(decimal AvailableBalance, int CustomerSysNo);
        /// <summary>
        /// 更新会员余额
        /// </summary>
        /// <param name="AvailableBalance"></param>
        /// <param name="CustomerSysNo"></param>
        /// <returns></returns>
        public abstract int UpdateABalanceForPayOrder(decimal AvailableBalance, int CustomerSysNo);
        /// <summary>
        /// 增加会员余额支付订单记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public abstract int CreateCrBalancePayOrderLog(CrBalancePayOrderLog model);
        /// <summary>
        /// 充值记录
        /// </summary>
        /// <param name="pager"></param>
        public abstract void GetCrRechargeLog(ref Pager<CrRecharge> pager);

        /// <summary>
        /// 是否存在会员保证金返利记录
        /// </summary>
        /// <param name="CustomerSysNo">用户ID</param>
        /// <param name="tradeNo">外部订单号</param>
        /// <returns>2016-7-6 罗远康 创建</returns>
        public abstract int IsDealerCrRecharge(int CustomerSysNo, string OutTradeNo);

        /// <summary>
        /// 分页查询充值记录
        /// </summary>
        /// <param name="pager">分页</param>
        /// <param name="filter">查询条件</param>
        /// <remarks> 2016-08-17 刘伟豪 创建 </remarks>
        public abstract void Search(ref Pager<Model.Transfer.CBCrRecharge> pager, Model.Parameter.ParaCrRechargeFilter filter);
    }
}
