using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transfer
{
    /// <summary>
    /// 企业用户信息
    /// </summary>
    /// <remarks>2014-10-15 谭显锋 创建</remarks>
    [Serializable]
    public class CBEnterpriseUser
    {

        /// <summary>
        /// 用户ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 企业编号
        /// </summary>
        public int EnterpriseNO { get; set; }
        /// <summary>
        /// 企业名称
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// 帐号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string EmailAddress { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public string MobilePhoneNumber { get; set; }
        /// <summary>
        /// 注册日期
        /// </summary>
        public DateTime RegisterDate { get; set; }
        /// <summary>
        ///     状态:有效(1),无效(0)
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 是否可关联（0：是 1：否）
        /// </summary>
        public bool IsAdd { get;set; }
    }
}
