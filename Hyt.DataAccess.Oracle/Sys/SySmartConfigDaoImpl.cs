using System.Collections.Generic;
using Hyt.DataAccess.Sys;
using Hyt.Model;

namespace Hyt.DataAccess.Oracle.Sys
{
    /// <summary>
    /// 角色权限
    /// </summary>
    /// <remarks>2013-08-01 朱家宏 创建</remarks>
    public class SySmartConfigDaoImpl : ISySmartConfigDao
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
        public override bool Update(string type, string typeIdentity, string config, string timestamp, string newTimestamp)
        {
            var row = Context.Sql("update SySmartConfig set Config=@Config,Timestamp=@NewTimestamp where Type=@Type and TypeIdentity=@TypeIdentity and Timestamp=@Timestamp")
                   .Parameter("Config", config)
                   .Parameter("NewTimestamp", newTimestamp)
                   .Parameter("Type", type)
                   .Parameter("TypeIdentity", typeIdentity)
                   .Parameter("Timestamp", timestamp)
                   .Execute();
            return row == 1;
        }

        /// <summary>
        /// 获取 SySmartConfig 如果数据库中没有，则创建并返回SySmartConfig
        /// </summary>
        /// <param name="type">业务类型</param>
        /// <param name="typeIdentity">业务类型标识</param>
        /// <param name="timestamp">获取时的时间戳</param>
        /// <returns></returns>
        /// <remarks>2014-08-20 杨文兵 创建</remarks>
        public override SySmartConfig Get(string type, string typeIdentity, string timestamp)
        {
            var config = Context.Sql("select * from SySmartConfig where Type=@Type and TypeIdentity=@TypeIdentity")
                .Parameter("Type", type)
                .Parameter("TypeIdentity", typeIdentity)
                .QuerySingle<SySmartConfig>();
            if (config != null) return config;

            config = new SySmartConfig()
            {
                Config = string.Empty,
                Timestamp = timestamp,
                Type = type,
                TypeIdentity = typeIdentity
            };
            config.SysNo = Context.Insert("SySmartConfig", config)
                          .AutoMap(o => o.SysNo)
                          .ExecuteReturnLastId<int>("SysNo");
            return config;
        }
    }
}
