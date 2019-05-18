
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 系统配置
	/// </summary>
    /// <remarks>
    /// 2014-01-14 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class SyConfig
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 分类编号
		/// </summary>
		[Description("分类编号")]
		public int CategoryId { get; set; }
 		/// <summary>
		/// 键
		/// </summary>
		[Description("键")]
		public string Key { get; set; }
 		/// <summary>
		/// 值
		/// </summary>
		[Description("值")]
		public string Value { get; set; }
 		/// <summary>
		/// 描述
		/// </summary>
		[Description("描述")]
		public string Description { get; set; }
 		/// <summary>
		/// 创建时间
		/// </summary>
		[Description("创建时间")]
		public DateTime CreatedDate { get; set; }
 		/// <summary>
		/// 创建人
		/// </summary>
		[Description("创建人")]
		public int CreatedBy { get; set; }
 		/// <summary>
		/// 最后更新时间
		/// </summary>
		[Description("最后更新时间")]
		public DateTime LastUpdateDate { get; set; }
 		/// <summary>
		/// 最后更新人
		/// </summary>
		[Description("最后更新人")]
		public int LastUpdateBy { get; set; }
 	}
}

	