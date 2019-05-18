using System;
using System.Collections.Generic;
using Hyt.Model;
using System.Web;
using System.IO;
using Hyt.Model.Common;
using Hyt.Model.DouShabaoModel;

namespace Hyt.BLL.Config
{
    /// <summary>
    /// 配置类
    /// </summary>
    /// <remarks>2013-7-30 杨浩 创建</remarks>
    public class Config : BOBase<Config>
    {
        /// <summary>
        /// 锁对象
        /// </summary>
        /// <remarks>2016-3-20 杨浩 创建</remarks>
        private static object lockHelper = new object();

        /// <summary>
        /// 获取短信模板配置信息列表
        /// </summary>
        /// <returns>返回所有短信模板</returns>
        /// <remarks>2016-1-20 杨浩 创建</remarks>
        public SmsTemplateConfig GetSmsTemplateConfig()
        {
            return GetConfig<SmsTemplateConfig>("SmsTemplate.Config");
        }
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
        /// 获取上传文件配置信息
        /// </summary>
        /// <returns>上传配置信息</returns>
        /// <remarks>2013-7-30 黄波 创建</remarks>
        public UpLoadFileConfig GetUpLoadFileConfig()
        {
            return GetConfig<UpLoadFileConfig>("UploadFile.config");
        }

        /// <summary>
        /// 获取订单标识信息
        /// </summary>
        /// <returns>订单标识信息</returns>
        /// <remarks>2014-05-19 黄波 创建</remarks>
        public OrderImgFlagConfig GetOrderImgFlagConfig()
        {
            return GetConfig<OrderImgFlagConfig>("OrderImgFlag.config");
        }

        /// <summary>
        /// 获取豆沙包接口配置信息
        /// </summary>
        /// <returns>2017-7-7 罗熙 创建</returns>
        public DouShabaoConfig GetDouShabaoConfig()
        {
            return GetConfig<DouShabaoConfig>("DouShabao.config");
        }
        
        /// <summary>
        /// 获取配置文件信息
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="configFileName">配置文件名称</param>
        /// <returns>配置文件</returns>
        /// <remarks>
        /// 2013-7-30 杨浩 创建
        /// 2016-3-20 杨浩 增加找不到配置文件则生成一个
        /// </remarks>
        public T GetConfig<T>(string configFileName)
        {
            lock (lockHelper)
            {
                try
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
                catch
                {
                    return default(T);
                }
                    
            }                             
        }
        /// <summary>
        /// 更新配置文件
        /// </summary>
        /// <param name="config">配置文件对象</param>
        /// <param name="configFileName">配置文件名称</param>
        /// <returns></returns>
        /// <remarks>2017-1-10 杨浩 创建</remarks>
        public bool UpdateConfig(object config, string configFileName)
        {
            lock (lockHelper)
            {
                try
                {
                    string fileName = Hyt.Util.WebUtil.GetMapPath("/config/") + configFileName;
                    Hyt.Util.Serialization.SerializationUtil.Save(config, fileName);
                }
                catch 
                {
                    return false;
                }
               
                return true;             
            }          
        }

        /// <summary>
        /// 获取海关配置信息
        /// </summary>
        /// <returns>海关配置信息</returns>
        /// <remarks>2015-10-27 王耀发 创建</remarks>
        public CustomsConfig GetCustomsConfig()
        {
            return GetConfig<CustomsConfig>("Customs.config");
        }
        /// <summary>
        /// 获取开通联配置信息
        /// </summary>
        /// <returns>开通联海关配置信息</returns>
        /// <remarks>2015-10-27 王耀发 创建</remarks>
        public KaiLianTongConfig KaiLianTongConfig()
        {
            return GetConfig<KaiLianTongConfig>("KaiLianTong.config");
        }
        /// <summary>
        /// 获取海关配置信息
        /// </summary>
        /// <returns>海关配置信息3.0</returns>
        /// <remarks>2016-12-29 杨云奕 创建</remarks>
        public Customs3Config GetCustoms3Config()
        {
            return GetConfig<Customs3Config>("Customs3.config");
        }
        /// <summary>
        /// 微信支付报关配置信息
        /// </summary>
        /// <returns>海关配置信息</returns>
        /// <remarks>2016-6-8 杨云奕 创建</remarks>
        public TenpayCustomsConfig GetTenpayCustomsConfig()
        {
            return GetConfig<TenpayCustomsConfig>("TenpayCustoms.config");
        }
        /// <summary>
        /// 获取支付宝海关配置信息
        /// </summary>
        /// <returns>支付宝海关配置信息</returns>
        /// <remarks>2015-10-27 王耀发 创建</remarks>
        public AlipayCustomsConfig GetAlipayCustomsConfig()
        {
            return GetConfig<AlipayCustomsConfig>("AlipayCustoms.config");
        }

        /// <summary>
        /// 获取心怡物流
        /// </summary>
        /// <returns>订单标识信息</returns>
        /// <remarks>2014-05-19 黄波 创建</remarks>
        public AnnaConfig GetAnnaConfig()
        {
            return GetConfig<AnnaConfig>("Anna.config");
        }

        /// <summary>
        /// 获取物流配置
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-3-8 杨浩 创建</remarks>
        public LogisticsConfig GetLogisticsConfig()
        {
            return GetConfig<LogisticsConfig>("Logistics.config");
        }
        /// <summary>
        /// 获取物流配置
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-3-8 杨浩 创建</remarks>
        public LogisticsConfig2 GetLogisticsConfig2()
        {
            return GetConfig<LogisticsConfig2>("Logistics2.config");
        }
        /// <summary>
        /// 获取供应链配置信息
        /// </summary>
        /// <returns>供应链配置信息</returns>
        /// <remarks> Create By 刘伟豪 2016-3-8 </remarks>
        public SupplyConfig GetSupplyConfig()
        {
            return GetConfig<SupplyConfig>("Supply.config");
        }
        /// <summary>
        /// 网站全局配置
        /// </summary>
        /// <returns>网站全局配置信息</returns>
        /// <remarks> 2016-3-14 王耀发 创建 </remarks>
        public GeneralConfig GetGeneralConfig()
        {
            return GetConfig<GeneralConfig>("General.config");
        }
        /// <summary>
        /// 商检配置信息
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-4-1 王耀发 修改</remarks>
        public IcpInfoConfig GetIcqInfoConfig()
        {
            IcpInfoConfig ConfigEntity = new IcpInfoConfig();
            ConfigEntity = GetConfig<IcpInfoConfig>("IcpInfo.config");
            return ConfigEntity;
        }
        /// <summary>
        /// 定时任务配置
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-5-4 杨浩 添加</remarks>
        public ScheduleConfig GetScheduleConfig()
        {
            return GetConfig<ScheduleConfig>("Schedule.config");
        }
        /// <summary>
        /// 获取高捷物流配置信息
        /// </summary>
        /// <returns>高捷物流配置信息</returns>
        /// <remarks>2016-7-1 王耀发 创建</remarks>
        public GaoJieConfig GetGaoJieConfig()
        {
            return GetConfig<GaoJieConfig>("GaoJie.config");
        }
        /// <summary>
        /// 又一城第三方平台参数
        /// </summary>
        /// <returns></returns>
        public U1CityConfig GetU1CityConfig()
        {
            return GetConfig<U1CityConfig>("U1City.config");
        }

        
        internal QianDaiPayConfig GetQianDaiPayConfig()
        {
            return GetConfig<QianDaiPayConfig>("QianDaiPay.config");
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

        /// <summary>
        /// 数据库升级配置
        /// </summary>
        /// <returns></returns>
        /// <remarks>2017-1-10 杨浩 创建</remarks>
        public UpgradeConfig GetUpgradeConfig()
        {
            return GetConfig<UpgradeConfig>("Upgrade.config");
        }
        internal ElectricBusinessConfig GetElectricBusinessConfig()
        {
            return GetConfig<ElectricBusinessConfig>("ElectricBusiness.config");
        }
        /// <summary>
        /// 获取快递配置
        /// </summary>
        /// <returns></returns>
        /// <remarks>2017-12-13 杨浩 创建</remarks>
        public ExpressConfig GetExpressConfig()
        {
            return GetConfig<ExpressConfig>("ExpressList.config");
        }
    }
}