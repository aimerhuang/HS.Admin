using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 会员积分表
    /// </summary>
    /// <remarks>
    /// 2013-06-17 杨文兵 创建
    /// </remarks>
    public class CustomerScore
    {
        /// <summary>
        /// 会员系统编号
        /// </summary>
        public int CustomerSysNo { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public int Score { get; set; }
    }
}
