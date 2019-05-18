using System.Collections.Generic;
using Hyt.DataAccess.Base;
using Hyt.Model;


namespace Hyt.DataAccess.Sys
{    
    /// <summary>
    /// 业务数据配置
    /// </summary>
    /// <remarks>2014-08-20 杨文兵 创建</remarks>
    public abstract class ISySmartConfigDao : DaoBase<ISySmartConfigDao>
    {
        /// <summary>
        /// 更新 SySmartConfig
        /// </summary>
        /// <param name="type">业务类型</param>
        /// <param name="typeIdentity">业务类型标识</param>
        /// <param name="config">配置</param>
        /// <param name="timestamp">更新前的时间戳</param>
        /// <param name="newTimestamp">更新后的时间戳</param>
        /// <returns></returns>
        /// <remarks>2014-08-20 杨文兵 创建</remarks>
        public abstract bool Update(string type, string typeIdentity, string config, string timestamp, string newTimestamp);
        /// <summary>
        /// 获取 SySmartConfig 如果数据库中没有，则创建并返回SySmartConfig
        /// </summary>
        /// <param name="type">业务类型</param>
        /// <param name="typeIdentity">业务类型标识</param>
        /// <param name="timestamp">获取时的时间戳</param>
        /// <remarks>2014-08-20 杨文兵 创建</remarks>
        public abstract SySmartConfig Get(string type, string typeIdentity, string timestamp);
    }
}
