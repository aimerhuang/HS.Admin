using Hyt.DataAccess.Logistics;
using Hyt.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Oracle.Logistics
{
    /// <summary>
    /// 物流日志
    /// </summary>
    /// <remarks>2014-1-15 沈强 创建</remarks>
    public class MkExpressLogDaoImpl : IMkExpressLogDao
    {
        /// <summary>
        /// 获取物流日志
        /// </summary>
        /// <param name="pagerFilter">查询过滤对象</param>
        /// <returns>返回物流日志集合</returns>
        /// <remarks>2013-12-19 沈强 创建</remarks>
        /// <remarks>2014-6-9 何明壮 修改</remarks>
        public override Pager<MkExpressLog> GetLogisticsDeliveryItems(Pager<MkExpressLog> pagerFilter)
        {
            var filter = pagerFilter.PageFilter;
            var pager = new Pager<MkExpressLog>
            {
                CurrentPage = pagerFilter.CurrentPage,
                PageSize = pagerFilter.PageSize
            };
            #region 查询sql
//            var selectSql = @"select * from
//                                (
//                                   select b.*
//                                        ,row_number() over (order by b.sysno desc) FLUENTDATA_ROWNUMBER
//                                    from MkExpressLog b
//                                   where exists (select 1 from(select max(a.sysno) as sysno from MkExpressLog a group by a.expressno) c where b.sysno = c.sysno)
//                                         and (:ExpressNo is null or b.expressno = :ExpressNo)
//                                )
//                                where fluentdata_RowNumber between {0} and {1}
//                                  order by fluentdata_RowNumber";

            var selectSql = @"select * from MkExpressLog where sysno in
                                (
                                    select sysno from
                                    (
                                        select b.*,row_number() over (order by b.sysno desc) FLUENTDATA_ROWNUMBER from 
                                        (
                                            select e.expressno,max(e.sysno) sysno from MkExpressLog e  
                                                where (@0 is null or e.expressno = @0)
                                                group by e.expressno
                                        )b
                                    ) tb
                                    where FLUENTDATA_ROWNUMBER between {0} and {1}
                                )
                              order by sysno desc";

            var countSql = @"select count(distinct expressno)
                            from MkExpressLog b
                            where (@0 is null or b.expressno = @0)";
            #endregion

            #region 设置默认参数

            int beginNum = pager.PageSize * (pager.CurrentPage - 1) + 1;
            int endNum = beginNum + pager.PageSize - 1;

            selectSql = string.Format(selectSql, beginNum, endNum);

            #endregion
            using (var context = Context.UseSharedConnection(true))
            {
                #region 设置查询参数
                var param = new object[]
                    {
                        filter.ExpressNo
                    };
                #endregion

                pager.TotalRows = context.Sql(countSql).Parameters(param).QuerySingle<int>();

                pager.Rows = context.Sql(selectSql).Parameters(param).QueryMany<MkExpressLog>();
            }
            return pager;
        }

        /// <summary>
        /// 根据物流单号，获取物流日志
        /// </summary>
        /// <param name="expressNo">物流单号</param>
        /// <returns>返回物流日志表集合</returns>
        /// <remarks>2013-12-19 沈强 创建</remarks>
        public override IList<MkExpressLog> GetMkExpressLogList(string expressNo)
        {
            string sql = "select * from MkExpressLog a where a.expressno = @0";
            return Context.Sql(sql, expressNo).QueryMany<MkExpressLog>();

        }
        /// <summary>
        /// 批量插入物流日志
        /// </summary>
        /// <param name="logs">物流日志集合</param>
        /// <returns></returns>
        /// <remarks>2013-12-19 沈强 创建</remarks>
        public override void Insert(List<MkExpressLog> logs)
        {
            using (var tran = Context.UseTransaction(true))
            {
                foreach (var log in logs)
                {
                    tran.Insert<MkExpressLog>("MkExpressLog", log).AutoMap(m => m.SysNo).Execute();
                }
                tran.Commit();
            }
        }
    }
}
