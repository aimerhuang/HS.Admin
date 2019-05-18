using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 充值记录拓展
    /// </summary>
    /// <remarks> 2016-08-17 刘伟豪 创建</remarks>
    [Serializable]
    public class CBCrRecharge : CrRecharge
    {
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 用户手机号
        /// </summary>
        public string MobilePhoneNumber { get; set; }
        /// <summary>
        /// 用户账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 账户余额
        /// </summary>
        public decimal AvailableBalance { get; set; }
    }
}