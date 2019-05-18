
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// App推送服务
	/// </summary>
    /// <remarks>
    /// 2014-01-14 杨浩 T4生成
    /// </remarks>
	[Serializable]
	public partial class ApPushService
	{
	  	/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 客户端类型:Android(10),Iphone(20),全部(90)
		/// </summary>
		[Description("客户端类型:Android(10),Iphone(20),全部(90)")]
		public int AppType { get; set; }
 		/// <summary>
		/// 类型:ProductDetail(产品详情),AppBrowser(App浏览器)
		/// </summary>
		[Description("类型:ProductDetail(产品详情),AppBrowser(App浏览器)")]
		public int ServiceType { get; set; }
 		/// <summary>
		/// 参数
		/// </summary>
		[Description("参数")]
		public string Parameter { get; set; }
 		/// <summary>
		/// 客户系统编号
		/// </summary>
		[Description("客户系统编号")]
		public int CustomerSysNo { get; set; }
 		/// <summary>
		/// 标题
		/// </summary>
		[Description("标题")]
		public string Title { get; set; }
 		/// <summary>
		/// 内容
		/// </summary>
		[Description("内容")]
		public string Content { get; set; }
 		/// <summary>
		/// 状态:待审(10),已审(20),作废(-10)
		/// </summary>
		[Description("状态:待审(10),已审(20),作废(-10)")]
		public int Status { get; set; }
 		/// <summary>
		/// 开始时间
		/// </summary>
		[Description("开始时间")]
		public DateTime BeginTime { get; set; }
 		/// <summary>
		/// 结束时间
		/// </summary>
		[Description("结束时间")]
		public DateTime EndTime { get; set; }
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
 	}
}

	