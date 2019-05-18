using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Extra.UpGrade.Api
{
    public sealed class WebUtils
    {
        public static Regex REG_URL_ENCODING = new Regex("%[a-f0-9]{2}");
        private int _timeout = 100000;
        public int Timeout
        {
            get
            {
                return this._timeout;
            }
            set
            {
                this._timeout = value;
            }
        }
        public string DoPost(string url, IDictionary<string, string> parameters)
        {
            HttpWebRequest webRequest = this.GetWebRequest(url, "POST");
            webRequest.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            byte[] bytes = Encoding.UTF8.GetBytes(this.BuildQuery(parameters));
            Stream requestStream = webRequest.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();
            HttpWebResponse httpWebResponse = (HttpWebResponse)webRequest.GetResponse();
            Encoding encoding = Encoding.GetEncoding(httpWebResponse.CharacterSet);
            return this.GetResponseAsString(httpWebResponse, encoding);
        }
        public string DoGet(string url, IDictionary<string, string> parameters)
        {
            if (parameters != null && parameters.Count > 0)
            {
                if (url.Contains("?"))
                {
                    url = url + "&" + this.BuildQuery(parameters);
                }
                else
                {
                    url = url + "?" + this.BuildQuery(parameters);
                }
            }
            HttpWebRequest webRequest = this.GetWebRequest(url, "GET");
            webRequest.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            HttpWebResponse httpWebResponse = (HttpWebResponse)webRequest.GetResponse();
            Encoding encoding = Encoding.GetEncoding(httpWebResponse.CharacterSet);
            return this.GetResponseAsString(httpWebResponse, encoding);
        }
        public string DoPost(string url, IDictionary<string, string> textParams, IDictionary<string, FileItem> fileParams)
        {
            string result;
            if (fileParams == null || fileParams.Count == 0)
            {
                result = this.DoPost(url, textParams);
            }
            else
            {
                string str = DateTime.Now.Ticks.ToString("X");
                HttpWebRequest webRequest = this.GetWebRequest(url, "POST");
                webRequest.ContentType = "multipart/form-data;charset=utf-8;boundary=" + str;
                Stream requestStream = webRequest.GetRequestStream();
                byte[] bytes = Encoding.UTF8.GetBytes("\r\n--" + str + "\r\n");
                byte[] bytes2 = Encoding.UTF8.GetBytes("\r\n--" + str + "--\r\n");
                string text = "Content-Disposition:form-data;name=\"{0}\"\r\nContent-Type:text/plain\r\n\r\n{1}";
                IEnumerator<KeyValuePair<string, string>> enumerator = textParams.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    KeyValuePair<string, string> current = enumerator.Current;
                    string key = current.Key;
                    current = enumerator.Current;
                    string value = current.Value;
                    if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                    {
                        string arg_118_0 = text;
                        current = enumerator.Current;
                        object arg_118_1 = current.Key;
                        current = enumerator.Current;
                        string s = string.Format(arg_118_0, arg_118_1, current.Value);
                        byte[] bytes3 = Encoding.UTF8.GetBytes(s);
                        requestStream.Write(bytes, 0, bytes.Length);
                        requestStream.Write(bytes3, 0, bytes3.Length);
                    }
                }
                string format = "Content-Disposition:form-data;name=\"{0}\";filename=\"{1}\"\r\nContent-Type:{2}\r\n\r\n";
                IEnumerator<KeyValuePair<string, FileItem>> enumerator2 = fileParams.GetEnumerator();
                while (enumerator2.MoveNext())
                {
                    KeyValuePair<string, FileItem> current2 = enumerator2.Current;
                    string key2 = current2.Key;
                    current2 = enumerator2.Current;
                    FileItem value2 = current2.Value;
                    string s2 = string.Format(format, key2, value2.GetFileName(), value2.GetMimeType());
                    byte[] bytes3 = Encoding.UTF8.GetBytes(s2);
                    requestStream.Write(bytes, 0, bytes.Length);
                    requestStream.Write(bytes3, 0, bytes3.Length);
                    byte[] content = value2.GetContent();
                    requestStream.Write(content, 0, content.Length);
                }
                requestStream.Write(bytes2, 0, bytes2.Length);
                requestStream.Close();
                HttpWebResponse httpWebResponse = (HttpWebResponse)webRequest.GetResponse();
                Encoding encoding = Encoding.GetEncoding(httpWebResponse.CharacterSet);
                result = this.GetResponseAsString(httpWebResponse, encoding);
            }
            return result;
        }
        public bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }
        public HttpWebRequest GetWebRequest(string url, string method)
        {
            HttpWebRequest httpWebRequest;
            if (url.Contains("https"))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(this.CheckValidationResult);
                httpWebRequest = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
            }
            else
            {
                httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            }
            httpWebRequest.ServicePoint.Expect100Continue = false;
            httpWebRequest.Method = method;
            httpWebRequest.KeepAlive = true;
            httpWebRequest.UserAgent = "sszg";
            httpWebRequest.Timeout = this._timeout;
            return httpWebRequest;
        }
        public string GetResponseAsString(HttpWebResponse rsp, Encoding encoding)
        {
            Stream stream = null;
            StreamReader streamReader = null;
            string result;
            try
            {
                stream = rsp.GetResponseStream();
                streamReader = new StreamReader(stream, encoding);
                result = streamReader.ReadToEnd();
            }
            finally
            {
                if (streamReader != null)
                {
                    streamReader.Close();
                }
                if (stream != null)
                {
                    stream.Close();
                }
                if (rsp != null)
                {
                    rsp.Close();
                }
            }
            return result;
        }
        public  string BuildGetUrl(string url, IDictionary<string, string> parameters)
        {
            if (parameters != null && parameters.Count > 0)
            {
                if (url.Contains("?"))
                {
                    url = url + "&" + this.BuildQuery(parameters);
                }
                else
                {
                    url = url + "?" + this.BuildQuery(parameters);
                }
            }
            return url;
        }
        public string BuildQuery(IDictionary<string, string> parameters)
        {
            StringBuilder stringBuilder = new StringBuilder();
            bool flag = false;
            IEnumerator<KeyValuePair<string, string>> enumerator = parameters.GetEnumerator();
            while (enumerator.MoveNext())
            {
                KeyValuePair<string, string> current = enumerator.Current;
                string key = current.Key;
                current = enumerator.Current;
                string value = current.Value;
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                {
                    if (flag)
                    {
                        stringBuilder.Append("&");
                    }
                    stringBuilder.Append(key);
                    stringBuilder.Append("=");
                    stringBuilder.Append(this.UrlEncode(value, Encoding.UTF8));
                    //stringBuilder.Append(value);
                    flag = true;
                }
            }
            return stringBuilder.ToString();
        }
        public string UrlEncode(string str, Encoding e)
        {
            string result;
            if (str == null)
            {
                result = null;
            }
            else
            {
                string input = HttpUtility.UrlEncode(str, e).Replace("+", "%20").Replace("*", "%2A").Replace("(", "%28").Replace(")", "%29");
                result = WebUtils.REG_URL_ENCODING.Replace(input, (Match m) => m.Value.ToUpperInvariant());
            }
            return result;
        }
    }
}
