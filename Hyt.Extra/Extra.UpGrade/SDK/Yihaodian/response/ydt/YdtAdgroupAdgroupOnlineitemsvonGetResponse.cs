using System;
using System.Xml.Serialization;
using Extra.UpGrade.SDK.Yihaodian.Object;
using Extra.UpGrade.SDK.Yihaodian.Object.Ydt;


namespace Extra.UpGrade.SDK.Yihaodian.Response
{
	/// <summary>
	/// 获取用户上架在线销售的全部宝贝
	/// </summary>
	public class YdtAdgroupAdgroupOnlineitemsvonGetResponse 
		: YhdResponse 
	{
		/**返回的每页数据量大小,默认200最大200
支持的最大列表长度为：200 */
		[XmlElement("page_size")]
			public int?  Page_size{ get; set; }

		/**返回的第几页数据，默认为1 从1开始，最大50 */
		[XmlElement("page_no")]
			public int?  Page_no{ get; set; }

		/**商品列表 */
		[XmlElement("subway_item_list")]
		public SubwayItemList  Subway_item_list{ get; set; }

		/**错误码 */
		[XmlElement("error_code")]
			public string  Error_code{ get; set; }

		/**错误描述 */
		[XmlElement("msg")]
			public string  Msg{ get; set; }

		/**子错误码 */
		[XmlElement("sub_code")]
			public string  Sub_code{ get; set; }

		/**子错误描述 */
		[XmlElement("sub_msg")]
			public string  Sub_msg{ get; set; }

	}
}
