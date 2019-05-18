
using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Parameter;
namespace Hyt.DataAccess.Sys
{
    /// <summary>
    /// 用户组
    /// </summary>
    /// <remarks>2013-07-30  朱成果 创建</remarks>
    public abstract class ISyUserGroupDao : DaoBase<ISyUserGroupDao>
    {

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2013-07-30  朱成果 创建</remarks>
        public abstract int Insert(SyUserGroup entity);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        /// <remarks>2013-07-30  朱成果 创建</remarks>
        public abstract void Update(SyUserGroup entity);

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2013-07-30  朱成果 创建</remarks>
        public abstract SyUserGroup GetEntity(int sysNo);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns></returns>
        /// <remarks>2013-07-30  朱成果 创建</remarks>
        public abstract void Delete(int sysNo);


        /// <summary>
        /// 根据用户组名获取用户组
        /// </summary>
        /// <param name="groupName">用户组名</param>
        /// <returns></returns>
        /// <remarks>2013-08-01  朱成果 创建</remarks>
        public abstract SyUserGroup GetByGroupName(string groupName);


        /// <summary>
        /// 获取全部用户组
        /// </summary>
        /// <returns>用户组列表</returns>
        /// <remarks>2013-08-05  黄志勇 创建</remarks>
        public abstract IList<SyUserGroup> GetAllSyGroup();

        /// <summary>
        /// 检查当前用户组是否在使用
        /// </summary>
        /// <param name="sysNo">用户组编号</param>
        /// <returns></returns>
        /// <remarks>2013-08-05  朱成果 创建</remarks> 
        public abstract bool IsBeingUsed(int sysNo);
    }
}
