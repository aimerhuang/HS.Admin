using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.UnionAPI
{
	/// <summary>
	/// 商家活动
	/// </summary>
	[Serializable]
	public class BusinessActivityInfoOuter 
	{
		/**活动ID */
		[XmlElement("id")]
			public long?  Id{ get; set; }

		/**活动名称 */
		[XmlElement("name")]
			public string  Name{ get; set; }

		/**活动URL, PC端 */
		[XmlElement("pc_url")]
			public string  Pc_url{ get; set; }

		/**活动URL, 无线 */
		[XmlElement("m_url")]
			public string  M_url{ get; set; }

		/**活动链接图片URL, PC端 */
		[XmlElement("pc_img_url")]
			public string  Pc_img_url{ get; set; }

		/**活动链接图片URL, 无线 */
		[XmlElement("wf_img_url")]
			public string  Wf_img_url{ get; set; }

		/**活动开始时间 */
		[XmlElement("start_date")]
			public string  Start_date{ get; set; }

		/**活动结束时间 */
		[XmlElement("end_date")]
			public string  End_date{ get; set; }

		/**活动描述 */
		[XmlElement("description")]
			public string  Description{ get; set; }

	}
}
