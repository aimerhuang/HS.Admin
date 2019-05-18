
using System;
namespace Hyt.Model
{
    /// <summary>
	/// 锁定任务
	/// </summary>
    /// <remarks>
    /// 2014-02-28 杨浩 T4生成
    /// </remarks>
	[Serializable]
    public partial class SyLockJob : BaseEntity
	{
	  
		/// <summary>
        /// 任务池系统编号
		/// </summary>
        public int JobPoolSysNo { get; set; }
 
		/// <summary>
        /// 解锁时间
		/// </summary>
        public DateTime UnLockDate { get; set; }
 
		/// <summary>
        /// 解锁状态（1、自动解锁  0、非自动解锁）
		/// </summary>
        public int UnLockState { get; set; }
 
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

	