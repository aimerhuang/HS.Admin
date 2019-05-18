using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;

namespace Hyt.Util
{
    /// <summary>
    /// Web工具
    /// </summary>
    /// <remarks>2013-03-13 杨浩 创建</remarks>
    public class WebUtil
    {

        #region Utilities
        /// <summary>
        /// Maps a virtual path to a physical disk path.
        /// </summary>
        /// <param name="path">The path to map. E.g. "~/bin"</param>
        /// <returns>The physical path. E.g. "c:\inetpub\wwwroot\bin"</returns>
        public static string MapPath(string path)
        {
            if (HostingEnvironment.IsHosted)
            {
                //hosted
                return HostingEnvironment.MapPath(path);
            }

            //not hosted. For example, run in unit tests
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            path = path.Replace("~/", "").TrimStart('/').Replace('/', '\\');
            return Path.Combine(baseDirectory, path);
        }
        public static bool TryWriteWebConfig(string configPaths)
        {
            try
            {
                foreach (var path in configPaths.Split(','))
                {
                    if (!string.IsNullOrWhiteSpace(path))
                    {
                        // In medium trust, "UnloadAppDomain" is not supported. Touch web.config
                        // to force an AppDomain restart.
                        File.SetLastWriteTimeUtc(path, DateTime.UtcNow);
                    }               
                }
              
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool TryWriteWebConfig()
        {
            try
            {
                TryWriteWebConfig(MapPath("~/web.config"));                         
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool TryWriteGlobalAsax()
        {
            try
            {
                //When a new plugin is dropped in the Plugins folder and is installed into nopCommerce, 
                //even if the plugin has registered routes for its controllers, 
                //these routes will not be working as the MVC framework couldn't 
                //find the new controller types and couldn't instantiate the requested controller. 
                //That's why you get these nasty errors 
                //i.e "Controller does not implement IController".
                //The issue is described here: http://www.nopcommerce.com/boards/t/10969/nop-20-plugin.aspx?p=4#51318
                //The solution is to touch global.asax file
                File.SetLastWriteTimeUtc(MapPath("~/global.asax"), DateTime.UtcNow);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion
        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受  
        } 
  
        public static string PostString(string url, string param, string contentType = "application/json;charset=UTF-8", string accept = "*/*")
        {
            byte[] postData = Encoding.UTF8.GetBytes(param);

            HttpWebRequest req = null;

            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                req = WebRequest.Create(url) as HttpWebRequest;
                req.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                req = WebRequest.Create(url) as HttpWebRequest;
            }

            req.Method = "POST";
            req.ContentType = contentType;
            req.Accept = accept;
            req.KeepAlive = true;
            req.Timeout = 300000;
            req.ContentLength = postData.Length;



            Stream reqStream = req.GetRequestStream();

            reqStream.Write(postData, 0, postData.Length);

            reqStream.Close();
            HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();

            Stream stream = rsp.GetResponseStream();

            StreamReader sr = new StreamReader(stream, Encoding.UTF8);
            string result = sr.ReadToEnd();
            sr.Close();
            stream.Close();
            return result;
        }
        #region 生成随机密码字母和数字
        /// <summary>
        /// 字母+数字
        /// </summary>
        /// <param name="zmLen">字母长度</param>
        /// <param name="szLen">数字长度</param>
        /// <param name="Sleep">是否要在生成前将当前线程阻止以避免重复</param>
        /// <returns></returns>
        /// <remarks>2016－5-25 杨浩 创建</remarks>
        public static string GeneratePwd(int zmLen, int szLen, bool Sleep = false)
        {
            if (Sleep)
                System.Threading.Thread.Sleep(3);
            char[] zm = new char[] { 'a', 'b', 'b', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'm', 'n', 'p', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
            char[] sz = new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            string result = "";
            System.Random random = new Random(~unchecked((int)DateTime.Now.Ticks));
            //字母随机
            for (int i = 0; i < zmLen; i++)
            {
                int index1 = random.Next(0, zm.Length);
                result += zm[index1];
            }
            //数字随机
            for (int i = 0; i < szLen; i++)
            {
                int index = random.Next(0, sz.Length);
                result += sz[index];
            }
            return result;
        }

        #endregion
        /// <summary>
        /// 获取请求方式
        /// </summary>
        /// <param name="url">接口地址</param>
        /// <param name="method">GET或POST</param>
        /// <returns>请求方式</returns>
        /// <remarks>2016-1-9 杨浩 创建</remarks>
        public static HttpWebRequest GetWebRequest(string url, string method)
        {
            HttpWebRequest req = null;
            if (url.Contains("https"))
            {
                req = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
            }
            else
            {
                req = (HttpWebRequest)WebRequest.Create(url);
            }

            req.ServicePoint.Expect100Continue = false;
            req.Method = method;
            req.KeepAlive = true;

            return req;
        }
        /// <summary>
        /// POST 表单数据
        /// </summary>
        /// <param name="url">接收数据链接</param>
        /// <param name="param">表单数据</param>
        /// <returns></returns>
        public static string PostForm(string url, string param)
        {
            Encoding myEncode = Encoding.GetEncoding("UTF-8");
            byte[] postBytes = Encoding.UTF8.GetBytes(param);

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            req.ContentLength = postBytes.Length;

            try
            {
                using (Stream reqStream = req.GetRequestStream())
                {
                    reqStream.Write(postBytes, 0, postBytes.Length);
                }
                using (WebResponse res = req.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(res.GetResponseStream(), myEncode))
                    {
                        string strResult = sr.ReadToEnd();
                        return strResult;
                    }
                }
            }
            catch (WebException ex)
            {
                return "无法连接到服务器\r\n错误信息：" + ex.Message;
            }
        } 
        /// <summary>
        /// Post json 数据
        /// </summary>
        /// <param name="url">接收数据链接</param>
        /// <param name="param">json参数</param>
        /// <returns></returns>
        /// <remarks>2015-12-29 杨浩 创建</remarks>
        public static string PostJson(string url, string param)
        {
     

            byte[] postData =param==""?new byte[0]:Encoding.UTF8.GetBytes(param);

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/json;charset=utf-8";
            //req.ContentType = "application/vnd.ehking-v1.0+json;charset=UTF-8";
            req.KeepAlive = true;
            req.Timeout = 300000;
            req.ContentLength = postData.Length;
           
            Stream reqStream = req.GetRequestStream();

            reqStream.Write(postData, 0, postData.Length);

            reqStream.Close();
            HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();

            Stream stream = rsp.GetResponseStream();

            StreamReader sr = new StreamReader(stream, Encoding.UTF8);
            string result = sr.ReadToEnd();
            sr.Close();
            stream.Close();
            return result;
        }
        /// <summary>
        /// 获得当前绝对路径
        /// </summary>
        /// <param name="path">指定的路径</param>
        /// <returns>绝对路径</returns>
        /// <remarks>2013-03-13 杨浩 创建</remarks>
        /// <remarks>2013-06-08 罗雄伟 重构</remarks>
        public static string GetMapPath(string path)
        {
            if (path.ToLower().StartsWith("http://"))
            {
                return path;
            }
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(path);
            }
            else //非web程序引用
            {
                path = path.Replace("/", "\\");
                if (path.StartsWith("\\"))
                {
                    //path = path.Substring(path.IndexOf('\\', 1)).TrimStart('\\');
                    path = path.TrimStart('\\');
                }
                return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
            }
        }

        /// <summary>
        /// 检查是否为本地地址正则
        /// </summary>
        private static Regex checkUrlRegex = null;

        /// <summary>
        /// 检查地址是否为本地地址（包含相对路径和绝对路径，例如：xxx://开头的都不是本地地址）
        /// </summary>
        /// <param name="url"></param>
        /// <returns>bool</returns>
        /// <remarks>2013-04-03 杨浩 创建</remarks>
        /// <remarks>2013-06-08 罗雄伟 重构</remarks>
        public static bool IsLocalUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return false;

            if (url.StartsWith("/"))
                return true;

            //先判断前缀
            if (
                url.StartsWith("http://", true, null)
                || url.StartsWith("https://", true, null)
                || url.StartsWith("ftp://", true, null)
                )
            {
                return false;
            }
            if (checkUrlRegex == null)
                checkUrlRegex = new Regex(@"^\w+://.*$", RegexOptions.IgnoreCase);

            return !checkUrlRegex.IsMatch(url);
        }

        /// <summary>
        /// 生成指定长度随机数字
        /// </summary>
        /// <param name="length">生成长度</param>
        /// <param name="sleep">是否要在生成前将当前线程阻止以避免重复</param>
        /// <returns>指定长度的随机数</returns>
        /// <remarks>2013－07-24 苟治国 创建</remarks>
        public static string Number(int length, bool sleep)
        {
            if (sleep)
                System.Threading.Thread.Sleep(3);
            string result = "";
            Random random = new Random();
            for (int i = 0; i < length; i++)
            {
                result += random.Next(10).ToString();
            }
            return result;
        }

        /// <summary>
        /// 隐藏手机号中间4位
        /// </summary>
        /// <param name="phone">手机号码</param>
        /// <returns>格式:135****0534</returns>
        /// <remarks>2013-4-2 杨浩 添加</remarks>
        public static string HideMobilePhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return "";
            string pattern = @"(1[3,5,8]\d)(\d{4})(\d{4})";
            Regex reg = new Regex(pattern);
            return reg.Replace(phone, "$1****$3");
        }

        /// <summary>
        /// 将时间转换为Unix格式的刻度
        /// </summary>
        /// <param name="time"></param>
        /// <returns>刻度</returns>
        /// <remarks>2014-1-21 杨浩 添加注释</remarks>
        public static long ConvertDateTimeUnix(System.DateTime time)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            DateTime dtNow = time;
            TimeSpan t = (dtNow - dtStart);
            return (long)t.TotalSeconds * 1000;
        }

        /// <summary>
        /// 获取当前页面URL参数
        /// </summary>
        /// <returns></returns>
        /// <remarks>2014-1-21 杨浩 添加注释</remarks>
        public static string GetUrl()
        {
            if (System.Web.HttpContext.Current.Request.Url != null)
                return System.Web.HttpContext.Current.Request.Url.PathAndQuery;
            else
                return "";
        }

        #region 获得用户IP

        /// <summary>
        /// 获得用户IP
        /// </summary>
        /// <remarks>2014-1-21 杨浩 添加注释</remarks>
        public static string GetUserIp()
        {
            try
            {
                string ip;
                string[] temp;
                bool isErr = false;
                if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_ForWARDED_For"] == null)
                    ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                else
                    ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_ForWARDED_For"].ToString();
                //if (ip.Length > 15)
                //    isErr = true;
                //else
                //{
                //    temp = ip.Split('.');
                //    if (temp.Length == 4)
                //    {
                //        for (int i = 0; i < temp.Length; i++)
                //        {
                //            if (temp[i].Length > 3) isErr = true;
                //        }
                //    }
                //    else
                //        isErr = true;
                //}

                //if (isErr)
                //    return "1.1.1.1";
                //else
                return ip;
            }
            catch (Exception ex)
            {
                return "0.0.0.0";
            }
        }

        /// <summary>
        /// 根据第三方接口返回用户公网IP地址
        /// </summary>
        /// <returns>用户公网IP地址</returns>
        /// <remarks>2013-12-16 黄波 创建</remarks>
        public static string GetRealIP()
        {
            var content = NetUtil.GetHttpRequest("http://iframe.ip138.com/ic.asp");
            var m = System.Text.RegularExpressions.Regex.Match(content, @"\[(?<IP>[0-9\.]*)\]");
            return m.Value.Trim(new char[] { '[', ']' });
        }

        #endregion

        /// <summary>     
        /// 去除HTML标记     
        /// </summary>     
        /// <param name="strHtml">包括HTML的源码 </param>     
        /// <returns>已经去除后的文字</returns>  
        /// <remarks>2013－11-22 杨晗 创建</remarks> 
        public static string StripHTML(string strHtml)
        {
            string[] aryReg =
                {
                    @"<script[^>]*?>.*?</script>",
                    @"<(\/\s*)?!?((\w+:)?\w+)(\w+(\s*=?\s*(([""'])(\\[""'tbnr]|[^\7])*?\7|\w+)|.{0})|\s)*?(\/\s*)?>",
                    @"([ ])[\s]+",
                    @"&(quot|#34);",
                    @"&(amp|#38);",
                    @"&(lt|#60);",
                    @"&(gt|#62);",
                    @"&(nbsp|#160);",
                    @"&(iexcl|#161);",
                    @"&(cent|#162);",
                    @"&(pound|#163);",
                    @"&(copy|#169);",
                    @"&#(\d+);",
                    @"-->",
                    @"<!--.* "
                };
            string[] aryRep =
                {
                    "",
                    "",
                    "",
                    "\"",
                    "&",
                    "<",
                    ">",
                    " ",
                    "\xa1", //chr(161),    
                    "\xa2", //chr(162),    
                    "\xa3", //chr(163),     
                    "\xa9", //chr(169),        
                    "",
                    " ",
                    ""
                };
            string newReg = aryReg[0];
            string strOutput = strHtml;
            for (int i = 0; i < aryReg.Length; i++)
            {
                Regex regex = new Regex(aryReg[i], RegexOptions.IgnoreCase);
                strOutput = regex.Replace(strOutput, aryRep[i]);
            }
            strOutput.Replace("<", "");
            strOutput.Replace(">", "");
            strOutput.Replace(" ", "");
            return strOutput;
        }

        /// <summary>
        /// 判断当前客户端请求是否为微信浏览器
        /// </summary>
        /// <returns></returns>
        /// <remarks>2015-09-13 杨浩 创建</remarks>
        public static bool IsWeiXin()
        {
            return false;
            //try
            //{
            //    return HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"].IndexOf("MicroMessenger") >= 0;
            //}
            //catch
            //{
            //    return false;
            //}
            
        }

        /// <summary>
        /// 读cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <returns>cookie值</returns>
        public static string GetCookie(string strName)
        {
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[strName] != null)
                return HttpContext.Current.Request.Cookies[strName].Value.ToString();

            return "";
        }

        /// <summary>
        /// 返回 URL 字符串的编码结果
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>编码结果</returns>
        public static string UrlEncode(string str)
        {
            return HttpUtility.UrlEncode(str);
        }

        /// <summary>
        /// http请求post Json字符串
        /// </summary>
        /// <param name="url">请求url</param>
        /// <param name="postData">json数据</param>
        /// <returns></returns>
        public static string GetHttpWebPostJson(string url, string postData)
        {
            Encoding encoding = Encoding.UTF8;
            byte[] data = encoding.GetBytes(postData);
            //此处为为http请求url  
            var uri = new Uri(url);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            //用此方法可以添加标准或非标准http请求，诸如conten-type ,accept,range等  
            request.Headers.Add("X-Auth-Token", HttpUtility.UrlEncode("openstack"));
            //此处为C#实现的一些标准http请求头添加方法，用上面的方面也可以实现  
            request.ContentType = "application/json";
            request.Accept = "application/json";
            //此处添加标准http请求方面  
            request.Method = "POST";
            System.IO.Stream sm = request.GetRequestStream();
            sm.Write(data, 0, data.Length);
            sm.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream streamResponse = response.GetResponseStream();
            StreamReader streamRead = new StreamReader(streamResponse, Encoding.UTF8);
            Char[] readBuff = new Char[256];
            int count = streamRead.Read(readBuff, 0, 256);
            //content为http响应所返回的字符流  
            String content = "";
            while (count > 0)
            {
                String outputData = new String(readBuff, 0, count);
                content += outputData;
                count = streamRead.Read(readBuff, 0, 256);
            }
            response.Close();
            return content;
        }

        /// <summary>
        /// CURL提交表单
        /// </summary>
        /// <param name="list">表单字段集合</param>
        /// <param name="uri">远程url</param>
        /// <returns></returns>
        /// <remarks>2016-4-16 杨浩 创建</remarks>
        public static string PostFormData(List<FormItem> list, string uri)
        {
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            //请求 
            WebRequest req = WebRequest.Create(uri);
            req.Method = "POST";
            req.ContentType = "multipart/form-data; boundary=" + boundary;
            //组织表单数据 
            StringBuilder sb = new StringBuilder();
            foreach (FormItem item in list)
            {
                switch (item.ParamType)
                {
                    case ParamType.Text:
                        sb.Append("--" + boundary);
                        sb.Append("\r\n");
                        sb.Append("Content-Disposition: form-data; name=\"" + item.Name + "\"");
                        sb.Append("\r\n\r\n");
                        sb.Append(item.Value);
                        sb.Append("\r\n");
                        break;
                    case ParamType.File:
                        sb.Append("--" + boundary);
                        sb.Append("\r\n");
                        sb.Append("Content-Disposition: form-data; name=\"media\"; filename=\"" + item.Value + "\"");
                        sb.Append("\r\n");
                        sb.Append("Content-Type: application/octet-stream");
                        sb.Append("\r\n\r\n");
                        break;
                }
            }
            string head = sb.ToString();
            //post字节总长度
            long length = 0;
            byte[] form_data = Encoding.UTF8.GetBytes(head);
            //结尾 
            byte[] foot_data = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");
            List<FormItem> fileList = list.Where(f => f.ParamType == ParamType.File).ToList();
            length = form_data.Length + foot_data.Length;
            foreach (FormItem fi in fileList)
            {
                length += fi.FileStream.Length;            
            }
            req.ContentLength = length;
            using (Stream requestStream = req.GetRequestStream())
            {
                //发送表单参数 
                requestStream.Write(form_data, 0, form_data.Length);
                foreach (FormItem fd in fileList)
                {
                    fd.FileStream.Seek(0, SeekOrigin.Begin);
                    //文件内容 
                    byte[] buffer = new Byte[checked((uint)Math.Min(4096, (int)fd.FileStream.Length))];
                    int bytesRead = 0;
                    while ((bytesRead = fd.FileStream.Read(buffer, 0, buffer.Length)) != 0)
                        requestStream.Write(buffer, 0, bytesRead);
                    //结尾 
                    requestStream.Write(foot_data, 0, foot_data.Length);
                    fd.FileStream.Close();
                }
                //requestStream.Close();
                //响应 
                WebResponse pos = req.GetResponse();
                StreamReader sr = new StreamReader(pos.GetResponseStream(), Encoding.UTF8);
                string html = sr.ReadToEnd().Trim();
                sr.Close();
                if (pos != null)
                {
                    pos.Close();

                    pos = null;
                }
                if (req != null)
                {
                    req = null;
                }
                return html;
            }
        }
        public static string ChineseToUTF8(string txt)
        {
            return System.Web.HttpUtility.UrlEncode(txt, System.Text.Encoding.UTF8);
        }



        #region HTTP通信接口 POST、GET

        public static string Post(string xml, string url, bool isUseCert, int timeout)
        {
            System.GC.Collect();//垃圾回收，回收没有正常关闭的http连接

            string result = "";//返回结果

            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream reqStream = null;

            try
            {
                //设置最大连接数
                ServicePointManager.DefaultConnectionLimit = 200;
                //设置https验证方式
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback =
                            new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
                }

                /***************************************************************
                * 下面设置HttpWebRequest的相关属性
                * ************************************************************/
                request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = "POST";
                request.Timeout = timeout * 1000;

                //设置代理服务器
                //WebProxy proxy = new WebProxy();                          //定义一个网关对象
                //proxy.Address = new Uri(WxPayConfig.PROXY_URL);              //网关服务器端口:端口
                //request.Proxy = proxy;

                //设置POST的数据类型和长度
                request.ContentType = "text/xml";
                byte[] data = System.Text.Encoding.UTF8.GetBytes(xml);
                request.ContentLength = data.Length;

                //是否使用证书
                if (isUseCert)
                {
                    string path = HttpContext.Current.Request.PhysicalApplicationPath;
                    //X509Certificate2 cert = new X509Certificate2(path + WxPayConfig.SSLCERT_PATH, WxPayConfig.SSLCERT_PASSWORD);
                    //request.ClientCertificates.Add(cert);                  
                }

                //往服务器写入数据
                reqStream = request.GetRequestStream();
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();

                //获取服务端返回
                response = (HttpWebResponse)request.GetResponse();

                //获取服务端返回数据
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                result = sr.ReadToEnd().Trim();
                sr.Close();
            }
            catch (System.Threading.ThreadAbortException e)
            {
              
                System.Threading.Thread.ResetAbort();
            }
            catch (WebException e)
            {
               
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                   
                }
               
            }
            catch (Exception e)
            {
               
            }
            finally
            {
                //关闭连接和流
                if (response != null)
                {
                    response.Close();
                }
                if(request != null)
                {
                    request.Abort();
                }
            }
            return result;
        }

        /// <summary>
        /// 处理http GET请求，返回数据
        /// </summary>
        /// <param name="url">请求的url地址</param>
        /// <returns>http GET成功后返回的数据，失败抛WebException异常</returns>
        public static string Get(string url)
        {
            System.GC.Collect();
            string result = "";

            HttpWebRequest request = null;
            HttpWebResponse response = null;

            //请求url以获取数据
            try
            {
                //设置最大连接数
                ServicePointManager.DefaultConnectionLimit = 200;
                //设置https验证方式
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback =
                            new RemoteCertificateValidationCallback(CheckValidationResult);
                }

                /***************************************************************
                * 下面设置HttpWebRequest的相关属性
                * ************************************************************/
                request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = "GET";

                //设置代理
                //WebProxy proxy = new WebProxy();
                //proxy.Address = new Uri(WxPayConfig.PROXY_URL);
                //request.Proxy = proxy;

                //获取服务器返回
                response = (HttpWebResponse)request.GetResponse();

                //获取HTTP返回数据
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                result = sr.ReadToEnd().Trim();
                sr.Close();
            }
            catch (System.Threading.ThreadAbortException e)
            {
              
                System.Threading.Thread.ResetAbort();
            }
            catch (WebException e)
            {
                
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                   
                }
         
            }
            catch (Exception e)
            {
               
            }
            finally
            {
                //关闭连接和流
                if (response != null)
                {
                    response.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
            }
            return result;
        }
        #endregion
    }
}
