using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 电子面单账号查询实体
    /// </summary>
    /// <remarks>2015-10-9 王江 创建</remarks>
    public class CBLgDeliveryCompanyAccount : LgDeliveryCompanyAccount
    {
        /// <summary>
        /// 配送方式名称
        /// </summary>
        public string DeliveryTypeName { get; set; }
        /// <summary>
        /// 用户名(创建人)
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户名(更新人)
        /// </summary>
        public string UserName1 { get; set; }

    }
}
