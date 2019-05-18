using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Hyt.Util
{
    public class HttpHelper
    {
        #region Http Post请求
        /// <summary>
        /// Http Post请求
        /// </summary>
        /// <param name="url">请求的路径</param>
        /// <param name="parameters">NameValueCollection类集合 一键多值</param>
        /// <returns>string</returns>
        /// 2017/10/12 吴琨 创建
        public static string PostData(string url, NameValueCollection parameters)
        {
            var req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";

            var parassb = new StringBuilder();
            foreach (string key in parameters.Keys)
            {
                if (parassb.Length > 0)
                    parassb.Append("&");
                parassb.AppendFormat("{0}={1}", key, HttpUtility.UrlEncode(parameters[key]));
            }
            var data = Encoding.UTF8.GetBytes(parassb.ToString());
            var reqstream = req.GetRequestStream();
            reqstream.Write(data, 0, data.Length);
            reqstream.Close();
            string result;
            // ReSharper disable once AssignNullToNotNullAttribute
            using (var reader = new StreamReader(stream: req.GetResponse().GetResponseStream(), encoding: Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }
        #endregion


        public static string HttpGet(string Url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }
    }
}
