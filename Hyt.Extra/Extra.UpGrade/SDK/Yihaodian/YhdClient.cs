using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Web;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Extra.UpGrade.SDK.Yihaodian.Request;
using Extra.UpGrade.SDK.Yihaodian.Util;
using Extra.UpGrade.SDK.Yihaodian.Parser;
using Extra.UpGrade.SDK.Yihaodian.Response;
using System.IO.Compression;
using Extra.UpGrade.SDK.Yihaodian.Util.Version;

namespace Extra.UpGrade.SDK.Yihaodian
{
    /// <summary>
    /// 基于REST的YHD客户端。
    /// </summary>
    public class YhdClient
    {
        const string FILE_PART_HEADER = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n" +
                                         "Content-Type: application/octet-stream\r\n\r\n";

        public const string APP_KEY = "appKey";
        public const string FORMAT = "format";
        public const string METHOD = "method";
        public const string TIMESTAMP = "timestamp";
        public const string VERSION = "ver";
        public const string SIGN = "sign";
        public const string SESSION_KEY = "sessionKey";
        public const string FORMAT_JSON = "json";
        public const string SDK_TYPE = "sdkType";

        private string serverUrl;
        private string appKey;
        private string appSecret;
        private string format = FORMAT_JSON;
        private bool useHttpsUrl = true;
        private int timeOut=100000;
        //private int readTimeout;
        #region YhdClient Constructors

        public YhdClient(string serverUrl, string appKey, string appSecret)
        {
            this.appKey = appKey;
            this.appSecret = appSecret;
            this.serverUrl = serverUrl;
        }

        public YhdClient(string serverUrl, string appKey, string appSecret, string format)
            : this(serverUrl, appKey, appSecret)
        {
            this.format = format;
        }

        #endregion

        public string Execute<T>(IYhdRequest<T> request, string sessionKey) where T : YhdResponse
        {
            return Execute(request, sessionKey, null);
        }

        public string Execute<T>(IYhdRequest<T> request, string sessionKey, string[] filePathArray) where T : YhdResponse
        {
            // 添加协议级请求参数
            YhdDictionary txtParams = new YhdDictionary(request.GetParameters());
            txtParams.Add(METHOD, request.GetApiName());
            txtParams.Add(VERSION, "1.0");
            txtParams.Add(APP_KEY, appKey);
            txtParams.Add(FORMAT, format);
            txtParams.Add(TIMESTAMP, DateTime.Now);
            txtParams.Add(SESSION_KEY, sessionKey);
            txtParams.Add(SDK_TYPE, "C#-" + YHDJarVersion.YHDNETVERSION);

            // 添加签名参数
            txtParams.Add(SIGN, YhdUtil.getSignature(txtParams, appSecret));
            string url = this.serverUrl;
            if ((txtParams[METHOD].Contains("yhd.order") || txtParams[METHOD].Contains("yhd.invoices.get")) && useHttpsUrl)
            {
                url = "https://openapi.yhd.com/app/api/rest/router";
            }

            return sendByPost(url, txtParams, filePathArray);
        }

        /**
         * 支持参数和附件的post请求
         * 
        **/
        public string sendByPost(string url, IDictionary<string, string> formDataDict, string[] filePathArray)
        {
            Encoding encode = System.Text.Encoding.GetEncoding("utf-8");

            // 边界符  
            var boundary = "---------------" + DateTime.Now.Ticks.ToString("x");
            // 最后的结束符  
            var endBoundary = Encoding.ASCII.GetBytes("--" + boundary + "--\r\n");

            HttpWebRequest webRequest = null;

            if (url.Contains("https"))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                webRequest = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
            }
            else
            {
                webRequest = (HttpWebRequest)WebRequest.Create(url);
            }
            webRequest.Method = "POST";
            webRequest.ContentType = "multipart/form-data; boundary=" + boundary;
            webRequest.Headers["Accept-Encoding"] = "gzip,deflate";
            webRequest.Timeout = this.timeOut;

            var memStream = new MemoryStream();

            //添加文件信息
            addFileInfo(memStream, filePathArray, boundary);

            //添加form参数信息
            addFormData(memStream, formDataDict, boundary);

            //写入最后的结束边界符  
            memStream.Write(endBoundary, 0, endBoundary.Length);

            webRequest.ContentLength = memStream.Length;

            var requestStream = webRequest.GetRequestStream();

            //将post的信息设置到请求流里面
            memStream.Position = 0;
            var tempBuffer = new byte[memStream.Length];
            memStream.Read(tempBuffer, 0, tempBuffer.Length);
            memStream.Close();

            requestStream.Write(tempBuffer, 0, tempBuffer.Length);
            requestStream.Close();
            StreamReader readStream = null;
            //处理响应信息
            HttpWebResponse myResp = (HttpWebResponse)webRequest.GetResponse();
            string responseData = null;
            using (var ReceiveStream = myResp.GetResponseStream())
            {
                if (myResp.ContentEncoding.ToLower().Contains("gzip"))
                {

                    using (var gzip = new GZipStream(ReceiveStream, CompressionMode.Decompress))
                    {
                        readStream = new StreamReader(gzip, encode);
                        Char[] read = new Char[256];
                        int count = readStream.Read(read, 0, 256);
                        while (count > 0)
                        {
                            responseData += new String(read, 0, count);
                            count = readStream.Read(read, 0, 256);
                        }

                        readStream.Close();
                        // Console.WriteLine("The encoding method used is: " + myResp.ContentEncoding);
                        // Console.ReadKey();
                    }
                }
                else
                {
                    readStream = new StreamReader(ReceiveStream, encode);
                    Char[] read = new Char[256];
                    int count = readStream.Read(read, 0, 256);

                    while (count > 0)
                    {
                        responseData += new String(read, 0, count);
                        count = readStream.Read(read, 0, 256);
                    }

                    readStream.Close();
                    //  Console.WriteLine("The encoding method used is: normal " );
                    // Console.ReadKey();
                }

                ReceiveStream.Close();
                myResp.Close();
                //  Console.WriteLine(responseData);
                // Console.ReadKey();
                //返回响应信息
                return responseData;
            }

        }

        public bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        { //直接确认，否则打不开
            return true;
        }

        /**
         * 把文件添加到post请求流里面
         * 
        **/
        private static void addFileInfo(MemoryStream memStream, string[] filePathArray, string boundary)
        {
            //没有文件的情况下，直接返回
            if (filePathArray == null || filePathArray.Length == 0)
            {
                return;
            }

            // 边界符  
            var beginBoundary = Encoding.ASCII.GetBytes("--" + boundary + "\r\n");

            //循环处理文件
            for (int i = 0; i < filePathArray.Length; i++)
            {
                FileStream fileStream = new FileStream(filePathArray[i], FileMode.Open, FileAccess.Read);

                FileInfo fileInfo = new FileInfo(filePathArray[i]);
                //设置文件名路径和文件名称  
                string encodingFilePath = HttpUtility.UrlEncode(fileInfo.FullName, Encoding.UTF8).Replace("+", "%20");
                string encodingFileName = HttpUtility.UrlEncode(fileInfo.Name, Encoding.UTF8).Replace("+", "%20");
                var header = string.Format(FILE_PART_HEADER, encodingFilePath, encodingFileName);
                var headerbytes = Encoding.UTF8.GetBytes(header);

                //设置每个文件的分界信息
                memStream.Write(beginBoundary, 0, beginBoundary.Length);
                //设置文件的头信息（包括文件名、文件路径）
                memStream.Write(headerbytes, 0, headerbytes.Length);

                //设置文件信息到post请求流里面
                var buffer = new byte[1024];
                int bytesRead;
                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    memStream.Write(buffer, 0, bytesRead);
                }
            }

        }

        /**
         * 把参数添加到post请求流里面
         * 
        **/
        private static void addFormData(MemoryStream memStream, IDictionary<string, string> formDataDict, string boundary)
        {
            //没有form参数的情况下，直接返回
            if (formDataDict == null || formDataDict.Keys.Count == 0)
            {
                return;
            }

            // 写入字符串的Key  
            var stringKeyHeader = "\r\n--" + boundary +
                                   "\r\nContent-Disposition: form-data; name=\"{0}\"" +
                                   "\r\n\r\n{1}";
                                   
 			//将需要提交的form的参数信息，设置到post请求流里面
            string formitem = string.Empty;
            byte[] formitembytes = null;
            foreach (string key in formDataDict.Keys)
            {
                formitem = string.Format(stringKeyHeader, key, formDataDict[key]);
                formitembytes = Encoding.UTF8.GetBytes(formitem);
                memStream.Write(formitembytes, 0, formitembytes.Length);
            }
        }

        public void SetUseHttpsUrl(bool useHttpsUrl)
        {
            this.useHttpsUrl = useHttpsUrl;
        }

        public void SetTimeOut(int timeout)
        {
            this.timeOut = timeout;
        }

    }

}
