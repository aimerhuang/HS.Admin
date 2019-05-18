using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Category
{
	/// <summary>
	/// 产品类别信息
	/// </summary>
	[Serializable]
	public class CategoryInfo 
	{
		/**产品类别id */
		[XmlElement("categoryId")]
			public long?  CategoryId{ get; set; }

		/**类别名称 */
		[XmlElement("categoryName")]
			public string  CategoryName{ get; set; }

		/**是否叶子节点 0:否 1:是 */
		[XmlElement("categoryIsLeaf")]
			public int?  CategoryIsLeaf{ get; set; }

		/**类别编码 */
		[XmlElement("categoryCode")]
			public string  CategoryCode{ get; set; }

		/**父类别id */
		[XmlElement("categoryParentId")]
			public long?  CategoryParentId{ get; set; }

		/**分类描述 */
		[XmlElement("categoryDescription")]
			public string  CategoryDescription{ get; set; }

		/**分类关键字 */
		[XmlElement("categoryKeyword")]
			public string  CategoryKeyword{ get; set; }

		/**排序顺序 */
		[XmlElement("listOrder")]
			public int?  ListOrder{ get; set; }

		/**对销售价格的特殊管理人id，用逗号分割 */
		[XmlElement("backOperatorId")]
			public string  BackOperatorId{ get; set; }

		/**搜索用的冗余字段:0-c1id-c2id-c3id-c4id:c1name-c2name-c3name-c4name */
		[XmlElement("categorySearchName")]
			public string  CategorySearchName{ get; set; }

		/**论坛id */
		[XmlElement("forumId")]
			public long?  ForumId{ get; set; }

		/**是否可见 0 不可见 1可见 */
		[XmlElement("isVisible")]
			public int?  IsVisible{ get; set; }

		/**此类别对应的商品最大库存值 */
		[XmlElement("maxStockNum")]
			public long?  MaxStockNum{ get; set; }

	}
}
