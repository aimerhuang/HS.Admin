using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Script.Serialization;

namespace Hyt.Util.Serialization
{
    /// <summary>
    /// Json序列化工具类    
    /// </summary>
    /// <remarks>
    /// 2013-02-22 罗雄伟 创建
    /// </remarks>
    public static class JsonUtil
    {
        /// <summary>
        /// 对象序列化为Json字符串
        /// 优先尝试使用效率高的DataContractJsonSerializer类转换,
        /// 失败后使用JavaScriptSerializerToJson类转换
        /// </summary>
        /// <param name="obj">要序列化的对象</param>
        /// <param name="recursionDepth">序列化深度，可不传值</param>
        /// <returns>Json字符串</returns>
        /// <remarks>
        /// 2013-02-22 罗雄伟 创建
        /// 2013-10-08 黄波 修改  增加DataContractJsonSerializer序列化方式
        /// </remarks>
        public static string ToJson(this object obj, int recursionDepth = 100)
        {
            try
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
                using (MemoryStream stream = new MemoryStream())
                {
                    serializer.WriteObject(stream, obj);
                    return Encoding.UTF8.GetString(stream.ToArray());
                }
            }
            catch
            {
                try
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    serializer.RecursionLimit = recursionDepth;
                    return serializer.Serialize(obj);
                }
                catch
                {
                    return string.Empty;
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj">要转换的实体对象</param>
        /// <returns></returns>
        public static string EntityToJson(object obj)
        {
            StringBuilder jsonStr = new StringBuilder();
            PropertyInfo[] pInfos = obj.GetType().GetProperties();
            string pValue = string.Empty;
            jsonStr.Append("{");
            foreach (PropertyInfo p in pInfos)
            {
                if (!(p.GetValue(obj, null) == null))
                {
                    //转义掉Json格式特殊字符 ‘\’,‘"’
                    pValue = p.GetValue(obj, null).ToString().Replace("\\", "\\\\").Replace("\"", "\\\"");
                }
                else
                {
                    pValue = string.Empty;
                }
                jsonStr.Append(string.Format("\"{0}\":\"{1}\",", p.Name, pValue));

            }
            jsonStr.Remove(jsonStr.Length - 1, 1);
            jsonStr.Append("}");
            return jsonStr.ToString();
        }

        /// <summary>
        /// 任意类型转换为Json(上面的方法被修改后影响到我编辑器上传文件了，特意再写个方法)
        /// </summary>
        /// <param name="obj">任意数据类型</param>
        /// <param name="recursionDepth">序列化深度</param>
        /// <returns>json字符串</returns>
        /// <remarks>2013-10-17 杨晗 创建</remarks>
        public static string ToJson2(this object obj, int recursionDepth = 100)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.RecursionLimit = recursionDepth;
            return serializer.Serialize(obj);
        }

        /// <summary>
        /// 把Json字符串反序列化为对象
        /// 优先尝试使用效率高的DataContractJsonSerializer类转换,
        /// 失败后使用JavaScriptSerializerToJson类转换
        /// </summary>        
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">Json字符串</param>
        /// <param name="recursionDepth">反序列化深度，可不传值</param>
        /// <returns>反序列化得到的对象</returns>
        /// <remarks>
        /// 2013-02-22 罗雄伟 创建
        /// 2013-10-08 黄波 修改  增加DataContractJsonSerializer序列化方式
        /// </remarks>
        public static T ToObject<T>(this string obj, int recursionDepth = 100) where T : class
        {
            if (!string.IsNullOrWhiteSpace(obj))
            {
                try
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));

                    using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(obj)))
                    {
                        return (T)serializer.ReadObject(ms);
                    }
                }
                catch
                {
                    try
                    {
                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        return serializer.Deserialize<T>(obj);
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            return null;
        }
    }
}
