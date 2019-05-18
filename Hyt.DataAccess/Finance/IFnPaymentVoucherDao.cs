using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.Finance
{

    /// <summary>
    /// 创建付款单
    /// </summary>
    /// <remarks>
    /// 2013-07-12 何方 创建
    /// </remarks>
    public abstract class IFnPaymentVoucherDao : DaoBase<IFnPaymentVoucherDao>
    {
        /// <summary>
        /// 添加付款單
        /// </summary>
        /// <param name="model">付款單實體.</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013/7/12 何方 创建
        /// </remarks>
        public abstract int Insert(FnPaymentVoucher model);

        /// <summary>
        /// 添加付款单明细
        /// </summary>
        /// <param name="model">付款单明细</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-7-12 何方 创建
        /// 2013-07-19 朱成果 修改
        /// </remarks>
        public abstract int InsertItem(FnPaymentVoucherItem model);

        /// <summary>
        /// 获取付款单明细
        /// </summary>
        /// <param name="sysNo">明细编号</param>
        /// <returns>付款单明细</returns>
        /// <remarks>2013-7-22 朱成果 创建 </remarks>
        public abstract FnPaymentVoucherItem GetVoucherItem(int sysNo);

        /// <summary>
        /// 更新付款单
        /// </summary>
        /// <param name="entity">付款单</param>
        /// <returns></returns>
        /// <remarks>2013-07-22  黄志勇 创建</remarks>
        public abstract void UpdateVoucher(FnPaymentVoucher entity);

        /// <summary>
        /// 更新付款单明细
        /// </summary>
        /// <param name="entity">付款单明细</param>
        /// <returns></returns>
        /// <remarks>2013-07-22  朱成果 创建</remarks>
        public abstract void UpdateVoucherItem(FnPaymentVoucherItem entity);

        /// <summary>
        /// 获取付款单详情(不包括明细)
        /// </summary>
        /// <param name="sysNo">付款单编号</param>
        /// <returns></returns>
        /// <remarks>2013-7-15 朱成果 创建 </remarks>
        public abstract CBFnPaymentVoucher GetEntity(int sysNo);

        /// <summary>
        /// 根据单据来源获取实体
        /// </summary>
        /// <param name="source">单据来源</param>
        /// <param name="sourceSysNo">单据编号</param>
        /// <returns></returns>
        /// <remarks>2013-11-8 朱成果 创建 </remarks>
        public abstract FnPaymentVoucher GetEntity(int source, int sourceSysNo);

        /// <summary>
        /// 获取付款明细
        /// </summary>
        /// <param name="paymentVoucherSysNo">付款单编号</param>
        /// <returns></returns>
        /// <remarks>2013-7-19 朱成果 创建 </remarks>
        public abstract List<FnPaymentVoucherItem> GetVoucherItems(int paymentVoucherSysNo);

        /// <summary>
        /// 以TransactionSysNo获取实体
        /// </summary>
        /// <param name="transactionSysNo">事务编号</param>
        /// <returns></returns>
        /// <remarks>2013-7-17 朱家宏 创建 </remarks>
        public abstract FnPaymentVoucher GetEntityByTransactionSysNo(string transactionSysNo);

        /// <summary>
        /// 根据单据来源获取实体(不包括明细)
        /// </summary>
        /// <param name="source">单据来源</param>
        /// <param name="sourceSysNo">单据编号</param>
        /// <returns></returns>
        /// <remarks>2013-7-25 朱家宏 创建 </remarks>
        public abstract CBFnPaymentVoucher GetEntityByVoucherSource(int source, int sourceSysNo);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns></returns>
        /// <remarks>2013-07-19 朱家宏 创建</remarks>
        public abstract Pager<CBPaymentVoucher> GetAll(ParaVoucherFilter filter);

    }
}
