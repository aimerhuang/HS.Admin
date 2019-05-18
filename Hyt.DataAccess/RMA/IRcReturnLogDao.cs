using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.RMA
{
    /// <summary>
    /// 退换货日志
    /// </summary>
    /// <remarks>2013-07-11 朱成果 创建</remarks>
    public  abstract class IRcReturnLogDao : DaoBase<IRcReturnLogDao>
    {
        /// <summary>
        /// 插入退货日志
        /// </summary>
        /// <param name="model">退换货日志</param>
        /// <returns>新增编号</returns>
        /// <remarks>2013-07-11 朱成果 创建</remarks>
        public abstract int Insert(RcReturnLog model);

        /// <summary>
        /// 获取退换货日志
        /// </summary>
        /// <param name="returnSysNo">退换货编号</param>
        /// <returns>退换货日志列表</returns>
        /// <remarks>2013-07-11 朱成果 创建</remarks>
        public abstract List<CBRcReturnLog> GetListByReturnSysNo(int returnSysNo);

        /// <summary>
        /// 获取退换货日志
        /// </summary>
        /// <param name="transactionSysNo">事物编号</param>
        /// <returns>退换货日志</returns>
        /// <remarks>2013-07-11 朱成果 创建</remarks>
        public abstract List<CBRcReturnLog> GetListByTransactionSysNo(string transactionSysNo);

    }
}
