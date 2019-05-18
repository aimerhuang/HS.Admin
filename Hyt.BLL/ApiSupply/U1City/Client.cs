using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Security.Cryptography;

namespace Hyt.BLL.ApiSupply.U1City
{
    /// <summary>
    /// Api 调用Client
    /// </summary>
    public class Client
    {
        public Client(string pUrl, string pMethod, string pUser, string pSession, string pFormat)
        {
            url = pUrl;
            method = pMethod;
            user = pUser;
            session = pSession;
            format = pFormat;
        }

        public string url { get; set; }

        public string method { get; set; }

        public string user { get; set; }

        public string session { get; set; }

        public string format { get; set; }

        /// <summary>
        /// Post
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string Post(IDictionary<string, string> postDate)
        {
            StringBuilder pStr = new StringBuilder();

            pStr.Append("user").Append("=").Append(Uri.EscapeDataString(user));
            pStr.Append("&").Append("method").Append("=").Append(Uri.EscapeDataString(method));
            if (!string.IsNullOrEmpty(format))
            {
                pStr.Append("&").Append("format").Append("=").Append(Uri.EscapeDataString(format));
            }

            string str = method + session;
            foreach (var item in postDate)
            {
                str += item.Key;
                str += item.Value;

                pStr.Append("&").Append(item.Key).Append("=").Append(Uri.EscapeDataString(item.Value));
            }
            string strAsc = Asc(str.Replace(" ", "").ToLower());
            string token = Encrypt_MD5(strAsc);

            pStr.Append("&").Append("token").Append("=").Append(Uri.EscapeDataString(token));

            var strResult = GetResponse(url, pStr.ToString(), "POST");
            return strResult;
        }




        string GetResponse(string url, string param, string Method)
        {
            byte[] postData = Encoding.UTF8.GetBytes(param);

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.KeepAlive = true;
            req.Timeout = 300000;
            req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            req.ContentLength = postData.Length;
            req.Headers.Add("api-version", "2.0");

            Stream reqStream = req.GetRequestStream();

            reqStream.Write(postData, 0, postData.Length);

            reqStream.Close();
            HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();

            Stream stream = rsp.GetResponseStream();

            StreamReader sr = new StreamReader(stream, Encoding.GetEncoding("utf-8"));
            string result = sr.ReadToEnd();
            sr.Close();
            stream.Close();

            return result;
        }

        string Asc(string input)
        {
            Array arr = input.ToArray();
            Array.Sort(arr);
            string strAsc = "";
            foreach (var item in arr)
            {
                strAsc += item;
            }
            return strAsc;
        }



        public string Encrypt_MD5(string AppKey)
        {
            MD5 MD5 = new MD5CryptoServiceProvider();
            byte[] datSource = Encoding.GetEncoding("gb2312").GetBytes(AppKey);
            byte[] newSource = MD5.ComputeHash(datSource);
            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < newSource.Length; i++)
            {
                sb.Append(newSource[i].ToString("x").PadLeft(2, '0'));
            }
            string crypt = sb.ToString();
            return crypt;
        }
    }
}
