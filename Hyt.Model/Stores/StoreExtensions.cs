using System;
using System.ComponentModel;

namespace Hyt.Model
{
    /// <summary>
    /// 经销商扩展属性
    /// </summary>
    /// <remarks>2016-2-22 刘伟豪 创建</remarks>
    [Serializable]
    public partial class StoreExtensions
    {
        /// <summary>
        /// 微信端图标
        /// </summary>
        /// <remarks>2016-2-22 刘伟豪 创建</remarks>
        [Description("微信端图标")]
        public string WxLogoUrl { get; set; }
        /// <summary>
        /// 客服代码
        /// </summary>
        /// <remarks>2016-6-29 杨浩 创建</remarks>
        public string CustomerServiceCode { get; set; }

    }
}