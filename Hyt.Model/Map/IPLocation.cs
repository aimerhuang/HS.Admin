using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Map
{
    /// <summary>
    /// IP所在位置信息
    /// </summary>
    /// <remarks>2013-12-16 编号 创建</remarks>
    public class IPLocation
    {
        /// <summary>
        /// 国家
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// 地区
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 县/区
        /// </summary>
        public string County { get; set; }
    }
}
