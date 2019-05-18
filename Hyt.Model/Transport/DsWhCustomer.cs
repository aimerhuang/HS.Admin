using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.Transport
{
    public class CBDsWhCustomer : DsWhCustomer
    {
        public string DsCode { get; set; }
        public string DsName { get; set; }
        public bool IsAllDealer { get; set; }
        public bool IsDealer { get; set; }
        public bool IsCustomer { get; set; }
        public string CountryName { get; set; }
    }
    /// <summary>
    /// 转运系统客户档案实体
    /// </summary>
    /// <remarks>
    /// 2016-5-16 杨云奕 添加
    /// </remarks>
    public class DsWhCustomer
    {
        public int SysNo { get; set; }
        /// <summary>
        /// 代理经销商客户编号
        /// </summary>
        public int DsSysNo { get; set; }
        /// <summary>
        /// 客户编码
        /// </summary>
        public string CusCode { get; set; }
        /// <summary>
        /// 客户出库编码
        /// </summary>
        public string OutCode { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public string CusName { get; set; }
        /// <summary>
        /// 客户联系人
        /// </summary>
        public string CusLinkName { get; set; }
        /// <summary>
        /// 客户联系电话
        /// </summary>
        public string CusLinkTele { get; set; }
        /// <summary>
        /// 客户区域编码
        /// </summary>
        public string CusCountryCode { get; set; }
        /// <summary>
        /// 客户邮编
        /// </summary>
        public string CusMall { get; set; }
        /// <summary>
        /// 客户地址
        /// </summary>
        public string CusAddress { get; set; }
        /// <summary>
        /// 客户电子邮件
        /// </summary>
        public string CusEmall { get; set; }
        /// <summary>
        /// 客户有效状态
        /// </summary>
        public string CusValidity { get; set; }
        /// <summary>
        /// 系统账号
        /// </summary>
        public string AssAccount { get; set; }
    }
}
