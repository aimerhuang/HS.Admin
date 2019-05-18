
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
	public partial class BsPaymentType
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
        [DataMember]
		public int SysNo { get; set; }
 		/// <summary>
		/// 支付名称
		/// </summary>
		[Description("支付名称")]
        [DataMember]
		public string PaymentName { get; set; }
 		/// <summary>
		/// 支付描述
		/// </summary>
		[Description("支付描述")]
		public string PaymentDescription { get; set; }
 		/// <summary>
		/// 是否网上支付：是（1）、否（0）
		/// </summary>
		[Description("是否网上支付：是（1）、否（0）")]
		public int IsOnlinePay { get; set; }
 		/// <summary>
		/// 前台是否可见：可见（1）、不可见（0）
		/// </summary>
		[Description("前台是否可见：可见（1）、不可见（0）")]
        [DataMember]
		public int IsOnlineVisible { get; set; }
 		/// <summary>
		/// 支付类型：预付（10）、到付（20）
		/// </summary>
		[Description("支付类型：预付（10）、到付（20）")]
		public int PaymentType { get; set; }
 		/// <summary>
		/// 是否需要卡号：是(1),否(0)
		/// </summary>
		[Description("是否需要卡号：是(1),否(0)")]
		public int RequiredCardNumber { get; set; }
 		/// <summary>
		/// 显示顺序
		/// </summary>
		[Description("显示顺序")]
		public int DisplayOrder { get; set; }
 		/// <summary>
		/// 状态：有效（1）、无效（0）
		/// </summary>
		[Description("状态：有效（1）、无效（0）")]
		public int Status { get; set; }
 		/// <summary>
		/// 创建人
		/// </summary>
		[Description("创建人")]
		public int CreatedBy { get; set; }
 		/// <summary>
		/// 创建时间
		/// </summary>
		[Description("创建时间")]
		public DateTime CreatedDate { get; set; }
 		/// <summary>
		/// 最后更新人
		/// </summary>
		[Description("最后更新人")]
		public int LastUpdateBy { get; set; }
 		/// <summary>
		/// 最后更新时间
		/// </summary>
		[Description("最后更新时间")]
		public DateTime LastUpdateDate { get; set; }
        
        /// <summary>
        /// 海关报关编码 数据库字段增加报关编码 BsPaymentType.sql
		/// </summary> 
		[Description("海关报关编码")]
        public string CusPaymentCode { get; set; }
 		/// <summary>
        /// 海关报关公司名称  数据库字段增加报关公司名称 BsPaymentType.sql
		/// </summary>
		[Description("海关报关公司名称")]
        public string CusPaymentName { get; set; }
 	}
}

	