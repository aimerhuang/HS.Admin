using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.B2CApp
{
    /// <summary>
    /// 收货地址
    /// </summary>
    /// <remarks>2013-7-12 杨浩 添加</remarks>
    public class ReceiveAddress : CrReceiveAddress
    {
        /// <summary>
        /// 地区名
        /// </summary>
        public string AreaName { get; set; }
    }
}
