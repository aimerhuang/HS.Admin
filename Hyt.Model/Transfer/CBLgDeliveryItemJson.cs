using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 用于对页面json数据传递
    /// </summary>
    /// <remarks>2013-07-05 沈强 创建</remarks>
    public class CBLgDeliveryItemJson
    {
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
        public string StockOutAmount { get; set; }

        /// <summary>
        /// 应收金额
        /// </summary>
        public string Receivable { get; set; }

        /// <summary>
        /// 收获地址
        /// </summary>
        public int AddressSysNo { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreatedDate { get; set; }

        /// <summary>
        /// 收货人姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 收货人手机号码
        /// </summary>
        public string MobilePhoneNumber { get; set; }

        /// <summary>
        /// 快递单号
        /// </summary>
        public string ExpressNo { get; set; }

        /// <summary>
        /// 是否为第三方快递 (是[1]、否[0])
        /// </summary>
        public int DeliveryType { get; set; }

        /// <summary>
        /// 取件单总金额
        /// </summary>
        public string TotalAmount { get; set; }
    }
}
