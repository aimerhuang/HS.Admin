using System.Collections.Generic;
using Hyt.DataAccess.Warehouse;
using Hyt.Model;

namespace Hyt.DataAccess.Oracle.Warehouse
{
    /// <summary>
    /// 发票类型数据层实现
    /// </summary>
    /// <remarks>2013-07-10 周瑜 创建</remarks>
    public class FnInvoiceTypeDaoImpl : IFnInvoiceTypeDao
    {

        /// <summary>
        /// 根据系统编号获取发票类型
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>发票类型实体</returns>
        /// <remarks>2013-07-10 周瑜 创建</remarks>
        public override FnInvoiceType GetModel(int sysNo)
        {
            var model = Context.Sql("select * from FnInvoiceType where sysno= @sysno")
                                     .Parameter("sysno", sysNo)
                                     .QuerySingle<FnInvoiceType>();
            return model;
        }

        /// <summary>
        /// 获取所有发票类型
        /// </summary>
        /// <returns>发票类型实体</returns>
        /// <remarks>2013-07-10 周瑜 创建</remarks>
        public override IList<FnInvoiceType> GetAll()
        {
            var model = Context.Sql("select * from FnInvoiceType")
                               .QueryMany<FnInvoiceType>();
            return model;
        }
    }
}
