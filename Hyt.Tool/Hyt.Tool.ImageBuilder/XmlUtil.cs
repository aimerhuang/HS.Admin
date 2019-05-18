
using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Hyt.Tool.ImageBuilder.Serialization
{
    /// <summary>
    /// 对象的Xml序列化与反序列化
    /// </summary>
    /// <remarks>杨浩 添加</remarks>
    public class SerializationUtil
    {
        /// <summary>
        /// 将一个对象序列化成xml字符串
        /// </summary>
        /// <typeparam name="T">序列化的类型</typeparam>
        /// <param name="item">序列化的对象</param>
        /// <returns>返回序列化后的xml形式字符串</returns>
        public static string XmlSerialize<T>(T item)
        {
            var serializer = new XmlSerializer(typeof(T));
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
        public static T XmlDeserialize<T>(string xmlData)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (TextReader reader = new StringReader(xmlData))
            {
                T entity = (T)serializer.Deserialize(reader);
                return entity;
            }
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="filename">文件路径</param>
        /// <returns></returns>
        public static T XmlDeserialize<T>(string filename,Type type=null)
        {
            type = type ?? typeof (T);
            FileStream fs = null;
            try
            {
                fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                XmlSerializer serializer = new XmlSerializer(type);
                return (T)serializer.Deserialize(fs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }

        /// <summary>
        /// 保存序列化对象
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="filename">文件路径</param>
        public static bool Save(object obj, string filename)
        {
            bool success = false;
            FileStream fs = null;
            try
            {
                fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(fs, obj);
                success = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
            return success;
        }
    } 
}
