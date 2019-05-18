
using System;
using System.ComponentModel;
namespace Hyt.Model
{
    /// <summary>
	/// 
	/// </summary>
    /// <remarks>
    /// 2016-06-12 王耀发 T4生成
    /// </remarks>
	[Serializable]
    public partial class PdProductAttributeOption
	{
	  		/// <summary>
		/// 系统编号
		/// </summary>
		[Description("系统编号")]
		public int SysNo { get; set; }
 		/// <summary>
        /// 属性项系统编号
		/// </summary>
        [Description("属性项系统编号")]
        public int AttributeOptionSysNo { get; set; }
 		/// <summary>
        /// 商品系统编号
		/// </summary>
        [Description("商品系统编号")]
        public int ProductSysNo { get; set; }
 		/// <summary>
        /// 是否图片(0，否，1，是)
		/// </summary>
        [Description("是否图片(0，否，1，是")]
        public int IsImage { get; set; }
 		/// <summary>
        /// 属性项对应图片
		/// </summary>
        [Description("属性项对应图片")]
        public string AttributeOptionImage { get; set; }
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

	