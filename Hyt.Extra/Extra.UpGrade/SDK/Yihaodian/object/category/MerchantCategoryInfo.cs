using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Category
{
	/// <summary>
	/// 商家产品类别信息
	/// </summary>
	[Serializable]
	public class MerchantCategoryInfo 
	{
		/**商家类别id */
		[XmlElement("merchantCategoryId")]
			public long?  MerchantCategoryId{ get; set; }

		/**商家类别编码,号码从0000到ZZZZ */
		[XmlElement("categoryCode")]
			public string  CategoryCode{ get; set; }

		/**商家类别名称 */
		[XmlElement("categoryName")]
			public string  CategoryName{ get; set; }

		/**父商家类别id */
		[XmlElement("categoryParentId")]
			public long?  CategoryParentId{ get; set; }

		/**商家类别描述 */
		[XmlElement("categoryDescription")]
			public string  CategoryDescription{ get; set; }

		/**商家类别关键词 */
		[XmlElement("categoryKeyword")]
			public string  CategoryKeyword{ get; set; }

		/**0:no 1:yes, leaf category */
		[XmlElement("categoryIsLeaf")]
			public int?  CategoryIsLeaf{ get; set; }

		/**操作者id */
		[XmlElement("backOperatorId")]
			public string  BackOperatorId{ get; set; }

		/**类别层次 */
		[XmlElement("categoryLevel")]
			public int?  CategoryLevel{ get; set; }

		/**顺序 */
		[XmlElement("listOrder")]
			public int?  ListOrder{ get; set; }

		/**搜索名称:0-c1id-c2id-c3id-c4id:c1name-c2name-c3name-c4name */
		[XmlElement("categorySearchName")]
			public string  CategorySearchName{ get; set; }

		/**搜索标题 */
		[XmlElement("categorySeoTitle")]
			public string  CategorySeoTitle{ get; set; }

		/**搜索关键字 */
		[XmlElement("categorySeoKeyword")]
			public string  CategorySeoKeyword{ get; set; }

		/**搜索描述 */
		[XmlElement("categorySeoDescription")]
			public string  CategorySeoDescription{ get; set; }

		/**0否1是 */
		[XmlElement("isDelete")]
			public int?  IsDelete{ get; set; }

		/**论坛id */
		[XmlElement("forumId")]
			public long?  ForumId{ get; set; }

		/**是否可见 0 否 1是 */
		[XmlElement("isVisible")]
			public int?  IsVisible{ get; set; }

		/**是否导入WMS系统：0：未导入 1：已导入 */
		[XmlElement("dataExchangeFlag")]
			public int?  DataExchangeFlag{ get; set; }

		/**商家分类图片id */
		[XmlElement("picId")]
			public long?  PicId{ get; set; }

		/**商家分类图片URL */
		[XmlElement("picUrl")]
			public string  PicUrl{ get; set; }

	}
}
