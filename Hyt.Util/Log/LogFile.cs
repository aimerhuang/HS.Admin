using System;
using System.Diagnostics;
using System.IO;

namespace Hyt.Util.Log
{
    /// <summary>
    /// 日志Helper    
    /// </summary>
    /// <remarks>2013-3-21 添加 杨浩</remarks>
    internal  class LogFile : ILog
    {
        private static string SysErrLogSavePath =AppDomain.CurrentDomain.BaseDirectory;
        private object lockLog = new object();
        /// <summary>
        /// 在默认位置记录异常日志
        /// </summary>
        /// <param name="ex">异常</param>
        /// <param name="level">日志等级.</param>
        /// <returns></returns>
        /// <remarks>
        /// 2014-01-03 何方 创建
        /// </remarks>
        public string WriteLog(Exception ex, Level level = Level.Info)
        {
           return WriteLog(getErrMsg(ex));
        }

        /// <summary>
        /// 在默认位置记录异常日志
        /// </summary>
        /// <param name="filepath">The filepath.</param>
        /// <param name="ex">异常</param>
        /// <param name="level">日志等级 level.</param>
        /// <returns></returns>
        /// <remarks>
        /// 2013-04-03 何方 创建
        /// </remarks>
        public string WriteLog(string filepath, Exception ex, Level level = Level.Info)
        {
            return WriteLog(filepath,getErrMsg(ex));
        }

        /// <summary>
        /// 在指定位置写日志
        /// </summary>
        /// <param name="message">日志内容</param>
        /// <param name="level">日志等级 level.</param>
        /// <returns></returns>
        /// <remarks>
        /// 2014-01-03 何方 创建
        /// </remarks>
        public string WriteLog(string message, Level level = Level.Info)
        {
           return WriteLog(SysErrLogSavePath, message);
        }

        /// <summary>
        /// 记录log日志
        /// </summary>
        /// <param name="filepath">文件路径，硬盘地址</param>
        /// <param name="str">要写入的内容</param>
        /// <param name="level">日志等级 level.</param>
        /// <returns></returns>
        /// <remarks>
        /// 2014-01-03 何方 创建
        /// </remarks>
        public string WriteLog(string filepath, string str, Level level = Level.Info)
        {
            string sysID = string.Empty;
            lock (lockLog)
            {
                try
                {
                    if (!Directory.Exists(filepath)) Directory.CreateDirectory(filepath);
                    string filename = DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
                    sysID = Guid.NewGuid().ToString();
                    using (var sw = new StreamWriter(filepath + "\\" + filename, true, System.Text.Encoding.Default))
                    {
                        sw.WriteLine("-----------------------------------------------------");
                        sw.WriteLine("日志ID：{0}", sysID);
                        sw.WriteLine("{0}:{1}\t{2}", DateTime.Now.ToLongTimeString(), DateTime.Now.Millisecond, str);
                        sw.Close();
                    }
                }
                catch
                {
                    //EventLog.WriteEntry("HYTLOG",
                    //                    string.Format("文件日志写入失败{0},日志内容{1}", filepath, str),EventLogEntryType.Error);
                }
            }
            return sysID;
        }

        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="fileName">文件名</param>
        /// <param name="content">内容</param>
        /// <remarks>
        /// 2013-12-25 黄志 创建
        /// </remarks>
        public void WriteLog(string filePath, string fileName, string content)
        {
            var fileFullName = string.Format(@"{0}\{1}", filePath, fileName);
            if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);
            StreamWriter sw = null;
            FileStream fs = null;
            try
            {
                fs = new FileStream(fileFullName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                sw = new StreamWriter(fs, System.Text.Encoding.Default);
                sw.WriteLine(content);
            }
            catch
            {
                //
            }
            finally
            {
                if (sw != null)
                {
                    sw.Flush();
                    sw.Close();
                }
                if (fs != null)
                {
                    fs.Close();
                }
            }
        }

        /// <summary>
        /// 获取错误详细信息。
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        /// <remarks>
        /// 2014-01-03 何方 创建
        /// </remarks>
        private static string getErrMsg(Exception ex)
        {
            if (ex == null) return string.Empty;
            string errMessage = "";
            for (Exception tempException = ex; tempException != null; tempException = tempException.InnerException)
            {
                errMessage += tempException.Message + Environment.NewLine + Environment.NewLine;
            }
            errMessage += ex.ToString();
            return errMessage;
        }
    }
}
