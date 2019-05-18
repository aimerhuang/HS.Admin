using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.Model;

namespace Hyt.DataAccess.Sys
{
    /// <summary>
    /// 角色
    /// </summary>
    /// <remarks>2013-08-01 朱家宏 创建</remarks>
    public abstract class ISyRoleDao : DaoBase<ISyRoleDao>
    {
        /// <summary>
        /// insert
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>sysNo</returns>
        /// <remarks>2013-08-01 朱家宏 创建</remarks>
        public abstract int Insert(SyRole model);

        /// <summary>
        /// update
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>boolean</returns>
        /// <remarks>2013-08-01 朱家宏 创建</remarks>
        public abstract bool Update(SyRole model);

        /// <summary>
        /// delete
        /// </summary>
        /// <param name="sysNo">sysNo</param>
        /// <returns>boolean</returns>
        /// <remarks>2013-08-01 朱家宏 创建</remarks>
        public abstract bool Delete(int sysNo);

        /// <summary>
        /// select
        /// </summary>
        /// <param name="sysNo">sysNo</param>
        /// <returns>entity</returns>
        /// <remarks>2013-08-01 朱家宏 创建</remarks>
        public abstract SyRole Select(int sysNo);

        /// <summary>
        /// list
        /// </summary>
        /// <returns>IList</returns>
        /// <remarks>2013-08-01 朱家宏 创建</remarks>
        public abstract IList<SyRole> SelectAll();

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="currentPage">当前页号</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns>分页</returns>
        /// <remarks>2013-08-05 余勇 创建</remarks>
        public abstract Pager<SyRole> Query(int? status, int currentPage, int pageSize);

        /// <summary>
        /// 启用（禁用）角色
        /// </summary>
        /// <param name="sysNo">sysNo</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        /// <remarks>2013-08-06 余勇 创建</remarks>
        public abstract int ChangeStatus(int sysNo, int status);

        /// <summary>
        /// 是否存在相同的角色名
        /// </summary>
        /// <param name="roleName">角色名</param>
        /// <param name="sysNo">sysNo</param>
        /// <returns></returns>
        /// <remarks>2013-08-06 余勇 创建</remarks>
        public abstract SyRole GetByRoleName(string roleName, int sysNo);
    }
}
