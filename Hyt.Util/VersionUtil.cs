using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Util
{
    /// <summary>
    /// 版本功能辅助类
    /// </summary>
    /// <remarks>
    /// 用于程序发布之后，配置程序调用不同的功能版本代码
    /// </remarks>
    public static class VersionUtil
    {
        private static readonly string _path = System.AppDomain.CurrentDomain.BaseDirectory + "version.json";
                

        /// <summary>
        ///  是否包含指定名称的功能(不区分大小写)
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool Contains(string name) {
            if (System.IO.File.Exists(_path) == true) {
                string versionNames = System.IO.File.ReadAllText(_path);
                if (string.IsNullOrEmpty(versionNames) == true) return false;
                foreach (var versionName in versionNames.Split(','))
                {
                    if (versionName.ToLower() == name.ToLower()) {
                        return true;
                    }
                }

            }
            return false;
        }

    }
}
