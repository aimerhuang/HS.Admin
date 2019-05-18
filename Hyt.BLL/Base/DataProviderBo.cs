using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Provider;

namespace Hyt.BLL.Base
{
    /// <summary>
    /// 注册数据提供器
    /// </summary>
    /// <remarks>2013-10-11 杨浩 添加</remarks>
    public class DataProviderBo 
    {
        /// <summary>
        /// 设置一项数据提供实例
        /// </summary>
        /// <param name="provider"></param>
        /// <returns>void</returns>
        /// <remarks>2013-10-11 杨浩 添加</remarks>
        public static void Set(object provider)
        {
            ProviderManager.Set<IDataProvider>((IDataProvider)provider);
        }
    }
}
