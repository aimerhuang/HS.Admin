using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace Hyt.Util
{
    /// <summary>
    /// 网络工具类
    /// </summary>
    /// <remarks>2013-12-1 黄波 创建</remarks>
    public class NetUtil
    {
        /// <summary>
        /// 获取请求地址的返回值
        /// </summary>
        /// <param name="httpUrl">请求的HTTP地址</param>
        /// <returns>服务器返回内容</returns>
        /// <remarks>2013-12-1 黄波 创建</remarks>
        public static string GetHttpRequest(string httpUrl)
        {
            var webReqponseStream = HttpWebRequest.Create(httpUrl).GetResponse().GetResponseStream();
            using (var sr = new StreamReader(webReqponseStream))
            {
                return sr.ReadToEnd();
            }

        }
    }
}
