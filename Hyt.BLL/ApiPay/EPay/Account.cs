using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.ApiPay.EPay
{
    /// <summary>
    /// 易票联账号信息
    /// </summary>
    /// <remarks>2017-2-8 杨浩 创建</remarks>
    public class Account : IAccount
    {
        /// <summary>
        /// 包序列号
        /// </summary>
        public string PackId { get; set; }
        /// <summary>
        /// 注册账号
        /// </summary>
        public string RegNo { get; set; }
        /// <summary>
        /// 注册密码
        /// </summary>
        public string Pwd { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 账号类型(固定填写 01 ，表示 UMPSUMPS 账号)
        /// </summary>
        public string AccType { get; set; }
        /// <summary>
        /// 证件类型
        /// 00 ：身份证 01 ：军官证 02 ：护照 03 ：入境证 04 ：临时身份证 05 ：营业执照 06 ：组织机构代码证 08 ：返乡证 99：其它
        /// </summary>
        public string CertType { get; set; }
        /// <summary>
        /// 证件号码
        /// </summary>
        public string CertId { get; set; }
        /// <summary>
        /// 联系地址
        /// </summary>
        public string Addr { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string ContactMan { get; set; }
        /// <summary>
        /// 邮政编码
        /// </summary>
        public string ZipCode { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string TelNo { get; set; }
        /// <summary>
        /// 移动电话
        /// </summary>
        public string MobileNo { get; set; }
        /// <summary>
        /// 传真
        /// </summary>
        public string FaxNo { get; set; }
        /// <summary>
        /// 电邮
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Memo { get; set; }
        /// <summary>
        /// 终端编号
        /// </summary>
        public string TermNo { get; set; }
        /// <summary>
        /// 请求时间
        /// </summary>
        public string ReqTime { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string MobNo { get; set; }
        /// <summary>
        /// 校验码
        /// </summary>
        public string Mac { get; set; }
    }
}
