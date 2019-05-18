using System.Collections.Generic;
using Hyt.Model;
using Hyt.Model.WorkflowStatus;

namespace Hyt.DataAccess.Oracle.Sys
{
    /// <summary>
    /// 系统菜单
    /// </summary>
    /// <remarks>
    /// 2013-6-25 杨浩 创建
    /// </remarks>
    public class SyMenuDaoImpl : DataAccess.Sys.ISyMenuDao
    {
        /// <summary>
        /// 获取系统用户菜单权限
        /// </summary>
        /// <param name="userSysNo">系统用户号</param>
        /// <returns>菜单列表</returns>
        /// <remarks> 2013-6-25 杨浩 创建</remarks>
        public override IList<SyMenu> GetList(int userSysNo)
        {
            #region sql

            string sql = @"select * from SyMenu where sysno in
(
select SyMenu.SysNo 
from SyMenu 
     inner join SyPermission 
     on TargetSysNo=SyMenu.SysNo  
where Source={0} and Target={1} and SourceSysNo=@userSysNo--用户->菜单 SourceSysNo系统用户编号
union
select SyRoleMenu.MenuSysNo 
from SyRoleMenu 
     inner join SyPermission 
     on TargetSysNo=SyRoleMenu.RoleSysNo  
where Source={2} and Target={3} and SourceSysNo=@userSysNo --用户->角色 SourceSysNo系统用户编号
union
select SyMenu.SysNo 
from SyMenu 
     inner join SyPermission 
     on TargetSysNo=SyMenu.SysNo
     inner join SyGroupUser
     on SyGroupUser.Groupsysno = SyPermission.Sourcesysno
     inner join SyUserGroup
     on SyUserGroup.SysNo=SyGroupUser.GroupSysNo
where Source={4} and Target={5} and SyGroupUser.Usersysno=@userSysNo and  SyUserGroup.Status={6} --用户组->菜单
union
select SyRoleMenu.MenuSysNo 
from SyRoleMenu 
     inner join SyPermission 
     on TargetSysNo=SyRoleMenu.RoleSysNo
     inner join SyGroupUser
     on SyGroupUser.Groupsysno = SyPermission.Sourcesysno
     inner join SyUserGroup
     on SyUserGroup.SysNo=SyGroupUser.GroupSysNo
where Source={7} and Target={8} and SyGroupUser.Usersysno= @userSysNo and SyUserGroup.Status={9} --用户组->角色
) and Status={10} order by DisplayOrder asc

";
            sql = string.Format(sql, (int)SystemStatus.授权来源.系统用户, (int)SystemStatus.授权目标.菜单,
                                     (int)SystemStatus.授权来源.系统用户, (int)SystemStatus.授权目标.角色,
                                     (int)SystemStatus.授权来源.用户组, (int)SystemStatus.授权目标.菜单,(int)SystemStatus.用户组状态.启用,
                                     (int)SystemStatus.授权来源.用户组, (int)SystemStatus.授权目标.角色,(int)SystemStatus.用户组状态.启用,
                                     (int)SystemStatus.菜单状态.启用);

            #endregion
            //sql = @"select * from SyMenu ";
            var list = Context.Sql(sql)
                              .Parameter("userSysNo", userSysNo)
                              //.Parameter("userSysNo", userSysNo)
                              //.Parameter("userSysNo", userSysNo)
                              //.Parameter("userSysNo", userSysNo)
                              .QueryMany<SyMenu>();

            return list;
        }

        /// <summary>
        /// select model
        /// </summary>
        /// <param name="sysNo">sysNo</param>
        /// <returns>model</returns>
        /// <remarks>2013-08-02 朱家宏 创建</remarks>
        public override SyMenu Select(int sysNo)
        {
            return Context.Sql("select * from symenu where sysNo=@0", sysNo)
                          .QuerySingle<SyMenu>();
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="currentPage">当前页号</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns>分页菜单</returns>
        /// <remarks>2013-07-30 朱家宏 创建</remarks>
        public override Pager<SyMenu> GetAll(int currentPage, int pageSize)
        {
            const string sql = @"(select * from SyMenu a ) tb";

            var dataList = Context.Select<SyMenu>("tb.*").From(sql);
            var dataCount = Context.Select<int>("count(0)").From(sql);

            var pager = new Pager<SyMenu>
                {
                    TotalRows = dataCount.QuerySingle(),
                    Rows = dataList.OrderBy("tb.DisplayOrder").Paging(currentPage, pageSize).QueryMany()
                };

            return pager;
        }

        /// <summary>
        /// list
        /// </summary>
        /// <returns>IList</returns>
        /// <remarks>2013-07-31 朱家宏 创建</remarks>
        public override IList<SyMenu> GetAll()
        {
            const string sql = @"select * from SyMenu order by DisplayOrder";
            return Context.Sql(sql).QueryMany<SyMenu>();
        }

        /// <summary>
        /// 通过父级编号获取所有菜单
        /// </summary>
        /// <param name="menuSysNo">菜单编号</param>
        /// <returns>菜单列表</returns>
        /// <remarks>2013-08-02 朱家宏 创建</remarks>
        public override IList<SyMenu> GetAllByParentSysNo(int menuSysNo)
        {
            const string sql = @"select * from SyMenu where parentSysNo=@0 order by DisplayOrder";
            return Context.Sql(sql, menuSysNo).QueryMany<SyMenu>();
        }

        /// <summary>
        /// insert
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>sysNo</returns>
        /// <remarks>2013-07-30 朱家宏 创建</remarks>
        public override int Insert(SyMenu model)
        {
            return Context.Insert("SyMenu", model)
                                        .AutoMap(o => o.SysNo, o => o.Level)
                                        .ExecuteReturnLastId<int>("SysNo");
        }

        /// <summary>
        /// update
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>boolean</returns>
        /// <remarks>2013-07-30 朱家宏 创建</remarks>
        public override bool Update(SyMenu model)
        {
            var r = Context.Update("SyMenu", model)
                           .AutoMap(o => o.SysNo, o => o.Level)
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
            var r = Context.Delete("SyMenu").Where("SysNo", sysNo).Execute();
            return r > 0;
        }

        /// <summary>
        /// 通过parentSysNo删除菜单
        /// </summary>
        /// <param name="parentSysNo">parentSysNo</param>
        /// <returns>bool</returns>
        /// <remarks>2013-08-08 朱家宏 创建</remarks>
        public override bool DeleteByParentSysNo(int parentSysNo)
        {
            var r = Context.Delete("SyMenu").Where("parentSysNo", parentSysNo).Execute();
            return r > 0;
        }
    }
}
