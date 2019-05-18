
using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Hyt.Model
{
    /// <summary>
	/// 
	/// </summary>
    /// <remarks>
    /// 2013-08-27 杨浩 T4生成
    /// </remarks>
	[Serializable]
    [DataContract]
	public partial class CrReceiveAddress
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
        [DataMember]
		public int SysNo { get; set; }
 		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
        [DataMember]
		public int AreaSysNo { get; set; }
 		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
        [DataMember]
		public int CustomerSysNo { get; set; }
 		/// <summary>
		/// 地址标题
		/// </summary>
		[Description("地址标题")]
        [DataMember]
		public string Title { get; set; }
 		/// <summary>
		/// 姓名
		/// </summary>
		[Description("姓名")]
        [DataMember]
		public string Name { get; set; }
 		/// <summary>
		/// 性别：保密（0）、男（1）、女（2）
		/// </summary>
		[Description("性别：保密（0）、男（1）、女（2）")]
		public int Gender { get; set; }
 		/// <summary>
		/// 座机号码
		/// </summary>
		[Description("座机号码")]
        [DataMember]
		public string PhoneNumber { get; set; }
 		/// <summary>
		/// 手机号码
		/// </summary>
		[Description("手机号码")]
        [DataMember]
		public string MobilePhoneNumber { get; set; }
 		/// <summary>
		/// 传真号码
		/// </summary>
		[Description("传真号码")]
        [DataMember]
		public string FaxNumber { get; set; }
 		/// <summary>
		/// 电子邮箱
		/// </summary>
		[Description("电子邮箱")]
        [DataMember]
		public string EmailAddress { get; set; }
 		/// <summary>
		/// 街道地址
		/// </summary>
		[Description("街道地址")]
        [DataMember]
		public string StreetAddress { get; set; }
 		/// <summary>
		/// 邮编
		/// </summary>
		[Description("邮编")]
        [DataMember]
		public string ZipCode { get; set; }
 		/// <summary>
		/// 是否默认地址：是（1）、否（0）
		/// </summary>
		[Description("是否默认地址：是（1）、否（0）")]
        [DataMember]
		public int IsDefault { get; set; }
        /// <summary>
        /// 身份证号码
        /// </summary>
        [Description("身份证号码")]
        [DataMember]
        public string IDCardNo { get; set; }
        /// <summary>
        /// 身份证图片
        /// </summary>
        [Description("身份证图片")]
        [DataMember]
        public string IDCardImgs { get; set; }
 	}
}

	