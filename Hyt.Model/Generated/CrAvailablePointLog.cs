
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 
	/// </summary>
    /// <remarks>
    /// 2013-10-31 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class CrAvailablePointLog
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 客户系统编号
		/// </summary>
		[Description("客户系统编号")]
		public int CustomerSysNo { get; set; }
 		/// <summary>
		/// 事务编号
		/// </summary>
		[Description("事务编号")]
		public string TransactionSysNo { get; set; }
 		/// <summary>
		/// 增加积分
		/// </summary>
		[Description("增加积分")]
		public int Increased { get; set; }
 		/// <summary>
		/// 减少积分
		/// </summary>
		[Description("减少积分")]
		public int Decreased { get; set; }
 		/// <summary>
		/// 剩余积分
		/// </summary>
		[Description("剩余积分")]
		public int Surplus { get; set; }
 		/// <summary>
		/// 积分变更类型
		/// </summary>
		[Description("积分变更类型")]
		public int PointType { get; set; }
 		/// <summary>
		/// 积分变更类型：系统赠送(10),交易变更(20),参与活动(3
		/// </summary>
		[Description("积分变更类型：系统赠送(10),交易变更(20),参与活动(3")]
		public string PointDescription { get; set; }
 		/// <summary>
		/// 变更时间
		/// </summary>
		[Description("变更时间")]
		public DateTime ChangeDate { get; set; }
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

	