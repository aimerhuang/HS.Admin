using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.DataAccess.Base;
namespace Hyt.DataAccess.Sys
{
    /// <summary>
    /// 任务池自动分配接口
    /// </summary>
    /// <remarks> 
    /// 2013-06-13 郑荣华 创建
    /// </remarks>
    public abstract class ISyJobDispatcher : DaoBase<ISyJobDispatcher>
    {
        /// <summary>
        /// 插入自动分配记录
        /// </summary>
        /// <param name="model">自动分配实体</param>
        /// <returns>返回系统编号</returns>
        /// <remarks>2013-06-21 余勇 创建</remarks>
        public abstract int InsertJobDispatcher(SyJobDispatcher model);

        /// <summary>
        /// 通过用户编号跟任务类型获取订单分配信息
        /// </summary>
        /// <param name="userSysNo">用户编号</param>
        /// <param name="taskType">任务类型</param>
        /// <returns>返回任务分配实体</returns>
        /// <remarks>2013-06-21 余勇 创建</remarks>
        public abstract SyJobDispatcher GetJobDispatcher(int userSysNo, int taskType);

        /// <summary>
        /// 更新自动分配状态
        /// </summary>
        /// <param name="model">自动分配任务实体</param>
        /// <returns>返回系统编号</returns>
        /// <remarks>2013-06-21 余勇 创建</remarks>
        public abstract int UpdateStatus(SyJobDispatcher model);

        /// <summary>
        /// 通过SysNo删除该记录
        /// </summary>
        /// <param name="sysNo">自动分配编号</param>
        /// <returns>影响行数</returns>
        /// <remarks>2013-06-19 余勇 创建</remarks>
        public abstract int DeleteBySysNo(int sysNo);

        /// <summary>
        /// 获取所有记录
        /// </summary>
        /// <returns>list</returns>
        /// <remarks>2013-10-08 朱家宏 创建</remarks>
        public abstract IList<SyJobDispatcher> SelectAll();

        /// <summary>
        /// 通过userSysNo删除记录
        /// </summary>
        /// <param name="userSysNo">用户编号</param>
        /// <returns></returns>
        /// <remarks>2013-10-08 朱家宏 创建</remarks>
        public abstract int DeleteByUserSysNo(int userSysNo);

        /// <summary>
        /// 获取不同用户编号的数据
        /// </summary>
        /// <returns>syUser list</returns>
        /// <remarks>2013-10-08 朱家宏 创建</remarks>
        public abstract IList<SyUser> SelectDistinctUsers();

        /// <summary>
        /// 更新自动分配状态及数量
        /// </summary>
        /// <param name="model">自动分配任务实体</param>
        /// <returns>返回系统编号</returns>
        /// <remarks>2013-06-21 余勇 创建</remarks>
        public abstract int UpdateStatusQuantity(SyJobDispatcher model);
    }
}
