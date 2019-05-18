using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extra.Erp.Model
{

    /// <summary>
    /// 返还单明细
    /// </summary>
    /// <remarks>2015-5-4 谭显锋 创建</remarks>
    public class WhReplenishItem
    {

        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }

        /// <summary>
        /// 来源明细系统编号
        /// </summary>
        public string RelatedSysNo { get; set; }

        /// <summary>
        /// 来源单编号
        /// </summary>
        public string SourceSysNo { get; set; }

        /// <summary>
        /// 来源单据号
        /// </summary>
        public string SourceCode { get; set; }

        /// <summary>
        /// 商品编号
        /// </summary>
        public int ProductSysNo { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        ///商品 ErpCode
        /// </summary>
        public string ErpCode { get; set; }

        /// <summary>
        /// 应收客户
        /// </summary>
        public string ReceiveCustomer { get; set; }


        /// <summary>
        /// 送货客户
        /// </summary>
        public string DeliveryCustomer { get; set; }


        /// <summary>
        /// 收款客户
        /// </summary>
        public string PaymentCustomer { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 待返还数量
        /// </summary>
        public int InQuantity { get; set; }

        /// <summary>
        /// 实际返还数量
        /// </summary>
        public int AlreadInQuantity { get; set; }

        /// <summary>
        /// 状态 正常10,关闭20
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }


        /// <summary>
        /// 返还单编号
        /// </summary>
        public int WhReplenishSysNo { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public decimal UnitPrice { get; set; }


        /// <summary>
        /// 明细总金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 明细总折扣金额
        /// </summary>
        public decimal DiscountAmount { get; set; }

        /// <summary>
        /// 是否赠品
        /// </summary>
        public int IsPresent { get; set; }

    }
}
