using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Hyt.Util
{
    /// <summary>
    /// 文件工具类
    /// </summary>
    /// <remarks>2013-08-13 唐永勤 创建</remarks>
    public class FileUtil
    {
        /// <summary>
        /// 读取文件内容
        /// </summary>
        /// <param name="path">文件全路径</param>
        /// <returns>文件内容</returns>
        /// <remarks>2013-08-13 唐永勤 创建</remarks>
        public static string ReadFile(string path)
        {
            string content = "";
            if (File.Exists(path))
            {
                StreamReader sr = null;
                try
                {
                    sr = new StreamReader(path);
                    content = sr.ReadToEnd();
                }
                catch { }
                finally
                {
                    sr.Close();
                    sr.Dispose();
                }

            }
            return content;
        }

        /// <summary>
        /// 文件是否存在
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>存在:true  不存在:false</returns>
        /// <remarks>2013-12-30 黄波 创建</remarks>
        public static Boolean HasFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath)) return false;
            return File.Exists(filePath);
        }
    }
}
