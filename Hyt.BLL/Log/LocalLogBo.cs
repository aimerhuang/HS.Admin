using System;
using System.Configuration;
using System.Globalization;
using System.Text;
using System.IO;

namespace Hyt.BLL.Log
{
    /// <summary>
    /// 本地日志记录
    /// </summary>
    /// <remarks>2014-1-22 黄波 创建</remarks>
    public class LocalLogBo : BOBase<LocalLogBo>
    {
        /// <summary>
        /// 在默认位置记录异常日志
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <returns></returns>
        /// <remarks>2014-1-22 黄波 创建</remarks>
        public void Write(Exception ex, Uri sourceUrl, string userIP = "", string userDns = "")
        {

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("------------------------------------------------------------------------------------------------");
            sb.AppendLine("time:" + DateTime.Now.ToString());
            sb.AppendLine("Url：" + sourceUrl.ToString());
            sb.AppendLine("IP：" + userIP);
            sb.AppendLine("DNS：" + userDns);
            sb.AppendLine(ex.Message);
            sb.AppendLine(ex.StackTrace);
            BaseWrite(sb.ToString(), "SystemException");
        }

        /// <summary>
        /// 在默认位置记录异常日志
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <returns></returns>
        /// <remarks>2014-1-22 黄波 创建</remarks>
        public void Write(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("------------------------------------------------------------------------------------------------");
            sb.AppendLine("time:" + DateTime.Now.ToString());
            sb.AppendLine(ex.Message);
            sb.AppendLine(ex.StackTrace);
            BaseWrite(sb.ToString(), "SystemException");
        }

        /// <summary>
        /// 在默认位置记录异常日志
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <param name="dir">日志目录</param>
        /// <returns></returns>
        /// <remarks>2014-1-22 黄波 创建</remarks>
        public void Write(Exception ex, string dir)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("------------------------------------------------------------------------------------------------");
            sb.AppendLine("time:" + DateTime.Now.ToString());
            sb.AppendLine(ex.Message);
            sb.AppendLine(ex.StackTrace);
            BaseWrite(sb.ToString(), dir);
        }

        /// <summary>
        /// 在默认位置记录异常日志
        /// </summary>
        /// <param name="ex">异常信息</param>
        /// <returns></returns>
        /// <remarks>2014-1-22 黄波 创建</remarks>
        public void Write(string log)
        {

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("------------------------------------------------------------------------------------------------");
            sb.AppendLine("time:" + DateTime.Now.ToString());
            sb.AppendLine(log);
            BaseWrite(sb.ToString(), "textlog");

        }

        /// <summary>
        /// 在默认位置记录在线支付日志
        /// </summary>
        /// <param name="logContent">日志内容</param>
        /// <param name="payType">支付方式</param>
        /// <returns></returns>
        /// <remarks>2014-1-22 黄波 创建</remarks>
        public void Write(string logContent, string payType)
        {
            BaseWrite(logContent, payType);
        }
        /// <summary>
        /// 在指定位置记录日志
        /// </summary>
        /// <param name="logContent">日志内容</param>
        /// <param name="dir">日志目录</param>
        /// <returns></returns>
        /// <remarks>2017-5-20 罗勤尧 创建</remarks>
        public void WriteTo(string logContent, string dir)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("------------------------------------------开始------------------------------------------------------");
            sb.AppendLine("time:" + DateTime.Now.ToString());
            sb.AppendLine("发送参数:" + logContent);
            sb.AppendLine("------------------------------------------结束------------------------------------------------------");
            BaseWrite(sb.ToString(), dir);
        }
        /// <summary>
        /// 在指定位置记录日志
        /// </summary>
        /// <param name="logContent">日志内容</param>
        /// <param name="dir">日志目录</param>
        /// <returns></returns>
        /// <remarks>2017-5-20 罗勤尧 创建</remarks>
        public void WriteReturn(string logContent, string dir)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("------------------------------------------开始------------------------------------------------------");
            sb.AppendLine("time:" + DateTime.Now.ToString());
            sb.AppendLine("返回参数:" + logContent);
            sb.AppendLine("------------------------------------------结束------------------------------------------------------");
            BaseWrite(sb.ToString(), dir);
        }

        /// <summary>
        /// 记录日志信息
        /// </summary>
        /// <param name="logContent">日志内容</param>
        /// <param name="dir">日志目录</param>
        /// <returns></returns>
        /// <remarks>2014-1-22 黄波 创建</remarks>
        private void BaseWrite(string logContent, string dir)
        {
            try
            {
                var _localLogPath = ConfigurationManager.AppSettings["LocalLogPath"];
                _localLogPath = !string.IsNullOrWhiteSpace(_localLogPath) ? _localLogPath : Environment.GetEnvironmentVariable("windir").ToLower().Replace("windows", @"/WebLog");

                var _logFilePath = _localLogPath + "/" + dir + "/";
                var _logFileName = DateTime.Now.ToString("yyyy-MM-dd") + ".txt";

                if (!Directory.Exists(_logFilePath)) Directory.CreateDirectory(_logFilePath);
                File.AppendAllText(_logFilePath + _logFileName, logContent);
            }
            catch { }
        }

    }
}
