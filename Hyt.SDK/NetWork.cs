using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Hyt.SDK
{
    public class NetWork
    {
        /// <summary>
        /// 执行一个 HTTP 请求
        /// </summary>
        /// <param name="url">执行请求的URL</param>
        /// <param name="param">表单参数</param>
        /// <param name="cookie">cookie参数 </param>
        /// <param name="method">请求方法 post / get</param>
        ///  <param name="protocol"> http协议类型 http / https</param>
        /// <returns>返回结果数组</returns>
        static public ResponseResult MakeRequest(string url, Dictionary<string, string> param, Dictionary<string, string> cookie, string method, string protocol)
        {
            string query_string = MakeQueryString(param);
            string cookie_string = MakeCookieString(cookie);
            //结果
            ResponseResult result = new ResponseResult();
            //请求类
            HttpWebRequest request = null;
            //请求响应类
            HttpWebResponse response = null;
            //响应结果读取类
            StreamReader reader = null;

            //http连接数限制默认为2，多线程情况下可以增加该连接数，非多线程情况下可以注释掉此行代码
            ServicePointManager.DefaultConnectionLimit = 500;

            try
            {

                if (method.Equals("get", StringComparison.OrdinalIgnoreCase))
                {

                    if (url.IndexOf("?") > 0)
                    {
                        url = url + "&" + query_string;
                    }
                    else
                    {
                        url = url + "?" + query_string;
                    }
                    //如果是发送HTTPS请求   
                    if (protocol.Equals("https", StringComparison.OrdinalIgnoreCase))
                    {
                        ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                        request = WebRequest.Create(url) as HttpWebRequest;
                        request.ProtocolVersion = HttpVersion.Version10;
                    }
                    else
                    {
                        request = WebRequest.Create(url) as HttpWebRequest;
                    }
                    request.Method = "GET";
                    request.Timeout = 30000;

                }
                else
                {
                    //如果是发送HTTPS请求   
                    if (protocol.Equals("https", StringComparison.OrdinalIgnoreCase))
                    {
                        ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                        request = WebRequest.Create(url) as HttpWebRequest;
                        request.ProtocolVersion = HttpVersion.Version10;
                    }
                    else
                    {
                        request = WebRequest.Create(url) as HttpWebRequest;
                    }
                    //去掉“Expect: 100-Continue”请求头，不然会引起post（417） expectation failed
                    ServicePointManager.Expect100Continue = false;

                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.Timeout = 30000;
                    //POST数据   
                    byte[] data = Encoding.UTF8.GetBytes(query_string);
                    using (Stream stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                }

                //cookie
                if (string.IsNullOrEmpty(cookie_string) == false)
                {
                    request.Headers.Add("Cookie", cookie_string);
                }

                //response
                response = (HttpWebResponse)request.GetResponse();
                reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);

                //return
                result.Msg = reader.ReadToEnd();
                result.Code = 0;

            }
            catch (Exception e)
            {
                result.Msg = e.Message;
                result.Code = -1;
            }
            finally
            {
                if (request != null)
                {
                    request.Abort();
                }
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                }
                if (response != null)
                {
                    response.Close();
                }
            }
            return result;
        }

        static private string MakeQueryString(Dictionary<string, string> param)
        {
            if (param.Count == 0)
            {
                return "";
            }
            string query_string = "";
            foreach (string key in param.Keys)
            {
                query_string = query_string + Util.UrlEncode(key, Encoding.UTF8) + "=" + Util.UrlEncode(param[key], Encoding.UTF8) + "&";
            }
            query_string = query_string.Substring(0, query_string.Length - 1);

            return query_string;
        }
        
        static private string MakeCookieString(Dictionary<string, string> cookie)
        {
            if (cookie == null) return string.Empty;
            if (cookie.Count == 0)
            {
                return null;
            }
            string[] arr_cookies = new string[cookie.Count];
            int i = 0;
            foreach (string key in cookie.Keys)
            {
                arr_cookies[i] = key + "=" + cookie[key];
                i++;
            }
            return string.Join("; ", arr_cookies);
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受   
        } 
    }


    
    /// <summary>
    /// 结果类
    /// </summary>
    /// <remarks>
    /// code = -1  http请求异常
    /// </remarks>
    public class ResponseResult
    {
        public int Code = 0;
        public string Msg = "";
    }
}
