
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    public class IEasSyncLog{ }
    /// <summary>
	/// Eas同步日志表
	/// </summary>
    /// <remarks>
    /// 2013-10-22 杨浩 T4生成
    /// </remarks>
	[Serializable]
    public partial class EasSyncLog : IEasSyncLog
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }

        /// <summary>
        /// 流程编号
        /// </summary>
        [Description("流程编号")]
        public string FlowIdentify { get; set; }

        /// <summary>
        /// 仓库系统编号
        /// </summary>
        public int WarehouseSysNo { get; set; }

        /// <summary>
        /// 流程类型:订单(10),借货(20)
        /// </summary>
        public int FlowType { get; set; }

        /// <summary>
        /// 单据号
        /// </summary>
        public string VoucherNo { get; set; }

        /// <summary>
        /// 单据金额
        /// </summary>
        public decimal VoucherAmount { get; set; }

 		/// <summary>
		/// 接口名称
		/// </summary>
		[Description("接口名称")]
		public string Name { get; set; }
 		/// <summary>
		/// 接口类型
		/// </summary>
		[Description("接口类型")]
		public int InterfaceType { get; set; }
 		/// <summary>
		/// 同步耗时(毫秒)
		/// </summary>
		[Description("同步耗时(毫秒)")]
		public int ElapsedTime { get; set; }
 		/// <summary>
		/// 同步消息
		/// </summary>
		[Description("同步消息")]
		public string Message { get; set; }
 		/// <summary>
		/// 同步数据
		/// </summary>
		[Description("同步数据")]
		public string Data { get; set; }
 		/// <summary>
        /// 数据Md5
		/// </summary>
        [Description("数据Md5")]
		public string DataMd5 { get; set; }
 		/// <summary>
		/// 备注
		/// </summary>
		[Description("备注")]
		public string Remarks { get; set; }
 		/// <summary>
		/// Eas状态代码
		/// </summary>
		[Description("Eas状态代码")]
		public string StatusCode { get; set; }
 		/// <summary>
		/// 状态:成功(1),失败(0)
		/// </summary>
		[Description("状态:成功(1),失败(0)")]
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

        public int SyncNumber { get; set; }

        public int LastupdateBy { get; set; }

        public DateTime LastupdateDate { get; set; }

        public DateTime LastsyncTime { get; set; }

 	}
}

	