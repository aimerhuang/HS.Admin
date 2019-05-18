using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.UpGrade
{
    public class UpGradeAccount
    {
        /// <summary>
        /// 账户名
        /// </summary>
        public string ShopAccount { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string MobilePhoneNumber { get; set; }
        /// <summary>
        /// 账户余额
        /// </summary>
        public decimal AvailableAmount { get; set; }
        /// <summary>
        /// 累计充值
        /// </summary>
        public decimal TotalPrestoreAmount { get; set; }
        /// <summary>
        /// 冻结余额
        /// </summary>
        public decimal FrozenAmount { get; set; }
        /// <summary>
        /// 分销商预存款系统编号
        /// </summary>
        public int PrePaymentSysNo { get; set; }
        /// <summary>
        /// 分销商名称
        /// </summary>
        public string DealerName { get; set; }
        /// <summary>
        /// 余额提示额
        /// </summary>
        /// <remarks>2014-03-21 朱家宏 添加</remarks>
        public decimal AlertAmount { get; set; }
    }
}
