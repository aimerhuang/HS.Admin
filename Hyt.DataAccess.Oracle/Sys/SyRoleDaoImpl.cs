using System;
using System.Collections.Generic;
using Hyt.Model;

namespace Hyt.DataAccess.Oracle.Sys
{
    /// <summary>
    /// 角色
    /// </summary>
    /// <remarks>2013-08-01 朱家宏 创建</remarks>
    public class SyRoleDaoImpl : DataAccess.Sys.ISyRoleDao
    {
        /// <summary>
        /// insert
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>sysNo</returns>
        /// <remarks>2013-08-01 朱家宏 创建</remarks>
        public override int Insert(SyRole model)
        {
            return Context.Insert("SyRole", model)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
        }

        /// <summary>
        /// update
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>boolean</returns>
        /// <remarks>2013-08-01 朱家宏 创建</remarks>
        public override bool Update(SyRole model)
        {
            var r = Context.Update("SyRole", model)
                           .AutoMap(o => o.SysNo)
                           .Where("SysNo", model.SysNo).Execute();
            return r > 0;
        }

        /// <summary>
        /// delete
        /// </summary>
        /// <param name="sysNo">sysNo</param>
        /// <returns>boolean</returns>
        /// <remarks>2013-08-01 朱家宏 创建</remarks>
        public override bool Delete(int sysNo)
        {
            var r = Context.Delete("SyRole").Where("SysNo", sysNo).Execute();
            return r > 0;
        }

        /// <summary>
        /// select
        /// </summary>
        /// <param name="sysNo">sysNo</param>
        /// <returns>entity</returns>
        /// <remarks>2013-08-01 朱家宏 创建</remarks>
        public override SyRole Select(int sysNo)
        {
            return Context.Sql("select * from SyRole where SysNo=@0", sysNo)
                          .QuerySingle<SyRole>();
        }

        /// <summary>
        /// list
        /// </summary>
        /// <returns>IList</returns>
        /// <remarks>2013-08-01 朱家宏 创建</remarks>
        public override IList<SyRole> SelectAll()
        {
            const string sql = @"select * from SyRole order by CreatedDate desc";
            return Context.Sql(sql).QueryMany<SyRole>();
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="currentPage">当前页号</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns>分页</returns>
        /// <remarks>2013-08-05 余勇 创建</remarks>
        public override Pager<SyRole> Query(int? status,int currentPage, int pageSize)
        {
            const string sql = @"(select * from SyRole where 1=1 and (@0 is null or Status = @0)) tb";

            var dataList = Context.Select<SyRole>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(0)").From(sql);
            var paras = new object[]
                {
                 status
                };
            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<SyRole>
            {
                CurrentPage = currentPage,
                TotalRows = dataCount.QuerySingle(),
                Rows = dataList.OrderBy("tb.SysNo desc").Paging(currentPage, pageSize).QueryMany()
            };

            return pager;
        }

        /// <summary>
        /// 启用（禁用）角色
        /// </summary>
        /// <param name="sysNo">sysNo</param>
        /// <param name="status">状态</param>
        /// <returns>影响行数</returns>
        /// <remarks>2013-08-06 余勇 创建</remarks>
        public override int ChangeStatus(int sysNo, int status)
        {
            return Context.Update("SyRole")
                            .Column("Status", status)
                            .Where("SysNo", sysNo)
                            .Execute();

        }

        /// <summary>
        /// 是否存在相同的角色名
        /// </summary>
        /// <param name="roleName">角色名</param>
        /// <param name="sysNo">sysNo</param>
        /// <returns>角色实体</returns>
        /// <remarks>2013-08-06 余勇 创建</remarks>
        public override SyRole GetByRoleName(string roleName, int sysNo)
        {
            return Context.Sql(@"select * from SyRole where  RoleName=@0
                       and (@1=0 or SysNo <> @1)", roleName, sysNo)
                         .QuerySingle<SyRole>();
        }
    }
}
