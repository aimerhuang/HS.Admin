using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess;
using Hyt.DataAccess.CRM;

namespace Hyt.DataAccess.Oracle.CRM
{
    /// <summary>
    /// 经验积分
    /// </summary>
    /// <remarks>2013-07-10 黄波 添加</remarks>
    public class CrExperiencePointLogDaoImpl : ICrExperiencePointLogDao
    {
        /// <summary>
        /// 获取经验积分日志详情
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>经验积分日志详情</returns>
        /// <remarks>2013-07-10 黄波 添加</remarks>
        public override Model.CrExperiencePointLog GetCrExperiencePointLogItem(int sysNo)
        {
            return Context.Select<Model.CrExperiencePointLog>("select * from CrExperiencePointLog where sysNo=@sysNo")
                .Parameter(":sysNo", sysNo)
                .QuerySingle();
        }

        /// <summary>
        /// 插入经验积分日志
        /// </summary>
        /// <param name="model">经验积分信息</param>
        /// <returns>创建结果</returns>
        /// <remarks>2013-07-10 黄波 添加</remarks>
        public override bool InsertCrExperiencePointLogItem(Model.CrExperiencePointLog model)
        {
            return Context.Insert("CrExperiencePointLog", model)
                .AutoMap(o => o.SysNo)
                .ExecuteReturnLastId<int>("sysNo") > 0;
        }

        /// <summary>
        /// 查询经验积分日志
        /// </summary>
        /// <param name="pager">查询条件</param>
        /// <returns>void</returns>
        /// <remarks>2013-07-10 黄波 添加</remarks>
        public override void GetCrExperiencePointLogItems(ref Model.Pager<Model.CrExperiencePointLog> pager)
        {
            #region sql条件
            string sqlWhere = @"(:customersysno=0 or customersysno=:customersysno) and (:PointType=0 or PointType=:PointType) and (:createdby=0 or CREATEDBY=:createdby)";
            #endregion

            using (var context = Context.UseSharedConnection(true))
            {
                pager.Rows = context.Select<Model.CrExperiencePointLog>("cr.*")
                                    .From("CrExperiencePointLog cr")
                                    .Where(sqlWhere)
                                    .Parameter("customersysno", pager.PageFilter.CustomerSysNo)
                                    .Parameter("PointType", pager.PageFilter.PointType)
                                    .Parameter("createdby", pager.PageFilter.CreatedBy)
                                    .Paging(pager.CurrentPage, pager.PageSize)
                                    .OrderBy("changedate desc")
                                    .QueryMany();

                pager.TotalRows = context.Select<int>("count(1)")
                                         .From("CrExperiencePointLog")
                                         .Where(sqlWhere)
                                         .Parameter("customersysno", pager.PageFilter.CustomerSysNo)
                                         .Parameter("PointType", pager.PageFilter.PointType)
                                         .Parameter("createdby", pager.PageFilter.CreatedBy)
                                         .QuerySingle();
            }
        }

        /// <summary>
        /// 获取日志中用户的经验积分余额
        /// </summary>
        /// <param name="customerSysNo">会员编号</param>
        /// <returns>经验积分余额</returns>
        /// <remarks>2013-07-10 黄波 添加</remarks>
        public override int GetCrExperiencePointSurplus(int customerSysNo)
        {
            return Context.Select<int>("surplus")
                .From("CrExperiencePointLog")
                .Where("customersysno=:customersysno")
                .Parameter("customersysno", customerSysNo)
                .OrderBy("changedate desc")
                .QuerySingle();
        }
    }
}
