using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.UnionAPI
{
	/// <summary>
	/// Smart Ad资源信息
	/// </summary>
	[Serializable]
	public class SdkAdInfo 
	{
		/**广告图片链接 */
		[XmlElement("ad_img_url")]
			public string  Ad_img_url{ get; set; }

		/**广告详情链接 */
		[XmlElement("ad_detail_url")]
			public string  Ad_detail_url{ get; set; }

	}
}
