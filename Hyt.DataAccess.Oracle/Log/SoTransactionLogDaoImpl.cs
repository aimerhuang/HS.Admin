using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Log;
using Hyt.Model;

namespace Hyt.DataAccess.Oracle.Log
{
    /// <summary>
    /// 订单日志数据访问  
    /// </summary>
    /// <remarks>2013-06-27 吴文强 创建</remarks>
    public class SoTransactionLogDaoImpl : ISoTransactionLogDao
    {
        /// <summary>
        /// 获取订单日志分页数据
        /// </summary>
        /// <param name="transactionSysNo">事物编号</param>
        /// <param name="pager">分页设置</param>
        /// <returns>订单日志分页数据</returns>
        /// <remarks>2013-06-20 朱成果 创建</remarks>
        public override void GetPageData(string transactionSysNo, ref Model.Pager<Model.SoTransactionLog> pager)
        {
            #region Sql 获取订单日志总数

            const string sql = @"
                select count(1) 
                from SOTRANSACTIONLOG 
                where TransactionSysNo=@0
            ";
            #endregion

            pager.TotalRows = Context.Sql(sql, transactionSysNo)
                                     .QuerySingle<int>();

            var list = Context.Select<Model.SoTransactionLog>("p.*")
                              .From("SOTRANSACTIONLOG p")
                              .Where("TransactionSysNo=@TransactionSysNo")
                              .OrderBy("OperateDate asc,p.sysno asc")
                              .Paging(pager.CurrentPage, pager.PageSize)
                              .Parameter("TransactionSysNo", transactionSysNo)
                              .QueryMany();
            pager.Rows = list;
        }

        /// <summary>
        /// 获取订单日志分页数据
        /// </summary>
        /// <param name="sysNo">销售单系统编号</param>
        /// <param name="pager">分页设置</param>
        /// <returns></returns>
        /// <remarks>2013-06-21 朱成果 创建</remarks>
        /// <remarks>2014-04-11 余勇 修改(添加物流日志)</remarks>
        public override void GetPageDataByOrderID(int sysNo, ref Pager<SoTransactionLog> pager)
        {
            #region Sql 获取订单日志总数

            const string sql = @"(select * from (
                  select  p.sysno,p.transactionsysno as transactionsysno,p.logcontent,p.operator,p.operatedate,1 sort
                    from sotransactionlog p
                      inner join soorder f
                        on p.transactionsysno=f.transactionsysno
                    where f.sysno=@sysno
                    union 
                      select t.sysno,t.areaname as transactionsysno,t.logcontext as logcontent,t.areaname as operator,t.logtime as operatedate,2 sort
                         from  lgexpresslog t
                         where t.expressinfosysno 
                          in(
                          select a.sysno from lgexpressinfo a 
                          inner join soorder f
                                on a.transactionsysno=f.transactionsysno
                                where f.sysno=@sysno
                         )
              ) a) d
            ";
            #endregion

            pager.TotalRows = Context.Select<int>("count(1)")
                .From(sql)
                .Parameter("sysno", sysNo)
                .QuerySingle();

            var list = Context.Select<SoTransactionLog>("sysno,transactionsysno,logcontent,operator,operatedate")
                              .From(sql)
                              .Parameter("sysno", sysNo)
                              .OrderBy("sort,operatedate")
                              .Paging(pager.CurrentPage, pager.PageSize)
                              .QueryMany();
            pager.Rows = list;
        }

        /// <summary>
        /// 创建订单日志
        /// </summary>
        /// <param name="log">日志</param>
        /// <returns>日志sysNo</returns>
        /// <remarks>2013-06-19 朱家宏 创建</remarks>
        public override int CreateSoTransactionLog(SoTransactionLog log)
        {
            var sysNo = Context.Insert("SoTransactionLog", log)
                             .AutoMap(o => o.SysNo)
                             .ExecuteReturnLastId<int>("SysNo");
            return sysNo;
        }
    }
}
