using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.Warehouse
{
    /// <summary>
    /// 发票维护抽象
    /// </summary>
    /// <remarks>2013-07-10 周瑜 创建</remarks>
    public abstract class IInvoiceDao : DaoBase<IInvoiceDao>
    {
        /// <summary>
        /// 根据输入条件查询发票列表
        /// </summary>
        /// <param name="condition">搜索条件</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>发票实体列表</returns>
        /// <remarks>2013-07-09 周瑜 创建</remarks>
        public abstract Pager<CBFnInvoice> Search(InvoiceSearchCondition condition, int pageSize);

        /// <summary>
        /// 根据主键获取发票
        /// </summary>
        /// <param name="sysNo">发票系统编号.</param>
        /// <returns>发票实体</returns>
        /// <remarks>2013-07-10 周瑜 创建 </remarks>
        public abstract FnInvoice GetModel(int sysNo);

        /// <summary>
        /// 根据事务获取发票
        /// </summary>
        /// <param name="transactionSysNo">事务编号</param>
        /// <returns>发票实体</returns>
        /// <remarks>2013-11-12 周唐炬 创建</remarks>
        public abstract FnInvoice GetInvoiceByTransactionSysNo(string transactionSysNo);

        /// <summary>
        /// 修改发票状态
        /// </summary>
        /// <param name="fnInvoice">用于修改发票的实体</param>
        /// <returns>受影响的行数</returns>
        /// <remarks>2013-07-10 周瑜 创建</remarks>
        public abstract int Update(FnInvoice fnInvoice);

        /// <summary>
        /// 根据输入条件查询发票列表
        /// </summary>
        /// <param name="condition">搜索条件</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>发票实体列表</returns>
        /// <remarks>2013-07-09 周瑜 创建</remarks>
        public abstract Pager<SoOrder> QuickSearch(InvoiceSearchCondition condition, int pageSize);

    }
}
