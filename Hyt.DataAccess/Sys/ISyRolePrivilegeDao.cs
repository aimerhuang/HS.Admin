using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.Model;

namespace Hyt.DataAccess.Sys
{
    /// <summary>
    /// 角色权限
    /// </summary>
    /// <remarks>2013-08-01 朱家宏 创建</remarks>
    public abstract class ISyRolePrivilegeDao : DaoBase<ISyRolePrivilegeDao>
    {
        /// <summary>
        /// insert
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>sysNo</returns>
        /// <remarks>2013-08-01 朱家宏 创建</remarks>
        public abstract int Insert(SyRolePrivilege model);

        /// <summary>
        /// delete
        /// </summary>
        /// <param name="sysNo">sysNo</param>
        /// <returns>boolean</returns>
        /// <remarks>2013-08-01 朱家宏 创建</remarks>
        public abstract bool Delete(int sysNo);

        /// <summary>
        /// 通过权限编号查询
        /// </summary>
        /// <param name="privilegeSysNo">权限编号</param>
        /// <returns>列表</returns>
        /// <remarks>2013-08-05 朱家宏 创建</remarks>
        public abstract IList<SyRolePrivilege> SelectAllByPrivilegeSysNo(int privilegeSysNo);

        /// <summary>
        /// 通过角色编号删除角色权限
        /// </summary>
        /// <param name="roleSysNo">roleSysNo</param>
        /// <returns>boolean</returns>
        /// <remarks>2013-08-07 余勇 创建</remarks>
        public abstract bool DeleteByRoleSysNo(int roleSysNo);

        /// <summary>
        /// 通过角色编号查询所属权限
        /// </summary>
        /// <param name="roleSysNo">角色编号</param>
        /// <returns>IList</returns>
        /// <remarks>2013-08-07 余勇 创建</remarks>
        public abstract IList<SyRolePrivilege> GetListByRoleSysNo(int roleSysNo);
    }
}
