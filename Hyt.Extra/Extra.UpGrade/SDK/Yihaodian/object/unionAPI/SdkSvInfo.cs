using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.UnionAPI
{
	/// <summary>
	/// Smart View资源信息
	/// </summary>
	[Serializable]
	public class SdkSvInfo 
	{
		/**推广关键字 */
		[XmlElement("key_word")]
			public string  Key_word{ get; set; }

		/**广告详情链接 */
		[XmlElement("ad_detail_url")]
			public string  Ad_detail_url{ get; set; }

	}
}
