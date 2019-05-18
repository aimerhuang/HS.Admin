using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess;
using Hyt.DataAccess.CRM;

namespace Hyt.DataAccess.Oracle.CRM
{
    /// <summary>
    /// 惠源币日志
    /// </summary>
    /// <remarks>2013-07-10 黄波 添加</remarks>
    public class CrExperienceCoinLogDaoImpl : ICrExperienceCoinLogDao
    {
        /// <summary>
        /// 获取惠源币日志详情
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>惠源币日志详情详情</returns>
        /// <returns>惠源币日志详情</returns>
        /// <remarks>2013-07-10 黄波 添加</remarks>
        public override Model.CrExperienceCoinLog GetCrExperienceCoinLogItem(int sysNo)
        {
            return Context.Select<Model.CrExperienceCoinLog>("select * from CrExperienceCoinLog where sysNo=@sysNo")
                .Parameter(":sysNo", sysNo)
                .QuerySingle();
        }

        /// <summary>
        /// 创建惠源币日志
        /// </summary>
        /// <param name="model">惠源币日志详情</param>
        /// <returns>创建结果</returns>
        /// <remarks>2013-07-10 黄波 添加</remarks>
        public override bool InsertCrExperienceCoinLogItem(Model.CrExperienceCoinLog model)
        {
            return Context.Insert<Model.CrExperienceCoinLog>("CrExperienceCoinLog", model)
                 .AutoMap(o => o.SysNo)
                 .ExecuteReturnLastId<int>("sysNo") > 0;
        }

        /// <summary>
        /// 查询惠源币日志详情
        /// </summary>
        /// <param name="pager">查询条件</param>
        /// <returns>void</returns>
        /// <remarks>2013-07-10 黄波 添加</remarks>
        public override void GetCrExperienceCoinLogItems(ref Model.Pager<Model.CrExperienceCoinLog> pager)
        {
            #region sql条件
            string sqlWhere = @"(:customersysno=0 or customersysno=:customersysno) and (:changetype=0 or changetype=:changetype) and (:createdby=0 or CREATEDBY=:createdby)";
            #endregion

            using (var context = Context.UseSharedConnection(true))
            {
                pager.Rows = context.Select<Model.CrExperienceCoinLog>("cr.*")
                                    .From("CrExperienceCoinLog cr")
                                    .Where(sqlWhere)
                                    .Parameter("customersysno", pager.PageFilter.CustomerSysNo)
                                    .Parameter("changetype", pager.PageFilter.ChangeType)
                                    .Parameter("createdby", pager.PageFilter.CreatedBy)
                                    .Paging(pager.CurrentPage, pager.PageSize)
                                    .OrderBy("changedate desc")
                                    .QueryMany();

                pager.TotalRows = context.Select<int>("count(1)")
                                         .From("CrExperienceCoinLog")
                                         .Where(sqlWhere)
                                         .Parameter("customersysno", pager.PageFilter.CustomerSysNo)
                                         .Parameter("changetype", pager.PageFilter.ChangeType)
                                         .Parameter("createdby", pager.PageFilter.CreatedBy)
                                         .QuerySingle();
            }
        }

        /// <summary>
        /// 获取日志中用户的惠源币余额
        /// </summary>
        /// <param name="customerSysNo">会员编号</param>
        /// <returns>惠源币余额</returns>
        /// <remarks>2013-07-10 黄波 添加</remarks>
        public override int GetCrExperienceCoinSurplus(int customerSysNo)
        {
            return Context.Select<int>("surplus")
               .From("crexperiencecoinlog")
               .Where("customersysno=:customersysno")
               .Parameter("customersysno", customerSysNo)
               .OrderBy("changedate desc")
               .QuerySingle();
        }
    }
}
