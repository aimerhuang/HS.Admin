using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.SmartConfig
{
    /// <summary>
    /// 业务数据配置类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks>2014-08-20 杨文兵 创建</remarks>
    public class Smart<T>
    {
        /// <summary>
        /// 请勿直接new这个对象，使用SmartConfigBo.Get 方法获取对象
        /// </summary>
        /// <param name="typeIdentity"></param>
        /// <param name="timestamp"></param>
        /// <param name="config"></param>
        public Smart(string typeIdentity, string timestamp, T config)
        {
            this.Timestamp = timestamp;
            this.Type = typeof(T).Name.Replace("C_", "");
            this.TypeIdentify = typeIdentity;
            this.Config = config;
        }
        /// <summary>
        /// 业务数据类型
        /// </summary>
        public string Type { get; private set; }
        /// <summary>
        /// 业务数据标识
        /// </summary>
        public string TypeIdentify { get; private set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        public string Timestamp { get; private set; }
        /// <summary>
        /// 配置
        /// </summary>
        public T Config { get; private set; }
    }
}
