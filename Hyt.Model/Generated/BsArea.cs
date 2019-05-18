
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
	public partial class BsArea
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
        [DataMember]
		public int SysNo { get; set; }
 		/// <summary>
		/// 父级地区编号
		/// </summary>
		[Description("父级地区编号")]
        [DataMember]
		public int ParentSysNo { get; set; }
 		/// <summary>
		/// 地区名称
		/// </summary>
		[Description("地区名称")]
        [DataMember]
		public string AreaName { get; set; }
 		/// <summary>
		/// 地区编码
		/// </summary>
		[Description("地区编码")]
        [DataMember]
		public string AreaCode { get; set; }
 		/// <summary>
		/// 名称拼音
		/// </summary>
		[Description("名称拼音")]
        [DataMember]
		public string NameAcronym { get; set; }
 		/// <summary>
		/// 显示顺序
		/// </summary>
		[Description("显示顺序")]
        [DataMember]
		public int DisplayOrder { get; set; }
 		/// <summary>
		/// 地区级别:省(1)、市(2)、区(3)
		/// </summary>
		[Description("地区级别:省(1)、市(2)、区(3)")]
        [DataMember]
		public int AreaLevel { get; set; }
 		/// <summary>
		/// 状态:有效(1)、无效(0)
		/// </summary>
		[Description("状态:有效(1)、无效(0)")]
        [DataMember]
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
 	}
}

	