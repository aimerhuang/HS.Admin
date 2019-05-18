using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.Model;

namespace Hyt.DataAccess.Sys
{
    /// <summary>
    /// 角色菜单
    /// </summary>
    /// <remarks>2013-08-01 朱家宏 创建</remarks>
    public abstract class ISyRoleMenuDao : DaoBase<ISyRoleMenuDao>
    {   
        /// <summary>
        /// insert
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>sysNo</returns>
        /// <remarks>2013-08-01 朱家宏 创建</remarks>
        public abstract int Insert(SyRoleMenu model);

        /// <summary>
        /// delete
        /// </summary>
        /// <param name="sysNo">sysNo</param>
        /// <returns>boolean</returns>
        /// <remarks>2013-08-01 朱家宏 创建</remarks>
        public abstract bool Delete(int sysNo);

        /// <summary>
        /// 通过角色编号删除菜单权限
        /// </summary>
        /// <param name="roleSysNo">roleSysNo</param>
        /// <returns>boolean</returns>
        /// <remarks>2013-08-07 余勇 创建</remarks>
        public abstract bool DeleteByRoleSysNo(int roleSysNo);

        /// <summary>
        /// 通过角色编号查询所属菜单权限
        /// </summary>
        /// <param name="roleSysNo">角色编号</param>
        /// <returns>IList</returns>
        /// <remarks>2013-08-07 余勇 创建</remarks>
        public abstract IList<SyRoleMenu> GetListByRoleSysNo(int roleSysNo);

        /// <summary>
        /// 通过菜单编号删除菜单权限
        /// </summary>
        /// <param name="menuSysNo">menuSysNo</param>
        /// <returns>boolean</returns>
        /// <remarks>2013-08-08 朱家宏 创建</remarks>
        public abstract bool DeleteByMenuSysNo(int menuSysNo);
    }
}
