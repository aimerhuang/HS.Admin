
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 组织机构仓库
	/// </summary>
    /// <remarks>
    /// 2013-10-08 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class BsOrganizationWarehouse
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 组织机构系统编号
		/// </summary>
		[Description("组织机构系统编号")]
		public int OrganizationSysNo { get; set; }
 		/// <summary>
		/// 仓库系统编号
		/// </summary>
		[Description("仓库系统编号")]
		public int WarehouseSysNo { get; set; }
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
 	}
}

	