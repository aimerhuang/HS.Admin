using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;

namespace Hyt.BLL.ApiSupply.HYH
{
    public class WebUtils
    {
        public class HttpPacket
        {
            public string RequestUrl
            {
                get;
                set;
            }

            public NameValueCollection ResponseHeaders
            {
                get;
                set;
            }

            public string ResponseBody
            {
                get;
                set;
            }
        }

        public static HttpPacket DoPost(string url, string data, string contentType = "application/x-www-form-urlencoded", Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            WebClient client = new WebClient();
            client.Headers.Add("Content-Type", contentType);
            byte[] responseData = client.UploadData(url, "POST", encoding.GetBytes(data));
            string responseStr = encoding.GetString(responseData);//解码  

            HttpPacket packet = new HttpPacket
            {
                RequestUrl = url,
                ResponseBody = responseStr,
                ResponseHeaders = client.ResponseHeaders
            };
            return packet;
        }

        public static HttpPacket DoGet(string url, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            WebClient client = new WebClient();
            byte[] responseData = client.DownloadData(url);
            string responseStr = encoding.GetString(responseData);//解码  
            HttpPacket packet = new HttpPacket
            {
                RequestUrl = url,
                ResponseBody = responseStr,
                ResponseHeaders = client.ResponseHeaders
            };
            return packet;
        }
    }
}
