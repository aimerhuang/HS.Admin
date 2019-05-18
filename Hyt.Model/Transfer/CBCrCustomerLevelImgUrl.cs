using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 会员等级图标
    /// </summary>
    [Serializable]
    public class CBCrCustomerLevelImgUrl:CrCustomerLevel
    {
        /// <summary>
        /// 会员等级图标
        /// </summary>
        public string CustomerLevelImgUrl { get; set; }
    }
}
