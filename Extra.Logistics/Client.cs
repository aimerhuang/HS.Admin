using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Security.Cryptography;

namespace Extra.Logistics
{
    /// <summary>
    /// Api 调用
    /// </summary>
    public class Client
    {
        public static string Post(string url, IDictionary<string, string> postData)
        {
            string pStr = "{";
            string TimeStamp = GetTimeStamp();
            pStr += "\"appid\":\"" + ApiConfig.AppId + "\",\"timestamp\":\"" + TimeStamp + "\",\"token\":\"" + GetToken(TimeStamp) + "\"";
            foreach (var item in postData)
            {
                if (item.Value.IndexOf('}') > 0 || item.Value.IndexOf(']') > 0)
                    pStr += ",\"" + item.Key + "\":" + item.Value;
                else
                    pStr += ",\"" + item.Key + "\":\"" + item.Value + "\"";
            }
            pStr += "}";
            var strResult = GetResponse(url, pStr.ToString());
            return strResult;
        }

        static string GetResponse(string url, string param)
        {
            byte[] postData = Encoding.UTF8.GetBytes(param);

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/json;charset=UTF-8";
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

        static string GetToken(string timestamp)
        {
            return Encrypt_MD5(string.Format("{0}{1}", ApiConfig.AppKey, timestamp));
        }

        static string GetTimeStamp()
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            DateTime dtNow = DateTime.Parse(DateTime.Now.ToString());
            TimeSpan toNow = dtNow.Subtract(dtStart);
            string timeStamp = toNow.Ticks.ToString();
            timeStamp = timeStamp.Substring(0, timeStamp.Length - 7);
            return timeStamp;
        }

        static string Encrypt_MD5(string AppKey)
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