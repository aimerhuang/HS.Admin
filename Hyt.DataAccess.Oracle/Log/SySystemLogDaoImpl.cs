using Hyt.DataAccess.Log;
using Hyt.Model;
using Hyt.Model.Parameter;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Hyt.DataAccess.Oracle.Log
{
    /// <summary>
    /// 系统日志数据访问  
    /// </summary>
    /// <remarks>2013-06-27 吴文强 创建</remarks>
    public class SySystemLogDaoImpl : ISySystemLogDao
    {
        /// <summary>
        /// 创建日志
        /// </summary>
        /// <param name="log">model</param>
        /// <returns></returns>
        /// <remarks>2013-08-14 朱家宏 创建</remarks>
        public override void Create(Model.SySystemLog log)
        {

            Context.Insert("SySystemLog", log)
                   .AutoMap(o => o.SysNo)
                   .Execute();
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns>分页</returns>
        /// <remarks>2013-08-14 朱家宏 创建</remarks>
        public override Pager<SySystemLog> Query(ParaSystemLogFilter filter)
        {
            var sql = @"SySystemLog a 
                         {0} ";

            #region 构造sql

            var paras = new ArrayList();
            var where = "";
            int i = 0;
            if (filter.Operator.HasValue)
            {
                where += (string.IsNullOrWhiteSpace(where) ? "" : " and") + " a.Operator=@p0p" + i;
                paras.Add(filter.Operator);
                i++;
            }
            if (!string.IsNullOrWhiteSpace(filter.LogIp))
            {
                where += (string.IsNullOrWhiteSpace(where) ? "" : " and") + " a.LogIp = @p0p" + i;
                paras.Add(filter.LogIp);
                i++;
            }
            if (filter.LogLevels != null && filter.LogLevels.Any())
            {
                var logLevels = string.Join(",", filter.LogLevels);
                where += (string.IsNullOrWhiteSpace(where)
                    ? ""
                    : " and") +
                      " exists (select 1 from splitstr(@p0p" + i + ",',') tmp where tmp.col = a.LogLevel)";
                paras.Add(logLevels);
                i++;
            }
            if (filter.Sources != null && filter.Sources.Any())
            {
                var sources = string.Join(",", filter.Sources);
                where += (string.IsNullOrWhiteSpace(where)
                    ? ""
                    : " and") +
                      " exists (select 1 from splitstr(@p0p" + i + ",',') tmp where tmp.col = a.Source)";
                paras.Add(sources);
                i++;
            }
            if (filter.BeginDate.HasValue)
            {
                //有效时间(起)
                where += (string.IsNullOrWhiteSpace(where) ? "" : " and") + " a.logDate>=@p0p" + i;
                paras.Add(filter.BeginDate);
                i++;
            }
            if (filter.EndDate.HasValue)
            {
                //有效时间(止) 
                where += (string.IsNullOrWhiteSpace(where) ? "" : " and") + " a.logDate<@p0p" + i;
                paras.Add(filter.EndDate);
                i++;
            }
            if (!string.IsNullOrWhiteSpace(filter.Message))
            {
                where += (string.IsNullOrWhiteSpace(where) ? "" : " and") + " a.Message LIKE @p0p" + i;
                paras.Add("%" + filter.Message + "%");
                i++;
            }
            if (filter.TargetType.HasValue)
            {
                where += (string.IsNullOrWhiteSpace(where) ? "" : " and") + " a.TargetType=@p0p" + i;
                paras.Add(filter.TargetType);
                i++;
            }
            if (filter.TargetSysNo.HasValue)
            {
                where += (string.IsNullOrWhiteSpace(where) ? "" : " and") + " a.TargetSysNo=@p0p" + i;
                paras.Add(filter.TargetSysNo);
                i++;
            }

            sql = string.Format(sql, (string.IsNullOrWhiteSpace(where) ? "" : " where ") + where);


            #endregion

            var dataList = Context.Select<SySystemLog>("a.*").From(sql);
            var dataCount = Context.Select<int>("count(0)").From(sql);

            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<SySystemLog>
            {
                PageSize = filter.PageSize,
                CurrentPage = filter.Id,
                TotalRows = dataCount.QuerySingle(),
                Rows = dataList.OrderBy("a.sysno desc").Paging(filter.Id, filter.PageSize).QueryMany()
            };

            return pager;
        }

        //        /// <summary>
        //        /// 分页查询
        //        /// </summary>
        //        /// <param name="filter">查询参数</param>
        //        /// <returns>分页</returns>
        //        /// <remarks>2013-08-14 朱家宏 创建</remarks>
        //        [Obsolete]
        //        private Pager<SySystemLog> _Query(ParaSystemLogFilter filter)
        //        {
        //            const string sql = @"(select a.* from SySystemLog a 
        //                                left join syUser b on a.operator=b.sysno 
        //                                where 
        //                                (:operator is null or charindex(b.username,:operator)>0) and 
        //                                (:LogIp is null or charindex(a.LogIp,:LogIp)>0) and 
        //                                (:LogLevels is null or exists (select 1 from table(splitstr(:LogLevels,',')) tmp where tmp.column_value = a.LogLevel)) and      --日志级别
        //                                (:Sources is null or exists (select 1 from table(splitstr(:Sources,',')) tmp where tmp.column_value = a.Source)) and            --日志来源
        //                                (:BeginDate is null or a.logDate>=:BeginDate) and                                                                               --日期(起)
        //                                (:EndDate is null or a.logDate<:EndDate) and                                                                                        --日期(止) 
        //                                (:Message is null or charindex(a.Message,:Message)>0) and 
        //                                (:TargetType is null or a.TargetType=:TargetType) and
        //                                (:TargetSysNo is null or a.TargetSysNo=:TargetSysNo)
        //                                ) tb";

        //            var logLevels = filter.LogLevels != null ? string.Join(",", filter.LogLevels) : null;
        //            var sources = filter.Sources != null ? string.Join(",", filter.Sources) : null;

        //            var paras = new object[]
        //                {
        //                    filter.Operator,    filter.Operator,
        //                    filter.LogIp,       filter.LogIp,
        //                    logLevels,          logLevels,
        //                    sources,            sources,
        //                    filter.BeginDate,   filter.BeginDate,
        //                    filter.EndDate,     filter.EndDate,
        //                    filter.Message,     filter.Message,
        //                    filter.TargetType,  filter.TargetType,
        //                    filter.TargetSysNo, filter.TargetSysNo
        //                };

        //            var dataList = Context.Select<SySystemLog>("tb.*").From(sql);
        //            var dataCount = Context.Select<int>("count(0)").From(sql);

        //            dataList.Parameters(paras);
        //            dataCount.Parameters(paras);

        //            var pager = new Pager<SySystemLog>
        //            {
        //                PageSize = filter.PageSize,
        //                CurrentPage = filter.Id,
        //                TotalRows = dataCount.QuerySingle(),
        //                Rows = dataList.OrderBy("tb.logDate desc").Paging(filter.Id, filter.PageSize).QueryMany()
        //            };

        //            return pager;
        //        }

        /// <summary>
        /// 获取日志信息
        /// </summary>
        /// <param name="source">系统日志来源</param>
        /// <param name="targetType">系统日志目标类型</param>
        /// <param name="targetSysNo">来源系统编号</param>
        /// <returns>系统日志列表</returns>
        /// <remarks>2013-09-09 沈强 创建</remarks>
        public override IList<SySystemLog> Get(Model.WorkflowStatus.LogStatus.系统日志来源 source,
                                      Model.WorkflowStatus.LogStatus.系统日志目标类型 targetType, int targetSysNo)
        {
            string sql = @"select * from SySystemLog a  
            where a.targettype = @0 
                    and a.source = @1 
                    and a.targetsysno = @2 
                    order by a.logdate desc";

            return Context.Sql(sql)
                .Parameters((int)targetType, (int)source, targetSysNo)
                .QueryMany<SySystemLog>();
        }
    }
}
