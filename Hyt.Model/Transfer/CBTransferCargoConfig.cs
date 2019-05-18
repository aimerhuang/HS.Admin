using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    public class CBTransferCargoConfig : TransferCargoConfig
    {
        /// <summary>
        /// 申请调货仓库名称
        /// </summary>
        public string ApplyWarehouseName { get; set; }

        /// <summary>
        /// 配货仓库名称
        /// </summary>
        public string DeliveryWarehouseName { get; set; }

        /// <summary>
        /// 配货仓库名称
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 配货仓库名称
        /// </summary>
        public string UpdateUserName { get; set; }
    }
}
