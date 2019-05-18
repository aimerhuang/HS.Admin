using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Hyt.Util.Serialization;

namespace Hyt.Util
{
    /// <summary>
    /// Cookie工具类
    /// </summary>
    /// <remarks>2014-1-21 黄波 创建</remarks>
    public class CookieUtil
    {
        /// <summary>
        /// 保存Cookie
        /// </summary>
        /// <param name="key">Cookie名称</param>
        /// <param name="value">Cookie值</param>
        /// <param name="expires">过期时间</param>
        /// <returns></returns>
        /// <remarks>2014-1-21 黄波 创建</remarks>
        public static void SetCookie(string key, string value, DateTime expires)
        {
            HttpCookie cookie = new HttpCookie(key);
            cookie.Value = HttpUtility.UrlEncode(value);
            cookie.Expires = expires;
            if (HttpContext.Current.Response.Cookies.AllKeys.Any(o => o == key))
            {
                //HttpContext.Current.Response.SetCookie(cookie);
                HttpContext.Current.Response.Cookies.Remove(key);
            }
            //else
            //{
            //    HttpContext.Current.Response.AppendCookie(cookie);
            //}
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 保存Cookie
        /// </summary>
        /// <param name="key">Cookie名称</param>
        /// <param name="value">Cookie值</param>
        /// <returns></returns>
        /// <remarks>2014-1-21 黄波 创建</remarks>
        public static void SetCookie(string key, string value)
        {
            SetCookie(key, value, DateTime.Now.AddHours(24));
        }

        /// <summary>
        /// 获取Cookie
        /// </summary>
        /// <param name="key">Cookie名称</param>
        /// <returns>cookie字符串</returns>
        /// <remarks>2014-1-21 黄波 创建</remarks>
        public static string Get(string name)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[name];
            return cookie == null ? string.Empty : HttpUtility.UrlDecode(cookie.Value);
        }

        /// <summary>
        /// 返回指定名称的Cookie实例
        /// </summary>
        /// <param name="name">Cookie名称</param>
        /// <returns>Cookie对象</returns>
        /// <remarks>2014-1-21 黄波 创建</remarks>
        public static HttpCookie GetCookie(string name)
        {
            var _cookie = HttpContext.Current.Request.Cookies[name];
            return _cookie;
        }

        /// <summary>
        /// 返回将指定名称的Cookie的值反序列化的对象
        /// </summary>
        /// <typeparam name="T">返回的对象类型</typeparam>
        /// <param name="name">Cookie名称</param>
        /// <returns>Cookie字符串反序列化后后对象</returns>
        /// <remarks>2014-1-21 黄波 创建</remarks>
        public static T Get<T>(string name) where T : class
        {
            //可以对cookie进行连续的操作
            var _cookie = HttpContext.Current.Response.Cookies[name];
            if (string.IsNullOrWhiteSpace(_cookie.Value))
            {
                _cookie = HttpContext.Current.Request.Cookies[name];
            }
            if (!string.IsNullOrWhiteSpace(_cookie.Value))
            {
                return HttpUtility.UrlDecode(_cookie.Value).ToObject<T>();
            }
            return null;
        }

        /// <summary>
        /// 将指定的Cookie对象写入Response
        /// </summary>
        /// <param name="cookie">Cookie对象</param>
        /// <returns></returns>
        /// <remarks>2014-1-21 黄波 创建</remarks>
        public static void Set(HttpCookie cookie)
        {
            HttpContext.Current.Response.SetCookie(cookie);
        }
    }
}
