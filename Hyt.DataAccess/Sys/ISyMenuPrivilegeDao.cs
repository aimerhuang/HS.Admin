using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.Model;

namespace Hyt.DataAccess.Sys
{
    /// <summary>
    /// 菜单权限
    /// </summary>
    /// <remarks>2013-08-01 朱家宏 创建</remarks>
    public abstract class ISyMenuPrivilegeDao : DaoBase<ISyMenuPrivilegeDao>
    {        
        /// <summary>
        /// insert
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>sysNo</returns>
        /// <remarks>2013-08-01 朱家宏 创建</remarks>
        public abstract int Insert(SyMenuPrivilege model);

        /// <summary>
        /// delete
        /// </summary>
        /// <param name="sysNo">sysNo</param>
        /// <returns>boolean</returns>
        /// <remarks>2013-08-01 朱家宏 创建</remarks>
        public abstract bool Delete(int sysNo);

        /// <summary>
        /// 通过菜单sysNo删除权限
        /// </summary>
        /// <param name="menuSysNo">菜单sysNo</param>
        /// <returns>boolean</returns>
        /// <remarks>2013-08-01 朱家宏 创建</remarks>
        public abstract bool DeleteByMenuSysNo(int menuSysNo);

        /// <summary>
        /// 通过权限编号查询
        /// </summary>
        /// <param name="privilegeSysNo">权限编号</param>
        /// <returns>列表</returns>
        /// <remarks>2013-08-05 朱家宏 创建</remarks>
        public abstract IList<SyMenuPrivilege> SelectAllByPrivilegeSysNo(int privilegeSysNo);
    }
}
