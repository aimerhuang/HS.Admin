using System.Collections.Generic;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Log
{
    /// <summary>
    /// 订单日志数据访问 抽象类 
    /// </summary>
    /// <remarks>2013-06-27 吴文强 创建</remarks>
    public abstract class ISoTransactionLogDao : Hyt.DataAccess.Base.DaoBase<ISoTransactionLogDao>
    {
        /// <summary>
        /// 获取订单日志分页数据
        /// </summary>
        /// <param name="transactionSysNo">事物编号</param>
        /// <param name="pager"></param>
        /// <returns></returns>
        /// <remarks>2013-06-20 朱成果 创建</remarks>
        public abstract void GetPageData(string transactionSysNo, ref Pager<SoTransactionLog> pager);

        /// <summary>
        /// 获取订单日志分页数据
        /// </summary>
        /// <param name="OrderID">订单编号</param>
        /// <param name="pager"></param>
        /// <returns></returns>
        /// <remarks>2013-06-21 朱成果 创建</remarks>
        public abstract void GetPageDataByOrderID(int OrderID, ref Pager<SoTransactionLog> pager);

        /// <summary>
        /// 创建订单日志
        /// </summary>
        /// <param name="log">日志</param>
        /// <returns>日志主键</returns>
        /// <remarks>2013-06-19 朱家宏 创建</remarks>
        public abstract int CreateSoTransactionLog(SoTransactionLog log);
    }
}
