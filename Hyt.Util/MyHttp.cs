using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.IO.Compression;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Linq;

namespace Hyt.Util
{
    public class MyHttp
    {
        public MyHttp()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        private static CookieContainer MyCookieContainer = new CookieContainer();

        public static void FreeCookie()
        {
            MyCookieContainer = null;
            MyCookieContainer = new CookieContainer();
        }
        public static string GetResponse(string url, string charset)
        {
            return GetResponse(url, "", charset);
        }
        public static string GetResponse(string url)
        {
            return GetResponse(url, "", "utf-8");
        }

        public static string PostJsonData(string url, string param, ref string exception)
        {
            exception = "";
            try
            {
                string strURL = url;
                System.Net.HttpWebRequest request;
                request = (System.Net.HttpWebRequest)WebRequest.Create(strURL);
                request.Method = "POST";
                request.Timeout = 600 * 1000;
                request.ContentType = "application/json;charset=UTF-8";
                string paraUrlCoded = param;
                byte[] payload;
                payload = System.Text.Encoding.UTF8.GetBytes(paraUrlCoded);
                request.ContentLength = payload.Length;
                Stream writer = request.GetRequestStream();
                writer.Write(payload, 0, payload.Length);
                writer.Close();
                System.Net.HttpWebResponse response;
                response = (System.Net.HttpWebResponse)request.GetResponse();
                System.IO.Stream s;
                s = response.GetResponseStream();
                string StrDate = "";
                string strValue = "";
                StreamReader Reader = new StreamReader(s, Encoding.UTF8);
                while ((StrDate = Reader.ReadLine()) != null)
                {
                    strValue += StrDate + "\r\n";
                }
                return strValue;
            }
            catch (Exception ex)
            {
                exception = ex.Message;
                return "";
            }

        }
        public static string PostJsonData(string url, string param)
        {
           string exception="";
           return PostJsonData(url, param, ref exception);        
        }

        public static string GetResponse(string url, string postData, string charset)
        {
            url.Trim();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = 300000;
            request.KeepAlive = true;
            request.AllowAutoRedirect = true;
            request.Accept = "*/*";
            //request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; EmbeddedWB 14.52 from: http://www.bsalsa.com/ EmbeddedWB 14.52; .NET CLR 2.0.50727; CIBA)";
            //request.UserAgent= @"Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; MyIE2; Alexa Toolbar; mxie; .NET CLR 1.1.4322)";
            request.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 8_1_1 like Mac OS X) AppleWebKit/600.1.4 (KHTML, like Gecko) Mobile/12B435 MicroMessenger/6.1 NetType/WIFI";
            request.Headers.Add("Accept-Language", "zh-cn,zh;q=0.5");
            //request.Headers.Add("Accept-Encoding", "gzip, deflate");

            request.CookieContainer = MyCookieContainer;

            try
            {
                request.Referer = request.RequestUri.ToString();
            }
            catch
            {
            }

            request.ContentType = "application/x-www-form-urlencoded";

            HttpWebResponse webresponse = null;
            try
            {
                if (postData != "")
                {
                    request.Method = "POST";
                    byte[] loginDataBytes = Encoding.UTF8.GetBytes(postData);
                    request.ContentLength = loginDataBytes.Length;
                    Stream stream = request.GetRequestStream();
                    stream.Write(loginDataBytes, 0, loginDataBytes.Length);
                    stream.Close();
                }
                else
                {
                    request.Method = "GET";
                }

                webresponse = request.GetResponse() as HttpWebResponse;
                if (webresponse != null)
                {
                    return GetResponseHTML(webresponse, charset);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (webresponse != null)
                {
                    webresponse.Close();
                    webresponse = null;
                }

                if (request != null)
                {
                    request.Abort();
                    request = null;
                }
            }

            return "";
        }
        public static string GetResponseNOTCP(string url, string postData, string charset)
        {
            url.Trim();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = 300000;
            request.KeepAlive = true;
            request.AllowAutoRedirect = true;
            request.Accept = "*/*";
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; EmbeddedWB 14.52 from: http://www.bsalsa.com/ EmbeddedWB 14.52; .NET CLR 2.0.50727; CIBA)";
            //request.UserAgent= @"Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; MyIE2; Alexa Toolbar; mxie; .NET CLR 1.1.4322)";
            request.Headers.Add("Accept-Language", "zh-cn,zh;q=0.5");
            //request.Headers.Add("Accept-Encoding", "gzip, deflate");

            request.CookieContainer = MyCookieContainer;

            try
            {
                request.Referer = request.RequestUri.ToString();
            }
            catch
            {
            }

            request.ContentType = "application/x-www-form-urlencoded";

            HttpWebResponse webresponse = null;
            try
            {
                if (postData == "post")
                {
                    request.Method = "POST";
                    byte[] loginDataBytes = Encoding.ASCII.GetBytes(postData);
                    request.ContentLength = loginDataBytes.Length;
                    Stream stream = request.GetRequestStream();
                    stream.Write(loginDataBytes, 0, loginDataBytes.Length);
                    stream.Close();
                }
                else
                {
                    request.Method = "GET";
                }

                webresponse = request.GetResponse() as HttpWebResponse;
                if (webresponse != null)
                {
                    return GetResponseHTML(webresponse, charset);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (webresponse != null)
                {
                    webresponse.Close();
                    webresponse = null;
                }

                if (request != null)
                {
                    request.Abort();
                    request = null;
                }
            }

            return "";
        }
        private static string GetResponseHTML(HttpWebResponse response, string charset)
        {
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(charset));
            string strHTML = reader.ReadToEnd();

            if (reader != null)
                reader.Close();

            if (response != null)
                response.Close();

            return strHTML;
        }


    
        public static string GetHTMLTCP(string url)
        {
            string strHTML = GetTCP(url);
            Uri URI = new Uri(url);
            while (strHTML.StartsWith("HTTP/1.0 302"))
            {
                int start = strHTML.ToUpper().IndexOf("LOCATION");
                if (start > 0)
                {
                    string temp = strHTML.Substring(start, strHTML.Length - start);
                    string[] sArry = Regex.Split(temp, "\r\n");
                    string newUrl = sArry[0].Remove(0, 10);
                    if (!newUrl.StartsWith(URI.Host))
                        newUrl = "http://" + URI.Host + newUrl;
                    strHTML = GetTCP(newUrl);
                }
            }
            return strHTML;
        }
        private static string GetTCP(string URL)
        {
            TcpClient clientSocket = new TcpClient();
            try
            {
                if (clientSocket.Connected)
                    clientSocket.Close();
                string strHTML = "";//用来保存获得的HTML代码
                Uri URI = new Uri(URL);
                clientSocket.Connect(URI.Host, URI.Port);
                StringBuilder RequestHeaders = new StringBuilder();//用来保存HTML协议头部信息
                RequestHeaders.AppendFormat("{0} {1} HTTP/1.1\r\n", "GET", URI.PathAndQuery);
                RequestHeaders.AppendFormat("Connection:close\r\n");
                RequestHeaders.AppendFormat("Host:{0}\r\n", URI.Host);
                RequestHeaders.AppendFormat("Accept:*/*\r\n");
                RequestHeaders.AppendFormat("Accept-Language:zh-cn\r\n");
                RequestHeaders.AppendFormat("User-Agent:Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)\r\n\r\n");
                Encoding encoding = Encoding.Default;
                byte[] request = encoding.GetBytes(RequestHeaders.ToString());
                clientSocket.Client.Send(request);
                //获取要保存的网络流
                Stream readStream = clientSocket.GetStream();
                StreamReader sr = new StreamReader(readStream, Encoding.Default);
                strHTML = sr.ReadToEnd();

                readStream.Close();
                clientSocket.Close();
                return strHTML;
            }
            catch (Exception)
            {
                clientSocket.Close();
                throw;
            }
        }
        /// <summary>
        /// 获取html源码
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string GetHTML(string url, string param)
        {
            try
            {
                Uri uri = new Uri(url);
                HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(uri);
                myReq.Headers.Add("Accept-Encoding", "gzip,deflate");//sdch
                byte[] byData = Encoding.Default.GetBytes(param);
                myReq.Method = "post";
                myReq.ContentLength = byData.Length;
                Stream reqStrem = myReq.GetRequestStream();
                reqStrem.Write(byData, 0, byData.Length);
                reqStrem.Close();
                HttpWebResponse result = (HttpWebResponse)myReq.GetResponse();
                Stream recStream = result.GetResponseStream();

                //如果是Gzip方式则需要解压
                recStream = new GZipStream(recStream, CompressionMode.Decompress);

                StreamReader redStream = new StreamReader(recStream, System.Text.Encoding.Default);
                string strHTML = redStream.ReadToEnd();
                redStream.Close();
                recStream.Close();
                result.Close();
                return strHTML;
            }
            catch (Exception)
            {
                return "";
            }
        }

      

    }

}
