using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 调货单列表
    /// </summary>
    /// <remarks>2016-04-05 谭显锋 创建</remarks>
    [Serializable]
    public class CBTransferCargo : TransferCargo
    {
        /// <summary>
        /// 申请人
        /// </summary>
        public string CreatedName { get; set; }

        /// <summary>
        /// 处理人
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// 申请仓库名称
        /// </summary>
        public string ApplyWarehouse { get; set; }

        /// <summary>
        /// 申请仓库街道地址
        /// </summary>
        public string StreetAddress { get; set; }

        /// <summary>
        /// 申请仓库联系人
        /// </summary>
        public string Contact { get; set; }

        /// <summary>
        /// 申请仓库联系方式
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 配货仓库名称
        /// </summary>
        public string DeliveryWarehouse { get; set; }
    }
}
