using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.Model.SmartConfig;

namespace Hyt.BLL.Basic
{
    /// <summary>
    /// 业务数据配置
    /// </summary>
    /// <remarks>
    /// 业务数据配置类在Hyt.Model.SmartConfig中创建，类名规则为：前缀“C_ ”+ “业务表名”（这种方式不太符合规范但是好用，配合VS智能提示写代码方便）    
    /// 2014-08-20 杨文兵 创建
    /// </remarks>
    public static class SmartConfigBo
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
        private static bool Update(string type, string typeIdentity, string config, string timestamp, string newTimestamp) {
            return Hyt.DataAccess.Sys.ISySmartConfigDao.Instance.Update(type, typeIdentity, config, timestamp, newTimestamp);
        }
        /// <summary>
        /// 获取 SySmartConfig 如果数据库中没有，则创建并返回SySmartConfig
        /// </summary>
        /// <param name="type">业务类型</param>
        /// <param name="typeIdentity">业务类型标识</param>
        /// <param name="timestamp">时间戳</param>
        /// <returns></returns>
        /// <remarks>2014-08-20 杨文兵 创建</remarks>
        private static SySmartConfig Get(string type, string typeIdentity,string timestamp) {
            return Hyt.DataAccess.Sys.ISySmartConfigDao.Instance.Get(type, typeIdentity, timestamp);
        } 

        /// <summary>
        /// 保存 业务数据配置
        /// </summary>
        /// <param name="smart">业务数据配置.</param>
        /// <remarks>2014-08-20 杨文兵 创建</remarks>
        public static bool Save<T>(Smart<T> smart)
        {
            if(smart == null) throw new ArgumentNullException("smart");
            return Update(smart.Type, smart.TypeIdentify, Newtonsoft.Json.JsonConvert.SerializeObject(smart.Config), smart.Timestamp, Timestamp(DateTime.Now));
        }

        /// <summary>
        /// 获取 业务数据配置
        /// </summary>
        /// <param name="typeIdentity">业务类型标识.</param>
        /// <remarks>2014-08-20 杨文兵 创建</remarks>
        public static Smart<T> Get<T>(string typeIdentity)
        {
            var type = typeof(T).Name.Replace("C_", "");
            var smartConfig = Get(type, typeIdentity, Timestamp(DateTime.Now));
            if (string.IsNullOrEmpty(smartConfig.Config)) smartConfig.Config = "{}";

            //转换出错异常不处理
            var config = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(smartConfig.Config);
            return new Smart<T>(typeIdentity, smartConfig.Timestamp, config); 
        }

        /// <summary>
        /// 获取时间戳.
        /// </summary>
        /// <param name="dt">时间.</param>
        /// <returns></returns>
        /// <remarks>2014-08-20 杨文兵 创建</remarks>
        private static string Timestamp(DateTime dt)
        {
            DateTime dt1 = new DateTime(1970, 1, 1);
            TimeSpan ts = dt - dt1;
            return ts.TotalMilliseconds.ToString();
        }
    }  

}
