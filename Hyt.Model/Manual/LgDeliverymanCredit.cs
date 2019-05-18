using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 配送员信用额度实体
    /// </summary>
    /// <remarks>
    /// 2013-06-09 沈强 创建
    /// </remarks>
    public partial class LgDeliverymanCredit
    {
        /// <summary>
        /// 配送员信息
        /// </summary>
        /// <remarks>2013-06-09 沈强 创建</remarks>
        public SyUser User { get; set; }

        /// <summary>
        /// 用户仓库对应表
        /// </summary>
        /// <remarks>2013-06-15 沈强 创建</remarks>
        public SyUserWarehouse SystemUserWarehouse { get; set; }
    }
}
