using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace Hyt.Util
{
    /// <summary>
    /// email 模板处理类
    /// </summary>
    /// <remarks>何方 创建 20130304</remarks>
    public class EmailTemplate
    {

       /// <summary>
       /// 获取邮件内容
       /// </summary>
       /// <param name="templatePath">模板路径</param>
        /// <param name="beginTag">替换内容开始标志</param>
        /// <param name="endTag">替换内容结束标志</param>
        /// <param name="values">用来替换的数据</param>
       /// <returns>邮件模板</returns>
        /// <remarks>2014-1-21 黄波 创建</remarks>
        public static string GetContent(string templatePath, string beginTag, string endTag, Dictionary<string, string> values)
        {
            var template = System.IO.File.ReadAllText(templatePath);
            int index = template.IndexOf(beginTag, 0);
            int num2 = template.IndexOf(endTag, 0);
            if (num2 <= index)
            {
                return "";
            }
            template = template.Substring(index + beginTag.Length, num2 - index - beginTag.Length);
            foreach (string key in values.Keys)
            {
                template = template.Replace(key, values[key].ToString());
            }
            return template;
        }

        /// <summary>
        /// 获取邮件内容
        /// </summary>
        /// <param name="templatePath">模板路径</param>
        /// <param name="values">用来替换的数据</param>
        /// <returns>邮件模板</returns>
        /// <remarks>2014-1-21 黄波 创建</remarks>
        public static string GetContent(string templatePath,  Dictionary<string, string> values)
        {
            string beginTag = "_beginHead";
            if (values.ContainsKey("_beginHead")&& !string.IsNullOrWhiteSpace(values["_beginHead"]))
            {
                beginTag = values["_beginHead"];
            }
            string endTag = "_endFoot";

            if (values.ContainsKey("_endFoot") && !string.IsNullOrWhiteSpace(values["_endFoot"]))
            {
                endTag = values["_endFoot"];
            }
            return GetContent(templatePath, beginTag, endTag, values);
        }
    }
}
