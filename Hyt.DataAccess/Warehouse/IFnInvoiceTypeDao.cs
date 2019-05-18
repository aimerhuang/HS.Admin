using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.Model;

namespace Hyt.DataAccess.Warehouse
{
    /// <summary>
    /// 发票类型维护抽象
    /// </summary>
    /// <remarks>2013-07-10 周瑜 创建</remarks>
    public abstract class IFnInvoiceTypeDao : DaoBase<IFnInvoiceTypeDao>
    {

        /// <summary>
        /// 根据系统编号获取发票类型
        /// </summary>
        /// <param name="sysNo">发票类型系统编号.</param>
        /// <returns>发票类型实体</returns>
        /// <remarks>2013-07-10 周瑜 创建 </remarks>
        public abstract FnInvoiceType GetModel(int sysNo);

        /// <summary>
        /// 获取所有发票类型
        /// </summary>
        /// <returns>发票类型实体</returns>
        /// <remarks>2013-07-10 周瑜 创建</remarks>
        public abstract IList<FnInvoiceType> GetAll();
    }
}
