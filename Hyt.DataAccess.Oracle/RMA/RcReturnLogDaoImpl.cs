using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.RMA;
using Hyt.Model;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.Oracle.RMA
{
    /// <summary>
    /// 退换货日志
    /// </summary>
    /// <remarks>2013-07-11 朱成果 创建</remarks>
    public class RcReturnLogDaoImpl : IRcReturnLogDao
    {
        /// <summary>
        /// 插入退货日志
        /// </summary>
        /// <param name="model">退换货日志</param>
        /// <returns>新增编号</returns>
        /// <remarks>2013-07-11 朱成果 创建</remarks>
        public override int Insert(RcReturnLog model)
        {
            var sysNO = Context.Insert("RcReturnLog", model)
                                       .AutoMap(o => o.SysNo)
                                       .ExecuteReturnLastId<int>("SysNo");
            return sysNO;
        }

        /// <summary>
        /// 获取退换货日志
        /// </summary>
        /// <param name="returnSysNo">退换货编号</param>
        /// <returns>退换货日志</returns>
        /// <remarks>2013-07-11 朱成果 创建</remarks>
        public override List<CBRcReturnLog> GetListByReturnSysNo(int returnSysNo)
        {
          return   Context.Sql("select t1.*,t2.UserName from RcReturnLog  t1  left outer join syuser t2 on t1.operator=t2.sysNo   where t1.ReturnSysNo=@ReturnSysNo order by OperateDate").Parameter("ReturnSysNo", returnSysNo).QueryMany<CBRcReturnLog>();
        }

        /// <summary>
        /// 获取退换货日志
        /// </summary>
        /// <param name="transactionSysNo">事物编号</param>
        /// <returns>退换货日志</returns>
        /// <remarks>2013-07-11 朱成果 创建</remarks>
        public override List<CBRcReturnLog> GetListByTransactionSysNo(string transactionSysNo)
        {
            return Context.Sql("select t1.*,t2.UserName from RcReturnLog  t1  left outer join syuser t2 on t1.operator=t2.sysNo   where t1.TransactionSysNo=@TransactionSysNo order by OperateDate").Parameter("TransactionSysNo", transactionSysNo).QueryMany<CBRcReturnLog>();
        }
    }
}
