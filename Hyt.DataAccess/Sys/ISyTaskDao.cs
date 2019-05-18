using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;

namespace Hyt.DataAccess.Sys
{
    /// <summary>
    /// 任务
    /// </summary>
    /// <remarks>2013-10-15 杨浩 创建</remarks>
    public abstract class ISyTaskDao : DaoBase<ISyTaskDao>
    {
        /// <summary>
        /// Add
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>sysNo</returns>
        /// <remarks>2013-10-15 杨浩 创建</remarks>
        public abstract int Add(SyTaskConfig model);

        /// <summary>
        /// GetAll
        /// </summary>
        /// <returns>IList<SyTaskBo></returns>
        /// <remarks>2013-10-16 杨浩 创建</remarks>
        public abstract IList<SyTaskConfig> GetAll();

        /// <summary>
        /// 获取任务
        /// </summary>
        /// <param name="sysNo">任务系统编号</param>
        /// <returns>SyTaskConfig</returns>
        /// <remarks>2013-10-16 杨浩 创建</remarks>
        public abstract SyTaskConfig GetTask(int sysNo);

        /// <summary>
        /// 更新任务
        /// </summary>
        /// <param name="task">任务</param>
        /// <returns></returns>
        /// <remarks>2013-10-16 杨浩 创建</remarks>
        public abstract int UpdateTask(SyTaskConfig task);

        /// <summary>
        /// 查看任务执行日志
        /// </summary>
        /// <param name="sysNo">任务计划编号</param>
        /// <param name="currentPage">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <returns>Pager<SyTaskLog></returns>
        /// <remarks>2013-10-16 杨浩 创建</remarks>
        public abstract Pager<SyTaskLog> GetLogs(int sysNo, int currentPage, int pageSize);

        /// <summary>
        /// 添加一条任务执行日志
        /// </summary>
        /// <param name="model">任务计划日志</param>
        /// <returns>int</returns>
        /// <remarks>2013-10-16 杨浩 创建</remarks>
        public abstract int AddTaskLog(SyTaskLog model);

        /// <summary>
        /// 定时清理任务日志
        /// </summary>
        /// <param name="sysNo">任务计划编号</param>
        /// <returns>int</returns>
        /// <remarks>2013-10-18 杨浩 创建</remarks>
        public abstract void ClearTaskLog(int sysNo);

        /// <summary>
        /// 发送短信任务
        /// </summary>
        /// <param name="sendCount">发送条数</param>
        /// <returns>返回待发送Top条数</returns>
        /// <remarks>2013-10-22 苟治国 创建</remarks>
        public abstract IList<NcSms> GetSmsTask(int sendCount);

        /// <summary>
        /// 更新发送短信失败状态
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <param name="status">状态</param>
        /// <returns>返回操作状态</returns>
        /// <remarks>2013-10-22 苟治国 创建</remarks>
        public abstract int UpdateSmsTaskStatus(int sysNo, Hyt.Model.WorkflowStatus.NotificationStatus.短信发送状态 status);
    }
}
