using System;
using System.Collections.Generic;
using Hyt.Model;
using System.Web;
using System.IO;
using Hyt.Model.Common;
using Hyt.Tool.ImageBuilder.BLL;

namespace Hyt.Tool.ImageBuilder.Config
{
    /// <summary>
    /// 配置类
    /// </summary>
    /// <remarks>2013-7-30 黄波 创建</remarks>
    public class Config : BOBase<Config>
    {
        /// <summary>
        /// 获取商品图片配置信息
        /// </summary>
        /// <returns>商品图片配置信息</returns>
        /// <remarks>2013-7-30 黄波 创建</remarks>
        public ProductImageConfig GetProductImageConfig()
        {
            return GetConfig<ProductImageConfig>("ProductImage.Config");
        }

        /// <summary>
        /// 获取图片服务器配置信息
        /// </summary>
        /// <returns>图片服务器配置信息</returns>
        /// <remarks>2013-7-30 黄波 创建</remarks>
        public AttachmentConfig GetAttachmentConfig()
        {
            return GetConfig<AttachmentConfig>("Attachment.config");
        }

        /// <summary>
        /// 获取支付配置信息
        /// </summary>
        /// <returns>支付配置信息</returns>
        /// <remarks>2013-7-30 黄波 创建</remarks>
        public PayConfig GetPayConfig()
        {
            return GetConfig<PayConfig>("Pay.config");
        }

        /// <summary>
        /// 获取斯通配置信息
        /// </summary>
        /// <returns>系统配置信息</returns>
        /// <remarks>2013-7-30 黄波 创建</remarks>
        public SysConfig GetSysConfig()
        {
            return GetConfig<SysConfig>("Sys.config");
        }

        /// <summary>
        /// 获取配置文件信息
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="configFileName">配置文件名称</param>
        /// <returns>配置信息</returns>
        /// <remarks>2013-7-30 黄波 创建</remarks>
        private T GetConfig<T>(string configFileName)
        {
            //return Hyt.Util.Serialization.SerializationUtil.XmlDeserialize<T>(File.ReadAllText(System.Environment.CurrentDirectory + "/Config/" + configFileName));
            return Hyt.Tool.ImageBuilder.Serialization.SerializationUtil.XmlDeserialize<T>(File.ReadAllText(System.IO.Path.Combine(System.Environment.CurrentDirectory, configFileName)));
        }
    }
}
