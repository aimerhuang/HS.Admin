using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Util.Serialization;

namespace Hyt.Util.JPush
{
    /// <summary>
    /// 推送辅助工具
    /// </summary>
    /// <remarks>2014-01-17 邵斌 创建</remarks>
    public sealed class Utility
    {
        /// <summary>
        /// 转换对象成指定JSON对象
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <param name="title">标题</param>
        /// <param name="serviceType">推送服务类型</param>
        /// <param name="content">推送内容</param>
        /// <param name="parameter">参数</param>
        /// <returns>返回格式后的json字符串</returns>
        /// <remarks>2014-01-17 邵斌 创建</remarks>
        public static string ConvertObjectToContentString(int sysNo, string title, int serviceType, string content, string parameter)
        {
            /*
             Key名称	    是否必须	Value内容说明
             n_builder_id	可选        1-1000的数值，不填则默认为 0，使用 极光Push SDK 的默认通知样式。只有 Android 支持这个参数。进一步了解请参考文档 通知栏样式定制API
             n_title	    可选	    通知标题。不填则默认使用该应用的名称。只有 Android支持这个参考。
             n_content	    必须	    通知内容。
             n_extras	    可选        通知附加参数。JSON格式。客户端可取得全部内容。
             */

            title = System.Web.HttpContext.Current.Server.UrlEncode(title);
            content = System.Web.HttpContext.Current.Server.UrlEncode(content);
            parameter = System.Web.HttpContext.Current.Server.UrlEncode(parameter);

            var temp = new
            {
                n_builder_id = sysNo,
                n_title = title,
                n_content = content,
                n_extras = new { t = serviceType,p = parameter },
            };

            return temp.ToJson();
        }

        /// <summary>
        /// 针对IOS系统的，转换对象成指定JSON对象
        /// </summary>
        /// <param name="sysNo">系统编号</param>
        /// <param name="title">标题</param>
        /// <param name="content">推送内容</param>
        /// <param name="serviceType">推送服务类型</param>
        /// <param name="parameter">参数</param>
        /// <returns>返回格式后的json字符串</returns>
        /// <remarks>2014-01-17 邵斌 创建</remarks>
        public static string ConvertObjectToIosContentString(int sysNo, string title, string content,int serviceType, string parameter )
        {
            /*
                JPush 字段	                APNs 字段
                n_content	                alert
                n_extras -> ios -> badge	badge
                n_extras -> ios -> sound	sound
             */

            title = System.Web.HttpContext.Current.Server.UrlEncode(title);
            content = System.Web.HttpContext.Current.Server.UrlEncode(content);
            parameter = System.Web.HttpContext.Current.Server.UrlEncode(parameter);
            var temp = new
            {
                n_builder_id = sysNo,
                n_title = title,
                n_content = content,
                n_extras = new { 
                    ios=new
                        {
                            badge=1,
                            sound = "default"
                        },
                    t = serviceType,
                    p = parameter

                },
            };

            return temp.ToJson();

        }
    }
}
