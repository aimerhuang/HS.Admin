
using System;
namespace Hyt.Model
{
    /// <summary>
	/// 升舱禁止商品
	/// </summary>
    /// <remarks>
    /// 2014-03-21 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class DsForbidProduct : BaseEntity
	{
	  
		/// <summary>
		/// 系统编号
		/// </summary>
		public int SysNo { get; set; }
 
		/// <summary>
		/// 产品系统编号
		/// </summary>
		public int ProductSysNo { get; set; }
 
		/// <summary>
		/// 商品编号
		/// </summary>
		public string ProductErpCode { get; set; }
 
		/// <summary>
		/// 创建人
		/// </summary>
		public int CreatedBy { get; set; }
 
		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime CreatedDate { get; set; }
 	}
}

	