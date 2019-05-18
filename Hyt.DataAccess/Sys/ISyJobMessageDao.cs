
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.Parameter;
using System.Collections.Generic;
namespace Hyt.DataAccess.Sys
{
    /// <summary>
    /// 任务消息
    /// </summary>
    /// <remarks>2015-01-21  杨浩 创建</remarks>
    public abstract class ISyJobMessageDao : DaoBase<ISyJobMessageDao>
    {

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns>新增记录编号</returns>
        /// <remarks>2015-01-21  杨浩 创建</remarks>
        public abstract int Insert(SyJobMessage entity);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        /// <remarks>2015-01-21  杨浩 创建</remarks>
        public abstract void Update(SyJobMessage entity);

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns>数据实体</returns>
        /// <remarks>2015-01-21  杨浩 创建</remarks>
        public abstract SyJobMessage GetEntity(int sysNo);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sysNo">编号</param>
        /// <returns></returns>
        /// <remarks>2015-01-21  杨浩 创建</remarks>
        public abstract void Delete(int sysNo);

        /// <summary>
        /// 获取消息列表
        /// </summary>
        /// <param name="typeid">类型编号</param>
        /// <returns></returns>
        /// <remarks>2015-01-21  杨浩 创建</remarks>
        public abstract List<SyJobMessage> GetListByMessageType(int? typeid);

    }
}
