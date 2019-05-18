using System;
using System.ComponentModel;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 分销商退换货单
    /// </summary>
    /// <remarks>
    /// 2013-09-10 黄志勇 创建
    /// </remarks>
    [Serializable]
    public class CBDsReturn : DsReturn
    {
        /// <summary>
        /// 状态
        /// </summary>
        [Description("状态")]
        public int Status { get; set; }
    }
}
