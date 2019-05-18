
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
	public partial class CrCustomerQuestion
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
		public int CustomerSysNo { get; set; }
 		/// <summary>
		/// 商品编号
		/// </summary>
		[Description("商品编号")]
        [DataMember]
		public int ProductSysNo { get; set; }
 		/// <summary>
		/// 咨询类型：商品（10）、支付（20）、配送（30）、其它
		/// </summary>
		[Description("咨询类型：商品（10）、支付（20）、配送（30）、其它")]
        [DataMember]
		public int QuestionType { get; set; }
 		/// <summary>
		/// 咨询内容
		/// </summary>
		[Description("咨询内容")]
        [DataMember]
		public string Question { get; set; }
 		/// <summary>
		/// 咨询时间
		/// </summary>
		[Description("咨询时间")]
        [DataMember]
		public DateTime QuestionDate { get; set; }
 		/// <summary>
		/// 回复人
		/// </summary>
		[Description("回复人")]
        [DataMember]
		public int AnswerSysNo { get; set; }
 		/// <summary>
		/// 回复内容
		/// </summary>
		[Description("回复内容")]
        [DataMember]
		public string Answer { get; set; }
 		/// <summary>
		/// 回复时间
		/// </summary>
		[Description("回复时间")]
        [DataMember]
		public DateTime AnswerDate { get; set; }
 		/// <summary>
		/// 状态：待回复（10）、已回复（20）、作废（－10）
		/// </summary>
		[Description("状态：待回复（10）、已回复（20）、作废（－10）")]
        [DataMember]
		public int Status { get; set; }
 	}
}

	