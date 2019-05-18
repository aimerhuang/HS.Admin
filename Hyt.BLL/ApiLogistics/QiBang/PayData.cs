using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.ApiLogistics.QiBang
{
    public class PayData
    {
        public PayData()
        {

        }

        //采用排序的Dictionary的好处是方便对数据包进行签名，不用再签名之前再做一次排序
        private Dictionary<string, object> m_values = new Dictionary<string, object>();

        /**
        * 设置某个字段的值
        * @param key 字段名
         * @param value 字段值
        */
        public void SetValue(string key, object value)
        {
            m_values[key] = value;
        }

        /**
        * 根据字段名获取某个字段的值
        * @param key 字段名
         * @return key对应的字段值
        */
        public object GetValue(string key)
        {
            object o = null;
            m_values.TryGetValue(key, out o);
            return o;
        }

        /**
         * 判断某个字段是否已设置
         * @param key 字段名
         * @return 若字段key已被设置，则返回true，否则返回false
         */
        public bool IsSet(string key)
        {
            object o = null;
            m_values.TryGetValue(key, out o);
            if (null != o)
                return true;
            else
                return false;
        }
        /**
        * @将Dictionary转成xml
        * @return 经转换得到的xml串
        * @throws WxPayException
        **/
        public string ToXml()
        {
            //数据为空时不能转化为xml格式
            if (0 == m_values.Count)
            {
                // Log.Error(this.GetType().ToString(), "WxPayData数据为空!");
                throw new Exception("ToXml数据为空!");
            }

            string xml = "";//<?xml version='1.0' encoding='utf-8'?>
            xml += "<request>";
            foreach (KeyValuePair<string, object> pair in m_values)
            {
                if (pair.Value.GetType() == typeof(int) || pair.Value.GetType() == typeof(string) || pair.Value.GetType() == typeof(decimal))
                {
                    xml += "<" + pair.Key + ">" + pair.Value + "</" + pair.Key + ">";
                }
            }
            xml += "</request>";
            return xml;
        }
        /// <summary>
        /// 把订单项转换为XML
        /// </summary>
        /// <returns></returns>
        public string ToOrderItem()
        {
            //数据为空时不能转化为xml格式
            if (0 == m_values.Count)
            {
                // Log.Error(this.GetType().ToString(), "WxPayData数据为空!");
                // throw new WxPayException("WxPayData数据为空!");
            }

            string xml = "<order_item>";
            foreach (KeyValuePair<string, object> pair in m_values)
            {
                //字段值不能为null，会影响后续流程
                if (pair.Value == null)
                {
                    throw new Exception("ToOrderItem内部含有值为null的字段!");
                }
                if (pair.Value.GetType() == typeof(int) || pair.Value.GetType() == typeof(string))
                {
                    xml += "<" + pair.Key + ">" + pair.Value + "</" + pair.Key + ">";
                }
                else//除了string和int类型不能含有其他数据类型
                {
                    throw new Exception("ToOrderItem字段数据类型错误!");
                }
            }
            xml += "</order_item>";
            return xml;
        }
    }
}
