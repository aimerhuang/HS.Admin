using Hyt.DataAccess.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.DataAccess.Sys
{
    /// <summary>
    /// 升级数据库
    /// </summary>
    /// <remarks>2017-1-10 杨浩 创建</remarks>
    public abstract class ISyUpgradeTheDatabaseDao : DaoBase<ISyUpgradeTheDatabaseDao>
    {
        /// <summary>
        /// 升级版本
        /// </summary>
        public abstract decimal? Version {get;}
        /// <summary>
        /// 升级
        /// </summary>
        /// <returns></returns>
        /// <remarks>2017-1-10 杨浩 创建</remarks>
        public abstract bool Upgrade();      
    }
}
