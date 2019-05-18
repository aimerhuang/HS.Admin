using System;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.WorkflowStatus;

namespace Hyt.BLL.Log
{
    public interface ISysLog
    {
        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="message">The message.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="targetSysNo">The target sys no.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="logIP">The log IP.</param>
        /// <param name="operater">The operater.</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-08-26 何方 创建
        /// </remarks>
        void Debug(LogStatus.系统日志来源 source, string message, LogStatus.系统日志目标类型 targetType, int targetSysNo, Exception exception , string logIP , int operater);

        /// <summary>
        /// 消息
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="message">The message.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="targetSysNo">The target sys no.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="logIP">The log IP.</param>
        /// <param name="operater">The operater.</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-08-26 何方 创建
        /// </remarks>
        void Info(LogStatus.系统日志来源 source, string message, LogStatus.系统日志目标类型 targetType, int targetSysNo, Exception exception , string logIP , int operater );

        /// <summary>
        /// 消息
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="message">The message.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="targetSysNo">The target sys no.</param>
        /// <param name="logIP">The log IP.</param>
        /// <param name="operater">The operater.</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-08-26 何方 创建
        /// </remarks>
        void Info(LogStatus.系统日志来源 source, string message, LogStatus.系统日志目标类型 targetType, int targetSysNo, string logIP, int operater );

        /// <summary>
        /// 消息
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="message">The message.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="targetSysNo">The target sys no.</param>
        /// <param name="operater">The operater.</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-08-26 何方 创建
        /// </remarks>
        void Info(LogStatus.系统日志来源 source, string message, LogStatus.系统日志目标类型 targetType, int targetSysNo,  int operater );

        /// <summary>
        /// 消息
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="message">The message.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="targetSysNo">The target sys no.</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-08-26 何方 创建
        /// </remarks>
        void Info(LogStatus.系统日志来源 source, string message, LogStatus.系统日志目标类型 targetType, int targetSysNo);

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="message">The message.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="targetSysNo">The target sys no.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="logIP">The log IP.</param>
        /// <param name="operater">The operater.</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-08-26 何方 创建
        /// </remarks>
        void Warn(LogStatus.系统日志来源 source, string message, LogStatus.系统日志目标类型 targetType, int targetSysNo, int operater , Exception exception = null, string logIP = "");

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-08-26 何方 创建
        /// </remarks>
        void Error(LogStatus.系统日志来源 source, string message,  Exception exception);

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="message">The message.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="targetSysNo">The target sys no.</param>
        /// <param name="exception">The exception.</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-08-26 何方 创建
        /// </remarks>
        void Error(LogStatus.系统日志来源 source, string message, LogStatus.系统日志目标类型 targetType, int targetSysNo, Exception exception);

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="message">The message.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="targetSysNo">The target sys no.</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-08-26 何方 创建
        /// </remarks>
        void Error(LogStatus.系统日志来源 source, string message, LogStatus.系统日志目标类型 targetType, int targetSysNo);

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="message">The message.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="targetSysNo">The target sys no.</param>
        /// <param name="operater">The operater.</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-08-26 何方 创建
        /// </remarks>
        void Error(LogStatus.系统日志来源 source, string message, LogStatus.系统日志目标类型 targetType, int targetSysNo, int operater );

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="message">The message.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="targetSysNo">The target sys no.</param>
        /// <param name="logIP">The log IP.</param>
        /// <param name="operater">The operater.</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-08-26 何方 创建
        /// </remarks>
        void Error(LogStatus.系统日志来源 source, string message, LogStatus.系统日志目标类型 targetType, int targetSysNo, string logIP , int operater);

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="message">The message.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="targetSysNo">The target sys no.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="operater">The operater.</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-08-26 何方 创建
        /// </remarks>
        void Error(LogStatus.系统日志来源 source, string message, LogStatus.系统日志目标类型 targetType, int targetSysNo, Exception exception , int operater );

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="message">The message.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="targetSysNo">The target sys no.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="logIP">The log IP.</param>
        /// <param name="operater">The operater.</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-08-26 何方 创建
        /// </remarks>
        void Error(LogStatus.系统日志来源 source, string message, LogStatus.系统日志目标类型 targetType, int targetSysNo, Exception exception , string logIP , int operater);

        /// <summary>
        /// 致命错误
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="message">The message.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="targetSysNo">The target sys no.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="logIP">The log IP.</param>
        /// <remarks>
        /// 2013-08-26 何方 创建
        /// </remarks>
        void Fatal(LogStatus.系统日志来源 source, string message, LogStatus.系统日志目标类型 targetType, int targetSysNo, Exception exception , string logIP );

        /// <summary>
        /// 致命错误
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="message">The message.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="targetSysNo">The target sys no.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="logIP">The log IP.</param>
        /// <param name="operater">The operater.</param>
        /// <remarks>
        /// 2013-08-26 何方 创建
        /// </remarks>
        void Fatal(LogStatus.系统日志来源 source, string message, LogStatus.系统日志目标类型 targetType, int targetSysNo, Exception exception , string logIP , int operater );

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="level">SysLogLevel.</param>
        /// <param name="source">系统日志来源.</param>
        /// <param name="message">The message.</param>
        /// <param name="targetType">操作目标类型.</param>
        /// <param name="targetSysNo">操作目标系统编号.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="logIP">The log IP.</param>
        /// <param name="operater">The operater.</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-07-11 何方 创建
        /// 2013-08-26 何方 创建
        /// </remarks>
        void WriteLog(LogStatus.SysLogLevel level, LogStatus.系统日志来源 source, string message, LogStatus.系统日志目标类型 targetType,int targetSysNo, Exception exception = null,string logIP="",int operater=0);

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="filter">参数</param>
        /// <returns>分页</returns>
        /// <remarks>2013-08-15 朱家宏 创建</remarks>
        Pager<SySystemLog> GetLogs(ParaSystemLogFilter filter);

        /// <summary>
        /// 获取日志信息
        /// </summary>
        /// <param name="source">系统日志来源</param>
        /// <param name="targetType">系统日志目标类型</param>
        /// <param name="targetSysNo">来源系统编号</param>
        /// <returns>系统日志列表</returns>
        /// <remarks>2013-09-23 沈强 创建</remarks>
        System.Collections.Generic.IList<SySystemLog> Get(LogStatus.系统日志来源 source,
                                                          LogStatus.系统日志目标类型 targetType, int targetSysNo);
    }
}