using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Base
{
    /// <summary>
    /// 实例提供者
    /// </summary>
    /// <remarks>2015-10-12 杨浩 创建</remarks>
    public class InstanceProviderBo<T>
    {
        /// <summary>
        /// 类实例哈希仓储
        /// </summary>
        private static readonly Hashtable _instance = new Hashtable();
        private static object lockHelper = new object();
        private InstanceProviderBo() { }
        /// <summary>
        /// 获取类实例
        /// </summary>
        /// <param name="code">代码</param>
        /// <param name="partialNamespace">类所在的命名空间</param>
        /// <returns></returns>
        /// <remarks>2015-10-12 杨浩 创建</remarks>
        public static T GetInstance(string code, string namespaceStr)
        {
            string key = string.Format("{0}.{1}", namespaceStr, code);
            if (!_instance.ContainsKey(key))
            {
                lock (lockHelper)
                {
                    if (!_instance.ContainsKey(key))
                    {

                        var typeList = typeof(T)
                                     .Assembly.GetExportedTypes()
                                     .Where(x => (x.FullName.StartsWith(namespaceStr)))
                                     .ToList();

                        foreach (var type in typeList)
                        {
                            if (!type.IsAbstract && !type.IsSealed && !type.IsInterface)
                            {
                                try
                                {
                                    T _object = (T)Activator.CreateInstance(type);
                                    var propertyInfo = type.GetProperty("Code");
                                    //匹配对象下Code属性值与code是否一样否则继续循环(Code指定对象的标示)
                                    if (propertyInfo != null && ((int)propertyInfo.GetValue(_object, null)).ToString() == code)
                                    {
                                        _instance.Add(key, _object);
                                        break;
                                    }
                                }
                                catch { }

                            }
                        }
                        if (!_instance.ContainsKey(key))
                            throw new Exception(string.Format("命名空间:{0}下没找到属性Code值为{1},请指定Code属性。", namespaceStr, code));
                    }
                }
            }
            return (T)_instance[key];
        }
    }
}
