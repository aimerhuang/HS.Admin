
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
	public partial class PdAttributeGroup
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
		/// 名称(后台显示用)
		/// </summary>
		[Description("名称(后台显示用)")]
		public string BackEndName { get; set; }
 		/// <summary>
		/// 排序
		/// </summary>
		[Description("排序")]
		public int DisplayOrder { get; set; }
 		/// <summary>
		/// 状态：启用（1）、禁用（0）
		/// </summary>
		[Description("状态：启用（1）、禁用（0）")]
		public int Status { get; set; }
 	}
}

	