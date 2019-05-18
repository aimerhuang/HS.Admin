using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;

namespace Hyt.DataAccess.Sys
{
    /// <summary>
    /// 系统菜单
    /// </summary>
    /// <remarks>
    /// 2013-6-25 杨浩 创建
    /// </remarks>
    public abstract class ISyMenuDao : DaoBase<ISyMenuDao>
    {

        /// <summary>
        /// 获取系统用户菜单权限
        /// </summary>
        /// <param name="userSysNo">系统用户号</param>
        /// <returns>菜单列表</returns>
        /// <remarks> 2013-6-25 杨浩 创建</remarks>
        public abstract IList<SyMenu> GetList(int userSysNo);

        /// <summary>
        /// select model
        /// </summary>
        /// <param name="sysNo">sysNo</param>
        /// <returns>model</returns>
        /// <remarks>2013-08-02 朱家宏 创建</remarks>
        public abstract SyMenu Select(int sysNo);

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="currentPage">当前页号</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns>分页菜单</returns>
        /// <remarks>2013-07-30 朱家宏 创建</remarks>
        public abstract Pager<SyMenu> GetAll(int currentPage, int pageSize);

        /// <summary>
        /// list
        /// </summary>
        /// <returns>IList</returns>
        /// <remarks>2013-07-31 朱家宏 创建</remarks>
        public abstract IList<SyMenu> GetAll();

        /// <summary>
        /// 通过父级编号获取所有菜单
        /// </summary>
        /// <param name="menuSysNo">菜单编号</param>
        /// <returns>菜单列表</returns>
        /// <remarks>2013-08-02 朱家宏 创建</remarks>
        public abstract IList<SyMenu> GetAllByParentSysNo(int menuSysNo);

        /// <summary>
        /// insert
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>sysNo</returns>
        /// <remarks>2013-07-30 朱家宏 创建</remarks>
        public abstract int Insert(SyMenu model);

        /// <summary>
        /// update
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>boolean</returns>
        /// <remarks>2013-07-30 朱家宏 创建</remarks>
        public abstract bool Update(SyMenu model);

        /// <summary>
        /// delete
        /// </summary>
        /// <param name="sysNo">sysNo</param>
        /// <returns>boolean</returns>
        /// <remarks>2013-08-01 朱家宏 创建</remarks>
        public abstract bool Delete(int sysNo);
        
        /// <summary>
        /// 通过parentSysNo删除菜单
        /// </summary>
        /// <param name="parentSysNo">parentSysNo</param>
        /// <returns>bool</returns>
        /// <remarks>2013-08-08 朱家宏 创建</remarks>
        public abstract bool DeleteByParentSysNo(int parentSysNo);
    }
}
