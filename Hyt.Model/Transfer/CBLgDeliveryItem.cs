using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model
{
    /// <summary>
    /// 配送单明细查询实体类
    /// </summary>
    /// <remarks>
    /// 2013-06-17 14:46 沈强 创建
    /// </remarks>
    public class CBLgDeliveryItem
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }

        /// <summary>
        /// 配送单系统编号
        /// </summary>
        public int DeliverySysNo { get; set; }

        /// <summary>
        /// 事务编号
        /// </summary>
        public string TransactionSysNo { get; set; }

        /// <summary>
        /// 单据类型：出库单（10）、取件单（20）
        /// </summary>
        public int NoteType { get; set; }

        /// <summary>
        /// 单据编号
        /// </summary>
        public int NoteSysNo { get; set; }

        /// <summary>
        /// 是否到付：是（1）、否（0）
        /// </summary>
        public int IsCOD { get; set; }

        /// <summary>
        /// 出库单金额
        /// </summary>
        public decimal StockOutAmount { get; set; }

        /// <summary>
        /// 应收金额
        /// </summary>
        public decimal Receivable { get; set; }

        /// <summary>
        /// 支付类型：预付（10）、到付（20）
        /// </summary>
        public int PaymentType { get; set; }

        /// <summary>
        /// 支付单号
        /// </summary>
        public string PayNo { get; set; }

        /// <summary>
        /// 收获地址
        /// </summary>
        public int AddressSysNo { get; set; }

        /// <summary>
        /// 状态：待签收（10）、拒收（20）、未送达（30）、已签
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public int CreatedBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// 最后更新人
        /// </summary>
        public int LastUpdateBy { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastUpdateDate { get; set; }

        /// <summary>
        /// 收货人姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 收货人座机号码
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 收货人手机号码
        /// </summary>
        public string MobilePhoneNumber { get; set; }

        /// <summary>
        /// 收货街道地址EXPRESSNO
        /// </summary>
        public string StreetAddress { get; set; }

        /// <summary>
        /// 快递单号
        /// </summary>
        public string ExpressNo { get; set; }
    }
}
