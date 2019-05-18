using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Util.Log
{
    /// <summary>
    /// 日志接口
    /// </summary>
    /// <remarks>2013-3-21 杨浩 添加</remarks>
    public interface ILog
    {
        /// <summary>
        /// 在默认位置记录异常日志
        /// </summary>
        /// <param name="ex">异常</param>
        /// <param name="level">日志等级 level.</param>
        /// <returns>
        /// 返回日志ID
        /// </returns>
        /// <remarks>
        /// 2014-01-03 何方 创建
        /// </remarks>
        string WriteLog(Exception ex, Level level = Level.Info);

        /// <summary>
        /// 在指定位置记录异常日志
        /// </summary>
        /// <param name="filepath">日志路径</param>
        /// <param name="ex">异常</param>
        /// <param name="level">日志等级 level.</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-04-03 何方 创建
        /// </remarks>
        string WriteLog(string filepath, Exception ex, Level level = Level.Info);

        /// <summary>
        /// 在默认位置写日志
        /// </summary>
        /// <param name="message">日志内容</param>
        /// <param name="level">日志等级 level.</param>
        /// <returns>
        /// 返回日志ID
        /// </returns>
        /// <remarks>
        /// 2014-01-03 何方 创建
        /// </remarks>
        string WriteLog(string message, Level level = Level.Info);

        /// <summary>
        /// 在指定位置写日志
        /// </summary>
        /// <param name="filepath">日志路径</param>
        /// <param name="message">日志内容</param>
        /// <param name="level">日志等级 level.</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-04-03 何方 创建
        /// </remarks>
        string WriteLog(string filepath, string message, Level level = Level.Error);

        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="fileName">文件名</param>
        /// <param name="content">内容</param>
        /// <remarks>
        /// 2013-12-25 黄志 创建
        /// </remarks>
        void WriteLog(string filePath, string fileName, string content);
    }

    /// <summary>
    /// 日志类型
    /// </summary>
    /// <remarks>2014-1-21 杨浩 创建</remarks>
    public enum Level : byte
    {
        /// <summary>
        /// 信息
        /// </summary>
        Info = 1,

        /// <summary>
        /// 警告
        /// </summary>
        Warning,

        /// <summary>
        /// 错误记录
        /// </summary>
        Debug,

        /// <summary>
        /// 异常
        /// </summary>
        Error
    }
}
