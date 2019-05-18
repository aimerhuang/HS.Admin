using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 配送员信用额度实体
    /// </summary>
    /// <remarks>
    /// 2013-06-15 沈强 创建
    /// </remarks>
    public class CBLgDeliveryUserCredit :LgDeliveryUserCredit
    {
     
        /// <summary>
        /// 配送员姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 配送员所属仓库名称
        /// </summary>
        public string WareHouseName { get; set; }

        /// <summary>
        /// 配送员系统表SyUser中状态
        /// </summary>
        public int Status { get; set; }
    }
}
