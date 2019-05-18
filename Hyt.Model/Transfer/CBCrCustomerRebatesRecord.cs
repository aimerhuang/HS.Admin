using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    public class CBCrCustomerRebatesRecord : CrCustomerRebatesRecord
    {
        /// <summary>
        /// 推荐人账号
        /// </summary>
        /// <remarks>2015-11-04 王耀发 创建</remarks> 
        public string RecommendAccount { get; set; }

        /// <summary>
        /// 推荐人名称
        /// </summary>
        /// <remarks>2015-11-04 王耀发 创建</remarks> 
        public string RecommendName { get; set; }

        /// <summary>
        /// 消费者账号
        /// </summary>
        /// <remarks>2015-11-04 王耀发 创建</remarks> 
        public string ComplyAccount { get; set; }

        /// <summary>
        /// 消费者名称
        /// </summary>
        /// <remarks>2015-11-04 王耀发 创建</remarks> 
        public string ComplyName { get; set; }

        /// <summary>
        /// 所属分销商
        /// </summary>
        /// <remarks>2015-12-21 王耀发 创建</remarks> 
        public string RDealerName { get; set; }

        public string WarehouseName { get; set; }

        public string FreightAmount { get; set; }
        public string CouponAmount { get; set; }
        public string ProductAmount { get; set; }
        public string ProductChangeAmount { get; set; }
        public string OrderAmount { get; set; }
    }
}
