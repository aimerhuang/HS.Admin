using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.Ydt
{
	/// <summary>
	/// 广告组分页
	/// </summary>
	[Serializable]
	public class AdGroupPage 
	{
		/**每页数据大小 */
		[XmlElement("page_size")]
			public int?  Page_size{ get; set; }

		/**返回第几页的数据 */
		[XmlElement("page_no")]
			public int?  Page_no{ get; set; }

		/**所查询的数据总数 */
		[XmlElement("total_item")]
			public int?  Total_item{ get; set; }

		/**广告组对象数组 */
		[XmlElement("adgroup_list")]
		public AdGroupList  Adgroup_list{ get; set; }

	}
}
