using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess;
using Hyt.DataAccess.CRM;

namespace Hyt.DataAccess.Oracle.CRM
{
    /// <summary>
    /// 等级积分日志
    /// </summary>
    /// <remarks>2013-07-10 黄波 添加</remarks>
    public class CrLevelPointLogDaoImpl : ICrLevelPointLogDao
    {
        /// <summary>
        /// 获取等级积分日志详情
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>等级积分日志详情</returns>
        /// <remarks>2013-07-10 黄波 添加</remarks>
        public override Model.CrLevelPointLog GetCrLevelPointLogItem(int sysNo)
        {
            return Context.Select<Model.CrLevelPointLog>("select * from CrLevelPointLog where sysNo=@sysNo")
                .Parameter("@sysNo", sysNo)
                .QuerySingle();
        }

        /// <summary>
        /// 创建等级积分日志
        /// </summary>
        /// <param name="model">等级积分日志详情</param>
        /// <returns>创建结果</returns>
        /// <remarks>2013-07-10 黄波 添加</remarks>
        public override bool InsertCrLevelPointLogItem(Model.CrLevelPointLog model)
        {
            return Context.Insert("CrLevelPointLog", model)
                 .AutoMap(o => o.SysNo)
                 .ExecuteReturnLastId<int>("sysNo") > 0;
        }

        /// <summary>
        /// 查询等级积分日志详情
        /// </summary>
        /// <param name="pager">查询条件</param>
        /// <returns>void</returns>
        /// <remarks>2013-07-10 黄波 添加</remarks>
        public override void GetCrLevelPointLogItems(ref Model.Pager<Model.CrLevelPointLog> pager)
        {
            #region sql条件

            string sqlWhere =
                @"(@customersysno=0 or customersysno=@customersysno) and (@ChangeType=0 or ChangeType=@ChangeType) and (@createdby=0 or CREATEDBY=@createdby)";

            #endregion

            using (var context = Context.UseSharedConnection(true))
            {
                pager.Rows = context.Select<Model.CrLevelPointLog>("cr.*")
                                    .From("CrLevelPointLog cr")
                                    .Where(sqlWhere)
                                    .Parameter("customersysno", pager.PageFilter.CustomerSysNo)
                                    .Parameter("ChangeType", pager.PageFilter.ChangeType)
                                    .Parameter("createdby", pager.PageFilter.CreatedBy)
                                    .Paging(pager.CurrentPage, pager.PageSize)
                                    .OrderBy("changedate desc")
                                    .QueryMany();

                pager.TotalRows = context.Select<int>("count(1)")
                                         .From("CrLevelPointLog")
                                         .Where(sqlWhere)
                                         .Parameter("customersysno", pager.PageFilter.CustomerSysNo)
                                         .Parameter("ChangeType", pager.PageFilter.ChangeType)
                                         .Parameter("createdby", pager.PageFilter.CreatedBy)
                                         .QuerySingle();
            }
        }
    }
}
