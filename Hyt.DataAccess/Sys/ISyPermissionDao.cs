using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Parameter;
namespace Hyt.DataAccess.Sys
{
    /// <summary>
    /// 授权
    /// </summary>
    /// <remarks>2013-08-01  朱成果 创建</remarks>
    public abstract class ISyPermissionDao : DaoBase<ISyPermissionDao>
    {
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2013-08-01  朱成果 创建</remarks>
        public abstract int Insert(SyPermission entity);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        /// <remarks>2013-08-01  朱成果 创建</remarks>
        public abstract void Update(SyPermission entity);

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2013-08-01  朱成果 创建</remarks>
        public abstract SyPermission GetEntity(int sysNo);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-01  朱成果 创建</remarks>
        public abstract void Delete(int sysNo);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="source">来源:系统用户(10),用户组(20)</param>
        /// <param name="sourceSysNo">来源编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-01  朱成果 创建</remarks>
        public abstract int Delete(int source, int sourceSysNo);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="source">来源:系统用户(10),用户组(20)</param>
        /// <param name="sourceSysNo">来源编号</param>
        /// <param name="target">>目标:菜单(10),角色(20),权限(30)</param>
        /// <returns></returns>
        /// <remarks>2013-08-01  朱成果 创建</remarks>
        public abstract int Delete(int source, int sourceSysNo, int target);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="source">来源:系统用户(10),用户组(20)</param>
        /// <param name="sourceSysNo">来源编号</param>
        /// <param name="target">>目标:菜单(10),角色(20),权限(30)</param>
        /// <param name="targetSysNo">目标编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-08  黄志勇 创建</remarks>
        public abstract int Delete(int source, int sourceSysNo, int target, int targetSysNo);

        /// <summary>
        /// 获取授权列表
        /// </summary>
        /// <param name="source">来源:系统用户(10),用户组(20)</param>
        /// <param name="sourceSysNo">来源编号</param>
        /// <returns>授权列表</returns>
        /// <remarks>2013-08-01  朱成果 创建</remarks>
        public abstract List<SyPermission> GetList(int source, int sourceSysNo);

        /// <summary>
        /// 获取授权列表
        /// </summary>
        /// <param name="source">来源:系统用户(10),用户组(20)</param>
        /// <param name="sourceSysNo">来源编号</param>
        /// <param name="target">目标:菜单(10),角色(20),权限(30)</param>
        /// <returns>授权列表</returns>
        /// <remarks>2013-08-01  朱成果 创建</remarks>
        public abstract List<SyPermission> GetList(int source, int sourceSysNo, int target);

    }
}
