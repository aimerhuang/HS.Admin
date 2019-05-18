
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 分销商城地区关联表
	/// </summary>
    /// <remarks>
    /// 2014-05-05 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class DsMallAreaAssociation
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 分销商系统编号
		/// </summary>
		[Description("分销商系统编号")]
		public int DealerSysNo { get; set; }
 		/// <summary>
		/// 商城类型系统编号
		/// </summary>
		[Description("商城类型系统编号")]
		public int MallTypeSysNo { get; set; }
 		/// <summary>
		/// 商城地区名称
		/// </summary>
		[Description("商城地区名称")]
		public string MallAreaName { get; set; }
 		/// <summary>
		/// 商城地区编码
		/// </summary>
		[Description("商城地区编码")]
		public string MallAreaCode { get; set; }
 		/// <summary>
		/// 商城地区系统编号
		/// </summary>
		[Description("商城地区系统编号")]
		public int HytAreaSysNo { get; set; }
 	}
}

	