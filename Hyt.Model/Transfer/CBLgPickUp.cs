using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 取件单查询结果实体
    /// </summary>
    /// <remarks>
    /// 2013-07-04 郑荣华 创建
    /// </remarks>
    public class CBLgPickUp : LgPickUp
    {
        /// <summary>
        /// 座机号码
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string MobilePhoneNumber { get; set; }

        /// <summary>
        /// 街道地址
        /// </summary>
        public string StreetAddress { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WarehouseName { get; set; }

        /// <summary>
        /// 取件方式
        /// </summary>
        public string PiceupTypeName { get; set; }
    }
}
