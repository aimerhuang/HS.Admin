using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;
using Hyt.Model.B2CApp;

namespace Hyt.DataAccess.Version
{
    /// <summary>
    /// APP版本信息
    /// </summary>
    /// <remarks>2013-08-20 周唐炬 创建</remarks>
    public abstract class IApVersionDao : DaoBase<IApVersionDao>
    {
        /// <summary>
        /// 获取最新APP版本信息
        /// </summary>
        /// <param name="type">App系统类型</param>
        /// <returns>最新APP版本信息</returns>
        /// <remarks>2013-08-20 周唐炬 创建</remarks>
        public abstract ApVersion GetApVersion(AppEnum.AppType type);
    }
}
