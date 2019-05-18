using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Hyt.Util.Extension
{
    /// <summary>
    /// 字符串扩展
    /// </summary>
    /// <remarks>2014-1-21 杨浩 创建</remarks>
    public static class StringExtension
    {
        /// <summary>
        /// utf8编码和Js的_utf8_encode一致
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        /// <remarks>2015-9-11 杨浩 创建</remarks>
        public static string Utf8Encode(this string str)
        {
            str = str.Replace("\r\n", "\n");
            var utftext = "";
            for (var n = 0; n < str.Length; n++)
            {
                var c = (int)str[n];
                if (c < 128)
                {
                    utftext += Convert.ToChar(c);// String.fromCharCode(c);
                }
                else if ((c > 127) && (c < 2048))
                {
                    utftext += Convert.ToChar((c >> 6) | 192);
                    utftext += Convert.ToChar((c & 63) | 128);
                }
                else
                {
                    utftext += Convert.ToChar((c >> 12) | 224);
                    utftext += Convert.ToChar(((c >> 6) & 63) | 128);
                    utftext += Convert.ToChar((c & 63) | 128);
                }

            }
            return Microsoft.JScript.GlobalObject.escape(utftext);
        }
        /// <summary>
        /// 字符串转换为整型
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>转换后的整型</returns>
        /// <remarks>2013-06-08 罗雄伟 重构</remarks>
        public static int ToInt(this string str,int defaultValue=0)
        {
            int _ReturnValue;
            try
            {
                _ReturnValue = int.Parse(str);
            }
            catch
            {
                _ReturnValue = defaultValue;
            }
            return _ReturnValue;
        }

        /// <summary>
        /// 字符串截取
        /// </summary>
        /// <param name="value">字符串</param>
        /// <param name="suffix">字符串后缀</param>
        /// <param name="startIndex">截取字符串的开始位置(从一开始)</param>
        /// <param name="length">截取字符串的长度</param>
        /// <returns>字符串</returns>
        /// <remarks>2013-09-23 朱家宏 创建</remarks>
        public static string SubString(this string value, string suffix, int startIndex, int length = 0)
        {
            var resultValue = value;
            if (!string.IsNullOrWhiteSpace(value))
            {
                var valueLength = value.Length;
                startIndex--;

                if (startIndex < 0)
                    startIndex = 0;
                if (length <= 0 && startIndex <= valueLength)
                    length = value.Substring(startIndex).Length;

                if ((valueLength - startIndex) >= length)
                    resultValue = value.Substring(startIndex, length);

                var po = value.LastIndexOf(resultValue, System.StringComparison.CurrentCulture);

                if (((po + resultValue.Length) < valueLength) && !string.IsNullOrWhiteSpace(resultValue))
                    resultValue += suffix;
            }
            return resultValue;
        }

        /// <summary>
        /// 扩展HTML编码
        /// 扩展：空格转译成&nbsp;
        /// </summary>
        /// <param name="value">字符串</param>
        /// <returns>编码后的HTML字符，页面绑定时请注意，要进行无转译绑定</returns>
        /// <remarks>2013-12-30 邵斌 创建</remarks>
        public static string HtmlEncode(this string value)
        {
            //字符不为空才进行编码
            if (!string.IsNullOrWhiteSpace(value))
            {
                Regex reg = new Regex(@"\s");
                return reg.Replace(System.Web.HttpContext.Current.Server.HtmlEncode(value), "&nbsp;");
            }

            return value;
        }

        /// <summary>
        /// 取指定长度的字符串
        /// </summary>
        /// <param name="startIndex">起始位置(默认为0)</param>
        /// <param name="length">指定长度</param>
        /// <param name="tailString">用于替换的字符串</param>
        /// <returns>截取后的字符串</returns>
        /// <remarks>2016-1-9 杨浩 创建</remarks>
        public static string SubString(this string value, int length,string tailString,int startIndex = 0)
        {
            string myResult = value;

            Byte[] bComments = Encoding.UTF8.GetBytes(value);
            foreach (char c in Encoding.UTF8.GetChars(bComments))
            {    //当是日文或韩文时(注:中文的范围:\u4e00 - \u9fa5, 日文在\u0800 - \u4e00, 韩文为\xAC00-\xD7A3)
                if ((c > '\u0800' && c < '\u4e00') || (c > '\xAC00' && c < '\xD7A3'))
                {
                    //if (System.Text.RegularExpressions.Regex.IsMatch(p_SrcString, "[\u0800-\u4e00]+") || System.Text.RegularExpressions.Regex.IsMatch(p_SrcString, "[\xAC00-\xD7A3]+"))
                    //当截取的起始位置超出字段串长度时
                    if (startIndex >= value.Length)
                        return "";
                    else
                        return value.Substring(startIndex,
                                                       ((length + startIndex) > value.Length) ? (value.Length - startIndex) : length);
                }
            }

            if (length >= 0)
            {
                byte[] bsSrcString = Encoding.Default.GetBytes(value);

                //当字符串长度大于起始位置
                if (bsSrcString.Length > startIndex)
                {
                    int p_EndIndex = bsSrcString.Length;

                    //当要截取的长度在字符串的有效长度范围内
                    if (bsSrcString.Length > (startIndex + length))
                    {
                        p_EndIndex = length + startIndex;
                    }
                    else
                    {   //当不在有效范围内时,只取到字符串的结尾

                        length = bsSrcString.Length - startIndex;
                        tailString = "";
                    }

                    int nRealLength = length;
                    int[] anResultFlag = new int[length];
                    byte[] bsResult = null;

                    int nFlag = 0;
                    for (int i = startIndex; i < p_EndIndex; i++)
                    {
                        if (bsSrcString[i] > 127)
                        {
                            nFlag++;
                            if (nFlag == 3)
                                nFlag = 1;
                        }
                        else
                            nFlag = 0;

                        anResultFlag[i] = nFlag;
                    }

                    if ((bsSrcString[p_EndIndex - 1] > 127) && (anResultFlag[length - 1] == 1))
                        nRealLength = length + 1;

                    bsResult = new byte[nRealLength];

                    Array.Copy(bsSrcString, startIndex, bsResult, 0, nRealLength);

                    myResult = Encoding.Default.GetString(bsResult);
                    myResult = myResult + tailString;
                }
            }

            return myResult;
        }
    }
}
