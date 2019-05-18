using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Hyt.Util
{
    /// <summary>
    /// 枚举工具
    /// </summary>
    /// <remarks>2013-6-18 杨浩 创建</remarks>
    public static class EnumUtil                       
    {
        private static readonly Dictionary<string, IDictionary<int, string>> EnumList = new Dictionary<string, IDictionary<int, string>>(); //枚举缓存池
        private static readonly object Sync = new object();

        /// <summary>
        /// 枚举转换成字典
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <returns>字典</returns>
        /// <remarks>2013-6-18 杨浩 创建</remarks>
        public static IDictionary<int, string> ToDictionary(Type enumType)
        {
            string keyname = enumType.FullName;
            lock (Sync)
            {
                if (!EnumList.ContainsKey(keyname))
                {
                    IDictionary<int, string> list = new Dictionary<int, string>();

                    foreach (int i in Enum.GetValues(enumType))
                    {
                        string name = Enum.GetName(enumType, i);

                        //取描述名称
                        string showName = string.Empty;
                        FieldInfo enumInfo = enumType.GetField(name);
                        var enumAttributes =
                            (DescriptionAttribute[]) enumInfo.GetCustomAttributes(typeof (DescriptionAttribute), false);
                        showName = enumAttributes.Length > 0 ? enumAttributes[0].Description : name;

                        list.Add(i, showName);
                    }

                    if (!EnumList.ContainsKey(keyname))
                    {
                        EnumList.Add(keyname, list);
                    }
                }
            }
            return EnumList[keyname];
        }

        /// <summary>
        /// 获取枚举类型Description(没有缓存)
        /// </summary>
        /// <param name="e">枚举类型</param>
        /// <returns>Description</returns>
        /// <remarks>2013-6-18 杨浩 创建</remarks>
        public static string GetDescription(this Enum e)
        {
            FieldInfo enumInfo = e.GetType().GetField(e.ToString());
            var enumAttributes =
                (DescriptionAttribute[])enumInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return enumAttributes.Length > 0 ? enumAttributes[0].Description : e.ToString();
        }

        /// <summary>
        /// 获取枚举值对应的显示名称(取缓存)
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <param name="intValue">枚举项对应的int值</param>
        /// <returns>枚举名称</returns>
        /// <remarks>2013-6-18 杨浩 创建</remarks>
        public static string GetDescription(Type enumType, int intValue)
        {
            var list = ToDictionary(enumType);
            if (list.ContainsKey(intValue))
                return list[intValue];
            return string.Empty;
        }

        /// <summary>
        /// 返回指定枚举类型的指定名称或值的描述
        /// </summary>
        /// <param name="type">枚举类型</param>
        /// <param name="value">枚举名称或值</param>
        /// <returns>枚举的描述</returns>
        /// <remarks>2013-08-14 黄志勇 创建</remarks>
        public static string GetDescription(Type type, object value)
        {
            try
            {
                string name = value is int?Enum.GetName(type, value):value.ToString();
                FieldInfo fi = type.GetField(name);
                var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                return (attributes.Length > 0) ? attributes[0].Description : name;
            }
            catch
            {
                return "未知";
            }
        }

        /// <summary>
        /// 转换Enum到SelectListItem
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <returns>Text:Description Value:Name</returns>
        /// <remarks>2013-08-14 黄志勇 创建</remarks>
        public static List<SelectListItem> ToListItem<T>() where T : struct, IConvertible
        {
            var list = new List<SelectListItem>();
            if (!typeof(T).IsEnum) throw new ArgumentException("必须是枚举类型");
            Array arr = Enum.GetValues(typeof (T));
            list.AddRange(from T value in arr
                          select new SelectListItem
                          {
                              Value = value.ToString(),
                              Text = string.Format("{0}{1}", value, GetDescription(typeof(T), value))
                          });
            return list;
        }

        /// <summary>
        /// 访问列表，并把一个空值函数作用于列表中每个元素
        /// </summary>
        /// <typeparam name="T">泛型元素</typeparam>
        /// <param name="sequence">列表</param>
        /// <param name="action">空值函数</param>
        /// <returns></returns>
        /// 调用示例：
        /// var values = new List<int> {10,20,35};
        /// values.Apply(v=>Console.Write(x));
        /// <remarks>2013-08-19 黄志勇 创建</remarks>
        public static void Apply<T>(this IEnumerable<T> sequence, Action<T> action)
        {
            if (sequence != null && sequence.Count() > 0)
            {
                foreach (T item in sequence)
                {
                    action(item);
                }
            }
        }

        /// <summary>
        /// 访问列表，并把一个空值函数作用于列表中每个元素(并行)
        /// </summary>
        /// <typeparam name="T">泛型元素</typeparam>
        /// <param name="sequence">列表</param>
        /// <param name="action">空值函数</param>
        /// <returns></returns>
        /// 调用示例：
        /// var values = new List<int> {10,20,35};
        /// values.ApplyParallel(v=>Console.Write(x));
        /// <remarks>2013-08-21 黄志勇 创建</remarks>
        public static void ApplyParallel<T>(this IEnumerable<T> sequence, Action<T> action)
        {
            if (sequence != null && sequence.Count() > 0)
            {
                Parallel.ForEach(sequence, item => action(item));
            }
        }

        /// <summary>
        /// 转换Enum到SelectListItem
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <returns>返回List SelectListItem </returns>
        /// <remarks>2013-06-14 周唐炬 创建</remarks>
        public static void ToListItem<T>(ref List<SelectListItem> selectList) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum) throw new ArgumentException("必须是枚举类型");
            selectList.AddRange(from int s in Enum.GetValues(typeof(T))
                                select new SelectListItem
                                    {
                                        Value = s.ToString(CultureInfo.InvariantCulture),
                                        Text = Enum.GetName(typeof(T), s)
                                    });
        }

        /// <summary>
        /// 将枚举转换成Json数据
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <returns>返回object类型的动态类 </returns>
        /// <remarks>2013-6-18 杨浩 创建</remarks>
        public static object ToJson(Type enumType)
        {
            //将枚举转换成数据字典
            var list = ToDictionary(enumType);

            StringBuilder builder = new StringBuilder("{");

            //根据枚举的int值附加到json字符串中
            foreach (int i in list.Keys)
            {
                if (builder.Length > 1)
                    builder.Append(",");
                builder.Append(string.Format("{0}:\"{1}\"", i, list[i]));
            }

            builder.Append("}");

            //通过反序列化
            var result = Hyt.Util.Serialization.JsonUtil.ToObject<dynamic>(builder.ToString());

            //返回结果
            return result;
        }


        /// <summary>
        /// 将枚举类型字符串转换为特定Int类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <returns></returns>
        /// 吴琨 2017/8/30 创建
        public static int StringToInt<T>(this string s) where T : struct , IConvertible
        {
            int r = -1;

            foreach (T item in Enum.GetValues(typeof(T)))
            {
                if (Convert.ToString(item) == s)
                {
                    r = Convert.ToInt32(item);
                    break;
                }
            }
            return r;
        }
    }
}
