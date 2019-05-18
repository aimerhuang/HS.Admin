
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
	public partial class BsCode
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 父级系统编号
		/// </summary>
		[Description("父级系统编号")]
		public int ParentSysNo { get; set; }
 		/// <summary>
		/// 代码名称
		/// </summary>
		[Description("代码名称")]
		public string CodeName { get; set; }
 		/// <summary>
		/// 状态:有效(1),无效(0)
		/// </summary>
		[Description("状态:有效(1),无效(0)")]
		public int Status { get; set; }
 	}
}

	