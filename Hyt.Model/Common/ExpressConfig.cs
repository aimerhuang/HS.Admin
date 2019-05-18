using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Common
{
    /// <summary>
    /// 快递参数配置
    /// </summary>
    /// <remarks>2017-12-13 杨浩 创建</remarks>
    public class ExpressConfig :ConfigBase
    {
        /// <summary>
        /// 圆通
        /// </summary>
        public YT Yt { get; set; }
        /// <summary>
        /// KK
        /// </summary>
        public KK Kk { get; set; }
        /// <summary>
        /// 快递100
        /// </summary>
        public KD100 Kd100 { get; set; }

    }

    /// <summary>
    /// 圆通快递
    /// </summary>
    /// <remarks>2017-12-13 杨浩 创建</remarks>
    /// <remarks>2017-12-13 廖移凤 </remarks>
    public class YT
    {
        /// <summary>
        /// 账号
        /// </summary>
        //public string Account { get; set; }
        public string ClientId { get; set; }
        public string PartnerId { get; set; }
        public string CustomerId { get; set; }
        public string ReqURL { get; set; }
    }
    /// <summary>
    /// 圆通KK接口
    /// </summary>
    /// <remarks>2017-12-13 廖移凤 创建</remarks>
    public class KK { 
        //测试密钥
        // public string clientID = "SYM";
        // public string partnerId = "SYM";
        //正式密钥
        public string ClientID { get; set; }
        public string PartnerId { get; set; }
        //测试请求url
        //public string ReqURL { get; set; }
        //正式请求url
        public string ReqURL { get; set; }
    
    }
    /// <summary>
    /// 快递100接口
    /// </summary>
    /// <remarks>2017-12-13 廖移凤 创建</remarks>
    public class KD100 
    {

        //电商ID  
        public string Key { get; set; }
        //电商加密私钥 
        public string Secret { get; set; }
        //请求url
        public string ReqURL { get; set; }

    }
    
}
