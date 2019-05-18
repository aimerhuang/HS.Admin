using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Hyt.Util
{
    /// <summary>
    /// Provides methods for JSON serialization.
    /// 2017-05-19 罗勤尧创建
    /// </summary>
    public static class JsonSerializationHelper
    {
        /// <summary>
        /// Deserializers a JSON string into an strong-typed object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static T JsonToObject<T>(string jsonStr) where T : class
        {
            T result = null;
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));

            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonStr)))
            {
                result = ser.ReadObject(ms) as T;
            }

            return result;
        }

        public static void ObjectToJson(object target, string targetFileLocation)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(target.GetType());

            using (var fs = File.OpenWrite(targetFileLocation))
            {
                ser.WriteObject(fs, target);
            }
        }

        public static string ObjectToJson(object target)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(target.GetType());
            string result = string.Empty;

            using (var ms = new MemoryStream())
            {
                ser.WriteObject(ms, target);

                ms.Position = 0;
                StreamReader sReader = new StreamReader(ms);
                result = sReader.ReadToEnd();
            }

            return result;
        }
    }
}
