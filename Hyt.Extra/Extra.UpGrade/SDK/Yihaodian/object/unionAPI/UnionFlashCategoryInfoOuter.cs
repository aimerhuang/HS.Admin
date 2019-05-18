using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.UnionAPI
{
	/// <summary>
	/// 名品类目
	/// </summary>
	[Serializable]
	public class UnionFlashCategoryInfoOuter 
	{
		/**类目ID */
		[XmlElement("category_id")]
			public long?  Category_id{ get; set; }

		/**类目名称 */
		[XmlElement("category_name")]
			public string  Category_name{ get; set; }

		/**类目几级 */
		[XmlElement("category_level")]
			public int?  Category_level{ get; set; }

		/**父类目ID */
		[XmlElement("category_parent_id")]
			public long?  Category_parent_id{ get; set; }

		/**排序字段 */
		[XmlElement("seq_no")]
			public int?  Seq_no{ get; set; }

		/**创建时间 */
		[XmlElement("create_time")]
			public string  Create_time{ get; set; }

		/**最近更新时间 */
		[XmlElement("update_time")]
			public string  Update_time{ get; set; }

		/**类目图片链接 */
		[XmlElement("category_img_url")]
			public string  Category_img_url{ get; set; }

	}
}
