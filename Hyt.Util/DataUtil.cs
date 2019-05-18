using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Hyt.Util
{
    /// <summary>
    /// 数据工具类
    /// </summary>
    /// <remarks>2014-1-21 黄波 创建</remarks>
    public class DataUtil
    {
        /// <summary>
        /// 检查DataSet中是否有数据行
        /// </summary>
        /// <param name="ds">待检查的DataSet</param>
        /// <returns>
        /// <c>true:存在</c>
        /// <c>false:不存在</c>
        /// </returns>
        /// <remarks>2014-1-21 黄波 创建</remarks>
        public static bool HasRow(DataSet ds)
        {
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 将DataReader值转换为其它数据类型
        /// </summary>
        /// <typeparam name="T">目标数据类型</typeparam>
        /// <param name="value">要转换的数据</param>
        /// <returns>转换后的数据</returns>
        /// <remarks>
        /// 创建 by 杨文兵 2013-03-06
        /// 修改 2013-03-29 杨文兵 如果T为日期并且为空 默认值改为sql server中的最小日期1753-1-1 12:00:00
        /// </remarks>
        public static T GetDataReader<T>(object value) where T : struct
        {
            T tValue = default(T);
            Type tType = typeof(T);
            try
            {
                if (DBNull.Value.Equals(value) == false)
                {
                    return (T)Convert.ChangeType(value, typeof(T));
                }
                else
                {
                    if (tType == typeof(DateTime))
                    {
                        return (T)Convert.ChangeType(new DateTime(1753, 1, 1, 12, 0, 0), typeof(T));
                    }
                    else
                    {
                        return tValue;
                    }
                }
            }
            catch { }
            return tValue;
        }

        /// <summary>
        /// 将DataReader值转换为字符串
        /// </summary>
        /// <param name="value">要转换的数据</param>
        /// <returns>转换后的字符串</returns>
        /// <remarks>
        /// 创建 by 杨文兵 2013-03-06
        /// </remarks>
        public static string GetDataReader(object value)
        {
            if (DBNull.Value.Equals(value) == false)
            {
                return Convert.ToString(value);
            }
            return string.Empty;
        }

        /// <summary>
        /// 返回对象大小(为估算)
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>字节B</returns>
        /// <remarks>2013-8-9 yangheyu Add</remarks>
        /// <remarks>2013-8-21 黄志勇 修改</remarks>
        public static long ObjectSize(object obj)
        {
            long size = 0;
            try
            {
                if (obj != null)
                {
                    var type = obj.GetType();
                    var binaryFormatter = new BinaryFormatter();
                    var stream = new MemoryStream();
                    binaryFormatter.Serialize(stream, obj);
                    stream.Seek(0, SeekOrigin.Begin);
                    size = stream.Length;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return size;
        }
    }

    /// <summary>
    /// 将一个基本类型转换为另一个基本类型
    /// </summary>
    /// <remarks>2015-11-13 陈海裕 创建</remarks>
    public class TConvert
    {
        /// <summary>
        /// 转换为Int32
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defValue">默认值</param>
        /// <returns></returns>
        /// <remarks>2015-11-13 陈海裕 创建</remarks>
        public static int ToInt32(object obj, int defValue = 0)
        {
            int result = 0;
            try
            {
                result = Convert.ToInt32(obj);
            }
            catch (Exception ex)
            {
                result = defValue;
            }

            return result;
        }

        public static decimal ToDecimal(object obj, decimal defValue = 0.0m)
        {
            decimal result = 0;
            try
            {
                result = Convert.ToDecimal(obj);
            }
            catch (Exception ex)
            {
                result = defValue;
            }
            return result;
        }

        public static string ToString(object obj, string defValue = "")
        {
            string result = "";
            try
            {
                result = Convert.ToString(obj);
            }
            catch (Exception ex)
            {
                result = defValue;
            }
            return result;
        }
    }
}
