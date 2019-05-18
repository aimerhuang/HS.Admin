using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Generated
{
    public class SoChangeOrderPrice
    {
        /// <summary>
        /// 自动编号
        /// </summary>
        public int SysNo { get; set; }
        /// <summary>
        /// 订编号
        /// </summary>
        public int OrderSysNo { get; set; }
        /// <summary>
        /// 税费
        /// </summary>
        public decimal TaxFee { get; set; }
        /// <summary>
        /// 物流费
        /// </summary>
        public decimal FreightAmount { get; set; }
        /// <summary>
        /// 商品优惠
        /// </summary>
        public decimal ProductDiscountAmount { get; set; }
        /// <summary>
        /// 订单优惠
        /// </summary>
        public decimal OrderDiscountAmount { get; set; }
        /// <summary>
        /// 创建者
        /// </summary>
        public int Createby { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CraeteDate { get; set; }
        /// <summary>
        /// 审核值
        /// </summary>
        public int AuditBy { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime AuditDate { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Dis { get; set; }
    }
}
