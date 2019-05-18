using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.DataAccess.Base;
namespace Hyt.DataAccess.Sys
{
    /// <summary>
    /// 任务池优先级接口类
    /// </summary>
    /// <remarks>2014-02-28 余勇 创建</remarks>
    public abstract class ISyJobPoolPriorityDao : DaoBase<ISyJobPoolPriorityDao>
    {
        /// <summary>
        /// 插入实体记录
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>返回系统编号</returns>
        /// <remarks>2014-02-28 余勇 创建</remarks>
        public abstract int Insert(SyJobPoolPriority model);

        /// <summary>
        /// 通过系统编号获取实体信息
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>返回实体</returns>
        /// <remarks>2014-02-28 余勇 创建</remarks>
        public abstract SyJobPoolPriority Get(int sysNo);

        /// <summary>
        /// 通过优先级编码获取实体信息
        /// </summary>
        /// <param name="code">优先级编码</param>
        /// <returns>返回实体</returns>
        /// <remarks>2014-02-28 余勇 创建</remarks>
        public abstract SyJobPoolPriority GetByPriorityCode(string code);

        /// <summary>
        /// 通过SysNo删除该记录
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <returns>影响行数</returns>
        /// <remarks>2014-02-28 余勇 创建</remarks>
        public abstract int Delete(int sysNo);

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="model">实体信息</param>
        /// <returns>返回影响行数</returns>
        /// <remarks>2014-02-28 余勇 创建</remarks>
        public abstract int Update(SyJobPoolPriority model);

        /// <summary>
        /// 获取所有记录
        /// </summary>
        /// <returns>list</returns>
        /// <remarks>2014-02-28 余勇 创建</remarks>
        public abstract IList<SyJobPoolPriority> SelectAll();

        /// <summary>
        /// 通过优先级获取实体信息
        /// </summary>
        /// <param name="priority">优先级</param>
        /// <returns>返回实体</returns>
        /// <remarks>2014-02-28 余勇 创建</remarks>
        public abstract SyJobPoolPriority GetByPriority(int priority);
    }
}
