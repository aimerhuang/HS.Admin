using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hyt.Model.Generated
{
    /// <summary>
    /// 经销商利嘉模板新增
    /// </summary>
    /// <remarks>
    /// 2017-05-18 罗勤尧 生成
    /// </remarks>
    public partial class DsDealerLiJia
    {   
        /// <summary>
        /// 分销商名称
        /// </summary>
        [Description("分销商名称")]
        public string MemberName { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        [Description("联系人")]
        public string Contact { get; set; }
        /// <summary>
        /// 联系人座机号码
        /// </summary>
        [Description("联系人座机号码")]
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 联系人手机号码
        /// </summary>
        [Description("联系人手机号码")]
        public string CellPhone { get; set; }
        /// <summary>
        /// 传真
        /// </summary>
        public string Fax { get; set; }
        /// <summary>
        /// 联系人邮箱
        /// </summary>
        [Description("联系人邮箱")]
        public string Email { get; set; }
        /// <summary>
        /// 省
        /// </summary>
        [Description("省")]
        public string Province { get; set; }
        /// <summary>
        /// 市
        /// </summary>
        [Description("市")]
        public string City { get; set; }
        /// <summary>
        /// 区
        /// </summary>
        [Description("区")]
        public string District { get; set; }
        /// <summary>
        /// 街道地址
        /// </summary>
        [Description("街道地址")]
        public string AddressLine { get; set; }
        /// <summary>
        /// 银行帐号
        /// </summary>
        [Description("银行帐号")]
        public string BankAccount { get; set; }
        
        /// <summary>
        /// QQ
        /// </summary>
        [Description("QQ")]
        public string QQ { get; set; }
        /// <summary>
        /// 用户名(必须唯一,可以跟手机号一样)
        /// </summary>
        [Description("用户名")]
        public string UserName { get; set; }
       
    }
}

