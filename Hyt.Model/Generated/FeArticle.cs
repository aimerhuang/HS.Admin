
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
	[Serializable]
    [DataContract]
	public partial class FeArticle
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
		public int CategorySysNo { get; set; }
 		/// <summary>
		/// 文章标题
		/// </summary>
		[Description("文章标题")]
        [DataMember]
		public string Title { get; set; }
 		/// <summary>
		/// 文章内容
		/// </summary>
		[Description("文章内容")]
        [DataMember]
		public string Content { get; set; }
 		/// <summary>
		/// 状态：待审（10）、已审（20）、作废（－10）
		/// </summary>
		[Description("状态：待审（10）、已审（20）、作废（－10）")]
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
        /// 分销商系统编号
        /// </summary>
        [Description("分销商系统编号")]
        public int DealerSysNo { get; set; }
 	}
}

	