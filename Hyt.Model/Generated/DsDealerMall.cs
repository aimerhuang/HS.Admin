
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 分销商商城
	/// </summary>
    /// <remarks>
    /// 2013-09-18 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class DsDealerMall
	{
	    /// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 分销商系统编号
		/// </summary>
		[Description("分销商系统编号")]
		public int DealerSysNo { get; set; }
 		/// <summary>
		/// 分销商城类型系统编号
		/// </summary>
		[Description("分销商城类型系统编号")]
		public int MallTypeSysNo { get; set; }
 		/// <summary>
		/// 店铺名称
		/// </summary>
		[Description("店铺名称")]
		public string ShopName { get; set; }
 		/// <summary>
		/// 店铺账号
		/// </summary>
		[Description("店铺账号")]
		public string ShopAccount { get; set; }
 		/// <summary>
		/// 状态:启用(1),禁用(0)
		/// </summary>
		[Description("状态:启用(1),禁用(0)")]
		public int Status { get; set; }
        /// <summary>
        /// 授权码
        /// </summary>
        [Description("授权码")]
        public string AuthCode { get; set; }
 		/// <summary>
		/// 是否自营 
		/// </summary>
		[Description("是否自营 ")]
		public int IsSelfSupport { get; set; }
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
        /// <summary>
        /// 售后电话
        /// </summary>
        [Description("售后电话")]
        public string ServicePhone { get; set; }
        /// <summary>
        /// APPKEY系统编号
        /// </summary>
        /// 2014-07-24 余勇 APPKEY系统编号
        [Description("APPKEY系统编号")]
        public int DealerAppSysNo { get; set; }

        /// <summary>
        /// erp系统编号
        /// </summary>
        /// 2017-05-6 罗勤尧 添加erp系统编号
        [Description("erp系统编号")]
        public string ErpSysNo { get; set; }
        /// <summary>
        /// 默认仓库
        /// </summary>
        public string DefaultWarehouse { set; get; }
	}
}

	