
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
	public partial class RP_绩效_门店新增会员 : BaseEntity
	{
	  
		/// <summary>
		/// 
		/// </summary>
		public int 门店编号 { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public string 门店名称 { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int 新增会员总数 { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int 消费金额满30的会员数 { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal 新增会员销售 { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
        public string 统计日期 { get; set; }
 	}
}

	