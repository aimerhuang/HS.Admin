
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    public class CBSoReceiveAddress:SoReceiveAddress
    {
        public string ProvinceName { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }
    }

    /// <summary>
	/// 
	/// </summary>
    /// <remarks>
    /// 2013-08-27 杨浩 T4生成
    /// </remarks>
	public partial class SoReceiveAddress
	{
	  	/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 名称
		/// </summary>
		[Description("名称")]
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
		public string PhoneNumber { get; set; }
 		/// <summary>
		/// 手机号码
		/// </summary>
		[Description("手机号码")]
		public string MobilePhoneNumber { get; set; }
 		/// <summary>
		/// 传真号码
		/// </summary>
		[Description("传真号码")]
		public string FaxNumber { get; set; }
 		/// <summary>
		/// 电子邮箱
		/// </summary>
		[Description("电子邮箱")]
		public string EmailAddress { get; set; }
 		/// <summary>
		/// 地区编号
		/// </summary>
		[Description("地区编号")]
		public int AreaSysNo { get; set; }
 		/// <summary>
		/// 街道地址
		/// </summary>
		[Description("街道地址")]
		public string StreetAddress { get; set; }
 		/// <summary>
		/// 邮编
		/// </summary>
		[Description("邮编")]
		public string ZipCode { get; set; }

        private string _IDCardImgs = string.Empty;
        /// <summary>
        /// 身份证照片
        /// </summary>
        [Description("身份证照片")]
        public string IDCardImgs {
            get
            {
                return (_IDCardImgs == null ? null : _IDCardImgs.Replace("-", ""));
            } 
            set
            {
               _IDCardImgs = value;
            }
        }
        /// <summary>
        /// 身份证
        /// </summary>
        [Description("身份证")]
        public string IDCardNo { get; set; }
 	}
}

	