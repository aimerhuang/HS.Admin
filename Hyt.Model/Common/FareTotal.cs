using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Common
{
    /// <summary>
    /// 运费统计
    /// </summary>
    /// <remarks>2015-9-10 杨浩 创建</remarks>
    public class FareTotal
    {
        /// <summary>
        /// 物流方式（快递，EMS,平邮）
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 运费总额
        /// </summary>
        public decimal Freigh { get; set; }

        /// <summary>
        /// 物流编号
        /// </summary>
        public int DeliveryTypeSysNo { get; set; }
    }
}
