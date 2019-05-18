using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.DataAccess.Provider;

namespace Hyt.DataAccess
{
    /// <summary>
    /// 数据实例提供者的工厂实现
    /// </summary>
    /// <typeparam name="T">接口类型</typeparam>
    /// <Remark>
    /// 2013-6-26 杨浩 创建
    /// </Remark>
    public class DaoFactory<T> where T : DaoBase<T>
    {
        private static T _daoInstance;

        /// <summary>
        /// 创建实例
        /// </summary>
        /// <returns>实现了接口T的对象</returns>
        /// <remarks>2013-6-26 杨浩 创建</remarks>
        public static T Create()
        {
            if (_daoInstance != null)
                return _daoInstance;

            _daoInstance = ProviderManager.Get<IDataProvider>().Create<T>();

            return _daoInstance;
        }
    }
}
