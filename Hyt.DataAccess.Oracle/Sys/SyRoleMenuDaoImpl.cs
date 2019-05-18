using System.Collections.Generic;
using Hyt.DataAccess.Sys;
using Hyt.Model;

namespace Hyt.DataAccess.Oracle.Sys
{
    /// <summary>
    /// 角色菜单
    /// </summary>
    /// <remarks>2013-08-01 朱家宏 创建</remarks>
    public class SyRoleMenuDaoImpl : ISyRoleMenuDao
    {
        /// <summary>
        /// 通过角色编号查询所属菜单权限
        /// </summary>
        /// <param name="roleSysNo">角色编号</param>
        /// <returns>IList</returns>
        /// <remarks>2013-08-07 余勇 创建</remarks>
        public override IList<SyRoleMenu> GetListByRoleSysNo(int roleSysNo)
        {
            const string sql = @"select * from SyRoleMenu where RoleSysNo=@0 order by CreatedDate desc";
            return Context.Sql(sql, roleSysNo).QueryMany<SyRoleMenu>();
        }

        /// <summary>
        /// insert
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>sysNo</returns>
        /// <remarks>2013-08-01 朱家宏 创建</remarks>
        public override int Insert(SyRoleMenu model)
        {
            return Context.Insert("SyRoleMenu", model)
                                        .AutoMap(o => o.SysNo)
                                        .ExecuteReturnLastId<int>("SysNo");
        }

        /// <summary>
        /// delete
        /// </summary>
        /// <param name="sysNo">sysNo</param>
        /// <returns>boolean</returns>
        /// <remarks>2013-08-01 朱家宏 创建</remarks>
        public override bool Delete(int sysNo)
        {
            var r = Context.Delete("SyRoleMenu").Where("SysNo", sysNo).Execute();
            return r > 0;
        }

        /// <summary>
        /// 通过角色编号删除菜单权限
        /// </summary>
        /// <param name="roleSysNo">roleSysNo</param>
        /// <returns>boolean</returns>
        /// <remarks>2013-08-07 余勇 创建</remarks>
        public override bool DeleteByRoleSysNo(int roleSysNo)
        {
            var r = Context.Delete("SyRoleMenu").Where("RoleSysNo", roleSysNo).Execute();
            return r > 0;
        }

        /// <summary>
        /// 通过菜单编号删除菜单权限
        /// </summary>
        /// <param name="menuSysNo">menuSysNo</param>
        /// <returns>boolean</returns>
        /// <remarks>2013-08-08 朱家宏 创建</remarks>
        public override bool DeleteByMenuSysNo(int menuSysNo)
        {
            var r = Context.Delete("SyRoleMenu").Where("menuSysNo", menuSysNo).Execute();
            return r > 0;
        }
    }
}
