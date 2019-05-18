using System;
using System.Xml.Serialization;

namespace Extra.UpGrade.SDK.Yihaodian.Object.GroupUserAPI
{
	/// <summary>
	/// 区域商家基础信息
	/// </summary>
	[Serializable]
	public class MerchantVO 
	{
		/**区域商家ID */
		[XmlElement("id")]
			public long?  Id{ get; set; }

		/**商家名称 */
		[XmlElement("merchantName")]
			public string  MerchantName{ get; set; }

		/**商家编码 */
		[XmlElement("merchantCode")]
			public string  MerchantCode{ get; set; }

		/**所属集团商家ID */
		[XmlElement("merchantGroupId")]
			public long?  MerchantGroupId{ get; set; }

		/**0 普通商家，1 集团商家，2区域商家 */
		[XmlElement("groupLevel")]
			public int?  GroupLevel{ get; set; }

		/**创建时间 */
		[XmlElement("creatTime")]
			public string  CreatTime{ get; set; }

		/**最后修改时间 */
		[XmlElement("updateTime")]
			public string  UpdateTime{ get; set; }

	}
}
