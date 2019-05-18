using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 分销结算单打印实体
    /// </summary>
    /// <remarks>
    /// 2013-07-16 郑荣华 创建
    /// </remarks>
    public class PrtDsSettlement : LgSettlement
    {
        /// <summary>
        /// 明细列表
        /// </summary>
        public IList<PrtDsSubSettlement> List;

        /// <summary>
        /// 应收款（出库单应收金额）合计
        /// </summary>
        public decimal ReceivableCount { get; set; }
    }

    /// <summary>
    /// 分销结算单打印明细实体
    /// </summary>
    /// <remarks>
    /// 2013-07-16 郑荣华 创建
    /// </remarks>
    public class PrtDsSubSettlement : LgSettlementItem
    {
        /// <summary>
        /// 应收款（出库单应收金额）
        /// </summary>
        public decimal Receivable { get; set; }

        /// <summary>
        /// 收货人姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 收货地区编号
        /// </summary>
        public int AreaSysNo { get; set; }

        /// <summary>
        /// 收货街道地址
        /// </summary>
        public string StreetAddress { get; set; }

        /// <summary>
        /// 收货人固定电话号码
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 收货人手机号
        /// </summary>
        public string MobilePhoneNumber { get; set; }

    }
}
