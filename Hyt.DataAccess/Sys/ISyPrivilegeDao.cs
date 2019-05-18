using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Transfer;

namespace Hyt.DataAccess.Sys
{
    /// <summary>
    /// 权限
    /// </summary>
    /// <remarks>
    /// 2013-6-28 杨浩 创建
    /// </remarks>
    public abstract class ISyPrivilegeDao : DaoBase<ISyPrivilegeDao>
    {
        /// <summary>
        /// 根据用户SysNo获取权限列表
        /// </summary>
        /// <param name="userSysNo">用户系统号</param>
        /// <returns>权限列表</returns>
        /// <remarks>
        /// 2013-6-28 杨浩 创建
        /// </remarks>
        public abstract IList<Model.SyPrivilege> GetList(int userSysNo);

        /// <summary>
        /// 获取指定菜单下面的权限列表
        /// </summary>
        /// <param name="menuSysNo">菜单编号</param>
        /// <returns>权限列表</returns>
        /// <remarks>
        /// 2013-08-01 朱成果 创建
        /// </remarks> 
        public abstract IList<Model.SyPrivilege> GetListByMenu(int menuSysNo);

        /// <summary>
        /// 获取所有菜单下面的权限
        /// </summary>
        /// <returns>权限列表</returns>
        /// <remarks>
        /// 2013-08-01 朱成果 创建
        /// </remarks> 
        public abstract IList<CBSyPrivilege> GetMenuPrivilege();

        /// <summary>
        /// insert
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>sysNo</returns>
        /// <remarks>2013-08-01 朱家宏 创建</remarks>
        public abstract int Insert(SyPrivilege model);

        /// <summary>
        /// update
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>boolean</returns>
        /// <remarks>2013-08-01 朱家宏 创建</remarks>
        public abstract bool Update(SyPrivilege model);

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
        public abstract SyPrivilege Select(int sysNo);

        /// <summary>
        /// list
        /// </summary>
        /// <returns>IList</returns>
        /// <remarks>2013-08-01 朱家宏 创建</remarks>
        public abstract IList<SyPrivilege> SelectAll();

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="currentPage">当前页号</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="status">权限状态</param>
        /// <param name="keyword">权限名称/权限代码</param>
        /// <returns>分页</returns>
        /// <remarks>2013-08-05 朱家宏 创建</remarks>
        public abstract Pager<SyPrivilege> SelectAll(int currentPage, int pageSize, int? status, string keyword);

        /// <summary>
        /// 关键查询权限
        /// </summary>
        /// <returns>权限列表</returns>
        /// <remarks>2013-08-02 朱家宏 创建</remarks> 
        public abstract IList<CBSyPrivilege> Query(string value);

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
        public abstract Pager<CBSyPrivilege> Query(string keyword, int menuSysNo, int? status,
                                                   int currentPage, int pageSize);
    }
}
