
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
	public partial class CrExperiencePointLog
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
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
		/// 积分变更类型：系统赠送(10),交易获得(20),活动获得(3
		/// </summary>
		[Description("积分变更类型：系统赠送(10),交易获得(20),活动获得(3")]
		public int PointType { get; set; }
 		/// <summary>
		/// 积分变更描述
		/// </summary>
		[Description("积分变更描述")]
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

	