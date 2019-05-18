using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Manual
{
    /// <summary>
    /// 订单收货地址
    /// </summary>
    /// <remarks>2015-10-16 杨云奕 添加</remarks>
    public class SoReceiveAddressMod : SoReceiveAddress
    {
        /// <summary>
        /// 国家
        /// </summary>
        public string ReceiverCountry { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        public string ReceiverProvince { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string ReceiverCity { get; set; }
        /// <summary>
        /// 县区
        /// </summary>
        public string ReceiverArea { get; set; }
    }
}
