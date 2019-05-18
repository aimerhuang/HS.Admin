using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Hyt.Model.LogisApp
{
    /// <summary>
    /// APP使用会员信息扩展
    /// </summary>
    /// <remarks>2013-07-11 周唐炬 创建</remarks>
    [DataContract]
    public class AppCBCrCustomer
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [DataMember]
        public int SysNo { get; set; }

        ///// <summary>
        ///// 用户账号
        ///// </summary>
        //[DataMember]
        //public string Account { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        [DataMember]
        public string MobilePhoneNumber { get; set; }

        /// <summary>
        /// 手机状态：未验证（10）、已验证（20）
        /// </summary>
        [DataMember]
        public int MobilePhoneStatus { get; set; }

        /// <summary>
        /// 地区编号
        /// </summary>
        [DataMember]
        public int AreaSysNo { get; set; }

        /// <summary>
        /// 街道地址
        /// </summary>
        [DataMember]
        public string StreetAddress { get; set; }

        /// <summary>
        /// 邮箱状态：未验证（10）、已验证（20）
        /// </summary>
        [DataMember]
        public int EmailStatus { get; set; }

        /// <summary>
        /// 注册来源：PC网站（10）、信营全球购B2B2C3G网站（20）
        /// </summary>
        [DataMember]
        public int RegisterSource { get; set; }

        /// <summary>
        /// 邮编
        /// </summary>
        [DataMember]
        public string ZipCode { get; set; }

        /// <summary>
        /// 默认收货地址
        /// </summary>
        [DataMember]
        public AppCrReceiveAddress DefaultAddress { get; set; }
    }
}
