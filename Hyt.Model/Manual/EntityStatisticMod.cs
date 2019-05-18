using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Manual
{
    public class EntityStatisticMod
    {
        public string SysNo { get; set; }
        /// <summary>
        /// 实体名称描述
        /// </summary>
        public string EntityName { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 销售数量
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// 销售金额
        /// </summary>
        public decimal Amount { get; set; }
    }
}
