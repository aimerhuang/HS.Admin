using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model.Parameter;
using Hyt.Util;
using Hyt.Model;
using Hyt.DataAccess.Web;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Oracle.Web
{
    /// <summary>
    /// 积分数据访问  
    /// </summary>
    /// <remarks>2013-12-01 苟治国 创建</remarks>
    public class CrAvailablePointLogDaoImpl:ICrAvailablePointLogDao
    {
        /// <summary>
        /// 会员可用积分日志
        /// </summary>
        /// <param name="pager">条件</param>
        /// <returns>可用积分日志列表</returns>
        /// <remarks>2013-12-01 苟治国 创建</remarks>
        public override Pager<Model.CrAvailablePointLog> GetPager(Pager<CrAvailablePointLog> pager, ParaCrAvailablePointLogFilter apl)
        {
            #region sql条件
            string sqlWhere = @"(@customersysnos=-1 or cap.customersysno =@customersysno) and (@beginTime is null or cap.CreatedDate>=@beginTime) and (@endTime is null or cap.CreatedDate<=@endTime) and cap.Decreased <> 0";
            #endregion
            using (var context = Context.UseSharedConnection(true))
            {
                pager.Rows = context.Select<CrAvailablePointLog>("cap.*")
                                    .From("CrAvailablePointLog cap")
                                    .Where(sqlWhere)
                                    .Parameter("customersysno", pager.PageFilter.CustomerSysNo)
                                    .Parameter("beginTime", apl.BeginDate)
                                    .Parameter("endTime", apl.EndDate)
                                    .Paging(pager.CurrentPage, pager.PageSize).OrderBy("cap.ChangeDate desc").QueryMany();

                pager.TotalRows = context.Select<int>("count(1)")
                                    .From("CrAvailablePointLog cap")
                                    .Where(sqlWhere)
                                    .Parameter("customersysno", pager.PageFilter.CustomerSysNo)
                                    .Parameter("beginTime", apl.BeginDate)
                                    .Parameter("endTime", apl.EndDate)
                                    .QuerySingle();
            }
            return pager;
        }

        public override List<CrAvailablePointLog> GetAll()
        {
            string sql = " select * from CrAvailablePointLog ";
            return Context.Sql(sql).QueryMany<CrAvailablePointLog>();
        }

        public override void DeleteData(int p)
        {
            string sql = " delete from CrAvailablePointLog where SysNo='" + p + "' ";
            Context.Sql(sql).Execute();
        }
    }
}
