using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;
using System.Runtime.Serialization.Json;
using System.Web.Script.Serialization;
namespace Extra.Express.Public
{
    /// <summary>
    /// 接口公用方法
    /// </summary>
    /// <remarks>2017-12-13 廖移凤 创建</remarks>
   public static class SendData
    {
        /// <summary>  
        /// Post方式提交数据 
        /// </summary>  
        /// <param name="url">发送请求的 URL</param>  
        /// <param name="param">请求的参数集合</param>  
        /// <returns>远程资源的响应结果</returns>  
       public static string SendPost(string url, Dictionary<string, string> param)
        {

            string result = "";
            StringBuilder postData = new StringBuilder();
            if (param != null && param.Count > 0)
            {
                foreach (var p in param)
                {
                    if (postData.Length > 0)
                    {
                        postData.Append("&");
                    }
                    postData.Append(p.Key);
                    postData.Append("=");
                    postData.Append(p.Value);
                }
            }
            byte[] byteData = Encoding.GetEncoding("UTF-8").GetBytes(postData.ToString());
            try
            {  //发送请求
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.ContentType = "application/x-www-form-urlencoded";
                //request.Referer = url;
                request.Timeout = 30 * 1000;
                request.Method = "POST";
                Stream stream = request.GetRequestStream();
                stream.Write(byteData, 0, byteData.Length);
                //发送成功后接收返回信息
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream backStream = response.GetResponseStream();
                StreamReader sr = new StreamReader(backStream, Encoding.GetEncoding("UTF-8"));
                result = sr.ReadToEnd();

            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 对象转换为XML
        /// </summary>
        /// <typeparam name="T">类</typeparam>
        /// <param name="obj">对象</param>
        /// <returns>转换后的XML</returns>
        /// <remarks>2017-12-11 廖移凤 创建</remarks>
        public static  string XmlSerialize<T>(T obj)
        {
            MemoryStream ms = new MemoryStream();
            StreamWriter textWriter = new StreamWriter(ms, Encoding.GetEncoding("UTF-8"));//指定编码格式
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            serializer.Serialize(textWriter, obj);
            string xmlMessage = Encoding.UTF8.GetString(ms.GetBuffer());
            ms.Close();
            textWriter.Close();
            return xmlMessage;
        }


        /// <summary>
        /// DesKey
        /// </summary>
        private static  byte[] btKeys = { 61, 4, 104, (byte)(0xff & -119), 38, (byte)(0xff & -68), (byte)(0xff & -88), (byte)(0xff & -45) };
        /// <summary>
        /// Des加密
        /// </summary>
        /// <param name="encryptString">加密内容</param>
        /// <returns></returns>
        public static string DesEncrypt(string encryptString)
        {
            byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
            string des = string.Empty;
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            provider.Mode = CipherMode.ECB;
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, provider.CreateEncryptor(btKeys, btKeys), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            des = Convert.ToBase64String(mStream.ToArray());
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(des));
        }


        /// <summary>
        ///Sign 
        /// <param name="content">内容</param>
        /// <param name="epoch">时间戳</param>
        /// <param name="keyValue">key</param>
        /// <param name="secret">secret</param>
        /// <param name="charset">URL编码</param>
        /// <returns>Sign签名</returns>
        public static string Encrypt(String content, long epoch, String keyValue, string secret, String charset)
        {
            if (keyValue != null)
            {
                return MD5(content + epoch + keyValue + secret, 32);
            }
            return MD5(content, 32);
        }
        ///<summary>  
        /// 字符串MD5加密（大寫） 
        ///</summary>  
        ///<param name="str">要加密的字符串</param>  
        ///<param name="code">加密成多少位</param>  
        ///<returns>密文</returns>
        public static string MD5(string str, int code)
        {
            if (code == 16) //16位MD5加密（取32位加密的9~25字符）   
            {
                return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToUpper().Substring(8, 16);
            }
            else//32位加密   
            {
                return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToUpper();
            }
        }


        /// <summary>
        /// 从一个对象信息生成Json串
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>Json串</returns>
        public static string ObjectToJson(object obj)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            MemoryStream stream = new MemoryStream();
            serializer.WriteObject(stream, obj);
            byte[] dataBytes = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(dataBytes, 0, (int)stream.Length);
            return Encoding.UTF8.GetString(dataBytes);
        }

        /// <summary>
        /// 从一个Json串生成对象信息
        /// </summary>
        /// <param name="jsonString">Json串</param>
        /// <param name="obj">对象</param>
        /// <returns>对象</returns>
        public static object JsonToObject(string jsonString, object obj)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            MemoryStream mStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            return serializer.ReadObject(mStream);
        }

        /// <summary>
        /// XML反序列化
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="xml">XML字符串</param>
        /// <returns></returns>
        public static T Deserialize<T>(string xml)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            StringReader sr = new StringReader(xml);
            T obj = (T)xs.Deserialize(sr);
            sr.Close();
            sr.Dispose();
            return obj;
        }
    }
}
