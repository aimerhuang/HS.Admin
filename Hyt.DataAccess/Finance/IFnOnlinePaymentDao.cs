using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;
using System.Collections.Generic;

namespace Hyt.DataAccess.Finance
{
    /// <summary>
    /// 网上支付
    /// </summary>
    /// <remarks>2013-07-16 朱家宏 创建</remarks>
    public abstract class IFnOnlinePaymentDao : DaoBase<IFnOnlinePaymentDao>
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>分页</returns>
        /// <remarks>2013-07-16 朱家宏 创建</remarks>
        public abstract Pager<CBFnOnlinePayment> GetAll(ParaOnlinePaymentFilter filter);

        /// <summary>
        /// 插入记录
        /// </summary>
        /// <param name="model">插入对象</param>
        /// <returns>sysNo</returns>
        /// <remarks>2013-07-16 朱家宏 创建</remarks>
        public abstract int Insert(FnOnlinePayment model);
        /// <summary>
        /// 获取网上支付详情
        /// </summary>
        /// <param name="voucherNo">交易凭证</param>
        /// <returns></returns>
        /// <remarks>2015-12-26 杨浩 创建</remarks>
        public abstract FnOnlinePayment GetOnlinePaymentByVoucherNo(string voucherNo);
        /// <summary>
        /// 获取网上支付详情
        /// </summary>
        /// <param name="paymentTypeSysNo">支付类型编号</param>
        /// <param name="voucherNo">交易凭证</param>
        /// <returns></returns>
        /// <remarks>2016-09-10 杨浩 创建</remarks>
        public abstract FnOnlinePayment GetOnlinePaymentByVoucherNo(int paymentTypeSysNo,string voucherNo);
        /// <summary>
        /// 获取网上支付详情
        /// </summary>
        /// <param name="SourceSysNo">源单号</param>
        /// <returns></returns>
        /// <remarks>2016-4-19 王耀发 创建</remarks>
        public abstract FnOnlinePayment GetOnlinePaymentBySourceSysNo(int SourceSysNo);
        /// <summary>
        /// 通过订单来源和订单编号获取在线付款单数据
        /// </summary>
        /// <param name="Source">网上订单来源</param>
        /// <param name="SourceSysNo">订单编号</param>
        /// <returns>
        /// 2016-04-19 杨云奕 添加
        /// </returns>
        public abstract FnOnlinePayment GetOnlinePaymentBySourceSysNo(Model.WorkflowStatus.FinanceStatus.网上支付单据来源 Source, int SourceSysNo);

        /// <summary>
        /// 获取订单的在线支付记录
        /// </summary>
        /// <param name="orderSysNo"></param>
        /// <returns>sysNo</returns>
        /// <remarks>2014-05-13 何方 创建</remarks>
        public abstract IList<FnOnlinePayment> Get(int orderSysNo);

        /// <summary>
        /// 获取网上支付详情
        /// </summary>
        /// <param name="SourceSysNo">源单号</param>
        /// <returns></returns>
        /// <remarks>2016-4-19 王耀发 创建</remarks>
        public abstract CBFnOnlinePayment GetOnPaymentBySourceSysNo(int SourceSysNo);

        public abstract List<CBFnOnlinePayment> GetOnlinePaymentList(string SysNos);
        
    }
}
