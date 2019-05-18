using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Itemcats
{
	/// <summary>
	/// 类目信息
	/// </summary>
	[Serializable]
	public class ItemCat 
	{
		/**Feature对象列表 目前已有的属性： 若Attr_key为 udsaleprop，attr_value为1 则允许卖家在改类目新增自定义销售属性,不然为不允许（暂不提供） */
		[XmlElement("features")]
		public FeatureList  Features{ get; set; }

		/**商品所属类目ID */
		[XmlElement("cid")]
			public long?  Cid{ get; set; }

		/**父类目ID=0时，代表的是一级的类目 */
		[XmlElement("parent_cid")]
			public long?  Parent_cid{ get; set; }

		/**类目名称 */
		[XmlElement("name")]
			public string  Name{ get; set; }

		/**该类目是否为父类目(即：该类目是否还有子类目) */
		[XmlElement("is_parent")]
			public bool  Is_parent{ get; set; }

		/**状态。可选值:normal(正常),deleted(删除) */
		[XmlElement("status")]
			public string  Status{ get; set; }

		/** 	排列序号，表示同级类目的展现次序，如数值相等则按名称次序排列。取值范围:大于零的整数 */
		[XmlElement("sort_order")]
			public long?  Sort_order{ get; set; }

	}
}
