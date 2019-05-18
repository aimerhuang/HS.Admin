using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Promotion
{
    /// <summary>
    /// 秒杀明细
    /// </summary>
    public class SpSpikeItem
    {
        /// <summary>
        /// 自动编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 秒杀系统编号
        /// </summary>
        public int SpikeSysNo { get; set; }
        /// <summary>
        /// 商品系统编号
        /// </summary>
        public int ProductSysNo { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 秒杀价格
        /// </summary>
        public decimal SpikeAmount { get; set; }
        /// <summary>
        /// 秒杀库存
        /// </summary>
        public int SpikeStock { get; set; }
    }
}
