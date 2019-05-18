
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
	public partial class LgPickupType
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 父级编号
		/// </summary>
		[Description("父级编号")]
		public int ParentSysNo { get; set; }
 		/// <summary>
		/// 取件方式名称
		/// </summary>
		[Description("取件方式名称")]
		public string PickupTypeName { get; set; }
 		/// <summary>
		/// 取件方式描述
		/// </summary>
		[Description("取件方式描述")]
		public string PickupTypeDescription { get; set; }
 		/// <summary>
		/// 取件级别(0-5级,级别越高,处理优先级越高)
		/// </summary>
		[Description("取件级别(0-5级,级别越高,处理优先级越高)")]
		public int PickupLevel { get; set; }
 		/// <summary>
		/// 取件耗时
		/// </summary>
		[Description("取件耗时")]
		public string PickupTime { get; set; }
 		/// <summary>
		/// 物流跟踪查询Url
		/// </summary>
		[Description("物流跟踪查询Url")]
		public string TraceUrl { get; set; }
 		/// <summary>
		/// 显示顺序
		/// </summary>
		[Description("显示顺序")]
		public int DisplayOrder { get; set; }
 		/// <summary>
		/// 提供商
		/// </summary>
		[Description("提供商")]
		public string Provider { get; set; }
 		/// <summary>
		/// 前台是否可见：可见（1）、不可见（0）
		/// </summary>
		[Description("前台是否可见：可见（1）、不可见（0）")]
		public int IsOnlineVisible { get; set; }
 		/// <summary>
		/// 运费
		/// </summary>
		[Description("运费")]
		public decimal Freight { get; set; }
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
 	}
}

	