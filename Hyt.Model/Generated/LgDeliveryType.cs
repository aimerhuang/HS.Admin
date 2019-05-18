
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
	public partial class LgDeliveryType
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
		/// 父级编号
		/// </summary>
		[Description("父级编号")]
		public int ParentSysNo { get; set; }
 		/// <summary>
		/// 配送方式名称
		/// </summary>
		[Description("配送方式名称")]
		public string DeliveryTypeName { get; set; }
 		/// <summary>
		/// 配送方式描述
		/// </summary>
		[Description("配送方式描述")]
		public string DeliveryTypeDescription { get; set; }
 		/// <summary>
		/// 配送级别(0-5级,级别越高,处理优先级越高)
		/// </summary>
		[Description("配送级别(0-5级,级别越高,处理优先级越高)")]
		public int DeliveryLevel { get; set; }
 		/// <summary>
		/// 配送耗时
		/// </summary>
		[Description("配送耗时")]
		public string DeliveryTime { get; set; }
 		/// <summary>
		/// 物流跟踪查询Url
		/// </summary>
		[Description("物流跟踪查询Url")]
		public string TraceUrl { get; set; }
 		/// <summary>
		/// 显示顺序
		/// </summary>
		[Description("显示顺序")]
		public int DisplayOrder { get; set; }
 		/// <summary>
		/// 提供商
		/// </summary>
		[Description("提供商")]
		public string Provider { get; set; }
 		/// <summary>
		/// 前台是否可见：可见（1）、不可见（0）
		/// </summary>
		[Description("前台是否可见：可见（1）、不可见（0）")]
		public int IsOnlineVisible { get; set; }
 		/// <summary>
		/// 运费
		/// </summary>
		[Description("运费")]
		public decimal Freight { get; set; }
 		/// <summary>
		/// 是否是三方快递：是(1),否(0)
		/// </summary>
		[Description("是否是三方快递：是(1),否(0)")]
		public int IsThirdPartyExpress { get; set; }
        /// <summary>
        /// 物流
        /// </summary>
        [Description("物流")]
        public int Logistics { get; set; }
 		/// <summary>
		/// 状态：有效（1）、无效（0）
		/// </summary>
		[Description("状态：有效（1）、无效（0）")]
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
        /// 快递公司编号
        /// </summary>
        [Description("快递公司编号")]
        public string OverseaCarrier { get; set; }

        /// <summary>
        /// 商检备案编号
        /// </summary>
        [Description("商检备案编号")]
        public string CIQLogisticsNo { get; set; }
        /// <summary>
        /// 商检备案名称
        /// </summary>
        [Description("商检备案名称")]
        public string CIQLogisticsName { get; set; }
        /// <summary>
        /// 海关备案编号
        /// </summary>
        [Description("海关备案编号")]
        public string CUSLogisticsNo { get; set; }
        /// <summary>
        /// 海关备案名称
        /// </summary>
        [Description("海关备案名称")]
        public string CUSLogisticsName { get; set; }
        
        
        /// <summary>
        /// API账号
        /// </summary>
        [Description("API账号")]
        public string APIAccount { get; set; }
        /// <summary>
        /// APIKEY
        /// </summary>
        [Description("APIKEY")]
        public string APIKey { get; set; }
        /// <summary>
        /// API月结账号
        /// </summary>
        [Description("API月结账号")]
        public string APIMouthNumber { get; set; }
 	}
}

	