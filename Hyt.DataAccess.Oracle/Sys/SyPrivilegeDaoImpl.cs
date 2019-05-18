using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.Model.Transfer;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Oracle.Sys
{
    /// <summary>
    /// 权限
    /// </summary>
    /// <remarks>
    /// 2013-6-28 杨浩 创建
    /// </remarks>
    public class SyPrivilegeDaoImpl : DataAccess.Sys.ISyPrivilegeDao
    {
        /// <summary>
        /// 根据用户SysNo获取权限列表
        /// </summary>
        /// <param name="userSysNo">用户系统号</param>
        /// <returns>权限列表</returns>
        /// <remarks>
        /// 2013-6-28 杨浩 创建
        /// </remarks>
        public override IList<Model.SyPrivilege> GetList(int userSysNo)
        {
            #region sql

            string sql = @"
select * from SyPrivilege where sysno in
(
select SyPrivilege.SysNo 
from SyPrivilege 
     inner join SyPermission 
     on TargetSysNo=SyPrivilege.SysNo  
where Source={0} and Target={1} and SourceSysNo=@userSysNo --用户->权限 SourceSysNo系统用户编号
union
select SyRolePrivilege.PrivilegeSysNo 
from SyRolePrivilege 
     inner join SyPermission 
     on TargetSysNo=SyRolePrivilege.RoleSysNo  
where Source={2} and Target={3} and SourceSysNo=@userSysNo --用户->角色 SourceSysNo系统用户编号
union
select SyPrivilege.SysNo 
from SyPrivilege 
     inner join SyPermission 
     on TargetSysNo=SyPrivilege.SysNo
     inner join SyGroupUser
     on SyGroupUser.Groupsysno = SyPermission.Sourcesysno
     inner join SyUserGroup
     on SyUserGroup.SysNo=SyGroupUser.GroupSysNo
where Source={4} and Target={5} and SyGroupUser.Usersysno=@userSysNo and  SyUserGroup.Status={6} --用户组->权限SyGroupUser.Usersysno 用户组编号
union
select SyRolePrivilege.PrivilegeSysNo 
from SyRolePrivilege 
     inner join SyPermission 
     on TargetSysNo=SyRolePrivilege.RoleSysNo  
     inner join SyGroupUser
     on SyGroupUser.Groupsysno = SyPermission.Sourcesysno
     inner join SyUserGroup
     on SyUserGroup.SysNo=SyGroupUser.GroupSysNo
where Source={7} and Target={8} and SyGroupUser.Usersysno=@userSysNo and  SyUserGroup.Status={9} --用户组->角色SyGroupUser.Usersysno 用户组编号
)
";
            sql = string.Format(sql, (int)SystemStatus.授权来源.系统用户, (int)SystemStatus.授权目标.权限,
                                     (int)SystemStatus.授权来源.系统用户, (int)SystemStatus.授权目标.角色,
                                     (int)SystemStatus.授权来源.用户组, (int)SystemStatus.授权目标.权限,(int)SystemStatus.用户组状态.启用,
                                     (int)SystemStatus.授权来源.用户组, (int)SystemStatus.授权目标.角色, (int)SystemStatus.用户组状态.启用);

            #endregion

            var list = Context.Sql(sql)
                              .Parameter("userSysNo", userSysNo)
                              .QueryMany<SyPrivilege>();

            return list;
        }

        /// <summary>
        /// 获取指定菜单下面的权限列表
        /// </summary>
        /// <param name="menuSysNo">菜单编号</param>
        /// <returns>权限列表</returns>
        /// <remarks>
        /// 2013-08-01 朱成果 创建
        /// </remarks> 
        public override  IList<Model.SyPrivilege> GetListByMenu(int menuSysNo)
        {
           return Context.Sql(@"select t0.* 
                            from SyPrivilege t0
                            inner join SyMenuPrivilege t1 
                            on t0.sysno=t1.privilegesysno
                            where t1.menusysno=@menusysno").Parameter("menuSysNo", menuSysNo).QueryMany<SyPrivilege>();
        }

        /// <summary>
        /// 获取所有菜单下面的权限
        /// </summary>
        /// <returns>权限列表</returns>
        /// <remarks>
        /// 2013-08-01 朱成果 创建
        /// </remarks> 
        public override IList<CBSyPrivilege> GetMenuPrivilege()
        {
            return Context.Sql(@"select t0.*,t1.MenuSysNo
                            from SyPrivilege t0
                            inner join SyMenuPrivilege t1 
                            on t0.sysno=t1.privilegesysno").QueryMany<CBSyPrivilege>();
        }

        /// <summary>
        /// insert
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>sysNo</returns>
        /// <remarks>2013-08-01 朱家宏 创建</remarks>
        public override int Insert(SyPrivilege model)
        {
            return Context.Insert("SyPrivilege", model)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
        }

        /// <summary>
        /// update
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>boolean</returns>
        /// <remarks>2013-08-01 朱家宏 创建</remarks>
        public override bool Update(SyPrivilege model)
        {
            var r = Context.Update("SyPrivilege", model)
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
            var r = Context.Delete("SyPrivilege").Where("SysNo", sysNo).Execute();
            return r > 0;
        }

        /// <summary>
        /// select
        /// </summary>
        /// <param name="sysNo">sysNo</param>
        /// <returns>entity</returns>
        /// <remarks>2013-08-01 朱家宏 创建</remarks>
        public override SyPrivilege Select(int sysNo)
        {
            return Context.Sql("select * from SyPrivilege where SysNo=@0", sysNo)
                          .QuerySingle<SyPrivilege>();
        }

        /// <summary>
        /// list
        /// </summary>
        /// <returns>IList</returns>
        /// <remarks>2013-08-01 朱家宏 创建</remarks>
        public override IList<SyPrivilege> SelectAll()
        {
            const string sql = @"select * from SyPrivilege order by SysNo desc";
            return Context.Sql(sql).QueryMany<SyPrivilege>();
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="currentPage">当前页号</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="status">权限状态</param>
        /// <param name="keyword">权限名称/权限代码</param>
        /// <returns>分页</returns>
        /// <remarks>2013-08-05 朱家宏 创建</remarks>
        public override Pager<SyPrivilege> SelectAll(int currentPage, int pageSize, int? status, string keyword)
        {
            const string sql = @"(select * from SyPrivilege a 
                                where (@0 is null or (charindex(a.name,@0)>0 or charindex(a.code,@0)>0)) and 
                                (@1 is null or a.status=@1)
                                ) tb";

            var paras = new object[]
                {
                    (keyword==""?null:keyword),//     keyword,   keyword,
                    status//,      status
                };

            var dataList = Context.Select<SyPrivilege>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(0)").From(sql);

            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<SyPrivilege>
            {
                PageSize = pageSize,
                CurrentPage = currentPage,
                TotalRows = dataCount.QuerySingle(),
                Rows = dataList.OrderBy("tb.SysNo desc").Paging(currentPage, pageSize).QueryMany()
            };

            return pager;
        }

        /// <summary>
        /// 查询权限
        /// </summary>
        /// <param name="value">条件</param>
        /// <returns>权限列表</returns>
        /// <remarks>2013-08-02 朱家宏 创建</remarks> 
        public override IList<CBSyPrivilege> Query(string value)
        {
            return Context.Sql(@"select a.*,b.MenuSysNo
                           from SyPrivilege a
                           left join SyMenuPrivilege b 
                           on a.sysno=b.privilegesysno
                           where @value is null or (CHARINDEX(a.name,@value)>0 or CHARINDEX(a.code,@value)>0) ")
                          .Parameter("value", value)
                          //.Parameter("value", value)
                          //.Parameter("value", value)
                          .QueryMany<CBSyPrivilege>();
        }

        /// <summary>
        /// 查询权限分页
        /// </summary>
        /// <param name="keyword">关键字(名称或代码)</param>
        /// <param name="menuSysNo">菜单编号</param>
        /// <param name="status">权限状态</param>
        /// <param name="currentPage">当前页号</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns>分页</returns>
        /// <remarks>2013-08-12 朱家宏 创建</remarks> 
        public override Pager<CBSyPrivilege> Query(string keyword, int menuSysNo, int? status, int currentPage, int pageSize)
        {
            const string sql = @"(select a.*,b.MenuSysNo
                           from SyPrivilege a
                           left join SyMenuPrivilege b 
                           on a.sysno=b.privilegesysno
                           where (@0 is null or (CHARINDEX(a.name,@0)>0 or CHARINDEX(a.code,@0)>0)) and
                           (b.menuSysNo is null) and
                           (@1 is null or a.status=@1)
                           ) tb";

            var paras = new object[]
                {
                    (keyword==""?null:keyword), //    keyword,   keyword,
                    status//,      status
                };

            var dataList = Context.Select<CBSyPrivilege>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(0)").From(sql);

            dataList.Parameters(paras);
            dataCount.Parameters(paras);

            var pager = new Pager<CBSyPrivilege>
            {
                PageSize = pageSize,
                CurrentPage = currentPage,
                TotalRows = dataCount.QuerySingle(),
                Rows = dataList.OrderBy("tb.SysNo desc").Paging(currentPage, pageSize).QueryMany()
            };

            return pager;
        }

    }
}
