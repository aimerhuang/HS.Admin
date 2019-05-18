using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Hyt.SDK
{
    public class Util
    {
        /// <summary>
        ///解决HttpUtility.UrlEncode会将符号编码为小写英文的问题
        /// </summary>
        /// <param name="temp">待编码的字符串</param>
        /// <param name="encoding">编码的方式</param>
        /// <returns>返回编码结果</returns>
        static public string UrlEncode(string temp, Encoding encoding)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < temp.Length; i++)
            {
                string t = temp[i].ToString();
                string k;
                switch (t)
                {
                    case "'":
                        t = "%27";
                        builder.Append(t);
                        break;

                    case " ":
                        t = "%20";
                        builder.Append(t);
                        break;

                    case "(":
                        t = "%28";
                        builder.Append(t);
                        break;

                    case ")":
                        t = "%29";
                        builder.Append(t);
                        break;

                    case "!":
                        t = "%21";
                        builder.Append(t);
                        break;

                    case "*":
                        t = "%2A";
                        builder.Append(t);
                        break;

                    default:
                        k = HttpUtility.UrlEncode(t, encoding);
                        if (t == k)
                        {
                            builder.Append(t);
                        }
                        else
                        {
                            builder.Append(k.ToUpper());
                        }
                        break;
                }
            }
            return builder.ToString();
        }
    }
}
