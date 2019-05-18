
using System;
namespace Hyt.Model
{
    /// <summary>
	/// 
	/// </summary>
    /// <remarks>
    /// 2013-12-10 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class RP_绩效_客服 : BaseEntity
	{
	  
		/// <summary>
		/// 
		/// </summary>
		public int 客服编号 { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
        public string 客服名 { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int 单量 { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal 订单金额 { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int 新增会员 { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public string 统计日期 { get; set; }
 	}
}

	