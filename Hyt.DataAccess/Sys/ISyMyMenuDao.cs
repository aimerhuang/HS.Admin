using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.Model;

namespace Hyt.DataAccess.Sys
{
    /// <summary>
    /// 我的菜单
    /// </summary>
    /// <remarks>
    /// 2014-01-13 何方 创建
    /// </remarks>
    public abstract class ISyMyMenuDao : DaoBase<ISyMyMenuDao>
    {
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>
        /// 新增记录编号
        /// </returns>
        /// <remarks>
        /// 2013-10-09  周瑜 创建
        /// </remarks>
        public abstract int Insert(SyMyMenu entity);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="userSysNo">用户系统编号</param>
        /// <param name="menuSysNo">菜单系统编号</param>
        /// <remarks>
        /// 2013-10-09  周瑜 创建
        /// </remarks>
        public abstract void Delete(int userSysNo, int menuSysNo);

        /// <summary>
        /// 获取我的菜单
        /// </summary>
        /// <param name="userSysNo">系统用户号</param>
        /// <returns>
        /// 菜单列表
        /// </returns>
        /// <remarks>
        /// 2013-10-09 周瑜 创建
        /// </remarks>
        public abstract IList<SyMenu> GetList(int userSysNo);

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="userSysNo">用户系统编号</param>
        /// <param name="menuSysNo">菜单系统编号</param>
        /// <returns>
        /// 菜单实体
        /// </returns>
        /// <remarks>
        /// 2014/1/13 何方 创建
        /// </remarks>
        public abstract SyMyMenu GetModel(int userSysNo, int menuSysNo);
    }
}
