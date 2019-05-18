using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;

namespace Hyt.DataAccess.Oracle
{
    /// <summary>
    /// 数据层实例定位
    /// </summary>
    /// <remarks>
    /// 2013-6-17 杨浩 创建
    /// </remarks>
    public sealed class DataProvider:Hyt.DataAccess.Provider.IDataProvider
    {
        private static Type[] _types=null;

        /// <summary>
        /// 创建实例
        /// </summary>
        /// <typeparam name="T">接口类型</typeparam>
        /// <returns>对象</returns>
        /// <remarks>2013-6-17 杨浩 创建</remarks>
        public T Create<T>() where T : DaoBase<T>
        {
            if (_types == null)
                _types = this.GetType().Assembly.GetExportedTypes();

            foreach (var type in _types)
            {
                if (type.IsSubclassOf(typeof(T)))
                {
                    //返回对象后被缓存
                    var instance = (T)Activator.CreateInstance(type);
                    return instance;
                }
            }

            throw new Exception(string.Format("数据访问类未实现接口\"{0}\"", typeof(T)));
        }
    }
}
