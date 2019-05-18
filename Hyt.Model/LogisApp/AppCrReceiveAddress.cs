using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Hyt.Model.LogisApp
{
    /// <summary>
    /// 会员常用地址扩展
    /// </summary>
    /// <remarks>2013-07-11 周唐炬 创建</remarks>
    [DataContract]
    public class AppCrReceiveAddress
    {
        /// <summary>
        /// 系统编号
        /// </summary>
        [DataMember]
        public int SysNo { get; set; }

        /// <summary>
        /// 系统编号
        /// </summary>
        [DataMember]
        public int CustomerSysNo { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 座机号码
        /// </summary>
        [DataMember]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        [DataMember]
        public string MobilePhoneNumber { get; set; }

        /// <summary>
        /// 邮编
        /// </summary>
        [DataMember]
        public string ZipCode { get; set; }

        /// <summary>
        /// 地区编号
        /// </summary>
        [DataMember]
        public int AreaSysNo { get; set; }

        /// <summary>
        /// 省市区名称集合 符号(|)分隔
        /// </summary>
        [DataMember]
        public string AreaNameList { get; set; }

        /// <summary>
        /// 街道地址
        /// </summary>
        [DataMember]
        public string StreetAddress { get; set; }

        /// <summary>
        /// 是否默认地址：是（1）、否（0）
        /// </summary>
        [DataMember]
        public int IsDefault { get; set; }
    }
}
