using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Hyt.Util
{
    /// <summary>
    /// 格式化工具类
    /// </summary>
    /// <remarks>2013-10-16 黄波 创建</remarks>
    public class FormatUtil
    {
        /// <summary>
        /// 格式化手机号码
        /// </summary>
        /// <param name="phone">手机号码</param>
        /// <returns>XXX****XXXX</returns>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        public static string PhoneFormat(string phone)
        {
            string pattern = @"(1[3,5,8]\d)(\d{4})(\d{4})";
            Regex reg = new Regex(pattern);
            return reg.Replace(phone, "$1****$3");
        }

        /// <summary>
        /// 格式化字串长度(汉字一个算两个)
        /// </summary>
        /// <param name="str">要检测的字串</param>
        /// <param name="len">指定检测长度</param>
        /// <param name="p_TailString">用于替代被截掉字符的字串</param>
        /// <returns>格式化后的字串</returns>
        /// <remarks>2013-03-19 黄波 创建</remarks>
        public static string GetUnicodeSubString(string str, int len, string p_TailString)
        {
            if (String.IsNullOrEmpty(str)) return string.Empty;
            string result = string.Empty;// 最终返回的结果
            int byteLen = System.Text.Encoding.Default.GetByteCount(str);// 单字节字符长度
            int charLen = str.Length;// 把字符平等对待时的字符串长度
            int byteCount = 0;// 记录读取进度
            int pos = 0;// 记录截取位置
            if (byteLen > len)
            {
                for (int i = 0; i < charLen; i++)
                {
                    if (Convert.ToInt32(str.ToCharArray()[i]) > 255)// 按中文字符计算加2
                        byteCount += 2;
                    else// 按英文字符计算加1
                        byteCount += 1;
                    if (byteCount > len)// 超出时只记下上一个有效位置
                    {
                        pos = i;
                        break;
                    }
                    else if (byteCount == len)// 记下当前位置
                    {
                        pos = i + 1;
                        break;
                    }
                }

                if (pos >= 0)
                    result = str.Substring(0, pos) + p_TailString;
            }
            else
                result = str;

            return result;
        }

        /// <summary>
        /// 将 “IList”转换为用separator分开的字符串
        /// </summary>
        /// <param name="intList">The int list.</param>
        /// <param name="separator">The separator.</param>
        /// <returns>字符串</returns>
        /// <remarks>
        /// 2013-03-12 杨文兵 创建
        /// </remarks>
        public static string IListToString(IList<int> intList, string separator)
        {
            string str = string.Empty;
            foreach (var sysno in intList)
            {
                str += sysno + separator;
            }
            if (str.EndsWith(separator)) str = str.Substring(0, str.Length - separator.Length);

            return str;
        }

        /// <summary>
        /// 将 “IList”转换为用separator分开的字符串
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="list">要连接的对象</param>
        /// <param name="separator">分隔符</param>
        /// <returns></returns>
        /// <remarks>2013-10-16 黄波 创建</remarks>
        public static string IEnumerableToString<T>(IEnumerable<T> list, string separator)
        {
            var sb = new StringBuilder("");
            foreach (var item in list)  sb.Append(item).Append(separator);
            var result = sb.ToString();
            if (result.EndsWith(separator))  result = result.TrimEnd(separator.ToCharArray());
            return result;
        }

        /// <summary>
        /// 货币格式化
        /// </summary>
        /// <param name="number">输入数字</param>
        /// <param name="reserve">小数保留位数</param>
        /// <param name="symbol">货币符号</param>
        /// <returns>格式化数据</returns>
        /// <remarks>2013-07-01 朱家宏 创建</remarks>
        public static string FormatCurrency(object number, byte reserve, string symbol = "&yen;")
        {
            var formatted = "";
            if (!string.IsNullOrWhiteSpace(number.ToString()))
            {
                var format = "{0:N" + reserve.ToString() + "}";
                formatted = symbol + string.Format(format, decimal.Parse(number.ToString()));
            }
            return formatted;
        }
        
        /// <summary>
        /// 字节格式化成友好大小
        /// </summary>
        /// <param name="bytes">字节</param>
        /// <returns>格式化数据</returns>
        /// <remarks>2013-08-13 黄志勇 创建</remarks>
        public static string FormatByteCount(long bytes)
        {
            int unit = 1024;
            if (bytes < unit) return bytes + " B";
            int exp = (int)(Math.Log(bytes) / Math.Log(unit));
            return String.Format("{0:F1} {1}B", bytes / Math.Pow(unit, exp), "KMGTPE"[exp - 1]);
        }
        /// <summary>
        /// Json特符字符过滤，参见http://www.json.org/
        /// </summary>
        /// <param name="sourceStr">要过滤的源字符串</param>
        /// <returns>返回过滤的字符串</returns>
        public static string JsonCharFilter(string sourceStr)
        {
            sourceStr = sourceStr.Replace("\\", "\\\\");
            sourceStr = sourceStr.Replace("\b", "\\\b");
            sourceStr = sourceStr.Replace("\t", "\\\t");
            sourceStr = sourceStr.Replace("\n", "\\\n");
            sourceStr = sourceStr.Replace("\n", "\\\n");
            sourceStr = sourceStr.Replace("\f", "\\\f");
            sourceStr = sourceStr.Replace("\r", "\\\r");
            return sourceStr.Replace("\"", "\\\"");
        }
    }
}
