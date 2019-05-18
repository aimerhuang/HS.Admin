using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.Model.LogisApp
{
    /// <summary>
    /// 用于App接口的收货地址对象
    /// </summary>
    /// <remarks>2013-12-01 沈强 创建</remarks>
    public class AppSoReceiveAddress
    {

        /// <summary>
        /// 系统编号
        /// </summary>
        public int SysNo { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 性别：保密（0）、男（1）、女（2）
        /// </summary>
        public int Gender { get; set; }

        /// <summary>
        /// 座机号码
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string MobilePhoneNumber { get; set; }

        /// <summary>
        /// 传真号码
        /// </summary>
        public string FaxNumber { get; set; }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// 地区编号
        /// </summary>
        public int AreaSysNo { get; set; }

        /// <summary>
        /// 街道地址
        /// </summary>
        public string StreetAddress { get; set; }

        /// <summary>
        /// 邮编
        /// </summary>
        public string ZipCode { get; set; }
    }
}
