using Extra.UpGrade.HaiDaiModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Extra.UpGrade.Model
{
    /// <summary>
    /// 升舱配置信息
    /// </summary>
    /// <remarks>2014-01-03  陶辉 重构</remarks>
    public class UpGradeConfig
    {
        ///// <summary>
        ///// 升舱第三方商城订单是否确认发货
        ///// </summary>
        public static int SendDeliveryState
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["SendDeliveryState"]); }
        }



        /// <summary>
        /// 是否启用自动升舱
        /// </summary>
        public static bool EnableAutoUpGrade
        {
            get
            {
                try
                {
                    return Convert.ToBoolean(ConfigurationManager.AppSettings["EnableAutoUpGrade"]);
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 升舱发货标识
        /// </summary>
        /// <remarks>
        /// 在v3上进行测试时需要将标识配置成false
        /// 在正式环境中需要将标识配置成true
        /// </remarks>
        public static bool UpGradeDeliveryFlag
        {
            get
            {
                try
                {
                    return Convert.ToBoolean(ConfigurationManager.AppSettings["UpGradeDeliveryFlag"]);
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 淘宝授权地址
        /// </summary>
        /// <remarks> 2013-9-4 陶辉 创建 </remarks>
        public static string TaobaoAuthorizeUrl
        {
            get { return GetTaobaoConfig().AuthorizeUrl + string.Format("?response_type=code&client_id={0}&redirect_uri={1}&state=1212&view=tmall", GetTaobaoConfig().AppKey, GetTaobaoConfig().TaobaoCallBack); }
        }

        /// <summary>
        /// 拍拍授权地址
        /// </summary>
        /// <remarks>2014-01-03 陶辉 创建</remarks>
        public static string PaipaiAuthorizeUrl
        {
            get { return GetPaipaiConfig().AuthorizeUrl + string.Format("?responseType=access_token&appOAuthID={0}", GetPaipaiConfig().AppOAuthID); }
        }

        /// <summary>
        /// 一号店授权地址
        /// </summary>
        /// <remarks> 2017-08-23 黄杰 重构 </remarks>
        public static string YihaodianAuthorizeUrl
        {
            get { return GetYihaodianConfig().AuthorizeUrl + string.Format("?client_id={0}&response_type=code&redirect_uri={1}&state=1212&view=web", GetYihaodianConfig().AppKey, GetYihaodianConfig().YihaodianCallBack); }
        }

        /// <summary>
        /// 京东授权地址
        /// </summary>
        /// <remarks> 2017-08-21 黄杰 创建 </remarks>
        public static string JingDongAuthorizeUrl
        {
            get { return GetYihaodianConfig().AuthorizeUrl + string.Format("?response_type=code&client_id={0}&redirect_uri={1}&state=1212", GetJingDongConfig().AppKey, GetJingDongConfig().JingDongCallBack); }
        }

        /// <summary>
        /// 国美授权地址
        /// </summary>
        /// <remarks> 2017-08-30 黄杰 创建 </remarks>
        public static string GuoMeiAuthorizeUrl
        {
            get { return GetGuoMeiConfig().AuthorizeUrl + string.Format("?response_type=code&client_id={0}&state=2333&redirect_uri={1}", GetGuoMeiConfig().AppKey, GetGuoMeiConfig().GuoMeiCallBack); }
        }


        /// <summary>
        /// 获取海带接口配置信息
        /// </summary>
        /// <remarks>2013-9-4 陶辉 创建</remarks>
        public static HaiDaiConfig GetHaiDaiConfig()
        {
            return GetConfig<HaiDaiConfig>("HaiDai.config");
        }

        /// <summary>
        /// 获取淘宝接口配置信息
        /// </summary>
        /// <remarks>2013-9-4 陶辉 创建</remarks>
        public static TaobaoConfig GetTaobaoConfig()
        {
            return GetConfig<TaobaoConfig>("Taobao.config");
        }

        /// <summary>
        /// 获取拍拍接口配置信息
        /// </summary>
        /// <remarks>2013-9-4 陶辉 创建</remarks>
        public static PaipaiConfig GetPaipaiConfig()
        {
            return GetConfig<PaipaiConfig>("Paipai.config");
        }

        /// <summary>
        /// 获取一号店接口配置信息
        /// </summary>
        /// <remarks> 2017-08-23 黄杰 重构 </remarks>
        public static YihaodianConfig GetYihaodianConfig()
        {
            return GetConfig<YihaodianConfig>("Yihaodian.config");
        }

        /// <summary>
        /// 获取有赞接口配置信息
        /// </summary>
        /// <remarks>2016-6-11 杨浩 创建</remarks>
        public static YouZanConfig GetYouZanConfig()
        {
            return GetConfig<YouZanConfig>("YouZan.config");
        }

        /// <summary>
        /// 获取京东接口配置信息
        /// </summary>
        /// <returns>2017-08-10 黄杰 创建</returns>
        public static JingDongConfig GetJingDongConfig()
        {
            return GetConfig<JingDongConfig>("JingDong.config");
        }

        /// <summary>
        /// 获取格格家接口配置信息
        /// </summary>
        /// <returns>2017-08-18 黄杰 创建</returns>
        public static GeGeJiaConfig GetGeGeJiaConfig()
        {
            return GetConfig<GeGeJiaConfig>("GeGeJia.config");
        }

        /// <summary>
        /// 获取国美接口配置信息
        /// </summary>
        /// <returns>2017-08-29 黄杰 创建</returns>
        public static GuoMeiConfig GetGuoMeiConfig()
        {
            return GetConfig<GuoMeiConfig>("GuoMei.config");
        }

        /// <summary>
        /// 获取苏宁接口配置信息
        /// </summary>
        /// <returns>2017-09-07 黄杰 创建</returns>
        public static SUNINGConfig SUNINGConfig()
        {
            return GetConfig<SUNINGConfig>("SUNING.config");
        }


        /// <summary>
        /// 获取融e购接口配置信息
        /// </summary>
        /// <returns>2018-03-19 罗熙 创建</returns>
        public static RongEgouConfig GetRongEgouConfig()
        {
            return GetConfig<RongEgouConfig>("RongEgou.config");
        }


        /// <summary>
        /// 获取配置文件信息
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="configFileName">配置文件名称</param>
        /// <returns>配置文件信息</returns>
        /// <remarks>2013-9-4 陶辉 创建</remarks>
        private static T GetConfig<T>(string configFileName)
        {
            var file = "";

            if (HttpContext.Current != null)
            {
                file = HttpContext.Current.Server.MapPath("~/Config/") + configFileName;
            }
            else
            {
                file = AppDomain.CurrentDomain.BaseDirectory + "\\Config\\" + configFileName;
            }

            return Hyt.Util.Serialization.SerializationUtil.XmlDeserialize<T>(File.ReadAllText(file));
        }
    }
}
