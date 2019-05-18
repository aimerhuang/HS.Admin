using Hyt.Model.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Extra.Erp
{
    /// <summary>
    /// erp接口配置
    /// </summary>
    /// <remarks>2016-11-22 杨浩 创建</remarks>
    internal sealed class ErpConfigs
    {
        public static ErpConfigs Instance
        {
            get { return new ErpConfigs(); }
        }
        /// <summary>
        /// 获取配置文件信息
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="configFileName">配置文件名称</param>
        /// <returns>配置文件</returns>
        /// <remarks>
        /// 2016-11-22 杨浩 创建
        /// </remarks>
        public T GetConfig<T>(string configFileName)
        {

            string fileName = Hyt.Util.WebUtil.GetMapPath("/config/") + configFileName;

            if (!Hyt.Util.FileUtil.HasFile(fileName))
            {
                Type type = typeof(T);
                ConfigBase config = (ConfigBase)type.Assembly.CreateInstance(type.FullName);
                Hyt.Util.Serialization.SerializationUtil.Save(config, fileName);
            }

            return Hyt.Util.Serialization.SerializationUtil.XmlDeserialize<T>(File.ReadAllText(fileName));

        }
        /// <summary>
        /// 获取Erp接口配置
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-11-22 杨浩 创建</remarks>
        public ErpConfig GetErpConfig()
        {
            return GetConfig<ErpConfig>("Erp.config");
        }
    }
}
