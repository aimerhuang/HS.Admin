
using System;
using System.ComponentModel;
using System.Runtime.Serialization;
namespace Hyt.Model
{
    /// <summary>
	/// 
	/// </summary>
    /// <remarks>
    /// 2013-08-27 杨浩 T4生成
    /// </remarks>
    [DataContract]
	[Serializable]
	public partial class FeAdvertItem
	{
	    /// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
        [DataMember]
		public int SysNo { get; set; }
 		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
        [DataMember]
		public int GroupSysNo { get; set; }
 		/// <summary>
		/// 广告名称
		/// </summary>
		[Description("广告名称")]
		public string Name { get; set; }
 		/// <summary>
		/// 广告内容
		/// </summary>
		[Description("广告内容")]
        [DataMember]
		public string Content { get; set; }
 		/// <summary>
		/// 广告图片Url
		/// </summary>
		[Description("广告图片Url")]
        [DataMember]
		public string ImageUrl { get; set; }
 		/// <summary>
		/// 广告链接
		/// </summary>
		[Description("广告链接")]
        [DataMember]
		public string LinkUrl { get; set; }
 		/// <summary>
		/// 广告链接提示信息
		/// </summary>
		[Description("广告链接提示信息")]
        [DataMember]
		public string LinkTitle { get; set; }
 		/// <summary>
		/// 广告打开方式
		/// </summary>
		[Description("广告打开方式")]
        [DataMember]
		public int OpenType { get; set; }
 		/// <summary>
		/// 广告开始时间
		/// </summary>
		[Description("广告开始时间")]
		public DateTime BeginDate { get; set; }
 		/// <summary>
		/// 广告结束时间
		/// </summary>
		[Description("广告结束时间")]
        [DataMember]
		public DateTime EndDate { get; set; }
 		/// <summary>
		/// 显示顺序
		/// </summary>
		[Description("显示顺序")]
        [DataMember]
		public int DisplayOrder { get; set; }
 		/// <summary>
		/// 状态：待审（10）、已审（20）、作废（－10）
		/// </summary>
		[Description("状态：待审（10）、已审（20）、作废（－10）")]
        [DataMember]
		public int Status { get; set; }
 		/// <summary>
		/// 创建人
		/// </summary>
		[Description("创建人")]
        [DataMember]
		public int CreatedBy { get; set; }
 		/// <summary>
		/// 创建时间
		/// </summary>
		[Description("创建时间")]
        [DataMember]
		public DateTime CreatedDate { get; set; }
 		/// <summary>
		/// 最后更新人
		/// </summary>
		[Description("最后更新人")]
        [DataMember]
		public int LastUpdateBy { get; set; }
 		/// <summary>
		/// 最后更新时间
		/// </summary>
		[Description("最后更新时间")]
        [DataMember]
		public DateTime LastUpdateDate { get; set; }
        /// <summary>
        /// 分销商系统编号
        /// </summary>
        [Description("分销商系统编号")]
        [DataMember]
        public int DealerSysNo { get; set; }
 	}
}

	