using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Generated
{
    /// <summary>
    /// 提现返利记录关联
    /// </summary>
    /// <remarks>2016-1-9 杨浩 创建</remarks>
    public class CrPredepositCashRebatesRecordAssociation
    {
        /// <summary>
        /// 客户系统编号
        /// </summary>
        public int CustomerSysNo { get; set; }
        /// <summary>
        /// 返利记录系统编号
        /// </summary>
        public string CrCustomerRebatesRecordSysNos { get; set; }
        /// <summary>
        /// 提现订单系统编号
        /// </summary>
        public int CrPredepositCashSysNo { get; set; }
    }
}
