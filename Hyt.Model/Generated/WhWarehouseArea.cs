
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
	public partial class WhWarehouseArea
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 地区系统编号
		/// </summary>
		[Description("系统编号")]
		public int AreaSysNo { get; set; }
 		/// <summary>
		/// 仓库系统编号
		/// </summary>
		[Description("系统编号")]
		public int WarehouseSysNo { get; set; }
 		/// <summary>
		/// 是否默认仓库:是(1),否(0)
		/// </summary>
		[Description("是否默认仓库:是(1),否(0)")]
		public int IsDefault { get; set; }
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
 	}
}

	