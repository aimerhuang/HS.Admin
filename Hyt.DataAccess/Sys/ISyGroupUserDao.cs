
using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Parameter;
namespace Hyt.DataAccess.Sys
{
    /// <summary>
    /// 用户对于用户组
    /// </summary>
    /// <remarks>2013-07-30  朱成果 创建</remarks>
    public abstract class ISyGroupUserDao : DaoBase<ISyGroupUserDao>
    {
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2013-07-30  朱成果 创建</remarks>
        public abstract int Insert(SyGroupUser entity);

        /// <summary>
        /// 根据用户编号删除数据
        /// </summary>
        /// <param name="userSysNo">用户编号</param>
        /// <returns>影响的行数</returns>
        /// <remarks>2013-07-30  朱成果 创建</remarks>
        public abstract int DeleteByUserSysNo(int userSysNo);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns></returns>
        /// <remarks>2013-07-30  朱成果 创建</remarks>
        public abstract void Delete(int sysNo);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="userSysNo">用户编号</param>
        /// <param name="groupSysNo">用户组编号</param>
        /// <returns></returns>
        /// <remarks>2013-07-30  黄志勇 创建</remarks>
        public abstract void Delete(int userSysNo, int groupSysNo);

        /// <summary>
        /// 根据用户编号获取用户分组
        /// </summary>
        /// <param name="sysNo">用户编号</param>
        /// <remarks>2013-08-06  黄志勇 创建</remarks>
        public abstract IList<SyGroupUser> GetGroupUser(int sysNo);

        /// <summary>
        /// 组是否包含该用户
        /// </summary>
        /// <param name="groupSysNo">组系统编号</param>
        /// <param name="userSysNo">用户系统编号</param>
        /// <returns>true:用户组存在该用户;false:用户组不存在该用户</returns>
        /// <remarks>2013-10-22 吴文强 创建</remarks>
        public abstract bool GroupContainsUser(int groupSysNo, int userSysNo);
    }
}
