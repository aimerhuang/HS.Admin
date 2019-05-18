
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
	public partial class RP_绩效_电商中心 : BaseEntity
	{
	  
		/// <summary>
		/// 
		/// </summary>
        
        public int 分销商商城编号 { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public string 分销商 { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal 升舱金额_百城达 { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public decimal 升舱金额_第三方 { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
        public int 升舱单量_百城达 { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
		public int 升舱单量_第三方 { get; set; }
 
		/// <summary>
		/// 
		/// </summary>
        public string 统计日期 { get; set; }

        public string 商城类型 { get; set; }

        public string 店铺名称 { get; set; }
 	}
}

	