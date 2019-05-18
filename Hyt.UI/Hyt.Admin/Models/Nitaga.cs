using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;

namespace Hyt.Admin.Models
{
    /// <summary>
    /// 利嘉(你他购)API接口
    /// </summary>
    /// <remarks>2016-10-18 罗远康 创建</remarks>
    public class Nitaga
    {

        public Nitaga(string pUrl)
        {
            url = pUrl;
        }
        public string url { get; set; }
        /// <summary>
        /// Post
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string Post(IDictionary<string, string> postDate)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string strProduct = jss.Serialize(postDate);
            BLL.Log.LocalLogBo.Instance.Write(strProduct+"\r\n", "NitagaPostLog");
            var strResult = PostGetResponse(url, strProduct);
            var json = JObject.Parse(strResult);//{"state": "error","message": "用户名或密码不能为空"}
            //var json1 = JArray.Parse(strResult);
            return strResult;
        }

        /// <summary>
        /// 利嘉POST
        /// </summary>
        /// <param name="url">POST链接</param>
        /// <param name="param">JSON数据</param>
        /// <returns>JSON数据</returns>
        private string PostGetResponse(string url, string param)
        {
            byte[] postData = Encoding.UTF8.GetBytes(param);
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            req.Method = "POST";
            req.KeepAlive = true;
            req.Timeout = 300000;
            req.ContentType = "application/json;charset=UTF-8";
            req.ContentLength = postData.Length;
            req.Headers.Add("api-version", "2.0");
            req.Headers.Add("token", "nitago_xinying_token");
            req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.111 Safari/537.36";

            Stream reqStream = req.GetRequestStream();

            reqStream.Write(postData, 0, postData.Length);

            reqStream.Close();

            HttpWebResponse rsp = null;
            try
            {
                rsp = (HttpWebResponse)req.GetResponse();
            }
            catch (WebException ex)
            {
                rsp = (HttpWebResponse)ex.Response;
            }

            Stream stream = rsp.GetResponseStream();

            StreamReader sr = new StreamReader(stream, Encoding.GetEncoding("utf-8"));
            string result = sr.ReadToEnd();
            sr.Close();
            stream.Close();
            //System.Web.HttpUtility.HtmlDecode(result);
            return result;
        }
    }
}
