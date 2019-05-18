using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Hyt.BLL.Extras;
using Hyt.DataAccess.Log;
using Hyt.Model;
using Hyt.Model.Parameter;
using Hyt.Model.WorkflowStatus;
using Hyt.Util;
using System.Threading;
using System.Threading.Tasks;

namespace Hyt.BLL.Log
{
    /// <summary>
    /// 系统日志
    /// </summary>
    /// <remarks>2013-06-21 吴文强 创建</remarks>
    public class SysLog : BOBase<SysLog>, ISysLog
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
        public void Debug(LogStatus.系统日志来源 source, string message, LogStatus.系统日志目标类型 targetType, int targetSysNo, Exception exception , string logIP , int operater)
        {
            WriteLog(LogStatus.SysLogLevel.Debug, source, message, targetType, targetSysNo, exception, logIP, operater);
        }
        #region info 消息
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
        /// <remarks>
        /// 2013-08-26 何方 创建
        /// </remarks>
        public void Info(LogStatus.系统日志来源 source, string message, LogStatus.系统日志目标类型 targetType, int targetSysNo, Exception exception , string logIP , int operater )
        {
            WriteLog(LogStatus.SysLogLevel.Info, source, message, targetType, targetSysNo, exception, logIP, operater);
        }
        /// <summary>
        /// 消息
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="message">The message.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="targetSysNo">The target sys no.</param>
        /// <param name="logIP">The log IP.</param>
        /// <param name="operater">The operater.</param>
        /// <remarks>
        /// 2013-08-26 何方 创建
        /// </remarks>
        public void Info(LogStatus.系统日志来源 source, string message, LogStatus.系统日志目标类型 targetType, int targetSysNo, string logIP, int operater )
        {
            WriteLog(LogStatus.SysLogLevel.Info, source, message, targetType, targetSysNo, null, logIP, operater);
        }

    
        /// <summary>
        /// 消息
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="message">The message.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="targetSysNo">The target sys no.</param>
        /// <param name="operater">The operater.</param>
        /// <remarks>
        /// 2013-08-26 何方 创建
        /// </remarks>
        public void Info(LogStatus.系统日志来源 source, string message, LogStatus.系统日志目标类型 targetType, int targetSysNo,  int operater )
        {
            WriteLog(LogStatus.SysLogLevel.Info, source, message, targetType, targetSysNo, null, "", operater);
        }
        /// <summary>
        /// 消息
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="message">The message.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="targetSysNo">The target sys no.</param>
        /// <remarks>
        /// 2013-08-26 何方 创建
        /// </remarks>
        [Obsolete("建议使用带操作人的日志记录方法")]
        public void Info(LogStatus.系统日志来源 source, string message, LogStatus.系统日志目标类型 targetType, int targetSysNo)
        {

            WriteLog(LogStatus.SysLogLevel.Info, source, message, targetType, targetSysNo, null,"", 
                BLL.Authentication.AdminAuthenticationBo.Instance.Current==null?0 : BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo);
        }
        #endregion

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="message">The message.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="targetSysNo">The target sys no.</param>
        /// <param name="operater">The operater.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="logIP">The log IP.</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-08-26 何方 创建
        /// </remarks>
        public void Warn(LogStatus.系统日志来源 source, string message, LogStatus.系统日志目标类型 targetType, int targetSysNo, int operater,Exception exception = null, string logIP = "")
        {
            WriteLog(LogStatus.SysLogLevel.Warn, source, message, targetType, targetSysNo, exception, logIP, operater);
        }

        #region 错误

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        /// <remarks>
        /// 2013-08-26 何方 创建
        /// </remarks>
        public void Error(LogStatus.系统日志来源 source, string message,  Exception exception)
        {
            Error(source, message, 0, 0, exception, 0);
        }

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="message">The message.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="targetSysNo">The target sys no.</param>
        /// <param name="exception">The exception.</param>
        /// <remarks>
        /// 2013-08-26 何方 创建
        /// </remarks>
             public void Error(LogStatus.系统日志来源 source, string message, LogStatus.系统日志目标类型 targetType, int targetSysNo, Exception exception)
        {
            Error(source, message, targetType, targetSysNo,exception, 0);
        }

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="message">The message.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="targetSysNo">The target sys no.</param>
        /// <remarks>
        /// 2013-08-26 何方 创建
        /// </remarks>
        public void Error(LogStatus.系统日志来源 source, string message, LogStatus.系统日志目标类型 targetType, int targetSysNo)
        {

            Error(source, message, targetType, targetSysNo,BLL.Authentication.AdminAuthenticationBo.Instance.Current.Base.SysNo );
        }

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="message">The message.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="targetSysNo">The target sys no.</param>
        /// <param name="operater">The operater.</param>
        /// <remarks>
        /// 2013-08-26 何方 创建
        /// </remarks>
        public void Error(LogStatus.系统日志来源 source, string message, LogStatus.系统日志目标类型 targetType, int targetSysNo, int operater )
        {
            Error(source, message, targetType, targetSysNo, null, "",operater);
        }

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="message">The message.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="targetSysNo">The target sys no.</param>
        /// <param name="logIP">The log IP.</param>
        /// <param name="operater">The operater.</param>
        /// <remarks>
        /// 2013-08-26 何方 创建
        /// </remarks>
        public void Error(LogStatus.系统日志来源 source, string message, LogStatus.系统日志目标类型 targetType, int targetSysNo, string logIP , int operater)
        {
            Error(source, message, targetType, targetSysNo, null, logIP, operater);
        }

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="message">The message.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="targetSysNo">The target sys no.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="operater">The operater.</param>
        /// <remarks>
        /// 2013-08-26 何方 创建
        /// </remarks>
        public void Error(LogStatus.系统日志来源 source, string message, LogStatus.系统日志目标类型 targetType, int targetSysNo, Exception exception , int operater )
        {
            Error(source, message, targetType, targetSysNo, exception, null, operater);
        }

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
        /// <remarks>
        /// 2013-08-26 何方 创建
        /// </remarks>
        public void Error(LogStatus.系统日志来源 source, string message, LogStatus.系统日志目标类型 targetType, int targetSysNo, Exception exception , string logIP , int operater)
        {
            WriteLog(LogStatus.SysLogLevel.Error, source, message, targetType, targetSysNo, exception, logIP, operater);
        }

    
        #endregion

        /// <summary>
        /// 致命错误
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="message">The message.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="targetSysNo">The target sys no.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="logIP">The log IP.</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-08-26 何方 创建
        /// </remarks>
        public void Fatal(LogStatus.系统日志来源 source, string message, LogStatus.系统日志目标类型 targetType, int targetSysNo, Exception exception , string logIP )
        {
            Fatal(source, message, targetType, targetSysNo, exception, logIP, 0);
        }

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
        /// <returns></returns>
        /// <remarks>
        /// 2013-08-26 何方 创建
        /// </remarks>
        public void Fatal(LogStatus.系统日志来源 source, string message, LogStatus.系统日志目标类型 targetType, int targetSysNo, Exception exception , string logIP , int operater )
        {
            WriteLog(LogStatus.SysLogLevel.Fatal, source, message, targetType, targetSysNo, exception, logIP, operater);
        }

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
        public void WriteLog(LogStatus.SysLogLevel level, LogStatus.系统日志来源 source, string message,
                             LogStatus.系统日志目标类型 targetType, int targetSysNo, Exception exception = null,
                             string logIP = "", int operater = 0)
        {

            if (operater == 0)
            {
                var user = Authentication.AdminAuthenticationBo.Instance.Current;
                if (user != null)
                {
                    operater = user.Base.SysNo;
                }
            }

            if (string.IsNullOrWhiteSpace(logIP))
            {
                logIP = WebUtil.GetUserIp();
            }

            Task.Factory.StartNew(() =>
                {
                    try
                    {
                     
                        ISySystemLogDao.Instance.Create(new SySystemLog
                            {
                                Exception = exception == null ? "" : exception+(exception.StackTrace ?? ""),
                                LogDate = DateTime.Now,
                                LogIp = logIP,
                                LogLevel = (int) level,
                                Message = message,
                                Operator = operater,
                                Source = (int) source,
                                TargetType = targetType.GetHashCode(),
                                TargetSysNo = targetSysNo
                            });

                        #region 发送错误日志到邮箱

                        if (level == LogStatus.SysLogLevel.Error ||
                            level == LogStatus.SysLogLevel.Fatal ||
                            level == LogStatus.SysLogLevel.Warn)
                        {

                            var hostName = Dns.GetHostName();
                            var subject = string.Format("SysLog:{0},Level{1},{2}", source, level, message);
                            EmailBo.Instance.SendMail("all.list@pisendev.com", subject,
                                                      string.Format(
                                                          " message:{0}\n\r exception:{1}\n\r targetType:{2}\n\r targetSysNo:{3}\n\r logIP:{4}\n\r operater:{5} \n\r host:{6}",
                                                          message, exception == null ? "null" : exception.ToString(),
                                                          targetType,
                                                          targetSysNo, logIP, operater, hostName));
                            Console.WriteLine("send error mail");
                            LocalLogBo.Instance.Write("send error mail");
                        }

                        #endregion

                     }
                    catch (Exception ex)
                    {
                        LocalLogBo.Instance.Write(ex);
                    }
                });

        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="filter">参数</param>
        /// <returns>分页</returns>
        /// <remarks>2013-08-15 朱家宏 创建</remarks>
        public Pager<SySystemLog> GetLogs(ParaSystemLogFilter filter)
        {
            return ISySystemLogDao.Instance.Query(filter);
        }

        /// <summary>
        /// 获取日志信息
        /// </summary>
        /// <param name="source">系统日志来源</param>
        /// <param name="targetType">系统日志目标类型</param>
        /// <param name="targetSysNo">来源系统编号</param>
        /// <returns>系统日志列表</returns>
        /// <remarks>2013-09-23 沈强 创建</remarks>
        public IList<SySystemLog> Get(LogStatus.系统日志来源 source,
                                      LogStatus.系统日志目标类型 targetType, int targetSysNo)
        {
            return ISySystemLogDao.Instance.Get(source,
                                                targetType, targetSysNo);
        }
        #region 平台公司名
        public string GetPlatCompanyName()
        {
            return "商城";
        }
        #endregion
    }
}
