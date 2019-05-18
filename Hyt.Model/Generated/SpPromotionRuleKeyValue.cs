
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 
	/// </summary>
    /// <remarks>
    /// 2013-08-27 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class SpPromotionRuleKeyValue
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 促销系统编号
		/// </summary>
		[Description("促销系统编号")]
		public int PromotionSysNo { get; set; }
 		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public string RuleKey { get; set; }
 		/// <summary>
		/// 
		/// </summary>
		[Description("")]
		public string RuleValue { get; set; }
 		/// <summary>
		/// 键值描述
		/// </summary>
		[Description("键值描述")]
		public string Description { get; set; }
 	}
}

	