using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Util
{
    /// <summary>
    /// 扩展集合方法
    /// </summary>
    /// <remarks> 2013-6-26 杨浩 添加 </remarks>
    public static class EnumerableExtension
    {
        /// <summary>
        /// 遍历执行迭代器中的每一项
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="items">集合项</param>
        /// <param name="action">委托方法</param>
        /// <returns></returns>
        /// <remarks>2014-1-21 杨浩 创建</remarks>
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
                action(item);
        }

        /// <summary>
        /// 用指定分隔符拼接集合
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="items">集合对象</param>
        /// <param name="delimeter">分隔符</param>
        /// <returns>拼接后的字符串</returns>
        /// <remarks> 2013-6-26 杨浩 添加 </remarks>
        public static string Join<T>(this IList<T> items, string delimeter)
        {
            if (items == null || items.Count == 0)
                return string.Empty;

            if (items.Count == 1)
                return items[0].ToString();

            StringBuilder buffer = new StringBuilder();
            buffer.Append(items[0].ToString());

            for (int ndx = 1; ndx < items.Count; ndx++)
            {
                string append = items[ndx].ToString();
                buffer.Append(delimeter + append);
            }
            return buffer.ToString();
        }

        /// <summary>
        /// 用指定分隔符拼接集合项
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="items">对象</param>
        /// <param name="delimeter">分隔符</param>
        /// <param name="appender">使用指定方法处理</param>
        /// <returns>拼接后的字符串</returns>
        /// <remarks> 2013-6-26 杨浩 添加 </remarks>
        public static string JoinDelimited<T>(this IList<T> items, string delimeter, Func<T, string> appender)
        {
            if (items == null || items.Count == 0)
                return string.Empty;

            if (items.Count == 1)
                return appender(items[0]);

            StringBuilder buffer = new StringBuilder();
            string val = appender == null ? items[0].ToString() : appender(items[0]);
            buffer.Append(val);

            for(int ndx = 1; ndx < items.Count; ndx++)
            {
                T item = items[ndx];
                val = appender == null ? item.ToString() : appender(item);                
                buffer.Append(delimeter + val);
            }
            return buffer.ToString();
        }

        /// <summary>
        /// 用指定分隔符拼接集合项
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="items">对象</param>
        /// <param name="delimeter">分隔符</param>
        /// <param name="newLineAfterCount">指定行插入指定字符</param>
        /// <param name="newLineText">指定字符</param>
        /// <param name="appender">代理方法</param>
        /// <returns>拼接后的字符串</returns>
        /// <remarks> 2013-6-26 杨浩 添加 </remarks>
        public static string JoinDelimitedWithNewLine<T>(this IList<T> items, string delimeter, int newLineAfterCount, string newLineText, Func<T, string> appender)
        {
            if (items == null || items.Count == 0)
                return string.Empty;

            if (items.Count == 1)
                return appender(items[0]);

            StringBuilder buffer = new StringBuilder();
            buffer.Append(appender(items[0]));

            for (int ndx = 1; ndx < items.Count; ndx++)
            {
                T item = items[ndx];
                string append = appender(item);
                if (ndx % newLineAfterCount == 0)
                    buffer.Append(newLineText);

                buffer.Append(delimeter + append);
            }
            return buffer.ToString();
        }

        /// <summary>
        /// 转换可枚举集合到一个分隔的字符串
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="items">对象</param>
        /// <param name="delimeter">分隔符</param>
        /// <returns>拼接后的字符串</returns>
        /// <remarks>2014-1-21 杨浩 创建</remarks>
        public static string AsDelimited<T>(this IEnumerable<T> items, string delimeter)
        {
            List<string> itemList = new List<string>();
            foreach (T item in items)
            {
                itemList.Add(item.ToString());
            }
            return String.Join(delimeter, itemList.ToArray());
        }

        #region Conditional Checks
        /// <summary>
        /// Determines whether the specified enumerable collection is empty.
        /// Note: This method has the side effect of moving the position of
        /// the enumerator back to the starting position. Normally, this
        /// shouldn't effect anything unless you have a non-standard IEnumerable
        /// implementation.
        /// See also <a href="http://stackoverflow.com/questions/41319/checking-if-a-list-is-empty-with-linq"></a>.
        /// </summary>
        /// <typeparam name="T">Type of items to use.</typeparam>
        /// <param name="items">Enumerable to test</param>
        /// <returns>
        /// 	<c>true</c> if the specified collection is empty; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>2014-1-21 杨浩 创建</remarks>
        public static Boolean IsEmpty<T>(this IEnumerable<T> items)
        {
            if (items == null) throw new ArgumentNullException("items");

            var isEmpty = !items.GetEnumerator().MoveNext();
            /* Reset the enumerator back to the starting position in the off
             * chance that we have a very poorly implemented IEnumerable
             * that does not return a *new* enumerator with every invocation
             * of the GetEnumerator method. */
            try
            {
                items.GetEnumerator().Reset();
            }
            // If this method is not supported, just skip the operation
            catch (NotSupportedException)
            {
            }

            return isEmpty;
        }

        /// <summary>
        /// Determines whether the specified enumerable collection is empty.
        /// Note: This method has the side effect of moving the position of
        /// the enumerator back to the starting position.
        /// </summary>
        /// <typeparam name="T">Type of items to use.</typeparam>
        /// <param name="items">Enumerable to test</param>
        /// <returns>
        /// 	<c>true</c> if the specified collection is empty; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>2014-1-21 杨浩 创建</remarks>
        public static Boolean IsNullOrEmpty<T>(this IEnumerable<T> items)
        {
            return items == null || items.IsEmpty();
        }

        /// <summary>
        /// Check for any nulls.
        /// </summary>
        /// <typeparam name="T">Type of items to use.</typeparam>
        /// <param name="items">List of items.</param>
        /// <returns>True if a null is present in the list.</returns>
        /// <remarks>2014-1-21 杨浩 创建</remarks>
        public static bool HasAnyNulls<T>(this IEnumerable<T> items)
        {
            return IsTrueForAny<T>(items, t => t == null);
        }

        /// <summary>
        /// Check if any of the items in the collection satisfied by the condition.
        /// </summary>
        /// <typeparam name="T">Type of items to use.</typeparam>
        /// <param name="items">List of items to use.</param>
        /// <param name="executor">Function to call for each item.</param>
        /// <returns>True if the executor returned True for at least one item.</returns>
        /// <remarks>2014-1-21 杨浩 创建</remarks>
        public static bool IsTrueForAny<T>(this IEnumerable<T> items, Func<T, bool> executor)
        {

            foreach (T item in items)
            {
                bool result = executor(item);
                if (result)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Check if all of the items in the collection satisfied by the condition.
        /// </summary>
        /// <typeparam name="T">Type of items to use.</typeparam>
        /// <param name="items">List of items.</param>
        /// <param name="executor">Function to call for each item.</param>
        /// <returns>True if the executor returned true for all items.</returns>
        /// <remarks>2014-1-21 杨浩 创建</remarks>
        public static bool IsTrueForAll<T>(this IEnumerable<T> items, Func<T, bool> executor)
        {            
            foreach (T item in items)
            {
                bool result = executor(item);
                if (!result)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 将Ilist类型转换为IDictionary类型
        /// </summary>
        /// <typeparam name="T">Type of items to use.</typeparam>
        /// <param name="items">List of items.</param>
        /// <returns>Converted list as dictionary.</returns>
        /// <remarks>2014-1-21 杨浩 创建</remarks>
        public static IDictionary<T, T> ToDictionary<T>(this IList<T> items)
        {
            IDictionary<T, T> dict = new Dictionary<T, T>();
            foreach (T item in items)
            {
                dict[item] = item;
            }
            return dict;
        }        
        #endregion
    }
}
