using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Extra.UpGrade.SDK.Yihaodian.Response;

namespace Extra.UpGrade.SDK.Yihaodian.Parser
{
    /// <summary>
    /// YHD XML响应通用解释器。
    /// </summary>
    public class YhdParser : IYhdParser
    {
        private static Regex regex = new Regex("<(\\w+?)[ >]", RegexOptions.Compiled);
        private static Dictionary<string, XmlSerializer> parsers = new Dictionary<string, XmlSerializer>();

        #region IYhdParser Members

        public T ParseToXml<T>(string responseResult) where T : YhdResponse
        {
            Type type = typeof(T);
            string rootTagName = GetRootElement(responseResult);

            string key = type.FullName;
           
            XmlSerializer serializer = null;
            bool incl = parsers.TryGetValue(key, out serializer);
            if (!incl || serializer == null)
            {
                XmlAttributes rootAttrs = new XmlAttributes();
                rootAttrs.XmlRoot = new XmlRootAttribute(rootTagName);

                XmlAttributeOverrides attrOvrs = new XmlAttributeOverrides();
                attrOvrs.Add(type, rootAttrs);

                serializer = new XmlSerializer(type, attrOvrs);
                parsers[key] = serializer;
            }

            object obj = null;
            using (System.IO.Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(responseResult)))
            {
                obj = serializer.Deserialize(stream);
            }

            T rsp = (T)obj;
            return rsp;
        }

        public T ParseToJson<T>(string responseResult) where T : YhdResponse 
        {
            // 忽略根节点的名称
            string str = responseResult.Substring(1, responseResult.Length - 2);
            string result = str.Substring(str.IndexOf("{"));

            T rsp = (T)JsonConvert.DeserializeObject(result, typeof(T));
            return rsp;
        }
        #endregion

        /// <summary>
        /// 获取XML响应的根节点名称
        /// </summary>
        private string GetRootElement(string body)
        {
            Match match = regex.Match(body);
            if (match.Success)
            {
                return match.Groups[1].ToString();
            }
            else
            {
                throw new Exception("Invalid XML response format!");
            }
        }
    }
}
