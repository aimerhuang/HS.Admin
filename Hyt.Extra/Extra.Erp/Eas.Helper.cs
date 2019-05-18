using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

namespace Extra.Erp
{
    /// <summary>
    /// 对象的Xml序列化与反序列化
    /// </summary>
    /// <remarks>2013-10-22 杨浩 添加</remarks>
    public class Helper
    {
        /// <summary>
        /// 将一个对象序列化成xml字符串
        /// </summary>
        /// <typeparam name="T">序列化的类型</typeparam>
        /// <param name="item">序列化的对象</param>
        /// <returns>返回序列化后的xml形式字符串</returns>
        /// <remarks>2013-10-22 杨浩 添加</remarks>
        public static string XmlSerialize<T>(T item)
        {
            var serializer = new XmlSerializer(typeof (T));
            var stringBuilder = new StringBuilder();
            using (var writer = new StringWriter(stringBuilder))
            {
                serializer.Serialize(writer, item);
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 将一个对象序列化成xml字符串
        /// </summary>
        /// <param name="item">序列化的对象</param>
        /// <returns>返回序列化后的xml形式字符串</returns>
        /// <remarks>2013-10-22 杨浩 添加</remarks>
        public static string XmlSerialize(object item)
        {
            Type type = item.GetType();
            XmlSerializer serializer = new XmlSerializer(type);
            StringBuilder stringBuilder = new StringBuilder();
            using (StringWriter writer = new StringWriter(stringBuilder))
            {
                serializer.Serialize(writer, item);
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 从xml反序列化到适当的类型的对象
        /// </summary>
        /// <typeparam name="T">反序列化的类型对象.</typeparam>
        /// <param name="xmlData">对象的Xml字符串.</param>
        /// <returns>反序列化后的对象</returns>
        /// <remarks>2013-10-22 杨浩 添加</remarks>
        public static T XmlDeserialize<T>(string xmlData)
        {
            XmlSerializer serializer = new XmlSerializer(typeof (T));
            using (TextReader reader = new StringReader(xmlData))
            {
                T entity = (T) serializer.Deserialize(reader);
                return entity;
            }
        }

        /// <summary>  
        /// 给一个字符串进行MD5加密  
        /// </summary>  
        /// <param   name="strText">待加密字符串</param>  
        /// <returns>加密后的字符串</returns>  
        /// <remarks>2013-10-22 杨浩 添加</remarks>
        public static string MD5Encrypt(string strText)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(strText));

            string outString = "";
            for (int i = 0; i < result.Length; i++)
            {
                outString += result[i].ToString("x2");
            }

            return outString;

        }
    }
}
