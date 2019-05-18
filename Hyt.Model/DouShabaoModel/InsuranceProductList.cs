using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.DouShabaoModel
{
    /// <summary>
    /// 豆沙包产品
    /// </summary>
    /// <remarks>2017-7-07 罗熙 创建</remarks>
    public class InsuranceProductList
    {
        /// <summary>
        /// 产品ID（咨询豆沙包）
        /// </summary>
        public long InsuranceProductId { get; set; }
        /// <summary>
        /// 保险金额
        /// </summary>
        public string InsuranceProductAmount { get; set; }
        /// <summary>
        /// 有效期
        /// </summary>
        public int InsuranceProductPeriod { get; set; }
        /// <summary>
        /// 保费
        /// </summary>
        public int InsuranceProductPremium { get; set; }
    }
}
